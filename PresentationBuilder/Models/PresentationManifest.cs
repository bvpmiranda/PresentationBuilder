using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PresentationBuilder.Models
{
	public class PresentationManifest
	{
		public string author { get; set; }
		public DateTime date { get; set; }
		public string name { get; set; }
		public string description { get; set; }
		public List<Page> pages { get; set; }


		public class Page
		{
			public byte order { get; set; }
			public string image { get; set; }
			public string sound { get; set; }
			public int? soundLength { get; set; }
		}

	}
}