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
    /// 文件名:  KhMessageService.cs
    /// 功能描述: 业务逻辑层-待办箱数据处理
    /// 创建人：代码生成器
    /// 创建时间 ：2014/11/1 0:35:45
    /// </summary>
    public class KhMessageService
    {

        SysUserService userSrv = new SysUserService();

        #region 代码生成器

        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly IKhMessageData dal = new KhMessageData();

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhMessageModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhMessageModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhMessageModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhMessageModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhMessageModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region 自定义方法区

        /// <summary>
        /// 获取与考核相关的所有待办消息
        /// </summary>
        /// <param name="khid">考核ID</param>
        /// <returns></returns>
        public IList<KhMessageModel> GetMessagesByKhid(string khid)
        {
            string hql = "from KhMessageModel p where p.KHID='" + khid + "' order by p.MSGID desc";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// 获取与考核和用户登录名相关的所有待办消息
        /// </summary>
        /// <param name="khid">考核ID</param>
        /// <param name="loginId">登录ID</param>
        /// <returns></returns>
        public IList<KhMessageModel> GetMessagesByKhidAndLoginId(string khid, string loginId)
        {
            var userMessages = GetMessagesByKhid(khid).Where(p => p.LOGINID == loginId).ToList();
            return userMessages;
        }

        /// <summary>
        /// 获取提定用户的所有未处理消息
        /// </summary>
        /// <param name="loginId">登录名</param>
        /// <returns></returns>
        public IList<KhMessageModel> GetUntreatedMsgForUser(string loginId)
        {
            string hql = "from KhMessageModel p where p.LOGINID='" + loginId + "' and p.DQZT='0' order by p.MSGID desc";
            
            return GetListByHQL(hql);
        }

        /// <summary>
        /// 关闭消息
        /// </summary>
        /// <param name="msgId">消息ID</param>
        public void CloseMessage(string msgId)
        {
            var msg = GetSingle(msgId);
            if (msg != null)
            {
                msg.DB_Option_Action = WebKeys.UpdateAction;
                msg.DQZT = "1";//关闭
                msg.WCRQ = DateTime.Now;
                Execute(msg);
            }
            
        }

        /// <summary>
        /// 关闭消息
        /// </summary>
        /// <param name="user">用户MODEL</param>
        /// <param name="khid">考核ID</param>
        /// <param name="mid">模块ID</param>
        /// <param name="fsr">发送人</param>
        public void CloseMessage(SysUserModel user, int khid, string mid, string fsr)
        {
            //直接以当前用户身份提取其待办并关闭
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
                m.DQZT = "1";//关闭
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
            //直接以当前用户身份提取其待办并关闭
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
                m.DQZT = "1";//关闭
                m.WCRQ = DateTime.Now;
                Execute(m);
            }
        }

        /// <summary>
        /// 给指定的用户发送待办消息
        /// </summary>
        /// <param name="users">审核人信息集合</param>
        /// <param name="jgbm">机构编码</param>
        /// <param name="khid">考核ID</param>
        /// <param name="dbmc">待办名称</param>
        /// <param name="dbsm">待办说明</param>
        /// <param name="url">待办文件路径</param>
        /// <param name="mid">模块ID</param>
        /// <param name="fsr">发送人</param>
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
        /// 给指定的用户发送待办消息
        /// </summary>
        /// <param name="loginId">登录名</param>
        /// <param name="jgbm">机构编码</param>
        /// <param name="dbmc">待办名称</param>
        /// <param name="dbsm">待办说明</param>
        /// <param name="url">待办文件路径</param>
        /// <param name="mid">模块ID</param>
        /// <param name="fsr">发送人</param>
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
            //直接删除相同的记录
            if (CheckMessageDQZT(msg))
                isOk = Execute(msg);
            return isOk;
        }

        /// <summary>
        /// 检测是否出现重复的待办消息
        /// </summary>
        /// <param name="msg">消息类</param>
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
        /// 获取审核人的登录名称
        /// </summary>
        /// <param name="shr">审核人</param>
        /// <param name="jgbm">机构编码</param>
        /// <returns></returns>
        public List<string> GetUserLoginIds(string shr, int jgbm)
        {
            List<string> lst = new List<string>();
            if (shr.ToInt() > 0)
            {
                //机构
                var users = userSrv.GetUserListForValid().Where(p => p.JGBM == shr.ToInt()).ToList();
                foreach (var u in users)
                {
                    lst.Add(u.LOGINID);
                }
            }
            else if (shr == "YQTLD")
            {
                //油气田领导
                var users = userSrv.GetUserListForValid().Where(p => p.DUTY == "油气田领导").ToList();
                foreach (var u in users)
                {
                    lst.Add(u.LOGINID);
                }
            }
            else if (shr == "FGLD")
            {
                //分管领导
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
