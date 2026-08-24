// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.ItemFactories.SelectionsPicturesFactory
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using System;
using System.Data;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.SearchData.ItemFactories;

internal class SelectionsPicturesFactory : PumpItemFactory
{
  public static string TableName = "SMPPICT";
  public static string TableColumns = "SAMPLE_ID, FILEBODY, FILE_EXT";
  private static int idxSampleID = -1;
  private static int idxFileBody = -1;
  private static int idxFileExt = -1;

  public SelectionsPicturesFactory(IDataReader dataReader, IAppManager appManager)
    : base(SelectionsPicturesFactory.TableName, dataReader, appManager)
  {
    string fieldName1 = "SAMPLE_ID";
    string fieldName2 = "FILEBODY";
    string fieldName3 = "FILE_EXT";
    SelectionsPicturesFactory.idxSampleID = this.getFieldIndex(fieldName1);
    SelectionsPicturesFactory.idxFileBody = this.getFieldIndex(fieldName2);
    SelectionsPicturesFactory.idxFileExt = this.getFieldIndex(fieldName3);
  }

  public ISelectionsPicture NewItem(IDataReader idr)
  {
    SelectionsPicturesFactory.SelectionsPicture selectionsPicture = new SelectionsPicturesFactory.SelectionsPicture();
    selectionsPicture.sampleID = this.getInt32(idr, SelectionsPicturesFactory.idxSampleID);
    if (!idr.IsDBNull(SelectionsPicturesFactory.idxFileBody))
    {
      int length = 4096 /*0x1000*/;
      byte[] buffer = new byte[length];
      int fieldOffset = 0;
      MemoryStream memoryStream = new MemoryStream();
      try
      {
        while (true)
        {
          int bytes = (int) idr.GetBytes(SelectionsPicturesFactory.idxFileBody, (long) fieldOffset, buffer, 0, length);
          if (bytes > 0)
          {
            fieldOffset += bytes;
            memoryStream.Write(buffer, 0, bytes);
          }
          else
            break;
        }
        selectionsPicture.fileBody = memoryStream.ToArray();
      }
      catch (Exception ex)
      {
        this.appMngr.AddWarningMessage($"Не удалось прочитать изображение для выборки {selectionsPicture.sampleID} из базы SEARCH: {ex.Message}");
        selectionsPicture.fileBody = (byte[]) null;
      }
      finally
      {
        memoryStream.Close();
      }
    }
    selectionsPicture.fileExt = this.getString(idr, SelectionsPicturesFactory.idxFileExt);
    return (ISelectionsPicture) selectionsPicture;
  }

  private class SelectionsPicture : ISelectionsPicture
  {
    internal int sampleID = -1;
    internal byte[] fileBody;
    internal string fileExt = string.Empty;

    public int SampleID => this.sampleID;

    public byte[] FileBody => this.fileBody;

    public string FileExt => this.fileExt;
  }
}
