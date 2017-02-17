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
    /// �ļ���:  KhJgbmdfbData.cs
    /// ��������: ���ݲ�-���ز��Ŵ�ֱ����ݷ��ʷ���ʵ����
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:45
    /// </summary>
    public class KhJgbmdfbData : IKhJgbmdfbData
    {

        #region ����������
        /// <summary>
        /// ����������
        /// </summary>
        public static readonly string CacheClassKey = WebKeys.CacheItemKey + typeof(KhJgbmdfbData).ToString();
        /// <summary>
        /// �������������
        /// </summary>
        public static readonly string[] CacheRelationshipKeys = new string[] { 
            Enterprise.Data.Perfo.Kh.KhJgbmdfbData.CacheClassKey,
            Enterprise.Data.Perfo.Kh.KhDfzbmxData.CacheClassKey,
            Enterprise.Data.Perfo.Kh.KhDlzbmxData.CacheClassKey,
            Enterprise.Data.Perfo.Kh.KhKhglData.CacheClassKey
        };

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhJgbmdfbModel GetSingle(string key)
        {
            return null;
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhJgbmdfbModel> GetList()
        {
            IList<KhJgbmdfbModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //list = (IList<KhJgbmdfbModel>)CacheHelper.GetCache(CacheClassKey + "_GetList");
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhJgbmdfbData> db = new ORMDataAccess<KhJgbmdfbData>())
                {
                    list = db.Query<KhJgbmdfbModel>("from KhJgbmdfbModel");

                    //if (WebKeys.EnableCaching)
                    //{
                    ////���ݴ��뻺��ϵͳ
                    //CacheHelper.Add(typeof(KhJgbmdfbData), false, null, "GetList", null, CacheClassKey + "_GetList", list);
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
        public IList<KhJgbmdfbModel> GetListByHQL(string hql)
        {
            IList<KhJgbmdfbModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<KhJgbmdfbModel>)CacheHelper.GetCache(CacheClassKey + "_GetListByHQL_" + hql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhJgbmdfbData> db = new ORMDataAccess<KhJgbmdfbData>())
                {
                    if (string.IsNullOrEmpty(hql))
                    {
                        list = db.Query<KhJgbmdfbModel>("from KhJgbmdfbModel");
                    }
                    else
                    {
                        list = db.Query<KhJgbmdfbModel>(hql);
                    }

                    if (WebKeys.EnableCaching)
                    {
                        //���ݴ��뻺��ϵͳ
                        CacheHelper.Add(typeof(KhJgbmdfbData), false, null, "GetListByHQL", new object[] { hql }, CacheClassKey + "_GetListByHQL_" + hql, list);
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
        public IList<KhJgbmdfbModel> GetListBySQL(string sql)
        {
            IList<KhJgbmdfbModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<KhJgbmdfbModel>)CacheHelper.GetCache(CacheClassKey + "_GetListBySQL_" + sql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhJgbmdfbData> db = new ORMDataAccess<KhJgbmdfbData>())
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        list = db.QueryBySQL<KhJgbmdfbModel>(sql, typeof(KhJgbmdfbModel));

                        if (WebKeys.EnableCaching)
                        {
                            //���ݴ��뻺��ϵͳ
                            CacheHelper.Add(typeof(KhJgbmdfbData), false, null, "GetListBySQL", new object[] { sql }, CacheClassKey + "_GetListBySQL_" + sql, list);
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
        public bool Execute(KhJgbmdfbModel model)
        {
            bool isResult = true;

            using (ORMDataAccess<KhJgbmdfbData> db = new ORMDataAccess<KhJgbmdfbData>())
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
