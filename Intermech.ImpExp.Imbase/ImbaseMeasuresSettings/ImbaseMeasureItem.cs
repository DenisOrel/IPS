// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.ImbaseMeasuresSettings.ImbaseMeasureItem
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

#nullable disable
namespace Intermech.ImpExp.Imbase.ImbaseMeasuresSettings;

internal class ImbaseMeasureItem
{
  protected string name = string.Empty;
  protected long measureID;

  public string Name
  {
    get => this.name;
    set
    {
      if (this.name.Equals(value))
        return;
      this.name = value;
    }
  }

  public long MeasureID
  {
    get => this.measureID;
    set
    {
      if (this.measureID == value)
        return;
      this.measureID = value;
    }
  }

  public ImbaseMeasureItem(string measureName) => this.name = measureName;

  public override string ToString() => this.Name;
}
