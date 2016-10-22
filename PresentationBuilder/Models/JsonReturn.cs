using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PresentationBuilder.Models
{
	public class JsonReturn
	{
		public object data { get; set; }
		public bool isValid { get; set; }
		public List<string> messages { get; set; }
		public List<JsonError> errors { get; set; }

		public JsonReturn()
		{
			this.isValid = true;
			this.messages = new List<string>();
			this.errors = new List<JsonError>();
		}
		public JsonReturn(bool isValid)
		{
			this.isValid = isValid;
		}
		public JsonReturn(bool isValid, object data)
		{
			this.isValid = isValid;
			this.data = data;
		}
		public JsonReturn(object data)
		{
			this.data = data;
		}

		public void fillErrors(System.Web.Http.ModelBinding.ModelStateDictionary ModelState)
		{
			if (!ModelState.IsValid)
			{
				foreach (var k in ModelState.Keys)
				{
					var a = 0;
					//foreach (var e in ModelState.Items[k].Errors)
					//{

					//}
				}
			}
		}

	}
}