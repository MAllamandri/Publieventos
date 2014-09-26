namespace PubliEventos.Web.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class ModelErrors
    {
        /// <summary>
        /// Obtiene los errores del modelo como un diccionario.
        /// </summary>
        /// <param name="modelState">Modelo.</param>
        /// <returns>Diccionario de errores.</returns>
        public static IDictionary<string, string> GetModelErrors(ModelStateDictionary modelState)
        {
            // Inicializo elementos.
            var errors = new Dictionary<string, string>();
            int count = 0;

            var keys = modelState.Keys.ToArray();

            // Recorro los items del ModelState.
            foreach (var item in modelState.Values)
            {
                if (item.Errors.Count() != 0)
                {
                    foreach (var error in item.Errors)
                    {
                        if (!errors.ContainsKey(keys[count]))
                        {
                            errors.Add(keys[count], error.ErrorMessage);
                        }
                    }
                }

                count++;
            }

            return errors;
        }
    }
}