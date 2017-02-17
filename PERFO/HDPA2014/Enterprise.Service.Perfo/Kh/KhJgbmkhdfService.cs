using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Data.Perfo.Kh;
using Enterprise.Model.Perfo.Kh;
using Enterprise.Service.Perfo.Zbk;
using Enterprise.Model.Perfo.Zbk;

namespace Enterprise.Service.Perfo.Kh
{

    /// <summary>
    /// �ļ���:  KhJgbmkhdfService.cs
    /// ��������: ҵ���߼���-���ز��ſ��˵÷ֱ����ݴ���
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/28 16:45:02
    /// </summary>
    public class KhJgbmkhdfService
    {
        #region ����������

        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly IKhJgbmkhdfData dal = new KhJgbmkhdfData();

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhJgbmkhdfModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhJgbmkhdfModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhJgbmkhdfModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhJgbmkhdfModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ����ִ��SQL�ű�
        /// </summary>
        /// <param name="sqls"></param>
        /// <returns></returns>
        public bool ExcuteSQLByList(List<string> sqls)
        {
            bool isOk = true;
            foreach (var sql in sqls)
            {
               isOk = dal.ExecuteSQL(sql);
            }
            return isOk;
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhJgbmkhdfModel model)
        {
            return dal.Execute(model);
        }


        /// <summary>
        /// ִ��ԭ��SQL
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool ExecuteSQL(string sql)
        {
            return dal.ExecuteSQL(sql);
        }

        /// <summary>
        /// ����ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public bool ExecuteByList(List<KhJgbmkhdfModel> models)
        {
            bool isOk = true;
            foreach (var model in models)
            {
               isOk = dal.Execute(model);
            }
            return isOk;
        }

        #endregion

        #region �Զ��巽��

        /// <summary>
        /// ɾ��ָ���������µĵ÷�����
        /// </summary>
        /// <param name="khid">����ID</param>
        /// <returns></returns>
        public bool DeleteJgbmkhdfData(string khid)
        {
            string sql = "delete from [PERFO_KH_JGBMKHDF] where KHID='" + khid + "'";
            return dal.ExecuteSQL(sql);
        }

