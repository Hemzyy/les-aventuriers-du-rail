--This view provides an overview of all player statistics, combining data from the StatJoueur table with the player information from the Joueur table.--

CREATE VIEW OverallPlayerStatsView AS
SELECT j.idJoueur, j.pseudo, sj.highScore, sj.PartiesJouées, sj.partiesGagnées
FROM Joueur j
INNER JOIN StatJoueur sj ON j.idJoueur = sj.idStatJoueur;
GO

--top players by high score
CREATE VIEW TopPlayersView AS
SELECT j.idJoueur, j.pseudo, sj.highScore, sj.PartiesJouées, sj.partiesGagnées
FROM Joueur j
INNER JOIN StatJoueur sj ON j.idJoueur = sj.idStatJoueur
ORDER BY sj.highScore DESC;
GO
