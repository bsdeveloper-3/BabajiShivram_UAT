<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VendorApproval_Status.aspx.cs" Inherits="Service_VendorApproval_Status"
    MasterPageFile="~/MasterPage.master" Title="Vendor Tracking" EnableEventValidation="false" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager2" ScriptMode="Release" />

    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlVendorApproval" runat="server">
        <ProgressTemplate>
            <img alt="progress" src="../images/processing.gif" />
            Processing...
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="upnlVendorApproval" runat="server">
        <ContentTemplate>
            <div align="center" style="vertical-align: top">
                <asp:Label ID="Label1" runat="server" EnableViewState="false" CssClass="errorMsg"></asp:Label>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />

                <div class="clear">
                    <fieldset class="fieldset-AutoWidth">
                        <legend>Vendor Tracking Details</legend>
                        <asp:GridView ID="gvVendordetails" runat="server" AutoGenerateColumns="False" DataKeyNames="lId" EnableViewState="false" 
                            DataSourceID="DataSourceApprovalStatus" OnRowCreated="gvVendorDetails_RowCreated" CssClass="table" Width="100%" PageSize="20">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" Text="View" runat="server" CommandArgument='<%# Bind("lId") %>' OnClick="lnkEdit_Click"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            <asp:BoundField DataField="ACName" HeaderText="Company Name" />
<%--                        <asp:BoundField DataField="VendorName" HeaderText="Vendor" />--%>
                            <asp:BoundField DataField="Address" HeaderText="Address" />
                            <asp:BoundField DataField="ContactPerson" HeaderText="Contact Person" />
                            <asp:BoundField DataField="ContactNo" HeaderText="Contact No" />
                           <asp:BoundField DataField="Email" HeaderText="Email Id" />
                            <asp:BoundField DataField="UserName" HeaderText="User Name" />
                            <asp:BoundField DataField="dtDate" HeaderText="Created Date" DataFormatString="{0:dd/MM/yyyy}" />
                            </Columns>
                        </asp:GridView>
                    </fieldset>
                    </div>
             <asp:SqlDataSource ID="DataSourceApprovalStatus" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
             SelectCommand="VN_GetApproveVendor" SelectCommandType="StoredProcedure">
             <SelectParameters>
            <%--   <asp:SessionParameter Name="lId" SessionField="lId" />--%>
             </SelectParameters>
             </asp:SqlDataSource>    
            </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


