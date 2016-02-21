using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;


namespace Kocka
{
    class Stvorec3D
    {
        private float[] kocka;
        private float[] farba;
        private float[] normaly;
        private int[] VBO;
        private int[] VAO;
        private bool vtip;
        private Shaders.Shader VertexShader, FragmentShader;
        private Shaders.ShaderProgram spMain;
        private Matrix4 modelViewMatrix, projectionMatrix;
        private int width, height;
        private float scale;
        private Matrix4 Current, ScaleMatrix, TranslationMatrix, RotationMatrix, MatrixStore_Translations, MatrixStore_Rotations, MatrixStore_Scales;
        Kamera BigBrother; //iba pokusne, neriesil som nedostatky, chcel som sa len hybat
        DirectionalLight light;
        Material material;

        public Stvorec3D(bool trojuholniky, int w, int h, float s)
        {
            VBO = new int[3];
            VAO = new int[1];
            VertexShader = new Shaders.Shader();
            FragmentShader = new Shaders.Shader();
            spMain = new Shaders.ShaderProgram();
            BigBrother = new Kamera(new Vector3(0.0f, 0.0f, 3.5f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
            //svetlo - smer,ambient,specular,diffuse
            light = new DirectionalLight(new Vector3(0.0f, 0.0f, -1.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f));
            //material - ambient,specular,diffuse,koeficienty - ambient, specular, diffuse, shininess 
            material = new Material(new Vector3(0.5f, 0.5f, 0.5f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f), 0.30f, 1.10f, 0.420f, 8);
           
            vtip = trojuholniky;
            width = w;
            height = h;
            scale = s;
            Current = Matrix4.Identity;

            if (vtip)//steny zlozene z trojuholnikov
            {
                kocka = new float[108];
                farba = new float[108];
                normaly = new float[108];

                #region normaly
                //predna
                normaly[0] = 0.0f; normaly[1] = 0.0f; normaly[2] = 1.0f;
                normaly[3] = 0.0f; normaly[4] = 0.0f; normaly[5] = 1.0f;
                normaly[6] = 0.0f; normaly[7] = 0.0f; normaly[8] = 1.0f;
                normaly[9] = 0.0f; normaly[10] = 0.0f; normaly[11] = 1.0f;
                normaly[12] = 0.0f; normaly[13] = 0.0f; normaly[14] = 1.0f;
                normaly[15] = 0.0f; normaly[16] = 0.0f; normaly[17] = 1.0f;

                //zadna stena
                normaly[18] = -0.0f; normaly[19] = -0.0f; normaly[20] = -1.0f;
                normaly[21] = 0.0f; normaly[22] = -0.0f; normaly[23] = -1.0f;
                normaly[24] = 0.0f; normaly[25] = 0.0f; normaly[26] = -1.0f;
                normaly[27] = -0.0f; normaly[28] = -0.0f; normaly[29] = -1.0f;
                normaly[30] = 0.0f; normaly[31] = 0.0f; normaly[32] = -1.0f;
                normaly[33] = -0.0f; normaly[34] = 0.0f; normaly[35] = -1.0f;

                //lava stena
                normaly[36] = -1.0f; normaly[37] = -0.0f; normaly[38] = 0.0f;
                normaly[39] = -1.0f; normaly[40] = -0.0f; normaly[41] = -0.0f;
                normaly[42] = -1.0f; normaly[43] = 0.0f; normaly[44] = -0.0f;
                normaly[45] = -1.0f; normaly[46] = -0.0f; normaly[47] = 0.0f;
                normaly[48] = -1.0f; normaly[49] = 0.0f; normaly[50] = -0.0f;
                normaly[51] = -1.0f; normaly[52] = 0.0f; normaly[53] = 0.0f;

                //prava stena
                normaly[54] = 1.0f; normaly[55] = 0.0f; normaly[56] = 0.0f;
                normaly[57] = 1.0f; normaly[58] = 0.0f; normaly[59] = 0.0f;
                normaly[60] = 1.0f; normaly[61] = 0.0f; normaly[62] = 0.0f;
                normaly[63] = 1.0f; normaly[64] = 0.0f; normaly[65] = 0.0f;
                normaly[66] = 1.0f; normaly[67] = 0.0f; normaly[68] = 0.0f;
                normaly[69] = 1.0f; normaly[70] = 0.0f; normaly[71] = 0.0f;

                //spodna stena
                normaly[72] = 0.0f; normaly[73] = -1.0f; normaly[74] = 0.0f;
                normaly[75] = 0.0f; normaly[76] = -1.0f; normaly[77] = 0.0f;
                normaly[78] = 0.0f; normaly[79] = -1.0f; normaly[80] = 0.0f;
                normaly[81] = 0.0f; normaly[82] = -1.0f; normaly[83] = 0.0f;
                normaly[84] = 0.0f; normaly[85] = -1.0f; normaly[86] = 0.0f;
                normaly[87] = 0.0f; normaly[88] = -1.0f; normaly[89] = 0.0f;

                //vrchna stena
                normaly[90] = 0.0f; normaly[91] = 1.0f; normaly[92] = 0.0f;
                normaly[93] = 0.0f; normaly[94] = 1.0f; normaly[95] = 0.0f;
                normaly[96] = 0.0f; normaly[97] = 1.0f; normaly[98] = 0.0f;
                normaly[99] = 0.0f; normaly[100] = 1.0f; normaly[101] = 0.0f;
                normaly[102] = 0.0f; normaly[103] = 1.0f; normaly[104] = 0.0f;
                normaly[105] = 0.0f; normaly[106] = 1.0f; normaly[107] = 0.0f;

                #endregion

                #region vrcholy
                //predna stena
                kocka[0] = -1.0f; kocka[1] = -1.0f; kocka[2] = 1.0f;
                kocka[3] = 1.0f; kocka[4] = -1.0f; kocka[5] = 1.0f;
                kocka[6] = 1.0f; kocka[7] = 1.0f; kocka[8] = 1.0f;
                kocka[9] = -1.0f; kocka[10] = -1.0f; kocka[11] = 1.0f;
                kocka[12] = 1.0f; kocka[13] = 1.0f; kocka[14] = 1.0f;
                kocka[15] = -1.0f; kocka[16] = 1.0f; kocka[17] = 1.0f;

                //zadna stena
                kocka[18] = -1.0f; kocka[19] = -1.0f; kocka[20] = -1.0f;
                kocka[21] = 1.0f; kocka[22] = -1.0f; kocka[23] = -1.0f;
                kocka[24] = 1.0f; kocka[25] = 1.0f; kocka[26] = -1.0f;
                kocka[27] = -1.0f; kocka[28] = -1.0f; kocka[29] = -1.0f;
                kocka[30] = 1.0f; kocka[31] = 1.0f; kocka[32] = -1.0f;
                kocka[33] = -1.0f; kocka[34] = 1.0f; kocka[35] = -1.0f;

                //lava stena
                kocka[36] = -1.0f; kocka[37] = -1.0f; kocka[38] = 1.0f;
                kocka[39] = -1.0f; kocka[40] = -1.0f; kocka[41] = -1.0f;
                kocka[42] = -1.0f; kocka[43] = 1.0f; kocka[44] = -1.0f;
                kocka[45] = -1.0f; kocka[46] = -1.0f; kocka[47] = 1.0f;
                kocka[48] = -1.0f; kocka[49] = 1.0f; kocka[50] = -1.0f;
                kocka[51] = -1.0f; kocka[52] = 1.0f; kocka[53] = 1.0f;

                //prava stena
                kocka[54] = 1.0f; kocka[55] = -1.0f; kocka[56] = 1.0f;
                kocka[57] = 1.0f; kocka[58] = -1.0f; kocka[59] = -1.0f;
                kocka[60] = 1.0f; kocka[61] = 1.0f; kocka[62] = -1.0f;
                kocka[63] = 1.0f; kocka[64] = -1.0f; kocka[65] = 1.0f;
                kocka[66] = 1.0f; kocka[67] = 1.0f; kocka[68] = -1.0f;
                kocka[69] = 1.0f; kocka[70] = 1.0f; kocka[71] = 1.0f;

                //spodna stena
                kocka[72] = -1.0f; kocka[73] = -1.0f; kocka[74] = 1.0f;
                kocka[75] = 1.0f; kocka[76] = -1.0f; kocka[77] = 1.0f;
                kocka[78] = 1.0f; kocka[79] = -1.0f; kocka[80] = -1.0f;
                kocka[81] = -1.0f; kocka[82] = -1.0f; kocka[83] = 1.0f;
                kocka[84] = 1.0f; kocka[85] = -1.0f; kocka[86] = -1.0f;
                kocka[87] = -1.0f; kocka[88] = -1.0f; kocka[89] = -1.0f;

                //vrchna stena
                kocka[90] = -1.0f; kocka[91] = 1.0f; kocka[92] = 1.0f;
                kocka[93] = 1.0f; kocka[94] = 1.0f; kocka[95] = 1.0f;
                kocka[96] = 1.0f; kocka[97] = 1.0f; kocka[98] = -1.0f;
                kocka[99] = -1.0f; kocka[100] = 1.0f; kocka[101] = 1.0f;
                kocka[102] = 1.0f; kocka[103] = 1.0f; kocka[104] = -1.0f;
                kocka[105] = -1.0f; kocka[106] = 1.0f; kocka[107] = -1.0f;
                #endregion

                #region farby
                Random X = new Random();
                //predna stena
                farba[0] = (float)X.NextDouble(); farba[1] = 0.0f; farba[2] = 0.0f;
                farba[3] = (float)X.NextDouble(); farba[4] = 0.0f; farba[5] = 0.0f;
                farba[6] = (float)X.NextDouble(); farba[7] = 0.0f; farba[8] = 0.0f;
                farba[9] = (float)X.NextDouble(); farba[10] = 0.0f; farba[11] = 0.0f;
                farba[12] = (float)X.NextDouble(); farba[13] = 0.0f; farba[14] = 0.0f;
                farba[15] = (float)X.NextDouble(); farba[16] = 0.0f; farba[17] = 0.0f;

                //zadna stena
                farba[18] = 0.0f; farba[19] = (float)X.NextDouble(); farba[20] = 0.0f;
                farba[21] = 0.0f; farba[22] = (float)X.NextDouble(); farba[23] = 0.0f;
                farba[24] = 0.0f; farba[25] = (float)X.NextDouble(); farba[26] = 0.0f;
                farba[27] = 0.0f; farba[28] = (float)X.NextDouble(); farba[29] = 0.0f;
                farba[30] = 0.0f; farba[31] = (float)X.NextDouble(); farba[32] = 0.0f;
                farba[33] = 0.0f; farba[34] = (float)X.NextDouble(); farba[35] = 0.0f;

                //lava stena
                farba[36] = 0.0f; farba[37] = 0.0f; farba[38] = (float)X.NextDouble();
                farba[39] = 0.0f; farba[40] = 0.0f; farba[41] = (float)X.NextDouble();
                farba[42] = 0.0f; farba[43] = 0.0f; farba[44] = (float)X.NextDouble();
                farba[45] = 0.0f; farba[46] = 0.0f; farba[47] = (float)X.NextDouble();
                farba[48] = 0.0f; farba[49] = 0.0f; farba[50] = (float)X.NextDouble();
                farba[51] = 0.0f; farba[52] = 0.0f; farba[53] = (float)X.NextDouble();

                //prava stena
                farba[54] = (float)X.NextDouble(); farba[55] = (float)X.NextDouble(); farba[56] = 0.0f;
                farba[57] = (float)X.NextDouble(); farba[58] = (float)X.NextDouble(); farba[59] = 0.0f;
                farba[60] = (float)X.NextDouble(); farba[61] = (float)X.NextDouble(); farba[62] = 0.0f;
                farba[63] = (float)X.NextDouble(); farba[64] = (float)X.NextDouble(); farba[65] = 0.0f;
                farba[66] = (float)X.NextDouble(); farba[67] = (float)X.NextDouble(); farba[68] = 0.0f;
                farba[69] = (float)X.NextDouble(); farba[70] = (float)X.NextDouble(); farba[71] = 0.0f;

                //spodna stena
                farba[72] = 0.0f; farba[73] = (float)X.NextDouble(); farba[74] = (float)X.NextDouble();
                farba[75] = 0.0f; farba[76] = (float)X.NextDouble(); farba[77] = (float)X.NextDouble();
                farba[78] = 0.0f; farba[79] = (float)X.NextDouble(); farba[80] = (float)X.NextDouble();
                farba[81] = 0.0f; farba[82] = (float)X.NextDouble(); farba[83] = (float)X.NextDouble();
                farba[84] = 0.0f; farba[85] = (float)X.NextDouble(); farba[86] = (float)X.NextDouble();
                farba[87] = 0.0f; farba[88] = (float)X.NextDouble(); farba[89] = (float)X.NextDouble();

                //vrchna stena
                farba[90] = (float)X.NextDouble(); farba[91] = 0.0f; farba[92] = (float)X.NextDouble();
                farba[93] = (float)X.NextDouble(); farba[94] = 0.0f; farba[95] = (float)X.NextDouble();
                farba[96] = (float)X.NextDouble(); farba[97] = 0.0f; farba[98] = (float)X.NextDouble();
                farba[99] = (float)X.NextDouble(); farba[100] = 0.0f; farba[101] = (float)X.NextDouble();
                farba[102] = (float)X.NextDouble(); farba[103] = 0.0f; farba[104] = (float)X.NextDouble();
                farba[105] = (float)X.NextDouble(); farba[106] = 0.0f; farba[107] = (float)X.NextDouble();

                ////predna stena
                //farba[0] = 1.0f; farba[1] = 0.0f; farba[2] = 0.0f;
                //farba[3] = 0.0f; farba[4] = 1.0f; farba[5] = 0.0f;
                //farba[6] = 0.0f; farba[7] = 0.0f; farba[8] = 1.0f;
                //farba[9] = 1.0f; farba[10] = 0.0f; farba[11] = 0.0f;
                //farba[12] = 0.0f; farba[13] = 0.0f; farba[14] = 1.0f;
                //farba[15] = 0.0f; farba[16] = 1.0f; farba[17] = 0.0f;

                ////zadna stena
                //farba[18] = 0.0f; farba[19] = 0.0f; farba[20] = 1.0f;
                //farba[21] = 1.0f; farba[22] = 0.0f; farba[23] = 0.0f;
                //farba[24] = 0.0f; farba[25] = 1.0f; farba[26] = 0.0f;
                //farba[27] = 0.0f; farba[28] = 0.0f; farba[29] = 1.0f;
                //farba[30] = 0.0f; farba[31] = 1.0f; farba[32] = 0.0f;
                //farba[33] = 1.0f; farba[34] = 0.0f; farba[35] = 0.0f;

                ////lava stena
                //farba[36] = 1.0f; farba[37] = 0.0f; farba[38] = 0.0f;
                //farba[39] = 0.0f; farba[40] = 0.0f; farba[41] = 1.0f;
                //farba[42] = 0.0f; farba[43] = 1.0f; farba[44] = 0.0f;
                //farba[45] = 1.0f; farba[46] = 0.0f; farba[47] = 0.0f;
                //farba[48] = 0.0f; farba[49] = 1.0f; farba[50] = 0.0f;
                //farba[51] = 0.0f; farba[52] = 0.0f; farba[53] = 1.0f;

                ////prava stena
                //farba[54] = 0.0f; farba[55] = 1.0f; farba[56] = 0.0f;
                //farba[57] = 1.0f; farba[58] = 0.0f; farba[59] = 0.0f;
                //farba[60] = 0.0f; farba[61] = 0.0f; farba[62] = 1.0f;
                //farba[63] = 0.0f; farba[64] = 1.0f; farba[65] = 0.0f;
                //farba[66] = 0.0f; farba[67] = 0.0f; farba[68] = 1.0f;
                //farba[69] = 1.0f; farba[70] = 0.0f; farba[71] = 0.0f;

                ////spodna stena
                //farba[72] = 1.0f; farba[73] = 0.0f; farba[74] = 0.0f;
                //farba[75] = 0.0f; farba[76] = 1.0f; farba[77] = 0.0f;
                //farba[78] = 0.0f; farba[79] = 0.0f; farba[80] = 1.0f;
                //farba[81] = 1.0f; farba[82] = 0.0f; farba[83] = 0.0f;
                //farba[84] = 0.0f; farba[85] = 1.0f; farba[86] = 0.0f;
                //farba[87] = 0.0f; farba[88] = 0.0f; farba[89] = 1.0f;

                ////vrchna stena
                //farba[90] = 0.0f; farba[91] = 0.0f; farba[92] = 1.0f;
                //farba[93] = 1.0f; farba[94] = 0.0f; farba[95] = 0.0f;
                //farba[96] = 0.0f; farba[97] = 1.0f; farba[98] = 0.0f;
                //farba[99] = 0.0f; farba[100] = 0.0f; farba[101] = 1.0f;
                //farba[102] = 0.0f; farba[103] = 1.0f; farba[104] = 0.0f;
                //farba[105] = 1.0f; farba[106] = 0.0f; farba[107] = 0.0f;

                //predna stena
                farba[0] = 1.0f; farba[1] = 0.0f; farba[2] = 0.0f;
                farba[3] = 1.0f; farba[4] = 0.0f; farba[5] = 0.0f;
                farba[6] = 1.0f; farba[7] = 0.0f; farba[8] = 0.0f;
                farba[9] = 1.0f; farba[10] = 0.0f; farba[11] = 0.0f;
                farba[12] = 1.0f; farba[13] = 0.0f; farba[14] = 0.0f;
                farba[15] = 1.0f; farba[16] = 0.0f; farba[17] = 0.0f;

                //zadna stena
                farba[18] = 0.0f; farba[19] = 1.0f; farba[20] = 0.0f;
                farba[21] = 0.0f; farba[22] = 1.0f; farba[23] = 0.0f;
                farba[24] = 0.0f; farba[25] = 1.0f; farba[26] = 0.0f;
                farba[27] = 0.0f; farba[28] = 1.0f; farba[29] = 0.0f;
                farba[30] = 0.0f; farba[31] = 1.0f; farba[32] = 0.0f;
                farba[33] = 0.0f; farba[34] = 1.0f; farba[35] = 0.0f;

                //lava stena
                farba[36] = 0.0f; farba[37] = 0.0f; farba[38] = 1.0f;
                farba[39] = 0.0f; farba[40] = 0.0f; farba[41] = 1.0f;
                farba[42] = 0.0f; farba[43] = 0.0f; farba[44] = 1.0f;
                farba[45] = 0.0f; farba[46] = 0.0f; farba[47] = 1.0f;
                farba[48] = 0.0f; farba[49] = 0.0f; farba[50] = 1.0f;
                farba[51] = 0.0f; farba[52] = 0.0f; farba[53] = 1.0f;

                //prava stena
                farba[54] = 1.0f; farba[55] = 1.0f; farba[56] = 0.0f;
                farba[57] = 1.0f; farba[58] = 1.0f; farba[59] = 0.0f;
                farba[60] = 1.0f; farba[61] = 1.0f; farba[62] = 0.0f;
                farba[63] = 1.0f; farba[64] = 1.0f; farba[65] = 0.0f;
                farba[66] = 1.0f; farba[67] = 1.0f; farba[68] = 0.0f;
                farba[69] = 1.0f; farba[70] = 1.0f; farba[71] = 0.0f;

                //spodna stena
                farba[72] = 1.0f; farba[73] = 0.0f; farba[74] = 1.0f;
                farba[75] = 1.0f; farba[76] = 0.0f; farba[77] = 1.0f;
                farba[78] = 1.0f; farba[79] = 0.0f; farba[80] = 1.0f;
                farba[81] = 1.0f; farba[82] = 0.0f; farba[83] = 1.0f;
                farba[84] = 1.0f; farba[85] = 0.0f; farba[86] = 1.0f;
                farba[87] = 1.0f; farba[88] = 0.0f; farba[89] = 1.0f;

                //vrchna stena
                farba[90] = 1.0f; farba[91] = 1.0f; farba[92] = 1.0f;
                farba[93] = 1.0f; farba[94] = 1.0f; farba[95] = 1.0f;
                farba[96] = 1.0f; farba[97] = 1.0f; farba[98] = 1.0f;
                farba[99] = 1.0f; farba[100] = 1.0f; farba[101] = 1.0f;
                farba[102] = 1.0f; farba[103] = 1.0f; farba[104] = 1.0f;
                farba[105] = 1.0f; farba[106] = 1.0f; farba[107] = 1.0f;
                #endregion
            }
        }

        public Stvorec3D(bool trojuholniky,float x,float y ,float z, int w, int h, float s)
        {
            VBO = new int[3];
            VAO = new int[1];
            VertexShader = new Shaders.Shader();
            FragmentShader = new Shaders.Shader();
            spMain = new Shaders.ShaderProgram();
            vtip = trojuholniky;
            width = w;
            height = h;
            scale = s;

            if (vtip)//steny zlozene z trojuholnikov
            {
                kocka = new float[108];
                farba = new float[108];

                #region vrcholy
                //predna stena
                kocka[0] = -1.0f + x; kocka[1] = -1.0f; kocka[2] = 1.0f;
                kocka[3] = 1.0f + x; kocka[4] = -1.0f; kocka[5] = 1.0f;
                kocka[6] = 1.0f + x; kocka[7] = 1.0f; kocka[8] = 1.0f;
                kocka[9] = -1.0f + x; kocka[10] = -1.0f; kocka[11] = 1.0f;
                kocka[12] = 1.0f + x; kocka[13] = 1.0f; kocka[14] = 1.0f;
                kocka[15] = -1.0f + x; kocka[16] = 1.0f; kocka[17] = 1.0f;

                //zadna stena
                kocka[18] = -1.0f + x; kocka[19] = -1.0f; kocka[20] = -1.0f;
                kocka[21] = 1.0f + x; kocka[22] = -1.0f; kocka[23] = -1.0f;
                kocka[24] = 1.0f + x; kocka[25] = 1.0f; kocka[26] = -1.0f;
                kocka[27] = -1.0f + x; kocka[28] = -1.0f; kocka[29] = -1.0f;
                kocka[30] = 1.0f + x; kocka[31] = 1.0f; kocka[32] = -1.0f;
                kocka[33] = -1.0f + x; kocka[34] = 1.0f; kocka[35] = -1.0f;

                //lava stena
                kocka[36] = -1.0f + x; kocka[37] = -1.0f; kocka[38] = 1.0f;
                kocka[39] = -1.0f + x; kocka[40] = -1.0f; kocka[41] = -1.0f;
                kocka[42] = -1.0f + x; kocka[43] = 1.0f; kocka[44] = -1.0f;
                kocka[45] = -1.0f + x; kocka[46] = -1.0f; kocka[47] = 1.0f;
                kocka[48] = -1.0f + x; kocka[49] = 1.0f; kocka[50] = -1.0f;
                kocka[51] = -1.0f + x; kocka[52] = 1.0f; kocka[53] = 1.0f;

                //prava stena
                kocka[54] = 1.0f + x; kocka[55] = -1.0f; kocka[56] = 1.0f;
                kocka[57] = 1.0f + x; kocka[58] = -1.0f; kocka[59] = -1.0f;
                kocka[60] = 1.0f + x; kocka[61] = 1.0f; kocka[62] = -1.0f;
                kocka[63] = 1.0f + x; kocka[64] = -1.0f; kocka[65] = 1.0f;
                kocka[66] = 1.0f + x; kocka[67] = 1.0f; kocka[68] = -1.0f;
                kocka[69] = 1.0f + x; kocka[70] = 1.0f; kocka[71] = 1.0f;

                //spodna stena
                kocka[72] = -1.0f + x; kocka[73] = -1.0f; kocka[74] = 1.0f;
                kocka[75] = 1.0f + x; kocka[76] = -1.0f; kocka[77] = 1.0f;
                kocka[78] = 1.0f + x; kocka[79] = -1.0f; kocka[80] = -1.0f;
                kocka[81] = -1.0f + x; kocka[82] = -1.0f; kocka[83] = 1.0f;
                kocka[84] = 1.0f + x; kocka[85] = -1.0f; kocka[86] = -1.0f;
                kocka[87] = -1.0f + x; kocka[88] = -1.0f; kocka[89] = -1.0f;

                //vrchna stena
                kocka[90] = -1.0f + x; kocka[91] = 1.0f; kocka[92] = 1.0f;
                kocka[93] = 1.0f + x; kocka[94] = 1.0f; kocka[95] = 1.0f;
                kocka[96] = 1.0f + x; kocka[97] = 1.0f; kocka[98] = -1.0f;
                kocka[99] = -1.0f + x; kocka[100] = 1.0f; kocka[101] = 1.0f;
                kocka[102] = 1.0f + x; kocka[103] = 1.0f; kocka[104] = -1.0f;
                kocka[105] = -1.0f + x; kocka[106] = 1.0f; kocka[107] = -1.0f;
                #endregion

                #region farby
                Random X = new Random();
                //predna stena
                farba[0] = (float)X.NextDouble(); farba[1] = 0.0f; farba[2] = 0.0f;
                farba[3] = (float)X.NextDouble(); farba[4] = 0.0f; farba[5] = 0.0f;
                farba[6] = (float)X.NextDouble(); farba[7] = 0.0f; farba[8] = 0.0f;
                farba[9] = (float)X.NextDouble(); farba[10] = 0.0f; farba[11] = 0.0f;
                farba[12] = (float)X.NextDouble(); farba[13] = 0.0f; farba[14] = 0.0f;
                farba[15] = (float)X.NextDouble(); farba[16] = 0.0f; farba[17] = 0.0f;

                //zadna stena
                farba[18] = 0.0f; farba[19] = (float)X.NextDouble(); farba[20] = 0.0f;
                farba[21] = 0.0f; farba[22] = (float)X.NextDouble(); farba[23] = 0.0f;
                farba[24] = 0.0f; farba[25] = (float)X.NextDouble(); farba[26] = 0.0f;
                farba[27] = 0.0f; farba[28] = (float)X.NextDouble(); farba[29] = 0.0f;
                farba[30] = 0.0f; farba[31] = (float)X.NextDouble(); farba[32] = 0.0f;
                farba[33] = 0.0f; farba[34] = (float)X.NextDouble(); farba[35] = 0.0f;

                //lava stena
                farba[36] = 0.0f; farba[37] = 0.0f; farba[38] = (float)X.NextDouble();
                farba[39] = 0.0f; farba[40] = 0.0f; farba[41] = (float)X.NextDouble();
                farba[42] = 0.0f; farba[43] = 0.0f; farba[44] = (float)X.NextDouble();
                farba[45] = 0.0f; farba[46] = 0.0f; farba[47] = (float)X.NextDouble();
                farba[48] = 0.0f; farba[49] = 0.0f; farba[50] = (float)X.NextDouble();
                farba[51] = 0.0f; farba[52] = 0.0f; farba[53] = (float)X.NextDouble();

                //prava stena
                farba[54] = (float)X.NextDouble(); farba[55] = (float)X.NextDouble(); farba[56] = 0.0f;
                farba[57] = (float)X.NextDouble(); farba[58] = (float)X.NextDouble(); farba[59] = 0.0f;
                farba[60] = (float)X.NextDouble(); farba[61] = (float)X.NextDouble(); farba[62] = 0.0f;
                farba[63] = (float)X.NextDouble(); farba[64] = (float)X.NextDouble(); farba[65] = 0.0f;
                farba[66] = (float)X.NextDouble(); farba[67] = (float)X.NextDouble(); farba[68] = 0.0f;
                farba[69] = (float)X.NextDouble(); farba[70] = (float)X.NextDouble(); farba[71] = 0.0f;

                //spodna stena
                farba[72] = 0.0f; farba[73] = (float)X.NextDouble(); farba[74] = (float)X.NextDouble();
                farba[75] = 0.0f; farba[76] = (float)X.NextDouble(); farba[77] = (float)X.NextDouble();
                farba[78] = 0.0f; farba[79] = (float)X.NextDouble(); farba[80] = (float)X.NextDouble();
                farba[81] = 0.0f; farba[82] = (float)X.NextDouble(); farba[83] = (float)X.NextDouble();
                farba[84] = 0.0f; farba[85] = (float)X.NextDouble(); farba[86] = (float)X.NextDouble();
                farba[87] = 0.0f; farba[88] = (float)X.NextDouble(); farba[89] = (float)X.NextDouble();

                //vrchna stena
                farba[90] = (float)X.NextDouble(); farba[91] = 0.0f; farba[92] = (float)X.NextDouble();
                farba[93] = (float)X.NextDouble(); farba[94] = 0.0f; farba[95] = (float)X.NextDouble();
                farba[96] = (float)X.NextDouble(); farba[97] = 0.0f; farba[98] = (float)X.NextDouble();
                farba[99] = (float)X.NextDouble(); farba[100] = 0.0f; farba[101] = (float)X.NextDouble();
                farba[102] = (float)X.NextDouble(); farba[103] = 0.0f; farba[104] = (float)X.NextDouble();
                farba[105] = (float)X.NextDouble(); farba[106] = 0.0f; farba[107] = (float)X.NextDouble();
                #endregion
            }
        }
        private void NastavMatice(bool co)
        {
            //prvotny resize
            if (!co)
            {
                modelViewMatrix = Matrix4.LookAt(0.0f, 0.0f, 3.5f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f);
                projectionMatrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4.0f, width / (float)height, 0.01f, 300.0f);

                ScaleMatrix = Matrix4.CreateScale(scale, scale, scale);
                TranslationMatrix = Matrix4.CreateTranslation(0.0f, 0.0f, 0.0f);
                RotationMatrix = Matrix4.Identity;
                MatrixStore_Rotations = Matrix4.Identity;
                MatrixStore_Translations = Matrix4.Identity;
                MatrixStore_Scales = Matrix4.Identity;
                Current = Matrix4.Identity;
            }
            //resize vyvolany pouzivatelom
            else
                projectionMatrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4.0f, width / (float)height, 0.01f, 300.0f);
        }

