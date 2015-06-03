<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/editing.Master" CodeBehind="WF0101.aspx.cs" Inherits="OrixMvc.WF0101" %>
<%@ MasterType VirtualPath="~/Pattern/editing.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxDialog"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDialog.ascx" %>
<%@ Register TagName="ocxTime"  TagPrefix="ocxControl" Src="~/ocxControl/ocxTime.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
        
<asp:Content ContentPlaceHolderID="editingArea" ID="mySearchArea" runat="server">

<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlCASE_SOUR" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'CASE_SOUR' "
    runat="server" >
</asp:sqldatasource>    

<%--<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlINTERVIEW_TOPIC" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="select * from OR3_COND_DEF WHERE TypeField = 'INTERVIEW_TOPIC' And (End_date is null or End_date > GETDATE()) Union select * from OR3_COND_DEF WHERE TypeField = 'INTERVIEW_TOPIC' AND TypeCode = '@TypeCode'"
    runat="server" >
    <SelectParameters>
       <asp:Parameter Name="TypeCode" Type="String" />
   </SelectParameters>
</asp:sqldatasource>--%>

<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlTRANSPORTATION" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="Exec s_ConditionItems 'TRANSPORTATION' "
    runat="server" >
</asp:sqldatasource>    
<span style="display:none">
    <asp:textbox runat="server" ID="SeqNo" Text='<%# Eval("SeqNo") %>' />
    <asp:textbox runat="server" ID="AtmNum" Text='<%# Eval("ATM_NUM") %>' />
    <asp:textbox runat="server" ID="TOPIC" Text='<%# Eval("INTERVIEW_TOPIC") %>' />
    </span>
    <table>
        <tr>
            <th class="nonSpace" style="width:80px">員工代號：</th><td><asp:TextBox runat="server"  CssClass="display" ReadOnly="true" ID="EMP_CODE" Text='<%# Eval("EMP_CODE") %>' width="90"></asp:TextBox></td>
            <th style="width:80px">員工姓名：</th><td><asp:TextBox runat="server" ID="EMP_NAME" ReadOnly="true" CssClass="display" Text='<%# Eval("EMP_NAME") %>' Width="90"></asp:TextBox></td> 
            <th style="width:60px">新舊戶：</th><td><asp:DropDownList ID="CaseSour"  runat="server" Width="94"  OnPreRender='checkList'  ToolTip='<%# Eval("CASE_SOUR") %>'       
                 DataSourceID="sqlCASE_SOUR" DataValueField="TypeCode" DataTextField="TypeDesc" >
                </asp:DropDownList></td>
            <th style="width:110px"></th><td></td>
        </tr>  
        <tr>            
            <th class="nonSpace">客戶代號：</th><td><ocxControl:ocxDialog runat="server" ID="CUST_CODE" width="80" ControlID="CUST_NAME" FieldName="CUST_NAME" SourceName="OR_CUSTOM" Text='<%# Eval("CUST_CODE") %>'/>
            <input type="button" id="btnAddCustom" class="button func" style="width:50px;padding:0px; min-width:50px" value="新增" onclick="openDetail('Custom');" /> 
            </td>
            <th>客戶名稱：</th><td colspan="3"><asp:TextBox runat="server" ID="CUST_NAME" Text='<%# Eval("CUST_NAME") %>'  CssClass="display" width="268"></asp:TextBox></td> 
            <th class="nonSpace">聯絡人：</th><td><asp:TextBox runat="server" ID="CTAC" Text='<%# Eval("CTAC") %>'  MaxLength="5" size="5" Width="84"></asp:TextBox></td>
        </tr>  
        <tr>
            <th>供應商代號：</th><td><ocxControl:ocxDialog runat="server" ID="SUPL_CODE" width="80" ControlID="FRC_NAME" FieldName="FRC_NAME" Text='<%# Eval("SUPL_CODE") %>' SourceName="OR_FRC" />
            <input type="button" id="btnAddFRC" class="button func"  style="width:50px;padding:0px; min-width:50px"  value="新增" onclick="openDetail('Frc');" /> 
            </td>
            <th>供應商名稱：</th><td colspan="3"><asp:TextBox CssClass="display" ReadOnly="true" runat="server" ID="FRC_NAME" size="15"  Text='<%# Eval("FRC_NAME") %>' width="268"></asp:TextBox></td>            
            <th></th><td colspan="3"></td>
        </tr> 
        <tr>
            <th class="nonSpace">拜訪日期：</th>
            <td><ocxControl:ocxDate runat="server" ID="VISIT_DAT" Text='<%# Eval("VISIT_DAT") %>' /></td>            
            <th class="nonSpace">起時：</th>
            <td><ocxControl:ocxTime runat="server" ID="Mes_TStart" Text='<%# Eval("Mes_TStart") %>' /></td>            
            <th class="nonSpace">迄時：</th>
            <td><ocxControl:ocxTime runat="server" ID="Mes_TStop" Text='<%# Eval("Mes_TStop") %>' /></td>            
            <th>預估往來金額(NT/仟元)：</th>
            <td><ocxControl:ocxNumber runat="server" ID="FORECAST_APLY_AMT" MASK="999,999,999" Text='<%# Eval("FORECAST_APLY_AMT") %>' Width="90"/></td>            
        </tr>
        <tr>
            <th>主旨：</th><td colspan="3"><asp:TextBox runat="server" ID="RecTitle" Text='<%# Eval("REC_TITLE") %>' Width="313"></asp:TextBox></td>
            <th class="nonSpace"h>類別：</th>
            <td colspan="3">
                <asp:DropDownList ID="INTERVIEW_TOPIC"  runat="server" Width="94" ClientIDMode="Static">
                </asp:DropDownList> 
            </td>          
        </tr>              
        <tr>
            <th class="nonSpace">訪談內容：</th>
            <td colspan="7"><asp:TextBox runat="server" TextMode="MultiLine" Rows="12" ID="REC_MEAT"   Text='<%# Eval("REC_MEAT") %>' Width="736" ></asp:TextBox></td>
        </tr>  
        <tr> 
            <th>地點：</th><td><asp:TextBox runat="server" ID="RulPlace" Text='<%# Eval("RUL_PLACE") %>' Width="90"></asp:TextBox></td>
            <th >交通工具：</th>
            <td><asp:DropDownList ID="TRANSPORTATION"  runat="server"  Width="94" OnPreRender='checkList' ToolTip='<%# Eval("TRANSPORTATION") %>' 
                 DataSourceID="sqlTRANSPORTATION" DataValueField="TypeCode" DataTextField="TypeDesc" >
                </asp:DropDownList>  </td>     
            <th>交通說明：</th><td><asp:TextBox runat="server" ID="RulOther" Text='<%# Eval("RUL_OTHER") %>' Width="90"></asp:TextBox></td>
            <th>交通費：</th>
            <td><ocxControl:ocxNumber runat="server" ID="APLY_FEE" MASK="999,999,999" Text='<%# Eval("APLY_FEE") %>' Width="90"/></td>    
        </tr>     
    </table>
    
    
                           
