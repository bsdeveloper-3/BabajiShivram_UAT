<%@ Page Title="Asian Paint - Breakwall" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
        CodeFile="Asian_Breakwall.aspx.cs" Inherits="Reports_Asian_Breakwall" Culture="en-GB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingchecklist" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <%--<div>
                &nbsp;&nbsp;&nbsp;<asp:DropDownList ID="ddFCL" runat="server" OnSelectedIndexChanged="ddFCL_OnSelectedIndexChanged" 
                    AutoPostBack="true" Visible="false">
                    <asp:ListItem Value="0" Text="ALL"></asp:ListItem>
                    <asp:ListItem Value="1" Text="FCL"></asp:ListItem>
                    <asp:ListItem Value="2" Text="LCL"></asp:ListItem>
                </asp:DropDownList>
            </div>--%>
            <div class="clear"></div>
            <fieldset><legend>Asian Paint - Breakwall</legend>    
            <div>
                <asp:LinkButton ID="lnkReportXls" runat="server" OnClick="lnkReportXls_Click">
                    <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                </asp:LinkButton>
            </div>
            <div class="clear"></div>
                <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="True" CssClass="table"
                    ShowFooter="false" DataSourceID="DataSourceReport">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </fieldset>  
            <div>
                <asp:SqlDataSource ID="DataSourceReport" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                 SelectCommand="rptAsianPaintBreakWL" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter Name="UserId" SessionField="UserId" />
                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                </SelectParameters>
            </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


