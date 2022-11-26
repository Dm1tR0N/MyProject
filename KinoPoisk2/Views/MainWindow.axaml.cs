using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.ObjectiveC;
using Avalonia.Themes.Default;
using Avalonia.Themes.Fluent;
using Avalonia.Themes.Fluent.Controls;
using KinoPoisk2.Models;
using KinoPoisk2.ViewModels;
using Newtonsoft.Json;
using ReactiveUI;
using Tmds.DBus;
using System.Drawing;
using Microsoft.Win32;
using Image = System.Drawing.Image;

namespace KinoPoisk2.Views
{
    public partial class MainWindow : Window
    {
        public List<Result> Results = new List<Result>(8);
        public List<Multimedia> Multimedias = new List<Multimedia>();
        public List<Link> Links = new List<Link>();

        public string API_KEY = "5rBE8RxfB49Cr3r2wVbDtevufGJmYLxD"; // Является ключом доступа к API
        public static byte[] ImageToByteArrayFromFilePath(string imagefilePath) // Метод для перевода ссылки на картинку в масив байт
        {
            byte[] imageArray = File.ReadAllBytes(imagefilePath);
            return imageArray;
        }
        public static Image ByteArrayToImagebyMemoryStream(byte[] imageByte) // Метод для перевода масива байт в картинку
        {
            MemoryStream ms = new MemoryStream(imageByte);
            Image image = Image.FromStream(ms);
            return image;
        }
        public string ConvertName(string str) // Метод который удаляет все лишние символы в сторе
        {
            str = str.Replace(":", "_");
            str = str.Replace(" ", "_");
            str = str.Replace("!", "_");
            str = str.Replace(".", "_");
            str = str.Replace(",", "_");
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

                    string namePhoto = "";
                    string pathPhoto;


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

                            var str = item.multimedia.src; // Получаю путь к ссылке на картинку
                            byte[] photobytes = null; // Заранее создаю пустую переменную для кранение картинки в байт коде

                            namePhoto = ConvertName(item.TitleFilm); // Делаю название для будущей фотографии без пробелов и спец-знаков
                        
                        
                            using (WebClient client1 = new WebClient())
                            {
                                photobytes = client1.DownloadData(str);
                            }
                        
                            string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // Получаю путь к папки Мои документы
                            string path = myDocuments + "\\MyMovie"; // Добовляю название проекта
                            string subpath = $"{now.ToString("dd-MM-yyyy")}"; // Палучаю дату сегодня
                            DirectoryInfo dirInfo = new DirectoryInfo(path);
                            if (!dirInfo.Exists)
                            {
                                dirInfo.Create();
                            } // Если папка существует то не буду создавать 
                            dirInfo.CreateSubdirectory(subpath); // Иначе создам
                            
                            pathPhoto = $"{path}\\{now.ToString("dd-MM-yyyy")}\\{namePhoto}_Dm1Tr0N.jpg";
                            pathPhoto = pathPhoto.Replace(@"\", @"\\");
                            File.WriteAllBytes(pathPhoto, photobytes); // Добавляю все фотографии в папку с кэшем программы

                            Image photo;
                            photobytes = ImageToByteArrayFromFilePath($"{path}\\{now.ToString("dd-MM-yyyy")}\\{namePhoto}_Dm1Tr0N.jpg"); // Перевожу картинку в масив байт
                            photo = ByteArrayToImagebyMemoryStream(photobytes); // Перевожу масив байт в картинку
                        
                            Multimedia editMult = new Multimedia(item.multimedia.type, pathPhoto, item.multimedia.width, item.multimedia.height, photo);
                            Multimedias.Add(editMult); 
                            Links.Add(new Link(item.link.type, item.link.url, item.link.suggested_link_text));
                            Results.Add(new Result(item.TitleFilm, item.DopTitle, item.DiscriptionFilm, item.Author, item.RatingFilm, item.DatePublic, item.DateOut, item.DateUpdatePost, editMult, item.link));
                            // Заполняю калассы данными
                        }
                        else
                        {
                            Debug.Write("-> Увы, Нечего не найдено. ✘\n");
                        }
                    }
                    DataGridFilms.Items = Results; // В итоге привязываю калекцию к DataGrid
                    Debug.WriteLine($"Запрос к API выполнен успешно!");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Произошла критическая ошибка при выволнении запроса к API!!\n" +
                                $"Инофрмация об ощибке в блоке ниже:\n{e}\n////////////////////////////////////////////////////");
            }
        } // Метод созданный для обращения к API. Принимает URL запрос и запрлняет DataGrid
                                                        
        
        public MainWindow()
        {
            InitializeComponent();
            
            /*
             * На момент 25.11.2022 я застрял на проблеме того что не могу вывести картинку для
             * Фильма пробовал много способов но пока что не выходит
             *
             * Для потанциальных читателей этого кода потарался все сделать понятным
             * и закоментирал что считаю нужным
             * 
             * by Dm1Tr0N
             */
            
            string url = $"https://api.nytimes.com/svc/movies/v2/reviews/all.json?api-key={API_KEY}";
            GetApiRequest(url);

        }
        
        private void SearchBtn(object? sender, RoutedEventArgs e)
        {
            string url = $"https://api.nytimes.com/svc/movies/v2/reviews/search.json?query={SearchRequest.Text}&api-key={API_KEY}";
            GetApiRequest(url);
        }

        private void SortDate(object? sender, RoutedEventArgs e)
        {
            string url = $"https://api.nytimes.com/svc/movies/v2/reviews/all.json?opening-date={DateOut_OT.Text}:{DateOut_DO.Text}&api-key={API_KEY}";
            GetApiRequest(url);
        }

        private void Picks(object? sender, RoutedEventArgs e)
        {
            string url = $"https://api.nytimes.com/svc/movies/v2/reviews/picks.json?api-key={API_KEY}";
            GetApiRequest(url);
        }

        private void ConfigSettings(object? sender, RoutedEventArgs e)
        {
            if (DataGridFilms.IsVisible == true)
            {
                DataGridFilms.IsVisible = false;
                ConfigureSettings.IsVisible = true;
            }
            else if ( ConfigureSettings.IsVisible == true)
            {
                DataGridFilms.IsVisible = true;
                ConfigureSettings.IsVisible = false;
            }
        }

        private void AboutProgrammer(object? sender, RoutedEventArgs e)
        {
            AboutText.Text =
                "Программист: Михайлов Дмитрий Владимирович,\n" +
                "Но момент 2022 года обучаюсь в Томском Техникуме Информационных Технологий\n" +
                "Изначально, программирование было моим хобби но в будущем я хотел связать\n" +
                "это хобби с моей жизнью, В итоге выбрал специальность которая мне подъодит\n" +
                "И сейчас обучаясь на эту специальность пишу эту программу для Курсового проекта";
            ImageTeh.IsVisible = false;
        }

        private void AboutProgramm(object? sender, RoutedEventArgs e)
        {
            AboutText.Text =
                "Основная цель создания этого приложения это написание кусового проекта\n" +
                "Побочная цели проекта это:\n" +
                "   * Изучить новую IDE RIder\n" +
                "   * Научится работать с REST API\n" +
                "   * Изучить новую технологию Avolonia\n" +
                "   * Закрепить знания в C#,XAML\n" +
                "\n" +
                "Что изпользовалось в разработке:\n" +
                "   * Rider версии 2022.2.3\n" +
                "   * Avolonia версии 0.10.18\n" +
                "   * API Movie Reviews\n" +
                "   * Windows 11 PRO";
            ImageTeh.IsVisible = false;
        }

        private void Notes(object? sender, RoutedEventArgs e)
        {
            if (NotesPlase.IsVisible == false)
            {
                ConfigureSettings.IsVisible = false;
                DataGridFilms.IsVisible = false;
                NotesPlase.IsVisible = true;
            }
            else if (NotesPlase.IsVisible == true)
            {
                NotesPlase.IsVisible = false;
                DataGridFilms.IsVisible = true;
            }
        }

        private void OpenNote(object? sender, RoutedEventArgs e)
        {
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
        }
    }
}