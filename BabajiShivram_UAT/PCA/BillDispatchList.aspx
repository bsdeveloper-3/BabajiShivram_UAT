<%@ Page Title="Bill Dispatch" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BillDispatchList.aspx.cs" 
    Inherits="PCA_BillDispatchList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
    <asp:HiddenField ID="hdnJobIdList" runat="server" Value="0" />
    <asp:HiddenField ID="hdnBillId" runat="server" Value="0" />
    <div align="center">
        <asp:Label ID="lblMessage" runat="server"></asp:Label>
    </div>

    <fieldset runat="server">
    <legend>E-Bill Dispatch</legend>
        <div>
            <%--<div class="fleft">
                <b>Physical Bill Required </b>
                <asp:RadioButtonList ID="rblPhysicalBillRequired" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                </asp:RadioButtonList>
                </div>
                <div class="fleft" style="margin-left:20px;">
                <b>    E-Bill/Client Portal Required</b>
                <asp:RadioButtonList ID="rblEBillRequired" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                </asp:RadioButtonList>
            </div>--%>
            <%-- <div  class="fleft" style="margin-left:20px;">
        <b>    Client Portal Upload</b>
        <asp:RadioButtonList ID="rblBillClientPortal" runat="server" RepeatDirection="Horizontal">
            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
            <asp:ListItem Text="No" Value="0"></asp:ListItem>
        </asp:RadioButtonList>
            </div>--%>
            <div class="fleft">
                &nbsp;&nbsp;&nbsp;<asp:Button ID="btnClientPortalUpload" Text="Client Portal Update" runat="server" OnClick="btnClientPortalUpload_Click" Visible="false"> </asp:Button>
            </div>
            <div class="fleft">
                &nbsp;&nbsp;&nbsp;<asp:Button ID="btnPreviewEBill" Text="Preview E-Bill" runat="server" OnClick="btnPreviewEBill_Click"> </asp:Button>
            </div>

            <div class="fleft" style="margin-left:20px;">
                <asp:Button ID="btnPhysicalDispatch" Text="My Pacco Dispatch" runat="server" OnClick="btnPhysicalDispatch_Click" Visible="false"> </asp:Button>
            </div>
        <div  class="fleft" style="margin-left:20px;">
            <asp:Button ID="btnUpdateBillStatus" runat="server" Text = "Update" Visible="false" />
        </div>
        </div>
    <div class="clear"></div>
    <div class="m">
        <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False"
            CssClass="table" Width="99%" PagerStyle-CssClass="pgr"
            DataKeyNames="JobId,Billid" DataSourceID="DataSourceBillJobList" CellPadding="4"
            AllowPaging="True" AllowSorting="True" PageSize="80" OnRowCommand="gvJobDetail_RowCommand"
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
                        <asp:Label ID="lblBJVNo" runat="server" Text='<%#Eval("BJVNo")%>'/> 
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Bill Number">
                    <ItemTemplate>
                        <asp:Label ID="lblBillNumber" runat="server" Text='<%#Eval("INVNO")%>'/> 
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Bill Date" DataField="INVDATE" DataFormatString="{0:dd/MM/yyyy}"/>
                <asp:BoundField HeaderText="Bill Amount" DataField="INVAMOUNT" />
                <asp:BoundField HeaderText="E-Bill Date" DataField="EBillDate" DataFormatString="{0:dd/MM/yyyy}"/>
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

        <div id="divDatasource">
        <asp:SqlDataSource ID="DataSourceBillJobList" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
        SelectCommand="BL_GetBillJobDetailList" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter Name="JobIdList"/>
            <asp:Parameter Name="ModuleId"/>
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
                            <td>                                        
                            </td>
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
                    &nbsp;&nbsp;<asp:Label ID="lblErrorEmail" runat="server"></asp:Label>
                    <div id="dvMailSend" runat="server" style="max-height: 600px; max-width: 750px;">
                        <asp:Button ID="btnSendEmail" runat="server" Text="Send Email" OnClick="btnSendEmail_Click" ValidationGroup="mailRequired"
                            OnClientClick="if (!Page_ClientValidate('mailRequired')){ return false; } this.disabled = true; this.value = 'Processing...';" UseSubmitBehavior="false" />
                        <span style="font-weight:bold">From: Ebill@Babajishivram.com</span>
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
                        <fieldset style="width:700px;">
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
                                            <td>
                                                Dispatch From Location
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
                                            <td colspan="4">
                                                Dispatch To Address
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
                                                                <asp:CheckBox ID="chkAddress" runat="server" onclick ="SingleCheckboxCheck(this)"/> 
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
</asp:Content>

