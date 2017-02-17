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
    /// 文件名:  KhNoderunService.cs
    /// 功能描述: 业务逻辑层-考核节点运行表数据处理
    /// 创建人：代码生成器
    /// 创建时间 ：2014/11/1 0:35:45
    /// </summary>
    public class KhNoderunService
    {
        #region 代码生成器

        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly IKhNoderunData dal = new KhNoderunData();

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhNoderunModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhNoderunModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhNoderunModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhNoderunModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhNoderunModel model)
        {
            return dal.Execute(model);
        }

        /// <summary>
        /// 批量删除指定考核期下的所有运行节点
        /// </summary>
        /// <param name="khid"></param>
        /// <returns></returns>
        public bool DeleteRunNodesByKhid(string khid)
        {
            string sql = "delete from PERFO_KH_NODERUN where KHID='" + khid + "'";
            return dal.ExecuteSQL(sql);
        }

        #endregion

        #region 自定义方法区

        /// <summary>
        /// 获取指定考核期下的所有运行节点信息
        /// </summary>
        /// <param name="khid"></param>
        /// <returns></returns>
        public IList<KhNoderunModel> GetListByKhid(string khid)
        {
            string hql = "from KhNoderunModel p where p.KHID='" + khid + "'";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// 初始化指定考核的运行节点数据
        /// </summary>
        /// <param name="kaohe">考核实例</param>
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
        /// 更新二级单位指定期下的考核节点的运行状态
        /// </summary>
        /// <param name="khid">考核期对象</param>
        /// <param name="khNode">节点</param>
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
        /// 更新机关部门指定期下的考核节点的运行状态
        /// </summary>
        /// <param name="khid">考核期对象</param>
        /// <param name="khNode">节点</param>
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
