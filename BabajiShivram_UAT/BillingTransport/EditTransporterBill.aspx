<%@ Page Title="" Language="C#" MasterPageFile="~/TransportMaster.master" AutoEventWireup="true"
    CodeFile="EditTransporterBill.aspx.cs" Inherits="BillingTransport_EditTransporterBill"
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
        .fleft
        {
            margin-left: 2px;
        }
        table.table
        {
            white-space: normal;
        }
    </style>
    <script type="text/javascript" language="javascript">
        function Validate() {
            var txtDetentionDay = document.getElementById('ctl00_ContentPlaceHolder1_txtDetentionDay');
            if (txtDetentionDay.value != null && txtDetentionDay.value != '') {
                if (txtDetentionDay.value > '0') {
                    var txtDetentionCharges = document.getElementById('ctl00_ContentPlaceHolder1_txtDetentionCharges');
                    if (txtDetentionCharges.value == '') {
                        alert('Please Enter Detention Charges.');
                        document.getElementById('ctl00_ContentPlaceHolder1_txtDetentionCharges').focus();
                        return false;
                    }
                }
            }

            var txtDetentionCharges = document.getElementById('ctl00_ContentPlaceHolder1_txtDetentionCharges');
            if (txtDetentionCharges.value != null && txtDetentionCharges.value != '') {
                if (txtDetentionDay.value == '' || txtDetentionDay.value == '0') {
                    alert('Please Enter Detention Day.');
                    document.getElementById('ctl00_ContentPlaceHolder1_txtDetentionDay').focus();
                    return false;
                }
            }

            if (typeof (Page_ClientValidate) == 'function') {
                Page_ClientValidate('Required');
            }
        }

        function ValidateDetention() {
            var txtDetentionDay = document.getElementById('ctl00_ContentPlaceHolder1_txtDetentionDay');
            if (txtDetentionDay.value != null && txtDetentionDay.value != '') {
                if (txtDetentionDay.value > '0') {
                    var txtDetentionCharges = document.getElementById('ctl00_ContentPlaceHolder1_txtDetentionCharges');
                    if (txtDetentionCharges.value == '') {
                        alert('Please Enter Detention Charges.');
                        document.getElementById('ctl00_ContentPlaceHolder1_txtDetentionCharges').focus();
                        return false;
                    }
                }
            }

            var txtDetentionCharges = document.getElementById('ctl00_ContentPlaceHolder1_txtDetentionCharges');
            if (txtDetentionCharges.value != null && txtDetentionCharges.value != '') {
                if (txtDetentionDay.value == '' || txtDetentionDay.value == '0') {
                    alert('Please Enter Detention Day.');
                    document.getElementById('ctl00_ContentPlaceHolder1_txtDetentionDay').focus();
                    return false;
                }
            }
        }
    </script>
    <div align="center">
        <asp:Label ID="lberror" Text="" runat="server" EnableViewState="false"></asp:Label>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="Required" />
        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="vgJobNo" />
        <div>
            <asp:Label ID="lblDeliveryLid" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="lblTotal" runat="server" Visible="false"></asp:Label>
            <cc1:CalendarExtender ID="calDeliveredDate" runat="server" Enabled="True" EnableViewState="False"
                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgDeliveryDate" PopupPosition="BottomRight"
                TargetControlID="txtDeliveryDate">
            </cc1:CalendarExtender>
            <cc1:CalendarExtender ID="calReportDt" runat="server" Enabled="True" EnableViewState="False"
                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgReportDt" PopupPosition="BottomRight"
                TargetControlID="txtReportDate">
            </cc1:CalendarExtender>
            <cc1:CalendarExtender ID="calUnloadDt" runat="server" Enabled="True" EnableViewState="False"
                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgUnloadDt" PopupPosition="BottomRight"
                TargetControlID="txtUnloadDate">
            </cc1:CalendarExtender>
            <cc1:CalendarExtender ID="calEmptyReturnDt" runat="server" Enabled="True" EnableViewState="False"
                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgEmptyReturnDt"
                PopupPosition="BottomRight" TargetControlID="txtEmptyReturnDate">
            </cc1:CalendarExtender>
            <cc1:CalendarExtender ID="calNClosingDate" runat="server" Enabled="True" EnableViewState="False"
                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgnformclosingdate"
                PopupPosition="BottomRight" TargetControlID="txtNClosingDate">
            </cc1:CalendarExtender>
            <cc1:CalendarExtender ID="calNformDate" runat="server" Enabled="True" EnableViewState="False"
                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgnformdate" PopupPosition="BottomRight"
                TargetControlID="txtNFormDate">
            </cc1:CalendarExtender>
        </div>
    </div>
    <fieldset style="min-height: 380px; margin-top: 0px">
        <asp:HiddenField ID="hdnDeliveryLid" runat="server" />
        <asp:HiddenField ID="hdnLid" runat="server" />
        <asp:HiddenField ID="hdnStatusId" runat="server" />
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
                    <asp:SessionParameter Name="JobId" SessionField="JobId" Type="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>
        </fieldset>
        <fieldset id="fsAllVehicles" runat="server" style="min-height: 100px">
            <br />
            <legend>Vehicle Detail</legend>
            <div>
                <asp:GridView ID="gvDeliveredJobDetail" runat="server" AutoGenerateColumns="False"
                    CssClass="table" Width="50%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr"
                    DataSourceID="DataSourceDeliveredJobs" CellPadding="4" AllowPaging="True" AllowSorting="True"
                    PagerSettings-Position="TopAndBottom" PageSize="5">
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
                <asp:SqlDataSource ID="DataSourceDeliveredJobs" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="TR_GetDeliveredTransportJob" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <%-- <asp:SessionParameter Name="VehicleNo" SessionField="VehicleNo" Type="String" />--%>
                        <asp:SessionParameter Name="JobId" SessionField="JobId" Type="Int32" />
                        <asp:SessionParameter Name="UserId" SessionField="VendorId" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <br />
                <asp:GridView ID="gvVehicleDetails" runat="server" AutoGenerateColumns="False" CssClass="table"
                    PagerStyle-CssClass="pgr" DataKeyNames="lid,DeliveryId" AllowPaging="True" AllowSorting="True"
                    Width="99%" DataSourceID="DataSourceVehicles" PageSize="20" PagerSettings-Position="Top"
                    OnPreRender="gvVehicleDetails_PreRender" OnRowCommand="gvVehicleDetails_RowCommand"
                    OnRowDataBound="gvVehicleDetails_RowDataBound">
                    <Columns>
                        <asp:TemplateField ShowHeader="False" ItemStyle-Width="4%">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEdit" CommandName="Edit" ToolTip="Edit Delivery Details" runat="server"
                                    Text="Update" Font-Underline="true"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
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
                                <asp:Label ID="lblApprovedRate" runat="server" Text='<%#Eval("ApprovedRate") %>'
                                    ToolTip="Approved Rate for Job."></asp:Label>
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
                    SelectCommand="TR_GetTransporterVehicleDetail" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="JobId" Type="Int32" SessionField="JobId" />
                        <asp:SessionParameter SessionField="VendorId" Name="UserId" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </fieldset>
        <div class="fleft" style="float: none">
            <asp:Button ID="btnBackToVehicleDetail" runat="server" Text="Go Back" OnClick="btnBackToVehicleDetail_OnClick" Visible="false" />
        </div>
        <fieldset id="fsVehicleDetail" runat="server" style="margin-bottom: 0px">
            <legend>Vehicle Detail</legend>
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
                    <%-- <asp:SessionParameter Name="VehicleNo" SessionField="VehicleNo" Type="String" />--%>
                    <asp:SessionParameter Name="JobId" SessionField="JobId" Type="Int32" />
                    <asp:SessionParameter Name="UserId" SessionField="VendorId" Type="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>
            <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                <tr>
                    <td>
                        Vehicle No
                    </td>
                    <td>
                        <asp:Label ID="lblVehicleNo" runat="server" MaxLength="50" Width="150px"></asp:Label>
                    </td>
                    <td>
                        Vehicle Type
                    </td>
                    <td>
                        <asp:Label ID="lblVehicleType" runat="server" Enabled="false" Width="150px"></asp:Label>
                        <asp:DropDownList ID="ddVehicleType" runat="server" Width="150px" Enabled="false"
                            Visible="false">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Transporter Name
                    </td>
                    <td>
                        <asp:Label ID="lblTransporterName" runat="server" Width="230px"></asp:Label>
                    </td>
                    <td>
                        Location From
                    </td>
                    <td>
                        <asp:Label ID="lblDeliveryFrom" runat="server" MaxLength="100" Width="150px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Destination
                    </td>
                    <td>
                        <asp:Label ID="lblDestination" runat="server" MaxLength="150" Width="230px"></asp:Label>
                    </td>
                    <td>
                        Dispatch Date
                    </td>
                    <td>
                        <asp:Label ID="lblDispatchDate" runat="server" Width="150px" placeholder="dd/mm/yyyy"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        LR No
                    </td>
                    <td>
                        <asp:Label ID="lblLRNo" runat="server" MaxLength="150" Width="230px"></asp:Label>
                    </td>
                    <td>
                        LR Date
                    </td>
                    <td>
                        <asp:Label ID="lblLRDate" runat="server" Width="150px" placeholder="dd/mm/yyyy"></asp:Label>
                    </td>
                </tr>
            </table>
            <%--<table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                <tr>
                    <td>
                        Vehicle No
                    </td>
                    <td>
                        <asp:TextBox ID="txtVehicleNo" runat="server" MaxLength="50" Width="150px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        Vehicle Type
                    </td>
                    <td>
                        <asp:TextBox ID="txtVehicleType" runat="server" Enabled="false" Width="150px"></asp:TextBox>
                        <asp:DropDownList ID="ddVehicleType" runat="server" Width="150px" Enabled="false"
                            Visible="false">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Container No and Size
                    </td>
                    <td>
                        <asp:TextBox ID="txtContainerNo" MaxLength="25" runat="server" Width="150px" Enabled="false"></asp:TextBox>
                        <asp:TextBox ID="txtContainerSize" MaxLength="10" runat="server" Width="60px" Enabled="false"></asp:TextBox>
                        <asp:TextBox ID="txtContainerId" runat="server" Visible="false"></asp:TextBox>
                    </td>
                    <td>
                        No of Packages
                    </td>
                    <td>
                        <asp:TextBox ID="txtNoOfPackages" MaxLength="10" runat="server" Width="150px" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Transporter Name
                    </td>
                    <td>
                        <asp:TextBox ID="txtTransporterName" runat="server" Enabled="false" Width="230px"></asp:TextBox>
                    </td>
                    <td>
                        Location From
                    </td>
                    <td>
                        <asp:TextBox ID="txtDeliveryFrom" runat="server" MaxLength="100" Enabled="false"
                            Width="150px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Destination
                    </td>
                    <td>
                        <asp:TextBox ID="txtDestination" runat="server" MaxLength="150" Width="230px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        Dispatch Date
                    </td>
                    <td>
                        <asp:TextBox ID="txtDispatchDate" runat="server" Width="150px" placeholder="dd/mm/yyyy"
                            Enabled="false"></asp:TextBox>
                    </td>
                </tr>
            </table>--%>
        </fieldset>
        <fieldset id="fsEditDelivery" runat="server" style="margin-bottom: 0px">
            <legend>Delivery Detail</legend>
            <asp:Button ID="btnUpdateDelivery" runat="server" Text="Update" OnClientClick="return Validate();"
                ValidationGroup="Required" OnClick="btnUpdateDelivery_Click" TabIndex="19" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                CausesValidation="false" TabIndex="20" />
            <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                <tr>
                    <td>
                        Delivered Date
                        <cc1:MaskedEditExtender ID="MEditDeliveredDate" TargetControlID="txtDeliveryDate"
                            Mask="99/99/9999" MessageValidatorTip="true" MaskType="Date" AutoComplete="false"
                            runat="server">
                        </cc1:MaskedEditExtender>
                        <cc1:MaskedEditValidator ID="MEditValDeliveredDate" ControlExtender="MEditDeliveredDate"
                            EmptyValueBlurredText="*" EmptyValueMessage="Please Enter Delivered Date." ControlToValidate="txtDeliveryDate"
                            IsValidEmpty="false" InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Cargo Delivered Date is invalid"
                            MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015"
                            SetFocusOnError="true" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDeliveryDate" runat="server" Width="100px" placeholder="dd/mm/yyyy"
                            TabIndex="5"></asp:TextBox>
                        <asp:Image ID="imgDeliveryDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                            runat="server" />
                    </td>
                    <td>
                        Reporting Date
                        <cc1:MaskedEditExtender ID="meeReportDt" TargetControlID="txtReportDate" Mask="99/99/9999"
                            MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                        </cc1:MaskedEditExtender>
                        <cc1:MaskedEditValidator ID="mevReportDt" ControlExtender="meeReportDt" ControlToValidate="txtReportDate"
                            EmptyValueBlurredText="*" EmptyValueMessage="Please Enter Report Date." IsValidEmpty="false"
                            InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Report Date is invalid"
                            MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015"
                            SetFocusOnError="true" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="txtReportDate" runat="server" Width="100px" placeholder="dd/mm/yyyy"
                            TabIndex="6"></asp:TextBox>
                        <asp:Image ID="imgReportDt" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                            runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Unloading Date
                        <cc1:MaskedEditExtender ID="meeUnloadDt" TargetControlID="txtUnloadDate" Mask="99/99/9999"
                            MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                        </cc1:MaskedEditExtender>
                        <cc1:MaskedEditValidator ID="mevUnloadDt" ControlExtender="meeUnloadDt" ControlToValidate="txtUnloadDate"
                            IsValidEmpty="false" EmptyValueBlurredText="*" EmptyValueMessage="Please Enter Unloading Date."
                            InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Unloading Date is invalid"
                            MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015"
                            SetFocusOnError="true" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="txtUnloadDate" runat="server" Width="100px" placeholder="dd/mm/yyyy"
                            TabIndex="7"></asp:TextBox>
                        <asp:Image ID="imgUnloadDt" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                            runat="server" />
                    </td>
                    <td>
                        Detention Days
                        <asp:CompareValidator ID="cvDetentionDays" runat="server" ControlToValidate="txtDetentionDay"
                            Operator="DataTypeCheck" SetFocusOnError="true" Type="Integer" Text="*" ErrorMessage="Invalid Detention Days."
                            Display="Dynamic" ValidationGroup="Required"></asp:CompareValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDetentionDay" MaxLength="12" runat="server" Width="150px" TabIndex="8"
                            type="number"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Detention Charges
                        <asp:CompareValidator ID="cvDetentionCharges" runat="server" ControlToValidate="txtDetentionCharges"
                            Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" Text="*" ErrorMessage="Invalid Detention Charges."
                            Display="Dynamic" ValidationGroup="Required"></asp:CompareValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDetentionCharges" MaxLength="15" runat="server" Width="150px"
                            onchange="return ValidateDetention();" TabIndex="9"></asp:TextBox>
                    </td>
                    <td>
                        Warai Charges
                        <asp:CompareValidator ID="cvVaraiCharges" runat="server" ControlToValidate="txtVaraiCharges"
                            Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" Text="*" ErrorMessage="Invalid Varai Charges."
                            Display="Dynamic" ValidationGroup="Required"></asp:CompareValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="txtVaraiCharges" MaxLength="15" runat="server" Width="150px" TabIndex="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Empty Off Loading Charges
                        <asp:CompareValidator ID="cvEmptyLoadngChrges" runat="server" ControlToValidate="txtEmptyOffLoadingCharges"
                            Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" Text="*" ErrorMessage="Invalid Empty Off Loading Charges."
                            Display="Dynamic" ValidationGroup="Required"></asp:CompareValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmptyOffLoadingCharges" MaxLength="15" runat="server" Width="150px"
                            TabIndex="11"></asp:TextBox>
                    </td>
                    <td>
                        Tempo Union Charges
                        <asp:CompareValidator ID="cvTempoUnionChrges" runat="server" ControlToValidate="txtTempoUnionCharges"
                            Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" Text="*" ErrorMessage="Invalid Tempo Union Charges."
                            Display="Dynamic" ValidationGroup="Required"></asp:CompareValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTempoUnionCharges" MaxLength="15" runat="server" Width="150px"
                            TabIndex="12"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Transporter Freight Rate
                        <asp:CompareValidator ID="cvFrightRate" runat="server" ControlToValidate="txtFrightRate"
                            Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" Text="*" ErrorMessage="Invalid Fright Rate"
                            Display="Dynamic" ValidationGroup="Required"></asp:CompareValidator>
                        <asp:RequiredFieldValidator ID="RFVNoOfPkgs" runat="server" ControlToValidate="txtFrightRate"
                            SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Fright Rate." Display="Dynamic"
                            ValidationGroup="Required"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFrightRate" MaxLength="12" runat="server" Width="150px" TabIndex="12"></asp:TextBox>
                    </td>
                    <td>
                        Empty Container Return Date By Transporter
                        <cc1:MaskedEditExtender ID="meeEmptyRetDt" TargetControlID="txtEmptyReturnDate" Mask="99/99/9999"
                            MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                        </cc1:MaskedEditExtender>
                        <cc1:MaskedEditValidator ID="mevEmptyRetDt" ControlExtender="meeEmptyRetDt" ControlToValidate="txtEmptyReturnDate"
                            IsValidEmpty="true" EmptyValueBlurredText="*" EmptyValueMessage="Please Enter Empty Container Return Date By Transporter"
                            InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Empty Container Return Date is invalid"
                            MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015"
                            SetFocusOnError="true" runat="Server" ValidationGroup="Required"></cc1:MaskedEditValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmptyReturnDate" runat="server" Width="100px" placeholder="dd/mm/yyyy"
                            TabIndex="14"></asp:TextBox>
                        <asp:Image ID="imgEmptyReturnDt" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                            runat="server" />
                    </td>
                </tr>
                <tr id="trNFormDetail1" runat="server">
                    <td>
                        N Form No
                    </td>
                    <td>
                        <asp:Label ID="lblNFormNo" runat="server"></asp:Label>
                    </td>
                    <td>
                        N Form Date
                        <cc1:MaskedEditExtender ID="MEditNFormDate" TargetControlID="txtNFormDate" Mask="99/99/9999"
                            MessageValidatorTip="true" MaskType="Date" AutoComplete="false" runat="server">
                        </cc1:MaskedEditExtender>
                        <cc1:MaskedEditValidator ID="MEditValNFormDate" ControlExtender="MEditNFormDate"
                            ControlToValidate="txtNFormDate" IsValidEmpty="true" InvalidValueBlurredMessage="Invalid Date"
                            InvalidValueMessage="N Form Date is invalid" MinimumValueMessage="Invalid Date"
                            MaximumValueMessage="Invalid Date" MinimumValue="01/01/1900" MaximumValue="01/01/2025"
                            EmptyValueMessage="Enter N Form Date" SetFocusOnError="true" runat="Server" EmptyValueBlurredText="*"
                            ValidationGroup="Required"></cc1:MaskedEditValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNFormDate" runat="server" Width="65px" placeholder="dd/mm/yyyy"></asp:TextBox>
                        <asp:Image ID="imgnformdate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                            runat="server" />
                    </td>
                </tr>
                <tr id="trNFormDetail2" runat="server">
                    <td>
                        N Closing Date
                        <cc1:MaskedEditExtender ID="MEditNFormCloseDate" TargetControlID="txtNClosingDate"
                            Mask="99/99/9999" MessageValidatorTip="true" MaskType="Date" AutoComplete="false"
                            runat="server">
                        </cc1:MaskedEditExtender>
                        <cc1:MaskedEditValidator ID="MEditValNFormCloseDate" ControlExtender="MEditNFormCloseDate"
                            ControlToValidate="txtNClosingDate" IsValidEmpty="false" InvalidValueBlurredMessage="Invalid Date"
                            InvalidValueMessage="N Form Closing Date is invalid" MinimumValueMessage="Invalid Date"
                            MaximumValueMessage="Invalid Date" MinimumValue="01/01/1900" MaximumValue="01/01/2025"
                            EmptyValueMessage="Enter N Closing Date" SetFocusOnError="true" runat="Server"
                            EmptyValueBlurredText="*" ValidationGroup="Required"></cc1:MaskedEditValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNClosingDate" runat="server" Width="65px" Enabled="true" placeholder="dd/mm/yyyy"></asp:TextBox>
                        <asp:Image ID="imgnformclosingdate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                            runat="server" />
                    </td>
                    <td>
                        N Form Document
                    </td>
                    <td>
                        <asp:HiddenField ID="hdnCopyPath" runat="server" />
                        <asp:LinkButton ID="lnkDownloadInvoice" Text="Download Existing" CausesValidation="false"
                            runat="server" OnClick="lnkDownloadInvoice_OnClick"></asp:LinkButton>
                        &nbsp;&nbsp;&nbsp; OR &nbsp; &nbsp;&nbsp;
                        <asp:FileUpload ID="fuNformDoc" runat="server" Width="165px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Upload LR Copies
                    </td>
                    <td>
                        <asp:FileUpload ID="fuUploadLrCopies" runat="server" TabIndex="15" />
                    </td>
                    <td>
                        Upload Receipt
                    </td>
                    <td>
                        <asp:FileUpload ID="fuUploadReceipt" runat="server" TabIndex="16" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Remarks
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtRemarks" MaxLength="450" runat="server" Width="870px" TabIndex="18"
                            Rows="2" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </fieldset>
    </fieldset>
</asp:Content>
