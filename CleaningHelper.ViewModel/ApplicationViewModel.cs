using CleaningHelper.Core;
using CleaningHelper.Model;
using CleaningHelper.OntolisAdapter.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CleaningHelper.ViewModel
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private string pathToModel;
        private string pathToOntolis;
        private OntolisDataObject ontolisDataObject;
        private SemanticNetwork model;
        private Reasoner reasoner;

        public event PropertyChangedEventHandler PropertyChanged;

        public string PathToOntolis
        {
            get => pathToOntolis; set
            {
                pathToOntolis = value;
                OnPropertyChanged(nameof(PathToOntolis));
                OnPropertyChanged(nameof(IsEditModelButtonEnabled));
            }
        }
        public string PathToModel
        {
            get => pathToModel; set
            {
                pathToModel = value;
                OnPropertyChanged(nameof(PathToModel));
                TryLoadModel();
            }
        }

        public bool IsEditModelButtonEnabled
        {
            get => File.Exists(PathToOntolis) && Model != null;
        }

        public bool IsStartConsultationButtonEnabled
        {
            get => Model != null;
        }

        public OntolisDataObject OntolisDataObject
        {
            get => ontolisDataObject; set
            {
                ontolisDataObject = value;
                OnPropertyChanged(nameof(OntolisDataObject));
            }
        }

        public SemanticNetwork Model
        {
            get => model; set
            {
                model = value;
                OnPropertyChanged(nameof(Model));
                OnPropertyChanged(nameof(IsStartConsultationButtonEnabled));
                OnPropertyChanged(nameof(IsEditModelButtonEnabled));
            }
        }

        public Reasoner Reasoner
        {
            get => reasoner; set => reasoner = value;
        }

        public ApplicationViewModel()
        {
            PathToModel = "Не загружена";
            PathToOntolis = "Не указан";
        }

        private void TryLoadModel()
        {
            if (!File.Exists(PathToModel))
                return;

            var ontolisDataObject = OntolisAdapter.Tools.OntolisFileDeserializer.DeserializeOntolisFile(PathToModel);
            Model = OntolisAdapter.Tools.OntolisDataConverter.Convert(ontolisDataObject);
        }

        public void OnPropertyChanged([CallerMemberName]string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
