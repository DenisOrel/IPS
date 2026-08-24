// Decompiled with JetBrains decompiler
// Type: Intermech.ProEngineer.Integrator.ProEngineerContentPattern
// Assembly: Intermech.ProEngineer.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 19987673-5EB5-4BB3-AE60-6A96614A14F3
// Assembly location: D:\IPS\Client\Intermech.ProEngineer.Integrator.dll

#nullable disable
namespace Intermech.ProEngineer.Integrator;

internal class ProEngineerContentPattern
{
  public long Ofs;
  public string Value;
  public ProEngineerFilePart Type;

  public ProEngineerContentPattern(long ofs, string value, ProEngineerFilePart type)
  {
    this.Ofs = ofs;
    this.Value = value;
    this.Type = type;
  }
}
