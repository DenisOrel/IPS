// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.PublishHelper
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace Intermech.Portal.Server;

internal class PublishHelper
{
  public static void CorrectLinks(
    IDBObjectCollection objCollection,
    IDBAttributable dbAttributable,
    Dictionary<Guid, long> importedObjectsIDs)
  {
    IDBAttribute attributeById = dbAttributable.GetAttributeByID(IDHelper.AttributePublishGuidLinksID);
    IDBAttribute dbAttribute = dbAttributable.GetAttributeByID(IDHelper.AttributePublishLinksID);
    if (attributeById == null)
    {
      dbAttribute?.Delete(0L);
    }
    else
    {
      if (dbAttribute == null)
        dbAttribute = dbAttributable.Attributes.AddAttribute(IDHelper.AttributePublishLinksID, false);
      else
        dbAttribute.ClearValues();
      for (int index = 0; index < attributeById.ValuesCount; ++index)
      {
        if (index > 0)
          attributeById.Index = index;
        long newValue = 0;
        if (!importedObjectsIDs.TryGetValue(new Guid(attributeById.AsString), out newValue))
        {
          DataTable dataTable = objCollection.SelectWithLocalObjects(new DBRecordSetParams(new ConditionStructure[1]
          {
            new ConditionStructure(IDHelper.AttributePublishObjectGuidID, RelationalOperators.Equal, (object) attributeById.AsString, LogicalOperators.NONE, 0, false)
          }, new object[1]{ (object) -2 }));
          if (dataTable.Rows.Count > 0)
            newValue = Convert.ToInt64(dataTable.Rows[0][0]);
        }
        if (newValue != 0L)
        {
          if (index == 0)
            dbAttribute.Value = (object) newValue;
          else
            dbAttribute.AddValue((object) newValue);
        }
      }
    }
  }

  public static void AddAtribute(IDBAttributable dbUnit, int attributeID)
  {
    PublishHelper.AddAtribute(dbUnit, attributeID, (object) null);
  }

  public static void AddAtribute(IDBAttributable dbUnit, int attributeID, object value)
  {
    IDBAttribute dbAttribute = dbUnit.GetAttributeByID(attributeID) ?? dbUnit.Attributes.AddAttribute(attributeID, false);
    if (value == null)
      return;
    dbAttribute.Value = value;
  }

  public static void AddUnitFilesToPacket(
    PublishPacket packet,
    TransferedObject unit,
    string unitTempDirectory)
  {
    PublishHelper.AddUnitFilesToPacket(packet, unit, unitTempDirectory, string.Empty);
  }

  public static void AddUnitFilesToPacket(
    PublishPacket packet,
    TransferedObject unit,
    string unitTempDirectory,
    string tag)
  {
    if (packet == null)
      throw new ArgumentException();
    packet.AddData(unit, unitTempDirectory, tag);
  }

  public static void SetCreatorAndOwnerForNewObject(
    IDBAttribute dbAttrOwner,
    IDBAttribute dbAttrCompositionOwner,
    IDBAttribute dbAttrParentSites,
    IDBAttribute dbAttrCompositionParentSites,
    bool isNewObject,
    ObjectTag tag,
    char currentSiteCode)
  {
    if (!isNewObject)
      return;
    dbAttrOwner.AsString = !tag.OwnerCode.HasValue || (int) currentSiteCode == (int) tag.OwnerCode.Value ? currentSiteCode.ToString() : tag.CreatorCode.ToString();
    dbAttrCompositionOwner.AsString = !tag.CompositionOwnerCode.HasValue || (int) currentSiteCode == (int) tag.CompositionOwnerCode.Value ? currentSiteCode.ToString() : tag.CreatorCode.ToString();
    dbAttrParentSites.AsString = dbAttrOwner.AsString;
    dbAttrCompositionParentSites.AsString = dbAttrCompositionOwner.AsString;
    if (!TraceLog.Enabled)
      return;
    TraceLog.Write($"...set dbAttrOwner={dbAttrOwner.AsString} dbAttrParentSites={dbAttrParentSites.AsString}");
    TraceLog.Write($"...set dbAttrCompositionOwner={dbAttrCompositionOwner.AsString} dbAttrCompositionParentSites={dbAttrCompositionParentSites.AsString}");
  }

