// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.Selections.PortalConditionDataProvider
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.SelectionService;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel.Search;
using Intermech.Navigator.Conditions;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.SelectionView;
using Intermech.PropertyEditors;
using System;
using System.Collections.Generic;
using System.Drawing.Design;

#nullable disable
namespace Intermech.Site.Client.Selections;

internal sealed class PortalConditionDataProvider : ConditionDataProvider
{
  public override bool AnyAttributes(AttributeSourceTypes sourceType, int[] objectTypeIDs) => false;

  public override string GetAttributeName(object attributeID)
  {
    IPortalMetadata service = ServicesManager.GetService<IPortalMetadata>();
    PortalAttributeType portalAttributeType = (PortalAttributeType) null;
    if (attributeID is Guid attributeGuid)
      portalAttributeType = service.GetAttribute(attributeGuid);
    else if (attributeID is int attributeID1)
      portalAttributeType = service.GetAttribute(attributeID1);
    return portalAttributeType == null ? "Неизвестный атрибут" : portalAttributeType.Name;
  }

  public override FieldTypes GetFieldType(object attributeID)
  {
    switch (attributeID)
    {
      case int num:
        if (num == 0)
          return FieldTypes.ftUnknown;
        return (int) attributeID >= 0 ? MetaDataHelper.GetAttributeType((int) attributeID).FieldType : FieldTypes.ftSystem;
      case Guid aGUID:
        if (SystemGUIDs.IsSystemGUID(aGUID))
          return MetaDataHelper.GetAttributeType((Guid) attributeID).FieldType;
        break;
    }
    PortalAttributeType attribute = ServicesManager.GetService<IPortalMetadata>().GetAttribute((Guid) attributeID);
    return attribute == null ? FieldTypes.ftUnknown : attribute.Type;
  }

  public override List<ConditionAttributeInfo> GetListAttributes(
    AttributeSourceTypes sourceType,
    int[] objectTypeIDs)
  {
    List<ConditionAttributeInfo> obligatoryAttributes = this.GetObligatoryAttributes(sourceType);
    obligatoryAttributes.AddRange(sourceType == AttributeSourceTypes.Relation ? (IEnumerable<ConditionAttributeInfo>) this.GetAttributesForRelationTypes() : (IEnumerable<ConditionAttributeInfo>) this.GetAttributesForObjectTypes(objectTypeIDs));
    return obligatoryAttributes;
  }

  public override List<ConditionAttributeInfo> GetObligatoryAttributes(
    AttributeSourceTypes sourceType)
  {
    List<ConditionAttributeInfo> obligatoryAttributes = new List<ConditionAttributeInfo>();
    if (sourceType == AttributeSourceTypes.Object || sourceType == AttributeSourceTypes.Auto)
    {
      foreach (int obligatoryObjectAttribute in PortalConsts.EnabledObligatoryObjectAttributes)
        obligatoryAttributes.Add(new ConditionAttributeInfo((object) obligatoryObjectAttribute, ObligatoryObjectAttributesHelper.GetCaption((ObligatoryObjectAttributes) obligatoryObjectAttribute), FieldTypes.ftSystem));
    }
    return obligatoryAttributes;
  }

  private List<ConditionAttributeInfo> GetAttributesForRelationTypes()
  {
    PortalAttributeType[] relationAttributes = ServicesManager.GetService<IPortalMetadata>().GetPublishRelationAttributes();
    if (relationAttributes == null)
      return (List<ConditionAttributeInfo>) null;
    List<ConditionAttributeInfo> forRelationTypes = new List<ConditionAttributeInfo>();
    for (int index = 0; index < relationAttributes.Length; ++index)
    {
      if (Array.IndexOf<FieldTypes>(PortalConsts.EnabledFieldTypes, relationAttributes[index].Type) >= 0)
        forRelationTypes.Add(new ConditionAttributeInfo((object) new Guid(relationAttributes[index].GUID), relationAttributes[index].Name, relationAttributes[index].Type));
    }
    return forRelationTypes;
  }

