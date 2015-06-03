<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/query.Master" CodeBehind="WE010.aspx.cs" Inherits="OrixMvc.WE010" %>
<%@ MasterType VirtualPath="~/Pattern/query.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
    
<asp:Content ContentPlaceHolderID="searchArea" ID="mySearchArea" runat="server">
    
    <table >
        <tr>
            <th>集團代號：</th><td><asp:TextBox runat="server" ID="CUST_BLOC_CODE"  size="10"></asp:TextBox></td>
            <th>集團簡稱：</th><td><asp:TextBox runat="server" ID="CUST_BLOC_SNAME"  size="10"></asp:TextBox></td>
            <th>是否成交：</th><td>
            <asp:DropDownList  runat="server" ID="IS_TRANSACTION"  >
            <asp:ListItem Value="">全部</asp:ListItem>
            <asp:ListItem Value="Y">是</asp:ListItem>
            <asp:ListItem Value="N">否</asp:ListItem>
            </asp:DropDownList>
            </td>            
            
            <th>是否特殊：</th><td>
            <asp:DropDownList  runat="server" ID="SPEC_COND"  >
            <asp:ListItem Value="">全部</asp:ListItem>
            <asp:ListItem Value="Y">是</asp:ListItem>
            <asp:ListItem Value="N">否</asp:ListItem>
            </asp:DropDownList>
            </td>
            
        </tr>  
        <tr>
            <th>客戶代號：</th><td><asp:TextBox runat="server" ID="CUST_NO"  size="10"></asp:TextBox></td>
            <th>客戶簡稱：</th><td><asp:TextBox runat="server" ID="CUST_SNAME"  size="10"></asp:TextBox></td>
            <th>潛在客戶：</th><td>
            <asp:DropDownList  runat="server" ID="CUST_STS" >
            <asp:ListItem Value="">全部</asp:ListItem>
            <asp:ListItem Value="Y">是</asp:ListItem>
            <asp:ListItem Value="N">否</asp:ListItem>
            </asp:DropDownList>
            </td>            
        </tr>  
    </table>                   
</asp:Content>

<asp:Content ContentPlaceHolderID="queryHead" ID="myQueryHead" runat="server">
    <!--QueryHead!-->    
    <th class="fixCol">客戶代號</th>
    <th>客戶簡稱</th>
    <th>集團代號</th>    
    <th>集團簡稱</th>    
    <th>特殊客戶</th>      
</asp:Content>
<asp:Content ContentPlaceHolderID="queryBody" ID="myQueryBody" runat="server">
    <td class="fixCol"><%# Eval("CUST_NO")%><asp:HiddenField runat="server" ID="hiddenCUST_NO" Value='<%# Eval("CUST_NO")%>' /> </td>
    <td><%# Eval("CUST_SNAME")%></td>
    <td><%# Eval("CUST_BLOC_CODE")%></td>
    <td><%# Eval("BLOC_SNAME")%></td>
    <td><%# Eval("SPEC_COND")%></td>
</asp:Content>
