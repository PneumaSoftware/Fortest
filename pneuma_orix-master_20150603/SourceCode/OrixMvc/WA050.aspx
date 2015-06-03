<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/query.Master" CodeBehind="WA050.aspx.cs" Inherits="OrixMvc.WA050" %>
<%@ MasterType VirtualPath="~/Pattern/query.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
    
<asp:Content ContentPlaceHolderID="searchArea" ID="mySearchArea" runat="server">
 
<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlMASTER_STS" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'MASTER_STS'"
    runat="server" >
</asp:sqldatasource> 
    
    <table >
        <tr>
            <th>主約編號：</th><td><asp:TextBox runat="server" ID="MAST_CON_NO"  Width="90"  size="15"></asp:TextBox></td>            
            <th>部門代號：</th>
            <td><ocxControl:ocxDialog runat="server" ID="DEPT_CODE" width="60"   SourceName="OR_DEPT" />
            </td> 
            <th>員工代號：</th>
            <td style="width:110px"><asp:TextBox runat="server" ID="EMP_CODE" Width="100"  size="20"></asp:TextBox></td>
            <th>狀態：</th>
            <td ><asp:DropDownList runat="server" ID="CUR_STS"  Width="80" DataSourceID="sqlMASTER_STS"  DataValueField="TypeCode" DataTextField="TypeDesc" ></asp:DropDownList></td>            
        </tr>  
        <tr>            
            <th>客戶代號：</th><td><asp:TextBox runat="server" ID="CUST_NO" Width="90"  size="20"></asp:TextBox></td>
            <th>客戶簡稱：</th><td><asp:TextBox runat="server" ID="CUST_SNAME"  Width="100" size="20"></asp:TextBox></td>   
            <th>申請日期：</th>
            <td colspan="3"><ocxControl:ocxDate runat="server" ID="APLY_DATE_ST" />~<ocxControl:ocxDate runat="server" ID="APLY_DATE_EN" /></td>      
        </tr>  
    </table>                   
</asp:Content>


<asp:Content ContentPlaceHolderID="queryHead" ID="myQueryHead" runat="server">
    <!--QueryHead!-->    
    <th class="fixCol">狀態</th>
    <th class="fixCol">主約編號</th>    
    <th>客戶簡稱</th>
    <th>部門名稱</th>
    <th>員工姓名</th>
    <th>申請日期</th>    
    <th>核淮日期</th>
    <th>預計到期日期</th>
    <th>實際失效日期</th>
</asp:Content>
<asp:Content ContentPlaceHolderID="gridButton" runat="server" ID="myGridButton">
    <asp:Button runat="server" ID="btnYES" CssClass="button upd" Text="核淮"    CommandName="Appove" Visible='<%# Eval("CUR_STS_CODE").ToString().Trim()=="1"?true:false %>'  />
    <asp:Button runat="server" ID="btnCancel" CssClass="button del" Text="作廢"    CommandName="Cancel"  Visible='<%# Eval("CUR_STS_CODE").ToString().Trim()=="1" || Eval("CUR_STS_CODE").ToString().Trim()=="2" ?true:false %>' />    
</asp:Content>
<asp:Content ContentPlaceHolderID="queryBody" ID="myQueryBody" runat="server">   
    <td class="fixCol"><%# Eval("CUR_STS")%></td>
    <td class="fixCol"><%# Eval("MAST_CON_NO")%><asp:HiddenField runat="server" ID="hiddenMAST_CON_NO" Value='<%# Eval("MAST_CON_NO")%>' /> </td>    
    <td><%# Eval("CUST_SNAME")%></td>
    <td><%# Eval("DEPT_NAME")%></td>
    <td><%# Eval("EMP_NAME")%></td>
     <td><%# Eval("APLY_DATE")%></td>    
    <td><%# Eval("APRV_DATE")%></td>
     <td><%# Eval("PRE_EXPIRY_DATE")%></td>
    <td><%# Eval("EXPIRY_DATE")%></td>    
</asp:Content>
