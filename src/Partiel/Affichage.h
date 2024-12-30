#pragma once
#include <SFML/Window.hpp>
#include <SFML/Graphics.hpp>

class Affichage
{
public:
    sf::RenderWindow Win;
    std::vector<std::vector<int>> ListePoint;
    int Width;
    int Height;
    int Scale;

    Affichage(int scale);

    void Update();
    void add(std::vector<std::vector<int>> l);
};