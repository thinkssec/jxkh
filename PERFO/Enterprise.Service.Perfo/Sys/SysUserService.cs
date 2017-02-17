using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Data.Perfo.Sys;
using Enterprise.Model.Perfo.Sys;

namespace Enterprise.Service.Perfo.Sys
{

    /// <summary>
    /// �ļ���:  SysUserService.cs
    /// ��������: ҵ���߼���-�û������ݴ���
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:46
    /// </summary>
    public class SysUserService
    {

        /// <summary>
        /// �ֹܲ��Ż���-������
        /// </summary>
        SysFgbmjgService fgbmjgSrv = new SysFgbmjgService();

        #region ����������

        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly ISysUserData dal = new SysUserData();

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SysUserModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<SysUserModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<SysUserModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<SysUserModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(SysUserModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region �Զ��巽��

        /// <summary>
        /// ��ȡָ���û������зֹܻ�������
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public List<SysFgbmjgModel> GetFgjgbmList(string loginId)
        {
            return fgbmjgSrv.GetListByHQL("from SysFgbmjgModel p where p.LOGINID='" + loginId + "'").ToList();
        }

        /// <summary>
        /// ɾ��ָ���û����зֹܻ���
        /// </summary>
        /// <param name="roleId"></param>
        public bool DeleteFgjgbm(string loginId)
        {
            string sql = "delete from PERFO_SYS_FGBMJG where LOGINID='" + loginId + "'";
            return dal.ExecuteSQL(sql);
        }

        /// <summary>
        /// ��ȡ���д�����Ч״̬���û���Ϣ����
        /// </summary>
        /// <returns></returns>
        public IList<SysUserModel> GetUserListForValid()
        {
            string hql = "from SysUserModel p where p.DISABLE='0'";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// ��ȡָ����λ�ķֹ��쵼�û�MODEL
        /// </summary>
        /// <param name="jgbm">��������</param>
        /// <returns></returns>
        public SysUserModel GetUserByFgjgbm(int jgbm)
        {
            SysUserModel user = null;
            var fgbmLst = fgbmjgSrv.GetListByHQL("from SysFgbmjgModel p where p.JGBM='" + jgbm + "'").ToList();
            if (fgbmLst != null && fgbmLst.Count > 0)
            {
                user = GetSingle(fgbmLst.First().LOGINID);
            }
            return user;
        }

        /// <summary>
        /// ��ȡָ����ְͬ�����Ƶ������û���Ϣ����
        /// </summary>
        /// <param name="dutyName">ְ������</param>
        /// <returns></returns>
        public IList<SysUserModel> GetUserListByDuty(string dutyName)
        {
            return GetUserListForValid().Where(p => p.DUTY == dutyName).ToList();
        }

        #endregion

        #region ��̬������

        /// <summary>
        /// ��ȡ�û�����
        /// </summary>
        /// <param name="loginId">��¼��</param>
        /// <returns></returns>
        public static string GetUserNameByLoginId(string loginId)
        {
            var u = dal.GetSingle(loginId);
            return (u != null) ? u.USERNAME : "";
        }

        #endregion

        #region ���ɲ˵����

        private string CaiDanHtml;
        private SysRolepermissionService roleService = new SysRolepermissionService();
        private SysUserpermissionService userPmService = new SysUserpermissionService();
        private SysModuleService moduleService = new SysModuleService();
        //��ȡ���û���Ȩ�޵�����ģ��
        List<SysModuleModel> moduleList = new List<SysModuleModel>();

        /// <summary>
        /// �����û�Ȩ�޼����۵�ʽ�˵�
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public static string LoadAccordionMenu(SysUserModel userModel)
        {
            SysUserService userSrv = new SysUserService();
            return userSrv.CreateZheDieCaiDanHtml(userModel);
        }

        /// <summary>
        /// �����û�Ȩ�޼������Ͳ˵�
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public static string LoadTreeMenu(SysUserModel userModel)
        {
            SysUserService userSrv = new SysUserService();
            return userSrv.CreateTreeMenu(userModel);
        }

        #region �����Ͳ˵�

        /// <summary>
        /// �۵��˵�
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        private string CreateZheDieCaiDanHtml(SysUserModel userModel)
        {
            if (userModel != null)
            {
                CaiDanHtml = "";
                //��ȡ���û��Ľ�ɫȨ��
                List<SysRolepermissionModel> pmList = roleService.GetList().
                    Where(p => p.ROLEID == userModel.ROLEID).ToList();
                //��ȡ��ǰ�û�������Ȩ��
                List<SysUserpermissionModel> userPmList = userPmService.GetList().
                    Where(p => p.LOGINID == userModel.LOGINID).ToList();
                //��ȡ����ģ����Ϣ
                var moduleLst = moduleService.GetList();
                //���ݽ�ɫȨ����ȡ��Ӧ��ģ����Ϣ
                foreach (SysRolepermissionModel pm in pmList)
                {
                    List<SysModuleModel> mList = moduleLst.Where(p => (p.MID == pm.MID || p.MPARENTID == "0") && p.DISABLE == "0").
                        OrderBy(p => p.MID).ThenBy(p => p.XSXH).ToList();
                    foreach (SysModuleModel model in mList)
                    {
                        if (!moduleList.Exists(p => p.MID == model.MID))
                        {
                            moduleList.Add(model);
                        }
                    }
                }
                //�����û�����Ȩ����ȡ��Ӧ��ģ����Ϣ
                foreach (SysUserpermissionModel upm in userPmList)
                {
                    List<SysModuleModel> mList = moduleLst.Where(p => (p.MID == upm.MID || p.MPARENTID == "0") && p.DISABLE == "0").
                        OrderBy(p => p.MID).ThenBy(p => p.XSXH).ToList();
                    foreach (SysModuleModel model in mList)
                    {
                        if (!moduleList.Exists(p => p.MID == model.MID))
                        {
                            moduleList.Add(model);
                        }
                    }
                }
                CreateCaiDan("0");
            }
            return CaiDanHtml;
        }

        /// <summary>
        /// ����һ���˵�
        /// </summary>
        /// <param name="Parentid"></param>
        private void CreateCaiDan(string Parentid)
        {
            var q = from root in moduleList
                    where root.MPARENTID == Parentid
                    select root;
            int i = 0;
            foreach (var t in q)
            {
                int a = moduleList.Count(x => x.MPARENTID == t.MID);
                //���û���κ��ӽڵ� ��˵��û��Ȩ�� ����
                if (a != 0)
                {
                    string iconCss = (!string.IsNullOrEmpty(t.BZ)) ? t.BZ : "icon-sipc";
                    if (i == 0)
                        CaiDanHtml += "<div title=\"" + t.MNAME + "\" selected=\"true\"  style=\"padding:10px;overflow-x: hidden;\" iconCls=\"" + iconCss + "\">";
                    else
                        CaiDanHtml += "<div title=\"" + t.MNAME + "\" style=\"padding:10px;overflow-x: hidden;\" iconCls=\"" + iconCss + "\">";
                    CreateZiCaiDan(t.MID);
                    CaiDanHtml += "</div>";
                }
                i++;
            }
        }

        /// <summary>
        /// ���������˵�
        /// </summary>
        /// <param name="IntParent"></param>
        private void CreateZiCaiDan(string IntParent)
        {
            var q = from root in moduleList
                    where root.MPARENTID == IntParent
                    orderby root.XSXH
                    select root;
            foreach (var t in q)
            {
                CaiDanHtml += "<span><a style=\"line-height:30px;text-decoration: none;\" class=\"leftMenu\" href=\"javascript:addTab('/"
                    + t.WEBURL + "','" + t.MNAME + "')\">" + t.MNAME + "</a></span>";
            }
        }

        #endregion
        
        #region ���Ͳ˵�

        private string CreateTreeMenu(SysUserModel userModel)
        {
            if (userModel != null)
            {
                CaiDanHtml = "";
                //��ȡ���û��Ľ�ɫȨ��
                List<SysRolepermissionModel> pmList = roleService.GetList().
                    Where(p => p.ROLEID == userModel.ROLEID).ToList();
                //��ȡ��ǰ�û�������Ȩ��
                List<SysUserpermissionModel> userPmList = userPmService.GetList().
                    Where(p => p.LOGINID == userModel.LOGINID).ToList();
                //��ȡ����ģ����Ϣ
                var moduleLst = moduleService.GetList();
                //���ݽ�ɫȨ����ȡ��Ӧ��ģ����Ϣ
                foreach (SysRolepermissionModel pm in pmList)
                {
                    List<SysModuleModel> mList = moduleLst.Where(p => (p.MID == pm.MID || p.MPARENTID == "0") && p.DISABLE == "0").
                        OrderBy(p => p.MID).ThenBy(p => p.XSXH).ToList();
                    foreach (SysModuleModel model in mList)
                    {
                        if (!moduleList.Exists(p => p.MID == model.MID))
                        {
                            moduleList.Add(model);
                        }
                    }
                }
                //�����û�����Ȩ����ȡ��Ӧ��ģ����Ϣ
                foreach (SysUserpermissionModel upm in userPmList)
                {
                    List<SysModuleModel> mList = moduleLst.Where(p => (p.MID == upm.MID || p.MPARENTID == "0") && p.DISABLE == "0").
                        OrderBy(p => p.MID).ThenBy(p => p.XSXH).ToList();
                    foreach (SysModuleModel model in mList)
                    {
                        if (!moduleList.Exists(p => p.MID == model.MID))
                        {
                            moduleList.Add(model);
                        }
                    }
                }
                //moduleList = moduleService.GetList().OrderBy(p => p.MID).ThenBy(p => p.XSXH).ToList();
                CreateTree("0");
            }
            return CaiDanHtml;
        }

        private void CreateTree(string parentID)
        {
            var q = from root in moduleList
                    where root.MPARENTID == parentID
                    orderby root.XSXH
                    select root;
            foreach (var t in q)
            {  
                int a = moduleList.Count(x => x.MPARENTID == t.MID);
                //mod by qw 2014.12.24 ���ƽڵ�Ϊ����
                if (t.MID.Length > 4 || (a == 0 && parentID == "0")) continue;

                string node = (string.IsNullOrEmpty(t.WEBURL) || parentID == "0") ? "<span>" + t.MNAME + "</span>" : 
                    "<span><a style=\"text-decoration: none;\" href=\"javascript:addTab('/" + t.WEBURL + "','" 
                    + t.MNAME + "');\">" + t.MNAME + "</a></span>";
                //CaiDanHtml += "<li>"+node;
                //������ӽڵ�
                if (a != 0)
                {
                    CaiDanHtml += "<li ";
                    if (t.MID.Length == 2)
                    {
                        if (!string.IsNullOrEmpty(t.BZ))
                        {
                            CaiDanHtml += " state=\"closed\" iconCls=\"" + t.BZ + "\" ";
                        }
                        else
                        {
                            //CaiDanHtml += " iconCls=\"icon-sipc\" ";
                        }
                    }
                    CaiDanHtml += ">" + node;
                    //���ӽڵ�
                    CaiDanHtml += "<ul>";
                    CreateTree(t.MID);
                    CaiDanHtml += "</ul>";
                }
                else
                {
                    CaiDanHtml += "<li>" + node;
                    CaiDanHtml += "</li>";
                }
            }
        }       

        #endregion

        #endregion

    }
}
