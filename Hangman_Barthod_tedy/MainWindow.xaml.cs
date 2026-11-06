// Projet : Hangman (jeu du pendu) - TP2 BTS CIEL 1
// Auteur : Tedy Barthod
// Rôle : gère le jeu (choix du mot, lettres, erreurs, gagné / perdu)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hangman_Barthod_tedy
{
    public partial class MainWindow : Window
    {
        // Liste des mots possibles (en majuscules, sans accents)
        string[] mots = { "GIRAFE", "LION", "ZEBRE", "ELEPHANT", "GAZELLE", "HYENE", "SAVANE", "BAOBAB" };

        // Sert à tirer un nombre au hasard
        Random hasard = new Random();

        string mot = "";          // le mot à trouver
        string motAffiche = "";   // le mot avec des _ pour les lettres pas trouvées
        int erreurs = 0;          // nombre d'erreurs
        int erreursMax = 7;       // nombre d'erreurs avant de perdre
        bool partieFinie = false; // true quand on a gagné ou perdu

        Button[] boutons;  // les 26 boutons lettres
        Image[] feuilles;  // les 7 feuilles des essais

        public MainWindow()
        {
            InitializeComponent();

            // On range les boutons et les feuilles dans des tableaux
            boutons = new Button[] { BTN_A, BTN_B, BTN_C, BTN_D, BTN_E, BTN_F, BTN_G, BTN_H, BTN_I,
                                     BTN_J, BTN_K, BTN_L, BTN_M, BTN_N, BTN_O, BTN_P, BTN_Q, BTN_R,
                                     BTN_S, BTN_T, BTN_U, BTN_V, BTN_W, BTN_X, BTN_Y, BTN_Z };
            feuilles = new Image[] { Feuille1, Feuille2, Feuille3, Feuille4, Feuille5, Feuille6, Feuille7 };

            Recommencer();
        }

        // Remet tout à zéro et lance une nouvelle partie
        private void Recommencer()
        {
            erreurs = 0;
            partieFinie = false;
            ChoisirMot();
            ActiverLettres();
            AfficherMot();
            AfficherEssais();
            AfficherPendu();
        }

        // Tire un mot au hasard et prépare le mot caché
        private void ChoisirMot()
        {
            mot = mots[hasard.Next(mots.Length)];

            motAffiche = "";
            for (int i = 0; i < mot.Length; i++)
            {
                motAffiche = motAffiche + "_";
            }
        }

        // Réactive les 26 boutons et enlève leur couleur
        private void ActiverLettres()
        {
            for (int i = 0; i < boutons.Length; i++)
            {
                boutons[i].IsEnabled = true;
                boutons[i].Tag = "";
            }
        }

        // Regarde si la lettre est dans le mot et la dévoile
        // Renvoie true si la lettre est trouvée
        private bool VerifierLettre(char lettre)
        {
            bool trouvee = false;
            string nouveau = "";

            for (int i = 0; i < mot.Length; i++)
            {
                if (mot[i] == lettre)
                {
                    nouveau = nouveau + lettre;
                    trouvee = true;
                }
                else
                {
                    nouveau = nouveau + motAffiche[i];
                }
            }

            motAffiche = nouveau;
            return trouvee;
        }

        // Affiche le mot caché avec un espace entre chaque lettre
        private void AfficherMot()
        {
            string texte = "";
            for (int i = 0; i < motAffiche.Length; i++)
            {
                texte = texte + motAffiche[i] + " ";
            }
            TB_Mot.Text = texte;
        }

        // Affiche les essais restants et pâlit les feuilles perdues
        private void AfficherEssais()
        {
            int restants = erreursMax - erreurs;
            TB_Essais.Text = "Essais restants : " + restants.ToString();

            for (int i = 0; i < feuilles.Length; i++)
            {
                if (i < restants)
                {
                    feuilles[i].Opacity = 1.0;
                }
                else
                {
                    feuilles[i].Opacity = 0.2;
                }
            }
        }

        // Change l'image de la girafe selon le nombre d'erreurs
        private void AfficherPendu()
        {
            int numero = erreurs + 1;
            string chemin = "Assete/Images/pendu_" + numero.ToString() + ".png";
            Img_Pendu.Source = new BitmapImage(new Uri(chemin, UriKind.Relative));
        }

        // Renvoie true si toutes les lettres sont trouvées
        private bool Gagne()
        {
            return motAffiche == mot;
        }

        // Renvoie true si le joueur a fait trop d'erreurs
        private bool Perdu()
        {
            return erreurs >= erreursMax;
        }

        // Clic sur une lettre (les 26 boutons appellent cette méthode)
        private void BTN_Lettre_Click(object sender, RoutedEventArgs e)
        {
            // Si la partie est finie, on ne fait rien
            if (partieFinie)
            {
                return;
            }

            Button btn = sender as Button;
            string btnContent = btn.Content.ToString();
            btn.IsEnabled = false;

            // On prend la lettre du bouton (ex : "A" donne 'A')
            char lettre = btnContent[0];

            if (VerifierLettre(lettre))
            {
                btn.Tag = "ok";
            }
            else
            {
                btn.Tag = "ko";
                erreurs = erreurs + 1;
            }

            AfficherMot();
            AfficherEssais();
            AfficherPendu();

            if (Gagne())
            {
                partieFinie = true;
                TB_Essais.Text = "Gagné ! Bravo !";
            }
            else if (Perdu())
            {
                partieFinie = true;
                TB_Essais.Text = "Perdu ! Le mot était " + mot;
            }
        }

        // Clic sur Nouveau mot
        private void BTN_NouveauMot_Click(object sender, RoutedEventArgs e)
        {
            Recommencer();
        }
    }
}
