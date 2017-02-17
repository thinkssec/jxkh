using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Data.Perfo.Zbk;
using Enterprise.Model.Perfo.Zbk;

namespace Enterprise.Service.Perfo.Zbk
{
	
    /// <summary>
    /// �ļ���:  ZbkZbxxService.cs
    /// ��������: ҵ���߼���-ָ��������ݴ���
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:46
    /// </summary>
    public class ZbkZbxxService
    {
        #region ����������

        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly IZbkZbxxData dal = new ZbkZbxxData();

	/// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ZbkZbxxModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<ZbkZbxxModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<ZbkZbxxModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

	    /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<ZbkZbxxModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(ZbkZbxxModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region �Զ��巽��

        /// <summary>
        /// ��ȡָ�������µĴ�����Ч״̬��ָ�꼯��
        /// </summary>
        /// <param name="zblx"></param>
        /// <returns></returns>
        public IList<ZbkZbxxModel> GetListForValid(string zblx)
        {
            return dal.GetListByHQL("from ZbkZbxxModel p where p.ZBLX='" + zblx + "' and p.ZBZT='1' order by p.SXH");
        }

        /// <summary>
        /// ��ȡָ�������µ�����ָ�꼯��
        /// </summary>
        /// <param name="zblx">ָ������</param>
        /// <param name="yjzbmc">һ������</param>
        /// <returns></returns>
        public IList<ZbkZbxxModel> GetListByZblxAndYjzbmc(string zblx,string yjzbmc)
        {
            if (!string.IsNullOrEmpty(zblx) && !string.IsNullOrEmpty(yjzbmc))
            {
                return dal.GetListByHQL("from ZbkZbxxModel p where p.ZBLX='" + zblx + "' and p.YJZBMC='" + yjzbmc + "' order by p.SXH");
            }
            else if (!string.IsNullOrEmpty(zblx))
            {
                return dal.GetListByHQL("from ZbkZbxxModel p where p.ZBLX='" + zblx + "' order by p.SXH");
            }
            else if (!string.IsNullOrEmpty(yjzbmc))
            {
                return dal.GetListByHQL("from ZbkZbxxModel p where p.YJZBMC='" + yjzbmc + "' order by p.SXH");
            }
            else
            {
                return dal.GetList();
            }
        }

        #endregion
    }

}
