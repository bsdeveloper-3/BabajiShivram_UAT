<%@ Page Title="Pending Billing Advice" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BillingAdvice.aspx.cs"
    Inherits="ContMovement_BillingAdvice" EnableEventValidation="false" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="content1" runat="server">
    <%--<cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />--%>
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <script src="../JS/GridViewCellEdit.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript" src="../JS/CheckBoxListPCDDocument.js"></script>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upShipment" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upShipment" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </div>
            <div class="clear"></div>
            <fieldset class="fieldset-AutoWidth">
                <legend>Pending Billing Advice</legend>
                <div class="m clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:datafilter id="DataFilter1" runat="server" />
                        </div>
                        <div class="fleft" style="margin-left: 40px;">
                            <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                                <asp:Image ID="Image2" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                            </asp:LinkButton>
                        </div>
                    </asp:Panel>
                </div>
                <div class="clear">
                </div>
                <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                    PagerStyle-CssClass="pgr" DataKeyNames="JobId" AllowPaging="True" AllowSorting="True" Width="99%"
                    PageSize="20" PagerSettings-Position="TopAndBottom" OnPreRender="gvJobDetail_PreRender" DataSourceID="PCDSqlDataSource"
                    OnRowCommand="gvJobDetail_RowCommand" OnRowDataBound="gvJobDetail_RowDataBound">
                    <Columns>
                        <asp:ButtonField Text="SingleClick" CommandName="SingleClick" Visible="False" />
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Move To Scrutiny">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkScrutiny" runat="server" Text="Send" CommandName="SendForScrutiny" CommandArgument='<%#Eval("JobId") %>'
                                    OnClientClick="return confirm('Are you sure to Move the Job To Billing Scrutiny ?');" ToolTip="Move Job To Billing Scrutiny"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Billing Documents">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDocument" runat="server" Text="List of Billing Advice Doc" CommandName="DocumentPopup" CommandArgument='<%#Eval("JobId")+";"+ Eval("DocFolder")+";"+ Eval("FileDirName")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="BS Job No" SortExpression="JobRefNo">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkJobNo" runat="server" Text='<%#Eval("JobRefNo") %>' CommandName="select" CommandArgument='<%#Eval("JobId")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" Visible="false" />
                        <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                        <asp:BoundField DataField="ConsigneeName" HeaderText="Consignee" SortExpression="ConsigneeName" />
                        <asp:BoundField DataField="CFSName" HeaderText="CFS" SortExpression="CFSName" />
                        <asp:BoundField DataField="EmptyContReturnDate" HtmlEncode="false" HeaderText="Empty Container Return Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="EmptyContReturnDate" />
                        <asp:BoundField DataField="ContCFSReceivedDate" HeaderText="Container CFS Received Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ContCFSReceivedDate" />
                        <asp:BoundField DataField="BackOfficeDate" HeaderText="Forwarded to Advice" DataFormatString="{0:dd/MM/yyyy}" SortExpression="BackOfficeDate" />
                        <asp:TemplateField HeaderText="Instructions" SortExpression="Instructions">
                            <ItemTemplate>
                                <asp:Label ID="labInstructions" runat="server" Text='<%# Eval("Instructions") %>'></asp:Label>
                                <asp:Button ID="btnInstructions" runat="server" Text="" OnCommand="txtInstructions_Changed" CommandArgument='<%# Bind("JobId") %>' Style="display: none" />
                                <asp:TextBox ID="txtInstructions" runat="server" Text='<%# Eval("Instructions") %>' Width="175px"
                                    Style="display: none" TextMode="MultiLine"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Billing Scrutiny Rejection Date" DataField="RejectedDate" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField HeaderText="Rejection Remark" DataField="RejectRemark" />
                        <asp:BoundField HeaderText="Rejected By" DataField="RejectedBy" />
                        <asp:BoundField HeaderText="Remark" />
                        <asp:TemplateField HeaderText="Aging I">
                            <ItemTemplate>
                                <asp:Label ID="lblAgingOne" runat="server" Text='<%#Eval("Aging") %>' ToolTip="Today – Document Hand Over To Shipping Line Date."></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            </fieldset>
            <!--Document for BIlling Advice Start-->
            <div id="divDocument">
                <cc1:modalpopupextender id="ModalPopupDocument" runat="server" cachedynamicresults="false"
                    dropshadow="False" popupcontrolid="Panel2Document" targetcontrolid="lnkDummy">
                </cc1:modalpopupextender>

                <asp:Panel ID="Panel2Document" runat="server" CssClass="ModalPopupPanel">
                    <div class="header">
                        <div class="fleft">
                            &nbsp;<asp:Button ID="btnCancelPopup" runat="server" OnClick="btnCancelPopup_Click" Text="Close" CausesValidation="false" />
                            &nbsp;&nbsp;&nbsp;&nbsp;    
                            <asp:Button ID="btnSaveDocument" Text="Save Document" runat="server" OnClick="btnSaveDocument_Click" ValidationGroup="Required" />
                        </div>
                        <div class="fright">
                            <asp:ImageButton ID="imgClose" ImageUrl="~/Images/delete.gif" runat="server" OnClick="btnCancelPopup_Click" ToolTip="Close" />
                        </div>
                    </div>
                    <div class="m"></div>
                    <div id="Div1" runat="server" style="max-height: 550px; overflow: auto;">
                        <asp:HiddenField ID="hdnJobId" runat="server" />
                        <asp:HiddenField ID="hdnUploadPath" runat="server" />
                        <!--Document for BIlling Advice Start-->
                        <div>
                            <asp:Repeater ID="rptDocument" runat="server" OnItemDataBound="rpDocument_ItemDataBound">
                                <HeaderTemplate>
                                    <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr bgcolor="#FF781E">
                                            <th>Sl
                                            </th>
                                            <th>Name
                                            </th>
                                            <th>Type
                                            </th>
                                            <th>Browse
                                            </th>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <%#Container.ItemIndex +1%>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkDocType" Text='<%#DataBinder.Eval(Container.DataItem,"sName") %>'
                                                runat="server" />&nbsp;
                                        <asp:HiddenField ID="hdnDocId" Value='<%#DataBinder.Eval(Container.DataItem,"lId") %>'
                                            runat="server"></asp:HiddenField>
                                        </td>
                                        <td>
                                            <asp:CustomValidator ID="CVCheckBoxList" runat="server" ClientValidationFunction="ValidateCheckBoxList"
                                                Enabled="false" ErrorMessage="Please Select Type" ValidationGroup="Required" Display="Dynamic"></asp:CustomValidator>
                                            <asp:CheckBoxList ID="chkDuplicate" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Original" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Copy" Value="2"></asp:ListItem>
                                            </asp:CheckBoxList>

                                        </td>
                                        <td>
                                            <asp:FileUpload ID="fuDocument" runat="server" Enabled="false" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                    <!--Document for BIlling Advice- END -->
                </asp:Panel>
            </div>
            <div>
                <asp:LinkButton ID="lnkDummy" runat="server" Text=""></asp:LinkButton>
            </div>
            <!--Document for BIlling Advice- END -->
            <div>
                <asp:SqlDataSource ID="PCDSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CM_GetPendingBillingAdvice" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
