namespace PubliEventos.Web.Controllers
{
    using Microsoft.Practices.Unity;
    using Newtonsoft.Json;
    using PubliEventos.Contract.Class;
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Contract.Enums;
    using PubliEventos.Contract.Services.Account;
    using PubliEventos.Contract.Services.Event;
    using PubliEventos.Web.App_Start;
    using PubliEventos.Web.Helpers;
    using PubliEventos.Web.Models;
    using PubliEventos.Web.Models.AccountModels;
    using PubliEventos.Web.Mvc.Filters;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using System.Web.Security;

    /// <summary>
    /// Controlador de cuentas.
    /// </summary>
    public class AccountController : BaseController
    {
        #region Properties

        /// <summary>
        /// Servicio de localidades.
        /// </summary>
        [Dependency]
        public IAccountServices serviceAccounts { get; set; }

        /// <summary>
        /// Servicio de localidades.
        /// </summary>
        [Dependency]
        public ILocalityServices ServiceLocalities { get; set; }

        /// <summary>
        /// Servicio de eventos.
        /// </summary>
        [Dependency]
        public IEventServices serviceEvents { get; set; }

        #endregion

        #region Views

        /// <summary>
        /// Vista de login.
        /// </summary>
        /// <returns>Login view.</returns>
        [AllowAnonymous]
        public ActionResult Login(bool? isRegister)
        {
            var model = new UserModel();

            ViewBag.Provinces = new SelectList(ServiceLocalities.GetAllProvinces(), "Id", "Name");
            ViewBag.isRegister = isRegister;

            return View(model);
        }

