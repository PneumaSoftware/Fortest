<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/display.Master" CodeBehind="WE110.aspx.cs" Inherits="OrixMvc.WE110" %>
<%@ MasterType VirtualPath="~/Pattern/display.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>


 
<asp:Content ContentPlaceHolderID="searchArea" ID="mySearchArea" runat="server">

    <table >
        <tr>
             <th class="nonSpace">解約日期：</th><td><ocxControl:ocxDate runat="server" ID="PCANCEL_DATE" /></td> 
             <th class="nonSpace">月數：</th><td><ocxControl:ocxNumber MASK="999" runat="server" ID="PMONTH" /></td> 
            <th>集團代號：</th><td><ocxControl:ocxDialog runat="server" ID="PBLOC_NO"  width="100" ControlID="PBLOC_NAME" FieldName="BLOC_NAME" SourceName="OR_BLOC" /></td>
            <td><asp:TextBox CssClass="display" ReadOnly="true" runat="server" ID="PBLOC_NAME" size="20" ></asp:TextBox> </td> 
        </tr>
        <tr>            
             <th  class="nonSpace">折扣值：</th><td colspan="3"><ocxControl:ocxNumber MASK="99.9999" runat="server" ID="PDiscount_Rate" /><asp:RadioButtonList RepeatLayout="Flow" runat="server" ID="PDiscount_Way" RepeatDirection="Horizontal" >
                    <asp:ListItem Value="1" Selected="True">折扣</asp:ListItem>
                    <asp:ListItem Value="2">固定</asp:ListItem>
                </asp:RadioButtonList>
             </td> 
            <th>客戶代號：</th><td><ocxControl:ocxDialog runat="server" ID="PCUST_NO"  width="100" ControlID="PCUST_NAME" FieldName="CUST_NAME" SourceName="OR_CUSTOM" /></td>
            <td><asp:TextBox CssClass="display" ReadOnly="true" runat="server" ID="PCUST_NAME"  size="20"></asp:TextBox> </td> 
        </tr>
        <tr>            
            <th>申請書編號：</th><td colspan="3"><asp:TextBox runat="server" ID="PAPLY_NO"   Width="120"/></td> 
            <th>供應商代碼：</th><td>
            <ocxControl:ocxDialog runat="server" ID="PFRC_CODE"  width="100" ControlID="PFRC_NAME" FieldName="FRC_NAME" SourceName="OR_FRC" />
            </td><td><asp:TextBox CssClass="display" ReadOnly="true" runat="server" ID="PFRC_NAME"  size="20"></asp:TextBox> </td> 
        </tr>
    </table>
</asp:Content>

<asp:Content ContentPlaceHolderID="editingArea" ID="myEditArea" runat="server">



