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
    /// �ļ���:  SysRoleData.cs
    /// ��������: ���ݲ�-��ɫ�������ݷ��ʷ���ʵ����
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:46
    /// </summary>
    public class SysRoleData : ISysRoleData
    {

        #region ����������
        /// <summary>
        /// ����������
        /// </summary>
        public static readonly string CacheClassKey = WebKeys.CacheItemKey + typeof(SysRoleData).ToString();

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SysRoleModel GetSingle(string key)
        {
            //string hql = "from SysRoleModel p where p.ROLEID='" + key + "'";
            return GetList().FirstOrDefault(p => p.ROLEID == key);
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<SysRoleModel> GetList()
        {
            IList<SysRoleModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<SysRoleModel>)CacheHelper.GetCache(CacheClassKey + "_GetList");
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<SysRoleData> db = new ORMDataAccess<SysRoleData>())
                {
                    list = db.Query<SysRoleModel>("from SysRoleModel");

                    if (WebKeys.EnableCaching)
                    {
                        //���ݴ��뻺��ϵͳ
                        CacheHelper.Add(typeof(SysRoleData), false, null, "GetList", null, CacheClassKey + "_GetList", list);
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
        public IList<SysRoleModel> GetListByHQL(string hql)
        {
            IList<SysRoleModel> list = null;
            //if (WebKeys.EnableCaching)
            //{
            //list = (IList<SysRoleModel>)CacheHelper.GetCache(CacheClassKey + "_GetListByHQL_" + hql);
            //}

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<SysRoleData> db = new ORMDataAccess<SysRoleData>())
                {
                    if (string.IsNullOrEmpty(hql))
                    {
                        list = db.Query<SysRoleModel>("from SysRoleModel");
                    }
                    else
                    {
                        list = db.Query<SysRoleModel>(hql);
                    }

                    //if (WebKeys.EnableCaching)
                    //{
                    ////���ݴ��뻺��ϵͳ
                    //CacheHelper.Add(typeof(SysRoleData), false, null, "GetListByHQL", new object[] { hql }, CacheClassKey + "_GetListByHQL_" + hql, list);
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
        public IList<SysRoleModel> GetListBySQL(string sql)
        {
            IList<SysRoleModel> list = null;
            if (WebKeys.EnableCaching)
            {
                list = (IList<SysRoleModel>)CacheHelper.GetCache(CacheClassKey + "_GetListBySQL_" + sql);
            }

            if (list == null || list.Count == 0)
            {
                using (ORMDataAccess<SysRoleData> db = new ORMDataAccess<SysRoleData>())
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        list = db.QueryBySQL<SysRoleModel>(sql, typeof(SysRoleModel));

                        //if (WebKeys.EnableCaching)
                        //{
                        ////���ݴ��뻺��ϵͳ
                        //CacheHelper.Add(typeof(SysRoleData), false, null, "GetListBySQL", new object[] { sql }, CacheClassKey + "_GetListBySQL_" + sql, list);		
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
        public bool Execute(SysRoleModel model)
        {
            bool isResult = true;

            using (ORMDataAccess<SysRoleData> db = new ORMDataAccess<SysRoleData>())
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
