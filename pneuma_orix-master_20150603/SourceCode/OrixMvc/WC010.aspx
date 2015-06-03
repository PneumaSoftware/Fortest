<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/query.Master" CodeBehind="WC010.aspx.cs" Inherits="OrixMvc.WC010" %>
<%@ MasterType VirtualPath="~/Pattern/query.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
    
<asp:Content ContentPlaceHolderID="searchArea" ID="mySearchArea" runat="server">
    
    <table >
        <tr>
            <th>供應商集團代號：</th><td><ocxControl:ocxDialog runat="server" ID="BLOC_NO" width="100"  SourceName="OR_BLOC" /></td>
            <td><asp:TextBox runat="server" ID="BLOC_NAME" size="20"></asp:TextBox></td>
            <th>供應商代號：</th><td><ocxControl:ocxDialog runat="server" ID="FRC_CODE" width="100"  SourceName="OR_FRC" /></td>
            <td><asp:TextBox runat="server" ID="FRC_SNAME"  size="20"></asp:TextBox></td>                        
        </tr>  
        <tr>
            <th>客戶集團代號：</th><td><ocxControl:ocxDialog runat="server" ID="CUST_BLOC_CODE"  width="100"  SourceName="OR_BLOC" /></asp:TextBox></td>
            <td><asp:TextBox runat="server" ID="CUST_BLOC_NAME"  size="20"></asp:TextBox></td>
            <th>客戶代號：</th><td><ocxControl:ocxDialog runat="server" ID="CUST_NO" width="100"  SourceName="OR_CUSTOM" /></td>
            <td><asp:TextBox runat="server" ID="CUST_SNAME"  size="20"></asp:TextBox></td>            
        </tr>  
    </table>                   
</asp:Content>

<asp:Content ContentPlaceHolderID="queryHead" ID="myQueryHead" runat="server">
    <!--QueryHead!-->    
    <th class="fixCol">供應商代碼</th>
    <th>供應商簡稱</th>
    <th>集團代號</th>    
    <th>集團簡稱</th>      
</asp:Content>
<asp:Content ContentPlaceHolderID="queryBody" ID="myQueryBody" runat="server">
    <td class="fixCol"><%# Eval("FRC_CODE")%><asp:HiddenField runat="server" ID="hiddenFRC_CODE" Value='<%# Eval("FRC_CODE")%>' /> </td>
    <td><%# Eval("FRC_SNAME")%></td>
    <td><%# Eval("BLOC_NO")%></td>
    <td><%# Eval("BLOC_SNAME")%></td>
</asp:Content>
