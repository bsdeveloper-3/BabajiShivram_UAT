<%@ Page Title="Bill Scrutiny Details" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="PCDBillingScrutiny.aspx.cs" Inherits="PCA_PCDBillingScrutiny" EnableEventValidation="false" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter1" TagPrefix="uc1" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter2" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />

    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upJobDetail" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="upJobDetail" runat="server">
        <ContentTemplate>
            <style type="text/css">
                .hidden {
                    display: none;
                }
            </style>

            <script type="text/javascript">

                function Confirm() {

                    if (confirm("Do you want to Approve Billing Scrutiny?"))
                        return true;
                    else
                        return false;

                }

                function Reject(chk, txtReason) {

                    if (confirm("Do you want to Reject Billing Scrutiny?"))
                        return true;
                    else
                        return false;
                }

                function GridSelectAllColumn(spanChk) {
                    var oItem = spanChk.children;
                    var theBox = (spanChk.type == "checkbox") ? spanChk : spanChk.children.item[0]; xState = theBox.checked;
                    elm = theBox.form.elements;

                    for (i = 0; i < elm.length; i++) {
                        if (elm[i].type === 'checkbox' && elm[i].checked != xState)
                            elm[i].click();
                    }
                }

                function disableEnterKey(e) {
                    //create a variable to hold the number of the key that was pressed
                    var key;
                    //if the users browser is internet explorer
                    if (window.event) {
                        //store the key code (Key number) of the pressed key
                        key = window.event.keyCode;
                        //otherwise, it is firefox 
                    } else {
                        //store the key code (Key number) of the pressed key 
                        key = e.which;
                    }
                    //if key 13 is pressed (the enter key) 
                    if (key == 13) {
                        //do nothing
                        return false;
                        //otherwise
                    } else {
                        //continue as normal (allow the key press for keys other than "enter") 
                        return true;
                    }
                    //and don't forget to close the function   
                }

                function OnUserSelected(source, eventArgs) {

                    var results = eval('(' + eventArgs.get_value() + ')');

                    $get('ctl00_ContentPlaceHolder1_TabJobDetail_TabPCDBilling1_rpReason_ctl04_hdnUserId').value = results.Userid;
                    $get("ctl00_ContentPlaceHolder1_TabJobDetail_TabPCDBilling1_rpReason_ctl04_txt").value = results.Userid;

                    $get(ctl00_ContentPlaceHolder1_TabJobDetail_TabPCDBilling1_rpReason_ctl04_txtUser).focus();

                }

                function ResetUser() {
                    $get("ctl00_ContentPlaceHolder1_TabJobDetail_TabPCDBilling1_rpReason_ctl04_hdnUserId").value = '0';
                    $get("ctl00_ContentPlaceHolder1_TabJobDetail_TabPCDBilling1_rpReason_ctl04_txt").value = '0';

                }

                function toggleDiv(chk, ddlCategory, cboRFVCatgory, TSTId, lbltype, drlrtype, rfvreason, reason, userid, hndcust, lblmailto) {

                    //alert(document.getElementById(userid).style.display);                                  
                    var checkboxId = document.getElementById(chk);

                    var Varlbltype = document.getElementById(lbltype);
                    var Vardrlrtype = document.getElementById(drlrtype);
                    var reason1 = document.getElementsByName(reason);
                    var RFVReason = document.getElementById(rfvreason);
                    var RequiredCatgory = document.getElementById(cboRFVCatgory);
                    var txtuserid = document.getElementById(userid);
                    var hndcust = document.getElementById(hndcust);

                    if (checkboxId.checked == true) {

                        ValidatorEnable(RFVReason, true);

                        ValidatorEnable(RequiredCatgory, true);

                        document.getElementById(ddlCategory).style.display = 'inline';

                        if (reason == "LR/DC") {
                            //document.getElementById(lblReason).style.display = 'inline';
                            document.getElementById(TSTId).style.display = 'inline';
                            document.getElementById(lbltype).style.display = 'inline';
                            document.getElementById(drlrtype).style.display = 'inline';
                            document.getElementById(userid).style.display = 'none';
                            document.getElementById(lblmailto).style.display = 'none';
                        }
                        else if (reason == "Others") {

                            //document.getElementById(lblReason).style.display = 'inline';
                            document.getElementById(TSTId).style.display = 'inline';
                            document.getElementById(lbltype).style.display = 'none';
                            document.getElementById(drlrtype).style.display = 'none';
                            document.getElementById(userid).style.display = 'inline';
                            document.getElementById(lblmailto).style.display = 'inline';

                        }
                        else {
                            //document.getElementById(lblReason).style.display = 'inline';
                            document.getElementById(TSTId).style.display = 'inline';
                            document.getElementById(lbltype).style.display = 'none';
                            document.getElementById(drlrtype).style.display = 'none';
                            document.getElementById(userid).style.display = 'none';
                            document.getElementById(lblmailto).style.display = 'none';
                        }
                        checkboxId.checked = true;
                    }
                    else {
                        ValidatorEnable(RFVReason, false);
                        ValidatorEnable(RequiredCatgory, false);

                        document.getElementById(ddlCategory).style.display = 'none';

                        if (reason == "LR/DC") {
                            //document.getElementById(lblReason).style.display = 'none';
                            document.getElementById(TSTId).style.display = 'none';
                            document.getElementById(lbltype).style.display = 'none';
                            document.getElementById(drlrtype).style.display = 'none';
                            document.getElementById(userid).style.display = 'none';
                            document.getElementById(lblmailto).style.display = 'none';
                        }

                        else {
                            document.getElementById(TSTId).style.display = 'none';
                            //document.getElementById(lblReason).style.display = 'none';
                            document.getElementById(lbltype).style.display = 'none';
                            document.getElementById(drlrtype).style.display = 'none';
                            document.getElementById(userid).style.display = 'none';
                            document.getElementById(lblmailto).style.display = 'none';
                        }
                        checkboxId.checked = false;
                    }
                }

            </script>
            <div class="clear"></div>
            <div align="center">
                <asp:Label ID="lblMsgforApproveReject" runat="server"></asp:Label>
                <asp:Label ID="lblMsgforReceived" runat="server"></asp:Label>
            </div>
            <br />
            <div class="clear"></div>
            <cc1:TabContainer ID="TabJobDetail" runat="server" CssClass="Tab" Width="99%"
                ActiveTabIndex="0" OnActiveTabChanged="TabJobDetail_ActiveTabChanged">

                <%--<cc1:TabPanel ID="TabPCDBilling" runat="server" TabIndex="0" HeaderText="Non Receive" visible="false">
                    <ContentTemplate>

                        <div align="center">
                            <asp:Label ID="lblreceivemsg" runat="server"></asp:Label>
                            <asp:Label ID="lblMsgforNonReceived" runat="server"></asp:Label>
                        </div>

                        <asp:Panel ID="pnlFilter1" runat="server">
                            <div class="fleft">
                                <table>
                                    <tr>
                                        <td>
                                            <uc1:DataFilter1 ID="DataFilter1" runat="server" />
                                        </td>
                                        <td valign="top">
                                            <asp:Button ID="BtnReceive" runat="server" Text="Receive" CssClass="buttons" OnClick="Receive_Click" ToolTip="Receive File" /></td>
                                        <td valign="top">
                                            <asp:Button ID="BtnFreight" runat="server" Text="Freight" CssClass="buttons" OnClick="BtnFreight_Click" ToolTip="Freight File" /></td>
                                        <td valign="top">
                                            <asp:Button ID="btnPrintExpenseNonRCV" runat="server" Text="Print BJV" OnClick="btnPrintExpenseNonRCV_Click" ToolTip="Print BJV" />
                                        </td>
                                        <td valign="top">
                                            <asp:LinkButton ID="lnknonreceive" runat="server" OnClick="lnkNonreceiveExcel_Click" data-tooltip="&nbsp; Export To Excel">
                                                <asp:Image ID="img1" runat="server" ImageUrl="~/Images/Excel.jpg" />
                                            </asp:LinkButton></td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                        <%-- DataSourceID="PCDNonReceivedSqlDataSource" OnRowDataBound="gvNonRecievedJobDetail_RowDataBound"
                            OnPreRender="gvNonRecievedJobDetail_PreRender" OnPageIndexChanging="gvNonRecievedJobDetail_PageIndexChanging"--%
                        <asp:GridView ID="gvNonRecievedJobDetail" runat="server" PagerSettings-Position="TopAndBottom"
                            AutoGenerateColumns="False" CssClass="table" DataKeyNames="JobId" AllowPaging="True"
                            AllowSorting="True" Width="100%" PageSize="20">
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
                                        <asp:LinkButton ID="lnkJobNo" runat="server" Text='<%#Eval("JobRefNo") %>' CommandName="select" CommandArgument='<%#Eval("JobId") %>' Enabled="false"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:BoundField DataField="JobId" HeaderText="JobId" Visible="true" />--%
                                <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" Visible="False" />
                                <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                                <asp:BoundField DataField="JobType" HeaderText="Job Type" SortExpression="JobType" />
                                <asp:BoundField DataField="LastDispatchDate" HeaderText="Dispatch Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="LastDispatchDate" />
                                <asp:BoundField DataField="ShippingLineDate" HeaderText="Document HandOver Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ShippingLineDate" />
                                <asp:BoundField DataField="Aging1" HeaderText="Aging I" SortExpression="Aging1" />
                                <asp:BoundField DataField="Aging2" HeaderText="Aging II" SortExpression="Aging2" />
                                <asp:TemplateField HeaderText="rulename">
                                    <ItemTemplate>
                                        <asp:Label ID="rulename" runat="server" Text='<%#Eval("rulename")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                        <asp:SqlDataSource ID="PCDNonReceivedSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="GetPendingPCDToBillingScrutiny" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                                <asp:QueryStringParameter Name="IsReceived" DbType="String" QueryStringField="rulename" DefaultValue='1' />
                            </SelectParameters>
                        </asp:SqlDataSource>

                        <asp:Button ID="modelPopup2" runat="server" Style="display: none" />
                        <cc1:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="modelPopup2" PopupControlID="Panel2"></cc1:ModalPopupExtender>
                        <asp:Panel ID="Panel2" Style="display: none" runat="server">
                            <fieldset class="ModalPopupPanel">
                                <div title="Freight Details" class="header">
                                    <textbox>Freight Details</textbox>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:ImageButton ID="imgClose" align="center" ImageUrl="~/Images/delete.gif" runat="server" ToolTip="Close" />
                                </div>
                                <div class="AutoExtenderList">
                                    <td>
                                        <asp:Label ID="lblfreighterror" runat="server"></asp:Label>
                                    </td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblfreight" runat="server" Text="FreightJobno:  " Font-Bold="true" ForeColor="Black" Font-Size="9"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtjobno" runat="server" ReadOnly="true"></asp:TextBox>
                                                <asp:HiddenField ID="hdjobid" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:Button ID="btnsave" align="center" runat="server" Text="Save" OnClick="btnsave_click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </fieldset>
                        </asp:Panel>
                    </ContentTemplate>
                </cc1:TabPanel>--%>
                <div class="clear"></div>
                <cc1:TabPanel ID="TabPCDBilling1" runat="server" TabIndex="0" HeaderText="">
                    <ContentTemplate>
                        <asp:ValidationSummary ID="ValSummary" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="validateform" CssClass="errorMsg" EnableViewState="false" />
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="true" ValidationGroup="Required" CssClass="errorMsg" />

                        <div align="center">
                            <asp:Label ID="lblreceivemsg" runat="server"></asp:Label>
                            <asp:Label ID="lblMsgforNonReceived" runat="server"></asp:Label>
                        </div>
                        <div class="clear"></div>
                        <asp:Panel ID="pnlFilter2" runat="server">
                            <div class="fleft">
                                <table>
                                    <tr>
                                        <td>
                                            <uc2:DataFilter2 ID="DataFilter2" runat="server" />
                                        </td>
                                        <td valign="top">
                                            <asp:Button ID="BtnFreight" runat="server" Text="Freight" CssClass="buttons" OnClick="BtnFreight_Click" ToolTip="Freight File" /></td>
                                        <td valign="top">
                                           &nbsp; <asp:Button ID="btnPrintExpense" runat="server" Text="Print BJV" OnClick="btnPrintExpense_Click" ToolTip="Print BJV" />
                                        </td>
                                        <td valign="top">
                                           &nbsp;&nbsp; <asp:LinkButton ID="lnkReceiveXls" runat="server" OnClick="lnkreceiveExcel_Click" data-tooltip="&nbsp; Export To Excel">
                                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Excel.jpg" />
                                            </asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                        <br />
                        <div class="clear"></div>
                        <asp:GridView ID="gvRecievedJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                            PagerStyle-CssClass="pgr" DataKeyNames="JobId" AllowPaging="True"
                            AllowSorting="True" Width="97%"
                            PageSize="20" PagerSettings-Position="TopAndBottom"
                            DataSourceID="PCDReceivedSqlDataSource" OnRowDataBound="gvRecievedJobDetail_RowDataBound"
                            OnPreRender="gvRecievedJobDetail_PreRender" OnRowCommand="gvRecievedJobDetail_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkboxSelectAll2" Text="Print All" runat="server" onclick="GridSelectAllColumn(this);" />
                                    </HeaderTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkPrint" runat="server"></asp:CheckBox>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>

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
                                        <asp:LinkButton ID="lnkJobNo" ToolTip="Approve or Reject billing Scrutiny" runat="server" Text='<%#Eval("JobRefNo") %>' CommandName="DocumentPopup" CommandArgument='<%#Eval("JobId")+";"+ Eval("DocFolder")+";"+ Eval("FileDirName")+";"+ Eval("Customer")+";"+ Eval("JobRefNo")+";"+ Eval("RMSNonRMS")%>'></asp:LinkButton>

                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" Visible="false" />
                                <%--<asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("JobId") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                                <asp:BoundField DataField="JobType" HeaderText="Job Type" SortExpression="JobType" />
                                <asp:BoundField DataField="LastDispatchDate" HeaderText="Dispatch Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="LastDispatchDate" />
                                <asp:BoundField DataField="ShippingLineDate" HeaderText="Document HandOver Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ShippingLineDate" />
                                <asp:BoundField DataField="ReceivedDate" HeaderText="Received Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ReceivedDate" Visible="false" />
                                <asp:BoundField DataField="Aging1" HeaderText="Aging I" SortExpression="Aging1" />
                                <asp:BoundField DataField="Aging2" HeaderText="Aging II" SortExpression="Aging2" />
                                <asp:BoundField DataField="Aging3" HeaderText="Aging III" SortExpression="Aging3" />
                                <asp:TemplateField HeaderText="rulename">
                                    <ItemTemplate>
                                        <asp:Label ID="rulename" runat="server" Text='<%#Eval("rulename")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="RMSNonRMS" HeaderText="RMSNonRMS" SortExpression="RMSNonRMS" />
                                <asp:BoundField DataField="Instructions" HeaderText="Instructions" SortExpression="Instructions" />
                            </Columns>

                        </asp:GridView>
                        <br />
                        <div class="clear"></div>
                        <asp:Button ID="modelPopup" runat="server" Style="display: none" />
                        <div class="clear"></div>
                        <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="modelPopup" CancelControlID="btnCancel" PopupControlID="Panel1"></cc1:ModalPopupExtender>

                        <asp:Panel ID="Panel1" Style="display: none; min-width: 40%" runat="server">

                            <fieldset class="ModalPopupPanel">

                                <div title="Reject Details" class="header">
                                    <textbox>Reject Details</textbox>
                                </div>
                                <div class="clear"></div>
                                <div class="AutoExtenderList">
                                    <td>
                                        <asp:Label ID="lblerror" runat="server"></asp:Label>
                                    </td>

                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl" runat="server" Text="Reason for Pendency" ForeColor="Black" Font-Size="9"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:Repeater ID="rpReason" runat="server" OnItemDataBound="rpReason_ItemDataBound" DataSourceID="DsReasonforpendency">
                                                    <ItemTemplate>
                                                        <div class="clear">
                                                        </div>
                                                        <div style="float: left;">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkreasonofpendency" Text='<%#DataBinder.Eval(Container.DataItem,"ReasonforPendency") %>'
                                                                            runat="server" TabIndex="13" Width="100" Checked="false" ForeColor="Black" Font-Size="9" />
                                                                    </td>
                                                                    <td>
                                                                        <%--<asp:Label ID="lblReason" Text="Remarks: " runat="server" ForeColor="Black" Font-Size="9" /></td>--%>
                                                                        <asp:DropDownList ID="ddCategory" runat="server" Width="120px">
                                                                            <asp:ListItem Text="-Category-" Value="0"></asp:ListItem>
                                                                            <asp:ListItem Text="Transport" Value="1"></asp:ListItem>
                                                                            <asp:ListItem Text="CFS" Value="2"></asp:ListItem>
                                                                            <asp:ListItem Text="KAM" Value="3"></asp:ListItem>
                                                                            <asp:ListItem Text="DO" Value="4"></asp:ListItem>
                                                                            <asp:ListItem Text="Other" Value="10"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="RFVCatgory" ControlToValidate="ddCategory" runat="server" ValidationGroup="validateform" Text="*" SetFocusOnError="true"
                                                                            InitialValue="0" ErrorMessage='<%#DataBinder.Eval(Container.DataItem,"ReasonforPendency") + "- Please Select Category."%>' ForeColor="Red" Enabled="false"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtReason" runat="server" ForeColor="Black" Font-Size="9" ValidationGroup="validateform" placeholder="Remark"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="RFVRejectReason" ControlToValidate="txtReason" runat="server" ValidationGroup="validateform" Text="*" SetFocusOnError="true"
                                                                            ErrorMessage='<%#DataBinder.Eval(Container.DataItem,"ReasonforPendency") + "- Please Enter Remark."%>' ForeColor="Red" Enabled="false"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lbltype" runat="server" Text="Type:" ForeColor="Black" Font-Bold="true" Font-Size="8"></asp:Label>
                                                                        <asp:DropDownList ID="Drplrdctype" runat="server" DataSourceID="sqldatasourcelrdctype" ForeColor="Black" Font-Size="9" DataTextField="LRDCType" DataValueField="Id">
                                                                        </asp:DropDownList>
                                                                        <asp:HiddenField ID="hdnDocId" Value='<%#DataBinder.Eval(Container.DataItem,"Id") %>'
                                                                            runat="server" Visible="false"></asp:HiddenField>
                                                                    </td>
                                                                    <td>
                                                                        <div id="divUnderClear">
                                                                            <asp:HiddenField ID="hdnUserId" runat="server" />
                                                                            <asp:Label ID="lblMailTo" runat="server" Text="Mail To : " ForeColor="Black" Font-Bold="true" Font-Size="8" Visible="false"></asp:Label>
                                                                            <asp:TextBox ID="txt" runat="server" Text="" Style="display: none"></asp:TextBox>
                                                                            <asp:TextBox ID="txtUser" runat="server" MaxLength="100" placeholder="User Name"
                                                                                Width="50%" Visible="false" onKeyPress="return disableEnterKey(event)" onkeyUp="Javascript:ResetUser();"></asp:TextBox>
                                                                            <asp:CompareValidator ID="Comval" runat="server" ControlToValidate="txt" Operator="NotEqual" ValueToCompare="0" Type="Integer" ValidationGroup="validateform" Text="*"
                                                                                SetFocusOnError="true" ErrorMessage='<%#DataBinder.Eval(Container.DataItem,"ReasonforPendency") + "- Please select porper UserName."%>'
                                                                                ForeColor="Red"></asp:CompareValidator>
                                                                            <div id="divwidthCust" runat="server">
                                                                            </div>
                                                                            <cc1:AutoCompleteExtender ID="UserExtender" runat="server" BehaviorID="divwidthCust"
                                                                                CompletionListCssClass="AutoExtender" CompletionListElementID="divwidthCust"
                                                                                CompletionListHighlightedItemCssClass="AutoExtenderHighlight" CompletionListItemCssClass="AutoExtenderList"
                                                                                FirstRowSelected="true" MinimumPrefixLength="2" ServiceMethod="GetUserCompletionList"
                                                                                OnClientItemSelected="OnUserSelected" ServicePath="~/WebService/UserAutoComplete.asmx"
                                                                                TargetControlID="txtUser" UseContextKey="True">
                                                                            </cc1:AutoCompleteExtender>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="3">
                                                <br />
                                                <asp:Button ID="btnOkay" runat="server" Text="Done" OnClick="btnOkay_Click" ValidationGroup="validateform" ToolTip="Save" OnClientClick="if (!Reject()) return false;" />
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" />
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                </div>
                            </fieldset>
                        </asp:Panel>
                        <div class="clear"></div>
                        <div id="divDocument">
                            <cc1:ModalPopupExtender ID="ModalPopupDocument" runat="server" CacheDynamicResults="false"
                                DropShadow="False" PopupControlID="Panel2Document" TargetControlID="lnkDummy">
                            </cc1:ModalPopupExtender>
                            <div class="clear"></div>
                            <asp:Panel ID="Panel2Document" runat="server" CssClass="ModalPopupPanel" ScrollBars="Vertical" Height="300px">
                                <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                                <asp:HiddenField ID="hdnJobId" runat="server" />
                                <asp:HiddenField ID="hdnUploadPath" runat="server" />
                                <asp:HiddenField ID="hdnCurrentId" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnPrevId" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnNxtId" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnJobType" runat="server" />
                                <center>
                                    <table>
                                        <tr style="border-collapse: collapse">
                                            <td>
                                                <asp:Button ID="btnPrevious" runat="server" Text="<" Font-Size="10" ToolTip="Previous" CommandName="DocumentPopup" CommandArgument='<%#Eval("JobId")+";"+ Eval("DocFolder")+";"+ Eval("FileDirName")+";"+ Eval("Customer")+";"+ Eval("JobRefNo")%>' OnClick="btnPrevious_click" />
                                            </td>
                                            <td>
                                                <asp:Repeater ID="rptDocument" runat="server" OnItemDataBound="rpDocument_ItemDataBound" OnItemCommand="rpDocument_ItemCommand">
                                                    <HeaderTemplate>
                                                        <table class="table" cellpadding="0" cellspacing="0" width="100%" valign="top">
                                                            <tr class="header">
                                                                <td colspan="6">
                                                                    <asp:Label ID='lbldiv' runat="server" align="center" Text="Document Details" Font-Bold="true"></asp:Label>
                                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                    <asp:ImageButton ID="imgClose" align="center" ImageUrl="~/Images/delete.gif" runat="server" ToolTip="Close" /></td>
                                                            </tr>

                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lbljobrefno1" runat="server" Text="JobNo" Font-Bold="true"></asp:Label></td>
                                                                <h1><%# Eval("Customer")%></h1>
                                                                <td>
                                                                    <asp:Label ID="lbljobrefno2" runat="server" ForeColor="Black" /></td>

                                                                <td>
                                                                    <asp:Label ID="lblcustomer1" runat="server" Text="Customer" Font-Bold="true"></asp:Label></td>
                                                                <h1><%# Eval("JobRefNo")%></h1>
                                                                <td>
                                                                    <asp:Label ID="lblcustomer2" runat="server" ForeColor="Black" />
                                                                </td>

                                                                <%--<td><asp:Label ID="lblRMSNonRms" runat="server" Text="RMS/NONRMS" Font-Bold="true"></asp:Label></td>
                                                                      <h1><%# Eval("RMSNonRMS")%></h1>                        
                                                                    <td><asp:label ID="lblRMSNonRms2" runat="server" ForeColor="Black"/></td>--%>
                                                            </tr>

                                                            <tr bgcolor="#FF781E">
                                                                <th>Sl
                                                                </th>
                                                                <th>Name
                                                                </th>
                                                                <th colspan="5">Type
                                                                </th>
                                                            </tr>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td>
                                                                <%#Container.ItemIndex +1%>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkDocType" Text='<%#DataBinder.Eval(Container.DataItem,"sName") %>'
                                                                    runat="server" Checked="true" Enabled="false" />&nbsp;
                                                                <asp:HiddenField ID="hdnDocId" Value='<%#DataBinder.Eval(Container.DataItem,"lId") %>'
                                                                    runat="server"></asp:HiddenField>
                                                            </td>
                                                            <td>
                                                                <asp:CustomValidator ID="CVCheckBoxList" runat="server" ClientValidationFunction="ValidateCheckBoxList"
                                                                    Enabled="false" ErrorMessage="Please Select Type" ValidationGroup="Required" Display="Dynamic"></asp:CustomValidator>
                                                                <asp:CheckBoxList ID="chkDuplicate" runat="server" RepeatDirection="Horizontal">
                                                                    <asp:ListItem Text="Original" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="Copy" Value="2"></asp:ListItem>
                                                                </asp:CheckBoxList>

                                                            </td>
                                                            <td colspan="3">
                                                                <asp:LinkButton ID="lnkview" runat="server" Text="VIEW" CommandName="View" CommandArgument='<%#Eval("Docpath")%>'></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <tr>
                                                            <td colspan="6" align="center">
                                                                <asp:Button ID='lnkApprove' runat="server" Text="Approve" ToolTip="" CommandName="Approve" CommandArgument='<%#Eval("JobId") %>' OnClick="lnkApprove_click" OnClientClick="if (!Confirm()) return false;"></asp:Button>
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:Button ID='lnkReject' runat="server" Text="Reject" OnClick="lnkReject_click" ToolTip="" CommandName="Reject" CommandArgument='<%#Eval("JobId") %>'></asp:Button></td>
                                                        </tr>
                                                        </table>
                                                    </FooterTemplate>
                                                </asp:Repeater>

                                                <div align="center">
                                                    <asp:Label ID="lblMessage" runat="server" Test="No Contract Is Available In System"></asp:Label>
                                                </div>
                                                <asp:Repeater ID="rptBJVDetails" runat="server" OnItemDataBound="rptBJVDetails_ItemDataBound">
                                                    <HeaderTemplate>
                                                        <table class="table" cellpadding="0" cellspacing="0" width="100%" valign="top">
                                                            <tr class="header">
                                                                <td colspan="10">
                                                                    <asp:Label ID='lbldiv' runat="server" align="center" Text="BJV Details" Font-Bold="true"></asp:Label>
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;                        
                                                            </tr>

                                                            <tr bgcolor="#FF781E">
                                                                <th>SI</th>
                                                                <th>VCHDATE</th>
                                                                <th>VCHNO</th>
                                                                <th>CONTRANAME</th>
                                                                <th>CHQNO</th>
                                                                <th>CHQDATE</th>
                                                                <th>DEBITAMT</th>
                                                                <%--<th>CREDITAMT</th><th>AMOUNT</th>--%><th>NARRATION</th>
                                                            </tr>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td colspan="10">
                                                                <asp:Label ID="lblmsg" runat="server"></asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td><%#Container.ItemIndex +1%></td>
                                                            <td>
                                                                <asp:Label ID="lblVCHDATE" runat="server" Text='<%#Eval("VCHDATE")%>'></asp:Label></td>
                                                            <td>
                                                                <asp:Label ID="lblVCHNO" runat="server" Text='<%#Eval("VCHNO")%>'></asp:Label></td>
                                                            <td width="200px" style="white-space: normal">
                                                                <asp:Label ID="lblCONTRANAME" runat="server" Text='<%#Eval("CONTRANAME")%>'></asp:Label></td>
                                                            <td>
                                                                <asp:Label ID="lblCHQNO" runat="server" Text='<%#Eval("CHQNO")%>'></asp:Label></td>
                                                            <td>
                                                                <asp:Label ID="lblCHQDATE" runat="server" Text='<%#Eval("CHQDATE")%>'></asp:Label></td>
                                                            <td>
                                                                <asp:Label ID="lblDEBITAMT" runat="server" Text='<%#Eval("DEBITAMT")%>'></asp:Label></td>
                                                            <%--<td><asp:Label ID="lblCREDITAMT" runat="server" Text='<%#Eval("CREDITAMT")%>'></asp:Label></td>
			 <td><asp:Label ID="lblAMOUNT" runat="server" Text='<%#Eval("AMOUNT")%>'></asp:Label></td>--%>
                                                            <td width="200px" style="white-space: normal">
                                                                <asp:Label ID="lblNARRATION" runat="server" Text='<%#Eval("NARRATION")%>'></asp:Label></td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <tr>
                                                            <td colspan="6" align="right"><b>Total</b></td>
                                                            <td>
                                                                <asp:Label ID="lbltotDebitamt" runat="server"></asp:Label></td>
                                                            <%--<td> <asp:Label ID="lbltotCREDITAMT" runat="server"></asp:Label></td>
                        <td> <asp:Label ID="lbltotAMOUNT" runat="server"></asp:Label></td>--%>
                                                            <td></td>
                                                        </tr>

                                                    </FooterTemplate>
                                                </asp:Repeater>
                                    </table>
                                </center>
                                </td>
                                <td>
                                    <asp:Button ID="btnNext" runat="server" Text=">" Font-Size="10" ToolTip="Next" CommandName="DocumentPopup" CommandArgument='<%#Eval("JobId")+";"+ Eval("DocFolder")+";"+ Eval("FileDirName")+";"+ Eval("Customer")+";"+ Eval("JobRefNo")%>' OnClick="btnNext_click" /></td>
                                </tr>
