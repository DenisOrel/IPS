// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.ResolutionCommands
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using ImSSP;
using Intermech.Controls;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Office.Interfaces;
using System;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Office.Client;

internal sealed class ResolutionCommands
{
  private static void CreateResolution(long documentID, int typeID, long aTemplateResolutionID = 0)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(documentID);
      if (dbObject.CheckoutBy != 0L)
      {
        int num1 = (int) IMMessageBox.Show(Localization.GetString(sc_15101.ssp_office_15102()), $"Необходимо завершить редактирование {dbObject.NameInMessages}", MessageBoxButtons.OK, IMMessageBoxImage.Error);
      }
      else if (dbObject.ObjectType != OfficeConsts.ObjtypeResolutionsID && !sessionKeeper.Session.GetCustomService<IOfficeRegistrationService>().IsDocumentRegister(sessionKeeper.Session.SessionGUID, documentID))
      {
        int num2 = (int) IMMessageBox.Show(Localization.GetString(sc_15101.ssp_office_15103()), $"{dbObject.NameInMessages} не зарегистрирован в канцелярии. Создание поручения по нему невозможна.", MessageBoxButtons.OK, IMMessageBoxImage.Error);
      }
      else
      {
        long num3;
        if (aTemplateResolutionID != 0L)
          num3 = Holder.ObjectCreatorService.CreateObjectByTemplateDialog(aTemplateResolutionID, new ObjectRelationLink[1]
          {
            new ObjectRelationLink(documentID, OfficeConsts.ReltypeOfficeCompositionID)
          }, DateTime.Now);
        else
          num3 = Holder.ObjectCreatorService.CreateObjectByTypeDialog(typeID, new ObjectRelationLink[1]
          {
            new ObjectRelationLink(documentID, OfficeConsts.ReltypeOfficeCompositionID)
          }, DateTime.Now);
        if (num3 == 0L || num3 == -1L)
          return;
        IDBResolution resolution = sessionKeeper.Session.GetResolution(num3);
        bool result;
        if (resolution.TryGetAttrBoolValue(OfficeConsts.AttrTempDelayedRunID, out result) && result)
          return;
        Holder.NotificationService.FireEvent((object) null, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsCreated", num3, resolution.ObjectType));
        if (documentID == 0L)
          return;
        Holder.NotificationService.FireEvent((object) null, (NotificationEventArgs) new DBRelationsEventArgs("RelationsCreated", sessionKeeper.Session.GetRelation(documentID, num3, true).RelationID));
      }
    }
  }

  public static void Create(long documentID, bool confidential = false)
  {
    ResolutionCommands.CreateResolution(documentID, confidential ? OfficeConsts.ObjtypeConfidentialResolutionsID : OfficeConsts.ObjtypeResolutionsID);
  }

  public static void CreateByPrototype(long resolutionID, bool confidential = false)
  {
    ResolutionCommands.CreateResolution(Session.Invoke<long>((Session.SessionHandler<long>) (session => OfficeHelper.FindOfficeDocument(session, resolutionID))), confidential ? OfficeConsts.ObjtypeConfidentialResolutionsID : OfficeConsts.ObjtypeResolutionsID, resolutionID);
  }
}
