<%@ Page Title="" Language="C#" MasterPageFile="../Project.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="XsList.aspx.cs" Inherits="Enterprise.UI.Web.Kh.XsList" %>

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
                $("span[id*='hj']").each(function (i) {
                    var strs = $(this).attr("id").split("_");
                    var deptid = strs[1];
                    //alert(strs[2]);
                    if (strs[2] == "hj1") {
                        var col1 = $("#Txt_" + deptid + "_jiguan").val();
                        var col2 = $("#Txt_" + deptid + "_yunxingjizu").val();
                        var col3 = $("#Txt_" + deptid + "_rebeijizu").val();
                        var col4 = $("#Txt_" + deptid + "_lengbeijizu").val();
                        var hj1 = col2 * 1 + col3 * 1 + col4 * 1;
                        $(this).text(hj1);
                    } else if (strs[2] == "hj2") {
                        var col5 = $("#Txt_" + deptid + "_daxingshuqizhan").val();
                        var col6 = $("#Txt_" + deptid + "_xiaoxingshuqizhan").val();
                        var hj2 = col5 * 1 + col6 * 1;
                        $(this).text(hj2);
                    } else if (strs[2] == "hj3") {
                        var col7 = $("#Txt_" + deptid + "_weixiudui").val();
                        var hj3 = col7 * 1;
                        $(this).text(hj3);
                    }
                    //alert($(this).attr("id"));
                });
                var gls = 0;
                $("input[id^='Txt_'][id$='_gonglishu']").each(function (i) {
                    gls += $(this).val() * 1;
                });
                $("input[id^='Txtx_']").each(function (i) {
                    //alert($(this).attr("id"));
                    var strs = $(this).attr("id").split("_");
                    var deptid = strs[1];
                    var attr = strs[2];
                    if (attr == "jiguan") {
                        var col = $("#Txt_" + deptid + "_" + attr).val();
                        var hj = col * 0;
                        $(this).val(hj.toFixed(3));
                    } else if (attr == "yunxingjizu") {
                        var col = $("#Txt_" + deptid + "_" + attr).val();
                        var hj = col * 0.01;
                        $(this).val(hj.toFixed(3));
                    } else if (attr == "rebeijizu") {
                        var col = $("#Txt_" + deptid + "_" + attr).val();
                        var hj = col * 0.005;
                        $(this).val(hj.toFixed(3));
                    } else if (attr == "lengbeijizu") {
                        var col = $("#Txt_" + deptid + "_" + attr).val();
                        var hj = col * 0.001;
                        $(this).val(hj.toFixed(3));
                    } else if (attr == "daxingshuqizhan") {
                        var col = $("#Txt_" + deptid + "_" + attr).val();
                        var hj = col * 0.012;
                        $(this).val(hj.toFixed(3));
                    } else if (attr == "xiaoxingshuqizhan") {
                        var col = $("#Txt_" + deptid + "_" + attr).val();
                        var hj = col * 0.006;
                        $(this).val(hj.toFixed(3));
                    } else if (attr == "gonglishu") {
                        var col = $("#Txt_" + deptid + "_" + attr).val();
                        var hj = col * 0.005 / 100;
                        $(this).val(hj.toFixed(3));
                        if (deptid == "104" || deptid == "122" || deptid == "153")
                            $(this).val("0.000");
                    } else if (attr == "dimao") {
                        var col = $("#Ddl_" + deptid + "_" + attr).val();
                        var hj = col * 1;
                        $(this).val(hj.toFixed(3));
                    } else if (attr == "chengzhenhua") {
                        var col = $("#Ddl_" + deptid + "_" + attr).val();
                        var hj = col * 1;
                        $(this).val(hj.toFixed(3));
                    } else if (attr == "weixiudui") {
                        var col = $("#Txt_" + deptid + "_" + attr).val();
                        var hj = col * 0;
                        if (deptid == "104") hj = col * 0.04;
                        else if (deptid == "122") hj = col * 0.04;
                        if (deptid == "153") hj = col * 0.02;
                        $(this).val(hj.toFixed(3));
                    }
                    //$("#Lb_" + deptid + "_tj1").text(tj1);

                });
                $("span[id^='Lb_'][id*='_tj']").each(function (i) {
                    var strs = $(this).attr("id").split("_");
                    var deptid = strs[1];
                    var attr = strs[2];
                    if (attr == "tj1") {
                        var col1 = $("#Txtx_" + deptid + "_yunxingjizu").val();
                        var col2 = $("#Txtx_" + deptid + "_rebeijizu").val();
                        var col3 = $("#Txtx_" + deptid + "_lengbeijizu").val();
                        var tj = (col1 * 1 + col2 * 1 + col3 * 1);
                        $(this).text(tj.toFixed(3));
                    } else if (attr == "tj2") {
                        var col1 = $("#Txtx_" + deptid + "_daxingshuqizhan").val();
                        var col2 = $("#Txtx_" + deptid + "_xiaoxingshuqizhan").val();
                        var tj = (col1 * 1 + col2 * 1);
                        $(this).text(tj.toFixed(3));
                    } else if (attr == "tj3") {
                        var col1 = $("#Txtx_" + deptid + "_gonglishu").val();
                        var col2 = $("#Txtx_" + deptid + "_dimao").val();
                        var col3 = $("#Txtx_" + deptid + "_chengzhenhua").val();
                        var tj = (col1 * 1 + col2 * 1 + col3 * 1);
                        $(this).text(tj.toFixed(3));
                        if (deptid == "104" || deptid == "122") {
                            $(this).text("0.000");
                        }
                        if (deptid == "153") {
                            $(this).text("0.005");
                        }
                    } else if (attr == "tj4") {
                        var col1 = $("#Txtx_" + deptid + "_weixiudui").val();
                        var tj = (col1 * 1) / 1;
                        $(this).text(tj.toFixed(3));
                    }
                });
                var strxs = "";
                $("input[id^='Txt_'][id$='_zjxs']").each(function (i) {
                    var strs = $(this).attr("id").split("_");
                    var deptid = strs[1];
                    var attr = strs[2];
                    var col0 = $("#Txtx_" + deptid + "_jiguan").val();
                    var col1 = $("#Lb_" + deptid + "_tj1").text();
                    var col2 = $("#Lb_" + deptid + "_tj2").text();
                    var col3 = $("#Lb_" + deptid + "_tj3").text();
                    var col4 = $("#Lb_" + deptid + "_tj4").text();
                    var tj = (col0 * 1 + col1 * 1 + col2 * 1 + col3 * 1 + col4 * 1) / 5;
                    var zj = 1 + tj;
                    $(this).val(tj.toFixed(3));
                    $("#Txt_" + deptid + "_xs").val(zj.toFixed(3));
                    strxs += deptid + ":" + zj.toFixed(3) + "|";
                });
                $("#strxs").val(strxs);
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
                    <th rowspan="2" colspan="2"> 系数因素 </th>
                    <th rowspan="2">机关</th>
                    <th colspan="4">压缩机</th>
                    <th colspan="3">输气站（注采站、采卤站等）</th>
                    <th colspan="5">巡线队</th>
                    <th colspan="2">维修队</th>
                    <th rowspan="2">总增加单位系数</th>
                    <th rowspan="2">考核系数</th>
                    <th rowspan="2">备注</th>
                 </tr>
                <tr class="GridViewHeaderStyle">
                    <th> 运行机组 </th>
                    <th>热备机组 </th>
                    <th>冷备机组</th>
                    <th>小计</th>
                    <th>大型输气站</th>
                    <th>小型输气站</th>
                    <th>小计</th>
                    <th>公里数</th>
                    <th>地貌</th>
                    <th>城镇化</th>
                    <th>分队</th>
                    <th>小计</th>
                    <th>维修队</th>
                    <th>小计</th>

                 </tr>
                <tr class="GridViewRowStyle" style="background-color:gray">
                    <td align="center" nowrap colspan="2">基础系数</td>
                    <td align="center" nowrap>1.000</td>
                    <td align="center" nowrap>1.000</td>
                    <td align="center" nowrap>1.000</td>
                    <td align="center" nowrap>1.000</td>
                    <td align="center" nowrap>1.000</td>
                    <td align="center" nowrap>1.000</td>
                    <td align="center" nowrap>1.000</td>
                    <td align="center" nowrap>1.000</td>
                    <td align="center" nowrap>1.000</td>
                    <td align="center" nowrap>1.000</td>
                    <td align="center" nowrap>1.000</td>
                    <td align="center" nowrap>-</td>
                    <td align="center" nowrap>1.000</td>
                    <td align="center" nowrap>1.000</td>
                    <td align="center" nowrap>1.000</td>
                    <td align="center" nowrap>-</td>
                    <td align="center" nowrap>-</td>
                    <td align="center" nowrap>-</td>
                </tr>
                <tr class="GridViewRowStyle" style="background-color:gray">
                    <td align="center" nowrap colspan="2">增加系数</td>
                    <td align="center" nowrap>0.000</td>
                    <td align="center" nowrap>0.050</td>
                    <td align="center" nowrap>0.030</td>
                    <td align="center" nowrap>0.020</td>
                    <td align="center" nowrap>-</td>
                    <td align="center" nowrap>0.050</td>
                    <td align="center" nowrap>0.030</td>
                    <td align="center" nowrap>-</td>
                    <td align="center" nowrap>0.010</td>
                    <td align="center" nowrap>0.01/0.02</td>
                    <td align="center" nowrap>0.01/0.02</td>
                    <td align="center" nowrap>-</td>
                    <td align="center" nowrap>-</td>
                    <td align="center" nowrap>0.03/0.05/0.07</td>
                    <td align="center" nowrap>-</td>
                    <td align="center" nowrap>-</td>
                    <td align="center" nowrap>-</td>
                    <td align="center" nowrap>-</td>
                </tr>

                <tr class="GridViewRowStyle" style="background-color:c">
                    <td align="center" nowrap rowspan="2">川渝</td>
                    <td align="center" nowrap>数量</td>
                    <td align="center" nowrap>
                        <asp:HiddenField ID="Hid_2" Value="2" runat="server"/>
                        <asp:TextBox ID="Txt_2_jiguan" ClientIDMode="Static" runat="server" class="easyui-numberbox" min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_2_yunxingjizu" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_2_rebeijizu" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_2_lengbeijizu" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_2_hj1" ClientIDMode="Static" runat="server" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_2_daxingshuqizhan" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_2_xiaoxingshuqizhan" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_2_hj2" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_2_gonglishu" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="99999999" precision="1" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:DropDownList ID="Ddl_2_dimao" runat="server" ClientIDMode="Static" >
                            <asp:ListItem Value="0.003">水网</asp:ListItem>
                            <asp:ListItem Value="0.005">混合地区</asp:ListItem>
                            <asp:ListItem Value="0.01">山区</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="center" nowrap>
                        <asp:DropDownList ID="Ddl_2_chengzhenhua" runat="server" ClientIDMode="Static" >
                            <asp:ListItem Value="0.003">中等</asp:ListItem>
                            <asp:ListItem Value="0.005">中等发达</asp:ListItem>
                            <asp:ListItem Value="0.01">发达</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_2_fendui" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap >
                        <%--<asp:Label ID="Lb_2_hj3" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>--%>
                    -
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_2_weixiudui" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_2_hj3" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>-</td>
                    <td align="center" nowrap>-</td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_2_bz" runat="server" class="easyui-validatebox" Width="60px" ></asp:TextBox>
                    </td>
                </tr>
                <tr class="GridViewRowStyle" style="background-color:c">
                    <td align="center" nowrap>单项增加</td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_2_jiguan" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_2_yunxingjizu" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_2_rebeijizu" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_2_lengbeijizu" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_2_tj1" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true" ></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_2_daxingshuqizhan" ClientIDMode="Static" runat="server" Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_2_xiaoxingshuqizhan" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_2_tj2" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_2_gonglishu" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_2_dimao" ClientIDMode="Static" runat="server"   Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_2_chengzhenhua" ClientIDMode="Static" runat="server" Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <%--<asp:TextBox ID="Txtx_2_fendui" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true"></asp:TextBox>--%>
                    -
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_2_tj3" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_2_weixiudui" ClientIDMode="Static" runat="server" Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_2_tj4" runat="server"  ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_2_zjxs" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_2_xs" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_2_bzx" runat="server" class="easyui-validatebox" Width="60px" ></asp:TextBox>
                    </td>
                </tr>
                <tr class="GridViewRowStyle" style="background-color:#eeeeee">
                    <td align="center" nowrap rowspan="2">鄂西</td>
                    <td align="center" nowrap>数量</td>
                    <td align="center" nowrap>
                        <asp:HiddenField ID="Hid_17" Value="2" runat="server"/>
                        <asp:TextBox ID="Txt_17_jiguan" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_17_yunxingjizu" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_17_rebeijizu" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_17_lengbeijizu" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_17_hj1" ClientIDMode="Static" runat="server" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_17_daxingshuqizhan" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_17_xiaoxingshuqizhan" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_17_hj2" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_17_gonglishu" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="99999999" precision="1" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:DropDownList ID="Ddl_17_dimao" runat="server" ClientIDMode="Static" >
                            <asp:ListItem Value="0.003">水网</asp:ListItem>
                            <asp:ListItem Value="0.005">混合地区</asp:ListItem>
                            <asp:ListItem Value="0.01">山区</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="center" nowrap>
                        <asp:DropDownList ID="Ddl_17_chengzhenhua" runat="server" ClientIDMode="Static" >
                            <asp:ListItem Value="0.003">中等</asp:ListItem>
                            <asp:ListItem Value="0.005">中等发达</asp:ListItem>
                            <asp:ListItem Value="0.01">发达</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_17_fendui" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap  >
                        <%--<asp:Label ID="Lb_17_hj3" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>--%>
                    -
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_17_weixiudui" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_17_hj3" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>-</td>
                    <td align="center" nowrap>-</td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_17_bz" runat="server" class="easyui-validatebox" Width="60px" ></asp:TextBox>
                    </td>
                </tr>
                <tr class="GridViewRowStyle" style="background-color:#eeeeee">
                    <td align="center" nowrap>单项增加</td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_17_jiguan" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_17_yunxingjizu" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_17_rebeijizu" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_17_lengbeijizu" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_17_tj1" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_17_daxingshuqizhan" ClientIDMode="Static" runat="server" Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_17_xiaoxingshuqizhan" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_17_tj2" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_17_gonglishu" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_17_dimao" ClientIDMode="Static" runat="server"   Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_17_chengzhenhua" ClientIDMode="Static" runat="server" Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <%--<asp:TextBox ID="Txtx_17_fendui" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true"></asp:TextBox>--%>
                    -
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_17_tj3" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_17_weixiudui" ClientIDMode="Static" runat="server" Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_17_tj4" runat="server"  ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_17_zjxs" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_17_xs" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_17_bzx" runat="server" class="easyui-validatebox" Width="60px" ></asp:TextBox>
                    </td>
                </tr>
                <tr class="GridViewRowStyle" style="background-color:c">
                    <td align="center" nowrap rowspan="2">鄂东</td>
                    <td align="center" nowrap>数量</td>
                    <td align="center" nowrap>
                        <asp:HiddenField ID="Hid_40" Value="2" runat="server"/>
                        <asp:TextBox ID="Txt_40_jiguan" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_40_yunxingjizu" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_40_rebeijizu" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_40_lengbeijizu" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_40_hj1" ClientIDMode="Static" runat="server" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_40_daxingshuqizhan" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_40_xiaoxingshuqizhan" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_40_hj2" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_40_gonglishu" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="99999999" precision="1" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:DropDownList ID="Ddl_40_dimao" runat="server" ClientIDMode="Static" >
                            <asp:ListItem Value="0.003">水网</asp:ListItem>
                            <asp:ListItem Value="0.005">混合地区</asp:ListItem>
                            <asp:ListItem Value="0.01">山区</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="center" nowrap>
                        <asp:DropDownList ID="Ddl_40_chengzhenhua" runat="server" ClientIDMode="Static" >
                            <asp:ListItem Value="0.003">中等</asp:ListItem>
                            <asp:ListItem Value="0.005">中等发达</asp:ListItem>
                            <asp:ListItem Value="0.01">发达</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_40_fendui" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap >
                        <%--<asp:Label ID="Lb_40_hj3" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>--%>
                    -
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_40_weixiudui" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_40_hj3" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>-</td>
                    <td align="center" nowrap>-</td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_40_bz" runat="server" class="easyui-validatebox" Width="60px" ></asp:TextBox>
                    </td>
                </tr>
                <tr class="GridViewRowStyle" style="background-color:c">
                    <td align="center" nowrap>单项增加</td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_40_jiguan" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_40_yunxingjizu" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_40_rebeijizu" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_40_lengbeijizu" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_40_tj1" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_40_daxingshuqizhan" ClientIDMode="Static" runat="server" Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_40_xiaoxingshuqizhan" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_40_tj2" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_40_gonglishu" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_40_dimao" ClientIDMode="Static" runat="server"   Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_40_chengzhenhua" ClientIDMode="Static" runat="server" Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <%--<asp:TextBox ID="Txtx_40_fendui" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true"></asp:TextBox>--%>
                    -
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_40_tj3" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_40_weixiudui" ClientIDMode="Static" runat="server" Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_40_tj4" runat="server"  ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_40_zjxs" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_40_xs" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_40_bzx" runat="server" class="easyui-validatebox" Width="60px" ></asp:TextBox>
                    </td>
                </tr>
                <tr class="GridViewRowStyle" style="background-color:#eeeeee">
                    <td align="center" nowrap rowspan="2">安徽</td>
                    <td align="center" nowrap>数量</td>
                    <td align="center" nowrap>
                        <asp:HiddenField ID="Hid_63" Value="2" runat="server"/>
                        <asp:TextBox ID="Txt_63_jiguan" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txt_63_yunxingjizu" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txt_63_rebeijizu" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txt_63_lengbeijizu" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:Label ID="Lb_63_hj1" ClientIDMode="Static" runat="server" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_63_daxingshuqizhan" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_63_xiaoxingshuqizhan" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_63_hj2" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_63_gonglishu" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="99999999" precision="1" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:DropDownList ID="Ddl_63_dimao" runat="server" ClientIDMode="Static" >
                            <asp:ListItem Value="0.003">水网</asp:ListItem>
                            <asp:ListItem Value="0.005">混合地区</asp:ListItem>
                            <asp:ListItem Value="0.01">山区</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="center" nowrap>
                        <asp:DropDownList ID="Ddl_63_chengzhenhua" runat="server" ClientIDMode="Static" >
                            <asp:ListItem Value="0.003">中等</asp:ListItem>
                            <asp:ListItem Value="0.005">中等发达</asp:ListItem>
                            <asp:ListItem Value="0.01">发达</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_63_fendui" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap >
                        <%--<asp:Label ID="Lb_63_hj3" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>--%>
                    -
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_63_weixiudui" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_63_hj3" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>-</td>
                    <td align="center" nowrap>-</td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_63_bz" runat="server" class="easyui-validatebox" Width="60px" ></asp:TextBox>
                    </td>
                </tr>
                <tr class="GridViewRowStyle" style="background-color:#eeeeee">
                    <td align="center" nowrap>单项增加</td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_63_jiguan" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txtx_63_yunxingjizu" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txtx_63_rebeijizu" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txtx_63_lengbeijizu" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_63_tj1" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_63_daxingshuqizhan" ClientIDMode="Static" runat="server" Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_63_xiaoxingshuqizhan" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_63_tj2" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_63_gonglishu" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_63_dimao" ClientIDMode="Static" runat="server"   Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_63_chengzhenhua" ClientIDMode="Static" runat="server" Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap >
                        <%--<asp:TextBox ID="Txtx_63_fendui" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true"></asp:TextBox>--%>
                    -
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_63_tj3" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_63_weixiudui" ClientIDMode="Static" runat="server" Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_63_tj4" runat="server"  ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_63_zjxs" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_63_xs" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_63_bzx" runat="server" class="easyui-validatebox" Width="60px" ></asp:TextBox>
                    </td>
                </tr>
                <tr class="GridViewRowStyle" style="background-color:c">
                    <td align="center" nowrap rowspan="2">浙沪</td>
                    <td align="center" nowrap>数量</td>
                    <td align="center" nowrap>
                        <asp:HiddenField ID="Hid_164" Value="2" runat="server"/>
                        <asp:TextBox ID="Txt_164_jiguan" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40"  ></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txt_164_yunxingjizu" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txt_164_rebeijizu" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txt_164_lengbeijizu" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:Label ID="Lb_164_hj1" ClientIDMode="Static" runat="server" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_164_daxingshuqizhan" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_164_xiaoxingshuqizhan" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_164_hj2" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_164_gonglishu" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="99999999" precision="1" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:DropDownList ID="Ddl_164_dimao" runat="server" ClientIDMode="Static" >
                            <asp:ListItem Value="0.003">水网</asp:ListItem>
                            <asp:ListItem Value="0.005">混合地区</asp:ListItem>
                            <asp:ListItem Value="0.01">山区</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="center" nowrap>
                        <asp:DropDownList ID="Ddl_164_chengzhenhua" runat="server" ClientIDMode="Static" >
                            <asp:ListItem Value="0.003">中等</asp:ListItem>
                            <asp:ListItem Value="0.005">中等发达</asp:ListItem>
                            <asp:ListItem Value="0.01">发达</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_164_fendui" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap >
                        <%--<asp:Label ID="Lb_164_hj3" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>--%>
                    -
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_164_weixiudui" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_164_hj3" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>-</td>
                    <td align="center" nowrap>-</td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_164_bz" runat="server" class="easyui-validatebox" Width="60px" ></asp:TextBox>
                    </td>
                </tr>
                <tr class="GridViewRowStyle" style="background-color:c">
                    <td align="center" nowrap>单项增加</td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_164_jiguan" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txtx_164_yunxingjizu" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txtx_164_rebeijizu" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txtx_164_lengbeijizu" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_164_tj1" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_164_daxingshuqizhan" ClientIDMode="Static" runat="server" Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_164_xiaoxingshuqizhan" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_164_tj2" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_164_gonglishu" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_164_dimao" ClientIDMode="Static" runat="server"   Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_164_chengzhenhua" ClientIDMode="Static" runat="server" Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap >
                        <%--<asp:TextBox ID="Txtx_164_fendui" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true"></asp:TextBox>--%>
                    -
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_164_tj3" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_164_weixiudui" ClientIDMode="Static" runat="server" Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_164_tj4" runat="server"  ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_164_zjxs" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_164_xs" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_164_bzx" runat="server" class="easyui-validatebox" Width="60px" ></asp:TextBox>
                    </td>
                </tr>
                <tr class="GridViewRowStyle" style="background-color:#eeeeee">
                    <td align="center" nowrap rowspan="2">江苏</td>
                    <td align="center" nowrap>数量</td>
                    <td align="center" nowrap>
                        <asp:HiddenField ID="Hid_82" Value="2" runat="server"/>
                        <asp:TextBox ID="Txt_82_jiguan" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40"  ></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txt_82_yunxingjizu" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txt_82_rebeijizu" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txt_82_lengbeijizu" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:Label ID="Lb_82_hj1" ClientIDMode="Static" runat="server" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_82_daxingshuqizhan" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_82_xiaoxingshuqizhan" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_82_hj2" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_82_gonglishu" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="99999999" precision="1" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:DropDownList ID="Ddl_82_dimao" runat="server" ClientIDMode="Static" >
                            <asp:ListItem Value="0.003">水网</asp:ListItem>
                            <asp:ListItem Value="0.005">混合地区</asp:ListItem>
                            <asp:ListItem Value="0.01">山区</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="center" nowrap>
                        <asp:DropDownList ID="Ddl_82_chengzhenhua" runat="server" ClientIDMode="Static" >
                            <asp:ListItem Value="0.003">中等</asp:ListItem>
                            <asp:ListItem Value="0.005">中等发达</asp:ListItem>
                            <asp:ListItem Value="0.01">发达</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_82_fendui" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap >
                        <%--<asp:Label ID="Lb_82_hj3" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>--%>
                    -
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_82_weixiudui" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_82_hj3" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>-</td>
                    <td align="center" nowrap>-</td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_82_bz" runat="server" class="easyui-validatebox" Width="60px" ></asp:TextBox>
                    </td>
                </tr>
                <tr class="GridViewRowStyle" style="background-color:#eeeeee">
                    <td align="center" nowrap>单项增加</td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_82_jiguan" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txtx_82_yunxingjizu" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txtx_82_rebeijizu" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txtx_82_lengbeijizu" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_82_tj1" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_82_daxingshuqizhan" ClientIDMode="Static" runat="server" Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_82_xiaoxingshuqizhan" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_82_tj2" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_82_gonglishu" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_82_dimao" ClientIDMode="Static" runat="server"   Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_82_chengzhenhua" ClientIDMode="Static" runat="server" Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <%--<asp:TextBox ID="Txtx_82_fendui" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true"></asp:TextBox>--%>
                    -
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_82_tj3" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_82_weixiudui" ClientIDMode="Static" runat="server" Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_82_tj4" runat="server"  ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true" ></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_82_zjxs" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_82_xs" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_82_bzx" runat="server" class="easyui-validatebox" Width="60px" ></asp:TextBox>
                    </td>
                </tr>
                <tr class="GridViewRowStyle" style="background-color:c">
                    <td align="center" nowrap rowspan="2">宜昌</td>
                    <td align="center" nowrap>数量</td>
                    <td align="center" nowrap>
                        <asp:HiddenField ID="Hid_104" Value="2" runat="server"/>
                        <asp:TextBox ID="Txt_104_jiguan" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40"  ></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txt_104_yunxingjizu" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txt_104_rebeijizu" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txt_104_lengbeijizu" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:Label ID="Lb_104_hj1" ClientIDMode="Static" runat="server" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txt_104_daxingshuqizhan" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0"  Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txt_104_xiaoxingshuqizhan" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:Label ID="Lb_104_hj2" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txt_104_gonglishu" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="99999999" precision="1" Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:DropDownList ID="Ddl_104_dimao" runat="server" ClientIDMode="Static" Enabled="false" BackColor="Gray">
                            <asp:ListItem Value="0">无</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:DropDownList ID="Ddl_104_chengzhenhua" runat="server" ClientIDMode="Static" Enabled="false" BackColor="Gray">
                            <asp:ListItem Value="0">无</asp:ListItem>

                        </asp:DropDownList>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txt_104_fendui" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap >
                        <%--<asp:Label ID="Lb_104_hj3" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>--%>
                    -
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_104_weixiudui" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_104_hj3" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>-</td>
                    <td align="center" nowrap>-</td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_104_bz" runat="server" class="easyui-validatebox" Width="60px" ></asp:TextBox>
                    </td>
                </tr>
                <tr class="GridViewRowStyle" style="background-color:c">
                    <td align="center" nowrap>单项增加</td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_104_jiguan" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txtx_104_yunxingjizu" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txtx_104_rebeijizu" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txtx_104_lengbeijizu" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_104_tj1" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txtx_104_daxingshuqizhan" ClientIDMode="Static" runat="server" Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txtx_104_xiaoxingshuqizhan" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_104_tj2" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txtx_104_gonglishu" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txtx_104_dimao" ClientIDMode="Static" runat="server"   Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txtx_104_chengzhenhua" ClientIDMode="Static" runat="server" Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <%--<asp:TextBox ID="Txtx_104_fendui" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true"></asp:TextBox>--%>
                    -
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_104_tj3" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_104_weixiudui" ClientIDMode="Static" runat="server" Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_104_tj4" runat="server"  ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_104_zjxs" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_104_xs" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_104_bzx" runat="server" class="easyui-validatebox" Width="60px" ></asp:TextBox>
                    </td>
                </tr>
                <tr class="GridViewRowStyle" style="background-color:#eeeeee">
                    <td align="center" nowrap rowspan="2">嘉兴</td>
                    <td align="center" nowrap>数量</td>
                    <td align="center" nowrap>
                        <asp:HiddenField ID="Hid_122" Value="2" runat="server"/>
                        <asp:TextBox ID="Txt_122_jiguan" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40"  ></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txt_122_yunxingjizu" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txt_122_rebeijizu" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txt_122_lengbeijizu" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:Label ID="Lb_122_hj1" ClientIDMode="Static" runat="server" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txt_122_daxingshuqizhan" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0"  Width="40"  ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txt_122_xiaoxingshuqizhan" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0"   Width="40"  ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:Label ID="Lb_122_hj2" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txt_122_gonglishu" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="99999999" precision="1" Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:DropDownList ID="Ddl_122_dimao" runat="server" ClientIDMode="Static" Enabled="false" BackColor="Gray">
                            <asp:ListItem Value="0">无</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:DropDownList ID="Ddl_122_chengzhenhua" runat="server" ClientIDMode="Static" Enabled="false" BackColor="Gray">
                            <asp:ListItem Value="0">无</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txt_122_fendui" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap >
                        <%--<asp:Label ID="Lb_122_hj3" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>--%>
                    -
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_122_weixiudui" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_122_hj3" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>-</td>
                    <td align="center" nowrap>-</td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_122_bz" runat="server" class="easyui-validatebox" Width="60px" ></asp:TextBox>
                    </td>
                </tr>
                <tr class="GridViewRowStyle" style="background-color:#eeeeee">
                    <td align="center" nowrap>单项增加</td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_122_jiguan" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txtx_122_yunxingjizu" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txtx_122_rebeijizu" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txtx_122_lengbeijizu" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_122_tj1" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txtx_122_daxingshuqizhan" ClientIDMode="Static" runat="server" Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txtx_122_xiaoxingshuqizhan" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap >
                        <asp:Label ID="Lb_122_tj2" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txtx_122_gonglishu" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txtx_122_dimao" ClientIDMode="Static" runat="server"   Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txtx_122_chengzhenhua" ClientIDMode="Static" runat="server" Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <%--<asp:TextBox ID="Txtx_122_fendui" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true"></asp:TextBox>--%>
                    -
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_122_tj3" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_122_weixiudui" ClientIDMode="Static" runat="server" Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_122_tj4" runat="server"  ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_122_zjxs" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_122_xs" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_122_bzx" runat="server" class="easyui-validatebox" Width="60px" ></asp:TextBox>
                    </td>
                </tr>
                <tr class="GridViewRowStyle" style="background-color:c">
                    <td align="center" nowrap rowspan="2">金坛</td>
                    <td align="center" nowrap>数量</td>
                    <td align="center" nowrap>
                        <asp:HiddenField ID="Hid_153" Value="2" runat="server"/>
                        <asp:TextBox ID="Txt_153_jiguan" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_153_yunxingjizu" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_153_rebeijizu" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_153_lengbeijizu" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_153_hj1" ClientIDMode="Static" runat="server" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_153_daxingshuqizhan" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_153_xiaoxingshuqizhan" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:Label ID="Lb_153_hj2" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txt_153_gonglishu" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="99999999" precision="1" missingMessage="必须填写数字" Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:DropDownList ID="Ddl_153_dimao" runat="server" ClientIDMode="Static" Enabled="false" BackColor="Gray">
                            <asp:ListItem Value="0">无</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:DropDownList ID="Ddl_153_chengzhenhua" runat="server" ClientIDMode="Static" Enabled="false" BackColor="Gray">
                            <asp:ListItem Value="0">无</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txt_153_fendui" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap >
                        <%--<asp:Label ID="Lb_153_hj3" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>--%>
                    -
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_153_weixiudui" ClientIDMode="Static" runat="server" class="easyui-numberbox"  min="0"
                                    max="10000" precision="0" missingMessage="必须填写数字" Width="40" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_153_hj3" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>-</td>
                    <td align="center" nowrap>-</td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_153_bz" runat="server" class="easyui-validatebox" Width="60px" ></asp:TextBox>
                    </td>
                </tr>
                <tr class="GridViewRowStyle" style="background-color:c">
                    <td align="center" nowrap>单项增加</td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_153_jiguan" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_153_yunxingjizu" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_153_rebeijizu" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_153_lengbeijizu" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_153_tj1" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_153_daxingshuqizhan" ClientIDMode="Static" runat="server" Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_153_xiaoxingshuqizhan" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_153_tj2" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txtx_153_gonglishu" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txtx_153_dimao" ClientIDMode="Static" runat="server"   Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <asp:TextBox ID="Txtx_153_chengzhenhua" ClientIDMode="Static" runat="server" Width="40" ReadOnly="true" BackColor="Gray"></asp:TextBox>
                    </td>
                    <td align="center" nowrap style="background-color:gray">
                        <%--<asp:TextBox ID="Txtx_153_fendui" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true"></asp:TextBox>--%>
                    -
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_153_tj3" runat="server" ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txtx_153_weixiudui" ClientIDMode="Static" runat="server" Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:Label ID="Lb_153_tj4" runat="server"  ClientIDMode="Static" Text="" ForeColor="Black" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_153_zjxs" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_153_xs" ClientIDMode="Static" runat="server"  Width="40" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td align="center" nowrap>
                        <asp:TextBox ID="Txt_153_bzx" runat="server" class="easyui-validatebox" Width="60px" ></asp:TextBox>
                    </td>
                </tr>
            </table>
            <%--<asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand"
                AllowPaging="false" DataKeyNames="SXID,SXZBBM,ZRSZBID" ShowFooter="true">
                <Columns>
                    <asp:TemplateField HeaderText="选择">
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="考核指标类型">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="180px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="考核主要内容">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="指标性质" Visible="false">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="权重%" Visible="false">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="分值">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="计算关系式" Visible="false">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="指标代号" Visible="false">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="显示序号">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="基准分值" Visible="false">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="GridViewHeaderStyle" />
                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                <RowStyle CssClass="GridViewRowStyle" />
                <FooterStyle HorizontalAlign="Center" Height="30px" />
            </asp:GridView>--%>
            <%--<div class="msg-info">
                <div class="msg-tip icon-tip"></div>
                <div>
                    <asp:Label ID="Lbl_Msg" runat="server" Text="" ForeColor="Red"></asp:Label>&nbsp;</div>
            </div>--%>
<%--            <div id="contents" class="ssec-form">
                <div class="ssec-group ssec-group-hasicon">
                    <div class="icon-form"></div>
                    <span>编辑类型权重</span>
                </div>
                <%=GetTableForLhzb() %>
            </div>--%>
            <%--<div class="btn-bgcolor">
                <asp:LinkButton ID="LinkButton1" runat="server" class="easyui-linkbutton" iconCls="icon-ok"
                    OnClientClick="Heji();return false;">计算</asp:LinkButton>
                <asp:LinkButton ID="LnkBtn_Ins" runat="server" class="easyui-linkbutton" iconCls="icon-save"
                    OnClientClick="return checkForm();" OnClick="LnkBtn_Ins_Click">保存</asp:LinkButton>
            </div>--%>
        </div>
    </div>
</asp:Content>
