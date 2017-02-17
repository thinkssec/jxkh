using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Data.Perfo.Kh;
using Enterprise.Model.Perfo.Kh;
using Enterprise.Service.Perfo.Sys;
using Enterprise.Model.Perfo.Sys;

namespace Enterprise.Service.Perfo.Kh
{

    /// <summary>
    /// �ļ���:  KhJxzrsService.cs
    /// ��������: ҵ���߼���-����ָ�꿼�˱����ݴ���
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:45
    /// </summary>
    public class KhJxzrsService
    {
        #region ����������

        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly IKhJxzrsData dal = new KhJxzrsData();

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhJxzrsModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhJxzrsModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhJxzrsModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhJxzrsModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhJxzrsModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region �Զ��巽����

        /// <summary>
        /// ɾ��ָ����λ�µ����м�Ч������ָ��
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool DeleteJxzrsData(KhJxzrsModel model)
        {
            StringBuilder sqls = new StringBuilder();
            sqls.Append("delete from [PERFO_KH_JXZRSZB] where ZRSID='" + model.ZRSID + "';");
            sqls.Append("delete from [PERFO_KH_JXZRS] where ZRSID='" + model.ZRSID + "';");
            return dal.ExecuteSQL(sqls.ToString());
        }

        /// <summary>
        /// ����Ⱥ͵�λ�����ȡ�����鼯��
        /// </summary>
        /// <param name="niandu">���</param>
        /// <param name="jgbm">����</param>
        /// <returns></returns>
        public IList<KhJxzrsModel> GetListByNdAndBmjg(string niandu, string jgbm)
        {
            SysBmjgService bmjgSrv = new SysBmjgService();
            string hql = "from KhJxzrsModel p where p.SZND='" + niandu + "' and p.JGBM in (" + bmjgSrv.GetJgbmsForSQL(jgbm) + ")";
            return GetListByHQL(hql).OrderBy(p=>p.KhKind.LXID).ThenBy(p=>p.Bmjg.XSXH).ToList();
        }

        /// <summary>
        /// ����ȡ���λ����Ϳ������ͻ�ȡ�����鼯��
        /// </summary>
        /// <param name="niandu">���</param>
        /// <param name="jgbm">����</param>
        /// <param name="khlx">��������</param>
        /// <returns></returns>
        public IList<KhJxzrsModel> GetListByNd_Bmjg_Khlx(string niandu, string jgbm, string khlx)
        {
            SysBmjgService bmjgSrv = new SysBmjgService();
            string hql = "from KhJxzrsModel p where p.SZND='" + niandu + "' and p.JGBM in (" + bmjgSrv.GetJgbmsForSQL(jgbm) + ") and p.LXID='" + khlx + "'";
            return GetListByHQL(hql).OrderBy(p => p.Bmjg.XSXH).ToList();
        }

        /// <summary>
        /// ����Ⱥ͵�λ�����ȡ��������
        /// </summary>
        /// <param name="niandu">���</param>
        /// <param name="jgbm">����</param>
        /// <returns></returns>
        public KhJxzrsModel GetModelByNdAndBmjg(string niandu, string jgbm)
        {
            SysBmjgService bmjgSrv = new SysBmjgService();
            string hql = "from KhJxzrsModel p where p.SZND='" + niandu + "' and p.JGBM='" + jgbm + "'";
            return GetListByHQL(hql).FirstOrDefault();
        }

        /// <summary>
        /// ��⵱ǰ�û��Ƿ�Ϊ��Ч������ֹܲ���
        /// </summary>
        /// <param name="niandu">���</param>
        /// <param name="jgbm">��������</param>
        /// <param name="user">��ǰ�û�</param>
        /// <returns></returns>
        public bool IsJxzrsFgbm(string niandu, string jgbm, SysUserModel user)
        {
            var model = GetModelByNdAndBmjg(niandu, jgbm);
            if (model != null)
            {
                return (model.FZBM == user.JGBM);
            }
            return false;
        }

        #endregion

    }
}
