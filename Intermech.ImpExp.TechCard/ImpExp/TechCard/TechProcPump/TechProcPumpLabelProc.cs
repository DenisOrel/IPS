// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.TechProcPumpLabelProc
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump;

internal class TechProcPumpLabelProc
{
  private static long _lblCount = 0;
  public static long Interval = 5;

  public static void LoadLabel(ref TechProcPumpLabel label)
  {
    string path = Path.GetDirectoryName(Application.ExecutablePath) + TechProcPumpConst.TechProcFileLabelFileName;
    if (!File.Exists(path))
      return;
    FileStream serializationStream = File.OpenRead(path);
    BinaryFormatter binaryFormatter = new BinaryFormatter();
    label = (TechProcPumpLabel) binaryFormatter.Deserialize((Stream) serializationStream);
    serializationStream.Close();
  }

  public static void SaveLabel(TechProcPumpLabel label)
  {
    ++TechProcPumpLabelProc._lblCount;
    if (TechProcPumpLabelProc._lblCount < TechProcPumpLabelProc.Interval)
      return;
    FileStream serializationStream = File.Create(Path.GetDirectoryName(Application.ExecutablePath) + TechProcPumpConst.TechProcFileLabelFileName);
    new BinaryFormatter().Serialize((Stream) serializationStream, (object) label);
    serializationStream.Close();
    TechProcPumpLabelProc._lblCount = 0L;
  }
}
