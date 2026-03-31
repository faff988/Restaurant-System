-- phpMyAdmin SQL Dump
-- version 4.9.2
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1:3306
-- Generation Time: Mar 31, 2026 at 03:08 PM
-- Server version: 10.4.10-MariaDB
-- PHP Version: 7.3.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `restaurantdb`
--

-- --------------------------------------------------------

--
-- Table structure for table `aspnetroleclaims`
--

DROP TABLE IF EXISTS `aspnetroleclaims`;
CREATE TABLE IF NOT EXISTS `aspnetroleclaims` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `RoleId` varchar(80) NOT NULL,
  `ClaimType` longtext DEFAULT NULL,
  `ClaimValue` longtext DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetRoleClaims_RoleId` (`RoleId`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `aspnetroles`
--

DROP TABLE IF EXISTS `aspnetroles`;
CREATE TABLE IF NOT EXISTS `aspnetroles` (
  `Id` varchar(80) NOT NULL,
  `Name` varchar(256) DEFAULT NULL,
  `NormalizedName` varchar(80) DEFAULT NULL,
  `ConcurrencyStamp` longtext DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `RoleNameIndex` (`NormalizedName`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `aspnetroles`
--

INSERT INTO `aspnetroles` (`Id`, `Name`, `NormalizedName`, `ConcurrencyStamp`) VALUES
('1cb81024-d607-4e57-a8a2-453dbfd70245', 'Admin', 'ADMIN', NULL),
('d0dc5fae-4556-47fb-af26-6d8ebd8bfcc9', 'Customer', 'CUSTOMER', NULL);

-- --------------------------------------------------------

--
-- Table structure for table `aspnetuserclaims`
--

DROP TABLE IF EXISTS `aspnetuserclaims`;
CREATE TABLE IF NOT EXISTS `aspnetuserclaims` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` varchar(80) NOT NULL,
  `ClaimType` longtext DEFAULT NULL,
  `ClaimValue` longtext DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetUserClaims_UserId` (`UserId`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `aspnetuserlogins`
--

DROP TABLE IF EXISTS `aspnetuserlogins`;
CREATE TABLE IF NOT EXISTS `aspnetuserlogins` (
  `LoginProvider` varchar(80) NOT NULL,
  `ProviderKey` varchar(80) NOT NULL,
  `ProviderDisplayName` longtext DEFAULT NULL,
  `UserId` varchar(80) NOT NULL,
  PRIMARY KEY (`LoginProvider`,`ProviderKey`),
  KEY `IX_AspNetUserLogins_UserId` (`UserId`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `aspnetuserroles`
--

DROP TABLE IF EXISTS `aspnetuserroles`;
CREATE TABLE IF NOT EXISTS `aspnetuserroles` (
  `UserId` varchar(80) NOT NULL,
  `RoleId` varchar(80) NOT NULL,
  PRIMARY KEY (`UserId`,`RoleId`),
  KEY `IX_AspNetUserRoles_RoleId` (`RoleId`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `aspnetuserroles`
--

INSERT INTO `aspnetuserroles` (`UserId`, `RoleId`) VALUES
('3a3a67b6-6519-4b69-98fc-4e57606b62a5', 'd0dc5fae-4556-47fb-af26-6d8ebd8bfcc9'),
('702d1b5e-a605-48b2-8843-d3c3ea450ff4', '1cb81024-d607-4e57-a8a2-453dbfd70245');

-- --------------------------------------------------------

--
-- Table structure for table `aspnetusers`
--

DROP TABLE IF EXISTS `aspnetusers`;
CREATE TABLE IF NOT EXISTS `aspnetusers` (
  `Id` varchar(80) NOT NULL,
  `UserName` varchar(256) DEFAULT NULL,
  `NormalizedUserName` varchar(128) DEFAULT NULL,
  `Email` varchar(256) DEFAULT NULL,
  `NormalizedEmail` varchar(128) DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext DEFAULT NULL,
  `SecurityStamp` longtext DEFAULT NULL,
  `ConcurrencyStamp` longtext DEFAULT NULL,
  `PhoneNumber` longtext DEFAULT NULL,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEnd` datetime(6) DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  KEY `EmailIndex` (`NormalizedEmail`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `aspnetusers`
--

INSERT INTO `aspnetusers` (`Id`, `UserName`, `NormalizedUserName`, `Email`, `NormalizedEmail`, `EmailConfirmed`, `PasswordHash`, `SecurityStamp`, `ConcurrencyStamp`, `PhoneNumber`, `PhoneNumberConfirmed`, `TwoFactorEnabled`, `LockoutEnd`, `LockoutEnabled`, `AccessFailedCount`) VALUES
('702d1b5e-a605-48b2-8843-d3c3ea450ff4', 'admin@restaurant.com', 'ADMIN@RESTAURANT.COM', 'admin@restaurant.com', 'ADMIN@RESTAURANT.COM', 0, 'AQAAAAIAAYagAAAAEHkwX3fjnB3U3W9HMNPiSZghkTa4wFtVQvwc3q+pS2VGj7i1WsWK4KGIIjfv83Pqzg==', 'OT734IMVPXE5VT5RET4SVGL3ZHHWKHZZ', 'ff297ad5-2878-4780-8e84-a602b403a851', NULL, 0, 0, NULL, 1, 0),
('3a3a67b6-6519-4b69-98fc-4e57606b62a5', 'neangnongfa002@gmail.com', 'NEANGNONGFA002@GMAIL.COM', 'neangnongfa002@gmail.com', 'NEANGNONGFA002@GMAIL.COM', 0, 'AQAAAAIAAYagAAAAEAC/BaslCAqOB0vgJlUYeuRjLd5mgCapBlXbNqWX6lcdi4s4dHIaxsfHBOrPFAlCFw==', 'M5QCM2LS7M3MAI2BEBVP2ZCPVO4GT4RU', '3a8f32fc-7c8d-478b-b83b-3ecfd28098bc', NULL, 0, 0, NULL, 1, 0);

-- --------------------------------------------------------

--
-- Table structure for table `aspnetusertokens`
--

DROP TABLE IF EXISTS `aspnetusertokens`;
CREATE TABLE IF NOT EXISTS `aspnetusertokens` (
  `UserId` varchar(80) NOT NULL,
  `LoginProvider` varchar(80) NOT NULL,
  `Name` varchar(80) NOT NULL,
  `Value` longtext DEFAULT NULL,
  PRIMARY KEY (`UserId`,`LoginProvider`,`Name`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `menuitems`
--

DROP TABLE IF EXISTS `menuitems`;
CREATE TABLE IF NOT EXISTS `menuitems` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` longtext NOT NULL,
  `Description` longtext NOT NULL,
  `Price` decimal(65,30) NOT NULL,
  `Category` longtext NOT NULL,
  `IsAvailable` tinyint(1) NOT NULL,
  `ImageUrl` longtext NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `menuitems`
--

INSERT INTO `menuitems` (`Id`, `Name`, `Description`, `Price`, `Category`, `IsAvailable`, `ImageUrl`) VALUES
(5, 'Fried Spring Rolls', 'Crispy vegetable spring rolls with sweet chili sauce', '4.000000000000000000000000000000', 'Appetizer', 1, 'https://i.pinimg.com/originals/bb/b6/13/bbb613ee2a387d6041f15ea9281ffd3d.png'),
(3, 'Khmer Beef Lok Lak', 'Tender stir-fry beef served with lime and black pepper sauce', '8.500000000000000000000000000000', 'Main Course', 1, 'https://as1.ftcdn.net/v2/jpg/01/77/81/96/1000_F_177819689_F5VyfXSUA8PcrXo24Km2g6Wy3BgDaRug.jpg'),
(4, 'Fish Amok', 'Traditional steamed fish curry in banana leaf', '7.500000000000000000000000000000', 'Main Course', 1, 'https://www.shutterstock.com/image-photo/authentic-cambodian-amok-traditional-fish-600nw-2496550375.jpg'),
(6, 'Num Ansom', 'Sticky rice cake with banana or pork fillings', '3.500000000000000000000000000000', 'Dessert', 1, 'https://i.pinimg.com/736x/c9/62/b7/c962b7446564e61f91edd5ec4e67e375.jpg'),
(7, 'Iced Coffee with Milk', 'Strong Khmer coffee with sweet condensed milk', '2.000000000000000000000000000000', 'Drinks', 1, 'https://images.ctfassets.net/v601h1fyjgba/71VWCR6Oclk14tsdM9gTyM/6921cc6b21746f62846c99fa6a872c35/Iced_Latte.jpg');

-- --------------------------------------------------------

--
-- Table structure for table `orderdetails`
--

DROP TABLE IF EXISTS `orderdetails`;
CREATE TABLE IF NOT EXISTS `orderdetails` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `OrderId` int(11) NOT NULL,
  `MenuItemId` int(11) NOT NULL,
  `Quantity` int(11) NOT NULL,
  `UnitPrice` decimal(65,30) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_OrderDetails_MenuItemId` (`MenuItemId`),
  KEY `IX_OrderDetails_OrderId` (`OrderId`)
) ENGINE=MyISAM AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `orderdetails`
--

INSERT INTO `orderdetails` (`Id`, `OrderId`, `MenuItemId`, `Quantity`, `UnitPrice`) VALUES
(1, 1, 1, 1, '8.500000000000000000000000000000'),
(2, 1, 2, 1, '7.500000000000000000000000000000'),
(3, 2, 3, 2, '4.000000000000000000000000000000'),
(4, 2, 5, 1, '2.000000000000000000000000000000'),
(5, 3, 5, 2, '4.000000000000000000000000000000'),
(6, 3, 3, 1, '8.500000000000000000000000000000'),
(7, 3, 7, 1, '2.000000000000000000000000000000');

-- --------------------------------------------------------

--
-- Table structure for table `orders`
--

DROP TABLE IF EXISTS `orders`;
CREATE TABLE IF NOT EXISTS `orders` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CustomerName` longtext NOT NULL,
  `CustomerEmail` longtext NOT NULL,
  `OrderDate` datetime(6) NOT NULL,
  `Status` longtext NOT NULL,
  `TotalAmount` decimal(65,30) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `orders`
--

INSERT INTO `orders` (`Id`, `CustomerName`, `CustomerEmail`, `OrderDate`, `Status`, `TotalAmount`) VALUES
(1, 'Table 1', 'table1@internal.com', '2024-05-25 19:45:00.000000', 'Completed', '16.000000000000000000000000000000'),
(2, 'John', 'john.s@example.com', '2024-05-26 13:10:00.000000', 'Processing', '10.000000000000000000000000000000'),
(3, 'Neang Nongfa', 'neangnongfa002@gmail.com', '2026-03-31 22:07:13.063993', 'Pending', '18.500000000000000000000000000000');

-- --------------------------------------------------------

--
-- Table structure for table `reservations`
--

DROP TABLE IF EXISTS `reservations`;
CREATE TABLE IF NOT EXISTS `reservations` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CustomerName` longtext NOT NULL,
  `CustomerEmail` longtext NOT NULL,
  `CustomerPhone` longtext NOT NULL,
  `ReservationDate` datetime(6) NOT NULL,
  `NumberOfGuests` int(11) NOT NULL,
  `Status` longtext NOT NULL,
  `SpecialRequests` longtext NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `reservations`
--

INSERT INTO `reservations` (`Id`, `CustomerName`, `CustomerEmail`, `CustomerPhone`, `ReservationDate`, `NumberOfGuests`, `Status`, `SpecialRequests`) VALUES
(1, 'Sokha Mom', 'sokha@example.com', '012345678', '2024-06-01 18:30:00.000000', 4, 'Pending', 'Window seat if possible'),
(2, 'John Smith', 'john.s@example.com', '098765432', '2024-06-02 12:00:00.000000', 2, 'Confirmed', 'Birthday celebration'),
(3, 'Sophea Cheng', 'sophea@example.com', '011223344', '2024-06-02 19:00:00.000000', 6, 'Confirmed', '');

-- --------------------------------------------------------

--
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
CREATE TABLE IF NOT EXISTS `__efmigrationshistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `__efmigrationshistory`
--

INSERT INTO `__efmigrationshistory` (`MigrationId`, `ProductVersion`) VALUES
('20260331023048_InitialCreate', '9.0.0');
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
