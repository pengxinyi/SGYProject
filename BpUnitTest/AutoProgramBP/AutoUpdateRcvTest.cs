﻿

namespace UFIDA.U9.LH.LHPubBP.AutoProgramBP
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.IO;
	using NUnit.Framework;
	
	/// <summary>
	/// Business operation test
	/// </summary> 
	[TestFixture]		
	public class AutoUpdateRcvTest
	{
		private Proxy.AutoUpdateRcvProxy obj = new Proxy.AutoUpdateRcvProxy();

		public AutoUpdateRcvTest()
		{
		}
		#region AutoTestCode ...
		[Test]
		public void TestDo()
		{
			obj.Do() ;  
		
		}
		#endregion 				
	}
	
}