// Decompiled with JetBrains decompiler
// Type: Intermech.BugReports.ActionsInfo
// Assembly: Intermech.BugReports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 16F80F46-2B9D-4747-9BFD-4CC209192F4E
// Assembly location: D:\IPS\Client\Intermech.BugReports.dll

using Intermech.Client.Core.FormDesigner.Controls;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Intermech.BugReports;

public static class ActionsInfo
{
  public static FormDesignerAction FixBugAction = new FormDesignerAction(new Guid("6AB76640-8428-4521-BA7C-654C6CE3ED08"), "Исправил ошибку");
  public static FormDesignerAction RejectBugAction = new FormDesignerAction(new Guid("16FE47D2-5118-40c8-AB80-CD342DFEAD2C"), "Отклонил ошибку");
  public static FormDesignerAction CheckBugAction = new FormDesignerAction(new Guid("720F4B95-6E3C-418f-A4C1-F2F227A7B213"), "Проверил ошибку");

  public static Dictionary<int, List<IAttributeEditor>> GetAttributeEditors(
    List<int> IDs,
    DesForm desForm)
  {
    return desForm.GetEditors(desForm.Info.ElementIdentifier).Where<KeyValuePair<int, List<IAttributeEditor>>>((Func<KeyValuePair<int, List<IAttributeEditor>>, bool>) (x => IDs.Contains(x.Key))).ToDictionary<KeyValuePair<int, List<IAttributeEditor>>, int, List<IAttributeEditor>>((Func<KeyValuePair<int, List<IAttributeEditor>>, int>) (x => x.Key), (Func<KeyValuePair<int, List<IAttributeEditor>>, List<IAttributeEditor>>) (x => x.Value));
  }
}
