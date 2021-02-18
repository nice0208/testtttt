namespace UC_FirCircle
{
    partial class UC_FitRectangle2
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbMeasure_Transition = new System.Windows.Forms.ComboBox();
            this.cmbMeasure_Select = new System.Windows.Forms.ComboBox();
            this.label30 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnFitCircle = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.ucLength2 = new UC_Slider.UC_Slider();
            this.ucMeasure_Threshold = new UC_Slider.UC_Slider();
            this.ucMeasure_Length2 = new UC_Slider.UC_Slider();
            this.ucMeasure_Length1 = new UC_Slider.UC_Slider();
            this.ucNum_Measures = new UC_Slider.UC_Slider();
            this.ucLength1 = new UC_Slider.UC_Slider();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ucLength2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.ucMeasure_Threshold);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cmbMeasure_Transition);
            this.groupBox1.Controls.Add(this.ucMeasure_Length2);
            this.groupBox1.Controls.Add(this.ucMeasure_Length1);
            this.groupBox1.Controls.Add(this.ucNum_Measures);
            this.groupBox1.Controls.Add(this.ucLength1);
            this.groupBox1.Controls.Add(this.cmbMeasure_Select);
            this.groupBox1.Controls.Add(this.label30);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnFitCircle);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label31);
            this.groupBox1.Controls.Add(this.label32);
            this.groupBox1.Controls.Add(this.label33);
            this.groupBox1.Location = new System.Drawing.Point(0, -1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(348, 273);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "方形中心設定";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(9, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 27);
            this.label4.TabIndex = 122;
            this.label4.Text = "目標寬";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(9, 203);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 27);
            this.label3.TabIndex = 120;
            this.label3.Text = "卡尺閥值";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbMeasure_Transition
            // 
            this.cmbMeasure_Transition.FormattingEnabled = true;
            this.cmbMeasure_Transition.Items.AddRange(new object[] {
            "黑到白",
            "白到黑",
            "所有"});
            this.cmbMeasure_Transition.Location = new System.Drawing.Point(103, 86);
            this.cmbMeasure_Transition.Name = "cmbMeasure_Transition";
            this.cmbMeasure_Transition.Size = new System.Drawing.Size(69, 20);
            this.cmbMeasure_Transition.TabIndex = 119;
            this.cmbMeasure_Transition.SelectedIndexChanged += new System.EventHandler(this.cmbMeasure_Transition_SelectedIndexChanged);
            // 
            // cmbMeasure_Select
            // 
            this.cmbMeasure_Select.FormattingEnabled = true;
            this.cmbMeasure_Select.Items.AddRange(new object[] {
            "第一",
            "最後"});
            this.cmbMeasure_Select.Location = new System.Drawing.Point(264, 86);
            this.cmbMeasure_Select.Name = "cmbMeasure_Select";
            this.cmbMeasure_Select.Size = new System.Drawing.Size(69, 20);
            this.cmbMeasure_Select.TabIndex = 115;
            this.cmbMeasure_Select.SelectedIndexChanged += new System.EventHandler(this.cmbMeasure_Select_SelectedIndexChanged);
            // 
            // label30
            // 
            this.label30.Location = new System.Drawing.Point(9, 83);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(88, 27);
            this.label30.TabIndex = 114;
            this.label30.Text = "極性設定";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 113);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 27);
            this.label2.TabIndex = 111;
            this.label2.Text = "卡尺數量";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnFitCircle
            // 
            this.btnFitCircle.Location = new System.Drawing.Point(262, 236);
            this.btnFitCircle.Name = "btnFitCircle";
            this.btnFitCircle.Size = new System.Drawing.Size(75, 27);
            this.btnFitCircle.TabIndex = 116;
            this.btnFitCircle.Text = "找圓心";
            this.btnFitCircle.UseVisualStyleBackColor = true;
            this.btnFitCircle.Click += new System.EventHandler(this.btnFitCircle_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 173);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 27);
            this.label1.TabIndex = 111;
            this.label1.Text = "卡尺寬度";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label31
            // 
            this.label31.Location = new System.Drawing.Point(9, 143);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(88, 27);
            this.label31.TabIndex = 111;
            this.label31.Text = "卡尺高度";
            this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label32
            // 
            this.label32.Location = new System.Drawing.Point(9, 20);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(88, 27);
            this.label32.TabIndex = 110;
            this.label32.Text = "目標長";
            this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label33
            // 
            this.label33.Location = new System.Drawing.Point(170, 83);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(88, 27);
            this.label33.TabIndex = 112;
            this.label33.Text = "邊設定";
            this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ucLength2
            // 
            this.ucLength2.Location = new System.Drawing.Point(99, 53);
            this.ucLength2.Maximum = 2500;
            this.ucLength2.Minimum = 5;
            this.ucLength2.Name = "ucLength2";
            this.ucLength2.Size = new System.Drawing.Size(238, 27);
            this.ucLength2.TabIndex = 123;
            this.ucLength2.Value = 5;
            this.ucLength2.ValueChanged += new UC_Slider.UC_Slider.ValueChangeEventHandler(this.ucLength2_ValueChanged);
            // 
            // ucMeasure_Threshold
            // 
            this.ucMeasure_Threshold.Location = new System.Drawing.Point(99, 203);
            this.ucMeasure_Threshold.Maximum = 255;
            this.ucMeasure_Threshold.Minimum = 0;
            this.ucMeasure_Threshold.Name = "ucMeasure_Threshold";
            this.ucMeasure_Threshold.Size = new System.Drawing.Size(238, 27);
            this.ucMeasure_Threshold.TabIndex = 121;
            this.ucMeasure_Threshold.Value = 5;
            this.ucMeasure_Threshold.ValueChanged += new UC_Slider.UC_Slider.ValueChangeEventHandler(this.ucMeasure_Threshold_ValueChanged);
            // 
            // ucMeasure_Length2
            // 
            this.ucMeasure_Length2.Location = new System.Drawing.Point(99, 173);
            this.ucMeasure_Length2.Maximum = 100;
            this.ucMeasure_Length2.Minimum = 5;
            this.ucMeasure_Length2.Name = "ucMeasure_Length2";
            this.ucMeasure_Length2.Size = new System.Drawing.Size(238, 27);
            this.ucMeasure_Length2.TabIndex = 118;
            this.ucMeasure_Length2.Value = 5;
            this.ucMeasure_Length2.ValueChanged += new UC_Slider.UC_Slider.ValueChangeEventHandler(this.ucMeasure_Length2_ValueChanged);
            // 
            // ucMeasure_Length1
            // 
            this.ucMeasure_Length1.Location = new System.Drawing.Point(99, 143);
            this.ucMeasure_Length1.Maximum = 100;
            this.ucMeasure_Length1.Minimum = 5;
            this.ucMeasure_Length1.Name = "ucMeasure_Length1";
            this.ucMeasure_Length1.Size = new System.Drawing.Size(238, 27);
            this.ucMeasure_Length1.TabIndex = 118;
            this.ucMeasure_Length1.Value = 5;
            this.ucMeasure_Length1.ValueChanged += new UC_Slider.UC_Slider.ValueChangeEventHandler(this.ucMeasure_Length1_ValueChanged);
            // 
            // ucNum_Measures
            // 
            this.ucNum_Measures.Location = new System.Drawing.Point(99, 113);
            this.ucNum_Measures.Maximum = 1000;
            this.ucNum_Measures.Minimum = 3;
            this.ucNum_Measures.Name = "ucNum_Measures";
            this.ucNum_Measures.Size = new System.Drawing.Size(238, 27);
            this.ucNum_Measures.TabIndex = 118;
            this.ucNum_Measures.Value = 3;
            this.ucNum_Measures.ValueChanged += new UC_Slider.UC_Slider.ValueChangeEventHandler(this.ucNum_Measures_ValueChanged);
            // 
            // ucLength1
            // 
            this.ucLength1.Location = new System.Drawing.Point(99, 20);
            this.ucLength1.Maximum = 2500;
            this.ucLength1.Minimum = 5;
            this.ucLength1.Name = "ucLength1";
            this.ucLength1.Size = new System.Drawing.Size(238, 27);
            this.ucLength1.TabIndex = 118;
            this.ucLength1.Value = 5;
            this.ucLength1.ValueChanged += new UC_Slider.UC_Slider.ValueChangeEventHandler(this.ucLength1_ValueChanged);
            // 
            // UC_FitRectangle2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.groupBox1);
            this.Name = "UC_FitRectangle2";
            this.Size = new System.Drawing.Size(351, 275);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbMeasure_Transition;
        private UC_Slider.UC_Slider ucMeasure_Length1;
        private UC_Slider.UC_Slider ucNum_Measures;
        private UC_Slider.UC_Slider ucLength1;
        private System.Windows.Forms.ComboBox cmbMeasure_Select;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Button btnFitCircle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label33;
        private UC_Slider.UC_Slider ucMeasure_Length2;
        private System.Windows.Forms.Label label2;
        private UC_Slider.UC_Slider ucMeasure_Threshold;
        private System.Windows.Forms.Label label3;
        private UC_Slider.UC_Slider ucLength2;
        private System.Windows.Forms.Label label4;

    }
}
