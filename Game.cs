using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ErrorCode = OpenTK.Graphics.OpenGL4.ErrorCode;
using StbImageSharp;
using opentk_cg.Common;

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

        List<Vector3> bg1_vertices = new List<Vector3>()
        {
            new Vector3(-30f, -8f, -20f),
            new Vector3(-30f, 20f, -20f),
            new Vector3(30f, 20f, -20f),
            new Vector3(30f, -8f, -20f)
        };

        List<Vector3> bg_vertices = new List<Vector3>()
        {
            new Vector3(-100f, 0f, 10f),
            new Vector3(-100f, 0f, -300f),
            new Vector3(100f, 0f, -300f),
            new Vector3(100f, 0f, 10f)
        };

        List<Vector3> tree_vertices = new List<Vector3>()
        {
            new Vector3(-2f, 0f, 0f),
            new Vector3(-2f, 4f, 0f),
            new Vector3(2f, 4f, 0f),
            new Vector3(2f, 0f, 0f)
        };

        List<Vector3> road_vertices = new List<Vector3>()
        {
            new Vector3(-30f, 0.01f, 10f),
            new Vector3(-30f, 0.01f, -100f),
            new Vector3(30f, 0.01f, -100f),
            new Vector3(30f, 0.01f, 10f)
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

        List<Vector2> grassTexCoords = new List<Vector2>()
        {
            new Vector2(-4f, -4f),
            new Vector2(-4f, 4f),
            new Vector2(4f, 4f),
            new Vector2(4f, -4f),
        };

        float[] skyboxVertices = {
            // positions          
            -1.0f,  1.0f, -1.0f,
            -1.0f, -1.0f, -1.0f,
             1.0f, -1.0f, -1.0f,
             1.0f, -1.0f, -1.0f,
             1.0f,  1.0f, -1.0f,
            -1.0f,  1.0f, -1.0f,

            -1.0f, -1.0f,  1.0f,
            -1.0f, -1.0f, -1.0f,
            -1.0f,  1.0f, -1.0f,
            -1.0f,  1.0f, -1.0f,
            -1.0f,  1.0f,  1.0f,
            -1.0f, -1.0f,  1.0f,

             1.0f, -1.0f, -1.0f,
             1.0f, -1.0f,  1.0f,
             1.0f,  1.0f,  1.0f,
             1.0f,  1.0f,  1.0f,
             1.0f,  1.0f, -1.0f,
             1.0f, -1.0f, -1.0f,

            -1.0f, -1.0f,  1.0f,
            -1.0f,  1.0f,  1.0f,
             1.0f,  1.0f,  1.0f,
             1.0f,  1.0f,  1.0f,
             1.0f, -1.0f,  1.0f,
            -1.0f, -1.0f,  1.0f,

            -1.0f,  1.0f, -1.0f,
             1.0f,  1.0f, -1.0f,
             1.0f,  1.0f,  1.0f,
             1.0f,  1.0f,  1.0f,
            -1.0f,  1.0f,  1.0f,
            -1.0f,  1.0f, -1.0f,

            -1.0f, -1.0f, -1.0f,
            -1.0f, -1.0f,  1.0f,
             1.0f, -1.0f, -1.0f,
             1.0f, -1.0f, -1.0f,
            -1.0f, -1.0f,  1.0f,
             1.0f, -1.0f,  1.0f
        };

        List<string> faces = new List<string>()
        {
            "right.bmp",
            "left.bmp",
            "top.bmp",
            "bottom.bmp",
            "front.bmp",
            "back.bmp"
        };

        int VAO, bgVAO, treeVAO, roadVAO;
        int VBO, bgVBO, treeVBO, roadVBO;
        int EBO, bgEBO, treeEBO, roadEBO;
        Shader shaderProgram = new Shader();

        int textureID, bgTextureID, treeTextureID, roadTextureID;
        int textureVBO, bgTextureVBO, treeTextureVBO, roadTextureVBO;

        float tree1Z, tree2Z, tree3Z, tree4Z, tree5Z;
        int treeZMin = -300;
        int treeZMax = -200;
        float tree1X, tree2X, tree3X, tree4X, tree5X;
        int treeXMin = -3;
        int treeXMax = 3;
        float[] treesZ_sorted;
        Random rand = new Random();
        bool isStuck = false;
        bool isHit = false;

        MyTimer timer; 

        Camera camera;
        Movement move;

        Vector3 cameraPos = new Vector3(0f, 3f, 8f);
        //Vector3 cameraPos = new Vector3(0f, 0f, 0f);

        OpenALPlayer player;

        Model catModel, orcModel, elfModel;
        Shader modelShader = new Shader();
        int playerNum = 1;

        int skyboxTexture, skyboxVAO, skyboxVBO;
        Shader skyboxShader = new Shader();

        int LevelCounter = 0;

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
            Title = "Tree Dodge Game OpenGL " + GL.GetString(StringName.Version);
            this.height = Size.X;
            this.width = Size.Y;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            Console.WriteLine(GL.GetString(StringName.Renderer));
            ErrorCode error = GL.GetError();

            Matrix4 transform = Matrix4.Identity;
            transform *= Matrix4.CreateScale(1 / 35f);
            transform *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-90f));
            transform *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(180f));
            catModel = new Model("../../../Resources/Cat/12221_Cat_v1_l3.obj", transform);

            transform = Matrix4.Identity;
            transform *= Matrix4.CreateScale(1 / 7f);
            transform *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(180f));
            orcModel = new Model("../../../Resources/Orc/Orc-A.obj", transform);

            transform = Matrix4.Identity;
            transform *= Matrix4.CreateScale(1 / 3f);
            transform *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(180f));
            elfModel = new Model("../../../Resources/Elf/ElfGirl.obj", transform);

            modelShader.LoadShader("catShader.vert", "catShader.frag");

            //OnLoadRender(ref VAO,
            //             ref VBO,
            //             ref EBO,
            //             ref textureID,
            //             ref textureVBO,
            //             ref cube_vertices,
            //             ref cube_indices,
            //             ref cube_coords,
            //             "../../../Textures/player.jpg");
            OnLoadRender(ref bgVAO,
                         ref bgVBO,
                         ref bgEBO,
                         ref bgTextureID,
                         ref bgTextureVBO,
                         ref bg_vertices,
                         ref indices2d,
                         ref grassTexCoords,
                         "../../../Textures/grass.jpg");
            OnLoadRender(ref treeVAO,
                         ref treeVBO,
                         ref treeEBO,
                         ref treeTextureID,
                         ref treeTextureVBO,
                         ref tree_vertices,
                         ref indices2d,
                         ref texCoords2d,
                         "../../../Textures/tree.png");
            OnLoadRender(ref roadVAO,
                         ref roadVBO,
                         ref roadEBO,
                         ref roadTextureID,
                         ref roadTextureVBO,
                         ref road_vertices,
                         ref indices2d,
                         ref texCoords2d,
                         "../../../Textures/road.jpg");

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

            shaderProgram.LoadShader("shader.vert", "shader.frag");

            GL.Enable(EnableCap.DepthTest);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.TextureCubeMapSeamless);

            move = new Movement(Vector3.Zero);

            player = new OpenALPlayer();
            player.StartPlayback();

            timer = new MyTimer(GL.GetString(StringName.Renderer));
            timer.Start();

            skyboxVAO = GL.GenVertexArray();
            skyboxVBO = GL.GenBuffer();
            GL.BindVertexArray(skyboxVAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, skyboxVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, skyboxVertices.Length * sizeof(float), skyboxVertices, BufferUsageHint.StaticDraw);

            // Layout 0 - позиции (vec3)
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            // Отвязать
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            skyboxTexture = loadCubemap(faces);

            skyboxShader.LoadShader("skyboxShader.vert", "skyboxShader.frag");
            CursorState = CursorState.Grabbed;
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            ClearObject(VAO, VBO, EBO, textureID, textureVBO);
            ClearObject(bgVAO, bgVBO, bgEBO, bgTextureID, bgTextureVBO);
            ClearObject(treeVAO, treeVBO, treeEBO, treeTextureID, treeTextureVBO);
            ClearObject(roadVAO, roadVBO, roadEBO, roadTextureID, roadTextureVBO);
            shaderProgram.DeleteShader();
            modelShader.DeleteShader();
            skyboxShader.DeleteShader();
            player.StopPlayback();
        }

        void ClearObject(int vao, int vbo, int ebo, int textureid, int texturevbo)
        {
            GL.DeleteBuffer(vao);
            GL.DeleteBuffer(vbo);
            GL.DeleteBuffer(ebo);
            GL.DeleteTexture(textureid);
            GL.DeleteBuffer(texturevbo);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            if (!(move.position.X > tree1X - 0.5f && move.position.X < tree1X + 0.5f && tree1Z > move.position.Z - 0.5f && tree1Z < move.position.Z + 0.5f || 
                move.position.X > tree2X - 0.5f && move.position.X < tree2X + 0.5f && tree2Z > move.position.Z - 0.5f && tree2Z < move.position.Z + 0.5f || 
                move.position.X > tree3X - 0.5f && move.position.X < tree3X + 0.5f && tree3Z > move.position.Z - 0.5f && tree3Z < move.position.Z + 0.5f ||
                move.position.X > tree4X - 0.5f && move.position.X < tree4X + 0.5f && tree4Z > move.position.Z - 0.5f && tree4Z < move.position.Z + 0.5f ||
                move.position.X > tree5X - 0.5f && move.position.X < tree5X + 0.5f && tree5Z > move.position.Z - 0.5f && tree5Z < move.position.Z + 0.5f))
            {
                if (tree1Z > camera.position.Z)
                {
                    tree1Z = rand.Next(treeZMin + 100 + (int)move.position.Z, treeZMax + 100 + (int)move.position.Z);
                    tree1X = (float)rand.NextDouble() * (treeXMax - treeXMin) + treeXMin;
                }
                if (tree2Z > camera.position.Z)
                {
                    tree2Z = rand.Next(treeZMin + 100 + (int)move.position.Z, treeZMax + 100 + (int)move.position.Z);
                    tree2X = (float)rand.NextDouble() * (treeXMax - treeXMin) + treeXMin;
                }
                if (tree3Z > camera.position.Z)
                {
                    tree3Z = rand.Next(treeZMin + 100 + (int)move.position.Z, treeZMax + 100 + (int)move.position.Z);
                    tree3X = (float)rand.NextDouble() * (treeXMax - treeXMin) + treeXMin;
                }
                if (tree4Z > camera.position.Z)
                {
                    tree4Z = rand.Next(treeZMin + 100 + (int)move.position.Z, treeZMax + 100 + (int)move.position.Z);
                    tree4X = (float)rand.NextDouble() * (treeXMax - treeXMin) + treeXMin;
                }
                if (tree5Z > camera.position.Z)
                {
                    tree5Z = rand.Next(treeZMin + 100 + (int)move.position.Z, treeZMax + 100 + (int)move.position.Z);
                    tree5X = (float)rand.NextDouble() * (treeXMax - treeXMin) + treeXMin;
                }
                if (move.position.Z < -110f * (LevelCounter + 1))
                {
                    LevelCounter++;
                }
                treesZ_sorted = [tree1Z, tree2Z, tree3Z, tree4Z, tree5Z];
                Array.Sort(treesZ_sorted);

                //GL.ClearColor(0.3f, 0.3f, 1f, 1f);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                Matrix4 model;
                Matrix4 view = camera.GetViewMatrix(move);
                //Matrix4 view = camera.GetViewMatrix();
                Matrix4 projection = camera.GetProjection();

                //  render skybox
                GL.DepthFunc(DepthFunction.Lequal);
                skyboxShader.UseShader();
                Matrix4 skyboxView = new Matrix4(new Matrix3(view));
                int skyboxViewLocation = GL.GetUniformLocation(skyboxShader.shaderHandle, "view");
                int skyboxProjectionLocation = GL.GetUniformLocation(skyboxShader.shaderHandle, "projection");
                GL.UniformMatrix4(skyboxViewLocation, false, ref skyboxView);
                GL.UniformMatrix4(skyboxProjectionLocation, false, ref projection);
                GL.BindVertexArray(skyboxVAO);
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.TextureCubeMap, skyboxTexture);
                GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
                GL.BindVertexArray(0);
                GL.DepthFunc(DepthFunction.Less);

                //  render cat model
                modelShader.UseShader();

                int catModelLocation = GL.GetUniformLocation(modelShader.shaderHandle, "model");
                int catViewLocation = GL.GetUniformLocation(modelShader.shaderHandle, "view");
                int catProjectionLocation = GL.GetUniformLocation(modelShader.shaderHandle, "projection");

                GL.UniformMatrix4(catViewLocation, true, ref view);
                GL.UniformMatrix4(catProjectionLocation, true, ref projection);

                model = move.GetModel();
                GL.UniformMatrix4(catModelLocation, true, ref model);
                switch (playerNum)
                {
                    case 1:
                        catModel.Draw(modelShader);
                        break;
                    case 2:
                        orcModel.Draw(modelShader);
                        break;
                    case 3:
                        elfModel.Draw(modelShader);
                        break;
                }

                //  render everything else
                shaderProgram.UseShader();

                int modelLocation = GL.GetUniformLocation(shaderProgram.shaderHandle, "model");
                int viewLocation = GL.GetUniformLocation(shaderProgram.shaderHandle, "view");
                int projectionLocation = GL.GetUniformLocation(shaderProgram.shaderHandle, "projection");

                GL.UniformMatrix4(viewLocation, true, ref view);
                GL.UniformMatrix4(projectionLocation, true, ref projection);

                //  render bg
                model = Matrix4.Identity;
                model *= Matrix4.CreateTranslation(0f, 0f, 0f - 110f * LevelCounter);
                RenderObject(roadVAO, roadEBO, model, roadTextureID, modelLocation, indices2d);
                RenderObject(bgVAO, bgEBO, model, bgTextureID, modelLocation, indices2d);
                model = Matrix4.Identity;
                model *= Matrix4.CreateTranslation(0f, 0f, 0f - 110f * (LevelCounter + 1));
                RenderObject(roadVAO, roadEBO, model, roadTextureID, modelLocation, indices2d);
                RenderObject(bgVAO, bgEBO, model, bgTextureID, modelLocation, indices2d);
                model = Matrix4.Identity;
                model *= Matrix4.CreateTranslation(0f, 0f, 0f - 110f * (LevelCounter + 2));
                RenderObject(roadVAO, roadEBO, model, roadTextureID, modelLocation, indices2d);

                //  render cube
                //model = move.GetModel();
                //RenderObject(VAO, EBO, model, textureID, modelLocation, cube_indices);

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
                //tree1Z += 0.005f * timer.Exp();
                //tree2Z += 0.005f * timer.Exp();
                //tree3Z += 0.005f * timer.Exp();
                //tree4Z += 0.005f * timer.Exp();
                //tree5Z += 0.005f * timer.Exp();

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
                move.Reset();
                camera.Reset();
                LevelCounter = 0;
                isStuck = false;
                isHit = false;
                timer.Reset();
            }
            if (KeyboardState.IsKeyDown(Keys.D1))
            {
                playerNum = 1;
            }
            else if (KeyboardState.IsKeyDown(Keys.D2))
            {
                playerNum = 2;
            }
            else if (KeyboardState.IsKeyDown(Keys.D3))
            {
                playerNum = 3;
            }
            KeyboardState input = KeyboardState;
            MouseState mouse = MouseState;
            base.OnUpdateFrame(args);
            //camera.Update(input, mouse, args);
            if (!isStuck)
            {
                move.Update(input, timer.Exp(), args);
                camera.Update(timer.Exp());
            }

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
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

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

        int loadCubemap(List<string> faces)
        {
            int textureID = GL.GenTexture();
            GL.BindTexture(TextureTarget.TextureCubeMap, textureID);

            for (int i = 0; i < faces.Count; i++)
            {
                StbImage.stbi_set_flip_vertically_on_load(0);
                ImageResult skyboxTexture = ImageResult.FromStream(File.OpenRead("../../../Textures/Skybox/" + faces[i]), ColorComponents.RedGreenBlueAlpha);
                GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgba, skyboxTexture.Width, skyboxTexture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, skyboxTexture.Data);
            }
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.BindTexture(TextureTarget.TextureCubeMap, 0);
            return textureID;
        }

    }

}