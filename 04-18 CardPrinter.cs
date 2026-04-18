using System;

namespace ISPRGG_SHNGPRL_FINALS_TCG
{
    public static class CardPrinter
    {
        public static void Print(BaseCard card)
        {
            PrintInternal(card, 1);
        }

        public static void PrintWithCount(BaseCard card, int pullCount)
        {
            PrintInternal(card, pullCount);
        }

        private static void PrintInternal(BaseCard card, int pullCount)
        {
            string attackName = BaseCard.AttackNames.ContainsKey(card.AttackCode)
                ? BaseCard.AttackNames[card.AttackCode]
                : card.AttackCode;

            string line1 = $"{card.Name} HP:{card.HP}";
            string line2 = $"TYPE: {card.Type}";
            string line3 = $"{attackName}: {card.Attack}";
            string line4 = $"{card.SpecialMove}";

            int maxLen = Math.Max(
                Math.Max(line1.Length, line2.Length),
                Math.Max(line3.Length, line4.Length)
            );

            int width = maxLen + 4;

            char border = GetBorderChar(card.Rarity);

            string stack = "";
            string label = "";

            if (pullCount <= 4)
            {
                stack = new string(GetStackChar(card.Rarity), pullCount);
            }
            else if (pullCount >= 6)
            {
                label = $" {pullCount}x";
            }

            PrintTop(card.Rarity, border, width, stack, label);

            PrintRow(card.Rarity, line1, width, border, stack);
            PrintRow(card.Rarity, line2, width, border, stack);
            PrintRow(card.Rarity, line3, width, border, stack);
            PrintRow(card.Rarity, line4, width, border, stack);

            PrintBottom(card.Rarity, border, width, stack);
        }

        private static void PrintTop(int rarity, char border, int width, string stack, string label)
        {
            int extra = stack.Length;

            if (rarity == 1 || rarity == 2)
            {
                string horizontal = new string(border, width);
                string plusStack = new string('+', extra);

                if (!string.IsNullOrEmpty(label))
                    Console.WriteLine("+" + horizontal + "+" + label);
                else
                    Console.WriteLine("+" + horizontal + "+" + plusStack);
            }
            else
            {
                int totalWidth = width + 2 + extra;

                string line = new string(border, totalWidth);

                if (!string.IsNullOrEmpty(label))
                    Console.WriteLine(line + label);
                else
                    Console.WriteLine(line);
            }
        }

        private static void PrintRow(int rarity, string text, int width, char border, string stack)
        {
            string padded = text.PadRight(width - 2);

            if (rarity == 1 || rarity == 2)
                Console.WriteLine("| " + padded + " |" + stack);
            else
                Console.WriteLine(border + " " + padded + " " + border + stack);
        }

        private static void PrintBottom(int rarity, char border, int width, string stack)
        {
            int extra = stack.Length;

            if (rarity == 1 || rarity == 2)
            {
                string horizontal = new string(border, width);
                string plusStack = new string('+', extra);

                Console.WriteLine("+" + horizontal + "+" + plusStack);
            }
            else
            {
                // FIX: match FULL row width
                int totalWidth = width + 2 + extra;

                string line = new string(border, totalWidth);
                Console.WriteLine(line);
            }
        }

        private static char GetBorderChar(int rarity)
        {
            switch (rarity)
            {
                case 1: return '-';
                case 2: return '=';
                case 3: return '*';
                case 4: return '@';
                case 5: return '#';
                default: return '-';
            }
        }

        private static char GetStackChar(int rarity)
        {
            if (rarity == 1 || rarity == 2)
                return '|';

            return GetBorderChar(rarity);
        }
    }
}

