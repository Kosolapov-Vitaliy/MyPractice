using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

using StbImageSharp;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Pyramid
{
    internal class Game : GameWindow
    {
        int width, height;


        List<string> tex_paths = new List<string>();
        List<Model> all_models = new List<Model>();

        float yRoat = 0.0f;
        int[] VAO;
        int[] VBO;
        int[] EBO;
        int[] textureID;
        int textureVBO;
        Shader shaderProgram = new Shader();
        Camera camera;
        Models md=new Models();
        public Game(int width, int height) : base
        (GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            Console.WriteLine("F11 - FullScreen");
            for (int i = 0; i < md.model.Count; i++)
            {
                all_models.Add(md.model[i]);
                tex_paths.Add(md.tex_paths[i]);
            }

            VAO = new int[all_models.Count];
            VBO = new int[all_models.Count];
            EBO = new int[all_models.Count];
            textureID = new int[all_models.Count];
            this.CenterWindow(new Vector2i(width, height));
            this.height = height; this.width = width;
        }
        protected override void OnLoad()
        {
            base.OnLoad();
            for(int i = 0;i < all_models.Count;i++)
            {
                textureID[i] = GL.GenTexture();
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, textureID[i]);
                GL.TexParameter(TextureTarget.Texture2D,
                TextureParameterName.TextureWrapS, (int)TextureWrapMode.MirroredRepeat);
                GL.TexParameter(TextureTarget.Texture2D,
                TextureParameterName.TextureWrapT, (int)TextureWrapMode.MirroredRepeat);
                GL.TexParameter(TextureTarget.Texture2D,
                TextureParameterName.TextureMinFilter,
                (int)TextureMinFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D,
                TextureParameterName.TextureMagFilter,
                (int)TextureMagFilter.Nearest);

                StbImage.stbi_set_flip_vertically_on_load(1);
                ImageResult boxTexture =
                ImageResult.FromStream(File.OpenRead(tex_paths[i]),
                ColorComponents.RedGreenBlueAlpha);
                GL.TexImage2D(TextureTarget.Texture2D, 0,
                PixelInternalFormat.Rgba, boxTexture.Width, boxTexture.Height, 0,
                PixelFormat.Rgba, PixelType.UnsignedByte, boxTexture.Data);



                List<Vector3> vertices = all_models[i].vertices;
                List<Vector2> texCoords = all_models[i].texCoord;
                uint[] indices = all_models[i].indices;
                VAO[i] = GL.GenVertexArray();
                VBO[i] = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, VBO[i]);
                GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count *
                Vector3.SizeInBytes * sizeof(float), vertices.ToArray(), BufferUsageHint.StaticDraw);
                GL.BindVertexArray(VAO[i]);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
                GL.EnableVertexArrayAttrib(VAO[i], 0);
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                EBO[i] = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO[i]);
                GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length
                    * sizeof(uint), indices, BufferUsageHint.StaticDraw);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

                textureVBO = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO);
                GL.BufferData(BufferTarget.ArrayBuffer, texCoords.Count * Vector3.SizeInBytes *
                sizeof(float), texCoords.ToArray(), BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
                GL.EnableVertexArrayAttrib(VAO[i], 1);

                GL.BindVertexArray(0);
                shaderProgram.LoadShader();

                GL.BindTexture(TextureTarget.Texture2D, 0);
            }
            
            GL.Enable(EnableCap.DepthTest);

            camera = new Camera(width, height, Vector3.Zero);
            CursorState = CursorState.Grabbed;
        }
        protected override void OnUnload()
        {
            base.OnUnload();
            for (int i = 0; i<VAO.Length; i++)
            {
                GL.DeleteBuffer(VAO[i]);
            }
            for (int i = 0; i< VBO.Length; i++)
            {
                GL.DeleteBuffer(VBO[i]);
            }
            for (int i = 0; i < EBO.Length; i++)
            {
                GL.DeleteBuffer(EBO[i]);
            }
            for (int i = 0; i < textureID.Length; i++)
            {
                GL.DeleteTexture(textureID[i]);
            }
            shaderProgram.DeleteShader();
        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(1f, 0.3f, 1f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit| ClearBufferMask.DepthBufferBit);
            yRoat = 0f;
            int scle = 1;
            for (int i = 0; i < all_models.Count; i++)
            {
                uint[] indices = all_models[i].indices;
                shaderProgram.UseShader();
                GL.BindVertexArray(VAO[i]);                
                GL.BindTexture(TextureTarget.Texture2D, textureID[i]); 
                Matrix4 model = Matrix4.Identity;
                Matrix4 view = camera.GetViewMatrix();
                Matrix4 projection = camera.GetProjection();
                Matrix4 translation = Matrix4.CreateTranslation(0f, 0f, 0f);
                if(i==1||i==0||i==2)
                {
                    model = Matrix4.CreateRotationY(yRoat);
                    Matrix4 scale = Matrix4.CreateScale(scle, scle, scle);
                    yRoat += 45f;
                    scle += 1;
                    model *= translation;
                    model*= scale;
                }
                int modelLocation =
                GL.GetUniformLocation(shaderProgram.shaderHandle, "model");
                int viewLocation =
                GL.GetUniformLocation(shaderProgram.shaderHandle, "view");
                int projectionLocation =
                GL.GetUniformLocation(shaderProgram.shaderHandle, "projection");
                GL.UniformMatrix4(modelLocation, true, ref model);
                GL.UniformMatrix4(viewLocation, true, ref view);
                GL.UniformMatrix4(projectionLocation, true, ref projection);

                GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO[i]);
                GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            }                
            
            Context.SwapBuffers();
            base.OnRenderFrame(args);
        }
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            MouseState mouse = MouseState;
            KeyboardState input = KeyboardState;
            base.OnUpdateFrame(args);
            camera.Update(input, mouse, args);
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            if (KeyboardState.IsKeyDown(Keys.F11))
            {
                if (WindowState != WindowState.Fullscreen)
                {
                    WindowState = WindowState.Fullscreen;
                }
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
}

