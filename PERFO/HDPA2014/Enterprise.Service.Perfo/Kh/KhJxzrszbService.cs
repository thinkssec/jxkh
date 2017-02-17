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
    /// �ļ���:  KhJxzrszbService.cs
    /// ��������: ҵ���߼���-��ȼ�Ч������ָ�����ݴ���
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:45
    /// </summary>
    public class KhJxzrszbService
    {
        #region ����������

        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly IKhJxzrszbData dal = new KhJxzrszbData();

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhJxzrszbModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhJxzrszbModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhJxzrszbModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhJxzrszbModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhJxzrszbModel model)
        {
            return dal.Execute(model);
        }


        #endregion

        #region �Զ��巽����

        /// <summary>
        /// ����Ⱥ͵�λ�����ȡ�����鿼��ָ�꼯��
        /// </summary>
        /// <param name="niandu">���</param>
        /// <param name="jgbm">����</param>
        /// <returns></returns>
        public IList<KhJxzrszbModel> GetValidListBySearch(string niandu, string jgbm)
        {
            string hql = "from KhJxzrszbModel p where p.ZSZND='" + niandu + "' and p.ZJGBM='" + jgbm + "' order by p.BBMC desc, p.ZXSXH";
            var list = GetListByHQL(hql).Where(p=>p.Jxzrs.ZRSZT == "1").ToList();
            var bbmcs = list.Distinct<KhJxzrszbModel>(new FastPropertyComparer<KhJxzrszbModel>("BBMC"));
            if (bbmcs.Count() > 1)
            {
                list = list.Where(p => p.BBMC == bbmcs.First().BBMC).ToList();
            }
            return list.ToList();
        }

        /// <summary>
        /// ����Ⱥ͵�λ���롢ָ��汾��ȡ�����鿼��ָ�꼯��
        /// </summary>
        /// <param name="niandu">���</param>
        /// <param name="jgbm">����</param>
        /// <param name="bbmc">�汾����</param>
        /// <returns></returns>
        public IList<KhJxzrszbModel> GetListBySearch(string niandu, string jgbm, string bbmc)
        {
            string hql = "from KhJxzrszbModel p where p.ZSZND='" + niandu + "' and p.ZJGBM='" + jgbm + "' and p.BBMC='" + bbmc + "' order by p.ZXSXH";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// ����ȺͰ汾���ƻ�ȡ���м�Чָ��
        /// </summary>
        /// <param name="niandu">���</param>
        /// <param name="bbmc">�汾����</param>
        /// <returns></returns>
        public IList<KhJxzrszbModel> GetListByNdAndBbmc(string niandu, string bbmc)
        {
            SysBmjgService bmjgSrv = new SysBmjgService();
            string hql = "from KhJxzrszbModel p where p.ZSZND='" + niandu + "' and p.BBMC='" + bbmc + "' order by p.ZJGBM,p.ZXSXH";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// ɾ��������ָ���ɸѡָ��
        /// </summary>
        /// <param name="model">KhJxzrszbModel</param>
        /// <returns></returns>
        public bool DeleteJxzrsZbAndZbsx(KhJxzrszbModel model)
        {
            StringBuilder sqlSB = new StringBuilder();
            sqlSB.Append("delete from [PERFO_KH_ZBSX] where ZRSZBID='" + model.ZRSZBID + "';");
            sqlSB.Append("delete from [PERFO_KH_JXZRSZB] where ZRSZBID='" + model.ZRSZBID + "';");
            return dal.ExecuteSQL(sqlSB.ToString());
        }

        /// <summary>
        /// �������¼�Чָ���еĹ���ͬһ����ָ���Ȩ��ֵ
        /// </summary>
        /// <param name="model">KhJxzrszbModel</param>
        /// <returns></returns>
        public bool UpdateZbqzByLhzbbm(KhJxzrszbModel model)
        {
            StringBuilder sqlSB = new StringBuilder();
            sqlSB.Append("update [PERFO_KH_JXZRSZB] set ZZBQZ='" + model.ZZBQZ + "' where LHZBBM='" 
                + model.LHZBBM + "' and ZJGBM='" + model.ZJGBM + "' and BBMC='" + model.BBMC 
                + "' and ZSZND='" + model.ZSZND + "';");
            return dal.ExecuteSQL(sqlSB.ToString());
        }

        #endregion
    }

}
