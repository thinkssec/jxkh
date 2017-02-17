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
    /// �ļ���:  KhDfzbmxData.cs
    /// ��������: ���ݲ�-���ָ�꿼�˱����ݷ��ʷ���ʵ����
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:45
    /// </summary>
    public class KhDfzbmxData : IKhDfzbmxData
    {

        #region ����������
        /// <summary>
        /// ����������
        /// </summary>
        public static readonly string CacheClassKey = WebKeys.CacheItemKey + typeof(KhDfzbmxData).ToString();
        /// <summary>
        /// �������������
        /// </summary>
        public static readonly string[] CacheRelationshipKeys = new string[] { 
            Enterprise.Data.Perfo.Kh.KhDfzbmxData.CacheClassKey,
            Enterprise.Data.Perfo.Kh.KhKhglData.CacheClassKey
        };

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhDfzbmxModel GetSingle(string key)
        {
            string hql = "from KhDfzbmxModel p where p.DFZBID='" + key + "'";
            return GetListByHQL(hql).FirstOrDefault();
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhDfzbmxModel> GetList()
        {
            IList<KhDfzbmxModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //list = (IList<KhDfzbmxModel>)CacheHelper.GetCache(CacheClassKey + "_GetList");
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhDfzbmxData> db = new ORMDataAccess<KhDfzbmxData>())
                {
                    list = db.Query<KhDfzbmxModel>("from KhDfzbmxModel");

                    //if (WebKeys.EnableCaching)
                    //{
                    ////���ݴ��뻺��ϵͳ
                    //CacheHelper.Add(typeof(KhDfzbmxData), false, null, "GetList", null, CacheClassKey + "_GetList", list);
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
        public IList<KhDfzbmxModel> GetListByHQL(string hql)
        {
            IList<KhDfzbmxModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<KhDfzbmxModel>)CacheHelper.GetCache(CacheClassKey + "_GetListByHQL_" + hql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhDfzbmxData> db = new ORMDataAccess<KhDfzbmxData>())
                {
                    if (string.IsNullOrEmpty(hql))
                    {
                        list = db.Query<KhDfzbmxModel>("from KhDfzbmxModel");
                    }
                    else
                    {
                        list = db.Query<KhDfzbmxModel>(hql);
                    }

                    if (WebKeys.EnableCaching)
                    {
                        //���ݴ��뻺��ϵͳ
                        CacheHelper.Add(typeof(KhDfzbmxData), false, null, "GetListByHQL", new object[] { hql }, CacheClassKey + "_GetListByHQL_" + hql, list);
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
        public IList<KhDfzbmxModel> GetListBySQL(string sql)
        {
            IList<KhDfzbmxModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<KhDfzbmxModel>)CacheHelper.GetCache(CacheClassKey + "_GetListBySQL_" + sql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhDfzbmxData> db = new ORMDataAccess<KhDfzbmxData>())
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        list = db.QueryBySQL<KhDfzbmxModel>(sql, typeof(KhDfzbmxModel));

                        if (WebKeys.EnableCaching)
                        {
                            //���ݴ��뻺��ϵͳ
                            CacheHelper.Add(typeof(KhDfzbmxData), false, null, "GetListBySQL", new object[] { sql }, CacheClassKey + "_GetListBySQL_" + sql, list);
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
        public bool Execute(KhDfzbmxModel model)
        {
            bool isResult = true;

            using (ORMDataAccess<KhDfzbmxData> db = new ORMDataAccess<KhDfzbmxData>())
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
