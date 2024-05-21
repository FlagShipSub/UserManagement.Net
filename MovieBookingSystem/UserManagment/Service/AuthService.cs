using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using UserManagment.Data;
using UserManagment.Models;
using UserManagment.Models.Dtos;
using UserManagment.Service.IService;

namespace UserManagment.Service
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IJwtTokenGenerator _jwtTokenGenerator;


        public AuthService(ApplicationDbContext db, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<Response<string>> GenerateOtp(VerificationRequest requestDto)
        {
            var user = await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.Email.ToUpper() == requestDto.Email.ToUpper() || u.UserName.ToUpper() == requestDto.UserName);
            if (user != null)
            {

                var verificationdata = await _db.OtpValidations.FirstOrDefaultAsync(o => o.Id == user.Id);
                if (verificationdata == null)
                {
                    var otp = new Random().Next(100000, 1000000);
                    var otpValidation = new OtpValidation(otp.ToString(), DateTime.Now, DateTime.Today, DateTime.Now.AddMinutes(2), 3, user.Id);
                    _db.OtpValidations.Add(otpValidation);
                    _db.SaveChanges();
                    return new Response<string>("Successfull", StatusCodes.Status200OK, "Otp Sent Successfully", "");
                }

                if (verificationdata != null && verificationdata.TrailCout > 0)
                {
                    GenerateRandomNumber(user, verificationdata);
                }
                else
                {
                    if (verificationdata.ExpiryDateTime < DateTime.Now)
                    {
                        verificationdata.TrailCout = 4;
                        GenerateRandomNumber(user, verificationdata);
                    }
                    else
                    {
                        return new Response<string>("UnSuccessful", StatusCodes.Status406NotAcceptable, "", "Otp Time limit reached");
                    }

                }

                return new Response<string>("Successful", StatusCodes.Status200OK, "Opt sent Succefully", "");
            }

            return new Response<string>("UnSuccessful", StatusCodes.Status404NotFound, "User not found", "Opt failed to send");
        }

        private void GenerateRandomNumber(ApplicationUser user, OtpValidation verificationdata)
        {
            var otp = new Random().Next(100000, 1000000);
            MapVerificationData(user, verificationdata, otp, verificationdata.TrailCout - 1);
            _db.OtpValidations.Update(verificationdata);
            _db.SaveChanges();
        }

        private static void MapVerificationData(ApplicationUser user, OtpValidation verificationdata, int otp, int trailcount)
        {
            verificationdata.Otp = otp.ToString();
            verificationdata.TrailCout = trailcount;
            verificationdata.CreatedDateTime = DateTime.Now;
            verificationdata.UpdatedDateTime = DateTime.Today;
            verificationdata.ExpiryDateTime = DateTime.Now.AddMinutes(2);
            verificationdata.Id = user.Id;
            verificationdata.ApplicationUser = user;
        }

        public async Task<Response<object>> GetAllUsers()
        {
            var user = await _db.ApplicationUsers.ToListAsync();
            return new Response<object>("Successful", StatusCodes.Status200OK, user, "");
        }

        public async Task<Response<LoginResponseDto>> Login(LoginRequestDto requestDto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == requestDto.UserName.ToLower());

            bool isUserMatched = await _userManager.CheckPasswordAsync(user, requestDto.Password);
            if (user == null || isUserMatched == false)
            {
                return new Response<LoginResponseDto>()
                {
                    Code = StatusCodes.Status404NotFound,
                    Data = new LoginResponseDto() { User = new UserDto(), Token = "" },
                    Error = "User Not Found",
                    Message = ""
                };
            }
            var token = _jwtTokenGenerator.GeneraterToken(user);
            var loggedInUser = new UserDto()
            {
                ID = user.Id,
                Email = user.Email,
                UserName = user.UserName

            };
            return new Response<LoginResponseDto>()
            {
                Code = StatusCodes.Status200OK,
                Data = new LoginResponseDto() { User = loggedInUser, Token = token }

            };
        }

        public async Task<Response<object>> Register(RegisterationRequestDto requestDto)
        {
            ApplicationUser user = new ApplicationUser()
            {
                UserName = requestDto.UserName,
                Email = requestDto.Email,
                NormalizedEmail = requestDto.Email.ToUpper(),
                City = requestDto.City,
                IsRemoved = false,
                PhoneNumber = requestDto.PhoneNumber,
                State = requestDto.State,
                Country = requestDto.Country,
                Gender = requestDto.Gender,
            };
            try
            {
                var result = await _userManager.CreateAsync(user, requestDto.Password);
                if (result.Succeeded)
                {
                    var userToReturn = _db.ApplicationUsers.First(u => u.Email == requestDto.Email);

                    if (userToReturn != null)
                    {
                        UserDto userDto = new UserDto()
                        {
                            Email = userToReturn.Email ?? "",
                            ID = userToReturn.Id,
                            UserName = userToReturn.UserName ?? ""

                        };
                        return new Response<object>("Successful", StatusCodes.Status200OK, userDto, "");
                    }
                    return new Response<object>("UnSuccessful", StatusCodes.Status406NotAcceptable, "", result.Errors.FirstOrDefault().Description);

                }
            }
            catch (Exception ex)
            {


            }
            return new Response<object>("UnKnown Error Occured", StatusCodes.Status400BadRequest, "", "");

        }
    }
}
