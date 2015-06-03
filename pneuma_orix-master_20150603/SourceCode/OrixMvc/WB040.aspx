<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" ValidateRequest="false"   MasterPageFile="~/Pattern/display.Master" CodeBehind="WB040.aspx.cs" Inherits="OrixMvc.WB040" %>
<%@ MasterType VirtualPath="~/Pattern/display.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
        
<asp:Content ContentPlaceHolderID="searchArea" ID="mySearchArea" runat="server">
    <table >
        <tr> 
           <th class="nonSpace">客戶代號：</th>  <td><ocxControl:ocxDialog runat="server" ID="PCUST_NO" width="100" FieldName="CUST_SNAME" ControlID="PCUST_SNAME" SourceName="OR_CUSTOM" /></td>
            <td><asp:TextBox CssClass="display"  runat="server" ID="PCUST_SNAME"  MaxLength="25" width="150" ></asp:TextBox></td>
                                       
        </tr>                         
    </table>
</asp:Content>

<asp:Content ContentPlaceHolderID="editingArea" ID="myEditArea" runat="server">

                 
<asp:UpdatePanel runat="server" RenderMode="Inline" ID="upGrid1">
<ContentTemplate>

<div id="query" class="gridMain ">
    <div style="padding:0;margin:0; overflow-y:scroll;position:relative;width:800px;height:150px" id="divGrid">
        <table cellpadding="0" cellspacing="0" style="width:400px" > 
        <thead>
            <tr>                
                <th style="width:5%"></th>
                <th style="width:5%">額度申請書編號</th>
                <th style="width:15%">本次申請額度</th>
                <th style="width:20%">已使用額度</th>                
                <th style="width:30%">剩餘額度</th>
                <th style="width:25%">是否循環</th>
            </tr> 
        </thead>    
            <asp:Repeater runat="server" ID="rptQuery"> 
                <ItemTemplate>
                    <tr id='tr<%#Container.ItemIndex+1%>' class="<%#Container.ItemIndex%2==0?"srow":"" %>">     
                        <td><asp:UpdatePanel runat="server" ID="upDetail" RenderMode="Inline" UpdateMode="Conditional"> <ContentTemplate>
                           <asp:Button ID="btnDetail" runat="server"  cssclass="button func"  Text="明細" OnCommand="GridFunc_Click" CommandName='<%# Eval("QUOTA_APLY_NO").ToString().Trim()+","+ Eval("APRV_APPR_TYPE").ToString().Trim() %>' />
                           </ContentTemplate></asp:UpdatePanel>
                        </td>               
                        <td><%# Eval("QUOTA_APLY_NO") %></td>                         
                        <td class="number"><%# Eval("APRV_TOT_QUOTA", "{0:###,###,###,##0}")%></td>
                        <td class="number"><%# Eval("QUOTA_AMT", "{0:###,###,###,##0}")%></td>                   
                        <td class="number"><%# Eval("UNUSE_QUTA", "{0:###,###,###,##0}")%></td>                   
                        <td><%# Eval("APRV_APPR_TYPE")%></td>                        
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

<h4>額度內動撥申請書</h4>
<div id="query" class="gridMain " style="margin-top:0px">
    <div style="padding:0;margin:0; overflow-y:scroll;width:800px;height:270px" id="divGrid">
        <table cellpadding="0" cellspacing="0" style="width:500px" > 
        <thead>
            <tr>
                <th>客戶代號</th>
                <th>客戶簡稱</th>                
                <th>動用額度上限</th>                
                <th>申請書編號</th>
                <th>本次申請</th>
                <th>契約餘額</th>
                <th>剩餘額度</th>
            </tr> 
        </thead>    
            <asp:Repeater runat="server" ID="rptDetail"> 
                <ItemTemplate>
                    <tr class='<%# Eval("CUST_SNAME").ToString()!=""?"":"sub"%>' >                                   
                        <td colspan="2" style="display:<%# Eval("CUST_SNAME").ToString()!=""?"none":""%>">Sub Total:</td>                         
                        <td style="display:<%# Eval("CUST_SNAME").ToString()!=""?"":"none"%>"><%# Eval("CUST_NO")%></td>                         
                        <td style="display:<%# Eval("CUST_SNAME").ToString()!=""?"":"none"%>"><%# Eval("CUST_SNAME")%></td>
                        <td class="number"><%# Eval("CUST_SNAME").ToString()!=""?"":Eval("UPPER_LIMIT", "{0:###,###,###,##0}")%></td>                   
                        <td><%# Eval("Aply_no")%></td>                   
                        <td class="number"><%# Eval("L_THIS", "{0:###,###,###,##0}")%></td>                        
                        <td class="number"><%# Eval("CON_SUR", "{0:###,###,###,##0}")%></td>                        
                        <td class="number"><%# Eval("CUST_SNAME").ToString()!=""?"":Eval("UNUSE_QUTA", "{0:###,###,###,##0}")%></td>                        
                    </tr>
                </ItemTemplate> 
            </asp:Repeater>            
        </table>
    </div>     
</div> 
</ContentTemplate>
</asp:UpdatePanel> 
                            
</asp:Content>

