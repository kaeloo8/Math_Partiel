#include "pch.h"
#include "main.h"
#include "Affichage.h"
#include <SFML/Graphics.hpp>
#include <cmath>

std::vector<std::vector<float>> Lagrange(std::vector<std::vector<float>> points, int NombreDePoint) {
    std::vector<std::vector<float>> result;
    int n = points.size();  //recup la taille de Point 

    //interpolation pour le NombreDePoint min et max de x
    float minX = points[0][0], maxX = points[0][0];
    for (const auto& point : points) { // verif dans tout les point pour obtenir le plus petit x
        if (point[0] < minX) minX = point[0];
        if (point[0] > maxX) maxX = point[0];
    }

    float step = (maxX - minX) / (NombreDePoint - 1); // obtien la distance entre chaque point qui sera genere

    //calcule les point de la courbe
    for (int i = 0; i < NombreDePoint; ++i) {
        float x = minX + i * step;
        float y = 0;

        // polinome de Lagrange en x
        for (int j = 0; j < n; ++j) {
            float term = points[j][1]; // recup le y
            for (int k = 0; k < n; ++k) {
                if (k != j) {
                    term *= (x - points[k][0]) / (points[j][0] - points[k][0]);
                }
            }
            y += term;
        }

        result.push_back({ x, y });
    }

    return result;
}

std::vector<std::vector<float>> Hermite(std::vector<std::vector<float>> points, std::vector<std::vector<float>> derivatives, int NombreDePoint) {
    std::vector<std::vector<float>> result;
    int n = points.size(); 

    if (points.size() != derivatives.size()) {
        std::cerr << "Le nombre de points et le nombre de dérivées ne correspondent pas." << std::endl;
        return result;
    }

    float minX = points[0][0], maxX = points[0][0];
    for (const auto& point : points) {
        if (point[0] < minX) minX = point[0];
        if (point[0] > maxX) maxX = point[0];
    }

    if (maxX == minX) {
        std::cerr << "Erreur : maxX et minX sont égaux, division par zéro impossible." << std::endl;
        return result;
    }

    float step = (maxX - minX) / (NombreDePoint - 1);

    for (int i = 0; i < NombreDePoint; ++i) {
        float x = minX + i * step;
        float y = 0;
        float dy = 0;

        for (int j = 0; j < n; ++j) {
            float dx = x - points[j][0];
            float dx2 = dx * dx;
            float dx3 = dx2 * dx;
            float denom2 = std::pow(maxX - minX, 2);
            float denom3 = std::pow(maxX - minX, 3);

            float h1 = 2 * dx3 / denom3 - 3 * dx2 / denom2 + 1;
            float h2 = dx3 / denom3 - 2 * dx2 / denom2 + dx / denom2;

            y += h1 * points[j][1] + h2 * derivatives[j][1];
        }

        result.push_back({ x, y });
    }

    return result;
}



int main()
{
    std::vector<std::vector<float>> liste = {
        {0,3},
        {-1,-1},
        {4,-2},
    };

    std::vector<std::vector<float>> points = {
    {0, 0},    // (x0, y0)
    {1, 2},    // (x1, y1)
    {2, 0},    // (x2, y2)
    };

    std::vector<std::vector<float>> derivatives = {
        {0, 1},    // Dérivée en (x0, y0)
        {1, -1},   // Dérivée en (x1, y1)
        {2, 1},    // Dérivée en (x2, y2)
    };

    Affichage affichage;

    while (affichage.Win.isOpen())
    {

        //affichage.add(Hermite(points, derivatives,250));
        affichage.addR(Lagrange(liste, 250));
        affichage.add(Lagrange(liste, 250));
        affichage.Update();
    }

    return 0;
}