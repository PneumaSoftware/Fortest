<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/display.Master" CodeBehind="WD050.aspx.cs" Inherits="OrixMvc.WD050" %>
<%@ MasterType VirtualPath="~/Pattern/display.Master" %>
<%@ Register TagName="ocxYM"  TagPrefix="ocxControl" Src="~/ocxControl/ocxYM.ascx" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
      
<asp:Content ContentPlaceHolderID="searchArea" ID="mySearchArea" runat="server">

    <asp:UpdatePanel runat="server" ID="upSearch">
        <ContentTemplate>
        
    <table >
        <tr>
            <th >供應商發票號碼：</th><td><asp:TextBox runat="server" ID="PFRC_INV_NO"  size="10"></asp:TextBox></td>            
            <td rowspan="5" style="vertical-align:top" >
                <fieldset><legend class="nonSpace">列印類別</legend>
                    <asp:RadioButtonList runat="server" ID="PPrt_Type" AutoPostBack="true">
                        <asp:ListItem Value="1" Selected="True">一般查詢</asp:ListItem>
                        <asp:ListItem Value="2">未付款予供應商查詢</asp:ListItem>
                        <asp:ListItem Value="3">可付款明細表</asp:ListItem>
                        <asp:ListItem Value="4">應付款 by AP</asp:ListItem>
                    </asp:RadioButtonList> 
                </fieldset>
            </td> 
            
        </tr>   
        <tr>
            <th >申請書編號：</th><td><asp:TextBox runat="server" ID="PAPLY_NO"  size="14"></asp:TextBox></td>  
        </tr>                    
        <tr>
            <th >客戶代號：</th><td ><asp:TextBox runat="server" ID="PCUST_CODE"  size="10"></asp:TextBox></td>
        </tr>                    
        <tr>
            <th >客戶名稱：</th><td><asp:TextBox runat="server" ID="PCUST_NAME"  size="40"></asp:TextBox></td>             
        </tr>      
        <tr >
            <th >最大票據到期日：</th>
            <td style="display:<%=this.PPrt_Type.SelectedValue.CompareTo("2")<=0?"":"none"%>"  ><ocxControl:ocxDate runat="server" ID="PMAX_Due_Date" /></td>            
            <td style="display:<%=this.PPrt_Type.SelectedValue.CompareTo("2")<=0?"none":""%>"  ><input disabled=disabled readonly=readonly /></td>            
        </tr>        
    </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ContentPlaceHolderID="editingArea" ID="myEditArea" runat="server">
          

<div id="query" class="gridMain ">
    <div style="padding:0;margin:0; overflow-y:scroll;position:relative;width:800px;height:370px" id="divGrid">
        <table cellpadding="0" cellspacing="0" style="width:auto" id="tbGrid" >  
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
                <th class="fixCol">客戶名稱</th>
                <th class="fixCol">期</th>
                <th class="fixCol">供應商</th>                
                <th>供應商發票</th>                
                <th>供_發票日</th>
                <th>供_發票金額</th>
                <th>應收備註</th>
                <th>應收憑單</th>
                <th>應收金額</th>                
                <th>收款金額</th>
                <th>收款日</th>
                <th>票據之一</th>
                <th>票據日Max</th>                
                <th>退票日</th>
                <th>備註2</th>
                <th>未收金額</th>
                <th>營業人員</th>
                <th>應付憑單</th>                
                <th>應付金額</th>
                <th>已付金額</th>
                <th>付款日</th>
                <th>未付金額</th>
                <th>特殊說明</th>
                <th>租金_含</th>
                <th>付款對像</th>
                <th>銷項發票NO.</th>
            </tr>         
            <asp:Repeater runat="server" ID="rptQuery" > 
                <ItemTemplate>
                    <tr class="<%#Container.ItemIndex%2==0?"srow":"" %>" >           
                        <td class="fixCol"><%# Container.ItemIndex+1 %></td> 
                        <td class="fixCol"><%# Eval("APLY_NO") %></td>                         
                        <td class="fixCol"><%# Eval("CUST_NAME") %></td>
                        <td class="fixCol"><%# Eval("PERIOD") %></td>                   
                        <td class="fixCol"><%# Eval("Frc_Name") %></td> 
                        <td><%# Eval("SUPPLY_INVOICE") %></td>                   
                        <td><%# Eval("INVO_DATE") %></td> 
                        <td class="number"><%# Eval("INVO_AMT", "{0:###,###,###,##0}")%></td>                         
                        <td><%# Eval("INVO_REMK") %></td>
                        <td><%# Eval("AR_RECP_NO")%></td>                   
                        <td class="number"><%# Eval("AR_NET_AMT_NT", "{0:###,###,###,##0}")%></td>                   
                        <td class="number"><%# Eval("AR_CAN_AMT_NT", "{0:###,###,###,##0}")%></td>                        
                        <td><%# Eval("AR_ACEP_DATE")%></td> 
                        <td><%# Eval("CHEQ_NO") %></td>                         
                        <td><%# Eval("DUE_DATE") %></td>
                        <td><%# Eval("BACK_DATE") %></td>                   
                        <td><%# Eval("REMK") %></td>                   
                        <td class="number"><%# Eval("Unreceive_amt", "{0:###,###,###,##0}")%></td> 
                        <td><%# Eval("EMP_NAME") %></td>                         
                        <td><%# Eval("AP_RECP_NO") %></td>
                        <td class="number"><%# Eval("AP_NET_AMT_NT", "{0:###,###,###,##0}")%></td>                   
                        <td class="number"><%# Eval("AP_CAN_AMT_NT", "{0:###,###,###,##0}")%></td>                   
                        <td><%# Eval("AP_ACEP_DATE")%></td>                        
                        <td class="number"><%# Eval("Unacep_amt", "{0:###,###,###,##0}")%></td> 
                        <td><%# Eval("REMARK") %></td> 
                        <td class="number"><%# Eval("APRV_amt", "{0:###,###,###,##0}")%></td> 
                        <td><%# Eval("MENU_NAME") %></td> 
                        <td><%# Eval("INVO_NO") %></td> 
                    </tr>
                </ItemTemplate> 
            </asp:Repeater>            
        </table>
    </div>     
</div>    
              
                            
</asp:Content>

