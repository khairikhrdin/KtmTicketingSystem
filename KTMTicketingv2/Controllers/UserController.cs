using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KTMTicketingv2.Models;
using System.Net.Mail;
using System.Net;
using System.Web.Security;

namespace KTMTicketingv2.Controllers
{
    public class UserController : Controller
    {
        //Registration GET
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        //Registration POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude="IsEmailVerified,ActivationCode")]User user)
        {
            bool Status = false;
            string Message = "";

            //
            //Model Validation
            if (ModelState.IsValid)
            {
                #region //Email is already exist

                var isExist = IsEmailExist(user.EmailID);

                if(isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return View(user);
                }
                #endregion

                #region //Generate activation code
                user.ActivationCode = Guid.NewGuid();
                #endregion

                #region //Pasword Hashing
                user.Password = Crypto.Hash(user.Password);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);
                #endregion

                user.IsEmailVerified = false;

                #region //Save data to database
                using(KTMTicketingEntities dc = new KTMTicketingEntities())
                {
                    dc.Users.Add(user);
                    dc.SaveChanges();

                    //Send email to user
                    SendVerificationLinkEmail(user.EmailID, user.ActivationCode.ToString());
                    Message = "Registration successfully done. Account activation link has been send to your " +
                        "email id: "+ user.EmailID;

                    Status = true;

                }
                #endregion
            }
            else
            {
                Message = "Invalid Request";
            }

            ViewBag.Message = Message;
            ViewBag.Status = Status;
            return View(user);
        }

        //Verify Account
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (KTMTicketingEntities dc = new KTMTicketingEntities())
            {
                dc.Configuration.ValidateOnSaveEnabled = false; // This line I have added here to avoid 
                // Confirm password does not match issue on save changes
                var v = dc.Users.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVerified = true;
                    dc.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }
            }
            ViewBag.Status = Status;
            return View();
        }

        //Login GET
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        //Login Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login, string ReturnUrl)
        {
            string Message = "";
            using (KTMTicketingEntities dc = new KTMTicketingEntities())
            {
                var v = dc.Users.Where(a => a.EmailID == login.EmailID).FirstOrDefault();

                if(v != null)
                {
                    if (!v.IsEmailVerified)
                    {
                        ViewBag.Message = "Please verify your email first";
                        return View();
                    }

                    if (string.Compare(Crypto.Hash(login.Password), v.Password) == 0)
                    {
                        int timeout = login.RememberMe ? 525600 : 20; // 525600 min = 1 year
                        //var ticket = FormsAuthentication.SetAuthCookie(login.EmailID, login.RememberMe);
                        var ticket = new FormsAuthenticationTicket(login.EmailID, login.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);

                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        Message = "Invalid credential provided";
                    }
                }
                else
                {
                    Message = "Invalid credential provided";
                }
            }
            ViewBag.Message = Message;
            return View();
        }

        //Logout
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }

        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            using(KTMTicketingEntities dc = new KTMTicketingEntities())
            {
                var v = dc.Users.Where(a => a.EmailID == emailID).FirstOrDefault();
                return v != null;
            }
            
        }

        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode)
        {
            var verifyUrl = "/User/VerifyAccount/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("your gmail", "Admin KTM TICKETING");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "your gmail password";
            string subject = "Your account is successfully created!";

            string body = "<br/><br/>We are excited to tell you that your KTM Ticketing account is " +
                "successfully created. Please click on the below link to verify your account " + 
                "<br/><br/><a href='"+link+"'>"+link+"</a>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,

            }) smtp.Send(message);

        }
    }
}