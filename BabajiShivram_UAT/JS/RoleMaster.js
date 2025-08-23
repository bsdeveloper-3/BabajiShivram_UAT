// JScript File

//This values comes from Sever side pageload event
var txtName;
var lblError;
var lblId;

function callBackCheckUser()
{
    if(document.getElementById(txtName).value != "" || document.getElementById(txtName).value != null)
    {
        var roleName = document.getElementById(txtName).value;
        UseCallback(roleName, "");
    }
}

function getDataFromServer(result, context)
{
    if(result != "" && result != null)
        document.getElementById(lblError).innerText = "Role name already exist";
    else
        document.getElementById(lblError).innerText = "";
}


function callAssignAR()
{
    if(document.getElementById(lblId).innerText != "" || document.getElementById(lblId).innerText != null)
    {
        var WinSettings = "center:yes; resizable:no; dialogHeight:700px; dialogWidth:950px; status:no; scrollbars:yes";          
        window.showModalDialog("RoleDetails.aspx?lId=" + document.getElementById(lblId).innerText, "", WinSettings);
         window.Open("RoleDetails.aspx?lId=" + document.getElementById(lblId).innerText, "", WinSettings);
    }
}

 function callAssignHRDetails()
{
    if(document.getElementById(lblId).innerText != "" || document.getElementById(lblId).innerText != null)
    {
        var WinSettings = "center:yes; resizable:no; dialogHeight:700px; dialogWidth:950px; status:no; scrollbars:yes";
        window.showModalDialog("HRRoleDetails.aspx?RoleId=" + document.getElementById(lblId).innerText+"&RoleName="+document.getElementById(txtName).value, "", WinSettings);
    }
}