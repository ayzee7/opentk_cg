using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace opentk_cg
{
    class Camera
    {
        private int SCREENWIDTH;
        private int SCREENHEIGHT;

        public Vector3 position;

        Vector3 up = Vector3.UnitY;

        public Camera() { }
        public Camera(int width, int height, Vector3 position)
        {
            SCREENHEIGHT = height;
            SCREENWIDTH = width;
            this.position = position;
        }

        public Matrix4 GetViewMatrix() 
        { 
            return Matrix4.LookAt(position, Vector3.Zero + up * 3, up);
        }

        public Matrix4 GetProjection() 
        {
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(60f), (float)SCREENWIDTH / (float)SCREENHEIGHT, 0.1f, 100f);
            return projection;
        }

        public void ResizeUpdate(ResizeEventArgs e)
        {
            SCREENHEIGHT = e.Height;
            SCREENWIDTH = e.Width;
        }
        
    }
}
