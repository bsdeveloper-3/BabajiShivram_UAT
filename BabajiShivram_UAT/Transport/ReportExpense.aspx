<%@ Page Title="Vehicle Maintenance Expense Summary" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="ReportExpense.aspx.cs" Inherits="Transport_ReportExpense" EnableEventValidation="false" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
 <asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPanel" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upPanel" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <fieldset><legend>Vehicle Maintenance Expense Summary</legend>
                <div><b>Report Month</b>
                    <asp:DropDownList ID="ddMonth" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddMonth_SelectedIndexChanged">
                        <asp:ListItem Value="0" Text="Financial Year"></asp:ListItem>
                        <asp:ListItem Value="1" Text="January"></asp:ListItem>
                        <asp:ListItem Value="2" Text="February"></asp:ListItem>
                        <asp:ListItem Value="3" Text="March"></asp:ListItem>
                        <asp:ListItem Value="4" Text="April"></asp:ListItem>
                        <asp:ListItem Value="5" Text="May"></asp:ListItem>
                        <asp:ListItem Value="6" Text="June"></asp:ListItem>
                        <asp:ListItem Value="7" Text="July"></asp:ListItem>
                        <asp:ListItem Value="8" Text="Aug"></asp:ListItem>
                        <asp:ListItem Value="9" Text="Sep"></asp:ListItem>
                        <asp:ListItem Value="10" Text="Oct"></asp:ListItem>
                        <asp:ListItem Value="11" Text="Nov"></asp:ListItem>
                        <asp:ListItem Value="12" Text="Dec"></asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="lnkXls" runat="server" OnClick="lnkXls_Click" data-tooltip="&nbsp; Export To Excel">
                    <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ImageAlign="AbsMiddle" />
                    </asp:LinkButton>
                </div>
                <div>
                    <asp:GridView ID="gvSummaryMonth" runat="server" CssClass="table" DataSourceID="DataSourceMonthExpense" 
                        Width="99%" AutoGenerateColumns="true">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    </asp:GridView>
                </div>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="DataSourceMonthExpense" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="TR_rptMonthExpense" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                        <asp:ControlParameter ControlID="ddMonth" Name="MonthId" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
 </asp:Content>

