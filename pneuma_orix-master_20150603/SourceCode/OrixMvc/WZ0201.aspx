<%@ Page  Language="C#" EnableEventValidation = "false" AutoEventWireup="true" MasterPageFile="~/Pattern/editing.Master" CodeBehind="WZ0201.aspx.cs" Inherits="OrixMvc.WZ0201" %>
<%@ MasterType VirtualPath="~/Pattern/editing.Master" %>
<%@ Register TagName="ocxDate"  TagPrefix="ocxControl" Src="~/ocxControl/ocxDate.ascx" %>
<%@ Register TagName="ocxNumber"  TagPrefix="ocxControl" Src="~/ocxControl/ocxNumber.ascx" %>
<%@ Register TagName="ocxUpload"  TagPrefix="ocxControl" Src="~/ocxControl/ocxUpload.ascx" %>    
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
        
<asp:Content ContentPlaceHolderID="editingArea" ID="mySearchArea" runat="server">
<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlGroup" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="select USER_ID=' ',USER_NAME='請選擇..' union all select USER_ID,USER_NAME  from OR3_USERS where USER_TYPE='G' "
    runat="server" >  
   
</asp:sqldatasource>

<asp:sqldatasource EnableViewState="true" SelectCommandType="Text" 
    id="sqlFUNC" 
    connectionstring="<%$ ConnectionStrings:myConnectionString%>"
    SelectCommand="select FUNC_ID='',FUNC_NAME='請選擇....' UNION ALL select FUNC_ID,FUNC_NAME=ltrim(rtrim(FUNC_ID))+'：'+FUNC_NAME from OR3_FUNCTION where FUNC_TYPE='P' order by 1"
    runat="server" >  
   
</asp:sqldatasource>
    
    <table>
        <tr>
            <th class="nonSpace">使用者ID/群組代號：</th>
            <td><asp:TextBox runat="server" ID="USER_ID" Text='<%# Eval("USER_ID") %>'  size="10"></asp:TextBox></td>            
            <th class="nonSpace">員工代號：</th>
            <td><asp:TextBox runat="server" ID="EMP_CODE"  Text='<%# Eval("EMP_CODE") %>' size="10"></asp:TextBox></td>
            <th class="nonSpace">使用者姓名：</th>
            <td><asp:TextBox runat="server" ID="USER_NAME"  Text='<%# Eval("USER_NAME") %>'  size="10"></asp:TextBox></td>
        </tr>
        <tr>
            <th class="nonSpace">使用者密碼：</th>
            <td><asp:TextBox runat="server" CssClass="display" ReadOnly="true"  Text='●●●●●●' ID="sUSER_PASS" size="10"></asp:TextBox>
                <asp:UpdatePanel runat="server" ID="upPASS" RenderMode="Inline" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:HiddenField runat="server" id="USER_PASS"  Value='<%# Eval("USER_PASS") %>'  /> 
                    </ContentTemplate>                   
                </asp:UpdatePanel>                
            </td>
            <th>密碼到期日：</th>
            <td colspan="2" ><asp:TextBox runat="server" ID="PWD_DATE"   Text='<%# Eval("PWD_DATE") %>' size="10"></asp:TextBox>
                <input type="button" id="btnPASSChg" class="button func" value="變更密碼" onclick="openDetail();" /> 
            </td>
        </tr>        
        <tr>
            <th class="nonSpace">使用者區分：</th>
            <td><asp:RadioButtonList runat="server" ID="USER_TYPE" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="Type_Change" OnPreRender='checkList' ToolTip='<%# Eval("USER_TYPE") %>'  >                    
                    <asp:ListItem Value="U">使用者</asp:ListItem>
                    <asp:ListItem Value="G">群組</asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <th>所屬群組：</th>
            <td colspan="2" >
                <asp:DropDownList  runat="server" ID="GROUP_ID" 
                OnPreRender='checkList' ToolTip='<%# Eval("GROUP_ID") %>' DataSourceID="sqlGroup" DataValueField="USER_ID" DataTextField="USER_NAME" >
                </asp:DropDownList>                                  
            </td>
            <td colspan="2"><asp:CheckBox runat="server" id="Hint" Text="提示編輯"  OnPreRender='checkList' ToolTip='<%# Eval("HINT") %>'  /></td>
        </tr>   
          
    </table>
    
