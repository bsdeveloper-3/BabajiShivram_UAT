<%@ Page Title="KPI LIST" Language="C#" MasterPageFile="~/CommonMaster.master" AutoEventWireup="true" CodeFile="ListKPI.aspx.cs" 
    EnableEventValidation="false" Inherits="Service_ListKPI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <link href="../CSS/babaji-shivram.css" rel="stylesheet" type="text/css" />
    <div class="right_col" role="main" style="background-color: #feeace">
        <div class="row tile_count">
            <div class="x_panel" style="width: 90%; height: 100%; background-color: #b0e0e6">
                <div align="center">
                    <asp:Label ID="lblError" runat="server" Font-Size="14px"></asp:Label>
                </div>
                <div class="tab-content">
                    <h2 style="color: #337ab7">
                        EMPLOYEE KPI LIST
                    </h2>
                    <asp:LinkButton ID="lnkexport" runat="server" OnClick="lnkExport_Click">
                        <asp:Image ID="imgExport" runat="server" ImageUrl="../images/Excel.jpg" ToolTip="Export To Excel" />
                    </asp:LinkButton>
                </div>
                <div class="x_panel">
                    <div class="x_content">
                    <div class="row">
                    <asp:GridView ID="gvKPIList" runat="server" AutoGenerateColumns="False" CssClass="table" Width="99%" PagerStyle-CssClass="pager" 
                        DataKeyNames="lId" DataSourceID="DataSourceKPI" CellPadding="4" AllowPaging="True" AllowSorting="True" PageSize="40"
                        PagerSettings-Position="TopAndBottom"  OnRowEditing="gvKPIList_RowEditing" OnRowUpdating="gvKPIList_RowUpdating">
                        <Columns>
                            <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEdit" CommandName="Edit" runat="server" Text="Edit" 
                                    ToolTip="Click To Update Employee Detail"></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkUpdate" CommandName="Update" ToolTip="Update Detail" runat="server"
                                    Text="Update" ValidationGroup="Required"></asp:LinkButton>
                                <asp:LinkButton ID="lnkCancel" CommandName="Cancel" ToolTip="Cancel Update" CausesValidation="false"
                                    runat="server" Text="Cancel"></asp:LinkButton>
                            </EditItemTemplate>
                        </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sl">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Emp Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblEmpName" runat="server" Text='<%#Bind("EmpName")%>' ></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:RequiredFieldValidator ID="RFVEmpName" runat="server" ErrorMessage="*"
                                    ForeColor="Red" ControlToValidate="txtEmpName" SetFocusOnError="True"
                                    ValidationGroup="Required"></asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtEmpName" runat="server" Text='<%#Bind("EmpName")%>' Maxlength="50" ></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Emp Code">
                                <ItemTemplate>
                                    <asp:Label ID="lblEmpCode" runat="server" Text='<%#Bind("EmpCode")%>' ></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:RequiredFieldValidator ID="RFVEmpCode" runat="server" ErrorMessage="*"
                                    ForeColor="Red" ControlToValidate="txtEmpCode" SetFocusOnError="True"
                                    ValidationGroup="Required"></asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtEmpCode" runat="server" Text='<%#Bind("EmpCode")%>' Maxlength="50" ></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email">
                                <ItemTemplate>
                                    <asp:Label ID="lblEmpEmail" runat="server" Text='<%#Bind("EmpEmail")%>' ></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:RequiredFieldValidator ID="RFVEmail" runat="server" ErrorMessage="*"
                                    ForeColor="Red" ControlToValidate="txtEmpEmail" SetFocusOnError="True"
                                    ValidationGroup="Required"></asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtEmpEmail" runat="server" Text='<%#Bind("EmpEmail")%>' Maxlength="100" ></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="HOD">
                                <ItemTemplate>
                                    <asp:Label ID="lblHODName" runat="server" Text='<%#Bind("HOD")%>' ></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:RequiredFieldValidator ID="RefHOD" runat="server" ControlToValidate="ddHOD"
                                    Text="*" Display="Dynamic" ErrorMessage="*" ValidationGroup="Required" SetFocusOnError="true"
                                    ForeColor="Red" InitialValue="0"></asp:RequiredFieldValidator>
                                    <asp:DropDownList ID="ddHOD" runat="server" class="form-control" SelectedValue='<%#Bind("HODID") %>' >
                                    <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Chairman" Value="189"></asp:ListItem>
                                    <asp:ListItem Text="Abdul Qureshi" Value="206"></asp:ListItem>
                                    <asp:ListItem Text="Adiraju Murthy" Value="940"></asp:ListItem>
                                    <asp:ListItem Text="Amod Pathak" Value="663"></asp:ListItem>
                                    <asp:ListItem Text="Amit Thakur" Value="883"></asp:ListItem>
                                    <asp:ListItem Text="Devendra Donde" Value="313"></asp:ListItem>
                                    <asp:ListItem Text="Dhaval Davada" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="G. Badrinarayanan " Value="181"></asp:ListItem>
                                    <asp:ListItem Text="Harish Kumar" Value="8"></asp:ListItem>
                                    <asp:ListItem Text="Javed Shaikh" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="Mandar Patil" Value="224"></asp:ListItem>
                                    <asp:ListItem Text="Manish Radhakrishnan" Value="185"></asp:ListItem>
                                    <asp:ListItem Text="Manisha Waghmare" Value="68"></asp:ListItem>
                                    <asp:ListItem Text="Navin Sequeira" Value="933"></asp:ListItem>
                                    <asp:ListItem Text="Navin Sharma" Value="938"></asp:ListItem>
                                    <asp:ListItem Text="Neeraj Mandovara" Value="926"></asp:ListItem>
                                    <asp:ListItem Text="Rekha Deshpande" Value="9"></asp:ListItem>
                                    <asp:ListItem Text="Sajid Shaikh" Value="1021"></asp:ListItem>
                                    <asp:ListItem Text="Siddhi Bhosale" Value="23"></asp:ListItem>
                                    <asp:ListItem Text="Sunil Shekhawat" Value="931"></asp:ListItem>
                                    <asp:ListItem Text="Tarique Pathan" Value="932"></asp:ListItem>
                                    <asp:ListItem Text="Thanigaivelu" Value="112"></asp:ListItem>
                                    <asp:ListItem Text="Vishal Diwevdi" Value="106"></asp:ListItem>
                                    <asp:ListItem Text="Yogesh Sawant" Value="928"></asp:ListItem>
                                </asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                <asp:SqlDataSource ID="DataSourceKPI" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                    SelectCommand="KPI_GetALLKPI" SelectCommandType="StoredProcedure">
                </asp:SqlDataSource>
        </div>
            
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

