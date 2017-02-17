<%@ Page Title="" Language="C#" MasterPageFile="../Project.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="EjdwWczShenhe.aspx.cs" Inherits="Enterprise.UI.Web.Kh.EjdwWczShenhe" %>

<%@ Import Namespace="Enterprise.Model.Perfo.Kh" %>
<%@ Import Namespace="Enterprise.Component.Infrastructure" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function checkForm() {
            return $("#form1").form('validate');
        }
        function showInfo(tt, msg) {
            $.messager.show({
                title: tt,
                msg: msg,
                height: '270',
                width:'400',
                showType: 'show',
                timeout: 30000,
            });
        }
        function calDefen() {
            //var v = "";
            //if (confirm('您需要重新计算所有指标得分吗?(‘否’则只重新计算主指标得分!)')) {
            //    v = "1";
            //}
            //$('#<%//=Hid_Recount.ClientID%>').val(v);
            showLoading();
            return true;
        }
        function showLoading() {
            var win = $.messager.progress({
                title: '请您稍侯',
                msg: '正在提交数据...'
            });
            setTimeout(function () {
                $.messager.progress('close');
            }, 10000)
        }
        function selectAll(obj) {
            var inputs = $(":input[type='checkbox']");
            if (obj.value == "全部选择") {
                inputs.each(function () {
                    $(this).attr("checked", true);
                });
                obj.value = "取消选择";
            }
            else {
                inputs.each(function () {
                    $(this).attr("checked", false);
                });
                obj.value = "全部选择";
            }
        }
        function openJcsj() {
            var param = '<%=UrlParam %>';
            parent.addTab('/M.K.EjdwJcsjData?' + param, '基础数据表');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ProjectPH" runat="server">
    <div data-options="region:'center'">
        <div id="Div1" class="ssec-form">
            <div class="ssec-group ssec-group-hasicon">
                <div class="icon-form"></div>
                <span>二级单位考核完成值审核</span>
            </div>
        </div>
        <div class="main-gridview">
            <div class="main-gridview-title">
                考核期：<Ent:SSECDropDownList ID="Ddl_Kaohe" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_Kaohe_SelectedIndexChanged">
                </Ent:SSECDropDownList>
                &nbsp;&nbsp;
                单位：<Ent:SSECDropDownList ID="Ddl_Danwei" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_Danwei_SelectedIndexChanged">
                </Ent:SSECDropDownList>
                &nbsp;&nbsp;
                <asp:LinkButton ID="Btn_Add" runat="server" Text="审核完成值" class="easyui-linkbutton" plain="true"
                    iconCls="icon-shenhe" OnClick="Btn_Add_Click"></asp:LinkButton>
                <%--&nbsp;&nbsp;
                <asp:LinkButton ID="Btn_Upd" runat="server" Text="审核基础数据" class="easyui-linkbutton" plain="true"
                    iconCls="icon-calendar" OnClick="Btn_Upd_Click"></asp:LinkButton>--%>
                &nbsp;&nbsp;
                <a href="javascript:openJcsj();" 
                    iconCls="icon-calendar" class="easyui-linkbutton" plain="true" runat="server" id="Btn_Jcsj">审核基础数据</a>
                <asp:HiddenField ID="Hid_TabTitle" runat="server" />
            </div>
            <div id="tt" class="easyui-tabs" style="width: auto;">
                <div title="考核指标" data-options="iconCls:'icon-yunxing'" style="padding: 4px; overflow-y: hidden;">
                    <div class="main-gridview">
                        <asp:HiddenField ID="Hid_Recount" runat="server" />
                        <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                            OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand"
                            AllowPaging="false" DataKeyNames="ID">
                            <Columns>
                                <asp:TemplateField HeaderText="序号">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Chk_XH" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="指标类别">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="指标名称">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="指标性质及权重">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="考核目标值">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="上报完成值">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="完成值说明">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="审核完成值">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="审核值说明">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="本项实际得分">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="操作权限">
                                    <ItemTemplate>
                                        <asp:Image ID="Image1" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                            <RowStyle CssClass="GridViewRowStyle" />
                        </asp:GridView>
                    </div>
                </div>
                <div title="关联指标" data-options="iconCls:'icon-xlsx'" style="padding: 4px">
                    <asp:Panel ID="Pnl_EditGlzb" runat="server" Visible="false">
                        <div style="background:#C9EDCC;padding:5px;width:auto;">
                            <asp:LinkButton ID="LnkBtn_GetData" runat="server" class="easyui-linkbutton" 
                                iconCls="icon-sum" OnClick="LnkBtn_GetData_Click">获取基础表数据</asp:LinkButton>
	                    </div>
                    </asp:Panel>
                    <div class="main-gridview">
                        <asp:GridView ID="GridView2" Width="100%" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                            OnRowDataBound="GridView2_RowDataBound" AllowPaging="false" DataKeyNames="ID">
                            <Columns>
                                <asp:TemplateField HeaderText="序号">
                                    <ItemTemplate>
                                        <%#(Container.DataItemIndex + 1) %>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="指标类别" Visible="false">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="指标名称">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="计算关系式和数据">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="计算完成值">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                            <RowStyle CssClass="GridViewRowStyle" />
                        </asp:GridView>
                    </div>
                </div>
                <div title="我审核的单位" data-options="iconCls:'icon-search'" style="padding: 4px;display:none;">
                    <div class="main-gridview">
                        <asp:GridView ID="GridView3" Width="100%" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                            OnRowDataBound="GridView3_RowDataBound" AllowPaging="false">
                            <Columns>
                                <asp:TemplateField HeaderText="序号">
                                    <ItemTemplate>
                                        <%#(Container.DataItemIndex + 1) %>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="单位名称">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="审核状态">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="审核日期">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="文件附件">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                            <RowStyle CssClass="GridViewRowStyle" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <div class="msg-info">
		        <div class="msg-tip icon-tip"></div>
		        <div><asp:Label ID="Lbl_Msg" runat="server" Text="" ForeColor="Red"></asp:Label>&nbsp;</div>
	        </div>
            <asp:Panel ID="Pnl_Edit" runat="server" Visible="false">
                <input type='button' value='全部选择' onclick='selectAll(this);'/>&nbsp;&nbsp;
                <asp:LinkButton ID="LnkBtn_Ins" runat="server" class="easyui-linkbutton" iconCls="icon-save"
                    OnClientClick="return calDefen();" OnClick="LnkBtn_Ins_Click">计算指标得分</asp:LinkButton>
                <asp:LinkButton ID="LnkBtn_Upd" runat="server" class="easyui-linkbutton" iconCls="icon-ok"
                    OnClientClick="showLoading();" OnClick="LnkBtn_Upd_Click">正式保存得分</asp:LinkButton>
                <asp:LinkButton ID="LnkBtn_Cancel" runat="server" class="easyui-linkbutton" iconCls="icon-back"
                    OnClick="LnkBtn_Cancel_Click">返回</asp:LinkButton>
            </asp:Panel>
        </div>
    </div>
    <script type="text/javascript">
        $('#tt').tabs({
            onSelect: function (title, index) {
                $('#tt').tabs('getTab', index).show();
                $('#<%=Hid_TabTitle.ClientID%>').val(title);
            }
        });
        $(function () {
            //tabs
            $('#tt').tabs('select', '<%=TabTitle%>');
        });
    </script>
</asp:Content>
