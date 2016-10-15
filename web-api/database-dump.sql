-- phpMyAdmin SQL Dump
-- version 4.5.1
-- http://www.phpmyadmin.net
--
-- Host: 127.0.0.1
-- Generation Time: Nov 15, 2015 at 01:48 PM
-- Server version: 10.1.8-MariaDB
-- PHP Version: 5.6.14

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `aplikacija`
--

-- --------------------------------------------------------

--
-- Table structure for table `camps`
--

CREATE TABLE `camps` (
  `id` int(10) UNSIGNED NOT NULL,
  `name` varchar(255) NOT NULL,
  `location` text NOT NULL COMMENT 'adresa',
  `longitude` float(10,6) NOT NULL,
  `latitude` float(10,6) NOT NULL,
  `capacity` int(10) UNSIGNED NOT NULL DEFAULT '10'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `camps`
--

INSERT INTO `camps` (`id`, `name`, `location`, `longitude`, `latitude`, `capacity`) VALUES
(1, 'PR1', 'Nis', 23.948599, 43.325027, 100),
(2, 'PR2', 'Nis', 22.948599, 43.325027, 100),
(3, 'SLRT 3', 'Preševo', 21.619173, 42.315746, 0),
(8, 'SLRT 2', 'Preševo2', 20.619123, 42.315746, 0),
(9, 'SLRT 2', 'Preševo2', 20.619123, 42.315746, 0);

-- --------------------------------------------------------

--
-- Table structure for table `destinationcamp`
--

CREATE TABLE `destinationcamp` (
  `id` int(10) UNSIGNED NOT NULL,
  `campid` int(10) UNSIGNED NOT NULL,
  `groupid` int(10) UNSIGNED NOT NULL,
  `date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `leftcamp` tinyint(1) NOT NULL DEFAULT '0',
  `incamp` tinyint(1) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `destinationcamp`
--

INSERT INTO `destinationcamp` (`id`, `campid`, `groupid`, `date`, `leftcamp`, `incamp`) VALUES
(2, 1, 35, '2015-11-15 08:48:29', 0, 1),
(6, 3, 34, '2015-11-15 11:59:10', 1, 0);

-- --------------------------------------------------------

--
-- Table structure for table `groupmembers`
--

CREATE TABLE `groupmembers` (
  `id` int(12) UNSIGNED NOT NULL,
  `groupid` int(10) UNSIGNED NOT NULL,
  `name` varchar(255) NOT NULL,
  `age` int(10) UNSIGNED NOT NULL,
  `male` tinyint(1) NOT NULL DEFAULT '1',
  `child` tinyint(1) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `groupmembers`
--

INSERT INTO `groupmembers` (`id`, `groupid`, `name`, `age`, `male`, `child`) VALUES
(1, 1, 'zika', 0, 0, 0),
(2, 1, 'mika', 0, 0, 0),
(3, 1, 'pera', 0, 0, 0),
(4, 1, 'smrda', 0, 0, 1),
(9, 1, 'majmun', 0, 0, 1),
(10, 1, 'manga', 0, 0, 0),
(11, 2, 'mile', 0, 0, 0),
(12, 2, 'nole', 0, 0, 1);

-- --------------------------------------------------------

--
-- Table structure for table `incamps`
--

CREATE TABLE `incamps` (
  `id` int(10) UNSIGNED NOT NULL,
  `groupid` int(10) UNSIGNED NOT NULL,
  `campid` int(10) UNSIGNED NOT NULL,
  `numpeople` int(11) NOT NULL,
  `checkindate` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `lastlocation`
--

CREATE TABLE `lastlocation` (
  `id` int(12) UNSIGNED NOT NULL,
  `groupid` int(11) NOT NULL,
  `longitude` float(10,6) NOT NULL,
  `latitude` float(10,6) NOT NULL,
  `date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `city` varchar(128) NOT NULL,
  `gps` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `lastlocation`
--

INSERT INTO `lastlocation` (`id`, `groupid`, `longitude`, `latitude`, `date`, `city`, `gps`) VALUES
(1, 1, 1.100000, 1.200000, '2015-11-15 03:47:41', 'Niš', 1),
(2, 1, 1.100000, 1.200000, '2015-11-15 03:48:38', 'Niš', 1),
(3, 1, 1.100000, 1.200000, '2015-11-15 03:48:47', 'Niš', 1),
(4, 1, 21.948599, 43.325027, '2015-11-15 06:58:45', 'Niš', 1),
(6, 24, 21.444441, 15.555550, '2015-11-15 08:26:25', ' ', 1),
(7, 27, 21.891960, 43.331829, '2015-11-15 09:01:19', ' ', 1),
(8, 28, 1.000000, 1.000000, '2015-11-15 09:30:53', ' ', 1),
(9, 31, 1.000000, 1.000000, '2015-11-15 09:59:10', ' ', 1),
(10, 32, 1.000000, 1.000000, '2015-11-15 10:10:08', ' ', 1),
(11, 33, 1.000000, 1.000000, '2015-11-15 10:14:49', ' ', 1),
(12, 35, 1.000000, 1.000000, '2015-11-15 10:27:08', ' ', 1),
(13, 35, 21.891939, 43.331928, '2015-11-15 11:25:23', ' ', 1),
(14, 35, 21.892151, 43.331539, '2015-11-15 12:03:30', ' ', 1),
(15, 35, 21.891911, 43.332020, '2015-11-15 12:04:56', ' ', 1),
(16, 35, 21.891899, 43.332001, '2015-11-15 12:32:24', ' ', 1),
(17, 35, 21.892071, 43.331791, '2015-11-15 12:33:57', ' ', 1);

-- --------------------------------------------------------

--
-- Table structure for table `news`
--

CREATE TABLE `news` (
  `id` int(11) NOT NULL,
  `headline` varchar(255) NOT NULL,
  `content` text NOT NULL,
  `date` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `tokens`
--

CREATE TABLE `tokens` (
  `id` int(11) NOT NULL,
  `token` varchar(255) NOT NULL,
  `description` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tokens`
--

INSERT INTO `tokens` (`id`, `token`, `description`) VALUES
(1, '6bb22f1a9be94d929136641119ca6f3d2839e85d', 'admin'),
(2, '123456', '');

-- --------------------------------------------------------

--
-- Table structure for table `userpool`
--

CREATE TABLE `userpool` (
  `imei` varchar(255) NOT NULL,
  `firstseen` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `location` varchar(120) NOT NULL,
  `longitude` float(10,6) NOT NULL,
  `latitude` float(10,6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `groupid` int(10) UNSIGNED NOT NULL,
  `imei` varchar(255) NOT NULL,
  `viewid` varchar(10) NOT NULL,
  `phonenumber` varchar(60) NOT NULL,
  `destinationcountry` varchar(2) NOT NULL COMMENT 'country code',
  `origincountry` varchar(2) NOT NULL,
  `numadults` int(10) UNSIGNED NOT NULL DEFAULT '1',
  `numchildren` int(10) UNSIGNED NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`groupid`, `imei`, `viewid`, `phonenumber`, `destinationcountry`, `origincountry`, `numadults`, `numchildren`) VALUES
(1, '123456789', 'PK956208DE', '', 'DE', 'PK', 2, 2),
(2, '23456789', 'SI806343DE', '', 'DE', 'SI', 5, 1),
(3, '3456789', 'LI386793ME', '', 'ME', 'LI', 5, 0),
(4, '456789', 'LY597304GB', '', 'GB', 'LY', 2, 0),
(33, 'be89f85fbb5e3ac30ac8d6e22b372abdf481e99c', 'Se092152Se', '', 'Se', 'Se', 1, 1),
(34, 'f2aba239b5f722cd0733e69c22be9eb1bb61237c', 'VS123DS', '', 'DS', 'VS', 1, 0),
(35, 'e2a95547d9a2e9d525a8ea7530ed102e', 'OM186248IE', '', 'IE', 'OM', 45, 34);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `camps`
--
ALTER TABLE `camps`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `destinationcamp`
--
ALTER TABLE `destinationcamp`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `groupmembers`
--
ALTER TABLE `groupmembers`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `incamps`
--
ALTER TABLE `incamps`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `lastlocation`
--
ALTER TABLE `lastlocation`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `news`
--
ALTER TABLE `news`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `tokens`
--
ALTER TABLE `tokens`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `userpool`
--
ALTER TABLE `userpool`
  ADD PRIMARY KEY (`imei`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`groupid`),
  ADD UNIQUE KEY `imei` (`imei`),
  ADD UNIQUE KEY `viewid` (`viewid`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `camps`
--
ALTER TABLE `camps`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;
--
-- AUTO_INCREMENT for table `destinationcamp`
--
ALTER TABLE `destinationcamp`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;
--
-- AUTO_INCREMENT for table `groupmembers`
--
ALTER TABLE `groupmembers`
  MODIFY `id` int(12) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;
--
-- AUTO_INCREMENT for table `incamps`
--
ALTER TABLE `incamps`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT for table `lastlocation`
--
ALTER TABLE `lastlocation`
  MODIFY `id` int(12) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=18;
--
-- AUTO_INCREMENT for table `news`
--
ALTER TABLE `news`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT for table `tokens`
--
ALTER TABLE `tokens`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;
--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `groupid` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=36;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
