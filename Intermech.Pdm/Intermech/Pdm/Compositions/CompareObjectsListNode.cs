// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareObjectsListNode
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Parts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Intermech.Pdm.Compositions;

public class CompareObjectsListNode : CompositeNode, IContextAware, INodeNotifications
{
  private List<Tuple<long, int>> _compareObjects;
  private AdvancedServiceContainer _services = new AdvancedServiceContainer();
  private Dictionary<int, bool> _relationTypes;
  private Dictionary<long, bool> _refreshColumns;
  private CompareObjectsInfo _info;
  private BackgroundReaderComparer _reader;

  public CompareObjectsListNode(
    BackgroundReaderComparer reader,
    List<Tuple<long, int>> compareObjects,
    Dictionary<int, bool> relationTypes,
    CompareObjectsInfo info)
  {
    this._services.AddService(typeof (ObjectsSelectionOptionsHolder), (object) new ObjectsSelectionOptionsHolder(ObjectsSelectionOptions.ShowAllModifications));
    this._relationTypes = relationTypes;
    this._info = info;
    this._reader = reader;
    this._compareObjects = compareObjects;
    this._refreshColumns = new Dictionary<long, bool>(compareObjects.Count);
    for (int index = 0; index < compareObjects.Count; ++index)
      this._refreshColumns.Add(compareObjects[index].Item1, true);
  }

  protected override List<PartSlot> CreateFolderSlots()
  {
    return this.SlotsFromSinglePart((INodePart) new CompareObjectsListPart(this._reader, this._compareObjects, this.Services, this._relationTypes, this._info, this._refreshColumns));
  }

  protected override List<PartSlot> CreateNonFolderSlots()
  {
    return this.SlotsFromSinglePart((INodePart) new ObjectsListPart((IList) this._compareObjects.ConvertAll<long>((Converter<Tuple<long, int>, long>) (item => item.Item1)), this.Services));
  }

  public virtual IServiceProvider Services
  {
    [DebuggerStepThrough] get => (IServiceProvider) this._services;
    set => this._services.AdvancedProvider = value;
  }

  public ProcessResult Process(NotificationEventArgs e, object AdditionalInfo)
  {
    if (e is DBObjectsEventArgs objectsEventArgs && this._compareObjects != null && this._compareObjects.Count > 0 && (e.EventName == "ObjectsChanged" || e.EventName == "ObjectsChangesCancelled" || e.EventName == "ObjectsCheckedIn" || e.EventName == "ObjectsCheckedOut" || e.EventName == "ObjectsCreated" || e.EventName == "ObjectsFiltrationChanged" || e.EventName == "ObjectsRemoved" || e.EventName == "ProjectChanged"))
    {
      for (int index = 0; index < this._compareObjects.Count; ++index)
      {
        if (objectsEventArgs.ObjectIDs.Contains(this._compareObjects[index].Item1))
        {
          this.RefreshIDCollections(e.EventName, this._compareObjects[index].Item1, index);
          return ProcessResult.RefreshNode;
        }
      }
    }
    return ProcessResult.None;
  }

  private void RefreshIDCollections(string eventName, long objectID, int index)
  {
    if (eventName != "ObjectsCheckedIn" && eventName != "ObjectsCheckedOut" && eventName != "ObjectsChangesCancelled")
      return;
    long key = -1L * objectID;
    bool refreshColumn = this._refreshColumns[objectID];
    this._refreshColumns.Remove(objectID);
    this._refreshColumns.Add(key, refreshColumn);
    this._compareObjects[index] = new Tuple<long, int>(key, this._compareObjects[index].Item2);
  }
}
