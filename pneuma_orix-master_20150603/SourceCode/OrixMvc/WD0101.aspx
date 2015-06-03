<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/editing.Master" CodeBehind="WD0101.aspx.cs" Inherits="OrixMvc.WD0101" %>
<%@ MasterType VirtualPath="~/Pattern/editing.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxYM"  TagPrefix="ocxControl" Src="~/ocxControl/ocxYM.ascx" %>
<%@ Register TagName="ocxYear"  TagPrefix="ocxControl" Src="~/ocxControl/ocxYear.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxTime"  TagPrefix="ocxControl" Src="~/ocxControl/ocxTime.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
        

<asp:Content ContentPlaceHolderID="editingArea" runat="server" ID="myDataArea">
<fieldset><legend>基本資料</legend>
<table>
    <tr>
        <th >申請書編號：</th>
        <td><asp:TextBox runat="server" ID="APLY_NO"  CssClass="display" Text='<%# Eval("APLY_NO").ToString().Trim() %>'  Width="150"></asp:TextBox></td>
        <th >客戶代號：</th>
        <td><asp:TextBox runat="server" ID="CUST_NO"  CssClass="display" Text='<%# Eval("CUST_NO").ToString().Trim() %>'  Width="100"></asp:TextBox></td>
        <td><asp:TextBox runat="server" ID="CUST_SNAME"  CssClass="display" Text='<%# Eval("CUST_SNAME").ToString().Trim() %>'  Width="150"></asp:TextBox></td>
        <th class="nonSpace" >是否含稅：</th>
        <td class="nonSpace"><asp:DropDownList runat="server" ID="INC_TAX" Width="80" OnPreRender='checkList'  ToolTip='<%# Eval("INC_TAX") %>'  >
            <asp:ListItem Value=""></asp:ListItem>
            <asp:ListItem Value="Y">是</asp:ListItem>
            <asp:ListItem Value="N">否</asp:ListItem>
        </asp:DropDownList></td>
    </tr>
    <tr>
        <th >聯絡人：</th>
        <td><asp:TextBox runat="server" ID="CONTACT"  CssClass="display" Text='<%# Eval("CONTACT").ToString().Trim() %>'  Width="150"></asp:TextBox></td>
        <th >聯絡電話：</th>
        <td colspan="2"><asp:TextBox runat="server" ID="CTAC_TEL"  CssClass="display" Text='<%# Eval("CTAC_TEL").ToString().Trim() %>'  Width="200"></asp:TextBox></td>
        <th >傳真：</th>
        <td><asp:TextBox runat="server" ID="FAX"  CssClass="display" Text='<%# Eval("FAX").ToString().Trim() %>'  Width="120"></asp:TextBox></td>        
    </tr>
    <tr>
        <th >供應商：</th>
        <td><asp:TextBox runat="server" ID="FRC_CODE"  CssClass="display" Text='<%# Eval("FRC_CODE").ToString().Trim() %>'  Width="100"></asp:TextBox></td>
        <td colspan="3"><asp:TextBox runat="server" ID="FRC_SNAME"  CssClass="display" Text='<%# Eval("FRC_SNAME").ToString().Trim() %>'  Width="150"></asp:TextBox></td>
        <th >營業單位：</th>
        <td><asp:TextBox runat="server" ID="SALES_UNIT"  CssClass="display" Text='<%# Eval("SALES_UNIT").ToString().Trim() %>'  Width="120"></asp:TextBox></td>  
    </tr>
    <tr>
        <th >承辦業務：</th>
        <td><asp:TextBox runat="server" ID="SALES_NAME"   Text='<%# Eval("SALES_NAME").ToString().Trim() %>'  Width="100"></asp:TextBox></td>        
        <th >承辦業務電話：</th>
        <td colspan="2"><asp:TextBox runat="server" ID="SALES_TEL"  Text='<%# Eval("SALES_TEL").ToString().Trim() %>'  Width="200"></asp:TextBox></td>
    </tr>        
    <tr>
        <th >備註：</th>
        <td colspan="5"><asp:TextBox runat="server" ID="REMARK" Text='<%# Eval("REMARK").ToString().Trim() %>'  Width="350" MaxLength="20"></asp:TextBox></td>      
    </tr>
</table>
</fieldset> 

