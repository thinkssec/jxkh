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
    /// �ļ���:  SysTablechangeData.cs
    /// ��������: ���ݲ�-���ݱ仯���ٱ����ݷ��ʷ���ʵ����
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:46
    /// </summary>
    public class SysTablechangeData : ISysTablechangeData
    {

        #region ����������
        /// <summary>
        /// ����������
        /// </summary>
        public static readonly string CacheClassKey = WebKeys.CacheItemKey + typeof(SysTablechangeData).ToString();

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SysTablechangeModel GetSingle(string key)
        {
            return null;
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<SysTablechangeModel> GetList()
        {
            IList<SysTablechangeModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //list = (IList<SysTablechangeModel>)CacheHelper.GetCache(CacheClassKey + "_GetList");
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<SysTablechangeData> db = new ORMDataAccess<SysTablechangeData>())
                {
                    list = db.Query<SysTablechangeModel>("from SysTablechangeModel");

                    //if (WebKeys.EnableCaching)
                    //{
                    ////���ݴ��뻺��ϵͳ
                    //CacheHelper.Add(typeof(SysTablechangeData), false, null, "GetList", null, CacheClassKey + "_GetList", list);
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
        public IList<SysTablechangeModel> GetListByHQL(string hql)
        {
            IList<SysTablechangeModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //list = (IList<SysTablechangeModel>)CacheHelper.GetCache(CacheClassKey + "_GetListByHQL_" + hql);
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<SysTablechangeData> db = new ORMDataAccess<SysTablechangeData>())
                {
                    if (string.IsNullOrEmpty(hql))
                    {
                        list = db.Query<SysTablechangeModel>("from SysTablechangeModel");
                    }
                    else
                    {
                        list = db.Query<SysTablechangeModel>(hql);
                    }

                    //if (WebKeys.EnableCaching)
                    //{
                    ////���ݴ��뻺��ϵͳ
                    //CacheHelper.Add(typeof(SysTablechangeData), false, null, "GetListByHQL", new object[] { hql }, CacheClassKey + "_GetListByHQL_" + hql, list);
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
        public IList<SysTablechangeModel> GetListBySQL(string sql)
        {
            IList<SysTablechangeModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<SysTablechangeModel>)CacheHelper.GetCache(CacheClassKey + "_GetListBySQL_" + sql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<SysTablechangeData> db = new ORMDataAccess<SysTablechangeData>())
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        list = db.QueryBySQL<SysTablechangeModel>(sql, typeof(SysTablechangeModel));

                        //if (WebKeys.EnableCaching)
                        //{
                        ////���ݴ��뻺��ϵͳ
                        //CacheHelper.Add(typeof(SysTablechangeData), false, null, "GetListBySQL", new object[] { sql }, CacheClassKey + "_GetListBySQL_" + sql, list);		
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
        public bool Execute(SysTablechangeModel model)
        {
            bool isResult = true;

            //using (ORMDataAccess<SysTablechangeData> db = new ORMDataAccess<SysTablechangeData>())
            //{
            //    if (model.DB_Option_Action == WebKeys.InsertAction)
            //    {
            //        db.Insert(model);
            //    }
            //    else if (model.DB_Option_Action == WebKeys.UpdateAction)
            //    {
            //        db.Update(model);
            //    }
            //    else if (model.DB_Option_Action == WebKeys.DeleteAction)
            //    {
            //        db.Delete(model);
            //    }
            //}

            //if (WebKeys.EnableCaching)
            //{
            //    //�����صĻ���
            //    CacheHelper.RemoveCacheForClassKey(CacheClassKey);
            //}
            return isResult;
        }

        #endregion
    }
}
