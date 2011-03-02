using System;
using System.Collections.Generic;
using System.IO;
using FRom.Logic;

namespace FRom
{

	/// <summary>
	/// класс содержит список адресных дирректив RomEdit
	/// </summary>
	public class AddressStructREList : IEnumerable<AddressInstanceBase>
	{
		/// <summary>
		/// Список карт
		/// </summary>
		List<AddressInstanceBase> _maps;

		/// <summary>
		/// Словарь параметров адресного файла
		/// </summary>
		Dictionary<string, string> _params = new Dictionary<string, string>();

		/// <summary>
		/// Лог
		/// </summary>
		private Logger.Log _log = Logger.Log.Instance;

		const int _defaultCountOfMaps = 32;
		public AddressStructREList(string fileName)
		{
			Init(fileName);
		}

		private void Init(string fileName)
		{
			try
			{
				using (StreamReader sr = new StreamReader(fileName))
				{
					_maps = new List<AddressInstanceBase>(_defaultCountOfMaps);
					while (!sr.EndOfStream)
					{
						string line = sr.ReadLine().Trim();

						//Комментарий
						if (line.Length == 0 || line[0] == '#') continue;

						//Параметр
						if (line.Contains("=") && !line.Contains(","))
						{
							AddParam(line);
						}
						//Заглушка для карты TYPE_UNIT
						else if (line.Substring(0, 10).Contains("TYPE_UNIT,"))
						{
							string newLine = "TYPE_UNIT=" + line.Substring(11);
							AddParam(newLine);
						}
						//Инклуд файла
						else if (line.Substring(0, 8).Contains("INCLUDE,"))
						{
							string incFile = line.Substring(8);
							string incPath = Directory.GetParent(fileName).ToString();
							Init(Path.Combine(incPath, incFile));
						}
						//Карта
						else
						{
							try { _maps.Add(new AddressInstanceBase(line)); }
							catch (FromException ex)
							{
								string m = string.Format("Строка проигнорирована: '{1}'{2}{0}", Environment.NewLine, line, ex.Message);
								_log.WriteEntry(this, m, Logger.EventEntryType.Error);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception(String.Format("Ошибка чтения адресного файла '{0}'", fileName), ex);
			}
		}

		/// <summary>
		/// Добавить параметр
		/// </summary>
		/// <param name="line"></param>
		private void AddParam(string line)
		{
			string[] param = line.Split(new char[] { '=' });
			if (param.Length != 2)
				throw new Exception(String.Format("Ошибка расшифровки параметров из строки: {0}{1}Ожидается '<key>,<value>'", line, Environment.NewLine));

			//Если параметр существует, заменим его новым
			if (_params.ContainsKey(param[0]))
				_params.Remove(param[0]);

			_params.Add(param[0], param[1]);
		}

		internal Dictionary<string, string> GetParams()
		{
			return _params;
		}

		#region IEnumerable<AdressStructRE> Members
		public IEnumerator<AddressInstanceBase> GetEnumerator()
		{
			return _maps.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}
