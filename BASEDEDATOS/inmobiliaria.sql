-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: localhost
-- Tiempo de generación: 02-09-2023 a las 11:58:55
-- Versión del servidor: 10.4.28-MariaDB
-- Versión de PHP: 8.2.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `inmobiliaria`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contratos`
--

CREATE TABLE `contratos` (
  `IdContrato` int(11) NOT NULL,
  `InquilinoId` int(11) NOT NULL,
  `InmuebleId` int(11) NOT NULL,
  `FechaDesde` date NOT NULL,
  `FechaHasta` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `contratos`
--

INSERT INTO `contratos` (`IdContrato`, `InquilinoId`, `InmuebleId`, `FechaDesde`, `FechaHasta`) VALUES
(5, 1, 13, '2023-09-01', '2023-09-30'),
(6, 2, 13, '2023-09-10', '2023-09-20'),
(8, 1, 11, '2023-09-23', '2023-09-01'),
(9, 2, 11, '2023-09-10', '2023-09-30');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmuebles`
--

CREATE TABLE `inmuebles` (
  `IdInmueble` int(11) NOT NULL,
  `PropietarioId` int(11) NOT NULL,
  `CantidadAmbientes` int(11) NOT NULL,
  `Uso` int(11) NOT NULL,
  `Direccion` varchar(100) NOT NULL,
  `Tipo` int(11) NOT NULL,
  `Latitud` decimal(10,2) NOT NULL,
  `Longitud` decimal(10,2) NOT NULL,
  `Precio` decimal(10,2) NOT NULL,
  `Disponible` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inmuebles`
--

INSERT INTO `inmuebles` (`IdInmueble`, `PropietarioId`, `CantidadAmbientes`, `Uso`, `Direccion`, `Tipo`, `Latitud`, `Longitud`, `Precio`, `Disponible`) VALUES
(11, 19, 3, 1, '123 Calle Principal', 1, 41.34, -74.01, 251.00, 1),
(13, 23, 2, 2, '789 Calle Secundaria', 4, 11.11, 12.76, 13.25, 0),
(21, 24, 2, 1, 'Av. siempre viva', 2, 2344.32, 22.09, 123.45, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilinos`
--

CREATE TABLE `inquilinos` (
  `IdInquilino` int(11) NOT NULL,
  `Nombre` varchar(100) NOT NULL,
  `Apellido` varchar(100) NOT NULL,
  `Dni` varchar(100) NOT NULL,
  `Email` varchar(100) DEFAULT NULL,
  `Telefono` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inquilinos`
--

INSERT INTO `inquilinos` (`IdInquilino`, `Nombre`, `Apellido`, `Dni`, `Email`, `Telefono`) VALUES
(1, 'martintin', 'Panelosky', '3434343434', 'martinmartin@mail.com', '26666666661'),
(2, 'Pluton', 'MedioPlaneta', '999', 'Planetoide@mail.com', '222');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pagos`
--

CREATE TABLE `pagos` (
  `IdPago` int(11) NOT NULL,
  `ContratoId` int(11) NOT NULL,
  `Pago` decimal(10,0) NOT NULL,
  `Fecha` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `pagos`
--

INSERT INTO `pagos` (`IdPago`, `ContratoId`, `Pago`, `Fecha`) VALUES
(1, 5, 100, '2023-09-02'),
(2, 5, 100, '2023-09-03'),
(3, 5, 100, '2023-09-04'),
(4, 5, 100, '2023-09-05');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietarios`
--

CREATE TABLE `propietarios` (
  `IdPropietario` int(11) NOT NULL,
  `Nombre` varchar(100) NOT NULL,
  `Apellido` varchar(100) NOT NULL,
  `Dni` varchar(100) NOT NULL,
  `Telefono` varchar(100) NOT NULL,
  `Email` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `propietarios`
--

INSERT INTO `propietarios` (`IdPropietario`, `Nombre`, `Apellido`, `Dni`, `Telefono`, `Email`) VALUES
(19, 'Lorena', 'García', '1234567890', '555-1234', 'lorena@example.com'),
(23, 'Pepe', 'Perez', '393939', '266444444', 'pepe_Perez@mail.com'),
(24, 'Juan', 'Domino', '123321', '5497899991', 'JDomino@email.com'),
(26, 'Panelo', 'Martin', '123456', '123123123', 'panelomartinn@hotmail.com'),
(27, '123', 'Martin1231', 'qweqweq', '131231', 'panelomartinn@hotmail.com'),
(28, '123', 'martin', '123', '123231', 'panelomartinn@hotmail.com');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tiposInmuebles`
--

CREATE TABLE `tiposInmuebles` (
  `IdTipoInmueble` int(11) NOT NULL,
  `Tipo` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `tiposInmuebles`
--

INSERT INTO `tiposInmuebles` (`IdTipoInmueble`, `Tipo`) VALUES
(1, 'local'),
(2, 'depósito'),
(3, 'casa'),
(4, 'departamento');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usosInmuebles`
--

CREATE TABLE `usosInmuebles` (
  `IdUsoInmueble` int(11) NOT NULL,
  `Uso` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `usosInmuebles`
--

INSERT INTO `usosInmuebles` (`IdUsoInmueble`, `Uso`) VALUES
(1, 'comercial'),
(2, 'residencial'),
(3, 'industrial');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD PRIMARY KEY (`IdContrato`),
  ADD KEY `InquilinoId` (`InquilinoId`,`InmuebleId`),
  ADD KEY `InmuebleId` (`InmuebleId`);

--
-- Indices de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD PRIMARY KEY (`IdInmueble`),
  ADD KEY `PropietarioId` (`PropietarioId`),
  ADD KEY `Uso` (`Uso`),
  ADD KEY `Tipo` (`Tipo`);

--
-- Indices de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  ADD PRIMARY KEY (`IdInquilino`);

--
-- Indices de la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD PRIMARY KEY (`IdPago`),
  ADD KEY `ContratoId` (`ContratoId`);

--
-- Indices de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  ADD PRIMARY KEY (`IdPropietario`);

--
-- Indices de la tabla `tiposInmuebles`
--
ALTER TABLE `tiposInmuebles`
  ADD PRIMARY KEY (`IdTipoInmueble`);

--
-- Indices de la tabla `usosInmuebles`
--
ALTER TABLE `usosInmuebles`
  ADD PRIMARY KEY (`IdUsoInmueble`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contratos`
--
ALTER TABLE `contratos`
  MODIFY `IdContrato` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  MODIFY `IdInmueble` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=23;

--
-- AUTO_INCREMENT de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  MODIFY `IdInquilino` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `pagos`
--
ALTER TABLE `pagos`
  MODIFY `IdPago` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  MODIFY `IdPropietario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=29;

--
-- AUTO_INCREMENT de la tabla `tiposInmuebles`
--
ALTER TABLE `tiposInmuebles`
  MODIFY `IdTipoInmueble` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `usosInmuebles`
--
ALTER TABLE `usosInmuebles`
  MODIFY `IdUsoInmueble` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD CONSTRAINT `contratos_ibfk_1` FOREIGN KEY (`InquilinoId`) REFERENCES `inquilinos` (`IdInquilino`) ON UPDATE CASCADE,
  ADD CONSTRAINT `contratos_ibfk_2` FOREIGN KEY (`InmuebleId`) REFERENCES `inmuebles` (`IdInmueble`) ON UPDATE CASCADE;

--
-- Filtros para la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD CONSTRAINT `inmuebles_ibfk_1` FOREIGN KEY (`PropietarioId`) REFERENCES `propietarios` (`IdPropietario`) ON UPDATE CASCADE,
  ADD CONSTRAINT `inmuebles_ibfk_3` FOREIGN KEY (`Uso`) REFERENCES `usosInmuebles` (`IdUsoInmueble`) ON UPDATE CASCADE,
  ADD CONSTRAINT `inmuebles_ibfk_4` FOREIGN KEY (`Tipo`) REFERENCES `tiposInmuebles` (`IdTipoInmueble`) ON UPDATE CASCADE;

--
-- Filtros para la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD CONSTRAINT `pagos_ibfk_1` FOREIGN KEY (`ContratoId`) REFERENCES `contratos` (`IdContrato`) ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
