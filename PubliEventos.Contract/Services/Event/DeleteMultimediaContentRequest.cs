namespace PubliEventos.Contract.Services.Event
{
    /// <summary>
    /// Representa los parámetros de la operación DeleteMultimediaContent.
    /// </summary>
    public class DeleteMultimediaContentRequest
    {
        /// <summary>
        /// Identificador del evento.
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// Identificador del archivo.
        /// </summary>
        public string FileName { get; set; }
    }
}
