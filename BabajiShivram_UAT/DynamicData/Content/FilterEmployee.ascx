<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterEmployee.ascx.cs" Inherits="DynamicData_Content_FilterEmployee" %>
<!-- Filter Content Start-->
   <table style="width: 100%; background-image:none; background-color:white" class="mawbbg" >
    <tr>
        <td colspan="3">
            <table>
                <tr>
                    <td colspan="2">Search by month</td>
                    <td style="width:10px" rowspan="2"></td>
                    <td colspan="2">Search by period</td>
                    <td style="width:10px" rowspan="2"></td>
                    <td>
                    <select name="ctl00$ContentPlaceHolder1$ddl_cond1_search" id="ctl00_ContentPlaceHolder1_ddl_cond1_search">
	<option selected="selected" value="ship_name">SHIPPER</option>
	<option value="cons_name">CONSIGNEE</option>
	<option value="lot_no">LOT NO.</option>
	<option value="mawb_no">MAWB</option>
	<option value="hawb_no">HAWB</option>
	<option value="flight_no">FLIGHT NO.</option>
	<option value="flight_date">FLIGHT DATE</option>
	<option value="a_departure">POL</option>
	<option value="a_destination">POD</option>

</select>
                    <input name="ctl00$ContentPlaceHolder1$txt_cond1_search" type="text" id="ctl00_ContentPlaceHolder1_txt_cond1_search" />
                    </td>
                </tr>
                <tr>
                    <td>Year
                    <select name="ctl00$ContentPlaceHolder1$ddl_year_search"                     onchange="javascript:setTimeout('__doPostBack(\'ctl00$ContentPlaceHolder1$ddl_year_search\',\'\')', 0)" id="ctl00_ContentPlaceHolder1_ddl_year_search">
	<option value="1999">1999</option>
	<option value="2000">2000</option>
	<option value="2001">2001</option>
	<option value="2002">2002</option>
	<option value="2003">2003</option>
	<option value="2004">2004</option>
	<option value="2005">2005</option>
	<option value="2006">2006</option>
	<option value="2007">2007</option>
	<option value="2008">2008</option>
	<option value="2009">2009</option>
	<option value="2010">2010</option>
	<option selected="selected" value="2011">2011</option>
	<option value="2012">2012</option>
	<option value="2013">2013</option>
	<option value="2014">2014</option>
	<option value="2015">2015</option>
	<option value="2016">2016</option>
	<option value="2017">2017</option>
	<option value="2018">2018</option>
	<option value="2019">2019</option>
	<option value="2020">2020</option>
	<option value="2021">2021</option>
	<option value="2022">2022</option>
	<option value="2023">2023</option>
	<option value="2024">2024</option>
	<option value="2025">2025</option>
	<option value="2026">2026</option>
	<option value="2027">2027</option>
	<option value="2028">2028</option>
	<option value="2029">2029</option>
	<option value="2030">2030</option>
	<option value="2031">2031</option>
	<option value="2032">2032</option>
	<option value="2033">2033</option>
	<option value="2034">2034</option>
	<option value="2035">2035</option>
	<option value="2036">2036</option>
	<option value="2037">2037</option>
	<option value="2038">2038</option>
	<option value="2039">2039</option>
	<option value="2040">2040</option>
	<option value="ALL">ALL</option>

</select>
                    </td>
                    <td>Month
                        <select name="ctl00$ContentPlaceHolder1$ddl_month_search"                         onchange="javascript:setTimeout('__doPostBack(\'ctl00$ContentPlaceHolder1$ddl_month_search\',\'\')', 0)" id="ctl00_ContentPlaceHolder1_ddl_month_search">
	<option value="1">1</option>
	<option value="2">2</option>
	<option value="3">3</option>
	<option value="4">4</option>
	<option value="5">5</option>
	<option value="6">6</option>
	<option value="7">7</option>
	<option value="8">8</option>
	<option value="9">9</option>
	<option value="10">10</option>
	<option value="11">11</option>
	<option selected="selected" value="12">12</option>
	<option value="ALL">ALL</option>

