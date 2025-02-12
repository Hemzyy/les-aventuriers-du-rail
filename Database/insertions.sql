INSERT INTO Ville (idVille, NomVille) VALUES 
(1, 'Rotonde'),
(2, 'Montagne Verte'),
(3, 'Gare Centrale'),
(4, 'Place de Haguenau'),
(5, 'Petite France'),
(6, 'Etoile-Bourse'),
(7, 'République'),
(8, 'Lycée Kleber'),
(9, 'Landsberg'),
(10, 'Esplanade'),
(11, 'Wagner'),
(12, 'Carpe Haute'),
(13, 'Citadelle'),
(14, 'Bassin du Commerce'),
(15, 'Arcelor Mittal'),
(16, 'Kehl');



INSERT INTO Couleur (idClr, nomClr) VALUES 
(1, 'rouge'),
(2, 'bleu'),
(3, 'vert'),
(4, 'jaune'),
(5, 'rose'),
(6, 'blanc'),
(7, 'multicolore');


INSERT INTO Rails (idRails, idVille1, idVille2, idClr, nbrDeRail) VALUES 
(1, 1, 2, 2, 3),
(2, 1, 3, 4, 2),
(3, 1, 3, 5, 2),
(4, 1, 4, 1, 2),
(5, 2, 3, 3, 2),
(6, 2, 3, 6, 2),
(7, 2, 5, 6, 3),
(8, 2, 6, 4, 3),
(9, 3, 4, 6, 2),
(10, 3, 5, 1, 1),
(11, 4, 8, 2, 2),
(12, 4, 7, 4, 2),
(13, 4, 7, 5, 2),
(14, 5, 7, 2, 1),
(15, 5, 6, 5, 1),
(16, 6, 10, 3, 2),
(17, 6, 10, 2, 2),
(18, 6, 9, 1, 1),
(19, 7, 8, 6, 1),
(20, 7, 10, 1, 2),
(21, 7, 10, 6, 2),
(22, 8, 12, 3, 3),
(23, 8, 11, 5, 2),
(24, 9, 10, 6, 2),
(25, 9, 13, 5, 2),
(26, 9, 13, 3, 2),
(27, 10, 11, 6, 1),
(28, 10, 13, 4, 2),
(29, 10, 14, 5, 3),
(30, 11, 12, 1, 1),
(31, 11, 14, 6, 2),
(32, 11, 14, 2, 2),
(33, 12, 15, 4, 2),
(34, 13, 16, 1, 3),
(35, 14, 15, 1, 1),
(36, 14, 16, 4, 2),
(37, 15, 16, 3, 2),
(38, 15, 16, 2, 2);



INSERT INTO cartesWagon (idCarte, idClr) VALUES 
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5),
(6, 6),
(7, 7),
(8, 8);


INSERT INTO cartesMission (idMission, idVille1, idVille2, nbrDePoints) VALUES
(1, 4, 8, 2),
(2, 3, 2, 2),

(3, 1, 2, 3),
(4, 4, 5, 3),
(5, 10, 14, 3),
(6, 10, 5, 3),
(7, 7, 11, 3),
(8, 7, 9, 3),
(9, 6, 11, 3),

(10, 15, 10, 4),
(11, 7, 13, 4),

(12, 15, 13, 5),
(13, 9, 14, 5),

(14, 8, 16, 6),
(15, 1, 10, 6),
(16, 16, 6, 6),
(17, 3, 12, 6);