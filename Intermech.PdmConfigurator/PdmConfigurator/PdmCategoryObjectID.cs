// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.PdmCategoryObjectID
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

#nullable disable
namespace Intermech.PdmConfigurator;

public sealed class PdmCategoryObjectID
{
  public long CategoryID { get; private set; }

  public PdmCategoryObjectID(long categoryID) => this.CategoryID = categoryID;
}
