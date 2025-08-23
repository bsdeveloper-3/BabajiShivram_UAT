<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TermsConditionMS.aspx.cs" EnableEventValidation="false"
    Inherits="Master_TermsConditionMS" %>

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
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlTermsCondition" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="upnlTermsCondition" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblresult" runat="server" EnableViewState="false"></asp:Label>

            </div>
            <fieldset>
                <legend>Terms & Condition Setup</legend>
                <div>
                    <asp:GridView ID="gvTermsCondition" runat="server" CssClass="table" ShowFooter="true" PagerStyle-CssClass="pgr"
                        AllowSorting="true" AutoGenerateColumns="false" AllowPaging="true"
                        OnPageIndexChanging="gvTermsCondition_PageIndexChanging" PageSize="20" PagerSettings-Position="TopAndBottom"
                        DataKeyNames="lid" OnRowCommand="gvTermsCondition_RowCommand" OnRowUpdating="gvTermsCondition_RowUpdating"
                        OnRowDeleting="gvTermsCondition_RowDeleting" OnRowEditing="gvTermsCondition_RowEditing" Width="100%"
                        OnRowCancelingEdit="gvTermsCondition_RowCancelingEdit" OnRowDataBound="gvTermsCondition_RowDataBound">
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
                            <asp:TemplateField HeaderText="Category Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblCatgFor" runat="server" Text='<%#Eval("sName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtCatgFor" runat="server" Text='<%#Eval("sName") %>' Width="90%" TabIndex="1"></asp:TextBox>
                                    <span style="color: Red">*</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtCatgFor_footer" runat="server" Text='<%#Eval("sName") %>' Width="90%" TabIndex="1"></asp:TextBox>
                                    <span style="color: Red">*</span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Terms & Condition">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkGetTermsCondition" CommandName="GetTerms" Text="Get Details" ToolTip="Get Terms & Condition." runat="server"
                                        CommandArgument='<%#Eval("lid") + ";" + Eval("sName") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:FileUpload ID="fuTermsDoc" runat="server" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:FileUpload ID="fuTermsDoc_footer" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit/Delete">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEdit" CommandName="Edit" ToolTip="Edit" Width="22" runat="server" Text="Edit" Font-Underline="true"></asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="lnkDelete" CommandName="Delete" ToolTip="Delete" Width="22" OnClientClick="return confirm('Sure to delete?');" runat="server" Text="Delete" Font-Underline="true"></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update" Width="39" runat="server" Text="Update" TabIndex="2" Font-Underline="true"></asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel" Width="22" runat="server" TabIndex="3" Text="Cancel" Font-Underline="true"></asp:LinkButton>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:LinkButton ID="lnkAdd" CommandName="Insert" ToolTip="Add" Width="22" runat="server" Text="Add" Font-Underline="true" TabIndex="2"></asp:LinkButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </fieldset>

            <div id="dvTerms&Conditions">
                <asp:HiddenField ID="hdnTermCondition" runat="server" />
                <AjaxToolkit:ModalPopupExtender ID="mpeTermConditions" runat="server" TargetControlID="hdnTermCondition"
                    PopupControlID="pnlTermCondition" BackgroundCssClass="modalBackground" DropShadow="true"
                    CancelControlID="imgdelRange">
                </AjaxToolkit:ModalPopupExtender>

                <asp:Panel ID="pnlTermCondition" runat="server" CssClass="modalPopup" BackColor="#F5F5DC" Width="850px" Height="550px">
                    <div id="div3" runat="server">

                        <table width="100%">
                            <tr class="heading">
                                <td align="center" style="background-color: #F5F5DC">
                                    <b>Terms & Condition -
                                        <asp:Label ID="lblTermName" runat="server"></asp:Label>
                                    </b>
                                    <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click" ToolTip="Export To Excel">
                                        <asp:Image ID="Image5" runat="server" ImageUrl="~/Images/Excel.jpg" Style="margin-top: 5px" />
                                    </asp:LinkButton>
                                    <span style="float: right;">
                                        <asp:ImageButton ID="imgdelRange" ImageUrl="~/Images/delete.gif" runat="server" OnClick="imgdelRange_Click" ToolTip="Close" />
                                    </span>
                                </td>
                            </tr>
                        </table>

                        <div class="m clear" style="text-align: center; font-weight: 600">
                            <asp:Label ID="lblError_Popup" runat="server" EnableViewState="false"></asp:Label>
                        </div>
                        <asp:Panel ID="pnlTermConditionContent" runat="server" Width="845px" Height="500px" ScrollBars="Auto" BorderStyle="Solid" BorderWidth="1px">
                            <asp:GridView ID="gvConditionDetail" runat="server" CssClass="gridview" ShowFooter="true" PagerStyle-CssClass="pgr"
                                AllowSorting="true" AutoGenerateColumns="false" AllowPaging="true" Style="white-space: normal"
                                OnPageIndexChanging="gvConditionDetail_PageIndexChanging" PageSize="20" PagerSettings-Position="TopAndBottom"
                                DataKeyNames="lid" OnRowCommand="gvConditionDetail_RowCommand" OnRowUpdating="gvConditionDetail_RowUpdating"
                                OnRowDeleting="gvConditionDetail_RowDeleting" OnRowEditing="gvConditionDetail_RowEditing" Width="100%"
                                OnRowCancelingEdit="gvConditionDetail_RowCancelingEdit">
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
                                    <asp:TemplateField HeaderText="Terms & Condition" ItemStyle-Width="60%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTerms" runat="server" Text='<%#Eval("sTermCondition") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtTerms" runat="server" Text='<%#Eval("sTermCondition") %>' Width="95%" TabIndex="1" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvterms" runat="server" ControlToValidate="txtTerms" SetFocusOnError="true" Display="Dynamic"
                                                ErrorMessage="Please Enter Terms & Condition." Text="*" ForeColor="Red" ValidationGroup="vgEditTerms"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtTerms_Footer" runat="server" Text='<%#Eval("sTermCondition") %>' Width="95%" TabIndex="1" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvAddMinRange" runat="server" ControlToValidate="txtTerms_Footer" SetFocusOnError="true" Display="Dynamic"
                                                ErrorMessage="Please Enter Terms & Condition." Text="*" ForeColor="Red" ValidationGroup="vgAddTerms"></asp:RequiredFieldValidator>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit/Delete" ItemStyle-Width="18%">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" CommandName="Edit" ToolTip="Edit" Width="22" runat="server" Text="Edit" Font-Underline="true"></asp:LinkButton>
                                            &nbsp;<asp:LinkButton ID="lnkDelete" CommandName="Delete" ToolTip="Delete" Width="22" OnClientClick="return confirm('Sure to delete?');" runat="server" Text="Delete" Font-Underline="true"></asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update" Width="39" runat="server" Text="Update" ValidationGroup="vgEditTerms" Font-Underline="true" TabIndex="2"></asp:LinkButton>
                                            &nbsp;<asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel" Width="22" runat="server" Text="Cancel" Font-Underline="true" TabIndex="3"></asp:LinkButton>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:LinkButton ID="lnkAdd" CommandName="Insert" ToolTip="Add" Width="22" runat="server" Text="Add" ValidationGroup="vgAddTerms" Font-Underline="true" TabIndex="2"></asp:LinkButton>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

