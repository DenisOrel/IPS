// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PublishCompositionNode
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel.Search;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Parts;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Intermech.Site.Client;

internal sealed class PublishCompositionNode : CompositeNode, IContextAware
{
  private static NodeColumnCollection _columns;
  private IServiceProvider _services;
  private List<PublishCompositionObject> _publishObjects;

  public PublishCompositionNode(List<PublishCompositionObject> publishObjects)
  {
    this._publishObjects = publishObjects;
  }

  public override NodeColumnCollection GetDefaultColumns(ContentType content)
  {
    return PublishCompositionNode.DefaultColumns;
  }

  protected override List<PartSlot> CreateNonFolderSlots()
  {
    if (this._publishObjects == null || this._publishObjects.Count == 0)
      return (List<PartSlot>) null;
    Dictionary<int, List<PublishCompositionObject>> dictionary = new Dictionary<int, List<PublishCompositionObject>>();
    foreach (PublishCompositionObject publishObject in this._publishObjects)
    {
      List<PublishCompositionObject> compositionObjectList;
      if (!dictionary.TryGetValue(publishObject.ObjectType, out compositionObjectList))
      {
        compositionObjectList = new List<PublishCompositionObject>();
        dictionary.Add(publishObject.ObjectType, compositionObjectList);
      }
      compositionObjectList.Add(publishObject);
    }
    List<PartSlot> nonFolderSlots = new List<PartSlot>(dictionary.Count);
    foreach (KeyValuePair<int, List<PublishCompositionObject>> keyValuePair in dictionary)
    {
      if (keyValuePair.Value.Count > 0)
        nonFolderSlots.Add(new PartSlot(Intermech.Consts.CategoryObjectVersionGUID, (INodePart) new PublishCompositionPart(keyValuePair.Value, keyValuePair.Key, this.Services)));
    }
    return nonFolderSlots;
  }

  public static NodeColumnCollection DefaultColumns
  {
    get
    {
      if (PublishCompositionNode._columns == null)
      {
        PublishCompositionNode._columns = new NodeColumnCollection();
        IColumnSchemes service = ServicesManager.GetService(typeof (IColumnSchemes)) as IColumnSchemes;
        NodeColumn column1 = service.CreateColumn(Intermech.Navigator.Consts.ObjectObligatoryColumnSchemeGuid, (object) ObligatoryObjectAttributes.F_OBJECT_ID);
        PublishCompositionNode._columns.Add(column1);
        NodeColumn column2 = service.CreateColumn(Intermech.Navigator.Consts.ObjectObligatoryColumnSchemeGuid, (object) ObligatoryObjectAttributes.CAPTION);
        PublishCompositionNode._columns.Add(column2);
        NodeColumn column3 = service.CreateColumn(Intermech.Navigator.Consts.ObjectObligatoryColumnSchemeGuid, (object) ObligatoryObjectAttributes.F_VERSION_ID);
        PublishCompositionNode._columns.Add(column3);
        NodeColumn column4 = service.CreateColumn(Intermech.Navigator.Consts.ObjectColumnSchemeGuid, (object) MetaDataHelper.GetAttributeTypeID(PortalConsts.attributeVerCode));
        PublishCompositionNode._columns.Add(column4);
        NodeColumn column5 = service.CreateColumn(Intermech.Navigator.Consts.ObjectObligatoryColumnSchemeGuid, (object) ObligatoryObjectAttributes.F_OBJECT_TYPE);
        PublishCompositionNode._columns.Add(column5);
        NodeColumn column6 = service.CreateColumn(Intermech.Navigator.Consts.ObjectColumnSchemeGuid, (object) MetaDataHelper.GetAttributeTypeID(PortalConsts.attributeReasonInfo));
        PublishCompositionNode._columns.Add(column6);
        NodeColumn column7 = service.CreateColumn(Intermech.Navigator.Consts.ObjectColumnSchemeGuid, (object) MetaDataHelper.GetAttributeTypeID(PortalConsts.attributeEnabledSites));
        PublishCompositionNode._columns.Add(column7);
      }
      return PublishCompositionNode._columns;
    }
  }

  public override NodeColumnCollection GetSupportedColumns(
    ContentType content,
    string ColumnSetName)
  {
    return base.GetSupportedColumns(content, ColumnSetName);
  }

  public IServiceProvider Services
  {
    [DebuggerStepThrough] get => this._services;
    set => this._services = value;
  }
}