<fieldset><legend>計價方式</legend>
<table>
<tr><td>
<asp:UpdatePanel runat="server" ID="upObjDetail" RenderMode="Inline" UpdateMode="Conditional">
                <ContentTemplate>
                <asp:Repeater runat="server" ID="rptObjDetail">
            <ItemTemplate>     


<table style="width:350px">
<tr> 
    <th>期數起：</th>
    <td><ocxControl:ocxNumber runat="server" ID="PERIOD_S" MASK="999" bolEnabled="false"  Text='<%# Eval("PERIOD_S").ToString().Trim() %>' /></td>
    <th>期數迄：</th>
    <td><ocxControl:ocxNumber runat="server" ID="PERIOD_E" MASK="999"  Text='<%# Eval("PERIOD_E").ToString().Trim() %>'/></td>
    <th>基本租金：</th>
    <td><ocxControl:ocxNumber runat="server"  ID="BASE_RENT_FEE" MASK="9,999,999"  Text='<%# Eval("BASE_RENT_FEE").ToString().Trim() %>'/></td>
</tr>
</table>
<table  cellpadding="1" cellspacing="1">
    <tr><td colspan="2">每月基本張數</td><td colspan="2">單價收費</td><td colspan="2">單張(供應商)</td><td colspan="2">單張(ORIX)</td></tr>
    <tr>
        <th>A4黑白</th>
        <td><ocxControl:ocxNumber runat="server" ID="A4_MONO_MONTH_BASE_PAGE" MASK=",999,999"  Text='<%# Eval("A4_MONO_MONTH_BASE_PAGE").ToString().Trim() %>' /></td>
        <td><ocxControl:ocxNumber runat="server" ID="A4_MONO_PAGE_FEE" MASK="99.999"  Text='<%# Eval("A4_MONO_PAGE_FEE").ToString().Trim() %>' /></td>
        <td><ocxControl:ocxNumber  runat="server" bolEnabled="false" ID="A4_MONO_MONTH_BASE_PAGE_TTL" MASK=",999,999" /></td>
        <td><ocxControl:ocxNumber runat="server" ID="A4_MONO_PAGE_FEE_FRC" MASK="99.999" Text='<%# Eval("A4_MONO_PAGE_FEE_FRC").ToString().Trim() %>' /></td>
        <td><ocxControl:ocxNumber  runat="server" bolEnabled="false" ID="A4_MONO_PAGE_FEE_FRC_TTL" MASK=",999,999" /></td>
        <td><ocxControl:ocxNumber  runat="server" bolEnabled="false" ID="A4_MONO_PAGE_FEE_ORIX" MASK="99.999"  /></td>
        <td><ocxControl:ocxNumber  runat="server" bolEnabled="false" ID="A4_MONO_PAGE_FEE_ORIX_TTL" MASK=",999,999" /></td>        
    </tr>
    <tr>
        <th>A4彩色</th>
        <td><ocxControl:ocxNumber runat="server" ID="A4_COLOR_MONTH_BASE_PAGE" MASK=",999,999" Text='<%# Eval("A4_COLOR_MONTH_BASE_PAGE").ToString().Trim() %>' /></td>
        <td><ocxControl:ocxNumber runat="server" ID="A4_COLOR_PAGE_FEE" MASK="99.999" Text='<%# Eval("A4_COLOR_PAGE_FEE").ToString().Trim() %>' /></td>
        <td><ocxControl:ocxNumber  runat="server" bolEnabled="false" ID="A4_COLOR_MONTH_BASE_PAGE_TTL" MASK=",999,999"  /></td>
        <td><ocxControl:ocxNumber runat="server" ID="A4_COLOR_PAGE_FEE_FRC" MASK="99.999" Text='<%# Eval("A4_COLOR_PAGE_FEE_FRC").ToString().Trim() %>' /></td>
        <td><ocxControl:ocxNumber  runat="server" bolEnabled="false" ID="A4_COLOR_PAGE_FEE_FRC_TTL" MASK=",999,999" /></td>
        <td><ocxControl:ocxNumber  runat="server" bolEnabled="false" ID="A4_COLOR_PAGE_FEE_ORIX" MASK="99.999"  /></td>
        <td><ocxControl:ocxNumber  runat="server" bolEnabled="false" ID="A4_COLOR_PAGE_FEE_ORIX_TTL" MASK=",999,999"  /></td>
        
    </tr>
    <tr>
        <th>A3黑白</th>
       <td><ocxControl:ocxNumber runat="server" ID="A3_MONO_MONTH_BASE_PAGE" MASK=",999,999"  Text='<%# Eval("A3_MONO_MONTH_BASE_PAGE").ToString().Trim() %>' /></td>
        <td><ocxControl:ocxNumber runat="server" ID="A3_MONO_PAGE_FEE" MASK="99.999"  Text='<%# Eval("A3_MONO_PAGE_FEE").ToString().Trim() %>' /></td>
        <td><ocxControl:ocxNumber  runat="server" bolEnabled="false" ID="A3_MONO_MONTH_BASE_PAGE_TTL" MASK=",999,999"  /></td>
        <td><ocxControl:ocxNumber runat="server" ID="A3_MONO_PAGE_FEE_FRC" MASK="99.999" Text='<%# Eval("A3_MONO_PAGE_FEE_FRC").ToString().Trim() %>' /></td>
        <td><ocxControl:ocxNumber  runat="server" bolEnabled="false" ID="A3_MONO_PAGE_FEE_FRC_TTL" MASK=",999,999" /></td>
        <td><ocxControl:ocxNumber  runat="server" bolEnabled="false" ID="A3_MONO_PAGE_FEE_ORIX" MASK="99.999"  /></td>
        <td><ocxControl:ocxNumber  runat="server" bolEnabled="false" ID="A3_MONO_PAGE_FEE_ORIX_TTL" MASK=",999,999"  /></td>
        
    </tr>
    <tr>
        <th>A3彩色</th>
        <td><ocxControl:ocxNumber runat="server" ID="A3_COLOR_MONTH_BASE_PAGE" MASK=",999,999" Text='<%# Eval("A3_COLOR_MONTH_BASE_PAGE").ToString().Trim() %>' /></td>
        <td><ocxControl:ocxNumber runat="server" ID="A3_COLOR_PAGE_FEE" MASK="99.999" Text='<%# Eval("A3_COLOR_PAGE_FEE").ToString().Trim() %>' /></td>
        <td><ocxControl:ocxNumber  runat="server" bolEnabled="false" ID="A3_COLOR_MONTH_BASE_PAGE_TTL" MASK=",999,999"  /></td>
        <td><ocxControl:ocxNumber runat="server" ID="A3_COLOR_PAGE_FEE_FRC" MASK="99.999" Text='<%# Eval("A3_COLOR_PAGE_FEE_FRC").ToString().Trim() %>' /></td>
        <td><ocxControl:ocxNumber  runat="server" bolEnabled="false" ID="A3_COLOR_PAGE_FEE_FRC_TTL" MASK=",999,999"  /></td>
        <td><ocxControl:ocxNumber  runat="server" bolEnabled="false" ID="A3_COLOR_PAGE_FEE_ORIX" MASK="99.999"  /></td>
        <td><ocxControl:ocxNumber  runat="server" bolEnabled="false" ID="A3_COLOR_PAGE_FEE_ORIX_TTL" MASK=",999,999"  /></td>
        
    </tr>
