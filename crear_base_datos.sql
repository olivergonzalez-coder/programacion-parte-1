CREATE DATABASE IF NOT EXISTS mesa_ayuda;

USE mesa_ayuda;

CREATE TABLE IF NOT EXISTS Categorias (
    Id INT NOT NULL AUTO_INCREMENT,
    Nombre VARCHAR(50) NOT NULL UNIQUE,
    PRIMARY KEY (Id)
);

CREATE TABLE IF NOT EXISTS Tickets (
    Id INT NOT NULL AUTO_INCREMENT,
    Titulo VARCHAR(200) NOT NULL,
    Descripcion TEXT NOT NULL,
    Prioridad VARCHAR(20) NOT NULL,
    Estado VARCHAR(20) NOT NULL DEFAULT 'Abierto',
    FechaCreacion DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CategoriaId INT NOT NULL,
    PRIMARY KEY (Id),
    CONSTRAINT FK_Tickets_Categorias
        FOREIGN KEY (CategoriaId)
        REFERENCES Categorias(Id),
    CONSTRAINT CHK_Tickets_Prioridad
        CHECK (Prioridad IN ('Alta', 'Media', 'Baja')),
    CONSTRAINT CHK_Tickets_Estado
        CHECK (Estado IN ('Abierto', 'Cerrado'))
);

INSERT INTO Categorias (Nombre) VALUES
('Hardware'),
('Software'),
('Redes');

INSERT INTO Tickets
(Titulo, Descripcion, Prioridad, Estado, CategoriaId)
VALUES
('No enciende la PC',
 'La computadora del laboratorio no enciende',
 'Alta', 'Abierto', 1),

('Sin internet en el salon',
 'No hay conexion en el salon 3',
 'Media', 'Abierto', 3),

('Instalar programa',
 'Instalar Visual Studio en la PC del profesor',
 'Baja', 'Cerrado', 2);
