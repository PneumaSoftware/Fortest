<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/query.Master" CodeBehind="WD010.aspx.cs" Inherits="OrixMvc.WD010" %>
<%@ MasterType VirtualPath="~/Pattern/query.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
    
<asp:Content ContentPlaceHolderID="searchArea" ID="mySearchArea" runat="server">

    
    <table >
        <tr>
            <th>申請書編號：</th><td colspan="2"><ocxControl:ocxDialog runat="server" ID="PAPLY_NO" width="120"  SourceName="OR_CASE_APLY_BASE_WD010" /> </td>           
        </tr>  
        <tr>            
            <th>客戶代號：</th>            
            <td><ocxControl:ocxDialog runat="server" ID="PCUST_NO" width="100"  ControlID="CUST_SNAME" FieldName="CUST_SNAME" SourceName="OR_CUSTOM" /></td>
            <th>客戶簡稱：</th><td><asp:TextBox runat="server" ID="CUST_SNAME"  Width="170" size="20"  CssClass="display"></asp:TextBox></td>                
        </tr>  
        <tr>            
            <th>供應商代號：</th>
            <td><ocxControl:ocxDialog runat="server" ID="PFRC_CODE" width="100"  SourceName="OR_FRC" FieldName="FRC_SNAME"   /></td>
            <th>供應商簡稱：</th><td><asp:TextBox runat="server" ID="FRC_SNAME"  Width="170" size="20"   CssClass="display" ></asp:TextBox></td>                
        </tr>  
         <tr>                        
            <th>承辦業務：</th><td><asp:TextBox runat="server" ID="PSALES_NAME"  Width="100" size="20"></asp:TextBox></td>                
            <td colspan="2"><asp:CheckBox runat="server" id="PYN" Text="只顯示已報價資料" /></td>
        </tr>  
        
    </table>                   
</asp:Content>


<asp:Content ContentPlaceHolderID="queryHead" ID="myQueryHead" runat="server">       
    <th class="fixCol">申請書編號</th>
    <th>客戶代號</th>
    <th>客戶簡稱</th>   
    <th>聯絡人</th>   
    <th>連絡電話</th>   
    <th>供應商代號</th>
    <th>供應商簡稱</th>   
    <th>營業單位</th>    
    <th>承辦業務</th>
    <th>承辦業務電話</th>
</asp:Content>
<asp:Content ContentPlaceHolderID="gridButton" runat="server" ID="myGridButton">
    <asp:Button runat="server" ID="btnAdd" CssClass="button dadd" Text="新增" Visible='<%#Eval("INC_TAX").ToString().Trim()==""?true:false %>'   CommandName="Upd"   />
    <asp:Button runat="server" ID="btnUpd" CssClass="button upd" Text="修改" Visible='<%#Eval("INC_TAX").ToString().Trim()==""?false:true %>'   CommandName="Upd"   />
</asp:Content>

<asp:Content ContentPlaceHolderID="queryBody" ID="myQueryBody" runat="server">       
    <td class="fixCol"><%# Eval("APLY_NO")%><asp:HiddenField runat="server" ID="hiddenAPLY_NO" Value='<%# Eval("APLY_NO")%>' /> </td>
    <td><%# Eval("CUST_NO")%></td>
    <td><%# Eval("CUST_SNAME")%></td>    
    <td><%# Eval("CONTACT")%></td>
    <td><%# Eval("CTAC_TEL")%></td>    
    <td><%# Eval("FRC_CODE")%></td> 
    <td><%# Eval("FRC_SNAME")%></td>  
    <td><%# Eval("SALES_UNIT")%></td>    
    <td><%# Eval("SALES_NAME")%></td>   
    <td><%# Eval("SALES_TEL")%></td> 
</asp:Content>
