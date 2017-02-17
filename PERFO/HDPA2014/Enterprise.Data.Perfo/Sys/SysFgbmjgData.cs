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
    /// 文件名:  SysFgbmjgData.cs
    /// 功能描述: 数据层-用户分管机构数据访问方法实现类
    /// 创建人：代码生成器
    /// 创建时间 ：2014/11/1 0:35:45
    /// </summary>
    public class SysFgbmjgData : ISysFgbmjgData
    {

        #region 代码生成器
        /// <summary>
        /// 缓存项名称
        /// </summary>
        public static readonly string CacheClassKey = WebKeys.CacheItemKey + typeof(SysFgbmjgData).ToString();

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SysFgbmjgModel GetSingle(string key)
        {
            return null;
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<SysFgbmjgModel> GetList()
        {
            IList<SysFgbmjgModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //list = (IList<SysFgbmjgModel>)CacheHelper.GetCache(CacheClassKey + "_GetList");
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<SysFgbmjgData> db = new ORMDataAccess<SysFgbmjgData>())
                {
                    list = db.Query<SysFgbmjgModel>("from SysFgbmjgModel");

                    //if (WebKeys.EnableCaching)
                    //{
                    ////数据存入缓存系统
                    //CacheHelper.Add(typeof(SysFgbmjgData), false, null, "GetList", null, CacheClassKey + "_GetList", list);
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
        public IList<SysFgbmjgModel> GetListByHQL(string hql)
        {
            IList<SysFgbmjgModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<SysFgbmjgModel>)CacheHelper.GetCache(CacheClassKey + "_GetListByHQL_" + hql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<SysFgbmjgData> db = new ORMDataAccess<SysFgbmjgData>())
                {
                    if (string.IsNullOrEmpty(hql))
                    {
                        list = db.Query<SysFgbmjgModel>("from SysFgbmjgModel");
                    }
                    else
                    {
                        list = db.Query<SysFgbmjgModel>(hql);
                    }

                    if (WebKeys.EnableCaching)
                    {
                        //数据存入缓存系统
                        CacheHelper.Add(typeof(SysFgbmjgData), false, null, "GetListByHQL", new object[] { hql }, CacheClassKey + "_GetListByHQL_" + hql, list);
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
        public IList<SysFgbmjgModel> GetListBySQL(string sql)
        {
            IList<SysFgbmjgModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<SysFgbmjgModel>)CacheHelper.GetCache(CacheClassKey + "_GetListBySQL_" + sql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<SysFgbmjgData> db = new ORMDataAccess<SysFgbmjgData>())
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        list = db.QueryBySQL<SysFgbmjgModel>(sql, typeof(SysFgbmjgModel));

                        //if (WebKeys.EnableCaching)
                        //{
                        ////数据存入缓存系统
                        //CacheHelper.Add(typeof(SysFgbmjgData), false, null, "GetListBySQL", new object[] { sql }, CacheClassKey + "_GetListBySQL_" + sql, list);		
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
        public bool Execute(SysFgbmjgModel model)
        {
            bool isResult = true;

            using (ORMDataAccess<SysFgbmjgData> db = new ORMDataAccess<SysFgbmjgData>())
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
