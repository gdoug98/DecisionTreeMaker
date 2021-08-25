using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree.Entities.Classes
{
    public class DecisionData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if(value != _title)
                {
                    _title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }
        public ObservableCollection<DecisionAction> Actions { get; private set; }
        public string Description { get; set; }
        public bool IsRootDecision { get; set; }

        public DecisionData()
        {
            Actions = new ObservableCollection<DecisionAction>();
        }

        public DecisionData(IEnumerable<string> actions, string title, string desc)
        {
            Actions = new ObservableCollection<DecisionAction>();
            foreach(string action in actions)
            {
                Actions.Add(new DecisionAction { Action = action });
            }
            Title = title;
            Description = desc;
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class DecisionAction : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _action;
        public string Action
        {
            get
            {
                return _action;
            }

            set
            {
                if(value != _action)
                {
                    _action = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
