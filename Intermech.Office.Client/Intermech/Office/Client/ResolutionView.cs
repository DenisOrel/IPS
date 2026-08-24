// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.ResolutionView
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using ImSSP;
using Intermech.Client.Core.FormDesigner.Navigator;
using Intermech.DataFormats;
using Intermech.Diagnostics;
using Intermech.Extensions;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using Intermech.Office.Interfaces;
using Intermech.PropertyEditors;
using Intermech.Workflow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Office.Client;

[ViewDescriptionProvider(typeof (ResolutionView.ResolutionViewDescriptionProvider))]
public class ResolutionView : FormDesignerView
{
  private bool _loaded;
  private long _taskID;
  private IContainer components;

  public override int OrderID => 0;

  [NotNull]
  public override string Caption => Localization.GetString("Office.Client_64");

  public override void Activate(IView previousView)
  {
    if (!this._loaded)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        this._objID = sessionKeeper.Session.GetObject(this._taskID).AttributeByID(wfConsts.AttrProcessID).As<IDBObjectLinkAttribute>().DBObject.AttributeByID(OfficeConsts.AttrResolutionIdentityID).AsInteger;
        this._info = (IElementInfo) new Intermech.Client.Core.FormDesigner.Controls.ElementInfo(this._objID, AttributableElements.Object);
        ICollection<FormInformation> formsForObject = sessionKeeper.Session.GetCustomService<IFormDesignerService>().GetFormsForObject(this._objID, sessionKeeper.Session.SessionGUID);
        long formId = this.FormID;
        using (IEnumerator<FormInformation> enumerator = formsForObject.GetEnumerator())
        {
          if (enumerator.MoveNext())
            this.FormID = enumerator.Current.ID;
        }
        if (formId != 0L && formId != this.FormID)
          this.RemoveForm();
        if (this.FormID == 0L)
          throw new Exception(Localization.GetString(sc_15065.ssp_office_15066(), (object) this._objID));
      }
      this._loaded = true;
    }
    base.Activate(previousView);
  }

  public override void Initialize([NotNull] ISelectedItems items, [CanBeNull] IServiceProvider provider)
  {
    this._taskID = items.GetItemData<IDBTypedObjectID>(0).ObjectID;
    base.Initialize(items, provider);
    this._loaded = false;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ResolutionView));
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Name = nameof (ResolutionView);
    this.ResumeLayout(false);
  }

  private sealed class ResolutionViewDescriptionProvider : BaseViewDescriptionProvider
  {
    public override ViewDescription DoGetViewDescription(
      ISelectedItems selectedItems,
      IServiceProvider serviceProvider)
    {
      if (!(serviceProvider.GetService(typeof (INamedImageList)) is INamedImageList service))
        service = ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList;
      INamedImageList namedImageList = service;
      return new ViewDescription()
      {
        Caption = Localization.GetString("Office.Client_64"),
        ImageIndex = namedImageList.ImageIndex("imgCard"),
        OrderID = 0
      };
    }
  }
}
