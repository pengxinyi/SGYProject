

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
	public class AutoCreateU9NoSrcShipDocTest
	{
		private Proxy.AutoCreateU9NoSrcShipDocProxy obj = new Proxy.AutoCreateU9NoSrcShipDocProxy();

		public AutoCreateU9NoSrcShipDocTest()
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