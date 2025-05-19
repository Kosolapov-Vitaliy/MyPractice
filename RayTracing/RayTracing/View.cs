using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using System;
using System.IO;

namespace RayTracing
{
    internal class View
    {
        private int BasicProgramID;
        private int BasicVertexShader;
        private int BasicFragmentShader;
        private int vbo_position;
        private int vao;
        private Vector3[] vertdata;
        private GameWindow window;
        private float aspectRatio;

        public View(GameWindow window)
        {
            this.window = window;
            aspectRatio = window.Size.X / (float)window.Size.Y;
        }

        public void Initialize()
        {

            string version = GL.GetString(StringName.Version);
            string glslVersion = GL.GetString(StringName.ShadingLanguageVersion);
            Console.WriteLine($"OpenGL: {version}, GLSL: {glslVersion}");

            GL.ClearColor(Color4.White);

            InitShaders();
            SetupVBO();

            GL.GenVertexArrays(1, out vao);
            GL.BindVertexArray(vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_position);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.UseProgram(BasicProgramID);
            var aspectLoc = GL.GetUniformLocation(BasicProgramID, "uAspect");
            GL.Uniform1(aspectLoc, aspectRatio);
        }

        private void InitShaders()
        {
            BasicProgramID = GL.CreateProgram();

            if (!File.Exists(Path.Combine("Shaders", "raytracing.vert")) ||
                !File.Exists(Path.Combine("Shaders", "raytracing.frag")))
            {
                throw new FileNotFoundException("Shader files not found!");
            }

            loadShader("Shaders/raytracing.vert", ShaderType.VertexShader, BasicProgramID, out BasicVertexShader);
            loadShader("Shaders/raytracing.frag", ShaderType.FragmentShader, BasicProgramID, out BasicFragmentShader);

            GL.LinkProgram(BasicProgramID);

            GL.GetProgram(BasicProgramID, GetProgramParameterName.LinkStatus, out int status);
            if (status == 0)
            {
                string log = GL.GetProgramInfoLog(BasicProgramID);
                throw new Exception($"Program linking failed: {log}");
            }

            GL.BindAttribLocation(BasicProgramID, 0, "vPosition");
        }

        private void loadShader(string filename, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type);
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);

            if (!File.Exists(path))
                throw new FileNotFoundException($"Shader file not found: {path}");

            string shaderSource = File.ReadAllText(path);
            GL.ShaderSource(address, shaderSource);
            GL.CompileShader(address);

            GL.GetShader(address, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string log = GL.GetShaderInfoLog(address);
                throw new Exception($"Shader compilation failed ({type}): {log}");
            }

            GL.AttachShader(program, address);
        }

        private void SetupVBO()
        {
            vertdata = new Vector3[]
            {
                new Vector3(-1f, -1f, 0f),
                new Vector3(1f, -1f, 0f),
                new Vector3(1f, 1f, 0f),
                new Vector3(1f, 1f, 0f),
                new Vector3(-1f, 1f, 0f),
                new Vector3(-1f, -1f, 0f)
            };

            GL.GenBuffers(1, out vbo_position);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_position);
            GL.BufferData(BufferTarget.ArrayBuffer, vertdata.Length * Vector3.SizeInBytes, vertdata, BufferUsageHint.StaticDraw);
        }

        public void Render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            GL.UseProgram(BasicProgramID);
            GL.BindVertexArray(vao);

            float currentAspect = window.Size.X / (float)window.Size.Y;
            if (Math.Abs(currentAspect - aspectRatio) > 0.001f)
            {
                aspectRatio = currentAspect;
                var aspectLoc = GL.GetUniformLocation(BasicProgramID, "uAspect");
                GL.Uniform1(aspectLoc, aspectRatio);
            }

            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        }
    }
}