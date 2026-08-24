// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PublishCompositionPart
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel.Search;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Queries;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Site.Client;

internal sealed class PublishCompositionPart : ObjectsListPart
{
  private List<PublishCompositionObject> _publishObjects;

  public PublishCompositionPart(
    List<PublishCompositionObject> objectIDs,
    int objectTypeID,
    IServiceProvider services)
    : base((IList) objectIDs.ConvertAll<long>((Converter<PublishCompositionObject, long>) (x => x.ObjectID)), services, objectTypeID)
  {
    this._publishObjects = objectIDs;
  }

  protected override INodeQuery GetObjectsQuery(
    INodeQuerySupport support,
    int objTypeID,
    ConditionStructure[] conditions,
    IServiceProvider services)
  {
    return (INodeQuery) new PublishCompositionQuery(support, objTypeID, conditions, services, this._publishObjects);
  }

  public override List<object> GetSpecialFields()
  {
    List<object> specialFields = base.GetSpecialFields();
    specialFields.Add((object) new NodeColumnID((object) MetaDataHelper.GetAttributeTypeID(PortalConsts.attributeVerCode), AttributeSourceTypes.Object));
    specialFields.Add((object) new NodeColumnID((object) MetaDataHelper.GetAttributeTypeID(PortalConsts.attributeReasonInfo), AttributeSourceTypes.Object));
    return specialFields;
  }
}
