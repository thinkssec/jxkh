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
    /// �ļ���:  KhUnlockService.cs
    /// ��������: ҵ���߼���-���ݽ������ݴ���
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:45
    /// </summary>
    public class KhUnlockService
    {
        #region ����������

        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly IKhUnlockData dal = new KhUnlockData();

	    /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhUnlockModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhUnlockModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhUnlockModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

	    /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhUnlockModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhUnlockModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region �Զ��巽����

        /// <summary>
        /// ��ȡָ�������ڵ����������ύ��λ
        /// </summary>
        /// <param name="khid"></param>
        /// <returns></returns>
        public IList<KhUnlockModel> GetListByKhid(string khid)
        {
            string hql = "from KhUnlockModel p where p.KHID='" + khid + "'";
            return GetListByHQL(hql).OrderBy(p => p.Bmjg.XSXH).ToList();
        }

        /// <summary>
        /// ɾ��ָ�������ڵ�����
        /// </summary>
        /// <param name="khid"></param>
        /// <returns></returns>
        public bool DeleteByKhid(string khid)
        {
            string sql = "delete from [PERFO_KH_UNLOCK] where KHID='" + khid + "'";
            return dal.ExecuteSQL(sql);
        }

        #endregion

    }

}
