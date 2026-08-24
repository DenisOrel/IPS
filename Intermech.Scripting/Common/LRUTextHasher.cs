// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.LRUTextHasher
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Intermech.Scripting.Common;

internal sealed class LRUTextHasher : ITextHasher
{
  private ITextHasher baseAlgorithm;
  private int maxTotalLength;
  private StringComparer textComparer;
  private List<string> texts;
  private List<LRUTextHasher.TextHashDescriptor> hashDescriptors;
  private long totalLength;

  public LRUTextHasher(ITextHasher baseAlgorithm, int maxTotalLength)
  {
    if (baseAlgorithm == null)
      throw new ArgumentNullException(nameof (baseAlgorithm));
    if (maxTotalLength <= 0)
      throw new ArgumentOutOfRangeException(nameof (maxTotalLength));
    this.baseAlgorithm = baseAlgorithm;
    this.maxTotalLength = maxTotalLength;
    this.textComparer = StringComparer.Ordinal;
    this.texts = new List<string>(64 /*0x40*/);
    this.hashDescriptors = new List<LRUTextHasher.TextHashDescriptor>(64 /*0x40*/);
    this.totalLength = 0L;
  }

  /// <summary>Возвращает базовый алгоритм вычисления хэша.</summary>
  public ITextHasher BaseAlgorithm
  {
    [DebuggerStepThrough] get => this.baseAlgorithm;
  }

  /// <summary>
  /// Возвращает ограничение на максимальную общую длину текстов в кэше.
  /// При превышении этого ограничения текущий объект начинает удалять самые старые элементы из кэша.
  /// </summary>
  public int MaxTotalLength
  {
    [DebuggerStepThrough] get => this.maxTotalLength;
  }

  /// <summary>Возвращает количество элементов в кэше.</summary>
  public int CacheItemCount
  {
    [DebuggerStepThrough] get => this.hashDescriptors.Count;
  }

  public byte[] ComputeHash(string text)
  {
    switch (text)
    {
      case null:
        throw new ArgumentNullException(nameof (text));
      case "":
        return this.baseAlgorithm.ComputeHash(text);
      default:
        int index1 = this.texts.BinarySearch(text, (IComparer<string>) this.textComparer);
        byte[] hash;
        if (index1 >= 0)
        {
          LRUTextHasher.TextHashDescriptor hashDescriptor = this.hashDescriptors[index1];
          hash = hashDescriptor.Hash;
          hashDescriptor.Age = 0;
        }
        else
        {
          hash = this.baseAlgorithm.ComputeHash(text);
          int index2 = ~index1;
          this.texts.Insert(index2, text);
          this.hashDescriptors.Insert(index2, new LRUTextHasher.TextHashDescriptor(hash));
          this.totalLength += (long) text.Length;
          if (this.totalLength > (long) this.maxTotalLength)
            this.ReduceTotalLength();
        }
        for (int index3 = 0; index3 < this.hashDescriptors.Count; ++index3)
        {
          LRUTextHasher.TextHashDescriptor hashDescriptor = this.hashDescriptors[index3];
          if (hashDescriptor.Age < int.MaxValue)
            ++hashDescriptor.Age;
        }
        return hash;
    }
  }

  private void ReduceTotalLength()
  {
    int length;
    for (; this.totalLength > (long) this.maxTotalLength; this.totalLength -= (long) length)
    {
      int recentlyUsedItemIndex = this.FindLeastRecentlyUsedItemIndex();
      length = this.texts[recentlyUsedItemIndex].Length;
      this.texts.RemoveAt(recentlyUsedItemIndex);
      this.hashDescriptors.RemoveAt(recentlyUsedItemIndex);
    }
  }

  private int FindLeastRecentlyUsedItemIndex()
  {
    int num = -1;
    int recentlyUsedItemIndex = -1;
    for (int index = 0; index < this.hashDescriptors.Count; ++index)
    {
      int age = this.hashDescriptors[index].Age;
      if (age > num)
      {
        num = age;
        recentlyUsedItemIndex = index;
      }
    }
    return recentlyUsedItemIndex;
  }

  private sealed class TextHashDescriptor
  {
    public TextHashDescriptor(byte[] hash) => this.Hash = hash;

    public byte[] Hash { get; private set; }

    public int Age { get; set; }
  }
}
