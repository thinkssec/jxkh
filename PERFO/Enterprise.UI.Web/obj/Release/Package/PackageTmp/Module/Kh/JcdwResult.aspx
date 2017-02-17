<%@ Page Title="" Language="C#" MasterPageFile="../Project.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="JcdwResult.aspx.cs" Inherits="Enterprise.UI.Web.Kh.JcdwResult" %>

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
                    var glcqz2=$("#glc2_"+i).text()*1;
                    var wqxqz=$("#wqx_"+i).text()*1;
                    var jtqz=$("#jt_"+i).text()*1;
                    var ysjqz=$("#ysj_"+i).text()*1;
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
                            $("#fz_"+i+"_"+j).text(fz.toFixed(4));
                        }

                        var cur=$("#hj_"+j).text();
                        var xs=$("#xs_"+j).text();
                        var zj=$("#zj_"+j).text();
                        if(isNaN(cur*1)) cur=0;
                        if(isNaN(zj*1)) zj=0;
                        $("#hj_"+j).text((cur*1+fz*1).toFixed(4));
                        //$("#zj_"+j).text((zj*1+fz*1*xs).toFixed(4));

                    }
                }
                for(var j=1;j<=<%=jgL.Count() %>;j++){
                    var h=$("#hj_"+j).text();
                    var x=$("#xs_"+j).text();
                    $("#zj_"+j).text((h*1*x).toFixed(4));

                }

                var myArray=new Array(10);
                myArray[0]= $("#zj_"+1).text()*1;
                myArray[1]= $("#zj_"+2).text()*1;
                myArray[2]= $("#zj_"+3).text()*1;
                myArray[3]= $("#zj_"+4).text()*1;
                myArray[4]= $("#zj_"+5).text()*1;
                myArray[5]= $("#zj_"+6).text()*1;
                myArray[6]= $("#zj_"+7).text()*1;
                myArray[7]= $("#zj_"+8).text()*1;
                myArray[8]= $("#zj_"+9).text()*1;
                myArray[9]= $("#zj_"+10).text()*1;
                myArray.sort(function(a,b){return a>b?1:-1});
                for(var i=1;i<=10;i++){
                    var t=$("#zj_"+i).text();
                    for(var j=0;j<myArray.length;j++){
                        if(t==myArray[j]){
                            $("#px_"+i).text(10-j);
                            continue;
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
                                <span   id="glc_<%=i %>"><%=glc.ContainsKey(m.ID)?glc[m.ID]+"":"" %></span>
                        
                            </td>
                            <td nowrap align="center">
                                <span id="pf_<%=i %>_<%=j %>"><%=dic.ContainsKey(m.ID+n.JGBM)?dic[m.ID+n.JGBM]+"":"" %></span>
                            </td>
                            <td nowrap align="center">
                                <span id="fz_<%=i %>_<%=j %>"></span>
                            </td>
                    <%      }else if(j==4){
                            %>
                            <td nowrap align="center">
                                <span   id="glc2_<%=i %>"><%=glc2.ContainsKey(m.ID)?glc2[m.ID]+"":"" %></span>
                        
                            </td>
                            <td nowrap align="center">
                                <span id="pf_<%=i %>_<%=j %>"><%=dic.ContainsKey(m.ID+n.JGBM)?dic[m.ID+n.JGBM]+"":"" %></span>
                            </td>
                            <td nowrap align="center">
                                <span id="fz_<%=i %>_<%=j %>"></span>
                            </td>
                            <%
                            }else if (j == 7 ) { 
                            %>
                            <td nowrap align="center">
                                <span id="wqx_<%=i %>"><%=wqx.ContainsKey(m.ID)?wqx[m.ID]+"":"" %></span>
                        
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
                                <span  id="jt_<%=i %>"><%=jt.ContainsKey(m.ID)?jt[m.ID]+"":"" %></span>
                        
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
                                <span  id="ysj_<%=i %>"><%=ysj.ContainsKey(m.ID)?ysj[m.ID]+"":"" %></span>
                        
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
                <tr>
                    <td nowrap align="center"></td>
                    <td nowrap align="center">初始合计分数</td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"><span id="hj_1" style="color:red"></span></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"><span id="hj_2" style="color:red"></span></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"><span id="hj_3" style="color:red"></span></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"> </td>
                    <td nowrap align="center"><span id="hj_4" style="color:red"></span></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"><span id="hj_5" style="color:red"></span></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"><span id="hj_6" style="color:red"></span></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"><span id="hj_7" style="color:red"></span></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"><span id="hj_8" style="color:red"></span></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"><span id="hj_9" style="color:red"></span></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"><span id="hj_10" style="color:red"></span></td>
                </tr>

                <tr>
                    <td nowrap align="center"></td>
                    <td nowrap align="center">考核系数</td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"><span id="xs_1" style="color:red"><%=xsL[0].XS %></span></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"><span id="xs_2" style="color:red"><%=xsL[1].XS %></span></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"><span id="xs_3" style="color:red"><%=xsL[2].XS %></span></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"> </td>
                    <td nowrap align="center"><span id="xs_4" style="color:red"><%=xsL[3].XS %></span></td>
                    <td nowrap align="center"></td>

                    <td nowrap align="center"><span id="xs_5" style="color:red"><%=xsL[4].XS %></span></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"><span id="xs_6" style="color:red"><%=xsL[5].XS %></span></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"><span id="xs_7" style="color:red"><%=xsL[6].XS %></span></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"><span id="xs_8" style="color:red"><%=xsL[7].XS %></span></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"><span id="xs_9" style="color:red"><%=xsL[8].XS %></span></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"><span id="xs_10" style="color:red"><%=xsL[9].XS %></span></td>
                </tr>
                <tr>
                    <td nowrap align="center"></td>
                    <td nowrap align="center">最终合计分数</td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"><span id="zj_1" style="color:red"></span></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"><span id="zj_2" style="color:red"></span></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"><span id="zj_3" style="color:red"></span></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"><span id="zj_4" style="color:red"></span></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"><span id="zj_5" style="color:red"></span></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"><span id="zj_6" style="color:red"></span></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"><span id="zj_7" style="color:red"></span></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"><span id="zj_8" style="color:red"></span></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"><span id="zj_9" style="color:red"></span></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"></td>
                    <td nowrap align="center"><span id="zj_10" style="color:red"></span></td>
                </tr>
                
                <tr>
                    <td nowrap align="center"> </td>
                    <td nowrap align="center">最终排名</td>
                    <td nowrap align="center"> </td>
                    <td nowrap align="center" colspan="2"><span id="px_1" style="color:red"></span></td>
                    <td nowrap align="center" colspan="2"><span id="px_2" style="color:red"></span></td>
                    <td nowrap align="center" colspan="2"><span id="px_3" style="color:red"></span></td>
                    <td nowrap align="center"> </td>
                    <td nowrap align="center" colspan="2"><span id="px_4" style="color:red"> </span></td>
                    <td nowrap align="center" colspan="2"><span id="px_5" style="color:red"> </span></td>
                    <td nowrap align="center" colspan="2"><span id="px_6" style="color:red"> </span></td>
                     <td nowrap align="center"> </td>
                    <td nowrap align="center" colspan="2"><span id="px_7" style="color:red"> </span></td>
                    <td nowrap align="center" colspan="2"><span id="px_8" style="color:red"> </span></td>
                    <td nowrap align="center"> </td>
                    <td nowrap align="center" colspan="2"><span id="px_9" style="color:red"> </span></td>
                    <td nowrap align="center"> </td>
                    <td nowrap align="center" colspan="2"><span id="px_10" style="color:red"> </span></td>
                </tr>
            </table>
            
            <div class="btn-bgcolor">
                <%--<asp:LinkButton ID="LinkButton1" runat="server" class="easyui-linkbutton" iconCls="icon-ok"
                    OnClientClick="Heji();return false;">计算</asp:LinkButton>
                <asp:LinkButton ID="LnkBtn_Ins" runat="server" class="easyui-linkbutton" iconCls="icon-save"
                    OnClientClick="return checkForm();" OnClick="LnkBtn_Ins_Click">保存</asp:LinkButton>--%>
                <asp:LinkButton ID="LinkButton2" runat="server" Text="返回" class="easyui-linkbutton" plain="true"
                    iconCls="icon-back" OnClick="LnkBtn_Cancel_Click"></asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
