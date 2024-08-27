using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Intrinsics;
using System.Threading.Tasks;

namespace ACG_labs
{
    public class Drawing
    {
        private static BitmapData bData;
        public static byte bitsPerPixel;
        private static float[][] zBuffer = new float[2000][];
        private static int bWidth;
        private static int bHeight;
        public static void RefillZBuffer()
        {
            for (var i = 0; i < zBuffer.Length; i++)
            {
                for (var j = 0; j < zBuffer[i].Length; j++)
                {
                    zBuffer[i][j] = 1.0f;
                }
            }
        }

        public static void MakeZBuffer()
        {
            for (int i = 0; i < zBuffer.Length; i++)
            {
                zBuffer[i] = new float[2000];
            }
        }

        private static string[] skyboxPrefixes = {
            "",
            Path.Combine("C:\\Users\\User\\source\\AKG\\AСG_labs","forest-skyboxes\\Brudslojan\\"),
            Path.Combine("C:\\Users\\User\\source\\AKG\\AСG_labs","forest-skyboxes\\Langholmen2\\"),
            Path.Combine("C:\\Users\\User\\source\\AKG\\AСG_labs","forest-skyboxes\\Langholmen3\\"),
            Path.Combine("C:\\Users\\User\\source\\AKG\\AСG_labs","forest-skyboxes\\MountainPath\\"),
            Path.Combine("C:\\Users\\User\\source\\AKG\\AСG_labs","forest-skyboxes\\Plants\\"),
            Path.Combine("C:\\Users\\User\\source\\AKG\\AСG_labs","forest-skyboxes\\envmap_interstellar\\"),
        };

        private static string skyboxPrefix = skyboxPrefixes[1];

        private static Bitmap[] cubeTextures = {
            new Bitmap(skyboxPrefix + "posx.jpg"),
            new Bitmap(skyboxPrefix + "negx.jpg"),
            new Bitmap(skyboxPrefix + "posz.jpg"),
            new Bitmap(skyboxPrefix + "negz.jpg"),
            new Bitmap(skyboxPrefix + "posy.jpg"),
            new Bitmap(skyboxPrefix + "negy.jpg"),
        };

        //cube
        private static string getSightS(Vector3 interpolatedNormal)
        {
            string sight;

            // ������� ������������ ���������� �������
            float maxX = Math.Abs(interpolatedNormal.X);
            float maxY = Math.Abs(interpolatedNormal.Y);
            float maxZ = Math.Abs(interpolatedNormal.Z);

            float maxComponent = Math.Max(maxX, Math.Max(maxY, maxZ));

            // ���������� �����������, ����� �������� ������� �������
            if (maxComponent == maxX)
            {
                sight = "+x";
                if (interpolatedNormal.X < 0)
                {
                    sight = "-x";
                }
            }
            else if (maxComponent == maxY)
            {
                sight = "+y";
                if (interpolatedNormal.Y < 0)
                {
                    sight = "-y";
                }
            }
            else
            {
                sight = "+z";
                if (interpolatedNormal.Z < 0)
                {
                    sight = "-z";
                }
            }

            return sight;
        }

        // x is u and y is v
        private static Vector2 getUV(Vector3 normal, string sight)
        {
            Vector2 uv = Vector2.Zero;

            float x = normal.X;
            float y = normal.Y;
            float z = normal.Z;

            if (sight == "+x")
            {
                uv.X = (-z / MathF.Abs(x) + 1) / 2;
                uv.Y = (-y / MathF.Abs(x) + 1) / 2;
            }
            else if (sight == "-x")
            {
                uv.X = (z / MathF.Abs(x) + 1) / 2;
                uv.Y = (-y / MathF.Abs(x) + 1) / 2;
            }
            else if (sight == "+z")
            {
                uv.X = (x / MathF.Abs(z) + 1) / 2;
                uv.Y = (-y / MathF.Abs(z) + 1) / 2;
            }
            else if (sight == "-z")
            {
                uv.X = (-x / MathF.Abs(z) + 1) / 2;
                uv.Y = (-y / MathF.Abs(z) + 1) / 2;
            }
            else if (sight == "+y")
            {
                uv.X = (-z / MathF.Abs(y) + 1) / 2;
                uv.Y = (-x / MathF.Abs(y) + 1) / 2;
            }
            else if (sight == "-y")
            {
                uv.X = (z / MathF.Abs(y) + 1) / 2;
                uv.Y = (-x / MathF.Abs(y) + 1) / 2;
            }

            return uv;
        }

