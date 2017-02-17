using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.ORM;
using Enterprise.Component.Infrastructure;
using Enterprise.Component.Cache;
using Enterprise.Model.Perfo.Khs;

namespace Enterprise.Data.Perfo.Khs
{

    /// <summary>
    /// 文件名:  KhsQzData.cs
    /// 功能描述: 数据层-数据访问方法实现类
    /// 创建人：代码生成器
	/// 创建时间 ：2015/11/7 19:21:46
    /// </summary>
    public class KhsQzData : IKhsQzData
    {
        public KhsQzModel GetSingle(string key)
        {
            return null;
        }
        #region 代码生成器
		/// <summary>
        /// 缓存项名称
        /// </summary>
        public static readonly string CacheClassKey = WebKeys.CacheItemKey + typeof(KhsQzData).ToString();

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhsQzModel> GetList()
        {
            IList<KhsQzModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
                //list = (IList<KhsQzModel>)CacheHelper.GetCache(CacheClassKey + "_GetList");
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhsQzData> db = new ORMDataAccess<KhsQzData>())
                {
                    list = db.Query<KhsQzModel>("from KhsQzModel");
                    
		    //if (WebKeys.EnableCaching)
           	    //{
                    ////数据存入缓存系统
                    //CacheItemRefreshAction refreshAction =
                        //new CacheItemRefreshAction(typeof(KhsQzData), false);
                    //refreshAction.ConstuctParms = null;
                    //refreshAction.MethodName = "GetList";
                    //refreshAction.Parameters = null;//new object[]{};
                    //CacheHelper.Add(CacheClassKey + "_GetList", list, refreshAction);
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
        public IList<KhsQzModel> GetListByHQL(string hql)
        {
            IList<KhsQzModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
                //list = (IList<KhsQzModel>)CacheHelper.GetCache(CacheClassKey + "_GetListByHQL_" + hql);
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhsQzData> db = new ORMDataAccess<KhsQzData>())
                {
			if (string.IsNullOrEmpty(hql))
			{
				list = db.Query<KhsQzModel>("from KhsQzModel");
			}
			else
			{
				list = db.Query<KhsQzModel>(hql);
			}

			    //if (WebKeys.EnableCaching)
	           	    //{
	                    ////数据存入缓存系统
	                    //CacheItemRefreshAction refreshAction =
	                        //new CacheItemRefreshAction(typeof(KhsQzData), false);
	                    //refreshAction.ConstuctParms = null;
	                    //refreshAction.MethodName = "GetListByHQL";
	                    //refreshAction.Parameters = new object[]{ hql };
	                    //CacheHelper.Add(CacheClassKey + "_GetListByHQL_" + hql, list, refreshAction);
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
        public IList<KhsQzModel> GetListBySQL(string sql)
        {
            IList<KhsQzModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<KhsQzModel>)CacheHelper.GetCache(CacheClassKey + "_GetListBySQL_" + sql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhsQzData> db = new ORMDataAccess<KhsQzData>())
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        list = db.QueryBySQL<KhsQzModel>(sql, typeof(KhsQzModel));

                        if (WebKeys.EnableCaching)
                        {
                            //数据存入缓存系统
                            CacheItemRefreshAction refreshAction =
                            new CacheItemRefreshAction(typeof(KhsQzData), false);
                            refreshAction.ConstuctParms = null;
                            refreshAction.MethodName = "GetListBySQL";
                            refreshAction.Parameters = new object[] { sql };
                            CacheHelper.Add(CacheClassKey + "_GetListBySQL_" + sql, list, refreshAction);
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
        public bool Execute(KhsQzModel model)
        {
            bool isResult = true;

            using (ORMDataAccess<KhsQzData> db = new ORMDataAccess<KhsQzData>())
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
        public bool ExecuteSQL(string sql)
        {
            bool isResult = true;

            using (ORMDataAccess<KhsQzData> db = new ORMDataAccess<KhsQzData>())
            {
                isResult = db.ExecuteNonQuery(sql);
            }

            return isResult;
        }
        #endregion
    }
}
