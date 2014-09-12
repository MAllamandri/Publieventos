namespace PubliEventos.Contract.Class
{
    public class Locality
    {
        /// <summary>
        /// Identificador de la localidad.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre de la localidad.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Provincia de la localidad.
        /// </summary>
        public Province Province { get; set; }

        /// <summary>
        /// Latitud de la localidad.
        /// </summary>
        public float Latitude { get; set; }

        /// <summary>
        /// Longitud de la localidad.
        /// </summary>
        public float Longitude { get; set; }
    }
}
