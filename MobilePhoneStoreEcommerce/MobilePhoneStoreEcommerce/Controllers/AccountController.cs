using MobilePhoneStoreEcommerce.Core.Domain;
using MobilePhoneStoreEcommerce.Core.Dtos;
using MobilePhoneStoreEcommerce.Core.ViewModels;
using MobilePhoneStoreEcommerce.Models.ControllerModels;
using MobilePhoneStoreEcommerce.Models.ViewModels;
using MobilePhoneStoreEcommerce.Persistence;
using MobilePhoneStoreEcommerce.Persistence.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace MobilePhoneStoreEcommerce.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        private ApplicationDbContext _context;
        public AccountController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Login(int roleID = RoleIds.Unknown)
        {
            var loginViewModel = new LoginViewModel() { AccountDto = new AccountDto(), RoleID = roleID };
            return View(loginViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Login", loginViewModel);
            }

            var acc = loginViewModel.AccountDto;

            var result = Login(acc.Username, acc.Password);

            if (result == 0) // Invalid user name or password
                return View(loginViewModel);

            var accInDb = _context.Accounts.SingleOrDefault(a => a.UserName == acc.Username);

            if (accInDb == null)
                throw new Exception("Not found");

            if (result == RoleIds.Admin)
            {
                Session[SessionNames.AdminID] = accInDb.ID;
                return RedirectToAction("Index", "Admin");
            }
            else if (result == RoleIds.Seller)
            {
                Session[SessionNames.SellerID] = accInDb.ID;
                return RedirectToAction("Index", "Seller", new { sellerID = Session[SessionNames.SellerID] });
            }
            else if (result == RoleIds.Customer)
            {
                Session[SessionNames.CustomerID] = accInDb.ID;
                return RedirectToAction("Index", "HomeScreen");
            }

            return View(loginViewModel);
        }

        [HttpGet]
        public ActionResult Register()
        {
            var registerDto = new RegisterDto();
            return View(registerDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return View(registerDto);

            var subject = "Welcome to our website!";
            var code = new AccountModels().RandomString(10);
            var content = "Hi " + registerDto.Name + "!. Your account has is successfully created. You need to confirm your email. Your password is: " + code;

            var result = Register(registerDto.AccountType, registerDto.Name, registerDto.PhoneNumber, registerDto.Email, registerDto.Username, registerDto.Address, code);
            if (result)
            {
                var sendMail = SendMail(registerDto.Email, subject, content);
                ViewBag.Success = true;
                return View(registerDto);
            }

            return View(registerDto);
        }

        private int Login(string username, string password)
        {
            string pwd = AccountModels.Encrypt(password, true);
            var account = _context.Accounts.SingleOrDefault(r => r.UserName == username && r.PasswordHash == pwd);
            var res = 0;
            if (account != null)
            {
                res = account.RoleID;
            }

            return res;
        }
        private bool Register(int accType, string name, string phone, string email, string username,string address, string password)
        {
            string pwd = AccountModels.Encrypt(password, true);

            var acc = _context.Accounts.SingleOrDefault(a => a.UserName == username);
            if (acc != null)
            {
                return false;
            }
            else
            {
                var newAcc = new Account();
                newAcc.UserName = username;
                newAcc.PasswordHash = pwd;
                newAcc.RoleID = accType;
                if (new AccountModels().AddAcc(newAcc))
                {
                    if(accType == RoleIds.Seller)
                    {
                        if (new AccountModels().AddSeller(newAcc.ID, name, phone, email, address))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if(accType == RoleIds.Customer)
                    {
                        if (new AccountModels().AddCustomer(newAcc.ID, name, phone, email, address))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        private bool SendMail(string toEmail, string subject, string content)
        {
            try
            {
                var host = "smtp.gmail.com";
                var port = 587;
                var fromEmail = "beekunfar@gmail.com";
                var password = "Kurosaki007";
                var fromName = "Mobile Store";

                var smtpClient = new SmtpClient(host, port)
                {
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(fromEmail, password),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = true,
                    Timeout = 100000
                };

                var mail = new MailMessage
                {
                    Body = content,
                    Subject = subject,
                    From = new MailAddress(fromEmail, fromName)
                };

                mail.To.Add(new MailAddress(toEmail));
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = false;
                mail.Priority = MailPriority.High;

                smtpClient.Send(mail);

                return true;
            }
            catch (SmtpException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public ActionResult Logout(string sessionName)
        {
            if (Session[sessionName] == null)
                throw new System.Web.Http.HttpResponseException(HttpStatusCode.NotFound);

            Session[sessionName] = null;

            return RedirectToAction("Index", "HomeScreen");
        }

        public ActionResult ChangePassword(int ID)
        {
            var changePasswordViewModel = new ChangePasswordViewModels()
            {
                ID = ID
            };
            return View(changePasswordViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModels changePasswordViewModels)
        {
            if(ModelState.IsValid)
            {
                if (changePasswordViewModels.NewPassword == changePasswordViewModels.OldPassword)
                {
                    ModelState.AddModelError("NewPassword", "The new password must be different from old password");
                    return View(changePasswordViewModels);
                }
                var acc = _context.Accounts.SingleOrDefault(s => s.ID == changePasswordViewModels.ID);
                if (acc != null)
                {
                    string pwd = AccountModels.Encrypt(changePasswordViewModels.OldPassword, true);
                    if (pwd == acc.PasswordHash)
                    {
                        acc.PasswordHash = AccountModels.Encrypt(changePasswordViewModels.NewPassword, true);
                        if (new AccountModels().UpdatePassword(acc)) 
                        {
                            ViewBag.ChangPass = true;
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("OldPassword", "Old Password is not correct.");
                    }
                }
            }
            return View(changePasswordViewModels);
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordViewModels forgotPasswordViewModels)
        {
            if (!ModelState.IsValid)
                return View(forgotPasswordViewModels);

            var newPass = new AccountModels().RandomString(10);
            string pwd = AccountModels.Encrypt(newPass, true);
            var subject = "Reset Password";
            var content = "Your new password is: " + newPass;

            if (forgotPasswordViewModels.AccountType == RoleIds.Customer)
            {
                var findMail = _context.Cutomers.SingleOrDefault(e => e.Email == forgotPasswordViewModels.Email);
                if (findMail == null)
                {
                    ModelState.AddModelError("Email", "Email account does not coincide with registration!");
                }
                else
                {
                    findMail.Account.PasswordHash = pwd;
                    if (new AccountModels().UpdatePassword(findMail.Account))
                    {
                        var sendMail = SendMail(forgotPasswordViewModels.Email, subject, content);
                        ViewBag.Success = true;
                        return View(forgotPasswordViewModels);
                    }
                }
            }
            else if (forgotPasswordViewModels.AccountType == RoleIds.Seller)
            {
                var findMail = _context.Sellers.SingleOrDefault(e => e.Email == forgotPasswordViewModels.Email);
                if (findMail == null)
                {
                    ModelState.AddModelError("Email", "Email account does not coincide with registration!");
                }
                else
                {
                    findMail.Account.PasswordHash = pwd;
                    if (new AccountModels().UpdatePassword(findMail.Account))
                    {
                        var sendMail = SendMail(forgotPasswordViewModels.Email, subject, content);
                        ViewBag.Success = true;
                        return View(forgotPasswordViewModels);
                    }
                }
            }
            return View(forgotPasswordViewModels);
        }
    }
}