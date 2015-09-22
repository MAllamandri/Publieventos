namespace PubliEventos.Services.Services
{
    using System;
    using System.Linq;
    using System.Transactions;
    using LinqKit;
    using NHibernate.Linq;
    using PubliEventos.Contract.Class;
    using PubliEventos.DataAccess.Querys;
    using PubliEventos.Contract.Services.Account;

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
                return CurrentSession.Query<Domain.Domain.User>()
                        .Where(u => u.UserName.ToLower() == userName.ToLower() && !u.NullDate.HasValue)
                        .Select(u => InternalServices.GetUserSummary(u))
                        .SingleOrDefault();
            }
        }

        /// <summary>
        /// Verifica si ya existe un usuario con ese email.
        /// </summary>
        /// <param name="request">Los parámetros de la operación.</param>
        /// <returns>El resultado de la operación.</returns>
        public static ExistsEmailResponse ExistsEmail(ExistsEmailRequest request)
        {
            var predicate = PredicateBuilder.True<Domain.Domain.User>();

            predicate = predicate.And(x => !x.NullDate.HasValue);

            if (!string.IsNullOrEmpty(request.Email))
            {
                predicate = predicate.And(x => x.Email.ToLower() == request.Email.ToLower());
            }

            if (request.UserId.HasValue)
            {
                predicate = predicate.And(x => x.Id != request.UserId.Value);
            }

            var exists = CurrentSession.Query<Domain.Domain.User>().Where(predicate).Any();

            return new ExistsEmailResponse()
            {
                Exists = exists
            };
        }

        /// <summary>
        /// Da de alta un usuario.
        /// </summary>
        /// <param name="request">Los parámetros de la operación.</param>
        /// <returns>El resultado de la operación.</returns>
        public static RegisterUserResponse RegisterUser(RegisterUserRequest request)
        {
            if (request != null)
            {
                var user = new Domain.Domain.User()
                {
                    FirstName = request.User.FirstName,
                    LastName = request.User.LastName,
                    Email = request.User.Email,
                    Password = request.User.Password,
                    EffectDate = request.User.EffectDate,
                    UserName = request.User.UserName,
                    Locality = CurrentSession.Get<Domain.Domain.Locality>(request.User.Locality.Id),
                    BirthDate = request.User.BirthDate
                };

                new BaseQuery<Domain.Domain.User, int>().Create(user);

                return new RegisterUserResponse
                {
                    UserId = user.Id
                };
            }

            return new RegisterUserResponse();
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

        /// <summary>
        /// Busca usuarios por autocompletado de nombre de usuario.
        /// </summary>
        /// <param name="request">Los parámetros de la operación.</param>
        /// <returns>El resultado de la operación.</returns>
        public static SearchUsersByPartialUserNameResponse SearchUsersByPartialUserName(SearchUsersByPartialUserNameRequest request)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                var total = CurrentSession.Query<Domain.Domain.User>().
                     Where(u => u.UserName.ToLower().StartsWith(request.UserName.ToLower()) && !u.NullDate.HasValue && u.Active).Count();

                var users = CurrentSession.Query<Domain.Domain.User>().
                     Where(u => u.UserName.ToLower().StartsWith(request.UserName.ToLower()) && !u.NullDate.HasValue && u.Active)
                     .Skip(request.PageNumber - 1)
                     .Take(request.PageSize)
                     .Select(u => InternalServices.GetUserSummary(u)).ToList();

                return new SearchUsersByPartialUserNameResponse()
                {
                    Users = users,
                    Quantity = total
                };
            }
        }

        /// <summary>
        /// Recupera un usuario por su Id.
        /// </summary>
        /// <param name="request">Los parámetros de la operación.</param>
        /// <returns>El resultado de la operación.</returns>
        public static GetUserByIdResponse GetUserById(GetUserByIdRequest request)
        {
            var user = CurrentSession.Query<Domain.Domain.User>().Where(x => x.Id == request.UserId && !x.NullDate.HasValue && x.Active).Select(x => InternalServices.GetUserSummary(x)).Single();

            return new GetUserByIdResponse()
            {
                User = user
            };
        }

        /// <summary>
        /// Edita el perfil de un usuario.
        /// </summary>
        /// <param name="request">Los parámetros de la operación.</param>
        /// <returns>El resultado de la operación.</returns>
        public static EditProfileResponse EditProfile(EditProfileRequest request)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                var user = CurrentSession.Query<Domain.Domain.User>().Where(u => u.Id == request.UserId).SingleOrDefault();

                if (user == null)
                {
                    throw new Exception("El usuario no existe o fue dado de baja");
                }

                user.UserName = request.UserName;
                user.LastName = request.LastName;
                user.FirstName = request.FirstName;
                user.ImageProfile = request.ImageProfile;
                user.Locality = CurrentSession.Get<Domain.Domain.Locality>(request.LocalityId);
                user.Email = request.Email;
                user.BirthDate = request.BirthDate;

                new BaseQuery<Domain.Domain.User, int>().Update(user);

                transaction.Complete();
            }

            return new EditProfileResponse();
        }

        /// <summary>
        /// Edita el password del usuario actual.
        /// </summary>
        /// <param name="request">Los parámetros de la operación..</param>
        /// <returns>El resultado de la operación.</returns>
        public static EditPasswordResponse EditPassword(EditPasswordRequest request)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                var user = CurrentSession.Get<Domain.Domain.User>(request.UserId);

                //Actualizo el password.
                user.Password = request.NewPassword;

                transaction.Complete();

                return new EditPasswordResponse();
            }
        }

        /// <summary>
        /// Envía código de verificación de usuario para recuperar su contraseña.
        /// </summary>
        /// <param name="request">Los parámetros de la operación..</param>
        /// <returns>El resultado de la operación.</returns>
        public static SendRecoverPasswordCodeResponse SendRecoverPasswordCode(SendRecoverPasswordCodeRequest request)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                var user = CurrentSession.Query<Domain.Domain.User>()
                            .Where(x => x.UserName.ToLower() == request.UserName.ToLower() && !x.NullDate.HasValue && x.Active && request.Email.ToLower() == x.Email.ToLower())
                            .SingleOrDefault();

                if (user == null)
                {
                    return new SendRecoverPasswordCodeResponse()
                    {
                        Success = false
                    };
                }

                var code = Guid.NewGuid().ToString().Substring(0, 6).ToUpper();

                //Envío el mail notificando al usuario que fue desactivado.
                var subject = "Publieventos - Código de Verificación";
                var body = string.Format("Estimado/a {0}:" +
                    "<br/><br/>Este es su código de seguridad para recuperar su contraseña <strong>{1}</strong>" +
                    "<br/><br/>Saludos cordiales." +
                    "<br/>Equipo de administración de Publieventos", user.FirstName, code);

                try
                {
                    InternalServices.SendMail(user.Email, subject, body, true);

                    // Doy de baja los códigos existentes del usuario.
                    var userCodes = CurrentSession.Query<Domain.Domain.RecoverPasswordCode>().Where(x => x.User.Id == user.Id && !x.NullDate.HasValue).ToList();

                    foreach (var userCode in userCodes)
                    {
                        userCode.NullDate = DateTime.Now;
                    }

                    // Creo el nuevo código.
                    var recoverCode = new Domain.Domain.RecoverPasswordCode();
                    recoverCode.User = user;
                    recoverCode.EffectDate = DateTime.Now;
                    recoverCode.Code = code;

                    new BaseQuery<Domain.Domain.RecoverPasswordCode, int>().Create(recoverCode);

                    transaction.Complete();

                    return new SendRecoverPasswordCodeResponse()
                    {
                        Success = true,
                        UserId = user.Id
                    };
                }
                catch (Exception)
                {
                    transaction.Complete();

                    return new SendRecoverPasswordCodeResponse()
                    {
                        Success = false
                    };
                }
            }
        }

        /// <summary>
        /// Valida si el código de verificación para cambiar contraseña es correcto.
        /// </summary>
        /// <param name="request">Los parámetros de la operación..</param>
        /// <returns>El resultado de la operación.</returns>
        public static ValidateRecoverPasswordCodeResponse ValidateRecoverPasswordCode(ValidateRecoverPasswordCodeRequest request)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                var code = CurrentSession.Query<Domain.Domain.RecoverPasswordCode>().Where(x => x.User.Id == request.UserId && !x.NullDate.HasValue).FirstOrDefault();

                if (code != null && code.Code == request.Code)
                {
                    // Lo doy de baja.
                    code.NullDate = DateTime.Now;

                    transaction.Complete();

                    return new ValidateRecoverPasswordCodeResponse
                    {
                        IsValid = true
                    };
                }
            }

            return new ValidateRecoverPasswordCodeResponse
            {
                IsValid = false
            };
        }
    }
}
