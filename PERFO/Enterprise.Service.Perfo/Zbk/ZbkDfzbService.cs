using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Data.Perfo.Zbk;
using Enterprise.Model.Perfo.Zbk;
using Enterprise.Service.Perfo.Sys;

namespace Enterprise.Service.Perfo.Zbk
{
	
    /// <summary>
    /// �ļ���:  ZbkDfzbService.cs
    /// ��������: ҵ���߼���-���ָ��ά�����ݴ���
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:46
    /// </summary>
    public class ZbkDfzbService
    {
        #region ����������

        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly IZbkDfzbData dal = new ZbkDfzbData();

	/// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ZbkDfzbModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<ZbkDfzbModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<ZbkDfzbModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

	/// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<ZbkDfzbModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(ZbkDfzbModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region �Զ��巽��

        /// <summary>
        /// ����ָ�����ɾ�����ָ��
        /// </summary>
        /// <param name="dfzbbm"></param>
        /// <returns></returns>
        public bool DeleteDFZB(string dfzbbm)
        {
            string deleteSQL = 
                @"begin
            delete from PERFO_ZBK_BDFJG where DFZBBM='" + dfzbbm + "';"
                + "delete from PERFO_ZBK_DFZ where DFZBBM='" + dfzbbm + "';"
                + "delete from PERFO_ZBK_DFZB where DFZBBM='" + dfzbbm + "';end;";
            return dal.ExecuteSQL(deleteSQL);
        }

        /// <summary>
        /// ��ȡָ���汾�Ĵ��ָ�꼯��
        /// </summary>
        /// <param name="bbmc"></param>
        /// <returns></returns>
        public IList<ZbkDfzbModel> GetListByBBMC(string bbmc)
        {
            string hql = "from ZbkDfzbModel p where p.BBMC='" + bbmc + "'";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// ��ȡָ����λ�Ͱ汾�µ�������Ч״̬�Ĵ��ָ�꼯��
        /// </summary>
        /// <param name="jgbm">��������</param>
        /// <param name="bbmc">�汾����</param>
        /// <param name="isAll">��ʾȫ��</param>
        /// <returns></returns>
        public IList<ZbkDfzbModel> GetListForValid(string jgbm, string bbmc, bool isAll)
        {
            SysBmjgService bmjgSrv = new SysBmjgService();
            //string jgbms = bmjgSrv.GetJgbmsForSQL(jgbm);
            string s = ((!isAll) ? " p.DISABLE='0' and " : "");
            string hql = "from ZbkDfzbModel p where " + s + " p.BBMC='" + bbmc + "'";
            if (!string.IsNullOrEmpty(jgbm))
            {
                hql = "select p from ZbkDfzbModel p,ZbkBdfjgModel c where " + s + " p.BBMC='"
                    + bbmc + "' and p.DFZBBM=c.DFZBBM and c.JGBM in (" + jgbm + ")";
            }
            return GetListByHQL(hql).OrderBy(p => p.Zbxx.SXH).ToList();
        }

        #endregion

    }

}
