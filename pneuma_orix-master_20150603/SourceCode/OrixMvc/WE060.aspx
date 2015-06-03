<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/report.Master" CodeBehind="WE060.aspx.cs" Inherits="OrixMvc.WE060" %>
<%@ MasterType VirtualPath="~/Pattern/report.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>

        
<asp:Content ContentPlaceHolderID="editingArea" ID="mySearchArea" runat="server">
<table>
    <tr><th class="nonSpace">申請書編號：</th><td><ocxControl:ocxDialog SourceName="OR_CASE_APLY_BASE" runat="server" ID="APLY_NO" width="140" /></td></tr>
    <tr><th>收票日期：</th><td><ocxControl:ocxDate runat="server" ID="RE_DATE" /></td> </tr>
</table>

                                 
</asp:Content>

