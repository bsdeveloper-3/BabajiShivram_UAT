<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ApproveRejected_Vendor.aspx.cs" Inherits="Service_ApproveRejected_Vendor" MasterPageFile="~/MasterPage.master" EnableSessionState="true"
    Title="Rejected Vendor Tracking" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" Culture="en-GB"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager2" ScriptMode="Release" />

   <%-- <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upJobDetail" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); -moz-opacity: .8; opacity: .8;">
                    <img alt="progress" src="images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>--%>

    <%--<asp:UpdatePanel ID="upJobDetail" runat="server">--%>
        <ContentTemplate>
            <div align="center">
                   <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:Label ID="lblreject" runat="server" EnableViewState="false" CssClass="errorMsg"></asp:Label>
                   <asp:HiddenField ID="hdnhod" runat="server" />
                  <asp:HiddenField ID="hdngsttype" runat="server" Value="0" />
                <asp:HiddenField ID="hdnVendorKycId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnUploadPath" runat="server" />
                <asp:HiddenField ID="hdnCountry" runat="server" Value="0" />
                 <asp:HiddenField ID="hdnState" runat="server" />
                 <asp:HiddenField ID="hdnVendortype" runat="server" />
                <asp:HiddenField ID="hdnDivision" runat="server" />
               <asp:HiddenField ID="hdnAccounttype" runat="server" />

                            
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False"
                    ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
                <div class="clear"></div>
             
                            <fieldset>
                                <legend>Rejected vendor Details</legend>
                                <asp:FormView ID="RejectedDetails" HeaderStyle-Font-Bold="true" runat="server" DataKeyNames="lId"
                                    Width="100%" OnDataBound="FVRejectedDetails_DataBound">
                                    <ItemTemplate>
                                        <div class="m clear">
                                            <asp:Button ID="btnEdit" runat="server" CssClass="btn"
                                                Text="Edit" OnClick="btnEdit_Click"/>
                                            <asp:Button ID="btnBackButton" runat="server" UseSubmitBehavior="false"
                                                Text="Back" CausesValidation="false" OnClick="btnBackButton_Click" /> 
                                        </div>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                            <tr>
                                                <td>Company Name</td>
                                                <td><%# Eval("VendorName") %></td>
                                                  <td>Service Category</td>
                                                <td><%# Eval("VendorType") %></td>
                                            </tr>
                                            <tr>
                                                <td>Legal Name</td>
                                                <td><%# Eval("LegalName") %></td>
                                                <td>Trade Name</td>
                                                 <td><%# Eval("TradeName") %></td>
                                               
                                            </tr>
                                            <tr>
                                                <td>Division</td>
                                                <td><%# Eval("Division") %></td>
                                               
                                                 <td>Mobile No</td>
                                                   <td><%# Eval("OfficeTelephone") %></td>
                                            </tr>
                                            <tr>
                                                <td>Contact Person</td>
                                                <td><%# Eval("ContactPerson") %></td>
                                                <td>ContactNo</td>
                                                <td><%# Eval("ContactNo") %></td>
                                            </tr>
                                            <tr>
                                                <td>Email</td>
                                                <td><%# Eval("Email") %></td>
                                                <td>Credit Days</td>
                                                <td><%# Eval("CreditDays") %></td>
                                            </tr>
                                              <tr>
                                                 <%-- <td>Service Catagory  </td>
                                              <td><%# Eval("VendorCategory") %></td>--%>
                                                      <td>GST Reg.Type</td>
                                                <td><%# Eval("GSTRegTypeName") %></td>
                                            </tr>
                                            <tr>
                                                <td>Pan No</td>
                                                <td><%# Eval("PanNo") %></td>
                                                <td>GST NO</td>
                                                <td><%# Eval("GSTN") %></td>
                                            </tr>
                                           
                                         <tr>
                                             <td>HOD Name</td>
                                             <td><%# Eval("HODName") %></td>
                                             <td>Kam Name</td>
                                             <td><%# Eval("KamName") %></td>
                                         </tr>
                                            <tr>
                                                <td>Address</td>
                                                <td><%# Eval("Address") %></td>
                                                <td>Country</td>
                                                <td><%# Eval("Country") %></td>
                                            </tr>
                                            <tr>
                                                <td>State</td>
                                                <td><%# Eval("State") %></td>
                                                <td>City</td>
                                                <td><%# Eval("City") %></td>
                                            </tr>
                                            <tr>
                                                <td>Pincode</td>
                                                <td><%# Eval("Pincode") %></td>
                                                <td>Account No</td>
                                                <td><%# Eval("AccountNo") %></td>
                                            </tr>
                                            <tr>
                                                <td>Bank Name</td>
                                                <td><%# Eval("BankName") %></td>
                                                <td>IFSCCode</td>
                                                <td><%# Eval("IFSCCode") %></td>
                                            </tr>
                                            <tr>
                                                <td>MICRCode</td>
                                                <td><%# Eval("MICRCode") %></td>
                                                <td>Account Type</td>
                                                <td><%# Eval("AccountType") %></td>
                                            </tr>
                                    
                                        </table>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div class="m clear">
                                            <asp:Button ID="btnUpdate" runat="server" CssClass="update"
                                                Text="Update" CommandName="Update" ValidationGroup="Required" OnClick="btnUpdate_Click" /> <%-- --%>
                                            <asp:Button ID="btnCancelButton" runat="server" CausesValidation="False"
                                                CssClass="cancel" Text="Cancel" CommandName="Cancel" OnClick="btnCancelButton_Click" /> <%-- --%>
                                        </div>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                            <tr>
                                                <td>Company Name</td>
                                                <td>
                                                    <asp:TextBox ID="txtCompanyName" Text='<%# Bind("VendorName") %>' runat="server" MaxLength="100" Width="200"/>
                                                </td>
                                                <td>Legal Name</td>
                                                <td>
                                                    <asp:TextBox ID="txtLegalName" Text='<%# Bind("LegalName") %>' runat="server" MaxLength="100" Width="200"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Trade Name</td>
                                                <td>
                                                    <asp:TextBox ID="txtTradeName" Text='<%# Bind("TradeName") %>' runat="server" MaxLength="15" Width="200"/>
                                                </td>
                                                <td>Division </td>
                                                <td>
                                                  <asp:DropDownList ID="ddlDivison" runat="server" >
                                                    </asp:DropDownList>
                                                      <asp:HiddenField ID="hdnDivision" runat="server" Value='<%#Eval("Division") %>'  />
                                                  </td>
                                            </tr>
                                            <tr>
                                                <td>Mobile No
                                                      <asp:RegularExpressionValidator   ID="RFVMobile"  runat="server" ControlToValidate="txtMobileNo" 
                                                     ErrorMessage="Contact No.must be numeric code." ForeColor="Red"     ValidationExpression="^[0-9]+$"  ValidationGroup="Required" Display="Dynamic">
                                                    </asp:RegularExpressionValidator>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMobileNo" Text='<%# Bind("OfficeTelephone") %>' runat="server" MaxLength="100" Width="200"/>
                                                </td>
                                                 <td>Contact Person</td>
                                             <td>
                                                 <asp:TextBox ID="txtcontactperson" Text='<%# Bind("ContactPerson") %>' runat="server" MaxLength="100" Width="200"/>
                                             </td>
                                             </tr>
                                            <tr>
                                                <td>Contact No
                                                      <asp:RegularExpressionValidator   ID="RFVContactNo"  runat="server" ControlToValidate="txtContactNo" 
                                                     ErrorMessage="Contact No.must be numeric code." ForeColor="Red" ValidationExpression="^[0-9]+$" ValidationGroup="Required" Display="Dynamic">
                                                    </asp:RegularExpressionValidator>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtContactNo" Text='<%# Bind("ContactNo") %>' runat="server" MaxLength="15" Width="200" />
                                                </td>
                                                <td>Email
                                                     <asp:RegularExpressionValidator ID="RFVmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Invalid Email Address."
                                                      ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="Required"/>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEmail" Text='<%# Bind("Email") %>' runat="server" MaxLength="100" Width="200" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Credit Days
                                             
                                                    <asp:RegularExpressionValidator 
                                                    ID="RFVcredit" runat="server" ControlToValidate="txtCreditDays" ErrorMessage="Credit Days must be numeric value between 1 to 365 Days." 
                                                    ForeColor="Red" ValidationExpression="^\d{1,3}$" ValidationGroup="Required" Display="Dynamic"></asp:RegularExpressionValidator>

                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCreditDays" Text='<%# Bind("CreditDays") %>' runat="server" MaxLength="100" Width="200" />
                                                </td>
                                               
                                                    <td>Service Category</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlvendortype" runat="server"  >  
                                                    </asp:DropDownList>
                                                    <asp:HiddenField ID="hdnVendortype" runat="server" Value='<%#Eval("VendorCategoryID") %>'/>
                                                </td>
                                                
                                          </tr>
                                             <tr>
                                                <td>GST Reg.Type</td>
                                             <td>
                                             <asp:DropDownList ID="ddlGSTRegType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlGSTRegType_SelectedIndexChanged"><%-- --%>
                                           
                                               </asp:DropDownList>
                                                 <asp:HiddenField ID="hdngsttype" runat="server" Value='<%#Eval("GSTRegTypeId") %>' />
                                             </td>
                                             
                                                 <td> <asp:Label ID="lblGstn" runat="server" Text="GST No" Visible="false"></asp:Label> 

                                                 <asp:RegularExpressionValidator ID="rfvgst" runat="server" ControlToValidate="txtGSTN" ErrorMessage="Invalid GSTNO Number."
                                                     ValidationExpression="\d{2}[A-Z]{5}\d{4}[A-Z]{1}[A-Z\d]{1}[Z]{1}[A-Z\d]{1}" ValidationGroup="Required"/>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGSTN" Text='<%# Bind("GSTN") %>' runat="server"  MaxLength="100" Width="200" />
                                            </td>
                                         </tr>
                                              <tr>
                                          <td> <asp:Label ID="lblPan" runat="server" Text="Pan No" ></asp:Label> 

                                               <asp:RegularExpressionValidator ID="RFVPAN" runat="server" ControlToValidate="txtPanNo" ErrorMessage="Invalid PAN Number."
                                                    ValidationExpression="^[A-Z]{5}[0-9]{4}[A-Z]{1}$" ValidationGroup="Required"/>
                                          </td>
                                          <td>
                                              <asp:TextBox ID="txtPanNo" Text='<%# Bind("PanNo") %>' runat="server" Visible="false"  MaxLength="15" Width="200"/>
                                          </td>                                       
                                      </tr>                                         
                                            <tr>
                                                  <td>HOD Name
                                             </td>
                                            <td>
                                                <asp:DropDownList ID="dlHOD" runat="server" >
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="hdnhod" runat="server" Value='<%#Eval("HODName") %>' />
                                            </td>
                                            <td>Kam Name
                                            </td>
                                            <td>
                                           <asp:TextBox ID="txtKamName" Text='<%# Bind("KamName") %>' runat="server" MaxLength="100" Width="200" />

                                            </td>
                                        </tr>
                                            <tr>
                                                <td>Address</td>
                                                <td>
                                                    <asp:TextBox ID="txtAddress" Text='<%# Bind("Address") %>' runat="server" TextMode="MultiLine"  Width="300"/>
                                                </td>
                                            <td>Country
                                            </td>
                                          <td>
                                              <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged"><%--  --%>
                                              </asp:DropDownList>
                                              <asp:HiddenField ID="hdnCountry" runat="server" Value='<%#Eval("Country") %>' />
                                          </td>
                                            </tr>
                                            <tr>
                                          <td><asp:Label ID="lblState" runat="server" Text="State" ></asp:Label> 
                                            </td>
                                          <td>
                                              <asp:DropDownList ID="ddlState" runat="server" > 
                                              </asp:DropDownList>
                                              <asp:HiddenField ID="hdnState" runat="server"  Value='<%#Eval("State") %>' />
                                          </td>
                                                <td>City</td>
                                                <td>
                                                    <asp:TextBox ID="txtCity" Text='<%# Bind("City") %>' runat="server"  MaxLength="15" Width="200"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Pincode</td>
                                                <td>
                                                    <asp:TextBox ID="txtPincode" Text='<%# Bind("Pincode") %>' runat="server" MaxLength="100" Width="200" />
                                            
                                                </td>
                                                <td>Account No
                                                <asp:RegularExpressionValidator ID="RFVAccountNumber" runat="server" ControlToValidate="txtAccountNo" ErrorMessage="Invalid Account Number." 
                                                             ValidationExpression="^[A-Za-z0-9]+$"   ValidationGroup="Required" /> <%--Special characters Spaces are Not Allowed--%>
                                                              
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAccountNo" Text='<%# Bind("AccountNo") %>' runat="server" MaxLength="100" Width="200"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                
                                                <td>Bank Name</td>
                                                <td>
                                                    <asp:TextBox ID="txtBankName" Text='<%# Bind("BankName") %>' runat="server" MaxLength="100" Width="200" />
                                                </td>
                                                <td>Account Type</td>
                                            <td>
                                                 <asp:DropDownList ID="ddlAccounttype" runat="server" >
                                                 </asp:DropDownList>
                                                 <asp:HiddenField ID="hdnAccounttype" runat="server" Value='<%#Eval("AccountType") %>' />
                                              </td>
                                            </tr>
                                            <tr>                                             
                                                <td>IFSCCode

                                                     <asp:RequiredFieldValidator ID="rfvifscCode" runat="server" InitialValue="" ControlToValidate="txtIFSCCode" SetFocusOnError="true"
                                                     Text="*" ErrorMessage="Please Enter IFSC Code" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                                                     <asp:RegularExpressionValidator  ID="RFVIfsc" runat="server" ControlToValidate="txtIFSCCode"   
                                                 ErrorMessage="Please enter a valid IFSC code." ForeColor="Red"  ValidationExpression="^[A-Za-z]{4}[0-9]{1}[A-Za-z0-9]{6}$" ValidationGroup="Required" 
                                                 Display="Dynamic" />    
                                                    
                                                </td>
                                                <td>
                                               <asp:TextBox ID="txtIFSCCode" Text='<%# Bind("IFSCCode") %>' runat="server" MaxLength="100" Width="200"/>
                                                </td>
                                                <td>MICRCode</td>
                                                <td>
                                                    <asp:TextBox ID="txtMICRCode" Text='<%# Bind("MICRCode") %>' runat="server" MaxLength="100" Width="200"/>
                                                </td>
                                            </tr>                                         
                                          
                                            <tr>                                              
                                                <td>Created Date</td>
                                                <td>
                                                    <asp:TextBox ID="txtdate" Text='<%# Bind("dtDate","{0:dd/MM/yyyy}") %>' runat="server" MaxLength="100" Width="200"/>
                                                </td>
                                                 <td>Remark
                                                      <asp:RequiredFieldValidator ID="RFVRemark" runat="server" InitialValue="" ControlToValidate="txtremark" SetFocusOnError="true"
                                                        Text="*" ErrorMessage="Please Enter Remark" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                                                 </td>
                                                 <td>
                                                     <asp:TextBox ID="txtremark" runat="server" TextMode="MultiLine"  Width="300"></asp:TextBox>
                                                 </td>
                                           </tr>
                                           
                                        </table>
                                   
                                    </EditItemTemplate>
                                </asp:FormView>
                            </fieldset>
                         <fieldset>
                            <legend>Upload Document</legend>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td>Document Type
                                    <asp:DropDownList ID="ddDocument" runat="server" CssClass="DropDownBox">
                                    </asp:DropDownList></td>
                                <td>
                                    <asp:FileUpload ID="fuDocument" runat="server" /> &nbsp;&nbsp;
                                <asp:Button ID="btnUpload" runat ="server"
                                 OnClick="btnUpload_Click"  Text="Upload Document"  /> <%----%>
                            </td>
                            </tr>
                        </table>
                            </fieldset>
                               <fieldset>
                                <legend>Download Documents</legend>
                                <asp:GridView ID="GridViewDoc" runat="server" AutoGenerateColumns="False" DataKeyNames="lId" EnableViewState="false"
                                    CssClass="table" Width="100%" DataSourceID="DataSourceDocDownload" OnRowCommand="GridViewDoc_RowCommand">
                                    <%-- --%>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DocumentName" HeaderText="Document Name" />
                                        <asp:BoundField DataField="dtDate" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="UserName" HeaderText="Uploaded By" />
                                        <asp:TemplateField HeaderText="Download">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"
                                                    CommandArgument='<%# Eval("lid") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </fieldset>
                          <fieldset >
                             <legend>Rejected Vendor History</legend>
                                <asp:GridView ID="gvRejectHistory" runat="server" AutoGenerateColumns="False" DataKeyNames="lId" EnableViewState="false" PagerStyle-CssClass="pgr"
                                    CssClass="table" Width="100%" DataSourceID="DataSourceRejectHistory" PageSize="20">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>                         
                                 <%--          <asp:BoundField DataField="LId" HeaderText="VendorId" />--%>
                                          <asp:BoundField DataField="StatusName" HeaderText="StatusName" />
                                        <asp:BoundField DataField="Remark" HeaderText="Remark" />
                                        <asp:BoundField DataField="UserName" HeaderText="Created By" />
                                        <asp:BoundField DataField="dtDate" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy}" />
                                     </Columns>
                                </asp:GridView>
                          </fieldset>
                          <asp:SqlDataSource ID="DataSourceDocDownload" runat="server" 
                          ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                          SelectCommand="VN_GetKYCDocument" SelectCommandType="StoredProcedure">  <%-- VN_GetUploadDocuments--%>
                          <SelectParameters>
                              <asp:SessionParameter Name="VendorKYCId" SessionField="lId" />
                          </SelectParameters>
                      </asp:SqlDataSource>  
                   <asp:SqlDataSource ID="DataSourceRejectHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="VN_GetVendorHistory" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:SessionParameter Name="VendorKycId" SessionField="lId" />
                        </SelectParameters>
                     </asp:SqlDataSource>
                    </ContentTemplate>            
  <%--  </asp:UpdatePanel>--%>
</asp:Content>
