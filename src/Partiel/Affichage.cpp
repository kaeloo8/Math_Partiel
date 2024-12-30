#include "pch.h"
#include "Affichage.h"

Affichage::Affichage(int scale) : Scale(scale) {
    Width = 200;
    Height = 200;
    Win.create(sf::VideoMode(Width, Height), "SFML works!");
}

void Affichage::Update() {
    
    sf::Event event;
    while (Win.pollEvent(event)) {
        if (event.type == sf::Event::Closed) {
            Win.close();
        }
    }

    Win.clear();
    
    Win.display();
}

void Affichage::add(std::vector< std::vector<int>> liste) {
    for (std::vector<int> Nliste : liste) {
        ListePoint.push_back(Nliste);
    }
}