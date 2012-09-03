using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace VersionUpdaterSrv
{
    public class XMLHelper
    {
        public const string C_FILENAME = "FilesDescription.xml";
        public static string XMLFilePath = HttpContext.Current.Server.MapPath(Path.Combine("XMLStore", C_FILENAME));

        private const string C_XE_INFO = "Info";
        private const string C_XE_FILENAME = "Filename";
        private const string C_XE_VERSION = "Version";
        private const string C_XE_CHECKSUM = "Checksum";
        private const string C_XE_DATETIME = "DateTime";

        private static XDocument _filesDescr;
        public static XDocument XMLFilesDescr
        {
            get { return _filesDescr ?? (_filesDescr = LoadOrCreateFile()); }
        }

        /// <summary>
        /// Ако XML-файлът с описанията е наличен, го зарежда и връща като резултат. Ако не е - създава го и пак го връща.
        /// </summary>
        /// <returns></returns>
        private static XDocument LoadOrCreateFile()
        {
            XDocument _doc;
            if (!Directory.Exists(Path.GetDirectoryName(XMLFilePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(XMLFilePath));
            }
            if (File.Exists(XMLFilePath))
            {
                _doc = XDocument.Load(XMLFilePath);
            }
            else
            {
                _doc = new XDocument(new XDeclaration("1.0", Encoding.UTF8.WebName, "yes"), new XElement("Files"));
                _doc.Save(XMLFilePath);
            }
            return _doc;
        }

        /// <summary>
        /// Връща последната налична версия на даден application или NULL, ако няма такъв ApplicationName
        /// </summary>
        /// <param name="inApplicationName"></param>
        /// <returns></returns>
        public static Version Get_Last_Version(string inApplicationName)
        {
            Version _resultVersion = null;
            var _appElement = Get_Application_Node(inApplicationName);
            if (_appElement != null)
            {
                var _strVersions = _appElement.Elements(C_XE_VERSION).Select(v => v.Value);
                var _versions = _strVersions.Select(Version.Parse).OrderByDescending(v => v);
                _resultVersion = _versions.First();
            }
            return _resultVersion;
        }

        /// <summary>
        /// Проверява дали подадената версия съществува за даден ApplicationName. Ползва се при ForceDownload
        /// </summary>
        /// <param name="inApplicationName"></param>
        /// <param name="inVersion"></param>
        /// <returns></returns>
        public static bool VersionExists(string inApplicationName, string inVersion)
        {
            var _appElement = Get_Application_Node(inApplicationName);
            var _exists = _appElement.Elements(C_XE_VERSION).Any(e => e.Value.Equals(inVersion));
            Debug.WriteLine(_exists.ToString());
            return ((_appElement != null) && (_appElement.Elements(C_XE_VERSION).Any(e => e.Value.Equals(inVersion))));
        }

        /// <summary>
        /// Взима целия XELEMENT на последната версия
        /// </summary>
        /// <param name="inApplicationName"></param>
        /// <returns></returns>
        public static XElement Get_Last_Version_Element(string inApplicationName)
        {
            XElement _resultElement = null;
            var _appElements = Get_Application_Node(inApplicationName);
            if (_appElements != null)
            {
                Version _maxVersion = null;
                foreach (XElement _versionElement in _appElements.Elements(C_XE_VERSION))
                {
                    Version _version = Version.Parse(_versionElement.Value);
                    if (_version <= _maxVersion) continue;
                    _maxVersion = _version;
                    _resultElement = _versionElement;
                }
            }
            return _resultElement;
        }

        /// <summary>
        /// Връща XElement за подадения Application, или NULL, ако няма такъв
        /// </summary>
        /// <param name="inApplicationName"></param>
        /// <returns></returns>
        public static XElement Get_Application_Node(string inApplicationName)
        {
            return XMLFilesDescr.Root.Elements().Where(e => e.Name.LocalName.Equals(inApplicationName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        }

        /// <summary>
        /// Създава VersionedFile от XElement по подаден Version-елемент и подчинения му INFO-елемент.
        /// </summary>
        /// <param name="inVersionXElement"></param>
        /// <returns></returns>
        public static VersionedFile GetVersionedFile_from_XElement(XElement inVersionXElement)
        {
            VersionedFile _file = new VersionedFile();
            Version _version;
            _file.Version = (Version.TryParse(inVersionXElement.Value, out _version)) ? _version : null;
            XElement _infoElement = inVersionXElement.Elements().First();
            if (_infoElement == null) throw new NullReferenceException("Не е намерен Info-елемент в елемента Version!");
            _file.FileName = _infoElement.Attribute(C_XE_FILENAME).Value;
            _file.CheckSum = _infoElement.Attribute(C_XE_CHECKSUM).Value;
            _file.DateTime = DateTime.Parse(_infoElement.Attribute(C_XE_DATETIME).Value);
            _file.ApplicationName = inVersionXElement.Parent.Name.LocalName;
            return _file;
        }

        /// <summary>
        /// Прави XElement по подаден VersionedFile-обект.
        /// </summary>
        /// <param name="inVersionedFile"></param>
        /// <returns></returns>
        public static XElement Make_XElement_from_VersionedFile(VersionedFile inVersionedFile)
        {
            XElement _xElement = new XElement(C_XE_VERSION, inVersionedFile.Version.ToString());
            _xElement.RemoveAttributes();
            _xElement.Add(new XElement(C_XE_INFO,
                                       new XAttribute(C_XE_FILENAME, inVersionedFile.FileName),
                                       new XAttribute(C_XE_CHECKSUM, inVersionedFile.CheckSum),
                                       new XAttribute(C_XE_DATETIME, inVersionedFile.DateTime.ToString("dd.MM.yyyy HH:mm:ss"))));
            return _xElement;
        }

        public static void Insert_VersionedFile(VersionedFile inFile)
        {
            XElement _newElement = Make_XElement_from_VersionedFile(inFile);
            XElement _appNode = Get_Application_Node(inFile.ApplicationName);
            if (_appNode == null)
            {
                XMLFilesDescr.Root.Add(new XElement(inFile.ApplicationName));
            }
            XMLFilesDescr.Root.Element(inFile.ApplicationName).Add(_newElement);
            XMLFilesDescr.Save(XMLFilePath);
        }

        /// <summary>
        /// Връща списък с приложенията и последната им версия.
        /// </summary>
        /// <returns></returns>
        public static List<VersionedFile> GetApps_and_LastVersion()
        {
            List<VersionedFile> _resList = new List<VersionedFile>();
            var _appElements = XMLFilesDescr.Root.Elements();
            foreach (XElement _appElement in _appElements)
            {
                XElement _lastVersionElement = Get_Last_Version_Element(_appElement.Name.LocalName);
                VersionedFile _verFile = GetVersionedFile_from_XElement(_lastVersionElement);
                _resList.Add(_verFile);
            }
            return _resList;
        }

        /// <summary>
        /// Връща списък с версиите на избраното приложение
        /// </summary>
        /// <returns></returns>
        public static List<VersionedFile> GetVersions(string inApplicationName)
        {
            var _appNode = Get_Application_Node(inApplicationName);
            if (_appNode == null) return null;
            List<VersionedFile> _resList = new List<VersionedFile>();
            foreach (XElement _version in _appNode.Elements())
            {
                VersionedFile _verFile = GetVersionedFile_from_XElement(_version);
                _resList.Add(_verFile);
            }
            return _resList.OrderBy(e=>e.Version).ToList();
        }

    }
}