        /// <summary>
        /// Vista de login.
        /// </summary>
        /// <param name="model">LoginModel.</param>
        /// <returns>Index o Login view.</returns>
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Login(UserModel model, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                var returnAction = string.Empty;

                if (this.ValidateUser(ref model))
                {
                    if (string.IsNullOrEmpty(ReturnUrl))
                    {
                        returnAction = "/Home/Index";
                    }
                    else
                    {
                        returnAction = ReturnUrl;
                    }

                    return Json(new { Success = true, ReturnUrl = returnAction }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { Success = false, Errors = ModelErrors.GetModelErrors(ModelState), IsExpirate = model.IsExpirate }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Vista de LogOut.
        /// </summary>
        /// <returns>Index View.</returns>
        [AllowAnonymous]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Activación de cuenta mediante el token.
        /// </summary>
        /// <param name="token">token de activación.</param>
        [AllowAnonymous]
        public ActionResult AccountActivation(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                ViewBag.active = serviceAccounts.ActivateAccount(token);
            }

            return View();
        }

        /// <summary>
        /// Perfil del usuario.
        /// </summary>
        /// <param name="id">Identificador del usuario.</param>
        /// <returns>Profile view.</returns>
        [AllowAnonymous]
        public ActionResult Profile(int id)
        {
            var model = this.serviceAccounts.GetUserById(new GetUserByIdRequest() { UserId = id }).User;

            var events = this.serviceEvents.SearchFilteredEvents(new SearchFilteredEventsRequest() { SearchPublics = true, UserId = id }).Events;

            // Obtengo los ultimos eventos del usuario.
            ViewBag.events = events.OrderByDescending(x => x.EventDate).Take(5).ToList();

            // Obtengo los eventos mas populares.
            ViewBag.mostPopularEvents = this.serviceEvents.SearchMostPopularEvents(new SearchMostPopularEventsRequest() { UserId = id }).Events;

            return View(model);
        }

        /// <summary>
        /// Vista de edición del perfil de usuario.
        /// </summary>
        /// <param name="id">Identificador del usuario.</param>
        /// <returns>EditProfile view.</returns>
        [Authorize]
        [UserActionRestriction(ValidateCondition.Profile)]
        public ActionResult EditProfile(int id)
        {
            var user = this.serviceAccounts.GetUserById(new GetUserByIdRequest() { UserId = id }).User;

            var model = new EditProfileRequest()
            {
                UserId = user.Id.Value,
                Email = user.Email,
                FirstNameOld = user.FirstName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                LocalityId = user.Locality.Id.Value,
                ProvinceId = user.Locality.Province.Id.Value,
                UserName = user.UserName,
                BirthDate = user.BirthDate.HasValue ? user.BirthDate.Value : DateTime.Now,
                PathProfile = user.PathProfile,
                Password = Encryptor.Decrypt(user.Password),
                ImageProfile = user.ImageProfile
            };

            ViewBag.Provinces = new SelectList(ServiceLocalities.GetAllProvinces(), "Id", "Name", model.ProvinceId);
            ViewBag.Localities = new SelectList(ServiceLocalities.GetAllLocalities().Where(x => x.Province.Id == model.ProvinceId).ToList(), "Id", "Name", model.LocalityId);

            return View(model);
        }

        /// <summary>
        /// Vista de recuperación de contraseña.
        /// </summary>
        /// <param name="id">Identificador del usuario.</param>
        /// <returns>RecoverPassword view.</returns>
        [AllowAnonymous]
        public ActionResult RecoverPassword(int id)
        {
            ViewBag.userId = id;

            return View();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Valida lo datos del usuario.
        /// </summary>
        /// <param name="model">Modelo de login.</param>
        /// <returns>True si es valido, false caso contrario.</returns>
        private bool ValidateUser(ref UserModel model)
        {
            if (!string.IsNullOrEmpty(model.UserName) && !string.IsNullOrEmpty(model.Password))
            {
                var user = serviceAccounts.GetUserByUserName(model.UserName.ToLower().Trim());

                if (user != null)
                {
                    if (!user.Active)
                    {
                        ModelState.AddModelError("Error", "Usuario no activo, por favor active su cuenta verificando su email.");

                        model.IsExpirate = this.serviceAccounts.HasActiveActivationToken(user.Id.Value);

                        return false;
                    }
                    else if (user.HasActiveSuspension)
                    {
                        ModelState.AddModelError("Error", "Usuario deshabilitado");

                        return false;
                    }
                    else if (user.Password == Encryptor.Encrypt(model.Password))
                    {
                        // Inicializa el identity y crea la cookie.
                        var custom = new CustomPrincipal(model.UserName);
                        this.CreateAuthenticationTicket(model.UserName, model.RememberMe);

                        return true;
                    }
                }
            }

            ModelState.AddModelError("Error", "Usuario o ccntraseña Incorrecta.");

            return false;
        }

        /// <summary>
        /// Creao una cookie y la devuelve al usuario.
        /// </summary>
        /// <param name="userName">userName.</param>
        private void CreateAuthenticationTicket(string userName, bool rememberMe)
        {
            var authUser = this.serviceAccounts.GetUserByUserName(userName);

            // Datos a pasar al usuario en la cookie.
            var customPrincipalModel = new CustomPrincipalSerializeModel()
            {
                Id = authUser.Id.Value,
                FirstName = authUser.FirstName,
                LastName = authUser.LastName,
                ImageProfile = Path.GetFileName(authUser.ImageProfile),
                IsAdministrator = authUser.IsAdministrator
            };

            var serializer = new JavaScriptSerializer();
            string userData = serializer.Serialize(customPrincipalModel);

            // Creo el ticket.
            var authTicket = new FormsAuthenticationTicket(
              1,
              userName,
              DateTime.Now,
              DateTime.Now.AddDays(90),
              rememberMe,
              userData,
              "/");

            // Encrypto el ticket.
            string encTicket = FormsAuthentication.Encrypt(authTicket);

            // Creo la cookie.
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);

            // Devuelvo la cookie al usuario.
            HttpContext.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Envia email de activación de cuenta.
        /// </summary>
        /// <param name="userName">Username del usuario.</param>
        private void SendEmailAccountConfirmation(string userName)
        {
            var user = this.serviceAccounts.GetUserByUserName(userName);

            var token = Guid.NewGuid();

            var smtp = new SmtpClient
            {
                Host = ConfigurationManager.AppSettings["SmtpHost"].ToString(),
                Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]),
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(ConfigurationManager.AppSettings["Mail"], ConfigurationManager.AppSettings["Password"])
            };

            var fromAddress = new MailAddress(ConfigurationManager.AppSettings["Mail"]);
            var toAddress = new MailAddress(user.Email);
            string subject = "Publieventos - Activación de cuenta";

            string body = string.Format("Estimado/a {0}:" +
                         "<br/><br/>Para activar su cuenta ingrese al link que figura a continuación: <br/>" +
                         "<a href='{1}://{2}/Account/AccountActivation?token={3}'>Activar mi cuenta</a>" +
                         "<br/><br/>Saludos cordiales. <br/>Equipo de administración de Publieventos",
                         userName, System.Web.HttpContext.Current.Request.RequestContext.HttpContext.Request.Url.Scheme,
                         System.Web.HttpContext.Current.Request.RequestContext.HttpContext.Request.Url.Authority, token);
            try
            {
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                {
                    smtp.Send(message);
                }

                // Guarda el token de activación de cuenta.
                serviceAccounts.SaveAccountActivationToken(token.ToString(), user.Id.Value);
            }
            catch (Exception)
            {
                throw new Exception("Ha ocurrido un error al enviar mail.");
            }
        }

        /// <summary>
        /// Valida la si el nombre de usuario ya existe.
        /// </summary>
        /// <param name="userName">Nombre de usuario.</param>
        /// <param name="userIdToExclude">Id de usuario a excluir en la validación.</param>
        /// <returns>True si no existe, false si ya esta usado.</returns>
        private bool ValidateExistsUserName(string userName, int? userIdToExclude)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                var user = serviceAccounts.GetUserByUserName(userName.ToLower().Trim());

                if (user != null)
                {
                    if (!userIdToExclude.HasValue || user.Id != userIdToExclude)
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Elimina fotos de perfil.
        /// </summary>
        /// <param name="fileName">Nombre del archivo.</param>
        private void DeleteFilePicture(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                var path = HttpContext.Server.MapPath(pathImageProfile + fileName);

                if (System.IO.File.Exists(path))
                {
                    if (System.IO.File.Exists(path) &&
                        (System.IO.File.GetAttributes(path) & FileAttributes.Hidden) == FileAttributes.ReadOnly ||
                        (System.IO.File.GetAttributes(path) & FileAttributes.Hidden) == FileAttributes.Archive)
                    {
                        System.IO.File.SetAttributes(path, FileAttributes.Normal);
                    }

                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(HttpContext.Server.MapPath(pathImageProfile + fileName));
                    }
                }
            }
        }

