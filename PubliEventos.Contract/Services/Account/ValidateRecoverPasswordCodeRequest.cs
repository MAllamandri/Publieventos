﻿namespace PubliEventos.Contract.Services.Account
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Representa los parámetros de entrada de la operación ValidateRecoverPasswordCode.
    /// </summary>
    public class ValidateRecoverPasswordCodeRequest
    {
        /// <summary>
        /// Código.
        /// </summary>
        [Required(ErrorMessage = "El valor es requerido")]
        public string Code { get; set; }

        /// <summary>
        /// Identificador del usuario.
        /// </summary>
        [Required(ErrorMessage = "El valor es requerido")]
        public int UserId { get; set; }
    }
}
