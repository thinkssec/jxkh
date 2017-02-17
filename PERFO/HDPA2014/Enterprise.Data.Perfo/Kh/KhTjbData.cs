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
    /// �ļ���:  KhTjbData.cs
    /// ��������: ���ݲ�-����ͳ�Ʊ����ݷ��ʷ���ʵ����
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:45
    /// </summary>
    public class KhTjbData : IKhTjbData
    {
    
        #region ����������
	/// <summary>
        /// ����������
        /// </summary>
        public static readonly string CacheClassKey = WebKeys.CacheItemKey + typeof(KhTjbData).ToString();

	/// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhTjbModel GetSingle(string key)
        {
            return null;
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhTjbModel> GetList()
        {
            IList<KhTjbModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
                //list = (IList<KhTjbModel>)CacheHelper.GetCache(CacheClassKey + "_GetList");
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhTjbData> db = new ORMDataAccess<KhTjbData>())
                {
                    list = db.Query<KhTjbModel>("from KhTjbModel");
                    
		    //if (WebKeys.EnableCaching)
           	    //{
                    ////���ݴ��뻺��ϵͳ
		    //CacheHelper.Add(typeof(KhTjbData), false, null, "GetList", null, CacheClassKey + "_GetList", list);
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
        public IList<KhTjbModel> GetListByHQL(string hql)
        {
            IList<KhTjbModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
                //list = (IList<KhTjbModel>)CacheHelper.GetCache(CacheClassKey + "_GetListByHQL_" + hql);
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhTjbData> db = new ORMDataAccess<KhTjbData>())
                {
			if (string.IsNullOrEmpty(hql))
			{
				list = db.Query<KhTjbModel>("from KhTjbModel");
			}
			else
			{
				list = db.Query<KhTjbModel>(hql);
			}

			    //if (WebKeys.EnableCaching)
	           	    //{
	                    ////���ݴ��뻺��ϵͳ
			    //CacheHelper.Add(typeof(KhTjbData), false, null, "GetListByHQL", new object[] { hql }, CacheClassKey + "_GetListByHQL_" + hql, list);
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
        public IList<KhTjbModel> GetListBySQL(string sql)
        {
            IList<KhTjbModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<KhTjbModel>)CacheHelper.GetCache(CacheClassKey + "_GetListBySQL_" + sql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhTjbData> db = new ORMDataAccess<KhTjbData>())
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        list = db.QueryBySQL<KhTjbModel>(sql, typeof(KhTjbModel));

                        //if (WebKeys.EnableCaching)
                        //{
                            ////���ݴ��뻺��ϵͳ
			    //CacheHelper.Add(typeof(KhTjbData), false, null, "GetListBySQL", new object[] { sql }, CacheClassKey + "_GetListBySQL_" + sql, list);		
                        //}
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
        public bool Execute(KhTjbModel model)
        {
            bool isResult = true;

            using (ORMDataAccess<KhTjbData> db = new ORMDataAccess<KhTjbData>())
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
