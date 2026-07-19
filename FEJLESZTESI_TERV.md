# Into The Void – fejlesztési terv

## 1. Javasolt irány

Az **Into The Void** legerősebb, megkülönböztető eleme ne önmagában az RPG-rendszer vagy a pályák száma legyen, hanem a **zuhanási sebesség tudatos kezelése**. A játékos folyamatosan döntsön arról, hogy gyorsít-e a nagyobb támadóerő, áttörés és jutalom érdekében, vagy lassít a pontosabb irányítás, védekezés és biztonság kedvéért.

A legelső játszható változat csak ezt a fő játékhurkot bizonyítsa:

1. a karakter automatikusan zuhan;
2. a játékos oldalirányban mozog, gyorsít és lassít;
3. akadályokat kerül ki vagy megfelelő sebességgel áttör;
4. bónuszokat gyűjt;
5. eléri a pálya alját vagy elfogy az életereje;
6. kap egy rövid eredményképernyőt, majd újrapróbálhatja a pályát.

Az RPG-rendszer, a sok karakter és a több világ csak akkor kapjon nagyobb hangsúlyt, ha ez a 2–5 perces alapélmény már önmagában szórakoztató.

## 2. PC vagy mobil legyen előbb?

**Elsőként PC-re érdemes elkészíteni a prototípust és a vertical slice-ot**, de már az első naptól mobilbarát korlátokkal.

Ennek okai:

- Unity Editorban és PC buildben gyorsabb az iteráció, hibakeresés és teljesítménymérés.
- Billentyűzettel vagy kontrollerrel könnyebb kitapasztalni, hogy jó-e a mozgás és a sebességmechanika.
- A mobilos érintésvezérlés ne fedje el az alapmechanika hibáit a prototípus időszakában.
- A PC-s próbakörök könnyebben megoszthatók tesztelőkkel.
- Az auto-runner műfaj és a rövid menetek ugyanakkor jól illenek mobilra, ezért a végleges célplatform akár mobil is lehet.

Javasolt sorrend:

1. **PC prototípus:** billentyűzet + kontroller, egyszerű grafikával.
2. **PC vertical slice:** egy teljes, látványos pálya és egy karakter.
3. **Korai Android próba:** még a tartalomgyártás előtt, hogy az érintésvezérlés és a teljesítmény validálva legyen.
4. **Platformdöntés:** tesztelői visszajelzés alapján PC-first vagy mobile-first kiadás.

Fontos: ne a teljes PC-s játék elkészülte után kezdődjön a mobilport. A képernyőarányokat, UI-t, irányítást, objektumpoolozást és grafikai keretet már az első verzióban mobilra is tervezni kell.

## 3. Javasolt játékmeneti pontosítások

### Sebesség mint erőforrás

A sebesség ne csak folyamatosan növekvő szám legyen. Legyen egy kezelhető tartomány, amelyen belül a játékos döntéseket hoz:

- **lassú zuhanás:** pontosabb irányítás, nagyobb védelem, bizonyos képességek használata;
- **közepes zuhanás:** kiegyensúlyozott állapot;
- **gyors zuhanás:** nagyobb sebzés, akadályáttörés, jobb pont- vagy jutalomszorzó, de kisebb reakcióidő;
- **kritikus sebesség:** rövid ideig nagyon erős, de túlmelegedést, sérülést vagy irányításvesztést okozhat.

Érdemes három jól olvasható sebességzónával kezdeni a teljesen folytonos, sok képletes rendszer helyett. A háttér, a hang, a kamera, a szélcsíkok és a karakter animációja együtt jelezze az aktuális állapotot.

### A gödör felépítése

A végtelennek tűnő gödör technikailag véges pálya legyen. A pálya előre definiált hosszúságú, egymás után választott szeletekből áll, így:

- kiszámítható a nehézség és a pálya hossza;
- működik a progress csík;
- garantálható, hogy a generálás nem hoz létre lehetetlen akadálykombinációt;
- később keverhető a kézzel tervezett és a szabályalapú tartalom.

