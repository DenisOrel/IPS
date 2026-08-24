// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Mbom.IMbomClientService
// Assembly: Intermech.Mbom, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 13559C9A-4DBC-479B-BA71-AFEA0247DEC7
// Assembly location: D:\IPS\Client\Intermech.Mbom.dll
// XML documentation location: D:\IPS\Client\Intermech.Mbom.xml

#nullable disable
namespace Intermech.Search.Mbom;

public interface IMbomClientService
{
  void CreateMbom(long ebomVersionID);

  void EditMbom(long mbomVersionID);
}
