#pragma once
#include <SFML/Window.hpp>
#include <SFML/Graphics.hpp>
#include <iostream>
#include <vector>

class Affichage
{
public:
    sf::RenderWindow Win;
    std::vector<std::vector<float>> ListePoint;
    int Width;
    int Height;
    int Scale;
    sf::Font font;

    Affichage();

    void Update();
    void add(std::vector<std::vector<float>> l);
    void DrawBase();
    void DrawShape();
};