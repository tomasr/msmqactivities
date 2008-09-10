
//
// Customer.cs
//
// Author: 
//    Tomas Restrepo (tomasr@mvps.org)
//

using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp
{
   [Serializable]
	public class Customer
	{
      private int _id;
      private string _name;

      public int Id
      {
         get { return _id; }
         set { _id = value; }
      }

      public string Name
      {
         get { return _name; }
         set { _name = value; }
      }
	} // class Customer

} // namespace TestApp
