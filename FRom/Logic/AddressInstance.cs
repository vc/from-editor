using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace FRom.Logic
{
	/// <summary>
	/// Класс описывающий одну карту
	/// </summary>
	[Serializable]
	[XmlRoot("AddressInstance")]
	public partial class AddressInstance : AddressInstanceBase, IXmlSerializable
	{
		#region Members
		// ********* Отображение данных ********* 
		protected string _valueName = "";		//еденица измерения
		protected int _loColor = 0x00ff00;	// Цвет малых значений
		protected int _hiColor = 0xff0000;	// Цвет бОльших значений
		protected int _sizeOfCellX = 28;	// Размер ячейки карты по X
		protected int _sizeOfCellY = 15;	// Размер ячейки карты по Y

		// ********* Данные о картах-спутниках ********* //
		protected string _xSatteliteConstName = "";	// идентификатор карты-спутника по оси X
		protected string _ySatteliteConstName = ""; // идентификатор карты-спутника по оси Y 
		protected bool _hide = false;	// Признак вспомогоательной карты. (Не показывать)
		protected bool _flip = false;	// Поменять местами X и Y
		protected ushort _mask = 0x0;	// маска отображения данных
		protected ViewEnum _mapView = ViewEnum.Scale;	// Отображение данных
		#endregion

		#region Properties
		//[field:NonSerialized]
		//[Browsable(false)]

		/// <summary>
		/// Еденица измерения
		/// </summary>
		[Description("Единца измерения")]
		[DisplayName("Scale value name")]
		[Category(_cView)]
		//[XmlElement()]
		public string ValueName
		{
			get { return _valueName; }
			set { _valueName = value; }
		}

		/// <summary>
		/// Идентификатор карты-спутника по оси X
		/// </summary>
		//[Editor(typeof(EnumGridComboBox), typeof(UITypeEditor))]		
		[Category(_cView), Description("Идентификатор карты-спутника по оси X"), DisplayName("Add map on X")]
		public string XMapConstName
		{
			get { return _xSatteliteConstName; }
			set { _xSatteliteConstName = value; }
		}

		/// <summary>
		/// Идентификатор карты-спутника по оси Y
		/// </summary>
		[Description("Идентификатор карты-спутника по оси Y")]
		[DisplayName("Add map on Y")]
		[Category(_cView)]
		public string YMapConstName
		{
			get { return _ySatteliteConstName; }
			set { _ySatteliteConstName = value; }
		}

		/// <summary>
		/// Признак вспомогоательной карты.
		/// Если true, то карта не показывается в списке выбора
		/// </summary>
		[Description("Признак \"вспомогатльной\" карты. Если true, то карта не будет отображаться в общем списке")]
		[DisplayName("Hidden")]
		[Category(_cView)]
		public bool Hidden
		{
			get { return _hide; }
			set { _hide = value; }
		}

		/// <summary>
		/// Если true, то оси X и Y меняются местами
		/// </summary>
		[Description("Признак флипа осей X и Y. Если true, то ось X встанет вместо Y, а Y вместо X")]
		[DisplayName("Flipped")]
		[Category(_cView)]
		public bool Flipped
		{
			get { return _flip; }
			set { _flip = value; }
		}

		/// <summary>
		/// Размер ячейки таблицы по X
		/// </summary>
		[Description("Размер ячейки таблицы по Горизонтали (X)")]
		[DisplayName("Size of Cell on X")]
		[Category(_cView)]
		public int SizeOfCellX
		{
			get { return _sizeOfCellX; }
			set { _sizeOfCellX = value; }
		}

		/// <summary>
		/// Размер ячейки таблицы по Y
		/// </summary>
		[Description("Размер ячейки таблицы по Горизонтали (Y)")]
		[DisplayName("Size of Cell on Y")]
		[Category(_cView)]
		public int SizeOfCellY
		{
			get { return _sizeOfCellY; }
			set { _sizeOfCellY = value; }
		}

		/// <summary>
		/// Цвет нижних значений в карте
		/// </summary>
		[Description("Цвет малых значений карты")]
		[DisplayName("Color Lo Values")]
		[Category(_cView)]
		public Color ColorLo
		{
			get { return Color.FromArgb((int)((uint)0xff000000 | (uint)_loColor)); }
			set { _loColor = 0xffffff & value.ToArgb(); }
		}

		/// <summary>
		/// Цвет верхних значений в карте
		/// </summary>
		[Description("Цвет больших значений карты")]
		[DisplayName("Color Hi Values")]
		[Category(_cView)]
		public Color ColorHi
		{
			get { return Color.FromArgb((int)((uint)0xff000000 | (uint)_hiColor)); }
			set { _hiColor = 0xffffff & value.ToArgb(); }
		}

		/// <summary>
		/// Маска отображения данных
		/// </summary>
		[Description("Маска отображения данных в двоичном формате")]
		[DisplayName("Mask to view")]
		[Category(_cView)]
		[TypeConverter(typeof(UInt32BinTypeConverter))]
		[System.ComponentModel.Editor("bool", typeof(bool))]
		public ushort Mask
		{
			get { return _mask; }
			set { _mask = value; }
		}

		/// <summary>
		/// Тип отображения данных
		/// </summary>
		[Description("Определяет как отображать данные в таблице")]
		[DisplayName("View function")]
		[Category(_cView)]
		public ViewEnum MapView
		{
			get { return _mapView; }
			set { _mapView = value; }
		}
		#endregion

		#region Methods & Constructors
		//public AddressInstance(AddressInstanceBase _adrRE)
		//{
		//    _byteOnCell = _adrRE.LengthCell;
		//    _columnsOnMap = _adrRE.Columns;
		//    _comment = _adrRE.Comment;
		//    _constName = _adrRE.ConstName;
		//    _countOfCells = _adrRE.CountOfCells;
		//    _mapName = _adrRE.MapName;
		//    _rowsOnMap = _adrRE.Rows;
		//    _scale = _adrRE.ValueOfElement;
		//    _startAddress = _adrRE.StartAdress;
		//    //VerifyVariables(); 
		//}
		public AddressInstance() { }

		/// <summary>
		/// Конструктор копирования
		/// </summary>
		/// <param name="aib"></param>
		public AddressInstance(AddressInstanceBase aib)
			: base(aib)
		{
			AdjustmentAdvancedParams(aib);
		}

		/// <summary>
		/// Производит подстройку дополнительных параметров this.
		/// </summary>
		/// <param name="aib">основные параметры</param>
		private void AdjustmentAdvancedParams(AddressInstanceBase aib)
		{
			switch (aib.ConstName)
			{
				case "HIGH_FUEL":
				case "REG_FUEL":
					this._xSatteliteConstName = "TP_SCALE_FUEL";
					this._ySatteliteConstName = "RPM_SCALE_FUEL";
					this._mapView = ViewEnum.Filtered;
					this._mask = 0x7f;
					break;
				case "HIGH_FIRE":
				case "REG_FIRE":
					this._xSatteliteConstName = "TP_SCALE_FIRE";
					this._ySatteliteConstName = "RPM_SCALE_FIRE";
					this._mapView = ViewEnum.Filtered;
					this._mask = 0x7f;
					break;
				case "TP_SCALE_FIRE":
				case "RPM_SCALE_FIRE":
					this._mapView = ViewEnum.Scale;
					break;
				default:
					this._mapView = ViewEnum.Scale;
					this._mask = 0x0;
					break;
			}
		}

		/// <summary>
		/// Десереализатор
		/// </summary>
		protected AddressInstance(
			  SerializationInfo info,
			  StreamingContext context)
			: base(info, context) { }
		#endregion

		/// <summary>
		/// Переопредленный оператор ToString
		/// </summary>
		/// <returns>Название карты</returns>
		public override string ToString()
		{
			return "[" + _variable + "] " + _mapName;
		}

		#region IXmlSerializable Members
		public System.Xml.Schema.XmlSchema GetSchema()
		{
			throw null;
			//XmlSchema s = new XmlSchema();
		}

		public void ReadXml(System.Xml.XmlReader r)
		{
			bool wasEmpty = r.IsEmptyElement;
			if (wasEmpty)
				return;
			_variable = r.GetAttribute("Map");
			r.Read();							//<AddressInstance>
			_startAddress = Convert.ToUInt32(r.ReadElementContentAsString(), 16);
			_X = r.ReadElementContentAsInt();
			_Y = r.ReadElementContentAsInt();
			_mapSize = r.ReadElementContentAsInt();
			_value = r.ReadElementContentAsInt();
			_byteOnCell = r.ReadElementContentAsInt();
			_mapName = r.ReadElementContentAsString();
			_comment = r.ReadElementContentAsString();

			r.Read();
			{									//<FRom>
				_sizeOfCellX = r.ReadElementContentAsInt();
				_sizeOfCellY = r.ReadElementContentAsInt();
				_loColor = (int)Convert.ToUInt32(r.ReadElementContentAsString(), 16);
				_hiColor = (int)Convert.ToUInt32(r.ReadElementContentAsString(), 16);
				_xSatteliteConstName = r.ReadElementContentAsString();
				_ySatteliteConstName = r.ReadElementContentAsString();
				_hide = r.ReadElementContentAsBoolean();
				_flip = r.ReadElementContentAsBoolean();
				_mask = (byte)r.ReadElementContentAsInt();
				_mapView = (ViewEnum)r.ReadElementContentAsInt();
			} r.ReadEndElement();				//</FRom>
			r.ReadEndElement();					//</AddressInstance>
		}
		//protected byte _mask = 0x7F;					// маска отображения данных
		//protected ViewEnum _mapView = ViewEnum.None;		// Отображение данных
		public void WriteXml(System.Xml.XmlWriter w)
		{
			//"RPM_SCALE_FIRE,&H3BF0,16,1,16,50,RPM scale (Ignition time),123"
			//# Variable,Start-Address,X,Y,Map size,value,Map-name
			w.WriteStartAttribute("Map"); w.WriteValue(_variable);

			w.WriteStartElement("Offset"); w.WriteValue(Convert.ToString(_startAddress, 16)); w.WriteEndElement();
			w.WriteStartElement("Columns"); w.WriteValue(_X); w.WriteEndElement();
			w.WriteStartElement("Rows"); w.WriteValue(_Y); w.WriteEndElement();
			w.WriteStartElement("Cells"); w.WriteValue(_mapSize); w.WriteEndElement();
			w.WriteStartElement("Scale"); w.WriteValue(_value); w.WriteEndElement();
			w.WriteStartElement("ByteOnCell"); w.WriteValue(_byteOnCell); w.WriteEndElement();
			w.WriteStartElement("Name"); w.WriteValue(_mapName); w.WriteEndElement();
			w.WriteStartElement("Comment"); w.WriteValue(_comment); w.WriteEndElement();

			w.WriteStartElement("FRom");
			{
				w.WriteStartElement("ColumnSize"); w.WriteValue(_sizeOfCellX); w.WriteEndElement();
				w.WriteStartElement("RowSize"); w.WriteValue(_sizeOfCellY); w.WriteEndElement();
				w.WriteStartElement("ColorLoValues"); w.WriteValue(Convert.ToString(_loColor, 16)); w.WriteEndElement();
				w.WriteStartElement("ColorHiValues"); w.WriteValue(Convert.ToString(_hiColor, 16)); w.WriteEndElement();
				w.WriteStartElement("X-Map"); w.WriteValue(_xSatteliteConstName); w.WriteEndElement();
				w.WriteStartElement("Y-Map"); w.WriteValue(_ySatteliteConstName); w.WriteEndElement();
				w.WriteStartElement("Hide"); w.WriteValue(_hide); w.WriteEndElement();
				w.WriteStartElement("Flip"); w.WriteValue(_flip); w.WriteEndElement();
				w.WriteStartElement("Mask"); w.WriteValue(_mask); w.WriteEndElement();
				w.WriteStartElement("MapView"); w.WriteValue(Convert.ToInt32(_mapView)); w.WriteEndElement();
			}
			w.WriteEndElement();
		}
		#endregion
	}
}