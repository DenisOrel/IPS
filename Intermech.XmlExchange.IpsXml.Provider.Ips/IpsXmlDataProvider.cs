// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Provider.Ips.IpsXmlDataProvider
// Assembly: Intermech.XmlExchange.IpsXml.Provider.Ips, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B2EE3099-B947-440E-865D-611E406056AB
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Provider.Ips.dll

using Intermech.Interfaces;
using Intermech.Interfaces.TechCard;
using Intermech.Interfaces.XmlExchange;
using Intermech.IpsXmlViewer.Interfaces;
using Intermech.XmlExchange.IpsXml.Interfaces;
using Intermech.XmlExchange.IpsXml.Interfaces.Provider;
using Intermech.XmlExchange.IpsXml.Provider.Ips.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using XmlReaderAPI.Data;
using XmlReaderAPI.MetaData;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Provider.Ips;

public class IpsXmlDataProvider : BaseXmlDataProvider, IXmlDataProvider
{
  private IDictionary<string, IImXmlObject> _objects = (IDictionary<string, IImXmlObject>) new Dictionary<string, IImXmlObject>();
  private IDictionary<int, IImObjectType> _objectTypes = (IDictionary<int, IImObjectType>) new Dictionary<int, IImObjectType>();
  private IList<IImRelation> _relations = (IList<IImRelation>) new List<IImRelation>();
  private IDictionary<string, List<IImXmlRelation>> _objectsChildRels = (IDictionary<string, List<IImXmlRelation>>) new Dictionary<string, List<IImXmlRelation>>();
  private IDictionary<string, List<IImXmlRelation>> _objectsParentRels = (IDictionary<string, List<IImXmlRelation>>) new Dictionary<string, List<IImXmlRelation>>();
  private IDictionary<int, IImRelationType> _relationTypes = (IDictionary<int, IImRelationType>) new Dictionary<int, IImRelationType>();
  private IDictionary<int, IImAttributeType> _attrTypesByAttrId = (IDictionary<int, IImAttributeType>) new Dictionary<int, IImAttributeType>();
  private IDictionary<string, IImAttributeType> _attrTypesByAttrName = (IDictionary<string, IImAttributeType>) new Dictionary<string, IImAttributeType>();
  private IReadOnlyCollection<IImXmlObject> _rootObjects;

  public IEnumerable<IXmlObject> Load(string[] fileNames)
  {
    this.ClearCaches();
    Parallel.ForEach<string>((IEnumerable<string>) fileNames, (Action<string>) (curFileName => this.LoadFromXMLFile(curFileName, new IpsXmlDataProvider.OnVisitXMLNode(this.OnLoadData))));
    IImAttributeType sortAttr;
    if (this._attrTypesByAttrId.TryGetValue(TechCardConsts.AttributeTypes.SortAttrTypeID, out sortAttr))
      Parallel.ForEach<KeyValuePair<string, List<IImXmlRelation>>>((IEnumerable<KeyValuePair<string, List<IImXmlRelation>>>) this._objectsChildRels, (Action<KeyValuePair<string, List<IImXmlRelation>>>) (objRelationData => objRelationData.Value.Sort((Comparison<IImXmlRelation>) ((left, right) =>
      {
        object asObject1 = left.GetAsObject(sortAttr.DictAttrKey, (object) null);
        object asObject2 = right.GetAsObject(sortAttr.DictAttrKey, (object) null);
        long num1 = 0;
        long num2 = 0;
        if (asObject1 != null)
          num1 = (asObject1 as IImAttribute).GetAsInt64("F_VALUE", 0L);
        if (asObject2 != null)
          num2 = (asObject2 as IImAttribute).GetAsInt64("F_VALUE", 0L);
        if (num1 > num2)
          return 1;
        return num1 == num2 ? 0 : -1;
      }))));
    IImObject targetObj;
    this._rootObjects = (IReadOnlyCollection<IImXmlObject>) this._objects.Values.Where<IImXmlObject>((Func<IImXmlObject, bool>) (objData => (IImXmlObject) (targetObj = (IImObject) objData) != null && targetObj.IsHead())).ToList<IImXmlObject>();
    return (IEnumerable<IXmlObject>) this._rootObjects;
  }

  public override IReadOnlyCollection<IXmlObject> RootObjects
  {
    get => (IReadOnlyCollection<IXmlObject>) this._rootObjects;
  }

  public override IReadOnlyCollection<IXmlObject> GetAllObjects()
  {
    return (IReadOnlyCollection<IXmlObject>) this._objects.Values.ToList<IImXmlObject>();
  }

  public override IXmlObject GetRelParentObj(IXmlRelation rel)
  {
    return (IXmlObject) this._objects[(rel as IImRelation).GetAsString("F_PROJ_OBJ", "")];
  }

  public override IXmlObject GetRelChildObj(IXmlRelation rel)
  {
    return (IXmlObject) this._objects[(rel as IImRelation).GetAsString("F_PART_OBJ", "")];
  }

