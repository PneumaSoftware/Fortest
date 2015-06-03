<%@ Page Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/editing.Master" CodeBehind="WE0301.aspx.cs" Inherits="OrixMvc.WE0301" %>

<%@ MasterType VirtualPath="~/Pattern/editing.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxYM"  TagPrefix="ocxControl" Src="~/ocxControl/ocxYM.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxTime"  TagPrefix="ocxControl" Src="~/ocxControl/ocxTime.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
        
<asp:Content ContentPlaceHolderID="editingArea" ID="mySearchArea" runat="server">

<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlPAY_MTHD" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'PAY_MTHD' "
    runat="server" >
</asp:sqldatasource> 

<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlAPART" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="        
       select ORD_NO,CUST_NO,CUST_NAME,RECV_DEPT,RECV_NAME,ADDR from OR_INV_APART_SET where APLY_NO=@APLY_NO"
    runat="server" >
    <SelectParameters>
    <asp:ControlParameter ControlID="APLY_NO" PropertyName="Text" Name="APLY_NO" />
    </SelectParameters>
</asp:sqldatasource> 

<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlOBJECT" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="        
       select b.OBJ_CODE,PROD_NAME,OBJ_LOC_ADDR,OBJ_LOC_CTAC,OBJ_LOC_TEL,OBJ_LOC_FAX FROM OR_CASE_APLY_OBJ a left join OR_OBJECT b on a.OBJ_CODE=b.OBJ_CODE 
        where APLY_NO=@APLY_NO"
    runat="server" >
    <SelectParameters>
    <asp:ControlParameter ControlID="APLY_NO" PropertyName="Text" Name="APLY_NO" />
    </SelectParameters>
</asp:sqldatasource>


<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlMERGE" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="        
        SELECT a,MERGE_NO,RECVER,RECVERDEP,POST_NUM,RECVERADDR FROM OR_CASE_INV_MERGE_SET a left join OR_CASE_INV_MERGE b on a.MERGE_NO=b.MERGE_NO
        where APLY_NO=@APLY_NO"
    runat="server" >
    <SelectParameters>
    <asp:ControlParameter ControlID="APLY_NO" PropertyName="Text" Name="APLY_NO" />
    </SelectParameters>
