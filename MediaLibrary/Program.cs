using System;
using NLog;
using System.Linq;

namespace MediaLibrary
{
    class MainClass
    {
        // create a class level instance of logger (can be used in methods other than Main)
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            logger.Info("Program started");
            string scrubbedFile = FileScrubber.ScrubMovies("../../../movies.csv");
            string aFile = "albums.csv";
            string bFile = "books.csv";

            MovieFile movieFile = new MovieFile(scrubbedFile);
            AlbumFile albumFile = new AlbumFile(aFile);
            BookFile bookFile = new BookFile(bFile);

            int answer;
            do
            {
                Console.WriteLine("1. Add Movie\n2. Display Movies\n3. Add Album\n" +
                    "4. Display Albums\n5. Add Book\n6. Display Books\nPress Enter to go to Media Search Application.");
                int.TryParse(Console.ReadLine(), out answer);
                switch (answer)
                {
                    case 1:
                        {
                            Movie movie = new Movie();
                            Console.WriteLine("What is the title of the new movie?");
                            string title = Console.ReadLine();
                            Console.WriteLine("What year was the movie released?");
                            int year;
                            if (!int.TryParse(Console.ReadLine(), out year))
                            {
                                Console.WriteLine("Invalid Year.");
                                logger.Warn("Invalid Year.");
                                movie.title = $"{title} ({year})";
                            }
                            else
                            {
                                movie.title = $"{title}";
                            }                            
                            if (movieFile.isUniqueTitle(movie.title))
                            {
                                string input;
                                do
                                {
                                    Console.WriteLine("What genres are the movie? (done to quit)");
                                    input = Console.ReadLine();
                                    if (input != "done" && input.Length > 0)
                                    {
                                        movie.genres.Add(input);
                                    }
                                } while (input != "done");
                                if (movie.genres.Count == 0)
                                {
                                    movie.genres.Add("(no genres listed)");
                                }
                                Console.WriteLine("Who directed the movie?");
                                input = Console.ReadLine();
                                if (input == "")
                                {
                                    movie.director = "unassigned";
                                }
                                else
                                {
                                    movie.director = input;
                                }
                                Console.WriteLine("How long was the movie? (h:m:s)");
                                input = Console.ReadLine();
                                if (input == "")
                                {
                                    movie.runningTime = new TimeSpan(0);
                                }
                                else
                                {
                                    movie.runningTime = TimeSpan.Parse(input);
                                }
                                movieFile.AddMovie(movie);
                            }
                            else
                            {
                                Console.WriteLine("Movie title already exists\n");
                            }

                        }
                        break;
                    case 2:
                        {
                            foreach (Movie m in movieFile.Movies)
                            {
                                Console.WriteLine(m.Display());
                            }
                        }
                        break;
                    case 3:
                        {
                            Album album = new Album();
                            Console.WriteLine("What is the title of the new album?");
                            string title = Console.ReadLine();
                            Console.WriteLine("What year was the album released?");
                            int year;
                            if (!int.TryParse(Console.ReadLine(), out year))
                            {
                                Console.WriteLine("Invalid Year.");
                                logger.Warn("Invalid Year.");
                                album.title = $"{title} ({year})";
                            }
                            else
                            {
                                album.title = $"{title}";
                            }
                            if (albumFile.isUniqueTitle(album.title))
                            {
                                string input;
                                do
                                {
                                    Console.WriteLine("What genres are the album? (done to quit)");
                                    input = Console.ReadLine();
                                    if (input != "done" && input.Length > 0)
                                    {
                                        album.genres.Add(input);
                                    }
                                } while (input != "done");
                                if (album.genres.Count == 0)
                                {
                                    album.genres.Add("(no genres listed)");
                                }
                                Console.WriteLine("Who was the album artist?");
                                input = Console.ReadLine();
                                if (input == "")
                                {
                                    album.artist = "unassigned";
                                }
                                else
                                {
                                    album.artist = input;
                                }
                                Console.WriteLine("What is the record label?");
                                input = Console.ReadLine();
                                if (input == "")
                                {
                                    album.label = "unassigned";
                                }
                                else
                                {
                                    album.label = input;
                                }
                                albumFile.AddAlbum(album);
                            }
                            else
                            {
                                Console.WriteLine("Album title already exists\n");
                            }

                        }
                        break;
                    case 4:
                        {
                            foreach (Album a in albumFile.Albums)
                            {
                                Console.WriteLine(a.Display());
                            }
                        }
                        break;
                    case 5:
                        {
                            Book book = new Book();
                            Console.WriteLine("What is the title of the new book?");
                            string title = Console.ReadLine();
                            Console.WriteLine("What year was the book published?");
                            int year;
                            if (!int.TryParse(Console.ReadLine(), out year))
                            {
                                Console.WriteLine("Invalid Year.");
                                logger.Warn("Invalid Year.");
                                book.title = $"{title} ({year})";
                            }
                            else
                            {
                                book.title = $"{title}";
                            }
                            if (bookFile.isUniqueTitle(book.title))
                            {
                                string input;
                                do
                                {
                                    Console.WriteLine("What genres are the book? (done to quit)");
                                    input = Console.ReadLine();
                                    if (input != "done" && input.Length > 0)
                                    {
                                        book.genres.Add(input);
                                    }
                                } while (input != "done");
                                if (book.genres.Count == 0)
                                {
                                    book.genres.Add("(no genres listed)");
                                }
                                Console.WriteLine("Who was the book author?");
                                input = Console.ReadLine();
                                if (input == "")
                                {
                                    book.author = "unassigned";
                                }
                                else
                                {
                                    book.author = input;
                                }
                                Console.WriteLine("Who is the publisher?");
                                input = Console.ReadLine();
                                if (input == "")
                                {
                                    book.publisher = "unassigned";
                                }
                                else
                                {
                                    book.publisher = input;
                                }
                                Console.WriteLine("How many pages is the book?");
                                int pages;
                                input = Console.ReadLine();
                                if (input == "")
                                {
                                    book.pageCount = 0;
                                }
                                else if (!int.TryParse(input, out pages))
                                {
                                    Console.WriteLine("Invalid Number.");
                                    logger.Warn("Invalid Number.");
                                }                            
                                else
                                {
                                    book.pageCount = UInt64.Parse(input);
                                }
                                bookFile.AddBook(book);
                            }
                            else
                            {
                                Console.WriteLine("Album title already exists\n");
                            }

                        }
                        break;
                    case 6:
                        {
                            foreach (Book b in bookFile.Books)
                            {
                                Console.WriteLine(b.Display());
                            }
                        }
                        break;
                    default:
                        break;
                }

            } while (answer >= 1 && answer <= 6);
            int answer2;
            do
            {
                Console.WriteLine("What would you like to search for?\n[1] Movie\n[2] Album\n[3] Book\nEnter to Quit.");
                int.TryParse(Console.ReadLine(), out answer2);
                string title;
                switch (answer2)
                {
                    case 1:
                        Console.WriteLine("What is the title you are searching for?");
                        title = Console.ReadLine();
                        var moviesWithTitle = movieFile.Movies.Where(m => m.title.Contains(title));
                        Console.WriteLine($"\nThere are {moviesWithTitle.Count()} movies with {title} in their title.\n");
                        foreach (Movie m in moviesWithTitle)
                        {
                            Console.WriteLine(m.Display());
                        }
                        break;
                    case 2:
                        Console.WriteLine("What is the title you are searching for?");
                        title = Console.ReadLine();
                        var albumsWithTitle = albumFile.Albums.Where(a => a.title.Contains(title));
                        Console.WriteLine($"\nThere are {albumsWithTitle.Count()} albums with {title} in their title.\n");
                        foreach (Album a in albumsWithTitle)
                        {
                            Console.WriteLine(a.Display());
                        }
                        break;
                    case 3:
                        Console.WriteLine("What is the title you are searching for?");
                        title = Console.ReadLine();
                        var booksWithTitle = bookFile.Books.Where(b => b.title.Contains(title));
                        Console.WriteLine($"\nThere are {booksWithTitle.Count()} books with {title} in their title.\n");
                        foreach (Book b in booksWithTitle)
                        {
                            Console.WriteLine(b.Display());
                        }
                        break;
                    default:
                        Console.WriteLine("Thank you for using this application.");
                        break;
                }
            } while (answer2 == 1 || answer2 == 2 || answer2 == 3);
            logger.Info("Program ended");
        }
    }
}