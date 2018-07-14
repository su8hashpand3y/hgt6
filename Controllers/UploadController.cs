using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using HGT6.Models;
using HGT6.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using HGT6.Helpers;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Amazon.Runtime;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using Microsoft.Net.Http.Headers;

namespace HGT6.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [DisableRequestSizeLimit]
    public class UploadController : Controller
    {
        private const string bucketName = "hgtdata";
        // Specify your bucket region (an example region is shown).
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.APSoutheast1;
        private IAmazonS3 s3Client;
        private IConfiguration Configuration { get; }

        private ILogger logger;
        private IServiceProvider services { get; }
        private readonly IHostingEnvironment hostingEnvironment;
        public UploadController(IAmazonS3 s3Client,IServiceProvider services, IHostingEnvironment hostingEnvironment, IConfiguration configuration,ILogger logger)
        {
            this.s3Client = s3Client;
            this.services = services;
            Configuration = configuration;
            this.hostingEnvironment = hostingEnvironment;
            this.logger = logger;
        }

        //[HttpPost("[action]")]
        //// [Authorize]
        //// [RequestSizeLimit(1048576 * 500)]
        //[DisableRequestSizeLimit]
        //public async Task<IActionResult> Upload33()
        //{
        //    var t = Request.ReadFormAsync();
        //    t.form
        //    if (!Request.ReadFormAsync().Content.IsMimeMultipartContent())
        //    {
        //        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        //    }
        //}

        public async Task<IActionResult> UploadMultipartUsingReader(string fileAddress)
        {
            var boundary = GetBoundary(Request.ContentType);
            var reader = new MultipartReader(boundary, Request.Body, 80 * 1024);

            var valuesByKey = new Dictionary<string, string>();
            MultipartSection section;

            while ((section = await reader.ReadNextSectionAsync()) != null)
            {
                var contentDispo = section.GetContentDispositionHeader();

                if (contentDispo.IsFileDisposition())
                {
                    var fileSection = section.AsFileSection();
                    var bufferSize = 32 * 1024;
                    await ReadStream(fileSection.FileStream, bufferSize, fileAddress);
                }
                else if (contentDispo.IsFormDisposition())
                {
                    var formSection = section.AsFormDataSection();
                    var value = await formSection.GetValueAsync();
                    valuesByKey.Add(formSection.Name, value);
                }
            }

            return Ok();
        }

        public async Task<int> ReadStream(Stream stream, int bufferSize,string fileAddress)
        {
            var buffer = new byte[bufferSize];
            FileStream fs = new FileStream(Path.Combine(this.hostingEnvironment.ContentRootPath,fileAddress), FileMode.CreateNew);
            int bytesRead;
            int totalBytes = 0;

            do
            {
                bytesRead = await stream.ReadAsync(buffer, 0, bufferSize);
                fs.Write(buffer, 0, bytesRead);
                totalBytes += bytesRead;
            } while (bytesRead > 0);

            fs.Close();
            return totalBytes;
        }

        private static string GetBoundary(string contentType)
        {
            if (contentType == null)
                throw new ArgumentNullException(nameof(contentType));

            var elements = contentType.Split(' ');
            var element = elements.First(entry => entry.StartsWith("boundary="));
            var boundary = element.Substring("boundary=".Length);

            boundary = HeaderUtilities.RemoveQuotes(boundary).Value;

            return boundary;
        }

        [HttpPost("[action]")]
        [Authorize]
        // [RequestSizeLimit(1048576 * 500)]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFileToStore()
        {
            string userEmail = HttpContext.GetUserEmail();
            if (!String.IsNullOrEmpty(userEmail))
            {
                try
                {
                    var context = this.services.GetService(typeof(HGTDbContext)) as HGTDbContext;
                    var user = context.HGTUsers.FirstOrDefault(x => x.Email.Equals(userEmail, StringComparison.InvariantCultureIgnoreCase));
                    if (user != null)
                    {
                        String ext = Request.Query["ext"]; 
                        // Very immportant to check format here other anyone will upload anything
                        var uniqueID = CreateUniqueVideoID();
                        var fileAddress = $"{user.Id}_{ uniqueID}.{ext}";
                        await this.UploadMultipartUsingReader(fileAddress);
                        TempVideo tv = new TempVideo { Path= $"https://s3.ap-south-1.amazonaws.com/{bucketName}/{fileAddress}" };
                        context.TempVideo.Add(tv);
                        context.SaveChanges();
                        return await UploadByteAsync(fileAddress);
                    }
                }
                catch(Exception e)
                {
                    this.logger.Log("UploadFileToStore has Problem", e.Message, e.InnerException?.Message);
                    return Ok(new ServiceResponse { Status = "error", Message = "Something Went Wrong When Uploading The File" });
                }
            }

            return Ok(new ServiceResponse { Status = "error", Message = "User Not Logged in to Upload" });
        }

        [HttpPost("[action]")]
        [Authorize]
        // [RequestSizeLimit(1073741823)]
        public async Task<IActionResult> Upload([FromBody]UploadInfoViewModel fileInfo)
        {
            string userId = HttpContext.GetUserID();
            if (!String.IsNullOrEmpty(userId))
            {
                try
                {
                    var context = this.services.GetService(typeof(HGTDbContext)) as HGTDbContext;

                    if (context.Capthas.Single(x => x.Id == fileInfo.CapthaId).CapthaAnswer != fileInfo.Captha)
                    {
                        return Ok(new ServiceResponse { Status = "error", Message = "Captha Answer not Matched" });
                    }

                    VideoInfo videoInfo = new VideoInfo
                    {
                        VideoUrl = fileInfo.VideoUrl,
                        UploadDateTime = DateTime.Now,
                        Title = fileInfo.Title,
                        Description = fileInfo.Description,
                        Category = fileInfo.Category,
                        HGTUserID = userId
                    };

                    if (!String.IsNullOrEmpty(fileInfo.PosterUrl))
                    {
                        var startIndex = fileInfo.PosterUrl.IndexOf("base64,");
                        var base64Image = fileInfo.PosterUrl.Substring(startIndex + 7);
                        var index1 = fileInfo.PosterUrl.IndexOf('/');
                        var index2 = fileInfo.PosterUrl.IndexOf(';');
                        var ext = fileInfo.PosterUrl.Substring(index1 + 1, index2 - index1 - 1);
                        var uniqueID = CreateUniqueVideoID();
                        var path = $"{userId}_{ uniqueID}.{ext}";
                        await this.UploadFileAsync(Convert.FromBase64String(base64Image), path);
                        videoInfo.PosterUrl = $"https://s3.ap-south-1.amazonaws.com/{bucketName}/{path}";
                    }
                    context.Videos.Add(videoInfo);
                    var tv = context.TempVideo.FirstOrDefault(x => x.Path == fileInfo.VideoUrl);
                    if (tv != null)
                    {
                        context.TempVideo.Remove(tv);
                    }

                    await context.SaveChangesAsync();
                    return Ok(new ServiceResponse { Status = "success", Message = videoInfo.ID.ToString() });
                }
                catch(Exception e)
                {
                    this.logger.Log("Upload has Problem", e.Message, e.InnerException?.Message);
                    return Ok(new ServiceResponse { Status = "error", Message = "Something Went Wrong When Uploading The File" });
                }
            }

            return Ok(new ServiceResponse { Status = "error", Message = "Something Went Wrong When Uploading The File" });
        }


        private async Task<IActionResult> UploadByteAsync(string keyName)
        {
            try
            {
                string path = $"{this.hostingEnvironment.ContentRootPath}/{keyName}";
                FileStream fs = new FileStream(path, FileMode.Open);
                var fileTransferUtility = new TransferUtility(this.s3Client);
                var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                {
                    BucketName = bucketName,
                    InputStream = fs,
                    StorageClass = S3StorageClass.Standard,
                    Key = keyName,
                    CannedACL = S3CannedACL.PublicRead
                };


                await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);
                fs.Close();
                System.IO.File.Delete(path);
                return Ok(new ServiceResponse { Status = "success", Message = $"https://s3.ap-south-1.amazonaws.com/{bucketName}/{keyName}" });
            }
            catch (AmazonS3Exception e)
            {
                this.logger.Log("UploadByteAsync Amazon has Problem", e.Message, e.InnerException?.Message);
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                this.logger.Log("UploadByteAsync has Problem", e.Message, e.InnerException?.Message);
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }

            return Ok(new ServiceResponse { Status = "error", Message = "Something Went Wrong When Uploading The File" });
        }

        private async Task UploadFileAsync(byte[] file, string keyName)
        {
            try
            {
                var fileTransferUtility = new TransferUtility(this.s3Client);
                Stream stream = new MemoryStream(file);
                var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                {
                    BucketName = bucketName,
                    InputStream = stream,
                    StorageClass = S3StorageClass.Standard,
                    Key = keyName,
                    CannedACL = S3CannedACL.PublicRead
                };


                await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);
            }
            catch (AmazonS3Exception e)
            {
                this.logger.Log("UploadFileAsync Amazon has Problem", e.Message, e.InnerException?.Message);

                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                this.logger.Log("UploadFileAsync has Problem", e.Message, e.InnerException?.Message);

                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }

        }
        //private  async Task<IActionResult> UploadFileAsync(IFormFile file,string keyName)
        //{
        //    try
        //    {
        //        var fileTransferUtility = new TransferUtility(this.s3Client);
           
        //        var fileTransferUtilityRequest = new TransferUtilityUploadRequest
        //        {
        //            BucketName = bucketName,
        //            InputStream = file.OpenReadStream(),
        //            StorageClass = S3StorageClass.Standard,
        //            //PartSize = 6291456, // 6 MB.
        //            Key = keyName,
        //            CannedACL = S3CannedACL.PublicRead
        //        };

        //        //fileTransferUtilityRequest.UploadProgressEvent += FileTransferUtilityRequest_UploadProgressEvent;


        //        await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);
        //        return Ok(new ServiceResponse { Status = "success", Message = $"https://s3.ap-south-1.amazonaws.com/{bucketName}/{keyName}" });
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
        //        return Ok(new ServiceResponse { Status = "error", Message = "Something Went Wrong When Uploading The File" });
        //    }

        //}

        private void FileTransferUtilityRequest_UploadProgressEvent(object sender, UploadProgressArgs e)
        {
            HttpContext.Response.WriteAsync(e.PercentDone.ToString());
        }

        private string CreateUniqueVideoID()
        {
            return DateTime.Now.Year.ToString()
                + DateTime.Now.Month
                + DateTime.Now.Day
                + DateTime.Now.Hour
                + DateTime.Now.Minute
                + DateTime.Now.Second;
        }
    }
}