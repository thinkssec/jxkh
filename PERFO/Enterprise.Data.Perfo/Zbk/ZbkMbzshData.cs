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
    /// �ļ���:  ZbkMbzshData.cs
    /// ��������: ���ݲ�-Ŀ��ֵ������ݷ��ʷ���ʵ����
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:46
    /// </summary>
    public class ZbkMbzshData : IZbkMbzshData
    {
    
        #region ����������
	/// <summary>
        /// ����������
        /// </summary>
        public static readonly string CacheClassKey = WebKeys.CacheItemKey + typeof(ZbkMbzshData).ToString();

	/// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ZbkMbzshModel GetSingle(string key)
        {
            return null;
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<ZbkMbzshModel> GetList()
        {
            IList<ZbkMbzshModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
                //list = (IList<ZbkMbzshModel>)CacheHelper.GetCache(CacheClassKey + "_GetList");
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<ZbkMbzshData> db = new ORMDataAccess<ZbkMbzshData>())
                {
                    list = db.Query<ZbkMbzshModel>("from ZbkMbzshModel");
                    
		    //if (WebKeys.EnableCaching)
           	    //{
                    ////���ݴ��뻺��ϵͳ
		    //CacheHelper.Add(typeof(ZbkMbzshData), false, null, "GetList", null, CacheClassKey + "_GetList", list);
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
        public IList<ZbkMbzshModel> GetListByHQL(string hql)
        {
            IList<ZbkMbzshModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
                //list = (IList<ZbkMbzshModel>)CacheHelper.GetCache(CacheClassKey + "_GetListByHQL_" + hql);
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<ZbkMbzshData> db = new ORMDataAccess<ZbkMbzshData>())
                {
			if (string.IsNullOrEmpty(hql))
			{
				list = db.Query<ZbkMbzshModel>("from ZbkMbzshModel");
			}
			else
			{
				list = db.Query<ZbkMbzshModel>(hql);
			}

			    //if (WebKeys.EnableCaching)
	           	    //{
	                    ////���ݴ��뻺��ϵͳ
			    //CacheHelper.Add(typeof(ZbkMbzshData), false, null, "GetListByHQL", new object[] { hql }, CacheClassKey + "_GetListByHQL_" + hql, list);
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
        public IList<ZbkMbzshModel> GetListBySQL(string sql)
        {
            IList<ZbkMbzshModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<ZbkMbzshModel>)CacheHelper.GetCache(CacheClassKey + "_GetListBySQL_" + sql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<ZbkMbzshData> db = new ORMDataAccess<ZbkMbzshData>())
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        list = db.QueryBySQL<ZbkMbzshModel>(sql, typeof(ZbkMbzshModel));

                        //if (WebKeys.EnableCaching)
                        //{
                            ////���ݴ��뻺��ϵͳ
			    //CacheHelper.Add(typeof(ZbkMbzshData), false, null, "GetListBySQL", new object[] { sql }, CacheClassKey + "_GetListBySQL_" + sql, list);		
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
        public bool Execute(ZbkMbzshModel model)
        {
            bool isResult = true;

            using (ORMDataAccess<ZbkMbzshData> db = new ORMDataAccess<ZbkMbzshData>())
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
