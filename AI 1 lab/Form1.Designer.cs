namespace AI_1_lab
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSolve = new System.Windows.Forms.Button();
            this.BtnIncreaseSize = new System.Windows.Forms.Button();
            this.BtnDecreaseSize = new System.Windows.Forms.Button();
            this.btnDFS = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.UpDownDepth = new System.Windows.Forms.NumericUpDown();
            this.UpDownStep = new System.Windows.Forms.NumericUpDown();
            this.btnBBFS = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnIDFS = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownDepth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownStep)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSolve
            // 
            this.btnSolve.BackColor = System.Drawing.Color.PowderBlue;
            this.btnSolve.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSolve.ForeColor = System.Drawing.Color.MediumSlateBlue;
            this.btnSolve.Location = new System.Drawing.Point(471, 22);
            this.btnSolve.Name = "btnSolve";
            this.btnSolve.Size = new System.Drawing.Size(164, 40);
            this.btnSolve.TabIndex = 0;
            this.btnSolve.Text = "Запустить BFS";
            this.btnSolve.UseVisualStyleBackColor = false;
            this.btnSolve.Click += new System.EventHandler(this.btnSolve_Click);
            // 
            // BtnIncreaseSize
            // 
            this.BtnIncreaseSize.BackColor = System.Drawing.Color.PowderBlue;
            this.BtnIncreaseSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtnIncreaseSize.ForeColor = System.Drawing.Color.MediumSlateBlue;
            this.BtnIncreaseSize.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.BtnIncreaseSize.Location = new System.Drawing.Point(471, 206);
            this.BtnIncreaseSize.Name = "BtnIncreaseSize";
            this.BtnIncreaseSize.Size = new System.Drawing.Size(164, 30);
            this.BtnIncreaseSize.TabIndex = 1;
            this.BtnIncreaseSize.Text = "+";
            this.BtnIncreaseSize.UseVisualStyleBackColor = false;
            this.BtnIncreaseSize.Click += new System.EventHandler(this.BtnIncreaseSize_Click);
            // 
            // BtnDecreaseSize
            // 
            this.BtnDecreaseSize.BackColor = System.Drawing.Color.PowderBlue;
            this.BtnDecreaseSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtnDecreaseSize.ForeColor = System.Drawing.Color.MediumSlateBlue;
            this.BtnDecreaseSize.Location = new System.Drawing.Point(471, 242);
            this.BtnDecreaseSize.Name = "BtnDecreaseSize";
            this.BtnDecreaseSize.Size = new System.Drawing.Size(164, 30);
            this.BtnDecreaseSize.TabIndex = 2;
            this.BtnDecreaseSize.Text = "-";
            this.BtnDecreaseSize.UseVisualStyleBackColor = false;
            this.BtnDecreaseSize.Click += new System.EventHandler(this.BtnDecreaseSize_Click);
            // 
            // btnDFS
            // 
            this.btnDFS.BackColor = System.Drawing.Color.PowderBlue;
            this.btnDFS.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnDFS.ForeColor = System.Drawing.Color.MediumSlateBlue;
            this.btnDFS.Location = new System.Drawing.Point(471, 68);
            this.btnDFS.Name = "btnDFS";
            this.btnDFS.Size = new System.Drawing.Size(164, 40);
            this.btnDFS.TabIndex = 3;
            this.btnDFS.Text = "Запустить DFS";
            this.btnDFS.UseVisualStyleBackColor = false;
            this.btnDFS.Click += new System.EventHandler(this.btnDFS_Click);
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.PowderBlue;
            this.btnReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnReset.ForeColor = System.Drawing.Color.MediumSlateBlue;
            this.btnReset.Location = new System.Drawing.Point(471, 338);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(164, 30);
            this.btnReset.TabIndex = 4;
            this.btnReset.Text = "Сброс";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // UpDownDepth
            // 
            this.UpDownDepth.Location = new System.Drawing.Point(471, 279);
            this.UpDownDepth.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.UpDownDepth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UpDownDepth.Name = "UpDownDepth";
            this.UpDownDepth.Size = new System.Drawing.Size(164, 22);
            this.UpDownDepth.TabIndex = 5;
            this.UpDownDepth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UpDownDepth.ValueChanged += new System.EventHandler(this.UpDownDepth_ValueChanged);
            // 
            // UpDownStep
            // 
            this.UpDownStep.Location = new System.Drawing.Point(471, 307);
            this.UpDownStep.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.UpDownStep.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UpDownStep.Name = "UpDownStep";
            this.UpDownStep.Size = new System.Drawing.Size(164, 22);
            this.UpDownStep.TabIndex = 6;
            this.UpDownStep.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UpDownStep.ValueChanged += new System.EventHandler(this.UpDownStep_ValueChanged);
            // 
            // btnBBFS
            // 
            this.btnBBFS.BackColor = System.Drawing.Color.PowderBlue;
            this.btnBBFS.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnBBFS.ForeColor = System.Drawing.Color.MediumSlateBlue;
            this.btnBBFS.Location = new System.Drawing.Point(471, 114);
            this.btnBBFS.Name = "btnBBFS";
            this.btnBBFS.Size = new System.Drawing.Size(164, 40);
            this.btnBBFS.TabIndex = 7;
            this.btnBBFS.Text = "Запустить BBFS";
            this.btnBBFS.UseVisualStyleBackColor = false;
            this.btnBBFS.Click += new System.EventHandler(this.btnBBFS_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.PowderBlue;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.ForeColor = System.Drawing.Color.MediumSlateBlue;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.button2.Location = new System.Drawing.Point(471, 374);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(164, 30);
            this.button2.TabIndex = 8;
            this.button2.Text = "анимация";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnIDFS
            // 
            this.btnIDFS.BackColor = System.Drawing.Color.PowderBlue;
            this.btnIDFS.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnIDFS.ForeColor = System.Drawing.Color.MediumSlateBlue;
            this.btnIDFS.Location = new System.Drawing.Point(471, 160);
            this.btnIDFS.Name = "btnIDFS";
            this.btnIDFS.Size = new System.Drawing.Size(164, 40);
            this.btnIDFS.TabIndex = 9;
            this.btnIDFS.Text = "Запустить IDFS";
            this.btnIDFS.UseVisualStyleBackColor = false;
            this.btnIDFS.Click += new System.EventHandler(this.btnIDFS_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 404);
            this.Controls.Add(this.btnIDFS);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnBBFS);
            this.Controls.Add(this.UpDownStep);
            this.Controls.Add(this.UpDownDepth);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnDFS);
            this.Controls.Add(this.BtnDecreaseSize);
            this.Controls.Add(this.BtnIncreaseSize);
            this.Controls.Add(this.btnSolve);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.UpDownDepth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownStep)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSolve;
        private System.Windows.Forms.Button BtnIncreaseSize;
        private System.Windows.Forms.Button BtnDecreaseSize;
        private System.Windows.Forms.Button btnDFS;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.NumericUpDown UpDownDepth;
        private System.Windows.Forms.NumericUpDown UpDownStep;
        private System.Windows.Forms.Button btnBBFS;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnIDFS;
    }
}

