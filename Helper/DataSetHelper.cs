using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using Helper.Logger;

namespace Helper
{
	public class DataSetHelper
	{
		private class FieldInfo
		{
			public string RelationName;
			public string FieldName;	//source table field name
			public string FieldAlias;	//destination table field name
			public string Aggregate;
		}

		public DataSet ds;
		private System.Collections.ArrayList m_FieldInfo; private string m_FieldList;

		public DataSetHelper(ref DataSet DataSet)
		{
			ds = DataSet;
		}
		public DataSetHelper()
		{
			ds = null;
		}

		private void ParseFieldList(string FieldList, bool AllowRelation)
		{
			/*
			 * This code parses FieldList into FieldInfo objects  and then 
			 * adds them to the m_FieldInfo private member
			 * 
			 * FieldList systax:  [relationname.]fieldname[ alias], ...
			*/
			if (m_FieldList == FieldList) return;
			m_FieldInfo = new System.Collections.ArrayList();
			m_FieldList = FieldList;
			FieldInfo Field; string[] FieldParts;
			string[] Fields = FieldList.Split(',');
			int i;
			for (i = 0; i <= Fields.Length - 1; i++)
			{
				Field = new FieldInfo();
				//parse FieldAlias
				FieldParts = Fields[i].Trim().Split(' ');
				switch (FieldParts.Length)
				{
					case 1:
						//to be set at the end of the loop
						break;
					case 2:
						Field.FieldAlias = FieldParts[1];
						break;
					default:
						throw new Exception("Too many spaces in field definition: '" + Fields[i] + "'.");
				}
				//parse FieldName and RelationName
				FieldParts = FieldParts[0].Split('.');
				switch (FieldParts.Length)
				{
					case 1:
						Field.FieldName = FieldParts[0];
						break;
					case 2:
						if (AllowRelation == false)
							throw new Exception("Relation specifiers not permitted in field list: '" + Fields[i] + "'.");
						Field.RelationName = FieldParts[0].Trim();
						Field.FieldName = FieldParts[1].Trim();
						break;
					default:
						throw new Exception("Invalid field definition: " + Fields[i] + "'.");
				}
				if (Field.FieldAlias == null)
					Field.FieldAlias = Field.FieldName;
				m_FieldInfo.Add(Field);
			}
		}

		/// <summary>
		/// dt = dsHelper.CreateJoinTable("TestTable", ds.Tables["Employees"], "FirstName FName,LastName LName,DepartmentEmployee.DepartmentName Department");
		/// [relationname.]fieldname[ alias], ...
		/// </summary>
		/// <param name="TableName"></param>
		/// <param name="SourceTable"></param>
		/// <param name="FieldList"></param>
		/// <returns></returns>
		public DataTable CreateJoinTable(string TableName, DataTable SourceTable, string FieldList)
		{
			/*
			 * Creates a table based on fields of another table and related parent tables
			 * 
			 * FieldList syntax: [relationname.]fieldname[ alias][,[relationname.]fieldname[ alias]]...
			*/
			if (FieldList == null)
			{
				throw new ArgumentException("You must specify at least one field in the field list.");
				//return CreateTable(TableName, SourceTable);
			}
			else
			{
				DataTable dt = new DataTable(TableName);
				ParseFieldList(FieldList, true);
				foreach (FieldInfo Field in m_FieldInfo)
				{
					if (Field.RelationName == null)
					{
						DataColumn dc = SourceTable.Columns[Field.FieldName];
						dt.Columns.Add(dc.ColumnName, dc.DataType, dc.Expression);
					}
					else
					{
						DataColumn dc = SourceTable.ParentRelations[Field.RelationName].ParentTable.Columns[Field.FieldName];
						dt.Columns.Add(dc.ColumnName, dc.DataType, dc.Expression);
					}
				}
				if (ds != null)
					ds.Tables.Add(dt);
				return dt;
			}
		}

		/// <summary>
		/// dsHelper.InsertJoinInto(ds.Tables["TestTable"], ds.Tables["Employees"], "FirstName FName,LastName LName,DepartmentEmployee.DepartmentName Department", "EmployeeID<5", "BirthDate");
		/// </summary>
		/// <param name="DestTable"></param>
		/// <param name="SourceTable"></param>
		/// <param name="FieldList"></param>
		/// <param name="RowFilter"></param>
		/// <param name="Sort"></param>
		public void InsertJoinInto(DataTable DestTable, DataTable SourceTable,
	string FieldList, string RowFilter, string Sort)
		{
			/*
			* Copies the selected rows and columns from SourceTable and inserts them into DestTable
			* FieldList has same format as CreatejoinTable
			*/
			if (FieldList == null)
			{
				throw new ArgumentException("You must specify at least one field in the field list.");
				//InsertInto(DestTable, SourceTable, RowFilter, Sort);
			}
			else
			{
				ParseFieldList(FieldList, true);
				DataRow[] Rows = SourceTable.Select(RowFilter, Sort);
				foreach (DataRow SourceRow in Rows)
				{
					DataRow DestRow = DestTable.NewRow();
					foreach (FieldInfo Field in m_FieldInfo)
					{
						if (Field.RelationName == null)
						{
							DestRow[Field.FieldName] = SourceRow[Field.FieldName];
						}
						else
						{
							DataRow ParentRow = SourceRow.GetParentRow(Field.RelationName);
							DestRow[Field.FieldName] = ParentRow[Field.FieldName];
						}
					}
					DestTable.Rows.Add(DestRow);
				}
			}
		}

