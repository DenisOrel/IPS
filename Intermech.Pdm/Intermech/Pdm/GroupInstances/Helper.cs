// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.GroupInstances.Helper
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using System;

#nullable disable
namespace Intermech.Pdm.GroupInstances;

internal class Helper
{
  internal static void AddNumInstance(
    IUserSession session,
    IDBObject obj,
    int attributeGroupInstanceID,
    Guid numGroupInstance)
  {
    bool flag = false;
    if (obj.ObjectModifyMode == ObjectModifyModes.CreateVersion)
      throw new Exception("На текущем шаге ЖЦ модифицировать объект можно только через выпуск новой версии.");
    if (obj.ObjectModifyMode == ObjectModifyModes.CantModify)
      throw new Exception("На текущем шаге ЖЦ нельзя модифицировать объект.");
    if (obj.ObjectModifyMode == ObjectModifyModes.Checkout && obj.CheckoutBy != session.UserID)
    {
      obj = obj.CheckOut();
      flag = true;
    }
    IDBAttribute dbAttribute = obj.GetAttributeByID(attributeGroupInstanceID);
    if (dbAttribute == null)
      dbAttribute = obj.Attributes.AddAttribute(attributeGroupInstanceID, false);
    else if (!dbAttribute.IsNull && !dbAttribute.Value.Equals((object) numGroupInstance))
      throw new Exception($"Операция недопустима, т.к. {obj.NameInMessages} является исполнением другого изделия.");
    dbAttribute.Value = (object) numGroupInstance;
    if (!flag)
      return;
    obj.CheckIn();
  }
}
