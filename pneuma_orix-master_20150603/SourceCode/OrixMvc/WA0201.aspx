<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/editing.Master" CodeBehind="WA0201.aspx.cs" Inherits="OrixMvc.WA0201" %>
<%@ MasterType VirtualPath="~/Pattern/editing.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxYM"  TagPrefix="ocxControl" Src="~/ocxControl/ocxYM.ascx" %>
<%@ Register TagName="ocxYear"  TagPrefix="ocxControl" Src="~/ocxControl/ocxYear.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxTime"  TagPrefix="ocxControl" Src="~/ocxControl/ocxTime.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
        

<asp:Content ContentPlaceHolderID="dataArea" runat="server" ID="myDataArea">
<table style="">
    <tr>            
    <th >申請書編號：</th>
    <td><asp:TextBox runat="server" ID="APLY_NO"  CssClass="display"   Width="150"></asp:TextBox></td>
    <th>先行出合約狀態：</th>
    <td><asp:TextBox runat="server" ID="FAST_STS"  CssClass="display"   Width="100"></asp:TextBox></td>
    </tr> 
</table> 

<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlCASE_SOUR" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'CASE_SOUR'"
    runat="server" >
</asp:sqldatasource> 

<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlPAY_PERD" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'PAY_PERD'"
    runat="server" >
</asp:sqldatasource> 

<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlPAY_MTHD" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'PAY_MTHD'"
    runat="server" >
</asp:sqldatasource> 

<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlAMOR_MTHD" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'AMOR_MTHD'"
    runat="server" >
</asp:sqldatasource> 

<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlSCUR_NATUR" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems'SCUR_NATUR'"
    runat="server" >
</asp:sqldatasource> 

<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlSCUR_RELATION" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="select TypeCode, TypeDesc from (select '' TypeCode,'' TypeDesc,seq=0  union all select toge_no TypeCode,toge_name TypeDesc,seq from or_common_code where toge_group='A01' and (end_date is null or end_date='')) s order by seq"
runat="server" >
</asp:sqldatasource> 


<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlCON_TYPE" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'CON_TYPE'"
    runat="server" >
</asp:sqldatasource> 



<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlTMP_CODE" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
      SelectCommand="
 select TMP_CODE='',TMP_DESC='請選擇...' union all select TMP_CODE,TMP_DESC from OR3_CONTRACT_TEMP_SET"
runat="server" >
</asp:sqldatasource> 




<script language="javascript" type="text/javascript">

function tab_select(intIndex,intLen)
{
    for (var i=1;i<=intLen;i++) 
    {
        document.getElementById("tab"+i.toString()).className="";
        document.getElementById("tabpanel"+i.toString()).style.display="none";
    }
    
    document.getElementById("tab"+intIndex.toString()).className="tabs-selected";
    document.getElementById("tabpanel"+intIndex.toString()).style.display="block";

}
</script> 

