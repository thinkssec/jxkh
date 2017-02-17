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
using Enterprise.Model.Perfo.Zbk;
using System.Web.UI.WebControls;

namespace Enterprise.Service.Perfo.Kh
{
	
    /// <summary>
    /// �ļ���:  KhDfzbmxService.cs
    /// ��������: ҵ���߼���-���ָ�꿼�˱����ݴ���
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:45
    /// </summary>
    public class KhDfzbmxService
    {

        /// <summary>
        /// �û���Ϣ--������
        /// </summary>
        SysUserService userSrv = new SysUserService();

        #region ����������

        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly IKhDfzbmxData dal = new KhDfzbmxData();

	    /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhDfzbmxModel GetSingle(string khid, string key)
        {
            return GetListByKhid(khid).FirstOrDefault(p => p.DFZBID == key);
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhDfzbmxModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhDfzbmxModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

	    /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhDfzbmxModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhDfzbmxModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region �Զ��巽����

        /// <summary>
        /// ��ȡ��ָ����������ص����д������
        /// </summary>
        /// <param name="khid">������</param>
        /// <returns></returns>
        public IList<KhDfzbmxModel> GetListByKhid(string khid)
        {
            string hql = "from KhDfzbmxModel p where p.KHID='" + khid + "'";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// ����ָ�������µĴ��ָ��Ļ�������
        /// </summary>
        /// <param name="khid">����ID</param>
        /// <param name="khType">��������</param>
        /// <param name="zbxsModel">ָ��ɸѡMODEL</param>
        /// <param name="model">���ָ��</param>
        /// <returns></returns>
        public bool LoadZrszbModelInDfzbmxByDfzb(string khid, WebKeys.KaoheType khType, KhZbsxModel zbxsModel, KhDfzbmxModel model)
        {
            KhJgbmdfbService jgbmdfbSrv = new KhJgbmdfbService();
            SysBmjgService bmjgSrv = new SysBmjgService();

            List<KhJgbmdfbModel> jgbmdfList = model.KhJgbmdfbLst.ToList();
            if (zbxsModel.JxzrsZb.Dfzb != null)
            {
                var dfzLst = zbxsModel.JxzrsZb.Dfzb.DfzLst.Where(p => p.DFZBBM == zbxsModel.SXZBBM);
                string dfzs = "";
                foreach (var dfr in dfzLst)
                {
                    dfzs += dfr.OPERATOR + ",";
                    //�ɶ��˴��ʱ��ͬʱ���ڴ�ֱ���
                    var users = GetUserListByDfz(dfr, model);
                    if (users.Count > 0)
                    {
                        foreach (var u in users)
                        {
                            KhJgbmdfbModel jgbmdfModel = new KhJgbmdfbModel();
                            jgbmdfModel.DB_Option_Action = WebKeys.InsertAction;
                            jgbmdfModel.DFBID = CommonTool.GetGuidKey(); //��ֱ�ID
                            jgbmdfModel.ID = null; //�������˱�ID
                            jgbmdfModel.DFZBID = model.DFZBID;//���ָ��ID
                            jgbmdfModel.ZBBM = model.ZBBM; //ָ�����
                            jgbmdfModel.JGBM = model.JGBM; //��������
                            jgbmdfModel.KHID = model.KHID; //����ID
                            jgbmdfModel.DFZ = u.LOGINID; //����ߣ�ָ���û�
                            if (zbxsModel.JxzrsZb.Dfzb.Zbxx.YJZBMC.Contains("��������"))
                            {
                                jgbmdfModel.DFZLX = ((int)WebKeys.DFUserType.�ϼ��쵼).ToString();
                            }
                            else
                            {
                                jgbmdfModel.DFZLX = dfr.OPERTYPE; //���������
                            }
                            //jgbmdfModel.DFQZ = dfr.OPERQZ.ToDecimal() / users.Count; //���Ȩ�� ���ʱ���ݼ���������и���
                            if (!jgbmdfList.Exists(p => p.JGBM == jgbmdfModel.JGBM && p.DFZBID == jgbmdfModel.DFZBID
                                && p.DFZ == jgbmdfModel.DFZ))
                                jgbmdfList.Add(jgbmdfModel);//�ݴ�
                        }
                    }
                    else
                    {
                        KhJgbmdfbModel jgbmdfModel = new KhJgbmdfbModel();
                        jgbmdfModel.DB_Option_Action = WebKeys.InsertAction;
                        jgbmdfModel.DFBID = CommonTool.GetGuidKey(); //��ֱ�ID
                        jgbmdfModel.ID = null; //�������˱�ID
                        jgbmdfModel.DFZBID = model.DFZBID;//���ָ��ID
                        jgbmdfModel.ZBBM = model.ZBBM; //ָ�����
                        jgbmdfModel.JGBM = model.JGBM; //��������
                        jgbmdfModel.KHID = model.KHID; //����ID
                        jgbmdfModel.DFZ = dfr.OPERATOR; //����ߣ��Ե�λID����
                        if (zbxsModel.JxzrsZb.Dfzb.Zbxx.YJZBMC.Contains("��������"))
                        {
                            //��쵱ǰ��λ�ǲ��Ż��Ƕ�����λ
                            var bmjg = bmjgSrv.GetSingle(jgbmdfModel.DFZ);
                            if (bmjg != null)
                            {
                                if (bmjg.JGLX.Contains("ְ��"))
                                {
                                    jgbmdfModel.DFZLX = ((int)WebKeys.DFUserType.ͬ������).ToString();
                                }
                                else if (bmjg.JGLX.Contains("����"))
                                {
                                    jgbmdfModel.DFZLX = ((int)WebKeys.DFUserType.ͬ��������λ).ToString();
                                }
                            }
                            else
                            {
                                jgbmdfModel.DFZLX = dfr.OPERTYPE; //���������
                            }
                        }
                        else
                        {
                            jgbmdfModel.DFZLX = dfr.OPERTYPE; //���������
                        }
                        jgbmdfModel.DFZLX = dfr.OPERTYPE; //���������
                        //jgbmdfModel.DFQZ = shr.OPERQZ; //���Ȩ�� ���ʱ���ݼ���������и���
                        if (!jgbmdfList.Exists(p => p.JGBM == jgbmdfModel.JGBM && p.DFZBID == jgbmdfModel.DFZBID
                                && p.DFZ == jgbmdfModel.DFZ))
                            jgbmdfList.Add(jgbmdfModel);//�ݴ�
                    }

                }
                model.DFZ = dfzs.TrimEnd(',');//�����
            }
            return Execute(model) && jgbmdfbSrv.ExecuteByList(jgbmdfList);
        }

        /// <summary>
        /// ����ָ�������µĴ��ָ��
        /// </summary>
        /// <param name="khid">����ID</param>
        /// <param name="khType">��������</param>
        /// <param name="zbxsModel">ָ��ɸѡMODEL</param>
        /// <returns></returns>
        public bool LoadZrszbModelInDfzbmx(string khid, WebKeys.KaoheType khType, KhZbsxModel zbxsModel)
        {
            KhJgbmdfbService jgbmdfbSrv = new KhJgbmdfbService();
            SysBmjgService bmjgSrv = new SysBmjgService();
            List<KhJgbmdfbModel> jgbmdfList = new List<KhJgbmdfbModel>();

            KhDfzbmxModel model = new KhDfzbmxModel();
            model.DB_Option_Action = WebKeys.InsertAction;
            model.DFZBID = "DF" + CommonTool.GetPkId();//���ָ��ID
            model.SXID = zbxsModel.SXID;//ɸѡ��ID
            model.ZBBM = zbxsModel.SXZBBM;//ָ�����
            model.JGBM = zbxsModel.SXJGBM; //��������
            model.KHID = khid.ToInt();//����ID
            model.KHDX = ((int)khType).ToString();
            if (zbxsModel.JxzrsZb.Dfzb != null)
            {
                var dfzLst = zbxsModel.JxzrsZb.Dfzb.DfzLst.Where(p => p.DFZBBM == zbxsModel.SXZBBM);
                string dfzs = "";
                foreach (var dfr in dfzLst)
                {
                    dfzs += dfr.OPERATOR + ",";
                    //�ɶ��˴��ʱ��ͬʱ���ڴ�ֱ���
                    var users = GetUserListByDfz(dfr, model);
                    if (users.Count > 0)
                    {
                        foreach (var u in users)
                        {
                            KhJgbmdfbModel jgbmdfModel = new KhJgbmdfbModel();
                            jgbmdfModel.DB_Option_Action = WebKeys.InsertAction;
                            jgbmdfModel.DFBID = CommonTool.GetGuidKey(); //��ֱ�ID
                            jgbmdfModel.ID = null; //�������˱�ID
                            jgbmdfModel.DFZBID = model.DFZBID;//���ָ��ID
                            jgbmdfModel.ZBBM = model.ZBBM; //ָ�����
                            jgbmdfModel.JGBM = model.JGBM; //��������
                            jgbmdfModel.KHID = model.KHID; //����ID
                            jgbmdfModel.DFZ = u.LOGINID; //����ߣ�ָ���û�
                            if (zbxsModel.JxzrsZb.Dfzb.Zbxx.YJZBMC.Contains("��������"))
                            {
                                jgbmdfModel.DFZLX = ((int)WebKeys.DFUserType.�ϼ��쵼).ToString();
                            }
                            else
                            {
                                jgbmdfModel.DFZLX = dfr.OPERTYPE; //���������
                            }
                            //jgbmdfModel.DFQZ = dfr.OPERQZ.ToDecimal() / users.Count; //���Ȩ�� ���ʱ���ݼ���������и���
                            if (!jgbmdfList.Exists(p => p.JGBM == jgbmdfModel.JGBM && p.DFZBID == jgbmdfModel.DFZBID
                                && p.DFZ == jgbmdfModel.DFZ))
                                jgbmdfList.Add(jgbmdfModel);//�ݴ�
                        }
                    }
                    else
                    {
                        KhJgbmdfbModel jgbmdfModel = new KhJgbmdfbModel();
                        jgbmdfModel.DB_Option_Action = WebKeys.InsertAction;
                        jgbmdfModel.DFBID = CommonTool.GetGuidKey(); //��ֱ�ID
                        jgbmdfModel.ID = null; //�������˱�ID
                        jgbmdfModel.DFZBID = model.DFZBID;//���ָ��ID
                        jgbmdfModel.ZBBM = model.ZBBM; //ָ�����
                        jgbmdfModel.JGBM = model.JGBM; //��������
                        jgbmdfModel.KHID = model.KHID; //����ID
                        jgbmdfModel.DFZ = dfr.OPERATOR; //����ߣ��Ե�λID����
                        if (zbxsModel.JxzrsZb.Dfzb.Zbxx.YJZBMC.Contains("��������"))
                        {
                            //��쵱ǰ��λ�ǲ��Ż��Ƕ�����λ
                            var bmjg = bmjgSrv.GetSingle(jgbmdfModel.DFZ);
                            if (bmjg != null)
                            {
                                if (bmjg.JGLX.Contains("ְ��"))
                                {
                                    jgbmdfModel.DFZLX = ((int)WebKeys.DFUserType.ͬ������).ToString();
                                }
                                else if (bmjg.JGLX.Contains("����"))
                                {
                                    jgbmdfModel.DFZLX = ((int)WebKeys.DFUserType.ͬ��������λ).ToString();
                                }
                            }
                            else
                            {
                                jgbmdfModel.DFZLX = dfr.OPERTYPE; //���������
                            }
                        }
                        else
                        {
                            jgbmdfModel.DFZLX = dfr.OPERTYPE; //���������
                        }
                        //jgbmdfModel.DFZLX = dfr.OPERTYPE; //���������
                        //jgbmdfModel.DFQZ = shr.OPERQZ; //���Ȩ�� ���ʱ���ݼ���������и���
                        if (!jgbmdfList.Exists(p => p.JGBM == jgbmdfModel.JGBM && p.DFZBID == jgbmdfModel.DFZBID
                                && p.DFZ == jgbmdfModel.DFZ))
                            jgbmdfList.Add(jgbmdfModel);//�ݴ�
                    }

                }
                model.DFZ = dfzs.TrimEnd(',');//�����
            }
            return Execute(model) && jgbmdfbSrv.ExecuteByList(jgbmdfList);
        }

        /// <summary>
        /// ��ȡ����߶�Ӧ�ľ����û���Ϣ����
        /// </summary>
        /// <param name="dfz">�����</param>
        /// <param name="dfzbmx">���ָ����ϸ</param>
        /// <returns></returns>
        public IList<SysUserModel> GetUserListByDfz(ZbkDfzModel dfz, KhDfzbmxModel dfzbmx)
        {
            List<SysUserModel> users = new List<SysUserModel>();
            if (dfz.OPERATOR == "YQTLD")
            {
                users = userSrv.GetUserListByDuty("�������쵼") as List<SysUserModel>;
            }
            else if (dfz.OPERATOR == "FGLD")
            {
                var user = userSrv.GetUserByFgjgbm(dfzbmx.JGBM.Value);
                if (user != null)
                {
                    users.Add(user);
                }
            }
            return users;
        }

        /// <summary>
        /// ��ȡ��ָ�������ںͻ���������ص����д������
        /// </summary>
        /// <param name="khid">������</param>
        /// <param name="jgbm">��������</param>
        /// <returns></returns>
        public IList<KhDfzbmxModel> GetListByKhidAndJgbm(string khid, string jgbm)
        {
            string hql = "from KhDfzbmxModel p where p.KHID='" + khid + "' and p.JGBM='" + jgbm + "'";
            return GetListByHQL(hql).OrderBy(p => p.ZbsxModel.SXXH).ToList();
        }

        /// <summary>
        /// ��ȡ�����ں�ָ������ĸ���λ�������
        /// </summary>
        /// <param name="khid">������</param>
        /// <param name="zbid">ָ��ID</param>
        /// <param name="kaoheType">��������</param>
        /// <returns></returns>
        public IList<KhDfzbmxModel> GetListByKhidAndZhibiao(string khid, string zbid, WebKeys.KaoheType kaoheType)
        {
            string hql = "from KhDfzbmxModel p where p.KHID='" + khid + "' and p.ZBBM='" + zbid + "'";
            return GetListByHQL(hql).Where(p => p.KHDX == ((int)kaoheType).ToString()).OrderBy(p => p.Danwei.XSXH).ToList();
        }

        /// <summary>
        /// ��ȡָ���������µ����пɴ�ֵĶ���ָ�꼯��
        /// </summary>
        /// <param name="khid">����ID</param>
        /// <returns></returns>
        public IList<KhDfzbmxModel> GetListByKhidAndKhdx(string khid, WebKeys.KaoheType kaoheType)
        {
            string hql = "from KhDfzbmxModel p where p.KHID='" + khid + "' and p.KHDX='" + ((int)kaoheType) + "'";
            return GetListByHQL(hql).OrderBy(p=>p.DfzbModel.Zbxx.YJZBMC).ThenBy(p => p.ZbsxModel.SXXH).ToList();
        }

        /// <summary>
        /// �󶨴����鹦�ܵĴ��ָ�������ؼ�
        /// </summary>
        /// <param name="ddl">�����ؼ�</param>
        /// <param name="dfzbmxLst">���ָ����ϸ</param>
        public void BindSSECDropDownListForDfzb(SSECDropDownList ddl, IList<KhDfzbmxModel> dfzbmxLst)
        {
            ddl.Items.Clear();
            string zblx = string.Empty;
            List<string> itemKeys = new List<string>();
            foreach (var q in dfzbmxLst)
            {
                if (string.IsNullOrEmpty(zblx))
                {
                    ddl.Items.Add(new ListItem(q.DfzbModel.Zbxx.YJZBMC, "optgroup"));
                    ddl.Items.Add(new ListItem(q.DfzbModel.Zbxx.ZBMC, q.ZBBM));
                    zblx = q.DfzbModel.Zbxx.YJZBMC;
                    itemKeys.Add(q.DfzbModel.Zbxx.YJZBMC + "��" + q.DfzbModel.Zbxx.ZBMC);
                }
                else if (q.DfzbModel.Zbxx.YJZBMC == zblx)
                {
                    if (!itemKeys.Exists(p => p == q.DfzbModel.Zbxx.YJZBMC + "��" + q.DfzbModel.Zbxx.ZBMC))
                    {
                        ddl.Items.Add(new ListItem(q.DfzbModel.Zbxx.ZBMC, q.ZBBM));
                    }
                }
                else
                {
                    if (!itemKeys.Exists(p => p == q.DfzbModel.Zbxx.YJZBMC + "��" + q.DfzbModel.Zbxx.ZBMC))
                    {
                        ddl.Items.Add(new ListItem(q.DfzbModel.Zbxx.YJZBMC, "optgroup"));
                        ddl.Items.Add(new ListItem(q.DfzbModel.Zbxx.ZBMC, q.ZBBM));
                        zblx = q.DfzbModel.Zbxx.YJZBMC;
                    }
                }
            }
        }

        #endregion
    }

}
