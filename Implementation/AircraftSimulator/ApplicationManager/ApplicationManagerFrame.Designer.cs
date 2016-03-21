using System.Collections.Generic;
using System;
using Common.Containers;

namespace ApplicationManager
{
    partial class ApplicationManagerFrame
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.toolTabControl = new System.Windows.Forms.TabControl();
            this.toolTabPages = new List<System.Windows.Forms.TabPage>();

            List<IObserver<IData>> subscribers = this.applicationManagerInstance.ToolsManagerCommunication.
                ManagerInstance.ToolsManagement.ConcreteObservableSubject.GetSubscribers();
            
            
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolTabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(12, 12);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.toolTabControl);
            this.splitContainer1.Size = new System.Drawing.Size(760, 537);
            this.splitContainer1.SplitterDistance = 253;
            this.splitContainer1.TabIndex = 1;
            // 
            // tabControl1
            // 
            //this.toolTabControl.Controls.Add(this.tabPage1);
            //this.toolTabControl.Controls.Add(this.tabPage2);
            this.toolTabControl.Location = new System.Drawing.Point(3, 3);
            this.toolTabControl.Name = "tabControl1";
            this.toolTabControl.SelectedIndex = 0;
            this.toolTabControl.Size = new System.Drawing.Size(500, 534);
            this.toolTabControl.TabIndex = 0;
            //// 
            //// tabPage1
            //// 
            //this.tabPage1.Location = new System.Drawing.Point(4, 22);
            //this.tabPage1.Name = "tabPage1";
            //this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            //this.tabPage1.Size = new System.Drawing.Size(492, 508);
            //this.tabPage1.TabIndex = 0;
            //this.tabPage1.Text = "Server";
            //this.tabPage1.UseVisualStyleBackColor = true;
            //// 
            //// tabPage2
            //// 
            //this.tabPage2.Location = new System.Drawing.Point(4, 22);
            //this.tabPage2.Name = "tabPage2";
            //this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            //this.tabPage2.Size = new System.Drawing.Size(492, 508);
            //this.tabPage2.TabIndex = 1;
            //this.tabPage2.Text = "tabPage2";
            //this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // ApplicationManagerFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.splitContainer1);
            this.Name = "ApplicationManagerFrame";
            this.Text = "Application Manager";
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolTabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl toolTabControl;
        private List<System.Windows.Forms.TabPage> toolTabPages;
    }
}

