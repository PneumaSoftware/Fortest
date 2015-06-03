<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/display.Master" CodeBehind="WE100.aspx.cs" Inherits="OrixMvc.WE100" %>
<%@ MasterType VirtualPath="~/Pattern/display.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
        
<asp:Content ContentPlaceHolderID="searchArea" ID="mySearchArea" runat="server">
    <table >
        <tr>
            <th>集團代號：</th>
            <td><ocxControl:ocxDialog runat="server" ID="PBLOC_NO"  width="100" ControlID="PBLOC_NAME" FieldName="BLOC_NAME" SourceName="OR_BLOC" /> </td>
            <td><asp:TextBox CssClass="display"  runat="server" ID="PBLOC_NAME" size="25" ></asp:TextBox> </td> 
            <td colspan="2">
                <fieldset style="padding:0">
                <asp:RadioButtonList runat="server" ID="PIncludeClosed" RepeatDirection="Horizontal">
                    <asp:ListItem Value="1" Selected="True">含已結清案件</asp:ListItem>
                    <asp:ListItem Value="2">不含已結清案件</asp:ListItem>
                </asp:RadioButtonList></fieldset></td>
        </tr>
        <tr>            
            <th>客戶代號：</th><td><ocxControl:ocxDialog runat="server" ID="PCUST_NO" width="100"  SourceName="OR_CUSTOM" ControlID="PCUST_NAME" FieldName="CUST_SNAME" /></td><td><asp:TextBox runat="server" ID="PCUST_NAME" CssClass="display" ReadOnly="true" size="25"></asp:TextBox> </td> 
            <td><asp:CheckBox runat="server" ID="PIncludeBail"  Text="包含保證人"  /></td>
        </tr>
    </table>
</asp:Content>

	
<asp:Content ContentPlaceHolderID="editingArea" ID="myEditArea" runat="server">


<div id="query" >    
        
    <div class="gridMain " style="padding:0;margin:0; overflow:scroll;position:relative;width:800px;height:310px" id="editGrid">
        
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
                <th class="seq fixCol">No</th>
                <th class="fixCol">關係</th>
                <th class="fixCol">申請書編號</th>
                <th class="fixCol">客戶簡稱</th>                
                <th>契約起日</th>
                <th>契約迄日</th>              
                <th>標的物</th>
                <th>標的物所有權</th>
                <th>標的物件數</th>                
                <th>目前狀況</th>
                <th>期數</th>                
                <th>已實行期數</th>
                <th>每期租金</th>
                <th>契約總額</th>
                <th>購買額</th>
                <th>T/R</th>
                <th>毛收益</th>
                <th>契約餘額</th>
                <th>本金餘額</th>                
                <th>經辦人</th> 
                <th>部門</th> 
            </tr> 

            <asp:Repeater runat="server" ID="rptQuery"> 
                <ItemTemplate>
                    <tr class="<%#Container.ItemIndex%2==0?"srow":"" %>">   
                        <td class="seq fixCol"><%#Container.ItemIndex+1%></td>       
                        <td class="fixCol"><%# Eval("SCUR_RELATION")%></td>
                        <td class="fixCol"><%# Eval("Aply_no")%></td>
                        <td class="fixCol"><%# Eval("Cust_Sname")%></td>                                                 
                        <td><%# Eval("CON_DATE_FR")%></td>                   
                        <td><%# Eval("CON_DATE_TO") %></td>                   
                        <td><%# Eval("PROD_NAME")%></td>                                 
                        <td><%# Eval("obj_due_owner")%></td>      
                        <td class="number"><%# Eval("Obj_Cnt", "{0:###,###,###,##0}")%></td>
                        <td><%# Eval("CUR_STS")%></td>                         
                        <td class="number"><%# Eval("APRV_PERD", "{0:###,###,###,##0}")%></td>
                        <td class="number"><%# Eval("pay_mm", "{0:###,###,###,##0}")%></td>                   
                        <td class="number"><%# (Eval("PERD_AMT").ToString().Trim() == "" ? "變階" : Eval("PERD_AMT", "{0:###,###,###,##0}"))%></td>                   
                        <td class="number"><%# Eval("CON_TOL", "{0:###,###,###,##0}")%></td>                        
                        <td class="number"><%# Eval("PURS_AMT", "{0:###,###,###,##0}")%></td>                         
                        <td class="number"><%# Eval("TR", "{0:###,###,###,##0.###0}")%></td>
                        <td class="number"><%# Eval("MARG", "{0:###,###,###,##0}")%></td>                   
                        <td class="number"><%# Eval("CON_SUR_AMT", "{0:###,###,###,##0}")%></td>                   
                        <td class="number"><%# Eval("CAPT_SUR_AMT", "{0:###,###,###,##0}")%></td>                                
                        <td><%# Eval("EMP_NAME")%></td>      
                        <td><%# Eval("DEPT_NAME")%></td>     
                    </tr>
                </ItemTemplate> 
            </asp:Repeater>            
        </table>
    </div>     
