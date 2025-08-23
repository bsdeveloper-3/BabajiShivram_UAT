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
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;

public partial class CRM_CompleteTask : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["i"] != null)
            {
                int FollowupId = Convert.ToInt32(Request.QueryString["i"]);
                if (FollowupId > 0)
                {
                    int lUser = 0;
                    DataSet dsGetFollowupHistory = DBOperations.CRM_GetFollowupHistoryByLId(FollowupId);
                    if (dsGetFollowupHistory != null)
                    {
                        lUser = Convert.ToInt32(dsGetFollowupHistory.Tables[0].Rows[0]["lUser"].ToString());
                    }
                    int result = DBOperations.CRM_UpdFollowupStatusHistory(FollowupId, 1, lUser);
                    lblMessage.Text = "Task Completed Successfully! Thank You.";
                }
            }
        }
    }
}