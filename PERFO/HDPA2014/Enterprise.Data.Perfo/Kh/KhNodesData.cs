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
    /// �ļ���:  KhNodesData.cs
    /// ��������: ���ݲ�-���˽ڵ�����ݷ��ʷ���ʵ����
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:45
    /// </summary>
    public class KhNodesData : IKhNodesData
    {

        #region ����������
        /// <summary>
        /// ����������
        /// </summary>
        public static readonly string CacheClassKey = WebKeys.CacheItemKey + typeof(KhNodesData).ToString();

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhNodesModel GetSingle(string key)
        {
            return null;
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhNodesModel> GetList()
        {
            IList<KhNodesModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<KhNodesModel>)CacheHelper.GetCache(CacheClassKey + "_GetList");
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhNodesData> db = new ORMDataAccess<KhNodesData>())
                {
                    list = db.Query<KhNodesModel>("from KhNodesModel");

                    if (WebKeys.EnableCaching)
                    {
                        //���ݴ��뻺��ϵͳ
                        CacheHelper.Add(typeof(KhNodesData), false, null, "GetList", null, CacheClassKey + "_GetList", list);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// ���������������ݼ���
        /// </summary>
        /// <param name="hql">HQL</param>
        /// <returns></returns>
        public IList<KhNodesModel> GetListByHQL(string hql)
        {
            IList<KhNodesModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //list = (IList<KhNodesModel>)CacheHelper.GetCache(CacheClassKey + "_GetListByHQL_" + hql);
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhNodesData> db = new ORMDataAccess<KhNodesData>())
                {
                    if (string.IsNullOrEmpty(hql))
                    {
                        list = db.Query<KhNodesModel>("from KhNodesModel");
                    }
                    else
                    {
                        list = db.Query<KhNodesModel>(hql);
                    }

                    //if (WebKeys.EnableCaching)
                    //{
                    ////���ݴ��뻺��ϵͳ
                    //CacheHelper.Add(typeof(KhNodesData), false, null, "GetListByHQL", new object[] { hql }, CacheClassKey + "_GetListByHQL_" + hql, list);
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
        public IList<KhNodesModel> GetListBySQL(string sql)
        {
            IList<KhNodesModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //    list = (IList<KhNodesModel>)CacheHelper.GetCache(CacheClassKey + "_GetListBySQL_" + sql);
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<KhNodesData> db = new ORMDataAccess<KhNodesData>())
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        list = db.QueryBySQL<KhNodesModel>(sql, typeof(KhNodesModel));

                        //if (WebKeys.EnableCaching)
                        //{
                        ////���ݴ��뻺��ϵͳ
                        //CacheHelper.Add(typeof(KhNodesData), false, null, "GetListBySQL", new object[] { sql }, CacheClassKey + "_GetListBySQL_" + sql, list);		
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
        public bool Execute(KhNodesModel model)
        {
            bool isResult = true;

            using (ORMDataAccess<KhNodesData> db = new ORMDataAccess<KhNodesData>())
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
