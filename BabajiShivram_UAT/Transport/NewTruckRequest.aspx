<%@ Page Title="Truck Request" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="NewTruckRequest.aspx.cs"
    Inherits="Transport_TruckRequest" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <cc1:toolkitscriptmanager id="ScriptManager1" runat="server" scriptmode="Release">
    </cc1:toolkitscriptmanager>

    <script type="text/javascript">
        function OnJobSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnJobId').value = results.JobId;
        }
    </script>

    <asp:UpdatePanel ID="upFillDetails" runat="server">
        <ContentTemplate>
            <div>
                <div align="center">
                    <asp:HiddenField ID="hdnJobId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnJobType" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnMode" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnNewPaymentLid" runat="server" Value="0" />
                    <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                    <asp:ValidationSummary ID="vsFillDetails" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
                </div>

                <fieldset>
                    <legend>Fill Job Detail</legend>
                    <div id="divInstruction" class="info" runat="server">
                    </div>
                    <div class="m clear">
                        <asp:Button ID="btnSubmit" runat="server" Text="Save" ValidationGroup="Required" TabIndex="7" OnClick="btnSubmit_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" TabIndex="8" OnClick="btnCancel_Click" />
                    </div>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>Transport Ref No</td>
                            <td>
                                <asp:Label ID="lblTransRefNo" runat="server" Font-Bold="true"></asp:Label>
                            </td>
                            <td>Job Number
                            <asp:RequiredFieldValidator ID="rfvJobNo" runat="server" ValidationGroup="Required" Display="Dynamic" SetFocusOnError="true"
                                ControlToValidate="txtJobNumber" Text="*" ErrorMessage="Please Select Job Number."></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtJobNumber" Width="160px" runat="server" ToolTip="Enter Job Number."
                                    CssClass="SearchTextbox" placeholder="Search" TabIndex="1" AutoPostBack="true" OnTextChanged="txtJobNumber_TextChanged"></asp:TextBox>
                                <div id="divwidthCust_Loc" runat="server">
                                </div>
                                <cc1:autocompleteextender id="JobDetailExtender" runat="server" targetcontrolid="txtJobNumber"
                                    completionlistelementid="divwidthCust_Loc" servicepath="~/WebService/TransportJobNumberAutoComplete.asmx"
                                    servicemethod="GetCompletionList" minimumprefixlength="2" behaviorid="divwidthCust_Loc"
                                    contextkey="4567" usecontextkey="True" onclientitemselected="OnJobSelected"
                                    completionlistcssclass="AutoExtender" completionlistitemcssclass="AutoExtenderList"
                                    completionlisthighlighteditemcssclass="AutoExtenderHighlight">
                                </cc1:autocompleteextender>
                            </td>
                        </tr>
                        <tr>
                            <td>Customer
                            </td>
                            <td>
                                <asp:Label ID="lblCustomer" runat="server" Enabled="false" Width="290px"></asp:Label>
                                <asp:HiddenField ID="hdnCustId" runat="server" Value="0" />
                            </td>
                            <td>
                                <asp:Label ID="lblConsigneeShipper_Title" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblShipper" runat="server"></asp:Label>
                                <asp:Label ID="lblConsignee" runat="server" Enabled="false" Width="290px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Branch</td>
                            <td>
                                <asp:Label ID="lblBranch" runat="server"></asp:Label>
                            </td>
                            <td>Port</td>
                            <td>
                                <asp:Label ID="lblPort" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Total No of Containers
                            </td>
                            <td>
                                <asp:Label ID="lblNoOfContainers" runat="server"></asp:Label>
                            </td>
                            <td>No of Packages
                            </td>
                            <td>
                                <asp:Label ID="lblNoOfPackgs" runat="server"></asp:Label>
                                <asp:Label ID="lblPackageType" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Sum of 20
                            </td>
                            <td>
                                <asp:Label ID="lblSum20" runat="server"></asp:Label>
                            </td>
                            <td>Sum of 40
                            </td>
                            <td>
                                <asp:Label ID="lblSum40" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Gross Weight</td>
                            <td>
                                <asp:Label ID="lblGrossWt" runat="server"></asp:Label></td>
                            <td>
                                <asp:Label ID="lblBOEType_Title" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblBOEType" runat="server"></asp:Label>
                            </td>
                        </tr>
                       <%-- <tr>
                            <td>
                                <asp:Label ID="lblType_Title" runat="server"></asp:Label>
                                <asp:RequiredFieldValidator ID="RFVDeliveryType" ValidationGroup="Required" runat="server" Display="Dynamic"
                                    ControlToValidate="ddDeliveryType" InitialValue="0" ErrorMessage="Please Select Delivery Type"
                                    Text="*"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlExportType" runat="server" TabIndex="2" AutoPostBack="true" OnSelectedIndexChanged="ddlExportType_SelectedIndexChanged">
                                    <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Factory Stuff" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Doc Stuff" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="On Wheel" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Excise Sealing" Value="4"></asp:ListItem>
                                </asp:DropDownList>

                                <asp:DropDownList ID="ddDeliveryType" runat="server" TabIndex="2" AutoPostBack="true" OnSelectedIndexChanged="ddDeliveryType_SelectedIndexChanged">
                                    <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Loaded" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="DeStuff" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="LCL" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Break Bulk" Value="4"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                               <td>
                                <asp:Label ID="lblEmpty_Letter" runat="server" Text="Empty Letter" ViewStateMode="Enabled" Visible="false"></asp:Label>
                                <asp:RequiredFieldValidator ID="rfvLoadedDocuments" runat="server" ControlToValidate="loadedDocuments" SetFocusOnError="true"
                                    Text="*" ErrorMessage="Please select a document to upload" ValidationGroup="Required"></asp:RequiredFieldValidator>
                            </td>
                            <td>                                 
                                  <div class="file-upload">                                  
                                 <label for="FileUpload1" class="file-upload-label"> </label>                               
                                  <asp:FileUpload ID="loadedDocuments" runat="server" CssClass="file-upload-input" Visible="false"/> 
                                <%--  <asp:Button ID="UpdBtn" Text="Upload File" runat="server" Visible="false" OnClick="UpdBtn_Click"/>--%>
                               <%-- </div>
                            </td>
                            </tr>--%>
                            <tr>
                            <td>Delivery Destination
                                <asp:RequiredFieldValidator ID="rfvDestination" runat="server" ControlToValidate="txtDeliveryDestination" SetFocusOnError="true"
                                    Text="*" ErrorMessage="Please Enter Delivery Destination" ></asp:RequiredFieldValidator><%--ValidationGroup="Required"--%>
                            </td>

                            <td>
                                <asp:TextBox ID="txtDeliveryDestination" runat="server" MaxLength="100" TabIndex="3" Width="250px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Dimension</td>
                            <td>
                                <asp:TextBox ID="txtDimension" runat="server" TextMode="MultiLine" ToolTip="Enter Dimension"
                                    TabIndex="4" PlaceHolder="Dimension" Width="250px" Visible="true"></asp:TextBox>
                            </td>
                            <td>Vehicle Place Require Date
                                <cc1:calendarextender id="calVehiclePlaceDate" runat="server" firstdayofweek="Sunday" popupbuttonid="imgVehiclePlaceDate"
                                    format="dd/MM/yyyy" popupposition="BottomRight" targetcontrolid="txtVehiclePlaceDate">
                                </cc1:calendarextender>
                                <cc1:maskededitextender id="meeVehiclePlaceDate" targetcontrolid="txtVehiclePlaceDate" mask="99/99/9999" messagevalidatortip="true"
                                    masktype="Date" autocomplete="false" runat="server">
                                </cc1:maskededitextender>
                                <cc1:maskededitvalidator id="mevVehiclePlaceDate" controlextender="meeVehiclePlaceDate" controltovalidate="txtVehiclePlaceDate" isvalidempty="false"
                                    invalidvaluemessage="Vehicle Place Require Date is invalid" invalidvalueblurredmessage="Invalid Vehicle Place Require Date" setfocusonerror="true"
                                    minimumvaluemessage="Invalid Vehicle Place Require Date" maximumvaluemessage="Invalid Date" minimumvalue="01/01/2015" maximumvalue="31/12/2025"
                                    runat="Server" validationgroup="Required" emptyvaluemessage="Please enter vehicle place require date." emptyvalueblurredtext="*"></cc1:maskededitvalidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVehiclePlaceDate" runat="server" Width="125px" placeholder="dd/mm/yyyy" TabIndex="5" ToolTip="Enter Vehicle Place Require Date."></asp:TextBox>
                                <asp:Image ID="imgVehiclePlaceDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            </td>
                        </tr>

                          <tr>
                            <td>  PickUp Address
                                 <asp:RequiredFieldValidator ID="rfvPickUpAdd" runat="server" ControlToValidate="txtPickupAdd" SetFocusOnError="true"
                                Text="*" ErrorMessage="Please Enter Pick Up Address" ValidationGroup="Required" ></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                
                                <asp:TextBox ID="txtPickupAdd" runat="server" TextMode="MultiLine" ToolTip="Enter PickUpAdd"
                                    TabIndex="4" PlaceHolder="PickUp Address" Width="250px" Visible="true" ></asp:TextBox><%--AutoPostBack="True"--%>
                            </td>
                            <td>Drop Address
                                 <asp:RequiredFieldValidator ID="rfvDropAdd" runat="server" ControlToValidate="txtDropAdd" SetFocusOnError="true"
                                Text ="*" ErrorMessage="Please Enter Drop Address" ValidationGroup="Required" ></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                  <asp:TextBox ID="txtDropAdd" runat="server" TextMode="MultiLine" ToolTip="Enter DropAdd"
                                    TabIndex="4" PlaceHolder="Drop Address" Width="250px" Visible="true" ></asp:TextBox><%--AutoPostBack="True"--%>
                            </td>
                        </tr>
                       <tr>
                           <td>
                           </td>
                           <td> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Pincode&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; City&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;State
                           </td>
                           <td></td>
                           <td>
                               &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Pincode&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; City&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; State
                           </td>
                       </tr>
                        <tr>
                            <td>Enter PickUp Pincode
                                  <asp:RequiredFieldValidator ID="rfvPickUpPin" runat="server" ControlToValidate="txtPincode1" SetFocusOnError="true"
                                 Text ="*" ErrorMessage="Please Enter PickUp Pincode " ValidationGroup="Required" ></asp:RequiredFieldValidator>
                            </td>
                            <td>
