﻿namespace Minary.Plugin.Main
{
  using Minary.Plugin.Main.DnsPoison.DataTypes;
  using MinaryLib;
  using MinaryLib.Plugin;
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.IO;
  using System.Windows.Forms;


  public partial class Plugin_DnsPoisoning : UserControl, IPlugin
  {

    #region MEMBERS

    private readonly int maxRowNum = 256;
    private readonly string dnsPoisoningConfigFilePath;

    private List<Tuple<string, string, string>> targetList;
    private BindingList<RecordDnsPoison> dnsPoisonRecords;
    private DnsPoison.Infrastructure.DnsPoisoning infrastructureLayer;
    private PluginProperties pluginProperties;
    private bool isUpToDate = false;

    #endregion


    #region PROPERTIES

    public Control PluginControl { get { return (this); } }

    #endregion


    #region PUBLIC

    public Plugin_DnsPoisoning(PluginProperties pluginProperties)
    {
      this.InitializeComponent();

      DataGridViewTextBoxColumn columnHostName = new DataGridViewTextBoxColumn();
      columnHostName.DataPropertyName = "HostName";
      columnHostName.Name = "HostName";
      columnHostName.HeaderText = "Host name";
      columnHostName.ReadOnly = true;
      columnHostName.Width = 296;
      this.dgv_Spoofing.Columns.Add(columnHostName);

      DataGridViewTextBoxColumn columnIpAddress = new DataGridViewTextBoxColumn();
      columnIpAddress.DataPropertyName = "IPAddress";
      columnIpAddress.Name = "IPAddress";
      columnIpAddress.HeaderText = "Spoofed IP address";
      columnIpAddress.ReadOnly = true;
      columnIpAddress.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dgv_Spoofing.Columns.Add(columnIpAddress);

      this.dnsPoisonRecords = new BindingList<RecordDnsPoison>();
      this.dgv_Spoofing.DataSource = this.dnsPoisonRecords;

      // Verify passed parameter(s)
      if (pluginProperties == null)
      {
        throw new Exception("Parameter PluginParameters is null");
      }

      if (pluginProperties.HostApplication == null)
      {
        throw new Exception("Parameter HostApplication is null");
      }

      if (pluginProperties.ApplicationBaseDir == null)
      {
        throw new Exception("Parameter ApplicationBaseDir is null");
      }

      if (pluginProperties.PluginBaseDir == null)
      {
        throw new Exception("Parameter PluginBaseDir is null");
      }

      if (pluginProperties.HostApplication.AttackServiceList == null ||
          pluginProperties.HostApplication.AttackServiceList.ContainsKey("ArpPoisoning") == false ||
          pluginProperties.HostApplication.AttackServiceList["ArpPoisoning"].SubModules == null ||
          pluginProperties.HostApplication.AttackServiceList["ArpPoisoning"].SubModules.ContainsKey("ArpPoisoning.DnsPoisoning") == false ||
          string.IsNullOrEmpty(pluginProperties.HostApplication.AttackServiceList["ArpPoisoning"].SubModules["ArpPoisoning.DnsPoisoning"].WorkingDirectory) ||
          string.IsNullOrEmpty(pluginProperties.HostApplication.AttackServiceList["ArpPoisoning"].SubModules["ArpPoisoning.DnsPoisoning"].ConfigFilePath))
      {
        throw new Exception("Attack services parameters are invalid");
      }

      // Plugin configuration
      this.pluginProperties = pluginProperties;

      this.pluginProperties.PluginName = "DNS Poisoning";
      this.pluginProperties.PluginType = "Active";
      this.pluginProperties.PluginDescription = "Poison client system DNS request and servers DNS responses.";
      this.pluginProperties.Ports = new Dictionary<int, MinaryLib.DataTypes.IpProtocols>();

      // Set DNS poisoning config file path
      this.dnsPoisoningConfigFilePath = Path.Combine(
                                                     pluginProperties.HostApplication.AttackServiceList["ArpPoisoning"].SubModules["ArpPoisoning.DnsPoisoning"].WorkingDirectory,
                                                     pluginProperties.HostApplication.AttackServiceList["ArpPoisoning"].SubModules["ArpPoisoning.DnsPoisoning"].ConfigFilePath);

      // Instantiate infrastructure layer
      this.infrastructureLayer = DnsPoison.Infrastructure.DnsPoisoning.GetInstance(this);

      // Initialize plugin environment
      this.infrastructureLayer.OnInit();
    }

    #endregion


    #region PRIVATE

    /// <summary>
    ///
    /// </summary>
    private delegate void SetGuiInactiveDelegate();
    private void SetGuiInactive()
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new SetGuiInactiveDelegate(this.SetGuiInactive), new object[] { });
        return;
      }

      this.tb_Address.Enabled = false;
      this.tb_Host.Enabled = false;
      this.bt_Add.Enabled = false;
      this.cms_DnsPoison.Enabled = false;
      this.tsmi_Delete.Enabled = false;
      this.tsmi_ClearList.Enabled = false;
    }


    /// <summary>
    ///
    /// </summary>
    private delegate void SetGuiActiveDelegate();
    private void SetGuiActive()
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new SetGuiActiveDelegate(this.SetGuiActive), new object[] { });
        return;
      }

      this.tb_Address.Enabled = true;
      this.tb_Host.Enabled = true;
      this.bt_Add.Enabled = true;
      this.cms_DnsPoison.Enabled = true;
      this.tsmi_Delete.Enabled = true;
      this.tsmi_ClearList.Enabled = true;
    }


    /// <summary>
    /// Poisoning exited.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnDNSHijackExited(object sender, System.EventArgs e)
    {
      this.SetGuiActive();
    }

    #endregion

  }
}