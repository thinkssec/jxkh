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
    /// 文件名:  KhMessageData.cs
    /// 功能描述: 数据层-待办箱数据访问方法实现类
    /// 创建人：代码生成器
    /// 创建时间 ：2014/11/1 0:35:45
    /// </summary>
    public class KhMessageData : IKhMessageData
    {

        #region 代码生成器
        /// <summary>
        /// 缓存项名称
        /// </summary>
        public static readonly string CacheClassKey = WebKeys.CacheItemKey + typeof(KhMessageData).ToString();

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhMessageModel GetSingle(string key)
        {
            string hql = "from KhMessageModel p where p.MSGID='" + key + "'";
            return GetListByHQL(hql).FirstOrDefault();
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhMessageModel> GetList()
        {
            IList<KhMessageModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //list = (IList<KhMessageModel>)CacheHelper.GetCache(CacheClassKey + "_GetList");
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhMessageData> db = new ORMDataAccess<KhMessageData>())
                {
                    list = db.Query<KhMessageModel>("from KhMessageModel");

                    //if (WebKeys.EnableCaching)
                    //{
                    ////数据存入缓存系统
                    //CacheHelper.Add(typeof(KhMessageData), false, null, "GetList", null, CacheClassKey + "_GetList", list);
                    //}
                }
            }
            return list;
        }

        /// <summary>
        /// 根据条件返回数据集合
        /// </summary>
        /// <param name="hql">HQL</param>
        /// <returns></returns>
        public IList<KhMessageModel> GetListByHQL(string hql)
        {
            IList<KhMessageModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<KhMessageModel>)CacheHelper.GetCache(CacheClassKey + "_GetListByHQL_" + hql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhMessageData> db = new ORMDataAccess<KhMessageData>())
                {
                    if (string.IsNullOrEmpty(hql))
                    {
                        list = db.Query<KhMessageModel>("from KhMessageModel");
                    }
                    else
                    {
                        list = db.Query<KhMessageModel>(hql);
                    }

                    if (WebKeys.EnableCaching)
                    {
                        //数据存入缓存系统
                        CacheHelper.Add(typeof(KhMessageData), false, null, "GetListByHQL", new object[] { hql }, CacheClassKey + "_GetListByHQL_" + hql, list);
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
        public IList<KhMessageModel> GetListBySQL(string sql)
        {
            IList<KhMessageModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<KhMessageModel>)CacheHelper.GetCache(CacheClassKey + "_GetListBySQL_" + sql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhMessageData> db = new ORMDataAccess<KhMessageData>())
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        list = db.QueryBySQL<KhMessageModel>(sql, typeof(KhMessageModel));

                        if (WebKeys.EnableCaching)
                        {
                            //数据存入缓存系统
                            CacheHelper.Add(typeof(KhMessageData), false, null, "GetListBySQL", new object[] { sql }, CacheClassKey + "_GetListBySQL_" + sql, list);
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
        public bool Execute(KhMessageModel model)
        {
            bool isResult = true;

            using (ORMDataAccess<KhMessageData> db = new ORMDataAccess<KhMessageData>())
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

            using (ORMDataAccess<KhMessageData> db = new ORMDataAccess<KhMessageData>())
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
