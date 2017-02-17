using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Component.MVC;
using Enterprise.Data.Perfo.Sys;
using Enterprise.Model.Perfo.Sys;

namespace Enterprise.Service.Perfo.Sys
{

    /// <summary>
    /// �ļ���:  SysModuleService.cs
    /// ��������: ҵ���߼���-����ģ������ݴ���
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:45
    /// </summary>
    public class SysModuleService
    {
        #region ����������

        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly ISysModuleData dal = new SysModuleData();

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SysModuleModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<SysModuleModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<SysModuleModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<SysModuleModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(SysModuleModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region �Զ��巽��

        /// <summary>
        /// ���ݱ���ǰ׺��ȡ������
        /// ������Ӳ���
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public int GetModuleAmount(string parentId)
        {
            string hql =
                "from SysModuleModel p where p.MPARENTID='" + parentId + "'";
            var q = dal.GetListByHQL(hql);
            int amount = 0;
            if (q != null)
            {
                amount = q.Count;
            }
            return amount;
        }

        #endregion

        #region ·���������

        /// <summary>
        /// ��������ģ���·��������Ϣ
        /// </summary>
        /// <returns></returns>
        public static List<UrlMapPageRoute> LoadUrlRoute()
        {
            List<UrlMapPageRoute> routeList = new List<UrlMapPageRoute>();
            var moduleList = dal.GetList();
            string urlPrefix = "~/";
            foreach (var m in moduleList)
            {
                if (!string.IsNullOrEmpty(m.WEBURL.Trim()) && !string.IsNullOrEmpty(m.MURL.Trim()))
                {
                    UrlMapPageRoute route = new UrlMapPageRoute(
                        m.MID, m.WEBURL, urlPrefix + m.MURL.TrimStart(urlPrefix.ToCharArray()));
                    routeList.Add(route);
                }
            }
            return routeList;
        }

        #endregion

    }

}
