namespace PubliEventos.Web.Models
{
    using PubliEventos.Web.Attributes;
    using System.Web;

    /// <summary>
    /// Modelo base para subir archivos.
    /// </summary>
    public class FileModel
    {
        /// <summary>
        /// Archivo.
        /// </summary>
        [FileTypes("gif,jpg,jpeg,png")]
        public HttpPostedFileBase File { get; set; }
    }
}