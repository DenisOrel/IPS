// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.UpdateDataAttributeHelper
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.BlobStream;
using Intermech.Interfaces.WebPortal;
using Intermech.IO;
using Intermech.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace Intermech.Portal.Server;

internal static class UpdateDataAttributeHelper
{
  public static void Save(
    IUserSession session,
    IDBAttribute attrUnits,
    List<TransferedObject> units)
  {
    if (units == null)
      return;
    BlobWriterStream destination = new BlobWriterStream(attrUnits, 0, new BlobInformation(0L, 0L, DateTime.Now, string.Empty, ArcMethods.ZLibPacked, string.Empty), session);
    for (int index = 0; index < units.Count; ++index)
    {
      using (ImChunkedStream output = new ImChunkedStream())
      {
        BinaryWriter writer = new BinaryWriter((Stream) output, Encoding.UTF8);
        try
        {
          units[index].Save(writer);
        }
        finally
        {
          writer.Flush();
        }
        output.Position = 0L;
        output.CopyTo((Stream) destination);
      }
    }
    destination.Commit();
  }

  public static List<TransferedObject> Load(
    IDBAttribute attrUnits,
    bool correctFilePath,
    bool throwiffilesnotexists = true)
  {
    List<TransferedObject> transferedObjectList = new List<TransferedObject>(attrUnits.ValuesCount);
    IBlobReader blobReader = attrUnits as IBlobReader;
    BlobInformation blobInformation = blobReader.OpenBlob(0);
    try
    {
      if (blobInformation.RealFileSize > 0L)
      {
        byte[] buffer = blobReader.ReadDataBlock(0);
        if (buffer != null)
        {
          Stream stream = (Stream) new MemoryStream(buffer);
          try
          {
            stream.Position = 0L;
            if (blobInformation.ArcMethod == ArcMethods.ZLibPacked)
            {
              ImChunkedStream outStream = new ImChunkedStream();
              ZLibStreamHelper.UnpackStream(stream, (Stream) outStream);
              stream.Close();
              stream = (Stream) outStream;
            }
            stream.Position = 0L;
            BinaryReader reader = new BinaryReader(stream, Encoding.UTF8);
            try
            {
              while (stream.Position < stream.Length)
              {
                TransferedObject instance = (TransferedObject) Activator.CreateInstance(typeof (TransferedObject));
                instance.Load(reader);
                if (correctFilePath && instance.DataFiles != null && instance.DataFiles.Length != 0)
                {
                  string updateUnitPath = TempStorage.GetUpdateUnitPath(instance.GUID);
                  for (int index = 0; index < instance.DataFiles.Length; ++index)
                  {
                    FileInfo fileInfo = new FileInfo(Path.Combine(updateUnitPath, instance.DataFiles[index]));
                    if (!fileInfo.Exists & throwiffilesnotexists)
                      throw new Exception(string.Format(LocalizationHolder.rm.GetString("PortalServer_43"), (object) fileInfo.FullName));
                    instance.DataFiles[index] = fileInfo.Name;
                  }
                }
                transferedObjectList.Add(instance);
              }
            }
            finally
            {
              reader.Close();
            }
          }
          finally
          {
            stream.Close();
          }
        }
      }
      else if (throwiffilesnotexists)
        throw new Exception(LocalizationHolder.rm.GetString("PortalServer_44"));
    }
    finally
    {
      blobReader.CloseBlob();
    }
    return transferedObjectList;
  }
}