</table>

</ItemTemplate>
        </asp:Repeater>
        </ContentTemplate>
       
        </asp:UpdatePanel>
    </td>
<td>
<table cellpadding="1" cellspacing="1">
<tr><td><br /><br /></td></tr>
<tr>
    <td></td>
        <td></td>
        <td>係數</td>
        <td><ocxControl:ocxNumber runat="server" ID="COEFFICIENT" MASK="999.9"  Text='<%# Eval("COEFFICIENT").ToString().Trim() %>' /></td>
        <td>估機對開</td>
        <td><ocxControl:ocxNumber runat="server" ID="ESTIMATE" MASK="99,999"  Text='<%# Eval("ESTIMATE").ToString().Trim() %>' /></td>
        
</tr>

<tr>
<th>基本租費</th>
        <td><ocxControl:ocxNumber runat="server" ID="CUST_AMT" MASK="9,999,999" bolEnabled="false" /></td>
        <td colspan="4">合約價值<ocxControl:ocxNumber runat="server" ID="CONTRACT_AMT" MASK="9,999,999"  bolEnabled="false" />發票金額<ocxControl:ocxNumber runat="server" ID="INVOICE_AMT" MASK="9,999,999" bolEnabled="false"  /></td>
</tr>
<tr>
 <th>供應商計</th>
        <td><ocxControl:ocxNumber runat="server" ID="FRC_AMT" MASK="9,999,999" bolEnabled="false"  /></td>
         <td colspan="4">舊機殘值<ocxControl:ocxNumber runat="server" ID="OLDMAC_AMT" MASK="9,999,999"  bolEnabled="false" />全錄輸入<ocxControl:ocxNumber runat="server" ID="XEROX_INPUT" MASK="9,999,999"  Text='<%# Eval("XEROX_INPUT").ToString().Trim() %>' /></td>
