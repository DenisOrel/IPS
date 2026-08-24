// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.ItemFactories.ImFieldsItem
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.CommonData.SettingsItems;

#nullable disable
namespace Intermech.ImpExp.Imbase.ItemFactories;

internal sealed class ImFieldsItem : 
  SettingsAttributeTypeItem,
  IImFieldsItem,
  ISettingsAttributeTypeItem,
  ISettingsItem
{
  public int Key { get; }

  public string UniqueKey => this.Key.ToString();

  public int TableId { get; }

  public string Field { get; }

  public string Units { get; }

  public int Sort { get; }

  public long Width { get; set; }

  public int Flags { get; }

  public ImDataMode DataMode { get; }

  public int Required { get; }

  public ImDataTypeEx DataType { get; set; }

  public ImEnterMode EnterMode { get; set; }

  public string Data { get; }

  public FieldTypes AttrFieldType { get; set; } = FieldTypes.ftString;

  public AttributeCheckResult PumpPosible { get; set; } = AttributeCheckResult.cresError;

  public ImFieldsItem(
    int key,
    int tableID,
    string field,
    string longName,
    string shortName,
    string units,
    int sort,
    int flags,
    ImDataMode dataMode,
    int required,
    ImDataTypeEx dataType,
    long width,
    ImEnterMode enterMode,
    string data)
    : base(longName, shortName, string.Empty, FieldTypes.ftUnknown)
  {
    this.Options = AttributeOptions.ImbaseFlag_UsedInTables;
    this.Key = key;
    this.TableId = tableID;
    this.Field = field;
    this.Units = units;
    this.Sort = sort;
    this.Flags = flags;
    this.DataMode = dataMode;
    this.Required = required;
    this.DataType = dataType;
    this.Width = width;
    this.EnterMode = enterMode;
    this.Data = data;
    if (this.LongName == "")
      this.LongName = "Атрибут";
    if (this.ShortName.StartsWith("$"))
    {
      this.ShortName = string.Empty;
      this.LongName += " список";
      this.Options = this.Options | AttributeOptions.ImbaseFlag_IMHGen;
    }
    if (!this.Units.Equals(string.Empty))
      this.AttrFieldType = FieldTypes.ftMeasured;
    else if (this.DataMode == ImDataMode.IDM_IMAGE || this.DataMode == ImDataMode.IDM_TEXT)
    {
      this.AttrFieldType = FieldTypes.ftObjectLink;
    }
    else
    {
      switch (this.DataType)
      {
        case ImDataTypeEx.IEX_UNKNOWN:
          this.AttrFieldType = FieldTypes.ftUnknown;
          break;
        case ImDataTypeEx.IEX_STRING:
          this.AttrFieldType = FieldTypes.ftString;
          break;
        case ImDataTypeEx.IEX_INTEGER:
          this.AttrFieldType = FieldTypes.ftInteger;
          break;
        case ImDataTypeEx.IEX_FLOAT:
          this.AttrFieldType = FieldTypes.ftDouble;
          break;
        case ImDataTypeEx.IEX_BOOL:
          this.AttrFieldType = FieldTypes.ftBoolean;
          break;
        case ImDataTypeEx.IEX_REF:
          this.AttrFieldType = this.IsObjectLinkType(this.EnterMode) ? FieldTypes.ftObjectLink : FieldTypes.ftString;
          break;
        case ImDataTypeEx.IEX_USER:
          this.AttrFieldType = FieldTypes.ftInteger;
          break;
        default:
          this.AttrFieldType = FieldTypes.ftString;
          break;
      }
    }
  }

  private bool IsObjectLinkType(ImEnterMode mode)
  {
    return mode == ImEnterMode.IEM_FOLDER || mode == ImEnterMode.IEM_SEARCH_DOCUMENT || mode == ImEnterMode.IEM_SEARCH_OBJECT;
  }

  public AttributeOptions Options { get; }
}
