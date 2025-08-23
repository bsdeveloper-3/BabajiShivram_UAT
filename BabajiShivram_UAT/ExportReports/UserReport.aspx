<%@ Page Title="User Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UserReport.aspx.cs"
    Inherits="ExportReports_UserReport" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="updAdhocReport" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="updAdhocReport" runat="server">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:ValidationSummary ID="Valsummary" runat="server" ShowMessageBox="true" ShowSummary="false"
                    ValidationGroup="Required" />
            </div>
            <div class="clear">
            </div>
                <fieldset>
                <legend>Report Type</legend>
                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                    <tr>
                        <td>
                            Report Name &nbsp;
                            <asp:RequiredFieldValidator ID="rfvReportName" runat="server" ControlToValidate="txtReportName"
                                ErrorMessage="Please Enter Report Name" Text="*" InitialValue="" ValidationGroup="Required"
                                Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        
                            <asp:TextBox ID="txtReportName" runat="server" MaxLength="100" TabIndex="1"></asp:TextBox>
                        </td>
                        <td>
                            Report Type &nbsp;
                            <asp:RequiredFieldValidator ID="RFVReportType" runat="server" ControlToValidate="ddReportType"
                                ErrorMessage="Please Select Report Type" Text="*" InitialValue="0" ValidationGroup="Required"
                                Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            <asp:DropDownList ID="ddReportType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddReportType_SelectedIndexChanged"
                                Width="120px" TabIndex="2">
                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Babaji" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Customer" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                            
                        </td>
                        <td>
                            Customer &nbsp;
                            <asp:RequiredFieldValidator ID="rfvCustomer" runat="server" ValidationGroup="Required" Enabled="false" 
                                ControlToValidate="ddCustomer" Text="*" InitialValue="0" ErrorMessage="Please Select Customer Name"
                                SetFocusOnError="true" Display="Dynamic" ></asp:RequiredFieldValidator>
                            <asp:DropDownList ID="ddCustomer" runat="server" TabIndex="3" AutoPostBack="true" OnSelectedIndexChanged="ddCustomer_SelectedIndexChanged">
                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                            
                        </td>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="Save Report" OnClick="btnSave_Click"
                                ValidationGroup="Required" TabIndex="4"/>
                            <asp:Button ID="btnUpdate" runat="server" Text="Update Report" OnClick="btnUpdate_Click"
                                Visible="false" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" Visible="false" OnClick="btnCancel_Click" />
                        </td>
                    </tr>
                </table>
                </fieldset>
                <div class="clear">
                </div>
                <fieldset>
                <legend>Report Field</legend>
                <table class="table" id="tblAuto" width="80%">
                    <tr>
                        <th style="width:50%;padding-left:100px;">
                            <h4>
                                Available Report Field
                            </h4>
                        </th>
                        <th style="width:50%">
                            <h4>
                                Report Field
                            </h4>
                        </th>
                    </tr>
                </table>
                <div class="clear"></div>
                <div id="divReportField">
                    <div style="overflow:auto; width:350px; height:300px; border:1px solid #abadb3; margin-left:100px;margin-right:30px; float: left;">
                        <asp:TreeView ID="TreeViewField" runat="server" ShowExpandCollapse="true" ForeColor="black" TabIndex="5"
                            ExpandDepth="10"  ShowLines="false">
                        </asp:TreeView>
                      <%--OnTreeNodeExpanded="TreeViewField_OnTreeNodeExpanded"--%>  
                    </div>
                    <div style="float: left;">
                        <br /><br /><br /><br />
                        <asp:ImageButton ID="btnInsert" runat="server" OnClick="btnInsert_Click" ImageUrl="~/Images/right-arrow-key.png" Width="30px" Height="30px" ToolTip="Add Report Field" TabIndex="6"/>
                        
                        <br /><br /><br /><br />
                        <asp:ImageButton ID="btnRemove" runat="server" OnClick="btnRemove_Click" ImageUrl="~/Images/left-arrow-key.png" Width="30px" Height="30px" ToolTip="Remove Report Field" TabIndex="7"/>
                    </div>
                    <div style="float: left; margin-left:20px;margin-right:20px">
                        <asp:ListBox ID="listReport" runat="server" SelectionMode="Multiple" Height="300px" Width="350px" TabIndex="8"></asp:ListBox>
                    </div>
                    <div style="float: left;">
                        <br /><br /><br /><br />
                        <asp:ImageButton ID="btnUp" runat="server" ImageUrl="~/Images/up-arrow-key.png" Width="30px" Height="30px" ToolTip="Up" 
                            OnClick="MoveUp_Click" TabIndex="9"/>
                        <br /><br /><br /><br />
                        <asp:ImageButton ID="btnDown" runat="server" ImageUrl="~/Images/down-arrow-key.png" Width="30px" Height="30px" ToolTip="Down" 
                            OnClick="MoveDown_Click" TabIndex="10"/>
                    </div>
                </div>
                </fieldset>        
                <div class="clear">
                </div>
                <fieldset>
                <legend>Conditional Field</legend>
                <table class="table" id="tblAuto2" width="99%">
                    <tr>
                        <th style="width:50%;padding-left:100px;">
                            <h4 style="width:40%">
                                Available Condition Field
                            </h4>
                        </th>
                        <th>
                            <h4 style="width:60%; margin-right:120px;">
                                Report Filter 
                             </h4>
                        </th>
                    </tr>
                </table>
                <div class="clear"></div>
                <div id="divConsitionalField">
                    <div style="overflow:auto; width:350px; height:300px; border:1px solid #abadb3;margin-left:100px; margin-right:30px; float: left;">
                        <asp:TreeView ID="TreeViewforConditon" runat="server" ShowExpandCollapse="true" ForeColor="black"
                             ExpandDepth="5">
                        </asp:TreeView>
                    </div>
                    <%--OnTreeNodeExpanded="TreeViewforConditon_OnTreeNodeExpanded"--%>
                    <div style="float: left;">
                        <br /><br /><br /><br />
                        <asp:ImageButton ID="btnAddCondition" runat="server" OnClick="btnAddCondition_Click" ImageUrl="~/Images/right-arrow-key.png" Width="30px" Height="30px" ToolTip="Add Report Conditional Field" />
                        <br /><br /><br /><br />
                        <asp:ImageButton ID="btnRemoveCondition" runat="server" OnClick="btnRemoveCondition_Click" ImageUrl="~/Images/left-arrow-key.png" Width="30px" Height="30px" ToolTip="Remove Report Conditional Field" />
                    </div>
                    <div style="float: left; margin-left:20px;margin-right:10px">
                        <asp:ListBox ID="listCondition" runat="server" SelectionMode="Multiple" Height="300px"
                            Width="350px"></asp:ListBox>
                    </div>
                </div>
                </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

