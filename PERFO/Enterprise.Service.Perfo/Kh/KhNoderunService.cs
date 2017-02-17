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
    /// �ļ���:  KhNoderunService.cs
    /// ��������: ҵ���߼���-���˽ڵ����б����ݴ���
    /// �����ˣ�����������
    /// ����ʱ�� ��2014/11/1 0:35:45
    /// </summary>
    public class KhNoderunService
    {
        #region ����������

        /// <summary>
        /// �õ����ݷ�����ʵ��
        /// </summary>
        private static readonly IKhNoderunData dal = new KhNoderunData();

        /// <summary>
        /// ����������ȡΨһ��¼
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhNoderunModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// ��ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhNoderunModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// ����������ȡ���ݼ���
        /// </summary>
        /// <returns></returns>
        public IList<KhNoderunModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// ����ԭ��SQL�Ĳ�ѯ�б�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhNoderunModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// ִ����ӡ��޸ġ�ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhNoderunModel model)
        {
            return dal.Execute(model);
        }

        /// <summary>
        /// ����ɾ��ָ���������µ��������нڵ�
        /// </summary>
        /// <param name="khid"></param>
        /// <returns></returns>
        public bool DeleteRunNodesByKhid(string khid)
        {
            string sql = "delete from PERFO_KH_NODERUN where KHID='" + khid + "'";
            return dal.ExecuteSQL(sql);
        }

        #endregion

        #region �Զ��巽����

        /// <summary>
        /// ��ȡָ���������µ��������нڵ���Ϣ
        /// </summary>
        /// <param name="khid"></param>
        /// <returns></returns>
        public IList<KhNoderunModel> GetListByKhid(string khid)
        {
            string hql = "from KhNoderunModel p where p.KHID='" + khid + "'";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// ��ʼ��ָ�����˵����нڵ�����
        /// </summary>
        /// <param name="kaohe">����ʵ��</param>
        /// <returns></returns>
        public bool InitNoderunData(KhKhglModel kaohe)
        {
            bool isOk = DeleteRunNodesByKhid(kaohe.KHID.ToString());
            KhNodesService nodeSrv = new KhNodesService();
            var nodeLst = nodeSrv.GetList().Where(p=>p.LXID == kaohe.LXID).OrderBy(p=>p.JDXH).ToList();
            foreach (var node in nodeLst)
            {
                KhNoderunModel runM = new KhNoderunModel();
                runM.DB_Option_Action = WebKeys.InsertAction;
                runM.YXID = "RUN" + CommonTool.GetPkId();
                runM.YXZT = "0";
                runM.YXKSSJ = DateTime.Now;
                runM.YXJDZ = node.JDZ;
                runM.KHID = kaohe.KHID;
                runM.JDID = node.JDID;
                isOk = Execute(runM);
            }
            return isOk;
        }

        /// <summary>
        /// ���¶�����λָ�����µĿ��˽ڵ������״̬
        /// </summary>
        /// <param name="khid">�����ڶ���</param>
        /// <param name="khNode">�ڵ�</param>
        /// <returns></returns>
        public bool UpdateEjdwNodeByKhid(string khid, WebKeys.EjdwKhNode khNode)
        {
            var nodeRunModel = GetListByKhid(khid).FirstOrDefault(p => p.JdNode.JDMC == khNode.ToString());
            if (nodeRunModel != null)
            {
                nodeRunModel.DB_Option_Action = WebKeys.UpdateAction;
                nodeRunModel.YXWCSJ = DateTime.Now;
                nodeRunModel.YXZT = "1";
            }
            return Execute(nodeRunModel);
        }

        /// <summary>
        /// ���»��ز���ָ�����µĿ��˽ڵ������״̬
        /// </summary>
        /// <param name="khid">�����ڶ���</param>
        /// <param name="khNode">�ڵ�</param>
        /// <returns></returns>
        public bool UpdateJgbmNodeByKhid(string khid, WebKeys.JgbmKhNode khNode)
        {
            var nodeRunModel = GetListByKhid(khid).FirstOrDefault(p => p.JdNode.JDMC == khNode.ToString());
            if (nodeRunModel != null)
            {
                nodeRunModel.DB_Option_Action = WebKeys.UpdateAction;
                nodeRunModel.YXWCSJ = DateTime.Now;
                nodeRunModel.YXZT = "1";
            }
            return Execute(nodeRunModel);
        }

        #endregion

    }

}
