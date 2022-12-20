using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using KinoPoisk2.Models;
using Newtonsoft.Json;
using Avalonia.Input;
using Microsoft.Data.Sqlite;

namespace KinoPoisk2.Views
{
    public partial class MainWindow : Window
    {
        public List<Result> Results = new List<Result>(8);
        public List<Multimedia> Multimedias = new List<Multimedia>();
        public List<Link> Links = new List<Link>();
        public List<ResultCritic> Critics = new List<ResultCritic>();

        public string API_KEY = "5rBE8RxfB49Cr3r2wVbDtevufGJmYLxD"; // Является ключом доступа к API
        public string connectionString = @"D:\КУРСАЧ\KinoPoisk2New\KinoPoisk2\Models\Chinook.db"; // Временно
        
        public string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MyMovie"; // Получаю путь к папки Мои документы
        public string DeleteBr(string str) // Метод который удаляет все лишние символы в сторе
        {
            str = str.Replace("<br/><br/>", " ");
            return str;
        }

        public void GetApiRequest(string urlPath)
        {
            try
            {
                DataGridFilms.Items = null; // Производится очистка DataGrid на тот случай если там что то есть.
                Results.Clear(); // Чищу класс Результатов на тот случай если оно заполнен
                using (var client = new HttpClient())
                {
                    var endpoint = new Uri(urlPath);
                    var result = client.GetAsync(endpoint).Result;
                    var json = result.Content.ReadAsStringAsync().Result;
                    // Тут я запрос выполил и получил JSON 
                    
                    Rootobject? result1 = JsonConvert.DeserializeObject<Rootobject>(json); // Десериалезую JSON в классы
                    
                    var listFilms = result1.results; // Создаю лист для последущей работы с ним
                    DateTime now = DateTime.Now;
                    foreach (var item in listFilms) // Перебираю всю информацию
                    {
                        if (listFilms != null)
                        {
            
                            if (item.RatingFilm == null || item.RatingFilm == "")
                            {
                                item.RatingFilm = "Отсутствует";
                            } // Если поля пусты то заменяю их тексом 

                            if (item.DateOut == null || item.DateOut == null)
                            {
                                item.DateOut = "Отсутствует";
                            } // Если поля пусты то заменяю их тексом 


                            Results.Add(new Result(item.TitleFilm, item.DopTitle, item.DiscriptionFilm, item.Author, item.RatingFilm, item.DatePublic, item.DateOut, item.DateUpdatePost, item.multimedia, item.link));
                            // Заполняю калассы данными
                        }
                        else
                        {
                            Debug.Write("-> Увы, Нечего не найдено. ✘\n");
                            Time.Content = "Увы, Нечего не найдено. ✘";
                        }
                        Debug.Write("-> Точки условий запроса к API пройдена\n");
                    }
                    DataGridFilms.Items = Results; // В итоге привязываю калекцию к DataGrid
                    DataGridFilms.IsVisible = true;
                    Debug.WriteLine($"Запрос к API выполнен успешно!");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Произошла критическая ошибка при выволнении запроса к API!!\n" +
                                $"Инофрмация об ощибке в блоке ниже:\n{e}\n////////////////////////////////////////////////////");
            }
        } // Метод созданный для обращения к API. Принимает URL запрос и запрлняет DataGrid

        public void UpdateTime()
        {
            Time.Content = "Местное время: " + DateTime.Now.ToString("t");
        }
        
        public MainWindow()
        {
            InitializeComponent();
            string url = $"https://api.nytimes.com/svc/movies/v2/reviews/all.json?api-key={API_KEY}";
            GetApiRequest(url);
            UpdateTime();
            
            string text = "";
            string pathTxt = "";

            try
            {
                DateTime now = DateTime.Now;
                string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // Получаю путь к папки Мои документы
                string path = myDocuments + "\\MyMovie"; // Добовляю название проекта

                pathTxt = $"{path}\\Notes.txt";
            
                using (StreamReader fs = new StreamReader(pathTxt))
                {
                    while (true)
                    {
                        // Читаем строку из файла во временную переменную.
                        string temp = fs.ReadLine();

                        // Если достигнут конец файла, прерываем считывание.
                        if(temp == null) break;

                        // Пишем считанную строку в итоговую переменную.
                        text  += temp + "\n";
                    }
                }
                NotesText.Text = text;
            }
            catch (Exception exception)
            {
                NotesText.Text = "Нет сохраненной заметки!";
            }

            DataGridFilms.IsVisible = true;
        }
        
