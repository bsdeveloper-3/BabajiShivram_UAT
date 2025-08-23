<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PCDBillReturnProcess.aspx.cs" Inherits="PCA_PCDBillReturnProcess" 
    EnableEventValidation="false" Culture="en-GB"
    %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/DataFilter.ascx" TagName="DataFilter" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">    

        function OnShippingSelected(source, eventArgs) {
            var results = eval('(' + eventArgs.get_value() + ')');

            $get('<%=hdnCustId.ClientID%>').value = results.ClientId;
        }
        $addHandler
        (
            $get('txtSearchBillNo'), 'keyup',

            function () {
                $get('<%=txtSearchBillNo.ClientID%>').value = '0';
            }
        );
    </script>

    <asp:scriptmanager runat="server" id="ScriptManager1" />

          <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
          DisplayMode ="List" ShowSummary = "true" ValidationGroup="ReqSummary" />

<asp:updatepanel id="updBillReturn" runat="server" updatemode="Conditional" rendermode="Inline">
        <ContentTemplate>      

            <div align="center" style="vertical-align: top">
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
                <%--<asp:Label ID="lblFinYear" runat="server" ></asp:Label>--%>
            </div>
            <fieldset>
                <legend>Bill Return</legend>
                <div class="m clear">
                    <table>
                        <tr>
                            <td>Search Bill No.
                            </td>
                            <td><%-- OnTextChanged="txtShippingAirline_TextChanged" AutoPostBack="true"--%>
                                <asp:TextBox ID="txtSearchBillNo" runat="server" ToolTip="Enter Shipping/Airline Name" 
                                    placeholder="Search" Width="200px" OnTextChanged="txtSearchBillNo_TextChanged" AutoPostBack="true"></asp:TextBox>
                                <asp:HiddenField ID="hdnCustId" runat="server" />
                                <%--<asp:HiddenField ID="hdnShipCompId" runat="server" />--%>

                                <div id="divwidthBill">
                                </div>
                                <cc1:autocompleteextender id="AutoCompleteExtender10" runat="server" targetcontrolid="txtSearchBillNo"
                                    completionlistelementid="divwidthBill" servicepath="~/WebService/CustBillReturn.asmx"
                                    servicemethod="GetBillReturnList" minimumprefixlength="2" behaviorid="divwidthBill"
                                    ContextKey="1"
                                    usecontextkey="True" onclientitemselected="OnShippingSelected"
                                    completionlistcssclass="AutoExtender" completionlistitemcssclass="AutoExtenderList"
                                    completionlisthighlighteditemcssclass="AutoExtenderHighlight">
                                </cc1:autocompleteextender>
                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <fieldset>
                        <legend>Bill Return Detail</legend>
                        <asp:GridView ID="gridBillReturn" runat="server" AutoGenerateColumns="False" CssClass="table"
                            Width="100%" PagerStyle-CssClass="pgr" CellPadding="4" AllowPaging="True"
                            AllowSorting="True" PageSize="20" OnRowCommand="gridBillReturn_RowCommand">
                            <%--OnRowCommand="GridViewDocument_RowCommand">--%>
                            <Columns>
                                <asp:TemplateField HeaderText="SI">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="BJV No">
                                    <ItemTemplate>                                        
                                        <asp:Label ID="lblBJVNo" runat="server" Text='<%#Bind("BJVNO") %>'></asp:Label>                                     
                                        <asp:Label ID="lblJobId" runat="server" Text='<%#Bind("JobId") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustName" runat="server" Text='<%#Bind("CustName") %>'></asp:Label>                                     
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Customer Ref No">
                                    <ItemTemplate>
                                        <asp:Label ID="lbllid" runat="server" Text='<%#Bind("lid") %>'></asp:Label>    
                                        <asp:Label ID="lblCustRfNo" runat="server" Text='<%#Bind("CustRefNo") %>'></asp:Label>                                     
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Bill No" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBillNo" runat="server" Text='<%#Bind("INVNO") %>'></asp:Label>                                     
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblINVAMOUNT" runat="server" Text='<%#Bind("INVAMOUNT") %>'></asp:Label>                                     
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Is Return" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                       <asp:CheckBox ID="chkIsReturn" runat="server" />                                 
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reason">
                                    <ItemTemplate>
                                       <asp:DropDownList ID="ddlReason" runat="server">
                                           <asp:ListItem Value="0" Text="-Select-"></asp:ListItem>
                                           <asp:ListItem Value="1" Text="GST Invoice Related"></asp:ListItem>
                                           <asp:ListItem Value="2" Text="Teriff/Contract"></asp:ListItem>
                                           <asp:ListItem Value="3" Text="SOP Contract"></asp:ListItem>
                                           <asp:ListItem Value="4" Text="Dispatch/Wrong Attension"></asp:ListItem>
                                           <asp:ListItem Value="5" Text="Warehouse"></asp:ListItem>
                                           <asp:ListItem Value="6" Text="Shipping Line"></asp:ListItem>
                                           <asp:ListItem Value="7" Text="CFS"></asp:ListItem>
                                           <asp:ListItem Value="8" Text="Other"></asp:ListItem>
                                       </asp:DropDownList>                      
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Return Remark /Error">
                                    <ItemTemplate>
                                      <asp:TextBox ID="txtRemark" runat="server"></asp:TextBox> 
                                    <%-- <asp:RequiredFieldValidator ID="rfvRemark" runat="server" ControlToValidate ="txtRemark"
                                       ErrorMessage="*" >   
                                    </asp:RequiredFieldValidator>         --%>                 
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                      <asp:Button ID="btnOK" runat="server" Text="OK" Width="50px" CommandName="UpdBillReturn"/>            
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        
                    </fieldset>
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:updatepanel>

</asp:Content>

