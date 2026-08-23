// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Server.CryptoExporter
// Assembly: Intermech.Signs.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 3ABC2C25-9F6B-4AA7-A176-FCB28F816B8C
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Signs.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Briefcase;
using Intermech.Signs.Interfaces;
using System;

#nullable disable
namespace Intermech.Signs.Server;

public class CryptoExporter : ICategoryExport
{
  public string ExporterName => "Signs.CryptoExporter";

  public long[] GetLinkedObjectVersions(IUserSession session, int category, object id)
  {
    if (category == 1)
    {
      IDBObject dbObject1 = session.GetObject(Convert.ToInt64(id), false);
      if (dbObject1 != null)
      {
        try
        {
          if (dbObject1.ObjectType == SignsHolder.CryptoSignObjectTypeID)
          {
            IDBAttribute attributeByGuid = dbObject1.GetAttributeByGuid(SignsHolder.OpenKeysAttrTypeGuid, false);
            if (attributeByGuid != null)
            {
              if (attributeByGuid.Values.Length != 0)
              {
                for (int index = 0; index < attributeByGuid.Values.Length; ++index)
                {
                  OpenKey openKey = new OpenKey(attributeByGuid.Values[index].ToString());
                  IDBObject dbObject2 = session.GetObject(openKey.ProviderGuid, false);
                  if (dbObject2 != null)
                    return new long[1]{ dbObject2.ObjectID };
                }
              }
            }
          }
        }
        catch
        {
        }
      }
    }
    return (long[]) null;
  }

  public ExportAttribute[] GetLinkedDataByAttribute(
    IUserSession session,
    AttributableElements kind,
    long id,
    IDBAttributable iDBAttributable,
    int attributeId,
    object attrValueOriginal,
    ref object attrValueCurrent)
  {
    return (ExportAttribute[]) null;
  }

  public bool ProcessShortBlobs => false;
}