        /// <summary>
        /// Corta la imagen que subio el usuario.
        /// </summary>
        /// <param name="originalFilePath">Path de la imagen original.</param>
        /// <param name="origW">Ancho original.</param>
        /// <param name="origH">Altura original.</param>
        /// <param name="targetW">Ancho final.</param>
        /// <param name="targetH">Alto final.</param>
        /// <param name="cropStartY">Comienzo de corta vertical.</param>
        /// <param name="cropStartX">Comienzo de corte horizontal.</param>
        /// <param name="cropW">Ancho del corte.</param>
        /// <param name="cropH">Altura del corte.</param>
        /// <returns>FileName.</returns>
        private string CropImage(string originalFilePath, int origW, int origH, int targetW, int targetH, int cropStartY, int cropStartX, int cropW, int cropH)
        {
            var originalImage = Image.FromFile(originalFilePath);

            var resizedOriginalImage = new Bitmap(originalImage, targetW, targetH);
            var targetImage = new Bitmap(cropW, cropH);

            using (var g = Graphics.FromImage(targetImage))
            {
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(resizedOriginalImage, new Rectangle(0, 0, cropW, cropH), new Rectangle(cropStartX, cropStartY, cropW, cropH), GraphicsUnit.Pixel);
            }

            string fileName = Path.GetFileName(originalFilePath);

            string extension = Path.GetExtension(fileName);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            var fileNameWithOutDate = fileNameWithoutExtension.Substring(0, fileNameWithoutExtension.Length - 15);

            fileName = string.Format("{0}_{1}{2}", fileNameWithOutDate, DateTime.Now.ToString("ddMMyyyyhhMMss"), extension);

            var folder = HttpContext.Server.MapPath(pathImageProfile);
            string croppedPath = Path.Combine(folder, fileName);

            targetImage.Save(croppedPath);

            return fileName;
        }

        #endregion

        #region Json Methods

        /// <summary>
        /// Vista de registración.
        /// </summary>
        /// <returns>SignUp view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public JsonResult SignUp(UserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User()
                {
                    FirstName = model.SignUpModel.FirstName,
                    LastName = model.SignUpModel.LastName,
                    Email = model.SignUpModel.Email,
                    UserName = model.SignUpModel.UserNameToRegister,
                    Password = Encryptor.Encrypt(model.SignUpModel.PasswordToRegister),
                    Locality = new Locality()
                    {
                        Id = model.SignUpModel.Locality.Value
                    },
                    EffectDate = DateTime.Now,
                    BirthDate = model.SignUpModel.BirthDate
                };

                var response = serviceAccounts.RegisterUser(new RegisterUserRequest() { User = user });

