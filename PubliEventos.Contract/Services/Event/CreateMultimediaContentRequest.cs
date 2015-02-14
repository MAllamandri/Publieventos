namespace PubliEventos.Contract.Services.Event
{
    /// <summary>
    /// Representa los parámetros de entrada de la operación CreateMultimediaContent.
    /// </summary>
    public class CreateMultimediaContentRequest
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
