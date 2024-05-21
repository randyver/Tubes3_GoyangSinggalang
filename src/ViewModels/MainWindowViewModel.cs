using System;
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
        private bool _error = false;
        private string _error_message = "No error occurred";


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

public string ExecutionTimeString 
        {
            get {
                if (_error) {
                    return "Error occurred";
                }
                return "Execution time: " + ExecutionTime + " ms";
            }
        }

        public string ExecutionTime
        {
            get => _time;
            set {
                this.RaiseAndSetIfChanged(ref _time, value);
                this.RaisePropertyChanged(nameof(ExecutionTimeString));
            }
        }

        public string MatchRateString
        {
            get
            {
                if (_error)
                {
                    return _error_message;
                }
                return "Match rate: " + MatchRate + "%";
            }
        }

        public string MatchRate
        {
            get => _matchRate;
            set {
                this.RaiseAndSetIfChanged(ref _matchRate, value);
                this.RaisePropertyChanged(nameof(MatchRateString));
            }
        }

        public User UserStatus 
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

        public string ResultColor 
        {
            get {
                if (_error)
                {
                    return "Red";
                }
                return "Green";
            }
        }

        public string ErrorMessage 
        {
            get => _error_message;
            set {
                this.RaiseAndSetIfChanged(ref _error_message, value);
                this.RaisePropertyChanged(nameof(ResultColor));
                this.RaisePropertyChanged(nameof(ExecutionTimeString));
                this.RaisePropertyChanged(nameof(MatchRateString));
            }
        }

        public User user 
        {
            get => _user;
            set {
                this.RaiseAndSetIfChanged(ref _user, value);
                this.RaisePropertyChanged(nameof(userNIK));
                this.RaisePropertyChanged(nameof(userNama));
                this.RaisePropertyChanged(nameof(userTempatLahir));
                this.RaisePropertyChanged(nameof(userTanggalLahir));
                this.RaisePropertyChanged(nameof(userJenisKelamin));
                this.RaisePropertyChanged(nameof(userGolonganDarah));
                this.RaisePropertyChanged(nameof(userAlamat));
                this.RaisePropertyChanged(nameof(userAgama));
                this.RaisePropertyChanged(nameof(userStatusPerkawinan));
                this.RaisePropertyChanged(nameof(userPekerjaan));
                this.RaisePropertyChanged(nameof(userKewarganegaraan));
            }
        }



        public ReactiveCommand<Unit, Unit> OpenFileCommand { get; }
        public ReactiveCommand<Unit, Unit> SearchCommand { get; }


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


            SearchCommand = ReactiveCommand.Create(() =>
            {
                // TODO: Implement search logic here 
                // Assigned to: @randy
                _error = false;

                // No image selected
                if (SelectedImage == null)
                {
                    _error = true;
                    ErrorMessage = "No image selected";
                    return;
                }

                // Image selected but format not supported
                if (SelectedImage.PixelSize.Width == 0)
                {
                    _error = true;
                    ErrorMessage = "Unsupported image format";
                    return;
                }
                
                // OK!
                ExecutionTime = "123";
                MatchRate = "95";   
                Console.WriteLine("Search command executed");
                Console.WriteLine("Execution time: " + ExecutionTime + " ms");
                Console.WriteLine("Agama" + userAgama);
                UserStatus = new User("1234567890", "Dewantoro Triatmojo", "Jakarta", "01-01-2000", "Laki-laki", "O", "Jl. Kebon Jeruk", "Islam", "Belum Kawin", "Mahasiswa", "WNI");
                ResultImage = SelectedImage;
            });
        }
    }
}
