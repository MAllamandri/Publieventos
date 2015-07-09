namespace PubliEventos.Services.Services
{
    using LinqKit;
    using NHibernate.Linq;
    using PubliEventos.Contract.Services.Group;
    using PubliEventos.DataAccess.Querys;
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
                                                                           || x.UsersGroup.Where(u => u.UserId == request.UserId && u.Active.HasValue && u.Active.Value == true).Any())
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

        /// <summary>
        /// Crea un grupo.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public static CreateGroupResponse CreateGroup(CreateGroupRequest request)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                var group = new Domain.Domain.Group()
                {
                    Administrator = CurrentSession.Get<Domain.Domain.User>(request.AdministratorId),
                    EffectDate = DateTime.Now,
                    Name = request.GroupName,
                    Message = request.Message
                };

                new BaseQuery<Domain.Domain.Group, int>().Create(group);

                foreach (var user in request.UserIds.Split(','))
                {
                    if (request.AdministratorId != Convert.ToInt32(user))
                    {
                        var userGroup = new Domain.Domain.UsersGroup();
                        userGroup.GroupId = group.Id;
                        userGroup.UserId = Convert.ToInt32(user);
                        userGroup.EffectDate = DateTime.Now;

                        new BaseQuery<Domain.Domain.UsersGroup, int>().Create(userGroup);
                    }
                }

                transaction.Complete();

                return new CreateGroupResponse()
                {
                    GroupId = group.Id
                };
            }
        }

        /// <summary>
        /// Obtiene un grupo por Id.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public static GetGroupByIdResponse GetGroupById(GetGroupByIdRequest request)
        {
            var group = CurrentSession.Query<Domain.Domain.Group>().Where(x => x.Id == request.GroupId && !x.NullDate.HasValue).Select(x => InternalServices.GetGroupSummary(x)).SingleOrDefault();

            if (group == null)
            {
                throw new Exception("El grupo no existe o fue dado de baja");
            }

            return new GetGroupByIdResponse()
            {
                Group = group
            };
        }

        /// <summary>
        /// Edita un grupo.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public static EditGroupResponse EditGroup(EditGroupRequest request)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                var group = CurrentSession.Get<Domain.Domain.Group>(request.GroupId);

                group.Message = request.Message;
                group.Name = request.GroupName;


                var userIds = request.UserIds.Split(',');

                // Doy de baja los usuarios que fueron eliminados del grupo.
                foreach (var user in group.UsersGroup.Where(x => !userIds.Contains(x.UserId.ToString())))
                {
                    user.NullDate = DateTime.Now;

                    // Busco las invitaciones pendientes que tenga el usuario al grupo y las doy de baja.
                    var invitations = CurrentSession.Query<Domain.Domain.Invitation>()
                                        .Where(x => x.Group.Id == group.Id && x.User.Id == user.UserId && !x.Confirmed.HasValue && !x.NullDate.HasValue)
                                        .ToList();

                    foreach (var invitation in invitations)
                    {
                        invitation.NullDate = DateTime.Now;
                    }
                }

                var idsToSendInvitation = new List<int>();

                foreach (var user in userIds)
                {
                    if (!group.UsersGroup.Where(x => x.UserId == Convert.ToInt32(user)).Any())
                    {
                        if (group.Administrator.Id != Convert.ToInt32(user))
                        {
                            var userGroup = new Domain.Domain.UsersGroup();
                            userGroup.GroupId = group.Id;
                            userGroup.UserId = Convert.ToInt32(user);
                            userGroup.EffectDate = DateTime.Now;

                            new BaseQuery<Domain.Domain.UsersGroup, int>().Create(userGroup);

                            idsToSendInvitation.Add(Convert.ToInt32(user));
                        }
                    }
                }

                transaction.Complete();
                transaction.Dispose();

                return new EditGroupResponse
                {
                    UserIdsToSendInvitation = idsToSendInvitation
                };
            }
        }

        /// <summary>
        /// Obtiene grupos por su nombre.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public static SearchGroupsByPartialNameResponse SearchGroupsByPartialName(SearchGroupsByPartialNameRequest request)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                var predicate = PredicateBuilder.True<Domain.Domain.Group>();
                predicate = predicate.And(x => !x.NullDate.HasValue && x.Name.ToLower().Contains(request.Name.ToLower()));

                // Busco donde el usuario sea administrador o pertenezca al grupo.
                predicate = predicate.And(x => x.Administrator.Id == request.UserId || x.Users.Where(u => u.Active).Select(u => u.Id).Contains(request.UserId));

                var total = CurrentSession.Query<Domain.Domain.Group>().Where(predicate).Count();

                var groups = CurrentSession.Query<Domain.Domain.Group>()
                     .Where(predicate)
                     .Skip(request.PageNumber - 1)
                     .Take(request.PageSize)
                     .Select(u => InternalServices.GetGroupSummary(u)).ToList();

                return new SearchGroupsByPartialNameResponse()
                {
                    Groups = groups,
                    Quantity = total
                };
            }
        }
    }
}
