namespace PubliEventos.Contract.Services.Event
{
    /// <summary>
    /// Representa los parámetros de la operación ValidateExistsContent.
    /// </summary>
    public class ValidateExistsContentRequest
    {
        /// <summary>
        /// Identificador del evento.
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// Identificador del contenido.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Tipo de contenido.
        /// </summary>
        public int ContentType { get; set; }
    }
}
