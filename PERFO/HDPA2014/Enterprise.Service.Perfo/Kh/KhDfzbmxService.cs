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
using Enterprise.Model.Perfo.Zbk;
using System.Web.UI.WebControls;

namespace Enterprise.Service.Perfo.Kh
{
	
    /// <summary>
    /// 文件名:  KhDfzbmxService.cs
    /// 功能描述: 业务逻辑层-打分指标考核表数据处理
    /// 创建人：代码生成器
    /// 创建时间 ：2014/11/1 0:35:45
    /// </summary>
    public class KhDfzbmxService
    {

        /// <summary>
        /// 用户信息--服务类
        /// </summary>
        SysUserService userSrv = new SysUserService();

        #region 代码生成器

        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly IKhDfzbmxData dal = new KhDfzbmxData();

	    /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhDfzbmxModel GetSingle(string khid, string key)
        {
            return GetListByKhid(khid).FirstOrDefault(p => p.DFZBID == key);
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhDfzbmxModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhDfzbmxModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

	    /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhDfzbmxModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhDfzbmxModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region 自定义方法区

        /// <summary>
        /// 获取与指定考核期相关的所有打分数据
        /// </summary>
        /// <param name="khid">考核期</param>
        /// <returns></returns>
        public IList<KhDfzbmxModel> GetListByKhid(string khid)
        {
            string hql = "from KhDfzbmxModel p where p.KHID='" + khid + "'";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// 导入指定考核下的打分指标的基础数据
        /// </summary>
        /// <param name="khid">考核ID</param>
        /// <param name="khType">考核类型</param>
        /// <param name="zbxsModel">指标筛选MODEL</param>
        /// <param name="model">打分指标</param>
        /// <returns></returns>
        public bool LoadZrszbModelInDfzbmxByDfzb(string khid, WebKeys.KaoheType khType, KhZbsxModel zbxsModel, KhDfzbmxModel model)
        {
            KhJgbmdfbService jgbmdfbSrv = new KhJgbmdfbService();
            SysBmjgService bmjgSrv = new SysBmjgService();

            List<KhJgbmdfbModel> jgbmdfList = model.KhJgbmdfbLst.ToList();
            if (zbxsModel.JxzrsZb.Dfzb != null)
            {
                var dfzLst = zbxsModel.JxzrsZb.Dfzb.DfzLst.Where(p => p.DFZBBM == zbxsModel.SXZBBM);
                string dfzs = "";
                foreach (var dfr in dfzLst)
                {
                    dfzs += dfr.OPERATOR + ",";
                    //由多人打分时，同时存在打分表中
                    var users = GetUserListByDfz(dfr, model);
                    if (users.Count > 0)
                    {
                        foreach (var u in users)
                        {
                            KhJgbmdfbModel jgbmdfModel = new KhJgbmdfbModel();
                            jgbmdfModel.DB_Option_Action = WebKeys.InsertAction;
                            jgbmdfModel.DFBID = CommonTool.GetGuidKey(); //打分表ID
                            jgbmdfModel.ID = null; //量化考核表ID
                            jgbmdfModel.DFZBID = model.DFZBID;//打分指标ID
                            jgbmdfModel.ZBBM = model.ZBBM; //指标编码
                            jgbmdfModel.JGBM = model.JGBM; //机构编码
                            jgbmdfModel.KHID = model.KHID; //考核ID
                            jgbmdfModel.DFZ = u.LOGINID; //打分者，指定用户
                            if (zbxsModel.JxzrsZb.Dfzb.Zbxx.YJZBMC.Contains("机关作风"))
                            {
                                jgbmdfModel.DFZLX = ((int)WebKeys.DFUserType.上级领导).ToString();
                            }
                            else
                            {
                                jgbmdfModel.DFZLX = dfr.OPERTYPE; //打分者类型
                            }
                            //jgbmdfModel.DFQZ = dfr.OPERQZ.ToDecimal() / users.Count; //打分权重 打分时根据计算规则再行更新
                            if (!jgbmdfList.Exists(p => p.JGBM == jgbmdfModel.JGBM && p.DFZBID == jgbmdfModel.DFZBID
                                && p.DFZ == jgbmdfModel.DFZ))
                                jgbmdfList.Add(jgbmdfModel);//暂存
                        }
                    }
                    else
                    {
                        KhJgbmdfbModel jgbmdfModel = new KhJgbmdfbModel();
                        jgbmdfModel.DB_Option_Action = WebKeys.InsertAction;
                        jgbmdfModel.DFBID = CommonTool.GetGuidKey(); //打分表ID
                        jgbmdfModel.ID = null; //量化考核表ID
                        jgbmdfModel.DFZBID = model.DFZBID;//打分指标ID
                        jgbmdfModel.ZBBM = model.ZBBM; //指标编码
                        jgbmdfModel.JGBM = model.JGBM; //机构编码
                        jgbmdfModel.KHID = model.KHID; //考核ID
                        jgbmdfModel.DFZ = dfr.OPERATOR; //打分者，以单位ID代替
                        if (zbxsModel.JxzrsZb.Dfzb.Zbxx.YJZBMC.Contains("机关作风"))
                        {
                            //测检当前单位是部门还是二级单位
                            var bmjg = bmjgSrv.GetSingle(jgbmdfModel.DFZ);
                            if (bmjg != null)
                            {
                                if (bmjg.JGLX.Contains("职能"))
                                {
                                    jgbmdfModel.DFZLX = ((int)WebKeys.DFUserType.同级部门).ToString();
                                }
                                else if (bmjg.JGLX.Contains("二级"))
                                {
                                    jgbmdfModel.DFZLX = ((int)WebKeys.DFUserType.同级二级单位).ToString();
                                }
                            }
                            else
                            {
                                jgbmdfModel.DFZLX = dfr.OPERTYPE; //打分者类型
                            }
                        }
                        else
                        {
                            jgbmdfModel.DFZLX = dfr.OPERTYPE; //打分者类型
                        }
                        jgbmdfModel.DFZLX = dfr.OPERTYPE; //打分者类型
                        //jgbmdfModel.DFQZ = shr.OPERQZ; //打分权重 打分时根据计算规则再行更新
                        if (!jgbmdfList.Exists(p => p.JGBM == jgbmdfModel.JGBM && p.DFZBID == jgbmdfModel.DFZBID
                                && p.DFZ == jgbmdfModel.DFZ))
                            jgbmdfList.Add(jgbmdfModel);//暂存
                    }

                }
                model.DFZ = dfzs.TrimEnd(',');//打分者
            }
            return Execute(model) && jgbmdfbSrv.ExecuteByList(jgbmdfList);
        }

        /// <summary>
        /// 导入指定考核下的打分指标
        /// </summary>
        /// <param name="khid">考核ID</param>
        /// <param name="khType">考核类型</param>
        /// <param name="zbxsModel">指标筛选MODEL</param>
        /// <returns></returns>
        public bool LoadZrszbModelInDfzbmx(string khid, WebKeys.KaoheType khType, KhZbsxModel zbxsModel)
        {
            KhJgbmdfbService jgbmdfbSrv = new KhJgbmdfbService();
            SysBmjgService bmjgSrv = new SysBmjgService();
            List<KhJgbmdfbModel> jgbmdfList = new List<KhJgbmdfbModel>();

            KhDfzbmxModel model = new KhDfzbmxModel();
            model.DB_Option_Action = WebKeys.InsertAction;
            model.DFZBID = "DF" + CommonTool.GetPkId();//打分指标ID
            model.SXID = zbxsModel.SXID;//筛选表ID
            model.ZBBM = zbxsModel.SXZBBM;//指标编码
            model.JGBM = zbxsModel.SXJGBM; //机构编码
            model.KHID = khid.ToInt();//考核ID
            model.KHDX = ((int)khType).ToString();
            if (zbxsModel.JxzrsZb.Dfzb != null)
            {
                var dfzLst = zbxsModel.JxzrsZb.Dfzb.DfzLst.Where(p => p.DFZBBM == zbxsModel.SXZBBM);
                string dfzs = "";
                foreach (var dfr in dfzLst)
                {
                    dfzs += dfr.OPERATOR + ",";
                    //由多人打分时，同时存在打分表中
                    var users = GetUserListByDfz(dfr, model);
                    if (users.Count > 0)
                    {
                        foreach (var u in users)
                        {
                            KhJgbmdfbModel jgbmdfModel = new KhJgbmdfbModel();
                            jgbmdfModel.DB_Option_Action = WebKeys.InsertAction;
                            jgbmdfModel.DFBID = CommonTool.GetGuidKey(); //打分表ID
                            jgbmdfModel.ID = null; //量化考核表ID
                            jgbmdfModel.DFZBID = model.DFZBID;//打分指标ID
                            jgbmdfModel.ZBBM = model.ZBBM; //指标编码
                            jgbmdfModel.JGBM = model.JGBM; //机构编码
                            jgbmdfModel.KHID = model.KHID; //考核ID
                            jgbmdfModel.DFZ = u.LOGINID; //打分者，指定用户
                            if (zbxsModel.JxzrsZb.Dfzb.Zbxx.YJZBMC.Contains("机关作风"))
                            {
                                jgbmdfModel.DFZLX = ((int)WebKeys.DFUserType.上级领导).ToString();
                            }
                            else
                            {
                                jgbmdfModel.DFZLX = dfr.OPERTYPE; //打分者类型
                            }
                            //jgbmdfModel.DFQZ = dfr.OPERQZ.ToDecimal() / users.Count; //打分权重 打分时根据计算规则再行更新
                            if (!jgbmdfList.Exists(p => p.JGBM == jgbmdfModel.JGBM && p.DFZBID == jgbmdfModel.DFZBID
                                && p.DFZ == jgbmdfModel.DFZ))
                                jgbmdfList.Add(jgbmdfModel);//暂存
                        }
                    }
                    else
                    {
                        KhJgbmdfbModel jgbmdfModel = new KhJgbmdfbModel();
                        jgbmdfModel.DB_Option_Action = WebKeys.InsertAction;
                        jgbmdfModel.DFBID = CommonTool.GetGuidKey(); //打分表ID
                        jgbmdfModel.ID = null; //量化考核表ID
                        jgbmdfModel.DFZBID = model.DFZBID;//打分指标ID
                        jgbmdfModel.ZBBM = model.ZBBM; //指标编码
                        jgbmdfModel.JGBM = model.JGBM; //机构编码
                        jgbmdfModel.KHID = model.KHID; //考核ID
                        jgbmdfModel.DFZ = dfr.OPERATOR; //打分者，以单位ID代替
                        if (zbxsModel.JxzrsZb.Dfzb.Zbxx.YJZBMC.Contains("机关作风"))
                        {
                            //测检当前单位是部门还是二级单位
                            var bmjg = bmjgSrv.GetSingle(jgbmdfModel.DFZ);
                            if (bmjg != null)
                            {
                                if (bmjg.JGLX.Contains("职能"))
                                {
                                    jgbmdfModel.DFZLX = ((int)WebKeys.DFUserType.同级部门).ToString();
                                }
                                else if (bmjg.JGLX.Contains("二级"))
                                {
                                    jgbmdfModel.DFZLX = ((int)WebKeys.DFUserType.同级二级单位).ToString();
                                }
                            }
                            else
                            {
                                jgbmdfModel.DFZLX = dfr.OPERTYPE; //打分者类型
                            }
                        }
                        else
                        {
                            jgbmdfModel.DFZLX = dfr.OPERTYPE; //打分者类型
                        }
                        //jgbmdfModel.DFZLX = dfr.OPERTYPE; //打分者类型
                        //jgbmdfModel.DFQZ = shr.OPERQZ; //打分权重 打分时根据计算规则再行更新
                        if (!jgbmdfList.Exists(p => p.JGBM == jgbmdfModel.JGBM && p.DFZBID == jgbmdfModel.DFZBID
                                && p.DFZ == jgbmdfModel.DFZ))
                            jgbmdfList.Add(jgbmdfModel);//暂存
                    }

                }
                model.DFZ = dfzs.TrimEnd(',');//打分者
            }
            return Execute(model) && jgbmdfbSrv.ExecuteByList(jgbmdfList);
        }

        /// <summary>
        /// 获取打分者对应的具体用户信息集合
        /// </summary>
        /// <param name="dfz">打分者</param>
        /// <param name="dfzbmx">打分指标明细</param>
        /// <returns></returns>
        public IList<SysUserModel> GetUserListByDfz(ZbkDfzModel dfz, KhDfzbmxModel dfzbmx)
        {
            List<SysUserModel> users = new List<SysUserModel>();
            if (dfz.OPERATOR == "YQTLD")
            {
                users = userSrv.GetUserListByDuty("油气田领导") as List<SysUserModel>;
            }
            else if (dfz.OPERATOR == "FGLD")
            {
                var user = userSrv.GetUserByFgjgbm(dfzbmx.JGBM.Value);
                if (user != null)
                {
                    users.Add(user);
                }
            }
            return users;
        }

        /// <summary>
        /// 获取与指定考核期和机构编码相关的所有打分数据
        /// </summary>
        /// <param name="khid">考核期</param>
        /// <param name="jgbm">机构编码</param>
        /// <returns></returns>
        public IList<KhDfzbmxModel> GetListByKhidAndJgbm(string khid, string jgbm)
        {
            string hql = "from KhDfzbmxModel p where p.KHID='" + khid + "' and p.JGBM='" + jgbm + "'";
            return GetListByHQL(hql).OrderBy(p => p.ZbsxModel.SXXH).ToList();
        }

        /// <summary>
        /// 获取考核期和指标关联的各单位打分数据
        /// </summary>
        /// <param name="khid">考核期</param>
        /// <param name="zbid">指标ID</param>
        /// <param name="kaoheType">考核类型</param>
        /// <returns></returns>
        public IList<KhDfzbmxModel> GetListByKhidAndZhibiao(string khid, string zbid, WebKeys.KaoheType kaoheType)
        {
            string hql = "from KhDfzbmxModel p where p.KHID='" + khid + "' and p.ZBBM='" + zbid + "'";
            return GetListByHQL(hql).Where(p => p.KHDX == ((int)kaoheType).ToString()).OrderBy(p => p.Danwei.XSXH).ToList();
        }

        /// <summary>
        /// 获取指定考核期下的所有可打分的定性指标集合
        /// </summary>
        /// <param name="khid">考核ID</param>
        /// <returns></returns>
        public IList<KhDfzbmxModel> GetListByKhidAndKhdx(string khid, WebKeys.KaoheType kaoheType)
        {
            string hql = "from KhDfzbmxModel p where p.KHID='" + khid + "' and p.KHDX='" + ((int)kaoheType) + "'";
            return GetListByHQL(hql).OrderBy(p=>p.DfzbModel.Zbxx.YJZBMC).ThenBy(p => p.ZbsxModel.SXXH).ToList();
        }

        /// <summary>
        /// 绑定带分组功能的打分指标下拉控件
        /// </summary>
        /// <param name="ddl">下拉控件</param>
        /// <param name="dfzbmxLst">打分指标明细</param>
        public void BindSSECDropDownListForDfzb(SSECDropDownList ddl, IList<KhDfzbmxModel> dfzbmxLst)
        {
            ddl.Items.Clear();
            string zblx = string.Empty;
            List<string> itemKeys = new List<string>();
            foreach (var q in dfzbmxLst)
            {
                if (string.IsNullOrEmpty(zblx))
                {
                    ddl.Items.Add(new ListItem(q.DfzbModel.Zbxx.YJZBMC, "optgroup"));
                    ddl.Items.Add(new ListItem(q.DfzbModel.Zbxx.ZBMC, q.ZBBM));
                    zblx = q.DfzbModel.Zbxx.YJZBMC;
                    itemKeys.Add(q.DfzbModel.Zbxx.YJZBMC + "※" + q.DfzbModel.Zbxx.ZBMC);
                }
                else if (q.DfzbModel.Zbxx.YJZBMC == zblx)
                {
                    if (!itemKeys.Exists(p => p == q.DfzbModel.Zbxx.YJZBMC + "※" + q.DfzbModel.Zbxx.ZBMC))
                    {
                        ddl.Items.Add(new ListItem(q.DfzbModel.Zbxx.ZBMC, q.ZBBM));
                    }
                }
                else
                {
                    if (!itemKeys.Exists(p => p == q.DfzbModel.Zbxx.YJZBMC + "※" + q.DfzbModel.Zbxx.ZBMC))
                    {
                        ddl.Items.Add(new ListItem(q.DfzbModel.Zbxx.YJZBMC, "optgroup"));
                        ddl.Items.Add(new ListItem(q.DfzbModel.Zbxx.ZBMC, q.ZBBM));
                        zblx = q.DfzbModel.Zbxx.YJZBMC;
                    }
                }
            }
        }

        #endregion
    }

}
