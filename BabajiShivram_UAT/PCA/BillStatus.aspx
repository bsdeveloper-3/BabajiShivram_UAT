<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BillStatus.aspx.cs" Inherits="PCA_BillStatus" 
Title="Billing Status" Culture="en-GB" EnableEventValidation="false" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingchecklist" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); -moz-opacity: .8; opacity: .8;">
                    <img alt="progress" src="../images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div align="center" style="vertical-align: top">
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
            </div>
            <div class="clear"></div>
            <fieldset class="fieldset-AutoWidth">
                <legend>Bill Job Tracking</legend>

                <div class="clear">
                    <asp:Label ID="lblreceivemsg" runat="server"></asp:Label>
                    <asp:Panel ID="pnlFilter" runat="server">
                        <div class="fleft">
                            <uc1:DataFilter ID="DataFilter1" runat="server" />
                        </div>
                        <div class="fleft" style="margin-left: 30px; padding-top: 3px;">
                            <asp:Button ID="BtnConsolidated" runat="server" Text="Consolidate" CssClass="buttons" OnClick="BtnConsolidated_Click" ToolTip="Consolidated File" /></td>
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
                        PagerStyle-CssClass="pgr" OnPreRender="gvJobDetail_PreRender" OnRowCommand="gvJobDetail_RowCommand" OnRowDataBound="gvJobDetail_RowDataBound"
                        DataKeyNames="JobId" DataSourceID="JobDetailSqlDataSource" AllowPaging="True" AllowSorting="True"
                        PagerSettings-Position="TopAndBottom" PageSize="20" Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkboxSelectAll2" Text="Check" runat="server" />
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk1" runat="server"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="BJV Details" SortExpression="">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkbtnBJVDetails" runat="server" Text="Show" CommandName="showBJV" CommandArgument='<%#Eval("JobId") + ";" + Eval("JobRefNo")%>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="BS Job No" SortExpression="JobRefNo">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkJobRefNo" CommandName="select" runat="server" Text='<%#Eval("JobRefNo") %>'
                                        CommandArgument='<%#Eval("JobId")  + ";" + Eval("JobType")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" Visible="false" />
                            <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                            <asp:BoundField DataField="JobType" HeaderText="Job Type" SortExpression="JobType" />
                            <asp:BoundField DataField="Aging1" HeaderText="Aging I" SortExpression="Aging1" />
                            <asp:BoundField DataField="Aging2" HeaderText="Aging II" SortExpression="Aging2" />
                            <asp:BoundField DataField="Aging3" HeaderText="Aging III" SortExpression="Aging3" />
                            <asp:BoundField DataField="StageAging" HeaderText="Stage Aging" SortExpression="StageAging" />
                            <asp:BoundField DataField="Stage" HeaderText="Stage" SortExpression="Stage" />
                            <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" />
                            <asp:BoundField DataField="UserName" HeaderText="User" SortExpression="UserName" />
                            <asp:TemplateField HeaderText="Document" SortExpression="OnlineBill">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkview" runat="server" Text='<%#Eval("OnlineBill") %>' CommandName="VIEW" CommandArgument='<%#Eval("JobId")+";"+ Eval("Customer")+";"+ Eval("JobRefNo")%>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                        <PagerTemplate>
                            <asp:GridViewPager ID="GridViewPager1" runat="server" />
                        </PagerTemplate>
                    </asp:GridView>
                </div>
            </fieldset>

            <div id="divDocument">

                <cc1:ModalPopupExtender ID="ModalPopupDocument" runat="server" CacheDynamicResults="false" DropShadow="False" PopupControlID="Panel2Document" TargetControlID="lnkDummy"></cc1:ModalPopupExtender>

                <asp:Panel ID="Panel2Document" runat="server" CssClass="ModalPopupPanel" ScrollBars="Both">
                    <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                    <asp:HiddenField ID="HiddenField2" runat="server" />
                    <asp:HiddenField ID="HiddenField3" runat="server" />
                    <asp:HiddenField ID="hdnCurrentId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnPrevId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnNxtId" runat="server" Value="0" />
                    <table>
                        <tr style="border-collapse: collapse">

                            <td>
                                <asp:Repeater ID="rptDocument" runat="server" OnItemDataBound="rpDocument_ItemDataBound" OnItemCommand="rpDocument_ItemCommand">
                                    <HeaderTemplate>
                                        <table class="table" cellpadding="0" cellspacing="0" width="80%">
                                            <tr class="header">
                                                <td colspan="7">
                                                    <asp:Label ID='lbldiv' runat="server" align="center" Text="Document Details" Font-Bold="true"></asp:Label>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:ImageButton ID="imgClose" align="center" ImageUrl="~/Images/delete.gif" runat="server" ToolTip="Close" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbljobrefno1" runat="server" Text="JobNo" Font-Bold="true"></asp:Label></td>
                                                <h1><%# Eval("Customer")%></h1>
                                                <td>
                                                    <asp:Label ID="lbljobrefno2" runat="server" ForeColor="Black" /></td>

                                                <td>
                                                    <asp:Label ID="lblcustomer1" runat="server" Text="Customer" Font-Bold="true"></asp:Label></td>
                                                <h1><%# Eval("JobRefNo")%></h1>
                                                <td>
                                                    <asp:Label ID="lblcustomer2" runat="server" ForeColor="Black" /></td>

                                            </tr>

                                            <%--<tr bgcolor="#FF781E">                          
                                            <th>
                                                Sl
                                            </th>
                                            <th>
                                                Name
                                            </th>
                                            <th colspan="4">
                                                Type
                                            </th>
                                </tr>  --%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <%#Container.ItemIndex +1%>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkDocType" Text='<%#DataBinder.Eval(Container.DataItem,"sName") %>'
                                                    runat="server" Checked="true" Enabled="false" />&nbsp;
                                            <asp:HiddenField ID="hdnDocId" Value='<%#DataBinder.Eval(Container.DataItem,"lId") %>'
                                                runat="server"></asp:HiddenField>
                                            </td>
                                            <td colspan="4">
                                                <asp:LinkButton ID="lnkview" runat="server" Text="View" CommandName="View" CommandArgument='<%#Eval("Docpath")%>'></asp:LinkButton>

                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                    </table>
                    </td>
        </tr>
