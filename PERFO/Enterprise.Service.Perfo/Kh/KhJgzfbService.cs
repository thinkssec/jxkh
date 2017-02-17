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
    /// �ļ���:  KhJgzfbService.cs
    /// ��������: ҵ���߼���-�������罨�迼�˻��ܱ����ݴ���
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/28 16:45:02
    /// </summary>
    public class KhJgzfbService
    {
        #region ����������

        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly IKhJgzfbData dal = new KhJgzfbData();

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhJgzfbModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhJgzfbModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhJgzfbModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhJgzfbModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhJgzfbModel model)
        {
            return dal.Execute(model);
        }

        /// <summary>
        /// ִ��ԭ��SQL����
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool ExecuteBySQL(string sql)
        {
            return dal.ExecuteSQL(sql);
        }

        /// <summary>
        /// ɾ��ָ�������µ�����
        /// </summary>
        /// <param name="khid"></param>
        /// <returns></returns>
        public bool DeleteByKhid(string khid)
        {
            string sql = "delete from PERFO_KH_JGZFB where KHID='" + khid + "'";
            return dal.ExecuteSQL(sql);
        }

        /// <summary>
        /// ����ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public bool ExecuteByList(List<KhJgzfbModel> models)
        {
            bool isOk = true;
            foreach (var model in models)
            {
                isOk = dal.Execute(model);
            }
            return isOk;
        }

        #endregion

        /// <summary>
        /// ���»������罨��ĵ÷���Ϣ
        /// </summary>
        /// <param name="khid"></param>
        /// <param name="jgbm"></param>
        /// <param name="zbbm"></param>
        /// <param name="dfzlx"></param>
        /// <param name="zfkhdf"></param>
        /// <param name="zdf"></param>
        /// <param name="pm"></param>
        /// <param name="jjf"></param>
        /// <returns></returns>
        public bool UpdateJGZF(int khid, int jgbm, string zbbm, string dfzlx, 
            decimal zfkhdf, decimal zdf, int pm, decimal jjf)
        {
            /*
             �Ĳ���
             * 1��ͬ�����Ŵ�ֵĽ��
             * 2��ͬ��������λ�Ľ��
             * 3���ϼ���ֵĽ��
             * 4���ܵ÷֡��������ӷֽ��
             */
            //KHID  JGBM  ZBBM  DFZLX
            string sql =
                string.Format("update [PERFO_KH_JGZFB] set ZFKHDF='{0}',ZDF='{1}',ZFPM='{2}',SJDF='{3}' "
                + " where KHID='{4}' and JGBM='{5}' and ZBBM='{6}' and DFZLX='{7}';", 
                zfkhdf, zdf, pm, jjf, khid, jgbm, zbbm, dfzlx);
            return ExecuteBySQL(sql);
        }

        /// <summary>
        /// ��ȡָ������ID��Ӧ�����ݼ�
        /// </summary>
        /// <param name="khid"></param>
        /// <returns></returns>
        public IList<KhJgzfbModel> GetListByKhid(string khid)
        {
            string hql = "from KhJgzfbModel p where p.KHID='" + khid + "' order by p.DFZBXH";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// ��ȡָ�������ں͵�λ��Ӧ�����ݼ�
        /// </summary>
        /// <param name="khid">������</param>
        /// <param name="jgbm">��������</param>
        /// <returns></returns>
        public IList<KhJgzfbModel> GetListByKhidAndJgbm(string khid, string jgbm)
        {
            string hql = "from KhJgzfbModel p where p.KHID='" + khid + "' and p.JGBM='" + jgbm + "' order by p.DFZBXH";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// ��ɻ������罨�����ݵĳ�ʼ��
        /// </summary>
        /// <param name="dfzbmxLst">���ָ����ϸ</param>
        /// <param name="sjsbLst">�����ϱ���λ����</param>
        /// <returns></returns>
        public bool InitJgzfData(List<KhDfzbmxModel> dfzbmxLst, List<KhSjsbModel> sjsbLst)
        {
            SysBmjgService bmjgSrv = new SysBmjgService();
            /*
             1=�������ϱ��еĵ�λѭ����ȡ���ָ�꼯����λ��ָ��
             2=�жϸ�ָ��Ĵ���ߣ����ΰѴ��ָ�����
             */
            int index = 1;
            List<KhJgzfbModel> list = new List<KhJgzfbModel>();
            foreach (var sjsb in sjsbLst)
            {
                //1==��ȡ��λ�µ�����ָ��
                var jgDfzbLst = dfzbmxLst.Where(p => p.JGBM == sjsb.JGBM);
                foreach (var dfzb in jgDfzbLst)
                {
                    //2==��ȡ��ָ���Ӧ�Ĵ���ߣ������δ���
                    foreach (var m in dfzb.KhJgbmdfbLst)
                    {
                        //3==����������߷����ϼ��û���ͬ�����ţ�ͬ��������λ
                        int jgbm = m.DFZ.ToInt();
                        if (jgbm > 0)
                        {
                            //ͬ������
                            var bmjg = bmjgSrv.GetSingle(jgbm.ToString());
                            if (m.DFZLX == ((int)WebKeys.DFUserType.ͬ������).ToString() || bmjg.JGLX.Contains("ְ��"))
                            {
                                //ͬ������ 02
                                KhJgzfbModel model = new KhJgzfbModel();
                                model.DB_Option_Action = WebKeys.InsertAction;
                                model.KHID = dfzb.KHID.Value;
                                model.ZBBM = dfzb.ZBBM;
                                model.ZBMC = dfzb.ZbsxModel.JxzrsZb.Dfzb.Zbxx.ZBMC;
                                model.JGBM = dfzb.JGBM.Value;
                                model.DFZSL = dfzb.KhJgbmdfbLst.Count;
                                model.ZFID = CommonTool.GetGuidKey();
                                model.DFZLX = ((int)WebKeys.DFUserType.ͬ������).ToString();
                                model.DFZBXH = "02" + CommonTool.BuZero_3(index++);
                                if (!list.Exists(p => p.ZBBM == model.ZBBM &&
                                    p.DFZLX.ToInt() == (int)WebKeys.DFUserType.ͬ������))
                                {
                                    list.Add(model);
                                }
                            }
                            else if (m.DFZLX == ((int)WebKeys.DFUserType.ͬ��������λ).ToString() || bmjg.JGLX.Contains("����"))
                            {
                                //ͬ��������λ 03
                                KhJgzfbModel model = new KhJgzfbModel();
                                model.DB_Option_Action = WebKeys.InsertAction;
                                model.KHID = dfzb.KHID.Value;
                                model.ZBBM = dfzb.ZBBM;
                                model.ZBMC = dfzb.ZbsxModel.JxzrsZb.Dfzb.Zbxx.ZBMC;
                                model.JGBM = dfzb.JGBM.Value;
                                model.DFZSL = dfzb.KhJgbmdfbLst.Count;
                                model.ZFID = CommonTool.GetGuidKey();
                                model.DFZLX = ((int)WebKeys.DFUserType.ͬ��������λ).ToString();
                                model.DFZBXH = "03" + CommonTool.BuZero_3(index++);
                                if (!list.Exists(p => p.ZBBM == model.ZBBM &&
                                    p.DFZLX.ToInt() == (int)WebKeys.DFUserType.ͬ��������λ))
                                {
                                    list.Add(model);
                                }
                            }
                        }
                        else
                        {
                            //�ϼ��쵼 01
                            KhJgzfbModel model = new KhJgzfbModel();
                            model.DB_Option_Action = WebKeys.InsertAction;
                            model.KHID = dfzb.KHID.Value;
                            model.ZBBM = dfzb.ZBBM;
                            model.ZBMC = dfzb.ZbsxModel.JxzrsZb.Dfzb.Zbxx.ZBMC;
                            model.JGBM = dfzb.JGBM.Value;
                            model.DFZSL = dfzb.KhJgbmdfbLst.Count;
                            model.ZFID = CommonTool.GetGuidKey();
                            model.DFZLX = ((int)WebKeys.DFUserType.�ϼ��쵼).ToString();
                            model.DFZBXH = "01" + CommonTool.BuZero_3(index++);
                            if (!list.Exists(p => p.ZBBM == model.ZBBM &&
                                p.DFZLX.ToInt() == (int)WebKeys.DFUserType.�ϼ��쵼))
                            {
                                list.Add(model);
                            }
                        }
                    }
                }
            }
            return ExecuteByList(list);
        }

        /// <summary>
        /// �����������ָ������ŵĵ÷�
        /// </summary>
        /// <param name="khid">������</param>
        /// <returns></returns>
        public bool CalculateJgzfDF(string khid)
        {
            /*
             1����ȡ��ǰ�����µ��������罨���ּ�¼
             2���ӻ��ز��Ŵ�ֱ�����ȡ��Ӧ��¼�����з���ͳ�Ƶ÷�
             */
            bool isOk = true;
            KhJgbmdfbService jgbmdfSrv = new KhJgbmdfbService();//���ز��Ŵ�ֱ�
            KhDfzbmxService dfzbmxSrv = new KhDfzbmxService();//���ָ����ϸ
            //1==��ȡ���ָ����е� �������罨�� ָ��
            List<KhDfzbmxModel> zbmxList = dfzbmxSrv.GetListByKhidAndKhdx(khid, WebKeys.KaoheType.���ز���)
                .Where(p=>p.ZbsxModel.JxzrsZb.Dfzb.Zbxx.YJZBMC.Contains("��������")).ToList();
            var jgzfbList = GetListByKhid(khid) as List<KhJgzfbModel>;
            foreach (var jgzf in jgzfbList)
            {
                var dfzb = zbmxList.FirstOrDefault(p => p.JGBM == jgzf.JGBM && p.ZBBM == jgzf.ZBBM);
                if (dfzb == null || dfzb.KhJgbmdfbLst.Count == 0) continue;
                var dfzLst = dfzb.KhJgbmdfbLst.Where(p => p.DFZLX == jgzf.DFZLX && p.KHDF != null).ToList();
                //2==�ϼ�����ͬ��������͵Ĵ�ֽ����ͳ��������
                if (dfzLst.Count > 0)
                {
                    jgzf.ZFKHDF = dfzLst.Sum(p => p.KHDF) / dfzLst.Count * 100M;//�÷�
                    jgzf.DFZSL = dfzLst.Count;//���������
                    jgzf.DB_Option_Action = WebKeys.UpdateAction;
                    isOk = Execute(jgzf);
                }
            }
            //3==����÷�ͳ����ɺ󣬻�Ҫ�ϳ����յ÷ֺ��������ӷ����
            jgzfbList = GetListByKhid(khid) as List<KhJgzfbModel>;
            var jgbms = jgzfbList.DistinctBy(p => p.JGBM).ToList();//һ����λһ����¼
            List<KhJgzfbModel> jgbmsLst = new List<KhJgzfbModel>();
            foreach (var jgbm in jgbms)
            {
                jgbm.DB_Option_Action = WebKeys.UpdateAction;
                int dfzlxCount = 0;//�������������
                var zpLst = jgzfbList.Where(p => p.JGBM == jgbm.JGBM && 
                    p.DFZLX == ((int)WebKeys.DFUserType.ͬ������).ToString()).ToList();
                decimal zpDefen = 0M;
                if (zpLst.Count > 0) {
                    dfzlxCount += 1;
                    zpDefen = zpLst.Sum(p => p.ZFKHDF).ToDecimal() / zpLst.Count;
                }

                var ejdwLst = jgzfbList.Where(p => p.JGBM == jgbm.JGBM &&
                    p.DFZLX == ((int)WebKeys.DFUserType.ͬ��������λ).ToString()).ToList();
                decimal ejdwDefen = 0M;
                if (ejdwLst.Count > 0)
                {
                    dfzlxCount += 1;
                    ejdwDefen = ejdwLst.Sum(p => p.ZFKHDF).ToDecimal() / ejdwLst.Count;
                }

                var ldLst = jgzfbList.Where(p => p.JGBM == jgbm.JGBM &&
                    p.DFZLX == ((int)WebKeys.DFUserType.�ϼ��쵼).ToString()).ToList();
                decimal ldDefen = 0M;
                if (ldLst.Count > 0)
                {
                    dfzlxCount += 1;
                    ldDefen = ldLst.Sum(p => p.ZFKHDF).ToDecimal() / ldLst.Count;
                }

                //�����ܵ÷ֺ�ƽ��
                if (dfzlxCount > 0)
                {
                    //�ܵ÷�=�������͵�ƽ����֮����ȡƽ��
                    jgbm.ZDF = (zpDefen + ejdwDefen + ldDefen) / dfzlxCount;
                    jgbmsLst.Add(jgbm);
                }
            }
            //4==�Ը���λ���ܵ÷ֽ���������������ͼӷ�ֵ
            /*
             * �����������÷�ͨ����������γ����۽����ƽ������
             * ���ŵ�1-5λ�ļ�5�֣�
             * ��6-10λ�ļ�3�֣���11-15λ�ļ�2�֣���16-20λ�ļ�1�֡�
             */
            jgbmsLst = jgbmsLst.OrderByDescending(p => p.ZDF).ToList();
            for (int i = 0; i < jgbmsLst.Count; i++ )
            {
                jgbmsLst[i].ZFPM = (i + 1);
                if (i < 5) jgbmsLst[i].SJDF = 5;
                else if (i >= 5 && i < 10) jgbmsLst[i].SJDF = 3;
                else if (i >= 10 && i < 15) jgbmsLst[i].SJDF = 2;
                else if (i >= 15 && i < 20) jgbmsLst[i].SJDF = 1;
                else jgbmsLst[i].SJDF = 0;

                string sql = string.Format(
                    "update [PERFO_KH_JGZFB] set ZDF='{0}',ZFPM='{1}',SJDF='{2}' where KHID='{3}' and JGBM='{4}';",
                    jgbmsLst[i].ZDF, jgbmsLst[i].ZFPM, jgbmsLst[i].SJDF, jgbmsLst[i].KHID, jgbmsLst[i].JGBM);
                ExecuteBySQL(sql);
            }
            return isOk;
        }
    }

}
