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
    /// �ļ���:  KhSjsbData.cs
    /// ��������: ���ݲ�-�����ϱ����ݷ��ʷ���ʵ����
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:45
    /// </summary>
    public class KhSjsbData : IKhSjsbData
    {

        #region ����������

        /// <summary>
        /// ����������
        /// </summary>
        public static readonly string CacheClassKey = WebKeys.CacheItemKey + typeof(KhSjsbData).ToString();
        /// <summary>
        /// �������������
        /// </summary>
        public static readonly string[] CacheRelationshipKeys = new string[] { 
            Enterprise.Data.Perfo.Kh.KhSjsbData.CacheClassKey,
            Enterprise.Data.Perfo.Kh.KhKhglData.CacheClassKey
        };

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhSjsbModel GetSingle(string key)
        {
            return null;
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhSjsbModel> GetList()
        {
            IList<KhSjsbModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //list = (IList<KhSjsbModel>)CacheHelper.GetCache(CacheClassKey + "_GetList");
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhSjsbData> db = new ORMDataAccess<KhSjsbData>())
                {
                    list = db.Query<KhSjsbModel>("from KhSjsbModel");

                    //if (WebKeys.EnableCaching)
                    //{
                    ////���ݴ��뻺��ϵͳ
                    //CacheHelper.Add(typeof(KhSjsbData), false, null, "GetList", null, CacheClassKey + "_GetList", list);
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
        public IList<KhSjsbModel> GetListByHQL(string hql)
        {
            IList<KhSjsbModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //list = (IList<KhSjsbModel>)CacheHelper.GetCache(CacheClassKey + "_GetListByHQL_" + hql);
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhSjsbData> db = new ORMDataAccess<KhSjsbData>())
                {
                    if (string.IsNullOrEmpty(hql))
                    {
                        list = db.Query<KhSjsbModel>("from KhSjsbModel");
                    }
                    else
                    {
                        list = db.Query<KhSjsbModel>(hql);
                    }

                    //if (WebKeys.EnableCaching)
                    //{
                    ////���ݴ��뻺��ϵͳ
                    //CacheHelper.Add(typeof(KhSjsbData), false, null, "GetListByHQL", new object[] { hql }, CacheClassKey + "_GetListByHQL_" + hql, list);
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
        public IList<KhSjsbModel> GetListBySQL(string sql)
        {
            IList<KhSjsbModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<KhSjsbModel>)CacheHelper.GetCache(CacheClassKey + "_GetListBySQL_" + sql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhSjsbData> db = new ORMDataAccess<KhSjsbData>())
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        list = db.QueryBySQL<KhSjsbModel>(sql, typeof(KhSjsbModel));

                        //if (WebKeys.EnableCaching)
                        //{
                        ////���ݴ��뻺��ϵͳ
                        //CacheHelper.Add(typeof(KhSjsbData), false, null, "GetListBySQL", new object[] { sql }, CacheClassKey + "_GetListBySQL_" + sql, list);		
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
        public bool Execute(KhSjsbModel model)
        {
            bool isResult = true;

            using (ORMDataAccess<KhSjsbData> db = new ORMDataAccess<KhSjsbData>())
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
    }
}
