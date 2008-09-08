//
// DateTimeOffsetTest.cs - NUnit Test Cases for the System.DateTimeOffset struct
//
// Authors:
//	Stephane Delcroix  (sdelcroix@novell.com)
//
// Copyright (C) 2007 Novell, Inc (http://www.novell.com)
//

#if NET_2_0
using System.Globalization;
using NUnit.Framework;
using System;

namespace MonoTests.System {

	[TestFixture]
	public class DateTimeOffsetTest
	{
		//ctor exception checking...
		[Test]
		[ExpectedException (typeof (ArgumentException))]
		public void UtcWithWrongOffset ()
		{
			DateTime dt = DateTime.SpecifyKind (DateTime.Now, DateTimeKind.Utc);
			TimeSpan offset = new TimeSpan (2, 0, 0);
			new DateTimeOffset (dt, offset);
		}

		[Test]
		[ExpectedException (typeof (ArgumentException))]
		public void LocalWithWrongOffset ()
		{
			DateTime dt = DateTime.SpecifyKind (DateTime.Now, DateTimeKind.Local);
			TimeSpan offset = TimeZone.CurrentTimeZone.GetUtcOffset (dt) + new TimeSpan (1,0,0);
			new DateTimeOffset (dt, offset);
		}

		[Test]
		[ExpectedException (typeof (ArgumentException))]
		public void OffsetNotInWholeminutes ()
		{
			DateTime dt = DateTime.SpecifyKind (DateTime.Now, DateTimeKind.Unspecified);
			TimeSpan offset = new TimeSpan (1,0,59);
			new DateTimeOffset (dt, offset);
		}

		[Test]
		[ExpectedException (typeof (ArgumentOutOfRangeException))]
		public void OffsetOutOfRange1 ()
		{
			DateTime dt = DateTime.SpecifyKind (DateTime.Now, DateTimeKind.Unspecified);
			TimeSpan offset = new TimeSpan (14, 1, 0);
			new DateTimeOffset (dt, offset);
		}

		[Test]
		[ExpectedException (typeof (ArgumentOutOfRangeException))]
		public void OffsetOutOfRange2 ()
		{
			DateTime dt = DateTime.SpecifyKind (DateTime.Now, DateTimeKind.Unspecified);
			TimeSpan offset = new TimeSpan (-14, -1, 0);
			new DateTimeOffset (dt, offset);
		}

		[Test]
		[ExpectedException (typeof (ArgumentOutOfRangeException))]
		public void UtcDateTimeOutOfRange1 ()
		{
			DateTime dt = DateTime.SpecifyKind (DateTime.MinValue, DateTimeKind.Unspecified);
			TimeSpan offset = new TimeSpan (1, 0, 0);
			new DateTimeOffset (dt, offset);
		}

		[Test]
		[ExpectedException (typeof (ArgumentOutOfRangeException))]
		public void UtcDateTimeOutOfRange2 ()
		{
			DateTime dt = DateTime.SpecifyKind (DateTime.MaxValue, DateTimeKind.Unspecified);
			TimeSpan offset = new TimeSpan (-1, 0, 0);
			new DateTimeOffset (dt, offset);
		}

		[Test]
		public void CompareTwoDateInDiffTZ ()
		{
			DateTimeOffset dt1 = new DateTimeOffset (2007, 12, 16, 15, 06, 00, new TimeSpan (1, 0, 0));
			DateTimeOffset dt2 = new DateTimeOffset (2007, 12, 16, 9, 06, 00, new TimeSpan (-5, 0, 0));
			DateTimeOffset dt3 = new DateTimeOffset (2007, 12, 16, 14, 06, 00, new TimeSpan (1, 0, 0));
			object o = dt1;
			Assert.IsTrue (dt1.CompareTo (dt2) == 0);
			Assert.IsTrue (DateTimeOffset.Compare (dt1, dt2) == 0);
			Assert.IsTrue (dt1 == dt2);
			Assert.IsTrue (dt1.Equals (dt2));
			Assert.IsFalse (dt1 == dt3);
			Assert.IsTrue (dt1 != dt3);
			Assert.IsFalse (dt1.EqualsExact (dt2));
			Assert.IsTrue (dt1.CompareTo (dt3) > 0);
			Assert.IsTrue (((IComparable)dt1).CompareTo (o) == 0);
		}

