﻿namespace PubliEventos.Web.Filters
{
    using Contract.Enums;
    using Microsoft.Practices.Unity;
    using PubliEventos.Contract.Class;
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Contract.Services.Group;
    using PubliEventos.Web.App_Start;
    using System;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// Valida la accióon a realizar por un usuario.
    /// </summary>
    public class UserActionRestriction : FilterAttribute, IAuthorizationFilter
    {
        #region Properties

        /// <summary>
        /// Servicio de localidades.
        /// </summary>
        [Dependency]
        public IEventServices serviceEvents { get; set; }

        /// <summary>
        /// Servicio de grupos.
        /// </summary>
        [Dependency]
        public IGroupServices serviceGroups { get; set; }

        /// <summary>
        /// Tipo de elemento a validar.
        /// </summary>
        private int ElementType { get; set; }

        #endregion

        #region Contructor

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="elementType">Tipo de objeto a validar.</param>
        public UserActionRestriction(ElementTypesConstraintValidations elementType)
        {
            this.ElementType = (int)elementType;

            this.serviceEvents = DependencyResolver.Current.GetService<IEventServices>();
            this.serviceGroups = DependencyResolver.Current.GetService<IGroupServices>();
        }

        #endregion

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            this.ValidateUserAction(filterContext);
        }

        /// <summary>
        /// Valida el usuario contra la acción que quiere realizar.
        /// </summary>
        /// <param name="filterContext">Contexto.</param>
        private void ValidateUserAction(AuthorizationContext filterContext)
        {
            // Usuario logueado.
            var user = (CustomPrincipal)filterContext.HttpContext.User;

            // Id del elemento.
            var id = this.GetElementId(filterContext);

            // Elemento.
            var element = this.GetElement(id, this.ElementType);
            var valid = true;

            if (this.ElementType == (int)ElementTypesConstraintValidations.Event)
            {
                Event _event = (Event)element;

                if (_event == null)
                {
                    throw new Exception("El evento no existe o fue dado de baja.");
                }

                if (_event.User.Id != user.Id)
                {
                    valid = false;
                }
            }
            else if (this.ElementType == (int)ElementTypesConstraintValidations.Group)
            {
                Group group = (Group)element;

                if (group == null)
                {
                    throw new Exception("El grupo no existe o fue dado de baja.");
                }

                if (filterContext.ActionDescriptor.ActionName == "Edit" && group.Administrator.Id != user.Id)
                {
                    valid = false;
                }

                if (filterContext.ActionDescriptor.ActionName == "Detail" && !group.UsersGroup.Where(x => x.Active.HasValue && x.Active.Value == true).Select(x => x.UserId).Contains(user.Id))
                {
                    valid = false;
                }
            }

            if (!valid)
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = "/Views/Error/UnauthorizedAccess.cshtml"
                };
            }
        }

        /// <summary>
        /// Obtiene el Id del route value.
        /// </summary>
        /// <param name="filterContext">Contexto.</param>
        /// <returns></returns>
        private int? GetElementId(AuthorizationContext filterContext)
        {
            int id;
            var valid = false;

            if (filterContext.RouteData.Values["id"] != null)
            {
                valid = int.TryParse(filterContext.RouteData.Values["id"].ToString(), out id);

                if (valid)
                {
                    return int.Parse(filterContext.RouteData.Values["id"].ToString());
                }

                throw new Exception("Parámetro incorrecto");
            }
            else if (filterContext.HttpContext.Request.Params["id"] != null)
            {
                valid = int.TryParse(filterContext.HttpContext.Request.Params["id"].ToString(), out id);

                if (valid)
                {
                    return int.Parse(filterContext.HttpContext.Request.Params["id"].ToString());
                }

                throw new Exception("Parámetro incorrecto");
            }

            return null;
        }

        /// <summary>
        /// Obtiene el objeto sobre el cual validar.
        /// </summary>
        /// <param name="elementId">Identificador del objeto.</param>
        /// <param name="elementType">Tipo de objeto.</param>
        /// <returns>Objeto.</returns>
        public object GetElement(int? elementId, int elementType)
        {
            if (elementId.HasValue)
            {
                if (elementType == (int)ElementTypesConstraintValidations.Event)
                {
                    return this.serviceEvents.GetEventById(elementId.Value);
                }
                else if (elementType == (int)ElementTypesConstraintValidations.Group)
                {
                    return this.serviceGroups.GetGroupById(new GetGroupByIdRequest() { GroupId = elementId.Value }).Group;
                }
            }

            return null;
        }
    }
}