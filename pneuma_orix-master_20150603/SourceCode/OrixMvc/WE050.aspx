<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/query.Master" CodeBehind="WE050.aspx.cs" Inherits="OrixMvc.WE050" %>
<%@ MasterType VirtualPath="~/Pattern/query.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
    
<asp:Content ContentPlaceHolderID="searchArea" ID="mySearchArea" runat="server">

   <table >
        
        <tr>
            <th >申請書編號：</th><td colspan="2"><ocxControl:ocxDialog SourceName="OR_CASE_APLY_BASE" width="140"  runat="server" ID="APLY_NO"></ocxControl:ocxDialog></td>                       
            <th >客戶代號：</th><td><ocxControl:ocxDialog runat="server" ID="CUST_NO" width="140" SourceName="OR_CUSTOM" FieldName="CUST_NAME" ControlID="CUST_NAME" /></td> 
            <td><asp:TextBox runat="server" ID="CUST_NAME"  size="30" CssClass="display" ReadOnly="true"></asp:TextBox></td>                       
        </tr> 
        <tr>
            <th>員工代號：</th><td><ocxControl:ocxDialog runat="server" ID="PCorp_Acct" width="100" FieldName="EMP_NAME" ControlID="EMP_NAME"  SourceName="OR_EMP" /></td><td><asp:TextBox runat="server" ID="EMP_NAME"  CssClass="display"   size="10"></asp:TextBox></td> 
        </tr>                        
    </table>  


                                   
</asp:Content>


<asp:Content ContentPlaceHolderID="queryHead" ID="myQueryHead" runat="server">
    <!--QueryHead!-->    
    <th class="fixCol">申請書編號</th>
    <th>總期數</th>
    <th>預計入款日</th>    
    <th>申請日期</th>    
    <th>客戶簡稱</th>  
    <th>員工姓名</th>  
    <th>已開票數</th>      
</asp:Content>

<asp:Content ContentPlaceHolderID="queryBody" ID="myQueryBody" runat="server">
    <td class="fixCol"><%# Eval("APLY_NO")%><asp:HiddenField runat="server" ID="hiddenAPLY_NO" Value='<%# Eval("APLY_NO")%>' /></td>
    <td class="number"><%# Eval("aprv_perd")%></td>
    <td><%# Eval("PRJ_PAY_DATE")%></td>
    <td><%# Eval("APLY_DATE")%></td>
    <td><%# Eval("CUST_SNAME")%></td>
    <td><%# Eval("EMP_NAME")%></td>
    <td class="Number" ><%# Eval("CNT", "{0:###,###,###,##0}")%></td>        
</asp:Content>


<asp:Content ContentPlaceHolderID="functionBar" runat="server">
<% if (this.bolWE020)
   {%>
 <div class="divButton">
    <input type="button" ID="btnExit" class="button exit" value="返回" OnClick="contentChange('frameContent');" />    
</div>   
<% }%>
</asp:Content>
