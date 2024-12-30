#include "pch.h"
#include "Affichage.h"

Affichage::Affichage() {
    Width = 700;
    Height = 700;
    Scale = 700 / 14;
    Win.create(sf::VideoMode(Width, Height), "SFML works!");

    if (!font.loadFromFile("../../../src/Hack-Regular.ttf")) {
        std::cerr << "Erreur de chargement de la police !" << std::endl;
    }
}

void Affichage::Update() {
    sf::Event event;
    while (Win.pollEvent(event)) {
        if (event.type == sf::Event::Closed) {
            Win.close();
        }
    }

    Win.clear(sf::Color::White); 
    DrawBase();

    DrawShape();

    Win.display(); 
}

void Affichage::add(std::vector<std::vector<float>> liste) {
    for (std::vector<float> Nliste : liste) {
        ListePoint.push_back({ Nliste[0] + 7,-1*Nliste[1] + 7 });
    }
}

void Affichage::DrawBase() {
    for (int i = 0; i * Scale < Height; i++) {
        float decal = i * Scale;

        const sf::Vector2f Pos = { 0, decal };
        int size = 2;
        if (i == 7) { size = 4; }

        sf::RectangleShape line(sf::Vector2f(Width, size));
        line.setFillColor(sf::Color::Black);
        line.setPosition(Pos);

        
        sf::Text text(std::to_string(-1*i+7), font, 15);
        text.setFillColor(sf::Color::Black);
        text.setPosition(Width / 2 - 20, decal - 20);

        Win.draw(line);
        Win.draw(text);
    }

    for (int i = 0; i * Scale < Width; i++) {
        float decal = i * Scale;

        const sf::Vector2f Pos = { decal, 0 };
        int size = 2;
        if (i == 7) { size = 4; }

        sf::RectangleShape line(sf::Vector2f(size, Height));
        line.setFillColor(sf::Color::Black);
        line.setPosition(Pos);

        sf::Text text(std::to_string(i-7), font, 15);
        text.setFillColor(sf::Color::Black);
        text.setPosition(decal - 20, Height / 2 - 20);

        Win.draw(line);
        Win.draw(text);
    }
}

void Affichage::DrawShape() {
    if (LasteListePoint.empty() && ListePoint.size() > 0) {
        LasteListePoint= ListePoint[0];
    }

    for (size_t i = 0; i < ListePoint.size(); i++) {
        const std::vector<float>& Point = ListePoint[i];

        if (i > 0) {
            const std::vector<float>& LastPoint = ListePoint[i - 1];

            sf::Vertex line[] = {
                sf::Vertex(sf::Vector2f(LastPoint[0] * Scale, LastPoint[1] * Scale), sf::Color::Red),
                sf::Vertex(sf::Vector2f(Point[0] * Scale, Point[1] * Scale), sf::Color::Red)
            };

            Win.draw(line, 2, sf::Lines);
        }

        LasteListePoint = Point;
    }
}