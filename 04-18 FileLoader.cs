using System;
using System.Collections.Generic;
using System.IO;

namespace ISPRGG_SHNGPRL_FINALS_TCG
{
    public static class FileLoader
    {
        public static List<BaseCard> LoadCards(string path)
        {
            List<BaseCard> cards = new List<BaseCard>();
            if (!File.Exists(path))
            {
                Console.WriteLine("cards.txt not found!");
                return cards;
            }

            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                string[] parts = line.Split(',');
                if (parts.Length < 7) continue;

                string name        = parts[0].Trim();
                string typeStr     = parts[1].Trim();
                int rarity         = int.Parse(parts[2].Trim());
                int hp             = int.Parse(parts[3].Trim());
                string attackCode  = parts[4].Trim();
                int attack         = int.Parse(parts[5].Trim());
                string specialMove = parts[6].Trim();

                BaseCard card = null;
                if (typeStr.ToUpper() == "FIRE")
                    card = new FireCard(name, hp, attackCode, attack, rarity, specialMove);
                else if (typeStr.ToUpper() == "WATER")
                    card = new WaterCard(name, hp, attackCode, attack, rarity, specialMove);
                else if (typeStr.ToUpper() == "GRASS")
                    card = new GrassCard(name, hp, attackCode, attack, rarity, specialMove);

                if (card != null)
                    cards.Add(card);
            }
            return cards;
        }
    }
}
