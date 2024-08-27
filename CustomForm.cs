using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace ACG_labs
{
    public partial class CustomForm : Form
    {

        private Main_form parent;
        private EnvironmentInfo dest;
        public CustomForm(Main_form form1, EnvironmentInfo info)
        {
            parent = form1;
            dest = info;
            InitializeComponent();

            //Bg Color
            backgroundTB.Text = v3ToText(dest.vBg);
            //Selected Color
            gridColorTB.Text = v3ToText(dest.vSc);

            //Light Pos
            lightXTB.Text = dest.Light.X.ToString(CultureInfo.InvariantCulture);
            lightYTB.Text = dest.Light.Y.ToString(CultureInfo.InvariantCulture);
            lightZTB.Text = dest.Light.Z.ToString(CultureInfo.InvariantCulture);

            //Phong Bg color
            lightBackTB.Text = v3ToText(dest.vIa);

            //Phong Diffuse color
            ambientLightTB.Text = v3ToText(dest.vId);

            //Phong Specular color
            mirrorLightTB.Text = v3ToText(dest.vIs);

            //Camera position
            cameraXTB.Text = dest.CameraPosition.X.ToString(CultureInfo.InvariantCulture);
            cameraYTB.Text = dest.CameraPosition.Y.ToString(CultureInfo.InvariantCulture);
            cameraZTB.Text = dest.CameraPosition.Z.ToString(CultureInfo.InvariantCulture);


            //Phong koef
            kaTB.Text = dest.Ka.ToString(CultureInfo.InvariantCulture);
            kdTB.Text = dest.Kd.ToString(CultureInfo.InvariantCulture);
            ksTB.Text = dest.Ks.ToString(CultureInfo.InvariantCulture);
            alphaTB.Text = dest.alpha.ToString(CultureInfo.InvariantCulture);


        }


        private Vector3 textToV3(string txt)
        {
            string[] parts = txt.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1)
            {
                return new Vector3(float.Parse(parts[0], CultureInfo.InvariantCulture.NumberFormat));
            }
            else
            {
                return new Vector3(
                float.Parse(parts[0], CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(parts[1], CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(parts[2], CultureInfo.InvariantCulture.NumberFormat)
            );
            }
        }

        private string v3ToText(Vector3 v3)
        {
            CultureInfo culture = new CultureInfo("en-US");
            culture.NumberFormat.NumberDecimalSeparator = ".";

            if (v3.X == v3.Y && v3.Y == v3.Z)
            {
                return v3.X.ToString("F2", culture);
            }
            else
            {
                string result = string.Format(culture, "{0:F2} {1:F2} {2:F2}", v3.X, v3.Y, v3.Z);
                return result;
            }
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            dest.vBg = textToV3(backgroundTB.Text);
            dest.vSc = textToV3(gridColorTB.Text);

            dest.Light = new Vector3(
                float.Parse(lightXTB.Text, CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(lightYTB.Text, CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(lightZTB.Text, CultureInfo.InvariantCulture.NumberFormat)
            );

            dest.vIa = textToV3(lightBackTB.Text);
            dest.vId = textToV3(ambientLightTB.Text);
            dest.vIs = textToV3(mirrorLightTB.Text);

            //Camera
            dest.CameraPosition = new Vector3(
               float.Parse(cameraXTB.Text, CultureInfo.InvariantCulture.NumberFormat),
               float.Parse(cameraYTB.Text, CultureInfo.InvariantCulture.NumberFormat),
               float.Parse(cameraZTB.Text, CultureInfo.InvariantCulture.NumberFormat)
           );

            dest.Ka = float.Parse(kaTB.Text, CultureInfo.InvariantCulture.NumberFormat);
            dest.Kd = float.Parse(kdTB.Text, CultureInfo.InvariantCulture.NumberFormat);
            dest.Ks = float.Parse(ksTB.Text, CultureInfo.InvariantCulture.NumberFormat);
            dest.alpha = float.Parse(alphaTB.Text, CultureInfo.InvariantCulture.NumberFormat);
            dest.UpdateValues();
            parent.wasUpdate = true;
            parent.pictureBox1.Invalidate();
        }

        private void CustomForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            parent.Show();
        }

        private void CustomForm_Shown(object sender, EventArgs e)
        {
            foreach (Control ctrl in this.Controls)
                ctrl.Visible = true;
        }
    }
}