<div id="query" class="gridMain " >
    <div  style="padding:0;margin:0; overflow:scroll;position:relative;width:800px;height:370px" id="editGrid">
        <table cellpadding="0" cellspacing="0" style="width:95%" id="tbGrid"  >  
        <thead>
            <tr  >
        <th style="text-align:left;padding-left:5px;border-right:none" class="fixCol"  >
            <asp:UpdatePanel runat="server" ID="upExcel" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
            <asp:Button runat="server" ID="excel" BackColor="Transparent" BorderStyle="None"  OnClick="toExcel" Width="20" Height="18" style="cursor:pointer;background-image:url(images/excel.png)" />       
            </ContentTemplate>
            </asp:UpdatePanel>                
        </th>
        <th colspan="30" ></th>
        </tr>       
           
            <tr id="topBorder">
                 <th class="fixCol" >NO</th>                
                <th >計張案件</th>
                <th >供應商</th>
                <th>購買額</th>
                <th>契約餘額(含稅)</th>
                <th>實質TR</th>
                <th>表面TR</th>
                <th>解約TR</th>
                <th>客戶簡稱</th>
                <th>申請書編號</th>
                <th>合約型態</th>
                <th>起租日</th>              
                <th>到期日</th>
                <th>總期數</th>
                <th>期租金</th> 
                <th>本期數</th> 
                <% for (int i = 0; i < Int32.Parse(this.PMONTH.Text); i++)
                   {%>
                   <th>付完<%= addMonths(i)%>殘值</th><!--1!-->
                <%} %>                                   
               
                <th>客戶代號</th>
                <th>標的物</th>
                <th>機號</th>
                <th>存放地</th>                
            </tr>  
   </thead>
            <asp:Repeater runat="server" ID="rptQuery" > 
                <ItemTemplate>
                    <tr class="<%#Container.ItemIndex%2==0?"srow":"" %>">     
                        <td class="seq fixCol"><%# Container.ItemIndex+1 %></td>      
                        <td ><%# Eval("Rec_Type").ToString() == "1" ? Eval("PAPER") : ""%> </td>                         
                        <td ><%# Eval("Rec_Type").ToString() == "1" ? Eval("FRC_SNAME") : ""%> </td>                         
                        <td style="text-align:right" ><%# Eval("Rec_Type").ToString() == "1" ? Eval("APRV_BUY_AMT", "{0:###,###,###,##0}") : ""%> </td>                         
                        <td style="text-align:right" ><%# Eval("Rec_Type").ToString() == "1" ? Eval("REAL_VAL", "{0:###,###,###,##0}") : ""%> </td>                         
                        <td style="text-align:right" ><%# Eval("Rec_Type").ToString() == "1" ? Eval("APRV_REAL_TR", "{0:###,###.###0}") : ""%> </td>                         
                        <td style="text-align:right" ><%# Eval("Rec_Type").ToString() == "1" ? Eval("APRV_SURF_TR", "{0:###,###.###0}") : ""%> </td>                         
                        <td  style="text-align:right"><%# Eval("Rec_Type").ToString() == "1" ? Eval("TERMTR", "{0:###,###.###0}") : ""%> </td>                         
                        <td ><%# Eval("Rec_Type").ToString() == "1" ? Eval("CUST_SNAME") : ""%> </td>                         
                        <td ><%# Eval("Rec_Type").ToString() == "1" ? Eval("APLY_NO") : ""%> </td>                         
                        <td ><%# Eval("Rec_Type").ToString() == "1" ? Eval("CON_TYPE") : ""%> </td>                         
                        <td ><%# Eval("Rec_Type").ToString() == "1" ? Eval("CON_DATE_FR") : ""%> </td>                         
                        <td ><%# Eval("Rec_Type").ToString() == "1" ? Eval("CON_DATE_TO") : ""%> </td>                         
                        <td style="text-align:right"  ><%# Eval("Rec_Type").ToString() == "1" ? Eval("CTTERMAN", "{0:###,###,###,##0}") : ""%> </td> 
                        <td style="text-align:right" ><%# (Eval("CTTERMPA", "{0:###,###,###,##0}") == "-1" ? "變額" : (Eval("Rec_Type").ToString() == "1" ? Eval("CTTERMPA", "{0:###,###,###,##0}") : ""))%> </td> 
                        <td style="text-align:<%# Eval("Rec_Type").ToString() != "3" ? "right": ""%>" ><%# Eval("Rec_Type").ToString() == "1" || Eval("Rec_Type").ToString() == "3" ? Eval("CTPAYTRM") : ""%> </td> 
                        
                     
                         <% if (Int32.Parse(this.PMONTH.Text)>=1)                   {%>
                    <td style="text-align:<%# Eval("Rec_Type").ToString() != "3" ? "right": ""%>" ><%# Eval("Rec_Type").ToString() == "1" || Eval("Rec_Type").ToString() == "3" ? Eval("Month_1") : ""%> </td>  
                <%} %>     
                          <% if (Int32.Parse(this.PMONTH.Text)>=2)                   {%>
                   <td style="text-align:<%# Eval("Rec_Type").ToString() != "3" ? "right": ""%>" ><%# Eval("Rec_Type").ToString() == "1" || Eval("Rec_Type").ToString() == "3" ? Eval("Month_2") : ""%> </td>  
                <%} %>            
                           <% if (Int32.Parse(this.PMONTH.Text)>=3)                   {%>
                   <td style="text-align:<%# Eval("Rec_Type").ToString() != "3" ? "right": ""%>" ><%# Eval("Rec_Type").ToString() == "1" || Eval("Rec_Type").ToString() == "3" ? Eval("Month_3") : ""%> </td>  
                <%} %>    
                 <% if (Int32.Parse(this.PMONTH.Text)>=4)                   {%>
                   <td style="text-align:<%# Eval("Rec_Type").ToString() != "3" ? "right": ""%>" ><%# Eval("Rec_Type").ToString() == "1" || Eval("Rec_Type").ToString() == "3" ? Eval("Month_4") : ""%> </td>    
                <%} %>     
                          <% if (Int32.Parse(this.PMONTH.Text)>=5)                   {%>
                   <td style="text-align:<%# Eval("Rec_Type").ToString() != "3" ? "right": ""%>" ><%# Eval("Rec_Type").ToString() == "1" || Eval("Rec_Type").ToString() == "3" ? Eval("Month_5") : ""%> </td>   
                <%} %>            
                           <% if (Int32.Parse(this.PMONTH.Text)>=6)                   {%>
                   <td style="text-align:<%# Eval("Rec_Type").ToString() != "3" ? "right": ""%>" ><%# Eval("Rec_Type").ToString() == "1" || Eval("Rec_Type").ToString() == "3" ? Eval("Month_6") : ""%> </td>  
                <%} %>    
                 <% if (Int32.Parse(this.PMONTH.Text)>=7)                   {%>
                   <td style="text-align:<%# Eval("Rec_Type").ToString() != "3" ? "right": ""%>" ><%# Eval("Rec_Type").ToString() == "1" || Eval("Rec_Type").ToString() == "3" ? Eval("Month_7") : ""%> </td>  
                <%} %>     
                          <% if (Int32.Parse(this.PMONTH.Text)>=8)                   {%>
                   <td style="text-align:<%# Eval("Rec_Type").ToString() != "3" ? "right": ""%>" ><%# Eval("Rec_Type").ToString() == "1" || Eval("Rec_Type").ToString() == "3" ? Eval("Month_8") : ""%> </td>  
                <%} %>            
                           <% if (Int32.Parse(this.PMONTH.Text)>=9)                   {%>
                   <td style="text-align:<%# Eval("Rec_Type").ToString() != "3" ? "right": ""%>" ><%# Eval("Rec_Type").ToString() == "1" || Eval("Rec_Type").ToString() == "3" ? Eval("Month_9") : ""%> </td>   
                <%} %>    
                 <% if (Int32.Parse(this.PMONTH.Text)>=10)                   {%>
                   <td style="text-align:<%# Eval("Rec_Type").ToString() != "3" ? "right": ""%>" ><%# Eval("Rec_Type").ToString() == "1" || Eval("Rec_Type").ToString() == "3" ? Eval("Month_10") : ""%> </td>  
                <%} %>     
                          <% if (Int32.Parse(this.PMONTH.Text)>=11)                   {%>
                   <td style="text-align:<%# Eval("Rec_Type").ToString() != "3" ? "right": ""%>" ><%# Eval("Rec_Type").ToString() == "1" || Eval("Rec_Type").ToString() == "3" ? Eval("Month_11") : ""%> </td>    
                <%} %>            
                           <% if (Int32.Parse(this.PMONTH.Text)>=12)                   {%>
                   <td style="text-align:<%# Eval("Rec_Type").ToString() != "3" ? "right": ""%>" ><%# Eval("Rec_Type").ToString() == "1" || Eval("Rec_Type").ToString() == "3" ? Eval("Month_12") : ""%> </td>   
                <%} %>   
                        <td ><%# Eval("Rec_Type").ToString() == "1" ? Eval("CUST_NO") : ""%> </td> 
                        <td ><%# Eval("Rec_Type").ToString() == "1" || Eval("Rec_Type").ToString() == "2" ? Eval("PROD_NAME") : ""%> </td> 
                        <td ><%# Eval("Rec_Type").ToString() == "1" || Eval("Rec_Type").ToString() == "2" ? Eval("MAC_NO") : ""%> </td> 
                        <td ><%# Eval("Rec_Type").ToString() == "1" || Eval("Rec_Type").ToString() == "2" ? Eval("SEND_ADDR") : ""%> </td> 
                         
                                 
                        
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