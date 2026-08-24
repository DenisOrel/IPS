// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.ProductionCopyDocumentView
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator.Controls;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MRP2;

[ViewDescriptionProvider(typeof (ProductionCopyDocumentView.ProductionCopyDocumentViewDescriptionProvider))]
public class ProductionCopyDocumentView : ChildrenView
{
  private string PLDocumentsViewStatesName = "PLDocuments_{E4B1261D-962D-4018-80E7-D7084A2185C1}";
  private int _imageIndex = -1;
  /// <summary>Required designer variable.</summary>
  private IContainer components;

  public ProductionCopyDocumentView()
  {
    this.InitializeComponent();
    if (!(ServicesManager.GetService(typeof (INamedImageList)) is INamedImageList service))
      return;
    this._imageIndex = service.ImageIndex("imgContains");
  }

  protected override bool UseInheritedNavViews
  {
    [DebuggerStepThrough] get => false;
    set => base.UseInheritedNavViews = false;
  }

  public override string StateStreamPrefix => this.PLDocumentsViewStatesName;

  public override string Caption => "Документы на изделие";

  public override int ImageIndex => this._imageIndex;

  public override int OrderID => 20;

  public override void Initialize(ISelectedItems items, IServiceProvider services)
  {
    if (!(items.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData))
      return;
    AdvRelationsDescriptor rootDescriptor;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBAttribute attributeById = sessionKeeper.Session.GetObject(itemData.ObjectID).GetAttributeByID(MRP2Consts.attrIdArticleLink);
      if (attributeById == null)
        return;
      long asInteger = attributeById.AsInteger;
      IDBObject dbObject = sessionKeeper.Session.GetObject(asInteger);
      rootDescriptor = new AdvRelationsDescriptor(Intermech.Navigator.Consts.CategoryAdvRelationsNode, 0, "cad001e2-306c-11d8-b4e9-00304f19f545", (List<long>) null, dbObject.ObjectID, dbObject.ObjectType, MRP2Consts.reltypeIdDocumentation, dbObject.Caption, dbObject.CheckoutBy, dbObject.OwnerID, 0L, dbObject.LCStep, (List<int>) null, (long) dbObject.VersionID, dbObject.IsBaseVersion ? 1L : 0L);
    }
    this.Initialize((IDescriptor) rootDescriptor, services);
  }

  /// <summary>Clean up any resources being used.</summary>
  /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  /// <summary>
  /// Required method for Designer support - do not modify
  /// the contents of this method with the code editor.
  /// </summary>
  private void InitializeComponent()
  {
    this.SuspendLayout();
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Name = nameof (ProductionCopyDocumentView);
    this.Size = new Size(505, 372);
    this.ResumeLayout(false);
  }

  private sealed class ProductionCopyDocumentViewDescriptionProvider : BaseViewDescriptionProvider
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
        Caption = "Документы на изделие",
        ImageIndex = namedImageList != null ? namedImageList.ImageIndex("imgContains") : -1,
        OrderID = 20
      };
    }
  }
}
