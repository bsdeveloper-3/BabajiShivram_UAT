using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for JobStageType
/// </summary>

public enum JobStageType
    {
	    NecessaryDocsAwaited = 1,
        TruckAwaited =2,
        UnderNoting =3,
        UnderPassing =4,
        VesselAwaited =5,
        DutyAwaited = 6,
        DOPending = 7,
        OBLAwaited = 8,
        
    }
