using System.Data;
using System.Net.Http.Json;

namespace Maga_Zin
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                User user;
                List<string> authData = new List<string>() { "", "" };
                do
                {
                    Menu menu = new Menu(new List<Button> { new Button("Логин", payload: authData[0]), new Button("Пароль", payload: authData[1], hide: true), new Button("Авторизоваться", "submit") }, "Авторизация", true);
                    authData = (List<string>)menu.Select();
                    Console.Clear();
                    user = Auth.LogIn(authData[0], authData[1]);
                }
                while (user == null);
                switch (user.role)
                {
                    case 5:
                        user.Methods = new Admin(user);
                        break;
                    case 4:
                        user.Methods = new Cashier(user);
                        break;
                    case 3:
                        user.Methods = new PersonManager(user);
                        break;
                    case 2:
                        user.Methods = new SkladManager(user);
                        break;
                    case 1:
                        user.Methods = new Booker(user);
                        break;
                }
            }
        }
    }
}