using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Audio.OpenAL;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ErrorCode = OpenTK.Graphics.OpenGL4.ErrorCode;
using StbImageSharp;

namespace opentk_cg
{
    internal class Game : GameWindow
    {
        int width, height;

        float[] vertices1 = 
        {
            0f,     0.5f,   0f,     // top vertex
            -0.5f,  -0.5f,  0f,     // bottom left vertex
            0.5f,   -0.5f,  0f      // bottom right vertex
        };

        float[] vertices2 =
        {
            -0.5f,  0.5f,   0f,     // top left vertex - 0
            0.5f,   0.5f,   0f,     // top right vertex - 1
            0.5f,   -0.5f,  0f,     // bottom right vertex - 2
            -0.5f,  -0.5f,  0f      // bottom left vertex - 3
        };

        uint[] indices2 =
        {
            0, 1, 2,    // top triangle
            2, 3, 0     // bottom triangle
        };

        float[] texCoords =
        {
            0f, 1f, 
            1f, 1f, 
            1f, 0f, 
            0f, 0f
        };

        int VAO;
        int VBO;
        int EBO;
        Shader shaderProgram = new Shader();

        int textureID;
        int textureVBO;
        public Game(int width, int height) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            this.CenterWindow(new Vector2i(width, height));
            this.height = height;
            this.width = width;
        }

        protected override void OnLoad()
        {
            ErrorCode error = GL.GetError();
            
            //  Create VAO
            VAO = GL.GenVertexArray();
            //  Create VBO
            VBO = GL.GenBuffer();
            //  Bind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            //  Copy verticles data to the buffer
            GL.BufferData(BufferTarget.ArrayBuffer, vertices2.Length * sizeof(float), vertices2, BufferUsageHint.StaticDraw);
            //  Bind the VAO
            GL.BindVertexArray(VAO);
            //  Bind a slot number 0
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            //  Enable the slot
            GL.EnableVertexArrayAttrib(VAO, 0);
            //  Unbind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);

            // Texture Loading
            textureID = GL.GenTexture(); //Generate empty texture
            GL.ActiveTexture(TextureUnit.Texture0); //Activate the texture in the unit
            GL.BindTexture(TextureTarget.Texture2D, textureID); //Bind texture
            
            //Texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            
            //Load image
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult boxTexture = ImageResult.FromStream(File.OpenRead("../../../Textures/img1.jpg"), ColorComponents.RedGreenBlueAlpha);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, boxTexture.Width, boxTexture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, boxTexture.Data);
            
            //Unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);

            EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices2.Length * sizeof(uint), indices2, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            //Create, bind texture
            textureVBO = GL.GenBuffer();
            error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine($"createdbufferOpenGL Error: {error}");
            }
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO);
            error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine($"bindedOpenGL Error: {error}");
            }
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords.Length * sizeof(float), texCoords, BufferUsageHint.StaticDraw);
            error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine($"copieddataOpenGL Error: {error}");
            }
            //Point a slot number 1
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine($"pointedslotOpenGL Error: {error}");
            }
            //Enable the slot
            GL.EnableVertexArrayAttrib(VAO, 1);
            error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine($"enabledOpenGL Error: {error}");
            }

            shaderProgram.LoadShader();
            
            base.OnLoad();
            
        }

        protected override void OnUnload()
        {
            GL.DeleteBuffer(VAO);
            GL.DeleteBuffer(VBO);
            GL.DeleteBuffer(EBO);
            GL.DeleteTexture(textureID);
            shaderProgram.DeleteShader();
            base.OnUnload();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(0.3f, 0.3f, 1f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            shaderProgram.UseShader();
            GL.BindTexture(TextureTarget.Texture2D, textureID);

            GL.BindVertexArray(VAO);
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 3);


            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.DrawElements(PrimitiveType.Triangles, indices2.Length, DrawElementsType.UnsignedInt, 0);

            Context.SwapBuffers();
            
            base.OnRenderFrame(args);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            this.width = e.Width;
            this.height = e.Height;
        }

        
    }

    public class Shader
    {
        int shaderHandle;

        public Shader() {}

        public void LoadShader()
        {
            shaderHandle = GL.CreateProgram();

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, LoadShaderSource("shader.vert"));
            GL.CompileShader(vertexShader);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, LoadShaderSource("shader.frag"));
            GL.CompileShader(fragmentShader);

            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int success1);
            if (success1 == 0)
            {
                string infoLog = GL.GetShaderInfoLog(vertexShader);
                Console.WriteLine(infoLog);
            }
            GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out int success2);
            if (success2 == 0)
            {
                string infoLog = GL.GetShaderInfoLog(fragmentShader);
                Console.WriteLine(infoLog);
            }

            GL.AttachShader(shaderHandle, vertexShader);
            GL.AttachShader(shaderHandle, fragmentShader);

            GL.LinkProgram(shaderHandle);

            GL.DeleteShader(shaderHandle);
        }

        public static string LoadShaderSource(string filepath)
        {
            string shaderSource = "";
            try
            {
                using (StreamReader reader = new StreamReader("../../../Shaders/" + filepath))
                {
                    shaderSource = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to load shader source file:" + e.Message);
            }
            return shaderSource;
        }
        public void UseShader()
        {
            GL.UseProgram(shaderHandle);
        }
        public void DeleteShader()
        {
            GL.DeleteProgram(shaderHandle);
        }
    }
}