<%@ Page Title="Pre Alert" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ExportPreAlert.aspx.cs"
    Inherits="ExportCHA_ExportPreAlert" Culture="en-GB" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upFile" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <div>
        <fieldset>
            <legend>Pre-Alert Detail</legend>
            <asp:UpdatePanel ID="upFile" runat="server">
                <ContentTemplate>
                    <div align="center">
                        <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                    </div>
                    <div class="m">
                        <!-- Filter Content Start-->
                        <uc1:DataFilter ID="DataFilter1" runat="server" />
                        <!-- Filter Content END-->
                    </div>
                    <div>
                        <asp:GridView ID="gvPreAlert" runat="server" AutoGenerateColumns="False" CssClass="table"
                            AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                            DataSourceID="GridviewSqlDataSource" AllowPaging="True" AllowSorting="True" PagerSettings-Position="TopAndBottom"
                            OnPreRender="gvPrealert_PreRender" OnRowCommand="gvPreAlert_RowCommand">
                            <%--OnRowDataBound="gvJobDetail_RowDataBound" PageSize="20" >--%>
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex +1%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="View">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="Server" Text="View" CommandName="select"
                                            CausesValidation="false" CommandArgument='<%#Eval("lid") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CustRefNo" HeaderText="Cust Ref No" SortExpression="CustRefNo" />
                                <asp:BoundField DataField="ENQRefNo" HeaderText="ENQ Ref No" SortExpression="ENQRefNo" />
                                <asp:BoundField DataField="FRJobNo" HeaderText="FRJobNo" SortExpression="FRJobNo" />
                                <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                                <asp:BoundField DataField="Consignee" HeaderText="Consignee" SortExpression="Consignee" />
                                <asp:BoundField DataField="PortOfLoading" HeaderText="Port Of Loading" SortExpression="PortOfLoading" />
                                <asp:BoundField DataField="Mode" HeaderText="Mode" SortExpression="Mode" />
                                <asp:BoundField DataField="lid" HeaderText="EnqId" SortExpression="lid" Visible="false"/>
                                <%--<asp:BoundField DataField="FRJobNo" HeaderText="FRJobNo" SortExpression="FRJobNo" Visible="false"/>--%>
                                <asp:BoundField DataField="sName" HeaderText="Created By" SortExpression="sName" />
                                <asp:BoundField DataField="dtDate" HeaderText="Created Date" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" SortExpression="dtDate" />
                                
                                <%--<asp:BoundField DataField="CustRefNo" HeaderText="CustRefNo" SortExpression="CustRefNo" />--%>
                            <%--    <asp:BoundField DataField="KAMName" HeaderText="KAM" SortExpression="KAMName" />
                                <asp:BoundField DataField="CustInstruction" HeaderText="Remark" SortExpression="CustInstruction" />--%>
                              <%--  <asp:BoundField DataField="dtDate" HeaderText="Receive Date" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" SortExpression="dtDate" />--%>
                               <%-- <asp:TemplateField HeaderText="Delete" Visible="false">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkeDelete" runat="Server" Text="Delete" CommandName="deletePreAlert"
                                            OnClientClick="return confirm('Sure to delete PreAlert document?');" CausesValidation="false" CommandArgument='<%#Eval("lid") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                            </Columns>
                            <PagerTemplate>
                                <asp:GridViewPager runat="server" />
                            </PagerTemplate>
                        </asp:GridView>
                    </div>
                    <div>
                        <asp:SqlDataSource ID="GridviewSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="GetPreAlertForFrightExport" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>

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
                                </div>
                                <div class="fright">
                                    <asp:ImageButton ID="imgClose" ImageUrl="~/Images/delete.gif" runat="server" OnClick="btnCancelPopup_Click" ToolTip="Close" />
                                </div>
                            </div>
                            <div class="m"></div>
                            <!--Document for BIlling Advice Start-->
                            <div id="Div1" runat="server" style="max-height: 550px; min-width: 400px; overflow: auto;">
                                <asp:HiddenField ID="hdnJobId" runat="server" />
                                <asp:HiddenField ID="hdnUploadPath" runat="server" />
                                <asp:GridView ID="GridViewPreAlertDoc" runat="server" CssClass="table" AutoGenerateColumns="false" EmptyDataText="No files uploaded">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Text" HeaderText="File Name" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDownload" Text="Download" CommandArgument='<%# Eval("Value") %>' runat="server" OnClick="DownloadPreAlertFile"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                    </div>
                    <div>
                        <asp:LinkButton ID="lnkDummy" runat="server" Text=""></asp:LinkButton>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </fieldset>
    </div>
</asp:Content>

