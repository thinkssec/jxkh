using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Data.Perfo.Sys;
using Enterprise.Model.Perfo.Sys;

namespace Enterprise.Service.Perfo.Sys
{

    /// <summary>
    /// �ļ���:  SysVisitlogService.cs
    /// ��������: ҵ���߼���-������־�����ݴ���
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:46
    /// </summary>
    public class SysVisitlogService
    {
        #region ����������

        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly ISysVisitlogData dal = new SysVisitlogData();

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SysVisitlogModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<SysVisitlogModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<SysVisitlogModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<SysVisitlogModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(SysVisitlogModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        /// <summary>
        /// ִ�л���SQL��ԭ������
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool ExecuteSQL(string sql)
        {
            return dal.ExecuteSQL(sql);
        }

        /// <summary>
        /// ��ȡ��ǰ�û������һ�ε�¼��Ϣ
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public SysVisitlogModel GetLastVisitInfo(string userName)
        {
            string hql = "from SysVisitlogModel p where p.USERNAME='" + userName + "' order by p.OPERATIONDATE desc";
            SysVisitlogModel model = GetListByHQL(hql).FirstOrDefault();
            if (model == null)
            {
                model = new SysVisitlogModel();
            }
            return model;
        }

    }

}