</tr>
<tr>
<th>ORIX計</th>
        <td><ocxControl:ocxNumber runat="server" ID="ORIX_AMT" MASK="9,999,999" bolEnabled="false" /></td>
        <td colspan="4">新機實收<ocxControl:ocxNumber runat="server" ID="NEWMAC_AMT" MASK="9,999,999"  bolEnabled="false" />留(欠)款<ocxControl:ocxNumber runat="server" ID="LEAVE_AMT" MASK="9,999,999" bolEnabled="false" /></td>       
        
</tr>
<tr>
<td colspan="6" valign="bottom">
        <div class="divFunction80" style='float:right'>
                <asp:Button runat="server" ID="btnEdit" CssClass="button trn80" Text="明細儲存" OnCommand="GridFunc_Click" CommandName="Save"   />
            </div>  
        </td>

</tr>
</table>
</td></tr></table>
 

 <asp:UpdatePanel runat="server" ID="upObjGrid" RenderMode="Inline" UpdateMode="Conditional">
                <ContentTemplate>
            <div  class="gridMain " style="width:765px;margin:0">
                <div style="padding:0;margin:0; position:relative;  overflow-y:scroll;width:755px;height:130px" id="Div1" >
                    <table cellpadding="0" cellspacing="0"  style="width:700px" > 
                    <thead>
                        <tr>      
                            <th>編輯</th>          
                            <th>期數起</th>
                            <th>期數迄</th>
                            <th>基本租金</th>
                            <th>每月A4<br />黑白基本張數</th>
                            <th>黑白A4<br />單張收費</th>
                            <th>黑白A4<br />單張供應商收費</th>
                            <th>每月A4<br />彩色基本張數</th>
                            <th>彩色A4<br />單張收費</th>
                            <th>彩色A4<br />單張供應商收費</th>
                            <th>每月A3<br />黑白基本張數</th>
                            <th>黑白A3<br />單張收費</th>
                            <th>黑白A3<br />單張供應商收費</th>
                            <th>每月A3<br />彩色基本張數</th>
                            <th>彩色A3<br />單張收費</th>
                            <th>彩色A3<br />單張供應商收費</th>                            
                        </tr> 
                    </thead>    
                        <asp:Repeater runat="server" ID="rptObjGrid"> 
                            <ItemTemplate>
                                <tr id='trObj<%#Container.ItemIndex+1%>' class="<%#Container.ItemIndex%2==0?"srow":"" %>"  > 
                                    <td>                                    
                                    <asp:Button ID="btnDel_Object" runat="server"  cssClass="button del"  Text="刪除" OnClientClick="return window.confirm('是否確定刪除此筆資料?');"  OnCommand="GridFunc_Click" CommandName='<%# Eval("APLY_NO").ToString().Trim()+","+Eval("PERIOD_S").ToString().Trim() %>' />
                                    <asp:Button ID="btnUpd_Object" runat="server"  cssClass="button upd"  Text="修改"  OnCommand="GridFunc_Click" CommandName='<%# Eval("APLY_NO").ToString().Trim()+","+Eval("PERIOD_S").ToString().Trim() %>' />
                                    </td>
                                    <td class="number"><%# Eval("PERIOD_S", "{0:###,###,###,##0}")%></td>
                                    <td class="number"><%# Eval("PERIOD_E", "{0:###,###,###,##0}")%></td>
                                    <td class="number"><%# Eval("BASE_RENT_FEE", "{0:###,###,###,##0}")%></td>
                                    <td class="number"><%# Eval("A4_MONO_MONTH_BASE_PAGE", "{0:###,###,###,##0}")%></td>
                                    <td class="number"><%# Eval("A4_MONO_PAGE_FEE", "{0:###,###,###,##0.#0}")%></td>
                                    <td class="number"><%# Eval("A4_MONO_PAGE_FEE_FRC", "{0:###,###,###,##0.#0}")%></td>
                                    <td class="number"><%# Eval("A4_COLOR_MONTH_BASE_PAGE", "{0:###,###,###,##0}")%></td>
                                    <td class="number"><%# Eval("A4_COLOR_PAGE_FEE", "{0:###,###,###,##0.#0}")%></td>
                                    <td class="number"><%# Eval("A4_COLOR_PAGE_FEE_FRC", "{0:###,###,###,##0.#0}")%></td>
                                    <td class="number"><%# Eval("A3_MONO_MONTH_BASE_PAGE", "{0:###,###,###,##0}")%></td>
                                    <td class="number"><%# Eval("A3_MONO_PAGE_FEE", "{0:###,###,###,##0.#0}")%></td>
                                    <td class="number"><%# Eval("A3_MONO_PAGE_FEE_FRC", "{0:###,###,###,##0.#0}")%></td>
                                    <td class="number"><%# Eval("A3_COLOR_MONTH_BASE_PAGE", "{0:###,###,###,##0}")%></td>
                                    <td class="number"><%# Eval("A3_COLOR_PAGE_FEE", "{0:###,###,###,##0.#0}")%></td>
                                    <td class="number"><%# Eval("A3_COLOR_PAGE_FEE_FRC", "{0:###,###,###,##0.#0}")%></td>
                                </tr>
                            </ItemTemplate> 
                        </asp:Repeater>                       
                        <tr id='trObj0' >
                            <td colspan="16"><asp:Button ID="btnAdd_Object" runat="server"  cssClass="button dadd"  Text="新增" OnCommand="GridFunc_Click" CommandName="" /> </td>                                                
                        </tr>
                    </table>
                </div>     
            </div> 
                </ContentTemplate>
            </asp:UpdatePanel>

