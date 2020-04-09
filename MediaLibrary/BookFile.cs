using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MediaLibrary
{
    public class BookFile
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        // public property
        public string filePath { get; set; }
        public List<Book> Books { get; set; }

        // constructor is a special method that is invoked
        // when an instance of a class is created
        public BookFile(string path)
        {
            Books = new List<Book>();
            filePath = path;
            // to populate the list with data, read from the data file
            try
            {
                if (File.Exists(path))
                {
                    StreamReader sr = new StreamReader(filePath);
                    while (!sr.EndOfStream)
                    {
                        Book book = new Book();
                        string line = sr.ReadLine();
                        string[] bookDetails = line.Split(',');
                        book.mediaId = UInt64.Parse(bookDetails[0]);
                        book.title = bookDetails[1];
                        book.genres = bookDetails[2].Split('|').ToList();
                        book.author = bookDetails[3];
                        book.publisher = bookDetails[4];
                        book.pageCount = UInt64.Parse(bookDetails[5]);
                        Books.Add(book);
                    }
                    sr.Close();
                    logger.Info("Books in file {Count}", Books.Count);
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
            if (Books.ConvertAll(m => m.title.ToLower()).Contains(title.ToLower()))
            {
                logger.Info("Duplicate book title {Title}", title);
                return false;
            }
            return true;
        }

        public void AddBook(Book book)
        {
            try
            {
                if (Books.Count == 0)
                    book.mediaId = 0;
                else
                    book.mediaId = Books.Max(m => m.mediaId) + 1;
                string title = book.title.IndexOf(',') != -1 || book.title.IndexOf('"') != -1 ? $"\"{book.title}\"" : book.title;
                StreamWriter sw = new StreamWriter(filePath, true);
                sw.WriteLine($"{book.mediaId},{title},{string.Join("|", book.genres)},{book.author},{book.publisher},{book.pageCount}");
                sw.Close();
                Books.Add(book);
                // log transaction
                logger.Info("Media id {Id} added", book.mediaId);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
    }
}