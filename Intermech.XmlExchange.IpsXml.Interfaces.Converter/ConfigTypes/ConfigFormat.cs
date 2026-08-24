// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Interfaces.Converter.ConfigTypes.ConfigFormat
// Assembly: Intermech.XmlExchange.IpsXml.Interfaces.Converter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9BB5546-0F4A-4E7E-A111-8011765E8C48
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Interfaces.Converter.dll

using System;
using System.ComponentModel;
using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Interfaces.Converter.ConfigTypes;

public class ConfigFormat
{
  public const string DEFALUT_OUTPUT_FILE_NAME = "XML_IM_TO_1C.xml";
  public const string DEFALUT_LOG_FILE_NAME = "XML_IM_TO_1C.log";
  public const string NN_CONVERSIONS = "convertions";
  public const string NN_CONVERSION = "convertion";
  public const string NN_CONVERT_LINK = "conv_link";
  public const string NN_VALUE = "value";
  public const string NN_DEFAULT = "default";
  public const string NN_CONVERTED_VALUE = "converted_value";
  public const string NN_NODES = "nodes";
  public const string NN_NODE = "node";
  public const string NN_ATTRIBUTES = "attrs";
  public const string NN_ATTRIBUTE = "attr";
  public const string NN_VALUE_CONFIGS = "value_configs";
  public const string NN_VALUE_CONFIG = "value_config";
  public const string NN_OUTPUT_FILE_INFO = "output_file_info";
  public const string NN_LOGGER_CONFIG = "logger_config";
  public const string AN_NAME = "name";
  public const string AN_DESCR = "descr";
  public const string AN_CONTEXT = "context";
  public const string AN_CONTEXTED = "contexted";
  public const string AN_ORIGIN = "origin";
  public const string AN_ENCODING = "encoding";
  public const string AN_VERSION = "version";
  public const string AN_VALUE = "value";
  public const string AN_EXPORT = "export";
  public const string AN_ATTR_NAME = "attr_name";
  public const string AN_SURR_SYMB = "sur_symb";
  public const string AN_GROUP_ID = "group_id";
  public const string AN_ORDER = "order";
  public const string AN_ORDER_IN_GROUP = "order_in_group";
  public const string AN_DELIMITER = "delim";
  public const string AN_TYPE = "type";
  public const string AN_VTYPE = "vtype";
  public const string AN_COMPARISON = "cmp";
  public const string AN_GROUP_COND = "group_cond";
  public const string AN_ATTR_INTERNAL_FIELD_NAME = "attr_internal_field_name";
  public const string AN_BASE64_ENCODE = "base64encode";
  public const string AN_INFOS = "infos";
  public const string AN_WARNINGS = "warnings";
  public const string AN_ERRORS = "errors";
  public const string AV_SIMPLE = "simple";
  public const string AV_IM_OBJECT_ATTR = "im_object";
  public const string AV_IM_RELATION_ATTR = "im_relation";
  public const string AV_LOCAL = "local";
  public const string AV_FIXED_FUNC = "fixed_func";
  public const string AV_SUBSTITUTE = "substitute";
  public const string AV_STRING = "string";
  public const string AV_INTEGER = "integer";
  public const string AV_FLOAT = "float";
  public const string AV_DATE = "date";
  public const string AV_DATETIME = "datetime";
  public const string AV_TIME = "time";
  public const string AV_FCN_CUR_DATE = "cur_date";
  public const string AV_FCN_ASSEMBLY_VERSION_INFO = "asm_ver_info";
  public const string AV_TRUE = "true";
  public const string AV_FALSE = "false";
  public const string AV_OR = "or";
  public const string AV_AND = "and";
  public const string AV_UNKNOWN = "unknown";

  public static string GetValueAttrValue(XElement node)
  {
    XAttribute xattribute = node.Attribute((XName) "value");
    if (xattribute != null && !xattribute.Value.Equals(string.Empty))
      return xattribute.Value;
    return !node.HasElements && !node.Value.Equals(string.Empty) ? node.Value : string.Empty;
  }

