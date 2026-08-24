// Decompiled with JetBrains decompiler
// Type: OxyPlot.OxyMouseEnterGesture
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot;

public class OxyMouseEnterGesture : OxyInputGesture
{
  public OxyMouseEnterGesture(OxyModifierKeys modifiers = OxyModifierKeys.None)
  {
    this.Modifiers = modifiers;
  }

  public OxyModifierKeys Modifiers { get; private set; }

  public override bool Equals(OxyInputGesture other)
  {
    return other is OxyMouseEnterGesture mouseEnterGesture && mouseEnterGesture.Modifiers == this.Modifiers;
  }
}
