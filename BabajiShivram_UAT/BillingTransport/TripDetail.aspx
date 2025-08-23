<%@ Page Title="Vehicle Details" Language="C#" MasterPageFile="~/TransportMaster.master" AutoEventWireup="true" CodeFile="TripDetail.aspx.cs"
    Inherits="BillingTransport_TripDetail" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <style type="text/css">
        input[type="text" i]:disabled {
            background-color: rgba(211, 211, 211, 0.72);
            color: black;
        }

        table textarea {
            background-color: rgba(211, 211, 211, 0.72);
            color: black;
        }

        table.table {
            white-space: normal;
        }
    </style>
    <script type="text/javascript" language="javascript">
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

    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlTripDetail"
            runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upnlTripDetail" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lberror" runat="server"></asp:Label>
            </div>
            <fieldset style="min-height: 380px; margin-top: 0px">
                <legend>Vehicle Details</legend>
                <div id="dvFilter" runat="server">
                    <div class="m clear">
                        <asp:Panel ID="pnlFilter" runat="server">
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
                    <div class="clear">
                    </div>
                </div>
                <div>
                    <asp:GridView ID="gvVendorJobDetail_New" runat="server" AutoGenerateColumns="False" CssClass="table"
                        PagerStyle-CssClass="pgr" AllowPaging="True" AllowSorting="True"
                        Width="1330px" PageSize="20" PagerSettings-Position="TopAndBottom" OnPreRender="gvVendorJobDetail_New_PreRender"
                        DataSourceID="DataSourceVendorJobs2" OnPageIndexChanging="gvVendorJobDetail_New_PageIndexChanging"
                        OnRowUpdating="gvVendorJobDetail_New_RowUpdating" OnRowEditing="gvVendorJobDetail_New_RowEditing"
                        OnRowCommand="gvVendorJobDetail_New_RowCommand" OnRowCancelingEdit="gvVendorJobDetail_New_RowCancelingEdit">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Vehicle No" DataField="VehicleNo" ReadOnly="true" />
                            <asp:BoundField HeaderText="Destination" DataField="DeliveryPoint" ReadOnly="true" />
                            <asp:BoundField HeaderText="Dispatch Date" DataField="DispatchDate" ReadOnly="true"
                                DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:TemplateField HeaderText="Detention Days">
                                <ItemTemplate>
                                    <asp:Label ID="lblDetentionDays" runat="server" Text='<%#Eval("DetentionDays").ToString() == "0" ? "" : Eval("DetentionDays")%>' ToolTip="Detention Days."></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CompareValidator ID="cvDetentionDays" runat="server" ControlToValidate="txtDetentionDays"
                                        Operator="DataTypeCheck" SetFocusOnError="true" Type="Integer" Text="*" ErrorMessage="Invalid Detention Days."
                                        Display="Dynamic" ValidationGroup="Required"></asp:CompareValidator>
                                <%-- <asp:RequiredFieldValidator ID="rfvDetentionDays" runat="server" ControlToValidate="txtDetentionDays" Display="Dynamic" ValidationGroup="Required"
                                        SetFocusOnError="true" Text="Required" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                    <asp:TextBox ID="txtDetentionDays" MaxLength="15" runat="server" Width="80px" TabIndex="9"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Detention">
                                <ItemTemplate>
                                    <asp:Label ID="lblDetentionCharges" runat="server" Text='<%#Eval("DetentionCharges").ToString() == "0.00" ? "" : Eval("DetentionCharges")%>' ToolTip="Detention Charges Of Vehicle."></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CompareValidator ID="cvDetentionCharges" runat="server" ControlToValidate="txtDetentionCharges"
                                        Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" Text="*" ErrorMessage="Invalid Detention Charges."
                                        Display="Dynamic" ValidationGroup="Required"></asp:CompareValidator>
                                    <asp:TextBox ID="txtDetentionCharges" MaxLength="15" runat="server" Width="80px"
                                        TabIndex="9"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Warai">
                                <ItemTemplate>
                                    <asp:Label ID="lblWaraiCharges" runat="server" Text='<%#Eval("WaraiCharges").ToString() == "0.00" ? "" : Eval("WaraiCharges")%>' ToolTip="Warai Charges Of Vehicle."></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CompareValidator ID="cvVaraiCharges" runat="server" ControlToValidate="txtWaraiCharges"
                                        Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" Text="*" ErrorMessage="Invalid Varai Charges."
                                        Display="Dynamic" ValidationGroup="Required"></asp:CompareValidator>
                                    <asp:TextBox ID="txtWaraiCharges" MaxLength="15" runat="server" Width="80px" TabIndex="10"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Off Loading">
                                <ItemTemplate>
                                    <asp:Label ID="lblOffLoadingCharges" runat="server" Text='<%#Eval("OffLoadingCharges").ToString() == "0.00" ? "" : Eval("OffLoadingCharges")%>' ToolTip="Empty Off Loading of Vehicles."></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CompareValidator ID="cvEmptyLoadingChrges" runat="server" ControlToValidate="txtEmptyOffLoadingCharges"
                                        Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" Text="*" ErrorMessage="Invalid Empty Off Loading Charges."
                                        Display="Dynamic" ValidationGroup="Required"></asp:CompareValidator>
                                    <asp:TextBox ID="txtEmptyOffLoadingCharges" MaxLength="15" runat="server" Width="80px" TabIndex="11"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Union">
                                <ItemTemplate>
                                    <asp:Label ID="lblTempoUnionCharges" runat="server" Text='<%#Eval("UnionCharges").ToString() == "0.00" ? "" : Eval("UnionCharges")%>' ToolTip="Tempo Union Total of Vehicles."></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CompareValidator ID="cvTempoUnionChrges" runat="server" ControlToValidate="txtTempoUnionCharges"
                                        Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" Text="*" ErrorMessage="Invalid Tempo Union Charges."
                                        Display="Dynamic" ValidationGroup="Required"></asp:CompareValidator>
                                    <asp:TextBox ID="txtTempoUnionCharges" MaxLength="15" runat="server" Width="80px" TabIndex="12"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Freight Rate">
                                <ItemTemplate>
                                    <asp:Label ID="lblFreightRate" runat="server" Text='<%#Eval("FreightCharges").ToString() == "0.00" ? "" : Eval("FreightCharges")%>' ToolTip="Freight Charges of Vehicle."></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CompareValidator ID="cvFrightRate" runat="server" ControlToValidate="txtFreightRate"
                                        Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" Text="*" ErrorMessage="Invalid Freight Rate"
                                        Display="Dynamic" ValidationGroup="Required"></asp:CompareValidator>
                                    <asp:RequiredFieldValidator ID="RFVNoOfPkgs" runat="server" ControlToValidate="txtFreightRate"
                                        SetFocusOnError="true" Text="*" ErrorMessage="Please Enter Transporter Freight Rate."
                                        Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtFreightRate" MaxLength="12" runat="server" Width="80px" TabIndex="13"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Rate">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotal" runat="server" Text='<%#Eval("TotalAmount").ToString() == "0.00" ? "" : Eval("TotalAmount")%>' ToolTip="Total amount."></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEdit" CommandName="Edit" ToolTip="Edit" Width="22" runat="server" Text="Edit" Font-Underline="true"></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update" Width="39" runat="server" Text="Update" TabIndex="2" ValidationGroup="Required" Font-Underline="true"></asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel" Width="35" runat="server" TabIndex="3" Text="Cancel" Font-Underline="true"></asp:LinkButton>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:LinkButton ID="lnkAdd" CommandName="Insert" ToolTip="Add" Width="22" runat="server" Text="Add" Font-Underline="true" TabIndex="2"></asp:LinkButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Move To Transport Dept" ItemStyle-Width="13%">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkbtnMoveForward" runat="server" Text="Move Forward" CommandName="MoveTrip"
                                        CommandArgument='<%#Eval("VehicleNo") + "," + Eval("DeliveryPoint") + "," + Eval("DispatchDate") %>'
                                        CausesValidation="false"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerTemplate>
                            <asp:GridViewPager ID="GridViewPager1" runat="server" />
                        </PagerTemplate>
                    </asp:GridView>
                    <asp:SqlDataSource ID="DataSourceVendorJobs2" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="TR_GetJobDetailForVendorTransport" SelectCommandType="StoredProcedure" UpdateCommand="insTOP_VehicleDetail"
                        UpdateCommandType="StoredProcedure" OnUpdated="DataSourceVendorJobs2_Updated">
                        <SelectParameters>
                            <asp:SessionParameter SessionField="VendorId" Name="UserId" />
                        </SelectParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="JobId" Type="Int32" />
                            <asp:Parameter Name="JobDeliveryId" Type="Int32" />
                            <asp:Parameter Name="TransporterName" Type="String" />
                            <asp:Parameter Name="Packages" Type="Int32" />
                            <asp:Parameter Name="ContainerId" Type="Int32" />
                            <asp:Parameter Name="VehicleType" Type="Int32" /> 
                            <asp:Parameter Name="DeliveryFrom" Type="String" />
                            <asp:Parameter Name="DeliveryTo" Type="String" />
                            <asp:Parameter Name="DispatchDate" Type="DateTime" />
                            <asp:Parameter Name="DeliveryDate" Type="DateTime" ConvertEmptyStringToNull="true" />
                            <asp:Parameter Name="lUser" Type="Int32" />
                            <asp:Parameter Name="VehicleNo" Type="String" />
                            <asp:Parameter Name="TPFrightRate" Type="Decimal" />
                            <asp:Parameter Name="ReportDate" Type="DateTime" ConvertEmptyStringToNull="true" />
                            <asp:Parameter Name="UnloadDate" Type="DateTime" ConvertEmptyStringToNull="true" />
                            <asp:Parameter Name="DetentionDays" Type="Int32" />
                            <asp:Parameter Name="DetentionCharges" Type="Decimal" />
                            <asp:Parameter Name="WaraiCharges" Type="Decimal" />
                            <asp:Parameter Name="EmptyOffLoadingCharges" Type="Decimal" />
                            <asp:Parameter Name="TempoUnionCharges" Type="Decimal" />
                            <asp:Parameter Name="Total" Type="Decimal" />
                            <asp:Parameter Name="Remarks" Type="String" />
                            <asp:Parameter Name="EmptyContReturnDate" Type="DateTime" ConvertEmptyStringToNull="true" />
                            <asp:Parameter Name="LrCopiesDocPath" Type="String" />
                            <asp:Parameter Name="ReceiptDocPath" Type="String" />
                            <asp:Parameter Name="OutPut" Direction="Output" Size="4" />
                        </UpdateParameters>
                    </asp:SqlDataSource>
                </div>
                <!-- Document for Doc Upload -->
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
                            <!-- Lists Of All Documents -->
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
                        <!--Document for BIlling Advice- END -->
                    </asp:Panel>
                </div>
                <div>
                    <asp:LinkButton ID="lnkDummy" runat="server" Text=""></asp:LinkButton>
                </div>
                <!-- Document for Doc Upload - END -->
            </fieldset>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSaveDocument" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

