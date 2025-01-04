#include "pch.h"
#include "main.h"
#include "Affichage.h"
#include <SFML/Graphics.hpp>
#include <cmath>

// fonction qui calcule le cos d'un angle
// utilise la serie de taylor pour faire une approximation
void Cos(float angle, float& cosValue) {
    cosValue = 1.0f; // premier terme de la série
    float term = 1.0f; // def la valeur initiale de chaque terme
    for (int i = 1; i < 10; ++i) { // on prend 10 termes pour plus de précison
        term *= -angle * angle / (2 * i * (2 * i - 1)); // on calcule chaque terme de la série
        cosValue += term; // on ajoute chaque terme au résultat total
    }
}


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// fonction qui calcule le sinus d'un angle
// pareil que pour le cos utilisation de taylor
void Sin(float angle, float& sinValue) {
    sinValue = angle; // premier terme de la série
    float term = angle; // def la valeur initiale de chaque terme
    for (int i = 1; i < 10; ++i) { // encore une fois, 10 termes pour plus de précision
        term *= -angle * angle / ((2 * i) * (2 * i + 1)); // calcul du terme suivant
        sinValue += term; // on ajoute ce terme au resultat final
    }
}


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// dessine une ellipse avec un centre (xC, yC) et les demi-axes (a, b)
void Ellipse(float xC, float yC, float a, float b, Affichage& affichage) {
    std::vector<std::vector<float>> points; // liste pour stocker les points de l'ellipse
    int numPoints = 250; // on veut 250 points pour dessiner l'ellipse

    // on fait varier t de 0 à 250 
    for (int t = 0; t < numPoints; ++t) {
        // calcul de l'angle 
        float angle = t * 2 * 3.14159f / numPoints;

        // approximation des valeurs de cos(angle) et sin(angle) avec les fonctions définies
        float cosValue;
        float sinValue;
        Cos(angle, cosValue); // ici on appelle la fonction Cos pour obtenir cos(angle)
        Sin(angle, sinValue); // ici on appelle la fonction Sin pour obtenir sin(angle)

        // calcul des coordonnées (x, y)
        float x = xC + a * cosValue; // pour x, on applique l'approximation de cos
        float y = yC + b * sinValue; // pour y, on applique l'approximation de sin

        // on ajoute ce point à la liste des points
        points.push_back({ x, y });
    }

    // on affiche tous les points de l'ellipse
    affichage.add(points);
}


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Fonction qui réalise l'interpolation de Lagrange sur un ensemble de points
std::vector<std::vector<float>> Lagrange(std::vector<std::vector<float>> points, int NombreDePoint) {
    std::vector<std::vector<float>> result; // liste des points calculés par Lagrange
    int n = points.size();  // on récupère la taille de la liste de points

    // interpolation pour trouver le min et max de x
    float minX = points[0][0], maxX = points[0][0]; // on initialise min et max avec le premier point
    for (const auto& point : points) { // on regarde tous les points pour trouver le min et max
        if (point[0] < minX) minX = point[0]; // on cherche le min
        if (point[0] > maxX) maxX = point[0]; // et le max
    }

    // on calcule la distance entre chaque point
    float step = (maxX - minX) / (NombreDePoint - 1); // combien de distance entre chaque point qu'on va calculer

    // on calcule les points de la courbe
    for (int i = 0; i < NombreDePoint; ++i) {
        float x = minX + i * step; // on calcule x pour chaque point
        float y = 0; // on initialise y à 0, on va le calculer

        // polynôme de Lagrange pour trouver y pour chaque x
        for (int j = 0; j < n; ++j) {
            float term = points[j][1]; // on prend y du point j
            for (int k = 0; k < n; ++k) {
                if (k != j) { // on calcule le produit pour chaque terme de Lagrange
                    term *= (x - points[k][0]) / (points[j][0] - points[k][0]);
                }
            }
            y += term; // on ajoute le terme au résultat final de y
        }

        // on ajoute le point (x, y) dans la liste des résultats
        result.push_back({ x, y });
    }

    return result; // on retourne la liste de points calculés
}


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Fonction pour l'interpolation de Hermite
std::vector<std::vector<float>> Hermite(std::vector<std::vector<float>> points, std::vector<std::vector<float>> derivatives, int NombreDePoint) {
    std::vector<std::vector<float>> result; // Liste qui va contenir les points calculés
    int n = points.size();  // On récupère la taille de la liste des points (nombre de points)

    // On vérifie si le nombre de points correspond au nombre de dérivées
    if (points.size() != derivatives.size()) {
        std::cerr << "Le nombre de points et le nombre de dérivées ne correspondent pas." << std::endl;
        return result;  // Si ce n'est pas le cas, on affiche une erreur et on retourne une liste vide
    }

    // Calcul des valeurs minimales et maximales de X dans la liste des points
    float minX = points[0][0], maxX = points[0][0]; // Initialisation
    for (const auto& point : points) { // On parcourt tous les points pour obtenir min et max
        if (point[0] < minX) minX = point[0];
        if (point[0] > maxX) maxX = point[0];
    }

    // On vérifie que maxX et minX sont différents, car une division par zéro est impossible
    if (maxX == minX) {
        std::cerr << "Erreur : maxX et minX sont égaux, division par zéro impossible." << std::endl;
        return result; // Si les valeurs sont égales, on retourne une liste vide
    }

    // On calcule l'écart entre chaque point qu'on va générer
    float step = (maxX - minX) / (NombreDePoint - 1); // Espacement entre les points

    // On commence à calculer les points de la courbe
    for (int i = 0; i < NombreDePoint; ++i) { // Pour chaque point que l'on veut calculer
        float x = minX + i * step; // Calcul de x
        float y = 0;  // La valeur de y sera calculée pour chaque x
        float dy = 0; // Dérivée en y (mais non utilisée directement dans ce calcul)

        // Calcul de l'interpolation de Hermite pour chaque x
        for (int j = 0; j < n; ++j) {  // Pour chaque point de données
            float dx = x - points[j][0];  // Différence entre x et le x du point
            float dx2 = dx * dx;  // Carré de dx
            float dx3 = dx2 * dx;  // Cube de dx

            // Calcul des polynômes de Hermite h1 et h2
            float h1 = 2 * dx3 / ((maxX - minX) * (maxX - minX) * (maxX - minX)) - 3 * dx2 / ((maxX - minX) * (maxX - minX)) + 1;
            float h2 = dx3 / ((maxX - minX) * (maxX - minX) * (maxX - minX)) - 2 * dx2 / ((maxX - minX) * (maxX - minX)) + dx / (maxX - minX);
            float h3 = dx3 / ((maxX - minX) * (maxX - minX) * (maxX - minX)) - dx2 / ((maxX - minX) * (maxX - minX)); // Termes supplémentaires si nécessaire

            // Calcul des termes d'interpolation
            y += h1 * points[j][1] + h2 * derivatives[j][1];
        }

        // Ajout du point calculé à la liste des résultats
        result.push_back({ x, y });
    }

    // Retourner la liste des points calculés
    return result;
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

#include <iostream> // Pour l'entrée de la console

int main()
{
    // liste des points utilisés pour Lagrange et Hermite
    std::vector<std::vector<float>> liste = {};
    std::vector<std::vector<float>> points = {};
    std::vector<std::vector<float>> derivatives = {};

    // création de l'affichage
    Affichage affichage;

    // boucle qui tourne tant que la fenêtre est ouverte
    while (affichage.Win.isOpen())
    {
        // Demander à l'utilisateur quelle partie exécuter
        std::cout << "Choisissez la partie a lancer (1 pour Ellipse, 2 pour Lagrange, 3 pour Hermite, 4 pour quitter) : ";
        int choix;
        std::cin >> choix;
        std::cout << std::endl;

        // Effectuer des actions en fonction du choix
        if (choix == 1) {
            affichage.clear();

            // Partie 1: Dessiner l'ellipse
            std::cout << "Dessiner l'ellipse..." << std::endl;
            Ellipse(0, 0, 3, 1, affichage);
        }
        if (choix == 2) {
            // Partie 2: Calculer et afficher la courbe Lagrange
            // Utile point : 1,5 // 4,2 // 3,-1 // 2,-2 // 0,-1 // -4,-3 // -4,0 // -2,2 // -1,3 // -2,3 // -5,1 // -6,1 // -6,2 // -5,2 // -2,4
            affichage.clear();
            
            points = { {4,2},{3,-1}, {2,-2} };
            affichage.addV(Lagrange(points, 250));

            // Hermite entre 2,-2 et 0,-1

            points = { {0,-1},{-2,-1}, {-4,-3} };
            affichage.addV(Lagrange(points, 250));

            // Hermite entre -4,-3 et -2,2 (problemme lagrange impossible 2 x meme val = /0)
            
            affichage.add({ {-2,3},{-5,1} });

            // Hermite entre -2,2 et -2,3

            // Hermite entre -5,1 et -6,2 (problemme lagrange impossible 2 x meme val = /0)

            // Hermite entre -6,2 et -5,2

            // Hermite entre -5,2 et -2,4

            // Hermite entre -2,4 et 1,5

            // Hermite entre 1,5 et 4,2

        }
        if (choix == 3) {
            affichage.clear();

            // Partie 3: Calculer et afficher la courbe Hermite


        }
        if (choix == 4) {
            // Partie 4: Quitter la boucle (fermer la fenêtre)
            std::cout << "Quitter..." << std::endl;
            affichage.Win.close();
        }
        if (choix != 1 && 2 && 3 && 4) {
            std::cout << "Choix invalide. Veuillez entrer un nombre entre 1 et 4." << std::endl;
        }

        // Mettre à jour l'affichage après chaque action
        affichage.Update();
    }

    return 0; // fin du programme
}

