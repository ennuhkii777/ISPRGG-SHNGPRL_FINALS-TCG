using System;

namespace ISPRGG_SHNGPRL_FINALS_TCG
{
    public static class CardPrinter
    {
        public static void Print(BaseCard card)
        {
            string attackName = BaseCard.AttackNames.ContainsKey(card.AttackCode)
                ? BaseCard.AttackNames[card.AttackCode]
                : card.AttackCode;

            string line1 = $"{card.Name} HP:{card.HP}";
            string line2 = $"TYPE: {card.Type}";
            string line3 = $"{attackName}: {card.Attack}";
            string line4 = $"{card.SpecialMove}";

            int maxLen = Math.Max(Math.Max(line1.Length, line2.Length),
                         Math.Max(line3.Length, line4.Length));
            int width = maxLen + 4;

            char border = GetBorderChar(card.Rarity);
            string horizontal = new string(border, width);

            if (card.Rarity == 1)
            {
                Console.WriteLine("+" + horizontal + "+");
                Console.WriteLine("| " + line1.PadRight(width - 2) + " |");
                Console.WriteLine("| " + line2.PadRight(width - 2) + " |");
                Console.WriteLine("| " + line3.PadRight(width - 2) + " |");
                Console.WriteLine("| " + line4.PadRight(width - 2) + " |");
                Console.WriteLine("+" + horizontal + "+");
            }
            else if (card.Rarity == 2)
            {
                Console.WriteLine("+" + horizontal + "+");
                Console.WriteLine("| " + line1.PadRight(width - 2) + " |");
                Console.WriteLine("| " + line2.PadRight(width - 2) + " |");
                Console.WriteLine("| " + line3.PadRight(width - 2) + " |");
                Console.WriteLine("| " + line4.PadRight(width - 2) + " |");
                Console.WriteLine("+" + horizontal + "+");
            }
            else
            {
                Console.WriteLine(new string(border, width + 2));
                Console.WriteLine(border + " " + line1.PadRight(width - 2) + " " + border);
                Console.WriteLine(border + " " + line2.PadRight(width - 2) + " " + border);
                Console.WriteLine(border + " " + line3.PadRight(width - 2) + " " + border);
                Console.WriteLine(border + " " + line4.PadRight(width - 2) + " " + border);
                Console.WriteLine(new string(border, width + 2));
            }
        }

        public static void PrintWithCount(BaseCard card, int pullCount)
        {
            string attackName = BaseCard.AttackNames.ContainsKey(card.AttackCode)
                ? BaseCard.AttackNames[card.AttackCode]
                : card.AttackCode;

            string line1 = $"{card.Name} HP:{card.HP}";
            string line2 = $"TYPE: {card.Type}";
            string line3 = $"{attackName}: {card.Attack}";
            string line4 = $"{card.SpecialMove}";

            int maxLen = Math.Max(Math.Max(line1.Length, line2.Length),
                         Math.Max(line3.Length, line4.Length));
            int width = maxLen + 2;

            char border = GetBorderChar(card.Rarity);

            if (card.Rarity == 1 || card.Rarity == 2)
            {
                char h = card.Rarity == 1 ? '-' : '=';
                string horizontal = new string(h, width + 2);
                Console.WriteLine("+" + horizontal + "+");
                Console.WriteLine("| " + line1.PadRight(width) + " |");
                Console.WriteLine("| " + line2.PadRight(width) + " |");
                Console.WriteLine("| " + line3.PadRight(width) + " |");
                Console.WriteLine("| " + line4.PadRight(width) + " |");
                Console.WriteLine("+" + horizontal + "+");
            }
            else
            {
                string horizontal = new string(border, width + 4);
                Console.WriteLine(horizontal);
                Console.WriteLine(border + " " + line1.PadRight(width) + " " + border);
                Console.WriteLine(border + " " + line2.PadRight(width) + " " + border);
                Console.WriteLine(border + " " + line3.PadRight(width) + " " + border);
                Console.WriteLine(border + " " + line4.PadRight(width) + " " + border);
                Console.WriteLine(horizontal);
            }

            Console.WriteLine($"Owned: {pullCount}x");
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
    }
}
