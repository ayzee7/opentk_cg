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
using System.Diagnostics;

namespace opentk_cg
{
    internal class Game : GameWindow
    {
        int width, height;

        List<Vector3> cube_vertices = new List<Vector3>()
        {
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(-0.5f, 0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, -0.5f),
            new Vector3(-0.5f, -0.5f, 0.5f),
            new Vector3(-0.5f, 0.5f, 0.5f),
            new Vector3(-0.5f, 0.5f, -0.5f),
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, 0.5f),
            new Vector3(0.5f, 0.5f, 0.5f),
            new Vector3(-0.5f, 0.5f, 0.5f),
            new Vector3(-0.5f, -0.5f, 0.5f),
            new Vector3(0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, 0.5f),
            new Vector3(0.5f, -0.5f, 0.5f),
            new Vector3(-0.5f, -0.5f, 0.5f),
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, 0.5f),
            new Vector3(-0.5f, 0.5f, -0.5f),
            new Vector3(-0.5f, 0.5f, 0.5f),
            new Vector3(0.5f, 0.5f, 0.5f),
            new Vector3(0.5f, 0.5f, -0.5f),
        };

        List<Vector3> bg_vertices = new List<Vector3>()
        {
            new Vector3(-30f, -20f, -20f),
            new Vector3(-30f, 20f, -20f),
            new Vector3(30f, 20f, -20f),
            new Vector3(30f, -20f, -20f)
        };

        List<Vector3> tree_vertices = new List<Vector3>()
        {
            new Vector3(-1.5f, -1f, 0f),
            new Vector3(-1.5f, 3f, 0f),
            new Vector3(1.5f, 3f, 0f),
            new Vector3(1.5f, -1f, 0f)
        };

        uint[] cube_indices =
        {
            0, 1, 2,
            2, 3, 0,
            4, 5, 6,
            6, 7, 4,
            8, 9, 10,
            10, 11, 8,
            12, 13, 14,
            14, 15, 12,
            16, 17, 18,
            18, 19, 16,
            20, 21, 22,
            22, 23, 20
        };

        uint[] indices2d =
        {
            0, 1, 2,
            2, 3, 0,
        };

        List<Vector2> cube_coords = new List<Vector2>()
        {
            new Vector2(1f, 0f),
            new Vector2(1f, 1f),
            new Vector2(0f, 1f),
            new Vector2(0f, 0f),
            new Vector2(1f, 0f),
            new Vector2(1f, 1f),
            new Vector2(0f, 1f),
            new Vector2(0f, 0f),
            new Vector2(1f, 0f),
            new Vector2(1f, 1f),
            new Vector2(0f, 1f),
            new Vector2(0f, 0f),
            new Vector2(1f, 0f),
            new Vector2(1f, 1f),
            new Vector2(0f, 1f),
            new Vector2(0f, 0f),
            new Vector2(1f, 0f),
            new Vector2(1f, 1f),
            new Vector2(0f, 1f),
            new Vector2(0f, 0f),
            new Vector2(1f, 0f),
            new Vector2(1f, 1f),
            new Vector2(0f, 1f),
            new Vector2(0f, 0f),
        };

        List<Vector2> texCoords2d = new List<Vector2>()
        {
            new Vector2(0f, 0f),
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
        };

        int VAO, bgVAO, treeVAO;
        int VBO, bgVBO, treeVBO;
        int EBO, bgEBO, treeEBO;
        Shader shaderProgram = new Shader();

        int textureID, bgTextureID, treeTextureID;
        int textureVBO, bgTextureVBO, treeTextureVBO;

        float tree1Z, tree2Z, tree3Z, tree4Z, tree5Z;
        int treeZMin = -45;
        int treeZMax = -20;
        float tree1X, tree2X, tree3X, tree4X, tree5X;
        int treeXMin = -3;
        int treeXMax = 3;
        float[] treesZ_sorted;
        Random rand = new Random();
        bool isStuck = false;
        bool isHit = false;

        Timer timer; 

        Camera camera;
        Movement move;

        Vector3 cameraPos = new Vector3(0f, 3f, 7.5f);

        OpenALPlayer player;

        bool floatEquals(float a, float b)
        {
            float absA = Math.Abs(a);
            float absB = Math.Abs(b);
            float diff = Math.Abs(a - b);
            float eps = 1e-9f;

            if (a == b)
            { // shortcut, handles infinities
                return true;
            }
            else
            { // use relative error
                return diff / (absA + absB) < eps;
            }
        }

        public Game() : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            camera = new Camera(Size.X, Size.Y, cameraPos);
            WindowState = WindowState.Maximized;
            Title = "Tree Dodge Game";
            this.height = Size.X;
            this.width = Size.Y;
        }

        protected override void OnLoad()
        {
            ErrorCode error = GL.GetError();

            OnLoadRender(ref VAO,
                         ref VBO,
                         ref EBO,
                         ref textureID,
                         ref textureVBO,
                         ref cube_vertices,
                         ref cube_indices,
                         ref cube_coords,
                         "../../../Textures/player.jpg");
            OnLoadRender(ref bgVAO,
                         ref bgVBO,
                         ref bgEBO,
                         ref bgTextureID,
                         ref bgTextureVBO,
                         ref bg_vertices,
                         ref indices2d,
                         ref texCoords2d,
                         "../../../Textures/background.jpg");
            OnLoadRender(ref treeVAO,
                         ref treeVBO,
                         ref treeEBO,
                         ref treeTextureID,
                         ref treeTextureVBO,
                         ref tree_vertices,
                         ref indices2d,
                         ref texCoords2d,
                         "../../../Textures/tree.png");

            tree1Z = rand.Next(treeZMin, treeZMax);
            tree2Z = rand.Next(treeZMin, treeZMax);
            tree3Z = rand.Next(treeZMin, treeZMax);
            tree4Z = rand.Next(treeZMin, treeZMax);
            tree5Z = rand.Next(treeZMin, treeZMax);
            treesZ_sorted = [tree1Z, tree2Z, tree3Z, tree4Z, tree5Z];
            Array.Sort(treesZ_sorted);

            //  generate trees X coordinates in range (-2, 2)
            tree1X = (float)rand.NextDouble() * (treeXMax - treeXMin) + treeXMin;
            tree2X = (float)rand.NextDouble() * (treeXMax - treeXMin) + treeXMin;
            tree3X = (float)rand.NextDouble() * (treeXMax - treeXMin) + treeXMin;
            tree4X = (float)rand.NextDouble() * (treeXMax - treeXMin) + treeXMin;
            tree5X = (float)rand.NextDouble() * (treeXMax - treeXMin) + treeXMin;

            shaderProgram.LoadShader();

            GL.Enable(EnableCap.DepthTest);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Blend);

            move = new Movement(Vector3.Zero);

            player = new OpenALPlayer();
            player.StartPlayback();

            timer = new Timer();
            timer.Start();

            base.OnLoad();
            
        }

        protected override void OnUnload()
        {
            ClearObject(VAO, VBO, EBO, textureID);
            ClearObject(bgVAO, bgVBO, bgEBO, bgTextureID);
            ClearObject(treeVAO, treeVBO, treeEBO, treeTextureID);
            shaderProgram.DeleteShader();
            player.StopPlayback();
            base.OnUnload();
        }

        void ClearObject(int vao, int vbo, int ebo, int textureid)
        {
            GL.DeleteBuffer(vao);
            GL.DeleteBuffer(vbo);
            GL.DeleteBuffer(ebo);
            GL.DeleteTexture(textureid);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            if (!(move.position.X > tree1X - 1f && move.position.X < tree1X + 1f && tree1Z > -0.5f && tree1Z < 0.5f || 
                move.position.X > tree2X - 1f && move.position.X < tree2X + 1f && tree2Z > -0.5f && tree2Z < 0.5f || 
                move.position.X > tree3X - 1f && move.position.X < tree3X + 1f && tree3Z > -0.5f && tree3Z < 0.5f ||
                move.position.X > tree4X - 1f && move.position.X < tree4X + 1f && tree4Z > -0.5f && tree4Z < 0.5f ||
                move.position.X > tree5X - 1f && move.position.X < tree5X + 1f && tree5Z > -0.5f && tree5Z < 0.5f))
            {
                if (tree1Z > cameraPos.Z)
                {
                    tree1Z = rand.Next(treeZMin, treeZMax);
                    tree1X = (float)rand.NextDouble() * (treeXMax - treeXMin) + treeXMin;
                }
                if (tree2Z > cameraPos.Z)
                {
                    tree2Z = rand.Next(treeZMin, treeZMax);
                    tree2X = (float)rand.NextDouble() * (treeXMax - treeXMin) + treeXMin;
                }
                if (tree3Z > cameraPos.Z)
                {
                    tree3Z = rand.Next(treeZMin, treeZMax);
                    tree3X = (float)rand.NextDouble() * (treeXMax - treeXMin) + treeXMin;
                }
                if (tree4Z > cameraPos.Z)
                {
                    tree4Z = rand.Next(treeZMin, treeZMax);
                    tree4X = (float)rand.NextDouble() * (treeXMax - treeXMin) + treeXMin;
                }
                if (tree5Z > cameraPos.Z)
                {
                    tree5Z = rand.Next(treeZMin, treeZMax);
                    tree5X = (float)rand.NextDouble() * (treeXMax - treeXMin) + treeXMin;
                }
                treesZ_sorted = [tree1Z, tree2Z, tree3Z, tree4Z, tree5Z];
                Array.Sort(treesZ_sorted);

                GL.ClearColor(0.3f, 0.3f, 1f, 1f);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                shaderProgram.UseShader();

                Matrix4 model;
                Matrix4 view = camera.GetViewMatrix();
                Matrix4 projection = camera.GetProjection();

                int modelLocation = GL.GetUniformLocation(shaderProgram.shaderHandle, "model");
                int viewLocation = GL.GetUniformLocation(shaderProgram.shaderHandle, "view");
                int projectionLocation = GL.GetUniformLocation(shaderProgram.shaderHandle, "projection");

                GL.UniformMatrix4(viewLocation, true, ref view);
                GL.UniformMatrix4(projectionLocation, true, ref projection);

                // end init

                //  render bg
                model = Matrix4.Identity;
                RenderObject(bgVAO, bgEBO, model, bgTextureID, modelLocation, indices2d);

                //  render cube
                model = move.GetModel();
                RenderObject(VAO, EBO, model, textureID, modelLocation, cube_indices);

                //  render trees
                for (int i = 0; i < treesZ_sorted.Length; i++)
                {
                    if (floatEquals(tree1Z, treesZ_sorted[i]))
                    {
                        model = Matrix4.CreateTranslation(tree1X, 0f, tree1Z);
                        RenderObject(treeVAO, treeEBO, model, treeTextureID, modelLocation, indices2d);
                    }
                    else if (floatEquals(tree2Z, treesZ_sorted[i]))
                    {
                        model = Matrix4.CreateTranslation(tree2X, 0f, tree2Z);
                        RenderObject(treeVAO, treeEBO, model, treeTextureID, modelLocation, indices2d);
                    }
                    else if (floatEquals(tree3Z, treesZ_sorted[i]))
                    {
                        model = Matrix4.CreateTranslation(tree3X, 0f, tree3Z);
                        RenderObject(treeVAO, treeEBO, model, treeTextureID, modelLocation, indices2d);
                    }
                    else if (floatEquals(tree4Z, treesZ_sorted[i]))
                    {
                        model = Matrix4.CreateTranslation(tree4X, 0f, tree4Z);
                        RenderObject(treeVAO, treeEBO, model, treeTextureID, modelLocation, indices2d);
                    }
                    else if (floatEquals(tree5Z, treesZ_sorted[i]))
                    {
                        model = Matrix4.CreateTranslation(tree5X, 0f, tree5Z);
                        RenderObject(treeVAO, treeEBO, model, treeTextureID, modelLocation, indices2d);
                    }
                }
                tree1Z += 0.001f * timer.Exp();
                tree2Z += 0.001f * timer.Exp();
                tree3Z += 0.001f * timer.Exp();
                tree4Z += 0.001f * timer.Exp();
                tree5Z += 0.001f * timer.Exp();

                Context.SwapBuffers();
                base.OnRenderFrame(args);
            }
            else
            {
                isStuck = true;
                if (!isHit)
                {
                    isHit = true;
                    player.PlayHitSound();
                    timer.Stop();
                }
            }
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            if (KeyboardState.IsKeyDown(Keys.R))
            {
                tree1Z = rand.Next(treeZMin, treeZMax);
                tree2Z = rand.Next(treeZMin, treeZMax);
                tree3Z = rand.Next(treeZMin, treeZMax);
                tree4Z = rand.Next(treeZMin, treeZMax);
                tree5Z = rand.Next(treeZMin, treeZMax);
                tree1X = (float)rand.NextDouble() * (treeXMax - treeXMin) + treeXMin;
                tree2X = (float)rand.NextDouble() * (treeXMax - treeXMin) + treeXMin;
                tree3X = (float)rand.NextDouble() * (treeXMax - treeXMin) + treeXMin;
                tree4X = (float)rand.NextDouble() * (treeXMax - treeXMin) + treeXMin;
                tree5X = (float)rand.NextDouble() * (treeXMax - treeXMin) + treeXMin;
                move.position.X = 0f;
                isStuck = false;
                isHit = false;
                timer.Reset();
            }
            MouseState mouse = MouseState;
            KeyboardState input = KeyboardState;
            base.OnUpdateFrame(args);
            if (!isStuck)
                move.Update(input, args);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            this.width = e.Width;
            this.height = e.Height;
            camera.ResizeUpdate(e);
        }

        void OnLoadRender(ref int vao, ref int vbo, ref int ebo, ref int textureid, ref int texturevbo, ref List<Vector3> vertices, ref uint[] indices, ref List<Vector2> texCoords, String imagePth)
        {
            //  Create VAO
            vao = GL.GenVertexArray();
            //  Bind the VAO
            GL.BindVertexArray(vao);
            //  Create VBO
            vbo = GL.GenBuffer();
            //  Bind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            //  Copy verticles data to the buffer
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * Vector3.SizeInBytes * sizeof(float), vertices.ToArray(), BufferUsageHint.StaticDraw);
            //  Bind a slot number 0
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            //  Enable the slot
            GL.EnableVertexArrayAttrib(vao, 0);
            //  Unbind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // Texture Loading
            textureid = GL.GenTexture(); //Generate empty texture
            GL.ActiveTexture(TextureUnit.Texture0); //Activate the texture in the unit
            GL.BindTexture(TextureTarget.Texture2D, textureid); //Bind texture

            //Texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            //Load image
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult boxTexture = ImageResult.FromStream(File.OpenRead(imagePth), ColorComponents.RedGreenBlueAlpha);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, boxTexture.Width, boxTexture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, boxTexture.Data);

            //Unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);

            ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            //Create, bind texture
            texturevbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, texturevbo);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords.Count * Vector2.SizeInBytes * sizeof(float), texCoords.ToArray(), BufferUsageHint.StaticDraw);
            //Point a slot number 1
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            //Enable the slot
            GL.EnableVertexArrayAttrib(vao, 1);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.BindVertexArray(0);
        }

        void RenderObject(int vao, int ebo, Matrix4 model, int textureid, int modelLocation, uint[] indices)
        {
            GL.BindTexture(TextureTarget.Texture2D, textureid);
            GL.UniformMatrix4(modelLocation, true, ref model);
            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
        }
        
    }

    public class Shader
    {
        public int shaderHandle;

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