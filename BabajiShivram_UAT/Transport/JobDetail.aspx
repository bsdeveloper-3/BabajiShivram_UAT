<%@ Page Title="Job Detail" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="JobDetail.aspx.cs"
    Inherits="Transport_JobDetail" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <cc1:toolkitscriptmanager runat="server" id="ScriptManager1" scriptmode="Release" />
    <div>
        <asp:HiddenField ID="hdnPageValid" runat="server" />
        <asp:HiddenField ID="hdnMode" runat="server" />
    </div>
    <script type="text/javascript">
        function ActiveTabChanged12() {
            /* Clear Error Message on Tab change Event */
            document.getElementById('<%=lblMessage.ClientID%>').innerHTML = '';
            document.getElementById('<%=lblMessage.ClientID%>').className = '';
        }
    </script>
    <div>
        <div align="center">
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
        </div>
    </div>
    <asp:UpdatePanel ID="upnlRequestRecd" runat="server">
        <ContentTemplate>
            <cc1:tabcontainer runat="server" id="TabRequestRecd" activetabindex="0" cssclass="Tab"
                width="100%" onclientactivetabchanged="ActiveTabChanged12" autopostback="true">
                <cc1:TabPanel runat="server" ID="TabPanelNormalJob" TabIndex="0" HeaderText="Job Detail">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Transport Request Detail</legend>
                            <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                <tr>
                                    <td>Ref No.
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTRRefNo" runat="server" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td>Job No.
                                    </td>
                                    <td>
                                        <asp:Label ID="lblJobNo" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Customer
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCustName" runat="server"></asp:Label>
                                    </td>
                                    <td>Truck Request Date
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTruckRequestDate" runat="server"></asp:Label>
                                    </td>
                                    
                                </tr>
                                <tr>
                                    <td>Division
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDivision" runat="server"></asp:Label>
                                    </td>
                                    <td>Plant
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPlant" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Location
                                    </td>
                                    <td>
                                        <asp:Label ID="lblLocationFrom" runat="server"></asp:Label>
                                    </td>
                                    <td>Destination
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDestination" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Gross Weight (Kgs)
                                    </td>
                                    <td>
                                        <b><asp:Label ID="lblGrossWeight" runat="server"></asp:Label><b>
                                        &nbsp; & &nbsp;&nbsp;
                                        <b></b><asp:Label ID="lblNoofPkg" runat="server"></asp:Label><b> &nbsp;
                                        Pkgs
                                    </td>
                                    <td>Cont 20"
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCon20" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Cont 40"
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCon40" runat="server"></asp:Label>
                                    </td>
                                    <td>Delivery Type
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDeliveryType" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                 <tr>
                                     <td>
                                            PickUp Address
                                        </td>
                                        <td>
                                             <asp:Label ID="lblPickAdd" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            Drop Address
                                        </td>
                                        <td>
                                              <asp:Label ID="lblDropAdd" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Address Details
                                        </td>
                                        <td>
                                            Pincode&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; City&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp State
                                        </td>
                                        <td> Address Details
                                        </td>
                                        <td>
                                            Pincode&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;City &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp State
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            </td>
                                        <td>
                                             <asp:Label ID="lblpickPincode" runat="server">  </asp:Label>
                                              &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
                                              <asp:Label ID="lblpickCity" runat="server"></asp:Label>
                                              &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
                                              <asp:Label ID="lblpickState" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            </td>
                                        <td>
                                              <asp:Label ID="lblDropPincode" runat="server"></asp:Label> 
                                              &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
                                              <asp:Label ID="lblDropCity" runat="server"> </asp:Label> 
                                              &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;
                                              <asp:Label ID="lblDropState" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                <%--<tr>
                                    <td>No Of Packages</td>
                                    <td>
                                        
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>--%>
                            </table>
                        </fieldset>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="TabPanelVehicles" runat="server" TabIndex="1" HeaderText="Vehicle">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Vehicle Rate Detail</legend>
                            <div style="width: 1350px; overflow-x: scroll">
                                <asp:Label ID="lblResult" runat="server"></asp:Label>
                                <asp:GridView ID="GridViewVehicle" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                                    DataSourceID="DataSourceRate" CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="false" OnRowCommand="GridViewVehicle_RowCommand"
                                    PageSize="20" FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:BoundField DataField="TransporterName" HeaderText="Transporter" SortExpression="TransporterName" ReadOnly="true" />--%>
                                         <asp:TemplateField HeaderText="TransporterName" ItemStyle-Width="80px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkSelect" CommandName="select" runat="server" Text='<%# Bind("TransporterName")%>' CommandArgument='<%#Eval("lid") + ";" + Eval("MemoAttachment") + ";" + Eval("JobRefNo") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No" SortExpression="VehicleNo" ReadOnly="true" />
                                        <asp:BoundField DataField="VehicleTypeName" HeaderText="Vehicle Type" SortExpression="VehicleTypeName" ReadOnly="true" />
                                        <asp:TemplateField HeaderText="Memo" ItemStyle-Width="80px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDownload" CommandName="Download" runat="server" Text='<%# Bind("MemoAttachment")%>' CommandArgument='<%#Eval("MemoAttachment") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="LRNo" HeaderText="LRNo" SortExpression="LRNo" ReadOnly="true" />
                                        <asp:BoundField DataField="LRDate" HeaderText="LRDate" DataFormatString="{0:dd/MM/yyyy}" SortExpression="LRDate" ReadOnly="true" />
                                        <asp:BoundField DataField="ChallanNo" HeaderText="ChallanNo" SortExpression="ChallanNo" ReadOnly="true" />
                                        <asp:BoundField DataField="ChallanDate" HeaderText="Challan Date" SortExpression="ChallanDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="Rate" HeaderText="Freight Rate" SortExpression="Rate" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="Advance" HeaderText="Advance (%)" SortExpression="Advance" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="AdvanceAmount" HeaderText="AdvanceAmount" SortExpression="AdvanceAmount" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="MarketBillingRate" HeaderText="Market Billing Rate" SortExpression="MarketBillingRate" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="FreightAmount" HeaderText="Freight Amt" SortExpression="FreightAmount" ReadOnly="true" Visible="false" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="DetentionAmount" HeaderText="Detention Amt" SortExpression="DetentionAmount" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="VaraiExpense" HeaderText="Varai Exp" SortExpression="VaraiExpense" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="EmptyContRecptCharges" HeaderText="Empty Cont Charges" SortExpression="EmptyContRecptCharges" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="TollCharges" HeaderText="Toll Charges" SortExpression="TollCharges" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="OtherCharges" HeaderText="Other Charges" SortExpression="OtherCharges" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="CFSDispatchDate" HeaderText="Dispatch Date" SortExpression="OtherCharges" ItemStyle-HorizontalAlign="Right" />
                                        <%--<asp:BoundField DataField="Instruction" HeaderText="Billing Intruction" SortExpression="Instruction" ReadOnly="true" />--%>
                                        <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy" ReadOnly="true" />
                                        <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" SortExpression="CreatedDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="UpdatedBy" HeaderText="Updated By" SortExpression="UpdatedBy" ReadOnly="true" />
                                        <asp:BoundField DataField="UpdatedDate" HeaderText="Updated Date" SortExpression="UpdatedDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                                        
                                    </Columns>
                                </asp:GridView>
                            </div>

                            <div style="overflow: scroll;">
                            <fieldset>
                                <legend>Consolidate Job Detail</legend>
                                <asp:HiddenField ID="hdnConsolidateId" runat="server" Value="0" />
                                <asp:GridView ID="gvTransportJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table" DataKeyNames="TransReqId" PagerStyle-CssClass="pgr"
                                    AllowPaging="True" AllowSorting="True" Width="100%" PageSize="20" PagerSettings-Position="TopAndBottom" DataSourceID="DataSourceTransportJobDetail">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex +1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="TRRefNo" HeaderText="TR Ref No" />
                                        <asp:BoundField DataField="JobRefNo" HeaderText="Job Ref No" />
                                        <asp:BoundField DataField="CustName" HeaderText="Customer" />
                                        <asp:BoundField DataField="VehiclePlaceDate" HeaderText="Vehicle Place Date" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="LocationFrom" HeaderText="From" />
                                        <asp:BoundField DataField="Destination" HeaderText="Destination" />
                                        <asp:BoundField DataField="NoOfPkgs" HeaderText="No Of Pkgs" />
                                        <asp:BoundField DataField="GrossWeight" HeaderText="Gross Weight" />
                                        <asp:BoundField DataField="Count20" HeaderText="Cont 20" />
                                        <asp:BoundField DataField="Count40" HeaderText="Cont 40" />
                                        <asp:BoundField DataField="DeliveryType" HeaderText="Delivery Type" />
                                        <asp:BoundField DataField="PlanningDate" HeaderText="Planning Date" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="LRNo" HeaderText="LR No" />
                                        <asp:BoundField DataField="LRDate" HeaderText="LR Date" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="BabajiChallanNo" HeaderText="Challan No" />
                                        <asp:BoundField DataField="BabajiChallanDate" HeaderText="Challan Date" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="RequestedBy" HeaderText="Requested By" />
                                        <asp:BoundField DataField="RequestedDate" HeaderText="Request Date" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="UnloadingDate" HeaderText="Unloading Date" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="ReportingDate" HeaderText="Reporting Date" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="ContReturnDate" HeaderText="Cont Return Date" DataFormatString="{0:dd/MM/yyyy}" />
                                    </Columns>
                                </asp:GridView>

                                 <asp:SqlDataSource ID="DataSourceTransportJobDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="TR_GetConsolidateJobDetail" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <%--<asp:ControlParameter ControlID="hdnConsolidateId" PropertyName="Value" Name="ConsolidateID" />--%>
                                    <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
                                </SelectParameters>
                            </asp:SqlDataSource>

                                 <%-- START  : Pop-up For memo Upload --%>
                            <div>
                                <asp:HiddenField ID="hdnMemoUpload" runat="server" Value="0" />
                                <cc1:ModalPopupExtender ID="mpeMemoUpload" runat="server" TargetControlID="hdnMemoUpload" BackgroundCssClass="modalBackground" CancelControlID="imgClosePopup"
                                    PopupControlID="pnlMemo" DropShadow="true">
                                </cc1:ModalPopupExtender>
                                <asp:Panel ID="pnlMemo" runat="server" CssClass="modalPopup1" BackColor="#F5F5DC" Style="border-radius: 5px" Width="310px" Height="150px" BorderStyle="Solid" BorderWidth="2px">
                                    <div id="div2" runat="server">
                                        <table width="100%" style="border-bottom: 1px solid black">
                                            <tr>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td align="center"><b><u>Memo Upload</u></b>
                                                    <span style="float: right">
                                                        <asp:ImageButton ID="ImageButton1" ImageUrl="~/Images/delete.gif" runat="server" OnClick="imgClosePopup_Click" ToolTip="Close" />
                                                    </span>
                                                </td>
                                            </tr>
                                        </table>
                                        <table border="0" width="100%">
                                            <tr>
                                                <td></td>
                                            </tr>
                                            <br />
                                            <tr>
                                                <td>
                                                Job ref No
                                            </td>
                                            <td>
                                                <asp:Label ID="lblJobRefNo" runat="server"></asp:Label>
                                            </td>
                                            </tr>
                                            
                                            <tr>
                                                <td>Memo </td>
                                                <td><asp:FileUpload ID="fuMemo" runat="server" /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Button ID="btnMemoSubmit" runat="server" OnClick="btnMemoSubmit_Click" Text="Submit" />
                                                </td>
                                            </tr>                

                                        </table>
                                       
                                    </div>
                                </asp:Panel>
                            </div>
                            <%-- END    : Pop-up For memo Upload --%>
                            </fieldset>
                        </div>
                        </fieldset>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="TabPanelDelivery" runat="server" TabIndex="4" HeaderText="Delivery">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Delivery Detail</legend>
                            <div style="width: 97%; overflow-x: scroll">
                                
                                <asp:Label ID="lblDeliveryMsg" runat="server"></asp:Label>
                                
                                <asp:GridView ID="GridViewDelivery" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    Width="1350px" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="DeliveryId"
                                    DataSourceID="DataSourceDelivery" OnRowCommand="GridViewDelivery_RowCommand" OnRowDataBound="GridViewDelivery_RowDataBound"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PagerSettings-Position="TopAndBottom" PageSize="40">
                                    <Columns>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEdit" CommandName="Edit" ToolTip="Edit" Width="22" runat="server"
                                                    Text="Edit" Font-Underline="true"></asp:LinkButton>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update" Width="45" runat="server"
                                                    Text="Update" Font-Underline="true" ValidationGroup="GridDeliveryRequired">
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel" Width="39" CausesValidation="false"
                                                    runat="server" Text="Cancel" Font-Underline="true"></asp:LinkButton>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Container No" DataField="ContainerNo" ReadOnly="true" />
                                        <asp:TemplateField HeaderText="Pkgs">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeliveredPackages" runat="server" Text='<%# Eval("NoOfPackages")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtDeliveredPackages" runat="server" MaxLength="8" Width="50px" Text='<%# Bind("NoOfPackages")%>'></asp:TextBox>
                                                <asp:CompareValidator ID="CompValDeliveredPackages" runat="server" ControlToValidate="txtDeliveredPackages" Operator="DataTypeCheck" SetFocusOnError="true"
                                                    Type="Integer" Text="Invalid Packages" ErrorMessage="Invalid No Of Packages" Display="Dynamic" ValidationGroup="GridDeliveryRequired"></asp:CompareValidator>
                                                <asp:RequiredFieldValidator ID="RFVNoOfPkgs" runat="server" ControlToValidate="txtDeliveredPackages" SetFocusOnError="true"
                                                    Text="*" ErrorMessage="Please Enter No of Packages" Display="Dynamic" ValidationGroup="Required"></asp:RequiredFieldValidator>
                                                <%--<asp:HiddenField ID="hdnBalancePkg" runat="server" Value="0" />--%>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Vehicle No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVehicleNo" runat="server" Text='<%#Eval("VehicleNo") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtVehicleNo" runat="server" TabIndex="2" MaxLength="10" Width="160px" Text='<%#Eval("VehicleNo") %>'></asp:TextBox>
                                                <asp:DropDownList ID="ddVehicleNo" runat="server"></asp:DropDownList>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVehicleType" runat="server" Text='<%#Eval("vehiclename") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddVehicleType1" runat="server" DataSourceID="GetVehicleMSSqlDataSource"
                                                    DataTextField="sName" DataValueField="lid" SelectedValue='<%#Eval("vehicleId")%>' AppendDataBoundItems="true">
                                                    <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="reqVehicleType1" runat="server" ErrorMessage="Please Select Vehicle Type." SetFocusOnError="true"
                                                    InitialValue="0" Text="*" Display="Dynamic" ValidationGroup="GridDeliveryRequired" ControlToValidate="ddVehicleType1"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Received Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVehicleRcvdDate" runat="server" Text='<%#Eval("VehicleRcvdDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtVehicleRcvdDate" runat="server" Width="100px" Text='<%# Eval("VehicleRcvdDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                                <asp:Image ID="imgVehicleRcvdDate" ImageAlign="top" ImageUrl="../Images/btn_calendar.gif"
                                                    runat="server" />
                                                <cc1:CalendarExtender ID="calVehicleRcvdDate" runat="server" Enabled="true"
                                                    EnableViewState="false" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgVehicleRcvdDate"
                                                    PopupPosition="BottomRight" TargetControlID="txtVehicleRcvdDate">
                                                </cc1:CalendarExtender>
                                                <asp:CompareValidator ID="ComValVehicleRcvdDate" runat="server" ControlToValidate="txtVehicleRcvdDate"
                                                    Display="Dynamic" Text="*" ErrorMessage="Invalid Vehicle Received Date." Type="Date" SetFocusOnError="false"
                                                    CultureInvariantValues="false" Operator="DataTypeCheck" ValidationGroup="GridDeliveryRequired"></asp:CompareValidator>
                                                <asp:RequiredFieldValidator ID="Rfv_VehicleRcvdDate1" runat="server" ControlToValidate="txtVehicleRcvdDate" ErrorMessage="Please Enter Vehicle Received Date"
                                                    ValidationGroup="GridDeliveryRequired" SetFocusOnError="true" Text="*"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Transporter">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTransporter" runat="server" Text='<%#Eval("TransporterName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddTransporter" runat="server" TabIndex="6">
                                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="LR No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLRNo" runat="server" Text='<%#Eval("LRNo") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtLRNo" runat="server" Width="100px" Text='<%# Eval("LRNo")%>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="LR Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLRDate" runat="server" Text='<%#Eval("LRDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtLRDate" runat="server" Width="100px" Text='<%# Eval("LRDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                                <cc1:CalendarExtender ID="calLRDate" runat="server" Enabled="true" EnableViewState="false"
                                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgLRDate"
                                                    PopupPosition="BottomRight" TargetControlID="txtLRDate">
                                                </cc1:CalendarExtender>
                                                <asp:Image ID="imgLRDate" ImageAlign="top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                                <asp:CompareValidator ID="ComValLRDate" runat="server" ControlToValidate="txtLRDate"
                                                    Display="Dynamic" Text="*" ErrorMessage="Invalid LR Date." Type="Date" CultureInvariantValues="false"
                                                    Operator="DataTypeCheck" ValidationGroup="GridDeliveryRequired"></asp:CompareValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Destination">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeliveryPoint" runat="server" Text='<%#Eval("DeliveryPoint") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtDeliveryPoint" runat="server" Width="100px" Text='<%#Eval("DeliveryPoint")%>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Dispatch Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDispatchDate" runat="server" Text='<%#Eval("DispatchDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtDispatchDate" runat="server" Width="100px" Text='<%# Eval("DispatchDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="true"
                                                    EnableViewState="false" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgDispatchDate"
                                                    PopupPosition="BottomRight" TargetControlID="txtDispatchDate">
                                                </cc1:CalendarExtender>
                                                <asp:Image ID="imgDispatchDate" ImageAlign="top" ImageUrl="../Images/btn_calendar.gif"
                                                    runat="server" />
                                                <asp:CompareValidator ID="ComValDispatchDate" runat="server" ControlToValidate="txtDispatchDate"
                                                    Display="Dynamic" Text="*" ErrorMessage="Invalid Dispatch Date." Type="Date" SetFocusOnError="true"
                                                    CultureInvariantValues="false" Operator="DataTypeCheck" ValidationGroup="GridDeliveryRequired"></asp:CompareValidator>
                                                <asp:RequiredFieldValidator ID="reqvDispatchDate1" runat="server" Display="Dynamic"
                                                    Text="*" ErrorMessage="Please Enter Dispatch Date." ValidationGroup="GridDeliveryRequired"
                                                    ControlToValidate="txtDispatchDate"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cargo Delivered Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeliveryDate" runat="server" Text='<%# Eval("DeliveryDate","{0:dd/MM/yyyy}")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtDeliveryDate" runat="server" Width="100px" Text='<%# Eval("DeliveryDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                                <cc1:CalendarExtender ID="calBOEDate" runat="server" Enabled="True" EnableViewState="False"
                                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgDeliveryDate" PopupPosition="BottomRight"
                                                    TargetControlID="txtDeliveryDate">
                                                </cc1:CalendarExtender>
                                                <asp:Image ID="imgDeliveryDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif"
                                                    runat="server" />
                                                <asp:RequiredFieldValidator ID="reqvDeliveryDate" runat="server" ErrorMessage="Please Enter Delivery Date."
                                                    Text="*" Display="Dynamic" ValidationGroup="GridDelivery" ControlToValidate="txtDeliveryDate"></asp:RequiredFieldValidator>
                                                <asp:CompareValidator ID="ComValDeliveryDate" runat="server" ControlToValidate="txtDeliveryDate"
                                                    Display="Dynamic" ErrorMessage="Invalid Delivery Date." Type="Date" Text="*" CultureInvariantValues="false"
                                                    Operator="DataTypeCheck" ValidationGroup="GridDeliveryRequired"></asp:CompareValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Empty Container Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmptyContainerDate" runat="server" Text='<%# Eval("EmptyContRetrunDate","{0:dd/MM/yyyy}")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtEmptyContainerDate" runat="server" Width="100px" Text='<%# Eval("EmptyContRetrunDate","{0:dd/MM/yyyy}") %>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                                <cc1:CalendarExtender ID="calEmptyContainerDate" runat="server" Enabled="true"
                                                    EnableViewState="false" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgtxtEmptyContainerDate"
                                                    PopupPosition="BottomRight" TargetControlID="txtEmptyContainerDate">
                                                </cc1:CalendarExtender>
                                                <asp:Image ID="imgtxtEmptyContainerDate" ImageAlign="top" ImageUrl="../Images/btn_calendar.gif"
                                                    runat="server" />
                                                <asp:CompareValidator ID="ComValEmptyDate" runat="server" ControlToValidate="txtEmptyContainerDate"
                                                    Display="Dynamic" ErrorMessage="Invalid Empty Container Date." Type="Date" Text="*" CultureInvariantValues="false"
                                                    Operator="DataTypeCheck" ValidationGroup="GridDeliveryRequired"></asp:CompareValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="LR Attachment">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton1" runat="server" Text='<%#Eval("PODAttachmentPath") %>'
                                                    CommandName="Download" CommandArgument='<%#Eval("PODDownload") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:FileUpload ID="fuPODAttchment" runat="server" />
                                                <asp:HiddenField ID="hdnPODPath" runat="server" Value='<%#Eval("PODDownload") %>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Babaji Challan Copy">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkChallanDownload" runat="server" Text='<%#Eval("BabajiChallanCopyFile") %>'
                                                    CommandName="Download" CommandArgument='<%#Eval("BabajiChallanCopyPath") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:FileUpload ID="fuBCCAttchment" runat="server" />
                                                <asp:HiddenField ID="hdnBCCPath" runat="server" Value='<%#Eval("BabajiChallanCopyPath") %>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Damage Image">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDamageDownload" runat="server" Text='<%#Eval("DamagedImageFile") %>'
                                                    CommandName="Download" CommandArgument='<%#Eval("DamagedImagePath") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cargo Recvd Person Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCargoRecvdby" runat="server" Text='<%#Eval("CargoReceivedBy") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtCargoRecvdby" runat="server" Width="100px" Text='<%# Eval("CargoReceivedBy")%>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Challan No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblChallanNo" runat="server" Text='<%#Eval("BabajiChallanNo") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtChallanNo" runat="server" Text='<%#Eval("BabajiChallanNo")%>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="reqvChallanNo" runat="server" Display="Dynamic"
                                                    Text="*" ErrorMessage="Please Enter Babaji Challan No" ValidationGroup="GridDeliveryRequired"
                                                    ControlToValidate="txtChallanNo"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Challan Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblChalanDate" runat="server" Text='<%#Eval("BabajiChallanDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtchallanDate" runat="server" Width="100px" Text='<%#Eval("BabajiChallanDate","{0:dd/MM/yyyy}") %>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                                <cc1:CalendarExtender ID="calchanlandate" runat="server" Enabled="true" EnableViewState="false" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txtchallanDate" PopupButtonID="imgChallandate"></cc1:CalendarExtender>
                                                <asp:Image ID="imgChallandate" runat="server" ImageUrl="~/Images/btn_calendar.gif" />
                                                <asp:CompareValidator ID="ComValChallanno" runat="server" ControlToValidate="txtchallanDate"
                                                    Display="Dynamic" ErrorMessage="Invalid Challan Date." Type="Date" Text="*" CultureInvariantValues="false"
                                                    Operator="DataTypeCheck" ValidationGroup="GridDeliveryRequired"></asp:CompareValidator>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Driver Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lbldrivername" runat="server" Text='<%#Eval("DriverName")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtdrivername" runat="server" Text='<%#Eval("DriverName")%>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Driver Phone no.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbldriverphoneno" runat="server" Text='<%#Eval("DriverPhone")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtdriverphoneno" runat="server" Text='<%#Eval("DriverPhone")%>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="User Name" DataField="UserName" ReadOnly="true" />
                                        <asp:BoundField HeaderText="Date" DataField="DeliveryCreatedDate" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" ReadOnly="true" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton2" runat="server" Text="Delete" ToolTip="Delete" CommandName="Delete"
                                                    OnClientClick="return confirm('Are you sure wants to Delete ?');"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>

                            <fieldset class="fieldset-AutoWidth">
                                        <legend>Delivery To Warehouse</legend>
                                        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" CssClass="table"
                                            Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                                            DataSourceID="DataSourceWarehouse" OnRowDataBound="GridViewWarehouse_RowDataBound"
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
                                                <asp:TemplateField HeaderText="Cargo Delivered Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDeliveryDate" runat="server" Text='<%# Eval("DeliveryDate","{0:dd/MM/yyyy}")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtDeliveryDate" runat="server" Width="100px" Text='<%# Bind("DeliveryDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="calBOEDate" runat="server" Enabled="True" EnableViewState="False"
                                                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgDeliveryDate" PopupPosition="BottomRight"
                                                            TargetControlID="txtDeliveryDate">
                                                        </cc1:CalendarExtender>
                                                        <asp:Image ID="imgDeliveryDate" ImageAlign="Top" ImageUrl="Images/btn_calendar.gif"
                                                            runat="server" />
                                                        <asp:RequiredFieldValidator ID="reqvDeliveryDate" runat="server" ErrorMessage="Please Enter Delivery Date."
                                                            Text="*" Display="Dynamic" ValidationGroup="GridDelivery" ControlToValidate="txtDeliveryDate"></asp:RequiredFieldValidator>
                                                        <asp:CompareValidator ID="ComValDeliveryDate" runat="server" ControlToValidate="txtDeliveryDate"
                                                            Display="Dynamic" ErrorMessage="Invalid Delivery Date." Type="Date" Text="*" CultureInvariantValues="false"
                                                            Operator="DataTypeCheck" ValidationGroup="GridDelivery"></asp:CompareValidator>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Empty Container Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmptyContainerDate" runat="server" Text='<%# Eval("EmptyContRetrunDate","{0:dd/MM/yyyy}")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtEmptyContainerDate" runat="server" Width="100px" Text='<%# Bind("EmptyContRetrunDate","{0:dd/MM/yyyy}") %>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="calEmptyContainerDate" runat="server" Enabled="true"
                                                            EnableViewState="false" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgtxtEmptyContainerDate"
                                                            PopupPosition="BottomRight" TargetControlID="txtEmptyContainerDate">
                                                        </cc1:CalendarExtender>
                                                        <asp:Image ID="imgtxtEmptyContainerDate" ImageAlign="top" ImageUrl="Images/btn_calendar.gif"
                                                            runat="server" />
                                                        <asp:CompareValidator ID="ComValEmptyDate" runat="server" ControlToValidate="txtEmptyContainerDate"
                                                            Display="Dynamic" ErrorMessage="Invalid Empty Container Date." Type="Date" Text="*" CultureInvariantValues="false"
                                                            Operator="DataTypeCheck" ValidationGroup="GridDelivery"></asp:CompareValidator>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="LR Attachment">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDownload" runat="server" Text='<%#Eval("PODAttachmentPath") %>'
                                                            CommandName="Download" CommandArgument='<%#Eval("PODDownload") %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:FileUpload ID="fuPODAttchment" runat="server" />
                                                        <asp:HiddenField ID="hdnPODPath" runat="server" Value='<%#Eval("PODDownload") %>' />
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Babaji Challan Copy">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkChallanDownload" runat="server" Text='<%#Eval("BabajiChallanCopyFile") %>'
                                                            CommandName="Download" CommandArgument='<%#Eval("BabajiChallanCopyPath") %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Damage Copy">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDamageDownload" runat="server" Text='<%#Eval("DamagedImageFile") %>'
                                                            CommandName="Download" CommandArgument='<%#Eval("DamagedImagePath") %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cargo Recvd Person Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCargoRecvdby" runat="server" Text='<%#Eval("CargoReceivedBy") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtCargoRecvdby" runat="server" Width="100px" Text='<%# Bind("CargoReceivedBy")%>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="N Form No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNFormNo" runat="server" Text='<%#Eval("NFormNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtNFormNo" runat="server" Width="100px" Text='<%# Bind("NFormNo")%>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="N Form Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNFormDate" runat="server" Text='<%#Eval("NFormDate","{0:dd/MM/yyyy}") %>' placeholder="dd/mm/yyyy"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtNFormDate" runat="server" Width="100px" Text='<%# Bind("NFormDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="calNFormDate" runat="server" Enabled="true" EnableViewState="false"
                                                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgNFormDate" PopupPosition="BottomRight"
                                                            TargetControlID="txtNFormDate">
                                                        </cc1:CalendarExtender>
                                                        <asp:Image ID="imgNFormDate" ImageAlign="top" ImageUrl="Images/btn_calendar.gif"
                                                            runat="server" />
                                                        <asp:CompareValidator ID="ComValNFormDate" runat="server" ControlToValidate="txtNFormDate"
                                                            Display="Dynamic" ErrorMessage="Invalid N Form Date." Type="Date" Text="*" CultureInvariantValues="false"
                                                            Operator="DataTypeCheck" ValidationGroup="GridDelivery"></asp:CompareValidator>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="N Closing Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNClosingDate" runat="server" Text='<%#Eval("NClosingDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtNClosingDate" runat="server" Width="100px" Text='<%# Bind("NClosingDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="calNClosingDate" runat="server" Enabled="true"
                                                            EnableViewState="false" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgNClosingDate"
                                                            PopupPosition="BottomRight" TargetControlID="txtNClosingDate">
                                                        </cc1:CalendarExtender>
                                                        <asp:Image ID="imgNClosingDate" ImageAlign="top" ImageUrl="Images/btn_calendar.gif"
                                                            runat="server" />
                                                        <asp:CompareValidator ID="ComValNClosingDate" runat="server" ControlToValidate="txtNClosingDate"
                                                            Display="Dynamic" ErrorMessage="Invalid N Closing Date." Type="Date" Text="*" CultureInvariantValues="false"
                                                            Operator="DataTypeCheck" ValidationGroup="GridDelivery"></asp:CompareValidator>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="S Form No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSFormNo" runat="server" Text='<%#Eval("SFormNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtSFormNo" runat="server" Width="100px" Text='<%# Bind("SFormNo")%>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="S Form Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSFormDate" runat="server" Text='<%#Eval("SFormDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtSFormDate" runat="server" Width="100px" Text='<%# Bind("SFormDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="calSFormDate" runat="server" Enabled="true" EnableViewState="false"
                                                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgSFormDate" PopupPosition="BottomRight"
                                                            TargetControlID="txtSFormDate">
                                                        </cc1:CalendarExtender>
                                                        <asp:Image ID="imgSFormDate" ImageAlign="top" ImageUrl="Images/btn_calendar.gif"
                                                            runat="server" />
                                                        <asp:CompareValidator ID="ComValSFormDate" runat="server" ControlToValidate="txtSFormDate"
                                                            Display="Dynamic" ErrorMessage="Invalid S Form Date." Type="Date" Text="*" CultureInvariantValues="false"
                                                            Operator="DataTypeCheck" ValidationGroup="GridDelivery"></asp:CompareValidator>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="S Form Closing Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSClosingDate" runat="server" Text='<%#Eval("SClosingDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtSClosingDate" runat="server" Width="100px" Text='<%# Bind("SClosingDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="calSClosingDate" runat="server" Enabled="true"
                                                            EnableViewState="false" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgSClosingDate"
                                                            PopupPosition="BottomRight" TargetControlID="txtSClosingDate">
                                                        </cc1:CalendarExtender>
                                                        <asp:Image ID="imgSClosingDate" ImageAlign="top" ImageUrl="Images/btn_calendar.gif"
                                                            runat="server" />
                                                        <asp:CompareValidator ID="ComValSClosingDate" runat="server" ControlToValidate="txtSClosingDate"
                                                            Display="Dynamic" ErrorMessage="Invalid S Closing Date." Type="Date" Text="*" CultureInvariantValues="false"
                                                            Operator="DataTypeCheck" ValidationGroup="GridDelivery"></asp:CompareValidator>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Octroi Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOctroiAmount" runat="server" Text='<%#Eval("OctroiAmount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtOctroiAmount" runat="server" Width="100px" Text='<%# Bind("OctroiAmount")%>'></asp:TextBox>
                                                        <asp:CompareValidator ID="CompValOctroiAmount" runat="server" ControlToValidate="txtOctroiAmount"
                                                            Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" ErrorMessage="Invalid Octroi Amount."
                                                            Display="Dynamic" ValidationGroup="GridDelivery"></asp:CompareValidator>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Octroi Receipt No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOctroiReceiptNo" runat="server" Text='<%#Eval("OctroiReceiptNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtOctroiReceiptNo" runat="server" Width="100px" Text='<%# Bind("OctroiReceiptNo")%>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Octroi Paid Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOctroiPaidDate" runat="server" Text='<%#Eval("OctroiPaidDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtOctroiPaidDate" runat="server" Width="100px" Text='<%# Bind("OctroiPaidDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="calOctroiPaidDate" runat="server" Enabled="true"
                                                            EnableViewState="false" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgOctroiPaidDate"
                                                            PopupPosition="BottomRight" TargetControlID="txtOctroiPaidDate">
                                                        </cc1:CalendarExtender>
                                                        <asp:Image ID="imgOctroiPaidDate" ImageAlign="top" ImageUrl="Images/btn_calendar.gif"
                                                            runat="server" />
                                                        <asp:CompareValidator ID="ComValOctroiPaidDate" runat="server" ControlToValidate="txtOctroiPaidDate"
                                                            Display="Dynamic" ErrorMessage="Invalid Octroi Paid Date." Type="Date" Text="*" CultureInvariantValues="false"
                                                            Operator="DataTypeCheck" ValidationGroup="GridDelivery"></asp:CompareValidator>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Road Permit No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRoadPermitNo" runat="server" Text='<%#Eval("RoadPermitNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtRoadPermitNo" runat="server" Width="100px" Text='<%# Bind("RoadPermitNo")%>' MaxLength="100"></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Road Permit Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRoadPermitDate" runat="server" Text='<%#Eval("RoadPermitDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtRoadPermitDate" runat="server" Width="100px" Text='<%# Bind("RoadPermitDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="calRoadPermitDate" runat="server" Enabled="true"
                                                            EnableViewState="false" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgRoadPermitDate"
                                                            PopupPosition="BottomRight" TargetControlID="txtRoadPermitDate">
                                                        </cc1:CalendarExtender>
                                                        <asp:Image ID="imgRoadPermitDate" ImageAlign="top" ImageUrl="Images/btn_calendar.gif"
                                                            runat="server" />
                                                        <asp:CompareValidator ID="ComValRoadPermitDate" runat="server" ControlToValidate="txtRoadPermitDate"
                                                            Display="Dynamic" ErrorMessage="Invalid Road Permit Date." Type="Date" Text="*" CultureInvariantValues="false"
                                                            Operator="DataTypeCheck" ValidationGroup="GridDelivery"></asp:CompareValidator>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Challan No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblChallanNo" runat="server" Text='<%#Eval("BabajiChallanNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Challan Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblChalanDate" runat="server" Text='<%#Eval("BabajiChallanDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="User Name" DataField="UserName" ReadOnly="true" />
                                            </Columns>
                                        </asp:GridView>
                                    </fieldset>

                             
                            <div style="overflow: scroll;">
                            <fieldset class="fieldset-AutoWidth">
        <legend>Customer Delivery Detail</legend>
        <div class="m clear">
            <asp:DropDownList ID="ddWarehouse" runat="server" TabIndex="2">
            </asp:DropDownList>
            <asp:Button ID="btnMoveToWarehouse" runat="server" Text="Move Delivery to Warehouse" OnClick="btnMoveToWarehouse_Click" />
        </div>
        
        <div class="clear"></div>
    <div style="overflow: scroll;">
    <fieldset class="fieldset-AutoWidth">
        <legend>Delivery To Warehouse</legend>
        <div class="m clear">
            <asp:TextBox ID="txtDeliveryDestinationUpd" runat="server" MaxLength="100" placeholder="Delivery Destination">
            </asp:TextBox>
            <asp:Button ID="btnMoveToCustomerPlace" runat="server" Text="Move Delivery to Customer Place" />
        </div>
        <asp:GridView ID="GridViewWarehouse" runat="server" AutoGenerateColumns="False" CssClass="table"
            Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
            DataSourceID="DataSourceWarehouse" OnRowCommand="GridViewWarehouse_RowCommand" OnRowDataBound="GridViewWarehouse_RowDataBound"
            CellPadding="4" AllowPaging="True" AllowSorting="True" PagerSettings-Position="TopAndBottom" PageSize="40">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkedit1" runat="server" CommandName="Edit" ToolTip="Edit" Width="22" Text="Edit" Font-Underline="true"></asp:LinkButton>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:LinkButton ID="lnkupdate1" runat="server" ToolTip="update" CommandName="Update" Text="Update" Width="45" Font-Underline="true" ValidationGroup="GridDeliveryRequired"></asp:LinkButton>
                        <asp:LinkButton ID="lnkcancel1" runat="server" ToolTip="cancel" CommandName="Cancel" Text="Cancel" Width="39" Font-Underline="true"></asp:LinkButton>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Container No" DataField="ContainerNo" ReadOnly="true" />
                <asp:TemplateField HeaderText="Packages">
                    <ItemTemplate>
                        <asp:Label ID="lblPackages" runat="server" Text='<%#Eval("NoOfPackages")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtPackages" runat="server" Text='<%#Eval("NoOfPackages")%>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Vehicle No">
                    <ItemTemplate>
                        <asp:Label ID="lblVehicleNo" runat="server" Text='<%#Eval("VehicleNo")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtVehicleNo" runat="server" Text='<%#Eval("VehicleNo")%>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="Rfv_VehicleNo" runat="server" ControlToValidate="txtVehicleNo" ErrorMessage="Please Enter Vehicle No" ValidationGroup="GridDeliveryRequired"
                            SetFocusOnError="true" Text="*"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Vehicle Type">
                    <ItemTemplate>
                        <asp:Label ID="lblVehicleType" runat="server" Text='<%#Eval("vehiclename") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddVehicleType" runat="server" DataSourceID="GetVehicleMSSqlDataSource"
                            DataTextField="sName" DataValueField="lid" SelectedValue='<%#Eval("vehicleId")%>' AppendDataBoundItems="true">
                            <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="reqVehicleType" runat="server" ErrorMessage="Please Select Vehicle Type." SetFocusOnError="true"
                            InitialValue="0" Text="*" Display="Dynamic" ValidationGroup="GridDeliveryRequired" ControlToValidate="ddVehicleType"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Vehicle Received Date">
                    <ItemTemplate>
                        <asp:Label ID="lblVehicleRcvdDate" runat="server" Text='<%#Eval("VehicleRcvdDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtVehicleRcvdDate" runat="server" Width="100px" Text='<%# Eval("VehicleRcvdDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                        <asp:Image ID="imgVehicleRcvdDate" ImageAlign="top" ImageUrl="Images/btn_calendar.gif"
                            runat="server" />
                        <asp:RequiredFieldValidator ID="Rfv_VehicleRcvdDate" runat="server" ControlToValidate="txtVehicleRcvdDate" ErrorMessage="Please Enter Vehicle Received Date"
                            ValidationGroup="GridDeliveryRequired" SetFocusOnError="true" Text="*"></asp:RequiredFieldValidator>
                        <cc1:CalendarExtender ID="calVehicleRcvdDate" runat="server" Enabled="true"
                            EnableViewState="false" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgVehicleRcvdDate"
                            PopupPosition="BottomRight" TargetControlID="txtVehicleRcvdDate">
                        </cc1:CalendarExtender>
                        <asp:CompareValidator ID="ComValVehicleRcvdDate" runat="server" ControlToValidate="txtVehicleRcvdDate"
                            Display="Dynamic" Text="*" ErrorMessage="Invalid Vehicle Received Date." Type="Date" SetFocusOnError="false"
                            CultureInvariantValues="false" Operator="DataTypeCheck" ValidationGroup="GridDeliveryRequired"></asp:CompareValidator>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Transporter Name">
                    <ItemTemplate>
                        <asp:Label ID="lblTransporter" runat="server" Text='<%#Eval("TransporterName")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtTransporter" runat="server" Text='<%#Eval("TransporterName")%>'> </asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="LR No">
                    <ItemTemplate>
                        <asp:Label ID="lblLRNo" runat="server" Text='<%#Eval("LRNo") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtLRNo" runat="server" Text='<%#Eval("LRNo")%>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="LR Date">
                    <ItemTemplate>
                        <asp:Label ID="lblLRDate" runat="server" Text='<%#Eval("LRdate","{0:dd/MM/yyyy}") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtLRDate" runat="server" Width="100px" Text='<%# Bind("LRdate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                        <cc1:CalendarExtender ID="calLRDate" runat="server" Enabled="true" EnableViewState="false"
                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgLRDate"
                            PopupPosition="BottomRight" TargetControlID="txtLRDate">
                        </cc1:CalendarExtender>
                        <asp:Image ID="imgLRDate" ImageAlign="top" ImageUrl="Images/btn_calendar.gif" runat="server" />
                        <asp:CompareValidator ID="ComValLRDate" runat="server" ControlToValidate="txtLRDate"
                            Display="Dynamic" Text="*" ErrorMessage="Invalid LR Date." Type="Date" CultureInvariantValues="false"
                            Operator="DataTypeCheck" ValidationGroup="GridDeliveryRequired"></asp:CompareValidator>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Delivery Point">
                    <ItemTemplate>
                        <asp:Label ID="lblDeliveryPoint" runat="server" Text='<%#Eval("DeliveryPoint") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtDeliveryPoint" runat="server" Width="100px" Text='<%# Eval("DeliveryPoint")%>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Dispatch Date">
                    <ItemTemplate>
                        <asp:Label ID="lblDispatchDate" runat="server" Text='<%#Eval("DispatchDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtDispatchDate" runat="server" Width="100px" Text='<%# Eval("DispatchDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                        <cc1:CalendarExtender ID="calDispatchDate" runat="server" Enabled="true"
                            EnableViewState="false" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgDispatchDate"
                            PopupPosition="BottomRight" TargetControlID="txtDispatchDate">
                        </cc1:CalendarExtender>
                        <asp:Image ID="imgDispatchDate" ImageAlign="top" ImageUrl="Images/btn_calendar.gif"
                            runat="server" />
                        <asp:CompareValidator ID="ComValDispatchDate" runat="server" ControlToValidate="txtDispatchDate"
                            Display="Dynamic" Text="*" ErrorMessage="Invalid Dispatch Date." Type="Date" SetFocusOnError="true"
                            CultureInvariantValues="false" Operator="DataTypeCheck" ValidationGroup="GridDeliveryRequired"></asp:CompareValidator>
                        <asp:RequiredFieldValidator ID="reqvDispatchDate" runat="server" Display="Dynamic"
                            Text="*" ErrorMessage="Please Enter Dispatch Date." ValidationGroup="GridDeliveryRequired"
                            ControlToValidate="txtDispatchDate"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Road Permit No">
                    <ItemTemplate>
                        <asp:Label ID="lblRoadPermitNo" runat="server" Text='<%#Eval("RoadPermitNo") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtRoadPermitNo" runat="server" Width="100px" Text='<%#Eval("RoadPermitNo")%>' MaxLength="100"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqvRoadPermitNo" runat="server" Display="Dynamic"
                            Text="*" ErrorMessage="Please Enter Road Permit No." ValidationGroup="GridDeliveryRequired"
                            ControlToValidate="txtRoadPermitNo"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Road Permit Date">
                    <ItemTemplate>
                        <asp:Label ID="lblRoadPermitDate" runat="server" Text='<%#Eval("RoadPermitDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtRoadPermitDate" runat="server" Width="100px" Text='<%# Eval("RoadPermitDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                        <cc1:CalendarExtender ID="calRoadPermitDate" runat="server" Enabled="true"
                            EnableViewState="false" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgRoadPermitDate"
                            PopupPosition="BottomRight" TargetControlID="txtRoadPermitDate">
                        </cc1:CalendarExtender>
                        <asp:Image ID="imgRoadPermitDate" ImageAlign="top" ImageUrl="Images/btn_calendar.gif"
                            runat="server" />
                        <asp:CompareValidator ID="ComValRoadPermitDate" runat="server" ControlToValidate="txtRoadPermitDate"
                            Display="Dynamic" ErrorMessage="Invalid Road Permit Date." Type="Date" Text="*" CultureInvariantValues="false"
                            Operator="DataTypeCheck" ValidationGroup="GridDeliveryRequired"></asp:CompareValidator>
                        <asp:RequiredFieldValidator ID="reqvRoadPermitDate" runat="server" Display="Dynamic"
                            Text="*" ErrorMessage="Please Enter Road Permit Date" ValidationGroup="GridDeliveryRequired"
                            ControlToValidate="txtRoadPermitDate"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                </asp:TemplateField>
                <%--                                        <asp:TemplateField HeaderText="Cargo Delivered Date">
                <ItemTemplate>
                    <asp:Label ID="lblDeliveryDate" runat="server" Text='<%# Eval("DeliveryDate","{0:dd/MM/yyyy}")%>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtDeliveryDate" runat="server" Width="100px" Text='<%# Eval("DeliveryDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy" ></asp:TextBox>
                    <AjaxToolkit:CalendarExtender ID="calBOEDate" runat="server" Enabled="True" EnableViewState="False"
                        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgDeliveryDate" PopupPosition="BottomRight"
                        TargetControlID="txtDeliveryDate">
                    </AjaxToolkit:CalendarExtender>
                    <asp:Image ID="imgDeliveryDate" ImageAlign="Top" ImageUrl="Images/btn_calendar.gif"
                        runat="server" />
                    <asp:RequiredFieldValidator ID="reqvDeliveryDate" runat="server" ErrorMessage="Please Enter Delivery Date."
                        Text="*" Display="Dynamic" ValidationGroup="GridDelivery" ControlToValidate="txtDeliveryDate"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="ComValDeliveryDate" runat="server" ControlToValidate="txtDeliveryDate"
                        Display="Dynamic" ErrorMessage="Invalid Delivery Date." Type="Date"  Text="*" CultureInvariantValues="false"
                        Operator="DataTypeCheck" ValidationGroup="GridDeliveryRequired"></asp:CompareValidator>
                </EditItemTemplate>
            </asp:TemplateField>
        <asp:TemplateField HeaderText="Empty Container Date">
                <ItemTemplate>
                    <asp:Label ID="lblEmptyContainerDate" runat="server" Text='<%# Eval("EmptyContRetrunDate","{0:dd/MM/yyyy}")%>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtEmptyContainerDate" runat="server" Width="100px" Text='<%# Eval("EmptyContRetrunDate","{0:dd/MM/yyyy}") %>' placeholder="dd/mm/yyyy" ></asp:TextBox>
                    <AjaxToolkit:CalendarExtender ID="calEmptyContainerDate" runat="server" Enabled="true"
                        EnableViewState="false" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgtxtEmptyContainerDate"
                        PopupPosition="BottomRight" TargetControlID="txtEmptyContainerDate">
                    </AjaxToolkit:CalendarExtender>
                    <asp:Image ID="imgtxtEmptyContainerDate" ImageAlign="top" ImageUrl="Images/btn_calendar.gif"
                        runat="server" />
                    <asp:CompareValidator ID="ComValEmptyDate" runat="server" ControlToValidate="txtEmptyContainerDate"
                        Display="Dynamic" ErrorMessage="Invalid Empty Container Date." Type="Date" Text="*" CultureInvariantValues="false"
                        Operator="DataTypeCheck" ValidationGroup="GridDeliveryRequired"></asp:CompareValidator>
                </EditItemTemplate>
            </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="POA Attachment Path">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkDownload" runat="server" Text='<%#Eval("PODAttachmentPath") %>'
                            CommandName="Download" CommandArgument='<%#Eval("PODDownload") %>'></asp:LinkButton>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:FileUpload ID="fuPODAttchment" runat="server" />
                        <asp:HiddenField ID="hdnPODPath" runat="server" Value='<%#Eval("PODDownload") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="N Form No">
                    <ItemTemplate>
                        <asp:Label ID="lblNFormNo" runat="server" Text='<%#Eval("NFormNo") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtNFormNo" runat="server" Text='<%# Eval("NFormNo")%>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqvNFormNo" runat="server" Display="Dynamic"
                            Text="*" ErrorMessage="Please Enter N Form No" ValidationGroup="GridDeliveryRequired"
                            ControlToValidate="txtNFormNo"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="N Form Date">
                    <ItemTemplate>
                        <asp:Label ID="lblNFormDate" runat="server" Text='<%#Eval("NFormDate","{0:dd/MM/yyyy}") %>' placeholder="dd/mm/yyyy"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtNFormDate" runat="server" Width="100px" Text='<%# Eval("NFormDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                        <cc1:CalendarExtender ID="calNFormDate" runat="server" Enabled="true" EnableViewState="false"
                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgNFormDate" PopupPosition="BottomRight"
                            TargetControlID="txtNFormDate">
                        </cc1:CalendarExtender>
                        <asp:Image ID="imgNFormDate" ImageAlign="top" ImageUrl="Images/btn_calendar.gif"
                            runat="server" />
                        <asp:CompareValidator ID="ComValNFormDate" runat="server" ControlToValidate="txtNFormDate"
                            Display="Dynamic" ErrorMessage="Invalid N Form Date." Type="Date" Text="*" CultureInvariantValues="false"
                            Operator="DataTypeCheck" ValidationGroup="GridDeliveryRequired"></asp:CompareValidator>

                        <asp:RequiredFieldValidator ID="reqvNFormDate" runat="server" Display="Dynamic"
                            Text="*" ErrorMessage="Please Enter N Form Date" ValidationGroup="GridDeliveryRequired"
                            ControlToValidate="txtNFormDate"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="N Closing Date">
                    <ItemTemplate>
                        <asp:Label ID="lblNClosingDate" runat="server" Text='<%#Eval("NClosingDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtNClosingDate" runat="server" Width="100px" Text='<%# Eval("NClosingDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                        <cc1:CalendarExtender ID="calNClosingDate" runat="server" Enabled="true"
                            EnableViewState="false" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgNClosingDate"
                            PopupPosition="BottomRight" TargetControlID="txtNClosingDate">
                        </cc1:CalendarExtender>
                        <asp:Image ID="imgNClosingDate" ImageAlign="top" ImageUrl="Images/btn_calendar.gif"
                            runat="server" />
                        <asp:CompareValidator ID="ComValNClosingDate" runat="server" ControlToValidate="txtNClosingDate"
                            Display="Dynamic" ErrorMessage="Invalid N Closing Date." Type="Date" Text="*" CultureInvariantValues="false"
                            Operator="DataTypeCheck" ValidationGroup="GridDeliveryRequired"></asp:CompareValidator>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="S Form No">
                    <ItemTemplate>
                        <asp:Label ID="lblSFormNo" runat="server" Text='<%#Eval("SFormNo") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtSFormNo" runat="server" Width="100px" Text='<%# Eval("SFormNo")%>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqvSFormNo" runat="server" Display="Dynamic"
                            Text="*" ErrorMessage="Please Enter S Form No" ValidationGroup="GridDeliveryRequired"
                            ControlToValidate="txtSFormNo"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="S Form Date">
                    <ItemTemplate>
                        <asp:Label ID="lblSFormDate" runat="server" Text='<%#Eval("SFormDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtSFormDate" runat="server" Width="100px" Text='<%# Bind("SFormDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                        <cc1:CalendarExtender ID="calSFormDate" runat="server" Enabled="true" EnableViewState="false"
                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgSFormDate" PopupPosition="BottomRight"
                            TargetControlID="txtSFormDate">
                        </cc1:CalendarExtender>
                        <asp:Image ID="imgSFormDate" ImageAlign="top" ImageUrl="Images/btn_calendar.gif"
                            runat="server" />
                        <asp:CompareValidator ID="ComValSFormDate" runat="server" ControlToValidate="txtSFormDate"
                            Display="Dynamic" ErrorMessage="Invalid S Form Date." Type="Date" Text="*" CultureInvariantValues="false"
                            Operator="DataTypeCheck" ValidationGroup="GridDeliveryRequired"></asp:CompareValidator>
                        <asp:RequiredFieldValidator ID="reqvSFormDate" runat="server" Display="Dynamic"
                            Text="*" ErrorMessage="Please Enter S Form Date" ValidationGroup="GridDeliveryRequired"
                            ControlToValidate="txtSFormDate"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="S Form Closing Date">
                    <ItemTemplate>
                        <asp:Label ID="lblSClosingDate" runat="server" Text='<%#Eval("SClosingDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtSClosingDate" runat="server" Width="100px" Text='<%# Eval("SClosingDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                        <cc1:CalendarExtender ID="calSClosingDate" runat="server" Enabled="true"
                            EnableViewState="false" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgSClosingDate"
                            PopupPosition="BottomRight" TargetControlID="txtSClosingDate">
                        </cc1:CalendarExtender>
                        <asp:Image ID="imgSClosingDate" ImageAlign="top" ImageUrl="Images/btn_calendar.gif"
                            runat="server" />
                        <asp:CompareValidator ID="ComValSClosingDate" runat="server" ControlToValidate="txtSClosingDate"
                            Display="Dynamic" ErrorMessage="Invalid S Closing Date." Type="Date" Text="*" CultureInvariantValues="false"
                            Operator="DataTypeCheck" ValidationGroup="GridDeliveryRequired"></asp:CompareValidator>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Octroi Amount">
                    <ItemTemplate>
                        <asp:Label ID="lblOctroiAmount" runat="server" Text='<%#Eval("OctroiAmount") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtOctroiAmount" runat="server" Width="100px" Text='<%# Eval("OctroiAmount")%>'></asp:TextBox>
                        <asp:CompareValidator ID="CompValOctroiAmount" runat="server" ControlToValidate="txtOctroiAmount"
                            Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" ErrorMessage="Invalid Octroi Amount."
                            Display="Dynamic" ValidationGroup="GridDeliveryRequired"></asp:CompareValidator>
                        <asp:RequiredFieldValidator ID="reqvOctroiAmount" runat="server" Display="Dynamic"
                            Text="*" ErrorMessage="Please Enter Octroi Amount" ValidationGroup="GridDeliveryRequired"
                            ControlToValidate="txtOctroiAmount"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Octroi Receipt No">
                    <ItemTemplate>
                        <asp:Label ID="lblOctroiReceiptNo" runat="server" Text='<%#Eval("OctroiReceiptNo") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtOctroiReceiptNo" runat="server" Width="100px" Text='<%# Eval("OctroiReceiptNo")%>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqvOctroiReceiptNo" runat="server" Display="Dynamic"
                            Text="*" ErrorMessage="Please Enter Octroi Receipt No" ValidationGroup="GridDeliveryRequired"
                            ControlToValidate="txtOctroiReceiptNo"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Octroi Paid Date">
                    <ItemTemplate>
                        <asp:Label ID="lblOctroiPaidDate" runat="server" Text='<%#Eval("OctroiPaidDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtOctroiPaidDate" runat="server" Width="100px" Text='<%# Eval("OctroiPaidDate","{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                        <cc1:CalendarExtender ID="calOctroiPaidDate" runat="server" Enabled="true"
                            EnableViewState="false" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgOctroiPaidDate"
                            PopupPosition="BottomRight" TargetControlID="txtOctroiPaidDate">
                        </cc1:CalendarExtender>
                        <asp:Image ID="imgOctroiPaidDate" ImageAlign="top" ImageUrl="Images/btn_calendar.gif"
                            runat="server" />
                        <asp:CompareValidator ID="ComValOctroiPaidDate" runat="server" ControlToValidate="txtOctroiPaidDate"
                            Display="Dynamic" ErrorMessage="Invalid Octroi Paid Date." Type="Date" Text="*" CultureInvariantValues="false"
                            Operator="DataTypeCheck" ValidationGroup="GridDeliveryRequired"></asp:CompareValidator>

                        <asp:RequiredFieldValidator ID="reqvOctroiPaidDat" runat="server" Display="Dynamic"
                            Text="*" ErrorMessage="Please Enter Octroi Paid Date" ValidationGroup="GridDeliveryRequired"
                            ControlToValidate="txtOctroiPaidDate"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Babaji Challan No">
                    <ItemTemplate>
                        <asp:Label ID="lblChallanNo" runat="server" Text='<%#Eval("BabajiChallanNo") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtChallanNo" runat="server" Text='<%#Eval("BabajiChallanNo")%>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqvChallanNo" runat="server" Display="Dynamic"
                            Text="*" ErrorMessage="Please Enter Babaji Challan No" ValidationGroup="GridDeliveryRequired"
                            ControlToValidate="txtChallanNo"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Babaji Challan Date">
                    <ItemTemplate>
                        <asp:Label ID="lblChalanDate" runat="server" Text='<%#Eval("BabajiChallanDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtChalanDate" runat="server" Text='<%#Eval("BabajiChallanDate","{0:dd/MM/yyyy}") %>'></asp:TextBox>
                        <cc1:CalendarExtender ID="calchallandate" runat="server" Enabled="true" EnableViewState="false"
                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgchallandt" TargetControlID="txtChalanDate">
                        </cc1:CalendarExtender>
                        <asp:Image ID="imgchallandt" runat="server" ImageUrl="~/Images/btn_calendar.gif" />
                        <asp:CompareValidator ID="comValchallandt" runat="server" ControlToValidate="txtChalanDate" Display="Dynamic"
                            Text="*" CultureInvariantValues="false" Operator="DataTypeCheck" ValidationGroup="GridDeliveryRequired" ErrorMessage="Invalid Babaji Challan Date." Type="Date"></asp:CompareValidator>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Babaji Challan Copy">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkChallanDownload_Warehouse" runat="server" Text='<%#Eval("BabajiChallanCopyFile") %>'
                            CommandName="Download" CommandArgument='<%#Eval("BabajiChallanCopyPath") %>'></asp:LinkButton>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:FileUpload ID="fuBCCAttchment_Warehouse" runat="server" />
                        <asp:HiddenField ID="hdnBCCPath_Warehouse" runat="server" Value='<%#Eval("BabajiChallanCopyPath") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Damage Image">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkDamageDownload" runat="server" Text='<%#Eval("DamagedImageFile") %>'
                            CommandName="Download" CommandArgument='<%#Eval("DamagedImagePath") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Driver Name">
                    <ItemTemplate>
                        <asp:Label ID="lbldrivername" runat="server" Text='<%#Eval("DriverName")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtdrivername" runat="server" Text='<%#Eval("DriverName")%>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Driver Phone no.">
                    <ItemTemplate>
                        <asp:Label ID="lbldriverphoneno" runat="server" Text='<%#Eval("DriverPhone")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtdriverphoneno" runat="server" Text='<%#Eval("DriverPhone")%>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="User Name" DataField="UserName" ReadOnly="true" />
                <asp:BoundField HeaderText="Date" DataField="DeliveryCreatedDate" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" ReadOnly="true" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkdelete1" runat="server" Text="Delete" ToolTip="Delete" CommandName="Delete1"
                            OnClientClick="return confirm('Are you sure wants to Delete ?');"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </fieldset>
