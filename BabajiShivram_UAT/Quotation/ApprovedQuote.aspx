<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ApprovedQuote.aspx.cs"
    Inherits="Quotation_ApprovedQuote" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" EnablePartialRendering="true" />

    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upApprovedQuotation" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <style type="text/css">
        .modalBackground {
            background-color: black;
            filter: alpha(opacity=90);
            opacity: 0.5;
        }
    </style>

    <asp:UpdatePanel ID="upApprovedQuotation" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:ValidationSummary ID="vsAddStatus" runat="server" ShowMessageBox="true" ShowSummary="false"
                    ValidationGroup="vgAddStatus" />
            </div>
            <fieldset>
                <legend>Add Draft Quotation</legend>

                <div id="dvBabajiQuote" runat="server">
                    <div id="dvDraftHeader" runat="server">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td style="border-right: none">Customer
                                </td>
                                <td>
                                    <asp:Label ID="lblCustomer" runat="server" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Quotation Ref No</td>
                                <td>
                                    <asp:Label ID="lblQuoteRefNo" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Quotation Format</td>
                                <td>
                                    <asp:Label ID="lblQuoteFormat" Text="Normal Quotation" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="border-right: none">Branch
                                </td>
                                <td>
                                    <asp:Label ID="lblBranch" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="border-right: none">Sales Person Name
                                </td>
                                <td>
                                    <asp:Label ID="lblSalesPerson" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="border-right: none">KAM
                                </td>
                                <td>
                                    <asp:Label ID="lblKAM" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Address Line 1
                                </td>
                                <td>
                                    <asp:Label ID="lblAddressLine1" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Address Line 2
                                </td>
                                <td>
                                    <asp:Label ID="lblAddressLine2" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Address Line 3
                                </td>
                                <td>
                                    <asp:Label ID="lblAddressLine3" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Kind Attention
                                </td>
                                <td>
                                    <asp:Label ID="lblKindAttn" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Subject 
                                </td>
                                <td>
                                    <asp:Label ID="lblSubject" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Payment Terms
                                </td>
                                <td>
                                    <asp:Label ID="lblTerms" runat="server" TabIndex="6" Width="56%"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Division</td>
                                <td>
                                    <asp:Label ID="lblDivision" runat="server" Width="70%"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Quote Generated For</td>
                                <td>
                                    <asp:Label ID="lblQuoteGeneratedFor" runat="server" Width="70%"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Select Mode</td>
                                <td>
                                    <asp:Label ID="lblMode" runat="server"></asp:Label>
                                </td>
                            </tr>

                        </table>

                    </div>
                    <fieldset>
                        <legend>Charges Applicable</legend>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td colspan="3" style="width: 55%">
                                    <div>
                                        <div style="padding: 5px; font-weight: 600">
                                            <asp:CheckBox ID="chkLumpSum" runat="server" Enabled="false" Visible="false" />
                                            <span style="text-align: right"></span>
                                            <asp:Label ID="lblTotal2" runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblMinTotal2" runat="server" Visible="false"></asp:Label>
                                        </div>
                                        <asp:UpdatePanel ID="upnlChargeGrid" runat="server">
                                            <ContentTemplate>
                                                <div>
                                                    <asp:GridView ID="gvGenerateCharge" runat="server" ShowFooter="True" AllowSorting="True" Style="border-collapse: initial; border: 1px solid #5D7B9D"
                                                        AutoGenerateColumns="False" Width="100%" CellPadding="4" OnRowDataBound="gvGenerateCharge_RowDataBound" ForeColor="#333333" GridLines="None" DataKeyNames="lid">
                                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Sl">
                                                                <ItemTemplate>
                                                                    <%#Container.DataItemIndex +1%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ChargeId" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblChargeId" runat="server" Text='<%#Eval("ChargeId") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-Width="1%">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkItemForLumpSum" runat="server" Enabled="false" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Particulars" ItemStyle-Width="55%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblParticulars" runat="server" Text='<%#Eval("Particulars") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Charges Applicable" ItemStyle-Width="40%">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtChargesApp" runat="server" Width="100px" Text='<%#Eval("ChargesAmt") %>'></asp:TextBox>
                                                                    <asp:DropDownList ID="ddlRanges" runat="server" AppendDataBoundItems="true" Width="250px">
                                                                        <asp:ListItem Selected="True" Value="0" Text="-Select-"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="LumpSum Amount" ItemStyle-Width="40%">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtLumpsumAmount" Width="100px" runat="server" Text='<%#Eval("LumpSumAmt") %>' AutoPostBack="true"></asp:TextBox>
                                                                    <asp:DropDownList ID="ddlRanges_LumpSum" runat="server" AppendDataBoundItems="true" Width="250px">
                                                                        <asp:ListItem Selected="True" Value="0" Text="-Select-"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="CategoryId" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCategoryId" runat="server" Text='<%#Eval("CategoryId") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EditRowStyle BackColor="#999999" />
                                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                                    </asp:GridView>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div>
                                    </div>
                                </td>
                            </tr>

                        </table>
                    </fieldset>
                    <fieldset id="fsTransportCharges" runat="server">
                        <legend>Transportation Charges
                        </legend>
                        <asp:GridView ID="gvTransportChg" runat="server" ShowFooter="false" PagerStyle-CssClass="pgr" CssClass="gridview"
                            AllowSorting="True" AutoGenerateColumns="False" AllowPaging="True" Style="white-space: normal; border-collapse: initial; border: 1px solid #5D7B9D"
                            OnPageIndexChanging="gvTransportChg_PageIndexChanging" PageSize="20" PagerSettings-Position="TopAndBottom" OnRowDataBound="gvTransportChg_OnDataBound"
                            DataKeyNames="lid" Width="100%" CellPadding="3" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                            <AlternatingRowStyle BackColor="White" ForeColor="#333333" />
                            <Columns>
                                <asp:TemplateField HeaderText="Sl" ItemStyle-Width="3%" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                    <ItemStyle Width="3%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Particulars" ItemStyle-Width="60%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblParticulars" runat="server" Text='<%#Eval("Particulars") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtParticulars" runat="server" Text='<%#Eval("Particulars") %>' Width="90%" TabIndex="1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvParticulars" runat="server" ControlToValidate="txtParticulars" SetFocusOnError="true" Display="Dynamic"
                                            ErrorMessage="Please Enter Particulars." Text="*" ForeColor="Red" ValidationGroup="vgEditTransp"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtParticulars_Footer" runat="server" Text='<%#Eval("Particulars") %>' Width="90%" TabIndex="1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvaddpart" runat="server" ControlToValidate="txtParticulars_Footer" SetFocusOnError="true" Display="Dynamic"
                                            ErrorMessage="Please Enter Particulars." Text="*" ForeColor="Red" ValidationGroup="vgAddTransp"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                    <ItemStyle Width="60%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Charges Applicable" ItemStyle-Width="67%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblChargesApplicable" runat="server" Text='<%#Eval("ChargesApplicable") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtChargesApplicable" runat="server" Text='<%#Eval("ChargesApplicable") %>' Width="90%" TabIndex="1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvChargesApplicable" runat="server" ControlToValidate="txtChargesApplicable" SetFocusOnError="true" Display="Dynamic"
                                            ErrorMessage="Please Enter Charges Applicable." Text="*" ForeColor="Red" ValidationGroup="vgEditTransp"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtChargesApplicable_Footer" runat="server" Text='<%#Eval("ChargesApplicable") %>' Width="90%" TabIndex="1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvaddChargesApplicable" runat="server" ControlToValidate="txtChargesApplicable_Footer" SetFocusOnError="true" Display="Dynamic"
                                            ErrorMessage="Please Enter Charges Applicable." Text="*" ForeColor="Red" ValidationGroup="vgAddTransp"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                    <ItemStyle Width="67%" />
                                </asp:TemplateField>

                            </Columns>
                            <PagerSettings Position="TopAndBottom" />
                            <PagerStyle CssClass="pgr" />
                            <EditRowStyle BackColor="#999999" />
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                        </asp:GridView>
                    </fieldset>
                </div>
                <div id="dvCustomerQuote" runat="server">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>Quotation Ref No</td>
                            <td>
                                <asp:Label ID="lblQuoteRefNo2" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Quotation Format</td>
                            <td>
                                <asp:Label ID="lblQuoteFormat2" Text="RFQ/Tender Quotation" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="border-right: none">Branch
                            </td>
                            <td>
                                <asp:Label ID="lblBranch2" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="border-right: none">Sales Person Name
                            </td>
                            <td>
                                <asp:Label ID="lblsalesRep_Cust" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="border-right: none">KAM
                            </td>
                            <td>
                                <asp:Label ID="lblKAM_cust" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="border-right: none">Customer                           
                            </td>
                            <td>
                                <asp:Label ID="lblTenderCustomer" runat="server" Width="70%" MaxLength="250"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Division                           
                            </td>
                            <td>
                                <asp:Label ID="lblDivision2" runat="server" Width="25%" MaxLength="250"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Notes</td>
                            <td>
                                <asp:Label ID="lblNotes" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <fieldset>
                        <legend>Documents</legend>
                        <div>
                            <div>
                                <asp:FileUpload ID="fuRFQDoc" runat="server" />
                                <asp:Button ID="btnSaveDoc" runat="server" OnClick="btnSaveDoc_OnClick" CausesValidation="false" Text="Save Document" />
                            </div>
                            <asp:GridView ID="gvRFQDocuments" runat="server" CssClass="table" AutoGenerateColumns="false"
                                PagerStyle-CssClass="pgr" DataKeyNames="lid" AllowPaging="True" AllowSorting="True" PageSize="20"
                                PagerSettings-Position="TopAndBottom" OnRowCommand="gvRFQDocuments_RowCommand" Width="50%"
                                DataSourceID="DataSourceRfqDocuments">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Document">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDownloadDoc" CommandName="download" ToolTip="Download document" runat="server"
                                                Text='<%#Eval("DocumentName") %>' CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDelete" CommandName="deletedoc" ToolTip="Delete document" runat="server"
                                                Text="Delete" CommandArgument='<%#Eval("lid") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div>
                            <asp:SqlDataSource ID="DataSourceRfqDocuments" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="BS_GetQuotationDocDetail" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="QuotationId" SessionField="QuotationId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </fieldset>
                </div>

            </fieldset>
            <asp:SqlDataSource ID="DataSourceStatusHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="BS_GetQuotationStatusHistory" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter Name="QuotationId" SessionField="QuotationId" DbType="Int32" />
                    <asp:SessionParameter Name="UserId" SessionField="UserId" DbType="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="DataSourceStatus" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="BS_GetQuotationStatus" SelectCommandType="StoredProcedure"></asp:SqlDataSource>

            <!--Document for Doc Upload-->
            <div id="divContractCopy">
                <AjaxToolkit:ModalPopupExtender ID="mpeContractCopy" runat="server" CacheDynamicResults="false"
                    PopupControlID="pnlContractCopy" TargetControlID="lnkbtnContractCopy" BackgroundCssClass="modalBackground" DropShadow="true">
                </AjaxToolkit:ModalPopupExtender>
                <asp:Panel ID="pnlContractCopy" runat="server" CssClass="ModalPopupPanel" Width="500px" Style="border-radius: 10px">
                    <div class="header">
                        <div class="fleft">
                            UPLOAD DOCUMENTS 
                        </div>
                        <div class="fright">
                            <asp:ImageButton ID="imgbtnContract" ImageUrl="~/Images/delete.gif" runat="server" OnClick="imgbtnContract_Click"
                                ToolTip="Close" />
                        </div>
                    </div>
                    <div class="m">
                    </div>
                    <!-- Lists Of All Documents -->
                    <div id="Div3" runat="server" style="max-height: 250px; overflow: auto; padding: 5px">
                        <div class="clear" style="text-align: center">
                            <asp:Label ID="lblErrorContract" runat="server" EnableViewState="false"></asp:Label>
                        </div>
                        <div>
                            <asp:GridView ID="gvContractCopy" runat="server" CssClass="table" AutoGenerateColumns="false"
                                PagerStyle-CssClass="pgr" DataKeyNames="lid" AllowPaging="True" AllowSorting="True" PageSize="20"
                                PagerSettings-Position="TopAndBottom" OnRowCommand="gvContractCopy_RowCommand" Width="100%"
                                DataSourceID="DataSourceContractDocuments">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Document">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDownloadDoc" CommandName="download" ToolTip="Download document" runat="server"
                                                Text='<%#Eval("DocumentName") %>' CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDelete" CommandName="deletedoc" ToolTip="Delete document" runat="server"
                                                Text="Delete" CommandArgument='<%#Eval("lid") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div>
                            <asp:SqlDataSource ID="DataSourceContractDocuments" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="BS_GetQuotationDocDetail" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="QuotationId" SessionField="QuotationId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </div>

                    <div class="m clear">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr id="trStartDt" runat="server">
                                <td>Contract Start Date
                                     <AjaxToolkit:CalendarExtender ID="calStartDt" runat="server" FirstDayOfWeek="Sunday" PopupButtonID="imgStartDt"
                                         Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtContractStartDt">
                                     </AjaxToolkit:CalendarExtender>
                                    <AjaxToolkit:MaskedEditExtender ID="meeStartDt" TargetControlID="txtContractStartDt" Mask="99/99/9999" MessageValidatorTip="true"
                                        MaskType="Date" AutoComplete="false" runat="server">
                                    </AjaxToolkit:MaskedEditExtender>
                                    <AjaxToolkit:MaskedEditValidator ID="mevStartDt" ControlExtender="meeStartDt" ControlToValidate="txtContractStartDt" IsValidEmpty="false"
                                        InvalidValueMessage="Contract Start Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true"
                                        MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2001" MaximumValue="31/12/2030"
                                        EmptyValueBlurredText="*" EmptyValueMessage="Please Enter Contract Start Date."
                                        runat="Server" ValidationGroup="ContractReq">
                                    </AjaxToolkit:MaskedEditValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtContractStartDt" runat="server" Width="100px" placeholder="dd/mm/yyyy" TabIndex="1"></asp:TextBox>
                                    <asp:Image ID="imgStartDt" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                </td>
                            </tr>
                            <tr id="trEndDt" runat="server">
                                <td>Contract End Date
                                     <AjaxToolkit:CalendarExtender ID="calEndDt" runat="server" FirstDayOfWeek="Sunday" PopupButtonID="imgEndDt"
                                         Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtContractEndDt">
                                     </AjaxToolkit:CalendarExtender>
                                    <AjaxToolkit:MaskedEditExtender ID="meeEndDt" TargetControlID="txtContractEndDt" Mask="99/99/9999" MessageValidatorTip="true"
                                        MaskType="Date" AutoComplete="false" runat="server">
                                    </AjaxToolkit:MaskedEditExtender>
                                    <AjaxToolkit:MaskedEditValidator ID="mevEndDt" ControlExtender="meeEndDt" ControlToValidate="txtContractEndDt" IsValidEmpty="false"
                                        InvalidValueMessage="Contract End Date is invalid" InvalidValueBlurredMessage="Invalid Date" SetFocusOnError="true"
                                        MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date" MinimumValue="01/01/2001" MaximumValue="31/12/2030"
                                        EmptyValueBlurredText="*" EmptyValueMessage="Please Enter Contract End Date."
                                        runat="Server" ValidationGroup="ContractReq">
                                    </AjaxToolkit:MaskedEditValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtContractEndDt" runat="server" Width="100px" placeholder="dd/mm/yyyy" TabIndex="2"></asp:TextBox>
                                    <asp:Image ID="imgEndDt" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>Upload Contract Copy
                                </td>
                                <td>
                                    <asp:FileUpload ID="fuUploadContractCopy" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Button ID="btnAddContractCopy" runat="server" OnClick="btnAddContractCopy_OnClick" ValidationGroup="ContractReq" Text="Save Document" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="m clear">
                    </div>
                </asp:Panel>
            </div>
            <div>
                <asp:HiddenField ID="lnkbtnContractCopy" runat="server"></asp:HiddenField>
            </div>
            <!--Document for Doc Upload - END -->

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

