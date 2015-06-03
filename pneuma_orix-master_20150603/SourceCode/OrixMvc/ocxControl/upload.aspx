<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="upload.aspx.cs" Inherits="OrixMvc.ocxControl.upload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link  href="../Style/content.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div  id="divUpload">
        <input type="button" value="上傳" class="button upload" style="width:50px;position:absolute; z-index:0"  />
        <asp:FileUpload runat="server"  ID="myFile"  style="position:absolute;width:50px; border:none;filter:alpha(opacity=0);-moz-opacity:0;opacity:0; z-index:1" onchange='fileChange();' />                   
        <asp:Button runat="server" ID="uploadFile" style="display:none" OnClick="upload_Click" />
    </div> 
    <div style="display:none" id="divWaiting">
        <img src="../images/ajax-upload.gif" />
     </div>   
    <script language="javascript" type="text/javascript">
         function fileChange() {
             document.getElementById("divUpload").style.display="none";
             document.getElementById("divWaiting").style.display = "";

             window.setTimeout('document.getElementById("<%=this.uploadFile.ClientID %>").click()', 100);
         }
     </script>
     
    </form>
</body>
</html>