        private static Vector3 GetColorFromBitmap(Bitmap map, Vector3 normal, string sight)
        {
            int width = map.Width;
            int height = map.Height;

            Vector2 uv = getUV(normal, sight);

            uv.X *= width;
            uv.Y *= height;

            int u = Math.Max(0, Math.Min((int)uv.X, width - 1));
            int v = Math.Max(0, Math.Min((int)uv.Y, height - 1));


            var tempClr = map.GetPixel(u, v);
            return Calculations.clrToV3(tempClr);
        }

        private static Vector3 GetColorFromSkybox(Vector3 interpolatedNormal)
        {
            Vector3 clr = Vector3.Zero;
            var sight = getSightS(interpolatedNormal);
            if (sight == "+x")
            {
                clr = GetColorFromBitmap(cubeTextures[0], interpolatedNormal, sight);
            }
            else if (sight == "-x")
            {
                clr = GetColorFromBitmap(cubeTextures[1], interpolatedNormal, sight);
            }
            else if (sight == "+z")
            {
                clr = GetColorFromBitmap(cubeTextures[2], interpolatedNormal, sight);
            }
            else if (sight == "-z")
            {
                clr = GetColorFromBitmap(cubeTextures[3], interpolatedNormal, sight);
            }
            else if (sight == "+y")
            {
                clr = GetColorFromBitmap(cubeTextures[4], interpolatedNormal, sight);
            }
            else if (sight == "-y")
            {
                clr = GetColorFromBitmap(cubeTextures[5], interpolatedNormal, sight);
            }

            return clr;

        }


