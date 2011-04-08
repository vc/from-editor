﻿using System;
using System.Data;
using Helper;

namespace InfoLibrary
{
	public enum TableInj
	{
		frame,
		engine,
		inj_type,
		inj_cc,
		fuel_pump,
		comment,
	}
	public enum TableAFM
	{
		engine,
		frame,
		afm_id,
		power,
		color,
		pin,
		size
	}
	public enum TableEngine
	{
		name,
		frame,
		displacement,
		bore_stroke,
		compression,
		chamber_vol,
		piston_crown,
		cylinders,
	}
	public enum TableECU
	{
		model,
		frame,
		transmission,
		rom_id,
		box_no,
		date,
		engine,
		remark
	}

	/// <summary>
	/// Список таблиц
	/// </summary>
	public enum LibTables
	{
		model,
		frame,
		inj_type,
		engine,
		ecu,
		afm,
		inj,
	}



	public class Library
	{
		public enum Tables
		{
			ECU,
			Engine,
			AFM,
			Injector
		}
		EngineInfoCollection _engine;

		const string _cECUTable = "ECU$";
		const string _cEngineTable = "Engine$";
		const string _cAFMTable = "AFM$";
		const string _cInjectorTable = "Injector$";
		string[] _excelSheets = { _cECUTable, _cEngineTable, _cAFMTable, _cInjectorTable };
		DataSet _dsExcel;

		public Library()
		{
			//LibraryDataBase db = new LibraryDataBase();

			// * Create TEMP tables [ECU$, Engine$, AFM$, Injector$]
			string file = @"D:\MyDocs\_Source\_FRom\from-editor\InfoLibrary\data\ecu.gt28.ru.xls";
			_dsExcel = DataSetHelper.GetDataSetFromExcel(file, _excelSheets);

			_engine = new EngineInfoCollection(this);

			//string q = GetCylNumbers("CA18DET");
		}

		public EngineInfoCollection GetEngineCollection()
		{
			return _engine;
		}

		public ECUInfo GetECUInfo(string ecu)
		{
			return null;
		}
		//public string Get

		public DataTable GetDataSource(Tables t)
		{
			switch (t)
			{
				case Tables.ECU:
					return _dsExcel.Tables[_cECUTable];
				case Tables.Engine:
					return _dsExcel.Tables[_cEngineTable];
				case Tables.AFM:
					return _dsExcel.Tables[_cAFMTable];
				case Tables.Injector:
					return _dsExcel.Tables[_cInjectorTable];
				default:
					throw new NotImplementedException();
			}
		}
	}
}