Minden világ egy vagy több **biome szakaszból** állhat. A medieval fantasy, sci-fi, factory, hell, jungle és toxic town témákat kezdetben külön pályaként érdemes kezelni, nem egy pályán belüli véletlenszerű keverékként. Így vizuálisan és játékmenetben is egységesebbek lesznek.

### Akadályok

Az akadályokat döntéstípus szerint érdemes tervezni:

- **kerülendő:** tüskék, falak, lézerek;
- **áttörhető:** csak minimumsebesség felett semmisül meg;
- **lassítandó:** gyors állapotban veszélyes, lassan átjárható;
- **mozgó:** forgó, követő vagy ritmusosan nyíló akadály;
- **választást adó:** biztonságos út kevés jutalommal, nehéz út nagy jutalommal;
- **ellenfél:** támad, blokkol vagy üldöz, de ugyanazt az akadály-interfészt használja.

## 4. Első fejlesztési lépések

### 0. Előkészítés – 1–2 nap

- Egymondatos játékígéret rögzítése: „Irányítsd a zuhanásod sebességét, hogy áttörj, kitérj és elérd a mélység alját.”
- Célképernyő és kameraállás kiválasztása: álló mobilnézet vagy fekvő, PC-kompatibilis nézet.
- Ideiglenes geometriai elemekkel egy tesztpálya létrehozása.
- Teljesítménycél meghatározása: PC 60 FPS; középkategóriás mobilon stabil 60 FPS, szükség esetén 30 FPS-es alsó profil.
- Verziókezelés és Git LFS ellenőrzése a nagy Unity assetek előtt.

### 1. Mozgásprototípus – 3–5 nap

- A karakter maradjon nagyjából a kamera közelében; a gödörszeletek mozogjanak felfelé.
- Oldalirányú mozgás megvalósítása.
- Gyorsítás és lassítás bemenet hozzáadása.
- Sebességminimum, sebességmaximum, gyorsulás és fékezés hangolható adatként szerepeljen.
- Egyszerű kamera-FOV, szélcsík és hangmagasság visszajelzés.
- Fejlesztői HUD: aktuális sebesség, sebességzóna és megtett mélység.

**Kilépési feltétel:** már szürke dobozokkal is élvezetes legyen gyorsítani, lassítani és kapuk között manőverezni.

### 2. Gödörszimulátor – 3–5 nap

- Fix számú aktív gödörszelet kezelése.
- Szeletek csatlakozási pontjainak egységesítése.
- Objektumpool használata létrehozás és törlés helyett.
- Szeletmetaadatok: hossz, nehézség, biome, engedélyezett sebességtartomány, súly.
- Determinisztikus seed lehetősége hibakereséshez.
- Kezdetben 6–10 kézzel épített tesztszelet.

**Kilépési feltétel:** legalább 10 percig megszakítás, rés és memóriafelhalmozás nélkül újrahasznosulnak a szeletek.

### 3. Interakciók és játékhurok – 1 hét

- Sebzés, életerő és halál.
- Áttörhető és kerülendő akadály.
- Egy gyűjthető bónusz és egy ideiglenes power-up.
- Pontozás vagy jutalomszorzó a sebesség alapján.
- Start, szünet, győzelem, vereség, újrakezdés.
- Pályamélység és progress csík.

**Kilépési feltétel:** elejétől a végéig lejátszható 2–3 perces menet.

### 4. Vertical slice – 2–4 hét

- Egy játszható karakter egyedi passzívval és aktív képességgel.
- Egy biome saját látvánnyal, zenével és 12–20 szelettel.
- 4–6 akadálytípus és 3 power-up.
- Alap RPG statok és egy rövid fejlesztési képernyő.
- Véglegeshez közeli UI, hang és vizuális visszajelzések.
- Első Android build és érintésvezérlés.
- Legalább 5–10 külső tesztelő.

