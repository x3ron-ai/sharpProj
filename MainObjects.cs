using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maga_Zin
{

    public class Initializator
    {
        public string rootPath;
        public string projectPath;
        public string usersFile;
        public string productsFile;
        public string workersFile;
        public string cashFile;
        public Initializator()
        {
            rootPath = String.Join('\\', System.Environment.ProcessPath.Split('\\')[0..2]);
            projectPath = rootPath + $"\\{Environment.UserName}\\Desktop\\GodAmn_shop\\";
            usersFile = projectPath + "Users.json";
            workersFile = projectPath + "Employee.json";
            productsFile = projectPath + "Products.json";
            cashFile = projectPath + "Cash.json";
            if (!Directory.Exists(projectPath))
            {

                Directory.CreateDirectory(projectPath);
                File.Create(usersFile).Close();
                File.Create(projectPath + "AuthData.json").Close();
                File.Create(projectPath + "Products.json").Close();
                string newEmployee = JsonConvert.SerializeObject(new List<Worker>
                    {
                        new Worker(0, "Admin", "Adminov", "Adminovych", "24.01.2006", "1234 123456", "Administrator", 677, 0)
                    });
                File.WriteAllText(projectPath + "Employee.json", newEmployee);
                string newUsers = JsonConvert.SerializeObject(new List<User>
                    {
                        new User("ROOT", "root", 5, 0)
                    });
                File.WriteAllText(usersFile, newUsers);
            }
        }
        
    }
    class Cash
    {
        public int value;
        public int name;
    }
    public class User
    {
        public int id;
        public string login;
        public string password;
        public int role;
        public string roleName;
        public int linkedWorkerId;
        public Worker linkedWorker;
        public object Methods;
        public List<string> searchArgs;
        public User(string login, string password, int role, int id, Worker linkedWorker = null, int linkedWorkerId = -1)
        {
            this.login = login;
            this.password = password;
            this.role = role;
            this.id = id;
            this.linkedWorker = linkedWorker;
            if (linkedWorkerId != -1)
            {
                this.linkedWorkerId = linkedWorkerId;
                foreach (Worker worker in Cfgs.GetWorkers())
                {
                    if (linkedWorkerId == worker.id)
                        this.linkedWorker = worker;
                }
            }
            switch (role)
            {
                case 5:
                    this.roleName = "Администратор";
                    break;
                case 4:
                    this.roleName = "Кассир";
                    break;
                case 3:
                    this.roleName = "Кадровик";
                    break;
                case 2:
                    this.roleName = "Склад-менеджер";
                    break;
                case 1:
                    this.roleName = "Бухгалтер";
                    break;
            }
            this.searchArgs = new List<string>()
            {
                $"{id}", login, password, roleName.ToLower(), 
            };
        }
    }
    public class Worker
    {
        public int id;
        public string name;
        public string surname;
        public string nameNoPoBatyushke;
        public string birthdate;
        public string passport;
        public string job;
        public float salary;
        public int userAccountId;
        public Worker(int id, string name, string surname, string nameNoPoBatyushke, string birthdate, string passport, string job, float salary, int userAccountId)
        {
            this.id = id;
            this.name = name;
            this.surname = surname;
            this.nameNoPoBatyushke = nameNoPoBatyushke;
            this.birthdate = birthdate;
            this.passport = passport;
            this.job = job;
            this.salary = salary;
            this.userAccountId = userAccountId;
        }
    }
    public class Product
    {
        public int id;
        public string name;
        public int price;
        public int count;
        public Product(int id, string name, int price, int count)
        {
            this.id = id;
            this.name = name;
            this.price = price;
            this.count = count;
        }
    }
}
