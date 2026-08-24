// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.PortalSelectionDialogControl
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Client.Core.ObjectCreator;
using Intermech.Client.Core.ObjectCreator.Controls;
using System;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

public class PortalSelectionDialogControl : ObjectCreatorControl
{
  private CreatedObjectItem selObject;
  private PortalSelectionDialog sForm;

  public PortalSelectionDialogControl(CreatedObjectItem createdObject)
  {
    this.selObject = createdObject;
    this.sForm = new PortalSelectionDialog();
    this.sForm.SetParent((Control) this);
    this.sForm.SelectionLoad(this.selObject.ObjectID);
  }

  public override bool Refresh(PageRefreshArgs args)
  {
    try
    {
      this.sForm.SelectionLoad(this.selObject.ObjectID);
      return true;
    }
    catch (Exception ex)
    {
      args.Error = ex;
      return false;
    }
  }

  public override bool Save(PageSaveArgs args)
  {
    try
    {
      this.sForm.SelectionSave();
      return true;
    }
    catch (Exception ex)
    {
      args.Error = ex;
      return false;
    }
  }

  public override int HelpTopicID => 1736;
}