**Kilépési feltétel:** a tesztelők külső magyarázat nélkül értik, mikor előny a gyors és mikor a lassú zuhanás, és szívesen újrapróbálják a pályát.

### 5. Tartalmi produkció – csak validáció után

- További karakterek, biome-ok és akadálycsaládok.
- Meta-progresszió és mentés.
- Nehézségi görbe, küldetések, feloldások.
- Akadály-szekvenciák szerkesztőeszköze.
- Platform-specifikus optimalizálás és kiadási folyamat.

## 5. Javasolt kódstruktúra és classok

Az adatokat lehetőleg `ScriptableObject` assetekben, a futás közbeni állapotot normál C# classokban vagy komponensekben kell tartani. Ne legyen egyetlen mindent vezérlő `GameManager`.

### Játékmenet és állapot

| Class | Szerep |
|---|---|
| `GameFlowController` | A menet állapotai: felkészülés, játék, szünet, győzelem, vereség. |
| `RunSession` | Az aktuális futam adatai: mélység, pont, seed, idő, begyűjtött jutalmak. |
| `LevelDefinition` | ScriptableObject: pályahossz, biome-szakaszok, nehézségi görbe, szeletkészletek. |
| `ProgressTracker` | A megtett és hátralévő mélységet számolja; a UI innen kap adatot. |

### Játékos

| Class | Szerep |
|---|---|
| `PlayerController` | A játékos mozgásának magas szintű koordinátora. |
| `PlayerInputReader` | Az Input System eseményeit egységes mozgás/gyorsítás/lassítás parancsokká alakítja PC-n és mobilon. |
| `PlayerMotor` | Oldalirányú mozgás, határok, gyorsulásérzet; nem foglalkozik statokkal vagy UI-val. |
| `PlayerHealth` | Életerő, sérülés, sérthetetlenségi idő és halál. |
| `PlayerAbilityController` | Aktív képesség használata, cooldown és sebességfeltételek. |
| `CharacterDefinition` | ScriptableObject: modell, alaptulajdonságok, képességek és egyedi passzív. |

### Sebességrendszer

| Class | Szerep |
|---|---|
| `FallSpeedController` | Az aktuális zuhanási sebesség, gravitációs gyorsulás, fékezés és limitek egyetlen hiteles forrása. |
| `FallSpeedConfig` | ScriptableObject: minimum/maximum, gyorsulási görbe és zónahatárok. |
| `SpeedZoneEvaluator` | Meghatározza, hogy lassú, normál, gyors vagy kritikus zónában van-e a játékos. |
| `SpeedEffectResolver` | A sebességből számít módosítókat: sebzés, irányíthatóság, áttörési erő, jutalomszorzó. |
| `FallFeedbackController` | Vizuális és hanghatásokat vezérel a sebesség alapján; nem módosít játékszabályt. |

### Gödör és pályagenerálás

| Class | Szerep |
|---|---|
| `PitStreamController` | Mozgatja az aktív szeleteket és kezeli, mikor kell egyet újrahasznosítani. |
| `PitSlice` | Egy pályaszelet komponense, csatlakozási pontokkal és a benne lévő elemek referenciáival. |
| `PitSliceDefinition` | ScriptableObject: prefab, hossz, biome, nehézség, súly és kompatibilitási címkék. |
| `SliceSequenceGenerator` | Szabályok alapján választ következő szeletet, elkerülve a lehetetlen kombinációkat. |
| `BiomeDefinition` | ScriptableObject: szeletkészlet, világítás, köd, zene, VFX és környezeti szabályok. |
| `ObjectPool<T>` / pool service | Szeletek, akadályok, lövedékek és effektek újrahasznosítása. Kezdetben Unity beépített poolingjára épülhet. |

### Akadályok, ellenfelek, pickupok

