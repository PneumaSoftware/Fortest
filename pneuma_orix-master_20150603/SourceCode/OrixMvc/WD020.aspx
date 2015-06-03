<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/report.Master" CodeBehind="WD020.aspx.cs" Inherits="OrixMvc.WD020" %>
<%@ MasterType VirtualPath="~/Pattern/report.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>

        
<asp:Content ContentPlaceHolderID="editingArea" ID="mySearchArea" runat="server">
<table>
    <tr><th class="nonSpace">申請書編號：</th><td><ocxControl:ocxDialog SourceName="OR_CASE_APLY_BASE_WD010" runat="server" ID="APLY_NO" width="140" /></td></tr>    
    <tr><th class="nonSpace">解約日期：</th><td><ocxControl:ocxDate runat="server" ID="CANCEL_DATE" /></td> </tr>
    <tr><th class="nonSpace">折扣值：</th><td><ocxControl:ocxNumber runat="server" ID="ACCOUNT" MASK="99.9999" />
    <asp:RadioButtonList RepeatLayout="Flow" runat="server" ID="PDiscount_Way" RepeatDirection="Horizontal" >
                    <asp:ListItem Value="1" Selected="True">折扣</asp:ListItem>
                    <asp:ListItem Value="2">固定</asp:ListItem>
                </asp:RadioButtonList></td> </tr>
</table>

                                 
</asp:Content>

 