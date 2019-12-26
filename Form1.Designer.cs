namespace Dirihle
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
            this.Table = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Nbox = new System.Windows.Forms.NumericUpDown();
            this.Mbox = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.NmaxBox = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.AccuracyBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.YoBox = new System.Windows.Forms.NumericUpDown();
            this.YnBox = new System.Windows.Forms.NumericUpDown();
            this.XoBox = new System.Windows.Forms.NumericUpDown();
            this.XnBox = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.dsafgasdg = new System.Windows.Forms.Label();
            this.IterLabel = new System.Windows.Forms.Label();
            this.AccMaxLabel = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.MaxDifLabel = new System.Windows.Forms.Label();
            this.DotLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Table)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Nbox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Mbox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NmaxBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.YoBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.YnBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.XoBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.XnBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Table
            // 
            this.Table.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Table.Location = new System.Drawing.Point(307, 106);
            this.Table.Name = "Table";
            this.Table.Size = new System.Drawing.Size(481, 332);
            this.Table.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 299);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Число разбиений по х:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 325);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Число разбиений по у:";
            // 
            // Nbox
            // 
            this.Nbox.Location = new System.Drawing.Point(140, 297);
            this.Nbox.Name = "Nbox";
            this.Nbox.Size = new System.Drawing.Size(120, 20);
            this.Nbox.TabIndex = 3;
            // 
            // Mbox
            // 
            this.Mbox.Location = new System.Drawing.Point(140, 323);
            this.Mbox.Name = "Mbox";
            this.Mbox.Size = new System.Drawing.Size(120, 20);
            this.Mbox.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 386);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(270, 52);
            this.button1.TabIndex = 5;
            this.button1.Text = "Решить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 273);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Макс. число шагов:";
            // 
            // NmaxBox
            // 
            this.NmaxBox.Location = new System.Drawing.Point(140, 271);
            this.NmaxBox.Name = "NmaxBox";
            this.NmaxBox.Size = new System.Drawing.Size(120, 20);
            this.NmaxBox.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 352);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Точность метода:";
            // 
            // AccuracyBox
            // 
            this.AccuracyBox.Location = new System.Drawing.Point(140, 349);
            this.AccuracyBox.Name = "AccuracyBox";
            this.AccuracyBox.Size = new System.Drawing.Size(120, 20);
            this.AccuracyBox.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 200);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Границы на оси х:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 226);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Границы на оси у:";
            // 
            // YoBox
            // 
            this.YoBox.Location = new System.Drawing.Point(140, 224);
            this.YoBox.Name = "YoBox";
            this.YoBox.Size = new System.Drawing.Size(59, 20);
            this.YoBox.TabIndex = 12;
            // 
            // YnBox
            // 
            this.YnBox.Location = new System.Drawing.Point(201, 224);
            this.YnBox.Name = "YnBox";
            this.YnBox.Size = new System.Drawing.Size(59, 20);
            this.YnBox.TabIndex = 13;
            // 
            // XoBox
            // 
            this.XoBox.Location = new System.Drawing.Point(140, 198);
            this.XoBox.Name = "XoBox";
            this.XoBox.Size = new System.Drawing.Size(59, 20);
            this.XoBox.TabIndex = 14;
            // 
            // XnBox
            // 
            this.XnBox.Location = new System.Drawing.Point(201, 198);
            this.XnBox.Name = "XnBox";
            this.XnBox.Size = new System.Drawing.Size(59, 20);
            this.XnBox.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(304, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(116, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Проведено итераций:";
            // 
            // dsafgasdg
            // 
            this.dsafgasdg.AutoSize = true;
            this.dsafgasdg.Location = new System.Drawing.Point(304, 33);
            this.dsafgasdg.Name = "dsafgasdg";
            this.dsafgasdg.Size = new System.Drawing.Size(123, 13);
            this.dsafgasdg.TabIndex = 17;
            this.dsafgasdg.Text = "Достигнутая точность:";
            // 
            // IterLabel
            // 
            this.IterLabel.AutoSize = true;
            this.IterLabel.Location = new System.Drawing.Point(433, 9);
            this.IterLabel.Name = "IterLabel";
            this.IterLabel.Size = new System.Drawing.Size(13, 13);
            this.IterLabel.TabIndex = 18;
            this.IterLabel.Text = "0";
            // 
            // AccMaxLabel
            // 
            this.AccMaxLabel.AutoSize = true;
            this.AccMaxLabel.Location = new System.Drawing.Point(433, 33);
            this.AccMaxLabel.Name = "AccMaxLabel";
            this.AccMaxLabel.Size = new System.Drawing.Size(13, 13);
            this.AccMaxLabel.TabIndex = 19;
            this.AccMaxLabel.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(304, 60);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(228, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Максимальная разница двух приближений:";
            // 
            // MaxDifLabel
            // 
            this.MaxDifLabel.AutoSize = true;
            this.MaxDifLabel.Location = new System.Drawing.Point(538, 60);
            this.MaxDifLabel.Name = "MaxDifLabel";
            this.MaxDifLabel.Size = new System.Drawing.Size(13, 13);
            this.MaxDifLabel.TabIndex = 21;
            this.MaxDifLabel.Text = "0";
            // 
            // DotLabel
            // 
            this.DotLabel.AutoSize = true;
            this.DotLabel.Location = new System.Drawing.Point(304, 82);
            this.DotLabel.Name = "DotLabel";
            this.DotLabel.Size = new System.Drawing.Size(45, 13);
            this.DotLabel.TabIndex = 22;
            this.DotLabel.Text = "В точке";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Dirihle.Properties.Resources.Picture;
            this.pictureBox1.Location = new System.Drawing.Point(27, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(222, 161);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 23;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.DotLabel);
            this.Controls.Add(this.MaxDifLabel);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.AccMaxLabel);
            this.Controls.Add(this.IterLabel);
            this.Controls.Add(this.dsafgasdg);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.XnBox);
            this.Controls.Add(this.XoBox);
            this.Controls.Add(this.YnBox);
            this.Controls.Add(this.YoBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.AccuracyBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.NmaxBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Mbox);
            this.Controls.Add(this.Nbox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Table);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.Table)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Nbox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Mbox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NmaxBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.YoBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.YnBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.XoBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.XnBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView Table;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown Nbox;
        private System.Windows.Forms.NumericUpDown Mbox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown NmaxBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox AccuracyBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown YoBox;
        private System.Windows.Forms.NumericUpDown YnBox;
        private System.Windows.Forms.NumericUpDown XoBox;
        private System.Windows.Forms.NumericUpDown XnBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label dsafgasdg;
        private System.Windows.Forms.Label IterLabel;
        private System.Windows.Forms.Label AccMaxLabel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label MaxDifLabel;
        private System.Windows.Forms.Label DotLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

