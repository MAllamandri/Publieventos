namespace PubliEventos.Domain.Domain
{
    using System;
    using PubliEventos.DataAccess.Infrastructure;

    public class AccountActivationToken : BaseIdentifier<int>
    {
        /// <summary>
        /// Fecha de expiración del token.
        /// </summary>
        public virtual DateTime ExpirationDate { get; set; }

        /// <summary>
        /// Token de activación.
        /// </summary>
        public virtual string Token { get; set; }

        /// <summary>
        /// Usuario al que corresponde el token.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Fecha de alta.
        /// </summary>
        public virtual DateTime EffectDate { get; set; }

        /// <summary>
        /// Fecha de baja.
        /// </summary>
        public virtual DateTime NullDate { get; set; }
    }
}
