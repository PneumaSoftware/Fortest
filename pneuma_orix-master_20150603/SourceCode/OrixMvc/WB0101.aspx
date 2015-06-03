<%@ Page Language="C#" EnableEventValidation="false" AutoEventWireup="true" MasterPageFile="~/Pattern/editing.Master"
    CodeBehind="WB0101.aspx.cs" Inherits="OrixMvc.WB0101" %>

<%@ MasterType VirtualPath="~/Pattern/editing.Master" %>
<%@ Register TagName="ocxDate" TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxYM" TagPrefix="ocxControl" Src="~/ocxControl/ocxYM.ascx" %>
<%@ Register TagName="ocxYear" TagPrefix="ocxControl" Src="~/ocxControl/ocxYear.ascx" %>
<%@ Register TagName="ocxDialog" TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxTime" TagPrefix="ocxControl" Src="~/ocxControl/ocxTime.ascx" %>
<%@ Register TagName="ocxNumber" TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload" TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ContentPlaceHolderID="dataArea" runat="server" ID="myDataArea">
    <table style="">
        <tr>
            <th>
                申請書編號：
            </th>
            <td>
                <asp:TextBox runat="server" ID="QUOTA_APLY_NO" CssClass="display" Width="150"></asp:TextBox>
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
    <asp:SqlDataSource EnableViewState="true" SelectCommandType="Text" ID="sqlAPP_TYPE"
        ConnectionString="<%$ ConnectionStrings:myConnectionString%>" SelectCommand="Exec s_ConditionItems 'APPR_TYPE'"
        runat="server"></asp:SqlDataSource>
    <asp:SqlDataSource EnableViewState="true" SelectCommandType="Text" ID="sqlTAX_TYPE"
        ConnectionString="<%$ ConnectionStrings:myConnectionString%>" SelectCommand="Exec s_ConditionItems 'TAX_TYPE'"
        runat="server"></asp:SqlDataSource>
    <asp:SqlDataSource EnableViewState="true" SelectCommandType="Text" ID="sqlAUD_LVL_CODE"
        ConnectionString="<%$ ConnectionStrings:myConnectionString%>" SelectCommand="select '' as TypeCode,'' as TypeDesc union all SELECT AUD_LVL_CODE as TypeCode,AUD_LVL_NAME as TypeDesc  FROM OR_AUD_LVL_NAME order by 1"
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
    
    if (intIndex==3)
        document.getElementById("<%=this.btnTab3.ClientID %>").click();
      
    if (intIndex==5)
        document.getElementById("<%=this.btnTab5.ClientID %>").click();

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
        document.getElementById("<%=this.QUOTA_APLY_NO.ClientID %>").value = AplyNo;
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
                    <li class="tabs-selected" id="tab1" onclick="tab_select(1,10)"><a href="javascript:void(0)"
                        class="tabs-inner"><span class="tabs-title">基本資料</span><span class="tabs-icon"></span></a></li>
                    <li class="" id="tab2" onclick="tab_select(2,10)"><a href="javascript:void(0)" class="tabs-inner">
                        <span class="tabs-title">型態標的物</span><span class="tabs-icon"></span></a></li>
                    <li class="" id="tab3" onclick="tab_select(3,10)"><a href="javascript:void(0)" class="tabs-inner">
                        <span class="tabs-title">申請條件</span><span class="tabs-icon"></span></a></li>
                    <li class="" id="tab10" onclick="tab_select(10,10)"><a href="javascript:void(0)"
                        class="tabs-inner"><span class="tabs-title">額度使用者</span><span class="tabs-icon"></span></a></li>
                    <li class="" id="tab4" onclick="tab_select(4,10)"><a href="javascript:void(0)" class="tabs-inner">
                        <span class="tabs-title">擔保條件</span><span class="tabs-icon"></span></a></li>
                    <li class="" id="tab5" onclick="tab_select(5,10)"><a href="javascript:void(0)" class="tabs-inner">
                        <span class="tabs-title">核准條件</span><span class="tabs-icon"></span></a></li>
                    <li class="" id="tab6" onclick="tab_select(6,10)"><a href="javascript:void(0)" class="tabs-inner">
                        <span class="tabs-title">往來實績</span><span class="tabs-icon"></span></a></li>
                    <li class="" id="tab7" onclick="tab_select(7,10)"><a href="javascript:void(0)" class="tabs-inner">
                        <span class="tabs-title">權限條件</span><span class="tabs-icon"></span></a></li>
                    <li class="" id="tab8" onclick="tab_select(8,10)"><a href="javascript:void(0)" class="tabs-inner">
                        <span class="tabs-title">授信權限</span><span class="tabs-icon"></span></a></li>
                    <li class="" id="tab9" onclick="tab_select(9,10)"><a href="javascript:void(0)" class="tabs-inner">
                        <span class="tabs-title">審查</span><span class="tabs-icon"></span></a></li>
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
                                    <td>
                                        <font class="memo">
                                            <%# Eval("PROJECT") %></font>
                                    </td>
                                <th>
                                    初次接觸日：
                                </th>
                                <td>
                                    <ocxControl:ocxDate runat="server" ID="InitContactDate" Text='<%# Eval("InitContactDate").ToString().Trim() %>' />
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
                                <td colspan="2">
                                    <asp:TextBox runat="server" ID="CTAC_TEL" Width="110" size="10" MaxLength="12" Text='<%# Eval("CTAC_TEL").ToString().Trim() %>'></asp:TextBox>
                                </td>
                                <th>
                                    核准日：
                                </th>
                                <td>
                                    <input type="text" class="display" style="width: 70px" value='<%# Eval("APRV_DATE").ToString().Trim() %>'
                                        id="APRV_DATE" />
                                </td>
                                <th>
                                    到期日：
                                </th>
                                <td>
                                    <input type="text" class="display" style="width: 70px" value='<%# Eval("DUE_DATE").ToString().Trim() %>'
                                        id="DUE_DATE" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    業務推廣過程：<asp:TextBox runat="server" ID="BIZ_PROMO_PROC" Width="240" size="10" MaxLength="100"
                                        Text='<%# Eval("BIZ_PROMO_PROC").ToString().Trim() %>'></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" style="vertical-align: top">
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
                <table style="width: 810px; margin: 0px">
                    <tr>
                        <th class="nonSpace">
                            案件類別：
                        </th>
                        <td colspan="8">
                            <asp:Repeater runat="server" ID="rptCASETYPE">
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="CASE_TYPE_CODE" Checked='<%# Eval("Q_CASE_STS") %>'
                                        CssClass='<%# Eval("CASE_STS") %>' Text='<%# Eval("CASE_DESC") %>' />
                                </ItemTemplate>
                            </asp:Repeater>
                        </td>
                    </tr>
                    <asp:Repeater runat="server" ID="rptObjMain">
                        <ItemTemplate>
                            <tr>
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
                                <th>
                                    案件來源：
                                </th>
                                <td>
                                    <asp:DropDownList runat="server" ID="CASE_SOUR" OnPreRender='checkList' ToolTip='<%# Eval("CASE_SOUR").ToString().Trim() %>'
                                        Width="80" DataSourceID="sqlCASE_SOUR" DataValueField="TypeCode" DataTextField="TypeDesc">
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
                                <td>
                                    <asp:TextBox runat="server" ID="OTHER" Width="170" MaxLength="20" Text='<%# Eval("OTHER").ToString().Trim() %>'></asp:TextBox>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
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
                                                        標的物代號：
                                                    </th>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="OBJ_CODE" Width="90" CssClass="display" MaxLength="20"
                                                            Text='<%# Eval("OBJ_CODE").ToString().Trim() %>'></asp:TextBox>
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
                                                    <th>
                                                        市價：
                                                    </th>
                                                    <td>
                                                        <ocxControl:ocxNumber runat="server" ID="REAL_BUY_PRC" MASK="9,999,999" Text='<%# Eval("REAL_BUY_PRC").ToString().Trim() %>' />
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
                                                        <ocxControl:ocxNumber runat="server" ID="BUDG_LEASE_AMT" MASK="9,999,999" Text='<%# Eval("BUDG_LEASE_AMT").ToString().Trim() %>' />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        進項發票金額：
                                                    </th>
                                                    <td>
                                                        <ocxControl:ocxNumber runat="server" ID="INV_AMT_I_IB" MASK="9,999,999" Text='<%# Eval("INV_AMT_I_IB").ToString().Trim() %>' />
                                                    </td>
                                                    <th>
                                                        原殘值：
                                                    </th>
                                                    <td>
                                                        <ocxControl:ocxNumber runat="server" ID="RV_AMT" MASK="9,999,999" Text='<%# Eval("RV_AMT").ToString().Trim() %>' />
                                                    </td>
                                                    <th>
                                                        自備率：
                                                    </th>
                                                    <td>
                                                        <ocxControl:ocxNumber runat="server" ID="SELF_RATE" MASK="99.9999" Text='<%# Eval("SELF_RATE").ToString().Trim() %>' />
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
                                                    <th>
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
                                                    <th>
                                                        買回比率：
                                                    </th>
                                                    <td>
                                                        <ocxControl:ocxNumber runat="server" ID="BUY_RATE" MASK="999.9999" Text='<%# Eval("BUY_RATE").ToString().Trim() %>' />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
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
                                                            Width="90" DataSourceID="sqlONUS" DataValueField="TypeCode" DataTextField="TypeDesc">
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
                                <th class="nonSpace">
                                    總額度：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber runat="server" ID="APLY_TOT_QUOTA" MASK="9,999,999" Text='<%# Eval("APLY_TOT_QUOTA").ToString().Trim() %>' />
                                </td>
                                <th class="nonSpace">
                                    動撥型態：
                                </th>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upAPPRTYPE" RenderMode="Inline" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:DropDownList runat="server" ID="APLY_APPR_TYPE" AutoPostBack="true" OnSelectedIndexChanged='APPType_Change'
                                                OnPreRender='checkList' ToolTip='<%# Eval("APLY_APPR_TYPE").ToString().Trim() %>'
                                                Width="80" DataSourceID="sqlAPP_TYPE" DataValueField="TypeCode" DataTextField="TypeDesc">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <th>
                                    預計首筆動撥日：
                                </th>
                                <td>
                                    <ocxControl:ocxDate runat="server" ID="APLY_FIRST_APPR_DATE" Text='<%# Eval("APLY_FIRST_APPR_DATE").ToString().Trim() %>' />
                                </td>
                            </tr>
                            <tr>
                                <th class="nonSpace">
                                    專案額度期間(月)：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber runat="server" ID="APLY_QUOTA_MONTHS" MASK="999" Text='<%# Eval("APLY_QUOTA_MONTHS").ToString().Trim() %>' />
                                </td>
                                <th class="nonSpace">
                                    逐筆動撥期間(最長月數)：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber runat="server" ID="APLY_DURN_M" MASK="999" Text='<%# Eval("APLY_DURN_M").ToString().Trim() %>' />
                                </td>
                                <th class="nonSpace">
                                    最長付款週期(月)：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber runat="server" ID="APLY_PAY_PERD" MASK="999" Text='<%# Eval("APLY_PAY_PERD").ToString().Trim() %>' />
                                </td>
                                <td>
                                    <asp:CheckBox runat="server" ID="APLY_B_RECEIVE" OnPreRender='checkList' ToolTip='<%# Eval("APLY_B_RECEIVE").ToString().Trim() %>'
                                        Text="限前收" />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    表面TR：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber runat="server" ID="APLY_SURF_TR" MASK=",999,999" Text='<%# Eval("APLY_SURF_TR").ToString().Trim() %>' />
                                </td>
                                <th class="nonSpace">
                                    實質TR：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber runat="server" ID="APLY_REAL_TR" MASK=",999,999" Text='<%# Eval("APLY_REAL_TR").ToString().Trim() %>' />
                                </td>
                                <th>
                                    手續費：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber runat="server" ID="APLY_SERV_CHAR" MASK=",999,999" Text='<%# Eval("APLY_SERV_CHAR").ToString().Trim() %>' />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    保險因子(%)：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber runat="server" ID="ISU_FACTOR" MASK="99,999" Text='<%# Eval("ISU_FACTOR").ToString().Trim() %>' />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    其他申請條件：
                                </th>
                                <td colspan="6">
                                    <asp:TextBox runat="server" ID="APLY_OTH_APLY_COND" TextMode="MultiLine" Rows="5"
                                        Width="350" Text='<%# Eval("APLY_OTH_APLY_COND").ToString().Trim() %>'></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    資金用途：
                                </th>
                                <td colspan="6">
                                    <asp:TextBox runat="server" ID="APLY_FINA_PURP" Width="150" Text='<%# Eval("APLY_FINA_PURP").ToString().Trim() %>'></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:Repeater>
                <table cellspacing="1" cellpadding="1" border="0">
                    <tr>
                        <td>
                            <fieldset>
                                <legend>包含前案</legend>
                                <asp:UpdatePanel runat="server" ID="upFORMER" RenderMode="Inline" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="gridMain " style="width: 425px; margin: 0">
                                            <div style="padding: 0; margin: 0; position: relative; overflow-y: scroll; width: 415px;
                                                height: 100px" id="editGrid">
                                                <table cellpadding="0" cellspacing="0" style="width: 360px">
                                                    <thead>
                                                        <tr>
                                                            <th>
                                                                選擇
                                                            </th>
                                                            <th>
                                                                前案額度申請書
                                                            </th>
                                                            <th>
                                                                總額度
                                                            </th>
                                                        </tr>
                                                    </thead>
                                                    <asp:Repeater runat="server" ID="rptFORMER">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox runat="server" ID="chkSelect" OnPreRender='checkList' ToolTip='<%# Eval("chkSelect").ToString().Trim() %>' />
                                                                </td>
                                                                <td>
                                                                    <asp:HiddenField ID="PREV_APLY_NO" runat="server" Value='<%# Eval("PREV_APLY_NO")%>' />
                                                                    <%# Eval("PREV_APLY_NO")%>
                                                                </td>
                                                                <td>
                                                                    <asp:HiddenField ID="USED_QUOTA" runat="server" Value='<%# Eval("APRV_TOT_QUOTA")%>' />
                                                                    <%# Eval("APRV_TOT_QUOTA", "{0:###,###,###,##0}")%>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </table>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </fieldset>
                        </td>
                        <td>
                            <fieldset>
                                <legend>額度內動撥案件審核層級</legend>
                                <div class="gridMain " style="width: 425px; margin: 0">
                                    <div style="padding: 0; margin: 0; position: relative; overflow-y: scroll; width: 400px;
                                        height: 100px" id="editGrid">
                                        <table cellpadding="0" cellspacing="0" style="width: 360px">
                                            <thead>
                                                <tr>
                                                    <th>
                                                        審核層級
                                                    </th>
                                                    <th>
                                                        TR
                                                    </th>
                                                    <th>
                                                        期間
                                                    </th>
                                                </tr>
                                            </thead>
                                            <asp:Repeater runat="server" ID="rptAPRV">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <asp:DropDownList ID="AUD_LVL_CODE" Style="width: 120px" runat="server" OnPreRender='checkList'
                                                                ToolTip='<%# Eval("AUD_LVL_CODE").ToString().Trim() %>' DataSourceID="sqlAUD_LVL_CODE"
                                                                DataValueField="TypeCode" DataTextField="TypeDesc">
                                                            </asp:DropDownList>
                                                        </td>
                                                        </td>
                                                        <td>
                                                            <ocxControl:ocxNumber runat="server" ID="B_TR" MASK="99,999" Text='<%# Eval("B_TR").ToString().Trim() %>' />
                                                        </td>
                                                        <td>
                                                            <ocxControl:ocxNumber runat="server" ID="DURN" MASK="99,999" Text='<%# Eval("DURN").ToString().Trim() %>' />
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
            </div>
            <div class="panel" style="height: auto; width: 810px; display: none" id="tabpanel10">
                <div class="gridMain " style="width: 790px">
                    <div style="padding: 0; margin: 0; overflow-y: scroll; position: relative; width: 790px;
                        height: 240px" id="editGrid">
                        <table cellpadding="0" cellspacing="0" style="width: 780px">
                            <thead>
                                <tr>
                                    <th>
                                        客戶代號
                                    </th>
                                    <th>
                                        客戶簡稱
                                    </th>
                                    <th>
                                        實收資本額
                                    </th>
                                    <th>
                                        母公司
                                    </th>
                                    <th>
                                        母公司持股比例
                                    </th>
                                    <th>
                                        母公司投資年
                                    </th>
                                    <th>
                                        母公司投資損益
                                    </th>
                                    <th>
                                        前專案額度契約餘額
                                    </th>
                                    <th>
                                        動用額度上限
                                    </th>
                                </tr>
                            </thead>
                            <tr>
                                <td>
                                    <input id="USER_CUST_NO" type="text" style="width: 100px" readonly="readonly" class="display" />
                                </td>
                                <td>
                                    <input id="USER_CUST_NAME" type="text" style="width: 180px" readonly="readonly" class="display" />
                                </td>
                                <td>
                                    <input id="REAL_CAPT_AMT" type="text" style="width: 100px" readonly="readonly" class="display number " />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="PARENT_COMP" MaxLength="20" Width="100" Text='<%# Eval("PARENT_COMP").ToString().Trim() %>'></asp:TextBox>
                                </td>
                                <td>
                                    <ocxControl:ocxNumber runat="server" ID="PARENT_COMP_SHAREHOLDING" MASK="99.9999"
                                        Text='<%# Eval("PARENT_COMP_SHAREHOLDING").ToString().Trim() %>' />
                                </td>
                                <td>
                                    <ocxControl:ocxYear runat="server" ID="PARENT_COMP_INVEST_YEAR" Text='<%# Eval("PARENT_COMP_INVEST_YEAR").ToString().Trim() %>' />
                                </td>
                                <td>
                                    <ocxControl:ocxNumber runat="server" ID="PARENT_COMP_INVEST_INCOME" MASK="999,999,999"
                                        Text='<%# Eval("PARENT_COMP_INVEST_INCOME").ToString().Trim() %>' />
                                </td>
                                <td>
                                    <ocxControl:ocxNumber runat="server" ID="PREV_REAL_VAL" MASK="999,999,999" Text='<%# Eval("PREV_REAL_VAL").ToString().Trim() %>' />
                                </td>
                                <td>
                                    <ocxControl:ocxNumber runat="server" ID="UPPER_LIMIT" MASK="999,999,999" Text='<%# Eval("UPPER_LIMIT").ToString().Trim() %>' />
                                </td>
                            </tr>
                            <asp:Repeater runat="server" ID="rptUSER">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <ocxControl:ocxDialog runat="server" ID="CUST_NO" width="80" Text='<%# Eval("CUST_NO").ToString().Trim() %>'
                                                FieldName="CUST_SNAME,REAL_CAPT_AMT" SourceName="OR_CUSTOM" />
                                        </td>
                                        <td>
                                            <asp:TextBox CssClass="display" ReadOnly="true" runat="server" ID="CUST_SNAME" MaxLength="20"
                                                Width="100" Text='<%# Eval("CUST_SNAME").ToString().Trim() %>'></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox CssClass="display number " ReadOnly="true" runat="server" ID="REAL_CAPT_AMT"
                                                MaxLength="20" Width="100" Text='<%# Eval("REAL_CAPT_AMT", "{0:###,###,###,##0}")%>'></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="PARENT_COMP" MaxLength="20" Width="100" Text='<%# Eval("PARENT_COMP").ToString().Trim() %>'></asp:TextBox>
                                        </td>
                                        <td>
                                            <ocxControl:ocxNumber runat="server" ID="PARENT_COMP_SHAREHOLDING" MASK="99.9999"
                                                Text='<%# Eval("PARENT_COMP_SHAREHOLDING").ToString().Trim() %>' />
                                        </td>
                                        <td>
                                            <ocxControl:ocxYear runat="server" ID="PARENT_COMP_INVEST_YEAR" Text='<%# Eval("PARENT_COMP_INVEST_YEAR").ToString().Trim() %>' />
                                        </td>
                                        <td>
                                            <ocxControl:ocxNumber runat="server" ID="PARENT_COMP_INVEST_INCOME" MASK="999,999,999"
                                                Text='<%# Eval("PARENT_COMP_INVEST_INCOME").ToString().Trim() %>' />
                                        </td>
                                        <td>
                                            <ocxControl:ocxNumber runat="server" ID="PREV_REAL_VAL" MASK="999,999,999" Text='<%# Eval("PREV_REAL_VAL").ToString().Trim() %>' />
                                        </td>
                                        <td>
                                            <ocxControl:ocxNumber runat="server" ID="UPPER_LIMIT" MASK="999,999,999" Text='<%# Eval("UPPER_LIMIT").ToString().Trim() %>' />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            <tr>
                                <th colspan="6">
                                </th>
                            </tr>
                        </table>
                    </div>
                </div>
                <br>
                <table cellpadding=5 cellspacing=5><tr><td>
                <asp:UpdatePanel runat="server" ID="upPREV_REAL_VAL" ChildrenAsTriggers="true">
                    <ContentTemplate>                        
                            <div class="divFunction" >
                                <asp:Button runat="server" ID="btnPREV_REAL_VAL" CssClass="button trn" Text="計算前額度餘額"
                                    OnCommand="GridFunc_Click" CommandName="PREV_REAL_VAL" />
                            </div>
                                         
                    </ContentTemplate>
                </asp:UpdatePanel>
                </td></tr></table>
                <br />
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
                                <div class="gridMain " style="width: 775px; margin: 0">
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
                                                    <ocxControl:ocxNumber runat="server" ID="RV_AMT" MASK="9,999,999" />
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
                                                    <ocxControl:ocxNumber runat="server" ID="ASUR_AMOUNT" MASK="999,999" />
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
                                                            <ocxControl:ocxNumber runat="server" ID="RV_AMT" MASK="9,999,999" Text='<%# Eval("RV_AMT").ToString().Trim() %>' />
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
                                                            <ocxControl:ocxNumber runat="server" ID="ASUR_AMOUNT" MASK="999,999" Text='<%# Eval("ASUR_AMOUNT").ToString().Trim() %>' />
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
                                                            <ocxControl:ocxNumber runat="server" ID="SET_AMT" MASK="99,999" Text='<%# Eval("SET_AMT").ToString().Trim() %>' />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="ORDER1" MaxLength="20" Width="100" Text='<%# Eval("ORDER1").ToString().Trim() %>'></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <ocxControl:ocxNumber runat="server" ID="ORDER1_AMT" MASK="99,999" Text='<%# Eval("ORDER1_AMT").ToString().Trim() %>' />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="ORDER2" MaxLength="20" Width="100" Text='<%# Eval("ORDER2").ToString().Trim() %>'></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <ocxControl:ocxNumber runat="server" ID="ORDER2_AMT" MASK="99,999" Text='<%# Eval("ORDER2_AMT").ToString().Trim() %>' />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="ORDER3" MaxLength="20" Width="100" Text='<%# Eval("ORDER3").ToString().Trim() %>'></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <ocxControl:ocxNumber runat="server" ID="ORDER3_AMT" MASK="99,999" Text='<%# Eval("ORDER3_AMT").ToString().Trim() %>' />
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
            </div>
            <div class="panel" style="height: auto; width: 820px; display: none" id="tabpanel5">
                <asp:Repeater runat="server" ID="rptRun">
                    <ItemTemplate>
                        <table cellspacing="1" cellpadding="1" border="0">
                            <tr>
                                <th class="nonSpace">
                                    總額度：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber runat="server" ID="APRV_TOT_QUOTA" MASK="9,999,999" Text='<%# Eval("APRV_TOT_QUOTA").ToString().Trim() %>' />
                                </td>
                                <th class="nonSpace">
                                    動撥型態：
                                </th>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upAPPRTYPE" RenderMode="Inline" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:DropDownList runat="server" ID="APRV_APPR_TYPE" AutoPostBack="true" OnSelectedIndexChanged='APPType_Change'
                                                OnPreRender='checkList' ToolTip='<%# Eval("APRV_APPR_TYPE").ToString().Trim() %>'
                                                Width="80" DataSourceID="sqlAPP_TYPE" DataValueField="TypeCode" DataTextField="TypeDesc">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <th>
                                    預計首筆動撥日：
                                </th>
                                <td>
                                    <ocxControl:ocxDate runat="server" ID="APRV_FIRST_APPR_DATE" Text='<%# Eval("APRV_FIRST_APPR_DATE").ToString().Trim() %>' />
                                </td>
                            </tr>
                            <tr>
                                <th class="nonSpace">
                                    專案額度期間(月)：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber runat="server" ID="APRV_QUOTA_MONTHS" MASK="999" Text='<%# Eval("APRV_QUOTA_MONTHS").ToString().Trim() %>' />
                                </td>
                                <th class="nonSpace">
                                    逐筆動撥期間(最長月數)：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber runat="server" ID="APRV_DURN_M" MASK="999" Text='<%# Eval("APRV_DURN_M").ToString().Trim() %>' />
                                </td>
                                <th class="nonSpace">
                                    最長付款週期(月)：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber runat="server" ID="APRV_PAY_PERD" MASK="999" Text='<%# Eval("APRV_PAY_PERD").ToString().Trim() %>' />
                                </td>
                                <td>
                                    <asp:CheckBox runat="server" ID="APRV_B_RECEIVE" OnPreRender='checkList' ToolTip='<%# Eval("APRV_B_RECEIVE").ToString().Trim() %>'
                                        Text="限前收" />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    表面TR：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber runat="server" ID="APRV_SURF_TR" MASK=",999,999" Text='<%# Eval("APRV_SURF_TR").ToString().Trim() %>' />
                                </td>
                                <th class="nonSpace">
                                    實質TR：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber runat="server" ID="APRV_REAL_TR" MASK=",999,999" Text='<%# Eval("APRV_REAL_TR").ToString().Trim() %>' />
                                </td>
                                <th>
                                    手續費：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber runat="server" ID="APRV_SERV_CHAR" MASK=",999,999" Text='<%# Eval("APRV_SERV_CHAR").ToString().Trim() %>' />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    保險因子(%)：
                                </th>
                                <td>
                                    <ocxControl:ocxNumber runat="server" ID="A_ISU_FACTOR" MASK="99,999" Text='<%# Eval("A_ISU_FACTOR").ToString().Trim() %>' />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    其他申請條件：
                                </th>
                                <td colspan="6">
                                    <asp:TextBox runat="server" ID="APRV_OTH_APLY_COND" TextMode="MultiLine" Rows="5"
                                        Width="350" Text='<%# Eval("APRV_OTH_APLY_COND").ToString().Trim() %>'></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    資金用途：
                                </th>
                                <td colspan="6">
                                    <asp:TextBox runat="server" ID="APRV_FINA_PURP" Width="150" Text='<%# Eval("APRV_FINA_PURP").ToString().Trim() %>'></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:Repeater>
                <table cellspacing="1" cellpadding="1" border="0">
                    <tr>
                        <td>
                            <fieldset>
                                <legend>包含前案</legend>
                                <asp:UpdatePanel runat="server" ID="upFORMER1" RenderMode="Inline" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="gridMain " style="width: 425px; margin: 0">
                                            <div style="padding: 0; margin: 0; position: relative; overflow-y: scroll; width: 415px;
                                                height: 100px" id="editGrid">
                                                <table cellpadding="0" cellspacing="0" style="width: 360px">
                                                    <thead>
                                                        <tr>
                                                            <th>
                                                                選擇
                                                            </th>
                                                            <th>
                                                                前案額度申請書
                                                            </th>
                                                            <th>
                                                                總額度
                                                            </th>
                                                        </tr>
                                                    </thead>
                                                    <asp:Repeater runat="server" ID="rptFORMER1">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox runat="server" ID="chkSelect" OnPreRender='checkList' ToolTip='<%# Eval("chkSelect").ToString().Trim() %>' />
                                                                </td>
                                                                <td>
                                                                    <%# Eval("PREV_APLY_NO")%>
                                                                </td>
                                                                <td>
                                                                    <%# Eval("APRV_TOT_QUOTA", "{0:###,###,###,##0}")%>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </table>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </fieldset>
                        </td>
                        <td>
                            <fieldset>
                                <legend>額度內動撥案件審核層級</legend>
                                <div class="gridMain " style="width: 425px; margin: 0">
                                    <div style="padding: 0; margin: 0; position: relative; overflow-y: scroll; width: 415px;
                                        height: 100px" id="editGrid">
                                        <table cellpadding="0" cellspacing="0" style="width: 360px">
                                            <thead>
                                                <tr>
                                                    <th>
                                                        審核層級
                                                    </th>
                                                    <th>
                                                        TR
                                                    </th>
                                                    <th>
                                                        期間
                                                    </th>
                                                </tr>
                                            </thead>
                                            <asp:Repeater runat="server" ID="rptAPRV1">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <asp:DropDownList ID="AUD_LVL_CODE" Style="width: 120px" runat="server" OnPreRender='checkList'
                                                                ToolTip='<%# Eval("AUD_LVL_CODE").ToString().Trim() %>' DataSourceID="sqlAUD_LVL_CODE"
                                                                DataValueField="TypeCode" DataTextField="TypeDesc">
                                                            </asp:DropDownList>
                                                        </td>
                                                        </td>
                                                        <td>
                                                            <ocxControl:ocxNumber runat="server" ID="B_TR" MASK="99,999" Text='<%# Eval("B_TR").ToString().Trim() %>' />
                                                        </td>
                                                        <td>
                                                            <ocxControl:ocxNumber runat="server" ID="DURN" MASK="99,999" Text='<%# Eval("DURN").ToString().Trim() %>' />
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
                                            <div style="width: 100px" class="text number">
                                                <%# Eval("L_APRV_IPREV_QUOTA_CYCLE", "{0:###,###,###,##0}")%></div>
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
                                            <div style="width: 100px" class="text number">
                                                <%# Eval("L_APRV_IPREV_QUOTA_NCYCLE", "{0:###,###,###,##0}")%></div>
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
                                            <div style="width: 100px" class="text number">
                                                <%# Eval("ALL_RISKS", "{0:###,###,###,##0}")%></div>
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
                                                    <asp:TextBox runat="server" ID="txtOpin" Width="410" TextMode="MultiLine" Rows="11"
                                                        onmouseout='aud_mouseout()'></asp:TextBox>
                                                </div>
                                            </th>
                                        </tr>
                                    </thead>
                                    <asp:Repeater runat="server" ID="rptOpin">
                                        <ItemTemplate>
                                            <tr style="cursor: pointer" onmouseover='aud_mouseover(this,<%# Container.ItemIndex+1 %>)'
                                                onmouseout='aud_mouseout()'>
                                                <td style="width: 180px;">
                                                    <asp:HiddenField runat="server" ID="AUD_LVL_CODE" Value='<%# Eval("AUD_LVL_CODE").ToString().Trim() %>' />
                                                    <asp:HiddenField runat="server" ID="LVL" Value='<%# Eval("LVL").ToString().Trim() %>' />
                                                    <%# Eval("AUD_LVL_NAME").ToString().Trim() %>
                                                </td>
                                                <td style="width: 120px">
                                                    <asp:DropDownList ID="IF_PASS" Style="width: 120px" Enabled='<%# Eval("ENABLED").ToString().Trim()=="1"?true:false %>'
                                                        runat="server" OnPreRender='checkList' ToolTip='<%# Eval("IF_PASS").ToString().Trim() %>'
                                                        DataSourceID="sqlIF_PASS" DataValueField="TypeCode" DataTextField="TypeDesc">
                                                    </asp:DropDownList>
                                                    <div style="z-index: 101; display: none" id="opin<%# Container.ItemIndex+1 %>" class='<%# Eval("ENABLED").ToString().Trim()=="1"?"enabled":"disabled" %>'>
                                                        <asp:TextBox runat="server" ID="AUD_OPIN" Width="450" TextMode="MultiLine" Rows="11"
                                                            Text='<%# Eval("AUD_OPIN").ToString().Trim() %>'></asp:TextBox></div>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <asp:UpdatePanel ID="upCustom" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <span style="display: none">
                <asp:Button runat="server" ID="reloadCustom" OnClick="Reload_Custom" />
                <asp:Button runat="server" ID="btnMobile" OnCommand="GridFunc_Click" CommandName="Agent" />
                <asp:Button runat="server" ID="btnTab3" OnCommand="GridFunc_Click" CommandName="Tab3" />
                <asp:Button runat="server" ID="btnTab5" OnCommand="GridFunc_Click" CommandName="Tab5" />
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



   function init() {
     
         $('select[id*=rptASUR][id*=ASUR_TYPE_CODE]').change(ASUR_TYPE_CODE_Change);
          $('input[id*=AGENT]').change(setAgentData);
       $('input[id*=REAL_BUY_PRC]').change(REAL_BUY_PRC_Change);
        
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
    
   function REAL_BUY_PRC_Change()
   {
          $('input[id*=BUDG_LEASE_AMT]').val($(this).val());
          $('input[id*=INV_AMT_I_IB]').val($(this).val());
   }
   
   
function setCustom(CUST_NO,CUST_NAME,REAL_CAPT_AMT) {

        document.getElementById('USER_CUST_NO').value = CUST_NO;
        document.getElementById('USER_CUST_NAME').value = CUST_NAME;
        document.getElementById('REAL_CAPT_AMT').value = REAL_CAPT_AMT;
       
    }
    
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

        if (sid.indexOf("AUD_CASE_TYPE") != -1) {

            if (val != "" && val != oldType) {
                document.getElementById("<%=this.txtCaseType.ClientID %>").value = val;
                document.getElementById("<%=this.btnReload.ClientID %>").click();
            }

            oldType = val;
        }
    }

    
    
function setAgentData()
    {
       // document.getElementById("<%=this.btnMobile.ClientID %>").click();
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
         $.getJSON("DialogService.ashx?SourceTable=OR3_FRC_SALES&Item="+FRC_CODE+","+FRC_SALES_NAME, function(json){
        
    
           
                if (json.rows.length>0)
                    $('input[id*=rptObjMain_ctl00_OTHER]').val(json.rows[0].KEY_NO);
                else
                   $('input[id*=rptObjMain_ctl00_OTHER]').val("");   
            });
    }
       
    function openDetail(strDate, strType, strName) {
        
        
        window.parent.openPopUpWindow();
    }
    </script>

</asp:Content>
