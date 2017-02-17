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
using Enterprise.Model.Perfo.Sys;

namespace Enterprise.Service.Perfo.Zbk
{

    /// <summary>
    /// �ļ���:  ZbkLhzbService.cs
    /// ��������: ҵ���߼���-����ָ��ά�����ݴ���
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:46
    /// </summary>
    public class ZbkLhzbService
    {
        #region ����������

        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly IZbkLhzbData dal = new ZbkLhzbData();

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ZbkLhzbModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<ZbkLhzbModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<ZbkLhzbModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<ZbkLhzbModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(ZbkLhzbModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region �Զ��巽��

        /// <summary>
        /// ���¼����е�ȫ��ָ������
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        public bool UpdateZbxhByList(List<ZbkLhzbModel> lst)
        {
            StringBuilder sqls = new StringBuilder();
            sqls.Append("begin ");
            foreach (var m in lst)
            {
                sqls.Append("update PERFO_ZBK_LHZB set ZBXH='" + m.ZBXH + "' where LHZBBM='" + m.LHZBBM + "';");
            }
            sqls.Append(" end;");
            return dal.ExecuteSQL(sqls.ToString());
        }

        /// <summary>
        /// ��ȡָ����λ�Ͱ汾�µ�������Ч״̬������ָ�꼯��
        /// </summary>
        /// <param name="jgbm">��������</param>
        /// <param name="bbmc">�汾����</param>
        /// <param name="isAll">��ʾȫ��</param>
        /// <returns></returns>
        public IList<ZbkLhzbModel> GetListForValid(string jgbm, string bbmc, bool isAll)
        {
            SysBmjgService bmjgSrv = new SysBmjgService();
            //string jgbms = bmjgSrv.GetJgbmsForSQL(jgbm);
            string s = ((!isAll) ? " p.SFJY='0' and " : "");
            string hql = "from ZbkLhzbModel p where " + s + " p.BBMC='" + bbmc + "'";
            if (!string.IsNullOrEmpty(jgbm))
            {
                hql = "select p from ZbkLhzbModel p,ZbkWcztbModel c where " + s + " p.BBMC='"
                    + bbmc + "' and p.LHZBBM=c.LHZBBM and c.JGBM in (" + jgbm + ")";
            }
            return GetListByHQL(hql).OrderBy(p => p.Zbxx.SXH).ToList();
        }

        public IList<ZbkLhzbModel> GetListForValid2(string jgbm, string bbmc, bool isAll)
        {
            SysBmjgService bmjgSrv = new SysBmjgService();
            //string jgbms = bmjgSrv.GetJgbmsForSQL(jgbm);
            string s = ((!isAll) ? " p.SFJY='0' and " : "");
            string sql = @"select * from PERFO_ZBK_LHZB p where " + s + " p.BBMC='" + bbmc + "' connect by prior lhzbbm = parentzbbm start with parentzbbm is null";
            if (!string.IsNullOrEmpty(jgbm))
            {
                sql = "select * from (select p.* from PERFO_ZBK_LHZB p,perfo_zbk_wcztb c where " + s + " p.BBMC='"
                    + bbmc + "' and p.LHZBBM=c.LHZBBM and c.JGBM in (" + jgbm + ") ) connect by prior lhzbbm = parentzbbm start with parentzbbm is null ";
            }
            return GetListBySQL(sql).ToList();
        }

        /// <summary>
        /// ��ȡָ����λ�Ͱ汾�µ�������Ч״̬������ָ�꼯��,������ָ���������
        /// </summary>
        /// <param name="jgbm">��������</param>
        /// <param name="bbmc">�汾����</param>
        /// <param name="isAll">��ʾȫ��</param>
        /// <returns></returns>
        public IList<ZbkLhzbModel> GetListForValidOrderByZbxh(string jgbm, string bbmc, bool isAll)
        {
            return GetListForValid2(jgbm, bbmc, isAll).ToList();
        }

        /// <summary>
        /// ɾ����ָ����ص���������Ϣ
        /// </summary>
        /// <param name="lhzbbm">����ָ�����</param>
        /// <returns></returns>
        public bool DeleteGljg(string lhzbbm)
        {
            string deleteSQL =
                @"begin
            delete from PERFO_ZBK_MBZTB where LHZBBM='" + lhzbbm + "';"
                + "delete from PERFO_ZBK_MBZSH where LHZBBM='" + lhzbbm + "';"
                + "delete from PERFO_ZBK_WCZTB where LHZBBM='" + lhzbbm + "';"
                + "delete from PERFO_ZBK_WCZSHDF where LHZBBM='" + lhzbbm + "'; end;";
            return dal.ExecuteSQL(deleteSQL);
        }

     
        #endregion

    }

}
