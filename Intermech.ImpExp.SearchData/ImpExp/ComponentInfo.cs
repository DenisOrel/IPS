// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.ComponentInfo
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

#nullable disable
namespace Intermech.ImpExp;

public class ComponentInfo
{
  public string Name;
  public string Assembly;

  public ComponentInfo(string name, string assembly)
  {
    this.Name = name;
    if (assembly == "")
      this.Assembly = FormConverter.asmImCore;
    else
      this.Assembly = assembly;
  }

  public ComponentInfo(string name)
    : this(name, "")
  {
  }
}
