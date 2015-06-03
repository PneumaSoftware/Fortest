<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/display.Master" CodeBehind="WC030.aspx.cs" Inherits="OrixMvc.WC030" %>
<%@ MasterType VirtualPath="~/Pattern/display.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
        
<asp:Content ContentPlaceHolderID="searchArea" ID="mySearchArea" runat="server">
<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlAPLYSTS" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'APLYSTS' "
    runat="server" >
</asp:sqldatasource>

<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlUNIT" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="select distinct SALES_UNIT from OR_OBJECT  order by SALES_UNIT "
    runat="server" >
</asp:sqldatasource>

    <table >
         <tr>
            <th>供應商集團代號：</th><td><ocxControl:ocxDialog runat="server" ID="BLOC_NO" width="100"  SourceName="OR_BLOC" ControlID="BLOC_NAME" FieldName="BLOC_NAME" /></td>
            <td><asp:TextBox runat="server" ID="BLOC_NAME" size="20" CssClass="display"></asp:TextBox></td>
            <th>供應商代號：</th><td><ocxControl:ocxDialog runat="server" ID="FRC_CODE" width="100"  SourceName="OR_FRC"  ControlID="FRC_SNAME" FieldName="FRC_SNAME"/></td>
            <td><asp:TextBox runat="server" ID="FRC_SNAME"  size="20" CssClass="display"></asp:TextBox></td>                        
        </tr>        
         <tr>
            <th>客戶集團代號：</th><td><ocxControl:ocxDialog runat="server" ID="CUST_BLOC_CODE"  width="100"  SourceName="OR_BLOC"  ControlID="CUST_BLOC_NAME" FieldName="CUST_SNAME" /></asp:TextBox></td>
            <td><asp:TextBox runat="server" ID="CUST_BLOC_NAME"  size="20" CssClass="display" ></asp:TextBox></td>
            <th>客戶代號：</th><td><ocxControl:ocxDialog runat="server" ID="CUST_CODE" width="100"  SourceName="OR_CUSTOM"  ControlID="CUST_SNAME" FieldName="CUST_SNAME" /></td>
            <td><asp:TextBox runat="server" ID="CUST_SNAME"  size="20" CssClass="display" ></asp:TextBox></td>            
        </tr>
         <tr>
            <th>營業單位：</th>
            <td colspan="2"  >
                <asp:DropDownList  runat="server" ID="PSALES_UNIT"  Width=180                                                      
                DataSourceID="sqlUNIT" DataValueField="SALES_UNIT" DataTextField="SALES_UNIT" >
                </asp:DropDownList> 
            </td>
            <th>契約起迄：</th>
            <td colspan="2"><ocxControl:ocxDate runat="server" id="PCON_DATE_FR_S" />~<ocxControl:ocxDate runat="server" id="PCON_DATE_FR_E" /></td>
        </tr> 
        <tr>            
            <th>申請書狀態：</th>
            <td colspan="2"  >
                 <asp:DropDownList runat="server" ID="PCUR_STS" Width="180"  DataSourceID="sqlAPLYSTS" DataValueField="TypeCode" DataTextField="TypeDesc"  >
                </asp:DropDownList>
                   
            </td>
            <td colspan="2"><asp:RadioButtonList runat="server" ID="PQUERY_Type" RepeatDirection="Horizontal" RepeatLayout="Flow" AutoPostBack="false">
                <asp:ListItem Selected="True" Value="1">依合約</asp:ListItem>
                <asp:ListItem Value="2">依標的物</asp:ListItem>
            </asp:RadioButtonList> </td>
        </tr>
    </table>
</asp:Content>

<asp:Content ContentPlaceHolderID="editingArea" ID="myEditArea" runat="server">

<div id="query" class="gridMain ">
    <div style="padding:0;margin:0; overflow-y:scroll;position:relative;width:800px;height:430px" id="editGrid">
        <table cellpadding="0" cellspacing="0" style="width:95%" id="tbGrid" >  
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
                <th class="fixCol">客戶代號</th>               
                <th class="fixCol">客戶簡稱</th>                
                <th class="fixCol">申請書編號</th>                
                <th>起租日期</th>
                <th>到期日期</th>
                <th>總期數</th>
                <th>每期租金</th>
                <th>實際TR</th>
                <th>表面TR</th>
                <th>購買額</th>
                <th>契約總額</th>
                <th>契約餘額</th>
                <th>標的物</th>
                <th>機號</th>
                <th>存放地</th>
                <th>買回條件</th>
                <th>買回比率</th>
                <th>是否為計張</th>
            </tr> 
            <asp:Repeater runat="server" ID="rptQuery" > 
                <ItemTemplate>
                    <tr  class=" <%#Container.ItemIndex%2==0?"srow":"" %>">   
                        <td class="fixCol"><%# Container.ItemIndex+1 %></td>       
                        <td  class="fixCol"><%# Eval("Cust_Code")%></td>                         
                        <td  class="fixCol"><%# Eval("Cust_Sname")%></td>
                        <td  class="fixCol"><%# Eval("Aply_no")%></td>                   
                        <td><%# Eval("CON_DATE_FR")%></td>                   
                        <td><%# Eval("CON_DATE_TO")%></td>                        
                        <td class="number"><%# Eval("CTTERMAN")%></td>                         
                        <td class="number"><%# Eval("PERD_AMT", "{0:###,###,###,##0}")%></td>
                        <td class="number"><%# Eval("APRV_REAL_TR","{0:###,###,###,##0}")%></td>                   
                        <td class="number"><%# Eval("APRV_SURF_TR","{0:###,###,###,##0}")%></td>                   
                        <td class="number"><%# Eval("PURS_AMT", "{0:###,###,###,##0}")%></td>                        
                        <td class="number"><%# Eval("CON_TOL", "{0:###,###,###,##0}")%></td>                         
                        <td class="number"><%# Eval("CON_SUR_AMT", "{0:###,###,###,##0}")%></td>
                        <td><%# Eval("PROD_NAME")%></td>                   
                        <td><%# Eval("MAC_NO")%></td>
                        <td><%# Eval("SEND_ADDR")%></td>                                           
                        <td><%# Eval("BUY_WAY")%></td>                        
                        <td class="number"><%# Eval("DUE_BUY_RATE", "{0:###,###,###,##0}")%></td>                        
                        <td><%# Eval("PAPER")%></td>                        
                    </tr>
                </ItemTemplate> 
            </asp:Repeater>            
        </table>
    </div>     
</div> 
      
		
</asp:Content>


