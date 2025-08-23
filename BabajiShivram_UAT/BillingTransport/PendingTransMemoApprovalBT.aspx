<%@ Page Title="BSCCPL Payment Memo Approval" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PendingTransMemoApprovalBT.aspx.cs" Inherits="BillingTransport_PendingTransMemoApprovalBT" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upFillDetails" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; visibility: visible; border: none; z-index: 100; width: 90%; height: 90%; background: #FAFAFA; filter: alpha(opacity=80); -moz-opacity: .8; opacity: .8;">
                    <img alt="progress" src="../images/progress-indicator.gif" style="position: relative; top: 40%; left: 40%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        </div>
        <asp:UpdatePanel ID="upFillDetails" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
             <script type="text/javascript">
                function GridSelectAllColumn(spanChk) {
                    var oItem = spanChk.children;
                    var theBox = (spanChk.type == "checkbox") ? spanChk : spanChk.children.item[0]; xState = theBox.checked;
                    elm = theBox.form.elements;
                    
                    for (i = 0; i < elm.length; i++) {
                        if (elm[i].type === 'checkbox' && elm[i].checked != xState) {
                            elm[i].click();
                        }
                    }

                    // Calculate Total Amount

                    GetTotalAmt();
                }

                function GetTotalAmt() {
                //Reference the GridView.
                var decTotalToPay = 0.0;
                var grid = document.getElementById("<%=gvDetail.ClientID%>");
                    var objMessage = document.getElementById("<%=lblMessage.ClientID%>");
                //Reference the CheckBoxes in GridView.
                var checkBoxes = grid.getElementsByTagName("INPUT");
                var message = "";

                //Loop through the CheckBoxes.
                    for (var i = 1; i < checkBoxes.length; i++) {
                        
                        if (checkBoxes[i].checked) {
                            
                            var row = checkBoxes[i].parentNode.parentNode.parentNode;

                            decTotalToPay = decTotalToPay + parseFloat(row.cells[4].innerText);
                            //message += row.cells[10].innerHTML;
                            //message += "\n";

                            //alert(decTotalToPay);
                            
                    }
                }

                //  Display selected Row data in Alert Box.
                                        
                    objMessage.innerText = "Total Payable: Rs. " + decTotalToPay.toString();

            }
             </script>   
            <div align="center">
                <asp:Label ID="lblMessage" runat="server" CssClass="errorMsg" Font-Size="Large"></asp:Label>
            </div>
            <fieldset><legend>Transport Memo Approval</legend>
            <div class="clear">
            <asp:Panel ID="pnlFilter" runat="server">
                <div class="fleft">
                    <uc1:DataFilter ID="DataFilter1" runat="server" />
                </div>
                <div class="fleft" style="margin-left:30px; padding-top:3px;">
                    <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                        <asp:Image ID="Image1" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                    </asp:LinkButton>
                </div>
                <div class="fleft" style="margin-left:30px; padding-top:3px;">
                <asp:Button ID="btnApproveMemo" runat="server" Text="Approve Transport Memo For Payment" OnClick="btnApproveMemo_Click" OnClientClick="return confirm('Sure to Approve Memo Transport Payment??');" />
                </div>
            </asp:Panel>
            </div>
            <div class="m clear"></div>
            <asp:GridView ID="gvDetail" runat="server" AutoGenerateColumns="False" CssClass="table" DataKeyNames="lId"
                AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataSourceID="InvoiceSqlDataSource" 
                AllowPaging="True" AllowSorting="True" PageSize="40" PagerSettings-Position="TopAndBottom"
                Width="100%" OnRowCommand="gvDetail_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex +1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkboxSelectAll" align="center" ToolTip="Check All" runat="server" onclick="GridSelectAllColumn(this);"/>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkjoball" runat="server" ToolTip="Check" onclick="GetTotalAmt()"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Memo Ref No">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkMemoRefNo" CommandName="select" runat="server" Text='<%#Eval("PayMemoRefNo") %>'
                            CommandArgument='<%#Eval("lid") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="PayMemoRefNo" HeaderText="Memo Ref No" SortExpression="PayMemoRefNo" Visible="false" />
                <asp:BoundField DataField="VendorName" HeaderText="Transporter" SortExpression="VendorName" />
                <asp:BoundField DataField="MemoAmount" HeaderText="Payable Amount" SortExpression="MemoAmount" />
                <asp:BoundField DataField="BranchName" HeaderText="Branch" SortExpression="BranchName" />
                <asp:BoundField DataField="dtDate" HeaderText="Memo Date" SortExpression="dtDate" DataFormatString="{0:dd/MM/yyyy}"/>
                <asp:BoundField DataField="UserName" HeaderText="User" SortExpression="UserName" />
            </Columns>
            <PagerTemplate>
                <asp:GridViewPager runat="server" />
            </PagerTemplate>
        </asp:GridView>
        </fieldset>
        <div>
            <asp:SqlDataSource ID="InvoiceSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="TRS_GetPendingMemoForApprovalBT" SelectCommandType="StoredProcedure">
            </asp:SqlDataSource>
        </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

