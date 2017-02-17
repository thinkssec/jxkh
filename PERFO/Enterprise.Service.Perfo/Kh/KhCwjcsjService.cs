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
            string sql = "delete from PERFO_KH_CWJCSJ where JGBM='" + jgbm + "' and NF='" + yy + "'";
            return dal.ExecuteSQL(sql);
        }

        #endregion
    }

}
