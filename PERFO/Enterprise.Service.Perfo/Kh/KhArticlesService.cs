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
    /// �ļ���:  KhArticlesService.cs
    /// ��������: ҵ���߼���-֪ͨ�������ݴ���
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:29:31
    /// </summary>
    public class KhArticlesService
    {
        #region ����������

        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly IKhArticlesData dal = new KhArticlesData();

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhArticlesModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhArticlesModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhArticlesModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhArticlesModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhArticlesModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region �Զ��巽��

        /// <summary>
        /// ��ȡָ������µ�����֪ͨ������Ϣ
        /// </summary>
        /// <param name="yy"></param>
        /// <returns></returns>
        public IList<KhArticlesModel> GetTzListByYear(string yy)
        {
            List<KhArticlesModel> list = new List<KhArticlesModel>();
            string hql = "from KhArticlesModel p ";
            if (!string.IsNullOrEmpty(yy))
            {
                hql += " where year(p.TJRQ)='" + yy + "' ";
            }
            hql += " order by p.TJRQ desc";
            list = GetListByHQL(hql) as List<KhArticlesModel>;
            if (list.Count == 0) 
            {
                var q = GetList().FirstOrDefault();
                if (q != null)
                    list.Add(q);
            }
            return list;
        }

        /// <summary>
        /// ɾ��ָ����֪ͨ��¼
        /// </summary>
        /// <param name="tzid">ID</param>
        /// <returns></returns>
        public bool DeleteArticleById(string tzid)
        {
            string sql = "delete from [PERFO_KH_SIGNIN] where TZID='" + tzid
                + "';delete from [PERFO_KH_ARTICLES] where TZID='" + tzid + "';";
            return dal.ExecuteSQL(sql);
        }

        #endregion

    }

}
