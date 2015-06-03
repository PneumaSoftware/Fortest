<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/display.Master" CodeBehind="WE080.aspx.cs" Inherits="OrixMvc.WE080" %>
<%@ MasterType VirtualPath="~/Pattern/display.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
        
<asp:Content ContentPlaceHolderID="searchArea" ID="mySearchArea" runat="server">
    <table >
        <tr>
            <th class="nonSpace">申請書編號：</th><td><ocxControl:ocxDialog SourceName="OR_CASE_APLY_BASE" runat="server" ID="APLY_NO" width="140" /></td>                       
            <th >期數：</th><td><ocxControl:ocxNumber runat="server" ID="Period" MASK="99" /> </td>                        
        </tr>                        
    </table>
</asp:Content>

<asp:Content ContentPlaceHolderID="editingArea" ID="myEditArea" runat="server">


                 
<asp:UpdatePanel runat="server" RenderMode="Inline" ID="upGrid1">
<ContentTemplate> 

<h4>各期張數/費用</h4>
<div id="query" class="gridMain " style="margin-top:0px">
    <div style="padding:0;margin:0; overflow-y:scroll;position:relative;width:800px;height:150px" id="editGrid">
        <table cellpadding="1" cellspacing="0" style="width:auto" id="tbGridM" >  
            <tr >
        <th style="text-align:left;padding-left:5px;border-right:none"  class="fixCol" >
            <asp:UpdatePanel runat="server" ID="upExcel" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
            <asp:Button runat="server" ID="M_excel" BackColor="Transparent" BorderStyle="None"  OnClick="toExcel" Width="20" Height="18" style="cursor:pointer;background-image:url(images/excel.png)" />       
            </ContentTemplate>
            </asp:UpdatePanel>                
        </th>
        
        </tr>    
           
            <tr id="topBorder">                
                <th class="seq fixCol">NO</th>
                <th class="fixCol">期數</th>
                <th class="fixCol">年月</th>
                <th>總金額</th>
                <th>基本租金</th>                
                <th>黑白張數</th>
                <th>免費張數</th>                
                <th>單張金額</th>
                <th>金額</th>
                <th>彩色張數</th>                
                <th>免費張數</th>                
                <th>單張金額</th>
                <th>金額</th>
                <th>黑白A3張數</th>
                <th>免費張數</th>                
                <th>單張金額</th>
                <th>金額</th>
                <th>彩色A3張數</th>                
                <th>免費張數</th>                
                <th>單張金額</th>
                <th>金額</th>
                <th>計算公式</th>
            </tr>  
            <asp:Repeater runat="server" ID="rptQuery"> 
                <ItemTemplate>
                    <tr  id='trA<%#Container.ItemIndex+1%>' class="<%#Container.ItemIndex%2==0?"srow":"" %>">        
                        <td class="fixCol">
                            <asp:UpdatePanel runat="server" ID="upDetail" RenderMode="Inline" UpdateMode="Conditional"> <ContentTemplate>
                           <asp:Button ID="btnDetail" runat="server"  cssclass="button func"  Text="明細" OnCommand="GridFunc_Click" CommandName='<%# Eval("PERIOD") %>' />
                           </ContentTemplate></asp:UpdatePanel>
                        </td>    
                        <td class="fixCol number"><%# Eval("PERIOD", "{0:###,###,###,##0}")%></td>                         
                        <td  class="fixCol"><%# Eval("YEAR_MONTH")%></td>
                        <td class="number"><%# Eval("ACEP_AMT", "{0:###,###,###,##0}")%></td>                   
                        <td class="number"><%# Eval("BASE_RENT_FEE", "{0:###,###,###,##0}")%></td>                   
                        <td class="number"><%# Eval("USE_QTY_MONO", "{0:###,###,###,##0}")%></td>                   
                        <td class="number"><%# Eval("FREE_QTY_MONO", "{0:###,###,###,##0}")%></td>                   
                        <td class="number"><%# Eval("ONE_PRICE_MONO", "{0:###,###,###,##0.0}")%></td> 
                        <td class="number"><%# Eval("MONO_AMT", "{0:###,###,###,##0}")%></td>                         
                        <td class="number"><%# Eval("USE_QTY_COLOR", "{0:###,###,###,##0}")%></td>
                        <td class="number"><%# Eval("FREE_QTY_COLOR", "{0:###,###,###,##0}")%></td>                   
                        <td class="number"><%# Eval("ONE_PRICE_COLOR", "{0:###,###,###,##0.0}")%></td>                   
                        <td class="number"><%# Eval("COLOR_AMT", "{0:###,###,###,##0}")%></td>                        
                        <td class="number"><%# Eval("USE_QTY_A3_MONO", "{0:###,###,###,##0}")%></td> 
                        <td class="number"><%# Eval("FREE_QTY_A3_MONO", "{0:###,###,###,##0}")%></td>                         
                        <td class="number"><%# Eval("ONE_PRICE_A3_MONO", "{0:###,###,###,##0.0}")%></td>
                        <td class="number"><%# Eval("A3_MONO_AMT", "{0:###,###,###,##0}")%></td>                   
                        <td class="number"><%# Eval("USE_QTY_A3_COLOR", "{0:###,###,###,##0}")%></td>                   
                        <td class="number"><%# Eval("FREE_QTY_A3_COLOR", "{0:###,###,###,##0}")%></td> 
                        <td class="number"><%# Eval("ONE_PRICE_A3_COLOR", "{0:###,###,###,##0.0}")%></td>                         
                        <td class="number"><%# Eval("A3_COLOR_AMT", "{0:###,###,###,##0}")%></td>                                           
                        <td><%# Eval("DESC") %></td> 
                    </tr>
                </ItemTemplate> 
            </asp:Repeater>            
        </table>
    </div>     
