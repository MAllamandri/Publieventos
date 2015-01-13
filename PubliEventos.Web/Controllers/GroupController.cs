namespace PubliEventos.Web.Controllers
{
    using Microsoft.Practices.Unity;
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Contract.Services.Group;
    using System;
    using System.Web.Mvc;

    /// <summary>
    /// Controlador de grupos.
    /// </summary>
    [Authorize]
    public class GroupController : BaseController
    {
        #region Properties

        /// <summary>
        /// Servicio de localidades.
        /// </summary>
        [Dependency]
        public IGroupServices serviceGroups { get; set; }

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
            return View();
        }
        #endregion

        #region Json Methods

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
