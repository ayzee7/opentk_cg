using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace opentk_cg.Common
{
    class Movement
    {
        private float SPEED = 3f;

        public Vector3 position;
        private Vector3 initPosition;

        Vector3 right = Vector3.UnitX;

        public Movement(Vector3 position)
        {
            this.position = position;
            this.initPosition = position;
        }
        public Matrix4 GetModel()
        {
            return Matrix4.CreateTranslation(position);
        }
        public void InputController(KeyboardState input, FrameEventArgs e)
        {
            if (input.IsKeyDown(Keys.A))
            {
                if (position.X > -2.5f)
                    position -= right * SPEED * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.D))
            {
                if (position.X < 2.5f)
                    position += right * SPEED * (float)e.Time;
            }
        }
        public void Update(KeyboardState input, float exp, FrameEventArgs e)
        {
            InputController(input, e);
            position.Z -= 0.005f * exp;
        }

        public void Reset()
        {
            position = initPosition;
        }
    }
}
