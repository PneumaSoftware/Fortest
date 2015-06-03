<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/query.Master" CodeBehind="WG010.aspx.cs" Inherits="OrixMvc.WG010" %>
<%@ MasterType VirtualPath="~/Pattern/query.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
    
<asp:Content ContentPlaceHolderID="searchArea" ID="mySearchArea" runat="server">
    
    <table >
        <tr>
            <th>銀行代碼：</th><td><ocxControl:ocxDialog runat="server" ID="BANK_NO" width="100"  SourceName="ACC18" /></td> 
            <th>銀行名稱：</th><td><asp:TextBox runat="server" ID="BANK_NAME"  Width="300"  size="10"></asp:TextBox></td>             
        </tr>          
    </table>
                     
</asp:Content>

<asp:Content ContentPlaceHolderID="queryHead" ID="myQueryHead" runat="server">
    <!--QueryHead!-->    
    <th class="fixCol">銀行代碼</th>
    <th class="fixCol">銀行名稱</th>
    <th>長短借</th>    
    <th>授信額度</th>    
    <th>已使用授信額度</th>  
    <th>剩餘額度</th>  
    <th>授信到期日</th>  
    <th>擔保方式</th>  
    <th>保證費率%</th>      
</asp:Content>
<asp:Content ContentPlaceHolderID="queryBody" ID="myQueryBody" runat="server">
    <td class="fixCol"><%# Eval("BANK_NO")%><asp:HiddenField runat="server" ID="hiddenSeqNo" Value='<%# Eval("SeqNo")%>' /> </td>
    <td class="fixCol"><%# Eval("BANK_NAME")%></td>
    <td><%# Eval("LONG_SHORT_LOAN_DESC")%></td>
    <td class="number"><%# Eval("CRD_AMT", "{0:###,###,###,##0}")%></td>
    <td class="number"><%# Eval("USED_CREDIT", "{0:###,###,###,##0}")%></td>
    <td class="number"><%# Eval("REST_CREDIT", "{0:###,###,###,##0}")%></td>
    <td><%# Eval("CRD_DATE_TO")%></td>
    <td><%# Eval("COLL_MTHD_DESC")%></td>
    <td class="number"><%# Eval("BOND_RATE", "{0:###,###,###,##0}")%></td>    
</asp:Content>
