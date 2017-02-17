using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.ORM;
using Enterprise.Component.Infrastructure;
using Enterprise.Component.Cache;
using Enterprise.Model.Perfo.Zbk;

namespace Enterprise.Data.Perfo.Zbk
{

    /// <summary>
    /// 文件名:  ZbkBanbenData.cs
    /// 功能描述: 数据层-指标版本管理数据访问方法实现类
    /// 创建人：代码生成器
    /// 创建时间 ：2014/11/1 0:35:46
    /// </summary>
    public class ZbkBanbenData : IZbkBanbenData
    {

        #region 代码生成器

        /// <summary>
        /// 缓存项名称
        /// </summary>
        public static readonly string CacheClassKey = WebKeys.CacheItemKey + typeof(ZbkBanbenData).ToString();

        /// <summary>
        /// 缓存关联项数组
        /// </summary>
        public static readonly string[] CacheRelationshipKeys = new string[] { 
            Enterprise.Data.Perfo.Zbk.ZbkBanbenData.CacheClassKey,
            Enterprise.Data.Perfo.Zbk.ZbkLhzbData.CacheClassKey,
            Enterprise.Data.Perfo.Zbk.ZbkDfzbData.CacheClassKey,
            Enterprise.Data.Perfo.Zbk.ZbkJsgzData.CacheClassKey
        };

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ZbkBanbenModel GetSingle(string key)
        {
            //string hql = "from ZbkBanbenModel p where p.BBMC='" + key + "'";
            return GetList().FirstOrDefault(p => p.BBMC == key);
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<ZbkBanbenModel> GetList()
        {
            IList<ZbkBanbenModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<ZbkBanbenModel>)CacheHelper.GetCache(CacheClassKey + "_GetList");
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<ZbkBanbenData> db = new ORMDataAccess<ZbkBanbenData>())
                {
                    list = db.Query<ZbkBanbenModel>("from ZbkBanbenModel");

                    if (WebKeys.EnableCaching)
                    {
                        //数据存入缓存系统
                        CacheHelper.Add(typeof(ZbkBanbenData), true, null, "GetList", null, CacheClassKey + "_GetList", list);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 根据条件返回数据集合
        /// </summary>
        /// <param name="hql">HQL</param>
        /// <returns></returns>
        public IList<ZbkBanbenModel> GetListByHQL(string hql)
        {
            IList<ZbkBanbenModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //list = (IList<ZbkBanbenModel>)CacheHelper.GetCache(CacheClassKey + "_GetListByHQL_" + hql);
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<ZbkBanbenData> db = new ORMDataAccess<ZbkBanbenData>())
                {
                    if (string.IsNullOrEmpty(hql))
                    {
                        list = db.Query<ZbkBanbenModel>("from ZbkBanbenModel");
                    }
                    else
                    {
                        list = db.Query<ZbkBanbenModel>(hql);
                    }

                    //if (WebKeys.EnableCaching)
                    //{
                    ////数据存入缓存系统
                    //CacheHelper.Add(typeof(ZbkBanbenData), false, null, "GetListByHQL", new object[] { hql }, CacheClassKey + "_GetListByHQL_" + hql, list);
                    //}
                }
            }
            return list;
        }


        /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<ZbkBanbenModel> GetListBySQL(string sql)
        {
            IList<ZbkBanbenModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //    list = (IList<ZbkBanbenModel>)CacheHelper.GetCache(CacheClassKey + "_GetListBySQL_" + sql);
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<ZbkBanbenData> db = new ORMDataAccess<ZbkBanbenData>())
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        list = db.QueryBySQL<ZbkBanbenModel>(sql, typeof(ZbkBanbenModel));

                        //if (WebKeys.EnableCaching)
                        //{
                        ////数据存入缓存系统
                        //CacheHelper.Add(typeof(ZbkBanbenData), false, null, "GetListBySQL", new object[] { sql }, CacheClassKey + "_GetListBySQL_" + sql, list);		
                        //}
                    }
                }
            }
            return list;
        }


        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(ZbkBanbenModel model)
        {
            bool isResult = true;

            using (ORMDataAccess<ZbkBanbenData> db = new ORMDataAccess<ZbkBanbenData>())
            {
                if (model.DB_Option_Action == WebKeys.InsertAction)
                {
                    db.Insert(model);
                }
                else if (model.DB_Option_Action == WebKeys.UpdateAction)
                {
                    db.Update(model);
                }
                else if (model.DB_Option_Action == WebKeys.DeleteAction)
                {
                    db.Delete(model);
                }
            }

            if (WebKeys.EnableCaching)
            {
                //清空相关的缓存
                CacheHelper.RemoveCacheForClassKeys(CacheRelationshipKeys);
            }
            return isResult;
        }

        #endregion

        /// <summary>
        /// 执行基于SQL的原生操作
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool ExecuteSQL(string sql)
        {
            bool isResult = true;

            using (ORMDataAccess<ZbkBanbenData> db = new ORMDataAccess<ZbkBanbenData>())
            {
                isResult = db.ExecuteNonQuery(sql);
            }

            if (WebKeys.EnableCaching)
            {
                //清空相关的缓存
                CacheHelper.RemoveCacheForClassKeys(CacheRelationshipKeys);
            }
            return isResult;
        }
    }
}
