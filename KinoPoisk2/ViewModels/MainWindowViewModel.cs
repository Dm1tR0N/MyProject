using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using Avalonia.Collections;
using DynamicData;
using KinoPoisk2.Models;
using KinoPoisk2.Views;
using ReactiveUI;
using Tmds.DBus;
using Image = Avalonia.Controls.Image;

namespace KinoPoisk2.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        // Стандартные настройки для приложения
        public string NameProgram => "My movie / Мой фильм"; // Название проекта

        public string ThemaProgram => "Dark";
        public string BackgroundProgram => "#3C4041"; // Цвет фона для приложения
        public string ForegroundLetters => "#FFFFFF"; // Цвет текса в программе

        public readonly List<Result> Results = new List<Result>();
        public readonly List<Link> Links = new List<Link>();
        public readonly List<Multimedia> Multimedias = new List<Multimedia>();
        
    }
}