  public override IReadOnlyCollection<IXmlRelation> GetObjChildRelations(IXmlObject obj)
  {
    List<IImXmlRelation> imXmlRelationList;
    return this._objectsChildRels.TryGetValue((obj as IImObject).F_OBJECT_ID, out imXmlRelationList) ? (IReadOnlyCollection<IXmlRelation>) imXmlRelationList : (IReadOnlyCollection<IXmlRelation>) null;
  }

  public override IReadOnlyCollection<IXmlRelation> GetObjParentRelations(IXmlObject obj)
  {
    List<IImXmlRelation> imXmlRelationList;
    return this._objectsParentRels.TryGetValue((obj as IImObject).F_OBJECT_ID, out imXmlRelationList) ? (IReadOnlyCollection<IXmlRelation>) imXmlRelationList : (IReadOnlyCollection<IXmlRelation>) null;
  }

  public IImXmlObject FindObjectById(string objId)
  {
    IImXmlObject imXmlObject;
    return this._objects.TryGetValue(objId, out imXmlObject) ? imXmlObject : (IImXmlObject) null;
  }

  public IImObjectType GetObjType(IImObject obj)
  {
    IImObjectType imObjectType;
    return this._objectTypes.TryGetValue(obj.GetAsInt32("F_OBJECT_TYPE", 0), out imObjectType) ? imObjectType : (IImObjectType) null;
  }

  public IImRelationType GetRelType(IImRelation relation)
  {
    IImRelationType imRelationType;
    return this._relationTypes.TryGetValue(relation.GetAsInt32("F_RELATION_TYPE", 0), out imRelationType) ? imRelationType : (IImRelationType) null;
  }

  public IImAttributeType GetAttrType(IImAttribute attribute)
  {
    IImAttributeType imAttributeType;
    return this._attrTypesByAttrId.TryGetValue(attribute.GetAsInt32("F_ATTRIBUTE_ID", 0), out imAttributeType) ? imAttributeType : (IImAttributeType) null;
  }

  public IImAttributeType GetAttrType(string attrName)
  {
    IImAttributeType imAttributeType;
    return this._attrTypesByAttrName.TryGetValue(attrName, out imAttributeType) ? imAttributeType : (IImAttributeType) null;
  }

  public IEnumerable<IImObjectType> GetAllObjTypes()
  {
    return (IEnumerable<IImObjectType>) this._objectTypes.Values;
  }

  public IEnumerable<IImRelationType> GetAllRelTypes()
  {
    return (IEnumerable<IImRelationType>) this._relationTypes.Values;
  }

  public IEnumerable<IImAttributeType> GetAllAttrTypes()
  {
    return (IEnumerable<IImAttributeType>) this._attrTypesByAttrId.Values;
  }

  protected IEnumerable<IImRelation> GetAllRelations()
  {
    return (IEnumerable<IImRelation>) this._relations;
  }

  protected void InternalAddObject(ImObject obj)
  {
    this._objects[obj.F_OBJECT_ID] = (IImXmlObject) obj;
    this.InternalAddObjType(obj.GetAsInt32("F_OBJECT_TYPE", 0));
  }

  protected void InternalAddRelation(ImRelation relation, bool addRelType)
  {
    this._relations.Add((IImRelation) relation);
    if (addRelType)
      this.InternalAddRelationType(relation.GetAsInt32("F_RELATION_TYPE", 0));
    string asString1 = relation.GetAsString("F_PROJ_OBJ", "");
    if (asString1 == string.Empty)
      return;
    string asString2 = relation.GetAsString("F_PART_OBJ", "");
    if (asString2 == string.Empty)
      return;
    List<IImXmlRelation> imXmlRelationList;
    if (!this._objectsChildRels.TryGetValue(asString1, out imXmlRelationList))
    {
      imXmlRelationList = new List<IImXmlRelation>();
      this._objectsChildRels.Add(asString1, imXmlRelationList);
    }
    imXmlRelationList.Add((IImXmlRelation) relation);
    if (!this._objectsParentRels.TryGetValue(asString2, out imXmlRelationList))
    {
      imXmlRelationList = new List<IImXmlRelation>();
      this._objectsParentRels.Add(asString2, imXmlRelationList);
    }
    imXmlRelationList.Add((IImXmlRelation) relation);
  }

  protected void InternalAddAttribute(IImDataElement target, ImAttribute attr)
  {
    target.Attributes[attr.DictAttrKey] = (object) attr;
    this.InternalAddAttributeType(int.Parse(attr.F_ATTRIBUTE_ID));
  }

  protected void ClearCaches()
  {
    this._objects.Clear();
    this._objectTypes.Clear();
    this._relations.Clear();
    this._relationTypes.Clear();
    this._objectsChildRels.Clear();
    this._objectsParentRels.Clear();
    this._attrTypesByAttrId.Clear();
    this._attrTypesByAttrName.Clear();
  }

  private void LoadFromXMLFile(string xmlFileName, IpsXmlDataProvider.OnVisitXMLNode action)
  {
    using (XmlReader xml = XmlReader.Create(xmlFileName))
    {
      while (xml.Read())
        action(xml);
    }
  }

