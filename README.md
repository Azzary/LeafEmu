# LeafEmu - Émulateur Dofus 1.29

## Introduction

LeafEmu est un émulateur open-source pour Dofus Retro, écrit en C#.
Ce projet n'est plus en développement et représente l'un de mes premiers projets. Par conséquent, le code n'est pas propre et pas optimisé.

## Fonctionnalités

### database
- mysql mais le code fait peur.

### Authentification (Auth)

- Connexion au jeu
- File d'attente
- Gestion des mots de passe et des bans
- Affichage des serveurs
- Affichage du nombre de personnages
- Connexion au serveur de jeu

### Monde (World)

#### Carte (Map)
- Affichage des cartes
- Déplacements sur les cartes
- Téléportations sur les cartes
- Gestion des autres personnages sur la carte
- Affichage des combats sur la carte
- Gestion des monstre sur la map
- Gestion des quetes

#### Chat
- Gestion des différents chats
- Affichage des messages

#### Personnage
- Création / suppression / sélection des personnages
- Gestion des statistiques (vie, force, intelligence, agilité, sagesse)
- Carte de départ pour toutes les classes

#### Objets (Items)
- Objets dans l'inventaire
- Sauvegarde de l'inventaire
- Équipement et déséquipement des objets
- Gestion des statistiques des objets équipés (partiel)
- Affichage des équipements sur la carte et en combat

#### Sorts
- Ajout des sorts en fonction des classes et du niveau
- Augmentation du niveau des sorts

#### Caractéristiques
- Gestion des caractéristiques
- Augmentation des caractéristiques

#### Combat (Fight)
- Demande de duel
- Acceptation ou refus d'un duel
- Gestion de l'entrée en combat
- Rejoindre un duel
- Gestion du tour en combat (passer son tour, temps, etc.)
- Lancement d'un combat contre les mobs
- Fin de combat (xp,....)

### Sort
- Gestion de pas mal d'effet
- Pareil pour les buffs

## To-Do

- Gestion de la position des sorts (à ajouter dans la base de données)
- Ajout du multi-serveur (nécessite 2/3 mises à jour)
- Ajouter des fonctionnalités pour les PNJ

