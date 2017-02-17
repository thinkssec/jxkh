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
    /// �ļ���:  KhJxzrsData.cs
    /// ��������: ���ݲ�-����ָ�꿼�˱����ݷ��ʷ���ʵ����
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:45
    /// </summary>
    public class KhJxzrsData : IKhJxzrsData
    {

        #region ����������
        /// <summary>
        /// ����������
        /// </summary>
        public static readonly string CacheClassKey = WebKeys.CacheItemKey + typeof(KhJxzrsData).ToString();
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
        public KhJxzrsModel GetSingle(string key)
        {
            string hql = "from KhJxzrsModel p where p.ZRSID='" + key + "'";
            return GetListByHQL(hql).FirstOrDefault();
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhJxzrsModel> GetList()
        {
            IList<KhJxzrsModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //list = (IList<KhJxzrsModel>)CacheHelper.GetCache(CacheClassKey + "_GetList");
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhJxzrsData> db = new ORMDataAccess<KhJxzrsData>())
                {
                    list = db.Query<KhJxzrsModel>("from KhJxzrsModel");

                    //if (WebKeys.EnableCaching)
                    //{
                    ////���ݴ��뻺��ϵͳ
                    //CacheHelper.Add(typeof(KhJxzrsData), false, null, "GetList", null, CacheClassKey + "_GetList", list);
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
        public IList<KhJxzrsModel> GetListByHQL(string hql)
        {
            IList<KhJxzrsModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<KhJxzrsModel>)CacheHelper.GetCache(CacheClassKey + "_GetListByHQL_" + hql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhJxzrsData> db = new ORMDataAccess<KhJxzrsData>())
                {
                    if (string.IsNullOrEmpty(hql))
                    {
                        list = db.Query<KhJxzrsModel>("from KhJxzrsModel");
                    }
                    else
                    {
                        list = db.Query<KhJxzrsModel>(hql);
                    }

                    if (WebKeys.EnableCaching)
                    {
                        //���ݴ��뻺��ϵͳ
                        CacheHelper.Add(typeof(KhJxzrsData), false, null, "GetListByHQL", new object[] { hql }, CacheClassKey + "_GetListByHQL_" + hql, list);
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
        public IList<KhJxzrsModel> GetListBySQL(string sql)
        {
            IList<KhJxzrsModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //    list = (IList<KhJxzrsModel>)CacheHelper.GetCache(CacheClassKey + "_GetListBySQL_" + sql);
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhJxzrsData> db = new ORMDataAccess<KhJxzrsData>())
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        list = db.QueryBySQL<KhJxzrsModel>(sql, typeof(KhJxzrsModel));

                        //if (WebKeys.EnableCaching)
                        //{
                        ////���ݴ��뻺��ϵͳ
                        //CacheHelper.Add(typeof(KhJxzrsData), false, null, "GetListBySQL", new object[] { sql }, CacheClassKey + "_GetListBySQL_" + sql, list);		
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
        public bool Execute(KhJxzrsModel model)
        {
            bool isResult = true;

            using (ORMDataAccess<KhJxzrsData> db = new ORMDataAccess<KhJxzrsData>())
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

            using (ORMDataAccess<KhJxzrsData> db = new ORMDataAccess<KhJxzrsData>())
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
