<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="CRM_Default" %>

<!DOCTYPE html>

<html>
<body style="height: 100%; width: 100%; font-family: Calibri; font-style: normal; font-size: 9pt; color: #000; padding-top: 100px">
    <style type="text/css">
        .tg {
            border-collapse: collapse;
            border-spacing: 0;
            border-color: #aaa;
            box-sizing: border-box;
        }

            .tg th {
                font-family: Calibri;
                padding: 5px 5px;
                font-weight: 500;
                overflow: hidden;
                word-break: normal;
                border-color: darkgray;
                color: black;
                background-color: white;
            }

            .tg td {
                font-family: Calibri;
                font-size: 17px;
                padding: 10px 5px;
                border-style: solid;
                border-width: 1px;
                overflow: hidden;
                word-break: normal;
                border-color: #aaa;
                color: #333;
                background-color: #fff;
            }

            .tg .tg-9ajh {
                text-align: left;
                font-size: 14px;
                background-color: #a9a9a9;
            }

        .topTR {
            text-align: center;
            padding-left: 10px;
            font-size: 20pt;
            padding: 20px;
            border-style: outset;
            border-color: cadetblue;
            background-color: #268CD8;
            color: white;
            font-weight: 600;
            border-radius: 5px;
            font-variant: all-petite-caps;
            font-weight: bold;
        }

        .lowerTR {
            text-align: center;
            padding-left: 10px;
            font-size: 12pt;
            padding: 15px;
            border-style: outset;
            border-color: cadetblue;
            background-color: #268CD8;
            color: white;
            border-radius: 5px;
            font-weight: bold;
        }

        .hyper {
            color: #268CD8;
            font-family: Calibri;
            font-size: 11.5pt;
            font-weight: bold;
            border: 2px solid #ccc;
            padding: 3px;
            text-decoration: none;
        }
    </style>
    <br>
    <br>
    <table style="border: 2px solid darkgray; border-radius: 6px;" class="tg">
        <tr>
            <th class="topTR" style="text-align: center; font-size: 18pt; background-color: #268CD8; color: white">BHAGYASHRI & CO.</th>
        </tr>
        <tr style="padding: 20px; font-size: 12pt;">
            <th style="padding-top: 20px; border: none">Dear Sir/Madam,</th>
        </tr>
        <tr style="padding: 20px; font-size: 12pt;">
            <th style="border: none">This task reminder was scheduled for you by Admin.</th>
        </tr>
        <tr style="padding: 20px; font-size: 12pt;">
            <th style="border: none">We"ll keep reminding you and Admin every 24 hours </th>
        </tr>
        <tr style="padding: 20px; font-size: 12pt;">
            <th style="border: none">unless completed or cancelled below.</th>
        </tr>
        <tr style="padding: 20px; font-size: 12pt;">
            <th style="padding-bottom: 25px; padding-top: 15px; border: none"><a class="hyper" href="192.168.5.59/CRMProject/CRM/CompleteTask.aspx?i=1">Complete</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a class="hyper" href="192.168.5.59/CRMProject/CRM/CancelTask.aspx?i=1">Cancel</a></th>
        </tr>
        <tr style="padding: 20px; padding-top: 30px; padding-bottom: 30px; font-size: 12pt; border-top: 2px solid #ccc">
            <th style="padding-top: 20px; border: none">Check out the remark for this followup given below.</th>
        </tr>
        <tr style="padding: 20px; padding-top: 30px; padding-bottom: 30px; font-size: 12pt;">
            <th style="padding-bottom: 20px; border: none">Please follow up with tomorrow"s schedule for customer meeting.</th>
        </tr>
        <tr>
            <th class="lowerTR" style="text-align: center; font-size: 12pt; background-color: #268CD8; color: white">This is system generated mail, please do not reply to this message via e-mail.</th>
        </tr>
    </table>
    <br>
</body>
</html>
