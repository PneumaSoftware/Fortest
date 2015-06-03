<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/query.Master" CodeBehind="WF010.aspx.cs" Inherits="OrixMvc.WF010" %>
<%@ MasterType VirtualPath="~/Pattern/query.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
    
<asp:Content ContentPlaceHolderID="searchArea" ID="mySearchArea" runat="server">
    
    <table >
        <tr>
            <th>員工代號：</th><td><ocxControl:ocxDialog runat="server" ID="PCorp_Acct" width="100"  SourceName="OR_EMP" /></td><td><asp:TextBox runat="server" ID="PEMP_NAME"   size="10"></asp:TextBox></td> 
            <th class="nonSpace">拜訪日期：</th><td colspan="2"><ocxControl:ocxDate runat="server" ID="PVISIT_DAT_S" />~<ocxControl:ocxDate runat="server" ID="PVISIT_DAT_E" /></td>           
        </tr>  
        <tr>
            <th>供應商代號：</th><td><ocxControl:ocxDialog runat="server" ID="PSUPL_CODE" width="100"  SourceName="OR_FRC" /></td><td><asp:TextBox runat="server" ID="PFRC_SNAME"  size="15"></asp:TextBox></td>  
             <th>客戶代號：</th><td><ocxControl:ocxDialog runat="server" ID="PCUST_CODE" width="100"  SourceName="OR_CUSTOM" /></td><td><asp:TextBox runat="server" ID="PCUST_SNAME"  size="15"></asp:TextBox></td>   
        </tr>   
    </table>
                     
</asp:Content>

<asp:Content ContentPlaceHolderID="queryHead" ID="myQueryHead" runat="server">
    <!--QueryHead!-->    
    <th class="fixCol">員工代號</th>
    <th>員工姓名</th>
    <th>供應商代號</th>    
    <th>供應商名稱</th>    
    <th>客戶代號</th>  
    <th>客戶名稱</th>  
    <th>聯絡人</th>  
    <th>拜訪日期</th>  
    <th>起時</th>  
    <th>迄時</th>  
    <th>預計申請金額</th>  
    <th>訪談主題</th>  
    <th>交通工具</th>  
    <th>申請費用</th>  
</asp:Content>
<asp:Content ContentPlaceHolderID="queryBody" ID="myQueryBody" runat="server">
    <td class="fixCol"><%# Eval("EMP_CODE")%><asp:HiddenField runat="server" ID="hiddenSeqNo" Value='<%# Eval("SeqNo")%>' /> </td>
    <td><%# Eval("EMP_NAME")%></td>
    <td><%# Eval("SUPL_CODE")%></td>
    <td><%# Eval("FRC_NAME")%></td>
    <td><%# Eval("CUST_CODE")%></td>
    <td><%# Eval("CUST_NAME")%></td>
    <td><%# Eval("CTAC")%></td>
    <td><%# Eval("VISIT_DAT")%></td>
    <td><%# Eval("Mes_TStart")%></td>
    <td><%# Eval("Mes_TStop")%></td>
    <td class="number"><%# Eval("FORECAST_APLY_AMT", "{0:###,###,###,##0}")%></td>
    <td><%# Eval("Topic_Desc")%></td>
    <td><%# Eval("Trans_Desc")%></td>
    <td class="number"><%# Eval("APLY_FEE", "{0:###,###,###,##0}")%></td>
</asp:Content>