<!--START Grid for editing!-->
<asp:UpdatePanel runat="server" ID="upGrid" RenderMode="Inline" UpdateMode="Conditional" ChildrenAsTriggers="false">
    <ContentTemplate>    
    
<div class="gridMain " >
    <table cellpadding="0" cellspacing="0" class="title"  style="width:800px">
       <tr>
        <th style="text-align:left;padding-left:5px; word-spacing:3px;color:Red; font-size:13px" width="50%">
            使用者權限設定                    
        </th>
    </table> 
    <div style="padding:0;margin:0; overflow:scroll;width:800px;height:400px;position:relative"  id="editGrid">
        <table cellpadding="0" cellspacing="0" id="tbFunction">         
            <tr>
                <th class="fixCol">編輯</th>
                <th class="fixCol">功能代碼</th>                                
                <th>執行</th>                
                <th>新增</th>
                <th>修改</th>
                <th>刪除</th>                
                <th>匯出</th>
                <th>複製</th>
                <th >按鍵1</th>
                <th>按鍵2</th>
                <th>按鍵3</th>
                <th>按鍵4</th>
                <th>按鍵5</th>
                <th>按鍵6</th>
                <th>按鍵7</th>
                <th>按鍵8</th>
                <th>按鍵9</th>
                <th>按鍵10</th>                
            </tr>
            <asp:Repeater runat="server" ID="rptEdit" >              
                <ItemTemplate>
                    <tr   style="display:<%# Eval("Status").ToString()=="D"?"none":"" %>">
                        <td class="fixCol dgDel">                        
                            <asp:Button ID="btnDel" runat="server"  cssClass="button del"  Text="刪除"  OnCommand="GridFunc_Click"   CommandName='<%# Eval("FUNC_ID") %>' />                                                        
                            <asp:hiddenField runat="server" ID="Status" Value='<%#Eval("STATUS") %>'  />                            
                        </td>
                         <td class="fixCol" >
                            <%# Eval("FUNC_ID") %>：<%# Eval("FUNC_NAME") %><asp:hiddenField runat="server" ID="FUNC_ID" Value='<%#Eval("FUNC_ID") %>'  />   
                        </td>                                 
                        <td><asp:checkBox runat="server" ID="AVAILABLE" Checked='<%#Eval("AVAILABLE") %>' /> </td>
                        <td><asp:checkBox runat="server" ID="CAN_ADD" Checked='<%#Eval("CAN_ADD") %>' /> </td>
                        <td><asp:checkBox runat="server" ID="CAN_UPDATE" Checked='<%#Eval("CAN_UPDATE") %>' /> </td>
                        <td><asp:checkBox runat="server" ID="CAN_DELETE" Checked='<%#Eval("CAN_DELETE") %>' /> </td>
                        <td><asp:checkBox runat="server" ID="CAN_EXPORT" Checked='<%#Eval("CAN_EXPORT") %>' /> </td>
                        <td><asp:checkBox runat="server" ID="CAN_COPY" Checked='<%#Eval("CAN_COPY") %>' /> </td>
                        <td><span style='display:<%#Eval("BTN_NAME_1").ToString().Trim()==""?"none":"" %>'><asp:checkBox runat="server" ID="Enable_1" Checked='<%#Eval("Enable_1") %>'  ToolTip='<%#Eval("BTN_NAME_1")%>' /><%#Eval("BTN_NAME_1")%></span></td>
                        <td><span style='display:<%#Eval("BTN_NAME_2").ToString().Trim()==""?"none":"" %>'><asp:checkBox runat="server" ID="Enable_2" Checked='<%#Eval("Enable_2") %>'  ToolTip='<%#Eval("BTN_NAME_2")%>' /><%#Eval("BTN_NAME_2")%></span></td>
                        <td><span style='display:<%#Eval("BTN_NAME_3").ToString().Trim()==""?"none":"" %>'><asp:checkBox runat="server" ID="Enable_3" Checked='<%#Eval("Enable_3") %>'  ToolTip='<%#Eval("BTN_NAME_3")%>' /><%#Eval("BTN_NAME_3")%></span></td>
                        <td><span style='display:<%#Eval("BTN_NAME_4").ToString().Trim()==""?"none":"" %>'><asp:checkBox runat="server" ID="Enable_4" Checked='<%#Eval("Enable_4") %>'  ToolTip='<%#Eval("BTN_NAME_4")%>' /><%#Eval("BTN_NAME_4")%></span></td>
                        <td><span style='display:<%#Eval("BTN_NAME_5").ToString().Trim()==""?"none":"" %>'><asp:checkBox runat="server" ID="Enable_5" Checked='<%#Eval("Enable_5") %>'  ToolTip='<%#Eval("BTN_NAME_5")%>' /><%#Eval("BTN_NAME_5")%></span></td>
                        <td><span style='display:<%#Eval("BTN_NAME_6").ToString().Trim()==""?"none":"" %>'><asp:checkBox runat="server" ID="Enable_6" Checked='<%#Eval("Enable_6") %>'  ToolTip='<%#Eval("BTN_NAME_6")%>' /><%#Eval("BTN_NAME_6")%></span></td>
                        <td><span style='display:<%#Eval("BTN_NAME_7").ToString().Trim()==""?"none":"" %>'><asp:checkBox runat="server" ID="Enable_7" Checked='<%#Eval("Enable_7") %>'  ToolTip='<%#Eval("BTN_NAME_7")%>' /><%#Eval("BTN_NAME_7")%></span></td>
                        <td><span style='display:<%#Eval("BTN_NAME_8").ToString().Trim()==""?"none":"" %>'><asp:checkBox runat="server" ID="Enable_8" Checked='<%#Eval("Enable_8") %>'  ToolTip='<%#Eval("BTN_NAME_8")%>' /><%#Eval("BTN_NAME_8")%></span></td>
                        <td><span style='display:<%#Eval("BTN_NAME_9").ToString().Trim()==""?"none":"" %>'><asp:checkBox runat="server" ID="Enable_9" Checked='<%#Eval("Enable_9") %>'  ToolTip='<%#Eval("BTN_NAME_9")%>' /><%#Eval("BTN_NAME_9")%></span></td>
                        <td><span style='display:<%#Eval("BTN_NAME_10").ToString().Trim()==""?"none":"" %>'><asp:checkBox runat="server" ID="Enable_10" Checked='<%#Eval("Enable_10") %>'  ToolTip='<%#Eval("BTN_NAME_10")%>' /><%#Eval("BTN_NAME_10")%></span></td>                                                
                    </tr>
                </ItemTemplate> 
            </asp:Repeater>
             <tr>
                        <td class="fixCol">                        
                           <asp:Button ID="btnAdd" runat="server"  cssClass="button dadd"  Text="新增" OnCommand="GridFunc_Click" CommandName="Add" />
                        </td>
                        <td class="fixCol" >             
                            <asp:UpdatePanel runat="server" ID="upFuncID" RenderMode="Inline" UpdateMode="Conditional">
                                <ContentTemplate>
                            <asp:DropDownList  runat="server" ID="addFUNC_ID" OnSelectedIndexChanged="FuncID_Change" AutoPostBack="true" DataSourceID="sqlFunc" DataValueField="FUNC_ID" DataTextField="FUNC_NAME"    ></asp:DropDownList>                                                              
                                </ContentTemplate>
                            </asp:UpdatePanel> 
                        </td>                                 
                        <td><asp:checkBox runat="server" ID="addAVAILABLE"   /> </td>
                        <td><asp:checkBox runat="server" ID="addCAN_ADD"   /> </td>
                        <td><asp:checkBox runat="server" ID="addCAN_UPDATE"  /> </td>
                        <td><asp:checkBox runat="server" ID="addCAN_DELETE" /> </td>
                        <td><asp:checkBox runat="server" ID="addCAN_EXPORT" /> </td>
                        <td><asp:checkBox runat="server" ID="addCAN_COPY"  /> </td> 
                        <td ><span id="addSpan_1" runat="server" style="display:none"><asp:checkBox runat="server" ID="addEnable_1" /><asp:TextBox runat="server" Width="40" id="txtEnable_1" BackColor="Transparent" ></asp:TextBox> </span></td>
                        <td ><span id="addSpan_2" runat="server" style="display:none"><asp:checkBox runat="server" ID="addEnable_2" /> <asp:TextBox runat="server" Width="40" id="txtEnable_2" BackColor="Transparent" ></asp:TextBox> </span></td>
                        <td ><span id="addSpan_3" runat="server" style="display:none"><asp:checkBox runat="server" ID="addEnable_3" /> <asp:TextBox runat="server" Width="40" id="txtEnable_3" BackColor="Transparent" ></asp:TextBox> </span></td>
                        <td ><span id="addSpan_4" runat="server" style="display:none"><asp:checkBox runat="server" ID="addEnable_4" /> <asp:TextBox runat="server" Width="40" id="txtEnable_4" BackColor="Transparent" ></asp:TextBox> </span></td>
                        <td ><span id="addSpan_5" runat="server" style="display:none"><asp:checkBox runat="server" ID="addEnable_5" />  <asp:TextBox runat="server" Width="40" id="txtEnable_5" BackColor="Transparent" ></asp:TextBox> </span></td>
                        <td ><span id="addSpan_6" runat="server" style="display:none"><asp:checkBox runat="server" ID="addEnable_6"  /><asp:TextBox runat="server" Width="40" id="txtEnable_6"  BackColor="Transparent"></asp:TextBox> </span></td>
                        <td ><span id="addSpan_7" runat="server" style="display:none"><asp:checkBox runat="server" ID="addEnable_7"  /> <asp:TextBox runat="server" Width="40" id="txtEnable_7"  BackColor="Transparent"></asp:TextBox> </span></td>
                        <td ><span id="addSpan_8" runat="server" style="display:none"><asp:checkBox runat="server" ID="addEnable_8"  /> <asp:TextBox runat="server" Width="40" id="txtEnable_8" BackColor="Transparent" ></asp:TextBox> </span></td>
                        <td ><span id="addSpan_9" runat="server" style="display:none"><asp:checkBox runat="server" ID="addEnable_9"  /> <asp:TextBox runat="server" Width="40" id="txtEnable_9" BackColor="Transparent" ></asp:TextBox> </span></td>
                        <td ><span id="addSpan_10" runat="server" style="display:none"><asp:checkBox runat="server" ID="addEnable_10" /><asp:TextBox runat="server" Width="40" id="txtEnable_10" BackColor="Transparent"></asp:TextBox> </span></td>
                    </tr>
        </table>
    </div>     
