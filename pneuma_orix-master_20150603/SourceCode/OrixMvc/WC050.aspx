<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/report.Master" CodeBehind="WC050.aspx.cs" Inherits="OrixMvc.WC050" %>
<%@ MasterType VirtualPath="~/Pattern/report.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>

        
<asp:Content ContentPlaceHolderID="editingArea" ID="mySearchArea" runat="server">
<table>
    <tr><th>申請書編號：</th><td><asp:TextBox runat="server" ID="APLY_NO" MaxLength="15"></asp:TextBox></td> </tr>
    <tr><th>收票日期：</th><td><ocxControl:ocxDate runat="server" ID="RE_DATE" /></td> </tr>
</table>

                                 
</asp:Content>

