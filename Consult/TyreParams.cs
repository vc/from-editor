﻿using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace FRom.Consult
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
	#endregion


	public class TyreParams : IXmlSerializable
	{
		const float cMilimetersInInch = 25.4f;
		public float Profile;
		public float Width;
		public float Radius;

		public TyreParams() { }
		public TyreParams(TyreWidth width, TyreProfile profile, TyreRadius radius)
		{
			Profile = (float)profile / 100f;
			Width = (float)width;
			Radius = (float)radius;
		}
		public TyreParams(float profile, float width, float radius)
		{
			Profile = profile;
			Width = width;
			Radius = radius;
		}
		public TyreParams(TyreParams t)
		{
			Profile = t.Profile;
			Width = t.Width;
			Radius = t.Radius;
		}

		/// <summary>
		/// Коэффициент разницы двух диаметров
		/// </summary>
		///<param name="originalTyre">Original Tyre</param>
		/// <param name="newTyre">New Tyre</param>
		/// <returns>K</returns>
		public static float CalcK(TyreParams originalTyre, TyreParams newTyre)
		{
			if (originalTyre == null || newTyre == null)
				throw new NullReferenceException();
			return newTyre.Diameter / originalTyre.Diameter;
		}

		public TyreWidth TyreWidth
		{
			get { return (TyreWidth)Width; }
			set { Width = (float)value; }
		}
		public TyreProfile TyreProfile
		{
			get { return (TyreProfile)(Profile * 100); }
			set { Profile = (float)value / 100; }
		}
		public TyreRadius TyreRadius
		{
			get { return (TyreRadius)Radius; }
			set { Radius = (float)value; }
		}

		/// <summary>
		/// Diameter in mm
		/// </summary>
		public float Diameter
		{
			get { return Width * Profile * 2 + Radius * cMilimetersInInch; }
		}

		/// <summary>
		/// Recomended width of disk
		/// </summary>
		public float RecomendedWidthOfDisk
		{
			get { return (float)(Math.Round((1 - Profile * 0.28) * Width / 12.7) / 2); }
		}

		public static TyreParams operator -(TyreParams p1, TyreParams p2)
		{
			return new TyreParams(
				p1.Profile - p2.Profile,
				p1.Width - p2.Width,
				p1.Radius - p2.Radius);
		}

		#region IXmlSerializable Members

		public System.Xml.Schema.XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(System.Xml.XmlReader r)
		{
			if (r.IsEmptyElement)
				return;
			if (r.Read())
			{
				this.TyreWidth = (TyreWidth)r.ReadElementContentAsInt();
				this.TyreProfile = (TyreProfile)r.ReadElementContentAsInt();
				this.TyreRadius = (TyreRadius)r.ReadElementContentAsInt();
			}
		}

		public void WriteXml(System.Xml.XmlWriter w)
		{
			w.WriteStartElement("TyreWidth"); w.WriteValue((int)this.TyreWidth); w.WriteEndElement();
			w.WriteStartElement("TyreProfile"); w.WriteValue((int)this.TyreProfile); w.WriteEndElement();
			w.WriteStartElement("TyreRadius"); w.WriteValue((int)this.TyreRadius); w.WriteEndElement();
		}

		#endregion
	}
}