using System;

namespace FRom.ConsultNS
{
	#region Tyre Enums
	public enum TyreWidth
	{
		_135 = 135,
		_145 = 145,
		_155 = 155,
		_165 = 165,
		_175 = 175,
		_185 = 185,
		_195 = 195,
		_205 = 205,
		_215 = 215,
		_225 = 225,
		_235 = 235,
		_245 = 245,
		_255 = 255,
		_265 = 265,
		_275 = 275,
		_285 = 285,
		_295 = 295,
		_305 = 305,
		_315 = 315,
		_325 = 325,
	}
	public enum TyreProfile
	{
		_35 = 35,
		_40 = 40,
		_45 = 45,
		_50 = 50,
		_55 = 55,
		_60 = 60,
		_65 = 65,
		_70 = 70,
		_75 = 75,
		_80 = 80,
	}
	public enum TyreRadius
	{
		R12 = 12,
		R13 = 13,
		R14 = 14,
		R15 = 15,
		R16 = 16,
		R17 = 17,
		R18 = 18,
		R19 = 19,
		R20 = 20,
		R21 = 21,
		R22 = 22,
		R23 = 23,
		R24 = 24,
	}
	public struct TyreParams
	{
		const float cMilimetersInInch = 25.4f;
		public float Profile;
		public float Width;
		public float Radius;
		public TyreParams(TyreProfile profile, TyreWidth width, TyreRadius radius)
		{
			Profile = (float)profile;
			Width = (float)width / 100f;
			Radius = (float)radius;
		}
		public TyreParams(float profile, float width, float radius)
		{
			Profile = profile;
			Width = width;
			Radius = radius;
		}

		public static TyreParams operator -(TyreParams p1, TyreParams p2)
		{
			return new TyreParams(
				p1.Profile - p2.Profile,
				p1.Width - p2.Width,
				p1.Radius - p2.Radius);
		}

		/// <summary>
		/// Diameter in mm
		/// </summary>
		public float Diameter
		{
			get { return Width * Profile * 2 * Radius * cMilimetersInInch; }
		}

		/// <summary>
		/// Recomended width of disk
		/// </summary>
		public float RecomendedWidthOfDisk
		{
			get { return (float)(Math.Round((1 - Profile * 0.28) * Width / 12.7) / 2); }
		}
	}
	#endregion

	public class TyreCalc
	{
		TyreParams _original;
		TyreParams _new;
		double _k;
		bool _calculated = false;

		/// <summary>
		/// Коэффициент изменеия скорости
		/// </summary>
		public double K
		{
			get
			{
				if (!_calculated)
					_k = CalcK();
				return _k;
			}
		}

		/// <summary>
		/// Расчет коэффициент изменения скорости
		/// </summary>
		private double CalcK()
		{
			_calculated = true;
			return (double)_new.Diameter / (double)_original.Diameter;
		}

		public TyreCalc(TyreParams originTyre, TyreParams newTyre)
		{
			_original = originTyre;
			_new = newTyre;
		}
	}
}

/*
 function sp_f(form)
{
Sp1 = Math.round((Th1/Th)*Sp*100)/100;
Sp2 = Math.round((Sp1-Sp)*100)/100
}
function t_calc(form)
{
<!--   alert(form);-->
//Ширина профиля P, мм
Tw = Wd;
Tw1 = Wdn;
Tw2 = Wdn-Wd;

//Посадочный диаметр обода C, дюймы
Tr = Math.round(Rd);
Tr1 =  Math.round(Rdn);
Tr2 =  Math.round(Rdn)-Math.round(Rd);

//Рекомендуемые размеры диска, дюймы Sp4xdTr1
dTr1 = Math.round(Rdn);
//Рекомендуемые размеры диска, дюймы Sp4xdTr1
Sp4 = Math.round((1-Hdn*0.28)*Wdn/12.7)/2

//Диаметр шины D, мм
Th = Math.round(Wd*Hd*2+Rd*25.4);
Th1 = Math.round(Wdn*Hdn*2+Rdn*25.4);
Th2 = Math.round(Wdn*Hdn*2+Rdn*25.4)-Math.round(Wd*Hd*2+Rd*25.4);

//Изменение клиренса, мм
Kl = Th2 - Th1 / 2;

//Показания спидометра, км/час
Sp

//Реальная скорость, км/час
Sp1 = Math.round((Th1/Th)*Sp*100)/100;

//Погрешность в показаниях спидометра, км/час
Sp2 = Math.round((Sp1-Sp)*100)/100

}
 */
