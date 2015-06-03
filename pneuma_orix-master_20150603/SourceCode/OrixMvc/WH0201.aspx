<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/editing.Master" CodeBehind="WH0201.aspx.cs" Inherits="OrixMvc.WH0201" %>
<%@ MasterType VirtualPath="~/Pattern/editing.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxTime"  TagPrefix="ocxControl" Src="~/ocxControl/ocxTime.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
        
<asp:Content ContentPlaceHolderID="editingArea" ID="mySearchArea" runat="server">
<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlAUTH_TYPE" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="select '' AS TypeCode,'' as TypeDesc union all select TypeCODE=[級別],TypeDesc=[級別] from [or3_授權類別] "
    runat="server" >
</asp:sqldatasource>

<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlMemo" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="select '' AS TypeCode,'' as TypeDesc,ORDER_SEQ=0 union all  select TypeCode=MEMO,TypeDesc=Memo,ORDER_SEQ FROM OR3_業績備註 order by 3"
    runat="server" >
</asp:sqldatasource>

<asp:SqlDataSource EnableViewState="true" SelectCommandType="Text"
id="sqlMachine"
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="SELECT [機具別] as TypeDesc,[代號] as TypeCode FROM [OR3_機具分類表] order by [代號]"
    runat="server" >
</asp:SqlDataSource>


    <table>
         <tr>
            <th class="nonSpace">日期：</th>
            <td><ocxControl:ocxDate runat="server" ID="DAY" Text='<%# Eval("DAY") %>' /></td> 
            <th>契約額：</th>
            <td><ocxControl:ocxNumber runat="server" ID="TOTAL" MASK="999,999,999" Text='<%# Eval("TOTAL") %>' /></td>   
             <th>資金成本：</th>
            <td><ocxControl:ocxNumber runat="server" ID="FUND" MASK="999,999,999" Text='<%# Eval("FUND") %>' /></td>              
        </tr>
        <tr>
            <th >件數：</th>
            <td><asp:DropDownList runat="server" ID="CODE" OnPreRender='checkList' ToolTip='<%# Eval("CODE") %>'>
                <asp:ListItem Value="0">0</asp:ListItem>
                <asp:ListItem Value="1">1</asp:ListItem>
                <asp:ListItem Value="-1">-1</asp:ListItem>
                </asp:DropDownList></td> 
            <th class="nonSpace">營業人員：</th>
            <td><ocxControl:ocxDialog runat="server" ID="SALES"  width="60" ControlID="EMP_NAME" FieldName="SALES1" SourceName="SALES" Text='<%# Eval("SALES") %>' /></td>            
            <td><asp:TextBox runat="server" ID="EMP_NAME" CssClass="display" ReadOnly="true"   size="14" Text='<%# Eval("EMP_NAME") %>'></asp:TextBox></td>
            <th>特殊報表排除：</th>
            <td><asp:DropDownList runat="server" ID="EXCULDE" OnPreRender='checkList' ToolTip='<%# Eval("EXCULDE") %>'>
                <asp:ListItem Value="Y">是</asp:ListItem>
                <asp:ListItem Value="N">否</asp:ListItem>                
                </asp:DropDownList>
            </td>             
        </tr>
        <tr>
            <th class="nonSpace">契約編號：</th>
            <td><asp:TextBox runat="server" ID="NUM"   size="15"  Text='<%# Eval("NUM") %>'></asp:TextBox></td> 
            <th >SHARE：</th>
            <td><ocxControl:ocxDialog runat="server" ID="SHARE"  width="60" ControlID="SHARE_NAME" FieldName="SALES1" SourceName="SALES" Text='<%# Eval("SHARE") %>'/></td>            
            <td><asp:TextBox runat="server" ID="SHARE_NAME" CssClass="display" ReadOnly="true"   size="14" Text='<%# Eval("SHARE_NAME") %>'></asp:TextBox></td>
        </tr>
        <tr>
            <th class="nonSpace">客戶名稱：</th>
            <td><asp:TextBox runat="server" ID="CUSTOMER"   size="15"  Text='<%# Eval("CUSTOMER") %>'></asp:TextBox></td>
            <th>SHARE比例：</th>
            <td><ocxControl:ocxNumber runat="server" ID="S_R" MASK="99.9" Text='<%# Eval("S_R") %>' />%</td>
        </tr>

     <tr>
            <th >授權類別：</th>
            <td><asp:DropDownList runat="server" ID="授權類別" DataSourceID="sqlAUTH_TYPE"  OnPreRender='checkList' ToolTip='<%# Eval("授權類別") %>' DataValueField="TypeCode" DataTextField="TypeDesc">                
                </asp:DropDownList></td> 
     <th>契約額(含頭款)：</th>
            <td><ocxControl:ocxNumber runat="server" ID="TAL" MASK="999,999" Text='<%# Eval("TAL") %>' /></td>              
        </tr>
      <tr>
            <th>案件類別：</th>
            <td><asp:DropDownList runat="server" ID="ST" OnPreRender='checkList' ToolTip='<%# Eval("ST") %>'>
                <asp:ListItem Value=""></asp:ListItem>
                <asp:ListItem Value="融">融</asp:ListItem>
                <asp:ListItem Value="應">應</asp:ListItem>
                </asp:DropDownList></td> 
            <th>機具別：</th>
            <td><asp:DropDownList runat="server" ID="MACHINE" OnPreRender='checkList' ToolTip='<%# Eval("MACHINE") %>'  DataSourceID="sqlMACHINE"  DataValueField="TypeCode" DataTextField="TypeDesc">
                <asp:ListItem Value=""></asp:ListItem>
                </asp:DropDownList></td>     
                
        </tr>                                                  
        <tr>
            <th>期數：</th>
            <td><ocxControl:ocxNumber runat="server" ID="TERM" MASK="999" Text='<%# Eval("TERM") %>' /></td>
             <th>標的物名稱：</th>
            <td colspan="3"><asp:TextBox runat="server" ID="標的物名稱" Text='<%# Eval("標的物名稱") %>'  size="40"></asp:TextBox></td>               
        </tr>
         <tr>
            <th>TR：</th>
            <td><ocxControl:ocxNumber runat="server" ID="TR" MASK="9,999" Text='<%# Eval("TR") %>' /></td>
            <th>客戶資本額：</th>
            <td><ocxControl:ocxNumber runat="server" ID="CAPITA" MASK="999,999,999" Text='<%# Eval("CAPITA") %>' /></td>              
        </tr>
        <tr>
            <th>購買額：</th>
            <td><ocxControl:ocxNumber runat="server" ID="COST" MASK="999,999,999" Text='<%# Eval("CAPITA") %>' /></td>              
            <th>DSCRPY_ME：</th>
            <td><asp:TextBox runat="server" ID="DSCRPY_ME" size=12 Text='<%# Eval("DSCRPY_ME") %>' ></asp:TextBox></td>                              
        </tr>
        <tr>
            <th>備註：</th>
            <td colspan="4">
            <asp:DropDownList runat="server" ID="MEMO" DataSourceID="sqlMemo"  OnPreRender='checkList' ToolTip='<%# Eval("MEMO") %>' DataValueField="TypeCode" DataTextField="TypeDesc">                
                </asp:DropDownList></td> 
            
            </td>             
        </tr>
        <tr>
            <th>新戶：</th>
            <td colspan="3"><asp:TextBox runat="server" ID="DSCRPY" Text='<%# Eval("DSCRPY") %>'  size="60"></asp:TextBox></td>             
        </tr>
        <tr>
             <th>新行業代號：</th>
            <td colspan="3">
                <asp:TextBox runat="server" ID="TextBox2"  CssClass="display" ReadOnly="true"  size="10"></asp:TextBox>
                <asp:TextBox runat="server" ID="TextBox1"  CssClass="display" ReadOnly="true"  size="30"></asp:TextBox>
            </td>                     
        </tr>
        </table>
</asp:Content>



<asp:Content ID="myPopWindow" ContentPlaceHolderID="PopWindow" runat="server">
 
  

</asp:Content> 
