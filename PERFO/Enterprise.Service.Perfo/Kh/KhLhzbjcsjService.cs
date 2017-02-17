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
    /// �ļ���:  KhLhzbjcsjService.cs
    /// ��������: ҵ���߼���-�������˻������ݱ����ݴ���
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:45
    /// </summary>
    public class KhLhzbjcsjService
    {
        #region ����������

        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly IKhLhzbjcsjData dal = new KhLhzbjcsjData();

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhLhzbjcsjModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhLhzbjcsjModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhLhzbjcsjModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhLhzbjcsjModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhLhzbjcsjModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        /// <summary>
        /// ִ��������ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public bool ExecuteByList(IList<KhLhzbjcsjModel> models)
        {
            bool isOk = false;
            foreach (var m in models)
            {
                isOk = dal.Execute(m);
            }
            return isOk;
        }

        /// <summary>
        /// ���ݿ����ںͻ��������ȡ����ָ��Ĺ�������ָ������
        /// </summary>
        /// <param name="khid">����ID</param>
        /// <param name="jgbm">��������</param>
        /// <returns></returns>
        public IList<KhLhzbjcsjModel> GetListByKhidAndJgbm(string khid, string jgbm)
        {
            string hql = "from KhLhzbjcsjModel p where p.KHID='" + khid + "' and p.JGBM='" + jgbm + "' order by p.JCZBID";
            return GetListByHQL(hql);
        }
    }

}
