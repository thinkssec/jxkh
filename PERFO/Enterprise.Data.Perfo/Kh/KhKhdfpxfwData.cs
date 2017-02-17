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
    /// 文件名:  KhKhdfpxfwData.cs
    /// 功能描述: 数据层-考核得分排序范围设置数据访问方法实现类
    /// 创建人：代码生成器
    /// 创建时间 ：2014/12/2 13:41:05
    /// </summary>
    public class KhKhdfpxfwData : IKhKhdfpxfwData
    {

        #region 代码生成器
        /// <summary>
        /// 缓存项名称
        /// </summary>
        public static readonly string CacheClassKey = WebKeys.CacheItemKey + typeof(KhKhdfpxfwData).ToString();
        /// <summary>
        /// 缓存关联项数组
        /// </summary>
        public static readonly string[] CacheRelationshipKeys = new string[] { 
            Enterprise.Data.Perfo.Kh.KhKhdfpxfwData.CacheClassKey,
            Enterprise.Data.Perfo.Kh.KhKhglData.CacheClassKey
        };

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhKhdfpxfwModel GetSingle(string key)
        {
            string hql = "from KhKhdfpxfwModel p where p.ID='" + key + "'";
            return GetListByHQL(hql).FirstOrDefault();
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhKhdfpxfwModel> GetList()
        {
            IList<KhKhdfpxfwModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //list = (IList<KhKhdfpxfwModel>)CacheHelper.GetCache(CacheClassKey + "_GetList");
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhKhdfpxfwData> db = new ORMDataAccess<KhKhdfpxfwData>())
                {
                    list = db.Query<KhKhdfpxfwModel>("from KhKhdfpxfwModel");

                    //if (WebKeys.EnableCaching)
                    //{
                    ////数据存入缓存系统
                    //CacheHelper.Add(typeof(KhKhdfpxfwData), false, null, "GetList", null, CacheClassKey + "_GetList", list);
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
        public IList<KhKhdfpxfwModel> GetListByHQL(string hql)
        {
            IList<KhKhdfpxfwModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<KhKhdfpxfwModel>)CacheHelper.GetCache(CacheClassKey + "_GetListByHQL_" + hql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhKhdfpxfwData> db = new ORMDataAccess<KhKhdfpxfwData>())
                {
                    if (string.IsNullOrEmpty(hql))
                    {
                        list = db.Query<KhKhdfpxfwModel>("from KhKhdfpxfwModel");
                    }
                    else
                    {
                        list = db.Query<KhKhdfpxfwModel>(hql);
                    }

                    if (WebKeys.EnableCaching)
                    {
                        //数据存入缓存系统
                        CacheHelper.Add(typeof(KhKhdfpxfwData), false, null, "GetListByHQL", new object[] { hql }, CacheClassKey + "_GetListByHQL_" + hql, list);
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
        public IList<KhKhdfpxfwModel> GetListBySQL(string sql)
        {
            IList<KhKhdfpxfwModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //    list = (IList<KhKhdfpxfwModel>)CacheHelper.GetCache(CacheClassKey + "_GetListBySQL_" + sql);
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhKhdfpxfwData> db = new ORMDataAccess<KhKhdfpxfwData>())
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        list = db.QueryBySQL<KhKhdfpxfwModel>(sql, typeof(KhKhdfpxfwModel));

                        //if (WebKeys.EnableCaching)
                        //{
                        ////数据存入缓存系统
                        //CacheHelper.Add(typeof(KhKhdfpxfwData), false, null, "GetListBySQL", new object[] { sql }, CacheClassKey + "_GetListBySQL_" + sql, list);		
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
        public bool Execute(KhKhdfpxfwModel model)
        {
            bool isResult = true;

            using (ORMDataAccess<KhKhdfpxfwData> db = new ORMDataAccess<KhKhdfpxfwData>())
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

            using (ORMDataAccess<KhKhdfpxfwData> db = new ORMDataAccess<KhKhdfpxfwData>())
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
