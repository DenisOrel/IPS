// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.VisScheme
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Expert;
using Intermech.Interfaces.Pdm;
using Intermech.Localization;
using Intermech.Pdm.VisDialogs;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public class VisScheme
{
  public Dictionary<long, VisObject> voIndex;
  public Dictionary<(long, long), VisRelation> vrIndex;
  public LoadSettings Loaded;
  private static readonly int attrCountId = 0;
  private static readonly string attrCountIdStr = (string) null;
  private static readonly Dictionary<int, bool> relTypeCount = (Dictionary<int, bool>) null;
  private static readonly string CadRelationTypeGUID = "cadd94da-306c-11d8-b4e9-00304f19f545";
  private bool _showStructLinks = true;
  private bool _showAssocLinks = true;
  private int _objCount = -1;
  private int previewMode;
  private IVisualizerService visServ;
  internal StyleData StyleData;
  private readonly string attributeFlag = "cad0147c-306c-11d8-b4e9-00304f19f545";

  public VisObject RootObj { get; set; }

  public List<VisLevel> ParentLevels { get; internal set; }

  public List<VisLevel> ChildLevels { get; internal set; }

  public string FiltrationOwnerID { get; internal set; }

  public HybridDictionary FiltrationParms { get; internal set; }

  public int MaxPreviewMode { get; set; }

  static VisScheme()
  {
    VisScheme.attrCountId = MetaDataHelper.GetAttributeTypeID(new Guid("cad00267-306c-11d8-b4e9-00304f19f545"));
    VisScheme.attrCountIdStr = VisScheme.attrCountId.ToString();
    VisScheme.relTypeCount = new Dictionary<int, bool>();
  }

  public VisScheme(PreviewMode pMode)
  {
    this.voIndex = new Dictionary<long, VisObject>();
    this.vrIndex = new Dictionary<(long, long), VisRelation>();
    this.previewMode = (int) pMode;
  }

  public void Clear()
  {
    this.ParentLevels?.Clear();
    this.ChildLevels?.Clear();
    this.voIndex?.Clear();
    this.vrIndex?.Clear();
  }

  public void UpdatePreviewMode(int newPreviewMode)
  {
    this.previewMode = newPreviewMode;
    if (newPreviewMode <= this.MaxPreviewMode)
      return;
    this.MaxPreviewMode = newPreviewMode;
  }

  public void Init(IVisualizerService serv, StyleData sData)
  {
    this.visServ = serv;
    this.StyleData = sData;
  }

  public void BuildRoot(long rootId)
  {
    this.RootObj = new VisObject((IVisObjectData) new VisObjData(rootId), (VisLevel) null);
    this.voIndex.Clear();
    this.voIndex.Add(rootId, this.RootObj);
    this.vrIndex.Clear();
    this.RootObj.ParentScheme = this;
  }

  public void BuildParents(BackgroundWorker bw, bool showStructLinks = true, bool showAssocLinks = true)
  {
    if (this.ParentLevels != null)
      this.ParentLevels.Clear();
    else
      this.ParentLevels = new List<VisLevel>();
    this._showStructLinks = showStructLinks;
    this._showAssocLinks = showAssocLinks;
    this._DoGetParents(bw, this.RootObj.ObjVerId);
  }

  public void ExpandParents(BackgroundWorker bw, long objId, int maxLevels)
  {
    if (this.ParentLevels == null)
      this.ParentLevels = new List<VisLevel>();
    this._DoGetParents(bw, objId, maxLevels);
  }

  private RelFilter GetRelFilter()
  {
    if (this._showStructLinks && this._showAssocLinks)
      return RelFilter.ShowAll;
    return this._showStructLinks ? RelFilter.OnlyStruct : RelFilter.OnlyAssoc;
  }

  private List<long> CollectUpdatableObjects(bool parents)
  {
    List<long> longList = new List<long>();
    foreach (List<VisObject> visObjectList in parents ? this.ParentLevels : this.ChildLevels)
    {
      foreach (VisObject visObject in visObjectList)
      {
        if (!visObject.PreviewChecked)
          longList.Add(visObject.ObjVerId);
      }
    }
    return longList;
  }

  private void ApplyData(Dictionary<long, VisScheme.PreviewInfo> dict, bool parents)
  {
    foreach (List<VisObject> visObjectList in parents ? this.ParentLevels : this.ChildLevels)
    {
      foreach (VisObject visObject in visObjectList)
      {
        if (dict.ContainsKey(visObject.ObjVerId))
        {
          VisScheme.PreviewInfo previewInfo = dict[visObject.ObjVerId];
          visObject.HasPreviewType = previewInfo.RightType;
          if (previewInfo.Preview != null && previewInfo.Preview.Length != 0)
          {
            using (MemoryStream memoryStream = new MemoryStream(previewInfo.Preview))
            {
              Image prevImage = Image.FromStream((Stream) memoryStream);
              visObject.PreparePreview(prevImage);
            }
          }
        }
      }
    }
  }

  public void UpdatePreviews(BackgroundWorker bw, bool parents, int newPreview)
  {
    HybridTableExp hybridTableExp = (HybridTableExp) null;
    List<long> longList = this.CollectUpdatableObjects(parents);
    long schemeId = 0;
    if (newPreview == 1)
      schemeId = parents ? PDMPlugin.ApplicabilitySchemeId : PDMPlugin.CompositionSchemeId;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IUserSession session = sessionKeeper.Session;
      long taskId = -1;
      try
      {
        taskId = this.visServ.StartCollectPreviews(longList.ToArray(), session.SessionGUID, schemeId);
        RelVisState taskStatus;
        do
        {
          Thread.Sleep(100);
          taskStatus = this.visServ.GetTaskStatus(taskId);
        }
        while (!bw.CancellationPending && taskStatus == RelVisState.Working);
        if (taskStatus == RelVisState.Ready)
          hybridTableExp = this.visServ.GetTaskResult(taskId);
        if (taskStatus == RelVisState.Error)
        {
          Exception error = this.visServ.GetError(taskId);
          if (error != null)
            throw new Exception(error.Message, error);
        }
      }
      finally
      {
        if (taskId >= 0L)
          this.visServ.KillTask(taskId);
      }
    }
    if (hybridTableExp == null || bw.CancellationPending)
      return;
    Dictionary<long, VisScheme.PreviewInfo> dict = new Dictionary<long, VisScheme.PreviewInfo>();
    foreach (HybridRowExp row in hybridTableExp.Rows)
    {
      long int64 = Convert.ToInt64(row["cad00029-306c-11d8-b4e9-00304f19f545"]);
      object obj1 = row[this.attributeFlag];
      object obj2 = row[SystemGUIDs.attributePreview.ToString()];
      bool rightType = obj1 != null && obj1 != DBNull.Value && Convert.ToBoolean(obj1);
      byte[] preview = obj2 as byte[];
      dict.Add(int64, new VisScheme.PreviewInfo(int64, rightType, preview));
    }
    this.ApplyData(dict, parents);
  }

  private HiddenCompositionFiltrationMode GetHiddenMode()
  {
    if (ServicesManager.GetService(typeof (IClientPluginsService)) is IClientPluginsService service)
    {
      HybridDictionary PluginsData = new HybridDictionary();
      service.GetClientPluginsData(ref PluginsData);
      if (PluginsData.Contains((object) "{54C2DCB9-63C7-4736-867B-1EA7539B7645}"))
        return (HiddenCompositionFiltrationMode) PluginsData[(object) "{54C2DCB9-63C7-4736-867B-1EA7539B7645}"];
    }
    return HiddenCompositionFiltrationMode.None;
  }

  private void _DoGetParents(BackgroundWorker bw, long rootObjId, int levelsOverride = -1)
  {
    if (rootObjId == 0L)
      throw new Exception(LocalizationHolder.rm.GetString("Pdm_rv_28"));
    if (this.visServ == null)
      throw new Exception(LocalizationHolder.rm.GetString("Pdm_rv_30"));
    IElementStatusesClientService service1 = ServicesManager.GetService<IElementStatusesClientService>();
    HybridDictionary PluginsData = new HybridDictionary();
    ServicesManager.GetService<IClientPluginsService>()?.GetClientPluginsData(ref PluginsData);
    ICompositionsAutosortRule rule = (ICompositionsAutosortRule) null;
    ICurrentUserAndRole service2 = ServicesManager.GetService<ICurrentUserAndRole>();
    if (service2 != null)
      rule = (ICompositionsAutosortRule) service2.Rule;
    HybridTableExp hybridTableExp = (HybridTableExp) null;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IUserSession session = sessionKeeper.Session;
      QuickObjectInfo objectInfo = session.GetObjectInfo(rootObjId);
      long taskId = -1;
      try
      {
        HiddenCompositionFiltrationMode hiddenMode = this.GetHiddenMode();
        taskId = this.visServ.StartBuildParentTree(rootObjId, objectInfo.ID, PDMPlugin.ApplicabilitySchemeId, this.FiltrationOwnerID, rule, hiddenMode, session.SessionGUID, this.GetRelFilter(), PluginsData, levelsOverride, this.previewMode);
        RelVisState taskStatus;
        do
        {
          Thread.Sleep(100);
          taskStatus = this.visServ.GetTaskStatus(taskId);
        }
        while (!bw.CancellationPending && taskStatus == RelVisState.Working);
        if (taskStatus == RelVisState.Ready)
          hybridTableExp = this.visServ.GetTaskResult(taskId);
        if (taskStatus == RelVisState.Error)
        {
          Exception error = this.visServ.GetError(taskId);
          if (error != null)
            throw new Exception(error.Message, error);
        }
      }
      finally
      {
        if (taskId >= 0L)
          this.visServ.KillTask(taskId);
      }
    }
    if (hybridTableExp == null || bw.CancellationPending)
      return;
    Dictionary<long, VisObject> dictionary = new Dictionary<long, VisObject>();
    List<VisRelation> visRelationList = new List<VisRelation>();
    for (int index = 0; index < hybridTableExp.RowsCount; ++index)
    {
      HybridRowExp row = hybridTableExp[index];
      if (row["cad0002e-306c-11d8-b4e9-00304f19f545"] != null && row["cad0002e-306c-11d8-b4e9-00304f19f545"] != DBNull.Value)
      {
        CADRelType crType = CADRelType.Unknown;
        object obj1 = row[VisScheme.CadRelationTypeGUID];
        if (obj1 != null && obj1 != DBNull.Value)
          crType = (CADRelType) Convert.ToInt32(obj1);
        long int64_1 = Convert.ToInt64(row["cad00033-306c-11d8-b4e9-00304f19f545"]);
        int int32_1 = Convert.ToInt32(row["cad00036-306c-11d8-b4e9-00304f19f545"]);
        long int64_2 = Convert.ToInt64(row["cad00034-306c-11d8-b4e9-00304f19f545"]);
        long int64_3 = Convert.ToInt64(row["F_PART_OBJ_ID"]);
        bool flag1;
        if (VisScheme.relTypeCount.ContainsKey(int32_1))
        {
          flag1 = VisScheme.relTypeCount[int32_1];
        }
        else
        {
          IMSRelationType relationType = MetaDataHelper.GetRelationType(int32_1);
          flag1 = relationType != null && (relationType.AnyAttributes || MetaDataHelper.GetAttribute4RelationType(int32_1, VisScheme.attrCountId) != null);
          VisScheme.relTypeCount.Add(int32_1, flag1);
        }
        MeasuredValue quan = (MeasuredValue) null;
        if (flag1 && row[VisScheme.attrCountIdStr] != null && row[VisScheme.attrCountIdStr] != DBNull.Value)
          quan = MeasureHelper.ConvertToMeasuredValue(Convert.ToString(row[VisScheme.attrCountIdStr]));
        VisObject visObject1;
        if (!this.voIndex.TryGetValue(int64_3, out visObject1) && !dictionary.TryGetValue(int64_3, out visObject1))
        {
          (long, long) key = (Math.Abs(int64_2), Math.Abs(int64_3));
          VisRelation visRelation = (VisRelation) null;
          if (!this.vrIndex.ContainsKey(key))
          {
            visRelation = new VisRelation((IVisRelationData) new VisRelData(int64_1, int32_1, int64_2, int64_3, quan, crType));
            visRelationList.Add(visRelation);
            visObject1.ParentRels.Add(visRelation);
            visRelation.Child = visObject1;
            this.vrIndex.Add(key, visRelation);
          }
          VisObject visObject2;
          if (this.voIndex.TryGetValue(int64_2, out visObject2) || dictionary.TryGetValue(int64_2, out visObject2))
          {
            if (visRelation != null)
            {
              visObject2.ChildRels.Add(visRelation);
              visRelation.Parent = visObject2;
              visRelation.ProcessStyle(row);
            }
          }
          else
          {
            int int32_2 = Convert.ToInt32(row["cad0002e-306c-11d8-b4e9-00304f19f545"]);
            string capt = Convert.ToString(row["cad00047-306c-11d8-b4e9-00304f19f545"]);
            int int32_3 = Convert.ToInt32(row["cad00030-306c-11d8-b4e9-00304f19f545"]);
            List<VisStatus> sList = VisStatusKeeper.MakeStatuses(row["cad005f1-306c-11d8-b4e9-00304f19f545"] as byte[], service1);
            Image prevImage = (Image) null;
            bool flag2 = false;
            if (row[SystemGUIDs.attributePreview.ToString()] is byte[] buffer)
            {
              if (buffer.Length != 0)
              {
                using (MemoryStream memoryStream = new MemoryStream(buffer))
                  prevImage = Image.FromStream((Stream) memoryStream);
              }
              flag2 = true;
            }
            object obj2 = row[this.attributeFlag];
            VisObject visObject3 = new VisObject((IVisObjectData) new VisObjData(int64_2, int32_2, int32_3, sList, capt), (VisLevel) null);
            visRelation.Parent = visObject3;
            visObject3.Level = visObject1.Level - 1;
            visObject3.ChildRels.Add(visRelation);
            visObject3.ParentScheme = this;
            visObject3.SetDataRow(row);
            visObject3.PreparePreview(prevImage);
            visObject3.PreviewChecked = flag2;
            if (obj2 != null && obj2 != DBNull.Value && Convert.ToBoolean(obj2))
              visObject3.HasPreviewType = true;
            visRelation.ProcessStyle(row);
            dictionary.Add(int64_2, visObject3);
          }
        }
      }
    }
    this.RootObj.ProcessRoot(hybridTableExp.Columns);
    foreach (VisRelation visRelation in visRelationList)
    {
      HashSet<long> stamp = new HashSet<long>();
      VisScheme.PropagateLevelChange(visRelation.Parent, false, visRelation.Child.Level - 1, stamp);
    }
    foreach (VisObject visObject in dictionary.Values)
    {
      if (visObject.Level < 0 && !this.voIndex.ContainsKey(visObject.ObjVerId))
      {
        this.voIndex.Add(visObject.ObjVerId, visObject);
        while (this.ParentLevels.Count < -visObject.Level)
          this.ParentLevels.Add(new VisLevel(-this.ParentLevels.Count - 1, this));
        VisLevel parentLevel = this.ParentLevels[-visObject.Level - 1];
        parentLevel.Add(visObject);
        visObject.ParentLevel = parentLevel;
      }
    }
  }

  private static void PropagateLevelChange(
    VisObject vo,
    bool childs,
    int newLevel,
    HashSet<long> stamp)
  {
    if (childs)
    {
      if (vo.Level >= newLevel)
        return;
      vo.Level = newLevel;
      vo.ChildRels.ForEach((Action<VisRelation>) (vr => ProcessLink(vr, newLevel + 1)));
    }
    else
    {
      if (vo.Level <= newLevel)
        return;
      vo.Level = newLevel;
      vo.ChildRels.ForEach((Action<VisRelation>) (vr => ProcessLink(vr, newLevel - 1)));
    }

    void ProcessLink(VisRelation vr, int childLevel)
    {
      long num = childs ? vr.Child.ObjVerId : vr.Parent.ObjVerId;
      if (stamp.Contains(num))
        return;
      stamp.Add(num);
      try
      {
        VisScheme.PropagateLevelChange(childs ? vr.Child : vr.Parent, childs, childLevel, stamp);
      }
      finally
      {
        stamp.Remove(num);
      }
    }
  }

  public void BuildChilds(BackgroundWorker bw, bool showStructLinks = true, bool showAssocLinks = true)
  {
    if (this.ChildLevels != null)
      this.ChildLevels.Clear();
    else
      this.ChildLevels = new List<VisLevel>();
    this._showStructLinks = showStructLinks;
    this._showAssocLinks = showAssocLinks;
    int hiddenMode = (int) this.GetHiddenMode();
    this._DoGetChilds(bw, this.RootObj.ObjVerId);
  }

  public void ExpandChilds(BackgroundWorker bw, long objId, int maxLevels)
  {
    if (this.ChildLevels == null)
      this.ChildLevels = new List<VisLevel>();
    this._DoGetChilds(bw, objId, maxLevels);
  }

  private void _DoGetChilds(BackgroundWorker bw, long rootObjId, int levelsOverride = -1)
  {
    if (rootObjId == 0L)
      throw new Exception(LocalizationHolder.rm.GetString("Pdm_rv_28"));
    if (this.visServ == null)
      throw new Exception(LocalizationHolder.rm.GetString("Pdm_rv_30"));
    IElementStatusesClientService service1 = ServicesManager.GetService<IElementStatusesClientService>();
    HybridDictionary PluginsData = new HybridDictionary();
    ServicesManager.GetService<IClientPluginsService>()?.GetClientPluginsData(ref PluginsData);
    ICompositionsAutosortRule rule = (ICompositionsAutosortRule) null;
    ICurrentUserAndRole service2 = ServicesManager.GetService<ICurrentUserAndRole>();
    if (service2 != null)
      rule = (ICompositionsAutosortRule) service2.Rule;
    HybridTableExp hybridTableExp = (HybridTableExp) null;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      long taskId = -1;
      try
      {
        HiddenCompositionFiltrationMode hiddenMode = this.GetHiddenMode();
        taskId = this.visServ.StartBuildChildTree(rootObjId, PDMPlugin.CompositionSchemeId, this.FiltrationOwnerID, rule, sessionKeeper.Session.SessionGUID, hiddenMode, this.GetRelFilter(), PluginsData, levelsOverride, this.previewMode);
        RelVisState taskStatus;
        do
        {
          Thread.Sleep(100);
          taskStatus = this.visServ.GetTaskStatus(taskId);
        }
        while (!bw.CancellationPending && taskStatus == RelVisState.Working);
        if (taskStatus == RelVisState.Ready)
          hybridTableExp = this.visServ.GetTaskResult(taskId);
        if (taskStatus == RelVisState.Error)
        {
          Exception error = this.visServ.GetError(taskId);
          if (error != null)
            throw new Exception(error.Message, error);
        }
      }
      finally
      {
        if (taskId >= 0L)
          this.visServ.KillTask(taskId);
      }
    }
    if (hybridTableExp == null || bw.CancellationPending)
      return;
    Dictionary<long, VisObject> dictionary = new Dictionary<long, VisObject>();
    List<VisRelation> visRelationList = new List<VisRelation>();
    for (int index = 0; index < hybridTableExp.RowsCount; ++index)
    {
      HybridRowExp row = hybridTableExp[index];
      if (row["cad0002e-306c-11d8-b4e9-00304f19f545"] != null && row["cad0002e-306c-11d8-b4e9-00304f19f545"] != DBNull.Value)
      {
        CADRelType crType = CADRelType.Unknown;
        object obj1 = row[VisScheme.CadRelationTypeGUID];
        if (obj1 != null && obj1 != DBNull.Value)
          crType = (CADRelType) Convert.ToInt32(obj1);
        long int64_1 = Convert.ToInt64(row["cad00033-306c-11d8-b4e9-00304f19f545"]);
        int int32_1 = Convert.ToInt32(row["cad00036-306c-11d8-b4e9-00304f19f545"]);
        long int64_2 = Convert.ToInt64(row["cad00034-306c-11d8-b4e9-00304f19f545"]);
        long int64_3 = Convert.ToInt64(row["cad00029-306c-11d8-b4e9-00304f19f545"]);
        bool flag1;
        if (VisScheme.relTypeCount.ContainsKey(int32_1))
        {
          flag1 = VisScheme.relTypeCount[int32_1];
        }
        else
        {
          IMSRelationType relationType = MetaDataHelper.GetRelationType(int32_1);
          flag1 = relationType != null && (relationType.AnyAttributes || MetaDataHelper.GetAttribute4RelationType(int32_1, VisScheme.attrCountId) != null);
          VisScheme.relTypeCount.Add(int32_1, flag1);
        }
        MeasuredValue quan = (MeasuredValue) null;
        if (flag1 && row[VisScheme.attrCountIdStr] != null && row[VisScheme.attrCountIdStr] != DBNull.Value)
          quan = MeasureHelper.ConvertToMeasuredValue(Convert.ToString(row[VisScheme.attrCountIdStr]));
        VisObject visObject1;
        if (this.voIndex.TryGetValue(int64_2, out visObject1) || dictionary.TryGetValue(int64_2, out visObject1))
        {
          (long, long) key = (Math.Abs(int64_2), Math.Abs(int64_3));
          VisRelation visRelation = (VisRelation) null;
          if (!this.vrIndex.ContainsKey(key))
          {
            visRelation = new VisRelation((IVisRelationData) new VisRelData(int64_1, int32_1, int64_2, int64_3, quan, crType));
            visRelationList.Add(visRelation);
            visObject1.ChildRels.Add(visRelation);
            visRelation.Parent = visObject1;
            this.vrIndex.Add(key, visRelation);
          }
          VisObject visObject2;
          if (this.voIndex.TryGetValue(int64_3, out visObject2) || dictionary.TryGetValue(int64_3, out visObject2))
          {
            if (visRelation != null)
            {
              visObject2.ParentRels.Add(visRelation);
              visRelation.Child = visObject2;
              visRelation.ProcessStyle(row);
            }
          }
          else
          {
            int int32_2 = Convert.ToInt32(row["cad0002e-306c-11d8-b4e9-00304f19f545"]);
            string capt = Convert.ToString(row["cad00047-306c-11d8-b4e9-00304f19f545"]);
            int int32_3 = Convert.ToInt32(row["cad00030-306c-11d8-b4e9-00304f19f545"]);
            List<VisStatus> sList = VisStatusKeeper.MakeStatuses(row["cad005f1-306c-11d8-b4e9-00304f19f545"] as byte[], service1);
            Image prevImage = (Image) null;
            bool flag2 = false;
            if (row[SystemGUIDs.attributePreview.ToString()] is byte[] buffer)
            {
              if (buffer.Length != 0)
              {
                using (MemoryStream memoryStream = new MemoryStream(buffer))
                  prevImage = Image.FromStream((Stream) memoryStream);
              }
              flag2 = true;
            }
            object obj2 = row[this.attributeFlag];
            VisObject visObject3 = new VisObject((IVisObjectData) new VisObjData(int64_3, int32_2, int32_3, sList, capt), (VisLevel) null);
            visRelation.Child = visObject3;
            visObject3.Level = visObject1.Level + 1;
            visObject3.ParentRels.Add(visRelation);
            visObject3.ParentScheme = this;
            visObject3.SetDataRow(row);
            visObject3.PreparePreview(prevImage);
            visObject3.PreviewChecked = flag2;
            if (obj2 != null && obj2 != DBNull.Value && Convert.ToBoolean(obj2))
              visObject3.HasPreviewType = true;
            visRelation.ProcessStyle(row);
            dictionary.Add(int64_3, visObject3);
          }
        }
      }
    }
    this.RootObj.ProcessRoot(hybridTableExp.Columns);
    foreach (VisRelation visRelation in visRelationList)
    {
      HashSet<long> stamp = new HashSet<long>();
      VisScheme.PropagateLevelChange(visRelation.Child, true, visRelation.Parent.Level + 1, stamp);
    }
    foreach (VisObject visObject in dictionary.Values)
    {
      if (visObject.Level > 0 && !this.voIndex.ContainsKey(visObject.ObjVerId))
      {
        this.voIndex.Add(visObject.ObjVerId, visObject);
        while (this.ChildLevels.Count < visObject.Level)
          this.ChildLevels.Add(new VisLevel(this.ChildLevels.Count + 1, this));
        VisLevel childLevel = this.ChildLevels[visObject.Level - 1];
        childLevel.Add(visObject);
        visObject.ParentLevel = childLevel;
      }
    }
  }

  private void ReportLevels(long relId, IEnumerable<VisObject> objList)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(relId.ToString() + ": ");
    foreach (VisObject visObject in objList)
      stringBuilder.Append($"{(object) (visObject.ObjVerId % 10000L)}-{(object) visObject.Level} ");
  }

  public int ObjectCount
  {
    get
    {
      if (this._objCount < 0)
        this._objCount = this.CalcObjectCount();
      return this._objCount;
    }
    set => this._objCount = value;
  }

  public int CalcObjectCount()
  {
    int res = 1;
    if (this.ParentLevels != null)
      this.ParentLevels.ForEach((Action<VisLevel>) (level => res += level.Count));
    if (this.ChildLevels != null)
      this.ChildLevels.ForEach((Action<VisLevel>) (level => res += level.Count));
    return res;
  }

  public (bool, bool) NeedExpandChildren(long objId)
  {
    if (this.RootObj.SameObject(objId))
      return (true, true);
    if (this.ChildLevels != null)
    {
      foreach (List<VisObject> childLevel in this.ChildLevels)
      {
        foreach (VisObject visObject in childLevel)
        {
          if (visObject.SameObject(objId))
            return (false, true);
        }
      }
    }
    if (this.ParentLevels != null)
    {
      foreach (List<VisObject> parentLevel in this.ParentLevels)
      {
        foreach (VisObject visObject in parentLevel)
        {
          if (visObject.SameObject(objId))
            return (true, false);
        }
      }
    }
    return (false, false);
  }

  internal VisNode FindVisNode(long objId, bool childs)
  {
    if (this.RootObj.SameObject(objId))
      return this.RootObj.Node;
    if (childs && this.ChildLevels != null)
    {
      foreach (List<VisObject> childLevel in this.ChildLevels)
      {
        foreach (VisObject visObject in childLevel)
        {
          if (visObject.SameObject(objId))
            return visObject.Node;
        }
      }
    }
    if (!childs && this.ParentLevels != null)
    {
      foreach (List<VisObject> parentLevel in this.ParentLevels)
      {
        foreach (VisObject visObject in parentLevel)
        {
          if (visObject.SameObject(objId))
            return visObject.Node;
        }
      }
    }
    return (VisNode) null;
  }

  public void UpdateStyle()
  {
    this.RootObj.UpdateStyle();
    foreach (VisRelation parentRel in this.RootObj.ParentRels)
      parentRel.UpdateStyle();
    foreach (VisRelation childRel in this.RootObj.ChildRels)
      childRel.UpdateStyle();
    foreach (List<VisObject> childLevel in this.ChildLevels)
    {
      foreach (VisObject visObject in childLevel)
        this._UpdateObjStyle(visObject, true);
    }
    foreach (List<VisObject> parentLevel in this.ParentLevels)
    {
      foreach (VisObject visObject in parentLevel)
        this._UpdateObjStyle(visObject, true);
    }
  }

  public void _UpdateObjStyle(VisObject obj, bool child)
  {
    obj.UpdateStyle();
    foreach (VisRelation visRelation in child ? obj.ChildRels : obj.ParentRels)
      visRelation.UpdateStyle();
  }

  public void ForEachObject(Action<VisObject> action)
  {
    if (this.RootObj != null)
      action(this.RootObj);
    foreach (List<VisObject> parentLevel in this.ParentLevels)
      parentLevel.ForEach(action);
    foreach (List<VisObject> childLevel in this.ChildLevels)
      childLevel.ForEach(action);
  }

  internal class PreviewInfo
  {
    public long ObjId { get; set; }

    public bool RightType { get; set; }

    public byte[] Preview { get; }

    public PreviewInfo(long objId, bool rightType, byte[] preview)
    {
      this.ObjId = objId;
      this.RightType = rightType;
      this.Preview = preview;
    }
  }
}
