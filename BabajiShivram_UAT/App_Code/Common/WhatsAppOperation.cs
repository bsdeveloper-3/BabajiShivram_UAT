using System;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for WhatsAppOperation
/// </summary>
public class WhatsAppOperation
{
    public WhatsAppOperation()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static int AddWhatsAppMessage(int EventTypeId, string SenderPhone, string SenderName, int SenderType,string SentTimeStamp,
      string MessageUID, string MesssageCUID, int MesssageDirectionId, int MessageTypeId,string MessageBODY, int AcknowledgeID)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EventTypeId", SqlDbType.Int).Value = EventTypeId;
        command.Parameters.Add("@SenderPhone", SqlDbType.NVarChar).Value = SenderPhone;
        command.Parameters.Add("@SenderName", SqlDbType.NVarChar).Value = SenderName;
        command.Parameters.Add("@SenderType", SqlDbType.Int).Value = SenderType;
        command.Parameters.Add("@SentTimeStamp", SqlDbType.NVarChar).Value = SentTimeStamp; 
        command.Parameters.Add("@MessageUID", SqlDbType.NVarChar).Value = MessageUID;
        command.Parameters.Add("@MesssageCUID", SqlDbType.NVarChar).Value = MesssageCUID;
        command.Parameters.Add("@MesssageDirectionId", SqlDbType.Int).Value = MesssageDirectionId;
        command.Parameters.Add("@MessageTypeId", SqlDbType.Int).Value = MessageTypeId;
        command.Parameters.Add("@MessageBODY", SqlDbType.NVarChar).Value = MessageBODY;
        command.Parameters.Add("@AcknowledgeID", SqlDbType.Int).Value = AcknowledgeID;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("WA_insMessageLog", command, "@OutPut");
        return Convert.ToInt32(SPresult);

    }

    public static int AddWhatsAppAcknowledg(int EventTypeId, string MessageUID, string strMessageMUID, string MesssageCUID, int AcknowledgeID)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EventTypeId", SqlDbType.Int).Value = EventTypeId;
        command.Parameters.Add("@MessageUID", SqlDbType.NVarChar).Value = MessageUID;
        command.Parameters.Add("@MessageMUID", SqlDbType.NVarChar).Value = strMessageMUID;
        command.Parameters.Add("@MesssageCUID", SqlDbType.NVarChar).Value = MesssageCUID;
        command.Parameters.Add("@AcknowledgeID", SqlDbType.Int).Value = AcknowledgeID;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("WA_insMessageAck", command, "@OutPut");
        return Convert.ToInt32(SPresult);

    }
    public static DataSet GetWhatsAppSender()
    {
        SqlCommand command = new SqlCommand();

        return CDatabase.GetDataSet("WA_GetSender", command);
    }
    public static DataSet GetWhatsAppChatBySender(string SenderPhone)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@SenderPhone", SqlDbType.NVarChar).Value = SenderPhone;

        return CDatabase.GetDataSet("WA_GetChatBySender", command);
    }
}