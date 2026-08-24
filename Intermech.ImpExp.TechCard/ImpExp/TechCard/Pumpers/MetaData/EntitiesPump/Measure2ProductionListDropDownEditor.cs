// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump.Measure2ProductionListDropDownEditor
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump;

public class Measure2ProductionListDropDownEditor : UITypeEditor
{
  private EntMeasureProdSetting _measureProdSett;

  public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
  {
    return context != null ? UITypeEditorEditStyle.DropDown : base.GetEditStyle((ITypeDescriptorContext) null);
  }

  public override object EditValue(
    ITypeDescriptorContext context,
    IServiceProvider provider,
    object value)
  {
    if (context != null && provider != null)
    {
      IWindowsFormsEditorService service = (IWindowsFormsEditorService) provider.GetService(typeof (IWindowsFormsEditorService));
      if (service != null)
      {
        Dictionary<long, List<int>> dictionary1 = (Dictionary<long, List<int>>) null;
        this._measureProdSett = (EntMeasureProdSetting) null;
        if (value is Dictionary<long, List<int>> dictionary2)
          dictionary1 = dictionary2;
        if (context.Instance is EntMeasureProdSetting instance)
          this._measureProdSett = instance;
        if (dictionary1 == null || this._measureProdSett == null)
          return (object) dictionary1;
        long key = -1;
        foreach (MeasureDescriptor measureDescriptor in EntityDescriptor.GetMeasureDescriptorsByPhisicalValueId(this._measureProdSett.PhysicalValueId))
        {
          if (context.PropertyDescriptor != null && measureDescriptor.LongName == context.PropertyDescriptor.DisplayName)
          {
            key = measureDescriptor.MeasureID;
            break;
          }
        }
        CheckedListBox checkedListBox1 = new CheckedListBox();
        checkedListBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
        checkedListBox1.CheckOnClick = true;
        checkedListBox1.Dock = DockStyle.Fill;
        CheckedListBox checkedListBox2 = checkedListBox1;
        if (key == -1L)
          return (object) dictionary1;
        for (int index = 0; index < this._measureProdSett.ProductionIDs.Length; ++index)
        {
          string productionNameById = Measure2ProductionListConverter.GetProductionNameById(this._measureProdSett.ProductionIDs[index]);
          checkedListBox2.Items.Add((object) productionNameById);
          if (dictionary1.ContainsKey(key) && dictionary1[key].Contains(this._measureProdSett.ProductionIDs[index]))
            checkedListBox2.SetItemCheckState(index, CheckState.Checked);
        }
        checkedListBox2.ItemCheck += new ItemCheckEventHandler(this.clb_ItemCheck);
        service.DropDownControl((Control) checkedListBox2);
        checkedListBox2.ItemCheck -= new ItemCheckEventHandler(this.clb_ItemCheck);
        List<int> intList = new List<int>();
        foreach (int checkedIndex in checkedListBox2.CheckedIndices)
        {
          int productionId = this._measureProdSett.ProductionIDs[checkedIndex];
          intList.Add(productionId);
        }
        if (!dictionary1.ContainsKey(key))
          dictionary1.Add(key, intList);
        dictionary1[key] = intList;
        return (object) dictionary1;
      }
    }
    return base.EditValue(context, provider, value);
  }

  private void clb_ItemCheck(object sender, ItemCheckEventArgs e)
  {
    if (e.NewValue != CheckState.Checked || this.IsCanCheck(e.Index))
      return;
    e.NewValue = CheckState.Unchecked;
  }

  private bool IsCanCheck(int index)
  {
    int prodId = this._measureProdSett.ProductionIDs[index];
    return this._measureProdSett.Measure2ProdList.Values.All<List<int>>((Func<List<int>, bool>) (tmpList => !tmpList.Contains(prodId)));
  }
}
