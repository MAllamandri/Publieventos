﻿namespace PubliEventos.Contract.Contracts
{
    using PubliEventos.Contract.Class;
    using PubliEventos.Contract.Services.Account;

    /// <summary>
    /// Interface del servicio de cuentas.
    /// </summary>
    public interface IAccountServices
    {
        /// <summary>
        /// Obtiene usuario por userName.
        /// </summary>
        /// <param name="userName">userName del usuario.</param>
        /// <returns>User.</returns>
        User GetUserByUserName(string userName);

        /// <summary>
        /// Verifica si ya existe un usuario con ese email.
        /// </summary>
        /// <param name="request">Los parámetros de la operación.</param>
        /// <returns>El resultado de la operación.</returns>
        ExistsEmailResponse ExistsEmail(ExistsEmailRequest request);

        /// <summary>
        /// Da de alta un usuario.
        /// </summary>
        /// <param name="user">usuario.</param>
        int RegisterUser(User user);

        /// <summary>
        /// Guarda un token de activación de cuenta.
        /// </summary>
        /// <param name="token">Token.</param>
        void SaveAccountActivationToken(string token, int idUser);

        /// <summary>
        /// Activa una cuenta mediante el token.
        /// </summary>
        /// <param name="token">Token.</param>
        /// <returns>True si se activo la cuenta, false caso contrario.</returns>
        bool ActivateAccount(string token);

        /// <summary>
        /// Indica si el usuario tiene un token de activación de cuenta activo.
        /// </summary>
        /// <param name="idUser">Identificador del usuario.</param>
        /// <returns>True si posee un token activo, false caso contrario.</returns>
        bool HasActiveActivationToken(int idUser);

        /// <summary>
        /// Doy de baja los token expirados del usuario.
        /// </summary>
        /// <param name="userName">Nombre de usuario.</param>
        void DeleteActivationToken(string userName);

        /// <summary>
        /// Busca usuarios por autocompletado de nombre de usuario.
        /// </summary>
        /// <param name="request">Los parámetros de la operación.</param>
        /// <returns>El resultado de la operación.</returns>
        SearchUsersByPartialUserNameResponse SearchUsersByPartialUserName(SearchUsersByPartialUserNameRequest request);

        /// <summary>
        /// Recupera un usuario por su Id.
        /// </summary>
        /// <param name="request">Los parámetros de la operación.</param>
        /// <returns>El resultado de la operación.</returns>
        GetUserByIdResponse GetUserById(GetUserByIdRequest request);

        /// <summary>
        /// Edita el perfil de un usuario.
        /// </summary>
        /// <param name="request">Los parámetros de la operación..</param>
        /// <returns>El resultado de la operación.</returns>
        EditProfileResponse EditProfile(EditProfileRequest request);

        /// <summary>
        /// Edita el password del usuario actual.
        /// </summary>
        /// <param name="request">Los parámetros de la operación..</param>
        /// <returns>El resultado de la operación.</returns>
        EditPasswordResponse EditPassword(EditPasswordRequest request);

        /// <summary>
        /// Envía código de verificación de usuario para recuperar su contraseña.
        /// </summary>
        /// <param name="request">Los parámetros de la operación..</param>
        /// <returns>El resultado de la operación.</returns>
        SendRecoverPasswordCodeResponse SendRecoverPasswordCode(SendRecoverPasswordCodeRequest request);

        /// <summary>
        /// Valida si el código de verificación para cambiar contraseña es correcto.
        /// </summary>
        /// <param name="request">Los parámetros de la operación..</param>
        /// <returns>El resultado de la operación.</returns>
        ValidateRecoverPasswordCodeResponse ValidateRecoverPasswordCode(ValidateRecoverPasswordCodeRequest request);
    }
}
