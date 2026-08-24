// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.ElementListsUIEditor
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Interfaces;
using Intermech.PropertyEditors.ChangeHighlighting;
using Intermech.Tools.Integrators.Electrical;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MG.Integrator;

[Serializable]
internal sealed class ElementListsUIEditor : CollectionEditor
{
  public ElementListsUIEditor()
    : base(typeof (List<ElementListItemSurrogate>))
  {
  }

  public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
  {
    return (!(context.PropertyDescriptor.PropertyType == typeof (ChangeTrackingListAdapter<ElementListItemSurrogate>)) ? 0 : (!context.PropertyDescriptor.IsReadOnly ? 1 : 0)) == 0 ? UITypeEditorEditStyle.None : base.GetEditStyle(context);
  }

  protected override CollectionEditor.CollectionForm CreateCollectionForm()
  {
    CollectionEditor.CollectionForm collectionForm = base.CreateCollectionForm();
    collectionForm.Text = this.Context.PropertyDescriptor.DisplayName;
    collectionForm.Width = 645;
    collectionForm.Height = 400;
    collectionForm.Controls[0].Controls[3].Text = "&Типы перечней элементов:";
    collectionForm.Controls[0].Controls[2].Text = "&Атрибуты:";
    PropertyGrid itemGrid = (PropertyGrid) collectionForm.Controls[0].Controls[5];
    itemGrid.ToolbarVisible = false;
    itemGrid.HelpVisible = true;
    itemGrid.PropertySort = PropertySort.NoSort;
    itemGrid.SelectedObjectsChanged += new EventHandler(this.AttachHighlighter);
    itemGrid.ContextMenuStrip = new ContextMenuStrip();
    itemGrid.ContextMenuStrip.Items.Add("Очистить значение", (Image) null, (EventHandler) ((sender, e) => this.ResetAttributes(itemGrid)));
    itemGrid.ContextMenuStrip.Opening += (CancelEventHandler) ((sender, e) => this.ResetAttributesMenuOpening(itemGrid, e));
    return collectionForm;
  }

  private void AttachHighlighter(object sender, EventArgs e)
  {
    PropertyGrid propertyGrid = (PropertyGrid) sender;
    if (propertyGrid.SelectedObject == null || !(propertyGrid.SelectedObject is ICloneable) || propertyGrid.SelectedObject is EditableObjectChangeHighlighter)
      return;
    propertyGrid.SelectedObject = (object) new EditableObjectChangeHighlighter((ICloneable) propertyGrid.SelectedObject);
  }

  private void ResetAttributesMenuOpening(PropertyGrid itemGrid, CancelEventArgs e)
  {
    GridItem selectedGridItem = itemGrid.SelectedGridItem;
    e.Cancel = selectedGridItem == null || selectedGridItem.PropertyDescriptor.PropertyType != typeof (string) || selectedGridItem.Value == null;
  }

  private void ResetAttributes(PropertyGrid itemGrid)
  {
    if (itemGrid.SelectedGridItem == null || !(itemGrid.SelectedGridItem.PropertyDescriptor.PropertyType == typeof (string)))
      return;
    itemGrid.SelectedGridItem.PropertyDescriptor.SetValue(itemGrid.SelectedObject, (object) null);
    itemGrid.Refresh();
    itemGrid.Parent.Refresh();
  }

  public override object EditValue(
    ITypeDescriptorContext context,
    IServiceProvider provider,
    object value)
  {
    ChangeTrackingListAdapter<ElementListItemSurrogate> objA = (ChangeTrackingListAdapter<ElementListItemSurrogate>) value;
    ChangeTrackingListAdapter<ElementListItemSurrogate> objB = objA.Clone();
    base.EditValue(context, provider, (object) objB.Items);
    return !object.Equals((object) objA, (object) objB) ? (object) objB : (object) objA;
  }

  protected override object CreateInstance(Type itemType)
  {
    object instance = base.CreateInstance(itemType);
    if (instance.GetType() == typeof (AttributeTableItemSurrogate))
    {
      ElementListItemSurrogate listItemSurrogate = (ElementListItemSurrogate) instance;
      listItemSurrogate.ObjectType = (GlobalId<int>) null;
      listItemSurrogate.Suffix = "";
    }
    return instance;
  }

  protected override string GetDisplayText(object value)
  {
    ElementListItemSurrogate listItemSurrogate = (ElementListItemSurrogate) value;
    return listItemSurrogate.ObjectType == null ? string.Empty : listItemSurrogate.ObjectType.ToString();
  }
}
