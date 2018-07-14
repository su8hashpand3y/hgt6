using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HGT6.Models;
using HGT6.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using Amazon;
using Amazon.S3;
using System.Threading.Tasks;
using Amazon.S3.Transfer;
using System.IO;
using HGT6.Helpers;

namespace HGT6.Controllers
{
  
  [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private const string bucketName = "hgtdata";
        // Specify your bucket region (an example region is shown).
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.APSoutheast1;
        private IAmazonS3 s3Client;
        private IConfiguration Configuration { get; }
        private IServiceProvider services { get; }
        private IEmailSender emailSender { get; }
        private ILogger logger { get; }

        public LoginController(IAmazonS3 s3Client, IConfiguration configuration, IServiceProvider services, IEmailSender emailSender)
        {
            Configuration = configuration;
            this.services = services;
            this.s3Client = s3Client;
            this.emailSender = emailSender;
            this.logger = logger;
        }

        private async Task<bool> UploadFileAsync(byte[] file, string keyName)
        {
            bool success = false;
            try
            {
                var fileTransferUtility = new TransferUtility(this.s3Client);
                Stream stream = new MemoryStream(file);
                // Option 4. Specify advanced settings.
                var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                {
                    BucketName = bucketName,
                    InputStream = stream,
                    StorageClass = S3StorageClass.Standard,
                    Key = keyName,
                    CannedACL = S3CannedACL.PublicRead
                };


                await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);
                success = true;
            }
            catch (AmazonS3Exception e)
            {
                this.logger.Log("Login  UploadFile Aysnc Amazon has Problem", e.Message, e.InnerException?.Message);

                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                this.logger.Log("Login  UploadFile Aysnc has Problem", e.Message, e.InnerException?.Message);
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }

            return success;
        }

