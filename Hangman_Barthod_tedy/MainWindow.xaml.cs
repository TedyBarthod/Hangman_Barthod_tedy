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
using System.Windows.Threading;
using System.IO;

namespace Hangman_Barthod_tedy
{
    public partial class MainWindow : Window
    {
        // Objet qui contient la liste des mots (classe ListeMots, en bas du fichier)
        ListeMots listeMots = new ListeMots();

        string mot = "";          // le mot à trouver
        string motAffiche = "";   // le mot avec des _ pour les lettres pas trouvées
        int erreurs = 0;          // nombre d'erreurs
        int erreursMax = 7;       // nombre d'erreurs avant de perdre
        bool partieFinie = false; // true quand on a gagné ou perdu

        DispatcherTimer timer;    // le chrono, il "tique" toutes les secondes
        int temps = 60;           // secondes qui restent

        Button[] boutons;  // les 26 boutons lettres
        Image[] feuilles;  // les 7 feuilles des essais

        // Lecteur pour la musique du jeu
        MediaPlayer musique = new MediaPlayer();
        string musiqueCalme = "Assete/Musique/calme.mp3";
        string musiqueStress = "Assete/Musique/stress.mp3";
        string musiqueVictoire = "Assete/Musique/victoire.mp3";
        string musiqueDefaite = "Assete/Musique/defaite.mp3";
        string musiqueActuelle = "";      // la musique qui joue en ce moment

        public MainWindow()
        {
            InitializeComponent();

            // On range les boutons et les feuilles dans des tableaux
            boutons = new Button[] { BTN_A, BTN_B, BTN_C, BTN_D, BTN_E, BTN_F, BTN_G, BTN_H, BTN_I,
                                     BTN_J, BTN_K, BTN_L, BTN_M, BTN_N, BTN_O, BTN_P, BTN_Q, BTN_R,
                                     BTN_S, BTN_T, BTN_U, BTN_V, BTN_W, BTN_X, BTN_Y, BTN_Z };
            feuilles = new Image[] { Feuille1, Feuille2, Feuille3, Feuille4, Feuille5, Feuille6, Feuille7 };

            // On crée le chrono : il appelle timer_tick toutes les 1 seconde
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += new EventHandler(timer_tick);

            // Quand une musique est finie, on appelle MusiqueFinie
            musique.MediaEnded += new EventHandler(MusiqueFinie);

            JouerMusique(musiqueCalme);
            Recommencer();
        }

        // Charge le fichier mp3 donné et lance la musique
        private void JouerMusique(string fichier)
        {
            Uri chemin = new Uri(fichier, UriKind.Relative);
            musique.Open(chemin);
            musique.Play();
            musiqueActuelle = fichier;
        }

        // Appelée quand la musique arrive à la fin : on la relance
        private void MusiqueFinie(object sender, EventArgs e)
        {
            // Pas de boucle pour la musique de victoire ou de défaite
            if (!partieFinie)
            {
                JouerMusique(musiqueActuelle);
            }
        }

        // Quand il reste 15 secondes ou moins, on passe à la musique stressante
        private void VerifierMusique()
        {
            if (temps <= 15 && musiqueActuelle == musiqueCalme && !partieFinie)
            {
                JouerMusique(musiqueStress);
            }
        }

        // Remet tout à zéro et lance une nouvelle partie
        private void Recommencer()
        {
            erreurs = 0;
            temps = 60;
            partieFinie = false;

            // Si une autre musique jouait, on remet la musique calme
            if (musiqueActuelle != musiqueCalme)
            {
                JouerMusique(musiqueCalme);
            }

            ChoisirMot();
            ActiverLettres();
            AfficherMot();
            AfficherEssais();
            AfficherPendu();
            AfficherTemps();
            timer.Start();
        }

        // Demande un mot à l'objet listeMots et prépare le mot caché
        private void ChoisirMot()
        {
            mot = listeMots.TirerMot();

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

        // Affiche le temps qui reste
        private void AfficherTemps()
        {
            TB_Temps.Text = "Temps : " + temps.ToString() + " s";
        }

        // Renvoie true si toutes les lettres sont trouvées
        private bool Gagne()
        {
            return motAffiche == mot;
        }

        // Renvoie true si trop d'erreurs ou plus de temps
        private bool Perdu()
        {
            return erreurs >= erreursMax || temps <= 0;
        }

        // Regarde si la partie est finie (gagné ou perdu) et arrête le chrono
        private void VerifierFin()
        {
            if (Gagne())
            {
                partieFinie = true;
                timer.Stop();
                TB_Essais.Text = "Gagné ! Bravo !";
                JouerMusique(musiqueVictoire);
            }
            else if (Perdu())
            {
                partieFinie = true;
                timer.Stop();

                // Même si on perd à cause du temps, on montre la girafe triste
                erreurs = erreursMax;
                AfficherEssais();
                AfficherPendu();

                TB_Essais.Text = "Perdu ! Le mot était " + mot;
                JouerMusique(musiqueDefaite);
            }
        }

        // Appelée par le chrono toutes les secondes
        private void timer_tick(object sender, EventArgs e)
        {
            temps = temps - 1;
            AfficherTemps();
            VerifierFin();
            VerifierMusique();
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
                temps = temps + 3;   // bonne lettre : +3 secondes
            }
            else
            {
                btn.Tag = "ko";
                erreurs = erreurs + 1;
                temps = temps - 5;   // mauvaise lettre : -5 secondes
            }

            // Le temps ne doit pas être négatif
            if (temps < 0)
            {
                temps = 0;
            }

            AfficherMot();
            AfficherEssais();
            AfficherPendu();
            AfficherTemps();
            VerifierFin();
            VerifierMusique();
        }

        // Clic sur Nouveau mot
        private void BTN_NouveauMot_Click(object sender, RoutedEventArgs e)
        {
            Recommencer();
        }
    }

    // Classe qui lit les mots dans le fichier et tire un mot au hasard
    public class ListeMots
    {
        // Attributs
        List<string> mots;   // la liste des mots possibles
        Random hasard;       // sert à tirer un nombre au hasard
        string chemin = @"Assete/motsPendu.txt";   // le fichier des mots

        // Constructeur : il est appelé tout seul quand on fait new ListeMots()
        public ListeMots()
        {
            hasard = new Random();
            mots = new List<string>();
            LireFichier();

            // Si le fichier est vide ou absent, on met un mot de secours
            if (mots.Count == 0)
            {
                mots.Add("GIRAFE");
            }
        }

        // Lit le fichier ligne par ligne et ajoute chaque mot dans la liste
        private void LireFichier()
        {
            // Si le fichier n'existe pas, on ne lit rien
            if (!File.Exists(chemin))
            {
                return;
            }

            using (var monFichier = File.OpenText(chemin))
            {
                string ligne = null;
                do
                {
                    ligne = monFichier.ReadLine();

                    // On ajoute le mot seulement si la ligne n'est pas vide
                    if (ligne != null && ligne != "")
                    {
                        mots.Add(ligne);
                    }
                } while (ligne != null);
            }
        }

        // Renvoie un mot tiré au hasard dans la liste
        public string TirerMot()
        {
            int numero = hasard.Next(mots.Count);
            return mots[numero];
        }
    }
}