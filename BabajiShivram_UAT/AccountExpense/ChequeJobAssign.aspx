<%@ Page Title="Issue Cheque" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ChequeJobAssign.aspx.cs" 
    Inherits="AccountExpense_ChequeJobAssign" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<asp:ScriptManager id="ScriptManager1" runat="server" scriptmode="Release"> </asp:ScriptManager>    
    <asp:UpdatePanel ID="upFillDetails" runat="server">
        <ContentTemplate>
            <div>
                <div align="center">
                    <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                    <asp:ValidationSummary ID="vsFillDetails" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
                </div>
                <fieldset>
                    <legend>Job No/Cheque No. Issue</legend>
                    Cheque Date 
                    <cc1:CalendarExtender ID="CalExtPayDueDate" runat="server" FirstDayOfWeek="Sunday" PopupButtonID="imgPaymentDueDate"
                        Format="dd/MM/yyyy" PopupPosition="BottomRight" TargetControlID="txtPaymentDueDate">
                    </cc1:CalendarExtender>
                    <cc1:MaskedEditExtender ID="MskEdtPayDueDate" TargetControlID="txtPaymentDueDate" Mask="99/99/9999" MessageValidatorTip="true"
                        MaskType="Date" AutoComplete="false" runat="server">
                    </cc1:MaskedEditExtender>
                    <cc1:MaskedEditValidator ID="MskEdtValChequeDate" ControlExtender="MskEdtPayDueDate" ControlToValidate="txtPaymentDueDate" IsValidEmpty="false"
                        InvalidValueMessage="Cheque Date is invalid" InvalidValueBlurredMessage="Invalid Cheque Date" SetFocusOnError="true"
                        MinimumValueMessage="Invalid Cheque Date" MaximumValueMessage="Invalid  Date" ValidationGroup="Required"
                        MaximumValue='<%#DateTime.Now.ToString("dd/MM/yyyy") %>' MinimumValue='01/01/2023'
                        EmptyValueBlurredText="Required" EmptyValueMessage="Invalid Cheque Date" runat="Server"></cc1:MaskedEditValidator>

                    <asp:TextBox ID="txtPaymentDueDate" runat="server" Width="100px" placeholder="dd/mm/yyyy"></asp:TextBox>
                    <asp:Image ID="imgPaymentDueDate" ImageAlign="Top" ImageUrl="../Images/btn_calendar.gif" runat="server" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnSave" runat="server" Text="Issue Cheque" OnClick="btnSave_Click" />
                    <br />  
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr style="background-color:yellowgreen;color:white;">
                            <td>Sl.</td>
                            <td>Job No</td>
                            <td>Cheque No</td>
                            <td>Consignee</td>
                            <td>CFS</td>
                        </tr>
                        <tr>
                            <td>1.</td>
                            <td>
                                <asp:TextBox ID="txtJobNumber1" Width="160px" runat="server" ToolTip="Enter Job Number."
                                    CssClass="SearchTextbox" placeholder="Search Job" AutoPostBack="true" OnTextChanged="txtJobNumber1_TextChanged"></asp:TextBox>
                                <div id="divJobNo_Loc1" runat="server">
                                </div>
                                <cc1:AutoCompleteExtender ID="JobDetailExtender1" runat="server" TargetControlID="txtJobNumber1"
                                    CompletionListElementID="divJobNo_Loc1" ServicePath="~/WebService/JobNumberAutoComplete.asmx"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="5" BehaviorID="divJobNo_Loc1"
                                    ContextKey="1071" UseContextKey="True" 
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:TextBox ID="txtChequeNo1" Width="160px" runat="server" ToolTip="Enter Cheque Number."
                                    CssClass="SearchTextbox" placeholder="Search"></asp:TextBox>
                                <div id="divCheque1" runat="server">
                                </div>
                                <cc1:AutoCompleteExtender ID="ChequeExtender1" runat="server" TargetControlID="txtChequeNo1"
                                    CompletionListElementID="divCheque1" ServicePath="~/WebService/AccountChequeNoAutoComplete.asmx"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="3" BehaviorID="divCheque1"
                                    ContextKey="10051" UseContextKey="True" 
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblConsignee1" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCFS1" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>2.</td>
                            <td>
                                <asp:TextBox ID="txtJobNumber2" Width="160px" runat="server" ToolTip="Enter Job Number."
                                    CssClass="SearchTextbox" placeholder="Search Job" AutoPostBack="true" OnTextChanged="txtJobNumber2_TextChanged"></asp:TextBox>
                                <div id="divJobNo_Loc2" runat="server">
                                </div>
                                <cc1:AutoCompleteExtender ID="JobDetailExtender2" runat="server" TargetControlID="txtJobNumber2"
                                    CompletionListElementID="divJobNo_Loc2" ServicePath="~/WebService/JobNumberAutoComplete.asmx"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="5" BehaviorID="divJobNo_Loc2"
                                    ContextKey="1072" UseContextKey="True" 
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:TextBox ID="txtChequeNo2" Width="160px" runat="server" ToolTip="Enter Cheque Number."
                                    CssClass="SearchTextbox" placeholder="Search"></asp:TextBox>
                                <div id="divCheque2" runat="server">
                                </div>
                                <cc1:AutoCompleteExtender ID="ChequeExtender2" runat="server" TargetControlID="txtChequeNo2"
                                    CompletionListElementID="divCheque2" ServicePath="~/WebService/AccountChequeNoAutoComplete.asmx"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="3" BehaviorID="divCheque2"
                                    ContextKey="10052" UseContextKey="True" 
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblConsignee2" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCFS2" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>3.</td>
                            <td>
                                <asp:TextBox ID="txtJobNumber3" Width="160px" runat="server" ToolTip="Enter Job Number."
                                    CssClass="SearchTextbox" placeholder="Search Job" AutoPostBack="true" OnTextChanged="txtJobNumber3_TextChanged" ></asp:TextBox>
                                <div id="divJobNo_Loc3" runat="server">
                                </div>
                                <cc1:AutoCompleteExtender ID="JobDetailExtender3" runat="server" TargetControlID="txtJobNumber3"
                                    CompletionListElementID="divJobNo_Loc3" ServicePath="~/WebService/JobNumberAutoComplete.asmx"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="5" BehaviorID="divJobNo_Loc3"
                                    ContextKey="1073" UseContextKey="True" 
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:TextBox ID="txtChequeNo3" Width="160px" runat="server" ToolTip="Enter Cheque Number."
                                    CssClass="SearchTextbox" placeholder="Search"></asp:TextBox>
                                <div id="divCheque3" runat="server">
                                </div>
                                <cc1:AutoCompleteExtender ID="ChequeExtender3" runat="server" TargetControlID="txtChequeNo3"
                                    CompletionListElementID="divCheque3" ServicePath="~/WebService/AccountChequeNoAutoComplete.asmx"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="3" BehaviorID="divCheque3"
                                    ContextKey="10053" UseContextKey="True" 
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblConsignee3" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCFS3" runat="server"></asp:Label>
                            </td>
                        </tr>                        
                        <tr>
                            <td>4.</td>
                            <td>
                                <asp:TextBox ID="txtJobNumber4" Width="160px" runat="server" ToolTip="Enter Job Number."
                                    CssClass="SearchTextbox" placeholder="Search Job" AutoPostBack="true" OnTextChanged="txtJobNumber4_TextChanged"></asp:TextBox>
                                <div id="divJobNo_Loc4" runat="server">
                                </div>
                                <cc1:AutoCompleteExtender ID="JobDetailExtender4" runat="server" TargetControlID="txtJobNumber4"
                                    CompletionListElementID="divJobNo_Loc4" ServicePath="~/WebService/JobNumberAutoComplete.asmx"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="5" BehaviorID="divJobNo_Loc4"
                                    ContextKey="1054" UseContextKey="True" 
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:TextBox ID="txtChequeNo4" Width="160px" runat="server" ToolTip="Enter Cheque Number."
                                    CssClass="SearchTextbox" placeholder="Search"></asp:TextBox>
                                <div id="divCheque4" runat="server">
                                </div>
                                <cc1:AutoCompleteExtender ID="ChequeExtender4" runat="server" TargetControlID="txtChequeNo4"
                                    CompletionListElementID="divCheque4" ServicePath="~/WebService/AccountChequeNoAutoComplete.asmx"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="3" BehaviorID="divCheque4"
                                    ContextKey="10074" UseContextKey="True" 
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblConsignee4" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCFS4" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>5.</td>
                            <td>
                                <asp:TextBox ID="txtJobNumber5" Width="160px" runat="server" ToolTip="Enter Job Number."
                                    CssClass="SearchTextbox" placeholder="Search Job" AutoPostBack="true" OnTextChanged="txtJobNumber5_TextChanged"></asp:TextBox>
                                <div id="divJobNo_Loc5" runat="server">
                                </div>
                                <cc1:AutoCompleteExtender ID="JobDetailExtender5" runat="server" TargetControlID="txtJobNumber5"
                                    CompletionListElementID="divJobNo_Loc5" ServicePath="~/WebService/JobNumberAutoComplete.asmx"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="5" BehaviorID="divJobNo_Loc5"
                                    ContextKey="1055" UseContextKey="True" 
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:TextBox ID="txtChequeNo5" Width="160px" runat="server" ToolTip="Enter Cheque Number."
                                    CssClass="SearchTextbox" placeholder="Search"></asp:TextBox>
                                <div id="divCheque5" runat="server">
                                </div>
                                <cc1:AutoCompleteExtender ID="ChequeExtender5" runat="server" TargetControlID="txtChequeNo5"
                                    CompletionListElementID="divCheque5" ServicePath="~/WebService/AccountChequeNoAutoComplete.asmx"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="3" BehaviorID="divCheque5"
                                    ContextKey="10075" UseContextKey="True" 
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblConsignee5" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCFS5" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>6.</td>
                            <td>
                                <asp:TextBox ID="txtJobNumber6" Width="160px" runat="server" ToolTip="Enter Job Number."
                                    CssClass="SearchTextbox" placeholder="Search Job" AutoPostBack="true" OnTextChanged="txtJobNumber6_TextChanged"></asp:TextBox>
                                <div id="divJobNo_Loc6" runat="server">
                                </div>
                                <cc1:AutoCompleteExtender ID="JobDetailExtender6" runat="server" TargetControlID="txtJobNumber6"
                                    CompletionListElementID="divJobNo_Loc6" ServicePath="~/WebService/JobNumberAutoComplete.asmx"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="5" BehaviorID="divJobNo_Loc6"
                                    ContextKey="1056" UseContextKey="True" 
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:TextBox ID="txtChequeNo6" Width="160px" runat="server" ToolTip="Enter Cheque Number."
                                    CssClass="SearchTextbox" placeholder="Search"></asp:TextBox>
                                <div id="divCheque6" runat="server">
                                </div>
                                <cc1:AutoCompleteExtender ID="ChequeExtender6" runat="server" TargetControlID="txtChequeNo6"
                                    CompletionListElementID="divCheque6" ServicePath="~/WebService/AccountChequeNoAutoComplete.asmx"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="3" BehaviorID="divCheque6"
                                    ContextKey="10076" UseContextKey="True" 
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblConsignee6" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCFS6" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>7.</td>
                            <td>
                                <asp:TextBox ID="txtJobNumber7" Width="160px" runat="server" ToolTip="Enter Job Number."
                                    CssClass="SearchTextbox" placeholder="Search Job" AutoPostBack="true" OnTextChanged="txtJobNumber7_TextChanged"></asp:TextBox>
                                <div id="divJobNo_Loc7" runat="server">
                                </div>
                                <cc1:AutoCompleteExtender ID="JobDetailExtender7" runat="server" TargetControlID="txtJobNumber7"
                                    CompletionListElementID="divJobNo_Loc7" ServicePath="~/WebService/JobNumberAutoComplete.asmx"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="5" BehaviorID="divJobNo_Loc7"
                                    ContextKey="1057" UseContextKey="True" 
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:TextBox ID="txtChequeNo7" Width="160px" runat="server" ToolTip="Enter Cheque Number."
                                    CssClass="SearchTextbox" placeholder="Search"></asp:TextBox>
                                <div id="divCheque7" runat="server">
                                </div>
                                <cc1:AutoCompleteExtender ID="ChequeExtender7" runat="server" TargetControlID="txtChequeNo7"
                                    CompletionListElementID="divCheque7" ServicePath="~/WebService/AccountChequeNoAutoComplete.asmx"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="3" BehaviorID="divCheque7"
                                    ContextKey="10077" UseContextKey="True" 
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblConsignee7" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCFS7" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>8.</td>
                            <td>
                                <asp:TextBox ID="txtJobNumber8" Width="160px" runat="server" ToolTip="Enter Job Number."
                                    CssClass="SearchTextbox" placeholder="Search Job" AutoPostBack="true" OnTextChanged="txtJobNumber8_TextChanged"></asp:TextBox>
                                <div id="divJobNo_Loc8" runat="server">
                                </div>
                                <cc1:AutoCompleteExtender ID="JobDetailExtender8" runat="server" TargetControlID="txtJobNumber8"
                                    CompletionListElementID="divJobNo_Loc8" ServicePath="~/WebService/JobNumberAutoComplete.asmx"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="5" BehaviorID="divJobNo_Loc8"
                                    ContextKey="1058" UseContextKey="True" 
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:TextBox ID="txtChequeNo8" Width="160px" runat="server" ToolTip="Enter Cheque Number."
                                    CssClass="SearchTextbox" placeholder="Search"></asp:TextBox>
                                <div id="divCheque8" runat="server">
                                </div>
                                <cc1:AutoCompleteExtender ID="ChequeExtender8" runat="server" TargetControlID="txtChequeNo8"
                                    CompletionListElementID="divCheque8" ServicePath="~/WebService/AccountChequeNoAutoComplete.asmx"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="3" BehaviorID="divCheque8"
                                    ContextKey="10058" UseContextKey="True" 
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblConsignee8" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCFS8" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>9.</td>
                            <td>
                                <asp:TextBox ID="txtJobNumber9" Width="160px" runat="server" ToolTip="Enter Job Number."
                                    CssClass="SearchTextbox" placeholder="Search Job" AutoPostBack="true" OnTextChanged="txtJobNumber9_TextChanged"></asp:TextBox>
                                <div id="divJobNo_Loc9" runat="server">
                                </div>
                                <cc1:AutoCompleteExtender ID="JobDetailExtender9" runat="server" TargetControlID="txtJobNumber9"
                                    CompletionListElementID="divJobNo_Loc9" ServicePath="~/WebService/JobNumberAutoComplete.asmx"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="5" BehaviorID="divJobNo_Loc9"
                                    ContextKey="1059" UseContextKey="True" 
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:TextBox ID="txtChequeNo9" Width="160px" runat="server" ToolTip="Enter Cheque Number."
                                    CssClass="SearchTextbox" placeholder="Search"></asp:TextBox>
                                <div id="divCheque9" runat="server">
                                </div>
                                <cc1:AutoCompleteExtender ID="ChequeExtender9" runat="server" TargetControlID="txtChequeNo9"
                                    CompletionListElementID="divCheque9" ServicePath="~/WebService/AccountChequeNoAutoComplete.asmx"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="3" BehaviorID="divCheque9"
                                    ContextKey="10079" UseContextKey="True" 
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblConsignee9" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCFS9" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>10.</td>
                            <td>
                                <asp:TextBox ID="txtJobNumber10" Width="160px" runat="server" ToolTip="Enter Job Number."
                                    CssClass="SearchTextbox" placeholder="Search Job" AutoPostBack="true" OnTextChanged="txtJobNumber10_TextChanged"></asp:TextBox>
                                <div id="divJobNo_Loc10" runat="server">
                                </div>
                                <cc1:AutoCompleteExtender ID="JobDetailExtender10" runat="server" TargetControlID="txtJobNumber10"
                                    CompletionListElementID="divJobNo_Loc10" ServicePath="~/WebService/JobNumberAutoComplete.asmx"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="3" BehaviorID="divJobNo_Loc10"
                                    ContextKey="1060" UseContextKey="True" 
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:TextBox ID="txtChequeNo10" Width="160px" runat="server" ToolTip="Enter Cheque Number."
                                    CssClass="SearchTextbox" placeholder="Search"></asp:TextBox>
                                <div id="divCheque10" runat="server">
                                </div>
                                <cc1:AutoCompleteExtender ID="ChequeExtender10" runat="server" TargetControlID="txtChequeNo10"
                                    CompletionListElementID="divCheque10" ServicePath="~/WebService/AccountChequeNoAutoComplete.asmx"
                                    ServiceMethod="GetCompletionList" MinimumPrefixLength="3" BehaviorID="divCheque10"
                                    ContextKey="10080" UseContextKey="True" 
                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblConsignee10" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCFS10" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                                                          
                </fieldset>
                
                
                <div>
            </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

