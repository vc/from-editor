using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using FRom.Properties;

namespace FRom.Logic
{
	/// <summary>
	/// Реализация адресной части класса FRom
	/// </summary>	
	partial class From
	{

		/// <summary>
		/// Словарь адресных структур описывающих карты
		/// </summary>
		Dictionary<string, AddressInstance> _stringToAdressMap;

		/// <summary>
		/// Параметры адресной карты
		/// </summary>
		Dictionary<string, string> _params;

		/// <summary>
		/// путь к файлу, из которого прочитаны адресные данные
		/// </summary>
		string _filenameAddr;

		/// <summary>
		/// Инициализация адресных данных из базового adr фала
		/// </summary>
		/// <param name="fileName">Полный путь к файлу для чтения настроек</param>
		private void LoadAddrBase(string fileName)
		{
			//Создается экземпляр базового и инициализируется переданным файлом
			AddressStructREList adrRE = new AddressStructREList(fileName);

			//Создается временный справочник, 
			//чтобы в случае проблем инициализации не испортить текущий
			Dictionary<string, AddressInstance> tmpDictionary = new Dictionary<string, AddressInstance>();

			//Заполняется справочник информацией о картах и константой карты
			foreach (AddressInstanceBase item in adrRE)
			{
				AddressInstance tmp = new AddressInstance(item);
				//Если ключ уже существует, то инкрементируем у него индекс
				while (tmpDictionary.ContainsKey(tmp.ConstName))
				{
					string key = tmp.ConstName;
					for (int i = key.Length - 1; i > 0; i--)
						if (Char.IsDigit(key[i]))
							continue;
						else
						{
							string strKey = key.Substring(0, i + 1);
							string valKey = key.Substring(i + 1);
							int keyNo = valKey == "" ? 1 : Int32.Parse(valKey) + 1;
							key = strKey + keyNo;
							tmp.ConstName = key;
							break;
						}
				}
				tmpDictionary.Add(tmp.ConstName, tmp);
			}
			_params = adrRE.GetParams();
			//Если дошли сюда, значит справочник заполнен. Заменяем.
			_stringToAdressMap = tmpDictionary;
		}

		/// <summary>
		/// Возвращает описание одной карты
		/// </summary>
		/// <param name="key">Константа - карты</param>
		/// <returns>Описание карты</returns>
		public AddressInstance GetAddressInstance(string key)
		{
			//Если запрошенный ключ существует в словаре, то выдается
			if (_stringToAdressMap.ContainsKey(key))
				return _stringToAdressMap[key];
			else
				throw new ArgumentOutOfRangeException("cnst", key, "Map const - '" + key + "' Not Found!");
		}

		/// <summary>
		/// Взять массив карт
		/// </summary>
		/// <returns>Массив карт</returns>
		public Map[] GetAllMaps()
		{
			Map[] arr = new Map[_stringToAdressMap.Count];
			int i = 0;
			foreach (var addr in _stringToAdressMap.Values)
				arr.SetValue(new Map(this, addr), i++);
			return arr;
		}

		#region Properties
		/// <summary>
		/// Для инициализации указать путь к файлу .adr
		/// </summary>
		public string DataSourceAddress
		{
			get { return _filenameAddr; }
		}

		/// <summary>
		/// Событие обновления источника данных класса
		/// </summary>
		public event FromEventHandler DataSourceChanged;

		public delegate void FromEventHandler(object sender, FromEventArgs e);

		/// <summary>
		/// Признак инициализации Адресного класса
		/// </summary>
		protected bool InitializedAddr
		{
			get { return _filenameAddr != null && _filenameAddr.Length > 0; }
		}
		#endregion

		#region Serializers, Deserializers
		/// <summary>
		/// Загрузка адресных данных из файла.
		/// Метод определяется в зависимости от расширения переданного файла
		/// </summary>
		/// <param name="fileName">Полный путь к файлу</param>
		public void OpenAddressFile(string fileName)
		{
			if (File.Exists(fileName))
			{
				switch (fileName.Substring(fileName.LastIndexOf('.') + 1).ToUpper())
				{
					case "FROM":
						using (Stream inStream = new FileStream(fileName, FileMode.Open))
						{
							_stringToAdressMap = (Dictionary<string, AddressInstance>)GetBinarySerializer().Deserialize(inStream);
						}
						break;
					case "XML":
						using (Stream inStream = new FileStream(fileName, FileMode.Open))
						{
							List<AddressInstance> tmp = (List<AddressInstance>)GetXMLSerializer().Deserialize(inStream);
							_stringToAdressMap = new Dictionary<string, AddressInstance>();
							foreach (var item in tmp)
								_stringToAdressMap[item.ConstName] = item;
						}
						break;
					case "ADR": LoadAddrBase(fileName); break;
					default: throw new Exception(String.Format(Resources.strExMsgNotSupportedFileFormat, fileName));
				}
				_filenameAddr = fileName;
				//Дергаем событие смены источника данных
				if (DataSourceChanged != null)
					DataSourceChanged(this, null);
			}
		}

		/// <summary>
		/// Сохранение адресных данных в файл. 
		/// Метод определяется в зависимости от расширения переданного файла
		/// </summary>
		/// <param name="fileName">Полный путь к файлу</param>
		public void SaveAddr(string fileName)
		{
			if (!InitializedAddr) throw new ArgumentNullException("Нечего сохранять");
			switch (fileName.Substring(fileName.LastIndexOf('.') + 1).ToUpper())
			{
				case "": SaveAddr(_filenameAddr); break;
				case "FROM":
					using (FileStream outStream = new FileStream(fileName, FileMode.OpenOrCreate))
					{
						GetBinarySerializer().Serialize(outStream, _stringToAdressMap);
					}
					break;
				case "XML":
					using (FileStream outStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
					{
						List<AddressInstance> tmp = new List<AddressInstance>(_stringToAdressMap.Values);
						GetXMLSerializer().Serialize(outStream, tmp);
					}
					break;
				//case "ADR": LoadFromADR(fileName); break;
				default: throw new Exception(Resources.strExMsgNotSupportedFileFormat);
			}
		}

		XmlSerializer GetXMLSerializer()
		{
			return new XmlSerializer(typeof(List<AddressInstance>));
		}
		BinaryFormatter GetBinarySerializer()
		{
			return new BinaryFormatter();
		}
		#endregion
	}
}
