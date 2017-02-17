using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Data.Perfo.Kh;
using Enterprise.Model.Perfo.Kh;
using Enterprise.Model.Perfo.Zbk;
using Enterprise.Model.Perfo.Sys;
using Enterprise.Service.Perfo.Sys;
using Enterprise.Service.Perfo.Zbk;

namespace Enterprise.Service.Perfo.Kh
{
	
    /// <summary>
    /// 文件名:  KhDlzbmxService.cs
    /// 功能描述: 业务逻辑层-定量指标考核表数据处理
    /// 创建人：代码生成器
    /// 创建时间 ：2014/11/1 0:35:45
    /// </summary>
    public class KhDlzbmxService
    {

        /// <summary>
        /// 用户信息--服务类
        /// </summary>
        SysUserService userSrv = new SysUserService();
        /// <summary>
        /// 计算规则-服务类
        /// </summary>
        ZbkJsgzService jsgzSrv = new ZbkJsgzService();
        /// <summary>
        /// 具有得分的定量指标数据集合
        /// </summary>
        List<KhDlzbmxModel> finishList = new List<KhDlzbmxModel>();

        #region 代码生成器

        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly IKhDlzbmxData dal = new KhDlzbmxData();

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhDlzbmxModel GetSingle(string khid, string key)
        {
            string hql = "from KhDlzbmxModel p where p.KHID='" + khid + "'";
            return GetListByHQL(hql).FirstOrDefault(p => p.ID == key);
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhDlzbmxModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhDlzbmxModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhDlzbmxModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhDlzbmxModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region 自定义方法区

        /// <summary>
        /// 执行批量添加、修改、删除操作
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public bool ExecuteByList(List<KhDlzbmxModel> models)
        {
            bool isOk = false;
            foreach (var m in models)
            {
                isOk = dal.Execute(m);
            }
            return isOk;
        }

        /// <summary>
        /// 导入指定考核下的量化指标的基础数据
        /// </summary>
        /// <param name="khid">考核ID</param>
        /// <param name="khType">考核类型</param>
        /// <param name="zbxsModel">指标筛选MODEL</param>
        /// <param name="model">定量指标</param>
        /// <returns></returns>
        public bool LoadZrszbModelInDlzbmxByDlzb(string khid, WebKeys.KaoheType khType, KhZbsxModel zbxsModel, KhDlzbmxModel model)
        {
            KhJgbmdfbService jgbmdfbSrv = new KhJgbmdfbService();//打分表
            KhLhzbjcsjService lhzbjcsjSrv = new KhLhzbjcsjService();//基础数据表
            SysBmjgService bmjgSrv = new SysBmjgService();//机构

            List<KhJgbmdfbModel> jgbmdfList = model.KhJgbmdfbLst.ToList();
            List<KhLhzbjcsjModel> lhzbjcsjList = model.LhzbjcsjLst.ToList();
            if (zbxsModel.JxzrsZb.Lhzb != null)
            {
                //计算表达式不为空，则还要分析表达式内容并存入基础数据表
                if (!string.IsNullOrEmpty(zbxsModel.JxzrsZb.Lhzb.JSBDS))
                {
                    string bds = zbxsModel.JxzrsZb.Lhzb.JSBDS;
                    bds = bds.Substring(bds.IndexOf('=') + 1);
                    List<string> values = Utility.GetMatchValues(bds, @"\{.*?\}");
                    int index = 1;
                    foreach (var v in values)
                    {
                        KhLhzbjcsjModel jcsjM = new KhLhzbjcsjModel();
                        jcsjM.DB_Option_Action = WebKeys.InsertAction;
                        jcsjM.JCZBID = CommonTool.GetGuidKey(); //基础指标ID
                        jcsjM.ID = model.ID;//量化考核表ID
                        jcsjM.ZBBM = model.ZBBM;//指标编码
                        jcsjM.JGBM = model.JGBM;//机构编码
                        jcsjM.KHID = model.KHID;//考核ID
                        jcsjM.ZBDH = v;//指标代号
                        jcsjM.ZBMC = v.Trim('{', '}');//指标名称
                        jcsjM.XH = index++;
                        if (!lhzbjcsjList.Exists(p => p.ID == model.ID && p.ZBDH == jcsjM.ZBDH))
                            lhzbjcsjList.Add(jcsjM);
                    }
                }

                //目标值审核
                var mbzshrLst = zbxsModel.JxzrsZb.Lhzb.MbzshLst.Where(p => p.LHZBBM == zbxsModel.SXZBBM);
                string mbzshr = "";
                foreach (var shr in mbzshrLst)
                {
                    mbzshr += shr.OPERATOR + ",";
                }
                model.MBZQRR = mbzshr.TrimEnd(',');
                //暂时不用了11.27
                //if (khType == WebKeys.KaoheType.机关部门)
                //{
                //    //目标值确认人默认为分管领导
                //    model.MBZQRR = "FGLD";
                //}

                //完成值审核
                var wczshrLst = zbxsModel.JxzrsZb.Lhzb.WczshdfLst.Where(p => p.LHZBBM == zbxsModel.SXZBBM);
                string wczshr = "";
                foreach (var shr in wczshrLst)
                {
                    wczshr += shr.OPERATOR + ",";
                    //完成值由多人审核时，同时存在打分表中
                    var users = GetUserListByWczshr(shr, model);
                    if (users.Count > 0)
                    {
                        foreach (var u in users)
                        {
                            KhJgbmdfbModel jgbmdfModel = new KhJgbmdfbModel();
                            jgbmdfModel.DB_Option_Action = WebKeys.InsertAction;
                            jgbmdfModel.DFBID = CommonTool.GetGuidKey(); //打分表ID
                            jgbmdfModel.ID = model.ID; //量化考核表ID
                            jgbmdfModel.DFZBID = null;//打分指标ID
                            jgbmdfModel.ZBBM = model.ZBBM; //指标编码
                            jgbmdfModel.JGBM = model.JGBM; //机构编码
                            jgbmdfModel.KHID = model.KHID; //考核ID
                            jgbmdfModel.DFZ = u.LOGINID; //打分者，指定用户
                            if (zbxsModel.JxzrsZb.Lhzb.Zbxx.YJZBMC.Contains("机关部门"))
                            {
                                jgbmdfModel.DFZLX = ((int)WebKeys.DFUserType.上级领导).ToString();
                            }
                            else
                            {
                                jgbmdfModel.DFZLX = shr.OPERTYPE; //打分者类型
                            }
                            //jgbmdfModel.DFQZ = shr.OPERQZ.ToDecimal()/users.Count; //打分权重 打分时根据计算规则再行更新
                            if (!jgbmdfList.Exists(p => p.JGBM == jgbmdfModel.JGBM && p.ID == jgbmdfModel.ID
                                && p.DFZ == jgbmdfModel.DFZ))
                                jgbmdfList.Add(jgbmdfModel);//暂存
                        }
                    }
                    else
                    {
                        KhJgbmdfbModel jgbmdfModel = new KhJgbmdfbModel();
                        jgbmdfModel.DB_Option_Action = WebKeys.InsertAction;
                        jgbmdfModel.DFBID = CommonTool.GetGuidKey(); //打分表ID
                        jgbmdfModel.ID = model.ID; //量化考核表ID
                        jgbmdfModel.DFZBID = null;//打分指标ID
                        jgbmdfModel.ZBBM = model.ZBBM; //指标编码
                        jgbmdfModel.JGBM = model.JGBM; //机构编码
                        jgbmdfModel.KHID = model.KHID; //考核ID
                        //自评时，直接取机构编码
                        jgbmdfModel.DFZ = ((shr.OPERATOR == "ZIPING") ? model.JGBM.ToRequestString() : shr.OPERATOR); //打分者，以单位ID代替
                        if (zbxsModel.JxzrsZb.Lhzb.Zbxx.YJZBMC.Contains("机关部门"))
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
                                jgbmdfModel.DFZLX = shr.OPERTYPE; //打分者类型
                            }
                        }
                        else
                        {
                            jgbmdfModel.DFZLX = shr.OPERTYPE; //打分者类型
                        }
                        //jgbmdfModel.DFQZ = shr.OPERQZ; //打分权重 打分时根据计算规则再行更新
                        if (!jgbmdfList.Exists(p => p.JGBM == jgbmdfModel.JGBM && p.ID == jgbmdfModel.ID
                                && p.DFZ == jgbmdfModel.DFZ))
                            jgbmdfList.Add(jgbmdfModel);//暂存
                    }
                }
                model.WCZSHR = wczshr.TrimEnd(',');
            }
            model.WCZSQR = zbxsModel.SXJGBM.ToRequestString();
            return Execute(model) &&
                jgbmdfbSrv.ExecuteByList(jgbmdfList) && lhzbjcsjSrv.ExecuteByList(lhzbjcsjList);
        }

