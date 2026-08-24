// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.OfficeClientHelper
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using ImSSP;
using Intermech.Client.Core;
using Intermech.DataFormats;
using Intermech.Diagnostics;
using Intermech.Extensions;
using Intermech.Interfaces;
using Intermech.Interfaces.Workflow;
using Intermech.Kernel.Search;
using Intermech.Navigator;
using Intermech.Navigator.Interfaces;
using Intermech.Office.Interfaces;
using Intermech.Security;
using Intermech.Workflow;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Office.Client;

public class OfficeClientHelper
{
  private const string VariableAddressee = "ADDRESSEE";

  public static OfficeDocumentTypes GetOfficeDocumentType(
    [NotNull] IUserSession session,
    int documentType,
    [CanBeNull] OfficeDocumentTypes[] enableTypes,
    long parentDocumentID,
    out bool designationEqualRegNumber)
  {
    designationEqualRegNumber = false;
    OfficeDocumentTypeSettings settings = session.GetCustomService<IOfficeDocumentTypeService>().GetSettings(session.SessionGUID, documentType);
    if (settings.EnableTypes == null || settings.EnableTypes.Length == 0)
      throw new Exception(Localization.GetString(sc_15055.ssp_office_15056(), (object) MetaDataHelper.GetObjectTypeName(documentType)));
    List<OfficeDocumentTypes> officeDocumentTypesList = (enableTypes != null ? ((IEnumerable<OfficeDocumentTypes>) enableTypes).Where<OfficeDocumentTypes>((System.Func<OfficeDocumentTypes, bool>) (enableType => Array.IndexOf<OfficeDocumentTypes>(settings.EnableTypes, enableType) >= 0)).ToList<OfficeDocumentTypes>(enableTypes.Length) : (List<OfficeDocumentTypes>) null) ?? ((IEnumerable<OfficeDocumentTypes>) settings.EnableTypes).AsList<OfficeDocumentTypes>();
    if (officeDocumentTypesList.Count == 0 && enableTypes != null)
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < enableTypes.Length; ++index)
      {
        if (index > 0)
          stringBuilder.Append(", ");
        stringBuilder.Append(EnumDescConverter.GetEnumDescription((Enum) enableTypes[index]));
      }
      throw new Exception(Localization.GetString(sc_15055.ssp_office_15057(), (object) MetaDataHelper.GetObjectTypeName(documentType), (object) stringBuilder.ToString()));
    }
    OfficeDocumentTypes key = OfficeDocumentTypes.Unknown;
    if (parentDocumentID != 0L)
    {
      switch ((int) session.GetObject(parentDocumentID).AttributeByID(OfficeConsts.AttrOfficeDocumentTypeID).AsInteger)
      {
        case 0:
          key = OfficeDocumentTypes.Outgoing;
          break;
        case 1:
          key = OfficeDocumentTypes.Incoming;
          break;
        case 2:
          key = OfficeDocumentTypes.Internal;
          break;
      }
      if (officeDocumentTypesList.IndexOf(key) < 0)
        throw new Exception(Localization.GetString(sc_15055.ssp_office_15058(), (object) MetaDataHelper.GetObjectTypeName(documentType), (object) EnumDescConverter.GetEnumDescription((Enum) key)));
    }
    else if (officeDocumentTypesList.Count == 1)
    {
      key = officeDocumentTypesList[0];
    }
    else
    {
      using (ChoiceOfficeDocTypeForm officeDocTypeForm = new ChoiceOfficeDocTypeForm(officeDocumentTypesList.ToArray(), documentType))
      {
        if (officeDocTypeForm.ShowDialog() != DialogResult.OK)
          return OfficeDocumentTypes.Unknown;
        key = officeDocTypeForm.OfficeDocumentType;
      }
    }
    RegNumberSettings regNumberSettings = (RegNumberSettings) null;
    Dictionary<OfficeDocumentTypes, RegNumberSettings> templates = settings.Templates;
    // ISSUE: explicit non-virtual call
    if ((templates != null ? (__nonvirtual (templates.TryGetValue(key, out regNumberSettings)) ? 1 : 0) : 0) != 0)
      designationEqualRegNumber = regNumberSettings.DesignationEqualRegNumber;
    return key;
  }

  [NotNull]
  public static string GetRegistrationNumber(
    [NotNull] IUserSession session,
    [NotEmpty] long documentID,
    int documentType,
    OfficeDocumentTypes type,
    long unitID)
  {
    return OfficeClientHelper.GetRegistrationNumber(session, documentID, documentType, type, true, unitID);
  }

  [NotNull]
  public static string GetRegistrationNumber(
    [NotNull] IUserSession session,
    [NotEmpty] long documentID,
    int documentType,
    OfficeDocumentTypes type,
    bool autoGenerate,
    long unitID)
  {
    IRegistrationNumberGenerator customService = session.GetCustomService<IRegistrationNumberGenerator>();
    if (autoGenerate && !customService.IsAutoGenerate(session.SessionGUID, documentType, type, unitID))
      return string.Empty;
    long classifierID = 0;
    if (customService.IsClassifierPresent(session.SessionGUID, documentType, type, unitID))
    {
      using (ClassifySelectionForm classifySelectionForm = new ClassifySelectionForm(session.GetCustomService<ISelectionsService>().GetClassifierForObjType((object) session.SessionGUID, documentType), Localization.GetString("Office.Client_62")))
      {
        if (classifySelectionForm.ShowDialog() == DialogResult.OK)
        {
          if (classifySelectionForm.SelectedItems.GetItemData(0, typeof (IDBObjectID)) is IDBObjectID itemData)
            classifierID = itemData.Value;
        }
      }
    }
    return unitID == 0L ? customService.Generate(session.SessionGUID, documentID, documentType, type, classifierID) : customService.PrivateGenerate(session.SessionGUID, documentID, documentType, type, classifierID, unitID);
  }

  [NotNull]
  public static string GetRegistrationNumber(
    [NotNull] IUserSession session,
    [NotEmpty] long documentID,
    int documentType,
    OfficeDocumentTypes type)
  {
    return OfficeClientHelper.GetRegistrationNumber(session, documentID, documentType, type, 0L);
  }

  public static void AddRegistrationAttributes(
    [NotNull] IDBObject obj,
    [NotNull] string regNumber,
    bool designationEqualRegNumber)
  {
    OfficeClientHelper.AddRegistrationAttributes(obj, regNumber, designationEqualRegNumber, true);
  }

  public static void AddRegistrationAttributes(
    [NotNull] IDBObject obj,
    [NotNull] string regNumber,
    bool designationEqualRegNumber,
    bool setDate)
  {
    (obj.GetAttributeByID(OfficeConsts.AttrRegNumberID) ?? obj.Attributes.AddAttribute(OfficeConsts.AttrRegNumberID, false)).AsString = regNumber;
    if (designationEqualRegNumber)
    {
      IDBAttribute attributeById = obj.GetAttributeByID(OfficeConsts.AttrDesignationID);
      if (attributeById != null)
        attributeById.AsString = regNumber;
    }
    if (!setDate)
      return;
    (obj.GetAttributeByID(OfficeConsts.AttrRegistrationDateID) ?? obj.Attributes.AddAttribute(OfficeConsts.AttrRegistrationDateID, false)).AsDateTime = DateTime.Now;
  }

  public static void SetTypeOfficeDocuments([NotNull] IDBObject obj, OfficeDocumentTypes type)
  {
    (obj.GetAttributeByID(OfficeConsts.AttrOfficeDocumentTypeID) ?? obj.Attributes.AddAttribute(OfficeConsts.AttrOfficeDocumentTypeID, false)).AsInteger = (long) type;
  }

  public static bool RegisterNewDocument(
    [NotEmpty] long documentID,
    int documentType,
    [CanBeNull] OfficeDocumentTypes[] enableTypes,
    long parentDocumentID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      return OfficeClientHelper.RegisterNewDocument(sessionKeeper.Session, documentID, documentType, enableTypes, parentDocumentID);
  }

  public static bool RegisterNewDocument(
    [NotNull] IUserSession session,
    [NotEmpty] long documentID,
    int documentType,
    [CanBeNull] OfficeDocumentTypes[] enableTypes,
    long parentDocumentID)
  {
    bool designationEqualRegNumber;
    OfficeDocumentTypes officeDocumentType = OfficeClientHelper.GetOfficeDocumentType(session, documentType, enableTypes, parentDocumentID, out designationEqualRegNumber);
    OfficeDocument officeDocument;
    switch (officeDocumentType)
    {
      case OfficeDocumentTypes.Unknown:
        return false;
      case OfficeDocumentTypes.Incoming:
        officeDocument = (OfficeDocument) new IncomingDocument();
        break;
      case OfficeDocumentTypes.Outgoing:
        officeDocument = (OfficeDocument) new OutgoingDocument();
        break;
      case OfficeDocumentTypes.Internal:
        officeDocument = (OfficeDocument) new InternalDocument();
        break;
      default:
        throw new NotSupportedException($"Unsupported {"OfficeDocumentTypes"} value:{officeDocumentType}");
    }
    IDBTransactions customService = (IDBTransactions) session.GetCustomService(typeof (IDBTransactions));
    customService.StartTransaction();
    try
    {
      int num = officeDocument.RegisterNewDocument(session, session.GetObject(documentID), parentDocumentID, designationEqualRegNumber) ? 1 : 0;
      customService.Commit();
      return num != 0;
    }
    catch
    {
      customService.Rollback();
      throw;
    }
  }

  [NotNull]
  public static int[] AddresseeTypes
  {
    get
    {
      List<int> childrenIdRecursive = MetaDataHelper.GetObjectTypeChildrenIDRecursive(OfficeConsts.ObjtypeUsersID);
      childrenIdRecursive.AddRange((IEnumerable<int>) MetaDataHelper.GetObjectTypeChildrenIDRecursive(OfficeConsts.ObjtypeGroupsID));
      childrenIdRecursive.AddRange((IEnumerable<int>) MetaDataHelper.GetObjectTypeChildrenIDRecursive(OfficeConsts.ObjtypeOrganizationUnitsID));
      return childrenIdRecursive.AsArray<int>();
    }
  }

  [CanBeNull]
  public static List<long> GetUserAddresseeList([NotNull] IUserSession session, long addresseeID)
  {
    QuickObjectInfo objectInfo = session.GetObjectInfo(addresseeID);
    if (objectInfo.ObjectTypeID == OfficeConsts.ObjtypeGroupsID)
    {
      DataTable dataTable = session.GetRelationCollection(OfficeConsts.ReltypeSimpleID).ConsistFrom(new DBRecordSetParams((ConditionStructure[]) null, new object[2]
      {
        (object) -2,
        (object) -7
      }), addresseeID, true);
      List<int> childrenIdRecursive = MetaDataHelper.GetObjectTypeChildrenIDRecursive(OfficeConsts.ObjtypeUsersID);
      List<long> longList = new List<long>(dataTable.Rows.Count);
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        if (childrenIdRecursive.Contains(Convert.ToInt32(row[1])))
        {
          long int64 = Convert.ToInt64(row[0]);
          if (!longList.Contains(int64))
            longList.Add(int64);
        }
      }
      return longList.Count <= 0 ? (List<long>) null : longList;
    }
    if (MetaDataHelper.GetObjectTypeChildrenIDRecursive(OfficeConsts.ObjtypeOrganizationUnitsID).Contains(objectInfo.ObjectTypeID))
    {
      long director = OfficeClientHelper.GetDirector(session, objectInfo.ObjectID);
      if (director == 0L)
        return (List<long>) null;
      return new List<long>((IEnumerable<long>) new long[1]
      {
        director
      });
    }
    return new List<long>((IEnumerable<long>) new long[1]
    {
      addresseeID
    });
  }

  public static long GetDirector([NotNull] IUserSession session, long unitID)
  {
    return OfficeClientHelper.GetDirector(session, unitID, out string _, false);
  }

  [NotNull]
  public static IDescriptor GetAddresseesDescriptor()
  {
    return (IDescriptor) new Intermech.Navigator.CustomNode.Descriptor("Адресаты", new DescriptorCollection()
    {
      (IDescriptor) new UsersGroupsDescriptor(),
      (IDescriptor) new Intermech.Navigator.DBObjectTypes.Descriptor(MetaDataHelper.GetObjectTypeID("cadd9235-306c-11d8-b4e9-00304f19f545"))
    });
  }

  public static bool CheckDirector([NotNull] IDBTypedObjectID obj)
  {
    if (MetaDataHelper.GetObjectTypeChildrenIDRecursive(OfficeConsts.ObjtypeOrganizationUnitsID).Contains(obj.ObjectType))
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        string message;
        if (OfficeClientHelper.GetDirector(sessionKeeper.Session, obj.ObjectID, out message, false) == 0L)
        {
          if (MessageBox.Show(message + " Продолжить?", MessageDialogs.msgConfirmAction, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            return false;
        }
      }
    }
    return true;
  }

  public static long GetDirector(
    [NotNull] IUserSession session,
    [NotEmpty] long unitID,
    [CanBeNull] out string message,
    bool throwException)
  {
    IDBObject dbObject = session.GetObject(unitID);
    IDBAttribute attributeById = dbObject.GetAttributeByID(OfficeConsts.AttrDirectorID);
    if ((attributeById != null ? (attributeById.IsNull ? 1 : 0) : 1) != 0 || attributeById.AsInteger == 0L)
    {
      message = "Не установлен Руководитель для " + dbObject.NameInMessages;
      if (throwException)
        throw new Exception(message);
      return 0;
    }
    message = (string) null;
    return attributeById.AsInteger;
  }

  public static void CreateAddresseesMessage(
    [NotNull] IUserSession session,
    [NotNull] long[] notifyAddressees,
    [NotNull] IDBObject document)
  {
    IRouterService customService1 = session.GetCustomService<IRouterService>();
    IOfficeGeneralSettingsService customService2 = session.GetCustomService<IOfficeGeneralSettingsService>();
    if (customService2.Settings.AddresseeTemplateID != 0L)
    {
      foreach (long notifyAddressee in notifyAddressees)
      {
        IProcess process = customService1.CreateProcess(session.SessionGUID, customService2.Settings.AddresseeTemplateID);
        process.Name = "Сообщение адресату канцелярского документа.";
        if (process.StartActivity == null)
          throw new Exception("Start activity not found!");
        process.StartActivity.Attachments.Add(document.ObjectID);
        IVariable variable = process.StartActivity.Variables.Find("ADDRESSEE");
        if (variable == null)
          throw new VariableMissingException("ADDRESSEE");
        ParticipantList participantList = new ParticipantList(session);
        participantList.AddParticipant(ParticipantKind.User, notifyAddressee);
        variable.Value = participantList.AsString;
        process.StartProcess();
      }
    }
    else
      customService1.CreateMessage(session.SessionGUID, notifyAddressees, document.NameInMessages, $"Вам адресован канцелярский документ <a href =\"#object={document.ObjectGUID}\">{document.NameInMessages}</a>", session.UserID);
  }
}
