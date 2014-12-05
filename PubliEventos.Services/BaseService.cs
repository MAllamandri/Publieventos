﻿namespace PubliEventos.Services
{
    using NHibernate;
    using PubliEventos.DataAccess.Infrastructure;
    using PubliEventos.Services.Services;

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

        #endregion
    }
}
