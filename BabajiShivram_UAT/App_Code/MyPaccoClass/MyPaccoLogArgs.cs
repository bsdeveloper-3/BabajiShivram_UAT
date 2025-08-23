using System;
namespace MyPacco.API
{
    public class MyPaccoLogArgs
    {
        public int Id { get; set; }

        public DateTime TxnDateTime { get; set; }

        public bool IsSuccess { get; set; }

        public string ApiAction { get; set; }

        public string ErrCode { get; set; }

        public string OutcomeMsg { get; set; }

        public string AppUserName { get; set; }
    }
}