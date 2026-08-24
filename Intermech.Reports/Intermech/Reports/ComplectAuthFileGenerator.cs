// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.ComplectAuthFileGenerator
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using Intermech.Document.Model.PdfGenerator;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Document;
using Intermech.Interfaces.Reports;
using Intermech.IO;
using Intermech.Reports.Commands;
using System;
using System.IO;

#nullable disable
namespace Intermech.Reports;

/// <summary>
/// Класс генерации аутентичных файлов для комплектов документов
/// </summary>
internal class ComplectAuthFileGenerator
{
  /// <summary>
  /// 
  /// </summary>
  private static ComplectAuthFileGenerator _instance;

  /// <summary>
  /// 
  /// </summary>
  /// <param name="args"></param>
  /// <returns></returns>
  private bool ValidateArgs(AuthFileAssignEventArgs args)
  {
    return args != null && !args.IsHandled && args.ObjectId != 0L && args.ObjectId != -1L && MetaDataHelper.IsObjectTypeChildOf(args.ObjectType, ReportsConsts.DocPackageBaseTypeID);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="args"></param>
  /// <param name="mainIndex"></param>
  /// <param name="existIndex"></param>
  /// <returns></returns>
  private bool NeedGenerate(AuthFileNeedGenerateEventArgs args, out int existIndex)
  {
    existIndex = -1;
    if (!this.ValidateArgs((AuthFileAssignEventArgs) args))
      return false;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBAttribute objectAttributeByGuid = sessionKeeper.Session.GetObjectAttributeByGuid(args.ObjectId, new Guid("cad0004b-306c-11d8-b4e9-00304f19f545"));
      if (objectAttributeByGuid != null)
      {
        for (int index = 0; index < objectAttributeByGuid.ValuesCount; ++index)
        {
          objectAttributeByGuid.Index = index;
          if (objectAttributeByGuid is IBlobReader blobReader)
          {
            BlobInformation blobInformation = blobReader.OpenBlob(-1);
            try
            {
              if (blobInformation.FileType == FileTypes.ftAuthentical)
              {
                existIndex = index;
                break;
              }
            }
            finally
            {
              blobReader.CloseBlob();
            }
          }
        }
      }
    }
    args.InternalDocument = true;
    args.NeedGenerate = true;
    args.IsHandled = true;
    return true;
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="args"></param>
  /// <returns></returns>
  private bool Generate(AuthFileAssignEventArgs args)
  {
    int existIndex;
    if (!this.ValidateArgs(args) || !this.NeedGenerate(new AuthFileNeedGenerateEventArgs(args.ObjectType, args.ObjectId, args.PDFOnly), out existIndex))
      return false;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(args.ObjectId);
      IDBAttribute aIDBAttribute = dbObject?.Attributes.AddAttribute(MetaDataHelper.GetAttributeID((object) "cad0004b-306c-11d8-b4e9-00304f19f545"), false);
      DocumentsComplect docComplect;
      if (aIDBAttribute == null || !ComplectBaseCommand.LoadDocumentComplect(args.ObjectId, out docComplect))
        return false;
      string str = ServiceUtils.GetService<IFileNamesService>((object) sessionKeeper.Session, true).GetUniqueFileName(dbObject.Caption, dbObject.ID, sessionKeeper.Session.SessionGUID) + ".pdf";
      using (Stream stream = (Stream) new ImChunkedStream())
      {
        PDFCreatePrinter.SaveToPdf(docComplect, stream, true);
        if (stream.Length == 0L)
          return false;
        if (existIndex != -1)
        {
          aIDBAttribute.Index = existIndex;
          if (aIDBAttribute is IBlobReader blobReader)
          {
            BlobInformation aBlobInformation = blobReader.OpenBlob(-1);
            try
            {
              aBlobInformation.ModifyDate = DateTime.Now;
              stream.Position = 0L;
              new BlobProcWriter(aIDBAttribute, 0, aBlobInformation, stream, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).WriteData();
            }
            finally
            {
              blobReader.CloseBlob();
            }
          }
        }
        else
        {
          aIDBAttribute.Index = aIDBAttribute.AddValue((object) FileTypes.ftAuthentical);
          if (aIDBAttribute is IBlobReader blobReader)
          {
            try
            {
              BlobInformation aBlobInformation = blobReader.OpenBlob(-1) with
              {
                FileType = FileTypes.ftAuthentical,
                FileName = str,
                ArcMethod = ArcMethods.ZLibPacked
              };
              stream.Position = 0L;
              new BlobProcWriter(aIDBAttribute, 0, aBlobInformation, stream, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).WriteData();
            }
            catch (Exception ex)
            {
              aIDBAttribute.DeleteValue();
              ExceptionHelper.ExceptionService.ShowException(ex);
            }
            finally
            {
              blobReader.CloseBlob();
            }
          }
        }
      }
      args.IsHandled = true;
    }
    return true;
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="eventArgs"></param>
  private void AuthFileNeedGenerate(object sender, AuthFileNeedGenerateEventArgs eventArgs)
  {
    this.NeedGenerate(eventArgs, out int _);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="eventArgs"></param>
  private void AuthFileAssignEvent(object sender, AuthFileAssignEventArgs eventArgs)
  {
    this.Generate(eventArgs);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="serviceContainer"></param>
  public static void Register(IServiceProvider serviceProvider)
  {
    if (!(serviceProvider.GetService(typeof (IAuthFilesService)) is IAuthFilesService service))
      return;
    if (ComplectAuthFileGenerator._instance == null)
      ComplectAuthFileGenerator._instance = new ComplectAuthFileGenerator();
    service.AuthFileAssignEvent += new AuthFileAssignEventHandler(ComplectAuthFileGenerator._instance.AuthFileAssignEvent);
    service.AuthFileNeedGenerate += new AuthFileNeedGenerateEventHandler(ComplectAuthFileGenerator._instance.AuthFileNeedGenerate);
  }
}
