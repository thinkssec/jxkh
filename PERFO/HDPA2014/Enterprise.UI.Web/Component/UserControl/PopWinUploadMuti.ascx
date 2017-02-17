<%@ Control Language="C#" AutoEventWireup="true"  CodeBehind="PopWinUploadMuti.ascx.cs" Inherits="Enterprise.UI.Web.Component.UserControl.PopWinUploadMuti"%>
<%--【接收弹出窗口的返回值：文件地址】 每成功一个更新一个 这样防止用户多个上传时取消其中一个后 这里无法获取已上传的值--%>
<asp:HiddenField ID="hd_UploadMuti_Value" runat="server"></asp:HiddenField>
<%--【接收弹出窗口的返回值：文件名】 --%>
<asp:HiddenField ID="hd_UploadMuti_Name" runat="server"></asp:HiddenField>

<%--【下拉菜单的同步隐藏域】 为了提交到后台--%>
<asp:HiddenField ID="hd_UploadMuti_PostBackValue" runat="server" />
<%--【文件名称的同步隐藏域】 为了提交到后台--%>
<asp:HiddenField ID="hd_UploadMuti_PostBackName" runat="server" />

<%--【@@@@如果是单文件上传@@@@】--%>
<%if(!Muti){ %>
<asp:HiddenField ID="hd_UploadSingle_Value" runat="server"></asp:HiddenField>
<%--【单文件上传的同步隐藏域】 为了提交到后台--%>
<asp:TextBox ID="tb_UploadSingle_TextBox" runat="server" CssClass="easyui-validatebox"></asp:TextBox>
<a href="#" onclick="<%=tb_UploadSingle_TextBox.ClientID%>Delete();" style="text-decoration:underline;">删除</a>
<%}else{ %>
<%--【用于客户端删除的下拉菜单】--%>
<asp:DropDownList ID="ddl_DropDownList_muti" runat="server"></asp:DropDownList> &nbsp; 
<a href="#" onclick="<%=ddl_DropDownList_muti.ClientID%>Delete();"  style="text-decoration:underline;">删除</a> 
<%} %>
<a href="#" onclick="openwin('<%=upwin.ClientID %>','<%=UploadUrl %>');" class="easyui-linkbutton" iconCls="icon-upload" plain="true" >上传</a>
<div id="upwin" runat="server" data-options="title:'文件上传'" class="easyui-window" style="width:450px;height:360px" closed="true" modal="true"></div>
<script>
    //删除单个文件上传的隐藏域
    function <%=tb_UploadSingle_TextBox.ClientID%>Delete() {
        $("#<%=tb_UploadSingle_TextBox.ClientID%>").val("");
        $("#<%=hd_UploadSingle_Value.ClientID%>").val("");
    }
    //删除下拉菜单
    function <%=ddl_DropDownList_muti.ClientID%>Delete() {
        $("#<%=ddl_DropDownList_muti.ClientID%> option[value='" + $("#<%=ddl_DropDownList_muti.ClientID%>").val() + "']").remove();
        <%=RtnId%>SyncUploadDropDownListAndHd();
    }

    var <%=hd_UploadMuti_Value.UniqueID%>value = '';

    //多文件名填充
    function <%=RtnId%>initUploadDropDownList() {   
        //var upfiles = ;
        //var upnames = $('#hd_UploadMuti_Name.ClientID').val();

        $("#<%=ddl_DropDownList_muti.ClientID%>").append(
                    "<option value=\"" + $('#<%=hd_UploadMuti_Value.ClientID%>').val() + "\">" + $('#<%=hd_UploadMuti_Name.ClientID%>').val() + "</option>"); 

        <%=RtnId%>SyncUploadDropDownListAndHd();

    }
    //同步值和DropDownlist
    function <%=RtnId%>SyncUploadDropDownListAndHd() {
        var fileStrs = "";
        var fileNameStrs = "";
        $("#<%=ddl_DropDownList_muti.ClientID%> option").each(function () {
            var txt = $(this).text();
            var val = $(this).val();
            fileStrs += val + "|";
            fileNameStrs += txt + "|";
        });
        if (fileStrs.length > 0) {
            fileStrs = fileStrs.substring(0, fileStrs.length - 1);
            fileNameStrs = fileNameStrs.substring(0, fileNameStrs.length - 1);
            
        }
        $("#<%=hd_UploadMuti_PostBackValue.ClientID%>").val(fileStrs);
        $("#<%=hd_UploadMuti_PostBackName.ClientID%>").val(fileNameStrs);
    }

    

    
</script>