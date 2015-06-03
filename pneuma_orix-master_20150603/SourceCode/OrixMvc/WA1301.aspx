

<%@ Page Language="C#" EnableEventValidation="false" AutoEventWireup="true" MasterPageFile="~/Pattern/editing.Master"
    CodeBehind="WA1301.aspx.cs" Inherits="OrixMvc.WA1301" %>

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
                �ӽЮѽs���G
            </th>
            <td>
                <asp:TextBox runat="server" ID="APLY_NO" CssClass="display" Width="150"></asp:TextBox>
            </td>
            <th>
                �ثe���p�G
            </th>
            <td>
                <asp:TextBox runat="server" ID="CUR_STS" CssClass="display" Width="100"></asp:TextBox>
            </td>
        </tr>
    </table>
    
    <asp:SqlDataSource EnableViewState="true" SelectCommandType="Text" ID="sqlSUPPLY_PAY_WAY"
        ConnectionString="<%$ ConnectionStrings:myConnectionString%>" SelectCommand="Exec s_ConditionItems 'SUPPLY_PAY_WAY'"
        runat="server"></asp:SqlDataSource>
    <asp:SqlDataSource EnableViewState="true" SelectCommandType="Text" ID="sqlCUS_PAY_WAY"
        ConnectionString="<%$ ConnectionStrings:myConnectionString%>" SelectCommand="Exec s_ConditionItems 'CUS_PAY_WAY'"
        runat="server"></asp:SqlDataSource>
    <asp:SqlDataSource EnableViewState="true" SelectCommandType="Text" ID="sqlCITY_CODE"
        ConnectionString="<%$ ConnectionStrings:myConnectionString%>" SelectCommand="select ' ' city_code,'�п��..' City_name union all select city_code,City_name from or3_zip where city_code!='' group by city_code, city_name  order by city_code "
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
        ConnectionString="<%$ ConnectionStrings:myConnectionString%>" SelectCommand="select '' TypeCode,'' TypeDesc  union all select ASUR_TYPE_CODE TypeCode,ASUR_TYPE_NAME TypeDesc  from OR_ASUR_TYPE where ltrim(ASUR_TYPE_CODE) !='' order by 1"
        runat="server"></asp:SqlDataSource>
    <asp:SqlDataSource EnableViewState="true" SelectCommandType="Text" ID="sqlONUS" ConnectionString="<%$ ConnectionStrings:myConnectionString%>"
        SelectCommand="Exec s_ConditionItems'ONUS'" runat="server"></asp:SqlDataSource>
    <asp:SqlDataSource EnableViewState="true" SelectCommandType="Text" ID="sqlTMP_CODE"
        ConnectionString="<%$ ConnectionStrings:myConnectionString%>" SelectCommand="
