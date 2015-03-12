namespace PubliEventos.Web.Controllers
{
    using Microsoft.Practices.Unity;
    using PubliEventos.Contract.Class;
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Contract.Services.Comment;
    using System;
    using System.Web.Mvc;

    /// <summary>
    /// Controlador de comentarios.
    /// </summary>
    [Authorize]
    public class CommentController : BaseController
    {
        #region Properties

        /// <summary>
        /// Servicio de localidades.
        /// </summary>
        [Dependency]
        public ICommentServices serviceComments { get; set; }

        #endregion

        #region Views

        #endregion

        #region Json Methods

        /// <summary>
        /// Da de alta un comentario.
        /// </summary>
        /// <param name="model">CreateCommentRequest model.</param>
        /// <returns>True si se dio de alta correctamente, false caso contrario.</returns>
        [HttpPost]
        public JsonResult Create(CreateCommentRequest model)
        {
            model.UserId = User.Id;

            if (ModelState.IsValid)
            {
                var response = this.serviceComments.CreateComment(model);

                return Json(new { Success = true, Comment = response.Comment }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Edita un comentario.
        /// </summary>
        /// <param name="model">EditCommentRequest model.</param>
        /// <returns>True si se dio de alta correctamente, false caso contrario.</returns>
        [HttpPost]
        public JsonResult Edit(EditCommentRequest model)
        {
            if (ModelState.IsValid)
            {
                model.CurrentUserId = User.Id;

                var response = this.serviceComments.EditComment(model);

                return Json(new { Success = true, Comment = response.Comment }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Elimina un comentario.
        /// </summary>
        /// <param name="commentId">Identificador del comentario.</param>
        /// <returns>True si se elimino correctamente, false caso contrario.</returns>
        public JsonResult Delete(int commentId)
        {
            try
            {
                this.serviceComments.DeleteComment(new DeleteCommentRequest() { CommentId = commentId });

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