| Class | Szerep |
|---|---|
| `ObstacleBase` | Közös akadály-életciklus és játékossal való interakció. |
| `IDamageable` | Sebezhető objektumok közös szerződése. |
| `IBreakableBySpeed` | Megadja az áttöréshez szükséges sebességet és az eredményt. |
| `MovingObstacle` | Útvonalon, tengelyen vagy görbe mentén mozgatott akadály. |
| `ObstacleDefinition` | ScriptableObject: sebzés, áttörési küszöb, jutalom és vizuális adatok. |
| `Pickup` | Begyűjtést és egyszeri hatást kezel. |
| `PowerUpDefinition` | ScriptableObject: hatás, időtartam, halmozhatóság és ikon. |
| `StatusEffectController` | Ideiglenes erősítések és gyengítések hozzáadása, frissítése, eltávolítása. |

### RPG és fejlődés

| Class | Szerep |
|---|---|
| `StatDefinition` | Egy stat azonosítója, neve, megjelenítése és érvényes tartománya. |
| `StatCollection` | Alapértékek és módosítók alapján kiszámolja a futás közbeni értékeket. |
| `StatModifier` | Fix vagy százalékos, ideiglenes vagy tartós statmódosítás. |
| `UpgradeDefinition` | ScriptableObject: ár, feltétel, szintkorlát és alkalmazott módosítók. |
| `ProgressionService` | Feloldások, valuta és tartós fejlesztések kezelése. |
| `SaveDataService` | Verziózott mentés betöltése, írása és migrációja. |

Az első prototípusba csak 4–5 valóban érezhető stat kerüljön, például:

- életerő;
- oldalirányú irányíthatóság;
- fékezőerő;
- áttörési erő;
- képesség-cooldown vagy power-up időtartam.

A „zuhanási sebesség” inkább dinamikus futásállapot legyen, ne hagyományos RPG stat. A karakter statjai annak gyorsulását, maximumát vagy hatásait módosíthatják.

### UI és visszajelzés

| Class | Szerep |
|---|---|
| `HUDPresenter` | Életerő, sebességzóna, képesség és pont kijelzésének összefogása. |
| `ProgressBarPresenter` | A `ProgressTracker` adatát megjeleníti. |
| `SpeedGaugePresenter` | A sebességet és a fontos küszöböket mutatja. |
| `RunResultPresenter` | Győzelem/vereség, jutalmak és újrapróbálás. |
| `CharacterSelectPresenter` | Karakterek képességei, statjai és kiválasztása. |

Az UI classok ne olvassák minden képkockán közvetlenül a játék összes komponensét. Eseményekből vagy ritkított frissítésből dolgozzanak.

## 6. Szükséges eszközök

### Már rendelkezésre áll a projektben

- **Unity 6.4** – játékmotor.
- **Universal Render Pipeline (URP)** – low-poly, PC- és mobilbarát renderelés.
- **Unity Input System** – billentyűzet, kontroller és érintés egységes kezelése.
- **Unity Test Framework** – sebességképletek, generálási szabályok és mentés automatikus tesztjeihez.
- **Visual Studio vagy JetBrains Rider** – C# fejlesztés és hibakeresés.
- **Git + Git LFS** – verziókezelés nagy bináris assetekkel.

### Unityn belül javasolt

- **Cinemachine** – kameraütés, FOV és követés; csak ha tényleg szükséges, a prototípushoz egy egyszerű saját kamera is elég.
- **Shader Graph** – szélcsík, mélységi köd, sebességtorzítás és biome átmenetek.
- **Particle System vagy VFX Graph** – PC-n gazdagabb effekt; mobilon egyszerű Particle System profil.
- **Unity Profiler, Frame Debugger, Memory Profiler** – rendszeres teljesítménymérés.
- **Addressables** – csak akkor, amikor már sok biome/karakter assetjét kell külön tölteni; az első prototípushoz felesleges.

### Tartalomkészítés

