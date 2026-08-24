// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalSelectionCreatorControl
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Client.Core.ObjectCreator;
using Intermech.Client.Core.ObjectCreator.Controls;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using Intermech.Navigator.SelectionView;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client;

public class PortalSelectionCreatorControl : ObjectCreatorControl
{
  private CreatedObjectItem _createdObject;
  private List<long> _objIDList;
  private SelectionForm _editorForm;

  public PortalSelectionCreatorControl(CreatedObjectItem createdObject)
  {
    this._createdObject = createdObject;
    this._editorForm = new SelectionForm()
    {
      ParentMode = SelectionFormMode.InObjectCreator
    };
    this._editorForm.SetParent((Control) this);
    this._objIDList = new List<long>()
    {
      createdObject.ObjectID
    };
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      int relationTypeId = MetaDataHelper.GetRelationTypeID(new Guid("cad00151-306c-11d8-b4e9-00304f19f545"));
      List<int> childrenIdRecursive = MetaDataHelper.GetObjectTypeChildrenIDRecursive(PortalConsts.objtypePortalSelections);
      foreach (ObjectRelationLink objectRelation in createdObject.ObjectRelationArray)
      {
        if (objectRelation.RelationTypeID == relationTypeId)
        {
          QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(objectRelation.ObjectID);
          if (!objectInfo.Empty && childrenIdRecursive.Contains(objectInfo.ObjectTypeID))
            this._objIDList.Add(objectInfo.ObjectID);
        }
      }
    }
    this._editorForm.SelectionLoad(this._createdObject.ObjectID, this._objIDList);
  }

  public override bool Refresh(PageRefreshArgs args)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      this._editorForm.ReloadObjTypes(sessionKeeper.Session, this._objIDList);
      IDBObject dbObject = sessionKeeper.Session.GetObject(this._createdObject.ObjectID);
      this._editorForm.ReloadSelectionType(sessionKeeper.Session, dbObject);
    }
    return base.Refresh(args);
  }

  public override bool Save(PageSaveArgs args)
  {
    try
    {
      this._editorForm.SelectionSave();
      return true;
    }
    catch (Exception ex)
    {
      args.Error = ex;
      return false;
    }
  }

  public override int HelpTopicID => 785;
}
