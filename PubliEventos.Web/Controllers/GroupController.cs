namespace PubliEventos.Web.Controllers
{
    using Microsoft.Practices.Unity;
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Contract.Enums;
    using PubliEventos.Contract.Services.Group;
    using PubliEventos.Contract.Services.Invitation;
    using PubliEventos.Web.App_Start;
    using PubliEventos.Web.Helpers;
    using PubliEventos.Web.Models;
    using PubliEventos.Web.Mvc.Filters;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// Controlador de grupos.
    /// </summary>
    [Authorize]
    public class GroupController : BaseController
    {
        #region Properties

        /// <summary>
        /// Servicio de grupos.
        /// </summary>
        [Dependency]
        public IGroupServices serviceGroups { get; set; }

        /// <summary>
        /// Servicio de invitaciones.
        /// </summary>
        [Dependency]
        public IInvitationServices servicesInvitations { get; set; }

        #endregion

        #region Views

        /// <summary>
        /// Vista de los grupos de un usuario.
        /// </summary>
        /// <returns></returns>
        public ActionResult MyGroups()
        {
            ViewBag.groups = this.serviceGroups.GetGroupsByUser(new GetGroupsByUserRequest() { UserId = User.Id }).Groups;

            return View();
        }

        /// <summary>
        /// Vista de creación de grupos.
        /// </summary>
        /// <returns>Create View.</returns>
        public ActionResult Create()
        {
            var model = new CreateGroupRequest() { AdministratorId = User.Id };

            return View(model);
        }

        /// <summary>
        /// Vista de edición de grupos.
        /// </summary>
        /// <param name="groupId">Identificador del grupo.</param>
        /// <returns>Edit View.</returns>
        [UserActionRestriction(ValidateCondition.Group)]
        public ActionResult Edit(int id)
        {
            var group = this.serviceGroups.GetGroupById(new GetGroupByIdRequest() { GroupId = id }).Group;

            var model = new EditGroupRequest()
            {
                GroupId = group.Id.Value,
                AdministratorId = group.Administrator.Id.Value,
                GroupName = group.Name,
                Message = group.Message,
                UserIds = string.Join(",", group.Users.Select(x => x.Id).ToArray()),
                UserNames = string.Join(",", group.Users.Select(x => x.UserName).ToArray())
            };

            return View(model);
        }

        /// <summary>
        /// Detalle del grupo.
        /// </summary>
        /// <param name="id">Identificador del grupo.</param>
        /// <returns>Detail view.</returns>
        [UserActionRestriction(ValidateCondition.Group)]
        public ActionResult Detail(int id)
        {
            var group = this.serviceGroups.GetGroupById(new GetGroupByIdRequest() { GroupId = id }).Group;
            ViewBag.messages = this.serviceGroups.SearchChatMessagesByGroup(new SearchChatMessagesByGroupRequest() { GroupId = id }).Messages;

            return View(group);
        }

        #endregion

        #region Json Methods

        /// <summary>
        /// Vista de creación de grupos.
        /// </summary>
        /// <param name="model">CreateGroupRequest model.</param>
        /// <returns>Create o MyGroups view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(CreateGroupRequest model)
        {
            if (ModelState.IsValid)
            {
                var response = this.serviceGroups.CreateGroup(model);

                var userIds = model.UserIds.Split(',');
                var ids = userIds.Where(x => Convert.ToInt32(x) != model.AdministratorId).Select(x => Convert.ToInt32(x)).ToList();

                //Mando las invitaciones.
                this.servicesInvitations.CreateInvitation(new CreateInvitationRequest()
                {
                    GroupId = response.GroupId,
                    UserIds = ids
                });

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = false, Errors = ModelErrors.GetModelErrors(ModelState) }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Edita un grupo.
        /// </summary>
        /// <param name="model">EditGroupRequest model.</param>
        /// <returns>Edit view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(EditGroupRequest model)
        {
            if (ModelState.IsValid)
            {
                var ids = this.serviceGroups.EditGroup(model).UserIdsToSendInvitation;

                if (ids.Any())
                {
                    //Mando las invitaciones a los nuevos usuarios.
                    this.servicesInvitations.CreateInvitation(new CreateInvitationRequest()
                    {
                        GroupId = model.GroupId,
                        UserIds = ids
                    });
                }

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = false, Errors = ModelErrors.GetModelErrors(ModelState) }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Elimina un grupo.
        /// </summary>
        /// <param name="groupId">Identificador del grupo.</param>
        /// <returns>True si se elimino correctamente, false caso contrario.</returns>
        public JsonResult Delete(int groupId)
        {
            try
            {
                this.serviceGroups.DeleteGroup(new DeleteGroupRequest() { GroupId = groupId });

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Da de baja un usuario de un grupo.
        /// </summary>
        /// <param name="groupId">Identificador del grupo.</param>
        /// <param name="userId">Identificador del usuario.</param>
        /// <returns></returns>
        public JsonResult LeaveGroup(int groupId, int? userId)
        {
            try
            {
                this.serviceGroups.LeaveGroup(new LeaveGroupRequest() { GroupId = groupId, UserId = !userId.HasValue ? User.Id : userId.Value });

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Busca grupo por autocompletado de su nombre.
        /// </summary>
        /// <param name="groupName">Nombre de grupo.</param>
        /// <param name="pageNumber">Número de página.</param>
        /// <param name="pageSize">Tamaño de página.</param>
        /// <returns>Grupos encontrados.</returns>
        public JsonResult SearchGroupsByName(string groupName, int pageNumber, int pageSize)
        {
            List<Select2Result> Groups = new List<Select2Result>();

            var response = this.serviceGroups.SearchGroupsByPartialName(new SearchGroupsByPartialNameRequest()
            {
                Name = groupName,
                PageNumber = pageNumber,
                PageSize = pageSize,
                UserId = User.Id
            });

            foreach (var group in response.Groups)
            {
                var groupResult = new Select2Result();
                groupResult.id = group.Id.Value;
                groupResult.text = group.Name;

                Groups.Add(groupResult);
            }

            return Json(new { Groups = Groups, Quantity = response.Quantity }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
