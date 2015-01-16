﻿namespace PubliEventos.Contract.Services.Invitation
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Representa los parámetros de la operación CreateInvitation.
    /// </summary>
    public class CreateInvitationRequest
    {
        /// <summary>
        /// Usuario invitado.
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Grupo al que fue invitado.
        /// </summary>
        public int? GroupId { get; set; }

        /// <summary>
        /// Evento al que fue invitado.
        /// </summary>
        public int? EventId { get; set; }
    }
}
