using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Component.Cache;
using Enterprise.Service.Perfo.Sys;
using Enterprise.Data.Perfo.Kh;
using Enterprise.Model.Perfo.Kh;
using Enterprise.Model.Perfo.Sys;
using System.Web.UI.WebControls;

namespace Enterprise.Service.Perfo.Kh
{

    /// <summary>
    /// �ļ���:  KhKhglService.cs
    /// ��������: ҵ���߼���-���˹������ݴ���
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:45
    /// </summary>
    public class KhKhglService
    {

        /// <summary>
        /// ����ָ����ز��Ŵ��-������
        /// </summary>
        KhJgbmdfbService jgbmdfbSrv = new KhJgbmdfbService();
        KhDlzbmxService dlzbmxSrv = new KhDlzbmxService();//����ָ����ϸ������
        KhDfzbmxService dfzbmxSrv = new KhDfzbmxService();//���ָ����ϸ������
        KhUnlockService unlockSrv = new KhUnlockService();//��������������
        KhSjsbService sjsbSrv = new KhSjsbService();//�����ļ��ϱ�������
        KhMessageService msgSrv = new KhMessageService();//�����������
        KhJgbmkhdfService jgbmkhdfSrv = new KhJgbmkhdfService();//���ز��ſ��˵÷�
        KhJxzrszbService jxzrszbSrv = new KhJxzrszbService();//��Ч������ָ��
        KhEjdwkhdfService ejdwkhdfSrv = new KhEjdwkhdfService();//������λ���˵÷�
        KhCwjcsjService cwjcsjSrv = new KhCwjcsjService();//����������ݱ�
        SysUserService userSrv = new SysUserService();//�û�������
        
        #region ����������

        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly IKhKhglData dal = new KhKhglData();
        /// <summary>
        /// ����������
        /// </summary>
        public string CacheKey
        {
            get 
            { 
                return KhKhglData.CacheClassKey; 
            }
        }

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhKhglModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhKhglModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhKhglModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhKhglModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhKhglModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region ���Ի���ҳ�����

        /// <summary>
        /// ��ȡ����һ�ڵĶ�����λ���쵼���ӿ�������Ҫ��ǰ�û���˵�ָ����Ϣ
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public string GetEjdwKaoheInfoByJgbm(SysUserModel userModel)
        {
            string khInfo = string.Empty;
            if (userModel == null) 
                return "";
            
            if (WebKeys.EnableCaching)
            {
                khInfo = (string)CacheHelper.GetCache(KhKhglData.CacheClassKey + "_GetEjdwKaoheInfoByJgbm_" + userModel.JGBM);
            }
            if (string.IsNullOrEmpty(khInfo))
            {
                StringBuilder sb = new StringBuilder();
                bool isAudit = false;
                //���һ�ڿ���
                var lastKaohe = GetLastKaoheInfo("LX2014A");//������λ���쵼���ӿ���
                if (lastKaohe != null)
                {
                    sb.Append("<tr>");
                    sb.Append("<td colspan=\"6\" class=\"td-bold\">��" + lastKaohe.KHMC + "����Ҫ����˵�ָ��</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<th style=\"width: 6%\" class=\"td-bold\">���</th>");
                    sb.Append("<th style=\"width: 15%\" class=\"td-bold\">ָ������</th>");
                    sb.Append("<th style=\"width: 30%\" class=\"td-bold\">ָ������</th>");
                    sb.Append("<th style=\"width: 33%\" class=\"td-bold\">����˵�λ</th>");
                    sb.Append("<th style=\"width: 8%\" class=\"td-bold\">Ӧ���</th>");
                    sb.Append("<th style=\"width: 8%\" class=\"td-bold\">�����</th>");
                    sb.Append("</tr>");
                    //��ȡ�������ڵĶ����Ͷ���ָ�꣬����һ������
                    var dlzbLst = dlzbmxSrv.GetListByKhid(lastKaohe.KHID.ToString()).
                        Where(p => ("," + p.WCZSHR + ",").Contains("," + userModel.JGBM + ",")).ToList();
                    var zbmcLst = dlzbLst.DistinctBy(p => p.LhzbModel.Zbxx.ZBMC).ToList();
                    var yjzbmcLst = dlzbLst.DistinctBy(p => p.LhzbModel.Zbxx.YJZBMC).ToList();
                    for (int i = 0; i < yjzbmcLst.Count; i++)
                    {
                        List<string> khjgbms = new List<string>();
                        sb.Append("<tr>");
                        sb.Append("<td>" + (i + 1) + "</td>");
                        sb.Append("<td>" + yjzbmcLst[i].LhzbModel.Zbxx.YJZBMC + "</td>");
                        var zbs = zbmcLst.Where(p => p.LhzbModel.Zbxx.YJZBMC == yjzbmcLst[i].LhzbModel.Zbxx.YJZBMC).ToList();
                        string lhzb = "";
                        string dwmc = "";
                        int yssl = 0;
                        int wcsl = 0;
                        foreach (var zb in zbs)
                        {
                            isAudit = true;
                            lhzb += zb.LhzbModel.Zbxx.ZBMC + "��";
                            if (!khjgbms.Exists(p => p == zb.JGBM.ToString()))
                            {
                                dwmc += SysBmjgService.GetBmjgName(zb.JGBM.Value) + "��";
                                khjgbms.Add(zb.JGBM.ToString());
                            }
                            yssl += zb.KhJgbmdfbLst.Count(p => p.DFZ == userModel.JGBM.ToString());
                            wcsl += zb.KhJgbmdfbLst.Count(p => p.DFZ == userModel.JGBM.ToString() && p.DFSJ != null);
                        }
                        sb.Append("<td style=\"text-align: left;\">" + lhzb.TrimEnd("��".ToCharArray()) + "</td>");
                        sb.Append("<td style=\"text-align: left;\">" + dwmc.TrimEnd("��".ToCharArray()) + "</td>");
                        sb.Append("<td><span class=\"label label-default\">" + yssl + "</span></td>");
                        sb.Append("<td><span class=\"label label-success\">" + wcsl + "</span></td>");
                        sb.Append("</tr>");
                    }
                    sb.Append("<tr>");
                    //���ָ��
                    var dfzbList = dfzbmxSrv.GetListByKhid(lastKaohe.KHID.ToString()).
                        Where(p => ("," + p.DFZ).Contains("," + userModel.JGBM + ",")).ToList();
                    var dfzbmcLst = dfzbList.DistinctBy(p => p.DfzbModel.Zbxx.ZBMC).ToList();
                    var dfyjzbmcLst = dfzbList.DistinctBy(p => p.DfzbModel.Zbxx.YJZBMC).ToList();
                    for (int i = 0; i < dfyjzbmcLst.Count; i++)
                    {
                        List<string> dfjgbms = new List<string>();
                        sb.Append("<tr>");
                        sb.Append("<td>" + (i + 1) + "</td>");
                        sb.Append("<td>" + dfyjzbmcLst[i].DfzbModel.Zbxx.YJZBMC + "</td>");
                        var zbs = dfzbmcLst.Where(p => p.DfzbModel.Zbxx.YJZBMC == dfyjzbmcLst[i].DfzbModel.Zbxx.YJZBMC).ToList();
                        string dfzb = "";
                        string dwmc = "";
                        int yssl = 0;
                        int wcsl = 0;
                        foreach (var zb in zbs)
                        {
                            isAudit = true;
                            dfzb += zb.DfzbModel.Zbxx.ZBMC + "��";
                            if (!dfjgbms.Exists(p => p == zb.JGBM.ToString()))
                            {
                                dwmc += SysBmjgService.GetBmjgName(zb.JGBM.Value) + "��";
                                dfjgbms.Add(zb.JGBM.ToString());
                            }
                            yssl += zb.KhJgbmdfbLst.Count(p => p.DFZ == userModel.JGBM.ToString());
                            wcsl += zb.KhJgbmdfbLst.Count(p => p.DFZ == userModel.JGBM.ToString() && p.KHDF != null);
                        }
                        sb.Append("<td style=\"text-align: left;\">" + dfzb.TrimEnd("��".ToCharArray()) + "</td>");
                        sb.Append("<td style=\"text-align: left;\">" + dwmc.TrimEnd("��".ToCharArray()) + "</td>");
                        sb.Append("<td><span class=\"label label-default\">" + yssl + "</span></td>");
                        sb.Append("<td><span class=\"label label-success\">" + wcsl + "</span></td>");
                        sb.Append("</tr>");
                    }
                }
                khInfo = (isAudit) ? sb.ToString() : "";
            }
            if (WebKeys.EnableCaching && !string.IsNullOrEmpty(khInfo))
            {
                //���ݴ��뻺��ϵͳ
                CacheHelper.Add(typeof(KhKhglService), true, null, "GetEjdwKaoheInfoByJgbm", 
                    new object[] { userModel }, KhKhglData.CacheClassKey + "_GetEjdwKaoheInfoByJgbm_" + userModel.JGBM, khInfo);
            }
            return khInfo;
        }

