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
            Debug.WriteLine(_appName + "; " + _appVersion);
            DownloadLastVersion_if_Present(_appName, _appVersion);
        }

        private static void DownloadLastVersion_if_Present(string inAppName, string inAppVersion)
        {
            Version _lastAvailableVersion = XMLHelper.Get_Last_Version(inAppName);
            if (_lastAvailableVersion == null)
            {
                FileManager.Write_AppNotFound_in_Response(inAppName);
                return;
            }
            Version _inputVersion = Version.Parse(inAppVersion);
            if (_inputVersion >= _lastAvailableVersion)
            {
                FileManager.Write_NoGreaterVersion_in_Response(inAppName, _lastAvailableVersion);
                return;
            }
            else
            {
                DownloadFile(inAppName, _lastAvailableVersion);
            }
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