  public override List<ConditionAttributeInfo> GetAttributesForObjectTypes(int[] objTypes)
  {
    if (objTypes == null || objTypes.Length == 0)
      return (List<ConditionAttributeInfo>) null;
    List<ConditionAttributeInfo> attributesForObjectTypes = new List<ConditionAttributeInfo>();
    List<Guid> guidList = new List<Guid>();
    IPortalMetadata service = ServicesManager.GetService<IPortalMetadata>();
    for (int index1 = 0; index1 < objTypes.Length; ++index1)
    {
      PortalObjectType publishObjectType = service.GetPublishObjectType(objTypes[index1]);
      if (publishObjectType.Attributes != null)
      {
        for (int index2 = 0; index2 < publishObjectType.Attributes.Length; ++index2)
        {
          Guid id = new Guid(publishObjectType.Attributes[index2].GUID);
          if (Array.IndexOf<FieldTypes>(PortalConsts.EnabledFieldTypes, publishObjectType.Attributes[index2].Type) >= 0 && !guidList.Contains(id))
          {
            guidList.Add(id);
            attributesForObjectTypes.Add(new ConditionAttributeInfo((object) id, publishObjectType.Attributes[index2].Name, publishObjectType.Attributes[index2].Type));
          }
        }
      }
    }
    return attributesForObjectTypes;
  }

  protected override Guid GetAttributeGuidFromId(int attributeID)
  {
    if (attributeID < 0)
      return MetaDataHelper.GetAttributeTypeGuid(attributeID);
    PortalAttributeType attribute = ServicesManager.GetService<IPortalMetadata>().GetAttribute(attributeID);
    return attribute == null ? Guid.Empty : new Guid(attribute.GUID);
  }

  protected override int GetAttributeIdFromGuid(Guid attributeGuid)
  {
    PortalAttributeType attribute = ServicesManager.GetService<IPortalMetadata>().GetAttribute(attributeGuid);
    return attribute == null ? 0 : attribute.ID;
  }

  public override Dictionary<object, string> GetPossibleValues(object attributeID)
  {
    int attributeId = this.GetAttributeID(attributeID);
    return attributeId <= 0 ? (Dictionary<object, string>) null : ServicesManager.GetService<IPortalMetadata>().GetPossibleValues(attributeId);
  }

  public override bool ChoiseObjectType(ref object objectType, SelectionType selectionType)
  {
    IAttributePropertyDescriberService service = ServicesManager.GetService<IAttributePropertyDescriberService>();
    int attributeTypeId = MetaDataHelper.GetAttributeTypeID(PortalConsts.attributePortalObjectTypes);
    if (attributeTypeId != -10000)
    {
      IAttributePropertyDescriber describer = service.GetDescriber(attributeTypeId);
      if (describer != null && describer.GetPropDescriptorEditor(attributeTypeId) is UITypeEditor descriptorEditor && descriptorEditor.GetEditStyle() == UITypeEditorEditStyle.Modal)
      {
        object obj = descriptorEditor.EditValue((IServiceProvider) null, objectType);
        int num = -1;
        if (objectType is int)
          num = (int) objectType;
        else if (objectType is PublishTypeAttProxy)
          num = ((PublishTypeAttProxy) objectType).ID;
        if (obj is PublishTypeAttProxy && ((PublishTypeAttProxy) obj).ID != num)
        {
          objectType = obj;
          return true;
        }
      }
    }
    return false;
  }

  public override string GetObjectTypeCaption(object value)
  {
    PortalObjectType portalObjectType = (PortalObjectType) null;
    IPortalMetadata service = ServicesManager.GetService<IPortalMetadata>();
    switch (value)
    {
      case int typeID:
        portalObjectType = service.GetPublishObjectType(typeID);
        break;
      case Guid typeGuid:
        portalObjectType = service.GetPublishObjectType(typeGuid);
        break;
      case PublishTypeAttProxy _:
        portalObjectType = service.GetPublishObjectType(((PublishTypeAttProxy) value).ID);
        break;
    }
    return portalObjectType == null ? $"Тип объектов {value}" : portalObjectType.Name;
  }

