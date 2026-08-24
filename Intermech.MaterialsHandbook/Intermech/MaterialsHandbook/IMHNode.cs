// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.IMHNode
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System.Collections.Generic;

#nullable disable
namespace Intermech.MaterialsHandbook;

internal class IMHNode : IIMHNode
{
  public IMHNode(int parentCategoryID, int categoryID, List<long> tablerefIDs)
  {
    this.ParentCategoryID = parentCategoryID;
    this.CategoryID = categoryID;
    this.TableRefIDs = tablerefIDs;
  }

  public int CategoryID { get; private set; }

  public int ParentCategoryID { get; private set; }

  public List<long> TableRefIDs { get; private set; }
}
