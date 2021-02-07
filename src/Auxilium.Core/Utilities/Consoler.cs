using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;

namespace Auxilium.Core.Utilities
{

	public static class Consoler
    {
        public const string Breaker = "---------------------------------------------";
		public static void ShowHeader(string text, string about = null)
		{
            var col = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.DarkCyan;
			Console.WriteLine(Breaker);
			Console.WriteLine(Breaker);
			Console.WriteLine("");
			Console.WriteLine(text);
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine("");

			if (!string.IsNullOrEmpty(about))
			{
				Console.WriteLine("");
				Console.WriteLine(about);
				Console.WriteLine("");
			}

			Console.ForegroundColor = ConsoleColor.DarkCyan;
			Console.WriteLine(Breaker);
			Console.WriteLine(Breaker);
			Console.WriteLine("");
            Console.ForegroundColor = col;
		}

		public static void Information(string text)
        {
            var col = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine(Breaker);
			Console.WriteLine(text);
			Console.WriteLine(Breaker);
            Console.ForegroundColor = col;
        }

		public static void TitleStart(string text = null)
		{
            var col = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine("---- start ----");
			if (!string.IsNullOrEmpty(text))Console.WriteLine(text);
            Console.ForegroundColor = col;
		}

		public static void TitleEnd(string text = null)
		{
            var col = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.White;
            if (!string.IsNullOrEmpty(text)) Console.WriteLine(text);
			Console.WriteLine("---- end ----");
            Console.ForegroundColor = col;
		}

		public static void Write(string text, bool newLine=true)
		{
            if (newLine)
            {
                Console.WriteLine(text);
                Trace.WriteLine(text);
			}
            else
            {
				Console.Write(text);
				Trace.Write(text);
            }
        }

		public static void Write(string format, object arg)
		{
            var col = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine(format, arg);
            Console.ForegroundColor = col;
        }

		public static void Warn(string text, string message = null)
		{
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine("-- WARN --");
			Console.WriteLine(text);
			if (!string.IsNullOrEmpty(message)) Write(message);
		}

		public static void Error(string text)
		{
            var col = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.DarkRed;
			Console.WriteLine("-- ERROR --");
			Console.WriteLine(text);
            Console.ForegroundColor = col;
		}

		public static void Error(string text, Exception ex)
		{
            var col = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.DarkRed;
			Console.WriteLine("-- ERROR --");
			Console.WriteLine(text);
			Console.WriteLine("-- EXCEPTION --");
			Console.WriteLine(ex.ToString());
            Console.ForegroundColor = col;
		}

		public static void Success(bool prompt = false)
		{
            var col = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(Breaker);
			Console.WriteLine("Success");
            Console.ForegroundColor = col;
			Pause(prompt);
		}

		public static void ShowError(Exception e, bool prompt = false)
		{
            var col = Console.ForegroundColor; 
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(Breaker);
			Console.WriteLine("Exception");
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine("");
			Console.WriteLine(e.Message);
			Console.WriteLine("");
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine(e);
            Console.ForegroundColor = col;
			Pause(prompt);
		}


		public static void Pause(bool prompt = false)
		{
            Console.ForegroundColor = ConsoleColor.White;
			Write("");
			if (prompt)
			{
				Write("\nPress any key to exit.");
				Console.ReadLine();
			}
		}
	}

}
