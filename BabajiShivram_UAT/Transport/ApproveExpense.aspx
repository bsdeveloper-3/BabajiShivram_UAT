<%@ Page Title="Vehicle Expense Approval" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="ApproveExpense.aspx.cs" Inherits="Transport_ApproveExpense" Culture="en-GB" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="content2" runat="server">
    <AjaxToolkit:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <div>
        <link rel="stylesheet" href="../CSS/lightbox.css" type="text/css" media="screen" />
        <script type="text/javascript" src="../JS/prototype.js"></script>
        <script type="text/javascript" src="../JS/scriptaculous.js?load=effects"></script>
        <script type="text/javascript" src="../JS/lightbox.js"></script>
    </div>
    <div>
        <script src="../JS/jquery-1.4.4.min.js" type="text/javascript"></script>
        <script type="text/javascript">
        $(document).keyup(function (e) {

            if (e.which == '27') {
                // Find the Generated Cancel Button for the Row in Edit Mode.
                var cancelButton = $('#<%= gvMaintenance.ClientID %>').find('a').filter(function () {
                    return $(this).text() === "Cancel"
                });

                // If Cancel Button is found, then show message and click the Cancel Button.
                if (cancelButton != null && cancelButton.length > 0) {
                    $("#lblCancel")
                .html("You pressed Escape. Cancelling the Approval...")
                .fadeOut(1100, function () {
                    buttonClick(cancelButton[0]);
                });
                }
            }

            if (e.which == '13') 
            {
                // Find the Generated Cancel Button for the Row in Edit Mode.
                var updateButton = $('#<%= gvMaintenance.ClientID %>').find('a').filter(function () {
                    return $(this).text() === "Update"
                });

                // If Cancel Button is found, then show message and click the Cancel Button.
                if (updateButton != null && updateButton.length > 0) {
                    buttonClick(updateButton[0]);
                }
            }
        });

        // This function fires the Button Click Event.
        function buttonClick(button) {
            button.click();
        }
    </script>
    </div>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upExpense" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <fieldset><legend>Maintenance Expense Approval </legend>
        <asp:UpdatePanel ID="upExpense" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
                <div class="clear" style="text-align:center;">
                    <asp:Label ID="lblError" runat="server"></asp:Label>
                    <label id="lblCancel"></label>
                </div>
                <div class="fleft">
                    <uc1:DataFilter ID="DataFilter1" runat="server" />
                </div>
                <asp:GridView ID="gvMaintenance" runat="server" AutoGenerateColumns="false" CssClass="table" DataKeyNames="MaintanceId,ExpenseId" 
                    AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" PageSize="20" AllowPaging="True" AllowSorting="True" AccessKey="1" 
                    PagerSettings-Position="TopAndBottom" AutoGenerateEditButton="true" DataSourceID="SqlDataSourceExp" OnPreRender="gvMaintenance_PreRender"
                    OnRowDataBound="gvMaintenance_RowDataBound" OnRowEditing="gvMaintenance_RowEditing" OnRowUpdating="gvMaintenance_RowUpdating"
                    OnRowCommand="gvMaintenance_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="RefNo" HeaderText="Ref No" SortExpression="RefNo" ReadOnly="true" />
                        <asp:BoundField DataField="WorkDate" HeaderText="Work Date" SortExpression="WorkDate" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="VehicleNo" HeaderText="Vehicle" SortExpression="VehicleNo" ReadOnly="true" />
                        <asp:BoundField DataField="CategoryName" HeaderText="Category" ReadOnly="true" />
                        <asp:TemplateField HeaderText="Amount">
                            <ItemTemplate>
                                <asp:Label ID="lblAmount" Text='<%#Eval("Amount") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Approved Amt">
                            <ItemTemplate>
                                <asp:Label ID="lblApprovedAmount" Text='<%#Eval("ApprovedAmount") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtApprovedAmount" Text='<%#BIND("ApprovedAmount") %>' runat="server" Width="100px"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <asp:Label ID="lblExpenseDesc" Text='<%#Eval("ShortExpenseDesc") %>' runat="server" ></asp:Label>
                                <asp:LinkButton ID="lnkMore" runat="server" Text="...More" ToolTip='<%#Eval("ExpenseDesc") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="BillNumber" HeaderText="Bill No" ReadOnly="true"/>
                        <asp:BoundField DataField="PaidTo" HeaderText="Paid To" ReadOnly="true" />
                        <asp:BoundField DataField="SupportBillPaidTo" HeaderText="Support Bill To" ReadOnly="true"/>
                        <asp:BoundField DataField="PayType" HeaderText="Pay Type" ReadOnly="true" />
                        <asp:TemplateField HeaderText="Doc" Visible="false">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDoc" runat="server" Text="View" CommandName="ViewDocument" CommandArgument='<%#Eval("MaintanceId") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    </Columns>
                    <PagerTemplate>
                        <asp:GridViewPager ID="GridViewPager1" runat="server" />
                    </PagerTemplate>
                </asp:GridView>
                <div>
                    <asp:SqlDataSource ID="SqlDataSourceExp" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                        SelectCommand="TR_GetApprovalExpense" SelectCommandType="StoredProcedure" UpdateCommand="TR_updApprovalExpense"
                        UpdateCommandType="StoredProcedure" OnUpdated="SqlDataSourceExp_Updated">
                        <UpdateParameters>
                            <asp:Parameter Name="MaintanceId" Type="Int32" />
                            <asp:Parameter Name="ExpenseId"  Type="Int32" />
                            <asp:Parameter Name="ApprovedAmount" Type="String" />
                            <asp:SessionParameter Name="UserId" SessionField="UserId" />
                            <asp:Parameter Name="OutPut" Type="int32" Direction="Output" />
                        </UpdateParameters>
                    </asp:SqlDataSource>
                </div>

            <!--Popup Month-Status Detail  -->

            <div id="divPopupMonthDetail">
                <AjaxToolkit:ModalPopupExtender ID="ModalPopupDoc" runat="server" CacheDynamicResults="false"
                    DropShadow="False" PopupControlID="Panel2Month" TargetControlID="lnkDummyMonth">
                </AjaxToolkit:ModalPopupExtender>
                <asp:Panel ID="Panel2Month" runat="server" CssClass="ModalPopupPanel">
                <div class="header">
                <div class="fleft">
                    &nbsp;<asp:Button ID="btnCancelPopup" runat="server" OnClick="btnCancelPopup_Click" Text="Close" CausesValidation="false" />
                </div>
                <div class="fright">
                    <asp:ImageButton ID="imgClose" ImageUrl="~/Images/delete.gif" OnClick="btnCancelPopup_Click" runat="server" ToolTip="Close"  />
                </div>
            </div>
                <!--Document Detail Start-->
            <div id="Div1" runat="server" style="max-height: 700px; max-width:900px; overflow: auto;">
                <asp:DataList ID="dlImages" runat="server" RepeatColumns="3" CellPadding="5">
                <ItemTemplate>
                    <a id="imageLink" href='<%# Eval("DocPath","~/UploadFiles/{0}") %>' title='<%#Eval("DocName") %>' target="_blank" rel="lightbox[Brussels]" runat="server" >
                        <asp:Image ID="Image1" ImageUrl='<%# Bind("DocPath", "~/UploadFiles/{0}") %>' runat="server" Width="120" Height="100" />
                    </a> 
                </ItemTemplate>
                <ItemStyle BorderColor="Brown" BorderStyle="dotted" BorderWidth="3px" HorizontalAlign="Center"
                    VerticalAlign="Bottom" />
                </asp:DataList>    
            </div>
            </asp:Panel>
            <div>
                <asp:LinkButton ID="lnkDummyMonth" runat="server" Text=""></asp:LinkButton>
            </div>
            </div>  
            <!--Popup Document Detail  -->
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
</asp:Content>

