// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Mbom.MbomClientService
// Assembly: Intermech.Mbom, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 13559C9A-4DBC-479B-BA71-AFEA0247DEC7
// Assembly location: D:\IPS\Client\Intermech.Mbom.dll
// XML documentation location: D:\IPS\Client\Intermech.Mbom.xml

using Intermech.Interfaces;
using Intermech.Navigator.Controls;
using Intermech.Search.UI;
using Intermech.Search.Utilities;
using System;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.Mbom;

public sealed class MbomClientService : IMbomClientService
{
  public void CreateMbom(long ebomVersionID)
  {
    if (ObjectHelper.IsUnknownObjectVersionID(ebomVersionID))
      throw new ArgumentException();
    long num1 = 0;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      num1 = ((IMbomServerService) sessionKeeper.Session.GetCustomService(typeof (IMbomServerService))).FindMbomForEbom(sessionKeeper.Session.SessionGUID, ebomVersionID);
    if (ObjectHelper.IsUnknownObjectVersionID(num1))
    {
      if (MessageBox.Show($"Создать ТЭСИ для ЭСИ '{this.GetObjectCaption(ebomVersionID)}'?", "Intermech Professional Solution", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
        return;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        using (NotificationContext.Create(sessionKeeper.Session, (object) this))
          num1 = ((IMbomServerService) sessionKeeper.Session.GetCustomService(typeof (IMbomServerService))).CreateMbom(sessionKeeper.Session.SessionGUID, ebomVersionID);
      }
      int num2 = (int) PropertiesWindow.Execute(SelectedItemsHelper.CreateSelectedItemsForObject(num1));
      using (MbomEditorForm mbomEditorForm = new MbomEditorForm())
      {
        mbomEditorForm.EbomVersionID = ebomVersionID;
        mbomEditorForm.MbomVersionID = num1;
        int num3 = (int) mbomEditorForm.ShowDialog();
      }
    }
    else
    {
      int num4 = (int) MessageBox.Show($"Для ЭСИ '{this.GetObjectCaption(ebomVersionID)}' уже существует ТЭСИ '{this.GetObjectCaption(num1)}'", "Intermech Professional Solution", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }
  }

  public void EditMbom(long mbomVersionID)
  {
    if (ObjectHelper.IsUnknownObjectVersionID(mbomVersionID))
      throw new ArgumentException();
    long versionID = 0;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      versionID = ((IMbomServerService) sessionKeeper.Session.GetCustomService(typeof (IMbomServerService))).FindEbomForMbom(sessionKeeper.Session.SessionGUID, mbomVersionID);
    if (ObjectHelper.IsUnknownObjectVersionID(versionID))
    {
      int num1 = (int) MessageBox.Show("Невозможно открыть редактор ТЭСИ. Не удалось найти сборочную единицу связанную с ТЭСИ.", "Ошибка редактирования ТЭСИ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }
    else
    {
      using (MbomEditorForm mbomEditorForm = new MbomEditorForm())
      {
        mbomEditorForm.EbomVersionID = versionID;
        mbomEditorForm.MbomVersionID = mbomVersionID;
        int num2 = (int) mbomEditorForm.ShowDialog();
      }
    }
  }

  private string GetObjectCaption(long objectVersionID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      return sessionKeeper.Session.GetObject(objectVersionID).Caption;
  }
}
