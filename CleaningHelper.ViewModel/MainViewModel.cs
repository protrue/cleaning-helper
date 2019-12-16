using CleaningHelper.Core;
using CleaningHelper.Model;
using CleaningHelper.OntolisAdapter.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CleaningHelper.ViewModel.Annotations;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace CleaningHelper.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _pathToModel;
        private string _pathToOntolis;
        private OntolisDataObject _ontolisDataObject;
        private SemanticNetwork _model;

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly string _applicationStatePath = ConfigurationManager.AppSettings["ApplicationStatePath"];

        public string PathToOntolis
        {
            get => _pathToOntolis; set
            {
                _pathToOntolis = value;
                OnPropertyChanged(nameof(PathToOntolis));
                OnPropertyChanged(nameof(IsEditModelButtonEnabled));
            }
        }
        public string PathToModel
        {
            get => _pathToModel; set
            {
                _pathToModel = value;
                OnPropertyChanged(nameof(PathToModel));
                TryLoadModel();
            }
        }

        public bool IsEditModelButtonEnabled => File.Exists(PathToOntolis) && Model != null;

        public bool IsStartConsultationButtonEnabled => Model != null;

        public OntolisDataObject OntolisDataObject
        {
            get => _ontolisDataObject; set
            {
                _ontolisDataObject = value;
                OnPropertyChanged(nameof(OntolisDataObject));
            }
        }

        public SemanticNetwork Model
        {
            get => _model; set
            {
                _model = value;
                OnPropertyChanged(nameof(Model));
                OnPropertyChanged(nameof(IsStartConsultationButtonEnabled));
                OnPropertyChanged(nameof(IsEditModelButtonEnabled));
            }
        }

        public Command SetPathToOntolisCommand => new Command(parameter =>
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Ontolis executable file (ontolis.exe)|ontolis.exe"
            };
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                PathToOntolis = openFileDialog.FileName;
            }
        });

        public Command LoadModelCommand => new Command(parameter =>
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Ontolis ontology file (*.ont)|*.ont"
            };
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                PathToModel = openFileDialog.FileName;
            }
        });

        public Command RunOntolisCommand => new Command(parameter =>
        {
            var ontolisRunner = new OntolisAdapter.Tools.OntolisRunner(PathToOntolis);
            ontolisRunner.RunOntolis();
        });

        public Command LoadStateCommand => new Command(parameter =>
        {
            if (!File.Exists(_applicationStatePath))
                return;

            var stateJson = File.ReadAllText(_applicationStatePath);
            var state = JsonConvert.DeserializeObject<ApplicationState>(stateJson);
            PathToOntolis = state.PathToOntolis;
            PathToModel = state.PathToModel;
        });

        public Command SaveStateCommand => new Command(parameter =>
        {
            var state = new ApplicationState
            {
                PathToModel = PathToModel,
                PathToOntolis = PathToOntolis
            };

            var stateJson = JsonConvert.SerializeObject(state, Formatting.Indented);
            File.WriteAllText(_applicationStatePath, stateJson);
        });

        public MainViewModel()
        {
            PathToModel = "Не загружена";
            PathToOntolis = "Не указан";
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void TryLoadModel()
        {
            if (!File.Exists(PathToModel))
                return;

            var ontolisDataObject = OntolisAdapter.Tools.OntolisFileDeserializer.DeserializeOntolisFile(PathToModel);
            Model = OntolisAdapter.Tools.OntolisDataConverter.Convert(ontolisDataObject);
        }
    }
}
