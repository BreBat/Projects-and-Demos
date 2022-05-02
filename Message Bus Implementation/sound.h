#include <ofMain.h>
#include "dataBusSender.h"

#pragma once
class sound
{
private:
	ofVec2f pos;
	string type;
	dataBusSender emitter;
	int lifespan;
	ofImage sprite;
	bool played;
	bool active;

public:
	sound(string _type, ofVec2f start);
	~sound();

	void setBus(dataBus* bus);
	void decay();
	void draw();
};

