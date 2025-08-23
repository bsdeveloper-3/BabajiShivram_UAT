using System;

namespace MyPacco.API
{
    public class MyPaccoRespWithObj<T> : EventArgs
    {
        public bool IsSuccess { get; set; }

        public string TxnOutcome { get; set; }

        public string RawData { get; set; }

        public T RespObj { get; set; }
    }
}