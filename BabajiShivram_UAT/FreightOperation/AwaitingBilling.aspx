<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AwaitingBilling.aspx.cs" 
    Inherits="FreightOperation_AwaitingBilling" Title="Awaiting Billing" Culture="en-GB" EnableEventValidation="false" %>

<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter1" TagPrefix="uc1" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter2" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upJobDetail" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upJobDetail" runat="server">
        <ContentTemplate>
            <style type="text/css">
                .hidden
                {
                    display: none;
                }
            </style>
            <script type="text/javascript">
                
                function GridSelectAllColumn(spanChk) {
                    var oItem = spanChk.children;
                    var theBox = (spanChk.type == "checkbox") ? spanChk : spanChk.children.item[0]; xState = theBox.checked;
                    elm = theBox.form.elements;

                    for (i = 0; i < elm.length; i++) {
                        if (elm[i].type === 'checkbox' && elm[i].checked != xState)
                            elm[i].click();
                    }
                }
                
            </script>
            <div align="center">
                <asp:Label ID="lblerror" runat="server"></asp:Label>
                <asp:Label ID="lblMsgforReceived" runat="server"></asp:Label>
            </div>
            <div class="clear">
                
            </div>
            
            <cc1:TabContainer ID="TabJobDetail" runat="server" CssClass="Tab" CssTheme="None"
                AutoPostBack="True" ActiveTabIndex="0" OnActiveTabChanged="TabJobDetail_ActiveTabChanged">
                <cc1:TabPanel ID="TabPCDBilling" runat="server" TabIndex="0" HeaderText="Non Receive">
                    <ContentTemplate>
                        <div align="center">
                            <asp:Label ID="lblreceivemsg" runat="server"></asp:Label>
                            <asp:Label ID="lblMsgforNonReceived" runat="server"></asp:Label>
                        </div>
                        <div class="m">
                        </div>
                        <asp:Panel ID="pnlFilter1" runat="server">
                            <div class="fleft">
                                <table>
                                    <tr>
                                        <td>
                                            <uc1:DataFilter1 ID="DataFilter1" runat="server" />
                                        </td>
                                        <td valign="top">
                                            <asp:Button ID="BtnReceive" runat="server" Text="Receive" CssClass="buttons" OnClick="Receive_Click"
                                                ToolTip="Receive Billing File" />
                                        </td>
                                        <td valign="top">
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lnknonreceive" runat="server" OnClick="lnkNonreceiveExcel_Click"
                                                data-tooltip="&nbsp; Export To Excel">
                                                <asp:Image ID="img1" runat="server" ImageUrl="~/Images/Excel.jpg" /></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="fleft">
                                &nbsp;&nbsp;&nbsp;<span style="color:Teal;">** Aging: ATA Date to Today</span>
                            </div>
                        </asp:Panel>
                        <div style="overflow:scroll; width:100%">
                        <asp:GridView ID="gvNonRecievedJobDetail" runat="server" AutoGenerateColumns="False"
                            CssClass="table" DataKeyNames="EnqID" AllowPaging="True" AllowSorting="True" Width="100%" PageSize="20"
                            DataSourceID="PCDNonReceivedSqlDataSource" OnRowDataBound="gvNonRecievedJobDetail_RowDataBound"
                            OnPreRender="gvNonRecievedJobDetail_PreRender" OnPageIndexChanging="gvNonRecievedJobDetail_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkboxSelectAll" align="center" ToolTip="Check All" runat="server"
                                            onclick="GridSelectAllColumn(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk1" runat="server" ToolTip="Check"></asp:CheckBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Job No" SortExpression="FRJobNo">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEnqNo" runat="server" Text='<%#Eval("FRJobNo") %>' CommandName="select"
                                            CommandArgument='<%#Eval("EnqID") %>' Enabled="false"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="FRJobNo" HeaderText="BS Job No" Visible="False" />
                                <asp:BoundField DataField="Aging" HeaderText="Aging" SortExpression="Aging" />
                                <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                                <asp:BoundField DataField="AgentName" HeaderText="Agent" SortExpression="AgentName"/>
                                <asp:BoundField DataField="Consignee" HeaderText="Consignee" SortExpression="Consignee"/>
                                <asp:BoundField DataField="ModeName" HeaderText="Mode" SortExpression="ModeName"/>
                                <asp:BoundField DataField="ATADate" HeaderText="ATA" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ATADate"/>
                                <asp:BoundField DataField="TermsName" HeaderText="Terms" SortExpression="TermsName"/>
                            </Columns>
                        </asp:GridView>
                        </div>
                        <asp:SqlDataSource ID="PCDNonReceivedSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="FOP_GetAwaitingBilling" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                                <asp:Parameter Name="IsFileReceived" DbType="String" DefaultValue='0' />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="TabPCDBilling1" runat="server" TabIndex="1" HeaderText="Receive">
                    <ContentTemplate>
                        <asp:ValidationSummary ID="ValSummary" runat="server" ShowMessageBox="True" ShowSummary="False"
                            ValidationGroup="validateform" CssClass="errorMsg" EnableViewState="false" />
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                            ShowSummary="true" ValidationGroup="Required" CssClass="errorMsg" />
                        <div class="m">
                            
                        </div>
                        <asp:Panel ID="pnlFilter2" runat="server">
                            <div class="fleft">
                                <table>
                                    <tr>
                                        <td>
                                            <uc2:DataFilter2 ID="DataFilter2" runat="server" />
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lnkreceive" runat="server" OnClick="lnkreceiveExcel_Click" data-tooltip="&nbsp; Export To Excel">
                                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Excel.jpg" /></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="fleft">
                                &nbsp;&nbsp;<span style="color:Teal;">** Aging: ATA Date to Today</span>
                            </div>
                        </asp:Panel>
                        <div style="overflow:scroll; width:100%">
                        <asp:GridView ID="gvRecievedJobDetail" runat="server" AutoGenerateColumns="False"
                            CssClass="table" PagerStyle-CssClass="pgr" DataKeyNames="EnqID" AllowPaging="True"
                            AllowSorting="True" Width="100%" PageSize="60" PagerSettings-Position="TopAndBottom"
                            DataSourceID="PCDReceivedSqlDataSource" OnRowDataBound="gvRecievedJobDetail_RowDataBound"
                            OnPreRender="gvRecievedJobDetail_PreRender" OnRowCommand="gvRecievedJobDetail_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Job No" SortExpression="FRJobNo">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkNavigate" ToolTip="Billing Detail" runat="server"
                                            Text='<%#Eval("FRJobNo") %>' CommandName="navigate" CommandArgument='<%#Eval("EnqId")%>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="FRJobNo" HeaderText="Job No" Visible="false" />
                                <asp:BoundField DataField="Aging" HeaderText="Aging" SortExpression="Aging" />
                                <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                                <asp:BoundField DataField="AgentName" HeaderText="Agent" SortExpression="AgentName"/>
                                <asp:BoundField DataField="Consignee" HeaderText="Consignee" SortExpression="Consignee"/>
                                <asp:BoundField DataField="ModeName" HeaderText="Mode" SortExpression="ModeName"/>
                                <asp:BoundField DataField="ATADate" HeaderText="ATA" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ATADate"/>
                                <asp:BoundField DataField="TermsName" HeaderText="Terms" SortExpression="TermsName"/>
                                <asp:BoundField DataField="FileReceivedDate" HeaderText="File Rcvd Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="FileReceivedDate"/>
                                <asp:BoundField DataField="BillAmount" HeaderText="Bill Amount" SortExpression="BillAmount"/>
                                <asp:BoundField DataField="BillDate" HeaderText="Bill Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="BillDate"/>
                                <asp:BoundField DataField="BillDispatchDate" HeaderText="Dispatch Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="BillDispatchDate"/>
                                <asp:BoundField DataField="BackOfficeRemark" HeaderText="Remark" SortExpression="BackOfficeRemark"/>
                                
                            </Columns>
                        </asp:GridView>
                        </div>
                        <asp:SqlDataSource ID="PCDReceivedSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="FOP_GetAwaitingBilling" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                                <asp:Parameter Name="IsFileReceived" DbType="String" DefaultValue='1' />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </ContentTemplate>
                </cc1:TabPanel>
            </cc1:TabContainer>
           
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>