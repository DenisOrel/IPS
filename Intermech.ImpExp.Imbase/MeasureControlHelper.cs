// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.MeasureControlHelper
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Interface.CommonData;
using Intermech.Interfaces.Client;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Imbase;

internal static class MeasureControlHelper
{
  public static bool CheckMeasureShortName(string shortName)
  {
    IMeasureItem measure = (ServicesManager.GetService(typeof (IMeasures)) as IMeasures).GetMeasure(shortName);
    if (measure == null)
      return true;
    int num = (int) MessageBox.Show($"Единица измерения \"{measure.LongName}\" с кратким именем \"{measure.ShortName}\" уже существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
    return false;
  }
}