select TMP_CODE='',TMP_DESC='�п��...' union all select TMP_CODE,TMP_DESC from OR3_CONTRACT_TEMP_SET"
        runat="server"></asp:SqlDataSource>

     <asp:SqlDataSource EnableViewState="true" SelectCommandType="Text" ID="sqlRemark"
        ConnectionString="<%$ ConnectionStrings:myConnectionString%>" SelectCommand="select seq,remark from OR3_Common_Remark"
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
                    <li class="tabs-selected" id="tab1" onclick="tab_select(1,5)"><a href="javascript:void(0)"
                        class="tabs-inner"><span class="tabs-title">�򥻸��</span><span class="tabs-icon"></span></a></li>
                    <li class="" id="tab2" onclick="tab_select(2,5)"><a href="javascript:void(0)" class="tabs-inner">
                        <span class="tabs-title">���A�Ъ���</span><span class="tabs-icon"></span></a></li>
                    <li class="" id="tab3" onclick="tab_select(3,5)"><a href="javascript:void(0)" class="tabs-inner">
                        <span class="tabs-title">�ӽб���</span><span class="tabs-icon"></span></a></li>
                    <li class="" id="tab4" onclick="tab_select(4,5)"><a href="javascript:void(0)" class="tabs-inner">
                        <span class="tabs-title">�T�{���e</span><span class="tabs-icon"></span></a></li>
                    <li class="" id="tab5" onclick="tab_select(5,5)"><a href="javascript:void(0)" class="tabs-inner">
                        <span class="tabs-title">�Ƶ�</span><span class="tabs-icon"></span></a></li>
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
                                    �ӽФ���G
                                </th>
                                <td>
                                    <ocxControl:ocxDate runat="server" ID="APLY_DATE" Text='<%# Eval("APLY_DATE").ToString().Trim() %>' />
                                </td>
                                <th class="nonSpace">
                                    �ӽг��G
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
                                    �g��N���G
                                </th>
                                <td>
                                    <ocxControl:ocxDialog runat="server" ID="EMP_CODE" Text='<%# Eval("EMP_CODE") %>'
                                        width="60"  FieldName="EMP_NAME" SourceName="OR_EMP" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" CssClass="display" ID="EMP_NAME" Width="70" MaxLength="10" Text='<%# Eval("EMP_NAME").ToString().Trim() %>'></asp:TextBox>                                   
                                </td>
                            </tr>
                            <tr>
                                <th class="nonSpace">
                                    �Ȥ�N���G
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
                                    
                                </td>
                                <th>
                                    �즸��Ĳ��G
                                </th>
                                <td>
                                    <ocxControl:ocxDate runat="server" ID="InitContactDate" Text='<%# Eval("InitContactDate").ToString().Trim() %>' />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    �����s���G
                                </th>
                                <td colspan="2">
                                    <input type="text" class="display" style="width: 130px" value='<%# Eval("CON_SEQ_NO").ToString().Trim() %>'
                                        id="CON_SEQ_NO" />
                                </td>
                                <th>
                                    �«����s���G
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="OLD_CON_NO" Width="130" MaxLength="20" Text='<%# Eval("OLD_CON_NO").ToString().Trim() %>'></asp:TextBox>
                                </td>
                                <td>
                                    <font class="memo">
                                        <%# Eval("PROJECT") %></font>
                                </td>
                                <th>
                                    �D���s���G
                                </th>
                                <td>
                                    <ocxControl:ocxDialog runat="server" ID="MAST_CON_NO" Text='<%# Eval("MAST_CON_NO") %>'
                                        width="90" SourceName="OR3_MASTER_CONTRACT" />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    ���B�׽s���G
                                </th>
                                <td colspan="2">
                                    <asp:TextBox runat="server" ID="ORG_QUOTA_APLY_NO" Width="130" MaxLength="20" Text='<%# Eval("ORG_QUOTA_APLY_NO").ToString().Trim() %>'></asp:TextBox>
                                </td>
                                <th>
                                    �{�B�׽s���G
                                </th>
                                <td>
                                    <ocxControl:ocxDialog runat="server" ID="CUR_QUOTA_APLY_NO" Text='<%# Eval("CUR_QUOTA_APLY_NO") %>'
                                        width="90" SourceName="OR3_QUOTA_APLY_BASE" />
                                </td>
                                <th>
                                    �s�����G
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
                                    �дڦa�}�G
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
                                    �s�g��G
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
                                    ����H�G
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="RECVER" MaxLength="20" Width="90" Text='<%# Eval("RECVER").ToString().Trim() %>'></asp:TextBox>
                                </td>
                                <th>
                                    ���󳡪��G
                                </th>
                                <td colspan="2">
                                    <asp:TextBox runat="server" ID="RECVER_DEPT" MaxLength="20" Width="200" Text='<%# Eval("RECVER_DEPT").ToString().Trim() %>'></asp:TextBox>
                                </td>
                                <th style="display:none">
                                    �Τ@�l�H�G
                                </th>
                                <td style="display:none">
                                    <ocxControl:ocxDialog SourceName="OR_MERG_MAIL" width="60" ControlID="MMAIL_NAME"
                                        FieldName="MMAIL_NAME" runat="server" Text='<%# Eval("MMAIL_NO").ToString().Trim() %>'
                                        ID="MMAIL_NO" />
                                </td>
                                <td style="display:none">
                                    <input type="text" readonly class="display" style="width: 70px" value='<%# Eval("MMAIL_NAME").ToString().Trim() %>'
                                        id="MMAIL_NAME" />
                                </td>
                            </tr>
                            <tr>
                                <th class="nonSpace">
                                    �p���H�G
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="CONTACT" Width="90" size="10" MaxLength="12" Text='<%# Eval("Contact").ToString().Trim() %>'></asp:TextBox>
                                </td>
                                <th class="nonSpace">
                                    �q�ܡG
                                </th>
                                <td >
                                    <asp:TextBox runat="server" ID="CTAC_TEL" Width="90" size="10" MaxLength="12" Text='<%# Eval("CTAC_TEL").ToString().Trim() %>'></asp:TextBox>
                                </td>
                                <th class="nonSpace">
                                    �ǯu�G
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="FAX" Width="100" size="10" MaxLength="12" Text='<%# Eval("FAX").ToString().Trim() %>'></asp:TextBox>
                                </td>
                                <th>
                                    ����G
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="MOBILE" Width="100" size="10" MaxLength="12" Text='<%# Eval("MOBILE").ToString().Trim() %>'></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="vertical-align: top">
                                    �����ӭI����O�G<asp:TextBox runat="server" ID="SupplierBackground" TextMode="MultiLine"
                                        Rows="3" Width="240" Text='<%# Eval("SupplierBackground").ToString().Trim() %>'></asp:TextBox>
                                </td>
                                <td colspan="4" style="vertical-align: top">
                                    �P�~���ӥD�n����G<asp:TextBox runat="server" ID="MainCondition" TextMode="MultiLine" Rows="3"
                                        Width="240" Text='<%# Eval("MainCondition").ToString().Trim() %>'></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:Repeater>
                <table style="width:810px">
                <tr><td>
                <asp:UpdatePanel runat="server" ID="upCustomBase" RenderMode="Inline" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Repeater runat="server" ID="rptBaseCustom">
                            <ItemTemplate>
                                <table style="background-color: #E3E3E3" >
                                    <tr>
                                        <th>
                                            �t�d�H�G
                                        </th>
                                        <td>
                                            <div style="width: 100px" class="text">
                                                <%# Eval("TAKER").ToString().Trim() %></div>
                                        </td>
                                        <th>
                                            �]�ߤ���G
                                        </th>
                                        <td>
                                            <asp:TextBox runat="server" ID="BUILD_DATE" CssClass="display" ReadOnly="true" Width="100"
                                                MaxLength="20" Text='<%# Eval("BUILD_DATE").ToString().Trim() %>'></asp:TextBox>
                                        </td>                                       
                                    </tr>
                                    <tr>
                                        <th>
                                            ��~�n�O�a�}�G
                                        </th>
                                        <td colspan="3">
                                            <div style="width: 250px" class="text">
                                                <%# Eval("SALES_RGT_ADDR").ToString().Trim()%></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            �ꥻ���c�G
                                        </th>
                                        <td>
                                            <div style="width: 120px" class="text">
                                                <%# Eval("CAPT_STR").ToString().Trim()%></div>
                                        </td>
                                        <th>
                                            ��´���A�G
                                        </th>
                                        <td>
                                            <div style="width: 120px" class="text">
                                                <%# Eval("ORG_TYPE").ToString().Trim()%></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            �n�O�ꥻ�B�G
                                        </th>
                                        <td>
                                            <div style="width: 120px" class="text number">
                                                <%# Eval("RGT_CAPT_AMT","{0:###,###,###,##0}") %></div>
                                            �U��
                                        </td>
                                        <th>
                                            ���u�H�ơG
                                        </th>
                                        <td>
                                            <div style="width: 120px" class="text number">
                                                <%# Eval("EMP_PSNS", "{0:###,###,###,##0}")%></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            �ꦬ�ꥻ�B�G
                                        </th>
                                        <td>
                                            <div style="width: 120px" class="text number">
                                                <%# Eval("REAL_CAPT_AMT", "{0:###,###,###,##0}")%></div>
                                            �U��
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            �D�n��~���ءG
                                        </th>
                                        <td colspan="3">
                                            <div style="width: 180px" class="text">
                                                <%# Eval("MAIN_BUS_ITEM").ToString().Trim()%></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            �H���B�סG
                                        </th>
                                        <td>
                                            <div style="width: 120px" class="text number">
                                                <%# Eval("CREDIT_CUST", "{0:###,###,###,##0}")%></div>
                                        </td>
                                        <th>
                                            �H�ε����G
                                        </th>
                                        <td>
                                            <div style="width: 50px" class="text">
                                                <%# Eval("JUDGE_LVL").ToString().Trim()%></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            �G���w�G
                                        </th>
                                        <td>
                                            <input type="checkbox" disabled <%# Eval("HONEST_AGREEMENT").ToString()=="Y"?"checked":"" %> />
                                        </td>
                                        <th>
                                            �O�K���w�G
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
                </td>
                    <td>
                        <asp:Repeater runat="server" ID="rptREF">
                        <ItemTemplate>
                        
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <th style="color: darkred">
                                    �O�_���ֳƨ�����
                                </th>
                                <td>
                                    <asp:RadioButtonList runat="server" ID="AP_SUP" RepeatDirection="Horizontal" SelectedIndex='<%# Eval("AP_SUP")%>'>
                                        <asp:ListItem Value="Y">�O</asp:ListItem>
                                        <asp:ListItem Value="N">�_</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <th style="color: green">
                                    �O�_���W���W�d���q���q
                                </th>
                                <td>
                                    <asp:RadioButtonList runat="server" ID="STOCK_CORP" RepeatDirection="Horizontal"
                                        SelectedIndex='<%# Eval("STOCK_CORP")%>'>
                                        <asp:ListItem Value="Y">�O</asp:ListItem>
                                        <asp:ListItem Value="N">�_</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    �O�_�����H����-����
                                </th>
                                <td>
                                    <asp:RadioButtonList runat="server" ID="CHK_RCD_S" RepeatDirection="Horizontal" SelectedIndex='<%# Eval("CHK_RCD_S")%>'>
                                        <asp:ListItem Value="Y">�O</asp:ListItem>
                                        <asp:ListItem Value="N">�_</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <th style="color: green">
                                    �O�_�����H����-�t�d�H
                                </th>
                                <td>
                                    <asp:RadioButtonList runat="server" ID="CHK_RCD_C" RepeatDirection="Horizontal" SelectedIndex='<%# Eval("CHK_RCD_C")%>'>
                                        <asp:ListItem Value="Y">�O</asp:ListItem>
                                        <asp:ListItem Value="N">�_</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    �O�_�����H����-�O�ҤH
                                </th>
                                <td>
                                    <asp:RadioButtonList runat="server" ID="CHK_RCD_G" RepeatDirection="Horizontal" SelectedIndex='<%# Eval("CHK_RCD_G")%>'>
                                        <asp:ListItem Value="Y">�O</asp:ListItem>
                                        <asp:ListItem Value="N">�_</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <th style="color: green">
                                    �s���q�ΫDVP��z�v���ץ�
                                </th>
                                <td>
                                    <asp:RadioButtonList runat="server" ID="NEW_CORP" RepeatDirection="Horizontal" SelectedIndex='<%# Eval("NEW_CORP")%>'>
                                        <asp:ListItem Value="Y">�O</asp:ListItem>
                                        <asp:ListItem Value="N">�_</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    �����Ӥ��@���P�@��
                                </th>
                                <td>
                                    <asp:RadioButtonList runat="server" ID="CRD_SAME" RepeatDirection="Horizontal" SelectedIndex='<%# Eval("CRD_SAME")%>'>
                                        <asp:ListItem Value="Y">�O</asp:ListItem>
                                        <asp:ListItem Value="N">�_</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    �����Y�ᶡ�����
                                </th>
                                <td>
                                    <asp:RadioButtonList runat="server" ID="CRD_RAL" RepeatDirection="Horizontal" SelectedIndex='<%# Eval("CRD_RAL")%>'>
                                        <asp:ListItem Value="Y">�O</asp:ListItem>
                                        <asp:ListItem Value="N">�_</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    �D�޽T�{�w����b�~(�t)<br />
                                    �H�W�Ө����ӥ��}�o��
                                </th>
                                <td>
                                    <asp:RadioButtonList runat="server" ID="CRD_UNINV" RepeatDirection="Horizontal" SelectedIndex='<%# Eval("CRD_UNINV")%>'>
                                        <asp:ListItem Value="Y">�O</asp:ListItem>
                                        <asp:ListItem Value="N">�_</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                        </ItemTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
                </table>
            </div>
            <div class="panel" style="height: auto; width: 820px; display: none" id="tabpanel2">
                <asp:Repeater runat="server" ID="rptObjMain">
                    <ItemTemplate>
                        <table style="width: 810px; margin: 0px">
                            <tr>
                                <th>
                                    ���v���O�G
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
                                    �ץ����O�G
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
                                    �f�d�ץ����O�G
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
                                    �ץ�ӷ��G
                                </th>
                                <td>
                                    <asp:DropDownList runat="server" ID="CASE_SOUR" OnPreRender='checkList' ToolTip='<%# Eval("CASE_SOUR").ToString().Trim() %>'
                                        Width="80" DataSourceID="sqlCASE_SOUR" DataValueField="TypeCode" DataTextField="TypeDesc">
                                    </asp:DropDownList>
                                </td>
                                <th>
                                    ���}�o���G
                                </th>
                                <td>
                                    <asp:DropDownList runat="server" ID="DIVIDE" OnPreRender='checkList' ToolTip='<%# Eval("DIVIDE").ToString().Trim() %>'
                                        Width="50">
                                        <asp:ListItem Value="N">�_</asp:ListItem>
                                        <asp:ListItem Value="Y">�O</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <th>
                                    ���ФH�G
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="AGENT" Width="70" MaxLength="20" Text='<%# Eval("AGENT").ToString().Trim() %>'></asp:TextBox>
                                </td>
                                <th>
                                    ��L�G
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
                                                            �s��
                                                        </th>
                                                        <th>
                                                            �Ъ����N��
                                                        </th>
                                                        <th>
                                                            �Ъ����Ҧb�a�}
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <asp:Repeater runat="server" ID="rptObjGrid">
                                                    <ItemTemplate>
                                                        <tr id='trObj<%#Container.ItemIndex+1%>' class="<%#Container.ItemIndex%2==0?"srow":"" %>">
                                                            <td>
                                                                <asp:Button ID="btnDel_Object" runat="server" CssClass="button del" Text="�R��" OnClientClick="return window.confirm('�O�_�T�w�R���������?');"
                                                                    OnCommand="GridFunc_Click" CommandName='<%# Eval("OBJ_KEY").ToString().Trim() %>' />
                                                                <asp:Button ID="btnUpd_Object" runat="server" CssClass="button upd" Text="�ק�" OnCommand="GridFunc_Click"
                                                                    CommandName='<%# Eval("OBJ_CODE").ToString().Trim() %>' />
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
                                                        <asp:Button ID="btnAdd_Object" runat="server" CssClass="button dadd" Text="�s�W" OnCommand="GridFunc_Click"
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
                         <asp:Repeater runat="server" ID="rptObjDetail_Sum">
                                        <ItemTemplate>                     
                            <table  style="width: 280px">
                                <tr>
                                    <th>
                                        �ݭȥ[�`�G
                                    </th>
                                    <td>
                                        <div style="width: 120px" class="text number"><%# Eval("RV_AMT_SUM", "{0:###,###,###,##0}")%></div>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        �������B�[�`�G
                                    </th>
                                    <td>
                                        <div style="width: 120px" class="text number"><%# Eval("AP_SUM", "{0:###,###,###,##0}")%></div>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        ��U���B�[�`�G
                                    </th>
                                    <td>
                                        <div style="width: 120px" class="text number"><%# Eval("SL_SUM", "{0:###,###,###,##0}")%></div>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        �o�����B�[�`�G
                                    </th>
                                    <td>
                                        <div style="width: 120px" class="text number"><%# Eval("IIB_SUM", "{0:###,###,###,##0}")%></div>
                                    </td>
                                </tr>
                            </table>
                            </ItemTemplate>
                            </asp:Repeater>
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
                                                        �®ץӽЮѽs���G
                                                    </th>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="OLD_APLY_NO" CssClass="display" ReadOnly="true" Width="70"
                                                            MaxLength="20" Text='<%# Eval("OLD_APLY_NO").ToString().Trim() %>'></asp:TextBox>
                                                    </td>
                                                    <th>
                                                        ��کӯ��H�G
                                                    </th>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="Actual_lessee" CssClass="display" ReadOnly="true"
                                                            Width="70" MaxLength="20" Text='<%# Eval("Actual_lessee").ToString().Trim() %>'></asp:TextBox>
                                                    </td>
                                                    <td colspan="2">
                                                        �P�����ӬۦP
                                                        <asp:RadioButtonList ID="Is_spec_repo" runat="server" OnPreRender='checkList' ToolTip='<%# Eval("Is_spec_repo").ToString().Trim() %>'
                                                            RepeatLayout="Flow" RepeatDirection="Horizontal">
                                                            <asp:ListItem Value="Y">�O</asp:ListItem>
                                                            <asp:ListItem Value="N">�_</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th class="nonSpace">
                                                        �Ъ����N���G
                                                    </th>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="OBJ_CODE" Width="90" MaxLength="20" Text='<%# Eval("OBJ_CODE").ToString().Trim() %>'></asp:TextBox>
                                                    </td>
                                                    <th class="nonSpace">
                                                        �~�W�G
                                                    </th>
                                                    <td colspan="2">
                                                        <asp:TextBox runat="server" ID="PROD_NAME" Width="150" MaxLength="200" Text='<%# Eval("PROD_NAME").ToString().Trim() %>'></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" OnPreRender='checkList' ID="OTC" ToolTip='<%# Eval("OTC").ToString().Trim() %>' />OTC�겣
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        �Ъ����Ҧb�a�G
                                                    </th>
                                                    <td colspan="5">
                                                        <asp:TextBox runat="server" ID="OBJ_LOC_ADDR" Width="350" MaxLength="100" Text='<%# Eval("OBJ_LOC_ADDR").ToString().Trim() %>'></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        �W��G
                                                    </th>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="SPEC" Width="90" MaxLength="20" Text='<%# Eval("SPEC").ToString().Trim() %>'></asp:TextBox>
                                                    </td>
                                                    <th>
                                                        �t�P�G
                                                    </th>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="BRAND" Width="70" MaxLength="20" Text='<%# Eval("BRAND").ToString().Trim() %>'></asp:TextBox>
                                                    </td>
                                                    <th>
                                                        �����Ҧ��v�G
                                                    </th>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="OBJ_DUE_OWNER" Width="70" MaxLength="20" Text='<%# Eval("OBJ_DUE_OWNER").ToString().Trim() %>'></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        �����G
                                                    </th>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="MAC_NO" Width="90" MaxLength="20" Text='<%# Eval("MAC_NO").ToString().Trim() %>'></asp:TextBox>
                                                    </td>
                                                    <th>
                                                        �~���G
                                                    </th>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="YEAR" Width="70" MaxLength="4" Text='<%# Eval("YEAR").ToString().Trim() %>'></asp:TextBox>
                                                    </td>
                                                    <th>
                                                        �����G
                                                    </th>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="CAR_NO" Width="70" MaxLength="20" Text='<%# Eval("CAR_NO").ToString().Trim() %>'></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th class="nonSpace">
                                                        �����G
                                                    </th>
                                                    <td>
                                                        <ocxControl:ocxNumber runat="server" ID="REAL_BUY_PRC" MASK="9,999,999" Text='<%# Eval("REAL_BUY_PRC").ToString().Trim() %>' />
                                                    </td>
                                                    <th>
                                                        ����/����G
                                                    </th>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="BUDG_LEASE" OnPreRender='checkList' ToolTip='<%# Eval("BUDG_LEASE").ToString().Trim() %>'
                                                            Width="50">
                                                            <asp:ListItem Value="S">����</asp:ListItem>
                                                            <asp:ListItem Value="L">����</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <th>
                                                        ���B�G
                                                    </th>
                                                    <td>
                                                        <ocxControl:ocxNumber runat="server" ID="BUDG_LEASE_AMT" MASK="9,999,999" Text='<%# Eval("BUDG_LEASE_AMT").ToString().Trim() %>' />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        �i���o�����B�G
                                                    </th>
                                                    <td>
                                                        <ocxControl:ocxNumber runat="server" ID="INV_AMT_I_IB" MASK="9,999,999" Text='<%# Eval("INV_AMT_I_IB").ToString().Trim() %>' />
                                                    </td>
                                                    <th>
                                                        ��ݭȡG
                                                    </th>
                                                    <td>
                                                        <ocxControl:ocxNumber runat="server" ID="RV_AMT" MASK="9,999,999" Text='<%# Eval("RV_AMT").ToString().Trim() %>' />
                                                    </td>
                                                    <th>
                                                        �۳Ʋv�G
                                                    </th>
                                                    <td>
                                                        <ocxControl:ocxNumber runat="server" ID="SELF_RATE" MASK="99.9999" Text='<%# Eval("SELF_RATE").ToString().Trim() %>' />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        �q�ܤ@�G
                                                    </th>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="OBJ_LOC_TEL" size="11" MaxLength="20" Text='<%# Eval("OBJ_LOC_TEL").ToString().Trim() %>'></asp:TextBox>
                                                    </td>
                                                    <th>
                                                        �ǯu�G
                                                    </th>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="OBJ_LOC_FAX" size="12" MaxLength="20" Text='<%# Eval("OBJ_LOC_FAX").ToString().Trim() %>'></asp:TextBox>
                                                    </td>
                                                    <th>
                                                        �p���H�G
                                                    </th>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="OBJ_LOC_CTAC" size="10" MaxLength="12" Text='<%# Eval("OBJ_LOC_CTAC").ToString().Trim() %>'></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th class="nonSpace">
                                                        ���R�^�覡�G
                                                    </th>
                                                    <td>
                                                        <asp:DropDownList ID="BUY_WAY" OnPreRender='checkList' ToolTip='<%# Eval("BUY_WAY").ToString().Trim() %>'
                                                            runat="server" Width="80">
                                                            <asp:ListItem Value=""></asp:ListItem>
                                                            <asp:ListItem Value="1">�����R�^</asp:ListItem>
                                                            <asp:ListItem Value="2">�ʶR�B</asp:ListItem>
                                                            <asp:ListItem Value="3">�����l�B</asp:ListItem>
                                                            <asp:ListItem Value="4">�����l�B</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <th>
                                                        �R�^�ӿդH�G
                                                    </th>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="BUY_PROMISE" size="10" MaxLength="12" Text='<%# Eval("BUY_PROMISE").ToString().Trim() %>'></asp:TextBox>
                                                    </td>
                                                    <th class="nonSpace">
                                                        �R�^��v�G
                                                    </th>
                                                    <td>
                                                        <ocxControl:ocxNumber runat="server" ID="BUY_RATE" MASK="999.9999" Text='<%# Eval("BUY_RATE").ToString().Trim() %>' />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th class="nonSpace">
                                                        �����ӡG
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
                                                        ��~���G
                                                    </th>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="SALES_UNIT" size="11" MaxLength="20" Text='<%# Eval("SALES_UNIT").ToString().Trim() %>'></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th class="nonSpace">
                                                        �I�اO�G
                                                    </th>
                                                    <td colspan="3">
                                                        <asp:DropDownList ID="OBJ_ASUR_TYPE" Style="width: 200px" runat="server" OnPreRender='checkList'
                                                            ToolTip='<%# Eval("OBJ_ASUR_TYPE").ToString().Trim() %>' DataSourceID="sqlASUR_TYPE_CODE"
                                                            DataValueField="TypeCode" DataTextField="TypeDesc">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <th>
                                                        �O�I�t��̡G
                                                    </th>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="OBJ_ONUS" OnPreRender='checkList' ToolTip='<%# Eval("OBJ_ONUS").ToString().Trim() %>'
                                                            Width="50" DataSourceID="sqlONUS" DataValueField="TypeCode" DataTextField="TypeDesc">
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
                                                            �s��
                                                        </th>
                                                        <th>
                                                            �t��W��
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <asp:Repeater runat="server" ID="rptAccs">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnDel_Accs" runat="server" CssClass="button del" Text="�R��" OnCommand="GridFunc_Click"
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
                                                        <asp:Button ID="btnAdd_Accs" runat="server" CssClass="button dadd" Text="�s�W" OnCommand="GridFunc_Click"
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
                                        <asp:Button runat="server" ID="btnEdit" CssClass="button trn80" Text="�����x�s" Font-Bold="true"
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
                                    �I�ڱ���G
                                </th>
                                <td>
                                    <ocxControl:ocxNumber runat="server" ID="PAY_COND_DAY" MASK="99,999" Text='<%# Eval("PAY_COND_DAY").ToString().Trim() %>' />
                                    ��
                                </td>
                                <th colspan="2">
                                    �I�ڨ����ӤѼơG
                                </th>
                                <td>
                                    <ocxControl:ocxNumber runat="server" ID="PAY_DAY" MASK="99,999" Text='<%# Eval("PAY_DAY").ToString().Trim() %>' />
                                </td>
                            </tr>
                            <tr>
                                <th style="width: 100px" class="nonSpace">
                                    �ʶR�B�G
                                </th>
                                <td style="width: 140px">
                                    <ocxControl:ocxNumber runat="server" ID="APLY_BUY_AMT" MASK=",999,999" Text='<%# Eval("APLY_BUY_AMT").ToString().Trim() %>' />
                                </td>
                                <th style="width: 100px">
                                    �����G
                                </th>
                                <td style="width: 70px">
                                    <ocxControl:ocxNumber runat="server" ID="APLY_RAKE" MASK=",999,999" Text='<%# Eval("APLY_RAKE").ToString().Trim() %>' />
                                </td>
                                <th style="width: 70px">
                                    ��L�O�ΡG
                                </th>
                                <td style="width: 70px">
                                    <ocxControl:ocxNumber runat="server" ID="APLY_ANT_EXP" MASK=",999,999" Text='<%# Eval("APLY_ANT_EXP").ToString().Trim() %>' />
                                </td>
                                <th style="width: 70px">
                                    ��X�X�p�G
                                </th>
                                <td style="width: 70px">
                                    <ocxControl:ocxNumber runat="server" bolEnabled="false" ID="TOTAL" MASK=",999,999"
                                        Text='<%# Eval("TOTAL").ToString().Trim() %>' />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    �O�Ҫ�(�t�|)�G
                                </th>
                                <td>
                                    <ocxControl:ocxNumber runat="server" ID="APLY_BOND" MASK=",999,999" Text='<%# Eval("APLY_BOND").ToString().Trim() %>' />
                                </td>
                                <th>
                                    �ݭȡG
                                </th>
                                <td>
                                    <ocxControl:ocxNumber runat="server" ID="APLY_REST" MASK=",999,999" Text='<%# Eval("APLY_REST").ToString().Trim() %>' />
                                </td>
                                <th>
                                    �i���|�B�G
                                </th>
                                <td>
                                    <ocxControl:ocxNumber runat="server" ID="APLY_PURS_TAX" MASK=",999,999" Text='<%# Eval("APLY_PURS_TAX").ToString().Trim() %>' />
                                </td>
                                <th class="nonSpace">
                                    �I�ڶg���G
                                </th>
                                <td>
                                    <asp:DropDownList runat="server" ID="APLY_PAY_PERD" OnPreRender='checkList' ToolTip='<%# Eval("APLY_PAY_PERD").ToString().Trim() %>'
                                        Width="80" DataSourceID="sqlPAY_PERD" DataValueField="TypeCode" DataTextField="TypeDesc">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th class="nonSpace">
                                    �����G
                                </th>
                                <td>
                                    <ocxControl:ocxNumber runat="server" ID="APLY_DURN_M" MASK="999" Text='<%# Eval("APLY_DURN_M").ToString().Trim() %>' />
                                    �Ӥ�
                                    <ocxControl:ocxNumber runat="server" ID="APLY_PERD" MASK="999" Text='<%# Eval("APLY_PERD").ToString().Trim() %>' />
                                    ��
                                </td>
                                <th>
                                    �O�I�]�l(%)�G
                                </th>
                                <td>
                                    <ocxControl:ocxNumber runat="server" ID="ISU_FACTOR" MASK="99,999" Text='<%# Eval("ISU_FACTOR").ToString().Trim() %>' />
                                </td>
                                <th>
                                    �O�d�ڡG
                                </th>
                                <td>
                                    <ocxControl:ocxNumber runat="server" ID="APLY_SAVING_EXP" MASK=",999,999" Text='<%# Eval("APLY_SAVING_EXP").ToString().Trim() %>' />
                                </td>
                                <th>
                                    ���TR�G
                                </th>
                                <td>
                                    <ocxControl:ocxNumber runat="server" bolEnabled="false" ID="APLY_REAL_TR" MASK=",999,999"
                                        Text='<%# Eval("APLY_REAL_TR").ToString().Trim() %>' />
                                </td>
                            </tr>
                            <tr>
                                <th class="nonSpace">
                                    ú�ڤ覡�G
                                </th>
                                <td>
                                    <asp:DropDownList runat="server" ID="APLY_PAY_MTHD" OnPreRender='checkList' ToolTip='<%# Eval("APLY_PAY_MTHD").ToString().Trim() %>'
                                        Width="80" DataSourceID="sqlPAY_MTHD" DataValueField="TypeCode" DataTextField="TypeDesc">
                                    </asp:DropDownList>
                                </td>
                                <th>
                                    �Y����(���|)�G
                                </th>
                                <td>
                                    <ocxControl:ocxNumber runat="server" ID="APLY_DEPS" MASK=",999,999" Text='<%# Eval("APLY_DEPS").ToString().Trim() %>' />
                                </td>
                                <th>
                                    �O�I�O�G
                                </th>
                                <td>
                                    <ocxControl:ocxNumber runat="server" ID="ISU_AMT" MASK=",999,999" Text='<%# Eval("ISU_AMT").ToString().Trim() %>' />
                                </td>
                                <th>
                                    ��TR�G
                                </th>
                                <td>
                                    <ocxControl:ocxNumber runat="server" bolEnabled="false" ID="APLY_SURF_TR" MASK=",999,999"
                                        Text='<%# Eval("APLY_SURF_TR").ToString().Trim() %>' />
                                </td>
                            </tr>
                            <th class="nonSpace">
                                �u���覡�G
                            </th>
                            <td>
                                <asp:DropDownList runat="server" onchange="changeMTHD(1,this)" ID="APLY_AMOR_MTHD"
                                    OnPreRender='checkList' ToolTip='<%# Eval("APLY_AMOR_MTHD").ToString().Trim() %>'
                                    Width="80" DataSourceID="sqlAMOR_MTHD" DataValueField="TypeCode" DataTextField="TypeDesc">
                                </asp:DropDownList>
                            </td>
                            <th>
                                ����O�G
                            </th>
                            <td>
                                <ocxControl:ocxNumber runat="server" ID="APLY_SERV_CHAR" MASK=",999,999" Text='<%# Eval("APLY_SERV_CHAR").ToString().Trim() %>' />
                            </td>
                            </tr>
                            <tr>
                                <th>
                                    �|�O�G
                                </th>
                                <td>
                                    <asp:DropDownList runat="server" ID="APLY_TAX_ZERO" OnPreRender='checkList' ToolTip='<%# Eval("APLY_TAX_ZERO").ToString().Trim() %>'
                                        Width="80" DataSourceID="sqlTAX_TYPE" DataValueField="TypeCode" DataTextField="TypeDesc">
                                    </asp:DropDownList>
                                </td>
                                <th colspan="2">
                                    ���p�~�Z����O�G
                                </th>
                                <td>
                                    <ocxControl:ocxNumber runat="server" ID="NON_FEAT_CHARGE" MASK=",999,999" Text='<%# Eval("NON_FEAT_CHARGE").ToString().Trim() %>' />
                                </td>
                                <th>
                                    ��L�Q���G
                                </th>
                                <td>
                                    <ocxControl:ocxNumber runat="server" bolEnabled="false" ID="APLY_OTH_INT" MASK=",999,999"
                                        Text='<%# Eval("APLY_OTH_INT").ToString().Trim() %>' />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <table cellpadding="0" cellspacing="0" style="margin: 0">
                                        <tr>
                                            <td>
                                                <fieldset id="MTHD1_1" style="padding: 0; margin: 0; display: none">
                                                    <legend>�w�B</legend>
                                                    <table style="margin: 0px" cellpadding="1" cellspacing="1">
                                                        <tr>
                                                            <th>
                                                                �����G
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber runat="server" ID="APLY_HIRE" MASK="9,999,999" Text='<%# Eval("APLY_HIRE").ToString().Trim() %>' />
                                                            </td>
                                                            <th>
                                                                �|���G
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber runat="server" ID="APLY_TAX" MASK="9,999,999" Text='<%# Eval("APLY_TAX").ToString().Trim() %>' />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                                <br />
                                                <fieldset id="MTHD1_2" style="padding: 0; margin: 0; display: none">
                                                    <legend>���w�B</legend>
                                                    <table style="margin: 0px; width: 300px" cellpadding="1" cellspacing="1">
                                                        <tr>
                                                            <th>
                                                                LF1�G
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber runat="server" ID="APLY_LF1_FR" MASK="99" bolEnabled="false"
                                                                    Text='<%# Eval("APLY_LF1_FR").ToString().Trim() %>' />
                                                                ~
                                                                <ocxControl:ocxNumber runat="server" ID="APLY_LF1_TO" MASK="99" Text='<%# Eval("APLY_LF1_TO").ToString().Trim() %>' />
                                                            </td>
                                                            <th>
                                                                �����G
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber runat="server" ID="APLY_LF1_HIRE" MASK=",999,999" Text='<%# Eval("APLY_LF1_HIRE").ToString().Trim() %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th>
                                                                LF2�G
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber runat="server" ID="APLY_LF2_FR" MASK="99" bolEnabled="false"
                                                                    Text='<%# Eval("APLY_LF2_FR").ToString().Trim() %>' />
                                                                ~
                                                                <ocxControl:ocxNumber runat="server" ID="APLY_LF2_TO" MASK="99" Text='<%# Eval("APLY_LF2_TO").ToString().Trim() %>' />
                                                            </td>
                                                            <th>
                                                                �����G
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber runat="server" ID="APLY_LF2_HIRE" MASK=",999,999" Text='<%# Eval("APLY_LF2_HIRE").ToString().Trim() %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th>
                                                                LF3�G
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber runat="server" ID="APLY_LF3_FR" MASK="99" bolEnabled="false"
                                                                    Text='<%# Eval("APLY_LF3_FR").ToString().Trim() %>' />
                                                                ~
                                                                <ocxControl:ocxNumber runat="server" ID="APLY_LF3_TO" MASK="99" Text='<%# Eval("APLY_LF3_TO").ToString().Trim() %>' />
                                                            </td>
                                                            <th>
                                                                �����G
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber runat="server" ID="APLY_LF3_HIRE" MASK=",999,999" Text='<%# Eval("APLY_LF3_HIRE").ToString().Trim() %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th>
                                                                LF4�G
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber runat="server" ID="APLY_LF4_FR" MASK="99" bolEnabled="false"
                                                                    Text='<%# Eval("APLY_LF4_FR").ToString().Trim() %>' />
                                                                ~
                                                                <ocxControl:ocxNumber runat="server" ID="APLY_LF4_TO" MASK="99" Text='<%# Eval("APLY_LF4_TO").ToString().Trim() %>' />
                                                            </td>
                                                            <th>
                                                                �����G
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber runat="server" ID="APLY_LF4_HIRE" MASK=",999,999" Text='<%# Eval("APLY_LF4_HIRE").ToString().Trim() %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th>
                                                                LF5�G
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber runat="server" ID="APLY_LF5_FR" MASK="99" bolEnabled="false"
                                                                    Text='<%# Eval("APLY_LF5_FR").ToString().Trim() %>' />
                                                                ~
                                                                <ocxControl:ocxNumber runat="server" ID="APLY_LF5_TO" MASK="99" Text='<%# Eval("APLY_LF5_TO").ToString().Trim() %>' />
                                                            </td>
                                                            <th>
                                                                �����G
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber runat="server" ID="APLY_LF5_HIRE" MASK=",999,999" Text='<%# Eval("APLY_LF5_HIRE").ToString().Trim() %>' />
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
                                                                ���J�`�B�G
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber runat="server" ID="APLY_INCM_TOL" bolEnabled="false" MASK=",999,999"
                                                                    Text='<%# Eval("APLY_INCM_TOL").ToString().Trim() %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th>
                                                                �P���|�B�G
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber runat="server" ID="APLY_SELL_TAX" bolEnabled="false" MASK=",999,999"
                                                                    Text='<%# Eval("APLY_SELL_TAX").ToString().Trim() %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th>
                                                                �򦬯q�G
                                                            </th>
                                                            <td>
                                                                <ocxControl:ocxNumber runat="server" ID="APLY_MARG" bolEnabled="false" MASK=",999,999"
                                                                    Text='<%# Eval("APLY_MARG").ToString().Trim() %>' />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                                <br />
                                            </td>
                    </ItemTemplate>
                </asp:Repeater>
                 </td></tr></table>
            </td>           
                <td colspan="4">
                    
                            <div class="tabs-header" style="width: 378px;">
                                <div class="tabs-scroller-left" style="display: none;">
                                </div>
                                <div class="tabs-scroller-right" style="display: none;">
                                </div>
                                <div class="tabs-wrap" style="margin-left: 0px; margin-right: 0px; width: 370px;">
                                    <ul class="tabs">
                                        <li class="tabs-selected" id="tabD1" onclick="tabD_select(1,3)"><a href="javascript:void(0)"
                                            class="tabs-inner"><span class="tabs-title">��TR</span><span class="tabs-icon"></span></a></li>
                                        <li class="" id="tabD2" onclick="tabD_select(2,3)"><a href="javascript:void(0)" class="tabs-inner">
                                            <span class="tabs-title">���TR</span><span class="tabs-icon"></span></a></li>
                                        <li class="" id="tabD3" onclick="tabD_select(3,3)"><a href="javascript:void(0)" class="tabs-inner">
                                            <span class="tabs-title">��+��L�O��</span><span class="tabs-icon"></span></a></li>
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
                                                            ����
                                                        </th>
                                                        <th>
                                                            ����
                                                        </th>
                                                        <th>
                                                            �Q��
                                                        </th>
                                                        <th>
                                                            �����u��
                                                        </th>
                                                        <th>
                                                            �����l�B
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <asp:Repeater runat="server" ID="rptShare">
                                                    <ItemTemplate>
                                                        <tr class="<%#Container.ItemIndex%2==0?"srow":"" %>">
                                                            <td class="number">
                                                                <%# (Eval("PERIOD").ToString().Trim()=="0"?"�Y��":Eval("PERIOD","{0:###,###,###,##0}")) %>
                                                            </td>
                                                            <td class="number" style='display: <%# Eval("PERIOD").ToString().Trim()!="0" && this.APLY_MTHD=="5"?"none":"" %>'>
                                                                <%# Eval("HIRE", "{0:###,###,###,##0}")%>
                                                            </td>
                                                            <td class="number" style='display: <%# Eval("PERIOD").ToString().Trim()!="0" && this.APLY_MTHD=="5"?"":"none" %>'>
                                                                <ocxControl:ocxNumber runat="server" ID="HIRE" MASK="999,999" Text='<%# Eval("HIRE").ToString().Trim() %>' />
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
                                                �X�p�G
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
                                                            ����
                                                        </th>
                                                        <th>
                                                            ����
                                                        </th>
                                                        <th>
                                                            �Q��
                                                        </th>
                                                        <th>
                                                            �����u��
                                                        </th>
                                                        <th>
                                                            �����l�B
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <asp:Repeater runat="server" ID="rptShareReal">
                                                    <ItemTemplate>
                                                        <tr class="<%#Container.ItemIndex%2==0?"srow":"" %>">
                                                            <td class="number">
                                                                <%# (Eval("PERIOD").ToString().Trim()=="0"?"�Y��":Eval("PERIOD","{0:###,###,###,##0}")) %>
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
                                                �X�p�G
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
                                                            ����
                                                        </th>
                                                        <th>
                                                            ����
                                                        </th>
                                                        <th>
                                                            �Q��
                                                        </th>
                                                        <th>
                                                            �����u��
                                                        </th>
                                                        <th>
                                                            �����l�B
                                                        </th>
                                                        <th>
                                                            �������l
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <asp:Repeater runat="server" ID="rptShareEx">
                                                    <ItemTemplate>
                                                        <tr class="<%#Container.ItemIndex%2==0?"srow":"" %>">
                                                            <td class="number">
                                                                <%# (Eval("PERIOD").ToString().Trim()=="0"?"�Y��":Eval("PERIOD","{0:###,###,###,##0}")) %>
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
                                                �X�p�G
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
                      
                </td>
                </tr> </table>
            </div>
            <div class="panel" style="height: auto; width: 820px; display: none" id="tabpanel4">
                <asp:Repeater runat="server" ID="rptConfirm">
                    <ItemTemplate>
                        <table>
                            <tr>
                                <th class="nonSpace">
                                    ñ������G
                                </th>
                                <td>
                                    <ocxControl:ocxDate runat="server" ID="SING_CON_DATE" Text='<%# Eval("SING_CON_DATE").ToString().Trim() %>' />
                                </td>
                                <th class="nonSpace">
                                    �w�p�_������G
                                </th>
                                <td>
                                    <ocxControl:ocxDate runat="server" ID="PREDICT_LEASE_DATE" Text='<%# Eval("PREDICT_LEASE_DATE").ToString().Trim() %>' />
                                </td>
                                <th style="color: red">
                                    �����I�ڤ���G
                                </th>
                                <td>
                                    <ocxControl:ocxDate runat="server" ID="FIRST_PAY_DATE" Text='<%# Eval("FIRST_PAY_DATE").ToString().Trim() %>' />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    �w�I�����Ӥ���G
                                </th>
                                <td>
                                    <ocxControl:ocxDate runat="server" ID="PREDICT_PAY_DATE" Text='<%# Eval("PREDICT_PAY_DATE").ToString().Trim() %>' />
                                </td>
                                <td colspan="2">
                                    <asp:CheckBox runat="server" OnPreRender='checkList' ID="IF_CON" ToolTip='<%# Eval("IF_CON").ToString().Trim() %>' />�T�{�˾�
                                </td>
                                <th>
                                    �T�{�H�G
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="CON_MAN" Text='<%# Eval("CON_MAN").ToString().Trim() %>'
                                        MaxLength="10" Width="150"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    �I�����Ӥ覡�G
                                </th>
                                <td>
                                    <asp:DropDownList runat="server" ID="SUPPLY_PAY_WAY" OnPreRender='checkList' ToolTip='<%# Eval("SUPPLY_PAY_WAY").ToString().Trim() %>'
                                        Width="120" DataSourceID="sqlSUPPLY_PAY_WAY" DataValueField="TypeCode" DataTextField="TypeDesc">
                                    </asp:DropDownList>
                                </td>
                                <th>
                                    �T�{����G
                                </th>
                                <td>
                                    <ocxControl:ocxDate runat="server" ID="CON_DATE" Text='<%# Eval("CON_DATE").ToString().Trim() %>' />
                                </td>
                                <th>
                                    OTC�q�ܽT�{�H�G
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="OTC_CON_MAN" Text='<%# Eval("OTC_CON_MAN").ToString().Trim() %>'
                                        MaxLength="10" Width="150"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    �I�����Ӳ����G
                                </th>
                                <td>
                                     <ocxControl:ocxDate runat="server" ID="PAY_SUPPLY_DUE_DATE" Text='<%# Eval("PAY_SUPPLY_DUE_DATE").ToString().Trim() %>' />
                                    
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    �q�ױb���G
                                </th>
                                <td colspan="5">
                                    <asp:TextBox runat="server" ID="PAY_SUPPLY_TT_ACC" Text='<%# Eval("PAY_SUPPLY_TT_ACC").ToString().Trim() %>'
                                        MaxLength="50" Width="400"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <tr>
                                    <th>
                                        �I�����Ӫ��B�G
                                    </th>
                                    <td>
                                        <ocxControl:ocxNumber runat="server" ID="PAY_AUPPY_AMT"  MASK="9,999,999"
                                            Text='<%# Eval("PAY_AUPPY_AMT").ToString().Trim() %>' />
                                    </td>                                
                                <th>
                                    �Τ@�l�H�G
                                </th>
                                <td>
                                    <ocxControl:ocxDialog SourceName="OR_MERG_MAIL" width="60" ControlID="MMAIL_NAME2"
                                        FieldName="MMAIL_NAME" runat="server" Text='<%# Eval("MMAIL_NO").ToString().Trim() %>'
                                        ID="MMAIL_NO" />
                                </td>
                                <td>
                                    <input type="text"  class="display" style="width: 70px" value='<%# Eval("MMAIL_NAME").ToString().Trim() %>'
                                        id="MMAIL_NAME2" />
                                </td>
                            </tr>
                            <tr>
                                <th class="nonSpace">
                                    �Ȥ�I�ڤ覡�G
                                </th>
                                <td>
                                    <asp:DropDownList runat="server" ID="CUS_PAY_WAY" OnPreRender='checkList' ToolTip='<%# Eval("CUS_PAY_WAY") %>' Width="120"
                                        DataSourceID="sqlCUS_PAY_WAY" DataValueField="TypeCode" DataTextField="TypeDesc">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:CheckBox runat="server" OnPreRender='checkList' ID="CHM_YN" ToolTip='<%# Eval("CHM_YN").ToString().Trim() %>' />����ղ�
                                </td>
                                <td colspan="2">
                                    <asp:CheckBox runat="server" OnPreRender='checkList' ID="DIS_CONTRACT" ToolTip='<%# Eval("DIS_CONTRACT").ToString().Trim() %>' />���o���~�Ѭ�
                                </td>
                                <td  style="color: red">
                                    <asp:CheckBox runat="server"  ID="IMPORT_YN"  />�O�_��J����w��
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox runat="server" OnPreRender='checkList' ID="selfuse" ToolTip='<%# Eval("selfuse").ToString().Trim() %>' />�����Ӧۥ�
                                </td>
                                <td>
                                    <asp:CheckBox runat="server" OnPreRender='checkList' ID="SUPPLYBUY" ToolTip='<%# Eval("SUPPLYBUY").ToString().Trim() %>' />���R�^
                                </td>
                                <td>
                                    <asp:CheckBox runat="server" OnPreRender='checkList' ID="EASYCASE" ToolTip='<%# Eval("EASYCASE").ToString().Trim() %>' />²���«H
                                </td>
                                <td colspan="3" class="nonSpace">
                                    <asp:CheckBox runat="server" OnPreRender='checkList' ID="CONFIRM_25TO31" ToolTip='<%# Eval("CONFIRM_25TO31").ToString().Trim() %>' />�O�_�_���鬰25-31��,
                                    �Ȥ�ڤ�ݧאּ�멳(�L�@��L�վ�)
                                </td>
                            </tr>
                          
                        </table>
                        <table style="width:790px">
                            <tr>
                                <td rowspan="5" style="background-color: darkblue; color: White;width:15px">
                                    ��<br />
                                    ��
                                </td>
                                <td colspan="2">
                                    <asp:TextBox runat="server" ID="MEM" Rows="5" Width="95%" TextMode="MultiLine"  Text='<%# Eval("MEM").ToString().Trim() %>'></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:CheckBox runat="server" OnPreRender='checkList' ID="REM_ORDER" ToolTip='<%# Eval("REM_ORDER").ToString().Trim() %>' />�q�ʳ�
                                    ���B300�U�H�W�w��<ocxControl:ocxDate runat="server" ID="CRD_CON_DATE" Text='<%# Eval("CRD_CON_DATE").ToString().Trim() %>' />�T�{���H�d�ߧ����C(�S��Ȥ�B���ľ��c��Monitor�d�򤧫Ȥᰣ�~)
                                </td>
                            </tr>
                            <tr>                               
                                    <th>
                                        �Ȥ�G
                                    </th>
                                    <td>
                                        <asp:CheckBox runat="server" OnPreRender='checkList' ID="REM_CUST_CONTRACT" ToolTip='<%# Eval("REM_CUST_CONTRACT").ToString().Trim() %>' />�X��
                                        <asp:CheckBox runat="server" OnPreRender='checkList' ID="REM_CUST_AGREEMENT" ToolTip='<%# Eval("REM_CUST_AGREEMENT").ToString().Trim() %>' />��ĳ��
                                        <asp:CheckBox runat="server" OnPreRender='checkList' ID="REM_CUST_INV" ToolTip='<%# Eval("REM_CUST_INV").ToString().Trim() %>' />�o��
                                        <asp:CheckBox runat="server" OnPreRender='checkList' ID="REM_CUST_OTH" ToolTip='<%# Eval("REM_CUST_OTH").ToString().Trim() %>' />��L
                                        <asp:TextBox runat="server" ID="REM_CUST_OTH_DESC" Text='<%# Eval("REM_CUST_OTH_DESC").ToString().Trim() %>'
                                            MaxLength="10" Width="150"></asp:TextBox>�G��VP�ۦ��I
                                    </td>
                            </tr>'
                            <tr>
                                
                                    <th> �����ӡG </th><td><asp:CheckBox runat="server" OnPreRender='checkList' ID="REM_FRC_CONTRACT" ToolTip='<%# Eval("REM_FRC_CONTRACT").ToString().Trim() %>' />�X��
                                    <asp:CheckBox runat="server" OnPreRender='checkList' ID="REM_FRC_AGREEMENT" ToolTip='<%# Eval("REM_FRC_AGREEMENT").ToString().Trim() %>' />��ĳ��
                                    <asp:CheckBox runat="server" OnPreRender='checkList' ID="REM_FRC_INV" ToolTip='<%# Eval("REM_FRC_INV").ToString().Trim() %>' />�o��
                                    <asp:CheckBox runat="server" OnPreRender='checkList' ID="REM_FRC_OTH" ToolTip='<%# Eval("REM_FRC_OTH").ToString().Trim() %>' />��L
                                    <asp:TextBox runat="server" ID="REM_FRC_OTH_DESC" Text='<%# Eval("REM_FRC_OTH_DESC").ToString().Trim() %>'
                                        MaxLength="10" Width="150"></asp:TextBox>�G��VP�ۦ��I
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:CheckBox runat="server" OnPreRender='checkList' ID="REM_FRC_ZEOX" ToolTip='<%# Eval("REM_FRC_ZEOX").ToString().Trim() %>' />�����R�^�ӿը示�e��~���w�T�{
                                </td>
                            </tr>
                            <tr>
                                <th colspan="2">
                                    �|�p�ƶ��G
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="ACC_MEM" Rows="5" Width="95%" Text='<%# Eval("ACC_MEM").ToString().Trim() %>' TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
            <div class="panel" style="height: auto; width: 820px; display: none" id="tabpanel5">
                <asp:Repeater runat="server" ID="rptRemark">
                    <ItemTemplate>
                        <table style="width:90%">
                            <tr>
                                <th>
                                    ���[�`�γƵ���r�G
                                </th>
                                <td colspan="4">
                                    <asp:DropDownList runat="server" ID="Memo"  DataSourceID="sqlRemark" DataTextField="Remark" DataValueField="Seq"
                                        Width="200">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    �дڳ�Ƶ��G
                                </th>
                                <td colspan="4">
                                    <asp:TextBox runat="server" ID="Remark" MaxLength="20" Text='<%# Eval("Remark").ToString().Trim() %>'
                                        TextMode="MultiLine" Rows="5" Width="99%"></asp:TextBox>
                                </td>
                            </tr>
                            
                             <tr>
                                <th>�����b���G</th>
                                <td>                                
                                    <asp:TextBox runat="server" ID="ACCNO" Width="130" MaxLength="20" ReadOnly="true" Text='<%# Eval("ACC_NO") %>'
                                        CssClass="display"></asp:TextBox>
                                </td>
                                 <th>�Ȧ�N���G</th>
                                <td>
                                    <ocxControl:ocxDialog runat="server" ID="BANK_NO" width="100" Text='<%# Eval("BANK_NO") %>'
                                        ControlID="BANK_NAME" FieldName="BANK_NAME" SourceName="ACC18" />
                                </td>
                                <td>
                                <input type="text" id="BANK_NAME" VALUE='<%# Eval("BANK_NAME") %>' style="width:150px"
                                       Class="display" />
                                    
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    �o���Ƶ��G
                                </th>
                                <td colspan="4">
                                    <asp:TextBox runat="server" ID="INVO_REMARK" MaxLength="20" Text='<%# Eval("INVO_REMARK").ToString().Trim() %>'
                                        TextMode="MultiLine" Rows="5" Width="99%"></asp:TextBox>
                                </td>
                            </tr>
                           
                            <tr>
                                <th style="width:20%">
                                    �s�W�H���G
                                </th>
                                <td style="width:10%">
                                    <asp:TextBox runat="server" ID="ADD_User" CssClass="display" Text='<%# Eval("ADD_User").ToString().Trim() %>'
                                        Width="100"></asp:TextBox>
                                </td>
                                <th style="width:20%">
                                    �s�W����G
                                </th>
                                <td style="width:20%">
                                    <asp:TextBox runat="server" ID="ADD_Date" CssClass="display" Text='<%# Eval("ADD_Date").ToString().Trim() %>'
                                        Width="100"></asp:TextBox>
                                </td><td style="width:30%"></td>
                            </tr>
                            <tr>                                
                                <th>
                                    ��s�H���G
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="UPD_User" CssClass="display" Text='<%# Eval("UPD_User").ToString().Trim() %>'
                                        Width="100"></asp:TextBox>
                                </td>
                                <th>
                                    ��s����G
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="UPD_Date" CssClass="display" Text='<%# Eval("UPD_Date").ToString().Trim() %>'
                                        Width="100"></asp:TextBox>
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
                <asp:Button runat="server" ID="btnOTC" OnClick="saveOTC" />
            </span>
        </ContentTemplate>
    </asp:UpdatePanel>
    <span style="display: none">
        <asp:TextBox runat="server" ID="checkObj"></asp:TextBox>
        <asp:TextBox runat="server" ID="checkOTC"></asp:TextBox>
    </span>