<asp:Panel runat="server" ID="myArea">
<div  style="width: 820px; height: auto;">
	<div class="tabs-header" style="width: 820px;"><div class="tabs-scroller-left" style="display: none;"></div>
		<div class="tabs-scroller-right" style="display: none;"></div><div class="tabs-wrap" style="margin-left: 0px; margin-right: 0px; width: 810px;">
			<ul class="tabs">
				<li class="tabs-selected" id="tab1" onclick="tab_select(1,2)"><a href="javascript:void(0)" class="tabs-inner"><span class="tabs-title">基本資料</span><span class="tabs-icon"></span></a></li>
				<li class=""  id="tab2" onclick="tab_select(2,2)"><a href="javascript:void(0)" class="tabs-inner"><span class="tabs-title">其他條件</span><span class="tabs-icon"></span></a></li>
			</ul>
		</div>
	</div>
	
    <div class="tabs-panels"  style="width: 820px;" >
    
        <div   class="panel" style="height: auto; width: 810px;display:block" id="tabpanel1">    
    <table style="margin:0px">     
        <asp:Repeater runat="server" ID="rptBase">
            <ItemTemplate> 
        <tr>            
            <th class="nonSpace">申請日期：</th>
            <td><ocxControl:ocxDate runat="server" ID="APLY_DATE" Text='<%# Eval("APLY_DATE").ToString().Trim() %>' /></td>
            <th class="nonSpace">案件類別：</th>
            <td><ocxControl:ocxDialog runat="server" ID="CASE_TYPE_CODE" width="60" Text='<%# Eval("CASE_TYPE_CODE") %>'   ControlID="CASE_TYPE_NAME" FieldName="CASE_TYPE_NAME" SourceName="OR_CASE_TYPE"  /></td>
            <td colspan="2"><input type="text" readonly class="display" style="width:140px" value='<%# Eval("CASE_TYPE_NAME").ToString().Trim() %>' id="CASE_TYPE_NAME" /></td>             
        </tr>
        <tr>            
            <th class="nonSpace">申請單位：</th>
            <td ><ocxControl:ocxDialog runat="server" ID="DEPT_CODE" width="90" Text='<%# Eval("DEPT_CODE").ToString().Trim() %>'   ControlID="DEPT_NAME" FieldName="DEPT_NAME" SourceName="OR_DEPT"  />  </td> 
            <td colspan="2"><input type="text" readonly class="display" style="width:140px" value='<%# Eval("DEPT_NAME").ToString().Trim() %>' id="DEPT_NAME" /></td>            
            <th class="nonSpace">經辦代號：</th>
            <td><ocxControl:ocxDialog runat="server" ID="EMP_CODE" Text='<%# Eval("EMP_CODE") %>' width="60" ControlID="EMP_NAME" FieldName="EMP_NAME" SourceName="OR_EMP" /></td>            
            <td><input type="text" readonly class="display" style="width:70px" value='<%# Eval("EMP_NAME").ToString().Trim() %>' id="EMP_NAME" /></td>
        </tr>
         <tr>            
            <th class="nonSpace">客戶代號：</th>
            <td><ocxControl:ocxDialog runat="server" ID="CUST_NO" width="90" Text='<%# Eval("CUST_NO").ToString().Trim() %>' ControlID="CUST_SNAME" FieldName="CUST_SNAME" SourceName="OR_CUSTOM" /></td>
            <td colspan="3"><input type="text" readonly class="display" style="width:140px" value='<%# Eval("CUST_SNAME").ToString().Trim() %>' id="CUST_SNAME" /> 
            <td><font class="memo" ><%# Eval("PROJECT") %></font></td>
            <th>主約編號：</th>
            <td><asp:TextBox runat="server"  ID="MAST_CON_NO"  Width="90"  MaxLength="20" Text='<%# Eval("MAST_CON_NO").ToString().Trim() %>' ></asp:TextBox></td>
        </tr>         
        <tr>            
            <th>負責人：</th>
            <td><div style="width:90px" class="text" id="TAKER"><%# Eval("TAKER").ToString().Trim() %></div></td>                        
            <th>營業登記地址：</th>
            <td colspan="4"><asp:TextBox runat="server" ID="SALES_RGT_ADDR" Width="250px" cssclass="display" size="10"  MaxLength="12" Text='<%# Eval("SALES_RGT_ADDR").ToString().Trim() %>'></asp:TextBox></td>            
        </tr>        
        
            </ItemTemplate> 
        </asp:Repeater>
         

        
    </table>   
    
                                                      
      <fieldset><legend>標的物</legend>
      <table cellpadding="0" cellspacing="2" style="margin:0px" >
      <tr><td valign="top">
            <asp:UpdatePanel runat="server" ID="upObjGrid" RenderMode="Inline" UpdateMode="Conditional">
                <ContentTemplate>
            <div  class="gridMain " style="width:465px;margin:0">
                <div style="padding:0;margin:0;  overflow-y:scroll;width:455px;height:90px" id="editGrid" >
                    <table cellpadding="0" cellspacing="0"  style="width:400px" > 
                    <thead>
                        <tr>      
                            <th>編輯</th>          
                            <th>標的物代號</th>
                            <th>標的物所在地址</th>                       
                        </tr> 
                    </thead>    
                        <asp:Repeater runat="server" ID="rptObjGrid"> 
                            <ItemTemplate>
                                <tr id='trObj<%#Container.ItemIndex+1%>' class="<%#Container.ItemIndex%2==0?"srow":"" %>"  > 
                                    <td>                                    
                                    <asp:Button ID="btnDel_Object" runat="server"  cssClass="button del"  Text="刪除" OnClientClick="return window.confirm('是否確定刪除此筆資料?');"  OnCommand="GridFunc_Click" CommandName='<%# Eval("OBJ_KEY").ToString().Trim() %>' />
                                    <asp:Button ID="btnUpd_Object" runat="server"  cssClass="button upd"  Text="修改"  OnCommand="GridFunc_Click" CommandName='<%# Eval("OBJ_KEY").ToString().Trim() %>' />
                                    </td>
                                    <td><%# Eval("OBJ_CODE")%></td>
                                    <td><%# Eval("OBJ_LOC_ADDR")%></td>
                                </tr>
                            </ItemTemplate> 
                        </asp:Repeater>                       
                        <tr id='trObj0'  >
                            <td colspan="3"><asp:Button ID="btnAdd_Object" runat="server"  cssClass="button dadd"  Text="新增" OnCommand="GridFunc_Click" CommandName="" /> </td>                                                
                        </tr>
                    </table>
                </div>     
            </div> 
                </ContentTemplate>
            </asp:UpdatePanel>
            </td>  
             <td valign="top">
            <asp:UpdatePanel runat="server" ID="upAccs" RenderMode="Inline" UpdateMode="Conditional"> 
                <ContentTemplate>
            <div  class="gridMain " style="width:245px;margin:0">
                <div style="padding:0;margin:0;  overflow-y:scroll;width:235px;height:90px" id="editGrid">
                    <table cellpadding="0" cellspacing="0"  style="width:160px" > 
                    <thead>
                        <tr>      
                            <th>編輯</th>          
                            <th>配件名稱</th>                            
                        </tr> 
                    </thead>    
                        <asp:Repeater runat="server" ID="rptAccs"> 
                            <ItemTemplate>
                                <tr  > 
                                    <td>                                    
                                    <asp:Button ID="btnDel_Accs" runat="server"  cssClass="button del"  Text="刪除"  OnCommand="GridFunc_Click" CommandName='<%# Eval("ACCS_SEQ").ToString().Trim() %>' /></td>
                                    <td><asp:TextBox runat="server" ID="ACCS_NAME"  size="20" width="150" Text='<%# Eval("ACCS_NAME") %>'></asp:TextBox></td>
                                </tr>
                            </ItemTemplate> 
                        </asp:Repeater>                       
                        <tr >
                            <td><asp:Button ID="btnAdd_Accs" runat="server"  cssClass="button dadd"  Text="新增" OnCommand="GridFunc_Click" CommandName="Add" /> </td>
                            <td><asp:TextBox runat="server" ID="addACCS_NAME"  size="20" width="150" Text='<%# Eval("ACCS_NAME") %>'></asp:TextBox></td>
                        </tr>
                    </table>
                </div>     
            </div> 
           
                </ContentTemplate>
            </asp:UpdatePanel>
            </td>         
            </tr>
        <tr><td colspan="2">      
         <asp:Repeater runat="server" ID="rptObjDetail">
            <ItemTemplate>   
            <asp:UpdatePanel runat="server" ID="upObjDetailM" RenderMode="Inline" UpdateMode="Conditional">
                <ContentTemplate>
        <table cellpadding="1" cellspacing="1" >
        <tr>  
            <th>舊案申請書編號：</th>            
            <td><ocxControl:ocxDialog SourceName="OLD_APLY_NO" runat="server" Text='<%# Eval("OLD_APLY_NO").ToString().Trim() %>' ID="OLD_APLY_NO" width="140" ControlID="" FieldName="OBJ_CODE,OBJ_LOC_ADDR,PROD_NAME,MAC_NO" /> </td> 
            <td colspan="5"><asp:CheckBox ID="NotInclude" runat="server" OnPreRender='checkList'  ToolTip='<%# Eval("NotInclude").ToString().Trim() %>'/>不帶入合約 </td>
           
        </tr>
        </table>      
        </ContentTemplate>
        </asp:UpdatePanel>
            <asp:UpdatePanel runat="server" ID="upObjDetail" RenderMode="Inline" UpdateMode="Conditional">
                <ContentTemplate>
                         
    <table>
        <tr>            
            <th class="nonSpace">標的物代號：</th>            
            <td><asp:TextBox runat="server"  ID="OBJ_CODE"  Width="120"  MaxLength="20" Text='<%# Eval("OBJ_CODE").ToString().Trim() %>'></asp:TextBox>     
            <span style="display:none">
            <asp:TextBox runat="server"  ID="OBJ_KEY"  Width="120"  MaxLength="20" Text='<%# Eval("OBJ_KEY").ToString().Trim() %>'></asp:TextBox> </span>
            </td> 
            <th class="nonSpace">品名：</th>
            <td colspan="3"><asp:TextBox runat="server"  ID="PROD_NAME"   Width="200"  MaxLength="200" Text='<%# Eval("PROD_NAME").ToString().Trim() %>'></asp:TextBox></td>             
            <td><asp:CheckBox runat="server" ID="OTC"  OnPreRender='checkList' ToolTip='<%# Eval("OTC").ToString().Trim() %>'/>OTC資產</td>
            <td rowspan="3" valign="bottom">
             <div class="divFunction80" style='float:right;'>
                <asp:Button runat="server" ID="btnEdit" CssClass="button trn80" Text="明細儲存"  Font-Bold="true" OnCommand="GridFunc_Click" CommandName="Save"   />
            </div> 
            </td>
       </tr>    
       <tr>
            <th>標的物地址：</th>            
            <td colspan="6"><asp:TextBox runat="server"  ID="OBJ_LOC_ADDR"  Width="350"  MaxLength="100" Text='<%# Eval("OBJ_LOC_ADDR").ToString().Trim() %>'></asp:TextBox></td>
       </tr>
       <tr>    
            <th>機號：</th>            
            <td><asp:TextBox runat="server"  ID="MAC_NO"   Width="120"  MaxLength="20" Text='<%# Eval("MAC_NO").ToString().Trim() %>'></asp:TextBox></td>  
            <th>廠牌：</th> 
            <td><asp:TextBox runat="server"  ID="BRAND"   Width="70"  MaxLength="20" Text='<%# Eval("BRAND").ToString().Trim() %>'></asp:TextBox></td> 
            <th>供應商：</th><td><ocxControl:ocxDialog runat="server" ID="FRC_CODE" width="70" Text='<%# Eval("FRC_CODE").ToString().Trim() %>' SourceName="OR_FRC"  ControlID="FRC_SNAME" FieldName="FRC_SNAME"/></td>
            <td ><input type="text" readonly class="display" style="width:100px" name="FRC_SNAME" value='<%# Eval("FRC_SNAME").ToString().Trim() %>' id="FRC_SNAME" /></td>  
            
        </tr> 
      
    </table>
            
                </ContentTemplate>
            </asp:UpdatePanel>     
            </ItemTemplate>
            </asp:Repeater>     
            </td>            
           </tr>
        </table> 
        
      </fieldset>      
     <table><tr><td>
     <fieldset><legend>申請條件</legend>
      <table cellspacing="0" cellpadding="0" border="0"  >
        <asp:Repeater runat="server" ID="rptRequest">
            <ItemTemplate>               
       
       <tr>            
            <th  >保證金：</th>            
            <td><ocxControl:ocxNumber runat="server" ID="APLY_BOND" MASK=",999,999" Text='<%# Eval("APLY_BOND").ToString().Trim() %>' /></td> 
        </tr>
        <tr>
            <th class="nonSpace">期間：</th>            
            <td><ocxControl:ocxNumber runat="server" ID="APLY_DURN_M" MASK="999" Text='<%# Eval("APLY_DURN_M").ToString().Trim() %>' />個月
                <ocxControl:ocxNumber runat="server" ID="APLY_PERD" MASK="999" Text='<%# Eval("APLY_PERD").ToString().Trim() %>' />期</td>
            <th >付款週期：</th>            
            <td><asp:DropDownList runat="server" ID="APLY_PAY_PERD"  OnPreRender='checkList' ToolTip='<%# Eval("APLY_PAY_PERD").ToString().Trim() %>' Width="80" DataSourceID="sqlPAY_PERD"  DataValueField="TypeCode" DataTextField="TypeDesc" ></asp:DropDownList></td>
       </tr>     
      
        <tr>
             <th>頭期款：</th> 
            <td><ocxControl:ocxNumber runat="server" ID="APLY_DEPS" MASK=",999,999" Text='<%# Eval("APLY_DEPS").ToString().Trim() %>' /></td> 
            <th class="nonSpace" >繳款方式：</th>            
            <td><asp:DropDownList runat="server" ID="APLY_PAY_MTHD"  OnPreRender='checkList' ToolTip='<%# Eval("APLY_PAY_MTHD").ToString().Trim() %>' Width="80" DataSourceID="sqlPAY_MTHD"  DataValueField="TypeCode" DataTextField="TypeDesc" ></asp:DropDownList></td>                       
        </tr>            
        <tr>
            <th>手續費：</th> 
            <td><ocxControl:ocxNumber runat="server" ID="APLY_SERV_CHAR" MASK=",999,999" Text='<%# Eval("APLY_SERV_CHAR").ToString().Trim() %>' /></td> 
            <th class="nonSpace" >攤提方式：</th>            
            <td><asp:DropDownList runat="server"  onchange="changeMTHD(1,this)" ID="APLY_AMOR_MTHD"  OnPreRender='checkList' ToolTip='<%# Eval("APLY_AMOR_MTHD").ToString().Trim() %>' Width="80" DataSourceID="sqlAMOR_MTHD"  DataValueField="TypeCode" DataTextField="TypeDesc" ></asp:DropDownList></td>            
        </tr> 
        
        <tr>            
            <td colspan="4">
                <table cellpadding="0" cellspacing="0" style="margin:0" ><tr><td>
                <fieldset id="MTHD1_1" style="padding:0;margin:0;display:none"><legend>定額</legend>
                    <table style="margin:0px"  cellpadding="1" cellspacing="1">
                        <tr><th>租金：</th><td><ocxControl:ocxNumber runat="server" ID="APLY_HIRE" MASK="9,999,999" Text='<%# Eval("APLY_HIRE").ToString().Trim() %>' /></td>
                        <th>稅金：</th><td><ocxControl:ocxNumber runat="server" ID="APLY_TAX" MASK="9,999,999" Text='<%# Eval("APLY_TAX").ToString().Trim() %>' /></td></tr>
                    </table>
                </fieldset><br />
                <fieldset id="MTHD1_2" style="padding:0;margin:0;display:none"><legend>階定額</legend>
                    <table style="margin:0px;width:300px"  cellpadding="1" cellspacing="1">
                        <tr><th>LF1：</th><td>
                        <ocxControl:ocxNumber runat="server" bolEnabled="false" ID="APLY_LF1_FR" MASK="99" Text='<%# Eval("APLY_LF1_FR").ToString().Trim() %>' />~
                        <ocxControl:ocxNumber runat="server" ID="APLY_LF1_TO" MASK="99" Text='<%# Eval("APLY_LF1_TO").ToString().Trim() %>' />    
                        </td>
                        <th>租金：</th><td><ocxControl:ocxNumber runat="server" ID="APLY_LF1_HIRE" MASK=",999,999" Text='<%# Eval("APLY_LF1_HIRE").ToString().Trim() %>' /></td></tr>
                        <tr><th>LF2：</th><td>
                        <ocxControl:ocxNumber runat="server" bolEnabled="false" ID="APLY_LF2_FR" MASK="99" Text='<%# Eval("APLY_LF2_FR").ToString().Trim() %>' />~
                        <ocxControl:ocxNumber runat="server" ID="APLY_LF2_TO" MASK="99" Text='<%# Eval("APLY_LF2_TO").ToString().Trim() %>' />    
                        </td>
                        <th>租金：</th><td><ocxControl:ocxNumber runat="server" ID="APLY_LF2_HIRE" MASK=",999,999" Text='<%# Eval("APLY_LF2_HIRE").ToString().Trim() %>' /></td></tr>
                        <tr><th>LF3：</th><td>
                        <ocxControl:ocxNumber runat="server" bolEnabled="false" ID="APLY_LF3_FR" MASK="99" Text='<%# Eval("APLY_LF3_FR").ToString().Trim() %>' />~
                        <ocxControl:ocxNumber runat="server" ID="APLY_LF3_TO" MASK="99" Text='<%# Eval("APLY_LF3_TO").ToString().Trim() %>' />    
                        </td>
                        <th>租金：</th><td><ocxControl:ocxNumber runat="server" ID="APLY_LF3_HIRE" MASK=",999,999" Text='<%# Eval("APLY_LF3_HIRE").ToString().Trim() %>' /></td></tr>
                        <tr style="display:none"><th>LF4：</th><td>
                        <ocxControl:ocxNumber runat="server"  bolEnabled="false" ID="APLY_LF4_FR" MASK="99" Text='<%# Eval("APLY_LF4_FR").ToString().Trim() %>' />~
                        <ocxControl:ocxNumber runat="server" ID="APLY_LF4_TO" MASK="99" Text='<%# Eval("APLY_LF4_TO").ToString().Trim() %>' />    
                        </td>
                        <th >租金：</th><td style="display:none"><ocxControl:ocxNumber runat="server" ID="APLY_LF4_HIRE" MASK=",999,999" Text='<%# Eval("APLY_LF4_HIRE").ToString().Trim() %>' /></td></tr>
                        <tr style="display:none"><th>LF5：</th><td>
                        <ocxControl:ocxNumber runat="server" bolEnabled="false" ID="APLY_LF5_FR" MASK="99" Text='<%# Eval("APLY_LF5_FR").ToString().Trim() %>' />~
                        <ocxControl:ocxNumber runat="server" ID="APLY_LF5_TO" MASK="99" Text='<%# Eval("APLY_LF5_TO").ToString().Trim() %>' />    
                        </td>
                        <th>租金：</th><td><ocxControl:ocxNumber runat="server" ID="APLY_LF5_HIRE" MASK=",999,999" Text='<%# Eval("APLY_LF5_HIRE").ToString().Trim() %>' /></td></tr>
                        
                    </table>
                </fieldset><br />
                </td>           
            </tr>
                </td></tr></table>
                
           
            </ItemTemplate>
            </asp:Repeater>   
            
            </table> 
            
       </fieldset>    </td><td valign="top">   
               <fieldset>
            <legend>保證人</legend>
             <div  class="gridMain " style="width:400px;margin:0">
                <div style="padding:0;margin:0; position:relative; overflow-y:scroll;width:400px;height:150px"  id="editGrid">
                    <table cellpadding="0" cellspacing="0"  style="width:160px" > 
                    <thead>
                        <tr >      
                            <th>類別</th>
                            <th>身份/統編</th>
                            <th>名稱</th>
                            <th>與申戶關係</th>                            
                        </tr> 
                    </thead>    
                        <asp:Repeater runat="server" ID="rptScur"> 
                            <ItemTemplate>
                                <tr > 
                                    <td> <asp:DropDownList ID="SCUR_NATUR"  style="width:70px"  runat="server"  OnPreRender='checkList' ToolTip='<%# Eval("SCUR_NATUR").ToString().Trim() %>' DataSourceID="sqlSCUR_NATUR"  DataValueField="TypeCode" DataTextField="TypeDesc" >
                             </asp:DropDownList> </td>
                                    </td>
                                    <td><asp:TextBox runat="server" ID="SCUR_ID"  MaxLength="20" width="90" Text='<%# Eval("SCUR_ID").ToString().Trim() %>'></asp:TextBox></td>
                                    <td><asp:TextBox runat="server" ID="SCUR_NAME"  MaxLength="20" width="90" Text='<%# Eval("SCUR_NAME").ToString().Trim() %>'></asp:TextBox></td>
                                    <td> <asp:DropDownList ID="SCUR_RELATION"  Width="80"  runat="server"  OnPreRender='checkList' ToolTip='<%# Eval("SCUR_RELATION").ToString().Trim() %>' DataSourceID="sqlSCUR_RELATION"  DataValueField="TypeCode" DataTextField="TypeDesc" >
                             </asp:DropDownList> </td>
                                    </td>
                                </tr>
                            </ItemTemplate> 
                        </asp:Repeater> 
                    </table>
                </div>     
            </div> 
        </fieldset>
          </td></tr></table> 
    
        </div> 
        
   
        <div class="panel" style="height: auto; width: 820px;display:none" id="tabpanel2">
            <asp:Repeater runat="server" ID="rptCon">
            <ItemTemplate>            
                <table style="margin:20px;margin-top:0px" >
                    <tr>            
                        <th>是否為計張：</th>
                        <td>
                             <asp:DropDownList ID="PAPER"   runat="server"  OnPreRender='checkList' ToolTip='<%# Eval("PAPER").ToString().Trim() %>' >
                                <asp:ListItem Value="Y">是</asp:ListItem>
                                <asp:ListItem Value="N">否</asp:ListItem>
                             </asp:DropDownList>     
                        </td>
                        <th>合約類別：</th>
                        <td>
                             <asp:DropDownList ID="CON_TYPE"   runat="server"  OnPreRender='checkList' ToolTip='<%# Eval("CON_TYPE").ToString().Trim() %>' DataSourceID="sqlCON_TYPE"  DataValueField="TypeCode" DataTextField="TypeDesc" >
                             </asp:DropDownList>     
                        </td>                                              
                    </tr>
                     <tr>            
                        <th class="nonSpace" >合約範本：</th>
                        <td>
                             <asp:DropDownList ID="TMP_CODE"   runat="server"  OnPreRender='checkList' ToolTip='<%# Eval("TMP_CODE").ToString().Trim() %>' DataSourceID="sqlTMP_CODE"  DataValueField="TMP_CODE" DataTextField="TMP_DESC" >
                             </asp:DropDownList>     
                        </td>                                              

                    </tr> 
                    <tr>            
                        <td colspan="2"><asp:CheckBox ID="EXPIRED_RENEW" runat="server"  OnPreRender='checkList' ToolTip='<%# Eval("EXPIRED_RENEW").ToString().Trim() %>'/>期滿再續約</td>
                    </tr>
                     <tr>            
                        <th>租金：</th>
                        <td><ocxControl:ocxNumber runat="server" ID="HIRE" MASK="999,999" Text='<%# Eval("HIRE").ToString().Trim() %>' /></td>
                        <th>營業稅：</th>
                        <td><ocxControl:ocxNumber runat="server" ID="TAX" MASK="999.9999" Text='<%# Eval("TAX").ToString().Trim() %>' /></td>
                        <th>年：</th>
                        <td><ocxControl:ocxYear runat="server" ID="YAR" MASK="999" Text='<%# Eval("YAR").ToString().Trim() %>' /></td>
                        <th>月：</th>
                        <td><ocxControl:ocxNumber runat="server" ID="MTH" MASK="999" Text='<%# Eval("MTH").ToString().Trim() %>' /></td>
                        <th>期數：</th>
                        <td><ocxControl:ocxNumber runat="server" ID="PERIOD" MASK="999" Text='<%# Eval("PERIOD").ToString().Trim() %>' /></td>
                    </tr>  
                    <tr>            
                        <td colspan="2"><asp:CheckBox ID="PACKAGE" runat="server"  OnPreRender='checkList' ToolTip='<%# Eval("PACKAGE").ToString().Trim() %>'/>套票</td>
                    </tr>
                    <tr>            
                        <td colspan="2"><asp:CheckBox ID="CASHIER" runat="server"  OnPreRender='checkList' ToolTip='<%# Eval("CASHIER").ToString().Trim() %>'/>簽立本票、授權書</td>
                    </tr>
                     <tr>            
                        <td colspan="3"><asp:CheckBox ID="CHG_CON" runat="server"  OnPreRender='checkList' ToolTip='<%# Eval("CHG_CON").ToString().Trim() %>'/>換約(附帶條款加註舊約終止字樣)</td>
                    </tr>
                    <tr>            
                        <th>原契約號碼：</th>
                        <td><asp:TextBox runat="server" ID="PRV_APLY_NO"  MaxLength="20" width="100" Text='<%# Eval("PRV_APLY_NO").ToString().Trim() %>'></asp:TextBox></td>
                    </tr>     
                     <tr>            
                        <td colspan="3"><asp:CheckBox ID="CHG_CODICIL" runat="server"  OnPreRender='checkList' ToolTip='<%# Eval("CHG_CODICIL").ToString().Trim() %>'/>中途換約附帶條文</td>
                    </tr>
                    <tr>
                        <td colspan="4">
                        <fieldset><legend>有無期間限制</legend>
                            <table><tr>
                                <td><asp:RadioButton runat="server" ID="LimitN" Checked='<%# Eval("Limit").ToString().Trim()=="Y" %>' />無期間限制</td>
                                <td><asp:RadioButton runat="server" ID="LimitY" Checked='<%# Eval("Limit").ToString().Trim()=="N" %>' />有期間限制</td>
                                <td>期(月)<ocxControl:ocxNumber runat="server" ID="RESTRICTION_PERIODS" MASK="999" Text='<%# Eval("RESTRICTION_PERIODS").ToString().Trim() %>' /></td>
                            </tr></table>
                        </fieldset>
                        </td>
                    </tr>
                    <tr>            
                        <td><asp:CheckBox ID="FREE_SHOW_MAC_NO" runat="server"  OnPreRender='checkList' ToolTip='<%# Eval("FREE_SHOW_MAC_NO").ToString().Trim() %>'/>免Show機號</td>
                    </tr>
                    <tr>
                        <th>附帶條款：</th>                        
                        <td colspan="9">
                            <asp:TextBox TextMode="MultiLine" Rows="6" runat="server" ID="CODICIL"   width="500" Text='<%# Eval("CODICIL").ToString().Trim() %>'></asp:TextBox>
                        </td>
                    </tr>
                </table>
                        </ItemTemplate> 
                    </asp:Repeater>
        
        </div> 
    </div>
