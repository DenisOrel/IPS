// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.SignUpActionHandler
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using Intermech.Client.Core.FormDesigner.Controls;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Signs.Client;
using Intermech.Signs.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Signs;

public class SignUpActionHandler : IFormDesignerActionHandler
{
  public bool ButtonEnabled(object button, object form)
  {
    bool flag = false;
    if (!(form is DesForm desForm))
      return false;
    IMSApplicability imsApplicability = (IMSApplicability) null;
    if (desForm.Info.ElementKind == AttributableElements.Object)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObject dbObject = sessionKeeper.Session.GetObject(desForm.Info.ElementIdentifier, false);
        if (dbObject != null)
          imsApplicability = MetaDataHelper.GetApplicability(dbObject.ObjectType, SignsHolder.SignObjectTypeID, SignsHolder.SignRelationTypeID);
      }
    }
    else if (desForm.Info.ElementKind == AttributableElements.Relation)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBRelation relation = sessionKeeper.Session.GetRelation(desForm.Info.ElementIdentifier, false);
        if (relation != null)
        {
          IDBObject objectById = sessionKeeper.Session.GetObjectByID(relation.PartID, false);
          if (objectById != null)
            imsApplicability = MetaDataHelper.GetApplicability(objectById.ObjectType, SignsHolder.SignObjectTypeID, SignsHolder.SignRelationTypeID);
        }
      }
    }
    if (imsApplicability != null && imsApplicability.ApplicabilityMode == ApplicabilityModes.Enabled)
      flag = true;
    return flag;
  }

  public void ButtonPressed(object button, object form)
  {
    if (!(form is DesForm desForm))
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(desForm.Info.ElementIdentifier, false);
      if (dbObject == null)
        return;
      SignsCommands.SignUpCommand(new List<IDBTypedObjectID>()
      {
        (IDBTypedObjectID) new DBTypedObjectID(dbObject.ObjectType, dbObject.ObjectID, dbObject.ID, dbObject.Caption, dbObject.OwnerID, (long) dbObject.VersionID, Convert.ToInt64(dbObject.IsBaseVersion), dbObject.SiteID, dbObject.ModificationID)
      });
    }
  }
}
