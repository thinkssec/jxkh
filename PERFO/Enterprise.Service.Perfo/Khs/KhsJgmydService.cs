using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Data.Perfo.Khs;
using Enterprise.Model.Perfo.Khs;

namespace Enterprise.Service.Perfo.Khs
{
	
    /// <summary>
    /// �ļ���:  KhsJgmydService.cs
    /// ��������: ҵ���߼���-������������ݴ���
    /// �����ˣ�����������
	/// ����ʱ�� ��2015/11/8 22:00:44
    /// </summary>
    public class KhsJgmydService
    {
        #region ����������
        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly IKhsJgmydData dal = new KhsJgmydData();

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhsJgmydModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhsJgmydModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

	/// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhsJgmydModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhsJgmydModel model)
        {
            return dal.Execute(model);
        }
        #endregion
    }

}
