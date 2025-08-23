<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ExPendingPCD.aspx.cs" Inherits="ExportPCA_ExPendingPCD"
    EnableEventValidation="false" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <script language="javascript" type="text/javascript" src="../JS/CheckBoxListPCDDocument.js"></script>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlPendingPCA" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upnlPendingPCA" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                <%-- <asp:LinkButton ID="lnkConsoleCover" runat="server" Text="Consolidated Cover" OnClick="lnkConsoleCover_Click"></asp:LinkButton>
                <asp:DropDownList ID="ddCustomer" runat="server">
                    <asp:ListItem Text="DOW CHEMICAL LTD" Value="8"></asp:ListItem>
                    <asp:ListItem Text="BASF INDIA LTD" Value="24"></asp:ListItem>
                    <asp:ListItem Text="INTL FLAVOURS & FRAGRANCES" Value="22"></asp:ListItem>
                </asp:DropDownList>--%>
            </div>
            <div class="clear"></div>
            <fieldset class="fieldset-AutoWidth">
                <legend>Pending PCA</legend>
                <div class="clear">
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:DataFilter ID="DataFilter1" runat="server" />
                        </div>
                        <div class="fleft" style="margin-left: 40px;">
                            <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                                <asp:Image ID="Image2" runat="server" ImageUrl="../Images/Excel.jpg" ToolTip="Export To Excel" />
                            </asp:LinkButton>
                        </div>
                    </asp:Panel>
                </div>
                <div class="clear"></div>
                <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                    PagerStyle-CssClass="pgr" DataKeyNames="JobId" AllowPaging="True" AllowSorting="True" Width="100%" DataSourceID="PCDSqlDataSource"
                    PageSize="20" PagerSettings-Position="TopAndBottom" OnRowCommand="gvJobDetail_RowCommand" OnPreRender="gvJobDetail_PreRender">

                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="BS Job No" SortExpression="JobRefNo">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkJobNo" runat="server" Text='<%#Eval("JobRefNo") %>' CommandName="GetShippingDetail" CommandArgument='<%#Eval("JobId") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" Visible="false" />
                        <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                        <asp:BoundField DataField="Shipper" HeaderText="Shipper" SortExpression="Shipper" />
                        <asp:BoundField DataField="ConsigneeName" HeaderText="Consignee" SortExpression="ConsigneeName" />
                        <asp:BoundField DataField="LEODate" HeaderText="Supretendent LEO Date" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="ShippingLineDate" HeaderText="Shipping Line Date" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:TemplateField HeaderText="Aging I" SortExpression="Aging">
                            <ItemTemplate>
                                <asp:Label ID="lblAgingOne" runat="server" Text='<%#Eval("Aging") %>' ToolTip="Today – Document Hand Over To Shipping Line Date."></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PCA Documents">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkCustomerDoc" runat="server" Text="List of Documents for PCA" CommandName="DocumentPopup" CommandArgument='<%#Eval("JobId")+";"+ Eval("DocFolder")+";"+ Eval("FileDirName")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Create/Print PCA Letter">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPCALetter" CommandName="CreatePCALetter" CommandArgument='<%#Eval("JobId") %>' runat="server" Text="Create"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Forward to Dispatch Dept">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDispatch" runat="server" Text="Forward to Dispatch Dept" OnClientClick="return confirm('Are you sure wants to Move the Job To Dispatch ?');"
                                    CommandName="ForwdToDispatch" CommandArgument='<%#Eval("JobId") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" />
                    </PagerTemplate>
                </asp:GridView>
            </fieldset>
            <div>
                <asp:SqlDataSource ID="PCDSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="EX_GetPendingPCDToCustomer" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
            <!--PCA To Customer Start -->
            <div id="divDocument">
                <cc1:ModalPopupExtender ID="ModalPopupDocument" runat="server" CacheDynamicResults="false"
                    DropShadow="False" PopupControlID="Panel2Document" TargetControlID="lnkDummy">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="Panel2Document" runat="server" CssClass="ModalPopupPanel">
                    <div align="center">
                    </div>
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
                    <!--Document for BIlling Advice Start-->
                    <div id="Div1" runat="server" style="max-height: 550px; overflow: auto;">
                        <asp:HiddenField ID="hdnJobId" runat="server" />
                        <asp:HiddenField ID="hdnUploadPath" runat="server" />

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
                </asp:Panel>
            </div>
            <div>
                <asp:LinkButton ID="lnkDummy" runat="server" Text=""></asp:LinkButton>
            </div>
            <!--PCA To Customer End -->
            <!--Consolidated COVER Letter Start -->
            <%--<div id="divConsoleCover">
                <cc1:ModalPopupExtender ID="ModalPopupCustomer" runat="server" CacheDynamicResults="false"
                    DropShadow="False" PopupControlID="Panel2Customer" TargetControlID="lnkDummy">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="Panel2Customer" runat="server" CssClass="ModalPopupPanel">
                    <div align="center">
                    </div>
                    <div class="header">
                        <div class="fleft">
                            &nbsp;<asp:Button ID="btnCancelCustomer" runat="server" OnClick="btnCancelCustomer_Click" Text="Close" CausesValidation="false" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnConsolidatedCoverPDF" Text="Create Cover PDF" runat="server" OnClick="btnConsolidatedCoverPDF_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnConsolidatedCoverXLS" Text="Create Cover Excel" runat="server" OnClick="btnConsolidatedCoverXLS_Click" />
                        </div>
                        <div class="fright">
                            <asp:ImageButton ID="imgCancelCustomer" ImageUrl="~/Images/delete.gif" runat="server" OnClick="btnCancelCustomer_Click" ToolTip="Close" />
                        </div>
                    </div>
                    <div class="m"></div>
                    <!--Document for BIlling Advice Start-->
                    <div id="Div3" runat="server" style="max-height: 550px; overflow: auto;">
                        <div>
                            <asp:GridView ID="gvCustomer" runat="server" AutoGenerateColumns="False" CssClass="table"
                                PagerStyle-CssClass="pgr" DataKeyNames="JobId" AllowPaging="False" AllowSorting="True" Width="100%"
                                DataSourceID="SqlDataSourceCustomer">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BS Job No" SortExpression="JobRefNo">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkJobNo" runat="server" Text='<%#Eval("JobRefNo") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" Visible="false" />
                                    <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                                    <asp:BoundField DataField="ConsigneeName" HeaderText="Consignee" SortExpression="ConsigneeName" />
                                    <asp:TemplateField HeaderText="PCA To Customer" SortExpression="PCDRequired">
                                        <ItemTemplate>
                                            <%# (Boolean.Parse(Eval("PCDRequired").ToString())) ? "Yes" : "No"%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="OutOfChargeDate" HeaderText="Out of Charge Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="OutOfChargeDate" />
                                    <asp:BoundField DataField="LastDispatchDate" HeaderText="Dispatch Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="LastDispatchDate" />
                                </Columns>
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSourceCustomer" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="GetPendingPCDByCustId" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddCustomer" PropertyName="SelectedValue" Name="CustomerID" />
                                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </div>
                </asp:Panel>
            </div>--%>
            <!--Consolidated COVER Letter End -->
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

