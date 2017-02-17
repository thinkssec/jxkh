<%@ Page Title="" Language="C#" MasterPageFile="../Project.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="Jxkhbhz3.aspx.cs" Inherits="Enterprise.UI.Web.Kh.Jxkhbhz3" %>

<%@ Import Namespace="Enterprise.Model.Perfo.Kh" %>
<%@ Import Namespace="Enterprise.Component.Infrastructure" %>
<%@ Register Src="~/Component/UserControl/PopWinUploadMuti.ascx" TagPrefix="uc1" TagName="PopWinUploadMuti" %>
<%@ Register Assembly="Enterprise.Component.Infrastructure" Namespace="Enterprise.Component.Infrastructure" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function checkForm() {
            Heji();
            var r = $("#form1").form('validate');
            return r;
        }
        function Heji(txtObj) {
            try {
                for(var i=1;i<=<%=dzbL.Count() %>;i++){
                    var glcqz=$("#glc_"+i).text()*1;
                    var qz=0;
                    for(var j=1;j<=<%=jgL.Count() %>;j++){
                        qz=glcqz;

                        var pf=$("#pf_"+i+"_"+j).text()*1;
                        var fz=pf*qz/100;
                        var zj=$("#zj_"+j).text();
                        if(isNaN(zj*1)) zj=0;
                        
                        $("#zj_"+j).text((zj*1+fz).toFixed(2));
                    }
                }
                
            }
            catch (e) {
                alert(e);
            }
        }
        window.onload = Heji;
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ProjectPH" runat="server">
    <div data-options="region:'center'">
        <div id="Div1" class="ssec-form">
            <%--<div class="ssec-group ssec-group-hasicon">
                <div class="icon-form"></div>
                <span>考核系数维护</span>
            </div>--%>
        </div>
        <div class="main-gridview">
            <asp:HiddenField ID="Hid_KHID" runat="server" />
            <%--<div class="main-gridview-title">
                考核期：<asp:Label ID="Lbl_Kaohe" runat="server" ForeColor="Black"></asp:Label>
                &nbsp;&nbsp;
                单位：<asp:Label ID="Lbl_Danwei" runat="server" ForeColor="Black"></asp:Label>
                &nbsp;&nbsp;
                考核年度：<asp:Label ID="Lbl_Niandu" runat="server" ForeColor="Black"></asp:Label>
            </div>--%>
            <asp:HiddenField ID="strxs" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="Hid_CB" runat="server" ClientIDMode="Static" />
            <div id="openwin" class="easyui-window" style="width: 600px; height: 360px" title="附件信息" closed="true" modal="true"></div>
            
            <table class="GridViewStyle" cellspacing="0" rules="all" border="1" id="ProjectPH_GridView1" style="width: 100%; border-collapse: collapse;">
                <tr class="GridViewHeaderStyle">
                    <th rowspan="2">序号 </th>
                    <th rowspan="2">考核指标</th>
                    <th rowspan="2">状态 </th>
                    <th rowspan="2">权重</br></th>
                    <th colspan="1"><%=Enterprise.Service.Perfo.Sys.SysBmjgService.GetBmjgName(dwdm.ToInt()) %></th>
                    
                </tr>
                <tr class="GridViewHeaderStyle">
                    <th colspan="1">评分</th>
                </tr>

                <%
                    int i = 1;
                    foreach (var m in dzbL)
                    { 
                         
                %>
                <tr>
                    <td nowrap align="center"><%=i %></td>
                    <td nowrap align="center"><input type="checkbox" name="CB_DZB" 
                        value="<%=dic3.ContainsKey(m.ID)?dic3[m.ID]:"" %>" 
                        <%=mbjg.ContainsKey(m.ID)?(mbjg[m.ID]== "50" ? "" : "disabled"):"disabled" %>/>
                        <a href="/Module/KhX/Khzbpf.aspx?khid=<%=Khid %>&mbjgid=<%=dic3.ContainsKey(m.ID)?dic3[m.ID]:"" %>&back=Jxkhbhz3&audit=1&dwdm=<%=dwdm %>" ><%=dic2[m.DZBID] %></a></td>
                    <td nowrap align="center"><%=mbjg.ContainsKey(m.ID)?mbjgSrv.GetStatus21(mbjg[m.ID]):"正在打分" %></td>
                    <td nowrap align="center">

                        <span id="glc_<%=i %>"><%=glc.ContainsKey(m.ID)?glc[m.ID]+"":"" %></span>

                    </td>
                    <td nowrap align="center">
                        <%
                             if (dic3.ContainsKey(m.ID))
                             { 
                        %>
                        <a href='#' onclick="openwin('openwin','/Module/Kh/KhfjList.aspx?MBJGID=<%=dic3[m.ID] %>&JGBM=<%=dwdm %>','600','360')">
                            <%
                         }
                            %>
                            <span id="pf_<%=i %>_1"><%=dic.ContainsKey(m.ID+dwdm)?dic[m.ID+dwdm]+"":"" %></span>
                            <%
                             if (dic3.ContainsKey(m.ID))
                             { 
                            %>
                        </a> 
                        <%
                         }
                        %>  
                               
                    </td>



                    <%
                       
                         i++;
                    %>
                </tr>
                <%
                    }   
                %>
                <tr>
                    <td colspan="4" align="center">最终得分</td>
                    <%
                        int t = 1;
                        foreach (var n in jgL)
                        {
                    %>
                    <td nowrap align="center"><span id="zj_<%=t %>" style="color: red"></span></td>
                    <%
                            t++;
                        }
                    %>
                </tr>
            </table>

            <div class="btn-bgcolor">
                <%--<asp:LinkButton ID="LinkButton1" runat="server" class="easyui-linkbutton" iconCls="icon-ok"
                    OnClientClick="Heji();return false;">计算</asp:LinkButton>
                <asp:LinkButton ID="LnkBtn_Ins" runat="server" class="easyui-linkbutton" iconCls="icon-save"
                    OnClientClick="return checkForm();" OnClick="LnkBtn_Ins_Click">保存</asp:LinkButton>--%>
                 <asp:LinkButton ID="LinkButton1" runat="server" Text="退回" class="easyui-linkbutton" plain="true"
                    iconCls="icon-cancel" OnClick="Btn_No_Click" OnClientClick="getCb();"></asp:LinkButton>
                &nbsp;&nbsp;
                <asp:LinkButton ID="LinkButton2" runat="server" Text="返回" class="easyui-linkbutton" plain="true"
                    iconCls="icon-back" OnClick="LnkBtn_Cancel_Click"></asp:LinkButton>
            </div>
        </div>
    </div>
    <script>
        function getCb(){
            var str="";
            $("input[name='CB_DZB']:checkbox").each(function(){ 
                if($(this).attr("checked")){
                    str += $(this).val()+","
                }
            })
            //alert(str);
            str.split(",");
            //alert(str);
            $("#Hid_CB").val(str);
        
        }
        </script>
</asp:Content>
