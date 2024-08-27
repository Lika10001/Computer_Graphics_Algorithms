using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ACG_labs
{
    public class EnvironmentInfo
    {
        public  Vector3 vBg = new Vector3(1f);
        public Vector3 vSc = new Vector3(0.5f, 0, 1);

        //Camera
        public Vector3 CameraPosition = new Vector3(3, 0, 0);
        public Vector3 Target = new Vector3(0, 0, 0);

        //Light
        public Vector3 Light = new Vector3(3, 0, 0);

        //Phong Lightning
        public Vector3 vIa = new Vector3(0.2f, 0f, 0.9f);
        public Vector3 vId = new Vector3(0f, 0f, 0.5f);
        public Vector3 vIs = new Vector3(0.9f, 0f, 0f);
        

        //Phong Koefs
        public float Ka = 0.01f;
        public  float Kd = 1.0f;
        public float Ks = 0.9f;
        public float alpha = 5.0f;
        //public bool impl_gamma_correction;

        public void UpdateValues()
        {
            Calculations.SelectedColor = vSc;
            Calculations.BgColor = vBg;
            Calculations.Ia = ApplyGamma(vIa, 2.2f);
            Calculations.Id = ApplyGamma(vId, 2.2f);
            Calculations.Is = ApplyGamma(vIs, 2.2f);
            Calculations.Ka = Ka;
            Calculations.Kd = Kd;
            Calculations.Ks = Ks;
            Calculations.Alpha = alpha;
            Calculations.LambertLight = Light;
            Calculations.Camera = CameraPosition;
            Calculations.Target = Target;
        }


        public static Vector3 ApplyGamma(Vector3 color, float gamma)
        {
            color.X = (float)Math.Pow(color.X, gamma);
            color.Y = (float)Math.Pow(color.Y, gamma);
            color.Z = (float)Math.Pow(color.Z, gamma);

            return color;
        }
    }
}