        private void SearchBtn(object? sender, RoutedEventArgs e)
        {
            Time.Content = "Выполнение...";
            string url = $"https://api.nytimes.com/svc/movies/v2/reviews/search.json?query={SearchRequest.Text}&api-key={API_KEY}";
            GetApiRequest(url);

            DataGridFilms.IsVisible = true;
            ConfigureSettings.IsVisible = false;
            NotesPlase.IsVisible = false;
            Criticks.IsVisible = false;
            AddRewiew.IsVisible = false;

            if (Results.Count() < 1 || Results.Count() == null)
            {
                Time.Content = $"Запрос на поиск фильма: '{SearchRequest.Text}' Увы, Нечего не дал.";
            }
            else if (Results.Count() >= 1 && Results != null)
            {
                Time.Content = $"Найдено";
            }
            
            
        }

        private void SortDate(object? sender, RoutedEventArgs e)
        {
            Time.Content = "Выполнение...";
            string url = $"https://api.nytimes.com/svc/movies/v2/reviews/all.json?opening-date={DateOut_OT.Text}:{DateOut_DO.Text}&api-key={API_KEY}";
            GetApiRequest(url);

            DataGridFilms.IsVisible = true;
            ConfigureSettings.IsVisible = false;
            NotesPlase.IsVisible = false;
            Criticks.IsVisible = false;
            AddRewiew.IsVisible = false;
            
            if (Results.Count() < 1 || Results.Count() == null)
            {
                Time.Content = $"Запрос на сортировку дат: '{DateOut_OT.Text} - {DateOut_DO.Text}' Увы, Нечего не дал. Проверьте формат даты";
            }
            else if (Results.Count() >= 1 && Results != null)
            {
                Time.Content = $"Найдено";
            }
            else
            {
                Time.Content =
                    "Диапазон дат должен содержать начальную и конечную даты, разделенные двоеточием (например, 1980-01-01:1990-01-01)";
            }
        }

        private void Picks(object? sender, RoutedEventArgs e)
        {
            Time.Content = "Выполнение...";
            string url = $"https://api.nytimes.com/svc/movies/v2/reviews/picks.json?api-key={API_KEY}";
            GetApiRequest(url);
            DataGridFilms.IsVisible = true;
            ConfigureSettings.IsVisible = false;
            NotesPlase.IsVisible = false;
            Criticks.IsVisible = false;
            AddRewiew.IsVisible = false;
            
            if (Results.Count() < 1 || Results.Count() == null)
            {
                Time.Content = $"Запрос на подборку от критиков: Увы, Нечего не дал.";
            }
            else if (Results.Count() >= 1 && Results != null)
            {
                Time.Content = $"Найдено";
            }
        }

        private void ConfigSettings(object? sender, RoutedEventArgs e)
        {

            DataGridFilms.IsVisible = false;
            NotesPlase.IsVisible = false;
            ConfigureSettings.IsVisible = true;
            Criticks.IsVisible = false;
            AddRewiew.IsVisible = false;
            
            UpdateTime();
        }

        private void AboutProgrammer(object? sender, RoutedEventArgs e)
        {
            AboutText.Text =
                "Разрабочик: Михайлов Дмитрий Владимирович,\n" +
                "Студент Томского Техникума Информационных Технологий, 606 группа\n";
            ImageTeh.IsVisible = false;
            UpdateTime();
        }

        private void AboutProgramm(object? sender, RoutedEventArgs e)
        {
            AboutText.Text =
                "Основная цель - создание приложения для кусового проекта\n" +
                "Побочная цели проекта это:\n" +
                "   * Изучить новую IDE RIder\n" +
                "   * Изучить новую технологию Avolonia\n" +
                "   * Закрепить знания в C#,XAML\n" +
                "\n" +
                "Что изпользовалось в разработке:\n" +
                "   * Rider версии 2022.2.3\n" +
                "   * Avolonia версии 0.10.18\n" +
                "   * API Movie Reviews\n" +
                "   * Windows 11 PRO";
            ImageTeh.IsVisible = false;
            UpdateTime();
        }

