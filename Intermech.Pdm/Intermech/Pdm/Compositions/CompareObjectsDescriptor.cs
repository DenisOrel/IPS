// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareObjectsDescriptor
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Persistence;
using Intermech.Navigator.VirtualNodes;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Intermech.Pdm.Compositions;

public class CompareObjectsDescriptor : HiveDescriptor
{
  private List<Tuple<long, int>> _compareObjectIDs;
  private List<Guid> _compareObjectGuids;
  private Dictionary<int, bool> _relationTypes;
  private readonly string _configCompareObjectsList = "CompareObjectsList";
  private BackgroundReaderComparer _reader;
  private CompareObjectsInfo _info;
  private IServiceProvider _services;

  public CompareObjectsDescriptor(
    IServiceProvider services,
    List<Guid> compareObjectGuids,
    List<Tuple<long, int>> compareObjectIDs,
    Dictionary<int, bool> relationTypes)
    : base(PDMPluginConsts.CategoryCompareObjectsRoot, 0, PDMPluginConsts.ListCompareObjects)
  {
    this._compareObjectIDs = compareObjectIDs;
    this._compareObjectGuids = compareObjectGuids;
    this._relationTypes = relationTypes;
    this._info = new CompareObjectsInfo(relationTypes);
    this._reader = new BackgroundReaderComparer(services);
    this._services = services;
  }

  protected CompareObjectsDescriptor(PersistentState state)
    : base(state)
  {
    string str = (string) state.GetValue(this._configCompareObjectsList);
    if (str == null || str.Length <= 0)
      return;
    string[] strArray = str.Split(';');
    this._compareObjectGuids = new List<Guid>(strArray.Length);
    this._compareObjectIDs = new List<Tuple<long, int>>(strArray.Length);
    this._services = (IServiceProvider) new AdvancedServiceContainer((IServiceProvider) ServicesManager.ServiceContainer);
    this._reader = new BackgroundReaderComparer(this._services);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      List<int> objectTypes = new List<int>(strArray.Length);
      for (int index = 0; index < strArray.Length; ++index)
      {
        Guid objectGUID = new Guid(strArray[index]);
        IDBObject dbObject = sessionKeeper.Session.GetObject(objectGUID);
        this._compareObjectIDs.Add(new Tuple<long, int>(dbObject.ObjectID, dbObject.ObjectType));
        this._compareObjectGuids.Add(objectGUID);
        if (!objectTypes.Contains(dbObject.ObjectType))
          objectTypes.Add(dbObject.ObjectType);
      }
      this._relationTypes = CompareHelper.GetOwnRelationTypes(sessionKeeper.Session, objectTypes);
      this._info = this._relationTypes.Count != 0 ? new CompareObjectsInfo(this._relationTypes) : throw new Exception(LocalizationHolder.rm.GetString("Pdm_481"));
    }
    if (this._compareObjectIDs.Count != 1)
      return;
    if (this._compareObjectIDs[0].Item1 >= 0L)
      throw new Exception(LocalizationHolder.rm.GetString("Pdm_524"));
    this._compareObjectGuids.Add(this._compareObjectGuids[0]);
    this._compareObjectIDs.Add(new Tuple<long, int>(Math.Abs(this._compareObjectIDs[0].Item1), this._compareObjectIDs[0].Item2));
  }

  public Guid Guid => PDMPluginGuids.CategoryCompareObjectsRootGuid;

  public override INode GetChild(INodeID nodeID)
  {
    return (INode) new CompareObjectsListNode(this._reader, this._compareObjectIDs, this._relationTypes, this._info);
  }

  public override void GetObjectData(PersistentState state)
  {
    base.GetObjectData(state);
    if (this._compareObjectGuids == null || this._compareObjectGuids.Count == 0)
      return;
    StringBuilder stringBuilder = new StringBuilder();
    if (this._compareObjectGuids.Count == 2 && this._compareObjectGuids[0] == this._compareObjectGuids[1])
    {
      stringBuilder.Append(this._compareObjectGuids[0].ToString());
    }
    else
    {
      for (int index = 0; index < this._compareObjectGuids.Count; ++index)
      {
        if (index > 0)
          stringBuilder.Append(';');
        stringBuilder.Append(this._compareObjectGuids[index].ToString());
      }
    }
    state.AddValue(this._configCompareObjectsList, (object) stringBuilder.ToString());
  }

  public override bool Equals(object obj)
  {
    if (obj == null || obj.GetType() != typeof (CompareObjectsDescriptor))
      return base.Equals(obj);
    CompareObjectsDescriptor objectsDescriptor = (CompareObjectsDescriptor) obj;
    if (objectsDescriptor._compareObjectIDs != null && this._compareObjectIDs == null || objectsDescriptor._compareObjectIDs == null && this._compareObjectIDs != null)
      return false;
    if (objectsDescriptor._compareObjectIDs == null && this._compareObjectIDs == null)
      return true;
    if (objectsDescriptor._compareObjectIDs.Count != this._compareObjectIDs.Count)
      return false;
    for (int index = 0; index < objectsDescriptor._compareObjectIDs.Count; ++index)
    {
      if (!objectsDescriptor._compareObjectIDs.Equals((object) this._compareObjectIDs))
        return false;
    }
    return true;
  }

  public override int GetHashCode()
  {
    if (this._compareObjectGuids == null || this._compareObjectGuids.Count == 0)
      return -1;
    int hashCode = this._compareObjectGuids[0].GetHashCode();
    if (this._compareObjectIDs.Count > 1)
    {
      for (int index = 1; index < this._compareObjectGuids.Count; ++index)
        hashCode ^= this._compareObjectGuids[index].GetHashCode();
    }
    return hashCode;
  }

  public override object GetData(INodeID nodeID, Type dataFormat)
  {
    if (dataFormat == typeof (IDescriptor))
      return (object) this;
    return dataFormat == typeof (ICanOpenInNewWindow) ? (object) new CanOpenInNewWindow() : base.GetData(nodeID, dataFormat);
  }
}