		/// <summary>
		/// dt = dsHelper.SelectInto("TestTable", ds.Tables["Employees"], "FirstName FName,LastName LName,DepartmentEmployee.DepartmentName Department", "EmployeeID<5", "BirthDate");
		/// </summary>
		/// <param name="TableName"></param>
		/// <param name="SourceTable"></param>
		/// <param name="FieldList"></param>
		/// <param name="RowFilter"></param>
		/// <param name="Sort"></param>
		/// <returns></returns>
		public DataTable SelectJoinInto(string TableName, DataTable SourceTable, string FieldList, string RowFilter, string Sort)
		{
			/*
			 * Selects sorted, filtered values from one DataTable to another.
			 * Allows you to specify relationname.fieldname in the FieldList to include fields from
			 *  a parent table. The Sort and Filter only apply to the base table and not to related tables.
			*/
			DataTable dt = CreateJoinTable(TableName, SourceTable, FieldList);
			InsertJoinInto(dt, SourceTable, FieldList, RowFilter, Sort);
			return dt;
		}

		#region Static Functions
		private static bool ColumnEqual(object A, object B)
		{

			// Compares two values to see if they are equal. Also compares DBNULL.Value.
			// Note: If your DataTable contains object fields, then you must extend this
			// function to handle them in a meaningful way if you intend to group on them.

			if (A == DBNull.Value && B == DBNull.Value) //  both are DBNull.Value
				return true;
			if (A == DBNull.Value || B == DBNull.Value) //  only one is DBNull.Value
				return false;
			return (A.Equals(B));  // value type standard comparison
		}
		public static DataTable SelectDistinct(string TableName, DataTable SourceTable, string FieldName)
		{
			DataTable dt = new DataTable(TableName);
			dt.Columns.Add(FieldName, SourceTable.Columns[FieldName].DataType);

			List<DataRow> drRet = new List<DataRow>();

			object LastValue = null;
			foreach (DataRow dr in SourceTable.Select("", FieldName))
			{
				if (LastValue == null || !(ColumnEqual(LastValue, dr[FieldName])))
				{
					LastValue = dr[FieldName];
					dt.Rows.Add(new object[] { LastValue });
				}
			}
			return dt;
		}

		public static DataSet TableJoin(DataTable dt1, DataTable dt2)
		{
			DataSet ds = new DataSet("DataSet");

			ds.Tables.Add(dt1);
			ds.Tables.Add(dt2);

			DataRelation drel = new DataRelation("EquiJoin", dt2.Columns["Deptno"], dt1.Columns["Deptno"]);
			ds.Relations.Add(drel);

			DataTable jt = new DataTable("Joinedtable");
			jt.Columns.Add("Eno", typeof(Int32));
			jt.Columns.Add("Ename", typeof(String));
			jt.Columns.Add("Salary", typeof(Double));
			jt.Columns.Add("Deptno", typeof(Int32));
			jt.Columns.Add("Dname", typeof(String));
			ds.Tables.Add(jt);

			foreach (DataRow dr in ds.Tables["Table1"].Rows)
			{
				DataRow parent = dr.GetParentRow("EquiJoin");
				DataRow current = jt.NewRow();
				// Just add all the columns' data in "dr" to the New table.
				for (int i = 0; i < ds.Tables["Table1"].Columns.Count; i++)
				{
					current[i] = dr[i];
				}

				// Add the column that is not present in the child, which is present in the parent.
				current["Dname"] = parent["Dname"];
				jt.Rows.Add(current);
			}

			return ds;
			//dataGridView1.DataSource = ds.Tables["Joinedtable"];
		}
		#endregion

		public static void CopyTables(DataTable dest, DataTable source)
		{
			foreach (DataRow r in source.Rows)
				dest.Rows.Add(r.ItemArray);
		}

		/// <summary>
		/// Взять список таблиц из excel файла
		/// </summary>
		/// <param name="excelFile">Excel файл</param>
		/// <param name="sheets">список таблиц со знаком $ на конце</param>
		/// <returns>список таблиц</returns>
		public static DataSet GetDataSetFromExcel(string excelFile, string[] sheets = null)
		{
			//Запросом тяну данные из файла Excel 2003...
			//List<DataTable> dataTablesRet = new List<DataTable>();
			DataSet ds = new DataSet();

			OleDbConnectionStringBuilder strB = new OleDbConnectionStringBuilder();
			strB.DataSource = excelFile;
			strB.Add("Extended Properties", "Excel 8.0");

			List<string> cs = new List<string>();
			strB.Provider = "Microsoft.ACE.OLEDB.12.0"; cs.Add(strB.ConnectionString);
			strB.Provider = "Microsoft.Jet.OLEDB.4.0"; cs.Add(strB.ConnectionString);

			foreach (string i in cs)
			{
				using (OleDbConnection cn = new OleDbConnection(i))
				{
					//Подключаюсь к файлу
					cn.Open();

					//если листы не указаны, возвращаем все
					if (sheets == null)
					{

						// Получаем списко листов в файле
						DataTable schemaTable =
								cn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,
												new object[] { null, null, null, "TABLE" });

						sheets = new string[schemaTable.Rows.Count];

						// список листов в файле
						for (int r = 0; r < schemaTable.Rows.Count; r++)
							sheets[r] = (string)schemaTable.Rows[r].ItemArray[2];
					}
					foreach (string tableName in sheets)
					{
						string select = String.Format("select * from [{0}]", tableName);
						using (OleDbCommand cmd = new OleDbCommand(select, cn))
						{
							try
							{
								using (OleDbDataReader dr = cmd.ExecuteReader())
								{
									//Выгружаю все данные в автономный кэш
									DataTable dt = new DataTable(tableName);
									dt.Load(dr);
									ds.Tables.Add(dt);
								}
							}
							catch (Exception ex)
							{
								Log.Instance.WriteEntry(Environment.StackTrace, ex);
								continue;
							}
						}
					}
				}
			}

			return ds;
		}
	}


}
