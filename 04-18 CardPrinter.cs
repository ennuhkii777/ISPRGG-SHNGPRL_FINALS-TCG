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

            string line1 = $"{card.Name} HP:{card.HP}".Trim();
            string line2 = $"TYPE: {card.Type}".Trim();
            string line3 = $"{attackName}: {card.Attack}".Trim();
            string line4 = $"{card.SpecialMove}".Trim();

            int contentWidth = Math.Max(Math.Max(line1.Length, line2.Length),
                               Math.Max(line3.Length, line4.Length));

            char border = GetBorderChar(card.Rarity);

            if (card.Rarity == 1)
            {
                string horizontal = new string('-', contentWidth + 2);
                Console.WriteLine("+" + horizontal + "+");
                Console.WriteLine("| " + line1.PadRight(contentWidth) + " |");
                Console.WriteLine("| " + line2.PadRight(contentWidth) + " |");
                Console.WriteLine("| " + line3.PadRight(contentWidth) + " |");
                Console.WriteLine("| " + line4.PadRight(contentWidth) + " |");
                Console.WriteLine("+" + horizontal + "+");
            }
            else if (card.Rarity == 2)
            {
                string horizontal = new string('=', contentWidth + 2);
                Console.WriteLine("+" + horizontal + "+");
                Console.WriteLine("| " + line1.PadRight(contentWidth) + " |");
                Console.WriteLine("| " + line2.PadRight(contentWidth) + " |");
                Console.WriteLine("| " + line3.PadRight(contentWidth) + " |");
                Console.WriteLine("| " + line4.PadRight(contentWidth) + " |");
                Console.WriteLine("+" + horizontal + "+");
            }
            else
            {
                string horizontal = new string(border, contentWidth + 4);
                Console.WriteLine(horizontal);
                Console.WriteLine(border + " " + line1.PadRight(contentWidth) + " " + border);
                Console.WriteLine(border + " " + line2.PadRight(contentWidth) + " " + border);
                Console.WriteLine(border + " " + line3.PadRight(contentWidth) + " " + border);
                Console.WriteLine(border + " " + line4.PadRight(contentWidth) + " " + border);
                Console.WriteLine(horizontal);
            }
        }

        public static void PrintWithCount(BaseCard card, int pullCount)
        {
            string attackName = BaseCard.AttackNames.ContainsKey(card.AttackCode)
                ? BaseCard.AttackNames[card.AttackCode]
                : card.AttackCode;

            string line1 = $"{card.Name} HP:{card.HP}".Trim();
            string line2 = $"TYPE: {card.Type}".Trim();
            string line3 = $"{attackName}: {card.Attack}".Trim();
            string line4 = $"{card.SpecialMove}".Trim();

            int contentWidth = Math.Max(Math.Max(line1.Length, line2.Length),
                               Math.Max(line3.Length, line4.Length));

            char border = GetBorderChar(card.Rarity);

            string borderSuffix = "";
            string contentSuffix = "";
            string topSuffix = "";

            if (pullCount >= 6)
            {
                topSuffix = " x" + pullCount;
            }
            else if (pullCount >= 2)
            {
                borderSuffix = new string('+', pullCount - 1);
                contentSuffix = new string('|', pullCount - 1);
            }

            if (card.Rarity == 1)
            {
                string horizontal = new string('-', contentWidth + 2);
                Console.WriteLine("+" + horizontal + "+" + borderSuffix + topSuffix);
                Console.WriteLine("| " + line1.PadRight(contentWidth) + " |" + contentSuffix);
                Console.WriteLine("| " + line2.PadRight(contentWidth) + " |" + contentSuffix);
                Console.WriteLine("| " + line3.PadRight(contentWidth) + " |" + contentSuffix);
                Console.WriteLine("| " + line4.PadRight(contentWidth) + " |" + contentSuffix);
                Console.WriteLine("+" + horizontal + "+" + borderSuffix);
            }
            else if (card.Rarity == 2)
            {
                string horizontal = new string('=', contentWidth + 2);
                Console.WriteLine("+" + horizontal + "+" + borderSuffix + topSuffix);
                Console.WriteLine("| " + line1.PadRight(contentWidth) + " |" + contentSuffix);
                Console.WriteLine("| " + line2.PadRight(contentWidth) + " |" + contentSuffix);
                Console.WriteLine("| " + line3.PadRight(contentWidth) + " |" + contentSuffix);
                Console.WriteLine("| " + line4.PadRight(contentWidth) + " |" + contentSuffix);
                Console.WriteLine("+" + horizontal + "+" + borderSuffix);
            }
            else
            {
                string horizontal = new string(border, contentWidth + 4);
                Console.WriteLine(horizontal + topSuffix);
                Console.WriteLine(border + " " + line1.PadRight(contentWidth) + " " + border + contentSuffix);
                Console.WriteLine(border + " " + line2.PadRight(contentWidth) + " " + border + contentSuffix);
                Console.WriteLine(border + " " + line3.PadRight(contentWidth) + " " + border + contentSuffix);
                Console.WriteLine(border + " " + line4.PadRight(contentWidth) + " " + border + contentSuffix);
                Console.WriteLine(horizontal);
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
