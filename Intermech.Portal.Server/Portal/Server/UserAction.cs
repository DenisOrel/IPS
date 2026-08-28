// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.UserAction
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel.Search;
using Intermech.Localization;
using Intermech.Protection;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml;

#nullable disable
namespace Intermech.Portal.Server;

internal sealed class UserAction : PortalAction
{
  public void ChangeUserPassword(Guid sessionGuid, string login, string newPassword)
  {
    new StringPasswordUserCreator().ChangeUserPassword(this.GetUserSession(sessionGuid), login, newPassword);
  }

  public void ChangeUserPassword(Guid sessionGuid, string login, PswPackage newPassword)
  {
    new PswPackagePasswordUserCreator().ChangeUserPassword(this.GetUserSession(sessionGuid), login, newPassword);
  }

  public void AddUser(
    Guid sessionGuid,
    string userName,
    string login,
    PswPackage password,
    Guid userGuid)
  {
    new PswPackagePasswordUserCreator().AddUser(this.GetUserSession(sessionGuid), userName, login, password, userGuid);
  }

  public void AddUser(
    Guid sessionGuid,
    string userName,
    string login,
    string password,
    Guid userGuid)
  {
    new StringPasswordUserCreator().AddUser(this.GetUserSession(sessionGuid), userName, login, password, userGuid);
  }

