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

namespace Triangle
{
    internal class Game : GameWindow
    {
        int width, height;
        float[] vertices =
        {
             0f,    0.5f, 0f, //top vertex
            -0.5f, -0.5f, 0f, //bottom left vertex
             0.5f, -0.5f, 0f  // bottom right vertex
        };
        int VAO;
        int VBO;
        Shader shaderProgram =new Shader();
        public Game(int width, int height) : base
        (GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            this.CenterWindow(new Vector2i(width, height));
            this.height = height; this.width = width;
        }
        protected override void OnLoad()
        {
            base.OnLoad();
            VAO = GL.GenVertexArray();
            VBO=GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length
                * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.BindVertexArray(VAO);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexArrayAttrib(VAO, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            shaderProgram.LoadShader();
        }
        protected override void OnUnload()
        {
            base.OnLoad();
            GL.DeleteBuffer(VAO);
            GL.DeleteBuffer(VBO);
            shaderProgram.DeleteShader();
        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(1f, 0.3f, 1f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            
            shaderProgram.UseShader();
            GL.BindVertexArray(VAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            
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
}
