<%@ Page Title="Pending Dispatch - PCA" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="PendingPCDDispatchPCA.aspx.cs" Inherits="PCA_PendingPCDDispatchPCA" EnableEventValidation="false" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="content1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <style type="text/css">
        .hidden {
            display: none;
        }
    </style>

    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upShipment" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upShipment" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <script type="text/javascript">
                function GridSelectAllColumn(spanChk) {
                    var oItem = spanChk.children;
                    var theBox = (spanChk.type == "checkbox") ? spanChk : spanChk.children.item[0]; xState = theBox.checked;
                    elm = theBox.form.elements;
                    for (i = 0; i < elm.length; i++) {
                        if (elm[i].type === 'checkbox' && elm[i].checked != xState)
                            elm[i].click();
                    }
                }
            </script>

            <div align="center">
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <div class="clear"></div>
            <fieldset>
                <legend>Pending Dispatch - PCA</legend>
                <div class="m clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <table>
                            <div class="fleft">
                                <tr>
                                    <td>
                                        <uc1:DataFilter ID="DataFilter1" runat="server" />
                                    </td>
                                    <td valign="baseline">
                                        <asp:Button ID="btnUpdate" runat="server" Text="Consolidate Update" OnClick="Updatedetails_Click"></asp:Button></td>
                                    <td valign="baseline">
                                        <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                                        <asp:Image ID="Image2" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                                        </asp:LinkButton>
                                    </td>
                                </tr>
                            </div>
                            <div class="fleft" style="margin-left: 40px;">
                            </div>
                        </table>
                    </asp:Panel>
                </div>
                <div class="clear">
                </div>
                <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                    PagerStyle-CssClass="pgr" DataKeyNames="JobId,PCDCustomer" AllowPaging="True" AllowSorting="True" Width="100%"
                    PageSize="500" PagerSettings-Position="TopAndBottom" OnPreRender="gvJobDetail_PreRender"
                    DataSourceID="PCDSqlDataSource" OnRowCommand="gvJobDetail_RowCommand" OnRowDataBound="gvJobDetail_RowDataBound" OnPageIndexChanging="gvJobDetail_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkboxSelectAll" align="center" ToolTip="Check All" runat="server" onclick="GridSelectAllColumn(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chk1" runat="server" ToolTip="Check"></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="BS Job No" SortExpression="JobRefNo">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkJobNo" runat="server" Text='<%#Eval("JobRefNo") %>' CommandName="select" CommandArgument='<%#Eval("JobId")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" Visible="false" />
                        <asp:BoundField DataField="JobType" HeaderText="Job Type" SortExpression="JobType" />
                        <asp:BoundField DataField="MasterInvoiceNo" HeaderText="MasterInvoiceNo" SortExpression="MasterInvoiceNo" />
                        <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                        <asp:BoundField DataField="Mode" HeaderText="Mode" SortExpression="Mode" />
                        <asp:BoundField DataField="Port" HeaderText="Port" SortExpression="Port" />
                        <asp:BoundField DataField="DispatchFor" HeaderText="Document Received From" SortExpression="DispatchFor" />
                        <asp:BoundField DataField="UserName" HeaderText="Forwarded By" SortExpression="UserName" />
                        <asp:BoundField DataField="ForwardDate" HeaderText="Forward Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ForwardDate" />
                        <asp:TemplateField HeaderText="Dispatch Type">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddTypeOfDelivery" runat="server" Width="120px">
                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Hand Delivery" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Courier" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                            <%--<HeaderStyle CssClass="hidden"/>
                                <ItemStyle CssClass="hidden"/>--%>
                        </asp:TemplateField>
                        <asp:BoundField DataField="TypeOfDeliveryName" HeaderText="Dispatch Type" Visible="false" />
                        <asp:TemplateField HeaderText="Update">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkUpdate" runat="server" Text='<%#Eval("Remark") %>' CommandName="UpdateDispatch" CommandArgument='<%#Eval("JobId")+";"+Eval("PCDCustomer")+";"+ Eval("DocFolder")+";"+ Eval("FileDirName") %>'></asp:LinkButton>
                            </ItemTemplate>

                        </asp:TemplateField>
                    </Columns>
                    <%--<PagerTemplate>
                            <asp:GridViewPager runat="server" />
                        </PagerTemplate>--%>
                </asp:GridView>
            </fieldset>
            <div class="clear"></div>
            <div id="divHiddenField">
                <asp:HiddenField ID="hdnJobId" runat="server" />
                <asp:HiddenField ID="hdnCustomerPCA" runat="server" />
                <asp:HiddenField ID="hdnUploadPath" runat="server" />
            </div>
            <!-- Dispatch Detail -->
            <!-- Select Delivery Type -->

            <div id="div1">
                <cc1:ModalPopupExtender ID="ModalPopupExtenderdeliverytype" runat="server" CacheDynamicResults="false"
                    DropShadow="False" PopupControlID="Paneldeliverytype" TargetControlID="lnkdeliverytype">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="Paneldeliverytype" runat="server" CssClass="ModalPopupPanel">
                    <div class="header">
                        <div class="fleft">Delivery Type Detail</div>

                        <div class="fright">
                            <asp:ImageButton ID="ImageButton1" ImageUrl="~/Images/delete.gif" runat="server"
                                ToolTip="Close" />
                        </div>
                    </div>

                    <asp:DropDownList ID="ddTypeOfDelivery" runat="server" Width="120px">
                        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Hand Delivery" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Courier" Value="2"></asp:ListItem>
                    </asp:DropDownList>

                    <div align="center">
                        <asp:Button ID="BtnSavedeliveryType" Text="Save" runat="server" OnClick="BtnSavedeliveryType_Click" />
                    </div>

                </asp:Panel>
            </div>
            <div>
                <asp:LinkButton ID="lnkdeliverytype" runat="server" Text=""></asp:LinkButton>
            </div>
            <!-- Select Delivery Type END -->

            <!-- Hand Delivery Start -->
            <div id="divHandDelivery">
                <cc1:ModalPopupExtender ID="ModalPopupHandDelivery" runat="server" CacheDynamicResults="false"
                    DropShadow="False" PopupControlID="Panel2Document" TargetControlID="lnkDummy">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="Panel2Document" runat="server" CssClass="ModalPopupPanel">
                    <div class="header">
                        <div class="fleft">Hand Delivery Dispatch Detail</div>

                        <div class="fright">
                            <asp:ImageButton ID="imgClose" ImageUrl="~/Images/delete.gif" runat="server"
                                OnClick="btnCancelHandDelivery_Click" ToolTip="Close" />
                        </div>
                    </div>
                    <div align="center">
                        <asp:Label ID="lblPopMessage" runat="server" EnableViewState="false"></asp:Label>
                    </div>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>Documents Carrying Person Name
                                <asp:RequiredFieldValidator ID="rfvCartyName" runat="server" Display="Dynamic" SetFocusOnError="true"
                                    Text="*" ControlToValidate="txtCarryingPersonName" ValidationGroup="RequiredHand"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCarryingPersonName" runat="server" MaxLength="100"></asp:TextBox>
                            </td>
                            <td>Documents Pick Up / Dispatch Date
                                <cc1:MaskedEditExtender ID="MEditExtDispatchDate" TargetControlID="txtDispatchDate" Mask="99/99/9999" MessageValidatorTip="true"
                                    MaskType="Date" AutoComplete="false" runat="server">
                                </cc1:MaskedEditExtender>
                                <cc1:MaskedEditValidator ID="MEditValDispatchDate" ControlExtender="MEditExtDispatchDate" ControlToValidate="txtDispatchDate" IsValidEmpty="true"
                                    InvalidValueMessage="Date is invalid" SetFocusOnError="true" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date"
                                    MinimumValue="01/01/2014" MaximumValue="31/12/2025" runat="Server" ValidationGroup="RequiredHand"></cc1:MaskedEditValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDispatchDate" runat="server" Width="100px" placeholder="dd/mm/yyyy"></asp:TextBox>
                                <asp:Image ID="imgDispatchDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                <cc1:CalendarExtender ID="CalDispatchDate" runat="server" Enabled="True" EnableViewState="False"
                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgDispatchDate" PopupPosition="BottomRight"
                                    TargetControlID="txtDispatchDate">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <asp:Panel ID="pnlHadDeliveryPartTwo" runat="server" Visible="false">
                            <tr>
                                <td>Documents Delivered Date
                               <cc1:MaskedEditExtender ID="MEditExtDeliverDate" TargetControlID="txtPCDDeliveryDate" Mask="99/99/9999" MessageValidatorTip="true"
                                   MaskType="Date" AutoComplete="false" runat="server">
                               </cc1:MaskedEditExtender>
                                    <cc1:MaskedEditValidator ID="MEditValDeliverDate" ControlExtender="MEditExtDeliverDate" ControlToValidate="txtPCDDeliveryDate" IsValidEmpty="true"
                                        InvalidValueMessage="Date is invalid" SetFocusOnError="true" MinimumValueMessage="Invalid Date" MaximumValueMessage="Invalid Date"
                                        MinimumValue="01/01/2014" MaximumValue="31/12/2025" runat="Server" ValidationGroup="RequiredHand"></cc1:MaskedEditValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPCDDeliveryDate" runat="server" Width="100px" placeholder="dd/mm/yyyy"></asp:TextBox>
                                    <asp:Image ID="imgPCDDeliveryDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                    <cc1:CalendarExtender ID="CalPCDDeliveryDate" runat="server" Enabled="True" EnableViewState="False"
                                        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgPCDDeliveryDate" PopupPosition="BottomRight"
                                        TargetControlID="txtPCDDeliveryDate">
                                    </cc1:CalendarExtender>
                                </td>
                                <td>Documents Received Person Name
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReceivedPersonName" runat="server" MaxLength="100"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>POD Copy Attachment
                                </td>
                                <td>
                                    <asp:FileUpload ID="fileUpHandDelivery" runat="server" />
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                        </asp:Panel>
                        <tr>
                            <td>
                                <asp:Button ID="btnCancelHandDelivery" Text="Cancel" CausesValidation="false" runat="server" OnClick="btnCancelHandDelivery_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnSaveHandDelivery" Text="Save" runat="server" ValidationGroup="RequiredHand" OnClick="btnSaveHandDelivery_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <div>
                <asp:LinkButton ID="lnkDummy" runat="server" Text=""></asp:LinkButton>
            </div>
            <!-- Hand Delivery END -->
            <!-- Courier Delivery Start -->
            <div id="divCourier">
                <cc1:ModalPopupExtender ID="ModalPopupCourier" runat="server" CacheDynamicResults="false"
                    DropShadow="False" PopupControlID="Panel2Courier" TargetControlID="lnkDummyCourier">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="Panel2Courier" runat="server" CssClass="ModalPopupPanel">
                    <div class="header">
                        <div class="fleft">
                            Courier Dispatch Detail
                        </div>
                        <div class="fright">
                            <asp:ImageButton ID="imgCourDelete" ImageUrl="~/Images/delete.gif" runat="server" OnClick="btnCancelCourier_Click" ToolTip="Close" />
                        </div>
                    </div>
                    <div class="m"></div>
                    <div align="center">
                        <asp:Label ID="lblPopMessageCourier" runat="server" EnableViewState="false"></asp:Label>
                    </div>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>Courier Name
                                    <asp:RequiredFieldValidator ID="rfvCourierName" runat="server" Display="Dynamic" SetFocusOnError="true"
                                        Text="*" ControlToValidate="txtCourierName" ValidationGroup="RequiredCourier"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCourierName" runat="server" MaxLength="100"></asp:TextBox>
                            </td>
                            <td>Dispatch Docket No.
                            </td>
                            <td>
                                <asp:TextBox ID="txtDocketNo" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Dispatch Date
                                   <asp:CompareValidator ID="CompValCourierDispatchDate" runat="server" ControlToValidate="txtCourierDispatchDate" Display="Dynamic" Text="Invalid Date."
                                       SetFocusOnError="true" Type="Date" CultureInvariantValues="false" Operator="DataTypeCheck" ValidationGroup="RequiredCourier"></asp:CompareValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCourierDispatchDate" runat="server" Width="100px" placeholder="dd/mm/yyyy"></asp:TextBox>
                                <asp:Image ID="imgCourierDispatchDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                <cc1:CalendarExtender ID="CalCourierDispatchDate" runat="server" Enabled="True" EnableViewState="False"
                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgCourierDispatchDate" PopupPosition="BottomRight"
                                    TargetControlID="txtCourierDispatchDate">
                                </cc1:CalendarExtender>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        <asp:Panel ID="pnlCourierPart2" runat="server" Visible="false">
                            <tr>
                                <td>Documents Delivered Date
                                   <asp:CompareValidator ID="CompValDeliveryDate" runat="server" ControlToValidate="txtCourierDeliveryDate" Display="Dynamic" Text="Invalid date."
                                       ErrorMessage="Invalid Date." SetFocusOnError="true" Type="Date" CultureInvariantValues="false" Operator="DataTypeCheck" ValidationGroup="RequiredCourier"></asp:CompareValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCourierDeliveryDate" runat="server" Width="100px" placeholder="dd/mm/yyyy"></asp:TextBox>
                                    <asp:Image ID="imgCourDeliveryDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                    <cc1:CalendarExtender ID="CalCourierDeliveryDate" runat="server" Enabled="True" EnableViewState="False"
                                        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgCourDeliveryDate" PopupPosition="BottomRight"
                                        TargetControlID="txtCourierDeliveryDate">
                                    </cc1:CalendarExtender>
                                </td>


                                <td>Documents Received Person Name
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCourierReceivedBy" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>POD Copy Attachment
                                </td>
                                <td>
                                    <asp:FileUpload ID="fileUploadCourier" runat="server" />
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                        </asp:Panel>
                        <tr>
                            <td>
                                <asp:Button ID="btnCancelCourier" Text="Cancel" CausesValidation="false" runat="server" OnClick="btnCancelCourier_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnSaveCourier" Text="Save" runat="server" ValidationGroup="RequiredCourier" OnClick="btnSaveCourier_Click" />

                            </td>
                        </tr>
                    </table>

                </asp:Panel>
            </div>
            <div>
                <asp:LinkButton ID="lnkDummyCourier" runat="server" Text=""></asp:LinkButton>
            </div>
            <!-- Courier Delivery END -->
            <!--Dispatch Detail End -->
            <div id="divDatasource">
                <asp:SqlDataSource ID="PCDSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetPendingPCDPCADispatch" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
