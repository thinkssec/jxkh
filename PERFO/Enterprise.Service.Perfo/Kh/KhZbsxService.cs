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
    /// �ļ���:  KhZbsxService.cs
    /// ��������: ҵ���߼���-����ָ����ܱ����ݴ���
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:45
    /// </summary>
    public class KhZbsxService
    {
        #region ����������

        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly IKhZbsxData dal = new KhZbsxData();

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhZbsxModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhZbsxModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhZbsxModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhZbsxModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhZbsxModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region �Զ��巽��

        /// <summary>
        /// ���ݿ���ID��ȡ����ɸѡ���ݵļ���
        /// </summary>
        /// <param name="khid">����ID</param>
        /// <returns></returns>
        public IList<KhZbsxModel> GetListByKaohe(string khid)
        {
            string hql = "from KhZbsxModel p where p.KHID='"+khid+"' order by p.SXXH";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// ���ݿ���ID�ͻ��������ȡ����ɸѡ���ݵļ���
        /// </summary>
        /// <param name="khid">����ID</param>
        /// <param name="jgbm">��������</param>
        /// <returns></returns>
        public IList<KhZbsxModel> GetListByKaoheAndJgbm(string khid, string jgbm)
        {
            string hql = "from KhZbsxModel p where p.KHID='" + khid + "' and p.SXJGBM='" + jgbm + "' order by p.SXXH";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// ɾ��ָ�������µ�ָ��ɸѡ����
        /// </summary>
        /// <param name="khid">����ID</param>
        /// <param name="jgbm">��������</param>
        /// <returns></returns>
        public bool DeleteByKhidAndJgbm(string khid, string jgbm)
        {
            string sql = "delete from PERFO_KH_ZBSX where KHID='" + khid + "' and SXJGBM='" + jgbm + "'";
            return dal.ExecuteSQL(sql);
        }

        #endregion

    }

}