</asp:Content>
<asp:Content ID="myPopWindow" ContentPlaceHolderID="PopWindow" runat="server">

    <script language="javascript" type="text/javascript">


    function checkOTC_Insert(){
    if (window.confirm("�O�_�л\Y/N�H")) 
        {
            document.getElementById("<%=this.checkOTC.ClientID %>").value = "Y";
            
        }
        else 
            document.getElementById("<%=this.checkOTC.ClientID %>").value = "";
        
        document.getElementById("<%=this.btnOTC.ClientID %>").click();
        
    }
    function checkObject_Fail() {
        if (window.confirm("���ץ]�t�y�DOTC�겣�z�Ъ��A�A�O�_�T�{�H(�w�q�G�Y���y�@����~���B�ꥻ���B�����ץ�z������OTC�겣�AVP�p�i�ץ�����Ӵ��Ѥ��Ъ��������ڥq�N���N�I�P�޲z���Ъ��~�DOTC�겣)Y/N�H")) 
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
    
    var old = document.getElementById('<%=this.rptBase.Items[0].FindControl("CUST_NO").ClientID %>').value;
    
    function dialogChange(sid, val) {

        if (sid.indexOf("rptBase_ctl00_CUST_NO") != -1)
        {
            if ( trim(val) != trim(old)){                
                document.getElementById("<%=this.reloadCustom.ClientID %>").click();
            }
            old = val;
        }

       
    }

    
    //select main from orix_otc..otc_proced_content where PROCED_APLY_NO='[�qñ���X]' and APLY_COM='OTC'
        

    

     function init() {
         
       
    }
    
  
    
    

    
    function openDetail(strDate, strType, strName) {

    
        window.parent.openPopUpWindow();
    }
    </script>

</asp:Content>
