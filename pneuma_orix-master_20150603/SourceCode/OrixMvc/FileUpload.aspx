<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileUpload.aspx.cs" Inherits="OrixMvc.FileUpload" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>  
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>未命名頁面</title>
    
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="MYsCRIPT"></asp:ScriptManager>
   <asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlGroup" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand=" select FUNC_ID,FUNC_NAME  from OR3_FUNCTION "
    runat="server" >  
   
</asp:sqldatasource>
 <asp:DropDownList  runat="server" ID="GROUP_ID" DataSourceID="sqlGroup" DataValueField="FUNC_ID" DataTextField="FUNC_NAME" >
                </asp:DropDownList>                     
   
    </form>
</body>
</html>