        public static unsafe void DrawFonkWithTextures(Bitmap _bitmap, int[][] _fArr, Vector4[] _modelVArr, Vector4[] _updateVArr, Bitmap diffuseMap, Bitmap specularMap, Bitmap normalMap, int[][] _fvtList, float[] _ws, Vector3[] _vtList, Vector3 angels)
        {
            BitmapData bData = _bitmap.LockBits(new Rectangle(0, 0, _bitmap.Width, _bitmap.Height),
                     ImageLockMode.ReadWrite, _bitmap.PixelFormat);
            Drawing.bitsPerPixel = (byte)Image.GetPixelFormatSize(bData.PixelFormat);
            var scan0 = (byte*)bData.Scan0;


            for (int j = 0; j < _fArr.Length; j++)
            {
                var temp = _modelVArr[_fArr[j][0] - 1];
                Vector3 n = new Vector3(temp.X, temp.Y, temp.Z);
                var normalCamView = Vector3.Normalize(Calculations.Camera - n);

                if (Vector3.Dot(Calculations.VPolygonNormals[j], normalCamView) > 0)
                {
                    var indexes = _fArr[j];
                    var tIndexes = _fvtList[j];

                    Vector4 f1 = _updateVArr[indexes[0] - 1];
                    Vector3 f1Vt = _vtList[tIndexes[0] - 1] / _ws[indexes[0] - 1];
                    Vector3 n1 = Calculations.VertexNormals[indexes[0] - 1];
                    Vector4 f1Mode = _modelVArr[indexes[0] - 1];

                    for (var i = 1; i <= indexes.Length - 2; i++)
                    {
                        Vector4 f2 = _updateVArr[indexes[i] - 1];
                        Vector4 f2Model = _modelVArr[indexes[i] - 1];
                        Vector3 f2Vt = _vtList[tIndexes[i] - 1] / _ws[indexes[i] - 1];

                        Vector4 f3 = _updateVArr[indexes[i + 1] - 1];
                        Vector4 f3Model = _modelVArr[indexes[i + 1] - 1];
                        Vector3 f3Vt = _vtList[tIndexes[i + 1] - 1] / _ws[indexes[i + 1] - 1];

                        Vector3 n2 = Calculations.VertexNormals[indexes[i] - 1];
                        Vector3 n3 = Calculations.VertexNormals[indexes[i + 1] - 1];

                        var minX = Math.Min(f1.X, Math.Min(f2.X, f3.X));
                        var maxX = Math.Max(f1.X, Math.Max(f2.X, f3.X));
                        var minY = Math.Min(f1.Y, Math.Min(f2.Y, f3.Y));
                        var maxY = Math.Max(f1.Y, Math.Max(f2.Y, f3.Y));

                        var startX = (int)Math.Ceiling(minX);
                        var endX = (int)Math.Floor(maxX);
                        var startY = (int)Math.Ceiling(minY);
                        var endY = (int)Math.Floor(maxY);

                        for (var y = startY; y <= endY; y++)
                        {
                            for (var x = startX; x <= endX; x++)
                            {
                                if (Drawing.IsPointInPolygon(x, y, _fArr[j], _updateVArr))
                                {
                                    Vector3 barycentricCoords = Translations.CalculateBarycentricCoordinates(x, y, f1, f2, f3);

                                    var z = barycentricCoords.X * f1.Z + barycentricCoords.Y * f2.Z +
                                            barycentricCoords.Z * f3.Z;


                                    Vector3 interpolatedNormal = barycentricCoords.X * n1 + barycentricCoords.Y * n2 +
                                                                 barycentricCoords.Z * n3;

                                    interpolatedNormal = Vector3.Normalize(interpolatedNormal);

                                    Vector4 frag = barycentricCoords.X * f1Mode + barycentricCoords.Y * f2Model +
                                                   barycentricCoords.Z * f3Model;

                                    Vector3 fragV3 = new Vector3(frag.X, frag.Y, frag.Z);

                                    Vector3 textureCoord = barycentricCoords.X * f1Vt + barycentricCoords.Y * f2Vt +
                                       barycentricCoords.Z * f3Vt;

                                    var lightDir = Vector3.Normalize(Calculations.LambertLight - fragV3);
                                    var cameraDir = Vector3.Normalize(Calculations.Camera - fragV3);

                                    var normal = interpolatedNormal;

                                    Vector3 Ia = new Vector3();
                                    Vector3 Is = new Vector3();

                                    if (diffuseMap != null)
                                    {
                                        textureCoord.X *= diffuseMap.Width;
                                        textureCoord.Y *= diffuseMap.Height;

                                        textureCoord /= textureCoord.Z;

                                        int u = Math.Max(0, Math.Min((int)textureCoord.X, diffuseMap.Height - 1));
                                        int v = Math.Max(0, Math.Min(diffuseMap.Width - (int)textureCoord.Y, diffuseMap.Width - 1));

                                        Ia = Calculations.clrToV3(diffuseMap.GetPixel(u, v));


                                        if (specularMap != null)
                                        {
                                            //specular
                                            Is = Calculations.clrToV3(specularMap.GetPixel(u, v));
                                        }

                                        if (normalMap != null)
                                        {
                                            //normal
                                            Color normalColor = normalMap.GetPixel(u, v);
                                            float r = normalColor.R / 255f;
                                            float g = normalColor.G / 255f;
                                            float b = normalColor.B / 255f;
                                            normal = new Vector3(
                                                (r * 2f) - 1f,
                                                (g * 2f) - 1f,
                                                (b * 2f) - 1f
                                                );

                                            var rotX = Matrix4x4.CreateRotationX(angels.X);
                                            var rotY = Matrix4x4.CreateRotationY(angels.Y);
                                            var rotZ = Matrix4x4.CreateRotationZ(angels.Z);

                                            normal = Vector3.Transform(normal, rotX);
                                            normal = Vector3.Transform(normal, rotY);
                                            normal = Vector3.Transform(normal, rotZ);
                                        }

                                    }

                                    var reflection = Vector3.Reflect(-cameraDir, normal);

                                    // var clr = GetColorFromSkybox(reflection);
                                    // clr = Drawing.applyGamma(clr, 2.2f);


                                    Ia = Drawing.applyGamma(Ia, 2.2f);
                                    Is = Drawing.applyGamma(Is, 2.2f);


                                    var phongBg = Calculations.CalculatePhong(Calculations.Ka, Calculations.multiplyClrs(Calculations.Ia, Ia));
                                    var diffuse = Calculations.CalcDiffuseLight(normal, lightDir, Calculations.multiplyClrs(Calculations.Id, Ia), Calculations.Kd);
                                    var spec = Calculations.CalculateSpecular(normal, cameraDir, lightDir, Calculations.Ks, Calculations.multiplyClrs(Calculations.Is, Is));

                                    var phongClr = phongBg + diffuse + spec;

                                    phongClr.X = phongClr.X > 1 ? 1 : phongClr.X;
                                    phongClr.Y = phongClr.Y > 1 ? 1 : phongClr.Y;
                                    phongClr.Z = phongClr.Z > 1 ? 1 : phongClr.Z;

                                    phongClr = Drawing.applyGamma(phongClr, 0.454545f);

                                    Drawing.DrawPoint(scan0, phongClr * 255, x, y, z);

                                }
                            }
                        }
                    }

                }
            }

            _bitmap.UnlockBits(bData);
        }



