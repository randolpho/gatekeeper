using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Gatekeeper
{
    public class ViewModel : Microsoft.Practices.Prism.Mvvm.BindableBase
    {
        public const int MaxGroupSize = 8;

        #region UI Bindings
        public ObservableCollection<Group> GroupsList { get; set; } = new ObservableCollection<Group>();
        public ObservableCollection<GroupQueueItem> GroupsQueue { get; set; } = new ObservableCollection<GroupQueueItem>();

        public int TotalGroups { get; set; }
        public int TotalPeople { get; set; }
        public double AverageSize { get; set; }
        public int CurrentGroupNumber { get; set; }
        public int CurrentGroupSize { get; set; }
        public string CurrentFocusName { get; set; }
        #endregion
        #region Commands
        public DelegateCommand<Group> RemoveGroupCommand { get; set; }
        public DelegateCommand<GroupQueueItem> RemoveGroupQueueCommand { get; set; }
        public DelegateCommand SetGroupNumberCommand { get; set; }
        public DelegateCommand SetGroupTotalCommand { get; set; }
        public DelegateCommand AddGroupCommand { get; set; }
        public DelegateCommand OneButtonCommand { get; set; }
        public DelegateCommand TwoButtonCommand { get; set; }
        public DelegateCommand ThreeButtonCommand { get; set; }
        public DelegateCommand FourButtonCommand { get; set; }
        public DelegateCommand FiveButtonCommand { get; set; }
        public DelegateCommand SixButtonCommand { get; set; }
        public DelegateCommand SevenButtonCommand { get; set; }
        public DelegateCommand EightButtonCommand { get; set; }
        public DelegateCommand NineButtonCommand { get; set; }
        public DelegateCommand ZeroButtonCommand { get; set; }
        public DelegateCommand BackButtonCommand { get; set; }
        public DelegateCommand ClearButtonCommand { get; set; }
        public DelegateCommand RefreshQueueCommand { get; set; }
        #endregion
        #region Private Fields
        private bool IsGroupSizeFocused = false;
        private string CurrentGroupNumberStr = "";
        private string CurrentGroupSizeStr = "";
        #endregion
        #region Public Methods
        public ViewModel()
        {
            RemoveGroupCommand = new DelegateCommand<Group>(OnRemoveGroupCommand);
            RemoveGroupQueueCommand = new DelegateCommand<GroupQueueItem>(OnRemoveGroupQueueCommand);
            SetGroupNumberCommand = new DelegateCommand(OnSetGroupNumberCommand);
            SetGroupTotalCommand = new DelegateCommand(OnSetGroupTotalCommand);
            AddGroupCommand = new DelegateCommand(OnAddGroupCommand);
            OneButtonCommand = new DelegateCommand(OnOneCommand);
            TwoButtonCommand = new DelegateCommand(OnTwoCommand);
            ThreeButtonCommand = new DelegateCommand(OnThreeCommand);
            FourButtonCommand = new DelegateCommand(OnFourCommand);
            FiveButtonCommand = new DelegateCommand(OnFiveCommand);
            SixButtonCommand = new DelegateCommand(OnSixCommand);
            SevenButtonCommand = new DelegateCommand(OnSevenCommand);
            EightButtonCommand = new DelegateCommand(OnEightCommand);
            NineButtonCommand = new DelegateCommand(OnNineCommand);
            ZeroButtonCommand = new DelegateCommand(OnZeroCommand);
            BackButtonCommand = new DelegateCommand(OnBackCommand);
            ClearButtonCommand = new DelegateCommand(OnClearCommand);
            RefreshQueueCommand = new DelegateCommand(OnRefreshQueueCommand);
            OnSetGroupNumberCommand();
            OnClearCommand();
        }
        #endregion
        #region Event Handlers 
        #region NumButtons
        private void OnZeroCommand()
        {
            AddNumber(0);
        }

        private void OnNineCommand()
        {
            AddNumber(9);
        }

        private void OnEightCommand()
        {
            AddNumber(8);
        }

        private void OnSevenCommand()
        {
            AddNumber(7);
        }

        private void OnSixCommand()
        {
            AddNumber(6);
        }

        private void OnFiveCommand()
        {
            AddNumber(5);
        }

        private void OnFourCommand()
        {
            AddNumber(4);
        }

        private void OnThreeCommand()
        {
            AddNumber(3);
        }

        private void OnTwoCommand()
        {
            AddNumber(2);
        }

        private void OnOneCommand()
        {
            AddNumber(1);
        }
        #endregion 
        private void OnClearCommand()
        {
            if (IsGroupSizeFocused)
            {
                CurrentGroupSize = 0;
                CurrentGroupSizeStr = "";
                RaiseChanged(nameof(CurrentGroupSize));
            }
            else
            {
                CurrentGroupNumber = 0;
                CurrentGroupNumberStr = "";
                RaiseChanged(nameof(CurrentGroupNumber));
            }
        }

        private void OnBackCommand()
        {
            if (IsGroupSizeFocused)
            {
                if (CurrentGroupSizeStr.Length > 0)
                {
                    CurrentGroupSizeStr = CurrentGroupSizeStr.Substring(0, CurrentGroupSizeStr.Length - 1);
                }
                if (CurrentGroupSizeStr == "")
                {
                    CurrentGroupSize = 0;
                }
                else
                {
                    CurrentGroupSize = int.Parse(CurrentGroupSizeStr);
                }
                RaiseChanged(nameof(CurrentGroupSize));
            }
            else
            {
                if (CurrentGroupNumberStr.Length > 0)
                {
                    CurrentGroupNumberStr = CurrentGroupNumberStr.Substring(0, CurrentGroupNumberStr.Length - 1);
                }
                if (CurrentGroupNumberStr == "")
                {
                    CurrentGroupNumber = 0;
                }
                else
                {
                    CurrentGroupNumber = int.Parse(CurrentGroupNumberStr);
                }
                RaiseChanged(nameof(CurrentGroupNumber));
            }
        }

        private void OnAddGroupCommand()
        {
            TotalGroups++;
            TotalPeople += CurrentGroupSize;
            AverageSize = Math.Round(((double)TotalPeople) / ((double)TotalGroups), 2);
            GroupsList.Add(new Group()
            {
                GroupNumber = CurrentGroupNumber,
                GroupSize = CurrentGroupSize,
            });
            CurrentGroupSize = 0;
            CurrentGroupSizeStr = "";
            CurrentGroupNumber = 0;
            CurrentGroupNumberStr = "";
            RaiseChanged(nameof(CurrentGroupSize), nameof(CurrentGroupNumber),
                nameof(TotalGroups), nameof(TotalPeople), nameof(AverageSize));
            OnSetGroupNumberCommand();
            UpdateQueue();
        }


        private void OnSetGroupTotalCommand()
        {
            IsGroupSizeFocused = true;
            CurrentFocusName = "Group Size";
            RaiseChanged(nameof(CurrentFocusName));
        }

        private void OnSetGroupNumberCommand()
        {
            IsGroupSizeFocused = false;
            CurrentFocusName = "Group Number";
            RaiseChanged(nameof(CurrentFocusName));
        }

        private void OnRemoveGroupQueueCommand(GroupQueueItem groups)
        {
            foreach (var group in groups.Groups)
            {
                GroupsList.Remove(group);
            }
            UpdateQueue();
        }

        private void OnRefreshQueueCommand()
        {
            UpdateQueue();
        }

        private void OnRemoveGroupCommand(Group group)
        {
            GroupsList.Remove(group);
            UpdateQueue();
        }
        #endregion
        #region Private Methods
        private void UpdateQueue()
        {
            var groups = new List<GroupQueueItem>();
            foreach (var group in GroupsList.ToArray())
            {
                var nextGroup = groups.Where(g => g.GroupSize + group.GroupSize <= MaxGroupSize).FirstOrDefault();
                if (nextGroup == null)
                {
                    var newGroup = new GroupQueueItem();
                    newGroup.GroupName = GetGroupName();
                    newGroup.Groups.Add(group);
                    groups.Add(newGroup);
                }
                else
                {
                    nextGroup.Groups.Add(group);
                }
            }
            var groupList = GroupsList.OrderBy(g => g.GroupNumber).ToList();
            GroupsList.Clear();
            GroupsQueue.Clear();
            foreach (var group in groupList)
            {
                GroupsList.Add(group);
            }
            foreach (var group in groups)
            {
                GroupsQueue.Add(group);
            }
        }

        private string GetGroupName()
        {
            return "Queued Group";
        }

        private void AddNumber(int number)
        {
            if (IsGroupSizeFocused)
            {
                CurrentGroupSizeStr = CurrentGroupSizeStr + number.ToString();
                CurrentGroupSize = int.Parse(CurrentGroupSizeStr);
                RaiseChanged(nameof(CurrentGroupSize));
            }
            else
            {
                CurrentGroupNumberStr = CurrentGroupNumberStr + number.ToString();
                CurrentGroupNumber = int.Parse(CurrentGroupNumberStr);
                RaiseChanged(nameof(CurrentGroupNumber));
            }
        }

        #region MVVM
        private void RaiseChanged(params string[] fields)
        {
            foreach (var field in fields)
            {
                OnPropertyChanged(field);
            }
        }

        protected async void Invoke(Action action)
        {
            await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => action());
        }
        #endregion
        #endregion
    }
}
