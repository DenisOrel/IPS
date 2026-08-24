// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Common.DataConvertor
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Data.SqlTypes;
using System.Globalization;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.ImpExp.TechCard.Common;

public static class DataConvertor
{
  private static readonly NumberFormatInfo NumFormatInfo;
  public static readonly DateTime DelphiEpochDate = new DateTime(1899, 12, 30);
  private static readonly Lazy<RtfDataConvertor> RtfConvertorLazy = new Lazy<RtfDataConvertor>();

  static DataConvertor()
  {
    DataConvertor.NumFormatInfo = new NumberFormatInfo()
    {
      NumberDecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator
    };
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsEmptyValue(object objValue) => objValue == null || objValue == DBNull.Value;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool ConvertObjToBool(object objValue, out bool blValue)
  {
    blValue = false;
    if (DataConvertor.IsEmptyValue(objValue))
      return false;
    Type type = objValue.GetType();
    if (type == typeof (bool))
    {
      blValue = (bool) objValue;
      return true;
    }
    if (!(type == typeof (int)) && !(type == typeof (long)))
      return DataConvertor.ConvertStrToBool(objValue.ToString(), out blValue);
    blValue = (int) objValue > 0;
    return true;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool ConvertObjToDouble(object objValue, out double dblValue)
  {
    dblValue = 0.0;
    if (DataConvertor.IsEmptyValue(objValue))
      return false;
    Type type = objValue.GetType();
    if (type == typeof (float))
    {
      dblValue = (double) (float) objValue;
      return true;
    }
    if (!(type == typeof (double)))
      return DataConvertor.ConvertStrToDouble(objValue.ToString(), out dblValue);
    dblValue = (double) objValue;
    return true;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool ConvertObjToInt(object objValue, out int intValue)
  {
    intValue = 0;
    if (DataConvertor.IsEmptyValue(objValue))
      return false;
    Type type = objValue.GetType();
    if (type == typeof (int))
    {
      intValue = (int) objValue;
      return true;
    }
    if (type == typeof (long))
    {
      intValue = Convert.ToInt32(objValue);
      return true;
    }
    if (!(type == typeof (double)) && !(type == typeof (float)))
      return DataConvertor.ConvertStrToInt(objValue.ToString(), out intValue);
    intValue = (int) Math.Truncate((double) objValue);
    return true;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool ConvertObjToInt(object objValue, out long intValue)
  {
    intValue = 0L;
    if (DataConvertor.IsEmptyValue(objValue))
      return false;
    Type type = objValue.GetType();
    if (type == typeof (int))
    {
      intValue = (long) (int) objValue;
      return true;
    }
    if (type == typeof (long))
    {
      intValue = (long) objValue;
      return true;
    }
    if (!(type == typeof (double)) && !(type == typeof (float)))
      return DataConvertor.ConvertStrToInt(objValue.ToString(), out intValue);
    try
    {
      intValue = Convert.ToInt64(objValue);
    }
    catch (FormatException ex)
    {
      intValue = 0L;
    }
    return true;
  }

  private static bool IsCorrectDbDateTimeValue(DateTime dateValue)
  {
    return !(dateValue < SqlDateTime.MinValue.Value) && !(dateValue > SqlDateTime.MaxValue.Value);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool ConvertObjToDate(object objValue, out DateTime dateValue)
  {
    dateValue = DateTime.MinValue;
    if (DataConvertor.IsEmptyValue(objValue))
      return false;
    if (objValue.GetType() == typeof (DateTime))
    {
      dateValue = ((DateTime) objValue).ToUniversalTime();
      return DataConvertor.IsCorrectDbDateTimeValue(dateValue);
    }
    string s = objValue.ToString();
    if (DateTime.TryParse(s, out dateValue))
    {
      dateValue = dateValue.ToUniversalTime();
      return DataConvertor.IsCorrectDbDateTimeValue(dateValue);
    }
    int result;
    if (!int.TryParse(s, out result) || result == 0)
      return false;
    dateValue = DataConvertor.DelphiEpochDate.AddDays((double) result).ToUniversalTime();
    return DataConvertor.IsCorrectDbDateTimeValue(dateValue);
  }

  public static bool ConvertStrToBool(string strValue, out bool blValue)
  {
    strValue = strValue.Trim();
    if (strValue == string.Empty)
      blValue = false;
    else if (!bool.TryParse(strValue, out blValue))
    {
      strValue = strValue.ToUpper();
      switch (strValue)
      {
        case "1":
        case "T":
        case "ДА":
          blValue = true;
          return true;
        case "0":
        case "F":
        case "НЕТ":
          blValue = false;
          return true;
        default:
          return false;
      }
    }
    return true;
  }

  public static bool ConvertStrToDouble(string strValue, out double dblValue)
  {
    strValue = strValue.Trim();
    if (strValue == string.Empty)
      dblValue = 0.0;
    else if (!double.TryParse(strValue, NumberStyles.Any, (IFormatProvider) DataConvertor.NumFormatInfo, out dblValue))
    {
      string decimalSeparator = DataConvertor.NumFormatInfo.NumberDecimalSeparator;
      try
      {
        DataConvertor.NumFormatInfo.NumberDecimalSeparator = DataConvertor.NumFormatInfo.NumberDecimalSeparator.Equals(".") ? "," : ".";
        if (!double.TryParse(strValue, NumberStyles.Any, (IFormatProvider) DataConvertor.NumFormatInfo, out dblValue))
          return false;
      }
      finally
      {
        DataConvertor.NumFormatInfo.NumberDecimalSeparator = decimalSeparator;
      }
    }
    return true;
  }

  public static bool ConvertStrToInt(string strValue, out int intValue)
  {
    strValue = strValue.Trim();
    if (strValue == string.Empty)
      intValue = 0;
    else if (!int.TryParse(strValue, NumberStyles.Any, (IFormatProvider) DataConvertor.NumFormatInfo, out intValue))
    {
      string decimalSeparator = DataConvertor.NumFormatInfo.NumberDecimalSeparator;
      try
      {
        DataConvertor.NumFormatInfo.NumberDecimalSeparator = DataConvertor.NumFormatInfo.NumberDecimalSeparator.Equals(".") ? "," : ".";
        if (!int.TryParse(strValue, NumberStyles.Any, (IFormatProvider) DataConvertor.NumFormatInfo, out intValue))
          return false;
      }
      finally
      {
        DataConvertor.NumFormatInfo.NumberDecimalSeparator = decimalSeparator;
      }
    }
    return true;
  }

  public static bool ConvertStrToInt(string strValue, out long intValue)
  {
    strValue = strValue.Trim();
    if (strValue == string.Empty)
      intValue = 0L;
    else if (!long.TryParse(strValue, NumberStyles.Any, (IFormatProvider) DataConvertor.NumFormatInfo, out intValue))
    {
      string decimalSeparator = DataConvertor.NumFormatInfo.NumberDecimalSeparator;
      try
      {
        DataConvertor.NumFormatInfo.NumberDecimalSeparator = DataConvertor.NumFormatInfo.NumberDecimalSeparator.Equals(".") ? "," : ".";
        if (!long.TryParse(strValue, NumberStyles.Any, (IFormatProvider) DataConvertor.NumFormatInfo, out intValue))
          return false;
      }
      finally
      {
        DataConvertor.NumFormatInfo.NumberDecimalSeparator = decimalSeparator;
      }
    }
    return true;
  }

  public static bool ConvertRtfToStr(string rftValue, out string strValue)
  {
    return DataConvertor.RtfConvertorLazy.Value.ConvertToString(rftValue, out strValue);
  }
}
