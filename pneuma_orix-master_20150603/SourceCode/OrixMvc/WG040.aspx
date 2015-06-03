<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/display.Master" CodeBehind="WG040.aspx.cs" Inherits="OrixMvc.WG040" %>
<%@ MasterType VirtualPath="~/Pattern/display.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxYM"  TagPrefix="ocxControl" Src="~/ocxControl/ocxYM.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
        
<asp:Content ContentPlaceHolderID="searchArea" ID="mySearchArea" runat="server">
<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlLOAN_MTHD_CODE" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
 SelectCommand="
    select '' as TypeCode,'' as TypeDesc union all
    select Loan_Mthd_code TypeCode,Mthd_Desc TypeDesc from or_Loan_Mthd WHERE Loan_Mthd_code !='00001' 
AND Loan_Mthd_code != '00002' AND Loan_Mthd_code != '00003' order by 1"
    runat="server" >
</asp:sqldatasource> 


<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlCREDIT_WAY" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'COLL_MTHD'"
    runat="server" >
</asp:sqldatasource> 

    <table >
        <tr>
            <th>銀行代碼：</th>
            <td><ocxControl:ocxDialog runat="server" ID="PBANK_NO" width="80" FieldName="BANK_NAME" ControlID="BANK_NAME"  SourceName="ACC18" />
            <asp:TextBox runat="server"  CssClass="display" ID="BANK_NAME"  Width="160"  size="10"></asp:TextBox></td>             
            
            <th>繳息年月：</th><td><ocxControl:ocxYM runat="server" ID="PINTEREST_YM" /></td> 
            <th>還款日期：</th><td><ocxControl:ocxDate runat="server" ID="PRED_DATE" /></td> 
        </tr>
        <tr>            
            <th>借款到期日：</th><td colspan="2"><ocxControl:ocxDate runat="server" ID="PDUE_DATE_S" />~<ocxControl:ocxDate runat="server" ID="PDUE_DATE_E" /></td> 
        </tr>
        <tr>             
             <th>借款方式：</th>
            <td ><asp:DropDownList runat="server" ID="PLOAN_MTHD_CODE"  Width="100" DataSourceID="sqlLOAN_MTHD_CODE"  DataValueField="TypeCode" DataTextField="TypeDesc" ></asp:DropDownList></td>            
           
           
            <th>授信方式：</th>
            <td ><asp:DropDownList runat="server" ID="PCredit_way"  Width="100" DataSourceID="sqlCREDIT_WAY"  DataValueField="TypeCode" DataTextField="TypeDesc" ></asp:DropDownList></td> 
           
            <th>長短借：</th>
            <td> <asp:DropDownList runat="server" ID="PLong_Short_LOAN"  Width="100">
                <asp:ListItem Value=""></asp:ListItem>
                <asp:ListItem Value="1">1.長</asp:ListItem>
                <asp:ListItem Value="2">2.短</asp:ListItem>
                </asp:DropDownList></td>      
            </td>
        </tr>
        <tr>            
            <th>一次/多次攤還：</th><td><asp:DropDownList ID="PREPAY_way"  runat="server" >        
             <asp:ListItem Value=""></asp:ListItem>         
                 <asp:ListItem Value="1">一次</asp:ListItem>
                        <asp:ListItem Value="2">多次</asp:ListItem>
                </asp:DropDownList>
                        
                        
            </td> 
        </tr>
    </table>
</asp:Content>

<asp:Content ContentPlaceHolderID="editingArea" ID="myEditArea" runat="server">
	
<div id="query" class="gridMain ">
    <div style="padding:0;margin:0; overflow-y:scroll;position:relative;width:800px;height:450px" id="divGrid">
        <table cellpadding="0" cellspacing="0" style="width:95%" id="tbGrid"> 
        <thead>
            <tr>     
      
        <th style="text-align:left;padding-left:5px;border-right:none"  class="fixCol" >
            <asp:UpdatePanel runat="server" ID="upExcel" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
            <asp:Button runat="server" ID="excel" BackColor="Transparent" BorderStyle="None"  OnClick="toExcel" Width="20" Height="18" style="cursor:pointer;background-image:url(images/excel.png)" />       
            </ContentTemplate>
            </asp:UpdatePanel>                
        </th>    
        </tr>
         <tr id="topBorder">  
                <th class="fixCol">銀行代碼</th>
                <th>借款方式</th>
                <th>利率</th>                
                <th>授信方式</th>
                <th>固定計息</th>
                <th>借款金額</th>
                <th>有無追索</th>
                <th>借款日期</th>
                <th>到期日</th>
                <th>利息付款方式</th>
                <th>長短借</th>
                <th>繳息日</th>
                <th>利息截止日</th>
                <th>上次計息日</th>
                <th>日數</th>
                <th>利息</th>
                <th>月底提列日</th>
                <th>月底起提列日</th>
                <th>提列日數</th>
                <th>應付利息</th>
            </tr> 
        </thead>    
            <asp:Repeater runat="server" ID="rptQuery"> 
                <ItemTemplate>
                    <tr class="<%#Container.ItemIndex%2==0?"srow":"" %>" >          
                        <td class="fixCol"><%# Eval("BANK_NO")%></td>                         
                        <td><%# Eval("LOAN_MTHD_CODE")%></td>
                        <td style="text-align:right"><%# Eval("RATE", "{0:###,###.###0}")%></td>                   
                        <td><%# Eval("Credit_way")%></td>                   
                        <td><%# Eval("Fixed_interest")%></td>                        
                        <td style="text-align:right"><%# Eval("LOAN_AMT", "{0:###,###,###,##0}")%></td>                         
                        <td><%# Eval("IsRecourse")%></td>
                        <td><%# Eval("LOAN_DATE")%></td>                   
                        <td><%# Eval("Due_date")%></td>                   
                        <td><%# Eval("Interest_Payment")%></td>                        
                        <td><%# Eval("Long_Short_LOAN")%></td>                         
                        <td><%# Eval("PAY_DIVD_DATE")%></td>
                        <td><%# Eval("interes_E_date") %></td>                   
                        <td><%# Eval("interes_S_date")%></td>                   
                        <td><%# Eval("days")%></td>                        
                        <td style="text-align:right"><%# Eval("INTEREST_amt", "{0:###,###,###,##0}")%></td>            
                        <td><%# Eval("Provision_date")%></td>                        
                        <td><%# Eval("Provision_S_date")%></td>                        
                        <td style="text-align:right"><%# Eval("Provision_days")%></td>                        
                        <td style="text-align:right"><%# Eval("AP_INTEREST", "{0:###,###,###,##0}")%></td>                        
                    </tr>
                </ItemTemplate> 
            </asp:Repeater>            
        </table>
    </div>     
</div> 
      
              
                            
</asp:Content>


