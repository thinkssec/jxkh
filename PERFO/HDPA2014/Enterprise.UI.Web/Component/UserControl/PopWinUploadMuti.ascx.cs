using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enterprise.Component.Infrastructure;

namespace Enterprise.UI.Web.Component.UserControl
{
    public partial class PopWinUploadMuti : System.Web.UI.UserControl
    {
        /// <summary>
        /// 多文件上传
        /// </summary>
        public bool Muti { get; set; }

        /// <summary>
        /// 上传地址 不用指定
        /// </summary>
        public string UploadUrl { get; set; }

        /// <summary>
        /// 宽度
        /// </summary>
        public Unit Width { get {
            if (!Muti)
                return tb_UploadSingle_TextBox.Width; 
            else 
                return ddl_DropDownList_muti.Width;
        }
            set
            {
                if (!Muti)
                {
                    tb_UploadSingle_TextBox.Width = value;
                }
                else
                    ddl_DropDownList_muti.Width = value;
            }
        }

        /// <summary>
        /// 文件类型
        /// </summary>
        public FileType Ext { get; set; }

        
        /// <summary>
        /// 必填
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// 自定义文件类型 文件扩展名
        /// </summary>
        public string Custom { get; set; }

        /// <summary>
        /// 竖线隔开的多个文件名称
        /// </summary>
        public string Text
        {
            get
            {
                if (Muti)
                {
                    string str = hd_UploadMuti_PostBackName.Value;
                    str = str.TrimEnd('|'); 
                    return str;
                }
                else
                {
                    return tb_UploadSingle_TextBox.Text;
                }
            }
            set
            {
                if (Muti)
                {
                    //赋值时赋给隐藏域
                    hd_UploadMuti_PostBackName.Value = value;                    
                }
                else
                {
                    //赋Text时给显示控件
                    tb_UploadSingle_TextBox.Text = value;
                }

            }
        }

        /// <summary>
        /// 竖线隔开的多个文件路径
        /// </summary>
        public string Value
        {
            get
            {
                if (Muti)
                {
                    string str = hd_UploadMuti_PostBackValue.Value ;
                    str = str.TrimEnd('|');                                 
                    return str;
                }
                else
                {
                    return hd_UploadSingle_Value.Value;
                }
            }
            set
            {
                if (Muti)
                {
                    if (value != null)
                    {
                        //赋值时赋给隐藏域
                        hd_UploadMuti_PostBackValue.Value = value.TrimEnd('|');
                        string[] str = value.Split('|');
                        if (Text != "")
                        {
                            string[] val = Text.Split('|');
                            for (int i = 0; i < str.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(str[i]) && !string.IsNullOrEmpty(val[i]))
                                    ddl_DropDownList_muti.Items.Add(new ListItem(val[i], str[i]));

                            }
                        }
                    }
                }
                else
                {
                    hd_UploadSingle_Value.Value = value;
                }
                
            }
        }

        /// <summary>
        /// 数据库格式为: 文件名 * 路径名 | 文件名 * 路径名 
        /// </summary>
        public string DBValue
        {
            get
            {
                string tmp="";
                if (!string.IsNullOrEmpty(Text) && !string.IsNullOrEmpty(Value))
                {
                    string[] str1 = Text.Split('|');
                    string[] str2 = Value.Split('|');
                    for (int i = 0; i < str2.Length;i++ )
                    {                        
                        tmp += str1[i] + "*" + str2[i]+"|";
                    }
                    tmp = tmp.TrimEnd('|');
                }
                return tmp;
                
            }
            set 
            {
                //分解给Text和value
                if (!string.IsNullOrEmpty(value))
                {
                    //将文件名用竖线隔开
                    string txt = "";
                    //将值用竖线隔开
                    string val = "";
                    string[] str1 = value.Split('|');
                    for (int i = 0; i < str1.Length; i++)
                    {

                        txt += str1[i].Split('*').First()+"|";
                        val += str1[i].Split('*').Last() + "|";
                    }
                    txt.TrimEnd('|');
                    val.TrimEnd('|');
                    Text = txt;
                    Value = val;
                }
            }
        }

        /// <summary>
        /// 不用指定 用于定义JS函数
        /// </summary>
        public string RtnId { get; set; }

        public PopWinUploadMuti()
        {
            Muti = true;
            Required = false;
            Ext = FileType.Office;
        }

        protected void Page_Load(object sender, EventArgs e)
        {         
            
            if (Muti)
            {
                //如果是多文件上传
                RtnId = hd_UploadMuti_Value.ClientID;
                ddl_DropDownList_muti.Visible = true;
                if (Required)
                {
                    ddl_DropDownList_muti.Attributes.Add("required", "true");
                }
                UploadUrl = "/Resources/OA/upload/MutiFileUpload.aspx" + "?rtnId=" + RtnId + "&ext=" + Ext + "&custom=" + Custom + "&muti=" + Muti.ToString().ToLower() + "&rtnNameId=" + hd_UploadMuti_Name.ClientID;
            }
            else
            {
                //单文件上传
                tb_UploadSingle_TextBox.Attributes.Add("readonly", "true");
                ddl_DropDownList_muti.Visible = false;
                //返回值ID
                RtnId = hd_UploadSingle_Value.ClientID;
                if (Required)
                {
                    tb_UploadSingle_TextBox.Attributes.Add("required", "true");
 
                }
                UploadUrl = "/Resources/OA/upload/MutiFileUpload.aspx" + "?rtnId=" + RtnId + "&ext=" + Ext + "&custom=" + Custom + "&muti=" + Muti.ToString().ToLower() + "&rtnNameId=" + tb_UploadSingle_TextBox.ClientID;
            }

            

           

            
        }
    }
}