  public static BaseConfigNode GetConfigClassByNode(XElement node)
  {
    BaseConfigNode configClassByNode;
    switch (node.Name.ToString())
    {
      case "attr":
        configClassByNode = (BaseConfigNode) Activator.CreateInstance<AttrConfig>();
        break;
      case "attrs":
        configClassByNode = (BaseConfigNode) Activator.CreateInstance<AttrConfigs>();
        break;
      case "conv_link":
        configClassByNode = (BaseConfigNode) Activator.CreateInstance<ConvertValueLink>();
        break;
      case "convertion":
        configClassByNode = (BaseConfigNode) Activator.CreateInstance<ValueConverter>();
        break;
      case "convertions":
        configClassByNode = (BaseConfigNode) Activator.CreateInstance<ValueConverters>();
        break;
      case nameof (node):
        configClassByNode = (BaseConfigNode) Activator.CreateInstance<NodeConfig>();
        break;
      case "nodes":
        configClassByNode = (BaseConfigNode) Activator.CreateInstance<NodeConfigs>();
        break;
      case "value_config":
        XAttribute xattribute = node.Attribute((XName) "type");
        switch (xattribute != null ? (int) ConfigFormat.ParseAttrValueType(xattribute.Value) : 1)
        {
          case 2:
            configClassByNode = (BaseConfigNode) Activator.CreateInstance<IMObjectAttrValueConfig>();
            break;
          case 3:
            configClassByNode = (BaseConfigNode) Activator.CreateInstance<IMRelationAttrValueConfig>();
            break;
          case 4:
            configClassByNode = (BaseConfigNode) Activator.CreateInstance<LocalValueConfig>();
            break;
          case 5:
            configClassByNode = (BaseConfigNode) Activator.CreateInstance<FixedFuncValueConfig>();
            break;
          case 6:
            configClassByNode = (BaseConfigNode) Activator.CreateInstance<SubstituteValueConfig>();
            break;
          default:
            configClassByNode = (BaseConfigNode) Activator.CreateInstance<SimpleValueConfig>();
            break;
        }
        break;
      case "value_configs":
        configClassByNode = (BaseConfigNode) Activator.CreateInstance<ValueConfigs>();
        break;
      default:
        configClassByNode = (BaseConfigNode) null;
        break;
    }
    return configClassByNode;
  }

  public static ConfigFormat.AttrValueType ParseAttrValueType(string value)
  {
    return (ConfigFormat.AttrValueType) EnumDescConverter.GetEnumValue(typeof (ConfigFormat.AttrValueType), value);
  }

  public static ConfigFormat.AttrValueDataType ParseAttrValueDataType(string value)
  {
    return (ConfigFormat.AttrValueDataType) EnumDescConverter.GetEnumValue(typeof (ConfigFormat.AttrValueDataType), value);
  }

  public static ConfigFormat.GroupCondType ParseGroupCondType(string value)
  {
    return (ConfigFormat.GroupCondType) EnumDescConverter.GetEnumValue(typeof (ConfigFormat.GroupCondType), value);
  }

  public static ConfigFormat.FixedFuncType ParseFixedFuncType(string value)
  {
    return (ConfigFormat.FixedFuncType) EnumDescConverter.GetEnumValue(typeof (ConfigFormat.FixedFuncType), value);
  }

  public enum AttrValueType
  {
    [Description("unknown")] avtUnknown,
    [Description("simple")] avtSimple,
    [Description("im_object")] avtIMObjectAttr,
    [Description("im_relation")] avtIMRelationAttr,
    [Description("local")] avtLocal,
    [Description("fixed_func")] avtFixedFunc,
    [Description("substitute")] avtSubstitute,
  }

  public enum AttrValueDataType
  {
    [Description("unknown")] avdtUnknown,
    [Description("string")] avdtString,
    [Description("integer")] avdtInteger,
    [Description("float")] avdtFloat,
    [Description("date")] avdtDate,
    [Description("datetime")] avdtDateTime,
    [Description("time")] avdtTime,
  }

  public enum GroupCondType
  {
    [Description("unknown")] gctUnknown,
    [Description("and")] gctAND,
    [Description("or")] gctOR,
  }

  public enum FixedFuncType
  {
    [Description("unknown")] fftUnknown,
    [Description("cur_date")] fftCurDate,
    [Description("asm_ver_info")] fftAssemblyInfo,
  }
}
