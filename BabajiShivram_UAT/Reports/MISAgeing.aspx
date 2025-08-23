<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MISAgeing.aspx.cs" Inherits="Reports_rptMISAgeing" 
    MasterPageFile="~/MasterPage.master" Title="MIS Aging" EnableEventValidation="false"%>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
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
            <div align="center">
                <asp:Label ID="lblMessage" runat="server" CssClass="info" Visible="false"></asp:Label>
            </div>
            <div class="clear"></div>
            <fieldset><legend>MIS Aging</legend>        
            <asp:LinkButton ID="lnkAgeingXls" runat="server" OnClick="lnkAgeingXls_Click">
            <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
            </asp:LinkButton>
            <div class="clear"> </div>
            <asp:GridView ID="gvAgeing" runat="server" AutoGenerateColumns="False" DataKeyNames="RangeLow,RangeHigh" CssClass="table"
                OnPreRender="gvAgeing_PreRender" OnRowCommand="gvAgeing_RowCommand" DataSourceID="AgeingSqlDataSource" Width="99%">
                <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex +1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="RangeName" HeaderText="Range Name" />
                <%--<asp:TemplateField HeaderText="RangeLow">
                    <ItemTemplate>
                        <asp:LinkButton ID="hdnRangeLow" Text='<%#Eval("RangeLow") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="RangeHigh">
                    <ItemTemplate>
                        <asp:LinkButton ID="hdnRangeHigh" Text='<%#Eval("RangeHigh") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label ID="lblHdrOpen2" Text="Job Opening" ToolTip="PreAlert Received Date To Job Opening Date" runat="server"></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnk2" runat="server" Text='<%#Eval("JobOpeningCount") %>' CommandName="select" CommandArgument='<%#Container.DataItemIndex + ";1" %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label ID="lblHdrOpen3" Text="Checklist Preparation" ToolTip="Job Opening To Checklist Preparation Date" runat="server"></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnk3" runat="server" Text='<%#Eval("ChecklistPreparationCount") %>' CommandName="select" CommandArgument='<%#Container.DataItemIndex + ";2" %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label ID="lblHdrOpen4" Text="Checklist Audit/Approval" ToolTip="Checklist Request Date To Audit/Approval" runat="server"></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnk4" runat="server" Text='<%#Eval("ChecklistApprovalCount") %>' CommandName="select" CommandArgument='<%#Container.DataItemIndex + ";3" %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label ID="lblHdrNoting5" Text="Noting" ToolTip="Date Of Checklist Audit/Approval To Noting Date" runat="server"></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnk5" runat="server" Text='<%#Eval("NotingCount") %>' CommandName="select" CommandArgument='<%#Container.DataItemIndex + ";4" %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label ID="lblHdrNoting6" Text="DO Collection" ToolTip="IGM Date To Final DO Date" runat="server"></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnk6" runat="server" Text='<%#Eval("DOCount") %>' CommandName="select" CommandArgument='<%#Container.DataItemIndex + ";5" %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label ID="lblHdrDuty7" Text="Duty" ToolTip="Duty Request Date To Duty Payment Date" runat="server"></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnk7" runat="server" Text='<%#Eval("DutyCount") %>' CommandName="select" CommandArgument='<%#Container.DataItemIndex + ";6" %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label ID="lblHdrDuty8" Text="IGM Clearance" ToolTip="IGM Date To Clearance Date" runat="server"></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnk8" runat="server" Text='<%#Eval("IGMClearanceCount") %>' CommandName="select" CommandArgument='<%#Container.DataItemIndex + ";7" %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label ID="lblHdrDuty9" Text="DO Clearance" ToolTip="Final DO Date To Clearance Date" runat="server"></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnk9" runat="server" Text='<%#Eval("DOClearanceCount") %>' CommandName="select" CommandArgument='<%#Container.DataItemIndex + ";8" %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="">
                    <HeaderTemplate>
                        <asp:Label ID="lblHdrDuty10" Text="Duty Clearance" ToolTip="Duty Payment Date To Clearance Date" runat="server"></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnk10" runat="server" Text='<%#Eval("DutyClearanceCount") %>' CommandName="select" CommandArgument='<%#Container.DataItemIndex + ";9" %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                    <asp:TemplateField HeaderText="">
                    <HeaderTemplate>
                        <asp:Label ID="lblHdrJobclr11" Text="Job Clearance" ToolTip="Job Date To Clearance Date" runat="server"></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate >
                        <asp:LinkButton ID="lnk11" runat="server" Text='<%#Eval("JobClearanceCount") %>' CommandName="select" CommandArgument='<%#Container.DataItemIndex + ";10" %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            </asp:GridView>
            </fieldset>
            <div class="clear"> </div>
            <fieldset><legend>Job Detail - Aging</legend>        
            <!-- Aging Job Detail -->
    
            <asp:Panel ID="pnlJobDetailXLS" runat="server" Visible="false">
            <asp:LinkButton ID="lnkAgeingDetailXls" runat="server" OnClick="lnkAgeingDetailXls_Click">
            <asp:Image ID="Image2" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
            </asp:LinkButton>
            </asp:Panel>
            <div class="clear">
            </div>
            <asp:GridView ID="gvAgeingJobDetail" runat="server" AutoGenerateColumns="True" CssClass="table" 
                PagerStyle-CssClass="pgr" AllowPaging="True" Width="99%" PageSize="20" PagerSettings-Position="TopAndBottom"
                DataSourceID="AgeingDetailSqlDataSource" OnPreRender="gvAgeingJobDetail_PreRender" AllowSorting="true">
                <Columns>
                    <asp:TemplateField HeaderText="Sl" >
                        <ItemTemplate>
                            <%#Container.DataItemIndex +1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerTemplate>
                <asp:GridViewPager runat="server" />
            </PagerTemplate>
            </asp:GridView>
            </fieldset>
            <div>
            <asp:SqlDataSource ID="AgeingSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                DataSourceMode="DataSet" EnableCaching="true" CacheDuration="300"
                SelectCommand="rptMISAgeing" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
            </SelectParameters>
            </asp:SqlDataSource>
        
            <asp:SqlDataSource ID="AgeingDetailSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="rptMISAgeingByRange" SelectCommandType="StoredProcedure">
                <SelectParameters>
                <asp:Parameter Name="RangeLow" Type="int32" />
                <asp:Parameter Name="RangeHigh" Type="int32" />
                <asp:Parameter Name="ReportType" Type="int32" />
                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
            </SelectParameters>
            </asp:SqlDataSource>
        </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>