</div> 

        <span style="display:none">
            <asp:HiddenField runat="server" ID="FUNC_ID" Value="" />
        </span>
        
    </ContentTemplate> 
</asp:UpdatePanel>
<!--START Grid for editing!-->
    
                           
</asp:Content>


<asp:Content ID="myPopWindow" ContentPlaceHolderID="PopWindow" runat="server">
      <!--for 假日設定!-->  
    <asp:UpdatePanel ID="upHolidayDetail" runat="server" UpdateMode="Conditional" >
        <ContentTemplate>         
                   
        <table border="0" cellpadding="2" cellspacing="1"  class="tbInside" >      
             <tr>
                <td class="PopupTitle" colspan="2">        
                    變更密碼                             
                </td>
            </tr>            
            <tr>
                <th class="nonSpace" >新密碼：</th>
                <td >
                    <asp:TextBox runat="server" ID="USER_PASSN" MaxLength="30"  TextMode="Password"></asp:TextBox>
                </td>        	    
            </tr> 
            <tr>
                <th class="nonSpace" >確認密碼：</th>
                <td >
                    <asp:TextBox runat="server" ID="USER_PASSNS"  MaxLength="30" TextMode="Password"></asp:TextBox>                   
                </td>        	    
            </tr> 
            <tr><td colspan="2" style="text-align:right">               
                  <asp:Button  runat="server" ID="btnSave" CssClass="button func" Text="儲存並關閉" OnCommand="PopUp_Command"  CommandName="ChangePASS" />                                  
            </td></tr>                         
        </table>
        
        </ContentTemplate>
    </asp:UpdatePanel>  

     
<script language="javascript" type="text/javascript">
    function openDetail() {

        var obj = document.getElementById("divPopWindow");
        obj.style.width = "300px";
        obj.style.height = "150px";

        obj.style.top = "100px";
        obj.style.left = "200px";

        window.parent.openPopUpWindow();
    }
</script>
</asp:Content> 
