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
    /// �ļ���:  SysBmjgData.cs
    /// ��������: ���ݲ�-�����������ݷ��ʷ���ʵ����
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:45
    /// </summary>
    public class SysBmjgData : ISysBmjgData
    {

        #region ����������
        /// <summary>
        /// ����������
        /// </summary>
        public static readonly string CacheClassKey = WebKeys.CacheItemKey + typeof(SysBmjgData).ToString();

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SysBmjgModel GetSingle(string key)
        {
            return GetList().FirstOrDefault(p=>p.JGBM == key.ToInt());
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<SysBmjgModel> GetList()
        {
            IList<SysBmjgModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<SysBmjgModel>)CacheHelper.GetCache(CacheClassKey + "_GetList");
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<SysBmjgData> db = new ORMDataAccess<SysBmjgData>())
                {
                    list = db.Query<SysBmjgModel>("from SysBmjgModel");

                    if (WebKeys.EnableCaching)
                    {
                        //���ݴ��뻺��ϵͳ
                        CacheHelper.Add(typeof(SysBmjgData), false, null, "GetList", null, CacheClassKey + "_GetList", list);
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
        public IList<SysBmjgModel> GetListByHQL(string hql)
        {
            IList<SysBmjgModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //list = (IList<SysBmjgModel>)CacheHelper.GetCache(CacheClassKey + "_GetListByHQL_" + hql);
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<SysBmjgData> db = new ORMDataAccess<SysBmjgData>())
                {
                    if (string.IsNullOrEmpty(hql))
                    {
                        list = db.Query<SysBmjgModel>("from SysBmjgModel");
                    }
                    else
                    {
                        list = db.Query<SysBmjgModel>(hql);
                    }

                    //if (WebKeys.EnableCaching)
                    //{
                    ////���ݴ��뻺��ϵͳ
                    //CacheHelper.Add(typeof(SysBmjgData), false, null, "GetListByHQL", new object[] { hql }, CacheClassKey + "_GetListByHQL_" + hql, list);
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
        public IList<SysBmjgModel> GetListBySQL(string sql)
        {
            IList<SysBmjgModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<SysBmjgModel>)CacheHelper.GetCache(CacheClassKey + "_GetListBySQL_" + sql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<SysBmjgData> db = new ORMDataAccess<SysBmjgData>())
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        list = db.QueryBySQL<SysBmjgModel>(sql, typeof(SysBmjgModel));

                        //if (WebKeys.EnableCaching)
                        //{
                        ////���ݴ��뻺��ϵͳ
                        //CacheHelper.Add(typeof(SysBmjgData), false, null, "GetListBySQL", new object[] { sql }, CacheClassKey + "_GetListBySQL_" + sql, list);		
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
        public bool Execute(SysBmjgModel model)
        {
            bool isResult = true;

            using (ORMDataAccess<SysBmjgData> db = new ORMDataAccess<SysBmjgData>())
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
                else if (model.DB_Option_Action == WebKeys.InsertOrUpdateAction)
                {
                    db.InsertOrUpdate(model);
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
