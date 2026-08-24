// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.SearchScheme.SearchSchemeView
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
namespace Intermech.Pdm.SearchScheme;

[ViewDescriptionProvider(typeof (SearchSchemeView.SearchSchemeViewDescriptionProvider))]
internal class SearchSchemeView : UserControl, IView
{
  private long _objectID;
  private bool _initmode;
  private int _imageIndex;
  private bool _loaded;
  private SearchSchemeEditor form;
  private IContainer components;

  public SearchSchemeView()
  {
    this.InitializeComponent();
    INamedImageList service = (INamedImageList) ServicesManager.GetService(typeof (INamedImageList));
    this._imageIndex = service != null ? service.ImageIndex("imgEditScheme") : -1;
  }

  public void Initialize(ISelectedItems items, IServiceProvider provider)
  {
    this._objectID = ((IDBTypedObjectID) items.GetItemData(0, typeof (IDBTypedObjectID))).ObjectID;
    this._initmode = true;
    this._loaded = false;
  }

  public void Activate(IView previousView)
  {
    if (this._initmode)
    {
      if (this.form == null)
      {
        this.form = new SearchSchemeEditor();
        this.form.SetParent((Control) this);
        this.form.ParentMode = 2;
      }
      this._initmode = false;
    }
    if (this._loaded)
      return;
    this.form.SchemeID = this._objectID;
    this.form.LoadObjectData(0);
    this._loaded = true;
  }

  public void Deactivate(IView nextView)
  {
    if (this.form == null || !this.form.IsChanged || !MessageBox.Show(LocalizationHolder.rm.GetString(sc_16701.ssp_pdm_16702()), this.Caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals((object) DialogResult.Yes))
      return;
    this.form.SaveObjectData();
  }

  public string Caption => SearchShemeConsts.SearchSchemeEditorName;

  public int ImageIndex => this._imageIndex;

  public int OrderID => 5;

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SearchSchemeView));
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Name = nameof (SearchSchemeView);
    this.Tag = (object) " ";
    this.ResumeLayout(false);
  }

  private sealed class SearchSchemeViewDescriptionProvider : BaseViewDescriptionProvider
  {
    public override ViewDescription DoGetViewDescription(
      ISelectedItems selectedItems,
      IServiceProvider serviceProvider)
    {
      INamedImageList service = (INamedImageList) ServicesManager.GetService(typeof (INamedImageList));
      return new ViewDescription()
      {
        Caption = SearchShemeConsts.SearchSchemeEditorName,
        ImageIndex = service != null ? service.ImageIndex("imgEditScheme") : -1,
        OrderID = 5
      };
    }
  }
}
