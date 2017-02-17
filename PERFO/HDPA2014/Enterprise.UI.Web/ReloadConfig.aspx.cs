using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Enterprise.Component.Cache;
using Enterprise.Component.Infrastructure;
using Enterprise.Component.MVC;
using Enterprise.Service.Perfo.Sys;

namespace Enterprise.UI.Web
{
    /// <summary>
    /// 重新加载配置页面
    /// </summary>
    public partial class ReloadConfig : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //1
            //IDictionaryEnumerator cacheEnum = HttpRuntime.Cache.GetEnumerator();
            //while (cacheEnum.MoveNext())
            //{
            //    DictionaryEntry abc = cacheEnum.Entry;
            //    string Tempstring = (string)abc.Key;
            //    if (Tempstring.StartsWith("CACHE_"))
            //        HttpRuntime.Cache.Remove(Tempstring);
            //}
            CacheHelper.Refresh();
            //MyCacheManager.ReloadCache();
            msg.Text = "缓存清空成功！";
            //2
            Global.LoadUrlRoute();
            msg.Text += "<br/>路由重新加载成功!";
        }
    }
}
