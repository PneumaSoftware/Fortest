<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/editing.Master" CodeBehind="WG0301.aspx.cs" Inherits="OrixMvc.WG0301" %>
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
<asp:Panel ID="myPanel" runat="server">
     <table style="margin-top:5px; " cellpadding="1" cellspacing="1">
        <tr>
            <th>銀行代號：</th>
            <td colspan="3"><ocxControl:ocxDialog runat="server" ID="BANK_NO" width="80"   FieldName="SeqNo,BANK_NAME,CRD_AMT,USED_CREDIT,REST_CREDIT,CRD_DATE_TO,AMT_LONG_SHORT_LOAN"  SourceName="OR_BANK_AMT" Text='<%# Eval("BANK_NO") %>' />                
            </td>
        </tr>
        <tr>
            <th style="width:130px">銀行名稱：</th>
            <td style="width:120px">
            <asp:TextBox runat="server" ID="BANK_NAME" CssClass="display"  Text='<%# Eval("BANK_NAME") %>'  Width="120"   ></asp:TextBox></td>
            <th style="width:80px">授信額度：</th>
            <td style="width:90px"><asp:TextBox runat="server" ID="CRD_AMT" CssClass="display number "  Text='<%# Eval("CRD_AMT", "{0:###,###,###,##0}") %>'  Width="70"   ></asp:TextBox></td> 
            <th  style="width:125px">已使用授信額度：</th>
            <td  style="width:95px"><asp:TextBox runat="server" ID="USED_CREDIT" CssClass="display number "  Text='<%# Eval("USED_CREDIT", "{0:###,###,###,##0}") %>'  Width="70"   ></asp:TextBox></td> 
            <th  style="width:95px">剩餘額度：</th>
            <td  style="width:95px"><asp:TextBox runat="server" ID="REST_CREDIT" CssClass="display number "  Text='<%# Eval("REST_CREDIT", "{0:###,###,###,##0}") %>'  Width="70"   ></asp:TextBox></td>             
       </tr>       
       <tr>
            <th>銀行帳號：</th>
            <td colspan="2" >
                <asp:TextBox runat="server" ID="BANK_ACCT_NO"   Text='<%# Eval("BANK_ACCT_NO") %>' Width="180" ></asp:TextBox>                
            </td>
       
            <th>借款日期：</th>
            <td><asp:TextBox runat="server" ID="LOAN_DATE" Text='<%# Eval("LOAN_DATE") %>' CssClass="display"  Width="70"   ></asp:TextBox> </td>
            <th>授信到期日：</th>
            <td><asp:TextBox runat="server" ID="CRD_DATE_TO" Text='<%# Eval("CRD_DATE_TO") %>' CssClass="display"  Width="70"   ></asp:TextBox></td>
            <th>長短借：</th>
            <td><asp:TextBox runat="server" ID="AMT_LONG_SHORT_LOAN" Text='<%# Eval("AMT_LONG_SHORT_LOAN") %>' CssClass="display"   Width="50" ></asp:TextBox></td>
       </tr> 
       <tr>
            <th>借款金額：</th>
            <td><asp:TextBox runat="server" ID="LOAN_AMT" Text='<%# Eval("LOAN_AMT", "{0:###,###,###,##0}") %>' CssClass="display"  Width="70"   ></asp:TextBox></td>        
            <th>到期日：</th>
            <td><asp:TextBox runat="server" ID="DUE_DATE" Text='<%# Eval("DUE_DATE") %>' CssClass="display"  Width="80"   ></asp:TextBox></td>   
            <th>借款方式：</th>
            <td colspan="2"><asp:TextBox runat="server" ID="MTHD_DESC"   Text='<%# Eval("MTHD_DESC") %>' Width="100" ></asp:TextBox></td> 
        </tr>
        <tr>               
            <th>授信方式：</th>
            <td><asp:TextBox runat="server" ID="CREDIT_WAY"   Text='<%# Eval("CREDIT_WAY_DESC") %>' Width="70" ></asp:TextBox></td>             
            <th>有無追索：</th>
            <td ><asp:TextBox runat="server" ID="IsRecourse"   Text='<%# Eval("IsRecourse") %>' Width="50" ></asp:TextBox></td>                        
            <th>長短借：</th>
            <td ><asp:TextBox runat="server" ID="LONG_SHORT_LOAN"   Text='<%# Eval("LONG_SHORT_LOAN_DESC") %>' Width="50" ></asp:TextBox></td>                        
                                  
        </tr>        
        <tr>
            <th>利息計算方式：</th>
            <td colspan="3"><asp:TextBox runat="server" ID="INTEREST_CAL"   Text='<%# Eval("INTEREST_CAL_DESC") %>' Width="180" ></asp:TextBox></td>                
            <th colspan="2">固定/浮動利率：</th>
            <td><asp:TextBox runat="server" ID="Fixed_interest"   Text='<%# Eval("Fixed_interest_DESC") %>' Width="70" ></asp:TextBox></td>    
        </tr> 
        <tr>
            <th>
                <asp:CheckBox runat="server" ID="Leap_year" OnPreRender='checkList' Text='考慮閏年' ToolTip='<%# Eval("Leap_year") %>'/>
            </th>
            <th >2月利率計算天算：</th>
            <td><asp:TextBox runat="server" ID="Feb_rate_Days"   Text='<%# Eval("Feb_rate_Days") %>' Width="50" ></asp:TextBox></td>                
        <th>備註：</th>
            <td colspan="4"><asp:TextBox runat="server" ID="REMARK" Width="350" TextMode="MultiLine" Enabled="false" Rows="2" Text='<%# Eval("REMARK") %>'></asp:TextBox></td>
        </tr> 
    </table>
    </asp:Panel> 
    <table cellpadding="0" cellspacing="0">
    <tr>
    <td valign="top">
    <fieldset><legend>預計還本日</legend>
        <div  class="gridMain " style="width:355px">
        <div style="padding:0;margin:0; overflow-y:scroll;width:350px;position:relative;height:260px" runat="server">
            <table cellpadding="0" cellspacing="0"  style="width:330px" > 
                <tr>                        
                    <th class="nonSpace">還款日期</th>
                    <th class="nonSpace">還款金額</th>                    
                    <th>繳息</th>                    
                </tr>  
                <asp:Repeater runat="server" ID="rptDTL_E"  > 
                    <ItemTemplate>
                        <tr>                           
                            <td><asp:TextBox ID="RED_DATE" runat="server" CssClass="display" Text='<%# Eval("RED_DATE") %>' Width=100></asp:TextBox></td>
                            <td><ocxControl:ocxNumber MASK="999,999" ID="RED_AMT" bolEnabled="false" runat="server" Text='<%# Eval("RED_AMT") %>'  /></td>
                            <td><asp:CheckBox runat="server" id="INTEREST_YN" Enabled="false"  OnPreRender='checkList' ToolTip='<%# Eval("INTEREST_YN").ToString().Trim() %>'/></td>           
                        </tr>
                    </ItemTemplate> 
                </asp:Repeater>                       
               
            </table>
        </div>     
    </div> 
     </fieldset>
    </td>
    
    <td valign="top">
    <fieldset><legend>還本日</legend>
    <asp:UpdatePanel runat="server" ID="upDTL" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>        
        <div  class="gridMain " style="width:355px">
        <div id="Div1" style="padding:0;margin:0; overflow-y:scroll;width:350px;position:relative;height:260px" runat="server"> 
            <table cellpadding="0" cellspacing="0"  style="width:270px" > 
                <tr>      
                    <th>編輯</th>          
                    <th class="nonSpace">還款日期</th>
                    <th class="nonSpace">還款金額</th>                                        
                </tr>  
                <asp:Repeater runat="server" ID="rptDTL"  > 
                    <ItemTemplate>
                        <tr style="display:<%# Eval("Status").ToString()=="D"?"none":"" %>" > 
                            <td>
                            <% if (!this.bolQuery)
                               {%>
                            <asp:Button ID="btnDel_DTL" runat="server"  cssClass="button del"  Text="刪除"  OnCommand="GridFunc_Click"  CommandName='<%# Eval("RED_DATE") %>' />
                            <%} %>
                            <asp:HiddenField runat="server" ID="SEQ_NO"  Value='<%# Eval("SEQ_NO") %>' /></td>
                            <asp:HiddenField runat="server" ID="Status"  Value='<%# Eval("Status") %>' /></td>
                            <td><asp:TextBox ID="RED_DATE" runat="server" CssClass="display" Text='<%# Eval("RED_DATE") %>' Width=100></asp:TextBox></td>
                            <td><ocxControl:ocxNumber MASK="9,999,999" ID="RED_AMT" runat="server" Text='<%# Eval("RED_AMT") %>'  /></td>                            
                        </tr>
                    </ItemTemplate> 
                </asp:Repeater>                       
                 <tr style='display:<%=  this.bolQuery ?"none":""%>'>
                    <td><asp:Button ID="btnAdd_DTL" runat="server"  cssClass="button dadd"  Text="新增" OnCommand="GridFunc_Click" CommandName="Add" /> </td>
                    <td><ocxControl:ocxDate runat="server" ID="addRED_DATE"   /></td>
                    <td><ocxControl:ocxNumber MASK="9,999,999" ID="addRED_AMT"  runat="server"   /></td>                                            
                </tr>
            </table>
        </div>     
    </div> 
    </ContentTemplate>
    </asp:UpdatePanel>
    </fieldset> 
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
