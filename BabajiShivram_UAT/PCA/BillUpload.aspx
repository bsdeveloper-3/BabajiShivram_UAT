<%@ Page Title="Bill Upload" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
        CodeFile="BillUpload.aspx.cs" Inherits="PCA_BillUpload" EnableEventValidation="false" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    
    <asp:HiddenField ID="hdnJobId" runat="server" Value="0" />
    <asp:HiddenField ID="hdnBillId" runat="server" Value="0" />
    <asp:HiddenField ID="hdnBillList" runat="server" Value="" />

    <div align="center">
        <asp:Label ID="lblMessage" runat="server"></asp:Label>
    </div>
    <div class="clear"></div>
        <fieldset><legend>Bill Upload</legend>
            <div class="fleft">
                <uc1:DataFilter ID="DataFilter1" runat="server" />
            </div>
            <div class="fleft" style="margin-left: 30px; padding-top: 3px;">
                <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                    <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                </asp:LinkButton>
            </div>
            <div class="m clear">
            <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False"
                CssClass="table" Width="99%" PagerStyle-CssClass="pgr" PagerSettings-Position="TopAndBottom" 
                DataKeyNames="JobId,Billid" DataSourceID="DataSourceBillJob" CellPadding="4"
                OnRowCommand="gvJobDetail_RowCommand"  OnRowDataBound="gvJobDetail_RowDataBound"
                AllowPaging="True" AllowSorting="True" PageSize="40">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Job No" SortExpression="JobRefNo">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkJobNo" runat="server" Text='<%#Eval("BJVNo") %>' CommandArgument='<%#Eval("JobId")%>' CommandName="JobSelect"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Job No" DataField="BJVNo" Visible="false" />
                    <asp:BoundField HeaderText="Customer" DataField="Customer" />
                    <asp:BoundField HeaderText="Bill Number" DataField="INVNO" />
                    <asp:BoundField HeaderText="Bill Date" DataField="INVDATE" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField HeaderText="Bill Amount" DataField="INVAMOUNT" />
                    <asp:BoundField HeaderText="E-Bill" DataField="EBillStatusName" />
                    <asp:BoundField HeaderText="Portal Update" DataField="PortalStatusName" />
                    <asp:BoundField HeaderText="Physical Dispatch" DataField="DispatchStatusName" />
                    <asp:TemplateField HeaderText="Upload">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkBillUpload" runat="server" Text="Bill Upload" CommandName="Upload" CommandArgument='<%#Eval("BillId")%>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="View">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkBillView" runat="server" Text="View" CommandName="View" CommandArgument='<%#Eval("BillId")%>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Download">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkBillDownload" runat="server" Text="Download" CommandName="Download" CommandArgument='<%#Eval("BillId")%>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="E-Bill Email" Visible="false">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEBillEmail" runat="server" Text="Send E-Mail" CommandName="EBillEmail" CommandArgument='<%#Eval("BillId")%>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Physical Dispatch" Visible="false">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkPhysicalDispatch" runat="server" Text="Hard Copy Dispatch" CommandName="PhysicalDispatch" CommandArgument='<%#Eval("BillId")%>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerTemplate>
                    <asp:GridViewPager runat="server" />
                </PagerTemplate>
            </asp:GridView>
        </div>
        </fieldset>
        <div id="divDatasource">
            <asp:SqlDataSource ID="DataSourceBillJob" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="BL_GetPendingBillUpload" SelectCommandType="StoredProcedure"
                DataSourceMode="DataSet" EnableCaching="true" CacheDuration="120" CacheKeyDependency="MyCacheDependency">
                <SelectParameters>
                    <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
   
    <%--  START : MODAL POP-UP FOR Upload-  --%>

    <div>
        <asp:LinkButton ID="lnkDomBillUpload10" runat="server"></asp:LinkButton>
    </div>

    <div id="divOpBillUpload">
        <cc1:ModalPopupExtender ID="BillUploadModalPopup" runat="server" CacheDynamicResults="false" PopupControlID="panBillUpload"
            CancelControlID="imgbtnBillUpload" TargetControlID="lnkDomBillUpload10" BackgroundCssClass="modalBackground" DropShadow="true">
        </cc1:ModalPopupExtender>
        <asp:Panel ID="panBillUpload" runat="server" CssClass="ModalPopupPanel" Width="600px">
            <div class="header">
                <div class="fleft">
                    <asp:Label ID="lblPopupName" runat="server" Text="Upload Bill"></asp:Label>
                </div>
                <div class="fright">
                    <asp:ImageButton ID="imgbtnBillUpload" ImageUrl="../Images/delete.gif" runat="server"
                        ToolTip="Close" />
                </div>
            </div>
            <div id="Div2" runat="server" style="max-height: 250px; overflow: auto; padding: 5px">
                <div>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>Job No</td>
                            <td colspan="3">
                                <asp:Label ID="lblBJVNumber" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Bill No</td>
                            <td>
                                <asp:Label ID="lblBJVBillNo" runat="server"></asp:Label>
                            </td>
                            <td>Bill Date</td>
                            <td>
                                <asp:Label ID="lblBJVBillDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Amount</td>
                            <td>
                                <asp:Label ID="lblBJVAmount" runat="server"></asp:Label>
                            </td>
                            <td>
                                Reduce Size <asp:CheckBox ID="chkPDFSizeReduce" runat="server" Enabled="false" />
                            </td>
                            <td>
                                Merge BoE Copy <asp:CheckBox ID="chkPDFMergerBoECopy" Enabled="false" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:RequiredFieldValidator ID="RFVUpload" runat="server" ControlToValidate="fuReceipt" SetFocusOnError="true" Display="Dynamic"
                    ForeColor="Red" ErrorMessage="Bill PDF Required" ValidationGroup="ValidateExpense" Font-Bold="true"></asp:RequiredFieldValidator>
                <div>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="white">
                        <tr>
                            <td>PDF Doc                                      
                            </td>
                            <td>
                                <asp:FileUpload ID="fuReceipt" runat="server"></asp:FileUpload>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnUploadBill" runat="server" Text="Upload Bill" OnClick="btnUploadBill_Click" ValidationGroup="ValidateExpense" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </asp:Panel>
    </div>
    
</asp:Content>

