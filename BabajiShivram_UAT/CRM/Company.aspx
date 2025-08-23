<%@ Page Title="Company" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Company.aspx.cs"
    Inherits="CRM_Company" EnableEventValidation="false" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlCompany" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upnlCompany" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div class="clear" style="text-align: center">
                <asp:Label ID="lblError" runat="server"></asp:Label>
            </div>
            <fieldset>
                <legend>Company Detail</legend>
                <div class="m clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <!-- Filter Content Start-->
                        <div class="fleft">
                            <uc1:datafilter id="DataFilter1" runat="server" />
                        </div>
                        <!-- Filter Content END-->
                        <div class="fleft" style="margin-left: 5px; padding-top: 3px;">
                            <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click">
                                <asp:Image ID="imgExportExcel" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                            </asp:LinkButton>
                        </div>
                    </asp:Panel>
                </div>
                <div class="clear"></div>
                <div>
                    <asp:GridView ID="gvCompany" runat="server" DataSourceID="DataSourceCompany"
                        DataKeyNames="lid" Width="100%" AllowPaging="True" PageSize="20" AllowSorting="true"
                        CssClass="table" OnRowCommand="gvCompany_RowCommand" AutoGenerateColumns="False"
                        PagerStyle-CssClass="pgr" PagerSettings-Position="TopAndBottom" OnPreRender="gvCompany_PreRender">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Company Name" SortExpression="CustName">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkcustname" runat="Server" Text='<%#Eval("sName") %>' CommandName="select"
                                        CommandArgument='<%#Eval("lId") %>' CausesValidation="false"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="sName" HeaderText="Company Name" Visible="false" />
                            <asp:BoundField DataField="ContactPerson" HeaderText="Contact Person" />
                            <asp:BoundField DataField="Email" HeaderText="Email" />
                            <asp:BoundField DataField="MobileNo" HeaderText="Mobile No" />
                            <asp:BoundField DataField="ContactNo" HeaderText="Contact No" />
                            <asp:BoundField DataField="Website" HeaderText="Website" />
                            <asp:BoundField DataField="OfficeLocation" HeaderText="Office Location" />
                            <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy" />
                            <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" DataFormatString="{0:dd/MM/yyyy}" />
                        </Columns>
                        <PagerTemplate>
                            <asp:GridViewPager runat="server" />
                        </PagerTemplate>
                    </asp:GridView>
                    <asp:SqlDataSource ID="DataSourceCompany" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="CRM_GetCompanyDetail" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:SessionParameter Name="UserId" SessionField="UserId" />
                            <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

