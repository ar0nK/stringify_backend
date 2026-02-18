-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2026. Feb 18. 11:19
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
CREATE DATABASE IF NOT EXISTS `stringify` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_hungarian_ci;
USE `stringify`;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `egyedi_gitar`
--

CREATE TABLE `egyedi_gitar` (
  `Id` int(11) NOT NULL,
  `FelhasznaloId` int(11) DEFAULT NULL,
  `TestformaId` int(11) NOT NULL,
  `FinishId` int(11) DEFAULT NULL,
  `PickguardId` int(11) DEFAULT NULL,
  `NeckId` int(11) NOT NULL,
  `Letrehozva` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- A nézet helyettes szerkezete `egyedi_gitar_ar`
-- (Lásd alább az aktuális nézetet)
--
CREATE TABLE `egyedi_gitar_ar` (
`EgyediGitarId` int(11)
,`OsszAr` bigint(14)
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
  `SALT` varchar(64) NOT NULL,
  `Jogosultsag` tinyint(1) NOT NULL,
  `Aktiv` int(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `felhasznalo`
--

INSERT INTO `felhasznalo` (`Id`, `Nev`, `Email`, `Jelszo`, `SALT`, `Jogosultsag`, `Aktiv`) VALUES
(1, 'teszt teszt', 'teszt@gmail.com', 'c74fc60fe5a0914e84da66d74cc483e6a9eea362780166f0d79ecda3902bc2dd', 'UAVUnYeQ5reKKUtPJ3reu6JBZeUwNskm1Ck65wXGnt38uQSKjgS1RC09LmQegzMK', 1, 1),
(2, 'teszt teszt2', 'teszt2@gmail.com', 'e734b8206e82fe0341278618ba053503bae0fee25a024e87986bd0aa5b1024d8', 'jXmrZZVPFoDIiuVINUTn2Pr7GjZy1YBjdDTxv23CeBRbVrDZyahOUl1Pjd3jOXNY', 1, 1);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `gitar_finish`
--

CREATE TABLE `gitar_finish` (
  `Id` int(11) NOT NULL,
  `Nev` varchar(50) NOT NULL,
  `KepUrl` varchar(255) NOT NULL,
  `Ar` int(11) NOT NULL,
  `TestFormaId` int(11) NOT NULL,
  `ZIndex` int(11) NOT NULL DEFAULT 10
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `gitar_finish`
--

INSERT INTO `gitar_finish` (`Id`, `Nev`, `KepUrl`, `Ar`, `TestFormaId`, `ZIndex`) VALUES
(1, 'Black ', 'https://cdn.synk.hu/stringify/Egyedi_gitar%2FStratocaster%2FBody%2Fblack_body.png', 40000, 1, 10),
(2, 'Red', 'https://cdn.synk.hu/stringify/Egyedi_gitar%2FStratocaster%2FBody%2Fred_body.png', 50000, 1, 10),
(3, 'Sunburst', 'https://cdn.synk.hu/stringify/Egyedi_gitar%2FStratocaster%2FBody%2Fsunburst_body.png', 60000, 1, 10),
(4, 'Black', 'https://cdn.synk.hu/stringify/Egyedi_gitar%2FTelecaster%2FBody%2Fblack_body.png', 40000, 2, 10),
(5, 'Red', 'https://cdn.synk.hu/stringify/Egyedi_gitar%2FTelecaster%2FBody%2Fred_body.png', 50000, 2, 10),
(6, 'Sunburst', 'https://cdn.synk.hu/stringify/Egyedi_gitar%2FTelecaster%2FBody%2Fsunburst_body.png', 60000, 2, 10);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `gitar_global_elem`
--

CREATE TABLE `gitar_global_elem` (
  `Id` int(11) NOT NULL,
  `Tipus` enum('bridge','headstock') NOT NULL,
  `KepUtvonal` varchar(255) NOT NULL,
  `ZIndex` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `gitar_nyak`
--

CREATE TABLE `gitar_nyak` (
  `Id` int(11) NOT NULL,
  `Nev` varchar(50) NOT NULL,
  `KepUrl` varchar(255) NOT NULL,
  `Ar` int(11) NOT NULL,
  `ZIndex` int(11) NOT NULL DEFAULT 30
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `gitar_nyak`
--

INSERT INTO `gitar_nyak` (`Id`, `Nev`, `KepUrl`, `Ar`, `ZIndex`) VALUES
(1, 'Maple', 'https://cdn.synk.hu/stringify/Egyedi_gitar%2FNecks%2Fneck1.png', 70000, 30),
(2, 'Rosewood', 'https://cdn.synk.hu/stringify/Egyedi_gitar%2FNecks%2Fneck2.png', 75000, 30),
(3, 'Ebony', 'https://cdn.synk.hu/stringify/Egyedi_gitar%2FNecks%2Fneck3.png', 80000, 30);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `gitar_pickguard`
--

CREATE TABLE `gitar_pickguard` (
  `Id` int(11) NOT NULL,
  `Nev` varchar(50) NOT NULL,
  `KepUrl` varchar(255) NOT NULL,
  `Ar` int(11) NOT NULL,
  `TestFormaId` int(11) NOT NULL,
  `ZIndex` int(11) NOT NULL DEFAULT 20
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `gitar_pickguard`
--

INSERT INTO `gitar_pickguard` (`Id`, `Nev`, `KepUrl`, `Ar`, `TestFormaId`, `ZIndex`) VALUES
(1, 'Black', 'https://cdn.synk.hu/stringify/Egyedi_gitar%2FStratocaster%2FPickguard%2Fblack_pickguard.png', 20000, 1, 20),
(2, 'Red', 'https://cdn.synk.hu/stringify/Egyedi_gitar%2FStratocaster%2FPickguard%2Fred_pickguard.png', 30000, 1, 20),
(3, 'White', 'https://cdn.synk.hu/stringify/Egyedi_gitar%2FStratocaster%2FPickguard%2Fwhite_pickguard.png', 25000, 1, 20),
(4, 'Black', 'https://cdn.synk.hu/stringify/Egyedi_gitar%2FTelecaster%2FPickguard%2Fblack_pickguard.png', 20000, 2, 20),
(5, 'Red', 'https://cdn.synk.hu/stringify/Egyedi_gitar%2FTelecaster%2FPickguard%2Fred_pickguard.png', 30000, 2, 20),
(6, 'White', 'https://cdn.synk.hu/stringify/Egyedi_gitar%2FTelecaster%2FPickguard%2Fwhite_pickguard.png', 25000, 2, 20);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `gitar_testforma`
--

CREATE TABLE `gitar_testforma` (
  `Id` int(11) NOT NULL,
  `Nev` varchar(50) NOT NULL,
  `Leiras` varchar(255) DEFAULT NULL,
  `Ar` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `gitar_testforma`
--

INSERT INTO `gitar_testforma` (`Id`, `Nev`, `Leiras`, `Ar`) VALUES
(1, 'Stratocaster', NULL, NULL),
(2, 'Telecaster', NULL, NULL);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `gitar_tipus`
--

CREATE TABLE `gitar_tipus` (
  `Id` int(11) NOT NULL,
  `Nev` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `gitar_tipus`
--

INSERT INTO `gitar_tipus` (`Id`, `Nev`) VALUES
(2, 'Akusztikus'),
(3, 'Basszus'),
(1, 'Elektromos');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `kedvenc_termek`
--

CREATE TABLE `kedvenc_termek` (
  `Id` int(11) NOT NULL,
  `FelhasznaloId` int(11) NOT NULL,
  `TermekId` int(11) NOT NULL,
  `Letrehozva` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `kedvenc_termek`
--

INSERT INTO `kedvenc_termek` (`Id`, `FelhasznaloId`, `TermekId`, `Letrehozva`) VALUES
(1, 1, 2, '2026-02-13 11:00:04');

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
-- Tábla szerkezet ehhez a táblához `rendeles_cimek`
--

CREATE TABLE `rendeles_cimek` (
  `Id` int(11) NOT NULL,
  `FelhasznaloId` int(11) NOT NULL,
  `iranyitoszam` int(4) NOT NULL,
  `varos` varchar(30) NOT NULL,
  `utca_hazszam` varchar(50) NOT NULL,
  `telefonszam` varchar(15) NOT NULL,
  `szamlazasi/szallitasi` int(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `rendeles_tetel`
--

CREATE TABLE `rendeles_tetel` (
  `Id` int(11) NOT NULL,
  `RendelesId` int(11) NOT NULL,
  `TermekId` int(11) DEFAULT NULL,
  `EgyediGitarId` int(11) DEFAULT NULL
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
  `GitarTipusId` int(11) DEFAULT NULL,
  `Letrehozva` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `termek`
--

INSERT INTO `termek` (`Id`, `Nev`, `Leiras`, `RovidLeiras`, `Ar`, `Elerheto`, `GitarTipusId`, `Letrehozva`) VALUES
(1, 'EVH Frankie Striped MN Red/White/Black', 'Eddie Van Halen ikonikus és tisztelt Frankenstein ™ gitárja, amelyet széles körben az eddigi legelismertebb elektromos gitárnak tartanak, most olyan árcédulával áll elő, amelyet minden zenész megengedhet magának az EVH® Striped Series Frankie formájában.\r\nJellemzője a híres vörös szín, fehér-fekete csíkokkal és kopott, jellegzetes megjelenéssel, hársfából készült test, Stratocaster® alakú, grafittal megerősített juharnyakkal összekötve. A nyak hátsó részén található lakkozott felület sok órányi kényelmes játékot tesz lehetővé, míg a 22 bundával ellátott juhar fogólapot a gyors és dühös játékstílusokhoz tervezték.\r\n', '\r\nTest: Amerikai hárs\r\nNyak: Juharfa\r\nFogólap: Juharfa\r\nHúrláb hangszedő: Direct Mount EVH Wolfgang Humbucking\r\nKözépső hangszedő: Nem\r\nNyaki hangszedő: Dummy Strat Pickup', 596800, 1, 1, '2026-01-27 09:34:39'),
(2, 'Fender American Professional II Stratocaster RW HSS Dark Night', 'Az American Professional II Stratocaster® HSS több mint hatvan éves innovációra, inspirációra és fejlesztésre támaszkodik, hogy megfeleljen a mai szorgalmas játékos igényeinek. Kedvenc \"C\" mély nyakunknak most sima, hengerelt fogólap élei vannak, \"természetes\" szatén felülettel és egy új alakú nyak sarokkal, amely rendkívül kényelmes érzetet és könnyű hozzáférést biztosít a felső nyilvántartáshoz. Az új egytekercses V-Mod II Stratocaster hangszedők minden eddiginél jobban tagoltak, de megtartják a csengést és a meleget, míg a Double Tap-Bridge hangszedők egy gombnyomással elárasztják a dübörgő hangokat és a kalibrált egytekercses hangokat. A továbbfejlesztett 2 pontos tremolo hidegen hengerelt acélblokkal növeli a fenntarthatóságot, a tisztaságot és a felső szikrát. Az American Pro II Stratocaster HSS azonnali tudást és szonikus sokoldalúságot nyújt, amelyet azonnal érezni és hallani fog, kiterjedt fejlesztésekkel, amelyek nem hoznak mást, mint egy új szabványt a professzionális hangszerek számára.', 'Test: Égerfa\r\nNyak: Juharfa\r\nFogólap: Rózsafa\r\nHúrláb hangszedő: V-Mod II Double Tap Humbucking\r\nKözépső hangszedő: V-Mod II Single-Coil Strat\r\nNyaki hangszedő: V-Mod II Single-Coil Strat', 818700, 1, 1, '2026-01-27 09:36:18'),
(3, 'Fender American Professional II Telecaster RW Dark Night', 'Az American Professional II Telecaster több mint hetven éves innovációra, inspirációra és fejlesztésre támaszkodik, hogy megfeleljen a mai keményen dolgozó játékosok igényeinek. A népszerű Deep \"C\" nyak immár sima hengerelt fogólap élekkel, \"Super-Natural\" szatén felülettel és egy új formájú nyaklábbal rendelkezik a rendkívül kényelmes érzés és a felső regiszter könnyű elérése érdekében. Az új, egytekercses V-Mod II Telecaster hangszedők minden eddiginél artikuláltabbak, miközben azt a csípést, csattanást és morgást adják, amelyről a Tele híres. A kompenzált \"golyós\" nyergekkel ellátott új felülterheléses/húros híd az eddigi legkényelmesebb és legrugalmasabb Fender Tele híd – a klasszikus sárgaréz nyereg megtartása mellett kiváló intonációt és rugalmas beállítási lehetőségeket biztosít, lehetővé téve a feszültség és a feszültség testreszabását. minden húr hangszíne ízlése szerint . A Telecaster American Pro II azonnali ismertséget és hangzási sokoldalúságot biztosít, amelyet azonnal érezhet és hallhat, a kiterjedt fejlesztésekkel, amelyek nem kevesebbet képviselnek, mint a professzionális hangszerek új szabványát.', 'Test: Égerfa\r\nNyak: Juharfa\r\nFogólap: Rózsafa\r\nFedlap: Nem\r\nHúrláb hangszedő: V-Mod II Single-Coil Tele\r\nNyaki hangszedő: V-Mod II Single-Coil Tele', 809200, 1, 1, '2026-01-27 09:38:00'),
(4, 'Fender American Vintage II 1975 Telecaster Deluxe MN Black', 'A korhű testekkel, nyakkal és hardverekkel, prémium kivitelezéssel és precíz hangzású, évre jellemző hangszedőkkel készült minden hangszer az autentikus Fender kivitelezés és hangnem esszenciáját tükrözi.\r\nAz eredetileg 1972-ben bemutatott és a Telecaster® család csúcsmodelljeként emlegetett Telecaster Deluxe a Fender első szilárd testű, kettős humbucker hangszedővel rendelkező telecasterje. Az eredetihez hasonlóan az American Vintage II 1975 Telecaster Deluxe egy nagy Stratocaster®-stílusú fejtartóval, kényelmes haskivágással, egy pár széles hatótávolságú humbucker hangszedővel, kettős hangerő- és hangszabályzóval ellátott kiterjesztett hangszedőre szerelve, valamint egy háromutas kapcsoló a felső részeken található. Az autentikus szélessávú humbucker hangszedők létrehozása érdekében helyreállították az 1981 óta gyártásból kivont CuNiFe mágneseket, ami elengedhetetlen eleme az eredeti hangzás visszaállításának.\r\nAmerican Vintage II 1975 Telecaster Deluxe 75 \"C\" alakú juhar nyakkal, 9,5\" sugarú juhar fogólappal és közepes jumbo fogólappal. 1975-ben készült, kényelmes \"Bullet\" stílusú fogólappal és három csavaros nyaklappal. Micro-Tilt™ mechanizmus.Fekete, mokkabarna vagy 3-Color Sunburst kivitelben, égertesten. A további jellemzők közé tartozik a 6 nyerges keményfarkú híd rozsdamentes acél tömbnyeregekkel és a régi Tele® Deluxe tunerekkel.\r\nAz American Vintage II sorozat hangszerei az eredeti Fenderek közvetlen leszármazottai: olyan játékosok számára készültek, akik érzik a vintage Fender hangot és reakciót, és az utolsó csavarig páratlan minőségben készültek. Ezek a legtisztább Fender elektromos készülékek.', 'Test: Égerfa\r\nNyak: Juharfa\r\nFogólap: Juharfa\r\nFedlap: Nem\r\nHúrláb hangszedő: Authentic CuNiFe Wide-Range Humbucking\r\nNyaki hangszedő: Authentic CuNiFe Wide-Range Humbucking', 973000, 1, 1, '2026-01-27 09:56:22'),
(5, 'Fender Jimmy Page Telecaster RW Natural', 'Amikor 1969-ben a \"Good Times Bad Times\" nyitóriffje megjelent a rádióban, minden megváltozott. Abban a pillanatban Jimmy Page megerősítette helyét a zenetörténetben, és örökre befolyásolta a populáris zene fejlődését egyetlen gitárral – a Fender Telecasterével. A Fender Jimmy Page Telecaster tisztelgés e gitár előtt, amely White Blonde gyári fényezéssel kezdte útját, később tükrös gitárrá alakult át, és végül elnyerte végső formáját – egy egyedi hangszer, amelyet maga Page maga festett kézzel. aki megalkotta a 20. század legikonikusabb riffjeit.\r\nA Jimmy Page Telecaster hamutestét Page eredeti dizájnja díszíti – zöld, narancssárga, sárga, kék és piros kígyó színes spiráljai a gitár körül. A Fender Jimmy Page Custom Tele egytekercses hangszedő párja biztosítja azt a tüzes, teljes hangzást, amely a Led Zeppelin első albumának felejthetetlen riffjeit ihlette. Az egyedi „Oval C” profilú juhar nyakat 7,25” sugarú rózsafa fogólap és 21 vintage stílusú szalag egészíti ki a gördülékeny játékélmény érdekében. kerekebb, melegebb tónus és alacsonyabb feszültség, ami megkönnyíti a húrok hajlítását. A további jellemzők közé tartozik a csont nulla feszítő és a vintage hangológépek stílusok, amelyek garantálják az autentikus megjelenést és teljesítményt.\r\nA magával Page-vel együttműködve készített Jimmy Page Telecaster olyan személyes vonásokkal is rendelkezik, mint Jimmy Page aláírása a gitár fejrészén és egyedi krómozott nyaklemez, egy piros tekercses kábel és egy vintage stílusú fehér biztonsági öv. Jimmy ezt mondta szeretett Telecasteréről: \"Tényleg magamévá tettem – ez egy igazán varázslatos gitár.\"', 'Test: Kőris\r\nNyak: Juharfa\r\nFogólap: Rózsafa\r\nFedlap: Nem\r\nHúrláb hangszedő: Jimmy Page Custom Tele\r\nNyaki hangszedő: Jimmy Page Custom Tele', 688700, 1, 1, '2026-01-27 09:57:40'),
(6, 'Fender Limited Edition Tom Delonge Stratocaster Daphne Blue', 'A Tom DeLonge Stratocaster Seymour Duncan Invader humbuckerrel büszkélkedhet a masszív teljesítmény érdekében, és egy felturbózott hangszínnel, amely tökéletes a vaskos power akkordokhoz és ropogós riffekhez. De ez nem csak a teljesítményen múlik – az egyszerűsített vezérlési beállítás, amely egyetlen master hangerőből és „Treble Bleed” áramkörből áll, megőrzi a gitár természetes magas hangtartományát, garantálva a csillogó tiszta hangokat és a kiméra hangokat a hanyatlás határán. A kényelmes, modern „C” nyakon egy 9,5” sugarú rózsafa fogólapon közepes jumbo szalagok találhatók a modern érzésért, kiváló játszhatóságért. További jellemzők a vintage stílusú tuninggépek és a keményfarkú Stratocaster híd blokknyeregekkel a klasszikus megjelenésért, kiváló intonációért és megnövelt hangolási stabilitás Egy egyedi készítésű Tom Delonge nyaklapot Delonge eredeti alkotásai díszítenek.', 'Test: Égerfa\r\nNyak: Juharfa\r\nFogólap: Slab Rosewood\r\nHúrláb hangszedő: Seymour Duncan Invader\r\nKözépső hangszedő: Nem\r\nNyaki hangszedő: Nem', 522600, 1, 1, '2026-01-27 09:58:41'),
(7, 'Fender Player II Series Jaguar RW Polar White', 'A Player II Jaguar a Fender időtlen stílusát árasztja, ugyanakkor mindent kínál, amire a modern játékosoknak szüksége van. A nyak körül mindent a gyors és zökkenőmentes játszhatóságra terveztek, a modern \"C\" profiltól a selymes szatén uretán bevonattal a hátoldalon a kényelmes, 9,5 hüvelykes rózsafa fogólapig, sima hengerelt élekkel és 22 közepes jumbo szalaggal. A klasszikus égertest időtlen kivitelben és a Fender archívumából előkerült soha nem látott színekben kapható. A Jaguar Alnico V (Bridge) és Alnico II (Neck) Single-Coil hangszedői kristályos magas hangokat, zenei középeket és erőteljes basszust kínálnak, amelyek bármilyen műfajt kiemelnek. A 3 állású hangszedő kapcsoló segítségével könnyedén elérhet mindenféle hangot a nyaki hangszedő üveges csengőhangjától a híd hangszedő vágóhangjaiig és minden között, míg a Jaguar 6 nyerges híd úszó tremolóval, továbbfejlesztett Mustang a nyergek és a ClassicGear tunerek precíz hangolást biztosítanak a végtelen hangzási lehetőségek rugalmas felfedezéséhez.', 'Test: Égerfa\r\nNyak: Juharfa\r\nFogólap: Slab Rosewood\r\nHúrláb hangszedő: Player Series Alnico 5 Jaguar Single-Coil\r\nNyaki hangszedő: Player Series Alnico 2 Jaguar Single-Coil\r\nHúrláb/Tremoló: Jaguar Bridge with Mustang Saddles and Vintage Style \"Floating\" T', 322200, 1, 1, '2026-01-27 09:59:46'),
(8, 'Fender Standard Stratocaster LRL 3-Color Sunburst', 'A játékosokat a formatív zenei kalandok kísérésére tervezték, a Fender Standard Stratocaster elérhető játszhatóságot és inspiráló hangzást kínál, amelyek a Fendert rock & roll ikonná tették. A nagy teljesítményű Fender Standard hangszedőkkel és a kényelmes \"Modern C\" nyakformával a kiváló játékkomfort érdekében ez a Stratocaster valóban mércét állít a Fender élményben.', 'Test: Nyárfa\r\nNyak: Juharfa\r\nFogólap: Indian Laurel\r\nHúrláb hangszedő: Standard Single-Coil Strat\r\nKözépső hangszedő: Standard Single-Coil Strat\r\nNyaki hangszedő: Standard Single-Coil Strat', 261700, 1, 1, '2026-01-27 10:00:52'),
(9, 'Gibson ES-335 60s Cherry', 'A Gibson ES-335 DOT a Gibson ES termékcsalád sarokköve. 1958-as első bemutatása óta a Gibson ES-335 páratlan mércét állított fel. A gyöngypöttyös rózsafa fogólap lekerekített \"C\" mahagóni nyakán emlékezteti a játékosokat, hol kezdődött minden. A kalibrált Gibson T-Type humbucker hangszedők kézi vezetékes elektronikánkkal párosulnak. Az eredmény a sokoldalú Gibson ES hangszín, amelyre a játékosok több mint 60 éve vágynak. A hangolási stabilitásról és a pontos intonációról a Keystone alakú gombokkal ellátott Vintage Deluxe tunerek gondoskodnak.', 'Hátlap és oldallap: 3-ply Maple/Poplar/Maple\r\nFedlap: 3-ply Maple/Poplar/Maple\r\nNyak: Mahagóni\r\nFogólap: Rózsafa\r\nHangszedő konfiguráció: HH\r\nHúrláb/Tremoló: ABR-1 Tune-O-Matic', 1212000, 1, 1, '2026-01-27 10:02:00'),
(10, 'Gibson Les Paul Standard 60s Bourbon Burst', 'Elektromos gitár a Gibson Les Paul Standard 60-as sorozatából. A Les Paul Standard új verziója visszatér a klasszikus ikonikus designhoz. Teljes és gazdag hangot nyújt, amely évtizedekig a zenefajták széles skálájára alkalmas. A gitár a klasszikus Gibson modell folytatása.', 'Test: Mahagóni\r\nFedlap: AA Figured Maple\r\nNyak: Mahagóni\r\nFogólap: Rózsafa\r\nHúrláb hangszedő: 60s Burstbucker\r\nNyaki hangszedő: 60s Burstbucker', 978420, 1, 1, '2026-01-27 10:02:46'),
(11, 'Fender American Professional II Jazz Bass MN Dark Night', 'Az American Professional II Jazz Bass® több mint hatvan éves innovációra, inspirációra és fejlesztésre támaszkodik, hogy megfeleljen a mai szorgalmas játékos igényeinek. Kedvenc vékony \"C\" nyakunknak most sima gördülő fogólap élei vannak, \"Super-Natural\" szatén felülettel és új formájú nyak sarokkal a rendkívül kényelmes érzés és a könnyű hozzáférés érdekében a felső regiszterhez. Az új Single-coil V-Mod II Jazz Bass hangszedők minden eddiginél fényesebbek, és olyan jellegzetességet és áttekinthetőséget nyújtanak, amelyről a Jazz Bass ismert. Az American Pro II Jazz Bass azonnali tudást és szonikus sokoldalúságot nyújt, amelyet azonnal érezni és hallani fog, kiterjedt fejlesztésekkel, amelyek csak új mércét állítanak a professzionális hangszerek számára.', 'Test: Égerfa\r\nNyak: Juharfa\r\nFogólap: Juharfa\r\nHúrláb hangszedő: V-Mod II Single-Coil Jazz Bass\r\nKözépső hangszedő: V-Mod II Single-Coil Jazz Bass', 835700, 1, 3, '2026-02-17 10:35:04'),
(12, 'Fender American Professional II Jazz Bass RW Mercury', 'Az American Professional II Jazz Bass® több mint hatvan éves innovációra, inspirációra és fejlesztésre támaszkodik, hogy megfeleljen a mai szorgalmas játékos igényeinek. Kedvenc vékony \"C\" nyakunknak most sima gördülő fogólap élei vannak, \"Super-Natural\" szatén felülettel és új formájú nyak sarokkal a rendkívül kényelmes érzés és a könnyű hozzáférés érdekében a felső regiszterhez. Az új Single-coil V-Mod II Jazz Bass hangszedők minden eddiginél fényesebbek, és olyan jellegzetességet és áttekinthetőséget nyújtanak, amelyről a Jazz Bass ismert. Az American Pro II Jazz Bass azonnali tudást és szonikus sokoldalúságot nyújt, amelyet azonnal érezni és hallani fog, kiterjedt fejlesztésekkel, amelyek csak új mércét állítanak a professzionális hangszerek számára.', 'Test: Égerfa\r\nNyak: Juharfa\r\nFogólap: Rózsafa\r\nHúrláb hangszedő: V-Mod II Single-Coil Jazz Bass\r\nKözépső hangszedő: V-Mod II Single-Coil Jazz Bass', 824100, 1, 3, '2026-02-17 10:36:38'),
(13, 'Fender American Ultra II Meteora Bass EB Ultraburst', 'A legjobbat kereső játékosok számára készült American Ultra II Series a Fender legfejlettebb hangszercsaládja. Az American Ultra II gitárok és basszusgitárok prémium anyagokból készülnek, precíz kidolgozással és modern dizájnnal, amely feszegeti a határokat, miközben kiállja az idő próbáját.\r\nA Fender American Ultra II Meteora Bass teste válogatott égerfából készült, kontúrozott kontúrokkal a maximális kényelem érdekében. A juharfa nyak modern D profilt kínál finom Ultra Satin bevonattal a sima játszhatóság érdekében. Az ébenfából vagy juharból készült, 10\"-14\"-es összetett sugarú fogólap lekerekített élekkel, közepes jumbo bordákkal, Luminlay betétekkel a nyak oldalán és Graph Tech TUSQ nulla szalaggal. A kontúrozott nyak láb és a test körvonalai könnyű hozzáférést tesznek lehetővé magasabb pozíciókba, míg a nyak fején lévő rácsos rúdhoz való hozzáférés kényelmes beállítást biztosít.\r\nA speciálisan tervezett Haymaker humbuckerekkel felszerelve ez a basszus hangszínek széles skáláját kínálja, a mély basszustól a kemény, ütős középhangokig a tiszta és artikulált magas hangokig, zökkenőmentesen. Az újratervezett előerősítő egyedi hangvezérlést kínál az aktív és passzív módok közötti S-1 kapcsolóval, a passzív hangszínszabályozással és a 3 sávos aktív EQ-val állítható középfrekvenciával. A további praktikus jellemzők közé tartozik az állítható HiMass híd, amelyen keresztül a húrok átfűzhetők a testen vagy felülről, a Fender hangológépek, az alumínium recézett potenciométerek és az egyrétegű eloxált alumínium kockavédő.\r\nAz amerikai Ultra II Meteora Bass ikonikus stílust kínál fejlett funkciókkal, amelyek új magasságokba emelik a játékot. A tökéletes hangszínt, a végtelen sokoldalúságot és a kompromisszumok nélküli teljesítményt igénylő játékosok számára az American Ultra II sorozat tökéletes megoldást nyújt.', 'Test: Selected Alder\r\nNyak: Quartersawn Maple\r\nFogólap: Ebony\r\nHúrláb hangszedő: Haymaker Bass Humbucker\r\nKözépső hangszedő: Haymaker Bass Humbucker', 1017200, 1, 3, '2026-02-17 10:37:47'),
(14, 'Fender Duff McKagan Deluxe Precision Bass RW White Pearl', 'A legendás basszusgitáros, Guns N \'Roses Duff McKagan aláírásmodellként készített 4-karakteres basszusgitár. A modern C profilú juhar nyakú, Deluxe Jazz Bass speciális logó, Rózsafátyol-gyöngyház gyöngy blokkjelekkel és Hipshot Bass Xtender hangolórendszerrel, a kar mozgásával való gyors pozicionáláshoz. A Seymour Duncan STKJ2B Jazz basszus és a Vintage-stílusú Split Single-Coil precíziós basszus különlegességet nyújt. Ráadásul fényes fekete nyak és fej, TBX magas / basszus áramkör, fekete hardver és speciális koponya vésett nyak. Tartalmaz egy hordtáskát is. Szín: fehér gyöngy', 'Test: Égerfa\r\nNyak: Juharfa\r\nFogólap: Rózsafa\r\nHúrláb hangszedő: Seymour Duncan STKJ2B Jazz Bass\r\nKözépső hangszedő: Vintage-Style Split Single-Coil Precision Bass\r\nNyaki hangszedő: Nem', 738600, 1, 3, '2026-02-17 10:39:07'),
(15, 'Fender Player Plus Blu DeTiger Jazz Bass RW Sky Burst Sparkle', 'A feltörekvő sztár, a Blu DeTiger viharba vette a zenei világot. A tekintélyes Forbes 30 Under 30 listájának zenei kategóriában címlapjaként a popzene következő generációját képviseli. Jellegzetes hangzásával és stílusával a Blu DeTiger a Fenderrel együttműködve megalkotta a limitált kiadású Player Plus x Blu DeTiger Jazz Bass készüléket. A merész Sky Burst Sparkle bevonattal, könnyű hamutesttel és egyedi hangszedőkkel ez a basszusgitár a Blu-rajongók milliói által szeretett élénk hangot és egyedi stílust kölcsönöz.\r\nA káprázatos Sky Burst Sparkle-től a csillogó krómozott hardverig és a tükrös pickguardig a Jazz Bass minden részlete a Blu merész művészi látásmódját tükrözi. Az ofszet kialakítású hamutest kamrás, hogy a basszus a lehető legkönnyebb és kényelmesebb legyen. A matt felületű juharfa nyak, a 9,5\"-os sugarú rózsafa fogólap és a Vintage-Tall szalagok biztosítják a zökkenőmentes lejátszást. Az egyedi Blu DeTiger Fireball Bass Humbucker és Player Plus Noiseless Jazz Bass hangszedők ötvözik a vintage bájt a modern ütéssel. A basszus 18 V Player Plus előerősítővel 3 sávos EQ és aktív/passzív kapcsoló, ideális a hangszín alakításához és a Blu stílusát meghatározó funky hangzás megörökítéséhez.\r\nAz inspiráló esztétikával, jellegzetes hangzással és maga a Blu által jóváhagyott funkciókkal a limitált kiadású Player Plus x Blu DeTiger Jazz Bass lehetővé teszi a fertőző popenergia megörökítését.', 'Test: Chambered Ash\r\nNyak: Juharfa\r\nFogólap: Slab Rosewood\r\nHúrláb hangszedő: Custom Blu DeTiger Fireball Bass Humbucker\r\nKözépső hangszedő: Player Plus Noiseless Jazz Bass', 530000, 1, 3, '2026-02-17 10:39:59'),
(16, 'Fender Standard Precision Bass MN Olympic White', 'A játékosokat a formatív zenei kalandok kísérésére tervezték, a Fender Standard Precision Bass azt a hozzáférhető játszhatóságot és inspiráló hangszínt kínálja, amely a Fendert rock & roll ikonná tette. A Fender Standard hangszedővel a sokoldalú ikonikus hangzásért és a kényelmes \"Modern C\" nyakformának köszönhetően a kiváló játékkomfort érdekében ez a P Bass valóban mércét állít a Fender élményben.', 'Test: Nyárfa\r\nNyak: Juharfa\r\nFogólap: Juharfa\r\nHúrláb hangszedő: Nem\r\nKözépső hangszedő: Standard Split Single-Coil Precision Bass\r\nNyaki hangszedő: Nem', 283550, 1, 3, '2026-02-17 10:41:08'),
(17, 'Fender Vintera II 50s Precision Bass MN Desert Sand\r\n', 'A Vintera® II \'50-es évek Precision Bass® égertesttel és juharfa nyakkal rendelkezik a klasszikus Fender tónus érdekében, amely tele van ütéssel és tisztasággal. Az 1950-es évek végén C-alakú nyak kényelmesen ismerős fogást kölcsönöz, amely jól illeszkedik a kézbe, míg a 7,25 hüvelykes sugarú fogólap nagy vintage szalagokkal kényelmet biztosít, bőven elegendő hely a nagy hanghajlításokhoz és a merész vibratóhoz. Az 1950-es évek vintage stílusú, osztott tekercses hangszedői az ikonikus Fender hangzást biztosítják: meleg, fás, dinamikus és erőteljes. A 4 nyerges híd és a vintage stílusú hangológépek klasszikus megjelenést biztosítanak, javított intonációval és hangolási stabilitással.', 'Test: Égerfa\r\nNyak: Juharfa\r\nFogólap: Juharfa\r\nKözépső hangszedő: Vintage-Style \'50s Split Single-Coil Precision Bass', 471700, 1, 3, '2026-02-17 10:41:43'),
(18, 'Gibson Non-Reverse Thunderbird Vintage Cherry', 'A Gibson Eredeti Gyűjtemény a klasszikus dizájnt és az eredetiséget visszahozza a Gibson rajongók elé, a Gibson aranykorát képviselő kollekcióval. Ide tartozik az 50-es és 60-as évek legendás Les Paul szabványa, valamint a 2021-es új, nem hátrameneti Thunderbird basszus termékek, amelyek kiterjedt, 34 hüvelykes nyakkal rendelkeznek, jobb egyensúlyban, eredeti színekben, többek között az Inverness Green, a Sparking Burgundy és a Pelham Blues színekben.\r\nA nem hátrameneti Thunderbird basszusok az eredeti Thunderbird sorozat új változatai. A Thunderbird a Gibson-vonal legelismertebb basszusgitárja, amely ikonikus testalakja alapján könnyen felismerhető. A megfordított testforma hamar népszerűvé vált a hosszabb felső sarok és a fej tetején lévő hangoló mechanizmusoknak köszönhetően. A modell mahagóni testtel és nyakkal, valamint indiai rózsafából készült fogólappal rendelkezik.', 'Test: Mahagóni\r\nNyak: Mahagóni\r\nFogólap: Indiai rózsafa\r\nHúrláb hangszedő: Thunderbird Lead\r\nNyaki hangszedő: Thunderbird Rhythm', 860200, 1, 3, '2026-02-17 10:42:24'),
(19, 'Gibson SG Standard Bass Ebony', 'Elektromos basszus a Gibson SG Standard sorozatból. A basszusgitár ikonikus kialakítással, legendás hangzással és nagyszerű játékélményt nyújt. Gibson egyik legnépszerűbb eszköze. A rövidebb nyak nemcsak kisebb játékosok számára alkalmas, hanem a gazdag alaptónus és a kiváló hangfelvétel érdekében is.', 'Test: Mahagóni\r\nNyak: Mahagóni\r\nFogólap: Rózsafa\r\nHúrláb hangszedő: Lead SG Bass\r\nNyaki hangszedő: Rhythm SG Bass', 701100, 1, 3, '2026-02-17 10:43:00'),
(20, 'Rickenbacker 4003S', 'Elektromos basszusgitár: 4000-es széria, klasszikus dizájn, hosszú hangkitartás, erőteljes magas- és gazdag mélyfrekvenciák, juhar test és nyak, rózsafa fogólap, 20 érintő, 845 mm hosszúságú menzúra, 254 mm rádiusz, 42.9 mm széles felső nyereg, 2 x Single Coil hangszedő, króm szerelékek, Schaller Deluxe hangolókulcsok, RIC húrláb, RIC húrvezető, szín: Fireglo', 'Test: Juhar\r\nNyak: Juhar\r\nFogólap: Rózsafa\r\nHúrláb hangszedő: Schaller Deluxe\r\nNyaki hangszedő: Schaller Deluxe', 1121080, 0, 3, '2026-02-17 10:43:52');

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
(10, 10, 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F10%2Fkep1.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F10%2Fkep2.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F10%2Fkep3.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F10%2Fkep4.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FElektromos%2F10%2Fkep5.jpg'),
(11, 11, 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F1%2Fkep1.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F1%2Fkep2.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F1%2Fkep3.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F1%2Fkep4.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F1%2Fkep5.jpg'),
(12, 12, 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F2%2Fkep1.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F2%2Fkep2.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F2%2Fkep3.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F2%2Fkep4.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F2%2Fkep5.jpg'),
(13, 13, 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F3%2Fkep1.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F3%2Fkep2.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F3%2Fkep3.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F3%2Fkep4.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F3%2Fkep5.jpg'),
(14, 14, 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F4%2Fkep1.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F4%2Fkep2.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F4%2Fkep3.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F4%2Fkep4.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F4%2Fkep5.jpg'),
(15, 15, 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F5%2Fkep1.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F5%2Fkep2.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F5%2Fkep3.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F5%2Fkep4.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F5%2Fkep5.jpg'),
(16, 16, 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F6%2Fkep1.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F6%2Fkep2.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F6%2Fkep3.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F6%2Fkep4.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F6%2Fkep5.jpg'),
(17, 17, 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F7%2Fkep1.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F7%2Fkep2.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F7%2Fkep3.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F7%2Fkep4.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F7%2Fkep5.jpg'),
(18, 18, 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F8%2Fkep1.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F8%2Fkep2.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F8%2Fkep3.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F8%2Fkep4.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F8%2Fkep5.jpg'),
(19, 19, 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F9%2Fkep1.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F9%2Fkep2.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F9%2Fkep3.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F9%2Fkep4.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F9%2Fkep5.jpg'),
(20, 20, 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F10%2Fkep1.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F10%2Fkep2.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F10%2Fkep3.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F10%2Fkep4.jpg', 'https://cdn.synk.hu/stringify/Gitarok%2FBasszusgitar%2F10%2Fkep5.jpg');

-- --------------------------------------------------------

--
-- Nézet szerkezete `egyedi_gitar_ar`
--
DROP TABLE IF EXISTS `egyedi_gitar_ar`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `egyedi_gitar_ar`  AS SELECT `eg`.`Id` AS `EgyediGitarId`, `gt`.`Ar`+ `f`.`Ar` + `pg`.`Ar` + `n`.`Ar` AS `OsszAr` FROM ((((`egyedi_gitar` `eg` join `gitar_testforma` `gt` on(`eg`.`TestformaId` = `gt`.`Id`)) join `gitar_finish` `f` on(`eg`.`FinishId` = `f`.`Id`)) join `gitar_pickguard` `pg` on(`eg`.`PickguardId` = `pg`.`Id`)) join `gitar_nyak` `n` on(`eg`.`NeckId` = `n`.`Id`)) ;

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `egyedi_gitar`
--
ALTER TABLE `egyedi_gitar`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `FelhasznaloId` (`FelhasznaloId`),
  ADD KEY `BodyShapeId` (`TestformaId`),
  ADD KEY `FinishId` (`FinishId`),
  ADD KEY `PickguardId` (`PickguardId`),
  ADD KEY `fk_eg_neck` (`NeckId`);

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
  ADD PRIMARY KEY (`Id`),
  ADD KEY `fk_finish_testforma` (`TestFormaId`);

--
-- A tábla indexei `gitar_global_elem`
--
ALTER TABLE `gitar_global_elem`
  ADD PRIMARY KEY (`Id`);

--
-- A tábla indexei `gitar_nyak`
--
ALTER TABLE `gitar_nyak`
  ADD PRIMARY KEY (`Id`);

--
-- A tábla indexei `gitar_pickguard`
--
ALTER TABLE `gitar_pickguard`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `fk_pickguard_testforma` (`TestFormaId`);

--
-- A tábla indexei `gitar_testforma`
--
ALTER TABLE `gitar_testforma`
  ADD PRIMARY KEY (`Id`);

--
-- A tábla indexei `gitar_tipus`
--
ALTER TABLE `gitar_tipus`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `Nev` (`Nev`);

--
-- A tábla indexei `kedvenc_termek`
--
ALTER TABLE `kedvenc_termek`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `unique_kedvenc` (`FelhasznaloId`,`TermekId`),
  ADD KEY `fk_kedvenc_termek` (`TermekId`);

--
-- A tábla indexei `rendeles`
--
ALTER TABLE `rendeles`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `FelhasznaloId` (`FelhasznaloId`);

--
-- A tábla indexei `rendeles_cimek`
--
ALTER TABLE `rendeles_cimek`
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
  ADD PRIMARY KEY (`Id`),
  ADD KEY `GitarTipusId` (`GitarTipusId`);

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
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT a táblához `gitar_finish`
--
ALTER TABLE `gitar_finish`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT a táblához `gitar_global_elem`
--
ALTER TABLE `gitar_global_elem`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `gitar_nyak`
--
ALTER TABLE `gitar_nyak`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT a táblához `gitar_pickguard`
--
ALTER TABLE `gitar_pickguard`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT a táblához `gitar_testforma`
--
ALTER TABLE `gitar_testforma`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT a táblához `gitar_tipus`
--
ALTER TABLE `gitar_tipus`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT a táblához `kedvenc_termek`
--
ALTER TABLE `kedvenc_termek`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT a táblához `rendeles`
--
ALTER TABLE `rendeles`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `rendeles_cimek`
--
ALTER TABLE `rendeles_cimek`
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
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;

--
-- AUTO_INCREMENT a táblához `termek_kepek`
--
ALTER TABLE `termek_kepek`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;

--
-- Megkötések a kiírt táblákhoz
--

--
-- Megkötések a táblához `egyedi_gitar`
--
ALTER TABLE `egyedi_gitar`
  ADD CONSTRAINT `egyedi_gitar_ibfk_1` FOREIGN KEY (`FelhasznaloId`) REFERENCES `felhasznalo` (`Id`),
  ADD CONSTRAINT `egyedi_gitar_ibfk_2` FOREIGN KEY (`TestformaId`) REFERENCES `gitar_testforma` (`Id`),
  ADD CONSTRAINT `egyedi_gitar_ibfk_8` FOREIGN KEY (`FinishId`) REFERENCES `gitar_finish` (`Id`),
  ADD CONSTRAINT `egyedi_gitar_ibfk_9` FOREIGN KEY (`PickguardId`) REFERENCES `gitar_pickguard` (`Id`),
  ADD CONSTRAINT `fk_eg_neck` FOREIGN KEY (`NeckId`) REFERENCES `gitar_nyak` (`Id`);

--
-- Megkötések a táblához `gitar_finish`
--
ALTER TABLE `gitar_finish`
  ADD CONSTRAINT `fk_finish_testforma` FOREIGN KEY (`TestFormaId`) REFERENCES `gitar_testforma` (`Id`);

--
-- Megkötések a táblához `gitar_pickguard`
--
ALTER TABLE `gitar_pickguard`
  ADD CONSTRAINT `fk_pickguard_testforma` FOREIGN KEY (`TestFormaId`) REFERENCES `gitar_testforma` (`Id`);

--
-- Megkötések a táblához `kedvenc_termek`
--
ALTER TABLE `kedvenc_termek`
  ADD CONSTRAINT `fk_kedvenc_felhasznalo` FOREIGN KEY (`FelhasznaloId`) REFERENCES `felhasznalo` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `fk_kedvenc_termek` FOREIGN KEY (`TermekId`) REFERENCES `termek` (`Id`) ON DELETE CASCADE;

--
-- Megkötések a táblához `rendeles`
--
ALTER TABLE `rendeles`
  ADD CONSTRAINT `rendeles_ibfk_1` FOREIGN KEY (`FelhasznaloId`) REFERENCES `felhasznalo` (`Id`);

--
-- Megkötések a táblához `rendeles_cimek`
--
ALTER TABLE `rendeles_cimek`
  ADD CONSTRAINT `rendeles_cimek_ibfk_1` FOREIGN KEY (`FelhasznaloId`) REFERENCES `felhasznalo` (`Id`);

--
-- Megkötések a táblához `rendeles_tetel`
--
ALTER TABLE `rendeles_tetel`
  ADD CONSTRAINT `rendeles_tetel_ibfk_1` FOREIGN KEY (`RendelesId`) REFERENCES `rendeles` (`Id`),
  ADD CONSTRAINT `rendeles_tetel_ibfk_2` FOREIGN KEY (`TermekId`) REFERENCES `termek` (`Id`),
  ADD CONSTRAINT `rendeles_tetel_ibfk_3` FOREIGN KEY (`EgyediGitarId`) REFERENCES `egyedi_gitar` (`Id`);

--
-- Megkötések a táblához `termek`
--
ALTER TABLE `termek`
  ADD CONSTRAINT `termek_ibfk_tipus` FOREIGN KEY (`GitarTipusId`) REFERENCES `gitar_tipus` (`Id`);

--
-- Megkötések a táblához `termek_kepek`
--
ALTER TABLE `termek_kepek`
  ADD CONSTRAINT `termek_kepek_ibfk_1` FOREIGN KEY (`TermekId`) REFERENCES `termek` (`Id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
