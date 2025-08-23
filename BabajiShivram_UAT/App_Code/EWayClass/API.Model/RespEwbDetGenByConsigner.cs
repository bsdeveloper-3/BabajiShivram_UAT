using System;
using System.Runtime.CompilerServices;

namespace TaxProEWB.API
{
	public class RespEwbDetGenByConsigner
	{
		public string alert
		{
			get;
			set;
		}

		public string ewayBillDate
		{
			get;
			set;
		}

		public long ewayBillNo
		{
			get;
			set;
		}

		public string validUpto
		{
			get;
			set;
		}

		public RespEwbDetGenByConsigner()
		{
		}
	}
}