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
    /// �ļ���:  KhCjqjService.cs
    /// ��������: ҵ���߼���-�ɼ������������ݴ���
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:45
    /// </summary>
    public class KhCjqjService
    {
        #region ����������

        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly IKhCjqjData dal = new KhCjqjData();

	    /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhCjqjModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhCjqjModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhCjqjModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

	    /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhCjqjModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhCjqjModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region �Զ��巽��

        /// <summary>
        /// ��ȡ�뿼���ڶ�Ӧ�ĳɼ�����ֲ�
        /// </summary>
        /// <param name="khid">����ID</param>
        /// <returns></returns>
        public IList<KhCjqjModel> GetListByKhid(string khid)
        {
            string hql = "from KhCjqjModel p where p.KHID='" + khid + "' order by p.QJDJ";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// ��ȡ�뿼���ڶ�Ӧ�ĳɼ�����ֲ���ά���ã�
        /// </summary>
        /// <param name="khid">����ID</param>
        /// <returns></returns>
        public IList<KhCjqjModel> GetListByKhidForEdit(string khid)
        {
            string hql = "from KhCjqjModel p where p.KHID='" + khid + "' order by p.QJDJ";
            var lst = GetListByHQL(hql);
            List<KhCjqjModel> cjqjLst = new List<KhCjqjModel>();
            cjqjLst.AddRange(lst);
            //����׷��һ��
            KhCjqjModel model = new KhCjqjModel();
            model.KHID = khid.ToInt();
            model.QJDJ = "";
            cjqjLst.Add(model);
            return cjqjLst;
        }

        #endregion
    }

}
