using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Enterprise.Component.Infrastructure;
namespace NewFileManage.Web.Content.Ashx
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>   
    public class FileDownload : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string filename = context.Request.QueryString["url"];
            string fpath = context.Request.QueryString["path"];
            string fileRealName = context.Request.QueryString["fn"];
            string ftype = context.Request.QueryString["type"];

            Stream iStream = null;
            byte[] buffer = new Byte[10240];
            int length;
            long dataToRead;
            try
            {
                
                string filepath="";
                if (ftype == "folder")
                {
                    ////首先执行打包
                    //string fName = filename.Substring(0, filename.LastIndexOf('.'));
                    //bool rbool = ZipServices.CreateZip(fpath, fName, HttpContext.Current.Server.MapPath("~/Temp/"));

                    ////下载的时候按照"文件夹名称.zip"来进行下载
                    //filepath = HttpContext.Current.Server.MapPath("~/Temp/" + filename);
                }
                else
                {
                    if (string.IsNullOrEmpty(fpath))
                    {
                        filepath = HttpContext.Current.Server.MapPath(filename); //待下载的文件路径
                    }
                    else
                    {
                        filepath = HttpContext.Current.Server.MapPath(
                            FileHelper.RootServerFolder + "/" + fpath + "/" + filename);
                    }
                }

                iStream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read);
                context.Response.Clear();
                dataToRead = iStream.Length;
                long p = 0;
                if (context.Request.Headers["Range"] != null)
                {
                    context.Response.StatusCode = 206;
                    p = long.Parse(context.Request.Headers["Range"].Replace("bytes=", "").Replace("-", ""));
                }
                if (p != 0)
                {
                    context.Response.AddHeader("Content-Range", "bytes " + p.ToString() + "-" + ((long)(dataToRead - 1)).ToString() + "/" + dataToRead.ToString());
                }
                context.Response.AddHeader("Content-Length", ((long)(dataToRead - p)).ToString());
                context.Response.ContentType = "application/octet-stream";
                if (!string.IsNullOrEmpty(fileRealName))
                {
                    context.Response.AddHeader("Content-Disposition", "attachment; filename=" +
                        System.Web.HttpUtility.UrlEncode(System.Text.Encoding.GetEncoding(65001).GetBytes(fileRealName)));
                }
                else
                {
                    context.Response.AddHeader("Content-Disposition", "attachment; filename=" +
                        System.Web.HttpUtility.UrlEncode(System.Text.Encoding.GetEncoding(65001).GetBytes(Path.GetFileName(filepath))));
                }

                iStream.Position = p;
                dataToRead = dataToRead - p;
                while (dataToRead > 0)
                {
                    if (context.Response.IsClientConnected)
                    {
                        length = iStream.Read(buffer, 0, 10240);
                        context.Response.OutputStream.Write(buffer, 0, length);
                        context.Response.Flush();
                        buffer = new Byte[10240];
                        dataToRead = dataToRead - length;
                    }
                    else
                    {
                        dataToRead = -1;
                    }
                }

                #region "写入日志文件"
                //LogServices.WriteLog("操作 : 下载文件 ; 文件名称: " + filename);
                #endregion
                ////当下载完成后删除该文件
                //if (ftype == "folder")
                //{
                //    File.Delete(filepath);
                //}
            }
            catch (Exception ex)
            {
                context.Response.Write("<script type='text/javascript'>alert('未找到指定的文件!');history.go(-1);</script>");
                Debuger.GetInstance().log(this, "未找到指定的文件：【" + filename + "】", ex);
            }
            finally
            {
                if (iStream != null)
                {
                    iStream.Close();
                }
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
}