        public static unsafe void DrawByPhongWithCube(Bitmap _bitmap, int[][] _fArr, Vector4[] _modelVArr, Vector4[] _updateVArr, Bitmap diffuseMap, Bitmap specularMap, Bitmap normalMap, int[][] _fvtList, float[] _ws, Vector3[] _vtList, Vector3 angels)
        {
            bData = _bitmap.LockBits(new Rectangle(0, 0, _bitmap.Width, _bitmap.Height), ImageLockMode.ReadWrite, _bitmap.PixelFormat);
            bitsPerPixel = (byte)Image.GetPixelFormatSize(bData.PixelFormat);
            var scan0 = (byte*)bData.Scan0;
            Vector3 Ia = new Vector3();                                    
            Vector3 Is = new Vector3();
            Vector4 f1, f1Mode, f2Model, f2,f3, f3Model;
            Vector3 f1Vt, n1, f2Vt,f3Vt, n2, n3, n;//попытка в оптимизацию
            for (int j = 0; j < _fArr.Length; j++)
            {
                var temp = _modelVArr[_fArr[j][0] - 1];
                n = new Vector3(temp.X, temp.Y, temp.Z);
                var normalCamView = Vector3.Normalize(Calculations.Camera - n);

                if (Vector3.Dot(Calculations.VPolygonNormals[j], normalCamView) > 0)
                {
                    var indexes = _fArr[j];
                    var tIndexes = _fvtList[j];

                    f1 = _updateVArr[indexes[0] - 1];
                    f1Vt = _vtList[tIndexes[0] - 1] / _ws[indexes[0] - 1];
                    n1 = Calculations.VertexNormals[indexes[0] - 1];
                    f1Mode = _modelVArr[indexes[0] - 1];

                    for (var i = 1; i <= indexes.Length - 2; i++)
                    {
                        f2 = _updateVArr[indexes[i] - 1];
                        f2Model = _modelVArr[indexes[i] - 1];
                        f2Vt = _vtList[tIndexes[i] - 1] / _ws[indexes[i] - 1];

                        f3 = _updateVArr[indexes[i + 1] - 1];
                        f3Model = _modelVArr[indexes[i + 1] - 1];
                        f3Vt = _vtList[tIndexes[i + 1] - 1] / _ws[indexes[i + 1] - 1];

                        n2 = Calculations.VertexNormals[indexes[i] - 1];
                        n3 = Calculations.VertexNormals[indexes[i + 1] - 1];

                        var minX = Math.Min(f1.X, Math.Min(f2.X, f3.X));
                        var maxX = Math.Max(f1.X, Math.Max(f2.X, f3.X));
                        var minY = Math.Min(f1.Y, Math.Min(f2.Y, f3.Y));
                        var maxY = Math.Max(f1.Y, Math.Max(f2.Y, f3.Y));

                        var startX = (int)Math.Ceiling(minX);
                        var endX = (int)Math.Floor(maxX);
                        var startY = (int)Math.Ceiling(minY);
                        var endY = (int)Math.Floor(maxY);

                        Vector3 barycentricCoords, interpolatedNormal, fragV3, textureCoord;//вынесла, чтобы меньше было выделений памяти
                        Vector4 frag;
                        for (var y = startY; y <= endY; y++)
                        {
                            for (var x = startX; x <= endX; x++)
                            {
                                if (Drawing.IsPointInPolygon(x, y, _fArr[j], _updateVArr))
                                {
                                    barycentricCoords = Translations.CalculateBarycentricCoordinates(x, y, f1, f2, f3);

                                    var z = barycentricCoords.X * f1.Z + barycentricCoords.Y * f2.Z +
                                            barycentricCoords.Z * f3.Z;


                                    interpolatedNormal = barycentricCoords.X * n1 + barycentricCoords.Y * n2 +
                                                                 barycentricCoords.Z * n3;

                                    interpolatedNormal = Vector3.Normalize(interpolatedNormal);

                                    frag = barycentricCoords.X * f1Mode + barycentricCoords.Y * f2Model +
                                                   barycentricCoords.Z * f3Model;

                                    fragV3 = new Vector3(frag.X, frag.Y, frag.Z);

                                    textureCoord = barycentricCoords.X * f1Vt + barycentricCoords.Y * f2Vt +
                                       barycentricCoords.Z * f3Vt;

                                    var lightDir = Vector3.Normalize(Calculations.LambertLight - fragV3);
                                    var cameraDir = Vector3.Normalize(Calculations.Camera - fragV3);

                                    var normal = interpolatedNormal;

                                    
                                    Ia = Calculations.Ia;
                                    Is = Calculations.Is;
                                    
                                    int u =0;
                                    int v =0;

                                    if (diffuseMap != null)
                                    {
                                        textureCoord.X *= diffuseMap.Width;
                                        textureCoord.Y *= diffuseMap.Height;

                                        textureCoord /= textureCoord.Z;

                                        u = Math.Max(0, Math.Min((int)textureCoord.X, diffuseMap.Height - 1));
                                        v = Math.Max(0, Math.Min(diffuseMap.Width - (int)textureCoord.Y, diffuseMap.Width - 1));

                                        Ia = Calculations.clrToV3(diffuseMap.GetPixel(u, v));
                                    }

                                    if (specularMap != null)
                                    {
                                        /*textureCoord.X *= specularMap.Width;//закомментила, т.к. обычно в наборе идут сразу 3 карты, если какой-то карты нет, то тупо надо раскомментить
                                        textureCoord.Y *= specularMap.Height;

                                        textureCoord /= textureCoord.Z;

                                        u = Math.Max(0, Math.Min((int)textureCoord.X, specularMap.Height - 1));
                                        v = Math.Max(0, Math.Min(specularMap.Width - (int)textureCoord.Y, specularMap.Width - 1));
                                        //specular*/
                                        Is = Calculations.clrToV3(specularMap.GetPixel(u, v));
                                    }

                                    if (normalMap != null)
                                    {
                                        /*textureCoord.X *= normalMap.Width;
                                        textureCoord.Y *= normalMap.Height;

                                        textureCoord /= textureCoord.Z;

                                        u = Math.Max(0, Math.Min((int)textureCoord.X, normalMap.Height - 1));
                                        v = Math.Max(0, Math.Min(normalMap.Width - (int)textureCoord.Y, normalMap.Width - 1));
                                        //normal*/
                                        Color normalColor = normalMap.GetPixel(u, v);
                                        float r = normalColor.R / 255f;
                                        float g = normalColor.G / 255f;
                                        float b = normalColor.B / 255f;
                                        normal = new Vector3(
                                            (r * 2f) - 1f,
                                            (g * 2f) - 1f,
                                            (b * 2f) - 1f
                                            );

                                        var rotX = Matrix4x4.CreateRotationX(angels.X);
                                        var rotY = Matrix4x4.CreateRotationY(angels.Y);
                                        var rotZ = Matrix4x4.CreateRotationZ(angels.Z);

                                        normal = Vector3.Transform(normal, rotX);
                                        normal = Vector3.Transform(normal, rotY);
                                        normal = Vector3.Transform(normal, rotZ);
                                    }

                                    

                                    var reflection = Vector3.Reflect(-cameraDir, normal);

                                    var clr = GetColorFromSkybox(reflection);
                                    clr = Drawing.applyGamma(clr, 2.2f);


                                    Ia = Drawing.applyGamma(Ia, 2.2f);
                                    Is = Drawing.applyGamma(Is, 2.2f);


                                    var phongBg = Calculations.CalculatePhong(Calculations.Ka, Calculations.multiplyClrs(Calculations.Ia, Ia));
                                    var diffuse = Calculations.CalcDiffuseLight(normal, lightDir, Calculations.multiplyClrs(clr, Ia), Calculations.Kd);
                                    var spec = Calculations.CalculateSpecular(normal, cameraDir, lightDir, Calculations.Ks, Calculations.multiplyClrs(clr, Is));

                                    var phongClr = phongBg + diffuse + spec;

                                    phongClr.X = phongClr.X > 1 ? 1 : phongClr.X;
                                    phongClr.Y = phongClr.Y > 1 ? 1 : phongClr.Y;
                                    phongClr.Z = phongClr.Z > 1 ? 1 : phongClr.Z;

                                    phongClr = Drawing.applyGamma(phongClr, 0.454545f);

                                    Drawing.DrawPoint(scan0, phongClr * 255, x, y, z);

                                }
                            }
                        }
                    }

                }
            }

            _bitmap.UnlockBits(bData);
        }
        public static unsafe void DrawByPhonk(Bitmap _bitmap, int[][] _fArr, Vector4[] _modelVArr, Vector4[] _updateVArr, Vector3 model_color)
        {
            bData = _bitmap.LockBits(new Rectangle(0, 0, _bitmap.Width, _bitmap.Height), ImageLockMode.ReadWrite, _bitmap.PixelFormat);
            bitsPerPixel = (byte)Image.GetPixelFormatSize(bData.PixelFormat);
            bWidth = _bitmap.Width;
            bHeight = _bitmap.Height;
            byte* scan0 = (byte*)bData.Scan0;

            for (int j = 0; j < _fArr.Length; j++)
            {
                var temp = _modelVArr[_fArr[j][0] - 1];
                Vector3 n = new Vector3(temp.X, temp.Y, temp.Z);
                var normalCamView = Vector3.Normalize(Calculations.Camera - n);

                if (Vector3.Dot(Calculations.VPolygonNormals[j], normalCamView) > 0)
                {
                    RasterizePointsPhonk(scan0, _fArr[j], _updateVArr, _modelVArr);
                }
            }

            _bitmap.UnlockBits(bData);
        }

