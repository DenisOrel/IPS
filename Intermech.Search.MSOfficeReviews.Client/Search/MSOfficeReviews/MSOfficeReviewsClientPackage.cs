// Decompiled with JetBrains decompiler
// Type: Intermech.Search.MSOfficeReviews.MSOfficeReviewsClientPackage
// Assembly: Intermech.Search.MSOfficeReviews.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 4AB1E446-C278-4B7C-8A5E-DB94EF37D83B
// Assembly location: D:\IPS\Client\Intermech.Search.MSOfficeReviews.Client.dll

using Intermech.Interfaces.Client;
using Intermech.Interfaces.Plugins;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using Intermech.Search.Configuration;
using System;

#nullable disable
namespace Intermech.Search.MSOfficeReviews;

internal sealed class MSOfficeReviewsClientPackage : IPackage
{
  private MSOfficeReviewsModule _msOfficeReviewsModule = new MSOfficeReviewsModule();
  private MenuTemplateNode _reviewMenuTemplateNode;
  private MSOfficeReviewsCommandsProvider _msOfficeReviewsCommandsProvider = new MSOfficeReviewsCommandsProvider();

  public void Load(IServiceProvider serviceProvider)
  {
    this._msOfficeReviewsModule.Load();
    IConfigurationOptionInfoProvider optionInfoProvider = ServiceLocator.Get<IConfigurationOptionInfoProvider>();
    if (optionInfoProvider != null)
    {
      optionInfoProvider.RegisterEditor(MSOfficeReviewsConfigurationOptionKeys.ExcelReviewDocumentTypes, typeof (MSOfficeReviewObjectTypesEditor));
      optionInfoProvider.RegisterEditor(MSOfficeReviewsConfigurationOptionKeys.WordReviewDocumentTypes, typeof (MSOfficeReviewObjectTypesEditor));
    }
    ConfigurationPageHelper.CreateAndRegisterPages();
    if (ServicesManager.GetService(typeof (IFactory)) is IFactory service)
    {
      this._reviewMenuTemplateNode = new MenuTemplateNode()
      {
        Name = "Review",
        Text = "Рецензирование",
        Nodes = {
          new MenuTemplateNode()
          {
            Name = "Review.Edit",
            Text = "Редактировать"
          },
          new MenuTemplateNode()
          {
            Name = "Review.Show",
            Text = "Смотреть",
            Nodes = {
              new MenuTemplateNode()
              {
                Name = "Review.Show.All",
                Text = "Все"
              },
              new MenuTemplateNode()
              {
                Name = "Review.Show.Own",
                Text = "Свою"
              },
              new MenuTemplateNode()
              {
                Name = "Review.Show.Select",
                Text = "Выбрать..."
              }
            }
          },
          new MenuTemplateNode()
          {
            Name = "Review.ReplaceDocumentByReview",
            Text = "Заменить документ файлом рецензии"
          },
          new MenuTemplateNode()
          {
            Name = "Review.CreateDocumentFromReview",
            Text = "Создать версию документа из файла рецензии"
          },
          new MenuTemplateNode()
          {
            Name = "Review.Delete",
            Text = "Удалить"
          },
          new MenuTemplateNode()
          {
            Name = "Review.DeleteAll",
            Text = "Удалить все"
          },
          new MenuTemplateNode()
          {
            Name = "Review.Save",
            Text = "Сохранить"
          }
        }
      };
      service.ContextMenuTemplate.Nodes.Add(this._reviewMenuTemplateNode);
      service.AddCommandsProvider((ICommandsProvider) this._msOfficeReviewsCommandsProvider);
    }
    ServicesManager.AddService(typeof (IMSOfficeReviewsClientService), (object) new MSOfficeReviewsClientService());
  }

  public void Unload()
  {
    if (ServicesManager.GetService(typeof (IFactory)) is IFactory service)
    {
      service.ContextMenuTemplate.Nodes.Remove(this._reviewMenuTemplateNode);
      service.RemoveCommandsProvider((ICommandsProvider) this._msOfficeReviewsCommandsProvider);
    }
    ServicesManager.RemoveService(typeof (IMSOfficeReviewsClientService));
  }

  public string Name => "Рецензии MS Office";
}
