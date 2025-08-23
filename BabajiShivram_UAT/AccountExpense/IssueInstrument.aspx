<%@ Page Title="Issue Instrument" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="IssueInstrument.aspx.cs" Inherits="AccountExpense_IssueInstrument" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<asp:ScriptManager id="ScriptManager1" runat="server" scriptmode="Release"> </asp:ScriptManager>
    <script type="text/javascript">
        function OnJobSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnJobId').value = results.JobId;
        }
        function OnPayToSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('ctl00_ContentPlaceHolder1_hdnPayToCode').value = results.Par_Code;
        }
    </script>
    <asp:UpdatePanel ID="upFillDetails" runat="server">
        <ContentTemplate>
            <div>
                <div align="center">
                    <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                    <asp:ValidationSummary ID="vsFillDetails" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
                </div>
                <fieldset>
                    <legend>Issue Instrument</legend>
                    <div class="m clear">
                        <asp:Button ID="btnSubmit" Text="Save" runat="server" ValidationGroup="Required" OnClick="btnSubmit_OnClick" />
                        <asp:Button ID="btnCancel" Text="Cancel" CausesValidation="false" runat="server" OnClick="btnCancel_OnClick" />
                        <asp:HiddenField ID="hdnJobId" runat="server" Value="0" />
                        <asp:HiddenField ID="hdnCustomerId" runat="server" Value="0" />
                        <asp:HiddenField ID="hdnPayToCode" runat="server" Value="0" />
                    </div>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td width="20%">Job Number
                                <asp:RequiredFieldValidator ID="rfvJobNo" runat="server" ValidationGroup="Required"
                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtJobNumber"
                                    Text="*" ErrorMessage="Please Select Job Number."></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtJobNumber" Width="160px" runat="server" ToolTip="Enter Job Number."
                                    CssClass="SearchTextbox" placeholder="Search" AutoPostBack="true" OnTextChanged="txtJobNumber_TextChanged"></asp:TextBox>
                                <div id="divwidthCust_Loc" runat="server">
                                </div>
                                <cc1:autocompleteextender id="JobDetailExtender" runat="server" TargetControlID="txtJobNumber"
                                    completionlistelementid="divwidthCust_Loc" ServicePath="~/WebService/JobNumberAutoComplete.asmx"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthCust_Loc"
                                    ContextKey="4567" usecontextkey="True" OnClientItemSelected="OnJobSelected"
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:autocompleteextender>
                            </td>
                            <td>
                                Customer
                            </td>
                            <td>
                                <asp:Label ID="lblCustomer" runat="server" Width="290px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Pay to</td>
                            <td>
                                <asp:TextBox ID="txtPayTo" runat="server" MaxLength="100" CssClass="SearchTextbox" placeholder="Search" ></asp:TextBox>
                                <div id="divPayTo" runat="server">
                                </div>
                                <cc1:autocompleteextender id="PatToExtender" runat="server" targetcontrolid="txtPayTo"
                                    completionlistelementid="divPayTo" servicepath="~/WebService/FAPayToAutoComplete.asmx"
                                    servicemethod="GetCompletionList" minimumprefixlength="2" behaviorid="divPayTo"
                                    contextkey="3211" usecontextkey="True" onclientitemselected="OnPayToSelected"
                                    completionlistcssclass="AutoExtender" completionlistitemcssclass="AutoExtenderList"
                                    completionlisthighlighteditemcssclass="AutoExtenderHighlight">
                                </cc1:autocompleteextender>
                            </td>
                            <td>Bank Name</td>
                            <td>
                                <asp:TextBox ID="txtBankName" runat="server" maxLEngth="100"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Cheque No</td>
                            <td>
                                <asp:TextBox ID="txtChequeNo" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                Cheque Date
                                <cc1:calendarextender id="calChequeDate" runat="server" FirstDayOfWeek="Sunday" PopupButtonID="imgChequeDate"
                                    Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtChequeDate">
                                </cc1:calendarextender>
                            </td>
                            <td>
                                <asp:TextBox ID="txtChequeDate" runat="server" Width="125px" placeholder="dd/mm/yyyy" ToolTip="Enter Cheque Date."></asp:TextBox>
                                <asp:Image ID="imgChequeDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Remark
                                <asp:RequiredFieldValidator ID="rfvRemark" runat="server" ValidationGroup="Required"
                                    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtRemark"
                                    Text="*" ErrorMessage="Please Enter Remark."> </asp:RequiredFieldValidator>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtRemark" runat="server" Rows="2" TextMode="MultiLine" MaxLength="200"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <div class="clear"></div>
                <fieldset><legend>History</legend>
                <div class="clear"></div>
                <asp:GridView ID="gvDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                    AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataSourceID="ChequeSqlDataSource" 
                    AllowPaging="True" AllowSorting="True" PageSize="40" PagerSettings-Position="TopAndBottom"
                    Width="100%">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:BoundField DataField="JobRefNo" HeaderText="Job No" />
                        <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                        <asp:BoundField DataField="PayTo" HeaderText="PayTo" SortExpression="PayTo" />
                        <asp:BoundField DataField="ChequeNo" HeaderText="Cheque No" SortExpression="ChequeNo" />
                        <asp:BoundField DataField="ChequeDate" HeaderText="Cheque Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ChequeDate" />
                        <asp:BoundField DataField="BankName" HeaderText="Bank Name" SortExpression="BankName" />
                        <asp:BoundField DataField="ChequeIssuedBy" HeaderText="Issued By" SortExpression="IssuedBy" />
                        <asp:BoundField DataField="ChequeIssueDate" HeaderText="Issue Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="IssueDate" />
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
                </fieldset>
                <div>
                <asp:SqlDataSource ID="ChequeSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AC_FAGetInstrumentHistory" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:ControlParameter ControlID="txtJobNumber" Name="JobRefNo" PropertyName="Text" />
                        <asp:ControlParameter ControlID="txtChequeNo" Name="ChequeNo" PropertyName="Text" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
            </div>
        </ContentTemplate>
        <%--<Triggers> 
            <asp:PostBackTrigger  ControlID="btnSubmit"/>
        </Triggers>--%>
    </asp:UpdatePanel>
</asp:Content>