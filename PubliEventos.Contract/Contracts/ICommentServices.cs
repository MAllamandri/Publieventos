﻿namespace PubliEventos.Contract.Contracts
{
    using PubliEventos.Contract.Services.Comment;

    /// <summary>
    /// Interfaz del servicio de comentarios.
    /// </summary>
    public interface ICommentServices
    {
        /// <summary>
        /// Obtiene los comentarios de un evento.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        GetCommentsByEventResponse GetCommentsByEvent(GetCommentsByEventRequest request);
    }
}
