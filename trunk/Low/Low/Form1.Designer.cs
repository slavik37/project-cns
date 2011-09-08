namespace Low
{
    partial class Form1
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
          this.btStepFrw = new System.Windows.Forms.Button();
          this.trackBar1 = new System.Windows.Forms.TrackBar();
          this.tbValue = new System.Windows.Forms.TextBox();
          this.tbOptimazedVal = new System.Windows.Forms.TextBox();
          this.bug1 = new Low.Bug();
          this.cnsView1 = new Low.CNSView();
          ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
          this.SuspendLayout();
          // 
          // btStepFrw
          // 
          this.btStepFrw.Location = new System.Drawing.Point(12, 105);
          this.btStepFrw.Name = "btStepFrw";
          this.btStepFrw.Size = new System.Drawing.Size(93, 23);
          this.btStepFrw.TabIndex = 2;
          this.btStepFrw.Text = "Step>>";
          this.btStepFrw.UseVisualStyleBackColor = true;
          this.btStepFrw.Click += new System.EventHandler(this.btStepFrw_Click);
          // 
          // trackBar1
          // 
          this.trackBar1.Location = new System.Drawing.Point(12, 134);
          this.trackBar1.Maximum = 200;
          this.trackBar1.Name = "trackBar1";
          this.trackBar1.Orientation = System.Windows.Forms.Orientation.Vertical;
          this.trackBar1.Size = new System.Drawing.Size(42, 104);
          this.trackBar1.TabIndex = 4;
          this.trackBar1.TickFrequency = 20;
          this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
          // 
          // tbValue
          // 
          this.tbValue.Location = new System.Drawing.Point(12, 244);
          this.tbValue.Name = "tbValue";
          this.tbValue.Size = new System.Drawing.Size(93, 20);
          this.tbValue.TabIndex = 5;
          // 
          // tbOptimazedVal
          // 
          this.tbOptimazedVal.Location = new System.Drawing.Point(12, 270);
          this.tbOptimazedVal.Name = "tbOptimazedVal";
          this.tbOptimazedVal.Size = new System.Drawing.Size(93, 20);
          this.tbOptimazedVal.TabIndex = 6;
          // 
          // bug1
          // 
          this.bug1.Location = new System.Drawing.Point(12, 12);
          this.bug1.Name = "bug1";
          this.bug1.Size = new System.Drawing.Size(93, 87);
          this.bug1.TabIndex = 3;
          this.bug1.Text = "bug1";
          // 
          // cnsView1
          // 
          this.cnsView1.Location = new System.Drawing.Point(111, 12);
          this.cnsView1.MyCNS = null;
          this.cnsView1.Name = "cnsView1";
          this.cnsView1.Size = new System.Drawing.Size(567, 459);
          this.cnsView1.TabIndex = 1;
          this.cnsView1.Text = "cnsView1";
          // 
          // Form1
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.ClientSize = new System.Drawing.Size(690, 483);
          this.Controls.Add(this.tbOptimazedVal);
          this.Controls.Add(this.tbValue);
          this.Controls.Add(this.trackBar1);
          this.Controls.Add(this.bug1);
          this.Controls.Add(this.btStepFrw);
          this.Controls.Add(this.cnsView1);
          this.Name = "Form1";
          this.Text = "Form1";
          ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
          this.ResumeLayout(false);
          this.PerformLayout();

        }

        #endregion

        private CNSView cnsView1;
        private System.Windows.Forms.Button btStepFrw;
        private Bug bug1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.TextBox tbValue;
        private System.Windows.Forms.TextBox tbOptimazedVal;
    }
}

