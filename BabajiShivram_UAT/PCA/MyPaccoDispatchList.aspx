<%@ Page Title="MyPacco Dispatch" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MyPaccoDispatchList.aspx.cs" 
    Inherits="PCA_MyPaccoDispatchList" EnableEventValidation="false" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
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
        function SingleCheckboxCheck(ob) {
                var gridvalue = ob.parentNode.parentNode.parentNode;
                var inputs = gridvalue.getElementsByTagName("input");

                for (var i = 0; i < inputs.length; i++) {
                    if (inputs[i].type == "checkbox") {
                        if (ob.checked && inputs[i] != ob && inputs[i].checked) {
                            inputs[i].checked = false;
                        }
                    }
                }
            }
    </script>
    <asp:HiddenField ID="hdnJobIdList" runat="server" Value="0" />
    <asp:HiddenField ID="hdnCustomerId" runat="server" Value="0" />
    <asp:HiddenField ID="hdnCustomerName" runat="server" />
    <asp:HiddenField ID="hdnBillId" runat="server" Value="0" />
    <div align="center">
        <asp:Label ID="lblMessage" runat="server"></asp:Label>
    </div>

    <fieldset runat="server">
    <legend>AWB Generation</legend>
        <div class="fleft">
            <asp:DropDownList ID="ddBranch" runat="server">
                <asp:ListItem Text="Dispatch From Location" Value="0"></asp:ListItem>
                <asp:ListItem Text="Mumbai" Value="3"></asp:ListItem>
                <asp:ListItem Text="Delhi" Value="5"></asp:ListItem>
                <asp:ListItem Text="Chennai" Value="6"></asp:ListItem>
            </asp:DropDownList>
                &nbsp;&nbsp;&nbsp;<asp:Button ID="btnGenerateAWB" Text="Generate AWB" runat="server" OnClick="btnGenerateAWB_Click"> </asp:Button>
        </div>
        <div class="m clear">
            <asp:GridView ID="gvDispatchPlantAddress" runat="server" Width="100%" AutoGenerateColumns="False"
                CssClass="table" PagerStyle-CssClass="pgr" DataKeyNames="AddressId" PageSize="40" AllowPaging="true"
                PagerSettings-Position="TopAndBottom">
                <Columns>
                    <asp:TemplateField HeaderText="Sl">
                        <ItemTemplate>
                            <%# Container.DataItemIndex +1%>
                            <asp:CheckBox ID="chkAddress" runat="server" onclick ="SingleCheckboxCheck(this)"/> 
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ContactPerson" HeaderText="Contact Name" /> 
                    <asp:BoundField DataField="MobileNo" HeaderText="Mobile No" /> 
                    <asp:BoundField DataField="AddressLine1" HeaderText="Address1" /> 
                    <asp:BoundField DataField="AddressLine2" HeaderText="Address2" /> 
                    <asp:BoundField DataField="City" HeaderText="City" /> 
                    <asp:BoundField DataField="Pincode" HeaderText="Pincode" /> 
                </Columns>   
            </asp:GridView>
        </div>
        <div class="m clear">
        <asp:GridView ID="gvJobDetail" runat="server" AutoGenerateColumns="False"
            CssClass="table" Width="99%" PagerStyle-CssClass="pgr"
            DataKeyNames="JobId,Billid" DataSourceID="DataSourceBillJobList" CellPadding="4"
            AllowPaging="True" AllowSorting="True" PageSize="1000" OnRowCommand="gvJobDetail_RowCommand"
            OnRowDataBound="gvJobDetail_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="Sl">
                    <ItemTemplate>
                        <%#Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkboxSelectAll" align="center" ToolTip="Check All" runat="server" onclick="GridSelectAllColumn(this);" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkBillNo" runat="server" /> 
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Job No">
                    <ItemTemplate>
                        <asp:Label ID="lblBJVNo" runat="server" Text='<%#Eval("BJVNo")%>'/> 
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Bill Number">
                    <ItemTemplate>
                        <asp:Label ID="lblBillNumber" runat="server" Text='<%#Eval("INVNO")%>'/> 
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Bill Date" DataField="INVDATE" DataFormatString="{0:dd/MM/yyyy}"/>
                <asp:BoundField HeaderText="Bill Amount" DataField="INVAMOUNT" />
                <asp:BoundField HeaderText="Order No" DataField="OrderNo" />
                <asp:BoundField HeaderText="AWB No" DataField="AWBNo" />
                <asp:BoundField HeaderText="Courier Name" DataField="LSPName"/>
                <asp:BoundField HeaderText="AWB Date" DataField="AWBDate" DataFormatString="{0:dd/MM/yyyy}"/>
                <asp:BoundField HeaderText="User Name" DataField="DispatchUserName"/>
                <%--<asp:TemplateField HeaderText="View">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkCoverView" runat="server" Text="View" CommandName="Download" CommandArgument='<%#Eval("CoverId")%>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                </Columns>
        </asp:GridView>
        </div>

        <div id="divDatasource">
        <asp:SqlDataSource ID="DataSourceBillJobList" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
        SelectCommand="BL_GetBillJobDetailMyPacco" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter Name="JobIdList"/>
            <asp:Parameter Name="ModuleId"/>
        </SelectParameters>
    </asp:SqlDataSource>
    </div>
</fieldset>

</asp:Content>


