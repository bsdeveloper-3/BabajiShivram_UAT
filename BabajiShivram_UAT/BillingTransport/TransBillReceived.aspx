<%@ Page Title="Bill Received" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="TransBillReceived.aspx.cs" Inherits="BillingTransport_TransBillReceived" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlTPBill" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <style type="text/css">
        .heading {
            line-height: 20px;
        }

        .modalBackground {
            background-color: black;
            filter: alpha(opacity=90);
            opacity: 0.5;
        }

        .modalPopup {
            border-radius: 5px;
            background-color: #FFFFFF;
            border-width: 1px;
            border-style: solid;
            border-color: black;
            padding-top: 5px;
            padding-left: 3px;
            width: 300px;
            height: 140px;
        }
    </style>
    <script type="text/javascript">
        function ActiveTabChanged12() {
            /* Clear Error Message on Tab change Event */
            document.getElementById('<%=lblMessage.ClientID%>').innerHTML = '';
            document.getElementById('<%=lblMessage.ClientID%>').className = '';
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
    <asp:UpdatePanel ID="upnlTPBill" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="vsReqStatus" runat="server" ShowMessageBox="True"
                ShowSummary="False" ValidationGroup="ReqStatus" CssClass="errorMsg" />
            <div align="center">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </div>
            <ajaxtoolkit:tabcontainer runat="server" id="TabRequestRecd" activetabindex="0" cssclass="Tab"
                width="100%" onclientactivetabchanged="ActiveTabChanged12" autopostback="true">
                <AjaxToolkit:TabPanel runat="server" ID="TabPanelNormalJob" TabIndex="0" HeaderText="Non-Receive">
                    <ContentTemplate>
                        <div style="overflow: auto;">
                            <div class="m clear">
                                <div align="center">
                                    <asp:Label ID="lblError_Job" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="m clear">
                                <asp:Panel ID="pnlFilter" runat="server">
                                    <div class="fleft">
                                        <uc1:DataFilter ID="DataFilter1" runat="server" />
                                    </div>
                                </asp:Panel>
                                <asp:Button ID="btnReceive" runat="server" Text="Receive" OnClick="btnReceive_Click" />
                                <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click">
                                    <asp:Image ID="Image2" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                                </asp:LinkButton>
                            </div>
                            <div class="clear"></div>
                            <asp:GridView ID="gvNonReceiveBill" runat="server" AutoGenerateColumns="False" CssClass="table"
                                Width="100%" DataKeyNames="TransBillId" DataSourceID="DataSourceBillNonReceive" 
                                CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20">
                                <AlternatingRowStyle CssClass="alt" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ref No" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkSelect" CommandName="select" runat="server" Text='<%# Bind("TRRefNo")%>' CommandArgument='<%#Eval("TransReqId") + ";" + Eval("ConsolidateID") + ";" + Eval("TransBillId") + ";" + Eval("TransporterID") %>'
                                                ToolTip="Show Transport Bill Detail"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Job No" DataField="JobRefNo" />
                                    <asp:BoundField HeaderText="Transporter" DataField="Transporter" />
                                    <asp:BoundField HeaderText="Bill Number" DataField="BillNumber" />
                                    <asp:BoundField HeaderText="Bill Submit Date" DataField="BillSubmitDate" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField HeaderText="Bill Date" DataField="BillDate" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField HeaderText="Bill Amount" DataField="BillAmount" />
                                    <asp:BoundField HeaderText="Detention" DataField="DetentionAmount" />
                                    <asp:BoundField HeaderText="Varai" DataField="VaraiAmount" />
                                    <asp:BoundField HeaderText="Empty Cont Charges" DataField="EmptyContRcptCharges" />
                                    <asp:BoundField HeaderText="Toll Charges" DataField="TollCharges" />
                                    <asp:BoundField HeaderText="Other Charges" DataField="OtherCharges" />
                                    <asp:BoundField HeaderText="Total" DataField="TotalAmount" />
                                    <asp:BoundField HeaderText="Billing Person" DataField="BillPersonName" />
                                    <asp:BoundField HeaderText="Created Date" DataField="BillCreatedOn" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField HeaderText="Created By" DataField="BillCreatedBy" />
                                    <asp:BoundField HeaderText="Sent User" DataField="SentUser" />
                                    <asp:BoundField HeaderText="Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy}" />
                                </Columns>
                                <PagerStyle CssClass="pgr" />
                            </asp:GridView>
                            <div>
                                <asp:SqlDataSource ID="DataSourceBillNonReceive" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="TR_GetBillNonReceived" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>
                        </div>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
                <AjaxToolkit:TabPanel ID="TabPanelConsolidateJob" runat="server" TabIndex="1" HeaderText="Receive">
                    <ContentTemplate>
                        <div style="overflow: auto;">
                            <div class="m clear">
                                <div align="center">
                                    <asp:Label ID="lblError_ConsolidateJob" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="m clear">
                                <asp:Panel ID="pnlFilter2" runat="server">
                                    <div class="fleft">
                                        <uc1:DataFilter ID="DataFilter2" runat="server" />
                                    </div>
                                    <div class="fleft" style="margin-left: 30px; padding-top: 3px;">
                                        <asp:LinkButton ID="lnkExport_Consolidate" runat="server" OnClick="lnkExport_Consolidate_Click">
                                            <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                                        </asp:LinkButton>
                                    </div>
                                </asp:Panel>
                            </div>
                            <div class="m clear"></div>
                            <asp:GridView ID="gvReceiveBill" runat="server" AutoGenerateColumns="False" CssClass="table" autopostback="true"
                                Width="1500px" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="TransBillId"
                                DataSourceID="DataSourceBillReceive" CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20"
                                OnRowCommand="gvReceiveBill_RowCommand" OnRowDataBound="gvReceiveBill_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Update detail" ItemStyle-Width="80px" Visible="false">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkUpdateBill" CommandName="UpdateDetail" runat="server" Text="Update" CommandArgument='<%#Eval("TransBillId")%>'
                                                ToolTip="Click To Update Received Bill Detail"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ref No" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkSelect" CommandName="select" runat="server" Text='<%# Bind("TRRefNo")%>' CommandArgument='<%#Eval("TransReqId") + ";" + Eval("ConsolidateID") + ";" + Eval("TransBillId") + ";" + Eval("TransporterID") %>'
                                                ToolTip="Show Transport Bill Detail"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Ref No" DataField="TRRefNo" Visible="false" />
                                    <asp:BoundField HeaderText="Job No" DataField="JobRefNo" ItemStyle-Width="120px" />
                                    <asp:BoundField HeaderText="Bill Status" DataField="BillStatus" />
                                    <asp:BoundField HeaderText="Transporter" DataField="Transporter" />
                                    <asp:BoundField HeaderText="Bill Number" DataField="BillNumber" />
                                    <asp:BoundField HeaderText="Bill Submit Date" DataField="BillSubmitDate" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField HeaderText="Bill Date" DataField="BillDate" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField HeaderText="Bill Amount" DataField="BillAmount" />
                                    <%--<asp:BoundField HeaderText="Detention" DataField="DetentionAmount" />
                                    <asp:BoundField HeaderText="Varai" DataField="VaraiAmount" />
                                    <asp:BoundField HeaderText="Empty Cont Charges" DataField="EmptyContRcptCharges" />
                                    <asp:BoundField HeaderText="Toll Charges" DataField="TollCharges" />
                                    <asp:BoundField HeaderText="Other Charges" DataField="OtherCharges" />--%>
                                    <asp:BoundField HeaderText="Total" DataField="TotalAmount" />
                                    <asp:BoundField HeaderText="Billing Person" DataField="BillPersonName" />
                                    <asp:BoundField HeaderText="Created Date" DataField="BillCreatedOn" DataFormatString="{0:dd/MM/yyyy}" Visible="false" />
                                    <asp:BoundField HeaderText="Created By" DataField="BillCreatedBy" Visible="false" />
                                    <asp:BoundField HeaderText="Sent User" DataField="SentUser" />
                                    <asp:BoundField HeaderText="Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy}" />
                                </Columns>
                            </asp:GridView>
                            <div>
                                <asp:SqlDataSource ID="DataSourceBillReceive" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="TR_GetBillReceived" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>
                        </div>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
            </ajaxtoolkit:tabcontainer>

            <%-- MODAL POPUP FOR RECEIVED BILLS --%>
            <div>
                <asp:HiddenField ID="hdnReceiveBill" runat="server" />
                <ajaxtoolkit:modalpopupextender id="mpeReceiveBill" runat="server" targetcontrolid="hdnReceiveBill" backgroundcssclass="modalBackground"
                    popupcontrolid="pnlReceiveBill" dropshadow="true">
                </ajaxtoolkit:modalpopupextender>
                <asp:Panel ID="pnlReceiveBill" runat="server" CssClass="modalPopup1" BackColor="#F5F5DC" Width="900px" Height="380px" Style="border: 1px solid #cccc; border-radius: 5px">
                    <div id="div2" runat="server">
                        <br />
                        <table width="100%">
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <b><u>Update Bill Detail</u></b>
                                    <span style="float: right">
                                        <asp:ImageButton ID="imgbtnClose" ImageUrl="~/Images/delete.gif" runat="server" OnClick="imgbtnClose_Click" ToolTip="Close" />
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblError_Popup" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <asp:Panel ID="pnlReceiveBill2" runat="server" Width="880px" Height="350px" Style="padding: 3px">
                            <table border="0" cellpadding="0" cellspacing="0" width="99%" bgcolor="white">
                                <tr>
                                    <td>TR Ref No
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTRRefNo" runat="server" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td>Job No</td>
                                    <td>
                                        <asp:Label ID="lblJobNo" runat="server" Font-Bold="true"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Transporter</td>
                                    <td>
                                        <asp:Label ID="lblTransporter" runat="server"></asp:Label>
                                    </td>
                                    <td>Bill Number</td>
                                    <td>
                                        <asp:Label ID="lblBillNumber" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Bill Submit Date</td>
                                    <td>
                                        <asp:Label ID="lblBillSubmitDate" runat="server"></asp:Label>
                                    </td>
                                    <td>Bill Date</td>
                                    <td>
                                        <asp:Label ID="lblBillDate" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Bill Amount</td>
                                    <td>
                                        <asp:Label ID="lblBillAmt" runat="server"></asp:Label>
                                    </td>
                                    <td>Detention Amount</td>
                                    <td>
                                        <asp:Label ID="lblDetentionAmt" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Varai Amount</td>
                                    <td>
                                        <asp:Label ID="lblVaraiAmt" runat="server"></asp:Label>
                                    </td>
                                    <td>Empty Cont Retrun Charges</td>
                                    <td>
                                        <asp:Label ID="lblEmptyContCharges" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Total Amount</td>
                                    <td>
                                        <asp:Label ID="lblTotalAmt" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:HiddenField ID="hdnTransBillId" runat="server"></asp:HiddenField>
                                    </td>
                                    <td></td>
                                </tr>
                            </table>
                            <br />
                            <table border="0" cellpadding="0" cellspacing="0" width="99%" bgcolor="white">
                                <tr>
                                    <td>Status
                                        <asp:RequiredFieldValidator ID="rfvStatus" runat="server" ControlToValidate="ddlStatus" InitialValue="0" SetFocusOnError="true"
                                            Display="Dynamic" ErrorMessage="Please Select Status." ValidationGroup="ReqStatus"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlStatus" runat="server" DataSourceID="DataSourceStatus" DataTextField="sName" DataValueField="lid"
                                            AppendDataBoundItems="true" TabIndex="1" Width="160px" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="-Select-" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>Cheque No
                                        <asp:RequiredFieldValidator ID="rfvChequeNo" runat="server" ControlToValidate="txtChequeNo" SetFocusOnError="true"
                                            Display="Dynamic" ErrorMessage="Please Enter Cheque No." ValidationGroup="ReqStatus"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtChequeNo" runat="server" TabIndex="2" Width="150px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>Cheque Date
                                        <ajaxtoolkit:calendarextender id="calChequeDate" runat="server" firstdayofweek="Sunday" popupbuttonid="imgChequeDate"
                                            format="dd/MM/yyyy" popupposition="BottomRight" targetcontrolid="txtChequeDate">
                                        </ajaxtoolkit:calendarextender>
                                        <ajaxtoolkit:maskededitextender id="meeChequeDate" targetcontrolid="txtChequeDate" mask="99/99/9999" messagevalidatortip="true"
                                            masktype="Date" autocomplete="false" runat="server">
                                        </ajaxtoolkit:maskededitextender>
                                        <ajaxtoolkit:maskededitvalidator id="mevChequeDate" controlextender="meeChequeDate" controltovalidate="txtChequeDate" isvalidempty="true"
                                            invalidvaluemessage="Cheque Date is invalid" invalidvalueblurredmessage="Invalid Date" setfocusonerror="true"
                                            emptyvalueblurredtext="*" emptyvaluemessage="Please enter cheque date." minimumvaluemessage="Invalid Date"
                                            maximumvaluemessage="Invalid Date" minimumvalue="01/01/2015" runat="Server" validationgroup="ReqStatus"></ajaxtoolkit:maskededitvalidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtChequeDate" runat="server" TabIndex="3" Width="150px"></asp:TextBox>
                                        <asp:Image ID="imgChequeDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                    </td>
                                    <td>Release Date
                                        <ajaxtoolkit:calendarextender id="calReleaseDate" runat="server" firstdayofweek="Sunday" popupbuttonid="imgReleaseDate"
                                            format="dd/MM/yyyy" popupposition="BottomRight" targetcontrolid="txtReleaseDate">
                                        </ajaxtoolkit:calendarextender>
                                        <ajaxtoolkit:maskededitextender id="meeReleaseDate" targetcontrolid="txtReleaseDate" mask="99/99/9999" messagevalidatortip="true"
                                            masktype="Date" autocomplete="false" runat="server">
                                        </ajaxtoolkit:maskededitextender>
                                        <ajaxtoolkit:maskededitvalidator id="mevReleaseDate" controlextender="meeReleaseDate" controltovalidate="txtReleaseDate" isvalidempty="true"
                                            invalidvaluemessage="Release Date is invalid" invalidvalueblurredmessage="Invalid Date" setfocusonerror="true"
                                            emptyvalueblurredtext="*" emptyvaluemessage="Please enter release date." minimumvaluemessage="Invalid Date"
                                            maximumvaluemessage="Invalid Date" runat="Server" validationgroup="ReqStatus"></ajaxtoolkit:maskededitvalidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtReleaseDate" runat="server" TabIndex="3" Width="150px"></asp:TextBox>
                                        <asp:Image ID="imgReleaseDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Hold Reason
                                        <asp:RequiredFieldValidator ID="rfvHoldReason" runat="server" ControlToValidate="txtHoldReason" SetFocusOnError="true"
                                            Display="Dynamic" ErrorMessage="Please Enter Hold Reason." ValidationGroup="ReqStatus"></asp:RequiredFieldValidator>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtHoldReason" runat="server" TabIndex="4" Width="600px" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Button ID="btnSaveBill" runat="server" OnClick="btnSaveBill_Click" Text="Update Bill Detail" CausesValidation="true" ValidationGroup="ReqStatus" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                        </asp:Panel>
                        <asp:SqlDataSource ID="DataSourceStatus" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="TR_GetBillReceiveStatusMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                    </div>
                </asp:Panel>
            </div>
            <%-- MODAL POPUP FOR RECEIVE BILLS --%>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