<table>
<tr> 
    <td rowspan="2" valign="top"><asp:CheckBox runat="server" id="chkINCREASE" ToolTip='<%# Eval("INCREASE") %>' OnPreRender='checkList' Text="是否加價版" style="margin-right:15px" /></td>
    <th>黑白A4超印費：</th>
    <td><ocxControl:ocxNumber runat="server" ID="EXCEED_FEE_A4_MONO" MASK="99,999" Text='<%# Eval("EXCEED_FEE_A4_MONO") %>'  /></td>
    <th>黑白A3超印費：</th>
    <td><ocxControl:ocxNumber runat="server" ID="EXCEED_FEE_A3_MONO" MASK="99,999" Text='<%# Eval("EXCEED_FEE_A3_MONO") %>'/></td>
    <th>彩色A4超印費：</th>
    <td><ocxControl:ocxNumber runat="server" ID="EXCEED_FEE_A4_COLOR" MASK="99,999" Text='<%# Eval("EXCEED_FEE_A4_COLOR") %>'  /></td>
    <th>彩色A3超印費：</th>
    <td><ocxControl:ocxNumber runat="server" ID="EXCEED_FEE_A3_COLOR" MASK="99,999" Text='<%# Eval("EXCEED_FEE_A3_COLOR") %>'/></td>
    <th>誤印率：</th>
    <td><ocxControl:ocxNumber runat="server" ID="MISPRINT_RATE" MASK="99,999" Text='<%# Eval("MISPRINT_RATE") %>'/></td>
</tr>
<tr>
    <td colspan="6"></td>
    <th>彩色A3加價費：</th>
    <td><ocxControl:ocxNumber runat="server" ID="INCREASE_FEE_A3_COLOR" MASK="99,999" Text='<%# Eval("INCREASE_FEE_A3_COLOR") %>'  /></td>
</tr>
</table>
                    
</fieldset>
        
