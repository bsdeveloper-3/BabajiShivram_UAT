// JScript File
//this js file used for RoleDetails


    var hidRowIndex;
    var grdUserRight;

    function setRowNum()
    {
            var table = document.getElementById(grdUserRightAll) ;
            
            if (!document.getElementsByTagName || !document.createTextNode) return;
                var rows = document.getElementById(grdUserRightAll).getElementsByTagName('tr');
            for (i = 0; i < rows.length ; i++) 
            {
                rows[i].onclick = function() 
                {
                    document.getElementById(hidRowIndex).value = (this.rowIndex - 1);
                        return true;
                        
                    //alert(document.getElementById('<%=hidRowIndex.ClientID %>').value);
                    // var row = table.rows[this.rowIndex];
                    // alert(this.rowIndex)
                }
            }
            return false;
     }
         
    function setRowNum1()
    {
                        
        var table = document.getElementById('ctl00_ContentPlaceHolder1_grdUserRight') ;
                                        
        if (!document.getElementsByTagName || !document.createTextNode) 
        {
            return;
        }
        
        var rows = document.getElementById('ctl00_ContentPlaceHolder1_grdUserRight').getElementsByTagName('tr');
                                
        for (i = 0; i < rows.length ; i++) 
        {
            rows[i].onclick = function() 
            {
                document.getElementById('ctl00_ContentPlaceHolder1_hidRowIndex').value = (this.rowIndex - 1);
                
                return true;
                // var row = table.rows[this.rowIndex];
                // alert(this.rowIndex)
            }
        }
        return false;
     }

    