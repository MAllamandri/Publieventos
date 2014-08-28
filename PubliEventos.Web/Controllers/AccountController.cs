namespace PubliEventos.Web.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using System.Web.Security;
    using Microsoft.Practices.Unity;
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Web.App_Start;
    using PubliEventos.Web.Helpers;
    using PubliEventos.Web.Models.AccountModels;

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

        #endregion

        /// <summary>
        /// Vista de login.
        /// </summary>
        /// <returns>Login view.</returns>
        public ActionResult Login()
        {
            var model = new UserModel();
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

                ModelState.AddModelError("Password", "Contraseña Incorrecta.");
            }

            return View();
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
                return RedirectToAction("Index", "Home");
            }

            return View("Login", model);
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

                if (Encryptor.Encrypt(password) == Encryptor.Encrypt(user.Password))
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

        #endregion
    }
}
