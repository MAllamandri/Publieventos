namespace PubliEventos.Web.Mvc.Extensions
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Metodos de extensión.
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// Convierte una fecha string en DateTime.
        /// </summary>
        /// <param name="dateTime">Fecha string.</param>
        /// <returns>Fecha DateTime.</returns>
        public static DateTime? ParseStringToDateTime(this string dateTime)
        {
            DateTime date;

            var valid = DateTime.TryParseExact(dateTime, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

            if (valid)
            {
                return date;
            }

            return null;
        }
    }
}