// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareCompositionViewColumnsCache
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Navigator.Interfaces;

#nullable disable
namespace Intermech.Pdm.Compositions;

internal sealed class CompareCompositionViewColumnsCache
{
  public int ObjectTypeID { get; private set; }

  public NodeColumnCollection Columns { get; set; }

  public CompareCompositionViewColumnsCache(int objectTypeID, NodeColumnCollection columns)
  {
    this.ObjectTypeID = objectTypeID;
    this.Columns = columns;
  }

  public static CompareCompositionViewColumnsCache Instance { get; set; }
}
