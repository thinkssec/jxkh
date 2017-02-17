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
    /// �ļ���:  KhsKhbzData.cs
    /// ��������: ���ݲ�-���ݷ��ʷ���ʵ����
    /// �����ˣ�����������
	/// ����ʱ�� ��2015/11/5 19:18:56
    /// </summary>
    public class KhsKhbzData : IKhsKhbzData
    {
        #region ����������
		/// <summary>
        /// ����������
        /// </summary>
        public static readonly string CacheClassKey = WebKeys.CacheItemKey + typeof(KhsKhbzData).ToString();
        public KhsKhbzModel GetSingle(string key)
        {
            return null;
        }
        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhsKhbzModel> GetList()
        {
            IList<KhsKhbzModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
                //list = (IList<KhsKhbzModel>)CacheHelper.GetCache(CacheClassKey + "_GetList");
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhsKhbzData> db = new ORMDataAccess<KhsKhbzData>())
                {
                    list = db.Query<KhsKhbzModel>("from KhsKhbzModel");
                    
		    //if (WebKeys.EnableCaching)
           	    //{
                    ////���ݴ��뻺��ϵͳ
                    //CacheItemRefreshAction refreshAction =
                        //new CacheItemRefreshAction(typeof(KhsKhbzData), false);
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
        /// ���������������ݼ���
        /// </summary>
        /// <param name="hql">HQL</param>
        /// <returns></returns>
        public IList<KhsKhbzModel> GetListByHQL(string hql)
        {
            IList<KhsKhbzModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
                //list = (IList<KhsKhbzModel>)CacheHelper.GetCache(CacheClassKey + "_GetListByHQL_" + hql);
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhsKhbzData> db = new ORMDataAccess<KhsKhbzData>())
                {
			if (string.IsNullOrEmpty(hql))
			{
				list = db.Query<KhsKhbzModel>("from KhsKhbzModel");
			}
			else
			{
				list = db.Query<KhsKhbzModel>(hql);
			}

			    //if (WebKeys.EnableCaching)
	           	    //{
	                    ////���ݴ��뻺��ϵͳ
	                    //CacheItemRefreshAction refreshAction =
	                        //new CacheItemRefreshAction(typeof(KhsKhbzData), false);
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
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhsKhbzModel> GetListBySQL(string sql)
        {
            IList<KhsKhbzModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<KhsKhbzModel>)CacheHelper.GetCache(CacheClassKey + "_GetListBySQL_" + sql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhsKhbzData> db = new ORMDataAccess<KhsKhbzData>())
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        list = db.QueryBySQL<KhsKhbzModel>(sql, typeof(KhsKhbzModel));

                        if (WebKeys.EnableCaching)
                        {
                            //���ݴ��뻺��ϵͳ
                            CacheItemRefreshAction refreshAction =
                            new CacheItemRefreshAction(typeof(KhsKhbzData), false);
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
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhsKhbzModel model)
        {
            bool isResult = true;

            using (ORMDataAccess<KhsKhbzData> db = new ORMDataAccess<KhsKhbzData>())
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
		    //�����صĻ���
		    CacheHelper.RemoveCacheForClassKey(CacheClassKey);
		}
            return isResult;
        }

        #endregion
    }
}
