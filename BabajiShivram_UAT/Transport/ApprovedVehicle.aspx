<%@ Page Title="Approved Vehicles" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="ApprovedVehicle.aspx.cs" Inherits="Transport_ApprovedVehicle"
    Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
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
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlApprovedVehicle"
            runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upnlApprovedVehicle" runat="server" UpdateMode="Conditional"
        RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <fieldset style="min-height: 380px; margin-top: 0px; margin-bottom: 0px">
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
                                    ItemStyle-HorizontalAlign="Center" Visible="false">
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
                                <asp:TemplateField HeaderText="Detention Days" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDetentionDays" runat="server" Text='<%#(String.IsNullOrEmpty(Eval("DetentionDays").ToString()) ? "" : Eval("DetentionDays"))%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Approved Rate" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblApprovedRate" runat="server" Text='<%#(String.IsNullOrEmpty(Eval("ApprovedRate").ToString()) ? "" : Eval("ApprovedRate"))%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Freight Rate" ItemStyle-Width="1%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFreightRate" runat="server" Text='<%#(String.IsNullOrEmpty(Eval("TPFrightRate").ToString()) ? "" : Eval("TPFrightRate"))%>'></asp:Label>
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
                                            CausesValidation="false" CommandArgument='<%#(String.IsNullOrEmpty(Eval("LrCopiesDocPath").ToString()) ? "" : Eval("LrCopiesDocPath"))%>'
                                            CommandName="downloadcopy" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Receipt" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnReceipt" runat="server" ImageUrl="~/Images/download-excel.png"
                                            CausesValidation="false" CommandArgument='<%#(String.IsNullOrEmpty(Eval("ReceiptDocPath").ToString()) ? "" : Eval("ReceiptDocPath"))%>'
                                            CommandName="downloadcopy" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remarks" ItemStyle-Width="35%">
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
                                <asp:SessionParameter Name="TransporterID" Type="Int32" SessionField="TransporterId" />
                                <%-- <asp:SessionParameter Name="VehicleNo" Type="String" SessionField="VehicleNo" />--%>
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                </fieldset>
                <fieldset>
                    <legend>Billing History</legend>
                    <div class="clear">
                    </div>
                    <div>
                        <asp:GridView ID="gvVehicleHistory" runat="server" AutoGenerateColumns="False" CssClass="table"
                            Width="85%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataSourceID="DataSourceHistory"
                            CellPadding="4" AllowPaging="True" AllowSorting="True" PagerSettings-Position="TopAndBottom"
                            PageSize="5">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description" ItemStyle-Width="28%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDesc" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remark" ItemStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemark" runat="server" Text='<%#Eval("Remark") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Updated By" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUpdBy" runat="server" Text='<%#Eval("UpdatedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Updated Date" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUpdDate" runat="server" Text='<%#Eval("UpdatedDate") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="DataSourceHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="TR_GetJobHistory" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="JobId" SessionField="TpJobId" Type="Int32" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                </fieldset>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
