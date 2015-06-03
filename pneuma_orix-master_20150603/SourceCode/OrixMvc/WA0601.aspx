<%@ Page Language="C#" EnableEventValidation="false" AutoEventWireup="true" MasterPageFile="~/Pattern/editing.Master"
    CodeBehind="WA0601.aspx.cs" Inherits="OrixMvc.WA0601" %>

<%@ MasterType VirtualPath="~/Pattern/editing.Master" %>
<%@ Register TagName="ocxDate" TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxYM" TagPrefix="ocxControl" Src="~/ocxControl/ocxYM.ascx" %>
<%@ Register TagName="ocxYear" TagPrefix="ocxControl" Src="~/ocxControl/ocxYear.ascx" %>
<%@ Register TagName="ocxDialog" TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxTime" TagPrefix="ocxControl" Src="~/ocxControl/ocxTime.ascx" %>
<%@ Register TagName="ocxNumber" TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload" TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>
<asp:Content ContentPlaceHolderID="dataArea" runat="server" ID="myDataArea">
    <table style="">
        <tr>
            <th>
                申請書編號：
            </th>
            <td>
                <asp:TextBox runat="server" ID="APLY_NO" CssClass="display" Width="150"></asp:TextBox>
            </td>
            <th>
                目前狀況：
            </th>
            <td>
                <asp:TextBox runat="server" ID="CUR_STS" CssClass="display" Width="100"></asp:TextBox>
            </td>
        </tr>
    </table>
    <asp:SqlDataSource EnableViewState="true" SelectCommandType="Text" ID="sqlCITY_CODE"
        ConnectionString="<%$ ConnectionStrings:myConnectionString%>" SelectCommand="select ' ' city_code,'請選擇..' City_name union all select city_code,City_name from or3_zip where city_code!='' group by city_code, city_name  order by city_code "
        runat="server"></asp:SqlDataSource>
    <asp:SqlDataSource EnableViewState="true" SelectCommandType="Text" ID="sqlCASE_SOUR"
        ConnectionString="<%$ ConnectionStrings:myConnectionString%>" SelectCommand="Exec s_ConditionItems 'CASE_SOUR'"
        runat="server"></asp:SqlDataSource>
    <asp:SqlDataSource EnableViewState="true" SelectCommandType="Text" ID="sqlPAY_PERD"
        ConnectionString="<%$ ConnectionStrings:myConnectionString%>" SelectCommand="Exec s_ConditionItems 'PAY_PERD'"
        runat="server"></asp:SqlDataSource>
    <asp:SqlDataSource EnableViewState="true" SelectCommandType="Text" ID="sqlTAX_TYPE"
        ConnectionString="<%$ ConnectionStrings:myConnectionString%>" SelectCommand="Exec s_ConditionItems 'TAX_TYPE'"
        runat="server"></asp:SqlDataSource>
    <asp:SqlDataSource EnableViewState="true" SelectCommandType="Text" ID="sqlPAY_MTHD"
        ConnectionString="<%$ ConnectionStrings:myConnectionString%>" SelectCommand="Exec s_ConditionItems 'PAY_MTHD'"
        runat="server"></asp:SqlDataSource>
    <asp:SqlDataSource EnableViewState="true" SelectCommandType="Text" ID="sqlAMOR_MTHD"
        ConnectionString="<%$ ConnectionStrings:myConnectionString%>" SelectCommand="Exec s_ConditionItems 'AMOR_MTHD'"
        runat="server"></asp:SqlDataSource>
    <asp:SqlDataSource EnableViewState="true" SelectCommandType="Text" ID="sqlCON_TYPE"
        ConnectionString="<%$ ConnectionStrings:myConnectionString%>" SelectCommand="Exec s_ConditionItems 'CON_TYPE'"
        runat="server"></asp:SqlDataSource>
    <asp:SqlDataSource EnableViewState="true" SelectCommandType="Text" ID="sqlIF_PASS"
        ConnectionString="<%$ ConnectionStrings:myConnectionString%>" SelectCommand="Exec s_ConditionItems'REVIEW_RESULT'"
        runat="server"></asp:SqlDataSource>
    <asp:SqlDataSource EnableViewState="true" SelectCommandType="Text" ID="sqlSCUR_NATUR"
        ConnectionString="<%$ ConnectionStrings:myConnectionString%>" SelectCommand="Exec s_ConditionItems'SCUR_NATUR'"
        runat="server"></asp:SqlDataSource>
    <asp:SqlDataSource EnableViewState="true" SelectCommandType="Text" ID="sqlSCUR_RELATION"
        ConnectionString="<%$ ConnectionStrings:myConnectionString%>" SelectCommand="select TypeCode, TypeDesc from (select '' TypeCode,'' TypeDesc,seq=0  union all select toge_no TypeCode,toge_name TypeDesc,seq from or_common_code where toge_group='A01' and (end_date is null or end_date='')) s order by seq"
        runat="server"></asp:SqlDataSource>
    <asp:SqlDataSource EnableViewState="true" SelectCommandType="Text" ID="sqlASUR_TYPE_CODE"
        ConnectionString="<%$ ConnectionStrings:myConnectionString%>" SelectCommand="select '' TypeCode,'' TypeDesc,'' ASUR_PAYER,0.0000 as ASUR_FACTOR  union all select ASUR_TYPE_CODE TypeCode,ASUR_TYPE_NAME TypeDesc,ASUR_PAYER,ASUR_FACTOR  from OR_ASUR_TYPE where ltrim(ASUR_TYPE_CODE) !='' order by 1"
        runat="server"></asp:SqlDataSource>
    <asp:SqlDataSource EnableViewState="true" SelectCommandType="Text" ID="sqlONUS" ConnectionString="<%$ ConnectionStrings:myConnectionString%>"
        SelectCommand="Exec s_ConditionItems'ONUS'" runat="server"></asp:SqlDataSource>
    <asp:SqlDataSource EnableViewState="true" SelectCommandType="Text" ID="sqlTMP_CODE"
        ConnectionString="<%$ ConnectionStrings:myConnectionString%>" SelectCommand="
select TMP_CODE='',TMP_DESC='請選擇...' union all select TMP_CODE,TMP_DESC from OR3_CONTRACT_TEMP_SET"
        runat="server"></asp:SqlDataSource>

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
function tabD_select(intIndex, intLen) {
    for (var i = 1; i <= intLen; i++) {
        document.getElementById("tabD" + i.toString()).className = "";
        document.getElementById("tabpanelD" + i.toString()).style.display = "none";
    }

    document.getElementById("tabD" + intIndex.toString()).className = "tabs-selected";
    document.getElementById("tabpanelD" + intIndex.toString()).style.display = "block";

}

function tabS_select(intIndex, intLen) {
    for (var i = 1; i <= intLen; i++) {
        document.getElementById("tabS" + i.toString()).className = "";
        document.getElementById("tabpanelS" + i.toString()).style.display = "none";
    }

    document.getElementById("tabS" + intIndex.toString()).className = "tabs-selected";
    document.getElementById("tabpanelS" + intIndex.toString()).style.display = "block";

}

var oldObj;
var oldIndex;
$("#<%=this.txtOpin.ClientID %>").val("");
function aud_mouseover(obj,intIndex){
//alert(intIndex);
    
    var nowid="ctl00_ctl00_body_dataArea_rptOpin_ctl0"+(intIndex-1).toString()+"_AUD_OPIN";
    
   
    if (oldObj) {
        oldObj.className = "";
        oldid="ctl00_ctl00_body_dataArea_rptOpin_ctl0"+(oldIndex-1).toString()+"_AUD_OPIN";
        document.getElementById(oldid).value=document.getElementById("<%=this.txtOpin.ClientID %>").innerHTML;
    }
    obj.className="crow";
    document.getElementById("<%=this.txtOpin.ClientID %>").innerHTML =  document.getElementById(nowid).value;
    var boldisabled = $("#ctl00_ctl00_body_dataArea_rptOpin_ctl0"+(intIndex-1).toString()+"_IF_PASS").attr('disabled');
   

    document.getElementById("<%=this.txtOpin.ClientID %>").disabled = boldisabled;

    if (boldisabled)
        document.getElementById("<%=this.txtOpin.ClientID %>").className = "display";
    
    oldObj = obj;
    oldIndex=intIndex;
    
    var status="<%=this.status %>";
    var empname="<%=this.empname %>";
    if (intIndex==1 && !boldisabled && document.getElementById("<%=this.txtOpin.ClientID %>").innerHTML=="" && (status=="Upd" || status=="UpdAfter" || status=="Query"))
    {
        document.getElementById("<%=this.txtOpin.ClientID %>").innerHTML="申請者："+empname;
        document.getElementById(nowid).value="申請者："+empname;
    }
    if (intIndex!=1 && !boldisabled && document.getElementById("<%=this.txtOpin.ClientID %>").innerHTML=="" && (status=="UpdAfter" || status=="Query"))
    {
        document.getElementById("<%=this.txtOpin.ClientID %>").innerHTML=empname;
        document.getElementById(nowid).value=empname;
    }
     
}

