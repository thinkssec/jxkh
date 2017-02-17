using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Data.Perfo.Khs;
using Enterprise.Model.Perfo.Khs;

namespace Enterprise.Service.Perfo.Khs
{
	
    /// <summary>
    /// �ļ���:  KhsMbjgService.cs
    /// ��������: ҵ���߼���-ģ��������ݴ���
    /// �����ˣ�����������
	/// ����ʱ�� ��2015/11/5 22:09:24
    /// </summary>
    public class KhsMbjgService
    {
        public bool DeleteById(string Id,string kh)
        {
            StringBuilder sqls = new StringBuilder();
            sqls.Append("begin ");
            sqls.Append("delete from PERFO_KHS_MBJG where MBID='" + Id + "' and KHDZBID='"+kh+"';");
            sqls.Append(" end;");
            return dal.ExecuteSQL(sqls.ToString());
        }

        public bool Update60(string khid)
        {
            StringBuilder sqls = new StringBuilder();
            sqls.Append("begin ");
            sqls.Append("update PERFO_KHS_MBJG set status='60' where  KHDZBID in (select id from PERFO_KHS_KHDZB where khid="+khid+");");
            sqls.Append(" end;");
            return dal.ExecuteSQL(sqls.ToString());
        }
        public bool Update42(string khid,string fgld)
        {
            StringBuilder sqls = new StringBuilder();
            sqls.Append("begin ");
            sqls.Append("update PERFO_KHS_MBJG set status='42' where  KHDZBID in (select id from PERFO_KHS_KHDZB where khid=" + khid + "and dzbid in (select dzbid from PERFO_KHS_DZB where fzbm in (select jgbm from PERFO_SYS_FGBMJG where loginid = '" + fgld + "')));");
            sqls.Append(" end;");
            return dal.ExecuteSQL(sqls.ToString());
        }

        public bool Update50(string khid,string fgld)
        {
            StringBuilder sqls = new StringBuilder();
            sqls.Append("begin ");
            sqls.Append("update PERFO_KHS_MBJG set status='50' where  KHDZBID in (select id from PERFO_KHS_KHDZB where khid=" + khid + "and dzbid in (select dzbid from PERFO_KHS_DZB where fzbm in (select jgbm from PERFO_SYS_FGBMJG where loginid = '"+fgld+"')));");
            sqls.Append(" end;");
            return dal.ExecuteSQL(sqls.ToString());
        }

        public bool Update50(string khid)
        {
            StringBuilder sqls = new StringBuilder();
            sqls.Append("begin ");
            sqls.Append("update PERFO_KHS_MBJG set status='50' where  KHDZBID in (select id from PERFO_KHS_KHDZB where khid=" + khid + ");");
            sqls.Append(" end;");
            return dal.ExecuteSQL(sqls.ToString());
        }

        public bool Update52(string khid)
        {
            StringBuilder sqls = new StringBuilder();
            sqls.Append("begin ");
            sqls.Append("update PERFO_KHS_MBJG set status='52' where  KHDZBID in (select id from PERFO_KHS_KHDZB where khid=" + khid + ");");
            sqls.Append(" end;");
            return dal.ExecuteSQL(sqls.ToString());
        }
        public bool Update51(string khid)
        {
            StringBuilder sqls = new StringBuilder();
            sqls.Append("begin ");
            sqls.Append("update PERFO_KHS_MBJG set status='51' where  KHDZBID in (select id from PERFO_KHS_KHDZB where khid=" + khid + ");");
            sqls.Append(" end;");
            return dal.ExecuteSQL(sqls.ToString());
        }

        //public bool Update61(string khid)
        //{
        //    StringBuilder sqls = new StringBuilder();
        //    sqls.Append("begin ");
        //    sqls.Append("update PERFO_KHS_MBJG set status='61' where  KHDZBID in (select id from PERFO_KHS_KHDZB where khid=" + khid + ");");
        //    sqls.Append(" end;");
        //    return dal.ExecuteSQL(sqls.ToString());
        //}
        public bool Update61(string khid)
        {
            StringBuilder sqls = new StringBuilder();
            sqls.Append("begin ");
            sqls.Append("update PERFO_KHS_MBJG set status='61' where  KHDZBID in (select id from PERFO_KHS_KHDZB where khid=" + khid + ");");
            sqls.Append(" end;");
            return dal.ExecuteSQL(sqls.ToString());
        }

        public bool Update62(string khid)
        {
            StringBuilder sqls = new StringBuilder();
            sqls.Append("begin ");
            sqls.Append("update PERFO_KHS_MBJG set status='62' where  KHDZBID in (select id from PERFO_KHS_KHDZB where khid=" + khid + ");");
            sqls.Append(" end;");
            return dal.ExecuteSQL(sqls.ToString());
        }
        #region ����������
        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly IKhsMbjgData dal = new KhsMbjgData();

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhsMbjgModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhsMbjgModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

