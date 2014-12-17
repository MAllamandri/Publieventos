namespace PubliEventos.Web.Controllers
{
    using Microsoft.Practices.Unity;
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Contract.Services.Comment;
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
        public ICommentServices commentService { get; set; }

        #endregion

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
                this.commentService.CreateComment(model);

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
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
                this.commentService.EditComment(model);

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        }
    }
}
