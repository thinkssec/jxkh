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
    /// �ļ���:  ZbkDfzbData.cs
    /// ��������: ���ݲ�-���ָ��ά�����ݷ��ʷ���ʵ����
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:46
    /// </summary>
    public class ZbkDfzbData : IZbkDfzbData
    {

        #region ����������
        /// <summary>
        /// ����������
        /// </summary>
        public static readonly string CacheClassKey = WebKeys.CacheItemKey + typeof(ZbkDfzbData).ToString();

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ZbkDfzbModel GetSingle(string key)
        {
            string hql = "from ZbkDfzbModel p where p.DFZBBM='" + key + "'";
            return GetListByHQL(hql).FirstOrDefault();
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<ZbkDfzbModel> GetList()
        {
            IList<ZbkDfzbModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<ZbkDfzbModel>)CacheHelper.GetCache(CacheClassKey + "_GetList");
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<ZbkDfzbData> db = new ORMDataAccess<ZbkDfzbData>())
                {
                    list = db.Query<ZbkDfzbModel>("from ZbkDfzbModel");

                    if (WebKeys.EnableCaching)
                    {
                        //���ݴ��뻺��ϵͳ
                        CacheHelper.Add(typeof(ZbkDfzbData), false, null, "GetList", null, CacheClassKey + "_GetList", list);
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
        public IList<ZbkDfzbModel> GetListByHQL(string hql)
        {
            IList<ZbkDfzbModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<ZbkDfzbModel>)CacheHelper.GetCache(CacheClassKey + "_GetListByHQL_" + hql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<ZbkDfzbData> db = new ORMDataAccess<ZbkDfzbData>())
                {
                    if (string.IsNullOrEmpty(hql))
                    {
                        list = db.Query<ZbkDfzbModel>("from ZbkDfzbModel");
                    }
                    else
                    {
                        list = db.Query<ZbkDfzbModel>(hql);
                    }

                    if (WebKeys.EnableCaching)
                    {
                        //���ݴ��뻺��ϵͳ
                        CacheHelper.Add(typeof(ZbkDfzbData), false, null, "GetListByHQL", new object[] { hql }, CacheClassKey + "_GetListByHQL_" + hql, list);
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
        public IList<ZbkDfzbModel> GetListBySQL(string sql)
        {
            IList<ZbkDfzbModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<ZbkDfzbModel>)CacheHelper.GetCache(CacheClassKey + "_GetListBySQL_" + sql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<ZbkDfzbData> db = new ORMDataAccess<ZbkDfzbData>())
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        list = db.QueryBySQL<ZbkDfzbModel>(sql, typeof(ZbkDfzbModel));

                        //if (WebKeys.EnableCaching)
                        //{
                        ////���ݴ��뻺��ϵͳ
                        //CacheHelper.Add(typeof(ZbkDfzbData), false, null, "GetListBySQL", new object[] { sql }, CacheClassKey + "_GetListBySQL_" + sql, list);		
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
        public bool Execute(ZbkDfzbModel model)
        {
            bool isResult = true;

            using (ORMDataAccess<ZbkDfzbData> db = new ORMDataAccess<ZbkDfzbData>())
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

        /// <summary>
        /// ִ�л���SQL��ԭ������
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool ExecuteSQL(string sql)
        {
            bool isResult = true;

            using (ORMDataAccess<ZbkDfzbData> db = new ORMDataAccess<ZbkDfzbData>())
            {
                isResult = db.ExecuteNonQuery(sql);
            }

            if (WebKeys.EnableCaching)
            {
                //�����صĻ���
                CacheHelper.RemoveCacheForClassKey(CacheClassKey);
            }

            return isResult;
        }


    }
}
