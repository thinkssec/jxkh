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
    /// �ļ���:  ZbkBdfjgData.cs
    /// ��������: ���ݲ�-����ֻ������ݷ��ʷ���ʵ����
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:46
    /// </summary>
    public class ZbkBdfjgData : IZbkBdfjgData
    {

        #region ����������
        /// <summary>
        /// ����������
        /// </summary>
        public static readonly string CacheClassKey = WebKeys.CacheItemKey + typeof(ZbkBdfjgData).ToString();

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ZbkBdfjgModel GetSingle(string key)
        {
            return null;
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<ZbkBdfjgModel> GetList()
        {
            IList<ZbkBdfjgModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<ZbkBdfjgModel>)CacheHelper.GetCache(CacheClassKey + "_GetList");
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<ZbkBdfjgData> db = new ORMDataAccess<ZbkBdfjgData>())
                {
                    list = db.Query<ZbkBdfjgModel>("from ZbkBdfjgModel");

                    if (WebKeys.EnableCaching)
                    {
                        //���ݴ��뻺��ϵͳ
                        CacheHelper.Add(typeof(ZbkBdfjgData), false, null, "GetList", null, CacheClassKey + "_GetList", list);
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
        public IList<ZbkBdfjgModel> GetListByHQL(string hql)
        {
            IList<ZbkBdfjgModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //list = (IList<ZbkBdfjgModel>)CacheHelper.GetCache(CacheClassKey + "_GetListByHQL_" + hql);
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<ZbkBdfjgData> db = new ORMDataAccess<ZbkBdfjgData>())
                {
                    if (string.IsNullOrEmpty(hql))
                    {
                        list = db.Query<ZbkBdfjgModel>("from ZbkBdfjgModel");
                    }
                    else
                    {
                        list = db.Query<ZbkBdfjgModel>(hql);
                    }

                    //if (WebKeys.EnableCaching)
                    //{
                    ////���ݴ��뻺��ϵͳ
                    //CacheHelper.Add(typeof(ZbkBdfjgData), false, null, "GetListByHQL", new object[] { hql }, CacheClassKey + "_GetListByHQL_" + hql, list);
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
        public IList<ZbkBdfjgModel> GetListBySQL(string sql)
        {
            IList<ZbkBdfjgModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<ZbkBdfjgModel>)CacheHelper.GetCache(CacheClassKey + "_GetListBySQL_" + sql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<ZbkBdfjgData> db = new ORMDataAccess<ZbkBdfjgData>())
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        list = db.QueryBySQL<ZbkBdfjgModel>(sql, typeof(ZbkBdfjgModel));

                        //if (WebKeys.EnableCaching)
                        //{
                        ////���ݴ��뻺��ϵͳ
                        //CacheHelper.Add(typeof(ZbkBdfjgData), false, null, "GetListBySQL", new object[] { sql }, CacheClassKey + "_GetListBySQL_" + sql, list);		
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
        public bool Execute(ZbkBdfjgModel model)
        {
            bool isResult = true;

            using (ORMDataAccess<ZbkBdfjgData> db = new ORMDataAccess<ZbkBdfjgData>())
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

            using (ORMDataAccess<ZbkBdfjgData> db = new ORMDataAccess<ZbkBdfjgData>())
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
