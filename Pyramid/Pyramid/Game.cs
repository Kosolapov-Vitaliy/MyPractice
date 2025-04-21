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


namespace Pyramid
{
    internal class Game : GameWindow
    {
        int width, height;

        
         uint[] indices =
        {
            0,1,2, //top triangle
            2,3,0,//bot triangle

            4,5,6,
            6,7,4,

            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 19, 16,

            20, 21, 22,
            22, 23, 20

        };

        /*
        float[] texCoords =
        {
            0.0f, 1.0f,
            1.0f, 1.0f,
            1.0f, 0.0f,
            0.0f, 0.0f
        };
        */
        List<Vector3> vertices = new List<Vector3>()
        {
        //front face
            new Vector3(-0.5f, 0.5f, 0.5f), 
            new Vector3( 0.5f, 0.5f, 0.5f), 
            new Vector3( 0.5f, -0.5f, 0.5f), 
            new Vector3(-0.5f, -0.5f, 0.5f),
        //right face
            new Vector3( 0.5f, 0.5f, 0.5f),
            new Vector3( 0.5f, 0.5f, -0.5f),
            new Vector3( 0.5f, -0.5f, -0.5f),
            new Vector3( 0.5f, -0.5f, 0.5f),
            //back face
            new Vector3( 0.5f, 0.5f, -0.5f),
            new Vector3( -0.5f, 0.5f, -0.5f),
            new Vector3( -0.5f, -0.5f, -0.5f),
            new Vector3( 0.5f, -0.5f, -0.5f),
            //left face
            new Vector3( -0.5f, 0.5f, -0.5f),
            new Vector3( -0.5f, 0.5f, 0.5f),
            new Vector3( -0.5f, -0.5f, 0.5f),
            new Vector3( -0.5f, -0.5f, -0.5f),
            //top face
            new Vector3( -0.5f, 0.5f, -0.5f),
            new Vector3( 0.5f, 0.5f, -0.5f),
            new Vector3( 0.5f, 0.5f, 0.5f),
            new Vector3( -0.5f, 0.5f, 0.5f),
            //down face
            new Vector3( -0.5f, -0.5f, -0.5f),
            new Vector3( 0.5f, -0.5f, -0.5f),
            new Vector3( 0.5f, -0.5f, 0.5f),
            new Vector3( -0.5f, -0.5f, 0.5f),
        };
        List<Vector2> texCoords = new List<Vector2>()
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
        };

        float yRoat = 0.0f;
        int VAO;
        int VBO;
        int EBO;
        int textureID;
        int textureVBO;
        Shader shaderProgram = new Shader();
        Camera camera;
        public Game(int width, int height) : base
        (GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            this.CenterWindow(new Vector2i(width, height));
            this.height = height; this.width = width;
        }
        protected override void OnLoad()
        {
            base.OnLoad();

            textureID = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            GL.TexParameter(TextureTarget.Texture2D,
            TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D,
            TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D,
            TextureParameterName.TextureMinFilter,
            (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D,
            TextureParameterName.TextureMagFilter,
            (int)TextureMagFilter.Nearest);

            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult boxTexture =
            ImageResult.FromStream(File.OpenRead("../../../Textures/PYRAMID.jpg"),
            ColorComponents.RedGreenBlueAlpha);
            GL.TexImage2D(TextureTarget.Texture2D, 0,
            PixelInternalFormat.Rgba, boxTexture.Width, boxTexture.Height, 0,
            PixelFormat.Rgba, PixelType.UnsignedByte, boxTexture.Data); 


            VAO = GL.GenVertexArray();
            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count *
            Vector3.SizeInBytes * sizeof(float), vertices.ToArray(), BufferUsageHint.StaticDraw);
            GL.BindVertexArray(VAO);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexArrayAttrib(VAO, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length 
                * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            textureVBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords.Count*Vector3.SizeInBytes *
            sizeof(float), texCoords.ToArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexArrayAttrib(VAO, 1);

            GL.BindVertexArray(0);
            shaderProgram.LoadShader();            

            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.Enable(EnableCap.DepthTest);

            camera = new Camera(width, height, Vector3.Zero);
            CursorState = CursorState.Grabbed;
        }
        protected override void OnUnload()
        {
            base.OnUnload();
            GL.DeleteBuffer(VAO);
            GL.DeleteBuffer(VBO);
            GL.DeleteBuffer(EBO);
            GL.DeleteTexture(textureID);
            shaderProgram.DeleteShader();
        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(1f, 0.3f, 1f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit| ClearBufferMask.DepthBufferBit);

            shaderProgram.UseShader();
            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            Matrix4 model = Matrix4.Identity;
            Matrix4 view = camera.GetViewMatrix();
            Matrix4 projection = camera.GetProjection();
            
            model = Matrix4.CreateRotationX(yRoat);
            yRoat +=0.0001f;
            Matrix4 translation = Matrix4.CreateTranslation(0f, 0f, -1.5f);
            model *= translation;
            //model *= Matrix4.CreateRotationZ(yRoat);
            int modelLocation =
            GL.GetUniformLocation(shaderProgram.shaderHandle, "model");
            int viewLocation =
            GL.GetUniformLocation(shaderProgram.shaderHandle, "view");
            int projectionLocation =
            GL.GetUniformLocation(shaderProgram.shaderHandle, "projection");
            GL.UniformMatrix4(modelLocation, true, ref model);
            GL.UniformMatrix4(viewLocation, true, ref view);
            GL.UniformMatrix4(projectionLocation, true, ref projection);
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

