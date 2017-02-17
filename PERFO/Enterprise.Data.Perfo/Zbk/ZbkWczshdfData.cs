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
    /// �ļ���:  ZbkWczshdfData.cs
    /// ��������: ���ݲ�-���ֵ��˼���������ݷ��ʷ���ʵ����
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:46
    /// </summary>
    public class ZbkWczshdfData : IZbkWczshdfData
    {
    
        #region ����������
	/// <summary>
        /// ����������
        /// </summary>
        public static readonly string CacheClassKey = WebKeys.CacheItemKey + typeof(ZbkWczshdfData).ToString();

	/// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ZbkWczshdfModel GetSingle(string key)
        {
            return null;
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<ZbkWczshdfModel> GetList()
        {
            IList<ZbkWczshdfModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
                //list = (IList<ZbkWczshdfModel>)CacheHelper.GetCache(CacheClassKey + "_GetList");
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<ZbkWczshdfData> db = new ORMDataAccess<ZbkWczshdfData>())
                {
                    list = db.Query<ZbkWczshdfModel>("from ZbkWczshdfModel");
                    
		    //if (WebKeys.EnableCaching)
           	    //{
                    ////���ݴ��뻺��ϵͳ
		    //CacheHelper.Add(typeof(ZbkWczshdfData), false, null, "GetList", null, CacheClassKey + "_GetList", list);
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
        public IList<ZbkWczshdfModel> GetListByHQL(string hql)
        {
            IList<ZbkWczshdfModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
                //list = (IList<ZbkWczshdfModel>)CacheHelper.GetCache(CacheClassKey + "_GetListByHQL_" + hql);
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<ZbkWczshdfData> db = new ORMDataAccess<ZbkWczshdfData>())
                {
			if (string.IsNullOrEmpty(hql))
			{
				list = db.Query<ZbkWczshdfModel>("from ZbkWczshdfModel");
			}
			else
			{
				list = db.Query<ZbkWczshdfModel>(hql);
			}

			    //if (WebKeys.EnableCaching)
	           	    //{
	                    ////���ݴ��뻺��ϵͳ
			    //CacheHelper.Add(typeof(ZbkWczshdfData), false, null, "GetListByHQL", new object[] { hql }, CacheClassKey + "_GetListByHQL_" + hql, list);
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
        public IList<ZbkWczshdfModel> GetListBySQL(string sql)
        {
            IList<ZbkWczshdfModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<ZbkWczshdfModel>)CacheHelper.GetCache(CacheClassKey + "_GetListBySQL_" + sql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<ZbkWczshdfData> db = new ORMDataAccess<ZbkWczshdfData>())
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        list = db.QueryBySQL<ZbkWczshdfModel>(sql, typeof(ZbkWczshdfModel));

                        //if (WebKeys.EnableCaching)
                        //{
                            ////���ݴ��뻺��ϵͳ
			    //CacheHelper.Add(typeof(ZbkWczshdfData), false, null, "GetListBySQL", new object[] { sql }, CacheClassKey + "_GetListBySQL_" + sql, list);		
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
        public bool Execute(ZbkWczshdfModel model)
        {
            bool isResult = true;

            using (ORMDataAccess<ZbkWczshdfData> db = new ORMDataAccess<ZbkWczshdfData>())
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
