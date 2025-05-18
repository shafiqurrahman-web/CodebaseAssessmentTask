using Microsoft.AspNetCore.Mvc;
using CodebaseAssessmentTask.Models;
using System;
using Newtonsoft.Json;
using CodebaseAssessmentTask.Data;
using CodebaseAssessmentTask.Data.DbModels;
using Microsoft.EntityFrameworkCore;

namespace CodebaseAssessmentTask.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly AppDbContext _dbContext;

        public RegistrationController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult Start() => View("LoginorRegister");

        [HttpPost]
        public async Task<IActionResult> Start(string icNumber)
        {
            if (string.IsNullOrEmpty(icNumber))
            {
                ModelState.AddModelError("ICNumber", "Please enter your IC Number.");
                return View("LoginorRegister");
            }

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.ICNumber == icNumber);

            if (user == null)
            {
                ModelState.AddModelError("ICNumber", "Account not found. Please register yourself by clicking Register now below.");
                return View("LoginorRegister");
            }

             TempData["CurrentICNumber"] = user.ICNumber;

            return RedirectToAction("VerifyOtp", new { icNumber = user.ICNumber });
        }

        [HttpGet]
        public IActionResult AccountCreationInstruction() => View();

        [HttpGet]
        public IActionResult UserExistCheck(RegistrationCreateAccountViewModel userModel)
        {
            return View(userModel);
        }

        [HttpPost]
        public IActionResult HandleUserExistChoice(string choice, string icNumber)
        {
            if (choice == "login")
            {
                return RedirectToAction("VerifyOtp", new { icNumber = icNumber });
            }
            else 
            {
                return RedirectToAction("CreateAccount");
            }
        }

        [HttpGet]
        public IActionResult VerifyOtp(string icNumber)
        {
            RegistrationCreateAccountViewModel fullModel = null;
            if (TempData[$"User_{icNumber}_FullModel"] != null)
            {
                fullModel = JsonConvert.DeserializeObject<RegistrationCreateAccountViewModel>(TempData[$"User_{icNumber}_FullModel"].ToString());
                TempData.Keep($"User_{icNumber}_FullModel");
            }

            var model = new RegistrationVerifyOtpViewModel
            {
                ICNumber = icNumber,
                PhoneNumber = fullModel?.MobileNumber, 
                Email = fullModel?.EmailAddress,
                Otp = string.Empty,
                IsEmailOtp = false
            };
                        
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("OTP_" + icNumber) ))
            {
                var random = new Random();
                string otp = random.Next(1000, 9999).ToString();
                HttpContext.Session.SetString("OTP_" + model.ICNumber, otp);
            }

            ViewBag.Otp = HttpContext.Session.GetString("OTP_" + icNumber);
            ViewBag.FullModel = fullModel;
            return View(model);
        }

        [HttpPost]
        public IActionResult VerifyOtp(RegistrationVerifyOtpViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var sessionOtp = HttpContext.Session.GetString("OTP_" + model.ICNumber);
            if (sessionOtp == null || model.Otp != sessionOtp)
            {
                ModelState.AddModelError("Otp", "Invalid OTP.");
                return View(model);
            }
            HttpContext.Session.Remove("OTP_" + model.ICNumber);
            var email = TempData[$"User_{model.ICNumber}_EmailAddress"]?.ToString();
            return RedirectToAction("VerifyEmailOtp", new { icNumber = model.ICNumber, email });
        }

        [HttpGet]
        public IActionResult VerifyEmailOtp(string icNumber, string email)
        {
            var random = new Random();
            string otp = random.Next(1000, 9999).ToString();
            HttpContext.Session.SetString("OTP_EMAIL_" + icNumber, otp);
            var model = new RegistrationVerifyOtpViewModel { ICNumber = icNumber, Email = email, IsEmailOtp = true };
            ViewBag.Otp = otp; // For testing
            return View("VerifyOtp", model);
        }

        [HttpPost]
        public IActionResult VerifyEmailOtp(RegistrationVerifyOtpViewModel model)
        {
            if (!ModelState.IsValid)
                return View("VerifyOtp", model);
            var sessionOtp = HttpContext.Session.GetString("OTP_EMAIL_" + model.ICNumber);
            if (sessionOtp == null || model.Otp != sessionOtp)
            {
                ModelState.AddModelError("Otp", "Invalid OTP.");
                return View("VerifyOtp", model);
            }
            HttpContext.Session.Remove("OTP_EMAIL_" + model.ICNumber);
            return RedirectToAction("PrivacyPolicy");
        }

        [HttpGet]
        public IActionResult PrivacyPolicy() => View();

        [HttpPost]
        public IActionResult PrivacyPolicy(RegistrationPrivacyPolicyViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            return RedirectToAction("CreatePin");
        }

        [HttpGet]
        public IActionResult CreatePin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreatePin(RegistrationCreatePinViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            TempData["InitialPin"] = model.Pin;
            return RedirectToAction("ConfirmPin");
        }

        [HttpGet]
        public IActionResult ConfirmPin()
        {
            if (TempData["InitialPin"] == null)
            {
                return RedirectToAction("CreatePin");
            }
            TempData.Keep("InitialPin");
            return View();
        }

        [HttpPost]
        public IActionResult ConfirmPin(RegistrationConfirmPinViewModel model)
        {
            var initialPin = TempData["InitialPin"]?.ToString();

            if (string.IsNullOrEmpty(initialPin) || !ModelState.IsValid)
            {
                TempData.Keep("InitialPin");
                return View(model);
            }

            if (model.ConfirmPin != initialPin)
            {
                ModelState.AddModelError("ConfirmPin", "PINs do not match.");
                TempData.Keep("InitialPin");
                return View(model);
            }
            return RedirectToAction("BiometricLogin");
        }
       
        [HttpGet]
        public IActionResult BiometricLogin() => View();

        [HttpGet]
        public IActionResult AccountCreationSuccess() => View();

        [HttpPost]
        public IActionResult HandleBiometricChoice(string choice)
        {
            var icNumber = TempData["CurrentICNumber"]?.ToString();
            RegistrationCreateAccountViewModel userData = null;

            if (!string.IsNullOrEmpty(icNumber) && TempData[$"User_{icNumber}_FullModel"] != null)
            {
                 userData = JsonConvert.DeserializeObject<RegistrationCreateAccountViewModel>(TempData[$"User_{icNumber}_FullModel"].ToString());
                 var initialPin = TempData["InitialPin"]?.ToString();

                var user = new User
                {
                    CustomerName = userData.CustomerName,
                    ICNumber = userData.ICNumber,
                    MobileNumber = userData.MobileNumber,
                    EmailAddress = userData.EmailAddress,
                    PasswordHash = initialPin 

                };

                User existingUser = _dbContext.Users.FirstOrDefault(u => u.ICNumber == userData.ICNumber);

                if (existingUser == null)
                {
                    _dbContext.Users.Add(user);
                    _dbContext.SaveChanges();
                }
                else
                {
                    existingUser.PasswordHash = initialPin;
                    _dbContext.Users.Update(existingUser);
                    _dbContext.SaveChanges();

                }   

TempData["CustomerName"] = userData.CustomerName;

                TempData.Remove("CurrentICNumber");
                TempData.Remove($"User_{icNumber}_FullModel");
                TempData.Remove("InitialPin"); // Clear initial PIN as well

            }

            return RedirectToAction("AccountCreationSuccess");
        }

        [HttpGet]
        public IActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateAccount(RegistrationCreateAccountViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existingUser = _dbContext.Users.FirstOrDefault(u => u.ICNumber == model.ICNumber);

            if (existingUser != null)
            {
                var existingUserModel = new RegistrationCreateAccountViewModel
                {
                    CustomerName = existingUser.CustomerName,
                    ICNumber = existingUser.ICNumber,
                    MobileNumber = existingUser.MobileNumber,
                    EmailAddress = existingUser.EmailAddress
                };
                return RedirectToAction("UserExistCheck", existingUserModel);
            }

            var random = new Random();
            string otp = random.Next(1000, 9999).ToString();
            HttpContext.Session.SetString("OTP_" + model.ICNumber, otp);
            TempData["MobileNumber"] = model.MobileNumber;
            TempData["TestOtp"] = otp;

            TempData[$"User_{model.ICNumber}_CustomerName"] = model.CustomerName;
            TempData[$"User_{model.ICNumber}_ICNumber"] = model.ICNumber;
            TempData[$"User_{model.ICNumber}_MobileNumber"] = model.MobileNumber;
            TempData[$"User_{model.ICNumber}_EmailAddress"] = model.EmailAddress;
            TempData["CurrentICNumber"] = model.ICNumber;
            TempData[$"User_{model.ICNumber}_FullModel"] = JsonConvert.SerializeObject(model);
            return RedirectToAction("VerifyOtp", new { icNumber = model.ICNumber, phoneNumber = model.MobileNumber });
        }

        [HttpGet]
        public JsonResult ResendOtp(string icNumber)
        {
            var random = new Random();
            string otp = random.Next(1000, 9999).ToString();
            HttpContext.Session.SetString("OTP_" + icNumber, otp);
            return Json(new { otp });
        }
    }
} 