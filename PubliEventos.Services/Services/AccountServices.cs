namespace PubliEventos.Services.Services
{
    using System;
    using System.Linq;
    using System.Transactions;
    using NHibernate.Linq;
    using PubliEventos.Contract.Class;
    using PubliEventos.DataAccess.Querys;

    /// <summary>
    /// Servicio de cuentas.
    /// </summary>
    public class AccountServices : BaseService
    {
        /// <summary>
        /// Obtiene usuario por userName.
        /// </summary>
        /// <param name="userName">userName del usuario.</param>
        /// <returns>User.</returns>
        public static User GetUserByUserName(string userName)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                return CurrentSession.Query<Domain.Domain.User>().
                     Where(u => u.UserName.ToLower() == userName.ToLower() && !u.NullDate.HasValue).Select(u => new User()
                     {
                         Id = u.Id,
                         UserName = u.UserName,
                         Password = u.Password,
                         Active = u.Active,
                         BirthDate = u.BirthDate,
                         Locality = new Locality()
                         {
                             Id = u.Locality.Id,
                             Name = u.Locality.Name,
                             Latitude = u.Locality.Latitude,
                             Longitude = u.Locality.Longitude,
                             Province = new Province()
                             {
                                 Id = u.Locality.Province.Id,
                                 Name = u.Locality.Province.Name
                             }
                         },
                         Email = u.Email,
                         FirstName = u.FirstName,
                         LastName = u.LastName,
                         ImageProfile = u.ImageProfile,
                         NullDate = u.NullDate
                     }).SingleOrDefault();
            }
        }

        /// <summary>
        /// Verifica si ya existe un usuario con ese email.
        /// </summary>
        /// <param name="email">Email del usuario.</param>
        /// <returns>True si existe uno, false caso contrario.</returns>
        public static bool UserExistsWithEmail(string email)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                return CurrentSession.Query<Domain.Domain.User>().
                    Where(u => u.Email.ToLower() == email.ToLower() && !u.NullDate.HasValue).Any();
            }
        }

        /// <summary>
        /// Da de alta un usuario.
        /// </summary>
        /// <param name="user">Usuario.</param>
        public static int RegisterUser(User user)
        {
            if (user != null)
            {
                var userCreate = new Domain.Domain.User()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = user.Password,
                    EffectDate = user.EffectDate,
                    UserName = user.UserName,
                    Locality = CurrentSession.Get<Domain.Domain.Locality>(user.Locality.Id),
                    BirthDate = user.BirthDate
                };

                new BaseQuery<Domain.Domain.User, int>().Create(userCreate);

                return userCreate.Id;
            }
            return 0;
        }

        /// <summary>
        /// Guarda un token de activación de cuenta.
        /// </summary>
        /// <param name="token">Token.</param>
        public static void SaveAccountActivationToken(string token, int idUser)
        {
            if (idUser != 0 && !string.IsNullOrEmpty(token))
            {
                var activationToken = new Domain.Domain.AccountActivationToken()
                {
                    EffectDate = DateTime.Now,
                    ExpirationDate = DateTime.Now.AddDays(30),
                    Token = token,
                    User = CurrentSession.Get<Domain.Domain.User>(idUser)
                };

                new BaseQuery<Domain.Domain.AccountActivationToken, int>().Create(activationToken);
            }
        }

        /// <summary>
        /// Activa una cuenta mediante el token.
        /// </summary>
        /// <param name="token">Token.</param>
        /// <returns>True si se activo la cuenta, false caso contrario.</returns>
        public static bool ActivateAccount(string token)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                var idUser = CurrentSession.Query<Domain.Domain.AccountActivationToken>().
                    Where(x => x.Token == token && x.ExpirationDate >= DateTime.Now && !x.NullDate.HasValue)
                    .Select(x => x.User.Id)
                    .SingleOrDefault();

                if (idUser != 0)
                {
                    var user = new BaseQuery<Domain.Domain.User, int>().LoadById(idUser);

                    var tokenValidate =
                        CurrentSession.Query<Domain.Domain.AccountActivationToken>()
                            .Where(x => x.Token == token)
                            .SingleOrDefault();

                    // Doy de baja el token.
                    tokenValidate.NullDate = DateTime.Now;

                    // Activo el usuario.
                    user.Active = true;

                    transaction.Complete();

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Indica si el usuario tiene un token de activación de cuenta activo.
        /// </summary>
        /// <param name="idUser">Identificador del usuario.</param>
        /// <returns>True si posee un token activo, false caso contrario.</returns>
        public static bool HasActiveActivationToken(int idUser)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                var tokens = CurrentSession.Query<Domain.Domain.AccountActivationToken>()
                    .Where(x => !x.NullDate.HasValue && x.User.Id == idUser && x.ExpirationDate < DateTime.Now).ToList();

                if (tokens.Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Doy de baja los token expirados del usuario.
        /// </summary>
        /// <param name="userName">Nombre de usuario.</param>
        public static void DeleteActivationToken(string userName)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                var tokens = CurrentSession.Query<Domain.Domain.AccountActivationToken>()
                    .Where(x => !x.NullDate.HasValue && x.User.UserName == userName && x.ExpirationDate < DateTime.Now).ToList();

                if (tokens.Any())
                {
                    foreach (var accountActivationToken in tokens)
                    {
                        accountActivationToken.NullDate = DateTime.Now;
                    }
                }

                transaction.Complete();
            }
        }
    }
}
