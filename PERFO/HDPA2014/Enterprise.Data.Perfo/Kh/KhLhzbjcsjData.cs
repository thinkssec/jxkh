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
    /// 文件名:  KhLhzbjcsjData.cs
    /// 功能描述: 数据层-量化考核基础数据表数据访问方法实现类
    /// 创建人：代码生成器
    /// 创建时间 ：2014/11/1 0:35:45
    /// </summary>
    public class KhLhzbjcsjData : IKhLhzbjcsjData
    {

        #region 代码生成器
        /// <summary>
        /// 缓存项名称
        /// </summary>
        public static readonly string CacheClassKey = WebKeys.CacheItemKey + typeof(KhLhzbjcsjData).ToString();
        /// <summary>
        /// 缓存关联项数组
        /// </summary>
        public static readonly string[] CacheRelationshipKeys = new string[] { 
            Enterprise.Data.Perfo.Kh.KhLhzbjcsjData.CacheClassKey,
            Enterprise.Data.Perfo.Kh.KhKhglData.CacheClassKey
        };

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhLhzbjcsjModel GetSingle(string key)
        {
            string hql = "from KhLhzbjcsjModel p where p.JCZBID='" + key + "'";
            return GetListByHQL(hql).FirstOrDefault();
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhLhzbjcsjModel> GetList()
        {
            IList<KhLhzbjcsjModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //list = (IList<KhLhzbjcsjModel>)CacheHelper.GetCache(CacheClassKey + "_GetList");
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhLhzbjcsjData> db = new ORMDataAccess<KhLhzbjcsjData>())
                {
                    list = db.Query<KhLhzbjcsjModel>("from KhLhzbjcsjModel");

                    //if (WebKeys.EnableCaching)
                    //{
                    ////数据存入缓存系统
                    //CacheHelper.Add(typeof(KhLhzbjcsjData), false, null, "GetList", null, CacheClassKey + "_GetList", list);
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
        public IList<KhLhzbjcsjModel> GetListByHQL(string hql)
        {
            IList<KhLhzbjcsjModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //list = (IList<KhLhzbjcsjModel>)CacheHelper.GetCache(CacheClassKey + "_GetListByHQL_" + hql);
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhLhzbjcsjData> db = new ORMDataAccess<KhLhzbjcsjData>())
                {
                    if (string.IsNullOrEmpty(hql))
                    {
                        list = db.Query<KhLhzbjcsjModel>("from KhLhzbjcsjModel");
                    }
                    else
                    {
                        list = db.Query<KhLhzbjcsjModel>(hql);
                    }

                    //if (WebKeys.EnableCaching)
                    //{
                    ////数据存入缓存系统
                    //CacheHelper.Add(typeof(KhLhzbjcsjData), false, null, "GetListByHQL", new object[] { hql }, CacheClassKey + "_GetListByHQL_" + hql, list);
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
        public IList<KhLhzbjcsjModel> GetListBySQL(string sql)
        {
            IList<KhLhzbjcsjModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<KhLhzbjcsjModel>)CacheHelper.GetCache(CacheClassKey + "_GetListBySQL_" + sql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhLhzbjcsjData> db = new ORMDataAccess<KhLhzbjcsjData>())
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        list = db.QueryBySQL<KhLhzbjcsjModel>(sql, typeof(KhLhzbjcsjModel));

                        //if (WebKeys.EnableCaching)
                        //{
                        ////数据存入缓存系统
                        //CacheHelper.Add(typeof(KhLhzbjcsjData), false, null, "GetListBySQL", new object[] { sql }, CacheClassKey + "_GetListBySQL_" + sql, list);		
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
        public bool Execute(KhLhzbjcsjModel model)
        {
            bool isResult = true;

            using (ORMDataAccess<KhLhzbjcsjData> db = new ORMDataAccess<KhLhzbjcsjData>())
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
    }
}
