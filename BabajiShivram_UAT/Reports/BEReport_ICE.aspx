<%@ Page Title="ICE BE Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="BEReport_ICE.aspx.cs" Inherits="Reports_BEReport_ICE" EnableEventValidation="false" Culture="en-GB"%>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1"/>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="updPanel" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="updPanel" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center" style="vertical-align: top">
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <div class="clear"></div>
            <div>
            <fieldset class="fieldset-AutoWidth"><legend>BE Report</legend>
            <div class="clear">
                <asp:Panel ID="pnlFilter" runat="server">
                    <div class="fleft">
                        <uc1:DataFilter ID="DataFilter1" runat="server" />
                    </div>
                    <div class="fleft" style="margin-left:20px; padding-top:3px;">
                        <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                            <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                        </asp:LinkButton>
                    </div>
                </asp:Panel>
            </div>
            <div class="clear">
            </div>
            <div>
                <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                    PagerStyle-CssClass="pgr" OnPreRender="gvJobDetail_PreRender" 
                    DataKeyNames="JobId" DataSourceID="ICEDetailSqlDataSource" AllowPaging="True" AllowSorting="True" 
                    PagerSettings-Position="TopAndBottom" PageSize="20" Width="100%">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Job No" DataField="JobRefNo" SortExpression="JobRefNo" />
                        <asp:BoundField HeaderText="BE No" DataField="BENo" SortExpression="BENo" />
                        <asp:BoundField HeaderText="BE Date" DataField="BEDate" SortExpression="BEDate" DataFormatString="{0:dd/MM/yyyy}"/>
                        <asp:BoundField HeaderText="IEC No" DataField="IECNo" SortExpression="IECNo" />
                        <asp:BoundField HeaderText="Total Value" DataField="TotalValue" SortExpression="TotalValue" />
                        <asp:BoundField HeaderText="TYP" DataField="TYP" SortExpression="TYP" />
                        <asp:BoundField HeaderText="CHA No" DataField="CHANo" SortExpression="CHANo" />
                        <asp:BoundField HeaderText="First Check" DataField="FirstCheckStatus" SortExpression="FirstCheckStatus" />
                        <asp:BoundField HeaderText="Prior BE" DataField="PriorBEStatus" SortExpression="PriorBEStatus" />
                        <asp:BoundField HeaderText="SEC 48" DataField="SEC48Status" SortExpression="SEC48Status" />
                        <asp:BoundField HeaderText="App Group" DataField="AppGroup" SortExpression="AppGroup" />
                        <asp:BoundField HeaderText="Total Assess" DataField="TotalAssess" SortExpression="TotalAssess"/>
                        <asp:BoundField HeaderText="Total Pkgs" DataField="TotalPkgs" SortExpression="TotalPkgs" />
                        <asp:BoundField HeaderText="Gross WT" DataField="GrossWT" SortExpression="GrossWT" />
                        <asp:BoundField HeaderText="Total Duty" DataField="TotalDuty" SortExpression="TotalDuty" />
                        <asp:BoundField HeaderText="Fine Penalty" DataField="FinePenalty" SortExpression="FinePenalty" />
                        <asp:BoundField HeaderText="WBE No" DataField="WBENo" SortExpression="WBENo"/>
                        <asp:BoundField HeaderText="Appraisement" DataField="Appraisement" SortExpression="Appraisement" />
                        <asp:BoundField HeaderText="CURRENT_QUEUE" DataField="CURRENT_QUEUE" SortExpression="CURRENT_QUEUE" />
                        <asp:BoundField HeaderText="APPR_DATE" DataField="APPR_DATE" SortExpression="APPR_DATE" DataFormatString="{0:dd/MM/yyyy}"/>
                        <asp:BoundField HeaderText="ASSESS_DATE" DataField="ASSESS_DATE" SortExpression="ASSESS_DATE" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField HeaderText="PAYMENT_DATE" DataField="PAYMENT_DATE" SortExpression="PAYMENT_DATE" DataFormatString="{0:dd/MM/yyyy}"/>
                        <asp:BoundField HeaderText="EXAM_DATE" DataField="EXAM_DATE" SortExpression="EXAM_DATE" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField HeaderText="OOC_DATE" DataField="OOC_DATE" SortExpression="OOC_DATE" DataFormatString="{0:dd/MM/yyyy}"/>
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            </div>
            </fieldset>
            </div>
            <div>
                <asp:SqlDataSource ID="ICEDetailSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="ICE_GetBEDetail" SelectCommandType="StoredProcedure" 
			        >
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

