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
    /// 文件名:  ZbkCwjcsjglzbData.cs
    /// 功能描述: 数据层-关联指标与财务基础数据对应表数据访问方法实现类
    /// 创建人：代码生成器
    /// 创建时间 ：2014/12/14 9:19:20
    /// </summary>
    public class ZbkCwjcsjglzbData : IZbkCwjcsjglzbData
    {

        #region 代码生成器
        /// <summary>
        /// 缓存项名称
        /// </summary>
        public static readonly string CacheClassKey = WebKeys.CacheItemKey + typeof(ZbkCwjcsjglzbData).ToString();
        /// <summary>
        /// 缓存关联项数组
        /// </summary>
        public static readonly string[] CacheRelationshipKeys = new string[] { Enterprise.Data.Perfo.Zbk.ZbkCwjcsjglzbData.CacheClassKey };

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ZbkCwjcsjglzbModel GetSingle(string key)
        {
            string hql = "from ZbkCwjcsjglzbModel p where p.ID='" + key + "'";
            return GetListByHQL(hql).FirstOrDefault();
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<ZbkCwjcsjglzbModel> GetList()
        {
            IList<ZbkCwjcsjglzbModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<ZbkCwjcsjglzbModel>)CacheHelper.GetCache(CacheClassKey + "_GetList");
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<ZbkCwjcsjglzbData> db = new ORMDataAccess<ZbkCwjcsjglzbData>())
                {
                    list = db.Query<ZbkCwjcsjglzbModel>("from ZbkCwjcsjglzbModel");

                    if (WebKeys.EnableCaching)
                    {
                        //数据存入缓存系统
                        CacheHelper.Add(typeof(ZbkCwjcsjglzbData), false, null, "GetList", null, CacheClassKey + "_GetList", list);
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
        public IList<ZbkCwjcsjglzbModel> GetListByHQL(string hql)
        {
            IList<ZbkCwjcsjglzbModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<ZbkCwjcsjglzbModel>)CacheHelper.GetCache(CacheClassKey + "_GetListByHQL_" + hql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<ZbkCwjcsjglzbData> db = new ORMDataAccess<ZbkCwjcsjglzbData>())
                {
                    if (string.IsNullOrEmpty(hql))
                    {
                        list = db.Query<ZbkCwjcsjglzbModel>("from ZbkCwjcsjglzbModel");
                    }
                    else
                    {
                        list = db.Query<ZbkCwjcsjglzbModel>(hql);
                    }

                    if (WebKeys.EnableCaching)
                    {
                        //数据存入缓存系统
                        CacheHelper.Add(typeof(ZbkCwjcsjglzbData), false, null, "GetListByHQL", new object[] { hql }, CacheClassKey + "_GetListByHQL_" + hql, list);
                    }
                }
            }
            return list;
        }


        /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<ZbkCwjcsjglzbModel> GetListBySQL(string sql)
        {
            IList<ZbkCwjcsjglzbModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //    list = (IList<ZbkCwjcsjglzbModel>)CacheHelper.GetCache(CacheClassKey + "_GetListBySQL_" + sql);
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<ZbkCwjcsjglzbData> db = new ORMDataAccess<ZbkCwjcsjglzbData>())
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        list = db.QueryBySQL<ZbkCwjcsjglzbModel>(sql, typeof(ZbkCwjcsjglzbModel));

                        //if (WebKeys.EnableCaching)
                        //{
                        ////数据存入缓存系统
                        //CacheHelper.Add(typeof(ZbkCwjcsjglzbData), false, null, "GetListBySQL", new object[] { sql }, CacheClassKey + "_GetListBySQL_" + sql, list);		
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
        public bool Execute(ZbkCwjcsjglzbModel model)
        {
            bool isResult = true;

            using (ORMDataAccess<ZbkCwjcsjglzbData> db = new ORMDataAccess<ZbkCwjcsjglzbData>())
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


        /// <summary>
        /// 执行基于SQL的原生操作
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool ExecuteSQL(string sql)
        {
            bool isResult = true;

            using (ORMDataAccess<ZbkCwjcsjglzbData> db = new ORMDataAccess<ZbkCwjcsjglzbData>())
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

        #endregion
    }
}