<script language="javascript" type="text/javascript">
    

    function INCREASE_init(){
        $('input[id*=chkINCREASE]').change(setEXCEED);
        setEXCEED(); 
    }
         
     function init() {
                  
        $('input[id*=A4_MONO_],input[id*=A3_MONO_],input[id*=A4_COLOR_],input[id*=A3_COLOR_],input[id*=COEFFICIENT],input[id*=BASE_RENT_FEE],input[id*=XEROX_INPUT]').change(FEE_Change);
      //  $('input[id*=INCREASE]').change(setEXCEED);
      //  setEXCEED();   
    }

   
    
    function setEXCEED(){
        $('input[id*=INCREASE_FEE_A3_COLOR]').removeAttr("readonly");//.attr('readonly', false);
        $('input[id*=EXCEED_FEE_A3_COLOR]').removeAttr("readonly");//.attr('readonly', false);
        $('input[id*=EXCEED_FEE_A3_COLOR]').removeAttr("tabindex");//.attr('readonly', false);
        $('input[id*=INCREASE_FEE_A3_COLOR]').removeAttr("tabindex");//.attr('readonly', false
        
        $('input[id*=INCREASE_FEE_A3_COLOR]').removeClass( "display" )
        $('input[id*=EXCEED_FEE_A3_COLOR]').removeClass("display");

        var check = document.getElementById($('input[id*=chkINCREASE]').attr('id')).checked;        
        if (!check)
        {
            $('input[id*=INCREASE_FEE_A3_COLOR]').attr("readonly",true);//.attr('readonly', false);
            $('input[id*=INCREASE_FEE_A3_COLOR]').addClass( "display" )
            $('input[id*=INCREASE_FEE_A3_COLOR]').attr( "tabindex",9999 );
            $('input[id*=INCREASE_FEE_A3_COLOR]').val(0);
        }
        else{
            $('input[id*=EXCEED_FEE_A3_COLOR]').attr("readonly",true);//.attr('readonly', false);
            $('input[id*=EXCEED_FEE_A3_COLOR]').addClass( "display" )
            $('input[id*=EXCEED_FEE_A3_COLOR]').attr( "tabindex",9999 );
            $('input[id*=EXCEED_FEE_A3_COLOR]').val(0);
        }    
        
       // reKeydown();    
    }
    
     
    
    function FEE_Change() {
        var oldValue = "";
        try {
            if ((this.id).indexOf("PAGE_FEE") != -1) {

                $(this).val(parseFloat($(this).val()).toFixed(3));
                 oldValue = $(this).attr('oldValue');
            }
        }
        catch (Err) {
        }
        finally { }
       
      
       // alert(this.id);
        var A4_MONO_MONTH_BASE_PAGE = $('input[id*=A4_MONO_MONTH_BASE_PAGE]').val().replace(/,/g, "");
        var A3_MONO_MONTH_BASE_PAGE = $('input[id*=A3_MONO_MONTH_BASE_PAGE]').val().replace(/,/g, "");
        var A4_COLOR_MONTH_BASE_PAGE = $('input[id*=A4_COLOR_MONTH_BASE_PAGE]').val().replace(/,/g, "");
        var A3_COLOR_MONTH_BASE_PAGE = $('input[id*=A3_COLOR_MONTH_BASE_PAGE]').val().replace(/,/g, "");


        $('input[id*=A4_MONO_MONTH_BASE_PAGE_TTL]').val(parseMoney(parseInt(parseInt(A4_MONO_MONTH_BASE_PAGE) * parseFloat($('input[id*=A4_MONO_PAGE_FEE]').val().replace(/,/g, "")))));
        $('input[id*=A4_MONO_PAGE_FEE_FRC_TTL]').val(parseMoney(parseInt(parseInt(A4_MONO_MONTH_BASE_PAGE) * parseFloat($('input[id*=A4_MONO_PAGE_FEE_FRC]').val().replace(/,/g, "")))));
        if (parseFloat($('input[id*=A4_MONO_PAGE_FEE]').val())==0)
            $('input[id*=A4_MONO_PAGE_FEE_ORIX]').val(0);
        else{
            if (parseFloat($('input[id*=A4_MONO_PAGE_FEE]').val())<parseFloat($('input[id*=A4_MONO_PAGE_FEE_FRC]').val()))
            {
                alert("單張金額, 必須大於等於 單張(供應商)");
                if (((this.id).indexOf("A4_MONO_PAGE_FEE") != -1 && oldValue!="") || ((this.id).indexOf("A4_MONO_PAGE_FEE_FRC") != -1 && oldValue!=""))
                {
                    $(this).val(oldValue);
                    $(this).focus();
                }      
                return;
           }
           $('input[id*=A4_MONO_PAGE_FEE_ORIX]').val((parseFloat($('input[id*=A4_MONO_PAGE_FEE]').val().replace(/,/g, "")) - parseFloat($('input[id*=A4_MONO_PAGE_FEE_FRC]').val().replace(/,/g, ""))).toFixed(3));
           
           }           
        
        $('input[id*=A4_MONO_PAGE_FEE_ORIX_TTL]').val(parseMoney(parseInt($('input[id*=A4_MONO_MONTH_BASE_PAGE_TTL]').val().replace(/,/g, "")) - parseInt($('input[id*=A4_MONO_PAGE_FEE_FRC_TTL]').val().replace(/,/g, ""))));
        
            
            
        $('input[id*=A4_COLOR_MONTH_BASE_PAGE_TTL]').val(parseMoney(parseInt(parseInt(A4_COLOR_MONTH_BASE_PAGE) * parseFloat($('input[id*=A4_COLOR_PAGE_FEE]').val().replace(/,/g, "")))));
        $('input[id*=A4_COLOR_PAGE_FEE_FRC_TTL]').val(parseMoney(parseInt(parseInt(A4_COLOR_MONTH_BASE_PAGE) * parseFloat($('input[id*=A4_COLOR_PAGE_FEE_FRC]').val().replace(/,/g, "")))));      
         if (parseFloat($('input[id*=A4_COLOR_PAGE_FEE]').val())==0)
            $('input[id*=A4_COLOR_PAGE_FEE_ORIX]').val(0);
        else{
            if (parseFloat($('input[id*=A4_COLOR_PAGE_FEE]').val())<parseFloat($('input[id*=A4_COLOR_PAGE_FEE_FRC]').val()))
            {
                alert("單張金額, 必須大於等於 單張(供應商)");
                if (((this.id).indexOf("A4_COLOR_PAGE_FEE") != -1 && oldValue!="") || ((this.id).indexOf("A4_COLOR_PAGE_FEE_FRC") != -1 && oldValue!=""))
                {
                    $(this).val(oldValue);
                    $(this).focus();
                }     
                return;
           }
           $('input[id*=A4_COLOR_PAGE_FEE_ORIX]').val((parseFloat($('input[id*=A4_COLOR_PAGE_FEE]').val().replace(/,/g, "")) - parseFloat($('input[id*=A4_COLOR_PAGE_FEE_FRC]').val().replace(/,/g, ""))).toFixed(3));     
        }
        
        $('input[id*=A4_COLOR_PAGE_FEE_ORIX_TTL]').val(parseMoney(parseInt($('input[id*=A4_COLOR_MONTH_BASE_PAGE_TTL]').val().replace(/,/g, "")) - parseInt($('input[id*=A4_COLOR_PAGE_FEE_FRC_TTL]').val().replace(/,/g, ""))));

        $('input[id*=A3_MONO_MONTH_BASE_PAGE_TTL]').val(parseMoney(parseInt(parseInt(A3_MONO_MONTH_BASE_PAGE) * parseFloat($('input[id*=A3_MONO_PAGE_FEE]').val().replace(/,/g, "")))));
        $('input[id*=A3_MONO_PAGE_FEE_FRC_TTL]').val(parseMoney(parseInt(parseInt(A3_MONO_MONTH_BASE_PAGE) * parseFloat($('input[id*=A3_MONO_PAGE_FEE_FRC]').val().replace(/,/g, "")))));
        if (parseFloat($('input[id*=A3_MONO_PAGE_FEE]').val())==0)
            $('input[id*=A3_MONO_PAGE_FEE_ORIX]').val(0);
        else{
            if (parseFloat($('input[id*=A3_MONO_PAGE_FEE]').val())<parseFloat($('input[id*=A3_MONO_PAGE_FEE_FRC]').val()))
            {
                alert("單張金額, 必須大於等於 單張(供應商)");
                if (((this.id).indexOf("A3_MONO_PAGE_FEE") != -1 && oldValue!="") || ((this.id).indexOf("A3_MONO_PAGE_FEE_FRC") != -1 && oldValue!=""))
                {
                    $(this).val(oldValue);
                    $(this).focus();
                }         
                return;
           }
           $('input[id*=A3_MONO_PAGE_FEE_ORIX]').val((parseFloat($('input[id*=A3_MONO_PAGE_FEE]').val().replace(/,/g, "")) - parseFloat($('input[id*=A3_MONO_PAGE_FEE_FRC]').val().replace(/,/g, ""))).toFixed(3));
        }
        
        $('input[id*=A3_MONO_PAGE_FEE_ORIX_TTL]').val(parseMoney(parseInt($('input[id*=A3_MONO_MONTH_BASE_PAGE_TTL]').val().replace(/,/g, "")) - parseInt($('input[id*=A3_MONO_PAGE_FEE_FRC_TTL]').val().replace(/,/g, ""))));


        $('input[id*=A3_COLOR_MONTH_BASE_PAGE_TTL]').val(parseMoney(parseInt(A3_COLOR_MONTH_BASE_PAGE) * parseFloat($('input[id*=A3_COLOR_PAGE_FEE]').val().replace(/,/g, ""))));
        $('input[id*=A3_COLOR_PAGE_FEE_FRC_TTL]').val(parseMoney(parseInt(parseInt(A3_COLOR_MONTH_BASE_PAGE) * parseFloat($('input[id*=A3_COLOR_PAGE_FEE_FRC]').val().replace(/,/g, "")))));
        if (parseFloat($('input[id*=A3_COLOR_PAGE_FEE]').val())==0)
            $('input[id*=A3_COLOR_PAGE_FEE_FRC_TTL]').val(0);
        else{
            if (parseFloat($('input[id*=A3_COLOR_PAGE_FEE_ORIX]').val())<parseFloat($('input[id*=A3_COLOR_PAGE_FEE_FRC]').val()))
            {
                alert("單張金額, 必須大於等於 單張(供應商)");
               if (((this.id).indexOf("A3_COLOR_PAGE_FEE") != -1 && oldValue!="") || ((this.id).indexOf("A3_COLOR_PAGE_FEE_FRC") != -1 && oldValue!=""))
                {
                    $(this).val(oldValue);
                    $(this).focus();
                }     
                return;
           }
          $('input[id*=A3_COLOR_PAGE_FEE_ORIX]').val((parseFloat($('input[id*=A3_COLOR_PAGE_FEE]').val().replace(/,/g, "")) - parseFloat($('input[id*=A3_COLOR_PAGE_FEE_FRC]').val().replace(/,/g, ""))).toFixed(3));
        }   
        
        $('input[id*=A3_COLOR_PAGE_FEE_ORIX_TTL]').val(parseMoney(parseInt($('input[id*=A3_COLOR_MONTH_BASE_PAGE_TTL]').val().replace(/,/g, "")) - parseInt($('input[id*=A3_COLOR_PAGE_FEE_FRC_TTL]').val().replace(/,/g, ""))));



        var CUST_AMT = parseInt($('input[id*=BASE_RENT_FEE]').val().replace(/,/g, "")) + parseInt($('input[id*=A4_MONO_MONTH_BASE_PAGE_TTL]').val().replace(/,/g, "")) + parseInt($('input[id*=A4_COLOR_MONTH_BASE_PAGE_TTL]').val().replace(/,/g, "")) + parseInt($('input[id*=A3_MONO_MONTH_BASE_PAGE_TTL]').val().replace(/,/g, "")) + parseInt($('input[id*=A3_COLOR_MONTH_BASE_PAGE_TTL]').val().replace(/,/g, ""));
        var FRC_AMT = parseInt($('input[id*=A4_MONO_PAGE_FEE_FRC_TTL]').val().replace(/,/g, "")) + parseInt($('input[id*=A4_COLOR_PAGE_FEE_FRC_TTL]').val().replace(/,/g, "")) + parseInt($('input[id*=A3_MONO_PAGE_FEE_FRC_TTL]').val().replace(/,/g, "")) + parseInt($('input[id*=A3_COLOR_PAGE_FEE_FRC_TTL]').val().replace(/,/g, ""));
        var ORIX_AMT=CUST_AMT-FRC_AMT;
        var CONTRACT_AMT = parseInt(ORIX_AMT * parseFloat($('input[id*=COEFFICIENT]').val().replace(/,/g, "")));
        var NEWMAC_AMT=CONTRACT_AMT-0;
        var INVOICE_AMT = NEWMAC_AMT + parseInt($('input[id*=ESTIMATE]').val().replace(/,/g, ""));

        $('input[id*=CUST_AMT]').val(parseMoney(CUST_AMT));
        $('input[id*=FRC_AMT]').val(parseMoney(FRC_AMT));
        $('input[id*=ORIX_AMT]').val(parseMoney(ORIX_AMT));
        $('input[id*=CONTRACT_AMT]').val(parseMoney(CONTRACT_AMT));
        $('input[id*=OLDMAC_AMT]').val(0);
        $('input[id*=NEWMAC_AMT]').val(parseMoney(NEWMAC_AMT));
        $('input[id*=INVOICE_AMT]').val(parseMoney(INVOICE_AMT));
        $('input[id*=LEAVE_AMT]').val(parseMoney(INVOICE_AMT - parseInt($('input[id*=XEROX_INPUT]').val().replace(/,/g, ""))));
        
      
        
                
    }
    window.setTimeout("INCREASE_init()","1000");
        
</script>

</asp:Content> 