  public static void SetSiteCodes(
    IDBObject publishObj,
    ObjectTag tag,
    TransferedObjectCategory category,
    string publishEnabledSites,
    string currentSiteCode,
    string site4Update,
    bool isAutoTransfer,
    bool isNewObject,
    bool inComposition)
  {
    if (isNewObject)
      publishObj.GetAttributeByGuid(PortalConsts.attributeFirstPublishSite).AsString = tag.CreatorCode.ToString();
    IDBAttribute attributeByGuid1 = publishObj.GetAttributeByGuid(PortalConsts.attributeEnabledSites);
    IDBAttribute attributeByGuid2 = publishObj.GetAttributeByGuid(PortalConsts.attributeOwner);
    IDBAttribute dbAttribute1 = publishObj.GetAttributeByGuid(PortalConsts.attributeCompositionOwner) ?? publishObj.Attributes.AddAttribute(MetaDataHelper.GetAttributeTypeID(PortalConsts.attributeCompositionOwner), false);
    IDBAttribute dbAttribute2 = publishObj.GetAttributeByGuid(PortalConsts.attributeCompositionParentSites) ?? publishObj.Attributes.AddAttribute(MetaDataHelper.GetAttributeTypeID(PortalConsts.attributeCompositionParentSites), false);
    IDBAttribute attributeByGuid3 = publishObj.GetAttributeByGuid(PortalConsts.attributeParentSites);
    string source = !string.IsNullOrEmpty(tag.EnableSites) ? tag.EnableSites : publishEnabledSites;
    switch (category)
    {
      case TransferedObjectCategory.Object:
        if (!string.IsNullOrEmpty(source))
        {
          if (!source.Contains(currentSiteCode))
            source += currentSiteCode;
        }
        else
          source = currentSiteCode;
        if (!isNewObject)
        {
          string asString = attributeByGuid1.AsString;
          if (!string.IsNullOrEmpty(asString))
          {
            foreach (char ch in asString)
            {
              if (!source.Contains<char>(ch))
                source += ch.ToString();
            }
          }
        }
        attributeByGuid1.AsString = source;
        if (TraceLog.Enabled)
          TraceLog.Write($"...set attrEnableSites={source}");
        if (isAutoTransfer)
        {
          if (TraceLog.Enabled)
            TraceLog.Write("...autoTransfer");
          if (PublishHelper.SetAutoTransferOwnAttribute(attributeByGuid2, attributeByGuid3, currentSiteCode, tag.OwnerCode))
          {
            if (TraceLog.Enabled)
              TraceLog.Write($"...set dbAttrParentSites={attributeByGuid3.AsString} dbAttrOwner={attributeByGuid2.AsString}");
            if (!tag.OwnerCode.HasValue && !string.IsNullOrEmpty(attributeByGuid2.AsString))
              tag.OwnerCode = new char?(attributeByGuid2.AsString[0]);
          }
          if (!PublishHelper.SetAutoTransferOwnAttribute(dbAttribute1, dbAttribute2, currentSiteCode, tag.CompositionOwnerCode))
            break;
          if (TraceLog.Enabled)
            TraceLog.Write($"...set dbAttrCompositionParentSites={dbAttribute2.AsString} dbAttrCompositionOwner={dbAttribute1.AsString}");
          if (tag.CompositionOwnerCode.HasValue || string.IsNullOrEmpty(dbAttribute1.AsString))
            break;
          tag.CompositionOwnerCode = new char?(dbAttribute1.AsString[0]);
          break;
        }
        PublishHelper.SetCreatorAndOwnerForNewObject(attributeByGuid2, dbAttribute1, attributeByGuid3, dbAttribute2, isNewObject, tag, currentSiteCode[0]);
        break;
      case TransferedObjectCategory.ObjectLink:
        string empty = string.Empty;
        if (source.Length > 0)
        {
          for (int index = 0; index < source.Length; ++index)
          {
            if (!attributeByGuid1.AsString.Contains(source[index].ToString()))
              empty += source[index].ToString();
          }
        }
        if (!attributeByGuid1.AsString.Contains(currentSiteCode) && !empty.Contains(currentSiteCode))
          empty += currentSiteCode;
        if (empty != string.Empty)
          attributeByGuid1.AsString += empty;
        PublishHelper.SetCreatorAndOwnerForNewObject(attributeByGuid2, dbAttribute1, attributeByGuid3, dbAttribute2, isNewObject, tag, currentSiteCode[0]);
        if (!TraceLog.Enabled)
          break;
        TraceLog.Write($"...add to attrEnableSites={empty}");
        break;
    }
  }

  public static void SetCompositionOwnerCodes(
    IDBObject publishObj,
    bool isAutoTransfer,
    ObjectTag tag,
    string currentSiteCode)
  {
    if (!isAutoTransfer)
      return;
    IDBAttribute dbAttrOwner = publishObj.GetAttributeByGuid(PortalConsts.attributeCompositionOwner) ?? publishObj.Attributes.AddAttribute(MetaDataHelper.GetAttributeTypeID(PortalConsts.attributeCompositionOwner), false);
    IDBAttribute dbAttrParentSites = publishObj.GetAttributeByGuid(PortalConsts.attributeCompositionParentSites) ?? publishObj.Attributes.AddAttribute(MetaDataHelper.GetAttributeTypeID(PortalConsts.attributeCompositionParentSites), false);
    if (!PublishHelper.SetAutoTransferOwnAttribute(dbAttrOwner, dbAttrParentSites, currentSiteCode, tag.CompositionOwnerCode) || !TraceLog.Enabled)
      return;
    TraceLog.Write($"...set dbAttrCompositionParentSites={dbAttrParentSites.AsString} dbAttrCompositionOwner={dbAttrOwner.AsString}");
  }

  public static bool SetAutoTransferOwnAttribute(
    IDBAttribute dbAttrOwner,
    IDBAttribute dbAttrParentSites,
    string currentSiteCode,
    char? ownCode)
  {
    if ((!PublishHelper.ValueEmpty(dbAttrOwner) || !PublishHelper.ValueEmpty(dbAttrParentSites) && !dbAttrParentSites.AsString.Contains(currentSiteCode)) && !currentSiteCode.Equals(dbAttrOwner.AsString))
      return false;
    string str = !ownCode.HasValue || !ownCode.HasValue ? currentSiteCode.ToString() : ownCode.Value.ToString();
    if (PublishHelper.ValueEmpty(dbAttrParentSites))
      PublishHelper.SetValue(dbAttrParentSites, str);
    PublishHelper.SetValue(dbAttrOwner, str);
    return true;
  }

  private static void SetValue(IDBAttribute attribute, string value) => attribute.AsString = value;

  private static bool ValueEmpty(IDBAttribute attribute)
  {
    return string.IsNullOrEmpty(attribute.AsString);
  }
}
