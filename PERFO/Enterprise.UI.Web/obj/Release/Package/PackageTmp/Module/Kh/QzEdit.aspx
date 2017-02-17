<%@ Page Title="" Language="C#" MasterPageFile="../Project.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="QzEdit.aspx.cs" Inherits="Enterprise.UI.Web.Kh.QzEdit" %>

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
                    var glcqz2=$("#glc2_"+i).val()*1;
                    var wqxqz=$("#wqx_"+i).val()*1;
                    var jtqz=$("#jt_"+i).val()*1;
                    var ysjqz=$("#ysj_"+i).val()*1;
                    var qz=0;
                    for(var j=1;j<=<%=jgL.Count() %>;j++){
                        if(j<=3) qz=glcqz;
                        if(j<=6&&j>3) qz=glcqz2;
                        if(j<=8&&j>6) qz=wqxqz;
                        if(j>8&&j<=9) qz=jtqz;
                        if(j>9) qz=ysjqz;
                        var pf=$("#pf_"+i+"_"+j).text()*1;
                        //if(i==1){
                        //    alert(pf*qz);
                        //}
                        var fz=pf*qz/100;
                        if(!isNaN(pf*qz)){
                            $("#fz_"+i+"_"+j).text(fz.toFixed(3));
                        }
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
                    <th rowspan="3"> 序号 </th>
                    <th rowspan="3">考核指标</th>
                    <th colspan="14">管理处</th>
                    <th colspan="5">维抢修中心</th>
                    <th rowspan="3"> 权重</br>（W） </th>
                    <th colspan="2" rowspan="2">金坛储气库</th>
                    <th rowspan="3"> 权重</br>（W） </th>
                    <th colspan="2" rowspan="2">压缩机保运中心</th>
                 </tr>
                <tr class="GridViewHeaderStyle">
                    <th rowspan="2"> 权重</br>（W） </th>
                    <th colspan="2">川渝管理处</th>
                    <th colspan="2">鄂西管理处</th>
                    <th colspan="2">鄂东管理处</th>
                    <th rowspan="2"> 权重</br>（W） </th>
                    <th colspan="2">安徽管理处</th>
                    <th colspan="2">江苏管理处</th>
                    <th colspan="2">浙沪管理处</th>
                    <th rowspan="2"> 权重</br>（W） </th>
                    <th colspan="2">宜昌维抢修中心</th>
                    <th colspan="2">嘉兴维抢修中心</th>                  
                 </tr>
                <tr class="GridViewHeaderStyle">
                    <th > 评分</br>（K） </th>  
                    <th > 分值</br>W*K </th>  
                    <th > 评分</br>（K） </th>  
                    <th > 分值</br>W*K </th> 
                    <th > 评分</br>（K） </th>  
                    <th > 分值</br>W*K </th> 
                    <th > 评分</br>（K） </th>  
                    <th > 分值</br>W*K </th> 
                    <th > 评分</br>（K） </th>  
                    <th > 分值</br>W*K </th>
                     <th > 评分</br>（K） </th>  
                    <th > 分值</br>W*K </th>
                     <th > 评分</br>（K） </th>  
                    <th > 分值</br>W*K </th>
                     <th > 评分</br>（K） </th>  
                    <th > 分值</br>W*K </th>   
                    <%--<th> 权重</br>（W） </th>--%>
                    <th > 评分</br>（K） </th>  
                    <th > 分值</br>W*K </th> 
                    <%--<th > 权重</br>（W） </th>--%>
                    <th > 评分</br>（K） </th>  
                    <th > 分值</br>W*K </th>         
                 </tr>
                 <%
                     int i = 1;
                     foreach (var m in dzbL) { 
                         
                        %>
                <tr>
                    <td nowrap align="center"><%=i %></td>
                    <td nowrap align="center"><%=dic2[m.DZBID] %></td>
                    <%
                         int j = 1;
                        foreach(var n in jgL){
                            if (j == 1 ) { 
                            %>
                            <td nowrap align="center">
                                <input name="glcqz" type="text" value="<%=glc.ContainsKey(m.ID)?glc[m.ID]+"":"" %>" id="glc_<%=i %>" class="easyui-numberbox" min="0" max="100" precision="0" missingMessage="必须填写数字" style="background-color:LawnGreen;width:20px;" />
                        
                            </td>
                            <td nowrap align="center">
                                <span id="pf_<%=i %>_<%=j %>"><%=dic.ContainsKey(m.ID+n.JGBM)?dic[m.ID+n.JGBM]+"":"" %></span>
                            </td>
                            <td nowrap align="center">
                                <span id="fz_<%=i %>_<%=j %>"></span>
                            </td>
                    <%      }else if (j == 4) { 
                            %>
                            <td nowrap align="center">
                                <input name="glcqz2" type="text" value="<%=glc.ContainsKey(m.ID)?glc2[m.ID]+"":"" %>" id="glc2_<%=i %>" class="easyui-numberbox" min="0" max="100" precision="0" missingMessage="必须填写数字" style="background-color:LawnGreen;width:20px;" />
                            </td>
                            <td nowrap align="center">
                                <span id="pf_<%=i %>_<%=j %>"><%=dic.ContainsKey(m.ID+n.JGBM)?dic[m.ID+n.JGBM]+"":"" %></span>
                            </td>
                            <td nowrap align="center">
                                <span id="fz_<%=i %>_<%=j %>"></span>
                            </td>
                            <%
                            }
                            else if (j == 7 ) { 
                            %>
                            <td nowrap align="center">
                                <input name="wqxqz" type="text" value="<%=wqx.ContainsKey(m.ID)?wqx[m.ID]+"":"" %>" id="wqx_<%=i %>" class="easyui-numberbox" min="0" max="100" precision="0" missingMessage="必须填写数字" style="background-color:LawnGreen;width:20px;" />
                        
                            </td>
                            <td nowrap align="center">
                                <span id="pf_<%=i %>_<%=j %>"><%=dic.ContainsKey(m.ID+n.JGBM)?dic[m.ID+n.JGBM]+"":"" %></span>
                            </td>
                            <td nowrap align="center">
                                <span id="fz_<%=i %>_<%=j %>"></span>
                            </td>
                    <%      }else if (j == 9 ) { 
                            %>
                            <td nowrap align="center">
                                <input name="jtqz" type="text" value="<%=jt.ContainsKey(m.ID)?jt[m.ID]+"":"" %>" id="jt_<%=i %>" class="easyui-numberbox" min="0" max="100" precision="0" missingMessage="必须填写数字" style="background-color:LawnGreen;width:20px;" />
                        
                            </td>
                            <td nowrap align="center">
                                <span id="pf_<%=i %>_<%=j %>"><%=dic.ContainsKey(m.ID+n.JGBM)?dic[m.ID+n.JGBM]+"":"" %></span>
                            </td>
                            <td nowrap align="center">
                                <span id="fz_<%=i %>_<%=j %>"></span>
                            </td>
                    <%      }else if (j == 10) { 
                            %>
                            <td nowrap align="center">
                                <input name="ysjqz" type="text" value="<%=ysj.ContainsKey(m.ID)?ysj[m.ID]+"":"" %>" id="ysj_<%=i %>" class="easyui-numberbox" min="0" max="100" precision="0" missingMessage="必须填写数字" style="background-color:LawnGreen;width:20px;" />
                        
                            </td>
                            <td nowrap align="center">
                                <span id="pf_<%=i %>_<%=j %>"><%=dic.ContainsKey(m.ID+n.JGBM)?dic[m.ID+n.JGBM]+"":"" %></span>
                            </td>
                            <td nowrap align="center">
                                <span id="fz_<%=i %>_<%=j %>"></span>
                            </td>
                    <%      }
                            else { 
                            %>
                              <td nowrap align="center">
                                <span id="pf_<%=i %>_<%=j %>"><%=dic.ContainsKey(m.ID+n.JGBM)?dic[m.ID+n.JGBM]+"":"" %></span>
                            </td>
                            <td nowrap align="center">
                                <span id="fz_<%=i %>_<%=j %>"></span>
                            </td>
                               <%
                            }
                            j++;
                        }
                         i++;
                         %>

                </tr>
                <%
                     }   
                      %>
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
