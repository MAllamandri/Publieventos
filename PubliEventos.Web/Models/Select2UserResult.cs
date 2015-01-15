namespace PubliEventos.Web.Models
{
    /// <summary>
    /// Representa el resultado en el componente select2.
    /// </summary>
    public class Select2UserResult
    {
        /// <summary>
        /// Identificador del usuario.
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Nombre de usuario.
        /// </summary>
        public string text { get; set; }
    }
}