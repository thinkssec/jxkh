using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enterprise.Component.Infrastructure;

namespace Enterprise.UI.Web.Component.UserControl
{
    public partial class EFileUpload : System.Web.UI.UserControl
    {
        public string FileTypeExt { get { return fileType.Value; } set { fileType.Value = value; } }
        public string ReturnId { get; set; }
        public string ReturnFileNameId { get; set; }
        public string Muti { get; set; }
        public string OnBindDDL { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string rq = Request.QueryString["ext"].ToRequestString();
                ReturnId = Request.QueryString["rtnId"].ToRequestString();
                ReturnFileNameId = Request.QueryString["rtnNameId"].ToRequestString();
                Muti = Request.QueryString["muti"].ToRequestString();
                //OnBindDDL = "OnBindDDL_list";
                if (!string.IsNullOrEmpty(rq))
                {
                    try
                    {
                        FileType rqExt = (FileType)Enum.Parse(typeof(FileType), rq);
                        string ext = Request.QueryString["custom"].ToRequestString();
                        switch (rqExt)
                        {
                            case FileType.Img:
                                FileTypeExt = "*.jpg;*.gif;*.bmp;*.jpeg;*.png";
                                break;
                            case FileType.Office:
                                FileTypeExt = "*.doc;*.xls;*.pdf;*.docx;*.xlsx;*.ppt;*.pptx;*.rar;*.zip";
                                break;
                            case FileType.Custom:
                                FileTypeExt = ext;
                                break;
                            //case    FileType.ImgOffice:
                            //    FileTypeExt = "*.jpg;*.gif;*.bmp;*.jpeg;*.png;*.doc;*.xls;*.pdf;*.docx;*.xlsx;*.ppt;*.pptx;*.rar;*.zip";
                            //    break;
                            default:
                                break;
                        }
                    }
                    catch(Exception ex)
                    {                        
                        Utility.ShowMsg(this.Page, "错误的参数！");
                    }
                }
                
            }
        }
    }

    public enum FileType
    {
        //ImgOffice,
        Img,
        Office,
        Custom
    }
}