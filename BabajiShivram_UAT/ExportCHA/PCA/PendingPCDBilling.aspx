<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PendingPCDBilling.aspx.cs" Inherits="PendingPCDBilling"
 MasterPageFile="~/MasterPage.master" Title="Pending Billing" EnableEventValidation="false" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="content1" runat="server">
<cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" />
<script language="javascript" type="text/javascript" src="../JS/CheckBoxListPCDDocument.js"></script>
    <div>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upShipment" runat="server">
            <ProgressTemplate>
                <img alt="progress" src="../images/processing.gif" />
                Processing...
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upShipment" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
    <div align="center">
        <asp:Label ID="lblMessage" runat="server"></asp:Label>
    </div>
    <div class="clear"></div>
    <fieldset>
        <legend>Pending Billing</legend>  
    <div class="m clear">
        <asp:Panel ID="pnlFilter" runat="server">
            <div class="fleft">
                <uc1:DataFilter ID="DataFilter1" runat="server" />
            </div>
            <div class="fleft" style="margin-left:40px;">
                <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkexport_Click">
                    <asp:Image ID="Image2" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                </asp:LinkButton>
            </div>
        </asp:Panel>
    </div>
    <div class="clear">
    </div>
    <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False" CssClass="table"
        PagerStyle-CssClass="pgr" DataKeyNames="JobId" AllowPaging="True" AllowSorting="True" Width="100%"
        PageSize="20" PagerSettings-Position="TopAndBottom" OnPreRender="gvJobDetail_PreRender" DataSourceID="PCDSqlDataSource"
        OnRowCommand="gvJobDetail_RowCommand">
        <Columns>
            <asp:TemplateField HeaderText="Sl">
                <ItemTemplate>
                    <%#Container.DataItemIndex +1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="BS Job No" SortExpression="JobRefNo">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkJobNo" runat="server" Text='<%#Eval("JobRefNo") %>' CommandName="select" CommandArgument='<%#Eval("JobId") %>'></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="JobRefNo" HeaderText="BS Job No" Visible="false"/>
            <asp:BoundField DataField="BranchName" HeaderText="Babaji Branch" SortExpression="BranchName"/>
            <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
            <asp:BoundField DataField="ConsigneeName" HeaderText="Consignee" SortExpression="ConsigneeName" />
            <asp:BoundField DataField="Mode" HeaderText="Mode" SortExpression="Mode" />
            <asp:BoundField DataField="OutOfChargeDate" HeaderText="Out Of Charge Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="OutOfChargeDate" />
            <asp:BoundField DataField="LastDispatchDate" HeaderText="Dispatch Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="LastDispatchDate" />
            <asp:TemplateField HeaderText="Billing Documents" >
               <ItemTemplate>
                    <asp:LinkButton ID="lnkDocument" runat="server" Text="List of Billing Doc" CommandName="DocumentPopup" CommandArgument='<%#Eval("JobId")+";"+ Eval("DocFolder")+";"+ Eval("FileDirName")%>'></asp:LinkButton>
               </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Aging I" >
               <ItemTemplate>
                <asp:Label ID="lblAgingOne" runat="server" Text='<%#Eval("AgingDispatch") %>' ToolTip="Today – Dispatch Date"></asp:Label>
               </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="ScrutinyDate" HeaderText="Scrutiny Date" DataFormatString="{0:dd/MM/yyyy}" SortExpression="ScrutinyDate" />
            <asp:TemplateField HeaderText="Aging II" >
               <ItemTemplate>
                <asp:Label ID="lblAgingTwo" runat="server" Text='<%#Eval("AgingScrutiny") %>' ToolTip="Today – Scrutiny Date"></asp:Label>
               </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Aging III" >
               <ItemTemplate>
                <asp:Label ID="lblAgingThree" runat="server" Text='<%#Eval("AgingScrutiny") %>' ToolTip="Today – Documents forwarded to Billing Date"></asp:Label>
               </ItemTemplate>
            </asp:TemplateField>            
            <asp:TemplateField HeaderText="Forward to Dispatch Dept" >
               <ItemTemplate>
                    <asp:LinkButton ID="lnkDispatch" runat="server" Text="Forward to Dispatch Dept" OnClientClick="return confirm('Are you sure wants to Move the Job To Dispatch ?');"
                        CommandName="ForwdToDispatch" CommandArgument='<%#Eval("JobId") %>'></asp:LinkButton>
               </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerTemplate>
            <asp:GridViewPager runat="server" />
        </PagerTemplate>
    </asp:GridView>
    </fieldset>
    <!--Document for BIlling Advice Start-->
        <div id="divDocument">
            <cc1:ModalPopupExtender ID="ModalPopupDocument" runat="server" CacheDynamicResults="false"
                DropShadow="False" PopupControlID="Panel2Document" TargetControlID="lnkDummy">
            </cc1:ModalPopupExtender>
                <asp:Panel ID="Panel2Document" runat="server" CssClass="ModalPopupPanel">
                <div class="header">
                    <div class="fleft">
                        &nbsp;<asp:Button ID="btnCancelPopup" runat="server" OnClick="btnCancelPopup_Click" Text="Close" CausesValidation="false" />
                    &nbsp;&nbsp;&nbsp;&nbsp;    
                    <asp:Button ID="btnSaveDocument" Text="Save Document" runat="server" OnClick="btnSaveDocument_Click" ValidationGroup="Required"/>
                    </div>
                    <div class="fright">
                        <asp:ImageButton ID="imgClose" ImageUrl="~/Images/delete.gif" runat="server" OnClick="btnCancelPopup_Click" ToolTip="Close"  />
                    </div>
                </div>
                <div class="m"></div>    
                <div id="Div1" runat="server" style="max-height: 550px; overflow: auto;">
                    <asp:HiddenField ID="hdnJobId" runat="server" />
                    <asp:HiddenField ID="hdnUploadPath" runat="server" />
                    
                    <!--Document for BIlling Advice Start-->
                    <div>
                        <asp:Repeater ID="rptDocument" runat="server" OnItemDataBound="rpDocument_ItemDataBound">
                                <HeaderTemplate>
                                    <table class="table" border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr bgcolor="#FF781E">
                                            <th>
                                                Name
                                            </th>
                                            <th>
                                                Type
                                            </th>
                                            <th>
                                                Browse
                                            </th>
                                        </tr>

                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                    <td>
                                        <asp:CheckBox ID="chkDocType" Text='<%#DataBinder.Eval(Container.DataItem,"sName") %>'
                                          runat="server"/>&nbsp;
                                         <asp:HiddenField ID="hdnDocId" Value='<%#DataBinder.Eval(Container.DataItem,"lId") %>'
                                            runat="server"></asp:HiddenField>   
                                    </td>
                                    <td>
                                        <asp:CustomValidator ID="CVCheckBoxList" runat="server" ClientValidationFunction="ValidateCheckBoxList"
                                            Enabled="false" ErrorMessage="Please Select Type" ValidationGroup="Required" Display="Dynamic"></asp:CustomValidator>
                                        <asp:CheckBoxList id="chkDuplicate" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Original" Value="1"></asp:ListItem> 
                                        <asp:ListItem Text="Copy" Value="2"></asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                    
                                    <td>
                                        <asp:FileUpload ID="fuDocument" runat="server" Enabled="false" />
                                    </td>
                                </tr>    
                                </ItemTemplate>
                                <FooterTemplate>
                                </table>
                                </FooterTemplate>
                            </asp:Repeater>
                    </div>                     
            <!--Document for BIlling Advice- END -->
                 </div>   
                </asp:Panel>
        </div>
        <div>
            <asp:LinkButton ID="lnkDummy" runat="server" Text=""></asp:LinkButton>
        </div>
    <!--Document for BIlling Advice- END -->
    <div>
        <asp:SqlDataSource ID="PCDSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
            SelectCommand="GetPendingPCDBilling" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="UserId" SessionField="UserId" />
                <asp:SessionParameter Name="FinYearId" SessionField="FinYearId" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
