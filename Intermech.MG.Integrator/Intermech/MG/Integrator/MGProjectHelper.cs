// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.MGProjectHelper
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using System.IO;

#nullable disable
namespace Intermech.MG.Integrator;

internal static class MGProjectHelper
{
  public static MGProjectType DefineProjectType(Stream stream, out string pcbPath)
  {
    TextReader textReader = (TextReader) new StreamReader(stream);
    pcbPath = (string) null;
    bool flag1 = false;
    bool flag2 = false;
    while (textReader.Peek() > 0)
    {
      string str = textReader.ReadLine();
      if (!string.IsNullOrEmpty(str))
      {
        if (flag1)
        {
          if (str.StartsWith("KEY VBPCBDesignPath"))
          {
            int num1 = str.IndexOf('"');
            int num2 = str.LastIndexOf('"');
            pcbPath = str.Substring(num1 + 1, num2 - num1 - 1);
          }
          else if (str.StartsWith("KEY VBPCBForeignNetlist"))
            flag2 = true;
          else if (str.Equals("ENDSECTION"))
            break;
        }
        else if (str.Equals("SECTION VBPCBDesignData"))
          flag1 = true;
      }
    }
    return !flag2 || string.IsNullOrEmpty(pcbPath) ? MGProjectType.Own : MGProjectType.Foreign;
  }
}
