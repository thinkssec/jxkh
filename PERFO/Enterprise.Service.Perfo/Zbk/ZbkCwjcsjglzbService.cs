using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Data.Perfo.Zbk;
using Enterprise.Model.Perfo.Zbk;

namespace Enterprise.Service.Perfo.Zbk
{

    /// <summary>
    /// 文件名:  ZbkCwjcsjglzbService.cs
    /// 功能描述: 业务逻辑层-关联指标与财务基础数据对应表数据处理
    /// 创建人：代码生成器
    /// 创建时间 ：2014/12/14 9:19:20
    /// </summary>
    public class ZbkCwjcsjglzbService
    {
        #region 代码生成器

        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly IZbkCwjcsjglzbData dal = new ZbkCwjcsjglzbData();

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ZbkCwjcsjglzbModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<ZbkCwjcsjglzbModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<ZbkCwjcsjglzbModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<ZbkCwjcsjglzbModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(ZbkCwjcsjglzbModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region 自定义方法区

        /// <summary>
        /// 完成财务基础数据与关联指标对应关系的初始化
        /// </summary>
        /// <returns></returns>
        public bool InitCwjcsjGlzbData()
        {
            bool isOK = true;
            List<ZbkCwjcsjglzbModel> list = new List<ZbkCwjcsjglzbModel>();
            list.AddRange(GetList());//把现有数据提取出来
            ZbkLhzbService lhzbSrv = new ZbkLhzbService();
            var lhzbLst =lhzbSrv.GetList().Where(p => p.JSBDS.IndexOf('=') > 0).ToList();
            foreach (var lhzb in lhzbLst)
            {
                string bds = lhzb.JSBDS;
                bds = bds.Substring(bds.IndexOf('=') + 1);
                List<string> values = Utility.GetMatchValues(bds, @"\{.*?\}");
                foreach (var v in values)
                {
                    ZbkCwjcsjglzbModel model = new ZbkCwjcsjglzbModel();
                    model.DB_Option_Action = WebKeys.InsertAction;
                    model.ID = CommonTool.GetGuidKey(); //ID
                    model.ZBXMC = v.Trim('{', '}');
                    if (!list.Exists(p => p.ZBXMC == model.ZBXMC))
                        list.Add(model);
                }
            }
            foreach (var m in list)
            {
                isOK = Execute(m);
            }
            return isOK;
        }

        #endregion
    }

}