		[Test]
		public void UtcDateTime ()
		{
			DateTimeOffset dt = new DateTimeOffset (2007, 12, 16, 15, 49, 00, new TimeSpan (1, 0, 0));
			object o = dt.UtcDateTime;
			Assert.IsTrue (o is DateTime);
			Assert.IsTrue (dt.UtcDateTime == new DateTime (2007,12,16,14,49,00,DateTimeKind.Utc));
		}

		[Test]
		public void ParameterlessToString ()
		{
			DateTimeOffset dt = new DateTimeOffset (2007, 12, 18, 12, 16, 30, new TimeSpan (1, 0, 0));
			Assert.AreEqual ("12/18/2007 12:16:30 PM +01:00", dt.ToString (new CultureInfo ("en-us")));
			dt = new DateTimeOffset (2007, 12, 18, 12, 16, 30, new TimeSpan (-5, 0, 0));
			Assert.AreEqual ("12/18/2007 12:16:30 PM -05:00", dt.ToString (new CultureInfo ("en-us")));
			dt = new DateTimeOffset (2007, 12, 18, 12, 16, 30, TimeSpan.Zero);
			Assert.AreEqual ("12/18/2007 12:16:30 PM +00:00", dt.ToString (new CultureInfo ("en-us")));
		}

		[Test]
		public void ToStringWithCultureInfo ()
		{
			DateTimeOffset dto = new DateTimeOffset(2007, 5, 1, 9, 0, 0, TimeSpan.Zero);
			Assert.AreEqual ("05/01/2007 09:00:00 +00:00", dto.ToString (CultureInfo.InvariantCulture));
			Assert.AreEqual ("5/1/2007 9:00:00 AM +00:00", dto.ToString (new CultureInfo ("en-us")));
			Assert.AreEqual ("01/05/2007 09:00:00 +00:00", dto.ToString (new CultureInfo ("fr-fr")));
			Assert.AreEqual ("01.05.2007 09:00:00 +00:00", dto.ToString (new CultureInfo ("de-DE")));
			Assert.AreEqual ("01/05/2007 9:00:00 +00:00", dto.ToString (new CultureInfo ("es-ES")));
		}

		[Test]
		[ExpectedException (typeof (FormatException))]
		public void ToStringExceptionalCase1 ()
		{
			DateTimeOffset.Now.ToString (";");
		}

		[Test]
		[ExpectedException (typeof (FormatException))]
		public void ToStringExceptionalCase2 ()
		{
			DateTimeOffset.Now.ToString ("U");
		}

		[Test]
		public void ToStringWithFormat ()
		{
			DateTimeOffset dto = new DateTimeOffset (2007, 10, 31, 21, 0, 0, new TimeSpan(-8, 0, 0));
			Assert.AreEqual ("10/31/2007", dto.ToString ("d", new CultureInfo ("en-us")));
			Assert.AreEqual ("Wednesday, October 31, 2007", dto.ToString ("D", new CultureInfo ("en-us")));
			Assert.AreEqual ("9:00 PM", dto.ToString ("t", new CultureInfo ("en-us")));
			Assert.AreEqual ("9:00:00 PM", dto.ToString ("T", new CultureInfo ("en-us")));
			Assert.AreEqual ("Wednesday, October 31, 2007 9:00 PM", dto.ToString ("f", new CultureInfo ("en-us")));
			Assert.AreEqual ("Wednesday, October 31, 2007 9:00:00 PM", dto.ToString ("F", new CultureInfo ("en-us")));
			Assert.AreEqual ("10/31/2007 9:00 PM", dto.ToString ("g", new CultureInfo ("en-us")));
			Assert.AreEqual ("10/31/2007 9:00:00 PM", dto.ToString ("G", new CultureInfo ("en-us")));
			Assert.AreEqual ("October 31", dto.ToString ("M", new CultureInfo ("en-us")));
			Assert.AreEqual (dto.ToString ("m", new CultureInfo ("en-us")), dto.ToString ("M", new CultureInfo ("en-us")));
			Assert.AreEqual ("Thu, 01 Nov 2007 05:00:00 GMT", dto.ToString ("R", new CultureInfo ("en-us")));
			Assert.AreEqual (dto.ToString ("r", new CultureInfo ("en-us")), dto.ToString ("R", new CultureInfo ("en-us")));
			Assert.AreEqual ("2007-10-31T21:00:00", dto.ToString ("s", new CultureInfo ("en-us")));
			Assert.AreEqual ("2007-11-01 05:00:00Z", dto.ToString ("u", new CultureInfo ("en-us")));
			Assert.AreEqual ("October, 2007", dto.ToString ("Y", new CultureInfo ("en-us")));
			Assert.AreEqual (dto.ToString ("y", new CultureInfo ("en-us")), dto.ToString ("Y", new CultureInfo ("en-us")));
		}

