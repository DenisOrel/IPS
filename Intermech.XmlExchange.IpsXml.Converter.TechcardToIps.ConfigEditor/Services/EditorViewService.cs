// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.Services.EditorViewService
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 0BD7AB18-9725-4F3A-95EA-AF9537E2626A
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.Attributes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.Services;

internal sealed class EditorViewService
{
  private Dictionary<NodeType, IEditorViewController> viewControllers = new Dictionary<NodeType, IEditorViewController>();
  private IServiceProvider _services;
  private IEditorViewController _currentViewController;

  public EditorViewService(IServiceProvider services)
  {
    this._services = services;
    this.InitializeViewControllers();
  }

  public void EditConfig(
    BaseConfig targetConfig,
    BaseConfig targetParentConfig,
    Control parent,
    EventHandler<bool> onDataChanged)
  {
    NodeType? nodeType = targetConfig.GetType().GetCustomAttribute<ConfigNodeTypeAttribute>()?.NodeType;
    if (nodeType.HasValue && this.viewControllers.TryGetValue(nodeType.Value, out this._currentViewController))
    {
      this._currentViewController.OnDataChanged += onDataChanged;
      this._currentViewController.TargetConfig = targetConfig;
      this._currentViewController.TargetParentConfig = targetParentConfig;
      this._currentViewController.ShowOnParent(parent);
    }
    else
      parent.Controls.Clear();
  }

  public bool ApplyChanges()
  {
    return this._currentViewController != null && this._currentViewController.ApplyChanges();
  }

  public void CancelChanges() => this._currentViewController.CancelChanges();

  private void InitializeViewControllers()
  {
    ((IEnumerable<Type>) Assembly.GetExecutingAssembly().GetTypes()).ToList<Type>().ForEach((Action<Type>) (type =>
    {
      IEnumerable<ConfigNodeTypeAttribute> customAttributes = type.GetCustomAttributes<ConfigNodeTypeAttribute>();
      if (customAttributes == null || customAttributes.Count<ConfigNodeTypeAttribute>() == 0)
        return;
      if (!(Activator.CreateInstance(type, (object) this._services) is IEditorViewController instance2))
        return;
      foreach (ConfigNodeTypeAttribute nodeTypeAttribute in customAttributes)
        this.viewControllers[nodeTypeAttribute.NodeType] = instance2;
    }));
  }
}
