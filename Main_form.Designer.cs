namespace ACG_labs;

partial class Main_form
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main_form));
        pictureBox1 = new PictureBox();
        paramsPB = new PictureBox();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
        ((System.ComponentModel.ISupportInitialize)paramsPB).BeginInit();
        SuspendLayout();
        // 
        // pictureBox1
        // 
        pictureBox1.BackColor = SystemColors.GrayText;
        pictureBox1.Location = new Point(1, 0);
        pictureBox1.Margin = new Padding(4, 5, 4, 5);
        pictureBox1.Name = "pictureBox1";
        pictureBox1.Size = new Size(1000, 662);
        pictureBox1.TabIndex = 0;
        pictureBox1.TabStop = false;
        pictureBox1.Paint += pictureBox1_Paint;
        // 
        // paramsPB
        // 
        paramsPB.BackColor = SystemColors.ButtonHighlight;
        paramsPB.BackgroundImageLayout = ImageLayout.Center;
        paramsPB.BorderStyle = BorderStyle.FixedSingle;
        paramsPB.Image = (Image)resources.GetObject("paramsPB.Image");
        paramsPB.Location = new Point(924, 12);
        paramsPB.Name = "paramsPB";
        paramsPB.Size = new Size(64, 68);
        paramsPB.TabIndex = 2;
        paramsPB.TabStop = false;
        paramsPB.Click += paramsPB_Click;
        // 
        // Main_form
        // 
        AutoScaleDimensions = new SizeF(10F, 25F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = SystemColors.ActiveCaption;
        ClientSize = new Size(1000, 702);
        Controls.Add(paramsPB);
        Controls.Add(pictureBox1);
        Margin = new Padding(4, 5, 4, 5);
        MinimizeBox = false;
        MinimumSize = new Size(744, 440);
        Name = "Main_form";
        Text = "Dolphin";
        Load += Main_form_load;
        SizeChanged += Main_form_SizeChanged;
        KeyDown += Main_form_KeyDown;
        MouseWheel += Main_form_MouseWheel;
        ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
        ((System.ComponentModel.ISupportInitialize)paramsPB).EndInit();
        ResumeLayout(false);
    }

    public System.Windows.Forms.PictureBox pictureBox1;

    #endregion
    private PictureBox paramsPB;
}
