// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.Settings.LoggingObjectTypesEditor
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
namespace Intermech.Site.Client.Settings;

internal sealed class LoggingObjectTypesEditor : CollectionEditor
{
  public LoggingObjectTypesEditor()
    : base(typeof (List<LoggingObjectTypeItem>))
  {
  }

  public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
  {
    return (!(context.PropertyDescriptor.PropertyType == typeof (ChangeTrackingListAdapter<LoggingObjectTypeItem>)) ? 0 : (!context.PropertyDescriptor.IsReadOnly ? 1 : 0)) == 0 ? UITypeEditorEditStyle.None : base.GetEditStyle(context);
  }

  protected override bool CanSelectMultipleInstances() => false;

  protected override object CreateInstance(Type itemType) => (object) new LoggingObjectTypeItem();

  protected override string GetDisplayText(object value)
  {
    return value is LoggingObjectTypeItem loggingObjectTypeItem ? loggingObjectTypeItem.Name : base.GetDisplayText(value);
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
    ChangeTrackingListAdapter<LoggingObjectTypeItem> objA = (ChangeTrackingListAdapter<LoggingObjectTypeItem>) value;
    ChangeTrackingListAdapter<LoggingObjectTypeItem> objB = objA.Clone();
    base.EditValue(context, provider, (object) objB.Items);
    return !object.Equals((object) objA, (object) objB) ? (object) objB : (object) objA;
  }

  protected override CollectionEditor.CollectionForm CreateCollectionForm()
  {
    CollectionEditor.CollectionForm collectionForm = base.CreateCollectionForm();
    collectionForm.Text = "Список типов объектов";
    collectionForm.Width = 700;
    collectionForm.Height = 450;
    PropertyGrid control = (PropertyGrid) collectionForm.Controls[0].Controls[5];
    control.ToolbarVisible = false;
    control.HelpVisible = true;
    control.PropertySort = PropertySort.NoSort;
    control.SelectedObjectsChanged += new EventHandler(this.AttachHighlighter);
    return collectionForm;
  }
}