</div> 
</ContentTemplate>
</asp:UpdatePanel>       
              
<asp:UpdatePanel runat="server" RenderMode="Inline" ID="upGrid2" UpdateMode="Conditional">
<ContentTemplate>
<br />
<h4>單期各機張數/積數</h4>
<div id="query" class="gridMain " style="margin-top:0px">
    <div style="padding:0;margin:0; overflow-y:scroll;position:relative;width:800px;height:240px" id="editGrid">
        <table cellpadding="1" cellspacing="0" style="width:auto" id="tbGridD" >  
            <tr >
        <th style="text-align:left;padding-left:5px;border-right:none"  class="fixCol" >
            <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
            <asp:Button runat="server" ID="D_excel" BackColor="Transparent" BorderStyle="None"  OnClick="toExcel" Width="20" Height="18" style="cursor:pointer;background-image:url(images/excel.png)" />       
            </ContentTemplate>
            </asp:UpdatePanel>                
        </th>
        
        </tr>    
           
            <tr id="Tr1">                
                <th class="seq fixCol">NO</th>             
                <th class="fixCol">機號</th>
                <th>黑白列印張數</th>
                <th>彩色列印張數</th>
                <th>黑白A3列印張數</th>                
                <th>彩色A3列印張數</th>
                <th>黑白A4本月積數</th>
                <th>黑白A4上月積數</th>
                <th>彩色A4本月積數</th>
                <th>彩色A4上月積數</th>
                <th>黑白A3本月積數</th>
                <th>黑白A3上月積數</th>
                <th>彩色A3本月積數</th>
                <th>彩色A3上月積數</th>                
            </tr>  
            <asp:Repeater runat="server" ID="rptDetail"> 
                <ItemTemplate>
                    <tr  class="<%#Container.ItemIndex%2==0?"srow":"" %>">       
                        <td class="fixCol"><%# Container.ItemIndex+1 %></td>     
                        <td class="fixCol"><%# Eval("MAC_NO")%></td>                         
                        <td class="number"><%# Eval("USE_QTY_MONO", "{0:###,###,###,##0}")%></td>        
                        <td class="number"><%# Eval("USE_QTY_COLOR", "{0:###,###,###,##0}")%></td>        
                        <td class="number"><%# Eval("USE_QTY_A3_MONO", "{0:###,###,###,##0}")%></td>        
                        <td class="number"><%# Eval("USE_QTY_A3_COLOR", "{0:###,###,###,##0}")%></td>        
                        <td class="number"><%# Eval("SUM_A4_MONO", "{0:###,###,###,##0}")%></td>        
                        <td class="number"><%# Eval("SUM_A4_MONO_LAST", "{0:###,###,###,##0}")%></td>        
                        <td class="number"><%# Eval("SUM_A4_COLOR", "{0:###,###,###,##0}")%></td>        
                        <td class="number"><%# Eval("SUM_A4_COLOR_LAST", "{0:###,###,###,##0}")%></td>        
                        <td class="number"><%# Eval("SUM_A3_MONO", "{0:###,###,###,##0}")%></td>        
                        <td class="number"><%# Eval("SUM_A3_MONO_LAST", "{0:###,###,###,##0}")%></td>        
                        <td class="number"><%# Eval("SUM_A3_COLOR", "{0:###,###,###,##0}")%></td>        
                        <td class="number"><%# Eval("SUM_A3_COLOR_LAST", "{0:###,###,###,##0}")%></td>        
                    </tr>
                </ItemTemplate> 
            </asp:Repeater>            
        </table>
    </div>     
</div> 
</ContentTemplate>
</asp:UpdatePanel> 
                         
                            
</asp:Content>


<asp:Content ID="Content1" ContentPlaceHolderID="functionBar" runat="server">
<% if (this.bolWE020)
   {%>
 <div class="divButton">
    <input type="button" ID="btnExit" class="button exit" value="返回" OnClick="contentChange('frameContent');" />    
</div>   
<% }%>
</asp:Content>