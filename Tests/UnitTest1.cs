using NUnit.Framework;
using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.BusinessLayer.BoardPackage;
using IntroSE.Kanban.Backend.BusinessLayer.UserPackage;
using System;

namespace Tests
{
    public class Tests
    {
        private Service service = new Service();
        private Service AfterLoadService = new Service();
        private Response reg;
        private Response<IntroSE.Kanban.Backend.ServiceLayer.User> reg1;

        [SetUp]
        public void Setup()
        {
            reg = new Response();
        }
        
        [TestCase("test1@gmail.com", "aA123456", "Test1","")]
        [TestCase("test2@gmail.com", "aA123456", "Test2","")]
        [TestCase("test3@gmail.com", "aA123456", "Test3","")]
        [TestCase("test4@gmail.com", "aA123456", "Test4", "test1@gmail.com")]
        [TestCase("test5@gmail.com", "aA123456", "Test5", "test2@gmail.com")]
        [TestCase("test6@gmail.com", "aA123456", "Test6", "test3@gmail.com")]
        public void Test1(string email,string password, string nickname,string emailHost)//Registers that should success
        {
            string host = "";
            if (emailHost.Equals(""))
                reg = service.Register(email, password, nickname);
            else
            {
                reg = service.Register(email, password, nickname, emailHost);
                host = $" to emailHost:{emailHost}\n";
            }
            Assert.AreEqual(false,reg.ErrorOccured,$"failed to register with details :\n email : {email} \n password : {password} \n nickname: {nickname} \n {host} reason :{reg.ErrorMessage}");
        }

        [TestCase("test1@gmail.com", "aA123456", "Test1","", "register succeed although test1@gmail.com allready exists in the system")]
        [TestCase("test1gmail.com", "aA123456", "Test2", "", "register succeed althought email does not contain @")]
        [TestCase("test1gmailcom", "aA123456", "Test2", "", "register succeed althought email does not contain '.'")]
        [TestCase("test1@gmail.", "aA123456", "Test2", "", "register succeed althought email contains . in the last char")]
        [TestCase("test1gmail.com@", "aA123456", "Test2", "", "register succeed althought email contains @ in the last char")]
        [TestCase("test1gmail.com", "aA123456", "Test2", "test10@gmail.com", "register succeed althought there is no host with the email test10@gmail.com")]//no exists email host
        public void Test2(string email,string password,string nickname,string emailHost ,string err)//registers that should fail
        {         
            if (emailHost.Equals(""))
                reg = service.Register(email, password, nickname);
            else
                reg = service.Register(email, password, nickname, emailHost);
            Assert.AreEqual(true,reg.ErrorOccured,err);
        }
        [TestCase("test1@gmail.com","a123456",false,"login succeed with wrong password succeed althought it should fail")]
        [TestCase("test1@gmail.com", "aA123456", true,"failed to log in with correct details")]
        [TestCase("test1@gmail.com", "aA123456", false,"succeed to log in with logged in user")]
        [TestCase("test2@gmail.com", "aA123456", false,"succeed log in while another user is online")]
        public void Test3(string email,string password,bool status,string err)// try to log in
        {
            Assert.AreEqual(status, !service.Login(email, password).ErrorOccured, err);       
        }

        [TestCase("test2@gmail.com",true,"succeed logout with non online user")]
        [TestCase("test1@gmail.com",false,"failed to logout with online user")]
        [TestCase("test1@gmail.com", true, "succeed to logout after logout")]
        public void Test4(string email,bool status,string err)//try to logout
        {
            Assert.AreEqual(status, service.Logout(email).ErrorOccured, err);
        }

        [TestCase("test1@gmail.com", "aA123456", true, "failed to log in with correct details")]
        public void Test5(string email, string password, bool status, string err)// log in again
        {
            Assert.AreEqual(status, !service.Login(email, password).ErrorOccured, err);
        }

        [TestCase("test1@gmail.com", "Title1", "Des1", "12.12.22", false, "failed to add valid task")]
        [TestCase("test1@gmail.com", "Title2", "Des2", "12.12.22", false, "failed to add valid task")]
        [TestCase("test1@gmail.com", "Title3", "Des3", "12.12.22", false, "failed to add valid task")]
        [TestCase("test1@gmail.com", "Title4", "Des4", "12.12.22", false, "failed to add valid task")]
        [TestCase("test1@gmail.com", "Title5", "Des5", "12.12.22", false, "failed to add valid task")]
        [TestCase("test1@gmail.com", "Title6", "Des6", "12.12.22", false, "failed to add valid task")]
        [TestCase("test1@gmail.com", "Title7", "", "12.12.22", false, "failed to add valid task")]
        [TestCase("test2@gmail.com", "Title1", "Des1", "12.12.22", true, "succed to add task from non online user")]
        [TestCase("test80@gmail.com", "Title1", "Des1", "12.12.22", true, "succed to add task from non exists user")]
        [TestCase("test1@gmail.com", "", "Des1", "12.12.22", true, "succed to add task with empty title")]
        [TestCase("test1@gmail.com", "Title1", "dadsad", "31.12.10", true, "succed to add task with invalid duedate")]
        public void Test6(string email,string title,string description,string dueDate,bool status,string err)//add tasks
        {
            Assert.AreEqual(status, service.AddTask(email,title,description,DateTime.Parse(dueDate)).ErrorOccured, err);
        }

        [TestCase("test1@gmail.com",0,0, "12.12.22", false, "failed to update valid task duedate")]
        [TestCase("test1@gmail.com", 0, 6, "12.12.22", false, "failed to update valid task duedate")]
        [TestCase("test1@gmail.com", 0, 7, "12.12.22", true, "succeed to update dueDate to non exists task")]
        [TestCase("test1@gmail.com", -1, 0, "12.12.22", true, "succeed to update dueDate to invalid colOrdinal")]
        [TestCase("test1@gmail.com", 0, -1, "12.12.22", true, "succeed to update dueDate to invalid taskID")]
        [TestCase("test5@gmail.com", 0, 0, "12.12.22", true, "succeed to update dueDate with not asignd email")]
        //add more tasks with another online user
        public void Test7(string email,int columnOrdinal,int taskId,string dueDate,bool status,string err)//update Task DueDate
        {
            Assert.AreEqual(status, service.UpdateTaskDueDate(email,columnOrdinal,taskId, DateTime.Parse(dueDate)).ErrorOccured, err);
        }

        [TestCase("test1@gmail.com", 0, 0, "Title1", false, "failed to update valid task duedate")]
        [TestCase("test1@gmail.com", 0, 6, "Title7", false, "failed to update valid task duedate")]
        [TestCase("test1@gmail.com", 0, 7, "Title1", true, "succeed to update dueDate to non exists task")]
        [TestCase("test1@gmail.com", -1, 0, "Title1", true, "succeed to update dueDate to invalid colOrdinal")]
        [TestCase("test1@gmail.com", 0, -1, "Title1", true, "succeed to update dueDate to invalid taskID")]
        [TestCase("test5@gmail.com", 0, 0, "Title1", true, "succeed to update dueDate with not asignd email")]
        public void Test8(string email, int columnOrdinal, int taskId, string title, bool status, string err)//update task Title
        {
            Assert.AreEqual(status, service.UpdateTaskTitle(email, columnOrdinal, taskId, title).ErrorOccured, err);
        }

        [Test]
        public void Test99999()//last test should delete all data and succes
        {
            service.DeleteData();
        }
    }
}