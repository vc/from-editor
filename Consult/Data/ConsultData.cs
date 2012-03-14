using FRom.Consult.Helper;

namespace FRom.Consult.Data
{

	public class ConsultData : IConsultData
	{
		/// <summary>
		/// Источник команд
		/// </summary>
		IConsultDataSource _datasource;

		internal ListIndexString<ConsultCommand> _lstCommands
			= new ListIndexString<ConsultCommand>();

		internal ListIndexString<ConsultSensor> _lstSensors
			= new ListIndexString<ConsultSensor>();

		internal ListIndexString<ConsultActiveTest> _lstActiveTests
			= new ListIndexString<ConsultActiveTest>();

		public ConsultData(IConsultDataSource datasource)
		{
			_datasource = datasource;
		}

		IConsultDataSource DataSource
		{
			get { return _datasource; }
		}

		#region IConsultData Members
		public override string ToString()
		{
			return _datasource.ToString();
		}

		public byte[] InitBytes
		{
			get { return DataSource.InitBytes; }
		}

		public ListIndexString<ConsultCommand> AllCommands
		{
			get { return DataSource.Commands; }
		}

		public ListIndexString<ConsultSensor> AllSensors
		{
			get { return DataSource.Sensors; }
		}

		public ListIndexString<ConsultActiveTest> AllActiveTests
		{
			get { return DataSource.ActiveTests; }
		}

		public ListIndexString<ConsultCommand> ValidCommands
		{
			get { return _lstCommands; }
		}

		public ListIndexString<ConsultSensor> ValidSensors
		{
			get { return _lstSensors; }
		}

		public ListIndexString<ConsultActiveTest> ValidActiveTests
		{
			get { return _lstActiveTests; }
		}

		public ConsultCommand GetCommand(ConsultTypeOfCommand name)
		{
			return DataSource.Commands[name.ToString()];
		}

		public ConsultSensor GetSensor(string name)
		{
			return DataSource.Sensors[name];
		}

		public ConsultActiveTest GetActiveTest(string name)
		{
			return DataSource.ActiveTests[name];
		}

		public void ValidateCommand(ConsultTypeOfCommand name)
		{
			if (_lstCommands[name.ToString()] == null)
				_lstCommands.Add(DataSource.Commands[name.ToString()]);
		}

		public void ValidateSensor(string name)
		{
			if (_lstSensors[name] == null)
				_lstSensors.Add(DataSource.Sensors[name]);
		}

		public void ValidateActiveTest(string name)
		{
			if (_lstActiveTests[name] == null)
				_lstActiveTests.Add(DataSource.ActiveTests[name]);
		}

		public void ValidateCommand(ConsultCommand command)
		{
			if (!_lstCommands.Contains(command))
				_lstCommands.Add(command);
		}

		public void ValidateSensor(ConsultSensor sensor)
		{
			if (!_lstSensors.Contains(sensor))
				_lstSensors.Add(sensor);
		}

		public void ValidateActiveTest(ConsultActiveTest activeTest)
		{
			if (!_lstActiveTests.Contains(activeTest))
				_lstActiveTests.Add(activeTest);
		}

		#endregion
	}
}