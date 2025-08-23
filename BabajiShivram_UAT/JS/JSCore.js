// JScript File

// Generates an unique identifier.
function GetUploadId()
{
    var uniqueIDBegin = Math.round(Math.random() * 100000);    
        
    var uniqueIDEnd = (new Date).getTime() % 10000000000;    
    
    return uniqueIDBegin.toString() + uniqueIDEnd.toString();
}

// Creates a new window with specified URL and size.
function CreateProgressWindow(progressPage, UploadId, width, height)
{
    var offsetX = (screen.width - width) / 2;
    var offsetY = (screen.height - height) / 2;
    
    var windowFeatures = "resizable=yes,width=" + width + ",height=" + height + ",left=" + offsetX + ",top=" + offsetY;
    
    // Add an UploadId field to queri string. 
    // We use a value of "UploadId" parameter that is specified in 
    // <EasyAlgo.EAUpload>/<Environment> section of the web.config file 
    // for defining a name of query string parameter. In our case it is "UploadId".    
    if(progressPage.indexOf("?")==-1)
        progressPage += '?UploadId=' + UploadId;
    else
        progressPage += '&UploadId=' + UploadId;
    
    return window.open(progressPage, "progressWindow_" + UploadId, windowFeatures);
}

// Refreshes query string and adds an unique identifier.
function RefreshQueryString(formAction, UploadId)
{
    // Check entry multiple parameters into query string.
    if(formAction.indexOf("?")==-1)
    {
        formAction += "?UploadId=" + UploadId;
    }
    else
    {
        // Replase old UploadId value for second and next uploads.
        if (formAction.indexOf("UploadId")!=-1)
        {                                                    
            var parameterPosition = formAction.indexOf("UploadId");
            
            if (formAction.indexOf("&", parameterPosition)==-1)
            {
                formAction = formAction.slice(0, parameterPosition + "UploadId=".length) + UploadId;
            }
            else
            {
                var andSymPosition = formAction.indexOf("&", parameterPosition);
                
                formAction = formAction.slice(0, parameterPosition + "UploadId=".length) + UploadId + formAction.slice(andSymPosition);
            }
        }
        else
        {
            formAction += '&UploadId=' + UploadId;
        }
    }
    
    return formAction;
}

function IsStopCommandSupported()
{
    if (window.stop || window.document.execCommand)
    {
        return true;
    }
    else
    {
        return false;
    }

}

function JSONtoJSObject(jsonString)
{
    jsonString = jsonString.replace(/\n|\r/g, "");
    
    return eval("(" + jsonString + ")");    
}