using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO.Compression;
using System.Numerics;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace ACG_labs;

public partial class Main_form : Form
{
    private CustomForm info_form;
    // public Drawing draw = new Drawing();
    private EnvironmentInfo system_info = new EnvironmentInfo();
    private Size _size = new Size();

    private const string modelFolder = "C:\\Users\\User\\source\\AKG\\AСG_labs\\Head";

    private static string modelPref = "/model.obj";
    /*private const string doplhinmodel = "/dolphin.obj";
    private const string headmodel = "/model.obj";
    private const string dolphinfolder = "C:\\Users\\User\\source\\AKG";
    private const string planeFolder = "C:\\Users\\User\\source\\AKG\\Plane";
    private const string headFolder = "C:\\Users\\User\\source\\AKG\\Head";
    private const string planemodel = "/model.obj";
    ЛР5
    --
    //отражение через кубические текстуры
    //(вики)
     */
    private ModelInfo model = new ModelInfo("","","","","");

    public static bool _shouldDraw;
    private static Bitmap _bitmap = new Bitmap(1, 1);

    private Vector3 angels = Vector3.Zero;

    private Vector4[] _vArr;
    private float[] _ws;

    private static Vector4[] _modelVArr;

    private Vector4[] _updateVArr;
    private int[][] _fArr;

    private Vector3[] _vtList;
    private int[][] _fvtList;

    private Bitmap? diffuseMap;
    private Bitmap? specularMap;
    private Bitmap? normalMap;



    public Main_form()
    {
        InitializeComponent();
        AllModels.initModels();
        _shouldDraw = false;
        model = new ModelInfo("", "", "", "", "");
        model = AllModels.getModel();//проверить, работает ли
        MakeResizing(this);
        Drawing.MakeZBuffer();
        //init();
    }

    //private static float[][] _zBuffer = new float[2000][];

    private unsafe void DrawPoints()
    {
        using (Graphics g = Graphics.FromImage(_bitmap))
        {
            g.Clear(Calculations.getColorFromV3(Calculations.BgColor * 255));
        }

        switch (Calculations.Mode)
        {
            case 1:
                Drawing.DrawOnlyGrid(_bitmap, Calculations.SelectedColor, _fArr, _updateVArr);
                break;
            case 2:
                Drawing.DrawByLambert(_bitmap, Calculations.SelectedColor, _fArr, _modelVArr,
                    _updateVArr);
                break;
            case 3:
                Drawing.DrawByPhonk(_bitmap, _fArr, _modelVArr, _updateVArr, system_info.vSc);
                break;
            case 4:
                if (diffuseMap != null && normalMap != null && specularMap != null)
                    Drawing.DrawFonkWithTextures(_bitmap, _fArr, _modelVArr, _updateVArr, diffuseMap, specularMap, normalMap, _fvtList, _ws, _vtList, angels);
                else
                    Drawing.DrawByPhonk(_bitmap, _fArr, _modelVArr, _updateVArr, system_info.vSc);
                break;
            case 5:
                Drawing.DrawByPhongWithCube(_bitmap, _fArr, _modelVArr, _updateVArr, diffuseMap, specularMap, normalMap, _fvtList, _ws, _vtList, angels);
                break;
            default:
                Drawing.DrawOnlyGrid(_bitmap, Calculations.SelectedColor, _fArr, _updateVArr);
                break;
        }

    }

    private void Main_form_load(object sender, EventArgs e)
    {
        int width = ClientSize.Width;
        int height = ClientSize.Height;
        system_info.UpdateValues();
        model_loading();

        pictureBox1.Invalidate();
    }

    private void updateVectorPos()
    {

        Calculations.UpdateMatrix();
        Calculations.TranslatePositions(_vArr, _updateVArr, _fArr, _modelVArr, _ws);

        var vertex = Calculations.FindVertexPoint(_modelVArr);

        Drawing.RefillZBuffer();
        DrawPoints();
    }

    private void MakeResizing(Main_form form)
    {
        Calculations.CameraViewSize = form.ClientSize;
        _size.Height = form.ClientSize.Height;
        _size.Width = form.ClientSize.Width;
        Calculations.CameraView = Calculations.CameraViewSize.Width / (float)Calculations.CameraViewSize.Height;
        form.pictureBox1.Size = _size;
        _bitmap = new Bitmap(_size.Width, _size.Height);

        int width = _bitmap.Width;
        int height = _bitmap.Height;

        if (_shouldDraw)
        {
            form.pictureBox1.Invalidate();
        }
    }

    private void Main_form_SizeChanged(object sender, EventArgs e)
    {
        MakeResizing(this);
    }


    private void Main_form_MouseWheel(object sender, MouseEventArgs e)
    {
        if (_shouldDraw)
        {
            if (e.Delta > 0)
            {
                Calculations.ScalingCof += Calculations.Delta;
            }
            else
            {
                Calculations.ScalingCof -= Calculations.Delta;
            }

            wasUpdate = true;
            Calculations.Delta = Calculations.ScalingCof / 8;
            pictureBox1.Invalidate();
        }
    }

