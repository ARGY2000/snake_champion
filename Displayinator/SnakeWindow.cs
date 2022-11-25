//#define Testing

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
    private uint[] _wallIndices { get; set; }
    private uint[] _lineIndices { get; set; }
    private uint[] _floorIndices { get; set; }
    
    public SnakeWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings,
        double[] verts, uint[] points, uint[] walls, uint[] floors) 
        : base(gameWindowSettings, nativeWindowSettings)
    {
        _vertices = verts.ConvertToFloats();
        _lineIndices = points;
        _wallIndices = walls;
        _floorIndices = floors;
    }

    #region BufferStuff

    // squares
    private int _vertexBufferObject;

    private int _vertexArrayObject;

    private int _elementBufferObject;
    
    
    // lines
    private int _lineVertexBufferObject;

    private int _lineVertexArrayObject;

    private int _lineElementBufferObject;
    
    
    // floors
    private int _floorVertexBufferObject;
                 
    private int _floorVertexArrayObject;
                 
    private int _floorElementBufferObject;
    
    #endregion


    private Shader _wallShader;
    private Shader _lineShader;
    private Shader _floorShader;
    private Shader _snakeShader;
    private Shader _headShader;
    private Shader _appleShader;

    // runs once on loading
    protected override void OnLoad()
    {
        base.OnLoad();
        GL.ClearColor(0.7f, 0.7f, 0.7f, 1.0f);
        
        
        BufferBinding(ref _vertexBufferObject, ref _vertexArrayObject, ref _elementBufferObject, _wallIndices);
        
        BufferBinding(ref _lineVertexBufferObject, ref _lineVertexArrayObject, ref _lineElementBufferObject, _lineIndices);
        
        BufferBinding(ref _floorVertexBufferObject, ref _floorVertexArrayObject, ref _floorElementBufferObject, _floorIndices);
        
        // set the maximum line width
        float[] linewidths = new [] {0f, 0f};
        GL.GetFloat(GetPName.LineWidth, linewidths);
        GL.LineWidth(linewidths[0]);
        
        _wallShader = new Shader("simple.vert", "shadey.frag");
        _lineShader = new Shader("line.vert", "shadey.frag");
        _floorShader = new Shader("floor.vert", "shadey.frag");
        _snakeShader = new Shader("snake.vert", "shadey.frag");
        _headShader = new Shader("head.vert", "shadey.frag");
        _appleShader = new Shader("apple.vert", "shadey.frag");
        _wallShader.Use();
    }

    private void BufferBinding(ref int vbo, ref int vao, ref int ebo, uint[] indices)
    {
        vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        vao = GL.GenVertexArray();
        GL.BindVertexArray(vao);
        
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, true, 3 * sizeof(float), 0);
        
        GL.EnableVertexAttribArray(0);
        
        ebo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        
        // clear the colors
        GL.Clear(ClearBufferMask.ColorBufferBit);
        
        
        DrawSection(_wallShader, _vertexArrayObject, _wallIndices, PrimitiveType.Triangles);
        DrawSection(_floorShader, _floorVertexArrayObject, _floorIndices, PrimitiveType.Triangles);
        DrawSection(_lineShader, _lineVertexArrayObject, _lineIndices, PrimitiveType.Lines);

        
        SwapBuffers();
    }

    private void DrawSection(Shader shader, int vao, uint[] indices, PrimitiveType primtyp)
    {
        // activate shader
        shader.Use();
        // bind vao
        GL.BindVertexArray(vao);
        // draw on screen
        GL.DrawElements(primtyp, indices.Length, DrawElementsType.UnsignedInt, 0);
    }
    
    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, Size.X, Size.Y);
    }
}