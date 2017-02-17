<%@ Page Title="" Language="C#" MasterPageFile="../Project.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="QzEdit2.aspx.cs" Inherits="Enterprise.UI.Web.Kh.QzEdit2" %>

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
                    var glcqz=$("#glc_"+i).val()*1;
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
            <table class="GridViewStyle" cellspacing="0" rules="all" border="1" id="ProjectPH_GridView1" style="width:100%;border-collapse:collapse;">
                <tr class="GridViewHeaderStyle">
                    <th rowspan="2"> 序号 </th>
                    <th rowspan="2">考核指标</th>
                    <th rowspan="2"> 权重</br></th>
                    <th colspan="<%=jgL.Count() %>" >考核得分</th>
                 </tr>
                <tr class="GridViewHeaderStyle">
                    <%
                        foreach (var m in jgL) { 
                        %>
                        <th><%=m.JGMC %></th>                   
                    <%
                        }
                         %>
                 </tr>
                
                 <%
                     int i = 1;
                     foreach (var m in dzbL) { 
                         
                        %>
                <tr>
                    <td nowrap align="center"><%=i %></td>
                    <td nowrap align="center"><%=dic2[m.DZBID] %></td>
                    <td nowrap align="center">
                                <input name="glcqz" type="text" value="<%=glc.ContainsKey(m.ID)?glc[m.ID]+"":"" %>" id="glc_<%=i %>" class="easyui-numberbox" min="0" max="100" precision="0" missingMessage="必须填写数字" style="background-color:LawnGreen;width:20px;" />                        
                    </td>
                    <%
                         int j = 1;
                        foreach(var n in jgL){
                            %>
                            <td nowrap align="center">
                                <span id="pf_<%=i %>_<%=j %>"><%=dic.ContainsKey(m.ID+n.JGBM)?dic[m.ID+n.JGBM]+"":"" %></span>
                            </td>
                            <%--<td nowrap align="center">
                                <span id="fz_<%=i %>_<%=j %>"></span>
                            </td>--%>
                    <%
                            j++;
                        }
                         i++;
                         %>

                </tr>
                <%
                     }   
                      %>
                <tr>
                    <td colspan="3" align="center">最终得分</td>   
                    <%
                        int t = 1;
                        foreach(var n in jgL){
                         %> 
                    <td nowrap align="center"><span id="zj_<%=t %>" style="color:red"></span></td>
                    <%
                        t++; 
                    }
                        %>
                </tr>
            </table>
            
            <div class="btn-bgcolor">
                <asp:LinkButton ID="LinkButton1" runat="server" class="easyui-linkbutton" iconCls="icon-ok"
                    OnClientClick="Heji();return false;">计算</asp:LinkButton>
                <asp:LinkButton ID="LnkBtn_Ins" runat="server" class="easyui-linkbutton" iconCls="icon-save"
                    OnClientClick="return checkForm();" OnClick="LnkBtn_Ins_Click">保存</asp:LinkButton>
                <asp:LinkButton ID="LinkButton2" runat="server" Text="返回" class="easyui-linkbutton" plain="true"
                    iconCls="icon-back" OnClick="LnkBtn_Cancel_Click"></asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
