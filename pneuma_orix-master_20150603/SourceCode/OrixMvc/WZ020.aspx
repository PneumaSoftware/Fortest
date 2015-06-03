<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/query.Master" CodeBehind="WZ020.aspx.cs" Inherits="OrixMvc.WZ020" %>
<%@ MasterType VirtualPath="~/Pattern/query.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
    
<asp:Content ContentPlaceHolderID="searchArea" ID="mySearchArea" runat="server">
<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlGroup" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand=" select USER_ID='',USER_NAME='' union all select USER_ID,USER_NAME  from OR3_USERS where USER_TYPE='G' "
    runat="server" />
    
    <table >
        <tr>
            <th>使用者ID：</th><td><asp:TextBox runat="server" ID="USER_ID"   size="10"></asp:TextBox></td>
            <th>所屬群組：</th><td>
            <asp:DropDownList  runat="server" ID="GROUP_ID" DataSourceID="sqlGroup" DataValueField="USER_ID" DataTextField="USER_NAME" ></asp:DropDownList>        
            </td>                   
        </tr>
        <tr>
            <th>員工代號：</th><td ><asp:TextBox runat="server" ID="EMP_CODE" size="10"></asp:TextBox></td>
            <th>使用者姓名：</th><td><asp:TextBox runat="server" ID="USER_NAME"   size="25"></asp:TextBox></td>
        </tr>        
    </table>
    

                                   
</asp:Content>

<asp:Content ContentPlaceHolderID="queryHead" ID="myQueryHead" runat="server">
    <!--QueryHead!-->    
    <th class="fixCol">使用者ID</th>
    <th>使用者姓名</th>
    <th>使用者區分</th>    
    <th>所屬群組</th>    
    <th>密碼到期日</th>  
    <th>員工代號</th>  
</asp:Content>

<asp:Content ContentPlaceHolderID="queryBody" ID="myQueryBody" runat="server">
    <td class="fixCol"><%# Eval("USER_ID")%><asp:HiddenField runat="server" ID="hiddenUSER" Value='<%# Eval("USER_ID")%>' /> </td>
    <td><%# Eval("USER_NAME")%></td>
    <td><%# Eval("USER_TYPE_NAME")%></td>
    <td><%# Eval("GROUP_ID")%></td>
    <td><%# Eval("PWD_DATE")%></td>
    <td><%# Eval("EMP_CODE")%></td>
</asp:Content>


