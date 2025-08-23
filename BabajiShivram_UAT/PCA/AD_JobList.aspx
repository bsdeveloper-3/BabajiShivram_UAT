<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AD_JobList.aspx.cs" Inherits="PCA_AD_JobList" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1"/>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="updPanel" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="images/processing.gif" />
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
            <fieldset class="fieldset-AutoWidth"><legend>Additional Job</legend>
            <div class="clear">
                <asp:Panel ID="pnlFilter" runat="server">
                    <div class="fleft">
                        <uc1:DataFilter ID="DataFilter1" runat="server" />
                    </div>
                    <div class="fleft" style="margin-left:20px; padding-top:3px;">
                        <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                            <asp:Image ID="Image1" runat="server" ImageUrl="images/Excel.jpg" ToolTip="Export To Excel" />
                        </asp:LinkButton>
                    </div>
                </asp:Panel>
            </div>
            <div class="clear">
            </div>
            <div>
                <asp:GridView ID="gvAddJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                    PagerStyle-CssClass="pgr"  OnRowCommand="gvAddJobDetail_RowCommand"
                     DataSourceID="JobDetailSqlDataSource" AllowPaging="True" AllowSorting="True" 
                    PagerSettings-Position="TopAndBottom" PageSize="20" Width="100%">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="BS Job No" SortExpression="ADJobRefNo">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkJobRefNo" CommandName="select" runat="server" Text='<%#Eval("ADJobRefNo") %>'
                                    CommandArgument='<%#Eval("JobId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CustName" HeaderText="Customer" SortExpression="Customer" />
                        <asp:BoundField DataField="CustRefNo" HeaderText="Cust Ref No" SortExpression="CustRefNo" />
                        <asp:BoundField DataField="PurposeOfJob" HeaderText="Purpose Of Job" SortExpression="Consignee" />
                        <asp:BoundField DataField="IsBillable" HeaderText="IsBillable" SortExpression="Consignee" />
                        <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Consignee" />
                        <asp:BoundField DataField="sName" HeaderText="Created By" SortExpression="Consignee" />
                        <asp:BoundField DataField="dtDate" HeaderText="Created Date" SortExpression="Consignee" />
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>

                <table id="tblNew" border="0" cellpadding="0" cellspacing="0" width="90%" bgcolor="white" visible="false" runat="server">
                <tr>
                    <td>Parent Job Number :
                    </td>
                    <td>
                        <asp:TextBox ID="txtPJobNumber" runat="server"></asp:TextBox>         
                        <asp:HiddenField ID="hdnJobId" runat="server" Value='<%#Eval("JobId") %>' />
                    </td>
                    <td>Customer Name :
                    </td>
                    <td>
                        <asp:Label ID="lblCustName" runat="server" ToolTip="Customer Name" TabIndex="2"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Job Number :
                    </td>
                    <td>
                        <asp:Label ID="lblJobNumber" runat="server"></asp:Label>
                    </td>
                    <td>
                        Consignee :
                    </td>
                    <td>
                        <asp:Label ID="lblConsignee" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>GST NO :</td>
                    <td>
                        <asp:Label ID="lblGSTNo" runat="server"></asp:Label>
                    </td>
                    <td>Customer Division :
                    </td>
                    <td>
                        <asp:Label ID="lblCustDivision" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Job Purpose :
                    </td>
                    <td>
                        <asp:Label ID="lblJobPurpose" runat="server"  ></asp:Label>
                    </td>
                    <td>Customer Plant :            
                    </td>
                    <td>
                        <%--<asp:DropDownList ID="DropDownList3" runat="server" Width="250" TabIndex="4"></asp:DropDownList>--%>
                        <asp:Label ID="lblCustPlant" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Billable :
                    </td>
                    <td>
                        <asp:Label ID="lblBillable" runat="server"></asp:Label>
                    </td>
                    <td>Billable Amount :
                        </td>   
                    <td>
                        <asp:Label ID="lblBillableAmt" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Email Approval Copy :
                    </td>
                    <td colspan="3">
                        <asp:LinkButton ID="lnkDocument" runat="server" OnClick="lnkDocument_Click"></asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Button ID="btnReject" runat="server" Text="Reject" ValidationGroup="RequiredAdditionaljob" OnClick="btnReject_Click"/>
                    
                        <asp:Button ID="btnApporove" runat="server" Text="Approve" ValidationGroup="RequiredAdditionaljob" OnClick="btnApporove_Click"/> 
                    </td>
                </tr>
                    <tr>
                        <td>Remark
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtRemark"
                                SetFocusOnError="true" Text="*" ErrorMessage="Enter Remark." Display="Dynamic"
                                ValidationGroup="RequiredAdditionaljob"></asp:RequiredFieldValidator>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark" TextMode="MultiLine" runat="server"></asp:TextBox>
                        </td>
                    </tr>
            </table>
            </div>
            </fieldset>
            </div>
            <div>
                <asp:SqlDataSource ID="JobDetailSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="AD_GetDetail" SelectCommandType="StoredProcedure" 
			        EnableCaching="true" >
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

