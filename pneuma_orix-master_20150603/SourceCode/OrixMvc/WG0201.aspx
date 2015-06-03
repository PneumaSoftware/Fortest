<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/editing.Master" CodeBehind="WG0201.aspx.cs" Inherits="OrixMvc.WG0201" %>
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
    id="sqlINDUSTRY_TYPE" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'INDUSTRY_TYPE' "
    runat="server" >
</asp:sqldatasource>
 

<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlLOAN_MTHD_CODE" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="
    select Loan_Mthd_code TypeCode,Mthd_Desc TypeDesc from or_Loan_Mthd WHERE Loan_Mthd_code !='00001' 
AND Loan_Mthd_code != '00002' AND Loan_Mthd_code != '00003' order by Loan_Mthd_Code"
    runat="server" >
</asp:sqldatasource> 

<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlLOAN_TYPE" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'LOAN_TYPE'"
    runat="server" >
</asp:sqldatasource> 

<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlCOLL_MTHD" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'COLL_MTHD'"
    runat="server" >
</asp:sqldatasource> 
<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlINTEREST_CAL" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'INTEREST_CAL'"
    runat="server" >
</asp:sqldatasource> 

<span style="display:none">
<asp:TextBox runat="server" ID="SeqNo" Text='<%# Eval("SeqNo") %>' ></asp:TextBox>        
<asp:TextBox runat="server" ID="LOAN_SEQ" Text='<%# Eval("LOAN_SEQ") %>' ></asp:TextBox>        
</span>        

     <table style="margin-top:5px; " cellpadding="1" cellspacing="1">
        <tr>
            <th>銀行代號：</th>
            <td ><ocxControl:ocxDialog runat="server" ID="BANK_NO" width="60"   FieldName="SeqNo,BANK_NAME,BANK_TYPE,CRD_AMT,USED_CREDIT,REST_CREDIT,CRD_DATE_TO,AMT_LONG_SHORT_LOAN"  SourceName="OR_BANK_AMT" Text='<%# Eval("BANK_NO") %>' />
                <span class="memo">(選擇額度)</span>
            </td>
            <th>銀行類別：</th>
            <td><asp:TextBox runat="server" ID="BANK_TYPE" CssClass="display"  Text='<%# Eval("BANK_TYPE") %>'  Width="80"   ></asp:TextBox></td>
        </tr>
        <tr>
            <th style="width:120px">銀行名稱：</th>
            <td style="width:140px">
            <asp:TextBox runat="server" ID="BANK_NAME" CssClass="display"  Text='<%# Eval("BANK_NAME") %>'  Width="120"   ></asp:TextBox></td>
            <th style="width:85px">授信額度：</th>
            <td style="width:95px"><asp:TextBox runat="server" ID="CRD_AMT" CssClass="display number "  Text='<%# Eval("CRD_AMT", "{0:###,###,###,##0}") %>'  Width="90"   ></asp:TextBox></td> 
            <th  style="width:125px">已使用授信額度：</th>
            <td  style="width:95px"><asp:TextBox runat="server" ID="USED_CREDIT" CssClass="display number "  Text='<%# Eval("USED_CREDIT", "{0:###,###,###,##0}") %>'  Width="90"   ></asp:TextBox></td> 
            <th  style="width:95px">剩餘額度：</th>
            <td  style="width:90px"><asp:TextBox runat="server" ID="REST_CREDIT" CssClass="display number "  Text='<%# Eval("REST_CREDIT", "{0:###,###,###,##0}") %>'  Width="85"   ></asp:TextBox></td>             
       </tr>       
       <tr>
            <th>入帳帳號：</th>
            <td >
    <asp:UpdatePanel runat="server" ID="upBANK_ACCT_NO"  UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
                <asp:DropDownList runat="server" ID="BANK_ACCT_NO" OnPreRender='checkList'  ToolTip='<%# Eval("BANK_ACCT_NO") %>' Width="130" ></asp:DropDownList>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnReload"  />
        </Triggers>
    </asp:UpdatePanel>    
            </td>
       
            <th class="nonSpace">借款日期：</th>
            <td><ocxControl:ocxDate runat="server" ID="LOAN_DATE" Text='<%# Eval("LOAN_DATE") %>' /></td>
            <th>授信到期日：</th>
            <td><asp:TextBox runat="server" ID="CRD_DATE_TO" Text='<%# Eval("CRD_DATE_TO") %>' CssClass="display"  Width="90"   ></asp:TextBox></td>
            <th>長短借：</th>
            <td><asp:TextBox runat="server" ID="AMT_LONG_SHORT_LOAN" Text='<%# Eval("AMT_LONG_SHORT_LOAN") %>' CssClass="display"   Width="50" ></asp:TextBox></td>
       </tr> 
       <tr> 
            <th class="nonSpace">借款金額：</th>
            <td><ocxControl:ocxNumber MASK="9,999,999,999" ID="LOAN_AMT"  runat="server" Text='<%# Eval("LOAN_AMT") %>'  /><ocxControl:ocxNumber MASK="9,999,999,999" ID="LOAN_AMT_OLD" Visible=false  runat="server" Text='<%# Eval("LOAN_AMT") %>'  /></td>       
            <th class="nonSpace">到期日：</th>
            <td><ocxControl:ocxDate runat="server" ID="DUE_DATE" Text='<%# Eval("DUE_DATE") %>' /></td>   
            <th>借款方式：</th>
            <td ><asp:DropDownList runat="server" ID="LOAN_MTHD_CODE"  Width="100" DataSourceID="sqlLOAN_MTHD_CODE"  OnPreRender='checkList' ToolTip='<%# Eval("LOAN_MTHD_CODE") %>' DataValueField="TypeCode" DataTextField="TypeDesc" ></asp:DropDownList></td> 
        </tr>
        <tr>          
            <th>授信方式：</th>
            <td ><asp:DropDownList runat="server" ID="COLL_MTHD"  Width="100" DataSourceID="sqlCOLL_MTHD"  OnPreRender='checkList' ToolTip='<%# Eval("CREDIT_WAY") %>'  DataValueField="TypeCode" DataTextField="TypeDesc" ></asp:DropDownList></td>            
            <th>有無追索：</th>
            <td ><asp:DropDownList runat="server" ID="IsRecourse"  OnPreRender='checkList' ToolTip='<%# Eval("IsRecourse") %>' Width="100"  >
                    <asp:ListItem Value=""></asp:ListItem>
                    <asp:ListItem Value="Y">Y</asp:ListItem>
                    <asp:ListItem Value="N">N</asp:ListItem>
                </asp:DropDownList>
            </td>                        
            <th>長短借：</th>
            <td >
                <asp:DropDownList runat="server" ID="LONG_SHORT_LOAN"  OnPreRender='checkList' ToolTip='<%# Eval("LONG_SHORT_LOAN") %>' Width="100">
                <asp:ListItem Value=""></asp:ListItem>
                <asp:ListItem Value="1">1.長</asp:ListItem>
                <asp:ListItem Value="2">2.短</asp:ListItem>
                </asp:DropDownList></td>                        
        </tr>        
        <tr>
            <th class="nonSpace">利息計算方式：</th>
            <td colspan="3" ><asp:DropDownList runat="server" ID="INTEREST_CAL"  Width="250" DataSourceID="sqlINTEREST_CAL"  OnPreRender='checkList' ToolTip='<%# Eval("INTEREST_CAL") %>'  DataValueField="TypeCode" DataTextField="TypeDesc" ></asp:DropDownList></td>                 
            <th colspan="2">固定/浮動利率：</th>
            <td ><asp:DropDownList runat="server" ID="Fixed_interest"  Width="70" OnPreRender='checkList' ToolTip='<%# Eval("Fixed_interest") %>' >                 
                <asp:ListItem Value="1">1.固定</asp:ListItem>
                <asp:ListItem Value="2">2.浮動</asp:ListItem>
            </asp:DropDownList></td>    
        </tr> 
        <tr>
            <th>
                <asp:CheckBox runat="server" ID="Leap_year" OnPreRender='checkList' Text='考慮閏年' ToolTip='<%# Eval("Leap_year") %>'/>
            </th>
            <th >2月利率計算天算：</th>
            <td >
                <asp:DropDownList runat="server" ID="Feb_rate_Days"  OnPreRender='checkList' ToolTip='<%# Eval("Feb_rate_Days") %>' Width="50">                
                <asp:ListItem Value="28">28</asp:ListItem>
                <asp:ListItem Value="29">29</asp:ListItem>
                <asp:ListItem Value="30">30</asp:ListItem>
                <asp:ListItem Value="31">31</asp:ListItem>
                </asp:DropDownList>
            </td>  
        <th>備註：</th>
            <td colspan="4"><asp:TextBox runat="server" ID="REMARK" Width="350" TextMode="MultiLine" Rows="2" Text='<%# Eval("REMARK") %>'></asp:TextBox></td>
        </tr>    
    <tr>
    <td colspan="8">
    <table cellpadding="0" cellspacing="0">
    <tr><td valign="top">
    <asp:UpdatePanel runat="server" ID="upRATE" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>        
        <div  class="gridMain " style="width:250px">
        <div style="padding:0;margin:0; overflow-y:scroll;width:245px; height:260px" runat="server">
            <table cellpadding="0" cellspacing="0"  style="width:220px" >
                <tr>      
                    <th>編輯</th>          
                    <th class="nonSpace">生效日</th>
                    <th >利率</th>                    
                </tr>   
                <asp:Repeater runat="server" ID="rptRATE"  > 
                    <ItemTemplate>
                        <tr style="display:<%# Eval("Status").ToString()=="D"?"none":"" %>" > 
                            <td>
                            <% if (!this.bolQuery)
                               {%>
                            <asp:Button ID="btnDel_RATE" runat="server"  cssClass="button del"  Text="刪除"  OnCommand="GridFunc_Click"  CommandName='<%# Eval("EFF_DATE") %>' />
                            <%} %>
                            <asp:HiddenField runat="server" ID="Status"  Value='<%# Eval("Status") %>' /></td>
                            <td><asp:TextBox runat="server" CssClass="display" ID="EFF_DATE" Text='<%# Eval("EFF_DATE") %>' Width=100></asp:TextBox></td>
                            <td><ocxControl:ocxNumber MASK="99.9999" ID="RATE" runat="server" Text='<%# Eval("RATE") %>'  /></td>           
                        </tr>
                    </ItemTemplate> 
                </asp:Repeater>                       
                <tr style='display:<%=  this.bolQuery ?"none":""%>'>
                    <td><asp:Button ID="btnAdd_RATE" runat="server"  cssClass="button dadd"  Text="新增" OnCommand="GridFunc_Click" CommandName="Add" /> </td>
                    <td><ocxControl:ocxDate runat="server" ID="addEFF_DATE"   /></td>
                    <td><ocxControl:ocxNumber MASK="99.9999" ID="addRATE" runat="server"   /></td>                        
                </tr>
            </table>
        </div>     
    </div> 
    </ContentTemplate>
    </asp:UpdatePanel>
    </td>
    <td valign="top">
    <asp:UpdatePanel runat="server" ID="upDTL" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>        
        <div  class="gridMain " style="width:305px">
        <div style="padding:0;margin:0; overflow-y:scroll;width:300px;height:260px" runat="server">
            <table cellpadding="0" cellspacing="0"  style="width:270px" > 
                <tr>      
                    <th>編輯</th>          
                    <th class="nonSpace">還款日期</th>
                    <th class="nonSpace">還款金額</th>                    
                    <th>繳息</th>                    
                </tr>  
                <asp:Repeater runat="server" ID="rptDTL"  > 
                    <ItemTemplate>
                        <tr style="display:<%# Eval("Status").ToString()=="D"?"none":"" %>" > 
                            <td>
                            <% if (!this.bolQuery)
                               {%>
                            <asp:Button ID="btnDel_DTL" runat="server"  cssClass="button del"  Text="刪除"  OnCommand="GridFunc_Click"  CommandName='<%# Eval("RED_DATE") %>' />
                            <%} %>
                            <asp:HiddenField runat="server" ID="Status"  Value='<%# Eval("Status") %>' /></td>
                            <td><asp:TextBox ID="RED_DATE" runat="server" CssClass="display" Text='<%# Eval("RED_DATE") %>' Width=100></asp:TextBox></td>
                            <td><ocxControl:ocxNumber MASK="999,999" ID="RED_AMT" runat="server" Text='<%# Eval("RED_AMT") %>'  /></td>
                            <td><asp:CheckBox runat="server" id="INTEREST_YN" Checked='<%# Eval("INTEREST_YN").ToString().Trim()=="Y"?true:false %>'/></td>           
                        </tr>
                    </ItemTemplate> 
                </asp:Repeater>                       
                <tr style='display:<%=  this.bolQuery ?"none":""%>'>
                    <td><asp:Button ID="btnAdd_DTL" runat="server"  cssClass="button dadd"  Text="新增" OnCommand="GridFunc_Click" CommandName="Add" /> </td>
                    <td><ocxControl:ocxDate runat="server" ID="addRED_DATE"   /></td>
                    <td><ocxControl:ocxNumber MASK="999,999" ID="addRED_AMT"  runat="server"   /></td>                        
                    <td><asp:CheckBox runat="server" id="addINTEREST_YN" /></td>           
                </tr>
            </table>                       
        </div>     
    </div> 
    <div style="color:Red;width:98%;text-align:right">
    還款金額總計：   <ocxControl:ocxNumber MASK="999,999,999" ID="RED_AMT_TTL" runat="server" bolEnabled=false   />
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
    </td>
    
    <td valign="top">
    <asp:UpdatePanel runat="server" ID="upDATE" RenderMode="Inline" UpdateMode="Conditional">    
        <ContentTemplate>        
         <table>
            <tr><th> 
            繳息週期(月數)：</th><td> <ocxControl:ocxNumber MASK="999" ID="Interest_Payment" runat="server"  Text='<%# Eval("Interest_Payment") %>' />
            </td></tr>
            <tr><th>
            繳息日：</th><td><ocxControl:ocxNumber MASK="999" ID="PAY_DIVD_DATE" runat="server"  Text='<%# Eval("PAY_DIVD_DATE") %>' />
            </td>            
            <td>
                <div class="divFunction80" >
            <asp:Button runat="server" ID="cal_DATE" CssClass="button trn80" Text="展開" OnCommand="GridFunc_Click" CommandName="CAL_DATE"  />
        </div> 
            </td></tr>
        </table>
        
        <div  class="gridMain " style="width:185px">
        <div style="padding:0;margin:0; overflow-y:scroll;width:180px;height:195px" runat="server" >
            <table cellpadding="0" cellspacing="0"  style="width:150px" > 
            <thead>
                <tr>      
                    <th>編輯</th>          
                    <th class="nonSpace">繳息日期</th>                                        
                </tr>   
            </thead>    
                <asp:Repeater runat="server" ID="rptDATE"  > 
                    <ItemTemplate>
                        <tr style="display:<%# Eval("Status").ToString()=="D"?"none":"" %>" > 
                            <td>
                            <% if (!this.bolQuery)
                               {%>
                            <asp:Button ID="btnDel_DATE" runat="server"  cssClass="button del"  Text="刪除"  OnCommand="GridFunc_Click"  CommandName='<%# Eval("INTEREST_DATE") %>' />
                            <%} %>
                            <asp:HiddenField runat="server" ID="Status"  Value='<%# Eval("Status") %>' /></td>
                            <td ><ocxControl:ocxDate runat="server" ID="INTEREST_DATE"  Text='<%# Eval("INTEREST_DATE") %>'  /></td>                            
                        </tr>
                    </ItemTemplate> 
                </asp:Repeater>                       
                <tr style='display:<%=  this.bolQuery ?"none":""%>'>
                    <td><asp:Button ID="btnAdd_DATE" runat="server"  cssClass="button dadd"  Text="新增" OnCommand="GridFunc_Click" CommandName="Add" /> </td>
                    <td><ocxControl:ocxDate runat="server" ID="addINTEREST_DATE"   /></td>                    
                </tr>
            </table>
        </div>     
    </div> 
    </ContentTemplate>
    </asp:UpdatePanel>
    </td>
    </tr>
    </table>
    </td>
    </tr>           
</table>            
          
          
   <span style="display:none">
     <asp:Button runat="server" ID="btnReload" OnClick="Reload_ACCTNO" />     
    </span>  

<script language="javascript"  type="text/javascript">
var old=document.getElementById('<%=this.BANK_NO.ClientID %>').value;

     function dialogChange(sid, val) {

        if (sid.indexOf("BANK_NO") != -1)
        {
            if ( val != old)
                document.getElementById("<%=this.btnReload.ClientID %>").click();
            
            old = val;
        }
       
    }

</script>    
                           
</asp:Content>


<asp:Content ID="myPopWindow" ContentPlaceHolderID="PopWindow" runat="server">
     

     
<script language="javascript" type="text/javascript">
    
    
    
    function openDetail(strDate, strType, strName) {

    
        window.parent.openPopUpWindow();
    }
</script>
</asp:Content> 
