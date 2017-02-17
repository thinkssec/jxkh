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
    /// �ļ���:  KhSigninService.cs
    /// ��������: ҵ���߼���-֪ͨǩ�ձ����ݴ���
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:45
    /// </summary>
    public class KhSigninService
    {
        #region ����������

        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly IKhSigninData dal = new KhSigninData();

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhSigninModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhSigninModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhSigninModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhSigninModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhSigninModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region

        /// <summary>
        /// ��ȡ֪ͨID��Ӧ��ǩ����Ϣ
        /// </summary>
        /// <param name="tzid">֪ͨID</param>
        /// <returns></returns>
        public IList<KhSigninModel> GetListByTZID(string tzid)
        {
            string hql = "from KhSigninModel p where p.TZID='" + tzid + "'";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// ���ǩ�ղ���
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SignInTongzhi(KhSigninModel model)
        {
            string hql = "from KhSigninModel p where p.TZID='" + model.TZID + "' and p.QSR='" + model.QSR + "'";
            var q = GetListByHQL(hql).FirstOrDefault();
            if (q == null)
            {
                return Execute(model);
            }
            return true;
        }

        #endregion

    }

}
