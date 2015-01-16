﻿namespace PubliEventos.Web.Controllers
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

        /// <summary>
        /// Busca los comentarios de un evento.
        /// </summary>
        /// <param name="eventId">Identificador del evento.</param>
        /// <returns>Comentarios.</returns>
        [HttpPost]
        public PartialViewResult GetComment(string detail, int commentId, string imageProfile, string elapsedTime, int userId, string userName)
        {
            var comment = new Comment
            {
                Detail = detail,
                Id = commentId,
                User = new User
                {
                    ImageProfile = imageProfile,
                    Id = userId,
                    UserName = userName
                },
                ElapsedTime = elapsedTime
            };

            return PartialView("Partial/_CommentBubble", comment);
        }

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