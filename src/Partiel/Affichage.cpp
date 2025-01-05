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
        ListePoint.push_back({ Nliste[0]+7, -1 * Nliste[1]+7 });
    }
}

void Affichage::addR(std::vector<std::vector<float>> liste) {
    std::vector<std::vector<float>> l;

    int size = liste.size() - 1;
    float m = (liste[size][0] + liste[0][0]) / 2;

    for (std::vector<float> Nliste : liste) {
        std::vector<float> r = { 2 * m - Nliste[0], Nliste[1] };
        l.push_back(r);
    }

    for (std::vector<float> Nliste : l) {
        ListePoint.push_back({ Nliste[0] + 7, -1 * Nliste[1] + 7 });
    }
}

void Affichage::DrawBase() {
    for (int i = 0; i * Scale < Height; i++) {
        float decal = i * Scale;
        
        int size = 2;
        if (i == 7) { size = 4; }

        decal -= size / 2;
        const sf::Vector2f Pos = { 0, decal};

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

        int size = 2;
        if (i == 7) { size = 4; }

        decal -= size / 2;
        const sf::Vector2f Pos = { decal, 0 };

        sf::RectangleShape line(sf::Vector2f(size, Height));
        line.setFillColor(sf::Color::Black);
        line.setPosition(Pos);

        sf::Text text(std::to_string(i-7), font, 15);
        text.setFillColor(sf::Color::Black);
        text.setPosition(decal - 20, Height / 2 - 20);

        Win.draw(line);
        if (i != 7) {
            Win.draw(text);
        }
    }
}

void Affichage::DrawShape() {
    if (ListePoint.size() > 1) {
        
        for (int i = 0; i < ListePoint.size() - 1; i++) {
            const std::vector<float>& Point1 = ListePoint[i];
            const std::vector<float>& Point2 = ListePoint[i + 1];
            float dx = Point2[0] - Point1[0];
            float dy = Point2[1] - Point1[1];
            float length = std::sqrt(dx * dx + dy * dy);
            float angle = std::atan2(dy, dx);

            sf::RectangleShape line(sf::Vector2f(length * Scale, 1));
            line.setOrigin(0, 0.5f);
            line.setPosition(Point1[0] * Scale, Point1[1] * Scale);
            line.setRotation(angle * 180.f / 3.14159f);
            line.setFillColor(sf::Color::Red);

            Win.draw(line);
        }
    }
}

void  Affichage::clear() {
    ListePoint.clear();
}

void Affichage::addV(std::vector<std::vector<float>> liste) {
    for (int i = 0; i < liste.size(); i++) {
        std::vector<float> Nliste = liste[liste.size() - 1 - i];
        ListePoint.push_back({ Nliste[0] + 7, -1 * Nliste[1] + 7 });
    }
    // Ajouter un point séparateur après la courbe
    ListePoint.push_back({ std::numeric_limits<float>::quiet_NaN(), std::numeric_limits<float>::quiet_NaN() });
}

void Affichage::addY(std::vector<std::vector<float>> liste) {
    for (std::vector<float> Nliste : liste) {
        ListePoint.push_back({ Nliste[1] + 7, -1 * Nliste[0] + 7 });
    }
}
