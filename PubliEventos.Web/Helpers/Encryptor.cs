namespace PubliEventos.Web.Helpers
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class Encryptor
    {
        /// <summary>
        /// Clave de encritación/Desencriptacion.
        /// </summary>
        private static string _key = "myVeryStrongPsw";

        /// <summary>
        /// Encrypta un string pasado como parámetro.
        /// </summary>
        /// <param name="strToEncrypt">String a ser encriptado.</param>
        /// <returns>String encriptado.</returns>
        public static string Encrypt(string strToEncrypt)
        {
            try
            {
                return Encrypt(strToEncrypt, _key);
            }

            catch (Exception ex)
            {
                return "Entrada incorrecta: " + ex.Message;
            }
        }

        /// <summary>
        /// Dessencripta un string pasado como paramatro con una clave especifica.
        /// </summary>
        /// <param name="strEncrypted">String encryptado.</param>
        /// <returns>String desencriptado.</returns>
        public static string Decrypt(string strEncrypted)
        {
            try
            {
                return Decrypt(strEncrypted, _key);
            }
            catch (Exception ex)
            {
                return "Entrada incorrecta: " + ex.Message;
            }
        }

        /// <summary>
        /// Encrypta un string pasado como parámetro.
        /// </summary>
        /// <param name="strToEncrypt">String a ser encriptado.</param>
        /// <param name="strKey">Clave de encriptación.</param>
        /// <returns>String encriptado.</returns>
        public static string Encrypt(string strToEncrypt, string strKey)
        {
            try
            {
                TripleDESCryptoServiceProvider objDESCrypto =
                    new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider objHashMD5 = new MD5CryptoServiceProvider();
                byte[] byteHash, byteBuff;
                string strTempKey = strKey;
                byteHash = objHashMD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(strTempKey));
                objHashMD5 = null;
                objDESCrypto.Key = byteHash;
                objDESCrypto.Mode = CipherMode.ECB; //CBC, CFB
                byteBuff = ASCIIEncoding.ASCII.GetBytes(strToEncrypt);

                return Convert.ToBase64String(objDESCrypto.CreateEncryptor().
                    TransformFinalBlock(byteBuff, 0, byteBuff.Length));
            }
            catch (Exception ex)
            {
                return "Entrada incorrecta: " + ex.Message;
            }
        }

        /// <summary>
        /// Dessencripta un string pasado como paramatro con una clave especifica.
        /// </summary>
        /// <param name="strEncrypted">String encryptado.</param>
        /// <param name="strKey">Clave de desencriptado.</param>
        /// <returns>String desencriptado.</returns>
        public static string Decrypt(string strEncrypted, string strKey)
        {
            try
            {
                TripleDESCryptoServiceProvider objDESCrypto =
                    new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider objHashMD5 = new MD5CryptoServiceProvider();
                byte[] byteHash, byteBuff;
                string strTempKey = strKey;
                byteHash = objHashMD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(strTempKey));
                objHashMD5 = null;
                objDESCrypto.Key = byteHash;
                objDESCrypto.Mode = CipherMode.ECB; //CBC, CFB
                byteBuff = Convert.FromBase64String(strEncrypted);
                string strDecrypted = ASCIIEncoding.ASCII.GetString
                    (objDESCrypto.CreateDecryptor().TransformFinalBlock
                        (byteBuff, 0, byteBuff.Length));
                objDESCrypto = null;

                return strDecrypted;
            }
            catch (Exception ex)
            {
                return "Entrada incorrecta: " + ex.Message;
            }
        }
    }
}