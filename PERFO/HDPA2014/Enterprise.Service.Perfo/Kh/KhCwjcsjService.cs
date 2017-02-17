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
    /// �ļ���:  KhCwjcsjService.cs
    /// ��������: ҵ���߼���-����������ݱ����ݴ���
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/12/13 10:23:51
    /// </summary>
    public class KhCwjcsjService
    {
        #region ����������

        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly IKhCwjcsjData dal = new KhCwjcsjData();

        /// <summary>
        /// �������������
        /// </summary>
        public static readonly string[] JcsjZbs = new string[] { 
                "Ӫҵ�����루�ۼƣ�",
                "���У����ⴴ�գ��ۼƣ�",
                "Ӫҵ�ܳɱ����ۼƣ�",
                "������ã��ۼƣ�",
                "���У���1���칫�ѣ��ۼƣ�",
                "��2�����÷ѣ��ۼƣ�",
                "��3������ѣ��ۼƣ�",
                "��4��ҵ���д��ѣ��ۼƣ�",
                "��5������ʹ�÷ѣ��ۼƣ�",
                "��6��������Ա���ѣ��ۼƣ�",
                "������ã��ۼƣ�",
                "���У���Ϣ֧�����ۼƣ�",
                "Ӫҵ�����ۼƣ�",
                "Ӫҵ�����루�ۼƣ�",
                "Ӫҵ��֧�����ۼƣ�",
                "�����ܶ�ۼƣ�",
                "�������ۼƣ�",
                "�˹��ɱ����ۼƣ�",
                "�̶��ʲ��۾ɣ��ۼƣ�",
                "�����ʲ�̯�����ۼƣ�",
                "���ڴ�̯����̯�����ۼƣ�",
                "���о��ѣ��ۼƣ�",
                "��������ר������ۼƣ�",
                "���汣����ҵ����֧�����ۼƣ�",
                "����ְ��Ա���ã��ۼƣ�",
                "���汣����ҵ�������루�ۼƣ�",
                "�ʲ��ܶƽ����",
                "�̶��ʲ�ԭֵ��ƽ����",
                "�ڽ����̣�ƽ����",
                "�����ʲ�ԭֵ��ƽ����",
                "���ڴ�̯����ԭֵ��ƽ����",
                "Ӧ��Ʊ�ݣ�ƽ����",
                "Ӧ���˿ƽ����",
                "Ԥ���˿ƽ����",
                "Ӧ��ְ��н�꣨ƽ����",
                "Ӧ��˰�ѣ�ƽ����",
                "Ӧ����Ϣ��ƽ����",
                "����Ӧ���ƽ����",
                "�ڲ�����ƽ����",
                "����������ծ��ƽ����",
                "ר��Ӧ���ƽ����",
                "ʵ���ʱ���ƽ����",
                "�ʱ�������ƽ����",
                "���������ʽ�ƽ����",
                "EVA���ۼƣ�"
            };

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhCwjcsjModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhCwjcsjModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhCwjcsjModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhCwjcsjModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhCwjcsjModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region �Զ��巽��

        /// <summary>
        /// ��ȡָ���ĵ�λ�������ݵĻ�������
        /// </summary>
        /// <param name="jgbm">��λ����</param>
        /// <param name="nf">���</param>
        /// <returns></returns>
        public IList<KhCwjcsjModel> GetListByJgbmAndNF(string jgbm, int nf)
        {
            string hql = "from KhCwjcsjModel p where p.JGBM='" + jgbm + "' and p.NF='" + nf + "' order by p.NF, p.XH";
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// ��ʼ��ָ�꿼������µ�ָ����λ��������
        /// </summary>
        /// <param name="yy">���</param>
        /// <param name="jgbm">��λ����</param>
        /// <returns></returns>
        public bool InitJcsjDataBySzndAndJgbm(int yy, int jgbm)
        {
            bool isOk = DeleteJcsjData(yy, jgbm);
            for (int i = 0; i < JcsjZbs.Length; i++)
            {
                KhCwjcsjModel jcsjM = new KhCwjcsjModel();
                jcsjM.DB_Option_Action = WebKeys.InsertAction;
                jcsjM.CWZBID = CommonTool.GetGuidKey();
                jcsjM.ZBMC = JcsjZbs[i];
                jcsjM.XH = (i + 1);
                jcsjM.NF = yy;
                jcsjM.JGBM = jgbm;
                isOk = Execute(jcsjM);
            }
            return isOk;
        }

        /// <summary>
        /// ɾ��ָ����λ��������ݵ�����
        /// </summary>
        /// <param name="yy">���</param>
        /// <param name="jgbm">��λ����</param>
        /// <returns></returns>
        public bool DeleteJcsjData(int yy, int jgbm)
        {
            string sql = "delete from [PERFO_KH_CWJCSJ] where JGBM='" + jgbm + "' and NF='" + yy + "';";
            return dal.ExecuteSQL(sql);
        }

        /// <summary>
        /// ����ָ����λ����ݵ�EVA�ܶ�����
        /// </summary>
        /// <param name="jgbm">��λ����</param>
        /// <param name="yy">���</param>
        /// <returns></returns>
        public bool CalcAndSaveEVA(int jgbm, int yy)
        {
            /*
             {EVA���ۼƣ�}={�������ۼƣ�}+({���У���Ϣ֧�����ۼƣ�}+{���о��ѣ��ۼƣ�})*0.75-{Ӫҵ�����루�ۼƣ�}+{Ӫҵ��֧�����ۼƣ�}-��{�ʲ��ܶƽ����}-{Ӧ��Ʊ�ݣ�ƽ����}-{Ӧ���˿ƽ����}-{Ԥ���˿ƽ����}-{Ӧ��ְ��н�꣨ƽ����}-{Ӧ��˰�ѣ�ƽ����}-{Ӧ����Ϣ��ƽ����}-{����Ӧ���ƽ����}-{�ڲ�����ƽ����}-{����������ծ��ƽ����}-{ר��Ӧ���ƽ����}-{�ڽ����̣�ƽ����})*0.055
             ˼·��
             * 1����ȡָ����λ����ȵĻ������ݣ�
             * 2������EVA��ʽ�滻��ָ��ֵ��������EVA
             * 3�����������
             */
            var jcsjLst = GetListByJgbmAndNF(jgbm.ToString(), yy);
            string evabds = "{�������ۼƣ�}+({���У���Ϣ֧�����ۼƣ�}+{���о��ѣ��ۼƣ�})*0.75-{Ӫҵ�����루�ۼƣ�}+{Ӫҵ��֧�����ۼƣ�}-({�ʲ��ܶƽ����}-{Ӧ��Ʊ�ݣ�ƽ����}-{Ӧ���˿ƽ����}-{Ԥ���˿ƽ����}-{Ӧ��ְ��н�꣨ƽ����}-{Ӧ��˰�ѣ�ƽ����}-{Ӧ����Ϣ��ƽ����}-{����Ӧ���ƽ����}-{�ڲ�����ƽ����}-{����������ծ��ƽ����}-{ר��Ӧ���ƽ����}-{�ڽ����̣�ƽ����})*0.055";
            foreach (var jcsj in jcsjLst)
            {
                if (jcsj.ZBMC.Contains("�ۼ�"))
                {
                    evabds = evabds.Replace("{" + jcsj.ZBMC + "}", "(" + jcsj.LJZ.ToDecimal().ToString() + ")");
                }
                else if (jcsj.ZBMC.Contains("ƽ��"))
                {
                    evabds = evabds.Replace("{" + jcsj.ZBMC + "}", "(" + jcsj.PJZ.ToDecimal().ToString() + ")");
                }
            }
            decimal eva = Utility.Eval(evabds).ToDecimal();
            //����EVA����
            decimal pjz = eva / 12;
            string sql = string.Format(
                "update [PERFO_KH_CWJCSJ] set LJZ='{0}',M1='{1}',M2='{2}',M3='{3}',M4='{4}',M5='{5}',M6='{6}',M7='{7}',M8='{8}',M9='{9}',M10='{10}',M11='{11}',M12='{12}' where ZBMC='EVA���ۼƣ�' and JGBM='{13}' and NF='{14}'",
                eva, pjz, pjz, pjz, pjz, pjz, pjz, pjz, pjz, pjz, pjz, pjz, pjz, jgbm, yy);
            return dal.ExecuteSQL(sql);
        }

        #endregion
    }

}
