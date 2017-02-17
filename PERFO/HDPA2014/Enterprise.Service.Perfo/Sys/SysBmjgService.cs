using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Component.Cache;
using Enterprise.Data.Perfo.Sys;
using Enterprise.Model.Perfo.Sys;
using System.Web.UI.WebControls;

namespace Enterprise.Service.Perfo.Sys
{

    /// <summary>
    /// 文件名:  SysBmjgService.cs
    /// 功能描述: 业务逻辑层-机构管理数据处理
    /// 创建人：代码生成器
    /// 创建时间 ：2014/11/1 0:35:45
    /// </summary>
    public class SysBmjgService
    {
        #region 代码生成器

        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly ISysBmjgData dal = new SysBmjgData();

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SysBmjgModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<SysBmjgModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<SysBmjgModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<SysBmjgModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(SysBmjgModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region 自定义方法

        /// <summary>
        /// 获取部门机构的树型排列集合
        /// </summary>
        /// <param name="isAll">是否全部显示</param>
        /// <returns></returns>
        public IList<SysBmjgModel> GetBmjgTreeLst(bool isAll)
        {
            List<SysBmjgModel> bmjgList = null;
            if (WebKeys.EnableCaching)
            {
                bmjgList = (List<SysBmjgModel>)CacheHelper.GetCache(SysBmjgData.CacheClassKey + "_GetBmjgTreeLst_" + isAll);
            }
            if (bmjgList == null || bmjgList.Count == 0)
            {
                string hql = "from SysBmjgModel p ";
                if (!isAll)
                {
                    hql += " where p.SFKH='1'";
                }
                bmjgList = GetListByHQL(hql).OrderBy(p => p.XSXH).ToList();
                foreach (var q in bmjgList)
                {
                    switch (q.XSXH.Length)
                    {
                        case 2:
                            break;
                        case 4:
                            q.JGMC = CommonTool.GenerateBlankSpace(1) + q.JGMC;
                            break;
                        default:
                            string jgmc = CommonTool.GenerateBlankSpace(q.XSXH.Length / 2 - 1);
                            q.JGMC = jgmc + q.JGMC;
                            break;
                    }
                }
            }
            if (WebKeys.EnableCaching)
            {
                //数据存入缓存系统
                CacheHelper.Add(SysBmjgData.CacheClassKey + "_GetBmjgTreeLst_" + isAll, bmjgList);
            }
            return bmjgList;
        }

        /// <summary>
        /// 获取对应的机构名称
        /// </summary>
        /// <param name="jgbm">机构编码</param>
        /// <returns></returns>
        public static string GetBmjgName(int jgbm)
        {
            var m = dal.GetList().FirstOrDefault(p => p.JGBM == jgbm);
            return ((m != null) ? m.JGMC : "");
        }

  
        /// <summary>
        /// 获取对应的机构名称
        /// </summary>
        /// <param name="xsxh">显示序号</param>
        /// <param name="len">显示序号长度</param>
        /// <returns></returns>
        public static string GetBmjgNameByXsxh(string xsxh, int len)
        {
            if (len > 0 && xsxh.Length >= len)
            {
                xsxh = xsxh.Substring(0, len);
            }
            var m = dal.GetList().FirstOrDefault(p => p.XSXH == xsxh);
            return ((m != null) ? m.JGMC : "");
        }

        /// <summary>
        /// 获取对应的机构对象
        /// </summary>
        /// <param name="xsxh">显示序号</param>
        /// <param name="len">显示序号长度</param>
        /// <returns></returns>
        public SysBmjgModel GetModelByXsxh(string xsxh, int len)
        {
            if (len > 0 && xsxh.Length >= len)
            {
                xsxh = xsxh.Substring(0, len);
            }
            var m = dal.GetList().FirstOrDefault(p => p.XSXH == xsxh);
            return m;
        }

        /// <summary>
        /// 获取指定机构的所有下级机构
        /// </summary>
        /// <param name="jgbm"></param>
        /// <returns></returns>
        public string GetJgbmsForSQL(string jgbm)
        {
            string jgbms = string.Empty;
            var bmjg = GetSingle(jgbm);
            if (bmjg != null)
            {
                var list = GetList().Where(p => p.XSXH.StartsWith(bmjg.XSXH)).OrderBy(p => p.XSXH);
                foreach (var q in list)
                {
                    jgbms += "'" + q.JGBM + "',";
                }
            }
            return jgbms.TrimEnd(',');
        }

        /// <summary>
        /// 获取同一层级的所有单位并排序
        /// </summary>
        /// <param name="xhLength"></param>
        /// <returns></returns>
        public IList<SysBmjgModel> GetSameLevelBmjg(int xhLength)
        {
            List<SysBmjgModel> bmjgList = null;
            if (WebKeys.EnableCaching)
            {
                bmjgList = (List<SysBmjgModel>)CacheHelper.GetCache(SysBmjgData.CacheClassKey + "_GetSameLevelBmjg_" + xhLength);
            }
            if (bmjgList == null || bmjgList.Count == 0)
            {
                string hql = "from SysBmjgModel p where length(p.XSXH)='" + xhLength + "' order by p.JGLX,p.BZ";
                bmjgList = GetListByHQL(hql).ToList();
            }
            if (WebKeys.EnableCaching)
            {
                //数据存入缓存系统
                CacheHelper.Add(SysBmjgData.CacheClassKey + "_GetSameLevelBmjg_" + xhLength, bmjgList);
            }
            return bmjgList;
        }

        /// <summary>
        /// 获取指定机构编码下的所有下级机构集合
        /// </summary>
        /// <param name="jgbm">机构编码</param>
        /// <returns></returns>
        public IList<SysBmjgModel> GetBmjgLisByParentJgbm(string jgbm)
        {
            List<SysBmjgModel> bmList = null;
            var model = GetSingle(jgbm);
            if (model != null)
            {
                bmList = GetBmjgTreeLst(false).
                    Where(p => p.XSXH.StartsWith(model.XSXH) && p.XSXH.Length >= model.XSXH.Length).OrderBy(p=>p.XSXH).ToList();

            }
            return bmList;
        }

        /// <summary>
        /// 绑定带分组功能的指定单位的下拉控件
        /// </summary>
        /// <param name="ddl">下拉控件</param>
        /// <param name="bmjgLst">部门机构</param>
        /// <param name="parentJgbms">机构编码</param>
        public void BindSSECDropDownListForBmjg(SSECDropDownList ddl, IList<SysBmjgModel> bmjgLst, int[] parentJgbms)
        {
            ddl.Items.Clear();
            string jglx = string.Empty;
            string xsxh = string.Empty;
            foreach (var q in bmjgLst)
            {
                if (parentJgbms.Contains(q.JGBM))
                {
                    ddl.Items.Add(new ListItem(q.JGMC, "optgroup"));
                    continue;
                }
                if (string.IsNullOrEmpty(jglx))
                {
                    ddl.Items.Add(new ListItem(q.JGLX, "optgroup"));
                    ddl.Items.Add(new ListItem(q.JGMC, q.JGBM.ToString()));
                    jglx = q.JGLX;
                    xsxh = q.XSXH.Substring(0, 4);
                }
                else if (q.JGLX == jglx && q.XSXH.StartsWith(xsxh))
                {
                    ddl.Items.Add(new ListItem(q.JGMC, q.JGBM.ToString()));
                }
                else
                {
                    ddl.Items.Add(new ListItem(q.JGLX, "optgroup"));
                    ddl.Items.Add(new ListItem(q.JGMC, q.JGBM.ToString()));
                    jglx = q.JGLX;
                    xsxh = q.XSXH.Substring(0, 4);
                }
            }
            ddl.Items.Insert(0, new ListItem("华东油气田", ""));
        }

        /// <summary>
        /// 绑定带分组功能的单位下拉控件
        /// </summary>
        /// <param name="ddl">下拉控件</param>
        /// <param name="bmjgLst">部门机构</param>
        public void BindSSECDropDownListForBmjg(SSECDropDownList ddl, IList<SysBmjgModel> bmjgLst)
        {
            ddl.Items.Clear();
            string jglx = string.Empty;
            foreach (var q in bmjgLst)
            {
                if (string.IsNullOrEmpty(jglx))
                {
                    ddl.Items.Add(new ListItem(q.JGLX, "optgroup"));
                    ddl.Items.Add(new ListItem(q.JGMC, q.JGBM.ToString()));
                    jglx = q.JGLX;
                }
                else if (q.JGLX == jglx)
                {
                    ddl.Items.Add(new ListItem(q.JGMC, q.JGBM.ToString()));
                }
                else
                {
                    ddl.Items.Add(new ListItem(q.JGLX, "optgroup"));
                    ddl.Items.Add(new ListItem(q.JGMC, q.JGBM.ToString()));
                    jglx = q.JGLX;
                }
            }
            ddl.Items.Insert(0, new ListItem("华东油气田", ""));
        }
                

        #endregion
    }

}
