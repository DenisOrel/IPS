// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.StArrayTypeInfo
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;

#nullable disable
namespace Intermech.Serialization;

public sealed class StArrayTypeInfo : StTypeInfo
{
  public StArrayTypeInfo(
    string typeName,
    string assemblyName,
    StTypeInfo template,
    StTypeInfo elementInfo,
    int rank)
    : base(typeName, assemblyName)
  {
    if (template == null)
      throw new ArgumentNullException(nameof (template));
    if (elementInfo == null)
      throw new ArgumentNullException(nameof (elementInfo));
    if (rank < 0)
      throw new ArgumentOutOfRangeException(nameof (rank));
    this.Template = template;
    this.ElementInfo = elementInfo;
    this.Rank = rank;
  }

  public StArrayTypeInfo(StTypeInfo template, StTypeInfo elementInfo, int rank)
    : base(string.Empty, string.Empty)
  {
    if (template == null)
      throw new ArgumentNullException(nameof (template));
    if (elementInfo == null)
      throw new ArgumentNullException(nameof (elementInfo));
    if (rank <= 0)
      throw new ArgumentOutOfRangeException(nameof (rank));
    this.assemblyName = elementInfo.AssemblyName;
    this.typeName = rank == 1 ? elementInfo.TypeName + "[]" : $"{elementInfo.TypeName}[{new string(',', rank - 1)}]";
    this.Template = template;
    this.ElementInfo = elementInfo;
    this.Rank = rank;
  }

  public StTypeInfo Template { get; }

  public StTypeInfo ElementInfo { get; }

  public int Rank { get; }
}
