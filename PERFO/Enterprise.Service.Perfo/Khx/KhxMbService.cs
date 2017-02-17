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
    /// �ļ���:  KhxMbService.cs
    /// ��������: ҵ���߼���-����ģ�����ݴ���
    /// �����ˣ�����������
	/// ����ʱ�� ��2015/11/5 20:46:51
    /// </summary>
    public class KhxMbService
    {
        #region ����������
        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly IKhxMbData dal = new KhxMbData();

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhxMbModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhxMbModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

	/// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhxMbModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhxMbModel model)
        {
            return dal.Execute(model);
        }
        #endregion
    }

}
