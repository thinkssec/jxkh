using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Enterprise.Service.Perfo.Sys;
using Enterprise.Service.Perfo.Zbk;
using Enterprise.Model.Perfo.Zbk;
using Enterprise.Model.Perfo.Sys;
using Enterprise.Service.Perfo.Kh;
using Enterprise.Model.Perfo.Kh;
using Enterprise.Component.Infrastructure;

namespace Enterprise.UI.Web.Component.FlexPaper
{

    /// <summary>
    /// PDF 查看器
    /// </summary>
    public partial class PdfViewer : System.Web.UI.Page
    {

        /// <summary>
        /// 下载文件显示名称
        /// </summary>
        protected string fn = (string)Utility.sink("fn", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);
        /// <summary>
        /// PDF转为SWF格式后的文件名称（含路径）
        /// </summary>
        protected string SwfFileName = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                OnStart();
            }
        }

        /// <summary>
        /// 初始化数据内容
        /// </summary>
        private void OnStart()
        {
            if (!string.IsNullOrEmpty(fn))
            {
                SwfFileName = Base64.Base64Decode(fn);
            }
        }
    }
}