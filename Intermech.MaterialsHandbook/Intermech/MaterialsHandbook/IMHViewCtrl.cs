// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.IMHViewCtrl
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Expressions;
using Intermech.Imbase;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Imbase;
using Intermech.Interfaces.MaterialsHandbook;
using Intermech.Localization;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

[ToolboxItem(false)]
public class IMHViewCtrl : IMHViewCtrlBase
{
  private DataSet _ds;
  private DataTable _dtData;
  private DataTable _dtAttrs;
  private DataTable _dtView;
  private Dictionary<string, AttributeTypeProperties> _dictAttrTypeProps;
  private List<string> _formulaColumns = new List<string>();
  private List<string> _visibleWithoutListColumns = new List<string>();
  private List<string> _visibleWithoutFormulaColumns = new List<string>();
  private List<string> _listColumns = new List<string>();
  private ExpressionTree _expressionTree;
  private TreeListNode _selectedNode;
  private FilterList _filter = new FilterList();
  protected string _formulaText = string.Empty;
  protected string _classAttrValue = string.Empty;
  private Dictionary<long, TreeListNode> _nodes;
  protected bool _isMaterial = true;
  protected bool _isStandart;
  protected bool _lock;
  private string _classColumnName;
  private string _usingColumnName;
  private IContainer components;
  private ImageList _img;
  private ToolStrip _tsDimensionType;
  private ToolStripLabel _tsLabel;
  private ToolStripSeparator _tsSeparator31;
  private ToolStripButton _tsBtnImbase;
  private ToolStripButton _tsBtnRefresh;
  private ToolStripSeparator _tsSeparator4;
  private ToolStripSeparator _tsSeparator5;
  private ToolStripButton _tsBtnConfig;
  private ToolStripButton _tsBtnEdit;
  private ToolStripButton _tsBtnDel;
  protected TreeListView _tlv;
  private ContextMenuStrip _contextMenuAssortment;
  private ToolStripMenuItem _cmImbase;
  private ToolStripMenuItem _cmRefresh;
  private ToolStripMenuItem _cmFilter;
  private ToolStripMenuItem _cmConfig;
  private ToolStripMenuItem _cmEdit;
  private ToolStripMenuItem _cmDel;
  private ToolStripSeparator _tsHSeparator2;
  private ToolStripSeparator _tsHSeparator3;
  protected ToolStripButton _tsBtnAssortmentApplicabilityFilter;
  private ToolStripButton _tsBtnFilter;

  public IMHViewCtrl()
  {
    this.InitializeComponent();
    this._tsDimensionType.ImageList = this._img;
    this._tsBtnFilter.ImageIndex = 0;
    this._classColumnName = Intermech.Imbase.Consts.ClassAttrGuid.ToString();
    this._usingColumnName = Intermech.Imbase.Consts.ImbaseUsingAttGUID.ToString();
    this._tsShowInImbase.Visible = this._cmShowInImbase.Visible = false;
  }

  private void On_tlv_Enter(object sender, EventArgs e) => this.TreeListViewEnter(e);

  private void On_tlv_SelectedChanged(object sender, EventArgs e)
  {
    TreeListNode treeListNode = sender as TreeListNode;
    bool selectable = true;
    if (treeListNode != null && treeListNode.Selected)
    {
      this._selectedNode = treeListNode;
      selectable = treeListNode.ForeColor != SystemColors.GrayText;
      if (this._selectedNode.Row != null)
      {
        this._aRecID = Convert.ToInt64(this._selectedNode.Row["F_KEY"]);
        if (this._selectedNode.Row.Table.Columns.Contains(this._classColumnName))
          this._classAttrValue = Convert.ToString(this._selectedNode.Row[this._classColumnName]);
      }
      else
        this._aRecID = -1L;
    }
    else
    {
      this._selectedNode = (TreeListNode) null;
      this._aRecID = -1L;
    }
    this._aCaption = this.GetParsedFormula(this._selectedNode);
    this._formulaText = this._aCaption;
    this._pnlFormula.Invalidate();
    ToolStripMenuItem cmConfig = this._cmConfig;
    ToolStripMenuItem cmEdit = this._cmEdit;
    ToolStripButton tsBtnConfig = this._tsBtnConfig;
    bool flag1;
    this._tsBtnEdit.Enabled = flag1 = this._selectedNode?.Row != null && this._listColumns.Count > 0;
    int num1;
    bool flag2 = (num1 = flag1 ? 1 : 0) != 0;
    tsBtnConfig.Enabled = num1 != 0;
    int num2;
    bool flag3 = (num2 = flag2 ? 1 : 0) != 0;
    cmEdit.Enabled = num2 != 0;
    int num3 = flag3 ? 1 : 0;
    cmConfig.Enabled = num3 != 0;
    this._cmDel.Enabled = this._tsBtnDel.Enabled = this._selectedNode != null;
    this.OnIMHMaterialChanged(this._aTableRefID, this._aRecID, selectable);
  }

  private void _tlv_DoubleClick(object sender, EventArgs e)
  {
    if (this._selectedNode == null || this._services == null || !(this._services.GetService(typeof (ISelectionWindow)) is ISelectionWindow service))
      return;
    service.OkButton.PerformClick();
  }

  private void On_tsBtnImbase_Click(object sender, EventArgs e)
  {
    this.GotoImbase(this._aTableRefID, this._aRecID);
  }

  private void On_tsBtnRefresh_Click(object sender, EventArgs e) => this.LoadAssortmentTable();

  private void On_tsBtnFilter_Click(object sender, EventArgs e)
  {
    List<string> attrGuids = new List<string>((IEnumerable<string>) this._formulaColumns);
    attrGuids.AddRange((IEnumerable<string>) this._visibleWithoutFormulaColumns.ToArray());
    using (FilterForm filterForm = new FilterForm(attrGuids, this._filter))
    {
      int num = (int) filterForm.ShowDialog();
      this._filter = filterForm.Filter;
      this.ReloadRows();
      this._tsBtnFilter.ImageIndex = this._filter.Count > 0 ? 1 : 0;
    }
  }

