using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.SessionState;

namespace Enterprise.Component.Infrastructure
{
    /// <summary>
    /// 文件操作方法类
    /// </summary>
    public class FileHelper
    {

        /// <summary>
        /// 固定共享文件夹名称
        /// </summary>
        static string StorageFolder
        {
            get { return HttpContext.Current.Server.MapPath("~/Public"); }
        }

        /// <summary>
        /// 根目录文件夹
        /// </summary>
        public static string RootFolder(string folderName)
        {
            string rootFolder = StorageFolder + Path.DirectorySeparatorChar + folderName;
            if (!Directory.Exists(rootFolder))
            {
                Directory.CreateDirectory(rootFolder);
            }
            Directory.SetCreationTime(rootFolder, DateTime.Now);
            return rootFolder;
        }

        /// <summary>
        /// 根目录
        /// </summary>
        public static string RootServerFolder
        {
            get { return "~/Public"; }
        }

        /// <summary>
        /// 临时文件夹
        /// </summary>
        public static string RootServerTempFolder
        {
            get { return HttpContext.Current.Server.MapPath("~/Temp/"); }
        }

        /// <summary>
        /// 利用Base64码对文件名进行加密处理。
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string Encrypt(string _str)
        {
            return HttpContext.Current.Server.UrlEncode(_str);
        }

        /// <summary>
        /// 利用Base64码对文件名进行解密处理。
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string Decrypt(string _str)
        {
            return HttpContext.Current.Server.UrlDecode(_str);
        }

        /// <summary>
        /// 获得物理路径
        /// </summary>
        /// <param name="a">路径</param>
        /// <returns>物理路径</returns>
        public static string GetFullPath(string a)
        {
            string AppDir = System.AppDomain.CurrentDomain.BaseDirectory;
            if (a.IndexOf(":") < 0)
            {
                string str = a.Replace("..\\", "");
                if (str != a)
                {
                    int Num = (a.Length - str.Length) / ("..\\").Length + 1;
                    for (int i = 0; i < Num; i++)
                    {
                        AppDir = AppDir.Substring(0, AppDir.LastIndexOf("\\"));
                    }
                    str = "\\" + str;

                }
                a = AppDir + str;
            }
            return a;
        }

        /// <summary>
        /// 获得日志文件存放目录
        /// </summary>
        public static string LogDir
        {
            get
            {
                string LogFilePath = GetFullPath("Log");
                if (!Directory.Exists(LogFilePath))
                    Directory.CreateDirectory(LogFilePath);
                return LogFilePath;
            }
        }

