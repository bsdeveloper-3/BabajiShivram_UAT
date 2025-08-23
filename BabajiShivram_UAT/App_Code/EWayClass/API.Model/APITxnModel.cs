// Decompiled with JetBrains decompiler
// Type: TaxProEWB.API.TxnRespWithObj`1
// Assembly: TaxProEWB.API, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 651C2957-9A00-43E1-9864-7C8CEF88DD73
// Assembly location: C:\inetpub\wwwroot\TaxProEWBApiIntigrationDemo.NET\bin\x86\Debug\TaxProEWB.API.dll

using System;

namespace TaxProEWB.API
{
  public class TxnRespWithObj<T> : EventArgs
  {
    public bool IsSuccess { get; set; }

    public string TxnOutcome { get; set; }

    public string RawData { get; set; }
    public T RespObj { get; set; }
  }
}
