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
    class Kamera
    {
        private Matrix4 LookAtMatrix;
        private Vector3 Eye, Target, Up;

        private Vector3 M(Matrix4 m,Vector3 v)
        {
            Vector3 tmp = new Vector3();
            tmp.X = m.M11 * v.X + m.M12 * v.Y + m.M13 * v.Z + m.M14;
            tmp.Y = m.M21 * v.X + m.M22 * v.Y + m.M23 * v.Z + m.M24;
            tmp.Z = m.M31 * v.X + m.M32 * v.Y + m.M33 * v.Z + m.M34;
            return tmp;
        }

        public Kamera(Vector3 eye,Vector3 target, Vector3 up)
        {
            Eye = eye; Target = target; Up = up;
            LookAtMatrix = Matrix4.LookAt(Eye, Target, Up);
        }

        public void MoveCamera(float dd)
        {
            Vector3 direction = Target - Eye;
            direction = direction.Normalized();
            direction = direction * dd;
            Eye += direction;
            Target += direction;
            UpdateCamera();
        }

        public void RotateCameraY(float angle)
        {
            angle = angle * (float)Math.PI / 180.0f;
            Matrix4 rot = Matrix4.CreateRotationY(angle);
            Vector3 direction = Target - Eye;
            Target = M(rot,direction);
            Target += Eye;
            UpdateCamera();
        }

        private void UpdateCamera()
        {
            LookAtMatrix = Matrix4.LookAt(Eye, Target, Up);
        }

        public Matrix4 ReturnCamera()
        {
            return LookAtMatrix;
        }
    }
}
