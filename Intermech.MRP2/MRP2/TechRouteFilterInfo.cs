// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.TechRouteFilterInfo
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Interfaces;

#nullable disable
namespace Intermech.MRP2;

/// <summary>
/// Класс хелпер, который отражает текущую входимости элемента дерева навигатора
/// </summary>
internal class TechRouteFilterInfo
{
  public TechRouteFilterInfo(
    IUserSession session,
    long aProdListObjectId,
    long aProdListId,
    long aExitAsmObjectId,
    long aAsmObjectId)
  {
    this.ProdListObjectId = aProdListObjectId;
    this.ProdListId = aProdListId;
    this.ExitAsmObjectId = aExitAsmObjectId;
    this.AsmObjectId = aAsmObjectId;
    IDBAttribute objectAttribute1 = session.GetObjectAttribute(aAsmObjectId, (object) MRP2Consts.attrIdPKDSE_Id, false, false);
    this.AsmPKDSE = objectAttribute1 != null ? objectAttribute1.AsString : "";
    IDBAttribute objectAttribute2 = session.GetObjectAttribute(aExitAsmObjectId, (object) MRP2Consts.attrIdPKDSE_Id, false, false);
    this.ExitAsmPKDSE = objectAttribute2 != null ? objectAttribute2.AsString : "";
  }

  public long AsmObjectId { get; }

  public long ExitAsmObjectId { get; }

  public long ProdListId { get; }

  public long ProdListObjectId { get; }

  public string AsmPKDSE { get; }

  public string ExitAsmPKDSE { get; }
}
