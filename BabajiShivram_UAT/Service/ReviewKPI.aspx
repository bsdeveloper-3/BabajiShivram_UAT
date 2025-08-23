<%@ Page Title="Review KPI" Language="C#" AutoEventWireup="true" CodeFile="ReviewKPI.aspx.cs" Inherits="Service_ReviewKPI" 
    MasterPageFile="~/MasterPage.master" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <asp:ScriptManager runat="server" ID="ScriptManager1" />
    
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upPendingchecklist" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    
    <asp:UpdatePanel ID="upPendingchecklist" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
        <div align="center">
            <asp:Label ID="lblError" runat="server"></asp:Label>
        </div>
        <div class="m clear">
        <fieldset><legend>KPI - Particulars For Review</legend>
        
        <div class="clear">
        </div>
            <asp:GridView ID="GridViewKPI" runat="server" AutoGenerateColumns="False" CssClass="table"
                Width="100%" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" DataKeyNames="lId"
                DataSourceID="DataSourceKPIReview" CellPadding="4" AllowPaging="True" AllowSorting="True"
                PageSize="20" OnRowCommand="GridViewKPI_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Employee">
                        <ItemTemplate>
                            <asp:Label ID="lblEmpName" runat="server" Text='<%#Eval("EmpName") %>' ></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:BoundField HeaderText="Submit Date" DataField="SubmitDate" DataFormatString="{0:dd/MM/yyyy}"/>
                    <asp:BoundField HeaderText="Review Date" DataField="ReviewDate" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkView" runat="server" Text="Review" CommandArgument='<%#Bind("lid") %>' CommandName="viewdetail"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </fieldset>
        <div>
            <asp:SqlDataSource ID="DataSourceKPIReview" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="KPI_GetEmpTargetForReview" SelectCommandType="StoredProcedure">
                <SelectParameters>
                        <asp:SessionParameter Name="HODID" SessionField="UserId" />
                    </SelectParameters>
            </asp:SqlDataSource>
        </div>
        <div id="popup">
            <!--Popup Month-Status Detail  -->

            <div id="divPopupMonthDetail">
                <cc1:ModalPopupExtender ID="ModalPopupMonthStatus" runat="server" CacheDynamicResults="false"
                    DropShadow="False" PopupControlID="Panel2Month" TargetControlID="lnkDummyMonth">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="Panel2Month" runat="server" CssClass="ModalPopupPanel" Style="display: none">
                <div class="header">
                <div class="fleft">
                    <asp:Button ID="btnApprove" runat="server" Text="Approve" OnClick="btnApprove_Click" ToolTip="Review and Approve KPI - Particulars" />
                    &nbsp;<asp:Label ID="lblViewName" Text="" runat="server" />
                    <asp:HiddenField ID="hdnKPIID" runat="server" />
                </div>
                <div class="fright">
                    <asp:Button ID="btnCancelPopup" runat="server" OnClick="btnCancelPopup_Click" Text="Close" 
                        CausesValidation="false" class="no" ToolTip="Close Window" />
                </div>
            </div>
                <!--Freight Detail Start-->
            <div id="Div1" runat="server" style="max-height: 600px; max-width:900px; overflow: auto;">
                <asp:GridView ID="gvParticulars" runat="server" CssClass="table" Width="99%" AutoGenerateColumns="false"
                     DataKeyNames="lid" AllowPaging="true" PageSize="20" PagerStyle-CssClass="pgr">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl">
                            <ItemTemplate>
                                <%#Container.DataItemIndex +1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Particulars">
                            <ItemTemplate>
                                <asp:TextBox ID="txtParticular" Width="700px" runat="server" Text='<%#Bind("KPIParticular") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:BoundField HeaderText="Particulars" DataField="KPIParticular" />--%>
                </Columns>
                </asp:GridView>    
                <div>Emp Remark
                <asp:TextBox ID="txtEmpRemark" runat="server" BorderStyle="Inset" TextMode="MultiLine" MaxLength="200"></asp:TextBox>
                
                <asp:TextBox ID="txtHODRemark" runat="server" BorderStyle="Inset" TextMode="MultiLine" MaxLength="200"></asp:TextBox>
                    HOD Remark
                </div>
            </div>
            </asp:Panel>
            <div>
                <asp:SqlDataSource ID="DataSourceParticular" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="KPI_GetEmpParticular" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter Name="KPIID" />
                </SelectParameters>
                </asp:SqlDataSource>
            </div>
            <div>
                <asp:LinkButton ID="lnkDummyMonth" runat="server" Text=""></asp:LinkButton>
            </div>
            </div>  
            <!--Popup Freight Summary Detail  -->
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>