  private void On_tsBtnConfig_Click(object sender, EventArgs e)
  {
    if (this._selectedNode == null)
      return;
    using (ConfigDimensionTypeForm dimensionTypeForm = new ConfigDimensionTypeForm(this._listColumns, this._dictAttrTypeProps, this._selectedNode, this._expressionTree, true))
    {
      dimensionTypeForm.ClassAttrValue = this._classAttrValue;
      if (dimensionTypeForm.ShowDialog() != DialogResult.OK)
        return;
      Dictionary<string, object> values = dimensionTypeForm.Values;
      bool flag = true;
      TreeListNode treeListNode = (TreeListNode) null;
      if (this._selectedNode.Parent.Visible)
      {
        treeListNode = this._selectedNode.Parent;
        foreach (TreeListNode node in (List<TreeListNode>) this._selectedNode.Parent.Nodes)
        {
          flag = this.IsUniqueData(node.Row, values);
          if (!flag)
          {
            node.Selected = true;
            break;
          }
        }
      }
      else
        flag = this.IsUniqueData(this._selectedNode.Row, values);
      if (flag)
      {
        DataRow newRow = this.CreateNewRow(this._selectedNode.Row, values);
        bool readOnly = !newRow.Table.Columns.Contains(this._usingColumnName) ? !this.CheckEnabledItemFromUsingAttr(this._aTableRefID) : Convert.ToString(newRow[this._usingColumnName]) == "-";
        TreeListNode node = this.CreateNode(newRow, this._formulaColumns, this._visibleWithoutFormulaColumns, false, readOnly);
        node.AdditionalValues = values;
        if (treeListNode == null)
        {
          TreeListNode selectedNode = this._selectedNode;
          treeListNode = this.CreateNode(this._selectedNode.Row, this._formulaColumns, this._visibleWithoutFormulaColumns, true, false);
          int index = selectedNode.Index;
          this._tlv.Nodes.Remove(selectedNode);
          treeListNode.Nodes.Add(selectedNode);
          treeListNode.Expanded = true;
          this._tlv.Nodes.Insert(index, treeListNode);
        }
        treeListNode.Nodes.Add(node);
        this.CorrectParentNode(treeListNode);
        node.Selected = true;
        this._tlv.Invalidate();
        this._formulaText = this._aCaption = this.GetParsedFormula(this._selectedNode);
        this.SaveChanges();
      }
      else
      {
        string caption = LocalizationHolder.rm.GetString("IMH_DuplicationRecord_Caption");
        int num = (int) MessageBox.Show(LocalizationHolder.rm.GetString("IMH_DuplicationRecord_Msg"), caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }
  }

  private void On_tsBtnEdit_Click(object sender, EventArgs e)
  {
    if (this._selectedNode == null)
      return;
    using (ConfigDimensionTypeForm dimensionTypeForm = new ConfigDimensionTypeForm(this._listColumns, this._dictAttrTypeProps, this._selectedNode, this._expressionTree, false))
    {
      dimensionTypeForm.ClassAttrValue = this._classAttrValue;
      if (dimensionTypeForm.ShowDialog() == DialogResult.OK)
      {
        Dictionary<string, object> values = dimensionTypeForm.Values;
        bool flag = true;
        if (this._selectedNode.Parent.Visible)
        {
          foreach (TreeListNode node in (List<TreeListNode>) this._selectedNode.Parent.Nodes)
          {
            if (node != this._selectedNode)
            {
              flag = this.IsUniqueData(node.Row, values);
              if (!flag)
              {
                node.Selected = true;
                break;
              }
            }
          }
        }
        if (flag)
        {
          this._selectedNode.AdditionalValues = values;
          DataRow row = this._selectedNode.Row;
          long key = Convert.ToInt64(row["F_KEY"]);
          DataRow dataRow = this._dtData.AsEnumerable().FirstOrDefault<DataRow>((System.Func<DataRow, bool>) (x => Convert.ToInt64(x["F_KEY"]) == key));
          foreach (KeyValuePair<string, object> keyValuePair in values)
          {
            if (dataRow != null)
              row[keyValuePair.Key] = dataRow[keyValuePair.Key] = keyValuePair.Value;
            this._selectedNode.SetValue(keyValuePair.Key, keyValuePair.Value);
          }
          this._formulaText = this._aCaption = this.GetParsedFormula(this._selectedNode);
          this.CorrectParentNode(this._selectedNode.Parent);
          this.SaveChanges();
        }
        else
        {
          string caption = LocalizationHolder.rm.GetString("IMH_DuplicationRecord_Caption");
          int num = (int) MessageBox.Show(LocalizationHolder.rm.GetString("IMH_DuplicationRecord_Msg"), caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
      }
    }
    this._tlv.Invalidate();
    this._pnlFormula.Invalidate();
  }

  private void On_tsBtnDel_Click(object sender, EventArgs e)
  {
    string caption = LocalizationHolder.rm.GetString("IMH_DeleteRecord_Caption");
    if (MessageBox.Show(LocalizationHolder.rm.GetString("IMH_DeleteRecord_Msg"), caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes || this._selectedNode == null)
      return;
    List<FavouriteData> assortmentFavourites = (List<FavouriteData>) null;
    if (this._selectedNode.Nodes.Count > 0)
    {
      assortmentFavourites = new List<FavouriteData>(this._selectedNode.Nodes.Count);
      foreach (TreeListNode node in (List<TreeListNode>) this._selectedNode.Nodes)
      {
        FavouriteData favouriteData = this.GetFavouriteData(node);
        if (favouriteData != null)
          assortmentFavourites.Add(favouriteData);
        this.RemoveNode(node);
      }
      this._selectedNode.Nodes.Clear();
      this._selectedNode.Parent.Nodes.Remove(this._selectedNode);
    }
    else
    {
      FavouriteData favouriteData = this.GetFavouriteData(this._selectedNode);
      if (favouriteData != null)
        assortmentFavourites = new List<FavouriteData>()
        {
          favouriteData
        };
      this.RemoveNode(this._selectedNode);
      TreeListNode parent = this._selectedNode.Parent;
      parent.Nodes.Remove(this._selectedNode);
      if (parent.Visible)
      {
        if (parent.Nodes.Count == 1)
        {
          TreeListNode node = parent.Nodes[0];
          int index = parent.Index;
          parent.Nodes.Clear();
          parent.Parent.Nodes.Remove(parent);
          this._tlv.Nodes.Insert(index, node);
          node.Selected = true;
        }
        else
          this.CorrectParentNode(parent);
      }
    }
    this.SaveChanges();
    if (assortmentFavourites == null || assortmentFavourites.Count == 0)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(sessionKeeper.Session.GetCustomService(typeof (IIMHUserSettingsService)) is IIMHUserSettingsService customService))
        return;
      customService.RemoveAssortmentFavourites(this._categoryNodeGuid, assortmentFavourites);
    }
  }

  private void On_tsBtnAssortmentApplicabilityFilter_Click(object sender, EventArgs e)
  {
    this._tsBtnAssortmentApplicabilityFilter.Checked = !this._tsBtnAssortmentApplicabilityFilter.Checked;
    this.CheckAssortmentApplicabilityFilterState();
    this.ReloadRows();
  }

  public override void Initialize(
    ISelectedItems items,
    IServiceProvider provider,
    NavigatorTreeNode parentINode)
  {
    base.Initialize(items, provider, parentINode);
    bool result = true;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (sessionKeeper.Session.GetCustomService(typeof (IIMHSystemSettingsService)) is IIMHSystemSettingsService customService)
      {
        if (!bool.TryParse(Convert.ToString(customService.GetValueByName("DISPLAY_SETTING_SHOW_RECORDS")), out result))
          result = true;
      }
    }
    this._tsBtnAssortmentApplicabilityFilter.Checked = !result;
    this.CheckAssortmentApplicabilityFilterState();
  }

  private string ApplyQuotes(string value)
  {
    if (!string.IsNullOrEmpty(value) && value[0] != '\'')
      value = $"'{value}'";
    return value;
  }

  private string BuildFilterString(SortamentFilter filter)
  {
    string str1 = string.Empty;
    string key = filter.AttrGuid.ToString();
    if (this._dictAttrTypeProps.ContainsKey(key))
    {
      AttributeTypeProperties dictAttrTypeProp = this._dictAttrTypeProps[key];
      Condition cond = filter.Cond;
      string str2 = Convert.ToString(filter.Value);
      if (!string.IsNullOrEmpty(str2) || cond == Condition.Equal || cond == Condition.NotEqual)
      {
        FieldTypes fieldType = dictAttrTypeProp.FieldType;
        bool flag = false;
        switch (fieldType)
        {
          case FieldTypes.ftString:
          case FieldTypes.ftObjectLink:
          case FieldTypes.ftMemo:
          case FieldTypes.ftGuid:
            flag = true;
            break;
          case FieldTypes.ftDouble:
          case FieldTypes.ftMeasured:
            str2 = str2.Replace(',', '.');
            break;
        }
        string str3 = $"[{key}]";
        if (!flag)
        {
          char[] charArray = str2.ToCharArray();
          for (int index = 0; index < charArray.Length; ++index)
          {
            char c = charArray[index];
            switch (c)
            {
              case '-':
              case '.':
              case ';':
                continue;
              default:
                if (!char.IsDigit(c))
                {
                  string caption = LocalizationHolder.rm.GetString("IMH_FilterError_Caption");
                  int num = (int) MessageBox.Show(string.Format(LocalizationHolder.rm.GetString("IMH_FilterError_Caption"), (object) str2, (object) index, (object) c), caption, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                  throw new AbortException();
                }
                continue;
            }
          }
        }
        string str4 = string.Empty;
        switch (cond)
        {
          case Condition.Equal:
            if (flag)
            {
              if (str2.IndexOfAny(new char[2]{ '*', '?' }) != -1)
              {
                str2 = str2.Replace('?', '_').Replace('*', '%');
                str1 = $"{str3} LIKE {this.ApplyQuotes(str2)}";
                break;
              }
            }
            str1 = !string.IsNullOrEmpty(str2) ? $"{str3}={(flag ? this.ApplyQuotes(str2) : str2)}" : (flag ? string.Format("{0}='' OR {0} is NULL", (object) str3) : str3 + " is NULL");
            break;
          case Condition.NotEqual:
            if (flag)
            {
              if (str2.IndexOfAny(new char[2]{ '*', '?' }) != -1)
              {
                str2 = str2.Replace('?', '_').Replace('*', '%');
                str1 = $"{str3} NOT LIKE {this.ApplyQuotes(str2)}";
                break;
              }
            }
            str1 = !string.IsNullOrEmpty(str2) ? string.Format("{0}<>{1} OR {0} is NULL", (object) str3, flag ? (object) this.ApplyQuotes(str2) : (object) str2) : (flag ? string.Format("{0}<>'' AND {0} is not NULL", (object) str3) : str3 + " is not NULL");
            break;
          case Condition.Substring:
            str2 = $"%{str2.Replace("*", "[*]").Replace("%", "[%]")}%";
            if (flag)
              str2 = this.ApplyQuotes(str2);
            str1 = $"{str3} LIKE {str2}";
            break;
          case Condition.Great:
            str4 = ">";
            break;
          case Condition.GreatOrEqual:
            str4 = ">=";
            break;
          case Condition.Less:
            str4 = "<";
            break;
          case Condition.LessOrEqual:
            str4 = "<=";
            break;
        }
        if (str4.Length > 0)
        {
          if (flag)
            str2 = this.ApplyQuotes(str2);
          str1 = str3 + str4 + str2;
        }
      }
    }
    return str1;
  }

  private void CheckAccessRights(long objID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject objectActualCopy = sessionKeeper.Session.GetObjectActualCopy(objID, false);
      long userId = sessionKeeper.Session.UserID;
      if (objectActualCopy == null || userId == 0L)
      {
        this._tsBtnConfig.Visible = this._tsBtnEdit.Visible = this._tsBtnDel.Visible = false;
        this._cmConfig.Visible = this._cmEdit.Visible = this._cmDel.Visible = false;
      }
      else
      {
        IDBSecurity dbSecurity = objectActualCopy as IDBSecurity;
        long checkoutBy = objectActualCopy.CheckoutBy;
        if (dbSecurity != null)
        {
          ObjectModifyModes objectModifyMode = objectActualCopy.ObjectModifyMode;
          bool flag = !dbSecurity.CheckAccess(ActionType.Edit, true, false) || objectModifyMode == ObjectModifyModes.CantModify || objectModifyMode == ObjectModifyModes.Checkout && checkoutBy != 0L && checkoutBy != userId || objectModifyMode == ObjectModifyModes.CreateVersion && checkoutBy != 0L && checkoutBy != userId;
          this._tsBtnConfig.Visible = this._tsBtnEdit.Visible = this._tsBtnDel.Visible = !flag;
          this._cmConfig.Visible = this._cmEdit.Visible = this._cmDel.Visible = !flag;
        }
        else
        {
          this._tsBtnConfig.Visible = this._tsBtnEdit.Visible = this._tsBtnDel.Visible = false;
          this._cmConfig.Visible = this._cmEdit.Visible = this._cmDel.Visible = false;
        }
        if (!this._tsBtnDel.Visible || objID != this._aTableRefID)
          return;
        this.CheckAccessRights(IMHHelper.GetTableIDByTableRefID(this._aTableRefID));
      }
    }
  }

  private void CheckAssortmentApplicabilityFilterState()
  {
    this._tsBtnAssortmentApplicabilityFilter.ImageIndex = this._tsBtnAssortmentApplicabilityFilter.Checked ? 1 : 0;
  }

  private void ClearTlvData()
  {
    this._ds = (DataSet) null;
    this._dtData = (DataTable) null;
    this._dtAttrs = (DataTable) null;
    this._dtView = (DataTable) null;
    this._expressionTree = (ExpressionTree) null;
    this._tlv.SuspendLayout();
    try
    {
      this._tlv.Nodes.Clear();
      this._tlv.Columns.Clear();
    }
    finally
    {
      this._tlv.ResumeLayout();
    }
    this._formulaColumns.Clear();
    this._visibleWithoutListColumns.Clear();
    this._visibleWithoutFormulaColumns.Clear();
    this._listColumns.Clear();
    this._dictAttrTypeProps?.Clear();
  }

  private void CorrectParentNode(TreeListNode parentNode)
  {
    if (parentNode == null || parentNode.Nodes.Count <= 0)
      return;
    string str1 = parentNode.Nodes[0].Text;
    for (int index = 1; index < parentNode.Nodes.Count; ++index)
    {
      if (!(str1 == parentNode.Nodes[index].Text))
      {
        str1 = string.Empty;
        break;
      }
    }
    parentNode.Value = (object) str1;
    if (parentNode.SubNodes.Count <= 0)
      return;
    for (int index1 = 0; index1 < parentNode.SubNodes.Count; ++index1)
    {
      string str2 = parentNode.Nodes[0].SubNodes[index1].Text;
      for (int index2 = 1; index2 < parentNode.Nodes.Count; ++index2)
      {
        if (!(str2 == parentNode.Nodes[index2].SubNodes[index1].Text))
        {
          str2 = string.Empty;
          break;
        }
      }
      parentNode.SubNodes[index1].Value = (object) str2;
    }
  }

  private void CreateColumns(List<string> attrGuids, DataTable dtData)
  {
    if (attrGuids.Count <= 0)
      return;
    foreach (string attrGuid in attrGuids)
    {
      DataTable dataTable = (DataTable) null;
      string str = string.Empty;
      string text = attrGuid;
      if (this._dictAttrTypeProps.ContainsKey(attrGuid))
      {
        AttributeTypeProperties dictAttrTypeProp = this._dictAttrTypeProps[attrGuid];
        PropertyCollection extendedProperties = dtData.Columns[attrGuid].ExtendedProperties;
        text = this.GetColumnCaption(dictAttrTypeProp, extendedProperties.Contains((object) "F_MEASURE_U") ? Convert.ToString(extendedProperties[(object) "F_MEASURE_U"]) : string.Empty);
        if (dictAttrTypeProp.MultiValueMode == MultiValueModes.SingleValueFromList)
        {
          IDBAttributeTypeInfo attributeType = ApplicationServices.Container.GetService<IClientMetadataCache>().GetAttributeType(dictAttrTypeProp.AttributeGuid, false);
          if (attributeType.AttributeType == FieldTypes.ftMeasured && extendedProperties.Contains((object) "F_MEASURE"))
          {
            object obj = extendedProperties[(object) "F_MEASURE"];
            dataTable = obj == null || obj == DBNull.Value ? IMHHelper.GetPossibleValues(attributeType) : IMHHelper.GetPossibleValues(attributeType, Convert.ToInt64(obj));
          }
          else
            dataTable = IMHHelper.GetPossibleValues(attributeType);
          str = attributeType.PossibleValueFieldName;
        }
      }
      ColumnHeader colHeader = new ColumnHeader(text, 150);
      if (dataTable != null)
      {
        colHeader.DataSource = dataTable;
        colHeader.DisplayMember = "F_DESCRIPTION";
        colHeader.ValueMember = str;
      }
      this._tlv.Columns.Add(colHeader);
    }
  }

  private string CreateFilterString(bool addApplicabilityFilter)
  {
    StringBuilder stringBuilder = new StringBuilder();
    foreach (KeyValuePair<Guid, SortamentFilter> keyValuePair in this._filter.Dict)
    {
      string str = this.BuildFilterString(keyValuePair.Value);
      if (!string.IsNullOrEmpty(str))
        stringBuilder.Append(stringBuilder.Length > 0 ? $" AND ({str})" : $"({str})");
    }
    if (addApplicabilityFilter && this._tsBtnAssortmentApplicabilityFilter.ImageIndex == 1)
      stringBuilder.Append(stringBuilder.Length > 0 ? $" AND ([{Intermech.Imbase.Consts.ImbaseUsingAttGUID}]='+')" : $"[{Intermech.Imbase.Consts.ImbaseUsingAttGUID}]='+'");
    return stringBuilder.ToString();
  }

  private DataRow CreateNewRow(DataRow row, Dictionary<string, object> values)
  {
    DataTable table1 = row.Table;
    long nKey = Convert.ToInt64(row["F_KEY"]);
    DataRow sourceRow = this._dtData.AsEnumerable().FirstOrDefault<DataRow>((System.Func<DataRow, bool>) (x => Convert.ToInt64(x["F_KEY"]) == nKey));
    Guid rowGuid = Guid.NewGuid();
    DataRow table2 = this.AddRowToTable(table1, row, rowGuid);
    DataRow table3 = this.AddRowToTable(this._dtData, sourceRow, rowGuid);
    if (values != null)
    {
      foreach (KeyValuePair<string, object> keyValuePair in values)
        table2[keyValuePair.Key] = keyValuePair.Value == null || keyValuePair.Value == DBNull.Value ? (table3[keyValuePair.Key] = (object) DBNull.Value) : (table3[keyValuePair.Key] = keyValuePair.Value);
    }
    return table2;
  }

  private DataRow AddRowToTable(DataTable dt, DataRow sourceRow, Guid rowGuid)
  {
    DataRow row = dt.NewRow();
    long int64 = Convert.ToInt64(row["F_KEY"]);
    row.ItemArray = sourceRow.ItemArray;
    row["F_KEY"] = (object) int64;
    row["F_GUID"] = (object) rowGuid;
    dt.Rows.Add(row);
    return row;
  }

  private TreeListNode CreateNode(
    DataRow row,
    List<string> inFormulaColumns,
    List<string> visibleColumns,
    bool isRoot,
    bool readOnly)
  {
    TreeListNode treeListNode = (TreeListNode) null;
    string str1 = string.Empty;
    int num = 0;
    if (inFormulaColumns != null && inFormulaColumns.Count > 0)
    {
      List<string> stringList = inFormulaColumns;
      int index1 = num;
      int index2 = index1 + 1;
      str1 = stringList[index1];
      string str2 = !isRoot || !this._listColumns.Contains(str1) ? Convert.ToString(row[str1]) : string.Empty;
      treeListNode = new TreeListNode(MetaDataHelper.GetAttributeType(new Guid(str1)).FieldType, isRoot ? string.Empty : str1, str2);
      for (; index2 < inFormulaColumns.Count; ++index2)
      {
        str1 = inFormulaColumns[index2];
        string text = !isRoot || !this._listColumns.Contains(str1) ? Convert.ToString(row[str1]) : string.Empty;
        IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(new Guid(str1));
        treeListNode.SubNodes.Add(attributeType.FieldType, str1, text);
      }
    }
    if (visibleColumns != null && visibleColumns.Count > 0)
    {
      int index = 0;
      if (treeListNode == null)
      {
        str1 = visibleColumns[index++];
        treeListNode = new TreeListNode(MetaDataHelper.GetAttributeType(new Guid(str1)).FieldType, isRoot ? string.Empty : str1, Convert.ToString(row[str1]));
      }
      for (; index < visibleColumns.Count; ++index)
      {
        str1 = visibleColumns[index];
        IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(new Guid(str1));
        treeListNode.SubNodes.Add(attributeType.FieldType, str1, Convert.ToString(row[str1]));
      }
    }
    TreeListNode node = treeListNode ?? new TreeListNode(name: isRoot ? string.Empty : str1, value: string.Empty);
    if (readOnly)
      node.ForeColor = SystemColors.GrayText;
    if (!isRoot)
      node.Row = row;
    return node;
  }

  private void CreateNodes(DataRow[] rows)
  {
    if (rows.Length == 0)
      return;
    Dictionary<DataRow, TreeListNode> dict = new Dictionary<DataRow, TreeListNode>(rows.Length);
    List<TreeListNode> treeListNodeList = new List<TreeListNode>(rows.Length);
    this._nodes = new Dictionary<long, TreeListNode>(rows.Length);
    bool flag = rows[0].Table.Columns.Contains(this._usingColumnName);
    bool readOnly = !flag && !this.CheckEnabledItemFromUsingAttr(this._aTableRefID);
    foreach (DataRow row1 in rows)
    {
      DataRow row = row1;
      if (flag)
        readOnly = Convert.ToString(row[this._usingColumnName]) == "-";
      TreeListNode node = this.CreateNode(row, this._formulaColumns, this._visibleWithoutFormulaColumns, false, readOnly);
      long int64 = Convert.ToInt64(row["F_KEY"]);
      if (!this._nodes.ContainsKey(int64))
        this._nodes.Add(int64, node);
      if (this._listColumns.Count > 0)
        node.AdditionalValues = this._listColumns.ToDictionary<string, string, object>((System.Func<string, string>) (x => x), (System.Func<string, object>) (x => row[x]));
      TreeListNode sibling = this.FindSibling(dict, row, this._visibleWithoutListColumns);
      if (sibling != null)
      {
        TreeListNode parentNode = sibling.Parent;
        if (parentNode != null)
        {
          parentNode.Nodes.Add(node);
        }
        else
        {
          parentNode = this.CreateNode(row, this._formulaColumns, this._visibleWithoutFormulaColumns, true, false);
          parentNode.Nodes.Add(sibling);
          parentNode.Nodes.Add(node);
          int index = treeListNodeList.IndexOf(sibling);
          treeListNodeList.Remove(sibling);
          treeListNodeList.Insert(index, parentNode);
        }
        this.CorrectParentNode(parentNode);
      }
      else
      {
        dict.Add(row, node);
        treeListNodeList.Add(node);
      }
    }
    this._tlv.SuspendLayout();
    try
    {
      foreach (TreeListNode node in treeListNodeList)
        this._tlv.Nodes.Add(node);
    }
    finally
    {
      this._tlv.ResumeLayout();
    }
  }

  private TreeListNode FindSibling(
    Dictionary<DataRow, TreeListNode> dict,
    DataRow row,
    List<string> visibleColumns)
  {
    TreeListNode sibling = (TreeListNode) null;
    if (visibleColumns != null && visibleColumns.Count > 0)
    {
      foreach (KeyValuePair<DataRow, TreeListNode> keyValuePair in dict)
      {
        bool flag = false;
        foreach (string visibleColumn in visibleColumns)
        {
          if (!(Convert.ToString(keyValuePair.Key[visibleColumn]) == Convert.ToString(row[visibleColumn])))
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          sibling = keyValuePair.Value;
          break;
        }
      }
    }
    return sibling;
  }

  private string GetColumnCaption(AttributeTypeProperties attProp, string units)
  {
    string str1 = attProp.ShortName.Length > 0 ? $" [{attProp.ShortName}]" : string.Empty;
    string str2 = attProp.Alias.Length > 0 ? $" ({attProp.Alias})" : string.Empty;
    string str3 = attProp.Name + str1 + str2;
    if (!string.IsNullOrEmpty(units))
      str3 = $"{str3}, {units}";
    return str3.Trim();
  }

  private FavouriteData GetFavouriteData(TreeListNode node)
  {
    FavouriteData favouriteData = (FavouriteData) null;
    DataRow row = node?.Row;
    if (row != null)
      favouriteData = new FavouriteData(this._aTableRefID, Convert.ToInt64(row["F_KEY"]), this.GetParsedFormula(node));
    return favouriteData;
  }

  private string GetFullFormula(DataTable dtAttrs, string expression, int index)
  {
    int num;
    if (dtAttrs != null && !string.IsNullOrEmpty(expression))
    {
      for (int startIndex = expression.IndexOf('[', index); startIndex > -1; startIndex = expression.IndexOf('[', num))
      {
        num = expression.IndexOf(']', startIndex);
        if (num >= startIndex)
        {
          string strGuid = expression.Substring(startIndex + 1, num - startIndex - 1);
          DataRow dataRow = dtAttrs.AsEnumerable().FirstOrDefault<DataRow>((System.Func<DataRow, bool>) (x => Convert.ToString(x["F_ATTRIBUTE_GUID"]) == strGuid));
          if (dataRow != null)
          {
            string newValue = Convert.ToString(dataRow["F_FORMULA"]);
            if (!string.IsNullOrEmpty(newValue))
            {
              if (newValue.Length < strGuid.Length + 2)
                num -= strGuid.Length + 2 - newValue.Length;
              expression = expression.Replace($"[{strGuid}]", newValue);
              expression = this.GetFullFormula(dtAttrs, expression, num);
              break;
            }
          }
        }
        else
          break;
      }
    }
    return expression;
  }

  private string GetParsedFormula(TreeListNode node)
  {
    string empty = string.Empty;
    DataRow row = node?.Row;
    if (row != null && this._expressionTree != null)
    {
      VariableValuesCollection usedVariables = this._expressionTree.UsedVariables;
      if (usedVariables != null)
      {
        foreach (VariableValue variableValue in (ReadOnlyCollectionBase) usedVariables)
          variableValue.Value = row[variableValue.Name];
        empty = this._expressionTree.Evaluate(usedVariables).ToString();
      }
    }
    return empty;
  }

  private Type GetTypeOfAttributeValue(FieldTypes fieldType)
  {
    Type ofAttributeValue = AttributesTypeHelper.GetTypeOfAttributeValue(fieldType);
    if (typeof (MeasuredValue) == ofAttributeValue)
      ofAttributeValue = typeof (double);
    return ofAttributeValue;
  }

  private bool IsUniqueData(DataRow row, Dictionary<string, object> values)
  {
    bool flag = false;
    foreach (KeyValuePair<string, object> keyValuePair in values)
    {
      if (!(Convert.ToString(keyValuePair.Value) == Convert.ToString(row[keyValuePair.Key])))
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  private bool LoadAssortmentTable()
  {
    bool flag = false;
    this.ClearTlvData();
    this._dtData = (DataTable) null;
    this._dtAttrs = (DataTable) null;
    this._dtView = (DataTable) null;
    if (this._aTableRefID != 0L)
    {
      long tableIdByTableRefId = IMHHelper.GetTableIDByTableRefID(this._aTableRefID);
      AttributeTypeProperties[] columnsAttributes;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        this._ds = TableLoadHelper.GetTables(sessionKeeper.Session, tableIdByTableRefId, true);
        if (this._ds == null)
        {
          QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(tableIdByTableRefId);
          int num = (int) MessageBox.Show($"{LocalizationHolder.rm.GetString("IMH_ImbaseTable_Data_Error")} '{objectInfo.Caption}' (ID = {tableIdByTableRefId.ToString()})", LocalizationHolder.rm.GetString("IMH_Error"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this._ds = TableLoadHelper.CreateDataSet();
        }
        this._dtAttrs = this._ds.Tables["IMS_ATTR_TYPES"];
        this._dtData = this._ds.Tables["IMS_DATA"];
        this._dtView = this._dtData.Copy();
        ImbaseKeyInfo keyInfo = new ImbaseKeyInfo(-1L);
        TableLoadHelper.AssignAttributes2(sessionKeeper.Session, this._aTableRefID, tableIdByTableRefId, this._dtView, this._dtAttrs, out columnsAttributes, new List<CalculatedColumn>(), ref keyInfo);
        this._ds.AcceptChanges();
      }
      if (this._dtData != null && this._dtAttrs != null && columnsAttributes != null)
      {
        List<Variable> variables = new List<Variable>(columnsAttributes.Length);
        string expression;
        this.ParseColumns(this._dtAttrs, columnsAttributes, out expression, ref variables);
        using (Parser parser = new Parser())
        {
          parser.UseCache = false;
          parser.Variables.AddRange((ICollection) variables);
          this._expressionTree = parser.Parse(expression);
        }
        if (this._formulaColumns.Count > 0 || this._visibleWithoutFormulaColumns.Count > 0)
        {
          this.CreateColumns(this._formulaColumns, this._dtView);
          this.CreateColumns(this._visibleWithoutFormulaColumns, this._dtView);
          this.LoadRows(this._dtView);
        }
        this._tsBtnImbase.Enabled = this._tsBtnRefresh.Enabled = this._tsBtnFilter.Enabled = true;
        this._cmImbase.Enabled = this._cmRefresh.Enabled = this._cmFilter.Enabled = true;
        this._tsBtnConfig.Enabled = this._tsBtnEdit.Enabled = this._listColumns.Count > 0;
        this._cmConfig.Enabled = this._cmEdit.Enabled = this._listColumns.Count > 0;
        flag = this._tlv.SelectedNode != null;
        if (this._tlv.Nodes.Count > 0 && this._dtView.Columns.Contains(this._usingColumnName))
          this._tsBtnAssortmentApplicabilityFilter.Visible = this._dtView.AsEnumerable().FirstOrDefault<DataRow>((System.Func<DataRow, bool>) (x => Convert.ToString(x[this._usingColumnName]) == "-")) != null;
        else
          this._tsBtnAssortmentApplicabilityFilter.Visible = false;
      }
    }
    else
    {
      this._aCaption = string.Empty;
      this._tsBtnImbase.Enabled = this._tsBtnRefresh.Enabled = this._tsBtnFilter.Enabled = this._tsBtnConfig.Enabled = this._tsBtnEdit.Enabled = this._tsBtnDel.Enabled = false;
      this._cmImbase.Enabled = this._cmRefresh.Enabled = this._cmFilter.Enabled = this._cmConfig.Enabled = this._cmEdit.Enabled = this._cmDel.Enabled = false;
    }
    this.CheckAssortmentApplicabilityFilterState();
    return flag;
  }

  private void LoadRows(DataTable dt)
  {
    string filterString = this.CreateFilterString(dt.Columns.Contains(this._usingColumnName));
    StringBuilder sb = new StringBuilder();
    this._formulaColumns.ForEach((Action<string>) (x => sb.Append($"[{x}] ASC, ")));
    this._visibleWithoutFormulaColumns.ForEach((Action<string>) (x => sb.Append($"[{x}] ASC, ")));
    string str = sb.Length > 2 ? sb.ToString(0, sb.Length - 2) : string.Empty;
    if (!string.IsNullOrEmpty(str))
    {
      dt.DefaultView.Sort = str;
      dt = dt.DefaultView.ToTable();
      this._dtView = dt;
    }
    DataRow[] rows = dt.Select(filterString);
    if (rows.Length != 0)
    {
      this.CreateNodes(rows);
      if (this._aRecID != -1L && this._nodes != null && this._nodes.ContainsKey(this._aRecID))
      {
        TreeListNode treeListNode = this._nodes[this._aRecID];
        if (!treeListNode.Selected)
          treeListNode.Selected = true;
        while (treeListNode.Parent != null)
        {
          treeListNode = treeListNode.Parent;
          treeListNode.Expanded = true;
        }
      }
      else if (this._tlv.Nodes.Count > 0)
      {
        TreeListNode node = this._tlv.Nodes[0];
        if (node.Nodes.Count > 0)
        {
          node.Expanded = true;
          node = node.Nodes[0];
        }
        node.Selected = true;
      }
    }
    this.CheckAccessRights(this._aTableRefID);
  }

  private void ReloadRows()
  {
    if (this._dtView == null)
      return;
    this._tlv.SuspendLayout();
    try
    {
      this._tlv.Nodes.Clear();
      this.LoadRows(this._dtView);
    }
    finally
    {
      this._tlv.ResumeLayout();
    }
  }

  private void ParseColumns(
    DataTable dtAttrs,
    AttributeTypeProperties[] attrProps,
    out string expression,
    ref List<Variable> variables)
  {
    expression = string.Empty;
    this._dictAttrTypeProps = new Dictionary<string, AttributeTypeProperties>(attrProps.Length);
    DataRow dataRow = dtAttrs.AsEnumerable().FirstOrDefault<DataRow>((System.Func<DataRow, bool>) (x => Convert.ToString(x["F_ATTRIBUTE_GUID"]) == "cad00020-306c-11d8-b4e9-00304f19f545"));
    if (dataRow != null)
    {
      expression = Convert.ToString(dataRow["F_FORMULA"]);
      if (!string.IsNullOrEmpty(expression))
        expression = this.GetFullFormula(dtAttrs, expression, 0);
    }
    List<Tuple<int, string>> source = new List<Tuple<int, string>>();
    foreach (AttributeTypeProperties attrProp in attrProps)
    {
      string str = attrProp.AttributeGuid.ToString();
      if ((attrProp.Options & AttributeOptions.ImbaseFlag_CADMECH_T) == AttributeOptions.ImbaseFlag_CADMECH_T)
      {
        int num = expression.IndexOf($"[{str}]");
        if (num >= 0 && !this._formulaColumns.Contains(str))
        {
          this._formulaColumns.Add(str);
          source.Add(new Tuple<int, string>(num, str));
        }
        else
          this._visibleWithoutFormulaColumns.Add(str);
        if (attrProp.MultiValueMode != MultiValueModes.SingleValueFromList)
          this._visibleWithoutListColumns.Add(str);
      }
      if (attrProp.MultiValueMode == MultiValueModes.SingleValueFromList && !this.IsComputed(str))
        this._listColumns.Add(str);
      this._dictAttrTypeProps.Add(str, attrProp);
      Type ofAttributeValue = this.GetTypeOfAttributeValue(attrProp.FieldType);
      variables.Add(new Variable(str, ofAttributeValue));
    }
    if (source.Count > 0)
      this._formulaColumns = source.OrderBy<Tuple<int, string>, int>((System.Func<Tuple<int, string>, int>) (x => x.Item1)).Select<Tuple<int, string>, string>((System.Func<Tuple<int, string>, string>) (x => x.Item2)).ToList<string>();
    if (this._formulaColumns.Count != 0 || this._visibleWithoutFormulaColumns.Count != 0)
      return;
    string key = "cad00211-306c-11d8-b4e9-00304f19f545";
    if (!this._dictAttrTypeProps.ContainsKey(key))
      return;
    this._visibleWithoutFormulaColumns.Add(key);
    if (this._dictAttrTypeProps[key].MultiValueMode == MultiValueModes.SingleValueFromList)
      return;
    this._visibleWithoutListColumns.Add(key);
  }

  private bool IsComputed(string strAttributeGuid)
  {
    DataRow dataRow = this._dtAttrs.AsEnumerable().FirstOrDefault<DataRow>((System.Func<DataRow, bool>) (x => Convert.ToString(x["F_ATTRIBUTE_GUID"]) == strAttributeGuid));
    return dataRow == null || Convert.ToInt32(dataRow["F_REQUIRED"]) != 2 || Convert.ToInt32(dataRow["F_COMPUTED"]) != 0;
  }

  private void RemoveNode(TreeListNode node)
  {
    DataRow row = node?.Row;
    if (row == null)
      return;
    long key = Convert.ToInt64(row["F_KEY"]);
    if (this._nodes.ContainsKey(key))
      this._nodes.Remove(key);
    row.Delete();
    this._dtData.AsEnumerable().FirstOrDefault<DataRow>((System.Func<DataRow, bool>) (x => Convert.ToInt64(x["F_KEY"]) == key))?.Delete();
  }

  private void SaveChanges(bool askUser = false)
  {
    if (this._dtData == null || this._dtAttrs == null || !this._ds.HasChanges())
      return;
    DialogResult dialogResult = DialogResult.Yes;
    if (askUser)
    {
      string caption = LocalizationHolder.rm.GetString("IMH_SaveChanges_Caption");
      dialogResult = MessageBox.Show(LocalizationHolder.rm.GetString("IMH_SaveChanges_Msg"), caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
    }
    if (dialogResult != DialogResult.Yes)
      return;
    long tableIdByTableRefId = IMHHelper.GetTableIDByTableRefID(this._aTableRefID);
    this._dtData.AcceptChanges();
    this._dtAttrs.AcceptChanges();
    this._dtView.AcceptChanges();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      TableLoadHelper.StoreData(sessionKeeper.Session, tableIdByTableRefId, this._ds, sessionKeeper.Session.GetCustomService(typeof (ITablesIndexer)) as ITablesIndexer);
    ServiceUtils.GetService<INotificationService>((object) ApplicationServices.Container, false)?.FireEvent((object) this, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsChanged", tableIdByTableRefId));
  }

  protected void AssortmentTableRefID(long tableRefID, long recID, bool bReload)
  {
    bool flag = false;
    if (this._aTableRefID != tableRefID | bReload)
    {
      if (this._aTableRefID != 0L)
        this.SaveChanges(true);
      this._aTableRefID = tableRefID;
      flag = this.LoadAssortmentTable();
    }
    if (this._nodes != null && this._nodes.ContainsKey(recID))
    {
      TreeListNode node = this._nodes[recID];
      node.Parent.Expanded = true;
      if (!node.Selected)
        node.Selected = true;
    }
    else if (!flag)
      this._aRecID = -1L;
    this._pnlFormula.Invalidate();
  }

  protected bool CheckEnabledItemFromUsingAttr(long objID)
  {
    bool flag = true;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject objectActualCopy = sessionKeeper.Session.GetObjectActualCopy(objID, false);
      if (objectActualCopy != null)
      {
        IDBAttribute attributeById = objectActualCopy.GetAttributeByID(Intermech.Imbase.Consts.ImbaseUsingAttID);
        flag = attributeById == null || attributeById.AsString.Trim() != "-";
      }
    }
    return flag;
  }

  protected new void ClearData()
  {
    base.ClearData();
    this._filter.Clear();
    this.ClearTlvData();
  }

  protected void ClearFilter()
  {
    this._filter.Clear();
    this._tsBtnFilter.ImageIndex = 0;
  }

  protected virtual void TreeListViewEnter(EventArgs e)
  {
    if (this._tlv.SelectedNode != null)
    {
      DataRow row = this._tlv.SelectedNode.Row;
      if (row == null)
        return;
      this.OnIMHMaterialChanged(this._aTableRefID, Convert.ToInt64(row["F_KEY"]), this._tlv.SelectedNode.ForeColor != SystemColors.GrayText);
    }
    else
      this.OnIMHMaterialChanged(this._aTableRefID, this._aRecID);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (IMHViewCtrl));
    this._img = new ImageList(this.components);
    this._tlv = new TreeListView();
    this._contextMenuAssortment = new ContextMenuStrip(this.components);
    this._cmImbase = new ToolStripMenuItem();
    this._cmRefresh = new ToolStripMenuItem();
    this._tsHSeparator2 = new ToolStripSeparator();
    this._cmFilter = new ToolStripMenuItem();
    this._tsHSeparator3 = new ToolStripSeparator();
    this._cmConfig = new ToolStripMenuItem();
    this._cmEdit = new ToolStripMenuItem();
    this._cmDel = new ToolStripMenuItem();
    this._tsDimensionType = new ToolStrip();
    this._tsLabel = new ToolStripLabel();
    this._tsBtnImbase = new ToolStripButton();
    this._tsBtnRefresh = new ToolStripButton();
    this._tsSeparator4 = new ToolStripSeparator();
    this._tsBtnFilter = new ToolStripButton();
    this._tsSeparator5 = new ToolStripSeparator();
    this._tsBtnConfig = new ToolStripButton();
    this._tsBtnEdit = new ToolStripButton();
    this._tsBtnDel = new ToolStripButton();
    this._tsBtnAssortmentApplicabilityFilter = new ToolStripButton();
    this._tsSeparator31 = new ToolStripSeparator();
    this._splt.BeginInit();
    this._splt.Panel2.SuspendLayout();
    this._splt.SuspendLayout();
    this._contextMenuAssortment.SuspendLayout();
    this._tsDimensionType.SuspendLayout();
    this.SuspendLayout();
    this._splt.Panel2.Controls.Add((Control) this._tlv);
    this._splt.Panel2.Controls.Add((Control) this._tsDimensionType);
    this._img.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("_img.ImageStream");
    this._img.TransparentColor = Color.Transparent;
    this._img.Images.SetKeyName(0, "Filter.png");
    this._img.Images.SetKeyName(1, "Filter_On.png");
    this._tlv.BackColor = SystemColors.Window;
    this._tlv.ContextMenuStrip = this._contextMenuAssortment;
    componentResourceManager.ApplyResources((object) this._tlv, "_tlv");
    this._tlv.Name = "_tlv";
    this._tlv.SelectedChanged += new EventHandler(this.On_tlv_SelectedChanged);
    this._tlv.DoubleClick += new EventHandler(this._tlv_DoubleClick);
    this._tlv.Enter += new EventHandler(this.On_tlv_Enter);
    this._contextMenuAssortment.Items.AddRange(new ToolStripItem[8]
    {
      (ToolStripItem) this._cmImbase,
      (ToolStripItem) this._cmRefresh,
      (ToolStripItem) this._tsHSeparator2,
      (ToolStripItem) this._cmFilter,
      (ToolStripItem) this._tsHSeparator3,
      (ToolStripItem) this._cmConfig,
      (ToolStripItem) this._cmEdit,
      (ToolStripItem) this._cmDel
    });
    this._contextMenuAssortment.Name = "_contextMenuAssortment";
    componentResourceManager.ApplyResources((object) this._contextMenuAssortment, "_contextMenuAssortment");
    componentResourceManager.ApplyResources((object) this._cmImbase, "_cmImbase");
    this._cmImbase.Name = "_cmImbase";
    this._cmImbase.Click += new EventHandler(this.On_tsBtnImbase_Click);
    componentResourceManager.ApplyResources((object) this._cmRefresh, "_cmRefresh");
    this._cmRefresh.Name = "_cmRefresh";
    this._cmRefresh.Click += new EventHandler(this.On_tsBtnRefresh_Click);
    this._tsHSeparator2.Name = "_tsHSeparator2";
    componentResourceManager.ApplyResources((object) this._tsHSeparator2, "_tsHSeparator2");
    componentResourceManager.ApplyResources((object) this._cmFilter, "_cmFilter");
    this._cmFilter.Name = "_cmFilter";
    this._cmFilter.Click += new EventHandler(this.On_tsBtnFilter_Click);
    this._tsHSeparator3.Name = "_tsHSeparator3";
    componentResourceManager.ApplyResources((object) this._tsHSeparator3, "_tsHSeparator3");
    componentResourceManager.ApplyResources((object) this._cmConfig, "_cmConfig");
    this._cmConfig.Name = "_cmConfig";
    this._cmConfig.Click += new EventHandler(this.On_tsBtnConfig_Click);
    componentResourceManager.ApplyResources((object) this._cmEdit, "_cmEdit");
    this._cmEdit.Name = "_cmEdit";
    this._cmEdit.Click += new EventHandler(this.On_tsBtnEdit_Click);
    componentResourceManager.ApplyResources((object) this._cmDel, "_cmDel");
    this._cmDel.Name = "_cmDel";
    this._cmDel.Click += new EventHandler(this.On_tsBtnDel_Click);
    this._tsDimensionType.GripStyle = ToolStripGripStyle.Hidden;
    this._tsDimensionType.Items.AddRange(new ToolStripItem[10]
    {
      (ToolStripItem) this._tsLabel,
      (ToolStripItem) this._tsBtnImbase,
      (ToolStripItem) this._tsBtnRefresh,
      (ToolStripItem) this._tsSeparator4,
      (ToolStripItem) this._tsBtnFilter,
      (ToolStripItem) this._tsSeparator5,
      (ToolStripItem) this._tsBtnConfig,
      (ToolStripItem) this._tsBtnEdit,
      (ToolStripItem) this._tsBtnDel,
      (ToolStripItem) this._tsBtnAssortmentApplicabilityFilter
    });
    componentResourceManager.ApplyResources((object) this._tsDimensionType, "_tsDimensionType");
    this._tsDimensionType.Name = "_tsDimensionType";
    this._tsLabel.Name = "_tsLabel";
    componentResourceManager.ApplyResources((object) this._tsLabel, "_tsLabel");
    this._tsBtnImbase.DisplayStyle = ToolStripItemDisplayStyle.Image;
    componentResourceManager.ApplyResources((object) this._tsBtnImbase, "_tsBtnImbase");
    this._tsBtnImbase.Name = "_tsBtnImbase";
    this._tsBtnImbase.Click += new EventHandler(this.On_tsBtnImbase_Click);
    this._tsBtnRefresh.DisplayStyle = ToolStripItemDisplayStyle.Image;
    componentResourceManager.ApplyResources((object) this._tsBtnRefresh, "_tsBtnRefresh");
    this._tsBtnRefresh.Name = "_tsBtnRefresh";
    this._tsBtnRefresh.Click += new EventHandler(this.On_tsBtnRefresh_Click);
    this._tsSeparator4.Name = "_tsSeparator4";
    componentResourceManager.ApplyResources((object) this._tsSeparator4, "_tsSeparator4");
    this._tsBtnFilter.DisplayStyle = ToolStripItemDisplayStyle.Image;
    componentResourceManager.ApplyResources((object) this._tsBtnFilter, "_tsBtnFilter");
    this._tsBtnFilter.Name = "_tsBtnFilter";
    this._tsBtnFilter.Click += new EventHandler(this.On_tsBtnFilter_Click);
    this._tsSeparator5.Name = "_tsSeparator5";
    componentResourceManager.ApplyResources((object) this._tsSeparator5, "_tsSeparator5");
    this._tsBtnConfig.DisplayStyle = ToolStripItemDisplayStyle.Image;
    componentResourceManager.ApplyResources((object) this._tsBtnConfig, "_tsBtnConfig");
    this._tsBtnConfig.Name = "_tsBtnConfig";
    this._tsBtnConfig.Click += new EventHandler(this.On_tsBtnConfig_Click);
    componentResourceManager.ApplyResources((object) this._tsBtnEdit, "_tsBtnEdit");
    this._tsBtnEdit.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this._tsBtnEdit.Name = "_tsBtnEdit";
    this._tsBtnEdit.Click += new EventHandler(this.On_tsBtnEdit_Click);
    this._tsBtnDel.DisplayStyle = ToolStripItemDisplayStyle.Image;
    componentResourceManager.ApplyResources((object) this._tsBtnDel, "_tsBtnDel");
    this._tsBtnDel.Name = "_tsBtnDel";
    this._tsBtnDel.Click += new EventHandler(this.On_tsBtnDel_Click);
    this._tsBtnAssortmentApplicabilityFilter.DisplayStyle = ToolStripItemDisplayStyle.Image;
    componentResourceManager.ApplyResources((object) this._tsBtnAssortmentApplicabilityFilter, "_tsBtnAssortmentApplicabilityFilter");
    this._tsBtnAssortmentApplicabilityFilter.Name = "_tsBtnAssortmentApplicabilityFilter";
    this._tsBtnAssortmentApplicabilityFilter.Click += new EventHandler(this.On_tsBtnAssortmentApplicabilityFilter_Click);
    this._tsSeparator31.Name = "_tsSeparator31";
    componentResourceManager.ApplyResources((object) this._tsSeparator31, "_tsSeparator31");
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Name = nameof (IMHViewCtrl);
    this.Controls.SetChildIndex((Control) this._pnlFormula, 0);
    this.Controls.SetChildIndex((Control) this._splt, 0);
    this._splt.Panel2.ResumeLayout(false);
    this._splt.Panel2.PerformLayout();
    this._splt.EndInit();
    this._splt.ResumeLayout(false);
    this._contextMenuAssortment.ResumeLayout(false);
    this._tsDimensionType.ResumeLayout(false);
    this._tsDimensionType.PerformLayout();
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
