namespace PubliEventos.Contract.Services.Event
{
    /// <summary>
    /// Representa los parámetros de entrada de la operación SearchEventsByDistance.
    /// </summary>
    public class SearchEventsByDistanceRequest
    {
        /// <summary>
        /// Latitud inicial.
        /// </summary>
        public string LatitudeInitial { get; set; }

        /// <summary>
        /// Longitud inicial.
        /// </summary>
        public string LongitudeInitial { get; set; }

        /// <summary>
        /// Distancia máxima a buscar.
        /// </summary>
        public int MaxDistance { get; set; }
    }
}