  private void OnLoadData(XmlReader xml)
  {
    if (xml.NodeType != XmlNodeType.Element)
      return;
    switch (xml.Name)
    {
      case "OBJECT":
        ImObject imObject = new ImObject();
        imObject.Load(xml, (IKernel) null);
        this._objects[imObject.F_OBJECT_ID] = (IImXmlObject) imObject;
        break;
      case "RELATION":
        ImRelation relation = new ImRelation();
        relation.Load(xml, (IKernel) null);
        this.InternalAddRelation(relation, false);
        break;
      case "OBJECT_TYPE":
        ImObjectType imObjectType = new ImObjectType();
        imObjectType.Load(xml, (IKernel) null);
        if (this._objectTypes.ContainsKey(imObjectType.F_OBJ_TYPE))
          throw new ArgumentException("Duplicate object type:" + (object) imObjectType.F_OBJ_TYPE);
        this._objectTypes.Add(imObjectType.F_OBJ_TYPE, (IImObjectType) imObjectType);
        break;
      case "RELATION_TYPE":
        ImRelationType imRelationType = new ImRelationType();
        imRelationType.Load(xml, (IKernel) null);
        if (this._relationTypes.ContainsKey(imRelationType.F_RELATION_TYPE))
          throw new ArgumentException("Duplicate relation type:" + (object) imRelationType.F_RELATION_TYPE);
        this._relationTypes.Add(imRelationType.F_RELATION_TYPE, (IImRelationType) imRelationType);
        break;
      case "ATTRIBUTE_TYPE":
        ImAttributeType imAttributeType = new ImAttributeType();
        imAttributeType.Load(xml, (IKernel) null);
        if (this._attrTypesByAttrId.ContainsKey(imAttributeType.F_ATTRIBUTE_ID))
          throw new ArgumentException("Duplicate attribute type:" + (object) imAttributeType.F_ATTRIBUTE_ID);
        this._attrTypesByAttrId.Add(imAttributeType.F_ATTRIBUTE_ID, (IImAttributeType) imAttributeType);
        if (this._attrTypesByAttrName.ContainsKey(imAttributeType.F_NAME))
          throw new ArgumentException("Duplicate attribute type name:" + imAttributeType.F_NAME);
        this._attrTypesByAttrName.Add(imAttributeType.F_NAME, (IImAttributeType) imAttributeType);
        break;
    }
  }

  private void InternalAddObjType(int objectTypeId)
  {
    if (this._objectTypes.ContainsKey(objectTypeId))
      return;
    IMSObjectType objectType = MetaDataHelper.GetObjectType(objectTypeId);
    ImObjectType imObjectType = new ImObjectType();
    if (objectType != null)
    {
      imObjectType.SetAsInt32(XmlExchangeConsts.XML.F_OBJ_TYPE, objectType.ObjectTypeID);
      imObjectType.SetAsString("F_OBJ_TYPE_NAME", objectType.ObjectName);
      imObjectType.SetAsGuid("F_GUID", objectType.Guid);
    }
    else
      imObjectType.SetAsInt32("F_OBJECT_TYPE", objectTypeId);
    this._objectTypes.Add(objectTypeId, (IImObjectType) imObjectType);
  }

  private void InternalAddRelationType(int relationTypeId)
  {
    if (this._relationTypes.ContainsKey(relationTypeId))
      return;
    IMSRelationType relationType = MetaDataHelper.GetRelationType(relationTypeId);
    ImRelationType imRelationType = new ImRelationType();
    if (relationType != null)
    {
      imRelationType = new ImRelationType(relationType.RelationTypeID, relationType.Description, relationType.Guid);
      imRelationType.SetAsInt32("F_RELATION_TYPE", relationType.RelationTypeID);
      imRelationType.SetAsString("F_TYPE_NAME", relationType.Description);
      imRelationType.SetAsGuid("F_GUID", relationType.Guid);
    }
    else
      imRelationType.SetAsInt32("F_RELATION_TYPE", relationTypeId);
    this._relationTypes.Add(relationTypeId, (IImRelationType) imRelationType);
  }

  private void InternalAddAttributeType(int attributeTypeId)
  {
    if (this._attrTypesByAttrId.ContainsKey(attributeTypeId))
      return;
    IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(attributeTypeId);
    ImAttributeType imAttributeType = new ImAttributeType();
    if (attributeType != null)
    {
      imAttributeType.SetAsInt32("F_ATTRIBUTE_ID", attributeType.AttributeID);
      imAttributeType.SetAsInt32("F_ATTRIBUTE_TYPE", (int) attributeType.FieldType);
      imAttributeType.SetAsString("F_NAME", attributeType.Name);
      imAttributeType.SetAsString("F_ALIAS", attributeType.Alias);
      imAttributeType.SetAsGuid("F_GUID", attributeType.AttributeGuid);
    }
    else
      imAttributeType.SetAsInt32("F_ATTRIBUTE_ID", attributeTypeId);
    this._attrTypesByAttrId.Add(attributeTypeId, (IImAttributeType) imAttributeType);
  }

  private delegate void OnVisitXMLNode(XmlReader xml);
}