        private static Vector3 multiplyClr(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z);
        }

        public static Vector3 applyGamma(Vector3 color, float gamma)
        {
            color.X = (float)Math.Pow(color.X, gamma);
            color.Y = (float)Math.Pow(color.Y, gamma);
            color.Z = (float)Math.Pow(color.Z, gamma);

            return color;
        }

        public static bool IsPointInPolygon(int x, int y, int[] polygon, Vector4[] updateV)
        {
            int numPoints = polygon.Length;
            bool inside = false;

            for (int i = 0, j = numPoints - 1; i < numPoints; j = i++)
            {
                Vector4 vertex1 = updateV[polygon[i] - 1];
                Vector4 vertex2 = updateV[polygon[j] - 1];

                if (((vertex1.Y > y) != (vertex2.Y > y)) && (x < (vertex2.X - vertex1.X) * (y - vertex1.Y) / (vertex2.Y - vertex1.Y) + vertex1.X))
                {
                    inside = !inside;
                }
            }

            return inside;
        }

        static unsafe void RasterizePointsPhonk(byte* scan0, int[] polygon, Vector4[] updateV, Vector4[] modelVArr)
        {
            float minX = updateV[polygon[0] - 1].X;
            float minY = updateV[polygon[0] - 1].Y;
            float maxX = updateV[polygon[0] - 1].X;
            float maxY = updateV[polygon[0] - 1].Y;
            Vector4 point;
            for (int i = 1; i < polygon.Length; i++)
            {
                point = updateV[polygon[i] - 1];
                minX = Math.Min(minX, point.X);
                minY = Math.Min(minY, point.Y);
                maxX = Math.Max(maxX, point.X);
                maxY = Math.Max(maxY, point.Y);
            }
            for (var i = 1; i <= polygon.Length - 2; i++)
            {
                for (int x = (int)minX; x <= (int)maxX; x++)
                {
                    for (int y = (int)minY; y <= (int)maxY; y++)
                    {
                        if (IsPointInPolygon(x, y, polygon, updateV))
                        {
                            Vector3 barycentricCoords = Translations.CalculateBarycentricCoordinates(x, y, updateV[polygon[0] - 1], updateV[polygon[i] - 1], updateV[polygon[i + 1] - 1]);
                            var z = barycentricCoords.X * updateV[polygon[0] - 1].Z + barycentricCoords.Y * updateV[polygon[i] - 1].Z + barycentricCoords.Z * updateV[polygon[i + 1] - 1].Z;

                            Vector3 interpolatedNormal = barycentricCoords.X * Calculations.VertexNormals[polygon[0] - 1] + barycentricCoords.Y * Calculations.VertexNormals[polygon[i] - 1] + barycentricCoords.Z * Calculations.VertexNormals[polygon[i + 1] - 1];
                            interpolatedNormal = Vector3.Normalize(interpolatedNormal);
                            Vector3 frag = Calculations.v4ToV3(barycentricCoords.X * modelVArr[polygon[0] - 1] + barycentricCoords.Y * modelVArr[polygon[i] - 1] + barycentricCoords.Z * modelVArr[polygon[i + 1] - 1]);
                            Vector3 lightDir = Vector3.Normalize(Calculations.LambertLight - frag);
                            Vector3 cameraDir = Vector3.Normalize(Calculations.Camera - frag);

                            Vector3 phongBg = Calculations.CalcPhongBgLight(Calculations.Ka, Calculations.Ia);
                            Vector3 diffuse = Calculations.CalcPhongDiffuseLight(interpolatedNormal, lightDir, Calculations.Id, Calculations.Kd);
                            Vector3 mirror = Calculations.CalcPhongMirrorLight(interpolatedNormal, cameraDir, lightDir, Calculations.Ks, Calculations.Is);

                            Vector3 phongClr = phongBg + diffuse + mirror;
                            // phongClr = multiplyClr(phongClr, model_color);

                            phongClr.X = phongClr.X > 1 ? 1 : phongClr.X;
                            phongClr.Y = phongClr.Y > 1 ? 1 : phongClr.Y;
                            phongClr.Z = phongClr.Z > 1 ? 1 : phongClr.Z;

                            phongClr = EnvironmentInfo.ApplyGamma(phongClr, 0.454545f);

                            DrawPoint(scan0, phongClr * 255, x, y, z);
                        }
                    }
                }
            }
        }

