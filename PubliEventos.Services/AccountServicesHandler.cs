namespace PubliEventos.Services
{
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Contract.Class;
    using PubliEventos.Services.Services;
    using PubliEventos.Contract.Services.Account;

    /// <summary>
    /// Manejador de cuentas.
    /// </summary>
    public class AccountServicesHandler : IAccountServices
    {
        /// <summary>
        /// Obtiene usuario por userName.
        /// </summary>
        /// <param name="userName">userName del usuario.</param>
        /// <returns></returns>
        public User GetUserByUserName(string userName)
        {
            return AccountServices.GetUserByUserName(userName);
        }

        /// <summary>
        /// Da de alta un usuario.
        /// </summary>
        /// <param name="request">Los parámetros de la operación.</param>
        /// <returns>El resultado de la operación.</returns>
        public RegisterUserResponse RegisterUser(RegisterUserRequest request)
        {
            return AccountServices.RegisterUser(request);
        }

        /// <summary>
        /// Guarda un token de activación de cuenta.
        /// </summary>
        /// <param name="token">Token.</param>
        public void SaveAccountActivationToken(string token, int idUser)
        {
            AccountServices.SaveAccountActivationToken(token, idUser);
        }

        /// <summary>
        /// Activa una cuenta mediante el token.
        /// </summary>
        /// <param name="token">Token.</param>
        /// <returns>True si se activo la cuenta, false caso contrario.</returns>
        public bool ActivateAccount(string token)
        {
            return AccountServices.ActivateAccount(token);
        }

        /// <summary>
        /// Verifica si ya existe un usuario con ese email.
        /// </summary>
        /// <param name="request">Los parámetros de la operación.</param>
        /// <returns>El resultado de la operación.</returns>
        public ExistsEmailResponse ExistsEmail(ExistsEmailRequest request)
        {
            return AccountServices.ExistsEmail(request);
        }

        /// <summary>
        /// Indica si el usuario tiene un token de activación de cuenta activo.
        /// </summary>
        /// <param name="idUser">Identificador del usuario.</param>
        /// <returns>True si posee un token activo, false caso contrario.</returns>
        public bool HasActiveActivationToken(int idUser)
        {
            return AccountServices.HasActiveActivationToken(idUser);
        }

        /// <summary>
        /// Doy de baja los token expirados del usuario.
        /// </summary>
        /// <param name="userName">Nombre de usuario.</param>
        public void DeleteActivationToken(string userName)
        {
            AccountServices.DeleteActivationToken(userName);
        }

        /// <summary>
        /// Busca usuarios por autocompletado de nombre de usuario.
        /// </summary>
        /// <param name="request">Los parámetros de la operación.</param>
        /// <returns>El resultado de la operación.</returns>
        public SearchUsersByPartialUserNameResponse SearchUsersByPartialUserName(SearchUsersByPartialUserNameRequest request)
        {
            return AccountServices.SearchUsersByPartialUserName(request);
        }

        /// <summary>
        /// Recupera un usuario por su Id.
        /// </summary>
        /// <param name="request">Los parámetros de la operación.</param>
        /// <returns>El resultado de la operación.</returns>
        public GetUserByIdResponse GetUserById(GetUserByIdRequest request)
        {
            return AccountServices.GetUserById(request);
        }

        /// <summary>
        /// Edita el perfil de un usuario.
        /// </summary>
        /// <param name="request">Los parámetros de la operación.</param>
        /// <returns>El resultado de la operación.</returns>
        public EditProfileResponse EditProfile(EditProfileRequest request)
        {
            return AccountServices.EditProfile(request);
        }

        /// <summary>
        /// Edita el password del usuario actual.
        /// </summary>
        /// <param name="request">Los parámetros de la operación..</param>
        /// <returns>El resultado de la operación.</returns>
        public EditPasswordResponse EditPassword(EditPasswordRequest request)
        {
            return AccountServices.EditPassword(request);
        }

        /// <summary>
        /// Envía código de verificación de usuario para recuperar su contraseña.
        /// </summary>
        /// <param name="request">Los parámetros de la operación..</param>
        /// <returns>El resultado de la operación.</returns>
        public SendRecoverPasswordCodeResponse SendRecoverPasswordCode(SendRecoverPasswordCodeRequest request)
        {
            return AccountServices.SendRecoverPasswordCode(request);
        }

        /// <summary>
        /// Valida si el código de verificación para cambiar contraseña es correcto.
        /// </summary>
        /// <param name="request">Los parámetros de la operación..</param>
        /// <returns>El resultado de la operación.</returns>
        public ValidateRecoverPasswordCodeResponse ValidateRecoverPasswordCode(ValidateRecoverPasswordCodeRequest request)
        {
            return AccountServices.ValidateRecoverPasswordCode(request);
        }
    }
}
