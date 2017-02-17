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
    /// 文件名:  KhCwjcsjData.cs
    /// 功能描述: 数据层-财务基础数据表数据访问方法实现类
    /// 创建人：代码生成器
    /// 创建时间 ：2014/12/13 10:23:51
    /// </summary>
    public class KhCwjcsjData : IKhCwjcsjData
    {

        #region 代码生成器
        /// <summary>
        /// 缓存项名称
        /// </summary>
        public static readonly string CacheClassKey = WebKeys.CacheItemKey + typeof(KhCwjcsjData).ToString();
        /// <summary>
        /// 缓存关联项数组
        /// </summary>
        public static readonly string[] CacheRelationshipKeys = new string[] { Enterprise.Data.Perfo.Kh.KhCwjcsjData.CacheClassKey };

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhCwjcsjModel GetSingle(string key)
        {
            return null;
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhCwjcsjModel> GetList()
        {
            IList<KhCwjcsjModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //list = (IList<KhCwjcsjModel>)CacheHelper.GetCache(CacheClassKey + "_GetList");
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhCwjcsjData> db = new ORMDataAccess<KhCwjcsjData>())
                {
                    list = db.Query<KhCwjcsjModel>("from KhCwjcsjModel");

                    //if (WebKeys.EnableCaching)
                    //{
                    ////数据存入缓存系统
                    //CacheHelper.Add(typeof(KhCwjcsjData), false, null, "GetList", null, CacheClassKey + "_GetList", list);
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
        public IList<KhCwjcsjModel> GetListByHQL(string hql)
        {
            IList<KhCwjcsjModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<KhCwjcsjModel>)CacheHelper.GetCache(CacheClassKey + "_GetListByHQL_" + hql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhCwjcsjData> db = new ORMDataAccess<KhCwjcsjData>())
                {
                    if (string.IsNullOrEmpty(hql))
                    {
                        list = db.Query<KhCwjcsjModel>("from KhCwjcsjModel");
                    }
                    else
                    {
                        list = db.Query<KhCwjcsjModel>(hql);
                    }

                    if (WebKeys.EnableCaching)
                    {
                        //数据存入缓存系统
                        CacheHelper.Add(typeof(KhCwjcsjData), false, null, "GetListByHQL", new object[] { hql }, CacheClassKey + "_GetListByHQL_" + hql, list);
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
        public IList<KhCwjcsjModel> GetListBySQL(string sql)
        {
            IList<KhCwjcsjModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //    list = (IList<KhCwjcsjModel>)CacheHelper.GetCache(CacheClassKey + "_GetListBySQL_" + sql);
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhCwjcsjData> db = new ORMDataAccess<KhCwjcsjData>())
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        list = db.QueryBySQL<KhCwjcsjModel>(sql, typeof(KhCwjcsjModel));

                        //if (WebKeys.EnableCaching)
                        //{
                        ////数据存入缓存系统
                        //CacheHelper.Add(typeof(KhCwjcsjData), false, null, "GetListBySQL", new object[] { sql }, CacheClassKey + "_GetListBySQL_" + sql, list);		
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
        public bool Execute(KhCwjcsjModel model)
        {
            bool isResult = true;

            using (ORMDataAccess<KhCwjcsjData> db = new ORMDataAccess<KhCwjcsjData>())
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

            using (ORMDataAccess<KhCwjcsjData> db = new ORMDataAccess<KhCwjcsjData>())
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
