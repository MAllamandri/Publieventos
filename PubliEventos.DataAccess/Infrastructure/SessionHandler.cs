namespace PubliEventos.DataAccess.Infrastructure
{
    using NHibernate;
    using NHibernate.Cfg;
    using NHibernate.Context;

    /// <summary>
    /// Clase que maneja las sesiones de NHibernate.
    /// </summary>
    public sealed class SessionHandler
    {
        /// <summary>
        /// Variable de sesión.
        /// </summary>
        private static ISessionFactory _sessionFactory = null;

        /// <summary>
        /// Path de mapeo.
        /// </summary>
        private static readonly string mappingPath = System.Configuration.ConfigurationSettings.AppSettings["MappingPath"];

        /// <summary>
        /// Configura la sesión.
        /// </summary>
        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    var configuration = new Configuration();
                    configuration.Configure();
                    configuration.AddFile(mappingPath + "Locality.hbm.xml");
                    configuration.AddFile(mappingPath + "User.hbm.xml");
                    configuration.AddFile(mappingPath + "Event.hbm.xml");
                    configuration.AddFile(mappingPath + "Comment.hbm.xml");
                    configuration.AddFile(mappingPath + "EventType.hbm.xml");
                    configuration.AddFile(mappingPath + "MultimediaContent.hbm.xml");
                    configuration.AddFile(mappingPath + "AccountActivationToken.hbm.xml");
                    configuration.AddFile(mappingPath + "Province.hbm.xml");
                    configuration.AddFile(mappingPath + "Group.hbm.xml");
                    configuration.AddFile(mappingPath + "UsersGroup.hbm.xml");
                    configuration.AddFile(mappingPath + "Invitation.hbm.xml");
                    configuration.AddFile(mappingPath + "Report.hbm.xml");
                    configuration.AddFile(mappingPath + "Suspension.hbm.xml");
                    _sessionFactory = configuration.BuildSessionFactory();
                }
                return _sessionFactory;
            }
        }

        /// <summary>
        /// Abre la sesión.
        /// </summary>
        /// <returns></returns>
        public ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        /// <summary>
        /// Crea un ISession y lo bindea con el NHibernate Context.
        /// </summary>
        public void CreateSession()
        {
            CurrentSessionContext.Bind(OpenSession());
        }

        /// <summary>
        /// Cierra la sesión y la desvincula del contexto.
        /// </summary>
        public void CloseSession()
        {
            if (CurrentSessionContext.HasBind(SessionFactory))
            {
                CurrentSessionContext.Unbind(SessionFactory).Dispose();
            }
        }

        /// <summary>
        /// Devuelve la sesión actual si es que hay una abierta, sino abre una.
        /// </summary>
        /// <returns>Sessión actual bindeada NHibernate ISession.</returns>
        public ISession GetCurrentSession()
        {
            if (!CurrentSessionContext.HasBind(SessionFactory))
            {
                CurrentSessionContext.Bind(SessionFactory.OpenSession());
            }
            return SessionFactory.GetCurrentSession();
        }
    }

    /// <summary>
    /// Helper para manejar sesiones de NHibernate.
    /// </summary>
    public class SessionHelper
    {
        /// <summary>
        /// NHibernate Helper
        /// </summary>
        private readonly SessionHandler _repository = null;

        public SessionHelper()
        {
            _repository = new SessionHandler();
        }

        /// <summary>
        /// Retorna la sesión actual.
        /// </summary>
        public ISession Current
        {
            get
            {
                return _repository.GetCurrentSession();
            }
        }

        /// <summary>
        /// Crea la sesión.
        /// </summary>
        public void CreateSession()
        {
            _repository.CreateSession();
        }

        /// <summary>
        /// Limpia la sesión.
        /// </summary>
        public void ClearSession()
        {
            Current.Clear();
        }

        /// <summary>
        /// Abre la sesión.
        /// </summary>
        public void OpenSession()
        {
            _repository.OpenSession();
        }

        /// <summary>
        /// Cierra la sesión.
        /// </summary>
        public void CloseSession()
        {
            _repository.CloseSession();
        }
    }
}
