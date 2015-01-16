namespace PubliEventos.Services.Services
{
    using System;
    using NHibernate.Linq;
    using System.Linq;
    using PubliEventos.Contract.Services.Group;
    using System.Transactions;
    using System.Collections.Generic;
    using PubliEventos.DataAccess.Querys;

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
                        userGroup.GroupId = group.Id;

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

                // Doy de baja los anteriores usuarios.
                foreach (var user in group.UsersGroup)
                {
                    user.NullDate = DateTime.Now;
                }

                foreach (var user in request.UserIds.Split(','))
                {
                    if (group.Administrator.Id != Convert.ToInt32(user))
                    {
                        var userGroup = new Domain.Domain.UsersGroup();
                        userGroup.GroupId = group.Id;
                        userGroup.UserId = Convert.ToInt32(user);
                        userGroup.EffectDate = DateTime.Now;
                        userGroup.GroupId = group.Id;

                        new BaseQuery<Domain.Domain.UsersGroup, int>().Create(userGroup);
                    }
                }

                transaction.Complete();

                return new EditGroupResponse();
            }
        }
    }
}