        /// <summary>
        /// ��ȡָ���������µ����ݼ���
        /// </summary>
        /// <param name="khid">������</param>
        /// <returns></returns>
        public IList<KhJgbmkhdfModel> GetListByKhid(string khid)
        {
            string hql = "from KhJgbmkhdfModel p where p.KHID='" + khid + "' order by p.XMXH";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// ��ȡָ���������ڵ����л��ز��ż������˵÷����ݣ�ȷ����λ��Ψһ��
        /// </summary>
        /// <param name="khid"></param>
        /// <returns></returns>
        public IList<KhJgbmkhdfModel> GetJgbmdfListByKhid(string khid)
        {
            var list = GetListByKhid(khid).Where(p => !string.IsNullOrEmpty(p.HZBZ)).DistinctBy(p => p.JGBM).ToList();
            return list;
        }

        /// <summary>
        /// ��ȡ����λ�Ŀ���ָ�����Ͳ�����÷ֱ�
        /// </summary>
        /// <param name="khid">������</param>
        /// <returns></returns>
        public bool InitJgbmkhdfData(string khid)
        {
            List<KhJgbmkhdfModel> list = new List<KhJgbmkhdfModel>();
            KhDlzbmxService dlzbmxSrv = new KhDlzbmxService();//����ָ������� 
            KhDfzbmxService dfzbmxSrv = new KhDfzbmxService();//���ָ�������
            KhSjsbService sjsbSrv = new KhSjsbService();//�����ϱ�==���˵Ļ����б�
            KhZbsxService zbsxSrv = new KhZbsxService();//ָ��ɸѡ

            var bmjgLst = sjsbSrv.GetListByKhid(khid).OrderBy(p => p.Bmjg.XSXH).ToList();
            foreach (var bmjg in bmjgLst)
            {
                int index = 1;
                //1==�Ӷ���ָ����ϸ��ȡ������ָ��
                var dlzbLst = dlzbmxSrv.GetListByKhidAndJgbm(khid,bmjg.JGBM.ToString()).
                    DistinctBy(p=>p.ZBBM).OrderBy(p=>p.ZbsxModel.SXXH).ToList();
                foreach(var dlzb in dlzbLst) 
                {
                    KhJgbmkhdfModel model = new KhJgbmkhdfModel();
                    model.DB_Option_Action = WebKeys.InsertAction;
                    model.DFID = CommonTool.GetGuidKey();
                    model.JGBM = bmjg.JGBM;
                    model.KHID = bmjg.KHID;
                    //����ָ�꣬ȡָ������
                    model.KHXMC = dlzb.ZbsxModel.JxzrsZb.Lhzb.Zbxx.ZBMC;
                    model.KHLX = ((int)WebKeys.KaoheType.���ز���).ToString();
                    model.XMXH = "01"+ CommonTool.BuZero_3(index++);
                    model.KHDWSL = dlzb.KhJgbmdfbLst.Count;
                    if (!list.Exists(p => p.JGBM == bmjg.JGBM && p.KHXMC == model.KHXMC))
                    {
                        list.Add(model);
                    }
                }

                //2==�Ӵ��ָ����ϸ��ȡ������ָ��
                var dfzbLst = dfzbmxSrv.GetListByKhidAndJgbm(khid, bmjg.JGBM.ToString()).
                    DistinctBy(p => p.ZBBM).OrderBy(p => p.ZbsxModel.SXXH).ToList();
                foreach (var dfzb in dfzbLst)
                {
                    KhJgbmkhdfModel model = new KhJgbmkhdfModel();
                    model.DB_Option_Action = WebKeys.InsertAction;
                    model.DFID = CommonTool.GetGuidKey();
                    model.JGBM = bmjg.JGBM;
                    model.KHID = bmjg.KHID;
                    //���ָ�꣬ȡһ����������
                    model.KHXMC = dfzb.ZbsxModel.JxzrsZb.Dfzb.Zbxx.YJZBMC;
                    if (model.KHXMC.Contains("��������"))
                    {
                        model.KHLX = ((int)WebKeys.KaoheType.���ز���).ToString();
                        model.XMXH = "02" + CommonTool.BuZero_3(index++);
                        model.KHDWSL = dfzb.KhJgbmdfbLst.Count;
                    }
                    else if (model.KHXMC.Contains("���ÿ���"))
                    {
                        model.KHLX = ((int)WebKeys.KaoheType.���ز���).ToString();
                        model.XMXH = "03" + CommonTool.BuZero_3(index++);
                        model.KHDWSL = dfzb.KhJgbmdfbLst.Count;
                    }
                    else if (model.KHXMC.Contains("����"))
                    {
                        model.KHLX = ((int)WebKeys.KaoheType.���Ÿ�����).ToString();
                        model.XMXH = "04" + CommonTool.BuZero_3(index++);
                        model.KHDWSL = dfzb.KhJgbmdfbLst.Count;
                    }
                    if (!list.Exists(p => p.JGBM == bmjg.JGBM && p.KHXMC == model.KHXMC))
                    {
                        list.Add(model);
                    }
                }
            }
            return ExecuteByList(list);
        }

        /// <summary>
        /// ����ָ���������µĸ���λ�÷�
        /// </summary>
        /// <param name="khid">������</param>
        /// <returns></returns>
        public bool CalculateJgbmkhdfData(string khid)
        {
            KhJgzfbService jgzfbSrv = new KhJgzfbService();//�������罨��
            KhDlzbmxService dlzbmxSrv = new KhDlzbmxService();//����ָ����ϸ
            KhDfzbmxService dfzbmxSrv = new KhDfzbmxService();//���ָ����ϸ;
            KhSjsbService sjsbSrv = new KhSjsbService();//�����ϱ�==���˵Ļ����б�
            ZbkDfzbService dfzbSrv = new ZbkDfzbService();//���ָ���
            /*
             1���Ӷ���ָ����ϸ�������ɸ���λָ��ĵ÷֣���������㣩
             2���ӻ������罨����ܱ�����ȡ����
             3���Ӵ��ָ����ϸ������ȡ���Ÿ����˵��ܲ�����ָ������
             4���Ӵ��ָ����ϸ������ȡ�����ŷ��ÿ������������
             */
            StringBuilder sqlSB = new StringBuilder();//���½ű�
            var bmjgLst = sjsbSrv.GetListByKhid(khid).OrderBy(p => p.Bmjg.XSXH).ToList();
            //һ���������λ���ܵ÷�
            foreach (var bmjg in bmjgLst)
            {
                //1==����ָ�������ǲ���һ���Ĺ����ϼ�0.6��ͬ������+����0.2������0.2����Ҫ�ֱ�ͳ��
                var dlzbLst = dlzbmxSrv.GetListByKhidAndJgbm(khid, bmjg.JGBM.ToString());//��λ�µ�����
                foreach (var dlzb in dlzbLst)
                {
                    if (dlzb.KhJgbmdfbLst.Count == 0) 
                        continue;
                    var lddfLst = dlzb.KhJgbmdfbLst.Where(p => p.DFZLX == ((int)WebKeys.DFUserType.�ϼ��쵼).ToString()).ToList();
                    var zpdfLst = dlzb.KhJgbmdfbLst.Where(p => p.DFZLX == ((int)WebKeys.DFUserType.ͬ������).ToString() && 
                        p.DFZ == p.JGBM.ToString()).ToList();
                    var tjdfLst = dlzb.KhJgbmdfbLst.Where(p => p.DFZLX == ((int)WebKeys.DFUserType.ͬ��������λ).ToString() ||
                        (p.DFZLX == ((int)WebKeys.DFUserType.ͬ������).ToString() && p.DFZ != p.JGBM.ToString())).ToList();
                    //����ǰ��λ��xsxh�����ֵ�λ���û����ڵĵ�λ��xsxh���жԱȣ������Ȩ��ֵ
                    string bds = dlzb.ZbsxModel.JxzrsZb.Lhzb.Jsgz.GZBDS;
                    decimal ldqz = 0M;
                    decimal tjqz = 0M;
                    decimal zpqz = 0M;
                    if (!string.IsNullOrEmpty(bds))
                    {
                        string ldBds = bds.Replace("��", "SJLD");
                        ldqz = Utility.Eval(ldBds).ToDecimal();//�ϼ�Ȩ��
                        string tjBds = bds.Replace("��", "TJBM");
                        tjqz = Utility.Eval(tjBds).ToDecimal();//ͬ��Ȩ�� 
                        string zpBds = bds.Replace("��", "ZP");
                        zpqz = Utility.Eval(zpBds).ToDecimal();//����Ȩ��
                    }
                    //�������Ϸֱ���ƽ�����ٰ�Ȩ�����
                    decimal zdf = 0M;
                    if (lddfLst.Count > 0)
                        zdf += lddfLst.Average(p => p.KHDF).ToDecimal() * ldqz;
                    if (zpdfLst.Count > 0)
                        zdf += zpdfLst.Average(p => p.KHDF).ToDecimal() * zpqz;
                    if (tjdfLst.Count > 0)
                        zdf += tjdfLst.Average(p => p.KHDF).ToDecimal() * tjqz;
                    dlzb.BFSFZ = zdf;//��Ȩ�غ���Ϻ��ֵ
                    ////��������ָ�궼�ֱ�����Ȩ�أ��������յ÷֣���Ӧ���������׼��ֵ�İٷֱ��ٳ���Ȩ��
                    //decimal bzf = (dlzb.ZbsxModel.SXFZ.ToDecimal() != 0M) ? dlzb.ZbsxModel.SXFZ.Value : 1M;
                    //zdf = zdf * dlzb.ZbsxModel.SXQZ.ToDecimal();
                    //11.30 Ŀǰ�Ѿ���Ȩ��ֵ��Ϊ��׼�ַ��䵽��ָ�꣬��ʱ�����ٳ���Ȩ����
                    dlzb.SJDF = zdf;//���������÷�(����ͳ��ʱֱ����Ӽ�Ϊʵ�ʵ÷���)
                    dlzb.DB_Option_Action = WebKeys.UpdateAction;
                    dlzbmxSrv.Execute(dlzb);
                }
                //2==��ȡ����ָ��ĵ÷֣�����ص�+������ְ��
                List<KhDlzbmxModel> dlzbmxList = dlzbmxSrv.
                    GetListByKhidAndJgbm(bmjg.KHID.ToString(), bmjg.JGBM.ToString()) as List<KhDlzbmxModel>;
                var oneDlzbLst = dlzbmxList.DistinctBy(p => p.ZBBM).ToList();
                foreach (var dlzb in oneDlzbLst)
                {
                    
                    decimal hjz = dlzbmxList.Where(p => p.ZBBM == dlzb.ZBBM).Sum(p => p.SJDF).ToDecimal();
                    sqlSB.Append("update [PERFO_KH_JGBMKHDF] set KHDF='" + hjz + "' where JGBM='" + dlzb.JGBM + "' and KHID='" 
                        + dlzb.KHID + "' and KHXMC='" + dlzb.ZbsxModel.JxzrsZb.Lhzb.Zbxx.ZBMC + "';");
                }
                //3==�ӻ������罨����ܱ�����ȡ����
                var jgzf = jgzfbSrv.GetListByKhidAndJgbm(bmjg.KHID.ToString(), bmjg.JGBM.ToString()).FirstOrDefault();
                if (jgzf != null)
                {
                    var dfzb = dfzbSrv.GetSingle(jgzf.ZBBM);
                    sqlSB.Append("update [PERFO_KH_JGBMKHDF] set KHDF='" + jgzf.SJDF + "',BZSM='" + jgzf.ZDF 
                        + "' where JGBM='" + jgzf.JGBM + "' and KHID='" + jgzf.KHID + "' and KHXMC='" + dfzb.Zbxx.YJZBMC + "';");
                }
                //4==�Ӵ��ָ����ϸ������ȡ���Ÿ����˵��ܲ�����ָ������
                var zbldzbLst = dfzbmxSrv.GetListByKhidAndKhdx(bmjg.KHID.ToString(), WebKeys.KaoheType.���Ÿ�����).
                    Where(p => p.JGBM == bmjg.JGBM).ToList();
                if (zbldzbLst.Count > 0)
                {
                    var zfModel = zbldzbLst.First();
                    decimal zdf = zbldzbLst.Sum(p => p.SJDF).ToDecimal();
                    sqlSB.Append("update [PERFO_KH_JGBMKHDF] set KHDF='" + zdf + "' where JGBM='"
                        + zfModel.JGBM + "' and KHID='" + zfModel.KHID + "' and KHXMC='"
                            + zfModel.ZbsxModel.JxzrsZb.Dfzb.Zbxx.YJZBMC + "';");
                }
                //5==�Ӵ��ָ����ϸ������ȡ�����ŷ��ÿ������������
                var fykzqk = dfzbmxSrv.GetListByKhidAndKhdx(bmjg.KHID.ToString(), WebKeys.KaoheType.���ز���)
                    .FirstOrDefault(p => p.JGBM == bmjg.JGBM && p.DfzbModel.Zbxx.YJZBMC.Contains("���ÿ���"));
                if (fykzqk != null)
                {
                    sqlSB.Append("update [PERFO_KH_JGBMKHDF] set KHDF='" + fykzqk.SJDF.ToDecimal() + "' where JGBM='"
                        + bmjg.JGBM + "' and KHID='" + bmjg.KHID + "' and KHXMC='"
                            + fykzqk.DfzbModel.Zbxx.YJZBMC + "';");
                }
            }
            bool isResult = dal.ExecuteSQL(sqlSB.ToString());
            //�����������λ���ܵ÷ֺ͸����˵��ܵ÷�
            if (isResult)
            {
                sqlSB = new StringBuilder();
                //��ȡ�������а��������ż������˵�����ָ��ĵ÷�����
                var bmjgkhdfLst = GetListByKhid(khid);
                foreach (var bmjg in bmjgLst)
                {
                    //���ŵ÷�=�ص㹤��+������ְ+��������+���ÿ���
                    decimal bmdf = bmjgkhdfLst.Where(p => p.JGBM == bmjg.JGBM && 
                        p.KHLX == ((int)WebKeys.KaoheType.���ز���).ToString()).Sum(p => p.KHDF).ToDecimal();
                    //������=���ŵ÷�+����ָ��
                    decimal fzrdf = bmjgkhdfLst.Where(p => p.JGBM == bmjg.JGBM &&
                        p.KHLX == ((int)WebKeys.KaoheType.���Ÿ�����).ToString()).Sum(p => p.KHDF).ToDecimal();
                    fzrdf = bmdf + fzrdf;
                    //��¼����SQL
                    sqlSB.Append("update [PERFO_KH_JGBMKHDF] set BMZDF='" + bmdf + "',FZRZDF='" + fzrdf + "' where JGBM='"
                        + bmjg.JGBM + "' and KHID='" + bmjg.KHID + "'");
                }
                //�������µ÷�����
                isResult = dal.ExecuteSQL(sqlSB.ToString());
                if (isResult)
                {
                    //������������ ƽ���� ����ϵ��
                    sqlSB = new StringBuilder();
                    //1=����
                    var oneBmdfLst = GetListByKhid(khid).DistinctBy(p => p.JGBM).OrderByDescending(p => p.BMZDF).ToList();
                    decimal bmpjf = oneBmdfLst.Average(p=>p.BMZDF).ToDecimal();
                    for (int i = 0; i < oneBmdfLst.Count; i++)
                    {
                        if (bmpjf == 0) continue;
                        oneBmdfLst[i].BMPM = (i + 1);
                        oneBmdfLst[i].BMPJF = bmpjf;
                        oneBmdfLst[i].BMDXBS = oneBmdfLst[i].BMZDF / bmpjf;
                        oneBmdfLst[i].DB_Option_Action = WebKeys.UpdateAction;
                        //���沿�ŵ÷�
                        sqlSB.Append("update [PERFO_KH_JGBMKHDF] set BMPM='" + oneBmdfLst[i].BMPM
                            + "',BMPJF='" + oneBmdfLst[i].BMPJF + "',BMDXBS='" + oneBmdfLst[i].BMDXBS + "' where JGBM='"
                        + oneBmdfLst[i].JGBM + "' and KHID='" + oneBmdfLst[i].KHID + "'");
                    }
                    //2==���Ÿ�����
                    var oneBmfzrDfLst = GetListByKhid(khid).DistinctBy(p => p.JGBM).OrderByDescending(p => p.FZRZDF).ToList();
                    decimal bmFzrPjf = oneBmfzrDfLst.Average(p => p.FZRZDF).ToDecimal();
                    for (int i = 0; i < oneBmfzrDfLst.Count; i++)
                    {
                        if (bmFzrPjf == 0) continue;
                        oneBmfzrDfLst[i].FZRPM = (i + 1);
                        oneBmfzrDfLst[i].FZRPJF = bmFzrPjf;
                        oneBmfzrDfLst[i].FZRDXBS = oneBmfzrDfLst[i].FZRZDF / bmFzrPjf;
                        oneBmfzrDfLst[i].DB_Option_Action = WebKeys.UpdateAction;
                        //���渺���˵÷�
                        sqlSB.Append("update [PERFO_KH_JGBMKHDF] set FZRPM='" + oneBmfzrDfLst[i].FZRPM
                            + "',FZRPJF='" + oneBmfzrDfLst[i].FZRPJF + "',FZRDXBS='" + oneBmfzrDfLst[i].FZRDXBS + "' where JGBM='"
                        + oneBmfzrDfLst[i].JGBM + "' and KHID='" + oneBmfzrDfLst[i].KHID + "'");
                    }
                    //����SQL
                    isResult = dal.ExecuteSQL(sqlSB.ToString());
                }
            }
            return isResult;
        }

        #endregion
    }

}
