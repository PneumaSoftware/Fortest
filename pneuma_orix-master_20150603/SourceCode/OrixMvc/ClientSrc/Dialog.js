
       
        function <%=txtDialog.ClientID %>onSelect(r) {
            var g = $('#<%=txtDialog.ClientID %>val').combogrid('grid'); // get datagrid object             
            var r = g.datagrid('getSelected'); // get the selected row
            <%=this.scriptSetValue %>  
        }
        
    
        function <%=txtDialog.ClientID %>onChange() {                
        
                if (<%=txtDialog.ClientID %>i==0)
                {
                    $('#<%=txtDialog.ClientID %>val').combogrid('setText', '<%=this.Text %>');
                    $('#<%=txtDialog.ClientID %>val').combogrid('setValue', '<%=this.Text %>');
                 }   
               
                <%=txtDialog.ClientID %>i++;
                
	            var g = $('#<%=txtDialog.ClientID %>val').combogrid('grid'); // get datagrid object

                var r;
                if (g.datagrid('getRows').length==1)                
                {
                    r=g.datagrid('getData').rows[0];                   
                    var value=$('#<%=txtDialog.ClientID %>val').combogrid('getText');                                        
                    if (trim(r.KEY_NO)==trim(value)){
                        <%=this.scriptSetValue %>                  
                    }
                    else{
                      <%=this.scriptSetValueNull %>                  
                    }
                }   
        }
        
        
        function <%=txtDialog.ClientID %>_init(){
        $('#<%=txtDialog.ClientID %>val').combogrid({                
                mode:'remote',
                fitColumns: true,
                pagination: true, 
                rownumbers: true, 
                collapsible: false,                 
                pageSize: 10,      
                panelWidth:430,
                panelHeight:360,
                value:'<%=this.Text %>',
<% if (this.bolLock()) { %>editable:false,<%}%>              
        <% if (this.SourceName=="OR_CUSTOM") { %>
                url: 'DialogService.ashx?SourceTable=OR_CUSTOM&Item=<%=this.Text %>',
                idField: 'KEY_NO',
                textField:'KEY_NO',                                      
                columns:[[
                    {field:'KEY_NO',title:'客戶代號',width:100},
                    {field:'CUST_SNAME',title:'客戶簡稱',width:150},
                    {field:'CUST_NAME',title:'客戶名稱',width:100},
                    {field:'EN_NAME',title:'客戶英文名稱',width:150}                    
                ]]
         <%}%>    
         <% if (this.SourceName=="OR_BLOC") { %>
                url: 'DialogService.ashx?SourceTable=OR_BLOC&Item=<%=this.Text %>',
                idField: 'KEY_NO',
                textField:'KEY_NO',                                      
                columns:[[
                    {field:'KEY_NO',title:'集團代號',width:100},
                    {field:'BLOC_SNAME',title:'集團簡稱',width:150},
                    {field:'BLOC_NAME',title:'集團名稱',width:100}
                ]]
         <%}%>    
         <% if (this.SourceName=="OR_FRC") { %>
                url: 'DialogService.ashx?SourceTable=OR_FRC&Item=<%=this.Text %>',
                idField: 'KEY_NO',
                textField:'KEY_NO',                                      
                columns:[[
                    {field:'KEY_NO',title:'供應商代碼',width:100},
                    {field:'FRC_SNAME',title:'供應商簡稱',width:150},
                    {field:'FRC_NAME',title:'供應商名稱',width:100}
                ]]
         <%}%> 
         <% if (this.SourceName=="OR_CASE_APLY_BASE") { %>
                url: 'DialogService.ashx?SourceTable=OR_CASE_APLY_BASE&Item=<%=this.Text %>',
                idField: 'KEY_NO',
                textField:'KEY_NO',                                      
                columns:[[
                    {field:'KEY_NO',title:'申請書編號',width:130},
                    {field:'APLY_DATE',title:'申請日期',width:100},
                    {field:'CUST_NO',title:'客戶代號',width:100},                 
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
         
         });
        }
        