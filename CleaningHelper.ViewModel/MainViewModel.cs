using CleaningHelper.Core;
using CleaningHelper.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CleaningHelper.Tools;
using CleaningHelper.ViewModel.Annotations;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace CleaningHelper.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            FrameModel = TestFrameModel;
        }

        public static Domain[] domains = new[]
        {
            new Domain("Логический", new[]
            {
                new DomainValue("Да"),
                new DomainValue("Нет"),
            }),
            new Domain("Тип ткани", new[]
            {
                new DomainValue("Натуральная"),
                new DomainValue("Синтетика"),
            }),
            new Domain("Цвет ткани", new[]
            {
                new DomainValue("Светлая"),
                new DomainValue("Тёмная"),
            }),
            new Domain("Ткань", new[]
            {
                new DomainValue("Хлопок"),
                new DomainValue("Шёлк"),
                new DomainValue("Лён"),
            }),
            new Domain("Вещество", new[]
            {
                new DomainValue("Жир"),
                new DomainValue("Кровь"),
            }),
            new Domain("Возраст пятна", new[]
            {
                new DomainValue("Свежее"),
                new DomainValue("Старое"),
            }),
        };

        public static FrameModel TestFrameModel
        {
            get
            {
                var frames = new[]
                {
                    /* 0 */ new Frame("Ткань"),
                    /* 1 */ new Frame("Натуральная ткань"),
                    /* 2 */ new Frame("Деликатная ткань"),
                    /* 3 */ new Frame("Светлый хлопок"),
                    /* 4 */ new Frame("Тёмный хлопок"),
                    /* 5 */ new Frame("Свежее жирное на светлом хлопке"),
                    /* 6 */ new Frame("Старое жирное на светлом хлопке"),
                    /* 7 */ new Frame("Старое жирное на тёмном хлопке"),
                    /* 8 */ new Frame("Жирное пятно"),
                    /* 9 */ new Frame("Свежее жирное пятно"),
                    /* 10 */ new Frame("Старое жирное пятно"),
                   
                    /* 11 */ new Frame("Кровавое пятно"),
                    /* 12 */ new Frame("Свежее кровавое пятно"),
                    /* 13 */ new Frame("Старое кровавое пятно"),
                    /* 14 */ new Frame("Свежее кровавое на светлом хлопке"),
                    /* 15 */ new Frame("Старое кровавое на светлом хлопке"),
                    /* 16 */ new Frame("Старое кровавое на тёмном хлопке"),   
                    
                    /* 17 */ new Frame("Неделикатная ткань"),
                    /* 18 */ new Frame("Тёмный лён"),
                    /* 19 */ new Frame("Старое жирное на тёмном льне"),
                    // /* 20 */ new Frame("Тёмный деним"),
                    
                };
                
                var frameModel = new FrameModel();
                
                frames[1].Slots.Add(new DomainSlot("Тип ткани", domains[1], domains[1][0]));
                frames[1].Parent = frames[0];
                
                frames[2].Slots.Add(new DomainSlot("Деликатная", domains[0], domains[0][0]));
                frames[2].Parent = frames[1];
                
                frames[3].Slots.Add(new DomainSlot("Цвет ткани", domains[2], domains[2][0], false, true));
                frames[3].Slots.Add(new DomainSlot("Ткань", domains[3], domains[3][0], false, true));
                frames[3].Parent = frames[2];
                
                frames[4].Slots.Add(new DomainSlot("Цвет ткани", domains[2], domains[2][1], false, true));
                frames[4].Slots.Add(new DomainSlot("Ткань", domains[3], domains[3][0], false, true));
                frames[4].Parent = frames[2];
                
                frames[8].Slots.Add(new DomainSlot("Вещество", domains[4], domains[4][0], false, true));
                
                frames[9].Slots.Add(new DomainSlot("Возраст пятна", domains[5], domains[5][0], false, true));
                frames[9].Parent = frames[8];
                frames[10].Slots.Add(new DomainSlot("Возраст пятна", domains[5], domains[5][1], false, true));
                frames[10].Parent = frames[8];
                
                frames[5].Slots.Add(new FrameSlot("Тип пятна", frames[9]));
                frames[5].Parent = frames[3];
                
                frames[6].Slots.Add(new FrameSlot("Тип пятна", frames[10]));
                frames[6].Parent = frames[3];
                
                frames[7].Slots.Add(new FrameSlot("Тип пятна", frames[10]));
                frames[7].Parent = frames[4];
                
                frames[11].Slots.Add(new DomainSlot("Вещество", domains[4], domains[4][1], false, true));

                frames[12].Slots.Add(new DomainSlot("Возраст пятна", domains[5], domains[5][0], false, true));
                frames[12].Parent = frames[11];
                
                frames[13].Slots.Add(new DomainSlot("Возраст пятна", domains[5], domains[5][1], false, true));
                frames[13].Parent = frames[11];
                
                frames[14].Slots.Add(new FrameSlot("Тип пятна", frames[12]));
                frames[14].Parent = frames[3];
                
                frames[15].Slots.Add(new FrameSlot("Тип пятна", frames[13]));
                frames[15].Parent = frames[3];
                
                frames[16].Slots.Add(new FrameSlot("Тип пятна", frames[13]));
                frames[16].Parent = frames[4];
                
                frames[17].Slots.Add(new DomainSlot("Деликатная", domains[0], domains[0][1]));
                frames[17].Parent = frames[1];
                
                frames[18].Slots.Add(new DomainSlot("Цвет ткани", domains[2], domains[2][1], false, true));
                frames[18].Slots.Add(new DomainSlot("Ткань", domains[3], domains[3][2], false, true));
                frames[18].Parent = frames[17];
                
                frames[19].Slots.Add(new FrameSlot("Тип пятна", frames[10]));
                frames[19].Parent = frames[18];
                
                foreach (var domain in domains)
                {
                    frameModel.Domains.Add(domain);
                }

                foreach (var frame in frames)
                {
                    frameModel.Frames.Add(frame);
                }

                return frameModel;
            }
        }
        
        private readonly string _applicationStatePath = ConfigurationManager.AppSettings["ApplicationStatePath"];
        private string _pathToModel;
        private FrameModel _frameModel = TestFrameModel;

        public event PropertyChangedEventHandler PropertyChanged;

        public const string FileDialogFilter = "Frame model binary file (*.fmb)|*.fmb";
        public const string DefaultModelName = "New frame model";
        public const string DefaultModelExtension = "fmb";

        public string DefaultPathToModel => $"{DefaultModelName}.{DefaultModelExtension}";

        public FrameModel FrameModel
        {
            get => _frameModel;
            set
            {
                _frameModel = value;
                OnPropertyChanged(nameof(FrameModel));
            }
        }


        public string PathToModel
        {
            get => _pathToModel;
            set
            {
                _pathToModel = value;
                OnPropertyChanged(nameof(PathToModel));
            }
        }

        public bool IsModelLoaded => FrameModel != null;

        public Command CreateModelCommand => new Command(parameter =>
        {
            if (FrameModel != null)
            {
                SaveModelCommand.Execute();
            }

            FrameModel = new FrameModel();

            PathToModel = DefaultPathToModel;
        });

        public Command LoadModelCommand => new Command(parameter =>
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = FileDialogFilter
            };
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                PathToModel = openFileDialog.FileName;

                try
                {
                    FrameModel = FrameModelSerializer.Deserialize(PathToModel);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        });

        public Command SaveModelCommand => new Command(parameter =>
        {
            var saveFileDialog = new SaveFileDialog()
            {
                Filter = FileDialogFilter,
                FileName = DefaultPathToModel
            };

            var result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                PathToModel = saveFileDialog.FileName;

                try
                {
                    FrameModelSerializer.Serialize(PathToModel, FrameModel);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        });

        public Command LoadStateCommand => new Command(parameter =>
        {
            if (!File.Exists(_applicationStatePath))
                return;

            var stateJson = File.ReadAllText(_applicationStatePath);
            var state = JsonConvert.DeserializeObject<ApplicationState>(stateJson);

            PathToModel = state.PathToModel;
            try
            {
                FrameModel = FrameModelSerializer.Deserialize(PathToModel);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        });

        public Command SaveStateCommand => new Command(parameter =>
        {
            var state = new ApplicationState
            {
                PathToModel = PathToModel,
            };

            var stateJson = JsonConvert.SerializeObject(state, Formatting.Indented);
            File.WriteAllText(_applicationStatePath, stateJson);

            if (FrameModel == null)
                return;

            try
            {
                FrameModelSerializer.Serialize(PathToModel, FrameModel);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        });

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
