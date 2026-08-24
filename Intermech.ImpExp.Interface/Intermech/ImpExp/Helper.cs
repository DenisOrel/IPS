// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Helper
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;

#nullable disable
namespace Intermech.ImpExp;

public class Helper
{
  public static int ToInt32(object obj) => DBNull.Value.Equals(obj) ? 0 : Convert.ToInt32(obj);

  public static string GetExceptionMessage(Exception ex)
  {
    string message = ex.Message;
    if (ex is ObjectsFoundException)
    {
      ObjectsFoundException objectsFoundException = (ObjectsFoundException) ex;
      for (int index = 0; index < objectsFoundException.ObjectsID.Length; ++index)
      {
        if (index > 0)
          message += ", ";
        message += objectsFoundException.ObjectsID[index].ToString();
      }
    }
    return message;
  }

  public static FieldTypes GetFieldType(string dataType, int numericScale, bool isLong)
  {
    switch (dataType)
    {
      case "System.Double":
        return FieldTypes.ftDouble;
      case "System.Decimal":
        return numericScale != 0 ? FieldTypes.ftDouble : FieldTypes.ftInteger;
      case "System.Int16":
      case "System.Int32":
        return FieldTypes.ftInteger;
      case "System.String":
        return !isLong ? FieldTypes.ftString : FieldTypes.ftMemo;
      case "System.DateTime":
        return FieldTypes.ftDateTime;
      default:
        return FieldTypes.ftString;
    }
  }
}
