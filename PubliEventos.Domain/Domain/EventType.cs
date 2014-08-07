namespace PubliEventos.Domain.Domain
{
    using PubliEventos.DataAccess.Infrastructure;

    /// <summary>
    /// Representa un tipo de evento.
    /// </summary>
    public class EventType : BaseIdentifier<int>
    {
        /// <summary>
        /// Description del tipo de evento.
        /// </summary>
        public virtual string Description { get; set; }
    }
}
