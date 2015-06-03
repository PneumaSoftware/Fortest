<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/display.Master" CodeBehind="WE070.aspx.cs" Inherits="OrixMvc.WE070" %>
<%@ MasterType VirtualPath="~/Pattern/display.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
        
<asp:Content ContentPlaceHolderID="searchArea" ID="mySearchArea" runat="server">

    <table >
        <tr>
            <th>集團代號：</th><td><ocxControl:ocxDialog runat="server" ID="PBLOC_NO" width="140" ControlID="PBLOC_SNAME" FieldName="BLOC_SNAME"  SourceName="OR_BLOC" /></td> 
            <th>集團簡稱：</th><td><asp:TextBox CssClass="display"  runat="server" ID="PBLOC_SNAME" size=15 ></asp:TextBox> </td> 
        </tr>
        <tr>            
            <th>客戶代號：</th><td><ocxControl:ocxDialog runat="server" ID="PCUST_NO" width="140" ControlID="PCUST_SNAME" FieldName="CUST_SNAME" SourceName="OR_CUSTOM" /></td>
            <th>客戶簡稱：</th><td><asp:TextBox CssClass="display"  runat="server" ID="PCUST_SNAME" size=15 ></asp:TextBox> </td> 
        </tr>
        <tr>
            <th>申請書編號：</th><td><ocxControl:ocxDialog SourceName="OR_CASE_APLY_BASE" width="140" runat="server" ID="PAPLY_NO"></ocxControl:ocxDialog></td> 
        </tr>
    </table>
</asp:Content>

<asp:Content ContentPlaceHolderID="editingArea" ID="myEditArea" runat="server">

<table style="margin-left:10px" cellpadding="2" cellspacing="5">
<tr><th>應收金額總計：</th><td><asp:TextBox runat="server" ID="RECV_AMT" CssClass="display number" ForeColor="red"></asp:TextBox></td>
    <th>已收金額總計：</th><td><asp:TextBox runat="server" ID="CAN_AMT_NT" CssClass="display number" ForeColor="red"></asp:TextBox></td>
    <th>未收金額總計：</th><td><asp:TextBox runat="server" ID="URCV_AMT" CssClass="display number" ForeColor="red"></asp:TextBox></td>
</tr>
</table>
<div id="query" class="gridMain ">
    <div style="padding:0;margin:0;overflow-x:scroll; overflow-y:scroll;width:800px;position:relative;height:400px" id="divGrid">
        <table cellpadding="0" cellspacing="0" style="width:95%"  id="tbGrid" >  
            <tr >
        <th style="text-align:left;padding-left:5px;border-right:none"  class="fixCol" >
            <asp:UpdatePanel runat="server" ID="upExcel" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
            <asp:Button runat="server" ID="excel" BackColor="Transparent" BorderStyle="None"  OnClick="toExcel" Width="20" Height="18" style="cursor:pointer;background-image:url(images/excel.png)" />       
            </ContentTemplate>
            </asp:UpdatePanel>                
        </th>
        
        </tr>               
            <tr id="topBorder">                
                <th class="seq fixCol">NO</th>
                <th class="fixCol">申請書編號</th>
                <th class="fixCol">期數</th>
                <th>總期數</th>                
                <th>請款期間</th>
                <th>~</th>
                <th>客戶代號</th>
                <th>客戶簡稱</th>
                <th>應收日期</th>
                <th>銷帳日</th>
                <th>延滯天數</th>
                <th>應收金額</th>
                <th>已收金額</th>
                <th>未收金額</th>
                <th>收款類別</th>
                <th>支票號碼</th>
                <th>發票票號碼</th>
                <th>備註</th>
                <th>發票日期</th>
                <th>是否為計張</th>
            </tr> 
            <asp:Repeater runat="server" ID="rptQuery"> 
                <ItemTemplate>
                    <tr class="<%#Container.ItemIndex%2==0?"srow":"" %>">          
                        <td class="fixCol"><%# Container.ItemIndex+1 %></td>
                        <td class="fixCol"><%# Eval("Aply_no")%></td>                         
                        <td class="fixCol"><%# Eval("CUR_PERD")%></td>
                        <td><%# Eval("CTTERMAN")%></td>                   
                        <td><%# Eval("Billing_date_S")%></td>                   
                        <td><%# Eval("Billing_date_E")%></td>                        
                        <td><%# Eval("Cust_no")%></td>                         
                        <td><%# Eval("Cust_Sname")%></td>
                        <td><%# Eval("RECV_DATE")%></td>                   
                        <td><%# Eval("CASH_DATE")%></td>                   
                        <td class="number"><%# Eval("DAYS")%></td>                        
                        <td class="number"><%# Eval("RECV_AMT","{0:###,###,###,##0}")%></td>                         
                        <td class="number"><%# Eval("CAN_AMT_NT","{0:###,###,###,##0}")%></td>
                        <td class="number"><%# Eval("URCV_AMT","{0:###,###,###,##0}")%></td>                   
                        <td><%# Eval("COLL_TYPE")%></td>                   
                        <td><%# Eval("ACCANO")%></td>                        
                        <td><%# Eval("Invo_no")%></td>            
                        <td><%# Eval("Remark")%></td>                        
                        <td><%# Eval("InvoDate")%></td>                        
                        <td><%# Eval("PAPER")%></td>                        
                    </tr>
                </ItemTemplate> 
            </asp:Repeater>            
        </table>
    </div>     
</div> 
      

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="functionBar" runat="server">
 <% if (this.bolWE020)
   {%>
 <div class="divButton">
    <input type="button" ID="btnExit" class="button exit" value="返回" OnClick="contentChange('frameContent');" />    
</div>   
<% }%>

</asp:Content>


