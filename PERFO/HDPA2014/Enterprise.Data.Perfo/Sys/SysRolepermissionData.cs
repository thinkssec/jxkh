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
    /// �ļ���:  SysRolepermissionData.cs
    /// ��������: ���ݲ�-��ɫȨ�ޱ����ݷ��ʷ���ʵ����
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:46
    /// </summary>
    public class SysRolepermissionData : ISysRolepermissionData
    {

        #region ����������
        /// <summary>
        /// ����������
        /// </summary>
        public static readonly string CacheClassKey = WebKeys.CacheItemKey + typeof(SysRolepermissionData).ToString();

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SysRolepermissionModel GetSingle(string key)
        {
            return null;
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<SysRolepermissionModel> GetList()
        {
            IList<SysRolepermissionModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<SysRolepermissionModel>)CacheHelper.GetCache(CacheClassKey + "_GetList");
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<SysRolepermissionData> db = new ORMDataAccess<SysRolepermissionData>())
                {
                    list = db.Query<SysRolepermissionModel>("from SysRolepermissionModel");

                    if (WebKeys.EnableCaching)
                    {
                        //���ݴ��뻺��ϵͳ
                        CacheHelper.Add(typeof(SysRolepermissionData), false, null, "GetList", null, CacheClassKey + "_GetList", list);
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
        public IList<SysRolepermissionModel> GetListByHQL(string hql)
        {
            IList<SysRolepermissionModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //list = (IList<SysRolepermissionModel>)CacheHelper.GetCache(CacheClassKey + "_GetListByHQL_" + hql);
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<SysRolepermissionData> db = new ORMDataAccess<SysRolepermissionData>())
                {
                    if (string.IsNullOrEmpty(hql))
                    {
                        list = db.Query<SysRolepermissionModel>("from SysRolepermissionModel");
                    }
                    else
                    {
                        list = db.Query<SysRolepermissionModel>(hql);
                    }

                    //if (WebKeys.EnableCaching)
                    //{
                    ////���ݴ��뻺��ϵͳ
                    //CacheHelper.Add(typeof(SysRolepermissionData), false, null, "GetListByHQL", new object[] { hql }, CacheClassKey + "_GetListByHQL_" + hql, list);
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
        public IList<SysRolepermissionModel> GetListBySQL(string sql)
        {
            IList<SysRolepermissionModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<SysRolepermissionModel>)CacheHelper.GetCache(CacheClassKey + "_GetListBySQL_" + sql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<SysRolepermissionData> db = new ORMDataAccess<SysRolepermissionData>())
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        list = db.QueryBySQL<SysRolepermissionModel>(sql, typeof(SysRolepermissionModel));

                        //if (WebKeys.EnableCaching)
                        //{
                        ////���ݴ��뻺��ϵͳ
                        //CacheHelper.Add(typeof(SysRolepermissionData), false, null, "GetListBySQL", new object[] { sql }, CacheClassKey + "_GetListBySQL_" + sql, list);		
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
        public bool Execute(SysRolepermissionModel model)
        {
            bool isResult = true;

            using (ORMDataAccess<SysRolepermissionData> db = new ORMDataAccess<SysRolepermissionData>())
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

            using (ORMDataAccess<SysRolepermissionData> db = new ORMDataAccess<SysRolepermissionData>())
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
