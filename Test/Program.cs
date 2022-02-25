using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;
using Task = IntroSE.Kanban.Backend.ServiceLayer.Task;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            int choice;
            bool terminate = false;
            IService service = new Service();
            Response  reg = service.DeleteData();
            //Response reg = service.LoadData();
            
            reg = service.Register("gg@gmail.com", "aA123456", "dasda");
            reg = service.Register("ggg@gmail.com", "aA123456", "beforeTest");
            reg = service.Register("gggg@gmail.com", "aA123456", "dasda","ggg@gmail.com");
            reg = service.Register("ggggg@gmail.com", "aA123456", "dasda");
            reg = service.Login("ggg@gmail.com", "aA123456");
            reg = service.AddTask("ggg@gmail.com", "dasd", "dasda", DateTime.Today.AddDays(1));
            reg = service.AddTask("ggg@gmail.com", "adasd", "daasda", DateTime.Today.AddDays(1));
            reg = service.AddTask("ggg@gmail.com", "adasd", "adasda", DateTime.Today.AddDays(1));
            reg = service.AdvanceTask("ggg@gmail.com", 0, 1);
            reg = service.DeleteTask("ggg@gmail.com", 0, 0);
            reg = service.DeleteTask("ggg@gmail.com", 0, 2);
            reg = service.DeleteTask("ggg@gmail.com", 1, 1);
            reg = service.AssignTask("ggg@gmail.com", 0, 2, "gggg@gmail.com");
            reg = service.ChangeColumnName("ggg@gmail.com", 0, "sasha");
            reg = service.AddTask("ggg@gmail.com", "dasaad", "dasdaaaaa", DateTime.Today.AddDays(1));


            while (!terminate)
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1) LoadDate");
                Console.WriteLine("2) Register");
                Console.WriteLine("3) Login");
                Console.WriteLine("4) Logout");
                Console.WriteLine("5) GetBoard");
                Console.WriteLine("6) LimitColumnTasks");
                Console.WriteLine("7) AddTask");
                Console.WriteLine("8) UpdateTaskDueDate");
                Console.WriteLine("9) UpdateTaskTitle");
                Console.WriteLine("10) UpdateTaskDescription");
                Console.WriteLine("11) AdvanceTask");
                Console.WriteLine("12) GetColumnByName");
                Console.WriteLine("13) GetColumnById");
                Console.WriteLine("14) Exit");
                Console.WriteLine();
                Console.WriteLine();
                Console.Write("Choose option: ");
                choice = int.Parse(Console.ReadLine());
                Console.WriteLine();
                switch (choice)
                {
                    case 1:
                        service.LoadData();
                        break;
                    case 2:
                        Console.Write("Enter email: ");
                        string register_email = Console.ReadLine();
                        Console.Write("Enter password: ");
                        string register_password = Console.ReadLine();
                        Console.Write("Enter nickname: ");
                        string register_nickname = Console.ReadLine();
                        Response register_user = service.Register(register_email, register_password, register_nickname);
                        if (register_user.ErrorOccured)
                        {
                            Console.WriteLine(register_user.ErrorMessage);
                        }
                        break;
                    case 3:
                        Console.Write("Enter email: ");
                        string login_email = Console.ReadLine();
                        Console.Write("Enter password: ");
                        string login_password = Console.ReadLine();
                        Response<User> login_user = service.Login(login_email, login_password);
                        if (login_user.ErrorOccured)
                        {
                            Console.WriteLine(login_user.ErrorMessage);
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("The user's email is: " + login_user.Value.Email);
                            Console.WriteLine("The user's nickname is: " + login_user.Value.Nickname);
                        }
                        break;
                    case 4:
                        Console.Write("Enter email: ");
                        string logout_email = Console.ReadLine();
                        Response logout_user = service.Logout(logout_email);
                        if (logout_user.ErrorOccured)
                        {
                            Console.WriteLine(logout_user.ErrorMessage);
                        }
                        break;
                    case 5:
                        Console.Write("Enter email: ");
                        string email5 = Console.ReadLine();
                        Response<Board> board = service.GetBoard(email5);
                        if (board.ErrorOccured)
                        {
                            Console.WriteLine(board.ErrorMessage);
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("The board's columns: ");
                            foreach (String name in board.Value.ColumnsNames)
                            {
                                Console.WriteLine("* " + name);
                            }
                        }
                        break;
                    case 6:
                        Console.Write("Enter email: ");
                        string email6 = Console.ReadLine();
                        Console.Write("Enter column's id: ");
                        int columnid6 = int.Parse(Console.ReadLine());
                        Console.Write("Enter limit: ");
                        int limit6 = int.Parse(Console.ReadLine());
                        Response newlimit = service.LimitColumnTasks(email6, columnid6, limit6);
                        if (newlimit.ErrorOccured)
                        {
                            Console.WriteLine(newlimit.ErrorMessage);
                        }
                        break;
                    case 7:
                        Console.Write("Enter email: ");
                        string email7 = Console.ReadLine();
                        Console.Write("Enter title: ");
                        string title7 = Console.ReadLine();
                        Console.Write("Enter description: ");
                        string description7 = Console.ReadLine();
                        Console.Write("Enter duedate: ");
                        DateTime duedate7 = DateTime.Parse(Console.ReadLine());
                        Response newtask7 = service.AddTask(email7, title7, description7, duedate7);
                        if (newtask7.ErrorOccured)
                        {
                            Console.WriteLine(newtask7.ErrorMessage);
                        }
                        break;
                    case 8:
                        Console.Write("Enter email: ");
                        string email8 = Console.ReadLine();
                        Console.Write("Enter column id: ");
                        int columnid8 = int.Parse(Console.ReadLine());
                        Console.Write("Enter task id: ");
                        int taskid8 = int.Parse(Console.ReadLine());
                        Console.Write("Enter duedate: ");
                        DateTime duedate8 = DateTime.Parse(Console.ReadLine());
                        Response newtask8 = service.UpdateTaskDueDate(email8, columnid8, taskid8, duedate8);
                        if (newtask8.ErrorOccured)
                        {
                            Console.WriteLine(newtask8.ErrorMessage);
                        }
                        break;
                    case 9:
                        Console.Write("Enter email: ");
                        string email9 = Console.ReadLine();
                        Console.Write("Enter column id: ");
                        int columnid9 = int.Parse(Console.ReadLine());
                        Console.Write("Enter task id: ");
                        int taskid9 = int.Parse(Console.ReadLine());
                        Console.Write("Enter title: ");
                        string title9 = Console.ReadLine();
                        Response newtask9 = service.UpdateTaskTitle(email9, columnid9, taskid9, title9);
                        if (newtask9.ErrorOccured)
                        {
                            Console.WriteLine(newtask9.ErrorMessage);
                        }
                        break;
                    case 10:
                        Console.Write("Enter email: ");
                        string email10 = Console.ReadLine();
                        Console.Write("Enter column id: ");
                        int columnid10 = int.Parse(Console.ReadLine());
                        Console.Write("Enter task id: ");
                        int taskid10 = int.Parse(Console.ReadLine());
                        Console.Write("Enter description: ");
                        string description10 = Console.ReadLine();
                        Response newtask10 = service.UpdateTaskDescription(email10, columnid10, taskid10, description10);
                        if (newtask10.ErrorOccured)
                        {
                            Console.WriteLine(newtask10.ErrorMessage);
                        }
                        break;
                    case 11:
                        Console.Write("Enter email: ");
                        string email11 = Console.ReadLine();
                        Console.Write("Enter column id: ");
                        int columnid11 = int.Parse(Console.ReadLine());
                        Console.Write("Enter task id: ");
                        int taskid11 = int.Parse(Console.ReadLine());
                        Response newtask11 = service.AdvanceTask(email11, columnid11, taskid11);
                        if (newtask11.ErrorOccured)
                        {
                            Console.WriteLine(newtask11.ErrorMessage);
                        }
                        break;
                    case 12:
                        Console.Write("Enter email: ");
                        string email12 = Console.ReadLine();
                        Console.Write("Enter column name: ");
                        string columnid12 = Console.ReadLine();
                        Response<Column> column12 = service.GetColumn(email12, columnid12);
                        if (column12.ErrorOccured)
                        {
                            Console.WriteLine(column12.ErrorMessage);
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("The column's name: " + column12.Value.Name);
                            Console.WriteLine("The column's limit: " + column12.Value.Limit);
                            if (column12.Value.Tasks.Count > 0)
                            {
                                Console.WriteLine("The column's tasks: ");
                                Console.WriteLine();
                                foreach (Task task in column12.Value.Tasks)
                                {
                                    Console.Write("*");
                                    Console.WriteLine("The task's id: " + task.Id);
                                    Console.WriteLine("The task's title: " + task.Title);
                                    Console.WriteLine("The task's creationtime: " + task.CreationTime);
                                    Console.WriteLine("The task's description: " + task.Description);
                                    Console.WriteLine("The task's duedate: " + task.DueDate);
                                    Console.WriteLine();
                                    Console.WriteLine();
                                }
                            }
                            else
                            {
                                Console.WriteLine("There is no tasks.");
                            }
                        }
                        break;
                    case 13:
                        Console.Write("Enter email: ");
                        string email13 = Console.ReadLine();
                        Console.Write("Enter column's id: ");
                        int columnid13 = int.Parse(Console.ReadLine());
                        Response<Column> column13 = service.GetColumn(email13, columnid13);
                        if (column13.ErrorOccured)
                        {
                            Console.WriteLine(column13.ErrorMessage);
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("The column's name: " + column13.Value.Name);
                            Console.WriteLine("The column's limit: " + column13.Value.Limit);
                            if (column13.Value.Tasks.Count > 0)
                            {
                                Console.WriteLine("The column's tasks: ");
                                Console.WriteLine();
                                foreach (Task task in column13.Value.Tasks)
                                {
                                    Console.Write("*");
                                    Console.WriteLine("The task's id: " + task.Id);
                                    Console.WriteLine("The task's title: " + task.Title);
                                    Console.WriteLine("The task's creationtime: " + task.CreationTime);
                                    Console.WriteLine("The task's description: " + task.Description);
                                    Console.WriteLine("The task's duedate: " + task.DueDate);
                                    Console.WriteLine();
                                    Console.WriteLine();
                                }
                            }
                            else
                            {
                                Console.WriteLine("There is no tasks.");
                            }
                        }
                        break;
                    case 14:
                        terminate = true;
                        break;
                }
                Console.WriteLine();
            }
        }
    }
}