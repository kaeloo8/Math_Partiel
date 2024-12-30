#include "pch.h"
#include "main.h"
#include "Affichage.h"
#include <SFML/Graphics.hpp>

int main()
{
    Affichage affichage;

    while (affichage.Win.isOpen())
    {
        affichage.Update();
    }

    return 0;
}