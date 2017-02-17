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
    /// 文件名:  KhHbjfgzData.cs
    /// 功能描述: 数据层-合并计分规则表数据访问方法实现类
    /// 创建人：代码生成器
    /// 创建时间 ：2014/12/2 13:41:05
    /// </summary>
    public class KhHbjfgzData : IKhHbjfgzData
    {

        #region 代码生成器
        /// <summary>
        /// 缓存项名称
        /// </summary>
        public static readonly string CacheClassKey = WebKeys.CacheItemKey + typeof(KhHbjfgzData).ToString();
        /// <summary>
        /// 缓存关联项数组
        /// </summary>
        public static readonly string[] CacheRelationshipKeys = new string[] { 
            Enterprise.Data.Perfo.Kh.KhHbjfgzData.CacheClassKey,
            Enterprise.Data.Perfo.Kh.KhKhglData.CacheClassKey
        };

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhHbjfgzModel GetSingle(string key)
        {
            string hql = "from KhHbjfgzModel p where p.HBJFID='" + key + "'";
            return null;
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhHbjfgzModel> GetList()
        {
            IList<KhHbjfgzModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //list = (IList<KhHbjfgzModel>)CacheHelper.GetCache(CacheClassKey + "_GetList");
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhHbjfgzData> db = new ORMDataAccess<KhHbjfgzData>())
                {
                    list = db.Query<KhHbjfgzModel>("from KhHbjfgzModel");

                    //if (WebKeys.EnableCaching)
                    //{
                    ////数据存入缓存系统
                    //CacheHelper.Add(typeof(KhHbjfgzData), false, null, "GetList", null, CacheClassKey + "_GetList", list);
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
        public IList<KhHbjfgzModel> GetListByHQL(string hql)
        {
            IList<KhHbjfgzModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<KhHbjfgzModel>)CacheHelper.GetCache(CacheClassKey + "_GetListByHQL_" + hql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhHbjfgzData> db = new ORMDataAccess<KhHbjfgzData>())
                {
                    if (string.IsNullOrEmpty(hql))
                    {
                        list = db.Query<KhHbjfgzModel>("from KhHbjfgzModel");
                    }
                    else
                    {
                        list = db.Query<KhHbjfgzModel>(hql);
                    }

                    if (WebKeys.EnableCaching)
                    {
                        //数据存入缓存系统
                        CacheHelper.Add(typeof(KhHbjfgzData), false, null, "GetListByHQL", new object[] { hql }, CacheClassKey + "_GetListByHQL_" + hql, list);
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
        public IList<KhHbjfgzModel> GetListBySQL(string sql)
        {
            IList<KhHbjfgzModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //    list = (IList<KhHbjfgzModel>)CacheHelper.GetCache(CacheClassKey + "_GetListBySQL_" + sql);
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhHbjfgzData> db = new ORMDataAccess<KhHbjfgzData>())
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        list = db.QueryBySQL<KhHbjfgzModel>(sql, typeof(KhHbjfgzModel));

                        //if (WebKeys.EnableCaching)
                        //{
                        ////数据存入缓存系统
                        //CacheHelper.Add(typeof(KhHbjfgzData), false, null, "GetListBySQL", new object[] { sql }, CacheClassKey + "_GetListBySQL_" + sql, list);		
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
        public bool Execute(KhHbjfgzModel model)
        {
            bool isResult = true;

            using (ORMDataAccess<KhHbjfgzData> db = new ORMDataAccess<KhHbjfgzData>())
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

            using (ORMDataAccess<KhHbjfgzData> db = new ORMDataAccess<KhHbjfgzData>())
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