- **Blender** – low-poly környezet, karakterek és akadályok.
- **Krita, Photoshop vagy Affinity Photo** – rajzolt textúrák, UI és portrék.
- **Aseprite** – opcionális, ha sprite-os vagy pixeles UI-effektek is készülnek.
- **Audacity vagy Reaper** – hangvágás és hangkeverés.
- **Figma** – opcionális UI-tervezés; az első prototípus papíron vagy közvetlenül Unityben is megtervezhető.

### Projektvezetés és minőség

- Egyszerű Kanban tábla: Backlog → Következő → Folyamatban → Teszt → Kész.
- Minden új mechanikához rövid elfogadási feltétel.
- Hetente legalább egy futtatható build.
- Unity Version Control helyett maradhat Git, ha a csapat már ezt ismeri.
- Build automatizálás csak akkor szükséges, amikor rendszeressé válik a külső tesztelés.

## 7. Technikai alapelvek

- A karakter fizikailag ne zuhanjon több kilométert a világ origójától; a világ mozogjon a karakter körül. Ez elkerüli a lebegőpontos pontatlanságot.
- A pályaszeletek, akadályok és effektek használjanak poolingot; menet közben minimális legyen az új objektum létrehozása.
- A sebesség legyen központi, csak olvasható eseményeken keresztül publikált állapot. Ne számolja külön a kamera, az akadály és a UI.
- A beállítások kerüljenek ScriptableObjectekbe, hogy programozás nélkül hangolhatók legyenek.
- A játékszabály és a látvány legyen különválasztva. A szélanimáció hibája ne változtathassa meg a sebességet.
- A procedurális választás legyen reprodukálható seed alapján.
- Már az elején legyen külön PC és Mobile minőségi profil.
- Ne kerüljön hálózat, multiplayer vagy online backend az első verzióba.

## 8. Tesztelési terv

### Automatikusan tesztelendő

- sebesség nem lépi túl a minimumot és maximumot;
- zónaváltás a megfelelő küszöbön történik;
- áttörési küszöb helyesen működik;
- progress érték 0 és 1 között marad;
- azonos seed azonos szeletsorrendet ad;
- a generátor nem rak egymás után tiltott szeleteket;
- mentés és betöltés megtartja a feloldásokat.

### Játékosteszten mérendő

- érti-e a játékos a sebesség előnyeit és veszélyeit;
- észreveszi-e időben az akadályt;
- kontrollálhatónak érzi-e a karaktert gyors állapotban;
- hol hal meg, és ezt igazságosnak érzi-e;
- mennyi idő után szeretné újraindítani vagy abbahagyni;
- melyik sebességtartományt használja, és van-e domináns stratégia.

## 9. Első backlog – prioritási sorrendben

1. Üres gameplay scene és tesztkamera.
2. `FallSpeedConfig` és `FallSpeedController`.
3. `PlayerInputReader` és `PlayerMotor`.
4. Fejlesztői sebességkijelzés.
5. Egy statikus tesztcső három egyszerű kapuval.
6. Sebességfüggő FOV, szél és hang.
7. `PitSlice`, majd három újrahasznosuló szelet.
8. Általános objektumpool.
9. Egy kerülendő és egy áttörhető akadály.
10. Életerő, halál és újrakezdés.
11. Mélységszámítás és progress csík.
12. Egy pickup és egy power-up.
13. Egy teljes 2–3 perces tesztpálya.
14. Első játékosteszt és a sebességmechanika újrahangolása.
15. Csak ezután: karakterképesség és alap RPG statok.

## 10. Kerülendő korai túltervezés

Az első játszható verzióhoz még nem szükséges:

- sok karakter és teljes kasztrendszer;
- hat külön biome;
- bonyolult lootritkaság és tárgyrendszer;
- procedurális pálya minden részletre;
- végleges történet és átvezetők;
- online funkciók;
- hirdetés, bolt vagy monetizáció;
- általános, minden jövőbeli esetre alkalmas framework.

Az első fontos mérföldkő nem a tartalom mennyisége, hanem annak bizonyítása, hogy a játékos a zuhanási sebességgel érdekes és jól olvasható döntéseket hoz.
