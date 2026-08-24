// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.Caches.FileManager
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.Manager.Caches;

public sealed class FileManager
{
  private readonly int _defaultBufferSize = 262143 /*0x03FFFF*/;
  private readonly Dictionary<int, FileStream> _streams;
  private readonly Dictionary<int, CategoryСacheInfo> _keys;
  private readonly Dictionary<int, RecordType> _type;

  private void CloseStream(FileStream stream)
  {
    if (stream == null)
      return;
    try
    {
      stream.Flush();
      stream.Close();
    }
    catch (ObjectDisposedException ex)
    {
    }
  }

  private void DeleteFiles(params string[] filenames)
  {
    if (filenames == null || filenames.Length == 0)
      return;
    for (int index = 0; index < filenames.Length; ++index)
    {
      if (File.Exists(filenames[index]))
        File.Delete(filenames[index]);
    }
  }

  private void ReadFile(BinaryReader br, BaseCache cache, int category)
  {
    CategoryСacheInfo categoryСacheInfo = new CategoryСacheInfo();
    try
    {
      long num1 = 0;
      try
      {
        long length = br.BaseStream.Length;
        while (br.BaseStream.Position < length)
        {
          num1 = br.BaseStream.Position;
          long position1 = br.BaseStream.Position;
          long num2 = br.ReadInt64();
          long position2 = br.BaseStream.Position;
          long newKey = br.ReadInt64();
          int count1 = br.ReadInt32();
          string empty = string.Empty;
          if (count1 > 0)
          {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(br.ReadChars(count1));
            empty = stringBuilder.ToString();
          }
          ITagImportObject tag = (ITagImportObject) null;
          int count2 = br.ReadInt32();
          if (count2 > 0)
          {
            int classID = (int) br.ReadInt16();
            byte[] s = br.ReadBytes(count2);
            tag = TagImportObjectHelper.GetImportObject(classID);
            tag?.Load(s);
          }
          if (num2 != long.MinValue)
          {
            categoryСacheInfo.Add((object) num2, new long[2]
            {
              position1,
              position2
            });
            cache.AddValue((object) num2, newKey, empty, tag);
          }
        }
      }
      catch (Exception ex)
      {
        if (br != null)
          br.BaseStream.Position = num1;
        ExceptionHelper.ExceptionService.ShowException(ex);
      }
    }
    finally
    {
      this._keys.Add(category, categoryСacheInfo);
      cache.Init = true;
    }
  }

  private void ReadStrFile(BinaryReader br, BaseCache cache, int category)
  {
    CategoryСacheInfo categoryСacheInfo = new CategoryСacheInfo();
    long num = 0;
    try
    {
      long length = br.BaseStream.Length;
      while (br.BaseStream.Position < length)
      {
        num = br.BaseStream.Position;
        string empty1 = string.Empty;
        int count1 = (int) br.ReadInt16();
        if (count1 > 0)
        {
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.Append(br.ReadChars(count1));
          empty1 = stringBuilder.ToString();
        }
        long position = br.BaseStream.Position;
        long newKey = br.ReadInt64();
        int count2 = br.ReadInt32();
        string empty2 = string.Empty;
        if (count2 > 0)
        {
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.Append(br.ReadChars(count2));
          empty2 = stringBuilder.ToString();
        }
        ITagImportObject tag = (ITagImportObject) null;
        int count3 = br.ReadInt32();
        if (count3 > 0)
        {
          int classID = (int) br.ReadInt16();
          byte[] s = br.ReadBytes(count3);
          tag = TagImportObjectHelper.GetImportObject(classID);
          tag?.Load(s);
        }
        categoryСacheInfo.Add((object) empty1, new long[2]
        {
          -1L,
          position
        });
        cache.AddValue((object) empty1, newKey, empty2, tag);
      }
    }
    catch (Exception ex)
    {
      if (br != null)
        br.BaseStream.Position = num;
      ExceptionHelper.ExceptionService.ShowException(ex);
    }
    this._keys.Add(category, categoryСacheInfo);
    cache.Init = true;
  }

