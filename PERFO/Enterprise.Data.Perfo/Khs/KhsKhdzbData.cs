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
    /// 文件名:  KhsKhdzbData.cs
    /// 功能描述: 数据层-考核指标对照表数据访问方法实现类
    /// 创建人：代码生成器
	/// 创建时间 ：2015/11/4 20:47:10
    /// </summary>
    public class KhsKhdzbData : IKhsKhdzbData
    {
        #region 代码生成器
		/// <summary>
        /// 缓存项名称
        /// </summary>
        public static readonly string CacheClassKey = WebKeys.CacheItemKey + typeof(KhsKhdzbData).ToString();
        public bool ExecuteSQL(string sql)
        {
            bool isResult = true;

            using (ORMDataAccess<KhsKhdzbData> db = new ORMDataAccess<KhsKhdzbData>())
            {
                isResult = db.ExecuteNonQuery(sql);
            }

             
            return isResult;
        }
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhsKhdzbModel> GetList()
        {
            IList<KhsKhdzbModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
                //list = (IList<KhsKhdzbModel>)CacheHelper.GetCache(CacheClassKey + "_GetList");
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhsKhdzbData> db = new ORMDataAccess<KhsKhdzbData>())
                {
                    list = db.Query<KhsKhdzbModel>("from KhsKhdzbModel");
                    
		    //if (WebKeys.EnableCaching)
           	    //{
                    ////数据存入缓存系统
                    //CacheItemRefreshAction refreshAction =
                        //new CacheItemRefreshAction(typeof(KhsKhdzbData), false);
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
        public IList<KhsKhdzbModel> GetListByHQL(string hql)
        {
            IList<KhsKhdzbModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
                //list = (IList<KhsKhdzbModel>)CacheHelper.GetCache(CacheClassKey + "_GetListByHQL_" + hql);
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhsKhdzbData> db = new ORMDataAccess<KhsKhdzbData>())
                {
			if (string.IsNullOrEmpty(hql))
			{
				list = db.Query<KhsKhdzbModel>("from KhsKhdzbModel");
			}
			else
			{
				list = db.Query<KhsKhdzbModel>(hql);
			}

			    //if (WebKeys.EnableCaching)
	           	    //{
	                    ////数据存入缓存系统
	                    //CacheItemRefreshAction refreshAction =
	                        //new CacheItemRefreshAction(typeof(KhsKhdzbData), false);
	                    //refreshAction.ConstuctParms = null;
	                    //refreshAction.MethodName = "GetListByHQL";
	                    //refreshAction.Parameters = new object[]{ hql };
	                    //CacheHelper.Add(CacheClassKey + "_GetListByHQL_" + hql, list, refreshAction);
			    //}
                }
            }
            return list;
        }

        public KhsKhdzbModel GetSingle(string key) {
            return null;
        }
	/// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhsKhdzbModel> GetListBySQL(string sql)
        {
            IList<KhsKhdzbModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<KhsKhdzbModel>)CacheHelper.GetCache(CacheClassKey + "_GetListBySQL_" + sql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhsKhdzbData> db = new ORMDataAccess<KhsKhdzbData>())
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        list = db.QueryBySQL<KhsKhdzbModel>(sql, typeof(KhsKhdzbModel));

                        if (WebKeys.EnableCaching)
                        {
                            //数据存入缓存系统
                            CacheItemRefreshAction refreshAction =
                            new CacheItemRefreshAction(typeof(KhsKhdzbData), false);
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
        public bool Execute(KhsKhdzbModel model)
        {
            bool isResult = true;

            using (ORMDataAccess<KhsKhdzbData> db = new ORMDataAccess<KhsKhdzbData>())
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
