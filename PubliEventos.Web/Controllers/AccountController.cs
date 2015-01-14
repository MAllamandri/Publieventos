namespace PubliEventos.Web.Controllers
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using System.Web.Security;
    using Microsoft.Practices.Unity;
    using PubliEventos.Contract.Class;
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Web.App_Start;
    using PubliEventos.Web.Helpers;
    using PubliEventos.Web.Models.AccountModels;
    using PubliEventos.Web.Models;
    using System.Collections.Generic;
    using PubliEventos.Contract.Services.Account;

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
        public IAccountServices serviceAccounts { get; set; }

        /// <summary>
        /// Servicio de localidades.
        /// </summary>
        [Dependency]
        public ILocalityServices ServiceLocalities { get; set; }

        #endregion

        /// <summary>
        /// Vista de login.
        /// </summary>
        /// <returns>Login view.</returns>
        public ActionResult Login()
        {
            var model = new UserModel();
            model.IsLogin = true;

            ViewBag.Provinces = new SelectList(ServiceLocalities.GetAllProvinces(), "Id", "Name");

            return View(model);
        }

        /// <summary>
        /// Vista de login.
        /// </summary>
        /// <param name="model">LoginModel.</param>
        /// <returns>Index o Login view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserModel model, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                if (this.ValidateUser(model))
                {
                    if (string.IsNullOrEmpty(ReturnUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return Redirect(ReturnUrl);
                    }
                }
            }

            model.IsLogin = true;
            ViewBag.Provinces = new SelectList(ServiceLocalities.GetAllProvinces(), "Id", "Name", 0);

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
        /// Activación de cuenta mediante el token.
        /// </summary>
        /// <param name="token">token de activación.</param>
        public ActionResult AccountActivation(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                ViewBag.active = serviceAccounts.ActivateAccount(token);
            }

            return View();
        }

        #region Public Methods



        #endregion

        #region Private Methods

        /// <summary>
        /// Valida lo datos del usuario.
        /// </summary>
        /// <param name="model">Modelo de login.</param>
        /// <returns>True si es valido, false caso contrario.</returns>
        private bool ValidateUser(UserModel model)
        {
            if (!string.IsNullOrEmpty(model.UserName) && !string.IsNullOrEmpty(model.Password))
            {
                var user = serviceAccounts.GetUserByUserName(model.UserName.ToLower().Trim());

                if (user != null)
                {
                    if (!user.Active)
                    {
                        ModelState.AddModelError("Error", "Usuario no activo, por favor active su cuenta verificando su email.");

                        ViewBag.isExpirate = this.serviceAccounts.HasActiveActivationToken(user.Id.Value);

                        return false;
                    }

                    if (user.Password == Encryptor.Encrypt(model.Password))
                    {
                        // Inicializa el identity y crea la cookie.
                        var custom = new CustomPrincipal(model.UserName);
                        this.CreateAuthenticationTicket(model.UserName, model.RememberMe);

                        return true;
                    }
                }
            }

            ModelState.AddModelError("Error", "Usuario o ccntraseña Incorrecta.");

            return false;
        }

        /// <summary>
        /// Creao una cookie y la devuelve al usuario.
        /// </summary>
        /// <param name="userName">userName.</param>
        private void CreateAuthenticationTicket(string userName, bool rememberMe)
        {
            var authUser = this.serviceAccounts.GetUserByUserName(userName);

            // Datos a pasar al usuario en la cookie.
            var customPrincipalModel = new CustomPrincipalSerializeModel()
            {
                Id = authUser.Id.Value,
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
              rememberMe,
              userData,
              "/");

            // Encrypto el ticket.
            string encTicket = FormsAuthentication.Encrypt(authTicket);

            // Creo la cookie.
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);

            // Devuelvo la cookie al usuario.
            HttpContext.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Envia email de activación de cuenta.
        /// </summary>
        /// <param name="userName">Username del usuario.</param>
        private void SendEmailAccountConfirmation(string userName)
        {
            var user = this.serviceAccounts.GetUserByUserName(userName);

            var token = Guid.NewGuid();

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
            body += string.Format("<a href='{0}://{1}/Account/AccountActivation?token={2}'>Activar mi cuenta</a>", System.Web.HttpContext.Current.Request.RequestContext.HttpContext.Request.Url.Scheme, System.Web.HttpContext.Current.Request.RequestContext.HttpContext.Request.Url.Authority, token);

            try
            {
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                {
                    smtp.Send(message);
                }

                // Guarda el token de activación de cuenta.
                serviceAccounts.SaveAccountActivationToken(token.ToString(), user.Id.Value);
            }
            catch (Exception)
            {
                throw new Exception("Ha ocurrido un error al enviar mail.");
            }
        }

        #endregion

        #region Json Methods

        /// <summary>
        /// Vista de registración.
        /// </summary>
        /// <returns>SignUp view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SignUp(UserModel model)
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

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }

            model.IsLogin = false;

            return Json(new { Success = false, Errors = ModelErrors.GetModelErrors(ModelState) }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Valida si el nombre de usuario ya existe.
        /// </summary>
        /// <param name="userName">nombre de usuario.</param>
        /// <returns>True si existe, false caso contrario.</returns>
        [HttpPost]
        public JsonResult ValidateExistUserName(string userName)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                var user = serviceAccounts.GetUserByUserName(userName.ToLower().Trim());

                if (user != null)
                {
                    return Json(new { Exist = true }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { Exist = false }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Valida si ya existe un usuario con ese email.
        /// </summary>
        /// <param name="email">Email a validar.</param>
        /// <returns>True si ya existe, false caso contrario.</returns>
        [HttpPost]
        public JsonResult ValidateExistEmail(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var exist = serviceAccounts.UserExistsWithEmail(email);

                return Json(new { Exist = exist }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Exist = false }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Obtiene las localidades filtradas por provincia.
        /// </summary>
        /// <param name="IdProvince">Identificador de la provincia.</param>
        /// <returns>Localidades.</returns>
        public JsonResult GetLocalitiesByProvince(string IdProvince)
        {
            if (!string.IsNullOrEmpty(IdProvince))
            {
                var localities =
                    ServiceLocalities.GetAllLocalities()
                        .Where(x => x.Province.Id == Convert.ToInt32(IdProvince))
                        .Select(x => new { x.Id, x.Name })
                        .ToList();

                return Json(localities, JsonRequestBehavior.AllowGet);
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Reenvia el email de activación de cuenta.
        /// </summary>
        /// <param name="userName">Nombre de usuario</param>
        /// <returns>Json.</returns>
        [HttpPost]
        public JsonResult ResendEmailActivation(string userName)
        {
            try
            {
                this.SendEmailAccountConfirmation(userName);

                // Doy de baja los token expirados y no eliminados del usuario.
                this.serviceAccounts.DeleteActivationToken(userName);

                return Json(new { isValid = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { isValid = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Busca usuario por autocompletado de nombre de usuario.
        /// </summary>
        /// <param name="userName">Nombre de usuario.</param>
        /// <param name="pageNumber">Número de página.</param>
        /// <param name="pageSize">Tamaño de página.</param>
        /// <returns>Usuarios encontrados.</returns>
        public JsonResult SearchUsersByUserName(string userName, int pageNumber, int pageSize)
        {
            List<Select2UserResult> Users = new List<Select2UserResult>();

            var response = this.serviceAccounts.SearchUsersByPartialUserName(new SearchUsersByPartialUserNameRequest() { UserName = userName, PageNumber = pageNumber, PageSize = pageSize });

            foreach (var user in response.Users)
            {
                var userResult = new Select2UserResult();
                userResult.Id = user.Id.Value;
                userResult.Text = user.UserName;

                Users.Add(userResult);
            }

            return Json(new { Users = Users, Quantity = response.Quantity }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
