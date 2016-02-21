using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Kocka
{
    class Material
    {
        private float SpecCoef, diffCoef, ambCoef;
        private int shininess;
        private Vector3 Ambient;
        private Vector3 Specular;
        private Vector3 Diffuse;

        public Material(Vector3 ambient, Vector3 specular, Vector3 diffuse, float amb, float spc, float diff, int sh)
        {
            SpecCoef = spc; ambCoef = amb; diffCoef = diff; shininess = sh;
            Ambient = ambient; Specular = specular; Diffuse = diffuse;
        }

        public void SetMaterialUniforms(Shaders.ShaderProgram prog)
        {
            prog.SetUniform("material.specCoef",SpecCoef);
            prog.SetUniform("material.diffCoef", diffCoef);
            prog.SetUniform("material.ambCoef", ambCoef);
            prog.SetUniform("material.ambient", Ambient);
            prog.SetUniform("material.diffuse", Diffuse);
            prog.SetUniform("material.specular", Specular);
            prog.SetUniform("material.shininess", shininess);
        }
    }
}
