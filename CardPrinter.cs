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

            // Content lines
            string line1 = $"{card.Name} HP:{card.HP}";
            string line2 = $"TYPE: {card.Type}";
            string line3 = $"{attackName}: {card.Attack}";
            string line4 = $"{card.SpecialMove}";

            // Find the longest line to set card width
            int maxLen = Math.Max(Math.Max(line1.Length, line2.Length),
                         Math.Max(line3.Length, line4.Length));
            int width = maxLen + 4; // padding

            // Choose border character based on rarity
            char border = GetBorderChar(card.Rarity);
            string horizontal = new string(border, width);

            // Print the card
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