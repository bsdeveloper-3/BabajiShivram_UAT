using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MyPaccoRespErrPl
/// </summary>
namespace MyPacco.API
{
public class MyPaccoRespErrPl
{
    public string status_cd { get; set; }

    public MyPaccoRespErrCodePl error { get; set; }

}
}