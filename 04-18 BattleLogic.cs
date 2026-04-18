using System;
using System.Collections.Generic;

namespace ISPRGG_SHNGPRL_FINALS_TCG
{
    public static class BattleLogic
    {
        public static bool ProgramBattleRestart = false;

        static readonly Dictionary<CardType, (string name, StatusEffect effect)> Spells =
            new Dictionary<CardType, (string name, StatusEffect effect)>
            {
                { CardType.FIRE, ("Burn", StatusEffect.Burned) },
                { CardType.WATER, ("Scald", StatusEffect.Confused) },
                { CardType.GRASS, ("Poison", StatusEffect.Poisoned) }
            };

        static bool HasAdvantage(CardType attacker, CardType defender)
        {
            if (attacker == CardType.FIRE && defender == CardType.GRASS) return true;
            if (attacker == CardType.GRASS && defender == CardType.WATER) return true;
            if (attacker == CardType.WATER && defender == CardType.FIRE) return true;
            return false;
        }

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
                int damage = attacker.Attack;
                if (HasAdvantage(attacker.Type, defender.Type))
                    damage += 20;
                damage += 20;
                defender.HP -= damage;
                Console.WriteLine($"{attacker.Name} attacks for {damage} damage!");
                Console.WriteLine($"{defender.Name} HP: {defender.HP}");
                var spell = Spells[attacker.Type];
                Console.WriteLine($"{attacker.Name} used {spell.name}!");
                Console.WriteLine($"{defender.Name} is {spell.effect}! (-20 HP)");
                defender.HP -= 20;
                Console.WriteLine($"{defender.Name} HP: {defender.HP}");
            }
            else
            {
                Console.WriteLine("Coin Flip: TAILS!");
                int damage = attacker.Attack;
                if (HasAdvantage(attacker.Type, defender.Type))
                    damage += 20;
                defender.HP -= damage;
                Console.WriteLine($"{attacker.Name} attacks for {damage} damage!");
                Console.WriteLine($"{defender.Name} HP: {defender.HP}");
            }
        };

        public static void RunBattle(
            BaseCard user,
            BaseCard ai,
            List<BaseCard> allCards,
            List<BaseCard> originalCards,
            Dictionary<int, int> binderPullCount,
            Random rng)
        {
            Console.WriteLine("\n========== BATTLE START ==========");

            Console.WriteLine("\n[Turn 1] Your Normal Attack!");
            NormalAttack(user, ai);
            if (ai.HP <= 0)
            {
                Console.WriteLine($"\n{ai.Name} fainted! YOU WIN! :)");
                ReplayOptions(user, allCards, originalCards, binderPullCount, rng);
                return;
            }

            Console.WriteLine("\n[Turn 2] AI's Normal Attack!");
            NormalAttack(ai, user);
            if (user.HP <= 0)
            {
                Console.WriteLine($"\n{user.Name} fainted! YOU LOSE! :(");
                ReplayOptions(user, allCards, originalCards, binderPullCount, rng);
                return;
            }

            Console.WriteLine("\n[Turn 3] Your Special Turn!");
            SpecialTurn(user, ai, rng);
            if (ai.HP <= 0)
            {
                Console.WriteLine($"\n{ai.Name} fainted! YOU WIN! :)");
                ReplayOptions(user, allCards, originalCards, binderPullCount, rng);
                return;
            }

            Console.WriteLine("\n[Turn 4] AI's Special Turn!");
            SpecialTurn(ai, user, rng);
            if (user.HP <= 0)
            {
                Console.WriteLine($"\n{user.Name} fainted! YOU LOSE! :(");
                ReplayOptions(user, allCards, originalCards, binderPullCount, rng);
                return;
            }

            Console.WriteLine("\n========== IT'S A TIE! ==========");
            ReplayOptions(user, allCards, originalCards, binderPullCount, rng);
        }

        static void ReplayOptions(
            BaseCard lastCard,
            List<BaseCard> allCards,
            List<BaseCard> originalCards,
            Dictionary<int, int> binderPullCount,
            Random rng)
        {
            Console.WriteLine("\n[1] Play again with the same card");
            Console.WriteLine("[2] Choose another card");
            Console.WriteLine("[3] Exit to main menu");
            Console.Write("Choice: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    lastCard.HP = originalCards.Find(c =>
                        c.Name.Equals(lastCard.Name,
                        StringComparison.OrdinalIgnoreCase))?.HP ?? lastCard.HP;

                    BaseCard newAI = allCards[rng.Next(allCards.Count)];
                    newAI.HP = originalCards.Find(c =>
                        c.Name.Equals(newAI.Name,
                        StringComparison.OrdinalIgnoreCase))?.HP ?? newAI.HP;

                    Console.WriteLine("\nAI selected:");
                    newAI.PrintCard();

                    RunBattle(lastCard, newAI, allCards, originalCards, binderPullCount, rng);
                    break;

                case "2":
                    ProgramBattleRestart = true;
                    break;

                case "3":
                    break;

                default:
                    Console.WriteLine("Invalid option.");
                    ReplayOptions(lastCard, allCards, originalCards, binderPullCount, rng);
                    break;
            }
        }
    }
}
