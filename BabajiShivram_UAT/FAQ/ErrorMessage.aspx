<%@ Page Title="" Language="C#" MasterPageFile="~/FAQ/FAQMaster.master" AutoEventWireup="true" CodeFile="ErrorMessage.aspx.cs" Inherits="FAQ_ErrorMessage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <style type="text/css">
        .form-style-6 {
            height: 50%;
        }
    </style>
    <div class="wrapper col1">
       <%-- <div id="header">
            <div id="logo">
               
               
            </div>
             <div id="newsletter">
                <fieldset>
                    <legend>NewsLetter</legend>
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/babajiNew.png" />
                </fieldset>

            </div>
            <br class="clear" />
        </div>--%>
         <asp:Panel ID="Panservice" runat="server" Visible="false">
                    <p>
                        <strong>Service:
                            <asp:Label ID="lblservice" runat="server"></asp:Label></strong>
                    </p>
                </asp:Panel>
    </div>
    <div class="wrapper col2">
        <div id="topbar">
            <div id="topnav">
                <ul>
                    <li class="active"></li>
                    <li></li>
                    <li></li>
                    <li>
                        <ul>
                        </ul>
                    </li>

                </ul>
            </div>
            <div id="search">
                <fieldset>
                    <legend></legend>
                    <div id="divwidthservice">
                    </div>

                </fieldset>
            </div>
            <br class="clear" />

        </div>
    </div>
    <div class="wrapper col3">
        <div id="breadcrumb">
            <asp:Panel runat="server" ID="panback">
                <ul>
                    <li class="first"></li>

                    <li><a href="FAQ.aspx">Back</a></li>
                    <li>&#187;</li>
                </ul>
                <center>
              <asp:Label ID="lblmssege" runat="server" Text="Service Record Not Found" ForeColor="Red" Font-Size="large"></asp:Label></center>
            </asp:Panel>
            <center><asp:Label ID="lblError" runat="server" EnableViewState="false" Style="font-size: large; color: red"></asp:Label></center>
        </div>
    </div>
    <div class="wrapper col3">
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
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
    </div>
    </div>
    

</asp:Content>

