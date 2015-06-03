<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ocxDialog.ascx.cs" Inherits="OrixMvc.ocxControl.ocxDialog" %>
<style>
 .combo-text{ display:none}
 .combo{ border:none;margin-left:-10px;margin-top:-5px}
 .dialogslock span.combo{display:none;}
</style>


<span class="dialog<%=this.bolLock()?"slock":"" %>">
<span class="spandate">
<asp:TextBox runat="server" id="txtDialog" style="border:none" ></asp:TextBox>
    <span class="combo" style="width:20px">
        <span>
            <span class="combo-arrow" id="arrow<%=txtDialog.ClientID %>" style="width:20px;height:20px;cursor:pointer;" onclick="<%=txtDialog.ClientID %>_init();this.parentNode.parentNode.style.display='none';$('#<%=txtDialog.ClientID %>val').combogrid('showPanel');">
            </span>
        </span>
    </span>
<input type="text" id="<%=txtDialog.ClientID %>val" name="<%=txtDialog.ClientID %>val"  style="width:18px;height:20px;display:none" data-options="onShowPanel:<%=txtDialog.ClientID %>onShow,onBeforeLoad:<%=txtDialog.ClientID %>onLoad,onLoadSuccess:<%=txtDialog.ClientID %>onChange,onSelect:<%=txtDialog.ClientID %>onSelect" /> 
</span>
</span> 
    
    <script type="text/javascript">    
        
        var <%=txtDialog.ClientID %>old="";    
        var <%=txtDialog.ClientID %>leave=false;
        var <%=txtDialog.ClientID %>show=false;
        var <%=txtDialog.ClientID %>post=false;
        var <%=txtDialog.ClientID %>time;

        document.getElementById("<%=this.txtDialog.ClientID %>").onfocusin = function()
    {        
       <%=this.txtDialog.ClientID %>focusin();
    }
    
         function <%=this.txtDialog.ClientID %>focusin() {
          
            
            var sp=document.getElementById("arrow<%=txtDialog.ClientID %>");           
            if (sp.parentNode.parentNode.style.display=="")
            {
                <%=txtDialog.ClientID %>_init();
                sp.parentNode.parentNode.style.display="none";
            }
    }
    
       
       
        
        function <%=txtDialog.ClientID %>onLoad(param){
                   
            
            var txt=document.getElementById("<%=txtDialog.ClientID %>").value.toUpperCase();            
            var p=param;
            if ("<%=txtDialog.ClientID %>".indexOf("rptBase_ctl00_EMP_CODE")!=-1)
            {                
                txt=$('input[id*=rptBase_ctl00_DEPT_CODE]').val()+","+txt;
            }
            
            
             if ("<%=txtDialog.ClientID %>".indexOf("ctl00_ctl00_body_masterFormView_editingArea_EMP_CODE")!=-1 )
            {   
                if($('input[id*=ctl00_ctl00_body_masterFormView_editingArea_DEPT_CODE]').length>0)             
                    txt=$('input[id*=ctl00_ctl00_body_masterFormView_editingArea_DEPT_CODE]').val()+","+txt;
            }
            
            if ("<%=txtDialog.ClientID %>".indexOf("rptBase_ctl00_EMP_CODE")!=-1)
            {                
                txt=$('input[id*=rptBase_ctl00_DEPT_CODE]').val()+","+txt;
            }
            
            if ("<%=txtDialog.ClientID %>".indexOf("rptBase_ctl00_MAST_CON_NO")!=-1)
            {                
                txt=$('input[id*=rptBase_ctl00_DEPT_CODE]').val()+","+$('input[id*=rptBase_ctl00_CUST_NO]').val()+","+txt;
            }
             
            if ("<%=txtDialog.ClientID %>".indexOf("rptBase_ctl00_CUR_QUOTA_APLY_NO")!=-1)
            {                
                txt=$('input[id*=rptBase_ctl00_CUST_NO]').val()+","+txt;
            }
            
            param.q = txt;          
                       
        }
        
        function <%=txtDialog.ClientID %>onShow(param){
       
             
                
            <%=txtDialog.ClientID %>show=true;
            window.clearTimeout(<%=txtDialog.ClientID %>time);
            
            var txt=document.getElementById("<%=txtDialog.ClientID %>").value.toUpperCase();   
            if ("<%=txtDialog.ClientID %>".indexOf("rptBase_ctl00_EMP_CODE")!=-1 )
            {                
                txt=$('input[id*=rptBase_ctl00_DEPT_CODE]').val()+","+txt;
            }
            
             if ("<%=txtDialog.ClientID %>".indexOf("ctl00_ctl00_body_masterFormView_editingArea_EMP_CODE")!=-1 )
            {                
                 if($('input[id*=ctl00_ctl00_body_masterFormView_editingArea_DEPT_CODE]').length>0)             
                    txt=$('input[id*=ctl00_ctl00_body_masterFormView_editingArea_DEPT_CODE]').val()+","+txt;
            }
            
            if ("<%=txtDialog.ClientID %>".indexOf("rptBase_ctl00_MAST_CON_NO")!=-1)
            {                
                txt=$('input[id*=rptBase_ctl00_DEPT_CODE]').val()+","+$('input[id*=rptBase_ctl00_CUST_NO]').val()+","+txt;
            }
             
            if ("<%=txtDialog.ClientID %>".indexOf("rptBase_ctl00CUR_QUOTA_APLY_NO")!=-1)
            {                
                txt=$('input[id*=rptBase_ctl00_CUST_NO]').val()+","+txt;
            }
             
             
            var p=param;
            if (p!=null)
                p.q = txt;          
            
           <%=txtDialog.ClientID %>time= $('#<%=txtDialog.ClientID %>val').combogrid('grid').datagrid('reload');    
           
           
           
        }

        function <%=txtDialog.ClientID %>loadDialog(){
            
            
                
            window.clearTimeout(<%=txtDialog.ClientID %>time);
            var txt=document.getElementById("<%=txtDialog.ClientID %>").value.toUpperCase();      
            
             $('#<%=txtDialog.ClientID %>val').combogrid('setText', "");
             $('#<%=txtDialog.ClientID %>val').combogrid('setValue', "");
             
                    
            <%=txtDialog.ClientID %>leave=true ;
            
            if (txt=="")    
            {
                <%=this.scriptSetValueNull %>     
               <%=txtDialog.ClientID %>leave=true ;
            }
           // else
            <%=txtDialog.ClientID %>time=window.setTimeout("<%=txtDialog.ClientID %>setTime()","100");
            
        }
        
        function  <%=txtDialog.ClientID %>setTime(){
        
           
                
        var txt=document.getElementById("<%=txtDialog.ClientID %>").value.toUpperCase();     
            $('#<%=txtDialog.ClientID %>val').combogrid('grid').datagrid('reload'); 
           <%=txtDialog.ClientID %>old=txt;
          // alert("load");
         //  alert(<%=txtDialog.ClientID %>show);
                             
  
        }
      
        var <%=txtDialog.ClientID %>i=0;
        function <%=txtDialog.ClientID %>onSelect(r) {
           
             
                
            var g = $('#<%=txtDialog.ClientID %>val').combogrid('grid'); // get datagrid object             
            var r = g.datagrid('getSelected'); // get the selected row
            
            
            <%=this.scriptSetValue %>                           
            
             try {
                    dialogChange('<%=txtDialog.ClientID %>',document.getElementById("<%=txtDialog.ClientID %>").value.toUpperCase());     
            }
            catch (err) { } 
            
           
            
        }
        
    
        function <%=txtDialog.ClientID %>onChange() {                
          
              
               
                if (<%=txtDialog.ClientID %>post)
                {
                    <%=txtDialog.ClientID %>post=false;
                    
                    return;
                }
                
                var txt=document.getElementById("<%=txtDialog.ClientID %>").value.toUpperCase(); 
               
                if (<%=txtDialog.ClientID %>i==0)
                {
                    $('#<%=txtDialog.ClientID %>val').combogrid('setText', txt);
                    $('#<%=txtDialog.ClientID %>val').combogrid('setValue', txt);
                 }   
              
                <%=txtDialog.ClientID %>i++;
                
	            var g = $('#<%=txtDialog.ClientID %>val').combogrid('grid'); // get datagrid object

                var r;
                if (g.datagrid('getRows').length>=1 && g.datagrid('getRows').length<=2)                
                {
                    r=g.datagrid('getData').rows[0];                   
                    var value=document.getElementById("<%=txtDialog.ClientID %>").value.toUpperCase();                                        
                    if (trim(r.KEY_NO)==trim(value) ){
                        <%=this.scriptSetValue %>                  
                    }
                    else{
                     if (g.datagrid('getRows').length>1){
                            r=g.datagrid('getData').rows[1];                   
                            value=document.getElementById("<%=txtDialog.ClientID %>").value.toUpperCase();                                        
                            if (trim(r.KEY_NO)==trim(value) ){
                                <%=this.scriptSetValue %>                  
                            }
                            else
                                <%=this.scriptSetValueNull %>
                        }                  
                    }
                } 
                     
                 
                                
                if (<%=txtDialog.ClientID %>show==false && <%=txtDialog.ClientID %>leave && g.datagrid('getRows').length==0)
                {
                      <%=this.scriptSetValueNull %>                  
                }
                
                if (<%=txtDialog.ClientID %>show==false && <%=txtDialog.ClientID %>leave && g.datagrid('getRows').length!=0 )                
                {
                    r=g.datagrid('getData').rows[0];                   
                    var value1=document.getElementById("<%=txtDialog.ClientID %>").value.toUpperCase();                                        
                    if (trim(r.KEY_NO)!=trim(value1)){
                      <%=this.scriptSetValueNull %>  
                    }                    
                } 
                
                <%=txtDialog.ClientID %>leave=false;
                <%=txtDialog.ClientID %>show=false;   
                
                g.datagrid('clearSelections');  
                
                
                
                 try {
                    dialogChange('<%=txtDialog.ClientID %>',document.getElementById("<%=txtDialog.ClientID %>").value.toUpperCase());     
            }
            catch (err) { }      
              
        }
        
        var bol<%=txtDialog.ClientID %>=false;
        function <%=txtDialog.ClientID %>_init(){
        
        $('#<%=txtDialog.ClientID %>val').combogrid({                
                mode:'remote',                
                fitColumns: false,
                frozenColumns:2,
                singleSelect:false,
                pagination: true, 
                rownumbers: true, 
                collapsible: false,                 
                pageSize: 10,      
                panelWidth:500,
                panelHeight:360,
                value:'<%=this.Text %>',
<% if (this.bolLock()) { %>editable:false,<%}%>              
        <% if (this.SourceName=="OR_CUSTOM") { %>
                url: 'DialogService.ashx?SourceTable=OR_CUSTOM&Item=<%=this.Text %>',
                idField: 'KEY_NO',
                textField:'KEY_NO',                                      
                columns:[[
                    {field:'KEY_NO',title:'客戶代號',width:120},
                    {field:'CUST_SNAME',title:'客戶簡稱',width:150},
                    {field:'CUST_NAME',title:'客戶名稱',width:280},
                    {field:'EN_NAME',title:'客戶英文名稱',width:200}                    
                ]]
         <%}%>    
         <% if (this.SourceName=="OR_BLOC") { %>
                url: 'DialogService.ashx?SourceTable=OR_BLOC&Item=<%=this.Text %>',
                idField: 'KEY_NO',
                textField:'KEY_NO',                                      
                columns:[[
                    {field:'KEY_NO',title:'集團代號',width:120},
                    {field:'BLOC_SNAME',title:'集團簡稱',width:150},
                    {field:'BLOC_NAME',title:'集團名稱',width:200}
                ]]
         <%}%>    
         <% if (this.SourceName=="OR_FRC") { %>
                url: 'DialogService.ashx?SourceTable=OR_FRC&Item=<%=this.Text %>',
                idField: 'KEY_NO',
                textField:'KEY_NO',                                      
                columns:[[
                    {field:'KEY_NO',title:'供應商代碼',width:120},
                    {field:'FRC_SNAME',title:'供應商簡稱',width:150},
                    {field:'FRC_NAME',title:'供應商名稱',width:200}
                ]]
         <%}%> 
         
      
         <% if (this.SourceName=="OR_CASE_APLY_BASE" || this.SourceName=="OR_CASE_APLY_BASE_WD010" || this.SourceName=="OR_CASE_APLY_BASE_WD040" ) { %>
                url: 'DialogService.ashx?SourceTable=<%=this.SourceName %>&Item=<%=this.Text %>',
                idField: 'KEY_NO',
                textField:'KEY_NO',                                      
                columns:[[
                    {field:'KEY_NO',title:'申請書編號',width:130},
                    {field:'CUR_STS',title:'目前狀況',width:100},
                    {field:'APLY_DATE',title:'申請日期',width:100},
                    {field:'CUST_NO',title:'客戶代號',width:100},                 
                    {field:'CUST_SNAME',title:'客戶簡稱',width:150}
                ]]
         <%}%>  
        
         <% if (this.SourceName=="OR_RECV_PAPER") { %>
                url: 'DialogService.ashx?SourceTable=OR_RECV_PAPER&Item=<%=this.Text %>',
                idField: 'KEY_NO',
                textField:'KEY_NO',                                      
                columns:[[
                    {field:'KEY_NO',title:'申請書編號',width:130},
                    {field:'APLY_DATE',title:'申請日期',width:100},
                    {field:'CUST_NO',title:'客戶代號',width:120},                 
                    {field:'CUST_SNAME',title:'客戶簡稱',width:150}
                ]]
         <%}%>  
         
          <% if (this.SourceName=="OR_EMP") { %>
                url: 'DialogService.ashx?SourceTable=OR_EMP&Item=<%=this.Text %>',
                idField: 'KEY_NO',
                textField:'KEY_NO',                                      
                columns:[[
                    {field:'KEY_NO',title:'員工代號',width:100},
                    {field:'EMP_NAME',title:'中文姓名',width:120},
                    {field:'EMP_ENAME',title:'英文名',width:120}                
                    
                ]]
         <%}%>  
          <% if (this.SourceName=="V_OR_EMP") { %>
                url: 'DialogService.ashx?SourceTable=OR_EMP&Item=<%=this.Text %>',
                idField: 'KEY_NO',
                textField:'KEY_NO',                                      
                columns:[[
                    {field:'KEY_NO',title:'員工代號',width:100},
                    {field:'EMP_NAME',title:'中文姓名',width:120},
                    {field:'EMP_ENAME',title:'英文名',width:120}                
                    
                ]]
         <%}%>  
         <% if (this.SourceName=="ACC18") { %>
                url: 'DialogService.ashx?SourceTable=ACC18&Item=<%=this.Text %>',
                idField: 'KEY_NO',
                textField:'KEY_NO',                                      
                columns:[[
                    {field:'KEY_NO',title:'銀行代碼',width:90},
                    {field:'BANK_NAME',title:'銀行名稱',width:200}
                ]]
         <%}%>  
         <% if (this.SourceName=="OR_APLY_WA060") { %>
                url: 'DialogService.ashx?SourceTable=ACC18&Item=<%=this.Text %>',
                idField: 'KEY_NO',
                textField:'KEY_NO',                                      
                columns:[[
                    {field:'KEY_NO',title:'申請書編號',width:90},
                    {field:'CON_SEQ_NO',title:'契約編號',width:200},
                    {field:'APLY_DATE',title:'申請日期',width:200},
                    {field:'CUR_STS',title:'',width:0}
                ]]
         <%}%>  
          <% if (this.SourceName=="SALES") { %>
                url: 'DialogService.ashx?SourceTable=SALES&Item=<%=this.Text %>',
                idField: 'KEY_NO',
                textField:'KEY_NO',                                      
                columns:[[
                    {field:'KEY_NO',title:'業務員代號',width:90},
                    {field:'SALES1',title:'業務員姓名',width:200}
                   
                ]]
         <%}%>  
          <% if (this.SourceName=="OR_DEPT") { %>
                url: 'DialogService.ashx?SourceTable=OR_DEPT&Item=<%=this.Text %>',
                idField: 'KEY_NO',
                textField:'KEY_NO',                                      
                columns:[[
                    {field:'KEY_NO',title:'部門代號',width:90},
                    {field:'DEPT_NAME',title:'部門名稱',width:200}
                   
                ]]
         <%}%>  
          <% if (this.SourceName=="OR_MERG_MAIL") { %>
                url: 'DialogService.ashx?SourceTable=OR_MERG_MAIL&Item=<%=this.Text %>',
                idField: 'KEY_NO',
                textField:'KEY_NO',                                      
                columns:[[
                    {field:'KEY_NO',title:'編號',width:90},
                    {field:'MMAIL_NAME',title:'名稱',width:200}
                   
                ]]
         <%}%>  
          <% if (this.SourceName=="OR_AUD_LVL_NAME") { %>
                url: 'DialogService.ashx?SourceTable=OR_AUD_LVL_NAME&Item=<%=this.Text %>',
                idField: 'KEY_NO',
                textField:'KEY_NO',                                      
                columns:[[
                    {field:'KEY_NO',title:'授權代號',width:90},
                    {field:'AUD_LVL_NAME',title:'授權名稱',width:200}
                   
                ]]
         <%}%>  
          <% if (this.SourceName=="OR_CASE_TYPE") { %>
                url: 'DialogService.ashx?SourceTable=OR_CASE_TYPE&Item=<%=this.Text %>',
                idField: 'KEY_NO',
                textField:'KEY_NO',                                      
                columns:[[
                    {field:'KEY_NO',title:'案件類別代號',width:90},
                    {field:'CASE_TYPE_NAME',title:'案件類別名稱',width:200},
                    {field:'Type',title:'類型1',width:100}
                   
                ]]
         <%}%>  
          <% if (this.SourceName=="OR_CASE_CAL") { %>
                url: 'DialogService.ashx?SourceTable=OR_CASE_CAL&Item=<%=this.Text %>',
                idField: 'KEY_NO',
                textField:'KEY_NO',                                      
                columns:[[
                    {field:'KEY_NO',title:'審查代號',width:90},
                    {field:'CAL_NAME',title:'審查名稱',width:200}
                   
                ]]
         <%}%>  
         
          <% if (this.SourceName=="OR_ATCH_CODE") { %>
                url: 'DialogService.ashx?SourceTable=OR_ATCH_CODE&Item=<%=this.Text %>',
                idField: 'KEY_NO',
                textField:'KEY_NO',                                      
                columns:[[
                    {field:'KEY_NO',title:'附件代碼',width:90},
                    {field:'ATCH_NAME',title:'附件名稱',width:200}
                   
                ]]
         <%}%>  
         
         <% if (this.SourceName=="OLD_APLY_NO") { %>
                url: 'DialogService.ashx?SourceTable=OLD_APLY_NO&Item=<%=this.Text %>',
                idField: 'KEY_NO',
                textField:'KEY_NO',                                      
                columns:[[
                    {field:'KEY_NO',title:'申請書編號',width:120},
                    {field:'OBJ_CODE',title:'標的物代號',width:120},
                    {field:'PROD_NAME',title:'品名',width:280},
                    {field:'MAC_NO',title:'機號',width:180}
                  
                ]]
         <%}%>  
         
         <% if (this.SourceName=="OR_BANK_AMT") { %>
                url: 'DialogService.ashx?SourceTable=OR_BANK_AMT&Item=<%=this.Text %>',
                idField: 'KEY_NO',
                textField:'KEY_NO',                                      
                columns:[[
                    {field:'SEQNO',title:'序號',hidden:true,width:90},
                    {field:'KEY_NO',title:'銀行代號',width:90},
                    {field:'BANK_TYPE',title:'銀行類別',width:90},
                    {field:'BANK_NAME',title:'銀行名稱',width:90},
                    {field:'CRD_AMT',title:'授信額度',width:100},
                    {field:'USED_CREDIT',title:'已使用授信額度',width:100},
                    {field:'REST_CREDIT',title:'剩餘額度',width:100},
                    {field:'CRD_DATE_TO',title:'授信到期日',width:100},
                    {field:'LONG_SHORT_LOAN',title:'長短借',width:50}
                    
                  
                ]]
         <%}%>  
         
         <% if (this.SourceName=="OR3_MASTER_CONTRACT") { %>
                url: 'DialogService.ashx?SourceTable=OR3_MASTER_CONTRACT&Item=<%=this.Text %>',
                idField: 'KEY_NO',
                textField:'KEY_NO',                                      
                columns:[[
                    {field:'KEY_NO',title:'主約編號',width:150}
                ]]
         <%}%>  
         
         <% if (this.SourceName=="OR3_QUOTA_APLY_BASE") { %>
                url: 'DialogService.ashx?SourceTable=OR3_QUOTA_APLY_BASE&Item=<%=this.Text %>',
                idField: 'KEY_NO',
                textField:'KEY_NO',                                      
                columns:[[
                    {field:'KEY_NO',title:'現額度編號',width:150}
                ]]
         <%}%>  
         
         <% if (this.SourceName=="OR_APLY_WH020") { %>
                url: 'DialogService.ashx?SourceTable=OR_APLY_WH020&Item=<%=this.Text %>',
                idField: 'KEY_NO',
                textField:'KEY_NO',                                      
                columns:[[
                    {field:'KEY_NO',title:'契約編號',width:150}
                ]]
         <%}%>  
         
         });
        }
        
       
        
       
        
        
    </script>

