<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="FreightEnqModule_Dashboard"
    MasterPageFile="~/FreightEnqModule/FreightEnqMaster.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <cc1:ToolkitScriptManager runat="server" ID="scrptMngr" />
    <style type="text/css">
        .SearchTextbox
        {
            background: #FFF url(../Images/input-text-search.png) no-repeat right;
        }
        
        .AutoExtender
        {
            font-family: "Helvetica Neue" , Helvetica, Arial, sans-serif;
            font-size: 14px;
            font-weight: normal;
            border: 1px solid #ddd;
            border-radius: 4px;
            box-sizing: border-box;
            display: block;
            position: absolute;
            line-height: 20px;
            padding: 3px;
            background-color: white;
            margin: 0px;
            overflow-y: auto;
            text-align: left;
            list-style: none;
            height: 200px;
            width: 391px;
        }
        .AutoExtenderList
        {
            padding: 6px;
            cursor: pointer;
            font-size: 14px;
        }
        .AutoExtenderHighlight
        {
            padding: 6px;
            background-color: #ddd;
            cursor: pointer;
            font-size: 14px;
        }
        .box .box-title i.fa
        {
            top: 24px;
            right: 5px;
            font-size: 25px;
        }
        .IconChild
        {
            padding: 3px;
            font-size: 15px;
        }
        td.legendColorBox
        {
            padding: 5px;
            padding-left: 10px;
        }
        a, a:focus, a:hover, a:active
        {
            color: Black;
        }
        .black_overlay
        {
            display: block;
            position: absolute;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 100%;
            background-color: black;
            z-index: 1001;
            -moz-opacity: 0.8;
            opacity: .80;
            filter: alpha(opacity=80);
        }
        .white_content
        {
            display: block;
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            padding: 0 8px;
            border: 0px solid #a6c25c;
            background-color: white;
            z-index: 1002;
            overflow: auto;
        }
        .min
        {
            top: 94% !important;
        }
        div#ctl00_ContentPlaceHolder1_ModalPopupExtender1_backgroundElement
        {
            position: absolute;
            left: 0px;
            top: 0px;
            display: none;
        }
        h4
        {
            font-size: 17px;
        }
    </style>
    <script type="text/javascript">
        function MinimizeMsg() {
            var button = document.getElementById('ctl00_ContentPlaceHolder1_btnMinMaxPopup');
            if (button.value.toString() == 'Minimize') {
                var popup = document.getElementById('ctl00_ContentPlaceHolder1_pnlpopup');
                popup.classList.add('min');
                button.value = 'Maximize';
                return false;
            }
            else if (button.value.toString() == 'Maximize') {
                var popup = document.getElementById('ctl00_ContentPlaceHolder1_pnlpopup');
                popup.classList.remove('min');
                button.value = 'Minimize';
                return false;
            }
        }
    </script>
    <!-- WRAPPER -->
    <div id="wrapper" class="clearfix" style="min-height: 200px">
        <aside id="aside">
	        <nav id="sideNav"><!-- MAIN MENU -->
		        <ul class="nav nav-list">
			        <li class="active"><!-- dashboard -->
				        <a class="dashboard" href="Dashboard.aspx"><!-- warning - url used by default by ajax (if eneabled) -->
					        <i class="main-icon fa fa-dashboard"></i> <span>Dashboard</span>
				        </a>
			        </li>	
                    <li><!-- Enquiry -->
                        <a href="FreightEnq.aspx"><!-- warning - url used by default by ajax (if eneabled) -->
	                        <i class="main-icon fa fa-pencil-square-o"></i><span>Enquiry</span>
                        </a>
                    </li>	
                    <li><!-- Enquiry Tracking-->
                        <a href="EnquiryTracking.aspx"><!-- warning - url used by default by ajax (if eneabled) -->
	                        <i class="main-icon fa fa-bar-chart-o"></i><span>Enquiry Tracking</span>
                        </a>
                    </li>																		
		        </ul>		
	        </nav>
	        <span id="asidebg"><!-- aside fixed background --></span>
        </aside>
        <!-- /ASIDE -->
        <!-- HEADER -->
        <header id="header">
	        <!-- Mobile Button -->
	        <button id="mobileMenuBtn"></button>

	        <!-- Logo -->
	        <span class="logo pull-left" style="margin-left:4px;">
		        <img src="FR_assets/images/Babaji_logo3.jpg" alt="admin panel" height="50px" width="225px" style="padding:3px" />
                &nbsp;&nbsp;
            </span>
            <span class="logo pull-left" style="margin-left: 60px;">
               <img src="https://img.alicdn.com/tps/TB1wy3QNXXXXXXMaXXXXXXXXXXX-347-38.png" alt="" height="50px" width="300px" style="padding:3px;">
	        </span>
            <!-- Logo -->

	        <nav>
		        <!-- OPTIONS LIST -->
		        <ul class="nav pull-right">
			        <!-- USER OPTIONS -->
			        <li class="dropdown pull-left">
				        <a href="#" class="dropdown-toggle" data-toggle="dropdown" data-hover="dropdown" data-close-others="true">
					        <img class="user-avatar" alt="" src="FR_assets/images/noavatar.jpg" height="34" /> 
					        <span class="user-name">
						        <span class="hidden-xs">
							       <asp:Label ID="lblUserName" runat="server"></asp:Label> <i class="fa fa-angle-down"></i>
						        </span>
					        </span>
				        </a>
				        <ul class="dropdown-menu hold-on-click">					       
					        <li id="li_Inbox" runat="server"></li>					       
					        <li class="divider"></li>
					        <li><a href="FRlogin.aspx"><i class="fa fa-power-off"></i> Log Out</a></li>
				        </ul>
			        </li>
			        <!-- /USER OPTIONS -->

		        </ul>
		        <!-- /OPTIONS LIST -->
	        </nav>
        </header>
        <!-- /HEADER -->
        <!-- 
				MIDDLE 
			-->
        <section id="middle">
				<div id="content" class="dashboard padding-15">
                <asp:HiddenField ID="hdnMsgsRec" runat="server" Value="0" />
					 <div class="row">
                        <div class="col-md-12 col-sm-12">
                            <div class="" id="dvPendingNumbers" runat="server">                    						
                            </div>
                        </div>
                     </div>        

                    <!--   Morris Normal Bar Graph   -->
                     <div class="row">
                        <div class="col-md-12 col-sm-12">	
                          <div class="panel panel-primary">
                            <div class="panel-heading">
                                <i class="fa fa-bar-chart-o fa-fw"></i> Freight Summary Details                          
                            </div>
                            <div class="panel-body">
                                <div style="text-align: center">
                                    <asp:Label ID="lblFreightSummaryMsg" runat="server"></asp:Label>
                                </div>
                                <div id="dvGraphDonut" runat="server" class="panel panel-default">
                                    <!-- panel content -->
                                    <div class="panel-body nopadding">
                                        <div id="graph-normal-bar"><!-- GRAPH CONTAINER --></div>
                                        </div>
                                    <!-- /panel content -->
                                </div>
                            </div>
                          </div>				                          
                        </div>                     
                    </div>
                    <!--   Morris Normal Bar Graph   -->
                    <!-- /FREIGHT SUMMARY DETAILS -->

                    <!-- FREIGHT USER SUMMARY REPORT -->                   
                    <div class="row">
                        <div class="col-md-12 col-sm-12">	
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    <i class="fa fa-bar-chart-o fa-fw"></i> Freight Customer Summary Report                          
                                </div>
                                <div class="panel-body">
                                <div class="row" style="margin-top: 2px; margin-bottom: 2px">
                                    <div class="col-sm-2 col-md-2" style="padding-right: 2px">
                                      <div class="fancy-form">
                                      <label>Month</label>
                                         <asp:DropDownList ID="ddMonth" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddMonth_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="Financial Month"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="January"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="February"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="March"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="April"></asp:ListItem>
                                            <asp:ListItem Value="5" Text="May"></asp:ListItem>
                                            <asp:ListItem Value="6" Text="June"></asp:ListItem>
                                            <asp:ListItem Value="7" Text="July"></asp:ListItem>
                                            <asp:ListItem Value="8" Text="Aug"></asp:ListItem>
                                            <asp:ListItem Value="9" Text="Sep"></asp:ListItem>
                                            <asp:ListItem Value="10" Text="Oct"></asp:ListItem>
                                            <asp:ListItem Value="11" Text="Nov"></asp:ListItem>
                                            <asp:ListItem Value="12" Text="Dec"></asp:ListItem>
                                        </asp:DropDownList>
                                        <span class="fancy-tooltip top-left"> <!-- positions: .top-left | .top-right -->
                                            <em>Select Financial Month</em>
                                        </span>   
                                      </div>
                                    </div>
                                    <div class="col-sm-4 col-md-4" style="padding-left: 2px; padding-right: 2px" id="dvCustomer" runat="server">
                                        <label>Customer</label>
                                        <asp:TextBox ID="txtCustomer" runat="server" TabIndex="3" placeholder="Customer" CssClass="form-control SearchTextbox"
                                            AutoPostBack="true" OnTextChanged="txtCustomer_OntextChanged"></asp:TextBox>                           
                                        <div id="divwidthCust"></div>
                                        <cc1:AutoCompleteExtender ID="CustomerExtender" runat="server" TargetControlID="txtCustomer"
                                            CompletionListElementID="divwidthCust" ServicePath="../WebService/FreightCustomerAutoComplete.asmx"
                                            ServiceMethod="GetCompletionList" MinimumPrefixLength="2" BehaviorID="divwidthCust" 
                                            ContextKey="4317" UseContextKey="True" CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                            CompletionListHighlightedItemCssClass="AutoExtenderHighlight" FirstRowSelected="true">
                                        </cc1:AutoCompleteExtender>  
                                    </div>
                                    <div class="col-sm-2 col-md-2" style="padding-left: 2px;" id="dvMode" runat="server">
                                       <div class="fancy-form">
                                       <label>Mode</label>
                                         <asp:DropDownList ID="ddlMode" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlMode_SelectedIndexChanged">
                                            <asp:ListItem Value="1" Selected="True">Air</asp:ListItem>  
                                            <asp:ListItem Value="2">Sea</asp:ListItem>                                         
                                        </asp:DropDownList>
                                        <span class="fancy-tooltip top-left"> <!-- positions: .top-left | .top-right -->
                                            <em>Select Mode</em>
                                        </span>   
                                      </div>
                                    </div>
                                </div>
                                                             
                                    <table id="tblIconDetail" runat="server" style="position: absolute; top: 110px; right: 50px; font-size: smaller; color: #545454; padding: 125px;">
                                        <tbody>
                                            <tr>
                                                <td class="legendColorBox">
                                                    <div>
                                                        <div style="width:4px; height:0; border:5px solid #0b62a4; overflow:hidden"></div>
                                                    </div>
                                                </td>
                                                <td class="legendLabel">Enquiry</td>
                                                <td class="legendColorBox">
                                                    <div>
                                                        <div style="width:4px;height:0;border:5px solid #7a92a3;overflow:hidden"></div>
                                                    </div>
                                                </td>
                                                <td class="legendLabel">Quoted</td>
                                                <td class="legendColorBox">
                                                    <div>
                                                        <div style="width:4px;height:0;border:5px solid #4da74d;overflow:hidden"></div>
                                                    </div>
                                                </td>
                                                <td class="legendLabel">Awarded</td>                                             
                                                <td class="legendColorBox">
                                                    <div>
                                                        <div style="width:4px;height:0;border:5px solid #edc240;overflow:hidden"></div>
                                                    </div>
                                                </td>
                                                <td class="legendLabel">Executed</td>
                                                  <td class="legendColorBox">
                                                    <div>
                                                        <div style="width:4px;height:0;border:5px solid #cb4b4b;overflow:hidden"></div>
                                                    </div>
                                                </td>
                                                <td class="legendLabel">Lost</td>
                                            </tr>
                                        </tbody>
                                     </table>			
                                     
                                    <div style="text-align: center">
                                        <asp:Label ID="lblMsg" runat="server"></asp:Label>
                                    </div>
                                    <%LoginClass loggedInClass = new LoginClass();
                                      if (loggedInClass.glRoleId.ToString() != "")
                                      {
                                          if (loggedInClass.glRoleId.ToString() == "53")
                                          {
                                     %>         
                                     
                                    <!-- Stacked Graph -->
							        <div id="panel-graphs-morris-r6" class="panel panel-default">								
								        <div class="panel-body" >
									        <div id="graph-stacked"><!-- GRAPH CONTAINER --></div>
								        </div>		                                              					 
							        </div>
							        <!-- /Stacked Graph -->

                                    <%    }
                                          else
                                          {
                                     %>
                                    <!-- Donut Graph -->
					                <div id="panel-graphs-morris-r4" class="panel panel-default">

						                <!-- panel content -->
						                <div class="panel-body nopadding">
							                <div id="graph-donut"><!-- GRAPH CONTAINER --></div>
						                </div>
						                <!-- /panel content -->

					                </div>
					                <!-- /Donut Graph -->  
                                     <%    }
                                      }
                                     %>
                                </div>
                            </div>
                        </div>
                   
                    </div>
                    <!-- /FREIGHT USER SUMMARY REPORT -->                             

				</div>
		</section>
        <!-- /MIDDLE -->
    </div>
    <!-- JAVASCRIPT FILES -->
    <script type="text/javascript">
        var plugin_path = 'FR_assets/plugins/';
    </script>
    <script type="text/javascript" src="FR_assets/plugins/jquery/jquery-2.1.4.min.js"></script>
    <script type="text/javascript" src="FR_assets/js/app.js"></script>
    <script src="FR_assets/plugins/chart.chartjs/canvasjs.min.js" type="text/javascript"></script>
    <!-- MODAL POPUP EVENTS -->
    <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="HiddenHL"
        PopupControlID="pnlpopup" CancelControlID="btnCancelPopup" BackgroundCssClass="modalBackground"
        X="700" Y="160">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlpopup" runat="server" Style="display: none;">
        <div class="row">
            <div class="col-md-12 col-sm-12">
                <div class="panel panel-info">
                    <div class="panel-heading">
                        MESSAGING <span class="pull-right">
                            <asp:Button ID="btnCancel" runat="server" OnClick="btnCancelPopup_OnClick" Text="Close"
                                CssClass="btn-xs btn-3d btn-default" CausesValidation="false" />
                            &nbsp;
                            <asp:Button ID="btnMinMaxPopup" runat="server" Text="Minimize" OnClientClick="return MinimizeMsg();"
                                CssClass="btn-xs btn-3d btn-default" CausesValidation="false" />
                        </span>
                    </div>
                    <div id="dvMsgAll" runat="server">
                    </div>
                    <div class="panel-body" style="background-color: rgba(66, 139, 202, 0.16); padding-bottom: 1px">
                        <%--<div class="col-md-8">--%>
                        <div class="col-md-8 col-sm-8" style="padding-right: 2px; padding-left: 2px">
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    RECENT CHAT HISTORY
                                </div>
                                <div id="dvRecentChats" runat="server" class="panel-body" style="background-color: White">
                                </div>
                                <div class="panel-footer">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtMessage" runat="server" CssClass="form-control" placeholder="Enter Message"></asp:TextBox>
                                        <span class="input-group-btn">
                                            <asp:Button ID="btnSendMsg" runat="server" Text="SEND" Height="34px" CssClass="btn btn-info"
                                                OnClick="btnSendMsg_OnClick" />
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4" style="padding-right: 2px; padding-left: 2px">
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    USERS
                                </div>
                                <div id="dvOnlineUsers" runat="server" class="panel-body" style="background-color: White">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:HyperLink ID="HiddenHL" runat="server" />
    <asp:Button ID="btnCancelPopup" runat="server" OnClick="btnCancelPopup_OnClick" CausesValidation="false" />
    <!-- /MODAL POPUP EVENTS -->
</asp:Content>
