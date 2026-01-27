-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2026. Jan 27. 10:12
-- Kiszolgáló verziója: 10.4.32-MariaDB
-- PHP verzió: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `stringify`
--

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `egyedi_gitar`
--

CREATE TABLE `egyedi_gitar` (
  `Id` int(11) NOT NULL,
  `FelhasznaloId` int(11) DEFAULT NULL,
  `BodyShapeId` int(11) NOT NULL,
  `BodyWoodId` int(11) NOT NULL,
  `NeckWoodId` int(11) NOT NULL,
  `NeckPickupId` int(11) DEFAULT NULL,
  `MiddlePickupId` int(11) DEFAULT NULL,
  `BridgePickupId` int(11) DEFAULT NULL,
  `FinishId` int(11) DEFAULT NULL,
  `PickguardId` int(11) DEFAULT NULL,
  `Letrehozva` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- A nézet helyettes szerkezete `egyedi_gitar_ar`
-- (Lásd alább az aktuális nézetet)
--
CREATE TABLE `egyedi_gitar_ar` (
`EgyediGitarId` int(11)
,`OsszAr` bigint(18)
);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `felhasznalo`
--

CREATE TABLE `felhasznalo` (
  `Id` int(11) NOT NULL,
  `Nev` varchar(100) NOT NULL,
  `Email` varchar(255) NOT NULL,
  `Jelszo` varchar(255) NOT NULL,
  `Telefonszam` varchar(30) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `gitar_finish`
--

CREATE TABLE `gitar_finish` (
  `Id` int(11) NOT NULL,
  `SzinNev` varchar(50) DEFAULT NULL,
  `SzinKod` varchar(10) DEFAULT NULL,
  `Ar` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `gitar_nyak_fa`
--

CREATE TABLE `gitar_nyak_fa` (
  `Id` int(11) NOT NULL,
  `Nev` varchar(50) NOT NULL,
  `Leiras` varchar(255) DEFAULT NULL,
  `Ar` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `gitar_pickguard`
--

CREATE TABLE `gitar_pickguard` (
  `Id` int(11) NOT NULL,
  `SzinNev` varchar(50) DEFAULT NULL,
  `SzinKod` varchar(10) DEFAULT NULL,
  `Ar` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `gitar_pickup`
--

CREATE TABLE `gitar_pickup` (
  `Id` int(11) NOT NULL,
  `Nev` varchar(50) NOT NULL,
  `CoilCount` int(11) NOT NULL,
  `Aktiv` tinyint(1) NOT NULL,
  `Ar` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `gitar_testforma`
--

CREATE TABLE `gitar_testforma` (
  `Id` int(11) NOT NULL,
  `Nev` varchar(50) NOT NULL,
  `Leiras` varchar(255) DEFAULT NULL,
  `Ar` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `gitar_test_fa`
--

CREATE TABLE `gitar_test_fa` (
  `Id` int(11) NOT NULL,
  `Nev` varchar(50) NOT NULL,
  `Leiras` varchar(255) DEFAULT NULL,
  `Ar` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `rendeles`
--

CREATE TABLE `rendeles` (
  `Id` int(11) NOT NULL,
  `FelhasznaloId` int(11) NOT NULL,
  `Osszeg` int(11) NOT NULL,
  `Status` varchar(30) NOT NULL,
  `Datum` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `rendeles_tetel`
--

CREATE TABLE `rendeles_tetel` (
  `Id` int(11) NOT NULL,
  `RendelesId` int(11) NOT NULL,
  `TermekId` int(11) DEFAULT NULL,
  `EgyediGitarId` int(11) DEFAULT NULL,
  `Darab` int(11) NOT NULL DEFAULT 1,
  `Ar` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `termek`
--

CREATE TABLE `termek` (
  `Id` int(11) NOT NULL,
  `Nev` varchar(150) NOT NULL,
  `Leiras` text NOT NULL,
  `RovidLeiras` varchar(255) DEFAULT NULL,
  `Ar` int(11) NOT NULL,
  `Elerheto` tinyint(1) NOT NULL,
  `Letrehozva` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `termek`
--

INSERT INTO `termek` (`Id`, `Nev`, `Leiras`, `RovidLeiras`, `Ar`, `Elerheto`, `Letrehozva`) VALUES
(1, 'EVH Frankie Striped MN Red/White/Black', 'Eddie Van Halen ikonikus és tisztelt Frankenstein ™ gitárja, amelyet széles körben az eddigi legelismertebb elektromos gitárnak tartanak, most olyan árcédulával áll elő, amelyet minden zenész megengedhet magának az EVH® Striped Series Frankie formájában.\r\nJellemzője a híres vörös szín, fehér-fekete csíkokkal és kopott, jellegzetes megjelenéssel, hársfából készült test, Stratocaster® alakú, grafittal megerősített juharnyakkal összekötve. A nyak hátsó részén található lakkozott felület sok órányi kényelmes játékot tesz lehetővé, míg a 22 bundával ellátott juhar fogólapot a gyors és dühös játékstílusokhoz tervezték.\r\n', '\r\nTest: Amerikai hárs\r\nNyak: Juharfa\r\nFogólap: Juharfa\r\nHúrláb hangszedő: Direct Mount EVH Wolfgang Humbucking\r\nKözépső hangszedő: Nem\r\nNyaki hangszedő: Dummy Strat Pickup', 596800, 1, '2026-01-27 09:34:39'),
(2, 'Fender American Professional II Stratocaster RW HSS Dark Night', 'Az American Professional II Stratocaster® HSS több mint hatvan éves innovációra, inspirációra és fejlesztésre támaszkodik, hogy megfeleljen a mai szorgalmas játékos igényeinek. Kedvenc \"C\" mély nyakunknak most sima, hengerelt fogólap élei vannak, \"természetes\" szatén felülettel és egy új alakú nyak sarokkal, amely rendkívül kényelmes érzetet és könnyű hozzáférést biztosít a felső nyilvántartáshoz. Az új egytekercses V-Mod II Stratocaster hangszedők minden eddiginél jobban tagoltak, de megtartják a csengést és a meleget, míg a Double Tap-Bridge hangszedők egy gombnyomással elárasztják a dübörgő hangokat és a kalibrált egytekercses hangokat. A továbbfejlesztett 2 pontos tremolo hidegen hengerelt acélblokkal növeli a fenntarthatóságot, a tisztaságot és a felső szikrát. Az American Pro II Stratocaster HSS azonnali tudást és szonikus sokoldalúságot nyújt, amelyet azonnal érezni és hallani fog, kiterjedt fejlesztésekkel, amelyek nem hoznak mást, mint egy új szabványt a professzionális hangszerek számára.', 'Test: Égerfa\r\nNyak: Juharfa\r\nFogólap: Rózsafa\r\nHúrláb hangszedő: V-Mod II Double Tap Humbucking\r\nKözépső hangszedő: V-Mod II Single-Coil Strat\r\nNyaki hangszedő: V-Mod II Single-Coil Strat', 818700, 1, '2026-01-27 09:36:18'),
(3, 'Fender American Professional II Telecaster RW Dark Night', 'Az American Professional II Telecaster több mint hetven éves innovációra, inspirációra és fejlesztésre támaszkodik, hogy megfeleljen a mai keményen dolgozó játékosok igényeinek. A népszerű Deep \"C\" nyak immár sima hengerelt fogólap élekkel, \"Super-Natural\" szatén felülettel és egy új formájú nyaklábbal rendelkezik a rendkívül kényelmes érzés és a felső regiszter könnyű elérése érdekében. Az új, egytekercses V-Mod II Telecaster hangszedők minden eddiginél artikuláltabbak, miközben azt a csípést, csattanást és morgást adják, amelyről a Tele híres. A kompenzált \"golyós\" nyergekkel ellátott új felülterheléses/húros híd az eddigi legkényelmesebb és legrugalmasabb Fender Tele híd – a klasszikus sárgaréz nyereg megtartása mellett kiváló intonációt és rugalmas beállítási lehetőségeket biztosít, lehetővé téve a feszültség és a feszültség testreszabását. minden húr hangszíne ízlése szerint . A Telecaster American Pro II azonnali ismertséget és hangzási sokoldalúságot biztosít, amelyet azonnal érezhet és hallhat, a kiterjedt fejlesztésekkel, amelyek nem kevesebbet képviselnek, mint a professzionális hangszerek új szabványát.', 'Test: Égerfa\r\nNyak: Juharfa\r\nFogólap: Rózsafa\r\nFedlap: Nem\r\nHúrláb hangszedő: V-Mod II Single-Coil Tele\r\nNyaki hangszedő: V-Mod II Single-Coil Tele', 809200, 1, '2026-01-27 09:38:00'),
(4, 'Fender American Vintage II 1975 Telecaster Deluxe MN Black', 'A korhű testekkel, nyakkal és hardverekkel, prémium kivitelezéssel és precíz hangzású, évre jellemző hangszedőkkel készült minden hangszer az autentikus Fender kivitelezés és hangnem esszenciáját tükrözi.\r\nAz eredetileg 1972-ben bemutatott és a Telecaster® család csúcsmodelljeként emlegetett Telecaster Deluxe a Fender első szilárd testű, kettős humbucker hangszedővel rendelkező telecasterje. Az eredetihez hasonlóan az American Vintage II 1975 Telecaster Deluxe egy nagy Stratocaster®-stílusú fejtartóval, kényelmes haskivágással, egy pár széles hatótávolságú humbucker hangszedővel, kettős hangerő- és hangszabályzóval ellátott kiterjesztett hangszedőre szerelve, valamint egy háromutas kapcsoló a felső részeken található. Az autentikus szélessávú humbucker hangszedők létrehozása érdekében helyreállították az 1981 óta gyártásból kivont CuNiFe mágneseket, ami elengedhetetlen eleme az eredeti hangzás visszaállításának.\r\nAmerican Vintage II 1975 Telecaster Deluxe 75 \"C\" alakú juhar nyakkal, 9,5\" sugarú juhar fogólappal és közepes jumbo fogólappal. 1975-ben készült, kényelmes \"Bullet\" stílusú fogólappal és három csavaros nyaklappal. Micro-Tilt™ mechanizmus.Fekete, mokkabarna vagy 3-Color Sunburst kivitelben, égertesten. A további jellemzők közé tartozik a 6 nyerges keményfarkú híd rozsdamentes acél tömbnyeregekkel és a régi Tele® Deluxe tunerekkel.\r\nAz American Vintage II sorozat hangszerei az eredeti Fenderek közvetlen leszármazottai: olyan játékosok számára készültek, akik érzik a vintage Fender hangot és reakciót, és az utolsó csavarig páratlan minőségben készültek. Ezek a legtisztább Fender elektromos készülékek.', 'Test: Égerfa\r\nNyak: Juharfa\r\nFogólap: Juharfa\r\nFedlap: Nem\r\nHúrláb hangszedő: Authentic CuNiFe Wide-Range Humbucking\r\nNyaki hangszedő: Authentic CuNiFe Wide-Range Humbucking', 973000, 1, '2026-01-27 09:56:22'),
(5, 'Fender Jimmy Page Telecaster RW Natural', 'Amikor 1969-ben a \"Good Times Bad Times\" nyitóriffje megjelent a rádióban, minden megváltozott. Abban a pillanatban Jimmy Page megerősítette helyét a zenetörténetben, és örökre befolyásolta a populáris zene fejlődését egyetlen gitárral – a Fender Telecasterével. A Fender Jimmy Page Telecaster tisztelgés e gitár előtt, amely White Blonde gyári fényezéssel kezdte útját, később tükrös gitárrá alakult át, és végül elnyerte végső formáját – egy egyedi hangszer, amelyet maga Page maga festett kézzel. aki megalkotta a 20. század legikonikusabb riffjeit.\r\nA Jimmy Page Telecaster hamutestét Page eredeti dizájnja díszíti – zöld, narancssárga, sárga, kék és piros kígyó színes spiráljai a gitár körül. A Fender Jimmy Page Custom Tele egytekercses hangszedő párja biztosítja azt a tüzes, teljes hangzást, amely a Led Zeppelin első albumának felejthetetlen riffjeit ihlette. Az egyedi „Oval C” profilú juhar nyakat 7,25” sugarú rózsafa fogólap és 21 vintage stílusú szalag egészíti ki a gördülékeny játékélmény érdekében. kerekebb, melegebb tónus és alacsonyabb feszültség, ami megkönnyíti a húrok hajlítását. A további jellemzők közé tartozik a csont nulla feszítő és a vintage hangológépek stílusok, amelyek garantálják az autentikus megjelenést és teljesítményt.\r\nA magával Page-vel együttműködve készített Jimmy Page Telecaster olyan személyes vonásokkal is rendelkezik, mint Jimmy Page aláírása a gitár fejrészén és egyedi krómozott nyaklemez, egy piros tekercses kábel és egy vintage stílusú fehér biztonsági öv. Jimmy ezt mondta szeretett Telecasteréről: \"Tényleg magamévá tettem – ez egy igazán varázslatos gitár.\"', 'Test: Kőris\r\nNyak: Juharfa\r\nFogólap: Rózsafa\r\nFedlap: Nem\r\nHúrláb hangszedő: Jimmy Page Custom Tele\r\nNyaki hangszedő: Jimmy Page Custom Tele', 688700, 1, '2026-01-27 09:57:40'),
(6, 'Fender Limited Edition Tom Delonge Stratocaster Daphne Blue', 'A Tom DeLonge Stratocaster Seymour Duncan Invader humbuckerrel büszkélkedhet a masszív teljesítmény érdekében, és egy felturbózott hangszínnel, amely tökéletes a vaskos power akkordokhoz és ropogós riffekhez. De ez nem csak a teljesítményen múlik – az egyszerűsített vezérlési beállítás, amely egyetlen master hangerőből és „Treble Bleed” áramkörből áll, megőrzi a gitár természetes magas hangtartományát, garantálva a csillogó tiszta hangokat és a kiméra hangokat a hanyatlás határán. A kényelmes, modern „C” nyakon egy 9,5” sugarú rózsafa fogólapon közepes jumbo szalagok találhatók a modern érzésért, kiváló játszhatóságért. További jellemzők a vintage stílusú tuninggépek és a keményfarkú Stratocaster híd blokknyeregekkel a klasszikus megjelenésért, kiváló intonációért és megnövelt hangolási stabilitás Egy egyedi készítésű Tom Delonge nyaklapot Delonge eredeti alkotásai díszítenek.', 'Test: Égerfa\r\nNyak: Juharfa\r\nFogólap: Slab Rosewood\r\nHúrláb hangszedő: Seymour Duncan Invader\r\nKözépső hangszedő: Nem\r\nNyaki hangszedő: Nem', 522600, 1, '2026-01-27 09:58:41'),
(7, 'Fender Player II Series Jaguar RW Polar White', 'A Player II Jaguar a Fender időtlen stílusát árasztja, ugyanakkor mindent kínál, amire a modern játékosoknak szüksége van. A nyak körül mindent a gyors és zökkenőmentes játszhatóságra terveztek, a modern \"C\" profiltól a selymes szatén uretán bevonattal a hátoldalon a kényelmes, 9,5 hüvelykes rózsafa fogólapig, sima hengerelt élekkel és 22 közepes jumbo szalaggal. A klasszikus égertest időtlen kivitelben és a Fender archívumából előkerült soha nem látott színekben kapható. A Jaguar Alnico V (Bridge) és Alnico II (Neck) Single-Coil hangszedői kristályos magas hangokat, zenei középeket és erőteljes basszust kínálnak, amelyek bármilyen műfajt kiemelnek. A 3 állású hangszedő kapcsoló segítségével könnyedén elérhet mindenféle hangot a nyaki hangszedő üveges csengőhangjától a híd hangszedő vágóhangjaiig és minden között, míg a Jaguar 6 nyerges híd úszó tremolóval, továbbfejlesztett Mustang a nyergek és a ClassicGear tunerek precíz hangolást biztosítanak a végtelen hangzási lehetőségek rugalmas felfedezéséhez.', 'Test: Égerfa\r\nNyak: Juharfa\r\nFogólap: Slab Rosewood\r\nHúrláb hangszedő: Player Series Alnico 5 Jaguar Single-Coil\r\nNyaki hangszedő: Player Series Alnico 2 Jaguar Single-Coil\r\nHúrláb/Tremoló: Jaguar Bridge with Mustang Saddles and Vintage Style \"Floating\" T', 322200, 1, '2026-01-27 09:59:46'),
(8, 'Fender Standard Stratocaster LRL 3-Color Sunburst', 'A játékosokat a formatív zenei kalandok kísérésére tervezték, a Fender Standard Stratocaster elérhető játszhatóságot és inspiráló hangzást kínál, amelyek a Fendert rock & roll ikonná tették. A nagy teljesítményű Fender Standard hangszedőkkel és a kényelmes \"Modern C\" nyakformával a kiváló játékkomfort érdekében ez a Stratocaster valóban mércét állít a Fender élményben.', 'Test: Nyárfa\r\nNyak: Juharfa\r\nFogólap: Indian Laurel\r\nHúrláb hangszedő: Standard Single-Coil Strat\r\nKözépső hangszedő: Standard Single-Coil Strat\r\nNyaki hangszedő: Standard Single-Coil Strat', 261700, 1, '2026-01-27 10:00:52'),
(9, 'Gibson ES-335 60s Cherry', 'A Gibson ES-335 DOT a Gibson ES termékcsalád sarokköve. 1958-as első bemutatása óta a Gibson ES-335 páratlan mércét állított fel. A gyöngypöttyös rózsafa fogólap lekerekített \"C\" mahagóni nyakán emlékezteti a játékosokat, hol kezdődött minden. A kalibrált Gibson T-Type humbucker hangszedők kézi vezetékes elektronikánkkal párosulnak. Az eredmény a sokoldalú Gibson ES hangszín, amelyre a játékosok több mint 60 éve vágynak. A hangolási stabilitásról és a pontos intonációról a Keystone alakú gombokkal ellátott Vintage Deluxe tunerek gondoskodnak.', 'Hátlap és oldallap: 3-ply Maple/Poplar/Maple\r\nFedlap: 3-ply Maple/Poplar/Maple\r\nNyak: Mahagóni\r\nFogólap: Rózsafa\r\nHangszedő konfiguráció: HH\r\nHúrláb/Tremoló: ABR-1 Tune-O-Matic', 1212000, 1, '2026-01-27 10:02:00'),
(10, 'Gibson Les Paul Standard 60s Bourbon Burst', 'Elektromos gitár a Gibson Les Paul Standard 60-as sorozatából. A Les Paul Standard új verziója visszatér a klasszikus ikonikus designhoz. Teljes és gazdag hangot nyújt, amely évtizedekig a zenefajták széles skálájára alkalmas. A gitár a klasszikus Gibson modell folytatása.', 'Test: Mahagóni\r\nFedlap: AA Figured Maple\r\nNyak: Mahagóni\r\nFogólap: Rózsafa\r\nHúrláb hangszedő: 60s Burstbucker\r\nNyaki hangszedő: 60s Burstbucker', 978420, 1, '2026-01-27 10:02:46');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `termek_kepek`
--

CREATE TABLE `termek_kepek` (
  `Id` int(11) NOT NULL,
  `TermekId` int(11) NOT NULL,
  `kep1` varchar(255) NOT NULL,
  `kep2` varchar(255) NOT NULL,
  `kep3` varchar(255) NOT NULL,
  `kep4` varchar(255) NOT NULL,
  `kep5` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `termek_kepek`
--

INSERT INTO `termek_kepek` (`Id`, `TermekId`, `kep1`, `kep2`, `kep3`, `kep4`, `kep5`) VALUES
(1, 1, 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F1%2Fkep1.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F1%2Fkep2.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F1%2Fkep3.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F1%2Fkep4.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F1%2Fkep5.jpg'),
(2, 2, 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F2%2Fkep1.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F2%2Fkep2.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F2%2Fkep3.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F2%2Fkep4.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F2%2Fkep5.jpg'),
(3, 3, 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F3%2Fkep1.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F3%2Fkep2.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F3%2Fkep3.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F3%2Fkep4.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F3%2Fkep5.jpg'),
(4, 4, 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F4%2Fkep1.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F4%2Fkep2.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F4%2Fkep3.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F4%2Fkep4.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F4%2Fkep5.jpg'),
(5, 5, 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F5%2Fkep1.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F5%2Fkep2.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F5%2Fkep3.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F5%2Fkep4.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F5%2Fkep5.jpg'),
(6, 6, 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F6%2Fkep1.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F6%2Fkep2.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F6%2Fkep3.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F6%2Fkep4.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F6%2Fkep5.jpg'),
(7, 7, 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F7%2Fkep1.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F7%2Fkep2.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F7%2Fkep3.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F7%2Fkep4.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F7%2Fkep5.jpg'),
(8, 8, 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F8%2Fkep1.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F8%2Fkep2.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F8%2Fkep3.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F8%2Fkep4.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F8%2Fkep5.jpg'),
(9, 9, 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F9%2Fkep1.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F9%2Fkep2.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F9%2Fkep3.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F9%2Fkep4.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F9%2Fkep5.jpg'),
(10, 10, 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F10%2Fkep1.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F10%2Fkep2.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F10%2Fkep3.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F10%2Fkep4.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F10%2Fkep5.jpg');

-- --------------------------------------------------------

--
-- Nézet szerkezete `egyedi_gitar_ar`
--
DROP TABLE IF EXISTS `egyedi_gitar_ar`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `egyedi_gitar_ar`  AS SELECT `eg`.`Id` AS `EgyediGitarId`, `gt`.`Ar`+ `tf`.`Ar` + `nf`.`Ar` + ifnull(`np`.`Ar`,0) + ifnull(`mp`.`Ar`,0) + ifnull(`bp`.`Ar`,0) + ifnull(`f`.`Ar`,0) + ifnull(`pg`.`Ar`,0) AS `OsszAr` FROM ((((((((`egyedi_gitar` `eg` join `gitar_testforma` `gt` on(`eg`.`BodyShapeId` = `gt`.`Id`)) join `gitar_test_fa` `tf` on(`eg`.`BodyWoodId` = `tf`.`Id`)) join `gitar_nyak_fa` `nf` on(`eg`.`NeckWoodId` = `nf`.`Id`)) left join `gitar_pickup` `np` on(`eg`.`NeckPickupId` = `np`.`Id`)) left join `gitar_pickup` `mp` on(`eg`.`MiddlePickupId` = `mp`.`Id`)) left join `gitar_pickup` `bp` on(`eg`.`BridgePickupId` = `bp`.`Id`)) left join `gitar_finish` `f` on(`eg`.`FinishId` = `f`.`Id`)) left join `gitar_pickguard` `pg` on(`eg`.`PickguardId` = `pg`.`Id`)) ;

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `egyedi_gitar`
--
ALTER TABLE `egyedi_gitar`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `FelhasznaloId` (`FelhasznaloId`),
  ADD KEY `BodyShapeId` (`BodyShapeId`),
  ADD KEY `BodyWoodId` (`BodyWoodId`),
  ADD KEY `NeckWoodId` (`NeckWoodId`),
  ADD KEY `NeckPickupId` (`NeckPickupId`),
  ADD KEY `MiddlePickupId` (`MiddlePickupId`),
  ADD KEY `BridgePickupId` (`BridgePickupId`),
  ADD KEY `FinishId` (`FinishId`),
  ADD KEY `PickguardId` (`PickguardId`);

--
-- A tábla indexei `felhasznalo`
--
ALTER TABLE `felhasznalo`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `Email` (`Email`);

--
-- A tábla indexei `gitar_finish`
--
ALTER TABLE `gitar_finish`
  ADD PRIMARY KEY (`Id`);

--
-- A tábla indexei `gitar_nyak_fa`
--
ALTER TABLE `gitar_nyak_fa`
  ADD PRIMARY KEY (`Id`);

--
-- A tábla indexei `gitar_pickguard`
--
ALTER TABLE `gitar_pickguard`
  ADD PRIMARY KEY (`Id`);

--
-- A tábla indexei `gitar_pickup`
--
ALTER TABLE `gitar_pickup`
  ADD PRIMARY KEY (`Id`);

--
-- A tábla indexei `gitar_testforma`
--
ALTER TABLE `gitar_testforma`
  ADD PRIMARY KEY (`Id`);

--
-- A tábla indexei `gitar_test_fa`
--
ALTER TABLE `gitar_test_fa`
  ADD PRIMARY KEY (`Id`);

--
-- A tábla indexei `rendeles`
--
ALTER TABLE `rendeles`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `FelhasznaloId` (`FelhasznaloId`);

--
-- A tábla indexei `rendeles_tetel`
--
ALTER TABLE `rendeles_tetel`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `RendelesId` (`RendelesId`),
  ADD KEY `TermekId` (`TermekId`),
  ADD KEY `EgyediGitarId` (`EgyediGitarId`);

--
-- A tábla indexei `termek`
--
ALTER TABLE `termek`
  ADD PRIMARY KEY (`Id`);

--
-- A tábla indexei `termek_kepek`
--
ALTER TABLE `termek_kepek`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `TermekId` (`TermekId`);

--
-- A kiírt táblák AUTO_INCREMENT értéke
--

--
-- AUTO_INCREMENT a táblához `egyedi_gitar`
--
ALTER TABLE `egyedi_gitar`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `felhasznalo`
--
ALTER TABLE `felhasznalo`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `gitar_finish`
--
ALTER TABLE `gitar_finish`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `gitar_nyak_fa`
--
ALTER TABLE `gitar_nyak_fa`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `gitar_pickguard`
--
ALTER TABLE `gitar_pickguard`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `gitar_pickup`
--
ALTER TABLE `gitar_pickup`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `gitar_testforma`
--
ALTER TABLE `gitar_testforma`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `gitar_test_fa`
--
ALTER TABLE `gitar_test_fa`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `rendeles`
--
ALTER TABLE `rendeles`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `rendeles_tetel`
--
ALTER TABLE `rendeles_tetel`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `termek`
--
ALTER TABLE `termek`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT a táblához `termek_kepek`
--
ALTER TABLE `termek_kepek`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- Megkötések a kiírt táblákhoz
--

--
-- Megkötések a táblához `egyedi_gitar`
--
ALTER TABLE `egyedi_gitar`
  ADD CONSTRAINT `egyedi_gitar_ibfk_1` FOREIGN KEY (`FelhasznaloId`) REFERENCES `felhasznalo` (`Id`),
  ADD CONSTRAINT `egyedi_gitar_ibfk_2` FOREIGN KEY (`BodyShapeId`) REFERENCES `gitar_testforma` (`Id`),
  ADD CONSTRAINT `egyedi_gitar_ibfk_3` FOREIGN KEY (`BodyWoodId`) REFERENCES `gitar_test_fa` (`Id`),
  ADD CONSTRAINT `egyedi_gitar_ibfk_4` FOREIGN KEY (`NeckWoodId`) REFERENCES `gitar_nyak_fa` (`Id`),
  ADD CONSTRAINT `egyedi_gitar_ibfk_5` FOREIGN KEY (`NeckPickupId`) REFERENCES `gitar_pickup` (`Id`),
  ADD CONSTRAINT `egyedi_gitar_ibfk_6` FOREIGN KEY (`MiddlePickupId`) REFERENCES `gitar_pickup` (`Id`),
  ADD CONSTRAINT `egyedi_gitar_ibfk_7` FOREIGN KEY (`BridgePickupId`) REFERENCES `gitar_pickup` (`Id`),
  ADD CONSTRAINT `egyedi_gitar_ibfk_8` FOREIGN KEY (`FinishId`) REFERENCES `gitar_finish` (`Id`),
  ADD CONSTRAINT `egyedi_gitar_ibfk_9` FOREIGN KEY (`PickguardId`) REFERENCES `gitar_pickguard` (`Id`);

--
-- Megkötések a táblához `rendeles`
--
ALTER TABLE `rendeles`
  ADD CONSTRAINT `rendeles_ibfk_1` FOREIGN KEY (`FelhasznaloId`) REFERENCES `felhasznalo` (`Id`);

--
-- Megkötések a táblához `rendeles_tetel`
--
ALTER TABLE `rendeles_tetel`
  ADD CONSTRAINT `rendeles_tetel_ibfk_1` FOREIGN KEY (`RendelesId`) REFERENCES `rendeles` (`Id`),
  ADD CONSTRAINT `rendeles_tetel_ibfk_2` FOREIGN KEY (`TermekId`) REFERENCES `termek` (`Id`),
  ADD CONSTRAINT `rendeles_tetel_ibfk_3` FOREIGN KEY (`EgyediGitarId`) REFERENCES `egyedi_gitar` (`Id`);

--
-- Megkötések a táblához `termek_kepek`
--
ALTER TABLE `termek_kepek`
  ADD CONSTRAINT `termek_kepek_ibfk_1` FOREIGN KEY (`TermekId`) REFERENCES `termek` (`Id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
