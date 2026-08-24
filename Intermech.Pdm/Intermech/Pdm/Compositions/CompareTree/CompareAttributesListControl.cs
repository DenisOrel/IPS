// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.CompareAttributesListControl
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Infralution.Controls.VirtualTree;
using Intermech.Client.Core;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.PropertyEditors;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class CompareAttributesListControl : Intermech.VirtualTreeView.VirtualTreeView
{
  private string _configName;
  private readonly IAttributePropertyDescriberService _propertyDescriberService;
  private IElementInfo _currentElementInfo;

  public CompareAttributesListControl()
  {
    this._propertyDescriberService = ServicesManager.GetService(typeof (IAttributePropertyDescriberService)) as IAttributePropertyDescriberService;
  }

  public void ResizeColumns(string parentName)
  {
    this._configName = $"{parentName}_{this.Name}";
    Hashtable hashtable = new Hashtable();
    FormStorage.LoadLayout((Control) this, this._configName, (IDictionary) hashtable, true, out Point _, out Size _);
    if (hashtable.ContainsKey((object) "Column1_Width"))
    {
      this.Columns[0].Width = (int) hashtable[(object) "Column1_Width"];
      this.Columns[1].Width = (int) hashtable[(object) "Column2_Width"];
    }
    else
    {
      this.Columns[0].Width = this.Width / 2;
      this.Columns[1].Width = this.Width / 2;
    }
  }

  protected override void Dispose(bool disposing)
  {
    if (!string.IsNullOrEmpty(this._configName))
      FormStorage.SaveLayout((Control) this, this._configName, (IDictionary) new Hashtable()
      {
        [(object) "Column1_Width"] = (object) this.Columns[0].Width,
        [(object) "Column2_Width"] = (object) this.Columns[1].Width
      });
    base.Dispose(disposing);
  }

  public void Initialize()
  {
    this.GetCellData += new GetCellDataHandler(this.CompareAttributesListControl_GetCellData);
    this.AllowDrop = true;
    this.DisableHeaderContextMenu = true;
    this.ShowRootRow = false;
    this.AddColumn("Атрибут", "F_ATTRIBUTE_ID");
    this.AddColumn("Значение", "F_VALUE");
  }

  protected override CellWidget CreateCellWidget(RowWidget rowWidget, Column column)
  {
    return (CellWidget) new ValueCellWidget(rowWidget, column);
  }

  public void Clear()
  {
    this.DataSource = (object) null;
    this._currentElementInfo = (IElementInfo) null;
  }

  private Column AddColumn(string caption, string name)
  {
    Column column = new Column()
    {
      Caption = caption,
      Name = name,
      Sortable = false
    };
    column.CellStyle.HorzAlignment = StringAlignment.Near;
    this.Columns.Add(column);
    return column;
  }

  private void CompareAttributesListControl_GetCellData(object sender, GetCellDataEventArgs e)
  {
    IAttributesListControlCellDataHandler controlCellDataHandler = (IAttributesListControlCellDataHandler) null;
    if (e.Row.Item is CompositionItemAttribute compositionItemAttribute)
      controlCellDataHandler = (IAttributesListControlCellDataHandler) new ItemAttributesListControlCellDataHandler(compositionItemAttribute);
    if (e.Row.Item is CompositionItemAttributeValue itemAttributeValue)
      controlCellDataHandler = (IAttributesListControlCellDataHandler) new ValueAttributesListControlCellDataHandler(itemAttributeValue);
    if (controlCellDataHandler == null)
      return;
    controlCellDataHandler.SetDataValue(e, this._propertyDescriberService, this._currentElementInfo);
    controlCellDataHandler.SetBackColor(e);
  }

  public VScrollBar VScrollBar => this.VertScrollBar;

  public void AddAtributes(
    List<CompositionItemAttribute> compositionItemAttributes,
    IElementInfo elementInfo)
  {
    CompositionItemAttributes compositionItemAttributes1 = new CompositionItemAttributes();
    compositionItemAttributes1.AddRange((IEnumerable<CompositionItemAttribute>) compositionItemAttributes);
    this._currentElementInfo = elementInfo;
    this.DataSource = (object) compositionItemAttributes1;
  }
}
