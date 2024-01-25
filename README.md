# Bienvenue sur notre jeu AMJV

Ce projet a été fait par Aymeric Lacroix et Jeremy Lenoir.

Il s'agit d'un RTS et l'objectif est de capturer le drapeau ou de le défendre.

Pour cela, les attaquants disposent des unités suivantes:
    - Paladin
    - Sorcier
    - Prêtre
    - Assassin
    - Paysan

Quant aux défenseurs, voici leurs unités :
    - Squelette
    - Nécromancien

Chacune des unités possèdent des attaquent spéciales:
Paladin -> Boost d'armure de l'unité et des unités autours
Sorcier -> AOE qui fait des dégats
Prêtre -> Soigne les unités autours
Assassin -> Augmentation des dégats
Paysan -> Pose des pièges
Squelette -> Rien
Nécromancien -> Fait apparaitre des squelettes

Voici les controles pour jouer au jeu:
    Tout le temps :
        Clique gauche pour selectionner une unité
        Maintenir clique gauche pour selectionner plusieurs unités
    Lorsqu'une/plusieurs unités sont sélectionnées :
        Clique droit pour envoyer une unité (en cliquant sur le terrain)
        Clique droit pour attaquer une unité (en cliquant sur un ennemi)
        La touche R pour activer les attaques spéciales
    Code de triche:
        Touche espace pour tuer toutes les unités dans le state Attack (unité joueur et ennemi)

Voici la liste des bugs:
    - Les unités peuvent se trouver sur la même case à l'arrêt
    - Le controle du son ne fonctionne pas
    - Les unités du joueur peuvent refuser d'attaquer et aller à des endroits aléatoires sur la map
