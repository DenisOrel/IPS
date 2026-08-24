// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.InseparableObjectTypesEditor
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.PropertyEditors.ChangeHighlighting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client;

internal sealed class InseparableObjectTypesEditor : CollectionEditor
{
  public InseparableObjectTypesEditor()
    : base(typeof (List<InseparableObjectTypesItem>))
  {
  }

  public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
  {
    return (!(context.PropertyDescriptor.PropertyType == typeof (ChangeTrackingListAdapter<InseparableObjectTypesItem>)) ? 0 : (!context.PropertyDescriptor.IsReadOnly ? 1 : 0)) == 0 ? UITypeEditorEditStyle.None : base.GetEditStyle(context);
  }

  protected override bool CanSelectMultipleInstances() => false;

  protected override object CreateInstance(Type itemType)
  {
    return (object) new InseparableObjectTypesItem();
  }

  protected override string GetDisplayText(object value)
  {
    return value is InseparableObjectTypesItem inseparableObjectTypesItem ? inseparableObjectTypesItem.Name : base.GetDisplayText(value);
  }

  protected override CollectionEditor.CollectionForm CreateCollectionForm()
  {
    CollectionEditor.CollectionForm collectionForm = base.CreateCollectionForm();
    collectionForm.Text = "Синхронно публикуемые типы объектов";
    collectionForm.Width = 700;
    collectionForm.Height = 450;
    PropertyGrid control = (PropertyGrid) collectionForm.Controls[0].Controls[5];
    control.ToolbarVisible = false;
    control.HelpVisible = true;
    control.PropertySort = PropertySort.NoSort;
    control.SelectedObjectsChanged += new EventHandler(this.AttachHighlighter);
    return collectionForm;
  }

  private void AttachHighlighter(object sender, EventArgs e)
  {
    PropertyGrid propertyGrid = (PropertyGrid) sender;
    if (propertyGrid.SelectedObject == null || !(propertyGrid.SelectedObject is ICloneable) || propertyGrid.SelectedObject is EditableObjectChangeHighlighter)
      return;
    propertyGrid.SelectedObject = (object) new EditableObjectChangeHighlighter((ICloneable) propertyGrid.SelectedObject);
  }

  public override object EditValue(
    ITypeDescriptorContext context,
    IServiceProvider provider,
    object value)
  {
    ChangeTrackingListAdapter<InseparableObjectTypesItem> objA = (ChangeTrackingListAdapter<InseparableObjectTypesItem>) value;
    ChangeTrackingListAdapter<InseparableObjectTypesItem> objB = objA.Clone();
    base.EditValue(context, provider, (object) objB.Items);
    return !object.Equals((object) objA, (object) objB) ? (object) objB : (object) objA;
  }
}
