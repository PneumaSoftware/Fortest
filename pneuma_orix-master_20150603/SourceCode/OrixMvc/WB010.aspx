<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/query.Master" CodeBehind="WB010.aspx.cs" Inherits="OrixMvc.WB010" %>
<%@ MasterType VirtualPath="~/Pattern/query.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxYM"  TagPrefix="ocxControl" Src="~/ocxControl/ocxYM.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
    
<asp:Content ContentPlaceHolderID="searchArea" ID="mySearchArea" runat="server">

    <asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlPCUR_STS" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'QuotaSts'"
    runat="server" >
</asp:sqldatasource> 

    <table >
        <tr>
            <th>額度申請書編號：</th><td><asp:TextBox runat="server" ID="APLY_NO"  Width="90"  size="15"></asp:TextBox></td>
            <th>目前狀況：</th><td ><asp:DropDownList runat="server" ID="CUR_STS"  Width="80" DataSourceID="sqlPCUR_STS"  DataValueField="TypeCode" DataTextField="TypeDesc" ></asp:DropDownList></td>            
            <td><asp:CheckBox runat="server" ID="chkQUOTA" Text="只顯示有效額度" /><!--[上午 11:48:13] Louis: 到期日>系統日!--></td>              
            
        </tr>  
        <tr>            
            <th>客戶代號：</th><td><asp:TextBox runat="server" ID="CUST_NO" Width="90"  size="20"></asp:TextBox></td>
            <th>客戶簡稱：</th><td><asp:TextBox runat="server" ID="CUST_SNAME"  Width="100" size="20"></asp:TextBox></td>               
            <th>到期日：</th><td ><ocxControl:ocxDate runat="server" ID="DUE_DATE_ST"  />～<ocxControl:ocxDate runat="server" ID="DUE_DATE_EN" /></td>                                    
        </tr>  
    </table>                   
</asp:Content>



<asp:Content ContentPlaceHolderID="queryHead" ID="myQueryHead" runat="server">
    <!--QueryHead!-->    
    <th class="fixCol">額度申請書編號</th>
    <th>申請日期</th>
    <th>到期日期</th>
    <th>客戶代號</th>
    <th>客戶簡稱</th>   
    <th>目前狀況</th>   
    <th>總額度</th>
</asp:Content>
<asp:Content ContentPlaceHolderID="gridButton" runat="server" ID="myGridButton">
    <asp:Button runat="server" ID="btnAPLY" CssClass="button upd" Text="申請"    CommandName="Upd"   />
    <asp:Button runat="server" ID="btnYES" CssClass="button upd" Text="審核"    CommandName="UpdAfter"   />
    <asp:Button runat="server" ID="btnCancel" CssClass="button del" Text="作廢"    CommandName="Del"   />
</asp:Content>
<asp:Content ContentPlaceHolderID="queryBody" ID="myQueryBody" runat="server">   
     
    <td class="fixCol"><%# Eval("QUOTA_APLY_NO")%><asp:HiddenField runat="server" ID="hiddenAPLY_NO" Value='<%# Eval("QUOTA_APLY_NO")%>' /><asp:HiddenField runat="server" ID="hiddenCUR_STS" Value='<%# Eval("STSCODE").ToString().Trim()+","+Eval("CUR_STS").ToString().Trim()%>' /> </td>
    <td><%# Eval("APLY_DATE")%></td>
    <td><%# Eval("DUE_DATE")%></td>
    <td><%# Eval("CUST_NO")%></td>
    <td><%# Eval("CUST_SNAME")%></td>    
    <td><%# Eval("CUR_STS")%></td>  
    <td><%# Eval("APLY_TOT_QUOTA", "{0:###,###,###,##0}")%></td>
</asp:Content>
