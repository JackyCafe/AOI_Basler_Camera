namespace AOI_Baser_Camera
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnInitCamera = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.picBox = new System.Windows.Forms.PictureBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.orgiPictureBox = new System.Windows.Forms.PictureBox();
            this.Contours = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.orgiPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // btnInitCamera
            // 
            this.btnInitCamera.Location = new System.Drawing.Point(59, 36);
            this.btnInitCamera.Name = "btnInitCamera";
            this.btnInitCamera.Size = new System.Drawing.Size(120, 46);
            this.btnInitCamera.TabIndex = 0;
            this.btnInitCamera.Text = "init Camera";
            this.btnInitCamera.UseVisualStyleBackColor = true;
            this.btnInitCamera.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(990, 36);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(138, 29);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "abc";
            // 
            // picBox
            // 
            this.picBox.Location = new System.Drawing.Point(777, 58);
            this.picBox.Name = "picBox";
            this.picBox.Size = new System.Drawing.Size(614, 589);
            this.picBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picBox.TabIndex = 2;
            this.picBox.TabStop = false;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(261, 36);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(101, 46);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(450, 36);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(84, 46);
            this.btnStop.TabIndex = 4;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            // 
            // orgiPictureBox
            // 
            this.orgiPictureBox.Location = new System.Drawing.Point(48, 124);
            this.orgiPictureBox.Name = "orgiPictureBox";
            this.orgiPictureBox.Size = new System.Drawing.Size(624, 492);
            this.orgiPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.orgiPictureBox.TabIndex = 5;
            this.orgiPictureBox.TabStop = false;
            // 
            // Contours
            // 
            this.Contours.Location = new System.Drawing.Point(573, 36);
            this.Contours.Name = "Contours";
            this.Contours.Size = new System.Drawing.Size(99, 46);
            this.Contours.TabIndex = 6;
            this.Contours.Text = "Contours";
            this.Contours.UseVisualStyleBackColor = true;
            this.Contours.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1578, 659);
            this.Controls.Add(this.Contours);
            this.Controls.Add(this.orgiPictureBox);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.picBox);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnInitCamera);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.orgiPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnInitCamera;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.PictureBox picBox;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.PictureBox orgiPictureBox;
        private System.Windows.Forms.Button Contours;
    }
}