		[Test]
		public void ToStringWithFormatAndCulture ()
		{
			DateTimeOffset dto = new DateTimeOffset (2007, 11, 1, 9, 0, 0, new TimeSpan(-7, 0, 0));
			string format = "dddd, MMM dd yyyy HH:mm:ss zzz";
			if (CultureInfo.CurrentCulture == CultureInfo.InvariantCulture)
				Assert.AreEqual ("Thursday, Nov 01 2007 09:00:00 -07:00", dto.ToString (format, null as DateTimeFormatInfo), "ts1");
			Assert.AreEqual ("Thursday, Nov 01 2007 09:00:00 -07:00", dto.ToString (format, CultureInfo.InvariantCulture), "ts2");
			Assert.AreEqual ("jeudi, nov. 01 2007 09:00:00 -07:00", dto.ToString (format, new CultureInfo ("fr-FR")), "ts3");
			Assert.AreEqual ("jueves, nov 01 2007 09:00:00 -07:00", dto.ToString (format, new CultureInfo ("es-ES")), "ts4");
		}

		[Test]
		public void ParseExactDatesOnly ()
		{
			DateTimeOffset dto = DateTimeOffset.Now;
			CultureInfo fp = CultureInfo.InvariantCulture;

			string[] formats = {
				"d",
				"M/dd/yyyy",
				"M/d/y",
				"M/d/yy",
				"M/d/yyy",
				"M/d/yyyy",
				"M/d/yyyyy",
				"M/d/yyyyyy",
				"M/dd/yyyy",
				"MM/dd/yyyy",
			};

			foreach (string format in formats)
			{
				string serialized = dto.ToString (format, fp);
				//Console.WriteLine ("{0} {1} {2}", format, dto, serialized);
				Assert.AreEqual (dto.Date, DateTimeOffset.ParseExact (serialized, format, fp).Date, format);

			}
		}

