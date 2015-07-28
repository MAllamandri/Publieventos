namespace PubliEventos.Contract.Services.Account
{
    using PubliEventos.Contract.Class;

    /// <summary>
    /// Representa los parámetros de la operación RegisterUser.
    /// </summary>
    public class RegisterUserRequest
    {
        public User User { get; set; }
    }
}
