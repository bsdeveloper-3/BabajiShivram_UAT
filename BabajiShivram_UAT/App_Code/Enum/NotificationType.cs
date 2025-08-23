using System;

/// <summary>
/// Summary description for NotificationType
/// </summary>
public enum NotificationType
{
	PendingDocument = 1,
    ChecklistApproval = 2,
    DutyRequest = 3,
    DispatchDetail = 4,
    TruckRequest = 5,
    Noting = 6,
    PCADispatch = 7,
    BillingDispatch = 8,
    CustomQuery = 9,
    CustomQueryResolve = 10,
}

public enum NotificationMode
{
    Email = 1,
    SMS  =2,
}