<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/query.Master" CodeBehind="WD040.aspx.cs" Inherits="OrixMvc.WD040" %>
<%@ MasterType VirtualPath="~/Pattern/query.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxYM"  TagPrefix="ocxControl" Src="~/ocxControl/ocxYM.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
    
<asp:Content ContentPlaceHolderID="searchArea" ID="mySearchArea" runat="server">

    
    <table >
        <tr>
            <th>原申請書編號：</th><td ><ocxControl:ocxDialog runat="server" ID="PAPLY_NO" width="120"  SourceName="OR_CASE_APLY_BASE_WD040" /> </td>                   
            <th>申請書編號：</th><td ><ocxControl:ocxDialog runat="server" ID="APLY_NO" width="120"  SourceName="OR_CASE_APLY_BASE_WD040" /> </td>                   
            <th>期數：</th>            
            <td><ocxControl:ocxNumber  runat="server"  ID="PERIOD_ST" MASK="999"  />～<ocxControl:ocxNumber  runat="server"  ID="PERIOD_EN" MASK="999"  /></td>
        </tr>  
        <tr>            
            <th>年月：</th><td><ocxControl:ocxYM runat="server" ID="YEAR_MONTH"  /></td> 
            <th>匯入日期：</th>
            <td><ocxControl:ocxDate runat="server" ID="IMPORT_DATE"  /></td>             
        </tr>  
        <tr>
            <th>發票號碼：</th>
            <td><asp:TextBox runat="server" ID="INVO_NO"  Width="100"></asp:TextBox></td>
            <th>發票日：</th>
            <td><ocxControl:ocxDate runat="server" ID="INV_DATE"  /></td>            
            <th>供應商機號：</th>
            <td><asp:TextBox runat="server" ID="MAC_NO"  Width="100"></asp:TextBox></td>
        </tr>
    </table>                   
</asp:Content>

<asp:Content ContentPlaceHolderID="queryHead" ID="myQueryHead" runat="server">       
    <th class="fixCol">原匯入申請書</th>
    <th>期數</th>
    <th>申請書編號</th>   
    <th>期數</th>   
    <th>年月</th>   
    <th>匯入日期</th>
    <th>起始日期</th>       
</asp:Content>

<asp:Content ContentPlaceHolderID="gridButton" runat="server" ID="myGridButton">
<!--
    <asp:checkbox runat="server" ID="btnUpd" CssClass="button dadd" Text="" Visible="false"     />    
    !-->
</asp:Content>

<asp:Content ContentPlaceHolderID="queryBody" ID="myQueryBody" runat="server">       
    <td style='<%# Eval("MSG").ToString()!=""?"background-color:red":""%>' class="fixCol"><%# Eval("ORI_APLY_NO")%><asp:HiddenField runat="server" ID="hiddenAPLY_NO" Value='<%# Eval("APLY_NO")%>' /><asp:HiddenField runat="server" ID="hiddenPAPLY_NO" Value='<%# Eval("ORI_APLY_NO")%>' /> </td>
    <td style='<%# Eval("MSG").ToString()!=""?"background-color:red":""%>'><%# Eval("ORI_PERIOD")%></td>
    <td style='<%# Eval("MSG").ToString()!=""?"background-color:red":""%>'><%# Eval("APLY_NO")%></td>    
    <td style='<%# Eval("MSG").ToString()!=""?"background-color:red":""%>'><%# Eval("PERIOD")%></td>
    <td style='<%# Eval("MSG").ToString()!=""?"background-color:red":""%>'><%# Eval("YEAR_MONTH")%></td>    
    <td style='<%# Eval("MSG").ToString()!=""?"background-color:red":""%>'><%# Eval("IMPORT_DATE")%></td> 
    <td style='<%# Eval("MSG").ToString()!=""?"background-color:red":""%>'><%# Eval("STAR_DATE")%></td>      
</asp:Content>
