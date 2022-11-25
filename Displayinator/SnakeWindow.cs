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


    private Shader _wallShader;
    private Shader _lineShader;
    private Shader _floorShader;

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
        GL.BufferData(BufferTarget.ElementArrayBuffer, _wallIndices.Length * sizeof(uint), _wallIndices, BufferUsageHint.StaticDraw);
        
        
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
        
        
        // floors
        _floorVertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _floorVertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        _floorVertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_floorVertexArrayObject);
        
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, true, 3 * sizeof(float), 0);
        
        GL.EnableVertexAttribArray(0);
        
        _floorElementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _floorElementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _floorIndices.Length * sizeof(uint), _floorIndices, BufferUsageHint.StaticDraw);
        
        
        // set the maximum line width
        float[] linewidths = new [] {0f, 0f};
        GL.GetFloat(GetPName.LineWidth, linewidths);
        GL.LineWidth(linewidths[0]);
        
        _wallShader = new Shader("simple.vert", "shadey.frag");
        _lineShader = new Shader("line.vert", "shadey.frag");
        _floorShader = new Shader("floor.vert", "shadey.frag");
        _wallShader.Use();
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