using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MediaLibrary
{
    class AlbumFile
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        // public property
        public string filePath { get; set; }
        public List<Album> Albums { get; set; }

        // constructor is a special method that is invoked
        // when an instance of a class is created
        public AlbumFile(string path)
        {
            Albums = new List<Album>();
            filePath = path;
            // to populate the list with data, read from the data file
            try
            {
                if (File.Exists(path))
                {
                    StreamReader sr = new StreamReader(filePath);
                    while (!sr.EndOfStream)
                    {
                        Album album = new Album();
                        string line = sr.ReadLine();
                        string[] albumDetails = line.Split(',');
                        album.mediaId = UInt64.Parse(albumDetails[0]);
                        album.title = albumDetails[1];
                        album.genres = albumDetails[2].Split('|').ToList();
                        album.artist = albumDetails[3];
                        album.label = albumDetails[4];
                        Albums.Add(album);
                    }
                    sr.Close();
                    logger.Info("Albums in file {Count}", Albums.Count);
                }
                else
                {
                    logger.Info("The file does not exist {Path}", path);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        // public method
        public bool isUniqueTitle(string title)
        {
            if (Albums.ConvertAll(m => m.title.ToLower()).Contains(title.ToLower()))
            {
                logger.Info("Duplicate Album title {Title}", title);
                return false;
            }
            return true;
        }

        public void AddAlbum(Album album)
        {
            try
            {
                if (Albums.Count == 0)
                    album.mediaId = 0;
                else
                    album.mediaId = Albums.Max(m => m.mediaId) + 1;
                string title = album.title.IndexOf(',') != -1 || album.title.IndexOf('"') != -1 ? $"\"{album.title}\"" : album.title;
                StreamWriter sw = new StreamWriter(filePath, true);
                sw.WriteLine($"{album.mediaId},{title},{string.Join("|", album.genres)},{album.artist},{album.label}");
                sw.Close();
                Albums.Add(album);
                // log transaction
                logger.Info("Media id {Id} added", album.mediaId);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
    }
}
