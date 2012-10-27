using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using System.IO;
using System.Security.Cryptography;
using Microsoft.Win32;

namespace VersionUpdaterSrv
{
    public class FileManager
    {
        public static string StoragePath = HttpContext.Current.Server.MapPath("Storage");
        /// <summary>
        /// Записва файл на файловата система и го добавя в XML-файла
        /// </summary>
        /// <param name="inFileBytes">съдържание на файла</param>
        /// <param name="inFileName">име на файла с разширание, без път. (file.txt)</param>
        /// <param name="inVersion">версия на файла</param>
        /// <param name="inGroup">име на групата на файла </param>
        /// <returns></returns>
        public static VersionedFile UploadFile_to_Storage(byte[] inFileBytes, string inFileName, Version inVersion, string inGroup)
        {
            if (string.IsNullOrWhiteSpace(inFileName)) throw new ArgumentNullException("inFileName", "[UploadFile_to_Storage]: FileName is not supplied.");
            if (string.IsNullOrWhiteSpace(inVersion.ToString())) throw new ArgumentNullException("inVersion", "[UploadFile_to_Storage]: Version is not supplied.");
            if (!Directory.Exists(StoragePath)) Directory.CreateDirectory(StoragePath);
            string _fileName = Get_FilePath_in_Storage(inFileName, inVersion, true);
            using (FileStream _fileStream = new FileStream(_fileName, FileMode.Create))
            {
                _fileStream.Write(inFileBytes, 0, inFileBytes.Length);
                _fileStream.Flush(true);
            }
            VersionedFile _versionedFile = new VersionedFile();
            _versionedFile.ApplicationName = inFileName;
            _versionedFile.Group = inGroup;
            _versionedFile.FileName = _fileName;
            _versionedFile.FileSize = inFileBytes.Length;
            _versionedFile.DateTime = DateTime.Now;
            _versionedFile.Version = inVersion;
            _versionedFile.CheckSum = GetSHA1Checksum(inFileBytes);
            XMLHelper.Insert_VersionedFile(_versionedFile);
            return _versionedFile;
        }

        public static string Get_FilePath_in_Storage(string inFileName, Version inVersion, bool inAppendFileName)
        {
            string _path = Path.Combine(StoragePath, inFileName);
            if (inVersion != null)
            {
                _path = Path.Combine(_path, inVersion.ToString());
            }
            if (!Directory.Exists(_path)) Directory.CreateDirectory(_path);
            if (inAppendFileName)
            {
                _path = Path.Combine(_path, inFileName);
            }
            return _path;
        }

        public static string GetSHA1Checksum(byte[] inBytes)
        {
            using (SHA1CryptoServiceProvider _sha1CryptoServiceProvider = new SHA1CryptoServiceProvider())
            {
                return BitConverter.ToString(_sha1CryptoServiceProvider.ComputeHash(inBytes));
            }
        }

        /// <summary>
        /// Чете файл от файловата система и връща байтовете му по подадени ApplicationName и версия
        /// </summary>
        /// <param name="inApplicationName"></param>
        /// <param name="inVersion"></param>
        /// <returns></returns>
        public static byte[] ReadFile_from_Storage(string inApplicationName, Version inVersion)
        {
            byte[] _bytes;
            string _fileName = Get_FilePath_in_Storage(inApplicationName, inVersion, true);
            if (!File.Exists(_fileName)) throw new FileNotFoundException(string.Format("Не е намерен файл за ApplicationName {0}, Version {1}!", inApplicationName, inVersion));
            using (FileStream _fileStream = new FileStream(_fileName, FileMode.Open))
            {
                _bytes = new byte[_fileStream.Length];
                _fileStream.Read(_bytes, 0, (int) _fileStream.Length);
            }
            return _bytes;
        }

        public static void WriteFileToResponse(byte[] inBytes, string inApplicationName, Version inVersion)
        {
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(inApplicationName));
            HttpContext.Current.Response.AddHeader("Content-Length", inBytes.Length.ToString());
            HttpContext.Current.Response.AddHeader(Constants.C_APPNAME, inApplicationName);
            HttpContext.Current.Response.AddHeader(Constants.C_APPVERSION, inVersion.ToString());
            HttpContext.Current.Response.AddHeader(Constants.C_ERRORCODE, ((int)(Constants.ErrorCodes.ecNoError)).ToString());
            HttpContext.Current.Response.ContentType = ReturnExtension(Path.GetExtension(inApplicationName).ToLower());
            HttpContext.Current.Response.BinaryWrite(inBytes);
            HttpContext.Current.Response.End();
        }

        public static void Write_AppNotFound_in_Response(string inApplicationName)
        {
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader(Constants.C_ERRORCODE, ((int)Constants.ErrorCodes.ecApplicationNotFound).ToString());
            HttpContext.Current.Response.AddHeader(Constants.C_ERRORMSG, string.Format("The requested application \"{0}\" is not present on server", inApplicationName));
            HttpContext.Current.Response.End();
        }
        public static void Write_NoGreaterVersion_in_Response(string inApplicationName, Version inVersion)
        {
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader(Constants.C_ERRORCODE, ((int)Constants.ErrorCodes.ecNoGreaterVersion).ToString());
            HttpContext.Current.Response.AddHeader(Constants.C_ERRORMSG, string.Format("The last version of application \"{0}\" is {1}.", inApplicationName, inVersion));
            HttpContext.Current.Response.End();
        }


        public static string ReturnExtension(string fileextension)
        {
            //set the default content-type
            const string DEFAULT_CONTENT_TYPE = "application/unknown";


            RegistryKey regkey, fileextkey;
            string filecontenttype;


            //the file extension to lookup
            //fileextension = ".zip";


            try
            {
                //look in HKCR
                regkey = Registry.ClassesRoot;


                //look for extension
                fileextkey = regkey.OpenSubKey(fileextension);


                //retrieve Content Type value
                filecontenttype = fileextkey.GetValue("Content Type", DEFAULT_CONTENT_TYPE).ToString();


                //cleanup
                fileextkey = null;
                regkey = null;
            }
            catch
            {
                filecontenttype = DEFAULT_CONTENT_TYPE;
            }


            //print the content type
            return filecontenttype;


        }

        public static void Save_New_XMLFile(string inBase64String)
        {
            string _fileName = XMLHelper.XMLFilePath;
            try
            {
                if (File.Exists(_fileName))
                {
                    File.Delete(_fileName);
                }
                using (FileStream _fileStream = new FileStream(_fileName, FileMode.Create))
                {
                    var _xmlFileBytes = Convert.FromBase64String(inBase64String);
                    _fileStream.Write(_xmlFileBytes, 0, _xmlFileBytes.Length);
                    _fileStream.Flush(true);
                }
            }
            catch (Exception ex)
            {
                throw new IOException("[Save_New_XMLFile]", ex);
            }
        }
    }
}