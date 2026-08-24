// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.SeriesDates.SeriesDatesView
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using Intermech.Search.Data.Repositories;
using Intermech.Search.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.Pdm.SeriesDates;

[ViewDescriptionProvider(typeof (SeriesDatesView.SeriesDatesViewDescriptionProvider))]
public class SeriesDatesView : UserControl, IView
{
  private bool _activated;
  private long[] _objectVersionIds;
  private LazyService<INamedImageList> _namedImageList = new LazyService<INamedImageList>();
  private IContainer components;
  private SeriesDatesEditorControl _seriesDatesEditorControl;

  public static bool CheckViewParams(ISelectedItems selectedItems, IServiceProvider serviceProvider)
  {
    if (selectedItems == null)
      throw new ArgumentNullException(nameof (selectedItems));
    if (selectedItems.Count == 0)
      return false;
    IAttributeTypeForObjectRepository objectRepository = ServiceLocator.Get<IAttributeTypeForObjectRepository>();
    List<long> longList = new List<long>();
    int index = 0;
    for (int count = selectedItems.Count; index < count; ++index)
    {
      if (!(selectedItems.GetItemData(index, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData) || ObjectHelper.IsUnknownObjectVersionID(itemData.ObjectID) || ObjectHelper.IsUnknownObjectID(itemData.ID) || itemData.ObjectType == -1 || objectRepository.Find(itemData.ObjectType).Where<IMSAttribute4ObjectType>((Func<IMSAttribute4ObjectType, bool>) (o => o.AttributeID == SeriesDatesConstants.SeriesDatesApplicabilityAttributeTypeID)).Count<IMSAttribute4ObjectType>() == 0 || longList.Contains(itemData.ID))
        return false;
      longList.Add(itemData.ID);
    }
    return true;
  }

  public SeriesDatesView() => this.InitializeComponent();

  public void Initialize(ISelectedItems items, IServiceProvider provider)
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    this._objectVersionIds = SeriesDatesView.CheckViewParams(items, provider) ? this.GetObjectVersionIds(items) : throw new ArgumentException();
    this._activated = false;
  }

  public void Activate(IView previousView)
  {
    if (this._activated)
      return;
    this._seriesDatesEditorControl.ObjectVersionIds = this._objectVersionIds;
    this._activated = true;
  }

  public void Deactivate(IView nextView)
  {
  }

  public string Caption => LocalizationHolder.rm.GetString("Pdm_702");

  public int ImageIndex => this._namedImageList.Value.ImageIndex("imgObjectsFilter");

  public int OrderID => 27;

  private long[] GetObjectVersionIds(ISelectedItems selectedItems)
  {
    List<long> longList = new List<long>();
    int num = 0;
    for (int count = selectedItems.Count; num < count; ++num)
    {
      IDBTypedObjectID itemData = selectedItems.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
      if (!longList.Contains(itemData.ObjectID))
        longList.Add(itemData.ObjectID);
    }
    return longList.ToArray();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this._seriesDatesEditorControl = new SeriesDatesEditorControl();
    this.SuspendLayout();
    this._seriesDatesEditorControl.Dock = DockStyle.Fill;
    this._seriesDatesEditorControl.Location = new Point(0, 0);
    this._seriesDatesEditorControl.Name = "_seriesDatesEditorControl";
    this._seriesDatesEditorControl.Size = new Size(800, 350);
    this._seriesDatesEditorControl.TabIndex = 0;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this._seriesDatesEditorControl);
    this.Name = nameof (SeriesDatesView);
    this.Size = new Size(800, 350);
    this.ResumeLayout(false);
  }

  private sealed class SeriesDatesViewDescriptionProvider : BaseViewDescriptionProvider
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
        Caption = LocalizationHolder.rm.GetString("Pdm_702"),
        ImageIndex = namedImageList != null ? namedImageList.ImageIndex("imgObjectsFilter") : -1,
        OrderID = 27
      };
    }
  }
}
