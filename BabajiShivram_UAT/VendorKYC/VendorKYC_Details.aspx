<%@ Page Title="Vendor KYC Details" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="VendorKYC_Details.aspx.cs" Inherits="Service_VendorKYC_Details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />

  
    <%-- <asp:UpdatePanel ID="upFillDetails" runat="server" UpdateMode="Conditional" RenderMode="Inline">
     <ContentTemplate>--%>
        <div align="center">
            <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
        <asp:ValidationSummary ID="ValSummary" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
        <asp:Label ID="lblResult" runat="server" EnableViewState="false"></asp:Label>           
        </div>      
        <fieldset id="fsGeneral">
            <legend>New Vendor Entry</legend>
                <InsertItemTemplate>
                <div>
                         <asp:Button ID="btnSubmit" runat="server" Text="Save" ValidationGroup="Required" OnClick="btnSubmit_Click" />  <%----%>
                          <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />  
                      </div>
                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                    <tr>
                            <td>
                               Company Name
                            <asp:RequiredFieldValidator  ID="RFVCompany"  runat="server"  ControlToValidate="txtCompanyName" ErrorMessage="Please Enter Company Name"  Text="*"   
                               Display="Dynamic"  SetFocusOnError="true" ValidationGroup="Required" InitialValue="" />  

                            </td>
                            <td>
                            <asp:TextBox ID="txtCompanyName" runat="server" Width="250"></asp:TextBox>                            
                            </td>
                            <td>
                                Vendor
                                <asp:RequiredFieldValidator ID="RFVendor" runat="server" InitialValue="0" ControlToValidate="ddlVendor" SetFocusOnError="true"
                                    Text="*" ErrorMessage="Please Select KYC Type" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                    <asp:DropDownList ID="ddlVendor" runat="server" class="form-control">
                                    <asp:ListItem Text="Vendor" Value="1"></asp:ListItem>  
                                </asp:DropDownList>
                            </td>
                        </tr>
                    <tr>
                                <td>
                                  Legal Name
                                    <asp:RequiredFieldValidator ID="RFVLegalName" runat="server" InitialValue="" ControlToValidate="txtLegalName" SetFocusOnError="true"
                                        Text="*" ErrorMessage="Please Enter Legal Name " Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                        <asp:TextBox ID="txtLegalName" runat="server" Width="250"></asp:TextBox>
                                </td>
                                <td>
                                   Trade Name
                                    <asp:RequiredFieldValidator ID="RFVTradeName" runat="server" InitialValue="" ControlToValidate="txtTradeName" SetFocusOnError="true"
                                        Text="*" ErrorMessage="Please Enter Trade Name" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                       <asp:TextBox ID="txtTradeName" runat="server" Width="250"></asp:TextBox>
                                </td>
                             </tr>
                    <tr>
                                      <td>
                                      Division
                                     <%-- <asp:RequiredFieldValidator ID="RFVDivision" runat="server" InitialValue="" ControlToValidate="ddlDivision" SetFocusOnError="true"
                                          Text="*" ErrorMessage="Please Enter Division" Display="Dynamic"  ValidationGroup="Required"></asp:RequiredFieldValidator>--%>
                                  </td>
                                  <td>
                                         <asp:DropDownList ID="ddlDivision" runat="server" class="form-control">
                                         <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                         <asp:ListItem Text="CB-Imports" Value="CB-Imports"></asp:ListItem>
                                         <asp:ListItem Text="CB-Exports" Value="CB-Exports"></asp:ListItem>
                                         <asp:ListItem Text="Transport" Value="Transport"></asp:ListItem>
                                         <asp:ListItem Text="Freight Forwarding" Value="Freight Forwarding"></asp:ListItem>
                                         <asp:ListItem Text="SEZ" Value="SEZ"></asp:ListItem>
                                         <asp:ListItem Text="Marine" Value="Marine"></asp:ListItem>
                                         <asp:ListItem Text="Warehouse" Value="Warehouse"></asp:ListItem>
                                         <asp:ListItem Text="Essesntial Certificate" Value="Essesntial Certificate"></asp:ListItem>
                                         <asp:ListItem Text="Equipment Hire" Value="Equipment Hire"></asp:ListItem>
                                         <asp:ListItem Text="Project" Value="Project"></asp:ListItem>
                                     </asp:DropDownList>
                                  </td>
                                <td>
                                 Mobile No
                                          <asp:RegularExpressionValidator  ID="RFVMobile"  runat="server" ControlToValidate="txtOfficeTelephone" 
                                      ErrorMessage="Contact No.must be numeric code." ForeColor="Red" ValidationExpression="^[0-9]+$" ValidationGroup="Required" Display="Dynamic">
                                     </asp:RegularExpressionValidator>
                                    </td>
                                    <td>
                                           <asp:TextBox ID="txtOfficeTelephone" runat="server" Width="250"></asp:TextBox>
                                    </td>
                                </tr>
                              <tr>
                                <td>
                                  Contact Person
                                    <asp:RequiredFieldValidator ID="RFVContactPerson" runat="server" InitialValue="" ControlToValidate="txtContactPerson" SetFocusOnError="true"
                                        Text="*" ErrorMessage="Please Enter Contact Person" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                     <asp:TextBox ID="txtContactPerson" runat="server" Width="250"></asp:TextBox>
                                </td>
                                <td>
                                    Contact No
                                      <asp:RegularExpressionValidator   ID="RFVContact"  runat="server" ControlToValidate="txtContactNo" 
                                      ErrorMessage="Contact No.must be numeric code." ForeColor="Red" ValidationExpression="^[0-9]+$" ValidationGroup="Required" Display="Dynamic">
                                    </asp:RegularExpressionValidator>
                                     <asp:RequiredFieldValidator ID="RFVContactNo" runat="server" InitialValue="0" ControlToValidate="txtContactNo" SetFocusOnError="true"
                                        Text="*" ErrorMessage="Please Enter Contact No" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                       <asp:TextBox ID="txtContactNo" runat="server" Width="250"></asp:TextBox>
                                </td>
                            </tr>
                             <tr>
                                   <td>
                                    EMail
                                       <asp:RequiredFieldValidator ID="RFVEmail" runat="server" InitialValue="0" ControlToValidate="txtEmail" SetFocusOnError="true"
                                           Text="*" ErrorMessage="Please Enter Email" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                                       <asp:RegularExpressionValidator ID="RFVmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Invalid Email Address."
                                          ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="Required"/>
                                   </td>
                                   <td>
                                          <asp:TextBox ID="txtEmail" runat="server"  Text="" Width="250"></asp:TextBox>
                                     
                                   </td>
                                   <td>
                                       Credit Days
                                        <asp:RequiredFieldValidator ID="RFVcredit" runat="server" InitialValue="" ControlToValidate="txtCrDays" SetFocusOnError="true"
                                      Text="*" ErrorMessage="Please Enter  Credit Days" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                                      <asp:RegularExpressionValidator   ID="RFVcreditDays"  runat="server" ControlToValidate="txtCrDays" 
                                        ErrorMessage="Credit Days must be a numeric value between 1 to 365 Days." ForeColor="Red"  ValidationExpression="^\d{1,3}$"
                                             ValidationGroup="Required" Display="Dynamic">
                                       </asp:RegularExpressionValidator>
                                   </td>
                                   <td>
                                          <asp:TextBox ID="txtCrDays" runat="server" Width="250"></asp:TextBox>
                                   </td>
                               </tr>
                                <tr>
                                      <td>Vendor Category</td>
                                        <td>
                                        <asp:DropDownList ID="ddlvendorcatagory" runat="server"></asp:DropDownList>
                                        </td>
                                     <td>
                                      Address
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" InitialValue="" ControlToValidate="txtAddress" SetFocusOnError="true"
                                            Text="*" ErrorMessage="Please Enter Address" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                    <asp:TextBox ID="txtAddress" runat="server" MaxLength="200" TextMode="MultiLine" Columns="3" Width="300px"></asp:TextBox>
                                    </td>
                                </tr>
                             <tr>
                             <td>Country
                                </td>
                                 <td>
                                    <asp:DropDownList ID="ddlCountry" runat="server"  AutoPostBack="True" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged"  >                                
                                    </asp:DropDownList><%-- --%>
                                </td> 
                              <td>
                                    <asp:Label ID="lblState" Text="State" Visible="false" runat="server"></asp:Label>    
     
                                </td>
                                 <td>
                                    <asp:DropDownList ID="ddlState"  Visible="false" runat="server" ></asp:DropDownList>
                                </td> 
                                  </tr>
                             <tr>
                             <td>City  
                                </td>
                                 <td>
                                   <asp:TextBox ID="txtCity" runat="server"  Width="250"></asp:TextBox>
                                </td> 
                                  <td>Pincode                                 
                                     </td>
                                      <td>
                                          <asp:TextBox ID="txtPincode" runat="server"  Width="250"></asp:TextBox>
                                     </td> 
                              </tr>
                            <tr>
                                 <td>
                                    GST Reg. Type
                                    <%--<asp:RequiredFieldValidator ID="RFVGSTRegType" runat="server" InitialValue="0" ControlToValidate="ddlGSTRegType" SetFocusOnError="true"
                                     Text="*" ErrorMessage="Please Enter GST Reg Type" Display="Dynamic"  ValidationGroup="Required"></asp:RequiredFieldValidator>--%>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlGSTRegType" runat="server" class="form-control" AutoPostBack="true" 
                                                      OnSelectedIndexChanged="ddlGSTRegType_SelectedIndexChanged">
                                        <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Registered" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Un-Registered" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Foreign" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="SEZ" Value="4"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                      <td>
                                     <asp:Label ID="lblPan" runat="server" Text="Pan No" Visible="false"></asp:Label>
                                    <asp:RequiredFieldValidator ID="RFVPan" runat="server" InitialValue="" ControlToValidate="txtPan" SetFocusOnError="true"
                                        Text="*" ErrorMessage="Please Enter Pan NO" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                                           <asp:RegularExpressionValidator ID="revPAN" runat="server" ControlToValidate="txtPan" ErrorMessage="Invalid PAN Number."
                                            ValidationExpression= "^[a-zA-Z]{5}[0-9]{4}[a-zA-Z]{1}$"  ValidationGroup="Required"/> <%--"^[A-Z]{5}[0-9]{4}[A-Z]{1}$"--%>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPan" runat="server" Width="250" Visible="false"></asp:TextBox>
                                </td>
                            </tr>
                    <tr>
                               <%-- <td>
                                  Location
                                    <asp:RequiredFieldValidator ID="RFVLocation" runat="server" InitialValue="" ControlToValidate="txtLocation" SetFocusOnError="true"
                                        Text="*" ErrorMessage="Please Enter Location" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                        <asp:TextBox ID="txtLocation" runat="server" MaxLength="100"></asp:TextBox>
                                </td>--%>
                                 <td>
                                     <asp:Label ID="lblGstn" runat="server" Text="GST No:" Visible="false"></asp:Label>
                                      <asp:RegularExpressionValidator ID="rfvgst" runat="server" ControlToValidate="txtGSTN" ErrorMessage="Invalid GSTNO Number."
                                        ValidationExpression="\d{2}[a-zA-Z]{5}\d{4}[a-zA-Z]{1}[a-zA-Z\d]{1}[Zz]{1}[a-zA-Z\d]{1}" ValidationGroup="Required"/>
                                 </td>
                                 <td>
                                     <asp:TextBox ID="txtGSTN" runat="server" Width="250" Visible="false"></asp:TextBox>
                                 </td>
                                   <td>HOD
                                     <%--  <asp:RequiredFieldValidator ID="RFVHOD" runat="server" InitialValue="" ControlToValidate="ddHOD" SetFocusOnError="true"
                                 Text="*" ErrorMessage="Please Select HOD" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>--%>
                            </td>
                             <td>
                                <asp:DropDownList ID="ddHOD" runat="server"></asp:DropDownList>
                            </td>              
                            </tr>
                    <tr>
                               <td>
                                KAM
                                 
                               </td>
                               <td>
                                       <asp:TextBox ID="txtKAM" runat="server" Width="250"></asp:TextBox>
                               </td>
                                <td>
                               CCM
                               
                            </td>
                            <td>
                                   <asp:TextBox ID="txtCCM" runat="server" Width="250"></asp:TextBox>
                            </td>
                            </tr>
                    <tr>
                            <td>
                              Bank Name
                                <asp:RequiredFieldValidator ID="RFVBankName" runat="server" InitialValue="" ControlToValidate="txtBankName" SetFocusOnError="true"
                                    Text="*" ErrorMessage="Please Enter Bank Name" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                    <asp:TextBox ID="txtBankName" runat="server" Width="250"></asp:TextBox>
                            </td>
                            <td>
                                Account No
                                <asp:RequiredFieldValidator ID="RFVAccount" runat="server" InitialValue="" ControlToValidate="txtAcctNo" SetFocusOnError="true"
                                    Text="*" ErrorMessage="Please Enter Account No" Display="Dynamic"  ValidationGroup="Required"></asp:RequiredFieldValidator>
                                  <asp:RegularExpressionValidator ID="RFVAccountNumber" runat="server" ControlToValidate="txtAcctNo" ErrorMessage="Invalid Account Number." 
                                  ValidationExpression="^[A-Za-z0-9]+$"   ValidationGroup="Required" />     
                            </td>
                            <td>
                                   <asp:TextBox ID="txtAcctNo" runat="server" Width="250"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                           <td>
                            IFSC Code
                               <asp:RequiredFieldValidator ID="RFVIFSC" runat="server" InitialValue="" ControlToValidate="txtIFSC" SetFocusOnError="true"
                                   Text="*" ErrorMessage="Please Enter IFSC Code" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                               <asp:RegularExpressionValidator  ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtIFSC"   
                               ErrorMessage="Please enter a valid IFSC code." ForeColor="Red"  ValidationExpression="^[A-Za-z]{4}[0-9]{1}[A-Za-z0-9]{6}$" ValidationGroup="Required" 
                               Display="Dynamic" />    <%--^[A-Za-z]{4}0[A-Za-z0-9]{6}$--%>                             
                           </td>
                           <td>
                                   <asp:TextBox ID="txtIFSC" runat="server" Width="250"></asp:TextBox>
                           </td>
                             <td>
                                MICR Code
                             <%-- <asp:RequiredFieldValidator ID="RFVMICR" runat="server" InitialValue="" ControlToValidate="txtMICR" SetFocusOnError="true"
                                     Text="*" ErrorMessage="Please Enter MICR Code" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>--%>
                             </td>
                             <td>
                                     <asp:TextBox ID="txtMICR" runat="server" Width="250"></asp:TextBox>
                             </td>
                           
                        </tr>
                    <tr>
                            <%-- <td>
                            Branch Code
                              <%--<asp:RequiredFieldValidator ID="RFVBrranchCode" runat="server" InitialValue="" ControlToValidate="txtBranchCode" SetFocusOnError="true"
                                  Text="*" ErrorMessage="Please Enter Branch Code" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>--%>
                         <%-- </td>
                          <td>
                                  <asp:TextBox ID="txtBranchCode" runat="server" Width="250"></asp:TextBox>
                          </td>--%>
                            <%--<td>
                                Name of the Branch
                                <%-- <asp:RequiredFieldValidator ID="RFVBranch" runat="server" InitialValue="" ControlToValidate="txtBranchName" SetFocusOnError="true"
                                    Text="*" ErrorMessage="Please Enter Branch Name" Display="Dynamic"  ValidationGroup="Required"></asp:RequiredFieldValidator>--%>
                            <%--</td>
                            <td>
                                    <asp:TextBox ID="txtBranchName" runat="server" MaxLength="100"></asp:TextBox>
                            </td>--%>

                            
                                <td>
                            Account Type                          
                        </td>
                            <td>
                           <asp:DropDownList ID="txtAccountType" runat="server" class="form-control">
                                   <asp:ListItem Text="-Select-" Value="Select"></asp:ListItem>
                                   <asp:ListItem Text="Current" Value="Current"></asp:ListItem>
                                   <asp:ListItem Text="Saving" Value="Saving"></asp:ListItem>                                       
                           </asp:DropDownList>
                            </td>
                        </tr>  
                    <tr>
                        <td>
                            TDS
                        </td>
                        <td> <%----%>
                            <asp:RadioButtonList ID="rblTDS" runat="server" RepeatDirection="Horizontal"
                               AutoPostBack="true" OnSelectedIndexChanged="rblTDS_SelectedIndexChanged" >
                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                            </asp:RadioButtonList>
                            </td>
                        <td>
                            MSME                                    
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblMSME" runat="server" RepeatDirection="Horizontal"
                                AutoPostBack="true" OnSelectedIndexChanged="rblMSME_SelectedIndexChanged" >
                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                        </asp:RadioButtonList>
                        <%--<asp:RadioButton ID="rbtnMSMEYes" runat="server" GroupName="MSMEGroup" Text="Yes" CssClass="form-check" AutoPostBack="true" OnCheckedChanged="rbtnMSME_CheckedChanged" /> 
                        <asp:RadioButton ID="rbtnMSMENo" runat="server" GroupName="MSMEGroup" Text="No" CssClass="form-check" AutoPostBack="true" OnCheckedChanged="rbtnMSME_CheckedChanged" />--%>
                            <%-- <asp:CustomValidator ID="CVMSME" runat="server"  ClientValidationFunction="rbtnMSMEYes" Text="*"  ErrorMessage="Please select MSME" 
                                Display="Dynamic" ValidationGroup="Required" />
                            <asp:CustomValidator ID="CustomValidator2" runat="server"  ClientValidationFunction="rbtnMSMENo" Text="*"  ErrorMessage="Please select MSME" 
                                Display="Dynamic" ValidationGroup="Required" />--%>
                            </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="UPDTDS" runat="server" Text="TDS Certificate" Visible="false"></asp:Label>   
                            <asp:RequiredFieldValidator ID="RFVTDS" runat="server" InitialValue="" ControlToValidate="fuTDSCirteficate" SetFocusOnError="true"
                                Text="*" ErrorMessage="Please Upload TDS Certificate" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:FileUpload ID="fuTDSCirteficate" runat="server" CssClass="form-control" Visible="false" />
                        </td>
                        <td>
                        <asp:Label ID="UPDMSME" runat="server" Text="MSME Certificate"  Visible="false"></asp:Label>   
                            <asp:RequiredFieldValidator ID="RFVMSME" runat="server" InitialValue="" ControlToValidate="fuMSMECirteficate" SetFocusOnError="true"
                                Text="*" ErrorMessage="Please Upload MSME Certificate" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:FileUpload ID="fuMSMECirteficate" runat="server" CssClass="form-control" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Pan Copy
                        <asp:Label ID="lblPanCopy" runat="server" Visible="false"></asp:Label>   
                            </td>
                        <td>
                            <asp:FileUpload ID="fuPanCopy" runat="server" CssClass="form-control" />
                        </td>
                        <td>
                        GST Copy 
                            <asp:Label ID="lblGstdoc" runat="server" Visible="false"></asp:Label>   
                        </td>
                            <td>
                                <asp:FileUpload ID="fuGstCopy" runat="server" CssClass="form-control" />
                            </td>
                            </tr>
                        <tr>
                              <td>
                                Blank Cheque Copy
                                <asp:Label ID="lblBlackcheque" runat="server" Visible="false"></asp:Label>   
                                </td>
                            <td>
                                <asp:FileUpload ID="fuBlankChequecopy" runat="server" CssClass="form-control" />
                            </td>
                               <td>
                              Remark
                                <asp:RequiredFieldValidator ID="RFVRemark" runat="server" InitialValue="" ControlToValidate="txtRemark" SetFocusOnError="true"
                                    Text="*" ErrorMessage="Please Enter Remark" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                            <asp:TextBox ID="txtRemark" runat="server" MaxLength="300" TextMode="MultiLine" Columns="3" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                          
                           <%-- <tr>                               
                                 <td>
                                     9 Digit Code Bank & Branch                                
                                 </td>
                                 <td>
                                         <asp:TextBox ID="txtBankBranchCode" runat="server" MaxLength="100"></asp:TextBox>
                                 </td>
                                 <td>
                                 Address of the Bank--%>
                                      <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" InitialValue="" ControlToValidate="txtMICR" SetFocusOnError="true"
                                          Text="*" ErrorMessage="Please Enter MICR Code" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>--%>
                                <%--</td>
                                <td>
                                <asp:TextBox ID="txtBankAddress" runat="server" MaxLength="200" TextMode="MultiLine" Columns="3" Width="300px"></asp:TextBox>
                              </td>
                             </tr>--%>
                             <%--<tr>
                                 <td>
                                 Account Type                          
                             </td>
                                 <td>
                                <asp:DropDownList ID="txtAccountType" runat="server" class="form-control">
                                        <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Current" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Saving" Value="2"></asp:ListItem>                                       
                                </asp:DropDownList>
                                 </td>
                                 <td>
                                  Whether Branch is RTGS Internet Enabled
                                </td>
                                <td>
                                   <asp:RadioButton ID="rbtnRTGSYes" runat="server" GroupName="RTGSGroup" Text="Yes" CssClass="form-check" />
                                   <asp:RadioButton ID="rbtnRTGSNo" runat="server" GroupName="RTGSGroup" Text="No" CssClass="form-check" />
                                </td>
                             </tr>--%>                           
                         </table> 
            </InsertItemTemplate>
        </fieldset>   
       <%--  </ContentTemplate>
</asp:UpdatePanel>--%>
  </asp:Content>
       