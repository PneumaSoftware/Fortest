<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/query.Master" CodeBehind="WA130.aspx.cs" Inherits="OrixMvc.WA130" %>
<%@ MasterType VirtualPath="~/Pattern/query.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
    
<asp:Content ContentPlaceHolderID="searchArea" ID="mySearchArea" runat="server">
<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlCNTR_DEPT" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'CNTR_DEPT' "
    runat="server" >
</asp:sqldatasource>  
<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlPCUR_STS" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'APLYSTS'"
    runat="server" >
</asp:sqldatasource> 
    
    <table >
        <tr>
            <th>申請書編號：</th><td><asp:TextBox runat="server" ID="APLY_NO"  Width="120"  size="15"></asp:TextBox></td>
            <th>目前狀況：</th><td ><asp:DropDownList runat="server" ID="CUR_STS"  Width="80" DataSourceID="sqlPCUR_STS"  DataValueField="TypeCode" DataTextField="TypeDesc" ></asp:DropDownList></td>            
            <th>申請單位：</th>
            <td><ocxControl:ocxDialog runat="server" ID="DEPT_CODE" width="60"   SourceName="OR_DEPT" />
            </td> 
            <th>經辦：</th>
            <td style="width:110px"><asp:TextBox runat="server" ID="EMP_CODE" Width="100"  size="20"></asp:TextBox></td>
        </tr>  
        <tr>            
            <th>客戶代號：</th><td><asp:TextBox runat="server" ID="CUST_NO" Width="90"  size="20"></asp:TextBox></td>
            <th>客戶簡稱：</th><td><asp:TextBox runat="server" ID="CUST_SNAME"  Width="100" size="20"></asp:TextBox></td>   
            <th>是否為計張：</th>
            <td><asp:DropDownList ID="PAPER"  runat="server" Width="70"  >
                <asp:ListItem Value="">全部</asp:ListItem>
                <asp:ListItem Value="Y">是</asp:ListItem>
                <asp:ListItem Value="N">否</asp:ListItem>
                </asp:DropDownList>
            </td> 
             <th>額度編號：</th>
            <td ><asp:TextBox runat="server" ID="CUR_QUOTA_APLY_NO"  Width="100" size="20"></asp:TextBox></td>         
        </tr>  
    </table>                   
</asp:Content>


<asp:Content ContentPlaceHolderID="queryHead" ID="myQueryHead" runat="server">
    <!--QueryHead!-->    
    <th class="fixCol">申請書編號</th>
    <th>客戶代號</th>
    <th>客戶簡稱</th>
    <th>契約編號</th>
    <th>申請日期</th>
    <th>目前狀況</th>
    <th>核淮日期</th>
    <th>是否計張</th>
    <th>經辦</th>
    <th>現用額度編號</th>
</asp:Content>
<asp:Content ContentPlaceHolderID="gridButton" runat="server" ID="myGridButton">
    <asp:Button runat="server" ID="btnAPLY" CssClass="button upd" Text="確認"    CommandName="Upd"   />    
</asp:Content>
<asp:Content ContentPlaceHolderID="queryBody" ID="myQueryBody" runat="server">   
     
    <td class="fixCol"><%# Eval("APLY_NO")%><asp:HiddenField runat="server" ID="hiddenAPLY_NO" Value='<%# Eval("APLY_NO")%>' /><asp:HiddenField runat="server" ID="hiddenCUR_STS" Value='<%# Eval("STSCODE").ToString().Trim()+","+Eval("CUR_STS").ToString().Trim()%>' /> </td>
    <td><%# Eval("CUST_NO")%></td>
    <td><%# Eval("CUST_SNAME")%></td>
    <td><%# Eval("CON_SEQ_NO")%></td>
     <td><%# Eval("APLY_DATE")%></td>
    <td><%# Eval("CUR_STS")%></td>
    <td><%# Eval("APLY_APRV_DATE")%></td>
     <td><%# Eval("PAPER")%></td>
    <td><%# Eval("EMP_NAME")%></td>
    <td><%# Eval("CUR_QUOTA_APLY_NO")%></td>
</asp:Content>
