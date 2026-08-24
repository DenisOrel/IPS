// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.VisDialogs.StyleData
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Pdm;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Pdm.VisDialogs;

public class StyleData
{
  private Dictionary<int, ObjNodeData> objStyles;
  private Dictionary<int, PreviewNodeData> prevStyles;
  private Dictionary<int, LinkNodeData> linkStyles;
  private static readonly ObjNodeData defObjNodeData = new ObjNodeData();
  private static readonly PreviewNodeData defPrevNodeData = new PreviewNodeData();
  private static readonly LinkNodeData defLinkNodeData = new LinkNodeData();

  public StyleData()
  {
    this.objStyles = new Dictionary<int, ObjNodeData>();
    this.prevStyles = new Dictionary<int, PreviewNodeData>();
    this.linkStyles = new Dictionary<int, LinkNodeData>();
  }

  public void AddStyleNode(VisStyleNode vsn)
  {
    switch (vsn.Kind)
    {
      case StyleKind.CommonObject:
        if (vsn.CatList.Count == 0)
        {
          if (this.objStyles.ContainsKey(-1))
            break;
          this.objStyles.Add(-1, vsn.Data as ObjNodeData);
          break;
        }
        using (List<GlobalType>.Enumerator enumerator = vsn.CatList.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            GlobalType current = enumerator.Current;
            if (this.objStyles.ContainsKey(current.TypeID))
              this.objStyles[current.TypeID] = vsn.Data as ObjNodeData;
            else
              this.objStyles.Add(current.TypeID, vsn.Data as ObjNodeData);
          }
          break;
        }
      case StyleKind.ObjPreview:
        if (vsn.CatList.Count == 0)
        {
          if (this.prevStyles.ContainsKey(-1))
            break;
          this.prevStyles.Add(-1, vsn.Data as PreviewNodeData);
          break;
        }
        using (List<GlobalType>.Enumerator enumerator = vsn.CatList.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            GlobalType current = enumerator.Current;
            if (this.prevStyles.ContainsKey(current.TypeID))
              this.prevStyles[current.TypeID] = vsn.Data as PreviewNodeData;
            else
              this.prevStyles.Add(current.TypeID, vsn.Data as PreviewNodeData);
          }
          break;
        }
      case StyleKind.Relation:
        if (vsn.CatList.Count == 0)
        {
          if (this.linkStyles.ContainsKey(-1))
            break;
          this.linkStyles.Add(-1, vsn.Data as LinkNodeData);
          break;
        }
        using (List<GlobalType>.Enumerator enumerator = vsn.CatList.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            GlobalType current = enumerator.Current;
            if (this.linkStyles.ContainsKey(current.TypeID))
              this.linkStyles[current.TypeID] = vsn.Data as LinkNodeData;
            else
              this.linkStyles.Add(current.TypeID, vsn.Data as LinkNodeData);
          }
          break;
        }
    }
  }

  public ObjNodeData GetObjectStyle(int objType)
  {
    if (this.objStyles.ContainsKey(objType))
      return this.objStyles[objType];
    List<int> objectTypeParentsId = MetaDataHelper.GetObjectTypeParentsID(objType);
    objectTypeParentsId.Add(-1);
    foreach (int key in objectTypeParentsId)
    {
      if (this.objStyles.ContainsKey(key))
        return this.objStyles[key];
    }
    return StyleData.defObjNodeData;
  }

  public PreviewNodeData GetPreviewStyle(int objType)
  {
    if (this.prevStyles.ContainsKey(objType))
      return this.prevStyles[objType];
    List<int> objectTypeParentsId = MetaDataHelper.GetObjectTypeParentsID(objType);
    objectTypeParentsId.Add(-1);
    foreach (int key in objectTypeParentsId)
    {
      if (this.prevStyles.ContainsKey(key))
        return this.prevStyles[key];
    }
    return StyleData.defPrevNodeData;
  }

  public LinkNodeData GetLinkStyle(int linkType)
  {
    if (this.linkStyles.ContainsKey(linkType))
      return this.linkStyles[linkType];
    return this.linkStyles.ContainsKey(-1) ? this.linkStyles[-1] : StyleData.defLinkNodeData;
  }
}
