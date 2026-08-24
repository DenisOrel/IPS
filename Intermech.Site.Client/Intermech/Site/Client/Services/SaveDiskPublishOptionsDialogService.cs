// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.Services.SaveDiskPublishOptionsDialogService
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces.WebPortal;
using System;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client.Services;

internal sealed class SaveDiskPublishOptionsDialogService : ISaveDiskPublishOptionsDialogService
{
  private readonly OpenFileDialog _openDialog;
  private readonly SaveFileDialog _saveDialog;

  public SaveDiskPublishOptionsDialogService(string directory)
  {
    OpenFileDialog openFileDialog = new OpenFileDialog();
    openFileDialog.Title = "Выберите файл с настройками публикации для открытия";
    openFileDialog.Filter = "Файлы с настройками публикации|*.po|Все файлы|*.*";
    openFileDialog.SupportMultiDottedExtensions = true;
    openFileDialog.Multiselect = false;
    openFileDialog.InitialDirectory = directory;
    openFileDialog.RestoreDirectory = true;
    this._openDialog = openFileDialog;
    SaveFileDialog saveFileDialog = new SaveFileDialog();
    saveFileDialog.Title = "Куда сохранить настройки публикации?";
    saveFileDialog.Filter = "Файлы с настройками публикации|*.po|Все файлы|*.*";
    saveFileDialog.SupportMultiDottedExtensions = true;
    saveFileDialog.InitialDirectory = directory;
    saveFileDialog.RestoreDirectory = true;
    this._saveDialog = saveFileDialog;
  }

  public ExtendedPublishOptions LoadOptions()
  {
    if (this._openDialog.ShowDialog() != DialogResult.OK || !File.Exists(this._openDialog.FileName))
      return (ExtendedPublishOptions) null;
    using (FileStream fileStream = File.OpenRead(this._openDialog.FileName))
    {
      byte[] numArray = new byte[fileStream.Length];
      fileStream.Read(numArray, 0, Convert.ToInt32(fileStream.Length));
      return PublishOptionsHelper.Deserialize(numArray);
    }
  }

  public bool SaveOptions(ExtendedPublishOptions publishOptions, bool showSuccessMessage)
  {
    if (this._saveDialog.ShowDialog() != DialogResult.OK)
      return false;
    using (Stream stream = this._saveDialog.OpenFile())
    {
      byte[] buffer = PublishOptionsHelper.Serialize(publishOptions);
      stream.Write(buffer, 0, buffer.Length);
    }
    if (showSuccessMessage)
    {
      int num = (int) MessageBox.Show("Настройки публикации успешно сохранены!", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }
    return true;
  }
}
