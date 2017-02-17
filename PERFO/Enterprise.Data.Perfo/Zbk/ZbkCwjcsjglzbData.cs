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
    /// �ļ���:  ZbkCwjcsjglzbData.cs
    /// ��������: ���ݲ�-����ָ�������������ݶ�Ӧ�����ݷ��ʷ���ʵ����
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/12/14 9:19:20
    /// </summary>
    public class ZbkCwjcsjglzbData : IZbkCwjcsjglzbData
    {

        #region ����������
        /// <summary>
        /// ����������
        /// </summary>
        public static readonly string CacheClassKey = WebKeys.CacheItemKey + typeof(ZbkCwjcsjglzbData).ToString();
        /// <summary>
        /// �������������
        /// </summary>
        public static readonly string[] CacheRelationshipKeys = new string[] { Enterprise.Data.Perfo.Zbk.ZbkCwjcsjglzbData.CacheClassKey };

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ZbkCwjcsjglzbModel GetSingle(string key)
        {
            string hql = "from ZbkCwjcsjglzbModel p where p.ID='" + key + "'";
            return GetListByHQL(hql).FirstOrDefault();
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<ZbkCwjcsjglzbModel> GetList()
        {
            IList<ZbkCwjcsjglzbModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<ZbkCwjcsjglzbModel>)CacheHelper.GetCache(CacheClassKey + "_GetList");
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<ZbkCwjcsjglzbData> db = new ORMDataAccess<ZbkCwjcsjglzbData>())
                {
                    list = db.Query<ZbkCwjcsjglzbModel>("from ZbkCwjcsjglzbModel");

                    if (WebKeys.EnableCaching)
                    {
                        //���ݴ��뻺��ϵͳ
                        CacheHelper.Add(typeof(ZbkCwjcsjglzbData), false, null, "GetList", null, CacheClassKey + "_GetList", list);
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
        public IList<ZbkCwjcsjglzbModel> GetListByHQL(string hql)
        {
            IList<ZbkCwjcsjglzbModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<ZbkCwjcsjglzbModel>)CacheHelper.GetCache(CacheClassKey + "_GetListByHQL_" + hql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<ZbkCwjcsjglzbData> db = new ORMDataAccess<ZbkCwjcsjglzbData>())
                {
                    if (string.IsNullOrEmpty(hql))
                    {
                        list = db.Query<ZbkCwjcsjglzbModel>("from ZbkCwjcsjglzbModel");
                    }
                    else
                    {
                        list = db.Query<ZbkCwjcsjglzbModel>(hql);
                    }

                    if (WebKeys.EnableCaching)
                    {
                        //���ݴ��뻺��ϵͳ
                        CacheHelper.Add(typeof(ZbkCwjcsjglzbData), false, null, "GetListByHQL", new object[] { hql }, CacheClassKey + "_GetListByHQL_" + hql, list);
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
        public IList<ZbkCwjcsjglzbModel> GetListBySQL(string sql)
        {
            IList<ZbkCwjcsjglzbModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //    list = (IList<ZbkCwjcsjglzbModel>)CacheHelper.GetCache(CacheClassKey + "_GetListBySQL_" + sql);
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<ZbkCwjcsjglzbData> db = new ORMDataAccess<ZbkCwjcsjglzbData>())
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        list = db.QueryBySQL<ZbkCwjcsjglzbModel>(sql, typeof(ZbkCwjcsjglzbModel));

                        //if (WebKeys.EnableCaching)
                        //{
                        ////���ݴ��뻺��ϵͳ
                        //CacheHelper.Add(typeof(ZbkCwjcsjglzbData), false, null, "GetListBySQL", new object[] { sql }, CacheClassKey + "_GetListBySQL_" + sql, list);		
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
        public bool Execute(ZbkCwjcsjglzbModel model)
        {
            bool isResult = true;

            using (ORMDataAccess<ZbkCwjcsjglzbData> db = new ORMDataAccess<ZbkCwjcsjglzbData>())
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


        /// <summary>
        /// ִ�л���SQL��ԭ������
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool ExecuteSQL(string sql)
        {
            bool isResult = true;

            using (ORMDataAccess<ZbkCwjcsjglzbData> db = new ORMDataAccess<ZbkCwjcsjglzbData>())
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

        #endregion
    }
}
