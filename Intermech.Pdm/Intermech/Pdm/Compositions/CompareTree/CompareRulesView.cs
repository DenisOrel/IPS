// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.CompareRulesView
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using ImSSP;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

[ViewDescriptionProvider(typeof (CompareRulesView.CompareRulesViewDescriptionProvider))]
public class CompareRulesView : UserControl, IView
{
  private CompareRulesForm form;
  private Guid _objectGuid;
  private bool _initmode;
  private bool _loaded;
  private IContainer components;

  public CompareRulesView()
  {
    this.InitializeComponent();
    INamedImageList service = (INamedImageList) ServicesManager.GetService(typeof (INamedImageList));
    this.ImageIndex = service != null ? service.ImageIndex("imgEditScheme") : -1;
  }

  public string Caption => "Настройки правила";

  public int ImageIndex { get; } = -1;

  public int OrderID => 5;

  public void Activate(IView previousView)
  {
    if (this._initmode)
    {
      if (this.form == null)
      {
        this.form = new CompareRulesForm();
        this.form.SetParent((Control) this);
        this.form.ParentMode = 2;
      }
      this._initmode = false;
    }
    if (this._loaded)
      return;
    this.form.LoadObjectData(this._objectGuid, 0);
    this._loaded = true;
  }

  public void Deactivate(IView nextView)
  {
    if (this.form == null || !this.form.IsChanged || !MessageBox.Show(LocalizationHolder.rm.GetString(sc_16630.ssp_pdm_16631()), this.Caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals((object) DialogResult.Yes))
      return;
    this.form.SaveObjectData();
  }

  public void Initialize(ISelectedItems items, IServiceProvider provider)
  {
    IDBTypedObjectID itemData = (IDBTypedObjectID) items.GetItemData(0, typeof (IDBTypedObjectID));
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this._objectGuid = sessionKeeper.Session.GetObjectInfo(itemData.ObjectID).VersionGuid;
    this._initmode = true;
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
    this.components = (IContainer) new System.ComponentModel.Container();
    this.AutoScaleMode = AutoScaleMode.Font;
  }

  private sealed class CompareRulesViewDescriptionProvider : BaseViewDescriptionProvider
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
        Caption = "Настройки правила",
        ImageIndex = namedImageList != null ? namedImageList.ImageIndex("imgEditScheme") : -1,
        OrderID = 5
      };
    }
  }
}
