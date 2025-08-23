<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BillStatusDetail.aspx.cs" Inherits="PCA_BillStatusDetail"
    Title="Job Detail" MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="GVPager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .Tab .ajax__tab_header {
            white-space: nowrap !important;
        }
    </style>
    <AjaxToolkit:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upJobDetail" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); -moz-opacity: .8; opacity: .8;">
                    <img alt="progress" src="../images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <script type="text/javascript">

        function ActiveTabChanged12() {
            /* Clear Error Message on Tab change Event */
            document.getElementById('<%=lblError.ClientID%>').innerHTML = '';
            document.getElementById('<%=lblError.ClientID%>').className = '';
        }

        function divexpandcollapse(divname) {
            var div = document.getElementById(divname);
            var img = document.getElementById('img' + divname);

            if (div.style.display == "none") {
                div.style.display = "inline";
                img.src = "Images/minus.png";
                img.title = 'Collapse';
            }
            else {
                div.style.display = "none";
                img.src = "Images/plus.png";
                img.title = 'Expand';
            }
        }

    </script>

    <script type="text/javascript">

        function OnPortOfLoadingSelected(source, eventArgs) {
            // alert(eventArgs.get_value());
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=hdnLoadingPortId.ClientID%>').value = results.PortOfLoadingId;
        }
        $addHandler
        (
            $get('ctl00_ContentPlaceHolder1_Tabs_TabPanelJobDetail_FVJobDetail_txtPortOfLoading'), 'keyup',
            function () {
                $get('<%=hdnLoadingPortId.ClientID %>').value = '0';
                alert($get('<%=hdnLoadingPortId.ClientID %>').value)
            }
        );
    </script>

    <asp:UpdatePanel ID="upJobDetail" runat="server">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:HiddenField ID="hdnPreAlertId" runat="server" />
                <asp:HiddenField ID="hdnCustId" runat="server" />
                <asp:HiddenField ID="hdnMode" runat="server" />
                <asp:HiddenField ID="hdnBoeTypeId" runat="server" />
                <asp:HiddenField ID="hdnDeliveryType" runat="server" />
                <asp:HiddenField ID="hdnLoadingPortId" runat="server" />
                <asp:HiddenField ID="hdnUploadPath" runat="server" />
            </div>
            <div align="center">
                <asp:Label ID="lblMessage" runat="server" Test="No Contract Is Available In System"></asp:Label>
            </div>
            <div class="clear"></div>

            <AjaxToolkit:TabContainer runat="server" ID="Tabs" ActiveTabIndex="0" CssClass="Tab"
                Width="100%" OnClientActiveTabChanged="ActiveTabChanged12" AutoPostBack="false">
                <%--Start Billing--%>
                <AjaxToolkit:TabPanel runat="server" ID="TabBiiling" HeaderText="Biiling Details">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Job Detail</legend>
                        <asp:FormView ID="FVJobDetail" HeaderStyle-Font-Bold="true" runat="server" DataKeyNames="JobId"
                            Width="100%" OnDataBound="FVJobDetail_DataBound">
                            <ItemTemplate>
                                <div class="m clear">
                                    <asp:Button ID="btnBackButton" runat="server" OnClick="btnBackButton_Click" Text="Back" Visible="false" />
                                </div>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                    <tr>
                                        <td>BS Job No.
                                        </td>
                                        <td>
                                                <asp:Label ID="lblJobRefNo" Text='<%# Eval("JobRefNo") %>' runat="server"></asp:Label>

                                                &nbsp;
                                            <asp:Label ID="lblInbondJobNo" Text="Inbond Job" runat="server" Visible="false"></asp:Label>
                                        </td>
                                        <td>Cust Ref No.
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("CustRefNo") %>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Customer
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("Customer")%>
                                            </span>
                                        </td>
                                        <td>Consignee
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("Consignee")%>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Customer Division
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDivision" runat="server" Text='<%#Eval("Division") %>'></asp:Label>
                                        </td>
                                        <td>Customer Plant
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPlant" runat="server" Text='<%#Eval("Plant") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Mode
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("Mode")%>
                                            </span>
                                        </td>
                                        <td>Port of Discharge
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("Port")%>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Babaji Branch
                                        </td>
                                        <td>
                                            <span>
                                                <asp:Label ID="lblBabajiBranch" runat="server" Text='<%#Eval("BabajiBranch") %>'></asp:Label>
                                            </span>
                                        </td>
                                        <td>Priority
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("Priority")%>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Gross Weight
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("GrossWT")%> <span>Kgs</span>
                                            </span>
                                        </td>
                                        <td>Packages
                                        </td>
                                        <td>
                                            <%# Eval("NoOfPackages")%>
                                            &nbsp;
                                            <%#Eval("PackageTypeName")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Container
                                        </td>
                                        <td>
                                            <span>
                                                20" - <b> <%# Eval("Count20")%> </b> &nbsp;&nbsp;&nbsp;
                                                40" - <b><%# Eval("Count40")%> </b> &nbsp;&nbsp;&nbsp;
                                                LCL - <b><%# Eval("CountLCL")%> </b> 
                                            </span>
                                        </td>
                                        <td>
                                            Total Container
                                        </td>
                                        <td>
                                            <b><%# Eval("CountTotal")%> </b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Job Type
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("JobType")%>
                                            </span>
                                        </td>
                                        <td>RMS/NonRMS
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("RMSNonRMS")%>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>BoE No
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("BOENo")%>
                                            </span>
                                        </td>
                                        <td>BoE Date
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("BOEDate","{0:dd/MM/yyyy}")%>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>MBL No
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("MAWBNo")%>
                                            </span>
                                        </td>
                                        <td>MBL Date
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("MAWBDate","{0:dd/MM/yyyy}")%>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Delivery Type
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("DeliveryType")%>
                                            </span>
                                        </td>
                                        <td>First Check Required?	
                                        </td>
                                        <td>
                                            <span>
                                                <%# (Boolean.Parse(Eval("FirstCheckRequired").ToString())) ? "Yes": "No"%>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Transport By
                                        </td>
                                        <td>
                                            <%# (Convert.ToBoolean(Eval("TransportationByBabaji"))) ? "Babaji": "Customer"%>
                                        </td>
                                        <td>KAM
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("KAMUser")%>
                                            </span>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:FormView>
                        </fieldset>
                        <div>
                            <fieldset id="fldPaymentHistory" runat="server">
                            <legend>Vendor Payment Detail</legend>
                                <asp:GridView ID="gvPaymentHistory" runat="server" AutoGenerateColumns="False"
                                    CssClass="table" Width="99%" PagerStyle-CssClass="pgr"
                                    DataKeyNames="lId" DataSourceID="DataSourcePaymentHistory" CellPadding="4"
                                    AllowPaging="True" AllowSorting="True" PageSize="40">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Expense Type" DataField="ExpenseType" />
                                        <asp:BoundField HeaderText="Payment Type" DataField="PaymentType" />
                                        <asp:BoundField HeaderText="Amount" DataField="PaidAmount" />
                                        <asp:BoundField HeaderText="Instrument No" DataField="InstrumentNo" />
                                        <asp:BoundField HeaderText="Instrument Date" DataField="InstrumentDate" DataFormatString="{0:dd/MM/yyyy}"/>
                                        <asp:BoundField HeaderText="User" DataField="UserName" />
                                        <asp:BoundField HeaderText="Date" DataField="updDate" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" />
                                        </Columns>
                                </asp:GridView>
                            </fieldset>
                            <fieldset>
                        <legend>Additional Expense</legend>    
                        <div id="content" style="overflow:scroll; width:75%">
                        <asp:GridView ID="gvExpenseDetails" runat="server" AutoGenerateColumns="False" CssClass="table"
                            Width="98%" PagerStyle-CssClass="pgr" DataKeyNames="lid" CellPadding="4" PageSize="80" 
                            PagerSettings-Position="TopAndBottom" AllowPaging="true" 
                            DataSourceID="DataJobExpenseDetails">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl" >
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex +1%>
                                    </ItemTemplate>
                                    </asp:TemplateField>
                                <asp:BoundField DataField="ExpenseName" HeaderText="Expense Type" />
                                <asp:BoundField DataField="Amount" HeaderText="Amount" />
                                <asp:BoundField DataField="PaidTo" HeaderText="Paid To" />
                                <asp:BoundField DataField="ReceiptType" HeaderText="Receipt" />
                                <asp:BoundField DataField="PaymentTypeName" HeaderText="Payment Type" />
                                <asp:BoundField DataField="ReceiptNo" HeaderText="Receipt No" />
                                <asp:BoundField DataField="ReceiptAmount" HeaderText="Receipt Amt" />
                                <asp:BoundField DataField="ReceiptDate" HeaderText="Receipt Date" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="BillableNoBillable" HeaderText="Billable ?" />
                                <asp:BoundField DataField="StatusName" HeaderText="Status" />
                                <asp:BoundField DataField="StatusDate" HeaderText="Status Date" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="StatusUser" HeaderText="Audit By" />
                                <asp:BoundField DataField="Remark" HeaderText="User Remark" />
                                <asp:BoundField DataField="UserName" HeaderText="Created By" />
                                <asp:BoundField DataField="dtDate" HeaderText="Creation Date" DataFormatString="{0:dd/MM/yyyy}" />               
                            </Columns>
                        </asp:GridView>
                    </div>
                        </fieldset>
                           <fieldset>
                                <legend>Consolidate Job Detail</legend>
                               <asp:HiddenField ID="hdnTransReqId" runat="server" Value="0" />
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
                                            <asp:ControlParameter ControlID="hdnTransReqId" PropertyName="Value" Name="TransReqId" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                            </fieldset>
                            <fieldset>
                                <legend>Vehicle Rate Detail</legend>
                                <div style="width: 1350px; overflow-x: scroll">
                                    <asp:GridView ID="GridViewVehicle" runat="server" AutoGenerateColumns="False" CssClass="table"
                                        Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                                        DataSourceID="DataSourceRate" CellPadding="4" AllowPaging="True" AllowSorting="True" ShowFooter="false"
                                        PageSize="20" FooterStyle-ForeColor="Black" FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False"
                                        FooterStyle-BackColor="#CCCCFF">
                                        <%----%>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sl">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="TransporterName" HeaderText="Transporter" SortExpression="TransporterName" ReadOnly="true" />
                                            <asp:BoundField DataField="TransitType" HeaderText="Delivery To" ReadOnly="true" />
                                            <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No" SortExpression="VehicleNo" ReadOnly="true" />
                                            <asp:BoundField DataField="VehicleTypeName" HeaderText="Vehicle Type" SortExpression="VehicleTypeName" ReadOnly="true" />
                                            <asp:BoundField DataField="LRNo" HeaderText="LRNo" SortExpression="LRNo" ReadOnly="true" />
                                            <asp:BoundField DataField="LRDate" HeaderText="LRDate" DataFormatString="{0:dd/MM/yyyy}" SortExpression="LRDate" ReadOnly="true" />
                                            <asp:BoundField DataField="ChallanNo" HeaderText="ChallanNo" SortExpression="ChallanNo" ReadOnly="true" />
                                            <asp:BoundField DataField="ChallanDate" HeaderText="Challan Date" SortExpression="ChallanDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="Rate" HeaderText="Freight Rate" SortExpression="Rate" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="Advance" HeaderText="Adv (%)" SortExpression="Advance" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="AdvanceAmount" HeaderText="Adv Amt" SortExpression="AdvanceAmount" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                            <%--<asp:BoundField DataField="MarketBillingRate" HeaderText="Market Billing Rate" SortExpression="MarketBillingRate" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />--%>
                                            <asp:BoundField DataField="FreightAmount" HeaderText="Freight Amt" SortExpression="FreightAmount" ReadOnly="true" Visible="false" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="DetentionAmount" HeaderText="Detention" SortExpression="DetentionAmount" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="VaraiExpense" HeaderText="Varai" SortExpression="VaraiExpense" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="EmptyContRecptCharges" HeaderText="Empty" SortExpression="EmptyContRecptCharges" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="TollCharges" HeaderText="Toll" SortExpression="TollCharges" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="OtherCharges" HeaderText="Other" SortExpression="OtherCharges" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="ContractPrice" HeaderText="Contract Price" SortExpression="ContractPrice" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="SellingPrice" HeaderText="Selling Price" SortExpression="SellingPrice" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                            <%--<asp:BoundField DataField="Instruction" HeaderText="Billing Intruction" SortExpression="Instruction" ReadOnly="true" />--%>
                                            <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy" ReadOnly="true" />
                                            <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" SortExpression="CreatedDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="UpdatedBy" HeaderText="Updated By" SortExpression="UpdatedBy" ReadOnly="true" />
                                            <asp:BoundField DataField="UpdatedDate" HeaderText="Updated Date" SortExpression="UpdatedDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </fieldset>
                                                        
                            <fieldset>
                                <legend>Selling Rate Detail</legend>
                                <div style="width: 1350px; overflow-x: scroll">
                                    <asp:GridView ID="gvSellDetail" runat="server" AutoGenerateColumns="false" CssClass="table"
                                        Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                                         DataSourceID="DataSourceSellingRate" FooterStyle-CssClass="table"
                                         OnRowCommand="gvSellDetail_RowCommand" autopostback="true">
                                        <Columns>
                                            <asp:BoundField DataField="TransporterName" HeaderText="Transporter" SortExpression="TransporterName" ReadOnly="true" />
                                            <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle No" ReadOnly="true" />
                                            <asp:BoundField DataField="DetentionAmount" HeaderText="Detention Amt" ReadOnly="true" />
                                            <asp:BoundField DataField="VaraiExpense" HeaderText="Varai Amt" ReadOnly="true" />
                                            <asp:BoundField DataField="EmptyContRecptCharges" HeaderText="Empty Amt" ReadOnly="true" />
                                            <asp:BoundField DataField="TollCharges" HeaderText="Toll" ReadOnly="true" />
                                            <asp:BoundField DataField="OtherCharges" HeaderText="Other Amt" ReadOnly="true" />
                                            <asp:BoundField DataField="SellFreighRate" HeaderText="Selling Freight rate" ReadOnly="true" />
                                            <asp:TemplateField HeaderText="Email Approval">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnEmailApprovalCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download Email Approval copy."
                                                        CommandName="EmailApprovalCopy" CommandArgument='<%#Eval("EmailAttachment")%>' Visible='<%# DecideHere((string)Eval("EmailAttachment")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Contract">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnContractCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download contract copy."
                                                        CommandName="ContractCopy" CommandArgument='<%#Eval("ContractAttachment")%>' Visible='<%# DecideHere((string)Eval("ContractAttachment")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Det Copy">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnDetentionCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download detention copy."
                                                        CommandName="DetentionCopy" CommandArgument='<%#Eval("DetentionDoc")%>' Visible='<%# DecideHereImg((string)Eval("DetentionDoc")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Varai">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnVaraiCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download varai copy."
                                                        CommandName="varaiCopy" CommandArgument='<%#Eval("VaraiDoc")%>' Visible='<%# DecideHereImg((string)Eval("VaraiDoc")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Empty">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnemptyContCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download Empty cont copy."
                                                        CommandName="EmptyContCopy" CommandArgument='<%#Eval("EmptyContDoc")%>' Visible='<%# DecideHereImg((string)Eval("EmptyContDoc")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Toll">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnTollCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download toll copy."
                                                        CommandName="TollCopy" CommandArgument='<%#Eval("TollDoc")%>' Visible='<%# DecideHereImg((string)Eval("TollDoc")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Other Copy">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnOtherCopy" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Download other copy."
                                                        CommandName="OtherCopy" CommandArgument='<%#Eval("OtherDoc")%>' Visible='<%# DecideHereImg((string)Eval("OtherDoc")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:BoundField DataField="Instruction" HeaderText="Billing Instruction" ReadOnly="true" />--%>
                                            <asp:BoundField DataField="Remark" HeaderText="Other Remark" ReadOnly="true" />
                                            <asp:BoundField DataField="SellDetail" HeaderText="Charge to Party" ReadOnly="true" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </fieldset>

                            <fieldset>
                            <legend>All Document</legend>
                            <asp:GridView ID="gvPCDDocument" runat="server" AutoGenerateColumns="False" CssClass="table"
                                Width="100%" PagerStyle-CssClass="pgr" DataKeyNames="lid" CellPadding="4" AllowPaging="True"
                                AllowSorting="True" PageSize="20" DataSourceID="PCDDocumentSqlDataSource" OnRowDataBound="gvPCDDocument_RowDataBound"
                                OnRowCommand="gvPCDDocument_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DocumentName" HeaderText="All Document" />
                                    <%--<asp:BoundField DataField="PCDToCustomer" HeaderText="PCD To Customer" />
                                    <asp:BoundField DataField="PCDToScrutiny" HeaderText="Scrutiny" Visible="false" />--%>
                                    <asp:BoundField DataField="UserName" HeaderText="Uploaded By" />
                                    <asp:TemplateField HeaderText="Download">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"
                                                CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </fieldset>

                            <fieldset>
                                <legend>Billing Advice Document</legend>
                                <asp:GridView ID="GridViewBillingAdvice" runat="server" AutoGenerateColumns="False"
                                    CssClass="table" Width="100%" PagerStyle-CssClass="pgr" DataKeyNames="lid" CellPadding="4"
                                    AllowPaging="True" AllowSorting="True" PageSize="20" DataSourceID="BillingAdviceSqlDataSource">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DocumentName" HeaderText="Billing Advice" SortExpression="DocumentName" />
                                        <asp:BoundField DataField="IsCopy" HeaderText="Copy" SortExpression="IsCopy" />
                                        <asp:BoundField DataField="IsOriginal" HeaderText="Original" SortExpression="IsOriginal" />
                                        <asp:BoundField DataField="CreatedBy" HeaderText="User Name" SortExpression="CreatedBy" />
                                    </Columns>
                                </asp:GridView>
                            </fieldset>

                            <fieldset>
                            <legend>PCA Document</legend>
                            <asp:GridView ID="GridViewPCADoc" runat="server" AutoGenerateColumns="False" CssClass="table"
                                Width="100%" PagerStyle-CssClass="pgr" DataKeyNames="lid" CellPadding="4" AllowPaging="True"
                                AllowSorting="True" PageSize="20" DataSourceID="PCADocSqlDataSource">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DocumentName" HeaderText="PCA" SortExpression="DocumentName" />
                                    <asp:BoundField DataField="IsCopy" HeaderText="Copy" SortExpression="IsCopy" />
                                    <asp:BoundField DataField="IsOriginal" HeaderText="Original" SortExpression="IsOriginal" />
                                    <asp:BoundField DataField="CreatedBy" HeaderText="User Name" SortExpression="CreatedBy" />
                                </Columns>
                            </asp:GridView>
                        </fieldset>

                            <fieldset>
                                <legend>Billing Instruction</legend>
                                <div id="dvResult" runat="server" style="max-height: 550px; overflow: auto; text-align: center;">
                                    <br />
                                    <div align="center">
                                        <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%" align="center" style="text-align: left;">
                                            <tr>
                                                <td><b>Allied Service</b></td>
                                                <td>
                                                    <div style="word-wrap: break-word; width: 200px; white-space:normal;">
                                                    <asp:Label ID="lblAlliedAgencyService" runat="server" Text='<%# Bind("AlliedAgencyService") %>'></asp:Label>
                                                        </div>
                                                </td>
                                                <td><b>Allied Remark</b></td>
                                                <td>
                                                    <div style="word-wrap: break-word; width: 200px; white-space:normal;">
                                                    <asp:Label ID="lblAlliedAgencyRemark" runat="server" Text='<%# Bind("AlliedAgencyRemark") %>'></asp:Label>
                                                        </div>
                                                </td>
                                                <td>
                                                    <b>Read By & Date</b>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblAlliedReadBy" runat="server"></asp:Label>
                                                </td>
                                                <td><b>Charge By & Date</b></td>
                                                <td>
                                                    <asp:Label ID="lblAlliedChargeBy" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>Other Service</b></td>
                                                <td>
                                                    <div style="word-wrap: break-word; width: 200px; white-space:normal;">
                                                    <asp:Label ID="lblOtherService" runat="server" ></asp:Label>
                                                        </div>
                                                </td>
                                                <td><b>Other Service remark</b></td>
                                                <td>
                                                    <div style="word-wrap: break-word; width: 200px; white-space:normal;">
                                                    <asp:Label ID="lblOtherServiceRemark" runat="server"></asp:Label>
                                                        </div>
                                                </td>
                                                <td>
                                                    <b>Read By & Date</b>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblOtherServiceReadBy" runat="server"></asp:Label>
                                                </td>
                                                <td><b>Charge By & Date</b></td>
                                                <td>
                                                    <asp:Label ID="lblOtherServiceChargeBy" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>Other Instruction</b></td>
                                                <td>
                                                    <div style="word-wrap: break-word; width: 200px; white-space:normal;">
                                                    <asp:Label ID="lblInstruction" runat="server"></asp:Label>
                                                    </div>
                                                </td>
                                                <td><b>Instruction Copy</b></td>
                                                <td>
                                                    <div style="word-wrap: break-word; width: 200px; white-space:normal;">
                                                    <asp:LinkButton ID="lnkInstructionCopy" runat="server" OnClick="lnkInstructionCopy_Click"></asp:LinkButton>
                                                        <asp:HiddenField id="hdnInstructionCopy" runat="server"/>
                                                    </div>
                                                </td>
                                                <td>
                                                    <b>Read By & Date</b>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblReadBy1" runat="server"></asp:Label>
                                                </td>
                                                <td><b>Charge By & Date</b></td>
                                                <td>
                                                    <asp:Label ID="lblChargeBy1" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>Other Instruction</b></td>
                                                <td>
                                                    <div style="word-wrap: break-word; width: 200px; white-space:normal;">
                                                    <asp:Label ID="lblInstruction1" runat="server"></asp:Label>
                                                    </div>
                                                </td>
                                                <td><b>Instruction Copy</b></td>
                                                <td>
                                                    <div style="word-wrap: break-word; width: 200px; white-space:normal;">
                                                    <asp:LinkButton ID="lnkInstructionCopy1" runat="server" OnClick="lnkInstructionCopy1_Click"></asp:LinkButton>
                                                        <asp:HiddenField id="hdnInstructionCopy1" runat="server"/>
                                                </div>
                                                    </td>
                                                <td>
                                                    <b>Read By & Date</b>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblReadBy2" runat="server"></asp:Label>
                                                </td>
                                                <td><b>Charge By & Date</b></td>
                                                <td>
                                                    <asp:Label ID="lblChargeBy2" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>Other Instruction</b></td>
                                                <td>
                                                    <div style="word-wrap: break-word; width: 200px; white-space:normal;">
                                                    <asp:Label ID="lblInstruction2" runat="server"></asp:Label>
                                                </div>
                                                </td>
                                                <td><b>Instruction Copy</b></td>
                                                <td>
                                                    <div style="word-wrap: break-word; width: 200px; white-space:normal;">
                                                    <asp:LinkButton ID="lnkInstructionCopy2" runat="server" OnClick="lnkInstructionCopy2_Click"></asp:LinkButton>
                                                        <asp:HiddenField id="hdnInstructionCopy2" runat="server" />
                                                    </div>
                                                    </td>
                                                <td>
                                                    <b>Read By & Date</b>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblReadBy3" runat="server"></asp:Label>
                                                </td>
                                                <td><b>Charge By & Date</b></td>
                                                <td>
                                                    <asp:Label ID="lblChargeBy3" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>Other Instruction</b></td>
                                                <td>
                                                    <div style="word-wrap: break-word; width: 200px; white-space:normal;">
                                                    <asp:Label ID="lblInstruction3" runat="server"></asp:Label>
                                                    </div>
                                                </td>
                                                <td><b>Instruction Copy</b></td>
                                                <td>
                                                    <div style="word-wrap: break-word; width: 200px; white-space:normal;">
                                                    <asp:LinkButton ID="lnkInstructionCopy3" runat="server" OnClick="lnkInstructionCopy3_Click"></asp:LinkButton>
                                                        <asp:HiddenField id="hdnInstructionCopy3" runat="server"/>
                                                    </div>
                                                </td>
                                                <td>
                                                    <b>Read By & Date</b>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblReadBy4" runat="server"></asp:Label>
                                                </td>
                                                <td><b>Charge By & Date</b></td>
                                                <td>
                                                    <asp:Label ID="lblChargeBy4" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>User Name</b></td>
                                                <td>
                                                    <asp:Label ID="lblUserId" runat="server"></asp:Label>
                                                </td>
                                                <td><b>User Date</b></td>
                                                <td>
                                                    <asp:Label ID="lblUserDate" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    </div>
                            </fieldset>

                            
                            <%-- Start Added by Swamini 27 Dec 2022 --%>
                            <fieldset>
                                <legend>Billing User Details</legend>
                                <asp:GridView ID="grdbillinguser" runat="server" AutoGenerateColumns="False"
                                    CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceBillingUser"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Charge Name" DataField="Particulars" />
                                        <asp:BoundField HeaderText="UOM" DataField="UOM" />
                                        <asp:BoundField HeaderText="Condition" DataField="Condition" />
                                        <asp:BoundField HeaderText="Qty" DataField="Qty" />
                                        <asp:BoundField HeaderText="Y/N" DataField="chkstatus" />
                                        <asp:BoundField HeaderText="User Name" DataField="username" />
                                        <asp:BoundField HeaderText="Date" DataField="date1" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    </Columns>
                                </asp:GridView>
                            </fieldset>
                            <%-- End --%>

                            <fieldset>
                                <legend>Billing Scrutiny</legend>
                                <asp:GridView ID="gvbillingscrutiny" runat="server" AutoGenerateColumns="False"
                                    CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceBillingScrutiny"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
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
                                    </Columns>
                                </asp:GridView>
                            </fieldset>

                            <fieldset>
                                <legend>Draft Invoice</legend>
                                <asp:Button ID="btnCheckBJVInvoice" Text="Check Draft Status" runat="server" OnClick="btnCheckBJVInvoice_Click" />
                                <asp:GridView ID="gvDraftInvoice" runat="server" AutoGenerateColumns="False"
                                    CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceDraftinvoice"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
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
                                    </Columns>
                                </asp:GridView>
                            </fieldset>

                            <fieldset>
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

                            <fieldset>
                                <legend>Final Invoice Typing</legend>
                                <asp:GridView ID="gvFinaltyping" runat="server" AutoGenerateColumns="False"
                                    CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceFinalTyping"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
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
                                    </Columns>
                                </asp:GridView>
                            </fieldset>

                            <fieldset>
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

                            <fieldset>
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

                            <fieldset>
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

                            <div id="div1">
                                <asp:SqlDataSource ID="DataSourceBillingScrutiny" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetBillingScrutinyById" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                
                                <%-- Start Added by Swamini 27 Dec 2022 --%>
                                <asp:SqlDataSource ID="DataSourceBillingUser" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetBillingUserById" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                        <asp:SessionParameter Name="username" SessionField="username" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                <%-- End --%>

                                <asp:SqlDataSource ID="DataSourceDraftinvoice" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetDraftInvoiceById" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                                <asp:SqlDataSource ID="DataSourceDraftCheck" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetDraftCheckById" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                                <asp:SqlDataSource ID="DataSourceFinalTyping" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetFinalTypingById" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                                <asp:SqlDataSource ID="DataSourceFinalCheck" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetFinalCheckById" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                                <asp:SqlDataSource ID="DataSourceBillDispatch" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetBillDispatchById" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                                <asp:SqlDataSource ID="DataSourceBillRejection" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetBillRejectionById" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                                <asp:SqlDataSource ID="DataSourcePaymentHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="AC_GetInvoicePaymentByJobId" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                
                                <asp:SqlDataSource ID="PCADocSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetPCDDocumentByWorkFlow" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                        <asp:Parameter Name="DocumentForType" DefaultValue="2" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                                <asp:SqlDataSource ID="BillingAdviceSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetPCDDocumentByWorkFlow" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                        <asp:Parameter Name="DocumentForType" DefaultValue="3" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                                <asp:SqlDataSource ID="PCDDocumentSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetUploadedPCDDocument" SelectCommandType="StoredProcedure" OnSelected="PCDDocumentSqlDataSource_Selected">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                <asp:SqlDataSource ID="DataSourceRate" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetTransDetail" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                <asp:SqlDataSource ID="DataSourceSellingRate" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetSellTransDetail" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                <asp:SqlDataSource ID="DataJobExpenseDetails" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetAdditionalExpenseByJobId" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>
                        </div>

                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
                <AjaxToolkit:TabPanel ID="TabDocument" runat="server" HeaderText="Document">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Job Document</legend>
                            <asp:GridView ID="GridViewDocument" runat="server" AutoGenerateColumns="False" CssClass="table"
                                Width="100%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DocumentSqlDataSource"
                                OnRowCommand="GridViewDocument_RowCommand" CellPadding="4" AllowPaging="True"
                                AllowSorting="True" PageSize="20">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="lid" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbllid" runat="server" Text='<%#Eval("lid") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Document Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDocType" runat="server" Text='<%#Eval("DocumentName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField DataField="FileName" HeaderText="Document" />--%>
                                    <%--<asp:BoundField DataField="DocName" HeaderText="Dcument Type" />--%>                                   
                                    <%--<asp:BoundField DataField="DocDate" HeaderText="Date" />--%>
                                    <asp:BoundField DataField="sName" HeaderText="Uploaded By" />
                                    <asp:TemplateField HeaderText="Download">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" Text="Download" CommandName="Download"
                                                CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="View">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkView" runat="server" Text="View" CommandName="View" 
                                                CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <br />
                            <br />
                        </fieldset>
                        <fieldset>
                    <legend>Vendor Invoice Document</legend>
                    <asp:GridView ID="gvInvoiceDocument" runat="server" CssClass="table" AutoGenerateColumns="false"
                        PagerStyle-CssClass="pgr" DataKeyNames="InvoiceID" AllowPaging="True" AllowSorting="True" PageSize="20"
                        DataSourceID="DataSourcePaymentDoument" OnRowCommand="gvInvoiceDocument_RowCommand" PagerSettings-Position="TopAndBottom" Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" SortExpression="InvoiceNo"/>
                            <asp:BoundField DataField="ExpenseType" HeaderText="Type" SortExpression="ExpenseType"/>
                            <asp:BoundField DataField="VendorName" HeaderText="Vendor" SortExpression="VendorName"/>
                            <asp:BoundField DataField="DocumentName" HeaderText="Document" SortExpression="DocumentName"/>
                            <asp:BoundField DataField="InvoiceTypeName" HeaderText="Tax/Proforma" SortExpression="InvoiceTypeName"/>
                            <asp:TemplateField HeaderText="Download">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton21" runat="server" Text="Download" CommandName="Download"
                                        CommandArgument='<%#Eval("lid") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="View">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton3" runat="server" Text="View" CommandName="View" 
                                        CommandArgument='<%#Eval("lid") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView> 
                </fieldset>
                        <div>
                            <asp:SqlDataSource ID="DocumentSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="GetUploadedDocument" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:SqlDataSource ID="DataSourcePaymentDoument" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="AC_GetInvoiceDocumentBYJobId" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
                <AjaxToolkit:TabPanel ID="TabContainer" runat="server" HeaderText="Container">
                <ContentTemplate>
                    <div>
                        <fieldset>
                            <legend>Container Detail</legend>
                            <asp:GridView ID="gvContainer" runat="server" AllowPaging="true" CssClass="table"
                                PagerStyle-CssClass="pgr" AutoGenerateColumns="false" DataKeyNames="lid" Width="100%"
                                PageSize="20" DataSourceID="DataSourceContainer">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ContainerNo" HeaderText="Container No" />
                                    <asp:BoundField DataField="ContainerTypeName" HeaderText="Container Type" />
                                    <asp:BoundField DataField="ContainerSizeName" HeaderText="Container Size" />
                                </Columns>
                            </asp:GridView>
                        </fieldset>
                    </div>
                    <div>
                        <asp:SqlDataSource ID="DataSourceContainer" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="GetContainerDetail" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="JobId" SessionField="JobId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                </ContentTemplate>
            </AjaxToolkit:TabPanel>
                <%--end Billing--%>
            </AjaxToolkit:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


