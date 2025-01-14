namespace SpelQwixx
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
            this.picCanvas = new System.Windows.Forms.PictureBox();
            this.btnDobbelsteen = new System.Windows.Forms.Button();
            this.btnVolgendebeurt = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picCanvas)).BeginInit();
            this.SuspendLayout();
            // 
            // picCanvas
            // 
            this.picCanvas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.picCanvas.Location = new System.Drawing.Point(919, 448);
            this.picCanvas.Name = "picCanvas";
            this.picCanvas.Size = new System.Drawing.Size(10, 10);
            this.picCanvas.TabIndex = 0;
            this.picCanvas.TabStop = false;
            this.picCanvas.Click += new System.EventHandler(this.picCanvas_Click);
            this.picCanvas.Paint += new System.Windows.Forms.PaintEventHandler(this.UpdatePictureBoxGraphics);
            // 
            // btnDobbelsteen
            // 
            this.btnDobbelsteen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnDobbelsteen.Location = new System.Drawing.Point(880, 222);
            this.btnDobbelsteen.Name = "btnDobbelsteen";
            this.btnDobbelsteen.Size = new System.Drawing.Size(116, 92);
            this.btnDobbelsteen.TabIndex = 1;
            this.btnDobbelsteen.Text = "Dobbelsteen";
            this.btnDobbelsteen.UseVisualStyleBackColor = false;
            this.btnDobbelsteen.Click += new System.EventHandler(this.btnDobbelsteen_Click);
            // 
            // btnVolgendebeurt
            // 
            this.btnVolgendebeurt.Location = new System.Drawing.Point(880, 448);
            this.btnVolgendebeurt.Name = "btnVolgendebeurt";
            this.btnVolgendebeurt.Size = new System.Drawing.Size(116, 63);
            this.btnVolgendebeurt.TabIndex = 2;
            this.btnVolgendebeurt.Text = "Volgende beurt";
            this.btnVolgendebeurt.UseVisualStyleBackColor = true;
            this.btnVolgendebeurt.Click += new System.EventHandler(this.btnVolgendebeurt_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1428, 766);
            this.Controls.Add(this.btnVolgendebeurt);
            this.Controls.Add(this.btnDobbelsteen);
            this.Controls.Add(this.picCanvas);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picCanvas)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picCanvas;
        private System.Windows.Forms.Button btnDobbelsteen;
        private System.Windows.Forms.Button btnVolgendebeurt;
    }
}

