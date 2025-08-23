<%@ Page Title="Maintenance Detail" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="EditBranchMaintenance.aspx.cs" Inherits="Service_EditBranchMaintenance" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:ScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release"/>
    <div align="center">
        <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
    </div>
    <div class="m clear">
        <asp:Button ID="btnBack" Text="Back" CausesValidation="false" OnClick="btnBack_OnClick" runat="server" TabIndex="13" />
    </div>
    <fieldset><legend id="legRefNo" runat="server">Work Detail</legend>
    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
        <tr>
            <td>
                Ref No
            </td>
            <td>
                <asp:Label ID="lblRefNo" runat="server" ></asp:Label>
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
                Work Date
            </td>
            <td>
                <asp:Label ID="lblWorkDate" runat="server" ></asp:Label>
            </td>
            <td>
            </td>
            <td>
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
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
        <th style="color:#FFFFFF;background-color:#5A7FA5;" colspan="2">Branch/Expense Category</th><th style="color:#FFFFFF;background-color:#5A7FA5">Document</th>
        <tr>
            <td class="label" colspan="2">
                <asp:GridView ID="gvCategory" runat="server" AutoGenerateColumns="false" DataSourceID="DSCategory">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%# Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Branch" DataField="BranchName" />
                        <asp:BoundField HeaderText="Category" DataField="CategoryName" />
                        <asp:BoundField HeaderText="Amount" DataField="Amount" />
                        <asp:BoundField HeaderText="Bill No" DataField="BillNumber" />
                        <asp:BoundField HeaderText="Paid To" DataField="PaidTo" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="DSCategory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="IMS_GetMaintainCategory" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="MaintenanceID" SessionField="BrMaintainId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
            
            <td class="label" colspan="2">
                <asp:BulletedList ID="blDocument" DataSourceID="DSDownloadDocument" DataTextField="DocName"
                    DataValueField="DocPath" runat="server" DisplayMode="LinkButton" CausesValidation="false"
                    Target="_blank" OnClick="blDocument_Click" ToolTip="Download Document" CssClass="ulList" TabIndex="35">
                    <asp:ListItem Text="Document Not Uploaded" Value="0" Enabled="false"></asp:ListItem>
                </asp:BulletedList>
                <asp:SqlDataSource ID="DSDownloadDocument" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="IMS_GetMaintainDocument" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="MaintenanceID" SessionField="BrMaintainId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>

        </tr>
    </table>
   </fieldset>
    <fieldset><legend>Add Expense Category</legend>
    <table border="0" cellpadding="0" cellspacing="0" width="99%" bgcolor="white">
        <tr>
            <td width="110px" align="center">
                Branch
            </td>
            <td width="50px" align="center">
                <asp:Label ID="lblBranchName" Text="" runat="server"></asp:Label>
            </td>
            <td align="center">
                Category
                <asp:RequiredFieldValidator ID="RFVCategory" runat="server" ControlToValidate="ddCategory" Display="Dynamic" ValidationGroup="validateVehicle"
                    InitialValue="0" SetFocusOnError="true" Text="*" ErrorMessage="Select Category"></asp:RequiredFieldValidator>
            </td>    
            <td>
                <asp:DropDownList ID="ddCategory" runat="server"></asp:DropDownList>
                <asp:Button ID="btnSaveVehicle" runat="server" Text="Save" OnClick="btnSaveVehicle_Click" ValidationGroup="validateVehicle"  />
            </td>
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
