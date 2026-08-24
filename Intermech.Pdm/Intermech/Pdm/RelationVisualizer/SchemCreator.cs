// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.SchemCreator
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Pdm.RelationVisualizer;
using Intermech.Map;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public class SchemCreator
{
  internal VisControl control;
  internal VisScheme scheme;
  internal MapDocument document;
  internal PreviewMode prevMode;
  internal Dictionary<long, VisNode> NodeDict;
  internal Dictionary<(long, long), VisLink> RelDict;

  public SchemCreator(VisScheme sch, VisControl vc, MapDocument doc, PreviewMode prevMode)
  {
    this.control = vc;
    this.scheme = sch;
    this.document = doc;
    this.prevMode = prevMode;
    this.NodeDict = new Dictionary<long, VisNode>();
    this.RelDict = new Dictionary<(long, long), VisLink>();
  }

  public virtual void CreateFullScheme()
  {
    this.NodeDict.Clear();
    this.RelDict.Clear();
    this.CreateNode(this.scheme.RootObj, this.document.Layers.Find((object) 0));
    this.scheme.ChildLevels.ForEach((Action<VisLevel>) (level => this.CreateNodes(level)));
    this.scheme.ParentLevels.ForEach((Action<VisLevel>) (level => this.CreateNodes(level)));
    this.scheme.ChildLevels.ForEach((Action<VisLevel>) (level => this.RemoveInvalidLinks(level)));
    this.scheme.ParentLevels.ForEach((Action<VisLevel>) (level => this.RemoveInvalidLinks(level)));
    this.scheme.ChildLevels.ForEach((Action<VisLevel>) (level => this.CreateLinks(level)));
    this.scheme.ParentLevels.ForEach((Action<VisLevel>) (level => this.CreateLinks(level)));
  }

  public virtual void CreateNodes(VisLevel level)
  {
    MapLayer layer = this.document.Layers.Find((object) (int) this.LevNum2LayerKey(level));
    level.ForEach((Action<VisObject>) (obj => this.CreateNode(obj, layer)));
  }

  private void RemoveInvalidLinks(VisLevel level)
  {
    if (level.LevelNum < 0)
      level.ForEach((Action<VisObject>) (visObj =>
      {
        for (int index = visObj.ChildRels.Count - 1; index >= 0; --index)
        {
          VisRelation childRel = visObj.ChildRels[index];
          if (childRel.Parent.Node == null || childRel.Child.Node == null)
            visObj.ChildRels.RemoveAt(index);
        }
      }));
    else
      level.ForEach((Action<VisObject>) (visObj =>
      {
        for (int index = visObj.ParentRels.Count - 1; index >= 0; --index)
        {
          VisRelation parentRel = visObj.ParentRels[index];
          if (parentRel.Parent.Node == null || parentRel.Child.Node == null)
            visObj.ParentRels.RemoveAt(index);
        }
      }));
  }

  public virtual void CreateLinks(VisLevel level)
  {
    MapLayer layer = this.document.Layers.Find((object) (int) this.LevNum2LayerKey(level));
    if (level.LevelNum < 0)
      level.ForEach((Action<VisObject>) (visObj => visObj.ChildRels.ForEach((Action<VisRelation>) (vr => this.CreateLink(vr, layer)))));
    else
      level.ForEach((Action<VisObject>) (visObj => visObj.ParentRels.ForEach((Action<VisRelation>) (vr => this.CreateLink(vr, layer)))));
  }

  public static void MarkInvisibleChanged(VisScheme scheme)
  {
    if (scheme.ChildLevels != null)
    {
      foreach (IEnumerable<VisObject> childLevel in scheme.ChildLevels)
      {
        foreach (VisObject visObject in childLevel.Where<VisObject>((Func<VisObject, bool>) (vo => !vo.Visible)))
          visObject.VisibleChanged = true;
      }
    }
    if (scheme.ParentLevels == null)
      return;
    foreach (IEnumerable<VisObject> parentLevel in scheme.ParentLevels)
    {
      foreach (VisObject visObject in parentLevel.Where<VisObject>((Func<VisObject, bool>) (vo => !vo.Visible)))
        visObject.VisibleChanged = true;
    }
  }

  public static void ProcessInvisible(VisScheme scheme)
  {
    if (scheme.ChildLevels != null)
    {
      foreach (VisLevel childLevel in scheme.ChildLevels)
        SchemCreator.ChangeLevelVisibility(childLevel);
    }
    if (scheme.ParentLevels == null)
      return;
    foreach (VisLevel parentLevel in scheme.ParentLevels)
      SchemCreator.ChangeLevelVisibility(parentLevel);
  }

  internal static void ChangeLevelVisibility(VisLevel level)
  {
    foreach (VisObject vo in level.Where<VisObject>((Func<VisObject, bool>) (vo => vo.VisibleChanged)))
    {
      SchemCreator.ChangeNodeVisibility(vo);
      vo.VisibleChanged = false;
    }
  }

  internal static void ChangeNodeVisibility(VisObject vo)
  {
    vo.Node.Visible = vo.Visible;
    if (vo.ChildRels != null)
    {
      foreach (VisRelation childRel in vo.ChildRels)
      {
        bool flag = vo.Visible && childRel.Child.Visible;
        if (childRel.Link.Visible != flag)
          childRel.Link.Visible = flag;
      }
    }
    if (vo.ParentRels == null)
      return;
    foreach (VisRelation parentRel in vo.ParentRels)
    {
      bool flag = vo.Visible && parentRel.Parent.Visible;
      if (parentRel.Link.Visible != flag)
        parentRel.Link.Visible = flag;
    }
  }

  internal RelVisPred.RelVisLayers LevNum2LayerKey(VisLevel level)
  {
    RelVisPred.RelVisLayers relVisLayers = RelVisPred.RelVisLayers.ChildTree;
    if (level.LevelNum < 0)
      relVisLayers = RelVisPred.RelVisLayers.ParentTree;
    if (level.LevelNum == 0)
      relVisLayers = RelVisPred.RelVisLayers.GeneralTree;
    return relVisLayers;
  }

  internal virtual VisNode CreateNode(VisObject visObj, MapLayer layer)
  {
    bool flag = false;
    switch (this.prevMode)
    {
      case PreviewMode.SelPreview:
        flag = visObj.Preview != null && (visObj.Level == 0 || visObj.HasPreviewType);
        break;
      case PreviewMode.FullPreview:
        flag = visObj.Preview != null;
        break;
    }
    long key = Math.Abs(visObj.ObjVerId);
    if (this.NodeDict.ContainsKey(key))
      return this.NodeDict[key];
    VisNode node = flag ? (VisNode) new VisPrevNode(visObj, layer) : new VisNode(visObj, layer);
    this.NodeDict.Add(key, node);
    return node;
  }

  internal virtual VisLink CreateLink(VisRelation visRel, MapLayer layer)
  {
    (long, long) key = (Math.Abs(visRel.Parent.ObjVerId), Math.Abs(visRel.Child.ObjVerId));
    if (this.RelDict.ContainsKey(key))
      return this.RelDict[key];
    VisLink link = new VisLink(visRel, layer);
    this.RelDict.Add(key, link);
    return link;
  }

  public SchemePackage GetPackage() => new SchemePackage(this.scheme, this.document);
}
