// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.Substitutes.ISubstitutesClientService
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

#nullable disable
namespace Intermech.Search.Pdm.Substitutes;

public interface ISubstitutesClientService
{
  void ActualizeSubstitute(long relationID);

  void SaveSubstitutes(SaveSubstitutesParams @params);

  void RemoveSubstitutes(long projectVersionID, int relationTypeID);
}
