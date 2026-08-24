// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.IIMHNode
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System.Collections.Generic;

#nullable disable
namespace Intermech.MaterialsHandbook;

public interface IIMHNode
{
  int CategoryID { get; }

  int ParentCategoryID { get; }

  List<long> TableRefIDs { get; }
}
