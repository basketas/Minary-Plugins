﻿namespace Minary.Plugin.Main
{
  using Minary.Plugin.Main.HttpAccounts.DataTypes;
  using MinaryLib;
  using MinaryLib.DataTypes;
  using System;
  using System.Collections.Generic;
  using System.Threading;


  public partial class Plugin_HttpAccounts
  {

    #region IPlugin Member

    public PluginProperties Config { get { return this.pluginProperties; } set { this.pluginProperties = value; } }


    /// <summary>
    ///
    /// </summary>
    public delegate void OnInitDelegate();
    public void OnInit()
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new OnInitDelegate(this.OnInit), new object[] { });
        return;
      }

      // Plugin initialisation
      this.pluginProperties.HostApplication.Register(this);
      this.pluginProperties.HostApplication.ReportPluginSetStatus(this, MinaryLib.Plugin.Status.NotRunning);

      this.SetGuiActive();
      this.t_GuiUpdate.Start();

      try
      {
        this.infrastructureLayer.OnInit();
      }
      catch (Exception ex)
      {
        this.pluginProperties.HostApplication.LogMessage("{0}: {1}", this.Config.PluginName, ex.Message);
      }
    }


    /// <summary>
    ///
    /// </summary>
    public delegate void OnStartUpdateDelegate();
    public void OnStartUpdate()
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new OnStartUpdateDelegate(this.OnStartUpdate), new object[] { });
        return;
      }

      Thread updateProcessThread = new Thread(new ThreadStart(this.SyncPatternFileFromServer));
      updateProcessThread.Start();
    }


    /// <summary>
    ///
    /// </summary>
    public delegate void OnStartAttackDelegate();
    public void OnStartAttack()
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new OnStartAttackDelegate(this.OnStartAttack), new object[] { });
        return;
      }

      // Get active HTTP Account patterns
      lock (this)
      {
        try
        {
          this.accountPatterns = this.manageHttpAccountsPresentationLayer.GetActiveAuthenticationPatterns();
        }
        catch (Exception ex)
        {
          this.pluginProperties.HostApplication.LogMessage("{0}: {1}", this.Config.PluginName, ex.Message);
        }
      }

      this.pluginProperties.HostApplication.ReportPluginSetStatus(this, MinaryLib.Plugin.Status.Running);
      this.SetGuiInactive();
    }


    /// <summary>
    ///
    /// </summary>
    public delegate void OnStopAttackDelegate();
    public void OnStopAttack()
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new OnStopAttackDelegate(this.OnStopAttack), new object[] { });
        return;
      }

      this.SetGuiActive();
      this.pluginProperties.HostApplication.ReportPluginSetStatus(this, MinaryLib.Plugin.Status.NotRunning);
    }


    /// <summary>
    /// New input data arrived (Not relevant in this plugin)
    /// </summary>
    /// <param name="data"></param>
    public delegate void OnNewDataDelegate(string data);
    public void OnNewData(string data)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new OnNewDataDelegate(this.OnNewData), new object[] { data });
        return;
      }

      lock (this)
      {
        if (this.dataBatch != null && data != null && data.Length > 0)
        {
          this.dataBatch.Add(data);
        }
      }
    }


    /// <summary>
    ///
    /// </summary>
    public delegate void OnShutDownDelegate();
    public void OnShutDown()
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new OnShutDownDelegate(this.OnShutDown), new object[] { });
        return;
      }

      this.infrastructureLayer.OnStop();
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="targetList"></param>
    public void SetTargets(List<Tuple<string, string, string>> targetList)
    {
      this.targetList = targetList;
    }


    /// <summary>
    ///
    /// </summary>
    public delegate void OnResetPluginDelegate();
    public void OnResetPlugin()
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new OnResetPluginDelegate(this.OnResetPlugin), new object[] { });
        return;
      }

      this.ClearRecordList();
      this.SetGuiActive();
      this.pluginProperties.HostApplication.ReportPluginSetStatus(this, MinaryLib.Plugin.Status.NotRunning);

      this.infrastructureLayer.OnReset();
    }


    public delegate TemplatePluginData OnGetTemplateDataDelegate();
    public TemplatePluginData OnGetTemplateData()
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new OnGetTemplateDataDelegate(this.OnGetTemplateData), new object[] { });
        return null;
      }

      TemplatePluginData newTemplateData = new TemplatePluginData();

      // Configuration items
      newTemplateData.PluginConfigurationItems = this.infrastructureLayer.OnGetTemplateData(this.accountRecords);

      // Pattern items
      newTemplateData.PluginDataSearchPatternItems = this.manageHttpAccountsPresentationLayer.OnGetTemplateData();

      return newTemplateData;
    }


    public delegate void OnLoadTemplateDataDelegate(TemplatePluginData templateData);
    public void OnLoadTemplateData(TemplatePluginData templateData)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new OnLoadTemplateDataDelegate(this.OnLoadTemplateData), new object[] { templateData });
        return;
      }

      // Configuration items
      this.accountRecords.Clear();
      List<AccountRecord> loadedApplicationRecords = this.infrastructureLayer.OnLoadTemplateData(templateData);
      loadedApplicationRecords.ForEach(elem => this.accountRecords.Add(elem));

      // Pattern items
      this.manageHttpAccountsPresentationLayer.OnLoadTemplateData(templateData);
    }


    public delegate void OnUnloadTemplateDataDelegate();
    public void OnUnloadTemplateData()
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new OnUnloadTemplateDataDelegate(this.OnUnloadTemplateData), new object[] { });
        return;
      }

      // Remove all template data
      this.infrastructureLayer.OnUnoadTemplateData();

      // Reload local and remote pattern files
      this.manageHttpAccountsPresentationLayer.LocalPatternsEnabled = true;
      this.manageHttpAccountsPresentationLayer.RemotePatternsEnabled = true;
      this.manageHttpAccountsTaskLayer.ReadAccountsPatterns();
    }

    #endregion

  }
}