		[Test]
		public void ParseExactTest ()
		{
			CultureInfo fp = CultureInfo.InvariantCulture;
			DateTimeOffset[] dtos = {
				new DateTimeOffset (2008, 01, 14, 13, 21, 0, new TimeSpan (1,0,0)),
				new DateTimeOffset (2008, 01, 14, 13, 21, 0, new TimeSpan (-5,0,0)),
			};

			string[] formats = {
				"M/dd/yyyy HH:m zzz",  "MM/dd/yyyy HH:m zzz",
				"M/d/yyyy HH:m zzz",   "MM/d/yyyy HH:m zzz",
				"M/dd/yy HH:m zzz",    "MM/dd/yy HH:m zzz",
				"M/d/yy HH:m zzz",     "M/d/yy HH:m zzz",
				"M/dd/yyyy H:m zzz",   "MM/dd/yyyy H:m zzz",
				"M/d/yyyy H:m zzz",    "MM/d/yyyy H:m zzz",
				"M/dd/yy H:m zzz",     "MM/dd/yy H:m zzz",
				"M/d/yy H:m zzz",      "MM/d/yy H:m zzz",
				"M/dd/yyyy HH:mm zzz", "MM/dd/yyyy HH:mm zzz",
				"M/d/yyyy HH:mm zzz",  "MM/d/yyyy HH:mm zzz",
				"M/dd/yy HH:mm zzz",   "MM/dd/yy HH:mm zzz",
				"M/d/yy HH:mm zzz",    "M/d/yy HH:mm zzz",
				"M/dd/yyyy H:m zzz",   "MM/dd/yyyy H:m zzz",
				"M/d/yyyy H:m zzz",    "MM/d/yyyy H:m zzz",
				"M/dd/yy H:m zzz",     "MM/dd/yy H:m zzz",
				"M/d/yy H:m zzz",      "M/d/yy H:m zzz",
				"ddd dd MMM yyyy h:mm tt zzz", 
			};

			foreach (DateTimeOffset dto in dtos)
				foreach (string format in formats)
				{
					string serialized = dto.ToString (format, fp);
					//Console.WriteLine ("{0} {1} {2}", format, dto, serialized);
					Assert.AreEqual (dto, DateTimeOffset.ParseExact (serialized, format, fp), format);
				}
		}
		[Test]
		public void ParseExactYearFormat ()
		{
			CultureInfo fp = CultureInfo.InvariantCulture;
			DateTimeOffset[] dtos = {
				new DateTimeOffset (2008, 01, 14, 13, 21, 0, new TimeSpan (1,0,0)),
				new DateTimeOffset (2008, 01, 14, 13, 21, 0, new TimeSpan (-5,0,0)),
				new DateTimeOffset (1978, 02, 16, 13, 21, 0, new TimeSpan (2,0,0)),
				new DateTimeOffset (999, 9, 9, 9, 9, 0, new TimeSpan (3,0,0)),
			};

			string[] formats = {
				"M/d/yyy H:m zzz",
				"M/d/yyyy H:m zzz",
				"M/d/yyyyy H:m zzz",
				"M/d/yyyyyy H:m zzz",
				"M/d/yyyyyyy H:m zzz",
			};

			foreach (DateTimeOffset dto in dtos)
				foreach (string format in formats)
				{
					string serialized = dto.ToString (format, fp);
					//Console.WriteLine ("{0} {1} {2}", format, dto, serialized);
					Assert.AreEqual (dto, DateTimeOffset.ParseExact (serialized, format, fp), format);
				}
			
		}

		[Test]
		[ExpectedException (typeof (FormatException))]
		public void ParseExactYearException ()
		{
			CultureInfo fp = CultureInfo.InvariantCulture;
			string format = "M/d/yyyy H:m zzz";
			string date = "1/15/999 10:58 +01:00";
			DateTimeOffset.ParseExact(date, format, fp);	
		}

		[Test]
		[ExpectedException (typeof (FormatException))]
		public void ParseExactFormatException1 ()
		{
			CultureInfo fp = CultureInfo.InvariantCulture;
			string format = "d";
			string date = "1/14/2008";
			DateTimeOffset.ParseExact(date, format, fp);
		}

		[Test]
		[ExpectedException (typeof (FormatException))]
		public void ParseExactFormatException2 ()
		{
			CultureInfo fp = CultureInfo.InvariantCulture;
			string format = "ddd dd MMM yyyy h:mm tt zzz";
			string date = "Mon 14 Jan 2008 2:56 PM +01";
			DateTimeOffset.ParseExact(date, format, fp);
		}

		[Test]
		[ExpectedException (typeof (FormatException))]
		public void ParseExactFormatException3 ()
		{
			CultureInfo fp = CultureInfo.InvariantCulture;
			string format = "ddd dd MMM yyyy h:mm tt zzz";
			string date = "Mon 14 Jan 2008 2:56 PM +01:1";
			DateTimeOffset.ParseExact(date, format, fp);
		}

		[Test]
		[ExpectedException (typeof (FormatException))]
		public void ParseExactFormatException4 ()
		{
			CultureInfo fp = CultureInfo.InvariantCulture;
			string format = "ddd dd MMM yyyy h:mm tt zzz";
			string date = "Mon 14 Jan 2008 2:56 PM +01: 00";
			DateTimeOffset.ParseExact(date, format, fp);
		}

