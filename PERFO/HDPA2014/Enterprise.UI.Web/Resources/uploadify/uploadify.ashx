<%@ WebHandler Language="C#" Class="uploadify" %>
using System;
using System.Web;
using System.IO;
using Enterprise.Component.Infrastructure;

public class uploadify : IHttpHandler
{
    protected System.Web.UI.HtmlControls.HtmlInputFile filMyFile;
    
    public void ProcessRequest(HttpContext context)
    {

        if (!string.IsNullOrEmpty(context.Request.QueryString["path"]) &&
            context.Request.Files["Filedata"] != null)
        {
            //指定文件保存目录
            string saveFilePath = FileHelper.RootFolder(context.Request.QueryString["path"]);
            string saveUrlPath = "/Public/" + context.Request.QueryString["path"] + "/";
            HttpPostedFile myFile = context.Request.Files["Filedata"];
            int nFileLen = myFile.ContentLength;
            byte[] myData = new byte[nFileLen];
            myFile.InputStream.Read(myData, 0, nFileLen);
            //文件保存名称
            string fileName = myFile.FileName;
            System.IO.FileStream newFile = new System.IO.FileStream(saveFilePath + Path.DirectorySeparatorChar + fileName, System.IO.FileMode.Create);
            newFile.Write(myData, 0, myData.Length);
            newFile.Close();

            //输出实际保存的文件名
            context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
            context.Response.Write(saveUrlPath + fileName);
            context.Response.End();
        }
        else
        {
            //文件保存目录路径
            string savePath = "/Public/Platform/" + DateTime.Now.Year + "/";
            if (null != context.Request.Files["Filedata"])
            {
                HttpPostedFile myFile = context.Request.Files["Filedata"];
                int nFileLen = myFile.ContentLength;
                byte[] myData = new byte[nFileLen];
                myFile.InputStream.Read(myData, 0, nFileLen);
                //文件名
                string name = Path.GetFileNameWithoutExtension(myFile.FileName);
                //扩展名
                string fileExt = Path.GetExtension(myFile.FileName).ToLower();
                //创建文件夹
                string dirPath = savePath + "file/";
                string ymd = DateTime.Now.ToString("yyyyMMdd");
                dirPath += ymd + "/";
                //保存路径
                string saveFilePath = context.Server.MapPath(dirPath);
                if (!Directory.Exists(saveFilePath))
                {
                    Directory.CreateDirectory(saveFilePath);
                }
                //文件保存名称 name + "_" + 
                string fileName = DateTime.Now.ToString("yyyyMMddHHmm") + Guid.NewGuid().ToString().Substring(0, 4) + fileExt;
                System.IO.FileStream newFile = new System.IO.FileStream(saveFilePath + fileName, System.IO.FileMode.Create);
                newFile.Write(myData, 0, myData.Length);
                newFile.Close();

                //输出实际保存的文件名
                context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
                context.Response.Write(dirPath + fileName);
                context.Response.End();
            }
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}