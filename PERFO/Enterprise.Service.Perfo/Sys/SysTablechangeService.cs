using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Component.Cache;
using Enterprise.Data.Perfo.Sys;
using Enterprise.Model.Perfo.Sys;

namespace Enterprise.Service.Perfo.Sys
{

    /// <summary>
    /// �ļ���:  SysTablechangeService.cs
    /// ��������: ҵ���߼���-���ݱ仯���ٱ����ݴ���
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:46
    /// </summary>
    public class SysTablechangeService
    {
        #region ����������

        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly ISysTablechangeData dal = new SysTablechangeData();

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SysTablechangeModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<SysTablechangeModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<SysTablechangeModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<SysTablechangeModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(SysTablechangeModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region ��̬����

        /// <summary>
        /// �������ݱ���¼�¼���е�������Ϣ����
        /// </summary>
        /// <returns></returns>
        public static IList<SysTablechangeModel> GetAllTableChangeList()
        {
            return dal.GetList();
        }

        #endregion

    }

}
