<%@ Page Title="Quotation Charges Master" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="QuotationChargesMS.aspx.cs"
    Inherits="Master_QuotationChargesMS" EnableViewState="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />

    <style type="text/css">
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

    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upCharges" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="upCharges" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblresult" runat="server" EnableViewState="false"></asp:Label>
                <asp:ValidationSummary ID="vsAddRange" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="vgAddRange" CssClass="errorMsg" />
                <asp:ValidationSummary ID="vsEditRange" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="vgEditRange" CssClass="errorMsg" />
            </div>

            <fieldset>
                <legend>Quotation Charges Setup</legend>
                <div class="m clear">
                    Search For Category &nbsp;
                    <asp:DropDownList ID="ddlSearchForCatg" runat="server"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlSearchForCatg_OnSelectedIndexChanged">
                    </asp:DropDownList>
                    &nbsp;
                      <asp:Button ID="btnAddCategory" runat="server" Text="Add Quotation Category" OnClick="btnAddCategory_OnClick" CausesValidation="false" />
                </div>
                <div>
                    <asp:GridView ID="gvQuotationCharge" runat="server" CssClass="table" ShowFooter="true" PagerStyle-CssClass="pgr"
                        AllowSorting="true" AutoGenerateColumns="false" AllowPaging="true"
                        OnPageIndexChanging="gvQuotationCharge_PageIndexChanging" PageSize="20" PagerSettings-Position="TopAndBottom"
                        DataKeyNames="lid" OnRowCommand="gvQuotationCharge_RowCommand" OnRowUpdating="gvQuotationCharge_RowUpdating"
                        OnRowDeleting="gvQuotationCharge_RowDeleting" OnRowEditing="gvQuotationCharge_RowEditing" Width="100%"
                        OnRowCancelingEdit="gvQuotationCharge_RowCancelingEdit" OnRowDataBound="gvQuotationCharge_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="lid" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbllid" runat="server" Text='<%#Eval("lid") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtlid" runat="server" Text='<%#Eval("lid") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtlidfooter" runat="server" Text='<%#Eval("lid") %>'></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Quotation Category">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkGetCatg" CommandName="GetCatg" runat="server"
                                        Text="Show Category" CommandArgument='<%#Eval("lid") + "," + Eval("sName") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Quotation Charge">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkGetRanges" CommandName="GetRange" runat="server"
                                        Text='<%#Eval("sName") %>' CommandArgument='<%#Eval("lid") + "," + Eval("sName") %>'></asp:LinkButton>
                                    <%-- <asp:Label ID="lblQuotationCharge" runat="server" Text='<%#Eval("sName") %>'></asp:Label>--%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtQuotationCharge" runat="server" Text='<%#Eval("sName") %>' Width="70%"></asp:TextBox>
                                    <span style="color: Red">*</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtQuotationChargefooter" runat="server" Text='<%#Eval("sName") %>' Width="70%" TabIndex="1"></asp:TextBox>
                                    <span style="color: Red">*</span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description">
                                <ItemTemplate>
                                    <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("sDescription") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDescription" runat="server" Text='<%#Eval("sDescription") %>' MaxLength="300" Width="70%"></asp:TextBox>

                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtDescriptionfooter" runat="server" Text='<%#Eval("sDescription") %>' MaxLength="300" Width="70%" TabIndex="2"></asp:TextBox>

                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit/Delete">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEdit" CommandName="Edit" ToolTip="Edit" Width="22" runat="server" Text="Edit" Font-Underline="true"></asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="lnkDelete" CommandName="Delete" ToolTip="Delete" Width="22" OnClientClick="return confirm('Sure to delete?');" runat="server" Text="Delete" Font-Underline="true"></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update" Width="45" runat="server" Text="Update" Font-Underline="true"></asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel" Width="35" runat="server" Text="Cancel" Font-Underline="true"></asp:LinkButton>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:LinkButton ID="lnkAdd" CommandName="Insert" ToolTip="Add" Width="22" runat="server" Text="Add" Font-Underline="true" TabIndex="4"></asp:LinkButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="DataSourceQuoteCatg" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="BS_GetQuotationCategory" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                </div>
            </fieldset>

            <div id="dvRangesForCharge">
                <asp:HiddenField ID="hdnRange" runat="server" />
                <AjaxToolkit:ModalPopupExtender ID="ModalPopupExtender3" runat="server" TargetControlID="hdnRange"
                    PopupControlID="pnlRangesForCharge" BackgroundCssClass="modalBackground" DropShadow="true"
                    CancelControlID="imgdelRange">
                </AjaxToolkit:ModalPopupExtender>

                <asp:Panel ID="pnlRangesForCharge" runat="server" CssClass="modalPopup" BackColor="#F5F5DC" Width="850px" Height="300px">
                    <div id="div3" runat="server">
                        <table width="100%">
                            <tr class="heading">
                                <td align="center" style="background-color: #F5F5DC">
                                    <b>&nbsp;&nbsp;Ranges Applicable for Charge -
                                        <asp:Label ID="lblChargeName" runat="server"></asp:Label></b>
                                    &nbsp;&nbsp;&nbsp;
                                            <asp:LinkButton ID="lnkExportRangeDetail" runat="server" OnClick="lnkExportRangeDetail_Click" ToolTip="Export To Excel">
                                                <asp:Image ID="Image5" runat="server" ImageUrl="~/Images/Excel.jpg" Style="margin-top: 5px" />
                                            </asp:LinkButton>
                                    <span style="float: right">
                                        <asp:ImageButton ID="imgdelRange" ImageUrl="~/Images/delete.gif" runat="server" OnClick="btnCancelPopup_Click1" ToolTip="Close" />
                                    </span>
                                </td>
                            </tr>
                        </table>
                        <asp:Panel ID="pnlRangeDetail" runat="server" Width="845px" Height="265px" ScrollBars="Auto" BorderStyle="Solid" BorderWidth="1px">
                            <div class="m clear">
                                <asp:Button ID="btnAddAppFields" runat="server" Text="Add Applicable On Fields" OnClick="btnAddAppFields_Onclick" CausesValidation="false" />
                            </div>
                            <asp:GridView ID="gvChargeWsRanges" runat="server" CssClass="gridview" ShowFooter="true" PagerStyle-CssClass="pgr"
                                AllowSorting="true" AutoGenerateColumns="false" AllowPaging="true" Style="white-space: normal"
                                OnPageIndexChanging="gvChargeWsRanges_PageIndexChanging" PageSize="20" PagerSettings-Position="TopAndBottom"
                                DataKeyNames="lid" OnRowCommand="gvChargeWsRanges_RowCommand" OnRowUpdating="gvChargeWsRanges_RowUpdating"
                                OnRowDeleting="gvChargeWsRanges_RowDeleting" OnRowEditing="gvChargeWsRanges_RowEditing" Width="100%"
                                OnRowCancelingEdit="gvChargeWsRanges_RowCancelingEdit">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="lid" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbllid" runat="server" Text='<%#Eval("lid") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtlid" runat="server" Text='<%#Eval("lid") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtlidfooter" runat="server" Text='<%#Eval("lid") %>'></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Currency" ItemStyle-Width="2%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCurrency" runat="server" Text='<%#Eval("Currency") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlCurrency" runat="server" TabIndex="1">
                                                <asp:ListItem Selected="True" Value="0" Text="Indian Rupee"></asp:ListItem>
                                                <asp:ListItem Selected="False" Value="1" Text="Other"></asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlCurrencyfooter" runat="server" TabIndex="1">
                                                <asp:ListItem Selected="True" Value="0" Text="Indian Rupee"></asp:ListItem>
                                                <asp:ListItem Selected="False" Value="1" Text="Other"></asp:ListItem>
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Minimum Range" ItemStyle-Width="20%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMinRange" runat="server" Text='<%#Eval("MinRange") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtMinRange" runat="server" Text='<%#Eval("MinRange") %>' Width="90%" TabIndex="2"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvEditMinRange" runat="server" ControlToValidate="txtMinRange" SetFocusOnError="true" Display="Dynamic"
                                                ErrorMessage="Please Enter Minimum Range." Text="*" ForeColor="Red" ValidationGroup="vgEditRange"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtMinRangefooter" runat="server" Text='<%#Eval("MinRange") %>' Width="90%" TabIndex="2"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvAddMinRange" runat="server" ControlToValidate="txtMinRangefooter" SetFocusOnError="true" Display="Dynamic"
                                                ErrorMessage="Please Enter Minimum Range." Text="*" ForeColor="Red" ValidationGroup="vgAddRange"></asp:RequiredFieldValidator>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Maximum Range" ItemStyle-Width="20%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMaxRange" runat="server" Text='<%#Eval("MaxRange") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtMaxRange" runat="server" Text='<%#Eval("MaxRange") %>' Width="90%" TabIndex="3"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvEditMaxRange" runat="server" ControlToValidate="txtMaxRange" SetFocusOnError="true" Display="Dynamic"
                                                ErrorMessage="Please Enter Maximum Range." Text="*" ForeColor="Red" ValidationGroup="vgEditRange"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtMaxRangefooter" runat="server" Text='<%#Eval("MaxRange") %>' Width="90%" TabIndex="3"></asp:TextBox>

                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Applicable On" ItemStyle-Width="35%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblApplicableOn" runat="server" Text='<%#Eval("ApplicableOn") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlApplicableOn" runat="server" AppendDataBoundItems="true" DataSourceID="DataSourceAppFields" DataTextField="sName"
                                                DataValueField="lid" SelectedValue='<%#Eval("ApplicableOnId") %>'>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlApplicableOnFooter" runat="server" AppendDataBoundItems="true" DataSourceID="DataSourceAppFields" DataTextField="sName"
                                                DataValueField="lid">
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit/Delete" ItemStyle-Width="18%">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" CommandName="Edit" ToolTip="Edit" Width="22" runat="server" Text="Edit" Font-Underline="true"></asp:LinkButton>
                                            &nbsp;<asp:LinkButton ID="lnkDelete" CommandName="Delete" ToolTip="Delete" Width="22" OnClientClick="return confirm('Sure to delete?');" runat="server" Text="Delete" Font-Underline="true"></asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update" Width="39" runat="server" Text="Update" ValidationGroup="vgEditRange" Font-Underline="true" TabIndex="5"></asp:LinkButton>
                                            &nbsp;<asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel" Width="22" runat="server" Text="Cancel" Font-Underline="true" TabIndex="6"></asp:LinkButton>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:LinkButton ID="lnkAdd" CommandName="Insert" ToolTip="Add" Width="22" runat="server" Text="Add" ValidationGroup="vgAddRange" Font-Underline="true" TabIndex="5"></asp:LinkButton>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:SqlDataSource ID="DataSourceAppFields" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="BS_GetQuoteApplicableFields" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                        </asp:Panel>
                    </div>
                </asp:Panel>
            </div>
            <div id="dvCategoryForCharge">
                <asp:HiddenField ID="hdnCatgForCharge" runat="server" />
                <AjaxToolkit:ModalPopupExtender ID="mpeCatgForCharge" runat="server" TargetControlID="hdnCatgForCharge"
                    PopupControlID="pnlCatgForCharge" BackgroundCssClass="modalBackground" DropShadow="true"
                    CancelControlID="imgdelCatg">
                </AjaxToolkit:ModalPopupExtender>

                <asp:Panel ID="pnlCatgForCharge" runat="server" CssClass="modalPopup" BackColor="#F5F5DC" Width="450px" Height="270px">
                    <div id="div1" runat="server">
                        <table width="100%">
                            <tr class="heading">
                                <td align="center" style="background-color: #F5F5DC">
                                    <b>&nbsp;&nbsp;Quotation Category -
                                        <asp:Label ID="lblChargeName2" runat="server"></asp:Label></b>
                                    <span style="float: right">
                                        <asp:ImageButton ID="imgdelCatg" ImageUrl="~/Images/delete.gif" runat="server" OnClick="imgdelCatg_Click" ToolTip="Close" />
                                    </span>
                                </td>
                            </tr>
                        </table>
                        <asp:Panel ID="pnlCatgsLists" runat="server" Width="445px" Height="200px" ScrollBars="Auto" Style="border: 1px solid #abadb3">
                            <div style="padding: 5px; height: 150px; overflow: auto">
                                <asp:CheckBoxList ID="cblCategories" runat="server" AppendDataBoundItems="true" RepeatColumns="1" RepeatLayout="Table"
                                    DataSourceID="DataSourceCatgForCharges" DataTextField="sName" DataValueField="lid" CellPadding="5" CellSpacing="0"
                                    CssClass="gridview" Style="border-radius: 5px">
                                </asp:CheckBoxList>
                            </div>
                            <div class="m clear" style="border-top: 1px solid #abadb3; text-align: right">
                                <asp:Button ID="btnSaveCatg" runat="server" Text="Save" OnClick="btnSaveCatg_OnClick" CausesValidation="false" />
                            </div>
                        </asp:Panel>
                    </div>
                    <div>
                        <asp:SqlDataSource ID="DataSourceCatgForCharges" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="BS_GetQuotationCategory" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

