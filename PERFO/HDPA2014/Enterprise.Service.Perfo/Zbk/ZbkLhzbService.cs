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
            foreach (var m in lst)
            {
                sqls.Append("update [PERFO_ZBK_LHZB] set ZBXH='" + m.ZBXH + "' where LHZBBM='" + m.LHZBBM + "';");
            }
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

        /// <summary>
        /// ��ȡָ����λ�Ͱ汾�µ�������Ч״̬������ָ�꼯��,������ָ���������
        /// </summary>
        /// <param name="jgbm">��������</param>
        /// <param name="bbmc">�汾����</param>
        /// <param name="isAll">��ʾȫ��</param>
        /// <returns></returns>
        public IList<ZbkLhzbModel> GetListForValidOrderByZbxh(string jgbm, string bbmc, bool isAll)
        {
            return GetListForValid(jgbm, bbmc, isAll).OrderBy(p => p.ZBXH).ToList();
        }

        /// <summary>
        /// ��ȡָ���汾�Ķ���ָ�꼯��
        /// </summary>
        /// <param name="bbmc"></param>
        /// <returns></returns>
        public IList<ZbkLhzbModel> GetListByBBMC(string bbmc)
        {
            string hql = "from ZbkLhzbModel p where p.BBMC='" + bbmc + "'";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// ɾ����ָ����ص���������Ϣ
        /// </summary>
        /// <param name="lhzbbm">����ָ�����</param>
        /// <returns></returns>
        public bool DeleteGljg(string lhzbbm)
        {
            string deleteSQL =
                "delete from [PERFO_ZBK_MBZTB] where LHZBBM='" + lhzbbm + "';"
                + "delete from [PERFO_ZBK_MBZSH] where LHZBBM='" + lhzbbm + "';"
                + "delete from [PERFO_ZBK_WCZTB] where LHZBBM='" + lhzbbm + "';"
                + "delete from [PERFO_ZBK_WCZSHDF] where LHZBBM='" + lhzbbm + "'";
            return dal.ExecuteSQL(deleteSQL);
        }

     
        #endregion

    }

}
