<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="VehicleBillDetails.aspx.cs" Inherits="Transport_VehicleBillDetails"
    Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <style type="text/css">
        input[type="text" i]:disabled
        {
            background-color: rgba(211, 211, 211, 0.72);
            color: black;
        }
        table#ctl00_ContentPlaceHolder1_gvVehicleDetails textarea
        {
            /* background-color: rgba(211, 211, 211, 0.72); */
            background-color: White;
            border: none;
            color: black;
        }
        table.table
        {
            white-space: normal;
        }
    </style>
    <script type="text/javascript" language="javascript">
        function ConfirmSubmit() {
            if (confirm('Are you sure to approve the bill?')) {
            }
            else {
                return false;
            }
        }
    </script>
    <script type="text/javascript">
        function ValidateCheckBoxList(sender, args) {
            var checkBoxList = document.getElementById("<%=chklstVehicles.ClientID %>");
            var checkboxes = checkBoxList.getElementsByTagName("input");
            var isValid = false;
            for (var i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].checked) {
                    isValid = true;
                    break;
                }
            }
            args.IsValid = isValid;
        }
    </script>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlDeliveryVehicle"
            runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upnlDeliveryVehicle" runat="server" UpdateMode="Conditional"
        RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <fieldset style="min-height: 380px; margin-top: 0px; margin-bottom: 0px">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="vgTransp" />
                <asp:ValidationSummary ID="ValidationSummary2" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="vgReject" />
                <asp:Button ID="btnApprove" runat="server" OnClientClick="return ConfirmSubmit(); return false;"
                    OnClick="btnApprove_OnClick" Text="Approve Bill" />
                <asp:Button ID="btnReject" runat="server" OnClick="btnReject_OnClick" Text="Reject Bill" />
                <fieldset id="fsVehicleDetails" runat="server">
                    <legend>Vehicle Bill Details</legend>
                    <div class="clear">
                    </div>
                    <div>
                        <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                            Width="50%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataSourceID="DataSourceJobDetail"
                            CellPadding="4" AllowPaging="True" AllowSorting="True" PagerSettings-Position="TopAndBottom"
                            PageSize="5">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Job Id" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblJobId" runat="server" Text='<%#Eval("JobId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Vehicle No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVehicleNo" runat="server" Text='<%#Eval("VehicleNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BS Job No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblJobRefNo" runat="server" Text='<%#Eval("JobRefNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="No of Packages">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPkg" runat="server" Text='<%#Eval("NoOfPackages") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delivered Packages">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDeliveredPkgs" runat="server" Text='<%#Eval("DeliveredPkgs") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="DataSourceJobDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="TR_GetDeliveredTransportJob" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="JobId" SessionField="TpJobId" Type="Int32" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <asp:GridView ID="gvVehicleDetails" runat="server" AutoGenerateColumns="False" CssClass="table"
                            PagerStyle-CssClass="pgr" DataKeyNames="lid" AllowPaging="True" AllowSorting="True"
                            Width="90%" DataSourceID="DataSourceVehicles" PageSize="20" PagerSettings-Position="Top"
                            OnPreRender="gvVehicleDetails_PreRender" OnRowCommand="gvVehicleDetails_RowCommand"
                            OnRowDataBound="gvVehicleDetails_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Vehicle No" DataField="VehicleNo" ReadOnly="true" ItemStyle-Width="7%" />
                                <asp:BoundField HeaderText="Vehicle Type" DataField="VehicleType" ReadOnly="true"
                                    ItemStyle-Width="8%" />
                                <asp:BoundField HeaderText="Container No" DataField="ContainerNo" ReadOnly="true"
                                    ItemStyle-Width="5%" />
                                <asp:TemplateField HeaderText="Pkgs" SortExpression="NoOfPackages" ItemStyle-Width="2%"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNoOfPkg" runat="server" Text='<%#(String.IsNullOrEmpty(Eval("NoOfPackages").ToString()) ? "" : Eval("NoOfPackages"))%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Destination" DataField="DeliveryPoint" ReadOnly="true"
                                    ItemStyle-Width="9%" />
                                <asp:BoundField HeaderText="Delievered Date" DataField="DeliveryDate" ReadOnly="true"
                                    DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="5%" />
                                <asp:BoundField HeaderText="Report Date" DataField="ReportDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}"
                                    ItemStyle-Width="5%" />
                                <asp:BoundField HeaderText="Unload Date" DataField="UnloadDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}"
                                    ItemStyle-Width="5%" />
                                <asp:TemplateField HeaderText="Approved Rate" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblApprovedRate" runat="server" Text='<%#Eval("ApprovedRate") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Freight Rate" ItemStyle-Width="1%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFreightRate" runat="server" Text='<%#(String.IsNullOrEmpty(Eval("TPFrightRate").ToString()) ? "" : Eval("TPFrightRate"))%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Detention Days" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDetentionDays" runat="server" Text='<%#(String.IsNullOrEmpty(Eval("DetentionDays").ToString()) ? "" : Eval("DetentionDays"))%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Detention Charges" ItemStyle-Width="1%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDetentionCharges" runat="server" Text='<%#(String.IsNullOrEmpty(Eval("DetentionCharges").ToString()) ? "" : Eval("DetentionCharges"))%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Warai Charges" ItemStyle-Width="1%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblWaraiTotal" runat="server" Text='<%#(String.IsNullOrEmpty(Eval("WaraiCharges").ToString()) ? "" : Eval("WaraiCharges"))%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Empty Off Loading Charges" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmptyOffLoadingTotal" runat="server" Text='<%#(String.IsNullOrEmpty(Eval("EmptyOffLoadingCharges").ToString()) ? "" : Eval("EmptyOffLoadingCharges"))%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tempo Union Charges" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTempoUnionTotal" runat="server" Text='<%#(String.IsNullOrEmpty(Eval("TempoUnionCharges").ToString()) ? "" : Eval("TempoUnionCharges"))%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTotal" runat="server" Text='<%#Eval("TotalAmount") %>' ToolTip="Total amount."></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Empty Container Return Date" DataField="EmptyContReturnDate"
                                    ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="7%" />
                                <asp:TemplateField HeaderText="LR Copy" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnLRDoc" runat="server" ImageUrl="~/Images/download-excel.png"
                                            CommandArgument='<%#(String.IsNullOrEmpty(Eval("LrCopiesDocPath").ToString()) ? "" : Eval("LrCopiesDocPath"))%>'
                                            CommandName="downloadcopy" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Receipt" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnReceipt" runat="server" ImageUrl="~/Images/download-excel.png"
                                            CommandArgument='<%#(String.IsNullOrEmpty(Eval("ReceiptDocPath").ToString()) ? "" : Eval("ReceiptDocPath"))%>'
                                            CommandName="downloadcopy" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remarks" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtRemarks" MaxLength="450" runat="server" Width="130px" TabIndex="18"
                                            Enabled="false" Text='<%#Eval("Remark") %>' Rows="2" TextMode="MultiLine"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerTemplate>
                                <asp:GridViewPager ID="GridViewPager1" runat="server" />
                            </PagerTemplate>
                            <FooterStyle Font-Bold="true" />
                        </asp:GridView>
                        <asp:SqlDataSource ID="DataSourceVehicles" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="TR_GetBillDetailForApprval" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="VehicleNo" Type="String" SessionField="VehicleNo" />
                                <asp:SessionParameter Name="JobId" Type="Int32" SessionField="TpJobId" />
                                <asp:SessionParameter Name="TransporterID" SessionField="TransporterId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                </fieldset>
                <fieldset id="fsApprovedRate" runat="server">
                    <legend>Approved Rate Detail</legend>
                    <div>
                        <asp:GridView ID="GridViewVehicle" runat="server" AutoGenerateColumns="False" CssClass="table"
                            Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                            DataSourceID="DataSourceRate" CellPadding="4" AllowPaging="True" AllowSorting="True"
                            PageSize="20">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Vehicle No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVehicleNo" runat="server" Text='<%#BIND("VehicleNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Type" DataField="VehicleType" ReadOnly="true" />
                                <asp:BoundField HeaderText="Pkgs" DataField="Packages" ReadOnly="true" />
                                <asp:BoundField HeaderText="Approved Rate" DataField="ApprovedAmount" ReadOnly="true" />
                                <asp:BoundField HeaderText="Delivery From" DataField="DeliveryFrom" ReadOnly="true" />
                                <asp:BoundField HeaderText="Delivery Point" DataField="DeliveryTo" ReadOnly="true" />
                                <asp:BoundField HeaderText="Transporter" DataField="Transporter" ReadOnly="true" />
                                <asp:BoundField HeaderText="Dispatch Date" DataField="DispatchDate" DataFormatString="{0:dd/MM/yyyy}"
                                    ReadOnly="true" />
                                <asp:BoundField HeaderText="Unloading Date" DataField="UnloadingDate" DataFormatString="{0:dd/MM/yyyy}"
                                    ReadOnly="true" />
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="DataSourceRate" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="TR_GetApprovedRateTransporter" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="JobId" SessionField="TpJobId" Type="Int32" />
                                <asp:SessionParameter Name="TransporterID" SessionField="TransporterId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                </fieldset>
                <fieldset id="fsNform" runat="server">
                    <legend>NForm Details</legend>
                    <asp:GridView ID="gvNFormDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                        Width="50%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataSourceID="DataSourceNFormDetail"
                        CellPadding="4" AllowPaging="True" AllowSorting="True" PagerSettings-Position="TopAndBottom"
                        OnRowCommand="gvNFormDetail_RowCommand" PageSize="5">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="BS Job No">
                                <ItemTemplate>
                                    <asp:Label ID="lblJobRefNo" runat="server" Text='<%#Eval("JobRefNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="N Form No">
                                <ItemTemplate>
                                    <asp:Label ID="lblNFormNo" runat="server" Text='<%#Eval("NFormNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="N Form Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblNFormDate" runat="server" Text='<%#Eval("NFormDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="N Closing Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblNClosingDate" runat="server" Text='<%#Eval("NClosingDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="N Form Document">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgbtnNFormDoc" runat="server" ImageUrl="~/Images/download-excel.png"
                                        CausesValidation="false" CommandArgument='<%#(String.IsNullOrEmpty(Eval("NformDocPath").ToString()) ? "" : Eval("NformDocPath"))%>'
                                        CommandName="downloadnformcopy" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="DataSourceNFormDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="TR_GetNformDetailForJob" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:SessionParameter Name="JobId" SessionField="TpJobId" Type="Int32" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </fieldset>
            </fieldset>
            <!-- Reject Reason Panel -->
            <cc1:ModalPopupExtender ID="ModalPopupRejection" runat="server" DropShadow="False"
                CacheDynamicResults="false" PopupControlID="pnlRejectBill" TargetControlID="lnkDummy">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="pnlRejectBill" runat="server" CssClass="ModalPopupPanel" Width="500px">
                <div class="header">
                    <div class="fleft">
                        REJECT BILL
                    </div>
                    <div class="fright">
                        <asp:ImageButton ID="imgClose" ImageUrl="~/Images/delete.gif" runat="server" OnClick="btnCancelPopup_Click"
                            ToolTip="Close" />
                    </div>
                </div>
                <div class="m">
                </div>
                <div id="Div1" runat="server" style="max-height: 200px; overflow: auto;">
                    <asp:HiddenField ID="hdnJobId" runat="server" />
                    <!-- Lists Of All Documents -->
                    <div align="center">
                        <asp:Label ID="lbError_Popup" runat="server" Visible="true"></asp:Label>
                    </div>
                    <div align="center">
                        <asp:Label ID="lblJobNo" runat="server" Width="150px"></asp:Label>
                    </div>
                </div>
                <!-- Add new Document -->
                <div class="m clear">
                </div>
                <table cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                    <tr>
                        <asp:Label ID="lblValue" runat="server"></asp:Label>
                        <td>
                            Select Vehicle
                            <asp:CustomValidator ID="CustomValidator1" Text="*" ErrorMessage="Please select vehicle to be rejected."
                                ForeColor="Red" ClientValidationFunction="ValidateCheckBoxList" runat="server"
                                ValidationGroup="vgReject" />
                        </td>
                        <td>
                            <asp:CheckBoxList ID="chklstVehicles" runat="server" DataSourceID="DataSourceVehicleList"
                                DataTextField="VehicleNo" DataValueField="lid" RepeatDirection="Vertical" RepeatLayout="Table">
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp; Remark
                            <asp:RequiredFieldValidator ID="rfvRemark" runat="server" SetFocusOnError="true"
                                Display="Dynamic" ControlToValidate="txtRejectRemark" ValidationGroup="vgReject"
                                ForeColor="Red" ErrorMessage="Please Enter Remark." Text="*"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRejectRemark" runat="server" TextMode="MultiLine" Rows="3" Width="340px"
                                Height="42px" ToolTip="Remarks for rejecting job number."></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="btnRejectBill" Text="Reject" runat="server" OnClientClick="ValidateCheckBoxList"
                                OnClick="btnRejectBill_Click" ValidationGroup="vgReject" />
                            &nbsp;
                            <asp:Button ID="btnCancelRejection" Text="Cancel" runat="server" OnClick="btnCancelPopup_Click"
                                CausesValidation="false" />
                        </td>
                    </tr>
                </table>
                <div class="m clear">
                </div>
                <!--Document for Billing Advice- END -->
            </asp:Panel>
            <div>
                <asp:LinkButton ID="lnkDummy" runat="server" Text="" CausesValidation="false"></asp:LinkButton>
            </div>
            <asp:SqlDataSource ID="DataSourceVehicleList" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="TR_GetVehicleForRejection" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter Name="JobId" SessionField="TpJobId" Type="Int32" />
                    <asp:SessionParameter Name="TransporterID" SessionField="TransporterId" />
                </SelectParameters>
            </asp:SqlDataSource>
            <!-- Reject Reason Panel -->
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
