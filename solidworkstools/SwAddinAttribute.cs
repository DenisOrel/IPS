// Decompiled with JetBrains decompiler
// Type: SolidWorksTools.SwAddinAttribute
// Assembly: SolidWorksTools, Version=2.0.0.0, Culture=neutral, PublicKeyToken=bd18593873b4686d
// MVID: 863FC724-66C1-47FF-B7E4-FE091B230BC6
// Assembly location: D:\Projects\IPS Code\IPS\CADSystem\CAD\SolidWorks\Bin\solidworkstools.dll

using System;

#nullable disable
namespace SolidWorksTools;

[AttributeUsage(AttributeTargets.Class)]
public class SwAddinAttribute : Attribute
{
  public string Description = "";
  public string Title = "";
  public bool LoadAtStartup;
}