<%--                                <asp:TextBox ID="txtPincode1" runat="server" Width="115px" AutoPostBack="True" CssClass="SearchTextbox" ToolTip="Enter Pincode" placeholder="Search" TabIndex="3" OnTextChanged="txtPincode1_TextChanged"></asp:TextBox><%----%>
                               <%-- <asp:HiddenField ID="hdnPincodeId" runat="server" Value="0" />&nbsp;
                                <asp:TextBox ID="txtCity1" runat="server" Width="115px" Enabled="false"></asp:TextBox> &nbsp;
                                <asp:TextBox ID="txtState1" runat="server" Width="115px" Enabled="false"></asp:TextBox> --%>

                            <asp:TextBox ID="txtPincode1" runat="server" Width="115px" AutoPostBack="True" CssClass="SearchTextbox" ToolTip="Enter Pincode" placeholder="Search" TabIndex="3" OnTextChanged="txtPincode1_TextChanged" />
                            <asp:TextBox ID="txtCity1" runat="server" Width="115px" Enabled="false" />
                            <asp:TextBox ID="txtState1" runat="server" Width="115px" Enabled="false" />
                            <asp:HiddenField ID="hdnPincodeId" runat="server" Value="0" />


                            </td>
                            <td>Enter Drop Pincode
                                 <asp:RequiredFieldValidator ID="rvfDropPin" runat="server" ControlToValidate="txtPincode2" SetFocusOnError="true"
                                 Text ="*" ErrorMessage="Please Enter Drop Pincode " ValidationGroup="Required" ></asp:RequiredFieldValidator>
                            </td>
                            <td>                                                              
                               <%--<asp:TextBox ID="txtPincode2" runat="server" Width="115px" AutoPostBack="True" CssClass="SearchTextbox" ToolTip="Enter Pincode" placeholder="Search" TabIndex="3" OnTextChanged="txtPincode2_TextChanged"></asp:TextBox>--%>
                             <%--   <asp:HiddenField ID="hdnpinid" runat="server" Value="0" />&nbsp;
                                 <asp:TextBox ID="txtCity2" runat="server" Width="115px" Enabled="false"></asp:TextBox> &nbsp;
                                <asp:TextBox ID="txtState2" runat="server" Width="115px" Enabled="false"></asp:TextBox>--%>
                               
                                <asp:TextBox ID="txtPincode2" runat="server" Width="115px" AutoPostBack="True" CssClass="SearchTextbox" ToolTip="Enter Pincode" placeholder="Search" TabIndex="3" OnTextChanged="txtPincode2_TextChanged"></asp:TextBox>
                                <asp:HiddenField ID="hdnpinid" runat="server" Value="0" />
                                <asp:TextBox ID="txtCity2" runat="server" Width="115px" Enabled="false"></asp:TextBox>
                                <asp:TextBox ID="txtState2" runat="server" Width="115px" Enabled="false"></asp:TextBox> 


                                                       
                            </td>                        
                        </tr>    
                         <tr>
                            <td>
                                <asp:Label ID="lblType_Title" runat="server"></asp:Label>
                                <asp:RequiredFieldValidator ID="RFVDeliveryType" ValidationGroup="Required" runat="server" Display="Dynamic"
                                    ControlToValidate="ddDeliveryType" InitialValue="0" ErrorMessage="Please Select Delivery Type"
                                    Text="*"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlExportType" runat="server" TabIndex="2" AutoPostBack="true" OnSelectedIndexChanged="ddlExportType_SelectedIndexChanged">
                                    <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Factory Stuff" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Doc Stuff" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="On Wheel" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Excise Sealing" Value="4"></asp:ListItem>
                                </asp:DropDownList>

                                <asp:DropDownList ID="ddDeliveryType" runat="server" TabIndex="2" AutoPostBack="true" OnSelectedIndexChanged="ddDeliveryType_SelectedIndexChanged">
                                    <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Loaded" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="DeStuff" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="LCL" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Break Bulk" Value="4"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                               <td>
                                <asp:Label ID="lblEmpty_Letter" runat="server" Text="Empty Letter" ViewStateMode="Enabled" Visible="false"></asp:Label>
                                <asp:RequiredFieldValidator ID="rfvLoadedDocuments" runat="server" ControlToValidate="loadedDocuments" SetFocusOnError="true"
                                    Text="*" ErrorMessage="Please select a Empty Letter document to upload" ValidationGroup="Required"></asp:RequiredFieldValidator>
                            </td>
                            <td>                                 
                                  <div class="file-upload">                                  
                                 <label for="FileUpload1" class="file-upload-label"> </label>                               
                                  <asp:FileUpload ID="loadedDocuments" runat="server" CssClass="file-upload-input" Visible="false"/> 
                                <%--  <asp:Button ID="UpdBtn" Text="Upload File" runat="server" Visible="false" OnClick="UpdBtn_Click"/>--%>
                                </div>
                            </td>
                            </tr>
                        <tr>
                            <td>Remark</td>
                            <td colspan="3">
                                <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Rows="3" Width="900px" TabIndex="6"></asp:TextBox>
                            </td>
                        </tr>
                        <%--  <tr>
                            <td>
                                <asp:Label ID="lblDispatchFor" runat="server" Text="Number of vehicles required"></asp:Label>
                                <asp:RequiredFieldValidator ID="rfvDispatchFor" runat="server" ControlToValidate="txtTotalDispatch" SetFocusOnError="true"
                                    Display="Dynamic" Text="*" ValidationGroup="Required" Enabled="false"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvDispatch" runat="server" ControlToValidate="txtTotalDispatch" Operator="DataTypeCheck" Type="Integer"
                                    SetFocusOnError="true" Display="Dynamic" Text="*" ErrorMessage="Invalid number of dispatch." ValidationGroup="Required"></asp:CompareValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTotalDispatch" runat="server" TabIndex="6" Width="160px"></asp:TextBox>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>--%>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>Add Documents - Packing List</legend>
                    <div id="dvUploadNewFile2" runat="server" style="max-height: 200px; overflow: auto;">
                        <asp:FileUpload ID="fuDocument" runat="server" />
                        <asp:Button ID="btnSaveDocument" Text="Save Document" runat="server" OnClick="btnSaveDocument_Click" />
                    </div>
                    <br />
                    <div>
                        <asp:Repeater ID="rptDocument" runat="server" OnItemCommand="rptDocument_ItemCommand">
                            <HeaderTemplate>
                                <table class="table" border="0" cellpadding="0" cellspacing="0" style="width: 50%">
                                    <tr bgcolor="#FF781E">
                                        <th>Sl
                                        </th>
                                        <th>Document Name
                                        </th>
                                        <th>Action
                                        </th>
                                    </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <%#Container.ItemIndex +1%>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnkDownload" Text='<%#DataBinder.Eval(Container.DataItem,"DocumentName") %>'
                                            CommandArgument='<%# Eval("DocPath") %>' CausesValidation="false" runat="server"
                                            Width="200px" CommandName="DownloadFile"></asp:LinkButton>
                                        &nbsp;
                                        <asp:HiddenField ID="hdnDocLid" Value='<%#DataBinder.Eval(Container.DataItem,"PkId") %>'
                                            runat="server"></asp:HiddenField>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnkDelete" CommandName="Delete" ToolTip="Delete" Width="39" CausesValidation="false"
                                            runat="server" Text="Delete" Font-Underline="true" OnClientClick="return confirm('Are you sure you want to remove this document?')"></asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

