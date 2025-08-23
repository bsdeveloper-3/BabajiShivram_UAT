<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BillStatusDetailSez.aspx.cs" Inherits="PCA_BillStatusDetailSez" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="GVPager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <style type="text/css">
        .Tab .ajax__tab_header {
            white-space: nowrap !important;
        }
    </style>
    <AjaxToolkit:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upJobDetail" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); -moz-opacity: .8; opacity: .8;">
                    <img alt="progress" src="../images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <script type="text/javascript">

        function ActiveTabChanged12() {
            /* Clear Error Message on Tab change Event */
            document.getElementById('<%=lblError.ClientID%>').innerHTML = '';
            document.getElementById('<%=lblError.ClientID%>').className = '';
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

    <script type="text/javascript">

        function OnPortOfLoadingSelected(source, eventArgs) {
            // alert(eventArgs.get_value());
            var results = eval('(' + eventArgs.get_value() + ')');
            $get('<%=hdnLoadingPortId.ClientID%>').value = results.PortOfLoadingId;
        }
        $addHandler
            (
            $get('ctl00_ContentPlaceHolder1_Tabs_TabPanelJobDetail_FVJobDetail_txtPortOfLoading'), 'keyup',
            function () {
                $get('<%=hdnLoadingPortId.ClientID %>').value = '0';
                alert($get('<%=hdnLoadingPortId.ClientID %>').value)
            }
            );
    </script>

    <asp:UpdatePanel ID="upJobDetail" runat="server">
        <ContentTemplate>
            <div align="center">
                <asp:Label ID="lblError" runat="server" EnableViewState="false"></asp:Label>
                <asp:HiddenField ID="hdnPreAlertId" runat="server" />
                <asp:HiddenField ID="hdnCustId" runat="server" />
                <asp:HiddenField ID="hdnMode" runat="server" />
                <asp:HiddenField ID="hdnBoeTypeId" runat="server" />
                <asp:HiddenField ID="hdnDeliveryType" runat="server" />
                <asp:HiddenField ID="hdnLoadingPortId" runat="server" />
                <asp:HiddenField ID="hdnUploadPath" runat="server" />
            </div>
            <div class="clear"></div>

            <AjaxToolkit:TabContainer runat="server" ID="Tabs" ActiveTabIndex="0" CssClass="Tab"
                Width="100%" OnClientActiveTabChanged="ActiveTabChanged12" AutoPostBack="false">
                <%--Start Billing--%>
                <AjaxToolkit:TabPanel runat="server" ID="TabBiiling" HeaderText="Biiling Details">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Job Detail</legend>
                        <asp:FormView ID="FVJobDetail" HeaderStyle-Font-Bold="true" runat="server" DataKeyNames="lid"
                            Width="100%" OnDataBound="FVJobDetail_DataBound">
                            <ItemTemplate>

                                <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                                    <tr>
                                        <td><b>SEZ Type</b>
                                        </td>
                                        <td>
                                            <b><%# Eval("SEZType") %></b>

                                        </td>
                                        <td><b>Request Type</b></td>
                                        <td> 
                                                <b><%# Eval("ReqTypeName") %></b>
                                        </td>

                                        <td colspan="2">
                                            <span></span>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>BS Job Number.
                                        </td>
                                        <td>
                                            <%--  <%# Eval("JobRefNo") %>--%>
                                            <asp:Label ID="lblSEZJobNo" Text='<%# Bind("JobRefNo") %>' runat="server"></asp:Label>
                                            <asp:HiddenField ID="hdnDirName" runat="server" Value='<%#Eval("FileDirName") %>' />

                                        </td>
                                        <td>Customer Name
                                        </td>
                                        <td colspan="3">
                                            <span>
                                                <%# Eval("CustName") %>
                                            </span>
                                        </td>
                                    </tr
                                    <tr>
                                            <td>Division
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("Division") %>
                                            </span>
                                        </td>
                                            <td>Plant
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("Plant") %>
                                            </span>
                                        </td>
                                        <td>Mode
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("Mode")%>
                                            </span>
                                        </td>
                                    </tr>
                                   
                                    <tr>
                                        <td>Request ID
                                        </td>
                                        <td>
                                            <%# Eval("RequestId")%>
                                        </td>

                                            <td>BE No
                                        </td>
                                        <td>
                                            <%# Eval("BENo")%>
                                        </td>
                                        <td>BE Date
                                        </td>
                                        <td>
                                            <%--<%# Eval("BEDate","{0:dd/MM/yyyy}") %> --%><%--, "{0:dd/MM/yyyy  hh:mm tt}")%>--%>
                                            <%# (Eval("BEDate", "{0:dd/MM/yyyy}") == "01/01/1900" ? "" : Eval("BEDate", "{0:dd/MM/yyyy}"))%>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            Assesable Value
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("AssesableValue")%>
                                            </span>
                                        </td>
                                        <td>Ex Rate
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("ExRate")%>
                                            </span>
                                        </td>

                                        <td>Currency
                                        </td>
                                        <td>
                                            <span>
                                                <%# Eval("Currency")%>
                                            </span>
                                        </td>
                                    </tr>

                                    <tr id="trinwardfalse" runat="server">
                                        <td>
                                            <asp:Label ID="lbl1InwardBE" runat="server" Text="Inward BE No"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl2InwardBE" runat="server" Text='<%# Eval("InwardBENo")%>'></asp:Label>

                                        </td>
                                        <td>
                                            <asp:Label ID="lbl1InwardDate" runat="server" Text="Inward BE Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl2InwardDate" runat="server" Text='<%# Eval("InwardBEDate", "{0:dd/MM/yyyy}")%>'></asp:Label>

                                        </td>
                                        <td>
                                            <asp:Label ID="lbl1InwardJobNo" runat="server" Text="Inward Job No"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl2InwardJobNo" runat="server" Text='<%# Eval("InwardJobNo")%>'></asp:Label>

                                        </td>
                                    </tr>                                   
                                    <tr id="trDTASale" runat="Server">
                                                   
                                        <td>
                                            <asp:Label ID="lblCIFVal1" runat="server" Text="CIF Value"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCIFVal" runat="server" Text='<%# Eval("CIFValue")%>'></asp:Label>
                                        </td>
                                                  
                                        <td>
                                            <asp:Label ID="lblGrossUnit1" runat="server" Text="Goods Measurement Unit"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblGrossUnit" runat="server" Text='<%# Eval("GrossWtUnit")%>'></asp:Label>
                                        </td>
                                            <td></td>
                                        <td></td>
                                    </tr>
                                    <tr id="trDTAP" runat="server">
                                        <td>
                                            <asp:Label ID="lblDutyAmnt1" runat="server" Text="Duty Amount"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDutyAmnt" runat="server" Text='<%# Eval("DutyAmount")%>'></asp:Label>                                                        
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr id="trBOE" runat="server">
                                        <td>
                                            <asp:Label ID="lblSupplierName1" runat="server" Text="Supplier Name"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="lblSupplierName" runat="server" Text='<%# Eval("SupplierName")%>'></asp:Label>                                                        
                                        </td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                     </tr> 
                                      <tr id="trDtASale1" runat="Server">
                                        <td>
                                            <asp:Label ID="lblDiscount1" runat="server" Text="Discount Applicable"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDiscount" runat="server" Text='<%# Eval("Discount1")%>'></asp:Label>                                                      
                                        </td>
                                        <td>
                                            <asp:Label ID="lblReImport1" runat="server" Text="Re-Import"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblReImport" runat="server" Text='<%# Eval("ReImport1")%>'></asp:Label>                                                       
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPrevImport1" runat="server" Text="Previous Import"></asp:Label>                                                        
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPrevImport" runat="server" Text='<%# Eval("PrevImport1")%>'></asp:Label>                                                       
                                        </td>
                                    </tr>
                                        <tr id="trSBill2" runat="server">
                                        <td>
                                            <asp:Label ID="lblBuyerName1" runat="server" Text="Buyer Name"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="lblBuyerName" runat="server" Text='<%# Eval("BuyerName")%>'></asp:Label>                                                        
                                        </td>
                                            <td>
                                            <asp:Label ID="lblSchemeCode1" runat="server" Text="Scheme Code"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="lblSchemeCode" runat="server" Text='<%# Eval("SchemeCode")%>'></asp:Label>                                                        
                                        </td>
                                    </tr>
                                    <tr id="trSBill" runat="Server">
                                        <td>
                                            <asp:Label ID="lblPrevExpGoods1" runat="server" Text="Previous Export Goods"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPrevExpGoods" runat="server" Text='<%# Eval("PrevExpGoods1")%>'></asp:Label>                                                      
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCessDetail1" runat="server" Text="Cess Details"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCessDetail" runat="server" Text='<%# Eval("CessDetail1")%>'></asp:Label>                                                       
                                        </td>
                                        <td>
                                            <asp:Label ID="lblLicenceRegNo1" runat="server" Text="Licence Registration No"></asp:Label>                                                        
                                        </td>
                                        <td>
                                            <asp:Label ID="lblLicenceRegNo" runat="server" Text='<%# Eval("LicenceRegNo1")%>'></asp:Label>                                                       
                                        </td>
                                    </tr>
                                    <tr id="trSBill1" runat="Server">
                                        <td>
                                            <asp:Label ID="lblReExport1" runat="server" Text="Re-Export"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblReExport" runat="server" Text='<%# Eval("ReExport1")%>'></asp:Label>                                                      
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPrevExport1" runat="server" Text="Previous Export"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPrevExport" runat="server" Text='<%# Eval("PrevExport1")%>'></asp:Label>                                                       
                                        </td>
                                        <td>
                                                       
                                        </td>
                                        <td>
                                                                                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Dispatch Date
                                        </td>
                                        <td>
                                            <%# (Eval("OutwardDate", "{0:dd/MM/yyyy}") == "01/01/1900" ? "" : Eval("OutwardDate", "{0:dd/MM/yyyy}"))%>
                                            
                                        </td>
                                        <td>PCD From Dahej
                                        </td>
                                        <td>
                                            <%# (Eval("PCDFrDahej", "{0:dd/MM/yyyy}") == "01/01/1900" ? "" : Eval("PCDFrDahej", "{0:dd/MM/yyyy}"))%>
                                            
                                    
                                        </td>
                                        <td>PCD Sent Client
                                        </td>
                                        <td>
                                            <%# (Eval("PCDSentClient", "{0:dd/MM/yyyy}") == "01/01/1900" ? "" : Eval("PCDSentClient", "{0:dd/MM/yyyy}"))%>
                                            <%--<%# Eval("PCDSentClient", "{0:dd/MM/yyyy}")%>--%>
                                    
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>File Sent To Billing
                                        </td>
                                        <td>
                                            <%# (Eval("FileSentToBilling", "{0:dd/MM/yyyy}") == "01/01/1900" ? "" : Eval("FileSentToBilling", "{0:dd/MM/yyyy}"))%>
                                            <%--<%# Eval("FileSentToBilling", "{0:dd/MM/yyyy}")%>--%>
                                    
                                        </td>
                                        <td>Billing Status
                                        </td>
                                        <td>
                                            <%# Eval("BillingStatus")%>
                                        </td>
                                        <td></td>
                                        <td></td>                                      
                                    </tr>
                                    <tr>
                                        <td>Remark
                                        </td>
                                        <td colspan="3">
                                            <%# Eval("Remark")%>
                                        </td>
                                        <td></td>
                                        <td></td>
                                    </tr>

                                </table>
                              
                            </ItemTemplate>
                            
                        </asp:FormView>
                        </fieldset>    

                            <fieldset>
                                <legend>Billing Scrutiny</legend>
                                <asp:GridView ID="gvbillingscrutiny" runat="server" AutoGenerateColumns="False"
                                    CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceBillingScrutiny"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Status" DataField="Status" />
                                        <asp:BoundField HeaderText="Sent By Billing Advice" DataField="SentBy" />
                                        <asp:BoundField HeaderText="Billing Advice Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Received By Billing Scrutiny" DataField="ReceivedBy" />
                                        <asp:BoundField HeaderText="Billing Scrutiny Received Date" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Scrutiny Completed Date" DataField="ScrutinyDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Scrutiny Completed By" DataField="ScrutinyCompletedBy" />
                                    </Columns>
                                </asp:GridView>
                            </fieldset>

                            <fieldset>
                                <legend>Draft Invoice</legend>
                                <asp:Button ID="btnCheckBJVInvoice" Text="Check Draft Status" runat="server" OnClick="btnCheckBJVInvoice_Click" />
                                <asp:GridView ID="gvDraftInvoice" runat="server" AutoGenerateColumns="False"
                                    CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceDraftinvoice"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Status" DataField="Status" />
                                        <asp:BoundField HeaderText="Sent By Billing Scrutiny" DataField="SentBy" />
                                        <asp:BoundField HeaderText="Billing Scrutiny Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Received By Draft Invoice" DataField="ReceivedBy" />
                                        <asp:BoundField HeaderText="Draft Invoice Received Date" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Draft Invoice Completed Date" DataField="DraftInvoiceDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Draft Invoice Completed By" DataField="FAUserName" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    </Columns>
                                </asp:GridView>
                            </fieldset>

                            <fieldset>
                                <legend>Draft Check</legend>
                                <asp:GridView ID="gvDraftcheck" runat="server" AutoGenerateColumns="False"
                                    CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceDraftCheck"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Status" DataField="Status" />
                                        <asp:BoundField HeaderText="Sent By Draft Invoice" DataField="SentBy" />
                                        <asp:BoundField HeaderText="Draft Invoice Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Received By Draft Check" DataField="ReceivedBy" />
                                        <asp:BoundField HeaderText="Draft Check Received Date" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Draft Check Completed Date" DataField="DraftCheckDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    </Columns>
                                </asp:GridView>
                            </fieldset>

                            <fieldset>
                                <legend>Final Invoice Typing</legend>
                                <asp:GridView ID="gvFinaltyping" runat="server" AutoGenerateColumns="False"
                                    CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceFinalTyping"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Status" DataField="Status" />
                                        <asp:BoundField HeaderText="Sent By Draft Check" DataField="SentBy" />
                                        <asp:BoundField HeaderText="Draft Check Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Received By Final Typing" DataField="ReceivedBy" />
                                        <asp:BoundField HeaderText="Final Typing Received Date" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Final Typing Completed Date" DataField="FinalTypingDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Final Typing Completed by" DataField="FAUserName" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Comment" DataField="Comment" />
                                    </Columns>
                                </asp:GridView>
                            </fieldset>

                            <fieldset>
                                <legend>Final Invoice Check</legend>
                                <asp:GridView ID="gvfinalcheck" runat="server" AutoGenerateColumns="False"
                                    CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceFinalCheck"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Status" DataField="Status" />
                                        <asp:BoundField HeaderText="Sent By Final Typing" DataField="SentBy" />
                                        <asp:BoundField HeaderText="Final Typing Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Received By Final Check" DataField="ReceivedBy" />
                                        <asp:BoundField HeaderText="Final Check Received Date" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Final Check Completed Date" DataField="FinalCheckDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    </Columns>
                                </asp:GridView>
                            </fieldset>

                            <fieldset>
                                <legend>Bill Dispatch</legend>
                                <asp:GridView ID="gvbilldispatch" runat="server" AutoGenerateColumns="False" CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceBillDispatch"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Status" DataField="Status" />
                                        <asp:BoundField HeaderText="Sent By Final Check" DataField="SentBy" />
                                        <asp:BoundField HeaderText="Final Check Sent Date" DataField="SentDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Received By Bill Dispatch" DataField="ReceivedBy" />
                                        <asp:BoundField HeaderText="Bill Dispatch Received Date" DataField="ReceivedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Bill Dispatch Completed Date" DataField="BillDispatchDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                    </Columns>
                                </asp:GridView>
                            </fieldset>

                            <fieldset>
                                <legend>Bill Rejection</legend>
                                <asp:GridView ID="gvBillrejection" runat="server" AutoGenerateColumns="False" CssClass="table" Width="99%" PagerStyle-CssClass="pgr" DataKeyNames="lId" DataSourceID="DataSourceBillRejection"
                                    CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Stage" DataField="Stage" />
                                        <asp:BoundField HeaderText="Rejected by" DataField="RejectedBy" />
                                        <asp:BoundField HeaderText="Bill Rejection Date" DataField="RejectedDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Reason" DataField="Reason" />
                                        <asp:BoundField HeaderText="Remark" DataField="Remark" />
                                        <asp:BoundField HeaderText="Followup Date" DataField="FollowupDate" DataFormatString="{0:dd/MM/yyyy  hh:mm tt}" />
                                        <asp:BoundField HeaderText="Followup Remark" DataField="FollowupRemark" />
                                    </Columns>
                                </asp:GridView>
                            </fieldset>

                            <div id="div1">
                                <asp:SqlDataSource ID="DataSourceBillingScrutiny" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetBillingScrutinyById" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                                <asp:SqlDataSource ID="DataSourceDraftinvoice" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetDraftInvoiceById" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                                <asp:SqlDataSource ID="DataSourceDraftCheck" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetDraftCheckById" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                                <asp:SqlDataSource ID="DataSourceFinalTyping" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetFinalTypingById" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                                <asp:SqlDataSource ID="DataSourceFinalCheck" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetFinalCheckById" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                                <asp:SqlDataSource ID="DataSourceBillDispatch" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetBillDispatchById" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                                <asp:SqlDataSource ID="DataSourceBillRejection" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetBillRejectionById" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                                <asp:SqlDataSource ID="DataSourcePaymentHistory" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="AC_GetInvoicePaymentByJobId" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                
                                <asp:SqlDataSource ID="PCADocSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetPCDDocumentByWorkFlow" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                        <asp:Parameter Name="DocumentForType" DefaultValue="2" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                                <asp:SqlDataSource ID="BillingAdviceSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetPCDDocumentByWorkFlow" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                        <asp:Parameter Name="DocumentForType" DefaultValue="3" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                                <asp:SqlDataSource ID="PCDDocumentSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetUploadedPCDDocument" SelectCommandType="StoredProcedure" OnSelected="PCDDocumentSqlDataSource_Selected">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                <asp:SqlDataSource ID="DataSourceRate" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetTransDetail" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                <asp:SqlDataSource ID="DataSourceSellingRate" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetSellTransDetail" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                <asp:SqlDataSource ID="DataJobExpenseDetails" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                    SelectCommand="GetAdditionalExpenseByJobId" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>
                        </div>

                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
                <AjaxToolkit:TabPanel ID="TabDocument" runat="server" HeaderText="Document">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Job Document</legend>
                            <asp:GridView ID="grdDocument" runat="server" AutoGenerateColumns="False" CssClass="table"
                            Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr"
                            OnRowCommand="GrdDocument_RowCommand" DataSourceID="DocumentSqlDataSource" DataKeyNames="JobId"
                            CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="20">
                            <%--DataSourceID="DocumentSqlDataSource" DataKeyNames="DocId"--%>
                            <Columns>
                                <%-- <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>--%>

                                <asp:BoundField DataField="JobId" HeaderText="Doc Id" />

                                <asp:BoundField DataField="DocType" HeaderText="Doc TypeId" Visible="false" />
                                <asp:BoundField DataField="DocTypeName" HeaderText="Doc Type" />

                                <asp:BoundField DataField="DocPath" HeaderText="Doc Path" />
                                <asp:BoundField DataField="DocumentName" HeaderText="Document Name" />
                                <asp:BoundField DataField="sName" HeaderText="User Name" />
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFileDirName" runat="server" Text='<%#Eval("FileDirName") %>'></asp:Label>

                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Download">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="Download"
                                            CommandArgument='<%#Eval("DocPath") %>'></asp:LinkButton>
                                        <%--<asp:HiddenField ID="hdnDirName" runat="server" Value='<%#Eval("FileDirName") %>' />--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                            <br />
                        </fieldset>
                       
                        <div>
                            <asp:SqlDataSource ID="DocumentSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="SEZ_GetUploadedDocument" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="JobId" SessionField="JobId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                            <asp:SqlDataSource ID="DataSourcePaymentDoument" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                                SelectCommand="AC_GetInvoiceDocumentBYJobId" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter Name="JobId" SessionField="JobId" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </ContentTemplate>
                </AjaxToolkit:TabPanel>
                <AjaxToolkit:TabPanel ID="TabContainer" runat="server" HeaderText="Container">
                <ContentTemplate>
                    <div>
                        <fieldset>
                            <legend>Container Detail</legend>
                            <asp:GridView ID="gvContainer" runat="server" AllowPaging="true" CssClass="table"
                                PagerStyle-CssClass="pgr" AutoGenerateColumns="false" DataKeyNames="lid" Width="100%"
                                PageSize="20" DataSourceID="DataSourceContainer">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex +1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ContainerNo" HeaderText="Container No" />
                                    <asp:BoundField DataField="ContainerTypeName" HeaderText="Container Type" />
                                    <asp:BoundField DataField="ContainerSizeName" HeaderText="Container Size" />
                                </Columns>
                            </asp:GridView>
                        </fieldset>
                    </div>
                    <div>
                       <asp:SqlDataSource ID="DataSourceContainer" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                            SelectCommand="SEZ_GetContainerDetail" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:SessionParameter Name="JobId" SessionField="JobId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                </ContentTemplate>
            </AjaxToolkit:TabPanel>
                <%--end Billing--%>
            </AjaxToolkit:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