	/// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhsMbjgModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhsMbjgModel model)
        {
            return dal.Execute(model);
        }
        #endregion
        public string GetStatus(KhsMbjgModel model)
        {
            string sName = GetStatus2(model.STATUS);
            

            return sName;
        }
        public string GetStatus2(string status)
        {
            string sName = "���ڴ��";
            //0=δ��� 1=���ͨ��  2=��˲�ͨ��  3=��ӡ���
            switch (status)
            {
                case "30":
                    sName = "<span style='color:#ffcc00'>�ύ<span style='color:#6600cc'>���ز����쵼</span>���</span>";
                    break;
                case "31":
                    sName = "<span style='color:#339900'><span style='color:#6600cc'>���ز����쵼</span>���ͨ��</span>";
                    break;
                case "32":
                    sName = "<span style='color:#ff0000'><span style='color:#6600cc'>���ز����쵼</span>��˲�ͨ��</span>";
                    break;
                case "40":
                    sName = "<span style='color:#ffcc00'>�ύ<span style='color:#6600cc'>���㵥λ</span>ȷ��</span>";
                    break;
                case "41":
                    sName = "<span style='color:#339900'><span style='color:#6600cc'>���㵥λ</span>ȷ��ͨ��</span>";
                    break;
                case "42":
                    sName = "<span style='color:#ff0000'><span style='color:#6600cc'>���㵥λ</span>ȷ�ϲ�ͨ��</span>";
                    break;
                case "50":
                    sName = "<span style='color:#ffcc00'>�ύ<span style='color:#6600cc'>��Ч���˰�</span>���</span>";
                    break;
                case "51":
                    sName = "<span style='color:#339900'><span style='color:#6600cc'>��Ч���˰�</span>���ͨ��</span>";
                    break;
                case "52":
                    sName = "<span style='color:#ff0000'><span style='color:#6600cc'>��Ч���˰�</span>��˲�ͨ��</span>";
                    break;
                case "60":
                    sName = "<span style='color:#ffcc00'>�ύ<span style='color:#6600cc'>�ֹ��쵼</span>���</span>";
                    break;
                case "61":
                    sName = "<span style='color:#339900'><span style='color:#6600cc'>�ֹ��쵼</span>���ͨ��</span>";
                    break;
                case "62":
                    sName = "<span style='color:#ff0000'><span style='color:#6600cc'>�ֹ��쵼</span>��ͨ��</span>";
                    break;
                
            }

            return sName;
        }

        public string GetStatus2(KhsMbjgModel model)
        {
            string sName = GetStatus21(model.STATUS);


            return sName;
        }
        public string GetStatus21(string status)
        {
            string sName = "<span style='color:#ffcc00'>���ڴ��</span>";
            //0=δ��� 1=���ͨ��  2=��˲�ͨ��  3=��ӡ���
            switch (status)
            {
                case "30":
                    sName = "<span style='color:#ffcc00'>�ύ<span style='color:#6600cc'>���ز����쵼</span>���</span>";
                    break;
                case "31":
                    sName = "<span style='color:#339900'><span style='color:#6600cc'>���ز����쵼</span>���ͨ��</span>";
                    break;
                case "32":
                    sName = "<span style='color:#ff0000'><span style='color:#6600cc'>���ز����쵼</span>��˲�ͨ��</span>";
                    break;
                case "40":
                    sName = "<span style='color:#ffcc00'>�ύ<span style='color:#6600cc'>רҵ�ֹ��쵼</span>���</span>";
                    break;
                case "41":
                    sName = "<span style='color:#339900'><span style='color:#6600cc'>רҵ�ֹ��쵼</span>���ͨ��</span>";
                    break;
                case "42":
                    sName = "<span style='color:#ff0000'><span style='color:#6600cc'>רҵ�ֹ��쵼</span>��˲�ͨ��</span>";
                    break;
                case "50":
                    sName = "<span style='color:#ffcc00'>�ύ<span style='color:#6600cc'>��Ч���˰�</span>����</span>";
                    break;
                case "51":
                    sName = "<span style='color:#339900'><span style='color:#6600cc'>��Ч���˰�</span>���ͨ��</span>";
                    break;
                case "52":
                    sName = "<span style='color:#ff0000'><span style='color:#6600cc'>��Ч���˰�</span>��˲�ͨ��</span>";
                    break;
                case "60":
                    sName = "<span style='color:#ffcc00'>�ύ<span style='color:#6600cc'>�ֹ��쵼</span>���</span>";
                    break;
                case "61":
                    sName = "<span style='color:#339900'><span style='color:#6600cc'>�ֹ��쵼</span>���ͨ��</span>";
                    break;
                case "62":
                    sName = "<span style='color:#ff0000'><span style='color:#6600cc'>�ֹ��쵼</span>��ͨ��</span>";
                    break;
            }

            return sName;
        }
    }

}
