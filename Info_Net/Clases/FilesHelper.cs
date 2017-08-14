using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Info_Net.Clases
{
	public class FilesHelper
	{
		internal object ImagenFile;

		public static string UploadPhoto(HttpPostedFileBase file, string folder)
		{
			string path = string.Empty;
			string pic = string.Empty;

			if (file != null)
			{
				pic = Path.GetFileName(file.FileName);
				path = Path.Combine(HttpContext.Current.Server.MapPath(folder), pic);
				file.SaveAs(path);
				using (MemoryStream ms = new MemoryStream())
				{
					file.InputStream.CopyTo(ms);
					byte[] array = ms.GetBuffer();
				}
			}

			return pic;
		}

		internal static string UploadPhoto(object imagenFile, string folder)
		{
			throw new NotImplementedException();
		}
	}
	}
   