</div> 
</asp:Panel>
 <asp:UpdatePanel ID="upCustom" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    
    <span style="display:none">
     <asp:Button runat="server" ID="btnReload" OnClick="Reload_Custom" />
     <asp:Button runat="server" ID="btnReload1" OnClick="Reload_Object" />
    </span>                             
    </ContentTemplate>
 </asp:UpdatePanel>
 
</asp:Content>


<asp:Content ID="myPopWindow" ContentPlaceHolderID="PopWindow" runat="server">
     

     
<script language="javascript" type="text/javascript">
    var old=document.getElementById('<%=this.rptBase.Items[0].FindControl("CUST_NO").ClientID %>').value;
    var oldO = document.getElementById('<%=this.rptObjDetail.Items[0].FindControl("OBJ_CODE").ClientID %>').value;

    $(document).ready(init());

    
    function init() {
        $('input[id*=APLY_LF][id*=TO]').blur(APLY_Change);
        $('input[id*=APLY_HIRE]').blur(APLY_HIRE_Change);
    }

    function APLY_HIRE_Change(){
     
     try{
         $('input[id*=APLY_TAX]').val(parseMoney(Math.round(parseInt($('input[id*=APLY_HIRE]').val().replace(/,/g, ""))*<%=this.tRate %>)));         
         $('input[id*=APLY_TAX]').prop('disabled', false); 
        }
        catch(Error)
        {
        }
        finally{}
    }
  
    
   function OLD_APLY_Change(){
        if ($(this).val()=="")
                document.getElementById("<%=this.btnReload1.ClientID %>").click();
   }
    function APLY_Change() {

        var tid = $(this).attr("id");
        for (var i = 1; i <= 3; i++) {
            if (tid.indexOf(i.toString()+"_TO") != -1)
                tid = tid = tid.replace(i.toString()+"_TO", (i+1).toString()+"_FR");
        }

        var period = parseInt($('input[id*=APLY_PERD]').val());
        var periodc=parseInt($(this).val());
        if (periodc>period) {
            window.parent.errorMessage('錯誤訊息','不得大於期數，請重新輸入！');
            document.getElementById($(this).attr("id")).value = "0";　
            document.getElementById($(this).attr("id")).focus();
        }
            
        if (parseInt($(this).val()) != 0 && periodc < period)
            document.getElementById(tid).value = parseInt($(this).val()) + 1;
            
        if (periodc==0)
            document.getElementById(tid).value = "0";
    }
    
    function dialogChange(sid, val) {

        if (sid.indexOf("CUST_NO") != -1)
        {
            if ( val != old)
                document.getElementById("<%=this.btnReload.ClientID %>").click();
            
            old = val;
        }
       
        if (sid.indexOf("OLD_APLY_NO") != -1)
        {
            val= document.getElementById('<%=this.rptObjDetail.Items[0].FindControl("OBJ_CODE").ClientID %>').value 
            if (val!= oldO && val!=""){
                document.getElementById("<%=this.btnReload1.ClientID %>").click();
                APLY_HIRE_Change();
            }
            oldD = val;
        }
    }
    
    function changeMTHD(stype,obj)
    {
   
        var val=obj.value;
       
        document.getElementById("MTHD"+stype.toString()+"_1").style.display="none";
        document.getElementById("MTHD"+stype.toString()+"_2").style.display="none";

        if (val == "1") {
            document.getElementById("MTHD" + stype.toString() + "_1").style.display = "";
        }
        if (val == "2") {
            document.getElementById("MTHD" + stype.toString() + "_2").style.display = "";
            document.getElementById('<%=this.rptRequest.Items[0].FindControl("APLY_LF1_FR").ClientID %>').value = "1"; 
            
         //   document.getElementById('<%=this.rptRequest.Items[0].FindControl("APLY_AMOR_MTHD").ClientID %>')
        }
        
    }
    
    changeMTHD(1,document.getElementById('<%=this.rptRequest.Items[0].FindControl("APLY_AMOR_MTHD").ClientID %>'));
    
    function openDetail(strDate, strType, strName) {

    
        window.parent.openPopUpWindow();
    }
</script>
</asp:Content> 