        /// <summary>
        /// 导入指定考核下的量化指标
        /// </summary>
        /// <param name="khid">考核ID</param>
        /// <param name="khType">考核类型</param>
        /// <param name="zbxsModel">指标筛选MODEL</param>
        /// <returns></returns>
        public bool LoadZrszbModelInDlzbmx(string khid, WebKeys.KaoheType khType, KhZbsxModel zbxsModel)
        {
            KhJgbmdfbService jgbmdfbSrv = new KhJgbmdfbService();//打分表
            List<KhJgbmdfbModel> jgbmdfList = new List<KhJgbmdfbModel>();
            KhLhzbjcsjService lhzbjcsjSrv = new KhLhzbjcsjService();//基础数据表
            List<KhLhzbjcsjModel> lhzbjcsjList = new List<KhLhzbjcsjModel>();
            SysBmjgService bmjgSrv = new SysBmjgService();//机构

            KhDlzbmxModel model = new KhDlzbmxModel();
            model.ID = "LH" + CommonTool.GetPkId();
            model.DB_Option_Action = WebKeys.InsertAction;
            model.KHID = khid.ToInt();
            model.JGBM = zbxsModel.SXJGBM;
            model.KHDX = ((int)khType).ToString();
            model.ZBKHZT = "0";
            model.ZBBM = zbxsModel.SXZBBM;
            model.SXID = zbxsModel.SXID;
            model.NCMBZ = zbxsModel.JxzrsZb.ZMBZ;
            //model.MBZBZ = zbxsModel.JxzrsZb.MBZBZ;
            if (zbxsModel.JxzrsZb.Lhzb != null)
            {
                //计算表达式不为空，则还要分析表达式内容并存入基础数据表
                if (!string.IsNullOrEmpty(zbxsModel.JxzrsZb.Lhzb.JSBDS))
                {
                    string bds = zbxsModel.JxzrsZb.Lhzb.JSBDS;
                    bds = bds.Substring(bds.IndexOf('=')+1);
                    List<string> values = Utility.GetMatchValues(bds, @"\{.*?\}");
                    int index = 1;
                    foreach (var v in values)
                    {
                        KhLhzbjcsjModel jcsjM = new KhLhzbjcsjModel();
                        jcsjM.DB_Option_Action = WebKeys.InsertAction;
                        jcsjM.JCZBID = CommonTool.GetGuidKey(); //基础指标ID
                        jcsjM.ID = model.ID;//量化考核表ID
                        jcsjM.ZBBM = model.ZBBM;//指标编码
                        jcsjM.JGBM = model.JGBM;//机构编码
                        jcsjM.KHID = model.KHID;//考核ID
                        jcsjM.ZBDH = v;//指标代号
                        jcsjM.ZBMC = v.Trim('{', '}');//指标名称
                        jcsjM.XH = index++;
                        if (!lhzbjcsjList.Exists(p=>p.ID == model.ID && p.ZBDH == jcsjM.ZBDH))
                            lhzbjcsjList.Add(jcsjM);
                    }
                }

                //目标值审核
                var mbzshrLst = zbxsModel.JxzrsZb.Lhzb.MbzshLst.Where(p=>p.LHZBBM == zbxsModel.SXZBBM);
                string mbzshr = "";
                foreach (var shr in mbzshrLst)
                {
                    mbzshr += shr.OPERATOR + ",";
                }
                model.MBZQRR = mbzshr.TrimEnd(',');
                //暂时不用了11.27
                //if (khType == WebKeys.KaoheType.机关部门)
                //{
                //    //目标值确认人默认为分管领导
                //    model.MBZQRR = "FGLD";
                //}

                //完成值审核
                var wczshrLst = zbxsModel.JxzrsZb.Lhzb.WczshdfLst.Where(p => p.LHZBBM == zbxsModel.SXZBBM);
                string wczshr = "";
                foreach (var shr in wczshrLst)
                {
                    wczshr += shr.OPERATOR + ",";
                    //完成值由多人审核时，同时存在打分表中
                    var users = GetUserListByWczshr(shr, model);
                    if (users.Count > 0)
                    {
                        foreach (var u in users)
                        {
                            KhJgbmdfbModel jgbmdfModel = new KhJgbmdfbModel();
                            jgbmdfModel.DB_Option_Action = WebKeys.InsertAction;
                            jgbmdfModel.DFBID = CommonTool.GetGuidKey(); //打分表ID
                            jgbmdfModel.ID = model.ID; //量化考核表ID
                            jgbmdfModel.DFZBID = null;//打分指标ID
                            jgbmdfModel.ZBBM = model.ZBBM; //指标编码
                            jgbmdfModel.JGBM = model.JGBM; //机构编码
                            jgbmdfModel.KHID = model.KHID; //考核ID
                            jgbmdfModel.DFZ = u.LOGINID; //打分者，指定用户
                            if (zbxsModel.JxzrsZb.Lhzb.Zbxx.YJZBMC.Contains("机关部门"))
                            {
                                jgbmdfModel.DFZLX = ((int)WebKeys.DFUserType.上级领导).ToString();
                            }
                            else
                            {
                                jgbmdfModel.DFZLX = shr.OPERTYPE; //打分者类型
                            }
                            //jgbmdfModel.DFQZ = shr.OPERQZ.ToDecimal()/users.Count; //打分权重 打分时根据计算规则再行更新
                            if (!jgbmdfList.Exists(p => p.JGBM == jgbmdfModel.JGBM && p.ID == jgbmdfModel.ID 
                                && p.DFZ == jgbmdfModel.DFZ))
                                jgbmdfList.Add(jgbmdfModel);//暂存
                        }
                    }
                    else
                    {
                        KhJgbmdfbModel jgbmdfModel = new KhJgbmdfbModel();
                        jgbmdfModel.DB_Option_Action = WebKeys.InsertAction;
                        jgbmdfModel.DFBID = CommonTool.GetGuidKey(); //打分表ID
                        jgbmdfModel.ID = model.ID; //量化考核表ID
                        jgbmdfModel.DFZBID = null;//打分指标ID
                        jgbmdfModel.ZBBM = model.ZBBM; //指标编码
                        jgbmdfModel.JGBM = model.JGBM; //机构编码
                        jgbmdfModel.KHID = model.KHID; //考核ID
                        //自评时，直接取机构编码
                        jgbmdfModel.DFZ = ((shr.OPERATOR == "ZIPING") ? model.JGBM.ToRequestString() : shr.OPERATOR); //打分者，以单位ID代替
                        if (zbxsModel.JxzrsZb.Lhzb.Zbxx.YJZBMC.Contains("机关部门"))
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
                                jgbmdfModel.DFZLX = shr.OPERTYPE; //打分者类型
                            }
                        }
                        else
                        {
                            jgbmdfModel.DFZLX = shr.OPERTYPE; //打分者类型
                        }
                        //jgbmdfModel.DFQZ = shr.OPERQZ; //打分权重 打分时根据计算规则再行更新
                        if (!jgbmdfList.Exists(p => p.JGBM == jgbmdfModel.JGBM && p.ID == jgbmdfModel.ID
                                && p.DFZ == jgbmdfModel.DFZ))
                            jgbmdfList.Add(jgbmdfModel);//暂存
                    }
                }
                model.WCZSHR = wczshr.TrimEnd(',');
            }
            model.WCZSQR = zbxsModel.SXJGBM.ToRequestString();
            return Execute(model) && 
                jgbmdfbSrv.ExecuteByList(jgbmdfList) && lhzbjcsjSrv.ExecuteByList(lhzbjcsjList);
        }

        /// <summary>
        /// 获取完成值审核人对应的具体用户信息集合
        /// </summary>
        /// <param name="dfz">完成值审核或打分者</param>
        /// <param name="dlzbmx">定量指标明细</param>
        /// <returns></returns>
        public IList<SysUserModel> GetUserListByWczshr(ZbkWczshdfModel dfz, KhDlzbmxModel dlzbmx)
        {
            List<SysUserModel> users = new List<SysUserModel>();
            if (dfz.OPERATOR == "YQTLD")
            {
                users = userSrv.GetUserListByDuty("油气田领导") as List<SysUserModel>;
            }
            else if (dfz.OPERATOR == "FGLD")
            {
                var user = userSrv.GetUserByFgjgbm(dlzbmx.JGBM.Value);
                if (user != null)
                {
                    users.Add(user);
                }
            }
            return users;
        }

        /// <summary>
        /// 获取指定考核ID的量化指标信息
        /// </summary>
        /// <param name="khid">考核ID</param>
        /// <returns></returns>
        public IList<KhDlzbmxModel> GetListByKhid(string khid)
        {
            string hql = "from KhDlzbmxModel p where p.KHID='" + khid + "'";
            return GetListByHQL(hql).OrderBy(p=>p.JGBM).ThenBy(p => p.ZbsxModel.SXXH).ToList();
        }

        /// <summary>
        /// 获取指定考核ID和机构编码对应的量化指标信息
        /// </summary>
        /// <param name="khid">考核ID</param>
        /// <param name="jgbm">机构编码</param>
        /// <returns></returns>
        public IList<KhDlzbmxModel> GetListByKhidAndJgbm(string khid, string jgbm)
        {
            string hql = "from KhDlzbmxModel p where p.KHID='" + khid + "' and p.JGBM='" + jgbm + "'";
            return GetListByHQL(hql).OrderBy(p => p.ZbsxModel.SXXH).ToList();
        }

        /// <summary>
        /// 获取指定考核ID和指标编码对应的量化指标信息
        /// </summary>
        /// <param name="khid">考核ID</param>
        /// <param name="zbbm">指标编码</param>
        /// <returns></returns>
        public IList<KhDlzbmxModel> GetListByKhidAndZbbm(string khid, string zbbm)
        {
            string hql = "from KhDlzbmxModel p where p.KHID='" + khid + "' and p.ZBBM='" + zbbm + "'";
            return GetListByHQL(hql).OrderBy(p => p.ZbsxModel.SXXH).ToList();
        }

        /// <summary>
        /// 根据计算关系式计算关联指标的实际完成值
        /// </summary>
        /// <param name="model">量化指标对象</param>
        /// <param name="glzbs">关联指标集合</param>
        /// <returns></returns>
        public decimal CalculateGlzbsWcz(KhDlzbmxModel model, List<KhLhzbjcsjModel> glzbs)
        {
            decimal result = 0M;
            string bds = model.LhzbModel.JSBDS;
            bds = bds.Substring(bds.IndexOf('=') + 1);
            foreach (var glzb in glzbs)
            {
                bds = bds.Replace(glzb.ZBDH, "(" + glzb.ZBZ.ToDecimal().ToString() + ")");
            }
            result = Utility.Eval(bds).ToDecimal();
            return result;
        }

        /// <summary>
        /// 根据计算关系式计算关联指标的目标值
        /// </summary>
        /// <param name="model">量化指标对象</param>
        /// <param name="glzbs">关联指标集合</param>
        /// <returns></returns>
        public decimal CalculateGlzbsMbz(KhDlzbmxModel model, List<KhLhzbjcsjModel> glzbs)
        {
            decimal result = 0M;
            string bds = model.LhzbModel.JSBDS;
            bds = bds.Substring(bds.IndexOf('=') + 1);
            foreach (var glzb in glzbs)
            {
                bds = bds.Replace(glzb.ZBDH, "(" + glzb.ZBSHZ.ToDecimal().ToString() + ")");
            }
            result = Utility.Eval(bds).ToDecimal();
            return result;
        }

        #region 财务基础数据表相关

        /// <summary>
        /// 获取财务基础数据表中的指定考核期和单位编码、指定年份的数据并设置到明细表
        /// </summary>
        /// <param name="khid">考核期</param>
        /// <param name="jgbm">单位编码</param>
        /// <param name="yy">年份</param>
        /// <returns></returns>
        public bool SetCwjcsjDataByKhid_Jgbm_Year(string khid, string jgbm, int yy)
        {
            /*
             1、提取指定单位和考核期下的量化指标基础数据
             2、提取指定单位和年份下的财务基础数据
             3、提取财务数据与关联指标项的对应关系
             4、依次将数据设置到量化指标基础数据表中
             */
            bool isOk = true;
            KhKhglService khglSrv = new KhKhglService();
            var kaohe = khglSrv.GetSingle(khid);
            if (kaohe == null) return false;

            KhLhzbjcsjService lhzbjcsjSrv = new KhLhzbjcsjService();
            //1==量化指标基础数据
            List<KhLhzbjcsjModel> jcsjList = lhzbjcsjSrv.GetListByKhidAndJgbm(khid, jgbm) as List<KhLhzbjcsjModel>;

            KhCwjcsjService cwjcsjSrv = new KhCwjcsjService();
            //2==获取指定年份的财务数据
            var cwjcsjForYear = cwjcsjSrv.GetListByJgbmAndNF(jgbm, yy);

            //3==财务基础指标关联数据
            ZbkCwjcsjglzbService cwjcsjglzbSrv = new ZbkCwjcsjglzbService();
            var cwjcsjGlzbLst = cwjcsjglzbSrv.GetList();

            //4==依次将数据设置到量化指标基础数据表中
            foreach (KhLhzbjcsjModel jcsj in jcsjList)
            {
                var cwglzb = cwjcsjGlzbLst.FirstOrDefault(p => p.ZBXMC == jcsj.ZBMC && !string.IsNullOrEmpty(p.JCSJZB));
                if (cwglzb != null)
                {
                    KhCwjcsjModel cwjcsj = cwjcsjForYear.FirstOrDefault(p => p.ZBMC == cwglzb.JCSJZB);
                    if (cwjcsj != null)
                    {
                        jcsj.DB_Option_Action = WebKeys.UpdateAction;
                        jcsj.ZBSHZ = getCwsjData(kaohe, cwjcsj);
                        //if (cwjcsj.ZBMC.Contains("累计"))
                        //{
                        //    //cwjcsj.LJZ.ToDecimal();
                        //}
                        //else if (cwjcsj.ZBMC.Contains("平均"))
                        //{
                        //    jcsj.ZBSHZ = getCwsjData(kaohe, cwjcsj);//cwjcsj.PJZ.ToDecimal();
                        //}
                        jcsj.ZBSHBZ = yy + "年数据";
                        isOk = lhzbjcsjSrv.Execute(jcsj);
                    }
                }
            }
            return isOk;
        }

        /// <summary>
        /// 获取财务基础数据表中的指定考核期和单位编码的数据并设置到明细表
        /// </summary>
        /// <param name="khid">考核期</param>
        /// <param name="jgbm">单位编码</param>
        /// <returns></returns>
        public bool SetCwjcsjDataByKhid_Jgbm(string khid, string jgbm)
        {
            /*
             1、提取指定单位和考核期下的量化指标基础数据
             2、提取指定单位和年份下的财务基础数据
             3、提取财务数据与关联指标项的对应关系
             4、依次将数据设置到量化指标基础数据表中
             */
            bool isOk = true;
            KhKhglService khglSrv = new KhKhglService();
            var kaohe = khglSrv.GetSingle(khid);
            if (kaohe == null) return false;
            
            KhLhzbjcsjService lhzbjcsjSrv = new KhLhzbjcsjService();
            //1==量化指标基础数据
            List<KhLhzbjcsjModel> jcsjList = lhzbjcsjSrv.GetListByKhidAndJgbm(khid, jgbm) as List<KhLhzbjcsjModel>;
            
            KhCwjcsjService cwjcsjSrv = new KhCwjcsjService();
            //2==考核年份的财务数据
            var cwjcsjForThisYear = cwjcsjSrv.GetListByJgbmAndNF(jgbm, kaohe.KHND.ToInt());
            //2==考核上一年的财务数据
            var cwjcsjForPrevYear = cwjcsjSrv.GetListByJgbmAndNF(jgbm, kaohe.KHND.ToInt() - 1);
            
            //3==财务基础指标关联数据
            ZbkCwjcsjglzbService cwjcsjglzbSrv = new ZbkCwjcsjglzbService();
            var cwjcsjGlzbLst = cwjcsjglzbSrv.GetList();

            //4==依次将数据设置到量化指标基础数据表中
            foreach (KhLhzbjcsjModel jcsj in jcsjList)
            {
                var cwglzb = cwjcsjGlzbLst.FirstOrDefault(p => p.ZBXMC == jcsj.ZBMC && !string.IsNullOrEmpty(p.JCSJZB));
                if (cwglzb != null)
                {
                    KhCwjcsjModel cwjcsj = null;
                    if (cwglzb.JCSJLX == "1")
                    {
                        //考核年份的财务数据
                        cwjcsj = cwjcsjForThisYear.FirstOrDefault(p => p.ZBMC == cwglzb.JCSJZB);
                        
                    }
                    else if (cwglzb.JCSJLX == "0")
                    {
                        //考核上一年的财务数据
                        cwjcsj = cwjcsjForPrevYear.FirstOrDefault(p => p.ZBMC == cwglzb.JCSJZB);
                    }
                    if (cwjcsj != null)
                    {
                        jcsj.DB_Option_Action = WebKeys.UpdateAction;
                        jcsj.ZBZ = getCwsjData(kaohe, cwjcsj);
                        //if (cwjcsj.ZBMC.Contains("累计")) {
                        //    //cwjcsj.LJZ.ToDecimal();
                        //}
                        //else if (cwjcsj.ZBMC.Contains("平均"))
                        //{
                        //    jcsj.ZBZ = getCwsjData(kaohe, cwjcsj);//cwjcsj.PJZ.ToDecimal();
                        //}
                        isOk = lhzbjcsjSrv.Execute(jcsj);
                    }
                }
            }
            return isOk;
        }

        /// <summary>
        /// 根据考核周期自动计算各月的累计或平均数据
        /// </summary>
        /// <param name="kaohe">考核</param>
        /// <param name="cwjcsj">财务数据</param>
        /// <returns></returns>
        private decimal getCwsjData(KhKhglModel kaohe, KhCwjcsjModel cwjcsj)
        {
            decimal cwsj = 0M;
            int mStart = 1;
            int mEnd = 12;
            int mCount = 12;
            switch (kaohe.KHZQ)
            {
                case "年度":
                    mStart = 1;
                    mEnd = 12;
                    mCount = 12;
                    break;
                case "季度":
                    mStart = kaohe.KHJD.ToInt() * 3 - 2;
                    mEnd = kaohe.KHJD.ToInt() * 3;
                    mCount = 3;
                    break;
                case "月度":
                    mStart = mEnd = kaohe.KHYD.ToInt();
                    mCount = 1;
                    break;
            }
            //先求累计
            for (int i = mStart; i <= mEnd; i++)
            {
                switch (i)
                {
                    case 1:
                        cwsj += cwjcsj.M1.ToDecimal();
                        break;
                    case 2:
                        cwsj += cwjcsj.M2.ToDecimal();
                        break;
                    case 3:
                        cwsj += cwjcsj.M3.ToDecimal();
                        break;
                    case 4:
                        cwsj += cwjcsj.M4.ToDecimal();
                        break;
                    case 5:
                        cwsj += cwjcsj.M5.ToDecimal();
                        break;
                    case 6:
                        cwsj += cwjcsj.M6.ToDecimal();
                        break;
                    case 7:
                        cwsj += cwjcsj.M7.ToDecimal();
                        break;
                    case 8:
                        cwsj += cwjcsj.M8.ToDecimal();
                        break;
                    case 9:
                        cwsj += cwjcsj.M9.ToDecimal();
                        break;
                    case 10:
                        cwsj += cwjcsj.M10.ToDecimal();
                        break;
                    case 11:
                        cwsj += cwjcsj.M11.ToDecimal();
                        break;
                    case 12:
                        cwsj += cwjcsj.M12.ToDecimal();
                        break;
                }
            }
           
            if (cwjcsj.ZBMC.Contains("平均"))
            {
                //平均
                return cwsj / mCount;
            }
            else
            {
                //累计
                return cwsj;
            }
        }

        #endregion

        #region 计算实际得分

        /// <summary>
        /// 计算各项实际得分并保存
        /// </summary>
        /// <param name="dlzbmxLst">定量指标明细</param>
        /// <param name="isCalculateAll">全部计算标志</param>
        public void CalDlzbmxSjdfAndSave(List<KhDlzbmxModel> dlzbmxLst, bool isCalculateAll)
        {
            finishList = new List<KhDlzbmxModel>();
            /*
             1、先分析辅助指标，调用计算规则，计算乘其指标权重后的实际得分
             2、再分析主指标，如果是由辅助指标构成，则将辅助指标的实际得分按关系式合成作为其实际得分
                如果主指标没有辅助指标，则直接调用其计算规则计算其百分制得分，再计算乘其权重后的实际得分
             */
            if (dlzbmxLst.Count == 0) return;
            //对应版本的计算规则集合
            List<ZbkJsgzModel> jsgzList = jsgzSrv.
                GetListByBBMC(dlzbmxLst.First().LhzbModel.BBMC) as List<ZbkJsgzModel>;

            ////1==计算辅助指标的得分
            //var fzzbLst = dlzbmxLst.Where(p => p.ZbsxModel.JxzrsZb.ZZBXZ.Contains("辅助")).OrderByDescending(p => p.ZbsxModel.JxzrsZb.ZXSXH).ToList();
            //foreach (var fzzb in fzzbLst)
            //{
            //    if (isCalculateAll)
            //    {
            //        //计算辅助指标得分
            //        if (!string.IsNullOrEmpty(fzzb.ZbsxModel.JxzrsZb.ZJSGXS))
            //        {
            //            //计算关系式不为空，其得分取决于所有下级指标的得分
            //            var subZbList = dlzbmxLst.Where(p => p.ZbsxModel.JxzrsZb.ZSJZB == fzzb.LhzbModel.LHZBBM).ToList();
            //            KhDlzbmxModel m = calSubZhibiaoDF(fzzb, subZbList, dlzbmxLst, jsgzList, isCalculateAll);
            //            //存入集合
            //            m.DB_Option_Action = WebKeys.UpdateAction;
            //            if (!finishList.Exists(p => p.ID == m.ID))
            //                finishList.Add(m);
            //        }
            //        else
            //        {
            //            //没有计算关系式，可以直接计算得分
            //            KhDlzbmxModel m = CalculateJsgzByDlzb(fzzb, jsgzList);
            //            //存入集合
            //            m.DB_Option_Action = WebKeys.UpdateAction;
            //            if (!finishList.Exists(p => p.ID == m.ID))
            //                finishList.Add(m);
            //        }
            //    }
            //    else
            //    {
            //        //辅助指标得分不重新计算，采用输入值
            //        if (!finishList.Exists(p => p.ID == fzzb.ID))
            //            finishList.Add(fzzb);
            //    }
            //}

            //2==计算主指标的得分,mod by qw 2014.12.20 通过主指标迭代计算就可计算出所有得分
            var mainZbLst = dlzbmxLst.Where(p => p.ZbsxModel.JxzrsZb.ZZBXZ == "主指标").ToList();
            foreach (var zb in mainZbLst)
            {
                if (!string.IsNullOrEmpty(zb.ZbsxModel.JxzrsZb.ZJSGXS))
                {
                    //计算关系式不为空，其得分取决于所有下级指标的得分
                    var subZbList = dlzbmxLst.Where(p => p.ZbsxModel.JxzrsZb.ZSJZB == zb.LhzbModel.LHZBBM).ToList();
                    KhDlzbmxModel m = calSubZhibiaoDF(zb, subZbList, dlzbmxLst, jsgzList, zb.IsCalculate);//isCalculateAll
                    //存入集合
                    m.DB_Option_Action = WebKeys.UpdateAction;
                    if (!finishList.Exists(p => p.ID == m.ID))
                        finishList.Add(m);
                }
                else
                {
                    //没有计算关系式，可以直接计算得分
                    KhDlzbmxModel m = CalculateJsgzByDlzb(zb, jsgzList);
                    //存入集合
                    m.DB_Option_Action = WebKeys.UpdateAction;
                    if (!finishList.Exists(p => p.ID == m.ID))
                        finishList.Add(m);
                }
            }

            //执行批量更新
            ExecuteByList(finishList);
        }

        /// <summary>
        /// 根据计算规则计算实际得分
        /// </summary>
        /// <param name="fzzb">定量指标</param>
        /// <param name="jsgzList">规则集合</param>
        /// <returns></returns>
        public KhDlzbmxModel CalculateJsgzByDlzb(KhDlzbmxModel fzzb, List<ZbkJsgzModel> jsgzList)
        {
            if (!string.IsNullOrEmpty(fzzb.LhzbModel.GZID))
            {
                var jsgzQ = jsgzList.FirstOrDefault(p => p.GZID == fzzb.LhzbModel.GZID);
                if (jsgzQ != null)
                {
                    if (!string.IsNullOrEmpty(jsgzQ.GZBDS))
                    {
                        //表达式
                        string bds = jsgzQ.GZBDS;
                        try
                        {
                            bds = bds.Replace("P", "(" + fzzb.MBZ.ToRequestString() + ")");
                            bds = bds.Replace("F", "(" + fzzb.WCZ.ToRequestString() + ")");
                            decimal? calV = Utility.Eval(bds).ToNullOrDecimal();
                            if (calV != null)
                            {
                                decimal standV = fzzb.ZbsxModel.SXQZ.ToDecimal() * 100;//要用筛选的权重计算
                                calV = calV.Value * standV;//计算实际值
                                decimal maxV = 0;
                                decimal minV = 0;
                                if (jsgzQ.MAXV != null && jsgzQ.MINV != null)
                                {
                                    //限定值
                                    maxV = jsgzQ.MAXV.Value;
                                    minV = jsgzQ.MINV.Value;
                                    if (calV.Value > maxV) calV = maxV;
                                    if (calV.Value < minV) calV = minV;
                                }
                                else if (jsgzQ.UPPER != null && jsgzQ.LOWER != null)
                                {
                                    //范围值
                                    maxV = standV * (1 + jsgzQ.UPPER.Value);
                                    minV = standV * (1 - jsgzQ.LOWER.Value);
                                    if (calV.Value > maxV) calV = maxV;
                                    if (calV.Value < minV) calV = minV;
                                }
                            }
                            //保存得分
                            fzzb.SJDF = calV;
                        }
                        catch (Exception ex)
                        {
                            Debuger.GetInstance().log(this, "CalculateGlzbsWcz调用bds=" + bds + "出现了错误!", ex);
                        }

                    }
                    else if (!string.IsNullOrEmpty(jsgzQ.METHODNAME))
                    {
                        //调用方法名称
                        string methodName = jsgzQ.METHODNAME;
                        try
                        {
                            JsgzLogicResult result = ZbkJsgzHandler.CallJsgzMethod(fzzb, methodName) as JsgzLogicResult;
                            fzzb.DFJSQK = result.Msg;
                            decimal? calV = result.ReturnValue;
                            if (result.HasValue)
                            {
                                decimal standV = fzzb.ZbsxModel.SXQZ.ToDecimal() * 100;//要用筛选的权重计算
                                calV = calV.Value * standV;//计算实际值
                                decimal maxV = 0;
                                decimal minV = 0;
                                if (jsgzQ.MAXV != null && jsgzQ.MINV != null)
                                {
                                    //限定值
                                    maxV = jsgzQ.MAXV.Value;
                                    minV = jsgzQ.MINV.Value;
                                    if (calV.Value > maxV) calV = maxV;
                                    if (calV.Value < minV) calV = minV;
                                }
                                else if (jsgzQ.UPPER != null && jsgzQ.LOWER != null)
                                {
                                    //范围值
                                    maxV = standV * (1 + jsgzQ.UPPER.Value);
                                    minV = standV * (1 - jsgzQ.LOWER.Value);
                                    if (calV.Value > maxV) calV = maxV;
                                    if (calV.Value < minV) calV = minV;
                                }
                            }
                            //保存得分
                            fzzb.SJDF = calV;
                        }
                        catch (Exception ex)
                        {
                            Debuger.GetInstance().log(this, "CalculateGlzbsWcz调用methodName=" + methodName + "出现了错误!", ex);
                        }
                    }
                }
            }

            return fzzb;
        }

        #endregion

        #endregion

        #region 专用方法区

        /// <summary>
        /// 迭代计算各指标的所有下级指标得分
        /// </summary>
        /// <param name="model">本指标</param>
        /// <param name="subZhibiaoLst">下级指标集合</param>
        /// <param name="allZhibiaoLst">所有指标集合</param>
        /// <param name="jsgzList">计算规则集合</param>
        /// <param name="isCalcFzzb">重新计算辅助指标标志</param>
        /// <returns></returns>
        private KhDlzbmxModel calSubZhibiaoDF(KhDlzbmxModel model, List<KhDlzbmxModel> subZhibiaoLst,
            List<KhDlzbmxModel> allZhibiaoLst, List<ZbkJsgzModel> jsgzList, bool isCalcFzzb)
        {
            string bds = model.ZbsxModel.JxzrsZb.ZJSGXS;
            bds = bds.Substring(bds.IndexOf('=') + 1);//只取等号右边部分
            foreach (var zb in subZhibiaoLst)
            {
                if (!string.IsNullOrEmpty(zb.ZbsxModel.JxzrsZb.ZJSGXS))
                {
                    //计算关系式不为空，其得分取决于所有下级指标的得分
                    var subZbList = allZhibiaoLst.Where(p => p.ZbsxModel.JxzrsZb.ZSJZB == zb.LhzbModel.LHZBBM).ToList();
                    KhDlzbmxModel m = calSubZhibiaoDF(zb, subZbList, allZhibiaoLst, jsgzList, zb.IsCalculate);
                    //替换表达式里的代号
                    bds = bds.Replace(m.ZbsxModel.JxzrsZb.ZZBDH, "(" + m.SJDF.ToRequestString() + ")");
                    //存入集合
                    m.DB_Option_Action = WebKeys.UpdateAction;
                    if (!finishList.Exists(p=>p.ID == m.ID))
                        finishList.Add(m);
                }
                else
                {
                    //没有计算关系式，可以直接计算得分
                    KhDlzbmxModel m = zb;
                    if (zb.IsCalculate)
                    {
                        m = CalculateJsgzByDlzb(zb, jsgzList);   
                    }
                    //替换表达式里的代号
                    bds = bds.Replace(m.ZbsxModel.JxzrsZb.ZZBDH, "(" + m.SJDF.ToRequestString() + ")");
                    //存入集合
                    m.DB_Option_Action = WebKeys.UpdateAction;
                    if (!finishList.Exists(p => p.ID == m.ID))
                        finishList.Add(m);
                }
            }
            decimal? calV = Utility.Eval(bds).ToNullOrDecimal();
            if (calV != null)
            {
                //mod by qw 2014.12.20 start 分公司的指标是将主指标的权重依次分解下去，所以可以不用再乘一次权重
                decimal subQzhj = subZhibiaoLst.Sum(p => p.ZbsxModel.SXQZ).ToDecimal();
                if (model.ZbsxModel.SXQZ != null && subQzhj > 0)
                {
                    if (model.ZbsxModel.SXQZ.Value == subQzhj)
                    {
                        calV = calV.Value;//不用再乘权重
                    }
                    else
                    {
                        calV = calV.Value * model.ZbsxModel.SXQZ.ToDecimal();//合计后的指标值也要乘以其权重
                    }
                }
                //end
                model.SJDF = calV;
                if (model.LhzbModel.Jsgz != null)
                {
                    model = chkKhDlzbmxDfByJsgz(model, model.LhzbModel.Jsgz);//调用其规则，对得分进行区间控制
                }
            }        
            return model;
        }

        /// <summary>
        /// 专用于检测当前指标的得分是否在设定范围内
        /// </summary>
        /// <param name="fzzb">定量指标MODEL</param>
        /// <param name="jsgzQ">计算规则MODEL</param>
        /// <returns></returns>
        private KhDlzbmxModel chkKhDlzbmxDfByJsgz(KhDlzbmxModel fzzb, ZbkJsgzModel jsgzQ)
        {
            try
            {
                decimal calV = fzzb.SJDF.Value;//目前的得分（乘权重后值）
                decimal standV = fzzb.ZbsxModel.SXQZ.ToDecimal() * 100;//要用筛选的权重作为标准值
                decimal maxV = 0;
                decimal minV = 0;
                if (jsgzQ.MAXV != null && jsgzQ.MINV != null)
                {
                    //限定值
                    maxV = jsgzQ.MAXV.Value;
                    minV = jsgzQ.MINV.Value;
                    if (calV > maxV) calV = maxV;
                    if (calV < minV) calV = minV;
                }
                else if (jsgzQ.UPPER != null && jsgzQ.LOWER != null)
                {
                    //范围值
                    maxV = standV * (1 + jsgzQ.UPPER.Value);
                    minV = standV * (1 - jsgzQ.LOWER.Value);
                    if (calV > maxV) calV = maxV;
                    if (calV < minV) calV = minV;
                }
                //保存得分
                fzzb.SJDF = calV;
            }
            catch (Exception ex)
            {
                Debuger.GetInstance().log(this, "chkKhDlzbmxDfByJsgz方法出现了错误!", ex);
            }
            return fzzb;
        }

        #endregion
    }

}
