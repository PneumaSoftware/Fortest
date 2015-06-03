<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/query.Master" CodeBehind="WA020.aspx.cs" Inherits="OrixMvc.WA020" %>
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
    id="sqlFAST_STS" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'FAST_STS'"
    runat="server" >
</asp:sqldatasource> 
    
    <table >
        <tr>
            <th>申請書編號：</th><td><asp:TextBox runat="server" ID="APLY_NO"  Width="90"  size="15"></asp:TextBox></td>
            <th>經辦：</th>
            <td style="width:110px"> 
            <ocxControl:ocxDialog runat="server" ID="EMP_CODE" Text='<%# Eval("EMP_CODE") %>' width="60"  SourceName="OR_EMP" />
            </td>
            </td>
            <th>先行出合約狀態：</th><td ><asp:DropDownList runat="server" ID="FAST_STS"  Width="80" DataSourceID="sqlFAST_STS"  DataValueField="TypeCode" DataTextField="TypeDesc" ></asp:DropDownList></td>            
        </tr>  
        <tr>            
            <th>客戶代號：</th><td><asp:TextBox runat="server" ID="CUST_NO" Width="90"  size="20"></asp:TextBox></td>
            <th>客戶簡稱：</th><td><asp:TextBox runat="server" ID="CUST_SNAME"  Width="100" size="20"></asp:TextBox></td>                
        </tr>  
    </table>                   
</asp:Content>


<asp:Content ContentPlaceHolderID="queryHead" ID="myQueryHead" runat="server">
    <!--QueryHead!-->    
    <th class="fixCol">申請日期</th>
    <th class="fixCol">申請書編號</th>
    <th>客戶代號</th>
    <th>客戶簡稱</th>   
    <th>經辦</th>    
    <th>目前狀況</th>
</asp:Content>

<asp:Content ContentPlaceHolderID="gridButton" runat="server" ID="Content1">
    <asp:Button runat="server" ID="btnYES" CssClass="button upd" Text="核淮"    CommandName="Appove" Visible='<%# Eval("CUR_STS_CODE").ToString().Trim()=="1"?true:false %>'  />
    <asp:Button runat="server" ID="btnCancel" CssClass="button del" Text="作廢"    CommandName="Cancel"  Visible='<%# Eval("CUR_STS_CODE").ToString().Trim()=="1" || Eval("CUR_STS_CODE").ToString().Trim()=="2" ?true:false %>' />   
    <asp:UpdatePanel runat="server" ID="upPrint" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
         <asp:Button runat="server" ID="btnPrint" CssClass="button upd" Text="送審程序單"    CommandName="print"  /> 
        </ContentTemplate>
    </asp:UpdatePanel>
   
</asp:Content>

<asp:Content ContentPlaceHolderID="queryBody" ID="myQueryBody" runat="server">   
    <td class="fixCol"><%# Eval("APLY_DATE")%></td>
    <td class="fixCol"><%# Eval("APLY_NO")%><asp:HiddenField runat="server" ID="hiddenAPLY_NO" Value='<%# Eval("APLY_NO")%>' /><asp:HiddenField runat="server" ID="hiddenFAST_STS" Value='<%# Eval("CUR_STS")%>' /> </td>
    <td><%# Eval("CUST_NO")%></td>
    <td><%# Eval("CUST_SNAME")%></td>    
    <td><%# Eval("EMP_NAME")%></td> 
    <td><%# Eval("CUR_STS")%></td>    
</asp:Content>
