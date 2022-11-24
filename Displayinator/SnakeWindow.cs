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
            -0.8f, 0.8f, 0, // top left         0
            0.8f, 0.8f, 0, // top right         1
            -0.8f, -0.8f, 0, // bottom left     2
            0.8f, -0.8f, 0, // bottom right     3
            0, 0.8f, 0, // top center           4
            -0.8f, 0, 0, // left center         5
            0, -0.8f, 0, // bottom center       6
            0.8f, 0, 0, // right center         7
        };

        _indices = new[]
        {
            0u, 1u, 2u,
            1u, 2u, 3u
        };

        _lineIndices = new[]
        {
            0u, 4u, // top left to center
            4u, 1u, // top right to center
            0u, 5u, // top left to left center
            5u, 2u, // left center to bottom left
            2u, 6u, // bottom eft to bottom center
            6u, 3u, // bottom center to bottom right
            1u, 7u,
            7u, 3u,
            4u, 6u, // top center to bottom center
            5u, 7u,
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
    private Shader _lineShader;

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
        
        
        // lines
        _lineVertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _lineVertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        _lineVertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_lineVertexArrayObject);
        
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, true, 3 * sizeof(float), 0);
        
        GL.EnableVertexAttribArray(0);
        
        _lineElementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _lineElementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _lineIndices.Length * sizeof(uint), _lineIndices, BufferUsageHint.StaticDraw);
        
        
        // set the maximum line width
        float[] linewidths = new [] {0f, 0f};
        GL.GetFloat(GetPName.LineWidth, linewidths);
        GL.LineWidth(linewidths[0]);
        
        _shader = new Shader("simple.vert", "shadey.frag");
        _lineShader = new Shader("line.vert", "shadey.frag");
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
        
        _lineShader.Use();
        
        // Bind the VAO
        GL.BindVertexArray(_lineVertexArrayObject);
        
        GL.DrawElements(PrimitiveType.Lines, _lineIndices.Length, DrawElementsType.UnsignedInt, 0);
        
        SwapBuffers();
    }
    
    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, Size.Y, Size.Y);
    }
}