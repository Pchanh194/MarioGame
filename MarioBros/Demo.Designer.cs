namespace MarioBros
{
    partial class Demo
    {
        // Required designer variable.
        private System.ComponentModel.IContainer components = null;

        // True if managed resources should be disposed; otherwise, false.
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        // Required method for Designer support 
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Demo));
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).BeginInit();
            this.SuspendLayout();
            // 
            // Canvas
            // 
            this.Canvas.Image = ((System.Drawing.Image)(resources.GetObject("Canvas.Image")));
            this.Canvas.Location = new System.Drawing.Point(0, 0);
            this.Canvas.Dock = System.Windows.Forms.DockStyle.Left;
            this.Canvas.Margin = new System.Windows.Forms.Padding(5);
            this.Canvas.Size = new System.Drawing.Size(1035, 554);
            // 
            // Demo
            // 
            this.Name = "Demo";
            this.Text = "MarioBros Game";
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1051, 554);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.Load += new System.EventHandler(this.Demo_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Demo_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Demo_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).EndInit();
            this.ResumeLayout(false);

        }
    }
}