        private void Notes(object? sender, RoutedEventArgs e)
        {
            ConfigureSettings.IsVisible = false;
            DataGridFilms.IsVisible = false;
            NotesPlase.IsVisible = true;
            Criticks.IsVisible = false;
            AddRewiew.IsVisible = false;
            
            UpdateTime();
        }

        private void SaveNote(object? sender, RoutedEventArgs e)
        {
            string pathTxt = "";

            try
            {
                DateTime now1 = DateTime.Now;
                string myDocuments1 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // Получаю путь к папки Мои документы
                string path1 = myDocuments1 + "\\MyMovie"; // Добовляю название проекта

                pathTxt = $"{path1}\\Notes.txt";
            
                string path = pathTxt;
                string multiLineText = NotesText.Text;
 
                File.WriteAllLines(path, multiLineText.Split(Environment.NewLine));
            }
            catch (Exception exception)
            {
                NotesText.Text = "Нечего сохранять!";
            }
            UpdateTime();
        }

        private void Button_OnClick(object? sender, RoutedEventArgs e)
        {
            string LinkFilm_Unique = ((Result)DataGridFilms.SelectedItem).link.url;

            var SelectedFilm = Results.SingleOrDefault(x => x.link.url == LinkFilm_Unique);
            
            string sqlExpression = "SELECT * FROM Favorites";
            using (var connection = new SqliteConnection("Data Source=" + connectionString))
            {
                connection.Open();

                bool CheckLink = false;
                
                SqliteCommand command1 = new SqliteCommand(sqlExpression, connection);
                using (SqliteDataReader reader = command1.ExecuteReader())
                {
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read())   // построчно считываем данные
                        {
                            var Link = Convert.ToString(reader.GetValue(0));
                            string TitleFilm = Convert.ToString(reader.GetValue(1));
                            var RatingFilm = Convert.ToString(reader.GetValue(2));
                            var Pick = Convert.ToString(reader.GetValue(3));
                            var Author = Convert.ToString(reader.GetValue(4));
                            var DopTitle = Convert.ToString(reader.GetValue(5));
                            var DiscriptionFilm = Convert.ToString(reader.GetValue(6));
                            var DatePublic = Convert.ToString(reader.GetValue(7));
                            var DateOut = Convert.ToString(reader.GetValue(8));
                            var DateUpdatePost = Convert.ToString(reader.GetValue(9));
                            
                            Debug.WriteLine($"{Link} \t {TitleFilm} \t {RatingFilm} \t {Pick} \t {Author} \t {DopTitle} \t {DiscriptionFilm} \t {DatePublic} \t {DateOut} \t {DateUpdatePost}");
                        }
                    }
                }
                
