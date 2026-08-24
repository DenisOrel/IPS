// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.DataWriter.IDataWriterProxy
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface.DataWriter;
using System;

#nullable disable
namespace Intermech.ImpExp.Manager.DataWriter;

internal interface IDataWriterProxy
{
  Guid NewPumpGuid();

  IAttributeTypeItem CreateAttributeType(
    string name,
    string shortName,
    string alias,
    string note,
    FieldTypes fieldType,
    string defVal,
    MultiValueModes multiMode,
    ComputeValueModes computeMode,
    UniqueValueModes uniqueMode,
    long size,
    int level,
    string formula,
    string language,
    Guid guid,
    string area,
    bool isContent,
    short inView,
    AttributeOptions options,
    string mask,
    int groupID);

  IObjectTypeItem CreateObjectType(
    Guid parentID,
    string name,
    string objectName,
    string shortName,
    ObjectVersionModes versionable,
    string note,
    Guid defRelId,
    Guid guid,
    string area,
    int captionAttribute,
    bool anyAttributes,
    lcType publicLc,
    int delTime,
    Guid shemaId,
    byte[] icon);

  IAttributeGroupItem CreateAttributeGroup(
    string groupName,
    Guid groupGuid,
    string note,
    string area,
    string lang);

  void CreateAttributePossibleValue(int attrId, IAttributePossibleValue possibleValue);

  void CreateLinkAttrTypeToRelType(
    int attrTypeId,
    int relTypeId,
    RequiredModes requiredMod,
    string validationRule,
    ComputeValueModes computeMode,
    string formula,
    string defaultValue,
    short inViewMode,
    bool isContent,
    AttributeOptions options,
    string mask,
    int masterId,
    int sourceId);

  void CreateLinkAttrTypeToObjType(
    int attrTypeId,
    int objTypeId,
    bool isPublic,
    RequiredModes requiredMod,
    string validationRule,
    ComputeValueModes computeMode,
    string formula,
    UniqueValueModes uniqueMode,
    int level,
    string defaultValue,
    OptimizationModes inViewMode,
    bool isContent,
    AttributeOptions options,
    string mask,
    int masterId,
    int sourceId);

  void CreateLinkAttrTypeToGroup(int attrTypeId, Guid attrTypeGuid, int attrGroupId);

  long GetNextID(string tableName);
}
