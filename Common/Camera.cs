using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace opentk_cg.Common
{
    class Camera
    {
        private int SCREENWIDTH;
        private int SCREENHEIGHT;

        public Vector3 position;
        private Vector3 initPosition;

        Vector3 up = Vector3.UnitY;

        public Camera() { }
        public Camera(int width, int height, Vector3 position)
        {
            SCREENHEIGHT = height;
            SCREENWIDTH = width;
            this.position = position;
            this.initPosition = position;
        }

        public Matrix4 GetViewMatrix(Movement move) 
        { 
            return Matrix4.LookAt(position, new Vector3(0, position.Y, move.position.Z), up);
        }

        public Matrix4 GetProjection() 
        {
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(60f), SCREENWIDTH / (float)SCREENHEIGHT, 0.1f, 200f);
            return projection;
        }
        
        public void Update(float exp)
        {
            position.Z -= 0.005f * exp;
        }

        public void ResizeUpdate(ResizeEventArgs e)
        {
            SCREENHEIGHT = e.Height;
            SCREENWIDTH = e.Width;
        }

        public void Reset()
        {
            position = initPosition;
        }
        
    }
}
