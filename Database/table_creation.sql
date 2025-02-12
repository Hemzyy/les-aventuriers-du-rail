CREATE TABLE Joueur (
    idJoueur INT AUTO_INCREMENT PRIMARY KEY,
    pseudo VARCHAR(255) UNIQUE,
    motDePass VARCHAR(255)
);

CREATE TABLE StatJoueur (
    idStat INT AUTO_INCREMENT PRIMARY KEY,
    idStatJoueur INT,
    highScore INT,
    PartiesJouées INT,
    partiesGagnées INT,
    FOREIGN KEY (idStatJoueur) REFERENCES Joueur(idJoueur)
);

CREATE TABLE Ville (
    idVille INT PRIMARY KEY,
    NomVille VARCHAR(255)
);

CREATE TABLE cartesMission (
    idMission INT PRIMARY KEY,
    idVille1 INT,
    idVille2 INT,
    nbrDePoints INT,
    FOREIGN KEY (idVille1) REFERENCES Ville(idVille),
    FOREIGN KEY (idVille2) REFERENCES Ville(idVille)
);

CREATE TABLE Couleur (
    idClr INT PRIMARY KEY,
    nomClr VARCHAR(255)
);

CREATE TABLE Rails (
    idRails INT PRIMARY KEY,
    idVille1 INT,
    idVille2 INT,
    idClr INT,
    nbrDeRail INT,
    FOREIGN KEY (idVille1) REFERENCES Ville(idVille),
    FOREIGN KEY (idVille2) REFERENCES Ville(idVille),
    FOREIGN KEY (idClr) REFERENCES Couleur(idClr)
);

CREATE TABLE cartesWagon (
    idCarte INT PRIMARY KEY,
    idClr INT,
    FOREIGN KEY (idClr) REFERENCES Couleur(idClr)
);

CREATE TABLE Partie (
    idPartie INT AUTO_INCREMENT PRIMARY KEY
);
