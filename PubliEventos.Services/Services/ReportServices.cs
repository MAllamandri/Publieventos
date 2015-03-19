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
                    report.MultimediaContent = CurrentSession.Query<Domain.Domain.MultimediaContent>().Where(x => x.Name.Equals(request.ContentId)).Single();
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
                    reportsQuantity = CurrentSession.Query<Domain.Domain.Report>().Where(x => x.Comment.Id == Convert.ToInt32(request.ContentId) && !x.IsReported.HasValue).Count();

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
                    reportsQuantity = CurrentSession.Query<Domain.Domain.Report>().Where(x => x.Event.Id == Convert.ToInt32(request.ContentId) && !x.IsReported.HasValue).Count();

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
                    var multimediaContent = CurrentSession.Query<Domain.Domain.MultimediaContent>().Where(x => x.Name.Equals(request.ContentId)).Single();

                    reportsQuantity = CurrentSession.Query<Domain.Domain.Report>().Where(x => x.MultimediaContent.Id == multimediaContent.Id && !x.IsReported.HasValue).Count();

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

            var reports = CurrentSession.Query<Domain.Domain.Report>().Where(x => !x.IsReported.HasValue).ToList();

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

            var multimediaContents = reports.Where(x => x.MultimediaContent != null)
                        .GroupBy(x => x.MultimediaContent.Id)
                        .Where(x => x.Count() >= countReportsForDisabled)
                        .Select(x => InternalServices.GetMultimediaContentSummary(x.First().MultimediaContent))
                        .ToList();

            return new SearchReportedContentsResponse()
            {
                Events = events,
                Comments = comments,
                MultimediaContents = multimediaContents
            };
        }
    }
}
