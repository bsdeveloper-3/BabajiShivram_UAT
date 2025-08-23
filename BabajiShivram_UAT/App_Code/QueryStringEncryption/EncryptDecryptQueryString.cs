using System.Collections.Specialized;
using System.Text;
using System.Collections;


namespace QueryStringEncryption
{
    /// <summary>
    /// This class contains methods to en-/decrypt query strings.
    /// </summary>
    public static class EncryptDecryptQueryString
    {
        /// <summary>
        /// Encrypt query strings from string array.
        /// </summary>
        /// <param name="unencryptedStrings">Unencrypted query strings in the format 'param=value'.</param>
        /// <param name="key">Key, being used to encrypt.</param>
        /// <returns></returns>
        public static string EncryptQueryStrings(string[] unencryptedStrings, string key)
        {
            StringBuilder strings = new StringBuilder();

            foreach (string unencryptedString in unencryptedStrings)
            {
                if (strings.Length > 0) strings.Append("&");
                strings.Append(unencryptedString);
            }

            return string.Concat("request=", Encryption64.Encrypt(strings.ToString(), key));
        }

        /// <summary>
        /// Encrypt query strings from name value collection.
        /// </summary>
        /// <param name="unencryptedStrings">Unencrypted query strings.</param>
        /// <param name="key">Key, being used to encrypt.</param>
        /// <returns></returns>
        public static string EncryptQueryStrings(NameValueCollection unencryptedStrings, string key)
        {
            StringBuilder strings = new StringBuilder();

            foreach (string stringKey in unencryptedStrings.Keys)
            {
                if (strings.Length > 0) strings.Append("&");
                strings.Append(string.Format("{0}={1}", stringKey, unencryptedStrings[stringKey]));
            }

            return string.Concat("request=", Encryption64.Encrypt(strings.ToString(), key));
        }

        /// <summary>
        /// Decrypt query string.
        /// </summary>
        /// <param name="encryptedStrings">Encrypted query string.</param>
        /// <param name="key">Key, being used to decrypt.</param>
        /// <remarks>The query string object replaces '+' character with an empty character.</remarks>
        /// <returns></returns>
        public static string DecryptQueryStrings(string encryptedStrings, string key)
        {
            return Encryption64.Decrypt(encryptedStrings.Replace(" ", "+"), key);
        }


        // Modified to accomodate LoggedInUser//
        #region Modifications

        /// <summary>
        /// Encrypts query strings from string array.
        /// </summary>
        /// <param name="unencryptedStrings">Unencrypted string array in the format 'param=value'.</param>		
        /// <returns string>Encrypted QueryString</returns>
        public static string EncryptQueryStrings(string[] unencryptedStrings)
        {
            //string key = "!Q@W#E$R%T^Y&U*I(O)P_{+}";

            string key = new LoginClass().glUserId.ToString();

            StringBuilder strings = new StringBuilder();

            foreach (string unencryptedString in unencryptedStrings)
            {
                if (strings.Length > 0) strings.Append("&");
                strings.Append(unencryptedString);
            }

            return string.Concat("request=", Encryption64.Encrypt(strings.ToString(), key));
        }

        /// <summary>
        /// Encrypts url path.
        /// </summary>
        /// <param name="unencryptedStrings">Unencrypted url path.</param>		
        /// <returns string>Encrypted QueryString</returns>
        public static string EncryptQueryStrings(string Fullpath)
        {
            string[] PageAddress = Fullpath.Split('?');
            string MainPage = PageAddress[0];
            string QueryString = PageAddress[1];

            string[] unencryptedStrings = QueryString.Split('&');

            //string key = "!Q@W#E$R%T^Y&U*I(O)P_{+}";
            string key = new LoginClass().glUserId.ToString();

            StringBuilder strings = new StringBuilder();

            foreach (string unencryptedString in unencryptedStrings)
            {
                if (strings.Length > 0) strings.Append("&");
                strings.Append(unencryptedString);
            }

            return string.Concat(MainPage, "?request=", Encryption64.Encrypt(strings.ToString(), key));
        }

        /// <summary>
        /// Encrypts query strings from name value collection.
        /// </summary>
        /// <param name="unencryptedStrings">Unencrypted NameValueCollection in the format 'param=value'..</param>
        /// <returns></returns>
        public static string EncryptQueryStrings(NameValueCollection unencryptedStrings)
        {
            //string key = "!Q@W#E$R%T^Y&U*I(O)P_{+}";
            string key = new LoginClass().glUserId.ToString();
            StringBuilder strings = new StringBuilder();

            foreach (string stringKey in unencryptedStrings.Keys)
            {
                if (strings.Length > 0) strings.Append("&");
                strings.Append(string.Format("{0}={1}", stringKey, unencryptedStrings[stringKey]));
            }

            return string.Concat("request=", Encryption64.Encrypt(strings.ToString(), key));
        }

        public static string EncryptQueryStrings2(string StringToEncrypt)
        {
            //string key = "!Q@W#E$R%T^Y&U*I(O)P_{+}";
            string key = new LoginClass().glUserId.ToString();

            return Encryption64.Encrypt(StringToEncrypt, key);
        }

        /// <summary>
        /// Decrypts Encrypted QueryString.
        /// </summary>
        /// <param name="encryptedStrings">Encrypted query string.</param>
        /// <remarks>The query string object replaces '+' character with an empty character.</remarks>
        /// <returns></returns>
        public static string DecryptQueryStrings(string encryptedStrings)
        {
            //string key = "!Q@W#E$R%T^Y&U*I(O)P_{+}";

            string key = new LoginClass().glUserId.ToString();

            return Encryption64.Decrypt(encryptedStrings.Replace(" ", "+"), key);
        }


        #endregion
    }
}