		[Test]
		public void ParseExactWithSeconds ()
		{
			CultureInfo fp = CultureInfo.InvariantCulture;
			DateTimeOffset[] dtos = {
				new DateTimeOffset (2008, 01, 14, 13, 21, 25, new TimeSpan (1,0,0)),
				new DateTimeOffset (2008, 01, 14, 13, 21, 5, new TimeSpan (-5,0,0)),
			};

			string[] formats = {
				"dddd, dd MMMM yyyy HH:mm:ss zzz",
			};

			foreach (DateTimeOffset dto in dtos)
				foreach (string format in formats)
				{
					string serialized = dto.ToString (format, fp);
					//Console.WriteLine ("{0} {1} {2}", format, dto, serialized);
					Assert.AreEqual (dto, DateTimeOffset.ParseExact (serialized, format, fp), format);
				}
		}

		[Test]
		public void ParseExactWithFractions ()
		{
			CultureInfo fp = CultureInfo.InvariantCulture;
			DateTimeOffset[] dtos = {
				new DateTimeOffset (2008, 1, 15, 14, 36, 44, 900, TimeSpan.Zero),
			};

			string[] formats = {
				"M/d/yyyy H:m:ss,f zzz",
				"M/d/yyyy H:m:ss,ff zzz",
				"M/d/yyyy H:m:ss,fff zzz",
				"M/d/yyyy H:m:ss,ffff zzz",
				"M/d/yyyy H:m:ss,fffff zzz",
				"M/d/yyyy H:m:ss,ffffff zzz",
				"M/d/yyyy H:m:ss,fffffff zzz",
				"M/d/yyyy H:m:ss,F zzz",
				"M/d/yyyy H:m:ss,FF zzz",
				"M/d/yyyy H:m:ss,FFF zzz",
				"M/d/yyyy H:m:ss,FFFF zzz",
				"M/d/yyyy H:m:ss,FFFFF zzz",
				"M/d/yyyy H:m:ss,FFFFFF zzz",
				"M/d/yyyy H:m:ss,FFFFFFF zzz",
			};

			foreach (DateTimeOffset dto in dtos)
				foreach (string format in formats)
				{
					string serialized = dto.ToString (format, fp);
					//Console.WriteLine ("{0} {1} {2}", format, dto, serialized);
					Assert.AreEqual (dto, DateTimeOffset.ParseExact (serialized, format, fp), format);
				}
		}

		[Test]
		public void EqualsObject ()
		{
			DateTimeOffset offset1 = new DateTimeOffset ();
			Assert.IsFalse (offset1.Equals (null), "null"); // found using Gendarme :)
			Assert.IsTrue (offset1.Equals (offset1), "self");
			DateTimeOffset offset2 = new DateTimeOffset (DateTime.Today);
			Assert.IsFalse (offset1.Equals (offset2), "1!=2");
			Assert.IsFalse (offset2.Equals (offset1), "2!=1");
		}

		[Test]
		public void Serialization()
		{
			global::System.IO.MemoryStream dst = new global::System.IO.MemoryStream ();
			global::System.Runtime.Serialization.IFormatter fmtr
				= new global::System.Runtime.Serialization.Formatters.Binary.BinaryFormatter ();
			for (int i = 0; i < SerializationCases.Length; ++i) {
				dst.SetLength (0);
				DateTimeOffset cur = SerializationCases[i].Input;
				fmtr.Serialize (dst, cur);
				String result = BitConverter.ToString (dst.GetBuffer (), 0, (int)dst.Length);
				Assert.AreEqual (SerializationCases[i].ResultBinaryString, result, "resultToString #" + i);
			}//for
		}

