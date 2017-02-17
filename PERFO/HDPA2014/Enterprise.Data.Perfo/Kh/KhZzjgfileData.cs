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
    /// 文件名:  KhZzjgfileData.cs
    /// 功能描述: 数据层-考核结果文件数据访问方法实现类
    /// 创建人：代码生成器
    /// 创建时间 ：2014/11/1 0:35:45
    /// </summary>
    public class KhZzjgfileData : IKhZzjgfileData
    {

        #region 代码生成器
        /// <summary>
        /// 缓存项名称
        /// </summary>
        public static readonly string CacheClassKey = WebKeys.CacheItemKey + typeof(KhZzjgfileData).ToString();

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhZzjgfileModel GetSingle(string key)
        {
            string hql = "from KhZzjgfileModel p where p.KHID='" + key + "'";
            return GetListByHQL(hql).FirstOrDefault();
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhZzjgfileModel> GetList()
        {
            IList<KhZzjgfileModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //list = (IList<KhZzjgfileModel>)CacheHelper.GetCache(CacheClassKey + "_GetList");
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhZzjgfileData> db = new ORMDataAccess<KhZzjgfileData>())
                {
                    list = db.Query<KhZzjgfileModel>("from KhZzjgfileModel");

                    //if (WebKeys.EnableCaching)
                    //{
                    ////数据存入缓存系统
                    //CacheHelper.Add(typeof(KhZzjgfileData), false, null, "GetList", null, CacheClassKey + "_GetList", list);
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
        public IList<KhZzjgfileModel> GetListByHQL(string hql)
        {
            IList<KhZzjgfileModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //list = (IList<KhZzjgfileModel>)CacheHelper.GetCache(CacheClassKey + "_GetListByHQL_" + hql);
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhZzjgfileData> db = new ORMDataAccess<KhZzjgfileData>())
                {
                    if (string.IsNullOrEmpty(hql))
                    {
                        list = db.Query<KhZzjgfileModel>("from KhZzjgfileModel");
                    }
                    else
                    {
                        list = db.Query<KhZzjgfileModel>(hql);
                    }

                    //if (WebKeys.EnableCaching)
                    //{
                    ////数据存入缓存系统
                    //CacheHelper.Add(typeof(KhZzjgfileData), false, null, "GetListByHQL", new object[] { hql }, CacheClassKey + "_GetListByHQL_" + hql, list);
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
        public IList<KhZzjgfileModel> GetListBySQL(string sql)
        {
            IList<KhZzjgfileModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<KhZzjgfileModel>)CacheHelper.GetCache(CacheClassKey + "_GetListBySQL_" + sql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhZzjgfileData> db = new ORMDataAccess<KhZzjgfileData>())
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        list = db.QueryBySQL<KhZzjgfileModel>(sql, typeof(KhZzjgfileModel));

                        //if (WebKeys.EnableCaching)
                        //{
                        ////数据存入缓存系统
                        //CacheHelper.Add(typeof(KhZzjgfileData), false, null, "GetListBySQL", new object[] { sql }, CacheClassKey + "_GetListBySQL_" + sql, list);		
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
        public bool Execute(KhZzjgfileModel model)
        {
            bool isResult = true;

            using (ORMDataAccess<KhZzjgfileData> db = new ORMDataAccess<KhZzjgfileData>())
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
