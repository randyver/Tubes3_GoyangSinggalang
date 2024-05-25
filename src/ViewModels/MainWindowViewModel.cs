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
    using System.Text.RegularExpressions;
    using Solver = Controllers.Solver;

    namespace src.ViewModels
    {
        public class MainWindowViewModel : ViewModelBase
        {
            private bool _isKMPChecked;
            private Bitmap? _selectedImage;
            private Bitmap? _resultImage;
            private string _selectedImagePath = string.Empty;
            private string _time = "0";
            private string _matchRate = "0";
            private User? _user;
            private bool _error = false;
            private bool _initial = true;
            private string _error_message = "No error occurred";
            private string _resultName;
            private bool _isSearching = false;

            public bool IsKMPChecked
            {
                get => _isKMPChecked;
                set
                {
                    this.RaiseAndSetIfChanged(ref _isKMPChecked, value);
                }
            }

            public Bitmap? SelectedImage
            {
                get => _selectedImage;
                set => this.RaiseAndSetIfChanged(ref _selectedImage, value);
            }

            public Bitmap? ResultImage
            {
                get => _resultImage;
                set => this.RaiseAndSetIfChanged(ref _resultImage, value);
            }

            public string ResultName
            {
                get => _resultName;
                set
                {
                    this.RaiseAndSetIfChanged(ref _resultName, value);
                    this.RaisePropertyChanged(nameof(ResultNameString));
                }
            }

            public string ResultNameString
            {
                get
                {
                    if (_initial || _isSearching || _error)
                    {
                        return "";
                    }

                    return "Nama: " + ResultName;
                }
            }

            public string ExecutionTimeString
            {
                get
                {
                    if (_isSearching)
                    {
                        return "Searching...";
                    }

                    if (_error)
                    {
                        return "Error occurred";
                    }

                    return "Execution time: " + ExecutionTime + " ms";
                }
            }

            public string ExecutionTime
            {
                get => _time;
                set
                {
                    this.RaiseAndSetIfChanged(ref _time, value);
                    this.RaisePropertyChanged(nameof(ExecutionTimeString));
                }
            }

            public string MatchRateString
            {
                get
                {
                    if (_isSearching)
                    {
                        return "";
                    }

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
                set
                {
                    this.RaiseAndSetIfChanged(ref _matchRate, value);
                    this.RaisePropertyChanged(nameof(MatchRateString));
                }
            }

            public bool IsSearching
            {
                get => _isSearching;
                set
                {
                    this.RaiseAndSetIfChanged(ref _isSearching, value);
                    this.RaisePropertyChanged(nameof(ExecutionTimeString));
                    this.RaisePropertyChanged(nameof(MatchRateString));
                }
            }


            public User UserStatus
            {
                set
                {
                    this.RaiseAndSetIfChanged(ref _user, value);
                    this.RaisePropertyChanged(nameof(userNIK));
                    this.RaisePropertyChanged(nameof(userTempatLahir));
                    this.RaisePropertyChanged(nameof(userTanggalLahirString));
                    this.RaisePropertyChanged(nameof(userJenisKelamin));
                    this.RaisePropertyChanged(nameof(userGolonganDarah));
                    this.RaisePropertyChanged(nameof(userAlamat));
                    this.RaisePropertyChanged(nameof(userAgama));
                    this.RaisePropertyChanged(nameof(userStatusPerkawinan));
                    this.RaisePropertyChanged(nameof(userPekerjaan));
                    this.RaisePropertyChanged(nameof(userKewarganegaraan));
                }
            }

            public string? userNIK
            {
                get
                {
                    if (_initial || _isSearching || _error)
                    {
                        return "";
                    }

                    return "NIK: " + _user?.GetNik();
                }
            }

            public string? userNama
            {
                get
                {
                    if (_initial || _isSearching || _error)
                    {
                        return "";
                    }

                    return "Nama: " + ResultName;
                }
            }

            public string? userTempatLahir
            {
                get
                {
                    if (_initial || _isSearching || _error)
                    {
                        return "";
                    }

                    return "Tempat Lahir: " + _user?.GetTempatLahir();
                }
            }

            public string? userTanggalLahirString
            {
                get
                {
                    if (_initial || _isSearching || _error || _user?.GetTanggalLahir() == null)
                    {
                        return "";
                    }
                    return "Tanggal Lahir: " + _user.GetTanggalLahir().ToString("dd MMMM yyyy");
                }
            }


            public string? userJenisKelamin
            {
                get
                {
                    if (_initial || _isSearching || _error)
                    {
                        return "";
                    }

                    return "Jenis Kelamin: " + _user?.GetJenisKelamin();
                }
            }

            public string? userGolonganDarah
            {
                get
                {
                    if (_initial || _isSearching || _error)
                    {
                        return "";
                    }

                    return "Golongan Darah: " + _user?.GetGolonganDarah();
                }
            }

            public string? userAlamat
            {
                get
                {
                    if (_initial || _isSearching || _error)
                    {
                        return "";
                    }

                    return "Alamat: " + _user?.GetAlamat();
                }
            }

            public string? userAgama
            {
                get
                {
                    if (_initial || _isSearching || _error)
                    {
                        return "";
                    }

                    return "Agama: " + _user?.GetAgama();
                }
            }

            public string? userStatusPerkawinan
            {
                get
                {
                    if (_initial || _isSearching || _error)
                    {
                        return "";
                    }

                    return "Status Perkawinan: " + _user?.GetStatusPerkawinan();
                }
            }

            public string? userPekerjaan
            {
                get
                {
                    if (_initial || _isSearching || _error)
                    {
                        return "";
                    }

                    return "Pekerjaan: " + _user?.GetPekerjaan();
                }
            }

            public string? userKewarganegaraan
            {
                get
                {
                    if (_initial || _isSearching || _error)
                    {
                        return "";
                    }

                    return "Kewarganegaraan: " + _user?.GetKewarganegaraan();
                }
            }


            public string ResultColor
            {
                get
                {
                    if (_error)
                    {
                        return "Red";
                    }
                    return "#E84545";
                }
            }

            public string ErrorMessage
            {
                get => _error_message;
                set
                {
                    this.RaiseAndSetIfChanged(ref _error_message, value);
                    this.RaisePropertyChanged(nameof(ResultColor));
                    this.RaisePropertyChanged(nameof(ExecutionTimeString));
                    this.RaisePropertyChanged(nameof(MatchRateString));
                    UserStatus = new User("", "", "", DateTime.Now, "", "", "", "", "", "", "");
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
                        Title = "Select an Image",
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
                        _selectedImagePath = filePath;
                    }
                });

                SearchCommand = ReactiveCommand.CreateFromTask(async () =>
                {
                    _error = false;
                    _initial = false;
                    IsSearching = true;

                    // Reset biodata and result image
                    UserStatus = new User("", "", "", DateTime.Now, "", "", "", "", "", "", "");
                    ResultImage = null;
                    ExecutionTime = "0";
                    MatchRate = "0";
                    ResultName = "";

                    // No image selected
                    if (SelectedImage == null)
                    {
                        _error = true;
                        ErrorMessage = "No image selected";
                        IsSearching = false;
                        return;
                    }

                    // Image selected but format not supported
                    if (SelectedImage.PixelSize.Width == 0)
                    {
                        _error = true;
                        ErrorMessage = "Unsupported image format";
                        IsSearching = false;
                        return;
                    }

                    // Use the stored file path
                    var filePath = _selectedImagePath;
                    if (string.IsNullOrEmpty(filePath))
                    {
                        _error = true;
                        ErrorMessage = "Unable to retrieve the file path for the selected image";
                        IsSearching = false;
                        return;
                    }

                    // Create a Solver instance
                    Solver solver = new Solver(filePath, IsKMPChecked);

                    // Perform the solve operation asynchronously
                    await Task.Run(() => solver.Solve());

                    // End searching state
                    IsSearching = false;

                    // Get the results from the solver
                    var duration = solver.GetDuration();
                    var similarity = solver.GetSimilarity();
                    var userData = solver.GetUserData();
                    var fingerPrintData = solver.GetFingerPrintData();

                    if (userData == null || fingerPrintData == null)
                    {
                        _error = true;
                        ErrorMessage = "No matching fingerprint found";
                        IsSearching = false;
                        return;
                    }

                    // Update the view model properties with the results
                    ExecutionTime = duration?.ToString() ?? "N/A";
                    MatchRate = ((similarity ?? 0) * 100).ToString("F2");

                    UserStatus = userData;

                    // Load the fingerprint image as the result image
                    var resultImageFilePath = fingerPrintData?.GetPath();
                    ResultName = fingerPrintData?.GetNama();

                    if (resultImageFilePath != null)
                    {
                        ResultImage = new Bitmap(resultImageFilePath);
                    }
                    else
                    {
                        _error = true;
                        ErrorMessage = "Failed to load the result image";
                    }

                    // // End searching state
                    // IsSearching = false;

                    Console.WriteLine(resultImageFilePath);
                    Console.WriteLine(ResultName);
                    Console.WriteLine("Execution time   : " + ExecutionTime + " ms");
                    Console.WriteLine("Match rate       : " + MatchRate + "%");
                });
            }
        }
    }
