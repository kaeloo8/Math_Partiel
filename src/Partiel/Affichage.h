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
    void DrawBase();
    void DrawShape();

    void add(std::vector<std::vector<float>> liste);
    void addR(std::vector<std::vector<float>> liste);
};