</div> 
 <table cellpadding="1" cellspacing="1"  style="margin-top:10px">        
        <tr><td><h5>本戶總計</h5></td> 
            <th>契約總額：</th><td><asp:TextBox CssClass="number" runat="server" ID="CON_TOL1" ReadOnly="true"   size="10"></asp:TextBox></td>
            <th>購買額：</th><td><asp:TextBox CssClass="number" runat="server" ID="PURS_AMT1" ReadOnly="true"   size="10"></asp:TextBox></td>
            <th>加權平均T/R：</th><td><asp:TextBox CssClass="number" runat="server" ID="TR1"  ReadOnly="true"   size="7"></asp:TextBox></td>
            <th>契約餘額：</th><td><asp:TextBox CssClass="number" runat="server" ID="CON_SUR_AMT1"  ReadOnly="true"   size="10"></asp:TextBox></td>
            <th>毛收益：</th><td><asp:TextBox CssClass="number" runat="server" ID="MARG1"  ReadOnly="true"   size="7"></asp:TextBox></td>
        </tr>
        <tr><td><h5>保證總計</h5></td> 
            <th>契約總額：</th><td><asp:TextBox CssClass="number" runat="server" ID="CON_TOL2" ReadOnly="true"   size="10"></asp:TextBox></td>
            <th>購買額：</th><td><asp:TextBox CssClass="number" runat="server" ID="PURS_AMT2" ReadOnly="true"   size="10"></asp:TextBox></td>
            <th>加權平均T/R：</th><td><asp:TextBox CssClass="number" runat="server" ID="TR2"  ReadOnly="true"   size="7"></asp:TextBox></td>
            <th>契約餘額：</th><td><asp:TextBox CssClass="number" runat="server" ID="CON_SUR_AMT2"  ReadOnly="true"   size="10"></asp:TextBox></td>
            <th>毛收益：</th><td><asp:TextBox CssClass="number" runat="server" ID="MARG2"  ReadOnly="true"   size="7"></asp:TextBox></td>
        </tr>
        <tr><td><h5>集團總計</h5></td>         
            <th>契約總額：</th><td><asp:TextBox CssClass="number" runat="server" ID="CON_TOL3" ReadOnly="true"   size="10"></asp:TextBox></td>
            <th>購買額：</th><td><asp:TextBox CssClass="number" runat="server" ID="PURS_AMT3" ReadOnly="true"   size="10"></asp:TextBox></td>
            <th>加權平均T/R：</th><td><asp:TextBox CssClass="number" runat="server" ID="TR3"  ReadOnly="true"   size="7"></asp:TextBox></td>
            <th>契約餘額：</th><td><asp:TextBox CssClass="number" runat="server" ID="CON_SUR_AMT3"  ReadOnly="true"   size="10"></asp:TextBox></td>
            <th>毛收益：</th><td><asp:TextBox CssClass="number" runat="server" ID="MARG3"  ReadOnly="true"   size="7"></asp:TextBox></td>
        </tr>
        <tr><td><h5>總計</h5></td> 
            <th>契約總額：</th><td><asp:TextBox CssClass="number" runat="server" ID="CON_TOL" ReadOnly="true"   size="10"></asp:TextBox></td>
            <th>購買額：</th><td><asp:TextBox CssClass="number" runat="server" ID="PURS_AMT" ReadOnly="true"   size="10"></asp:TextBox></td>
            <th>加權平均T/R：</th><td><asp:TextBox CssClass="number" runat="server" ID="TR"  ReadOnly="true"   size="7"></asp:TextBox></td>
            <th>契約餘額：</th><td><asp:TextBox CssClass="number" runat="server" ID="CON_SUR_AMT"  ReadOnly="true"   size="10"></asp:TextBox></td>
            <th>毛收益：</th><td><asp:TextBox CssClass="number" runat="server" ID="MARG"  ReadOnly="true"   size="7"></asp:TextBox></td>           
        </tr>
    </table>       
                    
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="functionBar" runat="server">
<% if (this.bolWE020)
   {%>
 <div class="divButton">
    <input type="button" ID="btnExit" class="button exit" value="返回" OnClick="contentChange('frameContent');" />    
</div>   
<% }%>
</asp:Content>