function aud_mouseout(){
       if (oldObj) {
    var nowid="ctl00_ctl00_body_dataArea_rptOpin_ctl0"+(oldIndex-1).toString()+"_AUD_OPIN";
    document.getElementById(nowid).value=document.getElementById("<%=this.txtOpin.ClientID %>").innerHTML; 
    }
    
}



    var aReload;
    function reloadData(AplyNo, STS) {
                
        window.parent.Loading();
        
        window.clearTimeout(aReload);
        document.getElementById("<%=this.APLY_NO.ClientID %>").value = AplyNo;
        document.getElementById("<%=this.CUR_STS.ClientID %>").value = STS;
        
        aReload=window.setTimeout("startReload()", "100");
    }
    
    function startReload() {
       // window.setTimeout("window.parent.Loading()", "");
        
        document.getElementById("<%=this.btnReloadData.ClientID %>").click();
    }
    
    </script>

    <span style="display: none">
        <asp:Button runat="server" ID="btnReloadData" OnClick="reload_data" />
    </span>
    <div style="width: 820px; height: auto;">
        <div class="tabs-header" style="width: 820px;">
            <div class="tabs-scroller-left" style="display: none;">
            </div>
            <div class="tabs-scroller-right" style="display: none;">
            </div>
            <div class="tabs-wrap" style="margin-left: 0px; margin-right: 0px; width: 810px;">
                <ul class="tabs">
                    <li class="tabs-selected" id="tab1" onclick="tab_select(1,11)"><a href="javascript:void(0)"
                        class="tabs-inner"><span class="tabs-title">基本資料</span><span class="tabs-icon"></span></a></li>
                    <li class="" id="tab2" onclick="tab_select(2,11)"><a href="javascript:void(0)" class="tabs-inner">
                        <span class="tabs-title">型態標的物</span><span class="tabs-icon"></span></a></li>
                    <li class="" id="tab3" onclick="tab_select(3,11)"><a href="javascript:void(0)" class="tabs-inner">
                        <span class="tabs-title">申請條件</span><span class="tabs-icon"></span></a></li>
                    <li class="" id="tab4" onclick="tab_select(4,11)"><a href="javascript:void(0)" class="tabs-inner">
                        <span class="tabs-title">擔保條件</span><span class="tabs-icon"></span></a></li>
                    <li class="" id="tab5" onclick="tab_select(5,11)"><a href="javascript:void(0)" class="tabs-inner">
                        <span class="tabs-title">實行條件</span><span class="tabs-icon"></span></a></li>
                    <li class="" id="tab6" onclick="tab_select(6,11)"><a href="javascript:void(0)" class="tabs-inner">
                        <span class="tabs-title">往來實績</span><span class="tabs-icon"></span></a></li>
                    <li class="" id="tab7" onclick="tab_select(7,11)"><a href="javascript:void(0)" class="tabs-inner">
                        <span class="tabs-title">權限條件</span><span class="tabs-icon"></span></a></li>
                    <li class="" id="tab8" onclick="tab_select(8,11)"><a href="javascript:void(0)" class="tabs-inner">
                        <span class="tabs-title">授信權限</span><span class="tabs-icon"></span></a></li>
                    <li class="" id="tab9" onclick="tab_select(9,11)"><a href="javascript:void(0)" class="tabs-inner">
                        <span class="tabs-title">審查</span><span class="tabs-icon"></span></a></li>
                    <li class="" id="tab10" onclick="tab_select(10,11)"><a href="javascript:void(0)"
                        class="tabs-inner"><span class="tabs-title">承租人</span><span class="tabs-icon"></span></a></li>
                    <li class="" id="tab11" onclick="tab_select(11,11)"><a href="javascript:void(0)"
                        class="tabs-inner"><span class="tabs-title">其他條件</span><span class="tabs-icon"></span></a></li>
                </ul>
            </div>
        </div>
        <asp:HiddenField runat="server" ID="hiddenREQU_ZIP" />
        <div class="tabs-panels" style="width: 820px;">
            <div class="panel" style="height: auto; width: 810px; display: block" id="tabpanel1">
                <asp:Repeater runat="server" ID="rptBase">
                    <ItemTemplate>
                        <table style="width: 810px; margin: 0px">
                            <tr>
                                <th class="nonSpace">
                                    申請日期：
                                </th>
                                <td>
                                    <ocxControl:ocxDate runat="server" ID="APLY_DATE" Text='<%# Eval("APLY_DATE").ToString().Trim() %>' />
                                </td>
                                <th class="nonSpace">
                                    申請單位：
                                </th>
                                <td>
                                    <ocxControl:ocxDialog runat="server" ID="DEPT_CODE" width="60" Text='<%# Eval("DEPT_CODE").ToString().Trim() %>'
                                        ControlID="DEPT_NAME" FieldName="DEPT_NAME" SourceName="OR_DEPT" />
                                </td>
                                <td>
                                    <input type="text" class="display" style="width: 100px" value='<%# Eval("DEPT_NAME").ToString().Trim() %>'
                                        id="DEPT_NAME" />
                                </td>
                                <th class="nonSpace">
                                    經辦代號：
                                </th>
                                <td>
                                    <ocxControl:ocxDialog runat="server" ID="EMP_CODE" Text='<%# Eval("EMP_CODE") %>'
                                        width="60" ControlID="EMP_NAME" FieldName="EMP_NAME" SourceName="OR_EMP" />
                                </td>
                                <td>
                                    <input type="text" class="display" style="width: 70px" value='<%# Eval("EMP_NAME").ToString().Trim() %>'
                                        id="EMP_NAME" />
                                </td>
                            </tr>
                            <tr>
                                <th class="nonSpace">
                                    客戶代號：
                                </th>
                                <td>
                                    <ocxControl:ocxDialog runat="server" ID="CUST_NO" width="80" Text='<%# Eval("CUST_NO").ToString().Trim() %>'
                                        ControlID="CUST_SNAME" FieldName="CUST_SNAME" SourceName="OR_CUSTOM" />
                                </td>
                                <td colspan="4">
                                    <input type="text" readonly class="display" style="width: 170px" value='<%# Eval("CUST_SNAME").ToString().Trim() %>'
                                        id="CUST_SNAME" />
                                    <asp:HiddenField runat="server" Value='<%# Eval("CUST_EMAIL_ADDR").ToString().Trim() %>'
                                        ID="CUST_EMAIL_ADDR" />
                                    全方位帳號：
                                    <asp:TextBox runat="server" ID="ACCNO" Width="130" MaxLength="20" ReadOnly="true"
                                        CssClass="display"></asp:TextBox>
                                </td>
                                <th>
                                    初次接觸日：
                                </th>
                                <td>
                                    <ocxControl:ocxDate runat="server" ID="InitContactDate" Text='<%# Eval("InitContactDate").ToString().Trim() %>' />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    主約編號：
                                </th>
                                <td colspan="2">
                                    <ocxControl:ocxDialog runat="server" ID="MAST_CON_NO" Text='<%# Eval("MAST_CON_NO") %>'
                                        width="120" SourceName="OR3_MASTER_CONTRACT" />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    契約編號：
                                </th>
                                <td colspan="2">
                                    <asp:TextBox runat="server" ID="CON_SEQ_NO" Width="130" CssClass="display" MaxLength="20"
                                        Text='<%# Eval("CON_SEQ_NO").ToString().Trim() %>'></asp:TextBox>
                                </td>
                                <th>
                                    舊契約編號：
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="OLD_CON_NO" Width="130" MaxLength="20" Text='<%# Eval("OLD_CON_NO").ToString().Trim() %>'></asp:TextBox>
                                </td>
                                <td>
                                    <font class="memo">
                                        <%# Eval("PROJECT") %></font>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    原額度編號：
                                </th>
                                <td colspan="2">
                                    <input type="text" class="display" style="width: 130px" value='<%# Eval("ORG_QUOTA_APLY_NO").ToString().Trim() %>' />
                                </td>
                                <th>
                                    現額度編號：
                                </th>
                                <td>
                                    <ocxControl:ocxDialog runat="server" ID="CUR_QUOTA_APLY_NO" Text='<%# Eval("CUR_QUOTA_APLY_NO") %>'
                                        width="120" SourceName="OR3_QUOTA_APLY_BASE" />
                                </td>
                                <th>
                                    新部門：
                                </th>
                                <td>
                                    <input type="text" class="display" style="width: 70px" value='<%# Eval("N_DEPT_CODE").ToString().Trim() %>' />
                                </td>
                                <td>
                                    <input type="text" class="display" style="width: 70px" value='<%# Eval("N_DEPT_NAME").ToString().Trim() %>' />
                                </td>
                            </tr>
                            <tr>
                                <th class="nonSpace">
                                    請款地址：
                                </th>
                                <td colspan="4">
                                    <asp:UpdatePanel runat="server" ID="upZIP_CODE" UpdateMode="Conditional" RenderMode="Inline">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="CITY_CODE" AutoPostBack="true" runat="server" OnPreRender='checkList'
                                                ToolTip='<%# Eval("CITY_CODE").ToString().Trim() %>' DataSourceID="sqlCITY_CODE"
                                                DataValueField="CITY_CODE" DataTextField="CITY_NAME" OnSelectedIndexChanged="ZIP_CODE_LOAD">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="REQU_ZIP" runat="server" AutoPostBack="true" OnPreRender='checkList'
                                                OnSelectedIndexChanged='SetAddress' ToolTip='<%# Eval("REQU_ZIP").ToString().Trim() %>'>
                                            </asp:DropDownList>
                                            <asp:TextBox runat="server" ID="REQ_PAY_ADDR" size="50" MaxLength="80" Width="200"
                                                Text='<%# Eval("REQ_PAY_ADDR").ToString().Trim() %>'></asp:TextBox>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="REQU_ZIP" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <th>
                                    新經辦：
                                </th>
                                <td>
                                    <input type="text" class="display" style="width: 70px" value='<%# Eval("N_EMP_CODE").ToString().Trim() %>' />
                                </td>
                                <td>
                                    <input type="text" class="display" style="width: 70px" value='<%# Eval("N_EMP_NAME").ToString().Trim() %>' />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    收件人：
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="RECVER" MaxLength="20" Width="90" Text='<%# Eval("RECVER").ToString().Trim() %>'></asp:TextBox>
                                </td>
                                <th>
                                    收件部門：
                                </th>
                                <td colspan="2">
                                    <asp:TextBox runat="server" ID="RECVER_DEPT" MaxLength="20" Width="200" Text='<%# Eval("RECVER_DEPT").ToString().Trim() %>'></asp:TextBox>
                                </td>
                                <th>
                                    統一郵寄：
                                </th>
                                <td>
                                    <ocxControl:ocxDialog SourceName="OR_MERG_MAIL" width="60" ControlID="MMAIL_NAME"
                                        FieldName="MMAIL_NAME" runat="server" Text='<%# Eval("MMAIL_NO").ToString().Trim() %>'
                                        ID="MMAIL_NO" />
                                </td>
                                <td>
                                    <input type="text" readonly class="display" style="width: 70px" value='<%# Eval("MMAIL_NAME").ToString().Trim() %>'
                                        id="MMAIL_NAME" />
                                </td>
                            </tr>
                            <tr>
                                <th class="nonSpace">
                                    聯絡人：
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="CONTACT" Width="90" size="10" MaxLength="12" Text='<%# Eval("Contact").ToString().Trim() %>'></asp:TextBox>
                                </td>
                                <th class="nonSpace">
                                    電話：
                                </th>
                                <td colspan>
                                    <asp:TextBox runat="server" ID="CTAC_TEL" Width="90" size="10" MaxLength="12" Text='<%# Eval("CTAC_TEL").ToString().Trim() %>'></asp:TextBox>
                                </td>
                                <th class="nonSpace">
                                    傳真：
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="FAX" Width="100" size="10" MaxLength="12" Text='<%# Eval("FAX").ToString().Trim() %>'></asp:TextBox>
                                </td>
                                <th>
                                    手機：
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="MOBILE" Width="100" size="10" MaxLength="12" Text='<%# Eval("MOBILE").ToString().Trim() %>'></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="vertical-align: top">
                                    供應商背景資力：<asp:TextBox runat="server" ID="SupplierBackground" TextMode="MultiLine"
                                        Rows="3" Width="240" Text='<%# Eval("SupplierBackground").ToString().Trim() %>'></asp:TextBox>
                                </td>
                                <td colspan="4" style="vertical-align: top">
                                    同業往來主要條件：<asp:TextBox runat="server" ID="MainCondition" TextMode="MultiLine" Rows="3"
                                        Width="240" Text='<%# Eval("MainCondition").ToString().Trim() %>'></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:UpdatePanel runat="server" ID="upCustomBase" RenderMode="Inline" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Repeater runat="server" ID="rptBaseCustom">
                            <ItemTemplate>
                                <table style="width: 810px; background-color: #E3E3E3">
                                    <tr>
                                        <th>
                                            負責人：
                                        </th>
                                        <td>
                                            <div style="width: 100px" class="text">
                                                <%# Eval("TAKER").ToString().Trim() %></div>
                                        </td>
                                        <th>
                                            設立日期：
                                        </th>
                                        <td>
                                            <asp:TextBox runat="server" ID="BUILD_DATE" CssClass="display" ReadOnly="true" Width="100"
                                                MaxLength="20" Text='<%# Eval("BUILD_DATE").ToString().Trim() %>'></asp:TextBox>
                                        </td>
                                        <th>
                                            客戶簡介：
                                        </th>
                                        <td rowspan="8" style="vertical-align: top">
                                            <textarea style="width: 250px; background-color: Transparent" cols="51" readonly="readonly"
                                                class="display" rows="12"><%# Eval("BACKGROUND").ToString().Trim()%></textarea>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            營業登記地址：
                                        </th>
                                        <td colspan="3">
                                            <asp:TextBox runat="server" ID="SALES_RGT_ADDR" Width="250px" CssClass="display"
                                                size="10" MaxLength="12" Text='<%# Eval("SALES_RGT_ADDR").ToString().Trim() %>'></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            資本結構：
                                        </th>
                                        <td>
                                            <div style="width: 120px" class="text">
                                                <%# Eval("CAPT_STR").ToString().Trim()%></div>
                                        </td>
                                        <th>
                                            組織型態：
                                        </th>
                                        <td>
                                            <div style="width: 120px" class="text">
                                                <%# Eval("ORG_TYPE").ToString().Trim()%></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            登記資本額：
                                        </th>
                                        <td>
                                            <div style="width: 120px" class="text number">
                                                <%# Eval("RGT_CAPT_AMT","{0:###,###,###,##0}") %></div>
                                            萬元
                                        </td>
                                        <th>
                                            員工人數：
                                        </th>
                                        <td>
                                            <div style="width: 120px" class="text number">
                                                <%# Eval("EMP_PSNS", "{0:###,###,###,##0}")%></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            實收資本額：
                                        </th>
                                        <td>
                                            <div style="width: 120px" class="text number">
                                                <%# Eval("REAL_CAPT_AMT", "{0:###,###,###,##0}")%></div>
                                            萬元
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            主要營業項目：
                                        </th>
                                        <td colspan="3">
                                            <div style="width: 180px" class="text">
                                                <%# Eval("MAIN_BUS_ITEM").ToString().Trim()%></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            信用額度：
                                        </th>
                                        <td>
                                            <div style="width: 120px" class="text number">
                                                <%# Eval("CREDIT_CUST", "{0:###,###,###,##0}")%></div>
                                        </td>
                                        <th>
                                            信用評等：
                                        </th>
                                        <td>
                                            <div style="width: 50px" class="text">
                                                <%# Eval("JUDGE_LVL").ToString().Trim()%></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            廉潔協定：
                                        </th>
                                        <td>
                                            <input type="checkbox" disabled <%# Eval("HONEST_AGREEMENT").ToString()=="Y"?"checked":"" %> />
                                        </td>
                                        <th>
                                            保密約定：
                                        </th>
                                        <td>
                                            <input type="checkbox" disabled <%# Eval("SECRET_PROMISE").ToString()=="Y"?"checked":"" %> />
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="panel" style="height: auto; width: 820px; display: none" id="tabpanel2">
                <asp:Repeater runat="server" ID="rptObjMain">
                    <ItemTemplate>
                        <table style="width: 810px; margin: 0px">
                            <tr>
                                <th>
                                    授權類別：
                                </th>
                                <td>
                                    <ocxControl:ocxDialog runat="server" ID="AUD_LVL_CODE" width="60" Text='<%# Eval("AUD_LVL_CODE").ToString().Trim() %>'
                                        ControlID="AUD_LVL_NAME" FieldName="AUD_LVL_NAME" SourceName="OR_AUD_LVL_NAME" />
                                </td>
                                <td>
                                    <input type="text" class="display" style="width: 70px" value='<%# Eval("AUD_LVL_NAME").ToString().Trim() %>'
                                        id="AUD_LVL_NAME" />
                                </td>
                                <th class="nonSpace">
                                    案件類別：
                                </th>
                                <td>
                                    <ocxControl:ocxDialog runat="server" ID="CASE_TYPE_CODE" width="60" Text='<%# Eval("CASE_TYPE_CODE") %>'
                                        ControlID="CASE_TYPE_NAME" FieldName="CASE_TYPE_NAME" SourceName="OR_CASE_TYPE" />
                                </td>
                                <td>
                                    <input type="text" class="display" style="width: 70px" value='<%# Eval("CASE_TYPE_NAME").ToString().Trim() %>'
                                        id="CASE_TYPE_NAME" />
                                </td>
                                <th class="nonSpace">
                                    審查案件類別：
                                </th>
                                <td>
                                    <ocxControl:ocxDialog runat="server" ID="AUD_CASE_TYPE" width="60" Text='<%# Eval("AUD_CASE_TYPE").ToString().Trim() %>'
                                        ControlID="CAL_NAME" FieldName="CAL_NAME" SourceName="OR_CASE_CAL" />
                                </td>
                                <td>
                                    <input type="text" class="display" style="width: 90px" value='<%# Eval("CAL_NAME").ToString().Trim() %>'
                                        id="CAL_NAME" />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    案件來源：
                                </th>
                                <td>
                                    <asp:DropDownList runat="server" ID="CASE_SOUR" OnPreRender='checkList' ToolTip='<%# Eval("CASE_SOUR").ToString().Trim() %>'
                                        Width="80" DataSourceID="sqlCASE_SOUR" DataValueField="TypeCode" DataTextField="TypeDesc">
                                    </asp:DropDownList>
                                </td>
                                <th>
                                    分開發票：
                                </th>
                                <td>
                                    <asp:DropDownList runat="server" ID="DIVIDE" OnPreRender='checkList' ToolTip='<%# Eval("DIVIDE").ToString().Trim() %>'
                                        Width="50">
                                        <asp:ListItem Value="N">否</asp:ListItem>
                                        <asp:ListItem Value="Y">是</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <th>
                                    介紹人：
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="AGENT" Width="70" MaxLength="20" Text='<%# Eval("AGENT").ToString().Trim() %>'></asp:TextBox>
                                </td>
                                <th>
                                    其他：
                                </th>
                                <td colspan="2">
                                    <asp:TextBox runat="server" ID="OTHER" Width="170" MaxLength="20" Text='<%# Eval("OTHER").ToString().Trim() %>'></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:Repeater>
                <table cellpadding="0" cellspacing="0" style="margin: 0px">
                    <tr>
                        <td>
                            <asp:UpdatePanel runat="server" ID="upObjGrid" RenderMode="Inline" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="gridMain " style="width: 465px; margin: 0">
                                        <div style="padding: 0; margin: 0; position: relative; overflow-y: scroll; width: 455px;
                                            height: 130px" id="editGrid">
                                            <table cellpadding="0" cellspacing="0" style="width: 400px">
                                                <thead>
                                                    <tr>
                                                        <th>
                                                            編輯
                                                        </th>
                                                        <th>
                                                            標的物代號
                                                        </th>
                                                        <th>
                                                            標的物所在地址
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <asp:Repeater runat="server" ID="rptObjGrid">
                                                    <ItemTemplate>
                                                        <tr id='trObj<%#Container.ItemIndex+1%>' class="<%#Container.ItemIndex%2==0?"srow":"" %>">
                                                            <td>
                                                                <asp:Button ID="btnDel_Object" runat="server" CssClass="button del" Text="刪除" OnClientClick="return window.confirm('是否確定刪除此筆資料?');"
                                                                    OnCommand="GridFunc_Click" CommandName='<%# Eval("OBJ_KEY").ToString().Trim() %>' />
                                                                <asp:Button ID="btnUpd_Object" runat="server" CssClass="button upd" Text="修改" OnCommand="GridFunc_Click"
                                                                    CommandName='<%# Eval("OBJ_KEY").ToString().Trim() %>' />
                                                            </td>
                                                            <td>
                                                                <%# Eval("OBJ_CODE")%>
                                                            </td>
                                                            <td>
                                                                <%# Eval("OBJ_LOC_ADDR")%>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                                <tr id='trObj0'>
                                                    <td colspan="3">
                                                        <asp:Button ID="btnAdd_Object" runat="server" CssClass="button dadd" Text="新增" OnCommand="GridFunc_Click"
                                                            CommandName="" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:UpdatePanel runat="server" ID="upRETK" RenderMode="Inline" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <span style="display: none">
                                        <asp:Button ID="btnRETK" runat="server" Text="新增" OnCommand="GridFunc_Click" CommandName="RETK" />
                                    </span>
                                    <div class="gridMain " style="width: 295px; margin: 0px; margin-left: 5px">
                                        <div style="padding: 0; margin: 0; position: relative; overflow-y: scroll; width: 290px;
                                            height: 130px" runat="server">
                                            <table cellpadding="0" cellspacing="0" style="width: 280px">
                                                <thead>
                                                    <tr>
                                                        <th>
                                                            取回期間-起(月數)
                                                        </th>
                                                        <th>
                                                            取回期間-迄(月數)
                                                        </th>
                                                        <th>
                                                            買回比率(%)
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <asp:Repeater runat="server" ID="rptRETK">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td align="right">
                                                                <%# Eval("RETK_DURN_FR", "{0:###,###,###,##0}")%>
                                                            </td>
                                                            <td align="right">
                                                                <%# Eval("RETK_DURN_TO", "{0:###,###,###,##0}")%>
                                                            </td>
                                                            <td align="right">
                                                                <%# Eval("DUE_BUY_RATE", "{0:###,###,###,##0}")%>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </table>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:UpdatePanel runat="server" ID="upObjDetail" RenderMode="Inline" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Repeater runat="server" ID="rptObjDetail">
                                        <ItemTemplate>
                                            <table>
                                                <tr>
                                                    <th>
                                                        舊案申請書：
                                                    </th>
                                                    <td colspan="2">
                                                        <asp:TextBox runat="server" ID="OLD_APLY_NO" CssClass="display" ReadOnly="true" Width="70"
                                                            MaxLength="20" Text='<%# Eval("OLD_APLY_NO").ToString().Trim() %>'></asp:TextBox>
                                                    </td>
                                                    <td colspan="4">
                                                        實際承租人：<asp:TextBox runat="server" ID="Actual_lessee" CssClass="display" ReadOnly="true"
                                                            Width="60" MaxLength="20" Text='<%# Eval("Actual_lessee").ToString().Trim() %>'></asp:TextBox>
                                                        與供應商同
                                                        <asp:RadioButtonList ID="Is_spec_repo" runat="server" OnPreRender='checkList' ToolTip='<%# Eval("Is_spec_repo").ToString().Trim() %>'
                                                            RepeatLayout="Flow" RepeatDirection="Horizontal">
                                                            <asp:ListItem Value="Y">是</asp:ListItem>
                                                            <asp:ListItem Value="N">否</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th class="nonSpace">
                                                        標的物代號：
                                                    </th>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="OBJ_CODE" Width="90" MaxLength="20" Text='<%# Eval("OBJ_CODE").ToString().Trim() %>'></asp:TextBox>
                                                        <span style="display: none">
                                                            <asp:TextBox runat="server" ID="OBJ_KEY" Width="120" MaxLength="20" Text='<%# Eval("OBJ_KEY").ToString().Trim() %>'></asp:TextBox>
                                                        </span>
                                                    </td>
                                                    </td>
                                                    <th class="nonSpace">
                                                        品名：
                                                    </th>
                                                    <td colspan="2">
                                                        <asp:TextBox runat="server" ID="PROD_NAME" Width="150" MaxLength="200" Text='<%# Eval("PROD_NAME").ToString().Trim() %>'></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" OnPreRender='checkList' ID="OTC" ToolTip='<%# Eval("OTC").ToString().Trim() %>' />OTC資產
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        標的物所在地：
                                                    </th>
                                                    <td colspan="5">
                                                        <asp:TextBox runat="server" ID="OBJ_LOC_ADDR" Width="350" MaxLength="100" Text='<%# Eval("OBJ_LOC_ADDR").ToString().Trim() %>'></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        規格：
                                                    </th>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="SPEC" Width="90" MaxLength="20" Text='<%# Eval("SPEC").ToString().Trim() %>'></asp:TextBox>
                                                    </td>
                                                    <th>
                                                        廠牌：
                                                    </th>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="BRAND" Width="70" MaxLength="20" Text='<%# Eval("BRAND").ToString().Trim() %>'></asp:TextBox>
                                                    </td>
                                                    <th>
                                                        期滿所有權：
                                                    </th>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="OBJ_DUE_OWNER" Width="70" MaxLength="20" Text='<%# Eval("OBJ_DUE_OWNER").ToString().Trim() %>'></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        機號：
                                                    </th>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="MAC_NO" Width="90" MaxLength="20" Text='<%# Eval("MAC_NO").ToString().Trim() %>'></asp:TextBox>
                                                    </td>
                                                    <th>
                                                        年份：
                                                    </th>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="YEAR" Width="70" MaxLength="4" Text='<%# Eval("YEAR").ToString().Trim() %>'></asp:TextBox>
                                                    </td>
                                                    <th>
                                                        車號：
                                                    </th>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="CAR_NO" Width="70" MaxLength="20" Text='<%# Eval("CAR_NO").ToString().Trim() %>'></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th class="nonSpace">
                                                        市價：
                                                    </th>
                                                    <td>
                                                        <ocxControl:ocxNumber Minus="true" runat="server" ID="REAL_BUY_PRC" MASK="9,999,999"
                                                            Text='<%# Eval("REAL_BUY_PRC").ToString().Trim() %>' />
                                                    </td>
                                                    <th>
                                                        分期/租賃：
                                                    </th>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="BUDG_LEASE" OnPreRender='checkList' ToolTip='<%# Eval("BUDG_LEASE").ToString().Trim() %>'
                                                            Width="50">
                                                            <asp:ListItem Value="S">分期</asp:ListItem>
                                                            <asp:ListItem Value="L">租賃</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <th>
                                                        金額：
                                                    </th>
                                                    <td>
                                                        <ocxControl:ocxNumber Minus="true" runat="server" ID="BUDG_LEASE_AMT" MASK="9,999,999"
                                                            Text='<%# Eval("BUDG_LEASE_AMT").ToString().Trim() %>' />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        進項發票金額：
                                                    </th>
                                                    <td>
                                                        <ocxControl:ocxNumber Minus="true" runat="server" ID="INV_AMT_I_IB" MASK="9,999,999"
                                                            Text='<%# Eval("INV_AMT_I_IB").ToString().Trim() %>' />
                                                    </td>
                                                    <th>
                                                        原殘值：
                                                    </th>
                                                    <td>
                                                        <ocxControl:ocxNumber Minus="true" runat="server" ID="RV_AMT" MASK="9,999,999" Text='<%# Eval("RV_AMT").ToString().Trim() %>' />
                                                    </td>
                                                    <th>
                                                        自備率：
                                                    </th>
                                                    <td>
                                                        <ocxControl:ocxNumber Minus="true" runat="server" ID="SELF_RATE" MASK="99.9999" Text='<%# Eval("SELF_RATE").ToString().Trim() %>' />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        電話一：
                                                    </th>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="OBJ_LOC_TEL" size="11" MaxLength="20" Text='<%# Eval("OBJ_LOC_TEL").ToString().Trim() %>'></asp:TextBox>
                                                    </td>
                                                    <th>
                                                        傳真：
                                                    </th>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="OBJ_LOC_FAX" size="12" MaxLength="20" Text='<%# Eval("OBJ_LOC_FAX").ToString().Trim() %>'></asp:TextBox>
                                                    </td>
                                                    <th>
                                                        聯絡人：
                                                    </th>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="OBJ_LOC_CTAC" size="10" MaxLength="12" Text='<%# Eval("OBJ_LOC_CTAC").ToString().Trim() %>'></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th class="nonSpace">
                                                        附買回方式：
                                                    </th>
                                                    <td>
                                                        <asp:DropDownList ID="BUY_WAY" OnPreRender='checkList' ToolTip='<%# Eval("BUY_WAY").ToString().Trim() %>'
                                                            runat="server" Width="80">
                                                            <asp:ListItem Value=""></asp:ListItem>
                                                            <asp:ListItem Value="1">不附買回</asp:ListItem>
                                                            <asp:ListItem Value="2">購買額</asp:ListItem>
                                                            <asp:ListItem Value="3">契約餘額</asp:ListItem>
                                                            <asp:ListItem Value="4">本金餘額</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <th>
                                                        買回承諾人：
                                                    </th>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="BUY_PROMISE" size="10" MaxLength="12" Text='<%# Eval("BUY_PROMISE").ToString().Trim() %>'></asp:TextBox>
                                                    </td>
                                                    <th class="nonSpace">
                                                        買回比率：
                                                    </th>
                                                    <td>
                                                        <ocxControl:ocxNumber Minus="true" runat="server" ID="BUY_RATE" MASK="999.9999" Text='<%# Eval("BUY_RATE").ToString().Trim() %>' />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th class="nonSpace">
                                                        供應商：
                                                    </th>
                                                    <td>
                                                        <ocxControl:ocxDialog runat="server" ID="FRC_CODE" width="70" Text='<%# Eval("FRC_CODE").ToString().Trim() %>'
                                                            SourceName="OR_FRC" ControlID="FRC_SNAME" FieldName="FRC_SNAME" />
                                                    </td>
                                                    <td colspan="2">
                                                        <input type="text" class="display" style="width: 100px" name="FRC_SNAME" value='<%# Eval("FRC_SNAME").ToString().Trim() %>'
                                                            id="FRC_SNAME" />
                                                    </td>
                                                    <th>
                                                        營業單位：
                                                    </th>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="SALES_UNIT" size="11" MaxLength="20" Text='<%# Eval("SALES_UNIT").ToString().Trim() %>'></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        險種別：
                                                    </th>
                                                    <td colspan="3">
                                                        <asp:DropDownList ID="OBJ_ASUR_TYPE" Style="width: 200px" runat="server" OnPreRender='checkList'
                                                            ToolTip='<%# Eval("OBJ_ASUR_TYPE").ToString().Trim() %>' DataSourceID="sqlASUR_TYPE_CODE"
                                                            DataValueField="TypeCode" DataTextField="TypeDesc">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <th>
                                                        保險負擔者：
                                                    </th>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="OBJ_ONUS" OnPreRender='checkList' ToolTip='<%# Eval("OBJ_ONUS").ToString().Trim() %>'
                                                            Width="87" DataSourceID="sqlONUS" DataValueField="TypeCode" DataTextField="TypeDesc">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:UpdatePanel runat="server" ID="upAccs" RenderMode="Inline" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="gridMain " style="width: 235px">
                                        <div style="padding: 0; margin: 0; position: relative; overflow-y: scroll; width: 215px;
                                            height: 230px" id="editGrid">
                                            <table cellpadding="0" cellspacing="0" style="width: 160px">
                                                <thead>
                                                    <tr>
                                                        <th>
                                                            編輯
                                                        </th>
                                                        <th>
                                                            配件名稱
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <asp:Repeater runat="server" ID="rptAccs">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnDel_Accs" runat="server" CssClass="button del" Text="刪除" OnCommand="GridFunc_Click"
                                                                    CommandName='<%# Eval("ACCS_SEQ").ToString().Trim() %>' />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="ACCS_NAME" size="20" Width="150" Text='<%# Eval("ACCS_NAME") %>'></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnAdd_Accs" runat="server" CssClass="button dadd" Text="新增" OnCommand="GridFunc_Click"
                                                            CommandName="Add" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="addACCS_NAME" size="20" Width="150" Text='<%# Eval("ACCS_NAME") %>'></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                    <div class="divFunction80" style='float: right;'>
                                        <asp:Button runat="server" ID="btnEdit" CssClass="button trn80" Text="明細儲存" Font-Bold="true"
                                            OnCommand="GridFunc_Click" CommandName="Save" />
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="panel" style="height: auto; width: 820px; display: none" id="tabpanel3">
                <asp:Repeater runat="server" ID="rptRequest">
                    <ItemTemplate>
                        <table cellspacing="1" cellpadding="1" border="0">
                            <tr>
                                <th>
                                    付款條件：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="PAY_COND_DAY" MASK="99,999"
                                        Text='<%# Eval("PAY_COND_DAY").ToString().Trim() %>' />
                                    天
                                </td>
                                <th colspan="2">
                                    付款供應商天數：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="PAY_DAY" MASK="99,999" Text='<%# Eval("PAY_DAY").ToString().Trim() %>' />
                                </td>
                            </tr>
                            <tr>
                                <th style="width: 100px" class="nonSpace">
                                    購買額：
                                </th>
                                <td style="width: 140px">
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="APLY_BUY_AMT" MASK=",999,999"
                                        Text='<%# Eval("APLY_BUY_AMT").ToString().Trim() %>' />
                                </td>
                                <th style="width: 100px">
                                    佣金：
                                </th>
                                <td style="width: 70px">
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="APLY_RAKE" MASK=",999,999"
                                        Text='<%# Eval("APLY_RAKE").ToString().Trim() %>' />
                                </td>
                                <th>
                                    其他費用：
                                </th>
                                <td style="width: 70px">
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="APLY_ANT_EXP" MASK=",999,999"
                                        Text='<%# Eval("APLY_ANT_EXP").ToString().Trim() %>' />
                                </td>
                                <th>
                                    支出合計：
                                </th>
                                <td style="width: 70px">
                                    <ocxControl:ocxNumber Minus="true" runat="server" bolEnabled="false" ID="TOTAL" MASK=",999,999"
                                        Text='<%# Eval("TOTAL").ToString().Trim() %>' />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    保證金(含稅)：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="APLY_BOND" MASK=",999,999"
                                        Text='<%# Eval("APLY_BOND").ToString().Trim() %>' />
                                </td>
                                <th>
                                    殘值：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="APLY_REST" MASK=",999,999"
                                        Text='<%# Eval("APLY_REST").ToString().Trim() %>' />
                                </td>
                                <th>
                                    進項稅額：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="APLY_PURS_TAX" MASK=",999,999"
                                        Text='<%# Eval("APLY_PURS_TAX").ToString().Trim() %>' />
                                </td>
                                <th class="nonSpace">
                                    付款週期：
                                </th>
                                <td>
                                    <asp:DropDownList runat="server" ID="APLY_PAY_PERD" OnPreRender='checkList' ToolTip='<%# Eval("APLY_PAY_PERD").ToString().Trim() %>'
                                        Width="80" DataSourceID="sqlPAY_PERD" DataValueField="TypeCode" DataTextField="TypeDesc">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th class="nonSpace">
                                    期間：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="APLY_DURN_M" MASK="999" Text='<%# Eval("APLY_DURN_M").ToString().Trim() %>' />
                                    個月
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="APLY_PERD" MASK="999" Text='<%# Eval("APLY_PERD").ToString().Trim() %>' />
                                    期
                                </td>
                                <th>
                                    保險因子(%)：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="ISU_FACTOR" MASK="99,999" Text='<%# Eval("ISU_FACTOR").ToString().Trim() %>' />
                                </td>
                                <th>
                                    保留款：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="APLY_SAVING_EXP" MASK=",999,999"
                                        Text='<%# Eval("APLY_SAVING_EXP").ToString().Trim() %>' />
                                </td>
                                <th>
                                    實值TR：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber Minus="true" runat="server" bolEnabled="false" ID="APLY_REAL_TR"
                                        MASK=",999,999" Text='<%# Eval("APLY_REAL_TR").ToString().Trim() %>' />
                                </td>
                            </tr>
                            <tr>
                                <th class="nonSpace">
                                    繳款方式：
                                </th>
                                <td>
                                    <asp:DropDownList runat="server" ID="APLY_PAY_MTHD" OnPreRender='checkList' ToolTip='<%# Eval("APLY_PAY_MTHD").ToString().Trim() %>'
                                        Width="80" DataSourceID="sqlPAY_MTHD" DataValueField="TypeCode" DataTextField="TypeDesc">
                                    </asp:DropDownList>
                                </td>
                                <th>
                                    頭期款(未稅)：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="APLY_DEPS" MASK=",999,999"
                                        Text='<%# Eval("APLY_DEPS").ToString().Trim() %>' />
                                </td>
                                <th>
                                    保險費：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="ISU_AMT" MASK=",999,999" Text='<%# Eval("ISU_AMT").ToString().Trim() %>' />
                                </td>
                                <th>
                                    表面TR：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber Minus="true" runat="server" bolEnabled="false" ID="APLY_SURF_TR"
                                        MASK=",999,999" Text='<%# Eval("APLY_SURF_TR").ToString().Trim() %>' />
                                </td>
                            </tr>
                            <th class="nonSpace">
                                攤提方式：
                            </th>
                            <td>
                                <asp:DropDownList runat="server" onchange="changeMTHD(1,this)" ID="APLY_AMOR_MTHD"
                                    OnPreRender='checkList' ToolTip='<%# Eval("APLY_AMOR_MTHD").ToString().Trim() %>'
                                    Width="80" DataSourceID="sqlAMOR_MTHD" DataValueField="TypeCode" DataTextField="TypeDesc">
                                </asp:DropDownList>
                            </td>
                            <th>
                                手續費：
                            </th>
                            <td>
                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APLY_SERV_CHAR" MASK=",999,999"
                                    Text='<%# Eval("APLY_SERV_CHAR").ToString().Trim() %>' />
                            </td>
                            </tr>
                            <tr>
                                <th class="nonSpace">
                                    稅別：
                                </th>
                                <td>
                                    <asp:DropDownList runat="server" ID="APLY_TAX_ZERO" OnPreRender='checkList' ToolTip='<%# Eval("APLY_TAX_ZERO").ToString().Trim() %>'
                                        Width="80" DataSourceID="sqlTAX_TYPE" DataValueField="TypeCode" DataTextField="TypeDesc">
                                    </asp:DropDownList>
                                </td>
                                <th colspan="2">
                                    不計業績手續費：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="NON_FEAT_CHARGE" MASK=",999,999"
                                        Text='<%# Eval("NON_FEAT_CHARGE").ToString().Trim() %>' />
                                </td>
                                <th>
                                    其他利息：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber Minus="true" runat="server" bolEnabled="false" ID="APLY_OTH_INT"
                                        MASK=",999,999" Text='<%# Eval("APLY_OTH_INT").ToString().Trim() %>' />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <table cellpadding="0" cellspacing="0" style="margin: 0">
                                        <tr>
                                            <td>
                                                <fieldset id="MTHD1_1" style="padding: 0; margin: 0; display: none">
                                                    <legend>定額</legend>
                                                    <table style="margin: 0px" cellpadding="1" cellspacing="1">
                                                        <tr>
                                                            <th>
                                                                租金：
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APLY_HIRE" MASK="9,999,999"
                                                                    Text='<%# Eval("APLY_HIRE").ToString().Trim() %>' />
                                                            </td>
                                                            <th>
                                                                稅金：
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APLY_TAX" MASK="9,999,999"
                                                                    Text='<%# Eval("APLY_TAX").ToString().Trim() %>' />
                                                                <span style="display: none">
                                                                    <asp:TextBox runat="server" ID="hidden_APLY_TAX"></asp:TextBox>
                                                                </span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                                <br />
                                                <fieldset id="MTHD1_2" style="padding: 0; margin: 0; display: none">
                                                    <legend>階定額</legend>
                                                    <table style="margin: 0px; width: 300px" cellpadding="1" cellspacing="1">
                                                        <tr>
                                                            <th>
                                                                LF1：
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APLY_LF1_FR" MASK="99" bolEnabled="false"
                                                                    Text='<%# Eval("APLY_LF1_FR").ToString().Trim() %>' />
                                                                ~
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APLY_LF1_TO" MASK="99" Text='<%# Eval("APLY_LF1_TO").ToString().Trim() %>' />
                                                            </td>
                                                            <th>
                                                                租金：
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APLY_LF1_HIRE" MASK=",999,999"
                                                                    Text='<%# Eval("APLY_LF1_HIRE").ToString().Trim() %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th>
                                                                LF2：
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APLY_LF2_FR" MASK="99" bolEnabled="false"
                                                                    Text='<%# Eval("APLY_LF2_FR").ToString().Trim() %>' />
                                                                ~
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APLY_LF2_TO" MASK="99" Text='<%# Eval("APLY_LF2_TO").ToString().Trim() %>' />
                                                            </td>
                                                            <th>
                                                                租金：
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APLY_LF2_HIRE" MASK=",999,999"
                                                                    Text='<%# Eval("APLY_LF2_HIRE").ToString().Trim() %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th>
                                                                LF3：
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APLY_LF3_FR" MASK="99" bolEnabled="false"
                                                                    Text='<%# Eval("APLY_LF3_FR").ToString().Trim() %>' />
                                                                ~
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APLY_LF3_TO" MASK="99" Text='<%# Eval("APLY_LF3_TO").ToString().Trim() %>' />
                                                            </td>
                                                            <th>
                                                                租金：
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APLY_LF3_HIRE" MASK=",999,999"
                                                                    Text='<%# Eval("APLY_LF3_HIRE").ToString().Trim() %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th>
                                                                LF4：
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APLY_LF4_FR" MASK="99" bolEnabled="false"
                                                                    Text='<%# Eval("APLY_LF4_FR").ToString().Trim() %>' />
                                                                ~
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APLY_LF4_TO" MASK="99" Text='<%# Eval("APLY_LF4_TO").ToString().Trim() %>' />
                                                            </td>
                                                            <th>
                                                                租金：
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APLY_LF4_HIRE" MASK=",999,999"
                                                                    Text='<%# Eval("APLY_LF4_HIRE").ToString().Trim() %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th>
                                                                LF5：
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APLY_LF5_FR" MASK="99" bolEnabled="false"
                                                                    Text='<%# Eval("APLY_LF5_FR").ToString().Trim() %>' />
                                                                ~
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APLY_LF5_TO" MASK="99" Text='<%# Eval("APLY_LF5_TO").ToString().Trim() %>' />
                                                            </td>
                                                            <th>
                                                                租金：
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APLY_LF5_HIRE" MASK=",999,999"
                                                                    Text='<%# Eval("APLY_LF5_HIRE").ToString().Trim() %>' />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                                <br />
                                                <fieldset style="padding: 0; margin: 0">
                                                    <legend></legend>
                                                    <table style="margin: 0px; width: 300px" cellpadding="1" cellspacing="1">
                                                        <tr>
                                                            <th>
                                                                收入總額：
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APLY_INCM_TOL" bolEnabled="false"
                                                                    MASK=",999,999" Text='<%# Eval("APLY_INCM_TOL").ToString().Trim() %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th>
                                                                銷項稅額：
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APLY_SELL_TAX" bolEnabled="false"
                                                                    MASK=",999,999" Text='<%# Eval("APLY_SELL_TAX").ToString().Trim() %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th>
                                                                毛收益：
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APLY_MARG" bolEnabled="false"
                                                                    MASK=",999,999" Text='<%# Eval("APLY_MARG").ToString().Trim() %>' />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                                <br />
                                            </td>
                                            <td>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:UpdatePanel runat="server" ID="upSetR" UpdateMode="Conditional" RenderMode="Inline">
                    <ContentTemplate>
                        <div class="divFunction80" id="Set1">
                            <asp:Button runat="server" ID="btnSet_Request" CssClass="button trn80" Text="攤提試算"
                                OnCommand="GridFunc_Click" CommandName="Set_Request" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <asp:UpdatePanel runat="server" ID="upTRR" UpdateMode="Conditional" RenderMode="Inline">
                    <ContentTemplate>
                        <div class="divFunction80" id="TR1">
                            <asp:Button runat="server" ID="btnTR_Request" CssClass="button trn80" Text="TR計算"
                                OnCommand="GridFunc_Click" CommandName="TR_Request" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                </td></tr></table> </td>
                <td colspan="4">
                    <asp:UpdatePanel runat="server" ID="upShare" UpdateMode="Conditional" RenderMode="Inline">
                        <ContentTemplate>
                            <div class="tabs-header" style="width: 378px;">
                                <div class="tabs-scroller-left" style="display: none;">
                                </div>
                                <div class="tabs-scroller-right" style="display: none;">
                                </div>
                                <div class="tabs-wrap" style="margin-left: 0px; margin-right: 0px; width: 370px;">
                                    <ul class="tabs">
                                        <li class="tabs-selected" id="tabD1" onclick="tabD_select(1,3)"><a href="javascript:void(0)"
                                            class="tabs-inner"><span class="tabs-title">表面TR</span><span class="tabs-icon"></span></a></li>
                                        <li class="" id="tabD2" onclick="tabD_select(2,3)"><a href="javascript:void(0)" class="tabs-inner">
                                            <span class="tabs-title">實質TR</span><span class="tabs-icon"></span></a></li>
                                        <li class="" id="tabD3" onclick="tabD_select(3,3)"><a href="javascript:void(0)" class="tabs-inner">
                                            <span class="tabs-title">表面+其他費用</span><span class="tabs-icon"></span></a></li>
                                    </ul>
                                </div>
                            </div>
                            <div class="tabs-panels" style="width: 378px;">
                                <div class="panel" style="height: auto; width: 370px; display: block" id="tabpanelD1">
                                    <div class="gridMain " style="width: 355px">
                                        <div style="padding: 0; margin: 0; position: relative; overflow-y: scroll; width: 355px;
                                            height: 180px" id="editGrid">
                                            <table cellpadding="0" cellspacing="0" style="width: 180px">
                                                <thead>
                                                    <tr>
                                                        <th>
                                                            期數
                                                        </th>
                                                        <th>
                                                            租金
                                                        </th>
                                                        <th>
                                                            利息
                                                        </th>
                                                        <th>
                                                            本金攤還
                                                        </th>
                                                        <th>
                                                            本金餘額
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <asp:Repeater runat="server" ID="rptShare">
                                                    <ItemTemplate>
                                                        <tr class="<%#Container.ItemIndex%2==0?"srow":"" %>">
                                                            <td class="number">
                                                                <%# (Eval("PERIOD").ToString().Trim()=="0"?"頭款":Eval("PERIOD","{0:###,###,###,##0}")) %>
                                                            </td>
                                                            <td class="number" style='display: <%# Eval("PERIOD").ToString().Trim()!="0" && this.APLY_MTHD=="5"?"none":"" %>'>
                                                                <%# Eval("HIRE", "{0:###,###,###,##0}")%>
                                                            </td>
                                                            <td class="number" style='display: <%# Eval("PERIOD").ToString().Trim()!="0" && this.APLY_MTHD=="5"?"":"none" %>'>
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="HIRE" MASK="999,999" Text='<%# Eval("HIRE").ToString().Trim() %>' />
                                                            </td>
                                                            <td class="number">
                                                                <%# Eval("DIVD","{0:###,###,###,##0}") %>
                                                            </td>
                                                            <td class="number">
                                                                <%# Eval("CAPT_AMOR","{0:###,###,###,##0}") %>
                                                            </td>
                                                            <td class="number">
                                                                <%# Eval("REST","{0:###,###,###,##0}") %>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </table>
                                        </div>
                                    </div>
                                    <table>
                                        <tr>
                                            <td>
                                                合計：
                                            </td>
                                            <td>
                                                <div style="width: 120px" class="text number" id="rptShare_HIRE">
                                                </div>
                                            </td>
                                            <td>
                                                <div style="width: 120px" class="text number" id="rptShare_DIVD">
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="panel" style="height: auto; width: 370px; display: none" id="tabpanelD2">
                                    <div class="gridMain " style="width: 355px">
                                        <div style="padding: 0; margin: 0; position: relative; overflow-y: scroll; width: 355px;
                                            height: 180px" id="editGrid">
                                            <table cellpadding="0" cellspacing="0" style="width: 180px">
                                                <thead>
                                                    <tr>
                                                        <th>
                                                            期數
                                                        </th>
                                                        <th>
                                                            租金
                                                        </th>
                                                        <th>
                                                            利息
                                                        </th>
                                                        <th>
                                                            本金攤還
                                                        </th>
                                                        <th>
                                                            本金餘額
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <asp:Repeater runat="server" ID="rptShareReal">
                                                    <ItemTemplate>
                                                        <tr class="<%#Container.ItemIndex%2==0?"srow":"" %>">
                                                            <td class="number">
                                                                <%# (Eval("PERIOD").ToString().Trim()=="0"?"頭款":Eval("PERIOD","{0:###,###,###,##0}")) %>
                                                            </td>
                                                            <td class="number">
                                                                <%# Eval("HIRE", "{0:###,###,###,##0}")%>
                                                            </td>
                                                            <td class="number">
                                                                <%# Eval("DIVD", "{0:###,###,###,##0}")%>
                                                            </td>
                                                            <td class="number">
                                                                <%# Eval("CAPT_AMOR", "{0:###,###,###,##0}")%>
                                                            </td>
                                                            <td class="number">
                                                                <%# Eval("REST", "{0:###,###,###,##0}")%>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </table>
                                        </div>
                                    </div>
                                    <table>
                                        <tr>
                                            <td>
                                                合計：
                                            </td>
                                            <td>
                                                <div style="width: 120px" class="text number" id="rptShareReal_HIRE">
                                                </div>
                                            </td>
                                            <td>
                                                <div style="width: 120px" class="text number" id="rptShareReal_DIVD">
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="panel" style="height: auto; width: 370px; display: none" id="tabpanelD3">
                                    <div class="gridMain " style="width: 355px">
                                        <div style="padding: 0; margin: 0; position: relative; overflow-y: scroll; width: 355px;
                                            height: 180px" id="editGrid">
                                            <table cellpadding="0" cellspacing="0" style="width: 180px">
                                                <thead>
                                                    <tr>
                                                        <th>
                                                            期數
                                                        </th>
                                                        <th>
                                                            租金
                                                        </th>
                                                        <th>
                                                            利息
                                                        </th>
                                                        <th>
                                                            本金攤還
                                                        </th>
                                                        <th>
                                                            本金餘額
                                                        </th>
                                                        <th>
                                                            扣表面本餘
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <asp:Repeater runat="server" ID="rptShareEx">
                                                    <ItemTemplate>
                                                        <tr class="<%#Container.ItemIndex%2==0?"srow":"" %>">
                                                            <td class="number">
                                                                <%# (Eval("PERIOD").ToString().Trim()=="0"?"頭款":Eval("PERIOD","{0:###,###,###,##0}")) %>
                                                            </td>
                                                            <td class="number">
                                                                <%# Eval("HIRE", "{0:###,###,###,##0}")%>
                                                            </td>
                                                            <td class="number">
                                                                <%# Eval("DIVD", "{0:###,###,###,##0}")%>
                                                            </td>
                                                            <td class="number">
                                                                <%# Eval("CAPT_AMOR", "{0:###,###,###,##0}")%>
                                                            </td>
                                                            <td class="number">
                                                                <%# Eval("REST", "{0:###,###,###,##0}")%>
                                                            </td>
                                                            <td class="number">
                                                                <%# Eval("TOTAL", "{0:###,###,###,##0}")%>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </table>
                                        </div>
                                    </div>
                                    <table>
                                        <tr>
                                            <td>
                                                合計：
                                            </td>
                                            <td>
                                                <div style="width: 120px" class="text number">
                                                </div>
                                            </td>
                                            <td>
                                                <div style="width: 120px" class="text number">
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                </tr> </table>
            </div>
            <div class="panel" style="height: auto; width: 820px; display: none" id="tabpanel4">
                <table>
                    <tr>
                        <td>
                            <fieldset>
                                <legend>保證人</legend>
                                <div class="gridMain " style="width: 425px; margin: 0">
                                    <div style="padding: 0; margin: 0; position: relative; overflow-y: scroll; width: 415px;
                                        height: 100px" id="editGrid">
                                        <table cellpadding="0" cellspacing="0" style="width: 160px">
                                            <thead>
                                                <tr>
                                                    <th>
                                                        類別
                                                    </th>
                                                    <th>
                                                        身份/統編
                                                    </th>
                                                    <th>
                                                        名稱
                                                    </th>
                                                    <th>
                                                        與申戶關係
                                                    </th>
                                                </tr>
                                            </thead>
                                            <asp:Repeater runat="server" ID="rptScur">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <asp:DropDownList ID="SCUR_NATUR" Style="width: 70px" runat="server" OnPreRender='checkList'
                                                                ToolTip='<%# Eval("SCUR_NATUR").ToString().Trim() %>' DataSourceID="sqlSCUR_NATUR"
                                                                DataValueField="TypeCode" DataTextField="TypeDesc">
                                                            </asp:DropDownList>
                                                        </td>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="SCUR_ID" MaxLength="20" Width="90" Text='<%# Eval("SCUR_ID").ToString().Trim() %>'></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="SCUR_NAME" MaxLength="20" Width="90" Text='<%# Eval("SCUR_NAME").ToString().Trim() %>'></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="SCUR_RELATION" Width="80" runat="server" OnPreRender='checkList'
                                                                ToolTip='<%# Eval("SCUR_RELATION").ToString().Trim() %>' DataSourceID="sqlSCUR_RELATION"
                                                                DataValueField="TypeCode" DataTextField="TypeDesc">
                                                            </asp:DropDownList>
                                                        </td>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </table>
                                    </div>
                                </div>
                            </fieldset>
                        </td>
                        <td>
                            <fieldset>
                                <legend>保險</legend>
                                <div class="gridMain " style="width: 325px; margin: 0">
                                    <div style="padding: 0; margin: 0; overflow-y: scroll; position: relative; width: 310px;
                                        height: 100px" id="editGrid">
                                        <table cellpadding="0" cellspacing="0" style="width: 160px">
                                            <thead>
                                                <tr>
                                                    <th>
                                                        種類
                                                    </th>
                                                    <th>
                                                        負擔者
                                                    </th>
                                                    <th>
                                                        金額
                                                    </th>
                                                </tr>
                                            </thead>
                                            <asp:Repeater runat="server" ID="rptASUR">
                                                <ItemTemplate>
                                                    <tr class="<%#Container.ItemIndex%2==0?"srow":"" %>">
                                                        <td>
                                                            <span style="display: none">
                                                                <asp:DropDownList ID="ASUR_PAYER" Style="width: 200px" runat="server" DataSourceID="sqlASUR_TYPE_CODE"
                                                                    DataValueField="TypeCode" DataTextField="ASUR_PAYER">
                                                                </asp:DropDownList>
                                                            </span>
                                                            <asp:DropDownList ID="ASUR_TYPE_CODE" Style="width: 120px" runat="server" OnPreRender='checkList'
                                                                ToolTip='<%# Eval("ASUR_TYPE_CODE").ToString().Trim() %>' DataSourceID="sqlASUR_TYPE_CODE"
                                                                DataValueField="TypeCode" DataTextField="TypeDesc">
                                                            </asp:DropDownList>
                                                        </td>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ONUS" Width="80" runat="server" OnPreRender='checkList' ToolTip='<%# Eval("ONUS").ToString().Trim() %>'
                                                                DataSourceID="sqlONUS" DataValueField="TypeCode" DataTextField="TypeDesc">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <ocxControl:ocxNumber Minus="true" runat="server" ID="AMOUNT" MASK="999,999" Text='<%# Eval("AMOUNT").ToString().Trim() %>' />
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </table>
                                    </div>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <fieldset>
                                <legend>動產擔保品-不含主標的物</legend>
                                <div class="gridMain " style="width: 775px; margin: 0;">
                                    <div style="padding: 0; margin: 0; overflow-y: scroll; overflow-x: hidden; position: relative;
                                        width: 765px; height: 20px;" id="editGrid2" class="hiddenScroll">
                                        <table cellpadding="0" cellspacing="0" style="width: 760px">
                                            <tr>
                                                <th>
                                                    品名
                                                </th>
                                                <th>
                                                    規格
                                                </th>
                                                <th>
                                                    廠牌
                                                </th>
                                                <th>
                                                    機號
                                                </th>
                                                <th>
                                                    車號
                                                </th>
                                                <th>
                                                    年份
                                                </th>
                                                <th>
                                                    目前市價
                                                </th>
                                                <th>
                                                    保險種類
                                                </th>
                                                <th>
                                                    負擔者
                                                </th>
                                                <th>
                                                    保險金額
                                                </th>
                                                <th>
                                                    備註
                                                </th>
                                                <th>
                                                    終止日
                                                </th>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="PROD_NAME" MaxLength="20" Width="100"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="SPEC" MaxLength="20" Width="100"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="BRAND" MaxLength="20" Width="100"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="MAC_NO" MaxLength="20" Width="100"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="CAR_NO" MaxLength="20" Width="100"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <ocxControl:ocxYear runat="server" ID="YEAR" />
                                                </td>
                                                <td>
                                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="RV_AMT" MASK="9,999,999" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ASUR_TYPE_CODE" Style="width: 120px" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ASUR_PAYER" Width="60" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="ASUR_AMOUNT" MASK="999,999" />
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="Memos" MaxLength="20" Width="100"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <ocxControl:ocxDate runat="server" ID="End_Date" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div style="padding: 0; margin: 0; overflow-y: scroll; width: 765px; height: 100px;"
                                        id="editGrid1" onscroll="document.getElementById('editGrid2').scrollLeft=this.scrollLeft;">
                                        <table>
                                            <asp:Repeater runat="server" ID="rptObject">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="PROD_NAME" MaxLength="20" Width="100" Text='<%# Eval("PROD_NAME").ToString().Trim() %>'></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="SPEC" MaxLength="20" Width="100" Text='<%# Eval("SPEC").ToString().Trim() %>'></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="BRAND" MaxLength="20" Width="100" Text='<%# Eval("BRAND").ToString().Trim() %>'></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="MAC_NO" MaxLength="20" Width="100" Text='<%# Eval("MAC_NO").ToString().Trim() %>'></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="CAR_NO" MaxLength="20" Width="100" Text='<%# Eval("CAR_NO").ToString().Trim() %>'></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <ocxControl:ocxYear runat="server" ID="YEAR" Text='<%# Eval("YEAR").ToString().Trim() %>' />
                                                        </td>
                                                        <td>
                                                            <ocxControl:ocxNumber Minus="true" runat="server" ID="RV_AMT" MASK="9,999,999" Text='<%# Eval("RV_AMT").ToString().Trim() %>' />
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ASUR_TYPE_CODE" Style="width: 120px" runat="server" OnPreRender='checkList'
                                                                ToolTip='<%# Eval("ASUR_TYPE_CODE").ToString().Trim() %>' DataSourceID="sqlASUR_TYPE_CODE"
                                                                DataValueField="TypeCode" DataTextField="TypeDesc">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ASUR_PAYER" Width="60" runat="server" OnPreRender='checkList'
                                                                ToolTip='<%# Eval("ASUR_PAYER").ToString().Trim() %>' DataSourceID="sqlONUS"
                                                                DataValueField="TypeCode" DataTextField="TypeDesc">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <ocxControl:ocxNumber Minus="true" runat="server" ID="ASUR_AMOUNT" MASK="999,999"
                                                                Text='<%# Eval("ASUR_AMOUNT").ToString().Trim() %>' />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="Memos" MaxLength="20" Width="100" Text='<%# Eval("Memos").ToString().Trim() %>'></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <ocxControl:ocxDate runat="server" ID="End_Date" Text='<%# Eval("End_Date").ToString().Trim() %>' />
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </table>
                                    </div>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <fieldset>
                                <legend>不動產擔保品</legend>
                                <div class="gridMain " style="width: 775px; margin: 0">
                                    <div style="padding: 0; margin: 0; overflow-y: scroll; position: relative; width: 765px;
                                        height: 100px" id="editGrid">
                                        <table cellpadding="0" cellspacing="0" style="width: 760px">
                                            <thead>
                                                <tr>
                                                    <th>
                                                        土地地段(地號)
                                                    </th>
                                                    <th>
                                                        建物門牌(建號)
                                                    </th>
                                                    <th>
                                                        土地面積
                                                    </th>
                                                    <th>
                                                        建物面積
                                                    </th>
                                                    <th>
                                                        鑑定時價
                                                    </th>
                                                    <th>
                                                        擔保餘值
                                                    </th>
                                                    <th>
                                                        1本案2前案
                                                    </th>
                                                    <th>
                                                        鑑價依據
                                                    </th>
                                                    <th>
                                                        OTC順位
                                                    </th>
                                                    <th>
                                                        OTC設定金額
                                                    </th>
                                                    <th>
                                                        一順位
                                                    </th>
                                                    <th>
                                                        一順位金額
                                                    </th>
                                                    <th>
                                                        二順位
                                                    </th>
                                                    <th>
                                                        二順位金額
                                                    </th>
                                                    <th>
                                                        三順位
                                                    </th>
                                                    <th>
                                                        三順位金額
                                                    </th>
                                                </tr>
                                            </thead>
                                            <asp:Repeater runat="server" ID="rptIMP">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="RLTY_DESC" MaxLength="20" Width="120" Text='<%# Eval("RLTY_DESC").ToString().Trim() %>'></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="BUILDING_NO" MaxLength="20" Width="120" Text='<%# Eval("BUILDING_NO").ToString().Trim() %>'></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="LAND_AREA" MaxLength="20" Width="100" Text='<%# Eval("LAND_AREA").ToString().Trim() %>'></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="BUILDING_AREA" MaxLength="20" Width="100" Text='<%# Eval("BUILDING_AREA").ToString().Trim() %>'></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="DECD_PRC" MaxLength="20" Width="100" Text='<%# Eval("DECD_PRC").ToString().Trim() %>'></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="COLL_SUR" MaxLength="20" Width="100" Text='<%# Eval("COLL_SUR").ToString().Trim() %>'></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="CASE_FLAG" MaxLength="20" Width="100" Text='<%# Eval("CASE_FLAG").ToString().Trim() %>'></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="PRICE_BASE" MaxLength="20" Width="100" Text='<%# Eval("PRICE_BASE").ToString().Trim() %>'></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="ORDER" MaxLength="20" Width="100" Text='<%# Eval("ORDER").ToString().Trim() %>'></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <ocxControl:ocxNumber Minus="true" runat="server" ID="SET_AMT" MASK="99,999" Text='<%# Eval("SET_AMT").ToString().Trim() %>' />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="ORDER1" MaxLength="20" Width="100" Text='<%# Eval("ORDER1").ToString().Trim() %>'></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <ocxControl:ocxNumber Minus="true" runat="server" ID="ORDER1_AMT" MASK="99,999" Text='<%# Eval("ORDER1_AMT").ToString().Trim() %>' />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="ORDER2" MaxLength="20" Width="100" Text='<%# Eval("ORDER2").ToString().Trim() %>'></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <ocxControl:ocxNumber Minus="true" runat="server" ID="ORDER2_AMT" MASK="99,999" Text='<%# Eval("ORDER2_AMT").ToString().Trim() %>' />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="ORDER3" MaxLength="20" Width="100" Text='<%# Eval("ORDER3").ToString().Trim() %>'></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <ocxControl:ocxNumber Minus="true" runat="server" ID="ORDER3_AMT" MASK="99,999" Text='<%# Eval("ORDER3_AMT").ToString().Trim() %>' />
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </table>
                                    </div>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <th>
                            其他條件：
                        </th>
                        <td>
                            <asp:TextBox runat="server" ID="OTHER_CONDITION" MaxLength="20" TextMode="MultiLine"
                                Rows="2" Width="270"></asp:TextBox>
                        </td>
                        <th>
                            資金用途：
                        </th>
                        <td>
                            <asp:TextBox runat="server" ID="FINANCIAL_PURPOSE" MaxLength="20" TextMode="MultiLine"
                                Rows="2" Width="270"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            預計動撥日：
                        </th>
                        <td>
                            <ocxControl:ocxDate runat="server" ID="EXPECT_AR_DATE" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="panel" style="height: auto; width: 820px; display: none" id="tabpanel5">
                <asp:Repeater runat="server" ID="rptRun">
                    <ItemTemplate>
                        <table cellspacing="1" cellpadding="1" border="0">
                            <tr>
                                <th>
                                    付款條件：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="PAY_COND_DAY" MASK="99,999"
                                        Text='<%# Eval("PAY_COND_DAY").ToString().Trim() %>' />
                                    天
                                </td>
                                <th colspan="2">
                                    付款供應商天數：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="PAY_DAY" MASK="99,999" Text='<%# Eval("PAY_DAY").ToString().Trim() %>' />
                                </td>
                            </tr>
                            <tr>
                                <th style="width: 100px" class="nonSpace">
                                    購買額：
                                </th>
                                <td style="width: 140px">
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="APRV_BUY_AMT" MASK=",999,999"
                                        Text='<%# Eval("APRV_BUY_AMT").ToString().Trim() %>' />
                                </td>
                                <th style="width: 100px">
                                    佣金：
                                </th>
                                <td style="width: 70px">
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="APRV_RAKE" MASK=",999,999"
                                        Text='<%# Eval("APRV_RAKE").ToString().Trim() %>' />
                                </td>
                                <th>
                                    其他費用：
                                </th>
                                <td style="width: 70px">
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="APRV_ANT_EXP" MASK=",999,999"
                                        Text='<%# Eval("APRV_ANT_EXP").ToString().Trim() %>' />
                                </td>
                                <th>
                                    支出合計：
                                </th>
                                <td style="width: 70px">
                                    <ocxControl:ocxNumber Minus="true" runat="server" bolEnabled="false" ID="TOTAL" MASK=",999,999"
                                        Text='<%# Eval("TOTAL").ToString().Trim() %>' />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    保證金(含稅)：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="APRV_BOND" MASK=",999,999"
                                        Text='<%# Eval("APRV_BOND").ToString().Trim() %>' />
                                </td>
                                <th>
                                    殘值：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="APRV_REST" MASK=",999,999"
                                        Text='<%# Eval("APRV_REST").ToString().Trim() %>' />
                                </td>
                                <th>
                                    進項稅額：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="APRV_PURS_TAX" MASK=",999,999.99"
                                        Text='<%# Eval("APRV_PURS_TAX").ToString().Trim() %>' />
                                </td>
                                <th>
                                    付款週期：
                                </th>
                                <td>
                                    <asp:DropDownList runat="server" ID="APRV_PAY_PERD" OnPreRender='checkList' ToolTip='<%# Eval("APRV_PAY_DURN").ToString().Trim() %>'
                                        Width="80" DataSourceID="sqlPAY_PERD" DataValueField="TypeCode" DataTextField="TypeDesc">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th class="nonSpace">
                                    期間：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="APRV_DURN_M" MASK="999" Text='<%# Eval("APRV_DURN_M").ToString().Trim() %>' />
                                    個月
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="APRV_PERD" MASK="999" Text='<%# Eval("APRV_PERD").ToString().Trim() %>' />
                                    期
                                </td>
                                <th>
                                    保險因子(%)：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="ISU_FACTOR" MASK="99,999" Text='<%# Eval("A_ISU_FACTOR").ToString().Trim() %>' />
                                </td>
                                <th>
                                    保留款：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="APRV_SAVING_EXP" MASK=",999,999"
                                        Text='<%# Eval("APRV_SAVING_EXP").ToString().Trim() %>' />
                                </td>
                                <th>
                                    實值TR：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="APRV_REAL_TR" bolEnabled="false"
                                        MASK=",999,999" Text='<%# Eval("APRV_REAL_TR").ToString().Trim() %>' />
                                </td>
                            </tr>
                            <tr>
                                <th class="nonSpace">
                                    繳款方式：
                                </th>
                                <td>
                                    <asp:DropDownList runat="server" ID="APRV_PAY_MTHD" OnPreRender='checkList' ToolTip='<%# Eval("APRV_PAY_MTHD").ToString().Trim() %>'
                                        Width="80" DataSourceID="sqlPAY_MTHD" DataValueField="TypeCode" DataTextField="TypeDesc">
                                    </asp:DropDownList>
                                </td>
                                <th>
                                    頭期款(未稅)：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="APRV_DEPS" MASK=",999,999"
                                        Text='<%# Eval("APRV_DEPS").ToString().Trim() %>' />
                                </td>
                                <th>
                                    保險費：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="ISU_AMT" MASK=",999,999" Text='<%# Eval("A_ISU_AMT").ToString().Trim() %>' />
                                </td>
                                <th>
                                    表面TR：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="APRV_SURF_TR" bolEnabled="false"
                                        MASK=",999,999" Text='<%# Eval("APRV_SURF_TR").ToString().Trim() %>' />
                                </td>
                            </tr>
                            <th class="nonSpace">
                                攤提方式：
                            </th>
                            <td>
                                <asp:DropDownList runat="server" onchange="changeMTHD(2,this)" ID="APRV_AMOR_MTHD"
                                    OnPreRender='checkList' ToolTip='<%# Eval("APRV_AMOR_MTHD").ToString().Trim() %>'
                                    Width="80" DataSourceID="sqlAMOR_MTHD" DataValueField="TypeCode" DataTextField="TypeDesc">
                                </asp:DropDownList>
                            </td>
                            <th>
                                手續費：
                            </th>
                            <td>
                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APRV_SERV_CHAR" MASK=",999,999"
                                    Text='<%# Eval("APRV_SERV_CHAR").ToString().Trim() %>' />
                            </td>
                            </tr>
                            <tr>
                                <th class="nonSpace">
                                    稅別：
                                </th>
                                <td>
                                    <asp:DropDownList runat="server" ID="APRV_TAX_ZERO" OnPreRender='checkList' ToolTip='<%# Eval("APRV_TAX_ZERO").ToString().Trim() %>'
                                        Width="80" DataSourceID="sqlTAX_TYPE" DataValueField="TypeCode" DataTextField="TypeDesc">
                                    </asp:DropDownList>
                                </td>
                                <th colspan="2">
                                    不計業績手續費：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="NON_FEAT_CHARGE" MASK=",999,999"
                                        Text='<%# Eval("NON_FEAT_CHARGE").ToString().Trim() %>' />
                                </td>
                                <th>
                                    其他利息：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="APRV_OTH_INT" bolEnabled="false"
                                        MASK=",999,999" Text='<%# Eval("APRV_OTH_INT").ToString().Trim() %>' />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <table cellpadding="0" cellspacing="0" style="margin: 0">
                                        <tr>
                                            <td>
                                                <fieldset id="MTHD2_1" style="padding: 0; margin: 0; display: none">
                                                    <legend>定額</legend>
                                                    <table style="margin: 0px" cellpadding="1" cellspacing="1">
                                                        <tr>
                                                            <th>
                                                                租金：
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APRV_HIRE" MASK="9,999,999"
                                                                    Text='<%# Eval("APRV_HIRE").ToString().Trim() %>' />
                                                            </td>
                                                            <th>
                                                                稅金：
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APRV_TAX" MASK="9,999,999"
                                                                    Text='<%# Eval("APRV_TAX").ToString().Trim() %>' />
                                                                <span style="display: none">
                                                                    <asp:TextBox runat="server" ID="hidden_APRV_TAX"></asp:TextBox>
                                                                </span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                                <br />
                                                <fieldset id="MTHD2_2" style="padding: 0; margin: 0; display: none">
                                                    <legend>階定額</legend>
                                                    <table style="margin: 0px; width: 300px" cellpadding="1" cellspacing="1">
                                                        <tr>
                                                            <th>
                                                                LF1：
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APRV_LF1_FR" bolEnabled="false"
                                                                    MASK="99" Text='<%# Eval("APRV_LF1_FR").ToString().Trim() %>' />
                                                                ~
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APRV_LF1_TO" MASK="99" Text='<%# Eval("APRV_LF1_TO").ToString().Trim() %>' />
                                                            </td>
                                                            <th>
                                                                租金：
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APRV_LF1_HIRE" MASK=",999,999"
                                                                    Text='<%# Eval("APRV_LF1_HIRE").ToString().Trim() %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th>
                                                                LF2：
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APRV_LF2_FR" bolEnabled="false"
                                                                    MASK="99" Text='<%# Eval("APRV_LF2_FR").ToString().Trim() %>' />
                                                                ~
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APRV_LF2_TO" MASK="99" Text='<%# Eval("APRV_LF2_TO").ToString().Trim() %>' />
                                                            </td>
                                                            <th>
                                                                租金：
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APRV_LF2_HIRE" MASK=",999,999"
                                                                    Text='<%# Eval("APRV_LF2_HIRE").ToString().Trim() %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th>
                                                                LF3：
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APRV_LF3_FR" bolEnabled="false"
                                                                    MASK="99" Text='<%# Eval("APRV_LF3_FR").ToString().Trim() %>' />
                                                                ~
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APRV_LF3_TO" MASK="99" Text='<%# Eval("APRV_LF3_TO").ToString().Trim() %>' />
                                                            </td>
                                                            <th>
                                                                租金：
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APRV_LF3_HIRE" MASK=",999,999"
                                                                    Text='<%# Eval("APRV_LF3_HIRE").ToString().Trim() %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th>
                                                                LF4：
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APRV_LF4_FR" bolEnabled="false"
                                                                    MASK="99" Text='<%# Eval("APRV_LF4_FR").ToString().Trim() %>' />
                                                                ~
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APRV_LF4_TO" MASK="99" Text='<%# Eval("APRV_LF4_TO").ToString().Trim() %>' />
                                                            </td>
                                                            <th>
                                                                租金：
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APRV_LF4_HIRE" MASK=",999,999"
                                                                    Text='<%# Eval("APRV_LF4_HIRE").ToString().Trim() %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th>
                                                                LF5：
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APRV_LF5_FR" bolEnabled="false"
                                                                    MASK="99" Text='<%# Eval("APRV_LF5_FR").ToString().Trim() %>' />
                                                                ~
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APRV_LF5_TO" MASK="99" Text='<%# Eval("APRV_LF5_TO").ToString().Trim() %>' />
                                                            </td>
                                                            <th>
                                                                租金：
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APRV_LF5_HIRE" MASK=",999,999"
                                                                    Text='<%# Eval("APRV_LF5_HIRE").ToString().Trim() %>' />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                                <br />
                                                <fieldset style="padding: 0; margin: 0">
                                                    <legend></legend>
                                                    <table style="margin: 0px; width: 300px" cellpadding="1" cellspacing="1">
                                                        <tr>
                                                            <th>
                                                                收入總額：
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APRV_INCM_TOL" bolEnabled="false"
                                                                    MASK=",999,999" Text='<%# Eval("APRV_INCM_TOL").ToString().Trim() %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th>
                                                                銷項稅額：
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APRV_SELL_TAX" bolEnabled="false"
                                                                    MASK=",999,999" Text='<%# Eval("APRV_SELL_TAX").ToString().Trim() %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th>
                                                                毛收益：
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="APRV_MARG" bolEnabled="false"
                                                                    MASK=",999,999" Text='<%# Eval("APRV_MARG").ToString().Trim() %>' />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                                <br />
                                            </td>
                    </ItemTemplate>
                </asp:Repeater>
                <td>
                    <asp:UpdatePanel runat="server" ID="upSet" RenderMode="Inline" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="divFunction80" id="Set2">
                                <asp:Button runat="server" ID="btnSet_Run" CssClass="button trn80" Text="攤提試算" OnCommand="GridFunc_Click"
                                    CommandName="Set_Run" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                    <asp:UpdatePanel runat="server" ID="upTR" RenderMode="Inline" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="divFunction80" id="TR2">
                                <asp:Button runat="server" ID="btnTR_Run" CssClass="button trn80" Text="TR計算" OnCommand="GridFunc_Click"
                                    CommandName="TR_Run" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                </tr></table> </td>
                <td colspan="4">
                    <asp:UpdatePanel ID="upShare1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                        <ContentTemplate>
                            <div class="tabs-header" style="width: 378px;">
                                <div class="tabs-scroller-left" style="display: none;">
                                </div>
                                <div class="tabs-scroller-right" style="display: none;">
                                </div>
                                <div class="tabs-wrap" style="margin-left: 0px; margin-right: 0px; width: 370px;">
                                    <ul class="tabs">
                                        <li class="tabs-selected" id="tabS1" onclick="tabS_select(1,3)"><a href="javascript:void(0)"
                                            class="tabs-inner"><span class="tabs-title">表面TR</span><span class="tabs-icon"></span></a></li>
                                        <li class="" id="tabS2" onclick="tabS_select(2,3)"><a href="javascript:void(0)" class="tabs-inner">
                                            <span class="tabs-title">實質TR</span><span class="tabs-icon"></span></a></li>
                                        <li class="" id="tabS3" onclick="tabS_select(3,3)"><a href="javascript:void(0)" class="tabs-inner">
                                            <span class="tabs-title">表面+其他費用</span><span class="tabs-icon"></span></a></li>
                                    </ul>
                                </div>
                            </div>
                            <div class="tabs-panels" style="width: 378px;">
                                <div class="panel" style="height: auto; width: 370px; display: block" id="tabpanelS1">
                                    <div class="gridMain " style="width: 355px">
                                        <div style="padding: 0; margin: 0; position: relative; overflow-y: scroll; width: 355px;
                                            height: 180px" id="editGrid">
                                            <table cellpadding="0" cellspacing="0" style="width: 180px">
                                                <thead>
                                                    <tr>
                                                        <th>
                                                            期數
                                                        </th>
                                                        <th>
                                                            租金
                                                        </th>
                                                        <th>
                                                            利息
                                                        </th>
                                                        <th>
                                                            本金攤還
                                                        </th>
                                                        <th>
                                                            本金餘額
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <asp:Repeater runat="server" ID="rptShare1">
                                                    <ItemTemplate>
                                                        <tr class="<%#Container.ItemIndex%2==0?"srow":"" %>">
                                                            <td class="number">
                                                                <%# (Eval("PERIOD").ToString().Trim()=="0"?"頭款":Eval("PERIOD","{0:###,###,###,##0}")) %>
                                                            </td>
                                                            <td class="number" style='display: <%# Eval("PERIOD").ToString().Trim()!="0" && this.APRV_MTHD=="5"?"none":"" %>'>
                                                            </td>
                                                            <td class="number" style='display: <%# Eval("PERIOD").ToString().Trim()!="0" && this.APRV_MTHD=="5"?"":"none" %>'>
                                                                <ocxControl:ocxNumber Minus="true" runat="server" ID="HIRE" MASK="999,999" Text='<%# Eval("HIRE").ToString().Trim() %>' />
                                                            </td>
                                                            <td class="number">
                                                                <%# Eval("DIVD", "{0:###,###,###,##0}")%>
                                                            </td>
                                                            <td class="number">
                                                                <%# Eval("CAPT_AMOR", "{0:###,###,###,##0}")%>
                                                            </td>
                                                            <td class="number">
                                                                <%# Eval("REST","{0:###,###,###,##0}") %>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </table>
                                        </div>
                                    </div>
                                    <table>
                                        <tr>
                                            <td>
                                                合計：
                                            </td>
                                            <td>
                                                <div style="width: 120px" class="text number" id="rptShare1_HIRE">
                                                </div>
                                            </td>
                                            <td>
                                                <div style="width: 120px" class="text number" id="rptShare1_DIVD">
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="panel" style="height: auto; width: 370px; display: none" id="tabpanelS2">
                                    <div class="gridMain " style="width: 355px">
                                        <div style="padding: 0; margin: 0; position: relative; overflow-y: scroll; width: 355px;
                                            height: 180px" id="editGrid">
                                            <table cellpadding="0" cellspacing="0" style="width: 180px">
                                                <thead>
                                                    <tr>
                                                        <th>
                                                            期數
                                                        </th>
                                                        <th>
                                                            租金
                                                        </th>
                                                        <th>
                                                            利息
                                                        </th>
                                                        <th>
                                                            本金攤還
                                                        </th>
                                                        <th>
                                                            本金餘額
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <asp:Repeater runat="server" ID="rptShareReal1">
                                                    <ItemTemplate>
                                                        <tr class="<%#Container.ItemIndex%2==0?"srow":"" %>">
                                                            <td class="number">
                                                                <%# (Eval("PERIOD").ToString().Trim()=="0"?"頭款":Eval("PERIOD","{0:###,###,###,##0}")) %>
                                                            </td>
                                                            <td class="number">
                                                                <%# Eval("HIRE", "{0:###,###,###,##0}")%>
                                                            </td>
                                                            <td class="number">
                                                                <%# Eval("DIVD", "{0:###,###,###,##0}")%>
                                                            </td>
                                                            <td class="number">
                                                                <%# Eval("CAPT_AMOR", "{0:###,###,###,##0}")%>
                                                            </td>
                                                            <td class="number">
                                                                <%# Eval("REST", "{0:###,###,###,##0}")%>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </table>
                                        </div>
                                    </div>
                                    <table>
                                        <tr>
                                            <td>
                                                合計：
                                            </td>
                                            <td>
                                                <div style="width: 120px" class="text number" id="rptShareReal1_HIRE">
                                                </div>
                                            </td>
                                            <td>
                                                <div style="width: 120px" class="text number" id="rptShareReal1_DIVD">
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="panel" style="height: auto; width: 370px; display: none" id="tabpanelS3">
                                    <div class="gridMain " style="width: 355px">
                                        <div style="padding: 0; margin: 0; position: relative; overflow-y: scroll; width: 355px;
                                            height: 180px" id="editGrid">
                                            <table cellpadding="0" cellspacing="0" style="width: 160px">
                                                <thead>
                                                    <tr>
                                                        <th>
                                                            期數
                                                        </th>
                                                        <th>
                                                            租金
                                                        </th>
                                                        <th>
                                                            利息
                                                        </th>
                                                        <th>
                                                            本金攤還
                                                        </th>
                                                        <th>
                                                            本金餘額
                                                        </th>
                                                        <th>
                                                            扣表面本餘
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <asp:Repeater runat="server" ID="rptShareEx1">
                                                    <ItemTemplate>
                                                        <tr class="<%#Container.ItemIndex%2==0?"srow":"" %>">
                                                            <td class="number">
                                                                <%# (Eval("PERIOD").ToString().Trim()=="0"?"頭款":Eval("PERIOD","{0:###,###,###,##0}")) %>
                                                            </td>
                                                            <td class="number">
                                                                <%# Eval("HIRE", "{0:###,###,###,##0}")%>
                                                            </td>
                                                            <td class="number">
                                                                <%# Eval("DIVD", "{0:###,###,###,##0}")%>
                                                            </td>
                                                            <td class="number">
                                                                <%# Eval("CAPT_AMOR", "{0:###,###,###,##0}")%>
                                                            </td>
                                                            <td class="number">
                                                                <%# Eval("REST", "{0:###,###,###,##0}")%>
                                                            </td>
                                                            <td class="number">
                                                                <%# Eval("TOTAL", "{0:###,###,###,##0}")%>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </table>
                                        </div>
                                    </div>
                                    <table>
                                        <tr>
                                            <td>
                                                合計：
                                            </td>
                                            <td>
                                                <div style="width: 120px" class="text number">
                                                </div>
                                            </td>
                                            <td>
                                                <div style="width: 120px" class="text number">
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                </tr> </table>
            </div>
            <div class="panel" style="height: auto; width: 820px; display: none" id="tabpanel6">
                <asp:UpdatePanel runat="server" ID="upContact" UpdateMode="Conditional" RenderMode="Inline">
                    <ContentTemplate>
                        <table cellpadding="1" cellspacing="1" style="margin-left: 10px">
                            <asp:Repeater runat="server" ID="rptContact">
                                <ItemTemplate>
                                    <tr>
                                        <th colspan="5">
                                            OALT 餘額：
                                        </th>
                                        <td>
                                            <div style="width: 120px" class="text number">
                                                <%# Eval("OALT_AMT","{0:###,###,###,##0}") %></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <h4>
                                                本戶</h4>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <font class="memo">申請中</font>
                                        </td>
                                        <td>
                                            <font class="memo">已核準</font>
                                        </td>
                                        <td style="background-color: gold">
                                            <font class="memo">總額</font>
                                        </td>
                                        <td style="background-color: gold">
                                            <font class="memo">已使用額度</font>
                                        </td>
                                        <td style="background-color: gold">
                                            <font class="memo">剩餘額度</font>
                                        </td>
                                        <td style="background-color: gold">
                                            <font class="memo">契約額度</font>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            申請本案：
                                        </th>
                                        <td>
                                            <div style="width: 100px" class="text number">
                                                <%# Eval("L_THIS", "{0:###,###,###,##0}")%></div>
                                        </td>
                                        <th>
                                            含前案額度-循環：
                                        </th>
                                        <td>                                           
                                            <ocxControl:ocxNumber Minus="true" bolEnabled=false  runat="server" ID="L_APRV_IPREV_QUOTA_CYCLE" MASK="999,999,999" Text='<%# Eval("L_APRV_IPREV_QUOTA_CYCLE").ToString().Trim() %>' />
                                                
                                        </td>
                                        <td>
                                            <div style="width: 100px" class="text number">
                                                <%# Eval("L_IPREV_QUOTA_CYCLE_USED", "{0:###,###,###,##0}")%></div>
                                        </td>
                                        <td>
                                            <div style="width: 100px" class="text number">
                                                <%# Eval("L_IPREV_QUOTA_CYCLE_Remains", "{0:###,###,###,##0}")%></div>
                                        </td>
                                        <td>
                                            <div style="width: 100px" class="text number">
                                                <%# Eval("L_IPREV_CYCLE_SUR", "{0:###,###,###,##0}")%></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            非使用：
                                        </th>
                                        <td>
                                        </td>
                                        <th>
                                            含前案額度-非循環：
                                        </th>
                                        <td>
                                            <ocxControl:ocxNumber Minus="true" bolEnabled=false  runat="server" ID="L_APRV_IPREV_QUOTA_NCYCLE" MASK="999,999,999" Text='<%# Eval("L_APRV_IPREV_QUOTA_NCYCLE").ToString().Trim() %>' />
                                           
                                        </td>
                                        <td>
                                            <div style="width: 100px" class="text number">
                                                <%# Eval("L_IPREV_QUOTA_NCYCLE_USED", "{0:###,###,###,##0}")%></div>
                                        </td>
                                        <td>
                                            <div style="width: 100px" class="text number">
                                                <%# Eval("L_IPREV_QUOTA_NCYCLE_Remains", "{0:###,###,###,##0}")%></div>
                                        </td>
                                        <td>
                                            <div style="width: 100px" class="text number">
                                                <%# Eval("L_IPREV_NCYCLE_SUR", "{0:###,###,###,##0}")%></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            申請中：
                                        </th>
                                        <td>
                                            <div style="width: 100px" class="text number">
                                                <%# Eval("L_APLY", "{0:###,###,###,##0}")%></div>
                                        </td>
                                        <th>
                                            其他額度-循環：
                                        </th>
                                        <td>
                                            <div style="width: 100px" class="text number">
                                                <%# Eval("L_APRV_PREV_QUOTA_CYCLE", "{0:###,###,###,##0}")%></div>
                                        </td>
                                        <td>
                                            <div style="width: 100px" class="text number">
                                                <%# Eval("L_PREV_QUOTA_CYCLE_USED", "{0:###,###,###,##0}")%></div>
                                        </td>
                                        <td>
                                            <div style="width: 100px" class="text number">
                                                <%# Eval("L_PREV_QUOTA_CYCLE_Remains", "{0:###,###,###,##0}")%></div>
                                        </td>
                                        <td>
                                            <div style="width: 100px" class="text number">
                                                <%# Eval("L_PREV_CYCLE_SUR", "{0:###,###,###,##0}")%></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            已核準：
                                        </th>
                                        <td>
                                            <div style="width: 100px" class="text number">
                                                <%# Eval("L_PREV", "{0:###,###,###,##0}")%></div>
                                        </td>
                                        <th>
                                            其他額度-非循環：
                                        </th>
                                        <td>
                                            <div style="width: 100px" class="text number">
                                                <%# Eval("L_APRV_PREV_QUOTA_NCYCLE", "{0:###,###,###,##0}")%></div>
                                        </td>
                                        <td>
                                            <div style="width: 100px" class="text number">
                                                <%# Eval("L_PREV_QUOTA_NCYCLE_USED", "{0:###,###,###,##0}")%></div>
                                        </td>
                                        <td>
                                            <div style="width: 100px" class="text number">
                                                <%# Eval("L_PREV_QUOTA_NCYCLE_Remains", "{0:###,###,###,##0}")%></div>
                                        </td>
                                        <td>
                                            <div style="width: 100px" class="text number">
                                                <%# Eval("L_PREV_NCYCLE_SUR", "{0:###,###,###,##0}")%></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <th>
                                            前案單筆餘額：
                                        </th>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <div style="width: 100px" class="text number">
                                                <%# Eval("L_APRV_PREV_QUOTA_SURPLUS", "{0:###,###,###,##0}")%></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <th>
                                            小計：
                                        </th>
                                        <th>
                                            曝險值：
                                        </th>
                                        <td>
                                            <div style="width: 100px" class="text number">
                                                <%# Eval("L_RISKS", "{0:###,###,###,##0}")%></div>
                                        </td>
                                        <th>
                                            累計餘額：
                                        </th>
                                        <td>
                                            <div style="width: 100px" class="text number">
                                                <%# Eval("L_SURPLUS", "{0:###,###,###,##0}")%></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <h4>
                                                關係戶</h4>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            申請中：
                                        </th>
                                        <td>
                                            <div style="width: 100px" class="text number">
                                                <%# Eval("R_APLY", "{0:###,###,###,##0}")%></div>
                                        </td>
                                        <th>
                                            其他額度-循環：
                                        </th>
                                        <td>
                                            <div style="width: 100px" class="text number">
                                                <%# Eval("R_APRV_PREV_QUOTA_CYCLE", "{0:###,###,###,##0}")%></div>
                                        </td>
                                        <td>
                                            <div style="width: 100px" class="text number">
                                                <%# Eval("R_PREV_QUOTA_CYCLE_USED", "{0:###,###,###,##0}")%></div>
                                        </td>
                                        <td>
                                            <div style="width: 100px" class="text number">
                                                <%# Eval("R_PREV_QUOTA_CYCLE_Remains", "{0:###,###,###,##0}")%></div>
                                        </td>
                                        <td>
                                            <div style="width: 100px" class="text number">
                                                <%# Eval("R_PREV_CYCLE_SUR", "{0:###,###,###,##0}")%></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            已核準：
                                        </th>
                                        <td>
                                            <div style="width: 100px" class="text number">
                                                <%# Eval("R_PREV", "{0:###,###,###,##0}")%></div>
                                        </td>
                                        <th>
                                            其他額度-非循環：
                                        </th>
                                        <td>
                                            <div style="width: 100px" class="text number">
                                                <%# Eval("R_APRV_PREV_QUOTA_NCYCLE", "{0:###,###,###,##0}")%></div>
                                        </td>
                                        <td>
                                            <div style="width: 100px" class="text number">
                                                <%# Eval("R_PREV_QUOTA_NCYCLE_USED", "{0:###,###,###,##0}")%></div>
                                        </td>
                                        <td>
                                            <div style="width: 100px" class="text number">
                                                <%# Eval("R_PREV_QUOTA_NCYCLE_Remains", "{0:###,###,###,##0}")%></div>
                                        </td>
                                        <td>
                                            <div style="width: 100px" class="text number">
                                                <%# Eval("R_PREV_NCYCLE_SUR", "{0:###,###,###,##0}")%></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <th>
                                            前案單筆餘額：
                                        </th>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <div style="width: 100px" class="text number">
                                                <%# Eval("R_APRV_PREV_QUOTA_SURPLUS", "{0:###,###,###,##0}")%></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <th>
                                            小計：
                                        </th>
                                        <th>
                                            曝險值：
                                        </th>
                                        <td>
                                            <div style="width: 100px" class="text number">
                                                <%# Eval("R_RISKS", "{0:###,###,###,##0}")%></div>
                                        </td>
                                        <th>
                                            累計餘額：
                                        </th>
                                        <td>
                                            <div style="width: 100px" class="text number">
                                                <%# Eval("R_SURPLUS", "{0:###,###,###,##0}")%></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <th>
                                            總計：
                                        </th>
                                        <th>
                                            曝險值：
                                        </th>
                                        <td>
                                        <asp:TextBox runat="server" ID="ALL_RISKS" CssClass="display number" Width="100" Text='<%# Eval("ALL_RISKS", "{0:###,###,###,##0}") %>'></asp:TextBox>                                            
                                        </td>
                                        <th>
                                            累計餘額：
                                        </th>
                                        <td>
                                            <asp:TextBox runat="server" ID="ALL_TOT" CssClass="display number" Width="100" Text='<%# Eval("ALL_SURPLUS", "{0:###,###,###,##0}") %>'></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            <tr>
                                <th colspan="6">
                                    <div class="divFunction80">
                                        <asp:Button runat="server" ID="btnSURPLUS" CssClass="button trn80" Text="實績計算" OnCommand="GridFunc_Click"
                                            CommandName="SURPLUS" />
                                    </div>
                                </th>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="panel" style="height: auto; width: 810px; display: none" id="tabpanel7">
                <asp:UpdatePanel runat="server" ID="upAud" RenderMode="Inline" UpdateMode="Conditional">
                    <ContentTemplate>
                        <span style="display: none">
                            <asp:TextBox runat="server" ID="txtCaseType"></asp:TextBox>
                            <asp:Button runat="server" ID="btnReload" OnClick="AUD_Reload" />
                        </span>
                        <div class="gridMain " style="width: 790px">
                            <div style="padding: 0; margin: 0; overflow-y: scroll; position: relative; width: 790px;
                                height: 240px" id="editGrid">
                                <table cellpadding="0" cellspacing="0" style="width: 500px">
                                    <thead>
                                        <tr>
                                            <th>
                                                選擇
                                            </th>
                                            <th>
                                                授權內容
                                            </th>
                                            <th>
                                                審查級
                                            </th>
                                            <th>
                                                備註
                                            </th>
                                        </tr>
                                    </thead>
                                    <asp:Repeater runat="server" ID="rptAud">
                                        <ItemTemplate>
                                            <tr class="<%#Container.ItemIndex%2==0?"srow":"" %>">
                                                <td>
                                                    <asp:CheckBox ID="SEL" runat="server" OnPreRender='checkList' ToolTip='<%# Eval("SEL").ToString().Trim() %>' />
                                                    <asp:HiddenField runat="server" ID="AUD_CODE" Value='<%# Eval("AUD_CODE").ToString().Trim() %>' />
                                                </td>
                                                <td>
                                                    <%# Eval("AUD_NAME") %>
                                                </td>
                                                <td>
                                                    <%# Eval("AUD_LVL_NAME") %>
                                                </td>
                                                <td>
                                                    <%# Eval("MEMO") %>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <table>
                    <tr>
                        <th>
                            說明：
                        </th>
                        <td>
                            <asp:TextBox runat="server" ID="Auth_Cond_Remark" TextMode="MultiLine" Rows="5" Width="720"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="panel" style="height: auto; width: 820px; display: none" id="tabpanel8">
                <asp:UpdatePanel runat="server" ID="upRef" RenderMode="Inline" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table cellpadding="1" cellspacing="1" style="margin-left: 10px">
                            <tr>
                                <td colspan="6" style="text-align: center">
                                    <div class="divFunction80">
                                        <asp:Button runat="server" ID="btnSet_Ref" CssClass="button trn80" Text="授信權限計算"
                                            OnCommand="GridFunc_Click" CommandName="Ref" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <h4>
                                        一般案件</h4>
                                </td>
                                <td colspan="3">
                                    <h4>
                                        VP案件</h4>
                                </td>
                            </tr>
                            <asp:Repeater runat="server" ID="rptRef">
                                <ItemTemplate>
                                    <tr>
                                        <th>
                                            TR權限：
                                        </th>
                                        <td>
                                            <div style="width: 100px" class="text">
                                                <%# Eval("S1").ToString().Trim()%></div>
                                        </td>
                                        <td>
                                            <div style="width: 120px" class="text">
                                                <%# Eval("SN1").ToString().Trim()%></div>
                                        </td>
                                        <th>
                                            TR權限：
                                        </th>
                                        <td>
                                            <div style="width: 100px" class="text">
                                                <%# Eval("S11").ToString().Trim()%></div>
                                        </td>
                                        <td>
                                            <div style="width: 120px" class="text">
                                                <%# Eval("SN11").ToString().Trim()%></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            案件期別權限：
                                        </th>
                                        <td>
                                            <div style="width: 100px" class="text">
                                                <%# Eval("S2").ToString().Trim()%></div>
                                        </td>
                                        <td>
                                            <div style="width: 120px" class="text">
                                                <%# Eval("SN2").ToString().Trim()%></div>
                                        </td>
                                        <th>
                                            案件期別權限：
                                        </th>
                                        <td>
                                            <div style="width: 100px" class="text">
                                                <%# Eval("S12").ToString().Trim()%></div>
                                        </td>
                                        <td>
                                            <div style="width: 120px" class="text">
                                                <%# Eval("SN12").ToString().Trim()%></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            新設公司權限：
                                        </th>
                                        <td>
                                            <div style="width: 100px" class="text">
                                                <%# Eval("S3").ToString().Trim()%></div>
                                        </td>
                                        <td>
                                            <div style="width: 120px" class="text">
                                                <%# Eval("SN3").ToString().Trim()%></div>
                                        </td>
                                        <th>
                                            新設公司權限：
                                        </th>
                                        <td>
                                            <div style="width: 100px" class="text">
                                                <%# Eval("S13").ToString().Trim()%></div>
                                        </td>
                                        <td>
                                            <div style="width: 120px" class="text">
                                                <%# Eval("SN13").ToString().Trim()%></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            契約餘額權限：
                                        </th>
                                        <td>
                                            <div style="width: 100px" class="text">
                                                <%# Eval("S4").ToString().Trim()%></div>
                                        </td>
                                        <td>
                                            <div style="width: 120px" class="text">
                                                <%# Eval("SN4").ToString().Trim()%></div>
                                        </td>
                                        <th>
                                            契約餘額權限：
                                        </th>
                                        <td>
                                            <div style="width: 100px" class="text">
                                                <%# Eval("S14").ToString().Trim()%></div>
                                        </td>
                                        <td>
                                            <div style="width: 120px" class="text">
                                                <%# Eval("SN14").ToString().Trim()%></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            條件權限：
                                        </th>
                                        <td>
                                            <div style="width: 100px" class="text">
                                                <%# Eval("S5").ToString().Trim()%></div>
                                        </td>
                                        <td>
                                            <div style="width: 120px" class="text">
                                                <%# Eval("SN5").ToString().Trim()%></div>
                                        </td>
                                        <th>
                                            條件權限：
                                        </th>
                                        <td>
                                            <div style="width: 100px" class="text">
                                                <%# Eval("S15").ToString().Trim()%></div>
                                        </td>
                                        <td>
                                            <div style="width: 120px" class="text">
                                                <%# Eval("SN15").ToString().Trim()%></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <h4>
                                                IT案件</h4>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            TR權限：
                                        </th>
                                        <td>
                                            <div style="width: 100px" class="text">
                                                <%# Eval("S6").ToString().Trim()%></div>
                                        </td>
                                        <td>
                                            <div style="width: 120px" class="text">
                                                <%# Eval("SN6").ToString().Trim()%></div>
                                        </td>
                                        <td>
                                            <h4>
                                                最高審查權限</h4>
                                        </td>
                                        <td>
                                            <div style="width: 100px" class="text">
                                                <%# Eval("S16").ToString().Trim()%></div>
                                        </td>
                                        <td>
                                            <div style="width: 120px" class="text">
                                                <%# Eval("SN16").ToString().Trim()%></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            案件期別權限：
                                        </th>
                                        <td>
                                            <div style="width: 100px" class="text">
                                                <%# Eval("S7").ToString().Trim()%></div>
                                        </td>
                                        <td>
                                            <div style="width: 120px" class="text">
                                                <%# Eval("SN7").ToString().Trim()%></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            新設公司權限：
                                        </th>
                                        <td>
                                            <div style="width: 100px" class="text">
                                                <%# Eval("S8").ToString().Trim()%></div>
                                        </td>
                                        <td>
                                            <div style="width: 120px" class="text">
                                                <%# Eval("SN8").ToString().Trim()%></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            契約餘額權限：
                                        </th>
                                        <td>
                                            <div style="width: 100px" class="text">
                                                <%# Eval("S9").ToString().Trim()%></div>
                                        </td>
                                        <td>
                                            <div style="width: 120px" class="text">
                                                <%# Eval("SN9").ToString().Trim()%></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            條件權限：
                                        </th>
                                        <td>
                                            <div style="width: 100px" class="text">
                                                <%# Eval("S10").ToString().Trim()%></div>
                                        </td>
                                        <td>
                                            <div style="width: 120px" class="text">
                                                <%# Eval("SN10").ToString().Trim()%></div>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="panel" style="height: auto; width: 820px; display: none" id="tabpanel9">
                <asp:UpdatePanel ID="upOpin" UpdateMode="Conditional" RenderMode="Inline" runat="server">
                    <ContentTemplate>                       
                        <div class="gridMain " style="width: 790px">
                            <div style="padding: 0; margin: 0; overflow-y: scroll; position: relative; width: 790px;
                                height: 200px" id="editGrid">
                                <table cellpadding="0" cellspacing="0" style="width: 730px">
                                    <thead>
                                        <tr>
                                            <th>
                                                審查級別
                                            </th>
                                            <th>
                                                結果
                                            </th>
                                            <th width="430">
                                                <div>
                                                    審查意見</div>
                                                <div style="position: absolute; z-index: 101; margin-left: -4px; margin-top: 4px">
                                                    <asp:TextBox runat="server" ID="txtOpin" Width="410" TextMode="MultiLine" Rows="11" onmouseout='aud_mouseout()'></asp:TextBox>
                                                </div>
                                            </th>
                                        </tr>
                                    </thead>
                                    <asp:Repeater runat="server" ID="rptOpin">
                                        <ItemTemplate>
                                            <tr style="cursor: pointer" onmouseover='aud_mouseover(this,<%# Container.ItemIndex+1 %>)' onmouseout='aud_mouseout()'>
                                                <td style="width: 180px;">
                                                    <asp:HiddenField runat="server" ID="AUD_LVL_CODE" Value='<%# Eval("AUD_LVL_CODE").ToString().Trim() %>' />
                                                    <asp:HiddenField runat="server" ID="LVL" Value='<%# Eval("LVL").ToString().Trim() %>' />
                                                    <%# Eval("AUD_LVL_NAME").ToString().Trim() %>
                                                </td>
                                                <td style="width: 120px">
                                                    <asp:DropDownList ID="IF_PASS" Style="width: 120px" Enabled='<%# Eval("ENABLED").ToString().Trim()=="1" && (this.STS.Trim()=="1" || this.STS.Trim()=="")?true:false %>'
                                                        runat="server" OnPreRender='checkList' ToolTip='<%# Eval("IF_PASS").ToString().Trim() %>'
                                                        DataSourceID="sqlIF_PASS" DataValueField="TypeCode" DataTextField="TypeDesc">
                                                    </asp:DropDownList>
                                                    <div style="z-index: 101; display: none" id="opin<%# Container.ItemIndex+1 %>" class='<%# Eval("ENABLED").ToString().Trim()=="1" && (this.STS.Trim()=="1" || this.STS.Trim()=="")?"enabled":"disabled" %>'>
                                                        <asp:TextBox runat="server" ID="AUD_OPIN" Width="450" TextMode="MultiLine" Rows="11" Text='<%# Eval("AUD_OPIN").ToString().Trim().Replace("\\n", Environment.NewLine) %>'></asp:TextBox></div>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="gridMain " style="width: 790px">
                    <div style="padding: 0; margin: 0; overflow-y: scroll; position: relative; width: 790px;
                        height: 200px" id="editGrid">
                        <table cellpadding="0" cellspacing="0" style="width: 470px">
                            <thead>
                                <tr>
                                    <th>
                                        簽呈號碼
                                    </th>
                                    <th>
                                        簽呈主旨
                                    </th>
                                    <th>
                                        扣款金額
                                    </th>
                                </tr>
                            </thead>
                            <asp:Repeater runat="server" ID="rptSign">
                                <ItemTemplate>
                                    <tr>
                                        <td style="width: 170px">
                                            <asp:TextBox runat="server" ID="SIGNED_NO" MaxLength="20" Width="170" Text='<%# Eval("SIGNED_NO").ToString().Trim() %>'></asp:TextBox>
                                        </td>
                                        <td style="width: 200px">
                                            <%# Eval("MAIN").ToString().Trim() %>
                                        </td>
                                        <td style="width: 100px">
                                            <ocxControl:ocxNumber Minus="true" runat="server" ID="DEBIT_AMT" MASK="999,999" Text='<%# Eval("DEBIT_AMT").ToString().Trim() %>' />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </div>
                </div>
                <br />
            </div>
            <div class="panel" style="height: auto; width: 810px; display: none" id="tabpanel10">
                <div class="gridMain " style="width: 790px">
                    <div style="padding: 0; margin: 0; overflow-y: scroll; position: relative; width: 790px;
                        height: 240px" id="editGrid">
                        <table cellpadding="0" cellspacing="0" style="width: 780px">
                            <thead>
                                <tr>
                                    <th>
                                    </th>
                                    <th>
                                        客戶代號
                                    </th>
                                    <th>
                                        客戶名稱
                                    </th>
                                    <th>
                                        連絡人
                                    </th>
                                    <th>
                                        電話
                                    </th>
                                    <th>
                                        傳真
                                    </th>
                                    <th>
                                        手機
                                    </th>
                                    <th>
                                        Email
                                    </th>
                                </tr>
                            </thead>
                            <tr>
                                <td>
                                    承租人1
                                </td>
                                <td>
                                    <input id="RENT_CUST_NO" type="text" style="width: 100px" readonly="readonly" class="display" />
                                </td>
                                <td>
                                    <input id="RENT_CUST_NAME" type="text" style="width: 180px" readonly="readonly" class="display" />
                                </td>
                                <td>
                                    <input id="RENT_CONTACT" type="text" style="width: 100px" readonly="readonly" class="display" />
                                </td>
                                <td>
                                    <input id="RENT_TEL" type="text" style="width: 100px" readonly="readonly" class="display" />
                                </td>
                                <td>
                                    <input id="RENT_FACSIMILE" type="text" style="width: 100px" readonly="readonly" class="display" />
                                </td>
                                <td>
                                    <input id="RENT_MOBILE" type="text" style="width: 100px" readonly="readonly" class="display" />
                                </td>
                                <td>
                                    <input id="RENT_EMAIL" type="text" style="width: 150px" readonly="readonly" class="display" />
                                </td>
                            </tr>
                            <asp:Repeater runat="server" ID="rptRent">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            承租人<%# Container.ItemIndex+2 %>
                                        </td>
                                        <td>
                                            <ocxControl:ocxDialog runat="server" ID="CUST_NO" width="80" Text='<%# Eval("CUST_NO").ToString().Trim() %>'
                                                FieldName="CUST_SNAME" SourceName="OR_CUSTOM" />
                                        </td>
                                        <td>
                                            <asp:TextBox CssClass="display" ReadOnly="true" runat="server" ID="CUST_SNAME" MaxLength="20"
                                                Width="100" Text='<%# Eval("CUST_SNAME").ToString().Trim() %>'></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="CONTACT" MaxLength="20" Width="100" Text='<%# Eval("CONTACT").ToString().Trim() %>'></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="TEL" MaxLength="20" Width="100" Text='<%# Eval("TEL").ToString().Trim() %>'></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="FACSIMILE" MaxLength="20" Width="100" Text='<%# Eval("FACSIMILE").ToString().Trim() %>'></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="MOBILE" MaxLength="20" Width="100" Text='<%# Eval("MOBILE").ToString().Trim() %>'></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="EMAIL" MaxLength="20" Width="100" Text='<%# Eval("EMAIL").ToString().Trim() %>'></asp:TextBox>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </div>
                </div>
                <br>
            </div>
            <div class="panel" style="height: auto; width: 820px; display: none" id="tabpanel11">
                <asp:Repeater runat="server" ID="rptCon">
                    <ItemTemplate>
                        <table style="margin: 20px; margin-top: 0px">
                            <tr>
                                <th>
                                    是否為計張：
                                </th>
                                <td>
                                    <asp:DropDownList ID="PAPER" runat="server" OnPreRender='checkList' ToolTip='<%# Eval("PAPER").ToString().Trim() %>'>
                                        <asp:ListItem Value="N">否</asp:ListItem>
                                        <asp:ListItem Value="Y">是</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <th>
                                    合約類別：
                                </th>
                                <td>
                                    <asp:DropDownList ID="CON_TYPE" runat="server" OnPreRender='checkList' ToolTip='<%# Eval("CON_TYPE").ToString().Trim() %>'
                                        DataSourceID="sqlCON_TYPE" DataValueField="TypeCode" DataTextField="TypeDesc">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th class="nonSpace">
                                    合約範本：
                                </th>
                                <td>
                                    <asp:DropDownList ID="TMP_CODE" runat="server" OnPreRender='checkList' ToolTip='<%# Eval("TMP_CODE").ToString().Trim() %>'
                                        DataSourceID="sqlTMP_CODE" DataValueField="TMP_CODE" DataTextField="TMP_DESC">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:CheckBox ID="EXPIRED_RENEW" runat="server" OnPreRender='checkList' ToolTip='<%# Eval("EXPIRED_RENEW").ToString().Trim() %>' />期滿再續約
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    租金：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="HIRE" MASK="999,999" Text='<%# Eval("HIRE").ToString().Trim() %>' />
                                </td>
                                <th>
                                    營業稅：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="TAX" MASK="999.9999" Text='<%# Eval("TAX").ToString().Trim() %>' />
                                </td>
                                <th>
                                    年：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="YAR" MASK="999" Text='<%# Eval("YAR").ToString().Trim() %>' />
                                </td>
                                <th>
                                    月：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="MTH" MASK="999" Text='<%# Eval("MTH").ToString().Trim() %>' />
                                </td>
                                <th>
                                    期數：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber Minus="true" runat="server" ID="PERIOD" MASK="999" Text='<%# Eval("PERIOD").ToString().Trim() %>' />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:CheckBox ID="PACKAGE" runat="server" OnPreRender='checkList' ToolTip='<%# Eval("PACKAGE").ToString().Trim() %>' />套票
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:CheckBox ID="CASHIER" runat="server" OnPreRender='checkList' ToolTip='<%# Eval("CASHIER").ToString().Trim() %>' />簽立本票、授權書
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:CheckBox ID="CHG_CON" runat="server" OnPreRender='checkList' ToolTip='<%# Eval("CHG_CON").ToString().Trim() %>' />換約(附帶條款加註舊約終止字樣)
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    原契約號碼：
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="PRV_APLY_NO" MaxLength="20" Width="100" Text='<%# Eval("PRV_APLY_NO").ToString().Trim() %>'></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:CheckBox ID="CHG_CODICIL" runat="server" OnPreRender='checkList' ToolTip='<%# Eval("CHG_CODICIL").ToString().Trim() %>' />中途換約附帶條文
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <fieldset>
                                        <legend>有無期間限制</legend>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:RadioButton runat="server" ID="LimitN" Checked='<%# Eval("Limit").ToString().Trim()=="Y" %>' />無期間限制
                                                </td>
                                                <td>
                                                    <asp:RadioButton runat="server" ID="LimitY" Checked='<%# Eval("Limit").ToString().Trim()=="N" %>' />有期間限制
                                                </td>
                                                <td>
                                                    期(月)<ocxControl:ocxNumber Minus="true" runat="server" ID="RESTRICTION_PERIODS" MASK="999"
                                                        Text='<%# Eval("RESTRICTION_PERIODS").ToString().Trim() %>' />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="FREE_SHOW_MAC_NO" runat="server" OnPreRender='checkList' ToolTip='<%# Eval("FREE_SHOW_MAC_NO").ToString().Trim() %>' />免Show機號
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    附帶條款：
                                </th>
                                <td colspan="9">
                                    <asp:TextBox TextMode="MultiLine" Rows="6" runat="server" ID="CODICIL" Width="500"
                                        Text='<%# Eval("CODICIL").ToString().Trim() %>'></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>
    <asp:UpdatePanel ID="upCustom" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <span style="display: none">
                <asp:Button runat="server" ID="reloadCustom" OnClick="Reload_Custom" />
                <asp:Button runat="server" ID="btnMobile" OnCommand="GridFunc_Click" CommandName="Agent" />
            </span>
        </ContentTemplate>
    </asp:UpdatePanel>
    <span style="display: none">
        <asp:TextBox runat="server" ID="checkObj"></asp:TextBox>
        <asp:DropDownList ID="dpFACTOR" runat="server" DataSourceID="sqlASUR_TYPE_CODE" DataValueField="TypeCode"
            DataTextField="ASUR_FACTOR">
        </asp:DropDownList>
    </span>
