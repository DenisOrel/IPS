// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.ReceiptTableHelper
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Document.Client;
using Intermech.Document.Model;
using Intermech.Interfaces;
using Intermech.Interfaces.Document;
using Intermech.Kernel.Search;
using Intermech.Site.Client.Settings;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal static class ReceiptTableHelper
{
  private static string GetColumnCaption(string columnName)
  {
    if (GuidHelper.IsGuid(columnName))
    {
      Guid attrTypeGuid = new Guid(columnName);
      return MetaDataHelper.GetAttributeTypeID(attrTypeGuid) == -10000 ? columnName : MetaDataHelper.GetAttributeTypeName(attrTypeGuid);
    }
    switch (columnName)
    {
      case "CAPTION":
        return EnumDescConverter.GetEnumDescription((Enum) ObligatoryObjectAttributes.CAPTION);
      case "F_FILENAME":
        return "Файлы";
      case "F_OBJECT_TYPE":
        return EnumDescConverter.GetEnumDescription((Enum) ObligatoryObjectAttributes.F_OBJECT_TYPE);
      case "F_VERSION_ID":
        return EnumDescConverter.GetEnumDescription((Enum) ObligatoryObjectAttributes.F_VERSION_ID);
      case "F_GUID":
        return EnumDescConverter.GetEnumDescription((Enum) ObligatoryObjectAttributes.F_GUID);
      case "F_OBJ_GUID":
        return EnumDescConverter.GetEnumDescription((Enum) ObligatoryObjectAttributes.F_OBJ_GUID);
      case "F_OBJECT_ID":
        return EnumDescConverter.GetEnumDescription((Enum) ObligatoryObjectAttributes.F_OBJECT_ID);
      case "F_NAME":
        return "Атрибут";
      case "F_INLIST_ID":
        return "№ значения";
      case "F_ORIGINAL_VALUE":
        return "Значение при публикации";
      case "F_VALUE":
        return "Значение в базе";
      case "F_NOTE":
        return EnumDescConverter.GetEnumDescription((Enum) ObligatoryObjectAttributes.F_NOTE);
      default:
        return columnName;
    }
  }

  public static ImDocumentEditorForm LoadDocumentToForm(
    IUserSession session,
    DataTable content,
    string caption,
    DateTime formingData)
  {
    if (content == null)
      throw new ArgumentNullException(nameof (content));
    if (content.Columns == null || content.Columns.Count == 0)
      throw new ArgumentException("content.Columns");
    ImDocument template = (ImDocument) null;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      long receiptTemplateId = SettingsHelper.GetReceiptTemplateID(sessionKeeper.Session);
      template = DocumentEditorPlugin.LoadDocumentFromDBObject(sessionKeeper.Session.GetObjectActualCopy(receiptTemplateId, true), -1, false, true, false);
      if (template == null)
        throw new Exception($"Ошибка загрузки шаблона {receiptTemplateId} для квитанции");
    }
    TableElement node1 = template.FindNode("table") as TableElement;
    TableElement node2 = template.FindNode("table_header") as TableElement;
    TableElement node3 = template.FindNode("table_row") as TableElement;
    if (template.FindNode("header") is TextData node4)
      node4.AssignText(caption, false, false, false);
    if (template.FindNode("date") is TextData node5)
      node5.AssignText(formingData.ToString("dd.MM.yyyy HH:mm"), false, false, false);
    TextData baseHeaderCell = (TextData) null;
    TextData baseRowCell = (TextData) null;
    TextBoxElement txtEditor = (TextBoxElement) null;
    TextBoxElement baseTxtEditor = (TextBoxElement) null;
    TextBoxElement baseHeaderTxtEditor = (TextBoxElement) null;
    RectangleF bounds = node1.bounds;
    double width1 = (double) node1.Size.Width;
    float width2 = node2.ProperBounds.Width / (float) content.Columns.Count;
    for (int index = 0; index < content.Columns.Count; ++index)
      ReceiptTableHelper.AddColumns(node1, node2, node3, ref baseHeaderCell, txtEditor, ref baseHeaderTxtEditor, ref baseRowCell, ref baseTxtEditor, bounds, index, width2, ReceiptTableHelper.GetColumnCaption(content.Columns[index].ColumnName), ReceiptTableHelper.GetHorzAlignment(content.Columns[index].DataType));
    template.UpdateLayout(0, true, false);
    ImDocument imDocument = new ImDocument(template, true, true);
    imDocument.Name = caption;
    ImDocument documentOrComplect = imDocument;
    TableElement node6 = documentOrComplect.FindNode("table") as TableElement;
    for (int index1 = 0; index1 < content.Rows.Count; ++index1)
    {
      TableElement child = (TableElement) node3.CloneFromTemplate(true, true);
      for (int index2 = 0; index2 < child.Nodes.Count; ++index2)
      {
        if (content.Columns[index2] != null)
        {
          string str = Convert.ToString(content.Rows[index1][index2], (IFormatProvider) CultureInfo.CurrentCulture);
          if (child.Nodes[index2] is TextData node7)
            node7.AssignText(str, false, false, false);
        }
      }
      node6.AddChildNode((DocumentTreeNode) child, false, false);
    }
    documentOrComplect.UpdateLayout(0, true, false);
    ImDocumentEditorForm form = DocumentEditorPlugin.Instance.OpenDocument((DocumentTreeNode) documentOrComplect, false, false);
    form.Document.Modified = false;
    form.AskForSaveBeforeClose = false;
    form.Manager = DocumentEditorPlugin.DockManager;
    return form;
  }

  private static HorzAlignment GetHorzAlignment(Type columnType)
  {
    return columnType == typeof (int) || columnType == typeof (long) || columnType == typeof (double) || columnType == typeof (DateTime) ? HorzAlignment.Right : HorzAlignment.Left;
  }

  private static void AddColumns(
    TableElement docTable,
    TableElement headerRow,
    TableElement docTableRowTemplate,
    ref TextData baseHeaderCell,
    TextBoxElement txtEditor,
    ref TextBoxElement baseHeaderTxtEditor,
    ref TextData baseRowCell,
    ref TextBoxElement baseTxtEditor,
    RectangleF oldBounds,
    int index,
    float width,
    string columnCaption,
    HorzAlignment alignment)
  {
    if (docTable.GridColumnsParams != null && index < docTable.GridColumnsParams.Count)
    {
      docTable.GridColumnsParams[index].AssignSize(width, false, false);
      docTable.GridColumnsParams[index].ColRowName = columnCaption;
      baseHeaderCell = headerRow.Nodes[index] as TextData;
      baseHeaderTxtEditor = baseHeaderCell as TextBoxElement;
      baseRowCell = docTableRowTemplate.Nodes[index] as TextData;
      baseRowCell.AssignProperBounds(baseRowCell.properBounds.Location, new SizeF(width, baseRowCell.Size.Height), false, false, false);
      baseTxtEditor = baseRowCell as TextBoxElement;
    }
    else
    {
      docTable.InsertNewGridColumn(index, new RowColParams((TableData) docTable, true, -1, columnCaption, width), false, false);
      docTable.SetCellSizes(oldBounds, true, false, false, false, true);
    }
    if (headerRow.Nodes[index] is TextData node1)
    {
      node1.AssignText(columnCaption, false, false, false);
      node1.SetParagraphFormat(baseHeaderCell.ParagraphFormat.Clone(), false, false, true);
      node1.SetCharFormat(baseHeaderCell.CharFormat.Clone(), false, false);
      node1.BackColor = baseHeaderCell.BackColor;
      node1.ForeColor = baseHeaderCell.ForeColor;
      node1.AssignReadOnly(baseHeaderCell.ReadOnly);
      if (baseHeaderCell.Borders != null)
        node1.Borders = baseHeaderCell.Borders.Clone();
      else
        node1.Borders = (RectangleBorder) null;
      txtEditor = node1 as TextBoxElement;
      if (txtEditor != null && baseHeaderTxtEditor != null)
        txtEditor.AssignAutoSizeHeight(baseHeaderTxtEditor.AutoSizeHeight, false, false, true);
    }
    if (!(docTableRowTemplate.Nodes[index] is TextData node2))
      return;
    ParagraphFormat paragraphFormat = baseRowCell.ParagraphFormat.Clone();
    paragraphFormat.HorzAlignment = new HorzAlignment?(alignment);
    node2.SetParagraphFormat(paragraphFormat, false, false, true);
    node2.SetCharFormat(baseRowCell.CharFormat.Clone(), false, false);
    node2.BackColor = baseRowCell.BackColor;
    node2.ForeColor = baseRowCell.ForeColor;
    node2.AssignReadOnly(baseRowCell.ReadOnly);
    if (baseRowCell.Borders != null)
      node2.Borders = baseRowCell.Borders.Clone();
    else
      node2.Borders = (RectangleBorder) null;
    txtEditor = node2 as TextBoxElement;
    if (txtEditor == null || baseTxtEditor == null)
      return;
    txtEditor.AssignAutoSizeHeight(baseTxtEditor.AutoSizeHeight, false, false, true);
  }
}
