<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/query.Master" CodeBehind="WH010.aspx.cs" Inherits="OrixMvc.WH010" %>
<%@ MasterType VirtualPath="~/Pattern/query.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
    
<asp:Content ContentPlaceHolderID="searchArea" ID="mySearchArea" runat="server">
    
    <table >
        <tr>
            <th>業務員代號：</th><td><ocxControl:ocxDialog runat="server" ID="SALES" width="80"   SourceName="SALES" /></td>
            <th>業務員姓名：</th><td><asp:TextBox runat="server" ID="SALES1" size="20"></asp:TextBox></td>                       
        </tr>   <tr>
            <th>部門代號：</th><td><ocxControl:ocxDialog runat="server" ID="DEPT_CODE" width="80"   SourceName="OR_DEPT" /></td>
            <th>部門名稱：</th><td><asp:TextBox runat="server" ID="DEPT_NAME" size="20"></asp:TextBox></td>                       
        </tr>
        <tr>           
            <th>組別：</th>
            <td colspan="3"><asp:TextBox runat="server" ID="DEP_NO2"  size="30"></asp:TextBox></td>            
        </tr>  
    </table>                   
</asp:Content>

<asp:Content ContentPlaceHolderID="queryHead" ID="myQueryHead" runat="server">
    <!--QueryHead!-->    
    <th class="fixCol">業務員代號</th>
    <th>業務員姓名</th>
    <th>業務員部門代號</th>  
    <th>業務員部門名稱</th>    
    <th>業務員組別</th>      
    <th>可用</th>      
    <th>SP可用</th>      
    <th>日報表群組</th>      
</asp:Content>
<asp:Content ContentPlaceHolderID="queryBody" ID="myQueryBody" runat="server">
    <td class="fixCol"><%# Eval("SALES")%><asp:HiddenField runat="server" ID="hiddenSALES" Value='<%# Eval("SALES")%>' /> </td>
    <td><%# Eval("SALES1")%></td>
    <td><%# Eval("DEP_NO")%></td>
    <td><%# Eval("DEP")%></td>
    <td><%# Eval("DEP_NO2")%></td>
    <td><%# Eval("ENABLE")%></td>    
    <td><%# Eval("SP_ENABLE")%></td>
    <td><%# Eval("DAY_REPORT_GROUP")%></td>
</asp:Content>
