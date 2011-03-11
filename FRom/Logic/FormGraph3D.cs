using System;
using System.Windows.Forms;
using FRom.Logic;
using OpenGLApp;

namespace FRom
{
	public partial class FormGraph3D : Form
	{
		//==== Создаем элемент управления OpenGL Control
		private OpenGlSurface _ctrlOpenGL;
		private Map _map;
		public FormGraph3D()
		{
			InitializeComponent();
			Init();
		}

		public FormGraph3D(Map map)
		{
			InitializeComponent();
			this._map = map;
			Init();
		}

		/// <summary>
		/// Основная инициализация
		/// </summary>
		private void Init()
		{
			_ctrlOpenGL = new OpenGlSurface(_map.GetBinaryMap());
			_ctrlOpenGL.Location = new System.Drawing.Point(0, 0);
			_ctrlOpenGL.Name = "ctrlOpenGL";
			_ctrlOpenGL.Size = this.Size;// new System.Drawing.Size(472, 456);
			_ctrlOpenGL.TabIndex = 0;
			Controls.Add(_ctrlOpenGL);

			//cmbFill.SelectedIndex = 0;
			//tbSpec.Value = ctrlOpenGL.m_LightParam[5];
			//tbDiff.Value = ctrlOpenGL.m_LightParam[4];
			//tbX.Value = ctrlOpenGL.m_LightParam[0];
			//tbY.Value = ctrlOpenGL.m_LightParam[1];
			//tbZ.Value = ctrlOpenGL.m_LightParam[2];
			//chkQuad.Checked = ctrlOpenGL.m_bQuad == true;
		}

		private void Graph_Load(object sender, EventArgs e)
		{
			cbViewMode.SelectedIndex = 1;
		}

		/// <summary>
		/// Вызывается при изменении карты
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void mapChanged(object sender, EventArgs e)
		{
			Map _map = (sender as ToolStripComboBox).SelectedItem as Map;
			_ctrlOpenGL.LoadMap(_map.GetBinaryMap());
		}

		private void Graph_ResizeEnd(object sender, EventArgs e)
		{
			_ctrlOpenGL.Size = this.Size;
		}

		private void Graph_FormClosed(object sender, FormClosedEventArgs e)
		{
			this.Dispose(true);
		}

		private void cbViewMode_SelectedIndexChanged(object sender, EventArgs e)
		{
			uint[] mode = { OpenGLControl.GL_FILL, OpenGLControl.GL_LINE, OpenGLControl.GL_POINT };
			_ctrlOpenGL.m_FillMode = mode[cbViewMode.SelectedIndex];
			_ctrlOpenGL.DrawScene();
			_ctrlOpenGL.Invalidate();
		}
	}
}
