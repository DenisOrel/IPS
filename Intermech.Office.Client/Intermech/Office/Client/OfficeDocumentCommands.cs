// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.OfficeDocumentCommands
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using ImSSP;
using Intermech.Client.Core.ObjectCreator;
using Intermech.Controls;
using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Configuration;
using Intermech.Kernel.Search;
using Intermech.Office.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Office.Client;

internal sealed class OfficeDocumentCommands
{
  private const string ConfigNewAnswerSelectType = "NewAnswerSelectType";

  public static void PublicRegister([NotNull] IUserSession session, long documentID)
  {
    IDBObject iDbAttributable = session.GetObject(documentID);
    AttributeValues[] attributesValues = iDbAttributable.GetAttributesValues(GetAttributeValuesModes.IncludeDescriptions);
    if (session.GetCustomService<IOfficeRegistrationService>().IsDocumentRegister(session.SessionGUID, documentID))
    {
      int num1 = (int) IMMessageBox.Show(Localization.GetString("Office.Client_30"), $"{iDbAttributable.NameInMessages} уже зарегистрирован. Повторная регистрация невозможна.", MessageBoxButtons.OK, IMMessageBoxImage.Warning);
    }
    else
    {
      IDBTransactions customService = (IDBTransactions) session.GetCustomService(typeof (IDBTransactions));
      customService.StartTransaction();
      try
      {
        bool flag = false;
        if (iDbAttributable.CheckoutBy == 0L && iDbAttributable.ObjectModifyMode == ObjectModifyModes.Checkout)
        {
          iDbAttributable = iDbAttributable.CheckOut();
          flag = true;
        }
        bool designationEqualRegNumber = false;
        IDBRelationCollection relationCollection = session.GetRelationCollection(OfficeConsts.ReltypeAnswerID);
        relationCollection.ChildObjectTypes = (IList<int>) MetaDataHelper.GetLocalObjectTypeChildrenIDRecursive(OfficeConsts.ObjtypeDocumentsID);
        DataTable dataTable1 = relationCollection.EntersIn(new DBRecordSetParams((ConditionStructure[]) null, new object[1]
        {
          (object) -2
        }), iDbAttributable.ID);
        long int64 = dataTable1.Rows.Count == 1 ? Convert.ToInt64(dataTable1.Rows[0][0]) : 0L;
        List<int> childrenIdRecursive = MetaDataHelper.GetObjectTypeChildrenIDRecursive(OfficeConsts.ObjtypeOfficeDocumentsID);
        OfficeDocumentTypes type = childrenIdRecursive.Contains(iDbAttributable.ObjectType) ? OfficeDocumentCommands.GetDocumentType(iDbAttributable) : OfficeClientHelper.GetOfficeDocumentType(session, iDbAttributable.ObjectType, (OfficeDocumentTypes[]) null, int64, out designationEqualRegNumber);
        if (type == OfficeDocumentTypes.Unknown)
        {
          customService.Rollback();
        }
        else
        {
          string regNumber = OfficeClientHelper.GetRegistrationNumber(session, iDbAttributable.ObjectID, iDbAttributable.ObjectType, type);
          if (regNumber == string.Empty)
          {
            using (ManualRegNumberForm manualRegNumberForm = new ManualRegNumberForm(false))
            {
              manualRegNumberForm.Initialize(documentID, iDbAttributable.ObjectType, string.Empty, false, false);
              if (manualRegNumberForm.ShowDialog() == DialogResult.OK)
              {
                regNumber = manualRegNumberForm.Template;
              }
              else
              {
                customService.Rollback();
                return;
              }
            }
            iDbAttributable = session.GetObject(documentID);
          }
          List<AttributeValues> attributeValuesList = new List<AttributeValues>();
          OfficeClientHelper.AddRegistrationAttributes(iDbAttributable, regNumber, designationEqualRegNumber, true);
          attributeValuesList.Add(OfficeDocumentCommands.CreateAttributeValue(OfficeConsts.AttrRegNumberGuid, (object) regNumber));
          if (designationEqualRegNumber)
            attributeValuesList.Add(OfficeDocumentCommands.CreateAttributeValue(new Guid("cad0001f-306c-11d8-b4e9-00304f19f545"), (object) regNumber));
          attributeValuesList.Add(OfficeDocumentCommands.CreateAttributeValue(OfficeConsts.AttrRegistrationDateGuid, iDbAttributable.GetAttributeAndCheckNotEmpty(OfficeConsts.AttrRegistrationDateID).Value));
          if (!childrenIdRecursive.Contains(iDbAttributable.ObjectType))
          {
            int aGroupID = 0;
            switch (type)
            {
              case OfficeDocumentTypes.Incoming:
                aGroupID = OfficeConsts.AttrGroupIncomingOfficeParamsID;
                break;
              case OfficeDocumentTypes.Outgoing:
                aGroupID = OfficeConsts.AttrGroupOutgoingOfficeParamsID;
                break;
              case OfficeDocumentTypes.Internal:
                aGroupID = OfficeConsts.AttrGroupInternalOfficeParamsID;
                break;
            }
            IDBAttributesGroup attributesGroup = session.GetAttributesGroup(aGroupID);
            int[] source = new int[7]
            {
              OfficeConsts.AttrAddresseesID,
              OfficeConsts.AttrAddresseeRegDateID,
              OfficeConsts.AttrAddresseeRegDatesID,
              OfficeConsts.AttrInputRegNumID,
              OfficeConsts.AttrInputRegNumsID,
              OfficeConsts.AttrDocRecipientID,
              OfficeConsts.AttrDocRecipientsID
            };
            OfficeClientHelper.SetTypeOfficeDocuments(iDbAttributable, type);
            attributeValuesList.Add(OfficeDocumentCommands.CreateAttributeValue(OfficeConsts.AttrOfficeDocumentTypeGuid, (object) (int) type));
            DataTable dataTable2 = attributesGroup.Attributes.Select(string.Empty);
            for (int index = 0; index < dataTable2.Rows.Count; ++index)
            {
              int int32 = Convert.ToInt32(dataTable2.Rows[index]["F_ATTRIBUTE_ID"]);
              if (!((IEnumerable<int>) source).Contains<int>(int32) && iDbAttributable.GetAttributeByID(int32) == null)
              {
                iDbAttributable.Attributes.AddAttribute(int32, false);
                attributeValuesList.Add(OfficeDocumentCommands.CreateAttributeValue(new Guid(Convert.ToString(dataTable2.Rows[index]["F_GUID"])), (object) null));
              }
            }
          }
          if (flag)
            iDbAttributable.CheckIn();
          customService.Commit();
          Holder.NotificationService.FireEvent((object) null, (NotificationEventArgs) new DBObjectsExtendedEventArgs("ObjectsChanged", iDbAttributable.ObjectID, iDbAttributable.ObjectType, attributesValues, attributeValuesList.ToArray()));
          int num2 = (int) IMMessageBox.Show(Localization.GetString(sc_15092.ssp_office_15093()), Localization.GetString("Office.Client_31"), MessageBoxButtons.OK, IMMessageBoxImage.Information);
        }
      }
      catch
      {
        customService.Rollback();
        throw;
      }
    }
  }

