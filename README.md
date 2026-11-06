# Hangman - Pendu Girafe

Jeu du pendu (Hangman) en C# WPF, réalisé pour le TP2 de BTS CIEL 1.

**Auteur :** Tedy Barthod

## Le jeu

Le joueur doit trouver un mot caché en proposant des lettres.

- Si la lettre est dans le mot, elle s'affiche à sa place.
- Si la lettre n'est pas dans le mot, le joueur perd une vie (une feuille d'acacia).
- Le joueur a 7 vies.
- Il gagne quand toutes les lettres du mot sont trouvées.
- Il perd quand il n'a plus de vies.

À la place du pendu classique, une girafe en peluche se construit sous un acacia à chaque erreur.

## Outils

- Visual Studio 2022
- C# / WPF (.NET Framework 4.7.2)

## Lancer le projet

1. Ouvrir `Hangman_Barthod_tedy.slnx` avec Visual Studio 2022.
2. Faire **Générer → Régénérer la solution**.
3. Lancer avec **F5**.

## Organisation des fichiers

| Fichier / dossier | Rôle |
|---|---|
| `MainWindow.xaml` | L'interface : titre, image de la girafe, mot, lettres, vies |
| `MainWindow.xaml.cs` | Le code du jeu |
| `Assete/Images/` | Les images du jeu |

## Les images

| Image | Utilisation |
|---|---|
| `fond_savane.png` | Fond de la fenêtre |
| `pendu_1.png` | Début de la partie : l'acacia seul |
| `pendu_2.png` à `pendu_7.png` | La girafe se construit à chaque erreur |
| `pendu_8.png` | Partie perdue : la girafe est triste |
| `feuille_pleine.png` | Une vie restante |
| `feuille_vide.png` | Une vie perdue |
| `vies_0.png` à `vies_7.png` | Bande de 7 feuilles (le chiffre = nombre de vies) |

Les images sont réglées en **Action de génération : Ressource**.

## Avancement

- [x] Interface (XAML) : fond, girafe, mot, lettres de A à Z, vies
- [x] Style des boutons
- [ ] Code du jeu (C#)