		[Test]
		public void Deserialization()
		{
			global::System.Runtime.Serialization.IFormatter fmtr
				= new global::System.Runtime.Serialization.Formatters.Binary.BinaryFormatter ();
			global::System.IO.MemoryStream src;
			for (int i = 0; i < SerializationCases.Length; ++i) {
				src = new global::System.IO.MemoryStream (
					BitConverter_ByteArray_FromString (SerializationCases[i].ResultBinaryString));
				DateTimeOffset result = (DateTimeOffset)fmtr.Deserialize (src);
				Assert.AreEqual (SerializationCases[i].Input, result, "equals #" + i);
				Assert.AreEqual (SerializationCases[i].Input.Offset, result.Offset, ".Offset #" + i);
				Assert.AreEqual (SerializationCases[i].Input.DateTime, result.DateTime, ".DateTime #" + i);
				// DateTimeOffset always stores as Kind==Unspecified
				Assert.AreEqual (DateTimeKind.Unspecified, SerializationCases[i].Input.DateTime.Kind, ".DateTime.Kind==unspec #" + i);
				Assert.AreEqual (SerializationCases[i].Input.DateTime.Kind, result.DateTime.Kind, ".DateTime.Kind #" + i);
			}//for
		}

		public class TestRow
		{
			public DateTimeOffset Input;
			public String ResultBinaryString;

