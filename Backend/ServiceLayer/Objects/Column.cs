
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct Column
    {
        public readonly IReadOnlyCollection<Task> Tasks;
        public readonly string Name;
        public readonly int Limit;
        internal Column(IReadOnlyCollection<Task> tasks, string name, int limit)
        {
            this.Tasks = tasks;
            this.Name = name;
            this.Limit = limit;
        }
        internal Column(BusinessLayer.BoardPackage.Column toCopy)
        {
            List<Task> newTasks = new List<Task>();
            foreach(BusinessLayer.BoardPackage.Task task in toCopy.TaskByID)
            {
                newTasks.Add(new Task(task));
            }
            this.Tasks = (IReadOnlyCollection<Task>)newTasks;
            this.Name = toCopy.Name;
            this.Limit = toCopy.Limit;
        }
    }
}


