using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Maga_Zin
{
    public class Cfgs
    {
        public static object Deserialize(string path, Type obj)
        {
            object vv = JsonConvert.DeserializeObject(File.ReadAllText(path), obj);
            return vv;
        }
        public static void Serialize(string path, object obj)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(obj));
        }
        public static void ShowGreeting(User user)
        {
            int w = Console.WindowWidth;
            string greeting;
            if (user.linkedWorker != null)
            {
                greeting = $"Добро пожаловать, {user.linkedWorker.name}         Роль: {user.roleName}";
            }
            else
            {
                greeting = $"Добро пожаловать, {user.login}         Роль: {user.roleName}";
            }
            Console.SetCursorPosition(w / 2 - greeting.Count() / 2, 0);
            Console.WriteLine(greeting);
        }
        public static Worker GetWorker(int userId)
        {
            List<Worker> users = (List<Worker>)GetWorkers();
            foreach (Worker elem in users)
            {
                if (elem.id == userId)
                    return elem;
            }
            return null;
        }
        public static List<Worker> GetWorkers()
        {
            Initializator init = new Initializator();
            List<Worker> users = new List<Worker>();
            users = (List<Worker>)Deserialize(init.workersFile, typeof(List<Worker>));
            return users;
        }
        public static User GetUser(int userId)
        {
            List<User> users = (List<User>)GetUsers();
            foreach (User elem in users)
            {
                if (elem.id == userId)
                    return elem;
            }
            return null;
        }
        public static List<User> GetUsers()
        {
            Initializator init = new Initializator();
            List<User> users = new List<User>();
            users = (List<User>)Deserialize(init.usersFile, typeof(List<User>));
            return users;
        }
    }
    public class CRUD
    {
        public object Search(int elem, string value, Type obj)
        {
            object resp = new List<object>();

            if (typeof(User) == obj)
            {
                List<User> finded = new List<User>();
                foreach (User user in Cfgs.GetUsers())
                {
                    if (user.searchArgs[elem] == value)
                    {
                        finded.Add(user);
                    }
                }
                resp = finded;
            }
            return resp;
        }
        public List<string> Create(Type obj)
        {
            if (obj == typeof(User))
            {
                List<Button> buttons = new List<Button>()
                {
                    new Button("ID", type: "int"),
                    new Button("Логин"),
                    new Button("Пароль"),
                    new Button("ID Роли", type: "int"),
                    new Button("ID аккаунта рабочего", type: "int"),
                    new Button("Принять", action: "submit")
                };
                Menu menu = new Menu(buttons, "Создание пользователя", editing: true, keyButtons: new List<KeyButton>() {new KeyButton(ConsoleKey.Escape, "escape")}, instructions: "ESC для отмены");
                List<string> values = new List<string>();
                while (true)
                    try
                    {
                        values = (List<string>)menu.Select();
                        Convert.ToInt32(values[0]);
                        Convert.ToInt32(values[3]);
                        if (values[4] != "")
                            Convert.ToInt32(values[4]);
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (ex.GetType() == typeof(System.InvalidCastException))
                            return new List<string>();
                        menu.title = "Ошибка ввода данных. Попробуйте снова";
                        Console.Clear();
                    }
                return values;
            }
            else
            {
                return new List<string>();
            }
        }
        public void Read(object value, Type obj)
        {
            if (typeof(User) == obj)
            {
                User user = (User)value;
                Console.Clear();
                Worker linked = user.linkedWorker;
                string linkedId = null;
                if (linked != null)
                    linkedId = $"{linked.id}";
                string text = $"ID: {user.id}\nЛогин: {user.login}\nПароль: {user.password}\nИмя роли: {user.roleName}\nID связанного аккаунта: {linkedId}";
                Console.WriteLine(text);
                Console.WriteLine("\nEnter для продолжения");
                Console.ReadKey();
                Console.Clear();
            }
        }
        public List<string> Update(object value, Type obj)
        {
            if (typeof(User) == obj)
            {
                User user = (User)value;
                List<Button> buttons = new List<Button>()
                {
                    new Button("ID", payload: $"{user.id}", type: "int"),
                    new Button("Логин", payload: user.login),
                    new Button("Пароль", payload: user.password),
                    new Button("ID Роли", payload: $"{user.role}", type: "int"),
                    new Button("ID аккаунта рабочего", payload: $"{user.linkedWorkerId}", type: "int"),
                    new Button("Принять", action: "submit"),
                };
                Menu menu = new Menu(buttons, "Изменение пользователя", editing: true, keyButtons: new List<KeyButton>() { new KeyButton(ConsoleKey.Escape, "escape") }, instructions: "ESC для отмены");

                List<string> values = new List<string>();
                while (true)
                    try
                    {
                        Console.Clear();
                        values = (List<string>)menu.Select();
                        Convert.ToInt32(values[0]);
                        Convert.ToInt32(values[3]);
                        Convert.ToInt32(values[4]);
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (ex.GetType() == typeof(System.InvalidCastException))
                            return new List<string>();
                        menu.title = "Ошибка ввода данных. Попробуйте снова";
                        Console.Clear();
                    }
                return values;
            }
            else
            {
                return new List<string>();
            }
        }
    }
    public class Admin : CRUD
    {
        public Admin(User user)
        {
            while (true)
            {
                int w = Console.WindowWidth;
                Console.Clear();
                Cfgs.ShowGreeting(user);
                Console.SetCursorPosition(0, 1);
                for (int i = 0; i < w; i++)
                    Console.Write('=');
                List<User> users = Cfgs.GetUsers();
                List<Button> buttons = new List<Button>();
                foreach (User elem in users)
                {
                    string label = "";
                    label += $"{elem.id} \t{elem.login}\t\t{elem.password}\t\t{elem.roleName}";
                    buttons.Add(new Button(label, data: $"{elem.id}"));
                }
                List<KeyButton> keyButtons = new List<KeyButton>()
                {
                    new KeyButton(ConsoleKey.C, "create"),
                    new KeyButton(ConsoleKey.Enter, "read"),
                    new KeyButton(ConsoleKey.U, "update"),
                    new KeyButton(ConsoleKey.D, "delete"),
                    new KeyButton(ConsoleKey.S, "search"),
                    new KeyButton(ConsoleKey.Escape, "escape")
                };

                Menu menu = new Menu(buttons, "ID\tLogin\t\tPassword\t\tRole", keyButtons: keyButtons, instructions: "C - Create\nEnter - Read\nU - Update\nD - Delete\nS - Search\nESC - выход");
                var menuAnswer = menu.Select(true);
                if (menuAnswer.GetType() == typeof(KeyButton))
                {
                    KeyButton keyButton = (KeyButton)menuAnswer;

                    if (keyButton.action == "escape")
                    {
                        break;
                    }
                    else if (keyButton.action == "create")
                    {
                        List<string> values = new List<string>()
                        {
                            "ID",
                            "Login",
                            "Password",
                            "Role"
                        };
                        List<string> newValues = Create(typeof(User));
                        int lwid;
                        try
                        {
                            lwid = Convert.ToInt32(newValues[4]);
                        }
                        catch
                        {
                            lwid = -1;
                        }
                        try
                        {
                            User newUserObject = new User(newValues[1], newValues[2], Convert.ToInt32(newValues[3]), Convert.ToInt32(newValues[0]), linkedWorkerId: lwid);
                            users.Add(newUserObject);
                            Cfgs.Serialize(new Initializator().usersFile, users);
                        }
                        catch
                        {
                            Console.WriteLine( "skipped");
                        }
                        
                    }
                    else if (keyButton.action == "search")
                    {
                        menu = new Menu(new List<Button>()
                        {
                            new Button("ID"),
                            new Button("Login"),
                            new Button("Password"),
                            new Button("Название роли")
                        },  "Выбор пункта для поиска");
                        int val = (int)menu.Select();
                        Console.Clear();
                        Console.Write("Ввод значения для поиска: ");
                        string searching = Console.ReadLine();
                        List<User> finded = (List<User>)Search(val, searching, typeof(User));
                        Console.WriteLine("ID\tLogin\t\tPassword\t\tRole");
                        foreach (User elem in finded)
                        {
                            Console.WriteLine($"{elem.id} \t{elem.login}\t\t{elem.password}\t\t{elem.roleName}");
                        }
                        Console.WriteLine("Enter чтобы продолжить");
                        Console.ReadKey(true);
                    }
                    else if (keyButton.action == "read")
                    {
                        Read(users[(int)keyButton.element], typeof(User));
                    }
                    else if (keyButton.action == "update")
                    {
                        int userId = (int)keyButton.element;
                        List<string> newValues = Update(users[userId], typeof(User));
                        try
                        {
                            User newUserObject = new User(newValues[1], newValues[2], Convert.ToInt32(newValues[3]), Convert.ToInt32(newValues[0]), linkedWorkerId: Convert.ToInt32(newValues[4]));
                            users.RemoveAt(userId);
                            users.Add(newUserObject);
                            Cfgs.Serialize(new Initializator().usersFile, users);
                        }
                        catch
                        {
                            Console.WriteLine("skipped");
                        }
                    }
                    else if (keyButton.action == "delete")
                    {
                        int userId = (int)keyButton.element;

                        users.RemoveAt(userId);
                        Cfgs.Serialize(new Initializator().usersFile, users);
                    }
                }
            }
        }
    }
    public class Cashier
    {
        public Cashier(User user)
        {

        }

    }
    public class SkladManager
    {
        public SkladManager(User user)
        {

        }
    }
    public class PersonManager
    {
        public PersonManager(User user)
        {

        }

    }
    public class Booker
    {
        public Booker(User user)
        {

        }

    }
}