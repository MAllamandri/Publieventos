namespace PubliEventos.Services.Services
{
    using System;
    using NHibernate.Linq;
    using System.Linq;
    using PubliEventos.Contract.Services.Group;
    using System.Transactions;

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

        /// <summary>
        /// Elimina un grupo.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public static DeleteGroupResponse DeleteGroup(DeleteGroupRequest request)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                var group = CurrentSession.Get<Domain.Domain.Group>(request.GroupId);

                foreach (var user in group.UsersGroup)
                {
                    user.NullDate = DateTime.Now;
                }

                group.NullDate = DateTime.Now;

                transaction.Complete();

                return new DeleteGroupResponse();
            }
        }

        /// <summary>
        /// Da de baja a un usuario de un grupo.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public static LeaveGroupResponse LeaveGroup(LeaveGroupRequest request)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                var group = CurrentSession.Get<Domain.Domain.Group>(request.GroupId);

                var userToDelete = group.UsersGroup.Where(x => x.UserId == request.UserId && !x.NullDate.HasValue).SingleOrDefault();

                userToDelete.NullDate = DateTime.Now;

                transaction.Complete();

                return new LeaveGroupResponse();
            }
        }
    }
}
