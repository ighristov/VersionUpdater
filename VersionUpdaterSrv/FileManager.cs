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

        public static void WriteFileToResponse(byte[] inBytes, string inFileName)
        {
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(inFileName));
            HttpContext.Current.Response.AddHeader("Content-Length", inBytes.Length.ToString());
            HttpContext.Current.Response.ContentType = ReturnExtension(Path.GetExtension(inFileName).ToLower());
            HttpContext.Current.Response.BinaryWrite(inBytes);
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
    }
}