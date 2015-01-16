namespace PubliEventos.Web.Controllers
{
    using Microsoft.Practices.Unity;
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Contract.Services.Group;
    using PubliEventos.Web.Helpers;
    using System;
    using System.Web.Mvc;
    using System.Linq;
    using PubliEventos.Contract.Services.Invitation;

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
        public ActionResult Edit(int id)
        {
            var group = this.serviceGroups.GetGroupById(new GetGroupByIdRequest() { GroupId = id }).Group;

            // Si no es el usuario creador, lo llevo a la página de error.
            if (group.Administrator.Id != User.Id)
            {
                return RedirectToAction("UnauthorizedAccess", "Error");
            }

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
        public ActionResult Detail(int id)
        {
            var group = this.serviceGroups.GetGroupById(new GetGroupByIdRequest() { GroupId = id }).Group;

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

                //Mando las invitaciones.
                foreach (var user in model.UserIds.Split(','))
                {
                    if (model.AdministratorId != Convert.ToInt32(user))
                    {
                        this.servicesInvitations.CreateInvitation(new CreateInvitationRequest()
                        {
                            GroupId = response.GroupId,
                            UserId = Convert.ToInt32(user)
                        });
                    }
                }

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
                var group = this.serviceGroups.GetGroupById(new GetGroupByIdRequest() { GroupId = model.GroupId }).Group;

                this.serviceGroups.EditGroup(model);

                //Mando las invitaciones a los nuevos usuarios.
                foreach (var user in model.UserIds.Split(','))
                {
                    if (model.AdministratorId != Convert.ToInt32(user) && !group.Users.Select(x => x.Id).ToArray().Contains(Convert.ToInt32(user)))
                    {
                        this.servicesInvitations.CreateInvitation(new CreateInvitationRequest()
                        {
                            GroupId = model.GroupId,
                            UserId = Convert.ToInt32(user)
                        });
                    }
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
        public JsonResult LeaveGroup(int groupId)
        {
            try
            {
                this.serviceGroups.LeaveGroup(new LeaveGroupRequest() { GroupId = groupId, UserId = User.Id });

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}
