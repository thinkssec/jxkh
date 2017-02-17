<%@ Page Title="" Language="C#" MasterPageFile="../Project.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="EjdwMbzLuru.aspx.cs" Inherits="Enterprise.UI.Web.Kh.EjdwMbzLuru" %>

<%@ Import Namespace="Enterprise.Model.Perfo.Kh" %>
<%@ Import Namespace="Enterprise.Component.Infrastructure" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function checkForm() {
            return $("#form1").form('validate');
        }
        function showInfo(tt, msg) {
            $("#msgSpan").html(msg);
            msg = "<a onclick='showMsg();' style='cursor:hand;color:#000;text-decoration:none;'>" + msg + "</a>";
            $.messager.show({
                title: tt,
                msg: msg,
                height: '200',
                width: '400',
                showType: 'show',
                timeout: 6000
            });
        }
        function showMsg() {
            $('#dlg').dialog('open');
        }
        function showLoading() {
            var win = $.messager.progress({
                title: '请您稍侯',
                msg: '正在提交数据...'
            });
            setTimeout(function () {
                $.messager.progress('close');
            }, 100000)
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ProjectPH" runat="server">
    <div data-options="region:'center'">
        <div id="Div1" class="ssec-form">
            <div class="ssec-group ssec-group-hasicon">
                <div class="icon-form"></div>
                <span>基层单位考核目标值录入</span>
            </div>
        </div>
        <div class="main-gridview-title">
            考核期：<Ent:SSECDropDownList ID="Ddl_Kaohe" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_Kaohe_SelectedIndexChanged">
            </Ent:SSECDropDownList>
            &nbsp;&nbsp;
                单位：<Ent:SSECDropDownList ID="Ddl_Danwei" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Ddl_Danwei_SelectedIndexChanged">
                </Ent:SSECDropDownList>
            &nbsp;&nbsp;
                <asp:LinkButton ID="Btn_Add" runat="server" Text="录入考核值" class="easyui-linkbutton" plain="true"
                    iconCls="icon-edit" OnClick="Btn_Add_Click" OnClientClick="showLoading();" ></asp:LinkButton>
            &nbsp;&nbsp;
                <%--<a href="javascript:parent.addTab('/M.K.EjdwJcsjData?<%=UrlParam %>','基础数据表');" 
                    iconCls="icon-calendar" class="easyui-linkbutton" plain="true">录入基础数据</a>--%>
            <asp:HiddenField ID="Hid_TabTitle" runat="server" />
        </div>
        <div id="tt" class="easyui-tabs" style="width: auto;">
            <div title="考核指标" data-options="iconCls:'icon-yunxing'" style="padding: 4px; overflow-y: hidden;">
                <div class="main-gridview">
                    <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                        OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand"
                        AllowPaging="false" DataKeyNames="ID">
                        <Columns>
                            <asp:TemplateField HeaderText="序号">
                                <ItemTemplate>
                                    <%#(Container.DataItemIndex + 1) %>
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
                            <asp:TemplateField HeaderText="年度目标" Visible="false">
                                <ItemTemplate>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="考核权重%" Visible="false">
                                <ItemTemplate>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="考核目标值">
                                <ItemTemplate>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="考核目标值说明">
                                <ItemTemplate>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
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

        </div>
        <div class="msg-info">
            <div class="msg-tip icon-tip"></div>
            <div>
                <asp:Label ID="Lbl_Msg" runat="server" Text="" ForeColor="Red"></asp:Label>&nbsp;
            </div>
        </div>
        <asp:Panel ID="Pnl_Edit" runat="server" Visible="false">
            <asp:LinkButton ID="LnkBtn_Ins" runat="server" class="easyui-linkbutton" iconCls="icon-save"
                OnClientClick="showLoading();" OnClick="LnkBtn_Ins_Click">计算并保存</asp:LinkButton>
            <asp:LinkButton ID="LnkBtn_Upd" runat="server" class="easyui-linkbutton" iconCls="icon-shenhe"
                OnClientClick="showLoading();" OnClick="LnkBtn_Upd_Click">正式提交</asp:LinkButton>
            <asp:LinkButton ID="LnkBtn_Cancel" runat="server" class="easyui-linkbutton" iconCls="icon-back"
                OnClick="LnkBtn_Cancel_Click">返回</asp:LinkButton>
        </asp:Panel>
    </div>
    <div id="dlg" class="easyui-dialog" title="详细内容" style="width: 400px; height: 400px; padding: 10px;"
        data-options="resizable:true,modal:true,closed:true">
        <span id="msgSpan"></span>
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
