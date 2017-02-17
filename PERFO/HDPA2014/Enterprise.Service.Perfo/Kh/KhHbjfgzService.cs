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
    /// �ļ���:  KhHbjfgzService.cs
    /// ��������: ҵ���߼���-�ϲ��Ʒֹ�������ݴ���
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/12/2 13:41:05
    /// </summary>
    public class KhHbjfgzService
    {
        #region ����������

        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly IKhHbjfgzData dal = new KhHbjfgzData();

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhHbjfgzModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhHbjfgzModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhHbjfgzModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhHbjfgzModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhHbjfgzModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region �Զ��巽��

        /// <summary>
        /// ��ȡ�뿼�������Ӧ�ĺϲ��Ʒֹ�����Ϣ
        /// </summary>
        /// <param name="khid"></param>
        /// <returns></returns>
        public IList<KhHbjfgzModel> GetListByKhid(string khid)
        {
            string hql = "from KhHbjfgzModel p where p.KHID='" + khid + "'";
            return GetListByHQL(hql);
        }

        #endregion

    }

}
