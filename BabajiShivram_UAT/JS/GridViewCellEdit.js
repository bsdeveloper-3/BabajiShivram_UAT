//hide label and display textbox
function HideLabel(labelId, e, txtId)
{ 
	document.getElementById(labelId).style.display = "none"; 
	ShowTextBox(txtId, e, labelId);
} 

//show label
function ShowLabel(labelId, e, txtId)
{ 
	document.getElementById(labelId).style.display = ""; 
} 

//show textbox
function ShowTextBox(txtId, e, labelId)
{ 
	var txt = document.getElementById(txtId)
	txt.style.display = "";
	txt.focus();
	HideLabel(labelId, e, txtId);
}

//save textbox data by pressing Enter
//or cancel on Escape
function SaveDataOnEnter(txtId, e, labelId, btnId)
{
	if (window.event)
	{
		e = window.event;
	}

	switch(e.keyCode)
	{
		case 13:	//Enter - save
			document.getElementById(btnId).click();
			break;
		case 27:	//Escape - hide
			document.getElementById(txtId).style.display = "none";
			ShowLabel(labelId, e, txtId);
			break;
	}
}

//save textbox data by lost focus
function SaveDataOnLostFocus(txtId, btnId)
{
	document.getElementById(txtId).style.display = "none";
	document.getElementById(btnId).click();
	return false;
}