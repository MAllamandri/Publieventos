using PubliEventos.Services.Services;

namespace PubliEventos.Web.App_Start
{
    using System;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Security;
    using Microsoft.Practices.Unity;
    using PubliEventos.Contract.Contracts;


    public class CustomPrincipal : IPrincipal
    {
        #region Properties

        /// <summary>
        /// Servicio de cuentas.
        /// </summary>
        [Dependency]
        public IServiceAccounts serviceAccounts { get; set; }

        public IIdentity Identity { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor de la clase.
        /// </summary>
        /// <param name="serviceAccounts">Servicio de cuentas.</param>
        public CustomPrincipal(IServiceAccounts serviceAccounts)
        {
            this.serviceAccounts = serviceAccounts;
        }

        /// <summary>
        /// Constructor de la clase.
        /// </summary>
        /// <param name="username">Usuario a identificar.</param>
        public CustomPrincipal(string username)
        {
            this.Identity = new GenericIdentity(username);
        }

        #endregion

        /// <summary>
        /// Indica si un usuario se encuentra en dicho rol.
        /// </summary>
        /// <param name="role">Rol a verificar.</param>
        /// <returns>True si se encuentra en el rol.</returns>
        public bool IsInRole(string role)
        {
            return Identity != null && Identity.IsAuthenticated &&
               !string.IsNullOrWhiteSpace(role) && Roles.IsUserInRole(Identity.Name, role);
        }

        /// <summary>
        /// Creao una cookie y la devuelve al usuario.
        /// </summary>
        /// <param name="userName">userName.</param>
        public void CreateAuthenticationTicket(string userName)
        {
            var authUser = ServiceAccounts.GetUserByUserName(userName);

            var authTicket = new FormsAuthenticationTicket(
              1,
              userName,
              DateTime.Now,
              DateTime.Now.AddHours(8),
              false,
              string.Format("{0}-{1}-{2}", authUser.Id, authUser.UserName, authUser.Email));

            // Encrypto el ticket.
            string encTicket = FormsAuthentication.Encrypt(authTicket);

            // Creo la cookie.
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);

            // Devuelvo la cookie al usuario.
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }

    //public class CustomIdentity : IIdentity
    //{
    //    public CustomIdentity(string name)
    //    {
    //        this.Name = name;
    //    }

    //    public string AuthenticationType
    //    {
    //        get { return "Custom"; }
    //    }

    //    public bool IsAuthenticated
    //    {
    //        get { return !string.IsNullOrEmpty(this.Name); }
    //    }

    //    public string Name { get; private set; }
    //}
}