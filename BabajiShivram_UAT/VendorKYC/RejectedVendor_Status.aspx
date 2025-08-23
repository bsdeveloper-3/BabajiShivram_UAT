<%--<%--<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RejectedVendor_Status.aspx.cs" Inherits="Service_RejectedVendor_Status" %>--%>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RejectedVendor_Status.aspx.cs" Inherits="Service_RejectedVendor_Status"
    MasterPageFile="~/MasterPage.master" Title="Rejected Vendor" EnableEventValidation="false" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager2" ScriptMode="Release" />

    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnRejectedvendor" runat="server">
        <ProgressTemplate>
            <img alt="progress" src="../images/processing.gif" />
            Processing...
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="upnRejectedvendor" runat="server">
        <ContentTemplate>
            <div align="center" style="vertical-align: top">
                <asp:Label ID="lblreject" runat="server" EnableViewState="false" CssClass="errorMsg"></asp:Label>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
                </div>
                   <div class="clear"></div>
                     <div>
                    <fieldset class="fieldset-AutoWidth">
                        <legend>Rejected Vendor Details</legend>
                        <asp:GridView ID="gvRejectedVendor" runat="server" AutoGenerateColumns="False" DataKeyNames="lId" EnableViewState="false" 
                            DataSourceID="DataSourceRejectedVendor" OnRowCreated="gvVendorDetails_RowCreated" CssClass="table" Width="100%" PageSize="20">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" Text="View" runat="server" CommandArgument='<%# Bind("lId") %>' OnClick="lnkEdit_Click" ToolTip="Click to Reject Vendor details"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                             <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" SortExpression="VendorName" />
                             <asp:BoundField DataField="Division" HeaderText="Division" SortExpression="Division" />
                             <asp:BoundField DataField="ContactPerson" HeaderText="Contact Person" SortExpression="ContactPerson" />
                             <asp:BoundField DataField="ContactNo" HeaderText="Contact No" SortExpression="ContactNo" />
                            <asp:BoundField DataField="Email" HeaderText="Email Id" SortExpression="Email" />
<%--                            <asp:BoundField DataField="sName" HeaderText="Status" />--%>
                             <asp:BoundField DataField="UserName" HeaderText="UserName" SortExpression="UserName" />
                             <asp:BoundField DataField="dtDate" HeaderText="Created Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="dtDate" />
                            </Columns>
                        </asp:GridView>
                    </fieldset>                  
             <asp:SqlDataSource ID="DataSourceRejectedVendor" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
             SelectCommand="VN_GetRejectedVendor" SelectCommandType="StoredProcedure">
             <SelectParameters>
            <%--   <asp:SessionParameter Name="lId" SessionField="lId" />--%>
             </SelectParameters>
             </asp:SqlDataSource>               
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


