// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.FolderNodeID
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Navigator.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class FolderNodeID : INodeID
{
  public string Caption { get; }

  public string ClassifFolderKey { get; }

  public long ObjectVersionID { get; }

  public Dictionary<long, string> TableRefs { get; set; }

  public FolderNodeID(int objTypeID, long objID, string caption, string classifFolderKey)
  {
    this.TypeID = objTypeID;
    this.ObjectVersionID = objID;
    this.Caption = caption;
    this.ClassifFolderKey = classifFolderKey;
  }

  public int CategoryID
  {
    [DebuggerStepThrough] get => 1;
  }

  public int TypeID { get; }

  public object Cookie { get; set; }
}
