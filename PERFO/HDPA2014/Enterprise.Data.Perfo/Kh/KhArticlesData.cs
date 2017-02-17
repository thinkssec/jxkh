using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.ORM;
using Enterprise.Component.Infrastructure;
using Enterprise.Component.Cache;
using Enterprise.Model.Perfo.Kh;

namespace Enterprise.Data.Perfo.Kh
{

    /// <summary>
    /// 文件名:  KhArticlesData.cs
    /// 功能描述: 数据层-通知公告数据访问方法实现类
    /// 创建人：代码生成器
    /// 创建时间 ：2014/11/1 0:29:31
    /// </summary>
    public class KhArticlesData : IKhArticlesData
    {

        #region 代码生成器
        /// <summary>
        /// 缓存项名称
        /// </summary>
        public static readonly string CacheClassKey = WebKeys.CacheItemKey + typeof(KhArticlesData).ToString();

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhArticlesModel GetSingle(string key)
        {
            //string hql = "from KhArticlesModel p where p.TZID='" + key + "'";
            return GetList().FirstOrDefault(p => p.TZID == key);
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhArticlesModel> GetList()
        {
            IList<KhArticlesModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<KhArticlesModel>)CacheHelper.GetCache(CacheClassKey + "_GetList");
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhArticlesData> db = new ORMDataAccess<KhArticlesData>())
                {
                    list = db.Query<KhArticlesModel>("from KhArticlesModel p order by p.TJRQ desc");

                    if (WebKeys.EnableCaching)
                    {
                        //数据存入缓存系统
                        CacheHelper.Add(typeof(KhArticlesData), true, null, "GetList", null, CacheClassKey + "_GetList", list);
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
        public IList<KhArticlesModel> GetListByHQL(string hql)
        {
            IList<KhArticlesModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<KhArticlesModel>)CacheHelper.GetCache(CacheClassKey + "_GetListByHQL_" + hql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhArticlesData> db = new ORMDataAccess<KhArticlesData>())
                {
                    if (string.IsNullOrEmpty(hql))
                    {
                        list = db.Query<KhArticlesModel>("from KhArticlesModel");
                    }
                    else
                    {
                        list = db.Query<KhArticlesModel>(hql);
                    }

                    if (WebKeys.EnableCaching)
                    {
                        //数据存入缓存系统
                        CacheHelper.Add(typeof(KhArticlesData), true, null, "GetListByHQL", new object[] { hql }, CacheClassKey + "_GetListByHQL_" + hql, list);
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
        public IList<KhArticlesModel> GetListBySQL(string sql)
        {
            IList<KhArticlesModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<KhArticlesModel>)CacheHelper.GetCache(CacheClassKey + "_GetListBySQL_" + sql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhArticlesData> db = new ORMDataAccess<KhArticlesData>())
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        list = db.QueryBySQL<KhArticlesModel>(sql, typeof(KhArticlesModel));

                        if (WebKeys.EnableCaching)
                        {
                            //数据存入缓存系统
                            CacheHelper.Add(typeof(KhArticlesData), false, null, "GetListBySQL", new object[] { sql }, CacheClassKey + "_GetListBySQL_" + sql, list);
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
        public bool Execute(KhArticlesModel model)
        {
            bool isResult = true;

            using (ORMDataAccess<KhArticlesData> db = new ORMDataAccess<KhArticlesData>())
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

        /// <summary>
        /// 执行基于SQL的原生操作
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool ExecuteSQL(string sql)
        {
            bool isResult = true;

            using (ORMDataAccess<KhArticlesData> db = new ORMDataAccess<KhArticlesData>())
            {
                isResult = db.ExecuteNonQuery(sql);
            }

            if (WebKeys.EnableCaching)
            {
                //清空相关的缓存
                CacheHelper.RemoveCacheForClassKey(CacheClassKey);
            }

            return isResult;
        }
    }
}