        static unsafe void RasterizePoints(Vector3 clr, byte* scan0, int[] polygon, Vector4[] updateV)
        {
            float minX = updateV[polygon[0] - 1].X;
            float minY = updateV[polygon[0] - 1].Y;
            float maxX = updateV[polygon[0] - 1].X;
            float maxY = updateV[polygon[0] - 1].Y;
            Vector4 point;
            for (int i = 1; i < polygon.Length; i++)
            {
                point = updateV[polygon[i] - 1];
                minX = Math.Min(minX, point.X);
                minY = Math.Min(minY, point.Y);
                maxX = Math.Max(maxX, point.X);
                maxY = Math.Max(maxY, point.Y);
            }

            for (int x = (int)minX; x <= (int)maxX; x++)
            {
                for (int y = (int)minY; y <= (int)maxY; y++)
                {
                    if (IsPointInPolygon(x, y, polygon, updateV))
                    {
                        Vector3 barycentricCoords = Translations.CalculateBarycentricCoordinates((int)x, (int)y, updateV[polygon[0] - 1], updateV[polygon[1] - 1], updateV[polygon[2] - 1]);
                        var z = barycentricCoords.X * updateV[polygon[0] - 1].Z + barycentricCoords.Y * updateV[polygon[1] - 1].Z +
                                barycentricCoords.Z * updateV[polygon[2] - 1].Z;
                        DrawPoint(scan0, clr, x, y, z);
                    }
                }
            }

        }

