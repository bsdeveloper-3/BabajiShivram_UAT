using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Collections.Generic;

public partial class Transport_SuccessPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Success Page";

        Session["TRConsolidateId"] = null;
        Session["TRId"] = null;
        Session["TransBillId"] = null;

        if (!IsPostBack)
        {
            if (Request.QueryString["Request"] != null)
            {
                SendApprovalMail(Convert.ToInt32(Request.QueryString["Request"]));
                //lblMessage.Text = "Successfully added truck request  <b><u>" + Request.QueryString["Request"].ToString() + "</b></u>. Request moved to request received tab.";
            }
            else if (Request.QueryString["Bill"] != null)
            {
                lblMessage.Text = "Bill Detail Added Successfully. Truck Request Moved to Bill Received Tab.";
            }
            else if (Request.QueryString["Expense"] != null)
            {
                lblMessage.Text = "Successfully added vehicle expense.";
            }
            else if (Request.QueryString["NewTransport"] != null)
            {
                lblMessage.Text = "Transport Details Added Successfully.";
            }
        }
    }

    protected void SendApprovalMail(int TransReqId)
    {
        bool bEmailSuccess = false;
        string EmailContent = "", MessageBody = "", Subject = "", TrRefNo = "", BabajiJobNo = "", strFileName = "", strMailCC = "", strMailTo = "";
        StringBuilder strbuilder = new StringBuilder();
        List<string> lstFileDoc = new List<string>();

        if (TransReqId > 0)
        {
            DataView dvTransRequest = DBOperations.GetTransportRequestDetail(TransReqId);
            if (dvTransRequest != null && dvTransRequest.Table.Rows[0]["lId"] != DBNull.Value)
            {
                TrRefNo = dvTransRequest.Table.Rows[0]["TRRefNo"].ToString();
                BabajiJobNo = dvTransRequest.Table.Rows[0]["JobRefNo"].ToString();
                //strMailTo = "tp.helpdesk@babajishivram.com";
                strMailTo = dvTransRequest.Table.Rows[0]["MailTo"].ToString(); //"tp.helpdesk@babajishivram.com";
                //strMailCC = "javed.shaikh@babajishivram.com , " + dvTransRequest.Table.Rows[0]["CreatedByMail"].ToString();
                strMailCC = dvTransRequest.Table.Rows[0]["CreatedByMail"].ToString();
                Subject = "New Truck Request " + TrRefNo + " for Babaji Job " + BabajiJobNo;

                if (Convert.ToInt32(dvTransRequest.Table.Rows[0]["ImportExportType"]) == 1)     // Import Type
                {
                    try
                    {
                        strFileName = "../EmailTemplate/TruckRequest.txt";
                        StreamReader sr = new StreamReader(Server.MapPath(strFileName));
                        sr = File.OpenText(Server.MapPath(strFileName));
                        EmailContent = sr.ReadToEnd();
                        sr.Close();
                        sr.Dispose();
                        GC.Collect();
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = ex.Message;
                        lblMessage.CssClass = "errorMsg";
                        return;
                    }

                    MessageBody = EmailContent.Replace("@TRRefNo", dvTransRequest.Table.Rows[0]["TRRefNo"].ToString());
                    MessageBody = MessageBody.Replace("@BabajiJobNo", dvTransRequest.Table.Rows[0]["JobRefNo"].ToString());
                    MessageBody = MessageBody.Replace("@CustName", dvTransRequest.Table.Rows[0]["CustName"].ToString());
                    MessageBody = MessageBody.Replace("@Consignee", dvTransRequest.Table.Rows[0]["ConsigneeName"].ToString());
                    MessageBody = MessageBody.Replace("@Branch", dvTransRequest.Table.Rows[0]["Branch"].ToString());
                    MessageBody = MessageBody.Replace("@Port", dvTransRequest.Table.Rows[0]["Port"].ToString());
                    MessageBody = MessageBody.Replace("@NoofConts", dvTransRequest.Table.Rows[0]["TotalNoContainer"].ToString());
                    MessageBody = MessageBody.Replace("@NoofPackages", dvTransRequest.Table.Rows[0]["NoOfPackages"].ToString());
                    MessageBody = MessageBody.Replace("@Sum20", dvTransRequest.Table.Rows[0]["CountOf20"].ToString());
                    MessageBody = MessageBody.Replace("@Sum40", dvTransRequest.Table.Rows[0]["CountOf40"].ToString());
                    MessageBody = MessageBody.Replace("@GrossWeight", dvTransRequest.Table.Rows[0]["GrossWT"].ToString());
                    MessageBody = MessageBody.Replace("@BOEType", dvTransRequest.Table.Rows[0]["BOEType"].ToString());
                    MessageBody = MessageBody.Replace("@DeliveryType", dvTransRequest.Table.Rows[0]["DeliveryType"].ToString());
                    MessageBody = MessageBody.Replace("@Destination", dvTransRequest.Table.Rows[0]["Destination"].ToString());
                    MessageBody = MessageBody.Replace("@Dimension", dvTransRequest.Table.Rows[0]["Dimension"].ToString());
                    MessageBody = MessageBody.Replace("@VehiclePlaceDate", Convert.ToDateTime(dvTransRequest.Table.Rows[0]["VehiclePlaceDate"]).ToString("dd/MM/yyyy"));
                    MessageBody = MessageBody.Replace("@EmpName", dvTransRequest.Table.Rows[0]["CreatedBy"].ToString());
                }
                else
                {
                    try
                    {
                        strFileName = "../EmailTemplate/ExpTruckRequest.txt";
                        StreamReader sr = new StreamReader(Server.MapPath(strFileName));
                        sr = File.OpenText(Server.MapPath(strFileName));
                        EmailContent = sr.ReadToEnd();
                        sr.Close();
                        sr.Dispose();
                        GC.Collect();
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = ex.Message;
                        lblMessage.CssClass = "errorMsg";
                        return;
                    }

                    MessageBody = EmailContent.Replace("@TRRefNo", dvTransRequest.Table.Rows[0]["TRRefNo"].ToString());
                    MessageBody = MessageBody.Replace("@BabajiJobNo", dvTransRequest.Table.Rows[0]["JobRefNo"].ToString());
                    MessageBody = MessageBody.Replace("@CustName", dvTransRequest.Table.Rows[0]["CustName"].ToString());
                    MessageBody = MessageBody.Replace("@Shipper", dvTransRequest.Table.Rows[0]["ShipperName"].ToString());
                    MessageBody = MessageBody.Replace("@Branch", dvTransRequest.Table.Rows[0]["Branch"].ToString());
                    MessageBody = MessageBody.Replace("@Port", dvTransRequest.Table.Rows[0]["Port"].ToString());
                    MessageBody = MessageBody.Replace("@NoofConts", dvTransRequest.Table.Rows[0]["TotalNoContainer"].ToString());
                    MessageBody = MessageBody.Replace("@NoofPackages", dvTransRequest.Table.Rows[0]["NoOfPackages"].ToString());
                    MessageBody = MessageBody.Replace("@Sum20", dvTransRequest.Table.Rows[0]["CountOf20"].ToString());
                    MessageBody = MessageBody.Replace("@Sum40", dvTransRequest.Table.Rows[0]["CountOf40"].ToString());
                    MessageBody = MessageBody.Replace("@GrossWeight", dvTransRequest.Table.Rows[0]["GrossWT"].ToString());
                    MessageBody = MessageBody.Replace("@ShippingBillType", dvTransRequest.Table.Rows[0]["ShippingBillType"].ToString());
                    MessageBody = MessageBody.Replace("@ExportType", dvTransRequest.Table.Rows[0]["ExportTypeName"].ToString());
                    MessageBody = MessageBody.Replace("@Destination", dvTransRequest.Table.Rows[0]["Destination"].ToString());
                    MessageBody = MessageBody.Replace("@Dimension", dvTransRequest.Table.Rows[0]["Dimension"].ToString());
                    MessageBody = MessageBody.Replace("@VehiclePlaceDate", Convert.ToDateTime(dvTransRequest.Table.Rows[0]["VehiclePlaceDate"]).ToString("dd/MM/yyyy"));
                    MessageBody = MessageBody.Replace("@EmpName", dvTransRequest.Table.Rows[0]["CreatedBy"].ToString());
                }

                DataSet dsGetDoc = DBOperations.GetPackingListDocs(TransReqId);
                if (dsGetDoc != null && dsGetDoc.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsGetDoc.Tables[0].Rows.Count; i++)
                    {
                        if (dsGetDoc.Tables[0].Rows[i]["DocPath"].ToString() != "")
                        {
                            lstFileDoc.Add("Transport\\" + dsGetDoc.Tables[0].Rows[i]["DocPath"].ToString());
                        }
                    }
                }

                //bEmailSuccess = EMail.SendMailMultiAttach(lblCustomerEmail.Text.Trim(), lblCustomerEmail.Text.Trim(), txtMailCC.Text.Trim(), txtSubject.Text.Trim(), MessageBody, lstFileDoc);
                if (strMailTo != "")
                {
                    strMailCC = strMailCC + "," + "airtp@babajishivram.com";

                    bEmailSuccess = EMail.SendMailMultiAttach2(strMailTo, strMailTo, strMailCC, Subject, MessageBody, lstFileDoc);
                    
                    if (bEmailSuccess == true)
                    {
                        lblMessage.Text = "Successfully added truck request  <b><u>" + TrRefNo + "</b></u>. Request moved to request received tab.";
                        lblMessage.CssClass = "success";
                    }
                    else
                    {
                        lblMessage.Text = "Error while sending mail. Please try again later!";
                        lblMessage.CssClass = "errorMsg";
                    }
                }
            }
        }
    }
}