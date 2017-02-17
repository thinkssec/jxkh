using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Data.Perfo.Khx;
using Enterprise.Model.Perfo.Khx;

namespace Enterprise.Service.Perfo.Khx
{
	
    /// <summary>
    /// �ļ���:  KhxResultService.cs
    /// ��������: ҵ���߼���-���˵÷����ݴ���
    /// �����ˣ�����������
	/// ����ʱ�� ��2015/11/12 19:14:36
    /// </summary>
    public class KhxResultService
    {
        #region ����������
        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly IKhxResultData dal = new KhxResultData();

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhxResultModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhxResultModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

	/// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhxResultModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhxResultModel model)
        {
            return dal.Execute(model);
        }
        #endregion
    }

}
