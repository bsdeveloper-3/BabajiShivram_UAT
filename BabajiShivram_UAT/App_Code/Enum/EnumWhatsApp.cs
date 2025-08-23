using System;

/// <summary>
/// Summary description for EnumWhatsApp
/// </summary>
/// 

public enum WhatsAppEventType
{
    Message =1, 
    Acknowledge =2
}
public enum WhatsAppSenderType
{
    User = 1,
    Group = 2
}
public enum WhatsAppMessageDirection
{
    Input = 1,
    Ouput = 2
}

public enum WhatsAppMessageType
{
    Chat =1, Image = 2, Video = 3, Audio =4,
    Document = 5, vCard = 6, Location = 7,  Other =10
}

public enum WhatsAppAckType
{
    NotSentToServer = 0,  
    SentToServer = 1,  
    DeliveredToRecipient =2, 
    ReadReceipt = 3
}
