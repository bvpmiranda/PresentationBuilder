using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PresentationBuilder.Models
{
	public enum uploadStatus
	{
		Success = 0,
		InvalidFileType = 1,
		Error = 2
	}

	public class UploadReturn
	{
		public object data { get; set; }
		public string message { get; set; }
		public string status { get; set; }
		public PresentationBuilder.Models.uploadStatus uploadStatus { get; set; }

		public UploadReturn()
		{
			status = "OK";
		}

	}
}