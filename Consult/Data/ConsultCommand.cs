
namespace FRom.ConsultNS.Data
{
	public class ConsultCommand
	{
		public readonly ConsultTypeOfCommand _name;
		public readonly byte _command;
		public ConsultCommand(ConsultTypeOfCommand name, byte cmd)
		{
			_name = name;
			_command = cmd;
		}

		/// <summary>
		/// Наименование команды
		/// </summary>
		public ConsultTypeOfCommand Name
		{ get { return _name; } }

		/// <summary>
		/// Команда в виде байта
		/// </summary>
		public byte Command
		{ get { return _command; } }

		/// <summary>
		/// Команда в виде массива байт
		/// </summary>
		public byte[] CommandArr
		{ get { return new byte[] { _command }; } }

		/// <summary>
		/// Для выборки экземпляра класса из списка путем индексирования по строке.
		/// </summary>
		/// <returns>Имя экземпляра</returns>
		public override string ToString()
		{
			return _name.ToString();
		}
	}
}
