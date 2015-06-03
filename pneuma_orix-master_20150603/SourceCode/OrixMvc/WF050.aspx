<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/report.Master" CodeBehind="WF050.aspx.cs" Inherits="OrixMvc.WF050" %>
<%@ MasterType VirtualPath="~/Pattern/report.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>

        
<asp:Content ContentPlaceHolderID="editingArea" ID="mySearchArea" runat="server">
<table>    
    <tr><th>員工代號：</th><td><ocxControl:ocxDialog runat="server" ID="Corp_Acct" width="100" ControlID="EMP_NAME" FieldName="EMP_NAME"  SourceName="OR_EMP" /></td><td><asp:TextBox runat="server" ID="EMP_NAME" ReadOnly="true" CssClass="display"   size="10"></asp:TextBox></td></tr> 
    <tr><th>供應商代號：</th><td><ocxControl:ocxDialog runat="server" ID="FRC_CODE" width="100" ControlID="PFRC_SNAME" FieldName="FRC_SNAME" SourceName="OR_FRC" /></td><td><asp:TextBox runat="server" ID="PFRC_SNAME" ReadOnly="true" CssClass="display" size="15"></asp:TextBox></td></tr>   
    <tr><th class="nonSpace">拜訪日期：</th><td colspan="2"><ocxControl:ocxDate runat="server" ID="DATE_S" />~<ocxControl:ocxDate runat="server" ID="DATE_E" /></td> </tr>
</table>

                                 
</asp:Content>

