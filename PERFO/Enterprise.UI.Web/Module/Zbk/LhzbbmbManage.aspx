<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LhzbbmbManage.aspx.cs" ValidateRequest="false"
    Inherits="Enterprise.UI.Web.Zbk.LhzbbmbManage" %>

<%@ Register Assembly="Enterprise.Component.Infrastructure" Namespace="Enterprise.Component.Infrastructure" TagPrefix="cc1" %>

<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>量化指标维护</title>
    <link rel="stylesheet" type="text/css" href="/Resources/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/Resources/themes/icon.css" />
    <link rel="stylesheet" type="text/css" href="/Resources/Css/Pstyle.css" />
    <style type="text/css">
        .LhzbTable {
            font-family: Times New Roman;
            border-style: dotted;
            border-color: #808080;
            font-size: 12px;
            border-top-width: 1px;
            border-right-width: 1px;
            border-bottom-width: 1px;
            border-left-width: 1px;
            border-collapse: collapse;
        }
    </style>
    <script type="text/javascript" src="/Resources/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="/Resources/Scripts/jquery.easyui.min.js"></script>
    <script type="text/javascript">
        function showWin() {
            $('#w').window('open');
        }
        function checkform() {
            var bds = encodeURIComponent($('#Txt_BDS').val());
            $('#Hid_JSBDS').val(bds);
            //alert($('#Txt_BDS').val());
            return $("#form1").form('validate');
        }
        function aa() {
            if (confirm('您确定要删除数据？')) {
                return true;
            }
            else {
                return false;
            }
        }
        function bb() {
            if (checkform() &&
                confirm('请注意:添加操作是在选择节点的下面添加子节点！您确定操作吗？')) {
                return true;
            }
            else {
                return false;
            }
        }
        //查找页面元素并定位
        function findZbmc() {
            var aObjs = $("a[class^='TreeView1']");
            var txtObj = $("#Sel_Zbmc");
            aObjs.each(function () {
                var obj = $(this);
                if (txtObj.val() != "" && obj.text().indexOf(txtObj.val()) > -1) {
                    //alert(obj.text());
                    obj.css("background-color", "Yellow");
                    $("html,body").animate({ scrollTop: obj.offset().top - 20 }, 1500); //1000是ms,也可以用slow代替
                    return true;
                }
                else {
                    obj.css("background-color", "White");
                }
            });
            return false;
        }
        var mc = "";
        $(function () {
            $('#tt').datagrid({
                url: '/Module/Zbk/WebDataHandler.ashx?zblx=01',
                width: 'auto',
                height: 'auto',
                fitColumns: true,
                singleSelect: true,
                columns: [[
					{ field: 'ZBID', title: 'ID', width: 50 },
					{ field: 'ZBMC', title: '指标名称', width: 180 },
					{ field: 'YJZBMC', title: '一级分类', width: 80 },
                    { field: 'EJZBMC', title: '二级分类', width: 80 },
                    { field: 'SJZBMC', title: '三级分类', width: 80 }
                ]],
                onDblClickRow: function (index, row) {
                    var selected = $('#tt').datagrid('getSelected');
                    if (selected) {
                        $('#w').window('close');
                        $('#Txt_ZBMC').val(selected.ZBMC);
                        $('#Hid_ZBID').val(selected.ZBID);
                        $('#Txt_JSDW').val("");
                        $('#Ddl_GZID').val("");
                        $('#Txt_ZBDH').val("");
                        $('#Txt_BJQZ').val("");
                        $('#Txt_BDS').val("");
                        $('#Txt_JZFS').val("");  
                    }
                }
            });
        });
        function qq(value) {
            mc = value;
            $('#tt').datagrid('options').url = "/Module/Zbk/WebDataHandler.ashx?zblx=01&zbmc=" + encodeURIComponent(mc);
            //alert($('#tt').datagrid('options').url);
            $('#tt').datagrid('reload');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="p" class="easyui-panel" title="量化指标维护" icon="icon-save" collapsible="true"
            style="padding: 10px; background: #fafafa;">
            <div>
                <input id="Sel_Zbmc" type="text" style="width: 240px" />&nbsp;&nbsp;
            <a href="#" class="easyui-linkbutton" iconcls="icon-search" onclick="return findZbmc();return false;">查找</a>&nbsp;&nbsp;
                <asp:Label ID="Lbl_Msg" runat="server" ForeColor="Red"></asp:Label>
                <hr />
                被考核单位：
                <cc1:SSECDropDownList ID="Ddl_Danwei" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_Danwei_SelectedIndexChanged">
                </cc1:SSECDropDownList>&nbsp;&nbsp;
            指标版本：<asp:DropDownList ID="Ddl_BBMC" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_BBMC_SelectedIndexChanged">
            </asp:DropDownList>&nbsp;&nbsp;
            <asp:CheckBox ID="Chk_ShowAll" runat="server" AutoPostBack="true"
                OnCheckedChanged="Chk_ShowAll_CheckedChanged" Text="显示全部" />
                &nbsp;&nbsp;<asp:LinkButton ID="LnkBtn_PX" runat="server" OnClick="LnkBtn_PX_Click" class="easyui-linkbutton" 
                    iconcls="icon-xtgl" plain="true" OnClientClick="return (confirm('您确定要全部重新排序吗？'));">重新排序</asp:LinkButton>
            </div>
            <hr />
            <table width="auto" border="0" cellspacing="5">
                <tr>
                    <td valign="top" width="260px">
                        <asp:TreeView ID="TreeView1" runat="server" ShowLines="true" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged">
                            <RootNodeStyle HorizontalPadding="3px" ImageUrl="/Resources/Images/tree/folder_plus.png" />
                            <Nodes>
                            </Nodes>
                            <ParentNodeStyle HorizontalPadding="3px" ImageUrl="/Resources/Images/tree/folder.png" />
                            <SelectedNodeStyle ImageUrl="/Resources/Images/tree/folder_open.png" CssClass="tabs-header" />
                            <LeafNodeStyle HorizontalPadding="3px" ImageUrl="/Resources/Images/tree/folder.png" />
                        </asp:TreeView>
                    </td>
                    <td valign="top">
                        <asp:HiddenField ID="Hid_LHZBBM" runat="server" />
                        <table class="LhzbTable" cellpadding="1" border="1">
                            <tr>
                                <td width="120px">所属上级指标
                                </td>
                                <td>
                                    <asp:Label ID="Lbl_PARENT" runat="server" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                    <asp:HiddenField ID="Hid_PARENTZBBM" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>指标名称
                                </td>
                                <td>
                                    <asp:TextBox ID="Txt_ZBMC" runat="server" Width="200px" ReadOnly="true"></asp:TextBox>
                                    <asp:HiddenField ID="Hid_ZBID" runat="server" />
                                    <a href="#" onclick="showWin();">
                                        <img alt="" src="/Resources/themes/icons/pencil.png" border="0" />选择指标</a>
                                </td>
                            </tr>
                            <tr>
                                <td>关联机构
                                </td>
                                <td>
                                    <input id="Rd_GLJG" type="radio" runat="server" name="RdGroup" onclick="javascript: $('#TR_GLJG').show(); $('#TR_DFJG').hide();" />
                                    <a style="text-decoration-line: underline;" onclick="javascript: $('#TR_GLJG').show(); $('#TR_DFJG').hide();$('#Rd_GLJG').attr('checked','checked');">关联机构设置</a>
                                    &nbsp;&nbsp;
                                    <input id="Rd_DFJG" type="radio" runat="server" name="RdGroup" onclick="javascript: $('#TR_DFJG').show(); $('#TR_GLJG').hide();" />
                                    <a style="text-decoration-line: underline;" onclick="javascript: $('#TR_DFJG').show(); $('#TR_GLJG').hide();$('#Rd_DFJG').attr('checked','checked');">打分机构设置</a>
                                    &nbsp;&nbsp;
                                    (点击展开︾)
                                </td>
                            </tr>
                            <tr id="TR_GLJG" style="display: none">
                                <td colspan="2">
                                    <asp:Panel ID="Pnl_GLJG" runat="server">
                                        <table cellpadding="2">
                                            <tr>
                                                <td>目标值填报机构
                                                </td>
                                                <td>目标值审核机构
                                                </td>
                                                <td>完成值填报机构
                                                </td>
                                                <td>完成值审核机构
                                                </td>
                                            </tr>
                                            <tr valign="top">
                                                <td>
                                                    <asp:CheckBoxList ID="cb_mb1" runat="server">
                                                    </asp:CheckBoxList>
                                                </td>
                                                <td>
                                                    <asp:CheckBoxList ID="cb_mb2" runat="server">
                                                    </asp:CheckBoxList>
                                                </td>
                                                <td>
                                                    <asp:CheckBoxList ID="cb_wc1" runat="server">
                                                    </asp:CheckBoxList>
                                                </td>
                                                <td>
                                                    <asp:CheckBoxList ID="cb_wc2" runat="server">
                                                    </asp:CheckBoxList>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr id="TR_DFJG" style="display: none">
                                <td colspan="2">
                                    <asp:Panel ID="Pnl_DFJG" runat="server">
                                        <table cellpadding="2">
                                            <tr>
                                                <td>被打分机构</td>
                                                <td>打分者</td>
                                            </tr>
                                            <tr valign="top">
                                                <td>
                                                    <asp:CheckBoxList ID="cb_bdfjg" runat="server">
                                                    </asp:CheckBoxList>
                                                </td>
                                                <td>
                                                    <asp:CheckBoxList ID="cb_dfz" runat="server">
                                                    </asp:CheckBoxList>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>指标说明
                                </td>
                                <td>
                                    <%--<FCKeditorV2:FCKeditor ID="Txt_ZBSM" runat="server" Width="400px" Height="110px" ToolbarSet="Basic">
                                    </FCKeditorV2:FCKeditor>--%>
                                    <asp:TextBox ID="Txt_ZBSM" runat="server" Width="400px" Height="110px" TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>评分标准
                                </td>
                                <td>
                                    <%--<FCKeditorV2:FCKeditor ID="Txt_PFBZ" runat="server" Width="400px" Height="110px" ToolbarSet="Basic">
                                    </FCKeditorV2:FCKeditor>--%>
                                    <asp:TextBox ID="Txt_PFBZ" runat="server" Width="400px" Height="110px" TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>计算单位
                                </td>
                                <td>
                                    <asp:TextBox ID="Txt_JSDW" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>计算规则
                                </td>
                                <td>
                                    <asp:DropDownList ID="Ddl_GZID" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>禁用</td>
                                <td>
                                    <asp:CheckBox ID="Chk_SFJY" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>指标代号</td>
                                <td>
                                    <asp:TextBox ID="Txt_ZBDH" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>辅助指标</td>
                                <td>
                                    <asp:CheckBox ID="Chk_FZZB" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>指标权重（%）</td>
                                <td>
                                    <asp:TextBox ID="Txt_BJQZ" runat="server" class="easyui-numberbox" required="true"
                                        min="0.0" max="100.0" precision="1" missingMessage="只能填数字0~100之间"></asp:TextBox>范围：(<font color="red">0-100</font>)
                                </td>
                            </tr>
                            <tr>
                                <td>计算关系说明</td>
                                <td>
                                    <%--<FCKeditorV2:FCKeditor ID="Txt_JSMS" runat="server" Width="400px" Height="110px" ToolbarSet="Basic">
                                    </FCKeditorV2:FCKeditor>--%>
                                    <asp:TextBox ID="Txt_JSMS" runat="server" Width="400px" Height="110px" TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>计算关系表达式</td>
                                <td>
                                    <asp:TextBox ID="Txt_BDS" runat="server" Width="400px" Height="60px" TextMode="MultiLine"></asp:TextBox>
                                    <asp:HiddenField ID="Hid_JSBDS" runat="server" />
                                    <br />
                                    <font color="red">注：以“{}”标识指标项。上报完成值时可录入指标数据</font>
                                </td>
                            </tr>
                            <tr>
                                <td>是否可取上年数据<br />
                                    为目标值</td>
                                <td>
                                    <asp:CheckBox ID="Chk_ISMBZ" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>基准分数</td>
                                <td>
                                    <asp:TextBox ID="Txt_JZFS" runat="server" class="easyui-numberbox" min="0" max="999" precision="2"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <asp:LinkButton ID="LnkBtn_Ins" runat="server" class="easyui-linkbutton" iconCls="icon-add"
                            OnClientClick="return bb();" OnClick="LnkBtn_Ins_Click">添加</asp:LinkButton>
                        <asp:LinkButton ID="LnkBtn_Upd" runat="server" class="easyui-linkbutton" iconCls="icon-save"
                            OnClientClick="return checkform();" OnClick="LnkBtn_Upd_Click">修改</asp:LinkButton>
                        <asp:LinkButton ID="LnkBtn_Del" runat="server" class="easyui-linkbutton" iconCls="icon-remove"
                            OnClientClick="return aa();" OnClick="LnkBtn_Del_Click">删除</asp:LinkButton>
                        <asp:LinkButton ID="LnkBtn_Up" runat="server" class="easyui-linkbutton" iconCls="icon-up"
                            OnClick="LnkBtn_Up_Click" >上调</asp:LinkButton>
                        <asp:LinkButton ID="LnkBtn_Down" runat="server" class="easyui-linkbutton" iconCls="icon-down"
                            OnClick="LnkBtn_Down_Click"  >下调</asp:LinkButton>
                        <asp:LinkButton ID="LnkBtn_Cancel" runat="server" class="easyui-linkbutton" iconCls="icon-back"
                            OnClick="LnkBtn_Cancel_Click">取消</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
        <%-- 指标选择窗口 --%>
        <div id="w" class="easyui-window" title="指标(双击选择)" iconcls="icon-add" style="width: 500px; height: 350px; padding: 5px;" closed="true" modal="true" maximizable="false" minimizable="false" collapsible="false" resizable="false" inline="false">
            <input id="ss" class="easyui-searchbox" searcher="qq" prompt="请输入指标名称" style="width: 300px" />
            <table id="tt"></table>
        </div>
    </form>
</body>
</html>
