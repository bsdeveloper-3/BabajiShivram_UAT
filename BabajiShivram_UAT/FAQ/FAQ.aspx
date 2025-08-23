<%@ Page Title="" Language="C#" MasterPageFile="~/FAQ/FAQMaster.master" AutoEventWireup="true"
    CodeFile="FAQ.aspx.cs" Inherits="FAQ_FAQ " EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <style type="text/css">
        .form-style-6 {
            height: 50%;
        }
    </style>

    <script type="text/javascript">

        function OnServicesSelected(source, eventArgs) {

            var results = eval('(' + eventArgs.get_value() + ')');
            document.getElementById('<%=hdnServiceId.ClientID%>').value = results.ServiceId;
            //alert(results.ServiceId)

        }

        $addHandler
        (
        document.getElementById('<%=TxtService.ClientID%>'), 'keyup',
                    function () {
                        document.getElementById('<%=hdnServiceId.ClientID %>').value = '0';
                    }
            );
    </script>
    <cc1:ToolkitScriptManager runat="server" ID="ScriptManager1" ScriptMode="Release" />
    <script type="text/javascript">

        function ActiveTabChanged12() {
            /* Clear Error Message on Tab change Event */
            document.getElementById('<%=lblError.ClientID%>').innerHTML = '';
            document.getElementById('<%=lblError.ClientID%>').className = '';
        }

    </script>
   <%-- <div class="wrapper col1">
        <div id="header">
            <div id="logo">
                <h1><a href="#">Frequently Asked Questions</a></h1>
               
            </div>
            <div id="newsletter">
                <fieldset>
                    <legend></legend>
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/babajiNew.png" />
                </fieldset>

            </div>
            <br class="clear" />
        </div>
    </div>--%>
    <%--<div class="wrapper col1">
     <asp:Panel ID="Panservice" runat="server" Visible="false">
                    <p>
                        <strong>Service:
                        <asp:Label ID="lblservice" runat="server"></asp:Label></strong>
                    </p>
                </asp:Panel>
        </div>--%>
     <asp:Panel ID="Panservice" runat="server" Visible="false">
         </asp:Panel>
    <div class="wrapper col2">
        <div id="topbar">
            <div id="topnav">
                <ul>
                    <li class="active"><a href="FAQ.aspx">HOME</a></li>
                    <li><a href="../Login.aspx">GO TO LOGIN PAGE</a></li>
                </ul>
            </div>
            <div id="search">
                <fieldset>
                    <legend></legend>
                    <div id="divwidthservice">
                    </div>
                    <table class="active" style="padding: 1px">
                        <tr>
                            <td>
                                <asp:TextBox ID="TxtService" runat="server" placeholder="Search here..."
                                    AutoPostBack="true" Style="width: 100%"></asp:TextBox>

                                <asp:HiddenField ID="hdnServiceId" runat="server" Value="0" />
                            </td>
                            <td>
                            <asp:Button ID="btnSearch" runat="server" Height="10%" Width="100%" Text="Search" OnClick="btnSearch_Click" BackColor="White" /></td>
                        </tr>
                    </table>
                </fieldset>
            </div>
           <br class="clear" />
        </div>
    </div>
    <div class="wrapper col3">
        <div id="breadcrumb">
            <asp:Panel runat="server" ID="panback" Visible="false">
                <ul>
                    <li class="first"></li>
                    <li><a href="FAQ.aspx">Back</a></li>
                     <li>&nbsp;&nbsp;</li>
                     <li>&#187;</li>
                    <li> <strong>Service:
                        <asp:Label ID="lblservice" runat="server"></asp:Label></strong></li>
                </ul>
            </asp:Panel>
            
           <center><asp:Label ID="lblError" runat="server" EnableViewState="false" Style="font-size: large; color: red"></asp:Label></center>
        </div>
    </div>
    <div class="wrapper col3">
        <div class="wrapper col3">
            <center> <asp:Panel ID="PanAllList" runat="server" Visible="true">
            <div id="div21">
                <cc1:AutoCompleteExtender ID="ServiceCompleteExtender" runat="server" TargetControlID="TxtService"
                    CompletionListElementID="divwidthservice" ServicePath="~/WebService/ServiceAutoComplete.asmx"
                    ServiceMethod="GetServiceList" MinimumPrefixLength="0" BehaviorID="divwidthservice"
                    ContextKey="1234" UseContextKey="True" OnClientItemSelected="OnServicesSelected"
                     CompletionListItemCssClass="AutoExtenderList"
                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                </cc1:AutoCompleteExtender>
            </div>
          <asp:GridView ID="GrvFAQAllList" runat="server" AutoGenerateColumns="False"
                Width="70%" DataSourceID="FAQServiceAllListDataSorce"
                CellPadding="0" DataKeyNames="LID" Style="border: hidden; padding: 4px"
                AllowPaging="True" AllowSorting="True" PageSize="20" 
            
                OnRowCommand="GrvFAQAllList_RowCommand">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Image ImageUrl="~/Images/arrows.png" CommandName="select" runat="server"
                                ID="btn_Edit" ToolTip="Select" Height="15px" Width="20px" style="display:inline-block;" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                             <asp:LinkButton ID="lnkSelect" runat="server" CommandName="select" Text='<%#Eval("Title") %>'
                                CommandArgument='<%#Eval("lId")%>' Height="20px" Font-Size="11px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle HorizontalAlign="Right" BorderWidth="0px" />
            </asp:GridView>

            <asp:SqlDataSource ID="FAQServiceAllListDataSorce" runat="server" ConnectionString="<%$ ConnectionStrings:ConBSImport %>"
                SelectCommand="FAQ_GetAllServicesList" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter Name="UserId" SessionField="UserId" />
                </SelectParameters>
            </asp:SqlDataSource>
        </asp:Panel>
        </center>
            <center> <asp:Panel ID="PanelService" runat="server" Visible="false">
               
                <asp:Label ID="lblCfs" runat="server"></asp:Label>
               <asp:GridView ID="GridFaqDetails"  runat="server" AutoGenerateColumns="False"
                    Width="70%"
                    OnRowCommand="GridFaqDetails_RowCommand" Style="border: hidden; border-bottom: hidden"
                    CellPadding="3" BorderWidth="0px"  BorderStyle="Solid"
                    AllowPaging="True" AllowSorting="True" PageSize="20" DataKeyNames="LId">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Image ImageUrl="~/Images/arrows.png" CommandName="select" runat="server"
                                    ID="btn_Edit" ToolTip="Edit Organisation" Height="15px" Width="20px" style="display:inline-block"/>
                               <asp:LinkButton ID="lnkSelect" runat="server" CommandName="select" Text='<%#Eval("Title") %>'
                                    CommandArgument='<%#Eval("lId")%>' Height="30px" Font-Size="Small" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle HorizontalAlign="Right" BorderWidth="0px" />
                </asp:GridView>
            </asp:Panel></center>
        </div>
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <div class="wrapper col6" style="background-color:white">
            <div id="footer">
                   
                
                <div class="footbox">
                    
                </div>
                <br class="clear" />
            </div>
        </div>

    </div>





</asp:Content>

