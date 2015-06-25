namespace PubliEventos.Services
{
    using NHibernate;
    using PubliEventos.DataAccess.Infrastructure;
    using PubliEventos.Services.Services;
    using System;

    /// <summary>
    /// Clase base de servicios.
    /// </summary>
    public abstract class BaseService
    {
        #region Properties

        /// <summary>
        /// Sesión de nhibernate.
        /// </summary>
        public static ISession CurrentSession
        {
            get
            {
                var sessionHelper = new SessionHelper();
                return sessionHelper.Current;
            }
        }

        /// <summary>
        /// Servicio interno.
        /// </summary>
        public static InternalServices InternalServices
        {
            get { return new InternalServices(); }
        }

        /// <summary>
        /// Cantidad máxima de contenidos reportados de un usuario, para suspenderlo o desactivarlo.
        /// </summary>
        public static int ContentsReports
        {
            get { return Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ContentsReports"]); }
        }

        /// <summary>
        /// Path donde se ubican las imágenes de perfil.
        /// </summary>
        public static string PathProfiles
        {
            get { return System.Configuration.ConfigurationSettings.AppSettings["PathProfiles"]; }
        }

        #endregion
    }
}
