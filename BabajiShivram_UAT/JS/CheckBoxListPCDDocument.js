// JScript File

        // Show/Hide File Upload and CheckBoxList CustomerValidator On check Box Selection
        function toggleDiv(chk,FUid,CusValId,RFVFileUpload) 
        {                
            var checkboxId = document.getElementById(chk); 
            var fileUploadId = document.getElementById(FUid); 
            var CustomValidatorList = document.getElementById(CusValId); 
            var fileUploadRequired = document.getElementById(RFVFileUpload);

            if(checkboxId.checked == true)
            {
               checkboxId.checked = true; 
               fileUploadId.disabled = false;
               ValidatorEnable(CustomValidatorList, true);
		
		// File Upload Required
               if (fileUploadRequired != null) {

                   ValidatorEnable(fileUploadRequired, true);
               }

            }
           else if(checkboxId.checked == false)
            {
            
               checkboxId.checked = false; 
               fileUploadId.disabled = true;
               ValidatorEnable(CustomValidatorList, false);

		// File Upload Not Required
               if (fileUploadRequired != null) {
                   ValidatorEnable(fileUploadRequired, false);
               }

            }
            
        }
        
        /**** ChekBoxList Copy/Original Validation for At lease One check Box Selected ****/
        function ValidateCheckBoxList(source, args)
        {              
                
          var chkListOriginal= document.getElementById(source.checklistId);
          
          var chkListinputs = chkListOriginal.getElementsByTagName("input");
                        
          for (var i=0;i<chkListinputs.length;i++)
          {
            if (chkListinputs [i].checked)
            {
              args.IsValid = true;
              return;
            }
          }
          args.IsValid = false;
        }
        
        /**** ChekBoxList Copy/Original Click Event Enable/Disable Custom Validator ****/
        function chkDuplicateChecked(chk,ParentCheckBox,CVCheck)
        {
            var checDuplicateId = document.getElementById(chk); 
            var ParentCheckBoxId = document.getElementById(ParentCheckBox); 
            var CVCheckId = document.getElementById(CVCheck); 
            
            var chkListItem = checDuplicateId.getElementsByTagName("input");
            
            if (ParentCheckBoxId.checked)
            {
                ValidatorEnable(CVCheckId, true);
            }
            for (var i=0;i<chkListItem.length;i++)
            {
                if (chkListItem[i].checked)
                {
                    ValidatorEnable(CVCheckId, false);
                }
            } 
        }  
    