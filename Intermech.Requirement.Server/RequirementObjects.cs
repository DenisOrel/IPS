// Decompiled with JetBrains decompiler
// Type: Intermech.Requirement.Server.RequirementObjects
// Assembly: Intermech.Requirement.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C85D341A-B4CB-4985-9EA3-68BB7F9530D7
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Requirement.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Compositions;
using Intermech.Kernel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace Intermech.Requirement.Server;

public class RequirementObjects(UserSession uSession, DataTable objectParams) : DBObject(uSession, objectParams)
{
  public override AttributeValues[] GetAttributesValues(GetAttributeValuesModes modes)
  {
    long load = 0;
    ICompositionLoadService customService = this.UserSession.GetCustomService(typeof (ICompositionLoadService)) as ICompositionLoadService;
    int defaultRelationTypeId = MetaDataHelper.GetDefaultRelationTypeID(this.ObjectType);
    if (customService != null)
      load = customService.FindCompositionParentObject((object) this.UserSession.SessionGUID, this.ObjectID, defaultRelationTypeId, string.Empty);
    AttributeValues[] attributesValues = base.GetAttributesValues(GetAttributeValuesModes.CheckWriteAccess | modes);
    foreach (AttributeValues attributeValues in attributesValues)
    {
      if (attributeValues.AttributeID == MetaDataHelper.GetAttributeID((object) new Guid(ServerConst.AttrContents)))
        attributeValues.ReadOnly = this.CheckObjectIsTZ(load, customService, defaultRelationTypeId);
      else if (attributeValues.AttributeID == MetaDataHelper.GetAttributeID((object) ServerConst.RequirementNameAttrGuid))
        attributeValues.ReadOnly = this.CheckObjectIsTZ(load, customService, defaultRelationTypeId);
      else if (attributeValues.AttributeID == MetaDataHelper.GetAttributeID((object) ServerConst.AttrIndexRequirement))
        attributeValues.ReadOnly = this.CheckObjectIsTZ(load, customService, defaultRelationTypeId);
    }
    return attributesValues;
  }

  public override AttributeValues[] SetAttributesValues(
    AttributeValues[] valuesList,
    bool deleteNotExistingAttributes,
    bool dontDeleteBlobs,
    bool returnDelta,
    GetAttributeValuesModes modes)
  {
    AttributeValues attributeValues1 = ((IEnumerable<AttributeValues>) valuesList).FirstOrDefault<AttributeValues>((System.Func<AttributeValues, bool>) (x => x.AttributeID == MetaDataHelper.GetAttributeID((object) new Guid("cad00020-306c-11d8-b4e9-00304f19f545"))));
    AttributeValues attributeValues2 = ((IEnumerable<AttributeValues>) valuesList).FirstOrDefault<AttributeValues>((System.Func<AttributeValues, bool>) (x => x.AttributeID == MetaDataHelper.GetAttributeID((object) ServerConst.RequirementNameAttrGuid)));
    if (attributeValues2 != null)
    {
      if (attributeValues1 != null)
        attributeValues1.Values = attributeValues2.Values;
      else
        this.Attributes.FindByGUID(new Guid("cad00020-306c-11d8-b4e9-00304f19f545")).Values = attributeValues2.Values;
    }
    return base.SetAttributesValues(valuesList, deleteNotExistingAttributes, dontDeleteBlobs, returnDelta, modes);
  }

  private bool CheckObjectIsTZ(long load, ICompositionLoadService loadService, int relTypeID)
  {
    if (load == 0L)
      return false;
    IDBObject dbObject = this.UserSession.GetObject(load);
    if (dbObject.ObjectType == MetaDataHelper.GetObjectTypeID(ServerConst.SpecificationGuid))
    {
      IDBAttribute[] attributesByType = dbObject.Attributes.GetAttributesByType(FieldTypes.ftFile);
      return attributesByType.Length != 0 && !string.IsNullOrEmpty(attributesByType[0].AsString);
    }
    load = loadService.FindCompositionParentObject((object) this.UserSession.SessionGUID, dbObject.ObjectID, relTypeID, string.Empty);
    return this.CheckObjectIsTZ(load, loadService, relTypeID);
  }
}
