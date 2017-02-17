using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Component.ORM;
using Enterprise.Component.Infrastructure;
using Enterprise.Component.Cache;
using Enterprise.Model.Perfo.Sys;

namespace Enterprise.Data.Perfo.Sys
{

    /// <summary>
    /// �ļ���:  SysFgbmjgData.cs
    /// ��������: ���ݲ�-�û��ֹܻ������ݷ��ʷ���ʵ����
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:45
    /// </summary>
    public class SysFgbmjgData : ISysFgbmjgData
    {

        #region ����������
        /// <summary>
        /// ����������
        /// </summary>
        public static readonly string CacheClassKey = WebKeys.CacheItemKey + typeof(SysFgbmjgData).ToString();

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SysFgbmjgModel GetSingle(string key)
        {
            return null;
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<SysFgbmjgModel> GetList()
        {
            IList<SysFgbmjgModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //list = (IList<SysFgbmjgModel>)CacheHelper.GetCache(CacheClassKey + "_GetList");
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<SysFgbmjgData> db = new ORMDataAccess<SysFgbmjgData>())
                {
                    list = db.Query<SysFgbmjgModel>("from SysFgbmjgModel");

                    //if (WebKeys.EnableCaching)
                    //{
                    ////���ݴ��뻺��ϵͳ
                    //CacheHelper.Add(typeof(SysFgbmjgData), false, null, "GetList", null, CacheClassKey + "_GetList", list);
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
        public IList<SysFgbmjgModel> GetListByHQL(string hql)
        {
            IList<SysFgbmjgModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<SysFgbmjgModel>)CacheHelper.GetCache(CacheClassKey + "_GetListByHQL_" + hql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<SysFgbmjgData> db = new ORMDataAccess<SysFgbmjgData>())
                {
                    if (string.IsNullOrEmpty(hql))
                    {
                        list = db.Query<SysFgbmjgModel>("from SysFgbmjgModel");
                    }
                    else
                    {
                        list = db.Query<SysFgbmjgModel>(hql);
                    }

                    if (WebKeys.EnableCaching)
                    {
                        //���ݴ��뻺��ϵͳ
                        CacheHelper.Add(typeof(SysFgbmjgData), false, null, "GetListByHQL", new object[] { hql }, CacheClassKey + "_GetListByHQL_" + hql, list);
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
        public IList<SysFgbmjgModel> GetListBySQL(string sql)
        {
            IList<SysFgbmjgModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<SysFgbmjgModel>)CacheHelper.GetCache(CacheClassKey + "_GetListBySQL_" + sql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<SysFgbmjgData> db = new ORMDataAccess<SysFgbmjgData>())
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        list = db.QueryBySQL<SysFgbmjgModel>(sql, typeof(SysFgbmjgModel));

                        //if (WebKeys.EnableCaching)
                        //{
                        ////���ݴ��뻺��ϵͳ
                        //CacheHelper.Add(typeof(SysFgbmjgData), false, null, "GetListBySQL", new object[] { sql }, CacheClassKey + "_GetListBySQL_" + sql, list);		
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
        public bool Execute(SysFgbmjgModel model)
        {
            bool isResult = true;

            using (ORMDataAccess<SysFgbmjgData> db = new ORMDataAccess<SysFgbmjgData>())
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
