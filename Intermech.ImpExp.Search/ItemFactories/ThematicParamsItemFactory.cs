// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.ItemFactories.ThematicParamsItemFactory
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using Intermech.ImpExp.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.Search.ItemFactories;

internal class ThematicParamsItemFactory : PumpItemFactory
{
  public static string TableName = "PARAMSTBL";
  public static string TableColumns = "PARAM_ID, P_LABEL, GROUP_ID, PARAM_TYPE, ALIASDOC,ALIASART, ART_OR_DOC, SRCALIAS, SRCFIELD, SRC_BD, U_NAME, BD_PWD, DEF_VALUES";
  private static int idxParamId = -1;
  private static int idxLabel = -1;
  private static int idxGroupId = -1;
  private static int idxParamType = -1;
  private static int idxAliasDoc = -1;
  private static int idxAliasArt = -1;
  private static int idxArtOrDoc = -1;
  private static int idxSrcAlias = -1;
  private static int idxSrcField = -1;
  private static int idxSrcBd = -1;
  private static int idxUName = -1;
  private static int idxBdPwd = -1;
  private static int idxDefValues = -1;

  public ThematicParamsItemFactory(
    string tableName,
    IDataReader dataReader,
    IAppManager appManager)
    : base(tableName, dataReader, appManager)
  {
    string fieldName1 = "PARAM_ID";
    string fieldName2 = "P_LABEL";
    string fieldName3 = "GROUP_ID";
    string fieldName4 = "PARAM_TYPE";
    string fieldName5 = "ALIASDOC";
    string fieldName6 = "ALIASART";
    string fieldName7 = "ART_OR_DOC";
    string fieldName8 = "SRCALIAS";
    string fieldName9 = "SRCFIELD";
    string fieldName10 = "SRC_BD";
    string fieldName11 = "U_NAME";
    string fieldName12 = "BD_PWD";
    string fieldName13 = "DEF_VALUES";
    ThematicParamsItemFactory.idxParamId = this.getFieldIndex(fieldName1);
    ThematicParamsItemFactory.idxLabel = this.getFieldIndex(fieldName2);
    ThematicParamsItemFactory.idxGroupId = this.getFieldIndex(fieldName3);
    ThematicParamsItemFactory.idxParamType = this.getFieldIndex(fieldName4);
    ThematicParamsItemFactory.idxAliasDoc = this.getFieldIndex(fieldName5);
    ThematicParamsItemFactory.idxAliasArt = this.getFieldIndex(fieldName6);
    ThematicParamsItemFactory.idxArtOrDoc = this.getFieldIndex(fieldName7);
    ThematicParamsItemFactory.idxSrcAlias = this.getFieldIndex(fieldName8);
    ThematicParamsItemFactory.idxSrcField = this.getFieldIndex(fieldName9);
    ThematicParamsItemFactory.idxSrcBd = this.getFieldIndex(fieldName10);
    ThematicParamsItemFactory.idxUName = this.getFieldIndex(fieldName11);
    ThematicParamsItemFactory.idxBdPwd = this.getFieldIndex(fieldName12);
    ThematicParamsItemFactory.idxDefValues = this.getFieldIndex(fieldName13);
  }

