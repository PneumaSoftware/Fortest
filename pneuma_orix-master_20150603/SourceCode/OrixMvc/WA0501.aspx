<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/editing.Master" CodeBehind="WA0501.aspx.cs" Inherits="OrixMvc.WA0501" %>
<%@ MasterType VirtualPath="~/Pattern/editing.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxYM"  TagPrefix="ocxControl" Src="~/ocxControl/ocxYM.ascx" %>
<%@ Register TagName="ocxYear"  TagPrefix="ocxControl" Src="~/ocxControl/ocxYear.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxTime"  TagPrefix="ocxControl" Src="~/ocxControl/ocxTime.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
        
<asp:Content ContentPlaceHolderID="editingArea" runat="server" ID="myEditArea">

<table style="margin:10px" cellpadding="2">
    <tr>            
        <th>主約編號：</th>
        <td colspan="2"><asp:TextBox runat="server" ID="MAST_CON_NO"  CssClass="display" ReadOnly="true" Text='<%#Eval("MAST_CON_NO") %>'  Width="150"></asp:TextBox></td>
    </tr>
    <tr>
        <th class="nonSpace">申請日期：</th>
        <td><ocxControl:ocxDate runat="server" ID="APLY_DATE"  Text='<%#Eval("APLY_DATE") %>' /></td>  
        <th class="nonSpace">案件類別：</th>
        <td><ocxControl:ocxDialog runat="server" ID="CASE_TYPE_CODE" width="60" Text='<%# Eval("CASE_TYPE_CODE") %>'   ControlID="CASE_TYPE_NAME" FieldName="CASE_TYPE_NAME" SourceName="OR_CASE_TYPE"  /></td>
            <td><input type="text" readonly class="display" style="width:150px" value='<%# Eval("CASE_TYPE_NAME") %>' id="CASE_TYPE_NAME" /></td>       
        <th >目前狀況：</th>
        <td><asp:TextBox runat="server" ID="CUR_STS"  CssClass="display" ReadOnly="true"  Width="100" Text='<%# Eval("CUR_STS") %>' ></asp:TextBox></td>      
    </tr> 
    <tr>
        <th class="nonSpace">部門代號：</th>
        <td><ocxControl:ocxDialog runat="server" ID="DEPT_CODE" width="80" Text='<%# Eval("DEPT_CODE") %>'   ControlID="DEPT_NAME" FieldName="DEPT_NAME" SourceName="OR_DEPT"  />  </td> 
        <th>部門名稱：</th>
        <td colspan="2"><input type="text" readonly class="display" style="width:100px" value='<%# Eval("DEPT_NAME") %>' id="DEPT_NAME" /></td>
        <th>核准日期：</th>
        <td><asp:TextBox runat="server" ID="APRV_DATE"  CssClass="display" ReadOnly="true"  Width="100"  Text='<%#Eval("APRV_DATE") %>'></asp:TextBox></td>  
    </tr>
    <tr>
        <th class="nonSpace">員工代號：</th>
        <td><ocxControl:ocxDialog runat="server" ID="EMP_CODE" Text='<%# Eval("EMP_CODE") %>' width="80" ControlID="EMP_NAME" FieldName="EMP_NAME" SourceName="OR_EMP" /></td>            
        <th>員工姓名：</th>
        <td colspan="2"><input type="text" readonly class="display" style="width:70px" value='<%# Eval("EMP_NAME") %>' id="EMP_NAME" /></td>
        <th class="nonSpace">預計到期日期：</th>
        <td><ocxControl:ocxDate runat="server" ID="PRE_EXPIRY_DATE"  Text='<%#Eval("PRE_EXPIRY_DATE") %>' /></td>  
    </tr>
    <tr>
        <th class="nonSpace">客戶代號：</th>
        <td><ocxControl:ocxDialog runat="server" ID="CUST_NO" width="80" Text='<%# Eval("CUST_NO")%>' ControlID="CUST_NAME" FieldName="CUST_NAME" SourceName="OR_CUSTOM" /></td>
        <th>客戶名稱：</th>
        <td colspan="2"><input type="text" readonly class="display" style="width:220px" value='<%# Eval("CUST_NAME") %>' id="CUST_NAME" /> </td>
        <th>實際失效日期：</th>
        <td><asp:TextBox runat="server" ID="EXPIRY_DATE"  CssClass="display" ReadOnly="true"  Width="100" Text='<%#Eval("EXPIRY_DATE") %>'></asp:TextBox></td>  
    </tr>
    <tr>         
        <th>簽呈號碼：</th>
        <td colspan="2"><asp:TextBox runat="server" ID="SIGNED_NO"   Width="150"  Text='<%#Eval("SIGNED_NO") %>'></asp:TextBox></td>  
        <td colspan="2"><ocxControl:ocxUpload ID="FILE_SEQ" runat="server" Seq='<%#Eval("FILE_SEQ") %>'  ></ocxControl:ocxUpload></td> 
    </tr>
