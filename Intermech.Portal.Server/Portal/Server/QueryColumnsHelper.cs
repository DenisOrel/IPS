// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.QueryColumnsHelper
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Kernel.Search;
using System;
using System.Reflection;

#nullable disable
namespace Intermech.Portal.Server;

internal class QueryColumnsHelper
{
  private static ColumnDescriptor[] _relationsColumns;
  private static ColumnDescriptor[] _versionsColumns;

  public static ColumnDescriptor[] RelationsColumns
  {
    get
    {
      if (QueryColumnsHelper._relationsColumns == null)
      {
        Array values = Enum.GetValues(typeof (CompositionColumnNames));
        QueryColumnsHelper._relationsColumns = new ColumnDescriptor[values.Length];
        for (int index = 0; index < values.Length; ++index)
        {
          CompositionColumnNames compositionColumnNames = (CompositionColumnNames) values.GetValue(index);
          FieldInfo field = compositionColumnNames.GetType().GetField(compositionColumnNames.ToString());
          object[] customAttributes1 = field.GetCustomAttributes(typeof (AttributeIDAttribute), false);
          object[] customAttributes2 = field.GetCustomAttributes(typeof (SourceType), false);
          object obj = ((AttributeIDAttribute) customAttributes1[0]).AttributeID;
          if (obj is string)
            obj = (object) new Guid((string) obj);
          QueryColumnsHelper._relationsColumns[index] = new ColumnDescriptor(obj, ((SourceType) customAttributes2[0]).AttributeSourceType, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0);
        }
      }
      return QueryColumnsHelper._relationsColumns;
    }
  }

  public static ColumnDescriptor[] VersionsColumns
  {
    get
    {
      if (QueryColumnsHelper._versionsColumns == null)
      {
        Array values = Enum.GetValues(typeof (VersionsColumnNames));
        QueryColumnsHelper._versionsColumns = new ColumnDescriptor[values.Length];
        for (int index = 0; index < values.Length; ++index)
        {
          VersionsColumnNames versionsColumnNames = (VersionsColumnNames) values.GetValue(index);
          object obj = ((AttributeIDAttribute) versionsColumnNames.GetType().GetField(versionsColumnNames.ToString()).GetCustomAttributes(typeof (AttributeIDAttribute), false)[0]).AttributeID;
          if (obj is string)
            obj = (object) new Guid((string) obj);
          QueryColumnsHelper._versionsColumns[index] = new ColumnDescriptor(obj, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0);
        }
      }
      return QueryColumnsHelper._versionsColumns;
    }
  }
}