  public FileManager()
  {
    this._streams = new Dictionary<int, FileStream>();
    this._type = new Dictionary<int, RecordType>();
    this._keys = new Dictionary<int, CategoryСacheInfo>();
  }

  private string GetFileName(int category)
  {
    return Path.Combine(CacheHelper.CacheFolder, Enum.IsDefined(typeof (ImportingCategory), (object) category) ? $"{(Enum) (ImportingCategory) category}{".dat"}" : $"cache_{category}{".dat"}");
  }

  public BaseCache ReadCache(int category)
  {
    string fileName = this.GetFileName(category);
    BaseCache cache = new BaseCache();
    RecordType recordType = RecordType.None;
    FileStream fileStream1;
    if (File.Exists(fileName))
    {
      FileInfo fileInfo = new FileInfo(fileName);
      FileStream fileStream2 = new FileStream(fileName, FileMode.Open, FileSystemRights.Modify, FileShare.ReadWrite, this._defaultBufferSize, FileOptions.None);
      fileStream2.Position = 0L;
      fileStream1 = fileStream2;
      if (fileInfo.Length > 0L)
      {
        BinaryReader br = new BinaryReader((Stream) fileStream1, Encoding.Unicode);
        recordType = (RecordType) br.ReadInt32();
        switch (recordType)
        {
          case RecordType.Int:
            cache = (BaseCache) new IntKeyCache();
            this.ReadFile(br, cache, category);
            break;
          case RecordType.Int64:
            cache = (BaseCache) new LongKeyCache();
            this.ReadFile(br, cache, category);
            break;
          case RecordType.Char:
            cache = (BaseCache) new CharKeyCache();
            this.ReadFile(br, cache, category);
            break;
          case RecordType.String:
            cache = (BaseCache) new StringKeyCache();
            this.ReadStrFile(br, cache, category);
            break;
        }
      }
    }
    else
    {
      fileStream1 = new FileStream(fileName, FileMode.Create, FileSystemRights.Modify, FileShare.ReadWrite, this._defaultBufferSize, FileOptions.None);
      if (recordType != RecordType.None)
      {
        BinaryWriter binaryWriter = new BinaryWriter((Stream) fileStream1, Encoding.Unicode);
        binaryWriter.Write(Convert.ToInt32((object) recordType));
        binaryWriter.Flush();
        fileStream1.Flush();
      }
      this._keys.Add(category, new CategoryСacheInfo());
    }
    this._streams.Add(category, fileStream1);
    this._type.Add(category, recordType);
    return cache;
  }