        /// <summary>
        /// 获取用户IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetIPAddress()
        {

            string user_IP = string.Empty;
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            {
                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                {
                    user_IP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                }
                else
                {
                    user_IP = System.Web.HttpContext.Current.Request.UserHostAddress;
                }
            }
            else
            {
                user_IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            return user_IP;
        }

        /// <summary>
        /// 获取当前访问页面地址参数
        /// </summary>
        public static string GetScriptNameQueryString
        {
            get
            {
                return HttpContext.Current.Request.ServerVariables["QUERY_STRING"].ToString();
            }
        }

        /// <summary>
        /// 获取当前访问页面地址
        /// </summary>
        public static string GetScriptName
        {
            get
            {
                return HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"].ToString();
            }
        }

        /// <summary>
        /// 检测当前url是否包含指定的字符
        /// </summary>
        /// <param name="sChar">要检测的字符</param>
        /// <returns></returns>
        public static bool CheckScriptNameChar(string sChar)
        {
            bool rBool = false;
            if (GetScriptName.ToLower().LastIndexOf(sChar) >= 0)
                rBool = true;
            return rBool;
        }

        /// <summary>
        /// 获取当前页面的扩展名
        /// </summary>
        public static string GetScriptNameExt
        {
            get
            {
                return GetScriptName.Substring(GetScriptName.LastIndexOf(".") + 1);
            }
        }


        /// <summary>
        /// 获取当前访问页面Url
        /// </summary>
        public static string GetScriptUrl
        {
            get
            {
                return GetScriptNameQueryString == "" ? GetScriptName : string.Format("{0}?{1}", GetScriptName, GetScriptNameQueryString);
            }
        }

        public static string GetFileIco(string fext)
        {
            string ext = "dir";
            switch (fext)
            {

                case ("ext.xls"):
                    ext = "2";
                    break;

                case ("ext.xlsx"):
                    ext = "2";
                    break;

                case ("ext.csv"):
                    ext = "2";
                    break;

                case ("ext.doc"):
                    ext = "3";
                    break;

                case ("ext.docx"):
                    ext = "3";
                    break;

                case ("ext.wps"):
                    ext = "3";
                    break;

                case ("ext.ppt"):
                    ext = "4";
                    break;

                case ("ext.pptx"):
                    ext = "4";
                    break;

                case ("ext.7z"):
                    ext = "5";
                    break;

                case ("ext.rar"):
                    ext = "6";
                    break;

                case ("ext.cab"):
                    ext = "6";
                    break;

                case ("ext.jar"):
                    ext = "6";
                    break;

                case ("ext.arj"):
                    ext = "6";
                    break;

                case ("ext.tar"):
                    ext = "6";
                    break;

                case ("ext.gz"):
                    ext = "6";
                    break;

                case ("ext.tgz"):
                    ext = "6";
                    break;

                case ("ext.zip"):
                    ext = "7";
                    break;

                case ("ext.iso"):
                    ext = "8";
                    break;

                case ("ext.img"):
                    ext = "8";
                    break;

                case ("ext.nrg"):
                    ext = "8";
                    break;

                case ("ext.mdf"):
                    ext = "8";
                    break;

                case ("ext.mds"):
                    ext = "8";
                    break;

                case ("ext.eml"):
                    ext = "9";
                    break;

                case ("ext.htm"):
                    ext = "10";
                    break;

                case ("ext.html"):
                    ext = "10";
                    break;

                case ("ext.shtml"):
                    ext = "10";
                    break;

                case ("ext.xhtml"):
                    ext = "10";
                    break;

                case ("ext.mht"):
                    ext = "10";
                    break;

                case ("ext.asp"):
                    ext = "10";
                    break;

                case ("ext.aspx"):
                    ext = "10";
                    break;

                case ("ext.js"):
                    ext = "11";
                    break;

                case ("ext.css"):
                    ext = "12";
                    break;

                case ("ext.ini"):
                    ext = "12";
                    break;

                case ("ext.inf"):
                    ext = "12";
                    break;

                case ("ext.xml"):
                    ext = "12";
                    break;

                case ("ext.hlp"):
                    ext = "13";
                    break;

                case ("ext.chm"):
                    ext = "13";
                    break;

                case ("ext.exe"):
                    ext = "14";
                    break;

                case ("ext.app"):
                    ext = "14";
                    break;

                case ("ext.com"):
                    ext = "14";
                    break;

                case ("ext.bat"):
                    ext = "15";
                    break;

                case ("ext.cmd"):
                    ext = "15";
                    break;

                case ("ext.ttf"):
                    ext = "16";
                    break;

                case ("ext.fon"):
                    ext = "16";
                    break;

                case ("ext.pdf"):
                    ext = "17";
                    break;

                case ("ext.psd"):
                    ext = "18";
                    break;

                case ("ext.pdd"):
                    ext = "18";
                    break;

                case ("ext.ps"):
                    ext = "18";
                    break;

                case ("ext.ai"):
                    ext = "19";
                    break;

                case ("ext.eps"):
                    ext = "19";
                    break;

                case ("ext.fla"):
                    ext = "20";
                    break;

                case ("ext.swf"):
                    ext = "21";
                    break;

                case ("ext.flv"):
                    ext = "21";
                    break;

                case ("ext.txt"):
                    ext = "22";
                    break;

                case ("ext.java"):
                    ext = "22";
                    break;

                case ("ext.jsp"):
                    ext = "22";
                    break;

                case ("ext.c"):
                    ext = "22";
                    break;

                case ("ext.cpp"):
                    ext = "22";
                    break;

                case ("ext.h"):
                    ext = "22";
                    break;

                case ("ext.hpp"):
                    ext = "22";
                    break;

                case ("ext.py"):
                    ext = "22";
                    break;

                case ("ext.cs"):
                    ext = "22";
                    break;

                case ("ext.sh"):
                    ext = "22";
                    break;

                case ("ext.rtf"):
                    ext = "23";
                    break;

                case ("ext.reg"):
                    ext = "24";
                    break;

                case ("ext.ra"):
                    ext = "25";
                    break;

                case ("ext.ram"):
                    ext = "25";
                    break;

                case ("ext.rpm"):
                    ext = "25";
                    break;

                case ("ext.rmx"):
                    ext = "25";
                    break;

                case ("ext.rm"):
                    ext = "25";
                    break;

                case ("ext.rmvb"):
                    ext = "25";
                    break;

                case ("ext.wm"):
                    ext = "26";
                    break;

                case ("ext.wma"):
                    ext = "26";
                    break;

                case ("ext.wmv"):
                    ext = "26";
                    break;

                case ("ext.asf"):
                    ext = "26";
                    break;

                case ("ext.wmp"):
                    ext = "26";
                    break;

                case ("ext.mov"):
                    ext = "27";
                    break;

                case ("ext.qt"):
                    ext = "27";
                    break;

                case ("ext.3gp"):
                    ext = "27";
                    break;

                case ("ext.3gpp"):
                    ext = "27";
                    break;

                case ("ext.amr"):
                    ext = "27";
                    break;

                case ("ext.avi"):
                    ext = "28";
                    break;

                case ("ext.mkv"):
                    ext = "28";
                    break;

                case ("ext.mp4"):
                    ext = "28";
                    break;

                case ("ext.mpg"):
                    ext = "28";
                    break;

                case ("ext.mpeg"):
                    ext = "28";
                    break;

                case ("ext.vob"):
                    ext = "28";
                    break;

                case ("ext.dat"):
                    ext = "28";
                    break;

                case ("ext.ts"):
                    ext = "28";
                    break;

                case ("ext.tp"):
                    ext = "28";
                    break;

                case ("ext.m2ts"):
                    ext = "28";
                    break;

                case ("ext.evo"):
                    ext = "28";
                    break;

                case ("ext.pmp"):
                    ext = "28";
                    break;

                case ("ext.vp6"):
                    ext = "28";
                    break;

                case ("ext.ivf"):
                    ext = "28";
                    break;

                case ("ext.dsm"):
                    ext = "28";
                    break;

                case ("ext.dsv"):
                    ext = "28";
                    break;

                case ("ext.dsa"):
                    ext = "28";
                    break;

                case ("ext.dss"):
                    ext = "28";
                    break;

                case ("ext.fli"):
                    ext = "28";
                    break;

                case ("ext.flc"):
                    ext = "28";
                    break;

                case ("ext.flic"):
                    ext = "28";
                    break;

                case ("ext.roq"):
                    ext = "28";
                    break;

                case ("ext.mpa"):
                    ext = "29";
                    break;

                case ("ext.mla"):
                    ext = "29";
                    break;

                case ("ext.m2a"):
                    ext = "29";
                    break;

                case ("ext.mp2"):
                    ext = "29";
                    break;

                case ("ext.mp3"):
                    ext = "29";
                    break;

                case ("ext.ac3"):
                    ext = "29";
                    break;

                case ("ext.dts"):
                    ext = "29";
                    break;
                case ("ext.ddp"):
                    ext = "29";
                    break;
                case ("ext.flac"):
                    ext = "29";
                    break;
                case ("ext.ape"):
                    ext = "29";
                    break;
                case ("ext.mac"):
                    ext = "29";
                    break;
                case ("ext.apl"):
                    ext = "29";
                    break;
                case ("ext.shn"):
                    ext = "29";
                    break;
                case ("ext.tta"):
                    ext = "29";
                    break;
                case ("ext.wv"):
                    ext = "29";
                    break;
                case ("ext.cda"):
                    ext = "29";
                    break;
                case ("ext.wav"):
                    ext = "29";
                    break;
                case ("ext.aac"):
                    ext = "29";
                    break;
                case ("ext.m4a"):
                    ext = "29";
                    break;
                case ("ext.mka"):
                    ext = "29";
                    break;
                case ("ext.ogg"):
                    ext = "29";
                    break;
                case ("ext.mpc"):
                    ext = "29";
                    break;
                case ("ext.mp+"):
                    ext = "29";
                    break;
                case ("ext.mpp"):
                    ext = "29";
                    break;
                case ("ext.au"):
                    ext = "29";
                    break;
                case ("ext.aif"):
                    ext = "29";
                    break;
                case ("ext.aifc"):
                    ext = "29";
                    break;
                case ("ext.aiff"):
                    ext = "29";
                    break;
                case ("ext.mid"):
                    ext = "29";
                    break;
                case ("ext.midi"):
                    ext = "29";
                    break;
                case ("ext.rmi"):
                    ext = "29";
                    break;
                case ("ext.todo"):
                    ext = "30";
                    break;
                case ("ext.jpg"):
                    ext = "31";
                    break;
                case ("ext.jpeg"):
                    ext = "31";
                    break;
                case ("ext.gif"):
                    ext = "32";
                    break;
                case ("ext.png"):
                    ext = "33";
                    break;
                case ("ext.bmp"):
                    ext = "34";
                    break;
                case ("ext.dib"):
                    ext = "34";
                    break;
                case ("ext.rle"):
                    ext = "34";
                    break;
                case ("ext.tif"):
                    ext = "35";
                    break;
                case ("ext.tiff"):
                    ext = "35";
                    break;
                case ("ext.xif"):
                    ext = "35";
                    break;
                case ("ext.emf"):
                    ext = "36";
                    break;
                case ("ext.wmf"):
                    ext = "37";
                    break;
                case ("ext.ico"):
                    ext = "38";
                    break;
                case ("ext.icl"):
                    ext = "38";
                    break;
                case ("ext.cur"):
                    ext = "38";
                    break;
                case ("ext.ani"):
                    ext = "38";
                    break;
                case ("ext.raw"):
                    ext = "39";
                    break;
                case ("ext.mos"):
                    ext = "39";
                    break;
                case ("ext.fff"):
                    ext = "39";
                    break;
                case ("ext.032"):
                    ext = "39";
                    break;
                case ("ext.bay"):
                    ext = "39";
                    break;
                case ("ext.cdr"):
                    ext = "40";
                    break;
                case ("ext.csl"):
                    ext = "40";
                    break;
                case ("ext.cmx"):
                    ext = "40";
                    break;
                case ("ext.wpg"):
                    ext = "40";
                    break;
                case ("ext.dll"):
                    ext = "41";
                    break;
                case ("ext.ocx"):
                    ext = "41";
                    break;
                case ("ext.chk"):
                    ext = "41";
                    break;
                case ("ext.manifest"):
                    ext = "41";
                    break;
                case ("ext.torrent"):
                    ext = "42";
                    break;
            }
            return string.Format("<b class=\"ico ico-file ico-file-{0}\"></b>", ext);
        }

    }
}
