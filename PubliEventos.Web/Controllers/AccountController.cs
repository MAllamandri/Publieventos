using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace PubliEventos.Web.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using System.Web.Security;
    using Microsoft.Practices.Unity;
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Domain.Domain;
    using PubliEventos.Web.App_Start;
    using PubliEventos.Web.Helpers;
    using PubliEventos.Web.Models.AccountModels;
    using User = PubliEventos.Contract.ContractClass.User;

    /// <summary>
    /// Controlador de cuentas.
    /// </summary>
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        #region Properties

        /// <summary>
        /// Servicio de localidades.
        /// </summary>
        [Dependency]
        public IServiceAccounts serviceAccounts { get; set; }

        /// <summary>
        /// Servicio de localidades.
        /// </summary>
        [Dependency]
        public IServiceLocalities ServiceLocalities { get; set; }

        #endregion

        /// <summary>
        /// Vista de login.
        /// </summary>
        /// <returns>Login view.</returns>
        public ActionResult Login()
        {
            var model = new UserModel();
            model.IsLogin = true;

            ViewBag.Localities = new SelectList(ServiceLocalities.GetAllLocalities(), "Id", "Name");

            return View(model);
        }

        /// <summary>
        /// Vista de login.
        /// </summary>
        /// <param name="model">LoginModel.</param>
        /// <returns>Index o Login view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserModel model)
        {
            if (ModelState.IsValid)
            {
                if (this.ValidateUser(model.UserName, model.Password))
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("UserNameOrPassword", "Usuario o ccntraseña Incorrecta.");
            }

            model.IsLogin = true;
            ViewBag.Localities = new SelectList(ServiceLocalities.GetAllLocalities(), "Id", "Name", 0);

            return View(model);
        }

        /// <summary>
        /// Vista de LogOut.
        /// </summary>
        /// <returns>Index View.</returns>
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Vista de registración.
        /// </summary>
        /// <returns>SignUp view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(UserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User()
                {
                    Email = model.SignUpModel.Email,
                    UserName = model.SignUpModel.UserNameToRegister,
                    Password = Encryptor.Encrypt(model.SignUpModel.PasswordToRegister),
                    Locality = new Locality()
                    {
                        Id = model.SignUpModel.Locality.Value
                    },
                    EffectDate = DateTime.Now,
                    BirthDate = model.SignUpModel.BirthDate
                };

                var idUser = serviceAccounts.RegisterUser(user);

                if (idUser != 0)
                {
                    this.SendEmailAccountConfirmation(user.UserName);
                }

                return RedirectToAction("Index", "Home");
            }

            model.IsLogin = false;
            ViewBag.Localities = new SelectList(ServiceLocalities.GetAllLocalities(), "Id", "Name", model.SignUpModel.Locality.HasValue ? model.SignUpModel.Locality.Value : 0);

            return View("Login", model);
        }

        public void AccountActivation(string token)
        {
            // TODO > Activar cuenta.
        }

        #region Private Methods

        /// <summary>
        /// Valida usuario.
        /// </summary>
        /// <param name="userName">userName.</param>
        /// <param name="password">Password</param>
        /// <returns>True si es valido.</returns>
        private bool ValidateUser(string userName, string password)
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                var user = serviceAccounts.GetUserByUserName(userName.ToLower().Trim());

                if (user == null)
                {
                    return false;
                }

                if (user.Password == Encryptor.Encrypt(password))
                {
                    // Inicializa el identity y crea la cookie.
                    var custom = new CustomPrincipal(userName);
                    this.CreateAuthenticationTicket(userName);

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Creao una cookie y la devuelve al usuario.
        /// </summary>
        /// <param name="userName">userName.</param>
        public void CreateAuthenticationTicket(string userName)
        {
            var authUser = this.serviceAccounts.GetUserByUserName(userName);

            // Datos a pasar al usuario en la cookie.
            var customPrincipalModel = new CustomPrincipalSerializeModel()
            {
                Id = authUser.Id,
                FirstName = authUser.FirstName,
                LastName = authUser.LastName,
                ImageProfile = authUser.ImageProfile
            };

            var serializer = new JavaScriptSerializer();
            string userData = serializer.Serialize(customPrincipalModel);

            // Creo el ticket.
            var authTicket = new FormsAuthenticationTicket(
              1,
              userName,
              DateTime.Now,
              DateTime.Now.AddDays(90),
              false,
              userData);

            // Encrypto el ticket.
            string encTicket = FormsAuthentication.Encrypt(authTicket);

            // Creo la cookie.
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);

            // Devuelvo la cookie al usuario.
            HttpContext.Response.Cookies.Add(cookie);
        }

        public void SendEmailAccountConfirmation(string userName)
        {
            var user = this.serviceAccounts.GetUserByUserName(userName);

            var token = new Guid();

            var smtp = new SmtpClient
            {
                Host = ConfigurationManager.AppSettings["SmtpHost"].ToString(),
                Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]),
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(ConfigurationManager.AppSettings["Mail"], ConfigurationManager.AppSettings["Password"])
            };

            var fromAddress = new MailAddress(ConfigurationManager.AppSettings["Mail"]);
            var toAddress = new MailAddress(user.Email);
            string subject = "PubliEventos - Activación de cuenta";
            string body = "Para activar su cuenta ingrese al link que figura a continuación: <br/>";
            body += System.Web.HttpContext.Current.Server.MapPath("/Account/AccountActivation") + "?token=" + token.ToString();

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }

        #endregion
    }
}
