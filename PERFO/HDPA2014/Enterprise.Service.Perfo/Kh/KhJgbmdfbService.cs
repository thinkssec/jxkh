using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Data.Perfo.Kh;
using Enterprise.Model.Perfo.Kh;
using Enterprise.Model.Perfo.Sys;

namespace Enterprise.Service.Perfo.Kh
{

    /// <summary>
    /// �ļ���:  KhJgbmdfbService.cs
    /// ��������: ҵ���߼���-���ز��Ŵ�ֱ����ݴ���
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:45
    /// </summary>
    public class KhJgbmdfbService
    {

        #region ����������

        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly IKhJgbmdfbData dal = new KhJgbmdfbData();

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhJgbmdfbModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhJgbmdfbModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhJgbmdfbModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhJgbmdfbModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhJgbmdfbModel model)
        {
            return dal.Execute(model);
        }

        /// <summary>
        /// ִ��������ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public bool ExecuteByList(IList<KhJgbmdfbModel> models)
        {
            bool isOk = false;
            foreach (var m in models)
            {
                isOk = dal.Execute(m);
            }
            return isOk;
        }

        #endregion

        #region �Զ��巽����

        /// <summary>
        /// ��ȡָ���������µ�����ָ��Ĵ����Ϣ
        /// </summary>
        /// <param name="khid">������</param>
        /// <returns></returns>
        public IList<KhJgbmdfbModel> GetListByKhid(string khid)
        {
            string hql = "from KhJgbmdfbModel p where p.KHID='" + khid + "'";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// ��ȡָ���������µ�����ָ��Ĵ����Ϣ
        /// </summary>
        /// <param name="khid">������</param>
        /// <param name="jgbm">��������</param>
        /// <returns></returns>
        public IList<KhJgbmdfbModel> GetListByKhidAndJgbm(string khid, string jgbm)
        {
            string hql = "from KhJgbmdfbModel p where p.KHID='" + khid + "' and p.JGBM='" + jgbm + "'";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// ��ȡָ���������µ���������ָ��Ĵ����Ϣ
        /// </summary>
        /// <param name="khid">������</param>
        /// <returns></returns>
        public IList<KhJgbmdfbModel> GetDlzbListByKhid(string khid)
        {
            string hql = "from KhJgbmdfbModel p where p.KHID='" + khid + "' and p.ZBBM like 'LH%'";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// ��ȡָ���������µ����д��ָ��Ĵ����Ϣ
        /// </summary>
        /// <param name="khid">������</param>
        /// <returns></returns>
        public IList<KhJgbmdfbModel> GetDfzbListByKhid(string khid)
        {
            string hql = "from KhJgbmdfbModel p where p.KHID='" + khid + "' and p.ZBBM like 'DF%'";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// ��ȡָ�������ں�����˿ɴ�ֵ�ָ�꼯��
        /// </summary>
        /// <param name="khid">������</param>
        /// <param name="user">�û�MODEL</param>
        /// <returns></returns>
        public IList<KhJgbmdfbModel> GetListByKhidAndDfz(string khid, SysUserModel user)
        {
            string hql = "from KhJgbmdfbModel p where p.KHID='" + khid + "' and (p.DFZ='" + user.JGBM + "' or p.DFZ='" + user.LOGINID + "')";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// ��⵱ǰ�û��Ƿ���д��Ȩ��
        /// ��Ч����ԱĬ����Ȩ��
        /// </summary>
        /// <param name="khid">������</param>
        /// <param name="zbbm">ָ�����</param>
        /// <param name="user">�û�MODEL</param>
        /// <returns></returns>
        public bool CheckUserPermessionForDfzb(string khid, string zbbm, SysUserModel user)
        {
            var dfzbLst = GetListByKhidAndDfz(khid, user);
            return (dfzbLst.FirstOrDefault(p => p.ZBBM == zbbm) != null) || user.Role.ROLEID == "paadmin";
        }

        #endregion

        #region ��̬������

        /// <summary>
        /// ��⵱ǰ�û��Ƿ���Ȩ�޲���ָ���Ŀ����ںͻ���������
        /// </summary>
        /// <param name="khid">������</param>
        /// <param name="jgbm">�����˻���</param>
        /// <param name="user">��ǰ�û�</param>
        /// <returns></returns>
        public static bool IsDfzUser(string khid, int jgbm, SysUserModel user)
        {
            KhJgbmdfbService srv = new KhJgbmdfbService();
            var dfzbLst = srv.GetListByKhidAndDfz(khid, user);
            return (dfzbLst.FirstOrDefault(p => p.JGBM == jgbm) != null);
        }

        /// <summary>
        /// ��⵱ǰ�û��Ƿ����ָ������Ȩ��
        /// </summary>
        /// <param name="khid">������</param>
        /// <param name="khid">��������</param>
        /// <param name="zbbm">ָ�����</param>
        /// <param name="user">�û�MODEL</param>
        /// <returns></returns>
        public static bool IsDfzForJgbmAndZbbm(string khid,int jgbm, string zbbm, SysUserModel user)
        {
            string hql = "from KhJgbmdfbModel p where p.KHID='" + khid + "' and (p.DFZ='" + user.JGBM + "' or p.DFZ='" + user.LOGINID + "')";
            var dfzbLst = dal.GetListByHQL(hql);
            return (dfzbLst.FirstOrDefault(p =>p.JGBM == jgbm && p.ZBBM == zbbm) != null);
        }

        #endregion

    }

}
