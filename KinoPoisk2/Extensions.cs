using KinoPoisk2.Models;

namespace KinoPoisk2;

public static class Extensions
{
    public static string ToFormatedString(this Models.Result result)
    {
        return string.Format("Название фильма: {0}\nЗаголовок: {1}\nКраткое изложение: {2}\nДата пуликации: {3}\nДата выхода: {4}\nДата обновления информации: {5}\n", result.TitleFilm, result.DopTitle, result.DiscriptionFilm, result.DatePublic, result.DateOut, result.DateUpdatePost);
    }
}