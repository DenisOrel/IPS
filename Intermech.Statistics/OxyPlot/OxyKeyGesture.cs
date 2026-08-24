// Decompiled with JetBrains decompiler
// Type: OxyPlot.OxyKeyGesture
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot;

public class OxyKeyGesture : OxyInputGesture
{
  public OxyKeyGesture(OxyKey key, OxyModifierKeys modifiers = OxyModifierKeys.None)
  {
    this.Key = key;
    this.Modifiers = modifiers;
  }

  public OxyModifierKeys Modifiers { get; set; }

  public OxyKey Key { get; set; }

  public override bool Equals(OxyInputGesture other)
  {
    return other is OxyKeyGesture oxyKeyGesture && oxyKeyGesture.Modifiers == this.Modifiers && oxyKeyGesture.Key == this.Key;
  }
}
