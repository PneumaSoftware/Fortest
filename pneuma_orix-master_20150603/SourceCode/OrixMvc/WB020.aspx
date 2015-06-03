<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/report.Master" CodeBehind="WB020.aspx.cs" Inherits="OrixMvc.WB020" %>
<%@ MasterType VirtualPath="~/Pattern/report.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
        
<asp:Content ContentPlaceHolderID="editingArea" ID="mySearchArea" runat="server">
<table>
    <tr><th class="nonSpace">申請書編號：</th><td><ocxControl:ocxDialog SourceName="OR3_QUOTA_APLY_BASE" runat="server" ID="APLY_NO" width="140" /></td></tr>
    <tr><th></th><td><asp:RadioButtonList runat="server" ID="PrintType" RepeatDirection="Horizontal" RepeatLayout="Flow">
            <asp:ListItem Value="1" Selected="True">列印送審申請書</asp:ListItem>
            <asp:ListItem Value="2">列印核淮申請書</asp:ListItem>
        </asp:RadioButtonList></td> </tr>
   
        
</table>

                                 
</asp:Content>

