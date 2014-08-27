using System;
using System.Web.Security;

namespace PubliEventos.Web.Controllers
{
    using System.Web.Mvc;
    using Microsoft.Practices.Unity;
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Web.Models.AccountModels;
    using PubliEventos.Web.App_Start;
    using PubliEventos.Web.Helpers;

    /// <summary>
    /// Controlador de cuentas.
    /// </summary>
    [AllowAnonymous]
    public class AccountController : Controller
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
            var model = new LoginModel();
            return View(model);
        }

        /// <summary>
        /// Vista de login.
        /// </summary>
        /// <param name="model">LoginModel.</param>
        /// <returns>Index o Login view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
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
                    custom.CreateAuthenticationTicket(userName);

                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
