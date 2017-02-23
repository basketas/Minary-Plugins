﻿namespace Minary.Plugin.Main
{
  public partial class Plugin_HttpHostMapping
  {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }

      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      this.tb_MappedHost = new System.Windows.Forms.TextBox();
      this.l_MappedHost = new System.Windows.Forms.Label();
      this.dgv_HostMapping = new System.Windows.Forms.DataGridView();
      this.tb_RequestedHost = new System.Windows.Forms.TextBox();
      this.bt_AddRecord = new System.Windows.Forms.Button();
      this.l_RequestedHost = new System.Windows.Forms.Label();
      this.cms_HostMapping = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.deleteEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.clearListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      ((System.ComponentModel.ISupportInitialize)(this.dgv_HostMapping)).BeginInit();
      this.cms_HostMapping.SuspendLayout();
      this.SuspendLayout();
      // 
      // tb_MappedHost
      // 
      this.tb_MappedHost.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.tb_MappedHost.Location = new System.Drawing.Point(426, 14);
      this.tb_MappedHost.Name = "tb_MappedHost";
      this.tb_MappedHost.Size = new System.Drawing.Size(184, 20);
      this.tb_MappedHost.TabIndex = 10;
      this.tb_MappedHost.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TB_AddRecord_KeyDown);
      // 
      // l_MappedHost
      // 
      this.l_MappedHost.AutoSize = true;
      this.l_MappedHost.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.l_MappedHost.Location = new System.Drawing.Point(336, 17);
      this.l_MappedHost.Name = "l_MappedHost";
      this.l_MappedHost.Size = new System.Drawing.Size(80, 13);
      this.l_MappedHost.TabIndex = 6;
      this.l_MappedHost.Text = "Mapped host";
      // 
      // dgv_HostMapping
      // 
      this.dgv_HostMapping.AllowUserToAddRows = false;
      this.dgv_HostMapping.AllowUserToDeleteRows = false;
      this.dgv_HostMapping.AllowUserToResizeColumns = false;
      this.dgv_HostMapping.AllowUserToResizeRows = false;
      this.dgv_HostMapping.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.dgv_HostMapping.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.dgv_HostMapping.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
      this.dgv_HostMapping.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
      this.dgv_HostMapping.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgv_HostMapping.Location = new System.Drawing.Point(17, 44);
      this.dgv_HostMapping.MultiSelect = false;
      this.dgv_HostMapping.Name = "dgv_HostMapping";
      this.dgv_HostMapping.RowHeadersVisible = false;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
      this.dgv_HostMapping.RowsDefaultCellStyle = dataGridViewCellStyle1;
      this.dgv_HostMapping.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.dgv_HostMapping.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.dgv_HostMapping.Size = new System.Drawing.Size(933, 313);
      this.dgv_HostMapping.TabIndex = 7;
      this.dgv_HostMapping.TabStop = false;
      this.dgv_HostMapping.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DGV_Spoofing_MouseDown);
      this.dgv_HostMapping.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DGV_Spoofing_MouseUp);
      // 
      // tb_RequestedHost
      // 
      this.tb_RequestedHost.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.tb_RequestedHost.Location = new System.Drawing.Point(126, 16);
      this.tb_RequestedHost.Name = "tb_RequestedHost";
      this.tb_RequestedHost.Size = new System.Drawing.Size(185, 20);
      this.tb_RequestedHost.TabIndex = 9;
      this.tb_RequestedHost.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TB_AddRecord_KeyDown);
      // 
      // bt_AddRecord
      // 
      this.bt_AddRecord.Location = new System.Drawing.Point(642, 14);
      this.bt_AddRecord.Name = "bt_AddRecord";
      this.bt_AddRecord.Size = new System.Drawing.Size(23, 21);
      this.bt_AddRecord.TabIndex = 11;
      this.bt_AddRecord.Text = "+";
      this.bt_AddRecord.UseVisualStyleBackColor = true;
      this.bt_AddRecord.Click += new System.EventHandler(this.BT_AddRecord_Click);
      // 
      // l_RequestedHost
      // 
      this.l_RequestedHost.AutoSize = true;
      this.l_RequestedHost.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.l_RequestedHost.Location = new System.Drawing.Point(23, 19);
      this.l_RequestedHost.Name = "l_RequestedHost";
      this.l_RequestedHost.Size = new System.Drawing.Size(96, 13);
      this.l_RequestedHost.TabIndex = 8;
      this.l_RequestedHost.Text = "Requested host";
      // 
      // cms_HostMapping
      // 
      this.cms_HostMapping.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteEntryToolStripMenuItem,
            this.clearListToolStripMenuItem});
      this.cms_HostMapping.Name = "CMS_HostMapping";
      this.cms_HostMapping.Size = new System.Drawing.Size(138, 48);
      // 
      // deleteEntryToolStripMenuItem
      // 
      this.deleteEntryToolStripMenuItem.Name = "deleteEntryToolStripMenuItem";
      this.deleteEntryToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
      this.deleteEntryToolStripMenuItem.Text = "Delete entry";
      this.deleteEntryToolStripMenuItem.Click += new System.EventHandler(this.DeleteRuleToolStripMenuItem_Click);
      // 
      // clearListToolStripMenuItem
      // 
      this.clearListToolStripMenuItem.Name = "clearListToolStripMenuItem";
      this.clearListToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
      this.clearListToolStripMenuItem.Text = "Clear list";
      this.clearListToolStripMenuItem.Click += new System.EventHandler(this.TSMI_Clear_Click);
      // 
      // Plugin_HttpHostMapping
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.tb_MappedHost);
      this.Controls.Add(this.l_MappedHost);
      this.Controls.Add(this.dgv_HostMapping);
      this.Controls.Add(this.tb_RequestedHost);
      this.Controls.Add(this.bt_AddRecord);
      this.Controls.Add(this.l_RequestedHost);
      this.Name = "Plugin_HttpHostMapping";
      this.Size = new System.Drawing.Size(996, 368);
      ((System.ComponentModel.ISupportInitialize)(this.dgv_HostMapping)).EndInit();
      this.cms_HostMapping.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox tb_MappedHost;
    private System.Windows.Forms.Label l_MappedHost;
    private System.Windows.Forms.DataGridView dgv_HostMapping;
    private System.Windows.Forms.TextBox tb_RequestedHost;
    private System.Windows.Forms.Button bt_AddRecord;
    private System.Windows.Forms.Label l_RequestedHost;
    private System.Windows.Forms.ContextMenuStrip cms_HostMapping;
    private System.Windows.Forms.ToolStripMenuItem deleteEntryToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem clearListToolStripMenuItem;
  }
}
