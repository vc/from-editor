using System;
using OpenGLApp;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace OpenGLApp
{
	public class OpenGlSurface : OpenGLControl
	{
		//======= Новые данные класса
		Color m_BkClr;		// Цвет фона окна
		public int[] m_LightParam = new int[11];	// Параметры освещения
		public uint m_FillMode;		// Режим заполнения полигонов
		public bool m_bQuad;		// Флаг использования GL_QUAD

		float m_AngleX;		// Угол поворота вокруг оси X
		float m_AngleY;		// Угол поворота вокруг оси Y
		float m_AngleView;	// Угол перспективы
		float m_fRangeX;		// Размер объекта вдоль X
		float m_fRangeY;		// Размер объекта вдоль Y
		float m_fRangeZ;		// Размер объекта вдоль Z
		float m_dx;			// Квант смещения вдоль X
		float m_dy;			// Квант смещения вдоль Y
		float m_xTrans;		// Cмещение вдоль X
		float m_yTrans;		// Cмещение вдоль Y
		float m_zTrans;		// Cмещение вдоль Z
		bool m_bCaptured;	// Признак захвата мыши

		Point m_pt;			// Текущая позиция мыши
		uint m_xSize;		// Текущий размер окна вдоль X
		uint m_zSize;		// Текущий размер окна вдоль Y
		Timer timer;

		//====== Массив вершин поверхности
		CPoint3D[] m_cPoints;

		public OpenGlSurface()
		{
			Init(null);
		}

		public OpenGlSurface(float[][] points)
		{
			Init(points);
		}

		public void LoadMap(float[][] points)
		{
			InitGraphic(points);
			DrawScene();
			Invalidate();
		}


		private void Init(float[][] points)
		{
			//====== Начальный разворот изображения
			m_AngleX = 35.0f;
			m_AngleY = 20.0f;

			//====== Угол зрения для матрицы проекции
			m_AngleView = 45.0f;

			//====== Начальный цвет фона
			m_BkClr = Color.FromArgb(0, 0, 96);

			// Начальный режим заполнения внутренних точек полигона
			m_FillMode = GL_LINE;

			//====== Подготовка графика по умолчанию
			if (points == null)
				DefaultGraphic();
			else
			{
				InitGraphic(points);
			}

			//====== Начальное смещение относительно центра сцены
			//====== Сдвиг назад на полуторный размер объекта
			m_zTrans = -1.5f * m_fRangeX;
			m_xTrans = m_yTrans = 0.0f;

			//== Начальные значения квантов смещения (для анимации)
			m_dx = m_dy = 0.0f;

			//====== Мышь не захвачена
			m_bCaptured = false;
			//====== Рисуем четырехугольниками
			m_bQuad = true;

			//====== Начальный значения параметров освещения
			m_LightParam[0] = 50;	// X position
			m_LightParam[1] = 80;	// Y position
			m_LightParam[2] = 100;	// Z position
			m_LightParam[3] = 15;	// Ambient light
			m_LightParam[4] = 70;	// Diffuse light
			m_LightParam[5] = 100;	// Specular light
			m_LightParam[6] = 100;	// Ambient material
			m_LightParam[7] = 100;	// Diffuse material
			m_LightParam[8] = 40;	// Specular material
			m_LightParam[9] = 70;	// Shininess material
			m_LightParam[10] = 0;	// Emission material

			timer = new Timer();
			timer.Interval = 33;
			timer.Tick += new EventHandler(timer_Tick);
		}

		protected override void OnPaint(PaintEventArgs eventArgs)
		{
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
			glMatrixMode(GL_MODELVIEW);
			glLoadIdentity();

			//======= Установка параметров освещения
			SetLight();

			//====== Формирование матрицы моделирования
			glTranslatef(m_xTrans, m_yTrans, m_zTrans);
			glRotatef(m_AngleX, 1, 0, 0);
			glRotatef(m_AngleY, 0, 1, 0);

			//====== Вызов рисующих команд из списка
			glCallList(1);

			//====== Переключение буферов
			SwapBuffers(m_hDC);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			SetProjection();
		}

		protected override PIXELFORMATDESCRIPTOR SetPFD()
		{
			PIXELFORMATDESCRIPTOR pfd;
			pfd.nSize = (ushort)0;
			pfd.nVersion = 1;
			pfd.dwFlags = (uint)(PFD_DRAW_TO_WINDOW | PFD_SUPPORT_OPENGL | PFD_DOUBLEBUFFER);
			pfd.iPixelType = (byte)PFD_TYPE_RGBA;
			pfd.cColorBits = 24;
			pfd.cRedBits = 24;
			pfd.cRedShift = 0;
			pfd.cGreenBits = 24;
			pfd.cGreenShift = 0;
			pfd.cBlueBits = 24;
			pfd.cBlueShift = 0;
			pfd.cAlphaBits = 24;
			pfd.cAlphaShift = 0;
			pfd.cAccumBits = 0;
			pfd.cAccumRedBits = 0;
			pfd.cAccumGreenBits = 0;
			pfd.cAccumBlueBits = 0;
			pfd.cAccumAlphaBits = 0;
			pfd.cDepthBits = 32;
			pfd.cStencilBits = 0;
			pfd.cAuxBuffers = 0;
			pfd.iLayerType = 0;
			pfd.bReserved = 0;
			pfd.dwLayerMask = 0;
			pfd.dwVisibleMask = 0;
			pfd.dwDamageMask = 0;
			return pfd;
		}
		protected void SetProjection()
		{
			//====== Вычисление диспропорций окна
			double dAspect = Height <= Width ? (double)Width / Height : (double)Height / Width;

			glMatrixMode(GL_PROJECTION);
			glLoadIdentity();

			//====== Установка режима перспективной проекции
			gluPerspective(m_AngleView, dAspect, 0.1f, 200);

			//====== Установка прямоугольника просмотра
			glViewport(0, 0, Width, Height);
		}

		protected override void PrepareScene()
		{
			SetProjection();
			//====== Теперь можно посылать команды OpenGL
			glEnable(GL_LIGHTING);			// Будет освещение
			//====== Будет только один источник света
			glEnable(GL_LIGHT0);
			//====== Необходимо учитывать глубину (ось Z)
			glEnable(GL_DEPTH_TEST);
			//====== Необходимо учитывать цвет материала поверхности
			glEnable(GL_COLOR_MATERIAL);

			//====== Устанавливаем цвет фона
			SetBkColor();

			//====== Создаем изображение и запоминаем в списке
			DrawScene();
		}

		private void SetLight()
		{
			//====== Обе поверхности изображения участвуют
			//====== при вычислении цвета пикселов
			//====== при учете параметров освещения
			glLightModeli(GL_LIGHT_MODEL_TWO_SIDE, 1);

			//====== Позиция источника освещения
			//====== зависит от размеров объекта
			float[] fPos =
			{
				(m_LightParam[0]-50) * 10 * m_fRangeX/100,
				(m_LightParam[1]-50) * 10 * m_fRangeY/100,
				(m_LightParam[2]-50) * 5 * m_fRangeZ/100,
				1.0f
			};
			glLightfv(GL_LIGHT0, GL_POSITION, fPos);

			//====== Интенсивность окружающего освещения
			float f = m_LightParam[3] / 100.0f;
			float[] fAmbient = { f, f, f, 0.0f };
			glLightfv(GL_LIGHT0, GL_AMBIENT, fAmbient);

			//====== Интенсивность рассеянного света
			f = m_LightParam[4] / 100.0f;
			float[] fDiffuse = { f, f, f, 0.0f };
			glLightfv(GL_LIGHT0, GL_DIFFUSE, fDiffuse);

			//====== Интенсивность отраженного света
			f = m_LightParam[5] / 100.0f;
			float[] fSpecular = { f, f, f, 0.0f };
			glLightfv(GL_LIGHT0, GL_SPECULAR, fSpecular);

			//====== Отражающие свойства материала
			//====== для разных компонент света
			f = m_LightParam[6] / 100.0f;
			float[] fAmbMat = { f, f, f, 0.0f };
			glMaterialfv(GL_FRONT_AND_BACK, GL_AMBIENT, fAmbMat);

			f = m_LightParam[7] / 100.0f;
			float[] fDifMat = { f, f, f, 1.0f };
			glMaterialfv(GL_FRONT_AND_BACK, GL_DIFFUSE, fDifMat);

			f = m_LightParam[8] / 100.0f;
			float[] fSpecMat = { f, f, f, 0.0f };
			glMaterialfv(GL_FRONT_AND_BACK, GL_SPECULAR, fSpecMat);

			//====== Блесткость материала
			float fShine = 128 * m_LightParam[9] / 100.0f;
			glMaterialf(GL_FRONT_AND_BACK, GL_SHININESS, fShine);

			//====== Излучение света материалом
			f = m_LightParam[10] / 100.0f;
			float[] fEmission = { f, f, f, 0.0f };
			glMaterialfv(GL_FRONT_AND_BACK, GL_EMISSION, fEmission);
		}

		private void SetBkColor()
		{
			//====== Расщепление цвета на три компоненты
			float red = m_BkClr.R / 255.0f,
					green = m_BkClr.G / 255.0f,
					blue = m_BkClr.B / 255.0f;
			//====== Установка цвета фона (стирания) окна
			glClearColor(red, green, blue, 0.0f);

			//====== Непосредственное стирание
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
		}

		/// <summary>
		/// Создание списка рисующих команд
		/// </summary>
		public void DrawScene()
		{
			//====== Создание списка рисующих команд
			glNewList(1, GL_COMPILE);

			//====== Установка режима заполнения
			//====== внутренних точек полигонов
			glPolygonMode(GL_FRONT_AND_BACK, m_FillMode);

			//====== Размеры изображаемого объекта
			uint nx = m_xSize - 1,
					nz = m_zSize - 1;

			//====== Выбор способа создания полигонов
			if (m_bQuad)
				glBegin(GL_QUADS);

			//====== Цикл прохода по слоям изображения (ось Z)
			for (uint z = 0, i = 0; z < nz; z++, i++)
			{
				//====== Связанные полигоны начинаются
				//====== на каждой полосе вновь
				if (!m_bQuad)
					glBegin(GL_QUAD_STRIP);

				//====== Цикл прохода вдоль оси X
				for (uint x = 0; x < nx; x++, i++)
				{
					// i, j, k, n - 4 индекса вершин примитива при
					// обходе в направлении против часовой стрелки

					int j = (int)i + (int)m_xSize,	// Индекс узла с большим Z
						k = j + 1,					// Индекс узла по диагонали
						n = (int)i + 1; 				// Индекс узла справа

					//=== Выбор координат 4-х вершин из контейнера
					float
						xi = m_cPoints[i].x,
						yi = m_cPoints[i].y,
						zi = m_cPoints[i].z,

						xj = m_cPoints[j].x,
						yj = m_cPoints[j].y,
						zj = m_cPoints[j].z,

						xk = m_cPoints[k].x,
						yk = m_cPoints[k].y,
						zk = m_cPoints[k].z,

						xn = m_cPoints[n].x,
						yn = m_cPoints[n].y,
						zn = m_cPoints[n].z,

					//=== Координаты векторов боковых сторон
						ax = xi - xn,
						ay = yi - yn,

						by = yj - yi,
						bz = zj - zi,

					//====== Вычисление вектора нормали
						vx = ay * bz,
						vy = -bz * ax,
						vz = ax * by,

					//====== Модуль нормали
						v = (float)Math.Sqrt(vx * vx + vy * vy + vz * vz);

					//====== Нормировка вектора нормали
					vx /= v;
					vy /= v;
					vz /= v;

					//====== Задание вектора нормали
					glNormal3f(vx, vy, vz);

					// Ветвь создания несвязанных четырехугольников
					if (m_bQuad)
					{
						//====== Обход вершин осуществляется
						//====== в направлении против часовой стрелки
						glColor3f(0.20f, 0.80f, 1.0f);
						glVertex3f(xi, yi, zi);
						glColor3f(0.60f, 0.70f, 1.0f);
						glVertex3f(xj, yj, zj);
						glColor3f(0.70f, 0.90f, 1.0f);
						glVertex3f(xk, yk, zk);
						glColor3f(0.70f, 0.80f, 1.0f);
						glVertex3f(xn, yn, zn);
					}
					else
					//====== Ветвь создания цепочки четырехугольников
					{
						glColor3f(0.90f, 0.90f, 1.0f);
						glVertex3f(xi, yi, zi);
						glColor3f(0.50f, 0.80f, 1.0f);
						glVertex3f(xj, yj, zj);
						if (x == nx - 1)
						{
							glColor3f(0.90f, 0.90f, 1.0f);
							glVertex3f(xn, yn, zn);
							glColor3f(0.50f, 0.80f, 1.0f);
							glVertex3f(xk, yk, zk);
						}
					}
				}
				//====== Закрываем блок команд GL_QUAD_STRIP
				if (!m_bQuad)
					glEnd();
			}
			//====== Закрываем блок команд GL_QUADS
			if (m_bQuad)
				glEnd();

			//====== Закрываем список команд OpenGL
			glEndList();
		}

		private void DefaultGraphic()
		{
			uint size = 33;
			//====== Временный буфер для хранения данных
			float[] buff = new float[size];

			//====== Число ячеек на единицу меньше числа узлов
			uint nz = size - 1,
					nx = size - 1;

			//=== Предварительно вычисляем коэффициенты уравнения
			double fi = Math.Atan(1.0) * 6,
					kx = fi / nx,
					kz = fi / nz;

			//====== В двойном цикле пробега по сетке узлов
			//=== вычисляем и помещаем в буфер данные типа float
			int k = 0;
			uint i, j;
			for (i = 0; i < m_zSize; i++)
			{
				for (j = 0; j < m_xSize; j++)
				{
					buff[k] = (float)(Math.Sin(kz * (i - nz / 2.0)) * Math.Sin(kx * (j - nx / 2.0)));
					k++;
				}
			}
			//InitGraphic(buff);
		}

		private void InitGraphic(float[][] buff)
		{
			//====== Размеры сетки узлов
			m_xSize = (uint)buff.Length;
			m_zSize = (uint)buff[0].Length;

			// Размер буфера для хранения значений функции
			ulong nSize = m_xSize * m_zSize;

			//====== Создание динамического массива m_cPoints
			//====== Готовимся к расшифровке данных буфера
			//====== Проверка на непротиворечивость
			if (m_xSize < 2 || m_zSize < 2 || m_xSize * m_zSize != nSize)
			{
				MessageBox.Show("Данные противоречивы");
				return;
			}

			//====== Изменяем размер контейнера
			//====== При этом его данные разрушаются
			m_cPoints = new CPoint3D[m_xSize * m_zSize];

			if (m_cPoints.Length == 0)
			{
				MessageBox.Show("Не возможно разместить данные");
				return;
			}

			//====== Подготовка к циклу пробега по буферу
			//====== и процессу масштабирования
			float x, z,
				//====== Считываем первую ординату
			fMinY = buff[0][0],
			fMaxY = buff[0][0],
			right = (m_xSize - 1) / 2.0f,
			left = -right,
			rear = (m_zSize - 1) / 2.0f,
			front = -rear,
			range = (right + rear) / 2.0f;

			uint n;

			//====== Вычисление размаха изображаемого объекта
			m_fRangeY = range;
			m_fRangeX = (float)m_xSize;
			m_fRangeZ = (float)m_zSize;

			//====== Величина сдвига вдоль оси Z
			m_zTrans = -1.5f * m_fRangeZ;

			//====== Генерируем координаты сетки (X-Z)
			//====== и совмещаем с ординатами Y из буфера
			int i = 0,
				j = 0;
			for (z = front, i = 0, n = 0; i < m_zSize; i++, z += 1.0f)
			{
				for (x = left, j = 0; j < m_xSize; j++, x += 1.0f, n++)
				{
					if (buff[i][j] > fMaxY)
						fMaxY = buff[i][j];			// Претендент на максимум
					else if (buff[i][j] < fMinY)
						fMinY = buff[i][j];			// Претендент на минимум
					m_cPoints[n] = new CPoint3D(x, z, buff[i][j]);
				}
			}

			//====== Масштабирование ординат
			float zoom = fMaxY > fMinY ? range / (fMaxY - fMinY) : 1.0f;

			for (n = 0; n < m_xSize * m_zSize; n++)
			{
				m_cPoints[n].y = zoom * (m_cPoints[n].y - fMinY) - range / 2.0f;
			}

			//====== Освобождаем временный буфер
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			m_dx = m_dy = 0;

			SetCapture(this.Handle);
			m_bCaptured = true;
			m_pt.X = e.X; m_pt.Y = e.Y;
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (m_bCaptured)
			{
				m_dy = (float)(e.Y - m_pt.Y) / 40.0f;
				m_dx = (float)(e.X - m_pt.X) / 40.0f;

				if (e.Button == MouseButtons.Right)
					m_zTrans += m_dx + m_dy;
				else
				{
					m_AngleX += m_dy;
					m_AngleY += m_dx;
				}
				m_pt.X = e.X; m_pt.Y = e.Y;
				Invalidate();
			}
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			m_yTrans -= e.Delta / 50.0f;
			Invalidate();
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (m_bCaptured)
			{
				if (Math.Abs(m_dx) > 0.5f || Math.Abs(m_dy) > 0.5f)
					timer.Start();
				else
					timer.Stop();

				m_bCaptured = false;
				ReleaseCapture();
			}
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			m_AngleX += m_dy;// Увеличиваем углы поворота
			if (m_AngleX > 360)
				m_AngleX -= 360;
			m_AngleY += m_dx;
			if (m_AngleY > 360)
				m_AngleY -= 360;

			Invalidate();
		}
	}
}
