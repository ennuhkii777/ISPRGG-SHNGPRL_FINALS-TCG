using System;
using System.Collections.Generic;
using System.IO;

namespace ISPRGG_SHNGPRL_FINALS_TCG
{
    class Program
    {
        static List<BaseCard> allCards = new List<BaseCard>();
        static List<BaseCard> originalCards = new List<BaseCard>();
        static List<BaseCard> pulledCards = new List<BaseCard>();
        static Dictionary<int, int> binderPullCount = new Dictionary<int, int>();
        static Random rng = new Random();

        static void Main(string[] args)
        {
            allCards = FileLoader.LoadCards("cards.txt");
            originalCards = FileLoader.LoadCards("cards.txt");
            LoadPulls();

            bool running = true;
            while (running)
            {
                Console.WriteLine("\n=== TRADING CARD GAME ===");
                Console.WriteLine("[1] Pull Cards");
                Console.WriteLine("[2] Battle");
                Console.WriteLine("[3] Display Binder");
                Console.WriteLine("[4] Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        PullCards();
                        break;
                    case "2":
                        Battle();
                        break;
                    case "3":
                        DisplayBinderSystem();
                        break;
                    case "4":
                        running = false;
                        Console.WriteLine("Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }

        static void PullCards()
        {
            Console.WriteLine("\n--- PULLING 5 CARDS ---\n");

            List<BaseCard> newPulls = new List<BaseCard>();

            for (int i = 0; i < 5; i++)
            {
                int index = rng.Next(allCards.Count);
                BaseCard pulled = allCards[index];
                newPulls.Add(pulled);

                int nationalIndex = GetNationalIndex(pulled.Name);
                if (nationalIndex > 0)
                {
                    if (!binderPullCount.ContainsKey(nationalIndex))
                        binderPullCount[nationalIndex] = 0;
                    binderPullCount[nationalIndex]++;
                }

                pulled.PrintCard();
                Console.WriteLine();
            }

            pulledCards.AddRange(newPulls);
            SavePulls();

            Console.WriteLine("\n--- YOUR BINDER ---");
            DisplayBinder();
        }

        static int GetNationalIndex(string name)
        {
            foreach (NationalIndex ni in Enum.GetValues(typeof(NationalIndex)))
            {
                if (ni.ToString().ToUpper() == name.ToUpper())
                    return (int)ni;
            }
            return 0;
        }

        static void DisplayBinder()
        {
            string border = "+------+------+------+------+------+";
            Console.WriteLine("\n" + border);
            Console.Write("|");

            for (int i = 1; i <= 15; i++)
            {
                string display;

                if (binderPullCount.ContainsKey(i) && binderPullCount[i] > 0)
                {
                    NationalIndex ni = (NationalIndex)i;
                    string name = ni.ToString().ToUpper();
                    display = CenterText(name, 6);
                }
                else
                {
                    string idx = "#" + i.ToString("D3");
                    display = CenterText(idx, 6);
                }

                Console.Write(display + "|");

                if (i % 5 == 0)
                {
                    Console.WriteLine();
                    Console.WriteLine(border);
                    if (i < 15)
                        Console.Write("|");
                }
            }
        }

        static string CenterText(string text, int width)
        {
            if (text.Length >= width)
                return text.Substring(0, width);

            int totalPadding = width - text.Length;
            int leftPadding = totalPadding / 2;
            int rightPadding = totalPadding - leftPadding;
            return new string(' ', leftPadding) + text + new string(' ', rightPadding);
        }

        static void DisplayBinderSystem()
        {
            bool inBinder = true;
            while (inBinder)
            {
                DisplayBinder();
                Console.WriteLine("\nEnter a number (1-15) to view card details, or 0 to exit:");
                Console.Write("Choice: ");
                string input = Console.ReadLine();

                if (input == "0")
                {
                    inBinder = false;
                    break;
                }

                int choice;
                if (!int.TryParse(input, out choice) || choice < 1 || choice > 15)
                {
                    Console.WriteLine("Invalid input! Please enter a number from 1 to 15.");
                    continue;
                }

                if (!binderPullCount.ContainsKey(choice) || binderPullCount[choice] == 0)
                {
                    Console.WriteLine($"\nCard #{choice} is not yet unlocked!");
                    continue;
                }

                NationalIndex ni = (NationalIndex)choice;
                string cardName = ni.ToString();
                BaseCard found = null;

                foreach (BaseCard c in originalCards)
                {
                    if (c.Name.ToUpper() == cardName.ToUpper())
                    {
                        found = c;
                        break;
                    }
                }

                if (found != null)
                {
                    int pullCount = binderPullCount[choice];
                    Console.WriteLine();
                    CardPrinter.PrintWithCount(found, pullCount);
                }
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

            Console.WriteLine("\n--- SELECT YOUR CARD ---");
            DisplayBinder();
            BaseCard userCard = null;

            while (userCard == null)
            {
                Console.Write("\nEnter card number (1-15) to select: ");
                string input = Console.ReadLine();
                int choice;

                if (!int.TryParse(input, out choice) || choice < 1 || choice > 15)
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
                string cardName = ni.ToString();

                foreach (BaseCard c in originalCards)
                {
                    if (c.Name.ToUpper() == cardName.ToUpper())
                    {
                        userCard = c;
                        break;
                    }
                }
            }

            userCard.HP = GetOriginalHP(userCard.Name);

            Console.WriteLine("\nYou selected:");
            userCard.PrintCard();

            BaseCard aiCard = allCards[rng.Next(allCards.Count)];
            aiCard.HP = GetOriginalHP(aiCard.Name);

            Console.WriteLine("\nAI selected:");
            aiCard.PrintCard();

            RunBattle(userCard, aiCard);
        }

        static int GetOriginalHP(string name)
        {
            foreach (BaseCard c in originalCards)
            {
                if (c.Name.ToUpper() == name.ToUpper())
                    return c.HP;
            }
            return 0;
        }

        static bool HasAdvantage(CardType attacker, CardType defender)
        {
            if (attacker == CardType.FIRE && defender == CardType.GRASS) return true;
            if (attacker == CardType.GRASS && defender == CardType.WATER) return true;
            if (attacker == CardType.WATER && defender == CardType.FIRE) return true;
            return false;
        }

        static readonly Dictionary<CardType, (string name, StatusEffect effect)> Spells =
            new Dictionary<CardType, (string name, StatusEffect effect)>
            {
                { CardType.FIRE, ("Burn", StatusEffect.Burned) },
                { CardType.WATER, ("Scald", StatusEffect.Confused) },
                { CardType.GRASS, ("Poison", StatusEffect.Poisoned) }
            };

        static Action<BaseCard, BaseCard> NormalAttack = (attacker, defender) =>
        {
            int damage = attacker.Attack;
            if (HasAdvantage(attacker.Type, defender.Type))
                damage += 20;
            defender.HP -= damage;
            Console.WriteLine($"{attacker.Name} attacks for {damage} damage!");
            Console.WriteLine($"{defender.Name} HP: {defender.HP}");
        };

        static Action<BaseCard, BaseCard, Random> SpecialTurn = (attacker, defender, rng) =>
        {
            Console.WriteLine($"\n{attacker.Name}'s Special Turn!");
            bool heads = rng.Next(2) == 0;
            if (heads)
            {
                Console.WriteLine("Coin Flip: HEADS!");
                NormalAttack(attacker, defender);
                defender.HP -= 20;
                var spell = Spells[attacker.Type];
                Console.WriteLine($"{attacker.Name} used {spell.name}!");
                Console.WriteLine($"{defender.Name} is {spell.effect}! (-20 HP)");
                Console.WriteLine($"{defender.Name} HP: {defender.HP}");
                if (HasAdvantage(attacker.Type, defender.Type))
                {
                    defender.HP -= 20;
                    Console.WriteLine($"Type advantage! +20 damage!");
                    Console.WriteLine($"{defender.Name} HP: {defender.HP}");
                }
            }
            else
            {
                Console.WriteLine("Coin Flip: TAILS!");
                NormalAttack(attacker, defender);
                if (HasAdvantage(attacker.Type, defender.Type))
                {
                    defender.HP -= 20;
                    Console.WriteLine($"Type advantage! +20 damage!");
                    Console.WriteLine($"{defender.Name} HP: {defender.HP}");
                }
            }
        };

        static void RunBattle(BaseCard user, BaseCard ai)
        {
            Console.WriteLine("\n========== BATTLE START ==========");

            Console.WriteLine("\n[Turn 1] Your Normal Attack!");
            NormalAttack(user, ai);
            if (ai.HP <= 0) { Console.WriteLine($"\n{ai.Name} fainted! YOU WIN! :)"); ReplayOptions(user); return; }

            Console.WriteLine("\n[Turn 2] AI's Normal Attack!");
            NormalAttack(ai, user);
            if (user.HP <= 0) { Console.WriteLine($"\n{user.Name} fainted! YOU LOSE! :("); ReplayOptions(user); return; }

            Console.WriteLine("\n[Turn 3] Your Special Turn!");
            SpecialTurn(user, ai, rng);
            if (ai.HP <= 0) { Console.WriteLine($"\n{ai.Name} fainted! YOU WIN! :)"); ReplayOptions(user); return; }

            Console.WriteLine("\n[Turn 4] AI's Special Turn!");
            SpecialTurn(ai, user, rng);
            if (user.HP <= 0) { Console.WriteLine($"\n{user.Name} fainted! YOU LOSE! :("); ReplayOptions(user); return; }

            Console.WriteLine("\n========== IT'S A TIE! ==========");
            ReplayOptions(user);
        }

        static void ReplayOptions(BaseCard lastCard)
        {
            Console.WriteLine("\n[1] Play again with the same card");
            Console.WriteLine("[2] Choose another card");
            Console.WriteLine("[3] Exit to main menu");
            Console.Write("Choice: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    lastCard.HP = GetOriginalHP(lastCard.Name);
                    BaseCard aiCard = allCards[rng.Next(allCards.Count)];
                    aiCard.HP = GetOriginalHP(aiCard.Name);
                    Console.WriteLine("\nAI selected:");
                    aiCard.PrintCard();
                    RunBattle(lastCard, aiCard);
                    break;
                case "2":
                    Battle();
                    break;
                case "3":
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    ReplayOptions(lastCard);
                    break;
            }
        }

        static void SavePulls()
        {
            List<string> lines = new List<string>();
            for (int i = 1; i <= 15; i++)
            {
                int count = binderPullCount.ContainsKey(i) ? binderPullCount[i] : 0;
                lines.Add($"{i},{count}");
            }
            File.WriteAllLines("pulls.txt", lines);
        }

        static void LoadPulls()
        {
            if (!File.Exists("pulls.txt")) return;

            string[] lines = File.ReadAllLines("pulls.txt");
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length == 2)
                {
                    int index = int.Parse(parts[0]);
                    int count = int.Parse(parts[1]);
                    binderPullCount[index] = count;
                }
            }
        }
    }
}
