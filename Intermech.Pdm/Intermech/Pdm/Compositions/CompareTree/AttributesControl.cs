// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.AttributesControl
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using Intermech.PropertyEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal class AttributesControl : UserControl
{
  private IContainer components;
  private TabControl tcProperties;
  private TabPage tpProperties;
  private TabPage tpRelationProperties;
  public CompareAttributesListControl ControlObjectAttributes;
  public CompareAttributesListControl ControlRelationAttributes;

  public AttributesControl()
  {
    this.InitializeComponent();
    INamedImageList service = (INamedImageList) ServicesManager.GetService(typeof (INamedImageList));
    if (service != null)
    {
      this.tcProperties.ImageList = service.ImageList;
      this.tpProperties.ImageIndex = service.ImageIndex("imgProp");
      this.tpRelationProperties.ImageIndex = service.ImageIndex("imgLink");
    }
    this.ControlObjectAttributes.Initialize();
    this.ControlRelationAttributes.Initialize();
  }

  public void RefreshAttributes(
    CompositionItem item,
    bool allAttributes,
    bool sortWithState,
    bool isRoot)
  {
    this.ControlObjectAttributes.Clear();
    this.ControlRelationAttributes.Clear();
    if (isRoot)
      this.tcProperties.TabPages.Remove(this.tpRelationProperties);
    else if (this.tcProperties.TabPages.Count == 1)
      this.tcProperties.TabPages.Add(this.tpRelationProperties);
    if (item == null || item.Attributes == null)
      return;
    List<CompositionItemAttribute> all1 = item.Attributes.FindAll((Predicate<CompositionItemAttribute>) (x => x.SourceType == AttributeSourceTypes.Object));
    if (allAttributes)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        foreach (IDBAttribute dbAttribute in sessionKeeper.Session.GetObject(item.ObjectID).Attributes.ToList())
        {
          IDBAttribute attribute = dbAttribute;
          if (!all1.Exists((Predicate<CompositionItemAttribute>) (x => x.AttributeID == attribute.AttributeID)))
            all1.Add(new CompositionItemAttribute(attribute.AttributeID, AttributeSourceTypes.Object, attribute.Value, attribute.Description));
        }
      }
    }
    IComparer<CompositionItemAttribute> comparer = !sortWithState ? (IComparer<CompositionItemAttribute>) new ListAttributesAlphabeticalSort() : (IComparer<CompositionItemAttribute>) new ListAttributesAlphabeticalAndStateSort();
    all1.Sort(comparer);
    this.ControlObjectAttributes.AddAtributes(all1, (IElementInfo) new Intermech.Client.Core.FormDesigner.Controls.ElementInfo(item.ObjectID, AttributableElements.Object));
    all1.Clear();
    if (isRoot)
      return;
    List<CompositionItemAttribute> all2 = item.Attributes.FindAll((Predicate<CompositionItemAttribute>) (x => x.SourceType == AttributeSourceTypes.Relation));
    if (allAttributes)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        foreach (IDBAttribute dbAttribute in sessionKeeper.Session.GetRelation(item.PrjLinkID).Attributes.ToList())
        {
          IDBAttribute attribute = dbAttribute;
          if (!all2.Exists((Predicate<CompositionItemAttribute>) (x => x.AttributeID == attribute.AttributeID)))
            all2.Add(new CompositionItemAttribute(attribute.AttributeID, AttributeSourceTypes.Relation, attribute.Value, attribute.Description));
        }
      }
    }
    all2.Sort(comparer);
    this.ControlRelationAttributes.AddAtributes(all2, (IElementInfo) new Intermech.Client.Core.FormDesigner.Controls.ElementInfo(item.PrjLinkID, AttributableElements.Relation));
    all2.Clear();
  }

  public void ResizeColumns()
  {
    this.ControlObjectAttributes.ResizeColumns(this.Name);
    this.ControlRelationAttributes.ResizeColumns(this.Name);
  }

  public TabControl TabControl => this.tcProperties;

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    this.tcProperties = new TabControl();
    this.tpProperties = new TabPage();
    this.tpRelationProperties = new TabPage();
    this.ControlObjectAttributes = new CompareAttributesListControl();
    this.ControlRelationAttributes = new CompareAttributesListControl();
    this.tcProperties.SuspendLayout();
    this.tpProperties.SuspendLayout();
    this.tpRelationProperties.SuspendLayout();
    this.ControlObjectAttributes.BeginInit();
    this.ControlRelationAttributes.BeginInit();
    this.SuspendLayout();
    this.tcProperties.Controls.Add((Control) this.tpProperties);
    this.tcProperties.Controls.Add((Control) this.tpRelationProperties);
    this.tcProperties.Dock = DockStyle.Fill;
    this.tcProperties.Location = new Point(0, 0);
    this.tcProperties.Name = "tcProperties";
    this.tcProperties.SelectedIndex = 0;
    this.tcProperties.Size = new Size(519, 311);
    this.tcProperties.TabIndex = 1;
    this.tpProperties.Controls.Add((Control) this.ControlObjectAttributes);
    this.tpProperties.Location = new Point(4, 22);
    this.tpProperties.Name = "tpProperties";
    this.tpProperties.Padding = new Padding(3);
    this.tpProperties.Size = new Size(511 /*0x01FF*/, 285);
    this.tpProperties.TabIndex = 0;
    this.tpProperties.Text = "Атрибуты объекта";
    this.tpProperties.UseVisualStyleBackColor = true;
    this.tpRelationProperties.Controls.Add((Control) this.ControlRelationAttributes);
    this.tpRelationProperties.Location = new Point(4, 22);
    this.tpRelationProperties.Name = "tpRelationProperties";
    this.tpRelationProperties.Padding = new Padding(3);
    this.tpRelationProperties.Size = new Size(884, 281);
    this.tpRelationProperties.TabIndex = 1;
    this.tpRelationProperties.Text = "Атрибуты связи";
    this.tpRelationProperties.UseVisualStyleBackColor = true;
    this.ControlObjectAttributes.AllowDrop = true;
    this.ControlObjectAttributes.DisableHeaderContextMenu = true;
    this.ControlObjectAttributes.Dock = DockStyle.Fill;
    this.ControlObjectAttributes.ImageList = (ImageList) null;
    this.ControlObjectAttributes.Location = new Point(3, 3);
    this.ControlObjectAttributes.Name = "controlObjectAttributes";
    this.ControlObjectAttributes.ShowRootRow = false;
    this.ControlObjectAttributes.Size = new Size(505, 279);
    this.ControlObjectAttributes.TabIndex = 0;
    this.ControlRelationAttributes.AllowDrop = true;
    this.ControlRelationAttributes.DisableHeaderContextMenu = true;
    this.ControlRelationAttributes.Dock = DockStyle.Fill;
    this.ControlRelationAttributes.ImageList = (ImageList) null;
    this.ControlRelationAttributes.Location = new Point(3, 3);
    this.ControlRelationAttributes.Name = "controlRelationAttributes";
    this.ControlRelationAttributes.ShowRootRow = false;
    this.ControlRelationAttributes.Size = new Size(878, 275);
    this.ControlRelationAttributes.TabIndex = 0;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.tcProperties);
    this.Name = nameof (AttributesControl);
    this.Size = new Size(519, 311);
    this.tcProperties.ResumeLayout(false);
    this.tpProperties.ResumeLayout(false);
    this.tpRelationProperties.ResumeLayout(false);
    this.ControlObjectAttributes.EndInit();
    this.ControlRelationAttributes.EndInit();
    this.ResumeLayout(false);
  }
}
