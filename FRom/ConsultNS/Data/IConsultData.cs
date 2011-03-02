using System;
using System.Collections.Generic;
using System.Text;

namespace FRom.ConsultNS.Data
{
	public interface IConsultData
	{
		/// <summary>
		/// Имя данных
		/// </summary>
		string ToString();

		/// <summary>
		/// Массив байт инициализации CU
		/// </summary>
		byte[] InitBytes { get; }

		/// <summary>
		/// Список всех команд
		/// </summary>
		ListIndexString<ConsultCommand> AllCommands { get; }
		/// <summary>
		/// Список всех сенсоров
		/// </summary>
		ListIndexString<ConsultSensor> AllSensors { get; }
		/// <summary>
		/// Список всех активных тестов
		/// </summary>
		ListIndexString<ConsultActiveTest> AllActiveTests { get; }

		/// <summary>
		/// Список валидных команд
		/// </summary>
		ListIndexString<ConsultCommand> ValidCommands { get; }
		/// <summary>
		/// Список валидных команд
		/// </summary>
		ListIndexString<ConsultSensor> ValidSensors { get; }
		/// <summary>
		/// Список валидных активных тестов
		/// </summary>
		ListIndexString<ConsultActiveTest> ValidActiveTests { get; }

		/// <summary>
		/// Выбрать команду по имени
		/// </summary>
		/// <param name="name">Имя команды</param>
		/// <returns>Команда</returns>
		ConsultCommand GetCommand(ConsultTypeOfCommand name);
		/// <summary>
		/// Выбрать сенсор по имени
		/// </summary>
		/// <param name="name">Имя сенсора</param>
		/// <returns>Сенсор</returns>
		ConsultSensor GetSensor(string name);
		/// <summary>
		/// Выбрать активный тест по имени
		/// </summary>
		/// <param name="name">Имя авктивного теста</param>
		/// <returns>Активный тест</returns>
		ConsultActiveTest GetActiveTest(string name);

		/// <summary>
		/// Добавить валидную команду
		/// </summary>
		/// <param name="name">Имя команды</param>
		void ValidateCommand(ConsultTypeOfCommand name);
		/// <summary>
		/// Добавить валидный сенсор
		/// </summary>
		/// <param name="name">Имя сенсора</param>
		void ValidateSensor(string name);
		/// <summary>
		/// Добавить валидный активный тест
		/// </summary>
		/// <param name="name">Имя активного теста</param>
		void ValidateActiveTest(string name);

		/// <summary>
		/// Добавить валидную команду
		/// </summary>
		/// <param name="name">Имя команды</param>
		void ValidateCommand(ConsultCommand command);
		/// <summary>
		/// Добавить валидный сенсор
		/// </summary>
		/// <param name="name">Имя сенсора</param>
		void ValidateSensor(ConsultSensor sensor);
		/// <summary>
		/// Добавить валидный активный тест
		/// </summary>
		/// <param name="name">Имя активного теста</param>
		void ValidateActiveTest(ConsultActiveTest activeTest);
	}
}
