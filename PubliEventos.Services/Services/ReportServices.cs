namespace PubliEventos.Services.Services
{
    using NHibernate.Linq;
    using PubliEventos.Contract.Enums;
    using PubliEventos.Contract.Services.Report;
    using PubliEventos.DataAccess.Querys;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;

    /// <summary>
    /// Servicios de reportes.
    /// </summary>
    public class ReportServices : BaseService
    {
        #region Public Methods

        /// <summary>
        /// Reporta un contenido.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public static ReportContentResponse ReportContent(ReportContentRequest request)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                var report = new Domain.Domain.Report();

                report.User = CurrentSession.Get<Domain.Domain.User>(request.UserId);
                report.Reason = request.Reason;
                report.EffectDate = DateTime.Now;

                report.Comment = request.ContentType == (int)ContentTypes.Comment ?
                                 CurrentSession.Get<Domain.Domain.Comment>(Convert.ToInt32(request.ContentId)) : null;
                report.Event = request.ContentType == (int)ContentTypes.Event ?
                                 CurrentSession.Get<Domain.Domain.Event>(Convert.ToInt32(request.ContentId)) : null;

                if (request.ContentType == (int)ContentTypes.Image || request.ContentType == (int)ContentTypes.Movie)
                {
                    report.MultimediaContent = CurrentSession.Query<Domain.Domain.MultimediaContent>().Where(x => x.Name.Equals(request.ContentId) && !x.NullDate.HasValue && x.Event.Id == request.EventId).Single();
                }

                new BaseQuery<Domain.Domain.Report, int>().Create(report);

                transaction.Complete();

                return new ReportContentResponse();
            }
        }

        /// <summary>
        /// Evalua la cantidad de reportes de un contenido, para ver su tratamiento.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public static EvaluateReportsForDisabledResponse EvaluateReportsForDisabled(EvaluateReportsForDisabledRequest request)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                var countReportsForDisabled = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["QuantityReports"]);
                int reportsQuantity;
                var isDisabled = false;

                if (request.ContentType == (int)ContentTypes.Comment)
                {
                    reportsQuantity = CurrentSession.Query<Domain.Domain.Report>().Where(x => x.Comment.Id == Convert.ToInt32(request.ContentId) && !x.IsReported.HasValue && !x.NullDate.HasValue).Count();

                    if (reportsQuantity >= countReportsForDisabled)
                    {
                        //Deshabilito el contenido.
                        var comment = CurrentSession.Get<Domain.Domain.Comment>(Convert.ToInt32(request.ContentId));
                        comment.Active = false;

                        isDisabled = true;
                    }
                }

                if (request.ContentType == (int)ContentTypes.Event)
                {
                    reportsQuantity = CurrentSession.Query<Domain.Domain.Report>().Where(x => x.Event.Id == Convert.ToInt32(request.ContentId) && !x.IsReported.HasValue && !x.NullDate.HasValue).Count();

                    if (reportsQuantity >= countReportsForDisabled)
                    {
                        //Deshabilito el contenido.
                        var _event = CurrentSession.Get<Domain.Domain.Event>(Convert.ToInt32(request.ContentId));
                        _event.Active = false;

                        isDisabled = true;
                    }
                }

                if (request.ContentType == (int)ContentTypes.Image || request.ContentType == (int)ContentTypes.Movie)
                {
                    var multimediaContent = CurrentSession.Query<Domain.Domain.MultimediaContent>().Where(x => x.Name.Equals(request.ContentId) && !x.NullDate.HasValue && x.Event.Id == request.EventId).Single();

                    reportsQuantity = CurrentSession.Query<Domain.Domain.Report>().Where(x => x.MultimediaContent.Id == multimediaContent.Id && !x.IsReported.HasValue && !x.NullDate.HasValue).Count();

                    if (reportsQuantity >= countReportsForDisabled)
                    {
                        //Deshabilito el contenido.
                        multimediaContent.Active = false;

                        isDisabled = true;
                    }
                }

                transaction.Complete();

                return new EvaluateReportsForDisabledResponse()
                {
                    IsDisabled = isDisabled
                };
            }
        }

        /// <summary>
        /// Obtiene los contenidos reportados por los usuarios.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public static SearchReportedContentsResponse SearchReportedContents(SearchReportedContentsRequest request)
        {
            var countReportsForDisabled = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["QuantityReports"]);

            var reports = CurrentSession.Query<Domain.Domain.Report>().Where(x => !x.IsReported.HasValue && !x.NullDate.HasValue).ToList();

            var events = reports.Where(x => x.Event != null)
                        .GroupBy(x => x.Event.Id)
                        .Where(x => x.Count() >= countReportsForDisabled)
                        .Select(x => InternalServices.GetEventSummary(x.First().Event))
                        .ToList();

            var comments = reports.Where(x => x.Comment != null)
                        .GroupBy(x => x.Comment.Id)
                        .Where(x => x.Count() >= countReportsForDisabled)
                        .Select(x => InternalServices.GetCommentSummary(x.First().Comment, request.CurrentUserId))
                        .ToList();

            var pictures = reports.Where(x => x.MultimediaContent != null && x.MultimediaContent.ContentType == (int)ContentTypes.Image)
                        .GroupBy(x => x.MultimediaContent.Id)
                        .Where(x => x.Count() >= countReportsForDisabled)
                        .Select(x => InternalServices.GetMultimediaContentSummary(x.First().MultimediaContent))
                        .ToList();

            var movies = reports.Where(x => x.MultimediaContent != null && x.MultimediaContent.ContentType == (int)ContentTypes.Movie)
                        .GroupBy(x => x.MultimediaContent.Id)
                        .Where(x => x.Count() >= countReportsForDisabled)
                        .Select(x => InternalServices.GetMultimediaContentSummary(x.First().MultimediaContent))
                        .ToList();

            return new SearchReportedContentsResponse()
            {
                Events = events,
                Comments = comments,
                Pictures = pictures,
                Movies = movies
            };
        }

        /// <summary>
        /// Administra contenidos reportados.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public static AdministrationReportedResponse AdministrationReported(AdministrationReportedRequest request)
        {
            int? reportedUserId = null;
            List<int> ignoredReportedUserId = new List<int>();

            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                var reports = new List<Domain.Domain.Report>();

                if (request.ContentType == (int)ContentTypes.Event)
                {
                    var _event = CurrentSession.Get<Domain.Domain.Event>(Convert.ToInt32(request.ContentId));

                    _event.Active = request.IsDisabled ? false : true;

                    reportedUserId = _event.User.Id;

                    // Busco los reportes relacionados.
                    reports = CurrentSession.Query<Domain.Domain.Report>()
                                .Where(x => x.Event != null && x.Event.Id == _event.Id && !x.NullDate.HasValue)
                                .ToList();
                }

                if (request.ContentType == (int)ContentTypes.Comment)
                {
                    var comment = CurrentSession.Get<Domain.Domain.Comment>(Convert.ToInt32(request.ContentId));

                    comment.Active = request.IsDisabled ? false : true;

                    reportedUserId = comment.User.Id;

                    // Busco los reportes relacionados.
                    reports = CurrentSession.Query<Domain.Domain.Report>()
                                .Where(x => x.Comment != null && x.Comment.Id == comment.Id && !x.NullDate.HasValue)
                                .ToList();
                }

                if (request.ContentType == (int)ContentTypes.Image || request.ContentType == (int)ContentTypes.Movie)
                {
                    var multimediaContent = CurrentSession.Query<Domain.Domain.MultimediaContent>().Where(x => x.Name == request.ContentId && x.Event.Id == request.EventId).Single();

                    multimediaContent.Active = request.IsDisabled ? false : true;

                    reportedUserId = multimediaContent.Event.User.Id;

                    // Busco los reportes relacionados.
                    reports = CurrentSession.Query<Domain.Domain.Report>()
                                .Where(x => x.MultimediaContent != null && x.MultimediaContent.Id == multimediaContent.Id && !x.NullDate.HasValue)
                                .ToList();
                }

                if (!request.IsDisabled)
                {
                    ignoredReportedUserId.AddRange(reports.Select(x => x.User.Id));
                }

                // Marco el reporte.
                foreach (var report in reports)
                {
                    report.IsReported = request.IsDisabled ? true : false;
                }

                transaction.Complete();
            }

            if (reportedUserId.HasValue)
            {
                if (request.IsDisabled)
                {
                    DisabledUserWithContentsReported(reportedUserId.Value);
                }
                else
                {
                    DisabledUserWithContentsIgnored(ignoredReportedUserId);
                }
            }

            return new AdministrationReportedResponse();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Verifica la condición del usuario para deshabilitarlo o suspenderlo si es necesario.
        /// </summary>
        /// <param name="userId">Identificador del usuario.</param>
        private static void DisabledUserWithContentsReported(int userId)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                // Busco los contenidos reportados del usuario.
                var eventsReported = CurrentSession.Query<Domain.Domain.Report>()
                                     .Where(x => x.IsReported.Value == true && x.Event != null && x.Event.User.Id == userId && !x.NullDate.HasValue)
                                     .ToList();

                var commentsReported = CurrentSession.Query<Domain.Domain.Report>()
                                      .Where(x => x.IsReported.Value == true && x.Comment != null && x.Comment.User.Id == userId && !x.NullDate.HasValue)
                                      .ToList();

                var contentsReported = CurrentSession.Query<Domain.Domain.Report>()
                                     .Where(x => x.IsReported.Value == true && x.MultimediaContent != null && x.MultimediaContent.Event.User.Id == userId && !x.NullDate.HasValue)
                                     .ToList();

                // Cantidad de contenidos reportados.
                var quantityReports = contentsReported.GroupBy(x => x.MultimediaContent.Id).Count();
                quantityReports += commentsReported.GroupBy(x => x.Comment.Id).Count();
                quantityReports += eventsReported.GroupBy(x => x.Event.Id).Count();

                if (quantityReports >= ContentsReports)
                {
                    DisabledUser(userId);

                    //Los doy de baja para indicar que ya los procese.
                    foreach (var content in contentsReported)
                    {
                        content.NullDate = DateTime.Now;
                    }

                    foreach (var comment in commentsReported)
                    {
                        comment.NullDate = DateTime.Now;
                    }

                    foreach (var _event in eventsReported)
                    {
                        _event.NullDate = DateTime.Now;
                    }
                }

                transaction.Complete();
            }
        }

        /// <summary>
        /// Deshabilita o suspende a usuarios con muchos reportes ignorados, si es necesario.
        /// </summary>
        /// <param name="userIds">Identificadores de los usuarios que ignoraron el contenido.</param>
        private static void DisabledUserWithContentsIgnored(List<int> userIds)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                foreach (var userId in userIds.Select(x => x).Distinct())
                {
                    // Busco los contenidos ignorados de ese usuario.
                    var reportsIgnored = CurrentSession.Query<Domain.Domain.Report>()
                     .Where(x => x.IsReported.HasValue && x.IsReported.Value == false && x.User.Id == userId && !x.NullDate.HasValue)
                     .ToList();

                    if (reportsIgnored.Count() >= ContentsReports)
                    {
                        DisabledUser(userId);

                        foreach (var report in reportsIgnored)
                        {
                            report.NullDate = DateTime.Now;
                        }
                    }
                }

                transaction.Complete();
            }
        }

        /// <summary>
        /// Deshabilita o suspende un usuario.
        /// </summary>
        /// <param name="userId">Identificador del usuario.</param>
        private static void DisabledUser(int userId)
        {
            var user = CurrentSession.Get<Domain.Domain.User>(userId);

            // Si el usuario ya tiene una suspension, lo deshabilito.
            if (user.Suspensions.Count() >= 1)
            {
                user.Active = false;

                new BaseQuery<Domain.Domain.User, int>().Update(user);

                //Envío el mail notificando al usuario que fue desactivado.
                var subject = "Publieventos - Usuario desactivado";
                var body = string.Format("Estimado/a {0}:" +
                    "<br/><br/>Lamentamos comunicarle que su usuario <strong>{1}</strong> ha sido desactivado debido a que sus acciones o contenidos en el sitio no fueron los adecuados." +
                    "<br/><br/>Saludos cordiales." +
                    "<br/>Equipo de administración de Publieventos", user.FirstName, user.UserName);

                InternalServices.SendMail(user.Email, subject, body, true);
            }
            else
            {
                // Deshabilito al usuario por 3 meses.
                var suspension = new Domain.Domain.Suspension();
                suspension.EffectDate = DateTime.Now.Date;
                suspension.EndDate = DateTime.Now.Date.AddMonths(3);
                suspension.User = user;

                new BaseQuery<Domain.Domain.Suspension, int>().Create(suspension);

                //Envío el mail notificando al usuario que fue desactivado.
                var subject = "Publieventos - Usuario deshabilitado temporalmente";
                var body = string.Format("Estimado/a {0}:" +
                    "<br/><br/>Lamentamos comunicarle que su usuario <strong>{1}</strong> ha sido desactivado por un período de 3 meses debido a que sus acciones o contenidos en el sitio no fueron los adecuados." +
                    "<br/><br/>Nos vemos pronto. Saludos cordiales." +
                    "<br/>Equipo de administración de Publieventos", user.FirstName, user.UserName);

                InternalServices.SendMail(user.Email, subject, body, true);
            }

        }

        #endregion
    }
}
