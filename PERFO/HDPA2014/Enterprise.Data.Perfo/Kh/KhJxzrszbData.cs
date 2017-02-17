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
    /// �ļ���:  KhJxzrszbData.cs
    /// ��������: ���ݲ�-��ȼ�Ч������ָ�����ݷ��ʷ���ʵ����
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:45
    /// </summary>
    public class KhJxzrszbData : IKhJxzrszbData
    {

        #region ����������
        /// <summary>
        /// ����������
        /// </summary>
        public static readonly string CacheClassKey = WebKeys.CacheItemKey + typeof(KhJxzrszbData).ToString();
        /// <summary>
        /// �������������
        /// </summary>
        public static readonly string[] CacheRelationshipKeys = new string[] { 
            Enterprise.Data.Perfo.Kh.KhJxzrszbData.CacheClassKey,
            Enterprise.Data.Perfo.Kh.KhJxzrsData.CacheClassKey,
            Enterprise.Data.Perfo.Kh.KhZbsxData.CacheClassKey
        };

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhJxzrszbModel GetSingle(string key)
        {
            string hql = "from KhJxzrszbModel p where p.ZRSZBID='" + key + "'";
            return GetListByHQL(hql).FirstOrDefault();
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhJxzrszbModel> GetList()
        {
            IList<KhJxzrszbModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //list = (IList<KhJxzrszbModel>)CacheHelper.GetCache(CacheClassKey + "_GetList");
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhJxzrszbData> db = new ORMDataAccess<KhJxzrszbData>())
                {
                    list = db.Query<KhJxzrszbModel>("from KhJxzrszbModel");

                    //if (WebKeys.EnableCaching)
                    //{
                    ////���ݴ��뻺��ϵͳ
                    //CacheHelper.Add(typeof(KhJxzrszbData), false, null, "GetList", null, CacheClassKey + "_GetList", list);
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
        public IList<KhJxzrszbModel> GetListByHQL(string hql)
        {
            IList<KhJxzrszbModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<KhJxzrszbModel>)CacheHelper.GetCache(CacheClassKey + "_GetListByHQL_" + hql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhJxzrszbData> db = new ORMDataAccess<KhJxzrszbData>())
                {
                    if (string.IsNullOrEmpty(hql))
                    {
                        list = db.Query<KhJxzrszbModel>("from KhJxzrszbModel");
                    }
                    else
                    {
                        list = db.Query<KhJxzrszbModel>(hql);
                    }

                    if (WebKeys.EnableCaching)
                    {
                        //���ݴ��뻺��ϵͳ
                        CacheHelper.Add(typeof(KhJxzrszbData), false, null, "GetListByHQL", new object[] { hql }, CacheClassKey + "_GetListByHQL_" + hql, list);
                    }
                }
            }
            return list;
        }


        /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhJxzrszbModel> GetListBySQL(string sql)
        {
            IList<KhJxzrszbModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<KhJxzrszbModel>)CacheHelper.GetCache(CacheClassKey + "_GetListBySQL_" + sql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhJxzrszbData> db = new ORMDataAccess<KhJxzrszbData>())
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        list = db.QueryBySQL<KhJxzrszbModel>(sql, typeof(KhJxzrszbModel));

                        if (WebKeys.EnableCaching)
                        {
                            //���ݴ��뻺��ϵͳ
                            CacheHelper.Add(typeof(KhJxzrszbData), false, null, "GetListBySQL", new object[] { sql }, CacheClassKey + "_GetListBySQL_" + sql, list);
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
        public bool Execute(KhJxzrszbModel model)
        {
            bool isResult = true;

            using (ORMDataAccess<KhJxzrszbData> db = new ORMDataAccess<KhJxzrszbData>())
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
                else if (model.DB_Option_Action == WebKeys.InsertOrUpdateAction)
                {
                    db.InsertOrUpdate(model);
                }
            }

            if (WebKeys.EnableCaching)
            {
                //�����صĻ���
                CacheHelper.RemoveCacheForClassKeys(CacheRelationshipKeys);
            }
            return isResult;
        }

        #endregion

        /// <summary>
        /// ִ�л���SQL��ԭ������
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool ExecuteSQL(string sql)
        {
            bool isResult = true;

            using (ORMDataAccess<KhJxzrszbData> db = new ORMDataAccess<KhJxzrszbData>())
            {
                isResult = db.ExecuteNonQuery(sql);
            }

            if (WebKeys.EnableCaching)
            {
                //�����صĻ���
                CacheHelper.RemoveCacheForClassKeys(CacheRelationshipKeys);
            }

            return isResult;
        }
    }
}
