<%@ Page Title="Un-Processed Job Detail" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UnProcessJobs.aspx.cs"
    Inherits="ContMovement_UnProcessJobs" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release">
    </asp:ScriptManager>

    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upMovementDetail" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../Images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="upMovementDetail" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:ValidationSummary ID="vsMovementDetail" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="vgRequired" CssClass="errorMsg" />
            </div>
            <div class="clear">
            </div>
            <asp:Button ID="btnProcessJobs" runat="server" Text="Process" OnClick="btnProcessJobs_Click" />
            <div class="clear"></div>
            <fieldset class="fieldset-AutoWidth">
                <legend>Un-Processed Job Detail</legend>
                <div class="clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:DataFilter ID="DataFilter1" runat="server" />
                        </div>
                        <div class="fleft" style="margin-left: 2px;">
                            <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                                <asp:Image ID="Image1" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                            </asp:LinkButton>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtcolor1" runat="server" Style="background-color: #668cbbb0; border-radius: 4px" Width="20px" Height="10px" Enabled="false"></asp:TextBox>
                            <asp:Label ID="lblColorName1" runat="server" Text="PN Movement in our scope" Font-Bold="true"></asp:Label>
                        </div>
                    </asp:Panel>
                </div>
                <div class="clear">
                </div>
                <asp:GridView ID="gvMovementDetail" runat="server" AutoGenerateColumns="False" Width="100%" PagerStyle-CssClass="pgr"
                    DataKeyNames="JobId" OnRowCommand="gvMovementDetail_RowCommand" AllowPaging="True" AllowSorting="True" CssClass="table" PageSize="20"
                    PagerSettings-Position="TopAndBottom" DataSourceID="DataSourceMovementDetail" OnRowDataBound="gvMovementDetail_RowDataBound">
                    <Columns>
                        <asp:TemplateField ShowHeader="false">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelectJob" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnShowDocuments" runat="server" ImageUrl="~/Images/file.gif" Height="18" Width="20" ToolTip="Click to view documents."
                                    CommandName="ShowDocs" CommandArgument='<%#Eval("JobId") + ";" + Eval("JobRefNo")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" SortExpression="JobRefNo" ReadOnly="true" />
                        <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" ReadOnly="true" />
                        <asp:BoundField DataField="ConsigneeName" HeaderText="Consignee" SortExpression="ConsigneeName" ReadOnly="true" />
                        <asp:BoundField DataField="ETADate" HeaderText="ETA" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ETADate" ReadOnly="true" />
                        <asp:BoundField DataField="BranchName" HeaderText="Branch" SortExpression="BranchName" ReadOnly="true" />
                        <asp:BoundField DataField="SumOf20" HeaderText="Sum Of 20" SortExpression="SumOf20" ReadOnly="true" />
                        <asp:BoundField DataField="SumOf40" HeaderText="Sum Of 40" SortExpression="SumOf40" ReadOnly="true" />
                        <asp:BoundField DataField="ContainerType" HeaderText="Cont Type" SortExpression="ContainerType" ReadOnly="true" />
                        <asp:BoundField DataField="Remark" HeaderText="Reason" SortExpression="Remark" ReadOnly="true" />
                        <asp:BoundField DataField="JobCreationDate" HeaderText="Job Creation" DataFormatString="{0:dd/MM/yyyy}" SortExpression="JobCreationDate" ReadOnly="true" />
                        <asp:BoundField DataField="JobCreatedBy" HeaderText="Job Created By" SortExpression="JobCreatedBy" ReadOnly="true" />
                        <asp:BoundField DataField="UpdatedDate" HeaderText="Unprocessed On" DataFormatString="{0:dd/MM/yyyy}" SortExpression="UpdatedDate" ReadOnly="true" />
                        <asp:BoundField DataField="UpdatedBy" HeaderText="Unprocessed By" SortExpression="UpdatedBy" ReadOnly="true" />
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="DataSourceMovementDetail" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CM_GetUnProcessedJobs" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
            <%-- START  : Pop-up for Documents --%>
            <div>
                <asp:HiddenField ID="hdnPopup" runat="server" />
                <asp:HiddenField ID="hdnJobId" runat="server" Value="0" />
                <cc1:ModalPopupExtender ID="mpeDocument" runat="server" TargetControlID="hdnPopup" BackgroundCssClass="modalBackground" CancelControlID="imgClose"
                    PopupControlID="pnlDocument" DropShadow="true">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pnlDocument" runat="server" CssClass="modalPopup1" BackColor="#F5F5DC" Width="670px" Height="335px" BorderStyle="Solid" BorderWidth="1px">
                    <div id="div2" runat="server">
                        <table width="100%">
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td align="center">Download Documents -
                                    <asp:Label ID="lblJobRefNo" runat="server"></asp:Label>
                                    <span style="float: right">
                                        <asp:ImageButton ID="imgClose" ImageUrl="~/Images/delete.gif" runat="server" OnClick="imgClose_Click" ToolTip="Close" />
                                    </span>
                                </td>
                            </tr>
                        </table>
                        <div align="center">
                            <asp:Panel ID="pnlDownloadDocs" runat="server" Width="660px" Height="300px" ScrollBars="Auto" BorderStyle="Solid" BorderWidth="1px">
                                <asp:GridView ID="GridViewDocument" runat="server" AutoGenerateColumns="False" CssClass="table" Width="100%" AlternatingRowStyle-CssClass="alt"
                                    PagerStyle-CssClass="pgr" DataKeyNames="DocId" DataSourceID="DocumentSqlDataSource" OnRowCommand="GridViewDocument_RowCommand"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DocumentName" HeaderText="Document" />
                                        <asp:BoundField DataField="sName" HeaderText="Uploaded By" />
                                        <asp:TemplateField HeaderText="Download">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"
                                                    CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="DocumentSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetUploadedDocument" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="hdnJobId" Name="JobId" PropertyName="Value" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </asp:Panel>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <%-- END    : Pop-up for Documents --%>

            <%-- START  : Pop-up for UnProcess Job --%>
            <div>
                <asp:HiddenField ID="hdnUnProcess" runat="server" Value="0" />
                <cc1:ModalPopupExtender ID="mpeUnProcess" runat="server" TargetControlID="hdnUnProcess" BackgroundCssClass="modalBackground" CancelControlID="imgClose2"
                    PopupControlID="pnlUnProcess" DropShadow="true">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pnlUnProcess" runat="server" CssClass="modalPopup1" BackColor="#F5F5DC" Width="460px" Height="180px" BorderStyle="Solid" BorderWidth="1px">
                    <div id="div1" runat="server">
                        <table width="100%">
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td align="center"><b><u>Un-Process Job</u></b>
                                    <span style="float: right">
                                        <asp:ImageButton ID="imgClose2" ImageUrl="~/Images/delete.gif" runat="server" OnClick="imgClose2_Click" ToolTip="Close" />
                                    </span>
                                </td>
                            </tr>
                        </table>
                        <div align="center">
                            <asp:Panel ID="pnlUnProcess2" runat="server" Width="450px" Height="130px" ScrollBars="Auto">
                                <div class="m clear">
                                    <asp:Label ID="lblError_UnProcesss" runat="server"></asp:Label>
                                </div>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                    <tr>
                                        <td>Job Ref No</td>
                                        <td>
                                            <asp:TextBox ID="lblJobNo" runat="server" Enabled="false"></asp:TextBox>
                                            <asp:HiddenField ID="hdnUnProcess_JobId" runat="server" Value="0" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Reason <span style="color: red">*</span>
                                            <asp:RequiredFieldValidator ID="rfvReason" runat="server" ControlToValidate="txtReason" ErrorMessage="Required"
                                                SetFocusOnError="true" Display="Dynamic" ValidationGroup="vgUnProcess"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtReason" runat="server" Rows="3" TextMode="MultiLine" TabIndex="1"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Button ID="btnUnProcessJob" runat="server" Text="Un-Process" OnClick="btnUnProcessJob_Click" ValidationGroup="vgUnProcess" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <%-- END    : Pop-up for UnProcess Job --%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