  [NotNull]
  private static AttributeValues CreateAttributeValue(Guid attributeGuid, [CanBeNull] object value)
  {
    IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(attributeGuid);
    return new AttributeValues(attributeType.AttributeID, value)
    {
      AttributeType = attributeType.FieldType,
      ComputeMode = attributeType.Computed,
      MultipleValued = attributeType.MultiValueMode,
      AttributeGuid = attributeGuid,
      AttributeName = attributeType.Name
    };
  }

  private static OfficeDocumentTypes GetDocumentType(IDBObject obj)
  {
    IDBAttribute attributeById = obj.GetAttributeByID(OfficeConsts.AttrOfficeDocumentTypeID);
    if (attributeById == null)
      return OfficeDocumentTypes.Unknown;
    int int32 = Convert.ToInt32(attributeById.AsInteger);
    return !Enum.IsDefined(typeof (OfficeDocumentTypes), (object) int32) ? OfficeDocumentTypes.Unknown : (OfficeDocumentTypes) int32;
  }

  public static void CreateAnswer(long documentID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject1 = sessionKeeper.Session.GetObject(documentID);
      if (!sessionKeeper.Session.GetCustomService<IOfficeRegistrationService>().IsDocumentRegister(sessionKeeper.Session.SessionGUID, documentID))
      {
        int num1 = (int) IMMessageBox.Show(Localization.GetString(sc_15092.ssp_office_15094()), Localization.GetString("Office.Client_27", (object) dbObject1.NameInMessages), MessageBoxButtons.OK, IMMessageBoxImage.Error);
      }
      else
      {
        DataTable dataTable1 = sessionKeeper.Session.GetRelationCollection(OfficeConsts.ReltypeAnswerID).ConsistFrom(new DBRecordSetParams((ConditionStructure[]) null, new object[1]
        {
          (object) -2
        }), dbObject1.ObjectID);
        if (dataTable1.Rows.Count > 0)
        {
          IDBObject dbObject2 = sessionKeeper.Session.GetObject(Convert.ToInt64(dataTable1.Rows[0][0]));
          int num2 = (int) IMMessageBox.Show(Localization.GetString(sc_15092.ssp_office_15095()), Localization.GetString("Office.Client_28", (object) dbObject1.NameInMessages, (object) dbObject2.NameInMessages), MessageBoxButtons.OK, IMMessageBoxImage.Error);
        }
        else
        {
          int aSelectedID = -1;
          if (Holder.ConfigurationManager != null)
          {
            IConfiguration configuration = Holder.ConfigurationManager.Open(typeof (OfficeDocumentCommands).Name);
            if (configuration != null)
            {
              string property = configuration.GetProperty("NewAnswerSelectType");
              if (property != string.Empty)
                aSelectedID = Convert.ToInt32(property);
            }
          }
          OfficeDocumentTypes documentType = OfficeDocumentCommands.GetDocumentType(dbObject1);
          IOfficeDocumentTypeService customService = sessionKeeper.Session.GetCustomService<IOfficeDocumentTypeService>();
          List<int> intList = new List<int>();
          DataTable dataTable2 = sessionKeeper.Session.GetObjectTypeCollection(OfficeConsts.ObjtypeOfficeDocumentsID, true).SelectRecursive(string.Empty);
          for (int index = 1; index < dataTable2.Rows.Count; ++index)
          {
            int int32 = Convert.ToInt32(dataTable2.Rows[index]["F_OBJECT_TYPE"]);
            OfficeDocumentTypeSettings settings = customService.GetSettings(sessionKeeper.Session.SessionGUID, int32);
            if (settings.EnableTypes != null && Array.IndexOf<OfficeDocumentTypes>(settings.EnableTypes, documentType) >= 0)
              intList.Add(int32);
          }
          if (intList.Count == 0)
          {
            int num3 = (int) IMMessageBox.Show(Localization.GetString(sc_15092.ssp_office_15096()), Localization.GetString("Office.Client_76", (object) EnumDescConverter.GetEnumDescription((Enum) documentType)), MessageBoxButtons.OK, IMMessageBoxImage.Error);
          }
          else
          {
            int aObjectTypeID = ObjectCreatorSelectForm.ShowSelectDialog(intList.ToArray(), aSelectedID);
            if (aObjectTypeID == -1)
              return;
            if (Holder.ConfigurationManager != null)
              Holder.ConfigurationManager.Create(typeof (OfficeDocumentCommands).Name).SetProperty("NewAnswerSelectType", aObjectTypeID.ToString());
            long objectByTypeDialog = Holder.ObjectCreatorService.CreateObjectByTypeDialog(aObjectTypeID, new ObjectRelationLink[1]
            {
              new ObjectRelationLink(documentID, OfficeConsts.ReltypeAnswerID)
            });
            switch (objectByTypeDialog)
            {
              case -1:
                break;
              case 0:
                break;
              default:
                Holder.NotificationService.FireEvent((object) null, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsCreated", objectByTypeDialog));
                Holder.NotificationService.FireEvent((object) null, (NotificationEventArgs) new DBRelationsEventArgs("RelationsCreated", sessionKeeper.Session.GetRelation(documentID, objectByTypeDialog, true).RelationID));
                break;
            }
          }
        }
      }
    }
  }

