// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.ItemFactories.SelectionsFactory
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.SearchData.ItemFactories;

internal class SelectionsFactory : PumpItemFactory
{
  public static string TableName = "SMPLIST";
  public static string TableColumns = "SAMPLE_ID, ARCH_ID, SAMPLENAME, SAMPLEFLT, SAMPLEDATE, USER_ID, ISSAMPLE, CANANYEDIT, ISCOMMON, SAMPLEHMN, SMP_IN, INHERIT, DEL_CONTROL";
  private static int idxSampleID = -1;
  private static int idxArchID = -1;
  private static int idxSampleName = -1;
  private static int idxSampleFLT = -1;
  private static int idxSampleDate = -1;
  private static int idxUserID = -1;
  private static int idxIsSample = -1;
  private static int idxCanAnyEdit = -1;
  private static int idxIsCommon = -1;
  private static int idxSampleHMN = -1;
  private static int idxSmp_IN = -1;
  private static int idxInherit = -1;
  private static int idxDelControl = -1;

  public SelectionsFactory(IDataReader dataReader, IAppManager appManager)
    : base(SelectionsFactory.TableName, dataReader, appManager)
  {
    string fieldName1 = "SAMPLE_ID";
    string fieldName2 = "ARCH_ID";
    string fieldName3 = "SAMPLENAME";
    string fieldName4 = "SAMPLEFLT";
    string fieldName5 = "SAMPLEDATE";
    string fieldName6 = "USER_ID";
    string fieldName7 = "ISSAMPLE";
    string fieldName8 = "CANANYEDIT";
    string fieldName9 = "ISCOMMON";
    string fieldName10 = "SAMPLEHMN";
    string fieldName11 = "SMP_IN";
    string fieldName12 = "INHERIT";
    string fieldName13 = "DEL_CONTROL";
    SelectionsFactory.idxSampleID = this.getFieldIndex(fieldName1);
    SelectionsFactory.idxArchID = this.getFieldIndex(fieldName2);
    SelectionsFactory.idxSampleName = this.getFieldIndex(fieldName3);
    SelectionsFactory.idxSampleFLT = this.getFieldIndex(fieldName4);
    SelectionsFactory.idxSampleDate = this.getFieldIndex(fieldName5);
    SelectionsFactory.idxUserID = this.getFieldIndex(fieldName6);
    SelectionsFactory.idxIsSample = this.getFieldIndex(fieldName7);
    SelectionsFactory.idxCanAnyEdit = this.getFieldIndex(fieldName8);
    SelectionsFactory.idxIsCommon = this.getFieldIndex(fieldName9);
    SelectionsFactory.idxSampleHMN = this.getFieldIndex(fieldName10);
    SelectionsFactory.idxSmp_IN = this.getFieldIndex(fieldName11);
    SelectionsFactory.idxInherit = this.getFieldIndex(fieldName12);
    SelectionsFactory.idxDelControl = this.getFieldIndex(fieldName13);
  }

  private List<string> GetSampleFLT(IDataReader idr, int sampleID)
  {
    if (idr.IsDBNull(SelectionsFactory.idxSampleFLT))
      return (List<string>) null;
    List<string> sampleFlt = new List<string>();
    int length = 4096 /*0x1000*/;
    byte[] buffer = new byte[length];
    MemoryStream memoryStream = new MemoryStream();
    try
    {
      int fieldOffset = 0;
      int count = length;
      while (count == length)
      {
        count = (int) idr.GetBytes(SelectionsFactory.idxSampleFLT, (long) fieldOffset, buffer, 0, length);
        if (count > 0)
          memoryStream.Write(buffer, 0, count);
        fieldOffset += count;
      }
      memoryStream.Position = 0L;
      StreamReader streamReader = new StreamReader((Stream) memoryStream, this.dataBaseEncoding);
      try
      {
        while (streamReader.Peek() >= 0)
          sampleFlt.Add(streamReader.ReadLine());
      }
      finally
      {
        streamReader.Close();
      }
    }
    catch (Exception ex)
    {
      this.appMngr.AddWarningMessage($"Не удалось прочитать условия выборки {sampleID} из базы SEARCH: {ex.Message}");
      return (List<string>) null;
    }
    finally
    {
      memoryStream.Close();
    }
    return sampleFlt;
  }

  public ISelectionItem NewItem(IDataReader idr)
  {
    SelectionsFactory.SelectionItem selectionItem = new SelectionsFactory.SelectionItem()
    {
      sampleID = this.getInt32(idr, SelectionsFactory.idxSampleID),
      archID = this.getInt32(idr, SelectionsFactory.idxArchID),
      sampleName = this.getString(idr, SelectionsFactory.idxSampleName)
    };
    selectionItem.sampleFLT = this.GetSampleFLT(idr, selectionItem.sampleID);
    selectionItem.sampleDate = this.getDateTime(idr, SelectionsFactory.idxSampleDate);
    selectionItem.userID = this.getInt32(idr, SelectionsFactory.idxUserID);
    selectionItem.isSample = Convert.ToBoolean(this.getInt32(idr, SelectionsFactory.idxIsSample));
    selectionItem.canAnyEdit = Convert.ToBoolean(this.getInt32(idr, SelectionsFactory.idxCanAnyEdit));
    selectionItem.isCommon = this.getInt32(idr, SelectionsFactory.idxIsCommon);
    selectionItem.sampleHMN = this.getInt32(idr, SelectionsFactory.idxSampleHMN);
    selectionItem.smp_IN = this.getInt32(idr, SelectionsFactory.idxSmp_IN);
    selectionItem.inherit = Convert.ToBoolean(this.getInt32(idr, SelectionsFactory.idxInherit));
    selectionItem.delControl = this.getInt32(idr, SelectionsFactory.idxDelControl);
    return (ISelectionItem) selectionItem;
  }

  private class SelectionItem : ISelectionItem
  {
    internal int sampleID = -1;
    internal int archID = -1;
    internal string sampleName = string.Empty;
    internal List<string> sampleFLT;
    internal DateTime sampleDate = DateTime.MinValue;
    internal int userID;
    internal bool isSample;
    internal bool canAnyEdit;
    internal int isCommon = -1;
    internal int sampleHMN = -1;
    internal int smp_IN = -1;
    internal bool inherit;
    internal int delControl = -1;
    internal string description = string.Empty;

    public int SampleID => this.sampleID;

    public int ArchID => this.archID;

    public string SampleName => this.sampleName;

    public List<string> SampleFLT
    {
      get => this.sampleFLT;
      set => this.sampleFLT = value;
    }

    public DateTime SampleDate => this.sampleDate;

    public int UserID => this.userID;

    public bool IsSample => this.isSample;

    public bool CanAnyEdit => this.canAnyEdit;

    public int IsCommon => this.isCommon;

    public int SampleHMN => this.sampleHMN;

    public int Smp_IN => this.smp_IN;

    public bool Inherit => this.inherit;

    public int DelControl => this.delControl;

    public string Description => this.description;
  }
}