    private void Main_form_KeyDown(object sender, KeyEventArgs e)
    {
        System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
        if (e.KeyCode == Keys.Enter)
        {
            info_form = new CustomForm(this, system_info);
            info_form.Show();
        }
        if (_shouldDraw)
        {
            const float angel = (float)Math.PI / 36.0f;

            switch (e.KeyCode)
            {
                case Keys.Down:
                    angels.X += angel;
                    Translations.Transform(_vArr, Matrix4x4.CreateRotationX(angel), Calculations.VertexNormals,
                        Calculations.VPolygonNormals);
                    break;
                case Keys.Up:
                    angels.X -= angel;
                    Translations.Transform(_vArr, Matrix4x4.CreateRotationX(-angel), Calculations.VertexNormals,
                        Calculations.VPolygonNormals);
                    break;
                case Keys.Right:
                    angels.Y += angel;
                    Translations.Transform(_vArr, Matrix4x4.CreateRotationY(angel), Calculations.VertexNormals,
                        Calculations.VPolygonNormals);
                    break;
                case Keys.Left:
                    angels.Y -= angel;
                    Translations.Transform(_vArr, Matrix4x4.CreateRotationY(-angel), Calculations.VertexNormals,
                        Calculations.VPolygonNormals);
                    break;
                case Keys.A:
                    angels.Z += angel;
                    Translations.Transform(_vArr, Matrix4x4.CreateRotationZ(angel), Calculations.VertexNormals,
                        Calculations.VPolygonNormals);
                    break;
                case Keys.D:
                    angels.Z -= angel;
                    Translations.Transform(_vArr, Matrix4x4.CreateRotationZ(-angel), Calculations.VertexNormals,
                        Calculations.VPolygonNormals);
                    break;
                case Keys.Q:
                    Calculations.Mode = 1;
                    break;
                case Keys.W:
                    Calculations.Mode = 2;
                    break;
                case Keys.E:
                    Calculations.Mode = 3;
                    break;
                case Keys.R:
                    Calculations.Mode = 4;
                    break;
                case Keys.T:
                    Calculations.Mode = 5;
                    break;
                case Keys.O:
                    {
                        model = AllModels.setModel(0);
                        model_loading();
                        pictureBox1.Invalidate();
                        break;
                    }
                case Keys.P:
                    {
                        model = AllModels.setModel(1);
                        model_loading();
                        pictureBox1.Invalidate();
                        break;
                    }
                case Keys.L:
                    {
                        model = AllModels.setModel(2);
                        model_loading();
                        pictureBox1.Invalidate();
                        break;
                    }

            }

            wasUpdate = true;
            pictureBox1.Invalidate();
        }
    }

    public bool wasUpdate = false;

    private void pictureBox1_Paint(object sender, PaintEventArgs e)
    {
        if (_shouldDraw && wasUpdate)
        {
            wasUpdate = false;
            Stopwatch sw = Stopwatch.StartNew();
            updateVectorPos();
            sw.Stop();

            double seconds = sw.Elapsed.TotalSeconds;
            double milliseconds = sw.Elapsed.TotalMilliseconds;
            double fps = 1 / seconds;

            //lbWidth.Text = string.Format($"{fps}");
            pictureBox1.Image = _bitmap;
        }
    }

    private void model_loading()
    {
        _shouldDraw = true;
        ObjParser parser = new ObjParser(model.modelFolder + model.modelPref);
        Calculations.UpdateMatrix();

        _vArr = parser.VList.ToArray();
        _vtList = parser.VTList.ToArray();

        _updateVArr = new Vector4[_vArr.Length];

        _ws = new float[_vArr.Length];

        _modelVArr = new Vector4[_vArr.Length];

        _fArr = new int[parser.FList.Count][];
        _fvtList = new int[parser.FVTList.Count][];

        Calculations.VPolygonNormals = new Vector3[_fArr.Length];
        Calculations.VertexNormals = new Vector3[_vArr.Length];

        for (var i = 0; i < parser.FList.Count; i++)
        {
            _fArr[i] = parser.FList[i].ToArray();
            _fvtList[i] = parser.FVTList[i].ToArray();
        }

        Calculations.UpdateMatrix();
        Calculations.TranslatePositions(_vArr, _updateVArr, _fArr, _modelVArr, _ws);
        Calculations.CalcStuff(_fArr, _modelVArr);

        loadTextures();
        wasUpdate = true;
    }



    private void loadTextures()
    {
        diffuseMap?.Dispose();
        diffuseMap = null;
        specularMap?.Dispose();
        specularMap = null;
        normalMap?.Dispose();
        normalMap = null;
        if (model.diffPref != "")
        {
            string diffuse = model.modelFolder + model.diffPref;
            if (File.Exists(diffuse))
            {
                diffuseMap = new Bitmap(diffuse);
            }
        }
        if (model.specPref != "")
        {
            string spec = model.modelFolder + model.specPref;
            if (File.Exists(spec))
            {
                specularMap = new Bitmap(spec);
            }
        }
        if (model.normalsPref != "")
        {
            string norms = model.modelFolder + model.normalsPref;
            if (File.Exists(norms))
            {
                normalMap = new Bitmap(norms);
            }
        }
    }

    private void paramsPB_Click(object sender, EventArgs e)
    {
        info_form = new CustomForm(this, system_info);
        info_form.Show();
    }
}
