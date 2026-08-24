// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.ElementQuantity
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;

#nullable disable
namespace Intermech.Pdm;

internal sealed class ElementQuantity
{
  private static bool _MeasureHelperInit;
  public string Caption = string.Empty;
  public int ObjectType = -1;
  public MeasuredValue DesignQuantity;
  public MeasuredValue TechQuantity;

  public ElementQuantity()
  {
  }

  public ElementQuantity(string captionValue, int typeID, string designValue, string techValue)
  {
    this.Caption = captionValue;
    this.ObjectType = typeID;
    if (!ElementQuantity._MeasureHelperInit)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
        MeasureHelper.Init(sessionKeeper.Session.GetMeasuresList());
      ElementQuantity._MeasureHelperInit = true;
    }
    if (designValue != string.Empty)
      this.DesignQuantity = MeasureHelper.ConvertToBaseMeasure(MeasureHelper.ConvertToMeasuredValue(designValue));
    if (!(techValue != string.Empty))
      return;
    this.TechQuantity = MeasureHelper.ConvertToBaseMeasure(MeasureHelper.ConvertToMeasuredValue(techValue));
  }
}