</table> 

</asp:Content>
<asp:Content ContentPlaceHolderID="dataArea" runat="server" ID="myDataArea">
<asp:UpdatePanel runat="server" ID="upCustom" RenderMode="Inline" UpdateMode="Conditional">

<ContentTemplate>
         <asp:Repeater runat="server" ID="rptBaseCustom">
            <ItemTemplate>            
    <table style="width:810px; background-color:#E3E3E3">
        <tr>            
            <th>負責人：</th>
            <td><div style="width:100px" class="text"><%# Eval("TAKER").ToString().Trim() %></div></td>
            <th>設立日期：</th>
            <td><asp:TextBox runat="server"  ID="BUILD_DATE" CssClass="display" ReadOnly="true"  Width="100"  MaxLength="20" Text='<%# Eval("BUILD_DATE").ToString().Trim() %>'></asp:TextBox></td>
            <th>客戶簡介：</th>
            <td rowspan="8" style="vertical-align:top">                
                <textarea style="width:250px; background-color:Transparent" cols="51" readonly=readonly class="display" rows="12" ><%# Eval("BACKGROUND").ToString().Trim()%></textarea>
            </td>
        </tr>
        <tr>            
            <th>營業登記地址：</th>
            <td colspan="3"><div style="width:250px" class="text"><%# Eval("SALES_RGT_ADDR").ToString().Trim()%></div></td>            
        </tr>
        <tr>            
            <th>資本結構：</th>
            <td><div style="width:120px" class="text"><%# Eval("CAPT_STR").ToString().Trim()%></div></td>            
            <th>組織型態：</th>
            <td><div style="width:120px" class="text"><%# Eval("ORG_TYPE").ToString().Trim()%></div></td>
        </tr> 
        <tr>            
            <th>登記資本額：</th>
            <td><div style="width:120px" class="text number"><%# Eval("RGT_CAPT_AMT","{0:###,###,###,##0}") %></div>萬元</td>            
            <th>員工人數：</th>
            <td><div style="width:120px" class="text number"><%# Eval("EMP_PSNS", "{0:###,###,###,##0}")%></div></td>
        </tr> 
        <tr>            
            <th>實收資本額：</th>
            <td><div style="width:120px" class="text number"><%# Eval("REAL_CAPT_AMT", "{0:###,###,###,##0}")%></div>萬元</td>
        </tr>       
        <tr>           
            <th>主要營業項目：</th>
            <td colspan="3"><div style="width:180px" class="text"><%# Eval("MAIN_BUS_ITEM").ToString().Trim()%></div></td>            
        </tr>      
        
    </table>
            </ItemTemplate>
        </asp:Repeater>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnReload" />
        </Triggers>
     </asp:UpdatePanel>   
     <span style="display:none">
     <asp:Button runat="server" ID="btnReload" OnClick="Reload_Custom" />
     </span>             
</asp:Content>


<asp:Content ID="myPopWindow" ContentPlaceHolderID="PopWindow" runat="server">
     

     
<script language="javascript" type="text/javascript">

    var old=document.getElementById("<%=this.CUST_NO.ClientID %>").value;
    function dialogChange(sid, val) {

        if (sid.indexOf("CUST_NO") == -1)
            return;

        if ( val != old) {            
            document.getElementById("<%=this.btnReload.ClientID %>").click();
        }

        old = val;
    }
    function openDetail(strDate, strType, strName) {

    
        window.parent.openPopUpWindow();
    }
</script>
</asp:Content> 
