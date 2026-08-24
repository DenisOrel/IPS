// Decompiled with JetBrains decompiler
// Type: InventorApprentice.SolutionNatureEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("5DF86046-6B16-11D3-B794-0060B0F159EF")]
public enum SolutionNatureEnum
{
  kUnknownSolutionNature,
  kUniqueSolution,
  kDistinctlyManySolutions,
  kInfinitelyManySolutions,
  kNoSolution,
}