        public void NastavKocku()
        {
            NastavMatice(false);
            GL.GenBuffers(3, VBO);
            GL.GenVertexArrays(1, VAO);

            GL.BindVertexArray(VAO[0]);

            //vrcholy
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(108 * sizeof(float)), kocka, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

            //farby
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO[1]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(108 * sizeof(float)), farba, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);

            //normaly
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO[2]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(108 * sizeof(float)), normaly, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 0, 0);

            //vytvorenie shader programu--- pridana kontrola nacitania, zo Shaders.dll by som teda mohol odstranit vyhadzaovanie messageboxov
            if (!VertexShader.LoadShader("..\\..\\Properties\\data\\shaders\\dirShader.vert", ShaderType.VertexShader))
                System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat vertex sahder!");
            if (!FragmentShader.LoadShader("..\\..\\Properties\\data\\shaders\\dirShader.frag", ShaderType.FragmentShader))
                System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat fragment sahder!");
            //if (!VertexShader.LoadShader("..\\..\\Properties\\data\\shaders\\l_shader.vert", ShaderType.VertexShader))
            //    System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat vertex sahder!");
            //if (!FragmentShader.LoadShader("..\\..\\Properties\\data\\shaders\\l_shader.frag", ShaderType.FragmentShader))
                //System.Windows.Forms.MessageBox.Show("Nepodarilo sa nacitat fragment sahder!");
       
