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
    /// �ļ���:  KhJgbmkhdfData.cs
    /// ��������: ���ݲ�-���ز��ſ��˵÷ֱ����ݷ��ʷ���ʵ����
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/28 16:45:02
    /// </summary>
    public class KhJgbmkhdfData : IKhJgbmkhdfData
    {

        #region ����������
        /// <summary>
        /// ����������
        /// </summary>
        public static readonly string CacheClassKey = WebKeys.CacheItemKey + typeof(KhJgbmkhdfData).ToString();
        /// <summary>
        /// �������������
        /// </summary>
        public static readonly string[] CacheRelationshipKeys = new string[] { 
            Enterprise.Data.Perfo.Kh.KhJgbmkhdfData.CacheClassKey,
            Enterprise.Data.Perfo.Kh.KhKhglData.CacheClassKey
        };

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhJgbmkhdfModel GetSingle(string key)
        {
            return null;
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhJgbmkhdfModel> GetList()
        {
            IList<KhJgbmkhdfModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //list = (IList<KhJgbmkhdfModel>)CacheHelper.GetCache(CacheClassKey + "_GetList");
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhJgbmkhdfData> db = new ORMDataAccess<KhJgbmkhdfData>())
                {
                    list = db.Query<KhJgbmkhdfModel>("from KhJgbmkhdfModel");

                    //if (WebKeys.EnableCaching)
                    //{
                    ////���ݴ��뻺��ϵͳ
                    //CacheHelper.Add(typeof(KhJgbmkhdfData), false, null, "GetList", null, CacheClassKey + "_GetList", list);
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
        public IList<KhJgbmkhdfModel> GetListByHQL(string hql)
        {
            IList<KhJgbmkhdfModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<KhJgbmkhdfModel>)CacheHelper.GetCache(CacheClassKey + "_GetListByHQL_" + hql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhJgbmkhdfData> db = new ORMDataAccess<KhJgbmkhdfData>())
                {
                    if (string.IsNullOrEmpty(hql))
                    {
                        list = db.Query<KhJgbmkhdfModel>("from KhJgbmkhdfModel");
                    }
                    else
                    {
                        list = db.Query<KhJgbmkhdfModel>(hql);
                    }

                    if (WebKeys.EnableCaching)
                    {
                        //���ݴ��뻺��ϵͳ
                        CacheHelper.Add(typeof(KhJgbmkhdfData), false, null, "GetListByHQL", new object[] { hql }, CacheClassKey + "_GetListByHQL_" + hql, list);
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
        public IList<KhJgbmkhdfModel> GetListBySQL(string sql)
        {
            IList<KhJgbmkhdfModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //    list = (IList<KhJgbmkhdfModel>)CacheHelper.GetCache(CacheClassKey + "_GetListBySQL_" + sql);
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhJgbmkhdfData> db = new ORMDataAccess<KhJgbmkhdfData>())
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        list = db.QueryBySQL<KhJgbmkhdfModel>(sql, typeof(KhJgbmkhdfModel));

                        //if (WebKeys.EnableCaching)
                        //{
                        ////���ݴ��뻺��ϵͳ
                        //CacheHelper.Add(typeof(KhJgbmkhdfData), false, null, "GetListBySQL", new object[] { sql }, CacheClassKey + "_GetListBySQL_" + sql, list);		
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
        public bool Execute(KhJgbmkhdfModel model)
        {
            bool isResult = true;

            using (ORMDataAccess<KhJgbmkhdfData> db = new ORMDataAccess<KhJgbmkhdfData>())
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