</select>
                    </td>
                     <td>From
                            <span id="ctl00_ContentPlaceHolder1_cal_from_search"><input name="ctl00$ContentPlaceHolder1$cal_from_search$textBox" type="text"                             value="6/11/2011" readonly="readonly" id="ctl00_ContentPlaceHolder1_cal_from_search_textBox" onclick="CalendarPopup_FindCalendar('ctl00_ContentPlaceHolder1_cal_from_search').Show();"                             style="width:70px;" /><input type="button" name="ctl00$ContentPlaceHolder1$cal_from_search$button" value=" ... " onclick="CalendarPopup_FindCalendar('ctl00_ContentPlaceHolder1_cal_from_search').Show();" id="ctl00_ContentPlaceHolder1_cal_from_search_button" class="mabwsubbtn" style="height:22px;width:20px;" /><input name="ctl00$ContentPlaceHolder1$cal_from_search$hidden" type="hidden" id="ctl00_ContentPlaceHolder1_cal_from_search_hidden" value="6/11/2011" /><input name="ctl00$ContentPlaceHolder1$cal_from_search$validateHidden" type="hidden" id="ctl00_ContentPlaceHolder1_cal_from_search_validateHidden" value="11/06/2011" /><input name="ctl00$ContentPlaceHolder1$cal_from_search$enableHidden" type="hidden" id="ctl00_ContentPlaceHolder1_cal_from_search_enableHidden" value="true" /><div id="ctl00_ContentPlaceHolder1_cal_from_search_calendar" style="z-index:5001;position:absolute;display:none;float:left;"></div><div id="ctl00_ContentPlaceHolder1_cal_from_search_monthYear" style="z-index:5003;position:absolute;display:none;float:left;"></div></span>
                    </td>
                    <td>To
                           <span id="ctl00_ContentPlaceHolder1_cal_to_search"><input name="ctl00$ContentPlaceHolder1$cal_to_search$textBox" type="text" value="6/12/2011" readonly="readonly"                            id="ctl00_ContentPlaceHolder1_cal_to_search_textBox" onclick="CalendarPopup_FindCalendar('ctl00_ContentPlaceHolder1_cal_to_search').Show();" style="width:70px;" />                           <input type="button" name="ctl00$ContentPlaceHolder1$cal_to_search$button" value=" ... " onclick="CalendarPopup_FindCalendar('ctl00_ContentPlaceHolder1_cal_to_search').Show();"                            id="ctl00_ContentPlaceHolder1_cal_to_search_button" class="mabwsubbtn" style="height:22px;width:20px;" /><input name="ctl00$ContentPlaceHolder1$cal_to_search$hidden" type="hidden"                            id="ctl00_ContentPlaceHolder1_cal_to_search_hidden" value="6/12/2011" /><input name="ctl00$ContentPlaceHolder1$cal_to_search$validateHidden" type="hidden" id="ctl00_ContentPlaceHolder1_cal_to_search_validateHidden" value="12/06/2011" />                           <input name="ctl00$ContentPlaceHolder1$cal_to_search$enableHidden" type="hidden" id="ctl00_ContentPlaceHolder1_cal_to_search_enableHidden" value="true" />                           <div id="ctl00_ContentPlaceHolder1_cal_to_search_calendar" style="z-index:5001;position:absolute;display:none;float:left;"></div>                           <div id="ctl00_ContentPlaceHolder1_cal_to_search_monthYear" style="z-index:5003;position:absolute;display:none;float:left;"></div></span>
                    </td>
                                        <td>
                    <select name="ctl00$ContentPlaceHolder1$ddl_cond2_search" id="ctl00_ContentPlaceHolder1_ddl_cond2_search">
	<option selected="selected" value="ship_name">SHIPPER</option>
	<option value="cons_name">CONSIGNEE</option>
	<option value="lot_no">LOT NO.</option>
	<option value="mawb_no">MAWB</option>
	<option value="hawb_no">HAWB</option>
	<option value="flight_no">FLIGHT NO.</option>
	<option value="flight_date">FLIGHT DATE</option>
	<option value="a_departure">POL</option>
	<option value="a_destination">POD</option>

</select>
                    <input name="ctl00$ContentPlaceHolder1$txt_cond2_search" type="text" id="ctl00_ContentPlaceHolder1_txt_cond2_search" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
        <tr>
            <td nowrap="nowrap" style="height: 20px;">
                <input type="submit" name="ctl00$ContentPlaceHolder1$bnFind" value="Search" id="ctl00_ContentPlaceHolder1_bnFind" class="mabwbtn" style="background-color:Transparent;border-style:None;" />
                    &nbsp;                    
                <span id="ctl00_ContentPlaceHolder1_RB_export"><input id="ctl00_ContentPlaceHolder1_RB_export_0" type="radio" name="ctl00$ContentPlaceHolder1$RB_export" value="AE" checked="checked" />                <label for="ctl00_ContentPlaceHolder1_RB_export_0">Export</label><input id="ctl00_ContentPlaceHolder1_RB_export_1" type="radio" name="ctl00$ContentPlaceHolder1$RB_export" value="AI"                 onclick="javascript:setTimeout('__doPostBack(\'ctl00$ContentPlaceHolder1$RB_export$1\',\'\')', 0)" /><label for="ctl00_ContentPlaceHolder1_RB_export_1">Import</label></span>
                    &nbsp;                    
                    
                <input type="submit" name="ctl00$ContentPlaceHolder1$bnPrint" value="Print" id="ctl00_ContentPlaceHolder1_bnPrint" class="mabwbtn" style="background-color:Transparent;border-style:None;" />
                &nbsp;
                <select name="ctl00$ContentPlaceHolder1$ddl_printF" id="ctl00_ContentPlaceHolder1_ddl_printF" class="list">
	<option selected="selected" value="XLS">Excel</option>

</select>
                <span id="ctl00_ContentPlaceHolder1_lbError" style="display:inline-block;color:Yellow;font-family:Arial;font-size:12px;height:21px;width:378px;"></span>
            </td>
            <td nowrap="nowrap" style="height: 20px; width:200px" align="left">
                &nbsp;</td>            
            <td nowrap="nowrap" style="height: 20px" align="right">
                <input type="image" name="ctl00$ContentPlaceHolder1$ImageButton1" id="ctl00_ContentPlaceHolder1_ImageButton1" title="Close" src="../Images/x_normal.gif" style="border-width:0px;" /></td>
        </tr>
    </table>
    <!-- Filter Content END-->
