<%@ Page Title="Vehicle Movement" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="TransMovement.aspx.cs" Inherits="Transport_TransMovement" Culture="en-GB" EnableEventValidation="false" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <%--<script type="text/javascript" src="http://maps.google.com/maps/api/js?key=AIzaSyAQLwtye7Rk-tjI7toc-zwOvMhTKd6hxtk&v=3&libraries=geometry"></script>--%>
    <script type="text/javascript">
        
        function buttonalert(event) {
            //var retVal = confirm("Do you want to continue ?");
            //document.getElementById("Result").innerHTML =
            //    retVal;
            if (confirm("Do you want to save changes?") == true) {
                document.getElementById("mpeVehicleDeliveryAdd").showModal();
            } else {
                userPreference = "Save Cancelled!";
            }
        }
    </script>
    <style type="text/css">
        .modalBackground {
            background-color: black;
            filter: alpha(opacity=90);
            opacity: 0.5;
        }

        .cssStatus {
            border: 1px solid #cccc;
            border-radius: 3px;
            background-color: white;
            border-style: ridge;
            color: black;
        }
    </style>

        <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingchecklist" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
    <div align="center">
        <asp:Label ID="lblError" runat="server"></asp:Label>
        <asp:ValidationSummary ID="vsVehicleMovement" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="VehicleRequired" CssClass="errorMsg" />
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="true" ValidationGroup="Required" CssClass="errorMsg" />

    </div>
    <div class="m clear">
        <fieldset>
            <legend>Vehicle Movement</legend>
            <div class="m clear">
                <asp:Panel ID="pnlFilter" runat="server">
                    <div class="fleft">
                        <uc1:DataFilter ID="DataFilter1" runat="server" />
                    </div>
                    <div class="fleft" style="margin-left: 5px; padding-top: 3px;">
                        <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click">
                            <asp:Image ID="Image2" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                        </asp:LinkButton>
                    </div>
                </asp:Panel>
            </div>
            <div class="clear">
            </div>
            <asp:Label ID="lblMail" runat="server"></asp:Label>
            <asp:GridView ID="GridViewVehicle" runat="server" AutoGenerateColumns="False" CssClass="table"
                Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="TransRateId"
                DataSourceID="DataSourceVehicle" CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20" PagerSettings-Position="TopAndBottom"
                OnRowEditing="GridViewVehicle_RowEditing" OnRowCancelingEdit="GridViewVehicle_RowCancelingEdit" OnPreRender="GridViewVehicle_PreRender"
                OnRowUpdating="GridViewVehicle_RowUpdating" OnRowCommand="GridViewVehicle_RowCommand" OnRowDataBound="GridViewVehicle_RowDataBound">
                <Columns>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" CommandName="Edit" runat="server" Text="Edit" CommandArgument='<%#Eval("JobRefNo")+";"+Eval("TRRefNo")+";"+Eval("TransRateId")+";"+Eval("JobType")+";"+Eval("DeliveryTypeId")  %>'
                                ToolTip="Click To Update Vehicle Movement Detail"></asp:LinkButton>
                        </ItemTemplate>
                        <%--<EditItemTemplate>
                            <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update Movement Detail" runat="server"
                                Text="Update" ValidationGroup="VehicleRequired" OnClientClick="buttonalert(event)"></asp:LinkButton>
                            <asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel Movement Detail Update" CausesValidation="false"
                                runat="server" Text="Cancel"></asp:LinkButton>
                            <asp:HiddenField ID="hdnJobType" runat="server" Value='<%#Bind("JobType") %>' />
                            <asp:HiddenField ID="hdnDeliveryTypeId" runat="server" Value='<%#Bind("DeliveryTypeId") %>' />
                        </EditItemTemplate>--%>
                    </asp:TemplateField>
                   <%-- <asp:TemplateField HeaderText="Other Charges">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkbtnAddVehicle" runat="server" Text="Add" ToolTip="Add Vehicle Detail" CommandName="AddVehicle"
                                CommandArgument='<%#Eval("JobRefNo") + ";" + Eval("TRRefNo")+";"+Eval("TransID") + ";" + Eval("VehicleNo")+ ";" + Eval("VehicleType")+ ";" + Eval("Transporter")+ ";" + Eval("CustName")+";"+Eval("TransporterID")%>'></asp:LinkButton>
                            <asp:HiddenField ID="hdnTransporter" runat="server" Value='<%#Bind("Transporter") %>' />
                            <asp:HiddenField ID="hdnVehicleNo" runat="server" Value='<%#Bind("VehicleNo") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Vehicle">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkbtnUpdateVehicle" runat="server" Text="Modify" ToolTip="Modify Vehicle Detail" CommandName="UpdateVehicle"
                                CommandArgument='<%#Eval("TransID") + ";" + Eval("ConsolidateID")%>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgbtnDailyStatus" runat="server" ImageUrl="~/Images/report.png" Height="16px" Width="18px" CommandName="DailyStatus"
                                CommandArgument='<%#Eval("VehicleNo") + ";" + Eval("VehicleType") + ";" + Eval("DeliveryFrom") + ";" + Eval("DeliveryTo") + ";" + Eval("DispatchDate","{0:dd/MM/yyyy}") + ";" + Eval("CustomerMail") + ";" + Eval("TransID") + ";" + Eval("JobRefNo") + ";" + Eval("CustName")+ ";" + Eval("CustRefNo")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="RefNo" DataField="TRRefNo" ReadOnly="true" SortExpression="TRRefNo" />
                    <asp:BoundField HeaderText="Job No" DataField="JobRefNo" ReadOnly="true" SortExpression="JobRefNo" />
                    <asp:BoundField HeaderText="Transporter" DataField="Transporter" ReadOnly="true" SortExpression="Transporter" />
                    <asp:BoundField HeaderText="Customer" DataField="CustName" ReadOnly="true" SortExpression="Customer" />
                    <asp:BoundField HeaderText="Pkg" DataField="Packages" ReadOnly="true" Visible="false" />
                    <asp:BoundField HeaderText="Vehicles Required" DataField="VehiclesRequired" ReadOnly="true" ItemStyle-HorizontalAlign="Right" Visible="false" />
                    <asp:BoundField HeaderText="Vehicle No" DataField="VehicleNo" SortExpression="VehicleNo" ReadOnly="true" />
                    <asp:BoundField HeaderText="Type" DataField="VehicleType" ReadOnly="true" />
                    <asp:BoundField HeaderText="From" DataField="DeliveryFrom" ReadOnly="true" SortExpression="DeliveryFrom" />
                    <asp:BoundField HeaderText="To" DataField="DeliveryTo" ReadOnly="true" SortExpression="DeliveryTo" />
                    <asp:BoundField HeaderText="Delivery Type" DataField="DeliveryType" ReadOnly="true" />
                    <asp:BoundField HeaderText="Eway Bill Validity" DataField="EwayBillValidity" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" SortExpression="EwayBillValidity"/>
                    <asp:BoundField HeaderText="Dispatch Date" DataField="DispatchDate" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                    <asp:BoundField HeaderText="Empty Validity Date" DataField="EmptyValidityDate" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                    <asp:BoundField HeaderText="Reporting Date" DataField="ReportingDate" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" Visible="false" />
                    <asp:BoundField HeaderText="Loading Date" DataField="LoadingDate" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" Visible="false" />
                    <asp:TemplateField HeaderText="Reporting Date">
                        <ItemTemplate>
                            <asp:Label ID="lblReportingDate" Text='<%# Bind("ReportingDate","{0:dd/MM/yyyy}")%>' runat="server"></asp:Label>
                        </ItemTemplate>
                        <%--<EditItemTemplate>
                            <asp:TextBox ID="txtReportingDate" runat="server" Width="80px" MaxLength="10" Text='<%# Bind("ReportingDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                            <cc1:CalendarExtender ID="calReportingDate" runat="server" Enabled="True"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgReportingDate" PopupPosition="BottomRight"
                                TargetControlID="txtReportingDate">
                            </cc1:CalendarExtender>
                            <asp:Image ID="imgReportingDate" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif" runat="server" />
                            <cc1:MaskedEditExtender ID="meeReportingDate" TargetControlID="txtReportingDate" Mask="99/99/9999" MessageValidatorTip="true"
                                MaskType="Date" AutoComplete="false" runat="server">
                            </cc1:MaskedEditExtender>
                            <cc1:MaskedEditValidator ID="mevReportingDate" ControlExtender="meeReportingDate" ControlToValidate="txtReportingDate" IsValidEmpty="false"
                                EmptyValueMessage="Please Enter Reporting Date" EmptyValueBlurredText="*" InvalidValueMessage="Reporting Date is invalid"
                                InvalidValueBlurredMessage="Invalid Date" MinimumValueMessage="Invalid Reporting Date" MaximumValueMessage="Invalid Reporting Date"
                                MinimumValue="01/07/2014" MaximumValue="31/12/2024"
                                runat="Server" SetFocusOnError="true" ValidationGroup="VehicleRequired"></cc1:MaskedEditValidator>
                        </EditItemTemplate>--%>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="UnLoading Date" SortExpression="UnLoadingDate">
                        <ItemTemplate>
                            <asp:Label ID="lblUnLoadingDate" Text='<%# Bind("UnLoadingDate","{0:dd/MM/yyyy}")%>' runat="server"></asp:Label>
                        </ItemTemplate>
                        <%--<EditItemTemplate>
                            <asp:TextBox ID="txtUnLoadingDate" runat="server" Width="80px" MaxLength="10" Text='<%# Bind("UnLoadingDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                            <cc1:CalendarExtender ID="calUnLoadingDate" runat="server" Enabled="True"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgUnLoadingDate" PopupPosition="BottomRight"
                                TargetControlID="txtUnLoadingDate">
                            </cc1:CalendarExtender>
                            <asp:Image ID="imgUnLoadingDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            <%--<cc1:MaskedEditExtender ID="MskExtUnLoadingDate" TargetControlID="txtUnLoadingDate" Mask="99/99/9999" MessageValidatorTip="true"
                                MaskType="Date" AutoComplete="false" runat="server">
                            </cc1:MaskedEditExtender>
                            <cc1:MaskedEditValidator ID="MskValUnLoadingDate" ControlExtender="MskExtUnLoadingDate" ControlToValidate="txtUnLoadingDate" IsValidEmpty="false"
                                EmptyValueMessage="Please Enter UnLoading Date" EmptyValueBlurredText="*" InvalidValueMessage="UnLoading Date is invalid"
                                InvalidValueBlurredMessage="Invalid Date" MinimumValueMessage="Invalid Loading Date" MaximumValueMessage="Invalid UnLoading Date"
                                runat="Server" SetFocusOnError="true" ValidationGroup="VehicleRequired"></cc1:MaskedEditValidator>--%
                        </EditItemTemplate>--%>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cont Return Date" SortExpression="ContReturnDate">
                        <ItemTemplate>
                            <asp:Label ID="lblContReturnDate" Text='<%# Bind("ContReturnDate","{0:dd/MM/yyyy}")%>' runat="server"></asp:Label>
                        </ItemTemplate>
                       <%-- <EditItemTemplate>
                            <asp:TextBox ID="txtContReturnDate" runat="server" Width="80px" MaxLength="10" Text='<%# Bind("ContReturnDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                            <cc1:CalendarExtender ID="calContReturnDate" runat="server" Enabled="True"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgContReturnDate" PopupPosition="BottomRight"
                                TargetControlID="txtContReturnDate">
                            </cc1:CalendarExtender>
                            <asp:Image ID="imgContReturnDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            <%--<cc1:MaskedEditExtender ID="MskExtContReturnDate" TargetControlID="txtContReturnDate" Mask="99/99/9999" MessageValidatorTip="true"
                                MaskType="Date" AutoComplete="false" runat="server">
                            </cc1:MaskedEditExtender>
                            <cc1:MaskedEditValidator ID="MskValContReturnDate" ControlExtender="MskExtUnLoadingDate" ControlToValidate="txtUnLoadingDate" IsValidEmpty="true"
                                EmptyValueMessage="Please Enter Cont Return Date Date" EmptyValueBlurredText="*" InvalidValueMessage="Cont Return Date is invalid"
                                InvalidValueBlurredMessage="Invalid Date" MinimumValueMessage="Invalid Cont Return Date" MaximumValueMessage="Invalid Cont Return Date"
                                runat="Server" SetFocusOnError="true"></cc1:MaskedEditValidator>--%
                        </EditItemTemplate>--%>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Created By" DataField="CreatedBy" ReadOnly="true" />
                    <asp:BoundField HeaderText="Created Date" DataField="CreatedDate" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                    <asp:BoundField HeaderText="Billing Instruction" DataField="Instruction" ReadOnly="true" />
                </Columns>
                <PagerTemplate>
                    <asp:GridViewPager ID="GridViewPager1" runat="server" />
                </PagerTemplate>
            </asp:GridView>
        </fieldset>
        <%-- START  : Pop-up For Daily Status --%>
        <div>
            <asp:HiddenField ID="hdnDailyStatus" runat="server" Value="0" />
            <cc1:ModalPopupExtender ID="mpeDailyStatus" runat="server" TargetControlID="hdnDailyStatus" BackgroundCssClass="modalBackground" CancelControlID="imgClosePopup"
                PopupControlID="pnlDailyStatus" DropShadow="true">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="pnlDailyStatus" runat="server" CssClass="modalPopup1" BackColor="#F5F5DC" Style="border-radius: 5px" Width="810px" Height="520px" BorderStyle="Solid" BorderWidth="2px">
                <div id="div1" runat="server">
                    <table width="100%" style="border-bottom: 1px solid black">
                        <tr>
                            <td></td>
                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                        <tr>
                            <td align="center"><b><u>Daily Status</u></b>
                                <span style="float: right">
                                    <asp:ImageButton ID="imgClosePopup" ImageUrl="~/Images/delete.gif" runat="server" OnClick="imgClosePopup_Click" ToolTip="Close" />
                                </span>
                            </td>
                        </tr>
                    </table>
                    <div>
                        <asp:Panel ID="pnlDailyStatus2" runat="server" Width="800px" Height="480px" ScrollBars="Auto">
                            <div class="m clear" align="center">
                                <asp:Label ID="lblError_Popup" runat="server"></asp:Label>
                                <asp:RequiredFieldValidator ID="rfvReason" runat="server" ControlToValidate="txtDailyStatus" ErrorMessage="Please enter daily status!"
                                    SetFocusOnError="true" Display="Dynamic" ValidationGroup="vgDraftMail"></asp:RequiredFieldValidator>
                                <asp:HiddenField ID="hdnVehicleNo" runat="server" />
                                <asp:HiddenField ID="hdnDispatchDate" runat="server" />
                                <asp:HiddenField ID="hdnVehicleType" runat="server" />
                                <asp:HiddenField ID="hdnDeliveryFrom" runat="server" />
                                <asp:HiddenField ID="hdnDeliveryTo" runat="server" />
                                <asp:HiddenField ID="hdnTransReqId" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnJobRefNo" runat="server" />
                                <asp:HiddenField ID="hdnCustomer" runat="server" />
                                <asp:HiddenField ID="hdnCustRefNo" runat="server" />
                                <asp:HiddenField ID="hdnTransporter" runat="server" />

                            </div>
                            <div id="DivStatus" runat="server" style="margin-left: 10px">
                                <asp:TextBox ID="txtDailyStatus" runat="server" Rows="3" TextMode="MultiLine" TabIndex="1" Width="770px" CssClass="cssStatus"
                                    placeholder=" Current Status : " Style="border: 1px solid; border-radius: 4px" AutoPostBack="true" OnTextChanged="txtDailyStatus_TextChanged"></asp:TextBox>
                            </div>
                            <asp:Button ID="btnShowDraftMail" runat="server" Visible="false" OnClick="btnShowDraftMail_Click" Text="Show Draft Mail" ValidationGroup="vgDraftMail" Style="text-align: center" />
                            <br />
                            <%--<table border="0" cellpadding="0" cellspacing="0" width="95%" bgcolor="white" style="margin-left: 10px; border: 1px solid #293452;">
                                        <tr>
                                            <td></td>
                                        </tr>
                                    </table>--%>
                            <div id="DivABC" runat="server" style="border: 1px solid black; margin: 5px; margin-top: 0px; border-radius: 4px; max-height: 620px; max-width: 780px;">
                                <div class="m clear">
                                    <asp:RegularExpressionValidator ID="revEmailTo" runat="server" ControlToValidate="lblCustomerEmail" SetFocusOnError="true" Display="Dynamic"
                                        ErrorMessage="Enter valid addresses (comma separated if more than one)" ValidationExpression="^((\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)\s*[,]{0,1}\s*)+$" ForeColor="Red"></asp:RegularExpressionValidator>
                                    <asp:RegularExpressionValidator ID="revEmailCC" runat="server" ControlToValidate="txtMailCC" SetFocusOnError="true" Display="Dynamic"
                                        ErrorMessage="Enter valid addresses (comma separated if more than one)" ValidationExpression="^((\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)\s*[,]{0,1}\s*)+$" ForeColor="Red"></asp:RegularExpressionValidator>
                                </div>
                                <div class="m" style="padding: 10px;">
                                    <asp:Label ID="lblPopMessageEmail" runat="server" EnableViewState="false"></asp:Label>
                                    <table border="0" width="100%" style="font-weight: 700">
                                        <tr>
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  To :
                                                        <asp:TextBox ID="lblCustomerEmail" runat="server" Width="89%" CssClass="cssStatus"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  CC :
                                                        <asp:TextBox ID="txtMailCC" runat="server" Width="89%" CssClass="cssStatus"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Subject :
                                                        <asp:TextBox ID="txtSubject" runat="server" Width="89%" Enabled="false" CssClass="cssStatus"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvSubject" runat="server" ControlToValidate="txtSubject" SetFocusOnError="true"
                                                    Text="*" ErrorMessage="Subject Required" ValidationGroup="mailRequired"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="divPreviewEmail" runat="server" style="padding: 10px; background-color: white; border-radius: 3px; margin-left: 10px; margin-right: 10px; margin-bottom: 20px; border: 1px solid black; border-style: ridge">
                                </div>
                                <div id="DivSendEmail" runat="server" style="text-align: right; margin-left: 350px">
                                    <asp:Button ID="btnCancelEmailPp" runat="server" OnClick="btnCancelEmailPp_Click" Text="Cancel"></asp:Button>
                                    &nbsp;
                                            <asp:Button ID="btnSendEmail" runat="server" OnClick="btnSendEmail_Click" Text="Send Mail" Style="text-align: center" />
                                </div>
                            </div>
                            <div id="map_canvas" style="width: 700px; height: 400px"></div>
                        </asp:Panel>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <%-- END    : Pop-up For Daily Status --%>

        <%-- START  : Pop-up For Vehicle delivery update --%>
        <div>
            <asp:HiddenField ID="hdnVehicleDelivery" runat="server" Value="0" />
            <cc1:ModalPopupExtender ID="mpeVehicleDelivery" runat="server" TargetControlID="hdnVehicleDelivery" BackgroundCssClass="modalBackground" CancelControlID="imgClose_Delivery"
                PopupControlID="pnlVehicleDelivery" DropShadow="true">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="pnlVehicleDelivery" runat="server" CssClass="modalPopup1" Style="border-radius: 5px" BackColor="#F5F5DC" Width="1150px" Height="400px" BorderStyle="Solid" BorderWidth="2px">
                <div id="div2" runat="server">
                    <table width="100%" style="border-bottom: 1px solid black">
                        <tr>
                            <td></td>
                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                        <tr>
                            <td align="center"><b><u>Vehicle Detail Update</u></b>
                                <span style="float: right">
                                    <asp:ImageButton ID="imgClose_Delivery" ImageUrl="~/Images/delete.gif" runat="server" OnClick="imgClose_Delivery_Click" ToolTip="Close" />
                                </span>
                            </td>
                        </tr>
                    </table>
                    <div>
                        <asp:Panel ID="pnlVehicleDelivery2" runat="server" Width="1150px" Height="400px" ScrollBars="Auto">
                            <div class="m clear" align="center">
                                <asp:Label ID="lblErrorDeliveryPopup" runat="server"></asp:Label>
                                <asp:HiddenField ID="hdnPopupConsolidateID" runat="server" Value="0"></asp:HiddenField>
                                <asp:HiddenField ID="hdnTransReqId_Vehicle" runat="server" Value="0" />
                                <br />
                                <div>
                                    <asp:GridView ID="gvVehicleDelivery" runat="server" AutoGenerateColumns="False" CssClass="table" BorderWidth="2" BackColor="White"
                                        Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="DeliveryId,JobId"
                                        DataSourceID="DataSourceVehicleDelivery" CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20" PagerSettings-Position="TopAndBottom"
                                        OnRowEditing="gvVehicleDelivery_RowEditing" OnRowCancelingEdit="gvVehicleDelivery_RowCancelingEdit"
                                        OnRowUpdating="gvVehicleDelivery_RowUpdating">
                                        <Columns>
                                            <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEdit" CommandName="Edit" runat="server" Text="Edit" ToolTip="Click To Update Vehicle Detail"></asp:LinkButton>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update Vehicle Detail" runat="server" Text="Update" ValidationGroup="VehicleRequired"></asp:LinkButton>
                                                    <asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel Vehicle Detail Update" CausesValidation="false" runat="server" Text="Cancel"></asp:LinkButton>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sl" Visible="false">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Job No" DataField="JobRefNo" ReadOnly="true" SortExpression="JobRefNo" />
                                            <asp:TemplateField HeaderText="Vehicle No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVehicleNo" runat="server" Text='<%#Bind("VehicleNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtVehicleNo" runat="server" MaxLength="50" Text='<%#Bind("VehicleNo") %>' TabIndex="1" Width="100px"></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="LR No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLRNo" runat="server" Text='<%#Bind("LRNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtLRNo" runat="server" MaxLength="50" Text='<%#Bind("LRNo") %>' Width="100px" TabIndex="1"></asp:TextBox>
                                                    <asp:HiddenField ID="hdnRateId" runat="server" Value='<%#Bind("TransRateId") %>' />
                                                    <asp:HiddenField ID="hdnDeliveryConsolidateID" runat="server" Value='<%#Bind("DeliveryConsolidateID") %>'></asp:HiddenField>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="LR Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLRDate" runat="server" Text='<%#Bind("LRDate" , "{0:dd/MM/yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <cc1:CalendarExtender ID="CalPaymentDate" runat="server" Enabled="True" EnableViewState="False"
                                                        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgLR" PopupPosition="BottomRight"
                                                        TargetControlID="txtLRDate">
                                                    </cc1:CalendarExtender>
                                                    <cc1:MaskedEditExtender ID="MEditLRDate" TargetControlID="txtLRDate" Mask="99/99/9999"
                                                        MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                                                    </cc1:MaskedEditExtender>
                                                    <cc1:MaskedEditValidator ID="MEditValLRDate" ControlExtender="MEditLRDate" ControlToValidate="txtLRDate"
                                                        IsValidEmpty="true" InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="LR Date is invalid"
                                                        MinimumValueMessage="Invalid LR Date" MaximumValueMessage="Invalid LR Date" MinimumValue="01/01/2015"
                                                        MaximumValueBlurredMessage="Invalid Date" SetFocusOnError="true" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                                                    <asp:TextBox ID="txtLRDate" runat="server" Width="80px" TabIndex="2" placeholder="dd/mm/yyyy" Text='<%#Bind("LRDate" , "{0:dd/MM/yyyy}") %>'></asp:TextBox>
                                                    <asp:Image ID="imgLR" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif" runat="server" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Babaji Challan No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChallanNo" runat="server" Text='<%#Bind("ChallanNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtBabajiChallanNo" runat="server" MaxLength="50" Width="100px" Text='<%#Bind("ChallanNo") %>' TabIndex="3"></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Babaji Challan Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChallanDate" runat="server" Text='<%#Bind("ChallanDate" , "{0:dd/MM/yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <cc1:CalendarExtender ID="calChallanDate" runat="server" Enabled="True" EnableViewState="False"
                                                        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgChallanDate" PopupPosition="BottomRight"
                                                        TargetControlID="txtBabajiChallanDate">
                                                    </cc1:CalendarExtender>
                                                    <cc1:MaskedEditExtender ID="MEditChallanDate" TargetControlID="txtBabajiChallanDate"
                                                        Mask="99/99/9999" MessageValidatorTip="true" MaskType="Date" AutoComplete="false"
                                                        runat="server">
                                                    </cc1:MaskedEditExtender>
                                                    <cc1:MaskedEditValidator ID="MEditValChallanDate" ControlExtender="MEditChallanDate"
                                                        ControlToValidate="txtBabajiChallanDate" IsValidEmpty="true" InvalidValueBlurredMessage="Invalid Date"
                                                        InvalidValueMessage="Challan Date is invalid" MinimumValueMessage="Invalid Challan Date"
                                                        MaximumValueMessage="Invalid Challan Date" MinimumValue="01/01/2015"
                                                        SetFocusOnError="true" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                                                    <asp:TextBox ID="txtBabajiChallanDate" runat="server" Width="80px" TabIndex="4" Text='<%#Bind("ChallanDate" , "{0:dd/MM/yyyy}") %>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                                    <asp:Image ID="imgChallanDate" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif" runat="server" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>

                                    <asp:SqlDataSource ID="DataSourceVehicleDelivery" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                        SelectCommand="TR_GetVehicleDeliveryByTRId" SelectCommandType="StoredProcedure">
                                        <SelectParameters>
                                            <asp:SessionParameter Name="TransReqId" SessionField="TransReqId" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <%-- END    : Pop-up For Vehicle delivery update --%>


        <%-- START  : Pop-up For Vehicle delivery Add 23-09-2020--%>
        <div>
            <cc1:ModalPopupExtender ID="mpeVehicleDeliveryAdd" runat="server" TargetControlID="hdnVehicleDelivery" BackgroundCssClass="modalBackground" CancelControlID="imgClose_Delivery"
                PopupControlID="pnlVehicleDeliveryAdd" DropShadow="true">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="pnlVehicleDeliveryAdd" runat="server" CssClass="modalPopup1" Style="border-radius: 5px" BackColor="#F5F5DC" Width="1150px" Height="400px" BorderStyle="Solid" BorderWidth="2px">
                <div id="div" runat="server">
                    <table width="100%" style="border-bottom: 1px solid black">
                        <tr>
                            <td></td>
                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                        <tr>
                            <td align="center"><b><u>Vehicle Detail Add</u></b>
                                <span style="float: right">
                                    <asp:ImageButton ID="imgClose_DeliveryAdd" ImageUrl="~/Images/delete.gif" runat="server" OnClick="imgClose_DeliveryAdd_Click" ToolTip="Close" />
                                </span>
                            </td>
                        </tr>
                    </table>
                    <div>
                        <asp:Panel ID="pnlVehicleDeliveryAdd2" runat="server" Width="1150px" Height="400px" ScrollBars="Auto">
                            <div class="m clear" runat="server" align="center">
                                <asp:Label ID="lblmsg" runat="server"></asp:Label>
                            </div>

                            <div runat="server" align="center" class="m clear">
                                <table border="1" cellpadding="0" cellspacing="0" bgcolor="white" width="100%">
                                    <tr>
                                        <td>Job No </td>
                                        <td>
                                            <asp:Label ID="lblJobNo" runat="server"></asp:Label></td>
                                        <td>Ref No </td>
                                        <td>
                                            <asp:Label ID="lblRefNo" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Transporter</td>
                                        <td>
                                            <asp:Label ID="lblTransporter" runat="server"></asp:Label>
                                        </td>
                                        <td>Consignee Name</td>
                                        <td>
                                            <asp:Label ID="lblConsigneeNm" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Vehicle No</td>
                                        <td>
                                            <asp:Label ID="lblVehicleNumber" runat="server"></asp:Label>
                                        </td>
                                        <td>Type</td>
                                        <td>
                                            <asp:Label ID="lblType" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <table id="tblEntryTable" border="1" cellpadding="0" cellspacing="0" bgcolor="white" width="100%" runat="server" visible="false">
                                    <tr>
                                        <td>Selling Detail
                                        </td>
                                        <td>Amount</td>
                                        <td>Approval Copy</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkDetention" runat="server" Text="Detention" Checked="false"
                                                OnCheckedChanged="chkDetention_CheckedChanged" AutoPostBack="true" />
                                        </td>
                                        <td>
                                            <span id="sp1" runat="server" style="color: red" visible="false">*</span>
                                            <asp:TextBox ID="txtDetention" runat="server" Enabled="false"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RFVFile" runat="server" ControlToValidate="txtDetention"
                                                InitialValue="" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Required"
                                                ValidationGroup="Required" Enabled="false"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <span id="sp2" runat="server" style="color: red" visible="false">*</span>
                                            <asp:FileUpload ID="fuDetention" runat="server" Enabled="false" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="fuDetention"
                                                InitialValue="" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Required"
                                                ValidationGroup="Required" Enabled="false"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkVarai" runat="server" Text="Varai" Checked="false"
                                                OnCheckedChanged="chkVarai_CheckedChanged" AutoPostBack="true" />
                                        </td>
                                        <td>
                                            <span id="sp3" runat="server" style="color: red" visible="false">*</span>
                                            <asp:TextBox ID="txtVarai" runat="server" Enabled="false"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtVarai"
                                                InitialValue="" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Enter Varai Amount"
                                                ValidationGroup="Required" Enabled="false"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <span id="sp4" runat="server" style="color: red" visible="false">*</span>
                                            <asp:FileUpload ID="fuVarai" runat="server" Enabled="false" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="fuVarai"
                                                InitialValue="" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Upload Varai Copy"
                                                ValidationGroup="Required" Enabled="false"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkEmptyContCharge" runat="server" Text="Empty Cont Charge" Checked="false"
                                                OnCheckedChanged="chkEmptyContCharge_CheckedChanged" AutoPostBack="true" />
                                        </td>
                                        <td>
                                            <span id="sp5" runat="server" style="color: red" visible="false">*</span>
                                            <asp:TextBox ID="txtEmptyContCharge" runat="server" Enabled="false"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtEmptyContCharge"
                                                InitialValue="" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Enter Empty cont Amount"
                                                ValidationGroup="Required" Enabled="false"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <span id="sp6" runat="server" style="color: red" visible="false">*</span>
                                            <asp:FileUpload ID="fuEmptyContCharge" runat="server" Enabled="false" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="fuEmptyContCharge"
                                                InitialValue="" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Upload Empty Cont Copy"
                                                ValidationGroup="Required" Enabled="false"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkTollCharges" runat="server" Text="Toll Charge" Checked="false"
                                                OnCheckedChanged="chkTollCharges_CheckedChanged" AutoPostBack="true" />
                                        </td>
                                        <td>
                                            <span id="sp7" runat="server" style="color: red" visible="false">*</span>
                                            <asp:TextBox ID="txtTollCharge" runat="server" Enabled="false"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtTollCharge"
                                                InitialValue="" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Enter Toll Amount"
                                                ValidationGroup="Required" Enabled="false"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <span id="sp8" runat="server" style="color: red" visible="false">*</span>
                                            <asp:FileUpload ID="fuTollCharge" runat="server" Enabled="false" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="fuTollCharge"
                                                InitialValue="" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Upload Toll Copy"
                                                ValidationGroup="Required" Enabled="false"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkOtherCharge" runat="server" Text="Other Charge" Checked="false"
                                                OnCheckedChanged="chkOtherCharge_CheckedChanged" AutoPostBack="true" />
                                        </td>
                                        <td>
                                            <span id="sp9" runat="server" style="color: red" visible="false">*</span>
                                            <asp:TextBox ID="txtOtherCharge" runat="server" Enabled="false"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtOtherCharge"
                                                InitialValue="" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Enter Other Copy"
                                                ValidationGroup="Required" Enabled="false"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <span id="sp10" runat="server" style="color: red" visible="false">*</span>
                                            <asp:FileUpload ID="fuOther" runat="server" Enabled="false" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="fuOther"
                                                InitialValue="" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Upload Other Copy"
                                                ValidationGroup="Required" Enabled="false"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Remark
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtBillingInstruction" runat="server" Width="400px" Rows="3" TextMode="MultiLine" TabIndex="15"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <table id="tblSave" runat="server" visible="false" border="0" cellpadding="0" cellspacing="0" bgcolor="white" width="15%">
                                    <tr align="center">
                                        <td>
                                            <asp:Button ID="btnSave" runat="server" Text="Save" Style="text-align: center"
                                                ValidationGroup="Required" OnClick="btnSave_Click"
                                                OnClientClick="if (!Page_ClientValidate('Required')){ return false; } this.disabled = true; this.value = 'Saving...';" UseSubmitBehavior="false" />
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" Style="text-align: center" />
                                        </td>
                                    </tr>
                                </table>

                                <div id ="dvgridDisplay" runat="server" visible="false">
                                    <asp:GridView ID="GVVehicle" runat="server" AutoGenerateColumns="False" CssClass="table" Visible="true"
                                        Width="90%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" 
                                        CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="false" DataSourceID="DataSourceRate"
                                        PageSize="20" FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False"
                                        FooterStyle-BackColor="#CCCCFF" autopostback="true"  OnRowCommand="GVVehicle_RowCommand">
                                        <%--"--%>
                                        <Columns>
                                            
                                            <asp:BoundField DataField="DocumentName" HeaderText="Document Name" SortExpression="DocumentName" ReadOnly="true" />
                                            <asp:TemplateField HeaderText="Download">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDownloadDetentionDoc" runat="server" Text="Download" CommandName="DownloadCopy"
                                                        CommandArgument='<%#Eval("DocPath")+";"+ Eval("DocumentName") %>'
                                                        Visible='<%# DecideHere((string)Eval("DocPath")) %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="amount" HeaderText="Amount" SortExpression="amount" ReadOnly="true" />
                                            <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="UserDate" HeaderText="User Date" SortExpression="UserDate" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                            <%--<asp:TemplateField HeaderText="Download">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDownloadDetentionDoc" runat="server" Text="Download" CommandName="DownloadDetentionCopy"
                                                        CommandArgument='<%#Eval("DetentionDoc") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Download">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDownloadVaraiDoc" runat="server" Text="Download" CommandName="DownloadVaraiCopy"
                                                        CommandArgument='<%#Eval("VaraiDoc") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Instruction" HeaderText="Billing Instruction" SortExpression="Instruction" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />--%>
                                        </Columns>
                                    </asp:GridView>
                                </div>

                            </div>

                            <div runat="server" align="center" class="m clear">
                            </div>

                        </asp:Panel>
                    </div>
                </div>
            </asp:Panel>

        </div>
        <%-- END    : Pop-up For Vehicle delivery Add --%>

        <%-- START  : Pop-up For Movement Update 12-11-2024--%>
        <div>
            <asp:HiddenField ID="hdnMovementUpdate" runat="server" />
            <cc1:ModalPopupExtender ID="mpeMovementUpdate" runat="server" TargetControlID="hdnMovementUpdate" BackgroundCssClass="modalBackground" CancelControlID="imgClose_Delivery"
                PopupControlID="pnlMovementUpdate" DropShadow="true">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="pnlMovementUpdate" runat="server" CssClass="modalPopup1" Style="border-radius: 5px" BackColor="#F5F5DC" Width="800px" Height="200px" BorderStyle="Solid" BorderWidth="2px">
            <div>
                <table width="100%" style="border-bottom: 1px solid black">
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td align="center"><b><u>Movement Details update</u></b>
                            <span style="float: right">
                                <asp:ImageButton ID="ImageButton1" ImageUrl="~/Images/delete.gif" runat="server" OnClick="imgClose_DeliveryAdd_Click" ToolTip="Close" />
                            </span>
                        </td>
                    </tr>
                </table>
                <div class="m clear" runat="server" align="center">
                    <asp:Label ID="Label1" runat="server"></asp:Label>
                </div>

                <div runat="server" align="center" class="m clear">
                        <table border="1" cellpadding="0" cellspacing="0" bgcolor="white" width="100%">
                            <tr>
                                <td>Job No </td>
                                <td>
                                    <asp:Label ID="lblJobRefNo1" runat="server"></asp:Label></td>
                                <td>Ref No </td>
                                <td>
                                    <asp:Label ID="lblJobRefNo" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Reporting Date</td>
                                <td> 
                                    <asp:TextBox ID="txtRptDate" runat="server"></asp:TextBox>
                                    <cc1:CalendarExtender ID="calReportingDate" runat="server" Enabled="True"
                                        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgReportingDate" PopupPosition="BottomRight"
                                        TargetControlID="txtRptDate">
                                    </cc1:CalendarExtender>
                                    <asp:Image ID="imgReportingDate" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif" runat="server" />
                                    <cc1:MaskedEditExtender ID="meeReportingDate" TargetControlID="txtRptDate" Mask="99/99/9999" MessageValidatorTip="true"
                                        MaskType="Date" AutoComplete="false" runat="server">
                                    </cc1:MaskedEditExtender>
                                    <cc1:MaskedEditValidator ID="mevReportingDate" ControlExtender="meeReportingDate" ControlToValidate="txtRptDate" IsValidEmpty="false"
                                        EmptyValueMessage="Please Enter Reporting Date" EmptyValueBlurredText="*" InvalidValueMessage="Reporting Date is invalid"
                                        InvalidValueBlurredMessage="Invalid Date" MinimumValueMessage="Invalid Reporting Date" MaximumValueMessage="Invalid Reporting Date"
                                        MinimumValue="01/07/2014" MaximumValue="31/12/2024"
                                        runat="Server" SetFocusOnError="true" ValidationGroup="VehicleRequired"></cc1:MaskedEditValidator>
                                </td>
                                <td>Unloading Date</td>
                                <td>
                                    <asp:TextBox ID="txtUnloadDate" runat="server"></asp:TextBox>
                                    <cc1:CalendarExtender ID="calUnLoadingDate" runat="server" Enabled="True"
                                        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgUnLoadingDate" PopupPosition="BottomRight"
                                        TargetControlID="txtUnloadDate">
                                    </cc1:CalendarExtender>
                                    <asp:Image ID="imgUnLoadingDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>Cont Return Date</td>
                                <td>
                                    <asp:TextBox ID="txtContReturndate" runat="server"></asp:TextBox>
                                     <cc1:CalendarExtender ID="calContReturnDate" runat="server" Enabled="True"
                                        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgContReturnDate" PopupPosition="BottomRight"
                                        TargetControlID="txtContReturndate">
                                    </cc1:CalendarExtender>
                                    <asp:Image ID="imgContReturnDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                </td>
                            </tr>
                             <tr align="center">
                                <td colspan="4">
                                    <asp:Button ID="btnUpdate" runat="server" Text="Save" Style="text-align: center"
                                        ValidationGroup="Required" OnClick="btnUpdate_Click"
                                        OnClientClick="if (!Page_ClientValidate('Required')){ return false; } this.disabled = true; this.value = 'Saving...';" UseSubmitBehavior="false" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                        </table>
                       
                    </div>
            </div>
            </asp:Panel>

        </div>
        <%-- END    : Pop-up For Movement Update --%>

        <div>
            <asp:SqlDataSource ID="DataSourceVehicle" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>" CacheDuration="600"
                SelectCommand="TR_GetTransMovement" SelectCommandType="StoredProcedure"
                DataSourceMode="DataSet">
                <SelectParameters>
                    <asp:SessionParameter Name="UserId" SessionField="UserId" />
                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>

        <asp:SqlDataSource ID="DataSourceRate" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetSellTransDetail" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqId" SessionField="TransReqId" />
                <asp:ControlParameter ControlID="hdnTransReqId" Name="TransporterID" PropertyName="Value" />
                <asp:ControlParameter ControlID="hdnVehicleNo" Name="VehicleNo" PropertyName="Value" />
            </SelectParameters>
        </asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

