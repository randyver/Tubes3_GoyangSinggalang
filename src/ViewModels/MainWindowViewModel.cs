﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Media.Imaging;
using ReactiveUI;
using Models;


namespace src.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private bool _isKMPChecked;
        private Bitmap? _selectedImage;
        private Bitmap? _resultImage;
        private string _time = "0";
        private string _matchRate = "0";
        private User? _user;


        public bool IsKMPChecked
        {
            get => _isKMPChecked;
            set {
                Console.WriteLine(Models.Utils.GetBahasaAlay("Dewantoro Triatmojo"));
                this.RaiseAndSetIfChanged(ref _isKMPChecked, value);
                Console.WriteLine("After change, KMP: " + value);
            }
        }

        public Bitmap? SelectedImage
        {
            get => _selectedImage;
            set {
                this.RaiseAndSetIfChanged(ref _selectedImage, value);
            }
        }

        public Bitmap? ResultImage
        {
            get => _resultImage;
            set => this.RaiseAndSetIfChanged(ref _resultImage, value);
        }

        public string ExecutionTime 
        {
            get => _time;
            set => this.RaiseAndSetIfChanged(ref _time, value);
        }

        public string MatchRate
        {
            get => _matchRate;
            set => this.RaiseAndSetIfChanged(ref _matchRate, value);
        }

        public User user 
        {
            set => this.RaiseAndSetIfChanged(ref _user, value);
        }

        public string? userNIK
        {
            get => _user?.GetNik();
        }

        public string? userNama
        {
            get => _user?.GetNama();
        }

        public string? userTempatLahir
        {
            get => _user?.GetTempatLahir();
        }

        public string? userTanggalLahir
        {
            get => _user?.GetTanggalLahir();
        }

        public string? userJenisKelamin
        {
            get => _user?.GetJenisKelamin();
        }

        public string? userGolonganDarah
        {
            get => _user?.GetGolonganDarah();
        }

        public string? userAlamat
        {
            get => _user?.GetAlamat();
        }

        public string? userAgama
        {
            get => _user?.GetAgama();
        }

        public string? userStatusPerkawinan
        {
            get => _user?.GetStatusPerkawinan();
        }

        public string? userPekerjaan
        {
            get => _user?.GetPekerjaan();
        }

        public string? userKewarganegaraan
        {
            get => _user.GetKewarganegaraan();
        }


        public ReactiveCommand<Unit, Unit> OpenFileCommand { get; }

        public MainWindowViewModel()
        {
            OpenFileCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var storageProvider = new Window().StorageProvider;
                var options = new FilePickerOpenOptions
                {
                    Title = "Select an image",
                    FileTypeFilter = new List<FilePickerFileType>
                    {
                        new FilePickerFileType("Images")
                        {
                            Patterns = new List<string> { "*.png", "*.jpg", "*.jpeg", "*.BMP" }
                        }
                    }
                };

                var result = await storageProvider.OpenFilePickerAsync(options);
                if (result.Any())
                {
                    var filePath = result.First().Path.LocalPath;
                    SelectedImage = new Bitmap(filePath);
                }
            });
        }
    }
}