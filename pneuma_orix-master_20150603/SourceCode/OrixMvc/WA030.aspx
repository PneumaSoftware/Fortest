<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/report.Master" CodeBehind="WA030.aspx.cs" Inherits="OrixMvc.WA030" %>
<%@ MasterType VirtualPath="~/Pattern/report.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
        
<asp:Content ContentPlaceHolderID="editingArea" ID="mySearchArea" runat="server">
<table>
    <tr><th class="nonSpace">申請書編號：</th><td><ocxControl:ocxDialog SourceName="OR_CASE_APLY_BASE" runat="server" ID="APLY_NO" width="140" /></td></tr> 
    <tr><td><asp:CheckBox runat="server" ID="chkFRC" Text='列印供應商合作協議書' /></td></tr>   
</table>
<iframe name="FRC" style="display:none" src=""></iframe>
                                  
</asp:Content>

