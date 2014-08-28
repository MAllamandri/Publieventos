namespace PubliEventos.Services.Services
{
    using System.Linq;
    using NHibernate;
    using NHibernate.Linq;
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
                return sessionHelper.Current.Query<Domain.Domain.User>().
                     Where(u => u.UserName.ToLower() == userName.ToLower() && !u.NullDate.HasValue).Select(u => new User()
                     {
                         Id = u.Id,
                         UserName = u.UserName,
                         Password = u.Password,
                         Active = u.Active,
                         BirthDate = u.BirthDate,
                         Locality = u.Locality,
                         Email = u.Email,
                         FirstName = u.FirstName,
                         LastName = u.LastName,
                         ImageProfile = u.ImageProfile,
                         NullDate = u.NullDate
                     }).Single();
            }
        }
    }
}
