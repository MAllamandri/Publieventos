namespace PubliEventos.Contract.Class
{
    using System.Collections.Generic;

    /// <summary>
    /// Representa un contenido multimedia.
    /// </summary>
    public class MultimediaContent
    {
        /// <summary>
        /// Identificador del archivo.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Indica el tipo de contenido.
        /// </summary>
        public int ContentType { get; set; }

        /// <summary>
        /// Reportes que recibio.
        /// </summary>
        public List<Report> Reports { get; set; }
    }
}