        [HttpGet("[action]")]
        public  IActionResult GetCaptha()
        {
            try
            {
                var context = this.services.GetService(typeof(HGTDbContext)) as HGTDbContext;
                Random r = new Random();
                var randomId = r.Next(1,context.Capthas.Count()+1);
               var captha = context.Capthas.Single(x=>x.Id == randomId);
              return Ok(new ServiceTypedResponse<CapthaResponse>() {Status ="ok" ,Message = new CapthaResponse { CapthaId =captha.Id,CapthaText =captha.CapthaText}});
    
            }
            catch (System.Exception e)
            {
                this.logger.Log("Login Captha Generation has Problem", e.Message, e.InnerException?.Message);
                return Ok(new ServiceTypedResponse<CapthaResponse>{Status = "error"});
            }
        }



        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var context = this.services.GetService(typeof(HGTDbContext)) as HGTDbContext;
                    if (context.HGTUsers.FirstOrDefault(x => x.Email.Equals(model.Email, StringComparison.InvariantCultureIgnoreCase)) == null)
                    {
                        HGTUser newUser = new HGTUser
                        {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            Gender = model.Gender,
                            District = model.District,
                            Town = model.Town,
                            Age = model.Age,
                        };

                        if (!String.IsNullOrEmpty(model.AvatarImage))
                        {
                            var startIndex = model.AvatarImage.IndexOf("base64,");
                            var base64Image = model.AvatarImage.Substring(startIndex + 7);
                            var index1 = model.AvatarImage.IndexOf('/');
                            var index2 = model.AvatarImage.IndexOf(';');
                            var ext = model.AvatarImage.Substring(index1+1, index2 - index1-1);
                            model.Email = model.Email.Replace('.', '_');
                            string path = $"{model.Email}.{ext}";
                            if (!await UploadFileAsync(Convert.FromBase64String(base64Image), path))
                            {
                                return Ok(new ServiceResponse { Status = "error", Message = "Service Not Up" });
                            }
                            newUser.AvatarImage = $"https://s3.ap-south-1.amazonaws.com/{bucketName}/{path}";
                        }

                        if(context.Capthas.Single(x => x.Id == model.CapthaId).CapthaAnswer != model.Captha)
                        {
                            return Ok(new ServiceResponse { Status = "error", Message = "Captha Answer not Matched" });
                        }

                        var hasher = new PasswordHasher<HGTUser>();
                        var hashedPassword = hasher.HashPassword(newUser,model.Password);
                        newUser.PasswordHash = hashedPassword;
                        context.HGTUsers.Add(newUser);
                        context.SaveChanges();
                        // Send the verification Mail
                        // Login the User and send a token back
                        return this.LoginUser(new LoginViewModel { Email= model.Email,Password= model.Password});
                    }
                    else
                    {
                        return Ok(new ServiceResponse { Status = "error", Message = "User Already Exists" });
                    }
                }
                catch(Exception e)
                {
                    this.logger.Log("Register has Problem", e.Message, e.InnerException?.Message);
                    return Ok(new ServiceResponse { Status = "error", Message = "Something Went Wrong" });
                }
            }
            else
            {
                var modelErrors = new StringBuilder();
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        modelErrors.AppendLine(modelError.ErrorMessage);
                    }
                }

                 return Ok(new ServiceResponse{ Status ="error",Message = modelErrors.ToString()});
            }
        }


        [AllowAnonymous]
        [HttpPost("[action]")]
        public IActionResult Login([FromBody]LoginViewModel user)
        {
            return LoginUser(user);
        }

        private IActionResult LoginUser(LoginViewModel user)
        {
            bool succeeded = false;
            var context = this.services.GetService(typeof(HGTDbContext)) as HGTDbContext;
            var foundUser = context.HGTUsers.FirstOrDefault(x => x.Email.Equals(user.Email, StringComparison.InvariantCultureIgnoreCase));
            if (foundUser == null)
                return Ok(new ServiceResponse { Status = "error", Message = "User not found!" });

            if (!foundUser.IsVerified)
            {
                // Send The verification Mail
            }

            var hash = new PasswordHasher<HGTUser>();

            if (hash.VerifyHashedPassword(foundUser,foundUser.PasswordHash, user.Password) ==  PasswordVerificationResult.Success)
                succeeded = true;
            else return Ok(new ServiceResponse { Status = "error", Message = "Wrong Passwords!" });

            if (succeeded)
            {
                var claims = new[]
                {
                              new Claim(ClaimTypes.Email, user.Email),
                              new Claim(ClaimTypes.NameIdentifier, foundUser.Id)
                        };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecurityKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: Configuration["ValidIssuer"],
                    audience: Configuration["ValidAudience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                return Ok(new ServiceResponse { Status = "registerd", Message = new JwtSecurityTokenHandler().WriteToken(token) });
            }
            else
            {
                return Ok(new ServiceResponse { Status = "error", Message = "Could not verify username and password!" });
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(MainController), "Main");
            }
        }

        public class PassReset
        {
            public string Email { get; set; }
            public string Code { get; set; }
            public string Password { get; set; }
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public IActionResult PasswordResetEmail([FromBody]PassReset passReset)
        {
            var context = this.services.GetService(typeof(HGTDbContext)) as HGTDbContext;
            var foundUser = context.HGTUsers.FirstOrDefault(x => x.Email.Equals(passReset.Email, StringComparison.InvariantCultureIgnoreCase));
            if (foundUser == null)
                return Ok(new ServiceResponse { Status = "error", Message = "User not found!" });

            var rand = new Random();
            if(DateTime.Now -  foundUser.LastPassowrdResetTime < TimeSpan.FromHours(5))
            {
                return Ok(new ServiceResponse { Status = "error", Message = "You Have to wait 5 hours before requesting new password reset code since your last request" });
            }
            if(foundUser.VerificationCode != "Not available")
            {
                return Ok(new ServiceResponse { Status = "error", Message = "Code Already sent to your email." });
            }

            foundUser.VerificationCode = rand.Next(1000, 9999).ToString();
            context.SaveChanges();
            if (this.emailSender.SendMail(passReset.Email, "Code To Reset Your Password", $"The code to reset your password is {foundUser.VerificationCode}"))
            {
                return Ok(new ServiceResponse { Status = "good", Message = "We have mailed you code to reset your password" });
            }

            return Ok(new ServiceResponse { Status = "error", Message = "Error in sending mail" });
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public IActionResult PasswordReset([FromBody]PassReset passReset)
        {
            var context = this.services.GetService(typeof(HGTDbContext)) as HGTDbContext;
            var foundUser = context.HGTUsers.FirstOrDefault(x => x.Email.Equals(passReset.Email, StringComparison.InvariantCultureIgnoreCase));
            if (foundUser == null)
                return Ok(new ServiceResponse { Status = "error", Message = "User not found!" });

            if (foundUser.VerificationCode != passReset.Code)
            {
                return Ok(new ServiceResponse { Status = "error", Message = "Wrong Code" });
            }

            foundUser.VerificationCode = "Not available";
            foundUser.LastPassowrdResetTime = DateTime.Now;

            var hasher = new PasswordHasher<HGTUser>();
            var hashedPassword = hasher.HashPassword(foundUser, passReset.Password);
            foundUser.PasswordHash = hashedPassword;

            context.SaveChanges();
            return Ok(new ServiceResponse { Status = "good", Message = "Password Reset Successful" });
        }
    }
}