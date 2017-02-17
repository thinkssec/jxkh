using Enterprise.Service.Perfo.Sys;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Web;

namespace Enterprise.UI.Web.Module.KhX.data
{
    /// <summary>
    /// KhpfHandler 的摘要说明
    /// </summary>
    public class KhpfHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            if (!string.IsNullOrEmpty(context.Request["type"]))
            {
                switch (context.Request["type"].ToString())
                {
                    case "getbkhdw":
                        GetBkhdw(context);
                        break;
                    case "getKhmb":
                        GetKhmb(context);
                        break;
                    case "saveMb":
                        SaveMb(context);
                        break;
                    case "getKhjg":
                        GetKhjg(context);
                        break;
                    case "getkhpjdw":
                        GetKhpjdw(context);
                        break;
                    case "saveKhfq":
                        SaveKhfq(context);
                        break;
                    case "delKhFile":
                        DeleteKhFile(context);
                        break;
                    case "getKhDetail":
                        GetKhDetail(context);
                        break;
                    case "saveKhfs":
                        SaveKhFs(context);
                        break;
                    case "getKhsj":
                        GetKhsj(context);
                        break;
                    case "getJcdwHz":
                        GetJcdwHz(context);
                        break;
                    case "getKhzb":
                        GetKhZb(context);
                        break;
                    case "deleteZb":
                        DeleteZb(context);
                        break;
                    case "getKhxm":
                        GetKhXm(context);
                        break;
                    case "saveKhzb":
                        SaveKhzb(context);
                        break;
                    case "saveZbType":
                        SaveZbType(context);
                        break;
                    case "getKhdwmb":
                        GetKhdwmb(context);
                        break;
                    case "saveDwmb":
                        SaveDwmb(context);
                        break;
                    case "getKhDwmbByID":
                        GetKhDwmbByID(context);
                        break;
                    case "deleteDwmb":
                        DeleteDwmb(context);
                        break;
                    #region New
                    case "getKhList":
                        GetKhList(context);
                        break;
                    case "getKhmbList"://考核模板下指标列表页面，模版继承的下拉框
                        GetKhmbList(context);
                        break;
                    case "saveKhzbWithMb"://考核模版继承
                        SaveKhzbWidthMb(context);
                        break;
                    case "getZbDetail":
                        GetZbDetail(context);
                        break;
                    case "getKhmbByDzb":
                        GetKhmbByDzb(context);
                        break;
                    #endregion
                    default: break;
                }
            }
        }

        #region 单位与模板配置表
        /// <summary>
        /// 获取当前用户所在考核单位的被考核单位列表
        /// </summary>
        /// <param name="p_Context"></param>
        private void GetBkhdw(HttpContext p_Context)
        {
            string sql = "select * from PERFO_SYS_BMJG where sfkh='1' and jglx in ('基层单位','职能部门')";//根据当前登录用户查询需要评分的单位列表
            //if (!string.IsNullOrEmpty(p_Context.Request["bm"]))
            //    sql += " and jgbm in(select khdw from perfo_khx_dwmb where pjdw='" + p_Context.Request["bm"] + "')";
            sql += " order by jglx,jgbm";
            DataTable dt = DbHelperSQL.QueryToTable(sql);
            p_Context.Response.Write(DbHelperSQL.TableToJson(dt));
        }

        /// <summary>
        /// 获取考核评价单位的列表
        /// </summary>
        /// <param name="p_Context"></param>
        private void GetKhpjdw(HttpContext p_Context)
        {
            string sql = "select * from PERFO_SYS_BMJG where sfkh='1' and jglx='职能部门' ";
            if (!string.IsNullOrEmpty(p_Context.Request["bm"]))
            {
                sql += " and jgbm='" + p_Context.Request["bm"] + "'";
            }
            sql += " order by xsxh";
            DataTable dt = DbHelperSQL.QueryToTable(sql);
            p_Context.Response.Write(DbHelperSQL.TableToJson(dt));
        }
        #endregion

        #region 考核模板
        /// <summary>
        /// 获取考核模板
        /// </summary>
        /// <param name="p_Context"></param>
        private void GetKhmb(HttpContext p_Context)
        {
            try
            {
                PageBase pb = new PageBase();
                string sql = null;
                sql = @"select A.ID,A.NAME ,A.MEMO
from PERFO_KHX_MB A,PERFO_KHS_DZB B, PERFO_KHS_KHDZB C
 WHERE A.KHDZBID = C.ID AND C.DZBID = B.DZBID";
                if (!String.IsNullOrEmpty(p_Context.Request["bm"]))
                {
                    sql += " AND B.FZBM = '" + p_Context.Request["bm"] + "'";
                }
                if (!string.IsNullOrEmpty(p_Context.Request["khid"]))
                {
                    sql += " and c.khid='" + p_Context.Request["khid"] + "'";
                }
                DataTable dt = DbHelperSQL.QueryToTable(sql);
                p_Context.Response.Write(DbHelperSQL.TableToJson(dt));
            }
            catch (Exception ex)
            {
                p_Context.Response.Write("失败！原因：" + ex.Message);
            }
        }

        private void SaveMb(HttpContext p_Context)
        {
            try
            {
                if (!string.IsNullOrEmpty(p_Context.Request["mb"]) && !string.IsNullOrEmpty(p_Context.Request["id"]))
                {
                    string khdzbid = p_Context.Request["id"];
                    List<string> sqls = new List<string>();
                    string sql = null;
                    JArray json = JArray.Parse(p_Context.Request["mb"]);
                    int index = 0;
                    string deleteMBIDs = "";
                    foreach (JObject jobject in json)
                    {
                        JArray data = JArray.Parse(jobject["data"].ToString());
                        if (data.Count > 0)
                        {
                            foreach (JObject jobjC in data)
                            {
                                switch (index)
                                {
                                    case 0://新增
                                        sql = "insert into PERFO_KHX_MB(NAME,MEMO,QZ,khdzbid) values('"
                                            + jobjC["NAME"] + "','" + jobjC["MEMO"] + "','" + jobjC["QZ"] + "','" + khdzbid + "')";
                                        break;
                                    case 1://编辑
                                        sql = "update PERFO_KHX_MB set name='" + jobjC["NAME"] + "', memo='" + jobjC["MEMO"]
                                            + "',QZ='" + jobjC["QZ"] + "' where id='" + jobjC["ID"] + "' ";
                                        break;
                                    case 2://删除
                                        sql = "delete from PERFO_KHX_MB where ID='" + jobjC["ID"] + "'";
                                        deleteMBIDs += "'" + jobjC["ID"] + "',";
                                        break;
                                    default: sql = null; break;
                                }
                                if (sql != null)
                                    sqls.Add(sql);
                            }
                        }
                        index++;
                    }
                    //                    sql = @"insert into PERFO_KHS_MBJG(khdzbid,mbid) 
                    //select khdzbid, id from PERFO_KHX_MB a 
                    //where (id, khdzbid) not in (select mbid, khdzbid from PERFO_KHS_MBJG) and khdzbid = '" + khdzbid + "'";
                    //                    sqls.Add(sql);
                    if (deleteMBIDs.Length > 1)
                    {
                        sql = "delete from PERFO_KHS_MBJG where khdzbid='" + khdzbid + "' and mbid in (" + deleteMBIDs.Substring(0, deleteMBIDs.Length - 1) + ")";
                        sqls.Add(sql);
                    }
                    DbHelperSQL.ExecuteOracleTran(sqls);
                }
            }
            catch (Exception ex)
            {
                p_Context.Response.Write("保存失败！" + ex.Message);
            }
        }

        private void GetKhmbList(HttpContext p_Context)
        {
            try
            {
                if (!string.IsNullOrEmpty(p_Context.Request["bm"]) && !string.IsNullOrEmpty(p_Context.Request["mbid"]))
                {
                    string sql = string.Format(@"select a.id,a.name from PERFO_KHX_MB a
                                                        left join PERFO_KHS_KHDZB b on a.khdzbid=b.id
                                                        left join perfo_khs_dzb c on b.dzbid=c.dzbid 
                                                        where a.id!='{1}' and c.fzbm='{0}' union all  select a.id, a.name
                                                    from PERFO_KHX_MB a where qz={0} order by name", p_Context.Request["bm"], p_Context.Request["mbid"]);
                    DataTable dt = DbHelperSQL.QueryToTable(sql);
                    if (dt != null)
                        p_Context.Response.Write(DbHelperSQL.TableToJson(dt));
                }
            }
            catch (Exception ex)
            {
                //p_Context.Response.Write("失败！原因：" + ex.Message);
            }
        }

        private void SaveKhzbWidthMb(HttpContext p_Context)
        {
            try
            {
                if (!string.IsNullOrEmpty(p_Context.Request["Fmbid"]) && !string.IsNullOrEmpty(p_Context.Request["Tmbid"]))
                {
                    string fmbid = p_Context.Request["Fmbid"];
                    string tmbid = p_Context.Request["Tmbid"];
                    List<string> sqls = new List<string>();
                    string sql = "delete from PERFO_KHX_ZB where mbid='" + tmbid + "'";
                    sqls.Add(sql);
                    sql = string.Format(@"insert into PERFO_KHX_ZB(xm1,xm2,nr,bz,zq,fs,bzf,ord,mbid,type)
select xm1,xm2,nr,bz,zq,fs,bzf,ord,'{0}',type from PERFO_KHX_ZB where mbid='{1}'", tmbid, fmbid);
                    sqls.Add(sql);
                    DbHelperSQL.ExecuteOracleTran(sqls);
                }
            }
            catch (Exception ex)
            {
                p_Context.Response.Write("失败！原因：" + ex.Message);
            }
        }

        private void GetKhmbByDzb(HttpContext p_Context)
        {
            if (!string.IsNullOrEmpty(p_Context.Request["khdzbid"]))
            {
                string sql = @"select * from PERFO_KHx_mb
where khdzbid = '" + p_Context.Request["khdzbid"] + "'";
                DataTable dt = DbHelperSQL.QueryToTable(sql);
                if (dt != null)
                    p_Context.Response.Write(DbHelperSQL.TableToJson(dt));
            }
        }

        #endregion

        #region 考核文件

        private void GetKhjg(HttpContext p_Context)
        {
            string sql = @"select a.ID,a.name,decode(a.status,'0','评分中','已提交') status,c.jgmc pjdw,d.jgmc khdw
from PERFO_KHX_KH a , perfo_khx_dwmb b, perfo_sys_bmjg c,perfo_sys_bmjg d
where a.dwmbid = b.id and b.pjdw = c.jgbm and b.khdw = d.jgbm";
            if (!string.IsNullOrEmpty(p_Context.Request["bm"]))
            {
                string dw = p_Context.Request["bm"];
                sql += " and (b.pjdw='" + dw + "' or b.khdw='" + dw + "')";
            }
            DataTable dt = DbHelperSQL.QueryToTable(sql);
            p_Context.Response.Write(DbHelperSQL.TableToJson(dt));
        }

        private void SaveKhfq(HttpContext p_Context)
        {
            try
            {
                if (!string.IsNullOrEmpty(p_Context.Request["name"]) && !string.IsNullOrEmpty(p_Context.Request["year"])
                    && !string.IsNullOrEmpty(p_Context.Request["jd"]) && !string.IsNullOrEmpty(p_Context.Request["pjdw"])
                    && !string.IsNullOrEmpty(p_Context.Request["khdw"]))
                {
                    string pjdw = p_Context.Request["pjdw"];
                    string khdw = p_Context.Request["khdw"];
                    string sql = "select * from PERFO_KHX_DWMB where 1=1";
                    if (pjdw != "null")
                        sql += " and pjdw='" + p_Context.Request["pjdw"] + "'";
                    if (khdw != "null")
                        sql += " and khdw='" + p_Context.Request["khdw"] + "'";
                    DataTable dt = DbHelperSQL.QueryToTable(sql);
                    if (dt.Rows.Count > 0)
                    {
                        List<string> sqls = new List<string>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            string khid = Guid.NewGuid().ToString();
                            sql = "insert into PERFO_KHX_KH(id,name,dwmbid,ny) values('" + khid + "','"
                                + p_Context.Request["name"] + "','" + dr["id"] + "','" + p_Context.Request["year"] + p_Context.Request["jd"] + "')";
                            sqls.Add(sql);
                            sql = string.Format(@"insert into PERFO_KHX_RESULT(forid,type,MBJGID,khid)
select distinct decode(a.type,'xm1',xm1,'xm2',xm2,nr) forid,a.type,'{0}','{1}' from PERFO_KHX_ZB a,PERFO_KHX_DWMB b
where a.mbid=b.mbid and b.pjdw='{2}' and b.khdw='{3}'", dr["id"], khid, dr["pjdw"], dr["khdw"]);
                            sqls.Add(sql);
                        }
                        DbHelperSQL.ExecuteOracleTran(sqls);
                    }
                }
            }
            catch (Exception ex)
            {
                p_Context.Response.Write("失败！原因：" + ex.Message);
            }
        }

        private void DeleteKhFile(HttpContext p_Context)
        {
            try
            {
                if (!string.IsNullOrEmpty(p_Context.Request["id"]))
                {
                    string khFileID = p_Context.Request["id"];
                    List<string> sqls = new List<string>();
                    string sql = "delete from PERFO_KHX_KH where id='" + khFileID + "'";
                    sqls.Add(sql);
                    sql = "delete from PERFO_KHX_RESULT where khid='" + khFileID + "'";
                    sqls.Add(sql);
                    DbHelperSQL.ExecuteOracleTran(sqls);
                }
            }
            catch (Exception ex)
            {
                p_Context.Response.Write("失败！原因：" + ex.Message);
            }
        }

        #endregion

        #region 考核指标
        private void GetZbDetail(HttpContext p_Context)
        {
            if (!string.IsNullOrEmpty(p_Context.Request["zbid"]))
            {
                string sql = string.Format("select * from PERFO_KHX_ZB where id='{0}'", p_Context.Request["zbid"]);
                DataTable dt = DbHelperSQL.QueryToTable(sql);
                if (dt != null)
                    p_Context.Response.Write(DbHelperSQL.TableToJson(dt));
            }
        }

        #endregion

        #region 考核评分
        /// <summary>
        /// 获取考核结果
        /// </summary>
        /// <param name="p_Context"></param>
        private void GetKhDetail(HttpContext p_Context)
        {
            string audit = p_Context.Request["audit"];
            if (!string.IsNullOrEmpty(p_Context.Request["mbjgid"]))
            {
                string sql = string.Format(@"select b.id,
                                               b.xm1,
                                               b.xm2,
                                               b.nr,
                                               b.bz,
                                               b.zq,
                                               b.fs,
                                               b.bzf,
                                               a.score,
                                               a.ms,
                                               b.type,
                                               a.forid,
                                               t3.status,
                                               t3.jgbm
                                          from perfo_khs_khdzb t2, perfo_khs_mbjg t3, perfo_khx_zb b
                                          left join PERFO_KHX_RESULT a
                                            on a.forid = decode(b.type, 'xm1', b.xm1, 'xm2', b.xm2, 'bz',b.bz,b.nr)
                                           and a.mbjgid = '{0}'
                                         where t2.id = t3.khdzbid
                                           and t3.mbid = b.mbid     
                                           and t3.id = '{0}'
                                         order by b.ord, b.xm1, b.xm2", p_Context.Request["mbjgid"]);
                DataTable dt = DbHelperSQL.QueryToTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    //bool haveSubmit = (dt.Rows[0]["status"].ToString() == "1" ? true : false);//是否已提交
                    //未提交、管理员及评价单位用户可以进行编辑，否则查询
                    //  bool edit = !haveSubmit && (string.IsNullOrEmpty(p_Context.Request["bm"]) || p_Context.Request["bm"] == dt.Rows[0]["jgbm"].ToString()) ? true : false;
                    bool haveSubmit = false;
                    if (dt.Rows[0]["status"].ToString().Contains("0") || dt.Rows[0]["status"].ToString().Contains("1"))
                    {
                        haveSubmit = true;
                    }
                    if (!string.IsNullOrEmpty(audit)) haveSubmit = true;

                    bool edit = !haveSubmit;

                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table>");
                    List<int> startIndex = new List<int> { 0, 0, 0, 0 };//项目1，项目2，,内容，标准
                    List<string> prevValue = new List<string> { "", "", "", "" };
                    List<int> rowSpan = new List<int> { 0, 0, 0, 0 };
                    List<bool> rowSpaned = new List<bool> { false, false, false, false };
                    bool xmHasChild = false;
                    DataRow[] rows = dt.Select("xm2 is not null");
                    if (rows != null && rows.Length > 0)
                        xmHasChild = true;
                    sb.Append("<tr><th ");
                    if (xmHasChild)
                        sb.Append(" colspan='2'");
                    sb.Append(">考核项目</th><th style='width:140px;'>考核内容</th><th>考核标准</th><th style='width:50px;'>考核周期</th><th style='width:70px'>考核方式</th><th style='width:50px'>标准分值</th><th style='width:50px'>考核得分</th><th style='width:180px'>加减分描述</th></tr>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        rowSpan = new List<int> { 0, 0, 0, 0 };
                        DataRow dr = dt.Rows[i];
                        sb.Append("<tr>");
                        #region 指标项目、内容、标准
                        if (i >= startIndex[0])
                        {
                            prevValue[0] = dr["xm1"].ToString();
                            rowSpan[0] = GetRowSpan(dt, "xm1", i);
                            sb.Append("<td style='width:100px;' class='center' ");
                            if (rowSpan[0] > 1)
                                sb.Append("rowspan='" + rowSpan[0] + "'");
                            sb.Append(">" + dr["xm1"] + "</td>");
                            startIndex[0] = i + rowSpan[0];
                        }
                        if (xmHasChild && i >= startIndex[1])
                        {
                            prevValue[1] = dr["xm2"].ToString();
                            rowSpan[1] = GetRowSpan(dt, "xm2", i);
                            sb.Append("<td style='width:80px;' class='center' ");
                            if (rowSpan[1] > 1)
                                sb.Append("rowspan='" + rowSpan[1] + "'");
                            sb.Append(">" + dr["xm2"] + "</td>");
                            startIndex[1] = i + rowSpan[1];
                        }
                        if (i >= startIndex[2])
                        {
                            prevValue[2] = dr["nr"].ToString();
                            rowSpan[2] = GetRowSpan(dt, "nr", i);
                            sb.Append("<td ");
                            if (rowSpan[2] > 1)
                                sb.Append("rowspan='" + rowSpan[2] + "'");
                            sb.Append(">" + dr["nr"] + "</td>");
                            startIndex[2] = i + rowSpan[2];
                        }
                        if (i >= startIndex[3])
                        {
                            prevValue[3] = dr["bz"].ToString();
                            rowSpan[3] = GetRowSpan(dt, "bz", i);
                            sb.Append("<td ");
                            if (rowSpan[3] > 1)
                                sb.Append("rowspan='" + rowSpan[3] + "'");
                            sb.Append(">" + dr["bz"] + "</td>");
                            startIndex[3] = i + rowSpan[3];
                        }
                        #endregion
                        sb.Append("<td class='center'>" + dr["zq"] + "</td><td class='center'>" + dr["fs"] + "</span></td>");
                        if ((dr["type"].ToString().ToLower() == "xm1" && i >= startIndex[0] - rowSpan[0]) ||
                            (dr["type"].ToString().ToLower() == "xm2" && i >= startIndex[1] - rowSpan[1]) ||
                            (dr["type"].ToString().ToLower() == "nr" && i >= startIndex[2] - rowSpan[2])||
                            (dr["type"].ToString().ToLower() == "bz" && i >= startIndex[3] - rowSpan[3]))

                        {
                            int span = (dr["type"].ToString() == "xm1" ? rowSpan[0] : (dr["type"].ToString() == "xm2" ? rowSpan[1] : (dr["type"].ToString() == "nr" ? rowSpan[2] : rowSpan[3])));
                            string scoreZb = dr["forid"].ToString();
                            if (scoreZb == "")
                            {
                                scoreZb = (dr["type"].ToString() == "xm1" ? dr["xm1"].ToString() : (dr["type"].ToString() == "xm2" ? dr["xm2"].ToString() : (dr["type"].ToString() == "nr" ? dr["nr"].ToString() : dr["bz"].ToString())));
                            }
                            sb.Append("<td class='right' ");
                            if (span > 1)
                                sb.Append("rowspan='" + span + "'");
                            sb.Append(">" + dr["bzf"] + "</td>");
                            sb.Append("<td ");
                            if (span > 1)
                                sb.Append("rowspan='" + span + "'");
                            sb.Append(" style='vertical-align:middle;'><input name='txtScore' id='" + scoreZb
                                + "' dddd='dddd"+i+"' type='text' style='width:40px;' value='"
                                + (dr["score"] == null || dr["score"].ToString() == "" ? dr["bzf"] : dr["score"]) + "'");
                            if (!edit)
                            {
                                sb.Append(" disabled='disabled'");
                            }
                            sb.Append(" /><span style='display:none;' id='span_dddd" + i + "'></td>");//考核得分
                            sb.Append("<td ");
                            if (span > 1)
                                sb.Append("rowspan='" + span + "'");
                            sb.Append("><textarea name='scoreMemo' id='ta_dddd" + i + "' style='width:90%;height:90%;' value='" + dr["ms"] + "'");
                            if (!edit)
                            {
                                sb.Append(" disabled='disabled'");
                            }
                            sb.Append(" >" + ((dr["ms"].ToString())== "undefined" ? "" : dr["ms"]) + "</textarea></td>");//加减分描述
                        }
                        sb.Append("</tr>");
                    }
                    sb.Append("<tr><td style='text-align:center;' colspan='");
                    if (xmHasChild)
                        sb.Append("9");
                    else sb.Append("8");
                    sb.Append("'> ");
                    if (edit)
                    {
                        sb.Append("<input type='button' id='btnSure' class='easyui-linkbutton' data-options=\"iconCls:'icon-save'\" value='保存' onclick='Save()' />");
                        //sb.Append("<input type='button' id=\"btnSubmit\" style='margin-left:10px' class=\"easyui-linkbutton\" data-options=\"iconCls:'icon-save'\" value='提交' onclick='Submit()' />");
                    }
                    sb.Append("<input type='button' id=\"btnCancel\" style='margin-left:10px' class=\"easyui-linkbutton\" data-options=\"iconCls:'icon-cancel'\" value='返回' onclick='Cancel()' />");
                    if (edit)
                    {
                        sb.Append("<span style='color:red;margin-left:8px;'>扣分项请输入负值</span>");
                    }
                    sb.Append("</td></tr>");
                    sb.Append("</table>");
                    p_Context.Response.Write(sb.ToString());
                }
            }
        }

        private int GetRowSpan(DataTable dt, string columnName, int startIndex)
        {
            int endIndex = dt.Rows.Count;
            for (int i = startIndex + 1; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][columnName].ToString() != dt.Rows[startIndex][columnName].ToString())
                {
                    endIndex = i;
                    break;
                }
            }
            return endIndex - startIndex;
        }

        /// <summary>
        /// 保存考核分值
        /// </summary>
        /// <param name="p_Context"></param>
        private void SaveKhFs(HttpContext p_Context)
        {
            try
            {
                if (!string.IsNullOrEmpty(p_Context.Request["data"]) &&
                    !string.IsNullOrEmpty(p_Context.Request["khid"]) && !string.IsNullOrEmpty(p_Context.Request["mbjgid"]))
                {
                    List<string> sqls = new List<string>();
                    string sql = "DELETE FROM PERFO_KHX_RESULT WHERE MBJGID='" + p_Context.Request["mbjgid"] + "' and khid='" + p_Context.Request["khid"] + "'";
                    sqls.Add(sql);
                    JArray jarray = JArray.Parse(p_Context.Request["data"]);
                    string khid = p_Context.Request["khid"];
                    string mgjgid = p_Context.Request["mbjgid"];
                    foreach (JObject jobj in jarray)
                    {
                        sql = "INSERT INTO PERFO_KHX_RESULT(forid,score,mbjgid,ms,khid,type) values('" + jobj["name"]
                            + "','" + jobj["value"] + "','" + mgjgid + "','" + jobj["memo"] + "','" + khid + "','" + jobj["type"] + "')";
                        sqls.Add(sql);
                    }
                    DbHelperSQL.ExecuteOracleTran(sqls);
                    string str = "0";
                    if (!string.IsNullOrEmpty(p_Context.Request["submit"]))
                    {
                        str = p_Context.Request["submit"];
                    }
                    if (str == "1")//提交评分结果
                    {
                        SubmitKhPf(mgjgid);
                    }
                }
            }
            catch (Exception ex)
            {
                p_Context.Response.Write("失败！原因：" + ex.Message);
            }
        }

        /// <summary>
        /// 提交考核分数
        /// </summary>
        /// <param name="p_Context"></param>
        private void SubmitKhPf(string mbjgid)
        {
            string sql = "update PERFO_KHS_MBJG set status='1' where id='" + mbjgid + "'";
            DbHelperSQL.ExecuteOracle(sql);
        }
        #endregion

        #region 考核结果汇总
        /// <summary>
        /// 机关单位汇总
        /// </summary>
        /// <param name="p_Context"></param>
        private void GetJgdwHz(HttpContext p_Context)
        {
            if (!string.IsNullOrEmpty(p_Context.Request[""]))
            {
                string sql = "";
            }
        }

        /// <summary>
        /// 基层单位汇总
        /// </summary>
        /// <param name="p_Context"></param>
        private void GetJcdwHz(HttpContext p_Context)
        {
            try
            {
                if (!string.IsNullOrEmpty(p_Context.Request["ny"]))//汇总的时间。例：2015年第三季度20153
                {
                    string sql = string.Format(@"select khid,sum(score) fs,c.qz,d.jgmc khdw,substr(substr(c.name,8),0,instr(substr(c.name,8),'考核模板')-1) zb
from PERFO_KHX_RESULT a,perfo_khx_dwmb b,perfo_khx_mb c,perfo_sys_bmjg d,perfo_khx_kh e
where a.MBJGID=b.id and b.mbid=c.id and b.khdw=d.jgbm and a.khid=e.id and d.jglx='基层单位' and e.ny='{0}'
group by khid,c.qz,d.jgmc,c.name", p_Context.Request["ny"]);
                    DataTable dt = DbHelperSQL.QueryToTable(sql);
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table>");
                    sb.Append("<tr><th style='width:50px' rowspan='2'>序号</th><th style='width:140px' rowspan='2'>考核指标</th><th style='width:80px' rowspan='2'>考核项目</th>");
                    sb.Append("<th rowspan='2' style='width:80px'>考核内容</th><th style='width:80px' rowspan='2'>考核方式</th><th style='width:200px' rowspan='2'>考核标准</th>");
                    sb.Append("<th style='width:50px' rowspan='2'>考核周期</th><th rowspan='2' style='width:50px'>标准分值</th><th rowspan='2' style='width:50px'>权重</th>");
                    sb.Append("<th colspan='6'>考核得分</th><th rowspan='2' style='width:50px'>权重</th><th colspan='2'>考核得分</th>");
                    sb.Append("<th rowspan='2' style='width:50px'>权重</th><th style='width:85px;'>考核得分</th><th rowspan='2' style='width:50px'>权重</th><th style='width:85px;'>考核得分</th>");
                    sb.Append("</tr>");
                    sb.Append("<tr><th style='width:50px'>川渝</th><th style='width:50px'>鄂西</th><th style='width:50px'>鄂东</th><th style='width:50px'>安徽</th><th style='width:50px'>浙沪</th><th style='width:50px'>江苏</th>");
                    sb.Append("<th style='width:50px'>宜昌</th><th style='width:50px'>嘉兴</th><th style='width:50px'>金坛</th><th style='width:50px'>压保</th></tr>");
                    if (dt != null)
                    {
                        #region 指标从库查询
                        //DataTable dtNew = new DataTable();
                        //dtNew.Columns.Add("序号", typeof(int));
                        //dtNew.Columns.Add("考核指标", typeof(string));
                        //dtNew.Columns.Add("考核项目", typeof(string));
                        //dtNew.Columns.Add("考核内容", typeof(string));
                        //dtNew.Columns.Add("考核方式", typeof(string));
                        //dtNew.Columns.Add("考核标准", typeof(string));
                        //dtNew.Columns.Add("考核周期", typeof(string));
                        //dtNew.Columns.Add("标准分值", typeof(int));
                        //dtNew.Columns.Add("权重1", typeof(int));
                        //dtNew.Columns.Add("川渝", typeof(int));
                        //dtNew.Columns.Add("鄂西", typeof(int));
                        //dtNew.Columns.Add("鄂东", typeof(int));
                        //dtNew.Columns.Add("安徽", typeof(int));
                        //dtNew.Columns.Add("浙沪", typeof(int));
                        //dtNew.Columns.Add("江苏", typeof(int));
                        //dtNew.Columns.Add("权重2", typeof(int));
                        //dtNew.Columns.Add("宜昌", typeof(int));
                        //dtNew.Columns.Add("嘉兴", typeof(int));
                        //dtNew.Columns.Add("权重3", typeof(int));
                        //dtNew.Columns.Add("金坛", typeof(int));
                        //dtNew.Columns.Add("权重4", typeof(int));
                        //dtNew.Columns.Add("压保", typeof(int));
                        //Hashtable hsZbIndex = new Hashtable(); 
                        #endregion
                        #region 考核分值
                        Hashtable hsZbDw = new Hashtable();
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (!hsZbDw.ContainsKey(dr["khdw"] + "_" + dr["zb"]))
                            {
                                hsZbDw.Add(dr["khdw"] + "_" + dr["zb"], dr["fs"] == null ? "" : dr["fs"]);
                            }
                        }
                        List<string> zb1 = new List<string>
                    { "生产运行综合指标<br>（含设备管理指标和压缩机考核）", "安全生产指标", "管道保护指标", "财务管理指标",
                        "工程管理指标", "预（结）算管理指标<br>（投资计划管理指标）", "物资管理指标","招投标和合同管理指标",
                        "人力资源管理指标", "党群工作综合指标", "技术信息管理指标", "行政管理指标" };
                        List<string> zb2 = new List<string>()
                    {"生产运行指标","安全生产指标","管道保护指标" ,"财务管理指标","工程管理指标","预结算及统计工作指标","物资管理指标","经营管理指标",
                        "人力资源工作指标","党群工作指标","技术信息管理指标","行政管理指标" };
                        List<double> score = new List<double> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                        List<double> qz1 = new List<double> { 0.2, 0.15, 0.15, 0.1, 0.05, 0.05, 0.05, 0.05, 0.05, 0.05, 0.05, 0.05 };
                        List<double> qz2 = new List<double> { 0.35, 0.15, 0, 0.1, 0.05, 0.05, 0.05, 0.05, 0.05, 0.05, 0.05, 0.05 };
                        List<double> qz3 = new List<double> { 0.05, 0.15, 0, 0.05, 0.15, 0.15, 0.1, 0.1, 0.05, 0.1, 0.05, 0.05 };
                        List<double> qz4 = new List<double> { 0.35, 0.15, 0, 0.1, 0.05, 0.05, 0.05, 0.05, 0.05, 0.05, 0.05, 0.05 };
                        double scoreTemp = 0;
                        for (int i = 0; i < zb1.Count; i++)
                        {
                            sb.Append("<tr><td class='center'>" + (i + 1) + "</td><td class='center'>" + zb1[i]);
                            sb.Append("</td><td></td><td></td><td></td><td></td><td class='center'>季度</td><td class='center'>100</td><td class='center'>" + qz1[i] * 100 + "%</td>");
                            scoreTemp = _getScoreByZbKhdw(hsZbDw, zb2[i], "川渝管理处");
                            score[0] += qz1[i] * scoreTemp;
                            sb.Append("<td class='right'>" + scoreTemp + "</td>");
                            scoreTemp = _getScoreByZbKhdw(hsZbDw, zb2[i], "鄂西管理处");
                            score[1] += qz1[i] * scoreTemp;
                            sb.Append("<td class='right'>" + scoreTemp + "</td>");
                            scoreTemp = _getScoreByZbKhdw(hsZbDw, zb2[i], "鄂东管理处");
                            score[2] += qz1[i] * scoreTemp;
                            sb.Append("<td class='right'>" + scoreTemp + "</td>");
                            scoreTemp = _getScoreByZbKhdw(hsZbDw, zb2[i], "安徽管理处");
                            score[3] += qz1[i] * scoreTemp;
                            sb.Append("<td class='right'>" + scoreTemp + "</td>");
                            scoreTemp = _getScoreByZbKhdw(hsZbDw, zb2[i], "浙沪管理处");
                            score[4] += qz1[i] * scoreTemp;
                            sb.Append("<td class='right'>" + scoreTemp + "</td>");
                            scoreTemp = _getScoreByZbKhdw(hsZbDw, zb2[i], "江苏管理处");
                            score[5] += qz1[i] * scoreTemp;
                            sb.Append("<td class='right'>" + scoreTemp + "</td>");
                            sb.Append("<td class='center'>" + qz2[i] * 100 + "%</td>");
                            scoreTemp = _getScoreByZbKhdw(hsZbDw, zb2[i], "宜昌维抢修中心");
                            score[6] += qz2[i] * scoreTemp;
                            sb.Append("<td class='right'>" + scoreTemp + "</td>");
                            scoreTemp = _getScoreByZbKhdw(hsZbDw, zb2[i], "嘉兴维抢修中心");
                            score[7] += qz2[i] * scoreTemp;
                            sb.Append("<td class='right'>" + scoreTemp + "</td>");
                            sb.Append("<td class='center'>" + qz3[i] * 100 + "%</td>");
                            scoreTemp = _getScoreByZbKhdw(hsZbDw, zb2[i], "金坛储气库");
                            score[8] += qz3[i] * scoreTemp;
                            sb.Append("<td class='right'>" + scoreTemp + "</td>");
                            sb.Append("<td class='center'>" + qz4[i] * 100 + "%</td>");
                            scoreTemp = _getScoreByZbKhdw(hsZbDw, zb2[i], "压缩机保运中心");
                            score[9] += qz4[i] * scoreTemp;
                            sb.Append("<td class='right'>" + scoreTemp + "</td>");
                            sb.Append("</tr>");
                        }
                        sb.Append("<tr><td colspan='7' class='center'>最终得分</td><td></td><td></td>");
                        sb.Append("<td class='right'>" + Math.Round(score[0], 2) + "</td><td class='right'>" + Math.Round(score[1], 2) + "</td><td class='right'>" + Math.Round(score[2], 2));
                        sb.Append("</td><td class='right'>" + Math.Round(score[3], 2) + "</td><td class='right'>" + Math.Round(score[4], 2) + "</td><td class='right'>" + Math.Round(score[5], 2) + "</td>");
                        sb.Append("<td></td>");
                        sb.Append("<td class='right'>" + Math.Round(score[6], 2) + "</td><td class='right'>" + Math.Round(score[7], 2) + "</td>");
                        sb.Append("<td></td>");
                        sb.Append("<td class='right'>" + Math.Round(score[8], 2) + "</td>");
                        sb.Append("<td></td>");
                        sb.Append("<td class='right'>" + Math.Round(score[9], 2) + "</td>");
                        sb.Append("</tr>");
                        #endregion
                    }
                    sb.Append("</table>");
                    p_Context.Response.Write(sb.ToString());
                }
            }
            catch (Exception ex)
            {
                p_Context.Response.Write("失败！原因：" + ex.Message);
            }
        }

        private double _getScoreByZbKhdw(Hashtable hs, string zb, string khdw)
        {
            double fz = 0;
            if (hs.ContainsKey(khdw + "_" + zb))
                fz = Convert.ToDouble(hs[khdw + "_" + zb]);
            return fz;
        }

        private void GetKhsj(HttpContext p_Context)
        {
            string sql = "select distinct ny from PERFO_KHX_KH order by ny desc";
            DataTable dt = DbHelperSQL.QueryToTable(sql);
            if (dt != null)
                p_Context.Response.Write(DbHelperSQL.TableToJson(dt));
        }
        #endregion

        #region 指标录入
        private void GetKhZb(HttpContext p_Context)
        {
            try
            {
                if (!string.IsNullOrEmpty(p_Context.Request["mbid"]))
                {//
                    string sql = string.Format("select id,xm1,xm2,nr,zq,fs,bzf,ord,type,bz from PERFO_KHX_ZB where mbid='{0}' order by ord", p_Context.Request["mbid"]);
                    DataTable dt = DbHelperSQL.QueryToTable(sql);
                    if (dt != null)
                        p_Context.Response.Write(DbHelperSQL.TableToJson(dt));
                }
            }
            catch (Exception ex)
            {
                p_Context.Response.Write("失败！原因：" + ex.Message);
            }
        }

        private void DeleteZb(HttpContext p_Context)
        {
            try
            {
                if (!string.IsNullOrEmpty(p_Context.Request["id"]))
                {
                    string sql = "delete from PERFO_KHX_ZB where id='" + p_Context.Request["id"] + "'";
                    DbHelperSQL.ExecuteOracle(sql);
                }
            }
            catch (Exception ex)
            {
                p_Context.Response.Write("失败！原因：" + ex.Message);
            }
        }

        private void GetKhXm(HttpContext p_Context)
        {
            try
            {
                if (!string.IsNullOrEmpty(p_Context.Request["lb"]))
                {
                    string sql = "select distinct " + p_Context.Request["lb"] + " from PERFO_KHX_ZB";
                    DataTable dt = DbHelperSQL.QueryToTable(sql);
                    if (dt != null)
                        p_Context.Response.Write(DbHelperSQL.TableToJson(dt));
                }
            }
            catch { }
        }

        private void SaveKhzb(HttpContext p_Context)
        {
            try
            {
                if (!string.IsNullOrEmpty(p_Context.Request["xm1"]) && !string.IsNullOrEmpty(p_Context.Request["nr"]) &&
                    !string.IsNullOrEmpty(p_Context.Request["mbid"]))
                {
                    #region 数据维护
                    string xm1 = p_Context.Request["xm1"];
                    string xm2 = p_Context.Request["xm2"];
                    string nr = p_Context.Request["nr"];
                    string bz = p_Context.Request["bz"];
                    string zq = p_Context.Request["zq"];
                    string fs = p_Context.Request["fs"];
                    string bzf = p_Context.Request["bzf"];
                    string id = p_Context.Request["id"];
                    string mbid = p_Context.Request["mbid"];
                    string pftype = p_Context.Request["pftype"];//如果改为根据考核内容打分，只需修改此处
                    string ord = p_Context.Request["ord"];
                    string sql = null;
                    if (string.IsNullOrEmpty(id))
                    {
                        sql = "insert into PERFO_KHX_ZB(xm1,xm2,nr,bz,zq,fs,bzf,ord,mbid,type) values(:xm1,:xm2,:nr,:bz,:zq,:fs,:bzf,:ord,:mbid,:type)";
                        OracleParameter[] param = new OracleParameter[] {
                        new OracleParameter(":xm1",OracleType.VarChar,50),
                        new OracleParameter(":xm2",OracleType.VarChar,100),
                        new OracleParameter(":nr",OracleType.VarChar,200),
                        new OracleParameter(":bz",OracleType.VarChar,1200),
                        new OracleParameter(":zq",OracleType.VarChar,50),
                        new OracleParameter(":fs",OracleType.VarChar,50),
                        new OracleParameter(":bzf",OracleType.Number),
                        new OracleParameter(":ord",OracleType.Number),
                        new OracleParameter(":mbid",OracleType.VarChar,50),
                        new OracleParameter(":type",OracleType.VarChar,10)
                    };
                        param[0].Value = xm1;
                        param[1].Value = xm2;
                        param[2].Value = nr;
                        param[3].Value = bz;
                        param[4].Value = zq;
                        param[5].Value = fs;
                        param[6].Value = bzf;
                        param[7].Value = ord;
                        param[8].Value = mbid;
                        param[9].Value = pftype;
                        DbHelperSQL.ExecuteOracle(sql, param);
                    }
                    else
                    {
                        sql = "update PERFO_KHX_ZB set xm1=:xm1,xm2=:xm2,nr=:nr,bz=:bz,zq=:zq,fs=:fs,bzf=:bzf,ord=:ord,mbid=:mbid where id=:id";
                        OracleParameter[] param = new OracleParameter[] {
                        new OracleParameter(":xm1",OracleType.VarChar,50),
                        new OracleParameter(":xm2",OracleType.VarChar,100),
                        new OracleParameter(":nr",OracleType.VarChar,200),
                        new OracleParameter(":bz",OracleType.VarChar,1200),
                        new OracleParameter(":zq",OracleType.VarChar,50),
                        new OracleParameter(":fs",OracleType.VarChar,50),
                        new OracleParameter(":bzf",OracleType.Number),
                        new OracleParameter(":ord",OracleType.Number),
                        new OracleParameter(":mbid",OracleType.VarChar,50),
                        new OracleParameter(":id",OracleType.VarChar,50)
                    };
                        param[0].Value = xm1;
                        param[1].Value = xm2;
                        param[2].Value = nr;
                        param[3].Value = bz;
                        param[4].Value = zq;
                        param[5].Value = fs;
                        param[6].Value = bzf;
                        param[7].Value = ord;
                        param[8].Value = mbid;
                        param[9].Value = id;
                        DbHelperSQL.ExecuteOracle(sql, param);
                    }
                    #endregion
                }
                else
                {
                    p_Context.Response.Write("失败！原因：您没有正确操作，请重新按规则操作！");
                }
            }
            catch (Exception ex) { p_Context.Response.Write("失败！原因：" + ex.Message); }
        }

        private void SaveZbType(HttpContext p_Context)
        {
            try
            {
                if (!string.IsNullOrEmpty(p_Context.Request["mbid"]) && !string.IsNullOrEmpty(p_Context.Request["pftype"]))
                {
                    string sql = "update PERFO_KHX_ZB set type='" + p_Context.Request["pftype"] + "' where mbid='" + p_Context.Request["mbid"] + "'";
                    DbHelperSQL.ExecuteOracle(sql);
                }
                else
                {
                    p_Context.Response.Write("失败！原因：操作不正确！");
                }
            }
            catch (Exception ex)
            {
                p_Context.Response.Write("失败！原因：" + ex.Message);
            }
        }
        #endregion

        #region 考核单位模板配置
        private void GetKhdwmb(HttpContext p_Context)
        {
            if (!string.IsNullOrEmpty(p_Context.Request["khdzbid"]))
            {
                string sql = @"select b.id,a.khdzbid,a.id mbid,b.jgbm,a.name mbmc,c.jgmc
from PERFO_KHX_MB a
left join PERFO_KHS_MBJG b on a.khdzbid=b.khdzbid and b.mbid=a.id
left join PERFO_SYS_BMJG c on b.jgbm=c.jgbm
where a.khdzbid='" + p_Context.Request["khdzbid"] + "'";
                DataTable dt = DbHelperSQL.QueryToTable(sql);
                if (dt != null)
                    p_Context.Response.Write(DbHelperSQL.TableToJson(dt));
            }
        }

        private void SaveDwmb(HttpContext p_Context)
        {
            try
            {
                if (!string.IsNullOrEmpty(p_Context.Request["mbid"]) && !string.IsNullOrEmpty(p_Context.Request["bkhdw"]) && !string.IsNullOrEmpty(p_Context.Request["khdzbid"]))
                {
                    string sql = null;
                    if (!string.IsNullOrEmpty(p_Context.Request["id"]))
                    {
                        sql = "update PERFO_KHS_MBJG set mbid='" + p_Context.Request["mbid"] + "' and khdzbid='" + p_Context.Request["khdzbid"] + "',jgbm='" + p_Context.Request["bkhdw"] + "' where id='" + p_Context.Request["id"] + "'";
                    }
                    else
                    {
                        sql = "select count(*) from PERFO_KHS_MBJG where khdzbid='" + p_Context.Request["khdzbid"] + "'";
                        DataTable dt = DbHelperSQL.QueryToTable(sql);
                        int count = Convert.ToInt32(dt.Rows[0][0]);
                        string id = Guid.NewGuid().ToString();
                        sql = "insert into PERFO_KHS_MBJG(id,mbid,khdzbid,jgbm,px) values('" + id + "','" + p_Context.Request["mbid"] + "','" + p_Context.Request["khdzbid"] + "','" + p_Context.Request["bkhdw"] + "','" + (count + 1) + "')";
                    }
                    DbHelperSQL.ExecuteOracle(sql);
                }
                else
                {
                    p_Context.Response.Write("失败！原因：操作错误！");
                }
            }
            catch (Exception ex)
            {
                p_Context.Response.Write("失败！原因：" + ex.Message);
            }
        }

        private void GetKhDwmbByID(HttpContext p_Context)
        {
            if (!string.IsNullOrEmpty(p_Context.Request["id"]))
            {
                string sql = "select * from PERFO_KHS_MBJG where id='" + p_Context.Request["id"] + "'";
                DataTable dt = DbHelperSQL.QueryToTable(sql);
                if (dt != null)
                    p_Context.Response.Write(DbHelperSQL.TableToJson(dt));
            }
        }

        private void DeleteDwmb(HttpContext p_Context)
        {
            try
            {
                if (!string.IsNullOrEmpty(p_Context.Request["id"]))
                {
                    string sql = "delete from PERFO_KHS_MBJG where id='" + p_Context.Request["id"] + "'";
                    DbHelperSQL.ExecuteOracle(sql);
                }
                else p_Context.Response.Write("失败！原因：操作不规范！");
            }
            catch (Exception ex)
            {
                p_Context.Response.Write("失败！原因：" + ex.Message);
            }
        }
        #endregion

        #region 考核列表
        private void GetKhList(HttpContext p_Context)
        {
            string sql = @"select b.id,a.khid,a.khmc,a.kssj,a.gbsj,decode(a.khzt,'0','考核中','1','已完成','待定义') khzt,c.zbmc,a.lxid
from PERFO_KH_KHGL a,perfo_khs_khdzb b, perfo_khs_dzb c
 where a.khid = b.khid and b.dzbid = c.dzbid";
            if (!string.IsNullOrEmpty(p_Context.Request["bm"]))
                sql += " and c.fzbm = '" + p_Context.Request["bm"] + "'";
            DataTable dt = DbHelperSQL.QueryToTable(sql);
            if (dt != null)
            {
                p_Context.Response.Write(DbHelperSQL.TableToJson(dt));
            }
        }

        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}