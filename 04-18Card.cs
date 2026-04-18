using System;
using System.Collections.Generic;

namespace ISPRGG_SHNGPRL_FINALS_TCG
{
    // INTERFACE - the contract every card must follow
    public interface ICard
    {
        string Name { get; }
        CardType Type { get; }
        int HP { get; set; }
        int Attack { get; }
        int Rarity { get; }
        string SpecialMove { get; }
        void PrintCard();
    }

    // ABSTRACT BASE CLASS - shared stuff all cards have
    public abstract class BaseCard : ICard
    {
        public string Name { get; protected set; }
        public CardType Type { get; protected set; }
        public int HP { get; set; }
        public int Attack { get; protected set; }
        public int Rarity { get; protected set; }
        public string AttackCode { get; protected set; }
        public string SpecialMove { get; protected set; }

        // Dictionary to map attack codes to attack names
        public static readonly Dictionary<string, string> AttackNames = new Dictionary<string, string>
        {
            { "JMS", "Jump Scare" },
            { "SFB", "Soft Block" },
            { "HPS", "Hard Pass" },
            { "DLL", "Delulu" },
            { "BRR", "Brain Rot" },
            { "MXS", "Mixed Signal" },
            { "RBD", "Rebound" },
            { "ORB", "Orbiting" },
            { "RLP", "Relapse" },
            { "GHS", "Ghosting" },
            { "HRL", "Hard Launch" },
            { "NCH", "Nonchalant" },
            { "LVB", "Love Bomb" },
            { "AUF", "Aura Farm" },
            { "TRD", "Trauma Dump" }
        };

        public abstract void PrintCard();
    }

    // FIRE CARD
    public class FireCard : BaseCard
    {
        public FireCard(string name, int hp, string attackCode, int attack, int rarity, string specialMove)
        {
            Name = name;
            Type = CardType.FIRE;
            HP = hp;
            AttackCode = attackCode;
            Attack = attack;
            Rarity = rarity;
            SpecialMove = specialMove;
        }

        public override void PrintCard()
        {
            CardPrinter.Print(this);
        }
    }

    // WATER CARD
    public class WaterCard : BaseCard
    {
        public WaterCard(string name, int hp, string attackCode, int attack, int rarity, string specialMove)
        {
            Name = name;
            Type = CardType.WATER;
            HP = hp;
            AttackCode = attackCode;
            Attack = attack;
            Rarity = rarity;
            SpecialMove = specialMove;
        }

        public override void PrintCard()
        {
            CardPrinter.Print(this);
        }
    }

    // GRASS CARD
    public class GrassCard : BaseCard
    {
        public GrassCard(string name, int hp, string attackCode, int attack, int rarity, string specialMove)
        {
            Name = name;
            Type = CardType.GRASS;
            HP = hp;
            AttackCode = attackCode;
            Attack = attack;
            Rarity = rarity;
            SpecialMove = specialMove;
        }

        public override void PrintCard()
        {
            CardPrinter.Print(this);
        }
    }
}
