using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Data.Perfo.Kh;
using Enterprise.Model.Perfo.Kh;
using Enterprise.Model.Perfo.Zbk;
using Enterprise.Model.Perfo.Sys;
using Enterprise.Service.Perfo.Sys;
using Enterprise.Service.Perfo.Zbk;

namespace Enterprise.Service.Perfo.Kh
{
	
    /// <summary>
    /// �ļ���:  KhDlzbmxService.cs
    /// ��������: ҵ���߼���-����ָ�꿼�˱����ݴ���
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:45
    /// </summary>
    public class KhDlzbmxService
    {

        /// <summary>
        /// �û���Ϣ--������
        /// </summary>
        SysUserService userSrv = new SysUserService();
        /// <summary>
        /// �������-������
        /// </summary>
        ZbkJsgzService jsgzSrv = new ZbkJsgzService();
        /// <summary>
        /// ���е÷ֵĶ���ָ�����ݼ���
        /// </summary>
        List<KhDlzbmxModel> finishList = new List<KhDlzbmxModel>();

        #region ����������

        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly IKhDlzbmxData dal = new KhDlzbmxData();

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhDlzbmxModel GetSingle(string khid, string key)
        {
            string hql = "from KhDlzbmxModel p where p.KHID='" + khid + "'";
            return GetListByHQL(hql).FirstOrDefault(p => p.ID == key);
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhDlzbmxModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhDlzbmxModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhDlzbmxModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhDlzbmxModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region �Զ��巽����

        /// <summary>
        /// ִ��������ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public bool ExecuteByList(List<KhDlzbmxModel> models)
        {
            bool isOk = false;
            foreach (var m in models)
            {
                isOk = dal.Execute(m);
            }
            return isOk;
        }

        /// <summary>
        /// ����ָ�������µ�����ָ��Ļ�������
        /// </summary>
        /// <param name="khid">����ID</param>
        /// <param name="khType">��������</param>
        /// <param name="zbxsModel">ָ��ɸѡMODEL</param>
        /// <param name="model">����ָ��</param>
        /// <returns></returns>
        public bool LoadZrszbModelInDlzbmxByDlzb(string khid, WebKeys.KaoheType khType, KhZbsxModel zbxsModel, KhDlzbmxModel model)
        {
            KhJgbmdfbService jgbmdfbSrv = new KhJgbmdfbService();//��ֱ�
            KhLhzbjcsjService lhzbjcsjSrv = new KhLhzbjcsjService();//�������ݱ�
            SysBmjgService bmjgSrv = new SysBmjgService();//����

            List<KhJgbmdfbModel> jgbmdfList = model.KhJgbmdfbLst.ToList();
            List<KhLhzbjcsjModel> lhzbjcsjList = model.LhzbjcsjLst.ToList();
            if (zbxsModel.JxzrsZb.Lhzb != null)
            {
                //������ʽ��Ϊ�գ���Ҫ�������ʽ���ݲ�����������ݱ�
                if (!string.IsNullOrEmpty(zbxsModel.JxzrsZb.Lhzb.JSBDS))
                {
                    string bds = zbxsModel.JxzrsZb.Lhzb.JSBDS;
                    bds = bds.Substring(bds.IndexOf('=') + 1);
                    List<string> values = Utility.GetMatchValues(bds, @"\{.*?\}");
                    int index = 1;
                    foreach (var v in values)
                    {
                        KhLhzbjcsjModel jcsjM = new KhLhzbjcsjModel();
                        jcsjM.DB_Option_Action = WebKeys.InsertAction;
                        jcsjM.JCZBID = CommonTool.GetGuidKey(); //����ָ��ID
                        jcsjM.ID = model.ID;//�������˱�ID
                        jcsjM.ZBBM = model.ZBBM;//ָ�����
                        jcsjM.JGBM = model.JGBM;//��������
                        jcsjM.KHID = model.KHID;//����ID
                        jcsjM.ZBDH = v;//ָ�����
                        jcsjM.ZBMC = v.Trim('{', '}');//ָ������
                        jcsjM.XH = index++;
                        if (!lhzbjcsjList.Exists(p => p.ID == model.ID && p.ZBDH == jcsjM.ZBDH))
                            lhzbjcsjList.Add(jcsjM);
                    }
                }

                //Ŀ��ֵ���
                var mbzshrLst = zbxsModel.JxzrsZb.Lhzb.MbzshLst.Where(p => p.LHZBBM == zbxsModel.SXZBBM);
                string mbzshr = "";
                foreach (var shr in mbzshrLst)
                {
                    mbzshr += shr.OPERATOR + ",";
                }
                model.MBZQRR = mbzshr.TrimEnd(',');
                //��ʱ������11.27
                //if (khType == WebKeys.KaoheType.���ز���)
                //{
                //    //Ŀ��ֵȷ����Ĭ��Ϊ�ֹ��쵼
                //    model.MBZQRR = "FGLD";
                //}

                //���ֵ���
                var wczshrLst = zbxsModel.JxzrsZb.Lhzb.WczshdfLst.Where(p => p.LHZBBM == zbxsModel.SXZBBM);
                string wczshr = "";
                foreach (var shr in wczshrLst)
                {
                    wczshr += shr.OPERATOR + ",";
                    //���ֵ�ɶ������ʱ��ͬʱ���ڴ�ֱ���
                    var users = GetUserListByWczshr(shr, model);
                    if (users.Count > 0)
                    {
                        foreach (var u in users)
                        {
                            KhJgbmdfbModel jgbmdfModel = new KhJgbmdfbModel();
                            jgbmdfModel.DB_Option_Action = WebKeys.InsertAction;
                            jgbmdfModel.DFBID = CommonTool.GetGuidKey(); //��ֱ�ID
                            jgbmdfModel.ID = model.ID; //�������˱�ID
                            jgbmdfModel.DFZBID = null;//���ָ��ID
                            jgbmdfModel.ZBBM = model.ZBBM; //ָ�����
                            jgbmdfModel.JGBM = model.JGBM; //��������
                            jgbmdfModel.KHID = model.KHID; //����ID
                            jgbmdfModel.DFZ = u.LOGINID; //����ߣ�ָ���û�
                            if (zbxsModel.JxzrsZb.Lhzb.Zbxx.YJZBMC.Contains("���ز���"))
                            {
                                jgbmdfModel.DFZLX = ((int)WebKeys.DFUserType.�ϼ��쵼).ToString();
                            }
                            else
                            {
                                jgbmdfModel.DFZLX = shr.OPERTYPE; //���������
                            }
                            //jgbmdfModel.DFQZ = shr.OPERQZ.ToDecimal()/users.Count; //���Ȩ�� ���ʱ���ݼ���������и���
                            if (!jgbmdfList.Exists(p => p.JGBM == jgbmdfModel.JGBM && p.ID == jgbmdfModel.ID
                                && p.DFZ == jgbmdfModel.DFZ))
                                jgbmdfList.Add(jgbmdfModel);//�ݴ�
                        }
                    }
                    else
                    {
                        KhJgbmdfbModel jgbmdfModel = new KhJgbmdfbModel();
                        jgbmdfModel.DB_Option_Action = WebKeys.InsertAction;
                        jgbmdfModel.DFBID = CommonTool.GetGuidKey(); //��ֱ�ID
                        jgbmdfModel.ID = model.ID; //�������˱�ID
                        jgbmdfModel.DFZBID = null;//���ָ��ID
                        jgbmdfModel.ZBBM = model.ZBBM; //ָ�����
                        jgbmdfModel.JGBM = model.JGBM; //��������
                        jgbmdfModel.KHID = model.KHID; //����ID
                        //����ʱ��ֱ��ȡ��������
                        jgbmdfModel.DFZ = ((shr.OPERATOR == "ZIPING") ? model.JGBM.ToRequestString() : shr.OPERATOR); //����ߣ��Ե�λID����
                        if (zbxsModel.JxzrsZb.Lhzb.Zbxx.YJZBMC.Contains("���ز���"))
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
                                jgbmdfModel.DFZLX = shr.OPERTYPE; //���������
                            }
                        }
                        else
                        {
                            jgbmdfModel.DFZLX = shr.OPERTYPE; //���������
                        }
                        //jgbmdfModel.DFQZ = shr.OPERQZ; //���Ȩ�� ���ʱ���ݼ���������и���
                        if (!jgbmdfList.Exists(p => p.JGBM == jgbmdfModel.JGBM && p.ID == jgbmdfModel.ID
                                && p.DFZ == jgbmdfModel.DFZ))
                            jgbmdfList.Add(jgbmdfModel);//�ݴ�
                    }
                }
                model.WCZSHR = wczshr.TrimEnd(',');
            }
            model.WCZSQR = zbxsModel.SXJGBM.ToRequestString();
            return Execute(model) &&
                jgbmdfbSrv.ExecuteByList(jgbmdfList) && lhzbjcsjSrv.ExecuteByList(lhzbjcsjList);
        }

