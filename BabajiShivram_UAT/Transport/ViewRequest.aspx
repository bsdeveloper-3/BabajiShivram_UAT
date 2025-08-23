<%@ Page Title="Transport Request" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewRequest.aspx.cs"
    Inherits="Transport_ViewRequest" Culture="en-GB" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upnlRequestRecd" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <style type="text/css">
        .heading {
            line-height: 20px;
        }

        .modalBackground {
            background-color: black;
            filter: alpha(opacity=90);
            opacity: 0.5;
        }

        .modalPopup {
            border-radius: 5px;
            background-color: #FFFFFF;
            border-width: 1px;
            border-style: solid;
            border-color: black;
            padding-top: 5px;
            padding-left: 3px;
            width: 300px;
            height: 140px;
        }
    </style>
    <script type="text/javascript">
        function ActiveTabChanged12() {
            /* Clear Error Message on Tab change Event */
            document.getElementById('<%=lblMessage.ClientID%>').innerHTML = '';
            document.getElementById('<%=lblMessage.ClientID%>').className = '';
        }

        function divexpandcollapse(divname) {
            var div = document.getElementById(divname);
            var img = document.getElementById('img' + divname);

            if (div.style.display == "none") {
                div.style.display = "inline";
                img.src = "Images/minus.png";
                img.title = 'Collapse';
            }
            else {
                div.style.display = "none";
                img.src = "Images/plus.png";
                img.title = 'Expand';
            }
        }
    </script>
    <asp:UpdatePanel ID="upnlRequestRecd" runat="server">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </div>
            <AjaxToolkit:TabContainer runat="server" ID="TabRequestRecd" ActiveTabIndex="0" CssClass="Tab"
                Width="100%" OnClientActiveTabChanged="ActiveTabChanged12" AutoPostBack="true">
                <AjaxToolkit:TabPanel runat="server" ID="TabPanelNormalJob" TabIndex="0" HeaderText="Normal Job Detail">
                    <ContentTemplate>
                        <div style="overflow: auto;">
                            <div class="m clear">
                                <div align="center">
                                    <asp:Label ID="lblError_Job" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="m clear">
                                <asp:Panel ID="pnlFilter" runat="server">
                                    <div class="fleft">
                                        <uc1:DataFilter ID="DataFilter1" runat="server" />
                                    </div>
                                </asp:Panel>
                                <asp:Button ID="btnVehiclePlaced" runat="Server" Text="Save Vehicle Placed" Visible="false" OnClick="btnVehiclePlaced_Click" />
                                <asp:Button ID="btnConsolidate" runat="server" OnClick="btnConsolidate_Click" Text="Add Consolidate" />
                                <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click">
                                    <asp:Image ID="Image2" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                                </asp:LinkButton>
                                <div class="fright">
                                    <asp:TextBox ID="txtVehiclePlacedNote" runat="server" Enabled="false" Style="background-color: #2266aa63" Width="50px"></asp:TextBox>
                                    Vehicle Placed
                                </div>
                            </div>
                            <div class="clear"></div>
                            <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table" DataKeyNames="lId"
                                PagerStyle-CssClass="pgr" AllowPaging="True" AllowSorting="True" Width="100%" PageSize="20" PagerSettings-Position="TopAndBottom"
                                OnRowDataBound="gvJobDetail_RowDataBound" OnRowCommand="gvJobDetail_RowCommand"
                                OnPreRender="gvJobDetail_PreRender" DataSourceID="TruckRequestSqlDataSource">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" Visible="false">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" CausesValidation="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ref No" SortExpression="JobRefNo">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkRefNo" CommandName="select" runat="server" Text='<%#Eval("TRRefNo") %>'
                                                CommandArgument='<%#Eval("lId")%>' Font-Bold="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="TRRefNo" HeaderText="Ref No" Visible="false" />
                                    <asp:BoundField DataField="JobRefNo" HeaderText="Job No" />
                                    <asp:BoundField DataField="CustName" HeaderText="Customer" SortExpression="CustName" />
                                    <asp:BoundField DataField="RequestType" HeaderText="Type" Visible="false" />
                                    <asp:BoundField DataField="VehiclesRequired" HeaderText="Vehicles Req" SortExpression="VehiclesRequired" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="LocationFrom" HeaderText="Location" SortExpression="LocationFrom" />
                                    <asp:BoundField DataField="Destination" HeaderText="Destination" SortExpression="Destination" />
                                    <asp:BoundField DataField="NoOfPkgs" HeaderText="Pkgs" SortExpression="NoOfPkgs"
                                        ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="GrossWeight" HeaderText="Weight (Kgs)" SortExpression="GrossWeight"
                                        ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="Count20" HeaderText="20" SortExpression="Count20" />
                                    <asp:BoundField DataField="Count40" HeaderText="40" SortExpression="Count40" />
                                    <asp:BoundField DataField="CountLCL" HeaderText="LCL" SortExpression="CountLCL" />
                                    <asp:BoundField DataField="DeliveryType" HeaderText="Type" SortExpression="DeliveryType" />
                                    <asp:BoundField DataField="RequestDate" HeaderText="Request Date" SortExpression="RequestDate" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="VehiclePlaceDate" HeaderText="Vehicle Place Required" SortExpression="VehiclePlaceDate" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="RequestedBy" HeaderText="Requested By" SortExpression="RequestedBy" />
                                </Columns>
                                <PagerTemplate>
                                    <asp:GridViewPager ID="GridViewPager1" runat="server" />
                                </PagerTemplate>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
                <AjaxToolkit:TabPanel ID="TabPanelConsolidateJob" runat="server" TabIndex="1" HeaderText="Consolidate Job Detail">
                    <ContentTemplate>
                        <div style="overflow: auto;">
                            <div class="m clear">
                                <div align="center">
                                    <asp:Label ID="lblError_ConsolidateJob" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="m clear">
                                <asp:Panel ID="pnlFilter2" runat="server">
                                    <div class="fleft">
                                        <uc1:DataFilter ID="DataFilter2" runat="server" />
                                    </div>
                                    <div class="fleft" style="margin-left: 30px; padding-top: 3px;">
                                        <asp:LinkButton ID="lnkExport_Consolidate" runat="server" OnClick="lnkExport_Consolidate_Click">
                                            <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                                        </asp:LinkButton>
                                    </div>
                                </asp:Panel>
                            </div>
                            <div class="m clear"></div>
                            <asp:GridView ID="gvConsolidateJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
                                Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lid"
                                DataSourceID="DataSourceConsolidateVehicle" CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20"
                                OnRowCommand="gvConsolidateJobDetail_RowCommand" OnRowDataBound="gvConsolidateJobDetail_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ref No" SortExpression="TRRefNo">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkSelect" CommandName="select" runat="server" Text='<%# Bind("TransRefNo")%>'
                                                ToolTip="Click To Update Bill Detail" CommandArgument='<%#Eval("lid") + ";" + Eval("TransReqId") + ";" + Eval("TransRefNo")%>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Ref No" DataField="TransRefNo" ReadOnly="true" Visible="true" />
                                    <asp:BoundField HeaderText="Transporter" DataField="Transporter" ReadOnly="true" SortExpression="Transporter" />
                                    <asp:BoundField HeaderText="VehicleNo" DataField="VehicleNo" ReadOnly="true" SortExpression="VehicleNo" />
                                    <asp:BoundField HeaderText="JobRefNo" DataField="JobRefNo" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Job Created By" DataField="CreatedBy" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Job Created Date" DataField="CreatedDate" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
            </AjaxToolkit:TabContainer>
            <div>
                <asp:SqlDataSource ID="DataSourceConsolidateJobs" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="TR_GetConsolidateRequests" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="TruckRequestSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="TR_GetVehicleRequest" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                        <asp:Parameter Name="JobType" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceConsolidateVehicle" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="TR_GetConsolidateRequestRecd" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
            <%--MODAL POPUP FOR CONSOLIDATE JOBS--%>
            <div>
                <asp:HiddenField ID="hdnConsolidateJob" runat="server" />
                <AjaxToolkit:ModalPopupExtender ID="mpeConsolidateJob" runat="server" TargetControlID="hdnConsolidateJob" BackgroundCssClass="modalBackground"
                    PopupControlID="pnlhdnConsolidateJob" DropShadow="true">
                </AjaxToolkit:ModalPopupExtender>
                <asp:Panel ID="pnlhdnConsolidateJob" runat="server" CssClass="modalPopup1" BackColor="#F5F5DC" Width="900px" Height="365px" Style="border: 1px solid #cccc; border-radius: 5px">
                    <div id="div2" runat="server">
                        <table width="100%">
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <b><u>Create Consolidate Job</u></b>
                                    <span style="float: right">
                                        <asp:ImageButton ID="imgbtnClose" ImageUrl="~/Images/delete.gif" runat="server" OnClick="imgbtnClose_Click" ToolTip="Close" />
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblError_Popup" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <asp:Panel ID="pnlpnlhdnConsolidateJob2" runat="server" Width="880px" Height="325px" ScrollBars="Vertical" Style="padding: 3px">
                            <table border="0" cellpadding="0" cellspacing="0" width="99%" bgcolor="white">
                                <tr>
                                    <td>Transporter
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:DropDownList ID="ddlTransporter" runat="server" AppendDataBoundItems="true" Width="300px">
                                            <asp:ListItem Text="-Select transporter name" Value="0" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnAddConsolidate" runat="server" Text="Save Consolidate" OnClick="btnAddConsolidate_Click" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:GridView ID="gvAddConsolidateJobDetail" runat="server" CssClass="table" AutoGenerateColumns="false" Width="99%" Style="white-space: normal; border: 1px solid #cccc; border-radius: 3px">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRowNumber" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Trans Req Id" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTransReqId" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Trans Ref No" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTransRefNo" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Job Ref No" ItemStyle-Width="130px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblJobRefNo" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Customer">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomer" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Location">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLocation" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Destination">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDestination" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnDeleteRow" runat="server" OnClick="btnDeleteRow_Click" Text="Remove" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </div>
                </asp:Panel>
            </div>
            <%--MODAL POPUP FOR CONSOLIDATE JOBS--%>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

