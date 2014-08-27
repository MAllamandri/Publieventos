using System.Linq;
using System.Security.Cryptography.X509Certificates;
using NHibernate.Linq;

namespace PubliEventos.Services.Services
{
    using NHibernate;
    using NHibernate.Criterion;
    using PubliEventos.Contract.ContractClass;
    using PubliEventos.DataAccess.Infrastructure;

    /// <summary>
    /// Servicio de cuentas.
    /// </summary>
    public class ServiceAccounts
    {
        /// <summary>
        /// Obtiene usuario por userName.
        /// </summary>
        /// <param name="userName">userName del usuario.</param>
        /// <returns>User.</returns>
        public static User GetUserByUserName(string userName)
        {
            var sessionHelper = new SessionHelper();

            using (ITransaction transaction = sessionHelper.Current.BeginTransaction())
            {
                var query =
                    sessionHelper.Current.QueryOver<Domain.Domain.User>()
                        .Where(u => u.UserName == userName.ToLower() && u.NullDate == null).SingleOrDefault();

                if (query != null)
                {
                    var user = new User();
                    {
                        user.Id = query.Id;
                        user.UserName = query.UserName;
                        user.Password = query.Password;
                        user.Active = query.Active;
                        user.BirthDate = query.BirthDate;
                        user.Locality = query.Locality;
                        user.Email = query.Email;
                        user.FirstName = query.FirstName;
                        user.LastName = query.LastName;
                        user.NullDate = query.NullDate;
                    };

                    return user;
                }

                return null;
            }
        }

    }
}