  public void DeleteUser(Guid sessionGuid, string login)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start DeleteUser login={login} sessionGuid={sessionGuid}");
    IUserSession userSession = this.GetUserSession(sessionGuid);
    SiteInfo siteInfo = this.GetSiteInfo(userSession);
    if (TraceLog.Enabled)
      TraceLog.Write($"..info={siteInfo.Code}");
    long objectID = UserAction.CheckPresentUser(userSession, siteInfo, login);
    userSession.GetObject(objectID).Delete(0L);
    if (!TraceLog.Enabled)
      return;
    TraceLog.Write($"End DeleteUser info={siteInfo.Code} login={login}");
  }

  public PublishObjectsTable GetSiteUsers(
    Guid sessionGuid,
    string siteGuid,
    DBQueryParams dbParams)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start GetSiteUsers siteGuid={siteGuid} sessionGuid={sessionGuid}");
    IUserSession userSession = this.GetUserSession(sessionGuid);
    if (!userSession.IsAdmin)
      throw new Exception(LocalizationHolder.rm.GetString("PortalServer_65"));
    DBRecordSetParams dbRecordSetParams = DBQueryParams.UnformingParams(dbParams);
    if (dbRecordSetParams.Columns == null || dbRecordSetParams.Columns.Length == 0)
      throw new Exception(LocalizationHolder.rm.GetString("PortalServer_50"));
    IDBObject dbObject = userSession.GetObject(new Guid(siteGuid), true);
    IDBRelationCollection relationCollection = userSession.GetRelationCollection(userSession.IdentHelper.SimpleRelationTypeID);
    if (TraceLog.Enabled)
      TraceLog.Write($"End GetSiteUsers siteGuid={siteGuid}");
    DBRecordSetParams paramSet = dbRecordSetParams;
    long objectId = dbObject.ObjectID;
    return new PublishObjectsTable(relationCollection.ConsistFrom(paramSet, objectId));
  }

  public void ImportUsers(Guid sessionGuid, Guid updateGuid, long[] userIDs)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start ImportUsers updateGuid={updateGuid} sessionGuid={sessionGuid}");
    IUserSession userSession = this.GetUserSession(sessionGuid);
    if (!userSession.IsAdmin)
      throw new Exception(LocalizationHolder.rm.GetString("PortalServer_65"));
    if (userIDs == null || userIDs.Length == 0)
      throw new Exception(LocalizationHolder.rm.GetString("PortalServer_66"));
    SiteInfo siteInfo = this.GetSiteInfo(userSession);
    IDBRelationCollection relationCollection = userSession.GetRelationCollection(userSession.IdentHelper.SimpleRelationTypeID);
    relationCollection.ObjectTypeID = MetaDataHelper.GetObjectTypeID(PortalConsts.objtypeSites);
    DBRecordSetParams paramSet = new DBRecordSetParams((ConditionStructure[]) null, new ColumnDescriptor[2]
    {
      new ColumnDescriptor((object) -2, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Default, SortOrders.NONE, 0),
      new ColumnDescriptor((object) MetaDataHelper.GetAttributeTypeID(PortalConsts.attributeSiteCode), AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Default, SortOrders.NONE, 0)
    });
    List<TransferedObject> data = new List<TransferedObject>(userIDs.Length);
    for (int index = 0; index < userIDs.Length; ++index)
    {
      IDBObject user = userSession.GetObject(userIDs[index], true);
      DataTable dataTable = relationCollection.EntersInVersion(paramSet, user.ObjectID);
      TransferedObject transferedObject = new TransferedObject(ChangeType.ctUpdate, TransferedObjectCategory.Object);
      if (dataTable.Rows.Count == 1)
      {
        char ch = Convert.ToChar(dataTable.Rows[0][1]);
        transferedObject.Tag = (TransferedObjectTag) new ObjectTag(false, false, user.SiteID != string.Empty ? user.SiteID[0] : ch, PublishObjectRootType.rtUnknown);
        ((ObjectTag) transferedObject.Tag).OwnerCode = new char?(user.SiteID.Length >= 2 ? user.SiteID[1] : ch);
        ((ObjectTag) transferedObject.Tag).CompositionOwnerCode = new char?(user.SiteID.Length >= 3 ? user.SiteID[2] : ch);
        string updateUnitPath = TempStorage.GetUpdateUnitPath(transferedObject.GUID);
        Directory.CreateDirectory(updateUnitPath);
        XmlDocument xmlDocument1 = new XmlDocument();
        xmlDocument1.AppendChild((XmlNode) xmlDocument1.CreateXmlDeclaration("1.0", (string) null, (string) null));
        XmlNode element1 = (XmlNode) xmlDocument1.CreateElement(PortalConsts.XmlRootNodeAttributes);
        XmlNode element2 = (XmlNode) xmlDocument1.CreateElement(PortalConsts.XmlNodeSysAttribute);
        XmlDocument xmlDocument2 = xmlDocument1;
        XmlNode node1 = element2;
        Guid guid = user.GUID;
        string str1 = guid.ToString();
        this.AddAttribute(xmlDocument2, node1, "F_GUID", str1);
        XmlDocument xmlDocument3 = xmlDocument1;
        XmlNode node2 = element2;
        guid = user.ObjectGUID;
        string str2 = guid.ToString();
        this.AddAttribute(xmlDocument3, node2, "F_OBJECT_GUID", str2);
        this.AddAttribute(xmlDocument1, element2, "F_OBJTYPE_GUID", "cad00002-306c-11d8-b4e9-00304f19f545");
        this.AddAttribute(xmlDocument1, element2, "F_ACCESS", user.AccessLevel.ToString());
        this.AddAttribute(xmlDocument1, element2, "CAPTION", user.Caption);
        element1.AppendChild(element2);
        this.AddUserAttribute(xmlDocument1, element1, user, new Guid("cad0001d-306c-11d8-b4e9-00304f19f545"));
        this.AddUserAttribute(xmlDocument1, element1, user, new Guid("cad00018-306c-11d8-b4e9-00304f19f545"));
        this.AddUserAttribute(xmlDocument1, element1, user, new Guid("cad00019-306c-11d8-b4e9-00304f19f545"));
        xmlDocument1.AppendChild(element1);
        IPackedStream service = ServiceUtils.GetService<IPackedStream>((object) ApplicationServices.Container, true);
        using (MemoryStream memoryStream = new MemoryStream())
        {
          xmlDocument1.Save((Stream) memoryStream);
          memoryStream.Position = 0L;
          using (FileStream outStream = File.Create(Path.Combine(updateUnitPath, PortalConsts.AttributesXmlFileName)))
          {
            service.PackStream((Stream) outStream, (Stream) memoryStream, 9);
            outStream.Flush();
            transferedObject.DataFiles = new string[1]
            {
              PortalConsts.AttributesXmlFileName
            };
          }
        }
        data.Add(transferedObject);
      }
      else
      {
        if (dataTable.Rows.Count == 0)
          throw new Exception(string.Format(LocalizationHolder.rm.GetString("PortalServer_67"), (object) user.NameInMessages));
        throw new Exception(string.Format(LocalizationHolder.rm.GetString("PortalServer_68"), (object) user.NameInMessages));
      }
    }
    new SiteUpdate(data, new long[1]{ siteInfo.ID }, siteInfo.Code.ToString()).SaveIntoBase(userSession, updateGuid);
    if (!TraceLog.Enabled)
      return;
    TraceLog.Write($"End ImportUsers site={siteInfo.Code} updateGuid={updateGuid}");
  }

  public bool IsAdmin(Guid sessionGuid)
  {
    if (TraceLog.Enabled)
      TraceLog.Write($"Start IsAdmin sessionGuid={sessionGuid}");
    IUserSession userSession = this.GetUserSession(sessionGuid);
    if (TraceLog.Enabled)
      TraceLog.Write($"End IsAdmin sessionGuid={sessionGuid}");
    return userSession.IsAdmin;
  }

  private void AddUserAttribute(
    XmlDocument xmlDocument,
    XmlNode xmlRootNode,
    IDBObject user,
    Guid attributeGuid)
  {
    IDBAttribute attributeByGuid = user.GetAttributeByGuid(attributeGuid);
    XmlNode element1 = (XmlNode) xmlDocument.CreateElement(PortalConsts.XmlNodeAttribute);
    this.AddAttribute(xmlDocument, element1, "F_GUID", attributeGuid.ToString());
    this.AddAttribute(xmlDocument, element1, "F_NAME", attributeByGuid.AttributeType.Name);
    this.AddAttribute(xmlDocument, element1, "F_ATTRIBUTE_TYPE", Convert.ToString((int) attributeByGuid.AttributeType.AttributeType));
    XmlNode element2 = (XmlNode) xmlDocument.CreateElement(PortalConsts.XmlNodeValueAttribute);
    this.AddAttribute(xmlDocument, element2, "F_INLIST_ID", "0");
    this.AddAttribute(xmlDocument, element2, "F_STRING_VALUE", attributeByGuid.AsString);
    element1.AppendChild(element2);
    xmlRootNode.AppendChild(element1);
  }

  private void AddAttribute(XmlDocument xmlDocument, XmlNode node, string attrName, string value)
  {
    XmlAttribute attribute = xmlDocument.CreateAttribute(attrName);
    attribute.Value = value;
    node.Attributes.Append(attribute);
  }

  public static long CheckPresentUser(IUserSession session, SiteInfo info, string login)
  {
    ConditionStructure conditionStructure = new ConditionStructure(new Guid("cad00018-306c-11d8-b4e9-00304f19f545"), RelationalOperators.Equal, (object) login, LogicalOperators.AND, 0);
    DataTable dataTable = session.GetObjectCollection(session.IdentHelper.UsersTypeID).Select(new DBRecordSetParams(new ConditionStructure[1]
    {
      conditionStructure
    }, new object[2]{ (object) -2, (object) -3 }));
    if (dataTable.Rows.Count != 1)
      throw new Exception(string.Format(LocalizationHolder.rm.GetString("PortalServer_54"), (object) login));
    if (session.GetRelation(info.ID, Convert.ToInt64(dataTable.Rows[0][1])) == null)
      throw new Exception(string.Format(LocalizationHolder.rm.GetString("PortalServer_55"), (object) login, (object) info.Caption));
    return Convert.ToInt64(dataTable.Rows[0][0]);
  }
}
