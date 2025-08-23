<%@ Page Title="Bill Dispatch" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BillDispatch.aspx.cs"
    Inherits="PCA_BillDispatch" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
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
        function SingleCheckboxCheck(ob) {
            var gridvalue = ob.parentNode.parentNode.parentNode;
            var inputs = gridvalue.getElementsByTagName("input");

            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "checkbox") {
                    if (ob.checked && inputs[i] != ob && inputs[i].checked) {
                        inputs[i].checked = false;
                    }
                }
            }
        }
    </script>
    <asp:HiddenField ID="hdnJobId" runat="server" Value="0" />
    <asp:HiddenField ID="hdnBillId" runat="server" Value="0" />
    <asp:HiddenField ID="hdnBillList" runat="server" Value="" />

    <div align="center">
        <asp:Label ID="lblMessage" runat="server"></asp:Label>
    </div>

    <fieldset>        
        <fieldset> <legend>Client Bill Requirement</legend>
        <div>
            <div class="fleft">
                <b>Physical Bill Required </b>
                <asp:RadioButtonList ID="rblPhysicalBillRequired" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Text="Yes" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Not Required" Value="2"></asp:ListItem>
                    <%--<asp:ListItem Text="Later" Value="3"></asp:ListItem>--%>
                </asp:RadioButtonList>
            </div>
            <div class="fleft" style="margin-left: 20px;">
                <b>E-Bill Email Required</b>
                <asp:RadioButtonList ID="rblEBillRequired" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Text="Yes" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Not Required" Value="2"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div class="fleft" style="margin-left: 20px;">
                <b>Client Portal Upload Required</b>
                <asp:RadioButtonList ID="rblBillClientPortal" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Text="Yes" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Not Required" Value="2"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div class="fleft" style="margin-left: 20px;">
                <asp:Button ID="btnUpdateBillStatus" runat="server" Text="Update" OnClick="btnUpdateBillStatus_Click" />
            </div>
        </div>
        </fieldset>
        <div class="clear"></div>
        <fieldset><legend>Bill Dispatch</legend>
            <div class="fleft">
                &nbsp;&nbsp;&nbsp;<asp:Button ID="btnClientPortalUpload" Text="Client Portal Update" runat="server" OnClick="btnClientPortalUpload_Click"> </asp:Button>
            </div>
            <div class="fleft">
                &nbsp;&nbsp;&nbsp;<asp:Button ID="btnPreviewEBill" Text="Preview E-Bill" runat="server" OnClick="btnPreviewEBill_Click"> </asp:Button>
            </div>
            <div class="fleft" style="margin-left:20px;">
                <asp:Button ID="btnPhysicalDispatch" Text="My Pacco Dispatch" runat="server" OnClick="btnPhysicalDispatch_Click" Visible="false"> </asp:Button>
            </div>
            <div class="fleft" style="margin-left:20px;">
                <asp:Button ID="btnApprove" runat="server" Text="Move Job To Dispatch Dept For Hand Delivery/Courier" OnClick="btnApprove_Click" OnClientClick="return confirm('Sure to Move Job To Dispatch Department for Hand Delivery/Courier ?');" />
             </div>
            <div class="m clear">
            <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False"
                CssClass="table" Width="99%" PagerStyle-CssClass="pgr"
                DataKeyNames="JobId,Billid" DataSourceID="DataSourceBillJob" CellPadding="4"
                AllowPaging="True" AllowSorting="True" PageSize="40" OnRowCommand="gvJobDetail_RowCommand"
                OnRowDataBound="gvJobDetail_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkboxSelectAll" align="center" ToolTip="Check All" runat="server" onclick="GridSelectAllColumn(this);" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkBillNo" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Job No">
                        <ItemTemplate>
                            <asp:Label ID="lblBJVNo" runat="server" Text='<%#Eval("BJVNo")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Bill Number">
                        <ItemTemplate>
                            <asp:Label ID="lblBillNumber" runat="server" Text='<%#Eval("INVNO")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Bill Date" DataField="INVDATE" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField HeaderText="Bill Amount" DataField="INVAMOUNT" />
                    <asp:BoundField HeaderText="E-Bill" DataField="EBillStatusName" />
                    <asp:BoundField HeaderText="Portal Update" DataField="PortalStatusName" />
                    <asp:BoundField HeaderText="Physical Dispatch" DataField="DispatchStatusName" />
                    <asp:TemplateField HeaderText="Upload">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkBillUpload" runat="server" Text="Bill Upload" CommandName="Upload" CommandArgument='<%#Eval("BillId")%>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="View">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkBillView" runat="server" Text="View" CommandName="View" CommandArgument='<%#Eval("BillId")%>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Download">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkBillDownload" runat="server" Text="Download" CommandName="Download" CommandArgument='<%#Eval("BillId")%>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Remove">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkBillRemove" runat="server" Text="Remove" CommandName="RemoveBill" CommandArgument='<%#Eval("BillId")%>'
                                OnClientClick="return confirm('Sure to delete Bill Document?');"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="E-Bill Email" Visible="false">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEBillEmail" runat="server" Text="Send E-Mail" CommandName="EBillEmail" CommandArgument='<%#Eval("BillId")%>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Physical Dispatch" Visible="false">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkPhysicalDispatch" runat="server" Text="Hard Copy Dispatch" CommandName="PhysicalDispatch" CommandArgument='<%#Eval("BillId")%>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        </fieldset>
        <div id="divDatasource">
            <asp:SqlDataSource ID="DataSourceBillJob" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="BL_GetPendingBillDetail" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter Name="JobId" />
                    <asp:Parameter Name="ModuleId" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </fieldset>

    <fieldset>        
        <div class="clear"></div>
        <fieldset><legend>History</legend>
           
            <div class="m clear">
            <asp:GridView ID="gvJobHistory" runat="server" AutoGenerateColumns="False"
                CssClass="table" PagerStyle-CssClass="pgr" DataKeyNames="Billid" AllowPaging="true"
                PagerSettings-Position="TopAndBottom" AllowSorting="True" Width="100%" PageSize="80" 
                OnRowCommand="gvJobHistory_RowCommand" DataSourceID="SqlDataSourceHistory">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex +1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="BJVNO" HeaderText="Job No"/>
                    <asp:BoundField DataField="BillNo" HeaderText="Bill No" SortExpression="BillNo"/>
                    <asp:BoundField DataField="BillDate" HeaderText="Bill Date" SortExpression="BillDate" DataFormatString="{0:dd/MM/yyyy}"/>
                    <%--<asp:BoundField DataField="BillAmount" HeaderText="Bill Amount" SortExpression="BillAmount" />--%>
                    <asp:BoundField DataField="EBillDate" HeaderText="E-Bill Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="EBillDate"/>
                    <asp:BoundField DataField="ClientPortalDate" HeaderText="Portal Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ClientPortalDate"/>
                    <asp:BoundField DataField="DocketNo" HeaderText="AWB No" SortExpression="DocketNo"/>
                    <asp:BoundField DataField="CourierName" HeaderText="Courier" SortExpression="CourierName"/>
                    <asp:BoundField DataField="DispatchDate" HeaderText="Dispatch Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="DispatchDate"/>
                    <asp:BoundField DataField="PCDDeliveryDate" HeaderText="Delivery Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="PCDDeliveryDate"/>
                    <asp:BoundField DataField="BillStatusName" HeaderText="Status" SortExpression="BillStatusName"/>
                    <asp:TemplateField HeaderText="View Cover Note">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDocumentCoverNote" runat="server" Text="View Cover Note"
                                CommandName="ViewCoverNote" CommandArgument='<%#Eval("BillId")%>'>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        </fieldset>
        <div id="divDatasourceHistory">
            <asp:SqlDataSource ID="SqlDataSourceHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="BL_GetOpenBillListById" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter Name="JobId" />
                    <asp:Parameter Name="ModuleId" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </fieldset>
    <%--  START : MODAL POP-UP FOR Upload-  --%>

    <div>
        <asp:LinkButton ID="lnkDomBillUpload10" runat="server"></asp:LinkButton>
    </div>

    <div id="divOpBillUpload">
        <cc1:ModalPopupExtender ID="BillUploadModalPopup" runat="server" CacheDynamicResults="false" PopupControlID="panBillUpload"
            CancelControlID="imgbtnBillUpload" TargetControlID="lnkDomBillUpload10" BackgroundCssClass="modalBackground" DropShadow="true">
        </cc1:ModalPopupExtender>
        <asp:Panel ID="panBillUpload" runat="server" CssClass="ModalPopupPanel" Width="600px">
            <div class="header">
                <div class="fleft">
                    <asp:Label ID="lblPopupName" runat="server" Text="Upload Bill"></asp:Label>
                </div>
                <div class="fright">
                    <asp:ImageButton ID="imgbtnBillUpload" ImageUrl="../Images/delete.gif" runat="server"
                        ToolTip="Close" />
                </div>
            </div>
            <div id="Div2" runat="server" style="max-height: 250px; overflow: auto; padding: 5px">
                <div>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>Job No</td>
                            <td colspan="3">
                                <asp:Label ID="lblBJVNumber" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Bill No</td>
                            <td>
                                <asp:Label ID="lblBJVBillNo" runat="server"></asp:Label>
                            </td>
                            <td>Bill Date</td>
                            <td>
                                <asp:Label ID="lblBJVBillDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Amount</td>
                            <td>
                                <asp:Label ID="lblBJVAmount" runat="server"></asp:Label>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                </div>
                <asp:RequiredFieldValidator ID="RFVUpload" runat="server" ControlToValidate="fuReceipt" SetFocusOnError="true" Display="Dynamic"
                    ForeColor="Red" ErrorMessage="Bill PDF Required" ValidationGroup="ValidateExpense" Font-Bold="true"></asp:RequiredFieldValidator>
                <div>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>PDF Doc                                      
                            </td>
                            <td>
                                <asp:FileUpload ID="fuReceipt" runat="server"></asp:FileUpload>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnUploadBill" runat="server" Text="Upload Bill" OnClick="btnUploadBill_Click" ValidationGroup="ValidateExpense" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </asp:Panel>
    </div>

    <%--  START : MODAL POP-UP FOR Email-  --%>
    <div id="divPreAlertEmail">
        <cc1:ModalPopupExtender ID="ModalPopupEmail" runat="server" CacheDynamicResults="false"
            DropShadow="False" PopupControlID="Panel2Email" TargetControlID="lnkDummy">
        </cc1:ModalPopupExtender>

        <asp:Panel ID="Panel2Email" runat="server" CssClass="ModalPopupPanel">
            <div class="header">
                <div class="fleft">
                    Customer E-Bill Email Draft
                </div>
                <div class="fright">
                    <asp:ImageButton ID="imgEmailClose" ImageUrl="~/Images/delete.gif" runat="server" OnClick="btnEMailCancel_Click" ToolTip="Close" />
                </div>
            </div>
            <div class="m"></div>
            <div id="dvMail" runat="server" style="max-height: 600px; max-width: 850px; overflow: auto;">
                <asp:Label ID="lblError1" runat="server"></asp:Label>
                <div id="dvMailSend" runat="server" style="max-height: 600px; max-width: 750px;">
                    <asp:Button ID="btnSendEmail" runat="server" Text="Send Email" OnClick="btnSendEmail_Click" ValidationGroup="mailRequired"
                        OnClientClick="if (!Page_ClientValidate('mailRequired')){ return false; } this.disabled = true; this.value = 'Processing...';" UseSubmitBehavior="false" />
                    <span style="font-weight:bold">From: Ebill@Babajishivram.com</span>
                    <div class="fright">
                    <asp:Button ID="btnBacklogEmail" runat="server" Text="Backlog Update" OnClick="btnBacklogEmail_Click" ValidationGroup="mailRequired"
                        OnClientClick="return confirm('Sure to Update Ebill Bill Backlog ?');" />
                    </div>
                    <%--<asp:Label ID="lblStatus" runat="server" Text="abc"></asp:Label>--%><br />
                    <div class="m">
                        <asp:Label ID="lblPopMessageEmail" runat="server" EnableViewState="false"></asp:Label>
                        Email To:&nbsp;<asp:TextBox ID="txtMailTo" runat="server" Width="85%"></asp:TextBox><br />
                        Email CC:&nbsp;<asp:TextBox ID="txtMailCC" runat="server" Width="85%"></asp:TextBox><br />
                        Subject:&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtSubject" runat="server" Width="85%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvSubject" runat="server" ControlToValidate="txtSubject" SetFocusOnError="true"
                            Text="*" ErrorMessage="Subject Required" ValidationGroup="mailRequired"></asp:RequiredFieldValidator>
                        <hr style="border-top: 1px solid #8c8b8b" />
                    </div>
                    <div id="divPreviewEmail" runat="server" style="margin-left: 10px;">
                    </div>
                    <fieldset style="width: 700px;">
                        <legend>Document Attachment</legend>
                        <asp:GridView ID="gvDocAttach" runat="server" AutoGenerateColumns="False" Width="100%"
                            CssClass="table"
                            CellPadding="4" PagerStyle-CssClass="pgr" PageSize="20" PagerSettings-Position="TopAndBottom">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Check">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkAttach" runat="server" Checked="true" />
                                        <asp:HiddenField ID="hdnDocPath" runat="server" Value='<%#Eval("DocPath") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="BillNo" HeaderText="Bill No " SortExpression="BillNo" />
                                <asp:BoundField DataField="DocName" HeaderText="Document Type" SortExpression="DocName" />
                                <asp:BoundField DataField="UserName" HeaderText="Uploaded By" />
                                <asp:BoundField DataField="UploadedDate" HeaderText="Uploaded Date" DataFormatString="{0:dd/MM/yyyy}" />
                            </Columns>
                        </asp:GridView>
                    </fieldset>
                </div>
            </div>
        </asp:Panel>
        <asp:LinkButton ID="lnkDummy" runat="server"></asp:LinkButton>
        <!--Customer Email Draft End -->
    </div>

    <%--  START : MODAL POP-UP FOR MyPacco Dispatch  --%>
    <div id="divMyPaccoDispatch">
        <asp:HiddenField ID="hdnUploadPath" runat="server" />
        <asp:HiddenField ID="hdnCustomerId" runat="server" />
        <asp:HiddenField ID="hdnCustomerName" runat="server" />
        <asp:Button ID="btnModalPopupDispatch" runat="server" Style="display: none" />
        <cc1:ModalPopupExtender ID="ModalPopupDispatch" runat="server" TargetControlID="btnModalPopupDispatch" CancelControlID="btnCancelDispatch" PopupControlID="PanelDispatch"></cc1:ModalPopupExtender>
        <asp:Panel ID="PanelDispatch" Style="display: none" runat="server">
            <fieldset class="ModalPopupPanel">
                <div title="Dispatch Details" class="header">
                    <textbox>MyPacco Dispatch Details</textbox>
                </div>
                <div class="AutoExtenderList">
                    <td>
                        <asp:Label ID="lblDispatchMessage" runat="server"></asp:Label>
                    </td>

                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>Dispatch From Location
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddBranch" runat="server">
                                    <asp:ListItem Text="Mumbai" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Delhi" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="Chennai" Value="6"></asp:ListItem>
                                </asp:DropDownList>

                            </td>
                            <td>
                                <asp:Button ID="btnGenerateAWB" Text="Generate Airway Bill No" runat="server" OnClick="btnGenerateAWB_Click" />
                            </td>
                        </tr>
                    </table>
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td colspan="4">Dispatch To Address
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="gvDispatchPlantAddress" runat="server" Width="100%" AutoGenerateColumns="False"
                                    CssClass="table" PagerStyle-CssClass="pgr" DataKeyNames="AddressId" PageSize="40" AllowPaging="true"
                                    PagerSettings-Position="TopAndBottom">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex +1%>
                                                <asp:CheckBox ID="chkAddress" runat="server" onclick="SingleCheckboxCheck(this)" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ContactPerson" HeaderText="Contact Name" />
                                        <asp:BoundField DataField="MobileNo" HeaderText="Mobile No" />
                                        <asp:BoundField DataField="AddressLine1" HeaderText="Address1" />
                                        <asp:BoundField DataField="AddressLine2" HeaderText="Address2" />
                                        <asp:BoundField DataField="City" HeaderText="City" />
                                        <asp:BoundField DataField="Pincode" HeaderText="Pincode" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                    <tr>
                        <td>
                            <asp:Button ID="btnCancelDispatch" runat="server" Text="Close" ToolTip="Close" />
                        </td>
                    </tr>
                </div>
            </fieldset>
        </asp:Panel>
    </div>

    <%--  START : MODAL POP-UP FOR Client Portal Upload  --%>
    <div>
        <asp:LinkButton ID="lnkDomClientUpload10" runat="server"></asp:LinkButton>
    </div>

    <div id="divClientPortalPopup">
        <cc1:ModalPopupExtender ID="ClientUploadModalPopup" runat="server" CacheDynamicResults="false" PopupControlID="panClientUpload"
            CancelControlID="imgbtnClientUpload" TargetControlID="lnkDomClientUpload10" BackgroundCssClass="modalBackground" DropShadow="true">
        </cc1:ModalPopupExtender>
        <asp:Panel ID="panClientUpload" runat="server" CssClass="ModalPopupPanel" Width="600px">
            <div class="header">
                <div class="fleft">
                    <asp:Label ID="lblPopupClient" runat="server" Text="Update Bill Upload Date On Client Portal"></asp:Label>
                </div>
                <div class="fright">
                    <asp:ImageButton ID="imgbtnClientUpload" ImageUrl="../Images/delete.gif" runat="server"
                        ToolTip="Close" />
                </div>
            </div>
            <div id="DivClientDate" runat="server" style="max-height: 250px; overflow: auto; padding: 5px">
                <div>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>Client Portal Upload Date
                                <asp:RequiredFieldValidator ID="RFVClientDate" runat="server" ControlToValidate="txtClientPortalDate"
                                    SetFocusOnError="true" ErrorMessage="Please Enter Date" Display="Dynamic"
                                    Text="Required" ValidationGroup="ValidateClient" InitialValue=""></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtClientPortalDate" runat="server" Width="100px" placeholder="dd/mm/yyyy"></asp:TextBox>
                                
                                <asp:Image ID="imgBill" ImageAlign="Top" ImageUrl="~/Images/btn_calendar.gif" runat="server" />
                                <cc1:CalendarExtender ID="calClientDate" runat="server" Enabled="True" EnableViewState="False"
                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgBill" PopupPosition="BottomRight"
                                    TargetControlID="txtClientPortalDate">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Client Portal Reference No
                                <asp:RequiredFieldValidator ID="RFVClientRefNo" runat="server" ControlToValidate="txtClientPortalRefNo"
                                    SetFocusOnError="true" ErrorMessage="Please Enter Ref No" Display="Dynamic"
                                    Text="Required" ValidationGroup="ValidateClient" InitialValue=""></asp:RequiredFieldValidator>
                            </td>
                            <td>
                            <asp:TextBox ID="txtClientPortalRefNo" runat="server" Width="100px" MaxLength="50"></asp:TextBox>
                                
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnSaveClientDate" runat="server" Text="Save" OnClick="btnSaveClientDate_Click" ValidationGroup="ValidateClient" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </asp:Panel>
    </div>

</asp:Content>

