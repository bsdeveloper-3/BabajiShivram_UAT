<%@ Page Title="Job Expense Payment Details" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ExpPaymentDetails.aspx.cs"
    Inherits="AccountExpense_ExpPaymentDetails" Culture="en-GB" Debug="true" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <cc1:toolkitscriptmanager runat="server" id="ScriptManager1" scriptmode="Release" />
    <script type="text/javascript">
        function OnACCodeSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnACCodeId').value = results.AcCodeId;
        }

        function OnAcNameSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnACCodeId').value = results.AcCodeId;
        }

        function OnVendorCodeSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnVendorCodeId').value = results.AcCodeId;
        }

        function OnJobSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnJobId').value = results.JobId;
        }

    </script>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upExpPayment" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upExpPayment" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:HiddenField ID="hdnPaymentTypeId" runat="server" Value="0" />
                <asp:ValidationSummary ID="vsAddExpense" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
                <asp:HiddenField ID="hdnACCodeId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnJobId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnVendorCodeId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnRequestTypeId" runat="server" Value="0" />

                <asp:HiddenField ID="hdnPenaltyCopyPath" runat="server" Value="0" />

                <div id="divwidthCode" runat="server"></div>
                <div id="divwidthCust_Loc" runat="server"></div>
                <div id="divwidthName" runat="server"></div>
            </div>
            <div class="clear">
                <%-- <asp:Button ID="btnSavePayment" runat="server" OnClick="btnSubmit_Click" Text="Save"  ValidationGroup="Required" />
                <asp:Button ID="btnClearPayment" runat="server" OnClick="btnCancel_Click" Text="Cancel" />--%>

                <asp:Button ID="btnSubmit" Text="Save" OnClick="btnSubmit_Click" runat="server" ValidationGroup="Required" TabIndex="39" />
                <asp:Button ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="false" TabIndex="40" runat="server" />
            </div>

            <asp:FormView ID="FormView1" runat="server" DataSourceID="FormViewDataSource" DataKeyNames="lid" Width="100%" OnItemCommand="FormView1_ItemCommand">
                <ItemTemplate>
                    <fieldset id="FsBasicDetails" runat="server" style="width: 90%">
                        <legend>Job Detail</legend>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <%-- <asp:Button ID="btnSubmit" Text="Save" OnClick="btnSubmit_Click" runat="server" ValidationGroup="Required"
                                    TabIndex="39" />
                              <asp:Button ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="false" TabIndex="40"
                                    runat="server" />
                            <br />
                            <br />--%>
                            <tr>
                                <td>BS Job Number</td>
                                <td>
                                    <asp:Label ID="lblBSJobNo" runat="server" Text='<%#Eval("JobRefNo") %>'></asp:Label>
                                </td>
                                <td>Location</td>
                                <td>
                                    <asp:Label ID="lblLocation" runat="server" Text='<%#Eval("LocationCode") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Consignee Name</td>
                                <td colspan="3">
                                    <asp:Label ID="lblConsignee" runat="server" Text='<%#Eval("ConsigneeName") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Payment Type</td>
                                <td>
                                    <asp:Label ID="lblPaymentType" runat="server" Text='<%#Eval("PaymentTypeName1") %>'></asp:Label>
                                </td>
                                <td>Request Type</td>
                                <td>
                                    <asp:Label ID="lblExpenseType" runat="server" Text='<%#Eval("ExpenseTypeName") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr id="trAmount" runat="server" visible="true">
                                <td>Total Amount</td>
                                <td>
                                    <asp:Label ID="lblAmount" runat="server" Text='<%#Eval("Amount") %>'></asp:Label>
                                </td>
                                <td>Paid To</td>
                                <td>
                                    <asp:Label ID="lblPaidTo" runat="server" Text='<%#Eval("PaidTo") %>'></asp:Label>
                                </td>
                                <%--<td>Total Amount</td>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text='<%#Eval("Amount") %>'></asp:Label>
                                </td>--%>
                            </tr>
                            <tr>
                                <td>Job Type</td>
                                <td>
                                    <asp:Label ID="lblJobType" runat="server" Text='<%#Eval("JobType") %>'></asp:Label>
                                </td>
                                <td>Container Type</td>
                                <td>
                                    <asp:Label ID="LblContType" runat="server" Text='<%#Eval("ContainerTypeName") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Delivery Type</td>
                                <td>
                                    <asp:Label ID="lblDelType" runat="server" Text='<%#Eval("DeliveryType") %>'></asp:Label>
                                </td>
                                <td>Delivery Status</td>
                                <td>
                                    <asp:Label ID="lblDelStatus" runat="server" Text='<%#Eval("DeliveryStatus") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Container Size Count</td>
                                <td>20 =
                                    <asp:Label ID="lblContCount" runat="server" Text='<%#Eval("Tot20") +"&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp "+" 40 = "+ Eval("Tot40") %>'></asp:Label>
                                </td>
                                <td>Total Container Count</td>
                                <td>
                                    <asp:Label ID="lblContTotal" runat="server" Text='<%#Eval("TOT") %>'></asp:Label>
                                </td>
                            </tr>
                            <%--  <tr id="trDutyAmount" runat="server" visible="true">
                                <td>Duty Amount</td>
                                <td>
                                    <asp:Label ID="lblDutyAmount" runat="server" Text='<%#Eval("DutyAmount") %>'></asp:Label>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>--%>

                            <tr>
                                <td>Remark</td>
                                <td width="400px">
                                    <asp:Label ID="lblRemark" runat="server" Text='<%#Eval("Remark") %>'></asp:Label>
                                </td>
                                <td>Created By</td>
                                <td>
                                    <asp:Label ID="lblCreatedBy" runat="server" Text='<%#Eval("CreatedBy") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Created Date</td>
                                <td>
                                    <asp:Label ID="lblCreatedDate" runat="server" Text='<%#Eval("CreatedDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                </td>
                                <td>Documents</td>
                                <td>
                                    <asp:ImageButton ID="imgbtnDocument" runat="server" ImageUrl="~/Images/file.gif" Width="17px" Height="18px" CommandArgument='<%#Eval("lid") %>'
                                        CommandName="Documents" ToolTip="Download Documents." Style="padding-right: 0px; margin-right: 0px; padding-left: 1px" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </ItemTemplate>
            </asp:FormView>

            <fieldset>
                <legend>Customer Delivery Detail</legend>
                <asp:GridView ID="GridViewDelivery" runat="server" AutoGenerateColumns="False" CssClass="table"
                    Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                    DataSourceID="DataSourceDelivery"
                    CellPadding="4" AllowPaging="True" AllowSorting="True" PagerSettings-Position="TopAndBottom" PageSize="40">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Container No" DataField="ContainerNo" ReadOnly="true" />
                        <asp:BoundField HeaderText="Packages" DataField="NoOfPackages" ReadOnly="true" />
                        <asp:BoundField HeaderText="Vehicle No" DataField="VehicleNo" ReadOnly="true" />
                        <asp:BoundField HeaderText="Transporter" DataField="TransporterName" ReadOnly="true" />
                        <asp:BoundField HeaderText="LR No" DataField="LRNo" ReadOnly="true" />
                        <asp:BoundField HeaderText="LR Date" DataField="LRDate" DataFormatString="{0:dd/MM/yyyy}"
                            ReadOnly="true" />
                        <asp:BoundField HeaderText="Dispatch Date" DataField="DispatchDate" DataFormatString="{0:dd/MM/yyyy}"
                            ReadOnly="true" />
                    </Columns>
                </asp:GridView>
            </fieldset>

            <fieldset>
                <legend>Consolidate Jobs</legend>
                <asp:GridView ID="gvConsolidateJobDetail" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr"
                    DataKeyNames="lid" AllowPaging="false" AllowSorting="True" CssClass="table" PageSize="20"
                    DataSourceID="DataSourceConsolidateJobs">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1%>.
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="JobRefNo" HeaderText="Job Ref No" SortExpression="JobRefNo" ReadOnly="true" />
                        <asp:BoundField DataField="ConsigneeName" HeaderText="Consignee Name" SortExpression="ConsigneeName" ReadOnly="true" />
                        <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy" ReadOnly="true" />
                        <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" SortExpression="CreatedDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            </fieldset>

            <fieldset id="fsDutyPayment" runat="server" style="width: 90%">
                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">

                    <tr>
                        <td>Party Name</td>
                        <td>
                            <asp:Label ID="lblPartyName" runat="server"></asp:Label>
                        </td>
                        <td>IEC No</td>
                        <td>
                            <asp:Label ID="lblIECNo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>BOE No</td>
                        <td>
                            <asp:Label ID="lblBoeNo" runat="server"></asp:Label>
                        </td>
                        <td>BOE Date</td>
                        <td>
                            <asp:Label ID="lblBoeDate" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Location Code</td>
                        <td>
                            <asp:Label ID="lblLocCode" runat="server"></asp:Label>
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>Location Name</td>
                        <td>
                            <asp:Label ID="lblLocationCode" runat="server"></asp:Label>
                        </td>
                        <td>ACP / Non-ACP ?</td>
                        <td>
                            <asp:DropDownList ID="ddlACPNonACP" runat="server" Enabled="false">
                                <asp:ListItem Value="1" Selected="True" Text="ACP"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Non ACP"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>TR6 Challan No</td>
                        <td>
                            <%--<asp:TextBox ID="txtChallenNo" runat="server" ReadOnly="true"></asp:TextBox>--%>
                            <asp:Label ID="txtChallenNo" runat="server"></asp:Label>
                        </td>
                        <td>Duty Amount</td>
                        <td>
                            <asp:Label ID="lblDutyAmount" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Interest Amount</td>
                        <td>
                            <asp:TextBox ID="txtIntAmount" runat="server" AutoPostBack="true" OnTextChanged="CalculateAmtTotal"></asp:TextBox>
                            <%--<asp:Label ID="txtIntAmount" runat="server"></asp:Label>--%>
                        </td>
                        <td>Penalty Amount</td>
                        <td>
                            <asp:TextBox ID="txtPenaltyAmount" runat="server" AutoPostBack="true" OnTextChanged="CalculateAmtTotal"></asp:TextBox>
                            <%--<asp:Label ID="txtPenaltyAmount" runat="server"></asp:Label>--%>
                        </td>
                    </tr>
                    <tr>
                        <td>Total</td>
                        <td>
                            <asp:Label ID="lblTotal" runat="server"></asp:Label>
                        </td>
                        <td>Received Mail From (name)</td>
                        <td>
                            <asp:Label ID="lblRecdMailFrom_Name" runat="server"></asp:Label>
                            <asp:Label ID="lblRecdMailFrom_Mail" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Approved By</td>
                        <td>
                            <asp:Label ID="lblApprovedBy" runat="server"></asp:Label>
                        </td>
                        <td>RD / Duty / Penalty</td>
                        <td>
                            <asp:DropDownList ID="ddlType" runat="server" Enabled="false">
                                <asp:ListItem Selected="True" Value="1" Text="RD"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Duty"></asp:ListItem>
                                <asp:ListItem Value="3" Text="Penalty"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Advance Details</td>
                        <td>
                            <%--<asp:TextBox ID="txtAdvanceDetails" runat="server" Enabled="false" ReadOnly="true"></asp:TextBox>--%>
                            <asp:Label ID="txtAdvanceDetails" runat="server"></asp:Label>
                        </td>
                        <td>Status</td>
                        <td>
                            <asp:Label ID="lblStatus" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Penalty Approval Mail</td>
                        <td>
                            <asp:Label ID="txtPenaltyMail" runat="server"></asp:Label>
                            <%--<asp:TextBox ID="txtPenaltyMail" runat="server"></asp:TextBox>--%>
                        </td>
                        <td>Penalty Copy Upload</td>
                        <td>

                            <%--<asp:FileUpload ID="fuPenaltyCopy" runat="server" /></td>--%>
                            <asp:LinkButton ID="lnkPenaltyCopy" runat="server" Text="Download" OnClick="lnkPenaltyCopy_OnClick"></asp:LinkButton>

                            <%--<asp:HiddenField ID="hdnPenaltyCopyPath" runat="server" Value='<%#Eval("PenaltyCopyPath") %>' />--%>
                    </tr>
                </table>

            </fieldset>

            <fieldset id="fsPenaltyPayment" runat="server" style="width: 90%">
                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">

                    <tr>
                        <td>Party Name</td>
                        <td>
                            <asp:Label ID="lblPartyPenal" runat="server"></asp:Label>
                        </td>
                        <td>IEC No</td>
                        <td>
                            <asp:Label ID="lblIECPenal" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>BOE No</td>
                        <td>
                            <asp:Label ID="lblBOEPenal" runat="server"></asp:Label>
                        </td>
                        <td>BOE Date</td>
                        <td>
                            <asp:Label ID="lblBOEDtPenal" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Location Code</td>
                        <td>
                            <asp:Label ID="lblLocationPenal" runat="server"></asp:Label>
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>Location Name</td>
                        <td>
                            <asp:Label ID="lblLocationNmPenal" runat="server"></asp:Label>
                        </td>
                        <td>Approved By</td>
                        <td>
                            <asp:Label ID="lblApprovebyPenal" runat="server"></asp:Label>
                        </td>
                    </tr>

                </table>

            </fieldset>



            <fieldset id="fsStampDuty" runat="server" style="width: 90%">
                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                    <tr>
                        <td>Party Name</td>
                        <td>
                            <asp:Label ID="lblPartyName_StampDuty" runat="server"></asp:Label>
                        </td>
                        <td>Client Address</td>
                        <td>
                            <asp:Label ID="lblClientAddress" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>BOE No</td>
                        <td>
                            <asp:Label ID="lblBoeNo_StampDuty" runat="server"></asp:Label>
                        </td>
                        <td>BOE Date</td>
                        <td>
                            <asp:Label ID="lblBOEDate_StampDuty" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Assessable Value</td>
                        <td>
                            <asp:Label ID="lblAssessableValue" runat="server"></asp:Label></td>
                        <td>B/L No</td>
                        <td>
                            <asp:Label ID="lbl_BLNo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>B/L Date</td>
                        <td>
                            <asp:Label ID="lbl_BLDate" runat="server"></asp:Label>
                        </td>
                        <td>IGM</td>
                        <td>
                            <asp:Label ID="lblIGM" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Custom Duty</td>
                        <td>
                            <asp:TextBox ID="txtCustomDuty" runat="server" OnTextChanged="CalculateAmtTotal" AutoPostBack="true"></asp:TextBox></td>
                        <td>Total (Assessable Value + Custom Duty)</td>
                        <td>
                            <asp:Label ID="lblAssCustomTotal" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Stamp Duty</td>
                        <td>
                            <asp:TextBox ID="txtStampDuty" runat="server"></asp:TextBox>
                        </td>
                        <td>GST No</td>
                        <td>
                            <asp:TextBox ID="txtGSTNo" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </fieldset>

            <fieldset id="fsRTGS" runat="server" style="width: 90%">
                <legend>Payment Details</legend>
                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                    <tr>
                        <td style="width: 12%">Payment Ref No
                            <asp:RequiredFieldValidator ID="rfvRefNo" runat="server" ControlToValidate="txtRefNo" SetFocusOnError="true"
                                Display="Dynamic" ErrorMessage="Please Enter Ref No." Text="*" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRefNo" runat="server" ToolTip="Enter Ref No." Width="100px" TabIndex="1"></asp:TextBox>
                        </td>
                        <td>Payment Date
                            <cc1:calendarextender id="calPayemntDate_RTGS" runat="server" firstdayofweek="Sunday" popupbuttonid="imgPaymentDt_RTGS"
                                format="dd/MM/yyyy" popupposition="BottomRight" targetcontrolid="txtPaymentDt_RTGS">
                            </cc1:calendarextender>
                            <cc1:maskededitextender id="meePaymentDt_RTGS" targetcontrolid="txtPaymentDt_RTGS" mask="99/99/9999" messagevalidatortip="true"
                                masktype="Date" autocomplete="false" runat="server">
                            </cc1:maskededitextender>
                            <cc1:maskededitvalidator id="mevPaymentDt_RTGS" controlextender="meePaymentDt_RTGS" controltovalidate="txtPaymentDt_RTGS" isvalidempty="false"
                                invalidvaluemessage="Payment Date is invalid" invalidvalueblurredmessage="Invalid Payment Date" setfocusonerror="true"
                                minimumvaluemessage="Invalid Payment Date" maximumvaluemessage="Invalid Date" minimumvalue="01/01/2014" maximumvalue="31/12/2025"
                                runat="Server" validationgroup="Required" emptyvalueblurredtext="*" emptyvaluemessage="Please Enter Payment Date."></cc1:maskededitvalidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPaymentDt_RTGS" runat="server" Width="100px" placeholder="dd/mm/yyyy" TabIndex="1" ToolTip="Enter Payment Date."></asp:TextBox>
                            <asp:Image ID="imgPaymentDt_RTGS" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>Bank Name
                            <asp:RequiredFieldValidator ID="rfvBankName_RTGS" runat="server" ControlToValidate="txtBankName_RTGS" SetFocusOnError="true"
                                Display="Dynamic" ErrorMessage="Please Enter Bank Name." Text="*" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBankName_RTGS" runat="server" ToolTip="Enter Bank Name." TabIndex="6" Width="220px"></asp:TextBox>
                        </td>
                        <td>Upload POD
                             <asp:RequiredFieldValidator ID="rfvPOD" runat="server" ControlToValidate="fuUploadPOD" SetFocusOnError="true"
                                 Display="Dynamic" ErrorMessage="Please Upload POD." Text="*" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:FileUpload ID="fuUploadPOD" runat="server" ToolTip="Upload POD." TabIndex="2" />
                        </td>
                    </tr>

                </table>
            </fieldset>

            <fieldset id="fsCheque_DD" runat="server" style="width: 90%">
                <legend>Payment Details</legend>
                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                    <tr>
                        <td>Payment Ref No
                            <asp:RequiredFieldValidator ID="rfvChqNo" runat="server" ControlToValidate="txtChequeNo" SetFocusOnError="true"
                                Display="Dynamic" ErrorMessage="Please Enter Cheque No." Text="*" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtChequeNo" Width="150px" runat="server" ToolTip="Enter Cheque No." TabIndex="4"></asp:TextBox>
                        </td>
                        <td>Payment Date
                            <cc1:calendarextender id="calChequeDate" runat="server" firstdayofweek="Sunday" popupbuttonid="imgChequeDate"
                                format="dd/MM/yyyy" popupposition="BottomRight" targetcontrolid="txtChequeDate">
                            </cc1:calendarextender>
                            <cc1:maskededitextender id="meeChequeDate" targetcontrolid="txtChequeDate" mask="99/99/9999" messagevalidatortip="true"
                                masktype="Date" autocomplete="false" runat="server">
                            </cc1:maskededitextender>
                            <cc1:maskededitvalidator id="mevChequeDate" controlextender="meeChequeDate" controltovalidate="txtChequeDate" isvalidempty="false"
                                invalidvaluemessage="Cheque Date is invalid" invalidvalueblurredmessage="Invalid Cheque Date" setfocusonerror="true"
                                minimumvaluemessage="Invalid Cheque Date" maximumvaluemessage="Invalid Date" minimumvalue="01/01/2014" maximumvalue="31/12/2025"
                                runat="Server" validationgroup="Required" emptyvaluemessage="Please Enter Cheque Date."></cc1:maskededitvalidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtChequeDate" runat="server" Width="100px" placeholder="dd/mm/yyyy" TabIndex="5" ToolTip="Enter Cheque Date."></asp:TextBox>
                            <asp:Image ID="imgChequeDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>Bank Name
                            <asp:RequiredFieldValidator ID="rfvBankName" runat="server" ControlToValidate="txtBankName" SetFocusOnError="true"
                                Display="Dynamic" ErrorMessage="Please Enter Bank Name." Text="*" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBankName" runat="server" ToolTip="Enter Bank Name." TabIndex="6" Width="220px"></asp:TextBox>
                        </td>
                        <td>Narration</td>
                        <td>
                            <asp:TextBox ID="txtNarration" runat="server" ToolTip="Enter Narration." TabIndex="8" TextMode="MultiLine" Rows="3"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Upload Documents</td>
                        <td>
                            <asp:FileUpload ID="fuUploadDoc_ChequeDD" runat="server" />
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                </table>
            </fieldset>

            <fieldset id="fsCash" runat="server" style="width: 90%">
                <legend>Payment Details</legend>
                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                    <tr>
                        <td>Ref No
                             <asp:RequiredFieldValidator ID="rfvRefNoCash" runat="server" ControlToValidate="txtRefNo_Cash" SetFocusOnError="true"
                                 Display="Dynamic" ErrorMessage="Please Enter Ref No." Text="*" ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRefNo_Cash" runat="server" TabIndex="2" ToolTip="Enter Ref No."></asp:TextBox>
                        </td>
                        <td>Payment Date
                            <cc1:calendarextender id="calPaymentDate" runat="server" firstdayofweek="Sunday" popupbuttonid="imgPaymentDate"
                                format="dd/MM/yyyy" popupposition="BottomRight" targetcontrolid="txtPaymentDate">
                            </cc1:calendarextender>
                            <cc1:maskededitextender id="meePaymentDate" targetcontrolid="txtPaymentDate" mask="99/99/9999" messagevalidatortip="true"
                                masktype="Date" autocomplete="false" runat="server">
                            </cc1:maskededitextender>
                            <cc1:maskededitvalidator id="mevPaymentDate" controlextender="meePaymentDate" controltovalidate="txtPaymentDate" isvalidempty="false"
                                invalidvaluemessage="Payment Date is invalid" invalidvalueblurredmessage="Invalid Payment Date" setfocusonerror="true"
                                minimumvaluemessage="Invalid Payment Date" maximumvaluemessage="Invalid Date" minimumvalue="01/01/2014" maximumvalue="31/12/2025"
                                runat="Server" validationgroup="Required" emptyvalueblurredtext="*" emptyvaluemessage="Please Enter Payment Date."></cc1:maskededitvalidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPaymentDate" runat="server" Width="100px" placeholder="dd/mm/yyyy" TabIndex="1" ToolTip="Enter Payment Date."></asp:TextBox>
                            <asp:Image ID="imgPaymentDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>Upload Documents</td>
                        <td>
                            <asp:FileUpload ID="fuDoc_Cash" runat="server" />
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                </table>
            </fieldset>

            <%--   <fieldset>
                    <div>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td>Voucher No
                                </td>
                                <td>
                                    <asp:TextBox ID="txtVoucherNo" runat="server" Enabled="false" Width="150px" ToolTip="Enter Voucher No."></asp:TextBox>
                                </td>
                                <td>Date</td>
                                <td>
                                    <asp:TextBox ID="txtDate" runat="server" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Vendor Code</td>
                                <td>
                                    <asp:TextBox ID="txtVendorCode" Width="150px" runat="server" ToolTip="Enter Voucher Code."
                                        CssClass="SearchTextbox" placeholder="Search" TabIndex="1" AutoPostBack="true" OnTextChanged="txtVendorCode_TextChanged"></asp:TextBox>
                                    <div id="divwidthVendorCode" runat="server">
                                    </div>
                                    <cc1:AutoCompleteExtender ID="VendorCodeExtender" runat="server" TargetControlID="txtVendorCode"
                                        CompletionListElementID="divwidthVendorCode" ServicePath="~/WebService/AccountCodeAutoComplete.asmx"
                                        ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthVendorCode"
                                        ContextKey="1234" UseContextKey="True" OnClientItemSelected="OnVendorCodeSelected"
                                        CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                        CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                    </cc1:AutoCompleteExtender>
                                    &nbsp;&nbsp;
                                     <asp:Label ID="lblVendorCodeName" runat="server" Width="180px"></asp:Label>
                                </td>
                                <td>Currency</td>
                                <td>
                                    <asp:DropDownList ID="ddCurrency" runat="server" DataSourceID="dataSourceCurrency" AppendDataBoundItems="true" TabIndex="2" ToolTip="Select Currency."
                                        DataValueField="lId" DataTextField="Currency" Width="200px">
                                        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp; 
                                    Rate
                                    <asp:TextBox ID="txtRate" runat="server" Width="100px" TabIndex="3" ToolTip="Enter Rate."></asp:TextBox>
                                </td>
                            </tr>

                        </table>
                    </div>
                </fieldset>--%>
            <%--<fieldset>
                <legend>DOCUMENTS</legend>
                <div>
                    <table border="0" cellpadding="0" cellspacing="0" width="30%" bgcolor="white">
                        <tr>
                            <td>
                                <asp:FileUpload ID="fuDocument" runat="server" />
                                &nbsp; &nbsp;
                                <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:GridView ID="GridViewDocument" runat="server" AutoGenerateColumns="False" CssClass="table"
                        Width="50%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DocumentSqlDataSource"
                        OnRowCommand="GridViewDocument_RowCommand" CellPadding="4" AllowPaging="True"
                        AllowSorting="True" PageSize="20">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FileName" HeaderText="Document" />
                            <asp:BoundField DataField="CreatedBy" HeaderText="Uploaded By" />
                            <asp:TemplateField HeaderText="Download">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"
                                        CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

            </fieldset>--%>
            <div>
                <asp:SqlDataSource ID="FormViewDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetPaymentRequestById" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="PaymentId" SessionField="PaymentId" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceJobExpenseDetails" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetApprovedJobExpense" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                <asp:SqlDataSource ID="dataSourceCurrency" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetCurrencyMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                <asp:SqlDataSource ID="DocumentSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetExpenseDocDetails" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="PaymentId" SessionField="PaymentId" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceConsolidateJobs" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetConsolidateJobs" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="PaymentId" SessionField="PaymentId" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceDelivery" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetCustDeliveryDetail" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="PaymentId" SessionField="PaymentId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

