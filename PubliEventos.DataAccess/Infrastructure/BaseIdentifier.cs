namespace PubliEventos.DataAccess.Infrastructure
{
    /// <summary>
    /// Clase que contiene el identificador de un objeto.
    /// </summary>
    /// <typeparam name="TIdentifier">Tipo de objeto.</typeparam>
    public class BaseIdentifier<TIdentifier> where TIdentifier : new()
    {
        /// <summary>
        ///Identificador del objeto.
        /// </summary>
        public virtual TIdentifier Id { get; set; }
    }
}