  public void AddValue(
    int category,
    object oldKey,
    long newKey,
    string caption,
    ITagImportObject tag)
  {
    FileStream stream = this._streams[category];
    if (stream == null)
      return;
    BinaryWriter binaryWriter = new BinaryWriter((Stream) stream, Encoding.Unicode);
    try
    {
      if (this._type[category] == RecordType.None)
      {
        this._type[category] = CacheHelper.GetRecordType(oldKey);
        binaryWriter.Write(Convert.ToInt32((object) this._type[category]));
      }
      if (!this._keys.ContainsKey(category))
        this._keys.Add(category, new CategoryСacheInfo());
      long position = binaryWriter.BaseStream.Position;
      switch (this._type[category])
      {
        case RecordType.Int:
          long int64_1 = Convert.ToInt64((int) oldKey);
          binaryWriter.Write(int64_1);
          this._keys[category].Add((object) int64_1, new long[2]
          {
            position,
            binaryWriter.BaseStream.Position
          });
          break;
        case RecordType.Char:
          long int64_2 = Convert.ToInt64((char) oldKey);
          binaryWriter.Write(int64_2);
          this._keys[category].Add((object) int64_2, new long[2]
          {
            position,
            binaryWriter.BaseStream.Position
          });
          break;
        case RecordType.String:
          string key1 = Convert.ToString(oldKey);
          binaryWriter.Write(Convert.ToInt16(key1.Length));
          binaryWriter.Write(key1.ToCharArray());
          this._keys[category].Add((object) key1, new long[2]
          {
            -1L,
            binaryWriter.BaseStream.Position
          });
          break;
        default:
          long key2 = (long) oldKey;
          binaryWriter.Write(key2);
          this._keys[category].Add((object) key2, new long[2]
          {
            position,
            binaryWriter.BaseStream.Position
          });
          break;
      }
      binaryWriter.Write(newKey);
      if (caption != string.Empty)
      {
        binaryWriter.Write(caption.Length);
        binaryWriter.Write(caption.ToCharArray());
      }
      else
        binaryWriter.Write(0);
      if (tag != null)
      {
        byte[] buffer = tag.Save();
        binaryWriter.Write(buffer.Length);
        binaryWriter.Write(tag.ClassID);
        binaryWriter.Write(buffer);
      }
      else
        binaryWriter.Write(0);
    }
    finally
    {
      binaryWriter.Flush();
      stream.Flush();
    }
  }

  public bool SetNewKey(int category, object oldKey, long newKey)
  {
    return this.SetKey(category, oldKey, newKey, 1);
  }

  public bool ClearValue(int category, object oldKey)
  {
    if (this._type[category] == RecordType.String)
      throw new Exception("Нельзя изменить строковый ключ");
    if (!this.SetKey(category, oldKey, long.MinValue, 0))
      return false;
    this._keys[category].Remove((object) Convert.ToInt64(oldKey));
    return true;
  }

  private bool SetKey(int category, object oldKey, long newKey, int index)
  {
    long num = -1;
    if (this._keys.ContainsKey(category))
    {
      switch (this._type[category])
      {
        case RecordType.String:
          string key = Convert.ToString(oldKey);
          if (this._keys[category].ContainsKey((object) key))
          {
            num = this._keys[category][(object) key][index];
            break;
          }
          break;
        default:
          long int64 = Convert.ToInt64(oldKey);
          if (this._keys[category].ContainsKey((object) int64))
          {
            num = this._keys[category][(object) int64][index];
            break;
          }
          break;
      }
      if (num != -1L)
      {
        try
        {
          if (this._streams.ContainsKey(category))
          {
            if (this._streams[category] != null)
            {
              long position = this._streams[category].Position;
              this._streams[category].Position = num;
              BinaryWriter binaryWriter = new BinaryWriter((Stream) this._streams[category], Encoding.Unicode);
              try
              {
                binaryWriter.Write(newKey);
              }
              finally
              {
                binaryWriter.Flush();
                this._streams[category].Flush();
                this._streams[category].Position = position;
              }
              return true;
            }
          }
        }
        catch
        {
          return false;
        }
      }
    }
    return false;
  }

  public void DeleteCategory(params int[] categories)
  {
    this.CloseCategory(categories);
    foreach (int category in categories)
      this.DeleteFiles(this.GetFileName(category));
  }

  public void CloseCategory(params int[] categories)
  {
    foreach (int category in categories)
    {
      if (this._streams.ContainsKey(category))
      {
        this.CloseStream(this._streams[category]);
        this._streams.Remove(category);
        this._type.Remove(category);
        if (this._keys.ContainsKey(category))
        {
          this._keys[category] = new CategoryСacheInfo();
          this._keys.Remove(category);
        }
      }
    }
  }

  public void Close()
  {
    if (this._streams == null)
      return;
    foreach (FileStream stream in this._streams.Values)
      this.CloseStream(stream);
    foreach (int key in new List<int>((IEnumerable<int>) this._streams.Keys))
      this._streams[key] = (FileStream) null;
  }
}
