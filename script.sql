CREATE TABLE [finca] (
[id_finca] INTEGER  PRIMARY KEY AUTOINCREMENT NOT NULL,
[nombre_fin] VARCHAR(255)  NOT NULL,
[ubicacion_fin] VARCHAR(255)  NOT NULL,
[id_usuario] VARCHAR(255)  NOT NULL,
 CONSTRAINT `usuario_ibfk` FOREIGN KEY (`id_usuario`) REFERENCES `usuario` (`id_usuario`) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE [lectura] (
[id_lectura] INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT,
[ph] VARCHAR(255)  NOT NULL,
[temperatura] VARCHAR(255)  NOT NULL,
[dioxido] VARCHAR(255)  NOT NULL,
[fecha_lectura] TIMESTAMP  NOT NULL,
[id_muestra] INTEGER KEY NOT NULL,
CONSTRAINT `lectura_ibfk` FOREIGN KEY (`id_muestra`) REFERENCES `muestra` (`id_muestra`) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE [muestra] (
[id_muestra] INTEGER  PRIMARY KEY AUTOINCREMENT NOT NULL,
[fecha_recepcion] VARCHAR(255)  NOT NULL,
[peso] VARCHAR(255)  NOT NULL,
[id_variedad] INTEGER KEY NOT NULL,
CONSTRAINT `muestra_ibfk` FOREIGN KEY (`id_variedad`) REFERENCES `variedad` (`id_variedad`) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE [usuario] (
[id_usuario] VARCHAR(255)  UNIQUE NOT NULL PRIMARY KEY,
[nombre_usu] VARCHAR(255)  NOT NULL,
[direccion] VARCHAR(255)  NOT NULL,
[email] VARCHAR(255)  NOT NULL,
[telefono] VARCHAR(255)  NOT NULL
);

CREATE TABLE [variedad] (
[id_variedad] INTEGER  PRIMARY KEY AUTOINCREMENT NOT NULL,
[nombre_var] VARCHAR(255)  NOT NULL,
[id_finca] INTEGER KEY NOT NULL,
CONSTRAINT `variedad_ibfk` FOREIGN KEY (`id_finca`) REFERENCES `finca` (`id_finca`) ON DELETE CASCADE ON UPDATE CASCADE
);
