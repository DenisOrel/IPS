// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.ComponentSelection.ContextMenu
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Navigator.ContextMenu;

#nullable disable
namespace Intermech.Pdm.ComponentSelection;

internal sealed class ContextMenu
{
  public static string cmdComponentSelection = "PDM.ComponentSelection";
  public static string menuComponentSelection = "Подборный компонент";
  public static string cmdCreateNew = "PDM.ComponentSelection.CreateNew";
  public static string menuCreateNew = "Создать новый объект";
  public static string cmdAddExisting = "PDM.ComponentSelection.AddExisting";
  public static string menuAddExisting = "Добавить существующий объект";
  public static string cmdAddFromImbase = "PDM.ComponentSelection.AddFromImbase";
  public static string menuAddFromImbase = "Добавить из Imbase";
  public static string cmdReset = "PDM.ComponentSelection.Reset";
  public static string menuReset = "Сброс подбора";

  public static void CreateTemplate(MenuTemplate template)
  {
    template.Nodes.Add(new MenuTemplateNode(Intermech.Pdm.ComponentSelection.ContextMenu.cmdComponentSelection, Intermech.Pdm.ComponentSelection.ContextMenu.menuComponentSelection, -1, 400, 10)
    {
      Nodes = {
        new MenuTemplateNode(Intermech.Pdm.ComponentSelection.ContextMenu.cmdCreateNew, Intermech.Pdm.ComponentSelection.ContextMenu.menuCreateNew, -1, 10, 10),
        new MenuTemplateNode(Intermech.Pdm.ComponentSelection.ContextMenu.cmdAddExisting, Intermech.Pdm.ComponentSelection.ContextMenu.menuAddExisting, -1, 10, 20),
        new MenuTemplateNode(Intermech.Pdm.ComponentSelection.ContextMenu.cmdAddFromImbase, Intermech.Pdm.ComponentSelection.ContextMenu.menuAddFromImbase, -1, 10, 30),
        new MenuTemplateNode(Intermech.Pdm.ComponentSelection.ContextMenu.cmdReset, Intermech.Pdm.ComponentSelection.ContextMenu.menuReset, -1, 10, 40)
      }
    });
  }
}
