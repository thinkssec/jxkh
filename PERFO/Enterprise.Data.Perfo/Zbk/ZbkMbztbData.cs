using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.ORM;
using Enterprise.Component.Infrastructure;
using Enterprise.Component.Cache;
using Enterprise.Model.Perfo.Zbk;

namespace Enterprise.Data.Perfo.Zbk
{

    /// <summary>
    /// �ļ���:  ZbkMbztbData.cs
    /// ��������: ���ݲ�-Ŀ��ֵ����ݷ��ʷ���ʵ����
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:46
    /// </summary>
    public class ZbkMbztbData : IZbkMbztbData
    {
    
        #region ����������
	/// <summary>
        /// ����������
        /// </summary>
        public static readonly string CacheClassKey = WebKeys.CacheItemKey + typeof(ZbkMbztbData).ToString();

	/// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ZbkMbztbModel GetSingle(string key)
        {
            return null;
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<ZbkMbztbModel> GetList()
        {
            IList<ZbkMbztbModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
                //list = (IList<ZbkMbztbModel>)CacheHelper.GetCache(CacheClassKey + "_GetList");
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<ZbkMbztbData> db = new ORMDataAccess<ZbkMbztbData>())
                {
                    list = db.Query<ZbkMbztbModel>("from ZbkMbztbModel");
                    
		    //if (WebKeys.EnableCaching)
           	    //{
                    ////���ݴ��뻺��ϵͳ
		    //CacheHelper.Add(typeof(ZbkMbztbData), false, null, "GetList", null, CacheClassKey + "_GetList", list);
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
        public IList<ZbkMbztbModel> GetListByHQL(string hql)
        {
            IList<ZbkMbztbModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
                //list = (IList<ZbkMbztbModel>)CacheHelper.GetCache(CacheClassKey + "_GetListByHQL_" + hql);
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<ZbkMbztbData> db = new ORMDataAccess<ZbkMbztbData>())
                {
			if (string.IsNullOrEmpty(hql))
			{
				list = db.Query<ZbkMbztbModel>("from ZbkMbztbModel");
			}
			else
			{
				list = db.Query<ZbkMbztbModel>(hql);
			}

			    //if (WebKeys.EnableCaching)
	           	    //{
	                    ////���ݴ��뻺��ϵͳ
			    //CacheHelper.Add(typeof(ZbkMbztbData), false, null, "GetListByHQL", new object[] { hql }, CacheClassKey + "_GetListByHQL_" + hql, list);
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
        public IList<ZbkMbztbModel> GetListBySQL(string sql)
        {
            IList<ZbkMbztbModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<ZbkMbztbModel>)CacheHelper.GetCache(CacheClassKey + "_GetListBySQL_" + sql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<ZbkMbztbData> db = new ORMDataAccess<ZbkMbztbData>())
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        list = db.QueryBySQL<ZbkMbztbModel>(sql, typeof(ZbkMbztbModel));

                        //if (WebKeys.EnableCaching)
                        //{
                            ////���ݴ��뻺��ϵͳ
			    //CacheHelper.Add(typeof(ZbkMbztbData), false, null, "GetListBySQL", new object[] { sql }, CacheClassKey + "_GetListBySQL_" + sql, list);		
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
        public bool Execute(ZbkMbztbModel model)
        {
            bool isResult = true;

            using (ORMDataAccess<ZbkMbztbData> db = new ORMDataAccess<ZbkMbztbData>())
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
