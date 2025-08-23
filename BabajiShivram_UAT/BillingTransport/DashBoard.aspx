<%@ Page Language="C#" MasterPageFile="~/TransportMaster.master" AutoEventWireup="true"
    CodeFile="DashBoard.aspx.cs" Inherits="BillingTransport_DashBoard" Title="Transport Dashboard"
    EnableEventValidation="false" Culture="en-GB" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <style type="text/css">
        table.table {
            white-space: normal;
        }
    </style>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPanel" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upPanel" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div style="float: left; width: 80%;">
                <div align="center">
                    <asp:HiddenField ID="hdnStatusId" runat="server" />
                    <asp:Label ID="lberror" Text="" runat="server" EnableViewState="false"></asp:Label>
                </div>
                <!-- No of pending updates for job bills -->
                <div id="dvPendingUpdates">
                    <fieldset>
                        <legend>Bill Submitted</legend>
                        <div style="overflow: scroll; height: 200px;">
                            <%--OnRowDataBound="gvPendingUpdates_RowDataBound"--%>
                            <asp:GridView ID="gvPendingUpdates" runat="server" CssClass="table" DataSourceID="DataSourcePendingUpd"
                                OnRowCommand="gvPendingUpdates_RowCommand"
                                DataKeyNames="BillID" Width="99%" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl" ItemStyle-Width="2%">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="BS Job No" DataField="JobRefNo" ReadOnly="true" ItemStyle-Width="20%" />
                                    <asp:BoundField HeaderText="Delivery From" DataField="LocationFrom"/>
                                    <asp:BoundField HeaderText="Destination" DataField="Destination"/>
                                    <asp:BoundField HeaderText="Bill Amount" DataField="BillAmount"/>
                                    <asp:BoundField HeaderText="Bill Date" DataField="BillDate" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField HeaderText="Bill Submission Date" DataField="BillSubmissionDate" DataFormatString="{0:dd/MM/yyyy}" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </fieldset>
                    <div>
                        <asp:SqlDataSource ID="DataSourcePendingUpd" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="TRS_GetBillSubmittedByVendorID" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter SessionField="CID" Name="VendorId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                    <!--Category Month Summary -->
                </div>
                <!-- No of pending updates for job bills (vehicle no & dispatch date wise) -->
                <%--<div id="dvPendingUpdatesVehicleWs">
                    <fieldset>
                        <legend>Pending Vehicles</legend>
                        <div style="overflow: scroll; height: 200px;">
                            <asp:GridView ID="gvPendingVehicles" runat="server" CssClass="table" DataSourceID="DataSourcePendingVehicle"
                                OnRowCommand="gvPendingVehicles_RowCommand" Width="99%" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl" ItemStyle-Width="2%">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Vehicle No" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVehicleNo" runat="server" Text='<%#Eval("VehicleNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="No of Jobs" SortExpression="NoofJobs" ItemStyle-Width="5%"
                                        ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnJobs" runat="server" Text='<%#Eval("NoofJobs") %>' ToolTip="Show no of jobs."
                                                CommandName="getJobs" CommandArgument='<%#Eval("VehicleNo")+","+Eval("DispatchDate")%>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Delivery From" DataField="DeliveryFrom" ReadOnly="true"
                                        ItemStyle-Width="20%" />
                                    <asp:BoundField HeaderText="Destination" DataField="DeliveryPoint" ReadOnly="true"
                                        ItemStyle-Width="20%" />
                                    <asp:BoundField HeaderText="Dispatch Date" DataField="DispatchDate" ReadOnly="true"
                                        DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="10%" />
                                    <asp:TemplateField HeaderText="Invoice Copy" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDocUpload" Text="Upload Document" CommandName="documentpopup"
                                                CausesValidation="false" runat="server" Width="100px"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </fieldset>
                    <div>
                        <asp:SqlDataSource ID="DataSourcePendingVehicle" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="TR_GetPendingVehicles" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter SessionField="VendorId" Name="UserId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                    <!--Category Month Summary -->
                </div>--%>
                <!-- No of jobs approved -->
                <div id="dvInProcessJobs">
                    <fieldset>
                        <legend>Bill - In Process</legend>
                        <div style="overflow: scroll; height: 200px;">
                            <asp:GridView ID="GridView1" runat="server" CssClass="table" DataSourceID="DataSourceInProcessJobs"
                                OnRowCommand="gvJobsApproved_RowCommand" Width="99%" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="BS Job No" DataField="JobRefNo" ReadOnly="true" ItemStyle-Width="20%" />
                                    <asp:BoundField HeaderText="Delivery From" DataField="LocationFrom"/>
                                    <asp:BoundField HeaderText="Destination" DataField="Destination"/>
                                    <asp:BoundField HeaderText="Bill Amount" DataField="BillAmount"/>
                                    <asp:BoundField HeaderText="Bill Date" DataField="BillDate" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField HeaderText="Bill Submission Date" DataField="BillSubmissionDate" DataFormatString="{0:dd/MM/yyyy}" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </fieldset>
                    <div>
                        <asp:SqlDataSource ID="DataSourceInProcessJobs" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="TRS_GetBillInProccessByVendorID" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter SessionField="CID" Name="VendorId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                </div>
                <!-- No of jobs approved -->
                <div id="dvApprovedJobs">
                    <fieldset>
                        <legend>Bill Approved</legend>
                        <div style="overflow: scroll; height: 200px;">
                            <asp:GridView ID="gvJobsApproved" runat="server" CssClass="table" DataSourceID="DataSourcememoApprovedJobs"
                                OnRowCommand="gvJobsApproved_RowCommand" Width="99%" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="BS Job No" DataField="JobRefNo" ReadOnly="true" ItemStyle-Width="20%" />
                                    <asp:BoundField HeaderText="Delivery From" DataField="LocationFrom"/>
                                    <asp:BoundField HeaderText="Destination" DataField="Destination"/>
                                    <asp:BoundField HeaderText="Bill Amount" DataField="BillAmount"/>
                                    <asp:BoundField HeaderText="Bill Date" DataField="BillDate" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField HeaderText="Bill Submission Date" DataField="BillSubmissionDate" DataFormatString="{0:dd/MM/yyyy}" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </fieldset>
                    <div>
                        <asp:SqlDataSource ID="DataSourcememoApprovedJobs" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="TRS_GetBillMemoApprovedByVendorID" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter SessionField="CID" Name="VendorId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                </div>
                <!-- No of jobs rejected -->
                <div id="dvRejectedJobs">
                    <fieldset>
                        <legend>Bill Rejected</legend>
                        <div style="overflow: scroll; height: 200px;">
                            <%-- <asp:Panel ID="pnlFilter" runat="server">
                                <div class="fleft">
                                    <uc1:DataFilter ID="DataFilter_RejectJobs" runat="server" />
                                </div>
                            </asp:Panel>
                            <br />
                            <br />--%>
                            <asp:GridView ID="gvRejectedJobs" runat="server" CssClass="table" DataSourceID="DataSourceRejectedJobs"
                                OnRowCommand="gvRejectedJobs_RowCommand" Width="99%" AutoGenerateColumns="false"
                                OnRowDataBound="gvRejectedJobs_RowDataBound" DataKeyNames="BillID" OnPreRender="gvRejectedJobs_PreRender">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl" ItemStyle-Width="2%">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BS Job No" SortExpression="JobRefNo" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnUpdatedVehicle" runat="server" Text='<%#Eval("JobRefNo") %>'
                                                ToolTip="Review Bill" CommandName="select" CommandArgument='<%#Eval("TransID") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="BS Job No" DataField="JobRefNo" Visible="false" />
                                    <asp:BoundField HeaderText="Transporter" DataField="Transporter"/>
                                    <asp:BoundField HeaderText="Delivery From" DataField="LocationFrom" ReadOnly="true"/>
                                    <asp:BoundField HeaderText="Total Vehicle" DataField="TotalVehiclePlaced" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </fieldset>
                    <div>
                        <asp:SqlDataSource ID="DataSourceRejectedJobs" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="TRS_GetRejectBillByVendorId" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <%--<asp:SessionParameter SessionField="VendorId" Name="UserId" />--%>
                                <asp:SessionParameter SessionField="CID" Name="VendorId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                    <!--Category Month Summary -->
                </div>
            </div>
            <!--Document for Doc Upload-->
            <div id="divDocument">
                <cc1:ModalPopupExtender ID="ModalPopupDocument" runat="server" CacheDynamicResults="false"
                    DropShadow="False" PopupControlID="Panel2Document" TargetControlID="lnkDummy">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="Panel2Document" runat="server" CssClass="ModalPopupPanel" Width="400px">
                    <div class="header">
                        <div class="fleft">
                            Upload Invoice Copy
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
                        <asp:HiddenField ID="hdnVehicleDeliveryId" runat="server" />
                        <asp:HiddenField ID="hdnTransportBillStatus" runat="server" />
                        <!-- Lists Of All
            Documents -->
                        <div align="center">
                            <asp:Label ID="lbError_Popup" runat="server" Visible="true"></asp:Label>
                        </div>
                    </div>
                    <!-- Add new Document -->
                    <div class="m clear">
                    </div>
                    <div id="dvUploadNewFile" runat="server" style="max-height: 200px; overflow: auto; margin-left: 15px">
                        <asp:FileUpload ID="fuDocument" runat="server" />
                        <asp:Button ID="btnSaveDocument" Text="Save Document" runat="server" OnClick="btnSaveDocument_Click"
                            CausesValidation="false" />
                    </div>
                    <div class="m clear">
                    </div>
                    <!--Document for
            BIlling Advice- END -->
                </asp:Panel>
            </div>
            <div>
                <asp:LinkButton ID="lnkDummy" runat="server" Text=""></asp:LinkButton>
            </div>
            <!--Document for Doc Upload - END
            -->
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
