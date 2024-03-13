using Newtonsoft.Json;
using Syncfusion.Maui.ListView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadMoreDemo
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region Fields

        private IList<dynamic> records;
        private bool isDownloaded;
        private bool isBusy;
        private int totalRecords;
        #endregion

        #region Properties
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }

        public IList<dynamic> Records
        {
            get { return records; }
            set
            {
                records = value;
                OnPropertyChanged("Items");
            }
        }

        public Command LoadMoreRecordsCommand { get; set; }

        public IList<dynamic> JSONCollection { get; set; }

        public DataServices DataService { get; set; }

        #endregion

        #region Constructor

        public ViewModel()
        {
            JSONCollection = new ObservableCollection<dynamic>();
            Records = new ObservableCollection<dynamic>();
            DataService = new DataServices();
            GetDataAsync();
            LoadMoreRecordsCommand = new Command(LoadMoreRecords);
        }
        #endregion

        #region Methods
        private async void GetDataAsync()
        {
            isDownloaded = await DataService.DownloadJsonAsync();
            if (isDownloaded)
            {
                var localFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var fileText = File.ReadAllText(Path.Combine(localFolder, "Data.json"));

                //Read data from the local path and set it to the collection bound to the DataGrid.
                JSONCollection = JsonConvert.DeserializeObject<IList<dynamic>>(fileText);

                AddRecords(1, 20);
                totalRecords = JSONCollection.Count;
            }
        }

        private void AddRecords(int index, int count)
        {
            for (int i = index; i < index + count; i++)
            {
                Records.Add(JSONCollection[i]);
            }
        }

        private async void LoadMoreRecords()
        {
            try
            {
                IsBusy = true;
                await Task.Delay(3000);
                var index = Records.Count;
                var count = index + 10 >= totalRecords ? totalRecords - index : 10;
                AddRecords(index, count);
            }
            catch
            {

            }
            finally
            {
                IsBusy = false;
            }
        }
        #endregion

        #region Interface Member

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
