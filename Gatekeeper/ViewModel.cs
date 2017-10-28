using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Gatekeeper
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region MVVM
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaiseChanged(params string[] fields)
        {
            try
            {
                foreach (var field in fields)
                {
                    RaisePropertyChanged(field);
                }
            }
            catch (Exception e)
            {
                //Debug.WriteLine("Exception caught during databind update");
                //Debug.WriteLine(e.ToString());
            }
        }

        private void RaisePropertyChanged(string field)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(field));
        }

        protected async void Invoke(Action action)
        {
            await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => action());
        }
        #endregion
    }
}
