using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Text;
using System.Data;
//using System.Data.SqlClient;
//using System.Data.OracleClient;

namespace Enterprise.Component.Infrastructure
{
    /// <summary>
    /// 公共方法类
    /// </summary>
    public sealed partial class CommonTool
    {


        private static long pkId; // 唯一Long型主码
        //同步
        private static object _synRoot = new Object();

        /// <summary>
        /// 得到以时间为依据的主键,17位
        /// </summary>
        /// <returns></returns>
        public static string GetPkId()
        {
            lock (_synRoot)
            {
                long lTmp = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                if (pkId < lTmp)
                {
                    pkId = lTmp;
                }
                else
                {
                    pkId++;
                }
            }
            return pkId + "";
        }



        /// <summary>
        /// 得到以时间为依据的主键,14位
        /// </summary>
        /// <returns></returns>
        public static string GetPkIdForDrawing()
        {
            lock (_synRoot)
            {
                long lTmp = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmss"));
                if (pkId < lTmp)
                {
                    pkId = lTmp;
                }
                else
                {
                    pkId++;
                }
            }
            return pkId + "";
        }



        #region 数据对象封装相关======================================

        /// <summary>
        /// 实现页面表单数据自动获取,并装配到对应的MODEL实例属性中
        /// </summary>
        /// <param name="t">实例类型</param>
        /// <param name="tabOptionItem">控件容器</param>
        /// <returns></returns>
        public static Object GetFormDataToModel(Type t, Control tabOptionItem)
        {
            Object obj = Activator.CreateInstance(t);
            if (tabOptionItem == null) return obj;

            PropertyInfo[] pi = t.GetProperties();
            foreach (PropertyInfo p in pi)
            {
                MethodInfo m = p.GetSetMethod();
                if (m == null) continue;
                ParameterInfo[] pai = m.GetParameters();
                if (pai == null) continue;
                try
                {
                    if (tabOptionItem.FindControl("Txt_" + p.Name) != null &&
                        tabOptionItem.FindControl("Txt_" + p.Name) is TextBox)
                    {
                        TextBox txtBox = tabOptionItem.FindControl("Txt_" + p.Name) as TextBox;
                        if (!txtBox.Visible) continue;

                        if (pai[0].ParameterType.IsGenericType)
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(txtBox.Text, pai[0].ParameterType.GetGenericArguments()[0]) });
                        }
                        else if (typeof(IConvertible).IsAssignableFrom(pai[0].ParameterType))
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(txtBox.Text, pai[0].ParameterType) });
                        }
                    }
                    else if (tabOptionItem.FindControl("Hid_" + p.Name) != null &&
                    tabOptionItem.FindControl("Hid_" + p.Name) is HiddenField)
                    {
                        HiddenField txtBox = tabOptionItem.FindControl("Hid_" + p.Name) as HiddenField;
                        //if (!txtBox.Visible) continue;

                        if (pai[0].ParameterType.IsGenericType)
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(txtBox.Value, pai[0].ParameterType.GetGenericArguments()[0]) });
                        }
                        else if (typeof(IConvertible).IsAssignableFrom(pai[0].ParameterType))
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(txtBox.Value, pai[0].ParameterType) });
                        }
                    }
                    else if (tabOptionItem.FindControl("Txt_" + p.Name) != null &&
                        tabOptionItem.FindControl("Txt_" + p.Name) is FredCK.FCKeditorV2.FCKeditor)
                    {

                        FredCK.FCKeditorV2.FCKeditor editor = tabOptionItem.FindControl("Txt_" + p.Name)
                                as FredCK.FCKeditorV2.FCKeditor;
                        if (!editor.Visible) continue;

                        if (pai[0].ParameterType.IsGenericType)
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(editor.Value, pai[0].ParameterType.GetGenericArguments()[0]) });
                        }
                        else if (typeof(IConvertible).IsAssignableFrom(pai[0].ParameterType))
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(editor.Value, pai[0].ParameterType) });
                        }
                    }
                    else if (tabOptionItem.FindControl("Ddl_" + p.Name) != null &&
                        tabOptionItem.FindControl("Ddl_" + p.Name) is DropDownList)
                    {
                        DropDownList txtBox = tabOptionItem.FindControl("Ddl_" + p.Name) as DropDownList;
                        if (!txtBox.Visible) continue;

                        if (pai[0].ParameterType.IsGenericType)
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(txtBox.SelectedValue, pai[0].ParameterType.GetGenericArguments()[0]) });
                        }
                        else if (typeof(IConvertible).IsAssignableFrom(pai[0].ParameterType))
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(txtBox.SelectedValue, pai[0].ParameterType) });
                        }
                    }
                    else if (tabOptionItem.FindControl("Ckl_" + p.Name) != null &&
                        tabOptionItem.FindControl("Ckl_" + p.Name) is CheckBoxList)
                    {
                        CheckBoxList txtBox = tabOptionItem.FindControl("Ckl_" + p.Name) as CheckBoxList;
                        if (!txtBox.Visible) continue;

                        if (pai[0].ParameterType.IsGenericType)
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(txtBox.SelectedValue, pai[0].ParameterType.GetGenericArguments()[0]) });
                        }
                        else if (typeof(IConvertible).IsAssignableFrom(pai[0].ParameterType))
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(txtBox.SelectedValue, pai[0].ParameterType) });
                        }
                    }
                    else if (tabOptionItem.FindControl("Chk_" + p.Name) != null &&
                        tabOptionItem.FindControl("Chk_" + p.Name) is CheckBox)
                    {
                        CheckBox txtBox = tabOptionItem.FindControl("Chk_" + p.Name) as CheckBox;
                        if (!txtBox.Visible) continue;

                        if (pai[0].ParameterType.IsGenericType)
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(((txtBox.Checked) ? "1" : "0"), pai[0].ParameterType.GetGenericArguments()[0]) });
                        }
                        else if (typeof(IConvertible).IsAssignableFrom(pai[0].ParameterType))
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(((txtBox.Checked) ? "1" : "0"), pai[0].ParameterType) });
                        }
                    }
                    else if (tabOptionItem.FindControl("Rdl_" + p.Name) != null &&
                        tabOptionItem.FindControl("Rdl_" + p.Name) is RadioButtonList)
                    {
                        RadioButtonList txtBox = tabOptionItem.FindControl("Rdl_" + p.Name) as RadioButtonList;
                        if (!txtBox.Visible) continue;

                        if (pai[0].ParameterType.IsGenericType)
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(txtBox.SelectedValue, pai[0].ParameterType.GetGenericArguments()[0]) });
                        }
                        else if (typeof(IConvertible).IsAssignableFrom(pai[0].ParameterType))
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(txtBox.SelectedValue, pai[0].ParameterType) });
                        }
                    }
                }
                catch (Exception)
                {
                }

            }

            return obj;
        }




        /// <summary>
        /// 实现页面表单数据自动获取,并装配到对应的MODEL实例属性中
        /// </summary>
        /// <param name="t">实例类型</param>
        /// <param name="tabOptionItem">Page</param>
        /// <returns></returns>
        public static Object GetFormDataToModel(Type t, Page tabOptionItem)
        {
            Object obj = Activator.CreateInstance(t);
            if (tabOptionItem == null) return obj;

            PropertyInfo[] pi = t.GetProperties();
            foreach (PropertyInfo p in pi)
            {
                MethodInfo m = p.GetSetMethod();
                if (m == null) continue;
                ParameterInfo[] pai = m.GetParameters();
                if (pai == null) continue;
                try
                {
                    if (tabOptionItem.FindControl("Txt_" + p.Name) != null &&
                        tabOptionItem.FindControl("Txt_" + p.Name) is TextBox)
                    {
                        TextBox txtBox = tabOptionItem.FindControl("Txt_" + p.Name) as TextBox;
                        if (!txtBox.Visible) continue;

                        if (pai[0].ParameterType.IsGenericType)
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(txtBox.Text, pai[0].ParameterType.GetGenericArguments()[0]) });
                        }
                        else if (typeof(IConvertible).IsAssignableFrom(pai[0].ParameterType))
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(txtBox.Text, pai[0].ParameterType) });
                        }
                    }
                    else if (tabOptionItem.FindControl("Hid_" + p.Name) != null &&
                        tabOptionItem.FindControl("Hid_" + p.Name) is HiddenField)
                    {
                        HiddenField txtBox = tabOptionItem.FindControl("Hid_" + p.Name) as HiddenField;
                        //if (!txtBox.Visible) continue;

                        if (pai[0].ParameterType.IsGenericType)
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(txtBox.Value, pai[0].ParameterType.GetGenericArguments()[0]) });
                        }
                        else if (typeof(IConvertible).IsAssignableFrom(pai[0].ParameterType))
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(txtBox.Value, pai[0].ParameterType) });
                        }
                    }
                    else if (tabOptionItem.FindControl("Txt_" + p.Name) != null &&
                        tabOptionItem.FindControl("Txt_" + p.Name) is FredCK.FCKeditorV2.FCKeditor)
                    {

                        FredCK.FCKeditorV2.FCKeditor editor = tabOptionItem.FindControl("Txt_" + p.Name)
                                as FredCK.FCKeditorV2.FCKeditor;
                        if (!editor.Visible) continue;

                        if (pai[0].ParameterType.IsGenericType)
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(editor.Value, pai[0].ParameterType.GetGenericArguments()[0]) });
                        }
                        else if (typeof(IConvertible).IsAssignableFrom(pai[0].ParameterType))
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(editor.Value, pai[0].ParameterType) });
                        }
                    }
                    else if (tabOptionItem.FindControl("Ddl_" + p.Name) != null &&
                        tabOptionItem.FindControl("Ddl_" + p.Name) is DropDownList)
                    {
                        DropDownList txtBox = tabOptionItem.FindControl("Ddl_" + p.Name) as DropDownList;
                        if (!txtBox.Visible) continue;

                        if (pai[0].ParameterType.IsGenericType)
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(txtBox.SelectedValue, pai[0].ParameterType.GetGenericArguments()[0]) });
                        }
                        else if (typeof(IConvertible).IsAssignableFrom(pai[0].ParameterType))
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(txtBox.SelectedValue, pai[0].ParameterType) });
                        }
                    }
                    else if (tabOptionItem.FindControl("Ckl_" + p.Name) != null &&
                        tabOptionItem.FindControl("Ckl_" + p.Name) is CheckBoxList)
                    {
                        CheckBoxList txtBox = tabOptionItem.FindControl("Ckl_" + p.Name) as CheckBoxList;
                        if (!txtBox.Visible) continue;

                        if (pai[0].ParameterType.IsGenericType)
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(txtBox.SelectedValue, pai[0].ParameterType.GetGenericArguments()[0]) });
                        }
                        else if (typeof(IConvertible).IsAssignableFrom(pai[0].ParameterType))
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(txtBox.SelectedValue, pai[0].ParameterType) });
                        }
                    }
                    else if (tabOptionItem.FindControl("Chk_" + p.Name) != null &&
                        tabOptionItem.FindControl("Chk_" + p.Name) is CheckBox)
                    {
                        CheckBox txtBox = tabOptionItem.FindControl("Chk_" + p.Name) as CheckBox;
                        if (!txtBox.Visible) continue;

                        if (pai[0].ParameterType.IsGenericType)
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(((txtBox.Checked) ? "1" : "0"), pai[0].ParameterType.GetGenericArguments()[0]) });
                        }
                        else if (typeof(IConvertible).IsAssignableFrom(pai[0].ParameterType))
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(((txtBox.Checked) ? "1" : "0"), pai[0].ParameterType) });
                        }
                    }
                    else if (tabOptionItem.FindControl("Rdl_" + p.Name) != null &&
                        tabOptionItem.FindControl("Rdl_" + p.Name) is RadioButtonList)
                    {
                        RadioButtonList txtBox = tabOptionItem.FindControl("Rdl_" + p.Name) as RadioButtonList;
                        if (!txtBox.Visible) continue;

                        if (pai[0].ParameterType.IsGenericType)
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(txtBox.SelectedValue, pai[0].ParameterType.GetGenericArguments()[0]) });
                        }
                        else if (typeof(IConvertible).IsAssignableFrom(pai[0].ParameterType))
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(txtBox.SelectedValue, pai[0].ParameterType) });
                        }
                    }
                }
                catch (Exception)
                {
                }

            }

            return obj;
        }


        /// <summary>
        /// 实现页面表单数据自动获取,并装配到对应的MODEL实例属性中
        /// </summary>
        /// <param name="t">实例类型</param>
        /// <param name="tabOptionItem">Page</param>
        /// <param name="prefix">前缀</param>
        /// <returns></returns>
        public static Object GetFormDataToModel(Type t, Page tabOptionItem, string prefix)
        {
            Object obj = Activator.CreateInstance(t);
            if (tabOptionItem == null) return obj;

            PropertyInfo[] pi = t.GetProperties();
            foreach (PropertyInfo p in pi)
            {
                MethodInfo m = p.GetSetMethod();
                if (m == null) continue;
                ParameterInfo[] pai = m.GetParameters();
                if (pai == null) continue;
                try
                {
                    if (tabOptionItem.FindControl("Txt_" + prefix + "_" + p.Name) != null &&
                        tabOptionItem.FindControl("Txt_" + prefix + "_" + p.Name) is TextBox)
                    {
                        TextBox txtBox = tabOptionItem.FindControl("Txt_" + prefix + "_" + p.Name) as TextBox;
                        if (!txtBox.Visible) continue;

                        if (pai[0].ParameterType.IsGenericType)
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(txtBox.Text, pai[0].ParameterType.GetGenericArguments()[0]) });
                        }
                        else if (typeof(IConvertible).IsAssignableFrom(pai[0].ParameterType))
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(txtBox.Text, pai[0].ParameterType) });
                        }
                    }
                    else if (tabOptionItem.FindControl("Hid_" + prefix + "_" + p.Name) != null &&
                        tabOptionItem.FindControl("Hid_" + prefix + "_" + p.Name) is HiddenField)
                    {
                        HiddenField txtBox = tabOptionItem.FindControl("Hid_" + prefix + "_" + p.Name) as HiddenField;
                        //if (!txtBox.Visible) continue;

                        if (pai[0].ParameterType.IsGenericType)
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(txtBox.Value, pai[0].ParameterType.GetGenericArguments()[0]) });
                        }
                        else if (typeof(IConvertible).IsAssignableFrom(pai[0].ParameterType))
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(txtBox.Value, pai[0].ParameterType) });
                        }
                    }
                    else if (tabOptionItem.FindControl("Txt_" + prefix + "_" + p.Name) != null &&
                        tabOptionItem.FindControl("Txt_" + prefix + "_" + p.Name) is FredCK.FCKeditorV2.FCKeditor)
                    {

                        FredCK.FCKeditorV2.FCKeditor editor = tabOptionItem.FindControl("Txt_" + prefix + "_" + p.Name)
                                as FredCK.FCKeditorV2.FCKeditor;
                        if (!editor.Visible) continue;

                        if (pai[0].ParameterType.IsGenericType)
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(editor.Value, pai[0].ParameterType.GetGenericArguments()[0]) });
                        }
                        else if (typeof(IConvertible).IsAssignableFrom(pai[0].ParameterType))
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(editor.Value, pai[0].ParameterType) });
                        }
                    }
                    else if (tabOptionItem.FindControl("Ddl_" + prefix + "_" + p.Name) != null &&
                        tabOptionItem.FindControl("Ddl_" + prefix + "_" + p.Name) is DropDownList)
                    {
                        DropDownList txtBox = tabOptionItem.FindControl("Ddl_" + prefix + "_" + p.Name) as DropDownList;
                        if (!txtBox.Visible) continue;

                        if (pai[0].ParameterType.IsGenericType)
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(txtBox.SelectedValue, pai[0].ParameterType.GetGenericArguments()[0]) });
                        }
                        else if (typeof(IConvertible).IsAssignableFrom(pai[0].ParameterType))
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(txtBox.SelectedValue, pai[0].ParameterType) });
                        }
                    }
                    else if (tabOptionItem.FindControl("Ckl_" + p.Name) != null &&
                        tabOptionItem.FindControl("Ckl_" + p.Name) is CheckBoxList)
                    {
                        CheckBoxList txtBox = tabOptionItem.FindControl("Ckl_" + p.Name) as CheckBoxList;
                        if (!txtBox.Visible) continue;

                        if (pai[0].ParameterType.IsGenericType)
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(txtBox.SelectedValue, pai[0].ParameterType.GetGenericArguments()[0]) });
                        }
                        else if (typeof(IConvertible).IsAssignableFrom(pai[0].ParameterType))
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(txtBox.SelectedValue, pai[0].ParameterType) });
                        }
                    }
                    else if (tabOptionItem.FindControl("Chk_" + p.Name) != null &&
                        tabOptionItem.FindControl("Chk_" + p.Name) is CheckBox)
                    {
                        CheckBox txtBox = tabOptionItem.FindControl("Chk_" + p.Name) as CheckBox;
                        if (!txtBox.Visible) continue;

                        if (pai[0].ParameterType.IsGenericType)
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(((txtBox.Checked) ? "1" : "0"), pai[0].ParameterType.GetGenericArguments()[0]) });
                        }
                        else if (typeof(IConvertible).IsAssignableFrom(pai[0].ParameterType))
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(((txtBox.Checked) ? "1" : "0"), pai[0].ParameterType) });
                        }
                    }
                    else if (tabOptionItem.FindControl("Rdl_" + prefix + "_" + p.Name) != null &&
                        tabOptionItem.FindControl("Rdl_" + prefix + "_" + p.Name) is RadioButtonList)
                    {
                        RadioButtonList txtBox = tabOptionItem.FindControl("Rdl_" + prefix + "_" + p.Name) as RadioButtonList;
                        if (!txtBox.Visible) continue;

                        if (pai[0].ParameterType.IsGenericType)
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(txtBox.SelectedValue, pai[0].ParameterType.GetGenericArguments()[0]) });
                        }
                        else if (typeof(IConvertible).IsAssignableFrom(pai[0].ParameterType))
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(txtBox.SelectedValue, pai[0].ParameterType) });
                        }
                    }
                }
                catch (Exception)
                {
                }

            }

            return obj;
        }



        /// <summary>
        /// 将MODEL属性值直接赋给对应的页面控件
        /// </summary>
        /// <param name="obj">类实例</param>
        /// <param name="tabOptionItem">控件容器</param>
        /// <param name="lbpr">前缀</param>
        /// <param name="isVisible">是否可见</param>
        public static void SetModelDataToForm(Object obj, Control tabOptionItem, string lbpr, bool isVisible)
        {
            //Object obj = Activator.CreateInstance(t);
            if (tabOptionItem != null && obj != null)
            {
                PropertyInfo[] pi = obj.GetType().GetProperties();
                foreach (PropertyInfo p in pi)
                {
                    MethodInfo m = p.GetGetMethod();
                    if (m == null) continue;
                    //ParameterInfo[] pai = m.GetParameters();
                    //if (pai == null) continue;
                    if (tabOptionItem.FindControl(lbpr + p.Name) == null) continue;
                    try
                    {
                        if (tabOptionItem.FindControl(lbpr + p.Name) is Label)
                        {
                            Label txtBox = tabOptionItem.FindControl(lbpr + p.Name) as Label;
                            txtBox.Text = _ConvertToString(p.GetValue(obj, null));
                            txtBox.Visible = isVisible;
                        }
                        else if (tabOptionItem.FindControl(lbpr + p.Name) is HiddenField)
                        {
                            HiddenField txtBox = tabOptionItem.FindControl(lbpr + p.Name) as HiddenField;
                            txtBox.Value = _ConvertToString(p.GetValue(obj, null));
                            txtBox.Visible = isVisible;
                        }
                        else if (tabOptionItem.FindControl(lbpr + p.Name) is TextBox)
                        {
                            TextBox txtBox = tabOptionItem.FindControl(lbpr + p.Name) as TextBox;
                            txtBox.Text = _ConvertToString(p.GetValue(obj, null));
                            txtBox.Visible = isVisible;
                        }
                        else if (tabOptionItem.FindControl(lbpr + p.Name) is DropDownList)
                        {
                            DropDownList txtBox = tabOptionItem.FindControl(lbpr + p.Name) as DropDownList;
                            txtBox.SelectedValue = _ConvertToString(p.GetValue(obj, null));
                            txtBox.Visible = isVisible;
                        }
                        else if (tabOptionItem.FindControl(lbpr + p.Name) is CheckBoxList)
                        {
                            CheckBoxList txtBox = tabOptionItem.FindControl(lbpr + p.Name) as CheckBoxList;
                            txtBox.SelectedValue = _ConvertToString(p.GetValue(obj, null));
                            txtBox.Visible = isVisible;
                        }
                        else if (tabOptionItem.FindControl(lbpr + p.Name) is CheckBox)
                        {
                            CheckBox txtBox = tabOptionItem.FindControl(lbpr + p.Name) as CheckBox;
                            txtBox.Checked = (_ConvertToString(p.GetValue(obj, null)) == "1") ? true : false;
                            txtBox.Visible = isVisible;
                        }
                        else if (tabOptionItem.FindControl(lbpr + p.Name) is RadioButtonList)
                        {
                            RadioButtonList txtBox = tabOptionItem.FindControl(lbpr + p.Name) as RadioButtonList;
                            txtBox.SelectedValue = _ConvertToString(p.GetValue(obj, null));
                            txtBox.Visible = isVisible;
                        }
                        else if (tabOptionItem.FindControl(lbpr + p.Name) is FredCK.FCKeditorV2.FCKeditor)
                        {
                            FredCK.FCKeditorV2.FCKeditor editor = tabOptionItem.FindControl(lbpr + p.Name)
                                as FredCK.FCKeditorV2.FCKeditor;
                            editor.Value = _ConvertToString(p.GetValue(obj, null));
                            editor.Visible = isVisible;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }


        /// <summary>
        /// 将MODEL属性值直接赋给对应的页面控件
        /// </summary>
        /// <param name="obj">类实例</param>
        /// <param name="tabOptionItem">控件容器</param>
        /// <param name="lbpr">前缀</param>
        public static void GetFromModelToLabel(Object obj, Control tabOptionItem, string lbpr)
        {
            //Object obj = Activator.CreateInstance(t);
            if (tabOptionItem != null && obj != null)
            {
                PropertyInfo[] pi = obj.GetType().GetProperties();
                foreach (PropertyInfo p in pi)
                {
                    MethodInfo m = p.GetGetMethod();
                    if (m == null) continue;
                    //ParameterInfo[] pai = m.GetParameters();
                    //if (pai == null) continue;
                    if (tabOptionItem.FindControl(lbpr + p.Name) == null) continue;
                    try
                    {

                        if (tabOptionItem.FindControl(lbpr + p.Name) is Label)
                        {
                            Label txtBox = tabOptionItem.FindControl(lbpr + p.Name) as Label;
                            //m.Invoke(obj, new object[] { Convert.ChangeType(txtBox.Text, pai[0].ParameterType) });
                            //txtBox.Text = (string)obj.GetType().GetProperty(p.Name).GetValue(obj, null);

                            //mod by qw 2011.6.3
                            txtBox.Text = (p.GetValue(obj, null) != null) ? Convert.ToString(p.GetValue(obj, null)) : "";
                        }
                        else if (tabOptionItem.FindControl(lbpr + p.Name) is TextBox)
                        {
                            TextBox txtBox = tabOptionItem.FindControl(lbpr + p.Name) as TextBox;
                            txtBox.Text = (p.GetValue(obj, null) != null) ? Convert.ToString(p.GetValue(obj, null)) : "";
                        }
                        else if (tabOptionItem.FindControl(lbpr + p.Name) is FredCK.FCKeditorV2.FCKeditor)
                        {
                            FredCK.FCKeditorV2.FCKeditor editor = tabOptionItem.FindControl(lbpr + p.Name)
                                as FredCK.FCKeditorV2.FCKeditor;
                            editor.Value = (p.GetValue(obj, null) != null) ? Convert.ToString(p.GetValue(obj, null)) : "";
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }


        /// <summary>
        /// 将MODEL属性值直接赋给对应的页面控件
        /// </summary>
        /// <param name="obj">类实例</param>
        /// <param name="tabOptionItem">Page</param>
        /// <param name="lbpr">前缀</param>
        /// <param name="isVisible">是否可见</param>
        public static void SetModelDataToForm(Object obj, Page tabOptionItem, string lbpr, bool isVisible)
        {
            //Object obj = Activator.CreateInstance(t);
            if (tabOptionItem != null && obj != null)
            {
                PropertyInfo[] pi = obj.GetType().GetProperties();
                foreach (PropertyInfo p in pi)
                {
                    MethodInfo m = p.GetGetMethod();
                    if (m == null) continue;
                    //ParameterInfo[] pai = m.GetParameters();
                    //if (pai == null) continue;
                    if (tabOptionItem.FindControl(lbpr + p.Name) == null) continue;
                    try
                    {
                        if (tabOptionItem.FindControl(lbpr + p.Name) is Label)
                        {
                            Label txtBox = tabOptionItem.FindControl(lbpr + p.Name) as Label;
                            txtBox.Text = _ConvertToString(p.GetValue(obj, null));
                            txtBox.Visible = isVisible;
                        }
                        else if (tabOptionItem.FindControl(lbpr + p.Name) is HiddenField)
                        {
                            HiddenField txtBox = tabOptionItem.FindControl(lbpr + p.Name) as HiddenField;
                            txtBox.Value = _ConvertToString(p.GetValue(obj, null));
                            txtBox.Visible = isVisible;
                        }
                        else if (tabOptionItem.FindControl(lbpr + p.Name) is TextBox)
                        {
                            TextBox txtBox = tabOptionItem.FindControl(lbpr + p.Name) as TextBox;
                            txtBox.Text = _ConvertToString(p.GetValue(obj, null));
                            txtBox.Visible = isVisible;
                        }
                        else if (tabOptionItem.FindControl(lbpr + p.Name) is DropDownList)
                        {
                            DropDownList txtBox = tabOptionItem.FindControl(lbpr + p.Name) as DropDownList;
                            txtBox.SelectedValue = _ConvertToString(p.GetValue(obj, null));
                            txtBox.Visible = isVisible;
                        }
                        else if (tabOptionItem.FindControl(lbpr + p.Name) is CheckBoxList)
                        {
                            CheckBoxList txtBox = tabOptionItem.FindControl(lbpr + p.Name) as CheckBoxList;
                            txtBox.SelectedValue = _ConvertToString(p.GetValue(obj, null));
                            txtBox.Visible = isVisible;
                        }
                        else if (tabOptionItem.FindControl(lbpr + p.Name) is CheckBox)
                        {
                            CheckBox txtBox = tabOptionItem.FindControl(lbpr + p.Name) as CheckBox;
                            txtBox.Checked = (_ConvertToString(p.GetValue(obj, null)) == "1") ? true : false;
                            txtBox.Visible = isVisible;
                        }
                        else if (tabOptionItem.FindControl(lbpr + p.Name) is RadioButtonList)
                        {
                            RadioButtonList txtBox = tabOptionItem.FindControl(lbpr + p.Name) as RadioButtonList;
                            txtBox.SelectedValue = _ConvertToString(p.GetValue(obj, null));
                            txtBox.Visible = isVisible;
                        }
                        else if (tabOptionItem.FindControl(lbpr + p.Name) is FredCK.FCKeditorV2.FCKeditor)
                        {
                            FredCK.FCKeditorV2.FCKeditor editor = tabOptionItem.FindControl(lbpr + p.Name)
                                as FredCK.FCKeditorV2.FCKeditor;
                            editor.Value = _ConvertToString(p.GetValue(obj, null));
                            editor.Visible = isVisible;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }


        /// <summary>
        /// 将MODEL属性值直接赋给对应的页面控件
        /// </summary>
        /// <param name="obj">类实例</param>
        /// <param name="tabOptionItem">Page</param>
        /// <param name="lbpr">前缀</param>
        public static void GetFromModelToLabel(Object obj, Page tabOptionItem, string lbpr)
        {
            //Object obj = Activator.CreateInstance(t);
            if (tabOptionItem != null && obj != null)
            {
                PropertyInfo[] pi = obj.GetType().GetProperties();
                foreach (PropertyInfo p in pi)
                {
                    MethodInfo m = p.GetGetMethod();
                    if (m == null) continue;
                    //ParameterInfo[] pai = m.GetParameters();
                    //if (pai == null) continue;
                    if (tabOptionItem.FindControl(lbpr + p.Name) == null) continue;
                    try
                    {

                        if (tabOptionItem.FindControl(lbpr + p.Name) is Label)
                        {
                            Label txtBox = tabOptionItem.FindControl(lbpr + p.Name) as Label;
                            //m.Invoke(obj, new object[] { Convert.ChangeType(txtBox.Text, pai[0].ParameterType) });
                            //txtBox.Text = (string)obj.GetType().GetProperty(p.Name).GetValue(obj, null);

                            //mod by qw 2011.6.3
                            txtBox.Text = (p.GetValue(obj, null) != null) ? Convert.ToString(p.GetValue(obj, null)) : "";
                        }
                        else if (tabOptionItem.FindControl(lbpr + p.Name) is TextBox)
                        {
                            TextBox txtBox = tabOptionItem.FindControl(lbpr + p.Name) as TextBox;
                            txtBox.Text = (p.GetValue(obj, null) != null) ? Convert.ToString(p.GetValue(obj, null)) : "";
                        }
                        else if (tabOptionItem.FindControl(lbpr + p.Name) is FredCK.FCKeditorV2.FCKeditor)
                        {
                            FredCK.FCKeditorV2.FCKeditor editor = tabOptionItem.FindControl(lbpr + p.Name)
                                as FredCK.FCKeditorV2.FCKeditor;
                            editor.Value = (p.GetValue(obj, null) != null) ? Convert.ToString(p.GetValue(obj, null)) : "";
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }


        /// <summary>
        /// 克隆一个新的对象
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Object Clone(Object model)
        {
            Type t = model.GetType();
            PropertyInfo[] pi = t.GetProperties();
            Object obj = Activator.CreateInstance(t);
            foreach (PropertyInfo p in pi)
            {
                MethodInfo s = p.GetSetMethod();
                if (s == null) continue;
                ParameterInfo[] pai = s.GetParameters();
                if (pai == null) continue;
                MethodInfo g = p.GetGetMethod();
                if (g == null) continue;
                try
                {
                    if (pai[0].ParameterType.IsGenericType)
                    {
                        s.Invoke(obj, new object[] { Convert.ChangeType(p.GetValue(model, null), pai[0].ParameterType.GetGenericArguments()[0]) });
                    }
                    else if (typeof(IConvertible).IsAssignableFrom(pai[0].ParameterType))
                    {
                        s.Invoke(obj, new object[] { Convert.ChangeType(p.GetValue(model, null), pai[0].ParameterType) });
                    }
                }
                catch (Exception)
                {
                }
            }
            return obj;
        }


        /// <summary>
        /// MSSQL和ORACLE通用
        /// 完成从DataReader对象到MODEL实例的装配。
        /// </summary>
        /// <param name="t"></param>
        /// <param name="rdr"></param>
        /// <returns></returns>
        public static Object InitialiModelBean(Type t, IDataReader rdr)
        {
            PropertyInfo[] pi = t.GetProperties();
            Object obj = Activator.CreateInstance(t);
            foreach (PropertyInfo p in pi)
            {
                MethodInfo m = p.GetSetMethod();
                if (m == null) continue;
                ParameterInfo[] pai = m.GetParameters();
                if (pai == null) continue;
                try
                {
                    if (pai[0].ParameterType.IsGenericType)
                    {
                        m.Invoke(obj, new object[] { Convert.ChangeType(rdr[p.Name], pai[0].ParameterType.GetGenericArguments()[0]) });
                    }
                    else if (typeof(IConvertible).IsAssignableFrom(pai[0].ParameterType))
                    {
                        m.Invoke(obj, new object[] { Convert.ChangeType(rdr[p.Name], pai[0].ParameterType) });
                    }
                }
                catch (Exception)
                {
                }
            }
            return obj;
        }



        /// <summary>
        /// MSSQL和ORACLE通用
        /// 完成从DataRow中数据的装配
        /// </summary>
        /// <param name="t"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static Object InitialiModelBean(Type t, DataRow dr)
        {
            PropertyInfo[] pi = t.GetProperties();
            Object obj = Activator.CreateInstance(t);
            foreach (PropertyInfo p in pi)
            {
                MethodInfo m = p.GetSetMethod();
                if (m == null) continue;
                ParameterInfo[] pai = m.GetParameters();
                if (pai == null) continue;
                try
                {
                    if (pai[0].ParameterType.IsGenericType)
                    {
                        m.Invoke(obj, new object[] { Convert.ChangeType(dr[p.Name], pai[0].ParameterType.GetGenericArguments()[0]) });
                    }
                    else if (typeof(IConvertible).IsAssignableFrom(pai[0].ParameterType))
                    {
                        m.Invoke(obj, new object[] { Convert.ChangeType(dr[p.Name], pai[0].ParameterType) });
                    }
                }
                catch (Exception)
                {
                }
            }
            return obj;
        }


        /// <summary>
        /// 完成从Hashtable对象到MODEL实例的装配。
        /// </summary>
        /// <param name="t">Type</param>
        /// <param name="ht">Hashtable</param>
        /// <returns>Object</returns>
        public static Object InitialiModelBean(Type t, Hashtable ht)
        {
            PropertyInfo[] pi = t.GetProperties();
            Object obj = Activator.CreateInstance(t);

            foreach (PropertyInfo p in pi)
            {
                MethodInfo m = p.GetSetMethod();
                if (m == null)
                    continue;
                ParameterInfo[] pai = m.GetParameters();
                if (pai == null)
                    continue;

                if (ht.ContainsKey(p.Name.ToUpper()))
                {
                    try
                    {
                        if (pai[0].ParameterType.IsGenericType)
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(ht[p.Name.ToUpper()], pai[0].ParameterType.GetGenericArguments()[0]) });
                        }
                        else if (typeof(IConvertible).IsAssignableFrom(pai[0].ParameterType))
                        {
                            m.Invoke(obj, new object[] { Convert.ChangeType(ht[p.Name.ToUpper()], pai[0].ParameterType) });
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

            }

            return obj;
        }


        ///// <summary>
        ///// 完成从DataReader对象到MODEL实例的装配。
        ///// </summary>
        ///// <param name="t"></param>
        ///// <param name="rdr"></param>
        ///// <returns></returns>
        //public static Object InitialiModelBean(Type t, OracleDataReader rdr)
        //{
        //    PropertyInfo[] pi = t.GetProperties();
        //    Object obj = Activator.CreateInstance(t);
        //    foreach (PropertyInfo p in pi)
        //    {
        //        MethodInfo m = p.GetSetMethod();
        //        if (m == null) continue;
        //        ParameterInfo[] pai = m.GetParameters();
        //        if (pai == null) continue;
        //        try
        //        {
        //            if (pai[0].ParameterType.IsGenericType)
        //            {
        //                m.Invoke(obj, new object[] { Convert.ChangeType(rdr[p.Name], pai[0].ParameterType.GetGenericArguments()[0]) });
        //            }
        //            else if (typeof(IConvertible).IsAssignableFrom(pai[0].ParameterType))
        //            {
        //                m.Invoke(obj, new object[] { Convert.ChangeType(rdr[p.Name], pai[0].ParameterType) });
        //            }
        //        }
        //        catch (Exception)
        //        {
        //        }
        //    }
        //    return obj;
        //}



        ///// <summary>
        ///// 完成从DataReader对象到MODEL实例的装配。
        ///// </summary>
        ///// <param name="t"></param>
        ///// <param name="rdr"></param>
        ///// <returns></returns>
        //public static Object InitialiModelBean(Type t, SqlDataReader rdr)
        //{
        //    PropertyInfo[] pi = t.GetProperties();
        //    Object obj = Activator.CreateInstance(t);
        //    foreach (PropertyInfo p in pi)
        //    {
        //        MethodInfo m = p.GetSetMethod();
        //        if (m == null) continue;
        //        ParameterInfo[] pai = m.GetParameters();
        //        if (pai == null) continue;
        //        try
        //        {
        //            if (pai[0].ParameterType.IsGenericType)
        //            {
        //                m.Invoke(obj, new object[] { Convert.ChangeType(rdr[p.Name], pai[0].ParameterType.GetGenericArguments()[0]) });
        //            }
        //            else if (typeof(IConvertible).IsAssignableFrom(pai[0].ParameterType))
        //            {
        //                m.Invoke(obj, new object[] { Convert.ChangeType(rdr[p.Name], pai[0].ParameterType) });
        //            }
        //        }
        //        catch (Exception)
        //        {
        //        }
        //    }
        //    return obj;
        //}



        #endregion

        /// <summary>
        /// 用于四位数的补零操作
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string BuZero_4(int i)
        {
            string bu = "0000";

            if (i >= 1000 && i <= 9999)
            {
                bu = i.ToString();
            }
            else if (i >= 100 && i <= 999)
            {
                bu = "0" + i;
            }
            else if (i >= 10 && i <= 99)
            {
                bu = "00" + i;
            }
            else if (i > 0 && i < 10)
            {
                bu = "000" + i;
            }

            return bu;
        }

        /// <summary>
        /// 用于三位数的补零操作
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string BuZero_3(int i)
        {
            string bu = "000";

            if (i >= 100 && i <= 999)
            {
                bu = i.ToString();
            }
            else if (i >= 10 && i <= 99)
            {
                bu = "0" + i;
            }
            else if (i > 0 && i < 10)
            {
                bu = "00" + i;
            }

            return bu;
        }


        /// <summary>
        /// 用于两位数的补零操作
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string BuZero_2(int i)
        {
            string bu = "00";

            if (i >= 10 && i <= 99)
            {
                bu = i.ToString();
            }
            else if (i > 0 && i < 10)
            {
                bu = "0" + i;
            }

            return bu;
        }


        /// <summary>
        /// 获取系统当前的GUID
        /// </summary>
        /// <returns></returns>
        public static string GetGuidKey()
        {
            return System.Guid.NewGuid().ToString("D");
        }



        /// <summary>
        /// 获取系统当前的GUID
        /// </summary>
        /// <returns></returns>
        public static Guid NewGuid()
        {
            return System.Guid.NewGuid();
        }

        /// <summary>
        /// 获取月份的英文简称
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public static string GetMonthNameAbbr(int month)
        {

            switch (month)
            {
                case 1:
                    return "Jan";
                case 2:
                    return "Feb";
                case 3:
                    return "Mar";
                case 4:
                    return "Apr";
                case 5:
                    return "May";
                case 6:
                    return "Jun";
                case 7:
                    return "Jul";
                case 8:
                    return "Aug";
                case 9:
                    return "Sept";
                case 10:
                    return "Oct";
                case 11:
                    return "Nov";
                case 12:
                    return "Dec";
            }

            return "";
        }


        /// <summary>
        /// 按时间进度换算成百分制
        /// </summary>
        /// <param name="sTime">计划开始日期</param>
        /// <param name="eTime">计划完成日期</param>
        /// <param name="nowTime">当前日期</param>
        /// <returns></returns>
        public static double GetCurrentProgressProportion(DateTime sTime, DateTime eTime, DateTime nowTime)
        {
            double p = 0;

            if (nowTime >= eTime)
            {
                p = 1;
            }
            else if (nowTime <= sTime)
            {
                p = 0;
            }
            else if (sTime < eTime)
            {
                //nowTime位于sTime 和 eTime 时间段内
                double val_Start_End = DateDiff(sTime, eTime, DateDiffMode.Days);
                double val_Start_Now = DateDiff(sTime, nowTime, DateDiffMode.Days);
                if (val_Start_Now < val_Start_End && val_Start_End > 0)
                {
                    p = val_Start_Now / val_Start_End;
                }
            }

            return p;
        }



        /// <summary>
        /// 计算两个日期的时间间隔
        /// </summary>
        /// <param name="DateTime1">第一个日期和时间</param>
        /// <param name="DateTime2">第二个日期和时间</param>
        /// <param name="mode">日期比较模式</param>
        /// <remarks>Add: qw 2011-05-09</remarks>
        /// <returns>时间间隔</returns>
        public static int DateDiff(DateTime DateTime1, DateTime DateTime2, DateDiffMode mode)
        {
            int dateDiff = 0;
            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            switch (mode)
            {
                case DateDiffMode.Days:
                    dateDiff = ts.Days;
                    break;
                case DateDiffMode.Hours:
                    dateDiff = ts.Hours;
                    break;
                case DateDiffMode.Minutes:
                    dateDiff = ts.Minutes;
                    break;
                case DateDiffMode.Seconds:
                    dateDiff = ts.Seconds;
                    break;
            }
            return dateDiff;
        }

        /// <summary>
        /// 日期比较模式
        /// </summary>
        public enum DateDiffMode
        {
            Days, Hours, Minutes, Seconds
        }



        #region 得到一周的周一和周日的日期

        /// <summary>
        /// C#日期函数计算本周的周一日期
        /// </summary>
        /// <returns></returns>
        public static DateTime GetMondayDate()
        {
            return GetMondayDate(DateTime.Now);
        }

        /// <summary>
        /// 计算本周周日的日期
        /// </summary>
        /// <returns></returns>
        public static DateTime GetSundayDate()
        {
            return GetSundayDate(DateTime.Now);
        }

        /// <summary>
        /// 计算某日起始日期（礼拜一的日期）
        /// </summary>
        /// <param name="someDate">该周中任意一天</param>
        /// <returns>返回礼拜一日期，后面的具体时、分、秒和传入值相等</returns>
        public static DateTime GetMondayDate(DateTime someDate)
        {
            int i = someDate.DayOfWeek - DayOfWeek.Monday;
            if (i == -1) i = 6;// i值 > = 0 ，因为枚举原因，Sunday排在最前，此时Sunday-Monday=-1，必须+7=6。
            TimeSpan ts = new TimeSpan(i, 0, 0, 0);
            return someDate.Subtract(ts);
        }

        /// <summary>
        /// 计算某日结束日期（礼拜日的日期）
        /// </summary>
        /// <param name="someDate">该周中任意一天</param>
        /// <returns>返回礼拜日日期，后面的具体时、分、秒和传入值相等</returns>
        public static DateTime GetSundayDate(DateTime someDate)
        {
            int i = someDate.DayOfWeek - DayOfWeek.Sunday;
            if (i != 0) i = 7 - i;// 因为枚举原因，Sunday排在最前，相减间隔要被7减。
            TimeSpan ts = new TimeSpan(i, 0, 0, 0);
            return someDate.Add(ts);
        }

        /// <summary>
        /// 得到提定日期所在月的第一天
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetMonthFirstDay(DateTime dt)
        {
            int yy = dt.Year;
            int mm = dt.Month;
            return DateTime.Parse(string.Format("{0}-{1}-{2} 00:00:00", yy, mm, 1));
        }

        /// <summary>
        /// 得到提定日期所在月的最后一天
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetMonthLastDay(DateTime dt)
        {
            int yy = dt.Year;
            int mm = dt.Month;
            DateTime d1 = new DateTime(yy, mm, 1);
            DateTime d2 = d1.AddMonths(1).AddDays(-1);
            return DateTime.Parse(string.Format("{0}-{1}-{2} 23:59:59", yy, mm, d2.Day));
        }


        #endregion



        #region 私有方法>>>>>>>>>>===================


        /// <summary>
        /// 转换各类型为字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static string _ConvertToString(object obj)
        {
            string _V = string.Empty;

            if (obj != null)
            {
                if (obj is DateTime)
                {
                    //日期型要单独处理一下
                    DateTime tempDT = (DateTime)obj;
                    if (tempDT.Year > 1)
                    {
                        _V = tempDT.ToString("yyyy-MM-dd");
                    }
                }
                else
                {
                    _V = Convert.ToString(obj);
                }
            }

            return _V;
        }

        #endregion


        #region 临时用

        /*
         string str = System.Guid.NewGuid().ToString("N") + "|"

        + System.Guid.NewGuid().ToString("D") + "|"

        + System.Guid.NewGuid().ToString("B") + "|"

        + System.Guid.NewGuid().ToString("P");
        Response.Write(str);

 

        返回的结果：

        ece4f4a60b764339b94a07c84e338a27|

        5bf99df1-dc49-4023-a34a-7bd80a42d6bb|

        {2280f8d7-fd18-4c72-a9ab-405de3fcfbc9}|

        (25e6e09f-fb66-4cab-b4cd-bfb429566549)
         */

        #endregion

    }
}
