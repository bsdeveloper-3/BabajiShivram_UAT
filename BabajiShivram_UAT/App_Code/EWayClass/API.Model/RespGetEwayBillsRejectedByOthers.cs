using System;
using System.Runtime.CompilerServices;

namespace TaxProEWB.API
{
	public class RespGetEwayBillsRejectedByOthers
	{
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

		public string genGstin
		{
			get;
			set;
		}

		public string rejectedDate
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

		public RespGetEwayBillsRejectedByOthers()
		{
		}
	}
}