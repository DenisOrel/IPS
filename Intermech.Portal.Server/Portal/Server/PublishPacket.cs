// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.PublishPacket
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.BlobStream;
using Intermech.Interfaces.WebPortal;
using Intermech.IO;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace Intermech.Portal.Server;

internal sealed class PublishPacket(IUserSession session, IDBObject obj) : GroupPublishItem(session, obj)
{
  private BlobWriterStream _dataStream;
  private BinaryWriter _writer;

  private void SaveInfoLength(int length)
  {
    using (BinaryWriter binaryWriter = new BinaryWriter((Stream) this._dataStream, Encoding.UTF8))
      binaryWriter.Write(length);
  }

  public override void CommitCreate()
  {
    if (this._dataStream != null)
      this._dataStream.Commit();
    if (this._writer != null)
      this._writer.Close();
    base.CommitCreate();
  }

  public void AddData(TransferedObject unit, string unitTempDirectory, string tag)
  {
    if (this._dataStream == null)
    {
      this._dataStream = new BlobWriterStream(this.DBObject.GetAttributeByGuid(PortalConsts.attributePacketFiles, false) ?? this.DBObject.Attributes.AddAttribute(MetaDataHelper.GetAttributeTypeID(PortalConsts.attributePacketFiles), false), 0, new BlobInformation(0L, 0L, DateTime.Now, string.Empty, ArcMethods.NotPacked, string.Empty), this.session);
      this._writer = new BinaryWriter((Stream) this._dataStream, Encoding.UTF8);
    }
    using (ImChunkedStream output = new ImChunkedStream())
    {
      BinaryWriter writer = new BinaryWriter((Stream) output, Encoding.UTF8);
      try
      {
        unit.Save(writer);
        if (!string.IsNullOrEmpty(tag))
        {
          writer.Write(tag.Length);
          writer.Write(tag.ToCharArray());
        }
        else
          writer.Write(0);
        writer.Write(unit.DataFiles.Length);
        for (int index = 0; index < unit.DataFiles.Length; ++index)
        {
          FileInfo fileInfo = new FileInfo(Path.Combine(unitTempDirectory, unit.DataFiles[index]));
          writer.Write(fileInfo.Length);
        }
      }
      finally
      {
        writer.Flush();
      }
      this._writer.Write(Convert.ToInt32(output.Length));
      output.Position = 0L;
      output.CopyTo((Stream) this._dataStream);
    }
    for (int index = 0; index < unit.DataFiles.Length; ++index)
    {
      using (FileStream fileStream = File.OpenRead(Path.Combine(unitTempDirectory, unit.DataFiles[index])))
        fileStream.CopyTo((Stream) this._dataStream);
    }
  }

  public string Name
  {
    get
    {
      IDBAttribute attributeByGuid = this.DBObject.GetAttributeByGuid(new Guid("cad00020-306c-11d8-b4e9-00304f19f545"));
      return attributeByGuid == null ? string.Empty : attributeByGuid.AsString;
    }
  }

  public string Designation
  {
    get
    {
      IDBAttribute attributeByGuid = this.DBObject.GetAttributeByGuid(new Guid("cad0001f-306c-11d8-b4e9-00304f19f545"));
      return attributeByGuid == null ? string.Empty : attributeByGuid.AsString;
    }
  }
}