</div>
    </fieldset>
</div>
                        </fieldset>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="TabPanelMovement" runat="server" TabIndex="2" HeaderText="Movement">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Movement History</legend>
                            <div>
                                <asp:GridView ID="gvMovementHistory" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    Width="100%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceMovementHistory"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No" SortExpression="VehicleNo" />
                                        <asp:BoundField DataField="VehicleType" HeaderText="Vehicle Type" SortExpression="VehicleType" Visible="false" />
                                        <asp:BoundField DataField="Transporter" HeaderText="Transporter" SortExpression="Transporter" />
                                        <asp:BoundField DataField="DeliveryFrom" HeaderText="From" SortExpression="DeliveryFrom" />
                                        <asp:BoundField DataField="DeliveryTo" HeaderText="Destination" SortExpression="DeliveryTo" />
                                        <asp:BoundField DataField="DeliveryType" HeaderText="Delivery Type" SortExpression="DeliveryType" />
                                        <asp:BoundField DataField="DispatchDate" HeaderText="Dispatch Date" SortExpression="DispatchDate" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="EmptyValidityDate" HeaderText="Empty Validity Date" SortExpression="EmptyValidityDate" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="ReportingDate" HeaderText="Reporting Date" SortExpression="ReportingDate" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="UnloadingDate" HeaderText="Unloading Date" SortExpression="UnloadingDate" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="ContReturnDate" HeaderText="Empty Cont Return Date" SortExpression="ContReturnDate" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy" />
                                        <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" SortExpression="CreatedDate" DataFormatString="{0:dd/MM/yyyy}" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>
                        <fieldset>
                            <legend>Vehicle Daily Status History</legend>
                            <div>
                                <asp:GridView ID="gvVehicleDailyStatus" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="TransReqId"
                                    DataSourceID="DataSourceDailyStatus" CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="false"
                                    PageSize="20" FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" OnRowCommand="gvVehicleDailyStatus_RowCommand" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Vehicle No" DataField="VehicleNo" />
                                        <asp:BoundField HeaderText="Vehicle Type" DataField="VehicleType" />
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkbtnDailyStatus" runat="server" Font-Underline="true" CommandName="DailyStatus" Text='<%#Eval("CurrentStatus") %>'
                                                    CommandArgument='<%#Eval("VehicleNo") + ";" + Eval("VehicleType") + ";" + Eval("DeliveryFrom") + ";" + Eval("DeliveryTo") + ";" + Eval("DispatchDate","{0:dd/MM/yyyy}") + ";" + Eval("CustomerMail") + ";" + Eval("TransReqId") + ";" + Eval("JobRefNo") + ";" + Eval("CustName") + ";" + Eval("CustRefNo") + ";" + Eval("CurrentStatus") + ";" + Eval("StatusCreatedBy")  + ";" + Eval("EmailTo") + ";" + Eval("EmailCC")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Current Status" DataField="CurrentStatus" Visible="false" />
                                        <asp:BoundField HeaderText="CreatedBy" DataField="StatusCreatedBy" />
                                        <asp:BoundField HeaderText="Created Date" DataField="StatusCreatedDate" DataFormatString="{0:dd/MM/yyyy HH:mm:tt}" />
                                    </Columns>
                                </asp:GridView>
                            </div>
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
                                                <div id="DivABC" runat="server" style="border: 1px solid black; margin: 5px; margin-top: 0px; border-radius: 4px; max-height: 620px; max-width: 780px;">
                                                    <div class="m" style="padding: 10px;">
                                                        <asp:Label ID="lblPopMessageEmail" runat="server" EnableViewState="false"></asp:Label>
                                                        <table border="0" width="100%">
                                                            <tr>
                                                                <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  <b>To : </b>
                                                                    <u>
                                                                        <asp:Label ID="lblCustomerEmail" runat="server" Font-Underline="true" Width="89%" Enabled="false" CssClass="cssStatus"></asp:Label></u>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b>CC :</b>
                                                                    <u>
                                                                        <asp:Label ID="txtMailCC" runat="server" Width="89%" Font-Underline="true" Enabled="false" CssClass="cssStatus"></asp:Label></u>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td><b>Subject :</b>
                                                                    <u>
                                                                        <asp:Label ID="txtSubject" runat="server" Width="89%" Enabled="false" Font-Underline="true" CssClass="cssStatus"></asp:Label></u>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <div id="divPreviewEmail" runat="server" style="padding: 10px; background-color: white; border-radius: 3px; margin-left: 10px; margin-right: 10px; margin-bottom: 20px; border: 1px solid black; border-style: ridge">
                                                    </div>
                                                    <div id="DivSendEmail" runat="server" style="text-align: right; margin-left: 350px">
                                                        <asp:Button ID="btnCancelEmailPp" runat="server" OnClick="btnCancelEmailPp_Click" Text="Cancel"></asp:Button>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                            <%-- END    : Pop-up For Daily Status --%>
                        </fieldset>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="TabPanelBillStatus" runat="server" TabIndex="3" HeaderText="Bill Detail">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Bill Detail</legend>
                            <div>
                                <asp:GridView ID="gvBillDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                                    DataSourceID="DataSourceBillDetail" CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="false"
                                    PageSize="20" FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                                    <Columns>
                                        <asp:BoundField HeaderText="Transporter" DataField="Transporter" Visible="false" />
                                        <asp:BoundField HeaderText="Bill Number" DataField="BillNumber" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField HeaderText="Bill Submit Date" DataField="BillSubmitDate" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField HeaderText="Bill Date" DataField="BillDate" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField HeaderText="Billing Person" DataField="BillPersonName" />
                                        <asp:BoundField HeaderText="Bill Amount" DataField="BillAmount" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField HeaderText="Detention" DataField="DetentionAmount" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField HeaderText="Varai" DataField="VaraiAmount" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField HeaderText="Empty Cont Charges" DataField="EmptyContRcptCharges" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField HeaderText="Total" DataField="TotalAmount" ItemStyle-HorizontalAlign="Right" ItemStyle-Font-Bold="true" />
                                        <asp:BoundField HeaderText="Justification" DataField="Justification" ItemStyle-Width="35%" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>
                        <fieldset>
                            <legend>Bill Approval History</legend>
                            <div>
                                <asp:GridView ID="gvBillApprovalHistory" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                                    DataSourceID="DataSourceBillApprovalHistory" CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="false"
                                    PageSize="20" FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="BillNumber" DataField="BillNumber" />
                                        <asp:BoundField HeaderText="Status" DataField="Status" />
                                        <asp:BoundField HeaderText="Remark" DataField="Remark" />
                                        <asp:BoundField HeaderText="Active/Inactive" DataField="ActiveStatus" Visible="false" />
                                        <asp:BoundField HeaderText="Created By" DataField="CreatedBy" />
                                        <asp:BoundField HeaderText="Created Date" DataField="CreatedDate" DataFormatString="{0:dd/MM/yyyy HH:mm:tt}" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>
                        <fieldset>
                            <legend>Vehicle Rate Detail</legend>
                            <div style="width: 1350px; overflow-x: auto">
                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                                    DataSourceID="DataSourceRate" CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="false"
                                    PageSize="20" FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="TRRefNo" HeaderText="Ref No" ReadOnly="true" Visible="false" />
                                        <asp:BoundField DataField="JobRefNo" HeaderText="Job No" ReadOnly="true" Visible="false" />
                                        <asp:BoundField DataField="TransitType" HeaderText="Delivery To" ReadOnly="true" />
                                        <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No" ReadOnly="true" />
                                        <asp:BoundField DataField="TransporterName" HeaderText="Transporter" ReadOnly="true" />
                                        <asp:BoundField DataField="MarketBillingRate" HeaderText="Market Rate" SortExpression="MarketBillingRate" ReadOnly="true" Visible="false" />
                                        <asp:BoundField DataField="Rate" HeaderText="Freight Rate" SortExpression="Rate" ReadOnly="true" />
                                        <asp:BoundField DataField="Advance" HeaderText="Advance" SortExpression="Advance" ReadOnly="true" Visible="false" />
                                        <asp:BoundField DataField="AdvanceAmount" HeaderText="Advance Amt" SortExpression="AdvanceAmount" ReadOnly="true" />
                                        <asp:BoundField DataField="DetentionAmount" HeaderText="Detention Amt" SortExpression="DetentionAmount" ReadOnly="true" />
                                        <asp:BoundField DataField="VaraiExpense" HeaderText="Varai Exp" SortExpression="VaraiExpense" ReadOnly="true" />
                                        <asp:BoundField DataField="EmptyContRecptCharges" HeaderText="Empty Cont Charges" SortExpression="EmptyContRecptCharges" ReadOnly="true" />
                                        <asp:BoundField DataField="TotalAmount" HeaderText="Total" SortExpression="TotalAmount" ReadOnly="true" />
                                        <asp:BoundField DataField="TollCharges" HeaderText="Toll Charges" SortExpression="TollCharges" ReadOnly="true" />
                                        <asp:BoundField DataField="OtherCharges" HeaderText="Other Charges" SortExpression="OtherCharges" ReadOnly="true" />
                                        <asp:BoundField DataField="VehicleTypeName" HeaderText="Type" ReadOnly="true" />
                                        <asp:BoundField DataField="LocationFrom" HeaderText="Delivery From" ReadOnly="true" />
                                        <asp:BoundField DataField="DeliveryPoint" HeaderText="Delivery Point" ReadOnly="true" />
                                        <asp:BoundField DataField="City" HeaderText="City" ReadOnly="true" />
                                        <asp:BoundField DataField="LRNo" HeaderText="LR No" SortExpression="LRNo" ReadOnly="true" />
                                        <asp:BoundField DataField="LRDate" HeaderText="LR Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="LRDate" ReadOnly="true" />
                                        <asp:BoundField DataField="ChallanNo" HeaderText="Challan No" SortExpression="ChallanNo" ReadOnly="true" />
                                        <asp:BoundField DataField="ChallanDate" HeaderText="Challan Date" SortExpression="ChallanDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="UnloadingDate" HeaderText="Unloading Date" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                                        <%--<asp:BoundField DataField="Instruction" HeaderText="Billing Intruction" SortExpression="Instruction" ReadOnly="true" />--%>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>
                            <fieldset>
                            <legend>Selling Rate Detail</legend>
                            <div>
                                <div style="width: 1350px;overflow-x: scroll">
                                    <asp:GridView ID="gvSellingDetail" runat="server" AutoGenerateColumns="False" CssClass="table" Width="90%" AlternatingRowStyle-CssClass="alt"
                                        PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceSellingRate" OnRowCommand="gvSellingDetail_RowCommand"
                                        OnPreRender="gvSellingDetail_PreRender" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sl">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No" ReadOnly="true" />
                                            <asp:BoundField DataField="TransporterName" HeaderText="Transporter" ReadOnly="true" />
                                            <asp:BoundField DataField="SellFreighRate" HeaderText="Selling Freight rate" ReadOnly="true" />
                                            <asp:BoundField DataField="SellDetentionAmount" HeaderText="Detention Amount" ReadOnly="true" />
                                            <asp:BoundField DataField="SellVaraiExpense" HeaderText="Varai Amount No" ReadOnly="true" />
                                            <asp:BoundField DataField="SellEmptyContRecptCharges" HeaderText="Empty Cont Amount" ReadOnly="true" />
                                            <asp:BoundField DataField="SellTollCharges" HeaderText="Toll Amount No" ReadOnly="true" />
                                            <asp:BoundField DataField="SellOtherCharges" HeaderText="Other Amount" ReadOnly="true" />
                                            <%--<asp:TemplateField HeaderText="Detention Doc">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnDetentionCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download detention copy."
                                                        CommandName="DetentionCopy" CommandArgument='<%#Eval("DetentionDoc")%>' Visible='<%# DecideHereImg((string)Eval("DetentionDoc")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Varai Doc">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnVaraiCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download varai copy."
                                                        CommandName="varaiCopy" CommandArgument='<%#Eval("VaraiDoc")%>' Visible='<%# DecideHereImg((string)Eval("VaraiDoc")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Empty Cont Doc">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnemptyContCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download Empty cont copy."
                                                        CommandName="EmptyContCopy" CommandArgument='<%#Eval("EmptyContDoc")%>' Visible='<%# DecideHereImg((string)Eval("EmptyContDoc")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Toll Doc">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnTollCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download toll copy."
                                                        CommandName="TollCopy" CommandArgument='<%#Eval("TollDoc")%>' Visible='<%# DecideHereImg((string)Eval("TollDoc")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Other Doc">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnOtherCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download other copy."
                                                        CommandName="OtherCopy" CommandArgument='<%#Eval("OtherDoc")%>' Visible='<%# DecideHereImg((string)Eval("OtherDoc")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>

                                            <asp:TemplateField HeaderText="Email Approval Copy">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnEmailApprovalCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download Email Approval copy."
                                                        CommandName="EmailApprovalCopy" CommandArgument='<%#Eval("EmailAttachment")%>' Visible='<%# DecideHereImg((string)Eval("EmailAttachment")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Contract Copy">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnContractCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download contract copy."
                                                        CommandName="ContractCopy" CommandArgument='<%#Eval("ContractAttachment")%>' Visible='<%# DecideHereImg((string)Eval("ContractAttachment")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:BoundField DataField="Instruction" HeaderText="Billing Instruction" ReadOnly="true" />--%>
                                            <asp:BoundField DataField="Remark" HeaderText="Other Remark" ReadOnly="true" />
                                            <asp:BoundField DataField="SellDetail" HeaderText="Charge to Party" ReadOnly="true" />
                                            <asp:BoundField DataField="SellAmount" HeaderText="Sell Amount" ReadOnly="true" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </fieldset>
                        <fieldset>
                            <legend>Bill Received Detail</legend>
                            <div>
                                <asp:GridView ID="gvBillReceived" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                                    DataSourceID="DataSourceBillReceived" CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="false"
                                    PageSize="20" FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="BillNumber" DataField="BillNumber" />
                                        <asp:BoundField HeaderText="Sent User" DataField="SentUser" />
                                        <asp:BoundField HeaderText="Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField HeaderText="ReceivedBy" DataField="ReceivedBy" />
                                        <asp:BoundField HeaderText="ReceivedDate" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField HeaderText="Status" DataField="StatusName" />
                                        <asp:BoundField HeaderText="Cheque No" DataField="ChequeNo" Visible="false" />
                                        <asp:BoundField HeaderText="Cheque Date" DataField="ChequeDate" DataFormatString="{0:dd/MM/yyyy}" Visible="false" />
                                        <asp:BoundField HeaderText="Hold Reason" DataField="HoldReason" Visible="false" />
                                        <asp:BoundField HeaderText="Release Date" DataField="ReleaseDate" DataFormatString="{0:dd/MM/yyyy}" Visible="false" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>
                        <fieldset>
                            <legend>Bill Received Status History</legend>
                            <div>
                                <asp:GridView ID="gvBillStatusHistory" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                                    DataSourceID="DataSourceBillStatusHistory" CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="false"
                                    PageSize="20" FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Status" DataField="StatusName" />
                                        <asp:BoundField HeaderText="Cheque No" DataField="ChequeNo" />
                                        <asp:BoundField HeaderText="Cheque Date" DataField="ChequeDate" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField HeaderText="Hold Reason" DataField="HoldReason" />
                                        <asp:BoundField HeaderText="Release Date" DataField="ReleaseDate" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField HeaderText="CreatedBy" DataField="CreatedBy" />
                                        <asp:BoundField HeaderText="Created Date" DataField="CreatedDate" DataFormatString="{0:dd/MM/yyyy HH:mm:tt}" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>
                    </ContentTemplate>
                </cc1:TabPanel>
                
                <cc1:TabPanel ID="TabPanelFund" runat="server" TabIndex="5" HeaderText="Fund Detail">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Fund Payment Detail</legend>
                            <div>
                                <asp:GridView ID="gvGetPaymentHistory" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="RequestId"
                                    DataSourceID="DataSourcePaymentHistory" CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="false"
                                    PageSize="20" FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:BoundField HeaderText="JobRefNo" DataField="JobRefNo" />--%>
                                        <asp:BoundField HeaderText="Type" DataField="InvoiceTypeName" />
                                        <asp:BoundField HeaderText="Status" DataField="StatusName" />
                                        <asp:BoundField HeaderText="Invoice No" DataField="InvoiceNo" />
                                        <asp:BoundField HeaderText="Invoice Date" DataField="InvoiceDate" DataFormatString="{0:dd/MM/yyyy HH:mm:tt}" />
                                        <asp:BoundField HeaderText="Amount" DataField="Amount" />
                                        <asp:BoundField HeaderText="Adv" DataField="AdvanceAmt" />
                                        <asp:BoundField HeaderText="Paid To" DataField="PaidTo" />
                                        <asp:BoundField HeaderText="Created By" DataField="RequestBy" />
                                        <asp:BoundField HeaderText="Created Date" DataField="dtDate" DataFormatString="{0:dd/MM/yyyy HH:mm:tt}" />
                                    </Columns>
                                </asp:GridView>
                                <br />
                            </div>
                        </fieldset>
                        <fieldset>
                            <legend>Fund Payment History</legend>
                            <div>
                                <asp:GridView ID="gvReqHistory" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    Width="100%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceRequestHistory"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Status" HeaderText="Status" />
                                        <asp:BoundField DataField="Remark" HeaderText="Remark" />
                                        <asp:BoundField DataField="CreatedBy" HeaderText="Updated By" />
                                        <asp:BoundField DataField="CreatedDate" HeaderText="Updated Date" DataFormatString="{0:dd/MM/yyyy HH:mm:ss tt}" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <asp:SqlDataSource ID="DataSourceRequestHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="TR_GetPaymentStatusHistory" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </fieldset>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="TabPanelBilling" runat="server" TabIndex="7" HeaderText="Billing">
                    <ContentTemplate>
                        <div>
                            <div class="m clear" style="text-align: center">
                                <asp:Button ID="btnSendToScrutiny" runat="server" Text="Send job to Scrutiny?" OnClick="btnSendToScrutiny_Click" />
                            </div>
                            <fieldset id="BillingScrutiny" runat="server">
                                <legend>Billing Scrutiny</legend>
                                <asp:Label ID="lblfreight" runat="server"></asp:Label>
                                <asp:GridView ID="gvbillingscrutiny" runat="server" AutoGenerateColumns="False"
                                    CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceBillingScrutiny"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40" OnRowDataBound="gvbillingscrutiny_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Status" DataField="Status" />
                                        <asp:BoundField HeaderText="Sent By Billing Advice" DataField="SentBy" />
                                        <asp:BoundField HeaderText="Billing Advice Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Received By Billing Scrutiny" DataField="ReceivedBy" />
                                        <asp:BoundField HeaderText="Billing Scrutiny Received Date" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Scrutiny Completed Date" DataField="ScrutinyDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Scrutiny Completed By" DataField="ScrutinyCompletedBy" />
                                        <asp:BoundField HeaderText="FreightJob" DataField="FreightjobNo" />
                                    </Columns>
                                </asp:GridView>
                            </fieldset>

                            <fieldset id="DraftInvoice" runat="server">
                                <legend>Draft Invoice</legend>
                                <asp:Label ID="lblConsolidated" runat="server"></asp:Label>
                                <asp:GridView ID="gvDraftInvoice" runat="server" AutoGenerateColumns="False"
                                    CssClass="table" Width="95%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceDraftinvoice"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40" OnRowCommand="gvDraftInvoice_RowCommand" OnRowDataBound="gvDraftInvoice_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Status" DataField="Status" />
                                        <asp:BoundField HeaderText="Sent By Billing Scrutiny" DataField="SentBy" />
                                        <asp:BoundField HeaderText="Billing Scrutiny Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Received By Draft Invoice" DataField="ReceivedBy" />
                                        <asp:BoundField HeaderText="Draft Invoice Received Date" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Draft Invoice Completed Date" DataField="DraftInvoiceDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Draft Invoice Completed By" DataField="FAUserName" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Consolidated Job No" DataField="ConsolidatedjobNo" />
                                        <asp:BoundField HeaderText="Remark" DataField="CrossRemark" />
                                        <asp:ButtonField Text="Next" ButtonType="Button" CommandName="DraftInvoiceNext" />
                                    </Columns>
                                </asp:GridView>
                            </fieldset>

                            <fieldset id="DraftCheck" runat="server">
                                <legend>Draft Check</legend>
                                <asp:GridView ID="gvDraftcheck" runat="server" AutoGenerateColumns="False"
                                    CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceDraftCheck"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Status" DataField="Status" />
                                        <asp:BoundField HeaderText="Sent By Draft Invoice" DataField="SentBy" />
                                        <asp:BoundField HeaderText="Draft Invoice Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Received By Draft Check" DataField="ReceivedBy" />
                                        <asp:BoundField HeaderText="Draft Check Received Date" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Draft Check Completed Date" DataField="DraftCheckDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    </Columns>
                                </asp:GridView>
                            </fieldset>

                            <fieldset id="FinalInvoiceTyping" runat="server">
                                <legend>Final Invoice Typing</legend>
                                <asp:GridView ID="gvFinaltyping" runat="server" AutoGenerateColumns="False"
                                    CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceFinalTyping"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40" OnRowCommand="gvFinaltyping_RowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Status" DataField="Status" />
                                        <asp:BoundField HeaderText="Sent By Draft Check" DataField="SentBy" />
                                        <asp:BoundField HeaderText="Draft Check Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Received By Final Typing" DataField="ReceivedBy" />
                                        <asp:BoundField HeaderText="Final Typing Received Date" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Final Typing Completed Date" DataField="FinalTypingDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Final Typing Completed by" DataField="FAUserName" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Comment" DataField="Comment" />
                                        <asp:ButtonField Text="Next" ButtonType="Button" CommandName="FinalTypingNext" />
                                    </Columns>
                                </asp:GridView>
                            </fieldset>

                            <fieldset id="FinalInvoiceCheck" runat="server">
                                <legend>Final Invoice Check</legend>
                                <asp:GridView ID="gvfinalcheck" runat="server" AutoGenerateColumns="False"
                                    CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceFinalCheck"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Status" DataField="Status" />
                                        <asp:BoundField HeaderText="Sent By Final Typing" DataField="SentBy" />
                                        <asp:BoundField HeaderText="Final Typing Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Received By Final Check" DataField="ReceivedBy" />
                                        <asp:BoundField HeaderText="Final Check Received Date" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Final Check Completed Date" DataField="FinalCheckDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    </Columns>
                                </asp:GridView>
                            </fieldset>

                            <fieldset id="Billdispatch" runat="server">
                                <legend>Bill Dispatch</legend>
                                <asp:GridView ID="gvbilldispatch" runat="server" AutoGenerateColumns="False" CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceBillDispatch"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Status" DataField="Status" />
                                        <asp:BoundField HeaderText="Sent By Final Check" DataField="SentBy" />
                                        <asp:BoundField HeaderText="Final Check Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Received By Bill Dispatch" DataField="ReceivedBy" />
                                        <asp:BoundField HeaderText="Bill Dispatch Received Date" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Bill Dispatch Completed Date" DataField="BillDispatchDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    </Columns>
                                </asp:GridView>
                            </fieldset>

                            <fieldset id="BillRejection" runat="server">
                                <legend>Bill Rejection</legend>
                                <asp:GridView ID="gvBillrejection" runat="server" AutoGenerateColumns="False" CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceBillRejection"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Stage" DataField="Stage" />
                                        <asp:BoundField HeaderText="Rejected by" DataField="RejectedBy" />
                                        <asp:BoundField HeaderText="Bill Rejection Date" DataField="RejectedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Reason" DataField="Reason" />
                                        <asp:BoundField HeaderText="Remark" DataField="Remark" />
                                        <asp:BoundField HeaderText="Followup Date" DataField="FollowupDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Followup Remark" DataField="FollowupRemark" />
                                    </Columns>
                                </asp:GridView>
                            </fieldset>

                            <fieldset>
                                <legend>Dispatch- Billing Dept</legend>
                                <asp:FormView ID="fvDispatchBilling" runat="server" HeaderStyle-Font-Bold="true" Width="100%">
                                    <ItemTemplate>
                                        <div class="m clear">
                                            <asp:Button ID="btnEditDispatchBilling" runat="server" OnClick="btnEditDispatchBilling_Click" CssClass="btn"
                                                Text="Edit" />
                                        </div>
                                        <asp:HiddenField ID="hdnBillingDelivery" Value='<%#Eval("BillingDeliveryId")%>' runat="server" />
                                        <asp:Panel runat="server" ID="pnlDispatchBillingHand" Visible="false">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                                <tr>
                                                    <td>Status
                                                    </td>
                                                    <td></td>
                                                    <td>Delivery Type
                                                    </td>
                                                    <td>
                                                        <%#Eval("BillingTypeOfDelivery")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Documents Carrying Person Name
                                                    </td>
                                                    <td>
                                                        <%#Eval("BillingCourierPersonName")%>
                                                    </td>
                                                    <td>Documents Pick Up / Dispatch Date
                                                    </td>
                                                    <td>
                                                        <%# Eval("BillingDispatchDate", "{0:dd/MM/yyyy}")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Documents Delivered Date
                                                    </td>
                                                    <td>
                                                        <%# Eval("BillingPCDRecvdDate", "{0:dd/MM/yyyy}")%>
                                                    </td>
                                                    <td>Documents Received Person Name
                                                    </td>
                                                    <td>
                                                        <%#Eval("BillingReceivedPersonName")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>POD Copy Attachment
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="lnkPODDispatchBilling" runat="server" Text="Download"
                                                            CommandArgument='<%#Eval("BillingDispatchDocPath") %>' OnClick="lnkPODCopyDownoad_Click"></asp:LinkButton>
                                                    </td>
                                                    <td></td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td>User Name
                                                    </td>
                                                    <td>
                                                        <%#Eval("BillingDispatchUser")%>
                                                    </td>
                                                    <td>Completed Date
                                                    </td>
                                                    <td>
                                                        <%# Eval("BillingDispatchUpdateDate", "{0:dd/MM/yyyy}")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="pnlDispatchBillingCour" Visible="false">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                                <tr>
                                                    <td>Status
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label8" Text='<%#GetBooleanToCompletedPending(Eval("PCDToDispatch"))%>' runat="server"></asp:Label>
                                                    </td>
                                                    <td>Delivery Type
                                                    </td>
                                                    <td>
                                                        <%#Eval("BillingTypeOfDelivery")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Courier Name
                                                    </td>
                                                    <td>
                                                        <%#Eval("BillingCourierName")%>
                                                    </td>
                                                    <td>Dispatch Docket No.
                                                    </td>
                                                    <td>
                                                        <%#Eval("BillingDocketNo")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Dispatch Date
                                                    </td>
                                                    <td>
                                                        <%# Eval("BillingDispatchDate", "{0:dd/MM/yyyy}")%>
                                                    </td>
                                                    <td>Documents Delivered Date
                                                    </td>
                                                    <td>
                                                        <%# Eval("BillingPCDRecvdDate", "{0:dd/MM/yyyy}")%>
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td>Documents Received Person Name
                                                    </td>
                                                    <td>
                                                        <%#Eval("BillingReceivedPersonName")%>
                                                    </td>
                                                    <td>POD Copy Attachment
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="lnkPODDispatchBillingCour" runat="server" Text="Download"
                                                            CommandArgument='<%#Eval("BillingDispatchDocPath") %>' OnClick="lnkPODCopyDownoad_Click"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>User Name
                                                    </td>
                                                    <td>
                                                        <%#Eval("BillingDispatchUser")%>
                                                    </td>
                                                    <td>Completed Date
                                                    </td>
                                                    <td>
                                                        <%# Eval("BillingDispatchUpdateDate", "{0:dd/MM/yyyy}")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>

                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:HiddenField ID="HiddenField1" Value='<%#Eval("BillingDeliveryId")%>' runat="server" />
                                        <div class="m clear">
                                            <asp:Button ID="btnUpdateDispatchBilling" runat="server" OnClick="btnUpdateDispatchBilling_Click" Text="Update" />
                                            <asp:Button ID="btnCancelDispatchBilling" runat="server" OnClick="btnCancelDispatchBilling_Click" CausesValidation="False"
                                                Text="Cancel" />
                                        </div>
                                        <asp:Panel runat="server" ID="pnlEditDispatchBillingHand" Visible="false">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                                <tr>
                                                    <td>Status
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label6" Text='<%# (Boolean.Parse(Eval("PCDToDispatch").ToString())) ? "Completed" : "Pending"%>'
                                                            runat="server"></asp:Label>
                                                    </td>
                                                    <td>Delivery Type
                                                    </td>
                                                    <td>
                                                        <%#Eval("BillingTypeOfDelivery")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Documents Carrying Person Name
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCarryingPersonName" runat="server" Text='<%#Bind("BillingCourierPersonName") %>' MaxLength="100"></asp:TextBox>
                                                    </td>
                                                    <td>Documents Pick Up / Dispatch Date
                                               <asp:CompareValidator ID="CompDispatchDate" runat="server" ControlToValidate="txtBillingDispatchDate" Display="Dynamic" Text="Invalid date."
                                                   ErrorMessage="Invalid Dispatch Date." SetFocusOnError="true" Type="Date" CultureInvariantValues="false" Operator="DataTypeCheck" ValidationGroup="Required"></asp:CompareValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtBillingDispatchDate" runat="server" Width="100px" Text='<%#Bind("BillingDispatchDate", "{0:dd/MM/yyyy}")%>' placeholder="dd/mm/yyyy"></asp:TextBox>
                                                        <asp:Image ID="imgDispatchDate2" ImageAlign="Top" ImageUrl="Images/btn_calendar.gif" runat="server" />
                                                        <cc1:CalendarExtender ID="CalDispatchDate" runat="server" Enabled="True" EnableViewState="False"
                                                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgDispatchDate2" PopupPosition="BottomRight"
                                                            TargetControlID="txtBillingDispatchDate">
                                                        </cc1:CalendarExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Documents Delivered Date
                                               <asp:CompareValidator ID="CompPCDDeliveryDate" runat="server" ControlToValidate="txtPCDDeliveryDate" Display="Dynamic" Text="Invalid date."
                                                   ErrorMessage="Invalid Delivered Date." SetFocusOnError="true" Type="Date" CultureInvariantValues="false" Operator="DataTypeCheck" ValidationGroup="Required"></asp:CompareValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPCDDeliveryDate" runat="server" Width="100px" Text='<%# Bind("BillingPCDRecvdDate", "{0:dd/MM/yyyy}")%>'></asp:TextBox>
                                                        <asp:Image ID="imgPCDDeliveryDate" ImageAlign="Top" ImageUrl="Images/btn_calendar.gif" runat="server" />
                                                        <cc1:CalendarExtender ID="CalPCDDeliveryDate2" runat="server" Enabled="True" EnableViewState="False"
                                                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgPCDDeliveryDate" PopupPosition="BottomRight"
                                                            TargetControlID="txtPCDDeliveryDate">
                                                        </cc1:CalendarExtender>
                                                    </td>
                                                    <td>Documents Received Person Name
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtReceivedPersonName" runat="server" Text='<%#Bind("BillingReceivedPersonName")%>'></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>POD Copy Attachment
                                                    </td>
                                                    <td>
                                                        <asp:FileUpload ID="fileUpHandDelivery" runat="server" />
                                                        <asp:HiddenField ID="hdnBillingHandDeliveryFilePath" runat="server" Value='<%#Bind("BillingDispatchDocPath") %>' />
                                                    </td>
                                                    <td></td>
                                                    <td></td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="pnlEditDispatchBillingCour" Visible="false">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                                <tr>
                                                    <td>Status
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label1" Text='<%# (Boolean.Parse(Eval("PCDToDispatch").ToString())) ? "Completed" : "Pending"%>'
                                                            runat="server"></asp:Label>
                                                    </td>
                                                    <td>Delivery Type
                                                    </td>
                                                    <td>
                                                        <%#Eval("BillingTypeOfDelivery")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Courier Name
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEditBillingCourierName" runat="server" Text='<%#Eval("BillingCourierName") %>'></asp:TextBox>
                                                    </td>
                                                    <td>Dispatch Docket No.
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEditBillingDocketNo" runat="server" Text='<%#Eval("BillingDocketNo") %>'></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Dispatch Date
                                               <asp:CompareValidator ID="CompValDate201" runat="server" ControlToValidate="txtEditBillingDispatchDate" Display="Dynamic" Text="Invalid date."
                                                   ErrorMessage="Invalid Dispatch Date." SetFocusOnError="true" Type="Date" CultureInvariantValues="false" Operator="DataTypeCheck" ValidationGroup="Required"></asp:CompareValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEditBillingDispatchDate" runat="server" Text='<%# Eval("BillingDispatchDate", "{0:dd/MM/yyyy}")%>' Width="100px"></asp:TextBox>
                                                        <asp:Image ID="imgEdDate201" ImageAlign="Top" ImageUrl="Images/btn_calendar.gif" runat="server" />
                                                        <cc1:CalendarExtender ID="CalEdDate201" runat="server" Enabled="True" EnableViewState="False"
                                                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgEdDate201" PopupPosition="BottomRight"
                                                            TargetControlID="txtEditBillingDispatchDate">
                                                        </cc1:CalendarExtender>
                                                    </td>
                                                    <td>Documents Delivered Date
                                                <asp:CompareValidator ID="CampVal202" runat="server" ControlToValidate="txtEditBillingPCDRecvdDate" Display="Dynamic" Text="Invalid date."
                                                    ErrorMessage="Invalid Delivered Date." SetFocusOnError="true" Type="Date" CultureInvariantValues="false" Operator="DataTypeCheck" ValidationGroup="Required"></asp:CompareValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEditBillingPCDRecvdDate" runat="server" Text='<%# Eval("BillingPCDRecvdDate", "{0:dd/MM/yyyy}")%>' Width="100px"></asp:TextBox>
                                                        <asp:Image ID="imgEdDate202" ImageAlign="Top" ImageUrl="Images/btn_calendar.gif" runat="server" />
                                                        <cc1:CalendarExtender ID="CalEdDate202" runat="server" Enabled="True" EnableViewState="False"
                                                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgEdDate202" PopupPosition="BottomRight"
                                                            TargetControlID="txtEditBillingPCDRecvdDate">
                                                        </cc1:CalendarExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Documents Received Person Name
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEditBillingReceivedPersonName" runat="server" Text='<%# Eval("BillingReceivedPersonName", "{0:dd/MM/yyyy}")%>'></asp:TextBox>
                                                    </td>
                                                    <td>POD Copy Attachment
                                                    </td>
                                                    <td>
                                                        <asp:FileUpload ID="fuEditPODDispatchBillingCour" runat="server" />
                                                        <asp:HiddenField ID="hdnEditPODDispatchBillingCour" Value='<%#Eval("BillingDispatchDocPath") %>' runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </EditItemTemplate>

                                </asp:FormView>
                            </fieldset>

                            <fieldset id="fsRepository" runat="server">
                                <legend>Billing Repository</legend>
                                <asp:Label ID="lblBillReportMsg" runat="server"></asp:Label>
                                <asp:GridView ID="gvBillingRepository" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    Width="100%" PagerStyle-CssClass="pgr" CellPadding="4" AllowPaging="True" AllowSorting="True"
                                    OnRowCommand="gvBillingRepository_RowCommand" PageSize="20">
                                    <Columns>
                                        <asp:TemplateField HeaderText="SI">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Name" HeaderText="Document" />
                                        <asp:BoundField DataField="CreationTime" HeaderText="Date" />
                                        <asp:TemplateField HeaderText="Download">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDownloadRepo" runat="server" Text="Download" CommandName="downloadrepo"
                                                    CommandArgument='<%#Eval("FullName") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </fieldset>

                            <div id="div1">
                                <asp:SqlDataSource ID="DataSourceBillingScrutiny" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetBillingScrutinyById" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="TRId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                                <asp:SqlDataSource ID="DataSourceDraftinvoice" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetDraftInvoiceById" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="TRId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                                <asp:SqlDataSource ID="DataSourceDraftCheck" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetDraftCheckById" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="TRId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                                <asp:SqlDataSource ID="DataSourceFinalTyping" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetFinalTypingById" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="TRId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                                <asp:SqlDataSource ID="DataSourceFinalCheck" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetFinalCheckById" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="TRId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                                <asp:SqlDataSource ID="DataSourceBillDispatch" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetBillDispatchById" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="TRId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                                <asp:SqlDataSource ID="DataSourceBillRejection" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetBillRejectionById" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="TRId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                            </div>
                        </div>
                    </ContentTemplate>
                </cc1:TabPanel>

                 <cc1:TabPanel ID="TabPanelTripDetail" runat="server" TabIndex="7" HeaderText="Trip Detail">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Trip Detail</legend>
                            <div>
                                <asp:GridView ID="gvVehicleExpense" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    Width="100%" DataKeyNames="lId" DataSourceID="DataSourceVehicleExpense" CellPadding="4">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Vehicle No" DataField="VehicleNo" ReadOnly="true" />
                                        <asp:BoundField HeaderText="Fuel" DataField="Fuel2" ReadOnly="true" />
                                        <asp:BoundField HeaderText="Fuel Liter" DataField="Fuel2Liter" ReadOnly="true" />
                                        <asp:BoundField HeaderText="Toll Charges" DataField="TollCharges" ReadOnly="true" />
                                        <asp:BoundField HeaderText="Fine Without Cleaner" DataField="FineWithoutCleaner" ReadOnly="true" />
                                        <asp:BoundField HeaderText="Xerox" DataField="Xerox" ReadOnly="true" />
                                        <asp:BoundField HeaderText="Varai Unloading" DataField="VaraiUnloading" ReadOnly="true" />
                                        <asp:BoundField HeaderText="Empty Container Receipt" DataField="EmptyContainerReceipt" ReadOnly="true" />
                                        <asp:BoundField HeaderText="Parking/GatePass" DataField="ParkingGatePass" ReadOnly="true" />
                                        <asp:BoundField HeaderText="Garage" DataField="Garage" ReadOnly="true" />
                                        <asp:BoundField HeaderText="Bhatta" DataField="Bhatta" ReadOnly="true" />
                                        <asp:BoundField HeaderText="ODC/Overweight" DataField="AdditionalChargesForODCOverweight" ReadOnly="true" />
                                        <asp:BoundField HeaderText="OtherCharges" DataField="OtherCharges" ReadOnly="true" />
                                        <asp:BoundField HeaderText="NakaPassing/DamageContainer" DataField="NakaPassingDamageContainer" ReadOnly="true" />
                                        <asp:BoundField HeaderText="Created By" DataField="CreatedBy" ReadOnly="true" />
                                        <asp:BoundField HeaderText="Created Date" DataField="CreatedDate" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="DataSourceVehicleExpense" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="TR_GetVehicleRateExpense" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>
                        </fieldset>
                    </ContentTemplate>
                </cc1:TabPanel>

                <cc1:TabPanel runat="server" ID="TabPanel2" HeaderText="Truck Request">
                    <ContentTemplate>
                         <div>
                            <div class="m clear" style="text-align: center">
                                <asp:Label ID="lblTruckerr" runat="server"></asp:Label>
                            </div>
                        </div>
                        <asp:HiddenField ID="hdnTransReqId" runat="server" Value="0" />
                        
                        <div>
                            <fieldset>
                                <legend>Truck Request Details</legend>
                                <asp:GridView ID="gvTruckRequest" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                                    DataSourceID="DataSourceTruckRequest" CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="false"
                                    PageSize="20" OnRowCommand="gvTruckRequest_RowCommand" FooterStyle-ForeColor="Black" FooterStyle-CssClass="table"
                                    Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:BoundField DataField="TRRefNo" HeaderText="Ref No" ReadOnly="true"  />--%>
                                        <asp:TemplateField HeaderText="Ref No">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkRefNo" runat="server" Text='<%#Eval("TRRefNo") %>' CommandName="Select"
                                                    CommandArgument='<%#Eval("TRRefNo") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="JobRefNo" HeaderText="Job No" ReadOnly="true" />
                                        <asp:BoundField DataField="RequestDate" HeaderText="Truck Request Date" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="CustName" HeaderText="Customer Name" ReadOnly="true" />
                                        <asp:BoundField DataField="VehiclePlaceDate" HeaderText="Vehicle Place Require Date" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="LocationFrom" HeaderText="Location" ReadOnly="true" />
                                        <asp:BoundField DataField="Destination" HeaderText="Destination" ReadOnly="true" />
                                        <asp:BoundField DataField="GrossWeight" HeaderText="Gross Weight (Kgs)" ReadOnly="true" />
                                        <asp:BoundField DataField="Count20" HeaderText="Cont 20'" ReadOnly="true" />
                                        <asp:BoundField DataField="Count40" HeaderText="Cont 40'" ReadOnly="true" />
                                        <asp:BoundField DataField="DelExportType_Value" HeaderText="Delivery Type" ReadOnly="true" />
                                        <asp:BoundField DataField="DelExportType_Value" HeaderText="Export Type" ReadOnly="true" />
                                        <asp:BoundField DataField="Dimension" HeaderText="Dimension" ReadOnly="true" />
                                        <asp:BoundField DataField="PickupPincode" HeaderText="PickUp Pincode" ReadOnly="True" />
                                       <asp:BoundField DataField="PickupCity" HeaderText="PickUp City" ReadOnly="True" />
                                       <asp:BoundField DataField="PickupState" HeaderText="PickUp State" ReadOnly="True" />                                 
                                       <asp:BoundField DataField="DropPincode" HeaderText="Drop Pincode" ReadOnly="True" />
                                       <asp:BoundField DataField="DropCity" HeaderText="Drop City" ReadOnly="True" />
                                       <asp:BoundField DataField="DropState" HeaderText="Drop State" ReadOnly="True" />
                                        <asp:TemplateField HeaderText="Packing List">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnPackingList" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20"
                                                    ToolTip="Click to view documents." CommandName="PackingDocs" CommandArgument='<%# Bind("lId") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy" ReadOnly="true" />
                                        <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" SortExpression="CreatedDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="UpdatedBy" HeaderText="Updated By" SortExpression="UpdatedBy" ReadOnly="true" />
                                        <asp:BoundField DataField="UpdatedDate" HeaderText="Updated Date" SortExpression="UpdatedDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                                    </Columns>
                                </asp:GridView>

                                <fieldset>
                                    <div id="dvtruckDetail" runat="server" visible="false">
                                    <legend >Fill Job Detail</legend>
                                    
                                        <table id="tblTruckRequest" border="0" cellpadding="0" cellspacing="0" bgcolor="white"  runat="server">
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Button ID="btnSubmit" runat="server" Text="Save" ValidationGroup="Required" OnClick="btnSubmit_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Transport Ref No</td>
                                                <td>
                                                    <asp:Label ID="lblTransRefNo" runat="server" Font-Bold="true"></asp:Label>
                                                </td>
                                                <td>Job Number
                                                </td>
                                                <td>
                                                    <%-- <asp:TextBox ID="txtJobNumber" Width="160px" runat="server" ToolTip="Enter Job Number."
                                                CssClass="SearchTextbox" placeholder="Search" TabIndex="1" AutoPostBack="true"></asp:TextBox>--%>
                                                    <asp:Label ID="lblJobNumber" runat="server" Font-Bold="true"></asp:Label>
                                                    <div id="divwidthCust_Loc" runat="server">
                                                    </div>
                                                </td>
                                            </tr>
                                            
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblType_Title" runat="server" Text="Delivery Type"></asp:Label>
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
                                                <td>Delivery Destination
                                            <asp:RequiredFieldValidator ID="rfvDestination" runat="server" ControlToValidate="txtDestination" SetFocusOnError="true"
                                                Text="*" ErrorMessage="Please Enter Delivery Destination" ValidationGroup="Required"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDestination" runat="server" MaxLength="100" TabIndex="3" Width="250px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Dimension</td>
                                                <td>
                                                    <asp:TextBox ID="txtDimension" runat="server" TextMode="MultiLine" ToolTip="Enter Dimension"
                                                        TabIndex="4" PlaceHolder="Dimension" Width="250px" Visible="true"></asp:TextBox>
                                                </td>
                                                <td>Vehicle Place Require Date
                                <cc1:CalendarExtender ID="calVehiclePlaceDate" runat="server" FirstDayOfWeek="Sunday" PopupButtonID="imgVehiclePlaceDate"
                                    Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtVehiclePlaceDate">
                                </cc1:CalendarExtender>
                                                    <cc1:MaskedEditExtender ID="meeVehiclePlaceDate" TargetControlID="txtVehiclePlaceDate" Mask="99/99/9999" MessageValidatorTip="true"
                                                        MaskType="Date" AutoComplete="false" runat="server">
                                                    </cc1:MaskedEditExtender>
                                                    <cc1:MaskedEditValidator ID="mevVehiclePlaceDate" ControlExtender="meeVehiclePlaceDate" ControlToValidate="txtVehiclePlaceDate" IsValidEmpty="false"
                                                        InvalidValueMessage="Vehicle Place Require Date is invalid" InvalidValueBlurredMessage="Invalid Vehicle Place Require Date" SetFocusOnError="true"
                                                        MinimumValueMessage="Invalid Vehicle Place Require Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2015" MaximumValue="31/12/2025"
                                                        runat="Server" ValidationGroup="Required" EmptyValueMessage="Please enter vehicle place require date." EmptyValueBlurredText="*"></cc1:MaskedEditValidator>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtVehiclePlaceDate" runat="server" Width="125px" placeholder="dd/mm/yyyy" TabIndex="5" ToolTip="Enter Vehicle Place Require Date."></asp:TextBox>
                                                    <asp:Image ID="imgVehiclePlaceDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                                </td>
                                            </tr>
                                            <tr runat="server">
                                               <td runat="server">
                                               </td>
                                               <td runat="server"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Pincode&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; City&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; State
                                               </td>
                                               <td runat="server"></td>
                                               <td runat="server">
                                                   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Pincode&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; City&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; State
                                               </td>
                                            </tr>
                                            <tr runat="server">
                                                <td runat="server">Enter PickUpPincode</td>
                                                <td runat="server">
                                                    <asp:TextBox ID="txtpickPincode" runat="server" AutoPostBack="True" CssClass="SearchTextbox" placeholder="Search" TabIndex="3" ToolTip="Enter Pincode" Width="100px" OnTextChanged="txtpickPincode_TextChanged"  ></asp:TextBox><%----%>
                                                        <asp:HiddenField ID="hdnPincodeId" runat="server" Value="0" />&nbsp;
                                                        &nbsp;<asp:TextBox ID="txtpickCity" runat="server" Enabled="False" Width="100px"></asp:TextBox>
                                                    &nbsp;<asp:TextBox ID="txtpickState" runat="server" Enabled="False" Width="100px"></asp:TextBox>
                                               
                                                </td>
                                                <td runat="server">Enter DropPincode</td>
                                                <td runat="server">
                                                    <asp:TextBox ID="txtdropPincode" runat="server" AutoPostBack="True" CssClass="SearchTextbox"  placeholder="Search" TabIndex="3" ToolTip="Enter Pincode" Width="100px" OnTextChanged="txtdropPincode_TextChanged" ></asp:TextBox><%----%>
                                                        <asp:HiddenField ID="hdnpinid" runat="server" Value="0" />&nbsp;
                                                    &nbsp;
                                                    <asp:TextBox ID="txtdropCity" runat="server" Enabled="False" Width="100px"></asp:TextBox>
                                                    &nbsp;
                                                    <asp:TextBox ID="txtdropState" runat="server" Enabled="False" Width="100px"></asp:TextBox>
                                             
                                                </td>
                                            </tr>    
                                            <tr>
                                                <td>Remark</td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtRemark1" runat="server" TextMode="MultiLine" Rows="3" Width="900px" TabIndex="6"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                              <td>     
                                                  <asp:Label ID="lblEmpty_Letter" runat="server" Text="Empty Letter" ValidationGroup="Required" Visible="false"></asp:Label>
                                                   <asp:RequiredFieldValidator ID="rfvLoadedDocuments" runat="server" ControlToValidate="loadedDocuments" SetFocusOnError="true"
                                                    Text="*" ErrorMessage="Please select a document to upload"></asp:RequiredFieldValidator><%-- --%>
                                              </td>
                                              <td>                                 
                                                   <div class="file-upload">                                  
                                                   <label for="FileUpload1" class="file-upload-label"> </label>                               
                                                   <asp:FileUpload ID="loadedDocuments"  runat="server" CssClass="file-upload-input" Visible="false"/> 
                                                <%--  <asp:Button ID="UpdBtn" Text="Upload File" runat="server" Visible="false" OnClick="UpdBtn_Click"/>--%> <%-- EnableViewState="true"--%>
                                                </div>
                                             </td>
                                            </tr>
                                             <tr>
                                                    <td>
                                                        Packing List
                                                    </td>
                                                    <td colspan="3"><asp:FileUpload ID="FileUpload1" runat="server" />
                                                        <asp:Button ID="btnSaveDocument" Text="Save Document" runat="server" OnClick="btnSaveDocument_Click" />
                                                    </td>
                                                </tr>
                                        </table>
                                        </div>
                                        <br />
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
                                    </div>
                                </fieldset>
                            </fieldset>
                        </div>
                        <%--
                        <div style="overflow: scroll;">
                            <fieldset>
                                <legend>Vehicle Rate Detail</legend>
                                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" CssClass="table"
                                    Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                                    DataSourceID="DataSourceRate" CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="false"
                                    PageSize="20" FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="TransporterName" HeaderText="Transporter" SortExpression="TransporterName" ReadOnly="true" />
                                        <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No" SortExpression="VehicleNo" ReadOnly="true" />
                                        <asp:BoundField DataField="VehicleTypeName" HeaderText="Vehicle Type" SortExpression="VehicleTypeName" ReadOnly="true" />
                                        <asp:BoundField DataField="LRNo" HeaderText="LRNo" SortExpression="LRNo" ReadOnly="true" />
                                        <asp:BoundField DataField="LRDate" HeaderText="LRDate" DataFormatString="{0:dd/MM/yyyy}" SortExpression="LRDate" ReadOnly="true" />
                                        <asp:BoundField DataField="ChallanNo" HeaderText="ChallanNo" SortExpression="ChallanNo" ReadOnly="true" />
                                        <asp:BoundField DataField="ChallanDate" HeaderText="Challan Date" SortExpression="ChallanDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="Rate" HeaderText="Freight Rate" SortExpression="Rate" ReadOnly="true" ItemStyle-HorizontalAlign="Right" Visible="false" />
                                        <asp:BoundField DataField="Advance" HeaderText="Advance (%)" SortExpression="Advance" ReadOnly="true" ItemStyle-HorizontalAlign="Right" Visible="false" />
                                        <asp:BoundField DataField="AdvanceAmount" HeaderText="AdvanceAmount" SortExpression="AdvanceAmount" ReadOnly="true" ItemStyle-HorizontalAlign="Right" Visible="false" />
                                        <asp:BoundField DataField="MarketBillingRate" HeaderText="Market Rate" SortExpression="MarketBillingRate" ReadOnly="true" ItemStyle-HorizontalAlign="Right" Visible="false" />
                                        <asp:BoundField DataField="FreightAmount" HeaderText="Freight Amt" SortExpression="FreightAmount" ReadOnly="true" Visible="false" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="DetentionAmount" HeaderText="Detention Amt" SortExpression="DetentionAmount" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="VaraiExpense" HeaderText="Varai Exp" SortExpression="VaraiExpense" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="EmptyContRecptCharges" HeaderText="Empty Cont Charges" SortExpression="EmptyContRecptCharges" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy" ReadOnly="true" />
                                        <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" SortExpression="CreatedDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="UpdatedBy" HeaderText="Updated By" SortExpression="UpdatedBy" ReadOnly="true" />
                                        <asp:BoundField DataField="UpdatedDate" HeaderText="Updated Date" SortExpression="UpdatedDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                                    </Columns>
                                </asp:GridView>
                            </fieldset>
                        </div>--%>

                        <div id="divTruckRequest">
                           
                            <asp:SqlDataSource ID="DataSourceTruckRequest" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="TR_GetTruckRequestById" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <%--<asp:ControlParameter ControlID="hdnTransReqId" PropertyName="Value" Name="TransportId" />--%>
                                    <asp:SessionParameter Name="TransportId" SessionField="TRId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="TR_GetTransRateDetail" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="hdnTransReqId" PropertyName="Value" Name="TransReqId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </ContentTemplate>
                </cc1:TabPanel>

                <cc1:TabPanel ID="TabPanelContainer" runat="server" TabIndex="6" HeaderText="Container">
                    <ContentTemplate>
                        <div>
                            <div align="center">
                                <asp:Label ID="lblError" runat="server"></asp:Label>
                            </div>
                        </div>
                        <fieldset id="fsContainerDetails" runat="server">
                            <legend>Add Container</legend>
                            <div class="m clear">
                                <asp:Button ID="btnAddContainer" Text="Add" OnClick="btnAddContainer_Click"
                                    ValidationGroup="valContainer" runat="server" TabIndex="24" />
                                <asp:Button ID="btnCancelContainer" Text="Cancel" TabIndex="25" OnClick="btnCancelContainer_Click" BackColor="#1c698a"
                                    CausesValidation="false" runat="server" Visible="false" />
                            </div>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white" style="border: 1px solid #1c698a;">
                                <tr>
                                    <td>Container No
                                        <asp:RequiredFieldValidator ID="RFVContainer" runat="server" ControlToValidate="txtContainerNo"
                                            ValidationGroup="valContainer" SetFocusOnError="True" ErrorMessage="Enter Container No"
                                            Text="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtContainerNo" runat="server" MaxLength="11" TabIndex="21"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="REVContainer" runat="server" ControlToValidate="txtContainerNo"
                                            ValidationGroup="valContainer" SetFocusOnError="True" ErrorMessage="Enter 11 Digit Container No."
                                            Display="Dynamic" ValidationExpression="^[a-zA-Z0-9]{11}$"></asp:RegularExpressionValidator>
                                    </td>
                                    <td>Container Type
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddContainerType" TabIndex="22" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddContainerType_SelectedIndexChanged">
                                            <asp:ListItem Text="FCL" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="LCL" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Container Size
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddContainerSize" runat="server" TabIndex="23">
                                            <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="20" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="40" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="45" Value="3"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                            <br />
                            <asp:GridView ID="gvContainer" CssClass="table" runat="server" AutoGenerateColumns="false" Width="100%" DataSourceID="DataSourceContainer"
                                OnRowCommand="gvContainer_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ContainerNo" HeaderText="Container No" />
                                    <asp:BoundField DataField="ContainerSize" HeaderText="Container Size" />
                                    <asp:BoundField DataField="ContainerType" HeaderText="Container Type" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton Text="Delete" runat="server" CommandName="DeleteRow" CommandArgument='<%#Eval("lid") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <div>
                                <asp:SqlDataSource ID="DataSourceVehicle" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="EX_GetDeliveryDetail" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>
                        </fieldset>
                    </ContentTemplate>
                </cc1:TabPanel>
               
            </cc1:tabcontainer>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="divDataSource">
        <asp:SqlDataSource ID="DataSourceDelivery" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetDeliveryDetail" SelectCommandType="StoredProcedure" UpdateCommand="TR_updateDeliveryDetail"
            UpdateCommandType="StoredProcedure" OnUpdated="DataSourceDelivery_Updated" DeleteCommand="TR_delDeliveryDetail" DeleteCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="JobId" SessionField="TRId" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="Deliveryid" Type="int32" />
                <asp:Parameter Name="NoOfPackages" Type="String" DefaultValue="0" />
                <asp:Parameter Name="VehicleNo" Type="String" />
                <%-- <asp:Parameter Name="VehicleType" Type="string" />
                <asp:Parameter Name="VehicleRcvdDate" DbType="datetime" />--%>
                <asp:Parameter Name="TransporterId" Type="int32" />
                <asp:Parameter Name="TransporterName" Type="string" />
                <%--<asp:Parameter Name="LRNo" Type="string" />
                <asp:Parameter Name="LRDate" DbType="datetime" />
                <asp:Parameter Name="DeliveryPoint" Type="string" />
                <asp:Parameter Name="DispatchDate" DbType="datetime" />
                <asp:Parameter Name="DeliveryDate" DbType="datetime" />
                <asp:Parameter Name="EmptyContRetrunDate" DbType="datetime" />
                <asp:Parameter Name="PODAttachment" DbType="String" />
                <asp:Parameter Name="CargoReceivedby" DbType="String" />
                <asp:Parameter Name="BabajiChallanNo" DbType="string" />
                <asp:Parameter Name="BabajiChallanDate" DbType="datetime" />
                <asp:Parameter Name="BabajiChallanCopyFile" DbType="string" />
                <asp:Parameter Name="DriverName" DbType="string" />
                <asp:Parameter Name="DriverPhoneno" DbType="string" />--%>
                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                <asp:Parameter Name="OutPut" Type="int32" Direction="Output" />
            </UpdateParameters>
            <DeleteParameters>
                <asp:Parameter Name="lid" Type="int32" />
                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                <asp:Parameter Name="OutPut" Type="int32" Direction="Output" />
            </DeleteParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="GetVehicleMSSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="GetVehicleMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceRate" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetTransRateDetail" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceBillDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetTransBillDetail" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceBillReceived" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetBillReceivedByTransReqId" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceBillApprovalHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetBillApprovalHistory" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceBillStatusHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetBillReceivedDetailHistory" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourcePaymentHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TRS_GetInvoiceByJobID" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceSellingRate" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetVehicleSellingDetail" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqID" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceMovementHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetMovementHistory" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceDailyStatus" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetDailyStatusHistory" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="TransReqId" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceContainer" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="TR_GetContainerDetail" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="JobId" SessionField="TRId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DataSourceWarehouse" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="GetDeliveryDetail" SelectCommandType="StoredProcedure" UpdateCommand="updJobWarehouseDeliveryDetail"
            UpdateCommandType="StoredProcedure" OnUpdated="DataSourceWarehouse_Updated" DeleteCommand="delDeliveryWarhouseDetail" DeleteCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="JobId" SessionField="TrJobId" />
                <asp:Parameter Name="TransitType" DefaultValue="2" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="lid" Type="int32" />
                <asp:Parameter Name="NoOfPackages" Type="String" DefaultValue="0" />
                <asp:Parameter Name="VehicleNo" Type="string" />
                <asp:Parameter Name="VehicleType" Type="string" />
                <asp:Parameter Name="VehicleRcvdDate" DbType="datetime" />
                <asp:Parameter Name="TransporterName" Type="string" />
                <asp:Parameter Name="LRNo" Type="string" />
                <asp:Parameter Name="LRdate" DbType="datetime" />
                <asp:Parameter Name="DeliveryPoint" Type="string" />
                <asp:Parameter Name="DispatchDate" DbType="datetime" />
                <asp:Parameter Name="RoadPermitNo" DbType="String" />
                <asp:Parameter Name="RoadPermitDate" DbType="datetime" />
                <asp:Parameter Name="PODAttachment" DbType="String" />
                <asp:Parameter Name="NFormNo" DbType="string" />
                <asp:Parameter Name="NFormDate" DbType="datetime" />
                <asp:Parameter Name="NClosingDate" DbType="datetime" />
                <asp:Parameter Name="SFormNo" DbType="string" />
                <asp:Parameter Name="SFormDate" DbType="datetime" />
                <asp:Parameter Name="SClosingDate" DbType="datetime" />
                <asp:Parameter Name="OctroiAmount" DbType="decimal" />
                <asp:Parameter Name="OctroiReceiptNo" DbType="String" />
                <asp:Parameter Name="OctroiPaidDate" DbType="datetime" />
                <asp:Parameter Name="BabajiChallanNo" DbType="string" />
                <asp:Parameter Name="BabajiChallanDate" DbType="datetime" />
                <asp:Parameter Name="BabajiChallanCopyFile" DbType="string" />
                <asp:Parameter Name="DamagedImageFile" DbType="string" />
                <asp:Parameter Name="DriverName" DbType="string" />
                <asp:Parameter Name="DriverPhoneno" DbType="string" />
                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                <asp:Parameter Name="OutPut" Type="int32" Direction="Output" />
            </UpdateParameters>
            <DeleteParameters>
                <asp:Parameter Name="lid" Type="int32" />
                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                <asp:Parameter Name="OutPut" Type="int32" Direction="Output" />
            </DeleteParameters>
        </asp:SqlDataSource>
    </div>
</asp:Content>