</table>
                        
                            </asp:Panel>

                        </div>
                        <div class="clear"></div>
                        <div>
                            <asp:LinkButton ID="lnkDummy" runat="server" Text=""></asp:LinkButton>
                        </div>

                        <asp:SqlDataSource ID="PCDDocumentSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="GetPCDDocumentByRecieved" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                <asp:Parameter Name="DocumentForType" DefaultValue="3" />
                            </SelectParameters>
                        </asp:SqlDataSource>

                        <asp:SqlDataSource ID="PCDReceivedSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="GetPendingPCDToBillingScrutiny" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                                <asp:QueryStringParameter Name="IsReceived" DbType="string" QueryStringField="rulename" DefaultValue='0' />
                            </SelectParameters>
                        </asp:SqlDataSource>

                        <asp:SqlDataSource ID="DsReasonforpendency" runat="server"
                            ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="GetReasonforpendency" SelectCommandType="StoredProcedure"></asp:SqlDataSource>

                        <asp:SqlDataSource ID="sqldatasourcelrdctype" runat="server"
                            ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="Getlrdctype" SelectCommandType="StoredProcedure"></asp:SqlDataSource>

                    </ContentTemplate>

                </cc1:TabPanel>

            </cc1:TabContainer>
            <div class="clear"></div>
            <div style="text-align: right">
                <br />
                <asp:Repeater ID="Rptpriorities" runat="server" DataSourceID="SqlDataSourcepriorities" OnItemDataBound="Rptpriorities_ItemDataBound">
                    <ItemTemplate>
                        <asp:TextBox ID="txtpriorities" runat="server" Width="0.5%"></asp:TextBox>
                        <asp:Label ID="lblpriorities" runat="server" Text='<%#Eval("prioritiesName")%>'></asp:Label>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
            <asp:SqlDataSource ID="SqlDataSourcepriorities" runat="server"
                ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="Getpriorities" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            <div class="clear"></div>
            <asp:Button ID="modelPopup2" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="modelPopup2" PopupControlID="Panel2"></cc1:ModalPopupExtender>
            <asp:Panel ID="Panel2" Style="display: none" runat="server">
                <fieldset class="ModalPopupPanel">
                    <div title="Freight Details" class="header">
                        <textbox>Freight Details</textbox>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:ImageButton ID="imgClose" align="center" ImageUrl="~/Images/delete.gif" runat="server" ToolTip="Close" />
                    </div>
                    <div class="AutoExtenderList">
                        <td>
                            <asp:Label ID="lblfreighterror" runat="server"></asp:Label>
                        </td>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblfreight" runat="server" Text="FreightJobno:  " Font-Bold="true" ForeColor="Black" Font-Size="9"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtjobno" runat="server" ReadOnly="true"></asp:TextBox>
                                    <asp:HiddenField ID="hdjobid" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Button ID="btnsave" align="center" runat="server" Text="Save" OnClick="btnsave_click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </fieldset>
            </asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

