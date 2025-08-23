<%@ Page Title="MIS Port" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MISPort.aspx.cs"
    Inherits="ExportReports_MISPort" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlExport_MISPort" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upnlExport_MISPort" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="clear"></div>
            <fieldset>
                <legend>MIS Port</legend>
                <div>
                    <asp:LinkButton ID="lnkbtnExport" runat="server" OnClick="lnkbtnExport_Click">
                        <asp:Image ID="imgExcel" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                    </asp:LinkButton>
                </div>
                <div class="clear"></div>
                <asp:GridView ID="gvPortWiseJob" runat="server" AutoGenerateColumns="False" DataKeyNames="Port" CssClass="table"
                    OnPreRender="gvPortWiseJob_PreRender" ShowFooter="false" OnRowCommand="gvPortWiseJob_RowCommand"
                    DataSourceID="DataSourcePortwise">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Port">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPortName" runat="server" Text='<%#Eval("PortName") %>' CommandName="select" CommandArgument='<%#Eval("Port") %>'></asp:LinkButton>
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
                <asp:SqlDataSource ID="DataSourcePortwise" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    DataSourceMode="DataSet" SelectCommand="EX_rptMISPort" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

