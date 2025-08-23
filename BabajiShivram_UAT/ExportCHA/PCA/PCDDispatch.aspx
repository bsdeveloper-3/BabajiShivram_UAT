<%@ Page Title="Bill Dispatch Details" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PCDDispatch.aspx.cs" 
    Inherits="PCA_PCDDispatch" EnableEventValidation="false" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter1" TagPrefix="uc1" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter2" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%------start Covering Letter------Version=13.0.3500.0-------Version=12.0.2000.0---%>
<%@ Register Assembly="CrystalDecisions.Web,Version=13.0.2000.0 , Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%----end Covering Letter--------------%>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <script src="../JS/GridViewCellEdit.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript" src="../JS/CheckBoxListPCDDocument.js"></script>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upJobDetail" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="upJobDetail" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <style type="text/css">
                .hidden {
                    display: none;
                }
            </style>

            <%------start Covering Letter----------------%>
            <script language="javascript" type="text/javascript" src="../JS/jquery-1.8.3.js"></script>
            <%------end Covering Letter----------------%>

            <script type="text/javascript">

                //-------- Start Covering letter--------------------------------

                $("[src*=plus]").live("click", function () {
                    $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
                    $(this).attr("src", "../images/minus.png");
                });
                $("[src*=minus]").live("click", function () {
                    $(this).attr("src", "../images/plus.png");
                    $(this).closest("tr").next().remove();
                });
                
                $("[id*=chkSelectAllCustomer1]").live("click", function () {
                    var chkHeader = $(this);
                    var grid = $(this).closest("table");
                    $("input[type=checkbox]", grid).each(function () {
                        if (chkHeader.is(":checked")) {
                            $(this).attr("checked", "checked");
                            $("td", $(this).closest("tr")).addClass("selected");
                        } else {
                            $(this).removeAttr("checked");
                            $("td", $(this).closest("tr")).removeClass("selected");
                        }
                    });
                });
                
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
                //-------- end Covering letter-------------------------------- 

                function Confirm() {

                    if (confirm("Do you want to Approve Bill Dispatch?"))
                        return true;
                    else
                        return false;

                }
                function Reject() {

                    if (confirm("Do you want to Reject Bill Dispatch?"))
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

                function toggleDiv1(chk, FUid, CusValId, RFVFileUpload) {
                    var checkboxId = document.getElementById(chk);
                    var fileUploadId = document.getElementById(FUid);
                    var CustomValidatorList = document.getElementById(CusValId);
                    var fileUploadRequired = document.getElementById(RFVFileUpload);

                    if (checkboxId.checked == true) {
                        checkboxId.checked = true;
                        fileUploadId.disabled = false;
                        ValidatorEnable(CustomValidatorList, true);

                        // File Upload Required
                        if (fileUploadRequired != null) {

                            ValidatorEnable(fileUploadRequired, true);
                        }

                    }
                    else if (checkboxId.checked == false) {

                        checkboxId.checked = false;
                        fileUploadId.disabled = true;
                        ValidatorEnable(CustomValidatorList, false);

                        // File Upload Not Required
                        if (fileUploadRequired != null) {
                            ValidatorEnable(fileUploadRequired, false);
                        }

                    }

                }
            </script>
            <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" ShowAllPageIds="True" />
            <CR:CrystalReportViewer ID="CrystalReportViewer2" runat="server" DisplayPage="true" EnableDrillDown="true" AutoDataBind="true" ShowAllPageIds="True" />
            <div align="center">
                <asp:Label ID="lblShowError" runat="server"></asp:Label>
            </div>
            <cc1:TabContainer ID="TabJobDetail" runat="server" AutoPostBack="True" ActiveTabIndex="0" CssClass="Tab" OnActiveTabChanged="TabJobDetail_ActiveTabChanged">
                <cc1:TabPanel ID="TabPCDBilling" runat="server" TabIndex="0" HeaderText="Non Receive">
                    <ContentTemplate>
                        <div align="center">
                            <asp:Label ID="lblreceivemsg" runat="server"></asp:Label>
                            <asp:Label ID="lblMsgforNonReceived" runat="server"></asp:Label>
                        </div>
                        <div class="fleft">
                            <table>
                                <tr>
                                    <td>
                                        <uc1:DataFilter1 ID="DataFilter1" runat="server" />
                                    </td>
                                    <td valign="top">
                                        <asp:Button ID="btnReceive" runat="server" Text="Receive" CssClass="buttons"
                                            ToolTip="Receive File" OnClick="btnReceive_Click" /></td>
                                    <td>
                                        <asp:LinkButton ID="lnknonreceive" runat="server" OnClick="lnkNonreceiveExcel_Click" data-tooltip="&nbsp; Export To Excel"><asp:Image ID="img1" runat="server" ImageUrl="~/Images/Excel.jpg" />
</asp:LinkButton></td>

                                </tr>
                            </table>
                        </div>
                        <asp:GridView ID="gvNonRecievedJobDetail" runat="server" AutoGenerateColumns="False"
                            CssClass="table" DataKeyNames="JobId" AllowPaging="True"
                            AllowSorting="True" Width="100%" DataSourceID="PCDSqlDataSource"
                            PageSize="20" OnRowCommand="gvNonRecievedJobDetail_RowCommand"
                             OnRowDataBound="gvNonRecievedJobDetail_RowDataBound"
                            OnPreRender="gvNonRecievedJobDetail_PreRender">   <%----%>
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
                                <asp:TemplateField HeaderText="E-Bill">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEBillOne" runat="server" Text="E-Bill Update" CommandName="EBillOne"
                                            ToolTip="Update E-Bill Details" CommandArgument='<%#Eval("JobId")  + ";" + Eval("JobRefNo")%>' CausesValidation="false"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BS Job No" SortExpression="JobRefNo">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkJobNo" runat="server" Text='<%#Eval("JobRefNo") %>' CommandName="select" CommandArgument='<%#Eval("JobId")%>' Enabled="false"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" Visible="False" />
                                <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                                <asp:BoundField DataField="JobType" HeaderText="Job Type" />
                                <asp:BoundField DataField="LastDispatchDate" HeaderText="Dispatch Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="LastDispatchDate" />
                                <asp:BoundField DataField="ShippingLineDate" HeaderText="Document HandOver Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ShippingLineDate" />
                                <asp:BoundField DataField="EBillDate" HeaderText="E-Bill Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="EBillDate" />
                                <asp:BoundField DataField="Aging1" HeaderText="Aging I" SortExpression="Aging1" />
                                <asp:BoundField DataField="Aging2" HeaderText="Aging II" SortExpression="Aging2" />
                                <asp:BoundField DataField="Aging3" HeaderText="Aging III" SortExpression="Aging3" />
                                <asp:TemplateField HeaderText="rulename">
                                    <ItemTemplate>
                                        <asp:Label ID="rulename" runat="server" Text='<%#Eval("rulename")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="PCDSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="GetPendingPCDToFinalDispatch" SelectCommandType="StoredProcedure"
			DataSourceMode="DataSet" EnableCaching="true" CacheDuration="600" CacheKeyDependency="MyCacheDependency">
				
                            <SelectParameters>
                                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                                <asp:QueryStringParameter Name="IsReceived" DbType="String"
                                    QueryStringField="rulename" DefaultValue='1' />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="TabPCDBilling1" runat="server" TabIndex="1" HeaderText="Receive">
                    <ContentTemplate>
                        <div align="center">
                            <asp:Label ID="lblMsgforApproveReject" runat="server"></asp:Label>
                            <asp:Label ID="lblMsgforReceived" runat="server"></asp:Label>
                        </div>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="true" ValidationGroup="Required" CssClass="errorMsg" />
                        <asp:ValidationSummary ID="ValSummary" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="validateform" CssClass="errorMsg" EnableViewState="false" />
                        <asp:Panel ID="pnlFilter2" runat="server">
                            <div class="fleft">
                                <table>
                                    <tr>
                                        <td>
                                            <uc2:DataFilter2 ID="DataFilter2" runat="server" />
                                        </td>
                                        <%--     start covering letter--%>
                                        <td valign="baseline">
                                            <asp:Button ID="btnApprove" runat="server" Text="Approve" OnClick="btnApprove_Click" />
                                        </td>
                                        <td valign="baseline">
                                            <asp:Button ID="btnCoveringLetter" runat="server" Text="CoveringLetter" OnClick="btnCoveringLetter_Click" />
                                        </td>

                                        <td valign="baseline">
                                            <asp:Button ID="btnMyPaccoAWBGeneration" runat="server" Text="MyPacco Dispatch" OnClick="btnMyPaccoAWBGeneration_Click" />
                                        </td>
                                        <%-- end covering letter--%>

                                        <td>
                                            <asp:LinkButton ID="lnkreceive" runat="server" OnClick="lnkreceiveExcel_Click" data-tooltip="&nbsp; Export To Excel">
                                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Excel.jpg" />
                                            </asp:LinkButton></td>
                                    </tr>
                                </table>
                            </div>

                            <div id="div1">
                                <cc1:ModalPopupExtender ID="ModalPopupCoveringLetter" runat="server" CacheDynamicResults="false" DropShadow="False" PopupControlID="PanelCoveringletter" TargetControlID="lnkDummy1">
                                </cc1:ModalPopupExtender>
                                <asp:Panel ID="PanelCoveringletter" Style="display: none" runat="server">
                                    <fieldset class="ModalPopupPanel">
                                        <div title="Reject Details" class="header">
                                            <textbox>Covering Letter List</textbox>
                                        </div>
                                        <div class="AutoExtenderList">
                                            <table id="table2" runat="server" class="table" style="background-color: ThreeDHighlight">
                                                <tr>
                                                    <td>
                                                        <%--  <asp:DropDownList ID="drpcoveringletter" runat="server" DataSourceId="filldropdown" DataValueField="Id" DataTextField="letter_name">
                                        </asp:DropDownList>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" align="center">
                                                        <asp:Button ID='lnkGenerate' runat="server" Text="Generate" ToolTip="Generate Covering Letter" CommandName="lnkGenerate" CommandArgument='<%#Eval("JobId") %>'></asp:Button>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID='lnkCancel' runat="server" Text="Cancel" CommandName="Cancel" CommandArgument='<%#Eval("JobId") %>'></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </fieldset>
                                </asp:Panel>
                            </div>

                            <%------Start Covering Letter-----------------------------%>

                            <asp:GridView ID="gvcustomer" runat="server" AutoGenerateColumns="False" CssClass="table"
                                PagerStyle-CssClass="pgr" DataKeyNames="Customerid" AllowPaging="True" AllowSorting="True"
                                Width="80%" PageSize="20" PagerSettings-Position="TopAndBottom" DataSourceID="PcdSqlDataCustomer"
                                OnRowDataBound="OnRowDataBound"  OnRowCommand="gvcustomer_RowCommand">  <%-- --%>
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkSelectAllCustomer" align="center" Text="Select All" ToolTip="Check All"
                                                runat="server" onclick="GridSelectAllColumn(this);" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <img alt="" style="cursor: pointer" src="../images/plus.png" />
                                            <asp:Panel ID="pnlCustomer" runat="server" Style="display: none">
                                                <asp:GridView ID="gvRecievedJobDetail" runat="server" AutoGenerateColumns="False"
                                                    CssClass="table" PagerStyle-CssClass="pgr" DataKeyNames="JobId" AllowPaging="false"
                                                    PagerSettings-Position="TopAndBottom" AllowSorting="True" Width="100%" PageSize="5"
                                                    OnRowDataBound="gvRecievedJobDetail_RowDataBound" OnRowCommand="gvRecievedJobDetail_RowCommand">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sl">
                                                            <ItemTemplate>
                                                                <%#Container.DataItemIndex +1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="chkSelectAllCustomer1" align="center" ToolTip="Check All" runat="server" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkjoball" runat="server" ToolTip="Check"></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Hold">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgbtnHoldJob" runat="server" ImageUrl="~/Images/UnlockImg.png"
                                                                    CommandArgument='<%#Eval("JobId")  + ";" + Eval("JobRefNo") + ";" + Eval("JobType")%>' CommandName="Hold" Width="18px" Height="18px" ToolTip="Hold Job."
                                                                    Style="padding-right: 1px; margin-right: 0px; padding-left: 1px"></asp:ImageButton>
                                                                <asp:ImageButton ID="imgbtnUnholdJob" runat="server" ImageUrl="~/Images/LockImg.png"
                                                                    CommandArgument='<%#Eval("JobId") + ";" + Eval("JobRefNo") + ";" + Eval("JobType")%>' CommandName="Unhold" Width="18px" Height="18px" ToolTip="Unhold Job."
                                                                    Style="padding-right: 1px; margin-right: 0px; padding-left: 1px"></asp:ImageButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="HoldRemark" HeaderText="Hold Reason" ReadOnly="true" />
                                                        <asp:BoundField DataField="JobTypeName" HeaderText="Job Type" ReadOnly="true" />
                                                        <asp:TemplateField HeaderText="BS Job No" SortExpression="JobRefNo">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkJobNo" runat="server" Text='<%#Eval("JobRefNo") %>' CommandArgument='<%#Eval("JobId")%>' Enabled="false"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" Visible="false" ReadOnly="true" />
                                                        <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" Visible="false" ReadOnly="true" />
                                                        <asp:BoundField DataField="LastDispatchDate" HeaderText="Dispatch Date" DataFormatString="{0:dd/MM/yyyy}"
                                                            SortExpression="LastDispatchDate" ReadOnly="true" />
                                                        <asp:BoundField DataField="ShippingLineDate" HeaderText="Document HandOver Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ShippingLineDate" ReadOnly="true" />
                                                        <asp:BoundField DataField="ReceivedDate" HeaderText="Received Date" DataFormatString="{0:dd/MM/yyyy}"
                                                            SortExpression="ReceivedDate" Visible="false" ReadOnly="true" />
                                                        <asp:BoundField DataField="Aging1" HeaderText="Aging I" SortExpression="Aging1" ReadOnly="true" />
                                                        <asp:BoundField DataField="Aging2" HeaderText="Aging II" SortExpression="Aging2" ReadOnly="true" />
                                                        <asp:BoundField DataField="Aging3" HeaderText="Aging III" SortExpression="Aging3" ReadOnly="true" />
                                                        <asp:TemplateField HeaderText="rulename">
                                                            <ItemTemplate>
                                                                <asp:Label ID="rulename" runat="server" Text='<%#Eval("rulename")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Reject">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkReject" runat="server" Text="Reject" CommandName="Reject"
                                                                    ToolTip="Reject" CommandArgument='<%#Eval("JobId")+";"+ Eval("JobRefNo") %>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="txtUploadPath" runat="server" Text='<%#Eval("JobId")+";"+ Eval("DocPath")+";"+ Eval("Filename")%>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="View Covering Letter">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkDocument" runat="server"
                                                                    CommandName="coveringletter" CommandArgument='<%#Eval("JobId")+";"+ Eval("DocPath")+";"+ Eval("Filename")%>'>
                                                                </asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Upload & Mail Send" ControlStyle-Width="100px">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkFileUpload" runat="server" Text="Upload/ Mail Send"
                                                                    CommandName="Mail" CommandArgument='<%#Eval("JobId") + ";" + Eval("JobRefNo") + ";" +Eval("Customer")  %>'></asp:LinkButton>
                                                                <asp:HiddenField ID="hdnJobId" runat="server" Value='<%#Eval("JobId") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" ReadOnly="true" Visible="false" />
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Customer" HeaderText="Customer" />
                                    <asp:BoundField DataField="noofjobs" HeaderText="TotalJobs" ReadOnly="False" />
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomerid" runat="server" Text='<%#Eval("Customerid")%>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Instruction">
                                        <ItemTemplate>
                                             <asp:LinkButton ID="lnkInstruction" Text="Show" runat="server" CommandName="Instruction" CommandArgument='<%#Eval("Customerid")%>'> </asp:LinkButton>
                                             <%--<asp:Label ID="lbl" runat="server" Text='<%#Eval("Customerid")%>' Visible="false"></asp:Label>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>

                            <asp:SqlDataSource ID="PcdSqlDataCustomer" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="GetCutomerPCDToFinalDispatch" SelectCommandType="StoredProcedure" 
				DataSourceMode="DataSet" EnableCaching="true" CacheDuration="200" CacheKeyDependency="MyCacheDependency">
				
                                <SelectParameters>
                                    <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                                    <asp:QueryStringParameter Name="IsReceived" DbType="string" QueryStringField="rulename"
                                        DefaultValue='0' />
                                </SelectParameters>
                            </asp:SqlDataSource>

                            <div id="divPreAlertEmail">
                                <cc1:ModalPopupExtender ID="ModalPopupEmail" runat="server" CacheDynamicResults="false"
                                    DropShadow="False" PopupControlID="Panel2Email" TargetControlID="lnkDummy">
                                </cc1:ModalPopupExtender>

                                <asp:Panel ID="Panel2Email" runat="server" CssClass="ModalPopupPanel">
                                    <div class="header">
                                        <div class="fleft">
                                            Customer Email
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
                                            <div>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBoxList ID="chkDraft" runat="server" OnSelectedIndexChanged="chkDraft_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Value="1" Text="User Define Draft Email"  style="font-weight:bold"></asp:ListItem>
                                                </asp:CheckBoxList>
                                                <br />
                                               &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtRemark" TabIndex="1" runat="server" contentEditable="true" TextMode="MultiLine" Width="90%" Height="100px" Style="border: 1px solid #8c8b8b; "></asp:TextBox>
                                            </div>
                                            <fieldset style="width:700px;">
                                                <legend>Document Attachment</legend>
                                                <asp:GridView ID="gvDocAttach" runat="server" AutoGenerateColumns="False" Width="100%"
                                                    DataKeyNames="DocId" CssClass="table"
                                                    CellPadding="4" PagerStyle-CssClass="pgr" PageSize="20" PagerSettings-Position="TopAndBottom">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sl">
                                                            <ItemTemplate>
                                                                <%#Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Check">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkAttach" runat="server" Checked="false" />
                                                                <asp:HiddenField ID="hdnDocPath" runat="server" Value='<%#Eval("DocPath") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="DocName" HeaderText="Document Name" SortExpression="DocName" />
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

                            <div id="divFileUpload">
                                <cc1:ModalPopupExtender ID="MpeFileUpload" runat="server" CacheDynamicResults="false"
                                    DropShadow="False" PopupControlID="PanelFileUpload" TargetControlID="lnkDummy2">
                                </cc1:ModalPopupExtender>

                                <asp:Panel ID="PanelFileUpload" runat="server" CssClass="ModalPopupPanel" Height="75%" Width="55%">
                                    <div class="header">
                                        <div class="fleft">
                                            <asp:Label ID="lblTitle" runat="server"></asp:Label>
                                        </div>
                                        <div class="fright">
                                            <asp:ImageButton ID="ImgClose" ImageUrl="~/Images/delete.gif" runat="server" ToolTip="Close" />
                                        </div>
                                    </div>
                                    <%--<div class="m"></div>--%>
                                    <div id="Div4" runat="server" style="max-height: 500px; max-width: 780px; overflow: auto;">
                                        <asp:Label ID="lblMsg" runat="server">&nbsp;&nbsp; </asp:Label><br />
                                        &nbsp;&nbsp; Customer Name :-
                                        <asp:Label ID="lblcustName" runat="server"></asp:Label><br />
                                        &nbsp;&nbsp; Job Ref No :-
                                        <asp:Label ID="lblJobRegNo" runat="server"></asp:Label><br />
                                        <div class="m">
                                            <table border="0" cellpadding="0" cellspacing="0" bgcolor="white" width="100%">
                                                <tr id="trrptDoc" runat="server">
                                                    <td>
                                                        <asp:Repeater ID="rptDocument" runat="server"
                                                            OnItemDataBound="rpDocument_ItemDataBound">
                                                            <%--DataSourceID="DocTypeSqlDataSource"--%>
                                                            <%----%>
                                                            <HeaderTemplate>
                                                                <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                    <tr bgcolor="#FF781E">
                                                                        <th>Sl
                                                                        </th>
                                                                        <th>Name
                                                                        </th>
                                                                        <th>File Upload
                                                                        </th>
                                                                        <%--<th>Supporting Doc
                                                                        </th>--%>
                                                                    </tr>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td>
                                                                        <%#Container.ItemIndex +1%>
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkDocType" Text='<%#DataBinder.Eval(Container.DataItem,"sName") %>'
                                                                            runat="server" Style="visibility: visible" />&nbsp;
                                                                        <asp:HiddenField ID="hdnDocId" Value='<%#DataBinder.Eval(Container.DataItem,"lId") %>'
                                                                            runat="server"></asp:HiddenField>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RequiredFieldValidator ID="RFVFile" runat="server" ControlToValidate="fuDocument" 
                                                                            InitialValue="" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Required"
                                                                            ValidationGroup="Required" Enabled="false"></asp:RequiredFieldValidator>
                                                                        <asp:FileUpload ID="fuDocument" runat="server" Enabled="false" ViewStateMode="Enabled" />
                                                                        <asp:RegularExpressionValidator
                                                                            id="RegularExpressionValidator1" runat="server" ValidationGroup="Required"
                                                                            ErrorMessage="Only PDF files are allowed!" Display="Dynamic"  SetFocusOnError="true"
                                                                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" type="reset"
                                                                            ControlToValidate="fuDocument" CssClass="text-red"></asp:RegularExpressionValidator>
                                                                    </td>
                                                                    
                                                                </tr>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                </table>
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </td>
                                                </tr>
                                                <tr id="trBillDispatchDocDetail" runat="server">
                                                    <td>
                                                        <asp:GridView ID="gvBillDispatchDocDetail" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="table"
                                                            OnRowCommand="gvBillDispatchDocDetail_RowCommand"
                                                            CellPadding="4" PagerStyle-CssClass="pgr" PageSize="20" PagerSettings-Position="TopAndBottom">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Sl">
                                                                    <ItemTemplate>
                                                                        <%#Container.DataItemIndex + 1 %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="DocName" HeaderText="Document Name" SortExpression="DocName" />
                                                                <asp:BoundField DataField="UserName" HeaderText="Upload By" />
                                                                <asp:BoundField DataField="UploadedDate" HeaderText="Upload Date" DataFormatString="{0:dd/MM/yyyy}" />
                                                                <asp:TemplateField HeaderText="Download">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"
                                                                            CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr id="trMailDetail" runat="server">
                                                    <td>
                                                        <asp:GridView ID="GridViewMailDetail" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="table"
                                                            CellPadding="4" PagerStyle-CssClass="pgr" PageSize="20" PagerSettings-Position="TopAndBottom">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Sl">
                                                                    <ItemTemplate>
                                                                        <%#Container.DataItemIndex + 1 %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="SentTo" HeaderText="Mail Send To" SortExpression="SentTo" />
                                                                <asp:BoundField DataField="SentCC" HeaderText="Mail Send CC" SortExpression="SentCC" />
                                                                <asp:BoundField DataField="SendBy" HeaderText="Mail Send By" DataFormatString="{0:dd/MM/yyyy}" />
                                                                <asp:BoundField DataField="SendDate" HeaderText="Mail Send Date" />
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                            <asp:Button ID="btnSave" runat="server" Text="Save File" OnClick="btnSave_Click" ValidationGroup="Required"
                                                OnClientClick="if (!Page_ClientValidate('Required')){ return false; } this.disabled = true; this.value = 'Saving...';" UseSubmitBehavior="false" />
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:LinkButton ID="lnkPreAlertEmailDraft" runat="server" Text="View & Send Customer Email" OnClick="lnkPreAlertEmailDraft_Click" Visible="false"></asp:LinkButton>
                                        </div>
                                        <%--<div id="div4" runat="server" style="margin-left: 10px;">
                                        </div>--%>
                                    </div>
                                </asp:Panel>
                                <asp:LinkButton ID="lnkDummy2" runat="server"></asp:LinkButton>
                                <!--Customer Email Draft End -->
                            </div>


                            <%--------end Covering Letter-------------------------------%>

                            <div>
                                <asp:LinkButton ID="lnkDummy1" runat="server" Text=""></asp:LinkButton>
                            </div>
                        </asp:Panel>

                        <%------ Instruction Popup Show --------%>
                         <asp:Button ID="btnInstruct" ValidationGroup="Required" runat="server" Style="display: none" />
                        <cc1:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="btnInstruct" CancelControlID="btnCancelInstruct" PopupControlID="PanelInstruction"></cc1:ModalPopupExtender>
                        <asp:Panel ID="PanelInstruction" Style="display: none" runat="server" width="45%" Height="270px" backcolor="white"  BorderStyle="Solid" BorderWidth="1px" >
                            <center>
                                    <table border="0" cellpadding="0" cellspacing="0" width="90%" Height="20px">
                                        <tr>
                                            <td width="5%"></td>
                                            <td width="80%"><center><h3>Billing Instruction</h3></center></td>
                                            <td width="10%">
                                                  <asp:ImageButton ID="btnCancelInstruct" ImageUrl="~/Images/delete.gif" runat="server" ToolTip="Close" />
                                             </td>

                                        </tr>   
                                    </table> 
                                <br />    
                                   <table border="0" cellpadding="0" cellspacing="0" width="90%" Height="20px">                              
                                        <tr>
                                            <td colspan="3">
                                                <%--<asp:Label ID="Label2" runat="server" Text="Reason for Pendency" ForeColor="Black" Font-Size="9"></asp:Label>--%>
                                                <asp:TextBox ID="txtInstruction" runat="server" width="100%" height="200px"  ReadOnly="true" backcolor="#FFF5E1" TextMode="MultiLine" ></asp:TextBox>
                                            </td>                                            
                                        </tr>                                       
                                    </table>
                                </center>
                        </asp:Panel>
                        <%---------- END Instruction -----------%>


                        <asp:Button ID="modelPopup" ValidationGroup="Required" runat="server" Style="display: none" />
                        <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="modelPopup" CancelControlID="btnCancel" PopupControlID="Panel1"></cc1:ModalPopupExtender>
                        <asp:Panel ID="Panel1" Style="display: none" runat="server">
                            <fieldset class="ModalPopupPanel">
                                <div title="Reject Details" class="header">
                                    <textbox>Reject Details</textbox>
                                </div>
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
                                                                            runat="server" TabIndex="13" Width="100" ForeColor="Black" Font-Size="9" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblReason" Text="Remarks: " runat="server" ForeColor="Black" Font-Size="9" />
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
                                                                        <asp:TextBox ID="txtReason" runat="server" Text="" ForeColor="Black" Font-Size="9" ValidationGroup="validateform"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="RFVRejectReason" ControlToValidate="txtReason" runat="server" ValidationGroup="validateform" SetFocusOnError="true"
                                                                            Text="*" Font-Bold="true" ErrorMessage='<%#DataBinder.Eval(Container.DataItem,"ReasonforPendency") + "- Please Enter Remark."%>' Enabled="false"></asp:RequiredFieldValidator></td>
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
                                                                            <asp:CompareValidator ID="Comval" runat="server" ControlToValidate="txt" Operator="NotEqual" ValueToCompare="0" Type="Integer" ValidationGroup="validateform" Text="*" SetFocusOnError="true" ErrorMessage='<%#DataBinder.Eval(Container.DataItem,"ReasonforPendency") + "- Please select porper UserName."%>'
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
                                                <asp:Button ID="btnReject" runat="server" Text="Done" OnClick="btnReject_Click" ValidationGroup="validateform" ToolTip="Reject" OnClientClick="if (!Reject()) return false;" />
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
             <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </fieldset>
                        </asp:Panel>
                        <asp:HiddenField ID="hdnJobId" runat="server" />

                        <%--start Covering letter--%>
                        <asp:HiddenField ID="hdnpath" runat="server" />
                        <%--end Covering letter--%>

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
                       

                        <asp:SqlDataSource ID="PCDDocumentSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="GetPCDDocumentByRecieved" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                <asp:Parameter Name="DocumentForType" DefaultValue="3" />
                            </SelectParameters>
                        </asp:SqlDataSource>

                        <asp:SqlDataSource ID="SqlDataSourceMailDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="GetBillDispatchMailDetail" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="JobId" SessionField="JobId" />
                            </SelectParameters>
                        </asp:SqlDataSource>

                        <asp:SqlDataSource ID="SqlDataSourceBillDispatchDoc" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="GetBillDispatchDocDetail" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="JobId" SessionField="JobId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <%--start Covering letter--%>
                        <asp:SqlDataSource ID="PCDSqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="GetReceivePCDToFinalDispatch" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                                <asp:Parameter Name="Customerid" DefaultValue="0" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <%--end Covering letter--%>
                        <asp:SqlDataSource ID="DsReasonforpendency" runat="server"
                            ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="GetReasonforpendency" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                        <asp:SqlDataSource ID="sqldatasourcelrdctype" runat="server"
                            ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="Getlrdctype" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                    </ContentTemplate>
                </cc1:TabPanel>
            </cc1:TabContainer>

            <div style="text-align: right">
                <br />
                <asp:Repeater ID="Rptpriorities" runat="server" DataSourceID="SqlDataSourcepriorities" OnItemDataBound="Rptpriorities_ItemDataBound">
                    <ItemTemplate>
                        <asp:TextBox ID="txtpriorities" runat="server" Width="0.5%"></asp:TextBox>
                        <asp:Label ID="lblpriorities" runat="server" Text='<%#Eval("prioritiesName")%>'></asp:Label>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <!-- START : MODAL POP-UP FOR HOLD EXPENSE  -->
            <div id="divHoldExpense">
                <cc1:ModalPopupExtender ID="mpeHoldExpense" runat="server" CacheDynamicResults="false"
                    PopupControlID="pnlHoldExpense" CancelControlID="imgbtnHoldExp" TargetControlID="hdnHoldExp" BackgroundCssClass="modalBackground" DropShadow="true">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pnlHoldExpense" runat="server" CssClass="ModalPopupPanel" Width="600px">
                    <div class="header">
                        <div class="fleft">
                            <asp:Label ID="lblHoldPopupName" runat="server"></asp:Label>
                            <asp:HiddenField ID="hdnJobRefNo" runat="server" />
                            <asp:HiddenField ID="hdnJobId_hold" runat="server" />
                        </div>
                        <div class="fright">
                            <asp:ImageButton ID="imgbtnHoldExp" ImageUrl="../Images/delete.gif" runat="server"
                                ToolTip="Close" />
                        </div>
                    </div>
                    <!-- Lists Of All Documents -->
                    <div id="Div3" runat="server" style="max-height: 250px; overflow: auto; padding: 5px">
                        <asp:Label ID="lblError_HoldExp" runat="server"></asp:Label>
                        <div>
                            <asp:FormView ID="fvHoldJobDetail" runat="server" DataKeyNames="JobId" Width="100%">
                                <ItemTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                        <tr>
                                            <td>BS Job Number</td>
                                            <td>
                                                <asp:Label ID="lblBSJobNo" runat="server" Text='<%#Eval("JobRefNo") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Customer</td>
                                            <td>
                                                <asp:Label ID="lblCustomer" runat="server" Text='<%#Eval("Customer") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Last Dispatch Date</td>
                                            <td>
                                                <asp:Label ID="lblLastDispatchDate" runat="server" Text='<%#Eval("LastDispatchDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:FormView>
                        </div>
                        <br />
                        &nbsp;
                        <asp:RequiredFieldValidator ID="rfvReason" runat="server" ControlToValidate="txtHoldReason" SetFocusOnError="true" Display="Dynamic"
                            ForeColor="Red" ErrorMessage="* Enter Reason" ValidationGroup="vgAddHoldExpense" Font-Bold="true"></asp:RequiredFieldValidator>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td>Reason                                      
                                </td>
                                <td>
                                    <asp:TextBox ID="txtHoldReason" runat="server" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Button ID="btnHoldJob" runat="server" ValidationGroup="vgAddHoldExpense"
                                        OnClick="btnHoldJob_OnClick" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <%--   <asp:SqlDataSource ID="DataSourceHoldJob" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="GetJobDetailById" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="hdnJobId_hold" Name="JobId" PropertyName="Value" />
                        </SelectParameters>
                    </asp:SqlDataSource>--%>
                </asp:Panel>
            </div>
            <div>
                <asp:LinkButton ID="hdnHoldExp" runat="server"></asp:LinkButton>
            </div>
            <!-- END   : MODAL POP-UP FOR HOLD EXPENSE  -->
            <!-- START : MODAL POP-UP E-Bill  -->
            <div id="divEbill">
                <cc1:ModalPopupExtender ID="ModalPopupEBill" runat="server" CacheDynamicResults="false"
                    PopupControlID="pnlEBill" CancelControlID="imgEBillPopup" TargetControlID="lnkPopupEbill" BackgroundCssClass="modalBackground" DropShadow="true">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pnlEBill" runat="server" CssClass="ModalPopupPanel" Width="600px">
                    <div class="header">
                        <div class="fleft">
                            <asp:Label ID="lblMsgEBill" runat="server"></asp:Label>
                            <asp:HiddenField ID="hdnEBillJobId" runat="server" />
                        </div>
                        <div class="fright">
                            <asp:ImageButton ID="imgEBillPopup" ImageUrl="../Images/delete.gif" runat="server"
                                ToolTip="Close" />
                        </div>
                    </div>
                    <!-- E-Bill Detail -->
                    <div id="Div2" runat="server" style="max-height: 250px; overflow: auto; padding: 5px">
                        <asp:Label ID="Label2" runat="server"></asp:Label>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                            <tr>
                                <td>
                                    Job Ref No
                                </td>
                                <td>
                                    <asp:Label ID="lblEBillJobRefNo" runat="server"></asp:Label> 
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>E-Bill Sent On
                                    <cc1:CalendarExtender ID="calEBillDate" runat="server" Enabled="True" EnableViewState="False"
                                        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="imgEBillDate"
                                        PopupPosition="BottomRight" TargetControlID="txtEBillDate">
                                    </cc1:CalendarExtender>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEBillDate" runat="server" Width="100px"></asp:TextBox>
                                    <asp:Image ID="imgEBillDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                                    <cc1:MaskedEditExtender ID="MskEdtEBillDate" TargetControlID="txtEBillDate" Mask="99/99/9999" MessageValidatorTip="true" 
                                        MaskType="Date" AutoComplete="false" runat="server"></cc1:MaskedEditExtender>
                                    <cc1:MaskedEditValidator ID="MskEdtValBillDate" ControlExtender="MskEdtEBillDate" ControlToValidate="txtEBillDate" IsValidEmpty="false" 
                                        InvalidValueMessage="E-Bill Date is invalid" MinimumValueMessage="E-Bill Date Is Invalid" MaximumValueMessage="E-Bill Date Is Invalid"
                                        MaximumValueBlurredMessage="Invalid Date" MinimumValueBlurredText="Invalid Date"
                                        MinimumValue="01/01/2020" SetFocusOnError="true" Runat="Server" ValidationGroup="vgEbillRequired"></cc1:MaskedEditValidator>
                                </td>
                                <td>
                                    Remark
                                    <asp:RequiredFieldValidator ID="rfvEbillRemark" runat="server" ControlToValidate="txtEBillRemark" SetFocusOnError="true" Display="Dynamic"
                                        ForeColor="Red" ErrorMessage="Required" ValidationGroup="vgEbillRequired" Font-Bold="true"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEBillRemark" runat="server" TextMode="MultiLine" MaxLength="200" Rows="3"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Button ID="btnEBillUpdate" runat="server" Text="Update E-Bill Detail" ValidationGroup="vgEbillRequired" OnClick="btnEBillUpdate_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    
                </asp:Panel>
            </div>
            <div>
                <asp:LinkButton ID="lnkPopupEbill" runat="server"></asp:LinkButton>
            </div>
            <!-- END   : MODAL POP-UP E-Bill  -->
            <asp:SqlDataSource ID="SqlDataSourcepriorities" runat="server"
                ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="Getpriorities" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:content>