            spMain.CreateProgram();
            spMain.AddShaderToProgram(VertexShader);
            spMain.AddShaderToProgram(FragmentShader);
            spMain.LinkProgram();
            spMain.UseProgram();

            spMain.SetUniform("projectionMatrix", projectionMatrix);
            spMain.SetUniform("eye", new Vector3(0.0f, 0.0f, 3.5f));//dorobit pri zmene pozicie kamery update
            material.SetMaterialUniforms(spMain);
            light.SetDirectionalLightUniforms(spMain);
            
            //prvy shader pre rovnobezne svetlo
            ////farba svetla
            //spMain.SetUniform("sunLight.vColor", new Vector3(1.0f, 1.0f, 1.0f));
            ////nastavenie intenzity ambientneho svetla
            //spMain.SetUniform("sunLight.fAmbientIntensity",0.25f);
            ////smer svetelnych lucov
            //spMain.SetUniform("sunLight.vDirection", new Vector3(0.0f, 0.0f, -1.0f));

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.ClearDepth(1.0);

            //KresliKocku();
        }

        public void SetLight(Vector3 specular, Vector3 ambient, Vector3 diffuse, Vector3 direction)
        {
            light = new DirectionalLight(direction, ambient, specular, diffuse);
            light.SetDirectionalLightUniforms(spMain);
        }

        public void KresliKocku()
        {
            Matrix4 mat = (MatrixStore_Scales * ScaleMatrix) * (MatrixStore_Rotations * RotationMatrix) * (TranslationMatrix * MatrixStore_Translations);
            Current = mat * modelViewMatrix;
            mat = mat.Inverted(); mat.Transpose();
            GL.BindVertexArray(VAO[0]);
            spMain.SetUniform("normalMatrix", mat);
            spMain.SetUniform("modelViewMatrix",Current);
        }

        public void PrekresliKocku()
        {
            GL.BindVertexArray(VAO[0]);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        }

        public void Translate(float x, float y)
        {
            TranslationMatrix = Matrix4.CreateTranslation(x, y, 0.0f);
            Current = (MatrixStore_Scales * ScaleMatrix) * (MatrixStore_Rotations * RotationMatrix)* (TranslationMatrix * MatrixStore_Translations) * modelViewMatrix;
            spMain.SetUniform("modelViewMatrix", Current);
        }

        public void Rotate(float x, float y, float angle)
        {
            RotationMatrix = Matrix4.CreateFromAxisAngle(new Vector3(y, x, 0.0f), angle);
            Matrix4 mat = (MatrixStore_Scales * ScaleMatrix) * (MatrixStore_Rotations * RotationMatrix);
            Current = mat * (TranslationMatrix * MatrixStore_Translations) * modelViewMatrix;
            mat = mat.Inverted(); mat = Matrix4.Transpose(mat);
            spMain.SetUniform("normalMatrix", mat);
            spMain.SetUniform("modelViewMatrix", Current);
        }

        public void Scale(float s)
        {
            scale = s;
            ScaleMatrix = Matrix4.CreateScale(scale, scale, scale);
            Matrix4 mat = (MatrixStore_Scales * ScaleMatrix) * (MatrixStore_Rotations * RotationMatrix);
            Current = mat * (TranslationMatrix * MatrixStore_Translations) * modelViewMatrix;
            mat = mat.Inverted(); mat = Matrix4.Transpose(mat);
            spMain.SetUniform("normalMatrix", mat);
            spMain.SetUniform("modelViewMatrix", Current);
        }

        public void Resize(int w, int h)
        {
            width = w; height = h;
            NastavMatice(true);
            GL.BindVertexArray(VAO[0]);
            spMain.SetUniform("projectionMatrix", projectionMatrix);
            spMain.SetUniform("modelViewMatrix", Current);
        }

        public void Ende()
        {
            MatrixStore_Translations = MatrixStore_Translations * TranslationMatrix;
            MatrixStore_Rotations = MatrixStore_Rotations * RotationMatrix;

            spMain.SetUniform("modelViewMatrix", Current);
            ScaleMatrix = Matrix4.Identity;
            RotationMatrix = Matrix4.Identity;
            TranslationMatrix = Matrix4.Identity;
        }

        public void ZnicKocku()
        {
            //zrejme by tu tiez mohla prist kontrola, ci znicenie programov prebehlo v poriadku, resp. ju zakomponovat do Shaders.dll
            VertexShader.DeleteShader();
            FragmentShader.DeleteShader();
            spMain.DeleteProgram();
            GL.DeleteBuffers(3, VBO);
            GL.DeleteVertexArrays(1, VAO);
        }

        public void Pohyb(float dd)
        {
            BigBrother.MoveCamera(dd);
            modelViewMatrix = BigBrother.ReturnCamera();
            Current = (MatrixStore_Scales * ScaleMatrix) * (MatrixStore_Rotations * RotationMatrix)* (TranslationMatrix * MatrixStore_Translations) * modelViewMatrix;
            spMain.SetUniform("modelViewMatrix", Current);
        }

        public void Natoc(float dd)
        {
            BigBrother.RotateCameraY(dd);
            modelViewMatrix = BigBrother.ReturnCamera();
            Current = (MatrixStore_Scales * ScaleMatrix) * (MatrixStore_Rotations * RotationMatrix) * (TranslationMatrix * MatrixStore_Translations) * modelViewMatrix;
            spMain.SetUniform("modelViewMatrix", Current);
        }
    }
}