        /// <summary>
        /// ����ָ�������µ�����ָ��
        /// </summary>
        /// <param name="khid">����ID</param>
        /// <param name="khType">��������</param>
        /// <param name="zbxsModel">ָ��ɸѡMODEL</param>
        /// <returns></returns>
        public bool LoadZrszbModelInDlzbmx(string khid, WebKeys.KaoheType khType, KhZbsxModel zbxsModel)
        {
            KhJgbmdfbService jgbmdfbSrv = new KhJgbmdfbService();//��ֱ�
            List<KhJgbmdfbModel> jgbmdfList = new List<KhJgbmdfbModel>();
            KhLhzbjcsjService lhzbjcsjSrv = new KhLhzbjcsjService();//�������ݱ�
            List<KhLhzbjcsjModel> lhzbjcsjList = new List<KhLhzbjcsjModel>();
            SysBmjgService bmjgSrv = new SysBmjgService();//����

            KhDlzbmxModel model = new KhDlzbmxModel();
            model.ID = "LH" + CommonTool.GetPkId();
            model.DB_Option_Action = WebKeys.InsertAction;
            model.KHID = khid.ToInt();
            model.JGBM = zbxsModel.SXJGBM;
            model.KHDX = ((int)khType).ToString();
            model.ZBKHZT = "0";
            model.ZBBM = zbxsModel.SXZBBM;
            model.SXID = zbxsModel.SXID;
            model.NCMBZ = zbxsModel.JxzrsZb.ZMBZ;
            //model.MBZBZ = zbxsModel.JxzrsZb.MBZBZ;
            if (zbxsModel.JxzrsZb.Lhzb != null)
            {
                //������ʽ��Ϊ�գ���Ҫ�������ʽ���ݲ�����������ݱ�
                if (!string.IsNullOrEmpty(zbxsModel.JxzrsZb.Lhzb.JSBDS))
                {
                    string bds = zbxsModel.JxzrsZb.Lhzb.JSBDS;
                    bds = bds.Substring(bds.IndexOf('=')+1);
                    List<string> values = Utility.GetMatchValues(bds, @"\{.*?\}");
                    int index = 1;
                    foreach (var v in values)
                    {
                        KhLhzbjcsjModel jcsjM = new KhLhzbjcsjModel();
                        jcsjM.DB_Option_Action = WebKeys.InsertAction;
                        jcsjM.JCZBID = CommonTool.GetGuidKey(); //����ָ��ID
                        jcsjM.ID = model.ID;//�������˱�ID
                        jcsjM.ZBBM = model.ZBBM;//ָ�����
                        jcsjM.JGBM = model.JGBM;//��������
                        jcsjM.KHID = model.KHID;//����ID
                        jcsjM.ZBDH = v;//ָ�����
                        jcsjM.ZBMC = v.Trim('{', '}');//ָ������
                        jcsjM.XH = index++;
                        if (!lhzbjcsjList.Exists(p=>p.ID == model.ID && p.ZBDH == jcsjM.ZBDH))
                            lhzbjcsjList.Add(jcsjM);
                    }
                }

                //Ŀ��ֵ���
                var mbzshrLst = zbxsModel.JxzrsZb.Lhzb.MbzshLst.Where(p=>p.LHZBBM == zbxsModel.SXZBBM);
                string mbzshr = "";
                foreach (var shr in mbzshrLst)
                {
                    mbzshr += shr.OPERATOR + ",";
                }
                model.MBZQRR = mbzshr.TrimEnd(',');
                //��ʱ������11.27
                //if (khType == WebKeys.KaoheType.���ز���)
                //{
                //    //Ŀ��ֵȷ����Ĭ��Ϊ�ֹ��쵼
                //    model.MBZQRR = "FGLD";
                //}

                //���ֵ���
                var wczshrLst = zbxsModel.JxzrsZb.Lhzb.WczshdfLst.Where(p => p.LHZBBM == zbxsModel.SXZBBM);
                string wczshr = "";
                foreach (var shr in wczshrLst)
                {
                    wczshr += shr.OPERATOR + ",";
                    //���ֵ�ɶ������ʱ��ͬʱ���ڴ�ֱ���
                    var users = GetUserListByWczshr(shr, model);
                    if (users.Count > 0)
                    {
                        foreach (var u in users)
                        {
                            KhJgbmdfbModel jgbmdfModel = new KhJgbmdfbModel();
                            jgbmdfModel.DB_Option_Action = WebKeys.InsertAction;
                            jgbmdfModel.DFBID = CommonTool.GetGuidKey(); //��ֱ�ID
                            jgbmdfModel.ID = model.ID; //�������˱�ID
                            jgbmdfModel.DFZBID = null;//���ָ��ID
                            jgbmdfModel.ZBBM = model.ZBBM; //ָ�����
                            jgbmdfModel.JGBM = model.JGBM; //��������
                            jgbmdfModel.KHID = model.KHID; //����ID
                            jgbmdfModel.DFZ = u.LOGINID; //����ߣ�ָ���û�
                            if (zbxsModel.JxzrsZb.Lhzb.Zbxx.YJZBMC.Contains("���ز���"))
                            {
                                jgbmdfModel.DFZLX = ((int)WebKeys.DFUserType.�ϼ��쵼).ToString();
                            }
                            else
                            {
                                jgbmdfModel.DFZLX = shr.OPERTYPE; //���������
                            }
                            //jgbmdfModel.DFQZ = shr.OPERQZ.ToDecimal()/users.Count; //���Ȩ�� ���ʱ���ݼ���������и���
                            if (!jgbmdfList.Exists(p => p.JGBM == jgbmdfModel.JGBM && p.ID == jgbmdfModel.ID 
                                && p.DFZ == jgbmdfModel.DFZ))
                                jgbmdfList.Add(jgbmdfModel);//�ݴ�
                        }
                    }
                    else
                    {
                        KhJgbmdfbModel jgbmdfModel = new KhJgbmdfbModel();
                        jgbmdfModel.DB_Option_Action = WebKeys.InsertAction;
                        jgbmdfModel.DFBID = CommonTool.GetGuidKey(); //��ֱ�ID
                        jgbmdfModel.ID = model.ID; //�������˱�ID
                        jgbmdfModel.DFZBID = null;//���ָ��ID
                        jgbmdfModel.ZBBM = model.ZBBM; //ָ�����
                        jgbmdfModel.JGBM = model.JGBM; //��������
                        jgbmdfModel.KHID = model.KHID; //����ID
                        //����ʱ��ֱ��ȡ��������
                        jgbmdfModel.DFZ = ((shr.OPERATOR == "ZIPING") ? model.JGBM.ToRequestString() : shr.OPERATOR); //����ߣ��Ե�λID����
                        if (zbxsModel.JxzrsZb.Lhzb.Zbxx.YJZBMC.Contains("���ز���"))
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
                                jgbmdfModel.DFZLX = shr.OPERTYPE; //���������
                            }
                        }
                        else
                        {
                            jgbmdfModel.DFZLX = shr.OPERTYPE; //���������
                        }
                        //jgbmdfModel.DFQZ = shr.OPERQZ; //���Ȩ�� ���ʱ���ݼ���������и���
                        if (!jgbmdfList.Exists(p => p.JGBM == jgbmdfModel.JGBM && p.ID == jgbmdfModel.ID
                                && p.DFZ == jgbmdfModel.DFZ))
                            jgbmdfList.Add(jgbmdfModel);//�ݴ�
                    }
                }
                model.WCZSHR = wczshr.TrimEnd(',');
            }
            model.WCZSQR = zbxsModel.SXJGBM.ToRequestString();
            return Execute(model) && 
                jgbmdfbSrv.ExecuteByList(jgbmdfList) && lhzbjcsjSrv.ExecuteByList(lhzbjcsjList);
        }

        /// <summary>
        /// ��ȡ���ֵ����˶�Ӧ�ľ����û���Ϣ����
        /// </summary>
        /// <param name="dfz">���ֵ��˻�����</param>
        /// <param name="dlzbmx">����ָ����ϸ</param>
        /// <returns></returns>
        public IList<SysUserModel> GetUserListByWczshr(ZbkWczshdfModel dfz, KhDlzbmxModel dlzbmx)
        {
            List<SysUserModel> users = new List<SysUserModel>();
            if (dfz.OPERATOR == "YQTLD")
            {
                users = userSrv.GetUserListByDuty("�������쵼") as List<SysUserModel>;
            }
            else if (dfz.OPERATOR == "FGLD")
            {
                var user = userSrv.GetUserByFgjgbm(dlzbmx.JGBM.Value);
                if (user != null)
                {
                    users.Add(user);
                }
            }
            return users;
        }

        /// <summary>
        /// ��ȡָ������ID������ָ����Ϣ
        /// </summary>
        /// <param name="khid">����ID</param>
        /// <returns></returns>
        public IList<KhDlzbmxModel> GetListByKhid(string khid)
        {
            string hql = "from KhDlzbmxModel p where p.KHID='" + khid + "'";
            return GetListByHQL(hql).OrderBy(p=>p.JGBM).ThenBy(p => p.ZbsxModel.SXXH).ToList();
        }

        /// <summary>
        /// ��ȡָ������ID�ͻ��������Ӧ������ָ����Ϣ
        /// </summary>
        /// <param name="khid">����ID</param>
        /// <param name="jgbm">��������</param>
        /// <returns></returns>
        public IList<KhDlzbmxModel> GetListByKhidAndJgbm(string khid, string jgbm)
        {
            string hql = "from KhDlzbmxModel p where p.KHID='" + khid + "' and p.JGBM='" + jgbm + "'";
            return GetListByHQL(hql).OrderBy(p => p.ZbsxModel.SXXH).ToList();
        }

        /// <summary>
        /// ��ȡָ������ID��ָ������Ӧ������ָ����Ϣ
        /// </summary>
        /// <param name="khid">����ID</param>
        /// <param name="zbbm">ָ�����</param>
        /// <returns></returns>
        public IList<KhDlzbmxModel> GetListByKhidAndZbbm(string khid, string zbbm)
        {
            string hql = "from KhDlzbmxModel p where p.KHID='" + khid + "' and p.ZBBM='" + zbbm + "'";
            return GetListByHQL(hql).OrderBy(p => p.ZbsxModel.SXXH).ToList();
        }

        /// <summary>
        /// ���ݼ����ϵʽ�������ָ���ʵ�����ֵ
        /// </summary>
        /// <param name="model">����ָ�����</param>
        /// <param name="glzbs">����ָ�꼯��</param>
        /// <returns></returns>
        public decimal CalculateGlzbsWcz(KhDlzbmxModel model, List<KhLhzbjcsjModel> glzbs)
        {
            decimal result = 0M;
            string bds = model.LhzbModel.JSBDS;
            bds = bds.Substring(bds.IndexOf('=') + 1);
            foreach (var glzb in glzbs)
            {
                bds = bds.Replace(glzb.ZBDH, "(" + glzb.ZBZ.ToDecimal().ToString() + ")");
            }
            result = Utility.Eval(bds).ToDecimal();
            return result;
        }

        /// <summary>
        /// ���ݼ����ϵʽ�������ָ���Ŀ��ֵ
        /// </summary>
        /// <param name="model">����ָ�����</param>
        /// <param name="glzbs">����ָ�꼯��</param>
        /// <returns></returns>
        public decimal CalculateGlzbsMbz(KhDlzbmxModel model, List<KhLhzbjcsjModel> glzbs)
        {
            decimal result = 0M;
            string bds = model.LhzbModel.JSBDS;
            bds = bds.Substring(bds.IndexOf('=') + 1);
            foreach (var glzb in glzbs)
            {
                bds = bds.Replace(glzb.ZBDH, "(" + glzb.ZBSHZ.ToDecimal().ToString() + ")");
            }
            result = Utility.Eval(bds).ToDecimal();
            return result;
        }

        #region ����������ݱ����

        /// <summary>
        /// ��ȡ����������ݱ��е�ָ�������ں͵�λ���롢ָ����ݵ����ݲ����õ���ϸ��
        /// </summary>
        /// <param name="khid">������</param>
        /// <param name="jgbm">��λ����</param>
        /// <param name="yy">���</param>
        /// <returns></returns>
        public bool SetCwjcsjDataByKhid_Jgbm_Year(string khid, string jgbm, int yy)
        {
            /*
             1����ȡָ����λ�Ϳ������µ�����ָ���������
             2����ȡָ����λ������µĲ����������
             3����ȡ�������������ָ����Ķ�Ӧ��ϵ
             4�����ν��������õ�����ָ��������ݱ���
             */
            bool isOk = true;
            KhKhglService khglSrv = new KhKhglService();
            var kaohe = khglSrv.GetSingle(khid);
            if (kaohe == null) return false;

            KhLhzbjcsjService lhzbjcsjSrv = new KhLhzbjcsjService();
            //1==����ָ���������
            List<KhLhzbjcsjModel> jcsjList = lhzbjcsjSrv.GetListByKhidAndJgbm(khid, jgbm) as List<KhLhzbjcsjModel>;

            KhCwjcsjService cwjcsjSrv = new KhCwjcsjService();
            //2==��ȡָ����ݵĲ�������
            var cwjcsjForYear = cwjcsjSrv.GetListByJgbmAndNF(jgbm, yy);

            //3==�������ָ���������
            ZbkCwjcsjglzbService cwjcsjglzbSrv = new ZbkCwjcsjglzbService();
            var cwjcsjGlzbLst = cwjcsjglzbSrv.GetList();

            //4==���ν��������õ�����ָ��������ݱ���
            foreach (KhLhzbjcsjModel jcsj in jcsjList)
            {
                var cwglzb = cwjcsjGlzbLst.FirstOrDefault(p => p.ZBXMC == jcsj.ZBMC && !string.IsNullOrEmpty(p.JCSJZB));
                if (cwglzb != null)
                {
                    KhCwjcsjModel cwjcsj = cwjcsjForYear.FirstOrDefault(p => p.ZBMC == cwglzb.JCSJZB);
                    if (cwjcsj != null)
                    {
                        jcsj.DB_Option_Action = WebKeys.UpdateAction;
                        jcsj.ZBSHZ = getCwsjData(kaohe, cwjcsj);
                        //if (cwjcsj.ZBMC.Contains("�ۼ�"))
                        //{
                        //    //cwjcsj.LJZ.ToDecimal();
                        //}
                        //else if (cwjcsj.ZBMC.Contains("ƽ��"))
                        //{
                        //    jcsj.ZBSHZ = getCwsjData(kaohe, cwjcsj);//cwjcsj.PJZ.ToDecimal();
                        //}
                        jcsj.ZBSHBZ = yy + "������";
                        isOk = lhzbjcsjSrv.Execute(jcsj);
                    }
                }
            }
            return isOk;
        }

        /// <summary>
        /// ��ȡ����������ݱ��е�ָ�������ں͵�λ��������ݲ����õ���ϸ��
        /// </summary>
        /// <param name="khid">������</param>
        /// <param name="jgbm">��λ����</param>
        /// <returns></returns>
        public bool SetCwjcsjDataByKhid_Jgbm(string khid, string jgbm)
        {
            /*
             1����ȡָ����λ�Ϳ������µ�����ָ���������
             2����ȡָ����λ������µĲ����������
             3����ȡ�������������ָ����Ķ�Ӧ��ϵ
             4�����ν��������õ�����ָ��������ݱ���
             */
            bool isOk = true;
            KhKhglService khglSrv = new KhKhglService();
            var kaohe = khglSrv.GetSingle(khid);
            if (kaohe == null) return false;
            
            KhLhzbjcsjService lhzbjcsjSrv = new KhLhzbjcsjService();
            //1==����ָ���������
            List<KhLhzbjcsjModel> jcsjList = lhzbjcsjSrv.GetListByKhidAndJgbm(khid, jgbm) as List<KhLhzbjcsjModel>;
            
            KhCwjcsjService cwjcsjSrv = new KhCwjcsjService();
            //2==������ݵĲ�������
            var cwjcsjForThisYear = cwjcsjSrv.GetListByJgbmAndNF(jgbm, kaohe.KHND.ToInt());
            //2==������һ��Ĳ�������
            var cwjcsjForPrevYear = cwjcsjSrv.GetListByJgbmAndNF(jgbm, kaohe.KHND.ToInt() - 1);
            
            //3==�������ָ���������
            ZbkCwjcsjglzbService cwjcsjglzbSrv = new ZbkCwjcsjglzbService();
            var cwjcsjGlzbLst = cwjcsjglzbSrv.GetList();

            //4==���ν��������õ�����ָ��������ݱ���
            foreach (KhLhzbjcsjModel jcsj in jcsjList)
            {
                var cwglzb = cwjcsjGlzbLst.FirstOrDefault(p => p.ZBXMC == jcsj.ZBMC && !string.IsNullOrEmpty(p.JCSJZB));
                if (cwglzb != null)
                {
                    KhCwjcsjModel cwjcsj = null;
                    if (cwglzb.JCSJLX == "1")
                    {
                        //������ݵĲ�������
                        cwjcsj = cwjcsjForThisYear.FirstOrDefault(p => p.ZBMC == cwglzb.JCSJZB);
                        
                    }
                    else if (cwglzb.JCSJLX == "0")
                    {
                        //������һ��Ĳ�������
                        cwjcsj = cwjcsjForPrevYear.FirstOrDefault(p => p.ZBMC == cwglzb.JCSJZB);
                    }
                    if (cwjcsj != null)
                    {
                        jcsj.DB_Option_Action = WebKeys.UpdateAction;
                        jcsj.ZBZ = getCwsjData(kaohe, cwjcsj);
                        //if (cwjcsj.ZBMC.Contains("�ۼ�")) {
                        //    //cwjcsj.LJZ.ToDecimal();
                        //}
                        //else if (cwjcsj.ZBMC.Contains("ƽ��"))
                        //{
                        //    jcsj.ZBZ = getCwsjData(kaohe, cwjcsj);//cwjcsj.PJZ.ToDecimal();
                        //}
                        isOk = lhzbjcsjSrv.Execute(jcsj);
                    }
                }
            }
            return isOk;
        }

        /// <summary>
        /// ���ݿ��������Զ�������µ��ۼƻ�ƽ������
        /// </summary>
        /// <param name="kaohe">����</param>
        /// <param name="cwjcsj">��������</param>
        /// <returns></returns>
        private decimal getCwsjData(KhKhglModel kaohe, KhCwjcsjModel cwjcsj)
        {
            decimal cwsj = 0M;
            int mStart = 1;
            int mEnd = 12;
            int mCount = 12;
            switch (kaohe.KHZQ)
            {
                case "���":
                    mStart = 1;
                    mEnd = 12;
                    mCount = 12;
                    break;
                case "����":
                    mStart = kaohe.KHJD.ToInt() * 3 - 2;
                    mEnd = kaohe.KHJD.ToInt() * 3;
                    mCount = 3;
                    break;
                case "�¶�":
                    mStart = mEnd = kaohe.KHYD.ToInt();
                    mCount = 1;
                    break;
            }
            //�����ۼ�
            for (int i = mStart; i <= mEnd; i++)
            {
                switch (i)
                {
                    case 1:
                        cwsj += cwjcsj.M1.ToDecimal();
                        break;
                    case 2:
                        cwsj += cwjcsj.M2.ToDecimal();
                        break;
                    case 3:
                        cwsj += cwjcsj.M3.ToDecimal();
                        break;
                    case 4:
                        cwsj += cwjcsj.M4.ToDecimal();
                        break;
                    case 5:
                        cwsj += cwjcsj.M5.ToDecimal();
                        break;
                    case 6:
                        cwsj += cwjcsj.M6.ToDecimal();
                        break;
                    case 7:
                        cwsj += cwjcsj.M7.ToDecimal();
                        break;
                    case 8:
                        cwsj += cwjcsj.M8.ToDecimal();
                        break;
                    case 9:
                        cwsj += cwjcsj.M9.ToDecimal();
                        break;
                    case 10:
                        cwsj += cwjcsj.M10.ToDecimal();
                        break;
                    case 11:
                        cwsj += cwjcsj.M11.ToDecimal();
                        break;
                    case 12:
                        cwsj += cwjcsj.M12.ToDecimal();
                        break;
                }
            }
           
            if (cwjcsj.ZBMC.Contains("ƽ��"))
            {
                //ƽ��
                return cwsj / mCount;
            }
            else
            {
                //�ۼ�
                return cwsj;
            }
        }

        #endregion

        #region ����ʵ�ʵ÷�

        /// <summary>
        /// �������ʵ�ʵ÷ֲ�����
        /// </summary>
        /// <param name="dlzbmxLst">����ָ����ϸ</param>
        /// <param name="isCalculateAll">ȫ�������־</param>
        public void CalDlzbmxSjdfAndSave(List<KhDlzbmxModel> dlzbmxLst, bool isCalculateAll)
        {
            finishList = new List<KhDlzbmxModel>();
            /*
             1���ȷ�������ָ�꣬���ü�����򣬼������ָ��Ȩ�غ��ʵ�ʵ÷�
             2���ٷ�����ָ�꣬������ɸ���ָ�깹�ɣ��򽫸���ָ���ʵ�ʵ÷ְ���ϵʽ�ϳ���Ϊ��ʵ�ʵ÷�
                �����ָ��û�и���ָ�꣬��ֱ�ӵ����������������ٷ��Ƶ÷֣��ټ������Ȩ�غ��ʵ�ʵ÷�
             */
            if (dlzbmxLst.Count == 0) return;
            //��Ӧ�汾�ļ�����򼯺�
            List<ZbkJsgzModel> jsgzList = jsgzSrv.
                GetListByBBMC(dlzbmxLst.First().LhzbModel.BBMC) as List<ZbkJsgzModel>;

            ////1==���㸨��ָ��ĵ÷�
            //var fzzbLst = dlzbmxLst.Where(p => p.ZbsxModel.JxzrsZb.ZZBXZ.Contains("����")).OrderByDescending(p => p.ZbsxModel.JxzrsZb.ZXSXH).ToList();
            //foreach (var fzzb in fzzbLst)
            //{
            //    if (isCalculateAll)
            //    {
            //        //���㸨��ָ��÷�
            //        if (!string.IsNullOrEmpty(fzzb.ZbsxModel.JxzrsZb.ZJSGXS))
            //        {
            //            //�����ϵʽ��Ϊ�գ���÷�ȡ���������¼�ָ��ĵ÷�
            //            var subZbList = dlzbmxLst.Where(p => p.ZbsxModel.JxzrsZb.ZSJZB == fzzb.LhzbModel.LHZBBM).ToList();
            //            KhDlzbmxModel m = calSubZhibiaoDF(fzzb, subZbList, dlzbmxLst, jsgzList, isCalculateAll);
            //            //���뼯��
            //            m.DB_Option_Action = WebKeys.UpdateAction;
            //            if (!finishList.Exists(p => p.ID == m.ID))
            //                finishList.Add(m);
            //        }
            //        else
            //        {
            //            //û�м����ϵʽ������ֱ�Ӽ���÷�
            //            KhDlzbmxModel m = CalculateJsgzByDlzb(fzzb, jsgzList);
            //            //���뼯��
            //            m.DB_Option_Action = WebKeys.UpdateAction;
            //            if (!finishList.Exists(p => p.ID == m.ID))
            //                finishList.Add(m);
            //        }
            //    }
            //    else
            //    {
            //        //����ָ��÷ֲ����¼��㣬��������ֵ
            //        if (!finishList.Exists(p => p.ID == fzzb.ID))
            //            finishList.Add(fzzb);
            //    }
            //}

            //2==������ָ��ĵ÷�,mod by qw 2014.12.20 ͨ����ָ���������Ϳɼ�������е÷�
            var mainZbLst = dlzbmxLst.Where(p => p.ZbsxModel.JxzrsZb.ZZBXZ == "��ָ��").ToList();
            foreach (var zb in mainZbLst)
            {
                if (!string.IsNullOrEmpty(zb.ZbsxModel.JxzrsZb.ZJSGXS))
                {
                    //�����ϵʽ��Ϊ�գ���÷�ȡ���������¼�ָ��ĵ÷�
                    var subZbList = dlzbmxLst.Where(p => p.ZbsxModel.JxzrsZb.ZSJZB == zb.LhzbModel.LHZBBM).ToList();
                    KhDlzbmxModel m = calSubZhibiaoDF(zb, subZbList, dlzbmxLst, jsgzList, zb.IsCalculate);//isCalculateAll
                    //���뼯��
                    m.DB_Option_Action = WebKeys.UpdateAction;
                    if (!finishList.Exists(p => p.ID == m.ID))
                        finishList.Add(m);
                }
                else
                {
                    //û�м����ϵʽ������ֱ�Ӽ���÷�
                    KhDlzbmxModel m = CalculateJsgzByDlzb(zb, jsgzList);
                    //���뼯��
                    m.DB_Option_Action = WebKeys.UpdateAction;
                    if (!finishList.Exists(p => p.ID == m.ID))
                        finishList.Add(m);
                }
            }

            //ִ����������
            ExecuteByList(finishList);
        }

        /// <summary>
        /// ���ݼ���������ʵ�ʵ÷�
        /// </summary>
        /// <param name="fzzb">����ָ��</param>
        /// <param name="jsgzList">���򼯺�</param>
        /// <returns></returns>
        public KhDlzbmxModel CalculateJsgzByDlzb(KhDlzbmxModel fzzb, List<ZbkJsgzModel> jsgzList)
        {
            if (!string.IsNullOrEmpty(fzzb.LhzbModel.GZID))
            {
                var jsgzQ = jsgzList.FirstOrDefault(p => p.GZID == fzzb.LhzbModel.GZID);
                if (jsgzQ != null)
                {
                    if (!string.IsNullOrEmpty(jsgzQ.GZBDS))
                    {
                        //���ʽ
                        string bds = jsgzQ.GZBDS;
                        try
                        {
                            bds = bds.Replace("P", "(" + fzzb.MBZ.ToRequestString() + ")");
                            bds = bds.Replace("F", "(" + fzzb.WCZ.ToRequestString() + ")");
                            decimal? calV = Utility.Eval(bds).ToNullOrDecimal();
                            if (calV != null)
                            {
                                decimal standV = fzzb.ZbsxModel.SXQZ.ToDecimal() * 100;//Ҫ��ɸѡ��Ȩ�ؼ���
                                calV = calV.Value * standV;//����ʵ��ֵ
                                decimal maxV = 0;
                                decimal minV = 0;
                                if (jsgzQ.MAXV != null && jsgzQ.MINV != null)
                                {
                                    //�޶�ֵ
                                    maxV = jsgzQ.MAXV.Value;
                                    minV = jsgzQ.MINV.Value;
                                    if (calV.Value > maxV) calV = maxV;
                                    if (calV.Value < minV) calV = minV;
                                }
                                else if (jsgzQ.UPPER != null && jsgzQ.LOWER != null)
                                {
                                    //��Χֵ
                                    maxV = standV * (1 + jsgzQ.UPPER.Value);
                                    minV = standV * (1 - jsgzQ.LOWER.Value);
                                    if (calV.Value > maxV) calV = maxV;
                                    if (calV.Value < minV) calV = minV;
                                }
                            }
                            //����÷�
                            fzzb.SJDF = calV;
                        }
                        catch (Exception ex)
                        {
                            Debuger.GetInstance().log(this, "CalculateGlzbsWcz����bds=" + bds + "�����˴���!", ex);
                        }

                    }
                    else if (!string.IsNullOrEmpty(jsgzQ.METHODNAME))
                    {
                        //���÷�������
                        string methodName = jsgzQ.METHODNAME;
                        try
                        {
                            JsgzLogicResult result = ZbkJsgzHandler.CallJsgzMethod(fzzb, methodName) as JsgzLogicResult;
                            fzzb.DFJSQK = result.Msg;
                            decimal? calV = result.ReturnValue;
                            if (result.HasValue)
                            {
                                decimal standV = fzzb.ZbsxModel.SXQZ.ToDecimal() * 100;//Ҫ��ɸѡ��Ȩ�ؼ���
                                calV = calV.Value * standV;//����ʵ��ֵ
                                decimal maxV = 0;
                                decimal minV = 0;
                                if (jsgzQ.MAXV != null && jsgzQ.MINV != null)
                                {
                                    //�޶�ֵ
                                    maxV = jsgzQ.MAXV.Value;
                                    minV = jsgzQ.MINV.Value;
                                    if (calV.Value > maxV) calV = maxV;
                                    if (calV.Value < minV) calV = minV;
                                }
                                else if (jsgzQ.UPPER != null && jsgzQ.LOWER != null)
                                {
                                    //��Χֵ
                                    maxV = standV * (1 + jsgzQ.UPPER.Value);
                                    minV = standV * (1 - jsgzQ.LOWER.Value);
                                    if (calV.Value > maxV) calV = maxV;
                                    if (calV.Value < minV) calV = minV;
                                }
                            }
                            //����÷�
                            fzzb.SJDF = calV;
                        }
                        catch (Exception ex)
                        {
                            Debuger.GetInstance().log(this, "CalculateGlzbsWcz����methodName=" + methodName + "�����˴���!", ex);
                        }
                    }
                }
            }

            return fzzb;
        }

        #endregion

        #endregion

        #region ר�÷�����

        /// <summary>
        /// ���������ָ��������¼�ָ��÷�
        /// </summary>
        /// <param name="model">��ָ��</param>
        /// <param name="subZhibiaoLst">�¼�ָ�꼯��</param>
        /// <param name="allZhibiaoLst">����ָ�꼯��</param>
        /// <param name="jsgzList">������򼯺�</param>
        /// <param name="isCalcFzzb">���¼��㸨��ָ���־</param>
        /// <returns></returns>
        private KhDlzbmxModel calSubZhibiaoDF(KhDlzbmxModel model, List<KhDlzbmxModel> subZhibiaoLst,
            List<KhDlzbmxModel> allZhibiaoLst, List<ZbkJsgzModel> jsgzList, bool isCalcFzzb)
        {
            string bds = model.ZbsxModel.JxzrsZb.ZJSGXS;
            bds = bds.Substring(bds.IndexOf('=') + 1);//ֻȡ�Ⱥ��ұ߲���
            foreach (var zb in subZhibiaoLst)
            {
                if (!string.IsNullOrEmpty(zb.ZbsxModel.JxzrsZb.ZJSGXS))
                {
                    //�����ϵʽ��Ϊ�գ���÷�ȡ���������¼�ָ��ĵ÷�
                    var subZbList = allZhibiaoLst.Where(p => p.ZbsxModel.JxzrsZb.ZSJZB == zb.LhzbModel.LHZBBM).ToList();
                    KhDlzbmxModel m = calSubZhibiaoDF(zb, subZbList, allZhibiaoLst, jsgzList, zb.IsCalculate);
                    //�滻���ʽ��Ĵ���
                    bds = bds.Replace(m.ZbsxModel.JxzrsZb.ZZBDH, "(" + m.SJDF.ToRequestString() + ")");
                    //���뼯��
                    m.DB_Option_Action = WebKeys.UpdateAction;
                    if (!finishList.Exists(p=>p.ID == m.ID))
                        finishList.Add(m);
                }
                else
                {
                    //û�м����ϵʽ������ֱ�Ӽ���÷�
                    KhDlzbmxModel m = zb;
                    if (zb.IsCalculate)
                    {
                        m = CalculateJsgzByDlzb(zb, jsgzList);   
                    }
                    //�滻���ʽ��Ĵ���
                    bds = bds.Replace(m.ZbsxModel.JxzrsZb.ZZBDH, "(" + m.SJDF.ToRequestString() + ")");
                    //���뼯��
                    m.DB_Option_Action = WebKeys.UpdateAction;
                    if (!finishList.Exists(p => p.ID == m.ID))
                        finishList.Add(m);
                }
            }
            decimal? calV = Utility.Eval(bds).ToNullOrDecimal();
            if (calV != null)
            {
                //mod by qw 2014.12.20 start �ֹ�˾��ָ���ǽ���ָ���Ȩ�����ηֽ���ȥ�����Կ��Բ����ٳ�һ��Ȩ��
                decimal subQzhj = subZhibiaoLst.Sum(p => p.ZbsxModel.SXQZ).ToDecimal();
                if (model.ZbsxModel.SXQZ != null && subQzhj > 0)
                {
                    if (model.ZbsxModel.SXQZ.Value == subQzhj)
                    {
                        calV = calV.Value;//�����ٳ�Ȩ��
                    }
                    else
                    {
                        calV = calV.Value * model.ZbsxModel.SXQZ.ToDecimal();//�ϼƺ��ָ��ֵҲҪ������Ȩ��
                    }
                }
                //end
                model.SJDF = calV;
                if (model.LhzbModel.Jsgz != null)
                {
                    model = chkKhDlzbmxDfByJsgz(model, model.LhzbModel.Jsgz);//��������򣬶Ե÷ֽ����������
                }
            }        
            return model;
        }

        /// <summary>
        /// ר���ڼ�⵱ǰָ��ĵ÷��Ƿ����趨��Χ��
        /// </summary>
        /// <param name="fzzb">����ָ��MODEL</param>
        /// <param name="jsgzQ">�������MODEL</param>
        /// <returns></returns>
        private KhDlzbmxModel chkKhDlzbmxDfByJsgz(KhDlzbmxModel fzzb, ZbkJsgzModel jsgzQ)
        {
            try
            {
                decimal calV = fzzb.SJDF.Value;//Ŀǰ�ĵ÷֣���Ȩ�غ�ֵ��
                decimal standV = fzzb.ZbsxModel.SXQZ.ToDecimal() * 100;//Ҫ��ɸѡ��Ȩ����Ϊ��׼ֵ
                decimal maxV = 0;
                decimal minV = 0;
                if (jsgzQ.MAXV != null && jsgzQ.MINV != null)
                {
                    //�޶�ֵ
                    maxV = jsgzQ.MAXV.Value;
                    minV = jsgzQ.MINV.Value;
                    if (calV > maxV) calV = maxV;
                    if (calV < minV) calV = minV;
                }
                else if (jsgzQ.UPPER != null && jsgzQ.LOWER != null)
                {
                    //��Χֵ
                    maxV = standV * (1 + jsgzQ.UPPER.Value);
                    minV = standV * (1 - jsgzQ.LOWER.Value);
                    if (calV > maxV) calV = maxV;
                    if (calV < minV) calV = minV;
                }
                //����÷�
                fzzb.SJDF = calV;
            }
            catch (Exception ex)
            {
                Debuger.GetInstance().log(this, "chkKhDlzbmxDfByJsgz���������˴���!", ex);
            }
            return fzzb;
        }

        #endregion
    }

}
