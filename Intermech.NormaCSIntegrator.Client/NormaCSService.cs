// Decompiled with JetBrains decompiler
// Type: Intermech.NormaCSIntegrator.Client.NormaCSService
// Assembly: Intermech.NormaCSIntegrator.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: BC215C8E-677A-43E5-99F7-5ED2ECAA0726
// Assembly location: D:\IPS\Client\Intermech.NormaCSIntegrator.Client.dll

using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace Intermech.NormaCSIntegrator.Client;

public class NormaCSService : INormaCSService
{
  private NormaCSAPI.Application _normaApp;

  private NormaCSAPI.Application NormaApp
  {
    get
    {
      try
      {
        if (this._normaApp != null)
        {
          if (!this._normaApp.IsConnected)
            this._normaApp = (NormaCSAPI.Application) null;
        }
      }
      catch
      {
        this._normaApp = (NormaCSAPI.Application) null;
      }
      try
      {
        if (this._normaApp != null)
        {
          NormaCSAPI.Application normaApp = this._normaApp;
          return normaApp;
        }
        this._normaApp = (NormaCSAPI.Application) Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("A93DC555-5784-4CE4-AC9C-8F9CD822BBFF")));
        this._normaApp.Connect();
        int num = 0;
        while (!this._normaApp.IsConnected)
        {
          ++num;
          Thread.Sleep(500);
          if (num > 20)
            throw new Exception(LocalizationHolder.rm.GetString("NormaCSIntegrator_21"));
        }
        NormaCSAPI.Application normaApp1 = this._normaApp;
        return normaApp1;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(string.Format(LocalizationHolder.rm.GetString("NormaCSIntegrator_20"), (object) Environment.NewLine, (object) ex.Message));
        throw new AbortException();
      }
    }
  }

  public void Start() => this.NormaApp.Launch();

  public void FindByNumber(string text)
  {
    if (text.Equals(string.Empty))
    {
      int num = (int) MessageBox.Show(LocalizationHolder.rm.GetString("NormaCSIntegrator_8"), LocalizationHolder.rm.GetString("NormaCSIntegrator_9"), MessageBoxButtons.OK, MessageBoxIcon.None);
    }
    // ISSUE: reference to a compiler-generated method
    this.NormaApp.StartDocumentSearchByNumber(text);
  }

  public void FindByName(string searchText)
  {
    using (FindInNormaCSForm findInNormaCsForm = new FindInNormaCSForm(LocalizationHolder.rm.GetString("NormaCSIntegrator_16"), searchText))
    {
      int num = (int) findInNormaCsForm.ShowDialog();
      searchText = findInNormaCsForm.SearchText;
    }
    if (searchText == string.Empty)
      return;
    // ISSUE: reference to a compiler-generated method
    this.NormaApp.StartDocumentSearchByTitle(searchText);
  }

  public void FindByText(string searchText)
  {
    using (FindInNormaCSForm findInNormaCsForm = new FindInNormaCSForm(LocalizationHolder.rm.GetString("NormaCSIntegrator_17"), searchText))
    {
      int num = (int) findInNormaCsForm.ShowDialog();
      searchText = findInNormaCsForm.SearchText;
    }
    if (searchText == string.Empty)
      return;
    // ISSUE: reference to a compiler-generated method
    this.NormaApp.StartDocumentSearchByText(searchText);
  }
}
