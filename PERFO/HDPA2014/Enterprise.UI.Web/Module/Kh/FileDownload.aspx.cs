using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;
using System.Data;

using Enterprise.Component.Infrastructure;

namespace Enterprise.UI.Web.Kh
{

    /// <summary>
    /// 文件下载页面
    /// </summary>
    public partial class FileDownload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //string fn = Request["fn"];
            //string docFile = Server.MapPath(string.Format("upload/{0}", fn));
            //if (File.Exists(docFile))
            //{
            //    if (docFile.Contains(".doc"))
            //        Response.ContentType = "application/msword'";
            //    else if (docFile.Contains(".xls"))
            //        Response.ContentType = "application/msexcel'";
            //    Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fn, System.Text.Encoding.UTF8));
            //    Response.TransmitFile(docFile);
            //}
            //else
            //{
            //    Response.Write(string.Format("<script>alert('{0}');</script>", "文件不存在！"));
            //}

            string filename = Request.QueryString["url"];
            string fpath = Request.QueryString["path"];
            string fileRealName = Request.QueryString["fn"];
            string ftype = Request.QueryString["type"];

            Stream iStream = null;
            byte[] buffer = new Byte[10240];
            int length;
            long dataToRead;
            try
            {

                string filepath = "";
                if (ftype == "folder")
                {
                    ////首先执行打包
                    //string fName = filename.Substring(0, filename.LastIndexOf('.'));
                    //bool rbool = ZipServices.CreateZip(fpath, fName, Server.MapPath("~/Temp/"));

                    ////下载的时候按照"文件夹名称.zip"来进行下载
                    //filepath = Server.MapPath("~/Temp/" + filename);
                }
                else
                {
                    if (string.IsNullOrEmpty(fpath))
                    {
                        filepath = Server.MapPath(filename); //待下载的文件路径
                    }
                    else
                    {
                        filepath = Server.MapPath(
                            FileHelper.RootServerFolder + "/" + fpath + "/" + filename);
                    }
                }

                iStream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read);
                Response.Clear();
                dataToRead = iStream.Length;
                long p = 0;
                if (Request.Headers["Range"] != null)
                {
                    Response.StatusCode = 206;
                    p = long.Parse(Request.Headers["Range"].Replace("bytes=", "").Replace("-", ""));
                }
                if (p != 0)
                {
                    Response.AddHeader("Content-Range", "bytes " + p.ToString() + "-" + ((long)(dataToRead - 1)).ToString() + "/" + dataToRead.ToString());
                }
                Response.AddHeader("Content-Length", ((long)(dataToRead - p)).ToString());
                Response.ContentType = "application/octet-stream";
                if (!string.IsNullOrEmpty(fileRealName))
                {
                    Response.AddHeader("Content-Disposition", "attachment; filename=" +
                        System.Web.HttpUtility.UrlEncode(System.Text.Encoding.GetEncoding(65001).GetBytes(fileRealName)));
                }
                else
                {
                    Response.AddHeader("Content-Disposition", "attachment; filename=" +
                        System.Web.HttpUtility.UrlEncode(System.Text.Encoding.GetEncoding(65001).GetBytes(Path.GetFileName(filepath))));
                }

                iStream.Position = p;
                dataToRead = dataToRead - p;
                while (dataToRead > 0)
                {
                    if (Response.IsClientConnected)
                    {
                        length = iStream.Read(buffer, 0, 10240);
                        Response.OutputStream.Write(buffer, 0, length);
                        Response.Flush();
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
                Response.Write("<script type='text/javascript'>alert('未找到指定的文件!');history.go(-1);</script>");
                Debuger.GetInstance().log(this, "未找到指定的文件：【" + filename + "】", ex);
            }
            finally
            {
                if (iStream != null)
                {
                    iStream.Close();
                }
                Response.End();
            }
        }
    }
}