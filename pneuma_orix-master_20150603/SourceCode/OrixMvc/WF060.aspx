<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/report.Master" CodeBehind="WF060.aspx.cs" Inherits="OrixMvc.WF060" %>
<%@ MasterType VirtualPath="~/Pattern/report.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>

        
<asp:Content ContentPlaceHolderID="editingArea" ID="mySearchArea" runat="server">
<table>
    <tr><th>員工代號：</th><td><ocxControl:ocxDialog runat="server" ID="Corp_Acct" width="100" ControlID="EMP_NAME" FieldName="EMP_NAME"  SourceName="OR_EMP" /></td><td><asp:TextBox runat="server" ID="EMP_NAME" ReadOnly="true" CssClass="display"   size="10"></asp:TextBox></td></tr> 
    <tr><th class="nonSpace">本週：</th><td colspan="2"><ocxControl:ocxDate runat="server" ID="DATE_S1" />~<ocxControl:ocxDate runat="server" ID="DATE_E1" /></td> </tr>
    <tr><th class="nonSpace">下週：</th><td colspan="2"><ocxControl:ocxDate runat="server" ID="DATE_S2" />~<ocxControl:ocxDate runat="server" ID="DATE_E2" /></td> </tr>
</table>

                                 
</asp:Content>

