using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace DropStatistics.ImageDownloader
{
    class PoeImageDownloader
    {
        private Dictionary<string, ImageCache> ImagesCache = new Dictionary<string, ImageCache>();
        private readonly string LocalPluginDirectory;

        public PoeImageDownloader(string pluginDirectory)
        {
            LocalPluginDirectory = pluginDirectory;
        }
        public string GetImage(string metadata)
        {
            ImageCache result;

            if (!ImagesCache.TryGetValue(metadata, out result))
            {
                result = DownloadImage(metadata);
                ImagesCache.Add(metadata, result);
            }
            if (result.bIsDownloaded)
                return result.FilePath;
            else
                return null;
        }
        //Images from site:
        //http://webcdn.pathofexile.com/image/Art/2DItems/Currency/CurrencyRerollRare.png
        private ImageCache DownloadImage(string metadata)
        {
            //Metadata will be always contains (ends with) ".dds" keyword. Check AddItemToCells.

            metadata = metadata.Replace(".dds", ".png");
            var url = "http://webcdn.pathofexile.com/image/" + metadata;

            var filePath = LocalPluginDirectory + "/resources/" + metadata;


            ImageCache img = new ImageCache()
            {
                FilePath = filePath,
                Url = url
            };


            try
            {
                if (File.Exists(img.FilePath))
                {
                    img.bIsDownloaded = true;
                    return img;
                }

                var settingsDirName = Path.GetDirectoryName(img.FilePath);
                if (!Directory.Exists(settingsDirName))
                    Directory.CreateDirectory(settingsDirName);

                WebClient webClient = new WebClient();
                webClient.DownloadDataAsync(new Uri(img.Url), img.FilePath);
                webClient.DownloadDataCompleted += img.OnGetDownloadedStringCompleted;
            }
            catch
            {
                MessageBox.Show("DropStatistics Error processing: Url: " + img.Url + ", Path: " + img.FilePath,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return img;
        }
        private class ImageCache
        {
            public bool bIsDownloaded;
            public string Url;
            public string FilePath;

            public void OnGetDownloadedStringCompleted(object sender, DownloadDataCompletedEventArgs e)
            {
                var contentType = ((WebClient)sender).ResponseHeaders[HttpResponseHeader.ContentType];
                if (e.Error == null && contentType == "image/png")
                {
                    Bitmap flaskImg;
                    using (var ms = new MemoryStream(e.Result))
                    {
                        flaskImg = new Bitmap(ms);
                    }

                    if (FilePath.Contains("Flasks"))//Cut 1/3 of flask image
                    {
                        flaskImg = CropImage(flaskImg, new System.Drawing.Rectangle(0, 0, flaskImg.Width / 3, flaskImg.Height));
                    }

                    flaskImg.Save(FilePath, System.Drawing.Imaging.ImageFormat.Png);

                    bIsDownloaded = true;//Due to async processing this must be in the last line
                }
                else
                {
                    MessageBox.Show("DropStatistics couldn't download images from:" + Url,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            //from http://stackoverflow.com/questions/9484935/how-to-cut-a-part-of-image-in-c-sharp
            private Bitmap CropImage(Bitmap source, System.Drawing.Rectangle section)
            {
                return source.Clone(section, source.PixelFormat);
            }
        }
    }
}
