using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace VersionUpdaterSrv
{
    public class ReqRespManager
    {
        public static void ProcessRequest_MakeResponse()
        {
            string _appName = HttpContext.Current.Request.Headers[Constants.C_APPNAME];
            string _appVersion = HttpContext.Current.Request.Headers[Constants.C_APPVERSION];
            if ((string.IsNullOrWhiteSpace(_appName)) || (string.IsNullOrWhiteSpace(_appVersion))) return;
            _appName = _appName.Replace(",", string.Empty).Trim();
            _appVersion = _appVersion.Replace(",", string.Empty).Trim();
            bool _forceDownloadVersion = HttpContext.Current.Request.Headers.AllKeys.Contains(Constants.C_FORCEDOWNLOAD);
            DownloadVersion_if_Present(_appName, _appVersion, _forceDownloadVersion);
        }

        private static void DownloadVersion_if_Present(string inCallingAppName, string inCallingVersion, bool inForceDownload)
        {
            Version _storedVersion = null;
            if (inForceDownload)
            {
                if (!XMLHelper.VersionExists(inCallingAppName, inCallingVersion))
                {
                    FileManager.Write_AppNotFound_in_Response(inCallingAppName, inCallingVersion);
                }
                else
                {
                    _storedVersion = Version.Parse(inCallingVersion);
                }
            }
            else
            {
                _storedVersion = XMLHelper.Get_Last_Version(inCallingAppName);

                if (_storedVersion == null)
                {
                    FileManager.Write_AppNotFound_in_Response(inCallingAppName, string.Empty);
                    return;
                }

                Version _inputVersion = Version.Parse(inCallingVersion);
                if (_inputVersion >= _storedVersion)
                {
                    FileManager.Write_NoGreaterVersion_in_Response(inCallingAppName, _storedVersion);
                    return;
                }
            }
            DownloadFile(inCallingAppName, _storedVersion);
        }

        public static void DownloadFile(string inApplicationName, Version inVersion)
        {
            byte[] _bytes = FileManager.ReadFile_from_Storage(inApplicationName, inVersion);
            FileManager.WriteFileToResponse(_bytes, inApplicationName, inVersion);
        }
    }

    public class Constants
    {
        public const string C_APPNAME = "appname";
        public const string C_APPVERSION = "appversion";
        public const string C_ERRORCODE = "errorcode";
        public const string C_ERRORMSG = "errormsg";
        public const string C_FORCEDOWNLOAD = "forcedownload";

        public enum ErrorCodes
        {
            ecNoError = 0,
            ecNoGreaterVersion = 11,
            ecApplicationNotFound = 12
        }

        public static Dictionary<ErrorCodes, string> ErrorCodesLabels = new Dictionary<ErrorCodes, string>()
                                                                            {
                                                                                {ErrorCodes.ecNoError, string.Empty},
                                                                                {ErrorCodes.ecApplicationNotFound, "Application Not Found"},
                                                                                {ErrorCodes.ecNoGreaterVersion, "No greater version"}
                                                                            };

        //public static string C_ERRORCODE 
    }
}