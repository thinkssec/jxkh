using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.ORM;
using Enterprise.Component.Infrastructure;
using Enterprise.Component.Cache;
using Enterprise.Model.Perfo.Sys;

namespace Enterprise.Data.Perfo.Sys
{

    /// <summary>
    /// 文件名:  SysCacheData.cs
    /// 功能描述: 数据层-系统缓存管理数据访问方法实现类
    /// 创建人：代码生成器
    /// 创建时间 ：2014/10/31 10:02:24
    /// </summary>
    public class SysCacheData : ISysCacheData
    {
        #region 代码生成器

        /// <summary>
        /// 缓存项名称
        /// </summary>
        public static readonly string CacheClassKey = WebKeys.CacheItemKey + typeof(SysCacheData).ToString();

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SysCacheModel GetSingle(string key)
        {
            return null;
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<SysCacheModel> GetList()
        {
            IList<SysCacheModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<SysCacheModel>)CacheHelper.GetCache(CacheClassKey + "_GetList");
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<SysCacheData> db = new ORMDataAccess<SysCacheData>())
                {
                    list = db.Query<SysCacheModel>("from SysCacheModel");

                    if (WebKeys.EnableCaching)
                    {
                        //数据存入缓存系统
                        CacheItemRefreshAction refreshAction =
                        new CacheItemRefreshAction(typeof(SysCacheData), false);
                        refreshAction.ConstuctParms = null;
                        refreshAction.MethodName = "GetList";
                        refreshAction.Parameters = null;//new object[]{};
                        CacheHelper.Add(CacheClassKey + "_GetList", list, refreshAction);
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
        public IList<SysCacheModel> GetListByHQL(string hql)
        {
            IList<SysCacheModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //list = (IList<SysCacheModel>)CacheHelper.GetCache(CacheClassKey + "_GetListByHQL_" + hql);
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<SysCacheData> db = new ORMDataAccess<SysCacheData>())
                {
                    if (string.IsNullOrEmpty(hql))
                    {
                        list = db.Query<SysCacheModel>("from SysCacheModel");
                    }
                    else
                    {
                        list = db.Query<SysCacheModel>(hql);
                    }

                    //if (WebKeys.EnableCaching)
                    //{
                    ////数据存入缓存系统
                    //CacheItemRefreshAction refreshAction =
                    //new CacheItemRefreshAction(typeof(SysCacheData), false);
                    //refreshAction.ConstuctParms = null;
                    //refreshAction.MethodName = "GetListByHQL";
                    //refreshAction.Parameters = new object[]{ hql };
                    //CacheHelper.Add(CacheClassKey + "_GetListByHQL_" + hql, list, refreshAction);
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
        public IList<SysCacheModel> GetListBySQL(string sql)
        {
            IList<SysCacheModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<SysCacheModel>)CacheHelper.GetCache(CacheClassKey + "_GetListBySQL_" + sql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<SysCacheData> db = new ORMDataAccess<SysCacheData>())
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        list = db.QueryBySQL<SysCacheModel>(sql, typeof(SysCacheModel));

                        if (WebKeys.EnableCaching)
                        {
                            CacheHelper.Add(typeof(SysCacheData), false, null, "GetListBySQL", 
                                new object[] { sql }, CacheClassKey + "_GetListBySQL_" + sql, list);
                        }
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
        public bool Execute(SysCacheModel model)
        {
            bool isResult = true;

            using (ORMDataAccess<SysCacheData> db = new ORMDataAccess<SysCacheData>())
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
                CacheHelper.RemoveCacheForClassKey(CacheClassKey);
            }

            return isResult;
        }

        #endregion
    }
}
