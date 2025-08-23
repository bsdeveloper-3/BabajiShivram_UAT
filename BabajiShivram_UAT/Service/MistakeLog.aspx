<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MistakeLog.aspx.cs" Inherits="MistakeLog" 
MasterPageFile="~/MasterPage.master" Title="Mistake Log" Culture="en-GB" %>
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
        <AjaxToolkit:CalendarExtender ID="CalMistDate" runat="server" Enabled="True"
            EnableViewState="False" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupButtonID="txtMistakeDate"
            PopupPosition="BottomRight" TargetControlID="txtMistakeDate">
        </AjaxToolkit:CalendarExtender>
        <div align="center">
            <asp:Label ID="lblError" runat="server"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
        ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" />
        </div>
        <div class="clear"></div>
        <fieldset>
        <legend>Mistake Detail</legend>
        <div class="m clear">
            <asp:Button ID="btnSubmit" CssClass="btn" Text="Save" runat="server" 
                OnClick="btnSubmit_Click" ValidationGroup="Required" TabIndex="7"/>
            <asp:Button ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click" runat="server"
                 CausesValidation="false" TabIndex="8"/>
        </div>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
            <tr>
                <td>
                    Mistake By
                    <asp:RequiredFieldValidator ID="RFVMistakeBy" runat="server" ControlToValidate="ddMistakeUser"
                        Text="Required" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Please Select Mistake By"
                        InitialValue="0" ValidationGroup="Required"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:DropDownList ID="ddMistakeUser" runat="server" AutoPostBack="true" 
                        OnSelectedIndexChanged="ddMistakeUser_SelectedIndexChanged"></asp:DropDownList>
                </td>
                <td>
                    Mistake Date
                    <asp:RequiredFieldValidator ID="RFVMistDate" runat="server" ControlToValidate="txtMistakeDate"
                        Text="Required" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Please Enter Mistake Date"
                        InitialValue="" ValidationGroup="Required"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtMistakeDate" Width="100px" runat="server" placeholder="DD/MM/YYYY" />
                    <asp:CompareValidator ID="ComValMistDate" runat="server" ControlToValidate="txtMistakeDate" Display="Dynamic" ValidationGroup="Required"
                        SetFocusOnError="true" Text="Invalid Date" ErrorMessage="Invalid Date." Type="Date" CultureInvariantValues="false" Operator="DataTypeCheck"></asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td>
                    Customer Name
                </td>
                <td>
                    <asp:TextBox ID="txtCustomer" runat="server" MaxLength="100"></asp:TextBox>
                </td>
                <td>
                    Amount
                </td>
                <td>
                    <asp:TextBox ID="txtMistakeAmount" runat="server" MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator ID="CompValAmount" runat="server" ControlToValidate="txtMistakeAmount" Display="Dynamic" SetFocusOnError="true"
                      Type="Integer" Operator="DataTypeCheck" ErrorMessage="Invalid Amount." ValidationGroup="Required"></asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td>
                    Status
                </td>
                <td>
                    <asp:DropDownList ID="ddStatus" runat="server">
                        <asp:ListItem Text="Unresolved" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Resolved" Value="2"></asp:ListItem>
                        <asp:ListItem Text="N.A." Value="3"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    Remarks
                    <asp:RequiredFieldValidator ID="RFVRemark" runat="server" ControlToValidate="txtRemarks"
                        Text="Required" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Please Enter Remark"
                        InitialValue="" ValidationGroup="Required"></asp:RequiredFieldValidator>
                </td>
                <td>
                   <asp:TextBox ID="txtRemarks" runat="server" MaxLength="800" TextMode="MultiLine"></asp:TextBox> 
                </td>
            </tr>
            
        </table>
        </fieldset>
        <div>
        <fieldset>
        <legend runat="server" id="legendLog">User Mistake Log</legend>
        
            <asp:GridView ID="gvUserMistkeHistory" runat="server" AutoGenerateColumns="False"
                CssClass="table" Width="99%" PagerStyle-CssClass="pgr"
                DataKeyNames="lId" DataSourceID="DataSourceUserHistory" CellPadding="4"
                AllowPaging="True" AllowSorting="True" PageSize="40">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Mistake By" DataField="MistakeByName" />
                    <asp:BoundField HeaderText="Mistake Date" DataField="MistakeDate" DataFormatString="{0:dd/MM/yyyy}"/>
                    <asp:BoundField HeaderText="Amount" DataField="Amount" />
                    <asp:BoundField HeaderText="Customer" DataField="CustomerName" />
                    <asp:BoundField HeaderText="Status" DataField="StatusName" />
                    <asp:BoundField HeaderText="Remarks" DataField="MistakeRemarks" />
                    <asp:BoundField HeaderText="Logged By" DataField="LoggedByName" />
                </Columns>
            </asp:GridView>
        </fieldset>
        </div>
        <div>
            <asp:SqlDataSource ID="DataSourceUserHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="GetMistakeByUser" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:ControlParameter ControlID="ddMistakeUser" Name="UserId" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
        
    </ContentTemplate>
    </asp:UpdatePanel>
    </div>
</asp:Content>