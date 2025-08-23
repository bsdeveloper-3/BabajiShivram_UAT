<%@ Page Title="Fund Request" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="FillJobDetails.aspx.cs"
    Inherits="AccountExpense_FillJobDetails" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Scriptmanager id="ScriptManager1" runat="server" scriptmode="Release">
    </asp:Scriptmanager>

    <script type="text/javascript">
        function OnJobSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnJobId').value = results.JobId;            
            $get('ctl00_ContentPlaceHolder1_hdnModuleId').value = results.ModuleId;
        }
    </script>

    <asp:UpdatePanel ID="upFillDetails" runat="server">
        <ContentTemplate>
            <div>
                <div align="center">
                    <asp:HiddenField ID="hdnJobId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnModuleId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnNewPaymentLid" runat="server" Value="0" />
                    <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                    <asp:ValidationSummary ID="vsFillDetails" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
                </div>

                <fieldset>
                    <legend>Fill Job Detail</legend>
                    <div id="divInstruction" class="info" runat="server">
                    </div>
                    <div class="m clear">
                        <asp:Button ID="btnSubmit" Text="Save" OnClick="btnSubmit_Click" runat="server" ValidationGroup="Required"
                            TabIndex="7" />
                        <asp:Button ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="false" TabIndex="8"
                            runat="server" />
                    </div>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td width="20%">Job Number
                            <asp:RequiredFieldValidator ID="rfvJobNo" runat="server" ValidationGroup="Required"
                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtJobNumber"
                                Text="*" ErrorMessage="Please Select Job Number."></asp:RequiredFieldValidator>
                            </td>
                            <td width="80%" colspan="3" >
                                <asp:TextBox ID="txtJobNumber" Width="160px" runat="server" ToolTip="Enter Job Number."
                                    CssClass="SearchTextbox" placeholder="Search" TabIndex="1" AutoPostBack="true" OnTextChanged="txtJobNumber_TextChanged"></asp:TextBox>
                                <div id="divwidthCust_Loc" runat="server">
                                </div>
                                <cc1:autocompleteextender id="JobDetailExtender" runat="server" targetcontrolid="txtJobNumber"
                                    completionlistelementid="divwidthCust_Loc" servicepath="~/WebService/JobNumberAutoComplete.asmx"
                                    servicemethod="GetCompletionList" minimumprefixlength="5" behaviorid="divwidthCust_Loc"
                                    contextkey="5569" usecontextkey="True" onclientitemselected="OnJobSelected"
                                    completionlistcssclass="AutoExtender" completionlistitemcssclass="AutoExtenderList"
                                    completionlisthighlighteditemcssclass="AutoExtenderHighlight">
                                </cc1:autocompleteextender>
                            </td>
                          
                        </tr>
                        <tr>
                            <td>Consignee
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="lblConsignee" runat="server" Enabled="false" Width="290px"></asp:TextBox>
                            </td>                         
                        </tr>
                        <tr>
                            <td>Branch</td>
                            <td>
                                <asp:DropDownList ID="ddlBranch" runat="server" AppendDataBoundItems="true" DataSourceID="DataSourceBranch"
                                    DataTextField="BranchName" DataValueField="lid" TabIndex="2" ToolTip="Select Branch." Enabled="false">
                                </asp:DropDownList>
                            </td>
                           <td>Planning Date
                                <cc1:calendarextender id="calPlanningDate" runat="server" firstdayofweek="Sunday" popupbuttonid="imgPlanningDate"
                                    format="dd/MM/yyyy" popupposition="BottomRight" targetcontrolid="txtPlanningDate">
                                </cc1:calendarextender>
                                <cc1:maskededitextender id="meePlanningDate" targetcontrolid="txtPlanningDate" mask="99/99/9999" messagevalidatortip="true"
                                    masktype="Date" autocomplete="false" runat="server">
                                </cc1:maskededitextender>
                                <cc1:maskededitvalidator id="mevPlanningDate" controlextender="meePlanningDate" controltovalidate="txtPlanningDate" isvalidempty="true"
                                    invalidvaluemessage="Planning Date is invalid" invalidvalueblurredmessage="Invalid Planning Date" setfocusonerror="true"
                                    minimumvaluemessage="Invalid Planning Date" maximumvaluemessage="Invalid Date" minimumvalue="01/01/2015" maximumvalue="31/12/2025"
                                    runat="Server"></cc1:maskededitvalidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPlanningDate" runat="server" Width="125px" placeholder="dd/mm/yyyy" TabIndex="3" ToolTip="Enter Planning Date."></asp:TextBox>
                                <asp:Image ID="imgPlanningDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            </td>
                        </tr>
                      
                        <tr>
                            <td>Type of Expense
                            <asp:RequiredFieldValidator ID="rfvExpenseType" runat="server" ValidationGroup="Required"
                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddlExpenseType" InitialValue="0"
                                Text="*" ErrorMessage="Please Select Type of Expense."> </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlExpenseType" runat="server" AppendDataBoundItems="true" DataSourceID="DataSourceExpense"
                                    DataTextField="sName" DataValueField="lid" TabIndex="5" ToolTip="Select Type Of Expense." AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlExpenseType_OnSelectedIndexChanged">
                                </asp:DropDownList>
                            </td>

                            <td>Type of Payment
                            <asp:RequiredFieldValidator ID="rfvpaytype" runat="server" ValidationGroup="Required"
                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddlPaymentType" InitialValue="0"
                                Text="*" ErrorMessage="Please Select Type of Payment."> </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPaymentType" runat="server" AppendDataBoundItems="true" DataSourceID="DataSourcePaymentType"
                                    DataTextField="sName" DataValueField="lid" TabIndex="4" ToolTip="Select Type Of Payment.">
                                </asp:DropDownList>
                            </td>
                          
                        </tr>
                        <tr Id="trDutyAmount" runat="server" visible="false">
                            <td>Duty Amount</td>
                            <td>
                                <asp:TextBox ID="txtDutyAmount" runat="server" OnTextChanged="txtDutyAmount_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                <asp:CompareValidator ID="CompValDuty" runat="server" ControlToValidate="txtDutyAmount"
                                    Display="Dynamic" SetFocusOnError="true" Type="Double" Operator="DataTypeCheck"
                                    ErrorMessage="Invalid Amount" ValidationGroup="Required"></asp:CompareValidator>
                            </td>
                             <td>TR6 Challan No</td>
                            <td>
                                <asp:TextBox ID="txtChallanNo" runat="server"></asp:TextBox>

                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="Required"
                                Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtChallanNo"
                                Text="*" ErrorMessage="Please Enter TR6 Challan No." Visible="false"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                      
                        <tr id="trDutyInterest" runat="server" visible="false">
                           <td>Interest </td>
                            <td  colspan="3">
                                <table width="100%" style="border-collapse: collapse">
                                    <tr>                                         
                                        <td>
                                            <asp:RadioButtonList ID="rdlInterest" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                                                 OnSelectedIndexChanged="rdlInterest_OnSelectedIndexChanged" Width="150px">
                                                 <asp:ListItem Text="YES" Value="1"></asp:ListItem>
                                               <asp:ListItem Text="NO" Value="2"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>

                                        <td  id="tdlblInterestAmnt" runat="server" visible="false">
                                             <asp:Label ID="lblInterestAmnt" runat="server" Text="Interest Amount" ></asp:Label>

                                        </td>
                                        <td  id="tdtxtInterestAmnt" runat="server" visible="false">
                                            <asp:TextBox ID="txtInterestAmnt" runat="server" OnTextChanged="txtInterestAmnt_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                               <asp:CompareValidator ID="CVInterestAmount" runat="server" ControlToValidate="txtInterestAmnt"
                                                Display="Dynamic" SetFocusOnError="true" Type="Double" Operator="DataTypeCheck"
                                                ErrorMessage="Invalid Interest Amount" ValidationGroup="Required"></asp:CompareValidator>
                                        </td>
                                        <td width="40%"></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trDutyPenalty" runat="server" visible="false">
                            <td>Penalty </td>
                            <td colspan="3">
                                <table  width="100%" style="border-collapse: collapse">
                                    <tr>
                                        <td>
                                            <asp:RadioButtonList ID="rdlPenalty" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                                                 OnSelectedIndexChanged="rdlPenalty_OnSelectedIndexChanged" Width="150px">
                                               <asp:ListItem Text="YES" Value="1"></asp:ListItem>
                                               <asp:ListItem Text="NO" Value="2"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td id="tdlblPenaltyAmnt" runat="server" visible="false">
                                             <asp:Label ID="lblPenaltyAmnt" runat="server" Text="Penalty Amount" ></asp:Label>

                                        </td>
                                        <td colspan="3" id="tdtxtPenaltyAmnt" runat="server" visible="false">
                                            <asp:TextBox ID="txtPenaltyAmnt" runat="server" OnTextChanged="txtPenaltyAmnt_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:CompareValidator ID="CVPenaltyAmount" runat="server" ControlToValidate="txtPenaltyAmnt"
                                                Display="Dynamic" SetFocusOnError="true" Type="Double" Operator="DataTypeCheck"
                                                ErrorMessage="Invalid Penalty Amount" ValidationGroup="Required"></asp:CompareValidator>
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:FileUpload ID="fuPenaltyCopy" runat="server" />                                

                                        </td>
                                    </tr>
                                </table>
                            </td>
                           
                        </tr>
                       
                         <tr id="trACPRD" runat="server" visible="false">
                            <td>ACP / Non-ACP ?</td>
                            <td>
                                <asp:DropDownList ID="ddlACPNonACP" runat="server">
                                    <asp:ListItem Value="0" Text="- Select -" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="ACP"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Non ACP"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                           <td>RD / Duty / Penalty</td>
                            <td>
                                <asp:DropDownList ID="ddlType" runat="server">
                                    <asp:ListItem Value="0" Text="- Select -" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="RD"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Duty"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Penalty"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="trPenalApprMail" runat="server" visible="false">
                            <td>Penalty Approval Mail</td>
                            <td>
                                <asp:TextBox ID="txtPenaltyMail" runat="server"></asp:TextBox>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr id="trAmount" runat="server">
                            <td>Amount
                               <%-- <asp:RequiredFieldValidator ID="rfvAmount" runat="server" ValidationGroup="Required"
                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtAmount"
                                    Text="*" ErrorMessage="Please Enter Amount."> </asp:RequiredFieldValidator>--%>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAmount" runat="server" Width="160px" ToolTip="Enter Amount." TabIndex="6" OnTextChanged="txtAmount_OnTextChanged" AutoPostBack="true"></asp:TextBox>
                                <asp:CompareValidator ID="cvAmount" runat="server" ControlToValidate="txtAmount" 
                                    Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" ErrorMessage="Invalid Amount."
                                    Display="Dynamic" ValidationGroup="Required"></asp:CompareValidator>
                            </td>
                            <td></td>
                            <td></td>
                         
                        </tr>                      
                        <tr>
                             <td>Total Amount</td>
                            <td>
                                <asp:TextBox ID="txtTotalAmnt" runat="server" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>Paid To</td>
                            <td>
                                <asp:TextBox ID="txtPaidTo" runat="server" ToolTip="Enter Paid To." Width="290px" TabIndex="7"></asp:TextBox>

                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="Required"
                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtPaidTo"
                                    Text="*" ErrorMessage="Please Enter Paid To."> </asp:RequiredFieldValidator>
                            </td>
                            <td></td>
                            <td></td>
                           
                        </tr>                  
                       
                         <tr>
                            <td>Advance Received?
                                <asp:RequiredFieldValidator ID="rfvAdvanceAmt" runat="server" ValidationGroup="Required"
                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtAdvanceAmt"
                                    Text="*" ErrorMessage="Please Enter Advance Amount."> </asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvAdvanceAmt" runat="server" ControlToValidate="txtAdvanceAmt"
                                    Operator="DataTypeCheck" SetFocusOnError="true" Type="Double" ErrorMessage="Invalid Advance Amount."
                                    Display="Dynamic" ValidationGroup="Required"></asp:CompareValidator>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblAdvanceRecd" runat="server" ValidationGroup="vgAdvanceRecd" RepeatDirection="Horizontal"
                                    RepeatLayout="Table" AutoPostBack="true" OnSelectedIndexChanged="rblAdvanceRecd_OnSelectedIndexChanged" Width="200px">
                                    <asp:ListItem Selected="True" Value="0" Text="No"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                </asp:RadioButtonList>
                               
                            </td>
                            <td>
                                 <asp:TextBox ID="txtAdvanceAmt" runat="server" ToolTip="Enter Advance Amount." TabIndex="9" placeholder="Advance Amount"></asp:TextBox>
                            </td>
                            <td></td>
                        </tr>
                     
                        <tr>
                            <td>Remark
                                <asp:RequiredFieldValidator ID="rfvRemark" runat="server" ValidationGroup="Required"
                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtRemark"
                                    Text="*" ErrorMessage="Please Enter Remark."> </asp:RequiredFieldValidator>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtRemark" runat="server" Rows="3" TextMode="MultiLine" TabIndex="8"></asp:TextBox>
                            </td>
                        </tr>                       
                    </table>

                    <fieldset>
                        <legend>Add Documents</legend>

                        <div id="dvUploadNewFile2" runat="server" style="max-height: 200px; overflow: auto;">
                            <asp:FileUpload ID="fuDocument2" runat="server" />


                            <asp:Button ID="btnSaveDocument2" Text="Save Document" runat="server" OnClick="btnSaveDocument2_Click" />
                        </div>
                        <br />
                        <div>
                            <asp:Repeater ID="rptDocument2" runat="server" OnItemCommand="rptDocument2_ItemCommand">
                                <HeaderTemplate>
                                    <table class="table" border="0" cellpadding="0" cellspacing="0" style="width: 50%">
                                        <tr bgcolor="#FF781E">
                                            <th>Sl
                                            </th>
                                            <th>Document Name
                                            </th>
                                            <th>Action
                                            </th>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <%#Container.ItemIndex +1%>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lnkDownload" Text='<%#DataBinder.Eval(Container.DataItem,"DocumentName") %>'
                                                CommandArgument='<%# Eval("DocPath") %>' CausesValidation="false" runat="server"
                                                Width="200px" CommandName="DownloadFile"></asp:LinkButton>
                                            &nbsp;
                                            <asp:HiddenField ID="hdnDocLid" Value='<%#DataBinder.Eval(Container.DataItem,"PkId") %>'
                                                runat="server"></asp:HiddenField>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lnkDelete" CommandName="Delete" ToolTip="Delete" Width="39" CausesValidation="false"
                                                runat="server" Text="Delete" Font-Underline="true" OnClientClick="return confirm('Are you sure you want to remove this document?')"></asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </fieldset>
                </fieldset>
            </div>
            <div>
                <asp:SqlDataSource ID="DataSourceJobdetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetJobDetailForArchive" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceBranch" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="BS_GetBranchByUser" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:Parameter Name="UserId" DefaultValue="1" />
                        <%--<asp:SessionParameter Name="UserId" SessionField="UserId" />--%>
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceExpense" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetRequestTypeMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourcePaymentType" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_GetPaymentTypeMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </div>
        </ContentTemplate>
    <%--    <Triggers> 
            <asp:PostBackTrigger  ControlID="btnSubmit"/>
        </Triggers>--%>
    </asp:UpdatePanel>
    
</asp:Content>

