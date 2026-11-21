CREATE DATABASE IF NOT EXISTS mesa_ayuda;

USE mesa_ayuda;

CREATE TABLE IF NOT EXISTS Tickets (
    Id INT NOT NULL AUTO_INCREMENT,
    Titulo VARCHAR(200) NOT NULL,
    Descripcion TEXT NOT NULL,
    Categoria VARCHAR(50) NOT NULL,
    Estado VARCHAR(20) NOT NULL DEFAULT 'Abierto',
    FechaCreacion DATETIME NOT NULL,
    PRIMARY KEY (Id)
);

-- Datos de ejemplo para la demostración
INSERT INTO Tickets (Titulo, Descripcion, Categoria, Estado, FechaCreacion) VALUES
('No enciende la PC', 'La computadora del laboratorio no enciende', 'Hardware', 'Abierto', NOW()),
('Sin internet en el salon', 'No hay conexion en el salon 3', 'Redes', 'Abierto', NOW()),
('Instalar programa', 'Instalar Visual Studio en la PC del profesor', 'Software', 'Cerrado', NOW());
