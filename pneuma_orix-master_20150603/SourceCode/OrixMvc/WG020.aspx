<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/query.Master" CodeBehind="WG020.aspx.cs" Inherits="OrixMvc.WG020" %>
<%@ MasterType VirtualPath="~/Pattern/query.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
    
<asp:Content ContentPlaceHolderID="searchArea" ID="mySearchArea" runat="server">
<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlLOAN_MTHD_CODE" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
     SelectCommand="
    select Loan_Mthd_code TypeCode,Mthd_Desc TypeDesc from or_Loan_Mthd WHERE Loan_Mthd_code !='00001' 
AND Loan_Mthd_code != '00002' AND Loan_Mthd_code != '00003' order by Loan_Mthd_Code"
    runat="server" >
</asp:sqldatasource> 

<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlLOAN_TYPE" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'LOAN_TYPE'"
    runat="server" >
</asp:sqldatasource> 


<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlCREDIT_WAY" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'COLL_MTHD'"
    runat="server" >
</asp:sqldatasource> 

<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlFixed_interest" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'Fixed_interest'"
    runat="server" >
</asp:sqldatasource> 
     <table >
        <tr>
            <th>銀行代碼：</th><td><ocxControl:ocxDialog runat="server" ID="BANK_NO" width="80" FieldName="BANK_NAME" ControlID="BANK_NAME"  SourceName="ACC18" /></td> 
            <td colspan="2"><asp:TextBox runat="server"  CssClass="display" ID="BANK_NAME"  Width="160"  size="10"></asp:TextBox></td>             
            <th>借款日期：</th>
            <td colspan="3"><ocxControl:ocxDate runat="server" ID="LOAN_DATE_ST" />~<ocxControl:ocxDate runat="server" ID="LOAN_DATE_EN" /></td>  
        </tr>
        <tr>
            <th>借款方式：</th>
            <td ><asp:DropDownList runat="server" ID="LOAN_MTHD_CODE"  Width="100" DataSourceID="sqlLOAN_MTHD_CODE"  DataValueField="TypeCode" DataTextField="TypeDesc" ></asp:DropDownList></td>            
            <th style=" visibility:hidden">借款分類：</th>
            <td  style="visibility:hidden"><asp:DropDownList runat="server" ID="LOAN_TYPE"  Width="100" DataSourceID="sqlLOAN_TYPE"  DataValueField="TypeCode" DataTextField="TypeDesc" ></asp:DropDownList></td>            
            <th>到期日：</th>
            <td colspan="3"><ocxControl:ocxDate runat="server" ID="DUE_DATE_ST" />~<ocxControl:ocxDate runat="server" ID="DUE_DATE_EN" /></td>  
       </tr>     
       <tr>            
            <th>長短借：</th>
            <td >
                <asp:DropDownList runat="server" ID="LONG_SHORT_LOAN"  Width="100">
                <asp:ListItem Value=""></asp:ListItem>
                <asp:ListItem Value="1">1.長</asp:ListItem>
                <asp:ListItem Value="2">2.短</asp:ListItem>
                </asp:DropDownList></td>            
            <th>有無追索：</th>
            <td ><asp:DropDownList runat="server" ID="IsRecourse"  Width="100"  >
                    <asp:ListItem Value=""></asp:ListItem>
                    <asp:ListItem Value="Y">Y</asp:ListItem>
                    <asp:ListItem Value="N">N</asp:ListItem>
                </asp:DropDownList>
            </td>                        
            <th>授信方式：</th>
            <td ><asp:DropDownList runat="server" ID="CREDIT_WAY"  Width="100" DataSourceID="sqlCREDIT_WAY"  DataValueField="TypeCode" DataTextField="TypeDesc" ></asp:DropDownList></td>            
            <th>固定/浮動利率：</th>
            <td ><asp:DropDownList runat="server" ID="Fixed_interest"  Width="100" >
                 <asp:ListItem Value=""></asp:ListItem>
                <asp:ListItem Value="1">1.固定</asp:ListItem>
                <asp:ListItem Value="2">2.浮動</asp:ListItem>
            </asp:DropDownList></td>            
        </tr>          
    </table>
    
                 
</asp:Content>

<asp:Content ContentPlaceHolderID="queryHead" ID="myQueryHead" runat="server">
    <!--QueryHead!-->    
    <th class="fixCol">銀行代碼</th>
    <th class="fixCol">銀行名稱</th>
    <th>借款日期</th>  
    <th>借款方式</th>    
    <th>借款金額</th>      
    <th>利率</th>      
    <th>繳息日</th>          
    <th>授信方式</th> 
    <th>長短借</th> 
    <th>有無追索</th> 
    <th>到期日</th> 
    <th>利息計算方式</th> 
</asp:Content>
<asp:Content ContentPlaceHolderID="gridButton" runat="server" ID="myGridButton">
    <asp:Button runat="server" ID="btnBack" CssClass="button upd" Text='還款'  CommandName="Back"  />    
</asp:Content>
<asp:Content ContentPlaceHolderID="queryBody" ID="myQueryBody" runat="server">
    <td class="fixCol"><%# Eval("BANK_NO")%><asp:HiddenField runat="server" ID="hiddenSeq" Value='<%# Eval("SeqNO").ToString().Trim()+","+Eval("LOAN_SEQ").ToString().Trim()%>' /> </td>
    <td class="fixCol"><%# Eval("BANK_NAME")%></td>
    <td><%# Eval("LOAN_DATE")%></td>
    <td><%# Eval("MTHD_DESC")%></td>
    <td><%#  Eval("LOAN_AMT", "{0:###,###,###,##0}") %></td>
    <td><%# Eval("RATE", "{0:###,###,###,###,###0}")%></td>    
    <td><%# Eval("LOAN_DATE")%></td>    
    <td><%# Eval("Credit_way_DESC")%></td>
    <td><%# Eval("Long_Short_LOAN_DESC")%></td>
    <td><%# Eval("IsRecourse")%></td>
    <td><%# Eval("Due_date")%></td>
    <td><%# Eval("Interest_Cal_DESC")%></td>
    
</asp:Content>
