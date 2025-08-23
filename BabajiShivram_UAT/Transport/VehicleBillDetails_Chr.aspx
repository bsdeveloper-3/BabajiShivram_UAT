<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VehicleBillDetails_Chr.aspx.cs"
    Inherits="Transport_VehicleBillDetails_Chr" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Babaji Shivram Clearing And Carriers Pvt Ltd</title>
    <link href="~/css/babaji-shivram.css" rel="stylesheet" type="text/css" />
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
</head>
<body>
    <div id="page">
        <asp:ScriptManager runat="server" ID="ScriptManager1" />
        <form runat="server" id="frmMaster" autocomplete="off">
        <input style="display: none" type="text" name="fakeusernameremembered" />
        <input style="display: none" type="password" name="fakepasswordremembered" />
        <input type="hidden" id="scrollPos" name="scrollPos" value="0" runat="server" />
        <div id="header">
            <img src="~/images/Babji-Logo.png" width="592" height="131" alt="Babaji Shivram" />
            <div class="clear">
            </div>
        </div>
        <div id="divUpdPanel" class="wrapper">
            <div id="right">
                <div class="heading">
                    <div class="UserName">
                        <span class="masterusername">Welcome</span>
                        <asp:Label ID="lbwelcomeuser" runat="server" Text="Sir"></asp:Label>
                    </div>
                    <div class="pageHeading">
                        <asp:Label ID="lblTitle" runat="server" Text="Approved Transporter Bills"></asp:Label>
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <div id="divCPH" class="CPH" onscroll="saveScrollPos();" runat="server">
                    <div>
                        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlTPBill" runat="server">
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
                                <asp:Button ID="btnBack" runat="server" OnClick="btnBack_OnClick" Text="Go Back" />
                                <asp:Button ID="btnApprove" runat="server" OnClick="btnApprove_OnClick" Text="Approve Bill" />
                                <asp:Button ID="btnReject" runat="server" OnClick="btnReject_OnClick" Text="Reject Bill" />
                                <fieldset id="fsVehicleDetails" runat="server">
                                    <legend>Vehicle Bill Details</legend>
                                    <div id="dvFilter_Vehicle" runat="server">
                                        <div class="m clear">
                                            <asp:Panel ID="pnlfilter_Vehicles" runat="server">
                                                <div class="fleft">
                                                    <uc1:DataFilter ID="DataFilter1" runat="server" />
                                                </div>
                                                <div style="margin-left: 2px;">
                                                    <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                                                        <asp:Image ID="Image2" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                                                    </asp:LinkButton>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                    </div>
                             desw       <div class="clear">

                                        <div>
                                            <%--DataSourceID="DataSourceVehicles" OnRowDataBound="gvVehicleDetails_RowDataBound"--%>
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
                                                            <asp:Label ID="lblApprovedRate" runat="server" Text='<%#Eval("ApprovedRate") %>'
                                                                ToolTip="Approved Rate for Job."></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Freight Rate" ItemStyle-Width="2%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFreightRate" runat="server" Text='<%#(String.IsNullOrEmpty(Eval("TPFrightRate").ToString()) ? "" : Eval("TPFrightRate"))%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Detention Days" ItemStyle-Width="2%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDetentionDays" runat="server" Text='<%#(String.IsNullOrEmpty(Eval("DetentionDays").ToString()) ? "" : Eval("DetentionDays"))%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Detention Charges" ItemStyle-Width="2%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDetentionCharges" runat="server" Text='<%#(String.IsNullOrEmpty(Eval("DetentionCharges").ToString()) ? "" : Eval("DetentionCharges"))%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Warai Charges" ItemStyle-Width="2%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblWaraiTotal" runat="server" Text='<%#(String.IsNullOrEmpty(Eval("WaraiCharges").ToString()) ? "" : Eval("WaraiCharges"))%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Empty Off Loading Charges" ItemStyle-Width="5%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmptyOffLoadingTotal" runat="server" Text='<%#(String.IsNullOrEmpty(Eval("EmptyOffLoadingCharges").ToString()) ? "" : Eval("EmptyOffLoadingCharges"))%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Tempo Union Charges" ItemStyle-Width="5%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTempoUnionTotal" runat="server" Text='<%#(String.IsNullOrEmpty(Eval("TempoUnionCharges").ToString()) ? "" : Eval("TempoUnionCharges"))%>'></asp:Label>
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
                                                SelectCommand="TR_GetDetailedVehicleDetails" SelectCommandType="StoredProcedure">
                                                <SelectParameters>
                                                    <asp:SessionParameter Name="JobId" Type="Int32" SessionField="TpJobId" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                        </div>
                                </fieldset>
                            </fieldset>
                            <!-- Reject Reason Panel -->
                            <cc1:ModalPopupExtender ID="ModalPopupRejection" runat="server" DropShadow="False"
                                CacheDynamicResults="false" PopupControlID="pnlRejectBill" TargetControlID="lnkDummy">
                            </cc1:ModalPopupExtender>
                            <asp:Panel ID="pnlRejectBill" runat="server" CssClass="ModalPopupPanel" Width="400px">
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
                                        <td>
                                            &nbsp; Remark
                                            <asp:RequiredFieldValidator ID="rfvRemark" runat="server" SetFocusOnError="true"
                                                Display="Dynamic" ControlToValidate="txtRejectRemark" ValidationGroup="vgReject"
                                                ForeColor="Red" ErrorMessage="Please Enter Remark." Text="*"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRejectRemark" runat="server" TextMode="MultiLine" Rows="3" Width="300px"
                                                Height="42px" ToolTip="Remarks for rejecting job number."></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Button ID="btnRejectBill" Text="Reject" runat="server" OnClick="btnRejectBill_Click"
                                                ValidationGroup="vgReject" />
                                            &nbsp;
                                            <asp:Button ID="btnCancelRejection" Text="Cancel" runat="server" OnClick="btnCancelPopup_Click"
                                                CausesValidation="false" />
                                        </td>
                                    </tr>
                                </table>
                                <%-- <div id="dvUploadNewFile" runat="server" style="max-height: 200px; overflow: auto;
                    margin-left: 15px">
                </div>--%>
                                <div class="m clear">
                                </div>
                                <!--Document for Billing Advice- END -->
                            </asp:Panel>
                            <div>
                                <asp:LinkButton ID="lnkDummy" runat="server" Text="" CausesValidation="false"></asp:LinkButton>
                            </div>
                            <!-- Reject Reason Panel -->
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="clear">
            </div>
        </div>
        <div id="copyPower">
            <div class="copy fleft">
                Copyright © 2016 Babaji Shivram. All rights reserved.</div>
            <div class="poweredby fright">
            </div>
        </div>
        </form>
        <div class="clear">
        </div>
    </div>
    <script type="text/javascript" language="javascript">

        // get total height 
        var totalHeight = document.body.offsetHeight;
        // get height of top and bottom div 
        //var topDivHeight = document.getElementById('1').offsetHeight; 
        //var bottomDivHeight = document.getElementById('3').offsetHeight; 
        // calculate height for center div and apply it 
        var centerDivHeight = totalHeight - 161;
        document.getElementById('ctl00_LeftNavigation1_trMktMenu').style.height = centerDivHeight + 'px';
        document.getElementById('<%=divCPH.ClientID%>').style.height = centerDivHeight - 40 + 'px';   
    </script>
    <script type="text/javascript" language="javascript">
        Sys.Application.add_init(AppInit);

        function AppInit(sender) {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        }

        function EndRequestHandler(sender, args) {
            setScrollPos();
        }

        function saveScrollPos() {

            document.getElementById('<%=scrollPos.ClientID%>').value =
                document.getElementById('<%=divCPH.ClientID%>').scrollTop;

        }
        function setScrollPos() {
            document.getElementById('<%=divCPH.ClientID%>').scrollTop =
                document.getElementById('<%=scrollPos.ClientID%>').value;

        }
    </script>
</body>
</html>
