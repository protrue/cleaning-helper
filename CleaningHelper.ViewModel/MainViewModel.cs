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
        private readonly string _applicationStatePath = ConfigurationManager.AppSettings["ApplicationStatePath"];
        private string _pathToModel;
        private FrameModel _frameModel;

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
