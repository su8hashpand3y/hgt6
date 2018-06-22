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

namespace HGT6.Controllers
{
[Route("api/[controller]")]
    public class UploadController : Controller
    {
        private const string bucketName = "hgtdata";
        private string publicadress = "https://s3.ap-south-1.amazonaws.com/hgtdata/user1/SampleVideo_1280x720_30mb.mp4";
        // Specify your bucket region (an example region is shown).
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.APSoutheast1;
        private IAmazonS3 s3Client;
        private IConfiguration Configuration { get; }


        private IServiceProvider services { get; }
        private readonly IHostingEnvironment hostingEnvironment;
        public UploadController(IAmazonS3 s3Client,IServiceProvider services, IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            this.s3Client = s3Client;
            this.services = services;
            Configuration = configuration;
            this.hostingEnvironment = hostingEnvironment;
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> UploadFileToStore(IFormFile file)
        {
            string userEmail = HttpContext.GetUserEmail();
            if (!String.IsNullOrEmpty(userEmail))
            {
                try
                {
                    var context = this.services.GetService(typeof(HGTDbContext)) as HGTDbContext;
                    var user = context.HGTUsers.FirstOrDefault(x => x.Email == userEmail);
                    if (user != null)
                    {
                        long size = file.Length;
                        String ext = System.IO.Path.GetExtension(file.FileName);
                        // Very immportant to check format here other anyone will upload anything
                        var uniqueID = CreateUniqueVideoID();
                        var fileAddress = $"{user.Id}_{ uniqueID}{ext}";
                        return await  UploadFileAsync(file, fileAddress);
                    }
                }
                catch
                {
                    return Ok(new ServiceResponse { Status = "error", Message = "Something Went Wrong When Uploading The File" });
                }
            }

            return Ok(new ServiceResponse { Status = "error", Message = "Something Went Wrong When Uploading The File" });
        }

        [HttpPost("[action]")]
        [Authorize]
        // [RequestSizeLimit(1073741823)]
        public async Task<IActionResult> Upload(UploadInfoViewModel fileInfo)
        {
            string userId = HttpContext.GetUserID();
            if (!String.IsNullOrEmpty(userId))
            {
                try
                {
                        var context = this.services.GetService(typeof(HGTDbContext)) as HGTDbContext;
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
                        var base64Image = fileInfo.PosterUrl.Substring(startIndex + 6);
                        var index1 = fileInfo.PosterUrl.IndexOf('/');
                        var index2 = fileInfo.PosterUrl.IndexOf(';');
                        var ext = fileInfo.PosterUrl.Substring(index1, index2 - index1);
                        string path = $"{fileInfo}.{ext}";
                        await this.UploadFileAsync(Convert.FromBase64String(base64Image), path);
                        videoInfo.PosterUrl  = path;
                    }
                        context.Videos.Add(videoInfo);
                        await context.SaveChangesAsync();
                        return Ok(new ServiceResponse { Status = "success", Message = videoInfo.ID.ToString() });
                }
                catch
                {
                    return Ok(new ServiceResponse { Status = "error", Message = "Something Went Wrong When Uploading The File" });
                }
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
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }

        }
        private  async Task<IActionResult> UploadFileAsync(IFormFile file,string keyName)
        {
            try
            {
                var fileTransferUtility = new TransferUtility(this.s3Client);
           
                var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                {
                    BucketName = bucketName,
                    InputStream = file.OpenReadStream(),
                    StorageClass = S3StorageClass.Standard,
                    //PartSize = 6291456, // 6 MB.
                    Key = keyName,
                    CannedACL = S3CannedACL.PublicRead
                };


                await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);
                return Ok(new ServiceResponse { Status = "success", Message = keyName });
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
                return Ok(new ServiceResponse { Status = "error", Message = "Something Went Wrong When Uploading The File" });
            }

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