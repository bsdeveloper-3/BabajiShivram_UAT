<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/ExportCustomerMaster.master" AutoEventWireup="true"
    CodeFile="CustomerExportDashboard.aspx.cs" Inherits="CustomerExport_CustomerExportDashboard" EnableEventValidation="false" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
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

        .modalPopup1 {
            border-radius: 5px;
            background-color: #FFFFFF;
            border-width: 1px;
            border-style: solid;
            border-color: black;
            padding-top: 5px;
            padding-left: 3px;
            width: 600px;
            height: 300px;
        }
    </style>
    <asp:UpdatePanel ID="upCustomerUser" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>          

            <div style="float: left; margin-left: 10px; width: 55%;">

                <fieldset>
                    <legend>No. Of Job - Port Wise</legend>
                    <asp:LinkButton ID="lnkJobPortwise" runat="server" 
                        data-tooltip="&nbsp; &nbsp; &nbsp; Export To Excel">
                        <asp:Image ID="Image8" runat="server" ImageUrl="../Images/Excel.jpg" />
                    </asp:LinkButton>     <%--OnClick="lnkJobPortWise_Click"--%>
                    <div style="height: 150px; overflow: scroll">
                        <asp:GridView ID="GrdPortWise" runat="server" AutoGenerateColumns="False" DataKeyNames="lid" Style="white-space: normal"
                            Width="99%" CssClass="table" DataSourceID="DataSourcePortWise"> <%--OnRowCommand="GrdPortWise_RowCommand"--%>
                            <Columns>
                                <asp:TemplateField HeaderText="Sl" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                             <%--   <asp:TemplateField HeaderText="Job No" ItemStyle-Width="25%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkPriorityShipmentJobNo" runat="server" Text='<%#Eval("JobRefNo") %>'
                                            CommandArgument='<%#Eval("lid")+","+ Eval("ActivityStatus") %>' CommandName="PriorityShipmentJob" ForeColor="Black"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:BoundField DataField="PortName" HeaderText="Port Name"  ItemStyle-Width="25%" />
                                <asp:BoundField DataField="NoOfJob" HeaderText="No Of Job"  ItemStyle-Width="10%" />
                                <asp:BoundField DataField="Count20" HeaderText="20"  ItemStyle-Width="10%" />
                                <asp:BoundField DataField="Count40" HeaderText="40"  ItemStyle-Width="10%" />
                                <asp:BoundField DataField="CountLCL" HeaderText="LCL"  ItemStyle-Width="10%" />
                                <%--<asp:BoundField DataField="NoOfDaysPending" HeaderText="No of days pending"  ItemStyle-Width="15%" />--%>
                            </Columns>
                        </asp:GridView>
                        <div>
                            <asp:SqlDataSource ID="DataSourcePortWise" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="GetPortWiseCountCustExp" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="CustUserId" SessionField="CustUserId" />
                                    <asp:SessionParameter Name="CustId" SessionField="CustId" />
                                    <asp:SessionParameter Name="FinYearID" SessionField="FinYearId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </div>
                </fieldset>

                <fieldset>
                <legend>Summary of Total Shipment - Aging wise</legend>
                <div>
                  <%--  <asp:LinkButton ID="lnkAgeingXls" runat="server" OnClick="lnkAgeingXls_Click">
                    <asp:Image ID="Image9" runat="server" ImageUrl="images/Excel.jpg" ToolTip="Export To Excel" />
                    </asp:LinkButton>--%>
                    <asp:GridView ID="gvAgeingDays" runat="server" AutoGenerateColumns="False" DataKeyNames="Age" 
                        CssClass="table" ShowFooter="false" Width="99%" DataSourceID="DataSourceAgeingDays">
                        <Columns>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex +1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Age" HeaderText="Aging"/>
                            <asp:TemplateField HeaderText="No of Jobs">
                                <ItemTemplate>
                                    <%--<asp:Label ID="lblJobCount" Text='<%#Eval("NoOfJobs") %>' runat="server"></asp:Label>--%>
                                    <asp:LinkButton ID="lnkJobCountShipment" runat="server" Text='<%#Eval("NoOfJobs") %>' 
                                            CommandArgument='<%#Eval("NoOfJobs")%>'></asp:LinkButton> <%--OnClick="lnkJobCountShipment_Click"--%>
                                    <asp:Label ID="lblAgeSr" runat="server" Font-Bold="true" Text='<%#Eval("AgeSr") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblgrdJobCount" runat="server" Font-Bold="true"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="40 Con">
                                <ItemTemplate>
                                    <asp:Label ID="lblCon40" Text='<%#Eval("Con40") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblgrdCon40" runat="server" Font-Bold="true"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="20 Con">
                                <ItemTemplate>
                                    <asp:Label ID="lblCon20" Text='<%#Eval("Con20") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblgrdCon20" runat="server" Font-Bold="true"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="LCL">
                                <ItemTemplate>
                                    <asp:Label ID="lblLCL" Text='<%#Eval("LCL") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblgrdLCL" runat="server" Font-Bold="true"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="No of Packages">
                                <ItemTemplate>
                                    <asp:Label ID="lblNoOfPackages" Text='<%#Eval("NoOfPackages") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblgrdNoOfPackages" runat="server" Font-Bold="true"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Gross Weight (kg)">
                                <ItemTemplate>
                                    <asp:Label ID="lblGrossWt" Text='<%#Eval("GrossWt") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblgrdGrossWt" runat="server" Font-Bold="true"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                </fieldset>
                <div>
                    <asp:SqlDataSource ID="DataSourceAgeingDays" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="Ex_AgeingDaysCustUser" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:SessionParameter Name="CustomerUserId" SessionField="CustUserId" />
                            <asp:SessionParameter Name="CustId" SessionField="CustId" />
                            <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                        </SelectParameters>
                    </asp:SqlDataSource>
        
                </div>

                <fieldset>
                    <legend>Priority Shipment</legend>
                    <asp:LinkButton ID="lnkPriorityWiseList" runat="server" OnClick="lnkPriorityWiseList_Click"
                        data-tooltip="&nbsp; &nbsp; &nbsp; Export To Excel">
                        <asp:Image ID="Image3" runat="server" ImageUrl="../Images/Excel.jpg" />
                    </asp:LinkButton>
                    <div style="height: 263px; overflow: scroll">
                        <asp:GridView ID="gvPriorityShipments" runat="server" AutoGenerateColumns="False" DataKeyNames="lid" Style="white-space: normal"
                            Width="99%" CssClass="table" DataSourceID="DataSourcePriorityShipment" OnRowCommand="gvPriorityShipments_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Job No" ItemStyle-Width="25%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkPriorityShipmentJobNo" runat="server" Text='<%#Eval("JobRefNo") %>'
                                            CommandArgument='<%#Eval("lid")+","+ Eval("ActivityStatus") %>' CommandName="PriorityShipmentJob" ForeColor="Black"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CustName" HeaderText="Customer Name"  ItemStyle-Width="35%" />
                                <asp:BoundField DataField="Status" HeaderText="Stage"  ItemStyle-Width="20%" />
                                <asp:BoundField DataField="NoOfDaysPending" HeaderText="No of days pending"  ItemStyle-Width="15%" />
                            </Columns>
                        </asp:GridView>
                        <div>
                            <asp:SqlDataSource ID="DataSourcePriorityShipment" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="CC_GetPriorityJobByCustId" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="UserId" SessionField="CustUserId" />
                                    <asp:SessionParameter Name="FinYearID" SessionField="FinYearId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </div>
                </fieldset>
              
            </div>
         
            <div style="float: right; margin-left: 2px; margin-right: 10px; width: 40%;">
                <fieldset>                    
                    <legend>No. of Job Pending - Stage Wise</legend>    
                    <asp:LinkButton ID="lnkStageWisePendinglist" runat="server" 
                        data-tooltip="&nbsp; &nbsp; &nbsp; Export To Excel"><%--OnClick="lnkStageWisePendinglist_Click"--%>
                        <asp:Image ID="Image1" runat="server" ImageUrl="../Images/Excel.jpg" />
                    </asp:LinkButton>
                    <%--<asp:Label ID="lblDemo" runat="server" Text="Testing"></asp:Label>--%>
                    <asp:GridView ID="gvPendingJob" runat="server" RowStyle-Wrap="false"  AutoGenerateColumns="false" Style="height: 100%" Width="99%"
                        DataSourceID="DataSourcePendingDeptWise" CssClass="table" FooterStyle-ForeColor="Black" OnRowDataBound="gvPendingJob_RowDataBound"
                        FooterStyle-CssClass="table" Font-Bold="False" Font-Italic="False" FooterStyle-BackColor="#CCCCFF"
                        OnRowCommand="gvPendingJob_RowCommand">
                        <RowStyle Wrap="true"/>  
                        <Columns>
                            <asp:TemplateField HeaderText="SI">
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("StatusID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="StatusName" HeaderText="Stage Name" SortExpression="StatusName" ItemStyle-Width="15%"
                                ItemStyle-Wrap="true" />

                            <asp:TemplateField HeaderText="No Of Jobs">
                                <ItemTemplate>
                                    <asp:Label ID="lblPendingJob" runat="server" Text='<%#Eval("OpenBAL") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Show Detail">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkNoOfStagePending" runat="server" Text="Show Detail" CommandName="show"
                                        CommandArgument='<%#Eval("StatusID") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="DataSourcePendingDeptWise" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="GetJobPendingStageWiseCustExp" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:SessionParameter Name="UserId" SessionField="CustUserId" />
                            <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </fieldset>
                <fieldset>
                    <legend>Total No. Of Files Pending For Billing</legend>
                    <asp:LinkButton ID="lnkShipmentClearancePending" runat="server" OnClick="lnkShipmentClearancePending_Click"
                        data-tooltip="&nbsp; &nbsp; &nbsp; &nbsp; Export To Excel">
                        <asp:Image ID="imgShipmentClearancePending" runat="server" ImageUrl="../Images/Excel.jpg" />
                    </asp:LinkButton>
                    <asp:GridView ID="gvShipmentGetInPending" runat="server" FooterStyle-BackColor="#CCCCFF"
                        OnRowDataBound="gvShipmentGetInPending_RowDataBound" ShowFooter="true" RowStyle-Wrap="false"
                        AutoGenerateColumns="false" Style="height: 100%" Width="99%" DataSourceID="SQlDataSourceForShipmentGetIn"
                        CssClass="table">
                        <RowStyle Wrap="true" />
                        <Columns>
                            <asp:BoundField DataField="Stage" HeaderText="Stage" SortExpression="Stage" />
                            <asp:TemplateField HeaderText="NoOfJobs">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkNoOfJobs" runat="server" Text='<%#Eval("noofjobs") %>' OnClick="lnkNoOfJobs_Click"
                                        CommandArgument='<%#Eval("Stage")%>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Creditamount" HeaderText="Credit Amount" SortExpression="Creditamount"
                                ItemStyle-Width="15%" ItemStyle-Wrap="true" Visible="false" />
                            <asp:BoundField DataField="Debitamount" HeaderText="Debit Amount" SortExpression="Debitamount"
                                ItemStyle-Width="15%" ItemStyle-Wrap="true" />
                        </Columns>
                    </asp:GridView>
                    
                    <asp:SqlDataSource ID="SQlDataSourceForShipmentGetIn" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="CC_GetPendingBillingByCustId" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:SessionParameter Name="UserId" SessionField="CustUserId" />
                            <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </fieldset>       
                
                <fieldset>
                    <legend>No. Of Job - Plant Wise</legend>
                    <asp:LinkButton ID="lnkBranch" runat="server" 
                        data-tooltip="&nbsp; &nbsp; &nbsp; Export To Excel">
                        <asp:Image ID="imgPlant" runat="server" ImageUrl="../Images/Excel.jpg" />
                    </asp:LinkButton>     <%--OnClick="lnkJobPortWise_Click"--%>
                    <div style="height: 150px; overflow: scroll">
                        <asp:GridView ID="grdPlant" runat="server" AutoGenerateColumns="False" DataKeyNames="lid" Style="white-space: normal"
                            Width="99%" CssClass="table" DataSourceID="DataSourcePlantWise"> <%--OnRowCommand="GrdPortWise_RowCommand"--%>
                            <Columns>
                                <asp:TemplateField HeaderText="Sl" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                             <%--   <asp:TemplateField HeaderText="Job No" ItemStyle-Width="25%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkPriorityShipmentJobNo" runat="server" Text='<%#Eval("JobRefNo") %>'
                                            CommandArgument='<%#Eval("lid")+","+ Eval("ActivityStatus") %>' CommandName="PriorityShipmentJob" ForeColor="Black"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:BoundField DataField="PlantName" HeaderText="Plant Name"  ItemStyle-Width="25%" />
                                <asp:BoundField DataField="NoOfJob" HeaderText="No Of Job"  ItemStyle-Width="10%" />
                                <asp:BoundField DataField="Count20" HeaderText="20"  ItemStyle-Width="10%" />
                                <asp:BoundField DataField="Count40" HeaderText="40"  ItemStyle-Width="10%" />
                                <asp:BoundField DataField="CountLCL" HeaderText="LCL"  ItemStyle-Width="10%" />
                                <%--<asp:BoundField DataField="NoOfDaysPending" HeaderText="No of days pending"  ItemStyle-Width="15%" />--%>
                            </Columns>
                        </asp:GridView>
                        <div>
                            <asp:SqlDataSource ID="DataSourcePlantWise" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="GetPlantWiseCountCustExp" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="CustUserId" SessionField="CustUserId"/>
                                    <asp:SessionParameter Name="FinYearID" SessionField="FinYearId"/>
                                    <asp:SessionParameter Name="CustId" SessionField="CustId"/>
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </div>
                </fieldset>        
   
                 <div>
                    <asp:HiddenField ID="modelPopup12" runat="server" />
                    <AjaxToolkit:ModalPopupExtender ID="ModalPopupExtender3" runat="server" TargetControlID="modelPopup12"
                        PopupControlID="Panel3" BackgroundCssClass="modalBackground" DropShadow="true"
                        CancelControlID="ImageButton2">
                    </AjaxToolkit:ModalPopupExtender>

                    <asp:Panel ID="Panel3" runat="server" CssClass="modalPopup" BackColor="#F5F5DC" Width="650px" Height="300px">
                        <div id="div3" runat="server">
                            <table width="100%">
                                <tr class="heading">
                                    <td align="center" style="background-color: #F5F5DC">
                                        <b>&nbsp;&nbsp;<asp:Label ID="lblTitle" runat="server" Text=""></asp:Label></b>
                                        &nbsp;&nbsp;&nbsp;
                                            <asp:LinkButton ID="lnkStageDetail" runat="server" ToolTip="Export To Excel"> <%--OnClick="lnkStageDetail_Click" --%>
                                                <asp:Image ID="Image5" runat="server" ImageUrl="~/Images/Excel.jpg" Style="margin-top: 5px" />
                                            </asp:LinkButton>
                                        <span style="float: right">
                                            <asp:ImageButton ID="ImageButton2" ImageUrl="~/Images/delete.gif" runat="server" OnClick="btnCancelPopup1_Click" ToolTip="Close" />
                                        </span>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="Panel5" runat="server" Width="645px" Height="265px" ScrollBars="Auto" BorderStyle="Solid" BorderWidth="1px">
                                <asp:GridView ID="grvPopupForSagePending" runat="server" AutoGenerateColumns="false" AllowPaging="false"
                                    CssClass="gridview" OnRowCommand="grvPopupForSagePending_RowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex +1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Job No">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkJobNo" runat="server" Text='<%#Eval("JobRefNo") %>'
                                                    CommandArgument='<%#Eval("lid") + ";" + Eval("ActivityStatus")%>' CommandName="RedirectJob" ForeColor="Black"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CustName" HeaderText="Party Name" SortExpression="CustName" />
                                        <asp:BoundField DataField="CurrentDate" HeaderText="Arrived On Date" SortExpression="CurrentDate" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="NoOfDaysPending" HeaderText="NoOfDaysPending" SortExpression="NoOfDaysPending" />
                                    </Columns>
                                </asp:GridView>

                            </asp:Panel>
                        </div>
                    </asp:Panel>
                </div>
                
                <div>
                    <asp:LinkButton ID="modelPopup" runat="server" />
                    <AjaxToolkit:ModalPopupExtender ID="mpeShipmentGetIn" runat="server" TargetControlID="modelPopup" 
                        PopupControlID="pnlShipmentGetInDetails" DropShadow="true">
                    </AjaxToolkit:ModalPopupExtender>

                    <asp:Panel ID="pnlShipmentGetInDetails" runat="server" Style="display: none">
                    <fieldset>
                        <legend>
                            <asp:Label ID="lblbillingpendingFiles" runat="server" Text=""></asp:Label></legend></b>
                            <div class="header">
                                    <div class="fleft">
                                        <asp:LinkButton ID="lnkExportShipmentDetails" runat="server" OnClick="lnkExportShipmentDetails_Click" ToolTip="Export To Excel">
                                            <asp:Image ID="Image2" runat="server" ImageUrl="~/images/Excel.jpg" Style="margin-top: 5px" />
                                        </asp:LinkButton>
                                    </div>
                                    <div class="fright">
                                        <asp:ImageButton ID="imgClose" ImageUrl="~/Images/delete.gif" runat="server" OnClick="btnCancelPopup_Click" ToolTip="Close" />
                                    </div>
                                </div>
                                <div class="clear"></div>
                            <div id="Div321" runat="server" style="max-height: 550px; overflow: auto;">
                                    <asp:GridView ID="gvShipmentDetails" runat="server" CssClass="table" AutoGenerateColumns="false"
                                        AllowPaging="false" OnRowCommand="gvShipmentDetails_RowCommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sl">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex +1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Job No">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkShipmentJobNo" runat="server" Text='<%#Eval("JobRefNo") %>'
                                                        CommandArgument='<%#Eval("lid")%>' CommandName="ShipmentJob" ForeColor="Black"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="PartyName" HeaderText="Party Name" SortExpression="PartyName" />
                                            <asp:BoundField DataField="Amount" HeaderText="Debit Amount" SortExpression="Amount" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                    </fieldset>
                        
                    </asp:Panel>
                        
                </div>
                <div>
                    <asp:LinkButton ID="modelPopup1" runat="server" />
                    <AjaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" 
                        TargetControlID="modelPopup1" PopupControlID="Panel2">
                            </AjaxToolkit:ModalPopupExtender>
                    <asp:Panel ID="Panel2" runat="server" Style="display: none">
                    <fieldset>
                    <legend>
                        <asp:Label ID="lblBillpendList" runat="server" Text=""></asp:Label></b>
                    </legend>
                        <div class="header">
                            <div class="fleft">        
                                <asp:LinkButton ID="lnkjobSummarylist" runat="server" OnClick="lnkjobSummarylist_Click" ToolTip="Export To Excel">
                                    <asp:Image ID="Image4" runat="server" ImageUrl="~/images/Excel.jpg" Style="margin-top: 5px" />
                                </asp:LinkButton>
                            </div>
                                            
                            <div class="fright">
                                    <asp:ImageButton ID="imgbtnSummaryShipmentDetails" ImageUrl="~/Images/delete.gif" runat="server" OnClick="btnCancelPopup_Click1" ToolTip="Close" />
                                </div>
                        </div>
                        <div class="clear"></div>
                                        
                        <asp:GridView ID="gvsummarylist" runat="server" CssClass="table" AutoGenerateColumns="false"
                            AllowPaging="false" OnRowCommand="gvsummarylist_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Job No">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBillJobNo" runat="server" Text='<%#Eval("JobrefNo") %>'
                                            CommandArgument='<%#Eval("lid")%>' CommandName="BillJob" ForeColor="Black"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="PartyName" HeaderText="Party Name" SortExpression="PartyName" />
                                <asp:BoundField DataField="Amount" HeaderText="Debit Amount" SortExpression="Amount" />
                            </Columns>
                        </asp:GridView>
                    </fieldset>
                </asp:Panel>
                </div>
                <asp:SqlDataSource ID="DsSummarylist" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="CC_GetBillPendingByCustID" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:Parameter Name="Stage" Type="Int16" />
                            <asp:SessionParameter Name="UserId" SessionField="CustUserId" />
                            <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                        </SelectParameters>
                    </asp:SqlDataSource>
            </div>
                        
            <asp:SqlDataSource ID="DataSourceShipmentGetIn" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="CC_GetPendingShipmentGetInByCustID" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:Parameter Name="Stage" Type="Int16" />
                        <asp:SessionParameter Name="UserId" SessionField="CustUserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>



               <table id="tbShipmentGetInDetails" runat="server" style="display: none">
                    <tr>
                        <td>
                            <asp:HiddenField ID="HiddenField1" runat="server" />
                            <AjaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="modelPopup" BackgroundCssClass="modalBackground"
                                PopupControlID="pnlShipmentGetInDetails" DropShadow="true">
                            </AjaxToolkit:ModalPopupExtender>

                            <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup1" BackColor="#F5F5DC" Width="605px" Height="335px">
                                <div id="div1" runat="server">
                                    <table width="100%">
                                        <tr>
                                            <td align="center">
                                                <b>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text=""></asp:Label></b>
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="lnkExportShipmentDetails_Click" ToolTip="Export To Excel">
                                                    <asp:Image ID="Image6" runat="server" ImageUrl="~/images/Excel.jpg" Style="margin-top: 5px" />
                                                </asp:LinkButton>
                                                <span style="float: right">
                                                    <asp:ImageButton ID="ImageButton1" ImageUrl="~/Images/delete.gif" runat="server" OnClick="btnCancelPopup_Click" ToolTip="Close" />
                                                </span>
                                            </td>
                                        </tr>
                                    </table>
                                 <%--   <asp:Panel ID="panScroll" runat="server" Width="600px" Height="300px" ScrollBars="Auto" BorderStyle="Solid" BorderWidth="1px">
                                        <asp:GridView ID="GridView1" runat="server" CssClass="gridview" AutoGenerateColumns="false"
                                            AllowPaging="false" OnRowCommand="gvShipmentDetails_RowCommand">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sl">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex +1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Job No">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkShipmentJobNo" runat="server" Text='<%#Eval("JobRefNo") %>'
                                                            CommandArgument='<%#Eval("lid")%>' CommandName="ShipmentJob" ForeColor="Black"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="PartyName" HeaderText="Party Name" SortExpression="PartyName" />
                                                <asp:BoundField DataField="Amount" HeaderText="Debit Amount" SortExpression="Amount" />
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>--%>
                                </div>
                            </asp:Panel>
                        </td>
                        <td>
                            <div>
                                <asp:HiddenField ID="HiddenField2" runat="server" />
                                <AjaxToolkit:ModalPopupExtender ID="ModalPopupExtender4" runat="server" TargetControlID="modelPopup1" BackgroundCssClass="modalBackground"
                                    PopupControlID="Panel2" DropShadow="true">
                                </AjaxToolkit:ModalPopupExtender>
                                <asp:Panel ID="Panel4" runat="server" CssClass="modalPopup1" BackColor="#F5F5DC" Width="405px" Height="335px">
                                    <div id="div2" runat="server">
                                        <table width="100%">
                                            <tr>
                                                <td align="center">
                                                    <b>&nbsp;&nbsp;<asp:Label ID="Label2" runat="server" Text=""></asp:Label></b>
                                                    &nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="LinkButton2" runat="server" OnClick="lnkjobSummarylist_Click" ToolTip="Export To Excel">
                                                        <asp:Image ID="Image7" runat="server" ImageUrl="~/images/Excel.jpg" Style="margin-top: 5px" />
                                                    </asp:LinkButton>
                                                    <span style="float: right">
                                                        <asp:ImageButton ID="ImageButton3" ImageUrl="~/Images/delete.gif" runat="server" OnClick="btnCancelPopup_Click1" ToolTip="Close" />
                                                    </span>
                                                </td>
                                            </tr>
                                        </table>
                                      <%--  <asp:Panel ID="Panel6" runat="server" Width="400px" Height="300px" ScrollBars="Auto" BorderStyle="Solid" BorderWidth="1px">
                                            <asp:GridView ID="GridView2" runat="server" CssClass="gridview" AutoGenerateColumns="false"
                                                AllowPaging="false" OnRowCommand="gvsummarylist_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sl">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex +1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Job No">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkBillJobNo" runat="server" Text='<%#Eval("JobrefNo") %>'
                                                                CommandArgument='<%#Eval("lid")%>' CommandName="BillJob" ForeColor="Black"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="PartyName" HeaderText="Party Name" SortExpression="PartyName" />
                                                    <asp:BoundField DataField="Amount" HeaderText="Debit Amount" SortExpression="Amount" />
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>--%>
                                    </div>
                                </asp:Panel>

                               <%-- <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="EX_GetBillPendingDetails" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:Parameter Name="Stage" Type="Int16" />
                                        <asp:ControlParameter ControlID="hdnCustId" PropertyName="Value" Name="CustWiseId" />
                                        <asp:ControlParameter ControlID="hdnBranchId" PropertyName="Value" Name="BranchwiseId" />
                                        <asp:ControlParameter ControlID="hdnUserId" PropertyName="Value" Name="UserwiseId" />
                                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>--%>
                            </div>
                        </td>
                    </tr>
                </table>
               <%-- <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="EX_GetPendingShipmentGetInDetails" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:Parameter Name="Stage" Type="Int16" />
                        <asp:ControlParameter ControlID="hdnCustId" PropertyName="Value" Name="CustWiseId" />
                        <asp:ControlParameter ControlID="hdnBranchId" PropertyName="Value" Name="BranchwiseId" />
                        <asp:ControlParameter ControlID="hdnUserId" PropertyName="Value" Name="UserwiseId" />
                        <asp:SessionParameter Name="UserId" SessionField="UserId" />
                        <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                    </SelectParameters>
                </asp:SqlDataSource>--%>
            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