        /// <summary>
        /// ��ȡ����һ�ڵĶ�����λ���쵼���ӿ�����Ϣ
        /// </summary>
        /// <returns></returns>
        public string GetEjdwKaoheInfo()
        {
            string khInfo = string.Empty;
            if (WebKeys.EnableCaching)
            {
                khInfo = (string)CacheHelper.GetCache(KhKhglData.CacheClassKey + "_GetEjdwKaoheInfo");
            }
            if (string.IsNullOrEmpty(khInfo))
            {
                StringBuilder sb = new StringBuilder();
                //���һ�ڿ���
                var lastKaohe = GetLastKaoheInfo("LX2014A");//������λ���쵼���ӿ���
                if (lastKaohe != null)
                {
                    sb.Append("<tr>");
                    sb.Append("<th class=\"td-bold\" colspan=\"4\">" + lastKaohe.KHMC + "-��չͳ��</th>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style=\"width: 25%\">������λ</td>");
                    sb.Append(
                        "<td style=\"width: 25%\" class=\"show\">"
                        + "<a href=\"javascript:parent.addTab('/Module/KaoheShowInfo.aspx?KH=" + lastKaohe.KHID + "','���˽�չ���ͳ��1')\">"
                        + "<span class=\"red\">"
                        + GetKaoheProcessResult(lastKaohe, "������λ") + "</span></a></td>");
                    sb.Append("<td style=\"width: 25%\">����ֵȷ��</td>");
                    sb.Append("<td style=\"width: 25%\" class=\"show\">"
                        + "<a href=\"javascript:parent.addTab('/Module/KaoheShowInfo.aspx?KH=" + lastKaohe.KHID + "','���˽�չ���ͳ��1')\">"
                        + "<span class=\"red\">"
                        + GetKaoheProcessResult(lastKaohe, "����ֵȷ��") + "</span></a></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td>�����</td>");
                    sb.Append("<td class=\"show\">"
                        + "<a href=\"javascript:parent.addTab('/Module/KaoheShowInfo.aspx?KH=" + lastKaohe.KHID + "','���˽�չ���ͳ��1')\">"
                        + "<span class=\"green\">"
                        + GetKaoheProcessResult(lastKaohe, "�����") + "</span></a></td>");
                    sb.Append("<td>�ļ��ύ</td>");
                    sb.Append("<td class=\"show\">"
                        + "<a href=\"javascript:parent.addTab('/Module/KaoheShowInfo.aspx?KH=" + lastKaohe.KHID + "','���˽�չ���ͳ��1')\">"
                        + "<span class=\"green\">"
                        + GetKaoheProcessResult(lastKaohe, "�ļ��ύ") + "</span></a></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td>�����</td>");
                    sb.Append("<td class=\"show\">"
                        + "<a href=\"javascript:parent.addTab('/Module/KaoheShowInfo.aspx?KH=" + lastKaohe.KHID + "','���˽�չ���ͳ��1')\">"
                        + "<span class=\"yellow\">"
                        + GetKaoheProcessResult(lastKaohe, "�����") + "</span></a></td>");
                    sb.Append("<td>����</td>");
                    sb.Append("<td class=\"show\">"
                        + "<a href=\"javascript:parent.addTab('/Module/KaoheShowInfo.aspx?KH=" + lastKaohe.KHID + "','���˽�չ���ͳ��1')\">"
                        + "<span class=\"blue\">"
                        + GetKaoheProcessResult(lastKaohe, "����") + "</span></a></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td>���˽���</td>");
                    sb.Append("<td colspan=\"3\">");
                    sb.Append("<div class=\"progress progress-striped progress-success active\">");
                    int processV = GetKaoheProcessValue(lastKaohe);
                    sb.Append("<div class=\"progress-bar\" style=\"width: " + processV
                        + "%;\" onclick=\"ShowProgress('LX2014A','" + processV + "');\">" + processV + "%</div>");
                    sb.Append("</div>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    khInfo = sb.ToString();
                }
            }
            if (WebKeys.EnableCaching && !string.IsNullOrEmpty(khInfo))
            {
                ////���ݴ��뻺��ϵͳ
                //CacheHelper.Add(KhKhglData.CacheClassKey + "_GetEjdwKaoheInfo", khInfo);
                //���ݴ��뻺��ϵͳ
                CacheHelper.Add(typeof(KhKhglService), true, null, "GetEjdwKaoheInfo", null, KhKhglData.CacheClassKey + "_GetEjdwKaoheInfo", khInfo);
            }
            return khInfo;
        }

        /// <summary>
        /// ��ȡ����һ�ڵĻ��ز��ż������˿�����Ϣ
        /// </summary>
        /// <returns></returns>
        public string GetJgbmKaoheInfo()
        {
            string khInfo = string.Empty;
            if (WebKeys.EnableCaching)
            {
                khInfo = (string)CacheHelper.GetCache(KhKhglData.CacheClassKey + "_GetJgbmKaoheInfo");
            }
            if (string.IsNullOrEmpty(khInfo))
            {
                StringBuilder sb = new StringBuilder();
                //���һ�ڿ���
                var lastKaohe = GetLastKaoheInfo("LX2014B");//���ز��ż������˿���
                if (lastKaohe != null)
                {
                    sb.Append("<tr>");
                    sb.Append("<th class=\"td-bold\" colspan=\"4\">" + lastKaohe.KHMC + "-��չͳ��</th>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style=\"width: 25%\">��������</td>");
                    sb.Append("<td style=\"width: 25%\" class=\"show\">"
                            + "<a href=\"javascript:parent.addTab('/Module/KaoheShowInfo.aspx?KH=" + lastKaohe.KHID + "','���˽�չ���ͳ��2')\">"
                            + "<span class=\"red\">"
                            + GetKaoheProcessResult(lastKaohe, "��������") + "</span></a></td>");
                    sb.Append("<td style=\"width: 25%\">�������</td>");
                    sb.Append("<td style=\"width: 25%\" class=\"show\">"
                            + "<a href=\"javascript:parent.addTab('/Module/KaoheShowInfo.aspx?KH=" + lastKaohe.KHID + "','���˽�չ���ͳ��2')\">"
                            + "<span class=\"red\">"
                            + GetKaoheProcessResult(lastKaohe, "�������") + "</span></a></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td>�ϼ�����</td>");
                    sb.Append("<td class=\"show\">"
                            + "<a href=\"javascript:parent.addTab('/Module/KaoheShowInfo.aspx?KH=" + lastKaohe.KHID + "','���˽�չ���ͳ��2')\">"
                            + "<span class=\"green\">"
                            + GetKaoheProcessResult(lastKaohe, "�ϼ�����") + "</span></a></td>");
                    sb.Append("<td>ͬ������</td>");
                    sb.Append("<td class=\"show\">"
                            + "<a href=\"javascript:parent.addTab('/Module/KaoheShowInfo.aspx?KH=" + lastKaohe.KHID + "','���˽�չ���ͳ��2')\">"
                            + "<span class=\"green\">"
                            + GetKaoheProcessResult(lastKaohe, "ͬ������") + "</span></a></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td>�ļ��ύ</td>");
                    sb.Append("<td class=\"show\">"
                            + "<a href=\"javascript:parent.addTab('/Module/KaoheShowInfo.aspx?KH=" + lastKaohe.KHID + "','���˽�չ���ͳ��2')\">"
                            + "<span class=\"yellow\">"
                            + GetKaoheProcessResult(lastKaohe, "�ļ��ύ") + "</span></a></td>");
                    sb.Append("<td>�����</td>");
                    sb.Append("<td class=\"show\">"
                            + "<a href=\"javascript:parent.addTab('/Module/KaoheShowInfo.aspx?KH=" + lastKaohe.KHID + "','���˽�չ���ͳ��2')\">"
                            + "<span class=\"blue\">"
                            + GetKaoheProcessResult(lastKaohe, "�����") + "</span></a></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td>���˽���</td>");
                    sb.Append("<td colspan=\"3\">");
                    sb.Append("<div class=\"progress progress-striped progress-success active\">");
                    int processV = GetKaoheProcessValue(lastKaohe);
                    sb.Append("<div class=\"progress-bar\" style=\"width: " + processV
                        + "%;\" onclick=\"ShowProgress('LX2014B','" + processV + "');\">" + processV + "%</div>");
                    sb.Append("</div>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    khInfo = sb.ToString();
                }
            }
            if (WebKeys.EnableCaching && !string.IsNullOrEmpty(khInfo))
            {
                ////���ݴ��뻺��ϵͳ
                //CacheHelper.Add(KhKhglData.CacheClassKey + "_GetJgbmKaoheInfo", khInfo);
                //���ݴ��뻺��ϵͳ
                CacheHelper.Add(typeof(KhKhglService), true, null, "GetJgbmKaoheInfo", null, KhKhglData.CacheClassKey + "_GetJgbmKaoheInfo", khInfo);
            }
            return khInfo;
        }

        #endregion

        #region ���˽�չͳ�����

        /// <summary>
        /// ��ȡָ�������µĸ�ͳ����Ŀ�Ľ��
        /// </summary>
        /// <param name="kaohe">����ʵ��</param>
        /// <param name="xmmc">ͳ����Ŀ����</param>
        /// <returns></returns>
        public int GetKaoheProcessResult(KhKhglModel kaohe, string xmmc)
        {
            int? r = 0;
            if (WebKeys.EnableCaching)
            {
                r = (int?)CacheHelper.GetCache(KhKhglData.CacheClassKey + "_GetKaoheProcessResult_" + kaohe.KHID + "_" + xmmc);
            }
            if (r == null || r.Value == 0)
            {
                if (kaohe.LXID == "LX2014A")
                {
                    //������λ���쵼����
                    switch (xmmc)
                    {
                        case "������λ":
                            r = sjsbSrv.GetListByKhid(kaohe.KHID.ToString()).Count;
                            break;
                        case "����ֵȷ��"://Ŀ��ֵȷ�����ڲ�Ϊ��
                            r = dlzbmxSrv.GetListByKhid(kaohe.KHID.ToString()).
                                Where(p => p.MBZQRRQ != null).DistinctBy(p => p.JGBM).Count();
                            break;
                        case "�����":
                            r = dlzbmxSrv.GetListByKhid(kaohe.KHID.ToString()).
                                Where(p => p.WCZSQRQ != null).DistinctBy(p => p.JGBM).Count();
                            break;
                        case "�ļ��ύ":
                            r = sjsbSrv.GetListByKhid(kaohe.KHID.ToString()).Where(p => p.SBSJ != null).Count();
                            break;
                        case "�����":
                            r = dlzbmxSrv.GetListByKhid(kaohe.KHID.ToString()).
                                Where(p => p.WCZSHRQ != null).DistinctBy(p => p.JGBM).Count();
                            break;
                        case "����":
                            r = dlzbmxSrv.GetListByKhid(kaohe.KHID.ToString()).
                                Where(p => p.WCZSDRQ != null).DistinctBy(p => p.JGBM).Count();
                            break;
                    }
                }
                else if (kaohe.LXID == "LX2014B")
                {
                    var list = jgbmdfbSrv.GetListByKhid(kaohe.KHID.ToString());
                    //���ز��ż�������
                    switch (xmmc)
                    {
                        case "��������":
                            r = sjsbSrv.GetListByKhid(kaohe.KHID.ToString()).Count;
                            break;
                        case "�������"://�Ӵ�ֱ���ͳ��JGBM��DFZһ�µ�����
                            r = list.Where(p => p.JGBM.ToString() == p.DFZ && p.KHDF != null).DistinctBy(p => p.JGBM).Count();
                            break;
                        case "�ϼ�����"://���������Ϊ1��ͬʱKHDF��Ϊ�յĵ�λ������
                            r = list.Where(p => p.DFZLX == ((int)WebKeys.DFUserType.�ϼ��쵼).ToString() && p.KHDF != null)
                                .DistinctBy(p => p.JGBM).Count();
                            break;
                        case "ͬ������":
                            r = list.Where(p => p.DFZLX == ((int)WebKeys.DFUserType.ͬ��������λ).ToString() && p.KHDF != null)
                                .DistinctBy(p => p.JGBM).Count();
                            break;
                        case "�ļ��ύ":
                            r = sjsbSrv.GetListByKhid(kaohe.KHID.ToString()).Where(p => p.SBSJ != null).Count();
                            break;
                        case "�����":
                            r = jgbmkhdfSrv.GetListByKhid(kaohe.KHID.ToString()).
                                Where(p => !string.IsNullOrEmpty(p.HZBZ)).DistinctBy(p => p.JGBM).Count();
                            break;
                    }
                }
            }
            if (WebKeys.EnableCaching && r.ToInt() > 0)
            {
                //���ݴ��뻺��ϵͳ
                CacheHelper.Add(KhKhglData.CacheClassKey + "_GetKaoheProcessResult_" + kaohe.KHID + "_" + xmmc, r);
            }
            return r.ToInt();
        }

        /// <summary>
        /// ��ȡָ�������ڵĽ���ֵ
        /// </summary>
        /// <param name="kaohe"></param>
        /// <returns></returns>
        public int GetKaoheProcessValue(KhKhglModel kaohe)
        {

            /*
             ��⵱ǰ�Ŀ��˵�����״̬�����Զ�����״̬����ʾ����ֵ
             * 1��������λ����
             *  ��Ч������,20 30 40 50 60 85 100
                Ŀ��ֵȷ��,
                ���ֵ¼��,
                Լ���ͼӷ�ָ����,
                �쵼���Ӵ��,
                ���ֵ�͵÷����,
                ���˽����
             * 2�����ز��ſ���
             *  ��Ч������,20 40 60 80 90 100
                ����������,
                ͬ��������,
                �ϼ�������,
                ����ָ����,
                ���˽����
             */
            int? r = 0;
            if (WebKeys.EnableCaching)
            {
                r = (int?)CacheHelper.GetCache(KhKhglData.CacheClassKey + "_GetKaoheProcessValue_" + kaohe.KHID);
            }
            if (r == null || r.Value == 0)
            {
                if (kaohe.LXID == "LX2014A")
                {
                    //������λ
                    if (jxzrszbSrv.GetListByNdAndBbmc(kaohe.KHND.ToString(), kaohe.BBMC).Count > 0)
                        r = 20;//��Ч������
                    if (dlzbmxSrv.GetListByKhid(kaohe.KHID.ToString()).Where(p => p.MBZQRRQ != null).Count() > 0)
                        r = 30;//Ŀ��ֵȷ��
                    if (dlzbmxSrv.GetListByKhid(kaohe.KHID.ToString()).Where(p => p.WCZSQRQ != null).Count() > 0)
                        r = 40;//���ֵ¼��
                    if (dfzbmxSrv.GetListByKhidAndKhdx(kaohe.KHID.ToString(),
                        WebKeys.KaoheType.������λ).Where(p => p.DFRQ != null).Count() > 0)
                        r = 50;//Լ���ͼӷ�ָ����
                    if (dfzbmxSrv.GetListByKhidAndKhdx(kaohe.KHID.ToString(),
                        WebKeys.KaoheType.�쵼����).Where(p => p.DFRQ != null).Count() > 0)
                        r = 60;//�쵼���Ӵ��
                    if (dlzbmxSrv.GetListByKhid(kaohe.KHID.ToString()).Where(p => p.WCZSHRQ != null).Count() > 0)
                        r = 85;//���ֵ�͵÷����
                    if (ejdwkhdfSrv.GetListByKhid(kaohe.KHID.ToString()).Where(p => !string.IsNullOrEmpty(p.HZBZ)).Count() > 0)
                        r = 100;//���˽����
                }
                else if (kaohe.LXID == "LX2014B")
                {
                    //���ز���
                    //��Ч������,20 40 60 80 90 100
                    if (jxzrszbSrv.GetListByNdAndBbmc(kaohe.KHND.ToString(), kaohe.BBMC).Count > 0)
                        r = 20;//��Ч������20
                    var list = jgbmdfbSrv.GetListByKhid(kaohe.KHID.ToString());
                    if (list.Where(p => p.JGBM.ToString() == p.DFZ && p.KHDF != null).DistinctBy(p => p.JGBM).Count() > 0)
                        r = 40;//����������,40
                    if (list.Where(p => p.DFZLX == ((int)WebKeys.DFUserType.ͬ��������λ).ToString() && p.KHDF != null)
                                .DistinctBy(p => p.JGBM).Count() > 0)
                        r = 60;//ͬ��������,60
                    if (list.Where(p => p.DFZLX == ((int)WebKeys.DFUserType.�ϼ��쵼).ToString() && p.KHDF != null)
                                .DistinctBy(p => p.JGBM).Count() > 0)
                        r = 80;//�ϼ�������,80
                    if (dfzbmxSrv.GetListByKhidAndKhdx(kaohe.KHID.ToString(),
                        WebKeys.KaoheType.���Ÿ�����).Where(p => p.DFRQ != null).Count() > 0)
                        r = 90;//����ָ����,90
                    if (jgbmkhdfSrv.GetListByKhid(kaohe.KHID.ToString()).
                                Where(p => !string.IsNullOrEmpty(p.HZBZ)).DistinctBy(p => p.JGBM).Count() > 0)
                        r = 100;//���˽����100
                }
            }
            if (WebKeys.EnableCaching && r.ToInt() > 0)
            {
                //���ݴ��뻺��ϵͳ
                CacheHelper.Add(KhKhglData.CacheClassKey + "_GetKaoheProcessValue_" + kaohe.KHID, r);
            }
            return r.ToInt();
        }

        #endregion

        #region �Զ��巽��


        /// <summary>
        /// ��ȡָ�������µ����һ�ڿ��˿��ˣ��Կ�ʼʱ��Ϊ׼��
        /// </summary>
        /// <param name="khlx">��������</param>
        /// <returns></returns>
        public KhKhglModel GetLastKaoheInfo(string khlx)
        {
            //string hql = "from KhKhglModel p where p.LXID='" + khlx + "' order by p.KSSJ desc";
            return GetKhListByLxidForIndex(khlx).FirstOrDefault();
        }

        /// <summary>
        /// ��ȡ״̬Ϊ�����кͿ��˿�ʼʱ��Ϊ��ǰ��ȵ����п���
        /// </summary>
        /// <returns></returns>
        public IList<KhKhglModel> GetKhListForIndex()
        {
            List<KhKhglModel> khList = null;
            if (WebKeys.EnableCaching)
            {
                khList = (List<KhKhglModel>)CacheHelper.GetCache(KhKhglData.CacheClassKey + "_GetKhListForIndex");
            }
            if (khList == null || khList.Count == 0)
            {
                string hql = "from KhKhglModel p where p.KHZT='0' or Year(p.KSSJ)='" + DateTime.Now.Year + "' order by p.KSSJ desc";
                khList = GetListByHQL(hql) as List<KhKhglModel>;
                if (khList.Count == 0)
                {
                    //������λ����
                    var ejdwkh = GetList().FirstOrDefault(p => p.LXID == "LX2014A");
                    if (ejdwkh != null)
                    {
                        khList.Add(ejdwkh);
                    }
                    //���ز��ſ���
                    var jgbmkh = GetList().FirstOrDefault(p => p.LXID == "LX2014B");
                    if (jgbmkh != null)
                    {
                        khList.Add(jgbmkh);
                    }
                }
                //���뻺����
                if (WebKeys.EnableCaching && khList.Count > 0)
                {
                    ////���ݴ��뻺��ϵͳ
                    //CacheHelper.Add(KhKhglData.CacheClassKey + "_GetKhListForIndex", khList);
                    //���ݴ��뻺��ϵͳ
                    CacheHelper.Add(typeof(KhKhglService), true, null, "GetKhListForIndex", null, KhKhglData.CacheClassKey + "_GetKhListForIndex", khList);
                }
            }
            return khList;
        }

        /// <summary>
        /// ��ȡ״̬Ϊ�����кͿ��˿�ʼʱ��Ϊ��ǰ��ȵ�ָ���������͵���Ϣ
        /// </summary>
        /// <param name="lxid">����ID</param>
        /// <returns></returns>
        public IList<KhKhglModel> GetKhListByLxidForIndex(string lxid)
        {
            List<KhKhglModel> khList = null;
            if (WebKeys.EnableCaching)
            {
                khList = (List<KhKhglModel>)CacheHelper.GetCache(KhKhglData.CacheClassKey + "_GetKhListByLxidForIndex_" + lxid);
            }
            if (khList == null || khList.Count > 0)
            {
                string hql = "from KhKhglModel p where p.KHZT='0' or Year(p.KSSJ)='" + DateTime.Now.Year + "' order by p.KSSJ desc";
                khList = GetListByHQL(hql).Where(p => p.LXID == lxid).ToList();
                if (khList.Count == 0)
                {
                    if (lxid == "LX2014A")
                    {
                        //������λ����
                        var ejdwkh = GetList().FirstOrDefault(p => p.LXID == "LX2014A");
                        if (ejdwkh != null)
                        {
                            khList.Add(ejdwkh);
                        }
                    }
                    else if (lxid == "LX2014B")
                    {
                        //���ز��ſ���
                        var jgbmkh = GetList().FirstOrDefault(p => p.LXID == "LX2014B");
                        if (jgbmkh != null)
                        {
                            khList.Add(jgbmkh);
                        }
                    }
                }
                //���뻺����
                if (WebKeys.EnableCaching && khList.Count > 0)
                {
                    ////���ݴ��뻺��ϵͳ
                    //CacheHelper.Add(KhKhglData.CacheClassKey + "_GetKhListByLxidForIndex_" + lxid, khList);
                    //���ݴ��뻺��ϵͳ
                    CacheHelper.Add(typeof(KhKhglService), true, null, "GetKhListByLxidForIndex", new object[] { lxid }, KhKhglData.CacheClassKey + "_GetKhListByLxidForIndex_" + lxid, khList);
                }
            }
            return khList;
        }

        /// <summary>
        /// ��ȡָ���汾�µ����п�����Ϣ
        /// </summary>
        /// <param name="zbbb">�汾����</param>
        /// <returns></returns>
        public IList<KhKhglModel> GetKhListByZbbb(string zbbb)
        {
            string hql = "from KhKhglModel p where p.BBMC='" + zbbb + "' order by p.KSSJ desc";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// ��ȡָ������µ����п�����Ϣ
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        public IList<KhKhglModel> GetKhListByYear(string year)
        {
            string hql = "from KhKhglModel p where p.KHND='" + year + "' order by p.KSSJ desc";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// ��ȡ�ɲ�ѯ�����п�����Ϣ
        /// </summary>
        /// <returns></returns>
        public IList<KhKhglModel> GetKhListForValid()
        {
            //״̬Ϊ�ɲ�ѯ�ͽ����еĿ���=��Ч����
            string hql = "from KhKhglModel p where p.SFKC='1' or p.KHZT='0' order by p.KHND desc, p.KSSJ desc";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// �󶨴����鹦�ܵĿ����������ؼ�
        /// </summary>
        /// <param name="ddl">�����ؼ�</param>
        /// <param name="kaoheLst">������</param>
        public void BindSSECDropDownListForKaohe(SSECDropDownList ddl, IList<KhKhglModel> kaoheLst)
        {
            ddl.Items.Clear();
            string khnd = string.Empty;
            foreach (var q in kaoheLst)
            {
                if (string.IsNullOrEmpty(khnd))
                {
                    ddl.Items.Add(new ListItem(q.KHND + "���", "optgroup"));
                    ddl.Items.Add(new ListItem(q.KHMC, q.KHID.ToString()));
                    khnd = q.KHND;
                }
                else if (q.KHND == khnd)
                {
                    ddl.Items.Add(new ListItem(q.KHMC, q.KHID.ToString()));
                }
                else
                {
                    ddl.Items.Add(new ListItem(q.KHND + "���", "optgroup"));
                    ddl.Items.Add(new ListItem(q.KHMC, q.KHID.ToString()));
                    khnd = q.KHND;
                }
            }
            //ddl.Items.Insert(0, new ListItem("���п�����", ""));
        }

        #endregion

        #region ��ʽ���𿼺����ݳ�ʼ��

        /// <summary>
        /// ����ָ��׷�ӵ�ָ���Ŀ����ںͻ�����
        /// </summary>
        /// <param name="khid">������</param>
        /// <param name="jgbm">��������</param>
        public void AppendKaoheZhibiao(string khid, string jgbm)
        {
            SysBmjgService bmjgSrv = new SysBmjgService();
            var bmjgs = bmjgSrv.GetBmjgLisByParentJgbm(jgbm);
            foreach (var bmjg in bmjgs)
            {
                jgbm = bmjg.JGBM.ToString();
                //1==��ȡָ��ɸѡ���е�����
                KhZbsxService zbsxSrv = new KhZbsxService();
                List<KhZbsxModel> zbsxLst = zbsxSrv.GetListByKaoheAndJgbm(khid, jgbm) as List<KhZbsxModel>;
                if (zbsxLst.Count == 0) 
                    continue;
                /*
                 * ˼·������ָ�����ӿ��˱��м����������ݣ�ֻ׷��δ������ָ����    
                 * �漰�������ݱ�
                 * PERFO_KH_UNLOCK ���ݽ�����
                 * PERFO_KH_SJSB �����ļ��ϱ���
                 * PERFO_KH_LHZBJCSJ �������˻������ݱ�
                 * PERFO_KH_JGBMDFB ���ز��Ŵ�ֱ�
                 * PERFO_KH_DLZBMX ����ָ�꿼�˱�
                 * PERFO_KH_DFZBMX ���ָ�꿼�˱�
                 */
                //2==׷�ӳ�ʼ����
                var qUnloack = unlockSrv.GetListByKhid(khid).FirstOrDefault(p => p.JGBM == jgbm.ToInt());
                if (qUnloack == null)
                {
                    //���ݽ�����
                    KhUnlockModel unlockM = new KhUnlockModel();
                    unlockM.DB_Option_Action = WebKeys.InsertAction;
                    unlockM.KHID = khid.ToInt();
                    unlockM.JGBM = jgbm.ToInt();
                    unlockM.SID = CommonTool.GetGuidKey();
                    unlockM.SDBZ = "0";
                    unlockSrv.Execute(unlockM);
                }
                var qSjsb = sjsbSrv.GetListByKhid(khid).FirstOrDefault(p => p.JGBM == jgbm.ToInt());
                if (qSjsb == null)
                {
                    //�����ļ��ϱ���
                    KhSjsbModel sjsbM = new KhSjsbModel();
                    sjsbM.DB_Option_Action = WebKeys.InsertAction;
                    sjsbM.KHID = khid.ToInt();
                    sjsbM.JGBM = jgbm.ToInt();
                    sjsbM.SBZT = "0";
                    sjsbSrv.Execute(sjsbM);
                }
                //3==׷�ӵ������ʹ��ָ�꿼�˱�------------------------------------------------------------
                var dlzbmxList = dlzbmxSrv.GetListByKhidAndJgbm(khid, jgbm).ToList();//����ָ��
                var dfzbmxList = dfzbmxSrv.GetListByKhidAndJgbm(khid, jgbm).ToList();//���ָ��
                foreach (var xszb in zbsxLst)
                {
                    //��ָ��Ϊ�¼�
                    if (!dlzbmxList.Exists(p => p.ZBBM == xszb.SXZBBM) &&
                        !dfzbmxList.Exists(p => p.ZBBM == xszb.SXZBBM))
                    {
                        //����ָ�굽���˱�
                        loadKhZbsxInKaohe(khid, xszb);
                    }
                    else
                    {
                        //ָ���Ѵ��ڣ������ֱ�ͻ������Ƿ��Ѵ�������=======================
                        var dlzb = dlzbmxList.FirstOrDefault(p => p.ZBBM == xszb.SXZBBM);
                        if (dlzb != null)
                        {
                            //����ָ��,������ָ��������ݱ�
                            loadKhZbsxInJcsjData(khid, xszb, dlzb, null);
                        }
                        else
                        {
                            var dfzb = dfzbmxList.FirstOrDefault(p => p.ZBBM == xszb.SXZBBM);
                            if (dfzb != null)
                            {
                                //���ָ�꣬�����ز��Ŵ�ֱ�
                                loadKhZbsxInJcsjData(khid, xszb, null, dfzb);
                            }
                        }
                    }
                }
                Debuger.GetInstance().log("AppendKaoheZhibiao(" + khid + "," + jgbm + ")==>׷�Ӷ����ʹ��ָ�꿼�˱����ݳɹ�!");
            }
        }

        /// <summary>
        /// ���ݿ���IDɾ����ؿ��˱������
        /// </summary>
        /// <param name="khid">����ID</param>
        /// <returns></returns>
        public bool DeleteAllDataByKhid(string khid)
        {
            StringBuilder sqlSB = new StringBuilder();
            //==��ɾ����������
            sqlSB.Append("delete from [PERFO_KH_UNLOCK] where KHID='" + khid + "';");//���ݽ�����
            sqlSB.Append("delete from [PERFO_KH_SJSB] where KHID='" + khid + "';");//�����ļ��ϱ���
            sqlSB.Append("delete from [PERFO_KH_ZZJG] where KHID='" + khid + "';");//���˽����
            sqlSB.Append("delete from [PERFO_KH_ZZJGFILE] where KHID='" + khid + "';");//����ļ���
            sqlSB.Append("delete from [PERFO_KH_TJB] where KHID='" + khid + "';");//���˽���ͳ�Ʊ� 
            sqlSB.Append("delete from [PERFO_KH_LHZBJCSJ] where KHID='" + khid + "';");//�������˻������ݱ�
            sqlSB.Append("delete from [PERFO_KH_JGBMDFB] where KHID='" + khid + "';");//���ز��Ŵ�ֱ�
            sqlSB.Append("delete from [PERFO_KH_DLZBMX] where KHID='" + khid + "';");//����ָ�꿼�˱�
            sqlSB.Append("delete from [PERFO_KH_DFZBMX] where KHID='" + khid + "';");//���ָ�꿼�˱�
            sqlSB.Append("delete from [PERFO_KH_MESSAGE] where KHID='" + khid + "';");//������
            sqlSB.Append("delete from [PERFO_KH_JGZFB] where KHID='" + khid + "';");//���ؽ��������
            sqlSB.Append("delete from [PERFO_KH_JGBMKHDF] where KHID='" + khid + "';");//���ز��ſ��˵÷ֱ�
            sqlSB.Append("delete from [PERFO_KH_EJDWKHDF] where KHID='" + khid + "';");//������λ���˵÷ֱ�
            sqlSB.Append("delete from [PERFO_KH_HBJFGZ] where KHID='" + khid + "';");//�ϲ��Ʒֹ����
            sqlSB.Append("delete from [PERFO_KH_KHDFPXFW] where KHID='" + khid + "';");//���˵÷������Χ��
            sqlSB.Append("delete from [PERFO_KH_NODERUN] where KHID='" + khid + "';");//���˽ڵ����б�
            return dal.ExecuteSQL(sqlSB.ToString());
        }

        /// <summary>
        /// �������˲�������ݵĳ�ʼ��
        /// </summary>
        /// <param name="khid">����ID</param>
        public void LaunchKaohe(string khid)
        {
            //��ɾ������
            if (DeleteAllDataByKhid(khid))
            {
                Debuger.GetInstance().log("LaunchKaohe(" + khid + ")==>1==ɾ���������ݳɹ�!");
            }
            //1==��ȡָ��ɸѡ���е�����
            KhZbsxService zbsxSrv = new KhZbsxService();
            var khModel = GetSingle(khid);
            List<KhZbsxModel> zbsxLst = zbsxSrv.GetListByKaohe(khid) as List<KhZbsxModel>;
            var khBmjg = zbsxLst.Distinct<KhZbsxModel>(new FastPropertyComparer<KhZbsxModel>("JGBM"));
            if (khModel != null && zbsxLst.Count > 0)
            {
                khModel.DB_Option_Action = WebKeys.UpdateAction;
                khModel.KHZT = "0";//������
                khModel.BBMC = zbsxLst.First().JxzrsZb.BBMC;
                Execute(khModel);//���°汾����
            }
            //2==��ӳ�ʼ����
            KhUnlockService unlockSrv = new KhUnlockService();
            KhSjsbService sjsbSrv = new KhSjsbService();
            foreach (var q in khBmjg)
            {
                //bbmc = q.JxzrsZb.BBMC;
                //sznd = q.JxzrsZb.ZSZND.ToRequestString();
                //���ݽ�����
                KhUnlockModel unlockM = new KhUnlockModel();
                unlockM.DB_Option_Action = WebKeys.InsertAction;
                unlockM.KHID = khid.ToInt();
                unlockM.JGBM = q.SXJGBM;
                unlockM.SID = CommonTool.GetGuidKey();
                unlockM.SDBZ = "0";
                unlockSrv.Execute(unlockM);
                //�����ļ��ϱ���
                KhSjsbModel sjsbM = new KhSjsbModel();
                sjsbM.DB_Option_Action = WebKeys.InsertAction;
                sjsbM.KHID = khid.ToInt();
                sjsbM.JGBM = q.SXJGBM;
                sjsbM.SBZT = "0";
                sjsbSrv.Execute(sjsbM);

                //4==��Ӳ����������--------------------------------
                if (khModel != null)
                {
                    var cwjcsjThisYear = cwjcsjSrv.GetListByJgbmAndNF(q.JGBM, khModel.KHND.ToInt());
                    if (cwjcsjThisYear == null || cwjcsjThisYear.Count == 0)
                    {
                        //��ӿ���������
                        cwjcsjSrv.InitJcsjDataBySzndAndJgbm(khModel.KHND.ToInt(), q.JGBM.ToInt());
                    }
                    var cwjcsjPrevYear = cwjcsjSrv.GetListByJgbmAndNF(q.JGBM, khModel.KHND.ToInt() - 1);
                    if (cwjcsjPrevYear == null || cwjcsjPrevYear.Count == 0)
                    {
                        //�����һ������
                        cwjcsjSrv.InitJcsjDataBySzndAndJgbm(khModel.KHND.ToInt() - 1, q.JGBM.ToInt());
                    }
                    Debuger.GetInstance().log("LaunchKaoheByJgbm(" + khid + "," + q.JGBM + ")==>4==��Ӳ���������ݳɹ�!=====");
                    //5==���ʹ�����Ϣ-----------------------------------------------
                    //���𿼺˺󣬻��ز�������
                    if (khModel.LXID == "LX2014B")
                    {
                        //���ز��ſ��ˣ�����
                        var u = userSrv.GetUserListForValid().FirstOrDefault(p => p.JGBM == q.SXJGBM && p.DUTY.Contains("Ա��"));
                        if (u != null)
                        {
                            msgSrv.SendMsgToUers(u.LOGINID, q.SXJGBM,
                            "���ز�������", "��Ҫ�����С�" + khModel.KHMC + "����" + u.Bmjg.JGMC + "����������ֲ���!",
                            string.Format("/Module/Kh/JgbmWczZiping.aspx?BM={0}&KH={1}", u.JGBM, khModel.KHID),
                            "1113", u.JGBM.ToString());
                        }
                    }
                }        

            }
            Debuger.GetInstance().log("LaunchKaohe(" + khid + ")==>2==������ݽ����Ϳ����ļ��ϱ����ݳɹ�!");
            //3==�����ʹ��ָ�꿼�˱�----------------------------------------------------------------------
            //ɸѡ�Ŀ���ָ��
            foreach (var xszb in zbsxLst)
            {
                //����ָ�굽���˱�
                loadKhZbsxInKaohe(khid, xszb);
                #region ע��
                ////�� ����ָ�꿼�˱�
                //if (!string.IsNullOrEmpty(xszb.JxzrsZb.LHZBBM))
                //{
                //    switch (xszb.Kaohe.LXID)
                //    {
                //        case "LX2014A"://������λ���쵼���ӿ���
                //            dlzbmxSrv.LoadZrszbModelInDlzbmx(khid, WebKeys.KaoheType.������λ, xszb);
                //            break;
                //        case "LX2014B"://���ز��ż������˿���
                //            dlzbmxSrv.LoadZrszbModelInDlzbmx(khid, WebKeys.KaoheType.���ز���, xszb);
                //            break;
                //        default:
                //            break;
                //    }
                //}
                ////�� ���ָ�꿼�˱�
                //else if (!string.IsNullOrEmpty(xszb.JxzrsZb.DFZBBM))
                //{

                //    switch (xszb.Kaohe.LXID)
                //    {
                //        case "LX2014A"://������λ���쵼���ӿ���
                //            if (xszb.JxzrsZb.Dfzb.Zbxx.YJZBMC.Contains("�Ӽ���"))
                //            {
                //                //�Ӽ���ֻ�쵼���ӿ���
                //                dfzbmxSrv.LoadZrszbModelInDfzbmx(khid, WebKeys.KaoheType.�쵼����, xszb);
                //            }
                //            else
                //            {
                //                dfzbmxSrv.LoadZrszbModelInDfzbmx(khid, WebKeys.KaoheType.������λ, xszb);
                //                //Լ����ָ���쵼����Ҳ����
                //                if (xszb.JxzrsZb.Dfzb.Zbxx.YJZBMC.Contains("Լ��"))
                //                {
                //                    dfzbmxSrv.LoadZrszbModelInDfzbmx(khid, WebKeys.KaoheType.�쵼����, xszb);
                //                }
                //            }
                //            break;
                //        case "LX2014B"://���ز��ż������˿���
                //            if (xszb.JxzrsZb.Dfzb.Zbxx.ZBMC.Contains("�ܲ�����"))
                //            {
                //                //ֻ���˸�����
                //                dfzbmxSrv.LoadZrszbModelInDfzbmx(khid, WebKeys.KaoheType.���Ÿ�����, xszb);
                //            }
                //            else
                //            {
                //                dfzbmxSrv.LoadZrszbModelInDfzbmx(khid, WebKeys.KaoheType.���ز���, xszb);
                //            }
                //            break;
                //        default:
                //            break;
                //    }
                //}
                #endregion
            }
            Debuger.GetInstance().log("LaunchKaohe(" + khid + ")==>3==��Ӷ����ʹ��ָ�꿼�˱����ݳɹ�!");
            //5==���ʹ�����Ϣ-------------------------------------------------------------
            //���𿼺˺󣬶�����λ���ˣ���ȡ���в���Ŀ��˵�ְ�ܲ��ţ��������Ƿ��ʹ�����Ϣ
            if (khModel != null && khModel.LXID == "LX2014A")
            {
                var lst = jgbmdfbSrv.GetDlzbListByKhid(khModel.KHID.ToString()).
                    Where(p => !string.IsNullOrEmpty(p.ID)).DistinctBy(p=>p.DFZ).ToList();
                foreach (var q in lst)
                {
                    int jgbm = q.DFZ.ToInt();
                    if (jgbm == 0) continue;
                    var u = userSrv.GetUserListForValid().FirstOrDefault(p => p.JGBM == jgbm && p.DUTY.Contains("Ա��"));
                    if (u != null)
                    {
                        msgSrv.SendMsgToUers(u.LOGINID, q.JGBM.Value,
                        "����ֵ�趨", "��Ҫ���趨��" + khModel.KHMC + "���Ĳ��ֵ�λ�Ŀ���ֵ!",
                        string.Format("/Module/Kh/EjdwMbzLuru.aspx?BM={0}&KH={1}", q.JGBM, khModel.KHID),
                        "1011", u.JGBM.ToString());
                    }
                }
            }
        }

        #endregion

        #region �޶������Ϳ����ڵ����ݳ�ʼ��

        /// <summary>
        /// ���ݿ����ںͻ�������ɾ����ؿ��˱������
        /// </summary>
        /// <param name="khid">������</param>
        /// <param name="jgbm">��������</param>
        /// <returns></returns>
        public bool DeleteAllDataByKhidAndJgbm(string khid, string jgbm)
        {
            /*
            * �漰�������ݱ�10��
            * PERFO_KH_UNLOCK ���ݽ�����
            * PERFO_KH_SJSB �����ļ��ϱ���
            * PERFO_KH_LHZBJCSJ �������˻������ݱ�
            * PERFO_KH_JGBMDFB ���ز��Ŵ�ֱ�
            * PERFO_KH_DLZBMX ����ָ�꿼�˱�
            * PERFO_KH_DFZBMX ���ָ�꿼�˱�
            * PERFO_KH_MESSAGE ������Ϣ
            * PERFO_KH_ZZJG ���˽����
            * PERFO_KH_JGZFB ���ؽ��������
            * PERFO_KH_JGBMKHDF ���ز��ſ��˵÷ֱ�
             * PERFO_KH_EJDWKHDF ������λ���˵÷ֱ�
            */
            StringBuilder sqlSB = new StringBuilder();
            //==��ɾ����������
            sqlSB.Append("delete from [PERFO_KH_UNLOCK] where KHID='" + khid + "' and JGBM='" + jgbm + "';");//���ݽ�����
            sqlSB.Append("delete from [PERFO_KH_SJSB] where KHID='" + khid + "' and JGBM='" + jgbm + "';");//�����ļ��ϱ���
            sqlSB.Append("delete from [PERFO_KH_ZZJG] where KHID='" + khid + "' and JGBM='" + jgbm + "';");//���˽����
            sqlSB.Append("delete from [PERFO_KH_LHZBJCSJ] where KHID='" + khid + "' and JGBM='" + jgbm + "';");//�������˻������ݱ�
            sqlSB.Append("delete from [PERFO_KH_JGBMDFB] where KHID='" + khid + "' and JGBM='" + jgbm + "';");//���ز��Ŵ�ֱ�
            sqlSB.Append("delete from [PERFO_KH_DLZBMX] where KHID='" + khid + "' and JGBM='" + jgbm + "';");//����ָ�꿼�˱�
            sqlSB.Append("delete from [PERFO_KH_DFZBMX] where KHID='" + khid + "' and JGBM='" + jgbm + "';");//���ָ�꿼�˱�
            sqlSB.Append("delete from [PERFO_KH_MESSAGE] where KHID='" + khid + "' and FSR='" + jgbm + "';");//������
            sqlSB.Append("delete from [PERFO_KH_JGZFB] where KHID='" + khid + "' and JGBM='" + jgbm + "';");//���ؽ��������
            sqlSB.Append("delete from [PERFO_KH_JGBMKHDF] where KHID='" + khid + "' and JGBM='" + jgbm + "';");//���ز��ſ��˵÷ֱ�
            sqlSB.Append("delete from [PERFO_KH_EJDWKHDF] where KHID='" + khid + "';");//������λ���˵÷ֱ�
            return dal.ExecuteSQL(sqlSB.ToString());
        }

        /// <summary>
        /// ����ָ�������ں�ָ����λ�ļ�Ч����
        /// </summary>
        /// <param name="khid">������</param>
        /// <param name="jgbm">��������</param>
        public void LaunchKaoheByJgbm(string khid, string jgbm)
        {
            //��ɾ������
            if (DeleteAllDataByKhidAndJgbm(khid, jgbm))
            {
                Debuger.GetInstance().log("LaunchKaoheByJgbm(" + khid + "," + jgbm + ")==>1==ɾ���������ݳɹ�!");
            }
            //1==��ȡָ��ɸѡ���е�����
            KhZbsxService zbsxSrv = new KhZbsxService();
            List<KhZbsxModel> zbsxLst = zbsxSrv.GetListByKaoheAndJgbm(khid, jgbm) as List<KhZbsxModel>;
            var khModel = GetSingle(khid);
            if (khModel != null && zbsxLst.Count > 0)
            {
                khModel.DB_Option_Action = WebKeys.UpdateAction;
                khModel.KHZT = "0";//������
                khModel.BBMC = zbsxLst.First().JxzrsZb.BBMC;
                Execute(khModel);//���°汾����
            }
            //2==��ӳ�ʼ����
            KhUnlockService unlockSrv = new KhUnlockService();
            KhSjsbService sjsbSrv = new KhSjsbService();
            //���ݽ�����
            KhUnlockModel unlockM = new KhUnlockModel();
            unlockM.DB_Option_Action = WebKeys.InsertAction;
            unlockM.KHID = khid.ToInt();
            unlockM.JGBM = jgbm.ToInt();
            unlockM.SID = CommonTool.GetGuidKey();
            unlockM.SDBZ = "0";
            unlockSrv.Execute(unlockM);
            //�����ļ��ϱ���
            KhSjsbModel sjsbM = new KhSjsbModel();
            sjsbM.DB_Option_Action = WebKeys.InsertAction;
            sjsbM.KHID = khid.ToInt();
            sjsbM.JGBM = jgbm.ToInt();
            sjsbM.SBZT = "0";
            sjsbSrv.Execute(sjsbM);
            Debuger.GetInstance().log("LaunchKaoheByJgbm(" + khid + "," + jgbm + ")==>2==������ݽ����Ϳ����ļ��ϱ����ݳɹ�!");
            //3==�����ʹ��ָ�꿼�˱�----------------------------------------------------------------------
            //ɸѡ�Ŀ���ָ��
            foreach (var xszb in zbsxLst)
            {
                //����ָ�굽���˱�
                loadKhZbsxInKaohe(khid, xszb);
            }
            Debuger.GetInstance().log("LaunchKaoheByJgbm(" + khid + "," + jgbm + ")==>3==��Ӷ����ʹ��ָ�꿼�˱����ݳɹ�!");
            //4==��Ӳ����������---------------------------------------------------------------------------
            if (khModel != null)
            {
                var cwjcsjThisYear = cwjcsjSrv.GetListByJgbmAndNF(jgbm, khModel.KHND.ToInt());
                if (cwjcsjThisYear == null || cwjcsjThisYear.Count == 0)
                {
                    //��ӿ���������
                    cwjcsjSrv.InitJcsjDataBySzndAndJgbm(khModel.KHND.ToInt(), jgbm.ToInt());
                }
                var cwjcsjPrevYear = cwjcsjSrv.GetListByJgbmAndNF(jgbm, khModel.KHND.ToInt() - 1);
                if (cwjcsjPrevYear == null || cwjcsjPrevYear.Count == 0)
                {
                    //�����һ������
                    cwjcsjSrv.InitJcsjDataBySzndAndJgbm(khModel.KHND.ToInt() - 1, jgbm.ToInt());
                }
            }
            Debuger.GetInstance().log("LaunchKaoheByJgbm(" + khid + "," + jgbm + ")==>4==��Ӳ���������ݳɹ�!");
            //5==���ʹ�����Ϣ-------------------------------------------------------------
            if (khModel != null)
            {
                //���𿼺˺󣬻��ز�������
                if (khModel.LXID == "LX2014B")
                {
                    //���ز��ſ��ˣ�����
                    var u = userSrv.GetUserListForValid().FirstOrDefault(p => p.JGBM.ToString() == jgbm && p.DUTY.Contains("Ա��"));
                    if (u != null)
                    {
                        msgSrv.SendMsgToUers(u.LOGINID, u.JGBM,
                        "���ز�������", "��Ҫ�����С�" + khModel.KHMC + "����" + u.Bmjg.JGMC + "����������ֲ���!",
                        string.Format("/Module/Kh/JgbmWczZiping.aspx?BM={0}&KH={1}", u.JGBM, khModel.KHID),
                        "1113", u.JGBM.ToString());
                    }
                }
                //���𿼺˺󣬶�����λ���ˣ���ȡ���в���Ŀ��˵�ְ�ܲ��ţ��������Ƿ��ʹ�����Ϣ
                else if (khModel.LXID == "LX2014A")
                {
                    var lst = jgbmdfbSrv.GetDlzbListByKhid(khModel.KHID.ToString()).
                        Where(p => !string.IsNullOrEmpty(p.ID)).DistinctBy(p => p.DFZ).ToList();
                    foreach (var q in lst)
                    {
                        var u = userSrv.GetUserListForValid().FirstOrDefault(p => p.JGBM.ToString() == q.DFZ && p.DUTY.Contains("Ա��"));
                        if (u != null)
                        {
                            msgSrv.SendMsgToUers(u.LOGINID, q.JGBM.Value,
                            "����ֵ�趨", "��Ҫ���趨��" + khModel.KHMC + "���Ĳ��ֵ�λ�Ŀ���ֵ!",
                            string.Format("/Module/Kh/EjdwMbzLuru.aspx?BM={0}&KH={1}", q.JGBM, khModel.KHID),
                            "1011", u.JGBM.ToString());
                        }
                    }
                }
            }
        }

        #endregion

        #region ר�÷�����

        /// <summary>
        /// ���ز��Ŵ�ֺ�����ָ�����������Ϣ
        /// </summary>
        /// <param name="khid">������</param>
        /// <param name="xszb">ָ��ɸѡMODEL</param>
        /// <param name="dlzb">����ָ��</param>
        /// <param name="dfzb">���ָ��</param>
        private void loadKhZbsxInJcsjData(string khid, KhZbsxModel xszb, KhDlzbmxModel dlzb, KhDfzbmxModel dfzb)
        {
            //�� ����ָ�꿼�˱�
            if (dlzb != null)
            {
                switch (xszb.Kaohe.LXID)
                {
                    case "LX2014A"://������λ���쵼���ӿ���
                        dlzbmxSrv.LoadZrszbModelInDlzbmxByDlzb(khid, WebKeys.KaoheType.������λ, xszb, dlzb);
                        break;
                    case "LX2014B"://���ز��ż������˿���
                        dlzbmxSrv.LoadZrszbModelInDlzbmxByDlzb(khid, WebKeys.KaoheType.���ز���, xszb, dlzb);
                        break;
                    default:
                        break;
                }
            }
            //�� ���ָ�꿼�˱�
            else if (dfzb != null)
            {
                switch (xszb.Kaohe.LXID)
                {
                    case "LX2014A"://������λ���쵼���ӿ���
                        if (xszb.JxzrsZb.Dfzb.Zbxx.YJZBMC.Contains("�Ӽ���"))
                        {
                            //�Ӽ���ֻ�쵼���ӿ���
                            dfzbmxSrv.LoadZrszbModelInDfzbmxByDfzb(khid, WebKeys.KaoheType.�쵼����, xszb, dfzb);
                        }
                        else
                        {
                            dfzbmxSrv.LoadZrszbModelInDfzbmxByDfzb(khid, WebKeys.KaoheType.������λ, xszb, dfzb);
                            //Լ����ָ���쵼����Ҳ����
                            if (xszb.JxzrsZb.Dfzb.Zbxx.YJZBMC.Contains("Լ��"))
                            {
                                dfzbmxSrv.LoadZrszbModelInDfzbmxByDfzb(khid, WebKeys.KaoheType.�쵼����, xszb, dfzb);
                            }
                        }
                        break;
                    case "LX2014B"://���ز��ż������˿���
                        if (xszb.JxzrsZb.Dfzb.Zbxx.YJZBMC.Contains("����"))
                        {
                            //ֻ���˸�����
                            dfzbmxSrv.LoadZrszbModelInDfzbmxByDfzb(khid, WebKeys.KaoheType.���Ÿ�����, xszb, dfzb);
                        }
                        else
                        {
                            dfzbmxSrv.LoadZrszbModelInDfzbmxByDfzb(khid, WebKeys.KaoheType.���ز���, xszb, dfzb);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// ����ָ���Ŀ���ָ�굽��صĿ��˱���
        /// </summary>
        /// <param name="khid">������</param>
        /// <param name="xszb">ָ��ɸѡMODEL</param>
        private void loadKhZbsxInKaohe(string khid, KhZbsxModel xszb)
        {
            //�� ����ָ�꿼�˱�
            if (!string.IsNullOrEmpty(xszb.JxzrsZb.LHZBBM))
            {
                switch (xszb.Kaohe.LXID)
                {
                    case "LX2014A"://������λ���쵼���ӿ���
                        dlzbmxSrv.LoadZrszbModelInDlzbmx(khid, WebKeys.KaoheType.������λ, xszb);
                        break;
                    case "LX2014B"://���ز��ż������˿���
                        dlzbmxSrv.LoadZrszbModelInDlzbmx(khid, WebKeys.KaoheType.���ز���, xszb);
                        break;
                    default:
                        break;
                }
            }
            //�� ���ָ�꿼�˱�
            else if (!string.IsNullOrEmpty(xszb.JxzrsZb.DFZBBM))
            {
                switch (xszb.Kaohe.LXID)
                {
                    case "LX2014A"://������λ���쵼���ӿ���
                        if (xszb.JxzrsZb.Dfzb.Zbxx.YJZBMC.Contains("�Ӽ���"))
                        {
                            //�Ӽ���ֻ�쵼���ӿ���
                            dfzbmxSrv.LoadZrszbModelInDfzbmx(khid, WebKeys.KaoheType.�쵼����, xszb);
                        }
                        else
                        {
                            dfzbmxSrv.LoadZrszbModelInDfzbmx(khid, WebKeys.KaoheType.������λ, xszb);
                            //Լ����ָ���쵼����Ҳ����
                            if (xszb.JxzrsZb.Dfzb.Zbxx.YJZBMC.Contains("Լ��"))
                            {
                                dfzbmxSrv.LoadZrszbModelInDfzbmx(khid, WebKeys.KaoheType.�쵼����, xszb);
                            }
                        }
                        break;
                    case "LX2014B"://���ز��ż������˿���
                        if (xszb.JxzrsZb.Dfzb.Zbxx.YJZBMC.Contains("����"))
                        {
                            //ֻ���˸�����
                            dfzbmxSrv.LoadZrszbModelInDfzbmx(khid, WebKeys.KaoheType.���Ÿ�����, xszb);
                        }
                        else
                        {
                            dfzbmxSrv.LoadZrszbModelInDfzbmx(khid, WebKeys.KaoheType.���ز���, xszb);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="khid"></param>
        /// <returns></returns>
        public void FinishKaohe(string khid)
        {
            /*
             1�����¶���ָ����󶨱�־��ʱ��
             2������������Ϊ����״̬
             3�����´������������뵱ǰ������ص�δ������Ϣ״̬Ϊ�����
             4�����¿���״̬Ϊ���
             */
            StringBuilder sqls = new StringBuilder();
            string[] cacheKeys = new string[] { 
                Enterprise.Data.Perfo.Kh.KhDlzbmxData.CacheClassKey,
                Enterprise.Data.Perfo.Kh.KhUnlockData.CacheClassKey,
                Enterprise.Data.Perfo.Kh.KhMessageData.CacheClassKey
            };
            sqls.Append("update [PERFO_KH_DLZBMX] set WCZSDRQ=CURRENT_TIMESTAMP where KHID='" + khid + "' and WCZSDRQ is null;");
            sqls.Append("update [PERFO_KH_UNLOCK] set SDBZ='1',TJSJ=CURRENT_TIMESTAMP where KHID='" + khid + "' and SDBZ='0';");
            sqls.Append("update [PERFO_KH_MESSAGE] set DQZT='1',WCRQ=CURRENT_TIMESTAMP where KHID='" + khid + "' and DQZT='0';");
            sqls.Append("update [PERFO_KH_KHGL] set KHZT='1',GBSJ=CURRENT_TIMESTAMP where KHID='" + khid + "' and KHZT='0';");
            if (dal.ExecuteSQL(sqls.ToString()))
            {
                //�建��
                CacheHelper.RemoveCacheForClassKeys(cacheKeys);
            }
        }

        #endregion

        #region ������Ӧ����

        /// <summary>
        /// ˢ�»����е����ݣ�������ҳ�������
        /// </summary>
        public void RefreshCacheDataForIndex()
        {
            KhKindhbService hbSrv = new KhKindhbService();
            hbSrv.GetList();
            KhArticlesService tzSrv = new KhArticlesService();
            tzSrv.GetList();

            GetEjdwKaoheInfo();
            GetJgbmKaoheInfo();
        }

        #endregion

    }

}
