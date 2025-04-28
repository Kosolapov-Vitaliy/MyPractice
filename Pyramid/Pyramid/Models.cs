using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

using StbImageSharp;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace Pyramid
{
    struct Model
    {
        public List<Vector3> vertices;
        public List<Vector2> texCoord;
        public uint[] indices;
        public Model(List<Vector3> vertices, List<Vector2> texCoord, uint[] indices)
        {
            this.vertices = vertices;
            this.texCoord = texCoord;
            this.indices = indices;
        }
    };
    internal class Models
    {
        public List<Model> model = new List<Model>();
        public List<string> tex_paths = new List<string>();
        public Models()
        {
            model.Add(new Pyramid().pyramid);
            tex_paths.Add("../../../Textures/PYRAMID.jpg");

            model.Add(new Pyramid().pyramid);
            tex_paths.Add("../../../Textures/PYRAMID.jpg");

            model.Add(new Pyramid().pyramid);
            tex_paths.Add("../../../Textures/PYRAMID.jpg");

            model.Add(new Sky().sky);
            tex_paths.Add("../../../Textures/SKYBOX.jpg");

            model.Add(new Desert().desert);
            tex_paths.Add("../../../Textures/DESERT.jpg");

            model.Add(new Bareer().bareer);
            tex_paths.Add("../../../Textures/BAREER.jpg");
        }
        internal class Pyramid
        {
            public Model pyramid;
            public Pyramid()
            {
                pyramid = new Model();
                pyramid.vertices = new List<Vector3>
                {
                    new Vector3(-5f, -5f, 10f),
                    new Vector3(5f, -5f, 10f),
                    new Vector3(0f, 5f, 15f),

                    new Vector3(5f, -5f, 10f),
                    new Vector3(5f, -5f, 20f),
                    new Vector3(0f, 5f, 15f),

                    new Vector3(5f, -5f, 20f),
                    new Vector3(-5f, -5f, 20f),
                    new Vector3(0f, 5f, 15f),

                    new Vector3(-5f, -5f, 20f),
                    new Vector3(-5f, -5f, 10f),
                    new Vector3(0f, 5f, 15f),

                    new Vector3(-5f, -5f, 10f),
                    new Vector3(5f, -5f, 10f),
                    new Vector3(5f, -5f, 20f),
                    new Vector3(-5f, -5f, 20f),
        };
                pyramid.texCoord = new List<Vector2>
                {
                    new Vector2(0f, 0f),
                    new Vector2(10f, 0f),
                    new Vector2(5f, 10f),

                    new Vector2(0f, 0f),
                    new Vector2(10f, 0f),
                    new Vector2(5f, 10f),

                    new Vector2(0f, 0f),
                    new Vector2(10f, 0f),
                    new Vector2(5f, 10f),

                    new Vector2(0f, 0f),
                    new Vector2(10f, 0f),
                    new Vector2(5f, 10f),

                    new Vector2(0f, 10f),
                    new Vector2(10f, 10f),
                    new Vector2(10f, 0f),
                    new Vector2(0f, 0f),
                };
                pyramid.indices= new uint[]
                {
                    0,1,2,

                    3,4,5,

                    6,7,8,

                    9,10,11,

                    12,13,14,
                    14,15,12
                };
            }
        }        
        class Sky
        {
            public Model sky;
            public Sky()
            {
                sky = new Model();
                sky.vertices = new List<Vector3>
                {
                    new Vector3(200f, -5f, 200f),
                    new Vector3(200f, -5f, -200f),
                    new Vector3(200f, 200f, -200f),
                    new Vector3(200f, 200f, 200f),

                    new Vector3(200f, -5f, 200f),
                    new Vector3(-200f, -5f, 200f),
                    new Vector3(-200f, 200f, 200f),
                    new Vector3(200f, 200f, 200f),

                    new Vector3(-200f, -5f, 200f),
                    new Vector3(-200f, -5f, -200f),
                    new Vector3(-200f, 200f, -200f),
                    new Vector3(-200f, 200f, 200f),

                    new Vector3(200f, -5f, -200f),
                    new Vector3(-200f, -5f, -200f),
                    new Vector3(-200f, 200f, -200f),
                    new Vector3(200f, 200f, -200f),

                    new Vector3(200f, 200f, 200f),
                    new Vector3(200f, 200f, -200f),
                    new Vector3(-200f, 200f, -200f),
                    new Vector3(-200f, 200f, 200f),
                };
                sky.texCoord = new List<Vector2>
                {
                    new Vector2(0f, 0.75f),
                    new Vector2(0f, 0.25f),
                    new Vector2(0.25f, 0.25f),
                    new Vector2(0.25f, 0.75f),

                    new Vector2(0.251f, 1f),
                    new Vector2(0.75f, 1f),
                    new Vector2(0.75f, 0.75f),
                    new Vector2(0.251f, 0.75f),

                    new Vector2(1f, 0.75f),
                    new Vector2(1f, 0.251f),
                    new Vector2(0.75f, 0.251f),
                    new Vector2(0.75f, 0.75f),

                    new Vector2(0.251f, 0f),
                    new Vector2(0.75f, 0f),
                    new Vector2(0.75f, 0.251f),
                    new Vector2(0.251f, 0.251f),

                    new Vector2(0.25f, 0.75f),
                    new Vector2(0.25f, 0.25f),
                    new Vector2(0.75f, 0.251f),
                    new Vector2(0.75f, 0.75f),                  
                    
                };
                sky.indices = new uint[] 
                {
                    0,1,2,
                    2,3,0,

                    4,5,6,
                    6,7,4,

                    8,9,10,
                    10,11,8,

                    12,13,14,
                    14,15,12,

                    16,17,18,
                    18,19,16
                };
            }
        }

        class Desert
        {
            public Model desert;
            public Desert()
            {
                desert = new Model();
                desert.vertices = new List<Vector3>
                {
                    new Vector3(200f, -5f, 200f),
                    new Vector3(200f, -5f, -200f),
                    new Vector3(-200f, -5f, -200f),
                    new Vector3(-200f, -5f, 200f),
                };
                desert.texCoord = new List<Vector2>
                {
                    new Vector2(0f, 0f),
                    new Vector2(0f, 200f),
                    new Vector2(200f, 200f),
                    new Vector2(200f, 0f),
                };
                desert.indices = new uint[]
                {
                    0,1,2,
                    2,3,0,
                };
            }
        }
        internal class Bareer
        {
            public Model bareer;
            public Bareer()
            {
                bareer = new Model();
                bareer.vertices = new List<Vector3>
                {
                    new Vector3(-10f, -5f, 5f),
                    new Vector3(10f, -5f, 5f),
                    new Vector3(10f, -4.8f, 5f),
                    new Vector3(-10f, -4.8f, 5f),

                    new Vector3(-10f, -5f, 5f),
                    new Vector3(-10f, -5f, -5f),
                    new Vector3(-10f, -4.8f, -5f),
                    new Vector3(-10f, -4.8f, 5f),

                    new Vector3(-10f, -5f, -5f),
                    new Vector3(10f, -5f, -5f),
                    new Vector3(10f, -4.8f, -5f),
                    new Vector3(-10f, -4.8f, -5f),

                    new Vector3(10f, -5f, -5f),
                    new Vector3(10f, -5f, 5f),
                    new Vector3(10f, -4.8f, 5f),
                    new Vector3(10f, -4.8f, -5f),
        };
                bareer.texCoord = new List<Vector2>
                {
                    new Vector2(0f, 0f),
                    new Vector2(0f, 40f),
                    new Vector2(1f, 40f),
                    new Vector2(1f, 0f),

                    new Vector2(0f, 0f),
                    new Vector2(0f, 20f),
                    new Vector2(1f, 20f),
                    new Vector2(1f, 0f),

                    new Vector2(0f, 0f),
                    new Vector2(0f, 40f),
                    new Vector2(1f, 40f),
                    new Vector2(1f, 0f),

                    new Vector2(0f, 0f),
                    new Vector2(0f, 20f),
                    new Vector2(1f, 20f),
                    new Vector2(1f, 0f),
                };
                bareer.indices = new uint[]
                {
                    0,1,2,
                    2,3,0,

                    4,5,6,
                    6,7,4,

                    8,9,10,
                    10,11,8,

                    12,13,14,
                    14,15,12
                };
            }
        }
    }
}
