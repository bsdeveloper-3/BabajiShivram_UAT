<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VendorKYC_Approval.aspx.cs" Inherits="Service_VendorKYC_Approval"
    MasterPageFile="~/MasterPage.master" Title="Vendor  Approval" EnableEventValidation="false" Culture="en-GB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
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
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:ValidationSummary ID="ValSummary" runat="server" ShowMessageBox="True"
                    ShowSummary="False" ValidationGroup="Required" CssClass="errorMsg" EnableViewState="false" />
                <asp:Label ID="lblResult" runat="server" EnableViewState="false"></asp:Label>

                <div class="clear"></div>
               <fieldset class="fieldset-AutoWidth">
                    <legend>Vendor KYC Approval</legend>
                    <asp:GridView ID="gvVendordetails" runat="server" AutoGenerateColumns="False" DataKeyNames="lId" EnableViewState="false" 
                        DataSourceID="DataSourcePendingApproval" OnRowCreated="gvVendorDetails_RowCreated" CssClass="table" Width="100%" PageSize="20">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>                                                
                            <asp:TemplateField HeaderText="Action">
                             <ItemTemplate>
                                 <asp:LinkButton ID="lnkEdit" Text="View" runat="server" CommandArgument='<%# Bind("lId") %>' OnClick="lnkEdit_Click" ToolTip="Click to View Approve Vendor details"></asp:LinkButton>
                             </ItemTemplate>
                         </asp:TemplateField>
                            <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" SortExpression="VendorName" />
                            <asp:BoundField DataField="Division" HeaderText="Division" SortExpression="Division" />
                            <asp:BoundField DataField="ContactPerson" HeaderText="Contact Person" SortExpression="ContactPerson" />
                            <asp:BoundField DataField="ContactNo" HeaderText="Contact No" SortExpression="ContactNo" />
                           <asp:BoundField DataField="Email" HeaderText="Email Id" SortExpression="Email" />
                            <asp:BoundField DataField="UserName" HeaderText="UserName" SortExpression="UserName" />
                            <asp:BoundField DataField="dtDate" HeaderText="Created Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="dtDate" />
                        </Columns>
                    </asp:GridView>
                    </fieldset> 
                    <asp:SqlDataSource ID="DataSourcePendingApproval" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="VN_GetPendingApproval" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                    <%--  <asp:SessionParameter Name="lId" SessionField="lId" />--%>
                    </SelectParameters>
                    </asp:SqlDataSource>    
                  <%--  <asp:SqlDataSource ID="DataApprovalDetails" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="VN_GetVendorRqstApproval" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                    <asp:SessionParameter Name="lId" SessionField="lId" />
                 </SelectParameters>
              </asp:SqlDataSource>  --%>  
            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

