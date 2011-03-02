using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OpenGLApp
{
	public class OpenGLControl : UserControl
	{
		private Container components = null;

		public OpenGLControl()
		{
			InitializeComponent();
			m_hDC = nullPtr;	// контекст Windows
			m_hRC = nullPtr;
		}

		protected override void Dispose (bool disposing)
		{
			if (disposing)
			{
				if (components != null)
					components.Dispose();
			}
			base.Dispose (disposing);
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion

		// Layer types
		public const sbyte
			PFD_MAIN_PLANE		= 0,
			PFD_OVERLAY_PLANE	= 1,
			PFD_UNDERLAY_PLANE	= -1;
		
		// Pixel types
		public const byte
			PFD_TYPE_RGBA		= 0,
			PFD_TYPE_COLORINDEX	= 1;

		// ListMode
		public const uint
			GL_COMPILE = 0x1300,
			GL_COMPILE_AND_EXECUTE = 0x1301,		
		
			// PIXELFORMATDESCRIPTOR flags
			PFD_DOUBLEBUFFER	= 0x00000001,
			PFD_STEREO			= 0x00000002,
			PFD_DRAW_TO_WINDOW	= 0x00000004,
			PFD_DRAW_TO_BITMAP	= 0x00000008,
			PFD_SUPPORT_GDI		= 0x00000010,
			PFD_SUPPORT_OPENGL	= 0x00000020,
			PFD_GENERIC_FORMAT	= 0x00000040,
			PFD_NEED_PALETTE	= 0x00000080,
			PFD_NEED_SYSTEM_PALETTE = 0x00000100,
			PFD_SWAP_EXCHANGE	= 0x00000200,
			PFD_SWAP_COPY		= 0x00000400,
			PFD_SWAP_LAYER_BUFFERS = 0x00000800,
			PFD_GENERIC_ACCELERATED = 0x00001000,
			PFD_SUPPORT_DIRECTDRAW = 0x00002000,
		
			// AttribMask
			GL_CURRENT_BIT		= 0x00000001,
			GL_POINT_BIT		= 0x00000002,
			GL_LINE_BIT			= 0x00000004,
			GL_POLYGON_BIT		= 0x00000008,
			GL_POLYGON_STIPPLE_BIT = 0x00000010,
			GL_PIXEL_MODE_BIT	= 0x00000020,
			GL_LIGHTING_BIT		= 0x00000040,
			GL_FOG_BIT			= 0x00000080,
			GL_DEPTH_BUFFER_BIT = 0x00000100,
			GL_ACCUM_BUFFER_BIT = 0x00000200,
			GL_STENCIL_BUFFER_BIT = 0x00000400,
			GL_VIEWPORT_BIT		= 0x00000800,
			GL_TRANSFORM_BIT	= 0x00001000,
			GL_ENABLE_BIT		= 0x00002000,
			GL_COLOR_BUFFER_BIT = 0x00004000,
			GL_HINT_BIT			= 0x00008000,
			GL_EVAL_BIT			= 0x00010000,
			GL_LIST_BIT			= 0x00020000,
			GL_TEXTURE_BIT		= 0x00040000,
			GL_SCISSOR_BIT		= 0x00080000,
			GL_ALL_ATTRIB_BITS	= 0x000fffff,
			// MatrixMode
			GL_MODELVIEW		= 0x1700,
			GL_PROJECTION		= 0x1701,
			GL_TEXTURE			= 0x1702,
		
			// BeginMode
			GL_POINTS			= 0x0000,
			GL_LINES			= 0x0001,
			GL_LINE_LOOP		= 0x0002,
			GL_LINE_STRIP		= 0x0003,
			GL_TRIANGLES		= 0x0004,
			GL_TRIANGLE_STRIP	= 0x0005,
			GL_TRIANGLE_FAN		= 0x0006,
			GL_QUADS			= 0x0007,
			GL_QUAD_STRIP		= 0x0008,
			GL_POLYGON			= 0x0009,

			//DrawBufferMode 
			GL_NONE				= 0,
			GL_FRONT_LEFT		= 0x0400,
			GL_FRONT_RIGHT		= 0x0401,
			GL_BACK_LEFT		= 0x0402,
			GL_BACK_RIGHT		= 0x0403,
			GL_FRONT			= 0x0404,
			GL_BACK				= 0x0405,
			GL_LEFT				= 0x0406,
			GL_RIGHT			= 0x0407,
			GL_FRONT_AND_BACK	= 0x0408,
			GL_AUX0				= 0x0409,
			GL_AUX1				= 0x040A,
			GL_AUX2				= 0x040B,
			GL_AUX3				= 0x040C,

			//MaterialParameter
			GL_EMISSION			= 0x1600,
			GL_SHININESS		= 0x1601,
			GL_AMBIENT_AND_DIFFUSE = 0x1602,
			GL_COLOR_INDEXES	= 0x1603,
		
			//GetTarget
			GL_COLOR_MATERIAL	= 0x0B57,
			GL_DEPTH_TEST		= 0x0B71,
			GL_LIGHTING			= 0x0B50,

			//LightName
			GL_LIGHT0			= 0x4000,
			GL_LIGHT1			= 0x4001,

			// PolygonMode
			GL_POINT			= 0x1B00,
			GL_LINE				= 0x1B01,
			GL_FILL				= 0x1B02,

			// LightParameter
			GL_AMBIENT			= 0x1200,
			GL_DIFFUSE			= 0x1201,
			GL_SPECULAR			= 0x1202,
			GL_POSITION			= 0x1203,

			// GetTarget
			GL_LIGHT_MODEL_TWO_SIDE=0x0B52;
		
		// Structure PIXELFORMATDESCRIPTOR 
		[StructLayout(LayoutKind.Sequential)]
		public struct PIXELFORMATDESCRIPTOR
		{
			public ushort
				nSize, nVersion;
			public uint
				dwFlags, dwLayerMask, dwVisibleMask, dwDamageMask;
			public byte
				iPixelType, cColorBits, cRedBits, cRedShift,
				cGreenBits, cGreenShift, cBlueBits, cBlueShift,
				cAlphaBits, cAlphaShift, cAccumBits, cAccumRedBits,
				cAccumGreenBits, cAccumBlueBits, cAccumAlphaBits,
				cDepthBits, cStencilBits, cAuxBuffers, bReserved;
			public sbyte
				iLayerType;
		}

		// Внешните библиотеки
		public const string
			kernel = "kernel32.dll",
			openGL = "opengl32.dll",
			glu32 = "glu32.dll",
			gdi32 = "gdi32.dll",
			user32 = "user32.dll";	
	
		// Экспортируемые функции из Win32 DLL
		// kernel32.dll 
		[DllImport(kernel)] public static extern uint LoadLibrary( string lpFileName );

		// opengl32.dll 
		[DllImport(openGL)] public static extern uint wglGetCurrentContext();
		[DllImport(openGL)] public static extern IntPtr wglCreateContext (IntPtr hdc);
		[DllImport(openGL)] public static extern int  wglMakeCurrent (IntPtr hdc, IntPtr hglrc);
		[DllImport(openGL)] public static extern int  wglDeleteContext (IntPtr hglrc);
		[DllImport(openGL)] public static extern void glClearColor (float red, float green, float blue, float alpha);
		[DllImport(openGL)] public static extern void glClear (uint mask);
		[DllImport(openGL)] public static extern void glColor3f (float red, float green, float blue);
		[DllImport(openGL)] public static extern void glOrtho (double left, double right, double bottom, double top, double zNear, double zFar);
		[DllImport(openGL)] public static extern void glBegin (uint mode);
		[DllImport(openGL)] public static extern void glVertex2f (float x, float y);
		[DllImport(openGL)] public static extern void glEnd();
		[DllImport(openGL)] public static extern void glFlush();
		[DllImport(openGL)] public static extern void glViewport (int x, int y, int width, int height);
		[DllImport(openGL)] public static extern void glLoadIdentity();
		[DllImport(openGL)] public static extern void glMatrixMode (uint mode);
		[DllImport(openGL)] public static extern void glVertex3d (double x, double y, double z);
		[DllImport(openGL)] public static extern void glVertex3f (float x, float y, float z);
		[DllImport(openGL)] public static extern void glNormal3d (double nx, double ny, double nz);
		[DllImport(openGL)] public static extern void glNormal3f (float nx, float ny, float nz);
		[DllImport(openGL)] public static extern void glColor3d (double red, double green, double blue);
		[DllImport(openGL)] public static extern void glColorMaterial (uint face, uint mode);
		[DllImport(openGL)] public static extern void glEnable (uint cap);
		[DllImport(openGL)] public static extern void glRotated (double angle, double x, double y, double z);
		[DllImport(openGL)] public static extern void glRotatef (float angle, float x, float y, float z);
		[DllImport(openGL)] public static extern void glTranslated (double x, double y, double z);
		[DllImport(openGL)] public static extern void glTranslatef (float x, float y, float z);
		[DllImport(openGL)] public static extern void glNewList (uint list, uint mode);
		[DllImport(openGL)] public static extern void glEndList ();
		[DllImport(openGL)] public static extern void glCallList (uint list);
		[DllImport(openGL)] public static extern void glScaled (double x, double y, double z);
		[DllImport(openGL)] public static extern void glLightModeli (uint pname, int param);
		[DllImport(openGL)] public static extern void glMaterialf (uint face, uint pname, float param);
		[DllImport(openGL)] public static extern void glPolygonMode (uint face, uint mode);

		[DllImport(openGL)] public static extern void glLightfv (uint light, uint pname, float[] param);
		[DllImport(openGL)] public static extern void glMaterialfv (uint face, uint pname, float[] param);

		//glu32.dll
		[DllImport(glu32)] public static extern void gluPerspective(double fovy, double aspect, double zNear, double zFar);

		// gdi32.dll
		[DllImport(gdi32)] public unsafe static extern int ChoosePixelFormat(IntPtr hdc, PIXELFORMATDESCRIPTOR* ppfd);
		[DllImport(gdi32)] public unsafe static extern int SetPixelFormat(IntPtr hdc, int iPixelFormat, PIXELFORMATDESCRIPTOR* ppfd );
		[DllImport(gdi32)] public static extern int  SwapBuffers(IntPtr hdc);

		// user32.dll
		[DllImport(user32)] public static extern IntPtr GetDC(IntPtr hWnd);
		[DllImport(user32)] public static extern int RelaseDC (IntPtr hWnd, uint hDC);
		[DllImport(user32)] public static extern IntPtr SetCapture(IntPtr hWnd);
		[DllImport(user32)] public static extern int ReleaseCapture();
		//==========================================
		private IntPtr nullPtr = new IntPtr(0);
		protected IntPtr m_hDC;	// контекст Windows
		protected IntPtr m_hRC;	// контекст передачи OpenGL
				
		// Инициализирующие действия проще всего 
		// произвести в методе OnCreateControl
		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			OpenGLInit(); //Создаем OpenGL rendering contex
		}

		// Вместо OnDelete() можно использовать 
		// деструктор
		~OpenGLControl()
		{
			wglMakeCurrent(nullPtr, nullPtr);
			if (m_hRC != nullPtr)
				wglDeleteContext (m_hRC);
		}

		// Замещаем методы OnPaint и OnSizeChanged класса Control, 
		// соответствеющие обработчикам WM_PAINT и WM_SIZE
		protected override void OnPaint(PaintEventArgs eventArgs)
		{
			// По умолчанию будем рисовать синий 
			// прямоугольник на черном фоне
			glClearColor (0.0f, 0.0f, 0.0f, 0.0f);
			glClear (GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
			glColor3f (0.0f, 0.0f, 1.0f);
			glOrtho (-1.0, 1.0, -1.0, 1.0, -1.0, 1.0);
			glBegin (GL_POLYGON);
			glVertex2f (-0.5f, -0.5f);
			glVertex2f (-0.5f,  0.5f);
			glVertex2f ( 0.5f,  0.5f);
			glVertex2f ( 0.5f, -0.5f);
			glEnd();
			glFlush();
			SwapBuffers(m_hDC);
		}
		
		protected override void OnSizeChanged(EventArgs e)
		{
			//base.OnSizeChanged(e);
			//При установке размеров прямоугольника просмотра 
			//можно воспользоваться свойствами Height и Width
			//базового класса Control 
			glViewport (0, 0, Width, Height);
			glMatrixMode (GL_PROJECTION);
			glLoadIdentity();
		}

		//Окно OpenGL не должно позволять Windows стирать свой фон
		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			return;
		}

		// Код подготовки окна OpenGL (который в данном
		// случае вызывается из конструктора элемента 
		// управления)
		protected unsafe bool OpenGLInit()
		{
			// "Явная" загрузка OPENGL32.DLL
			// uint m_hModuleOGL = LoadLibrary (openGL);

			// Retrieve a handle to a display device context for the client area of the specified window
			IntPtr hdc = GetDC (Handle);

			// Create a pixel format
			PIXELFORMATDESCRIPTOR pfd=SetPFD();
			pfd.nSize = (ushort)sizeof(PIXELFORMATDESCRIPTOR);

			// Match an appropriate pixel format 
			int iPixelformat;
			if ((iPixelformat = ChoosePixelFormat (hdc, &pfd)) == 0)
				return false;

			// Sets the pixel format
			if (SetPixelFormat (hdc, iPixelformat, &pfd) == 0)
				return false;

			// Create a new OpenGL rendering contex
			m_hRC = wglCreateContext (hdc);
			m_hDC = hdc;

			// Make the OpenGL rendering context the current rendering context
			int rv = wglMakeCurrent (m_hDC, m_hRC);

			PrepareScene();
			return true;
		}
		
		protected virtual PIXELFORMATDESCRIPTOR SetPFD()
		{
			PIXELFORMATDESCRIPTOR pfd;
			//pfd.nSize = (ushort)sizeof(PIXELFORMATDESCRIPTOR);
			pfd.nSize = (ushort)0;
			pfd.nVersion  = 1;
			pfd.dwFlags = (uint)(PFD_DRAW_TO_WINDOW | PFD_SUPPORT_OPENGL | PFD_DOUBLEBUFFER);
			pfd.iPixelType = (byte)PFD_TYPE_RGBA;
			pfd.cColorBits = 24;
			pfd.cRedBits = 0;
			pfd.cRedShift = 0;
			pfd.cGreenBits = 0;
			pfd.cGreenShift = 0;
			pfd.cBlueBits = 0;
			pfd.cBlueShift = 0;
			pfd.cAlphaBits = 0;
			pfd.cAlphaShift = 0;
			pfd.cAccumBits = 0;
			pfd.cAccumRedBits = 0;
			pfd.cAccumGreenBits = 0;
			pfd.cAccumBlueBits = 0;
			pfd.cAccumAlphaBits = 0;
			pfd.cDepthBits = 16;
			pfd.cStencilBits = 0;
			pfd.cAuxBuffers = 0;
			pfd.iLayerType = (sbyte)PFD_MAIN_PLANE;
			pfd.bReserved = 0;
			pfd.dwLayerMask = 0;
			pfd.dwVisibleMask = 0;
			pfd.dwDamageMask = 0;
			return pfd;
		}
		protected virtual void PrepareScene() 
		{
			glViewport(0, 0, Width, Height);
			glMatrixMode(GL_PROJECTION);
			glLoadIdentity();	
		}
	}
	//========== Вспомогательный класс
	public class CPoint3D
	{
		//====== Координаты точки
		public float x = 0, y = 0, z = 0;

		//====== Набор конструкторов
		public CPoint3D () {}
		public CPoint3D (float c1, float c2, float c3)
		{
			x = c1;	z = c2;	y = c3;
		}
	}
}

	