</table>
                        
                </asp:Panel>

            </div>

            <div>
                <asp:LinkButton ID="lnkDummy" runat="server" Text=""></asp:LinkButton>
            </div>

            <!--Popup for BJV details - Start -->
            <div id="divBJVDetails">
                <cc1:ModalPopupExtender ID="mpeBJVDetails" runat="server" CacheDynamicResults="false"
                    DropShadow="False" PopupControlID="pnlBJVDetails" TargetControlID="lnkDummy2">
                </cc1:ModalPopupExtender>

                <asp:Panel ID="pnlBJVDetails" runat="server" CssClass="ModalPopupPanel">

                    <div class="header">
                        <div class="fleft">
                            <asp:Label ID='lbldiv' runat="server" align="center" Font-Bold="true"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;   
                        </div>
                        <div class="fright">
                            &nbsp;<asp:Button ID="btnCancelBJVdetails" runat="server" OnClick="btnCancelBJVdetails_Click" Text="Close" CausesValidation="false" />
                        </div>
                    </div>

                    <div id="Div2" runat="server" style="max-height: 550px; overflow: auto;">
                        <div>
                            <asp:Repeater ID="rptBJVDetails" runat="server" OnItemDataBound="rptBJVDetails_ItemDataBound">
                                <HeaderTemplate>
                                    <table class="table" cellpadding="0" cellspacing="0" width="100%" valign="top">

                                        <tr bgcolor="#FF781E">
                                            <th>SI</th>
                                            <th>VCHDATE</th>
                                            <th>VCHNO</th>
                                            <th>CONTRANAME</th>
                                            <th>CHQNO</th>
                                            <th>CHQDATE</th>
                                            <th>DEBITAMT</th>
                                            <%--<th>CREDITAMT</th><th>AMOUNT</th>--%><th>NARRATION</th>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td colspan="10">
                                            <asp:Label ID="lblmsg" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td><%#Container.ItemIndex +1%></td>
                                        <td>
                                            <asp:Label ID="lblVCHDATE" runat="server" Text='<%#Eval("VCHDATE")%>'></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblVCHNO" runat="server" Text='<%#Eval("VCHNO")%>'></asp:Label></td>
                                        <td width="200px" style="white-space: normal">
                                            <asp:Label ID="lblCONTRANAME" runat="server" Text='<%#Eval("CONTRANAME")%>'></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblCHQNO" runat="server" Text='<%#Eval("CHQNO")%>'></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblCHQDATE" runat="server" Text='<%#Eval("CHQDATE")%>'></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblDEBITAMT" runat="server" Text='<%#Eval("DEBITAMT")%>'></asp:Label></td>
                                        <%--<td><asp:Label ID="lblCREDITAMT" runat="server" Text='<%#Eval("CREDITAMT")%>'></asp:Label></td>
			                                <td><asp:Label ID="lblAMOUNT" runat="server" Text='<%#Eval("AMOUNT")%>'></asp:Label></td>--%>
                                        <td width="200px" style="white-space: normal">
                                            <asp:Label ID="lblNARRATION" runat="server" Text='<%#Eval("NARRATION")%>'></asp:Label></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <tr>
                                        <td colspan="6" align="right"><b>Total</b></td>
                                        <td>
                                            <asp:Label ID="lbltotDebitamt" runat="server"></asp:Label></td>
                                        <%--<td> <asp:Label ID="lbltotCREDITAMT" runat="server"></asp:Label></td>
                                            <td> <asp:Label ID="lbltotAMOUNT" runat="server"></asp:Label></td>--%>
                                        <td></td>
                                    </tr>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div>
                <asp:LinkButton ID="lnkDummy2" runat="server" Text=""></asp:LinkButton>
            </div>
            <!--Popup for BJV details - END -->
            
            <div>
                <asp:SqlDataSource ID="JobDetailSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="GetJobDetailForBilling" SelectCommandType="StoredProcedure" DataSourceMode="DataSet" EnableCaching="true" CacheDuration="60">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>

             <!--Popup for Consolidate - Start -->
            <asp:Button ID="modelPopup2" ValidationGroup="Required" runat="server" Style="display: none" />

            <cc1:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="modelPopup2" PopupControlID="Panel2"></cc1:ModalPopupExtender>
                <asp:Panel ID="Panel2" Style="display: none; min-width: 40%" runat="server">
                    <fieldset class="ModalPopupPanel">
                        <div title="Consolidated Details" class="header">
                            <textbox>Consolidated Details</textbox>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="imgClose" align="center" ImageUrl="~/Images/delete.gif" runat="server" ToolTip="Close" />
                        </div>
                        <div class="AutoExtenderList">
                            <td>
                                <asp:Label ID="lblConsolidatederror" runat="server"></asp:Label>
                            </td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblConsolidated" runat="server" Text="Jobno" ForeColor="Black" Font-Size="9"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpjobno" runat="server">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:Button ID="btnsave" align="center" runat="server" Text="Save" OnClick="btnsave_click" /></td>
                                </tr>
                            </table>
                        </div>
                    </fieldset>
                </asp:Panel>
             <!--Popup for Consolidate - END -->

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

