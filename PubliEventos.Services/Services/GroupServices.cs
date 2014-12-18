namespace PubliEventos.Services.Services
{
    using System;
    using NHibernate.Linq;
    using System.Linq;
    using PubliEventos.Contract.Services.Group;

    /// <summary>
    /// Servicios de grupos de usuarios.
    /// </summary>
    public class GroupServices : BaseService
    {
        /// <summary>
        /// Obtiene los grupo de un usuario.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public static GetGroupsByUserResponse GetGroupsByUser(GetGroupsByUserRequest request)
        {
            var groups = CurrentSession.Query<Domain.Domain.Group>().Where(x => (x.Administrator.Id == request.UserId
                                                                           || x.Users.Where(u => !u.NullDate.HasValue).Select(u => u.Id).Contains(request.UserId))
                                                                           && !x.NullDate.HasValue).Select(x => InternalServices.GetGroupSummary(x)).ToList();

            return new GetGroupsByUserResponse()
            {
                Groups = groups
            };
        }
    }
}
