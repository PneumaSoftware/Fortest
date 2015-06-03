<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/empty.Master" CodeBehind="WE090.aspx.cs" Inherits="OrixMvc.WE090" %>
<%@ MasterType VirtualPath="~/Pattern/empty.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
        
<asp:Content ContentPlaceHolderID="editingArea" ID="myEditingArea" runat="server">
<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlForm" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand=
    " exec s_WE090_Form @OBJ_CODE   "
    runat="server" >   
   <SelectParameters>
    <asp:ControlParameter ControlID="OBJ_CODE" Name="OBJ_CODE" PropertyName="Text" ConvertEmptyStringToNull="true" />
   </SelectParameters>
</asp:sqldatasource>


<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlGrid" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand=
    " exec s_WE090_Grid @OBJ_CODE    "
    runat="server" >   
   <SelectParameters>
    <asp:ControlParameter ControlID="OBJ_CODE" Name="OBJ_CODE" PropertyName="Text" ConvertEmptyStringToNull="true" />
   </SelectParameters>
</asp:sqldatasource>


    
<table >
    <tr>
        <th>標的物代號：</th><td><asp:TextBox runat="server" ID="OBJ_CODE" ReadOnly="true" CssClass="display" size="20" ></asp:TextBox></td>
<asp:Repeater runat="server" ID="rptForm" DataSourceID="sqlForm">
    <ItemTemplate>
    
        <th>品名：</th><td  colspan="3"><asp:TextBox runat="server" ID="PROD_NAME" ReadOnly="true" CssClass="display" Text='<%# Eval("PROD_NAME")%>' size="30"></asp:TextBox></td>
    </tr>                        
    <tr>
        <th>規格：</th><td><asp:TextBox runat="server" ID="SPEC" ReadOnly="true" CssClass="display"  size="20" Text='<%# Eval("SPEC")%>'></asp:TextBox></td>
        <th>廠牌：</th><td colspan="3"><asp:TextBox runat="server" ID="BRAND" ReadOnly="true" CssClass="display"  Text='<%# Eval("BRAND")%>' size="30"></asp:TextBox></td>
    </tr>                        
    <tr>
        <th>機號：</th><td><asp:TextBox runat="server" ID="MAC_NO" ReadOnly="true" CssClass="display"  Text='<%# Eval("MAC_NO")%>' size="20"></asp:TextBox></td>
        <th>年份：</th><td><asp:TextBox runat="server" ID="YEAR" ReadOnly="true" CssClass="display"   Text='<%# Eval("YEAR")%>' size="4"></asp:TextBox></td>
        <th>車號：</th><td><asp:TextBox runat="server" ID="CAR_NO" ReadOnly="true" CssClass="display"   Text='<%# Eval("CAR_NO")%>' size="10"></asp:TextBox></td>
    </tr>                        
    <tr>
        <th>採購日期：</th><td><asp:TextBox runat="server" ID="PURS_DATE" ReadOnly="true" CssClass="display"  Text='<%# Eval("PURS_DATE")%>' size="12"></asp:TextBox></td>
        <td colspan="6" rowspan="5" style="vertical-align:top" >
        <div id="query" class="gridMain " style="width:440px;margin-top:0px">
        <div style="padding:0;margin:0; overflow-y:scroll;width:435px;height:120px" id="divGrid">
        <table cellpadding="0" cellspacing="0" style="width:99%" > 
            <thead>
                <tr>                
                    <th>配件名稱</th>                   
                </tr> 
            </thead>    
                <asp:Repeater runat="server" ID="rptGrid" DataSourceID="sqlGrid"> 
                    <ItemTemplate>
                        <tr >            
                            <td><%# Eval("ACCS_NAME")%></td> 
                        </tr>
                    </ItemTemplate> 
                </asp:Repeater>            
            </table>
        </div>     
    </div> 
        </td>
    </tr> 
    <tr>
        <th>購買價格：</th><td><asp:TextBox runat="server" ID="REAL_BUY_PRC" ReadOnly="true" CssClass="display number"  Text='<%# Eval("REAL_BUY_PRC","{0:###,###,###,##0}")%>'  size="12"></asp:TextBox></td>
    </tr>
    <tr>
        <th>頭期款：</th><td><asp:TextBox runat="server" ID="DEPS" ReadOnly="true" CssClass="display number" Text='<%# Eval("DEPS","{0:###,###,###,##0}")%>' size="12"></asp:TextBox></td>
    </tr>
    <tr>
        <th>發票號碼：</th><td><asp:TextBox runat="server" ID="INV_I_IB" ReadOnly="true" CssClass="display number" Text='<%# Eval("INV_I_IB")%>' size="12"></asp:TextBox></td>
    </tr>
    <tr>
        <th>發票金額：</th><td><asp:TextBox runat="server" ID="INV_AMT_I_IB" ReadOnly="true" CssClass="display number" Text='<%# Eval("INV_AMT_I_IB","{0:###,###,###,##0}")%>' size="12"></asp:TextBox></td>
    </tr>
    <tr>
        <th>自備率：</th><td><asp:TextBox runat="server" ID="SELF_RATE" ReadOnly="true" CssClass="display number" Text='<%# Eval("SELF_RATE")%>' size="12"></asp:TextBox></td>
        <th>供應商代號：</th>
        <td><asp:TextBox runat="server" ID="FRC_CODE" ReadOnly="true" CssClass="display"  size="10" Text='<%# Eval("FRC_CODE")%>' ></asp:TextBox></td>
        <td colspan="2"><asp:TextBox runat="server" ID="FRC_SNAME" ReadOnly="true" CssClass="display"  size="22" Text='<%# Eval("FRC_SNAME")%>' ></asp:TextBox></td>
        <th>營業單位：</th><td><asp:TextBox runat="server" ID="SALES_UNIT" ReadOnly="true" CssClass="display" Text='<%# Eval("SALES_UNIT")%>'  size="15"></asp:TextBox></td>
    </tr>                        
    <tr>
        <th>買回比率：</th><td><asp:TextBox runat="server" ID="BUY_RATE" ReadOnly="true" CssClass="display number" Text='<%# Eval("BUY_RATE")%>'  size="12"></asp:TextBox></td>
        <th>買回承諾人：</th><td><asp:TextBox runat="server" ID="BUY_PROMISE" ReadOnly="true" CssClass="display" Text='<%# Eval("BUY_PROMISE")%>' size="10"></asp:TextBox></td>        
        <th>維修：</th><td><asp:TextBox runat="server" ID="FIX" ReadOnly="true" CssClass="display"  size="13" Text='<%# Eval("FIX")%>'></asp:TextBox></td>
        <th>合約編號：</th><td><asp:TextBox runat="server" ID="CON_SEQ_NO" ReadOnly="true" CssClass="display" Text='<%# Eval("CON_SEQ_NO")%>' size="15"></asp:TextBox></td>
    </tr>                        
    <tr>
        <th>狀態：</th><td><asp:TextBox runat="server" ID="OBJ_STS" ReadOnly="true" CssClass="display" Text='<%# Eval("OBJ_STS")%>' size="20"></asp:TextBox></td>
        <th>所有權：</th><td colspan="3"><asp:TextBox runat="server" ID="OBJ_DUE_OWNER" ReadOnly="true" Text='<%# Eval("OBJ_DUE_OWNER")%>' CssClass="display"  size="20"></asp:TextBox></td>        
    </tr>                        
    <tr>
        <th>所在地：</th><td><asp:TextBox runat="server" ID="OBJ_LOC" ReadOnly="true" CssClass="display" Text='<%# Eval("OBJ_LOC")%>' size="12"></asp:TextBox></td>
        <th>所在地址：</th><td colspan="4"><asp:TextBox runat="server" ID="OBJ_LOC_ADDR" ReadOnly="true" Text='<%# Eval("OBJ_LOC_ADDR")%>' CssClass="display"  size="50"></asp:TextBox></td>        
    </tr>                        
    <tr>
        <th>收回日期：</th><td><asp:TextBox runat="server" ID="SEND_DATE" ReadOnly="true" CssClass="display"  Text='<%# Eval("SEND_DATE")%>' size="12"></asp:TextBox></td>
        <th>收回存放地址：</th><td colspan="4"><asp:TextBox runat="server" ID="SEND_ADDR" ReadOnly="true"  Text='<%# Eval("SEND_ADDR")%>' CssClass="display"  size="50"></asp:TextBox></td>        
    </tr>                        
    <tr>
        <th>出售日期：</th><td><asp:TextBox runat="server" ID="SELL_DATE" ReadOnly="true" CssClass="display"  Text='<%# Eval("SELL_DATE")%>' size="12"></asp:TextBox></td>
        <th>出售金額：</th><td colspan="2"><asp:TextBox runat="server" ID="SELL_PRICE" ReadOnly="true" CssClass="display number" Text='<%# Eval("SELL_PRICE","{0:###,###,###,##0}")%>' size="20"></asp:TextBox></td>
        <th>出售對像：</th><td colspan="2"><asp:TextBox runat="server" ID="SELL_BUYER" ReadOnly="true" CssClass="display" Text='<%# Eval("SELL_BUYER")%>'   size="26"></asp:TextBox></td>
    </tr>
    <tr>
        <th>出售備註：</th><td colspan=7><asp:TextBox runat="server" ID="SELL_REMARK" ReadOnly="true" CssClass="display"   Text='<%# Eval("SELL_REMARK")%>' size="121"></asp:TextBox></td>
       </ItemTemplate> 
</asp:Repeater>     
    </tr>                            
</table>

<div class="divButton" style="float:right">
    <input type="button" ID="btnExit" class="button exit" value="返回" OnClick="contentChange('frameContent');" />    
</div> 
</asp:Content>

