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
    class DirectionalLight
    {
        private Vector3 Direction;
        private Vector3 Ambient;
        private Vector3 Specular;
        private Vector3 Diffuse;

        public DirectionalLight(Vector3 dir,Vector3 amb,Vector3 spec,Vector3 diff)
        {
            Direction = dir;
            Ambient = amb; Specular = spec; Diffuse = diff;
        }

        public void SetDirectionalLightUniforms(Shaders.ShaderProgram prog)
        {
            prog.SetUniform("light.ambient", Ambient);
            prog.SetUniform("light.diffuse", Diffuse);
            prog.SetUniform("light.specular", Specular);
            prog.SetUniform("light.direction", Direction);
        }
    }
}
