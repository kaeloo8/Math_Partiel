#include "pch.h"
#include "main.h"
#include "Affichage.h"
#include <SFML/Graphics.hpp>

int main()
{
    std::vector<std::vector<float>> liste = {
        {-1,2},
        {2,3},
        {4,1},
        {0,-2}
    };
    Affichage affichage;

    while (affichage.Win.isOpen())
    {

        affichage.add(liste);
        affichage.Update();
    }

    return 0;
}