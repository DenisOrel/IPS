// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor.OverloadInsightProvider
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using ICSharpCode.AvalonEdit.CodeCompletion;
using Intermech.UI;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Views.AvalonCodeEditor;

internal sealed class OverloadInsightProvider : ViewModel, IOverloadProvider, INotifyPropertyChanged
{
  private List<OverloadInsightItem> items;
  private int selectedIndex;

  public OverloadInsightProvider(List<OverloadInsightItem> items)
  {
    this.selectedIndex = 0;
    this.items = items;
  }

  public int Count => this.items.Count;

  public object CurrentContent => (object) this.items[this.selectedIndex].Description;

  public object CurrentHeader => (object) this.items[this.selectedIndex].Text;

  public string CurrentIndexText => $"{this.selectedIndex + 1} of {this.Count}";

  public int SelectedIndex
  {
    get => this.selectedIndex;
    set
    {
      this.selectedIndex = value;
      if (this.selectedIndex >= this.items.Count)
        this.selectedIndex = this.items.Count - 1;
      if (this.selectedIndex < 0)
        this.selectedIndex = 0;
      this.RaisePropertyChanged(nameof (SelectedIndex));
      this.RaisePropertyChanged("CurrentIndexText");
      this.RaisePropertyChanged("CurrentHeader");
      this.RaisePropertyChanged("CurrentContent");
    }
  }
}
