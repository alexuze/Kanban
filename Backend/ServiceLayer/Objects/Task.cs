using System;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct Task
    {
        public readonly int Id;
        public readonly DateTime CreationTime;
        public readonly DateTime DueDate;
        public readonly string Title;
        public readonly string Description;
        public readonly string emailAssignee;
        internal Task(int id, DateTime creationTime, string title, string description, DateTime dueDate, string email)

        {
            this.Id = id;
            this.CreationTime = creationTime;
            this.Title = title;
            this.Description = description;
            this.DueDate = dueDate;
            this.emailAssignee = email;

        }
        internal Task(BusinessLayer.BoardPackage.Task toCopy)
        {
            this.Id = toCopy.Id;
            this.CreationTime = toCopy.Creationtime;
            this.Title = toCopy.Title;
            this.Description = toCopy.Description;
            this.DueDate = toCopy.DueDate;
            this.emailAssignee = toCopy.Email;
        }
    }
}
