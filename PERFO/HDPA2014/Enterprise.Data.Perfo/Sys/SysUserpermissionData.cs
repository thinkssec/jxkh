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
    /// 文件名:  SysUserpermissionData.cs
    /// 功能描述: 数据层-数据访问方法实现类
    /// 创建人：代码生成器
    /// 创建时间 ：2014/12/1 9:32:19
    /// </summary>
    public class SysUserpermissionData : ISysUserpermissionData
    {

        #region 代码生成器
        /// <summary>
        /// 缓存项名称
        /// </summary>
        public static readonly string CacheClassKey = WebKeys.CacheItemKey + typeof(SysUserpermissionData).ToString();
        /// <summary>
        /// 缓存关联项数组
        /// </summary>
        public static readonly string[] CacheRelationshipKeys = new string[] { Enterprise.Data.Perfo.Sys.SysUserpermissionData.CacheClassKey };

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SysUserpermissionModel GetSingle(string key)
        {
            return null;
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<SysUserpermissionModel> GetList()
        {
            IList<SysUserpermissionModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<SysUserpermissionModel>)CacheHelper.GetCache(CacheClassKey + "_GetList");
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<SysUserpermissionData> db = new ORMDataAccess<SysUserpermissionData>())
                {
                    list = db.Query<SysUserpermissionModel>("from SysUserpermissionModel");

                    if (WebKeys.EnableCaching)
                    {
                        //数据存入缓存系统
                        CacheHelper.Add(typeof(SysUserpermissionData), false, null, "GetList", null, CacheClassKey + "_GetList", list);
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
        public IList<SysUserpermissionModel> GetListByHQL(string hql)
        {
            IList<SysUserpermissionModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //list = (IList<SysUserpermissionModel>)CacheHelper.GetCache(CacheClassKey + "_GetListByHQL_" + hql);
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<SysUserpermissionData> db = new ORMDataAccess<SysUserpermissionData>())
                {
                    if (string.IsNullOrEmpty(hql))
                    {
                        list = db.Query<SysUserpermissionModel>("from SysUserpermissionModel");
                    }
                    else
                    {
                        list = db.Query<SysUserpermissionModel>(hql);
                    }

                    //if (WebKeys.EnableCaching)
                    //{
                    ////数据存入缓存系统
                    //CacheHelper.Add(typeof(SysUserpermissionData), false, null, "GetListByHQL", new object[] { hql }, CacheClassKey + "_GetListByHQL_" + hql, list);
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
        public IList<SysUserpermissionModel> GetListBySQL(string sql)
        {
            IList<SysUserpermissionModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //    list = (IList<SysUserpermissionModel>)CacheHelper.GetCache(CacheClassKey + "_GetListBySQL_" + sql);
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<SysUserpermissionData> db = new ORMDataAccess<SysUserpermissionData>())
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        list = db.QueryBySQL<SysUserpermissionModel>(sql, typeof(SysUserpermissionModel));

                        //if (WebKeys.EnableCaching)
                        //{
                        ////数据存入缓存系统
                        //CacheHelper.Add(typeof(SysUserpermissionData), false, null, "GetListBySQL", new object[] { sql }, CacheClassKey + "_GetListBySQL_" + sql, list);		
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
        public bool Execute(SysUserpermissionModel model)
        {
            bool isResult = true;

            using (ORMDataAccess<SysUserpermissionData> db = new ORMDataAccess<SysUserpermissionData>())
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

            using (ORMDataAccess<SysUserpermissionData> db = new ORMDataAccess<SysUserpermissionData>())
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