</asp:Content>



<asp:Content ID="myPopWindow" ContentPlaceHolderID="PopWindow" runat="server">
 
<iframe id="iframeCustom" src="\WE010\Detail?WF0101=true" style="width:800px;height:600px;"  frameborder="0" scrolling="no" ></iframe>
<iframe id="iframeFrc"  src="WC0101.aspx" style="width:800px;height:550px;"  frameborder="0" scrolling="no"></iframe>
     
<script language="javascript" type="text/javascript">
    $.ajax({
        url: "Common/getTopic",
        data: "{INTERVIEW_TOPIC : '" + $("#ctl00_ctl00_body_masterFormView_editingArea_TOPIC").val() + "'}",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",

        success: function (msg) {
            var listItems = "<option value=' '> </option>";
            var jsonData = msg.Data;
            for (var i = 0; i < jsonData.length; i++) {
                listItems += "<option value='" + jsonData[i].Value + "'>" + jsonData[i].Text + "</option>";
            }
            $("#INTERVIEW_TOPIC").html(listItems);
            $("#INTERVIEW_TOPIC").val($("#ctl00_ctl00_body_masterFormView_editingArea_TOPIC").val());
        },

        error: function (xhr, ajaxOptions, thrownError) {
            alert("ERROR!" + thrownError);
        }
    });

    $("#INTERVIEW_TOPIC").click(function () {
        $("#ctl00_ctl00_body_masterFormView_editingArea_TOPIC").val($("#INTERVIEW_TOPIC option:selected").val());
    });

    function openDetail(strType) {
       
        document.getElementById("iframeCustom").style.display="none";
        document.getElementById("iframeFrc").style.display="none";
        document.getElementById("iframe"+strType).style.display="";
        var obj = document.getElementById("divPopWindow");
        obj.style.width = "800px";
       if (strType == "Custom") {
           obj.style.height = "600px";
       }
       else {
           obj.style.height = "580px";
       }
        obj.style.top="15px";
        obj.style.left="0px";
     
        window.parent.openPopUpWindow();
    }
    
    function setData(strType,value,name)    {
    
        if (strType=="Custom"){
        //    $('#<%=this.CUST_CODE.ClientID %>val').combogrid('setValue', value);
            document.getElementById("<%=this.CUST_CODE.ClientID %>").value = value;
                document.getElementById("<%=this.CUST_NAME.ClientID %>").value=name;
            }
        else{
          //  $('#<%=this.SUPL_CODE.ClientID %>val').combogrid('setValue', value);
            document.getElementById("<%=this.SUPL_CODE.ClientID %>").value = value;
            document.getElementById("<%=this.FRC_NAME.ClientID %>").value=name;
            }
            
         window.parent.closePopUpWindow();
     }


</script>
</asp:Content> 
