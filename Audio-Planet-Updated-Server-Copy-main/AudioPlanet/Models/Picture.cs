using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace AudioPlanet.Models
{
    public class Picture
    {
        public int ID { get; set; }

        //name of the file
        public string Name { get; set; }

        public string AltText { get; set; }

        //Gets the height, in pixels, of this Image.
        public int Height { get; set; }

        //Gets the width, in pixels, of this Image.
        public int Width { get; set; }

        //Gets the size, in bytes, of the current file.
        public string Length { get; set; }

        public string Media { get; set; }

        //Gets the string representing the extension part of the file. 
        public string Extension { get; set; }

        public string Url { get; set; }

        internal List<Picture> GetFiles(string mediaType)
        {
            var db = new Audio();

            var folder = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/Content/Uploads"));
            FileInfo[] files = folder.GetFiles("*.*", SearchOption.AllDirectories);

            var pictures = new List<Picture>();
            int i = 0;
            foreach (FileInfo pic in files)
            {
                var absPath = pic.FullName.Substring(pic.FullName.IndexOf("\\Content", StringComparison.Ordinal) + 1);

                var newFileName = "";
                if (absPath.Contains("\\Product\\"))
                {
                    string[] strs = absPath.Split('\\');
                    var pId = Convert.ToInt16(strs[3]);
                    var product = db.Products.FirstOrDefault(p => p.ID == pId);
                    if (product != null)
                    {
                        newFileName = product.Name;
                    }
                }


                switch (mediaType)
                {
                    //default:
                    case "image":
                        if (pic.Extension == ".jpg" || pic.Extension == ".png" || pic.Extension == ".gif")
                        {
                            using (Image img = Image.FromFile(pic.FullName))
                            {
                                pictures.Add(new Picture
                                    {
                                        ID = i + 1,
                                        Name = string.IsNullOrEmpty(newFileName) ? pic.Name : newFileName,
                                        AltText = pic.Name,
                                        Height = img.Height,
                                        Width = img.Width,
                                        Length = GetSize(pic.Length),
                                        Media = mediaType,
                                        Extension = pic.Extension,
                                        Url = ("\\" + absPath).Replace("\\", "/")
                                    });
                            }
                        }
                        i = i + 1;
                        break;
                    case "media":
                        if (pic.Extension == ".swf" || pic.Extension == ".ram" || pic.Extension == ".avi" ||
                            pic.Extension == ".flv" || pic.Extension == ".mov")
                        {
                            pictures.Add(new Picture
                                {
                                    ID = i + 1,
                                    Name = string.IsNullOrEmpty(newFileName) ? pic.Name : newFileName,
                                    AltText = pic.Name,
                                    Height = 0,
                                    Width = 0,
                                    Length = GetSize(pic.Length),
                                    Media = mediaType,
                                    Extension = pic.Extension,
                                    Url = ("\\" + absPath).Replace("\\", "/")
                                });
                        }
                        i = i + 1;
                        break;
                    case "file":
                        if (pic.Extension == ".txt" || pic.Extension == ".doc" || pic.Extension == ".docx" ||
                            pic.Extension == ".xls" || pic.Extension == ".xlsx" || pic.Extension == ".pdf" ||
                            pic.Extension == ".ppt" || pic.Extension == ".pptx" || pic.Extension == ".odp" ||
                            pic.Extension == ".ods" || pic.Extension == ".odt")
                        {
                            pictures.Add(new Picture
                                {
                                    ID = i + 1,
                                    Name = string.IsNullOrEmpty(newFileName) ? pic.Name : newFileName,
                                    AltText = pic.Name,
                                    Height = 0,
                                    Width = 0,
                                    Length = GetSize(pic.Length),
                                    Media = mediaType,
                                    Extension = pic.Extension,
                                    Url = ("\\" + absPath).Replace("\\", "/")
                                });
                        }
                        i = i + 1;
                        break;
                    default:
                        if (pic.Extension == ".jpg" || pic.Extension == ".png" || pic.Extension == ".gif")
                        {
                            using (Image img = Image.FromFile(pic.FullName))
                            {
                                pictures.Add(new Picture
                                {
                                    ID = i + 1,
                                    Name = string.IsNullOrEmpty(newFileName) ? pic.Name : newFileName,
                                    AltText = pic.Name,
                                    Height = img.Height,
                                    Width = img.Width,
                                    Length = GetSize(pic.Length),
                                    Media = mediaType,
                                    Extension = pic.Extension,
                                    Url = ("\\" + absPath).Replace("\\", "/")
                                });
                            }
                        }
                        else if (pic.Extension == ".txt" || pic.Extension == ".doc" || pic.Extension == ".docx" ||
                            pic.Extension == ".xls" || pic.Extension == ".xlsx" || pic.Extension == ".pdf" ||
                            pic.Extension == ".ppt" || pic.Extension == ".pptx" || pic.Extension == ".odp" ||
                            pic.Extension == ".ods" || pic.Extension == ".odt" || pic.Extension == ".swf" ||
                            pic.Extension == ".ram" || pic.Extension == ".avi" || pic.Extension == ".flv" ||
                            pic.Extension == ".mov")
                        {
                            pictures.Add(new Picture
                                {
                                    ID = i + 1,
                                    Name = string.IsNullOrEmpty(newFileName) ? pic.Name : newFileName,
                                    AltText = pic.Name,
                                    Height = 0,
                                    Width = 0,
                                    Length = GetSize(pic.Length),
                                    Media = mediaType,
                                    Extension = pic.Extension,
                                    Url = ("\\" + absPath).Replace("\\", "/")
                                });
                        }
                        i = i + 1;
                        break;
                }
            }

            return pictures;
        }

        private string GetSize(long byteCount)
        {
            string size = "0 Bytes";
            if (byteCount >= 1073741824.0)
                size = String.Format("{0:##.##}", byteCount / 1073741824.0) + " GB";
            else if (byteCount >= 1048576.0)
                size = String.Format("{0:##.##}", byteCount / 1048576.0) + " MB";
            else if (byteCount >= 1024.0)
                size = String.Format("{0:##.##}", byteCount / 1024.0) + " KB";
            else if (byteCount > 0 && byteCount < 1024.0)
                size = byteCount + " Bytes";

            return size;
        }
    }
}