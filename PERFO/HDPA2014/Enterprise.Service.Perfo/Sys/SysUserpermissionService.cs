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
    /// �ļ���:  SysUserpermissionService.cs
    /// ��������: ҵ���߼���-���ݴ���
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/12/1 9:32:19
    /// </summary>
    public class SysUserpermissionService
    {
        #region ����������

        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly ISysUserpermissionData dal = new SysUserpermissionData();

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SysUserpermissionModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<SysUserpermissionModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<SysUserpermissionModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<SysUserpermissionModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(SysUserpermissionModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region �Զ��巽��

        /// <summary>
        /// ɾ��ָ���û��˺ŵ�����Ȩ��
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public bool DeleteUserPmByLoginId(string loginId)
        {
            string sql = "delete from [PERFO_SYS_USERPERMISSION] where LOGINID='" + loginId + "';";
            return dal.ExecuteSQL(sql);
        }

        #endregion
    }

}
