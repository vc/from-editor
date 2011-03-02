using System;
using System.Collections.Generic;
using System.IO;
using FRom.Logger;

namespace FRom.Logic
{
	/// <summary>
	/// Реализация бинарной части класса FRom
	/// </summary>
	public partial class From
	{
		byte[] _bin;					//массив данных (прошивка)
		string _filenameROM;			//файл из которого проичтаны бинарные данные
		Log _logger = Log.Instance;

		/// <summary>
		/// поддерживаемые размеры ROM файлов
		/// </summary>
		static List<int> _supportedROMSizes = new List<int>()
		{
			0x4000,
			0x8000,
		};

		/// <summary>
		/// Конструктор с инициализацией
		/// </summary>
		/// <param name="binFileName">полный путь к бинарному файлу</param>
		/// <param name="addressFileName">полный путь к адресному файлу</param>
		public From(string binFileName, string addressFileName)
		{
			InitMembers();
			OpenROMFile(binFileName);
			OpenAddressFile(addressFileName);
		}

		/// <summary>
		/// Конструктор по умолчанию
		/// </summary>
		public From()
		{
			InitMembers();
		}

		/// <summary>
		/// Инициализация мемберов
		/// </summary>
		private void InitMembers()
		{
			_logger.WriteEntry(this, "Launch FRom Editor Main Module...", Logger.EventEntryType.Event);
			//создается новый словарь
			_stringToAdressMap = new Dictionary<string, AddressInstance>();
		}

		/// <summary>
		/// Взять карту
		/// </summary>
		/// <param name="cnst">Константа, определяющая конкретную карту</param>
		/// <returns>Возвращает класс, полностью описывающий запрошенную карту (описание + данные)</returns>
		public Map GetMap(string cnst)
		{
			//создается новый класс map и инициализируется
			return new Map(this, _stringToAdressMap[cnst]);
		}

		/// <summary>
		/// Инициализация бинарного класса
		/// </summary>
		/// <param name="fileName">Полный путь к .bin файлу</param>
		public void OpenROMFile(string fileName)
		{
			//Проверка существования фала
			if (!File.Exists(fileName)) throw new ArgumentException("File '" + _filenameROM + "' not found!", fileName);

			//Попытка чтения файла
			try
			{
				_bin = File.ReadAllBytes(fileName);
				_filenameROM = fileName;
				if (DataSourceChanged != null)
					DataSourceChanged(this, null);
			}
			catch (Exception e) { throw new FromException("Ошибка чтения из файл: [" + fileName + "]", e); }
		}

		/// <summary>
		/// Инициализация ROM данных из массива
		/// </summary>
		/// <param name="data">массив данных</param>
		public void OpenROMFile(byte[] data)
		{
			//Если размер поддерживается
			if (_supportedROMSizes.Contains(data.Length))
			{
				_bin = data;
				_filenameROM = null;
				if (DataSourceChanged != null)
					DataSourceChanged(this, null);
			}
			else
				throw new FromException(
					String.Format("Размер ROM файла не поддерживается. ({0})", data.Length)
				);
		}

		/// <summary>
		/// Взять массив прошивки
		/// </summary>
		/// <returns>Массив байт</returns>
		internal byte[] GetBin()
		{
			return _bin;
		}

		/// <summary>
		/// Сохранить содержимое бинарного класса в файл .bin
		/// </summary>
		/// <param name="fileName">Полный путь к файлу</param>
		public void SaveBin(string fileName)
		{
			//Проверка инициализирован ли класс
			if (!InitializedBin) throw new Exception("Класс [" + this.ToString() + "] не проинициализирован");

			//Если параметров не передали - сохраняем в текущий файл _filenameBin
			if (fileName == "") fileName = _filenameROM;

			//Создание резервной копии фала, если он уже существует
			CreateBackFile(fileName);

			//Попытка записи данных
			try
			{ File.WriteAllBytes(fileName, _bin); }
			catch (Exception e)
			{ throw new FromException("Ошибка сохранения в файл: [" + fileName + "]", e); }
		}

		/// <summary>
		/// Сохранение ROM в текущий файл
		/// </summary>
		public void SaveBin()
		{
			SaveBin("");
		}

		/// <summary>
		/// Создание резервной копии файла
		/// </summary>
		/// <param name="_filenameBin">Путь к файлу, резервную копию которого необходимо создать</param>
		private void CreateBackFile(string fileName)
		{
			string backFilename = fileName;
			//если файл существует
			if (File.Exists(fileName))
			{
				int i = 0;
				//Ищу имя файла, которое не занято в текущей папке
				do
				{
					backFilename = backFilename.Substring(0, backFilename.LastIndexOf('.')) + ".bak#" + i++;
				} while (File.Exists(backFilename));
				//Создаю резервную копию
				File.Copy(fileName, backFilename);
			}
		}

		/// <summary>
		/// Источник данных для бинарного класса
		/// </summary>
		public string DataSourceROM
		{
			get { return _filenameROM; }
		}

		/// <summary>
		/// Признак инициализации класса Данных
		/// </summary>
		protected bool InitializedBin
		{
			get { return _filenameROM != null && _filenameROM.Length > 0; }
		}

		/// <summary>
		/// True, если клас данных И класс карты инициализированы
		/// </summary>
		public bool Initialized
		{
			get { return InitializedAddr && InitializedBin; }
		}

		/// <summary>
		/// Перезагрузка данных из файлов.
		/// </summary>
		public void Reload()
		{
			if (Initialized)
			{
				string fileAddr = _filenameAddr;
				string fileBin = _filenameROM;
				Clear();
				OpenAddressFile(fileAddr);
				OpenROMFile(fileBin);
			}
			DataSourceChanged(this, null);
		}

		/// <summary>
		/// Сбросить класс
		/// </summary>
		internal void Clear()
		{
			_filenameROM = _filenameAddr = "";
			_stringToAdressMap.Clear();
			_bin = null;
		}
	}
}