</asp:Content>
<asp:Content ID="myPopWindow" ContentPlaceHolderID="PopWindow" runat="server">

    <script language="javascript" type="text/javascript">



function calendarShown(sender, args) {
sender._popupBehavior._element.style.zIndex = 5000005;
}
//function calendarShown(sender, args) { alert(sender._popupBehavior._element.style.position);sender._popupBehavior._element.style.zIndex = 1000005;


    function checkObject_Fail() {
        if (window.confirm("本案包含『非OTC資產』標的，你是否確認？(定義：若為『一般營業型、資本型、分期案件』都應為OTC資產，VP計張案件供應商提供之標的物但須我司代收代付與管理的標的才非OTC資產)Y/N？")) 
        {
            document.getElementById("<%=this.checkObj.ClientID %>").value = "Y";
            document.getElementById("ctl00_ctl00_body_btnEdit").click();
        }
        
    }

function setCustom(CUST_NO,CUST_NAME,CONTACT,TEL,FACS,MOBILE,EMAIL) {

        document.getElementById('RENT_CUST_NO').value = CUST_NO;
        document.getElementById('RENT_CUST_NAME').value = CUST_NAME;
        document.getElementById('RENT_CONTACT').value = CONTACT;
        document.getElementById('RENT_TEL').value = TEL;
        document.getElementById('RENT_FACSIMILE').value = FACS;
        document.getElementById('RENT_MOBILE').value = MOBILE;
        document.getElementById('RENT_EMAIL').value = EMAIL;
        
    }
    
    var oldF = document.getElementById('<%=this.rptObjDetail.Items[0].FindControl("FRC_CODE").ClientID %>').value;
    var old = document.getElementById('<%=this.rptBase.Items[0].FindControl("CUST_NO").ClientID %>').value;
    var oldType = "<%=this.txtCaseType.Text.Trim() %>";
    function dialogChange(sid, val) {

        if (sid.indexOf("rptBase_ctl00_CUST_NO") != -1)
        {
            if ( trim(val) != trim(old)){                
                document.getElementById("<%=this.reloadCustom.ClientID %>").click();
            }
            old = val;
        }
        if (sid.indexOf("FRC_CODE") != -1)
        {
            if ( trim(val) != trim(oldF)){                
                setRETK();
            }
            oldF = val;
        }
        if (sid.indexOf("AUD_CASE_TYPE") != -1) {

            if (val != "" && val != oldType) {
                document.getElementById("<%=this.txtCaseType.ClientID %>").value = val;
                document.getElementById("<%=this.btnReload.ClientID %>").click();
            }

            oldType = val;
        }
        
     
    }

    
    //select main from orix_otc..otc_proced_content where PROCED_APLY_NO='[電簽號碼]' and APLY_COM='OTC'
        

     
