// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.Controls.InvalidAttributesReport
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Intermech.ImpExp.Imbase.ItemFactories;
using Intermech.ImpExp.Interface.CommonData.ItemsToCreate;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

#nullable disable
namespace Intermech.ImpExp.Imbase.Controls;

internal sealed class InvalidAttributesReport
{
  public void Create(
    string fileName,
    IAttributeTypeToCreateList attrService,
    List<ImbaseAttribute> attributes)
  {
    using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook))
    {
      WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
      workbookPart.Workbook = new Workbook();
      WorksheetPart part = workbookPart.AddNewPart<WorksheetPart>();
      part.Worksheet = new Worksheet(new OpenXmlElement[1]
      {
        (OpenXmlElement) new SheetData()
      });
      WorkbookStylesPart workbookStylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
      workbookStylesPart.Stylesheet = this.GenerateStyleSheet();
      workbookStylesPart.Stylesheet.Save();
      Columns newChild = part.Worksheet.GetFirstChild<Columns>();
      bool flag = false;
      if (newChild == null)
      {
        newChild = new Columns();
        flag = true;
      }
      newChild.Append((OpenXmlElement) new Column()
      {
        Min = (UInt32Value) 1U,
        Max = (UInt32Value) 20U,
        Width = (DoubleValue) 20.0,
        CustomWidth = (BooleanValue) true
      });
      newChild.Append((OpenXmlElement) new Column()
      {
        Min = (UInt32Value) 2U,
        Max = (UInt32Value) 20U,
        Width = (DoubleValue) 30.0,
        CustomWidth = (BooleanValue) true
      });
      newChild.Append((OpenXmlElement) new Column()
      {
        Min = (UInt32Value) 3U,
        Max = (UInt32Value) 20U,
        Width = (DoubleValue) 10.0,
        CustomWidth = (BooleanValue) true
      });
      newChild.Append((OpenXmlElement) new Column()
      {
        Min = (UInt32Value) 4U,
        Max = (UInt32Value) 20U,
        Width = (DoubleValue) 65.0,
        CustomWidth = (BooleanValue) true
      });
      newChild.Append((OpenXmlElement) new Column()
      {
        Min = (UInt32Value) 5U,
        Max = (UInt32Value) 20U,
        Width = (DoubleValue) 15.0,
        CustomWidth = (BooleanValue) true
      });
      newChild.Append((OpenXmlElement) new Column()
      {
        Min = (UInt32Value) 6U,
        Max = (UInt32Value) 20U,
        Width = (DoubleValue) 10.0,
        CustomWidth = (BooleanValue) true
      });
      newChild.Append((OpenXmlElement) new Column()
      {
        Min = (UInt32Value) 7U,
        Max = (UInt32Value) 20U,
        Width = (DoubleValue) 60.0,
        CustomWidth = (BooleanValue) true
      });
      newChild.Append((OpenXmlElement) new Column()
      {
        Min = (UInt32Value) 8U,
        Max = (UInt32Value) 20U,
        Width = (DoubleValue) 15.0,
        CustomWidth = (BooleanValue) true
      });
      newChild.Append((OpenXmlElement) new Column()
      {
        Min = (UInt32Value) 9U,
        Max = (UInt32Value) 20U,
        Width = (DoubleValue) 55.0,
        CustomWidth = (BooleanValue) true
      });
      newChild.Append((OpenXmlElement) new Column()
      {
        Min = (UInt32Value) 10U,
        Max = (UInt32Value) 20U,
        Width = (DoubleValue) 60.0,
        CustomWidth = (BooleanValue) true
      });
      newChild.Append((OpenXmlElement) new Column()
      {
        Min = (UInt32Value) 11U,
        Max = (UInt32Value) 20U,
        Width = (DoubleValue) 20.0,
        CustomWidth = (BooleanValue) true
      });
      if (flag)
        part.Worksheet.InsertAt<Columns>(newChild, 0);
      workbookPart.Workbook.AppendChild<Sheets>(new Sheets()).Append((OpenXmlElement) new Sheet()
      {
        Id = (StringValue) workbookPart.GetIdOfPart((OpenXmlPart) part),
        SheetId = (UInt32Value) 1U,
        Name = (StringValue) "Атрибуты с ошибками привязки"
      });
      SheetData firstChild = part.Worksheet.GetFirstChild<SheetData>();
      Row row1 = new Row()
      {
        RowIndex = (UInt32Value) 1U,
        Collapsed = (BooleanValue) false
      };
      firstChild.Append((OpenXmlElement) row1);
      this.InsertCell(row1, 1, "Используется в таблицах", CellValues.String, 1U);
      this.InsertCell(row1, 2, "Длинное имя", CellValues.String, 1U);
      this.InsertCell(row1, 3, "Короткое имя", CellValues.String, 1U);
      this.InsertCell(row1, 4, "Проверка", CellValues.String, 1U);
      this.InsertCell(row1, 5, "Тип", CellValues.String, 1U);
      this.InsertCell(row1, 6, "Ширина", CellValues.String, 1U);
      this.InsertCell(row1, 7, "Список", CellValues.String, 1U);
      this.InsertCell(row1, 8, "Единица измерения", CellValues.String, 1U);
      this.InsertCell(row1, 9, "Тип IPS", CellValues.String, 1U);
      this.InsertCell(row1, 10, "Список IPS", CellValues.String, 1U);
      this.InsertCell(row1, 11, "Существует в базе назначения", CellValues.String, 1U);
      for (int index = 0; index < attributes.Count; ++index)
      {
        ImbaseAttribute attribute = attributes[index];
        Row row2 = new Row()
        {
          RowIndex = new UInt32Value((uint) (index + 2)),
          Collapsed = (BooleanValue) false
        };
        firstChild.Append((OpenXmlElement) row2);
        IAttributeTypeToCreate byGuid = attrService.GetByGuid(attribute.BindingAttribute.AttributeType);
        uint styleIndex1 = 2;
        uint styleIndex2 = 3;
        this.InsertCell(row2, 1, this.ReplaceHexadecimalSymbols(string.Join(", ", (IEnumerable<string>) attribute.PresentInTables)), CellValues.String, styleIndex2);
        this.InsertCell(row2, 2, this.ReplaceHexadecimalSymbols(attribute.Name), CellValues.String, styleIndex2);
        this.InsertCell(row2, 3, this.ReplaceHexadecimalSymbols(attribute.ShortName), CellValues.String, styleIndex2);
        this.InsertCell(row2, 4, this.ReplaceHexadecimalSymbols(EnumDescConverter.GetEnumDescription((Enum) attribute.CheckResult)), CellValues.String, styleIndex2);
        this.InsertCell(row2, 5, this.ReplaceHexadecimalSymbols(EnumDescConverter.GetEnumDescription((Enum) attribute.AttributeType)), CellValues.String, styleIndex2);
        this.InsertCell(row2, 6, this.ReplaceHexadecimalSymbols(attribute.AttributeType == ImDataTypeEx.IEX_STRING ? attribute.Size.ToString() : string.Empty), CellValues.String, styleIndex1);
        this.InsertCell(row2, 7, this.ReplaceHexadecimalSymbols(EnumDescConverter.GetEnumDescription((Enum) attribute.MultiValueMode)), CellValues.String, styleIndex2);
        this.InsertCell(row2, 8, this.ReplaceHexadecimalSymbols(attribute.Unit), CellValues.String, styleIndex2);
        this.InsertCell(row2, 9, this.ReplaceHexadecimalSymbols(EnumDescConverter.GetEnumDescription((Enum) byGuid.FieldType)), CellValues.String, styleIndex2);
        this.InsertCell(row2, 10, this.ReplaceHexadecimalSymbols(EnumDescConverter.GetEnumDescription((Enum) byGuid.MultiValueMode)), CellValues.String, styleIndex2);
        this.InsertCell(row2, 11, this.ReplaceHexadecimalSymbols(attribute.ExistInBase ? "Да" : "Нет"), CellValues.String, styleIndex1);
      }
      workbookPart.Workbook.Save();
      spreadsheetDocument.Close();
    }
  }

  private void InsertCell(Row row, int cell_num, string val, CellValues type, uint styleIndex)
  {
    Cell referenceChild = (Cell) null;
    Cell cell = new Cell();
    cell.CellReference = (StringValue) $"{cell_num.ToString()}:{row.RowIndex.ToString()}";
    cell.StyleIndex = (UInt32Value) styleIndex;
    Cell newChild = cell;
    row.InsertBefore<Cell>(newChild, (OpenXmlElement) referenceChild);
    newChild.CellValue = new CellValue(val);
    newChild.DataType = new EnumValue<CellValues>(type);
  }

  private string ReplaceHexadecimalSymbols(string txt)
  {
    string pattern = "[\0-\b\v\f\u000E-\u001F&]";
    return Regex.Replace(txt, pattern, "", RegexOptions.Compiled);
  }

  private Stylesheet GenerateStyleSheet()
  {
    OpenXmlElement[] openXmlElementArray1 = new OpenXmlElement[4];
    OpenXmlElement[] openXmlElementArray2 = new OpenXmlElement[2];
    OpenXmlElement[] openXmlElementArray3 = new OpenXmlElement[3]
    {
      (OpenXmlElement) new FontSize()
      {
        Val = (DoubleValue) 11.0
      },
      null,
      null
    };
    Color color1 = new Color();
    HexBinaryValue hexBinaryValue1 = new HexBinaryValue();
    hexBinaryValue1.Value = "000000";
    color1.Rgb = hexBinaryValue1;
    openXmlElementArray3[1] = (OpenXmlElement) color1;
    openXmlElementArray3[2] = (OpenXmlElement) new FontName()
    {
      Val = (StringValue) "Calibri"
    };
    openXmlElementArray2[0] = (OpenXmlElement) new Font(openXmlElementArray3);
    OpenXmlElement[] openXmlElementArray4 = new OpenXmlElement[4]
    {
      (OpenXmlElement) new Bold(),
      (OpenXmlElement) new FontSize()
      {
        Val = (DoubleValue) 11.0
      },
      null,
      null
    };
    Color color2 = new Color();
    HexBinaryValue hexBinaryValue2 = new HexBinaryValue();
    hexBinaryValue2.Value = "000000";
    color2.Rgb = hexBinaryValue2;
    openXmlElementArray4[2] = (OpenXmlElement) color2;
    openXmlElementArray4[3] = (OpenXmlElement) new FontName()
    {
      Val = (StringValue) "Calibri"
    };
    openXmlElementArray2[1] = (OpenXmlElement) new Font(openXmlElementArray4);
    openXmlElementArray1[0] = (OpenXmlElement) new Fonts(openXmlElementArray2);
    OpenXmlElement[] openXmlElementArray5 = new OpenXmlElement[3]
    {
      (OpenXmlElement) new Fill(new OpenXmlElement[1]
      {
        (OpenXmlElement) new PatternFill()
        {
          PatternType = (EnumValue<PatternValues>) PatternValues.None
        }
      }),
      null,
      null
    };
    OpenXmlElement[] openXmlElementArray6 = new OpenXmlElement[1];
    OpenXmlElement[] openXmlElementArray7 = new OpenXmlElement[1];
    ForegroundColor foregroundColor1 = new ForegroundColor();
    HexBinaryValue hexBinaryValue3 = new HexBinaryValue();
    hexBinaryValue3.Value = "FFAAAAAA";
    foregroundColor1.Rgb = hexBinaryValue3;
    openXmlElementArray7[0] = (OpenXmlElement) foregroundColor1;
    openXmlElementArray6[0] = (OpenXmlElement) new PatternFill(openXmlElementArray7)
    {
      PatternType = (EnumValue<PatternValues>) PatternValues.Solid
    };
    openXmlElementArray5[1] = (OpenXmlElement) new Fill(openXmlElementArray6);
    OpenXmlElement[] openXmlElementArray8 = new OpenXmlElement[1];
    OpenXmlElement[] openXmlElementArray9 = new OpenXmlElement[1];
    ForegroundColor foregroundColor2 = new ForegroundColor();
    HexBinaryValue hexBinaryValue4 = new HexBinaryValue();
    hexBinaryValue4.Value = "FFFFAAAA";
    foregroundColor2.Rgb = hexBinaryValue4;
    openXmlElementArray9[0] = (OpenXmlElement) foregroundColor2;
    openXmlElementArray8[0] = (OpenXmlElement) new PatternFill(openXmlElementArray9)
    {
      PatternType = (EnumValue<PatternValues>) PatternValues.Solid
    };
    openXmlElementArray5[2] = (OpenXmlElement) new Fill(openXmlElementArray8);
    openXmlElementArray1[1] = (OpenXmlElement) new Fills(openXmlElementArray5);
    OpenXmlElement[] openXmlElementArray10 = new OpenXmlElement[2];
    OpenXmlElement[] openXmlElementArray11 = new OpenXmlElement[5];
    LeftBorder leftBorder1 = new LeftBorder();
    leftBorder1.Style = (EnumValue<BorderStyleValues>) BorderStyleValues.Thin;
    openXmlElementArray11[0] = (OpenXmlElement) leftBorder1;
    RightBorder rightBorder1 = new RightBorder();
    rightBorder1.Style = (EnumValue<BorderStyleValues>) BorderStyleValues.Thin;
    openXmlElementArray11[1] = (OpenXmlElement) rightBorder1;
    TopBorder topBorder1 = new TopBorder();
    topBorder1.Style = (EnumValue<BorderStyleValues>) BorderStyleValues.Thin;
    openXmlElementArray11[2] = (OpenXmlElement) topBorder1;
    BottomBorder bottomBorder1 = new BottomBorder();
    bottomBorder1.Style = (EnumValue<BorderStyleValues>) BorderStyleValues.Thin;
    openXmlElementArray11[3] = (OpenXmlElement) bottomBorder1;
    openXmlElementArray11[4] = (OpenXmlElement) new DiagonalBorder();
    openXmlElementArray10[0] = (OpenXmlElement) new Border(openXmlElementArray11);
    OpenXmlElement[] openXmlElementArray12 = new OpenXmlElement[5];
    OpenXmlElement[] openXmlElementArray13 = new OpenXmlElement[1];
    Color color3 = new Color();
    color3.Auto = (BooleanValue) true;
    openXmlElementArray13[0] = (OpenXmlElement) color3;
    LeftBorder leftBorder2 = new LeftBorder(openXmlElementArray13);
    leftBorder2.Style = (EnumValue<BorderStyleValues>) BorderStyleValues.Thin;
    openXmlElementArray12[0] = (OpenXmlElement) leftBorder2;
    OpenXmlElement[] openXmlElementArray14 = new OpenXmlElement[1];
    Color color4 = new Color();
    color4.Indexed = (UInt32Value) 64U /*0x40*/;
    openXmlElementArray14[0] = (OpenXmlElement) color4;
    RightBorder rightBorder2 = new RightBorder(openXmlElementArray14);
    rightBorder2.Style = (EnumValue<BorderStyleValues>) BorderStyleValues.Thin;
    openXmlElementArray12[1] = (OpenXmlElement) rightBorder2;
    OpenXmlElement[] openXmlElementArray15 = new OpenXmlElement[1];
    Color color5 = new Color();
    color5.Auto = (BooleanValue) true;
    openXmlElementArray15[0] = (OpenXmlElement) color5;
    TopBorder topBorder2 = new TopBorder(openXmlElementArray15);
    topBorder2.Style = (EnumValue<BorderStyleValues>) BorderStyleValues.Thin;
    openXmlElementArray12[2] = (OpenXmlElement) topBorder2;
    OpenXmlElement[] openXmlElementArray16 = new OpenXmlElement[1];
    Color color6 = new Color();
    color6.Indexed = (UInt32Value) 64U /*0x40*/;
    openXmlElementArray16[0] = (OpenXmlElement) color6;
    BottomBorder bottomBorder2 = new BottomBorder(openXmlElementArray16);
    bottomBorder2.Style = (EnumValue<BorderStyleValues>) BorderStyleValues.Thin;
    openXmlElementArray12[3] = (OpenXmlElement) bottomBorder2;
    openXmlElementArray12[4] = (OpenXmlElement) new DiagonalBorder();
    openXmlElementArray10[1] = (OpenXmlElement) new Border(openXmlElementArray12);
    openXmlElementArray1[2] = (OpenXmlElement) new Borders(openXmlElementArray10);
    openXmlElementArray1[3] = (OpenXmlElement) new CellFormats(new OpenXmlElement[4]
    {
      (OpenXmlElement) new CellFormat(new OpenXmlElement[1]
      {
        (OpenXmlElement) new Alignment()
        {
          Horizontal = (EnumValue<HorizontalAlignmentValues>) HorizontalAlignmentValues.Center,
          Vertical = (EnumValue<VerticalAlignmentValues>) VerticalAlignmentValues.Center,
          WrapText = (BooleanValue) true
        }
      })
      {
        FontId = (UInt32Value) 0U,
        FillId = (UInt32Value) 0U,
        BorderId = (UInt32Value) 1U
      },
      (OpenXmlElement) new CellFormat(new OpenXmlElement[1]
      {
        (OpenXmlElement) new Alignment()
        {
          Horizontal = (EnumValue<HorizontalAlignmentValues>) HorizontalAlignmentValues.Center,
          Vertical = (EnumValue<VerticalAlignmentValues>) VerticalAlignmentValues.Center,
          WrapText = (BooleanValue) true
        }
      })
      {
        FontId = (UInt32Value) 1U,
        FillId = (UInt32Value) 2U,
        BorderId = (UInt32Value) 1U
      },
      (OpenXmlElement) new CellFormat(new OpenXmlElement[1]
      {
        (OpenXmlElement) new Alignment()
        {
          Horizontal = (EnumValue<HorizontalAlignmentValues>) HorizontalAlignmentValues.Center,
          Vertical = (EnumValue<VerticalAlignmentValues>) VerticalAlignmentValues.Center,
          WrapText = (BooleanValue) true
        }
      })
      {
        FontId = (UInt32Value) 0U,
        FillId = (UInt32Value) 0U,
        BorderId = (UInt32Value) 1U
      },
      (OpenXmlElement) new CellFormat(new OpenXmlElement[1]
      {
        (OpenXmlElement) new Alignment()
        {
          Horizontal = (EnumValue<HorizontalAlignmentValues>) HorizontalAlignmentValues.Left,
          Vertical = (EnumValue<VerticalAlignmentValues>) VerticalAlignmentValues.Center,
          WrapText = (BooleanValue) true
        }
      })
      {
        FontId = (UInt32Value) 0U,
        FillId = (UInt32Value) 0U,
        BorderId = (UInt32Value) 1U
      }
    });
    return new Stylesheet(openXmlElementArray1);
  }
}
