using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[Serializable]
public class FreightChat
{
    public string lid { get; set; }
    public string Message { get; set; }
    public string MessageToName { get; set; }
    public string MessageTo_UserId { get; set; }
    public string CurrentStatusId { get; set; }
    public string CurrentStatus { get; set; }
    public string FormattedDate { get; set; }
    public DateTime Date { get; set; }
    public string PersonNaming { get; set; }
    public string MessageTime { get; set; }

    public string UserName { get; set; }
    public string lUser { get; set; }
    public string UserLastLoginDate { get; set; }
    public string IsAvailable { get; set; }
    public string TotalMsgs { get; set; }
}