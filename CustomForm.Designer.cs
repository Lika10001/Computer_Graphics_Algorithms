using System.Globalization;
using System.Numerics;

namespace ACG_labs
{
    public partial class CustomForm
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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            panel1 = new Panel();
            label13 = new Label();
            label12 = new Label();
            panel2 = new Panel();
            label11 = new Label();
            label10 = new Label();
            label9 = new Label();
            label8 = new Label();
            label7 = new Label();
            backgroundTB = new TextBox();
            gridColorTB = new TextBox();
            lightBackTB = new TextBox();
            ambientLightTB = new TextBox();
            mirrorLightTB = new TextBox();
            alphaTB = new TextBox();
            applyButton = new Button();
            ksTB = new TextBox();
            kdTB = new TextBox();
            kaTB = new TextBox();
            lightXTB = new TextBox();
            lightYTB = new TextBox();
            lightZTB = new TextBox();
            cameraXTB = new TextBox();
            cameraYTB = new TextBox();
            cameraZTB = new TextBox();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(63, 6);
            label1.Name = "label1";
            label1.Size = new Size(136, 32);
            label1.TabIndex = 0;
            label1.Text = "Цвет фона:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(18, 50);
            label2.Name = "label2";
            label2.Size = new Size(236, 32);
            label2.TabIndex = 1;
            label2.Text = "Цвет заливки сетки:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(9, 94);
            label3.Name = "label3";
            label3.Size = new Size(254, 32);
            label3.TabIndex = 2;
            label3.Text = "Цвет фонового света:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label4.Location = new Point(0, 136);
            label4.Name = "label4";
            label4.Size = new Size(284, 32);
            label4.TabIndex = 3;
            label4.Text = "Цвет рассеянного света:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label5.Location = new Point(0, 178);
            label5.Name = "label5";
            label5.Size = new Size(284, 32);
            label5.TabIndex = 4;
            label5.Text = "Цвет зеркального света:";
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ActiveCaption;
            panel1.Controls.Add(label13);
            panel1.Controls.Add(label12);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label4);
            panel1.Location = new Point(3, 1);
            panel1.Name = "panel1";
            panel1.Size = new Size(285, 343);
            panel1.TabIndex = 6;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label13.Location = new Point(34, 289);
            label13.Name = "label13";
            label13.Size = new Size(207, 32);
            label13.TabIndex = 6;
            label13.Text = "Позиция камеры:";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label12.Location = new Point(47, 244);
            label12.Name = "label12";
            label12.Size = new Size(181, 32);
            label12.TabIndex = 5;
            label12.Text = "Позиция света:";
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.ActiveCaption;
            panel2.Controls.Add(label11);
            panel2.Controls.Add(label10);
            panel2.Controls.Add(label9);
            panel2.Controls.Add(label8);
            panel2.Controls.Add(label7);
            panel2.Location = new Point(456, 1);
            panel2.Name = "panel2";
            panel2.Size = new Size(249, 343);
            panel2.TabIndex = 7;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label11.Location = new Point(42, 168);
            label11.Name = "label11";
            label11.Size = new Size(161, 32);
            label11.TabIndex = 10;
            label11.Text = "поверхности:";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label10.Location = new Point(3, 136);
            label10.Name = "label10";
            label10.Size = new Size(246, 32);
            label10.TabIndex = 9;
            label10.Text = "Коэффициент блеска";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label9.Location = new Point(0, 94);
            label9.Name = "label9";
            label9.Size = new Size(254, 32);
            label9.TabIndex = 8;
            label9.Text = "Кs зеркального света:";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label8.Location = new Point(0, 50);
            label8.Name = "label8";
            label8.Size = new Size(258, 32);
            label8.TabIndex = 7;
            label8.Text = "Кd рассеянного света:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label7.Location = new Point(18, 8);
            label7.Name = "label7";
            label7.Size = new Size(226, 32);
            label7.TabIndex = 6;
            label7.Text = "Ка фонового света:";
            // 
            // backgroundTB
            // 
            backgroundTB.Location = new Point(294, 10);
            backgroundTB.Name = "backgroundTB";
            backgroundTB.Size = new Size(150, 31);
            backgroundTB.TabIndex = 8;
            // 
            // gridColorTB
            // 
            gridColorTB.Location = new Point(294, 54);
            gridColorTB.Name = "gridColorTB";
            gridColorTB.Size = new Size(150, 31);
            gridColorTB.TabIndex = 9;
            // 
            // lightBackTB
            // 
            lightBackTB.Location = new Point(294, 98);
            lightBackTB.Name = "lightBackTB";
            lightBackTB.Size = new Size(150, 31);
            lightBackTB.TabIndex = 10;
            // 
            // ambientLightTB
            // 
            ambientLightTB.Location = new Point(294, 140);
            ambientLightTB.Name = "ambientLightTB";
            ambientLightTB.Size = new Size(150, 31);
            ambientLightTB.TabIndex = 11;
            // 
            // mirrorLightTB
            // 
            mirrorLightTB.Location = new Point(294, 182);
            mirrorLightTB.Name = "mirrorLightTB";
            mirrorLightTB.Size = new Size(150, 31);
            mirrorLightTB.TabIndex = 12;
            // 
            // alphaTB
            // 
            alphaTB.Location = new Point(711, 157);
            alphaTB.Name = "alphaTB";
            alphaTB.Size = new Size(150, 31);
            alphaTB.TabIndex = 17;
            // 
            // applyButton
            // 
            applyButton.BackColor = SystemColors.ActiveCaption;
            applyButton.Location = new Point(711, 291);
            applyButton.Name = "applyButton";
            applyButton.Size = new Size(150, 34);
            applyButton.TabIndex = 18;
            applyButton.Text = "OK";
            applyButton.UseVisualStyleBackColor = false;
            applyButton.Click += applyButton_Click;
            // 
            // ksTB
            // 
            ksTB.Location = new Point(711, 98);
            ksTB.Name = "ksTB";
            ksTB.Size = new Size(150, 31);
            ksTB.TabIndex = 19;
            // 
            // kdTB
            // 
            kdTB.Location = new Point(711, 54);
            kdTB.Name = "kdTB";
            kdTB.Size = new Size(150, 31);
            kdTB.TabIndex = 20;
            // 
            // kaTB
            // 
            kaTB.Location = new Point(711, 12);
            kaTB.Name = "kaTB";
            kaTB.Size = new Size(150, 31);
            kaTB.TabIndex = 21;
            // 
            // lightXTB
            // 
            lightXTB.Location = new Point(294, 248);
            lightXTB.Name = "lightXTB";
            lightXTB.Size = new Size(48, 31);
            lightXTB.TabIndex = 22;
            // 
            // lightYTB
            // 
            lightYTB.Location = new Point(348, 248);
            lightYTB.Name = "lightYTB";
            lightYTB.Size = new Size(48, 31);
            lightYTB.TabIndex = 23;
            // 
            // lightZTB
            // 
            lightZTB.Location = new Point(402, 248);
            lightZTB.Name = "lightZTB";
            lightZTB.Size = new Size(48, 31);
            lightZTB.TabIndex = 24;
            // 
            // cameraXTB
            // 
            cameraXTB.Location = new Point(294, 293);
            cameraXTB.Name = "cameraXTB";
            cameraXTB.Size = new Size(48, 31);
            cameraXTB.TabIndex = 25;
            // 
            // cameraYTB
            // 
            cameraYTB.Location = new Point(348, 294);
            cameraYTB.Name = "cameraYTB";
            cameraYTB.Size = new Size(48, 31);
            cameraYTB.TabIndex = 26;
            // 
            // cameraZTB
            // 
            cameraZTB.Location = new Point(402, 294);
            cameraZTB.Name = "cameraZTB";
            cameraZTB.Size = new Size(48, 31);
            cameraZTB.TabIndex = 27;
            // 
            // CustomForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(869, 337);
            Controls.Add(cameraZTB);
            Controls.Add(cameraYTB);
            Controls.Add(cameraXTB);
            Controls.Add(lightZTB);
            Controls.Add(lightYTB);
            Controls.Add(lightXTB);
            Controls.Add(kaTB);
            Controls.Add(kdTB);
            Controls.Add(ksTB);
            Controls.Add(applyButton);
            Controls.Add(alphaTB);
            Controls.Add(mirrorLightTB);
            Controls.Add(ambientLightTB);
            Controls.Add(lightBackTB);
            Controls.Add(gridColorTB);
            Controls.Add(backgroundTB);
            Controls.Add(panel2);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "CustomForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CustomForm";
            FormClosed += CustomForm_FormClosed;
            Shown += CustomForm_Shown;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Panel panel1;
        private Panel panel2;
        private Label label11;
        private Label label10;
        private Label label9;
        private Label label8;
        private Label label7;
        private TextBox backgroundTB;
        private TextBox gridColorTB;
        private TextBox lightBackTB;
        private TextBox ambientLightTB;
        private TextBox mirrorLightTB;
        private TextBox textBox6;
        private TextBox textBox7;
        private TextBox textBox8;
        private TextBox textBox9;
        private TextBox alphaTB;
        private Button applyButton;


        private TextBox ksTB;
        private TextBox kdTB;
        private TextBox kaTB;
        private Label label13;
        private Label label12;
        private TextBox lightXTB;
        private TextBox lightYTB;
        private TextBox lightZTB;
        private TextBox cameraXTB;
        private TextBox cameraYTB;
        private TextBox cameraZTB;
    }




}