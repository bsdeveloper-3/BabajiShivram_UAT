<%@ Page Title="Edit Maintenance Detail" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="HOEditMaintenance.aspx.cs" Inherits="Transport_HOEditMaintenance" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:ScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release"/>
    <div align="center">
        <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
            ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" />
    </div>
    <div class="m clear">
        <asp:Button ID="btnBack" Text="Back" CausesValidation="false" OnClick="btnBack_OnClick" runat="server" TabIndex="13" />
    </div>
    <fieldset><legend id="legRefNo" runat="server">Work Detail</legend>
    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
        <tr>
            <td>
                Vehicle No 
            </td>
            <td>
                <asp:Label ID="lblVehicleNo" runat="server" ></asp:Label>
                &nbsp;-&nbsp;<asp:Label ID="lblVehicleType" runat="server" ></asp:Label>
            </td>
            <td>
                Work Location
            </td>
            <td>
                <asp:Label ID="lblWorkLocation" runat="server" ></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                Work Start Date
            </td>
            <td>
                <asp:Label ID="lblWorkDate" runat="server" ></asp:Label>
            </td>
            <td>
                Work End Date
            </td>
            <td>
                <asp:Label ID="lblWorkEndDate" runat="server" ></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                Start Time
            </td>
            <td>
                <asp:Label ID="lblStartTime" runat="server" ></asp:Label>
            </td>
            <td>
                End Time
            </td>
            <td>
                <asp:Label ID="lblEndTime" runat="server" ></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                Work Description
            </td>
            <td colspan="3">
                <asp:Label ID="lblDesc" runat="server" ></asp:Label>
            </td>
        </tr>
        <tr>
            <td>Bill No</td>
            <td>
                <asp:Label ID="lblBillNo" runat="server" ></asp:Label>
            </td>
            <td>Paid To</td>
            <td>
                <asp:Label ID="lblPaidTo" runat="server" ></asp:Label>
            </td>
        </tr>
        <tr>
            <td>Support Bill Paid To</td>
            <td>
                <asp:Label ID="lblSupportBill" runat="server" ></asp:Label>
            </td>
            <td>Pay Type</td>
            <td>
                <asp:Label ID="lblPayType" runat="server" ></asp:Label>
            </td>
        </tr>
        <th style="color:#FFFFFF;background-color:#5A7FA5;">Category</th><th style="color:#FFFFFF;background-color:#5A7FA5">Employee</th><th style="color:#FFFFFF;background-color:#5A7FA5">Document</th><th style="background-color:#5A7FA5"></th>
        <tr>
            <td class="label">
                <asp:BulletedList ID="blCategory" DataSourceID="DSCategory" DataTextField="CategoryName"
                    DataValueField="CategoryId" runat="server" DisplayMode="Text" CssClass="ulList">
                </asp:BulletedList>
                <asp:SqlDataSource ID="DSCategory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="TR_GetMaintainCategory" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="MaintenanceID" SessionField="TrMaintainId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
            <td class="label">
                <asp:BulletedList ID="blEmployee" DataSourceID="DSEmployee" DataTextField="EmpName"
                    DataValueField="EmpId" runat="server" DisplayMode="Text" CssClass="ulList">
                </asp:BulletedList>
                <asp:SqlDataSource ID="DSEmployee" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="TR_GetMaintainEmp" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="MaintenanceID" SessionField="TrMaintainId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
            <td class="label">
                <asp:BulletedList ID="blDocument" DataSourceID="DSDownloadDocument" DataTextField="DocName"
                    DataValueField="DocPath" runat="server" DisplayMode="LinkButton" CausesValidation="false"
                    Target="_blank" OnClick="blDocument_Click" ToolTip="Download Document" CssClass="ulList" TabIndex="35">
                    <asp:ListItem Text="Document Not Uploaded" Value="0" Enabled="false"></asp:ListItem>
                </asp:BulletedList>
                <asp:SqlDataSource ID="DSDownloadDocument" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="TR_GetMaintainDocument" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="MaintenanceID" SessionField="TrMaintainId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
            <td></td>
        </tr>
    </table>
   </fieldset>
   <fieldset><legend>Upload Document</legend>
    <table border="0" cellpadding="0" cellspacing="0" width="99%" bgcolor="white">
        <tr>
            <td width="110px" align="center">
                Document Name
                <asp:RequiredFieldValidator ID="RFVDocName" runat="server" ControlToValidate="txtDocName" Display="Dynamic" ValidationGroup="validateDocument"
                    SetFocusOnError="true" Text="*" ErrorMessage="Enter Document Name."></asp:RequiredFieldValidator>
            </td>
            <td width="50px" align="center">
                <asp:TextBox ID="txtDocName" runat="server"></asp:TextBox>
                <asp:HiddenField ID="hdnUploadPath" runat="server" />
            </td>
            <td align="center">
                Attachment
                <asp:RequiredFieldValidator ID="RFVAttach" runat="server" ControlToValidate="fuDocument" Display="Dynamic" ValidationGroup="validateDocument"
                    SetFocusOnError="true" Text="*" ErrorMessage="Attach File For Upload."></asp:RequiredFieldValidator>
            </td>    
            <td>
                <asp:FileUpload ID="fuDocument" runat="server" />
                <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnFileUpload_Click" ValidationGroup="validateDocument"  />
            </td>
        </tr>
    </table>
    </fieldset>
</asp:Content>


