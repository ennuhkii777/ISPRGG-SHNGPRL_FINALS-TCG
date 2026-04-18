using System;
using System.Collections.Generic;

namespace ISPRGG_SHNGPRL_FINALS_TCG
{
    public static class BattleLogic
    {
        public static bool ProgramBattleRestart = false;
        private static Random rng;

        private static Dictionary<CardType, (string name, StatusEffect effect)> Spells =
            new Dictionary<CardType, (string name, StatusEffect effect)>()
        {
            { CardType.FIRE, ("Burn", StatusEffect.Burned) },
            { CardType.WATER, ("Scald", StatusEffect.Confused) },
            { CardType.GRASS, ("Poison", StatusEffect.Poisoned) }
        };

        public static void RunBattle(
            BaseCard user,
            BaseCard ai,
            List<BaseCard> allCards,
            List<BaseCard> originalCards,
            Dictionary<int, int> binderPullCount,
            Random sharedRng)
        {
            rng = sharedRng;

            Console.WriteLine("\n========== BATTLE START ==========");

            Console.WriteLine("\n[Turn 1] Your Normal Attack!");
            NormalAttack(user, ai);
            if (CheckEnd(user, ai, allCards, originalCards, binderPullCount)) return;

            Console.WriteLine("\n[Turn 2] AI's Normal Attack!");
            NormalAttack(ai, user);
            if (CheckEnd(user, ai, allCards, originalCards, binderPullCount)) return;

            Console.WriteLine("\n[Turn 3] Your Special Turn!");
            SpecialTurn(user, ai);
            if (CheckEnd(user, ai, allCards, originalCards, binderPullCount)) return;

            Console.WriteLine("\n[Turn 4] AI's Special Turn!");
            SpecialTurn(ai, user);
            if (CheckEnd(user, ai, allCards, originalCards, binderPullCount)) return;

            Console.WriteLine("\n========== IT'S A TIE! ==========");
            Replay(user, allCards, originalCards, binderPullCount);
        }

        private static void NormalAttack(BaseCard attacker, BaseCard defender)
        {
            int damage = attacker.Attack;

            if (HasAdvantage(attacker.Type, defender.Type))
                damage += 20;

            defender.HP -= damage;

            Console.WriteLine($"{attacker.Name} deals {damage} damage!");
            Console.WriteLine($"{defender.Name} HP: {defender.HP}");
        }

        private static void SpecialTurn(BaseCard attacker, BaseCard defender)
        {
            Console.WriteLine($"\n{attacker.Name}'s Special Turn!");

            if (rng.Next(2) == 0)
            {
                Console.WriteLine("HEADS!");
                NormalAttack(attacker, defender);
                defender.HP -= 20;

                var spell = Spells[attacker.Type];
                Console.WriteLine($"{attacker.Name} used {spell.name}!");
            }
            else
            {
                Console.WriteLine("TAILS!");
                NormalAttack(attacker, defender);
            }
        }

        private static bool HasAdvantage(CardType a, CardType b)
        {
            return (a == CardType.FIRE && b == CardType.GRASS) ||
                   (a == CardType.GRASS && b == CardType.WATER) ||
                   (a == CardType.WATER && b == CardType.FIRE);
        }

        private static bool CheckEnd(
            BaseCard user,
            BaseCard ai,
            List<BaseCard> allCards,
            List<BaseCard> originalCards,
            Dictionary<int, int> binderPullCount)
        {
            if (ai.HP <= 0)
            {
                Console.WriteLine($"\n{ai.Name} fainted! YOU WIN!");
                Replay(user, allCards, originalCards, binderPullCount);
                return true;
            }

            if (user.HP <= 0)
            {
                Console.WriteLine($"\n{user.Name} fainted! YOU LOSE!");
                Replay(user, allCards, originalCards, binderPullCount);
                return true;
            }

            return false;
        }

        private static void Replay(
            BaseCard lastCard,
            List<BaseCard> allCards,
            List<BaseCard> originalCards,
            Dictionary<int, int> binderPullCount)
        {
            Console.WriteLine("\n[1] Play again using the same card selected");
            Console.WriteLine("[2] Choose another card");
            Console.WriteLine("[3] Exit");
            Console.Write("Choice: ");

            string input = Console.ReadLine();

            if (input == "1")
            {
                BaseCard ai = allCards[rng.Next(allCards.Count)];
                RunBattle(lastCard, ai, allCards, originalCards, binderPullCount, rng);
            }
            else if (input == "2")
            {
                ProgramBattleRestart = true;
            }
        }
    }
}
