#define Testing

using Essentials;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK;
using Shared;

namespace Displayinator;

public class SnakeWindow : GameWindow
{
    private float[] _vertices { get; set; }
    private uint[] _indices { get; set; }
    private uint[] _lineIndices { get; set; }
    
    public SnakeWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
    {
        #if Testing

        _vertices = new[]
        {
            -0.8f, 0.8f, 0,
            0.8f, 0.8f, 0,
            -0.8f, -0.8f, 0,
            0.8f, -0.8f, 0
        };

        _indices = new[]
        {
            0u, 1u, 2u,
            1u, 2u, 3u
        };

        _lineIndices = new[]
        {
            0u, 1u,
            0u, 2u,
            1u, 3u,
            2u, 3u
        };

#endif

    }
    
    // squares
    private int _vertexBufferObject;

    private int _vertexArrayObject;

    private int _elementBufferObject;
    
    
    // lines
    private int _lineVertexBufferObject;

    private int _lineVertexArrayObject;

    private int _lineElementBufferObject;


    private Shader _shader;

    // runs once on loading
    protected override void OnLoad()
    {
        base.OnLoad();
        GL.ClearColor(0.7f, 0.7f, 0.7f, 1.0f);

        // squares
        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);
        
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, true, 3 * sizeof(float), 0);
        
        GL.EnableVertexAttribArray(0);
        
        
        _elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
        
        
        
        _shader = new Shader("simple.vert", "shadey.frag");
        _shader.Use();
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        
        // clear the colors
        GL.Clear(ClearBufferMask.ColorBufferBit);
        
        // Bind the shader
        _shader.Use();

        // Bind the VAO
        GL.BindVertexArray(_vertexArrayObject);
        
        //GL.DrawArrays(PrimitiveType.Triangles, 0, (_vertices.Length/3));
        GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
        
        SwapBuffers();
    }
    
    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, Size.Y, Size.Y);
    }
}