			public TestRow(DateTimeOffset input, String resultBinaryString)
			{
				this.Input = input;
				this.ResultBinaryString = resultBinaryString;
			}
		}
		readonly TestRow[] SerializationCases = {
			// Multiple tests of Unspecified and different timezone offsets; one 
			// for UTC; and one for Local which is disabled as it suits only a machine
			// in GMT or similar.
			new TestRow (new DateTimeOffset (new DateTime (2007, 01, 02, 12, 30, 50)),
				"00-01-00-00-00-FF-FF-FF-FF-01-00-00-00-00-00-00-"
				+ "00-04-01-00-00-00-15-53-79-73-74-65-6D-2E-44-61-"
				+ "74-65-54-69-6D-65-4F-66-66-73-65-74-02-00-00-00-"
				+ "08-44-61-74-65-54-69-6D-65-0D-4F-66-66-73-65-74-"
				+ "4D-69-6E-75-74-65-73-00-00-0D-07-00-39-75-F8-80-"
				+ "FC-C8-08-00-00-0B"),
			new TestRow (new DateTimeOffset (new DateTime (2007, 01, 02, 12, 30, 50), new TimeSpan (0, 0, 0)),
				"00-01-00-00-00-FF-FF-FF-FF-01-00-00-00-00-00-00-"
				+ "00-04-01-00-00-00-15-53-79-73-74-65-6D-2E-44-61-"
				+ "74-65-54-69-6D-65-4F-66-66-73-65-74-02-00-00-00-"
				+ "08-44-61-74-65-54-69-6D-65-0D-4F-66-66-73-65-74-"
				+ "4D-69-6E-75-74-65-73-00-00-0D-07-00-39-75-F8-80-"
				+ "FC-C8-08-00-00-0B"),
			new TestRow (new DateTimeOffset (new DateTime (2007, 01, 02, 12, 30, 50), new TimeSpan (1, 0, 0)),
				"00-01-00-00-00-FF-FF-FF-FF-01-00-00-00-00-00-00-"
				+ "00-04-01-00-00-00-15-53-79-73-74-65-6D-2E-44-61-"
				+ "74-65-54-69-6D-65-4F-66-66-73-65-74-02-00-00-00-"
				+ "08-44-61-74-65-54-69-6D-65-0D-4F-66-66-73-65-74-"
				+ "4D-69-6E-75-74-65-73-00-00-0D-07-00-D1-B0-96-78-"
				+ "FC-C8-08-3C-00-0B"),
			new TestRow (new DateTimeOffset (new DateTime (2007, 01, 02, 12, 30, 50), new TimeSpan (0, 30, 0)),
				"00-01-00-00-00-FF-FF-FF-FF-01-00-00-00-00-00-00-"
				+ "00-04-01-00-00-00-15-53-79-73-74-65-6D-2E-44-61-"
				+ "74-65-54-69-6D-65-4F-66-66-73-65-74-02-00-00-00-"
				+ "08-44-61-74-65-54-69-6D-65-0D-4F-66-66-73-65-74-"
				+ "4D-69-6E-75-74-65-73-00-00-0D-07-00-05-93-C7-7C-"
				+ "FC-C8-08-1E-00-0B"),
			new TestRow (new DateTimeOffset (new DateTime (2007, 01, 02, 12, 30, 50), new TimeSpan (-5, 0, 0)),
				"00-01-00-00-00-FF-FF-FF-FF-01-00-00-00-00-00-00-"
				+ "00-04-01-00-00-00-15-53-79-73-74-65-6D-2E-44-61-"
				+ "74-65-54-69-6D-65-4F-66-66-73-65-74-02-00-00-00-"
				+ "08-44-61-74-65-54-69-6D-65-0D-4F-66-66-73-65-74-"
				+ "4D-69-6E-75-74-65-73-00-00-0D-07-00-41-4B-E1-AA-"
				+ "FC-C8-08-D4-FE-0B"),
			new TestRow (new DateTimeOffset (new DateTime (2007, 01, 02, 12, 30, 50), new TimeSpan (-10, 30, 0)),
				"00-01-00-00-00-FF-FF-FF-FF-01-00-00-00-00-00-00-"
				+ "00-04-01-00-00-00-15-53-79-73-74-65-6D-2E-44-61-"
				+ "74-65-54-69-6D-65-4F-66-66-73-65-74-02-00-00-00-"
				+ "08-44-61-74-65-54-69-6D-65-0D-4F-66-66-73-65-74-"
				+ "4D-69-6E-75-74-65-73-00-00-0D-07-00-15-3F-99-D0-"
				+ "FC-C8-08-C6-FD-0B"),
			// invalid case: .ctor ArgEx "non whole minutes"
			//	new DateTimeOffset(new DateTime(2007, 01, 02, 12, 30, 50), new TimeSpan(1, 2, 3));
			//----
			new TestRow (new DateTimeOffset (new DateTime (2007, 01, 02, 12, 30, 50, DateTimeKind.Utc)),
				"00-01-00-00-00-FF-FF-FF-FF-01-00-00-00-00-00-00-"
				+ "00-04-01-00-00-00-15-53-79-73-74-65-6D-2E-44-61-"
				+ "74-65-54-69-6D-65-4F-66-66-73-65-74-02-00-00-00-"
				+ "08-44-61-74-65-54-69-6D-65-0D-4F-66-66-73-65-74-"
				+ "4D-69-6E-75-74-65-73-00-00-0D-07-00-39-75-F8-80-"
				+ "FC-C8-08-00-00-0B"),
			//----
#if ___MACHINE_TIMEZONE_HAS_ZERO_OFFSET
			// Local needs offset to be the same as the machine timezone,
			// not easy to cope with the tests being run around the world!
			// This one works in UK, etc.
			new TestRow (new DateTimeOffset (new DateTime (2007, 01, 02, 12, 30, 50, DateTimeKind.Local), new TimeSpan (0,0,0)),
				"00-01-00-00-00-FF-FF-FF-FF-01-00-00-00-00-00-00-"
				+ "00-04-01-00-00-00-15-53-79-73-74-65-6D-2E-44-61-"
				+ "74-65-54-69-6D-65-4F-66-66-73-65-74-02-00-00-00-"
				+ "08-44-61-74-65-54-69-6D-65-0D-4F-66-66-73-65-74-"
				+ "4D-69-6E-75-74-65-73-00-00-0D-07-00-39-75-F8-80-"
				+ "FC-C8-08-00-00-0B"),
#endif
		};

		static byte[] BitConverter_ByteArray_FromString(String hexString)
		{
			String[] numbers = hexString.Split('-');
			byte[] result = new byte[numbers.Length];
			for (int i = 0; i < numbers.Length; ++i) {
				byte x = Byte.Parse (numbers[i], NumberStyles.HexNumber, global::System.Globalization.CultureInfo.InvariantCulture);
				result[i] = x;
			}
			return result;
		}

	}
}
#endif

