// Decompiled with JetBrains decompiler
// Type: Intermech.Search.MSOfficeReviews.MSOfficeReviewsClientHelper
// Assembly: Intermech.Search.MSOfficeReviews.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 4AB1E446-C278-4B7C-8A5E-DB94EF37D83B
// Assembly location: D:\IPS\Client\Intermech.Search.MSOfficeReviews.Client.dll

using ClosedXML.Excel;
using DocumentFormat.OpenXml.Packaging;
using Intermech.Interfaces;
using Intermech.OpenXml;
using Intermech.Search.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#nullable disable
namespace Intermech.Search.MSOfficeReviews;

public static class MSOfficeReviewsClientHelper
{
  public static long MergeReviews(long[] reviewVersionIds)
  {
    if (reviewVersionIds == null)
      throw new ArgumentOutOfRangeException(nameof (reviewVersionIds));
    string str = !ObjectHelper.IsAnyUnknownObjectVersionID((IEnumerable<long>) reviewVersionIds) && reviewVersionIds.Length >= 2 ? MSOfficeReviewsClientHelper.ReadReviewToFile(reviewVersionIds[0]) : throw new ArgumentException();
    try
    {
      if (MSOfficeReviewsClientHelper.IsWordReview(reviewVersionIds[0]))
      {
        WordprocessingDocument destinationDocument;
        try
        {
          destinationDocument = WordprocessingDocument.Open(str, true);
        }
        catch (Exception ex)
        {
          throw new Exception("Данная функция поддерживается только для файлов *.docx.", ex);
        }
        try
        {
          foreach (long reviewVersionID in ((IEnumerable<long>) reviewVersionIds).Skip<long>(1))
          {
            string file = MSOfficeReviewsClientHelper.ReadReviewToFile(reviewVersionID);
            try
            {
              using (WordprocessingDocument sourceDocument = WordprocessingDocument.Open(file, true))
                OpenXmlWordprocessingHelper.MergeComments(sourceDocument, destinationDocument);
            }
            finally
            {
              File.Delete(file);
            }
          }
        }
        finally
        {
          destinationDocument.Dispose();
        }
        return MSOfficeReviewsClientHelper.CreateReviewFromFile(MSOfficeReviewsConstants.MSWordReviewObjectTypeID, str);
      }
      if (!MSOfficeReviewsClientHelper.IsExelReview(reviewVersionIds[0]))
        throw new NotSupportedException();
      XLWorkbook xlWorkbook1;
      try
      {
        xlWorkbook1 = new XLWorkbook(str);
      }
      catch (Exception ex)
      {
        throw new Exception("Данная функция поддерживается только для файлов *.xlsx", ex);
      }
      try
      {
        foreach (long reviewVersionID in ((IEnumerable<long>) reviewVersionIds).Skip<long>(1))
        {
          string file = MSOfficeReviewsClientHelper.ReadReviewToFile(reviewVersionID);
          try
          {
            using (XLWorkbook xlWorkbook2 = new XLWorkbook(file))
            {
              foreach (IXLWorksheet worksheet in (IEnumerable<IXLWorksheet>) xlWorkbook2.Worksheets)
              {
                IXLRow xlRow1 = worksheet.FirstRowUsed();
                IXLRow xlRow2 = worksheet.LastRowUsed();
                IXLColumn xlColumn1 = worksheet.FirstColumnUsed();
                IXLColumn xlColumn2 = worksheet.LastColumnUsed();
                if (xlRow1 != null && xlRow2 != null && xlColumn1 != null && xlColumn2 != null)
                {
                  for (int row = xlRow1.RowNumber(); row <= xlRow2.RowNumber(); ++row)
                  {
                    for (int column = xlColumn1.ColumnNumber(); column <= xlColumn2.ColumnNumber(); ++column)
                    {
                      IXLCell xlCell1 = worksheet.Cell(row, column);
                      if (xlCell1.HasComment && !string.IsNullOrEmpty(xlCell1.Comment.Text))
                      {
                        using (IXLWorksheet xlWorksheet = xlWorkbook1.Worksheet(worksheet.Name))
                        {
                          IXLCell xlCell2 = xlWorksheet.Cell(row, column);
                          if (xlCell2.HasComment && !string.IsNullOrEmpty(xlCell2.Comment.Text))
                            xlCell2.Comment.AddNewLine();
                          foreach (IXLRichString xlRichString in (IEnumerable<IXLRichString>) xlCell1.Comment)
                            xlCell2.Comment.AddText(xlRichString.Text);
                        }
                      }
                    }
                  }
                }
                worksheet.Dispose();
              }
            }
          }
          finally
          {
            File.Delete(file);
          }
        }
        xlWorkbook1.Save();
      }
      finally
      {
        xlWorkbook1.Dispose();
      }
      return MSOfficeReviewsClientHelper.CreateReviewFromFile(MSOfficeReviewsConstants.MSWordReviewObjectTypeID, str);
    }
    finally
    {
      File.Delete(str);
    }
  }

  private static bool IsWordReview(long reviewVersionID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      return sessionKeeper.Session.GetObject(reviewVersionID).ObjectType == MSOfficeReviewsConstants.MSWordReviewObjectTypeID;
  }

  private static bool IsExelReview(long reviewVersionID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      return sessionKeeper.Session.GetObject(reviewVersionID).ObjectType == MSOfficeReviewsConstants.MSExelReviewObjectTypeID;
  }

  private static string ReadReviewToFile(long reviewVersionID)
  {
    string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBAttribute attributeById = sessionKeeper.Session.GetObject(reviewVersionID).GetAttributeByID(MSOfficeReviewsConstants.FileAttributeTypeID);
      path = $"{path}_{attributeById.AsString}";
      using (FileStream aDestStream = new FileStream(path, FileMode.CreateNew))
        new BlobProcReader(attributeById, 0, (Stream) aDestStream, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).ReadData();
    }
    return path;
  }

  private static long CreateReviewFromFile(int reviewTypeID, string fileName)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObjectCollection(reviewTypeID).Create();
      using (FileStream aSourceStream = new FileStream(fileName, FileMode.Open))
        new BlobProcWriter(dbObject.GetAttributeByID(MSOfficeReviewsConstants.FileAttributeTypeID), 0, new BlobInformation()
        {
          ArcMethod = ArcMethods.ZLibPacked,
          FileName = $"Review {Guid.NewGuid().ToString()}{Path.GetExtension(fileName)}",
          ModifyDate = DateTime.Now,
          RealFileSize = aSourceStream.Length
        }, (Stream) aSourceStream, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).WriteData();
      dbObject.CommitCreation(true);
      return dbObject.ObjectID;
    }
  }
}
