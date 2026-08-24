// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.ItemFactories.CompositionAttributeFactory
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using Intermech.ImpExp.Interface;
using System;
using System.Data;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.Search.ItemFactories;

internal class CompositionAttributeFactory : PumpItemFactory
{
  public static string TableName = "PC_PARAMS_CFG";
  public static string TableColumns = "PARAM_ID, P_LABEL, P_FIELD, CFG_DATA, ISINHERITED";
  private static int idxParamId = -1;
  private static int idxLabel = -1;
  private static int idxField = -1;
  private static int idxCfgData = -1;
  private static int idxIsInherited = -1;

  public CompositionAttributeFactory(
    string tableName,
    IDataReader dataReader,
    IAppManager appManager)
    : base(tableName, dataReader, appManager)
  {
    string fieldName1 = "PARAM_ID";
    string fieldName2 = "P_LABEL";
    string fieldName3 = "P_FIELD";
    string fieldName4 = "CFG_DATA";
    string fieldName5 = "ISINHERITED";
    CompositionAttributeFactory.idxParamId = this.getFieldIndex(fieldName1);
    CompositionAttributeFactory.idxLabel = this.getFieldIndex(fieldName2);
    CompositionAttributeFactory.idxField = this.getFieldIndex(fieldName3);
    CompositionAttributeFactory.idxCfgData = this.getFieldIndex(fieldName4);
    CompositionAttributeFactory.idxIsInherited = this.getFieldIndex(fieldName5);
  }

  public ICompositionAttribute NewItem(IDataReader idr)
  {
    CompositionAttribute compositionAttribute = new CompositionAttribute();
    compositionAttribute.ParamID = this.getInt32(idr, CompositionAttributeFactory.idxParamId);
    compositionAttribute.Name = this.getString(idr, CompositionAttributeFactory.idxLabel);
    compositionAttribute.DBField = this.getString(idr, CompositionAttributeFactory.idxField);
    if (!idr.IsDBNull(CompositionAttributeFactory.idxCfgData))
    {
      int length = 4096 /*0x1000*/;
      byte[] buffer = new byte[length];
      int fieldOffset = 0;
      MemoryStream memoryStream = new MemoryStream();
      try
      {
        while (true)
        {
          int bytes = (int) idr.GetBytes(CompositionAttributeFactory.idxCfgData, (long) fieldOffset, buffer, 0, length);
          if (bytes > 0)
          {
            fieldOffset += bytes;
            memoryStream.Write(buffer, 0, bytes);
          }
          else
            break;
        }
        memoryStream.Position = 0L;
        StreamReader streamReader = new StreamReader((Stream) memoryStream, this.dataBaseEncoding);
        try
        {
          while (streamReader.Peek() >= 0)
          {
            string str1 = streamReader.ReadLine();
            if (str1 != string.Empty)
            {
              string str2 = str1.Substring(0, str1.IndexOf("="));
              string str3 = str1.Substring(str1.IndexOf("=") + 1);
              if (str2.IndexOf(".") != -1)
              {
                if (str2.IndexOf(".Imbase") != -1)
                {
                  int num = 0;
                  try
                  {
                    num = Convert.ToInt32(str3.Trim());
                  }
                  catch
                  {
                  }
                  compositionAttribute.IsImbaseLink = num == 1;
                }
                else if (str2.IndexOf(".Size") != -1)
                {
                  try
                  {
                    compositionAttribute.Size = Convert.ToInt32(str3.Trim());
                  }
                  catch
                  {
                  }
                }
              }
            }
          }
        }
        finally
        {
          streamReader.Close();
        }
      }
      finally
      {
        memoryStream.Close();
      }
    }
    compositionAttribute.IsInherited = this.getInt32(idr, CompositionAttributeFactory.idxIsInherited);
    return (ICompositionAttribute) compositionAttribute;
  }
}
