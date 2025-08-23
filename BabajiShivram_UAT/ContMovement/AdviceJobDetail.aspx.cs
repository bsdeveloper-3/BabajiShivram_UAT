using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using System.Configuration;
using Ionic.Zip;

public partial class ContMovement_AdviceJobDetail : System.Web.UI.Page
{
    LoginClass loggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(imgbtnDocuments);
        Page.ClientScript.RegisterOnSubmitStatement(this.GetType(), "val", "validateAndHighlight();");

        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Container Received Detail";
        if (!IsPostBack)
        {
            if (Convert.ToString(Session["JobId"]) != null)
            {
                GetData(Convert.ToInt32(Session["JobId"]));
            }
        }
    }

    protected void GetData(int JobId)
    {
        if (JobId > 0)
        {
            DataSet dsGetJobDetail = CMOperations.GetJobDetailByLid(JobId);
            if (dsGetJobDetail != null && dsGetJobDetail.Tables[0].Rows.Count > 0)
            {
                if (dsGetJobDetail.Tables[0].Rows[0]["lid"].ToString() != "")
                {
                    if (dsGetJobDetail.Tables[0].Rows[0]["lid"] != DBNull.Value)
                    {
                        hdnJobId.Value = Convert.ToString(dsGetJobDetail.Tables[0].Rows[0]["lid"]);
                    }
                    lblCFSName.Text = dsGetJobDetail.Tables[0].Rows[0]["CFSName"].ToString();
                    if (dsGetJobDetail.Tables[0].Rows[0]["MovementCompDate"] != DBNull.Value)
                    {
                        lblMovementCompDate.Text = Convert.ToDateTime(dsGetJobDetail.Tables[0].Rows[0]["MovementCompDate"]).ToString("dd/MM/yyyy");
                    }
                    if (dsGetJobDetail.Tables[0].Rows[0]["EmptyContReturnDate"] != DBNull.Value)
                    {
                        lblEmptyContReturnDate.Text = Convert.ToDateTime(dsGetJobDetail.Tables[0].Rows[0]["EmptyContReturnDate"]).ToString("dd/MM/yyyy");
                    }
                    if (dsGetJobDetail.Tables[0].Rows[0]["ContCFSReceivedDate"] != DBNull.Value)
                    {
                        lblContCFSReceivedDate.Text = Convert.ToDateTime(dsGetJobDetail.Tables[0].Rows[0]["ContCFSReceivedDate"]).ToString("dd/MM/yyyy");
                    }

                    lblJobRefNo.Text = dsGetJobDetail.Tables[0].Rows[0]["JobRefNo"].ToString();
                    lblCustName.Text = dsGetJobDetail.Tables[0].Rows[0]["Customer"].ToString();
                    lblConsigneeName.Text = dsGetJobDetail.Tables[0].Rows[0]["ConsigneeName"].ToString();
                    if (dsGetJobDetail.Tables[0].Rows[0]["ETADate"] != DBNull.Value)
                    {
                        lblETADate.Text = Convert.ToDateTime(dsGetJobDetail.Tables[0].Rows[0]["ETADate"]).ToString("dd/MM/yyyy");
                    }
                    lblBranch.Text = dsGetJobDetail.Tables[0].Rows[0]["BranchName"].ToString();
                    if (dsGetJobDetail.Tables[0].Rows[0]["SumOf20"] != DBNull.Value)
                    {
                        lblSumof20.Text = dsGetJobDetail.Tables[0].Rows[0]["SumOf20"].ToString();
                    }
                    else
                    {
                        lblSumof20.Text = "0";
                    }
                    if (dsGetJobDetail.Tables[0].Rows[0]["SumOf40"] != DBNull.Value)
                    {
                        lblSumof40.Text = dsGetJobDetail.Tables[0].Rows[0]["SumOf40"].ToString();
                    }
                    else
                    {
                        lblSumof40.Text = "0";
                    }
                    lblContType.Text = dsGetJobDetail.Tables[0].Rows[0]["ContainerType"].ToString();
                    lblJobCreationDate.Text = Convert.ToDateTime(dsGetJobDetail.Tables[0].Rows[0]["JobCreationDate"]).ToString("dd/MM/yyyy");
                    lblJobCreatedBy.Text = dsGetJobDetail.Tables[0].Rows[0]["CreatedBy"].ToString();
                    lblShippingName.Text = dsGetJobDetail.Tables[0].Rows[0]["ShippingName"].ToString();
                }
            }
        }
        else
        {
            lblError.Text = "No data found.";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void imgbtnDocuments_Click(object sender, ImageClickEventArgs e)
    {
        string FilePath = "";
        int JobId = 0;
        if (hdnJobId.Value != "" && hdnJobId.Value != "0")
            JobId = Convert.ToInt32(hdnJobId.Value);

        if (JobId > 0)
        {
            String ServerPath = FileServer.GetFileServerDir();
            using (ZipFile zip = new ZipFile())
            {
                zip.AddDirectoryByName("MovementFiles");
                DataSet dsGetDoc = CMOperations.GetDocuments(JobId);
                if (dsGetDoc != null)
                {
                    for (int i = 0; i < dsGetDoc.Tables[0].Rows.Count; i++)
                    {
                        if (dsGetDoc.Tables[0].Rows[i]["DocPath"].ToString() != "")
                        {
                            if (ServerPath == "")
                            {
                                FilePath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\PNMovement\\" + dsGetDoc.Tables[0].Rows[i]["DocPath"].ToString());
                            }
                            else
                            {
                                FilePath = ServerPath + "PNMovement\\" + dsGetDoc.Tables[0].Rows[i]["DocPath"].ToString();
                            }
                            zip.AddFile(FilePath, "MovementFiles");
                        }
                    }

                    Response.Clear();
                    Response.BufferOutput = false;
                    string zipName = String.Format("MovementZip_{0}.zip", DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"));
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                    zip.Save(Response.OutputStream);
                    Response.End();
                }
            }
        }
        else
        {
            lblError.Text = "No document found for this job.";
            lblError.CssClass = "errorMsg";
        }
    }
}