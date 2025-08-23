using System;
using System.Runtime.CompilerServices;

namespace TaxProEWB.API
{
	public class RespGetEwayBillsByDate
	{
		public int delPinCode
		{
			get;
			set;
		}

		public string delPlace
		{
			get;
			set;
		}

		public int delStateCode
		{
			get;
			set;
		}

		public string docDate
		{
			get;
			set;
		}

		public string docNo
		{
			get;
			set;
		}

		public string ewbDate
		{
			get;
			set;
		}

		public long ewbNo
		{
			get;
			set;
		}

		public int extendedTimes
		{
			get;
			set;
		}

		public string genGstin
		{
			get;
			set;
		}

		public char rejectStatus
		{
			get;
			set;
		}

		public string status
		{
			get;
			set;
		}

		public string validUpto
		{
			get;
			set;
		}

		public RespGetEwayBillsByDate()
		{
		}
	}
}