namespace PubliEventos.Web.Helpers
{
    using System;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Helpers generales de la app.
    /// </summary>
    public class Shared
    {
        /// <summary>
        /// Obtiene la fecha actual para mostrar en layout.
        /// </summary>
        /// <returns>Fecha</returns>
        public static string GetDateTime()
        {
            var culture = new CultureInfo("es-AR");

            var month = culture.DateTimeFormat.GetMonthName(DateTime.Now.Month);
            var day = culture.DateTimeFormat.GetDayName(DateTime.Now.DayOfWeek);

            var dayDescription = day.First().ToString().ToUpper() + day.Substring(1);
            var monthDescription = month.First().ToString().ToUpper() + month.Substring(1);

            return string.Format("HOY! {0} {1} de {2}", dayDescription, DateTime.Now.Day, monthDescription);
        }
    }
}