                if (response.UserId.HasValue)
                {
                    this.SendEmailAccountConfirmation(user.UserName);
                }

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = false, Errors = ModelErrors.GetModelErrors(ModelState) }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Valida si el nombre de usuario ya existe.
        /// </summary>
        /// <param name="userName">nombre de usuario.</param>
        /// <returns>True si existe, false caso contrario.</returns>
        [HttpPost]
        [AllowAnonymous]
        public JsonResult ValidateExistUserName(string userName, int? userIdToExclude)
        {
            var valid = this.ValidateExistsUserName(userName, userIdToExclude);

            return Json(new { Exist = valid }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Valida si ya existe un usuario con ese email.
        /// </summary>
        /// <param name="email">Email a validar.</param>
        /// <returns>True si ya existe, false caso contrario.</returns>
        [HttpPost]
        [AllowAnonymous]
        public JsonResult ValidateExistEmail(string email, int? userId)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var exist = serviceAccounts.ExistsEmail(new ExistsEmailRequest() { Email = email, UserId = userId }).Exists;

                return Json(new { Exist = exist }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Exist = false }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Obtiene las localidades filtradas por provincia.
        /// </summary>
        /// <param name="IdProvince">Identificador de la provincia.</param>
        /// <returns>Localidades.</returns>
        [AllowAnonymous]
        public JsonResult GetLocalitiesByProvince(string IdProvince)
        {
            if (!string.IsNullOrEmpty(IdProvince))
            {
                var localities =
                    ServiceLocalities.GetAllLocalities()
                        .Where(x => x.Province.Id == Convert.ToInt32(IdProvince))
                        .Select(x => new { x.Id, x.Name })
                        .ToList();

                return Json(localities, JsonRequestBehavior.AllowGet);
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Reenvia el email de activación de cuenta.
        /// </summary>
        /// <param name="userName">Nombre de usuario</param>
        /// <returns>Json.</returns>
        [HttpPost]
        [AllowAnonymous]
        public JsonResult ResendEmailActivation(string userName)
        {
            try
            {
                this.SendEmailAccountConfirmation(userName);

                // Doy de baja los token expirados y no eliminados del usuario.
                this.serviceAccounts.DeleteActivationToken(userName);

                return Json(new { isValid = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { isValid = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Busca usuario por autocompletado de nombre de usuario.
        /// </summary>
        /// <param name="userName">Nombre de usuario.</param>
        /// <param name="pageNumber">Número de página.</param>
        /// <param name="pageSize">Tamaño de página.</param>
        /// <returns>Usuarios encontrados.</returns>
        [AllowAnonymous]
        public JsonResult SearchUsersByUserName(string userName, int pageNumber, int pageSize)
        {
            List<Select2Result> Users = new List<Select2Result>();

            var response = this.serviceAccounts.SearchUsersByPartialUserName(new SearchUsersByPartialUserNameRequest() { UserName = userName, PageNumber = pageNumber, PageSize = pageSize });

            foreach (var user in response.Users)
            {
                var userResult = new Select2Result();
                userResult.id = user.Id.Value;
                userResult.text = user.UserName;

                Users.Add(userResult);
            }

            return Json(new { Users = Users, Quantity = response.Quantity }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Edita el perfil de un usuario.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditProfile(EditProfileRequest model, FileModel fileModel)
        {
            if (ModelState.IsValid)
            {
                // Valido la existencia del email y nombre de usuario.
                var existEmail = serviceAccounts.ExistsEmail(new ExistsEmailRequest() { Email = model.Email, UserId = model.UserId }).Exists;
                var existUserName = this.ValidateExistsUserName(model.UserName, model.UserId);

                if (!existEmail && !existUserName)
                {
                    if (!string.IsNullOrEmpty(model.ImageCrop))
                    {
                        if (!string.IsNullOrEmpty(model.ImageProfile) && System.IO.File.Exists(HttpContext.Server.MapPath(model.ImageProfile)))
                        {
                            // Elimino la portada anterior.
                            this.DeleteFilePicture(model.ImageProfile);
                        }

                        // Seteo el nombre de la imagen.
                        model.ImageProfile = model.ImageCrop;
                    }

                    if (!string.IsNullOrEmpty(model.UploadImage))
                    {
                        // Elimino la imagen que se subio primerio antes de cortarla.
                        this.DeleteFilePicture(model.UploadImage);
                    }

                    this.serviceAccounts.EditProfile(model);

                    if (!string.IsNullOrEmpty(model.ImageCrop) || model.FirstNameOld != model.FirstName)
                    {
                        //Obtengo la cookie y la desencripto.
                        HttpCookie cookie = (HttpCookie)(Request.Cookies[FormsAuthentication.FormsCookieName]);
                        FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

                        // Deslogueo y creo el identity nuevamente.
                        FormsAuthentication.SignOut();
                        var custom = new CustomPrincipal(model.UserName);
                        this.CreateAuthenticationTicket(model.UserName, ticket.IsPersistent);
                    }

                    return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ModelState.AddModelError("UserName", "El Usuario o Email ya existe");
                }
            }

            return Json(new { Success = false, Errors = ModelErrors.GetModelErrors(ModelState) }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Edita el password del usuario actual.
        /// </summary>
        /// <param name="model">EditPasswordRequest model.</param>
        /// <returns>True o false.</returns>
        [HttpPost]
        [AllowAnonymous]
        public JsonResult EditPassword(EditPasswordRequest model)
        {
            if (ModelState.IsValid)
            {
                // Encrypto la contraseña.
                model.NewPassword = Encryptor.Encrypt(model.NewPassword);

                this.serviceAccounts.EditPassword(model);

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = false, Errors = ModelErrors.GetModelErrors(ModelState) }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Envía código de verificación para recuperar contraseña.
        /// </summary>
        /// <param name="model">SendRecoverPasswordCodeRequest model.</param>
        /// <returns>true si se envío correctamente, false caso contrario.</returns>
        [HttpPost]
        [AllowAnonymous]
        public JsonResult SendRecoverPasswordCode(SendRecoverPasswordCodeRequest model)
        {
            if (ModelState.IsValid)
            {
                var response = this.serviceAccounts.SendRecoverPasswordCode(model);

                return Json(new { Success = response.Success, UserId = response.UserId }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Valida si el código de recuperación de contraseña es válido.
        /// </summary>
        /// <param name="model">ValidateRecoverPasswordCodeRequest model.</param>
        /// <returns>True si es válido, false caso contrario.</returns>
        [HttpPost]
        [AllowAnonymous]
        public JsonResult ValidateRecoverPasswordCode(ValidateRecoverPasswordCodeRequest model)
        {
            if (ModelState.IsValid)
            {
                var response = this.serviceAccounts.ValidateRecoverPasswordCode(model);

                return Json(new { Success = response.IsValid }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Elimina la foto de perfil que se utilizo para cortar.
        /// </summary>
        /// <param name="imageUploaded">Imagen subida.</param>
        /// <param name="imageCrop">Imagen cortada.</param>
        /// <returns>True.</returns>
        [HttpPost]
        public JsonResult DeleteProfilePicture(string imageUploaded, string imageCrop)
        {
            if (!string.IsNullOrEmpty(imageUploaded))
            {
                this.DeleteFilePicture(imageUploaded);
            }

            if (!string.IsNullOrEmpty(imageCrop))
            {
                this.DeleteFilePicture(imageCrop);
            }

            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Imagen que sube el usuario.
        /// </summary>
        /// <param name="img">Imagen.</param>
        /// <returns>Result.</returns>
        [HttpPost]
        public string UploadOriginalImage(HttpPostedFileBase img)
        {
            if (PicturesExtensions.Contains(Path.GetExtension(img.FileName)))
            {
                string folder = HttpContext.Server.MapPath(pathImageProfile);

                string extension = Path.GetExtension(img.FileName);
                string fileName = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(img.FileName), DateTime.Now.ToString("ddMMyyyyhhMMss"), extension);

                string tempFilePath = System.IO.Path.Combine(folder, fileName);

                img.SaveAs(tempFilePath);
                var image = System.Drawing.Image.FromFile(tempFilePath);

                var result = new
                {
                    status = "success",
                    width = image.Width,
                    height = image.Height,
                    url = pathImageProfile + fileName,
                    fileName = fileName
                };

                return JsonConvert.SerializeObject(result);
            }

            var resultError = new
            {
                status = "error",
                message = "Tipo de archivo no permitido"
            };

            return JsonConvert.SerializeObject(resultError);
        }

        /// <summary>
        /// Corta la imagen que subio el usuario.
        /// </summary>
        /// <param name="imgUrl">Url de la imagen original.</param>
        /// <param name="imgInitW">Ancho.</param>
        /// <param name="imgInitH">Alto.</param>
        /// <param name="imgW">Ancho final.</param>
        /// <param name="imgH">Alto final.</param>
        /// <param name="imgY1">Comienzo corte vertical.</param>
        /// <param name="imgX1">Comienzo de corte horizontal.</param>
        /// <param name="cropH">Alto del corte.</param>
        /// <param name="cropW">Ancho del corte.</param>
        /// <returns>Result.</returns>
        [HttpPost]
        public string CroppedImage(string imgUrl, int imgInitW, int imgInitH, double imgW, double imgH, int imgY1, int imgX1, int cropH, int cropW)
        {
            var originalFilePath = HttpContext.Server.MapPath(imgUrl);
            var fileName = CropImage(originalFilePath, imgInitW, imgInitH, (int)imgW, (int)imgH, imgY1, imgX1, cropW, cropH);

            var result = new
            {
                status = "success",
                url = pathImageProfile + fileName,
                fileName = fileName
            };

            return JsonConvert.SerializeObject(result);
        }

        #endregion
    }
}