                try
                {
                    SqliteCommand command = new SqliteCommand();
                    command.Connection = connection;
                    command.CommandText = $"INSERT INTO Favorites (Link, TitleFilm, RatingFilm, Pick, Author, DopTitle, DiscriptionFilm, DatePublic, DateOut, DateUpdatePost) " +
                                          $"VALUES ('{SelectedFilm.link.url}', '{SelectedFilm.TitleFilm}', '{SelectedFilm.RatingFilm}', '{SelectedFilm.Pick}', '{SelectedFilm.Author}', '{SelectedFilm.DopTitle}', '{SelectedFilm.DiscriptionFilm}', '{SelectedFilm.DatePublic}', '{SelectedFilm.DateOut}', '{SelectedFilm.DateUpdatePost}')";
                    Time.Content = "Добавлен! " + SelectedFilm.TitleFilm;
                    int number = command.ExecuteNonQuery();
                }
                catch (Exception exception)
                {
                    Time.Content = SelectedFilm.TitleFilm + " Уже есть!";
                }

            }
        }

        public void UpdateFavorites()
        {
            string sqlExpression = "SELECT * FROM Favorites";
            using (var connection = new SqliteConnection("Data Source=" + connectionString))
            {
                connection.Open();

                SqliteCommand command1 = new SqliteCommand(sqlExpression, connection);
                using (SqliteDataReader reader = command1.ExecuteReader())
                {
                    string LinkFilm_Unique;
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read()) // построчно считываем данные
                        {
                            var Link = Convert.ToString(reader.GetValue(0));
                            string TitleFilm = Convert.ToString(reader.GetValue(1));
                            var RatingFilm = Convert.ToString(reader.GetValue(2));
                            var Pick = Convert.ToString(reader.GetValue(3));
                            var Author = Convert.ToString(reader.GetValue(4));
                            var DopTitle = Convert.ToString(reader.GetValue(5));
                            var DiscriptionFilm = Convert.ToString(reader.GetValue(6));
                            var DatePublic = Convert.ToString(reader.GetValue(7));
                            var DateOut = Convert.ToString(reader.GetValue(8));
                            var DateUpdatePost = Convert.ToString(reader.GetValue(9));

                            Debug.WriteLine(
                                $"{Link} \t {TitleFilm} \t {RatingFilm} \t {Pick} \t {Author} \t {DopTitle} \t {DiscriptionFilm} \t {DatePublic} \t {DateOut} \t {DateUpdatePost}");
                            var editLink = new Link(null, Link, null);
                            Results.Add(new Result(TitleFilm, DopTitle, DiscriptionFilm, Author, RatingFilm, DatePublic,
                                DateOut, DateUpdatePost, null, editLink));
                        }
                    }
                }
            }
        }

        private void Favorites(object? sender, RoutedEventArgs e)
        {
            DataGridFilms.Items = null; // Производится очистка DataGrid на тот случай если там что то есть.
            Results.Clear(); // Чищу класс Результатов на тот случай если оно заполнен

            UpdateFavorites();
            Time.Content = "Список избранных фильмов";
            
            DataGridFilms.Items = Results; // В итоге привязываю калекцию к DataGrid
            
            DataGridFilms.IsVisible = true;
            ConfigureSettings.IsVisible = false;
            NotesPlase.IsVisible = false;
            Criticks.IsVisible = false;
            AddRewiew.IsVisible = false;
        }

        private void DeleteFromfavorite(object? sender, RoutedEventArgs e)
        {
            try
            {
                string LinkFilm_Unique = ((Result)DataGridFilms.SelectedItem).link.url;
                string sqlExpression = $"DELETE  FROM Favorites WHERE Link='{LinkFilm_Unique}'";
                using (var connection = new SqliteConnection($"Data Source={connectionString}"))
                {
                    connection.Open();
 
                    SqliteCommand command = new SqliteCommand(sqlExpression, connection);
 
                    int number = command.ExecuteNonQuery();
 
                    Debug.WriteLine($"Удалено объектов: {number}");
                    Time.Content = $"Удалено объектов: {number}";
                    if (number == 0)
                    {
                        Time.Content += " т.к нет Такого!";
                    }
                }
            
                DataGridFilms.Items = null; // Производится очистка DataGrid на тот случай если там что то есть.
                Results.Clear(); // Чищу класс Результатов на тот случай если оно заполнен
                UpdateFavorites();
                DataGridFilms.Items = Results;
            }
            catch (Exception)
            {
                Time.Content = $"У вас нет такого!";
            }
        }

        private void SearchCritick(object? sender, RoutedEventArgs e)
        {
            DataGridFilms.IsVisible = false;
            NotesPlase.IsVisible = false;
            ConfigureSettings.IsVisible = false;
            Criticks.IsVisible = true;
            AddRewiew.IsVisible = false;
        }

        private void SearchCriticInList(object? sender, RoutedEventArgs e)
        {
            string pathSearch = $"https://api.nytimes.com/svc/movies/v2/critics/{PlaceWithName.Text}.json?api-key={API_KEY}";
            try
            {
                ListCritics.Items = null;
                Critics.Clear(); // Чищу класс Результатов на тот случай если оно заполнен
                using (var client = new HttpClient())
                {
                    var endpoint = new Uri(pathSearch);
                    var result = client.GetAsync(endpoint).Result;
                    var json = result.Content.ReadAsStringAsync().Result;
                    // Тут я запрос выполил и получил JSON 
                    
                    RootobjectCritic? result1 = JsonConvert.DeserializeObject<RootobjectCritic>(json); // Десериалезую JSON в классы

                    string namePhoto = "";
                    string pathPhoto;


                    var listFilms = result1.results; // Создаю лист для последущей работы с ним
                    DateTime now = DateTime.Now;
                    foreach (var item in listFilms) // Перебираю всю информацию
                    {
                        if (listFilms != null)
                        {
                            Critics.Add(new ResultCritic(item.display_name, item.sort_name, item.status, DeleteBr(item.bio)));
                            // Заполняю калассы данными
                        }
                        else
                        {
                            Debug.Write("-> Увы, Нечего не найдено. ✘\n");
                        }
                    }

                    ListCritics.Items = Critics;
                    Time.Content = $"Информации о критеке {PlaceWithName.Text}";
                    Debug.WriteLine($"Запрос к API выполнен успешно!");
                }
            }
            catch (Exception e1)
            {
                Time.Content = "Информации о критеке нет!";
                Debug.WriteLine($"Произошла критическая ошибка при выволнении запроса к API!!\n" +
                                $"Инофрмация об ощибке в блоке ниже:\n{e1}\n////////////////////////////////////////////////////");
            }
        }

        private void OpenMenuOne(object? sender, RoutedEventArgs e)
        {
            MenuItem_One.Open();
        }
        
        public void UpdateAlreadyWatched()
        {
            string sqlExpression = "SELECT * FROM AlreadyWatche";
            using (var connection = new SqliteConnection("Data Source=" + connectionString))
            {
                connection.Open();

                SqliteCommand command1 = new SqliteCommand(sqlExpression, connection);
                using (SqliteDataReader reader = command1.ExecuteReader())
                {
                    string LinkFilm_Unique;
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read()) // построчно считываем данные
                        {
                            var Link = Convert.ToString(reader.GetValue(0));
                            string TitleFilm = Convert.ToString(reader.GetValue(1));
                            var RatingFilm = Convert.ToString(reader.GetValue(2));
                            var Pick = Convert.ToString(reader.GetValue(3));
                            var Author = Convert.ToString(reader.GetValue(4));
                            var DopTitle = Convert.ToString(reader.GetValue(5));
                            var DiscriptionFilm = Convert.ToString(reader.GetValue(6));
                            var DatePublic = Convert.ToString(reader.GetValue(7));
                            var DateOut = Convert.ToString(reader.GetValue(8));
                            var DateUpdatePost = Convert.ToString(reader.GetValue(9));

                            Debug.WriteLine(
                                $"{Link} \t {TitleFilm} \t {RatingFilm} \t {Pick} \t {Author} \t {DopTitle} \t {DiscriptionFilm} \t {DatePublic} \t {DateOut} \t {DateUpdatePost}");
                            var editLink = new Link(null, Link, null);
                            Results.Add(new Result(TitleFilm, DopTitle, DiscriptionFilm, Author, RatingFilm, DatePublic,
                                DateOut, DateUpdatePost, null, editLink));
                        }
                    }
                }
            }
        }
        private void AlreadyWatched(object? sender, RoutedEventArgs e)
        {
            DataGridFilms.Items = null; // Производится очистка DataGrid на тот случай если там что то есть.
            Results.Clear(); // Чищу класс Результатов на тот случай если оно заполнен

            UpdateAlreadyWatched();
            Time.Content = "Список просмотренных фильмов.";
            
            DataGridFilms.Items = Results; // В итоге привязываю калекцию к DataGrid
            DataGridFilms.IsVisible = true;
            ConfigureSettings.IsVisible = false;
            NotesPlase.IsVisible = false;
            Criticks.IsVisible = false;
            AddRewiew.IsVisible = false;
            }
        
        private void ClearFavorites(object? sender, RoutedEventArgs e) // Чистит избранные
        {
            string sqlExpression = "DELETE  FROM Favorites";
            using (var connection = new SqliteConnection("Data Source=" + connectionString))
            {
                connection.Open();
 
                SqliteCommand command = new SqliteCommand(sqlExpression, connection);
 
                int number = command.ExecuteNonQuery();
                Time.Content = $"Избранныен очищены! Было удалено {number} фильм(а/ов).";
            }
            
            DataGridFilms.Items = null; // Производится очистка DataGrid на тот случай если там что то есть.
            Results.Clear(); // Чищу класс Результатов на тот случай если оно заполнен

            UpdateFavorites();

            DataGridFilms.Items = Results; // В итоге привязываю калекцию к DataGrid
        }

        private void AlreadyWatchedAdd(object? sender, RoutedEventArgs e) // Дабовляет уже просмотренные фильмы
        {
            string LinkFilm_Unique = ((Result)DataGridFilms.SelectedItem).link.url;

            var SelectedFilm = Results.SingleOrDefault(x => x.link.url == LinkFilm_Unique);
            
            string sqlExpression = "SELECT * FROM AlreadyWatche";
            using (var connection = new SqliteConnection("Data Source=" + connectionString))
            {
                connection.Open();

                bool CheckLink = false;

                SqliteCommand command1 = new SqliteCommand(sqlExpression, connection);
                using (SqliteDataReader reader = command1.ExecuteReader())
                {
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read()) // построчно считываем данные
                        {
                            var Link = Convert.ToString(reader.GetValue(0));
                            string TitleFilm = Convert.ToString(reader.GetValue(1));
                            var RatingFilm = Convert.ToString(reader.GetValue(2));
                            var Pick = Convert.ToString(reader.GetValue(3));
                            var Author = Convert.ToString(reader.GetValue(4));
                            var DopTitle = Convert.ToString(reader.GetValue(5));
                            var DiscriptionFilm = Convert.ToString(reader.GetValue(6));
                            var DatePublic = Convert.ToString(reader.GetValue(7));
                            var DateOut = Convert.ToString(reader.GetValue(8));
                            var DateUpdatePost = Convert.ToString(reader.GetValue(9));

                            Debug.WriteLine(
                                $"{Link} \t {TitleFilm} \t {RatingFilm} \t {Pick} \t {Author} \t {DopTitle} \t {DiscriptionFilm} \t {DatePublic} \t {DateOut} \t {DateUpdatePost}");
                        }
                    }
                }
                
                try
                {
                    SqliteCommand command = new SqliteCommand();
                    command.Connection = connection;
                    command.CommandText =
                        $"INSERT INTO AlreadyWatche (Link, TitleFilm, RatingFilm, Pick, Author, DopTitle, DiscriptionFilm, DatePublic, DateOut, DateUpdatePost) " +
                        $"VALUES ('{SelectedFilm.link.url}', '{SelectedFilm.TitleFilm}', '{SelectedFilm.RatingFilm}', '{SelectedFilm.Pick}', '{SelectedFilm.Author}', '{SelectedFilm.DopTitle}', '{SelectedFilm.DiscriptionFilm}', '{SelectedFilm.DatePublic}', '{SelectedFilm.DateOut}', '{SelectedFilm.DateUpdatePost}')";
                    Time.Content = "Добавлен! " + SelectedFilm.TitleFilm;
                    int number = command.ExecuteNonQuery();
                }
                catch (Exception exception)
                {
                    Time.Content = SelectedFilm.TitleFilm + " Уже есть!";
                }
            }
        }

        private void SearchEnter(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string url = $"https://api.nytimes.com/svc/movies/v2/reviews/search.json?query={SearchRequest.Text}&api-key={API_KEY}";
                GetApiRequest(url);

                DataGridFilms.IsVisible = true;
                ConfigureSettings.IsVisible = false;
                NotesPlase.IsVisible = false;
                Criticks.IsVisible = false;
                AddRewiew.IsVisible = false;

                if (Results.Count() < 1 || Results.Count() == null)
                {
                    Time.Content = $"Запрос на поиск фильма: '{SearchRequest.Text}' Увы, Нечего не дал.";
                }
                else if (Results.Count() >= 1 && Results != null)
                {
                    Time.Content = $"Что-то найдено";
                }
            }
        }
    }
}