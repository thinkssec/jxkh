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
    /// �ļ���:  KhNdxsService.cs
    /// ��������: ҵ���߼���-��Ӫ�Ѷ�ϵ�����ݴ���
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:45
    /// </summary>
    public class KhNdxsService
    {
        #region ����������

        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly IKhNdxsData dal = new KhNdxsData();

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhNdxsModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhNdxsModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhNdxsModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhNdxsModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhNdxsModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region �Զ��巽����

        /// <summary>
        /// ��ȡָ�������ڵ����е�λ���Ѷ�ϵ������
        /// </summary>
        /// <param name="khid"></param>
        /// <returns></returns>
        public IList<KhNdxsModel> GetListByKhid(string khid)
        {
            string hql = "from KhNdxsModel p where p.KHID='" + khid + "'";
            return GetListByHQL(hql).OrderBy(p=>p.Bmjg.XSXH).ToList();
        }

        /// <summary>
        /// ��ȡָ�������ں͵�λ���Ѷ�ϵ��
        /// </summary>
        /// <param name="khid">����ID</param>
        /// <param name="jgbm">��������</param>
        /// <returns></returns>
        public decimal GetNdxsByKhidAndJgbm(string khid, int jgbm)
        {
            var q = GetListByKhid(khid).FirstOrDefault(p=>p.JGBM == jgbm);
            return (q != null) ? q.NDXS.ToDecimal() : 1.0M;
        }

        /// <summary>
        /// ����Ѷ�ϵ�����ݵĳ�ʼ��
        /// </summary>
        /// <param name="kaohe">�����ڶ���</param>
        /// <param name="jxzrsList">��Ч�����鼯��</param>
        /// <returns></returns>
        public IList<KhNdxsModel> InitKhNdxsByKaoheAndJxzrs(KhKhglModel kaohe, IList<KhJxzrsModel> jxzrsList)
        {
            foreach (var zrs in jxzrsList)
            {
                KhNdxsModel model = new KhNdxsModel();
                model.KHID = kaohe.KHID;
                model.JGBM = zrs.JGBM.Value;
                model.NDXS = 1.0M;
                model.DB_Option_Action = WebKeys.InsertOrUpdateAction;
                Execute(model);
            }
            return GetListByKhid(kaohe.KHID.ToString());
        }

        #endregion
    }

}
