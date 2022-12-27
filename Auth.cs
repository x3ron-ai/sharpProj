using System;
using System.Collections.Generic;
using System.Linq; // bams?
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace Maga_Zin
{
    internal class Auth
    {
        public static User LogIn(string login, string passwd)
        {
            List<User> users = GetUsers();
            for (int i = 0;i < users.Count; i++)
            {
                User elem = users[i];
                if (elem.login == login && elem.password == passwd) {
                    User user = elem;
                    foreach (Worker worker in GetWorkers())
                        if (worker.userAccountId == user.id)
                            user.linkedWorker = worker;
                    return user;
                }

            }
            Console.WriteLine("Не существует такой пары логина и пароля");
            Thread.Sleep(000);
            return null;
        }
        public static void LogOut()
        {

        }
        private static List<Worker> GetWorkers()
        {
            Initializator init = new Initializator();
            List<Worker> workers = new List<Worker>();
            workers = JsonConvert.DeserializeObject<List<Worker>>(File.ReadAllText(init.workersFile));
            return workers;
        }
        private static List<User> GetUsers()
        {
            Initializator init = new Initializator();
            List<User> users = new List<User>();
            users = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(init.usersFile));
            return users;
        }
    }
}
