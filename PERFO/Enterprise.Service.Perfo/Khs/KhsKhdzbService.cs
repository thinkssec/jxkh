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
    /// �ļ���:  KhsKhdzbService.cs
    /// ��������: ҵ���߼���-����ָ����ձ����ݴ���
    /// �����ˣ�����������
	/// ����ʱ�� ��2015/11/4 20:47:10
    /// </summary>
    public class KhsKhdzbService
    {
        #region ����������
        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly IKhsKhdzbData dal = new KhsKhdzbData();

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhsKhdzbModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhsKhdzbModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

	/// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhsKhdzbModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhsKhdzbModel model)
        {
            return dal.Execute(model);
        }

        public bool DeleteByKhId(string khId)
        {
            StringBuilder sqls = new StringBuilder();
            sqls.Append("begin ");
            sqls.Append("delete from PERFO_KHS_KHDZB where KHID=" + khId + ";");
            sqls.Append(" end;");
            return dal.ExecuteSQL(sqls.ToString());
        }

        public bool DeleteMb(string mbid)
        {
            StringBuilder sqls = new StringBuilder();
            sqls.Append("begin ");
            sqls.Append("delete from PERFO_KHX_RESULT where mbjgid in (select id from  PERFO_KHS_MBJG where  mbid='" + mbid + "');delete from PERFO_KHS_MBJG where mbid='" + mbid + "';delete from PERFO_KHX_ZB where mbid='" + mbid + "';");
            sqls.Append(" end;");
            return dal.ExecuteSQL(sqls.ToString());
        }
        #endregion

        public string GetStatus(KhsKhdzbModel model)
        {
            string sName = "�ƶ���";
            //0=δ��� 1=���ͨ��  2=��˲�ͨ��  3=��ӡ���
            switch (model.STATUS)
            {
                case 0:
                    sName = "���ύ���";
                    break;
                case 2:
                    sName = "��˲�ͨ��";
                    break;
                case 1:
                    sName = "���ͨ��";
                    break;
            }

            return sName;
        }
    }

}
