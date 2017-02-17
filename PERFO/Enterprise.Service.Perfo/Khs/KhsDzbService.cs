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
    /// �ļ���:  KhsDzbService.cs
    /// ��������: ҵ���߼���-���˴�ָ�����ݴ���
    /// �����ˣ�����������
	/// ����ʱ�� ��2015/11/4 20:47:10
    /// </summary>
    public class KhsDzbService
    {
        #region ����������
        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly IKhsDzbData dal = new KhsDzbData();

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhsDzbModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhsDzbModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

	/// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhsDzbModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhsDzbModel model)
        {
            return dal.Execute(model);
        }
        #endregion
    }

}
