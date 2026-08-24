// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.CompareTreeSettingsService
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.IO;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

public sealed class CompareTreeSettingsService : ICompareTreeSettingsService
{
  private readonly List<Tuple<Guid, ICompoitionSettings>> _settings;
  private readonly int _dataAttributeID;
  private readonly int _attributeObjectTypeGuidsID;

  public CompareTreeSettingsService()
  {
    this._settings = new List<Tuple<Guid, ICompoitionSettings>>();
    this._dataAttributeID = MetaDataHelper.GetAttributeTypeID("cadd9a97-306c-11d8-b4e9-00304f19f545");
    this._attributeObjectTypeGuidsID = MetaDataHelper.GetAttributeTypeID("cad00149-306c-11d8-b4e9-00304f19f545");
  }

  public ICompoitionSettings GetCompoitionSettings(Guid ruleID)
  {
    lock (this._settings)
    {
      Tuple<Guid, ICompoitionSettings> tuple = this._settings.Find((Predicate<Tuple<Guid, ICompoitionSettings>>) (x => x.Item1.Equals(ruleID)));
      if (tuple != null)
        return tuple.Item2;
      if (VirtualCompoitionSettings.VirtualSchemes.ContainsKey(ruleID))
      {
        VirtualCompoitionSettings compoitionSettings = new VirtualCompoitionSettings();
        this._settings.Add(new Tuple<Guid, ICompoitionSettings>(ruleID, (ICompoitionSettings) compoitionSettings));
        return (ICompoitionSettings) compoitionSettings;
      }
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBAttribute attributeById = sessionKeeper.Session.GetObject(ruleID).GetAttributeByID(this._dataAttributeID);
        if (attributeById != null)
        {
          if (!attributeById.IsNull)
          {
            IBlobReader blobReader = attributeById as IBlobReader;
            byte[] buffer = (byte[]) null;
            BlobInformation blobInformation = blobReader.OpenBlob(0);
            try
            {
              if (blobInformation.RealFileSize != 0L)
                buffer = blobReader.ReadDataBlock();
            }
            finally
            {
              blobReader.CloseBlob();
            }
            if (buffer != null)
            {
              if (buffer.Length != 0)
              {
                using (MemoryStream inStream = new MemoryStream(buffer))
                {
                  inStream.Position = 0L;
                  CompoitionSettings compoitionSettings = new CompoitionSettings();
                  if (blobInformation.ArcMethod == ArcMethods.ZLibPacked)
                  {
                    using (ImChunkedStream outStream = new ImChunkedStream())
                    {
                      ZLibStreamHelper.UnpackStream((Stream) inStream, (Stream) outStream);
                      compoitionSettings.Load((Stream) outStream);
                    }
                  }
                  else
                    compoitionSettings.Load((Stream) inStream);
                  this._settings.Add(new Tuple<Guid, ICompoitionSettings>(ruleID, (ICompoitionSettings) compoitionSettings));
                  return compoitionSettings.Clone();
                }
              }
            }
          }
        }
      }
      return (ICompoitionSettings) CompoitionSettings.CreateNew();
    }
  }

  public void SetCompoitionSettings(IDBObject ruleObject, ICompoitionSettings newSettings)
  {
    Tuple<Guid, ICompoitionSettings> tuple = this._settings.Find((Predicate<Tuple<Guid, ICompoitionSettings>>) (x => x.Item1.Equals(ruleObject.ObjectGUID)));
    if (tuple != null)
      this._settings.Remove(tuple);
    ICompoitionSettings compoitionSettings = newSettings.Clone();
    this._settings.Add(new Tuple<Guid, ICompoitionSettings>(ruleObject.ObjectGUID, compoitionSettings));
    IDBAttribute attributeById1 = ruleObject.GetAttributeByID(this._attributeObjectTypeGuidsID);
    attributeById1.ClearValues();
    List<object> objectList = new List<object>();
    foreach (Tuple<int, int, List<int>> childType in compoitionSettings.ChildTypes)
      objectList.Add((object) MetaDataHelper.GetObjectTypeGuid(childType.Item1));
    if (objectList.Count > 0)
      attributeById1.Values = objectList.ToArray();
    IDBAttribute attributeById2 = ruleObject.GetAttributeByID(this._dataAttributeID);
    using (ImChunkedStream inStream = new ImChunkedStream())
    {
      compoitionSettings.Save((Stream) inStream);
      using (ImChunkedStream outStream = new ImChunkedStream())
      {
        ZLibStreamHelper.PackStream((Stream) inStream, ZLibCompressLevels.LevelMax, (Stream) outStream);
        IBlobWriter blobWriter = attributeById2 as IBlobWriter;
        blobWriter.OpenBlob(new BlobInformation(inStream.Length, outStream.Length, DateTime.Now, string.Empty, ArcMethods.ZLibPacked, string.Empty), false);
        blobWriter.WriteDataBlock(outStream.ToArray());
      }
    }
  }

  public List<int> GetChildobjectTypeIDs(Guid ruleID, int parentTypeID, int relationTypeID)
  {
    ICompoitionSettings compoitionSettings = this.GetCompoitionSettings(ruleID);
    return compoitionSettings == null ? new List<int>(0) : compoitionSettings.GetChildTypes(parentTypeID, relationTypeID);
  }

  public List<int> GetIDObjectAttributes(Guid ruleID, int objectTypeID)
  {
    ICompoitionSettings compoitionSettings = this.GetCompoitionSettings(ruleID);
    return compoitionSettings == null ? new List<int>(0) : compoitionSettings.GetIDObjectAttributes(objectTypeID);
  }

  public List<int> GetIDRelationAttributes(Guid ruleID, int parentTypeID, int relationTypeID)
  {
    return this.GetCompoitionSettings(ruleID).GetIDRelationAttributes(parentTypeID, relationTypeID);
  }

  public List<int> GetObjectCompareAttributes(Guid ruleID, int objectTypeID)
  {
    ICompoitionSettings compoitionSettings = this.GetCompoitionSettings(ruleID);
    return compoitionSettings == null ? new List<int>(0) : compoitionSettings.GetObjectCompareAttributes(objectTypeID);
  }

  public List<int> GetRelationCompareAttributes(Guid ruleID, int relationTypeID)
  {
    ICompoitionSettings compoitionSettings = this.GetCompoitionSettings(ruleID);
    return compoitionSettings == null ? new List<int>(0) : compoitionSettings.GetRelationCompareAttributes(relationTypeID);
  }

  public List<int> GetRelationTypes(Guid ruleID, int objectTypeID)
  {
    ICompoitionSettings compoitionSettings = this.GetCompoitionSettings(ruleID);
    return compoitionSettings == null ? new List<int>(0) : compoitionSettings.GetRelationTypes(objectTypeID);
  }

  public List<Tuple<int, AttributeSourceTypes>> GetSortedAttributes(Guid ruleID, int parentTypeID)
  {
    ICompoitionSettings compoitionSettings = this.GetCompoitionSettings(ruleID);
    return compoitionSettings == null ? new List<Tuple<int, AttributeSourceTypes>>(0) : compoitionSettings.GetSortedAttributes(parentTypeID);
  }

  public bool CheckExistsAttributes(Guid ruleID)
  {
    return this.GetCompoitionSettings(ruleID).CheckExistsAttributes;
  }
}
