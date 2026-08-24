// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.FieldSelectorUITypeEditor
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Imbase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Design;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class FieldSelectorUITypeEditor : UITypeEditor
{
  private IWindowsFormsEditorService _svc;
  private Guid _g = Guid.Empty;

  public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
  {
    return UITypeEditorEditStyle.DropDown;
  }

  public override object EditValue(
    ITypeDescriptorContext context,
    IServiceProvider provider,
    object value)
  {
    if (provider != null)
      this._svc = (IWindowsFormsEditorService) provider.GetService(typeof (IWindowsFormsEditorService));
    ListBox listBox = new ListBox()
    {
      BorderStyle = BorderStyle.None,
      IntegralHeight = false
    };
    if (context != null)
    {
      string str = Convert.ToString(context.Instance);
      if (GuidHelper.IsGuid(str))
        this._g = new Guid(str);
    }
    string str1 = Convert.ToString(value);
    Guid attrGuid = GuidHelper.IsGuid(str1) ? new Guid(str1) : Guid.Empty;
    if (this._g != Guid.Empty)
    {
      FieldSelectorUITypeEditor.LbItem selItem;
      List<FieldSelectorUITypeEditor.LbItem> lbItemList = this.LoadData(this._g, attrGuid, out selItem);
      if (lbItemList != null)
      {
        listBox.Items.AddRange((object[]) lbItemList.ToArray());
        IMainFormUpdate service = ServiceUtils.GetService<IMainFormUpdate>((object) ApplicationServices.Container, false);
        int num = (int) Math.Round((double) listBox.ItemHeight * (double) service.ScaleFactor.Height);
        listBox.Height = (listBox.Items.Count < 8 ? listBox.Items.Count * num : 8 * num) + num / 2;
        listBox.SelectedItem = (object) selItem;
        EventHandler eventHandler = (EventHandler) ((_param1, _param2) => this._svc.CloseDropDown());
        listBox.Click += eventHandler;
        this._svc.DropDownControl((Control) listBox);
        listBox.Click -= eventHandler;
        if (listBox.SelectedItem is FieldSelectorUITypeEditor.LbItem selectedItem)
          value = (object) selectedItem.G;
      }
    }
    return value;
  }

  private List<FieldSelectorUITypeEditor.LbItem> LoadData(
    Guid tableGuid,
    Guid attrGuid,
    out FieldSelectorUITypeEditor.LbItem selItem)
  {
    List<FieldSelectorUITypeEditor.LbItem> lbItemList = (List<FieldSelectorUITypeEditor.LbItem>) null;
    selItem = (FieldSelectorUITypeEditor.LbItem) null;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(tableGuid);
      if (!objectInfo.Empty)
      {
        if (sessionKeeper.Session.GetCustomService(typeof (IImbaseServer)) is IImbaseServer customService)
        {
          AttributeTypeProperties[] columnsAttributes;
          customService.LoadRecords(sessionKeeper.Session.SessionGUID, objectInfo.ObjectID, string.Empty, Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator, out DataTable _, out columnsAttributes, out ImbaseKeyInfo _);
          if (columnsAttributes != null)
          {
            if (columnsAttributes.Length != 0)
            {
              lbItemList = new List<FieldSelectorUITypeEditor.LbItem>(columnsAttributes.Length);
              foreach (AttributeTypeProperties attributeTypeProperties in columnsAttributes)
              {
                FieldSelectorUITypeEditor.LbItem lbItem = new FieldSelectorUITypeEditor.LbItem(attributeTypeProperties.Name, attributeTypeProperties.AttributeGuid);
                lbItemList.Add(lbItem);
                if (!(attributeTypeProperties.AttributeGuid != attrGuid))
                  selItem = lbItem;
              }
            }
          }
        }
      }
    }
    return lbItemList;
  }

  private class LbItem
  {
    internal string Caption { get; set; }

    internal Guid G { get; set; }

    public LbItem(string caption, Guid g)
    {
      this.Caption = caption;
      this.G = g;
    }

    public override string ToString() => this.Caption;
  }
}