//APRV_PURS_TAX APRV_BUY_AMT
     function init() {
     
         $('input[id*=APRV_BUY_AMT]').blur(APRV_BUY_AMT_Change);
         $('input[id*=APLY_BUY_AMT]').blur(APLY_BUY_AMT_Change);
         $('input[id*=APLY_HIRE]').blur(APLY_HIRE_Change);
         $('input[id*=APRV_HIRE]').blur(APRV_HIRE_Change);
          $('select[id*=APLY_TAX_ZERO]').change(APLY_HIRE_Change);
         $('select[id*=APRV_TAX_ZERO]').change(APRV_HIRE_Change);
         $('select[id*=rptASUR][id*=ASUR_TYPE_CODE]').change(ASUR_TYPE_CODE_Change);
          
         
          $('input[id*=AGENT]').change(setAgentData);
          
        $('input[id*=APLY_LF][id*=TO]').blur(APLY_Change);
        $('input[id*=APRV_LF][id*=TO]').blur(APRV_Change);
        $('input[id*=SIGNED_NO]').change(SIGNED_Change);
        $('input[id*=REAL_BUY_PRC]').change(REAL_BUY_PRC_Change);               
       // $('input[id*=OBJ_ASUR_TYPE]').change(OBJ_ASUR_TYPE_Change);

        changeMTHD(1, document.getElementById('<%=this.rptRequest.Items[0].FindControl("APLY_AMOR_MTHD").ClientID %>'));
        changeMTHD(2, document.getElementById('<%=this.rptRun.Items[0].FindControl("APRV_AMOR_MTHD").ClientID %>'));
        
   }
  
   function REAL_BUY_PRC_Change()
   {
          $('input[id*=BUDG_LEASE_AMT]').val($(this).val());
          $('input[id*=INV_AMT_I_IB]').val($(this).val());
   }
   
    function ASUR_TYPE_CODE_Change(){
        var tid = $(this).attr("id");
        var payer=tid.replace("ASUR_TYPE_CODE","ASUR_PAYER");
        var onus=tid.replace("ASUR_TYPE_CODE","ONUS");
        
        if (tid.indexOf("ctl00")!=-1){
                var factor=document.getElementById("<%=this.dpFACTOR.ClientID %>").options[document.getElementById(tid).selectedIndex].text;
              //  alert(factor);
                $('input[id*=ISU_FACTOR]').val(factor);
        }
        
        if (document.getElementById(payer).options[document.getElementById(tid).selectedIndex].text!=""){
            document.getElementById(onus).selectedIndex=document.getElementById(payer).options[document.getElementById(tid).selectedIndex].text ;            
        }
        else{
        document.getElementById(onus).selectedIndex=0;
        }
    }
    
     function setRETK()
    {
        document.getElementById("<%=this.btnRETK.ClientID %>").click();
        
    } 
    function setAgentData()
    {
        //document.getElementById("<%=this.btnMobile.ClientID %>").click();
        setMobile();
        
        
    }
    function setMobile()
    {
        //SELECT MOBILE FROM OR3_FRC_SALES WHERE FRC_CODE=@FRC_CODE AND FRC_SALES_NAME=@NAME rptObjMain_ctl00_OTHER
        
         var FRC_CODE = $('input[id*=FRC_CODE_txtDialog]').val();
   
         var FRC_SALES_NAME=$('input[id*=AGENT]').val();
         if ( FRC_SALES_NAME=="")
            {
             //$('input[id*=rptObjMain_ctl00_OTHER]').val("");
             return;
            }
          var name=encodeURI(FRC_SALES_NAME,"UTF-8");
         $.getJSON("DialogService.ashx?SourceTable=OR3_FRC_SALES&Item="+FRC_CODE+","+name, function(json){
        
    
           
                if (json.rows.length>0)
                    $('input[id*=rptObjMain_ctl00_OTHER]').val(json.rows[0].KEY_NO);
                else
                   $('input[id*=rptObjMain_ctl00_OTHER]').val("");   
            });
    }
    // 定額稅金 < (定額租金*0.05 四捨五入至整數) 
    function APLY_HIRE_Change(){
      
        if ($('select[id*=APLY_TAX_ZERO]').val()=="1"){
         $('input[id*=APLY_TAX]').val(parseMoney(Math.round(parseInt($('input[id*=APLY_HIRE]').val().replace(/,/g, ""))*<%=this.tRate %>))); 
          $('input[id*=hidden_APLY_TAX]').val(Math.round(parseInt($('input[id*=APLY_HIRE]').val().replace(/,/g, ""))*<%= this.tRate %>));         
         $('input[id*=APLY_TAX]').prop('disabled', false); 
         }else{
            $('input[id*=APLY_TAX]').val(0);
             $('input[id*=APLY_TAX]').prop('disabled', "disabled");
         }
    }
     function APRV_HIRE_Change(){
     if ($('select[id*=APRV_TAX_ZERO]').val()=="1"){
         $('input[id*=APRV_TAX]').val(parseMoney(Math.round(parseInt($('input[id*=APRV_HIRE]').val().replace(/,/g, ""))*<%= this.tRate %>))); 
          $('input[id*=hidden_APRV_TAX]').val(Math.round(parseInt($('input[id*=APRV_HIRE]').val().replace(/,/g, ""))*<%= this.tRate %>)); 
         $('input[id*=APRV_TAX]').prop('disabled', false);        
         }else{
            $('input[id*=APRV_TAX]').val(0);
            $('input[id*=APRV_TAX]').prop('disabled', "disabled");
         }
    }
    
    function APRV_BUY_AMT_Change(){
       
         $('input[id*=APRV_PURS_TAX]').val(parseMoney(Math.round(parseInt($(this).val().replace(/,/g, ""))*<%=this.tRate %>)));
    }
     function APLY_BUY_AMT_Change(){
     
         $('input[id*=APLY_PURS_TAX]').val(parseMoney(Math.round(parseInt($(this).val().replace(/,/g, ""))*<%=this.tRate %>)));
    }
    
    
    function SIGNED_Change()
    {
         var tid = $(this).attr("id");
         var obj=document.getElementById(tid);
         $.getJSON("DialogService.ashx?SourceTable=otc_proced_content&Item="+obj.value, function(json){
        
    
           
                if (json.rows.length>0)
                    obj.parentNode.parentNode.cells[1].innerHTML=json.rows[0].KEY_NO;
                else
                    obj.parentNode.parentNode.cells[1].innerHTML="";    
            });

          
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

    function APRV_Change() {

        var tid = $(this).attr("id");
        for (var i = 1; i <= 3; i++) {
            if (tid.indexOf(i.toString() + "_TO") != -1)
                tid = tid = tid.replace(i.toString() + "_TO", (i + 1).toString() + "_FR");
        }

        var period = parseInt($('input[id*=APRV_PERD]').val());
        var periodc = parseInt($(this).val());
        if (periodc > period) {
            window.parent.errorMessage('錯誤訊息', '不得大於期數，請重新輸入！');
            document.getElementById($(this).attr("id")).value = "0";
            document.getElementById($(this).attr("id")).focus();
        }

        if (parseInt($(this).val()) != 0 && periodc < period)
            document.getElementById(tid).value = parseInt($(this).val()) + 1;

        if (periodc == 0)
            document.getElementById(tid).value = "0";
    }
    
    function changeMTHD(stype,obj)
    {
   
        var val=obj.value;
       
        document.getElementById("MTHD"+stype.toString()+"_1").style.display="none";
        document.getElementById("MTHD"+stype.toString()+"_2").style.display="none";
        
        if (val=="1")
            document.getElementById("MTHD"+stype.toString()+"_1").style.display="";

        if (val == "2") {
            document.getElementById('<%=this.rptRequest.Items[0].FindControl("APLY_LF1_FR").ClientID %>').value = "1";  
            document.getElementById("MTHD" + stype.toString() + "_2").style.display = "";
        }      
                
        if (val == "5")
            document.getElementById("TR"+stype).style.display=""        
        else
            document.getElementById("TR" + stype).style.display = "none"        
    }


    
    function openDetail(strDate, strType, strName) {

    
        window.parent.openPopUpWindow();
    }
  
    </script>

</asp:Content>
