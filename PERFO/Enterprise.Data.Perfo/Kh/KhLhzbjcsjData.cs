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
    /// �ļ���:  KhLhzbjcsjData.cs
    /// ��������: ���ݲ�-�������˻������ݱ����ݷ��ʷ���ʵ����
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:45
    /// </summary>
    public class KhLhzbjcsjData : IKhLhzbjcsjData
    {

        #region ����������
        /// <summary>
        /// ����������
        /// </summary>
        public static readonly string CacheClassKey = WebKeys.CacheItemKey + typeof(KhLhzbjcsjData).ToString();
        /// <summary>
        /// �������������
        /// </summary>
        public static readonly string[] CacheRelationshipKeys = new string[] { 
            Enterprise.Data.Perfo.Kh.KhLhzbjcsjData.CacheClassKey,
            Enterprise.Data.Perfo.Kh.KhKhglData.CacheClassKey
        };

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhLhzbjcsjModel GetSingle(string key)
        {
            string hql = "from KhLhzbjcsjModel p where p.JCZBID='" + key + "'";
            return GetListByHQL(hql).FirstOrDefault();
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhLhzbjcsjModel> GetList()
        {
            IList<KhLhzbjcsjModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //list = (IList<KhLhzbjcsjModel>)CacheHelper.GetCache(CacheClassKey + "_GetList");
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhLhzbjcsjData> db = new ORMDataAccess<KhLhzbjcsjData>())
                {
                    list = db.Query<KhLhzbjcsjModel>("from KhLhzbjcsjModel");

                    //if (WebKeys.EnableCaching)
                    //{
                    ////���ݴ��뻺��ϵͳ
                    //CacheHelper.Add(typeof(KhLhzbjcsjData), false, null, "GetList", null, CacheClassKey + "_GetList", list);
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
        public IList<KhLhzbjcsjModel> GetListByHQL(string hql)
        {
            IList<KhLhzbjcsjModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //list = (IList<KhLhzbjcsjModel>)CacheHelper.GetCache(CacheClassKey + "_GetListByHQL_" + hql);
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhLhzbjcsjData> db = new ORMDataAccess<KhLhzbjcsjData>())
                {
                    if (string.IsNullOrEmpty(hql))
                    {
                        list = db.Query<KhLhzbjcsjModel>("from KhLhzbjcsjModel");
                    }
                    else
                    {
                        list = db.Query<KhLhzbjcsjModel>(hql);
                    }

                    //if (WebKeys.EnableCaching)
                    //{
                    ////���ݴ��뻺��ϵͳ
                    //CacheHelper.Add(typeof(KhLhzbjcsjData), false, null, "GetListByHQL", new object[] { hql }, CacheClassKey + "_GetListByHQL_" + hql, list);
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
        public IList<KhLhzbjcsjModel> GetListBySQL(string sql)
        {
            IList<KhLhzbjcsjModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<KhLhzbjcsjModel>)CacheHelper.GetCache(CacheClassKey + "_GetListBySQL_" + sql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhLhzbjcsjData> db = new ORMDataAccess<KhLhzbjcsjData>())
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        list = db.QueryBySQL<KhLhzbjcsjModel>(sql, typeof(KhLhzbjcsjModel));

                        //if (WebKeys.EnableCaching)
                        //{
                        ////���ݴ��뻺��ϵͳ
                        //CacheHelper.Add(typeof(KhLhzbjcsjData), false, null, "GetListBySQL", new object[] { sql }, CacheClassKey + "_GetListBySQL_" + sql, list);		
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
        public bool Execute(KhLhzbjcsjModel model)
        {
            bool isResult = true;

            using (ORMDataAccess<KhLhzbjcsjData> db = new ORMDataAccess<KhLhzbjcsjData>())
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
