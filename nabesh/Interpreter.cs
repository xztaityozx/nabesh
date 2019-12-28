using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace nabesh {
    public class Interpreter :IDisposable {
        private readonly NabeContainer container;
        private const string prompt = "[nabe]>> ";
        private readonly int offset = prompt.Length;
        private Process process;

        public Interpreter() {
            container = new NabeContainer();
            Console.Clear();
            top = 0;
            WritePrompt();

            ResetBuffer();
        }

        private void ResetBuffer() {
            buffer = "";
            index = 0;
        }

        private void WritePrompt() {
            Console.SetCursorPosition(0, top);
            Console.WriteLine(prompt);
            Console.SetCursorPosition(offset, top);
        }

        private int top;
        private string buffer;
        private void Flush() {
            Console.SetCursorPosition(offset, top);
            Console.WriteLine(string.Join("", Enumerable.Repeat(' ', Console.BufferWidth-offset)));
            Console.SetCursorPosition(offset, top);
            container.Parse(buffer);
            var o = container.Nabelize();
            Console.WriteLine(o);
            var size = 0;
            for (var item = StringInfo.GetTextElementEnumerator(o); item.MoveNext();) size++;
            Console.SetCursorPosition(size*4-2 + offset, top);
        }

        public void Start() {
            Read();
        }

        private void Action() {
            Console.SetCursorPosition(0, top+1);
            process = new Process {
                StartInfo = new ProcessStartInfo {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    FileName = "bash",
                    ArgumentList = { "-c", buffer},
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };
            if(!process.Start()) throw new Exception();
            var o = process.StandardOutput.ReadToEnd();
            var e = process.StandardError.ReadToEnd();
            if (string.IsNullOrEmpty(o)) o = e;

            process.WaitForExit();

            container.Parse(o);
            Console.WriteLine(container.Nabelize());
            top++;
            top += o.Count(s => s == '\n');
            WritePrompt();
            ResetBuffer();
        }

        private int index;
        private void Remove() {
            buffer = buffer.Length == 0 ? "" : buffer.Remove(index-1, 1);
            index = Math.Max(0, index - 1);
        }

        private void Home() => index = offset;
        private void End() => index = buffer.Length - 1;

        private void Insert(ConsoleKeyInfo keyInfo) {
            buffer = buffer.Insert(index, $"{keyInfo.KeyChar}");
            index++;
        }

        private void Read() {
            while (true) {
                var keyInfo = Console.ReadKey(true);
                var key = keyInfo.Key;

                switch (key) {
                    case ConsoleKey.Enter:
                        Action();
                        break;
                    case ConsoleKey.Backspace:
                        Remove();
                        Flush();
                        break;
                    case ConsoleKey.LeftArrow:
                        index = Math.Max(index - 1, 0);
                        break;
                    case ConsoleKey.RightArrow:
                        index = Math.Min(index+1, buffer.Length);
                        break;
                    case ConsoleKey.UpArrow:
                        break;
                    case ConsoleKey.DownArrow:
                        break;
                    case ConsoleKey.Home:
                        Home();
                        break;
                    case ConsoleKey.End:
                        End();
                        break;
                    default:
                        Insert(keyInfo);
                        Flush();
                        break;
                }
            }
        }

        public void Dispose() {
            process?.Dispose();
        }
    }
}