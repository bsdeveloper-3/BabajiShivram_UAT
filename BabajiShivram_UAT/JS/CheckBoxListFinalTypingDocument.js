// JScript File

// Show/Hide File Upload and CheckBoxList CustomerValidator On check Box Selection
function toggleDiv1(chk, FUid) {
    var checkboxId = document.getElementById(chk);
    var fileUploadId = document.getElementById(FUid);
    //var CustomValidatorList = document.getElementById(CusValId);

    if (checkboxId.checked == true) {
        checkboxId.checked = true;
        fileUploadId.disabled = false;
        //ValidatorEnable(CustomValidatorList, true);
    }
    else if (checkboxId.checked == false) {

        checkboxId.checked = false;
        fileUploadId.disabled = true;
        //ValidatorEnable(CustomValidatorList, false);
    }

}


    