namespace PubliEventos.Web.Mvc.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// Valida los tipos de archivos.
    /// </summary>
    public class FileTypes : ValidationAttribute
    {
        private readonly List<string> _types;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="types">Tipos de atributos a valiar.</param>
        public FileTypes(string types)
        {
            _types = types.Split(',').ToList();
        }

        /// <summary>
        /// Metodo isValid.
        /// </summary>
        /// <param name="value">Archivo.</param>
        /// <returns>True o false.</returns>
        public override bool IsValid(object value)
        {
            if (value == null) return true;

            var fileExt = System.IO.Path.GetExtension((value as HttpPostedFileBase).FileName).Substring(1);

            return _types.Contains(fileExt, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Mensaje de error.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Mensaje.</returns>
        public override string FormatErrorMessage(string name)
        {
            return string.Format("Formato de archivo no permitido");
        }
    }
}