  public IThematicParamsItem NewItem(IDataReader idr)
  {
    ThematicParamsItemFactory.ThematicParamsItem thematicParamsItem = new ThematicParamsItemFactory.ThematicParamsItem();
    thematicParamsItem.paramId = this.getInt32(idr, ThematicParamsItemFactory.idxParamId);
    thematicParamsItem.label = this.getString(idr, ThematicParamsItemFactory.idxLabel);
    thematicParamsItem.groupId = this.getInt32(idr, ThematicParamsItemFactory.idxGroupId);
    thematicParamsItem.paramType = (ThematicParamsType) this.getInt32(idr, ThematicParamsItemFactory.idxParamType);
    thematicParamsItem.aliasDoc = this.getString(idr, ThematicParamsItemFactory.idxAliasDoc);
    thematicParamsItem.aliasArt = this.getString(idr, ThematicParamsItemFactory.idxAliasArt);
    thematicParamsItem.artOrDoc = this.getInt32(idr, ThematicParamsItemFactory.idxArtOrDoc);
    thematicParamsItem.srcAlias = this.getString(idr, ThematicParamsItemFactory.idxSrcAlias);
    thematicParamsItem.srcField = this.getString(idr, ThematicParamsItemFactory.idxSrcField);
    thematicParamsItem.srcBd = this.getString(idr, ThematicParamsItemFactory.idxSrcBd);
    thematicParamsItem.uName = this.getString(idr, ThematicParamsItemFactory.idxUName);
    thematicParamsItem.bdPwd = this.getString(idr, ThematicParamsItemFactory.idxBdPwd);
    thematicParamsItem.Guid = Guid.NewGuid();
    if (!idr.IsDBNull(ThematicParamsItemFactory.idxDefValues))
    {
      int length = 4096 /*0x1000*/;
      byte[] buffer = new byte[length];
      int fieldOffset = 0;
      MemoryStream memoryStream = new MemoryStream();
      try
      {
        while (true)
        {
          int bytes = (int) idr.GetBytes(ThematicParamsItemFactory.idxDefValues, (long) fieldOffset, buffer, 0, length);
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
            thematicParamsItem.defValues.Add(streamReader.ReadLine());
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
    return (IThematicParamsItem) thematicParamsItem;
  }

  private class ThematicParamsItem : IThematicParamsItem
  {
    internal int paramId;
    internal string label = string.Empty;
    internal int groupId;
    internal ThematicParamsType paramType = ThematicParamsType.ptUnknown;
    internal string aliasDoc = string.Empty;
    internal string aliasArt = string.Empty;
    internal int artOrDoc;
    internal string srcAlias = string.Empty;
    internal string srcField = string.Empty;
    internal string srcBd = string.Empty;
    internal string uName = string.Empty;
    internal string bdPwd = string.Empty;
    private int size;
    private Guid guid;
    internal List<string> defValues = new List<string>();

    public FieldTypes NewFieldType
    {
      get
      {
        FieldTypes newFieldType = FieldTypes.ftUnknown;
        switch (this.paramType)
        {
          case ThematicParamsType.ptUnknown:
            newFieldType = FieldTypes.ftUnknown;
            break;
          case ThematicParamsType.ptString:
            newFieldType = this.size > Consts.MaxStringSize ? FieldTypes.ftMemo : FieldTypes.ftString;
            break;
          case ThematicParamsType.ptInteger:
            newFieldType = FieldTypes.ftInteger;
            break;
          case ThematicParamsType.ptDouble:
            newFieldType = FieldTypes.ftDouble;
            break;
          case ThematicParamsType.ptDateTime:
            newFieldType = FieldTypes.ftDateTime;
            break;
          case ThematicParamsType.ptText:
            newFieldType = FieldTypes.ftMemo;
            break;
        }
        return newFieldType;
      }
    }

    public int ParamId => this.paramId;

    public string Label
    {
      get => this.label;
      set => this.label = value;
    }

    public int GroupId => this.groupId;

    public ThematicParamsType ParamType => this.paramType;

    public string AliasDoc => this.aliasDoc;

    public string AliasArt => this.aliasArt;

    public int ArtOrDoc => this.artOrDoc;

    public string SrcAlias => this.srcAlias;

    public string SrcField => this.srcField;

    public string SrcBd => this.srcBd;

    public string UName => this.uName;

    public string BdPwd => this.bdPwd;

    public string DefValue
    {
      get => this.defValues == null || this.defValues.Count <= 0 ? string.Empty : this.defValues[0];
    }

    public List<string> LisValues
    {
      get
      {
        List<string> lisValues = new List<string>();
        if (this.defValues != null && this.defValues.Count > 1)
        {
          for (int index = 0; index < this.defValues.Count; ++index)
          {
            if (index == 0)
            {
              if (this.defValues[index] != string.Empty)
                lisValues.Add(this.defValues[index]);
            }
            else
              lisValues.Add(this.defValues[index]);
          }
        }
        return lisValues;
      }
    }

    public Guid Guid
    {
      get => this.guid;
      set => this.guid = value;
    }

    public int Size
    {
      get => this.size;
      set => this.size = value;
    }
  }
}