  public override string GenerateConditionCaption(
    ConditionStructure conditionStructure,
    string value1,
    string value2)
  {
    string str1 = string.Empty;
    string str2 = string.Empty;
    string empty = string.Empty;
    object attribute1 = conditionStructure.Attribute;
    if (attribute1 != null && attribute1 is Guid guid && !guid.Equals(Guid.Empty))
    {
      string name;
      if (SystemGUIDs.IsSystemGUID((Guid) attribute1))
      {
        IMSAttributeType attributeType = MetaDataHelper.GetAttributeType((Guid) attribute1);
        name = attributeType.Name;
        IConditionEditorAttribute handler = ((IConditionEditorAttributeService) ServicesManager.GetService(typeof (IConditionEditorAttributeService))).GetHandler((Guid) attribute1);
        SelectionParameterTypes selParType = handler == null ? SelectionParameter.GetNodeValueType(attributeType.AttributeID, attributeType.RealFieldType) : handler.NodeValueType;
        str2 = this.ConvertToString(attribute1, conditionStructure.RelationalOperator, selParType, conditionStructure.Value, (Dictionary<object, string>) null, conditionStructure.TypeID);
        empty = this.ConvertToString(attribute1, conditionStructure.RelationalOperator, selParType, conditionStructure.Value2, (Dictionary<object, string>) null, conditionStructure.TypeID);
      }
      else
      {
        IPortalMetadata service = ServicesManager.GetService<IPortalMetadata>();
        PortalAttributeType attribute2 = service.GetAttribute((Guid) attribute1);
        if (attribute2 != null)
        {
          name = attribute2.Name;
          Dictionary<object, string> possibleValues = service.GetPossibleValues(attribute2.ID);
          SelectionParameterTypes nodeValueType = SelectionParameter.GetNodeValueType(attribute2.ID, attribute2.Type);
          str2 = this.ConvertToString(attribute1, conditionStructure.RelationalOperator, nodeValueType, conditionStructure.Value, possibleValues, conditionStructure.TypeID);
          empty = this.ConvertToString(attribute1, conditionStructure.RelationalOperator, nodeValueType, conditionStructure.Value2, possibleValues, conditionStructure.TypeID);
        }
        else
          name = Convert.ToString(attribute1);
      }
      str1 = $"\"{name}\" ";
    }
    else if (attribute1 != null && attribute1 is int num && num < 0)
    {
      str1 = EnumDescConverter.GetEnumDescription((Enum) (ObligatoryObjectAttributes) attribute1) + " ";
      SelectionParameterTypes nodeValueType = SelectionParameter.GetNodeValueType((int) attribute1, FieldTypes.ftSystem);
      str2 = this.ConvertToString(attribute1, conditionStructure.RelationalOperator, nodeValueType, conditionStructure.Value, (Dictionary<object, string>) null, conditionStructure.TypeID);
      empty = this.ConvertToString(attribute1, conditionStructure.RelationalOperator, nodeValueType, conditionStructure.Value2, (Dictionary<object, string>) null, conditionStructure.TypeID);
    }
    else if (SelectionParameter.IsInRelationOpr(conditionStructure.RelationalOperator))
      str2 = this.GetObjectTypeCaption(conditionStructure.Value);
    else if (conditionStructure.RelationalOperator == RelationalOperators.ObjectTypeFilter)
      str2 = this.GetObjectTypeCaption(conditionStructure.Value);
    return conditionStructure.RelationalOperator != RelationalOperators.Between ? str1 + RelationalOperatorsHelper.GetCaption(conditionStructure.RelationalOperator).ToLower() + (str2 != string.Empty ? $" \"{str2}\"" : string.Empty) + (empty != string.Empty ? $"и \"{empty}\"" : string.Empty) : str1 + RelationalOperatorsHelper.GetCaption(conditionStructure.RelationalOperator).ToLower() + $"от {str2} до {empty} ";
  }

  public override bool SelectDialog(
    ref object value,
    SelectionParameterTypes type,
    object addInfo,
    int attrID,
    int[] selection4Types)
  {
    switch (type)
    {
      case SelectionParameterTypes.sptSiteID:
        return this.SelectSitesDialog(ref value);
      case SelectionParameterTypes.sptObjectType:
        return this.ChoiseObjectType(ref value, SelectionType.ObjectType);
      case SelectionParameterTypes.sptGlobalID:
        return ValueRelationSelector.SelectVersionsGuid(ref value);
      default:
        return false;
    }
  }

  public override int GetObjectType4ObjectLink(int attributeID) => -1;

  public override int GetObjectTypeID(Guid objectTypeGuid)
  {
    PortalObjectType publishObjectType = ServicesManager.GetService<IPortalMetadata>().GetPublishObjectType(objectTypeGuid);
    return publishObjectType == null ? -1 : publishObjectType.ID;
  }

  public override RelationalOperators[] GetEnableRelationalOperators(
    FieldTypes fieldType,
    int attributeID)
  {
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    string empty3 = string.Empty;
    List<FieldTypes> convertList = new List<FieldTypes>();
    bool computableAttribute = false;
    RelationalOperators[] enabledOperators = (RelationalOperators[]) null;
    AttributeCacheHelper.GetAttributeTypeValues(fieldType, attributeID, ref empty1, ref empty2, ref convertList, ref enabledOperators, ref computableAttribute, ref empty3);
    return enabledOperators;
  }

  public override List<SelectionParameterTypes> EnabledParameterTypes
  {
    get
    {
      if (this.enabledParameterTypes == null)
      {
        this.ReloadEnabledParameterTypes();
        this.enabledParameterTypes.Remove(SelectionParameterTypes.sptUser);
        this.enabledParameterTypes.Remove(SelectionParameterTypes.sptObject);
      }
      return this.enabledParameterTypes;
    }
  }
}
