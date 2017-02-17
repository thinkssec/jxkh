using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Data.Perfo.Kh;
using Enterprise.Model.Perfo.Kh;

namespace Enterprise.Service.Perfo.Kh
{

    /// <summary>
    /// �ļ���:  KhEjdwkhdfService.cs
    /// ��������: ҵ���߼���-������λ���˵÷ֱ����ݴ���
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/12/2 13:41:05
    /// </summary>
    public class KhEjdwkhdfService
    {
        #region ����������

        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly IKhEjdwkhdfData dal = new KhEjdwkhdfData();

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhEjdwkhdfModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhEjdwkhdfModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhEjdwkhdfModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhEjdwkhdfModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhEjdwkhdfModel model)
        {
            return dal.Execute(model);
        }

        /// <summary>
        /// ִ��ԭ��SQL����
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
        /// <param name="t"></param>
        /// <returns></returns>
        public bool ExecuteList(List<KhEjdwkhdfModel> models)
        {
            bool isOk = true;
            foreach (var model in models)
            {
                isOk = dal.Execute(model);
            }
            return isOk;
        }

        #endregion

        #region �Զ��巽����

        /// <summary>
        /// ��ȡָ���������ڵ����ж�����λ���쵼���ӵ÷�����
        /// </summary>
        /// <param name="khid"></param>
        /// <returns></returns>
        public IList<KhEjdwkhdfModel> GetListByKhid(string khid)
        {
            string hql = "from KhEjdwkhdfModel p where p.KHID='" + khid + "' order by p.XMXH";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// ��ȡָ���������ڵ����ж�����λ���쵼���ӵ÷����ݣ�ȷ����λ��Ψһ��
        /// </summary>
        /// <param name="khid"></param>
        /// <returns></returns>
        public IList<KhEjdwkhdfModel> GetEjdwdfListByKhid(string khid)
        {
            var list = GetListByKhid(khid).Where(p => !string.IsNullOrEmpty(p.HZBZ)).DistinctBy(p => p.JGBM).ToList();
            return list;
        }

        /// <summary>
        /// ɾ��ָ���������µĵ÷�����
        /// </summary>
        /// <param name="khid">����ID</param>
        /// <returns></returns>
        public bool DeleteEjdwkhdfData(string khid)
        {
            string sql = "delete from [PERFO_KH_EJDWKHDF] where KHID='" + khid + "'";
            return dal.ExecuteSQL(sql);
        }

        /// <summary>
        /// ���ָ���������µ����ݳ�ʼ��
        /// </summary>
        /// <param name="khid">����ID</param>
        /// <returns></returns>
        public bool InitEjdwkhdfData(string khid)
        {
            List<KhEjdwkhdfModel> dataLst = new List<KhEjdwkhdfModel>();
            /*
             ˼·��
             1����ȡ����λ�Ķ���ָ�����ݣ���ָ���һ�����Ʒ�����ܳ����ݼ�����ӵ��÷ֱ�
             2����ȡ����λ�Ĵ��ָ�����ݣ���ָ���һ�����Ʒ�����ܳ����ݼ�����ӵ��÷ֱ�
             3����ȡ�����ϱ����е����ݣ�����λ��Ϣ�ֱ���ȡ��Ӧ������
             4����ȡ�ϲ�����ĵ�λ����Ӻϲ������¼
             */
            KhKhdfpxfwService pxfwSrv = new KhKhdfpxfwService();//����Χ
            KhNdxsService ndxsSrv = new KhNdxsService();//��Ӫ�Ѷ�ϵ��
            //����ָ��
            KhDlzbmxService dlzbmxSrv = new KhDlzbmxService();
            //���ָ��
            KhDfzbmxService dfzbmxSrv = new KhDfzbmxService();
            //�ϲ��Ʒֹ���
            KhHbjfgzService hbjfgzSrv = new KhHbjfgzService();
            var hbjfgzLst = hbjfgzSrv.GetListByKhid(khid) as List<KhHbjfgzModel>;

            //�����ϱ���λ
            KhSjsbService sjsbSrv = new KhSjsbService();
            var bmjgs = sjsbSrv.GetListByKhid(khid).OrderBy(p => p.Bmjg.XSXH).ToList();
            foreach (var bmjg in bmjgs)
            {
                int index = 1;
                //��ȡ�õ�λ�ĺϲ��Ʒֹ���
                var hbjfModel = hbjfgzLst.FirstOrDefault(p => p.HBJFDW.Contains("," + bmjg.JGBM + ","));
                //1==��ȡָ����λ�Ķ���ָ������
                var dlzbLst = dlzbmxSrv.GetListByKhidAndJgbm(khid, bmjg.JGBM.ToString());
                //��ָ���һ�����������γ����ݼ�
                var dlzbByYjzbmcLst = dlzbLst.DistinctBy(p => p.ZbsxModel.JxzrsZb.Lhzb.Zbxx.YJZBMC).
                    OrderBy(p => p.ZbsxModel.SXXH).ToList();
                foreach (var dlzb in dlzbByYjzbmcLst)
                {
                    KhEjdwkhdfModel khdfModel = new KhEjdwkhdfModel();
                    khdfModel.DB_Option_Action = WebKeys.InsertAction;
                    khdfModel.DFID = CommonTool.GetGuidKey();
                    khdfModel.JGBM = bmjg.JGBM;
                    khdfModel.KHID = bmjg.KHID;
                    khdfModel.KHXMC = dlzb.ZbsxModel.JxzrsZb.Lhzb.Zbxx.YJZBMC;
                    khdfModel.KHLX = dlzb.KHDX;
                    khdfModel.XMXH = "01" + CommonTool.BuZero_3(index++);
                    khdfModel.JGMC = bmjg.Bmjg.JGMC;
                    khdfModel.GSDWMC = pxfwSrv.GetGsdwmc(khdfModel.KHID.ToString(), bmjg.JGBM, khdfModel.KHLX);
                    khdfModel.HBJFID = (hbjfModel != null) ? hbjfModel.HBJFID : null;
                    var yjzbmcList = dlzbLst.Where(p => p.ZbsxModel.JxzrsZb.Lhzb.Zbxx.YJZBMC == khdfModel.KHXMC
                        && p.ZbsxModel.JxzrsZb.ZZBXZ == "��ָ��").ToList();
                    khdfModel.KHBZF = yjzbmcList.Sum(p => p.ZbsxModel.SXQZ) * 100;
                    khdfModel.KHDF = yjzbmcList.Sum(p => p.SJDF);
                    if (!dataLst.Exists(p => p.KHID == khdfModel.KHID && p.KHLX == khdfModel.KHLX && 
                        p.JGBM == khdfModel.JGBM && p.KHXMC == khdfModel.KHXMC))
                    {
                        dataLst.Add(khdfModel);
                    }
                }
                //2==��ȡָ����λ�Ĵ��ָ������
                var ejdwDfzbLst = dfzbmxSrv.GetListByKhidAndKhdx(khid, WebKeys.KaoheType.������λ).Where(p=>p.JGBM==bmjg.JGBM).ToList();
                var dfzbByYjzbmcLst = ejdwDfzbLst.DistinctBy(p => p.ZbsxModel.JxzrsZb.Dfzb.Zbxx.YJZBMC).
                    OrderBy(p => p.ZbsxModel.SXXH).ToList();
                foreach (var dfzb in dfzbByYjzbmcLst)
                {
                    KhEjdwkhdfModel khdfModel = new KhEjdwkhdfModel();
                    khdfModel.DB_Option_Action = WebKeys.InsertAction;
                    khdfModel.DFID = CommonTool.GetGuidKey();
                    khdfModel.JGBM = bmjg.JGBM;
                    khdfModel.KHID = bmjg.KHID;
                    khdfModel.KHXMC = dfzb.ZbsxModel.JxzrsZb.Dfzb.Zbxx.YJZBMC;
                    khdfModel.KHLX = dfzb.KHDX;
                    khdfModel.XMXH = "02" + CommonTool.BuZero_3(index++);
                    khdfModel.JGMC = bmjg.Bmjg.JGMC;
                    khdfModel.GSDWMC = pxfwSrv.GetGsdwmc(khdfModel.KHID.ToString(), bmjg.JGBM, khdfModel.KHLX);
                    khdfModel.HBJFID = (hbjfModel != null) ? hbjfModel.HBJFID : null;
                    khdfModel.BZSM = dfzb.DFBZ;
                    var yjzbmcList = ejdwDfzbLst.Where(p => p.ZbsxModel.JxzrsZb.Dfzb.Zbxx.YJZBMC == khdfModel.KHXMC).ToList();
                    khdfModel.KHDF = yjzbmcList.Sum(p => p.SJDF);
                    if (!dataLst.Exists(p => p.KHID == khdfModel.KHID && p.KHLX == khdfModel.KHLX &&
                        p.JGBM == khdfModel.JGBM && p.KHXMC == khdfModel.KHXMC))
                    {
                        dataLst.Add(khdfModel);
                    }
                }

                //3==��ȡ���е�λ�쵼���ӵĴ��ָ��
                var ldbzDfzbLst = dfzbmxSrv.GetListByKhidAndKhdx(khid, WebKeys.KaoheType.�쵼����).Where(p => p.JGBM == bmjg.JGBM).ToList();
                var ldzbDfzbByYjzbmcLst = ldbzDfzbLst.DistinctBy(p => p.ZbsxModel.JxzrsZb.Dfzb.Zbxx.YJZBMC).
                    OrderBy(p => p.ZbsxModel.SXXH).ToList();
                foreach (var ldbzDF in ldzbDfzbByYjzbmcLst)
                {
                    KhEjdwkhdfModel khdfModel = new KhEjdwkhdfModel();
                    khdfModel.DB_Option_Action = WebKeys.InsertAction;
                    khdfModel.DFID = CommonTool.GetGuidKey();
                    khdfModel.JGBM = bmjg.JGBM;
                    khdfModel.KHID = bmjg.KHID;
                    khdfModel.KHXMC = ldbzDF.ZbsxModel.JxzrsZb.Dfzb.Zbxx.YJZBMC;
                    khdfModel.KHLX = ldbzDF.KHDX;
                    khdfModel.XMXH = "03" + CommonTool.BuZero_3(index++);
                    khdfModel.BZSM = ldbzDF.YSSM;//Լ��˵��
                    khdfModel.JGMC = bmjg.Bmjg.JGMC;
                    khdfModel.NDXS = ndxsSrv.GetNdxsByKhidAndJgbm(bmjg.KHID.ToString(), bmjg.JGBM);//��Ӫ�Ѷ�ϵ��
                    khdfModel.GSDWMC = pxfwSrv.GetGsdwmc(khdfModel.KHID.ToString(), bmjg.JGBM, khdfModel.KHLX);
                    khdfModel.HBJFID = (hbjfModel != null) ? hbjfModel.HBJFID : null;
                    var yjzbmcList = ldbzDfzbLst.Where(p => p.ZbsxModel.JxzrsZb.Dfzb.Zbxx.YJZBMC == khdfModel.KHXMC).ToList();
                    khdfModel.KHDF = yjzbmcList.Sum(p => p.SJDF);
                    if (!dataLst.Exists(p => p.KHID == khdfModel.KHID && p.KHLX == khdfModel.KHLX &&
                        p.JGBM == khdfModel.JGBM && p.KHXMC == khdfModel.KHXMC))
                    {
                        dataLst.Add(khdfModel);
                    }
                }

                //4==�����õ�λ�Ƿ�Ϊ�ϲ��Ʒֵ�λ������ǲ�����
                if (hbjfModel != null)
                {
                    KhEjdwkhdfModel khdfModel = new KhEjdwkhdfModel();
                    khdfModel.DB_Option_Action = WebKeys.InsertAction;
                    khdfModel.DFID = CommonTool.GetGuidKey();
                    khdfModel.KHID = bmjg.KHID;
                    khdfModel.KHLX = ((int)WebKeys.KaoheType.������λ).ToString();
                    khdfModel.XMXH = "00" + CommonTool.BuZero_3(index++);
                    khdfModel.HBJFID = khdfModel.KHXMC = khdfModel.JGMC = hbjfModel.HBJFID;
                    khdfModel.GSDWMC = pxfwSrv.GetGsdwmc(khdfModel.KHID.ToString(), bmjg.JGBM, khdfModel.KHLX);
                    khdfModel.ISHBJF = "1";
                    if (!dataLst.Exists(p => p.KHID == hbjfModel.KHID && p.KHXMC == khdfModel.KHXMC))
                    {
                        dataLst.Add(khdfModel);
                    }
                }
            }
            return ExecuteList(dataLst);
        }


        /// <summary>
        /// ����ָ���������µ��쵼���ӿ��˵÷�
        /// </summary>
        /// <param name="khid">����ID</param>
        /// <returns></returns>
        public bool CalculateEjdwldbzKhdfData(string khid)
        {
            /*
             ˼·��
             * 1������Ҫ�����������λ�ĵ÷�������
             * 2���Ӷ�����λ���˽��������ȡ���˶���Ϊ������λ���쵼���ӵ�����
             * 3����ȡ�ϲ��Ʒֹ���
             * 4�����¼���ϲ��Ʒֵĵ�λ�ĵ÷�����
             * 5�����ݵ�ǰ���˵ĵ�λ����Ҫ�󣬶Ե�λ�������н��и���
             */
            bool isOk = true;
            KhKhdfpxfwService pxfwSrv = new KhKhdfpxfwService();//����Χ
            KhHbjfgzService hbjfgzSrv = new KhHbjfgzService();//�ϲ��Ʒֹ���
            KhCjqjService cjqjSrv = new KhCjqjService();//�ɼ�����
            StringBuilder sqls = new StringBuilder();
            string updSql = string.Empty;
            var allDataList = GetListByKhid(khid);//ȫ������=������λ+�쵼����
            var dataList = allDataList.Where(p => p.KHLX == ((int)WebKeys.KaoheType.�쵼����).ToString()).ToList();
            //1==��������Ʒֵ�λ�� �÷�=(Ч����+������)*��Ӫ�Ѷ�ϵ��+Լ���۷�+�Ӽ���------------------------------
            var oneEjdwLst = dataList.Where(p => p.JGBM != null && p.HBJFID == null).DistinctBy(p => p.JGBM).ToList();
            foreach (var ejdw in oneEjdwLst)
            {
                decimal dwFzrZdf = 0M;//�ܵ÷�
                decimal jia_jian_df = 0M;//Լ����Ӽ��ֵ÷�
                decimal xyl_gll_df = 0M;//����ָ��÷�
                decimal ndxs = (ejdw.NDXS.ToDecimal() > 0) ? ejdw.NDXS.Value : 1.0M;
                //Ч����---������λ
                var xylzb = allDataList.Where(p =>p.KHLX == ((int)WebKeys.KaoheType.������λ).ToString() 
                    && p.JGBM == ejdw.JGBM && p.KHXMC == "Ч����").FirstOrDefault();
                if (xylzb != null)
                {
                    xyl_gll_df += xylzb.KHDF.ToDecimal();
                }
                //������---������λ
                var gllzb = allDataList.Where(p => p.KHLX == ((int)WebKeys.KaoheType.������λ).ToString()
                    && p.JGBM == ejdw.JGBM && p.KHXMC == "������").FirstOrDefault();
                if (gllzb != null)
                {
                    xyl_gll_df += gllzb.KHDF.ToDecimal();
                }
                //Լ���۷�
                var ysxzb = dataList.Where(p => p.JGBM == ejdw.JGBM && p.KHXMC == "Լ����ָ��").FirstOrDefault();
                if (ysxzb != null)
                {
                    jia_jian_df += ysxzb.KHDF.ToDecimal();
                }
                //�Ӽ�������
                var jfzb = dataList.Where(p => p.JGBM == ejdw.JGBM && p.KHXMC == "�Ӽ�������").FirstOrDefault();
                if (jfzb != null)
                {
                    jia_jian_df += jfzb.KHDF.ToDecimal();
                }
                dwFzrZdf = xyl_gll_df * ndxs + jia_jian_df;//�ܵ÷�
                updSql = 
                    string.Format("update [PERFO_KH_EJDWKHDF] set FZRZDF='{0}' where KHID='{1}' and JGBM='{2}';",
                    dwFzrZdf, ejdw.KHID, ejdw.JGBM);
                sqls.Append(updSql);
            }
            if (!string.IsNullOrEmpty(sqls.ToString()))
                isOk = ExecuteSQL(sqls.ToString());
            if (isOk)
            {
                //2==����ϲ��Ʒֵĵ�λ�ĵ÷�--------------------------------------------------------------------
                List<string> hbjfmcLst = new List<string>();
                var hbjfEjdwLst = dataList.Where(p => p.JGBM != null && p.HBJFID != null).DistinctBy(p => p.JGBM).ToList();
                sqls = new StringBuilder();
                foreach (var ejdw in hbjfEjdwLst)
                {
                    //�ϲ�����÷�
                    if (ejdw.Hbjf != null && ejdw.Hbjf.Jsgz != null && !hbjfmcLst.Exists(p => p == ejdw.Hbjf.HBJFID))
                    {
                        decimal hjdwZdf = 0M;//�ܵ÷�
                        decimal hjdwDlzb = 0M;//����ָ��ϼ�
                        decimal hjdwDfzbJiaFen = 0M;//���ָ��ϼ� �ӷ�
                        decimal hjdwDfzbJianFen = 0M;//���ָ��ϼ� ����
                        decimal ndxs = (ejdw.NDXS.ToDecimal() > 0) ? ejdw.NDXS.Value : 1.0M;
                        string[] jgbms = ejdw.Hbjf.HBJFDW.TrimStart(',').TrimEnd(',').Split(',');
                        foreach (var jgbm in jgbms)
                        {
                            string bds = ejdw.Hbjf.Jsgz.GZBDS.Replace("��", jgbm);
                            decimal qz = Utility.Eval(bds).ToDecimal();
                            hjdwDlzb += allDataList.
                                Where(p => p.JGBM == jgbm.ToInt() && (p.KHXMC == "Ч����" || p.KHXMC == "������")).Sum(p => p.KHDF).ToDecimal() * qz;//�������λһ��
                            hjdwDfzbJiaFen += dataList.
                                Where(p => p.JGBM == jgbm.ToInt() && (p.KHXMC == "�Ӽ�������")).Sum(p => p.KHDF).ToDecimal();
                            hjdwDfzbJianFen += dataList.
                                Where(p => p.JGBM == jgbm.ToInt() && (p.KHXMC == "Լ����ָ��")).Sum(p => p.KHDF).ToDecimal();
                        }

                        hjdwZdf = hjdwDlzb * ndxs + hjdwDfzbJiaFen + hjdwDfzbJianFen;//�ܵ÷�
                        hbjfmcLst.Add(ejdw.Hbjf.HBJFID);//����ֻһ�ξ��ܰ���صĵ÷ֶ�����������԰�ʣ�����ص�λ���˵�

                        //�����⼸����λ���ܵ÷� �����־
                        //��1��==��λ��ֻ���°����ܵ÷�
                        updSql = string.Format(
                            "update [PERFO_KH_EJDWKHDF] set FZRZDF='{0}' where KHID='{1}' and JGBM in ({2});",
                            hjdwZdf, ejdw.KHID, ejdw.Hbjf.HBJFDW.TrimStart(',').TrimEnd(','));
                        sqls.Append(updSql);
                        //��2��==�ϲ��Ʒ���, ���� ������ܵ÷� �ۺϵ÷� �ӷ�+����
                        updSql = string.Format(
                            "update [PERFO_KH_EJDWKHDF] set FZRZDF='{0}',FZRPJF='{1}',KHBZF='{2}',NDXS='{5}' where KHID='{3}' and KHXMC='{4}';",
                            hjdwZdf, hjdwDlzb, (hjdwDfzbJiaFen + hjdwDfzbJianFen), ejdw.KHID, ejdw.Hbjf.HBJFID, ejdw.NDXS);
                        sqls.Append(updSql);
                    }
                }
                if (!string.IsNullOrEmpty(sqls.ToString()))
                    isOk = ExecuteSQL(sqls.ToString());
                //3==�������λ����,�����ݹ�������������----------------------------------------------------------------------
                if (isOk)
                {
                    //���»�ȡһ�����¼�¼����
                    var dataList3 = GetListByKhid(khid).Where(p => p.KHLX == ((int)WebKeys.KaoheType.�쵼����).ToString()).ToList();
                    var oneEjdwLst3 = dataList3.Where(p => p.JGBM != null).DistinctBy(p => p.JGBM).ToList();
                    var gsdwList = oneEjdwLst3.DistinctBy(p => p.GSDWMC).ToList();//�����ϼ���λ
                    List<string> hbjfmcList = new List<string>();
                    foreach (var gsdw in gsdwList)
                    {
                        sqls = new StringBuilder();
                        var oneEjdwLstByGsdw = oneEjdwLst3.Where(p => p.GSDWMC == gsdw.GSDWMC).
                            OrderByDescending(p => p.FZRZDF).ToList();
                        int dwpm = 1;//����
                        for (int i = 0; i < oneEjdwLstByGsdw.Count; i++)
                        {
                            var model = oneEjdwLstByGsdw[i];
                            if (model.Hbjf != null)
                            {
                                if (!hbjfmcList.Exists(p => p == model.HBJFID))
                                {
                                    //�ϲ��Ƿ�
                                    sqls.Append("update [PERFO_KH_EJDWKHDF] set FZRPM='" + (dwpm++)
                                        + "' where KHID='" + model.KHID + "' and HBJFID='" + model.HBJFID + "';");
                                    hbjfmcList.Add(model.HBJFID);
                                }
                            }
                            else
                            {
                                //�����Ƿ�
                                sqls.Append("update [PERFO_KH_EJDWKHDF] set FZRPM='" + (dwpm++)
                                    + "' where KHID='" + model.KHID + "' and JGBM='" + model.JGBM + "';");
                            }
                        }
                        Debuger.GetInstance().log(khid + "=====3�������λ����,�����ݹ�������������====��" + sqls.ToString() + "��");
                        if (!string.IsNullOrEmpty(sqls.ToString()))
                            isOk = ExecuteSQL(sqls.ToString());
                        //4==�쵼���Ӷ��ֱ�������==================================================
                        if (isOk)
                        {
                            //�Ӳ������ñ�����ȡ������ķֲ����ݣ����ݸ����ӵĵ÷����ͼ�����ֱ���
                            //���ֱ���=SUM(����min+(����max-����min)*(��ǰ�÷�-ͬ������ͷ�)/(ͬ����max-ͬ����min))
                            List<KhCjqjModel> cjqjLst = cjqjSrv.GetListByKhid(khid) as List<KhCjqjModel>;
                            //���»�ȡһ�����¼�¼����
                            var dataList4 = GetListByKhid(khid).Where(p => p.KHLX == ((int)WebKeys.KaoheType.�쵼����).ToString() 
                                && !string.IsNullOrEmpty(p.FZRDFLB)).ToList();
                            var oneEjdwLst4 = dataList4.Where(p => p.JGBM != null).DistinctBy(p => p.JGBM).ToList();
                            sqls = new StringBuilder();
                            foreach (var ejdw in oneEjdwLst4)
                            {
                                var gradeLst = oneEjdwLst4.Where(p => p.FZRDFLB == ejdw.FZRDFLB).ToList();
                                var cjqj = cjqjLst.FirstOrDefault(p => p.QJDJ == ejdw.FZRDFLB);
                                if (cjqj != null)
                                {
                                    decimal qjMin = cjqj.LOWERV.ToDecimal();//����min
                                    decimal qjMax = cjqj.UPPERV.ToDecimal();//����max
                                    decimal dfMax = gradeLst.Max(p => p.FZRZDF).ToDecimal();//ͬ����max
                                    decimal dfMin = gradeLst.Min(p => p.FZRZDF).ToDecimal();//ͬ����min
                                    decimal dxsx = 0M;
                                    try
                                    {
                                        dxsx = qjMin + (qjMax - qjMin) * ((ejdw.FZRZDF.Value - dfMin) / (dfMax - dfMin));
                                    }
                                    catch { }
                                    if (!string.IsNullOrEmpty(ejdw.HBJFID))
                                    {
                                        //�ϲ��Ʒ֣�ͬ������
                                        sqls.Append("update [PERFO_KH_EJDWKHDF] set FZRDXBS='" + dxsx
                                        + "' where HBJFID='" + ejdw.HBJFID + "';");//HBJFID�������ڣ�����Ψһ��
                                    }
                                    else
                                    {
                                        //��������
                                        sqls.Append("update [PERFO_KH_EJDWKHDF] set FZRDXBS='" + dxsx
                                        + "' where KHID='" + ejdw.KHID + "' and JGBM='" + ejdw.JGBM + "';");
                                    }
                                }
                            }
                            Debuger.GetInstance().log(khid + "=====4�쵼���Ӷ��ֱ�������====��" + sqls.ToString() + "��");
                            if (!string.IsNullOrEmpty(sqls.ToString()))
                                isOk = ExecuteSQL(sqls.ToString());
                        }
                    }
                    
                }
            }
            return isOk;
        }


        /// <summary>
        /// ����ָ���������µĶ�����λ���˵÷�
        /// </summary>
        /// <param name="khid">����ID</param>
        /// <returns></returns>
        public bool CalculateEjdwkhdfData(string khid)
        {
            /*
             ˼·��
             * 1���Ӷ�����λ���˽�����зֱ���ȡ���˶���Ϊ������λ���쵼���ӵ�����
             * 2����ȡ�ϲ��Ʒֹ���
             * 3�����¼���ϲ��Ʒֵĵ�λ�ĵ÷�����
             * 4�����ݵ�ǰ���˵ĵ�λ����Ҫ�󣬶Ե�λ�������н��и���
             */
            bool isOk = true;
            KhKhdfpxfwService pxfwSrv = new KhKhdfpxfwService();//����Χ
            KhHbjfgzService hbjfgzSrv = new KhHbjfgzService();//�ϲ��Ʒֹ���
            StringBuilder sqls = new StringBuilder();
            string updSql = string.Empty;
            var dataList = GetListByKhid(khid).Where(p => p.KHLX == ((int)WebKeys.KaoheType.������λ).ToString()).ToList();
            //1==��������Ʒֵ�λ�� �÷�=Ч����+������+Լ��+�ӷ�----------------------------------
            var oneEjdwLst = dataList.Where(p => p.JGBM != null && p.HBJFID == null).DistinctBy(p => p.JGBM).ToList();
            foreach (var ejdw in oneEjdwLst)
            {
                decimal dwzdf = 0M;
                decimal xyl_gll_df = 0M;
                //Ч����
                var xylzb = dataList.Where(p => p.JGBM == ejdw.JGBM && p.KHXMC == "Ч����").FirstOrDefault();
                if (xylzb != null)
                {
                    dwzdf += xylzb.KHDF.ToDecimal();
                    xyl_gll_df += xylzb.KHDF.ToDecimal();
                }
                //������
                var gllzb = dataList.Where(p => p.JGBM == ejdw.JGBM && p.KHXMC == "������").FirstOrDefault();
                if (gllzb != null)
                {
                    dwzdf += gllzb.KHDF.ToDecimal();
                    xyl_gll_df += gllzb.KHDF.ToDecimal();
                }
                //Լ���۷�
                var ysxzb = dataList.Where(p => p.JGBM == ejdw.JGBM && p.KHXMC == "Լ����ָ��").FirstOrDefault();
                if (ysxzb != null)
                {
                    dwzdf += ysxzb.KHDF.ToDecimal();
                }
                //�ӷ����
                var jfzb = dataList.Where(p => p.JGBM == ejdw.JGBM && p.KHXMC == "�ӷ�ָ��").FirstOrDefault();
                if (jfzb != null)
                {
                    dwzdf += jfzb.KHDF.ToDecimal();
                }
                updSql =
                    string.Format("update [PERFO_KH_EJDWKHDF] set DWZDF='{0}' where KHID='{1}' and JGBM='{2}';",
                    dwzdf, ejdw.KHID, ejdw.JGBM);
                sqls.Append(updSql);
            }
            if (!string.IsNullOrEmpty(sqls.ToString()))
                isOk = ExecuteSQL(sqls.ToString());
            if (isOk)
            {
                //2==����ϲ��Ʒֵĵ�λ�ĵ÷�--------------------------------------------------------------------
                List<string> hbjfmcLst = new List<string>();
                var hbjfEjdwLst = dataList.Where(p => p.JGBM != null && p.HBJFID != null).DistinctBy(p => p.JGBM).ToList();
                sqls = new StringBuilder();
                foreach (var ejdw in hbjfEjdwLst)
                {
                    //�ϲ�����÷�
                    if (ejdw.Hbjf != null && ejdw.Hbjf.Jsgz != null && !hbjfmcLst.Exists(p => p == ejdw.Hbjf.HBJFID))
                    {
                        decimal hjdwZdf = 0M;//�ܵ÷�
                        decimal hjdwDlzb = 0M;//����ָ��ϼ�
                        decimal hjdwDfzbJiaFen = 0M;//���ָ��ϼ� �ӷ�
                        decimal hjdwDfzbJianFen = 0M;//���ָ��ϼ� ����
                        string[] jgbms = ejdw.Hbjf.HBJFDW.TrimStart(',').TrimEnd(',').Split(',');
                        foreach (var jgbm in jgbms)
                        {
                            string bds = ejdw.Hbjf.Jsgz.GZBDS.Replace("��", jgbm);
                            decimal qz = Utility.Eval(bds).ToDecimal();
                            hjdwDlzb += dataList.
                                Where(p => p.JGBM == jgbm.ToInt() && (p.KHXMC == "Ч����" || p.KHXMC == "������")).Sum(p => p.KHDF).ToDecimal() * qz;
                            hjdwDfzbJiaFen += dataList.
                                Where(p => p.JGBM == jgbm.ToInt() && (p.KHXMC == "�ӷ�ָ��")).Sum(p => p.KHDF).ToDecimal();
                            hjdwDfzbJianFen += dataList.
                                Where(p => p.JGBM == jgbm.ToInt() && (p.KHXMC == "Լ����ָ��")).Sum(p => p.KHDF).ToDecimal();
                        }

                        hjdwZdf = hjdwDlzb + hjdwDfzbJiaFen + hjdwDfzbJianFen;//�ܵ÷�
                        hbjfmcLst.Add(ejdw.Hbjf.HBJFID);//����ֻһ�ξ��ܰ���صĵ÷ֶ�����������԰�ʣ�����ص�λ���˵�

                        //�����⼸����λ���ܵ÷� �����־
                        //��1��==��λ��ֻ���²����ܵ÷�
                        updSql = string.Format(
                            "update [PERFO_KH_EJDWKHDF] set DWZDF='{0}' where KHID='{1}' and JGBM in ({2});",
                            hjdwZdf, ejdw.KHID, ejdw.Hbjf.HBJFDW.TrimStart(',').TrimEnd(','));
                        sqls.Append(updSql);
                        //��2��==�ϲ��Ʒ���, ���� �䲿���ܵ÷� ���˵÷� �ӷ� ����
                        updSql = string.Format(
                            "update [PERFO_KH_EJDWKHDF] set DWZDF='{0}',KHDF='{1}',DWPJF='{2}',DWDXBS='{3}' where KHID='{4}' and KHXMC='{5}';",
                            hjdwZdf, hjdwDlzb, hjdwDfzbJiaFen, hjdwDfzbJianFen, ejdw.KHID, ejdw.Hbjf.HBJFID);
                        sqls.Append(updSql);
                    }
                }
                if (!string.IsNullOrEmpty(sqls.ToString()))
                    isOk = ExecuteSQL(sqls.ToString());

                //3==�������λ����,�����ݹ�������������----------------------------------------------------------------------
                if (isOk)
                {
                    //���»�ȡһ�����¼�¼����
                    var dataList3 = GetListByKhid(khid).Where(p => p.KHLX == ((int)WebKeys.KaoheType.������λ).ToString()).ToList();
                    var oneEjdwLst3 = dataList3.Where(p => p.JGBM != null).DistinctBy(p => p.JGBM).ToList();
                    var gsdwList = oneEjdwLst3.DistinctBy(p => p.GSDWMC).ToList();//�����ϼ���λ
                    List<string> hbjfmcList = new List<string>();
                    foreach (var gsdw in gsdwList)
                    {
                        sqls = new StringBuilder();
                        var oneEjdwLstByGsdw = oneEjdwLst3.Where(p => p.GSDWMC == gsdw.GSDWMC).
                            OrderByDescending(p => p.DWZDF).ToList();
                        int dwpm = 1;//����
                        for (int i = 0; i < oneEjdwLstByGsdw.Count; i++)
                        {
                            var model = oneEjdwLstByGsdw[i];
                            if (model.Hbjf != null)
                            {
                                if (!hbjfmcList.Exists(p => p == model.HBJFID))
                                {
                                    //�ϲ��Ƿ�
                                    sqls.Append("update [PERFO_KH_EJDWKHDF] set DWPM='" + (dwpm++)
                                        + "' where KHID='" + model.KHID + "' and HBJFID='" + model.HBJFID + "';");
                                    hbjfmcList.Add(model.HBJFID);
                                }
                            }
                            else
                            {
                                //�����Ƿ�
                                sqls.Append("update [PERFO_KH_EJDWKHDF] set DWPM='" + (dwpm++)
                                    + "' where KHID='" + model.KHID + "' and JGBM='" + model.JGBM + "';");
                            }
                        }
                        if (!string.IsNullOrEmpty(sqls.ToString()))
                            isOk = ExecuteSQL(sqls.ToString());
                    }

                }
            }
            return isOk;
        }

        #endregion

    }

}
