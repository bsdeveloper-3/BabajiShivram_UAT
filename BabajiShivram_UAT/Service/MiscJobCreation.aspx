<%@ Page Title="Job Creation" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="MiscJobCreation.aspx.cs" Inherits="Service_MiscJobCreation" Culture="en-GB" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <script type="text/javascript">

        function OnCustomerSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');

            $get('<%=hdnCustId.ClientID%>').value = results.ClientId;
        }
        $addHandler
            (
                $get('txtCustomerName'), 'keyup',

                function () {
                    $get('<%=txtCustomerName.ClientID%>').value = '0';
    }
);

    </script>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingchecklist"
            runat="server">
            <ProgressTemplate>
                <img alt="progress" src="./images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional"
        RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="Required" />
                <asp:ValidationSummary ID="ValidationSummary2" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="RequiredClaim" />
            </div>

            <fieldset>
                <legend>Add Job Detail</legend>

                <asp:Button ID="btnSave" runat="server" Text="Save" CausesValidation="true" ValidationGroup="Required"
                    TabIndex="6" OnClick="btnSave_Click" />
                <br />
                <br />
                <table border="0" cellpadding="0" cellspacing="0" width="80%" bgcolor="white">
                    <tr>
                        <td>
                            Job Number :
                        </td>
                        <td colspan="3">
                            <asp:Label ID="lblJobNo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Module
                        </td>
                        <td>
                            <asp:DropDownList ID="ddModule" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddModule_SelectedIndexChanged">
                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Marine" Value="30"></asp:ListItem>
                                <asp:ListItem Text="Warehouse" Value="40"></asp:ListItem>
                                <asp:ListItem Text="Essential Certificate" Value="35"></asp:ListItem>
                                <asp:ListItem Text="Equipment Hire" Value="45"></asp:ListItem>
                                <%--<asp:ListItem Text="Public Notice" Value="50"></asp:ListItem>--%>
                                <asp:ListItem Text="Project" Value="55"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>Branch
                            <asp:RequiredFieldValidator ID="rfvDDBranch1" runat="server" ControlToValidate="ddlBranch" InitialValue="0"
                                SetFocusOnError="true" Text="Required" ErrorMessage="Please Select Branch." Display="Dynamic"
                                ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBranch" runat="server" Width="250px" TabIndex="1" ToolTip="Branch" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>

                        </td>
                    </tr>
                    <tr>
                        <td>Trans Mode</td>
                        <td>
                            <asp:DropDownList ID="ddlTransMode" runat="server">
                                <asp:ListItem Text="Sea" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Air" Value="1"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>Type</td>
                        <td>
                            <asp:DropDownList ID="ddlType" runat="server">
                                <asp:ListItem Text="Import" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Export" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Customer
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtCustomerName"
                                SetFocusOnError="true" Text="Required" ErrorMessage="Please Enter Customer Name." Display="Dynamic"
                                ValidationGroup="Required"></asp:RequiredFieldValidator>
                            <asp:HiddenField ID="hdnCustId" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtCustomerName" placeholder="Customer Name" runat="server" MaxLength="300"
                                ToolTip="Customer Name" Width="240px" TabIndex="2" OnTextChanged="txtCustomerName_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <div id="divwidthCust1">
                            </div>
                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender10" runat="server" TargetControlID="txtCustomerName"
                                CompletionListElementID="divwidthCust1" ServicePath="~/WebService/CustomerAutoComplete.asmx"
                                ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthCust1"
                                ContextKey="4329" UseContextKey="True" OnClientItemSelected="OnCustomerSelected"
                                CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                CompletionListHighlightedItemCssClass="AutoExtenderHighlight" FirstRowSelected="true">
                            </cc1:AutoCompleteExtender>
                        </td>
                        <td>
                            Bill To GSTN/Consignee GSTN
                        </td>
                        <td>
                            <asp:DropDownList ID="ddConsigneeGSTN" runat="server"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Customer Division
                            <asp:RequiredFieldValidator ID="RFVCustDivis" ValidationGroup="Required" runat="server" Display="Dynamic"
                                Text="Required" ControlToValidate="ddDivision" InitialValue="0" ErrorMessage="Please Select Customer Division">
                            </asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddDivision" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddDivision_SelectedIndexChanged" Width="250" TabIndex="3">
                            </asp:DropDownList>
                            <asp:HiddenField ID="hdnDivisionId" runat="server" Value='<%#Eval("DivisionId") %>' />
                        </td>
                        <td>Customer Plant
                            <asp:RequiredFieldValidator ID="RFVPlant" ValidationGroup="Required" runat="server"
                                Display="Dynamic" ControlToValidate="ddPlant" InitialValue="0" ErrorMessage="Please Select Customer Plant"
                                Text="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddPlant" runat="server" Width="250" TabIndex="4"></asp:DropDownList>
                            <asp:HiddenField ID="hdnPlantId" runat="server" Value='<%#Eval("PlantId") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td>Job Description

                            <asp:RequiredFieldValidator ID="RFVDesc" runat="server" ControlToValidate="txtJobDescription"
                                SetFocusOnError="true" Text="Required" ErrorMessage="Please Enter Job Purpose" Display="Dynamic"
                                ValidationGroup="Required"></asp:RequiredFieldValidator>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtJobDescription" runat="server" MaxLength="200" TabIndex="5" ToolTip="Purpose"
                                TextMode="MultiLine" Rows="4" Width="400px"></asp:TextBox>
                        </td>
                    </tr>
                </table>

            </fieldset>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