        public static unsafe void DrawByLambert(Bitmap _bitmap, Vector3 clr, int[][] fArr, Vector4[] modelVArr, Vector4[] updateVArr)
        {

            bData = _bitmap.LockBits(new Rectangle(0, 0, _bitmap.Width, _bitmap.Height), ImageLockMode.ReadWrite, _bitmap.PixelFormat);
            bitsPerPixel = (byte)Image.GetPixelFormatSize(bData.PixelFormat);
            bWidth = _bitmap.Width;
            bHeight = _bitmap.Height;
            byte* scan0 = (byte*)bData.Scan0;

            for (var j = 0; j < fArr.Length; j++)
            {
                Vector4 temp = modelVArr[fArr[j][0] - 1];
                Vector3 n = new Vector3(temp.X, temp.Y, temp.Z);

                if (Vector3.Dot(Calculations.VPolygonNormals[j], Calculations.Camera - n) > 0)
                {
                    float intensity = Math.Abs(Vector3.Dot(Calculations.VPolygonNormals[j],
                        Vector3.Normalize((Calculations.LambertLight - n))));
                    Vector3 temp_clr = clr * 255 * intensity;
                    RasterizePoints(temp_clr, scan0, fArr[j], updateVArr);
                }
            }

            _bitmap.UnlockBits(bData);
        }

