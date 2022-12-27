using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maga_Zin
{
    public class KeyButton
    {
        public ConsoleKey key;
        public string action;
        public object element;
        public KeyButton(ConsoleKey key, string action)
        {
            this.key = key;
            this.action = action;
        }
    }
    public class Button
    {
        public int lenght;
        public string label;
        public string payload;
        public string action;
        public bool hide;
        public string data;
        public Button(string label, string action = "", string payload = "", bool hide = false, string data = "", string type = "str")
        {
            this.lenght = label.Length;
            this.payload = payload;
            this.label = label;
            this.action = action;
            this.hide = hide;
            this.data = data;
        }
        public string ShowLabel()
        {
            string resp = $"{label}";
            if (payload != "")
                resp += $": {payload}";
            return resp;
        }
    }
    public class Menu
    {
        public List<Button> buttons;
        public List<KeyButton> keyButtons;
        public string title;
        int cursorMin;
        bool editing;
        string instructions;
        public Menu(List<Button> buttons, string title, bool editing = false, List<KeyButton> keyButtons = null, string instructions = null)
        {
            this.title = title;
            this.editing = editing;
            cursorMin = title.Split('\n').Length;
            this.buttons = buttons;
            this.instructions = instructions;
            if (keyButtons != null)
                this.keyButtons = keyButtons;
            else
                this.keyButtons = new List<KeyButton>();

        }
        public void UpdateMenu()
        {
            Console.Clear();
            Console.WriteLine(title);
            for (int i = 0; i < buttons.Count(); i++)
            {
                string label = buttons[i].ShowLabel();
                Console.WriteLine("   " + label);
            }
            Console.WriteLine("\n\n" + instructions);
        }
        public static string GetPassword()
        {
            string text = "";

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (text.Length == 0)
                        continue;
                    text = text.Remove(text.Length - 1);
                    Tuple<int, int> pos = Console.GetCursorPosition().ToTuple();
                    Console.SetCursorPosition(pos.Item1 - 1, pos.Item2);
                    Console.Write(" ");
                    Console.SetCursorPosition(pos.Item1 - 1, pos.Item2);
                }
                else
                {
                    text += key.KeyChar;
                    Console.Write("*");
                }
            }
            return text;
        }
        public void EditPayload(int num)
        {
            Console.SetCursorPosition(4 + buttons[num].label.Length, num + cursorMin);
            if (buttons[num].hide)
                buttons[num].payload = GetPassword();
            else
                buttons[num].payload = Console.ReadLine();
        }
        public object Select(bool gg = false)
        {
            Cursor cursor = new Cursor(cursorMin, buttons.Count, cursorMin);
            UpdateMenu();
            while (true)
            {
                Console.SetCursorPosition(0, cursor.pos);
                Console.Write(">>");
                ConsoleKey key = Console.ReadKey(true).Key;
                Button currentButton = buttons[cursor.pos - cursor.min];
                if (key == ConsoleKey.Enter && !gg)
                {
                    if (editing)
                    {
                        if (currentButton.action == "")
                            EditPayload(cursor.pos - cursor.min);
                        else if (currentButton.action == "submit")
                        {
                            List<string> resp = new List<string>();
                            foreach (Button but in buttons)
                                resp.Add(but.payload);
                            return resp;
                        }
                    }
                    else if (currentButton.action == "return")
                        return currentButton;
                    else
                        return cursor.pos - cursor.min;
                }

                else if (key == ConsoleKey.DownArrow)
                {
                    Console.SetCursorPosition(0, cursor.pos);
                    Console.Write("  ");
                    if (cursor.pos == cursor.max)
                        cursor.pos = cursor.min;
                    else
                        cursor.pos++;
                }

                else if (key == ConsoleKey.UpArrow)
                {
                    Console.SetCursorPosition(0, cursor.pos);
                    Console.Write("  ");
                    if (cursor.pos == cursor.min)
                        cursor.pos = cursor.max;
                    else
                        cursor.pos--;
                }

                foreach (KeyButton keyButton in keyButtons)
                {
                    if (key != keyButton.key)
                    {
                        continue;
                    }
                    else if (key == ConsoleKey.U)
                    {
                        keyButton.element = cursor.pos - cursor.min;
                        return keyButton;
                    }

                    else if (key == ConsoleKey.C)
                    {
                        keyButton.element = currentButton;
                        return keyButton;
                    }
                    else if (key == ConsoleKey.Enter)
                    {
                        keyButton.element = cursor.pos - cursor.min;
                        return keyButton;
                    }
                    else if (key == ConsoleKey.D)
                    {
                        keyButton.element = cursor.pos - cursor.min;
                        return keyButton;
                    }
                    else if (key == ConsoleKey.S)
                    {
                        return keyButton;
                    }
                    else if (key == ConsoleKey.Escape)
                    {
                        return keyButton;
                    }
                }
            }
            return new object();
        }
    }

    public class Cursor
    {
        public int pos;
        public int min;
        public int max;
        public Cursor(int pos, int max, int min)
        {
            this.pos = pos;
            this.min = min;
            this.max = max;
        }
    }
}