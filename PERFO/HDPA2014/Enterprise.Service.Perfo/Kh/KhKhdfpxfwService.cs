using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Data.Perfo.Kh;
using Enterprise.Model.Perfo.Kh;
using Enterprise.Service.Perfo.Sys;

namespace Enterprise.Service.Perfo.Kh
{

    /// <summary>
    /// 文件名:  KhKhdfpxfwService.cs
    /// 功能描述: 业务逻辑层-考核得分排序范围设置数据处理
    /// 创建人：代码生成器
    /// 创建时间 ：2014/12/2 13:41:05
    /// </summary>
    public class KhKhdfpxfwService
    {
        #region 代码生成器

        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly IKhKhdfpxfwData dal = new KhKhdfpxfwData();

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhKhdfpxfwModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhKhdfpxfwModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhKhdfpxfwModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhKhdfpxfwModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhKhdfpxfwModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region 自定义方法

        /// <summary>
        /// 提取与考核期和考核对象相应的数据
        /// </summary>
        /// <param name="khid">考核期</param>
        /// <param name="khType">考核类型</param>
        /// <returns></returns>
        public IList<KhKhdfpxfwModel> GetListByKhidAndKhdx(string khid, string khType)
        {
            string hql = "from KhKhdfpxfwModel p where p.KHID='" + khid + "' and p.KHDX='" + khType + "'";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// 获取归属单位名称
        /// </summary>
        /// <param name="khid">考核期</param>
        /// <param name="jgbm">机构编码</param>
        /// <param name="khType">考核类型</param>
        /// <returns></returns>
        public string GetGsdwmc(string khid, int jgbm, string khType)
        {
            string gsdwmc = string.Empty;
            var list = GetListByKhidAndKhdx(khid, khType);
            SysBmjgService bmjgSrv = new SysBmjgService();
            var bmjg = bmjgSrv.GetSingle(jgbm.ToString());
            if (bmjg != null && bmjg.XSXH.Length >= 4)
            {
                var parentDw = bmjgSrv.GetModelByXsxh(bmjg.XSXH, 4);
                if (parentDw != null)
                {
                    var pxfw = list.FirstOrDefault(p => p.GSDW.Contains("," + parentDw.JGBM + ","));
                    if (pxfw != null)
                    {
                        string[] jgbms = pxfw.GSDW.TrimStart(',').TrimEnd(',').Split(',');
                        foreach (var dw in jgbms)
                        {
                            var bmjgModel = bmjgSrv.GetSingle(dw);
                            if (bmjgModel != null)
                            {
                                gsdwmc += bmjgModel.JGMC + "、";
                            }
                        }
                    }
                    else
                    {
                        gsdwmc = parentDw.JGMC;
                    }
                }
            }
            return gsdwmc.TrimEnd("、".ToCharArray());
        }

        #endregion
    }

}
