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
using Enterprise.Service.Perfo.Sys;

namespace Enterprise.Service.Perfo.Kh
{

    /// <summary>
    /// �ļ���:  KhMessageService.cs
    /// ��������: ҵ���߼���-���������ݴ���
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:45
    /// </summary>
    public class KhMessageService
    {

        SysUserService userSrv = new SysUserService();

        #region ����������

        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly IKhMessageData dal = new KhMessageData();

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhMessageModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhMessageModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhMessageModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhMessageModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhMessageModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region �Զ��巽����

        /// <summary>
        /// ��ȡ�뿼����ص����д�����Ϣ
        /// </summary>
        /// <param name="khid">����ID</param>
        /// <returns></returns>
        public IList<KhMessageModel> GetMessagesByKhid(string khid)
        {
            string hql = "from KhMessageModel p where p.KHID='" + khid + "' order by p.MSGID desc";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// ��ȡ�뿼�˺��û���¼����ص����д�����Ϣ
        /// </summary>
        /// <param name="khid">����ID</param>
        /// <param name="loginId">��¼ID</param>
        /// <returns></returns>
        public IList<KhMessageModel> GetMessagesByKhidAndLoginId(string khid, string loginId)
        {
            var userMessages = GetMessagesByKhid(khid).Where(p => p.LOGINID == loginId).ToList();
            return userMessages;
        }

        /// <summary>
        /// ��ȡ�ᶨ�û�������δ������Ϣ
        /// </summary>
        /// <param name="loginId">��¼��</param>
        /// <returns></returns>
        public IList<KhMessageModel> GetUntreatedMsgForUser(string loginId)
        {
            string hql = "from KhMessageModel p where p.LOGINID='" + loginId + "' and p.DQZT='0' order by p.MSGID desc";
            
            return GetListByHQL(hql);
        }

        /// <summary>
        /// �ر���Ϣ
        /// </summary>
        /// <param name="msgId">��ϢID</param>
        public void CloseMessage(string msgId)
        {
            var msg = GetSingle(msgId);
            if (msg != null)
            {
                msg.DB_Option_Action = WebKeys.UpdateAction;
                msg.DQZT = "1";//�ر�
                msg.WCRQ = DateTime.Now;
                Execute(msg);
            }
            
        }

        /// <summary>
        /// �ر���Ϣ
        /// </summary>
        /// <param name="user">�û�MODEL</param>
        /// <param name="khid">����ID</param>
        /// <param name="mid">ģ��ID</param>
        /// <param name="fsr">������</param>
        public void CloseMessage(SysUserModel user, int khid, string mid, string fsr)
        {
            //ֱ���Ե�ǰ�û������ȡ����첢�ر�
            List<KhMessageModel> messages = null;
            if (khid > 0)
            {
                messages = GetUntreatedMsgForUser(user.LOGINID).Where(p => p.KHID == khid && p.MID == mid && p.FSR == fsr).ToList();
            }
            else
            {
                messages = GetUntreatedMsgForUser(user.LOGINID).Where(p => p.MID == mid && p.FSR == fsr).ToList();
            }
            foreach (var m in messages)
            {
                m.DB_Option_Action = WebKeys.UpdateAction;
                m.DQZT = "1";//�ر�
                m.WCRQ = DateTime.Now;
                Execute(m);
            }


            //string sql = string.Empty;
            //sql = string.Format(
            //"delete from PERFO_KH_MESSAGE where KHID='{0}' and MID='{1}' and FSR='{2}' and LOGINID='{3}' and DQZT='0'",
            //khid, mid, fsr, user.LOGINID);
            ////if (khid >0)
            ////{
            ////}
            ////else
            ////{
            ////    sql = string.Format(
            ////    "delete from PERFO_KH_MESSAGE where MID='{0}' and FSR='{1}' and LOGINID='{2}' and DQZT='0'",
            ////    mid, fsr, user.LOGINID);
            ////}
            //dal.ExecuteSQL(sql);

        }

        public void CloseMessage(int khid, string mid)
        {
            //ֱ���Ե�ǰ�û������ȡ����첢�ر�
            List<KhMessageModel> messages = null;
            if (khid > 0)
            {
                messages = this.GetList().Where(p => p.KHID == khid && p.MID == mid ).ToList();
            }
            else
            {
                messages = this.GetList().Where(p => p.MID == mid ).ToList();
            }
            foreach (var m in messages)
            {
                m.DB_Option_Action = WebKeys.UpdateAction;
                m.DQZT = "1";//�ر�
                m.WCRQ = DateTime.Now;
                Execute(m);
            }
        }

        /// <summary>
        /// ��ָ�����û����ʹ�����Ϣ
        /// </summary>
        /// <param name="users">�������Ϣ����</param>
        /// <param name="jgbm">��������</param>
        /// <param name="khid">����ID</param>
        /// <param name="dbmc">��������</param>
        /// <param name="dbsm">����˵��</param>
        /// <param name="url">�����ļ�·��</param>
        /// <param name="mid">ģ��ID</param>
        /// <param name="fsr">������</param>
        /// <returns></returns>
        public bool SendMsgToUers(List<string> users, int jgbm, string khid, string dbmc, string dbsm, string url, string mid, string fsr)
        {
            bool isOk = true;

                //List<string> loginIds = GetUserLoginIds(u, jgbm);
                //foreach (var loginId in loginIds)
                foreach (var loginId in users)
                {
                    KhMessageModel msg = new KhMessageModel();
                    msg.DB_Option_Action = WebKeys.InsertAction;
                    msg.MSGID = "MSG" + CommonTool.GetPkId();
                    msg.KHID = khid.ToInt();
                    msg.LOGINID = loginId;
                    msg.DBMC = dbmc;
                    msg.DBSM = dbsm;
                    msg.DBLJ = url + ((url.IndexOf('?') > 0) ? "&MSGID=" + msg.MSGID : "?MSGID=" + msg.MSGID);
                    msg.JSRQ = DateTime.Now;
                    msg.DQZT = "0";
                    msg.FSR = fsr;
                    msg.MID = mid;
                    if (CheckMessageDQZT(msg))
                        isOk = Execute(msg);
                }

            return isOk;
        }


        public bool SendMsgToUers(List<SysUserModel> users, int jgbm, string khid, string dbmc, string dbsm, string url, string mid, string fsr)
        {
            bool isOk = true;

                //List<string> loginIds = GetUserLoginIds(u, jgbm);
                //foreach (var loginId in loginIds)
                foreach (var u in users)
                {
                    KhMessageModel msg = new KhMessageModel();
                    msg.DB_Option_Action = WebKeys.InsertAction;
                    msg.MSGID = "MSG" + CommonTool.GetPkId();
                    msg.KHID = khid.ToInt();
                    msg.LOGINID = u.LOGINID;
                    msg.DBMC = dbmc;
                    msg.DBSM = dbsm;
                    msg.DBLJ = url + ((url.IndexOf('?') > 0) ? "&MSGID=" + msg.MSGID : "?MSGID=" + msg.MSGID);
                    msg.JSRQ = DateTime.Now;
                    msg.DQZT = "0";
                    msg.FSR = fsr;
                    msg.MID = mid;
                    if (CheckMessageDQZT(msg))
                        isOk = Execute(msg);
            }
            return isOk;
        }
        /// <summary>
        /// ��ָ�����û����ʹ�����Ϣ
        /// </summary>
        /// <param name="loginId">��¼��</param>
        /// <param name="jgbm">��������</param>
        /// <param name="dbmc">��������</param>
        /// <param name="dbsm">����˵��</param>
        /// <param name="url">�����ļ�·��</param>
        /// <param name="mid">ģ��ID</param>
        /// <param name="fsr">������</param>
        /// <returns></returns>
        public bool SendMsgToUers(string loginId, int jgbm, string dbmc, string dbsm, string url, string mid, string fsr)
        {
            bool isOk = true;
            KhMessageModel msg = new KhMessageModel();
            msg.DB_Option_Action = WebKeys.InsertAction;
            msg.MSGID = "MSG" + CommonTool.GetPkId();
            msg.LOGINID = loginId;
            msg.DBMC = dbmc;
            msg.DBSM = dbsm;
            msg.DBLJ = url + ((url.IndexOf('?') > 0) ? "&MSGID=" + msg.MSGID : "?MSGID=" + msg.MSGID);
            msg.JSRQ = DateTime.Now;
            msg.DQZT = "0";
            msg.FSR = fsr;
            msg.MID = mid;
            //ֱ��ɾ����ͬ�ļ�¼
            if (CheckMessageDQZT(msg))
                isOk = Execute(msg);
            return isOk;
        }

        /// <summary>
        /// ����Ƿ�����ظ��Ĵ�����Ϣ
        /// </summary>
        /// <param name="msg">��Ϣ��</param>
        /// <returns></returns>
        public bool CheckMessageDQZT(KhMessageModel msg)
        {
            string sql = string.Empty;
            if (msg.KHID != null)
            {
                sql = string.Format(
                "delete from PERFO_KH_MESSAGE where KHID='{0}' and MID='{1}' and FSR='{2}' and LOGINID='{3}' and DQZT='0'",
                msg.KHID, msg.MID, msg.FSR, msg.LOGINID);
            }
            else
            {
                sql = string.Format(
                "delete from PERFO_KH_MESSAGE where MID='{0}' and FSR='{1}' and LOGINID='{2}' and DQZT='0'",
                msg.MID, msg.FSR, msg.LOGINID);
            }
            return dal.ExecuteSQL(sql);
            //var list = GetUntreatedMsgForUser(msg.LOGINID);
            //return (list.FirstOrDefault(p=>p.KHID == msg.KHID && p.MID == msg.MID && p.FSR == msg.FSR) == null);
        }


        /// <summary>
        /// ��ȡ����˵ĵ�¼����
        /// </summary>
        /// <param name="shr">�����</param>
        /// <param name="jgbm">��������</param>
        /// <returns></returns>
        public List<string> GetUserLoginIds(string shr, int jgbm)
        {
            List<string> lst = new List<string>();
            if (shr.ToInt() > 0)
            {
                //����
                var users = userSrv.GetUserListForValid().Where(p => p.JGBM == shr.ToInt()).ToList();
                foreach (var u in users)
                {
                    lst.Add(u.LOGINID);
                }
            }
            else if (shr == "YQTLD")
            {
                //�������쵼
                var users = userSrv.GetUserListForValid().Where(p => p.DUTY == "�������쵼").ToList();
                foreach (var u in users)
                {
                    lst.Add(u.LOGINID);
                }
            }
            else if (shr == "FGLD")
            {
                //�ֹ��쵼
                var users = userSrv.GetUserListForValid().Where(p=>p.FgbmjgLst.Count > 0).ToList();
                foreach (var u in users)
                {
                    if (u.FgbmjgLst.FirstOrDefault(c => c.JGBM == jgbm) != null)
                    {
                        lst.Add(u.LOGINID);
                    }
                }
            }
            return lst;
        }


        #endregion

    }

}
