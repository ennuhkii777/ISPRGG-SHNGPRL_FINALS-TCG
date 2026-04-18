using System;
using System.Collections.Generic;
using System.IO;

namespace ISPRGG_SHNGPRL_FINALS_TCG
{
    class Program
    {
        static List<BaseCard> allCards = new List<BaseCard>();
        static List<BaseCard> originalCards = new List<BaseCard>();
        static Dictionary<int, int> binderPullCount = new Dictionary<int, int>();
        static Random rng = new Random();

        static void Main(string[] args)
        {
            allCards = FileLoader.LoadCards("cards.txt");
            originalCards = FileLoader.LoadCards("cards.txt");
            LoadPulls();

            while (true)
            {
                Console.WriteLine("\n=== TRADING CARD GAME ===");
                Console.WriteLine("[1] Pull Cards");
                Console.WriteLine("[2] Battle");
                Console.WriteLine("[3] Display Binder");
                Console.WriteLine("[4] Exit");
                Console.Write("Choose: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": PullCards(); break;
                    case "2": Battle(); break;
                    case "3": DisplayBinderSystem(); break;
                    case "4": return;
                    default: Console.WriteLine("Invalid option."); break;
                }
            }
        }

        static void PullCards()
        {
            Console.WriteLine("\n--- PULLING 5 CARDS ---\n");

            for (int i = 0; i < 5; i++)
            {
                BaseCard pulled = allCards[rng.Next(allCards.Count)];
                int index = GetNationalIndex(pulled.Name);

                if (!binderPullCount.ContainsKey(index))
                    binderPullCount[index] = 0;

                binderPullCount[index]++;
                pulled.PrintCard();
                Console.WriteLine();
            }

            SavePulls();
            Console.WriteLine("\n--- BINDER UPDATED ---");
            DisplayBinder();
        }

        static void DisplayBinder()
        {
            string border = "+------+------+------+------+------+";
            Console.WriteLine("\n" + border);
            Console.Write("|");

            for (int i = 1; i <= 15; i++)
            {
                string display = binderPullCount.ContainsKey(i) && binderPullCount[i] > 0
                    ? CenterText(((NationalIndex)i).ToString().ToUpper(), 6)
                    : CenterText("#" + i.ToString("D3"), 6);

                Console.Write(display + "|");

                if (i % 5 == 0)
                {
                    Console.WriteLine();
                    Console.WriteLine(border);
                    if (i < 15) Console.Write("|");
                }
            }
        }

        static string CenterText(string text, int width)
        {
            if (text.Length >= width) return text.Substring(0, width);
            int pad = width - text.Length;
            return new string(' ', pad / 2) + text + new string(' ', pad - pad / 2);
        }

        static void DisplayBinderSystem()
        {
            while (true)
            {
                DisplayBinder();
                Console.Write("\nEnter 1-15 or 0 to exit: ");

                if (!int.TryParse(Console.ReadLine(), out int choice)) continue;
                if (choice == 0) return;

                if (choice < 1 || choice > 15)
                {
                    Console.WriteLine("Invalid input! Please enter a number from 1 to 15.");
                    continue;
                }

                if (!binderPullCount.ContainsKey(choice) || binderPullCount[choice] == 0)
                {
                    Console.WriteLine($"Card #{choice} is not yet unlocked!");
                    continue;
                }

                NationalIndex ni = (NationalIndex)choice;
                BaseCard found = originalCards.Find(c =>
                    c.Name.Equals(ni.ToString(), StringComparison.OrdinalIgnoreCase));

                if (found != null)
                    CardPrinter.PrintWithCount(found, binderPullCount[choice]);
            }
        }

        static void Battle()
        {
            bool hasCards = false;
            foreach (var kvp in binderPullCount)
            {
                if (kvp.Value > 0) { hasCards = true; break; }
            }
            if (!hasCards)
            {
                Console.WriteLine("\nYou have no cards yet! Pull some cards first.");
                return;
            }

            BaseCard userCard = SelectCard();
            BaseCard aiCard = GenerateAI();

            do
            {
                BattleLogic.ProgramBattleRestart = false;
                BattleLogic.RunBattle(
                    userCard,
                    aiCard,
                    allCards,
                    originalCards,
                    binderPullCount,
                    rng
                );

                if (BattleLogic.ProgramBattleRestart)
                {
                    userCard = SelectCard();
                    aiCard = GenerateAI();
                }

            } while (BattleLogic.ProgramBattleRestart);
        }

        static BaseCard SelectCard()
        {
            Console.WriteLine("\n--- SELECT YOUR CARD ---");
            BaseCard userCard = null;

            while (userCard == null)
            {
                DisplayBinder();
                Console.Write("Enter card number (1-15): ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Invalid input! Enter a number from 1 to 15.");
                    continue;
                }

                if (choice < 1 || choice > 15)
                {
                    Console.WriteLine("Invalid input! Enter a number from 1 to 15.");
                    continue;
                }

                if (!binderPullCount.ContainsKey(choice) || binderPullCount[choice] == 0)
                {
                    Console.WriteLine($"Card #{choice} is not yet unlocked! Choose another.");
                    continue;
                }

                NationalIndex ni = (NationalIndex)choice;
                userCard = originalCards.Find(c =>
                    c.Name.Equals(ni.ToString(), StringComparison.OrdinalIgnoreCase));
            }

            userCard.HP = GetOriginalHP(userCard.Name);
            Console.WriteLine("\nYou selected:");
            userCard.PrintCard();
            return userCard;
        }

        static BaseCard GenerateAI()
        {
            BaseCard ai = allCards[rng.Next(allCards.Count)];
            ai.HP = GetOriginalHP(ai.Name);
            Console.WriteLine("\nAI selected:");
            ai.PrintCard();
            return ai;
        }

        static int GetOriginalHP(string name)
        {
            var c = originalCards.Find(x =>
                x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            return c != null ? c.HP : 0;
        }

        static int GetNationalIndex(string name)
        {
            foreach (NationalIndex ni in Enum.GetValues(typeof(NationalIndex)))
            {
                if (ni.ToString().Equals(name, StringComparison.OrdinalIgnoreCase))
                    return (int)ni;
            }
            return 0;
        }

        static void SavePulls()
        {
            List<string> lines = new List<string>();
            for (int i = 1; i <= 15; i++)
            {
                int count = binderPullCount.ContainsKey(i) ? binderPullCount[i] : 0;
                lines.Add(i + "," + count);
            }
            File.WriteAllLines("pulls.txt", lines);
        }

        static void LoadPulls()
        {
            if (!File.Exists("pulls.txt")) return;

            foreach (var line in File.ReadAllLines("pulls.txt"))
            {
                var parts = line.Split(',');
                if (parts.Length == 2)
                    binderPullCount[int.Parse(parts[0])] = int.Parse(parts[1]);
            }
        }
    }
}
