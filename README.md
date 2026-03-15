#### ISPRGG-SHNGPRL Finals — TCG Project

haii!! so i've uploaded all the files here so we can all work on it together. kasi last finals medj nahihirapan kami mag transfer ng files from diff devices/laptop below is a quick guide on what's already done, what's left, and how to set everything up on your end. lmk if may mali or something needs to be updated so we can fix it asap — or just edit it directly na!! >__<

---

#### whats in this folder (repo)
we made this before sa machine exercise 3 so nag-rely din ako a bit dun.

| file | what it does |
|---|---|
| `cards.txt` | the data/blueprint that defines each card (name, type, rarity, etc.) |
| `Enums.cs` | defines `CardType`, `StatusEffect`, and `NationalIndex` enums |
| `Card.cs` | the `ICard` interface + `BaseCard` abstract class + concrete card classes (FireCard, WaterCard, GrassCard) |
| `CardPrinter.cs` | ASCII-based card printer — displays cards with borders that change based on rarity |
| `FileLoader.cs` | reads `cards.txt`, splits each line by commas, and creates the right card object |
| `Program.cs` | main entry point |

---

#### what's done

- [x] project set-up (cards.txt configured to copy always to bin/Debug/)
- [x] Enums.cs
- [x] Card class + inheritance (ICard → BaseCard → FireCard / WaterCard / GrassCard)
- [x] CardPrinter.cs (card display w/ rarity borders)
- [x] FileLoader.cs (file loader connecting cards.txt → Card objects)

---

#### what still needs to be done

- [ ] battle mechanics
- [ ] card pulling system
- [ ] card binding system
- [ ] replay options + menu loop
- [ ] remove all placeholder comments once finalized

---

#### how to set up

1. duplicate or download this repo (the whole files including the .txt)
2. open the `.sln` file in **visual studio**
3. make sure `cards.txt` is set to **copy always** in its properties → Copy to Output Directory (it should already be set but just double check!!)
4. try to click on the start / run — it should work from the `bin/Debug/` folder automatically

---

#### quickie (unnecessary) notes

- refer to the docs for the parts that still need to be done:
  - **battle mechanics** — check the relevant section in sir's docs
  - **card pulling + binding system** — connected to FileLoader flow: `cards.txt → FileLoader → Card objects → Game uses them`
- if you find any bugs (esp. in CardPrinter — nakailang try aq dun kc medj maraming errors!!) pls let us know sa gc
- feel free to push changes directly.
- also sabi ni sir pala wala naman finofollow na format yung file name so i think para di na tayo ma-confuse, ito na lang gamitin (no drafts na ko).

---

*last updated by: [eeenawh] — 03/15/2026*
