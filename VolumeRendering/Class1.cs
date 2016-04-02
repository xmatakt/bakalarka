using System.IO;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Shaders
{
    public class Shader
    {
        private int ShaderHandle;//ID shejdru
        ShaderType iType;//typ-vertex/fragment/geometry shader
        bool Loaded;//ci bol shejder loadnuty a skompilovany

        public bool LoadShader(string fileName, ShaderType a_iType)
        {
            int CompilationStatus;
            string ShaderSource;

            try
            {
                ShaderSource = File.ReadAllText(fileName);
            }
            catch (System.IO.FileNotFoundException)
            {
                System.Diagnostics.Debug.WriteLine("Nenasiel som fajlu pre sejder.");
                System.Windows.Forms.MessageBox.Show("Nenasiel som fajlu pre sejder.");
                return false;
            }

            ShaderHandle = GL.CreateShader(a_iType);//vytvorenie shejdra, vracia int (ID/meno/...) shejdru
            GL.ShaderSource(ShaderHandle, ShaderSource);
            GL.CompileShader(ShaderHandle);//skompilovanie shejdra

            //kontrola, ci fsetko zbehlo v poriadku
            GL.GetShader(ShaderHandle, ShaderParameter.CompileStatus, out CompilationStatus);//zrejme nakrmi CompilationStatus hodnotou 1, ak je to OK, inak 0
            System.Diagnostics.Debug.WriteLine("\n CompilationStatus = {0} \n", CompilationStatus);//teba vymazem

            if (CompilationStatus == 0)
            {
                Loaded = false;
                //System.Windows.Forms.MessageBox.Show("Nepodarilo sa skompilovat sejder.");
            }
            else
            {
                iType = a_iType;
                Loaded = true;
            }
            return Loaded;
        }

        public bool LoadShaderS(string shaderSource, ShaderType a_iType)
        {
            int CompilationStatus;

            ShaderHandle = GL.CreateShader(a_iType);//vytvorenie shejdra, vracia int (ID/meno/...) shejdru
            GL.ShaderSource(ShaderHandle, shaderSource);
            GL.CompileShader(ShaderHandle);//skompilovanie shejdra

            //kontrola, ci fsetko zbehlo v poriadku
            GL.GetShader(ShaderHandle, ShaderParameter.CompileStatus, out CompilationStatus);//zrejme nakrmi CompilationStatus hodnotou 1, ak je to OK, inak 0
            System.Diagnostics.Debug.WriteLine("\n CompilationStatus = {0} \n", CompilationStatus);//teba vymazem

            if (CompilationStatus == 0)
            {
                Loaded = false;
                //System.Windows.Forms.MessageBox.Show("Nepodarilo sa skompilovat sejder.");
            }
            else
            {
                iType = a_iType;
                Loaded = true;
            }
            return Loaded;
        }

        public void DeleteShader()
        {
            if (!Loaded)
                return;
            Loaded = false;
            GL.DeleteShader(ShaderHandle);
        }

        public bool IsLoaded() { return Loaded; }
        public int GetShaderHandle() { return ShaderHandle; }
        public Shader() { Loaded = false; }
    }
}
