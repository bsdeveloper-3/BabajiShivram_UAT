<%@ Page Title="MIS Branch" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MISBranch.aspx.cs"
    Inherits="ExportReports_MISBranch" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlExport_MISBranch" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upnlExport_MISBranch" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="clear"></div>
            <fieldset>
                <legend>MIS Branch</legend>
                <div>
                    <asp:LinkButton ID="lnkbtnExport" runat="server" OnClick="lnkbtnExport_Click">
                        <asp:Image ID="imgExcel" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                    </asp:LinkButton>
                </div>
                <div class="clear"></div>
                <asp:GridView ID="gvBranchWiseJob" runat="server" AutoGenerateColumns="False" DataKeyNames="Branch" CssClass="table"
                    OnPreRender="gvBranchWiseJob_PreRender" ShowFooter="false" OnRowCommand="gvBranchWiseJob_RowCommand"
                    DataSourceID="DataSourceBranchwise">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Port">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPortName" runat="server" Text='<%#Eval("BranchName") %>' CommandName="select" CommandArgument='<%#Eval("Branch") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="No of Jobs">
                            <ItemTemplate>
                                <asp:Label ID="lblJobCount" Text='<%#Eval("NoOfJobs") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblgrdJobCount" runat="server" Font-Bold="true"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="40 Con">
                            <ItemTemplate>
                                <asp:Label ID="lblCon40" Text='<%#Eval("Con40") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblgrdCon40" runat="server" Font-Bold="true"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="20 Con">
                            <ItemTemplate>
                                <asp:Label ID="lblCon20" Text='<%#Eval("Con20") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblgrdCon20" runat="server" Font-Bold="true"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="LCL">
                            <ItemTemplate>
                                <asp:Label ID="lblLCL" Text='<%#Eval("LCL") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblgrdLCL" runat="server" Font-Bold="true"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="TEU">
                            <ItemTemplate>
                                <asp:Label ID="lblTEU" Text='<%#Eval("TEU") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblgrdTEU" runat="server" Font-Bold="true"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Packages">
                            <ItemTemplate>
                                <asp:Label ID="lblNoOfPackages" Text='<%#Eval("NoOfPackages") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblgrdNoOfPackages" runat="server" Font-Bold="true"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Gross Weight (K.G)">
                            <ItemTemplate>
                                <asp:Label ID="lblGrossWt" Text='<%#Eval("GrossWt") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="DataSourceBranchwise" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    DataSourceMode="DataSet" SelectCommand="EX_rptMISBranch" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

