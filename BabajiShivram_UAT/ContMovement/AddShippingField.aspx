<%@ Page Title="Add Shipping Fields" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AddShippingField.aspx.cs"
    Inherits="ContMovement_AddShippingField" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release">
    </asp:ScriptManager>

    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlAddShippingField" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../Images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="upnlAddShippingField" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <fieldset class="fieldset-AutoWidth">
                <legend>Add Letter Field</legend>

                <div align="center">
                    <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                    <asp:ValidationSummary ID="vsLetterFields" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="vgRequired" CssClass="errorMsg" />
                </div>
                <div class="clear">
                    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" TabIndex="4" ValidationGroup="vgRequired" />
                    <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" TabIndex="5" />
                </div>
                <table class="table" border="0" style="width: 100%">
                    <tr>
                        <td>Shipping Line
                            <asp:RequiredFieldValidator ID="rfvShippingLine" runat="server" ControlToValidate="ddlShippingMS" InitialValue="0" SetFocusOnError="true"
                                Display="Dynamic" ErrorMessage="Please Select Shipping Line." Text="*" ForeColor="Red" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlShippingMS" runat="server" AppendDataBoundItems="true" DataSourceID="DataSourceShippingMaster"
                                DataTextField="CompName" DataValueField="lid" TabIndex="1" Width="360px" AutoPostBack="true" OnSelectedIndexChanged="ddlShippingMS_SelectedIndexChanged">
                                <asp:ListItem Selected="True" Value="0" Text="-Select-"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Add Field for Letter                     
                            <asp:RequiredFieldValidator ID="rfvLetters" runat="server" ControlToValidate="ddlLetters" InitialValue="0" SetFocusOnError="true"
                                Display="Dynamic" ErrorMessage="Please Add Field for Letter." Text="*" ForeColor="Red" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlLetters" runat="server" AppendDataBoundItems="true" DataSourceID="DataSourceShippingLetters"
                                DataTextField="LetterName" DataValueField="lid" TabIndex="1" Width="360px">
                                <asp:ListItem Selected="True" Value="0" Text="-Select-"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Enter Field Name
                            <asp:RequiredFieldValidator ID="rfvFieldName" runat="server" ControlToValidate="ddlLetters" InitialValue="0" SetFocusOnError="true"
                                Display="Dynamic" ErrorMessage="Please Enter Field Name." Text="*" ForeColor="Red" ValidationGroup="vgRequired"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFieldName" runat="server" TabIndex="2" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Is Table Or Not?</td>
                        <td>
                            <asp:RadioButtonList ID="rblFieldType" runat="server" ValidationGroup="vgFieldType" RepeatDirection="Horizontal" RepeatLayout="Flow" CellSpacing="5">
                                <asp:ListItem Value="0" Text="No" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
                <div>
                    <asp:SqlDataSource ID="DataSourceShippingLetters" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="CM_GetShippingLettersById" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddlShippingMS" Name="ShippingId" PropertyName="SelectedValue" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="DataSourceShippingMaster" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="GetShippingMS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="DataSourceBSFieldMaster" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="CM_GetFieldMaster" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

