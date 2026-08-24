// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.TrvParentNode
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

#nullable disable
namespace Intermech.MaterialsHandbook;

internal class TrvParentNode
{
  internal string Purpose { get; set; }

  internal string Instructions { get; set; }

  internal TrvParentNode(string purpose, string instructions)
  {
    this.Purpose = purpose;
    this.Instructions = instructions;
  }
}
