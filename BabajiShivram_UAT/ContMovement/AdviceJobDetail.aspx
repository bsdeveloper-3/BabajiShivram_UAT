<%@ Page Title="Job Detail" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AdviceJobDetail.aspx.cs"
    Inherits="ContMovement_AdviceJobDetail" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release">
    </asp:ScriptManager>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlAdviceJobDetail" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../Images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="upnlAdviceJobDetail" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div>
                <div align="center">
                    <asp:HiddenField ID="hdnJobId" runat="server" Value="0" />
                    <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                </div>
                <div class="clear">
                </div>
                <fieldset>
                    <legend>Job Detail</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>BS Job No.
                            </td>
                            <td>
                                <asp:Label ID="lblJobRefNo" runat="server"></asp:Label>
                            </td>
                            <td>Customer
                            </td>
                            <td>
                                <asp:Label ID="lblCustName" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Consignee
                            </td>
                            <td>
                                <asp:Label ID="lblConsigneeName" runat="server"></asp:Label>
                            </td>
                            <td>ETA</td>
                            <td>
                                <asp:Label ID="lblETADate" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Branch</td>
                            <td>
                                <asp:Label ID="lblBranch" runat="server"></asp:Label>
                            </td>
                            <td>Sum of 20</td>
                            <td>
                                <asp:Label ID="lblSumof20" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Sum of 40</td>
                            <td>
                                <asp:Label ID="lblSumof40" runat="server"></asp:Label>
                            </td>
                            <td>Container Type</td>
                            <td>
                                <asp:Label ID="lblContType" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Job Creation Date</td>
                            <td>
                                <asp:Label ID="lblJobCreationDate" runat="server"></asp:Label>
                            </td>
                            <td>Job Created By</td>
                            <td>
                                <asp:Label ID="lblJobCreatedBy" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Shipping Name</td>
                            <td>
                                <asp:Label ID="lblShippingName" runat="server"></asp:Label>
                            </td>
                            <td>CFS Name
                            </td>
                            <td>
                                <asp:Label ID="lblCFSName" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Movement Complete Date
                            </td>
                            <td>
                                <asp:Label ID="lblMovementCompDate" runat="server"></asp:Label>
                            </td>
                            <td>Empty Container Return Date
                            </td>
                            <td>
                                <asp:Label ID="lblEmptyContReturnDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Container CFS Received Date
                            </td>
                            <td>
                                <asp:Label ID="lblContCFSReceivedDate" runat="server"></asp:Label>
                            </td>
                            <td>Download documents</td>
                            <td>
                                <asp:ImageButton ID="imgbtnDocuments" runat="server" ImageUrl="~/Images/file.gif" Width="18px" Height="18px" OnClick="imgbtnDocuments_Click" />
                            </td>
                        </tr>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

