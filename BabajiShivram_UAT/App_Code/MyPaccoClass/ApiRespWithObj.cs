using System;

namespace MyPacco.API
{
    /// <summary>
    /// Summary description for ApiRespWithObj
    /// </summary>
    public class ApiRespWithObj<T> : EventArgs
    {
        public bool IsSuccess { get; set; }

        public string TxnOutcome { get; set; }

        public string RawData { get; set; }

        public T RespObj { get; set; }
    }
}