  public static bool CheckAndPrivateRegister(
    [NotNull] IUserSession session,
    long objectID,
    bool createNewDoc = false)
  {
    IOfficeGeneralSettingsService customService1 = session.GetCustomService<IOfficeGeneralSettingsService>();
    IOfficeRegistrationService customService2 = session.GetCustomService<IOfficeRegistrationService>();
    long userUnit = customService2.GetUserUnit(session.UserID);
    if (customService1.Settings.PrivateOffice && userUnit != 0L)
    {
      IDBAttribute attributeById = session.GetObject(userUnit).GetAttributeByID(OfficeConsts.AttrSelfOfficeID);
      if ((attributeById != null ? (attributeById.AsBoolean ? 1 : 0) : 0) != 0 && !customService2.IsDocumentPrivateRegister(session.SessionGUID, userUnit, objectID))
      {
        IDBObject dbObject = session.GetObject(objectID);
        OfficeDocumentCommands.PrivateRegister(dbObject.ObjectID, dbObject.ObjectType, createNewDoc);
        return true;
      }
    }
    return false;
  }

  public static bool PrivateRegister(long documentID, int documentType, bool createNewDoc = false)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IOfficeRegistrationService customService1 = sessionKeeper.Session.GetCustomService<IOfficeRegistrationService>();
      long userUnit = customService1.GetUserUnit(sessionKeeper.Session.UserID);
      if (userUnit == 0L)
      {
        int num = (int) IMMessageBox.Show("Регистрация документа во внутренней канцелярии", "Пользователь не входит ни в одну организационную единицу с собственной канцелярией. Регистрация во внутренней канцелярии невозможна.", MessageBoxButtons.OK, IMMessageBoxImage.Error);
        return false;
      }
      bool flag = false;
      IDBObject dbObject = sessionKeeper.Session.GetObject(documentID);
      AttributeValues[] attributesValues = dbObject.GetAttributesValues(GetAttributeValuesModes.IncludeDescriptions);
      string privateRegNumber = customService1.GetPrivateRegNumber(sessionKeeper.Session.SessionGUID, documentID);
      if (privateRegNumber != string.Empty)
      {
        using (ManualRegNumberForm manualRegNumberForm = new ManualRegNumberForm(true))
        {
          manualRegNumberForm.Initialize(documentID, documentType, privateRegNumber, false, true);
          if (manualRegNumberForm.ShowDialog() == DialogResult.OK)
          {
            if (privateRegNumber != manualRegNumberForm.Template)
            {
              IDBAttribute dbAttribute = sessionKeeper.Session.GetObject(documentID).AttributeByID(OfficeConsts.AttrPrivateRegNumberID);
              for (int index = 0; index < dbAttribute.ValuesCount; ++index)
              {
                dbAttribute.Index = index;
                if (dbAttribute.AsString == privateRegNumber)
                {
                  dbAttribute.AsString = manualRegNumberForm.Template;
                  break;
                }
              }
              customService1.UpdatePrivateRegNumber(sessionKeeper.Session.SessionGUID, documentID, manualRegNumberForm.Template);
              flag = true;
            }
          }
        }
      }
      else
      {
        OfficeDocumentTypes documentType1 = OfficeDocumentCommands.GetDocumentType(dbObject);
        if (documentType1 == OfficeDocumentTypes.Unknown)
          throw new Exception($"Для {dbObject.NameInMessages} не найден вид канцелярского документа. Сначала зарегистрируйте документ в общей канцелярии.");
        IRegistrationNumberGenerator customService2 = sessionKeeper.Session.GetCustomService<IRegistrationNumberGenerator>();
        string regNumber = !createNewDoc || !customService2.IsEmptyRegNumbersEnabled(sessionKeeper.Session.SessionGUID, documentType, documentType1, userUnit) ? OfficeClientHelper.GetRegistrationNumber(sessionKeeper.Session, dbObject.ObjectID, dbObject.ObjectType, documentType1, userUnit) : (string) null;
        if (regNumber == string.Empty)
        {
          using (ManualRegNumberForm manualRegNumberForm = new ManualRegNumberForm(true))
          {
            manualRegNumberForm.Initialize(documentID, documentType, string.Empty, false, true);
            if (manualRegNumberForm.ShowDialog() != DialogResult.OK)
              return false;
            regNumber = manualRegNumberForm.Template;
          }
        }
        if (!string.IsNullOrEmpty(regNumber))
          flag = sessionKeeper.Session.GetCustomService<IOfficeRegistrationService>().PrivateRegister(sessionKeeper.Session.SessionGUID, userUnit, documentID, regNumber);
      }
      IDBAttribute dbAttribute1 = sessionKeeper.Session.GetObject(documentID).AttributeByGuid(OfficeConsts.AttrPrivateRegNumberGuid);
      Holder.NotificationService.FireEvent((object) null, (NotificationEventArgs) new DBObjectsExtendedEventArgs("ObjectsChanged", documentID, documentType, attributesValues, new AttributeValues[1]
      {
        OfficeDocumentCommands.CreateAttributeValue(OfficeConsts.AttrPrivateRegNumberGuid, (object) dbAttribute1.Values)
      }));
      return flag;
    }
  }
}
