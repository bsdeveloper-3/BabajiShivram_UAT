<%@ Page Title="HO Service Request" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="ServiceRequest.aspx.cs" Inherits="Service_ServiceRequest" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AjaxToolkit:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        
        <div align="center">
            <asp:Label ID="lblError" runat="server"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" />
        </div>
        <div class="clear"></div>
        <fieldset>
        <legend>Employee Issue Detail</legend>
        <div class="m clear">
            <asp:Button ID="btnSubmit" CssClass="btn" Text="Save" runat="server" 
                OnClick="btnSubmit_Click" ValidationGroup="Required" TabIndex="7"/>
            <asp:Button ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click" runat="server"
                 CausesValidation="false" TabIndex="8"/>
        </div>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
            <tr>
                <td>
                    Branch Name
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtBranchName"
                        Text="Required" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Please Enter Branch Name"
                        InitialValue="" ValidationGroup="Required"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtBranchName" runat="server" MaxLength="100"></asp:TextBox>
                </td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td>
                    Reuqest By Emp Name
                    <asp:RequiredFieldValidator ID="RFVEmpName" runat="server" ControlToValidate="txtEmpName"
                        Text="Required" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Please Enter Emp Name"
                        InitialValue="" ValidationGroup="Required"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtEmpName" runat="server" MaxLength="100"></asp:TextBox>
                </td>
                <td>
                    Issue HO Department
                    <asp:RequiredFieldValidator ID="RFVDept" runat="server" ControlToValidate="ddDepartment"
                        Text="Required" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Please Select Department Name"
                        InitialValue="0" ValidationGroup="Required"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:DropDownList ID="ddDepartment" runat="server">
                        <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Accounts" Value="11"></asp:ListItem>
                        <asp:ListItem Text="Billing" Value="6"></asp:ListItem>
                        <asp:ListItem Text="HR" Value="12"></asp:ListItem>
                        <asp:ListItem Text="IT" Value="13"></asp:ListItem>
                        <asp:ListItem Text="Admin" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Issue Detail
                    <asp:RequiredFieldValidator ID="RFVRemark" runat="server" ControlToValidate="txtRemarks"
                        Text="Required" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Please Enter Issue Detail"
                        InitialValue="" ValidationGroup="Required"></asp:RequiredFieldValidator>
                </td>
                <td colspan="3">
                   <asp:TextBox ID="txtRemarks" runat="server" MaxLength="800" TextMode="MultiLine"></asp:TextBox> 
                </td>
            </tr>
        </table>
        </fieldset>
        <div>
        <fieldset>
        <legend runat="server" id="legendLog">Issue Log</legend>
        
            <asp:GridView ID="gvBranchIssueHistory" runat="server" AutoGenerateColumns="False" CssClass="table" Width="99%" 
                PagerStyle-CssClass="pgr" CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40"
                DataKeyNames="lid" DataSourceID="DataSourceIssueHistory">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Emp Name" DataField="EmpName" />
                    <asp:BoundField HeaderText="Dept" DataField="DeptName" />
                    <asp:BoundField HeaderText="Issue Date" DataField="RequestDate" DataFormatString="{0:dd/MM/yyyy}"/>
                    <asp:BoundField HeaderText="Resolved Date" DataField="ResolvedDate" DataFormatString="{0:dd/MM/yyyy}"/>
                    <asp:BoundField HeaderText="Resolved By" DataField="ResolvedByName" />
                    <asp:BoundField HeaderText="Status" DataField="StatusName" />
                </Columns>
            </asp:GridView>
        </fieldset>
        </div>
        <div>
            <asp:SqlDataSource ID="DataSourceIssueHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="SR_GetDeptIssue" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter Name="UserId" SessionField="UserId" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
        
    </ContentTemplate>
    </asp:UpdatePanel>
    </div>
</asp:Content>
