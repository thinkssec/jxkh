using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Enterprise.Component.Infrastructure
{
    /// <summary>
    /// 带分组功能的下拉列表控件
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:SSECDropDownList runat=server></{0}:SSECDropDownList>")]
    public class SSECDropDownList : DropDownList
    {
        /// <summary>
        /// 分组名称
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public virtual string OptionGroupValue
        {
            get
            {
                string s = (string)ViewState["OptionGroupValue"];
                return (s == null) ? "optgroup" : s;
            }
            set
            {
                ViewState["OptionGroupValue"] = value;
            }
        }

        /// <summary>
        /// 渲染内容
        /// </summary>
        /// <param name="writer"></param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            // 是否需要呈现OptionGroup的EndTag
            bool writerEndTag = false;

            foreach (ListItem li in this.Items)
            {
                // 如果没有optgroup属性则呈现Option
                if (li.Value != this.OptionGroupValue)
                {
                    // 呈现Option
                    RenderListItem(li, writer);
                }
                // 如果有optgroup属性则呈现OptionGroup
                else
                {
                    if (writerEndTag)
                        // 呈现OptionGroup的EndTag
                        OptionGroupEndTag(writer);
                    else
                        writerEndTag = true;

                    // 呈现OptionGroup的BeginTag
                    OptionGroupBeginTag(li, writer);
                }
            }

            if (writerEndTag)
                // 呈现OptionGroup的EndTag
                OptionGroupEndTag(writer);
        }

        ///
        /// 呈现OptionGroup的BeginTag
        ///
        /// OptionGroup数据项
        /// writer
        private void OptionGroupBeginTag(ListItem li, HtmlTextWriter writer)
        {
            writer.WriteBeginTag("optgroup");

            // 写入OptionGroup的label
            writer.WriteAttribute("label", li.Text);

            foreach (string key in li.Attributes.Keys)
            {
                // 写入OptionGroup的其它属性
                writer.WriteAttribute(key, li.Attributes[key]);
            }

            writer.Write(HtmlTextWriter.TagRightChar);
            writer.WriteLine();
        }

        ///
        /// 呈现OptionGroup的EndTag
        ///
        /// writer
        private void OptionGroupEndTag(HtmlTextWriter writer)
        {
            writer.WriteEndTag("optgroup");
            writer.WriteLine();
        }

        ///
        /// 呈现Option
        ///
        /// Option数据项
        /// writer
        private void RenderListItem(ListItem li, HtmlTextWriter writer)
        {
            writer.WriteBeginTag("option");

            // 写入Option的Value
            writer.WriteAttribute("value", li.Value, true);

            if (li.Selected)
            {
                // 如果该Option被选中则写入selected
                writer.WriteAttribute("selected", "selected", false);
            }

            foreach (string key in li.Attributes.Keys)
            {
                // 写入Option的其它属性
                writer.WriteAttribute(key, li.Attributes[key]);
            }

            writer.Write(HtmlTextWriter.TagRightChar);

            // 写入Option的Text
            HttpUtility.HtmlEncode(li.Text, writer);

            writer.WriteEndTag("option");
            writer.WriteLine();
        }
    }
}
