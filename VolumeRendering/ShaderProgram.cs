using System.Diagnostics;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Shaders
{
    public class ShaderProgram
    {
        private int ProgramHandle; //hadaj co
        private bool Linked; //ci bol program linknuty a je reditujus

        public ShaderProgram()
        {
            Linked = false;
        }
        public void CreateProgram()
        {
            ProgramHandle = GL.CreateProgram();
        }

        public bool AddShaderToProgram(Shader addMe)
        {
            if (!addMe.IsLoaded())//sejker nie je nacitany
                return false;
            GL.AttachShader(ProgramHandle, addMe.GetShaderHandle());
            return true;
        }

        public bool AddShaderToProgram(int shaderID)
        {
            if (shaderID < 0)//sejker nie je nacitany
                return false;
            GL.AttachShader(ProgramHandle, shaderID);
            return true;
        }

        public bool LinkProgram()
        {
            int LinkStatus;
            GL.LinkProgram(ProgramHandle);
            //kontrola
            GL.GetProgram(ProgramHandle, GetProgramParameterName.LinkStatus, out LinkStatus);
            if (LinkStatus == 1)
            {
                Debug.WriteLine("LinkStatus = {0}", LinkStatus);
                Linked = true;
            }
            else
            {
                //System.Windows.Forms.MessageBox.Show("Chyba pri linkovani programu!");
                Linked = false;
            }
            return Linked;
        }

        public void UseProgram()
        {
            if (Linked)
                GL.UseProgram(ProgramHandle);
        }

        public void UseProgram(int val)
        {
            //if (Linked)
                GL.UseProgram(val);
        }

        public int GetProgramHandle()
        {
            return ProgramHandle;
        }

        public void DeleteProgram()
        {
            if (!Linked)
                return;
            Linked = false;
            GL.DeleteProgram(ProgramHandle);
        }

        public void SetUniform(string uniformName, Matrix4 matrix)
        {
            if (!Linked)
                return;
            int location = GL.GetUniformLocation(ProgramHandle, uniformName);
            if (location >= 0)
                GL.UniformMatrix4(location, false, ref matrix);
            //else
            //    System.Windows.Forms.MessageBox.Show(uniformName + " is not bind to the uniform!");
        }

        public void SetUniform(string uniformName, int value)
        {
            if (!Linked)
                return;
            int location = GL.GetUniformLocation(ProgramHandle, uniformName);
            if (location >= 0)
                GL.Uniform1(location, value);
            else
                System.Windows.Forms.MessageBox.Show(uniformName + " is not bind to the uniform!");
        }


        public void SetUniform(string uniformName, float value)
        {
            if (!Linked)
                return;
            int location = GL.GetUniformLocation(ProgramHandle, uniformName);
            if (location >= 0)
                GL.Uniform1(location, value);
            //else
            //    System.Windows.Forms.MessageBox.Show(uniformName + " is not bind to the uniform!");
        }

        public void SetUniform(string uniformName, Vector3 vector)
        {
            if (!Linked)
                return;
            int location = GL.GetUniformLocation(ProgramHandle, uniformName);
            if (location >= 0)
                GL.Uniform3(location, vector);
            else
                System.Windows.Forms.MessageBox.Show(uniformName + " is not bind to the uniform!");
        }

        public void SetUniform(string uniformName, float v1, float v2)
        {
            if (!Linked)
                return;
            int location = GL.GetUniformLocation(ProgramHandle, uniformName);
            if (location >= 0)
                GL.Uniform2(location, v1, v2);
            else
                System.Windows.Forms.MessageBox.Show(uniformName + " is not bind to the uniform!");
        }
    }
}
