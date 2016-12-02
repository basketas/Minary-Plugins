﻿namespace Minary.Plugin.Main
{
  using Minary.Plugin.Main.DnsRequest.DataTypes;
  using Minary.Plugin.Main.DnsRequest.Infrastructure;
  using MinaryLib;
  using MinaryLib.DataTypes;
  using MinaryLib.Plugin;
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Text.RegularExpressions;
  using System.Windows.Forms;


  public partial class Plugin_DnsRequests : UserControl, IPlugin
  {

    #region MEMBERS

    private readonly int maxRowNum = 256;

    private List<Tuple<string, string, string>> targetList;
    private BindingList<DnsRequestRecord> dnsRequests;
    private List<string> dataBatch = new List<string>();
    private DnsRequests infrastructureLayer;
    private bool isUpToDate = false;
    private PluginProperties pluginProperties;

    #endregion


    #region PROPERTIES

    public Control PluginControl { get { return (this); } }

    #endregion


    #region PUBLIC

    /// <summary>
    /// Initializes a new instance of the <see cref="Plugin_DnsRequests"/> class.
    ///
    /// </summary>
    public Plugin_DnsRequests(PluginProperties pluginProperties)
    {
      this.InitializeComponent();

      DataGridViewTextBoxColumn columnSrcMac = new DataGridViewTextBoxColumn();
      columnSrcMac.DataPropertyName = "SrcMAC";
      columnSrcMac.Name = "SrcMAC";
      columnSrcMac.HeaderText = "MAC address";
      columnSrcMac.ReadOnly = true;
      columnSrcMac.Width = 140;
      this.dgv_DnsRequests.Columns.Add(columnSrcMac);
      
      DataGridViewTextBoxColumn columnSrcIp = new DataGridViewTextBoxColumn();
      columnSrcIp.DataPropertyName = "SrcIP";
      columnSrcIp.Name = "SrcIP";
      columnSrcIp.HeaderText = "Source IP";
      columnSrcIp.ReadOnly = true;
      columnSrcIp.Width = 120;
      this.dgv_DnsRequests.Columns.Add(columnSrcIp);
      
      DataGridViewTextBoxColumn columnTimestamp = new DataGridViewTextBoxColumn();
      columnTimestamp.DataPropertyName = "Timestamp";
      columnTimestamp.Name = "Timestamp";
      columnTimestamp.HeaderText = "Timestamp";
      columnTimestamp.ReadOnly = true;
      columnTimestamp.Width = 120;
      this.dgv_DnsRequests.Columns.Add(columnTimestamp);

      DataGridViewTextBoxColumn columnRemHost = new DataGridViewTextBoxColumn();
      columnRemHost.DataPropertyName = "DNSHostname";
      columnRemHost.Name = "DNSHostname";
      columnRemHost.HeaderText = "DNS request";
      columnRemHost.ReadOnly = true;
      columnRemHost.Width = 180;
      columnRemHost.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dgv_DnsRequests.Columns.Add(columnRemHost);

      DataGridViewTextBoxColumn columnPacketType = new DataGridViewTextBoxColumn();
      columnPacketType.DataPropertyName = "PacketType";
      columnPacketType.Name = "PacketType";
      columnPacketType.HeaderText = "Packet type";
      columnPacketType.ReadOnly = true;
      //// columnRemHost.Width = 280;
      columnPacketType.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dgv_DnsRequests.Columns.Add(columnPacketType);

      this.dnsRequests = new BindingList<DnsRequestRecord>();
      this.dgv_DnsRequests.DataSource = this.dnsRequests;
      
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

      // Plugin configuration
      this.t_GuiUpdate.Interval = 1000;
      this.pluginProperties = pluginProperties;

      this.pluginProperties.PluginName = "DNS requests";
      this.pluginProperties.PluginType = "Passive";
      this.pluginProperties.PluginDescription = "Eavesdrop client systems DNS requests.";
      this.pluginProperties.Ports = new Dictionary<int, IpProtocols>() { { 53, IpProtocols.Udp } };

      // Instantiate infrastructure layer
      this.infrastructureLayer = DnsRequests.GetInstance(this);

      // Initialize plugin environment
      this.infrastructureLayer.OnInit();
    }

    #endregion


    #region PRIVATE


    /// <summary>
    ///
    /// </summary>
    public void ProcessEntries()
    {
      if (this.dataBatch == null || this.dataBatch.Count <= 0)
      {
        return;
      }

      List<DnsRequestRecord> newRecords = new List<DnsRequestRecord>();
      List<string> newData;
      string[] splitter;
      string proto = string.Empty;
      string srcMac = string.Empty;
      string srcIp = string.Empty;
      string srcPort = string.Empty;
      string dstIP = string.Empty;
      string dstPort = string.Empty;
      string hostName = string.Empty;

      lock (this)
      {
        newData = new List<string>(this.dataBatch);
        this.dataBatch.Clear();
      }

      foreach (string tmpRecord in newData)
      {
        if (string.IsNullOrEmpty(tmpRecord))
        {
          continue;
        }

        try
        {
          if ((splitter = Regex.Split(tmpRecord, @"\|\|")).Length == 7)
          {
            proto = splitter[0];
            srcMac = splitter[1];
            srcIp = splitter[2];
            srcPort = splitter[3];
            dstIP = splitter[4];
            dstPort = splitter[5];
            hostName = splitter[6];

            if (dstPort != null && dstPort == "53")
            {
              newRecords.Add(new DnsRequestRecord(srcMac, srcIp, hostName, proto));
            }
          }
        }
        catch (Exception ex)
        {
          if (this.pluginProperties.HostApplication != null)
          {
            this.pluginProperties.HostApplication.LogMessage("{0}: {1}", this.Config.PluginName, ex.Message);
          }
        }
      }

      if (newRecords.Count > 0)
      {
        try
        {
          this.pluginProperties.HostApplication.LogMessage("Plugin_DnsRequest.ProcessEntries(): newRecords.Count:{0}", newRecords.Count);
          this.AddRecordsToDgv(newRecords);
        }
        catch (Exception ex)
        {
          if (this.pluginProperties.HostApplication != null)
          {
            this.pluginProperties.HostApplication.LogMessage("{0}: {1} (Host name: \"{2}\")", this.Config.PluginName, ex.Message, hostName);
          }
        }
      }
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="inputData"></param>
    /// <returns></returns>
    private bool CompareToFilter(string inputData)
    {
      bool retVal = false;

      if (Regex.Match(inputData, this.tb_Filter.Text, RegexOptions.IgnoreCase).Success)
      {
        retVal = true;
      }

      return (retVal);
    }


    /// <summary>
    ///
    /// </summary>
    private void UseFilter()
    {
      // TODO: Without this line we will get an exception :/ FIX IT!
      this.dgv_DnsRequests.CurrentCell = null;
      for (int i = 0; i < this.dgv_DnsRequests.Rows.Count; i++)
      {
        if (this.tb_Filter.Text.Length <= 0)
        {
          this.dgv_DnsRequests.Rows[i].Visible = true;
        }
        else
        {
          try
          {
            string selectedHostName = this.dgv_DnsRequests.Rows[i].Cells["DNSHostname"].Value.ToString();
            if (!Regex.Match(selectedHostName, Regex.Escape(this.tb_Filter.Text), RegexOptions.IgnoreCase).Success)
            {
              this.dgv_DnsRequests.Rows[i].Visible = false;
            }
            else
            {
              this.dgv_DnsRequests.Rows[i].Visible = true;
            }
          }
          catch (Exception ex)
          {
            this.pluginProperties.HostApplication.LogMessage("{0}: {1}", this.Config.PluginName, ex.Message);
          }
        }
      }

      this.dgv_DnsRequests.Refresh();
    }

    #endregion

  }
}