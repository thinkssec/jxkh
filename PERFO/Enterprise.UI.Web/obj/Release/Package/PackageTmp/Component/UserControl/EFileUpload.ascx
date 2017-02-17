<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EFileUpload.ascx.cs" Inherits="Enterprise.UI.Web.Component.UserControl.EFileUpload" %>
  <link href="/Resources/uploadify/uploadify.css" rel="stylesheet" type="text/css" /> 
  <script type="text/javascript" src="/Resources/uploadify/jquery.uploadify.min.js"></script>
<script>    
    var fileExt = '<%=FileTypeExt%>';
    var rtnValToId = '<%=ReturnId%>';
    var rtnNameToId='<%=ReturnFileNameId%>';
    var fileUploadasdfag='';
    var fileNameasdfg='';
    $(function () {
        jQuery.ajaxSetup({ cache: false })
        //附件
        $('#file_upload').uploadify({
            'buttonText': '请选择文件',
            'swf': '/Resources/uploadify/uploadify.swf',
            'uploader': '/Resources/uploadify/uploadify.ashx',
            'cancelImg': '/Resources/uploadify/cancel.png',
            'removeCompleted': true,
            'hideButton': true,
            'auto': true,
            'multi': <%=Muti.ToString().ToLower()%>,
            'queueID': 'upload-queue',
            'fileTypeDesc': 'Pic Files',
            'fileSizeLimit': '50000KB',
            'fileTypeExts': fileExt,
            <%--'onComplete':<%=OnBindDDL%>,--%>
            'onUploadError': function (file, errorCode, errorMsg, errorString) {
                $.messager.alert(errorCode, errorMsg, 'error');
            },
            
            'onQueueComplete': function (queueData) {
                if (queueData.uploadsSuccessful > 0) {
                    parent.$('.easyui-window').window('close');                   
                }
            },
            'onUploadSuccess': function (file, data, response) {                
                             
                parent.$("#" + rtnNameToId).val(file.name);
                parent.$("#" + rtnValToId).val(data);
                //如果是多选还要填充下拉 调用父页面的js
                if(<%=Muti.ToString().ToLower()%>){                    
                    parent.<%=ReturnId%>initUploadDropDownList();
                }
                

            }
        });
    });
</script>
<div id="upload-queue"></div>
<input id="file_upload" type="file" name="Filedata" />
<input id="Hid_ZSYYJ" name="Hid_ZSYYJ" type="hidden" />
<asp:HiddenField ID="fileType" runat="server" />