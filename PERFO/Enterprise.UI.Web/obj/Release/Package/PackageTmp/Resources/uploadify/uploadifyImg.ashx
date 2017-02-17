<%@ WebHandler Language="C#" Class="uploadifyImg" %>
using System;
using System.Web;
using System.IO;

public class uploadifyImg : IHttpHandler
{
    protected System.Web.UI.HtmlControls.HtmlInputFile filMyFile;
    
    public void ProcessRequest(HttpContext context)
    {
        //string saveFilePath = context.Server.MapPath(VirtualPathUtility.GetDirectory(context.Request.Path));
        
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
            string dirPath = savePath + "image/";
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

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}