        public static unsafe void DrawOnlyGrid(Bitmap _bitmap, Vector3 clr, int[][] fArr, Vector4[] updateVArr)
        {
            clr *= 255;
            bData = _bitmap.LockBits(new Rectangle(0, 0, _bitmap.Width, _bitmap.Height), ImageLockMode.ReadWrite, _bitmap.PixelFormat);
            bitsPerPixel = (byte)Image.GetPixelFormatSize(bData.PixelFormat);
            bWidth = _bitmap.Width;
            bHeight = _bitmap.Height;
            byte* scan0 = (byte*)bData.Scan0;
            for (int index = 0; index < fArr.Length; index++)
            {
                int[] polygon = fArr[index];
                for (int i = 0; i < polygon.Length - 1; i++)
                {
                    int index1 = i;
                    int index2 = i + 1;
                    DrawLineBresenham(scan0, index1, index2, index, updateVArr, fArr, clr);
                }
                int lastIndex = polygon.Length - 1;
                int firstIndex = 0;
                DrawLineBresenham(scan0, firstIndex, lastIndex, index, updateVArr, fArr, clr);
            }
            _bitmap.UnlockBits(bData);
        }

        public static unsafe void DrawLineBresenham(byte* scan0, int pt1, int pt2, int index, Vector4[] updateV, int[][] fArr, Vector3 clr)
        {
            var x0 = (int)Math.Round(updateV[fArr[index][pt1] - 1].X);
            var y0 = (int)Math.Round(updateV[fArr[index][pt1] - 1].Y);
            var x1 = (int)Math.Round(updateV[fArr[index][pt2] - 1].X);
            var y1 = (int)Math.Round(updateV[fArr[index][pt2] - 1].Y);

            var dx = Math.Abs(x1 - x0);
            var dy = Math.Abs(y1 - y0);
            var sx = x0 < x1 ? 1 : -1;
            var sy = y0 < y1 ? 1 : -1;
            var err = dx - dy;

            while (true)
            {
                if (x0 > 0 && x0 + 1 < bWidth && y0 > 0 && y0 + 1 < bHeight)
                {
                    var data = scan0 + y0 * bData.Stride + x0 * bitsPerPixel / 8;
                    if (data != null)
                    {
                        data[0] = (byte)clr.Z;
                        data[1] = (byte)clr.Y;
                        data[2] = (byte)clr.X;
                    }
                }

                if (x0 == x1 && y0 == y1)
                    break;
                var e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }

                if (e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
        }

        public static unsafe void DrawPoint(byte* scan0, Vector3 cl, int x, int y, float z)
        {

            if (x > 0 && x + 1 < bWidth && y > 0 && y + 1 < bHeight && zBuffer[x][y] > z)
            {
                zBuffer[x][y] = z;

                var data = scan0 + y * bData.Stride + x * bitsPerPixel / 8;
                if (data != null)
                {
                    // Примените интенсивность освещения к цвету пикселя
                    data[0] = (byte)cl.Z;
                    data[1] = (byte)cl.Y;
                    data[2] = (byte)cl.X;
                }
            }
        }
    }
}