using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Data.Perfo.Kh;
using Enterprise.Model.Perfo.Kh;

namespace Enterprise.Service.Perfo.Kh
{
	
    /// <summary>
    /// �ļ���:  KhSjsbService.cs
    /// ��������: ҵ���߼���-�����ϱ����ݴ���
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:45
    /// </summary>
    public class KhSjsbService
    {

        #region ����������

        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly IKhSjsbData dal = new KhSjsbData();

	    /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhSjsbModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhSjsbModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhSjsbModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

	    /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhSjsbModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhSjsbModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region �Զ��巽����

        /// <summary>
        /// ��ȡָ�������ڵ����ݼ���
        /// </summary>
        /// <param name="khid">������</param>
        /// <returns></returns>
        public IList<KhSjsbModel> GetListByKhid(string khid)
        {
            string hql = "from KhSjsbModel p where p.KHID='" + khid + "'";
            return GetListByHQL(hql).OrderBy(p=>p.Bmjg.BZ).ToList();
        }

        /// <summary>
        /// ��ȡָ����������µ����в��뵥λ��ȷ��Ψһ��
        /// </summary>
        /// <param name="maxKhnd"></param>
        /// <returns></returns>
        public IList<KhSjsbModel> GetSjsbListByKhnd(int maxKhnd)
        {
            string hql = "select p from KhSjsbModel p, KhKhglModel c where p.KHID=c.KHID and c.KHND='" + maxKhnd + "'";
            var lst = GetListByHQL(hql).DistinctBy(p => p.JGBM).ToList();
            return lst;
        }

        #endregion

    }

}
