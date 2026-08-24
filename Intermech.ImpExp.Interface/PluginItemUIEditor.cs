// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.PluginItemUIEditor
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.PropertyEditors.ChangeHighlighting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>Редактор записи о плагине</summary>
[Serializable]
internal sealed class PluginItemUIEditor : CollectionEditor
{
  public PluginItemUIEditor()
    : base(typeof (List<PluginItem>))
  {
  }

  public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
  {
    return (!(context.PropertyDescriptor.PropertyType == typeof (ChangeTrackingListAdapter<PluginItem>)) ? 0 : (!context.PropertyDescriptor.IsReadOnly ? 1 : 0)) == 0 ? UITypeEditorEditStyle.None : base.GetEditStyle(context);
  }

  protected override CollectionEditor.CollectionForm CreateCollectionForm()
  {
    CollectionEditor.CollectionForm collectionForm = base.CreateCollectionForm();
    collectionForm.Text = this.Context.PropertyDescriptor.DisplayName;
    collectionForm.Width = 800;
    collectionForm.Height = 400;
    collectionForm.Controls[0].Controls[3].Text = "&Загружаемые модули:";
    foreach (Control control in (ArrangedElementCollection) (collectionForm.Controls[0] as TableLayoutPanel).Controls)
    {
      if (control.GetType().ToString() == "System.ComponentModel.Design.CollectionEditor+FilterListBox")
        control.Width = 305;
    }
    PropertyGrid itemGrid = (PropertyGrid) collectionForm.Controls[0].Controls[5];
    itemGrid.ToolbarVisible = false;
    itemGrid.HelpVisible = true;
    itemGrid.LargeButtons = true;
    itemGrid.AutoScaleMode = AutoScaleMode.Dpi;
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
    ChangeTrackingListAdapter<PluginItem> objA = (ChangeTrackingListAdapter<PluginItem>) value;
    ChangeTrackingListAdapter<PluginItem> objB = objA.Clone();
    base.EditValue(context, provider, (object) objB.Items);
    return !object.Equals((object) objA, (object) objB) ? (object) objB : (object) objA;
  }

  protected override object CreateInstance(Type itemType)
  {
    object instance = base.CreateInstance(itemType);
    if (instance.GetType() == typeof (PluginItem))
    {
      PluginItem pluginItem = (PluginItem) instance;
      pluginItem.FileName = string.Empty;
      pluginItem.Description = string.Empty;
      pluginItem.Enable = false;
    }
    return instance;
  }

  protected override string GetDisplayText(object value)
  {
    PluginItem pluginItem = (PluginItem) value;
    string empty = string.Empty;
    return $"{(!(pluginItem.FileName != string.Empty) ? (object) "Не определен" : (object) new FileInfo(pluginItem.FileName).Name)} ({(pluginItem.Enable ? (object) "загружаемый" : (object) "незагружаемый")})";
  }
}
