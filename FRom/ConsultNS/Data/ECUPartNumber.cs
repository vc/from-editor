
using System;
namespace FRom.ConsultNS.Data
{
	public class ConsultECUPartNumber
	{
		/* Read ECU Part Number (0xd0)
		terminate command with (0xf0) to start data stream, stop with (0x30).
		Part Number format is XXXX 23710-YYYYYY. ECU sends frame with 22 data
		bytes (0x16 bytes). Bytes 2 & 3 = XXXX. Bytes 19 to 22 = YYYYYY. 23710 is
		a constant not included in the data.
		Eg. 0488 23710-50F00 where 50F00 is the ECU SW version.*/

		/*
		 * #D0#F0
		 * <30.472 RX>
		 * #2F#FF#16
		 * <30.519 TX>
		 * #30
		 * <30.534 RX>
		 *  00 21 04 88 00 00 00 3F C0 80 C3 01 00 60 00 E0 25 34 39 55 31 30
		 *	 0  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15 16 17 18 19 20 21  
		 * <31.628 RX>
		 * #CF
		 */
		const string _cnst_ECU_number = "23710";

		const int _cLenY = 5;

		const int _cLenX = 2;

		byte[] _data;

		/// <summary>
		/// XXXX 23710-YYYYYY
		/// </summary>
		string ECUNumber
		{
			get
			{
				byte[] arr = new byte[_cLenX];
				Buffer.BlockCopy(_data, 1, arr, 0, _cLenX);

				string str = "";

				foreach (byte i in arr)
					str += Convert.ToString(i, 16).PadLeft(2, '0');

				return str;
			}
		}

		/// <summary>
		/// XXXX 23710-YYYYYY
		/// YYYYYY - Software Version
		/// </summary>
		string ECUSoftwareVersion
		{
			get
			{
				byte[] arr = new byte[_cLenY];
				Buffer.BlockCopy(_data, 22 - _cLenY, arr, 0, _cLenY);

				string str = "";

				foreach (byte i in arr)
					str += Convert.ToChar(i);

				return str;
			}
		}

		public ConsultECUPartNumber(byte[] arr)
		{
			if (arr.Length != 22)
				throw new ConsultException("Длина массива данных ожидается = 22");

			_data = arr;
		}

		/// <summary>
		/// Формирует символьное представление ECU Info
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return ECUNumber + " " + _cnst_ECU_number + "-" + ECUSoftwareVersion;
		}
	}
}