</asp:sqldatasource>
<table cellpadding="0"  cellspacing="0">
    <tr><td valign="top">
     <table style="margin-top:15px">
        <tr>
            <th>申請書編號：</th>
            <td ><asp:TextBox runat="server" ID="APLY_NO" Text='<%# Eval("APLY_NO") %>'  Width="150" CssClass="display" ReadOnly="true"  MaxLength="50" ></asp:TextBox></td>
       </tr>
       <tr>
            <th>Orix 處理人員：</th>
            <td><ocxControl:ocxDialog runat="server" ID="EMP_CODE" Value='<%# Eval("EMP_CODE") %>' width="100" ControlID="EMP_NAME" FieldName="EMP_NAME" SourceName="OR_EMP" /></td>
            <td><asp:TextBox  runat="server" ID="EMP_NAME"  size="10"  MaxLength="10"  CssClass="display" ReadOnly="true" Text='<%# Eval("EMP_NAME") %>'></asp:TextBox></td>
       </tr>
       <tr>
            <th>客戶代號：</th>
            <td ><asp:TextBox runat="server" ID="CUST_NO" Text='<%# Eval("CUST_NO") %>'  Width="150" CssClass="display" ReadOnly="true"  MaxLength="50" ></asp:TextBox></td>
       </tr>
        <tr>
            <th>客戶名稱：</th>
            <td colspan="2" ><asp:TextBox runat="server" ID="CUST_NAME" Text='<%# Eval("CUST_NAME") %>'  Width="300" CssClass="display" ReadOnly="true"  MaxLength="50" ></asp:TextBox></td>
            <td><input type="button" id="btnAddFRC" class="button func"  style="width:50px"  value="客戶基本資料異動" onclick="window.parent.openPopUpWindow();" /> </td>
       </tr>
       <tr>
            <th >聯絡人：</th>
            <td ><asp:TextBox runat="server" ID="CONTACT"  size="10"  MaxLength="12" Text='<%# Eval("CONTACT") %>'></asp:TextBox></td>
            <th >聯絡電話：</th>
            <td ><asp:TextBox runat="server" ID="CTAC_TEL"  size="15"  MaxLength="20" Text='<%# Eval("CTAC_TEL") %>'></asp:TextBox></td>
            <th >傳真：</th>
            <td ><asp:TextBox runat="server" ID="FAX"  size="15"  MaxLength="20" Text='<%# Eval("FAX") %>'></asp:TextBox></td>
       </tr>
       <tr>
            <th>繳款方式：</th>
            <td><asp:DropDownList  runat="server" ID="APRV_PAY_MTHD" OnPreRender='checkList' ToolTip='<%# Eval("PAY_MTHD") %>' DataSourceID="sqlPAY_MTHD"  DataValueField="TypeCode" DataTextField="TypeDesc" ></asp:DropDownList></td>
            <th>手機：</th>
            <td ><asp:TextBox runat="server" ID="MOBILE"  size="15"  MaxLength="20" Text='<%# Eval("MOBILE") %>'></asp:TextBox></td>
       </tr> 
       <tr>
            <th >付款聯絡人：</th>
            <td ><asp:TextBox runat="server" ID="PAY_CTAC"  size="10"  MaxLength="12" Text='<%# Eval("PAY_CTAC") %>'></asp:TextBox></td>
            <th >付款聯絡電話：</th>
            <td ><asp:TextBox runat="server" ID="PAY_TEL"  size="15"  MaxLength="20" Text='<%# Eval("PAY_TEL") %>'></asp:TextBox></td>     
    </table>
    </td>
    </tr>
    <tr>
    <td >

    <h3>標的物存放地</h3>
    <div  class="gridMain" >
        <div style="padding:0;margin:0; overflow-y:scroll;width:800px;height:200px; position:relative" id="editGrid">
            <table cellpadding="0" cellspacing="0"  style="width:780px" >             
                <tr>      
                    <th class="fixCol">標的物代號</th>          
                    <th class="fixCol">品名</th>
                    <th>標的物所在地址</th>
                    <th>標的物所在連絡人</th>
                    <th>標的物所在電話</th>                            
                    <th>標的物所在傳真</th>                            
                </tr>             
                <asp:Repeater runat="server" ID="rptOBJ" DataSourceID="sqlObject"  > 
                    <ItemTemplate>
                        <tr> 
                            <td class="fixCol"><asp:TextBox runat="server" ID="OBJ_CODE" Text='<%# Eval("OBJ_CODE") %>' MaxLength="20" CssClass="display" ReadOnly="true"  ></asp:TextBox></td>
                            <td class="fixCol"><asp:TextBox runat="server" ID="PROD_NAME" Text='<%# Eval("PROD_NAME") %>' MaxLength="20" CssClass="display" ReadOnly="true"  ></asp:TextBox></td>
                            <td><asp:TextBox runat="server" ID="OBJ_LOC_ADDR" Text='<%# Eval("OBJ_LOC_ADDR") %>' MaxLength="100" Width="200"   ></asp:TextBox></td>
                            <td><asp:TextBox runat="server" ID="OBJ_LOC_CTAC" Text='<%# Eval("OBJ_LOC_CTAC") %>' MaxLength="10" Width="100" ></asp:TextBox></td>
                            <td><asp:TextBox runat="server" ID="OBJ_LOC_TEL" Text='<%# Eval("OBJ_LOC_TEL") %>' MaxLength="20" Width="150"  ></asp:TextBox></td>
                            <td><asp:TextBox runat="server" ID="OBJ_LOC_FAX" Text='<%# Eval("OBJ_LOC_FAX") %>' MaxLength="20" Width="150" ></asp:TextBox></td>
                        </tr>
                    </ItemTemplate> 
                </asp:Repeater>  
            </table>
        </div>     
    </div> 
   
    </td>
    </tr>           
    <tr>
    <td>
    <table >
        <tr>
            <th>統一郵寄編號：</th>
            <td ><asp:TextBox runat="server" ID="MMAIL_NO" Text='<%# Eval("MMAIL_NO") %>'  Width="70" MaxLength="10" ></asp:TextBox></td>
       </tr>
    </table> 
    </td> 
    </tr>
    <tr>
    <td style='display:<%=this.rptMERGE.Items.Count>0:"none":""%>' >    
    
    <h3>發票地/ 收件人/ 部門 (單張/ 多張)</h3>
    <div  class="gridMain " >
        <div  style="padding:0;margin:0; overflow-y:scroll;width:800px;height:200px">
            <table cellpadding="0" cellspacing="0"  style="width:780px" > 
                <tr>      
                    <th>順序號</th>          
                    <th>客戶代號</th>
                    <th>客戶名稱</th>
                    <th>部門</th>
                    <th>收件人</th>                            
                    <th>地址</th>                            
                </tr> 
                <asp:Repeater runat="server" ID="rptAPART" DataSourceID="sqlAPART"  > 
                    <ItemTemplate>
                        <tr> 
                            <td><asp:TextBox runat="server" ID="ORD_NO" Text='<%# Eval("ORD_NO") %>' MaxLength="20" CssClass="display" ReadOnly="true"  ></asp:TextBox></td>
                            <td><asp:TextBox runat="server" ID="CUST_NO" Text='<%# Eval("CUST_NO") %>' MaxLength="20" CssClass="display" ReadOnly="true"  ></asp:TextBox></td>
                            <td><asp:TextBox runat="server" ID="CUST_NAME" Text='<%# Eval("CUST_NAME") %>' MaxLength="100" Width="300" CssClass="display" ReadOnly="true"  ></asp:TextBox></td>
                            <td><asp:TextBox runat="server" ID="REC_DEPT" Text='<%# Eval("REC_DEPT") %>' MaxLength="10" Width="100" ></asp:TextBox></td>
                            <td><asp:TextBox runat="server" ID="RECV_NAME" Text='<%# Eval("RECV_NAME") %>' MaxLength="20" Width="150"  ></asp:TextBox></td>
                            <td><asp:TextBox runat="server" ID="ADDR" Text='<%# Eval("ADDR") %>' MaxLength="100" Width="200" ></asp:TextBox></td>
                        </tr>
                    </ItemTemplate> 
                </asp:Repeater>  
            </table>
        </div>     
    </div> 
   
    </td>
    </tr>   
     <tr>
    <td >
    
    <h3>發票地/ 收件人/ 部門 (合併開)</h3>
    <asp:Repeater runat="server" ID="rptMERGE" DataSourceID="sqlMERGE"  > 
                    <ItemTemplate>
    <table >
        <tr>
            <th>MERGE_NO：</th>
            <td ><asp:TextBox runat="server" ID="MERGE_NO" Text='<%# Eval("MERGE_NO") %>' CssClass="display" ReadOnly="true"  Width="70" MaxLength="10" ></asp:TextBox></td>
            <th>收件人：</th>
            <td ><asp:TextBox runat="server" ID="RECVER" Text='<%# Eval("RECVER") %>' Width="70" MaxLength="10" ></asp:TextBox></td>
            <th>收件部門：</th>
            <td ><asp:TextBox runat="server" ID="RECVERDEP" Text='<%# Eval("RECVERDEP") %>' Width="100" MaxLength="10" ></asp:TextBox></td>
            <th>郵遞區號：</th>
            <td ><asp:TextBox runat="server" ID="POST_NUM" Text='<%# Eval("POST_NUM") %>'   Width="50" MaxLength="5" ></asp:TextBox></td>
       </tr>
       <tr>
            <th>地址：</th>
            <td colspan="8" ><asp:TextBox runat="server" ID="RECVERADDR" Text='<%# Eval("RECVERADDR") %>'   Width="50" MaxLength="5" ></asp:TextBox></td>
       </tr>
    </table>         
        </ItemTemplate>
    </asp:Repeater> 
    </td>
    </tr>
         
</table>            
          
          
  
    
                           
</asp:Content>


<asp:Content ID="myPopWindow" ContentPlaceHolderID="PopWindow" runat="server">
     

     
<script language="javascript" type="text/javascript">
    function openDetail(strDate, strType, strName) {

    
        window.parent.openPopUpWindow();
    }
</script>
</asp:Content> 
