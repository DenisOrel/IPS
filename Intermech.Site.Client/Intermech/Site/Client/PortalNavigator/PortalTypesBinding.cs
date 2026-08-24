// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.PortalTypesBinding
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel.Search;
using Intermech.Localization;
using Intermech.Navigator.DB;
using Intermech.Navigator.Parts;
using Intermech.Navigator.Selections;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal class PortalTypesBinding : ITopBinding, IBinding
{
  protected int typeID;
  private Guid _typeGuid;
  private ConditionStructure[] _topConditions;

  public int TypeID => this.typeID;

  public PortalTypesBinding(int typeID)
  {
    this.typeID = typeID;
    if (typeID != -1)
    {
      IPortalMetadata service = (IPortalMetadata) ServicesManager.GetService(typeof (IPortalMetadata));
      if (service != null)
      {
        PortalObjectType publishObjectType = service.GetPublishObjectType(typeID);
        if (publishObjectType != null)
        {
          this._typeGuid = new Guid(publishObjectType.GUID);
          this.ViewCaption = publishObjectType.Name;
        }
      }
    }
    if (!(this.ViewCaption == string.Empty))
      return;
    this.ViewCaption = LocalizationHolder.rm.GetString("Site.Client_36");
  }

  public void BindSelection(long selObjectID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(selObjectID);
      IDBAttribute attributeByGuid = dbObject.GetAttributeByGuid(PortalConsts.attributePortalObjectTypes);
      if (attributeByGuid == null)
      {
        dbObject.Attributes.AddAttribute(MetaDataHelper.GetAttributeTypeID(PortalConsts.attributePortalObjectTypes), false, new object[1]
        {
          (object) this._typeGuid
        });
      }
      else
      {
        object[] values = attributeByGuid.Values;
        if (values != null && values.Length == 1 && !GuidHelper.IsGuid(values[0].ToString()))
        {
          attributeByGuid.Value = (object) this._typeGuid;
        }
        else
        {
          if (values != null)
          {
            for (int index = 0; index < values.Length; ++index)
            {
              if (values[index].Equals((object) this._typeGuid))
                return;
            }
          }
          attributeByGuid.AddValue((object) this._typeGuid);
        }
      }
    }
  }

  public string GetCaption(int selTypeID) => Intermech.Navigator.DBObjectTypes.Helper.GetObjectTypeName(selTypeID);

  public object GetData(Type dataFormat) => (object) null;

  public ConditionStructure[] TopConditions
  {
    get
    {
      if (this._topConditions == null)
      {
        List<ConditionStructure> conditionStructureList = new List<ConditionStructure>(3);
        if (this.typeID != -1)
          conditionStructureList.AddRange((IEnumerable<ConditionStructure>) new ConditionStructure[1]
          {
            new ConditionStructure(MetaDataHelper.GetAttributeTypeID(PortalConsts.attributePortalObjectTypes), RelationalOperators.Equal, (object) this._typeGuid.ToString(), (object) null, LogicalOperators.NONE, 0, false)
          });
        this._topConditions = conditionStructureList.ToArray();
      }
      return this._topConditions;
    }
  }

  public ConditionStructure[] GetConditions(long selObjectID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      return ((ISelectionsService) ServicesManager.GetService(typeof (ISelectionsService))).GetConditionStructures((object) sessionKeeper.Session, selObjectID);
  }

  public virtual INodePart GetPart(IConditionsProvider conditionProvider)
  {
    return (INodePart) new ContainsPart((IServiceProvider) null, conditionProvider, this.typeID);
  }

  public string ViewCaption { get; } = string.Empty;

  public BindingType BindingType => BindingType.Selections;
}
