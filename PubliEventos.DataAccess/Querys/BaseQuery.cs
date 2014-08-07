namespace PubliEventos.DataAccess.Querys
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;
    using NHibernate;
    using NHibernate.Linq;
    using PubliEventos.DataAccess.Infrastructure;

    /// <summary>
    /// Clase que contiene las querys base.
    /// </summary>
    /// <typeparam name="TEntity">Tipo de entidad.</typeparam>
    /// <typeparam name="TIdentifier">Identificador de la entidad.</typeparam>
    public class BaseQuery<TEntity, TIdentifier>
        where TIdentifier : new()
        where TEntity : BaseIdentifier<TIdentifier>
    {
        /// <summary>
        /// Sesión de nhibernate.
        /// </summary>
        protected ISession CurrentSession { get; set; }

        /// <summary>
        /// Variable de transacción.
        /// </summary>
        TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required);

        /// <summary>
        /// Constructor.
        /// </summary>
        public BaseQuery()
        {
            var sessionHelper = new SessionHelper();
            CurrentSession = sessionHelper.Current;
        }

        /// <summary>
        /// Busca una entidad por su identificador.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>Objeto entidad.</returns>
        public TEntity LoadById(TIdentifier id)
        {
            TEntity entity = CurrentSession.Get<TEntity>(id);
            return entity;
        }

        /// <summary>
        /// Crea un objeto entidad.
        /// </summary>
        /// <param name="entity">Entidad a crear.</param>
        /// <returns>Identificador de la entidad creada.</returns>
        public TIdentifier Create(TEntity entity)
        {
            var identifier = new TIdentifier();
            using (transaction)
            {
                identifier = (TIdentifier)CurrentSession.Save(entity);
                transaction.Complete();
            }
            return identifier;
        }

        /// <summary>
        ///Guarda o actualiza una entidad.
        /// </summary>
        /// <param name="entity">Entidad a guardar o actualizar.</param>
        public void SaveOrUpdate(TEntity entity)
        {
            var identifier = new TIdentifier();
            using (transaction)
            {
                CurrentSession.SaveOrUpdate(entity);
                transaction.Complete();
            }
        }

        /// <summary>
        /// Modifica una entidad.
        /// </summary>
        /// <param name="entity">Entidad a modificar.</param>
        public void Update(TEntity entity)
        {
            using (transaction)
            {
                CurrentSession.Update(entity);
                CurrentSession.Flush();
                transaction.Complete();
            }
        }

        /// <summary>
        /// Elimina un objeto entidad.
        /// </summary>
        /// <param name="entity">Objeto entidad.</param>
        public void Delete(TEntity entity)
        {
            using (transaction)
            {
                CurrentSession.Delete(entity);
                transaction.Complete();
            }
        }

        /// <summary>
        /// Elimina un objeto entidad por su Id.
        /// </summary>
        /// <param name="entityIdentifier">Identificador del objeto.</param>
        public void DeleteById(TIdentifier entityIdentifier)
        {
            using (transaction)
            {
                TEntity entity = LoadById(entityIdentifier);
                CurrentSession.Delete(entity);
                transaction.Complete();
            }
        }

        /// <summary>
        /// Devuelve todas los objetos de una tabla.
        /// </summary>
        /// <returns>Lista de objetos.</returns>
        public IList<TEntity> LoadAll()
        {
            return CurrentSession.Query<TEntity>().ToList();
        }
    }
}
